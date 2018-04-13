using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using UnityEngine;
using Engine;
using Client;
using System.Collections;
using GameCmd;
using table;
using DG.Tweening;

//红点模块枚举
enum WarningEnum
{
    WELFARE = 1,
    SEVENDAY = 2,
    TITLE_NEWTITLE = 3,
    LearnSkill = 4,
    Question = 5,
    Clan = 6,
    Achievement = 7,
    Equip = 8,
    Mail = 9,
    Noble = 10,
    Daily = 11,
    GodWeapen = 12,
    OpenServer = 13,
    FirstRechargeReward = 14,
    Activity = 15,
    Forging = 16,
    Accumulative = 17,
    Ride = 18,
    HttpDown = 19,
}
enum WarningDirection
{
    None = 0,
    Left = 1,
    Right = 2,
}
public partial class MainPanel : UIPanelBase, Engine.Utility.ITimer
{

    CMResAsynSeedData<CMTexture> m_currencyAsynSeed = null;
    readonly int totalBuffIconCount = 5;

    IEffect m_effctSelect = null;//箭头
    Camera uicamera;
    Camera maincamera;

    PetQuickInfo m_petInfo;
    //优化仇恨列表，用以创建格子
    UIGridCreatorBase enemyListCreator = null;

    //多个系统红点提示汇总  key ==>区分左侧和右侧
    Dictionary<WarningDirection, List<WarningEnum>> m_lstWarnningSystem = new Dictionary<WarningDirection, List<WarningEnum>>();


    bool m_initMainPanel = false;

    #region override

