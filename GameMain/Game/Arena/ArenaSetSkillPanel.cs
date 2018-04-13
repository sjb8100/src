using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Client;
using Engine.Utility;
partial class ArenaSetSkillPanel
{
    ArenaSetSkillManager skilldataManager
    {
        get
        {
            return DataManager.Manager<ArenaSetSkillManager>();
        }
    }

    SortedDictionary<uint, ArenaLeftSkillItem> m_dicLeftItem = new SortedDictionary<uint, ArenaLeftSkillItem>();

    Dictionary<int, ArenaRightSkillItem> m_dicRightItem = new Dictionary<int, ArenaRightSkillItem>();

    bool m_bLeftChange = false;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

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
    protected override void OnLoading()
    {
        base.OnLoading();


        //skilldataManager.ValueUpdateEvent += skilldataManager_ValueUpdateEvent;

        skilldataManager.InitData();
        List<SkillDatabase> commonSkillData = skilldataManager.GetCommonSkillDataBase();
        CurDataBase = commonSkillData[0];
        InitLearnControls();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

        SortedDictionary<uint, ArenaLeftSkillItem>.Enumerator etr = m_dicLeftItem.GetEnumerator();
        while (etr.MoveNext())
        {
            ArenaLeftSkillItem arenaLeftSkillItem = etr.Current.Value;
            arenaLeftSkillItem.Release();
        }

        Dictionary<int, ArenaRightSkillItem>.Enumerator etr2 = m_dicRightItem.GetEnumerator();
        while (etr2.MoveNext())
        {
            ArenaRightSkillItem arenaRightSkillItem = etr2.Current.Value;
            arenaRightSkillItem.Release();
        }
    }

    /*
    void skilldataManager_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e != null)
        {
            if (e.key == LearnSkillDispatchEvents.SkillLevelUP.ToString())
            {
                SkillInfo newInfo = (SkillInfo)e.newValue;
                SortedDictionary<uint, ArenaLeftSkillItem>.Enumerator itemIter = m_dicLeftItem.GetEnumerator();
                while (itemIter.MoveNext())
                {
                    if (itemIter.Current.Key == newInfo.skillID)
                    {
                        ArenaLeftSkillItem skillItem = itemIter.Current.Value;
                        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(newInfo.skillID, (int)newInfo.level);
                        if (db != null)
                        {
                            skillItem.InitItem(db);
                        }
                    }
                }

            }
        }
    }
    */

    public void OnDestroy()
    {
        //skilldataManager.ValueUpdateEvent -= skilldataManager_ValueUpdateEvent;
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        skilldataManager.ShowState = skilldataManager.CurState;
        List<SkillDatabase> commonSkillData = skilldataManager.GetCommonSkillDataBase();
        CurDataBase = commonSkillData[0];
        SetAllSkillSettingItem();
        //onClick_Skill_shengji_Btn(null);
        if (!skilldataManager.IsSettingPanel)
        {
            ShowShengjiItemState();
        }
        Transform changeTrans = m__stateSpr.transform.parent;
        if (changeTrans != null)
        {
            UIEventListener.Get(changeTrans.gameObject).onClick = ChangeClick;
        }

        onClick_Skill_setting_Btn(null);
    }
    public override void Init()
    {
        base.Init();
    }
    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnHide()
    {
        base.OnHide();

        //Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLINFO_REFRESH, null);

        //DataManager.Manager<UIPanelManager>().SendMsg(PanelID.SkillPanel, UIMsgID.eSkillBtnRefresh, null);
        ResetChange();

        ReleaseAtlas();
    }

