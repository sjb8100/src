using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;

partial class RidePanel : UIPanelBase
{
    //public enum TabMode
    //{
    //    None = 0,
    //    MaJiu = 1,
    //    Page_技能 = 2,
    //    Page_传承 = 3,
    //    TuJian = 4,
    //    Max,
    //}

    //Uiridegrid
    private UIGridCreatorBase m_UIGridCreatorBase = null;
    List<RideData> m_lstRideData = null;
    UIRideGrid m_preUIRideGrid = null;
    UIRideGrid m_currUIRideGrid = null;
    //-----------------------------------------------------------------------------------------------
    //Dictionary<uint, UITabGrid> m_dicTabs = new Dictionary<uint, UITabGrid>();
    TabMode m_Content = TabMode.None;

    RideManager m_rideMgr = null;
    RideData m_currRideData = null;
    Transform m_leftcontent = null;

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();
        m_rideMgr = DataManager.Manager<RideManager>();

        m_trans_AdorationContent.gameObject.SetActive(false);
        m_trans_RidePropContent.gameObject.SetActive(false);
        m_trans_SkillContent.gameObject.SetActive(false);
        m_widget_content.gameObject.SetActive(true);
        m_widget_tujiancontent.gameObject.SetActive(true);
        m_trans_RideStrategeContent.gameObject.SetActive(false);
        UIEventListener.Get(m__selecthorse.gameObject).onClick = OnSelectHorse;
        //EventDelegate.Add(m_toggle_ptchuancheng.onChange, () =>
        //{
        //    if (m_toggle_ptchuancheng.value)
        //    {
        //        this.OnTransExp(m_toggle_ptchuancheng.name);
        //    }
        //});
        //EventDelegate.Add(m_toggle_wmchuancheng.onChange, () =>
        //{
        //    if (m_toggle_wmchuancheng.value)
        //    {
        //        this.OnTransExp(m_toggle_wmchuancheng.name);
        //    }
        //});
        m_leftcontent = m_widget_content.transform.Find("left");
        LoadingPropetyUI();
        //  InitSkillContent();


