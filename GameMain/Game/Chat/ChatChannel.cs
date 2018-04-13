using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Cmd;
using System.Xml.Linq;
using GameCmd;
using Common;

/// <summary>
/// 客户端逻辑层使用的聊天结构
/// </summary>
public class ChatInfo
{
    public bool IsMe { get; set; }
    public CHATTYPE Channel { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string RichText { get; set; }
    public uint Timestamp { get; set; }
    public uint Id { get; set; }
    public string Name { get; set; }

    public int job;

    public float voiceLegth;

    public string voiceFileid = "";
    public bool played = false;
    public bool IsRedPacket = false;
    public uint redPacketID = 0;
}

/// <summary>
/// 聊天频道
/// </summary>
public class ChatChannel
{
    public CHATTYPE ChannelType;
    public string Head = "[无]";
    /// <summary>
    /// 聊天命令缓存
    /// </summary>
    private readonly CircularQueue<ChatInfo> dataCache;
    /// <summary>
    /// 聊天内容有变化，通知
    /// </summary>
    /// <param name="chatList"></param>
    public delegate void NoticeNewChat(IEnumerable<ChatInfo> chatList);
    /// <summary>
    /// 新聊天事件
    /// </summary>
    public event NoticeNewChat OnNewChat;

    public ChatChannel(CHATTYPE type, string head, int cacheLength)
    {
        ChannelType = type;
        Head = head;
        dataCache = new CircularQueue<ChatInfo>() { Capacity = cacheLength };
    }

    /// <summary>
    /// 添加一条聊天
    /// </summary>
    /// <param name="chatCmd"></param>
    public void Add(ChatInfo chatCmd)
    {
        dataCache.Enqueue(chatCmd);

        if (OnNewChat != null)
            OnNewChat(Enumerable.Repeat(chatCmd, 1));
    }

    public void Clear()
    {
        dataCache.Clear();
    }
    private ChatInfo ToSystemChatInfo(uint OPDesThisid, string speakText, string speakName, uint job, string voicefileid = "", uint voiceLength = 0, uint timestamp = 0)
    {

        speakText = speakText.Trim();
        TimeSpan ts = System.DateTime.Now - new System.DateTime(1970, 1, 1, 0, 0, 0);
        timestamp = (uint)System.Convert.ToInt32(ts.TotalSeconds);
        var title = string.Format(
                   "<color value=\"{0}\">{1} </color>",
                    string.Format("#{0:X8}", (uint)ConstDefine.GetChatColor(CHATTYPE.CHAT_NINE)),
                   Head);
        var richText = "";// FormatAddCharHead(Head, speakName, speakText);
        var chat = speakText;
        ChatInfo info = new ChatInfo()
        {
            IsMe = Client.ClientGlobal.Instance().IsMainPlayer(OPDesThisid),
            Id = OPDesThisid,
            Title = title,
            Content = FormatContent(chat),
            RichText = richText,
            Channel = ChannelType,
            Name = speakName,
            job = (int)job,
            voiceFileid = voicefileid,
            voiceLegth = voiceLength,
            Timestamp = timestamp != 0 ? timestamp : UserData.ServerTime.ToUnixTime(),
        };
        return info;

    }
    private ChatInfo ToChatInfo(uint OPDesThisid, string speakText, string speakName, uint job, string voicefileid = "", uint voiceLength = 0, uint timestamp = 0)
    {
        if(Head == "系统")
        {
           return  ToSystemChatInfo(OPDesThisid, speakText, speakName, job);
        }
        speakText = speakText.Trim();
        TimeSpan ts = System.DateTime.Now - new System.DateTime(1970, 1, 1, 0, 0, 0);
        timestamp = (uint)System.Convert.ToInt32(ts.TotalSeconds);
        var title = FormatCharTitle(Head, speakName, TimeSpan.FromTicks(timestamp).ToString(), speakText, OPDesThisid);
        var richText = FormatAddCharHead(Head, speakName, speakText);
        var chat = speakText;
        ChatInfo info = new ChatInfo()
        {
            IsMe = Client.ClientGlobal.Instance().IsMainPlayer(OPDesThisid),
            Id = OPDesThisid,
            Title = title,
            Content = FormatContent(chat),
            RichText = richText,
            Channel = ChannelType,
            Name = speakName,
            job = (int)job,
            voiceFileid = voicefileid,
            voiceLegth = voiceLength,
            Timestamp = timestamp != 0 ? timestamp : UserData.ServerTime.ToUnixTime(),
        };
        if (DataManager.Manager<ChatDataManager>().IsCanAutoPlayVoice(info.Channel))
        {
            string m_strVoicePath = DateTime.Now.ToFileTime().ToString();
            info.played = true;
            GVoiceManger.Instance.DownloadRecordedFile(info.voiceFileid, m_strVoicePath);
        }
        return info;
        
    }
 
