using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class UIPanelBase : UIBase, IUIAnimation
{
    #region define
    /// <summary>
    /// 面板状态
    /// </summary>
    public enum PanelStatus
    {
        None,
        Awake,          //唤醒
        PrepareShow,    //准备显示（当前不可见）
        Show,           //已经显示
        Hide,           //已经隐藏
        Destroy,        //销毁
    }

    /// <summary>
    /// 面板状态数据
    /// </summary>
    public class PanelStatusData
    {
        public PanelID ID = PanelID.None;
        public PanelStatus Status = PanelStatus.None;
        public GameObject Obj = null;

        private PanelStatusData ()
        {
            
        }

        public static PanelStatusData Create(PanelID panelId,PanelStatus status,GameObject obj = null)
        {
            PanelStatusData statusData = new PanelStatusData()
            {
                ID = panelId,
                Status = status,
                Obj = obj,
            };
            return statusData;
        }
    }

    /// <summary>
    /// 面板跳转数据
    /// </summary>
    public class PanelJumpData
    {
        //是否为回退
        public bool IsBackspacing = false;
        //大页签 
        public int[] Tabs;
        //选中数据
        public object Param;
        //扩展参数
        public object ExtParam;
        public override string ToString()
        {
            string str = "";
            if (Tabs != null)
            {
                for (int i = 0; i < Tabs.Length; i++)
                {
                    str += Tabs[i].ToString() + " ";
                }
            }
            return str + "  " + Param.ToString();
        }
    }

    public class PanelData
    {
        //id
        public PanelID PID = PanelID.None;
        //前置面板数据
        public PanelData PrePanelData = null;
        //跳转数据
        public PanelJumpData JumpData = null;
        //传递参数
        public object Data = null;
    }

    public class PanelReturnData
    {
        public List<PanelData> CachePanelList = null;
        //缓存CachePanelList的列表
        public PanelID PID = PanelID.None;
    }

    #endregion

    public static int FisrstTabsIndex = 1;

    public UIPanelManager.LocalPanelInfo PanelInfo { get; set; }

    //页签
    public Dictionary<int, Dictionary<int,UITabGrid>> dicUITabGrid = new Dictionary<int, Dictionary<int,UITabGrid>>(3);
    //当前高亮的页签
    public SortedDictionary<int, int> dicActiveTabGrid = new SortedDictionary<int, int>();
    /// <summary>
    /// 深度
    /// </summary>
    public int Depth
    {
        get
        {
            return Panel.depth;
        }
        set
        {
            Panel.depth = value;
        }
    }

    private UIPanel m_panel = null;
    /// <summary>
    /// 依赖的ngui面板
    /// </summary>
    public UIPanel Panel
    {
        get
        {
            if (null != m_panel)
            {
                return m_panel;
            }
            m_panel = CacheTransform.GetComponent<UIPanel>();
            if (m_panel == null)
                m_panel = CacheTransform.gameObject.AddComponent<UIPanel>();
            return m_panel;
        }
    }

    public void SetDirty()
    {
        UIPanel panel = Panel;
        if (null != panel)
        {
            panel.SetDirty();
        }
    }

    protected bool mbAnimEnable = false;
    private bool isLock = false;
    public void SetLock(bool lockpanel)
    {
        isLock = lockpanel;
    }

    //面板id
    public PanelID PanelId
    {
        get
        {
            return (null != PanelInfo) ? PanelInfo.PID : PanelID.None;
        }
    }

    private UIPanelBase.PanelData m_prePanelData = null;
    public UIPanelBase.PanelData PrePanelData
    {
        get
        {
            return m_prePanelData;
        }
        set
        {
            m_prePanelData = value;
        }
    }

    /// <summary>
    /// 面板类型
    /// </summary>
    public PanelType PanelType
    {
        get { 
            if (PanelInfo != null)
            {
                return PanelInfo.PType;
            }
            return PanelType.PopUp;
        }
    }

    protected object cacheData = null;

    #region PanelBaseData Property

    //面板名称
    public string Name
    {
        get
        {
            return "";
        }
    }

    public UIDefine.UIPanelColliderMode UIColliderMode
    {
        get
        {
            if (PanelInfo != null)
            {
                return PanelInfo.CollideMode;
            }
            return UIDefine.UIPanelColliderMode.None;
        }
    }
    /// <summary>
    /// 是否动画可用
    /// </summary>
    public bool IsAnimEnable
    {
        get
        {
            return mbAnimEnable;
        }
    }

    #endregion

    protected override void OnAwake()
    {
        base.OnAwake();
        try
        {
            OnInitPanelData();
            InitControls();
            RegisterControlEvents();
            OnLoading();
        }catch(Exception e)
        {
            Debug.LogError(gameObject.name + "->OnAwake Error message:" + e.Message);
        }
        //发送panelAwake事件
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_PANELSTATUS, PanelStatusData.Create(PanelId,PanelStatus.Awake));
    }

    #region Virtual Method
    /// <summary>
    /// 初始化panel数据 paneldata
    /// </summary>
    protected virtual void OnInitPanelData()
    {

    }
    //在窗口第一次加载时，调用
    protected virtual void OnLoading()
    {

    }

    //在窗口第一次加载时，调用
    protected virtual void InitControls()
    {
    }

    //在窗口第一次加载时，调用
    protected virtual void RegisterControlEvents()
    {

    }

    //准备显示
    protected virtual void OnPrepareShow(object data)
    {
        
    }

    /// <summary>
    /// 界面显示回调
    /// </summary>
    /// <param name="data"></param>
    protected virtual void OnShow(object data)
    {

    }

    protected virtual void OnPanelReady()
    {

    }
    /// <summary>
    /// 界面跳转
    /// </summary>
    protected virtual void OnJump(PanelJumpData jumpData)
    {

    }

    /// <summary>
    /// 隐藏
    /// </summary>
    protected virtual void OnHide()
    {
        
    }

    public void HideSelf()
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelId);
    }
    
    /// <summary>
    /// 重置面板
    /// </summary>
    public virtual void ResetPanel()
    {

    }

    /// <summary>
    /// 碰撞器遮罩点击事件响应
    /// </summary>
    public virtual void OnColliderMaskClicked()
    {
 
    }

    /// <summary>
    /// 关闭界面判断逻辑
    /// </summary>
    /// <returns></returns>
    public virtual bool ExecuteReturnLogic()
    {
        return false;
    }

    public virtual PanelData GetPanelData()
    {
        PanelData pd = new PanelData()
        {
            PID = PanelId,
            PrePanelData = PrePanelData,
            Data = cacheData,
        };
        return pd;
    }

    #endregion

    private bool m_bResetAnchor = false;
    #region Public Method

    /// <summary>
    /// 界面显示
    /// </summary>
    /// <param name="data"></param>
    public void ShowPanel(object data = null,PanelJumpData jumpData = null)
    {
        try
        {
            cacheData = data;
            //if (!Visible)
            {
                OnPrepareShow(data);
                //发送面板显示事件
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_PANELSTATUS
                    , PanelStatusData.Create(PanelId, PanelStatus.PrepareShow));
            }
            
            CacheTransform.localPosition = Vector3.zero;
            ResetAnchor();
            
            SetVisible(true);
            //发送面板显示事件
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_PANELSTATUS
                , PanelStatusData.Create(PanelId, PanelStatus.Show));

            OnShow(data);
            OnJump(jumpData);
            if (mbAnimEnable)
                AnimIn(null);
        }
        catch (Exception e)
        {
            Debug.LogError(gameObject.name + "->ShowPanel Error message:" + e.Message);
        }
        
       

        try
        {
            OnPanelReady();
        }
        catch (System.Exception ex)
        {
            Debug.LogError(gameObject.name + "->OnPanelReady Error message:" + ex.Message);
        }
        
    }
    
    /// <summary>
    /// ResetAnchor
    /// </summary>
    /// <param name="force"></param>
    public void ResetAnchor(bool force = false)
    {
        if (!m_bResetAnchor || force)
        {
            m_bResetAnchor = true;
            UIAnchor[] anchors = gameObject.GetComponentsInChildren<UIAnchor>();
            if (null != anchors)
            {
                for (int i = 0, max = anchors.Length; i < max; i++)
                {
                    anchors[i].DoAnchor();
                }
            }
        }
    }
    /// <summary>
    /// 界面隐藏
    /// </summary>
    /// <param name="action"></param>
    public void HidePanel(System.Action action = null)
    {
        Action onHideAction = delegate
        {
            try
            {
                //SetVisible(false);
                OnHide();
            }
            catch (Exception e)
            {
                Debug.LogError(gameObject.name + "->HidePanel Error message:" + e.Message);
            }
            //发送面板显示事件
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_PANELSTATUS
            , PanelStatusData.Create(PanelId, PanelStatus.Hide));
        };
        if (null != action)
        {
            onHideAction += action;
        }
        
        if (mbAnimEnable)
        {
            AnimOut(delegate
            {
                onHideAction.Invoke();
            });
        }
        else
        {
            onHideAction.Invoke();
        }
        
    }


    /// <summary>
    /// 返回true 说明可以点击设置切换状态
    /// </summary>
    /// <param name="pageid"></param>
    /// <returns></returns>
    public virtual bool OnTogglePanel(int tabType,int pageid)
    {
        return true;
    }

    protected virtual void OnPanelBaseDestory()
    {
        try
        {
            Release();
            if (dicActiveTabGrid != null)
            {
                dicActiveTabGrid.Clear();
                dicActiveTabGrid = null;
            }

            if (dicUITabGrid != null)
            {
                dicUITabGrid.Clear();
                dicUITabGrid = null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(gameObject.name + "->OnPanelBaseDestory Error message:" + e.Message);
        }
        
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_PANELSTATUS
            , PanelStatusData.Create(PanelId, PanelStatus.Destroy,gameObject));
    }
    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        OnPanelBaseDestory();
    }
    private List<UIPanel> m_lst_panels = null;
    public void InitPanelDetph()
    {
        if (null == m_lst_panels)
        {
            m_lst_panels = new List<UIPanel>();
            List<UIDefine.UIPanelHierarchyData> panelHierarchyData = new List<UIDefine.UIPanelHierarchyData>();
            UIPanel panel = CacheTransform.GetComponent<UIPanel>();
            if (null != panel)
            {
                m_lst_panels.Add(panel);
            }
            UIPanelManager.GetPanelHierarchydepth(CacheTransform, 0, ref panelHierarchyData);
            if (panelHierarchyData.Count > 0)
            {
                panelHierarchyData.Sort((left, right) =>
                {
                    return left.hierachyDepth - right.hierachyDepth;
                });
                
                for (int i = 0; i < panelHierarchyData.Count; i++)
                {
                    m_lst_panels.Add(panelHierarchyData[i].panel);
                }
            }
        }
    }

    public void UpdatePanelDepth(int startDepth)
    {
        if (null != m_lst_panels)
        {
            UIPanel tempPanel = null;
            for(int i = 0,max = m_lst_panels.Count;i < max;i++)
            {
                tempPanel = m_lst_panels[i];
                if (null != tempPanel)
                {
                    startDepth++;
                    tempPanel.depth = startDepth;
                }
            }
        }
    }

    /// <summary>
    /// 面板状态改变
    /// </summary>
    protected void OnPanelStateChanged()
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTUISTATECHANGED,PanelId);
    }
    #endregion

    #region IUIAnimation
    //动画In
    public virtual void AnimIn(EventDelegate.Callback onComplete)
    {
        try
        {
            if (null != onComplete)
                onComplete.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(gameObject.name + "->AnimIn Error message:" + e.Message);
        }
        
    }
    /// <summary>
    /// 动画Out (注：重载animation 要处理onComplete回调，开通过base.AnimOut(onComplete)
    /// 或者主动处理
    /// </summary>
    /// <param name="onComplete"></param>
    public virtual void AnimOut(EventDelegate.Callback onComplete)
    {
        try
        {
            if (null != onComplete)
                onComplete.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(gameObject.name + "->AnimOut Error message:" + e.Message);
        }
    }
    //重置动画
    public virtual void ResetAnim()
    {

    }
    #endregion

    #region panelBase
    public GameObject GetChild(string name)
    {
        Transform node = Util.findTransform(transform, name);
        if (node != null)
        {
            return node.gameObject;
        }
        return null;
    }

    public GameObject GetChild(Transform root, string name)
    {
        Transform node = Util.findTransform(root, name);
        if (node != null)
        {
            return node.gameObject;
        }
        return null;
    }

    public T GetChildComponent<T>(string name) where T : Component
    {
        GameObject obj = GetChild(name);
        if (obj)
        {
            return obj.GetComponent<T>();
        }
        else
        {
            Debug.Log("no child : " + name);
            return null;
        }
    }

    public T GetChildComponent<T>(Transform root, string name) where T : Component
    {
        Transform node = Util.findTransform(root, name);
        if (node == null)
        {
            //return node.gameObject;
            return null;
        }

        GameObject obj = node.gameObject;

        if (obj)
        {
            return obj.GetComponent<T>();
        }
        else
        {
            //Log.Error("no child : " + name);
            return null;
        }
    }

    //后面最好写成纯虚函数
    public virtual bool OnMsg(UIMsgID msgid, object param)
    {
        return true;
    }

    /// <summary>
    /// 非自动添加的页签初始化
    /// </summary>
    /// <param name="tabTrans">页签的Transform</param>
    /// <param name="nTabType">页签的类型</param>
    /// <param name="nindex">页签索引</param>
    public void InitTabGrids(Transform tabTrans,int nTabType,int nindex,string strName = "")
    {
        Dictionary<int, UITabGrid> dicTabs = null;
        if (!dicUITabGrid.TryGetValue(nTabType, out dicTabs))
        {
            dicTabs = new Dictionary<int, UITabGrid>(6);
            dicUITabGrid.Add(nTabType, dicTabs);
        }

        UITabGrid tab = tabTrans.GetComponent<UITabGrid>();
        if (tab == null)
        {
            tab = tabTrans.gameObject.AddComponent<UITabGrid>();
        }
        if (tab != null)
        {
            tab.TabID = nindex;
            tab.TabType = nTabType;
            
            dicTabs[tab.TabID] = tab;
            if (!string.IsNullOrEmpty(strName))
            {
                tab.SetName(strName);
            }
            tab.SetHightLight(false);
            tab.RegisterUIEventDelegate((eventType, data, param) => {
                switch (eventType)
                {
                    case UIEventType.Click:
                        if (data is UITabGrid)
                        {
                            UITabGrid tabGRid = data as UITabGrid;
                            UIPanelBase panelbase = this as UIPanelBase;
                            UIFrameManager.Instance.OnCilckTogglePanel(ref panelbase,  tabGRid.TabType,tabGRid.TabID);
                        }
                        break;
                    default:
                        break;
                }
            });
        }
    }


    #endregion
}