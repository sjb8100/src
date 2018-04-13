using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using GameCmd;
using table;
using Engine.Utility;
using System.Collections;

partial class PropPanel : UIPanelBase,ITimer
{
    #region define
    //面板功能模式
    public enum PropPanelMode
    {
        None = 0,
        Prop,           //属性
        Title,          //称号
        Fashion,        //时装
        Max,
    }
    #endregion

    #region property
    //     GameObject prefab;
    //     UILabel[] labels;

    Dictionary<FightCreatureProp, UILabel> m_left_Lables = new Dictionary<FightCreatureProp, UILabel>();
    Dictionary<FightCreatureProp, UILabel> m_right_Lables = new Dictionary<FightCreatureProp, UILabel>();
    Dictionary<FightCreatureProp, UILabel> m_left_attack_Labels = new Dictionary<FightCreatureProp, UILabel>();
    Dictionary<FightCreatureProp, UILabel> m_right_attack_Labels = new Dictionary<FightCreatureProp, UILabel>();
    Dictionary<FightCreatureProp, UILabel> m_left_defend_Labels = new Dictionary<FightCreatureProp, UILabel>();
    Dictionary<FightCreatureProp, UILabel> m_right_defend_Labels = new Dictionary<FightCreatureProp, UILabel>();
    public uint currUID = 0;
    int[] fightValue = new int[]{
      (int)FightCreatureProp.PhysicsCrit,(int)FightCreatureProp.MagicCrit,
      (int)FightCreatureProp.Hit,(int)FightCreatureProp.Dodge,
      (int)FightCreatureProp.FireAttack,
      (int)FightCreatureProp.IceAttack,
      (int)FightCreatureProp.EleAttack,
      (int)FightCreatureProp.WitchAttack,
      (int)FightCreatureProp.FireDefend,
      (int)FightCreatureProp.IceDefend,
      (int)FightCreatureProp.EleDefend,
      (int)FightCreatureProp.WitchDefend   
    };

    private IRenerTextureObj m_RTObj = null;

    private GraphicOption m_GraphicOption = new GraphicOption();
    uint m_currShowUID = 0;

    //面板模式
    private PropPanelMode m_em_panelMode = PropPanelMode.None;
    private TextManager m_tmgr = null;

    private Dictionary<PropPanelMode, UITabGrid> m_dic_panelTab = null;
    public SuitDataManager m_suitDataManager
    {
        get
        {
            return DataManager.Manager<SuitDataManager>();
        }
    }

    uint m_uintWorldLevel = 0;
    #endregion

    public enum PropPanelPageEnum
    {
        None = 0,
        Page_Prop = 1,
        Page_Title = 2,
        Page_Fashion = 3,
        Max,
    }
    #region overridemethod
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnAwake()
    {
        base.OnAwake();

    }