    public ChatInfo ToChatInfo(GameCmd.stWildChannelCommonChatUserCmd_CS cmd)
    {
        return ToChatInfo(cmd.dwOPDes, cmd.szInfo, cmd.name, (uint)cmd.profession, cmd.voiceFildId, cmd.voiceLength);
    }

    //     public ChatInfo ToChatInfo(Pmd.stCommonChatUserPmd_CS cmd)
    // 	{
    //         return ToChatInfo(cmd.dwOPDes, cmd.szInfo, cmd.name,cmd.profession);
    // 	}


    public ChatInfo ToChatInfo(GameCmd.stSpeakerChatUserCmd_CS cmd)
    {
        return ToChatInfo(cmd.dwOPDes, cmd.szInfo, cmd.username, (uint)cmd.profession);
    }

    public ChatInfo ToChatInfoWithRedPackgetMsg(stNoticeRedPacketChatUserCmd_S cmd)
    {
        ChatInfo info = new ChatInfo();
        info.IsMe = cmd.name == MainPlayerHelper.GetMainPlayer().GetName()?true:false;
        info.job = (int)cmd.job;
        info.Title = cmd.name;
        info.RichText = DataManager.Manager<RedEnvelopeDataManager>().GetShortMessage(cmd.title);
        info.IsRedPacket = true;
        info.redPacketID = cmd.id;
        info.Timestamp = UserData.ServerTime.ToUnixTime();
        info.Channel = cmd.world?CHATTYPE.CHAT_WORLD:CHATTYPE.CHAT_CLAN;
        return info;
    }
    /// <summary>
    /// 聊天对象名转换成超链接格式
    /// </summary>
    /// <param name="srcText"></param>
    /// <param name="thisid"></param>
    /// <param name="extentedString">密聊时指明聊天方向的字符串</param>
    /// <returns></returns>
    public static string NameToLinkXml(string srcText, uint thisid, string extentedString = null)
    {
        if (string.IsNullOrEmpty(srcText))
            return srcText;

        if (extentedString != null)
            srcText = srcText + extentedString;
        return new XElement("a",
            new XElement("href", new KeyValueString() { { "username", thisid } }.Value),
            new XText(srcText).ToString()) // xml escape
            .ToString(SaveOptions.DisableFormatting);
    }

    public IEnumerable<ChatInfo> GetChatInfoList()
    {
        return dataCache;
    }
    /*老的表情解析
    public static string FormatContent(string content)
    {
        ////<ani fps="30" loop="true" atlas="atlas path" prefix="sprite name" />表情
        int max = DataManager.Manager<ChatDataManager>().EmojiMaxNum;
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < content.Length; i++)
        {
            if (content[i] == '#')
            {
                int startIndex = i + 1;

                string strnum = "";
                while (startIndex < content.Length && startIndex <= i + 3)
                {
                    if (content[startIndex] >= '0' && content[startIndex] <= '9')
                    {
                        strnum += content[startIndex].ToString();

                        int emoji = int.Parse(strnum);
                        if (emoji > max)
                        {
                            strnum = strnum.Remove(strnum.Length - 1, 1);
                            break;
                        }
                        startIndex++;
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }

                if (string.IsNullOrEmpty(strnum) == false)
                {
                   // int emoji = int.Parse(strnum);
                    sb.Append(string.Format("<ani fps=\"10\" atlas=\"emojiatlas\" prefix=\"{0}\" />", strnum));
                }
            }
            else
            {
                sb.Append(content[i]);
            }
        }

        return sb.ToString();
    }
    */