        //  InitPageTab();
    }
    void ShowLeftContent(bool bShow)
    {
        if(m_leftcontent != null)
        {
            m_leftcontent.gameObject.SetActive(bShow);
        }
    }
    #region Page Tab
    TabMode m_curMode = TabMode.None;
    public override bool OnTogglePanel(int nTabType, int pageid)
    {
        if (nTabType == UIPanelBase.FisrstTabsIndex)
        {
            TabMode type = (TabMode)pageid;

            if (type == TabMode.MaJiu)
            {
                if (m_rideMgr.GetRideList().Count <= 0)
                {
                    TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_nihaimeiyouzuoqibunengdakaizuoqijiemian);
                    return false;
                }
            }
            m_curMode = type;
            ToggleRideContent(type, false);
        }

        return base.OnTogglePanel(nTabType, pageid);
    }

    protected override void OnJump(PanelJumpData jumpData)
    {
        base.OnJump(jumpData);

        int firstTab = -1;
        if (jumpData == null)
        {
            jumpData = new PanelJumpData();
        }
        if (firstTab == -1)
        {
            firstTab = (null != jumpData.Tabs && jumpData.Tabs.Length >= 1) ? jumpData.Tabs[0] : (int)TabMode.QiShu;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTab);

        if (firstTab == (int)TabMode.MaJiu && jumpData.Param != null)
        {
            uint rideId = (uint)jumpData.Param;
            List<UIRideGrid> lstGrid = m_UIGridCreatorBase.GetGrids<UIRideGrid>();
            for (int i = 0; i < lstGrid.Count; i++)
            {
                if (lstGrid[i].RideData.id == rideId)
                {
                    OnRideGridUIEvent(UIEventType.Click, lstGrid[i], null);
                }
            }
        }
    }

    public override PanelData GetPanelData()
    {

        PanelData pd = new PanelData()
        {
            PID = PanelId,
            Data = cacheData,
            JumpData = new PanelJumpData() { Tabs = new int[] { (int)m_curMode } },
        };

        return pd;
    }
    private void OnUIPageEventCallback(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UITabGrid)
                {
                    UITabGrid tabGRid = data as UITabGrid;
                    if (tabGRid.Data is TabMode)
                    {

                        TabMode type = (TabMode)tabGRid.Data;
                        if (type != TabMode.TuJian)
                        {
                            if (m_rideMgr.GetRideList().Count <= 0)
                            {
                                TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_nihaimeiyouzuoqibunengdakaizuoqijiemian);
                                return;
                            }
                        }
                        if (param != null && param is bool)
                        {
                            bool force = (bool)param;
                            ToggleRideContent(type, force);
                        }
                        else
                        {
                            ToggleRideContent(type, false);
                        }

                    }
                }
                break;
            default:
                break;
        }
    }
    private void ToggleRideContent(TabMode btnType, bool force = false)
    {
        if (m_Content == btnType && !force)
        {
            return;
        }
        //if (m_Content == TabMode.Page_传承 && btnType != TabMode.Page_传承)
        //{
        //    ResetUIRideGridAdoration();
        //}
        //SetRidePageBtnState(btnType,force);

        m_widget_content.alpha = btnType != TabMode.TuJian ? 1f : 0f;
        m_widget_tujiancontent.alpha = btnType == TabMode.TuJian ? 1f : 0f;

        if (m_widget_content.alpha == 1f)
        {

            //   m_trans_AdorationContent.gameObject.SetActive(btnType == TabMode.Page_传承 );
            m_trans_RidePropContent.gameObject.SetActive(btnType == TabMode.MaJiu);

            m_trans_RideStrategeContent.gameObject.SetActive(btnType == TabMode.QiShu);
            //  m_trans_SkillContent.gameObject.SetActive(btnType == TabMode.Page_技能);
        }
        m_Content = btnType;
        switch (m_Content)
        {
            case TabMode.QiShu:
                {
                    ShowLeftContent(false);
                    InitKnightUI();
                }
                break;
            case TabMode.MaJiu:
                {
                  
                    ShowLeftContent(true);
                    OnCreateRideGrid();
                    InitRideList();
                    if (m_currUIRideGrid == null)
                    {
                        SelectFirst();
                    }
                    else
                    {
                        OnRideGridUIEvent(UIEventType.Click, m_currUIRideGrid, null);
                    }
                  
                }
                break;
            //case TabMode.Page_传承:
            //    {
            //        if (m_currUIRideGrid != null)
            //        {
            //            m_currUIRideGrid.SetSelect(false);
            //        }

            //        InitAdoration();
            //    }
            //    break;
            case TabMode.TuJian:
                {
                    ShowLeftContent(false);
                    ResetAdoration();
                    InitRidePreViewUI();
                }
                break;
            default:
                break;
        }


    }

    #endregion

    #region RideGrid

    void OnCreateRideGrid()
    {
        m_UIGridCreatorBase = m_trans_Ridescrollview.GetComponent<UIGridCreatorBase>();
        if (m_UIGridCreatorBase == null)
            m_UIGridCreatorBase = m_trans_Ridescrollview.gameObject.AddComponent<UIGridCreatorBase>();
        m_UIGridCreatorBase.gridContentOffset = new UnityEngine.Vector2(0, 0);
        m_UIGridCreatorBase.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
        m_UIGridCreatorBase.gridWidth = 295;
        m_UIGridCreatorBase.gridHeight = 105;
        m_UIGridCreatorBase.rowcolumLimit = 1;
        m_UIGridCreatorBase.RefreshCheck();
        m_UIGridCreatorBase.Initialize<UIRideGrid>(m_sprite_UIRideGrid.gameObject, OnRidtGridDataUpdate, OnRideGridUIEvent);
    }

    void OnRidtGridDataUpdate(UIGridBase data, int index)
    {
        if (m_lstRideData != null && index < m_lstRideData.Count)
        {
            data.SetGridData(m_lstRideData[index]);
        }
    }

    void OnRideGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                UIRideGrid grid = data as UIRideGrid;
                if (grid != null)
                {
                    if (grid.RideData.id == 0)
                    {
                        if (grid.LastGrid)
                        {
                            //打开解锁提示
                            table.RideExpandData expand = GameTableManager.Instance.GetTableItem<table.RideExpandData>((uint)DataManager.Manager<RideManager>().ExpandNum);
                            TextManager tmg = DataManager.Manager<TextManager>();
                            if (expand != null)
                            {

                                TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO
                                    , tmg.GetLocalFormatText(LocalTextType.Ride_Commond_jiesuoxuyaoxiaohaoXdianjuan, expand.cost),
                                    () => { DataManager.Instance.Sender.RideExpandMaxRide(); }, null, null, "增加坐骑栏");
                            }
                        }
                        else
                        {
                            TipsManager.Instance.ShowTips("更多炫酷坐骑可通过商城获得");
                        }
                        return;
                    }


                    m_preUIRideGrid = m_currUIRideGrid;
                    if (m_preUIRideGrid != null)
                    {
                        m_preUIRideGrid.SetSelect(false);
                    }
                    m_currUIRideGrid = grid;
                    m_currUIRideGrid.SetSelect(true);

                    m_currRideData = grid.RideData;


                    if (m_Content == TabMode.MaJiu)
                    {
                        InitPropetyUI(m_currRideData);

                    }
                    //else if (m_Content == TabMode.Page_技能)
                    //{
                    //    InitSkillUI(m_currRideData);
                    //}
                    //else if (m_Content == TabMode.Page_传承)
                    //{
                    //    int select = OnSelectRide(grid);
                    //    if (select != 0)
                    //    {
                    //        grid.TransExpSelect = select;
                    //        if (select == 1)
                    //        {
                    //            grid.SetSufferingState(true);
                    //        }
                    //        else if (select == 2)
                    //        {
                    //            grid.SetAdirationState(true);
                    //        }
                    //    }
                    //}

                }
                break;
        }
    }

    #endregion


    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_rideMgr.RidePropUpdateCallback = RefreshProptyUI;
        m_rideMgr.ValueUpdateEvent += OnValueUpdateEventArgs;

        if (m_rideMgr.GetRideList().Count > 0)
        {
            InitRideList();
        }


    }

    void InitRideList()
    {
        m_lstRideData = null;
        m_lstRideData = m_rideMgr.GetRideList();

        //多加一个用于扩展
        for (int i = m_lstRideData.Count; i < m_rideMgr.MaxRideNum + 1; i++)
        {
            RideData rideNull = new RideData() { baseid = 0, id = 0, index = (uint)i };
            m_lstRideData.Add(rideNull);
        }

        if (m_UIGridCreatorBase != null)
        {
            m_UIGridCreatorBase.CreateGrids(m_lstRideData.Count);
        }
    }

    void OnClickToggleBtn(GameObject go)
    {
        if (go.GetComponent<UIToggle>().enabled)
        {
            return;
        }

        TipsManager.Instance.ShowTipsById(113515);
    }

    protected override void OnHide()
    {
        base.OnHide();
        Release();
        ReleseKnightTexture();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        m_currRideData = null;
        m_rideMgr.RidePropUpdateCallback = null;
        m_rideMgr.ValueUpdateEvent -= OnValueUpdateEventArgs;

        if (m_preUIRideGrid != null)
        {
            m_preUIRideGrid.Release();
            m_preUIRideGrid.SetSelect(false);
            m_preUIRideGrid = null;
        }

        if (m_currUIRideGrid != null)
        {
            m_currUIRideGrid.Release();
            m_currUIRideGrid.SetSelect(false);
            m_currUIRideGrid = null;
        }

        if (null != iconOldAtlas)
        {
            iconOldAtlas.Release(true);
            iconOldAtlas = null;
        }

        if (null != iconNewdAtlas)
        {
            iconNewdAtlas.Release(true);
            iconNewdAtlas = null;
        }

        if (null != borderOldAtlas)
        {
            borderOldAtlas.Release(true);
            borderOldAtlas = null;
        }

        if (null != borderNewAtlas)
        {
            borderNewAtlas.Release(true);
            borderNewAtlas = null;
        }

        if (null != m_UIGridCreatorBase)
        {
            m_UIGridCreatorBase.Release();
        }

        if (null != m_ctor_tujianscroll)
        {
            m_ctor_tujianscroll.Release();
        }

        if (null != m_ctor_rideQRoot)
        {
            m_ctor_rideQRoot.Release();
        }
        if (particle != null)
        {
            particle.ReleaseParticle();
        }

        m_Content = TabMode.None;

        PropetyRelease();

        ResetAdoration();
    }
    void RefreshProptyUI(RideData ridedata)
    {
        if (m_currRideData != null && ridedata.id == m_currRideData.id)
        {
            for (int i = 0; i < m_lstRideData.Count; i++)
            {
                if (m_lstRideData[i].id == ridedata.id)
                {
                    m_lstRideData[i] = ridedata;
                    m_currRideData = ridedata;
                    break;
                }
            }
            if (m_Content == TabMode.MaJiu)
            {
                InitPropetyUI(m_currRideData);
            }
            //else if(m_Content == TabMode.Page_技能)
            //{
            //    InitSkillUI(m_currRideData);
            //}
            if (m_currUIRideGrid != null)
            {
                m_currUIRideGrid.SetGridData(m_currRideData);
            }

            //刷新吃经验面板
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.UseItemCommonPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.UseItemCommonPanel, UIMsgID.eUseItemRefresh, UseItemCommonPanel.UseItemEnum.RideExp);
            }
        }
    }

    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs value)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        if (value.key == "ExpandeMaxNum")
        {
            InitRideList();
        }
        else if (value.key == "RideUpdatList")
        {
            InitRideList();
            SelectFirst();
        }
        else if (value.key == "RideFightState")//出战状态按钮刷新
        {
            if (m_currRideData != null)
            {
                UpdateFightState();
            }
        }
        else if (value.key == "ResetAdoration")
        {
            InitRideList();
            InitAdoration();
        }
        else if (value.key == RideManager.RideDispatchEnum.RefreshKnightLevel.ToString()
            || value.key == RideManager.RideDispatchEnum.RefreshKnightExp.ToString())
        {
            RefreshKnightLevelAndBreak();
            ShowRedPoint();
        }
        else if (value.key == RideManager.RideDispatchEnum.RefreshKnightPower.ToString())
        {
            RefreshPower();
        }
        else if (value.key == RideManager.RideDispatchEnum.RefreshKnightBreakLevel.ToString())
        {
            RefreshKnightLevelAndBreak();
        }
        else if(value.key == RideManager.RideDispatchEnum.RefreshRedPoint.ToString())
        {
            ShowRedPoint();
        }

    }

    private void SelectFirst()
    {
        if (m_rideMgr.GetRideList().Count <= 0)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_nihaimeiyouzuoqibunengdakaizuoqijiemian);
            //UIPanelBase panelbase = this as UIPanelBase;
            UIFrameManager.Instance.OnCilckTogglePanel(PanelID.RidePanel, (int)UIPanelBase.FisrstTabsIndex, (int)TabMode.TuJian);
            //UIFrameManager.Instance.OnCilckTogglePanel(ref panelbase, 1, (int)TabMode.TuJian);
            //OnUIPageEventCallback(UIEventType.Click, m_dicTabs[(uint)RidePageEnum.RidePreView], true);
        }
        else if (m_UIGridCreatorBase != null)
        {
            UIRideGrid grid = m_UIGridCreatorBase.GetGrid<UIRideGrid>(0);
            if (grid != null)
            {
                OnRideGridUIEvent(UIEventType.Click, grid, null);
            }
        }
    }


    /// <summary>
    /// 更新出站状态
    /// </summary>
    void UpdateFightState()
    {
        if (m_currRideData == null) return;
        m_btn_btn_fight.GetComponentInChildren<UILabel>().text = m_rideMgr.Auto_Ride == m_currRideData.id ? "休息" : "出战";

        if (m_UIGridCreatorBase != null)
        {
            List<UIRideGrid> lstgrid = m_UIGridCreatorBase.GetGrids<UIRideGrid>();
            for (int i = 0; i < lstgrid.Count; i++)
            {
                lstgrid[i].SetFightState(m_rideMgr.Auto_Ride == lstgrid[i].RideData.id);
            }
        }
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eShowUI)
        {
            if (param is ReturnBackUIMsg)
            {
                ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
                if (showInfo != null)
                {
                    if (showInfo.tabs.Length > 0)
                    {
                        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, showInfo.tabs[0]);
                    }

                    List<UIRideGrid> lstgrid = m_UIGridCreatorBase.GetGrids<UIRideGrid>();
                    for (int i = 0; i < lstgrid.Count; i++)
                    {
                        if (lstgrid[i].RideData.id == (uint)showInfo.param)
                        {
                            OnRideGridUIEvent(UIEventType.Click, lstgrid[i], null);
                            break;
                        }
                    }
                }
            }
        }
        //         if (msgid == UIMsgID.eRideUpdateAutoCost)
        //         {
        //             bool show = (bool)param;
        //          //   m_RideLevelUp.UpdateCost(show);
        //         }    
        return base.OnMsg(msgid, param);
    }

    public GameObject GetNewRideUIGo()
    {
        return m_trans_newObj.gameObject;
    }

    public GameObject GetOldRideUIGo()
    {
        return m_trans_oldObj.gameObject;
        
    }


    void onClick_RideQuality_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RideQualityPanel);
    }
    void OnSelectHorse(GameObject go)
    {
        UIFrameManager.Instance.OnCilckTogglePanel(PanelID.RidePanel,(int)UIPanelBase.FisrstTabsIndex, (int)TabMode.MaJiu);
      
    }
  
}