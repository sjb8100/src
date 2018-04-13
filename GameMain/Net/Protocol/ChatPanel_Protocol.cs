using System;
using System.Collections.Generic;
using Common;
using GameCmd;
using Engine.Utility;

public partial class Protocol
{

    #region 聊天消息
    //私聊
    [Execute]
    public void Excute(stCommonMChatUserCmd_CS cmd)
    {
        if (cmd.byChatType == GameCmd.CHATTYPE.CHAT_SILENT)
        {
            DataManager.Manager<ChatDataManager>().PrivateChatManager.AddChat(cmd);
            return;
        }
    }
    [Execute]
    public void Excute(stOnlineChatUserCmd_S cmd)
    {
        stCommonMChatUserCmd_CS cmdchat = new stCommonMChatUserCmd_CS();
        cmdchat.dwOPDes = cmd.dwOPDes;
        cmdchat.profession = (GameCmd.enumProfession)cmd.profession;
        cmdchat.sex = (GameCmd.enmCharSex)cmd.sex;
        cmdchat.timestamp = cmd.timestamp;
        cmdchat.byChatType = CHATTYPE.CHAT_SILENT;
        cmdchat.szOPDes = cmd.szOPDes;
        for (int i = 0; i < cmd.szInfo.Count; ++i )
        {
            cmdchat.szInfo = cmd.szInfo[i];
            DataManager.Manager<ChatDataManager>().PrivateChatManager.AddChat(cmdchat);
        }
    }
    /// <summary>
    /// 普通聊天
    /// </summary>
    /// <param name="cmd"></param>
    /// <returns></returns>
    [Execute]
    public void ExecuteChatCmd(GameCmd.stWildChannelCommonChatUserCmd_CS cmd)
    {
        if (!ChatDataManager.CanRecieveChatMsgWithBlack(cmd.dwOPDes))
        {
            return;
        }
        if (cmd.byChatType == GameCmd.CHATTYPE.CHAT_SYS)
        {
            if ((cmd.byChatPos & (uint)GameCmd.ChatPos.ChatPos_Tips) == (uint)GameCmd.ChatPos.ChatPos_Tips)
            {
                TipsManager.Instance.ShowTips(cmd.szInfo);
            }

            if ((cmd.byChatPos & (uint)GameCmd.ChatPos.ChatPos_Sys_chat) == (uint)GameCmd.ChatPos.ChatPos_Sys_chat)
            {
                DataManager.Manager<ChatDataManager>().PrivateChatManager.AddChat(new GameCmd.stCommonMChatUserCmd_CS()
                {
                    szInfo = cmd.szInfo,
                    byChatType = CHATTYPE.CHAT_SYS,
                    dwOPDes = 0,
                    szOPDes = "系统",
                });
                return;      
            }
        }

        ChatChannel channel = DataManager.Manager<ChatDataManager>().GetChannelByType(cmd.byChatType);
        if (channel != null)
        {
            channel.Add(channel.ToChatInfo(cmd));
        }

        if ((GameCmd.ChatPos)cmd.byChatPos == GameCmd.ChatPos.ChatPos_Gm || 
            (GameCmd.ChatPos)cmd.byChatPos == GameCmd.ChatPos.ChatPos_Important)
        {
            ShowRunlight(cmd.szInfo, RunLightInfo.Pos.Top);

        }
        //下走马灯 不显示
        if ((GameCmd.ChatPos)cmd.byChatPos == GameCmd.ChatPos.ChatPos_Sys &&
            DataManager.Manager<ChatDataManager>().SimpleChannelContain(CHATTYPE.CHAT_SYS))
	    {
            ShowRunlight(cmd.szInfo, RunLightInfo.Pos.Bottom);		 
	    }
    }

    private void ShowRunlight(string msg,RunLightInfo.Pos rpos,string username = "")
    {
        // 普通文本转换成富文本
        var yellow = Cmd.GXColor.Yellow;
        var colorYellow = string.Format("#{0:X8}", (uint)yellow);
        var text = ChatChannel.FormatContent(msg);
        
        if (string.IsNullOrEmpty(username) == false)
        {
            text = string.Format("<color value=\"{0}\">[{1}]</color>{2}", colorYellow, username, text);
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RunLightPanel, data: new RunLightInfo()
        {
            pos = rpos,
            msg = text,
            showTime = 1,
        });
    }

    [Execute]
    public void Excute(Pmd.stMessageBoxChatUserPmd_S cmd)
    {
        TipsManager.Instance.ShowTips(cmd.szInfo);
    }

