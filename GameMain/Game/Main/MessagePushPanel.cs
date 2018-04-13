
using System;
using System.Collections.Generic;
using UnityEngine;
public enum TransmitAndInviteType
{
    TeamInvite = 1,  //
    TeamMemberInvite, //
    TeamLeaderCallFollow,//
    ClanInvite,
    ArenaInvite,
    TeamTransmit,   //
    ClanTransmit,   //
    CoupleTransmit, //
    CityWarTeamTransmit,//
    CityWarClanTransmit,//
    TokenTaskReward,    //  悬赏任务推送
}
public class SysMsgPushParam
{
    public TransmitAndInviteType m_type;
    public string iconName = "";
    public string titleName = "";
}
public class SysMsgPushBtn : MonoBehaviour
{
    public UILabel m_lableNum = null;
    public UISprite m_spriteSlider = null;
    public UISprite m_spriteIcon = null;
    public UISprite m_spriteTitle = null;
    public UILabel m_labelTime = null;
    public PushMsg.MsgType m_pushMsgType;
    uint m_pushMsgSenderID = 0;
    private PushMsg m_pushMsg;
    bool CurPushMsgTypeIsInvite = false;
    public const string CONST_TRANSANDINVITE_ICON = "TransmitAndInviteIcon";


    /// <summary>
    /// 倒计时大于99 不显示倒计时时间
    /// </summary>
    public const int SHOWCDLIMIT = 99;
    public float m_l_leftSeconds = 0;
    public float m_l_pushCD = 1.0f;
    void Awake()
    {
        Transform numTrans = transform.Find("Num");
        if (numTrans)
        {
            m_lableNum = numTrans.GetComponent<UILabel>();
        }
        Transform sliderSp = transform.Find("slider");
        if (sliderSp)
        {
            m_spriteSlider = sliderSp.GetComponent<UISprite>();
        }
        Transform timeLabel = transform.Find("time");
        if (timeLabel)
        {
            m_labelTime = timeLabel.GetComponent<UILabel>();
        }
        m_spriteIcon = transform.Find("Icon").GetComponent<UISprite>();
        m_spriteTitle = transform.Find("Icon/icon_label").GetComponent<UISprite>();
        m_spriteIcon.type = UIBasicSprite.Type.Simple;
        UIEventListener.Get(gameObject).onClick = OnClickBtn;
    }

    Dictionary<TransmitAndInviteType, SysMsgPushParam> sysMsgParamDic = new Dictionary<TransmitAndInviteType, SysMsgPushParam>();
    SysMsgPushParam ParseBtnSpite(TransmitAndInviteType type)
    {
        SysMsgPushParam data = null;
        if (sysMsgParamDic.ContainsKey(type))
        {
            data = sysMsgParamDic[type];
        }
        else
        {
            data = new SysMsgPushParam();
            string stringKeys = GameTableManager.Instance.GetGlobalConfig<string>(CONST_TRANSANDINVITE_ICON, type.ToString());
            if (stringKeys != null)
            {
                string[] args = stringKeys.Split('|');
                if (args.Length != 2 || string.IsNullOrEmpty(args[0]))
                {
                    Engine.Utility.Log.Error("全局配置表中的货币图标参数无法解析");
                }
                else
                {
                    data.m_type = type;
                    data.iconName = args[0];
                    data.titleName = args[1];
                }
            }
            sysMsgParamDic.Add(type, data);
        }

        return data;
    }


