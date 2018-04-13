/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanPanel
 * 版本号：  V1.0.0.0
 * 创建时间：10/17/2016 11:23:45 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
partial class ClanPanel
{
    #region define
    //氏族界面模式
    public enum ClanPanelMode
    {
        None = 0,
        Info,
        Member,
        Activity,
        Skill,
        Max,
    }
    #endregion

    #region property
    //
    private ClanManger m_mgr = null;
    //当前面板模式
    private ClanPanelMode panelMode = ClanPanelMode.None;
    //氏族信息
    private ClanDefine.LocalClanInfo ClanInfo
    {
        get
        {
            return DataManager.Manager<ClanManger>().ClanInfo;
        }
    }
    private Dictionary<ClanPanelMode, UITabGrid> m_dic_clanPanlTabs = null;
    private Dictionary<ClanPanelMode, Transform> m_dic_clanPanlTs = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalUIEvent(true);
        //请求氏族申请信息
        m_mgr.GetClanInfoReq(m_mgr.ClanId, false);
        m_mgr.GetClanApplyUserIds();
        UpdateApplyRedPoint();
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        panelMode = ClanPanelMode.None;
        int firstTabData = -1;
        int secondTabData = - 1;
        if (null != jumpData.Tabs)
        {
            if (jumpData.Tabs.Length >=1)
            {
                firstTabData = jumpData.Tabs[0];
            }

            if (jumpData.Tabs.Length >= 2)
            {
                secondTabData = jumpData.Tabs[1];
            }
        }
        if (firstTabData == -1)
        {
            firstTabData = (int)ClanPanelMode.Info;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
        switch (firstTabData)
        {
//             case (int)ClanPanelMode.Info:
//                 SetInfoMode(((secondTabData != -1) ? (ClanInfoMode)secondTabData : ClanInfoMode.Detail));
//                 break;
            case (int)ClanPanelMode.Member:
                SetMemberMode(((secondTabData != -1) ? (ClanMemberMode)secondTabData : ClanMemberMode.Member));
                break;
            case (int)ClanPanelMode.Skill:
                SetSkillMode(((secondTabData != -1) ? (ClanSkillMode)secondTabData : ClanSkillMode.Learn));
                break;
            case (int)ClanPanelMode.Activity:
                SetInfoMode(((secondTabData != -1) ? (ClanInfoMode)secondTabData : ClanInfoMode.Upgrade));
                break;
        }
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[2];
        pd.JumpData.Tabs[0] = (int)panelMode;
        switch (panelMode)
        {
            case ClanPanelMode.Info:
                pd.JumpData.Tabs[1] = (int)m_em_activeInfoMode;
                break;
            case ClanPanelMode.Member:
                pd.JumpData.Tabs[1] = (int)m_em_clanMemberMode;
                break;
            case ClanPanelMode.Skill:
                pd.JumpData.Tabs[1] = (int)m_em_skillMode;
                break;
        }
        return pd;
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalUIEvent(false);
        Release();
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_ClanHonorScrollView != null)
        {
            m_ctor_ClanHonorScrollView.Release(depthRelease);
        }
        if (m_ctor_ActivityScrollView != null)
        {
            m_ctor_ActivityScrollView.Release(depthRelease);
        }
        if (m_ctor_LeftBtnRoot != null)
        {
            m_ctor_LeftBtnRoot.Release(depthRelease);
        }
        if (m_ctor_DonateScrollView != null)
        {
            m_ctor_DonateScrollView.Release(depthRelease);
        }
        if (m_ctor_DecareWarListScoll != null)
        {
            m_ctor_DecareWarListScoll.Release(depthRelease);
        }
        if (m_ctor_SkillScrollView != null)
        {
            m_ctor_SkillScrollView.Release(depthRelease);
        }
        if (m_ctor_ClanHonorScrollView != null)
        {
            m_ctor_ClanHonorScrollView.Release(depthRelease);
        }
        if(m_priceAsynSeed != null)
        {
            m_priceAsynSeed.Release(true);
            m_priceAsynSeed = null;
        }
        m_em_skillMode = ClanSkillMode.None;
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Release();
    }


    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            SetMode((ClanPanelMode)pageid, false);
        }

        return base.OnTogglePanel(tabType, pageid);
    }
    #endregion

    #region Init
    private void InitWidgets()
    {
        m_mgr = DataManager.Manager<ClanManger>();
        m_dic_clanPanlTabs = new Dictionary<ClanPanelMode, UITabGrid>();
        m_dic_clanPanlTs = new Dictionary<ClanPanelMode, Transform>();
        UITabGrid grid = null;
        Transform ts = null;
        if (null != m_trans_FunctioToggles && null != m_trans_LeftContent)
        {
            for (ClanPanelMode i = ClanPanelMode.None + 1; i < ClanPanelMode.Max; i++)
            {
                ts = m_trans_FunctioToggles.Find("Clan" + i.ToString());
                if (null != ts)
                {
                    grid = ts.GetComponent<UITabGrid>();
                    if (null == grid)
                    {
                        grid = ts.gameObject.AddComponent<UITabGrid>();
                    }
                    grid.SetGridData(i);
                    grid.RegisterUIEventDelegate(OnUIGridEventDlg);
                    grid.SetHightLight(false);
                    m_dic_clanPanlTabs.Add(i, grid);
                }

                ts = m_trans_LeftContent.Find(i.ToString() + "Content");
                if (null != ts)
                {
                    m_dic_clanPanlTs.Add(i, ts);
                }
            }
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void UpdatePanelWidgetsVisibleStatus()
    {
        if (null != m_dic_clanPanlTs)
        {
            bool visble = false;
            Transform ts = null;
            for (ClanPanelMode i = ClanPanelMode.None; i < ClanPanelMode.Max;i++ )
            {
                visble = (i == panelMode);
                if (m_dic_clanPanlTs.TryGetValue(i, out ts)
                    && ts.gameObject.activeSelf != visble)
                {
                    ts.gameObject.SetActive(visble);
                }

                visble = (i != ClanPanelMode.Info);
            }
                
        }
    }

    /// <summary>
    /// 初始化可视组件
    /// </summary>
    private void InitVisbileWidgets()
    {
        switch(panelMode)
        {
            case ClanPanelMode.Info:
//                InitInfo();
                break;
            case ClanPanelMode.Member:
                InitMember();
                break;
            case ClanPanelMode.Activity:
                InitInfo();
//                InitTask();
                break;
            case ClanPanelMode.Skill:
                InitSkill();
                break;
        }
    }

    /// <summary>
    /// 构建模式数据
    /// </summary>
    private void BuildModeData()
    {
        switch(panelMode)
        {
            case ClanPanelMode.Info:
                UpdateDetail();
                break;
            case ClanPanelMode.Member:
                BuildMember();
                break;
            case ClanPanelMode.Skill:
                BuildSkill();
                break;
            case ClanPanelMode.Activity:
                BuildInfo();
//                BuildTask();
                break;
        }
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="register"></param>
    private void RegisterGlobalUIEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANDONATESUCCESS, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTREFRESHCLANDONATEDATAS, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGETCLANHONORCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGETCLANHONORDATAS, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANUPDATE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTREFRESHCLANAPPLYINFO, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANGGUPDATE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANMEMBERUPDATE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANUPGRADE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANDEVSKILLCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTUSERLEARNCLANSKILLCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANMEMBEROPDIALOGCLOSED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANRIVALRYREFRESH, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANRIVALRYCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTQUITCLAN, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTJOINCLAN, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTDISSOLVECLAN, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTPOSITIVECLAN, OnGlobalUIEventHandler);
        }else
        {
            
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANDONATESUCCESS, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTREFRESHCLANDONATEDATAS, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGETCLANHONORCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGETCLANHONORDATAS, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANUPDATE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTREFRESHCLANAPPLYINFO, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANGGUPDATE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANMEMBERUPDATE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANUPGRADE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANDEVSKILLCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTUSERLEARNCLANSKILLCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANMEMBEROPDIALOGCLOSED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANRIVALRYREFRESH, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANRIVALRYCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTQUITCLAN, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTJOINCLAN, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTDISSOLVECLAN, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTPOSITIVECLAN, OnGlobalUIEventHandler);
        }
    }

    private void OnGlobalUIEventHandler(int eventType,object data)
    {
        switch(eventType)
        {
            case (int)Client.GameEventID.UIEVENTQUITCLAN:
                {
                   if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ClanPanel))
                   {
                       HideSelf();
                   }
                   DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanCreatePanel);
                }
                break;
            case (int)Client.GameEventID.UIEVENTJOINCLAN:
                {
                    //加入氏族

                }
                break;
            case (int)Client.GameEventID.UIEVENTDISSOLVECLAN:
                {
                    //氏族解散
                    HideSelf();
                }
                break;
            case (int)Client.GameEventID.UIEVENTPOSITIVECLAN:
                {
                    //氏族转正
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanPanel);
                }
                break;
            case (int)Client.GameEventID.UIEVENTREFRESHCLANDONATEDATAS:
                OnDonateDataChanged();
                break;
            case (int)Client.GameEventID.UIEVENTCLANDONATESUCCESS:
                OnDonateSuccess((uint)data);
                break;
            case (int)Client.GameEventID.UIEVENTGETCLANHONORCHANGED:
                //OnHonorInfoAdd((string)data);
                break;
            case (int)Client.GameEventID.UIEVENTGETCLANHONORDATAS:
                OnRefreshHonorInfos();
                break;
            case (int)Client.GameEventID.UIEVENTCLANUPDATE:
                UpdateUpgrade();
                if (null != data && data is bool)
                {
                    m_bool_showSkillRedPoint = (bool)data;
                    UpdateSkillRedPoint();
                }   
                if (IsPanelMode(ClanPanelMode.Skill))
                {
                    UpdateSkill();
                }
                   
                break;
            case (int)Client.GameEventID.UIEVENTREFRESHCLANAPPLYINFO:
                OnClanApplyInfoChanged();

                //BuildApplyList();
                if (null != data && data is bool)
                {
                    m_bool_showApplyRedPoint = (bool)data;
                    SetApplyRedPoint();                   
                }
                break;
            case (int)Client.GameEventID.UIEVENTCLANGGUPDATE:
                UpdateGG();
                UpdatePanel();
                break;
            case (int)Client.GameEventID.UIEVENTCLANMEMBERUPDATE:
                if (null != data && data is uint)
                {
                    uint id = (uint)data;
                    RefreshMemberList(id);
                }
             
                break;
            case (int)Client.GameEventID.UIEVENTUSERLEARNCLANSKILLCHANGED:
                if (m_em_skillMode == ClanSkillMode.Learn)
                {
                    OnSkillChanged((uint)data);
                }
                break;
            case (int)Client.GameEventID.UIEVENTCLANDEVSKILLCHANGED:
                if (m_em_skillMode == ClanSkillMode.Dev)
                {
                    OnSkillChanged((uint)data);
                }
                break;
            case (int)Client.GameEventID.UIEVENTCLANMEMBEROPDIALOGCLOSED:
                SetSelectMemberId(null);
                break;
            case (int)Client.GameEventID.UIEVENTCLANTASKCHANGED:
                OnClanTaskChanged(data);
                break;
            case (int)Client.GameEventID.UIEVENTCLANRIVALRYCHANGED:
                BuildDeclareWarList();
               // OnDeclareWarInfoChanged((uint)data);
                break;
            case (int)Client.GameEventID.UIEVENTCLANRIVALRYREFRESH:
                BuildDeclareWarList();
                break;
        }
    }
    #endregion


    #region Op
    //氏族面板初始化标示
    private int m_int_clanModeInitMask = 0;
    //氏族面板信息初始化标示
    private int m_int_clanInfoInitMask = 0;
    //氏族面板技能初始化标示
    private int m_int_clanSkillInitMask = 0;
    //氏族面板技能初始化标示
    private int m_int_clanMemberInitMask = 0;
    /// <summary>
    /// 当前模式是否为mode
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>

    public bool IsPanelMode(ClanPanelMode mode)
    {
        return panelMode == mode;
    }
    /// <summary>
    /// 是否初始化当前模式
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public bool IsInitMode(Enum mode)
    {
        int mask = 0;
        int value = 0;
        bool match = false;
        if (mode is ClanPanelMode)
        {
            match = true;
            mask = m_int_clanModeInitMask;
            value = (int)((ClanPanelMode)mode);
        }
        else if (mode is ClanInfoMode)
        {
            match = true;
            mask = m_int_clanInfoInitMask;
            value = (int)((ClanInfoMode)mode);
        }
        else if (mode is ClanSkillMode)
        {
            match = true;
            mask = m_int_clanSkillInitMask;
            value = (int)((ClanSkillMode)mode);
        }
        else if (mode is ClanMemberMode)
        {
            match = true;
            mask = m_int_clanMemberInitMask;
            value = (int)((ClanMemberMode)mode);
        }
        if (match)
        {
            return (mask & (1 << value)) != 0;
        }
        return false;
        
    }

    /// <summary>
    /// 设置初始化状态
    /// </summary>
    /// <param name="status"></param>
    /// <param name="init"></param>
    public void SetInitMode(Enum mode)
    {
        int mask = 0;
        int value = 0;
        if (mode is ClanPanelMode)
        {
            value = (int)((ClanPanelMode)mode);
            m_int_clanModeInitMask |= (1 << value);
        }
        else if (mode is ClanInfoMode)
        {
            value = (int)((ClanInfoMode)mode);
            m_int_clanInfoInitMask |= (1 << value);
        }
        else if (mode is ClanSkillMode)
        {
            value = (int)((ClanSkillMode)mode);
            m_int_clanSkillInitMask |= (1 << value);
        }
        else if (mode is ClanMemberMode)
        {
            value = (int)((ClanMemberMode)mode);
            m_int_clanMemberInitMask |= (1 << value);
        }

    }

    /// <summary>
    /// 格子刷新回调
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    private void OnUpdateUIGrid(UIGridBase grid,int index)
    {
        if (grid is UIClanTaskGrid)
        {
            if (null != m_list_taskInfos && m_list_taskInfos.Count > index)
            {
                uint id = m_list_taskInfos[index];
                UIClanTaskGrid clanTaskGrid = grid as UIClanTaskGrid;
                clanTaskGrid.SetGridData(m_mgr.GetClanQuestInfo(id));
            }else
            {
                Engine.Utility.Log.Error("ClanTask OnUpdateUIGrid error");
            }
        }else if (grid is UIClanApplyGrid)
        { 
            UIClanApplyGrid caGrid = grid as UIClanApplyGrid;
            if (null != caGrid && null != m_list_applyUserIds && m_list_applyUserIds.Count > index)
            {
                caGrid.SetGridData(m_mgr.GetClanApplyUserInfo(m_list_applyUserIds[index]));
            }

        }else if (grid is UIClanDonateGrid)
        {
            UIClanDonateGrid cdGrid = grid as UIClanDonateGrid;
            if (null != cdGrid && null != m_list_donateDatas && m_list_donateDatas.Count > index)
            {
                ClanDefine.LocalClanDonateDB cdDB 
                    = m_mgr.GetLocalDonateDB(m_list_donateDatas[index]);
                cdGrid.RegisterDonateAction(DoDonate);
                if (null != cdDB)
                {
                    cdGrid.SetGridData(cdDB);
                }
            }
        }else if (grid is UIClanMemberGrid)
        {
            UIClanMemberGrid cmGrid = grid as UIClanMemberGrid;
            //ClanDefine.LocalClanInfo clanInfo = ClanInfo;
            if (null != cmGrid && index < m_list_memberdatas.Count)
            {
                cmGrid.SetGridData(m_list_memberdatas[index]);
                cmGrid.SetHightLight((m_uint_selectmemberid == m_list_memberdatas[index]) ? true : false);
                cmGrid.SetBackGround(index);
            }

        }else if (grid is UIClanSkillGrid)
        {
            UIClanSkillGrid csGrid = grid as UIClanSkillGrid;
            if (null != csGrid && null != m_list_clanSkillIds && m_list_clanSkillIds.Count > index)
            {
                uint skillId = m_list_clanSkillIds[index];
                csGrid.SetGridData(m_list_clanSkillIds[index]);
                table.SkillDatabase skillDB = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(skillId, 1);
                if (null != skillDB)
                {
                    bool select = (m_uint_SelectSkillId == skillId) ? true : false;
                    
                    uint lv1 = 0;
                    uint lv2 = 0;
                    bool isLock = false;
                    if (IsSkillMode(ClanSkillMode.Learn))
                    {
                        lv1 = m_mgr.GetClanSkillLearnLv(skillId);
                        lv2 = m_mgr.GetClanSkillDevLv(skillId);
                        isLock = (lv1 >= lv2);
                    }else if (IsSkillMode(ClanSkillMode.Dev))
                    {
                        lv1 = m_mgr.GetClanSkillDevLv(skillId);
                        lv2 = skillDB.dwMaxLevel;
                    }

                    csGrid.SetInfo(skillDB.strName
                        , string.Format("{0}/{1}",lv1,lv2)
                        , skillDB.iconPath
                        , select, isLock);
                }
            }
        }else if (grid is UIClanHonorGrid)
        {
            UIClanHonorGrid cdGrid = grid as UIClanHonorGrid;
            if (null != cdGrid && null != m_list_honorInfos && m_list_honorInfos.Count > index)
            {
                cdGrid.SetGridData(m_list_honorInfos[index]);
            }
        }else if (grid is UIClanDecareWaRivalryGrid)
        {
            UIClanDecareWaRivalryGrid dwg = grid as UIClanDecareWaRivalryGrid;
            if (null != m_lst_Rivalry && m_lst_Rivalry.Count > index)
            {
                dwg.SetGridData(m_mgr.GetClanRivalryInfo(m_lst_Rivalry[index]));
            }
        }
        else if(grid is UIClanDeclareWarGrid)
        {
            UIClanDeclareWarGrid dwg = grid as UIClanDeclareWarGrid;
            if (null != m_lst_Rivalry && m_lst_Rivalry.Count > index)
            {
                dwg.SetGridData(m_mgr.GetClanRivalryInfo(m_lst_Rivalry[index]));
                dwg.SetIndex(index);
            }
        }
        else if(grid is UIClanTabGrid)
        {
            UIClanTabGrid tab = grid as UIClanTabGrid;
            tab.SetGridData(index);
            tab.SetName(m_list_tabContents[index]);   
            if(index == 0 && previous ==null)
            {
                previous = tab;
                tab.SetHightLight(true);
            }
        }
    }
    UIClanTabGrid previous = null;
    UIClanDeclareWarGrid previousDeclareGrid = null;
    /// <summary>
    /// 格子UI点击响应事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnUIGridEventDlg(UIEventType eventType,object data,object param)
    {
        if (eventType == UIEventType.Click)
        {
            if (data is UIClanTaskGrid)
            {
                UIClanTaskGrid taskGrid = data as UIClanTaskGrid;
            }
            else if (data is UIClanApplyGrid )
            {
                UIClanApplyGrid aGrid = data as UIClanApplyGrid;
                if (null != param && param is bool)
                {
                    bool agree = (bool)param;
                    DealClanApplyInfo(aGrid.UserId, agree);
                }
                else
                {
                   GameCmd.stRequestListClanUserCmd_S.Data member = m_mgr.GetClanApplyUserInfo(aGrid.UserId);
                   DataManager.Instance.Sender.RequestPlayerInfoForOprate(aGrid.UserId, PlayerOpreatePanel.ViewType.Normal);
                   
                }
              
            }
            else if (data is UIClanDonateGrid)
            {

            }
            else if (data is UIClanDeclareWarGrid)
            {
                UIClanDeclareWarGrid dwg = data as UIClanDeclareWarGrid;
                dwg.SetSelect(true);
                if (previousDeclareGrid == null)
                {
                    previousDeclareGrid = dwg;
                }
                else 
                {
                    if (previousDeclareGrid.Id == dwg.Id)
                    {
                        return;
                    }
                    else
                    {
                        previousDeclareGrid.SetSelect(false);
                        previousDeclareGrid = dwg;
                    }
                }
               
            }
            else if (data is UIClanMemberGrid)
            {
                UIClanMemberGrid mGrid = data as UIClanMemberGrid;
                GameCmd.stClanMemberInfo member = (null != ClanInfo) ? ClanInfo.GetMemberInfo(mGrid.UserId) : null;
                if (null != member)
                {
                    if (member.id != DataManager.Instance.UserId)
                    {
                        SetSelectMemberId(member);
                        DataManager.Instance.Sender.RequestPlayerInfoForOprate(mGrid.UserId, PlayerOpreatePanel.ViewType.Clan);
                    }

                }
                else
                {
                    Engine.Utility.Log.Error("GetClanMemberInfo null userid={0}", mGrid.UserId);
                }

            }
            else if (data is UIClanSkillGrid)
            {
                UIClanSkillGrid skillGrid = data as UIClanSkillGrid;
                SetSelectSkillId(skillGrid.Id);
            }else if (data is UITabGrid)
            {
                UITabGrid tabGrid = data as UITabGrid;
                if (tabGrid.Data is ClanPanelMode)
                {
                    SetMode((ClanPanelMode)tabGrid.Data);
                }
            }
            else if(data is UIClanTabGrid)
            {
                UIClanTabGrid tab = data as UIClanTabGrid;
                SetInfoMode((ClanInfoMode)tab.Data+1);
              
            }
        }
    }

    /// <summary>
    /// 设置当前面板模式
    /// </summary>
    /// <param name="mode"></param>
    private void SetMode(ClanPanelMode mode,bool force = false)
    {
        if (mode == panelMode && !force)
        {
            return;
        }
        
        if (null != m_dic_clanPanlTabs)
        {
            UITabGrid tab = null;
            if (m_dic_clanPanlTabs.TryGetValue(panelMode, out tab))
            {
                tab.SetHightLight(false);
            }
            if (m_dic_clanPanlTabs.TryGetValue(mode, out tab))
            {
                tab.SetHightLight(true);
            }
        }
        panelMode = mode;
        UpdatePanelWidgetsVisibleStatus();
        InitVisbileWidgets();
        BuildModeData();
        UpdatePanel();
        SetApplyRedPoint();
    }

    /// <summary>
    /// 更新面板（根据当前模式）
    /// </summary>
    private void UpdatePanel()
    {
        switch (panelMode)
        {
            case ClanPanelMode.Info:
                UpdateInfo();
                break;
            case ClanPanelMode.Member:
                UpdateMember();
                break;
            case ClanPanelMode.Skill:
                UpdateSkill();
                break;
            case ClanPanelMode.Activity:
                UpdateTask();
                break;
        }
    }
    #endregion

    #region UIEvent
    void onClick_ClanInfo_Btn(GameObject caster)
    {
        SetMode(ClanPanelMode.Info);
    }

    void onClick_ClanMember_Btn(GameObject caster)
    {
        SetMode(ClanPanelMode.Member);
    }

    void onClick_ClanTask_Btn(GameObject caster)
    {
        SetMode(ClanPanelMode.Activity);
    }

    void onClick_ClanSkill_Btn(GameObject caster)
    {
        SetMode(ClanPanelMode.Skill);
    }
    #endregion
}