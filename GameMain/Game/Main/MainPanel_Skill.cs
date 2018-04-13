/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Main
 * 创建人：  wenjunhua.zqgame
 * 文件名：  MainPanel_Skill
 * 版本号：  V1.0.0.0
 * 创建时间：7/10/2017 2:11:11 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;
using Engine.Utility;
using DG.Tweening;
using Engine;
using table;
using Controller;
using GameCmd;

partial class MainPanel
{
    #region property
    ISkillPart SkillPart
    {
        get
        {
            IPlayer player = ClientGlobal.Instance().MainPlayer;
            return Protocol.SkillHelper.GetSkillPart(player.GetID(), EntityType.EntityType_Player);
        }
    }
    Dictionary<uint, table.SkillDatabase> m_dictskill = null;

    public uint m_uSkillID = 0;
    public bool bReadingSlider = false;//是否正在读条
    int m_state = 1;//形態1 2
    uint m_uCommonAttackSkillID = 0;
    uint CommonAttackID
    {
        get
        {
            if (m_uCommonAttackSkillID == 0)
            {
                m_uCommonAttackSkillID = DataManager.Manager<LearnSkillDataManager>().GetCommonSkillIDByJob();
            }
            return m_uCommonAttackSkillID;
        }
    }
    List<SkillUseBtn> m_lstSkillBtns = new List<SkillUseBtn>();
    bool loaded = false;

    float m_fPressTime = 0;
    bool bPress = false;
    //普攻按钮
    UIButton m_btnCommonAttack = null;


    //skill Short Cut
    const int m_allItemCount = 4;  //总共道具栏数量
    int m_fixItemCount = 1;
    int m_movableItemCount = 3;

    List<GameObject> m_lstItemGo = new List<GameObject>();

    List<ShortCuts> itemList = null;


    #endregion

    #region InitSkill

    public void DoShowSkill()
    {

    }

    public void DoHideSkill()
    {

    }

