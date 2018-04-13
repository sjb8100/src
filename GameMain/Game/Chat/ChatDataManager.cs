using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Engine.Utility;
using System.Text.RegularExpressions;
using gcloud_voice;
using UnityEngine;
using Client;
using table;
enum ClientChannelGroup
{
    None = 0,
    SystemChannel = 1,//系统
    Team = 2,//队伍
    Clan = 4,//氏族
    Near = 8,//附近
    GodEvil = 16,//神魔
    Light = 32,//跑马灯
    Tips = 64,
    World = 128,
}
public class ChatDataManager : BaseModuleData, IManager
{
    class SendMsgCDInfo
    {
        public CHATTYPE type;
        public float currTime = 0;
        public int coolDownTime;
        public bool canSend = true;
    }

    //管理器是否准备好
    private bool ready = false;
    public bool Ready
    {
        get
        {
            return ready;
        }
    }

    /// <summary>
    /// 聊天频道的默认缓存长度
    /// </summary>
    private const int DefaultLength = 30;

    /// <summary>
    /// 聊天频道
    /// </summary>
    private Dictionary<CHATTYPE, ChatChannel> m_dictChannel;
    private Dictionary<CHATTYPE, ChatChannelFilter> m_dictChannelFilter;
    private ChatChannelFilter m_simpleChannelFilter;
    private ChatChannelFilter m_currChannelFilter;
    private Dictionary<CHATTYPE, bool> m_dictAutoPlay;
    private bool IsInLiuLiangAuto = false;
    private Dictionary<CHATTYPE, SendMsgCDInfo> m_dictSendMsgCd;
    private PrivateChannelManager m_privateChannelManager;
    private CHATTYPE m_curChatType = CHATTYPE.CHAT_NONE;
    public CHATTYPE CurChatType
    {
        get
        {
            return m_curChatType;
        }
        set
        {
            m_curChatType = value;
        }
    }
    /// <summary>
    /// 刷新当前聊天界面信息
    /// </summary>
    public ChatChannelFilter.RefreshText OnRefresh
    {
        get { return m_currChannelFilter.OnRefreshOutput; }
        set { m_currChannelFilter.OnRefreshOutput = value; }
    }
    /// <summary>
    /// 新添加聊天信息
    /// </summary>
    public ChatChannelFilter.RefreshText OnAddOutput
    {
        get { return m_currChannelFilter.OnAddOutput; }
        set { m_currChannelFilter.OnAddOutput = value; }
    }

    /// <summary>
    /// 小聊天窗口 刷新当前聊天界面信息
    /// </summary>
    public ChatChannelFilter.RefreshText OnSimpleRefresh
    {
        get { return m_simpleChannelFilter.OnRefreshOutput; }
        set { m_simpleChannelFilter.OnRefreshOutput = value; }
    }
    /// <summary>
    /// 小聊天窗口 新添加聊天信息
    /// </summary>
    public ChatChannelFilter.RefreshText OnSimpleAddOutput
    {
        get { return m_simpleChannelFilter.OnAddOutput; }
        set { m_simpleChannelFilter.OnAddOutput = value; }
    }

    public PrivateChannelManager PrivateChatManager
    {
        get { return m_privateChannelManager; }
    }
    /// <summary>
    /// 历史消息
    /// </summary>
    private List<string> HistoryMsgs;
    const int MaxHistoryMsg = 5;
    //预设消息
    const string presetMsg = "PresetMsg";
    string[] presetmessage = new string[] { "求好友一起游戏~", "求加入家族组织~", "哪个氏族收人~" };

    public int EmojiMaxNum = 1000;
    /// <summary>
    /// 世界频道消耗金币
    /// </summary>
    uint m_chatWorldChannelCost = 1000;
    public uint ChatWorldCost { get { return m_chatWorldChannelCost; } }