    #region  Btn
    void OnClickBtn(GameObject go)
    {
        if (m_pushMsgType == PushMsg.MsgType.TeamLeaderInvite)
        {
            OnTeamLeaderInvite();//队长邀请组队
        }
        else if (m_pushMsgType == PushMsg.MsgType.TeamMemberInvite)
        {
            OnTeamMemberInvite();//队员邀请
        }
        else if (m_pushMsgType == PushMsg.MsgType.TeamLeaderCallFollow)
        {
            OnTeamLeaderCallFollow();// 队长召唤跟随
        }
        else if (m_pushMsgType == PushMsg.MsgType.Arena)
        {
            OnArena();
        }
        else if (m_pushMsgType == PushMsg.MsgType.Clan)
        {
            OnClan();
        }
        else if (m_pushMsgType == PushMsg.MsgType.TokenTaskReward)
        {
            OnTokenTaskReward();
        }
        else
        {
            GameCmd.FeiLeiType transmit = GameCmd.FeiLeiType.FeiLeiType_Clan;
            if (m_pushMsgType == PushMsg.MsgType.TeamTransmit)
            {
                transmit = GameCmd.FeiLeiType.FeiLeiType_Team;
            }
            else if (m_pushMsgType == PushMsg.MsgType.ClanTransmit)
            {
                transmit = GameCmd.FeiLeiType.FeiLeiType_Clan;
            }
            else if (m_pushMsgType == PushMsg.MsgType.CoupleTransmit)
            {
                transmit = GameCmd.FeiLeiType.FeiLeiType_Couple;
            }
            else if (m_pushMsgType == PushMsg.MsgType.CityWarClan)
            {
                transmit = GameCmd.FeiLeiType.CallUp_CityWarClan;
            }
            else if (m_pushMsgType == PushMsg.MsgType.CityWarTeam)
            {
                transmit = GameCmd.FeiLeiType.CallUp_CityWarTeam;
            }
            OnFeiLeng(transmit);
        }
    }
    void OnFeiLeng(GameCmd.FeiLeiType transmit)
    {
        PushMsg msg = DataManager.Manager<FunctionPushManager>().GetPushMsg(m_pushMsgType, m_pushMsgSenderID);
        if (msg == null)
        {
            return;
        }
        string des = "";
        string title = "";
        if (transmit == GameCmd.FeiLeiType.FeiLeiType_Team || transmit == GameCmd.FeiLeiType.CallUp_CityWarTeam)
        {
            des = string.Format("队伍成员{0}({1},{2}级)正在召唤你前往{3}({4})，是否前往加入", msg.sendName, msg.profession, msg.level, msg.map, msg.vector);
            title = "队伍召集";
        }
        else
        {
            des = string.Format("氏族成员{0}({1},{2}级)正在召唤你前往{3}({4})，是否前往加入", msg.sendName, msg.profession, msg.level, msg.map, msg.vector);
            title = "氏族召集";
        }
        Action agree = delegate
        {
            if (!KHttpDown.Instance().SceneFileExists(msg.mapId))
            {
                DataManager.Manager<FunctionPushManager>().RemoveTransmitMsg(msg);
                //打开下载界面
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);

                return;
            }

            NetService.Instance.Send(new GameCmd.stInviteGoMapRequestUserCmd_CS() { userid = msg.senderId, type = (uint)transmit });
            DataManager.Manager<FunctionPushManager>().RemoveTransmitMsg(msg);
        };