    private void InitSkill()
    {
        if (loaded)
        {
            return;
        }
        loaded = true;


        SkillUseBtn skillusebtn = null;
        Transform skill = transform.Find("SkillPanel");
        if (skill != null)
        {
            Transform container = skill.Find("offset/SkillBtnContainer");
            UIButton[] btns = container.GetComponentsInChildren<UIButton>();
            foreach (var item in btns)
            {
                skillusebtn = item.gameObject.AddComponent<SkillUseBtn>();
                skillusebtn.parent = this;
                int index = int.Parse(item.name.Substring(item.name.IndexOf('_') + 1));
                if (index == 0)
                {
                    // UIEventListener.Get(item.gameObject).onPress = OnLongPress;
                    UIEventListener.Get(item.gameObject).onClick = OnCommonAttack;
                    m_btnCommonAttack = item;
                    InitLongPress(item.transform);
                }
                skillusebtn.Init(index);
                m_lstSkillBtns.Add(skillusebtn);
                if (index < 1 || index > 8)
                {//1-8 会旋转
                    item.transform.parent = m_widget_SkillBtns.transform.parent;
                }

            }
        }



        //InitShortcutGridGoCache();//快捷使用道具
        UpdateShortcutUI();

        RegisterGlobalEvent(true);
    }

    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, SkillEvent);
            EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLCD_BEGIN, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_RIDE, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_UNRIDE, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.SKILLSYSTEM_REUSESKILLLONGATTACK, SkillEvent);
            EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_RESTORE, SkillEvent);
            EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGE, SkillEvent);
            EventEngine.Instance().AddEventListener((int)Client.GameEventID.HOTKEYPRESSDOWN, SkillEvent);
            EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLSYSTEM_SKILLSTATECHANE, SkillEvent);
            EventEngine.Instance().AddEventListener((int)GameEventID.SKILLSYSTEM_CDEND, SkillEvent);


            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnUIEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTNEWFUNCCOMPLETE, OnUIEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_SHORTCUTITEMUI, OnUIEvent);

        }
        else
        {
            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKILLCD_BEGIN, SkillEvent);

            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_RIDE, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_UNRIDE, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLSYSTEM_REUSESKILLLONGATTACK, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_RESTORE, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGE, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.HOTKEYPRESSDOWN, SkillEvent);
            EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKILLSYSTEM_SKILLSTATECHANE, SkillEvent);//
            EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLSYSTEM_CDEND, SkillEvent);


            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnUIEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTNEWFUNCCOMPLETE, OnUIEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_SHORTCUTITEMUI, OnUIEvent);


        }
    }
    #endregion

    #region Op
    void InitLongPress(Transform item)
    {
        LongPress lp = item.gameObject.GetComponent<LongPress>();
        if (lp == null)
        {
            lp = item.gameObject.AddComponent<LongPress>();
        }
        lp.InitLongPress(() =>
        {
            OnCommonAttack(item.gameObject);

        }, () =>
        {


        }, doubleSkillTime);
    }

    void OnCommonAttack(GameObject go)
    {
        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }
        if (cs.GetCombatRobot().Status == CombatRobotStatus.RUNNING)
        {//挂机不插入普攻
            return;
        }
        if (m_bSkillLongPress)
        {//已经是连击状态 不再响应按钮事件

            return;

        }
        ReqUseSkill(CommonAttackID);
        OnClickSkillBtn(go);
      
    }
    //播放点击技能特效
    public void OnClickSkillBtn(GameObject go, uint resourceID = 50009)
    {
        OnPlayBtnTeXiao(go, resourceID);
    }
    void OnPlayBtnTeXiao(GameObject go, uint resourceID = 50009)
    {
        Transform root = go.transform.Find("Particle");
        if (root != null)
        {
            UIParticleWidget p = root.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = root.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 100;
            }
            if (p != null)
            {
                p.SetDimensions(1, 1);
                p.ReleaseParticle();
                p.AddRoundParticle(resourceID);

            }
        }
    }

    public uint doubleSkillTime = 500;
    public float m_btnScaletime = 0.03f;
    void OnLongPress(GameObject go, bool state)
    {
        //Log.Error("OnLongPress message state is "+state);
        if (state)
        {
            OnCommonAttack(go);
        }
        
    }
    /// <summary>
    /// 是否已经发送长按事件
    /// </summary>
    bool m_bSendLongPress = false;
 

    public void SetPlayerSkillIcon()
    {
        for (int i = 0; i < m_lstSkillBtns.Count; i++)
        {
            SkillUseBtn use = m_lstSkillBtns[i];
            if (use != null)
            {//过滤宠物技能
                if (use.SkillIndex != 9)
                {
                    m_lstSkillBtns[i].Refresh();
                }
            }
        }
    }
    public void SetSkillIcon()
    {
        if (MainPlayerHelper.GetMainPlayer() == null)
        {
            return;
        }
        bool ischange = (bool)MainPlayerHelper.GetMainPlayer().SendMessage(EntityMessage.EntityCommand_IsChange, null);
        for (int i = 0; i < m_lstSkillBtns.Count; i++)
        {
            m_lstSkillBtns[i].Refresh();
            if (ischange)
            {
                m_lstSkillBtns[i].SetSkillLockStatus(true);
            }
        }

    }

    private void ShowSkill()
    {
        RegisterGlobalEvent(true);

        SetSkillIcon();
        LearnSkillDataManager skillData = DataManager.Manager<LearnSkillDataManager>();
        if (skillData.CurState == SkillSettingState.StateTwo)
        {
            m_widget_SkillBtns.transform.DORotate(new Vector3(0, 0, 125), rotTime);
        }

        InitShortcutUIAndData();

        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
            return;
        }

        //Client.ICombatRobot robot = cs.GetCombatRobot();
        //if (robot != null)
        //{
        //    m_btn_BtnAI.GetComponent<UIToggle>().value = robot.Status != CombatRobotStatus.STOP;
        //}
    }
    void OnUIEvent(int type, object data)
    {
        switch (type)
        {
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                {
                    OnUpdateItemDataUI(data);//快捷使用道具
                }
                break;
            case (int)Client.GameEventID.UIEVENTNEWFUNCCOMPLETE:
                {
                    if (data is GuideDefine.FuncOpenShowData)
                    {
                        GuideDefine.FuncOpenShowData guideData = (GuideDefine.FuncOpenShowData)data;
                        if (guideData.FOT == GuideDefine.FuncOpenType.Skill)
                        {//SetSkillIcon
                            stSetSkillPos pos = (stSetSkillPos)guideData.Data;
                            for (int i = 0; i < m_lstSkillBtns.Count; i++)
                            {
                                if (m_lstSkillBtns[i].SkillIndex == pos.pos)
                                {
                                    m_lstSkillBtns[i].Refresh();
                                }
                            }
                        }
                    }
                }
                break;
            case (int)Client.GameEventID.UIEVENT_SHORTCUTITEMUI:
                {
                    InitShortcutUIAndData();
                }
                break;
        }
    }

    void SkillEvent(int eventID, object param)
    {
        if (MainPlayerHelper.GetMainPlayer() == null)
        {
            return;
        }
        long userUID = ClientGlobal.Instance().MainPlayer.GetUID();
        if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE)
        {
            stEntityBeginMove move = (stEntityBeginMove)param;
            if (move.uid != userUID)
            {
                return;
            }

        }
        else if (eventID == (int)Client.GameEventID.SKILLCD_BEGIN)
        {
            Client.stSkillCDChange st = (stSkillCDChange)param;
            uint skillid = (uint)st.skillid;
            DataManager.Manager<SkillCDManager>().AddSkillCD(st.skillid, st.cd);
            for (int i = 0; i < m_lstSkillBtns.Count; i++)
            {
                if (m_lstSkillBtns[i].SkillId == skillid)
                {
                    //Engine.Utility.Log.LogGroup("ZCX", "SKILLPANEL SHOW CD：技能{0} {1}", skillid, m_lstSkillBtns[i].name);
                    m_lstSkillBtns[i].RunCD();
                }
            }
            ExecutePublicCD(skillid);

        }
        else if (eventID == (int)GameEventID.SKILLGUIDE_PROGRESSBACK)
        {

        }
        else if (eventID == (int)GameEventID.SKILLSYSTEM_CDEND)
        {
            if (param != null && param is uint)
            {
                uint skillid = (uint)param;
                for (int i = 0; i < m_lstSkillBtns.Count; i++)
                {

                    if (m_lstSkillBtns[i].SkillId == skillid && m_lstSkillBtns[i].SkillId != CommonAttackID)
                    {
                        m_lstSkillBtns[i].AddEffectWhenCDEnd();
                    }
                }
            }
        }
        else if (eventID == (int)GameEventID.SKILLSYSTEM_REUSESKILLLONGATTACK)
        {
            ReqUseSkill(CommonAttackID);
        }
        else if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTITYDEAD)
        {
            stEntityDead ed = (stEntityDead)param;
            if (ed.uid == userUID)
            {

                bReadingSlider = false;
                CancelPressAttack();
            }
        }
        else if (eventID == (int)GameEventID.ENTITYSYSTEM_RIDE || eventID == (int)GameEventID.ENTITYSYSTEM_UNRIDE)
        {
            //             LearnSkillDataManager data = DataManager.Manager<LearnSkillDataManager>();
            //             if (data != null && data.CurState != SkillSettingState.StateOne)
            //             {
            //                 for (int i = 0; i < m_lstSkillBtns.Count; i++)
            //                 {
            //                     if (m_lstSkillBtns[i].SkillIndex == 10)//第十个是状态转换
            //                     {
            //                         ChangeSkill(m_lstSkillBtns[i].gameObject);
            //                     }
            //                 }
            //             }

            SetSkillIcon();
        }
        else if (eventID == (int)GameEventID.ENTITYSYSTEM_CHANGE || eventID == (int)GameEventID.ENTITYSYSTEM_RESTORE)
        {
            stPlayerChange pc = (stPlayerChange)param;
            if (pc.uid == MainPlayerHelper.GetPlayerID())
            {
                for (int i = 0; i < m_lstSkillBtns.Count; i++)
                {
                    m_lstSkillBtns[i].SetSkillLockStatus(pc.status == 1);
                }
            }
        }
        else if (eventID == (int)GameEventID.SKILLSYSTEM_SKILLSTATECHANE)
        {

            m_uCommonAttackSkillID = DataManager.Manager<LearnSkillDataManager>().GetCommonSkillIDByJob();
        }
        else if (eventID == (int)GameEventID.HOTKEYPRESSDOWN)
        {
            stHotKeyDown hd = (stHotKeyDown)param;
            if (hd.type == 1)
            {
                uint pos = hd.pos;
                for (int i = 0; i < m_lstSkillBtns.Count; i++)
                {
                    SkillUseBtn use = m_lstSkillBtns[i];
                    if (use != null)
                    {
                        if (use.SkillIndex == pos)
                        {
                            UIButton btn = use.GetComponent<UIButton>();
                            if (btn != null)
                            {
                                btn.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
                            }
                        }
                    }
                }
            }
        }
    }
    //注销长按攻击事件
    void CancelPressAttack()
    {

        TimerAxis.Instance().KillTimer(m_uSkillLongPressTimerID, this);
        m_btnCommonAttack.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
        stSkillLongPress longPress = new stSkillLongPress();
        // longPress.userID = MainPlayerHelper.GetPlayerUID();
        longPress.bLongPress = false;
        EventEngine.Instance().DispatchEvent((int)GameEventID.SKLL_LONGPRESS, longPress);
    }


    uint nPrePareSkillID;

    public void ReqUseSkill(uint uSkillID)
    {

        if (bReadingSlider)
        {//
            Log.Error("读条时不能使用技能");
            return;
        }

        DataManager.Manager<SkillModuleData>().OnUseSkill(uSkillID);


    }
    void UseSkill(uint uSkillID)
    {
        IControllerSystem ctrl = ClientGlobal.Instance().GetControllerSystem();
        if (ctrl != null)
        {
            MessageCode code = MessageCode.MessageCode_ButtonX;

            ctrl.OnMessage(code, uSkillID);
        }
    }

    void onClick_TAB_Btn(GameObject caster)
    {
        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
            return;
        }
        cs.GetSwitchTargetCtrl().Switch();
    }
    public float rotTime = 0.3f;
    public void ChangeSkill(GameObject caster)
    {

        int tempState = 0;
        LearnSkillDataManager data = DataManager.Manager<LearnSkillDataManager>();
        tempState = (int)data.CurState;

        int job = MainPlayerHelper.GetMainPlayer().GetProp((int)PlayerProp.Job);
        job = tempState == 1 ? job : job + 10;
        uint changeSkill = GameTableManager.Instance.GetGlobalConfig<uint>("ChangeStateSkillID", job.ToString());
        ReqUseSkill((uint)changeSkill);

    }
    void ExecutePublicCD(uint skillID)
    {
        SkillDatabase db = GameTableManager.Instance.GetTableItem<SkillDatabase>(skillID, 1);
        if (db != null)
        {
            uint group = db.dwCommonCDGroup;
            if (group == 0)
            {
                return;
            }
            for (int i = 0; i < m_lstSkillBtns.Count; i++)
            //foreach (var usebtn in m_lstSkillBtns)
            {
                var usebtn = m_lstSkillBtns[i];
                if (usebtn.SkillId != 0)
                {
                    SkillDatabase gdb = GameTableManager.Instance.GetTableItem<SkillDatabase>(usebtn.SkillId, 1);
                    if (gdb != null && gdb.dwCommonCDGroup == group)
                    {
                        usebtn.ShowPublicCd();
                    }
                }
            }
        }
    }


    uint m_uReadEffectID = 0;
    uint CreateEffect(uint nEffectViewID)
    {
        IEntity target = MainPlayerHelper.GetMainPlayer();
        FxResDataBase edb = GameTableManager.Instance.GetTableItem<FxResDataBase>(nEffectViewID);
        if (edb != null && target != null)
        {
            if (nEffectViewID == 1002 && EntitySystem.EntityHelper.IsMainPlayer(target))
            {
                //  Engine.Utility.Log.Error("nEffectViewID:{0}", nEffectViewID);
            }

            AddLinkEffect node = new AddLinkEffect();
            node.nFollowType = (int)edb.flowType;
            node.rotate = new Vector3(edb.rotate[0], edb.rotate[1], edb.rotate[2]);
            node.vOffset = new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]);

            // 使用资源配置表
            ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<ResourceDataBase>(edb.resPath);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("EffectViewFactory:找不到特效资源路径配置{0}", edb.resPath);
                return 0;
            }
            node.strEffectName = resDB.strPath;
            node.strLinkName = edb.attachNode;
            if (node.strEffectName.Length != 0)
            {
                int id = (int)target.SendMessage(EntityMessage.EntityCommand_AddLinkEffect, node);

                return (uint)id;
            }
        }

        return 0;
    }
    private void SendPlayAniMessage(IEntity defender, string aniName, int loop = -1)
    {
        CreatureState state = defender.GetCurState();
        if (state != CreatureState.Dead)
        {

            if (state != CreatureState.Move)
            {
                //defender.SendMessage( EntityMessage.EntityCommand_StopMove , defender.GetPos() );
                //移动不播放受击动作
                PlayAni anim_param = new PlayAni();
                anim_param.strAcionName = aniName;
                anim_param.fSpeed = 1;
                anim_param.nStartFrame = 0;
                anim_param.nLoop = loop;
                anim_param.fBlendTime = 0.2f;
                anim_param.aniCallback = null;
                anim_param.callbackParam = defender;
                defender.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
            }
            else
            {
                //  Engine.Utility.Log.Trace("移动不播放受击动作");
            }

        }
        else
        {
            // Engine.Utility.Log.Trace(defender.GetName() + "死亡状态不能播放受击动作");
        }
    }
    public void RemoveEffect(uint uEffectID)
    {
        IEntity target = MainPlayerHelper.GetMainPlayer();
        if (target != null)
        {
            target.SendMessage(EntityMessage.EntityCommand_RemoveLinkEffect, (int)uEffectID);
        }
    }

    public void ReleaseSkillPanel()
    {
        foreach(var btn in m_lstSkillBtns)
        {
            btn.ReleaseSkillBtn();
        }
    }
    #endregion

    #region Skill Short Cut

    /// <summary>
    /// 初始化Item  GameObject
    /// </summary>
    void InitShortcutGridGoCache()
    {
        m_lstItemGo.Clear();

        for (int i = 0; i < m_trans_ShortcutItemGridCache.childCount; i++)
        {
            m_lstItemGo.Add(m_trans_ShortcutItemGridCache.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 快捷面板UI和grid数据
    /// </summary>
    void InitShortcutUIAndData()
    {
        UpdateShortcutUI();
        InitShortcutGrid();
    }

    void UpdateShortcutUI()
    {
        m_fixItemCount = DataManager.Manager<SettingManager>().FixedShortcutItemCount;
        if (m_fixItemCount < 1)
        {
            return;
        }

        if (m_fixItemCount == m_allItemCount)
        {
            m_btn_BtnArrow.gameObject.SetActive(false);
        }
        else
        {
            m_btn_BtnArrow.gameObject.SetActive(true);
        }

        //可移动的
        m_movableItemCount = m_allItemCount - m_fixItemCount;

        //全部移动到缓存
        //m_grid_FixedItemGridsRoot.transform.DetachChildren();
        //m_grid_MovableItemGridsRoot.transform.DetachChildren();
        for (int i = 0; i < m_lstItemGo.Count; i++)
        {
            if (i < m_lstItemGo.Count)
            {
                m_lstItemGo[i].transform.parent = m_trans_ShortcutItemGridCache;
                m_lstItemGo[i].SetActive(false);
            }
        }

        //设置固定的
        for (int i = 0; i < m_fixItemCount; i++)
        {
            if (i < m_lstItemGo.Count)
            {
                m_lstItemGo[i].transform.parent = m_grid_FixedItemGridsRoot.transform;
                m_lstItemGo[i].transform.localScale = Vector3.one;
                m_lstItemGo[i].transform.localPosition = Vector3.zero;
                m_lstItemGo[i].transform.localRotation = Quaternion.identity;
                m_lstItemGo[i].SetActive(true);
            }
            else
            {
                //Engine.Utility.Log.Error("1--> fixItemCount = {0} i = {1} m_lstItemGo.Count= {2}", m_fixItemCount,i, m_lstItemGo.Count);
            }
        }
        m_grid_FixedItemGridsRoot.repositionNow = true;

        //设置可移动的
        for (int i = m_fixItemCount; i < m_allItemCount; i++)
        {
            if (i < m_lstItemGo.Count)
            {
                m_lstItemGo[i].transform.parent = m_grid_MovableItemGridsRoot.transform;
                m_lstItemGo[i].transform.localScale = Vector3.one;
                m_lstItemGo[i].transform.localPosition = Vector3.zero;
                m_lstItemGo[i].transform.localRotation = Quaternion.identity;
                m_lstItemGo[i].SetActive(true);
            }
            else
            {
                //Engine.Utility.Log.Error("2--> m_allItemCount = {0} i = {1} m_lstItemGo.Count= {2}", m_allItemCount, i, m_lstItemGo.Count);
            }
        }
        m_grid_MovableItemGridsRoot.repositionNow = true;

        //设置可移动面板位置
        m_trans_ShortcutMovablePanel.localPosition = new Vector3(-237 - 67 * (m_fixItemCount - 1), 0, 0);

        //设置点击区域
        m_widget_shortcutRect.SetDimensions(67 * m_movableItemCount, 84);
    }

    /// <summary>
    /// 初始化grid数据
    /// </summary>
    void InitShortcutGrid()
    {
        m_fixItemCount = DataManager.Manager<SettingManager>().FixedShortcutItemCount;

        itemList = DataManager.Manager<SettingManager>().GetShortcutSetItemList();

        for (int i = 0; i < m_grid_FixedItemGridsRoot.transform.childCount; i++)
        {
            int index = m_grid_FixedItemGridsRoot.transform.childCount - i - 1;
            if (index >= 0 && index < m_grid_FixedItemGridsRoot.transform.childCount)
            {
                GameObject gridGo = m_grid_FixedItemGridsRoot.transform.GetChild(index).gameObject;
                UIShortcutItemGrid grid = gridGo.GetComponent<UIShortcutItemGrid>();
                if (grid == null)
                {
                    grid = gridGo.AddComponent<UIShortcutItemGrid>();
                }

                if (i < itemList.Count)
                {
                    grid.SetGridData(itemList[i]);
                    grid.RegisterUIEventDelegate(OnGridUIEvent);
                }
            }


        }

        for (int i = 0; i < m_grid_MovableItemGridsRoot.transform.childCount; i++)
        {
            int index = m_grid_MovableItemGridsRoot.transform.childCount - i - 1;
            if (index >= 0 && index < m_grid_MovableItemGridsRoot.transform.childCount)
            {
                GameObject gridGo = m_grid_MovableItemGridsRoot.transform.GetChild(index).gameObject;
                UIShortcutItemGrid grid = gridGo.GetComponent<UIShortcutItemGrid>();
                if (grid == null)
                {
                    grid = gridGo.AddComponent<UIShortcutItemGrid>();
                }

                if (m_fixItemCount + i < itemList.Count)
                {
                    grid.SetGridData(itemList[m_fixItemCount + i]);
                    grid.RegisterUIEventDelegate(OnGridUIEvent);
                }
            }


        }
    }

    /// <summary>
    /// grid 点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEvent(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIShortcutItemGrid grid = data as UIShortcutItemGrid;
            if (grid == null)
            {
                return;
            }

            if (grid.item.itemid == 0)
            {
                GoToSettingPanel();// 跳转设置面板
            }
            else
            {
                //使用道具
                ShortCuts item = grid.item;

                List<BaseItem> itemList = DataManager.Manager<ItemManager>().GetItemByBaseId(item.itemid);

                if (itemList != null && itemList.Count > 0)
                {
                    DataManager.Manager<ItemManager>().Use(itemList[0].QWThisID);
                }
                else
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: item.itemid);
                }
            }

            if (grid.item.id >= m_fixItemCount)//点活动的item  面板要收
            {
                ClickShortcut();

                if (m_fixItemCount == m_allItemCount)
                {
                    return;
                }
                UIPlayTween playTween = m_btn_BtnArrow.GetComponent<UIPlayTween>();
                if (playTween != null)
                {
                    playTween.Play(true);
                }
            }
        }
    }

    /// <summary>
    /// 跳转设置面板
    /// </summary>
    void GoToSettingPanel()
    {
       object d = SettingPanel.RightTagType.UseItem;
       DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.SettingPanel, data:d);
    }

    /// <summary>
    /// 伸缩控制
    /// </summary>
    void ClickShortcut()
    {
        if (m_fixItemCount == m_allItemCount)
        {
            return;
        }

        bool show = m_trans_shortcutContent.transform.localPosition.x == 240;
        Vector3 pos;
        if (show)
        {
            pos = new Vector3(40 + 67 * (3 - m_movableItemCount), m_trans_shortcutContent.transform.localPosition.y, 0);
        }
        else
        {
            pos = new Vector3(240, m_trans_shortcutContent.transform.localPosition.y, 0);
        }
        TweenPosition.Begin(m_trans_shortcutContent.gameObject, 0.2f, pos);
    }

    /// <summary>
    /// 点击的点是否在道具区域内，如果不再，道具打开了就关闭
    /// </summary>
    /// <param name="screenPos"></param>
    void PointInRectEvent(Vector3 screenPos)
    {
        if (this.gameObject.activeSelf == false)
        {
            return;
        }

        if (m_fixItemCount == m_allItemCount)
        {
            return;
        }

        Vector3 worldPos = UICamera.currentCamera.ScreenToWorldPoint(screenPos);  //屏幕坐标到世界坐标
        Vector3 point = m_widget_shortcutRect.transform.worldToLocalMatrix.MultiplyPoint(worldPos);//transform的相对坐标
        Bounds bound = m_widget_shortcutRect.CalculateBounds();
        if (bound.Contains(point) == false)
        {
            bool show = m_trans_shortcutContent.transform.localPosition.x == 40 + 67 * (3 - m_movableItemCount);
            if (show)
            {
                Vector3 setPos = new Vector3(240, m_trans_shortcutContent.transform.localPosition.y, 0);
                TweenPosition.Begin(m_trans_shortcutContent.gameObject, 0.2f, setPos);
                UIPlayTween playTween = m_btn_BtnArrow.GetComponent<UIPlayTween>();
                if (playTween != null)
                {
                    playTween.Play(true);
                }
            }
        }
    }

    /// <summary>
    /// 更新物品UI
    /// </summary>
    /// <param name="data">物品数据</param>
    private void OnUpdateItemDataUI(object data)
    {
        if (null == data || !(data is ItemDefine.UpdateItemPassData))
            return;
        ItemDefine.UpdateItemPassData passData = data as ItemDefine.UpdateItemPassData;
        uint qwThisId = passData.QWThisId;
        if (qwThisId == 0)
        {
            Engine.Utility.Log.Warning("->UpdateItemDataUI qwThisId 0！");
            return;
        }
        if (m_lstItemGo.Count == 0)
        {
            return;
        }

        if (itemList == null)
        {
            Engine.Utility.Log.Error("--->>> itemList 数据为null ！");
            return;
        }

        ShortCuts item = itemList.Find((d) => { return d.itemid == passData.BaseId; });
        if (item == null)
        {
            return;
        }

        for (int i = 0; i < m_lstItemGo.Count; i++)
        {
            UIShortcutItemGrid grid = m_lstItemGo[i].GetComponent<UIShortcutItemGrid>();
            if (grid != null)
            {
                if (grid.item.itemid == passData.BaseId)
                {
                    grid.SetGridData(item);
                }
            }
        }
    }

    /// <summary>
    /// 点击箭头
    /// </summary>
    /// <param name="caster"></param>
    void onClick_BtnArrow_Btn(GameObject caster)
    {
        ClickShortcut();
    }

    #endregion
}