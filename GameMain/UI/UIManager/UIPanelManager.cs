using System;
using System.Collections.Generic;
using System.Text;
using Client;
using UnityEngine;

public partial class UIPanelManager :IManager,IGlobalEvent
 {
    #region Property
    public const string CLASS_NAME = "UIPanelManager";
    //活跃面板
    private List<uint> showPanels;
    //所有面板
    private Dictionary<uint, UIPanelBase> allPanels;
    //返回队列
    private Stack<UIPanelBase.PanelReturnData> panelCacheStack;

    //当前活动panel
    public PanelID CurShowCachePanel
    {
        get;
        private set;
    }

    bool isload = false;

    //UIRoot
    public Transform UIRoot
    {
        get
        {
            return UIRootHelper.Instance.UIRoot;
        }
    }
    //拉伸root
    public Transform StretchTransRoot
    {
        get
        {
            return UIRootHelper.Instance.StretchTransRoot;
        }
    }
    //本地面板信息
    private Dictionary<int, LocalPanelInfo> m_dic_localPanelInfo = null;
    #endregion

    #region IManager Method
    public void ClearData()
    {

    }

    private int uiHideLayer = 0;
    public int HideLayer
    {
        get
        {
            return uiHideLayer;
        }
    }
    private int uiShowLayer = 0;
    public int ShowLayer
    {
        get
        {
            return uiShowLayer;
        }
    }
    public void Initialize()
    {
        if (isload)
            return;
        StructLocalPanelInfo();

        showPanels = new List<uint>();
        allPanels = new Dictionary<uint, UIPanelBase>();
        m_lst_curFocusPanels = new List<PanelID>();
        panelCacheStack = new Stack<UIPanelBase.PanelReturnData>();
        //够着UIRoot
        UIRootHelper.Instance.StructRoot(new Vector2(1280,720));
        HomeSceneUIRoot.Instance.Init();
        isload = true;
        RegisterGlobalEvent(true);
        uiHideLayer = LayerMask.NameToLayer("UIHide");
        uiShowLayer = LayerMask.NameToLayer("UI");
    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            ClearCacheStack();
            ReleaseAllPanel();
            showPanels.Clear();
            allPanels.Clear();
            RemoveAllFocusPanel();
            CurShowCachePanel = PanelID.None;
            m_lstWaitingShow.Clear();
        }
    }

    public void Process(float deltaTime)
    {
        
    }
    #endregion

    #region PanelInfo
    /// <summary>
    /// 尝试回去本地面板信息
    /// </summary>
    /// <param name="pID"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool TryGetLocalPanelInfo(PanelID pID, out LocalPanelInfo info)
    {

        return m_dic_localPanelInfo.TryGetValue((int)pID, out info);
    }


    /// <summary>
    /// 构建本地面板数据
    /// </summary>
    private void StructLocalPanelInfo()
    {
        m_dic_localPanelInfo = new Dictionary<int, LocalPanelInfo>();
        List<table.PanelInfoDataBase> panelInfos = GameTableManager.Instance.GetTableList<table.PanelInfoDataBase>();
        if (null != panelInfos)
        {
            LocalPanelInfo localInfo = null;
            int pid = 0;
            for (int i = 0; i < panelInfos.Count; i++)
            {
                localInfo = LocalPanelInfo.Create(panelInfos[i].id);
                if (localInfo.PID == PanelID.None)
                {
                    Engine.Utility.Log.Error("UIPanelManager->StructLocalPanelInfo error tableid = {0}", panelInfos[i].id);
                    continue;
                }
                pid = (int)localInfo.PID;
                if (!m_dic_localPanelInfo.ContainsKey(pid))
                {
                    m_dic_localPanelInfo.Add(pid, localInfo);
                }
            }
        }
    }
    #endregion

    #region ShowPanel
 
    /// <summary>
    /// 根据面板类型获取目标UI根节点
    /// </summary>
    /// <param name="type">面板类型</param>
    /// <returns></returns>
    public Transform GetTargetRootWithType(PanelType pType)
    {
        return UIRootHelper.Instance.GetPanelRootByType(pType);

    }

    /// <summary>
    /// 获取面板
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="panelId"></param>
    /// <returns></returns>
    public T GetPanel<T>(PanelID panelId) where T : UIPanelBase
    {
        UIPanelBase panel = GetPanel(panelId);
        if (null != panel && panel is T)
        {
            return panel as T;
        }
        return null;
    }

    /// <summary>
    /// 获取panel
    /// </summary>
    /// <param name="panelId"></param>
    /// <returns></returns>
    public UIPanelBase GetPanel(PanelID panelId)
    {
        UIPanelBase pb = null;
        if (allPanels.TryGetValue((uint)panelId, out pb))
        {
            return pb;
        }
        return null;
    }


    /// <summary>
    /// 执行panel显示
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="data"></param>
    private void DoShowPanel(UIPanelBase panel, UIPanelBase.PanelData prePanelData = null, object data = null, UIPanelBase.PanelJumpData jumpData = null)
    {
        panel.PrePanelData = prePanelData;
        
        UIFrameManager.Instance.ShowUIFrame(panel);
        panel.ShowPanel(data, jumpData);
        if (panel.PanelInfo.CacheLv != PanelCacheLv.None)
        {
            CurShowCachePanel = panel.PanelId;
        }
    }

    private void ShowPanel(PanelID panelId, UIPanelBase.PanelData prePanelData, bool ignoreCache, object data = null
        , UIPanelBase.PanelJumpData jumpData = null, bool forceClearCacheBack = false, bool forceResetPanel = false, Action<UIPanelBase> panelShowAction = null)
    {
        if (panelId == PanelID.LoadingPanel)
        {
            m_lstWaitingShow.Clear();
        }

        if (IsShowPanel(panelId))
        {
            //UIPanelBase pBase = GetPanel(panelId);
            //pBase.PrePanelData = prePanelData;
            //pBase.ShowPanel(data, jumpData: jumpData);
            //if (null != panelShowAction)
            //{
            //    panelShowAction.Invoke(pBase);
            //}
            Engine.Utility.Log.Warning(CLASS_NAME + "-> ShowPanel warning,panelId = {0} already show!", panelId.ToString());
            //return ;
        }

        //如果存在互斥面板已经显示则直接返回
        //不做互斥处理
        LocalPanelInfo panelInfo = null;
        if (!TryGetLocalPanelInfo(panelId, out panelInfo))
        {
            return;
        }
        if (!IsWaitingShow(panelId))
        {
            PanelShowData showData = new PanelShowData()
            {
                PID = panelId,
                PrePanelData = prePanelData,
                Data = data,
                JumpData = jumpData,
                ForceClearCacheBack = forceClearCacheBack,
                ForceResetPanel = forceResetPanel,
                IgnoreCache = ignoreCache,
                PanelShowAction = panelShowAction,
            };
            m_lstWaitingShow.Add(showData);
        }
        else
        {
            Engine.Utility.Log.Warning(CLASS_NAME + "-> ShowPanel failed,panelId = {0} already in waitingshow Quene !", panelId.ToString());
        }

        //列表容量保护
        if (m_lstWaitingShow.Count >= MAX_WAITING_SHOW_NUM)
        {
            m_lstWaitingShow = new List<PanelShowData>();
            ShowMain();
            return ;
        }

        if (!IsPanelAssetReady(panelInfo))
        {
            ReadyPanelAsset(panelInfo);
        }
        else
        {
            ProccessPanelShowing();
        }
            
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="panelId">面板id</param>
    /// <param name="showData">面板显示数据</param>
    /// <param name="data">面板传递参数</param>
    /// <param name="jumpData">面板跳转参数</param>
    public void ShowPanel(PanelID panelId, UIPanelBase.PanelData prePanelData = null,object data = null, UIPanelBase.PanelJumpData jumpData = null, bool forceClearCacheBack = false, bool forceResetPanel = false,Action<UIPanelBase> panelShowAction = null)
    {
        ShowPanel(panelId, prePanelData, false, data, jumpData, forceClearCacheBack, forceResetPanel, panelShowAction);
    }

    /// <summary>
    /// 获取目标面板类型的最大深度
    /// </summary>
    /// <param name="rootType"></param>
    /// <param name="includeHide"></param>
    /// <returns></returns>
    public int GetTargetRootMaxDepth(PanelRootType rootType, bool includeHide)
    {
        int max = 0;
        Transform targetRoot = UIRootHelper.Instance.GetPanelRoot(rootType);
        int targetStartDepth = UIRootHelper.Instance.GetPanelDepthByType(rootType);
        if (targetRoot != null)
        {
            UIPanel[] panels = targetRoot.GetComponentsInChildren<UIPanel>(includeHide);
            if (panels.Length > 0)
            {
                List<UIPanel> lsPanels = new List<UIPanel>(panels);
                //排序
                lsPanels.Sort(delegate(UIPanel left, UIPanel right)
                {
                    return left.depth - right.depth;
                });
                max = lsPanels[lsPanels.Count - 1].depth;
            }
        }

        return (max == 0) ? targetStartDepth : max;
    }

    /// <summary>
    /// 获取目标面板类型的最大深度
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="includeHide"></param>
    /// <returns></returns>
    public int GetTargetRootMaxDepth(PanelType pType, bool includeHide = false)
    {
        return GetTargetRootMaxDepth(UIRootHelper.Instance.GetRootTypePanelType(pType), includeHide);
    }

    /// <summary>
    /// 获取目标UI当前最大的深度
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="includeHide">是否包含隐藏对象</param>
    /// <returns></returns>
    public int GetTargetRootMaxDepth(UIPanelBase panel, bool includeHide = false)
    {
        return GetTargetRootMaxDepth(panel.PanelType, includeHide);
    }

    /// <summary>
    /// 给目标添加Collider背景
    /// </summary>
    public void AddColliderBgToTarget(UIPanelBase target, string maskName, bool isTransparent)
    {
        // 保证添加的Collider放置在屏幕中间
        Transform mask = target.transform.Find("ColliderMask");
        if (mask == null)
        {
            mask = (new GameObject("ColliderMask")).transform;
            Util.AddChildToTarget(target.transform, mask);
        }

        Transform maskBtn = mask.Find("ColliderMaskBtn");
        if (maskBtn == null)
        {
            UIWidget widget = null;
            widget = NGUITools.AddWidget<UIWidget>(mask.gameObject);
            widget.width = (int)UIRootHelper.Instance.TargetFillSize.x;
            widget.height = (int)UIRootHelper.Instance.TargetFillSize.y;
            widget.gameObject.AddComponent<UIButton>();
            NGUITools.AddWidgetCollider(widget.gameObject);
            widget.name = "ColliderMaskBtn";
            widget.depth = -5;
            maskBtn = widget.transform;
            NGUITools.AddWidgetCollider(maskBtn.gameObject);
            UIEventListener.Get(maskBtn.gameObject).onClick += delegate
            {
                UIPanelBase targetPanelBase = target.GetComponent<UIPanelBase>();
                if (null != targetPanelBase)
                {
                    targetPanelBase.OnColliderMaskClicked();
                }
            };
        }
        //非透明，添加半透明背景
        if (!isTransparent
            && (!IsShowPanel(PanelID.CommonBgPanel) || target.PanelType != PanelType.Normal))
        {
            Transform maskBg = mask.Find("ColliderMaskBg");
            if (null == maskBg)
            {
                UISprite sp = NGUITools.AddSprite(mask.gameObject, null, maskName);
                sp.width = (int)UIRootHelper.Instance.TargetFillSize.x;
                sp.height = (int)UIRootHelper.Instance.TargetFillSize.y;
                sp.alpha = 0.5f;
                sp.depth = -6;
                sp.type = UIBasicSprite.Type.Sliced;
                maskBg = sp.transform;
                maskBg.gameObject.name = "ColliderMaskBg";

                CMResAsynSeedData<CMAtlas> cASD = null;
                UIManager.GetAtlasAsyn(maskName, ref cASD, () =>
                {
                    if (null != sp)
                    {
                        sp.atlas = null;
                    }
                }, sp, false);
            }
        }
    }

    /// <summary>
    /// 添加碰撞器
    /// </summary>
    /// <param name="panel"></param>
    public void AddPanelCollideMask(UIPanelBase panel)
    {
        UIDefine.UIPanelColliderMode colliderMode = panel.UIColliderMode;
        if (colliderMode == UIDefine.UIPanelColliderMode.None)
        {
            return;
        }
        if (colliderMode == UIDefine.UIPanelColliderMode.Normal)
        {
            AddColliderBgToTarget(panel, UIDefine.PANEL_MASK_BG, true);
        }
        else if (colliderMode == UIDefine.UIPanelColliderMode.TransBg)
        {
            AddColliderBgToTarget(panel, UIDefine.PANEL_MASK_BG, false);
        }

    }

    /// <summary>
    /// 调节当前显示面板的深度
    /// </summary>
    /// <param name="panel"></param>
    public void AdjustPanelBaseDepth(UIPanelBase panel)
    {
        if (panel == null)
            return;
        int depth = GetTargetRootMaxDepth(panel);
        panel.UpdatePanelDepth(depth);
    }

    /// <summary>
    /// 获取面板在树形结构中的深度数据
    /// </summary>
    /// <param name="root"></param>
    /// <param name="hierarchydepth"></param>
    /// <param name="data"></param>
    public static void GetPanelHierarchydepth(Transform root, int hierarchydepth, ref List<UIDefine.UIPanelHierarchyData> data)
    {
        if (null == root || root.childCount == 0)
            return;
        hierarchydepth++;
        Transform child = null;
        for (int i = 0; i < root.childCount; i++)
        {
            child = root.GetChild(i);
            if (null == child)
                continue;
            if (null != child.GetComponent<UIPanel>())
            {
                data.Add(new UIDefine.UIPanelHierarchyData()
                {
                    panel = child.GetComponent<UIPanel>(),
                    hierachyDepth = hierarchydepth,
                });
            }
            if (child.childCount != 0)
            {
                GetPanelHierarchydepth(child, hierarchydepth, ref data);
            }
        }
    }

    /// <summary>
    /// 创建面板
    /// </summary>
    /// <param name="pid">面板ID</param>
    /// <param name="panelTs">依赖的Transform</param>
    /// <returns></returns>
    private UIPanelBase CreatePanel(PanelID pid,Transform panelTs)
    {
        UIPanelBase panelBase = GetPanel(pid);
        if (null == panelTs)
        {
            Engine.Utility.Log.Error("UIPanelManager->CreatePanel failed ,Panel Transform null,PID:{0}", pid);
            return panelBase;
        }

        LocalPanelInfo localPanelInfo = null;
        if (TryGetLocalPanelInfo(pid, out localPanelInfo))
        {
            if (null == panelBase)
            {
                string panelPath = localPanelInfo.PrefabPath;
                string panelClassName = localPanelInfo.PanelEnumName;
                GameObject go = panelTs.gameObject;

                if (!string.IsNullOrEmpty(panelClassName))
                {
                    go.name = panelClassName;
                    panelBase = go.GetComponent<UIPanelBase>();
                    if (null == panelBase)
                    {
                        panelBase = Util.AddComponent(go, panelClassName) as UIPanelBase;
                        if (null != panelBase)
                        {
                            panelBase.PanelInfo = localPanelInfo;
                            if (null != go && !go.activeSelf)
                                go.SetActive(true);
                            //初始化
                            panelBase.Init();
                            panelBase.InitPanelDetph();
                            //go.SetActive(false);
                        }
                    }
                    allPanels.Add((uint)pid, panelBase);
                }
            }
            else
            {
                Debug.LogError("DespawnExistpanel:" + pid.ToString());
                if (IsShowPanel(pid))
                {
                    Debug.LogError("DespawnExistSSS##panel:" + pid.ToString());
                }
                UIManager.ReleaseObjs(localPanelInfo.TableData.resID, panelTs);
            }
        }
        
        return panelBase;
    }

    /// <summary>
    /// 面板Transform加载完成回调
    /// </summary>
    /// <param name="panelTs"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    private void OnPanelTransformAsynLoad(Transform panelTs,object param1,object param2,object param3)
    {
        if (null == panelTs)
        {
            Engine.Utility.Log.Error("UIPanelManager->OnPanelTransformAsynLoad null pid ={0}", param1.ToString());
            return;
        }
        
        PanelID pid = (PanelID)param1;
        //移除加载状态
        RemoveLoadingMask(pid);
        UIPanelBase createPanelBase = CreatePanel(pid, panelTs);

        ProccessPanelShowing();
    }



    

    /// <summary>
    /// 准备好面板
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="ingnoreCache"></param>
    /// <returns></returns>
    private UIPanelBase ReadyPanel(UIPanelBase panel, bool ingnoreCache)
    {
        if (null == panel)
            return null;
        //更新导航信息
        UpdateNavigationInfo(panel.PanelId, ingnoreCache);
        //调整panel层级关系
        AdjustPanelBaseDepth(panel);
        //添加碰撞器
        AddPanelCollideMask(panel);

        Transform targetRoot = UIRootHelper.Instance.GetPanelRootByType(panel.PanelInfo.PType);
        panel.CacheTransform.parent = targetRoot;
        panel.CacheTransform.localPosition = Vector3.zero;
        panel.CacheTransform.localScale = Vector3.one;
        return panel;
    }

    delegate bool HideCondition(LocalPanelInfo info);
    /// <summary>
    /// 更新导航信息
    /// </summary>
    /// <param name="panelBase"></param>
    private void UpdateNavigationInfo(PanelID pid, bool ignoreCache)
    {
        LocalPanelInfo localPanelInfo = null;
        if (TryGetLocalPanelInfo(pid,out localPanelInfo))
        {
            if (localPanelInfo.IsStartPanel)
            {
                ClearCacheStack();
            }

            if ((localPanelInfo.HidePanelMask == 0
                && localPanelInfo.CacheLv == PanelCacheLv.None))
            {
                return;
            }

            HideCondition condition = (info) =>
            {
                if (info != null && (localPanelInfo.IsMatchHideType(info.PType)
                    || localPanelInfo.CanCachePanel(info.PID)))
                {
                    return true;
                }
                return false;
            };

            
            List<UIPanelBase.PanelData> cachePanels = new List<UIPanelBase.PanelData>();
            if (showPanels.Count > 0 && condition != null)
            {
                List<PanelID> lstHide = new List<PanelID>();
                
                UIPanelBase.PanelData panelBackspaceData = null;
                UIPanelBase.PanelReturnData lastReturnData = (null != panelCacheStack && panelCacheStack.Count != 0) ? panelCacheStack.Peek() : null;
                bool canCache = (null == lastReturnData || lastReturnData.PID != pid);
                PanelID tempPID = PanelID.None;
                UIPanelBase tempPBase = null;
                LocalPanelInfo tempInfo = null;
                
                for (int i = 0, max = showPanels.Count; i < max; i++)
                {
                    tempPID = (PanelID)showPanels[i];
                    tempPBase = GetPanel(tempPID);
                    if (null == tempPBase)
                        continue;
                    if (TryGetLocalPanelInfo(tempPID,out tempInfo) && condition(tempInfo))
                    {
                        lstHide.Add(tempPID);
                        if (!ignoreCache && canCache && localPanelInfo.CanCachePanel(tempPID))
                        {
                            panelBackspaceData = tempPBase.GetPanelData();
                            if (null == panelBackspaceData.JumpData)
                            {
                                panelBackspaceData.JumpData = new UIPanelBase.PanelJumpData();
                            }
                            panelBackspaceData.JumpData.IsBackspacing = true;
                            cachePanels.Add(panelBackspaceData);
                        }
                    }
                }

                if (cachePanels.Count > 0)
                {
                    cachePanels.Sort((left, right) =>
                    {
                        UIPanelBase pleft = GetPanel(left.PID);
                        UIPanelBase pright = GetPanel(right.PID);
                        return pleft.Depth - pright.Depth;
                    });

                    panelCacheStack.Push(new UIPanelBase.PanelReturnData()
                    {
                        PID = pid,
                        CachePanelList = cachePanels,
                    });
                    
                }

                for (int i = 0; i < lstHide.Count; i++)
                {
                    HidePanel(lstHide[i], false);
                }

            }
        }
    }
    #endregion

    #region Showing & Hiding Panel
    //正在执行隐藏的面板
    private List<PanelID> m_lstHidingPanel = new List<PanelID>();
    //正在执行显示的面板
    private List<PanelShowData> m_lstWaitingShow = new List<PanelShowData>();
    //正在加载的面板
    private List<PanelID> m_lstLoadingPanel = new List<PanelID>();
    public const int MAX_WAITING_SHOW_NUM = 50;
    /// <summary>
    /// 是否正在加载
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    public bool IsLoadingPanel(PanelID pid)
    {
        return m_lstLoadingPanel.Contains(pid);
    }

    /// <summary>
    /// 移除加载状态
    /// </summary>
    /// <param name="pid"></param>
    private void RemoveLoadingMask(PanelID pid)
    {
        m_lstLoadingPanel.Remove(pid);
    }

    /// <summary>
    /// 添加加载状态
    /// </summary>
    /// <param name="pid"></param>
    private void AddLoadingMask(PanelID pid)
    {
        if (!IsLoadingPanel(pid))
        {
            m_lstLoadingPanel.Add(pid);
        }
    }

    /// <summary>
    /// 是否等待显示
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    public bool IsWaitingShow(PanelID pid)
    {
        if (null != m_lstWaitingShow)
        {
            int count = m_lstWaitingShow.Count;
            if (count > 0)
            {
                PanelShowData psd = m_lstWaitingShow[count - 1];
                if (psd.PID == pid)
                {
                    return true;
                }
            }
            
        }
        //var enumerator = m_lstWaitingShow.GetEnumerator();
        //while(enumerator.MoveNext())
        //{
        //    if (enumerator.Current.PID == pid)
        //    {
        //        return true;
        //    }
        //}
        return false;
    }

    /// <summary>
    /// 面板是否显示
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsShowPanel(PanelID id)
    {
        return showPanels.Contains((uint)id);
    }

    /// <summary>
    /// 准备资源
    /// </summary>
    /// <param name="panelInfo"></param>
    private void ReadyPanelAsset(LocalPanelInfo panelInfo)
    {
        if (panelInfo.NeedBg)
        {
            LoadingPanelAsyn(PanelID.CommonBgPanel);
        }
        if (panelInfo.NeedTopBar)
        {
            LoadingPanelAsyn(PanelID.TopBarPanel);
        }
        LoadingPanelAsyn(panelInfo.PID);
    }

    /// <summary>
    /// 异步加载面板
    /// </summary>
    /// <param name="pid"></param>
    /// <param name="panelCreateAction"></param>
    private void LoadingPanelAsyn(PanelID pid)
    {
        LocalPanelInfo panelInfo = null;
        if (TryGetLocalPanelInfo(pid, out panelInfo) && !IsPanelReady(pid) && !IsLoadingPanel(pid))
        {
            AddLoadingMask(pid);
            UIManager.GetObjAsyn(panelInfo.TableData.resID, OnPanelTransformAsynLoad, pid);
        }
    }

    /// <summary>
    /// 是否面板准备好了
    /// </summary>
    /// <param name="pid"></param>
    private bool IsPanelReady(PanelID pid)
    {
        return GetPanel(pid) != null;
    }

    /// <summary>
    /// 面板资源是否准备好
    /// </summary>
    /// <param name="localInfo"></param>
    /// <returns></returns>
    private bool IsPanelAssetReady(LocalPanelInfo localInfo)
    {
        UIPanelBase tempPanel = null;
        bool ready = true;
        if (localInfo.NeedBg && ready)
        {
            tempPanel = GetPanel(PanelID.CommonBgPanel);
            if (null == tempPanel)
            {
                ready = false;
            }
        }

        if (localInfo.NeedTopBar && ready)
        {
            tempPanel = GetPanel(PanelID.TopBarPanel);
            if (null == tempPanel)
            {
                ready = false;
            }
        }

        tempPanel = GetPanel(localInfo.PID);
        if (null == tempPanel)
        {
            ready = false;
        }

        return ready;
    }

    /// <summary>
    /// 是否下一个将要显示的面板ready
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    private bool IsNextShowPanelReady(ref PanelID pid)
    {
        nextShowPanelID = PanelID.None;
        bool ready = false;
        if (m_lstWaitingShow.Count > 0)
        {
            PanelShowData nextShowData = m_lstWaitingShow[0];
            nextShowPanelID = nextShowData.PID;
            LocalPanelInfo localInfo = null;
            if (TryGetLocalPanelInfo(pid, out localInfo))
            {
                ready = IsPanelAssetReady(localInfo);
            }
        }

        return ready;
    }
    //下一个将要显示面板ID
    private PanelID nextShowPanelID = PanelID.None;
    /// <summary>
    /// 面板显示回调
    /// </summary>
    /// <param name="pid"></param>
    private void OnPanelShow(PanelID pid)
    {
        //Debug.LogError("*********ShowPanel pid=" + pid + ",waitingpanelsize:" + m_lstWaitingShow.Count);
        if (!IsShowPanel(pid))
        {
            AddShowPanel(pid);
        }

        if (pid != PanelID.TopBarPanel 
            && pid != PanelID.CommonBgPanel
            && m_lstWaitingShow.Count > 0)
        {
            PanelShowData showData = m_lstWaitingShow[0];
            if (showData.PID == pid)
            {
                m_lstWaitingShow.RemoveAt(0);
                if (null != showData.PanelShowAction)
                {
                    showData.PanelShowAction.Invoke(GetPanel(pid));
                }
                ProccessPanelShowing();
            }
        }
    }

    /// <summary>
    /// 面板隐藏回掉
    /// </summary>
    /// <param name="pid"></param>
    private void OnPanelHide(PanelID pid)
    {
        //Debug.LogError("-------HidePanel pid=" + pid + ",waitingpanelsize:" + m_lstWaitingShow.Count);
        if (IsShowPanel(pid))
        {
            RemoveHidePanel(pid);
        }
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="panelShowData"></param>
    private UIPanelBase ShowPanel(PanelShowData panelShowData)
    {
        UIPanelBase pBase = GetPanel(panelShowData.PID);
        if (null == pBase)
        {
            Engine.Utility.Log.Error("UIPanelManager->ShowPanel pBase null,pid:{0}", panelShowData.PID);
            return pBase;
        }

        if (IsShowPanel(panelShowData.PID))
        {
            pBase.PrePanelData = panelShowData.PrePanelData;
            pBase.ShowPanel(panelShowData.Data, jumpData: panelShowData.JumpData);
            Engine.Utility.Log.Warning(CLASS_NAME + "-> ShowPanel failed,panelId = {0} already show!", panelShowData.PID.ToString());
            return pBase;
        }

        //如果存在互斥面板已经显示则直接返回
        LocalPanelInfo panelInfo = null;
        if (TryGetLocalPanelInfo(panelShowData.PID, out panelInfo))
        {
            List<PanelID> mutexPanels = panelInfo.MutexPanels;
            if (null != mutexPanels)
            {
                for (int i = 0; i < mutexPanels.Count; i++)
                {
                    if (IsShowPanel(mutexPanels[i]))
                    {
                        DealShowPanelMutex(panelShowData.PID);
                        return null;
                    }
                }
            }
        }

        

        //if (panelShowData.PID == PanelID.MainPanel || panelShowData.PID == PanelID.MissionAndTeamPanel)
        //{
        //    UIRootHelper.Instance.SetPanelRootStatusByType(PanelRootType.Main, true);
        //}

        ReadyPanel(pBase, panelShowData.IgnoreCache);

        if (pBase.PanelInfo.IsStartPanel || panelShowData.ForceResetPanel)
        {
            pBase.ResetPanel();
        }

        DoShowPanel(pBase, panelShowData.PrePanelData, panelShowData.Data, panelShowData.JumpData);
        if (pBase.PanelInfo.IsStartPanel || panelShowData.ForceClearCacheBack)
        {
            ClearCacheStack();
        }
        return pBase;
    }

    /// <summary>
    /// 处理面板显示互斥
    /// </summary>
    /// <param name="pid"></param>
    private void DealShowPanelMutex(PanelID pid)
    {
        if (pid == PanelID.CommonBgPanel || pid == PanelID.TopBarPanel)
            return;
        if (null != m_lstWaitingShow && m_lstWaitingShow.Count > 0)
        {
            PanelShowData pSD =  m_lstWaitingShow[0];
            if (pSD.PID == pid)
            {
                m_lstWaitingShow.RemoveAt(0);
            }
            ProccessPanelShowing();
        }
    }

    /// <summary>
    /// 执行面板显示
    /// </summary>
    private void ProccessPanelShowing()
    {
        LocalPanelInfo localPanelInfo = null;
        if (IsNextShowPanelReady(ref nextShowPanelID))
        {
            if (TryGetLocalPanelInfo(nextShowPanelID,out localPanelInfo))
            {
                PanelShowData nextShowData = new PanelShowData();
                if (nextShowPanelID != PanelID.CommonBgPanel
                    && nextShowPanelID != PanelID.TopBarPanel)
                {
                    if (localPanelInfo.NeedBg)
                    {
                        nextShowData.PID = PanelID.CommonBgPanel;
                        ShowPanel(nextShowData);
                    }

                    if (localPanelInfo.NeedTopBar)
                    {
                        nextShowData.PID = PanelID.TopBarPanel;
                        nextShowData.Data = localPanelInfo;
                        ShowPanel(nextShowData);
                    }
                }

                if (m_lstWaitingShow.Count > 0)
                {
                    nextShowData = m_lstWaitingShow[0];
                    ShowPanel(nextShowData);
                }
                
            }
            
        }else if (m_lstWaitingShow.Count > 0)
        {
            PanelShowData psd = m_lstWaitingShow[0];
            if (TryGetLocalPanelInfo(psd.PID, out localPanelInfo) && !IsPanelAssetReady(localPanelInfo))
            {
                ReadyPanelAsset(localPanelInfo);
            }
        }
    }

    #endregion

    #region HidePanel
    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="id"></param>
    /// <param name="action">关闭回调</param>
    public void HidePanel(PanelID id, Action action = null)
    {
        HidePanel(id, true, action);
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="id">面板id</param>
    /// <param name="ignoreCacheBack">是否忽略cacheBack</param>
    /// <param name="action"></param>
    /// <param name="needDestroy"></param>
    /// <param name="?"></param>
    public void HidePanel(PanelID id, bool needReturnBack, Action action = null)
    {
        if (!IsShowPanel(id))
            return;
        
        UIPanelBase panel = GetPanel(id);
        int pid = (int)id;
        Action hideCallback = () =>
        {
            LocalPanelInfo lp = null;
            if (TryGetLocalPanelInfo(id, out lp) && lp.PanelTaData.Enable)
            {
                UIFrameManager.Instance.ResetTabs();
            }
            if (needReturnBack)
            {
                if (panel.PanelInfo.CacheLv != PanelCacheLv.None)
                {
                    OnPanelCacheBack(id);
                }
                //如果有前置面板，显示前置面板
                if (null != panel.PrePanelData && panel.PrePanelData.PID != PanelID.None)
                {

                    ShowPanel(panel.PrePanelData.PID
                        ,prePanelData:panel.PrePanelData.PrePanelData
                        ,data:panel.PrePanelData.Data,jumpData:panel.PrePanelData.JumpData);
                }
            }
            //主界面影藏，根节点也隐藏
            //if (id == PanelID.MainPanel)
            //{
            //    UIRootHelper.Instance.SetPanelRootStatusByType(PanelRootType.Main,false);
            //}
        };

        if (null != action)
        {
            hideCallback += action;
        }

        panel.HidePanel(hideCallback);
    }

    /// <summary>
    ///添加显示面板 
    /// </summary>
    /// <param name="panelID"></param>
    private void AddShowPanel(PanelID panelID)
    {
        if (!IsShowPanel(panelID))
        {
            showPanels.Add((uint)panelID);
        }
    }

    /// <summary>
    /// 移除隐藏面板
    /// </summary>
    /// <param name="panelID"></param>
    private void RemoveHidePanel(PanelID panelID)
    {
        UIPanelBase panel = null;
        uint pid = (uint)panelID;
        if (IsShowPanel(panelID))
        {
            showPanels.Remove(pid);
        }
        if (allPanels.TryGetValue(pid, out panel))
        {
            allPanels.Remove(pid);
            //Debug.LogError("p2:" + panel.CacheTransform.GetInstanceID());
            UIManager.OnObjsRelease(panel.CacheTransform, panel.PanelInfo.TableData.resID);
            
        }
    }

    #endregion

    #region PanelCache
    /// <summary>
    /// 清空堆栈缓存面板
    /// </summary>
    private void ClearCacheStack()
    {
        CurShowCachePanel = PanelID.None;
        panelCacheStack.Clear();
    }


    /// <summary>
    ///  面板返回
    /// </summary>
    /// <param name="pid"></param>
    public void OnPanelCacheBack(PanelID pid = PanelID.None)
    {
        if (pid != PanelID.None && pid != CurShowCachePanel)
        {
            if (IsShowPanel(CurShowCachePanel))
            {
                HidePanel(CurShowCachePanel, false, null);
            }

            ShowMain();

            UIFrameManager.Instance.CheckCloseRightBtn();
            Engine.Utility.Log.Error("OnPanelCacheBack return {0},{1}", pid, CurShowCachePanel);
            return;
        }

        if (CurShowCachePanel != PanelID.None)
        {
            //内部跳转直接成功直接return
            UIPanelBase panelBase = GetPanel(CurShowCachePanel);
            if (null != panelBase && panelBase.ExecuteReturnLogic())
            {
                return;
            }
        }
        if (IsShowPanel(PanelID.TopBarPanel))
            HidePanel(PanelID.TopBarPanel, false);
        if (IsShowPanel(PanelID.CommonBgPanel))
            HidePanel(PanelID.CommonBgPanel, false);

        if (panelCacheStack.Count == 0)
        {
            if (IsShowPanel(CurShowCachePanel))
            {
                HidePanel(CurShowCachePanel, false, null);
            }

            ShowMain();

            UIFrameManager.Instance.CheckCloseRightBtn();

            return;
        }
        bool success = false;

       
        UIPanelBase.PanelReturnData panelReturnData = panelCacheStack.Peek();
        if (null != panelReturnData)
        {
            if (panelReturnData.PID == CurShowCachePanel)
            {
                panelCacheStack.Pop();
                success = true;
                Action returenAction = () =>
                {
                    CurShowCachePanel = PanelID.None;
                    if (null != panelReturnData.CachePanelList && panelReturnData.CachePanelList.Count > 0)
                    {
                        UIPanelBase.PanelData pd = null;
                        for (int i = 0; i < panelReturnData.CachePanelList.Count; i++)
                        {
                            pd = panelReturnData.CachePanelList[i];
                            if (null == pd)
                            {
                                continue;
                            }

                            ShowPanel(pd.PID, pd.PrePanelData, true, data: pd.Data, jumpData: pd.JumpData);
                        }
                    }
                };
                if (IsShowPanel(CurShowCachePanel))
                {
                    HidePanel(CurShowCachePanel, false, returenAction);
                }
                else
                {
                    returenAction.Invoke();
                }
            }
            else
            {
                Engine.Utility.Log.Error("PanelCacheBackError {0},{1}", panelReturnData.PID ,CurShowCachePanel);
            }
        }
        
        if (!success)
        {
            ShowMain();
        }

        UIFrameManager.Instance.CheckCloseRightBtn();
    }
    
    #endregion
    
    /// <summary>
    ///显示tips
    /// </summary>
    /// <param name="data">目标UI对象</param>
    /// <param name="targetUIObejct">目标UI对象</param>
     /// <param name="needCompare">是否需要装备对比</param>
     public void ShowItemTips(BaseItem data, GameObject targetUIObejct = null, bool needCompare = false)
     {
         UIDefine.TipsPanelData tipsPanelData = new UIDefine.TipsPanelData()
         {
             m_bool_needCompare = needCompare,
             m_data = data,
             m_obj_targetUIGameObj = (null != targetUIObejct && UnityEngine.LayerMask.LayerToName(targetUIObejct.layer) == "UI") ? targetUIObejct : null,
         };
         ShowPanel(PanelID.ItemTipsPanel, data: tipsPanelData);
     }

     public void ShowOtherItemTips(BaseItem data ,GameObject targetObj,Dictionary<uint,uint> dicGem,uint pLevel)
     {
         UIDefine.TipsPanelData tipsPanelData = new UIDefine.TipsPanelData()
         {
             m_bool_needCompare = false,
             m_data = data,
             m_dicGem = dicGem,
             m_nPlayerLevel = (int)pLevel,
             m_obj_targetUIGameObj = (null != targetObj && UnityEngine.LayerMask.LayerToName(targetObj.layer) == "UI") ? targetObj : null,
         };
         ShowPanel(PanelID.ItemTipsPanel, data: tipsPanelData);
     }

     #region ReleasePanel

    /// <summary>
    /// 释放面板
    /// </summary>
    /// <param name="panelId"></param>
    /// <param name="dlg"></param>
    /// <returns></returns>
    public void ReleasePanel(PanelID panelId,Action dlg = null)
    {
        UIPanelBase pb = null;
        if (null != allPanels && allPanels.TryGetValue((uint)panelId, out pb))
        {
            if (IsShowPanel(pb.PanelId))
            {
                HidePanel(pb.PanelId, false, dlg);
            }
        }
    }

    /// <summary>
    /// 释放所有面板
    /// </summary>
    public void ReleaseAllPanel()
    {
        if (null != allPanels && allPanels.Count > 0)
        {
            List<uint> pid = new List<uint>();
            pid.AddRange(allPanels.Keys);
            for(int i = 0;i < pid.Count;i++)
            {
                ReleasePanel((PanelID)pid[i]);
            }
        }
    }
    
    #endregion

    #region FocusPanel
    private List<PanelID> m_lst_curFocusPanels = null;
    /// <summary>
    /// 当前面板是否获取了焦点
    /// </summary>
    /// <param name="panelID"></param>
    /// <returns></returns>
    public bool IsPanelFocus(PanelID panelID)
    {
        return (null != m_lst_curFocusPanels && m_lst_curFocusPanels.Contains(panelID));
    }

    /// <summary>
    /// 从获取焦点的面板列表中移除
    /// </summary>
    /// <param name="panelID"></param>
    /// <returns></returns>
    public bool RemoveFocusPanel(PanelID panelID)
    {
        if (null != m_lst_curFocusPanels)
        {
            return  m_lst_curFocusPanels.Remove(panelID);
        }
        return false;
    }

    /// <summary>
    /// 清空所有获得焦点的面板
    /// </summary>
    /// <param name="panelID"></param>
    public void RemoveAllFocusPanel()
    {
        if (null != m_lst_curFocusPanels)
        {
            m_lst_curFocusPanels.Clear();
        }
    }

    /// <summary>
    /// 添加一个获取了焦点的面板
    /// </summary>
    /// <param name="panelID"></param>
    public void AddFocusPanel(PanelID panelID)
    {
        if (null != m_lst_curFocusPanels && !m_lst_curFocusPanels.Contains(panelID))
        {
            m_lst_curFocusPanels.Add(panelID);
        }
    }


    /// <summary>
    /// 获取当前获得焦点的面板
    /// </summary>
    /// <returns></returns>
    public List<PanelID> GetFocusPanels()
    {
        return m_lst_curFocusPanels;
    }

    /// <summary>
    /// 刷新当前焦点数据
    /// </summary>
    /// <param name="status"></param>
    private void RefreshFocusDatas(PanelID panelID,bool cansee)
    {
        if (!cansee)
        {
            if (RemoveFocusPanel(panelID))
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, new PanelFocusData()
                {
                    ID = panelID,
                    GetFocus = false,
                });
            }
        }
        else
        {
            CheckFocusDatas(panelID);
        }
    }

    List<PanelID> oldFocusTempData = new List<PanelID>();
    List<PanelID> curFocusTempData = new List<PanelID>();
    /// <summary>
    /// 根据当前显示面板重新检测当前获取焦点面板
    /// </summary>
    /// <param name="panelID">当前显示面板</param>
    private void CheckFocusDatas(PanelID panelID)
    {
        LocalPanelInfo panelInfo = null;
        if (!TryGetLocalPanelInfo(panelID, out panelInfo) || !panelInfo.IsNeedCalculateFocus)
        {
            return;
        }
        PanelRootType rootType = UIRootHelper.Instance.GetRootTypePanelType(panelInfo.PType);
        oldFocusTempData.Clear();
        curFocusTempData.Clear();
        oldFocusTempData.AddRange(GetFocusPanels());
        RemoveAllFocusPanel();

        PanelRootType tempRootType = PanelRootType.None;
        PanelRootType maxFocusRootType = PanelRootType.None;

        LocalPanelInfo tempPanelInfo = null;

        List<uint> showPanelsKeys = new List<uint>();
        showPanelsKeys.AddRange(showPanels);
        PanelID showNormal = PanelID.None;
        int maxRootPanelDepth = 0;
        UIPanelBase tempPanelBase = null;
        PanelID pid = PanelID.None;
        for(int i = 0;i< showPanelsKeys.Count;i++)
        {
            pid = (PanelID)showPanelsKeys[i];
            if (!TryGetLocalPanelInfo(pid, out tempPanelInfo) || !tempPanelInfo.IsNeedCalculateFocus)
            {
                continue;
            }
            if (tempPanelInfo.PType == PanelType.Normal)
            {
                showNormal = tempPanelInfo.PID;
                
            }
            tempRootType = UIRootHelper.Instance.GetRootTypePanelType(tempPanelInfo.PType);
            if ((tempRootType == PanelRootType.None || maxFocusRootType < tempRootType) && tempRootType != PanelRootType.MarQueen
                //&& tempPanelInfo.PType != PanelType.SmartPopUp
                )
            {
                maxFocusRootType = tempRootType;
            }
            tempPanelBase = GetPanel(pid);
            if (tempRootType == maxFocusRootType && tempPanelBase.Depth > maxRootPanelDepth)
            {
                maxRootPanelDepth = tempPanelBase.Depth;
            }
        }

        for (int i = 0; i < showPanelsKeys.Count; i++)
        {
            pid = (PanelID)showPanelsKeys[i];
            if (!TryGetLocalPanelInfo(pid, out tempPanelInfo) || !tempPanelInfo.IsNeedCalculateFocus)
            {
                continue;
            }
            tempRootType = UIRootHelper.Instance.GetRootTypePanelType(tempPanelInfo.PType);
            tempPanelBase = GetPanel(pid);
            if (tempRootType < maxFocusRootType)
            {
                if (maxFocusRootType == PanelRootType.Fixed)
                {
                    if (showNormal != PanelID.None)
                    {
                        if (tempPanelBase.PanelId == showNormal)
                        {
                            AddFocusPanel(pid);
                        }
                    }else
                    {
                        AddFocusPanel(pid);
                    }
                }
            }else if (tempRootType == maxFocusRootType)
            {
                if (maxFocusRootType == PanelRootType.Main || tempPanelBase.Depth >= maxRootPanelDepth)
                {
                    AddFocusPanel(pid);
                }
            }else if (tempRootType > maxFocusRootType)
            {
                AddFocusPanel(pid);
            }
            
        }

        //3、对比焦点变化的面板
        curFocusTempData.AddRange(GetFocusPanels());
        
        PanelFocusData focusData = new PanelFocusData();
        focusData.GetFocus = false;
        for(int i = 0;i < oldFocusTempData.Count;i++)
        {
            if (curFocusTempData.Contains(oldFocusTempData[i]))
            {
                curFocusTempData.Remove(oldFocusTempData[i]);
            }else
            {
                focusData.ID = oldFocusTempData[i];
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, focusData);
            }
        }

        focusData.GetFocus = true;
        for(int i=0;i < curFocusTempData.Count;i++)
        {
            focusData.ID = curFocusTempData[i];
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_PANELFOCUSSTATUSCHANGED, focusData);
        }

    }

    #endregion

    #region PanelManager
    public void SendMsg(PanelID id, UIMsgID msgid, object param)
    {
        if (!IsShowPanel(id))
        {
            return;
        }
        UIPanelBase panelBase = GetPanel(id);
        if (null != panelBase)
        {
            panelBase.OnMsg(msgid, param);
        }
    }

    /// <summary>
    /// 进度条
    /// </summary>
    /// <param name="tips"></param>
    /// <param name="progress"></param>
    public void ShowLoading(string tips = "",float progress = -1f,Action<UIPanelBase> panelShowAction = null)
    {
        if (!IsShowPanel(PanelID.LoadingPanel))
        {
            ShowPanel(PanelID.LoadingPanel,panelShowAction:panelShowAction);
        }
        else if (null != panelShowAction)
        {
            panelShowAction.Invoke(GetPanel(PanelID.LoadingPanel));
        }
        if (!string.IsNullOrEmpty(tips))
        {
            SendMsg(PanelID.LoadingPanel, UIMsgID.eLoadingTips, tips);
        }

        if (progress >= 0)
        {
            SendMsg(PanelID.LoadingPanel, UIMsgID.eLoadingProcess, progress);
        }
    }

    public void ShowMain(bool bFirst = false)
    {
        ClearCacheStack();
        
        if (!IsShowPanel(PanelID.RoleStateBarPanel))
        {
            ShowPanel(PanelID.RoleStateBarPanel);
        }
       
        if (DataManager.Manager<ArenaManager>().EnterArena == false)
        {
            ShowPanel(PanelID.MissionAndTeamPanel);
        }

        ShowPanel(PanelID.MainPanel);
        Client.IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
        if(ms != null)
        {
            if(ms.GetMapID() != GameTableManager.Instance.GetGlobalConfig<int>( "HomeSceneID" ))
            {
                //ShowPanel( PanelID.SkillPanel );
            }
        }

        //阵营战  打开战斗界面
        if (DataManager.Manager<CampCombatManager>().isEnterScene)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CampFightingPanel);
        }
        //城战  打开城战战斗界面
        if (DataManager.Manager<CityWarManager>().EnterCityWar)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CityWarFightingPanel);
        }

        ShowPanel(PanelID.GuideUnconstrainPanel);
        //if (DataManager.Manager<ComBatCopyDataManager>().EnterCopyID ==0)//非副本才显示
        if (!DataManager.Manager<ComBatCopyDataManager>().IsEnterCopy )//非副本才显示
        {
            ShowPanel(PanelID.MessagePushPanel);
        }
        ShowPanel(PanelID.EffectDiplayPanel);
        
        if (bFirst)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SYSTEM_LOADUICOMPELETE);
        }
    }

    public void HideMain()
    {
        HidePanel(PanelID.MainPanel);
        HidePanel(PanelID.MissionAndTeamPanel);
        HidePanel(PanelID.MessagePushPanel);
    }

    public void CacheShowPanel()
    {

    }

    public void ShowCachePanel()
    {

    }

    #endregion

    #region Story(剧情)

    /// <summary>
    /// 当前显示剧情面板缓存的活动panel
    /// </summary>
    public void ShowStory(StoryPanel.StoryData data)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.StoryPanel, data: data);
    }

    /// <summary>
    /// 隐藏剧情
    /// </summary>
    public void HideStory()
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.StoryPanel,true);
    }
    #endregion

    public void ShowPanelTips(string tipsContronlName, LocalPanelInfo info)
    {
        string btnName = tipsContronlName;
        if (!string.IsNullOrEmpty(btnName))
        {
            int len = btnName.Length;
            if (len > 0)
            {
                char chnum = btnName[len - 1];
                int num = 0;
                if (int.TryParse(chnum.ToString(), out num))
                {
                    if (info.TextIDList != null)
                    {
                        if (num > 0)
                        {
                            num -= 1;
                            if(num < info.TextIDList.Count)
                            {
                                uint textID = info.TextIDList[num];
                                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonTipsPanel, data: textID);
                            }                          
                        }
                    }
                }
            }
        }
    }
 }