        Action close = delegate
        {
            DataManager.Manager<FunctionPushManager>().RemoveTransmitMsg(msg);
        };
        Action refuse = delegate
        {

        };
        TipsManager.Instance.ShowTipWindow(0, (uint)msg.leftTime, Client.TipWindowType.CancelOk, des, agree, refuse, close, title: title, okstr: "确定", cancleStr: "挂起");
    }
    //队长邀请组队
    void OnTeamLeaderInvite()
    {
        PushMsg msg = DataManager.Manager<FunctionPushManager>().GetPushMsg(m_pushMsgType, m_pushMsgSenderID);
        if (msg == null)
        {
            return;
        }

        string des = string.Format("{0}邀请你加入{1}的队伍", msg.sendName, msg.name);
        Action agree = delegate
        {
            GameCmd.stAnswerTeamRelationUserCmd_CS sendCmd = new GameCmd.stAnswerTeamRelationUserCmd_CS();
            //sendCmd.dwRequestUserID = DataManager.Manager<TeamDataManager>().ReceiveTeamInviteInfo.uid;
            sendCmd.dwRequestUserID = msg.senderId;
            sendCmd.byAgree = 1;
            NetService.Instance.Send(sendCmd);

            DataManager.Manager<FunctionPushManager>().RemoveAllMsg(m_pushMsgType);
        };

        Action refuse = delegate
        {
            DataManager.Manager<FunctionPushManager>().RemoveFirstSysMsg(msg);

            if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
            {
                return;
            }

            GameCmd.stAnswerTeamRelationUserCmd_CS sendCmd = new GameCmd.stAnswerTeamRelationUserCmd_CS();
            //sendCmd.dwRequestUserID = DataManager.Manager<TeamDataManager>().ReceiveTeamInviteInfo.uid;
            sendCmd.dwRequestUserID = msg.senderId;
            sendCmd.byAgree = 0;
            NetService.Instance.Send(sendCmd);
        };
        TipsManager.Instance.ShowTipWindow(0, (uint)msg.leftTime, Client.TipWindowType.CancelOk, des, agree, refuse, title: "组队邀请");
    }

    //队员邀请
    void OnTeamMemberInvite()
    {
        PushMsg msg = DataManager.Manager<FunctionPushManager>().GetPushMsg(m_pushMsgType, m_pushMsgSenderID);
        if (msg == null)
        {
            return;
        }

        string des = string.Format("{0}邀请你加入{1}的队伍", msg.sendName, msg.name);
        Action agree = delegate
        {
            GameCmd.stTeamMemInviteRelationUserCmd_CS sendCmd = new GameCmd.stTeamMemInviteRelationUserCmd_CS();
            sendCmd.teamid = msg.senderId;
            sendCmd.ret = true;
            NetService.Instance.Send(sendCmd);
            DataManager.Manager<FunctionPushManager>().RemoveAllMsg(m_pushMsgType);
        };

        Action refuse = delegate
        {
            DataManager.Manager<FunctionPushManager>().RemoveFirstSysMsg(msg);

            if (DataManager.Manager<TeamDataManager>().IsJoinTeam)
            {
                return;
            }

            GameCmd.stTeamMemInviteRelationUserCmd_CS sendCmd = new GameCmd.stTeamMemInviteRelationUserCmd_CS();
            sendCmd.teamid = msg.senderId;
            sendCmd.ret = false;
            NetService.Instance.Send(sendCmd);
        };

        TipsManager.Instance.ShowTipWindow(0, (uint)msg.leftTime, Client.TipWindowType.CancelOk, des, agree, refuse, title: "组队邀请");

    }

    /// <summary>
    /// 队长召唤跟随
    /// </summary>
    void OnTeamLeaderCallFollow()
    {
        PushMsg msg = DataManager.Manager<FunctionPushManager>().GetPushMsg(m_pushMsgType, m_pushMsgSenderID);
        if (msg == null)
        {
            return;
        }

        string des = string.Format("队长向您发出跟随请求");
        Action agree = delegate
        {
            DataManager.Manager<TeamDataManager>().ReqManualFollow();
            DataManager.Manager<FunctionPushManager>().RemoveAllMsg(m_pushMsgType);
        };

        Action refuse = delegate
        {
            DataManager.Manager<FunctionPushManager>().RemoveFirstSysMsg(msg);
        };

        TipsManager.Instance.ShowTipWindow(0, (uint)msg.leftTime, Client.TipWindowType.CancelOk, des, agree, refuse, title: "跟随请求");

    }

    void OnClan()
    {
        PushMsg msg = DataManager.Manager<FunctionPushManager>().GetPushMsg(m_pushMsgType, m_pushMsgSenderID);
        if (msg == null)
        {
            return;
        }
        string content = string.Format("{0}邀请你加入氏族{1}",
        ColorManager.GetColorString(ColorType.JZRY_Blue, msg.sendName),
        ColorManager.GetColorString(ColorType.JZRY_Oranger, msg.name));
        TipsManager.Instance.ShowTipWindow(0, (uint)msg.leftTime, Client.TipWindowType.CancelOk, content,
            () =>
            {
                DataManager.Manager<ClanManger>().AnswerInvite(true, msg.senderId, msg.groupId);
                DataManager.Manager<FunctionPushManager>().RemoveAllMsg(m_pushMsgType);
            },
            () =>
            {
                DataManager.Manager<FunctionPushManager>().RemoveFirstSysMsg(msg);

                if (DataManager.Manager<ClanManger>().IsJoinClan)
                {
                    return;
                }

                DataManager.Manager<ClanManger>().AnswerInvite(false, msg.senderId, msg.groupId);

            });
    }

    void OnArena()
    {
        PushMsg msg = DataManager.Manager<FunctionPushManager>().GetPushMsg(m_pushMsgType, m_pushMsgSenderID);
        if (msg == null)
        {
            return;
        }
        string des = msg.sendName + "向你发起武斗场邀请\n是否参战";

        Action agree = delegate
        {
            MainPlayStop();

            GameCmd.stInviteResultArenaUserCmd_CS agreeCmd = new GameCmd.stInviteResultArenaUserCmd_CS();
            agreeCmd.offensive_id = msg.senderId;
            agreeCmd.result = 1;        //同意
            Client.ClientGlobal.Instance().netService.Send(agreeCmd);
            DataManager.Manager<FunctionPushManager>().RemoveAllMsg(m_pushMsgType);
        };

        Action refuse = delegate
        {
            GameCmd.stInviteResultArenaUserCmd_CS refuseCmd = new GameCmd.stInviteResultArenaUserCmd_CS();
            refuseCmd.offensive_id = msg.senderId;
            refuseCmd.result = 0;       //拒绝
            Client.ClientGlobal.Instance().netService.Send(refuseCmd);
            DataManager.Manager<FunctionPushManager>().RemoveFirstSysMsg(msg);
        };
        TipsManager.Instance.ShowTipWindow(0, (uint)msg.leftTime, Client.TipWindowType.CancelOk, des, agree, refuse, title: "挑战邀请");
    }

    /// <summary>
    /// 悬赏任务完成奖励
    /// </summary>
    void OnTokenTaskReward()
    {
        PushMsg msg = DataManager.Manager<FunctionPushManager>().GetPushMsg(m_pushMsgType, m_pushMsgSenderID);
        if (msg == null)
        {
            return;
        }

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RewardMissionPanel);

        DataManager.Manager<FunctionPushManager>().RemoveFirstSysMsg(msg);
    }

    void MainPlayStop()
    {
        Client.IPlayer player = Client.ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);//关闭自动寻路中
        }
        Controller.CmdManager.Instance().Clear();//清除寻路
    }
    #endregion


    public void SetSysMsgUI(PushMsg.MsgType msgType)
    {
        m_pushMsgType = msgType;

        if (m_lableNum != null)
        {
            int num = DataManager.Manager<FunctionPushManager>().GetMsgNum(m_pushMsgType);
            if (num > 0)
            {
                m_lableNum.text = num.ToString();
            }
            else
            {
                m_lableNum.text = "";
            }
        }
        PushMsg msg = DataManager.Manager<FunctionPushManager>().GetPushMsg(m_pushMsgType);
        if (msg == null)
        {
            Engine.Utility.Log.Error("pushmsg == null");
            return;
        }
        m_pushMsg = msg;

        m_l_leftSeconds = msg.cd - (UnityEngine.Time.realtimeSinceStartup - msg.sendTime);
        m_l_pushCD = msg.cd;
        CurPushMsgTypeIsInvite = true;
        if (m_l_leftSeconds >= SHOWCDLIMIT)
        {
            ShowCDUI(false);
        }
        else
        {
            ShowCDUI(true);
        }

        if (m_spriteSlider != null)
        {
            m_spriteSlider.fillAmount = msg.leftTime / msg.cd;
        }
        if (m_labelTime != null)
        {
            m_labelTime.text = ((uint)msg.leftTime + 1).ToString();
        }

        if (m_spriteIcon != null)
        {
            SysMsgPushParam param = null;
            if (msg.msgType == PushMsg.MsgType.TeamLeaderInvite)
            {
                param = ParseBtnSpite(TransmitAndInviteType.TeamInvite);

            }
            else if (msg.msgType == PushMsg.MsgType.TeamMemberInvite)
            {
                param = ParseBtnSpite(TransmitAndInviteType.TeamMemberInvite);
            }
            else if (msg.msgType == PushMsg.MsgType.Arena)
            {
                param = ParseBtnSpite(TransmitAndInviteType.ArenaInvite);
            }
            else if (msg.msgType == PushMsg.MsgType.Clan)
            {
                param = ParseBtnSpite(TransmitAndInviteType.ClanInvite);
            }
            else if (msg.msgType == PushMsg.MsgType.TeamLeaderCallFollow)
            {
                param = ParseBtnSpite(TransmitAndInviteType.TeamLeaderCallFollow);
            }
            else if (msg.msgType == PushMsg.MsgType.TokenTaskReward)
            {
                param = ParseBtnSpite(TransmitAndInviteType.TokenTaskReward);
            }
            if (param != null)
            {
                m_spriteIcon.spriteName = param.iconName;
                m_spriteTitle.spriteName = param.titleName;
            }

        }
    }

    public void SetTransmitUI(PushMsg msg)
    {
        m_pushMsgType = msg.msgType;
        m_pushMsgSenderID = msg.senderId;
        if (m_lableNum != null)
        {
            //这个地方num肯定为0
            int num = DataManager.Manager<FunctionPushManager>().GetMsgNum(m_pushMsgType);
            if (num > 0)
            {
                m_lableNum.text = num.ToString();
            }
            else
            {
                m_lableNum.text = "";
            }
        }
        m_pushMsg = msg;
        m_l_leftSeconds = msg.cd - (UnityEngine.Time.realtimeSinceStartup - msg.sendTime);
        m_l_pushCD = msg.cd;
        CurPushMsgTypeIsInvite = false;
        if (m_spriteIcon != null)
        {
            SysMsgPushParam param = null;
            if (m_pushMsgType == PushMsg.MsgType.TeamTransmit)
            {
                param = ParseBtnSpite(TransmitAndInviteType.TeamTransmit);
            }
            else if (m_pushMsgType == PushMsg.MsgType.ClanTransmit)
            {
                param = ParseBtnSpite(TransmitAndInviteType.ClanTransmit);

            }
            else if (m_pushMsgType == PushMsg.MsgType.CoupleTransmit)
            {
                param = ParseBtnSpite(TransmitAndInviteType.CoupleTransmit);
            }
            else if (m_pushMsgType == PushMsg.MsgType.CityWarTeam)
            {
                param = ParseBtnSpite(TransmitAndInviteType.CityWarTeamTransmit);
            }
            else if (m_pushMsgType == PushMsg.MsgType.CityWarClan)
            {
                param = ParseBtnSpite(TransmitAndInviteType.CityWarClanTransmit);
            }
            if (param != null)
            {
                m_spriteIcon.spriteName = param.iconName;
                m_spriteTitle.spriteName = param.titleName;
            }

        }
    }

    public void ShowCDUI(bool b)
    {
        if (m_spriteSlider != null && m_labelTime != null)
        {
            if (b)
            {
                m_spriteSlider.gameObject.SetActive(true);
                m_labelTime.gameObject.SetActive(true);
            }
            else
            {
                m_spriteSlider.gameObject.SetActive(false);
                m_labelTime.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (m_l_leftSeconds > 0)
        {
            m_l_leftSeconds -= Time.deltaTime;
            if (m_spriteSlider != null)
            {
                m_spriteSlider.fillAmount = m_l_leftSeconds / m_l_pushCD;
            }
            if (m_labelTime != null)
            {
                m_labelTime.text = ((uint)m_l_leftSeconds + 1).ToString();
            }
            if (m_l_leftSeconds <= 0)
            {
                if (m_pushMsg != null)
                {
                    if (CurPushMsgTypeIsInvite)
                    {
                        DataManager.Manager<FunctionPushManager>().RemoveFirstSysMsg(m_pushMsg);
                    }
                    else
                    {
                        DataManager.Manager<FunctionPushManager>().RemoveTransmitMsg(m_pushMsg);
                    }
                    m_pushMsg = null;
                }
            }
        }
    }
}

partial class MessagePushPanel : UIPanelBase
{
    FunctionPushManager dataMgr = DataManager.Manager<FunctionPushManager>();
    private List<SysMsgPushBtn> m_lstSysMsgPushBtn = new List<SysMsgPushBtn>();
    table.Trailerdatabase m_currTrailerdata = null;
    //UIGridCreatorBase dailyPushCreator = null;

    int DailyOpenLevel = 0;

    #region interface
    protected override void OnLoading()
    {
        base.OnLoading();
        m_trans_MessagePushContent.gameObject.SetActive(false);
        m_btn_FunctionPushContent.gameObject.SetActive(false);


        DailyOpenLevel = GameTableManager.Instance.GetGlobalConfig<int>("Daily_OpenLevel");
    }
    CMResAsynSeedData<CMAtlas> m_playerAvataCASD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        //         if (m_ctor_DailyPushContent != null)
        //         {
        //             m_ctor_DailyPushContent.Release(depthRelease);
        //         }
        if (m_playerAvataCASD != null)
        {
            m_playerAvataCASD.Release(depthRelease);
            m_playerAvataCASD = null;
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterEvent(true);
        InitDailyPushRoot();
        FunctionPushManager dataMgr = DataManager.Manager<FunctionPushManager>();

        AddSysTip();
        RefreshFunction();   
        RefreshDailyPushFunction();
    }
    private bool isDailyPushCreatorInit = false;
    void InitDailyPushRoot()
    {
        if (null != m_ctor_DailyPushContent && !isDailyPushCreatorInit)
        {
            isDailyPushCreatorInit = true;
            m_ctor_DailyPushContent.Initialize<UIDailyPushGrid>(m_trans_UIDailyPushGrid.gameObject, OnUpdateDailyPushGrid, OnDailyGridUIEvent);
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        Release();
        RegisterEvent(false);
    }

    void RegisterEvent(bool register)
    {

        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.REFRESHFUNCTIONPUSHOPEN, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.REFRESHINVITEPUSHMSGSTATUS, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.REFRESHDAILYPUSHSTATUS, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DONE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.REFRESHFUNCTIONPUSHOPEN, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.REFRESHINVITEPUSHMSGSTATUS, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.REFRESHDAILYPUSHSTATUS, OnEvent);
        }
    }



    #endregion

    private void RefreshFunction()
    {
        bool showFunctionContent = false;
        if (dailyList != null)
        {          
            if (dailyList.Count >= 2)
            {
                showFunctionContent = false;              
            }
            else 
            {
                int level = MainPlayerHelper.GetPlayerLevel();
                List<table.Trailerdatabase> lstdata = dataMgr.GetDataList();
                table.Trailerdatabase trailerdata = null;
                for (int i = 0; i < lstdata.Count; i++)
                {
                    trailerdata = lstdata[i];
                    if (dataMgr.IsOpened(trailerdata.dwId) == false)
                    {
                        if (trailerdata.showLevel > level)
                        {
                            continue;
                        }
                        showFunctionContent = true;
                        //m_btn_FunctionPushContent.gameObject.SetActive(true);
                        m_currTrailerdata = trailerdata;
                        if (m_currTrailerdata.type == 1)
                        {
                            SetType1UI(trailerdata, dataMgr.CanOpen(trailerdata));
                        }
                        else if (m_currTrailerdata.type == 2)
                        {
                            SetType2UI(trailerdata, dataMgr.CanOpen(trailerdata));
                        }
                        break;
                    }
                }
                if (dataMgr.IsAllOpened())
                {
                    showFunctionContent = false;             
                    //m_btn_FunctionPushContent.gameObject.SetActive(false);
                }
            }

        }
        m_btn_FunctionPushContent.gameObject.SetActive(showFunctionContent);
    }
    List<DailyPushData> dailyList = new List<DailyPushData>();
    private void RefreshDailyPushFunction()
    {
        dailyList.Clear();

        dataMgr.StructDailyPushMsg();
        List<DailyPushData> list = dataMgr.DailyPushList;
        int curPlayerLv = MainPlayerHelper.GetPlayerLevel();
        for (int i = 0; i < list.Count; i++)
        {
            if (!(curPlayerLv >= DailyOpenLevel))
            {
                break;
            }
            if (list[i].curState != DailyPushMsgState.Closed)
            {
                dailyList.Add(list[i]);
            }
        }
        int matchCount = ((dailyList.Count >= 2) ? 2 : dailyList.Count);

        if (m_ctor_DailyPushContent != null)
        {
            m_ctor_DailyPushContent.CreateGrids(matchCount);
        }

        if (matchCount >= 2)
        {
            m_btn_FunctionPushContent.gameObject.SetActive(false);
        }
//         else
//         {
//             RefreshFunction();
//         }
    }
    void OnUpdateDailyPushGrid(UIGridBase data, int index)
    {
        if (data is UIDailyPushGrid)
        {
            UIDailyPushGrid grid = data as UIDailyPushGrid;
            grid.SetGridData(dailyList[index]);
        }
    }
    private void OnDailyGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIDailyPushGrid)
                {
                    UIDailyPushGrid grid = data as UIDailyPushGrid;
                    DailyDataType curType = DataManager.Manager<DailyManager>().GetDailyTypeByID(grid.dailyID);
                    UIPanelBase.PanelJumpData jumpData = new PanelJumpData();
                    jumpData.Tabs = new int[2];
                    jumpData.Tabs[0] = 1;
                    jumpData.Tabs[1] = (int)curType;
                    jumpData.Param = grid.dailyID;
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DailyPanel, jumpData: jumpData);
                }
                break;
        }
    }

    private void SetType1UI(table.Trailerdatabase data, bool isOpen)
    {
        m_trans_Type1.gameObject.SetActive(true);
        m_trans_Type2.gameObject.SetActive(false);
        if (data == null)
        {
            return;
        }

        UISprite spriteIcon = m_trans_Type1.Find("Icon").GetComponent<UISprite>();
        if (spriteIcon != null)
        {
            spriteIcon.spriteName = data.strIcon;
            //策划要求不适用完美比例
            //spriteIcon.MakePixelPerfect();
        }
        //m_trans_Type1.Find("effect").gameObject.SetActive(isOpen);
        PlayEffect(m_trans_Type1.Find("effect"), isOpen);
        UILabel labelDesc = m_trans_Type1.Find("desc").GetComponent<UILabel>();
        if (labelDesc != null)
        {
            if (isOpen)//可以领奖
            {
                labelDesc.text = string.Format(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_Txt_Trailer_3),
                    DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_Txt_Trailer_4));
            }
            else
            {

                //labelDesc.color = new Color(62 / 255.0f, 71 / 255.0f, 90 / 255.0f);
                int level = MainPlayerHelper.GetPlayerLevel();
                if (level < data.level)
                {
                    labelDesc.text = string.Format(DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_FM_Trailer_Level, data.level));
                }
                else if (data.task != 0)
                {
                    if (DataManager.Manager<TaskDataManager>().CheckTaskFinished(data.task) == false)
                    {
                        table.QuestDataBase quest = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(data.task);
                        if (quest != null)
                        {
                            labelDesc.text = string.Format(DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Local_TXT_FM_Trailer_Task, quest.strName));
                        }
                    }
                }
            }
        }
    }
    CMResAsynSeedData<CMTexture> m_currencyAsynSeed = null;
    private void SetType2UI(table.Trailerdatabase data, bool isOpen)
    {
        m_trans_Type1.gameObject.SetActive(false);
        m_trans_Type2.gameObject.SetActive(true);
        if (data == null)
        {
            return;
        }

        UILabel labelTitle = m_trans_Type2.Find("title").GetComponent<UILabel>();
        if (labelTitle != null)
        {
            labelTitle.text = data.strTitle;
        }
        UILabel labelDesc = m_trans_Type2.Find("desc").GetComponent<UILabel>();
        if (labelDesc != null)
        {
            if (isOpen)
            {
                labelDesc.effectStyle = UILabel.Effect.Outline;
                //labelDesc.effectColor = new Color(8 / 255.0f, 28 / 255.0f, 8 / 255.0f);
                labelDesc.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_Txt_Trailer_4);
                //labelDesc.color = new Color(48 / 255.0f, 241 / 255.0f, 48 / 255.0f);
            }
            else
            {
                labelDesc.effectStyle = UILabel.Effect.Outline;
                //labelDesc.effectColor = new Color(44 / 255.0f, 24 / 255.0f, 4 / 255.0f);
                labelDesc.text = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_Txt_Trailer_5);
                //labelDesc.color = new Color(255 / 255.0f, 245 / 255.0f, 87 / 255.0f);
            }
        }
        //m_trans_Type2.Find("effect").gameObject.SetActive(isOpen);
        PlayEffect(m_trans_Type2.Find("effect"), isOpen);
        UITexture icon = m_trans_Type2.Find("Icon").GetComponent<UITexture>();
        if (icon != null)
        {
            if (string.IsNullOrEmpty(data.strJobItem) == false)
            {
                string[] strJobItem = data.strJobItem.Split(';');
                int job = MainPlayerHelper.GetMainPlayerJob();
                if (strJobItem.Length >= job && job > 0)
                {
                    string[] strItem = strJobItem[job - 1].Split('_');
                    if (strItem.Length == 2)
                    {
                        uint itemId = uint.Parse(strItem[0]);

                        table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemId);
                        if (itemdb != null)
                        {
                            UIManager.GetTextureAsyn(itemdb.itemIcon,
                                ref m_currencyAsynSeed, () =>
                            {
                                if (null != icon)
                                {
                                    icon.mainTexture = null;
                                }
                            }, icon, false);
                        }
                    }
                }
            }
            else if (string.IsNullOrEmpty(data.strItem) == false)
            {
                string[] strItem = data.strItem.Split('_');
                if (strItem.Length == 2)
                {
                    uint itemId = uint.Parse(strItem[0]);
                    table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemId);
                    if (itemdb != null)
                    {
                        UIManager.GetTextureAsyn(itemdb.itemIcon,
                               ref m_currencyAsynSeed, () =>
                               {
                                   if (null != icon)
                                   {
                                       icon.mainTexture = null;
                                   }
                               }, icon, false);
                    }
                }
            }
        }
    }

    void PlayEffect(Transform trans, bool show)
    {
        if (trans != null)
        {
            UIParticleWidget p = trans.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = trans.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
            if (p != null)
            {

                p.SetDimensions(1, 1);
                p.ReleaseParticle();
                if (show)
                {
                    p.AddParticle(50038);
                }
            }
        }
    }
    SysMsgPushBtn GetSysMsgPushBtn(int index)
    {
        SysMsgPushBtn msgBtn = null;

        Transform child = m_trans_BtnRoot.Find("Btn_" + (index + 1).ToString());
        if (child != null)
        {
            msgBtn = child.gameObject.AddComponent<SysMsgPushBtn>();
        }
        UIParticleWidget wight = msgBtn.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = msgBtn.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 0;
        }

        //只出现了一个特效  如果要改成全都加特效  就不要判断effect
        if (wight != null)
        {
            UIParticleWidget p = wight.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = wight.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 0;
            }
            if (p != null)
            {

                p.SetDimensions(1, 1);
                p.ReleaseParticle();
                p.AddParticle(50043);
            }
        }
        return msgBtn;
    }
    //邀请类
    private void AddSysTip()
    {
        if (!m_trans_MessagePushContent.gameObject.activeSelf)
        {
            m_trans_MessagePushContent.gameObject.SetActive(true);
        }
        SysMsgPushBtn msgBtn = null;
        FunctionPushManager dataMgr = DataManager.Manager<FunctionPushManager>();
        if (dataMgr.M_dicInviteMsg.Count > 0)
        {
            Dictionary<PushMsg.MsgType, List<PushMsg>>.Enumerator iter = dataMgr.M_dicInviteMsg.GetEnumerator();
            int i = 0;
            while (iter.MoveNext())
            {
                if (DataManager.Manager<FunctionPushManager>().IsPushMessageRightPos(iter.Current.Key))
                {
                    if (i >= m_lstSysMsgPushBtn.Count)
                    {
                        msgBtn = GetSysMsgPushBtn(i);
                        if (msgBtn != null)
                        {
                            m_lstSysMsgPushBtn.Add(msgBtn);
                        }
                    }
                    else
                    {
                        msgBtn = m_lstSysMsgPushBtn[i];
                    }
                    if (msgBtn.m_l_leftSeconds < 0)
                    {
                        break;
                    }
                    msgBtn.gameObject.SetActive(true);
                    msgBtn.transform.localPosition = new Vector3(90 - 67 * i, 0, 0);
                    msgBtn.SetSysMsgUI(iter.Current.Key);
                    i++;
                }
            }
            for (int k = i; k < m_lstSysMsgPushBtn.Count; k++)
            {
                m_lstSysMsgPushBtn[k].gameObject.SetActive(false);
            }
        }
        else
        {
            m_trans_MessagePushContent.gameObject.SetActive(false);
            RefreshFunction();
        }

    }

    void OnEvent(int nEventId, object param)
    {
        if (nEventId == (int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE)
        {
            Client.stPropUpdate prop = (Client.stPropUpdate)param;
            if (Client.ClientGlobal.Instance().IsMainPlayer(prop.uid))
            {
                if (prop.nPropIndex == (int)Client.CreatureProp.Level)
                {
                    RefreshFunction();
                }
            }
        }
        else if (nEventId == (int)Client.GameEventID.TASK_DONE)
        {
            Client.stTaskDone td = (Client.stTaskDone)param;
            if (m_currTrailerdata != null && m_currTrailerdata.task == td.taskid)
            {
                RefreshFunction();
            }
        }
        else if (nEventId == (int)Client.GameEventID.REFRESHFUNCTIONPUSHOPEN)
        {
            RefreshFunction();
        }
        else if (nEventId == (int)Client.GameEventID.REFRESHINVITEPUSHMSGSTATUS)
        {
            AddSysTip();
        }
        else if (nEventId == (int)Client.GameEventID.REFRESHDAILYPUSHSTATUS)
        {
            dataMgr.StructDailyPushMsg();
            RefreshDailyPushFunction();
        }
    }

    void onClick_FunctionPushContent_Btn(GameObject caster)
    {
        if (m_currTrailerdata != null)
        {
            if (m_currTrailerdata.type == 1)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.Announce1Panel, data: m_currTrailerdata);
            }
            else if (m_currTrailerdata.type == 2)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.Announce2Panel, data: m_currTrailerdata);
            }
        }
    }
}
