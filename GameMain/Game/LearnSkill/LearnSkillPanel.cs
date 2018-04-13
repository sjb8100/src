using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
using Engine.Utility;
using GameCmd;
partial class LearnSkillPanel
{
    LearnSkillDataManager skilldataManager
    {
        get
        {
            return DataManager.Manager<LearnSkillDataManager>();
        }
    }
    public enum LearnSkillPanelPageEnum
    {
        None = 0,
        Page_LearnSkill = 1,
        Page_HeartSkill = 2,
        Max,
    }
    LearnSkillPanelPageEnum curPageEnum = LearnSkillPanelPageEnum.Page_LearnSkill; //默认大页签

    SortedDictionary<uint, LeftSkillItem> m_dicLeftItem = new SortedDictionary<uint, LeftSkillItem>();

    Dictionary<int, RightSkillItem> m_dicRightItem = new Dictionary<int, RightSkillItem>();

    bool m_bLeftChange = false;
    /// <summary>
    /// 左边的显示切换图标 
    /// </summary>
    bool BLeftChange
    {
        get
        {
            return m_bLeftChange;
        }
        set
        {
            m_bLeftChange = value;
            OnShowLeftChange();
        }
    }

    bool m_bRightChange = false;
    bool BRightChange
    {
        get
        {
            return m_bRightChange;
        }
        set
        {
            m_bRightChange = value;
            OnShowRightChange();
        }
    }
    SkillDatabase curDataBase = null;
    public SkillDatabase CurDataBase
    {
        get
        {
            return curDataBase;
        }
        set
        {
            curDataBase = value;
            UpdateShengjiLabel();

        }
    }
    /// <summary>
    /// 要设置的技能id
    /// </summary>
    uint m_uWillSetSkillID = 0;
    /// <summary>
    /// 要设置的位置
    /// </summary>
    int m_nLoction = 0;

    UITabGridGroup m_tabGroup;
    protected override void OnLoading()
    {
        base.OnLoading();
        InitTabGrid();
        skilldataManager.ValueUpdateEvent += skilldataManager_ValueUpdateEvent;

        skilldataManager.InitData();
        List<SkillDatabase> commonSkillData = skilldataManager.GetCommonSkillDataBase();
        CurDataBase = commonSkillData[0];
        InitLearnControls();

        //重置心法道具IDId
        m_resetSkillItemId = (uint)GameTableManager.Instance.GetGlobalConfig<int>("ResetSkillItemId");

        //加心法点道具ID
        m_addPointItemId = (uint)GameTableManager.Instance.GetGlobalConfig<int>("AddPointItemId");
    }