    //世界聊天等级限制
    private int worldChatLimitLv = 0;
    public int WorldChatLimitLv
    {
        get
        {
            if (worldChatLimitLv == 0)
            {
                worldChatLimitLv = GameTableManager.Instance.GetGlobalConfig<int>("WorldChatLimitLv");
            }
            return worldChatLimitLv;
        }
    }
    public float ClanCallLeftTime { get; set; }
    public GameCmd.VoChatMode ClanChatMode { get; set; }
    void AddChatChannel(CHATTYPE chatType, string strName, int ncd, int nlength = DefaultLength)
    {
        int cdtime = 0;
        CHATTYPE type = chatType;
        m_dictChannel.Add(type, new ChatChannel(type, strName, nlength));
        m_dictAutoPlay.Add(type, false);
        cdtime = GameTableManager.Instance.GetGlobalConfig<int>("ChatMsgCD_" + ncd.ToString());
        if (cdtime > 0)
        {
            if (m_dictSendMsgCd.ContainsKey(type))
            {
                m_dictSendMsgCd[type] = new SendMsgCDInfo() { type = type, coolDownTime = cdtime };
            }
            else
            {
                m_dictSendMsgCd.Add(type, new SendMsgCDInfo() { type = type, coolDownTime = cdtime });
            }
        }
    }
    public void ClearData()
    {

    }
    public void Initialize()
    {
        if (ready)
        {
            return;
        }
        HistoryMsgs = new List<string>(MaxHistoryMsg);
        m_privateChannelManager = new PrivateChannelManager();

        m_dictChannel = new Dictionary<CHATTYPE, ChatChannel>();
        m_dictChannelFilter = new Dictionary<CHATTYPE, ChatChannelFilter>();
        m_dictSendMsgCd = new Dictionary<CHATTYPE, SendMsgCDInfo>();

        m_dictAutoPlay = new Dictionary<CHATTYPE, bool>();

        {
            AddChatChannel(CHATTYPE.CHAT_WORLD, "世界", 6);
            AddChatChannel(CHATTYPE.CHAT_TEAM, "队伍", 2);
            AddChatChannel(CHATTYPE.CHAT_RECRUIT, "招募", 3);
            AddChatChannel(CHATTYPE.CHAT_CLAN, "氏族", 4);
            AddChatChannel(CHATTYPE.CHAT_MAP, "附近", 1);
            AddChatChannel(CHATTYPE.CHAT_DEMON, "神魔", 5);
            AddChatChannel(CHATTYPE.CHAT_SYS, "系统", 7);
        }

        {
            m_simpleChannelFilter = new ChatChannelFilter(m_dictChannel.Values.ToArray());

            foreach (var item in m_dictChannel.Values)
            {
                if (item.ChannelType == CHATTYPE.CHAT_WORLD)
                {
                    m_dictChannelFilter.Add(item.ChannelType, new ChatChannelFilter(new ChatChannel[] {
                       m_dictChannel[CHATTYPE.CHAT_WORLD] }));
                }
                else
                {
                    m_dictChannelFilter.Add(item.ChannelType, new ChatChannelFilter(new ChatChannel[] { m_dictChannel[item.ChannelType] }));
                }

                if (PlayerPrefs.GetInt("ChatSimpleChannel" + item.ChannelType.ToString(), 1) != 1)
                {
                    m_simpleChannelFilter.RemoveChannel(item.ChannelType);
                }
            }

            RegisterEvent(true);
            m_simpleChannelFilter.ActiveFilter(true);

        }

        ready = true;
    }
    void RegisterEvent(bool bReg)
    {
        if (bReg)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANQUIT, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);//
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANQUIT, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECONNECT_SUCESS, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnEvent);
        }
    }
    public void Reset(bool depthClearData = false)
    {
        foreach (var item in m_dictChannel.Values)
        {
            item.Clear();
        }
        if (m_privateChannelManager != null)
        {
            m_privateChannelManager.Clear();
        }
        //         if (m_currChannelFilter != null)
        //         {
        //             m_currChannelFilter.ActiveFilter(false);
        //         }
        //         if (m_simpleChannelFilter != null)
        //         {
        //             m_simpleChannelFilter.ActiveFilter(false);
        //         }
        HistoryMsgs.Clear();

        m_dictAutoPlay.Clear();
        foreach (var dic in m_dictSendMsgCd)
        {
            dic.Value.canSend = false;
            dic.Value.currTime = 0;
        }
    }

    public void Process(float deltaTime)
    {
        if (!ready)
        {
            return;
        }
        // foreach (var item in m_dictSendMsgCd.Values)
        var iter = m_dictSendMsgCd.GetEnumerator();
        while (iter.MoveNext())
        {
            var item = iter.Current.Value;
            if (item.canSend == false)
            {
                item.currTime -= deltaTime;
                if (item.currTime <= 0)
                {
                    item.canSend = true;
                    item.currTime = 0;
                }
                if (curChatInputChannel != null && curChatInputChannel.ChannelType == item.type)
                {
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ChatPanel, UIMsgID.eRefreshSendBtnLable, UnityEngine.Mathf.CeilToInt(item.currTime));
                }
            }
        }

        if (ClanCallLeftTime > 0)
        {
            ClanCallLeftTime -= deltaTime;
        }
    }

    void OnEvent(int neventid, object param)
    {
        if (neventid == (int)Client.GameEventID.CLANQUIT)
        {
            Client.stClanQuit cq = (Client.stClanQuit)param;
            if (cq.uid == MainPlayerHelper.GetPlayerUID())
            {
                if (m_dictChannel.ContainsKey(CHATTYPE.CHAT_CLAN))
                {
                    m_dictChannel[CHATTYPE.CHAT_CLAN].Clear();
                }
            }
        }
        else if (neventid == (int)Client.GameEventID.RECONNECT_SUCESS)
        {
            Client.stReconnectSucess st = (Client.stReconnectSucess)param;
            if (st.isLogin)
            {

            }
        }
        else if (neventid == (int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE)
        {
            stPropUpdate prop = (stPropUpdate)param;
            if (prop.uid != MainPlayerHelper.GetPlayerUID())
            {
                return;
            }
            if (prop.nPropIndex == (int)CreatureProp.Level)
            {
                string msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_renwushengji, prop.newValue);
                //Talk_System_renwushengji
                SendToChatSystem(msg);
            }
            else if (prop.nPropIndex == (int)PlayerProp.Exp)
            {
                int delta = prop.newValue - prop.oldValue;
                if (delta <= 0)
                {
                    return;
                }
                string msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_huodejingyan, prop.newValue - prop.oldValue);
                //Talk_System_renwushengji
                SendToChatSystem(msg);
            }
            else if (prop.nPropIndex == (int)PlayerProp.PkMode)
            {
                PLAYERPKMODEL model = (PLAYERPKMODEL)prop.newValue;
                string msg = string.Format("成功切换为{0}模式", model.GetEnumDescription());
                TipsManager.Instance.ShowTips(msg);
                SendToChatSystem(msg);
            }
        }
        else if (neventid == (int)GameEventID.UIEVENT_REFRESHCURRENCYNUM)
        {
            ItemDefine.UpdateCurrecyPassData data = (ItemDefine.UpdateCurrecyPassData)param;
            if (data.MoneyType == MoneyType.MoneyType_Gold)
            {
                if (data.ChangeNum <= 0)
                {
                    return;
                }

                string msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_huodejinbi, data.ChangeNum);
                //Talk_System_renwushengji
                SendToChatSystem(msg);
            }
            else if (data.MoneyType == MoneyType.MoneyType_MoneyTicket)
            {
                if (data.ChangeNum <= 0)
                {
                    return;
                }
                string msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_huodewenqian, data.ChangeNum);
                //Talk_System_renwushengji
                SendToChatSystem(msg);
            }
            else if (data.MoneyType == MoneyType.MoneyType_Reputation)
            {
                if (data.ChangeNum <= 0)
                {
                    return;
                }
                string msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_huodejianshedu, data.ChangeNum);
                //Talk_System_renwushengji
                SendToChatSystem(msg);
                
            }
            else if (data.MoneyType == MoneyType.MoneyType_FishingMoney)
            {
                if (data.ChangeNum <= 0)
                {
                    return;
                }
                string msg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_huodeyubi, data.ChangeNum);
                SendToChatSystem(msg);
            }
        }
        else if (neventid == (int)GameEventID.UIEVENT_UPDATEITEM)
        {
            ItemDefine.UpdateItemPassData passData = (ItemDefine.UpdateItemPassData)param;
            if (passData.AddItemAction != AddItemAction.AddItemAction_Refresh &&
                passData.AddItemAction != AddItemAction.AddItemAction_Load &&
                passData.UpdateType == ItemDefine.UpdateItemType.Add)
            {
                BaseItem equip = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(passData.QWThisId);
                table.ItemDataBase item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(passData.BaseId);
                if (item != null)
                {
                    string msg = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Talk_System_huodewupin);
                    //Talk_System_renwushengji
                    if (equip != null)
                        SendToChatSystem(msg, 0, passData.QWThisId, item.itemName, (uint)equip.QualityType, false, LinkType.LinkItem);
                    else
                        SendToChatSystem(msg, 0, passData.QWThisId, item.itemName, item.quality, false, LinkType.LinkItem);
                }
            }
        }
    }

    #region 自动播放语音
    public bool IsAutoPlayVoice(CHATTYPE chatType)
    {
        if (m_dictAutoPlay.ContainsKey(chatType))
        {
            return m_dictAutoPlay[chatType];
        }
        else
        {
            return false;
        }
    }
    public bool IsCanAutoPlayVoice(CHATTYPE chatType)
    {
        if (!IsInLiuLiangAuto && Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)//这里要判断一下当前的网络是什么 如果是3G/4G那么就返回false
            return false;
        return IsAutoPlayVoice(chatType);
    }
    public bool IsAutoPlayInLiuLiang()
    {
        return IsInLiuLiangAuto;
    }
    public void SetAutoInLiuLiangPlay(bool bauto)
    {
        IsInLiuLiangAuto = bauto;
    }
    public void SetAutoPlayVoice(CHATTYPE chatType, bool bAuto)
    {
        m_dictAutoPlay[chatType] = bAuto;
    }
    #endregion 自动播放语音
    public bool IsCanSendMsg()
    {


        SendMsgCDInfo info;
        if (m_dictSendMsgCd.TryGetValue(curChatInputChannel.ChannelType, out info))
        {
            return info.canSend;
        }
        return true;
    }
    #region 预设消息
    public string GetPresetMsg(int index)
    {
        if (index >= 0 && index < 3)
        {
            return UnityEngine.PlayerPrefs.GetString(presetMsg + index.ToString(), presetmessage[index]);
        }

        return "";

    }

    public string GetNorMalPresetMsg(int index)
    {
        if (index >= 0 && index < 3)
        {
            return presetmessage[index];
        }

        return "";
    }

    public void SetPresetMsg(int index, string msg)
    {
        UnityEngine.PlayerPrefs.SetString(presetMsg + index.ToString(), msg);
    }
    #endregion 预设消息
    public string GetChannelStr(CHATTYPE type)
    {
        string channelStr = "客服";
        foreach (var item in m_dictChannel)
        {
            if (item.Value.ChannelType == type)
                return item.Value.Head;
        }
        return channelStr;
    }
    public ChatChannel GetChannelByType(CHATTYPE channeltype)
    {
        ChatChannel channel = null;
        // CHATTYPE type = (CHATTYPE)Enum.ToObject(typeof(CHATTYPE), channeltype);
        if (!m_dictChannel.TryGetValue(channeltype, out channel))
        {
            Log.Warning("收到无效聊天频道：" + channeltype);
        }
        return channel;
    }
    /// <summary>
    /// 设置频道过滤器
    /// </summary>
    /// <param name="filterName"></param>
    public void SetChannelFilter(CHATTYPE filterName)
    {
        ChatChannelFilter filter;
        if (!m_dictChannelFilter.TryGetValue(filterName, out filter))
        {
            return;
        }

        if (m_currChannelFilter == filter)
        {
            return;
        }

        if (m_currChannelFilter != null)
        {
            m_currChannelFilter.ActiveFilter(false);
        }

        m_currChannelFilter = filter;
        m_currChannelFilter.ActiveFilter(true);
    }

    /// <summary>
    /// 初始化当前输出
    /// </summary>
    public void InitOutputText()
    {
        if (m_currChannelFilter == null)
        {
            return;
        }

        m_currChannelFilter.InitFilterData();
    }

    /// <summary>
    /// 小聊天窗口输出
    /// </summary>
    public void InitSimpleOutputText()
    {
        if (m_simpleChannelFilter == null)
        {
            return;
        }

        m_simpleChannelFilter.InitFilterData();
    }

    /// <summary>
    /// 小聊天窗口频道过滤设置
    /// </summary>
    /// <param name="type"></param>
    /// <param name="bselect">是否添加</param>
    public void SetSimpleChannel(CHATTYPE type, bool bselect)
    {
        if (m_simpleChannelFilter == null)
        {
            return;
        }
        if (bselect)
        {
            if (m_dictChannel.ContainsKey(type))
            {
                m_simpleChannelFilter.AddChannel(m_dictChannel[type]);
                PlayerPrefs.SetInt("ChatSimpleChannel" + type.ToString(), 1);
            }
            else
            {
                Engine.Utility.Log.Error("设置小聊天窗口出错未知频道：{0}", type);
            }
        }
        else
        {
            PlayerPrefs.SetInt("ChatSimpleChannel" + type.ToString(), 0);
            m_simpleChannelFilter.RemoveChannel(type);
        }

    }

    public bool SimpleChannelContain(CHATTYPE type)
    {
        return m_simpleChannelFilter.HasChatChannel(type);
    }

    /// <summary>
    /// 聊天输入的频道
    /// </summary>
    ChatChannel curChatInputChannel;
    /// <summary>
    /// GM指令正则
    /// </summary>
    private static Regex gmcommandRegex = new Regex(@"^\s*(?<type>/{2,3})\s*(?<method>\w+)(\s+(?<args>.*?)\s*)?$");
    /// <summary>
    /// 设置当前输入频道
    /// </summary>
    /// <param name="strType"></param>
    /// <returns></returns>
    public bool SetChatInputType(CHATTYPE cahtType)
    {
        ChatChannel chatChannel;
        if (!m_dictChannel.TryGetValue(cahtType, out chatChannel))
        {
            return false;
        }
        curChatInputChannel = chatChannel;
        return true;
    }

    /// <summary>
    /// 跟当前输入频道是否一致
    /// </summary>
    /// <param name="chatType"></param>
    /// <returns></returns>
    public bool EqualsCurrChannel(CHATTYPE chatType)
    {
        if (curChatInputChannel == null)
        {
            return false;
        }
        return curChatInputChannel.ChannelType == chatType;
    }

    public bool SendChatText(string message)
    {
        // 先检查是不是GM指令
        if (TrySendGmCommand(message))
            return true;
        string msg = DataManager.Manager<TextManager>().ReplaceSensitiveWord(message, TextManager.MatchType.Max);
        // 正常聊天消息
        return SendNormalCommonChat(msg);
    }

    public bool SendText(CHATTYPE chatType, string strMsg)
    {
        string msg = DataManager.Manager<TextManager>().ReplaceSensitiveWord(strMsg, TextManager.MatchType.Max);
        if (m_dictChannel.ContainsKey(chatType))
        {
            curChatInputChannel = m_dictChannel[chatType];
            return SendNormalCommonChat(msg);
        }
        return false;
    }

    public bool SendVoice(string shareId, string text, int length)
    {
        string msg = text.Remove(text.Length - 1);//不能删这行代码--
        msg = DataManager.Manager<TextManager>().ReplaceSensitiveWord(msg, TextManager.MatchType.Max);
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FriendPanel))
        {
            Engine.Utility.Log.Error("发送私聊语音");
            SendPrivateChat(msg, false, shareId, (uint)length);
            return true;
        }
        else
        {
            Engine.Utility.Log.Error("发送普通语音");
            return SendNormalCommonChat(msg, shareId, (uint)length);
        }
    }
    /// <summary>
    /// 发送私聊
    /// </summary>
    /// <param name="message"></param>
    public void SendPrivateChat(string message, bool isRobot, string fileid = "", uint length = 0)
    {
        message = DataManager.Manager<TextManager>().ReplaceSensitiveWord(message, TextManager.MatchType.Max);
        bool success = m_privateChannelManager.SendPrivateChat(message, isRobot, fileid, length);
        if (!success)
        {
            return;
        }
        if (IsPresetMsg(message) == false)
        {
            UpdateHistorMsg(message);
        }
    }
    /// <summary>
    /// 若是GM指令，则发送
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private bool TrySendGmCommand(string message)
    {
        if (string.IsNullOrEmpty(message))
            return false;
        var match = gmcommandRegex.Match(message);
        if (match.Success == false)
            return false;
        var type = match.Groups["type"].Value;
        var method = match.Groups["method"].Value;
        var args = match.Groups["args"].Value;
        var cmd = new GameCmd.stGMChatUserCmd_C()
        {
            tag = new GameCmd.stGMTag()
            {
                szCommand = method,
                szOPCode = args,
            },
        };
        if (type == "//")
        {
            NetService.Instance.Send(cmd); // 服务器GM指令
            UpdateHistorMsg(message);
        }
        //else if (type == "///")
        // NetService.Instance.SendToMe(cmd); // 客户端GM指令
        return true;
    }

    private void UpdateHistorMsg(string message)
    {
        if (HistoryMsgs.Count < 5)
        {
            int index = HistoryMsgs.IndexOf(message);
            if (index != -1)
            {
                return;
            }

            HistoryMsgs.Add(message);

            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MsgSettingPanel))
            {
                MsgSettingPanel panel = DataManager.Manager<UIPanelManager>().GetPanel<MsgSettingPanel>(PanelID.MsgSettingPanel);
                if (panel != null)
                {
                    panel.RefreshHistoryMsg();
                }
            }
        }
        else
        {
            int index = HistoryMsgs.IndexOf(message);
            if (index == -1)
            {
                index = 4;
            }
            for (int i = index; i > 0; i--)
            {
                HistoryMsgs[i] = HistoryMsgs[i - 1];
            }
            HistoryMsgs[0] = message;

            if (index != 0 && DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MsgSettingPanel))
            {
                MsgSettingPanel panel = DataManager.Manager<UIPanelManager>().GetPanel<MsgSettingPanel>(PanelID.MsgSettingPanel);
                if (panel != null)
                {
                    panel.RefreshHistoryMsg();
                }
            }
        }
    }

    /// <summary>
    /// 发送普通聊天指令
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private bool SendNormalCommonChat(string message, string fileid = "", uint length = 0)
    {
        //历史消息不替换屏蔽字

        GameCmd.stWildChannelCommonChatUserCmd_CS cmd;
        if (curChatInputChannel.ChannelType == CHATTYPE.CHAT_TEAM)
        {
            //if (DataManager.Manager<TeamDataManager>().TeamState == TeamState.Alone)
            if (DataManager.Manager<TeamDataManager>().IsJoinTeam == false)
            {
                TipsManager.Instance.ShowTips("你不在队伍中");
                UnityEngine.Debug.LogError("--------->>>>>>> 你不在队伍中");
                return false;
            }
        }
        cmd = new GameCmd.stWildChannelCommonChatUserCmd_CS()
        {
            szInfo = message,
            byChatType = curChatInputChannel.ChannelType,
            dwOPDes = Client.ClientGlobal.Instance().MainPlayer.GetID(),
            voiceFildId = fileid,
            voiceLength = length,
        };
        if (IsPresetMsg(message) == false)
        {
            UpdateHistorMsg(message);
        }

        SendMsgCDInfo info;
        if (m_dictSendMsgCd.TryGetValue(curChatInputChannel.ChannelType, out info))
        {
            if (!info.canSend)
            {
                TipsManager.Instance.ShowTipsById(513);
                return false;
            }
            info.canSend = false;
            info.currTime = info.coolDownTime;
        }


        NetService.Instance.Send(cmd);

        return true;
    }

    /// <summary>
    /// 是否预设消息
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    private bool IsPresetMsg(string msg)
    {
        for (int i = 0; i < presetmessage.Length; i++)
        {
            if (msg == GetPresetMsg(i))
            {
                return true;
            }
        }
        return false;
    }



    /// <summary>
    /// "BBCode:{0}type:{1},uid:{2},ext:{3}"; 
    /// </summary>
    /// <param name="url"></param>
    public void OnUrlClick(string url)
    {
        string[] urls = url.Split(',');
        if (url.Length > 0)
        {
            if (urls[0].StartsWith("type:"))
            {
                ChatDataManager.LinkType type = (ChatDataManager.LinkType)(int.Parse(urls[0].Replace("type:", "")));
                switch (type)
                {
                    case LinkType.LinkItem:
                    case LinkType.LinkPet:
                    case LinkType.LinkRide:
                        OnItemLink(urls);
                        break;
                    case LinkType.LinkPlayer:
                        OnPlayerView(urls);
                        break;
                    case LinkType.JoinTeam:
                        OnJoinTeam(urls);
                        break;
                    case LinkType.JoinClan:
                        OnJoinClan(urls);
                        break;
                    case LinkType.LinkBoss:
                        OnBossRresh(urls);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            Engine.Utility.Log.Error("传入链接字符串错误{0}", url);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">历史消息索引</param>
    /// <param name="itemName"></param>
    /// <param name="itemThisId"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    public bool CheckIsLinkItem(int historyindex, ref string itemName, out uint itemThisId, out uint quality, out int type)
    {
        List<string> history = DataManager.Manager<ChatDataManager>().HistoryMsgs;
        itemThisId = 0;
        quality = 0;
        type = 0;
        if (historyindex >= 0 && historyindex < history.Count)
        {
            if (history[historyindex].Contains("<a href=\"") && history[historyindex].Contains("</a>"))
            {
                string[] temp = history[historyindex].Split(new string[] { "</a>" }, System.StringSplitOptions.RemoveEmptyEntries);

                for (int k = 0; k < temp.Length; k++)
                {
                    int index = temp[k].IndexOf("<a href=\"");
                    if (index == -1)
                    {
                        continue;
                    }
                    if (temp[k].Length > index + 9)
                    {
                        string strlink = temp[k].Substring(index + 9);
                        int pos = strlink.IndexOf("\">[");
                        if(pos == -1)
                        {
                            continue;
                        }
                        var strTemp = strlink.Substring(0, pos);
                        strTemp = strTemp.Replace("BBCode:", "");
                        if(strTemp.Length == 0)
                        {
                            continue;
                        }
                        string strLinkMsg = strTemp;//strlink.Substring(0, pos).Replace("BBCode:", "").Substring(1);
                        string[] linkMsgs = strLinkMsg.Split(',');


                        for (int i = 0; i < linkMsgs.Length; i++)
                        {
                            if (linkMsgs[i].StartsWith("itemid:"))
                            {
                                itemThisId = uint.Parse(linkMsgs[i].Replace("itemid:", ""));
                            }
                            else if (linkMsgs[i].StartsWith("quality:"))
                            {
                                quality = uint.Parse(linkMsgs[i].Replace("quality:", ""));
                            }
                            else if (linkMsgs[i].StartsWith("type"))
                            {
                                type = int.Parse(linkMsgs[i].Replace("type:", ""));
                            }
                        }
                        if(type == 0&&quality == 0&&itemThisId == 0)
                        {
                            return false;
                        }
                        index = strlink.IndexOf('[');
                        int endIndex = strlink.IndexOf(']');
                        int len = endIndex - index - 1;
                        if(strlink.Length > len)
                        {
                            itemName = strlink.Substring(index + 1, endIndex - index - 1);
                        }
                    }
                    return true;
                }
            }
        }
        return false;
    }

    public List<string> GetHistoryMsg()
    {
        return HistoryMsgs;
        List<string> msgs = new List<string>();

        for (int i = 0; i < HistoryMsgs.Count; i++)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(HistoryMsgs[i]))
            {
                if (HistoryMsgs[i].Contains("<a href=\"") && HistoryMsgs[i].Contains("</a>"))
                {
                    string[] temp = HistoryMsgs[i].Split(new string[] { "</a>" }, System.StringSplitOptions.RemoveEmptyEntries);
                    for (int k = 0; k < temp.Length; k++)
                    {
                        int index = temp[k].IndexOf('[');
                        if (index != -1)
                        {
                            msg += temp[k].Substring(index);
                        }
                    }
                }
                else
                {
                    msg = HistoryMsgs[i];
                }
            }
            msgs.Add(msg);
        }

        return msgs;
    }


    #region 超链接
    /// <summary>
    /// 道具查看
    /// </summary>
    /// <param name="strData"></param>
    void OnItemLink(string[] strData)
    {
        if (strData.Length != 4)
        {
            Engine.Utility.Log.Error("OnItemLink传入链接字符串格式错误");
            return;
        }

        uint uid = 0;
        uint itemid = 0;
        int type = 0;
        for (int i = 0; i < strData.Length; i++)
        {
            if (strData[i].StartsWith("type:"))
            {
                type = int.Parse(strData[i].Replace("type:", ""));
            }
            else if (strData[i].StartsWith("userid:"))
            {
                uid = uint.Parse(strData[i].Replace("userid:", ""));
            }
            else if (strData[i].StartsWith("itid:"))
            {
                itemid = uint.Parse(strData[i].Replace("itid:", ""));
            }
        }

        if (uid != 0 && itemid != 0)
        {
            if (type == (int)LinkType.LinkItem)
            {
                NetService.Instance.Send(new GameCmd.stRequestUrlItemInfoChatUserCmd_C() { id = uid, dwThisID = itemid });
            }
            else if (type == (int)LinkType.LinkRide)
            {
                NetService.Instance.Send(new GameCmd.stRequestUrlRideInfoChatUserCmd_C() { uid = uid, ride_id = itemid });
            }
            else if (type == (int)LinkType.LinkPet)
            {
                NetService.Instance.Send(new GameCmd.stRequestUrlPetInfoChatUserCmd_C() { uid = uid, pet_id = itemid });
            }
        }
        else
        {
            UrlItem itemInfo = GetItemSerializeInfoByThisID(itemid);
            if (itemInfo.info != null)
            {

                if (itemInfo.lt == LinkType.LinkItem)
                {
                    table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemInfo.info.dwObjectID);
                    if (itemdb != null)
                    {
                        BaseItem baseItem = new BaseItem(itemInfo.info.dwObjectID, itemInfo.info);
                        //cmd.obj
                        TipsManager.Instance.ShowItemTips(baseItem);
                    }

                }
                else if (itemInfo.lt == LinkType.LinkPet || itemInfo.lt == LinkType.LinkRide)
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RidePetTipsPanel, data: itemInfo.info);
                }
                else
                {
                    Log.Error("未解析");
                }
            }
        }
    }

    void OnPlayerView(string[] strData)
    {
        if (strData.Length != 2)
        {
            Engine.Utility.Log.Error("OnPlayerView传入链接字符串格式错误");
            return;
        }

        uint uid = 0;
        for (int i = 1; i < strData.Length; i++)
        {
            if (strData[i].StartsWith("userid:"))
            {
                uid = uint.Parse(strData[i].Replace("userid:", ""));
            }
        }

        if (uid != 0)
        {
            DataManager.Instance.Sender.RequestPlayerInfoForOprate(uid, PlayerOpreatePanel.ViewType.Normal);
        }
    }

    void OnJoinTeam(string[] strData)
    {
        if (strData.Length != 2)
        {
            Engine.Utility.Log.Error("OnJoinTeam传入链接字符串格式错误");
            return;
        }

        uint uid = 0;
        for (int i = 1; i < strData.Length; i++)
        {
            if (strData[i].StartsWith("uid:"))
            {
                uid = uint.Parse(strData[i].Replace("uid:", ""));
            }
        }

        if (uid != 0)
        {
            DataManager.Manager<TeamDataManager>().ReqJoinTeam(uid);
        }
    }
    void OnBossRresh(string[] strData)
    {
        if (strData.Length != 2)
        {
            Engine.Utility.Log.Warning("OnBossRresh传入链接字符串格式错误");
        }

        uint uid = 0;
        for (int i = 1; i < strData.Length; i++)
        {
            if (strData[i].StartsWith("id:"))
            {
                uid = uint.Parse(strData[i].Replace("id:", ""));
            }
        }
        ItemManager.DoJump(uid);
    }

    void OnJoinClan(string[] strData)
    {
        if (strData.Length != 2)
        {
            Engine.Utility.Log.Warning("OnJoinClan传入链接字符串格式错误");
        }

        uint uid = 0;
        for (int i = 1; i < strData.Length; i++)
        {
            if (strData[i].StartsWith("id:"))
            {
                uid = uint.Parse(strData[i].Replace("id:", ""));
            }
        }

        if (uid != 0)
        {
            DataManager.Manager<ClanManger>().JoinClan(uid);
        }

    }
    #endregion
    public enum LinkType
    {
        LinkPlayer = 0,
        LinkItem = 1,
        AddFriend = 2,//私聊加好友链接
        JoinTeam = 3,  //加入组队
        JoinClan = 4,  //加入氏族   
        LinkPet = 5,    //宠物
        LinkRide = 6,   //坐骑
        LinkBoss = 7,     //boos刷新
    }

    public static string GetPlayerHrefString(string strName, uint userid, Cmd.GXColor color = Cmd.GXColor.Yellow)
    {
        var colorYellow = string.Format("#{0:X8}", (uint)color);
        string strRet;
        if (string.IsNullOrEmpty(strName))
        {
            strRet = string.Format("<color value=\"{4}\"><a href=\"BBCode:{0}type:{1},userid:{2}\">{3}</a></color>",
                      0,
                      (int)LinkType.LinkPlayer,
                      userid,
                      strName,
                      colorYellow
                      );
        }
        else
        {
            strRet = string.Format("<color value=\"{4}\"><a href=\"BBCode:{0}type:{1},userid:{2}\">{3}:</a></color>",
                      0,
                      (int)LinkType.LinkPlayer,
                      userid,
                      strName,
                      colorYellow
                      );
        }
        return strRet;
    }
    static int GetColorByQuality(uint quality)
    {
        int color = (255 << 24) | (198 << 16) | (198 << 8) | (198 << 0);
        if (quality == 0)
        {
            color = (255 << 24) | (198 << 16) | (198 << 8) | (198 << 0);
        }
        else if (quality == 1)
        {
            color = (255 << 24) | (194 << 16) | (186 << 8) | (151 << 0);
        }
        else if (quality == 2)
        {
            color = (255 << 24) | (34 << 16) | (221 << 8) | (34 << 0);
        }
        else if (quality == 3)
        {
            color = (255 << 24) | (92 << 16) | (180 << 8) | (255 << 0);
        }
        else if (quality == 4)
        {
            color = (255 << 24) | (230 << 16) | (122 << 8) | (255 << 0);
        }
        else if (quality == 5)
        {
            color = (255 << 24) | (255 << 16) | (186 << 8) | (0 << 0);
        }
        return color;
    }
    public static string GetItemHrefString(string strShowName, uint userid, uint itemId, uint quality, bool underLine = false, int type = 1)
    {
        var strcolor = string.Format("#{0:X8}", GetColorByQuality(quality));

        LinkType linkType = LinkType.LinkItem;
        // //1 物品 2 坐骑 3 宠物
        if (type == 1)
        {
            linkType = LinkType.LinkItem;
        }
        else if (type == 2)
        {
            linkType = LinkType.LinkRide;
        }
        else if (type == 3)
        {
            linkType = LinkType.LinkPet;
        }

        string strRet = string.Format("<color value=\"{6}\"><a href=\"BBCode:{0}type:{1},userid:{2},itid:{3},quality:{4}\">{5}</a></color>",
            underLine ? "1" : "0",
            (int)linkType,
            userid,
            itemId,
            quality,
            strShowName,
            strcolor);

        return strRet;
    }

    public static string GetAddFriendHrefString(string strRequestName, uint nRequestUserId)
    {
        string strRet = string.Format("{0}添加您为好友<a href=\"BBCode:{1}type:{2},userid:{3}\">添加好友</a>", strRequestName, 1, (int)LinkType.AddFriend, nRequestUserId);
        return strRet;
    }
    /// <summary>
    /// 显示到系统频道的信息
    /// </summary>
    /// <param name="txt">要显示的纯文本</param>
    /// <param name="userid">发送目标id 自己不用填</param>
    /// <param name="linkThisId">超链接物品Thisid</param>
    /// <param name="linkName">物品名字</param>
    /// <param name="quality">品质</param>
    /// <param name="underLine">是否添加下划线</param>
    /// <param name="linkType">超链接类型</param>
    /// <returns></returns>
    public static void SendToChatSystem(string localtxt, uint userid = 0, uint linkThisId = 0, string linkName = "", uint quality = 1, bool underLine = false, LinkType linkType = LinkType.LinkItem)
    {
        string txt = localtxt;
        string linkTxt = "";
        userid = userid == 0 ? MainPlayerHelper.GetPlayerID() : userid;
        if (linkThisId != 0)
        {
            linkName = "[" + linkName + "]";
            var strcolor = string.Format("#{0:X8}", GetColorByQuality(quality));
            string strRet = string.Format("<color value=\"{6}\"><a href=\"BBCode:{0}type:{1},userid:{2},itid:{3},quality:{4}\">{5}</a></color>",
           underLine ? "1" : "0",
           (int)linkType,
           userid,
           linkThisId,
           quality,
           linkName,
           strcolor);
            linkTxt = string.Format(txt, strRet);
        }
        else
        {
            linkTxt = txt;
        }
        stWildChannelCommonChatUserCmd_CS cmd = new stWildChannelCommonChatUserCmd_CS();
        cmd.byChatType = CHATTYPE.CHAT_SYS;
        cmd.szInfo = linkTxt;
        cmd.dwOPDes = 0;
        cmd.timestamp = (uint)DateTimeHelper.Instance.Now;
        ChatChannel channel = DataManager.Manager<ChatDataManager>().GetChannelByType(CHATTYPE.CHAT_SYS);
        if (channel != null)
        {
            channel.Add(channel.ToChatInfo(cmd));
        }
        DataManager.Manager<ChatDataManager>().PrivateChatManager.AddChat(new GameCmd.stCommonMChatUserCmd_CS()
        {
            szInfo = txt,
            byChatType = CHATTYPE.CHAT_SYS,
            dwOPDes = 0,
            szOPDes = "系统",
        });

    }
    public struct UrlItem
    {
        public ItemSerialize info;
        public LinkType lt;
    }
    Dictionary<uint, UrlItem> m_dicItemSerializeInfo = new Dictionary<uint, UrlItem>();

    public UrlItem GetItemSerializeInfoByThisID(uint thisID)
    {
        if (m_dicItemSerializeInfo.ContainsKey(thisID))
        {
            return m_dicItemSerializeInfo[thisID];
        }
        return new UrlItem();
    }
    /// <summary>
    /// 根据物品列表获取富文本
    /// </summary>
    /// <param name="itemList">物品列表</param>
    /// <param name="type"> (0:物品 1:战魂 2:坐骑)</param>
    public string GetRichTextByIteminfos(List<ItemSerialize> itemList, uint type)
    {
        string richTxt = "";
        for (int i = 0; i < itemList.Count; i++)
        {
            ItemSerialize info = itemList[i];
            uint thisID = info.qwThisID;

            uint qua = 1;
            string name = "";
            LinkType lt = LinkType.LinkItem;
            if (type == 0)
            {
                table.ItemDataBase item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(info.dwObjectID);
                if (item != null)
                {
                    qua = item.quality;
                    name = item.itemName;
                    lt = LinkType.LinkItem;
                }
            }
            else if (type == 1)
            {
                table.PetDataBase pdb = GameTableManager.Instance.GetTableItem<table.PetDataBase>(info.dwObjectID);
                if (pdb != null)
                {
                    ItemDataBase idb = GameTableManager.Instance.GetTableItem<ItemDataBase>(pdb.ItemID);
                    if (idb != null)
                    {
                        qua = idb.quality;
                    }
                    name = pdb.petName;
                    lt = LinkType.LinkPet;
                }
            
            }
            else if (type == 2)
            {
                table.RideDataBase rdb = GameTableManager.Instance.GetTableItem<table.RideDataBase>(info.dwObjectID);
                if (rdb != null)
                {
                    ItemDataBase idb = GameTableManager.Instance.GetTableItem<ItemDataBase>(rdb.sealItemID);
                    if (idb != null)
                    {
                        qua = idb.quality;
                    }
                    name = rdb.name;
                    lt = LinkType.LinkRide;
                }
            }
            UrlItem ui = new UrlItem();
            ui.info = info;
            ui.lt = lt;
            if (thisID != 0)
            {
                if (!m_dicItemSerializeInfo.ContainsKey(thisID))
                {
                    m_dicItemSerializeInfo.Add(thisID, ui);
                }
                else
                {
                    m_dicItemSerializeInfo[thisID] = ui;
                }
                
                //BaseItem item = new BaseItem(info.dwObjectID, info);
                //qua = (uint)item.QualityType;
            }
            if (thisID != 0)
            {
                name = "[" + name + "]";
                var strcolor = string.Format("#{0:X8}", GetColorByQuality(qua));
                string strRet = string.Format("<color value=\"{6}\"><a href=\"BBCode:{0}type:{1},userid:{2},itid:{3},quality:{4}\">{5}</a></color>",
               "0",
               (int)lt,
               0,
               thisID,
               qua,
               name,
               strcolor);
                richTxt += strRet;
            }

        }
        return richTxt;
    }
    ClientChannelGroup GetShowChannel(uint group)
    {

        if ((group & (int)ClientChannelGroup.SystemChannel) == (int)ClientChannelGroup.SystemChannel)
        {
            return ClientChannelGroup.SystemChannel;
        }
        if ((group & (int)ClientChannelGroup.Team) == (int)ClientChannelGroup.Team)
        {
            return ClientChannelGroup.Team;
        }
        else if ((group & (int)ClientChannelGroup.Clan) == (int)ClientChannelGroup.Clan)
        {
            return ClientChannelGroup.Clan;
        }
        else if ((group & (int)ClientChannelGroup.Near) == (int)ClientChannelGroup.Near)
        {
            return ClientChannelGroup.Near;
        }
        else if ((group & (int)ClientChannelGroup.GodEvil) == (int)ClientChannelGroup.GodEvil)
        {
            return ClientChannelGroup.GodEvil;

        }
        else if ((group & (int)ClientChannelGroup.Light) == (int)ClientChannelGroup.Light)
        {
            return ClientChannelGroup.Light;
        }
        else if ((group & (int)ClientChannelGroup.Tips) == (int)ClientChannelGroup.Tips)
        {
            return ClientChannelGroup.Tips;
        }
        else if ((group & (int)ClientChannelGroup.World) == (int)ClientChannelGroup.World)
        {
            return ClientChannelGroup.World;
        }
        return ClientChannelGroup.None;
    }
    CHATTYPE GetChatType(ClientChannelGroup group)
    {
        if (group == ClientChannelGroup.Clan)
        {
            return CHATTYPE.CHAT_CLAN;
        }
        else if (group == ClientChannelGroup.GodEvil)
        {
            return CHATTYPE.CHAT_DEMON;
        }
        else if (group == ClientChannelGroup.Near)
        {
            return CHATTYPE.CHAT_MAP;
        }
        else if (group == ClientChannelGroup.SystemChannel)
        {
            return CHATTYPE.CHAT_SYS;
        }
        else if (group == ClientChannelGroup.Team)
        {
            return CHATTYPE.CHAT_TEAM;
        }
        else if (group == ClientChannelGroup.World)
        {
            return CHATTYPE.CHAT_WORLD;
        }

        return CHATTYPE.CHAT_NONE;
    }
    public void SeverSendToChatSystem(stSendInfoReminderChatUserCmd_S cmd, table.LangTextDataBase langText, object[] objs)
    {
        if (langText != null)
        {

            if (objs != null)
            {
                if (objs.Length > 0)
                {
                    if (objs != null)
                    {
                        string msg = string.Empty;
                        try
                        {
                            msg = string.Format(langText.strText, objs);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("string format 参数长度不够 检查 {0} /n {1}", langText.strText, ex.ToString());

                            return;
                        }
                        string richText = DataManager.Manager<ChatDataManager>().GetRichTextByIteminfos(cmd.obj, cmd.item_type);
                        string endTxt = msg.Replace("#item#", richText);


                        foreach (var evalue in Enum.GetValues(typeof(ClientChannelGroup)))
                        {
                            ClientChannelGroup group = (ClientChannelGroup)evalue;
                            if (((int)group & langText.channelGroup) != (int)group)
                            {
                                continue;
                            }
                            if (group == ClientChannelGroup.Tips)
                            {
                                if (objs != null)
                                {
                                    TipsManager.Instance.ShowTips(endTxt);
                                }
                                else
                                {
                                    TipsManager.Instance.ShowTips(langText.strText);
                                }
                            }
                            if (group == ClientChannelGroup.Light)
                            {
                                //跑马灯
                                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RunLightPanel, data: new RunLightInfo()
                                {
                                    pos = RunLightInfo.Pos.Top,
                                    msg = endTxt,
                                    showTime = 1,
                                });
                            }
                            CHATTYPE chatType = GetChatType(group);
                            if (chatType == CHATTYPE.CHAT_NONE)
                            {
                                continue;
                            }
                            stWildChannelCommonChatUserCmd_CS wild = new stWildChannelCommonChatUserCmd_CS();
                            wild.byChatType = chatType;
                            wild.szInfo = endTxt;
                            wild.dwOPDes = 0;
                            wild.timestamp = (uint)DateTimeHelper.Instance.Now;

                            ChatChannel channel = DataManager.Manager<ChatDataManager>().GetChannelByType(chatType);
                            if (channel != null)
                            {
                                channel.Add(channel.ToChatInfo(wild));
                            }

                        }

                    }
                }
                else
                {
                    foreach (var evalue in Enum.GetValues(typeof(ClientChannelGroup)))
                    {
                        ClientChannelGroup group = (ClientChannelGroup)evalue;
                        string richText = DataManager.Manager<ChatDataManager>().GetRichTextByIteminfos(cmd.obj, cmd.item_type);
                        string endTxt = langText.strText.Replace("#item#", richText);
                        if (group == ClientChannelGroup.Light)
                        {
                            //跑马灯
                            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RunLightPanel, data: new RunLightInfo()
                            {
                                pos = RunLightInfo.Pos.Top,
                                msg = endTxt,
                                showTime = 1,
                            });
                        }
                        else if (group == ClientChannelGroup.Tips)
                        {
                            TipsManager.Instance.ShowTips(langText.strText);
                        }
                    }
                }

            }
            else
            {
                foreach (var evalue in Enum.GetValues(typeof(ClientChannelGroup)))
                {
                    ClientChannelGroup group = (ClientChannelGroup)((int)evalue & (int)langText.channelGroup);
                    if (group == ClientChannelGroup.Tips)
                    {
                        TipsManager.Instance.ShowTips(langText.strText);
                    }

                    CHATTYPE chatType = GetChatType(group);
                    if (chatType == CHATTYPE.CHAT_NONE)
                    {
                        continue;
                    }
                    stWildChannelCommonChatUserCmd_CS wild = new stWildChannelCommonChatUserCmd_CS();
                    wild.byChatType = chatType;
                    wild.szInfo = langText.strText;
                    wild.dwOPDes = 0;
                    wild.timestamp = (uint)DateTimeHelper.Instance.Now;

                    ChatChannel channel = DataManager.Manager<ChatDataManager>().GetChannelByType(chatType);
                    if (channel != null)
                    {
                        channel.Add(channel.ToChatInfo(wild));
                    }

                }
            }
        }
    }

    #region 黑名单
    /// <summary>
    /// 是否满足发送聊天信息
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="nickName"></param>
    /// <param name="notMatchShowTip"></param>
    /// <returns></returns>
    public static bool CanSendChatMsgWithBlack(uint uid, string nickName, bool notMatchShowTip = false)
    {
        if (DataManager.Manager<RelationManager>().IsMyBlack(uid))
        {
            if (!string.IsNullOrEmpty(nickName) && notMatchShowTip)
            {
                string tipsMsg = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_InMyBlack, nickName);
                TipsManager.Instance.ShowTips(tipsMsg);
            }
            return false;
        }
        return true;
    }

    public static bool CanRecieveChatMsgWithBlack(uint uid)
    {
        if (DataManager.Manager<RelationManager>().IsMyBlack(uid))
        {
            return false;
        }
        return true;
    }

    public static void Siled(uint uid)
    {

    }
    #endregion
}
