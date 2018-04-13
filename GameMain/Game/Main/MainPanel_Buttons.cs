using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Client;

partial class MainPanel
{
    public List<MainBtn> m_lstBtns = null;
    TweenRotation m_btnTween = null;

    //label tips
    //GameObject m_goActionTips = null;
    //UILabel m_labelActionTips = null;

    public enum TipsUIState
    {
        None,           //无状态
        Fishing,        //钓鱼中
        Collect,        //采集中
        UseItem,        //使用道具中
        ChuanSong,      //传送中
        WaBao,          //挖宝中
        Path,           //寻路中
        Robot,          //自动战斗中（挂机）
    }

    public bool IsShowRightBtn()
    {
        if (m_btnTween != null)
        {
            UnityEngine.Vector3 rot = m_btnTween.transform.localEulerAngles;
            rot.z = rot.z < 0 ? rot.z + 360 : rot.z;

            return rot.z == m_btnTween.from.z;
        }
        return false;
    }

    void InitBtns()
    {
        m_lstBtns = new List<MainBtn>();

        //特效TipsUI 
        m_sprite_TipsUI.gameObject.SetActive(false);

        RegisterBtnEvent(true);
        //
        m_trans_roleInfoRoot.gameObject.SetActive(true);
        m_trans_target.gameObject.SetActive(false);
        m_trans_MainRoleBUffContainer.gameObject.SetActive(false);
        //


        m_trans_btn_Left_Root.gameObject.SetActive(false);
        //if (Application.isEditor)
        {
            if (m_trans_AnchorTopRight != null)
            {
                Transform gmTrans = m_trans_AnchorTopRight.Find("GM");
                if (gmTrans != null)
                {
                    gmTrans.gameObject.SetActive(true);
                }
            }
        }
        TweenRotation tween = m_btn_leftbtn.GetComponent<TweenRotation>();
        if (tween != null)
        {
            m_btnTween = tween;
            tween.AddOnFinished(new EventDelegate(() =>
            {
                bool show = IsShowRightBtn();
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINRIGHTBTN_TOGGLE, show);
                UISprite leftBtnSp = m_btnTween.GetComponent<UISprite>();
                if (show)
                {
                    ResetLeftBtnsPos(false);
                    leftBtnSp.spriteName = "anniu_yuan_shouqi";
                    m_btn_leftbtn.normalSprite = "anniu_yuan_shouqi";
                }
                else
                {
                    leftBtnSp.spriteName = "anniu_yuan_zhankai";
                    m_btn_leftbtn.normalSprite = "anniu_yuan_zhankai";
                }

                //发送UI，功能按钮状态变化事件
                SendGameObjMoveStatusEvent((show) ? UIDefine.GameObjMoveStatus.Visible : UIDefine.GameObjMoveStatus.Invisible, m_lst_FuncBtnsRight);
            }));
        }
    }

    private void RegisterBtnEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.MAINBUTTON_ADD, OnAddBtnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.MAINBUTTON_STATUSCHANGE, OnSetBtnStatus);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.MAINBUTTON_REDTIPS, OnSetBtnStatus);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTNEWFUNCOPEN, OnSetBtnStatus);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.JOYSTICK_PRESS, OnAddBtnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.JOYSTICK_UNPRESS, OnAddBtnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_RIDE, OnAddBtnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_UNRIDE, OnAddBtnEvent);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_START, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_STOP, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_VISITNPC, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, OnCombotEvent);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.MAINBTN_ONTOGGLE, OnMainBtnToggle);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.MAINBUTTON_ADD, OnAddBtnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.MAINBUTTON_STATUSCHANGE, OnSetBtnStatus);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.MAINBUTTON_REDTIPS, OnSetBtnStatus);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTNEWFUNCOPEN, OnSetBtnStatus);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.JOYSTICK_PRESS, OnAddBtnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.JOYSTICK_UNPRESS, OnAddBtnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_RIDE, OnAddBtnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_UNRIDE, OnAddBtnEvent);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ROBOTCOMBAT_START, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ROBOTCOMBAT_STOP, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ROBOTCOMBAT_VISITNPC, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, OnCombotEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, OnCombotEvent);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.MAINBTN_ONTOGGLE, OnMainBtnToggle);
        }
    }

    /// <summary>
    /// 发送UI，功能按钮状态变化事件
    /// </summary>
    /// <param name="status"></param>
    /// <param name="objs"></param>
    private void SendGameObjMoveStatusEvent(UIDefine.GameObjMoveStatus status, List<GameObject> objs)
    {

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGAMEOBJMOVESTATUSCHANGED, new UIDefine.GameObjMoveData()
        {
            Status = status,
            Objs = objs,
        });
    }
    MainBtn GetMainBtnByType(MainBtnDef.BtnType type)
    {
        for (int i = 0; i < m_lstBtns.Count; i++)
        {
            if (m_lstBtns[i] == null)
            {
                continue;
            }
            if (m_lstBtns[i].Index == (int)type)
            {
                return m_lstBtns[i];
            }
        }
        return null;
    }

    void OnMainBtnToggle(int nEventId, object param)
    {
        if (nEventId == (int)Client.GameEventID.MAINBTN_ONTOGGLE)
        {
            if (DataManager.Manager<GuideManager>().IsDoingGuide(GuideDefine.GuideType.Constraint)
                || DataManager.Manager<GuideManager>().HaveUnFinishGuide(GuideDefine.GuideType.Constraint))
            {
                //处于强制引导过程中忽略
                return;
            }
            stMainBtnSet setInfo = (stMainBtnSet)param;
            if (setInfo.pos == 1)
            {
                if (setInfo.isShow)
                {
                    ShowLeftBtn();
                }
                else
                {
                    ResetLeftBtnsPos();
                }
            }
            else if (setInfo.pos == 2)
            {
                if (m_btnTween != null)
                {
                    bool show = IsShowRightBtn();
                    if (setInfo.isShow != show)
                    {
                        m_btnTween.gameObject.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }
        }
    }

    void OnCombotEvent(int nEventId, object param)
    {
        if (nEventId == (int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH)
        {
            Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
            if (cs.GetCombatRobot().Status != Client.CombatRobotStatus.STOP)
            {
                return;
            }

            if (param != null)
            {
                bool bsearch = (bool)param;

                if (DataManager.Manager<SliderDataManager>().IsReadingSlider == false)
                {
                    ShowTipsUIByState(TipsUIState.Path, bsearch);
                }
            }
        }
        else if (nEventId == (int)Client.GameEventID.ROBOTCOMBAT_START)
        {
            //自动战斗特效
            ShowTipsUIByState(TipsUIState.Robot, true);
        }
        else if (nEventId == (int)Client.GameEventID.ROBOTCOMBAT_STOP)
        {
            ShowTipsUIByState(TipsUIState.Robot, false);
        }
        else if (nEventId == (int)Client.GameEventID.ROBOTCOMBAT_VISITNPC)
        {
            Client.stVisitNpc visit = (Client.stVisitNpc)param;
            if (visit.state == false)
            {
                ShowTipsUIByState(TipsUIState.None);
            }
        }
        else if (nEventId == (int)GameEventID.SKILLGUIDE_PROGRESSSTART)
        {
            Client.stUninterruptMagic uninterrupt = (Client.stUninterruptMagic)param;

            if (Client.ClientGlobal.Instance().IsMainPlayer(uninterrupt.uid) == false)//非本人
            {
                return;
            }

            if (ShowCollectTip(uninterrupt.type))
            {
                if (uninterrupt.type == GameCmd.UninterruptActionType.UninterruptActionType_SYDJ)
                {
                    ShowTipsUIByState(TipsUIState.UseItem, true);
                }
                else if (uninterrupt.type == GameCmd.UninterruptActionType.UninterruptActionType_CangBaoTuCJ)
                {
                    ShowTipsUIByState(TipsUIState.WaBao, true);
                }
                else if (uninterrupt.type == GameCmd.UninterruptActionType.UninterruptActionType_GOHOME)
                {
                    ShowTipsUIByState(TipsUIState.ChuanSong, true);
                }
                else
                {
                    ShowTipsUIByState(TipsUIState.Collect, true);
                }
            }
        }
        else if (nEventId == (int)GameEventID.SKILLGUIDE_PROGRESSBREAK)
        {
            if (param != null)
            {
                stGuildBreak guildbreak = (stGuildBreak)param;
                if (ShowCollectTip(guildbreak.action))
                {
                    ShowTipsUIByState(TipsUIState.None);
                }
            }
        }
        else if (nEventId == (int)GameEventID.SKILLGUIDE_PROGRESSEND)
        {
            if (param != null)
            {
                stGuildEnd guildEnd = (stGuildEnd)param;
                if (ShowCollectTip(guildEnd.action))
                {
                    ShowTipsUIByState(TipsUIState.None);
                }
            }

        }
    }
    bool ShowCollectTip(GameCmd.UninterruptActionType action)
    {
        return action != GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ && action != GameCmd.UninterruptActionType.UninterruptActionType_DEMON;
    }
    void OnAddBtnEvent(int eventId, object param)
    {
        if (eventId == (int)GameEventID.MAINBUTTON_ADD)
        {
            if (param is MainBtn)
            {
                MainBtn btn = param as MainBtn;
                if (!m_lstBtns.Contains(btn))
                {
                    MainBtnDef.BtnType btnType = (MainBtnDef.BtnType)System.Enum.Parse(typeof(MainBtnDef.BtnType), "BTN" + btn.gameObject.name.ToUpper());
                    btn.OnClick = OnMainBtnClick;
                    btn.Index = (int)btnType;
                    m_lstBtns.Add(btn);

                    if (btnType == MainBtnDef.BtnType.BTNBTNHORSE)
                    {
                        SetRideBtnState();
                    }
                    else if (btnType == MainBtnDef.BtnType.BTNPETROLE)
                    {
                        SetPetRoleBtn(btn);
                    }
                    //Engine.Utility.Log.LogGroup("ZCX", "add btn {0}", btn.m_btnType);
                }
            }
        }
        else if (eventId == (int)Client.GameEventID.JOYSTICK_UNPRESS)
        {
            MainBtn btn = GetMainBtnByType(MainBtnDef.BtnType.BTNBTNHORSE);
            if (btn != null)
            {
                btn.gameObject.SetActive(true);
            }
        }
        else if (eventId == (int)Client.GameEventID.JOYSTICK_PRESS)
        {
            MainBtn btn = GetMainBtnByType(MainBtnDef.BtnType.BTNBTNHORSE);
            if (btn != null)
            {
                btn.gameObject.SetActive(false);
            }

            ShowTipsUIByState(TipsUIState.None);
        }
        else if (eventId == (int)Client.GameEventID.ENTITYSYSTEM_UNRIDE)
        {
            stEntityUnRide ride = (stEntityUnRide)param;
            if (ride.uid == ClientGlobal.Instance().MainPlayer.GetUID())
            {
                MainBtn btn = GetMainBtnByType(MainBtnDef.BtnType.BTNBTNHORSE);
                if (btn != null)
                {
                    // btn.GetComponentInChildren<UILabel>().text = "骑乘";
                }
            }
        }
        else if (eventId == (int)Client.GameEventID.ENTITYSYSTEM_RIDE)
        {
            stEntityRide ride = (stEntityRide)param;
            if (ride.uid == ClientGlobal.Instance().MainPlayer.GetUID())
            {
                MainBtn btn = GetMainBtnByType(MainBtnDef.BtnType.BTNBTNHORSE);
                if (btn != null)
                {
                    //  btn.GetComponentInChildren<UILabel>().text = "下马";
                }
            }
        }
        SetGmBtn();
    }
    void SetGmBtn()
    {
        MainBtn btn = GetMainBtnByType(MainBtnDef.BtnType.BTNGM);
        if (btn != null)
        {
            if (Application.isEditor)
            {
                btn.gameObject.SetActive(true);
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }
    void SetRideBtnState()
    {
        bool show = DataManager.Manager<GuideManager>().IsNewFuncOpenComplete(4);

        MainBtn btn = GetMainBtnByType(MainBtnDef.BtnType.BTNBTNHORSE);
        if (btn != null)
        {
            btn.gameObject.SetActive(show);
        }
    }
    void SetMallRedPoint()
    {
        DataManager.Manager<Mall_HuangLingManager>().HasNobleWarning();
    }
    void SetFirstRechargeRewardRedPoint()
    {
        bool show = DataManager.Manager<Mall_HuangLingManager>().AlreadyFirstRecharge.Count > 0 && !DataManager.Manager<ActivityManager>().HadGotFirstRechargeReward;
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.FirstRechargeReward,
            direction = (int)WarningDirection.None,
            bShowRed = show,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }

    void SetAchievementRedPoint()
    {
        bool show = DataManager.Manager<AchievementManager>().HaveCanReceiveAchieve();
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Achievement,
            direction = (int)WarningDirection.Left,
            bShowRed = show,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }
    void SetActivityRedPoint()
    {
        bool show = DataManager.Manager<ActivityManager>().HaveRewardCanGet();
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Activity,
            direction = (int)WarningDirection.None,
            bShowRed = show,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }
    void SetForgingRedPoint()
    {
        bool CanStrength = DataManager.Manager<ForgingManager>().HaveEquipCanStrengthen();
        bool CanInlay = DataManager.Manager<ForgingManager>().HaveEquipCanInlayByIndex();
        bool open = MainPlayerHelper.GetPlayerLevel() >= DataManager.Manager<ForgingManager>().OpenLv;
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Forging,
            direction = (int)WarningDirection.Left,
            bShowRed = open && (CanStrength || CanInlay),
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }

    void SetFriendRedPoint()
    {
        bool haveChat = DataManager.Manager<ChatDataManager>().PrivateChatManager.HaveMsgFromFriend;
        bool haveMail = DataManager.Manager<MailManager>().HaveMailCanGet;
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Mail,
            direction = (int)WarningDirection.None,
            bShowRed = haveMail || haveChat,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }

    //累充
    void SetAccumulativeRechRedPoint()
    {
        if (DataManager.Manager<ActivityManager>().IsWeekRechargeReach())
        {
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.Accumulative,
                direction = (int)WarningDirection.None,
                bShowRed = true,
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        }
    }
    void SetRideRedPoint()
    {
        if (DataManager.Manager<RideManager>().IsShowRideRedPoint())
        {
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.Ride,
                direction = (int)WarningDirection.None,
                bShowRed = true,
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        }
    }

    void SetHttpDownPoint()
    {
        if (KDownloadInstance.Instance().GetTakeReward() == false)
        {
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.HttpDown,
                direction = (int)WarningDirection.None,
                bShowRed = true,
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        }
        else
        {

        }

        Transform gmTrans = m_trans_AnchorTopRight.Find("XiaZai");
        if (gmTrans)
        {
            //gmTrans.gameObject.SetActive(true);
            UIParticleWidget p = gmTrans.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = gmTrans.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
            if (p != null)
            {
                p.SetDimensions(150, 50);
                p.ReleaseParticle();
                p.AddParticle(50040);
            }

        }


    }

    void OnSetBtnStatus(int nEventId, object param)
    {
        if (nEventId == (int)GameEventID.MAINBUTTON_STATUSCHANGE)
        {
            stMainButtonStatus status = (stMainButtonStatus)param;
            MainBtn btn = GetMainBtnByType((MainBtnDef.BtnType)status.btnID);
            if (btn != null)
            {
                if (status.status == 1)
                {
                    btn.gameObject.SetActive(true);
                }
                else
                {
                    btn.gameObject.SetActive(false);
                }
            }
        }
        else if (nEventId == (int)GameEventID.MAINBUTTON_REDTIPS)
        {
            stMainButtonRedTips tipsdata = (stMainButtonRedTips)param;
            MainBtn btn = GetMainBtnByType((MainBtnDef.BtnType)tipsdata.btnID);
            if (btn != null)
            {
                btn.SetRedTipVisble(tipsdata.status == 1);
            }
        } if (nEventId == (int)Client.GameEventID.UIEVENTNEWFUNCOPEN)
        {
            uint funcId = (uint)param;
            if (funcId == 4)
            {
                MainBtn btn = GetMainBtnByType(MainBtnDef.BtnType.BTNBTNHORSE);
                if (btn != null)
                {
                    btn.gameObject.SetActive(true);
                }
            }
        }
    }

    void OnMainBtnClick(int index)
    {
        MainBtnDef.BtnType btnType = (MainBtnDef.BtnType)index;
        switch (btnType)
        {
            case MainBtnDef.BtnType.BTNBAG:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.KnapsackPanel, forceResetPanel: true);
                }
                break;
            case MainBtnDef.BtnType.BTNFROGING:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ForgingPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNFRIEND:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FriendPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNGEM:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ComposePanel);
                }
                break;
            case MainBtnDef.BtnType.BTNBTNHORSE:
                {

                    if (DataManager.Manager<RideManager>().Auto_Ride <= 0)
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_haiweishezhichuzhanzuoqiwufaqicheng);
                        return;
                    }
                    Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
                    if (mainPlayer != null)
                    {
                        bool bChange = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommand_IsChange, null);
                        if (bChange)
                        {
                            TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_bianshenzhuangtaixiawufaqicheng);
                            return;
                        }
                        bool bRide = DataManager.Manager<RideManager>().IsRide;
                        if (mainPlayer != null)
                        {
                            if (bRide)
                            {
                                DataManager.Instance.Sender.RideDownRide(null);
                            }
                            else
                            {
                                DataManager.Manager<RideManager>().TryUsingRide(null, null);
                            }
                        }
                    }
                }
                break;
            case MainBtnDef.BtnType.BTNRIDE:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RidePanel);
                }
                break;
            case MainBtnDef.BtnType.BTNPET:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNLEARNSKILL:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LearnSkillPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNMUHON:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MuhonPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNSETTING:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.SettingPanel);

                }
                break;
            case MainBtnDef.BtnType.BTNGM:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GMPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNXIAZAI:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNHOME:
                {

                    int level = GameTableManager.Instance.GetGlobalConfig<int>("HomeBaseOpenLv");
                    IPlayer mp = ClientGlobal.Instance().MainPlayer;
                    if (mp != null)
                    {
                        int playerLevel = mp.GetProp((int)CreatureProp.Level);
                        if (playerLevel < level)
                        {
                            TipsManager.Instance.ShowTips("家园需要" + level + "级开放");
                            return;
                        }
                        else
                        {
                            HomeScene.Instance.Enter();
                        }
                    }
                }
                break;

            case MainBtnDef.BtnType.BTNPROP:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PropPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNARENA:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ArenaPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNSTORE:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MallPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNBLACKMARKET:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.BlackMarketPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNCLAN:
                {
                    if (!DataManager.Manager<ClanManger>().IsClanEnable)
                    {
                        TipsManager.Instance.ShowTips(string.Format("{0}级开放", ClanManger.ClanUnlockLevel));
                    }
                    else
                    {
                        if (DataManager.Manager<ClanManger>().IsJoinClan
                            && DataManager.Manager<ClanManger>().IsJoinFormal)
                        {
                            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanPanel);
                        }
                        else
                        {
                            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanCreatePanel);
                        }
                    }

                }
                break;
            case MainBtnDef.BtnType.BTNRANKLIST:
                {
                    int level = GameTableManager.Instance.GetGlobalConfig<int>("Rank_OpenLevel");
                    IPlayer mp = ClientGlobal.Instance().MainPlayer;
                    if (mp != null)
                    {
                        int playerLevel = mp.GetProp((int)CreatureProp.Level);
                        if (playerLevel < level)
                        {
                            TipsManager.Instance.ShowTips(string.Format("{0}级开启排行榜功能", level));
                            return;
                        }
                    }
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RankPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNFUBEN:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FBPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNPETROLE:
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PetPanel);
                }
                break;
            case MainBtnDef.BtnType.BTNAUCTION:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ConsignmentPanel);
                break;
            case MainBtnDef.BtnType.BTNDAILY:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DailyPanel);
                break;
            case MainBtnDef.BtnType.BTNWELFARE:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.WelfarePanel);
                break;
            case MainBtnDef.BtnType.BTNSEVENDAY:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.SevenDayPanel);
                break;
            //             case MainBtnDef.BtnType.BTNHUNTING:
            //                 DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.HuntingPanel);
            //                 break;
            case MainBtnDef.BtnType.BTNACHIEVEMENT:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.AchievementPanel);
                break;
            case MainBtnDef.BtnType.BTNACTIVITY:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ActivityPanel);
                break;

            case MainBtnDef.BtnType.BTNGROWUP:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GrowUpPanel);
                break;
            case MainBtnDef.BtnType.BTNQUESTION:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.QuestionPanel);
                break;
            case MainBtnDef.BtnType.BTNGODWEAPEN:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GodWeapenPanel);
                break;
            case MainBtnDef.BtnType.BTNOPENSERVER:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.OpenServerPanel);
                break;
            case MainBtnDef.BtnType.BTNFIRSTRECHARGE:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.FirstRechargePanel);
                ControlMainBtnParticle(m_sprite_FirstRecharge_warning.parent.transform, false);
                break;
            case MainBtnDef.BtnType.BTNFANLI:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ActivityRewardListPanel);
                break;
            case MainBtnDef.BtnType.BTNACCUMULATIVERECHARGE:
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.AccumulativeRechargePanel);
                break;
            case MainBtnDef.BtnType.BTNNOBLE:
                ItemManager.DoJump(112);//黄令
                break;
            case MainBtnDef.BtnType.None:
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 左上角 按钮展开逻辑
    /// </summary>
    /// <param name="caster"></param>
    void onClick_BtnShowFunc_Btn(GameObject caster)
    {
        bool show = m_trans_btn_Left_Root.gameObject.activeSelf;
        bool guideForceShow = false;
        if (null != caster)
        {
            GuideTriggerData gtData = caster.GetComponent<GuideTriggerData>();
            if (null != gtData && gtData.IsGuideTrigger)
            {
                gtData.IsGuideTrigger = false;
                guideForceShow = true;
            }
        }

        if (show)
        {
            if (!guideForceShow)
                ResetLeftBtnsPos();
        }
        else
        {
            ShowLeftBtn();
        }
    }

    /// <summary>
    /// 左上角按钮显示
    /// </summary>
    private void ShowLeftBtn()
    {
        if (IsShowRightBtn())
        {
            m_btn_leftbtn.gameObject.SendMessage("OnClick", SendMessageOptions.RequireReceiver);
        }

        if (!m_trans_btn_Left_Root.gameObject.activeSelf)
        {
            m_trans_btn_Left_Root.gameObject.SetActive(true);
            m_trans_btn_Left_Root.transform.localPosition = new Vector3(0, 300, 0);

            m_trans_roleInfoRoot.gameObject.SetActive(false);
            TweenPosition tp = TweenPosition.Begin(m_trans_btn_Left_Root.gameObject, 0.25f, Vector3.zero);
            //发送UI，功能按钮状态变化事件
            SendGameObjMoveStatusEvent(UIDefine.GameObjMoveStatus.MoveToVisible, m_lst_FuncBtnsLeft);
            tp.onFinished.Clear();
            tp.AddOnFinished(new EventDelegate(() =>
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINLEFTTBTN_TOGGLE, true);
                //发送UI，功能按钮状态变化事件
                SendGameObjMoveStatusEvent(UIDefine.GameObjMoveStatus.Visible, m_lst_FuncBtnsLeft);
            }));
        }

    }
    /// <summary>
    /// 左上角按钮还原隐藏
    /// </summary>
    void ResetLeftBtnsPos(bool sendEvent = true)
    {
        if (m_trans_btn_Left_Root.gameObject.activeSelf)
        {
            m_trans_btn_Left_Root.gameObject.SetActive(false);

            m_trans_roleInfoRoot.transform.localPosition = new Vector3(0, 300, 0);
            m_trans_roleInfoRoot.gameObject.SetActive(true);

            TweenPosition tp = TweenPosition.Begin(m_trans_roleInfoRoot.gameObject, 0.25f, Vector3.zero);
            //发送UI，功能按钮状态变化事件
            SendGameObjMoveStatusEvent(UIDefine.GameObjMoveStatus.MoveToInvisible, m_lst_FuncBtnsLeft);
            tp.onFinished.Clear();
            //TweenPosition.Begin(m_trans_btn_Right_Root.gameObject, 0.25f, Vector3.zero);
            if (sendEvent)
            {
                tp.AddOnFinished(new EventDelegate(() =>
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINLEFTTBTN_TOGGLE, false);
                }));
            }
            tp.AddOnFinished(new EventDelegate(() =>
            {
                //发送UI，功能按钮状态变化事件
                SendGameObjMoveStatusEvent(UIDefine.GameObjMoveStatus.Invisible, m_lst_FuncBtnsLeft);
            }));

        }
    }

    void ReleaseBtns()
    {
        RegisterBtnEvent(false);
    }

    void onClick_Leftbtn_Btn(GameObject caster)
    {
        bool show = IsShowRightBtn();
        bool guideForceShow = false;
        if (null != caster)
        {
            GuideTriggerData gtData = caster.GetComponent<GuideTriggerData>();
            if (null != gtData && gtData.IsGuideTrigger)
            {
                gtData.IsGuideTrigger = false;
                guideForceShow = true;
            }
        }
        UIDefine.GameObjMoveStatus stats = UIDefine.GameObjMoveStatus.None;
        if (show)
        {
            if (!guideForceShow)
                stats = UIDefine.GameObjMoveStatus.MoveToInvisible;
        }
        else
        {
            stats = UIDefine.GameObjMoveStatus.MoveToVisible;
        }
        //发送UI，功能按钮状态变化事件
        SendGameObjMoveStatusEvent(stats, m_lst_FuncBtnsRight);
    }

    #region FunctionBtn Status
    List<GameObject> m_lst_FuncBtnsRight = null;
    List<GameObject> m_lst_FuncBtnsLeft = null;
    private void InitFuncBtns()
    {
        if (null == m_lst_FuncBtnsLeft)
        {
            m_lst_FuncBtnsLeft = new List<GameObject>();
        }
        Transform ts = null;
        if (null != m_trans_btn_Left_Root)
        {
            for (int i = 0; i < m_trans_btn_Left_Root.childCount; i++)
            {
                ts = m_trans_btn_Left_Root.GetChild(i);
                if (null != ts && !m_lst_FuncBtnsLeft.Contains(ts.gameObject))
                {
                    m_lst_FuncBtnsLeft.Add(ts.gameObject);
                }
            }
        }

        if (null == m_lst_FuncBtnsRight)
        {
            m_lst_FuncBtnsRight = new List<GameObject>();
        }
        if (null != m_trans_leftbtnRoot)
        {
            for (int i = 0; i < m_trans_leftbtnRoot.childCount; i++)
            {
                ts = m_trans_leftbtnRoot.GetChild(i);
                if (null != ts && !m_lst_FuncBtnsRight.Contains(ts.gameObject))
                {
                    m_lst_FuncBtnsRight.Add(ts.gameObject);
                }
            }
        }
    }

    #endregion

    #region TipsUI

    /// <summary>
    /// 下面的提示：自动战斗中
    /// </summary>
    void PlayTipsUI()
    {
        CombatRobotStatus robotState = GetRobotState();
        //挂机
        if (robotState == CombatRobotStatus.RUNNING)
        {
            ShowTipsUIByState(TipsUIState.Robot, true);
        }
    }

    /// <summary>
    /// 自动挂机特效
    /// </summary>
    void PlayRobotEffect(bool isShow)
    {
        //特效
        UIParticleWidget wight = m_trans_robotEffectRoot.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = m_trans_robotEffectRoot.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        if (isShow)
        {
            //1、挂机特效
            m_trans_robotEffectRoot.gameObject.SetActive(true);
            m_trans_pathEffectRoot.gameObject.SetActive(false);
            m_label_tipsLabelRoot.gameObject.SetActive(false);

            if (wight != null)
            {
                wight.ReleaseParticle();
                wight.AddParticle(50022);
            }

            //2、 挂机按钮状态
            if (m_spriteEx_btnAutoFight != null)
            {
                m_spriteEx_btnAutoFight.ChangeSprite(2);
            }
        }
        else
        {
            //1、释放特效
            m_trans_robotEffectRoot.gameObject.SetActive(false);
            if (wight != null)
            {
                wight.ReleaseParticle();
            }

            //2、 挂机按钮状态
            if (m_spriteEx_btnAutoFight != null)
            {
                m_spriteEx_btnAutoFight.ChangeSprite(1);
            }
        }
    }

    CombatRobotStatus GetRobotState()
    {
        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs != null)
        {
            ICombatRobot cr = cs.GetCombatRobot();
            if (cr != null)
            {
                return cr.Status;
            }
        }

        return CombatRobotStatus.STOP;
    }

    /// <summary>
    /// 寻路特效
    /// </summary>
    void PlayPathEffect()
    {
        m_trans_robotEffectRoot.gameObject.SetActive(false);
        m_trans_pathEffectRoot.gameObject.SetActive(true);
        m_label_tipsLabelRoot.gameObject.SetActive(false);
        //特效
        UIParticleWidget wight = m_trans_pathEffectRoot.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = m_trans_pathEffectRoot.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        if (wight != null)
        {
            wight.ReleaseParticle();
            wight.AddParticle(50021);
        }
    }

    void CleanPathEffect()
    {
        if (m_trans_pathEffectRoot.gameObject.activeSelf)
        {
            m_trans_pathEffectRoot.gameObject.SetActive(false);

            UIParticleWidget wight = m_trans_pathEffectRoot.GetComponent<UIParticleWidget>();
            if (null != wight)
            {
                wight.ReleaseParticle();
            }
        }
    }



    void ShowTipsUIByState(TipsUIState state, bool isShow = true)
    {
        m_sprite_TipsUI.gameObject.SetActive(state != TipsUIState.None);
        if (state != TipsUIState.None)
        {
            m_sprite_TipsUI.gameObject.SetActive(isShow);
        }

        if (state == TipsUIState.Collect) //采集中
        {
            m_trans_pathEffectRoot.gameObject.SetActive(false);
            m_trans_robotEffectRoot.gameObject.SetActive(false);
            m_label_tipsLabelRoot.gameObject.SetActive(true);
            m_label_tipsLabelRoot.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Task_Status_1);
        }
        else if (state == TipsUIState.UseItem) //道具使用中
        {
            m_trans_pathEffectRoot.gameObject.SetActive(false);
            m_trans_robotEffectRoot.gameObject.SetActive(false);
            m_label_tipsLabelRoot.gameObject.SetActive(true);
            m_label_tipsLabelRoot.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Task_Status_2);

        }
        else if (state == TipsUIState.ChuanSong) //传送中
        {
            m_trans_pathEffectRoot.gameObject.SetActive(false);
            m_trans_robotEffectRoot.gameObject.SetActive(false);
            m_label_tipsLabelRoot.gameObject.SetActive(true);
            m_label_tipsLabelRoot.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Task_Status_5);
        }
        else if (state == TipsUIState.WaBao)      //挖宝中
        {
            m_trans_pathEffectRoot.gameObject.SetActive(false);
            m_trans_robotEffectRoot.gameObject.SetActive(false);
            m_label_tipsLabelRoot.gameObject.SetActive(true);
            m_label_tipsLabelRoot.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Cangbaotu_Status_wabaozhong);
        }
        else if (state == TipsUIState.Path)  //寻路中
        {
            if (isShow)
            {
                PlayPathEffect();
            }
            else
            {
                CleanPathEffect();
            }
        }
        else if (state == TipsUIState.Robot) //自动战斗中
        {
            PlayRobotEffect(isShow);
        }
    }

    #endregion

}