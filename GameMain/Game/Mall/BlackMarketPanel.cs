using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class BlackMarketPanel
{
    #region property

    //商城物品字典
    private List<uint> mallItemPosIds = null;
    //View
    private Dictionary<GameCmd.CommonStore, UITabGrid> m_dic_tabGrid = null;
    #endregion

    #region overridemethod

    protected override void OnEnable()
    {
        base.OnEnable();
        //立即刷新时间
        ImmediatelyRefreshLeftTime();
    }
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGolbalUIEvent(true);
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];
        }
        else
        {
            firstTabData = (int)TabMode.YunYou;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[1];
        MallManager mgr = DataManager.Manager<MallManager>();
        switch (mgr.BlackMarketActiveStore)
        {
            case GameCmd.CommonStore.CommonStore_HundredThree:
                pd.JumpData.Tabs[0] = (int)TabMode.YunYou;
                break;
            //case GameCmd.CommonStore.CommonStore_HundredOne:
            //    pd.JumpData.Tabs[0] = (int)TabMode.ShengWang;
            //    break;
            //case GameCmd.CommonStore.CommonStore_HundredTwo:
            //    pd.JumpData.Tabs[0] = (int)TabMode.JiFen;
            //    break;
            //case GameCmd.CommonStore.CommonStore_HundredFour:
            //    pd.JumpData.Tabs[0] = (int)TabMode.ZhanXun;
            //    break;
            //case GameCmd.CommonStore.CommonStore_HundredFive:
            //    pd.JumpData.Tabs[0] = (int)TabMode.ShouLie;
            //    break;
            //default:
            //    pd.JumpData.Tabs[0] = (int)TabMode.ShengWang;
            //    break;
        }
        return pd;
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGolbalUIEvent(false);
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_bgv_BlockScorllView)
        {
            m_bgv_BlockScorllView.Release(depthRelease);
        }
        if (null != m_currencyCASD)
        {
            m_currencyCASD.Release(true);
            m_currencyCASD = null;
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        
    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            if (Enum.IsDefined(typeof(TabMode),pageid))
            {
                switch(pageid)
                {
                    case (int)TabMode.YunYou:
                        DataManager.Manager<MallManager>().SetBlackMarkeActiveStore(GameCmd.CommonStore.CommonStore_HundredThree, true);
                        break;
                    //case (int)TabMode.ShengWang:
                    //    DataManager.Manager<MallManager>().SetBlackMarkeActiveStore(GameCmd.CommonStore.CommonStore_HundredOne, true);
                    //    break;
                    //case (int)TabMode.JiFen:
                    //    DataManager.Manager<MallManager>().SetBlackMarkeActiveStore(GameCmd.CommonStore.CommonStore_HundredTwo, true);
                    //    break;
                    //case (int)TabMode.ZhanXun:
                    //    DataManager.Manager<MallManager>().SetBlackMarkeActiveStore(GameCmd.CommonStore.CommonStore_HundredFour, true);
                    //    break;
                    //case (int)TabMode.ShouLie:
                    //    DataManager.Manager<MallManager>().SetBlackMarkeActiveStore(GameCmd.CommonStore.CommonStore_HundredFive, true);
                    //    break;
                }
            }
        }
        return base.OnTogglePanel(tabType, pageid);
    }


    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (UIMsgID.eShowUI == msgid)
        {
            if (param is ReturnBackUIMsg)
            {
                ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
                if (showInfo.tabs.Length > 0)
                {
                    UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, showInfo.tabs[0]);
                }
            }
        }
        return base.OnMsg(msgid, param);
    }

    #endregion

    #region Update
    //更新商城刷新间隔
    public const float BLACKMARKET_REFRESH_LEFTTIME_GAP = 1F;
    //下一次刷新黑市时间标签剩余的时间
    private float next_refresh_left_time = 0;
    private void ImmediatelyRefreshLeftTime()
    {
        next_refresh_left_time = 0;
    }
    void Update()
    {
        next_refresh_left_time -= Time.deltaTime;
        if (next_refresh_left_time <= UnityEngine.Mathf.Epsilon)
        {
            next_refresh_left_time = BLACKMARKET_REFRESH_LEFTTIME_GAP;
            OnRefreshBlackMarketTimeCountDown(DataManager.Manager<MallManager>().GetBlackMallNextRefreshLeftTimeStr());
        }
    }


    #endregion

    #region Op
    private CMResAsynSeedData<CMAtlas> m_currencyCASD = null;
    /// <summary>
    /// 刷新剩余钱币爽
    /// </summary>
    private void UpdateCurrencyLeft()
    {
        GameCmd.CommonStore activeStore = DataManager.Manager<MallManager>().BlackMarketActiveStore;
        bool visible = (activeStore == GameCmd.CommonStore.CommonStore_HundredThree)? false : true;
        if (null != m_trans_LeftCurrencyContent 
            && m_trans_LeftCurrencyContent.gameObject.activeSelf != visible)
        {
            m_trans_LeftCurrencyContent.gameObject.SetActive(visible);
        }
        if (!visible)
        {
            return;
        }
        Client.IPlayer player = DataManager.Instance.MainPlayer;
        if (null == player)
        {
            return;
        }
        uint num = 0;
        string iconName = "";
        switch(activeStore)
        {
            case GameCmd.CommonStore.CommonStore_HundredOne:
                iconName = MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Reputation);
                num = (uint)player.GetProp((int)Client.PlayerProp.Reputation);
                break;
            case GameCmd.CommonStore.CommonStore_HundredTwo:
                iconName = MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_Score);
                num = (uint)player.GetProp((int)Client.PlayerProp.Score);
                break;
            case GameCmd.CommonStore.CommonStore_HundredFour:
                iconName = MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_CampCoin);
                num = (uint)player.GetProp((int)Client.PlayerProp.CampCoin);
                break;
            case GameCmd.CommonStore.CommonStore_HundredFive:
                iconName = MallDefine.GetCurrencyIconNameByType(GameCmd.MoneyType.MoneyType_HuntingCoin);
                num = (uint)player.GetProp((int)Client.PlayerProp.ShouLieScore);
                break;
        }
        if (null != m_sprite_CurrncyIcon)
        {
            UIManager.GetAtlasAsyn(iconName, ref m_currencyCASD, () =>
                {
                    if (null != m_sprite_CurrncyIcon)
                    {
                        m_sprite_CurrncyIcon.atlas = null;
                    }
                }, m_sprite_CurrncyIcon);
        }
        if (null != m_label_CurrncyNum)
        {
            m_label_CurrncyNum.text = num.ToString();
        }
    }

    /// <summary>
    /// 全局UI事件Handler
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    private void OnGlobalUIEventHandler(int eventType,object data)
    {
        switch(eventType)
        {
            case (int)Client.GameEventID.UIEVENTBLACKMARKETREFRESH:
                OnRefreshBlackMarket((bool) data);
                break;
            case (int)Client.GameEventID.UIEVENTBLACKACTIVESTORECHANGED:
                OnRefreshBlackMarket(true);
                break;
            case (int)Client.GameEventID.UIEVENTBLACKACTIVETABCHANGED:
                OnActiveTableChanged((int)data);
                break;
            case (int)Client.GameEventID.UIEVENTBLACKMARKETREFRESHTIMECOUNTDOWN:
                OnRefreshBlackMarketTimeCountDown((string) data);
                break;
            case (int)Client.GameEventID.UIEVENTBLACKMARKETITEMSOLDOUT:
                OnBlackMarketItemSoldOut((uint)data);
                break;
            case (int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM:
                UpdateCurrencyLeft();
                break;
        }
    }

    /// <summary>
    /// 刷新流动商城
    /// </summary>
    /// <param name="create">是否为创建</param>
    private void OnRefreshBlackMarket(bool create)
    {
        ImmediatelyRefreshLeftTime();
        if (create || null == mallItemPosIds || mallItemPosIds.Count == 0)
        {
            RebuildBlackMarket();
        }
        else if (null != m_bgv_BlockScorllView)
        {
            int oldCount = mallItemPosIds.Count;
            mallItemPosIds.Clear();
            mallItemPosIds.AddRange(DataManager.Manager<MallManager>().GetBlackMarketPosInfoByMallType());
            int needCreate = mallItemPosIds.Count - oldCount;
            
            if (needCreate <0)
            {
                for (int i = oldCount - 1; i >= mallItemPosIds.Count; i--)
                {
                    m_bgv_BlockScorllView.Remove(i);
                }
            }else if (needCreate > 0)
            {
                for (int i = oldCount; i < mallItemPosIds.Count; i++)
                {
                    m_bgv_BlockScorllView.Insert(i);
                }
            }
            m_bgv_BlockScorllView.UpdateActiveGrid();
        }

    }

    /// <summary>
    /// 活动页签改变
    /// </summary>
    /// <param name="activeIndex"></param>
    private void OnActiveTableChanged(int activeIndex)
    {
        OnRefreshBlackMarket(true);
        UpdateCurrencyLeft();
    }

    /// <summary>
    /// 重构流动商城
    /// </summary>
    private void RebuildBlackMarket()
    {
        UpdateTableStatus();
        StructData();
        CreateBlackMarketUI();
        UpdateCurrencyLeft();
    }

    /// <summary>
    /// 刷新流动商城倒计时
    /// </summary>
    /// <param name="timeLeftStr"></param>
    private void OnRefreshBlackMarketTimeCountDown(string timeLeftStr)
    {
        if (null != m_label_RefreshTimeCountDown)
        {
            m_label_RefreshTimeCountDown.text = timeLeftStr;
        }
    }

    /// <summary>
    /// 商品售罄回调
    /// </summary>
    /// <param name="mallItemId"></param>
    private void OnBlackMarketItemSoldOut(uint mallItemId)
    {
        //if (null != gridCreator)
        //{
        //    UIBlackMarketGrid blackGrid = gridCreator.GetGrid(mallItemIds.IndexOf(mallItemId));
        //    if (null != blackGrid)
        //        blackGrid.SetSoldOut(DataManager.Manager<MallManager>().IsMallItemSoldOut(mallItemId));
        //}
    }
    /// <summary>
    /// 注册UI全局事件
    /// </summary>
    /// <param name="register"></param>
    private void RegisterGolbalUIEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTBLACKMARKETREFRESHTIMECOUNTDOWN, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTBLACKMARKETREFRESH, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTBLACKMARKETITEMSOLDOUT, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTBLACKACTIVESTORECHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTBLACKACTIVETABCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnGlobalUIEventHandler);
        }else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTBLACKMARKETREFRESHTIMECOUNTDOWN, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTBLACKMARKETREFRESH, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTBLACKMARKETITEMSOLDOUT, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTBLACKACTIVESTORECHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTBLACKACTIVETABCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnGlobalUIEventHandler);
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void InitWidgets()
    {
        //初始化面板页签
        if (null == m_dic_tabGrid)
        {
            m_dic_tabGrid = new Dictionary<GameCmd.CommonStore, UITabGrid>();
        }

        m_dic_tabGrid.Clear();
        RebuildBlackMarket();
    }

    /// <summary>
    /// 更新页签状态
    /// </summary>
    private void UpdateTableStatus()
    {
        GameCmd.CommonStore activeStore = DataManager.Manager<MallManager>().BlackMarketActiveStore;
        foreach(KeyValuePair<GameCmd.CommonStore,UITabGrid> pair in m_dic_tabGrid)
        {
            pair.Value.SetHightLight(pair.Key == activeStore);
        }
    }

    /// <summary>
    /// 构建数据
    /// </summary>
    private void StructData()
    {
        if (null == mallItemPosIds)
            mallItemPosIds = new List<uint>();
        mallItemPosIds.Clear();
        mallItemPosIds.AddRange(DataManager.Manager<MallManager>().GetBlackMarketPosInfoByMallType());
    }

    /// <summary>
    /// 创建流动商城UI
    /// </summary>
    private void CreateBlackMarketUI()
    {
        CreateTabUI();
        CreateDataList();
        OnRefreshBlackMarketTimeCountDown(DataManager.Manager<MallManager>().GetBlackMallNextRefreshLeftTimeStr());
    }

    /// <summary>
    /// 创建分页
    /// </summary>
    private void CreateTabUI()
    {
        
    }
    
    private void OnUpdateMallGridData(UIGridBase grid,int index)
    {
        if (index >= mallItemPosIds.Count)
        {
            Engine.Utility.Log.Error("MallPanel OnUpdateMallGridData faield,mallItemPosIds data errir");
            return;
        }
        GameCmd.DynaStorePosInfo info = DataManager.Manager<MallManager>().GetBlackMarketPosInfo(mallItemPosIds[index]);
        if (null == info)
        {
            Engine.Utility.Log.Error("MallPanel OnUpdateMallGridData faield,mallItemPosIds index errir");
            return ;
        }
        UIBlackMarketGrid bGrid = grid as UIBlackMarketGrid;
        bGrid.SetGridData(info);
    }

    private bool m_bInitBlock = false;
    /// <summary>
    /// 创建数据列表
    /// </summary>
    private void CreateDataList()
    {
        if (null != m_bgv_BlockScorllView && null != m_trans_UIBlackMarketGrid)
        {
            if (!m_bInitBlock)
            {
                m_bInitBlock = true;
                m_bgv_BlockScorllView.Initialize<UIBlackMarketGrid>(
                        m_trans_UIBlackMarketGrid.gameObject
                        , m_trans_UIBlockIndexGrid.gameObject
                        , new Vector2(2, 3)
                        , new Vector2(350, 249)
                        , OnUpdateMallGridData
                        , OnGridUIEvent
                        , new Vector3(1, -1, 0)
                        , Vector2.zero
                        , pageGap: 1164);
            }
        
            if (null == mallItemPosIds)
            {
                Engine.Utility.Log.Error("BlackMarketPanel CreateDataList faield,mallItemPosIds null");
                return;
            }
            m_bgv_BlockScorllView.CreateView(mallItemPosIds.Count);
        }
        
    }

    /// <summary>
    /// 格子UI事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEvent(UIEventType eventType,object data,object param)
    {
        switch(eventType)
        {
            case UIEventType.Click:
                if (data is UIBlackMarketGrid)
                {
                    if (null != param)
                    {
                        if (param is UIItemInfoGrid)
                        {
                            UIBlackMarketGrid market = data as UIBlackMarketGrid;
                            MallDefine.MallLocalData localData = DataManager.Manager<MallManager>().GetMallLocalDataByMallItemId(market.MallInfo.baseid);
                            if (null != localData)
                            {
                                TipsManager.Instance.ShowItemTips(localData.LocalItem.BaseId);
                            }
                            
                        }else if (param is GameCmd.DynaStorePosInfo)
                        {
                            PurchaseMallItem((GameCmd.DynaStorePosInfo)param);
                        }
                    }
                        
                }else if (data is UICirclePointGrid)
                {
                    UICirclePointGrid cGrid = data as UICirclePointGrid;
                    if (null != cGrid.Data )
                    {
                        DataManager.Manager<MallManager>().SetActiveTabIndex((int)cGrid.Data);
                    }
                }else if (data is UITabGrid)
                {
                    UITabGrid tGrid = data as UITabGrid;
                    if (null != tGrid.Data)
                    {
                        GameCmd.CommonStore store = (GameCmd.CommonStore)tGrid.Data;
                        DataManager.Manager<MallManager>().SetBlackMarkeActiveStore(store);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 购买商品
    /// </summary>
    /// <param name="mallItemId"></param>
    private void PurchaseMallItem(GameCmd.DynaStorePosInfo info)
    {
        DataManager.Manager<MallManager>().PurchaseMallItem(info.baseid, (uint)DataManager.Manager<MallManager>().BlackMarketActiveStore, 1,info.pos);
    }

    #endregion

    #region UIEvent

    void onClick_RefreshBtn_Btn(GameObject caster)
    {
        MallDefine.BlackMarketRefreshData refreshData = DataManager.Manager<MallManager>().GetBlackMallRefreshData();
        int leftTimes = Mathf.Max(0, refreshData.RefreshCount -
            (int)DataManager.Manager<MallManager>().GetBlackMarketHandRefreshTimes());
        if (leftTimes <= 0)
        {
            TipsManager.Instance.ShowTips("今日刷新次数已用完");
            return;
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.BlackMarketRefreshPanel
            ,data: new BlackMarketRefreshPanel.BlackMarkeRefreshData()
        {
            LeftTimes = leftTimes,
            Data = refreshData.RefreshCost,
            TotalTimes = refreshData.RefreshCount,
        });
    }
    #endregion
}