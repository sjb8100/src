/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Clan
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ClanCreatePanel
 * 版本号：  V1.0.0.0
 * 创建时间：10/17/2016 11:21:17 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.ComponentModel;

partial class ClanCreatePanel
{
    #region define
    //氏族创建模式
    public enum ClanCreateMode
    {
        None = 0,
        [Description("申请氏族")]
        Apply = 1,
        [Description("创建氏族")]
        Create,
        [Description("支持氏族")]
        Support,
        Max,
    }

    #endregion

    #region property
    private ClanManger m_mgr = null;
    //当前氏族模式
    private ClanCreateMode m_em_clanCreateMode = ClanCreateMode.None;
    //组件初始化mask
    private int m_int_initMask = 0;
    //功能按钮字典
    private Dictionary<ClanCreateMode, UITabGrid> m_dic_Toggles = null;
    private Dictionary<ClanCreateMode,Transform> m_dic_trans = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
        m_str_inputGGInfo = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Clan_Commond_shizugonggao);
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalUIEvent(true);
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        m_em_clanCreateMode = ClanCreateMode.None;
        int firstTabData = -1;
        if (null != jumpData.Tabs)
        {
            if (jumpData.Tabs.Length >= 1)
            {
                firstTabData = jumpData.Tabs[0];
            }
        }
        if (firstTabData == -1)
        {
            firstTabData = (int)ClanCreateMode.Apply;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
        BuildClanInfoListReq();
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[1];
        pd.JumpData.Tabs[0] = (int)m_em_clanCreateMode;
        return pd;
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalUIEvent(false);
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            SetMode((ClanCreateMode)pageid, false);
//            BuildClanInfoListReq();
        }
        return base.OnTogglePanel(tabType, pageid);
    }
    #endregion

    #region Init
    private void InitWidgets()
    {
        m_mgr = DataManager.Manager<ClanManger>();
        m_dic_Toggles = new Dictionary<ClanCreateMode, UITabGrid>();
        m_dic_trans = new Dictionary<ClanCreateMode, Transform>();
        Transform ts = null;
        UITabGrid tab = null;
        for (ClanCreateMode i = ClanCreateMode.None; i < ClanCreateMode.Max; i++)
        {
            if (null != m_trans_FunctioToggles)
            {
                ts = m_trans_FunctioToggles.Find(i.ToString());
                if (null != ts)
                {
                    tab = ts.GetComponent<UITabGrid>();
                    if (null == tab)
                    {
                        tab = ts.gameObject.AddComponent<UITabGrid>();
                    }
                    if (null != tab)
                    {
                        tab.SetGridData(i);
                        tab.SetHightLight(false);
                        tab.RegisterUIEventDelegate(OnUIGridEventDlg);
                        m_dic_Toggles.Add(i, tab);
                    }
                }

            }

            if (null != m_trans_FucContent)
            {
                ts = m_trans_FucContent.Find(i.ToString());
                if (null == ts)
                {
                    continue;
                }
                m_dic_trans.Add(i, ts);
            }
        }
    }
    #endregion

    #region Op
    /// <summary>
    /// 是否初始化当前模式
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public bool IsInitMode(ClanCreateMode mode)
    {
        return (m_int_initMask & (1 << (int)mode)) != 0;
    }

    /// <summary>
    /// 设置初始化状态
    /// </summary>
    /// <param name="status"></param>
    /// <param name="init"></param>
    public void SetInitMode(ClanCreateMode mode)
    {
        m_int_initMask |= ((1 << (int)mode));
    }


    private void RegisterGlobalUIEvent(bool register =true)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTTEMPCLANEXISTTIMECHANGED, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTTEMPCLANSUPNUMCHANGED, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANCREATE, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANUPDATE, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANGGUPDATE, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTUSERAPPLYCLANLISTCHANGED, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTCLANLISTCHANGED, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTQUITCLAN, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTJOINCLAN, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTDISSOLVECLAN, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTPOSITIVECLAN, OnUIDataChange);
        }else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTTEMPCLANEXISTTIMECHANGED, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTTEMPCLANSUPNUMCHANGED, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANCREATE, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANUPDATE, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANGGUPDATE, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTUSERAPPLYCLANLISTCHANGED, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTCLANLISTCHANGED, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTQUITCLAN, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTJOINCLAN, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTDISSOLVECLAN, OnUIDataChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTPOSITIVECLAN, OnUIDataChange);
        }
    }

    /// <summary>
    /// UI数据改变
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    private void OnUIDataChange(int eventType,object data)
    {
        switch(eventType)
        {
            case (int)Client.GameEventID.UIEVENTQUITCLAN:
                {
                    //退出氏族
                    bool isFormal = false;
                    if (null != data && data is bool )
                    {
                        isFormal = (bool)data;
                    }
                    if (!isFormal)
                    {
                        //HideSelf();
                        //BuildClanInfoUI();
                        if (IsMode(ClanCreateMode.Support))
                        {
                            BuildClanInfoListReq();
                            if (null != m_ctor_ClanSupportScrollView)
                            {
                                m_ctor_ClanSupportScrollView.UpdateActiveGridData();
                            }
                        }
                        UpdateSupport();
                    }
                }
                break;
            case (int)Client.GameEventID.UIEVENTJOINCLAN:
                {
                    //加入氏族
                    bool isFormal = false;
                    if (null != data && data is bool)
                    {
                        isFormal = (bool)data;
                    }
                    if (isFormal)
                    {
                        HideSelf();
                    }
                    else 
                    {
                        if (IsMode(ClanCreateMode.Support))
                        {
                            BuildClanInfoListReq();
                            if (null != m_ctor_ClanSupportScrollView)
                            {
                                m_ctor_ClanSupportScrollView.UpdateActiveGridData();
                            }
                        }
//                         if ((IsMode(ClanCreateMode.Support))
//                          && null != m_applyClanGridCreator)
//                         {
//                             m_applyClanGridCreator.UpdateActiveGridData();
//                         }
//                         BuildClanInfoUI();
                       
                    }
                
                }
                break;
            case (int)Client.GameEventID.UIEVENTDISSOLVECLAN:
                {
                    BuildClanInfoListReq();
                    //氏族解散
                    if (null != m_ctor_ClanSupportScrollView)
                    {
                        m_ctor_ClanSupportScrollView.UpdateActiveGridData();
                    }
                }
                break;
            case (int)Client.GameEventID.UIEVENTPOSITIVECLAN:
                {
                    //氏族转正
                    HideSelf();
                }
                break;
            case (int)Client.GameEventID.UIEVENTTEMPCLANEXISTTIMECHANGED:
                break;
            case (int)Client.GameEventID.UIEVENTTEMPCLANSUPNUMCHANGED:
                if (IsMode(ClanCreateMode.Create))
                {
                    OnUpdateSupportNum((int)data);
                }
                else if (IsMode(ClanCreateMode.Support))
                {
                    BuildClanInfoListReq();
                    if (null != m_ctor_ClanSupportScrollView)
                    {
                        m_ctor_ClanSupportScrollView.UpdateActiveGridData();
                    }
                }
                break;
            case (int)Client.GameEventID.UIEVENTCLANLISTCHANGED:
                if (!IsMode(ClanCreateMode.Create))
                {
                    RebuildClanList((int)data);
                }
                break;
            case (int)Client.GameEventID.UIEVENTCLANUPDATE:
                if ((IsMode(ClanCreateMode.Apply) || IsMode(ClanCreateMode.Create))
                    && null != m_ctor_ClanApplyScrollView)
                {
                    m_ctor_ClanApplyScrollView.UpdateActiveGridData();
                }
                UpdateMode();
                break;
            case (int)Client.GameEventID.UIEVENTCLANCREATE:
                UpdateWidgetsVisibleStatus();
                InitCreate();
                UpdateMode();
                break;
            case (int)Client.GameEventID.UIEVENTCLANGGUPDATE:
                UpdateGG();
                break;
            case (int)Client.GameEventID.UIEVENTUSERAPPLYCLANLISTCHANGED:
//                 if (IsMode(ClanCreateMode.Apply))
//                 {
//                     UpdateClanList();
//                 }
                        if (null != m_ctor_ClanApplyScrollView)
                        {
                            m_ctor_ClanApplyScrollView.UpdateActiveGridData();
                        }
                        UpdateMode();
                break;
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                if (IsMode(ClanCreateMode.Create))
                {
                    UpdateHorn();
                }
                break;

        }
    }

    /// <summary>
    /// 初始化模式
    /// </summary>
    private void InitMode()
    {
        switch (m_em_clanCreateMode)
        {
            case ClanCreateMode.Apply:
                InitApply();
                break;
            case ClanCreateMode.Support:
                InitSupport();
                break;
            case ClanCreateMode.Create:
                InitCreate();
                break;
        }
    }

    /// <summary>
    /// 构建模式数据
    /// </summary>
    private void BuildMode()
    {
        switch (m_em_clanCreateMode)
        {
            case ClanCreateMode.Apply:
            case ClanCreateMode.Support:
                BuildApplySupport();
                break;
            case ClanCreateMode.Create:
                BuildCreate();
                break;
        }
    }
    /// <summary>
    /// 设置当前模式
    /// </summary>
    /// <param name="mode"></param>
    private void SetMode(ClanCreateMode mode,bool force = false)
    {
        if (IsMode(mode) && !force)
        {
            return;
        }
        if (null != m_dic_Toggles)
        {
            UITabGrid tab = null;
            if ( !IsMode(mode) && m_dic_Toggles.TryGetValue(m_em_clanCreateMode,out tab))
            {
                tab.SetHightLight(false);
            }
            if (m_dic_Toggles.TryGetValue(mode, out tab))
            {
                tab.SetHightLight(true);
            }
        }
        m_em_clanCreateMode = mode;
        UpdateWidgetsVisibleStatus();
        InitMode();
        BuildMode();
        UpdateMode();
    }
    
    /// <summary>
    /// 是否当前模式为mode
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private bool IsMode(ClanCreateMode mode)
    {
        return (m_em_clanCreateMode == mode) ? true : false;
    }

    /// <summary>
    /// 刷新公告
    /// </summary>
    private void UpdateGG()
    {
        if (IsMode(ClanCreateMode.Apply))
        {
            UpdateApplyGG();
        }else if (IsMode(ClanCreateMode.Support))
        {
            UpdateSupportGG();
        }else if (IsMode(ClanCreateMode.Create))
        {
            UpdateCreateGG();
        }
    }

    /// <summary>
    /// 刷新面板
    /// </summary>
    private void UpdateMode()
    {
        switch(m_em_clanCreateMode)
        {
            case ClanCreateMode.Apply:
                UpdateApply();
                break;
            case ClanCreateMode.Support:
                UpdateSupport();
                break;
            case ClanCreateMode.Create:
                UpdateCreate();
                break;
        }
    }

    /// <summary>
    /// 请求构建氏族列表
    /// </summary>
    private void BuildClanInfoListReq()
    {
        DataManager.Manager<ClanManger>().GetClanInfoList(GameCmd.eGetClanType.GCT_TempFormal, 1);
    }

    /// <summary>
    /// 刷新可视组件
    /// </summary>
    private void UpdateWidgetsVisibleStatus()
    {
        Transform ts = null;
        if (null != m_dic_trans)
        {
            bool cansee = false;
            for (ClanCreateMode i = ClanCreateMode.None + 1; i < ClanCreateMode.Max; i++)
            {
                cansee = IsMode(i);
                if (m_dic_trans.TryGetValue(i,out ts) && ts.gameObject.activeSelf != cansee)
                {
                    ts.gameObject.SetActive(cansee);
                }
            }
        }
    }
    #endregion


    #region UIEvent
    
    //void onClick_BtnSearch_Btn(GameObject caster)
    //{
    //    SearchClan();
    //}

    
    //void onClick_BtnNotice_Btn(GameObject caster)
    //{
    //    SendTopGG();
    //}

    //void onClick_BtnCreateTempClan_Btn(GameObject caster)
    //{
    //    CreateClan();
    //}

    //void onClick_BtnContactShaikh_Btn(GameObject caster)
    //{
    //    ContactShaikh();
    //}

    //void onClick_BtnApply_Btn(GameObject caster)
    //{
    //    ApplyClan();
    //}

    //void onClick_BtnApplyQuick_Btn(GameObject caster)
    //{
    //    QuickApplyClan();
    //}

    //void onClick_BtnSupport_Btn(GameObject caster)
    //{
    //    SupportClan();
    //}

    //void onClick_BtnSupportCancel_Btn(GameObject caster)
    //{
    //    CancelSupportClan();
    //}

    #endregion
}