using System;
using System.Collections.Generic;
using Client;
using System.Text;
using UnityEngine;

partial class MuhonPanel : UIPanelBase
{
    #region Define
    /// <summary>
    /// 精炼面板数据
    /// </summary>
    public class MuhonData
    {
        //当前选中的id
        public uint selectId;
        //当前页签状态
        public TabMode m_em_mode = TabMode.None;
    }
    #endregion

    #region Property

    //物品管理器
    private ItemManager imgr = null;
    //装备管理器
    private EquipManager emgr = null;
    //文本管理器
    private TextManager tmgr = null;
    public const string CLASS_NAME = "MuhonPanel";
    //当前状态
    private MuhonPanel.TabMode status = TabMode.None;
    //当前选中
    private uint selectedMuhonId = 0;
    //是否初始化
    private bool init = false;
    //自动使用点券Mask
    private int autoUseDQMask = 0;
    //面板模式初始化掩码
    private int m_int_modeInitMask = 0;
    //页签
    private Dictionary<TabMode, UITabGrid> m_dic_tabs = null;
    //内容
    private Dictionary<TabMode, Transform> m_dic_ts = null;
    public Muhon Data
    {
        get
        {
            return (selectedMuhonId != 0) ? imgr.GetBaseItemByQwThisId<Muhon>(selectedMuhonId) : null;
        }
    }

    /// <summary>
    /// 是否初始化当前模式
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public bool IsInitStatus(TabMode status)
    {
        return (m_int_modeInitMask & (1<<(int)status)) != 0;
    }

    /// <summary>
    /// 设置初始化状态
    /// </summary>
    /// <param name="status"></param>
    /// <param name="init"></param>
    public void SetInitStatus(TabMode status, bool init)
    {
        if (init)
        {
            m_int_modeInitMask |= ( 1<< (int)status);
        }else
        {
            m_int_modeInitMask &= (~( 1<<(int)status));
        }
        
    }

    #endregion