    /// <summary>
    /// 第二
    /// </summary>
    protected override void OnLoading()
    {
        base.OnLoading();

        foreach (var a in m_right_attack_Labels)
        {
            m_right_Lables.Add(a.Key, a.Value);
        }
        foreach (var a2 in m_left_attack_Labels)
        {
            m_left_Lables.Add(a2.Key, a2.Value);
        }
        foreach (var a3 in m_right_defend_Labels)
        {
            m_right_Lables.Add(a3.Key, a3.Value);
        }
        foreach (var a4 in m_left_defend_Labels)
        {
            m_left_Lables.Add(a4.Key, a4.Value);
        }

        m_lstTableTitle = GameTableManager.Instance.GetTableList<TitleDataBase>();

        InitWidgets();
        m_uintWorldLevel = DataManager.Manager<MailManager>().WorldLevel;
    }
    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (pageid == (int)PropPanelPageEnum.Page_Prop)
        {
            SetPanelMode(PropPanelMode.Prop, true);
            uint showUID = ClientGlobal.Instance().MainPlayer.GetID();
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                return false;
            }
            IPlayer player = es.FindPlayer(showUID);
            if (player == null)
            {
                return false;
            }
            m_currShowUID = showUID;
            ShowByPlayer(player);
            m_label_fight.gameObject.SetActive(true);

        }
        else if (pageid == (int)PropPanelPageEnum.Page_Title)
        {

            SetPanelMode(PropPanelMode.Title, true);
        }
        else if (pageid == (int)PropPanelPageEnum.Page_Fashion)
        {
            SetPanelMode(PropPanelMode.Fashion, true);
            m_label_fight.gameObject.SetActive(false);
        }

        // 页签红点提示
        UpdateRedPoint();
        return base.OnTogglePanel(tabType, pageid);
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
        int secondTabData = -1;
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];

        }
        else
        {
            firstTabData = (int)PropPanelMode.Prop;
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
        if (firstTabData == (int)PropPanelPageEnum.Page_Fashion)
        {
            if (jumpData.Tabs.Length > 1)
            {
                secondTabData = jumpData.Tabs[1];
                if (jumpData.Param is uint)
                {
                    ShowFashion(secondTabData, (uint)jumpData.Param);
                }
            }
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_CASD)
        {
            m_CASD.Release(true);
            m_CASD = null;
        }

        if (null != m_playerCASD)
        {
            m_playerCASD.Release();
            m_playerCASD = null;
        }
        ReleaseTitle(depthRelease);
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        RegisterGlobalUIEvent(false);
        if (null != m_RTObj)
        {
            m_RTObj.Release();
            m_RTObj = null;
        }
        if (null != m_petRTObj)
        {
            m_petRTObj.Release();
            m_petRTObj = null;
        }
        //Engine.Utility.TimerAxis.Instance().KillTimer(TITLE_TIMERID, this);
    }


    /// <summary>
    /// 页签红点提示
    /// </summary>
    void UpdateRedPoint()
    {
        UITabGrid tabGrid = null;
        Dictionary<int, UITabGrid> dicTabs = null;
        if (dicUITabGrid.TryGetValue(1, out dicTabs))
        {
            //属性
            if (dicTabs != null && dicTabs.TryGetValue((int)PropPanelPageEnum.Page_Prop, out tabGrid))
            {
                //tabGrid.SetRedPointStatus();
            }

            //时装
            if (dicTabs != null && dicTabs.TryGetValue((int)PropPanelPageEnum.Page_Fashion, out tabGrid))
            {
                //tabGrid.SetRedPointStatus();
            }

            //称号页签
            if (dicTabs != null && dicTabs.TryGetValue((int)PropPanelPageEnum.Page_Title, out tabGrid))
            {
                tabGrid.SetRedPointStatus(TManager.HaveNewTitle());
            }
        }
    }
    #endregion

    #region Init
    /// <summary>
    /// 注册UI全局事件
    /// </summary>
    /// <param name="register"></param>
    private void RegisterGlobalUIEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONADD, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONDISCARD, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONCHANGED, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONWEAR, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONTAKEOFF, UIGlobalEventHandler);//UIEventFashionData
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONDATA, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.REFRESHWORLDLEVEL, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.REFRESHPKVALUEREMAINTIME, UIGlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONADD, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONDISCARD, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONCHANGED, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONWEAR, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONTAKEOFF, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONDATA, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.REFRESHWORLDLEVEL, UIGlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.REFRESHPKVALUEREMAINTIME, UIGlobalEventHandler);
        }
    }

    /// <summary>
    /// 全局UI事件处理
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="data">数据</param>
    private void UIGlobalEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENTFASHIONDATA:
                {
                    RefreshFashionGrid();
                }
                break;

            case (int)Client.GameEventID.UIEVENTFASHIONADD:
            case (int)Client.GameEventID.UIEVENTFASHIONWEAR:
            case (int)Client.GameEventID.UIEVENTFASHIONTAKEOFF:
                break;
            case (int)GameEventID.REFRESHWORLDLEVEL:
                m_uintWorldLevel = (uint)data;
                RefreshWorldLevel(m_uintWorldLevel);
                break;
            case (int)Client.GameEventID.REFRESHPKVALUEREMAINTIME:
                uint time = (uint)data;
                OnResPkRemainTimeViewPanel(time);
                break;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void InitWidgets()
    {
        RegisterGlobalUIEvent(true);
        m_tmgr = DataManager.Manager<TextManager>();
        if (null == m_dic_panelTab)
        {
            m_dic_panelTab = new Dictionary<PropPanelMode, UITabGrid>();
        }
        m_dic_panelTab.Clear();

        SetPanelMode(PropPanelMode.Prop, true);
    }
    #endregion

    #region PanelOp
    private void SetPlayerFightPower()
    {
        if (null != m_label_fight)
        {
            m_label_fight.text =
                Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.FightCreatureProp.Power).ToString();
        }

    }
    /// <summary>
    /// 更新格子数据
    /// </summary>
    /// <param name="gridBase"></param>
    /// <param name="index"></param>
    private void OnUpdateGridData(UIGridBase gridBase, int index)
    {
        if (gridBase is UIFashionGrid)
        {
            List<ClientSuitData> suitList = m_suitDataManager.GetSortListData();
            if (null == suitList || suitList.Count <= index)
            {
                return;
            }
            //FashionDefine.LocalFashionData data = FashionDefine.LocalFashionData.Create(suitList[index].base_id);

            UIFashionGrid fashionGrid = gridBase as UIFashionGrid;
            fashionGrid.SetGridData(suitList[index]);
        }
    }

    /// <summary>
    ///格子UI事件委托
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType != UIEventType.Click)
        {
            if (data is UIFashionGrid)
            {
                UIFashionGrid fg = data as UIFashionGrid;
                int nCount = m_fashionCreator.ActiveCount;
                for (int i = 0; i < nCount; i++)
                {
                    UIFashionGrid temp = m_fashionCreator.GetGrid<UIFashionGrid>(i);
                    if (fg == temp)
                    {
                        temp.SetSelect(true);
                    }
                    else
                    {
                        temp.SetSelect(false);
                    }
                }

            }
            return;
        }


    }

    /// <summary>
    /// 设置当前面板的功能模式
    /// </summary>
    /// <param name="mode"></param>
    private void SetPanelMode(PropPanelMode mode, bool force = false)
    {
        if (m_em_panelMode == mode && !force)
        {
            return;
        }
        UITabGrid tab = null;
        if (null != m_dic_panelTab && m_dic_panelTab.TryGetValue(m_em_panelMode, out tab))
        {
            tab.SetHightLight(false);
        }
        m_em_panelMode = mode;
        if (null != m_dic_panelTab && m_dic_panelTab.TryGetValue(m_em_panelMode, out tab))
        {
            tab.SetHightLight(true);
        }
        UpdatePanelVisibleWidgets();

        UpdatePanel();

        ViewRemainTimeContent(false);
    }

    /// <summary>
    /// 当前面板模式是否为modeh
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private bool IsPanelMode(PropPanelMode mode)
    {
        return (m_em_panelMode == mode);
    }

    #endregion

    #region Update
    /// <summary>
    /// 刷新面板
    /// </summary>
    private void UpdatePanel()
    {
        HideFashiontips();
        switch (m_em_panelMode)
        {
            case PropPanelMode.Title:
                InitTitle();
                break;
            case PropPanelMode.Prop:
                {
                    ResetPlayerObj();
                    ShowPlayerRenderTex(true);

                    ShowPetRenderTex(false);
                    if (m_RTObj != null)
                    {
                        m_RTObj.SetModelScale(1);
                    }
                    PlayShowAni();
                }
                break;
            case PropPanelMode.Fashion:
                InitSuitUI();
                break;
        }
    }

    /// <summary>
    /// 更新面板可视组件
    /// </summary>
    private void UpdatePanelVisibleWidgets()
    {
        bool cansee = IsPanelMode(PropPanelMode.Fashion) || IsPanelMode(PropPanelMode.Prop);
        if (null != m_trans_PropAndFashionContent
            && m_trans_PropAndFashionContent.gameObject.activeSelf != cansee)
        {
            m_trans_PropAndFashionContent.gameObject.SetActive(cansee);
        }

        cansee = IsPanelMode(PropPanelMode.Prop);
        if (null != m_trans_PropContent
            && m_trans_PropContent.gameObject.activeSelf != cansee)
        {
            m_trans_PropContent.gameObject.SetActive(cansee);
        }
        cansee = IsPanelMode(PropPanelMode.Fashion);
        if (null != m_trans_FashionContent
            && m_trans_FashionContent.gameObject.activeSelf != cansee)
        {
            m_trans_FashionContent.gameObject.SetActive(cansee);
        }
        cansee = IsPanelMode(PropPanelMode.Title);
        if (null != m_trans_TitleContent
            && m_trans_TitleContent.gameObject.activeSelf != cansee)
        {
            m_trans_TitleContent.gameObject.SetActive(cansee);
        }
    }
    #endregion

    void EventCallBack(int nEventID, object param)
    {
        if (nEventID == (int)GameEventID.ENTITYSYSTEM_PROPUPDATE)
        {

            Client.stPropUpdate prop = (Client.stPropUpdate)param;
            //传过来的是Long型参数ID  强制转换不知道会不会有问题
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }
            Client.IEntity entity = es.FindEntity(prop.uid);
            if (entity != null)
            {
                if (entity.GetEntityType() != EntityType.EntityType_Player)
                {
                    return;
                }
                if (m_currShowUID == entity.GetID())
                {
                    ShowByPlayer(entity as Client.IPlayer);
                }

            }
        }
        else if (nEventID == (int)GameEventID.TITLE_WEAR)
        {

            Client.stTitleWear data = (Client.stTitleWear)param;
            if (Client.ClientGlobal.Instance().IsMainPlayer(data.uid))
            {
                SetWearTitleItem(data.titleId);

                RefreshTitleGrids(data.titleId);
            }

        }
        else if (nEventID == (int)GameEventID.TITLE_ACTIVATE)
        {
            uint titleId = (uint)param;
            SetActivateTitleItem(titleId);
            RefreshTitleGrids(titleId);
        }
        else if (nEventID == (int)GameEventID.TITLE_TIMEOUT)
        {
            uint titleId = (uint)param;
            RefreshTitleGrids(titleId);
        }
        else if (nEventID == (int)GameEventID.TITLE_NEWTITLE)
        {
            uint titleId = (uint)param;
            CreateTopTabs();
            RefreshTitleGrids(titleId);
        }
        else if (nEventID == (int)GameEventID.TITLE_DELETE)
        {
            uint titleId = (uint)param;
            CreateTopTabs();
            if (m_lstTitleData.Exists((data) => { return data == titleId ? true : false; }))
            {
                RefreshTitleGrids(titleId);
                //InitTitleGridListUI();
            }
        }
        else if (nEventID == (int)GameEventID.TITLE_USETIMES)
        {
            uint titleId = (uint)param;
            //刷新界面使用次数
            UpdateUseTimes(titleId);
            //刷新左侧的grid  （次数为0，取消佩戴激活）
            RefreshTitleGrids(titleId);
        }

    }

    void RefreshWorldLevel(uint worldLevel)
    {
        m_label_WorldLevel.text = worldLevel.ToString();
        int lv =  MainPlayerHelper.GetPlayerLevel();
        int gap = (int)worldLevel -lv;
        int openLv = GameTableManager.Instance.GetGlobalConfig<int>("WorldDiffOpenLev");
        if (lv < openLv)
        {
            m_label_extraExp.text = "0 %";
            return;
        }
        if (gap >= 0)
        {
            UpgradeAddDataBase data = GameTableManager.Instance.GetTableItem<UpgradeAddDataBase>((uint)gap);
            if (data != null)
            {
                m_label_extraExp.text = (data.expupxs / 100).ToString() + "%";
            }
        }
        else
        {
            m_label_extraExp.text = "0 %";
        }


    }

    /// <summary>
    /// 第四
    /// </summary>
    /// <param name="data"></param>
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        //
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, EventCallBack);

        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TITLE_TIMEOUT, EventCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TITLE_WEAR, EventCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TITLE_ACTIVATE, EventCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TITLE_NEWTITLE, EventCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TITLE_USETIMES, EventCallBack);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_CHANGERENDEROBJ, SuitEventCallBack);
        uint showUID = ClientGlobal.Instance().MainPlayer.GetID();
        if (data != null && data is uint)
        {
            showUID = (uint)data;
        }
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
            return;
        }

        IPlayer player = es.FindPlayer(showUID);
        if (player == null)
        {
            return;
        }
        m_currShowUID = showUID;
        ShowByPlayer(player);

        // 页签红点提示
        UpdateRedPoint();
    }

    private void CreatePlayerView()
    {
        if (null == m_RTObj)
        {
            m_RTObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj(DataManager.Instance.MainPlayer, 750);
            if (null != m_RTObj)
            {
                m_RTObj.SetModelRotateY(-135);
                m_RTObj.SetCamera(new Vector3(0, 1f, 0f), new Vector3(0, 45, 0), 4f);
                m_RTObj.PlayModelAni(Client.EntityAction.Show);

                if (m_RTObj != null)
                {
                    uint activeStoneSuitLv = DataManager.Manager<EquipManager>().ActiveStoneSuitLv;
                    DataManager.Manager<EquipManager>().AddEquipStoneSuitParticle(m_RTObj, activeStoneSuitLv);
                }

            }
        }
        SetPlayerFightPower();
    }

    private void EnablePlayerView(bool enable)
    {
        if (null != m_RTObj)
        {
            m_RTObj.Enable(enable);
        }
        if (null != m_petRTObj)
        {
            m_petRTObj.Enable(enable);
        }
        if (enable)
        {
            SetPlayerFightPower();
        }
    }
    void ReleaseRenderTexture()
    {
        if (null != m_RTObj)
        {
            m_RTObj.Release();
        }
        if (null != m_petRTObj)
        {
            m_petRTObj.Release();
        }
    }
    protected override void OnHide()
    {
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_PROPUPDATE, EventCallBack);

        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TITLE_TIMEOUT, EventCallBack);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TITLE_WEAR, EventCallBack);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TITLE_ACTIVATE, EventCallBack);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TITLE_NEWTITLE, EventCallBack);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TITLE_USETIMES, EventCallBack);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_CHANGERENDEROBJ, SuitEventCallBack);

        EnablePlayerView(false);
        ReleaseRenderTexture();
        Release();
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.BuyDressPanel);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.FashionTips);

        ViewRemainTimeContent(false);
    }

    private CMResAsynSeedData<CMTexture> m_playerCASD = null;
    /// <summary>
    /// 第五
    /// </summary>
    /// <param name="id"></param>
    void ShowByPlayer(IPlayer player)
    {
        m_label_Name.text = player.GetName();
        m_label_Level.text = player.GetProp((int)CreatureProp.Level).ToString();
        //PK值显示
        m_label_PKzhi.text = player.GetProp((int)PlayerProp.PKValue).ToString(); ;
        m_label_hp_percent.text = player.GetProp((int)CreatureProp.Hp).ToString() + "/" + player.GetProp((int)CreatureProp.MaxHp).ToString();
        m_slider_Hpslider.value = (player.GetProp((int)CreatureProp.Hp) + 0.0f) / (player.GetProp((int)CreatureProp.MaxHp));
        //魔法值mp显示
        m_label_mp_percent.text = player.GetProp((int)CreatureProp.Mp).ToString() + "/" + player.GetProp((int)CreatureProp.MaxMp).ToString();
        m_slider_Mpslider.value = (player.GetProp((int)CreatureProp.Mp) + 0.0f) / (player.GetProp((int)CreatureProp.MaxMp));
        //经验值exp显示
        table.UpgradeDataBase data = GameTableManager.Instance.GetTableItem<table.UpgradeDataBase>((uint)player.GetProp((int)CreatureProp.Level));
        ulong maxExp = 0;
        if (data != null)
        {
            maxExp = data.qwExp;
        }
        m_label_exp_percent.text = player.GetProp((int)PlayerProp.Exp).ToString() + "/" + maxExp.ToString();
        m_slider_Expslider.value = (player.GetProp((int)PlayerProp.Exp) + 0.0f) / (int)maxExp;
        if (DataManager.Manager<ClanManger>().IsJoinClan)
        {
            m_label_clanName.text = DataManager.Manager<ClanManger>().ClanInfo.Name;
        }
        else
        {
            m_label_clanName.text = "未加入";
        }
        int job = player.GetProp((int)PlayerProp.Job);
        SelectRoleDataBase roledata = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)1);
        if (roledata != null && null != m__headicon)
        {
            UIManager.GetTextureAsyn(roledata.strprofessionIcon, ref m_playerCASD, () =>
            {
                if (m__headicon != null)
                {
                    m__headicon.mainTexture = null;
                }
            }, m__headicon,false);
        }


        //神魔经验
        //         uint shenmoLevel = (uint)player.GetProp((int)PlayerProp.GodLevel) + 1;
        //         GodDemonDataBase god = GameTableManager.Instance.GetTableItem<GodDemonDataBase>(shenmoLevel);
        //         string godExp = "";
        //         float godSliderValue = 0.0f;
        //         if (god == null)
        //         {
        //             godExp = "--/--";
        //             godSliderValue = 0.0f;
        //         }
        //         else 
        //         {
        //             godExp = string.Format("{0}/{1}", DataManager.Manager<HeartSkillManager>().GhostExp, god.up_exp);
        //             godSliderValue = (DataManager.Manager<HeartSkillManager>().GhostExp + 0.0f) / god.up_exp;
        //         }
        //         m_label_godexp_percent.text = godExp;
        //         m_slider_GodExpslider.value = godSliderValue;

        #region 基础属性label赋值
        //基础属性面板获取数据
        m_label_liliang.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.Strength).ToString());
        m_label_minjie.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.Agility).ToString());
        m_label_zhili.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.Intelligence).ToString());
        m_label_tili.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.Corporeity).ToString());
        m_label_jingli.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.Spirit).ToString());
        m_label_wuligongji.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.PhysicsAttack).ToString() + "-"
                                  + player.GetProp((int)FightCreatureProp.MaxPhysicsAttack).ToString());
        m_label_fashugongji.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.MagicAttack).ToString() + "-"
                                  + player.GetProp((int)FightCreatureProp.MaxMagicAttack).ToString());
        m_label_wulifangyu.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.PhysicsDefend).ToString() + "-"
                                  + player.GetProp((int)FightCreatureProp.MaxPhysicsDefend).ToString());
        m_label_fashufangyu.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.MagicDefend).ToString() + "-"
                                  + player.GetProp((int)FightCreatureProp.MaxMagicDefend).ToString());
        m_label_zhiliao.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.Cure).ToString());
        m_label_mingzhong.text = string.Format("[00ff00]{0:N2}", ((player.GetProp((int)FightCreatureProp.Hit) / 100.0)).ToString("g0") + "%");
        m_label_shanbi.text = string.Format("[00ff00]{0:N2}", ((player.GetProp((int)FightCreatureProp.Dodge) / 100.0)).ToString("g0") + "%");
        m_label_wulibaoji.text = string.Format("[00ff00]{0:N2}", ((player.GetProp((int)FightCreatureProp.PhysicsCrit) / 100.0)).ToString("g0") + "%");
        m_label_fashubaoji.text = string.Format("[00ff00]{0:N2}", ((player.GetProp((int)FightCreatureProp.MagicCrit) / 100.0)).ToString("g0") + "%");
        m_label_baojishanghai.text = string.Format("[00ff00]{0:N2}", ((player.GetProp((int)FightCreatureProp.CritiRatio) / 100.0)).ToString("g0") + "%");


        m_label_huogong.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.FireAttack).ToString());
        m_label_binggong.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.IceAttack).ToString());
        m_label_diangong.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.EleAttack).ToString());
        m_label_angong.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.WitchAttack).ToString());
        m_label_huofang.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.FireDefend).ToString());
        m_label_bingfang.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.IceDefend).ToString());
        m_label_dianfang.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.EleDefend).ToString());
        m_label_anfang.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.WitchDefend).ToString());

        m_label_shanghaijiashen.text = string.Format("[00ff00]{0:N2}", ((player.GetProp((int)FightCreatureProp.HarmDeepen) / 100.0)).ToString("g0") + "%");
        m_label_wushangxishou.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.PhysicsAbsorb).ToString());
        m_label_fashangxishou.text = string.Format("[00ff00]{0}", player.GetProp((int)FightCreatureProp.MagicAbsorb).ToString());
        m_label_shanghaixishou.text = string.Format("[00ff00]{0:N2}", ((player.GetProp((int)FightCreatureProp.HarmAbsorb) / 100.0)).ToString("g0") + "%");
        #endregion

        #region 详情赋值

        //         m_label_dengji.text = player.GetProp((int)CreatureProp.Level).ToString();
        //         m_label_shenmodengji.text = player.GetProp((int)PlayerProp.GodLevel).ToString();
        //         m_label_hongmingzhi.text = player.GetProp((int)PlayerProp.PKValue).ToString();

        //         if (DataManager.Manager<ClanManger>().IsJoinClan)
        //         {
        //             m_label_shizu.text = DataManager.Manager<ClanManger>().ClanInfo.Name;
        //         }
        //         else
        //         {
        //             m_label_shizu.text = "未加入";
        //         }
        m_label_bianhao.text = player.GetID().ToString();
        m_label_PKNum.text = player.GetProp((int)PlayerProp.PKValue).ToString();
        m_label_shengmingzhi.text = player.GetProp((int)CreatureProp.MaxHp).ToString();
        m_label_fashuzhi.text = player.GetProp((int)CreatureProp.MaxMp).ToString();
        m_label_liliang2.text = m_label_liliang.text;
        m_label_minjie2.text = m_label_minjie.text;
        m_label_zhili2.text = m_label_zhili.text;
        m_label_tili2.text = m_label_tili.text;
        m_label_jingli2.text = m_label_jingli.text;
        m_label_wuligongji2.text = m_label_wuligongji.text;
        m_label_wulifangyu2.text = m_label_wulifangyu.text;
        m_label_zhiliao2.text = m_label_zhiliao.text;
        m_label_fashugongji2.text = m_label_fashugongji.text;
        m_label_fashufangyu2.text = m_label_fashufangyu.text;
        #endregion

        RefreshWorldLevel(m_uintWorldLevel);

    }

    /// <summary>
    /// 第三
    /// </summary>
    void OnDisable()
    {
        currUID = 0;
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {


        return true;
    }
    void onClick_BasePropContent_Btn(GameObject caster)
    {
        //点击这里隐藏的基础属性面板basePropTextContainer显示出来
        // basePropTextContainer_obj = m_widget_basePropTextContainer.gameObject;
        m_scrollview_detaiContent.gameObject.SetActive(false);
        if (m_scrollview_basePropTextContainer.gameObject != null)
        {
            m_scrollview_basePropTextContainer.gameObject.SetActive(true);
        }
    }
    void onClick_DetailPropContent_Btn(GameObject caster)
    {
        m_scrollview_basePropTextContainer.gameObject.SetActive(false);
        if (m_scrollview_detaiContent.gameObject != null)
        {
            m_scrollview_detaiContent.gameObject.SetActive(true);
        }

    }

    void onClick_Prop_Btn(GameObject caster)
    {
        m_trans_TitleContent.gameObject.SetActive(false);
    }
    void onClick_Integral_Btn(GameObject caster)
    {

    }

    void onClick_Setting_Btn(GameObject caster)
    {
        //           public void ShowTipsAction()
        //         {
        //             Action yes = delegate
        //             {
        //                 if (Protocol.Instance.IsReconnecting)
        //                 {
        //                     if (DataManager.Manager<CommonWaitDataManager>().ReconnectTime == DataManager.Manager<CommonWaitDataManager>().ShowWaitPanelTimes)
        //                     {
        //                         DataManager.Manager<CommonWaitDataManager>().ReconnectTime = 0;
        //                         DataManager.Manager<LoginDataManager>().Logout();
        //                     }
        //                     else
        //                     {
        //                         Protocol.Instance.ShowWaitPanel();
        //                         Protocol.Instance.StartReconnectServerTimer();
        //                     }
        //                 }
        //             };
        //             Action no = delegate
        //             {
        //                 if (Protocol.Instance.IsReconnecting)
        //                 {
        //                     Protocol.Instance.CloseHeartMsg();
        //                     DataManager.Manager<LoginDataManager>().Logout();
        //                 }
        //             };
        //             DataManager.Manager<TipsManager>().ShowTipWindow(Client.TipWindowType.CancelOk, Game.UpgradeDesc.NetWorkError, yes, no);
        //         }
    }
    void onClick_Close_btn_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.PropPanel);

    }
    void onClick_Sprite_Btn(GameObject caster)
    {
        NetService.Instance.Send(new stRequestPkRemainTimePropertyUserCmd_CS());
    }

    void OnResPkRemainTimeViewPanel(uint time) 
    {
        m_leftTime = time;
        ViewRemainTimeContent(true);   
    }
    void ViewRemainTimeContent(bool show) 
    {
        m_label_RemainTimeLabel.gameObject.SetActive(show);
        TimerAxis.Instance().KillTimer(TIPS_TIMERID, this);
        if (show)
        {
            TimerAxis.Instance().SetTimer(TIPS_TIMERID, 1000, this);
            string msg = "";
            if (m_leftTime <= 0)
            {
                msg = "0";
            }
            else 
            {
                msg = DateTimeHelper.ParseTimeSecondsFliter((int)m_leftTime);
            }
            if(msg == "已过期")
            {
                msg = "0";
            }
            m_label_RemainTimeLabel.text = msg;
        }
    }
    uint m_leftTime = 0;
    private const int TIPS_TIMERID = 1000;
    public void OnTimer(uint uTimerID)
    {
        if (this.m_leftTime > 0)
        {
            string msg = DateTimeHelper.ParseTimeSecondsFliter((int)m_leftTime);
            if (msg == "已过期")
            {
                msg = "0";
            }
            m_label_RemainTimeLabel.text = msg;
            this.m_leftTime--;               
        }
        else
        {
            m_label_RemainTimeLabel.text = "0";
            TimerAxis.Instance().KillTimer(TIPS_TIMERID, this);
        }
    }
    void onClick_BgCollider_Btn(GameObject caster)
    {
        ViewRemainTimeContent(false);
    }

    #region btns

    void onClick_UI_Btn(GameObject caster)
    {
        //UIToggle toggle = caster.GetComponent<UIToggle>();
        //m_GraphicOption.ui = toggle.value;
    }

    void onClick_Model_Btn(GameObject caster)
    {
        UIToggle toggle = caster.GetComponent<UIToggle>();
        m_GraphicOption.model = toggle.value;
    }

    void onClick_Headname_Btn(GameObject caster)
    {
        UIToggle toggle = caster.GetComponent<UIToggle>();
        m_GraphicOption.headName = toggle.value;

    }

    void onClick_Scenes_face_Btn(GameObject caster)
    {
        UIToggle toggle = caster.GetComponent<UIToggle>();
        m_GraphicOption.terrain = toggle.value;
    }

    void onClick_Scehes_sod_Btn(GameObject caster)
    {
        UIToggle toggle = caster.GetComponent<UIToggle>();
        m_GraphicOption.terrainObj = toggle.value;
    }

    void onClick_Grass_Btn(GameObject caster)
    {
        UIToggle toggle = caster.GetComponent<UIToggle>();
        m_GraphicOption.grass = toggle.value;
    }

    void onClick_Shadow_Btn(GameObject caster)
    {
        UIToggle toggle = caster.GetComponent<UIToggle>();
        m_GraphicOption.shadow = toggle.value;
    }

    void onClick_MapArea_Btn(GameObject caster)
    {
        UIToggle toggle = caster.GetComponent<UIToggle>();
        ClientGlobal.Instance().GetMapSystem().SetMapAreaVisible(toggle.value);
    }

    void onClick_Rolepos_Btn(GameObject caster)
    {
        UIToggle toggle = caster.GetComponent<UIToggle>();
        EntitySystem.EntityConfig.m_bShowMainPlayerServerPosCube = toggle.value;
    }

    #endregion
}