    void ReleaseAtlas()
    {
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }
    }

    void InitLearnControls()
    {
        UIButton[] commonBtnArray = m_widget_currency.GetComponentsInChildren<UIButton>();
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


        UIButton[] skillOneArray = m_widget_state_1.GetComponentsInChildren<UIButton>();
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

        UIButton[] skillTwoArray = m_widget_state_2.GetComponentsInChildren<UIButton>();
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
        foreach (var dic in m_dicLeftItem)
        {
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
            foreach (var dic in stateDic)
            {
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
        //ShowAutoAttack();
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
        ArenaLeftSkillItem item = go.GetComponent<ArenaLeftSkillItem>();
        if (item == null)
        {
            item = go.AddComponent<ArenaLeftSkillItem>();
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
            /*
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
            m_label_Name.text = CurDataBase.strName;
            m_label_Level.text = CurDataBase.wdLevel.ToString() + CommonData.GetLocalString("级");
            m_label_Mp.text = CurDataBase.costsp.ToString();
            m_label_CD.text = CurDataBase.dwIntervalTime + CommonData.GetLocalString("秒");
            m_label_Formneed.text = GetStatus(CurDataBase.dwSkillSatus);
            m_label_Describe.text = CurDataBase.strDesc;

            int playerExp = skilldataManager.GetPlayerExp();
            uint nextLevel = level + 1;
            SkillDatabase nextDataBase = GameTableManager.Instance.GetTableItem<SkillDatabase>(CurDataBase.wdID, (int)nextLevel);
            if (nextDataBase != null)
            {
                m_label_Describe_NextLevel.text = nextDataBase.strDesc;
                m_label_Exp_Cost.text = nextDataBase.dwExp.ToString();
                m_label_Gold_Cost.text = nextDataBase.dwMoney.ToString();


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
            */
            UpdateCurStateLabel();
        }
    }
    void UpdateCurStateLabel()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        uint job = (uint)mainPlayer.GetProp((int)PlayerProp.Job);
        m_label_StateOneLabel.text = GetStatus(job);
        m_label_StateTwoLabel.text = GetStatus(job + 10);
        uint changeSkill = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeStateSkillID", job.ToString());
        if (skilldataManager.ShowState == SkillSettingState.StateOne)
        {
            m_label_CurStateLabel.text = GetStatus(job);
        }
        if (skilldataManager.ShowState == SkillSettingState.StateTwo)
        {
            m_label_CurStateLabel.text = GetStatus(job + 10);
            changeSkill = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeStateSkillID", (job + 10).ToString());
        }

        int unLockLevel = GameTableManager.Instance.GetGlobalConfig<int>("ChangeStateLevel");
        if (MainPlayerHelper.GetPlayerLevel() < unLockLevel)
        {

            //m_label_state_unlocklevel.text = unLockLevel.ToString();
            //m_label_state_unlocklevel.gameObject.SetActive(true);

            //m_sprite_stateSpr.spriteName = "lock";
        }
        else
        {
            //m_label_state_unlocklevel.gameObject.SetActive(false);

            SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(changeSkill, 1);
            if (db != null)
            {
                //DataManager.Manager<UIManager>().SetSpriteDynamicIcon(m_sprite_stateSpr, db.iconPath, false, true);

                UIManager.GetTextureAsyn(UIManager.GetIconName(db.iconPath,true), ref iuiIconAtlas, () =>
                {
                    if (m__stateSpr != null)
                    {
                        m__stateSpr.mainTexture = null;
                    }
                }, m__stateSpr, true);

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
        //UIToggle toggle = caster.GetComponent<UIToggle>();
        //bool bAutoAttack = toggle.value;
        //GameCmd.stSetAutoFlagSkillUserCmd_CS cmd = new GameCmd.stSetAutoFlagSkillUserCmd_CS();
        //cmd.flag = (uint)(bAutoAttack ? 1 : 0);
        //NetService.Instance.Send(cmd);

    }
    void onClick_Arrow_left_Btn(GameObject caster)
    {
        skilldataManager.ShowState = SkillSettingState.StateOne;
        SetAllSkillSettingItem();
        ResetLeftItem();
        ResetChange();
        DataManager.Manager<ArenaSetSkillManager>().ReqSkillStatus((uint)skilldataManager.ShowState);
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
        DataManager.Manager<ArenaSetSkillManager>().ReqSkillStatus((uint)skilldataManager.ShowState);
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
        ArenaRightSkillItem rightItem = go.GetComponent<ArenaRightSkillItem>();
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
        ArenaLeftSkillItem item = go.GetComponent<ArenaLeftSkillItem>();
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

        foreach (var dic in m_dicLeftItem)
        {
            ArenaLeftSkillItem item = dic.Value;
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
        foreach (var dic in m_dicRightItem)
        {
            ArenaRightSkillItem item = dic.Value;
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
    /*
    void onClick_Skill_shengji_Btn(GameObject obj)
    {
        UIToggle tog = m_btn_skill_shengji.GetComponent<UIToggle>();
        if (tog != null)
        {
            tog.value = true;
        }
        skilldataManager.IsSettingPanel = false;
        m_trans_skill_shengji_area.gameObject.SetActive(true);
        m_widget_skill_setting_area.gameObject.SetActive(false);
        SetAllSkillSettingItem();
        InitLearnControls();
        ResetChange();
        ShowShengjiItemState();
    } 
     */

    void ShowShengjiItemState()
    {
        List<uint> keyList = m_dicLeftItem.Keys.ToList<uint>();
        for (int i = 0; i < keyList.Count; i++)
        {
            uint keyID = keyList[i];
            ArenaLeftSkillItem item = m_dicLeftItem[keyID];
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
        //GameCmd.stClearUsePosSkillUserCmd_CS cmd = new GameCmd.stClearUsePosSkillUserCmd_CS();
        //cmd.state = (uint)DataManager.Manager<LearnSkillDataManager>().ShowState;
        //NetService.Instance.Send(cmd);

        uint state = (uint)DataManager.Manager<ArenaSetSkillManager>().ShowState;
        DataManager.Manager<ArenaSetSkillManager>().ReqClearArenaSetSkill(state);
    }
    void onClick_Skill_setting_Btn(GameObject obj)
    {
        skilldataManager.IsSettingPanel = true;

        //       m_trans_skill_shengji_area.gameObject.SetActive(false);
        m_widget_skill_setting_area.gameObject.SetActive(true);
        SetAllSkillSettingItem();
        ResetChange();
        //UIToggle tog = m_btn_btnStatus1.GetComponent<UIToggle>();
        //if (tog != null)
        //{
        //    tog.value = true;
        //}
    }

    //void InitSetting() 
    //{
    //    skilldataManager.IsSettingPanel = true;
    //    m_widget_skill_setting_area.gameObject.SetActive(true);
    //    SetAllSkillSettingItem();
    //    ResetChange();
    //}


    /*
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
     */

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
                ArenaRightSkillItem item = trans.GetComponent<ArenaRightSkillItem>();
                if (item == null)
                {
                    item = trans.gameObject.AddComponent<ArenaRightSkillItem>();
                }
                if (i == 0)
                {
                    uint common = DataManager.Manager<LearnSkillDataManager>().GetCommonSkillIDByJob();
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
    /*
    void ShowAutoAttack()
    {
        
        bool autoAttack = skilldataManager.GetAutoAttack();
        UIToggle toggle = m_btn_auto_attack.GetComponent<UIToggle>();
        if (toggle != null)
        {
            toggle.value = autoAttack;
        }
    }*/
    string GetStatus(uint status)
    {
        if (status == 1)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_xushi);
        }
        else if (status == 11)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_panshi);
        }
        else if (status == 3)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_duxing);
        }
        else if (status == 13)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_qugong);
        }
        else if (status == 2)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_zhiliao);
        }
        else if (status == 12)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_huanhua);
        }
        else if (status == 4)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_zhaohuan);
        }
        else if (status == 14)
        {
            return CommonData.GetLocalString(LocalTextType.Skill_Commond_zuzhou);
        }
        else
        {
            return CommonData.GetLocalString("通用");
        }
    }

    //竞技场设置技能
    //public void ShowArenaSkillPanel(bool isArena)
    //{
    //    if (isArena)
    //    {
    //        m_btn_skill_shengji.gameObject.SetActive(false);
    //        m_btn_skill_setting.gameObject.SetActive(true);

    //        m_btn_auto_attack.gameObject.SetActive(false);
    //        m_label_zhu_Label.gameObject.SetActive(true);
    //        onClick_Skill_setting_Btn(m_btn_skill_setting.gameObject);
    //    }
    //    else 
    //    {
    //        m_btn_skill_shengji.gameObject.SetActive(true);
    //        m_btn_skill_setting.gameObject.SetActive(true);

    //        m_btn_auto_attack.gameObject.SetActive(true);
    //        m_label_zhu_Label.gameObject.SetActive(false);
    //        onClick_Skill_shengji_Btn(m_btn_skill_shengji.gameObject);
    //    }
    //}


}

