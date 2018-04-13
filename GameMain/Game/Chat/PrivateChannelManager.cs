using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCmd;
using System.Linq;
using Client;
using Common;
using Cmd;
using System.Xml.Linq;
using Engine.Utility;

/// <summary>
/// 私聊频道管理
/// </summary>
public class PrivateChannelManager
{
    Dictionary<uint, CircularQueue<ChatInfo>> m_dictChatInfo = null;
    Dictionary<uint, bool> m_dictMsgTips = null;
    /// <summary>
    /// 私聊id
    /// </summary>
    uint silentFriendUID = 0;
    public uint SilentFrienduid { get { return silentFriendUID; } set { silentFriendUID = value; } }

    public string silentOPDestName = string.Empty;

    public ChatChannel.NoticeNewChat OnNewChat;
    public ChatChannelFilter.RefreshText OnRefreshOutput;

    private readonly List<ChatInfo> lines = new List<ChatInfo>();
    public PrivateChannelManager()
    {
        m_dictChatInfo = new Dictionary<uint, CircularQueue<ChatInfo>>();
        m_dictMsgTips = new Dictionary<uint, bool>();
    }

    private bool haveMsgFromFriend = false;
    public bool HaveMsgFromFriend 
    {
        get
        {
            return haveMsgFromFriend;       
        }
        set 
        {
            value = haveMsgFromFriend;
        }
    }

    public void Clear()
    {
        m_dictChatInfo.Clear();
        m_dictMsgTips.Clear();
        silentFriendUID = 0;
        silentOPDestName = string.Empty;
        haveMsgFromFriend = false;
        lines.Clear();
    }

    public void SetCurrChatPlayer(uint id, string name)
    {
        silentFriendUID = id;
        silentOPDestName = name;
        SetReadMsgTipsById(id, false);
    }

    /// <summary>
    /// 发送私聊
    /// </summary>
    public bool SendPrivateChat(string msg, bool isRobot, string fileid = "", uint length = 0)
    {
        if (!ChatDataManager.CanSendChatMsgWithBlack(silentFriendUID, silentOPDestName, true))
        {
            return false;
        }
        stCommonMChatUserCmd_CS cmd = new stCommonMChatUserCmd_CS()
        {
            szInfo = msg,
            byChatType = CHATTYPE.CHAT_SILENT,
            dwOPDes = silentFriendUID,
            szOPDes = silentOPDestName,
            profession = (GameCmd.enumProfession)Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.PlayerProp.Job),
            voiceFildId = fileid,
            voiceLength = length,
        };