    /// <summary>
    /// 世界聊天
    /// </summary>
    /// <param name="cmd"></param>
//     [Execute]
//     public void Excute(GameCmd.stWildChannelCommonChatUserCmd_CS cmd)
//     {
//         ChatChannel channel = DataManager.Manager<ChatDataManager>().GetChannelByType((uint)cmd.byChatType);
//         if (channel != null)
//         {
//             channel.Add(channel.ToChatInfo(cmd));
//         }
//         if ((GameCmd.ChatPos)cmd.byChatPos == GameCmd.ChatPos.ChatPos_Gm ||
//             (GameCmd.ChatPos)cmd.byChatPos == GameCmd.ChatPos.ChatPos_Important)
//         {
//             ShowRunlight(cmd.szInfo, RunLightInfo.Pos.Top);
//         }
//     }


    [Execute]//道具连接信息返回 stRequestUrlItemInfoChatUserCmd
    public void Excute(GameCmd.stReturnUrlItemInfoChatUserCmd_S cmd)
    {
        table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(cmd.obj.dwObjectID);
        if (itemdb != null)
        {
            if (cmd.special)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RidePetTipsPanel, data: cmd.obj);
            }
            else
            {
                BaseItem baseItem = new BaseItem(cmd.obj.dwObjectID, cmd.obj);
                //cmd.obj
                TipsManager.Instance.ShowItemTips(baseItem);
            }
        }
        
    }


    [Execute]//喇叭默认走马灯
    public void Excute(GameCmd.stSpeakerChatUserCmd_CS cmd)
    {
        if (!ChatDataManager.CanRecieveChatMsgWithBlack(cmd.dwOPDes))
        {
            return;
        }
        ShowRunlight(cmd.szInfo, RunLightInfo.Pos.Top, cmd.username);

        ChatChannel channel = DataManager.Manager<ChatDataManager>().GetChannelByType(CHATTYPE.CHAT_WORLD);
        if (channel != null)
        {
            channel.Add(channel.ToChatInfo(cmd));
        }
    }

    #endregion

    /// <summary>
    ///私聊
    /// </summary>
    /// <param name="cmd"></param>