    #region OverrideMethod

    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalUIEvent(true);
        InitData();
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        int firstTabData = -1;
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }

        if (firstTabData == -1)
        {
            firstTabData = (null != jumpData.Tabs && jumpData.Tabs.Length >= 1) ? jumpData.Tabs[0] : (int)TabMode.ShengJi;
        }
        status = TabMode.None;
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
        uint selectId = 0;
        if (dataList.Count > 0 )
        {
            if (null != jumpData.Param)
            {
                selectId = (uint)jumpData.Param;
            }

            if (!dataList.Contains(selectId))
            {
                selectId = dataList[0];
            }
        }
        if (selectId != 0)
            SetSelectId(selectId, true, true);
        UpdateMuhonToggleVisible();
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[1];
        pd.JumpData.Tabs[0] = (int)status;
        pd.JumpData.Param = selectedMuhonId;
        return pd;
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalUIEvent(false);
        Release(false);
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_ctor_MohonGridScrollView)
        {
            m_ctor_MohonGridScrollView.Release(true);
        }
        dataList = null;

        if (null != m_itemGrowCostGrid)
        {
            m_itemGrowCostGrid.Release(depthRelease);
            if (depthRelease)
            {
                UIManager.OnObjsRelease(m_itemGrowCostGrid.CacheTransform, (uint)GridID.Uiitemgrowcostgrid);
                m_itemGrowCostGrid = null;
            }
        }
        ReleaseUpgrade(depthRelease);
        ReleaseEvolve(depthRelease);
        ReleaseBlend(depthRelease);
        ReleaseActiveRemove(depthRelease);
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Release(true);
    }

    public override bool ExecuteReturnLogic()
    {
        if (IsStatus(TabMode.JinHua))
        {
            return OnEvolveLogicBack();
        }
        return base.ExecuteReturnLogic();
    }
    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            if (Enum.IsDefined(typeof(TabMode), pageid))
            {
                SetStatus((TabMode)pageid);
            };
        }
        return base.OnTogglePanel(tabType, pageid);
    }
    #endregion

    #region OP
    /// <summary>
    /// 显示获取
    /// </summary>
    /// <param name="baseId"></param>
    private void ShowItemGet(uint baseId)
    {
        TipsManager.Instance.ShowItemTips(baseId);
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: baseId);
    }

    /// <summary>
    /// 获取UI返回数据
    /// </summary>
    /// <returns></returns>
    private ReturnBackUIData[] GetReturnBackUIData()
    {
        ReturnBackUIData[] returnData = new ReturnBackUIData[1];
        returnData[0] = new ReturnBackUIData();
        returnData[0].msgid = UIMsgID.eShowUI;
        returnData[0].panelid = PanelID.MuhonPanel;
        int[] tab = new int[1];
        tab[0] = (int)status;
        ReturnBackUIMsg backUIMsg = new ReturnBackUIMsg();
        backUIMsg.param = selectedMuhonId;
        backUIMsg.tabs = tab;
        returnData[0].param = backUIMsg;
        return returnData;
    }

    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch(eventType)
        {
            case (int)Client.GameEventID.UIEVENT_REFRESHMUHONEXP:
                OnExpChange(data);
                break;
            case (int)Client.GameEventID.UIEVENTMUHONUPGRADE:
                OnMuhonLevelUp();
                break;
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                OnUpdateItem((ItemDefine.UpdateItemPassData)data);
                break;
            case (int)Client.GameEventID.UIEVENTMUHONEVOLUTION:
                if (IsInitStatus(TabMode.JinHua))
                    OnMuhonEvolution((uint)data);
                break;
            case (int)Client.GameEventID.UIEVENTMUHONBLEND:
                if (IsInitStatus(TabMode.RongHe))
                    OnMuhonBlend((uint)data);
                break; 
        }
    }
    

    /// <summary>
    /// 物品改变回调
    /// </summary>
    /// <param name="passData"></param>
    void OnUpdateItem(ItemDefine.UpdateItemPassData passData)
    {
        if (null == passData || null == m_ctor_MohonGridScrollView)
        {
            return;
        }
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(passData.BaseId);
        baseItem.UpdateData(passData.BaseId);
        uint qwThisId = passData.QWThisId;
        BaseItem itemData = null;
        bool needRefreshUI = false;
        if (baseItem.IsMuhon)
        {
            switch(passData.UpdateType)
            {
                case ItemDefine.UpdateItemType.Add:
                    {
                        itemData = imgr.GetBaseItemByQwThisId(qwThisId);
                        if (itemData.PackType == GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP 
                            || itemData.PackType == GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN)
                        {
                            dataList.Add(qwThisId);
                            SortDataList();
                            m_ctor_MohonGridScrollView.InsertData(dataList.IndexOf(qwThisId));
                            if (selectedMuhonId == 0)
                            {
                                SetSelectId(qwThisId);
                            }
                        }
                    }
                    break;
                case ItemDefine.UpdateItemType.Remove:
                    {
                        RemoveEvolveDeputyItem(qwThisId);
                        if (dataList.Contains(qwThisId))
                        {
                            int index = dataList.IndexOf(qwThisId);
                            dataList.Remove(qwThisId);
                            m_ctor_MohonGridScrollView.RemoveData(index);
                            if (selectedMuhonId == qwThisId)
                            {
                                selectedMuhonId = (dataList.Count > 0) ? dataList[0] : 0;
                                if (selectedMuhonId != 0)
                                {
                                    SetSelectId(selectedMuhonId, true);
                                }else
                                {
                                    UpdateWidgetsVisbleStatus();
                                }
                            }
                        }
                    }
                    break;
                case ItemDefine.UpdateItemType.Update:
                    {
                        if (dataList.Contains(qwThisId))
                        {
                            itemData = imgr.GetBaseItemByQwThisId(qwThisId);
                            
                            int index = dataList.IndexOf(qwThisId);
                            m_ctor_MohonGridScrollView.UpdateData(index);
                            needRefreshUI = true;
                        }
                    }
                    break;
            }
        }
        if (needRefreshUI
            || baseItem.BaseType == GameCmd.ItemBaseType.ItemBaseType_Material)
        {
            UpdateDataUIByStatus();
        }
            
    }

    /// <summary>
    /// 检测孔圣魂状态
    /// </summary>
    private bool IsMuhonListEmpty()
    {
        return (null == dataList || dataList.Count == 0);
    }
    /// <summary>
    ///从新排列圣魂
    /// </summary>
    public void SortDataList()
    {
        if (null != dataList)
        {
            List<uint> equipList = new List<uint>();
            List<uint> knapList = new List<uint>();
            for (int i = 0; i < dataList.Count;i++ )
            {
                if (emgr.IsWearEquip(dataList[i]))
                {
                    equipList.Add(dataList[i]);
                }else
                {
                    knapList.Add(dataList[i]);
                }
            }

            if (equipList.Count >= 2)
            {
                equipList.Sort((left, right) =>
                {
                    Muhon leftM = imgr.GetBaseItemByQwThisId<Muhon>(left);
                    Muhon rightM = imgr.GetBaseItemByQwThisId<Muhon>(right);
                    if (null != leftM && null != rightM)
                    {
                        if ((int)leftM.Position.y < (int)rightM.Position.y)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    return 0;
                });
            }

            if (knapList.Count > 0)
            {
                knapList.Sort((left, right) =>
                {
                    Muhon leftM = imgr.GetBaseItemByQwThisId<Muhon>(left);
                    Muhon rightM = imgr.GetBaseItemByQwThisId<Muhon>(right);
                    if (null != leftM && null != rightM)
                    {
                        if (leftM.Power > rightM.Power)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    return 0;
                });
            }
            dataList.Clear();
            dataList.AddRange(equipList);
            dataList.AddRange(knapList);
        }
    }

    /// <summary>
    /// 注册全局UI事件
    /// </summary>
    /// <param name="register"></param>
    void RegisterGlobalUIEvent(bool register = true)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHMUHONEXP, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTMUHONEVOLUTION, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTMUHONBLEND, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTMUHONUPGRADE, OnGlobalUIEventHandler);
        }else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHMUHONEXP, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTMUHONEVOLUTION, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTMUHONBLEND, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTMUHONUPGRADE, OnGlobalUIEventHandler);
        }
    }
    //选中的圣魂id
    private uint selectMuhonId = 0;
    //列表数据
    private List<uint> dataList = new List<uint>();
    /// <summary>
    /// 初始化数据列表组件
    /// </summary>
    void InitDataListWiget()
    {
        if (null != m_ctor_MohonGridScrollView && null != m_trans_UIWeaponSoulInfoGrid)
        {
            m_ctor_MohonGridScrollView.RefreshCheck();
            m_ctor_MohonGridScrollView.Initialize<UIWeaponSoulInfoGrid>(m_trans_UIWeaponSoulInfoGrid.gameObject, OnUpdateDataListGrid, OnClickDataListEventCallback);
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitData()
    {
        if (null == dataList)
        {
            dataList = new List<uint>();
        }

        dataList.Clear();
        dataList.AddRange(emgr.GetWeaponSoulDataList());
        SortDataList();
        if (status == TabMode.None)
        {
            status = TabMode.ShengJi;
        }
        if (null != m_ctor_MohonGridScrollView)
        {
            m_ctor_MohonGridScrollView.CreateGrids(dataList.Count);
        }
    }

    /// <summary>
    /// 设置选中id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="force">是否强制更新</param>
    public void SetSelectId(uint id, bool force = false,bool focus = false)
    {
        if (id == selectedMuhonId && !force)
            return;
        ResetAutoUseDQ();
        
        int index = dataList.IndexOf(selectedMuhonId);
        //刷新数据
        UIWeaponSoulInfoGrid grid = m_ctor_MohonGridScrollView.GetGrid<UIWeaponSoulInfoGrid>(index);
        if (null != grid)
        {
            grid.SetHightLight(false);
        }
        index = dataList.IndexOf(id);
        if (focus)
            m_ctor_MohonGridScrollView.FocusGrid(index);

        grid = m_ctor_MohonGridScrollView.GetGrid<UIWeaponSoulInfoGrid>(index);
        if (null != grid)
        {
            grid.SetHightLight(true);
            
        }
        selectedMuhonId = id;
        UpdateCommonCostVisible();
        ResetModulesStatus();
        //刷新UI
        UpdateDataUIByStatus();
        
    }

    private void ResetModulesStatus()
    {
        if (IsInitStatus(status))
        {
            switch (status)
            {
                case TabMode.ShengJi:
                    break;
                case TabMode.JiHuo:
                    ResetActiveRemove();
                    break;
                case TabMode.JinHua:
                    ResetEvolve();
                    break;
                case TabMode.RongHe:
                    ResetBlend();
                    break;
            }
        }
    }
    /// <summary>
    /// 刷新数据列表格子回调
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    void OnUpdateDataListGrid(UIGridBase grid,int index)
    {
        UIWeaponSoulInfoGrid egrid = grid as UIWeaponSoulInfoGrid;
        if (index >= dataList.Count)
        {
            return;
        }
        uint currentId = dataList[index];
        Muhon iData = imgr.GetBaseItemByQwThisId<Muhon>(currentId);
        if (null == iData)
        {
            Engine.Utility.Log.Error("MuhonPanel ->OnUpdateDataListGrid error iData null QwThisId={0}",currentId);
            return ;
        }
        egrid.SetGridViewData(currentId, (currentId == selectedMuhonId), true);
    }

   /// <summary>
   /// 点击列表数据格回调
   /// </summary>
   /// <param name="eType">UIEvent</param>
   /// <param name="data">格子</param>
   /// <param name="param">参数</param>
    void OnClickDataListEventCallback(UIEventType eType,object data,object param)
    {
        switch(eType)
        {
            case UIEventType.Click:

                if (data is UIWeaponSoulInfoGrid)
                {
                    UIWeaponSoulInfoGrid egrid = data as UIWeaponSoulInfoGrid;
                    SetSelectId(egrid.QWThisId);
                }else if (data is UITabGrid)
                {
                    UITabGrid tabGrid = data as UITabGrid;
                    if (tabGrid.Data is TabMode)
                    {
                        if (IsMuhonListEmpty())
                        {
                            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Soul_OwnNoneNotice);
                        }
                        SetStatus((TabMode)tabGrid.Data);
                    }
                }
                break;
           
        }
    }

    void InitWidgets()
    {
        imgr = DataManager.Manager<ItemManager>();
        emgr = DataManager.Manager<EquipManager>();
        tmgr = DataManager.Manager<TextManager>();
        Transform ts = null;
        m_dic_tabs = new Dictionary<TabMode, UITabGrid>();
        m_dic_ts = new Dictionary<TabMode, Transform>();
        if (null != m_trans_FunctionConent)
        {
            for (TabMode i = TabMode.None + 1; i < TabMode.Max; i++)
            {
                ts = m_trans_FunctionConent.Find(i.ToString());
                if (null != ts)
                {
                    m_dic_ts.Add(i, ts);
                }
            }
        }
        InitDataListWiget();
        SetStatus(TabMode.ShengJi);
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void InitStatusdWidgets()
    {
        switch(status)
        {
            case TabMode.RongHe:
                InitCommonCost();
                InitBlendWidgets();
                break;
            case TabMode.JiHuo:
                InitCommonCost();
                InitActivateRemovedWidgets();
                break;
            case TabMode.JinHua:
                InitCommonCost();
                InitEvolveWidgets();
                break;
            case TabMode.ShengJi:
                InitPromoteWidgets();
                break;
        }
    }
    //初始化辅助道具
    private bool m_bool_initCommonCost = false;
    //自动使用点券
    private bool m_bool_autoUseDQ = false;
    private UIItemGrowCostGrid m_itemGrowCostGrid = null;
    public void InitCommonCost()
    {
        if (m_bool_initCommonCost)
        {
            return;
        }

        if (null != m_trans_AssistContentRoot && null == m_itemGrowCostGrid)
        {
            Transform ts = UIManager.AddGridTransform(GridID.Uiitemgrowcostgrid, m_trans_AssistContentRoot);
            if (null != ts)
            {
                m_itemGrowCostGrid = ts.GetComponent<UIItemGrowCostGrid>();
                if (null == m_itemGrowCostGrid)
                {
                    m_itemGrowCostGrid = ts.gameObject.AddComponent<UIItemGrowCostGrid>();
                }
                m_itemGrowCostGrid.RegisterUIEventDelegate(OnUIEventDlg);
            }
        }

        m_bool_initCommonCost = true;
    }

    private void OnUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch(eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemInfoGrid)
                    {
                        UIItemInfoGrid infoGrid = data as UIItemInfoGrid;
                        if (null != infoGrid && infoGrid.NotEnough && null != param && param is uint)
                        {
                            ShowItemGet((uint)param);
                        }
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 重置自动使用点券不足
    /// </summary>
    private void ResetAutoUseDQ()
    {
        m_bool_autoUseDQ = false;
    }

    private void UpdateCommonCostVisible()
    {
        bool empty = IsMuhonListEmpty();
        bool visible = !empty && null != Data
            && ((IsStatus(TabMode.JiHuo) || IsStatus(TabMode.RongHe))
            || ((IsStatus(TabMode.JinHua) && !Data.IsMaxStarLv)));
        if (null != m_trans_CmCostBottom && m_trans_CmCostBottom.gameObject.activeSelf != visible)
        {
            m_trans_CmCostBottom.gameObject.SetActive(visible);
        }
    }
   /// <summary>
   /// 设置辅助道具
   /// </summary>
   /// <param name="baseId">辅助道具id</param>
   /// <param name="needNum">辅助道具数量</param>
   /// <param name="growCost">完成操作所需货币数量</param>
    /// <param name="growCostType">完成操作所需货币类型</param>
    /// <param name="num">元宝代货币数量</param>
   /// <param name="moneyType"></param>
    private void SetCommonCost(uint baseId
        ,uint needNum
        ,uint growCost
        , GameCmd.MoneyType growCostType
        ,uint num,GameCmd.MoneyType moneyType = GameCmd.MoneyType.MoneyType_Gold)
    {
        if (IsStatus(TabMode.ShengJi) || IsMuhonListEmpty() || null == Data)
        {
            return;
        }
        Muhon data = Data;
        if (null != m_itemGrowCostGrid)
        {
            m_itemGrowCostGrid.SetGridData(baseId, num: needNum, useDq: m_bool_autoUseDQ, costNum: num,mType: moneyType);
        }

        if (null != m_btn_AutoUseDQ)
        {
            m_btn_AutoUseDQ.GetComponent<UIToggle>().value = m_bool_autoUseDQ;
        }
        bool cansee = IsStatus(TabMode.JiHuo) && !data.IsActive;
        if (null != m_btn_ActiveBtn && m_btn_ActiveBtn.gameObject.activeSelf != cansee)
        {
            m_btn_ActiveBtn.gameObject.SetActive(cansee);
        }
        cansee = IsStatus(TabMode.JiHuo) && data.IsActive;
        if (null != m_btn_RemoveBtn && m_btn_RemoveBtn.gameObject.activeSelf != cansee)
        {
            m_btn_RemoveBtn.gameObject.SetActive(cansee);
        }
        cansee = IsStatus(TabMode.JinHua);
        if (null != m_btn_EvolveBtn && m_btn_EvolveBtn.gameObject.activeSelf != cansee)
        {
            m_btn_EvolveBtn.gameObject.SetActive(cansee);
        }

        cansee = IsStatus(TabMode.RongHe);
        if (null != m_btn_BlendBtn && m_btn_BlendBtn.gameObject.activeSelf != cansee)
        {
            m_btn_BlendBtn.gameObject.SetActive(cansee);
        }

        if (null != m_trans_OPCost)
        {
            UICurrencyGrid currencyGrid = m_trans_OPCost.GetComponent<UICurrencyGrid>();
            if (null == currencyGrid)
            {
                currencyGrid = m_trans_OPCost.gameObject.AddComponent<UICurrencyGrid>();
                
            }
            currencyGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Gold), growCost));
        }

    }

    private bool IsStatus(MuhonPanel.TabMode status)
    {
        return this.status == status;
    }

    /// <summary>
    /// 设置页签状态
    /// </summary>
    /// <param name="status">页签状态</param>
    /// <param name="force">是否强制更新</param>
    private void SetStatus(MuhonPanel.TabMode status, bool force = false)
    {
        SetEvolvePre(false);
        if (this.status == status && !force)
            return;
        if (this.status == TabMode.JinHua)
        {
            ResetEvolveAnim();
        }
        ResetAutoUseDQ();
        ResetModulesStatus();
        if (null != m_dic_tabs)
        {
            UITabGrid tab = null;
            if (m_dic_tabs.TryGetValue(this.status,out tab))
            {
                tab.SetHightLight(false);
            }
            if (m_dic_tabs.TryGetValue(status, out tab))
            {
                tab.SetHightLight(true);
            }

        }
        this.status = status;
        UpdateWidgetsVisbleStatus();
        if (!IsMuhonListEmpty())
        {
            InitStatusdWidgets();
            UpdateDataUIByStatus();
        }
        
    }
    /// <summary>
    /// 更新功能toggle可视状态
    /// </summary>
    private void UpdateMuhonToggleVisible()
    {
        UITabGrid tabGrid = null;
        bool visible = false;
        for (TabMode i = TabMode.ShengJi; i < TabMode.Max; i++)
        {

            visible = IsStatus(i);
            if (null != m_dic_tabs && m_dic_tabs.TryGetValue(i, out tabGrid))
            {
                tabGrid.SetHightLight(visible);
            }
        }
    }
    /// <summary>
    /// 刷新组件可视状态
    /// </summary>
    public void UpdateWidgetsVisbleStatus()
    {
        if (null != m_dic_ts)
        {
            Transform ts = null;
            bool visible = false;
            bool empty = IsMuhonListEmpty();
            for (TabMode i = TabMode.None + 1; i < TabMode.Max; i++)
            {
                visible = (i == this.status) && !empty;
                if (m_dic_ts.TryGetValue(i,out ts) && ts.gameObject.activeSelf != visible)
                {
                    ts.gameObject.SetActive(visible);
                }
            }

            UpdateCommonCostVisible();

            if (null != m_trans_NullMuhonTipsContent
                && m_trans_NullMuhonTipsContent.gameObject.activeSelf != empty)
            {
                m_trans_NullMuhonTipsContent.gameObject.SetActive(empty);
            }

        }
     }

    /// <summary>
    /// 根据页签状态刷新UI
    /// </summary>
    public void UpdateDataUIByStatus()
    {
        if (selectedMuhonId == 0)
            return ;
        Muhon muhon = imgr.GetBaseItemByQwThisId<Muhon>(selectedMuhonId);
        switch (status)
        {
            case TabMode.ShengJi:
                UpdatePromote(muhon);
                break;
            case TabMode.JiHuo:
                UpdateActivateRemove(muhon);
                break;
            case TabMode.JinHua:
                UpdateEvolve(muhon);
                break;
            case TabMode.RongHe:
                UpdateBlend(muhon);
                break;
        }
    }
    #endregion

    #region UIEvent

    private void onClick_AutoUseDQ_Btn(GameObject obj)
    {
        if (null != m_btn_AutoUseDQ && m_btn_AutoUseDQ.GetComponent<UIToggle>() != null)
        {
            UIToggle toggle = m_btn_AutoUseDQ.GetComponent<UIToggle>();
            if (null != toggle)
            {
                m_bool_autoUseDQ = toggle.value;
            }
            UpdateDataUIByStatus();
        }
    }
    #endregion

    #region Promote(升级)
    //MuhonPanel_Upgrade.cs
    #endregion

    #region ActivateRemove(激活/消除)
    //MuhonPanell_ActiveRemove.cs
    #endregion

    #region Evolve(进化)
    //MuhonPanel_Evolve.cs
    #endregion

    #region Blend(融合)
    //MuhonPanel_Blend.cs
    #endregion

    #region FunctionToggle(功能开关)
    void onClick_PromoteToggle_Btn(GameObject caster)
    {
        //if (!UIEventEnable)
        //    return;
        SetStatus(TabMode.ShengJi);
    }

    void onClick_ActivateToggle_Btn(GameObject caster)
    {
        //if (!UIEventEnable)
        //    return;
        SetStatus(TabMode.JiHuo);
    }

    void onClick_EvolveToggle_Btn(GameObject caster)
    {
        //if (!UIEventEnable)
        //    return;
        SetStatus(TabMode.JinHua);
    }

    void onClick_BlendToggle_Btn(GameObject caster)
    {
        //if (!UIEventEnable)
        //    return;
        SetStatus(TabMode.RongHe);
    }
    #endregion

    #region IUIAnimation
    //动画In
    public override void AnimIn(EventDelegate.Callback onComplete)
    {
        base.AnimIn(onComplete);
    }
    //动画Out
    public override void AnimOut(EventDelegate.Callback onComplete)
    {
        base.AnimOut(onComplete);
    }
    //重置动画
    public override void ResetAnim()
    {
        base.ResetAnim();
        //if (null != tp)
        //    tp.ResetToBeginning();
    }
    #endregion
   public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (UIMsgID.eShowUI == msgid)
        {
            if (param is ReturnBackUIMsg)
            {
                ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
                MuhonData data = new MuhonData();
                if (showInfo.tabs.Length > 0)
                {
                    data.m_em_mode = (TabMode)showInfo.tabs[0];
                    UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, showInfo.tabs[0]);
                }
                if (null != showInfo.param && showInfo.param is uint)
                {
                    data.selectId = (uint)showInfo.param;
                }
                OnShow(data);
            }
            
        }
        return base.OnMsg(msgid, param);
    }
}