    public static string FormatContent(string content)
    {
        ////<ani fps="30" loop="true" atlas="atlas path" prefix="sprite name" />表情
        int max = DataManager.Manager<ChatDataManager>().EmojiMaxNum;
        StringBuilder sb = new StringBuilder();
        int endChar = 4;
        for (int i = 0; i < content.Length; i++)
        {

            if (content[i] == '#')
            {
                if (i + endChar < content.Length)
                {
                    if (content[i + endChar] == '#')
                    {
                        int startIndex = i + 1;

                        string strnum = "";
                        while (startIndex < content.Length && startIndex < i + endChar)
                        {
                            if (content[startIndex] >= '0' && content[startIndex] <= '9')
                            {
                                strnum += content[startIndex].ToString();

                                int emoji = int.Parse(strnum);
                                if (emoji > max)
                                {
                                    strnum = strnum.Remove(strnum.Length - 1, 1);
                                    break;
                                }
                                startIndex++;
                                i++;
                            }
                            else
                            {
                                i++;
                                break;
                            }

                        }
                        if (string.IsNullOrEmpty(strnum) == false)
                        {
                            // int emoji = int.Parse(strnum);
                            sb.Append(string.Format("<ani fps=\"10\" atlas=\"emojiatlas\" prefix=\"{0}\" />", strnum));
                        }
                    }
                    else
                    {
                        sb.Append(content[i]);
                    }
                }
                else
                {
                    sb.Append(content[i]);
                }
            }
            else
            {
                sb.Append(content[i]);
            }
        }

        return sb.ToString();
    }
    public string FormatAddCharHead(string channelHead, string extentedString, string srcText)
    {
        if (srcText.FirstOrDefault() == '<')
        {
            return RichXmlHelper.RemoveP(RichXmlHelper.RichXmlAdapt(srcText));
        }
        // 普通文本转换成富文本
        var color = string.Format("#{0:X8}", (uint)ConstDefine.GetChatColor(ChannelType));

       // Debug.Log("FormatAddCharHead：" + srcText + " " + srcText.Length);

        string xtext = new XText(srcText).ToString();
        //Debug.Log("FormatAddCharHeadxtext：" + xtext);

        return RichXmlHelper.RichXmlAdapt(string.Format("<color value=\"{0}\">{1}{2}:{3}</color>",
            color,
            channelHead,
            extentedString,
            xtext)); // xml escape
    }

    public string FormatCharTitle(string channelHead, string name, string time, string srcText, uint uid = 0)
    {
        try
        {
            // 			if (srcText.FirstOrDefault() == '<')
            // 			{
            // 				return RichXmlHelper.RemoveP(RichXmlHelper.RichXmlAdapt(srcText));
            // 			}
            // 普通文本转换成富文本
            var color = string.Format("#{0:X8}", (uint)ConstDefine.GetChatColor(ChannelType));
            var yellow = GXColor.Yellow;
            var colorYellow = string.Format("#{0:X8}", (uint)yellow);
            var gray = GXColor.Gray;
            var colorGray = string.Format("#{0:X8}", (uint)gray);

            string text = "";

            if (ChannelType == CHATTYPE.CHAT_SYS)
            {
                text = RichXmlHelper.RichXmlAdapt(string.Format(
                   "<color value=\"{0}\">{1} </color>",
                    string.Format("#{0:X8}", (uint)ConstDefine.GetChatColor(CHATTYPE.CHAT_NINE)),
                   channelHead,
                   new XText(srcText).ToString())); // xml escape
            }
            else
            {
                text = RichXmlHelper.RichXmlAdapt(string.Format(
                    //"<color value=\"{0}\">{1}</color>{2}",
                   "{0} {1}",
                    //color,
                   channelHead,
                   ChatDataManager.GetPlayerHrefString(name, uid, Cmd.GXColor.Yellow),
                   new XText(srcText).ToString())); // xml escape
            }

            //Debug.Log(text);
            return text;
        }
        catch (Exception e)
        {
            Debug.LogError(string.Format("e.Message:{0},e.StackTrace:{1}", e.Message, e.StackTrace));
        }
        return String.Empty;
    }
}