        SendMyChatCmdM(cmd); // 密聊不会回发给自己，所以发一份。
        if (!isRobot)
        {
            NetService.Instance.Send(cmd);
        }
        return true;
    }

    private void SendMyChatCmdM(GameCmd.stCommonMChatUserCmd_CS cmd)
    {
        cmd.szOPDes = Client.ClientGlobal.Instance().MainPlayer.GetName();
        AddChat(cmd);
    }

    /// <summary>
    /// 重新刷新当前聊天数据
    /// </summary>
    public void InitCurrChatData()
    {
        CircularQueue<ChatInfo> chatList;
        lines.Clear();
        if (m_dictChatInfo.TryGetValue(silentFriendUID, out chatList))
        {
            chatList.OrderBy(i => i.Timestamp);
            lines.AddRange(from i in chatList select i);
        }
        if (OnRefreshOutput != null)
            OnRefreshOutput(lines);
    }

    private void UpdateMsgTipsUI(ChatInfo chatInfo)
    {
        if (chatInfo.Id == silentFriendUID)
        {
            if (OnNewChat != null)
                OnNewChat(Enumerable.Repeat(chatInfo, 1));
            else
            {
                //没有选中的时候silentFriendUID 是0（系统）但是OnNewChat 为空。此时应判断如果打开了界面就刷新系统新消息提示
                SetReadMsgTipsById(chatInfo.Id, true);
                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FriendPanel))
                {
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FriendPanel, UIMsgID.eUpdateFriendMsgTips, chatInfo.Id);
                }
            }
        }
        else if (chatInfo.IsMe == false)
        {
            SetReadMsgTipsById(chatInfo.Id, true);
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FriendPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.FriendPanel, UIMsgID.eUpdateFriendMsgTips, chatInfo.Id);
            }
        }
    }

    public void AddChat(GameCmd.stCommonMChatUserCmd_CS cmd)
    {
        if(cmd.byChatType != CHATTYPE.CHAT_SILENT)
        {
            return;
        }
        if (!ChatDataManager.CanRecieveChatMsgWithBlack(cmd.dwOPDes))
        {
            //Engine.Utility.Log.Info("player {0} is in black not recieve chat msg!", cmd.dwOPDes);
            return;
        }
        
        CircularQueue<ChatInfo> chatList;
        if (!m_dictChatInfo.TryGetValue(cmd.dwOPDes, out chatList))
        {
            chatList = new CircularQueue<ChatInfo>() { Capacity = 50 };
            m_dictChatInfo.Add(cmd.dwOPDes, chatList);
            m_dictMsgTips[cmd.dwOPDes] = false;
        }

        ChatInfo chatInfo = ToChatInfo(cmd);
        chatList.Enqueue(chatInfo);

        if (chatInfo.Id != 0)
        {
            if (chatInfo.IsMe)
            {
                //DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0);
                //TimeSpan ts = (UserData.ServerTime - UnixBase );

                //chatInfo.Timestamp = (uint)ts.TotalSeconds - (uint)UserData.ServerTimeDelta;
                chatInfo.Timestamp = (uint)DateTimeHelper.Instance.Now;
            }
            UpdateMsgTipsUI(chatInfo);
            DataManager.Manager<RelationManager>().UpdateContactTimestamp(chatInfo.Id, chatInfo.Timestamp);
        }
        else
        {
            UpdateMsgTipsUI(chatInfo);
        }
    }

    void SetReadMsgTipsById(uint id, bool read)
    {
        if (m_dictMsgTips.ContainsKey(id))
        {
            m_dictMsgTips[id] = read;
        }
        bool btip = false;
        foreach(var i in m_dictMsgTips )
        {
            if (i.Key != 0 && i.Value)
           {
               btip = true;
               break;
           }
        }
//         foreach (bool status in m_dictMsgTips.Values)
//         {
//             if (status)
//             {
//                 btip = true;
//                 break;
//             }
//         }
        haveMsgFromFriend = btip;
        if (haveMsgFromFriend)
        {
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.Mail,
                direction = (int)WarningDirection.None,
                bShowRed = true,
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.FRIEND_ADDNEWMSG, null);
    }

    public bool GetNewMsgTipsWithId(uint id)
    {
        if (m_dictMsgTips.ContainsKey(id))
        {
            return m_dictMsgTips[id];
        }
        return false;
    }

    ChatInfo ToChatInfo(GameCmd.stCommonMChatUserCmd_CS cmd)
    {
        return ToChatInfo(cmd.dwOPDes, cmd.szInfo, cmd.szOPDes, cmd.byChatType, (uint)cmd.profession, cmd.timestamp, cmd.voiceFildId, cmd.voiceLength);
    }

    private ChatInfo ToChatInfo(uint OPDesThisid, string speakText, string name, CHATTYPE type, uint profession, uint timestmap, string voicefileid = "", uint voiceLength = 0)
    {
        speakText = speakText.Trim();
   
       // var title = FormatCharTitle(Head, name, TimeSpan.FromTicks(timestmap).ToString(), speakText, OPDesThisid);
        var richText = FormatPrivateAddCharHead(Head, name, speakText, voicefileid);
        return new ChatInfo()
        {
            IsMe = type == CHATTYPE.CHAT_SILENT ? Client.ClientGlobal.Instance().MainPlayer.GetName() == name : false,
            Id = OPDesThisid,
            Content = ChatChannel.FormatContent(speakText),
            Channel = type,
            Name = name,
            job = (int)profession,
            Timestamp = timestmap,
            voiceFileid = voicefileid,
            voiceLegth = voiceLength,
            RichText = richText
        };
    }
    public string Head = "私聊";
    public string FormatCharTitle(string channelHead, string name, string time, string srcText, uint uid = 0)
    {
        try
        {
            // 			if (srcText.FirstOrDefault() == '<')
            // 			{
            // 				return RichXmlHelper.RemoveP(RichXmlHelper.RichXmlAdapt(srcText));
            // 			}
            // 普通文本转换成富文本
            var color = string.Format("#{0:X8}", (uint)ConstDefine.GetChatColor(GameCmd.CHATTYPE.CHAT_PRIVATE));
            var yellow = GXColor.Yellow;
            var colorYellow = string.Format("#{0:X8}", (uint)yellow);
            var gray = GXColor.Gray;
            var colorGray = string.Format("#{0:X8}", (uint)gray);

            string text = "";


            text = RichXmlHelper.RichXmlAdapt(string.Format(
                //"<color value=\"{0}\">{1}</color>{2}",
               "{0} {1}",
                //color,
               channelHead,
               ChatDataManager.GetPlayerHrefString(name, uid, Cmd.GXColor.Yellow),
               new XText(srcText).ToString())); // xml escape


            //Debug.Log(text);
            return text;
        }
        catch (Exception e)
        {
            Log.Error(string.Format("e.Message:{0},e.StackTrace:{1}", e.Message, e.StackTrace));
        }
        return String.Empty;
    }
    public string FormatPrivateAddCharHead(string channelHead, string extentedString, string srcText, string voicefileid = "")
    {
      //  Log.LogGroup("ZDY", "enter ：FormatPrivateAddCharHead");
        if (srcText.FirstOrDefault() == '<')
        {
            return RichXmlHelper.RemoveP(RichXmlHelper.RichXmlAdapt(srcText));
        }

        //Log.LogGroup("ZDY", "普通文本转换成富文本：" + srcText + " " + srcText.Length);
        // 普通文本转换成富文本
        var color = string.Format("#{0:X8}", (uint)ConstDefine.GetChatColor(GameCmd.CHATTYPE.CHAT_PRIVATE));

        string xtext = new XText(srcText).ToString();
   
        return RichXmlHelper.RichXmlAdapt(string.Format("<color value=\"{0}\">{1}{2}:{3}</color>",
            color,
            channelHead,
            extentedString,
            xtext)); // xml escape
    }
}