//     [Execute]
//     public void Excute(GameCmd.stWildChannelCommonChatUserCmd_CS cmd)
//     {
//         DataManager.Manager<ChatDataManager>().PrivateChatManager.AddChat(cmd);
//     }

    [Execute]
    public void Excute(GameCmd.stSendMessageWildChannelCostChatUserCmd_S cmd)
    {
        TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Talk_World_shijiepindaofayanxiaohaojinbitishi, cmd.cost);
    }
    /// <summary>
    /// 请求玩家操作
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="vType"></param>
    public void RequestPlayerInfoForOprate(uint uid,PlayerOpreatePanel.ViewType vType)
    {
        //TODO 请求队伍，等级数据后显示面板
        if (Client.ClientGlobal.Instance().IsMainPlayer(uid))
        {
            return;
        }
        NetService.Instance.Send(new GameCmd.stSendUserMessageChatUserCmd_CS() { dwOPDes = uid,byChatPos = (uint)vType });
    }

    [Execute]
    public void Excute(GameCmd.stSendUserMessageChatUserCmd_CS cmd)
    {
        PlayerOpreatePanel.PlayerViewInfo data = new PlayerOpreatePanel.PlayerViewInfo();
        data.uid = cmd.dwOPDes;
        data.name = cmd.name;
        data.teamNum = cmd.teamnum;
        data.teamID = cmd.teamid;
        data.level = cmd.level;
        data.job = (uint)cmd.profession;
        data.clanid = cmd.clanid;
        data.viewType = (PlayerOpreatePanel.ViewType)cmd.byChatPos;
        PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns btns = 0;
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.SendTxt;
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.ViewMsg;
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.AddFriend;
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Visit;
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Shield;

        if (data.teamID == 0)
        {
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Invite;
        }
        else
        {
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Apply;
        }

        if (data.viewType == PlayerOpreatePanel.ViewType.AddRemove_Contact)
        {
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Shield;
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Remove;
            data.RelationToRemove = GameCmd.RelationType.Relation_Contact;
        }
        else if (data.viewType == PlayerOpreatePanel.ViewType.AddRemove_Interact)
        {
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Remove;
            data.RelationToRemove = GameCmd.RelationType.Relation_Interactive;
        }
        else if (data.viewType == PlayerOpreatePanel.ViewType.AddRemove_Enemy)
        {
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Remove;
            data.RelationToRemove = GameCmd.RelationType.Relation_Enemy;
        }
        else if (data.viewType == PlayerOpreatePanel.ViewType.AddRemove_Shield)
        {
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Remove;
            data.RelationToRemove = GameCmd.RelationType.Relation_Black;
        }
        else if (data.viewType == PlayerOpreatePanel.ViewType.Clan)
        {
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.Expel;
            btns |= PlayerOpreatePanel.PlayerViewInfo.PlayerViewBtns.ChangeDuty;
        }

        data.playerViewMask = (int)btns;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PlayerOpreatePanel, data: data);
    }


    [Common.Execute]
    public void OnVoiceChatOperChat(GameCmd.stVoiceChatOperChatUserCmd_CS cmd)
    {
        if (cmd.optype == GameCmd.VoChatOpType.VoChatOpType_Add)
        {
            GVoiceManger.Instance.AddRole((int)cmd.posid, cmd.posname);
        }
        else if (cmd.optype == GameCmd.VoChatOpType.VoChatOpType_Leave)
        {
            GVoiceManger.Instance.RemoveRole((int)cmd.posid);
        }
    }

    [Common.Execute]
    public void OnSetVoiceChatMode(GameCmd.stSetVoiceChatModeChatUserCmd_CS cmd)
    {
        if (cmd.chtype == GameCmd.VoChatType.VoChatType_Clan)
        {
            DataManager.Manager<ChatDataManager>().ClanChatMode = cmd.mode;
            if (cmd.mode == GameCmd.VoChatMode.VoChatMode_Freedom)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_SETMICBTN, true);
            }
            else if (cmd.mode == GameCmd.VoChatMode.VoChatMode_Leader)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_SETMICBTN, false);
            }
        }
    }

    [Common.Execute]
    public void OnVoiceChatMemberList(GameCmd.stVoiceChatMemberListChatUserCmd_S cmd)
    {
        GVoiceManger.Instance.ClearRole();
        for (int i = 0; i < cmd.info.Count; i++)
        {
            GameCmd.VoChatInfo info = cmd.info[i];

            if (info.chtype == GameCmd.VoChatType.VoChatType_Team && GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Team)
            {
                for (int n = 0; n < info.data.Count; n++)
                {
                    GVoiceManger.Instance.AddRole((int)info.data[n].posid, info.data[n].username);
                }
            }
            else if (info.chtype == GameCmd.VoChatType.VoChatType_Clan && GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation)
            {
                int total = GameTableManager.Instance.GetGlobalConfig<int>("VochatClanCallCD") * 60;
                long leftTime = total - (DateTimeHelper.Instance.Now - info.calltime);
                if (leftTime <= 0)
                {
                    leftTime = 0;
                }
                DataManager.Manager<ChatDataManager>().ClanChatMode = info.mode;
                DataManager.Manager<ChatDataManager>().ClanCallLeftTime = leftTime;
                for (int n = 0; n < info.data.Count; n++)
                {
                    GVoiceManger.Instance.AddRole((int)info.data[n].posid, info.data[n].username);
                }
            }
        }

        if (GVoiceManger.Instance.JoinRoomType == JoinRoomEnum.Nation) 
        {
            VoChatMode mode = DataManager.Manager<ChatDataManager>().ClanChatMode;
            if (mode == GameCmd.VoChatMode.VoChatMode_Freedom || mode == GameCmd.VoChatMode.VoChatMode_None)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_SETMICBTN, true);
            }
            else if (mode == GameCmd.VoChatMode.VoChatMode_Leader)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CHAT_SETMICBTN, false);
            }
        }
    }
     [Common.Execute]
    public void OnClanCallVoiceChatMember(GameCmd.stCallVoiceChatMemberChatUserCmd_CS cmd)
    {

        if (cmd.chtype == GameCmd.VoChatType.VoChatType_Clan)
        {
            DataManager.Manager<ChatDataManager>().ClanCallLeftTime = GameTableManager.Instance.GetGlobalConfig<int>("VochatClanCallCD") * 60;
            //int time = GameTableManager.Instance.GetGlobalConfig<int>("CF_EnterSceneLeftTime");
            if (GVoiceManger.Instance.JoinRoomType != JoinRoomEnum.Nation)
            {
                string msg = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Talk_ActualTime_shizuzhaojixiaoxitishi);
                TipsManager.Instance.ShowTipWindow(0, 0, Client.TipWindowType.CancelOk, msg,
                    delegate()
                    {
                        GVoiceManger.Instance.JoinClanRoom();
                    },
                     null, okstr: "确定", cancleStr: "取消");
            }
        }
    }
}