    void DoGameEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.HEARTSKILLUPGRADE)
        {
            InitHeartSkillUI();//全部刷新
        }
        else if (eventID == (int)GameEventID.HEARTSKILLRESET)
        {
            InitHeartSkillUI();
        }
        else if (eventID == (int)GameEventID.HEARTSKILLGODDATA)
        {
            UpdateTopUI();
            InitHeartSkillGridList();   //刷新左侧的grid
            UpdateRedPoint();
        }
    }

    void RegisterEvent(bool b)
    {
        //只处理心法的
        bool isHeartSkillOpen = DataManager.Manager<HeartSkillManager>().IsHeartSkillOpen();
        if (isHeartSkillOpen)
        {
            if (b)
            {
                Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.HEARTSKILLUPGRADE, DoGameEvent);
                Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.HEARTSKILLRESET, DoGameEvent);
                Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.HEARTSKILLGODDATA, DoGameEvent);
            }
            else
            {
                Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.HEARTSKILLUPGRADE, DoGameEvent);
                Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.HEARTSKILLRESET, DoGameEvent);
                Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.HEARTSKILLGODDATA, DoGameEvent);
            }
        }
    }


    void skilldataManager_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e != null)
        {
            if (e.key == LearnSkillDispatchEvents.SkillLevelUP.ToString())
            {
                SkillInfo newInfo = (SkillInfo)e.newValue;
                SortedDictionary<uint, LeftSkillItem>.Enumerator itemIter = m_dicLeftItem.GetEnumerator();
                while (itemIter.MoveNext())
                {
                    if (itemIter.Current.Key == newInfo.skillID)
                    {
                        LeftSkillItem skillItem = itemIter.Current.Value;
                        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(newInfo.skillID, (int)newInfo.level);
                        if (db != null)
                        {
                            skillItem.InitItem(db);
                            skillItem.AddEffectInSkillPanel();

                        }
                    }
                }

                //页签红点提示
                UpdateRedPoint();
            }
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        skilldataManager.ValueUpdateEvent -= skilldataManager_ValueUpdateEvent;
    }


    protected override void OnShow(object data)
    {
        base.OnShow(data);

        RegisterEvent(true);

        skilldataManager.ShowState = skilldataManager.CurState;
        List<SkillDatabase> commonSkillData = skilldataManager.GetCommonSkillDataBase();
        CurDataBase = commonSkillData[0];
        SetAllSkillSettingItem();

        UpdateRedPoint();//红点提示

        Transform changeTrans = m__stateSpr.transform.parent;
        if (changeTrans != null)
        {
            UIEventListener.Get(changeTrans.gameObject).onClick = ChangeClick;
        }
    }
    void InitTabGrid()
    {

        if (m_trans_skill_setting.GetComponent<UITabGrid>() == null)
        {
            m_trans_skill_setting.gameObject.AddComponent<UITabGrid>();
        }
        if (m_trans_skill_shengji.GetComponent<UITabGrid>() == null)
        {
            m_trans_skill_shengji.gameObject.AddComponent<UITabGrid>();
        }
        if (m_trans_up.GetComponent<UITabGridGroup>() == null)
        {
            m_tabGroup = m_trans_up.gameObject.AddComponent<UITabGridGroup>();
            m_tabGroup.InitGroup();
        }
        UIEventListener.Get(m_trans_skill_shengji.gameObject).onClick = onClick_Skill_shengji_Btn;
        UIEventListener.Get(m_trans_skill_setting.gameObject).onClick = onClick_Skill_setting_Btn;
    }
    public override void Init()
    {
        base.Init();

    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);

        if (jumpData == null)
        {
            UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, (int)LearnSkillPanelPageEnum.Page_LearnSkill);

            onClick_Skill_shengji_Btn(m_trans_skill_shengji.gameObject);
        }
        else
        {
            if (jumpData.Tabs != null)
            {
                if (jumpData.Tabs.Length > 0)
                {
                    int pageid = jumpData.Tabs[0];
                    UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, pageid);
                }
            }
        }
    }
    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            curPageEnum = (LearnSkillPanelPageEnum)pageid;
            //技能
            if ((LearnSkillPanelPageEnum)pageid == LearnSkillPanelPageEnum.Page_LearnSkill)
            {
                OnClickLearnSkillBtn();//点技能
                onClick_Skill_shengji_Btn(m_trans_skill_shengji.gameObject);
            }
            //心法
            else if ((LearnSkillPanelPageEnum)pageid == LearnSkillPanelPageEnum.Page_HeartSkill)
            {
                OnClickHeartSkillBtn();//点心法
            }

        }

        return true;
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[1];
        pd.JumpData.Tabs[0] = (int)curPageEnum;
        return pd;
    }

    protected override void OnHide()
    {
        base.OnHide();

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLINFO_REFRESH, null);

        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eSkillBtnRefresh, null);
        ResetChange();
        DataManager.Manager<LearnSkillDataManager>().DispatchRedPoingMsg();
        Release();

        RegisterEvent(false);
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
            if (dicTabs != null && dicTabs.TryGetValue((int)LearnSkillPanelPageEnum.Page_HeartSkill, out tabGrid))
            {
                tabGrid.SetRedPointStatus(HSManager.HaveHeartSkillEnableUpgrade());
            }

            if (dicTabs != null && dicTabs.TryGetValue((int)LearnSkillPanelPageEnum.Page_LearnSkill, out tabGrid))
            {
                tabGrid.SetRedPointStatus(skilldataManager.HaveSkillUpgrade());
            }

        }
        InitLearnControls();
    }

    void InitLearnControls()
    {
        BoxCollider[] commonBtnArray = m_widget_currency.GetComponentsInChildren<BoxCollider>();
        List<SkillDatabase> commonSkillData = skilldataManager.GetCommonSkillDataBase();
        for (int i = 0; i < commonBtnArray.Length; i++)
        {
            var commbtn = commonBtnArray[i];

            UIEventListener.Get(commbtn.gameObject).onClick = OnItemClick;
            if (i < commonSkillData.Count)
            {
                SetSkillItemInfo(commbtn.gameObject, commonSkillData[i]);
            }
        }


        BoxCollider[] skillOneArray = m_widget_state_1.GetComponentsInChildren<BoxCollider>();
        List<SkillDatabase> stateOneData = skilldataManager.GetShowStateOneList();
        for (int i = 0; i < skillOneArray.Length; i++)
        {
            var state = skillOneArray[i];
            if (i < stateOneData.Count)
            {
                SetSkillItemInfo(state.gameObject, stateOneData[i]);
            }
            UIEventListener.Get(state.gameObject).onClick = OnItemClick;
        }

        BoxCollider[] skillTwoArray = m_widget_state_2.GetComponentsInChildren<BoxCollider>();
        List<SkillDatabase> stateTwoData = skilldataManager.GetShowStateTwoList();
        for (int i = 0; i < skillTwoArray.Length; i++)
        {
            var state = skillTwoArray[i];
            if (i < stateTwoData.Count)
            {
                SetSkillItemInfo(state.gameObject, stateTwoData[i]);
            }
            UIEventListener.Get(state.gameObject).onClick = OnItemClick;

        }
        int count = m_widget_SkillContainer.transform.childCount;
        for (int i = 1; i < count; i++)
        {
            string name = "skill_" + i;
            Transform trans = m_widget_SkillContainer.transform.Find(name);
            if (trans != null)
            {
                UIEventListener.Get(trans.gameObject).onClick = OnRightItemClick;
            }
        }
    }
    /// <summary>
    /// 设置左边的面板
    /// </summary>
    public void ResetLeftItem()
    {
        var iter = m_dicLeftItem.GetEnumerator();
        while (iter.MoveNext())
        {
            var dic = iter.Current;
            dic.Value.ResetState();
        }
    }
    /// <summary>
    /// 设置右边的设置面板
    /// </summary>
    public void SetAllSkillSettingItem()
    {
        ClearSettingItem();
        SortedDictionary<int, uint> stateDic = skilldataManager.GetSkillSettingState();
        if (stateDic != null)
        {

            var iter = stateDic.GetEnumerator();
            while (iter.MoveNext())
            {
                var dic = iter.Current;
                int location = dic.Key;
                uint skillID = dic.Value;
                if (skillID == 0)
                    continue;
                string name = skilldataManager.GetParentNameByLocation(location);
                Transform trans = m_widget_SkillContainer.transform.Find(name);
                if (trans != null)
                {
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, 1);
                    RightSkillItem item = trans.GetComponent<RightSkillItem>();
                    if (item != null)
                    {
                        item.InitItem(db);
                    }
                }

            }
        }
        UpdateCurStateLabel();
        ShowAutoAttack();
        #region drag code
        //ClearSettingItem();
        //SortedDictionary<int , uint> stateDic = skilldataManager.GetSkillSettingState();
        //if ( stateDic != null )
        //{
        //    foreach ( var dic in stateDic )
        //    {
        //        int location = dic.Key;
        //        uint skillID = dic.Value;
        //        if ( skillID == 0 )
        //            continue;
        //        string name = skilldataManager.GetParentNameByLocation( location );
        //        GameObject go = GetSelectPrefab( skillID );
        //        if ( go != null )
        //        {
        //            if ( go.GetComponent<SkillDragDropItem>() == null )
        //            {
        //                SkillDragDropItem item = go.AddComponent<SkillDragDropItem>();
        //                item.cloneOnDrag = false;
        //            }

        //        }
        //        SetItemLocation( go , name );
        //    }
        //}
        //UpdateCurStateLabel();
        //ShowAutoAttack();
        #endregion
    }
    void SetSkillItemInfo(GameObject go, SkillDatabase db)
    {
        LeftSkillItem item = go.GetComponent<LeftSkillItem>();
        if (item == null)
        {
            item = go.AddComponent<LeftSkillItem>();
        }
        if (!m_dicLeftItem.ContainsKey(db.wdID))
        {
            m_dicLeftItem.Add(db.wdID, item);
        }
        item.InitItem(db);
        #region drag code
        //if(go.GetComponent<LearnSkillItem>() == null)
        //{
        //    go.AddComponent<LearnSkillItem>().InitItem( db );
        //}
        //else
        //{
        //    LearnSkillItem item = go.GetComponent<LearnSkillItem>();
        //    item.InitItem( db );
        //}
        //if(go.GetComponent<SkillDragDropItem>() == null)
        //{
        //    SkillDragDropItem item = go.AddComponent<SkillDragDropItem>();
        //    item.cloneOnDrag = true;
        //}
        #endregion


    }

    void OnClickLearnSkillBtn()
    {
        m_trans_LearnSkillContent.gameObject.SetActive(true);
        m_trans_HeartSkillContent.gameObject.SetActive(false);
    }


    void onClick_BtnStatus1_Btn(GameObject caster)
    {
        onClick_Arrow_left_Btn(null);
    }

    void onClick_BtnStatus2_Btn(GameObject caster)
    {
        onClick_Arrow_right_Btn(null);
    }

    void onClick_BtnClearSkillSet_Btn(GameObject caster)
    {
        ResetAllSkill();
    }
    void UpdateShengjiLabel()
    {
        if (CurDataBase != null)
        {
            int playerLevel = skilldataManager.GetPlayerLevel();
            SkillDatabase bseDataBase = GameTableManager.Instance.GetTableItem<SkillDatabase>(CurDataBase.wdID, (int)1);
            if (playerLevel < CurDataBase.dwNeedLevel)
            {//未解锁
                m_trans_WeiJieSuoContent.gameObject.SetActive(true);
                m_trans_JieSuoContent.gameObject.SetActive(false);
                m_trans_MaxContent.gameObject.SetActive(false);
                m_trans_MaxHideContent.gameObject.SetActive(false);
            }
            else
            {
                m_trans_WeiJieSuoContent.gameObject.SetActive(false);
                m_trans_JieSuoContent.gameObject.SetActive(true);
                m_trans_MaxContent.gameObject.SetActive(false);
                m_trans_MaxHideContent.gameObject.SetActive(true);
            }
            uint level = CurDataBase.wdLevel;
            #region maxlevel
            if (level == CurDataBase.dwMaxLevel)
            {
                m_trans_WeiJieSuoContent.gameObject.SetActive(false);
                m_trans_JieSuoContent.gameObject.SetActive(false);
                m_trans_MaxContent.gameObject.SetActive(true);
                m_trans_MaxHideContent.gameObject.SetActive(false);
            }
            #endregion

            #region lock
            uint minusCd = skilldataManager.GetHeartCd(CurDataBase.wdID);
            m_label_Name.text = CurDataBase.strName;
            m_label_Level.text = CurDataBase.wdLevel.ToString() + CommonData.GetLocalString("级");
            m_label_Mp.text = CurDataBase.costsp.ToString();
            uint cd = CurDataBase.dwIntervalTime - minusCd;
            string cdStr = cd.ToString();
            if (minusCd != 0)
            {
                cdStr = ColorManager.GetColorString(49, 170, 240, 255, cdStr);
            }
            m_label_CD.text = cdStr + CommonData.GetLocalString("秒");
            m_label_Formneed.text = skilldataManager.GetStatus(CurDataBase.dwSkillSatus);
            m_label_Describe.text = CurDataBase.strDesc;
            string heartDes = skilldataManager.GetHeartDes(CurDataBase.wdID);
            if (!string.IsNullOrEmpty(heartDes))
            {
                m_label_Describe.text = CurDataBase.strDesc + "\n" + ColorManager.GetColorString(49, 170, 240, 255, heartDes);
            }

            int playerExp = skilldataManager.GetPlayerExp();
            uint nextLevel = level + 1;
            SkillDatabase nextDataBase = GameTableManager.Instance.GetTableItem<SkillDatabase>(CurDataBase.wdID, (int)nextLevel);
            if (nextDataBase != null)
            {
                m_label_Describe_NextLevel.text = nextDataBase.strDesc;
                m_label_Exp_Cost.text = nextDataBase.dwExp.ToString();
                m_label_Gold_Cost.text = nextDataBase.dwMoney.ToString();
                if (MainPlayerHelper.GetMainPlayer() != null)
                {
                    int money = MainPlayerHelper.GetMoneyNumByType(ClientMoneyType.Gold);
                    if (money < nextDataBase.dwMoney)
                    {
                        m_label_Gold_Cost.color = new Color(254 * 1.0f / 255, 42 * 1.0f / 255, 21 * 1.0f / 255);
                    }
                    else
                    {
                        m_label_Gold_Cost.color = Color.white;
                    }
                }

                if (playerExp > nextDataBase.dwExp)
                {
                    m_label_Exp_Cost.color = Color.white;
                }
                else
                {
                    m_label_Exp_Cost.color = new Color(254 * 1.0f / 255, 42 * 1.0f / 255, 21 * 1.0f / 255);
                }
                m_label_WeiJieSuoLocklevel.text = nextDataBase.dwNeedLevel.ToString();
                m_label_Locklevel.text = nextDataBase.dwNeedLevel.ToString();
                if (playerLevel >= nextDataBase.dwNeedLevel)
                {
                    m_label_WeiJieSuoLocklevel.color = Color.white;
                    m_label_Locklevel.color = Color.white;
                }
                else
                {
                    m_label_WeiJieSuoLocklevel.color = new Color(254 * 1.0f / 255, 42 * 1.0f / 255, 21 * 1.0f / 255);
                    m_label_Locklevel.color = new Color(254 * 1.0f / 255, 42 * 1.0f / 255, 21 * 1.0f / 255);
                }
            }
            if (level == 0)
            {
                m_trans_WeiJieSuoContent.gameObject.SetActive(true);
                m_trans_JieSuoContent.gameObject.SetActive(false);
            }

            m_label_Exp_Now.text = playerExp.ToString();


            #endregion

            UpdateCurStateLabel();
        }
    }
    void UpdateCurStateLabel()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        uint job = (uint)mainPlayer.GetProp((int)PlayerProp.Job);
        m_label_StateOneLabel.text = skilldataManager.GetStatus(job);
        m_label_StateTwoLabel.text = skilldataManager.GetStatus(job + 10);
        uint changeSkill = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeStateSkillID", job.ToString());
        if (skilldataManager.ShowState == SkillSettingState.StateOne)
        {
            m_label_CurStateLabel.text = skilldataManager.GetStatus(job);
        }
        if (skilldataManager.ShowState == SkillSettingState.StateTwo)
        {
            m_label_CurStateLabel.text = skilldataManager.GetStatus(job + 10);
            changeSkill = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeStateSkillID", (job + 10).ToString());
        }

        int unLockLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChangeStateLevel");
        if (MainPlayerHelper.GetPlayerLevel() < unLockLevel)
        {

            m_label_state_unlocklevel.text = unLockLevel.ToString();
            m_sprite_statelock.gameObject.SetActive(true);


        }
        else
        {
            m_sprite_statelock.gameObject.SetActive(false);

            SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(changeSkill, 1);
            if (db != null)
            {
                string iconName = UIManager.BuildCircleIconName(db.iconPath);
                UIManager.GetTextureAsyn(iconName, ref m_curIconAsynSeed, () =>
                {
                    if (null != m__stateSpr)
                    {
                        m__stateSpr.mainTexture = null;
                    }
                }, m__stateSpr);
                //  DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_stateSpr, db.iconPath,false,true);
                //m_sprite_stateSpr.spriteName = db.iconPath;
            }
        }

    }

    void ChangeClick(GameObject go)
    {
        if (skilldataManager.ShowState == SkillSettingState.StateOne)
        {
            onClick_Arrow_right_Btn(null);
        }
        else
        {
            onClick_Arrow_left_Btn(null);
        }
    }
    GameObject GetSelectPrefab(uint skillID)
    {
        //List<string> deList;
        //SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>( skillID , 1 );
        //if ( db != null )
        //{
        //    UnityEngine.Object select = UILoader.Instance.LoadPrefab( "panel/LearnSkill/select" , out deList );
        //    GameObject go = GameObject.Instantiate( select ) as GameObject;
        //    LearnSkillItem item = go.GetComponent<LearnSkillItem>();
        //    if ( item == null )
        //    {
        //        item = go.AddComponent<LearnSkillItem>();
        //    }
        //    UISprite spr = go.GetComponent<UISprite>();
        //    if ( spr != null )
        //    {
        //        spr.spriteName = db.iconPath;
        //    }
        //    item.InitItem( db );
        //    return go;
        //}
        return null;
    }

    void onClick_Auto_attack_Btn(GameObject caster)
    {
        UIToggle toggle = caster.GetComponent<UIToggle>();
        bool bAutoAttack = toggle.value;
        GameCmd.stSetAutoFlagSkillUserCmd_CS cmd = new GameCmd.stSetAutoFlagSkillUserCmd_CS();
        cmd.flag = (uint)(bAutoAttack ? 1 : 0);
        NetService.Instance.Send(cmd);

    }
    void onClick_Arrow_left_Btn(GameObject caster)
    {
        skilldataManager.ShowState = SkillSettingState.StateOne;
        SetAllSkillSettingItem();
        ResetLeftItem();
        ResetChange();

    }

    void onClick_Arrow_right_Btn(GameObject caster)
    {
        int level = skilldataManager.GetPlayerLevel();
        int needLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChangeStateLevel");
        if (level < needLevel)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Local_TXT_Notice_CannotChangeSkillState);
            return;
        }
        skilldataManager.ShowState = SkillSettingState.StateTwo;
        SetAllSkillSettingItem();
        ResetLeftItem();
        ResetChange();
    }
    void OnRightItemClick(GameObject go)
    {
        int loc = skilldataManager.GetLocation(go.name);
        uint level = skilldataManager.GetUnLockLevelByLoc((uint)loc);
        if (level > MainPlayerHelper.GetPlayerLevel())
        {
            return;
        }
        if (!BRightChange && !BLeftChange)
        {
            BLeftChange = true;
        }
        RightSkillItem rightItem = go.GetComponent<RightSkillItem>();
        if (rightItem != null)
        {
            m_nLoction = loc;
            if (BRightChange)
            {
                if (m_uWillSetSkillID == 0)
                {
                    TipsManager.Instance.ShowTips(LocalTextType.Skill_Commond_qingxuanzhongyigekeyitihuandejineng);
                    return;
                }
                BRightChange = false;

                SendSetSkillMessage();
            }
            if (BLeftChange)
            {
                rightItem.SetItemState(RightLearnSkillItemState.SetRightSelect);
            }
        }
        //string name = go.name;
        //int index = -1;
        //int len = name.Length - 1;
        //string indexStr = name[len].ToString();
        //if ( int.TryParse( indexStr , out index ) )
        //{

        //}
    }
    void SendSetSkillMessage()
    {
        if (m_uWillSetSkillID > 0 && m_nLoction > 0)
        {
            skilldataManager.SendSetSkillMessage(m_nLoction, m_uWillSetSkillID, SkillSettingAction.Add);
        }
    }
    void OnItemClick(GameObject go)
    {
        LeftSkillItem item = go.GetComponent<LeftSkillItem>();
        if (item == null)
        {
            return;
        }
        m_uWillSetSkillID = item.ItemDataBase.wdID;
        CurDataBase = item.ItemDataBase;
        if (!skilldataManager.IsSettingPanel)
        {
            SkillDatabase bseDataBase = GameTableManager.Instance.GetTableItem<SkillDatabase>(CurDataBase.wdID, (int)1);
            if (MainPlayerHelper.GetPlayerLevel() < bseDataBase.dwNeedLevel)
            {
                item.SetItemState(LeftLearnSkillItemState.LockSelect);
            }
            else
            {
                item.SetItemState(LeftLearnSkillItemState.OpenNotSetAndSelect);
            }

            return;
        }
        if (!BRightChange && !BLeftChange)
        {
            BRightChange = true;
        }
        if (BRightChange)
        {
            ResetLeftItem();
            if (item.ItemState == LeftLearnSkillItemState.OpenNotSetAndNotSelect)
            {
                item.SetItemState(LeftLearnSkillItemState.OpenNotSetAndSelect);
            }
            else
            {
                BRightChange = false;
                m_uWillSetSkillID = 0;
            }
        }
        if (BLeftChange)
        {
            if (item.ItemState == LeftLearnSkillItemState.OpenNotSetAndCanSet)
            {
                BLeftChange = false;
                SendSetSkillMessage();
            }
        }
        #region dragcode
        //LearnSkillItem item = go.GetComponent<LearnSkillItem>();
        //if ( item == null )
        //    return;
        #endregion

    }
    #region 显示切换技能逻辑
    void ResetChange()
    {
        BRightChange = false;
        BLeftChange = false;
        m_uWillSetSkillID = 0;
    }
    void OnShowLeftChange()
    {

        var iter = m_dicLeftItem.GetEnumerator();
        while (iter.MoveNext())
        {
            var dic = iter.Current;
            LeftSkillItem item = dic.Value;
            if (m_bLeftChange)
            {
                if (item.ItemState == LeftLearnSkillItemState.OpenNotSetAndNotSelect || item.ItemState == LeftLearnSkillItemState.OpenNotSetAndSelect)
                {
                    item.SetItemState(LeftLearnSkillItemState.OpenNotSetAndCanSet);
                }
            }
            else
            {
                item.ResetState();
            }
        }

    }
    void OnShowRightChange()
    {
        var iter = m_dicRightItem.GetEnumerator();
        while (iter.MoveNext())
        {
            var dic = iter.Current;
            RightSkillItem item = dic.Value;
            if (m_bRightChange)
            {
                uint level = item.GetUnLockLevel();
                if (level > MainPlayerHelper.GetPlayerLevel())
                {
                    item.SetItemState(RightLearnSkillItemState.Lock);
                }
                else
                {
                    item.SetItemState(RightLearnSkillItemState.SetRightCanChange);
                }
            }
            else
            {
                item.ResetState();
            }
        }
    }
    #endregion
    void onClick_Skill_shengji_Btn(GameObject obj)
    {

        m_tabGroup.SetHighLightTabGameObject(m_trans_skill_shengji.gameObject);
        skilldataManager.IsSettingPanel = false;
        m_trans_skill_shengji_area.gameObject.SetActive(true);
        m_widget_skill_setting_area.gameObject.SetActive(false);

        ResetChange();
        ShowShengjiItemState();
    }
    void ShowShengjiItemState()
    {
        List<uint> keyList = m_dicLeftItem.Keys.ToList<uint>();
        for (int i = 0; i < keyList.Count; i++)
        {
            uint keyID = keyList[i];
            LeftSkillItem item = m_dicLeftItem[keyID];
            SkillDatabase skillDb = GameTableManager.Instance.GetTableItem<SkillDatabase>(item.ItemDataBase.wdID, 1);
            if (skillDb == null)
            {
                return;
            }
            if (MainPlayerHelper.GetPlayerLevel() < skillDb.dwNeedLevel)
            {//未解锁
                if (i == 0)
                {
                    item.SetItemState(LeftLearnSkillItemState.LockSelect);
                }
                else
                {
                    item.SetItemState(LeftLearnSkillItemState.Lock);
                }
            }
            else
            {
                if (i == 0)
                {
                    item.SetItemState(LeftLearnSkillItemState.OpenNotSetAndSelect);
                }
                else
                {
                    item.SetItemState(LeftLearnSkillItemState.OpenNotSetAndNotSelect);
                }
            }
        }
    }
    void ResetAllSkill()
    {
        GameCmd.stClearUsePosSkillUserCmd_CS cmd = new GameCmd.stClearUsePosSkillUserCmd_CS();
        cmd.state = (uint)DataManager.Manager<LearnSkillDataManager>().ShowState;
        NetService.Instance.Send(cmd);
    }
    void onClick_Skill_setting_Btn(GameObject obj)
    {
        skilldataManager.IsSettingPanel = true;

        m_trans_skill_shengji_area.gameObject.SetActive(false);
        m_widget_skill_setting_area.gameObject.SetActive(true);
        SetAllSkillSettingItem();
        ResetChange();
        m_tabGroup.SetHighLightTabGameObject(m_trans_skill_setting.gameObject);

    }
    void onClick_Btn_AllLevelUp_Btn(GameObject caster)
    {
        SkillLevelUpCode code = skilldataManager.HaveEnoughMoney();

        if (code == SkillLevelUpCode.NoMoney)
        {
            DataManager.Manager<Mall_HuangLingManager>().ShowExchange((uint)ErrorEnum.NotEnoughGold, ClientMoneyType.Gold, "提示", "去兑换", "取消");
            return;
        }
        else if (code == SkillLevelUpCode.LevelNotEnough)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Skill_Commond_renwudengjibuzubunnegshengji);
            return;
        }
        string des = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Skill_Commond_yijianshengji);
        Action agree = delegate
        {
            stAutolearnAllSkillUserCmd_C cmd = new stAutolearnAllSkillUserCmd_C();
            cmd.isAuto = true;
            NetService.Instance.Send(cmd);
        };
        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, des, agree, null, title: "技能升级");

    }
    void onClick_Btn_LevelUp_Btn(GameObject caster)
    {
        int playerLevel = skilldataManager.GetPlayerLevel();
        uint dbLevel = CurDataBase.wdLevel + 1;
        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(CurDataBase.wdID, (int)dbLevel);
        if (db != null)
        {
            if (playerLevel < db.dwNeedLevel)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Skill_Commond_renwudengjibuzubunnegshengji);
                return;
            }
        }
        DataManager.Manager<LearnSkillDataManager>().LearnSkill(CurDataBase.wdID, dbLevel);
    }
    //void SetItemLocation(UnityEngine.GameObject go , string parentName)
    //{
    //    if ( string.IsNullOrEmpty( parentName ) )
    //        return;
    //    Transform trans = m_widget_SkillContainer.transform.Find( parentName );
    //    if ( trans != null )
    //    {
    //        NGUITools.DestroyChildren( trans );
    //        if ( go != null )
    //        {
    //            go.transform.parent = trans;
    //            go.transform.localPosition = Vector3.zero;
    //            go.transform.localScale = Vector3.one;
    //            go.transform.localRotation = Quaternion.identity;
    //        }

    //    }
    //}
    void ClearSettingItem()
    {
        int count = m_widget_SkillContainer.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            string parentName = "skill_" + i;
            Transform trans = m_widget_SkillContainer.transform.Find(parentName);
            if (trans != null)
            {
                RightSkillItem item = trans.GetComponent<RightSkillItem>();
                if (item == null)
                {
                    item = trans.gameObject.AddComponent<RightSkillItem>();
                }
                if (i == 0)
                {
                    uint common = skilldataManager.GetCommonSkillIDByJob();
                    SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(common, 1);
                    item.InitItem(db);
                }
                else
                {
                    item.ResetState();
                    //  item.InitItem(null);
                }

                if (!m_dicRightItem.ContainsKey(i))
                {
                    m_dicRightItem.Add(i, item);
                }
                //NGUITools.DestroyChildren( trans );
            }
        }

    }

    void ShowAutoAttack()
    {
        bool autoAttack = skilldataManager.GetAutoAttack();
        //UIToggle toggle = m_btn_auto_attack.GetComponent<UIToggle>();
        //if (toggle != null)
        //{
        //    toggle.value = autoAttack;
        //}
    }
    void onClick_AutoSkillSet_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.AutoSkillPanel);
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release();
        }
        var iterRight = m_dicRightItem.GetEnumerator();
        while (iterRight.MoveNext())
        {
            var dic = iterRight.Current;
            dic.Value.Release();
        }
        var iter = m_dicLeftItem.GetEnumerator();
        while (iter.MoveNext())
        {
            var dic = iter.Current;
            dic.Value.Release();
        }
    }
}