    protected override void OnLoading()
    {
        base.OnLoading();
        AdjustUI();
        uicamera = Util.UICameraObj.GetComponent<Camera>();
        maincamera = Util.MainCameraObj.GetComponent<Camera>();

        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_TARGETCHANGE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLSYSTEM_SHOWDAMAGE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_MPUPDATE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLSYSYTEM_TAB, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_FIGHTPOWER_REFRESH, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILL_SHOWPETSKILL, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TITLE_NEWTITLE, OnEvent);//
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_STOP, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_START, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_RELIVE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_NEWNAME, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.EJOYSTICKSTABLE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.REFRESHTRANSMITPUSHMSGSTATUS, OnEvent);


        Engine.Utility.TimerAxis.Instance().SetTimer(MAIN_TIMER_ID, 60000, this, Engine.Utility.TimerAxis.INFINITY_CALL, "主界面显示坐标");
        InitPkUI();
        InitFuncBtns();
        InitBtns();
        InitTargetUI();
        InitShortcutGridGoCache();//快捷使用道具
        LoadingSkillPanel();
        InitFBWidget();
        InitAnswerUI();
        FlyFontDataManager.Instance.InitFlyFont();

        UIEventListener.Get(m_trans_MainRoleBUffContainer.gameObject).onClick = ShowBuffPanel;
        UIEventListener.Get(m_trans_TargetBUffContainer.gameObject).onClick = ShowTargetPanel;
        UIEventListener.Get(m_spriteEx_btnAutoFight.gameObject).onClick = onClick_BtnAI_Btn;
        if (m_spriteEx_btnAutoFight != null)
        {
            m_spriteEx_btnAutoFight.ChangeSprite(1);
        }
        InitJoystick();

        InitMiniMap();

        InitChat();

        InitTAB();

        if (m_transHateListRoot != null)
        {
            enemyListCreator = m_transHateListRoot.GetComponent<UIGridCreatorBase>();

        }
        RefreshTime();

        InitGvoice();

    }

    //4个充值特效
    void MainBtnParticleOnShow()
    {
        ControlMainBtnParticle(m_sprite_FirstRecharge_warning.parent.transform, true);
        ControlMainBtnParticle(m_sprite_activity_warning.parent.transform, true);
        ControlMainBtnParticle(m_sprite_Noble_warning.parent.transform, true);
        ControlMainBtnParticle(m_sprite_Accumulative_warning.parent.transform, true);
    }

    void ControlMainBtnParticle(Transform parent, bool bShow)
    {
        if (parent != null)
        {
            Transform effect = parent.Find("effect");
            if (effect)
            {
                UIParticleWidget wight = effect.GetComponent<UIParticleWidget>();

                if (wight == null)
                {
                    wight = effect.gameObject.AddComponent<UIParticleWidget>();
                    wight.depth = 20;
                }
                if (wight != null)
                {
                    wight.SetDimensions(1, 1);
                    wight.ReleaseParticle();
                    if (bShow)
                    {
                        wight.AddParticle(50040);
                    }
                    m_initMainPanel = bShow;
                }

            }

        }

    }
    void LoadingSkillPanel()
    {
        Transform skill = transform.Find("SkillPanel");
        if (skill != null)
        {
            skill.gameObject.SetActive(true);
        }
    }
    public override void ResetPanel()
    {
        TimerAxis.Instance().KillTimer(m_uSkillLongPressTimerID, this);
        base.ResetPanel();
    }


    protected override void OnDisable()
    {
        base.OnDisable();


        //技能
        //         if (m_btn_BtnArrow != null)
        //         {
        //             m_btnCommonAttack.SendMessage("OnPress", false, SendMessageOptions.DontRequireReceiver);
        //         }
    }

    void onClick_BtnAI_Btn(GameObject caster)
    {
        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
            return;
        }

        Client.ICombatRobot robot = cs.GetCombatRobot();
        if (robot.Status == Client.CombatRobotStatus.STOP)
        {
            ComBatCopyDataManager comBat = DataManager.Manager<ComBatCopyDataManager>();
            if (comBat.IsEnterCopy && comBat.EnterCopyID != 0)
            {
                robot.StartInCopy(comBat.EnterCopyID, comBat.LaskSkillWave, comBat.LastTransmitWave);
            }
            else
            {
                robot.Start();
            }
        }
        else
        {
            robot.Stop();
        }
    }
    private CMResAsynSeedData<CMTexture> m_faceCASD = null;
    protected override void OnShow(object data)
    {
        if (m_btnTween != null)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINRIGHTBTN_TOGGLE,
            !m_btnTween.transform.localEulerAngles.Equals(m_btnTween.to));
        }

        int level = MainPlayerHelper.GetPlayerLevel();
        m_label_Level.text = level.ToString();
        m_label_mainroleName.text = UserData.CurrentRole.name;
        if (null != m__Face)
        {
            UIManager.GetTextureAsyn(GetSpriteName(UserData.CurrentRole.type), ref m_faceCASD, () =>
                {
                    if (null != m__Face)
                    {
                        m__Face.mainTexture = null;
                    }
                }, m__Face, false);
        }
        Client.IEntity entity = ClientGlobal.Instance().GetEntitySystem().FindPlayer(UserData.CurrentRole.id);
        SetRoleExp(entity);
        ShowBuffIcon();

        SetRoleHpMpExpPkPower(ClientGlobal.Instance().MainPlayer);

        //目标
        InitTargetChangeUI();

        SetRideBtnState();

        ShowJoystick();

        ShowMiniMap();
        InitSkill();
        ShowSkill();

        //副本公用模块
        ShowFBWidgetUI();

        //副本引导
        ShowFBGuideUI();

        InitPetLineUP();

        //红点实现
        SetMallRedPoint();
        SetFirstRechargeRewardRedPoint();
        SetAchievementRedPoint();
        SetActivityRedPoint();
        SetForgingRedPoint();
        SetFriendRedPoint();
        SetAccumulativeRechRedPoint();//累充
        SetRideRedPoint();
        SetHttpDownPoint();

        // 下载领取过了.
        if(KDownloadInstance.Instance().GetTakeReward() == true)
        {
            Transform gmTrans = m_trans_AnchorTopRight.Find("XiaZai");
            if (gmTrans)
            {
                gmTrans.localPosition = new Vector3(1000, 1000, 0);
            }
        }


        if (Application.isEditor)
        {
            Transform gmTrans = m_trans_AnchorTopRight.Find("XiaZai");
            if (gmTrans)
            {
                gmTrans.localPosition = new Vector3(1000, 1000, 0);
            }
        }

        try
        {
            //if (GlobalConfig.Instance().smallPackage)
            if(KDownloadInstance.Instance().m_bSmallPackage)
            {
            }
            else
            {
                Transform gmTrans = m_trans_AnchorTopRight.Find("XiaZai");
                if (gmTrans)
                {
                    gmTrans.localPosition = new Vector3(1000, 1000, 0);
                }
            }

        }
        catch (Exception e)
        {
            // 以前的包回到这里
            Transform gmTrans = m_trans_AnchorTopRight.Find("XiaZai");
            if (gmTrans)
            {
                gmTrans.localPosition = new Vector3(1000, 1000, 0);
            }
        }


        //钓鱼按钮
        InitFishingUI();

        //状态tips 寻路中 ，自动战斗中，。。。
        PlayTipsUI();

        //充值按钮特效
        MainBtnParticleOnShow();

        //城战阵营站采集图标
        UpdateFBPickBtn(null);

        //限时答题
        InitAnswerUI();
      
    }
    Dictionary<uint, string> headIconDic = new Dictionary<uint, string>();
    string GetSpriteName(GameCmd.enumProfession type)
    {
        IPlayer player = MainPlayerHelper.GetMainPlayer();
        string headIcon = "";
        if (player != null)
        {
            uint job = (uint)type;
            int sex = player.GetProp((int)PlayerProp.Sex);
            if (headIconDic.ContainsKey((uint)type))
            {
                headIcon = headIconDic[job];
            }
            else
            {
                SelectRoleDataBase sdb = table.SelectRoleDataBase.Where(type, (GameCmd.enmCharSex)sex);
                if (sdb != null)
                {
                    headIcon = sdb.mainUIHeadIcon;
                    headIconDic.Add(job, headIcon);
                }
            }
        }
        return headIcon;
    }

    /// <summary>
    /// mono Update
    /// </summary>
    void Update()
    {
        MiniMapUpdate();

        UpdateSelectTarget();

        //副本倒计时
        UpdateFBTime();

        //跟新答题
        UpdateAnswerUI();

        //语音
        GvoiceChatUpdate();
    }

    protected override void OnHide()
    {
        if (m_effctSelect != null)
        {
            Engine.RareEngine.Instance().GetRenderSystem().RemoveEffect(m_effctSelect);
        }
        ResetLeftBtnsPos();

        //技能
        StopAllCoroutines();
        RegisterGlobalEvent(false);
        CancelPressAttack();
        // StopRecording();
        Release();
        HideMiniMap();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_faceCASD)
        {
            m_faceCASD.Release();
            m_faceCASD = null;
        }
        ReleaseSkillPanel();
    }

    #endregion

    private void AdjustUI()
    {
        UnityEngine.Vector2 offset = UIRootHelper.Instance.OffsetSize;
        if (null != m_slider_expSprite_bg)
        {
            if (null != m_slider_expSprite_bg.backgroundWidget)
            {
                m_slider_expSprite_bg.backgroundWidget.width = (int)UIRootHelper.Instance.TargetSize.x;
            }

            if (null != m_slider_expSprite_bg.foregroundWidget)
            {
                m_slider_expSprite_bg.foregroundWidget.width = (int)UIRootHelper.Instance.TargetSize.x;
            }
        }
    }


    void ShowBuffIcon()
    {
        ArrayList rolelist = DataManager.Manager<BuffDataManager>().MainRoleBuffList;
        RefreshBuffIcon(m_trans_MainRoleBUffContainer, rolelist);

        ArrayList list = DataManager.Manager<BuffDataManager>().TargetBuffList;
        RefreshBuffIcon(m_trans_TargetBUffContainer, list);
    }


    void SetSelectTargetEffectPos()
    {
        Client.IEntity entity = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().GetCurTarget();
        Vector3 pos = Vector3.zero;
        if (entity != null)
        {
            pos = maincamera.WorldToViewportPoint(entity.GetNode().GetWorldPosition());
            pos = uicamera.ViewportToWorldPoint(pos);
            m_effctSelect.GetNode().SetWorldPosition(pos);
        }
        else
        {
            m_effctSelect.GetNode().SetLocalPosition(new Vector3(0, 150, 0));
        }
        pos = m_effctSelect.GetNode().GetLocalPosition();
        pos.x = Mathf.FloorToInt(pos.x);
        pos.y = Mathf.FloorToInt(pos.y) + 25;
        pos.z = 0f;
        m_effctSelect.GetNode().SetLocalPosition(pos);
    }

    void SetRoleHpMpExpPkPower(Client.IEntity entity)
    {
        SetRoleHp(entity);
        SetRoleMp(entity);
        SetRoleExp(entity);
        SetRolePkModel(entity);
        if (m_label_lablePower != null)
        {
            m_label_lablePower.text = entity.GetProp((int)FightCreatureProp.Power).ToString();
        }
    }

    void SetRoleHp(Client.IEntity entity)
    {
        if (entity == null || m_slider_hpSpritebg == null) return;
        int hp = entity.GetProp((int)CreatureProp.Hp);
        float maxhp = (float)entity.GetProp((int)CreatureProp.MaxHp);
        m_slider_hpSpritebg.value = hp / maxhp;
        // m_sprite_hpSprite.fillAmount = 
    }

    void SetRoleMp(Client.IEntity entity)
    {
        if (entity == null || m_slider_mpSpritebg == null) return;
        int mp = entity.GetProp((int)CreatureProp.Mp);
        float maxmp = (float)entity.GetProp((int)CreatureProp.MaxMp);
        m_slider_mpSpritebg.value = mp / maxmp;
        //   m_sprite_mpSprite.fillAmount = 
    }

    void SetRoleExp(Client.IEntity entity)
    {
        if (entity == null || m_slider_expSprite_bg == null) return;
        //经验值exp显示
        table.UpgradeDataBase data = GameTableManager.Instance.GetTableItem<table.UpgradeDataBase>((uint)entity.GetProp((int)CreatureProp.Level));
        ulong maxExp = data.qwExp;
        m_label_Labelexp.text = string.Format("经验{0}%", Mathf.Floor((entity.GetProp((int)PlayerProp.Exp) + 0.0f) / (int)maxExp * 100));
        m_slider_expSprite_bg.value = (entity.GetProp((int)PlayerProp.Exp) + 0.0f) / (int)maxExp;
    }


    void InitEffct(IEffect effct)
    {
        if (effct == null)
        {
            return;
        }
        GameObject obj = effct.GetNode().GetTransForm().gameObject;
        obj.transform.parent = transform;
        obj.layer = gameObject.layer;
        obj.transform.SetChildLayer(gameObject.layer);
        obj.SetActive(true);

        obj.transform.localScale = new Vector3(75, 75, 0);
    }
    void OnEvent(int eventID, object param)
    {
        switch ((Client.GameEventID)eventID)
        {
            case GameEventID.ENTITYSYSTEM_TARGETCHANGE:
                {
                    OnTargetChange(param);
                }
                break;
            case GameEventID.SKILLSYSYTEM_TAB:
                {
                    if (m_effctSelect != null)
                    {
                        Engine.RareEngine.Instance().GetRenderSystem().RemoveEffect(m_effctSelect);
                        m_effctSelect = null;
                    }
                    string strSelectEffct = "effect/UI/EF_Buff_@MuBiaoXuanZe004.fx";
                    strSelectEffct = strSelectEffct.ToLower();
                    Engine.RareEngine.Instance().GetRenderSystem().CreateEffect(ref strSelectEffct, ref m_effctSelect, null);
                    InitEffct(m_effctSelect);
                    SetSelectTargetEffectPos();
                }
                break;
            case GameEventID.SKILLSYSTEM_SHOWDAMAGE:
                {
                    Client.IEntity entity = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().GetCurTarget();
                    if (entity != null)
                    {

                    }
                }
                break;
            case GameEventID.ENTITYSYSTEM_HPUPDATE:
                {
                    UpdateProprty(param);
                }
                break;
            case GameEventID.ENTITYSYSTEM_MPUPDATE:
                {
                    UpdateProprty(param);
                }
                break;
            case GameEventID.ENTITYSYSTEM_PROPUPDATE:
                {
                    UpdateProprty(param);
                }
                break;
            case GameEventID.PLAYER_FIGHTPOWER_REFRESH:
                {
                    RefreshFightPower(param);
                }
                break;
            case GameEventID.SKILL_SHOWPETSKILL:
                {
                    stShowPetSkill st = (stShowPetSkill)param;
                    m_bShowPetQuick = st.bShow;
                    // ShowPetQuickInfo(st.bShow);
                }
                break;
            case Client.GameEventID.CAMP_ADDCOLLECTNPC:
                {
                    UpdateFBPickBtn(param);
                }
                break;
            case Client.GameEventID.ROBOTCOMBAT_STOP:
                {
                    if (m_spriteEx_btnAutoFight != null)
                    {
                        m_spriteEx_btnAutoFight.ChangeSprite(1);
                    }
                }
                break;
            case Client.GameEventID.ROBOTCOMBAT_START:
                {
                    if (m_spriteEx_btnAutoFight != null)
                    {
                        m_spriteEx_btnAutoFight.ChangeSprite(2);
                    }
                }
                break;
            case GameEventID.MAINPANEL_SHOWREDWARING:
                {
                    stShowMainPanelRedPoint st = (stShowMainPanelRedPoint)param;
                    WarningDirection direction = (WarningDirection)st.direction;
                    WarningEnum model = (WarningEnum)st.modelID;
                    bool bShow = (bool)st.bShowRed;
                    GameObject redPointObj = null;
                    switch ((WarningEnum)st.modelID)
                    {
                        case WarningEnum.LearnSkill:
                            redPointObj = m_sprite_learnskill_warning.gameObject;
                            break;
                        case WarningEnum.SEVENDAY:
                            redPointObj = m_sprite_seven_warning.gameObject;
                            break;
                        case WarningEnum.WELFARE:
                            redPointObj = m_sprite_welfare_warning.gameObject;
                            break;
                        case WarningEnum.TITLE_NEWTITLE:
                            redPointObj = m_sprite_prop_warning.gameObject;
                            break;
                        case WarningEnum.Question:
                            redPointObj = m_sprite_question_warning.gameObject;
                            break;
                        case WarningEnum.Mail:
                            redPointObj = m_sprite_friend_warning.gameObject;
                            break;
                        case WarningEnum.Noble:
                            redPointObj = m_sprite_mall_warning.gameObject;
                            m_sprite_Noble_warning.gameObject.SetActive(bShow);
                            break;
                        case WarningEnum.Clan:
                            redPointObj = m_sprite_clan_warning.gameObject;
                            break;
                        case WarningEnum.Achievement:
                            redPointObj = m_sprite_achievement_warning.gameObject;
                            break;
                        case WarningEnum.Daily:
                            redPointObj = m_sprite_daily_warning.gameObject;
                            break;
                        case WarningEnum.GodWeapen:
                            redPointObj = m_sprite_godweapen_warning.gameObject;
                            break;
                        case WarningEnum.OpenServer:
                            redPointObj = m_sprite_openserver_warning.gameObject;
                            break;
                        case WarningEnum.FirstRechargeReward:
                            redPointObj = m_sprite_FirstRecharge_warning.gameObject;
                            break;
                        case WarningEnum.Activity:
                            redPointObj = m_sprite_activity_warning.gameObject;
                            break;
                        case WarningEnum.Forging:
                            redPointObj = m_sprite_forging_warning.gameObject;
                            break;
                        case WarningEnum.Accumulative:
                            redPointObj = m_sprite_Accumulative_warning.gameObject;
                            break;
                        case WarningEnum.Ride:
                            redPointObj = m_sprite_ride_warning.gameObject;
                            break;
                        case WarningEnum.HttpDown:
                            redPointObj = m_sprite_HttpDown_warning.gameObject;
                            break;

                    }
                    if (redPointObj != null)
                    {
                        redPointObj.SetActive(bShow);
                    }

                    if (direction == WarningDirection.None)
                    {
                        //不涉及到三角标和头像红点提示的系统return;
                        return;
                    }
                    if (!m_lstWarnningSystem.ContainsKey(direction))
                    {
                        m_lstWarnningSystem.Add(direction, new List<WarningEnum>());
                    }
                    if (bShow)
                    {
                        if (!m_lstWarnningSystem[direction].Contains(model))
                        {
                            m_lstWarnningSystem[direction].Add(model);
                        }
                    }
                    else
                    {
                        if (m_lstWarnningSystem[direction].Contains(model))
                        {
                            m_lstWarnningSystem[direction].Remove(model);
                        }
                    }
                    if (direction == WarningDirection.Right)
                    {
                        bool showRed = m_lstWarnningSystem[direction].Count > 0;
                        m_sprite_leftbtn_warning.gameObject.SetActive(showRed);
                    }
                    else if (direction == WarningDirection.Left)
                    {
                        m_sprite_face_warning.gameObject.SetActive(m_lstWarnningSystem[direction].Count > 0);
                    }


                }
                break;
            //摇杆
            case GameEventID.ENTITYSYSTEM_LEAVEMAP:
                {
                    ResetJoystick();
                    //小地图
                    ReleaseTexture();
                }
                break;
            case GameEventID.SKLL_LONGPRESS:
                {
                    stSkillLongPress press = (stSkillLongPress)param;
                    //if (press.userID == MainPlayerHelper.GetPlayerUID())
                    {
                        m_bSkillLongPress = press.bLongPress;
                    }
                }
                break;
            case GameEventID.ENTITYSYSTEM_RELIVE:
                {
                    stEntityRelive ed = (stEntityRelive)param;
                    if (ed.uid == MainPlayerHelper.GetPlayerUID())
                    {
                        ResetJoystick();

                        //清采集按钮
                        if (DataManager.Manager<ComBatCopyDataManager>().EnterCopyID != 0)
                        {
                            stCampCollectNpc npc = new stCampCollectNpc { enter = false, npcid = 0 };
                            UpdateFBPickBtn(npc);
                        }
                    }
                }
                break;
            case GameEventID.ENTITYSYSTEM_NEWNAME:
                {
                    Client.stNewName name = (Client.stNewName)param;
                    UserData.CurrentRole.name = name.newName;
                    m_label_mainroleName.text = name.newName;
                }
                break;
            case GameEventID.EJOYSTICKSTABLE:
                {
                    if (param != null)
                    {
                        m_bJoystickStable = (bool)param;
                    }
                    SetMainCollider();
                }
                break;
            case GameEventID.REFRESHTRANSMITPUSHMSGSTATUS:
                {
                    RefreshBtns();
                }
                break;
            case GameEventID.SYSTEM_GAME_READY:
                {
                    ControlMainBtnParticle(m_sprite_FanLi_warning.parent.transform, true);

                    //ControlMainBtnParticle(m_sprite_FirstRecharge_warning.parent.transform, true);
                    //ControlMainBtnParticle(m_sprite_activity_warning.parent.transform, true);
                    //ControlMainBtnParticle(m_sprite_Noble_warning.parent.transform, true);
                    //ControlMainBtnParticle(m_sprite_Accumulative_warning.parent.transform, true);

                }
                break;
            default:
                break;
        }

    }

    /// <summary>
    ///刷新角色战斗力
    /// </summary>
    private void RefreshFightPower(object param)
    {
        if (m_label_lablePower != null)
        {
            IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
            int power = 0;
            int previousPower = 0;
            if (null != player)
            {
                power = player.GetProp((int)Client.FightCreatureProp.Power);
            }
            stRefreshPowerParams par = (stRefreshPowerParams)param;
            previousPower = par.PreFightPower;
            m_label_lablePower.text = power.ToString();
            if (previousPower != par.CurFightPower)
            {
                TipsManager.Instance.ShowFightPowerChangeTips(previousPower < power, power, previousPower);
            }
        }
    }


    private void UpdateProprty(object param)
    {
        stPropUpdate prop = (stPropUpdate)param;

        //更新目标血条
        if (prop.nPropIndex == (int)CreatureProp.Hp)
        {
            Client.IEntity target = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl().GetCurTarget();
            if (target != null && target.GetUID() == prop.uid)
            {
                Client.IEntity entity = ClientGlobal.Instance().GetEntitySystem().FindEntity(prop.uid);
                UpdateTargetHp(entity);
                return;
            }
        }
        //更新自己信息
        if (!ClientGlobal.Instance().IsMainPlayer(prop.uid))
        {
            return;
        }
        if (prop.nPropIndex == (int)CreatureProp.Hp || prop.nPropIndex == (int)CreatureProp.MaxHp)
        {
            Client.IEntity entity = ClientGlobal.Instance().GetEntitySystem().FindEntity(prop.uid);
            if (entity != null)
                SetRoleHp(entity);
        }
        if (prop.nPropIndex == (int)CreatureProp.Mp || prop.nPropIndex == (int)CreatureProp.MaxMp)
        {
            Client.IEntity entity = ClientGlobal.Instance().GetEntitySystem().FindEntity(prop.uid);
            if (entity != null)
                SetRoleMp(entity);
        }
        else if (prop.nPropIndex == (int)PlayerProp.Exp)
        {
            Client.IEntity entity = ClientGlobal.Instance().GetEntitySystem().FindEntity(prop.uid);
            if (entity != null)
                SetRoleExp(entity);
        }
        else if (prop.nPropIndex == (int)PlayerProp.PkMode)
        {
            SetRolePkModel(ClientGlobal.Instance().MainPlayer);
        }
        else if (prop.nPropIndex == (int)CreatureProp.Level)
        {
            int level = MainPlayerHelper.GetPlayerLevel();
            m_label_Level.text = level.ToString();
            m_label_mainroleName.text = UserData.CurrentRole.name;
        }

    }
    #region Buff


    Dictionary<int, string> m_nameDic = new Dictionary<int, string> { { 1, "buff1" }, { 2, "buff2" }, { 3, "buff3" }, { 4, "buff4" }, { 5, "buff5" } };
    string buffname = "";

    void RefreshBuffIcon(Transform widget, ArrayList stateList)
    {
        if (widget == null)
        {
            return;
        }
        widget.gameObject.SetActive(true);
        for (int i = 0; i < totalBuffIconCount; i++)
        {
            int index = i + 1;
            if (m_nameDic.ContainsKey(index))
            {
                buffname = m_nameDic[index];
            }
            Transform buffTrans = widget.transform.Find(buffname.ToString());
            if (buffTrans != null)
            {
                if (i < stateList.Count)
                {
                    Client.stAddBuff st = (Client.stAddBuff)stateList[i];

                    BuffDataBase db = GameTableManager.Instance.GetTableItem<BuffDataBase>(st.buffid, (int)st.level);
                    if (db != null)
                    {
                        UILabel label = buffTrans.Find("Label").GetComponent<UILabel>();
                        label.text = "";
                        if (db.dwShield == 0)
                        {
                            buffTrans.gameObject.SetActive(true);
                        }
                        else
                        {
                            buffTrans.gameObject.SetActive(false);
                        }
                    }

                    BuffUICountDown bc = buffTrans.gameObject.GetComponent<BuffUICountDown>();
                    if (bc == null)
                    {
                        bc = buffTrans.gameObject.AddComponent<BuffUICountDown>();
                    }

                    bc.InitLeftTime((uint)st.lTime, db);
                }
                else
                {
                    buffTrans.gameObject.SetActive(false);
                }
            }
        }
    }
    BuffDataManager BuffManager
    {
        get
        {
            return DataManager.Manager<BuffDataManager>();
        }
    }
    void ShowBuffPanel(GameObject go)
    {
        Transform buffMessage = m_trans_MainRoleBUffContainer.transform.Find("BuffMessage");
        if (buffMessage == null)
        {
            return;
        }
        UIEventListener.Get(buffMessage.gameObject).onClick = HideBuffInfo;
        buffMessage.gameObject.SetActive(true);
        ShowBuffInfo(m_trans_MainRoleBUffContainer, true);

    }
    void HideBuffInfo(GameObject go)
    {
        if (go.activeSelf)
        {

            go.SetActive(false);
        }

    }
    Dictionary<int, string> m_buffItemNameDic = new Dictionary<int, string>(10);
    string GetBuffItemName(int index)
    {
        if (m_buffItemNameDic.ContainsKey(index))
        {
            return m_buffItemNameDic[index];
        }
        else
        {
            string name = "BuffItem" + index;
            m_buffItemNameDic.Add(index, name);
            return name;
        }
    }
    void ShowBuffInfo(Transform container, bool bMainRole)
    {
        if (container == null)
        {
            return;
        }
        int cellHegiht = 100;
        Transform buffMessage = container.transform.Find("BuffMessage");
        if (buffMessage == null)
        {
            return;
        }

        ArrayList buffList = BuffManager.MainRoleBuffList;
        if (!bMainRole)
        {
            buffList = BuffManager.TargetBuffList;
        }
        if (buffList.Count == 0)
        {
            buffMessage.gameObject.SetActive(false);
        }
        else
        {
            //  buffMessage.gameObject.SetActive(true);
        }
        if (!buffMessage.gameObject.activeSelf)
        {
            return;
        }
        int count = buffList.Count < totalBuffIconCount ? buffList.Count : totalBuffIconCount;
        Transform scrollTrans = buffMessage.Find("ScrollView");
        if (scrollTrans == null)
        {
            return;
        }
        Transform bg = buffMessage.Find("Bg");
        if (bg == null)
        {
            return;
        }
        UIScrollView sv = scrollTrans.GetComponent<UIScrollView>();
        if (sv == null)
        {
            return;
        }
        UIPanel panel = sv.GetComponent<UIPanel>();
        Vector4 baseVec = panel.baseClipRegion;
        panel.baseClipRegion = new Vector4(baseVec.x, -cellHegiht * (count) / 2 + cellHegiht / 2, baseVec.z, cellHegiht * count);
        panel.clipOffset = new UnityEngine.Vector2(0, 0);
        UISprite spr = bg.GetComponent<UISprite>();
        if (spr != null)
        {
            spr.height = cellHegiht * count;
        }
        spr.transform.localPosition = new Vector3(0, -cellHegiht * (count - 1) / 2, 0);
        // sv.ResetPosition();


        // Transform gridTrans = scrollTrans.Find("BuffGrid");
        Transform gridTrans = scrollTrans;
        if (gridTrans == null)
        {
            return;
        }

        int childCount = gridTrans.childCount;
        for (int j = buffList.Count; j < childCount; j++)
        {
            string buffItemName = GetBuffItemName(j);
            Transform itemTrans = gridTrans.Find(buffItemName);
            if (itemTrans != null)
            {
                itemTrans.gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < buffList.Count; i++)
        {
            Client.stAddBuff st = (Client.stAddBuff)buffList[i];

            BuffDataBase db = GameTableManager.Instance.GetTableItem<BuffDataBase>(st.buffid, (int)st.level);
            if (db != null)
            {
                GameObject buffItem = null;
                string buffItemName = GetBuffItemName(i);
                Transform itemTrans = gridTrans.Find(buffItemName);
                if (itemTrans != null)
                {
                    buffItem = itemTrans.gameObject;

                }
                else
                {
                    buffItem = GameObject.Instantiate(m_trans_BuffItemInfo.gameObject) as GameObject;
                    itemTrans = buffItem.transform;
                }

                buffItem.transform.parent = sv.transform;//gridTrans;
                itemTrans.localScale = Vector3.one;
                itemTrans.localRotation = Quaternion.identity;
                itemTrans.localPosition = new Vector3(0, -cellHegiht * i, 0);
                //  UIGrid grid = gridTrans.GetComponent<UIGrid>();

                buffItem.gameObject.SetActive(true);
                // grid.repositionNow = true;
                buffItem.name = buffItemName;
                UIDragScrollView dragScroll = buffItem.GetComponent<UIDragScrollView>();
                if (dragScroll == null)
                {
                    dragScroll = buffItem.AddComponent<UIDragScrollView>();
                }
                dragScroll.scrollView = sv;
                BuffItemInfo info = buffItem.GetComponent<BuffItemInfo>();
                if (info == null)
                {
                    info = buffItem.AddComponent<BuffItemInfo>();
                }

                info.InitBuffItemInfo(db, st.lTime);
            }
        }

    }
    void ShowTargetPanel(GameObject go)
    {
        Transform buffMessage = m_trans_TargetBUffContainer.transform.Find("BuffMessage");
        if (buffMessage == null)
        {
            return;
        }
        UIEventListener.Get(buffMessage.gameObject).onClick = HideBuffInfo;
        buffMessage.gameObject.SetActive(true);
        ShowBuffInfo(m_trans_TargetBUffContainer, false);
    }
    #endregion




    //     void OnRequestPeerUser(GameObject go)
    //     {
    //         IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
    //         if (cs != null)
    //         {
    //             IEntity entity = cs.GetActiveCtrl().GetCurTarget();
    //             //stRequestViewRolePropertyUserCmd_C
    //             GameCmd.stRequestViewRolePropertyUserCmd_C cmd = new GameCmd.stRequestViewRolePropertyUserCmd_C() { zoneid = 0, dwUserid = (uint)entity.GetID(), mycharid = (uint)entity.GetID() };
    //             NetService.Instance.Send(cmd);
    //         }
    //     }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_TARGETCHANGE, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.PLAYER_FIGHTPOWER_REFRESH, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKILL_SHOWPETSKILL, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKILLSYSTEM_SHOWDAMAGE, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKILLSYSYTEM_TAB, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_RELIVE, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_NEWNAME, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.REFRESHTRANSMITPUSHMSGSTATUS, OnEvent);
        Engine.Utility.TimerAxis.Instance().KillTimer(MAIN_TIMER_ID, this);

        ReleaseBtns();

        //摇杆
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKLL_LONGPRESS, OnEvent);

        //simplechat
        ReleaseChat();
        ReleaseVoice();

        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ROBOTCOMBAT_STOP, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ROBOTCOMBAT_START, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.EJOYSTICKSTABLE, OnEvent);
        //minimap
        RegisterMiniMapEvents(false);

        //技能
        RegisterGlobalEvent(false);
        UnInit();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eSetRoleProperty)//添加主角的时候刷新界面
        {
            // Client.IEntity entity = (Client.IEntity)param;
            Client.IEntity entity = param as Client.IEntity;
            if (entity != null)
            {
                //SetRoleHp(entity);
                //SetRoleMp(entity);
                //SetRoleExp(entity);
                //SetRolePkModel(entity);
                //if (m_label_lablePower != null)
                //{
                //    m_label_lablePower.text = entity.GetProp((int)FightCreatureProp.Power).ToString();
                //}

                SetRoleHpMpExpPkPower(entity);
            }
        }
        else if (msgid == UIMsgID.stShowBuff)
        {
            stShowBuffInfo info = (stShowBuffInfo)param;

            if (info.IsMainRole)
            {
                ArrayList list = DataManager.Manager<BuffDataManager>().MainRoleBuffList;
                RefreshBuffIcon(m_trans_MainRoleBUffContainer, list);
                ShowBuffInfo(m_trans_MainRoleBUffContainer, true);
            }
            else
            {
                ArrayList list = DataManager.Manager<BuffDataManager>().TargetBuffList;
                RefreshBuffIcon(m_trans_TargetBUffContainer, list);
                ShowBuffInfo(m_trans_TargetBUffContainer, false);
            }

        }
        else if (msgid == UIMsgID.eRefreshEnemyList)
        {
            GameCmd.stEnmityDataUserCmd_S cmd = (GameCmd.stEnmityDataUserCmd_S)param;
            OnEnemyList(cmd);
        }
        else if (msgid == UIMsgID.eShowCopyInfo)
        {
            stCopyInfo info = (stCopyInfo)param;
            ShowFBWidgetUI(info.bShow, info.bShowBattleInfoBtn);
        }
        else if (msgid == UIMsgID.eSkillBtnRefresh)
        {//走新手引导 OnUIEvent
            SetSkillIcon();
        }
        else if (msgid == UIMsgID.eSkillChangeState)
        {
            LearnSkillDataManager data = DataManager.Manager<LearnSkillDataManager>();
            if (m_widget_SkillBtns != null)
            {
                Quaternion rotation = m_widget_SkillBtns.transform.localRotation;
                if (data.CurState == SkillSettingState.StateTwo)
                {
                    m_widget_SkillBtns.transform.DORotate(new Vector3(0, 0, 125), rotTime);
                }
                else
                {
                    m_widget_SkillBtns.transform.DORotate(new Vector3(0, 0, 0), rotTime);
                }
                SetPlayerSkillIcon();
                uint stateSkill = data.GetCurStateSkillIDByJob();
                ExecutePublicCD(stateSkill);

            }
            data.SetCurStateSkillList();

        }
        else if (msgid == UIMsgID.eSkillShowPetSkill)
        {
            if (m_lstSkillBtns.Count > 9)
            {
                m_lstSkillBtns[9].Refresh();
            }
        }
        else if (msgid == UIMsgID.eShortcutList)
        {
            InitShortcutGrid();//快捷使用道具
        }
        else if (msgid == UIMsgID.eShortcutRect)
        {
            Vector3 pos = (Vector3)param;
            PointInRectEvent(pos);
        }
        else if (msgid == UIMsgID.eShortcutRect)
        {
            Vector3 pos = (Vector3)param;
            PointInRectEvent(pos);
        }
        else if (msgid == UIMsgID.eRefreshNpcBelong)
        {
            stRefreshNPCBelongParam data = (stRefreshNPCBelongParam)param;
            UpdateTargetStatus(data.npcid, data.teamid, data.ownerid,data.clanid,data.ownerName);

        }
        else if (msgid == UIMsgID.eAnswerState)
        {
            InitAnswerUI();
        }

        return true;
    }

    #region Fishing

    public void HideHttpDown()
    {
        Transform gmTrans = m_trans_AnchorTopRight.Find("XiaZai");
        if (gmTrans)
        {
            gmTrans.localPosition = new Vector3(1000, 1000, 0);
        }

    }
    /// <summary>
    /// 初始化钓鱼按钮
    /// </summary>
    public void InitFishingUI()
    {
        bool canFishing = DataManager.Manager<FishingManager>().CanFishing();

        if (canFishing)
        {
            m_btn_EnterFishingBtn.gameObject.SetActive(true);
        }
        else
        {
            m_btn_EnterFishingBtn.gameObject.SetActive(false);
        }
    }

    #endregion

    #region Btns

    void onClick_PopupList_Btn(GameObject caster)
    {

    }


    void onClick_ProUps_Btn(GameObject caster)
    {

    }

    public Transform GetPopUpsEnd()
    {
        return m_trans_PopUpsEnd;
    }

    void onClick_EnterFishingBtn_Btn(GameObject caster)
    {
        if (DataManager.Manager<FishingManager>().IsFishing == false)
        {
            //打开钓鱼
            DataManager.Manager<FishingManager>().ReqOpenFishing();

            //一开始请求排行数据
            DataManager.Manager<FishingManager>().ReqFishRanking();
        }
    }

    //void onClick_AccumulativeRecharge_Btn(GameObject caster)
    //{
    //    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.AccumulativeRechargePanel);
    //}

    #endregion Btns
    void onClick_RechargeCtrBtn_Btn(GameObject caster)
    {
        float distance = Mathf.Abs(m_trans_RechargeBtnRoot.localPosition.x);

        if (distance < 0.1f)
        {
            Vector3 newPos = new Vector3(-360f, m_trans_RechargeBtnRoot.localPosition.y, m_trans_RechargeBtnRoot.localPosition.z);

            TweenPosition.Begin(m_trans_RechargeBtnRoot.gameObject, 0.2f, newPos);

            m_btn_RechargeCtrBtn.transform.eulerAngles = new UnityEngine.Vector3(0, 0, 180f);
        }
        else
        {
            Vector3 newPos = new Vector3(0f, m_trans_RechargeBtnRoot.localPosition.y, m_trans_RechargeBtnRoot.localPosition.z);

            TweenPosition.Begin(m_trans_RechargeBtnRoot.gameObject, 0.2f, newPos);

            m_btn_RechargeCtrBtn.transform.eulerAngles = Vector3.zero;
        }
    }

    #region rechargeRoot

    #endregion

}
