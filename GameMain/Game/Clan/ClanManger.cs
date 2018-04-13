/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.Game.Clan
 * 文件名：  ClanManger
 * 版本号：  V1.0.0.0
 * 唯一标识：5ee07605-10d8-4c90-90fb-cdad4ebdaef8
 * 当前的用户域：wenjunhua.zqgame
 * 电子邮箱：mcking_wen@163.com
 * 创建时间：10/8/2016 10:04:54 AM
 * 描述：
 ************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Client;
using table;

partial class ClanManger : BaseModuleData, IManager
{
    #region GolbalConst

    //氏族荣誉最多保存数量
    public const string CONST_MAX_HONOR_KEEP_NUM_NAME = "ClanHonorMaxKeepNum";
    public static uint ClanHonorMaxKeepNum
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_MAX_HONOR_KEEP_NUM_NAME);
        }
    }

    //发布氏族置顶公告消耗的物品id
    public const string CONST_PUBLIC_CLAN_TOP_GG_COSTITEMID_NAME = "PublicClanTopGGCostItemId";
    public static uint PublicClanTopGGCostItemId
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_PUBLIC_CLAN_TOP_GG_COSTITEMID_NAME);
        }
    }

    //氏族最大人数
    public  uint MaxClanMembers
    {
        get
        {
            uint clanLv = 1;
            uint maxMemberCount = 0;
            if (m_clanInfo != null)
            {
                clanLv = m_clanInfo.Lv;             
            }
            ClanMemberDataBase data = GameTableManager.Instance.GetTableItem<ClanMemberDataBase>(clanLv);
            if (data != null)
            {
                maxMemberCount = data.memberNum;
            }
            return maxMemberCount;
        }
    }

    //氏族功能开启等级，功能开启后可以创建氏族以及加入氏族
    public const string CONST_CLAN_UNLOCK_LEVEL_NAME = "ClanUnlockLevel";
    public static uint ClanUnlockLevel
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_CLAN_UNLOCK_LEVEL_NAME);
        }
    }

    //创建氏族消耗的金元宝
    public const string CONST_BUILD_CLAN_COST_GOLD_NAME = "BuildClanCostCopper";
    public static uint BuildClanCostGold
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_BUILD_CLAN_COST_GOLD_NAME);
        }
    }

    //创建氏族消耗的铜钱
    public const string CONST_BUILD_CLAN_COST_COPPER_NAME = "BuildClanCost";
    public static uint BuildClanCostCopper
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_BUILD_CLAN_COST_COPPER_NAME);
        }
    }

    //临时氏族存在最长时间(分钟)
    public const string CONST_TEMP_CLAN_LASTTIME_NAME = "TempClanLastTime";
    public static float TempClanLastMinute
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>(CONST_TEMP_CLAN_LASTTIME_NAME);
        }
    }

    //临时氏族转正所需要支持者人数
    public const string CONST_TEMP_CLAN_SUPPORTER_NAME = "TempClanSupporter";
    public static uint TempClanSupporter
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_TEMP_CLAN_SUPPORTER_NAME);
        }
    }

    //离开氏族后，扣除声望的万分比
    public const string CONST_QUIT_CLAN_REPUTATION_TAX_NAME = "QuitClanReputationTax";
    public static float QuitClanReputationTax
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>(CONST_QUIT_CLAN_REPUTATION_TAX_NAME) / 10000f;
        }
    }

    //可被转让氏族长所需加入氏族的时间（小时），包括弹劾发起者所需加入氏族时间
    public const string CONST_GET_CLAN_SURVEY_TIME_NAME = "GetClanSurveyTime";
    public static float GetClanSurveyTime
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>(CONST_GET_CLAN_SURVEY_TIME_NAME);
        }
    }

    //族长离线天数，达到该天数则可被弹劾
    public const string CONST_PATRIALCH_OFFLINE_DAY_NAME = "PatriarchOfflineDay";
    public static uint PatriarchOfflineDay
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<uint>(CONST_PATRIALCH_OFFLINE_DAY_NAME);
        }
    }

    //群发消息的冷却时间(秒)
    public const string CONST_CLAN_MSG_CD_NAME = "ClanMessageCD";
    public static float ClanMessageCD
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<float>(CONST_CLAN_MSG_CD_NAME);
        }
    }

    //    宣战发起所需最低的职介
    public const string CONST_CLAN_WAR_NEED_DUTY = "ClanWarCostJob";
    public static int ClanDeclareWarNeedDuty
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(CONST_CLAN_WAR_NEED_DUTY);
        }
    }
    //宣战所需氏族资金
    public const string CONST_CLAN_WAR_NEED_ZJ = "ClanWarCostGold";
    public static int ClanDeclareWarNeedZJ
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(CONST_CLAN_WAR_NEED_ZJ);
        }
    }
    //宣战所需族贡
    public const string CONST_CLAN_WAR_NEED_ZG = "ClanWarCostContribute";
    public static int ClanDeclareWarNeedZG
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(CONST_CLAN_WAR_NEED_ZG);
        }
    }

    //战争历史数量
    public const string CONST_CLAN_WAR_HISTORY_NUM = "ClanWarHistoryNum";
    public static int ClanDeclareHistoryNum
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(CONST_CLAN_WAR_HISTORY_NUM);
        }
    }

    //战争持续时间小时
    public const string CONST_CLAN_WAR_DUR = "ClanWarTime";
    public static int ClanDeclareWarDur
    {
        get
        {
            int dur = GameTableManager.Instance.GetGlobalConfig<int>(CONST_CLAN_WAR_DUR);
            return (int)(dur / 3600f);

        }
    }
    //--氏族自动拒绝邀请延迟（单位：秒）
    public const string CONST_CLAN_AUTO_REFUSE_DELAY = "ClanAutoRefuseDelay";
    public static int ClanAutoRefuseDelay
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(CONST_CLAN_AUTO_REFUSE_DELAY);
        }
    }
    //--氏族本地存储邀请消息上限
    public const string CONST_STORE_INVITE_MSG_NUM = "ClanStoreInviteMsgNum";
    public static int ClanStoreInviteMsgNum
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(CONST_STORE_INVITE_MSG_NUM);
        }
    }
    #endregion

    #region Clan Common


    /// <summary>
    /// 角色身上的氏族id
    /// </summary>
    public uint ClanId
    {
        get
        {
            //int clanId = (int)((null != DataManager.Instance.MainPlayer)
            //    ? DataManager.Instance.MainPlayer.GetProp((int)CreatureProp.ClanId) : 0);
            if (null != DataManager.Instance.MainPlayer)
            {
               uint clanIdLow = (uint)DataManager.Instance.MainPlayer.GetProp((int)CreatureProp.ClanIdLow);
               uint clanIdHigh = (uint)DataManager.Instance.MainPlayer.GetProp((int)CreatureProp.ClanIdHigh);
               uint newClanId = (clanIdHigh << 16) | clanIdLow;
               return newClanId;
            }
            else 
            {
                return 0;
            }
        }
    }

    //缓存服务端氏族信息
    private Dictionary<uint, ClanDefine.LocalClanInfo> m_dic_clanInfos = null;
    //缓存服务端过滤氏族信息
    private Dictionary<uint, ClanDefine.LocalClanInfo> m_dic_clanFilterInfos = null;
    //当前氏族信息
    private ClanDefine.LocalClanInfo m_clanInfo = null;
    public ClanDefine.LocalClanInfo ClanInfo
    {
        get
        {
            return (IsJoinClan) ? m_clanInfo : null;
        }
    }
    //氏族功能是否开启
    public bool IsClanEnable
    {
        get
        {
            return DataManager.Instance.PlayerLv >= ClanUnlockLevel ? true : false;
        }
    }

    //是否加入氏族
    public bool IsJoinClan
    {
        get
        {
            return (ClanId != 0) ? true : false;
        }
    }

    //当前氏族创建者
    public uint ClanCreator
    {
        get
        {
            return (null != ClanInfo) ? ClanInfo.ShaikhId : 0;
        }
    }

    /// <summary>
    /// 当前氏族创建者是否为自己
    /// </summary>
    public bool IsClanCreatorSelf
    {
        get
        {
            return (IsJoinClan && (ClanCreator == DataManager.Instance.UserId)) ? true : false;
        }
    }

    //是否加入正式氏族
    public bool IsJoinFormal
    {
        get
        {
            return (null != ClanInfo && ClanInfo.IsFormal) ? true : false;
        }
    }

    //族长
    private GameCmd.stClanMemberInfo m_Shaikh = null;
    public GameCmd.stClanMemberInfo Shaikh
    {
        get
        {
            return m_Shaikh;
        }
    }

    //我的氏族信息
    //private GameCmd.stClanMemberInfo m_myClanInfo;
    public GameCmd.stClanMemberInfo MyClanInfo
    {
        get
        {
            if (null != ClanInfo)
            {
                stClanMemberInfo me = ClanInfo.GetMemberInfo(DataManager.Instance.UserId);
                return me;
            }
            return null;
        }
    }


    //自动拒绝邀请
    private bool m_bool_autoRefuseInvite = false;
    public bool AutoRefuseInvite
    {
        get
        {
            return m_bool_autoRefuseInvite;
        }
        set
        {
            if (value != m_bool_autoRefuseInvite)
            {
                m_bool_autoRefuseInvite = value;
            }
        }
    }
    //是否显示氏族名称
    private bool m_bool_showClanName = false;
    public bool ShowClanName
    {
        get
        {
            return m_bool_showClanName;
        }
        set
        {
            if (value != m_bool_showClanName)
            {
                m_bool_showClanName = value;
            }
        }
    }
    /// <summary>
    /// 是否两个玩家为同一氏族
    /// </summary>
    /// <param name="player1">玩家1</param>
    /// <param name="player2">玩家2</param>
    /// <returns></returns>
    public static bool IsSameClan(IPlayer player1, IPlayer player2)
    {
        if (null != player1 && null != player2)
        {
            uint player1clanIdLow = (uint)player1.GetProp((int)CreatureProp.ClanIdLow);
            uint player1clanIdHigh = (uint)player1.GetProp((int)CreatureProp.ClanIdHigh);
            uint player1newClanId = (player1clanIdHigh << 16) | player1clanIdLow;

            uint player2clanIdLow = (uint)player2.GetProp((int)CreatureProp.ClanIdLow);
            uint player2clanIdHigh = (uint)player2.GetProp((int)CreatureProp.ClanIdHigh);
            uint player2newClanId = (player2clanIdHigh << 16) | player2clanIdLow;

            return (player1newClanId != 0) && (player1newClanId == player2newClanId);

            //int clanId1 = player1.GetProp((int)Client.CreatureProp.ClanId);
            //return ((clanId1 != 0)
            //    && (clanId1 == player2.GetProp((int)Client.CreatureProp.ClanId)));
        }
        return false;
    }

    public static bool IsSameClan(ICreature creature1, ICreature creature2)
    {
        if (null != creature1 && null != creature2)
        {
            uint creature1clanIdLow = (uint)creature1.GetProp((int)CreatureProp.ClanIdLow);
            uint creature1clanIdHigh = (uint)creature1.GetProp((int)CreatureProp.ClanIdHigh);
            uint creature1newClanId = (creature1clanIdHigh << 16) | creature1clanIdLow;

            uint creature2clanIdLow = (uint)creature2.GetProp((int)CreatureProp.ClanIdLow);
            uint creature2clanIdHigh = (uint)creature2.GetProp((int)CreatureProp.ClanIdHigh);
            uint creature2newClanId = (creature2clanIdHigh << 16) | creature2clanIdLow;

            return (creature1newClanId != 0) && (creature1newClanId == creature2newClanId);

            //int clanId1 = creature1.GetProp((int)Client.CreatureProp.ClanId);
            //return ((clanId1 != 0)
            //    && (clanId1 == creature2.GetProp((int)Client.CreatureProp.ClanId)));
        }
        return false;
    }

    /// <summary>
    /// 玩家数据加载完成
    /// </summary>
    /// <param name="eventid"></param>
    /// <param name="data"></param>
    public void GlobalEventHandler(int eventid, object data)
    {
        switch (eventid)
        {
            case (int)GameEventID.PLAYER_LOGIN_SUCCESS:
            case (int)GameEventID.RECONNECT_SUCESS:
                {
                    //请求氏族详情
                    //是否加入氏族
                    if (IsJoinClan)
                    {
                        GetClanInfoReq(ClanId, true);
                        //请求氏族敌对关系
                        SendGetClanRivalryInfos();
                    }
                }
                break;
            case (int)GameEventID.TASK_DONING:
                {
                    Client.stDoingTask taskDoing = (Client.stDoingTask)data;
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, taskDoing.taskid);
                }
                break;
            case (int)GameEventID.TASK_CANSUBMIT:
                {
                    Client.stTaskCanSubmit taskSubmit = (Client.stTaskCanSubmit)data;
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, taskSubmit.taskid);
                }
                break;
            case (int)GameEventID.TASK_DONE:
                {
                    Client.stTaskDone taskDone = (Client.stTaskDone)data;
                    if (m_dic_clanTaskInfos.ContainsKey(taskDone.taskid))
                    {
                        CompleteClanTask(taskDone.taskid);
                    }
                }
                break;
            case (int)GameEventID.TASK_STATECHANEGE:
                {
                    Client.stDoingTask taskState = (Client.stDoingTask)data;
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, taskState.taskid);
                }
                break;
        }

    }

    /// <summary>
    /// 找到氏族中族长数据
    /// </summary>
    /// <returns></returns>
    public GameCmd.stClanMemberInfo FindClanShaikh()
    {
        GameCmd.stClanMemberInfo shaikh = null;
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null != clanInfo)
        {
            shaikh = clanInfo.GetMemberInfo(clanInfo.ShaikhId);
        }
        return shaikh;
    }

    /// <summary>
    /// 创建氏族
    /// </summary>
    /// <param name="clanName">氏族名称</param>
    ///  <param name="clanName">公告</param>
    public void CreateClan(string clanName, string msg)
    {
        TextManager tmgr = DataManager.Manager<TextManager>();
        if (IsJoinClan)
        {
            TipsManager.Instance.ShowTips("你已经是氏族成员了");
            return;
        }
        if (!tmgr.IsLegalNameFormat(tmgr.GetLocalText(LocalTextType.Local_TXT_Clan), clanName
            , TextManager.CONST_NAME_MIN_WORDS
            , TextManager.CONST_NAME_MAX_WORDS
            , true))
        {
            return;
        }
        clanName = TextManager.RemoveAllSpace(clanName);

        if (!string.IsNullOrEmpty(msg))
        {
            List<string> txts = null;
            StringBuilder builder = null;
            if (TextManager.IsMatchNGUIColor(msg, out txts))
            {
                builder = new StringBuilder();
                for (int i = 0; i < txts.Count; i++)
                {
                    builder.Append(string.Format(TextManager.CONST_TAG_WORDS_FORMAT, txts[i].Substring(1, txts[i].Length - 2)));
                    if (i < txts.Count - 1)
                    {
                        builder.Append(",");
                    }
                }
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Local_TXT_Warning_FM_NGUIColor
                    , tmgr.GetLocalText(LocalTextType.Local_TXT_Announcement)
                    , builder.ToString());
                return;
            }

            uint charNum = TextManager.GetCharNumByStrInUnicode(msg);
            if (charNum > TextManager.TransformWordNum2CharNum(ClanDefine.CONST_CLAN_GG_MAX_WORDS))
            {
                TipsManager.Instance.ShowTips(tmgr.GetLocalFormatText(LocalTextType.Local_TXT_ClanGGLimit));
                return;
            }
            if (!TextManager.IsLegalText(msg))
            {
                TipsManager.Instance.ShowTips(tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Warning_FM_IllegalChar
                    , tmgr.GetLocalText(LocalTextType.Local_TXT_Announcement)));
                return;
            }
            if (DataManager.Manager<TextManager>().IsContainSensitiveWord(msg, TextManager.MatchType.Max))
            {
                TipsManager.Instance.ShowTips(tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Warning_FM_Sensitive
                    , tmgr.GetLocalText(LocalTextType.Local_TXT_Announcement)));
                return;
            }
        }
        else
        {
            msg = "";
        }
        //1、氏族功能是否开启
        //2、不是氏族成员并且为支持临时氏族
        //3、名称敏感词长度检查，是否已存在
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.CreateClanReq(clanName, msg);
    }



    /// <summary>
    /// 创建氏族响应
    /// </summary>
    /// <param name="ret">创建状态</param>
    /// <param name="info">氏族信息</param>
    public void OnCreateClan(GameCmd.enumClanCreateRet ret, stClanInfo info)
    {
        string tips = "";
        switch (ret)
        {
            case enumClanCreateRet.CR_Success:
                {
                    tips = string.Format("恭喜你，创建临时氏族{0}", info.name);
                    UpdatePlayerClanInfo(info);
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANCREATE, null);
                    if (!m_dic_clanInfos.ContainsKey(info.id))
                    {
                        m_dic_clanInfos.Add(ClanId, ClanInfo);
                    }
                }
                break;
        }
        if (!string.IsNullOrEmpty(tips))
        {
            TipsManager.Instance.ShowTips(tips);
        }
    }

    /// <summary>
    /// 氏族转正
    /// </summary>
    /// <param name="ret">状态</param>
    /// <param name="info">氏族信息</param>
    public void OnFormalClan(uint ret, stClanInfo info)
    {
        string tips = "";
        if (ret == 0)
        {
            //失败
            tips = "氏族转正失败";
            if (IsJoinClan && !IsJoinFormal)
            {
                UpdatePlayerClanInfo(null);
            }
        }
        else if (ret == 1)
        {
            //成功
            tips = string.Format("恭喜，临时氏族{0}转为正式氏族", info.name);
            UpdatePlayerClanInfo(info);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CLANJOIN,
                new Client.stClanJoin() { uid = MainPlayerHelper.GetPlayerUID(), clanName = info.name });
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ClanCreatePanel))
            {
                DataManager.Manager<UIPanelManager>().GetPanel(PanelID.ClanCreatePanel).HideSelf();
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTPOSITIVECLAN);
        }
        if (!string.IsNullOrEmpty(tips))
        {
            TipsManager.Instance.ShowTips(tips);
        }
    }

    /// <summary>
    /// 支持氏族
    /// </summary>
    /// <param name="clanId"></param>
    public void SupportClan(uint clanId)
    {
        //1、氏族功能是否开启
        //2、是否已经为氏族成员
        //3、是否已有支持过氏族
        JoinClan(clanId);
    }

    /// <summary>
    /// 取消支持当前氏族
    /// </summary>
    public void CancelSupportClan()
    {
        if (!IsJoinClan)
        {
            TipsManager.Instance.ShowTips("你还没有支持过氏族无法取消");
            return;
        }

        if (IsJoinFormal)
        {
            TipsManager.Instance.ShowTips("你已经加入正式氏族无法取消支持");
            return;
        }

        QuitClan();
    }

    /// <summary>
    /// 氏族解散
    /// </summary>
    /// <param name="clanId">氏族id</param>
    /// <param name="isFormal">是否为正式氏族</param>
    public void OnDissolveClan(uint clanId, bool isFormal)
    {
        bool isMyClanId = (clanId == ClanId) ? true : false;
        if (isMyClanId)
        {
            string tips = (isFormal) ? "氏族{0}已解散" : "临时氏族{0}已解散";
            TipsManager.Instance.ShowTips(string.Format(tips, ((null != ClanInfo) ? ClanInfo.Name : "")));
            UpdatePlayerClanInfo(null);
            m_dic_clanInfos.Remove(clanId);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDISSOLVECLAN, isFormal);
            ResetDeclareWar();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CLANQUIT, new Client.stClanQuit() { uid = MainPlayerHelper.GetPlayerUID() });
        }
        else
        {
            Engine.Utility.Log.Error("Clan Dissolve Id={0}", clanId);
        }
    }

    /// <summary>
    /// 快速加入氏族
    /// </summary>
    public void QuickJoinClan()
    {
        if (IsJoinClan)
        {
            TipsManager.Instance.ShowTips("你已经加入或支持了氏族，无法快速加入");
            return;
        }
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.QuickJoinClan();
        }
    }

    /// <summary>
    /// 快速加入
    /// </summary>
    /// <param name="ret"></param>
    public void OnQuickJoinClan(enumQuickJoinRet ret, uint num)
    {
        string tips = "";
        if (ret == enumQuickJoinRet.QJR_Success)
        {
            if (num != 0)
            {
                tips = string.Format("你已经成功向{0}个氏族发出申请", num);
            }
            else
            {
                tips = string.Format("没有可用的氏族", num);
            }
        }
        if (!string.IsNullOrEmpty(tips))
        {
            TipsManager.Instance.ShowTips(tips);
        }
    }

    /// <summary>
    /// 加入氏族
    /// </summary>
    /// <param name="clanId"></param>
    public void JoinClan(uint clanId)
    {
        //1、氏族功能是否开启
        //2、是否已经为氏族成员
        //3、是否已有支持过氏族
        if (IsJoinClan)
        {
            TipsManager.Instance.ShowTips("你已经加入或支持了氏族，无法重复加入");
            return;
        }
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.JoinClanReq(clanId);
    }



    /// <summary>
    /// 加入氏族响应
    /// </summary>
    /// <param name="ret">是否成功</param>
    /// <param name="info">氏族信息</param>
    public void OnJoinClan(enumClanJoinRet ret, stClanInfo info)
    {
        string tips = "";
        if (ret == enumClanJoinRet.JR_Success)
        {

            UpdatePlayerClanInfo(info);


            if (IsJoinFormal)
            {
                tips = string.Format("你成功加入氏族{0}", info.name);
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CLANJOIN,
                    new Client.stClanJoin() { uid = MainPlayerHelper.GetPlayerUID(), clanName = info.name });
            }
            else
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTJOINCLAN, IsJoinFormal);
                tips = string.Format("支持氏族{0}成功", info.name);
            }
        }
        if (!string.IsNullOrEmpty(tips))
        {
            TipsManager.Instance.ShowTips(tips);
        }
        if (IsJoinFormal)
        {
            ClanCreatePanel panel = DataManager.Manager<UIPanelManager>().GetPanel<ClanCreatePanel>(PanelID.ClanCreatePanel);
            if (null != panel && panel.isActiveAndEnabled)
            {
                panel.HideSelf();
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanPanel);
            }
        }

    }

    /// <summary>
    /// 申请加入氏族
    /// </summary>
    /// <param name="clanId"></param>
    public void ApplyJoinClan(uint clanId)
    {
        if (IsJoinClan)
        {
            TipsManager.Instance.ShowTips("你已经加入或支持了氏族，无法重复加入");
            return;
        }
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.RequestJoinClanReq(clanId);
    }

    /// <summary>
    /// 申请加入氏族响应
    /// </summary>
    /// <param name="clanId"></param>
    public void OnApplyJoinClan(enumClanJoinRet ret)
    {
        switch (ret)
        {
            case enumClanJoinRet.JR_Success:
                TipsManager.Instance.ShowTips("成功申请氏族");
                break;
        }
        Engine.Utility.Log.Info("Join Clan id:{0}", ret);
    }

    /// <summary>
    /// 玩家申请过的氏族列表
    /// </summary>
    private List<uint> m_list_userApplyClans = new List<uint>();
    /// <summary>
    /// 玩家申请的氏族列表
    /// </summary>
    /// <param name="applyClanList"></param>
    public void OnUserApplyClanList(List<uint> applyClanList)
    {
        m_list_userApplyClans.Clear();
        if (null != applyClanList && applyClanList.Count != 0)
        {
            m_list_userApplyClans.AddRange(applyClanList);
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTUSERAPPLYCLANLISTCHANGED);
    }

    /// <summary>
    /// 获取玩家申请过的氏族列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetUserApplyClans()
    {
        List<uint> datas = new List<uint>();
        if (!IsJoinClan)
        {
            datas.AddRange(m_list_userApplyClans);
        }
        else
        {
            m_list_userApplyClans.Clear();
        }
        return datas;
    }

    /// <summary>
    /// 是否申请过氏族
    /// </summary>
    /// <param name="clanId">氏族id</param>
    /// <returns></returns>
    public bool IsApplyClan(uint clanId)
    {
        if (IsJoinClan)
        {
            return false;
        }
        return m_list_userApplyClans.Contains(clanId);
    }

    /// <summary>
    /// 退出氏族
    /// </summary>
    public bool QuitClan()
    {
        if (!(IsJoinClan || IsJoinFormal))
        {
            TipsManager.Instance.ShowTips("你还没有加入氏族");
            return false;
        }
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null == clanInfo)
        {
            TipsManager.Instance.ShowTips("氏族信息错误");
            return false;
        }
        if (IsClanCreatorSelf && clanInfo.MemberCount > 1)
        {
            TipsManager.Instance.ShowTips("族长仅能在氏族氏族中仅剩自己时退出氏族喔！");
            return false;
        }
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.QuitClanReq();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 玩家退出氏族
    /// </summary>
    /// <param name="ret"></param>
    /// <param name="quitType">1:自己离开 2：逐出 3：解散</param>
    /// <param name="userId"></param>
    public void OnQuitClan(uint ret, uint quitType, uint userId)
    {
        string tips = "";
        if (ret == (uint)enumClanLeaveRet.LR_Success)
        {
            if (quitType == (uint)enumClanLeaveType.LT_Self)
            {
                if (IsJoinFormal)
                {
                    tips = "你已经退出氏族";
                }
                else
                {
                    tips = "成功取消支持";
                }

            }
            else if (quitType == (uint)enumClanLeaveType.LT_Kick)
            {
                tips = "你被逐出出氏族";
            }
            else if (quitType == (uint)enumClanLeaveType.LT_Dissolve)
            {
                tips = "氏族解散";
            }
            UpdatePlayerClanInfo(null);
            if (userId == MainPlayerHelper.GetPlayerID())
            {
                ResetDeclareWar();
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CLANQUIT,
                new Client.stClanQuit() { uid = MainPlayerHelper.GetPlayerUID() });
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTQUITCLAN, IsJoinFormal);
        }
        else if (ret == (uint)enumClanLeaveRet.LR_Error)
        {
            tips = "退出氏族失败";
        }
        if (!string.IsNullOrEmpty(tips))
        {
            TipsManager.Instance.ShowTips(tips);
        }
    }



    public void OnBroadCastClanMsg(string msg)
    {

    }

    /// <summary>
    /// 查询氏族信息
    /// </summary>
    /// <param name="type"></param>
    /// <param name="filterString"></param>
    /// <param name="page"></param>
    public void SearchClans(eGetClanType type, string filterString, uint page)
    {
        if (string.IsNullOrEmpty(filterString))
        {
            TipsManager.Instance.ShowTips("请输入要查询的氏族信息");
            return;
        }
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.SearchClan(type, filterString, page);
        }
    }

    ///查询列表响应
    /// </summary>
    /// <param name="clanInfos"></param>
    public void OnSearchClans(List<stClanInfo> clanInfos)
    {

    }

    /// <summary>
    /// 获取氏族信息请求
    /// </summary>
    /// <param name="clanId">氏族id</param>
    /// <param name="isDetail">是否为详情</param>
    public void GetClanInfoReq(uint clanId, bool isDetail)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.GetClanInfoReq(clanId, (uint)((isDetail) ? 1 : 0));
        }
    }



    /// <summary>
    /// 服务器下发氏族信息
    /// </summary>
    /// <param name="clanInfo">氏族信息</param>
    /// <param name="detail">是否为详情</param>
    public void OnGetClanInfo(stClanInfo clanInfo, bool detail)
    {
        //如果是玩家所在的氏族更新
        if (null != clanInfo)
        {
            if (clanInfo.id == ClanId)
            {
                //自己氏族
                ClanDefine.LocalClanInfo info = ClanInfo;
                if (null != info && !detail)
                {
                    clanInfo.memberlist = info.MemberList;
                }
                UpdatePlayerClanInfo(clanInfo);
            }

            ClanDefine.ClanNameData nameData = new ClanDefine.ClanNameData()
            {
                ClanName = clanInfo.name,
                ClanID = clanInfo.id,
            };
            CacheClanName(nameData);
            InvokeNameGetCallback(nameData);
        }
    }

    /// <summary>
    /// 氏族成员变化
    /// </summary>
    /// <param name="type">类型</param>
    /// <param name="changeMember"></param>
    public void OnClanInfoChanged(eChangeMemberInfo type, stClanMemberInfo changeMember)
    {
        UpdateClanMemberInfo(changeMember, type);
        DispatchSupportChanged();
    }

    /// <summary>
    /// 服务器下发氏族资金变更
    /// </summary>
    /// <param name="moneyNum"></param>
    public void OnClanMoneyGet(uint moneyNum)
    {
        if (null != ClanInfo)
        {
            ClanInfo.Money = moneyNum;
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANUPDATE, null);
    }

    /// <summary>
    /// 发送临时氏族改变消息
    /// </summary>
    private void DispatchSupportChanged()
    {
        if (IsJoinClan && !IsJoinFormal
           && IsClanCreatorSelf)
        {
            int supCount = 0;
            ClanDefine.LocalClanInfo clanInfo = ClanInfo;
            if (null != clanInfo)
            {
                supCount = clanInfo.MemberCount;
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTTEMPCLANSUPNUMCHANGED, supCount);
        }
    }
    /// <summary>
    /// 更新氏族信息
    /// </summary>
    /// <param name="info"></param>
    public void UpdatePlayerClanInfo(stClanInfo info)
    {
        uint tempClanId = 0;
        m_clanInfo = (null != info) ? new ClanDefine.LocalClanInfo(info) : null;
        if (null != info)
        {
            tempClanId = info.id;
        }
        if (ClanId != tempClanId)
        {
            IPlayer mainPlayer = DataManager.Instance.MainPlayer;
            if (null != mainPlayer)
            {
                int clanIdLow = (int)(tempClanId & 0x0000ffff);
                int clanIdHigh = (int)(tempClanId >> 16);
                mainPlayer.SetProp((int)Client.CreatureProp.ClanIdLow, clanIdLow);
                mainPlayer.SetProp((int)Client.CreatureProp.ClanIdHigh, clanIdHigh);

                //int kf = (int)tempClanId;
                //mainPlayer.SetProp((int)CreatureProp.ClanId, (int)tempClanId);
            }
        }
        m_Shaikh = FindClanShaikh();

        bool showPanelRed = CanLearnClanSkill();
        bool hasDuty = false;
        if (MyClanInfo != null)
        {
            hasDuty = GetLocalClanDutyDB(MyClanInfo.duty).CanAgreeApply;
        }
        bool showMainPanelRed = (m_dic_clanApplyInfos.Count > 0 && hasDuty) || showPanelRed;
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Clan,
            direction = (int)WarningDirection.Left,
            bShowRed = showMainPanelRed,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANUPDATE, showPanelRed);
    }

    /// <summary>
    /// 更新氏族成员信息
    /// </summary>
    /// <param name="memberInfo"></param>
    /// <param name="type"></param>
    public void UpdateClanMemberInfo(stClanMemberInfo memberInfo, eChangeMemberInfo type)
    {
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;

        if (null != clanInfo)
        {
            if (type == eChangeMemberInfo.CMI_join)
            {
                clanInfo.AddMember(memberInfo);
            }
            else if (type == eChangeMemberInfo.CMI_leave
                || type == eChangeMemberInfo.CMI_kick)
            {
                clanInfo.RemoveMember(memberInfo.id);
            }
            else if (type == eChangeMemberInfo.CMI_duty
                || type == eChangeMemberInfo.CMI_line
                || type == eChangeMemberInfo.CMI_Offline
                || type == eChangeMemberInfo.CMI_update)
            {
                clanInfo.UpdateMember(memberInfo);
            }
        }

        if (null != clanInfo && m_dic_clanInfos.ContainsKey(clanInfo.Id))
        {
            m_dic_clanInfos[clanInfo.Id] = clanInfo;
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANMEMBERUPDATE, memberInfo.id);
    }

    /// <summary>
    /// 请求服务端氏族信息列表
    /// </summary>
    /// <param name="type"></param>
    /// <param name="page"></param>
    public void GetClanInfoList(eGetClanType type, uint page)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.GetClanInfoList(type, page);
        }
    }

    /// <summary>
    /// 获取服务端氏族列表信息
    /// </summary>
    public void OnGetClanInfoList(eGetClanType type, List<stClanInfo> Infos)
    {
        List<stClanInfo> clanInfos = Infos;
        if (Infos.Count == 0)
        {
            TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Clan_Commond_sousuobudaofuhetiaojiandeshizu));
            return;
        }
        SortClanInfo(clanInfos);
        switch (type)
        {
            case eGetClanType.GCT_Temp:
            case eGetClanType.GCT_Formal:
            case eGetClanType.GCT_TempFormal:
                {
                    m_dic_clanInfos.Clear();
                    if (null != clanInfos)
                    {
                        foreach (stClanInfo info in clanInfos)
                        {
                            if (m_dic_clanInfos.ContainsKey(info.id))
                            {
                                continue;
                            }
                            m_dic_clanInfos.Add(info.id, new ClanDefine.LocalClanInfo(info));
                        }
                    }
                }
                break;
            case eGetClanType.GCT_Key_Temp:
            case eGetClanType.GCT_Key_Formal:
            case eGetClanType.GCT_Key_TempFormal:
                {
                    m_dic_clanFilterInfos.Clear();
                    if (null != clanInfos)
                    {
                        foreach (stClanInfo info in clanInfos)
                        {
                            if (m_dic_clanFilterInfos.ContainsKey(info.id))
                            {
                                continue;
                            }
                            m_dic_clanFilterInfos.Add(info.id, new ClanDefine.LocalClanInfo(info));
                        }
                    }
                }
                break;
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANLISTCHANGED, (int)type);
    }

    /// <summary>
    /// 将氏族信息进行排序：1.等级高的优先   2.等级相同就看这个氏族到达这个等级时的时间越近越优先（现在是用氏族创建时间暂时代替一下）
    /// </summary>
    /// <param name="infos"></param>
    void SortClanInfo(List<stClanInfo> infos)
    {
        if (infos.Count > 0)
        {
            for (int i = 0; i < infos.Count; i++)
            {
                for (int j = 0; j < infos.Count; j++)
                {
                    if (infos[i].level > infos[j].level || (infos[i].level == infos[j].level && infos[i].id < infos[j].id))
                    {
                        stClanInfo temp = infos[i];
                        infos[i] = infos[j];
                        infos[j] = temp;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取本地缓存氏族信息
    /// </summary>
    /// <param name="formal">是否为正式</param>
    /// <param name="filter">是否为过滤</param>
    /// <returns></returns>
    public List<ClanDefine.LocalClanInfo> GetCacheClanInfos(bool formal, bool filter)
    {
        List<ClanDefine.LocalClanInfo> infos = new List<ClanDefine.LocalClanInfo>();
        List<ClanDefine.LocalClanInfo> cacheInfos = new List<ClanDefine.LocalClanInfo>();
        if (filter)
        {
            cacheInfos.AddRange(m_dic_clanFilterInfos.Values);
        }
        else
        {
            cacheInfos.AddRange(m_dic_clanInfos.Values);
        }

        if (null != cacheInfos && cacheInfos.Count != 0)
        {
            foreach (ClanDefine.LocalClanInfo info in cacheInfos)
            {
                if (null == info)
                {
                    continue;
                }
                if (info.IsFormal == formal)
                {
                    infos.Add(info);
                }
            }
        }
        return infos;
    }

    /// <summary>
    /// 根据职位获取名称
    /// </summary>
    /// <param name="duty"></param>
    /// <returns></returns>
    public string GetNameByClanDuty(GameCmd.enumClanDuty duty)
    {
        table.ClanDutyNameDataBase cdDB
            = GameTableManager.Instance.GetTableItem<table.ClanDutyNameDataBase>((uint)duty);
        return (null != cdDB) ? cdDB.name : "";
    }

    /// <summary>
    /// 根据职阶获取氏族职阶本地数据结构
    /// </summary>
    /// <param name="duty"></param>
    /// <returns></returns>
    public static ClanDefine.LocalClanDutyDB GetLocalClanDutyDB(enumClanDuty duty)
    {
        ClanDutyPermDataBase db
            = GameTableManager.Instance.GetTableItem<ClanDutyPermDataBase>((uint)duty);
        return (null != db) ? new ClanDefine.LocalClanDutyDB(db) : null;
    }

    /// <summary>
    /// 获取氏族成员表格本地数据
    /// </summary>
    /// <param name="lv">氏族等级</param>
    /// <returns></returns>
    public static ClanDefine.LocalClanMemberDB GetLocalCalnMemberDB(uint lv)
    {
        ClanDefine.LocalClanMemberDB ldb = null;
        table.ClanMemberDataBase mdb =
            GameTableManager.Instance.GetTableItem<table.ClanMemberDataBase>(lv);
        if (null != mdb)
        {
            ldb = new ClanDefine.LocalClanMemberDB(mdb);
        }
        return ldb;
    }

    /// <summary>
    /// 发送氏族公告
    /// </summary>
    /// <param name="content"></param>
    public void SendClanGG(string content)
    {
        TextManager tmgr = DataManager.Manager<TextManager>();
        if (!string.IsNullOrEmpty(content))
        {
            List<string> txts = null;
            StringBuilder builder = null;
            if (TextManager.IsMatchNGUIColor(content, out txts))
            {
                builder = new StringBuilder();
                for (int i = 0; i < txts.Count; i++)
                {
                    builder.Append(string.Format(TextManager.CONST_TAG_WORDS_FORMAT, txts[i].Substring(1, txts[i].Length - 2)));
                    if (i < txts.Count - 1)
                    {
                        builder.Append(",");
                    }
                }
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Local_TXT_Warning_FM_NGUIColor
                    , tmgr.GetLocalText(LocalTextType.Local_TXT_Announcement)
                    , builder.ToString());
                return;
            }
            uint charNum = TextManager.GetCharNumByStrInUnicode(content);
            if (charNum > TextManager.TransformWordNum2CharNum(ClanDefine.CONST_CLAN_GG_MAX_WORDS))
            {
                TipsManager.Instance.ShowTips(tmgr.GetLocalFormatText(LocalTextType.Local_TXT_ClanGGLimit));
                return;
            }
            if (!TextManager.IsLegalText(content))
            {
                TipsManager.Instance.ShowTips(tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Warning_FM_IllegalChar
                    , tmgr.GetLocalText(LocalTextType.Local_TXT_Announcement)));
                return;
            }
            if (DataManager.Manager<TextManager>().IsContainSensitiveWord(content, TextManager.MatchType.Max))
            {
                TipsManager.Instance.ShowTips(tmgr.GetLocalFormatText(LocalTextType.Local_TXT_Warning_FM_Sensitive
                    , tmgr.GetLocalText(LocalTextType.Local_TXT_Announcement)));
                return;
            }
        }
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null == clanInfo)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalText(LocalTextType.Local_TXT_Notice_ClanError));
            return;
        }
        string gg = (string.IsNullOrEmpty(clanInfo.GG) ? "" : clanInfo.GG);
        if (string.IsNullOrEmpty(content))
        {
            content = "";
        }
        if (gg.Equals(content))
        {
            Engine.Utility.Log.Info("GG is similar not send");
            return;
        }
        stClanMemberInfo me = clanInfo.GetMemberInfo(DataManager.Instance.UserId);
        if (null == me)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalText(LocalTextType.Local_TXT_Notice_ClanPlayerError));
            return;
        }
        ClanDefine.LocalClanDutyDB myDutyDB = GetLocalClanDutyDB(me.duty);
        if (null == myDutyDB)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalText(LocalTextType.Local_TXT_Notice_ClanPlayerDutyError));
            return;
        }

        if (!myDutyDB.CanAgreeApply)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalText(LocalTextType.Local_TXT_Notice_NoOpRight));
            return;
        }
        BroadCastClanNoticeReq(content, enumBroadCastType.BCT_BroadCastMsg);
    }

    /// <summary>
    /// 发送置顶公告
    /// </summary>
    /// <returns></returns>
    public void SendTopClanGG()
    {
        BroadCastClanNoticeReq(null, enumBroadCastType.BCT_Head);
    }

    /// <summary>
    /// 发送广播信息
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="type"></param>
    public void BroadCastClanNoticeReq(string msg, enumBroadCastType type)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.BroadCastClanNoticeReq(msg, type);
        }
    }

    /// <summary>
    /// 服务器下发氏族广播信息
    /// </summary>
    /// <param name="userId">发送的玩家</param>
    /// <param name="type">类型</param>
    /// <param name="msg">内容</param>
    public void OnBroadCastClanNotice(uint userId, enumBroadCastType type, string msg)
    {
        if (!IsJoinClan)
        {
            Engine.Utility.Log.Info("ClanManger OnBroadCastClanNotice failed,not exist clan!");
            return;
        }

        switch (type)
        {
            case enumBroadCastType.BCT_Head:
                //跑马灯
                break;
            case enumBroadCastType.BCT_ClanMsg:
                //氏族信息
                break;
            case enumBroadCastType.BCT_BroadCastMsg:
                //氏族公告
                if (userId == DataManager.Instance.UserId)
                {
                    TipsManager.Instance.ShowTips(
                        DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Notice_UpdateSuccess));
                }
                UpdateClanGG(msg);
                break;
            case enumBroadCastType.BCT_ClanHonor:
                //荣誉榜
                //DataManager.Manager<ClanManger>().OnAddClanHonorInfo(msg.szMessage,true);
                break;
        }
    }

    /// <summary>
    /// 更新公告
    /// </summary>
    /// <param name="content"></param>
    private void UpdateClanGG(string content)
    {
        if (!IsJoinClan)
        {
            return;
        }
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null == clanInfo)
        {
            return;
        }
        clanInfo.UpdateGG(content);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANGGUPDATE, content);
    }

    /// <summary>
    /// 发送公告
    /// </summary>
    /// <param name="content">内容</param>
    public void BroadCastGG(string content)
    {
        BroadCastClanNoticeReq(content, enumBroadCastType.BCT_BroadCastMsg);
    }

    private Dictionary<uint, ClanDefine.ClanNameData> m_dic_cacheClanName = null;
    private Dictionary<uint, List<ClanDefine.OnClanNameGet>> m_dic_getNameCallBack = null;

    /// <summary>
    /// 获取氏族名称
    /// </summary>
    /// <param name="clanid"></param>
    /// <param name="nameGetCallBack"></param>
    public void GetClanName(uint clanid, ClanDefine.OnClanNameGet nameGetCallBack)
    {
        ClanDefine.ClanNameData clanNameData = null;
        if (m_dic_cacheClanName.TryGetValue(clanid, out clanNameData))
        {
            if (null != nameGetCallBack)
            {
                nameGetCallBack.Invoke(clanNameData);
            }
            return;
        }
        CacheNameGetCallBack(clanid, nameGetCallBack);
        //不存在缓存，请求
        GetClanInfoReq(clanid, false);
    }

    /// <summary>
    /// 缓存氏族名称
    /// </summary>
    /// <param name="data"></param>
    private void CacheClanName(ClanDefine.ClanNameData data)
    {
        if (null == data)
        {
            return;
        }
        if (null == m_dic_cacheClanName)
        {
            m_dic_cacheClanName = new Dictionary<uint, ClanDefine.ClanNameData>();
        }

        if (!m_dic_cacheClanName.ContainsKey(data.ClanID))
        {
            m_dic_cacheClanName.Add(data.ClanID, data);
        }
    }

    /// <summary>
    /// 缓存获取名称回调
    /// </summary>
    /// <param name="clanId"></param>
    /// <param name="getCallback"></param>
    private void CacheNameGetCallBack(uint clanId, ClanDefine.OnClanNameGet getCallback)
    {
        if (null == m_dic_getNameCallBack)
        {
            m_dic_getNameCallBack = new Dictionary<uint, List<ClanDefine.OnClanNameGet>>();
        }
        List<ClanDefine.OnClanNameGet> callbacks = null;
        if (m_dic_getNameCallBack.TryGetValue(clanId, out callbacks))
        {
            callbacks.Add(getCallback);
        }
        else
        {
            callbacks = new List<ClanDefine.OnClanNameGet>();
            callbacks.Add(getCallback);
            m_dic_getNameCallBack.Add(clanId, callbacks);
        }
    }

    /// <summary>
    /// 氏族名称获取触发缓存回调
    /// </summary>
    /// <param name="clanId"></param>
    /// <param name="data"></param>
    private void InvokeNameGetCallback(ClanDefine.ClanNameData data)
    {
        if (null == data)
        {
            return;
        }
        List<ClanDefine.OnClanNameGet> callbacks = null;
        if (m_dic_getNameCallBack.TryGetValue(data.ClanID, out callbacks))
        {
            if (callbacks.Count > 0)
            {
                for (int i = 0; i < callbacks.Count; i++)
                {
                    callbacks[i].Invoke(data);
                }
            }
            callbacks.Clear();
        }
    }

    #endregion

    #region IManager Method
    /// <summary>
    /// 注册全局事件
    /// </summary>
    /// <param name="register"></param>
    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.PLAYER_LOGIN_SUCCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TASK_DONING, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TASK_CANSUBMIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TASK_DONE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_STATECHANEGE, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.PLAYER_LOGIN_SUCCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TASK_DONING, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TASK_CANSUBMIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TASK_DONE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_STATECHANEGE, GlobalEventHandler);
        }
    }
    public void ClearData()
    {

    }
    public void Initialize()
    {
        RegisterGlobalEvent(true);
        if (null == m_dic_clanInfos)
        {
            m_dic_clanInfos = new Dictionary<uint, ClanDefine.LocalClanInfo>();
        }
        m_dic_clanInfos.Clear();

        if (null == m_dic_clanFilterInfos)
        {
            m_dic_clanFilterInfos = new Dictionary<uint, ClanDefine.LocalClanInfo>();
        }
        m_dic_clanInfos.Clear();

        if (null == m_dic_clanTaskInfos)
        {
            m_dic_clanTaskInfos = new Dictionary<uint, ClanQuestInfo>();
        }
        m_dic_clanTaskInfos.Clear();

        if (null == m_dic_Rivalry)
        {
            m_dic_Rivalry = new Dictionary<uint, stWarClanInfo>();
        }
        else
        {
            m_dic_Rivalry.Clear();
        }
        if (null == m_dic_HistoryRivalry)
        {
            m_dic_HistoryRivalry = new Dictionary<uint, stWarClanInfo>();
        }
        else
        {
            m_dic_HistoryRivalry.Clear();
        }
        if (null == m_dic_DeclareWarSerchInfos)
        {
            m_dic_DeclareWarSerchInfos = new Dictionary<uint, stWarClanInfo>();
        }
        else
        {
            m_dic_DeclareWarSerchInfos.Clear();
        }

        if (null == m_dic_cacheClanName)
        {
            m_dic_cacheClanName = new Dictionary<uint, ClanDefine.ClanNameData>();
        }
        if (null == m_dic_getNameCallBack)
        {
            m_dic_getNameCallBack = new Dictionary<uint, List<ClanDefine.OnClanNameGet>>();
        }
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, OnPlayerLoginSuccess);

        //氏族任务数据
        InitClanTaskGlobalConfig();
    }
    //针对登陆成功时请求一下列表以便于控制显示主界面红点
    private bool m_firstLoginRequestClanList = false;
    private void OnPlayerLoginSuccess(int eventid, object cmd)
    {
        if (eventid == (int)Client.GameEventID.PLAYER_LOGIN_SUCCESS)
        {
            NetService.Instance.Send(new stRequestListClanUserCmd_C());
            m_firstLoginRequestClanList = true;
        }
    }
    public void Reset(bool depthClearData = false)
    {
        //RegisterGlobalEvent(false);

        m_clanInfo = null;
        m_Shaikh = null;
        m_bool_autoRefuseInvite = false;
        m_bool_showClanName = false;
        m_dic_clanInfos.Clear();
        m_dic_clanFilterInfos.Clear();
        m_list_userApplyClans.Clear();
        //任务
        m_bool_getClanTaskInfos = false;
        m_dic_clanTaskInfos.Clear();
        //捐献
        m_dic_donateLeftTimes.Clear();
        //荣誉
        m_list_honorinfos.Clear();
        //升级
        m_uint_clanMaxlv = 0;
        //成员
        m_bool_requestUsersApplyInfos = false;
        m_firstLoginRequestClanList = false;
        m_dic_clanApplyInfos.Clear();
        //氏族名称
        m_dic_cacheClanName.Clear();
        //氏族名称回调
        m_dic_getNameCallBack.Clear();
        ResetDeclareWar();
    }

    public void Process(float deltaTime)
    {
    }
    #endregion

    #region Donate
    /// <summary>
    /// 获取捐献列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetClanDonateList()
    {
        List<table.ClanDonateDataBase> tabList = GameTableManager.Instance.GetTableList<table.ClanDonateDataBase>();
        List<uint> donateIds = new List<uint>();
        if (null != tabList && tabList.Count != 0)
        {
            foreach (table.ClanDonateDataBase db in tabList)
            {
                if (!donateIds.Contains(db.id))
                {
                    donateIds.Add(db.id);
                }
            }
        }
        return donateIds;
    }
    //捐献次数id
    private Dictionary<uint, uint> m_dic_donateLeftTimes = new Dictionary<uint, uint>();
    private void UpdateDonateLeftTimes(uint id, uint left)
    {
        if (m_dic_donateLeftTimes.ContainsKey(id))
        {
            m_dic_donateLeftTimes[id] = left;
        }
        else
        {
            m_dic_donateLeftTimes.Add(id, left);
        }
    }
    /// <summary>
    /// 获取捐献本地数据
    /// </summary>
    /// <param name="donateId"></param>
    /// <returns></returns>
    public ClanDefine.LocalClanDonateDB GetLocalDonateDB(uint donateId)
    {
        table.ClanDonateDataBase cddb = GameTableManager.Instance.GetTableItem<table.ClanDonateDataBase>(donateId);
        if (null != cddb)
        {
            uint leftTimes = (m_dic_donateLeftTimes.ContainsKey(donateId)) ? m_dic_donateLeftTimes[donateId] : 0;
            return new ClanDefine.LocalClanDonateDB(cddb, leftTimes);
        }
        return null;
    }

    /// <summary>
    /// 捐献
    /// </summary>
    /// <param name="id"></param>
    public void ClanDonate(uint id)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.ClanDonateReq(id);
    }

    /// <summary>
    /// 捐献响应
    /// </summary>
    /// <param name="ret">状态</param>
    /// <param name="id"></param>
    /// <param name="leftTimes"></param>
    public void OnClanDonate(GameCmd.enumConRetType ret, uint id, uint leftTimes)
    {
        string tips = "";
        if (ret == enumConRetType.CRT_Success)
        {
            tips = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Clan_Donate_juanxianchenggong);
            //更新剩余次数
            UpdateDonateLeftTimes(id, leftTimes);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANDONATESUCCESS, id);
        }
        //else if (ret == enumConRetType.CRT_NoTimes)
        //{
        //    tips = "捐献失败，捐献次数不足";
        //}
        //else
        //{
        //    tips = "捐献失败，捐献不足";
        //}
        if (!string.IsNullOrEmpty(tips))
        {
            TipsManager.Instance.ShowTips(tips);
        }

    }

    /// <summary>
    /// 服务器同步剩余捐献次数
    /// </summary>
    /// <param name="donateDatas"></param>
    public void onClanDonateInfoChanged(List<PairNumber> donateDatas)
    {
        m_dic_donateLeftTimes.Clear();
        if (null != donateDatas)
        {
            for (int i = 0; i < donateDatas.Count; i++)
            {
                UpdateDonateLeftTimes(donateDatas[i].id, donateDatas[i].value);
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTREFRESHCLANDONATEDATAS);
    }
    #endregion

    #region Honor
    //氏族动态信息
    private List<GameCmd.stHonorInfo> m_list_honorinfos = new List<GameCmd.stHonorInfo>();
    //获取氏族动态
    public List<GameCmd.stHonorInfo> GetHonorInfos()
    {
        List<GameCmd.stHonorInfo> infos = new List<GameCmd.stHonorInfo>();
        if (null != m_list_honorinfos && m_list_honorinfos.Count > 0)
        {
            infos.AddRange(m_list_honorinfos);
        }
        if (infos.Count > 0)
        {
            infos.Sort((left, right) =>
                {
                    if (left.tm > right.tm)
                    {
                        return -1;
                    }
                    else if (left.tm < right.tm)
                    {
                        return 1;
                    }
                    return 0;
                });
        }
        return infos;
    }

    //从服务端请求荣誉信息
    public void GetHonorInfosReq()
    {
        if (IsJoinClan && null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.GetHonorInfoReq();
        }
    }
    /// <summary>
    /// 服务端同步荣誉信息
    /// </summary>
    /// <param name="infos"></param>
    public void OnGetHonorInfos(GameCmd.eHonorType type, List<GameCmd.stHonorInfo> infos)
    {
        if (type == eHonorType.HT_Request)
        {
            m_list_honorinfos.Clear();
        }
        if (null != infos && infos.Count > 0)
        {
            for (int i = 0; i < infos.Count; i++)
            {
                OnAddClanHonorInfo(infos[i]);
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTGETCLANHONORDATAS);
    }

    /// <summary>
    /// 服务器推送氏族添加动态
    /// </summary>
    /// <param name="info"></param>
    public void OnAddClanHonorInfo(stHonorInfo info)
    {
        if (null == info)
        {
            return;
        }
        if (m_list_honorinfos.Count > 0 && m_list_honorinfos.Count >= ClanHonorMaxKeepNum)
        {
            m_list_honorinfos.RemoveAt(0);
        }
        m_list_honorinfos.Add(info);
    }
    #endregion

    #region Upgrade
    //氏族最大等级缓存
    private uint m_uint_clanMaxlv = 0;
    /// <summary>
    /// 该等级是否为氏族最大等级
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public bool IsMaxClanLv(uint lv)
    {
        bool isMaxLv = false;
        if (m_uint_clanMaxlv == 0)
        {
            //获取最大等级
            List<table.ClanUpgradeDataBase> upgradeDbs
                = GameTableManager.Instance.GetTableList<table.ClanUpgradeDataBase>();
            if (null == upgradeDbs)
            {
                m_uint_clanMaxlv = 1;
            }
            else
            {
                foreach (table.ClanUpgradeDataBase db in upgradeDbs)
                {
                    if (db.lv > m_uint_clanMaxlv)
                    {
                        m_uint_clanMaxlv = db.lv;
                    }
                }
            }
        }
        isMaxLv = (lv >= m_uint_clanMaxlv) ? true : false;
        return isMaxLv;
    }

    /// <summary>
    /// 根据氏族等级获取氏族最大人数上线
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public uint GetClanMemberMaxLimit(uint lv)
    {
        table.ClanMemberDataBase mdb
                    = GameTableManager.Instance.GetTableItem<table.ClanMemberDataBase>(lv);
        if (null != mdb)
        {
            return mdb.memberNum;
        }
        return 0;
    }

    /// <summary>
    /// 升级请求
    /// </summary>
    public void ClanUpgrade()
    {
        if (IsJoinClan && IsJoinFormal
            && null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.ClanUpgradeReq();
        }
    }

    /// <summary>
    /// 服务响应升级
    /// </summary>
    public void OnClanUpgrade(bool success)
    {
        if (success)
        {
            string tips = "氏族升级成功";
            TipsManager.Instance.ShowTips(tips);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANUPGRADE);
        }
    }
    #endregion


    #region ClanSkill
    /// <summary>
    /// 技能改变
    /// </summary>
    /// <param name="skillIds"></param>
    public void OnPlayerSkillUpdate(List<uint> skillIds)
    {
        if (null != skillIds)
        {
            List<uint> clanSkills = GetClanSkillDatas();

            for (int i = 0; i < skillIds.Count; i++)
            {
                if (null != clanSkills && clanSkills.Contains(skillIds[i]))
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTUSERLEARNCLANSKILLCHANGED, skillIds[i]);
                    RefreshSkillRedPoint();
                }
            }
        }
    }

    /// <summary>
    /// 获取氏族技能表格id列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetClanSkillDatas()
    {
        List<uint> clanSkillIds = new List<uint>();
        List<table.ClanSkillDataBase> skillDatasDB
            = GameTableManager.Instance.GetTableList<table.ClanSkillDataBase>();
        if (null != skillDatasDB)
        {
            foreach (table.ClanSkillDataBase db in skillDatasDB)
            {
                if (clanSkillIds.Contains(db.skillID))
                {
                    continue;
                }
                clanSkillIds.Add(db.skillID);
            }
        }
        return clanSkillIds;
    }

    /// <summary>
    /// 研发技能
    /// </summary>
    /// <param name="skillId"></param>
    public void DevClanSkill(uint skillId)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.DevClanSkillReq(skillId);
        }
    }

    /// <summary>
    /// 服务器下发技能研发结果
    /// </summary>
    /// <param name="skillId"></param>
    public void OnDevClanSkill(uint skillId)
    {
        table.SkillDatabase skill
            = GameTableManager.Instance.GetTableItem<table.SkillDatabase>(skillId, 1);
        TipsManager.Instance.ShowTips("成功研发技能" + ((null != skill) ? skill.strName : ""));
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANDEVSKILLCHANGED, skillId);
    }

    /// <summary>
    /// 学习氏族技能
    /// </summary>
    /// <param name="skillId">技能id</param>
    public void LearnClanSkill(uint skillId)
    {
        //下一级id
        uint nextLv = GetClanSkillLearnLv(skillId) + 1;
        DataManager.Manager<LearnSkillDataManager>().LearnSkill(skillId, nextLv);
        uint lv = GetClanSkillLearnLv(skillId);
        uint devLv = GetClanSkillDevLv(skillId);
        if (lv >= devLv)
        {
            //研发等级不够  重新请求氏族信息刷新技能红点        
            GetClanInfoReq(ClanId, false);
        }
    }

    Dictionary<uint, SkillInfo> userSkilldic = new Dictionary<uint, SkillInfo>();
    /// <summary>
    /// 根据服务器消息初始化用户技能信息
    /// </summary>
    /// <param name="skillID">技能baseid</param>
    /// <param name="level">技能等级</param>
    /// <param name="coldTime">冷却时间</param>
    public void InitUserSkill(uint skillID, uint level, uint coldTime)
    {
        SkillInfo info = new SkillInfo(skillID, level, coldTime);
        if (!userSkilldic.ContainsKey(skillID))
        {
            userSkilldic.Add(skillID, info);
        }
        else
        {
            SkillInfo oldInfo = userSkilldic[skillID];
            uint oldLevel = oldInfo.level;
            userSkilldic[skillID] = info;
        }
    }
    /// <summary>
    /// 获取氏族技能学习等级
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public uint GetClanSkillLearnLv(uint skillId)
    {
        SkillInfo skill = GetOwnSkillInfoById(skillId);
        if (null != skill)
        {
            return skill.level;
        }
        return 0;
    }
    public SkillInfo GetOwnSkillInfoById(uint skillId)
    {
        SkillInfo skill = null;
        if (null != userSkilldic
                && userSkilldic.TryGetValue(skillId, out skill))
        {
            return skill;
        }
        return null;
    }

    /// <summary>
    /// 是否玩家学习过技能
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public bool IsLearnSkill(uint skillId)
    {
        return DataManager.Manager<LearnSkillDataManager>().IsLearnSkill(skillId);
    }

    /// <summary>
    /// 获取氏族技能研发等级
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public uint GetClanSkillDevLv(uint skillId)
    {
        if (IsJoinClan)
        {
            ClanDefine.LocalClanInfo clanInfo = ClanInfo;
            if (null == clanInfo)
            {
                TipsManager.Instance.ShowTips("氏族数据错误");
                return 0;
            }
            uint lv = 0;
            if (clanInfo.TryGetSkillDevLv(skillId, out lv))
            {
                return lv;
            }
        }
        return 0;
    }

    /// <summary>
    /// 是否氏族技能解锁
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    public bool IsClanSkillUnlock(uint skillId)
    {
        uint skillLv = GetClanSkillDevLv(skillId);

        return (skillLv != 0);
    }
    #endregion

    #region ClanTask
    //是否向服务器请求氏族任务
    private bool m_bool_getClanTaskInfos = false;

    /// <summary>
    /// 今天完成任务次数
    /// </summary>
    uint m_todayFinishTimes = 0;
    public uint TodayFinishTimes
    {
        get
        {
            return m_clanTaskMaxTimes - m_todayFinishTimes;
        }
    }


    /// <summary>
    /// 氏族本周处在第几阶段目标
    /// </summary>
    uint m_clanStep = 0;
    public uint ClanStep
    {
        get
        {
            return m_clanStep;
        }
    }

    //氏族任务id  index
    Dictionary<uint, uint> m_dicClanTaskIndex = new Dictionary<uint, uint>();

    public List<uint> m_lstClanTaskIndex = new List<uint>();

    //任务难度（星级）
    Dictionary<uint, uint> m_dicClanTaskStar = new Dictionary<uint, uint>(); //key:taskId,  value : star

    public Dictionary<uint, uint> ClanTaskStarDic
    {
        get
        {
            return m_dicClanTaskStar;
        }
    }


    Dictionary<uint, uint> m_dicClanTaskFinCount = new Dictionary<uint, uint>(); //key: taskId , value: finishCount

    public Dictionary<uint, uint> ClanTaskFinCountDic
    {
        get
        {
            return m_dicClanTaskFinCount;
        }
    }

    //每一个阶段 要完成的总任务数
    List<uint> m_lstClanTaskStepNum = new List<uint>();
    public List<uint> ClanTaskStepNumLst
    {
        get
        {
            return m_lstClanTaskStepNum;
        }
    }

    //每日完成任务最大数
    uint m_clanTaskMaxTimes;

    public uint ClanTaskMaxTimes
    {
        get
        {
            return m_clanTaskMaxTimes;
        }
    }

    List<uint> m_lstClanTaskRewardItemId = new List<uint>();

    public List<uint> ClanTaskRewardItemIdList
    {
        get
        {
            return m_lstClanTaskRewardItemId;
        }
    }

    /// <summary>
    /// 初始化氏族任务全局配置
    /// </summary>
    void InitClanTaskGlobalConfig()
    {
        //taskIndex
        m_lstClanTaskIndex = GameTableManager.Instance.GetGlobalConfigList<uint>("ClanTaskIdList");

        //taskStar(难度)
        List<string> taskIdStarKeys = GameTableManager.Instance.GetGlobalConfigKeyList("ClanTaskStar");
        for (int i = 0; i < taskIdStarKeys.Count; i++)
        {
            uint taskStar = GameTableManager.Instance.GetGlobalConfig<uint>("ClanTaskStar", taskIdStarKeys[i]);

            uint taskId;
            if (uint.TryParse(taskIdStarKeys[i], out taskId))
            {
                uint tempStar;
                if (false == m_dicClanTaskStar.TryGetValue(taskId, out tempStar))
                {
                    m_dicClanTaskStar.Add(taskId, taskStar);
                }
            }
        }

        //每一个阶段 要完成的总任务数
        m_lstClanTaskStepNum = GameTableManager.Instance.GetGlobalConfigList<uint>("ClanTaskStepNum");

        m_clanTaskMaxTimes = GameTableManager.Instance.GetGlobalConfig<uint>("ClanTaskTimes");

        //奖励itemList
        for (int i = 1; i <= 5; i++)
        {
            string str = string.Format("ClanTaskStepReward" + i);
            uint rewardItemId = GameTableManager.Instance.GetGlobalConfig<uint>(str);
            if (false == m_lstClanTaskRewardItemId.Contains(rewardItemId))
            {
                m_lstClanTaskRewardItemId.Add(rewardItemId);
            }
        }
    }

    public class ClanQuestInfo
    {
        #region property

        private GameCmd.clanTaskInfo m_serverInfo = null;
        public GameCmd.clanTaskInfo ServerInfo
        {
            get
            {
                return m_serverInfo;
            }
        }
        //任务id
        public uint ID
        {
            get
            {
                return (null != ServerInfo) ? ServerInfo.task_id : 0;
            }
        }

        //任务追踪信息
        public QuestTraceInfo TraceInfo
        {
            get
            {
                return QuestTranceManager.Instance.GetQuestTraceInfo(ID);
            }
        }

        /// <summary>
        /// 任务名
        /// </summary>
        public string TaskName
        {
            get
            {
                QuestDataBase db = GameTableManager.Instance.GetTableItem<QuestDataBase>(ID);
                if (db != null)
                {
                    return db.strName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public GameCmd.TaskChildType taskChildType
        {
            get
            {
                return (GameCmd.TaskChildType)ServerInfo.task_child_type;
            }
        }
        //描述
        public string Des
        {
            get
            {
                if (null == ServerInfo)
                {
                    return "";
                }
                string strcolor = "red";

                string des = "";
                int numIndex = 0;
                switch (ServerInfo.task_child_type)
                {
                    case (int)GameCmd.TaskChildType.TaskChildType_KillMaster:
                        numIndex = 2;
                        break;
                    case (int)GameCmd.TaskChildType.TaskChildType_Collection:
                        numIndex = 3;
                        break;
                    case (int)GameCmd.TaskChildType.TaskChildType_SubmitItem:
                        numIndex = 1;
                        break;
                }
                uint state = 0;
                uint operate = 0;
                table.QuestDataBase qdb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(ID);
                if (null != qdb)
                {
                    des = LangTalkData.GetTextById(qdb.dwTranceId);
                    des = des.Replace("\n", "");
                    TaskSubType taskSub = (TaskSubType)qdb.dwSubType;
                    if (taskSub == TaskSubType.DeliverItem || taskSub == TaskSubType.UseItem)
                    {
                        if (ServerInfo.@params.Count > 0)
                        {
                            qdb.usecommitItemID = uint.Parse(ServerInfo.@params[0]);
                        }
                        if (ServerInfo.@params.Count > 1)
                        {
                            qdb.destMapID = uint.Parse(ServerInfo.@params[1]);
                        }
                    }
                    else if (taskSub == TaskSubType.KillMonster || taskSub == TaskSubType.KillMonsterCollect || taskSub == TaskSubType.CallMonster)
                    {
                        if (ServerInfo.@params.Count > 2)
                        {
                            qdb.monster_npc = uint.Parse(ServerInfo.@params[0]);
                            qdb.destMapID = uint.Parse(ServerInfo.@params[1]);
                        }
                    }
                    else if (taskSub == TaskSubType.Collection)
                    {
                        if (ServerInfo.@params.Count > 0)
                        {
                            qdb.collect_npc = uint.Parse(ServerInfo.@params[0]);
                        }
                    }
                }
                if (null != TraceInfo)
                {
                    if (TraceInfo.state <= TraceInfo.operate)
                    {
                        strcolor = "green";
                    }
                    state = TraceInfo.state;
                    operate = TraceInfo.operate;
                }
                if (qdb != null)
                {
                    if (TaskStatus != GameCmd.TaskProcess.TaskProcess_None)
                    {
                        TaskSubType taskSub = (TaskSubType)qdb.dwSubType;
                        if (taskSub == TaskSubType.DeliverItem || taskSub == TaskSubType.UseItem)
                        {
                            table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(qdb.usecommitItemID);
                            if (itemdb != null)
                            {
                                des = des.Replace("item", string.Format("<color value=\"green\">{0}</color>", itemdb.itemName));
                            }
                        }
                        else if (taskSub == TaskSubType.KillMonster || taskSub == TaskSubType.KillMonsterCollect || taskSub == TaskSubType.CallMonster)
                        {
                            des = des.Replace("npc", string.Format("<color value=\"green\">{0}</color>", GetNpcName(qdb.monster_npc)));
                        }
                        else if (taskSub == TaskSubType.Collection)
                        {
                            des = des.Replace("npc", string.Format("<color value=\"green\">{0}</color>", GetNpcName(qdb.collect_npc)));
                        }
                        des = des.Insert(des.EndsWith("</p>") ? des.Length - 4 : des.Length, string.Format("<color value=\"{2}\">({0}/{1})</color>", operate, state, strcolor));
                    }
                    else
                    {
                        TaskSubType taskSub = (TaskSubType)qdb.dwSubType;
                        if (taskSub == TaskSubType.DeliverItem || taskSub == TaskSubType.UseItem)
                        {
                            table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(qdb.usecommitItemID);
                            if (itemdb != null)
                            {
                                des = des.Replace("item", string.Format("<color value=\"green\">{0}</color>", itemdb.itemName));
                            }
                        }
                        else if (taskSub == TaskSubType.KillMonster || taskSub == TaskSubType.KillMonsterCollect || taskSub == TaskSubType.CallMonster)
                        {
                            des = des.Replace("npc", string.Format("<color value=\"green\">{0}</color>", GetNpcName(qdb.monster_npc)));
                        }
                        else if (taskSub == TaskSubType.Collection)
                        {
                            des = des.Replace("npc", string.Format("<color value=\"green\">{0}</color>", GetNpcName(qdb.collect_npc)));
                        }
                    }
                }
                return des;
            }
        }
        string GetNpcName(uint nNpcId)
        {
            table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(nNpcId);
            if (npcdb != null)
            {
                return npcdb.strName;
            }
            return "";
        }
        //是否完成
        public bool Commplete
        {
            get
            {
                return (TaskStatus == TaskProcess.TaskProcess_Done) ? true : false;
            }
        }

        //是否可以提交
        public bool CanSubmit
        {
            get
            {
                return (TaskStatus == TaskProcess.TaskProcess_CanDone) ? true : false;
            }
        }

        //任务状态
        public GameCmd.TaskProcess TaskStatus
        {
            get
            {
                return (null != TraceInfo)
                    ? TraceInfo.GetTaskProcess()
                    : TaskProcess.TaskProcess_None;
            }
        }
        //任务类型
        public GameCmd.TaskType TaskType
        {
            get
            {
                return (null != TraceInfo)
                    ? TraceInfo.taskType : GameCmd.TaskType.TaskType_None;
            }
        }
        //奖励文钱
        public uint AwardMoney
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.money : 0;
            }
        }
        //奖励金币
        public uint AwardGold
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.gold : 0;
            }
        }
        //奖励声望
        public uint AwardRep
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.clanrep : 0;
            }
        }
        //奖励族贡
        public uint AwardZG
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.clancon : 0;
            }
        }
        //奖励资金
        public uint AwardZJ
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.clanmoney : 0;
            }
        }

        //奖励物品
        public List<uint> AwardItems
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.item_base_id : null;
            }
        }

        //奖励物品数量
        public List<uint> AwardNums
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.item_num : null;
            }
        }

        //奖励神魔经验
        public uint AwardDevExp
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.devilexp : 0;
            }
        }

        //奖励经验
        public uint AwardExp
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.exp : 0;
            }
        }


        //剩余次数
        public uint LeftTimes
        {
            get
            {
                return (null != m_serverInfo) ? m_serverInfo.times : 0;
            }
        }

        /// <summary>
        /// 星级（难度）
        /// </summary>
        public uint Star
        {
            get
            {
                uint taskStar = 1;
                if (DataManager.Manager<ClanManger>().ClanTaskStarDic.TryGetValue(ID, out taskStar))
                {
                    return taskStar;
                }
                return taskStar;
            }
        }

        /// <summary>
        /// 每一个阶段 要完成的总任务数
        /// </summary>
        public uint ClanTaskStepNum
        {
            get
            {
                uint step = DataManager.Manager<ClanManger>().ClanStep;
                if (step < DataManager.Manager<ClanManger>().ClanTaskStepNumLst.Count)
                {
                    return DataManager.Manager<ClanManger>().ClanTaskStepNumLst[(int)step];
                }
                return 100;
            }
        }

        /// <summary>
        /// 已经完成任务的数量
        /// </summary>
        public uint FinishNum
        {
            get
            {
                uint num = 0;
                if (DataManager.Manager<ClanManger>().ClanTaskFinCountDic.TryGetValue(ID, out num))
                {
                    return num;
                }
                return num;
            }
        }

        /// <summary>
        /// 奖励倍数
        /// </summary>
        public uint RewardMultiple
        {
            get
            {
                if (FinishNum <= ClanTaskStepNum)
                {
                    return Star;
                }
                else
                {
                    return 1;
                }
            }
        }

        #endregion


        #region StructMethod
        public ClanQuestInfo(GameCmd.clanTaskInfo info)
        {
            m_serverInfo = info;
        }
        #endregion


    }
    //氏族任务
    private Dictionary<uint, ClanQuestInfo> m_dic_clanTaskInfos = null;
    /// <summary>
    /// 当前氏族任务列表是否存在id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsClanTaskExist(uint id)
    {
        return (null != m_dic_clanTaskInfos && m_dic_clanTaskInfos.ContainsKey(id)) ? true : false;
    }

    /// <summary>
    /// 完成氏族任务
    /// </summary>
    /// <param name="id"></param>
    public void CompleteClanTask(uint id)
    {
        RemoveClanTask(id);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, id);
    }

    /// <summary>
    /// 移除氏族任务
    /// </summary>
    /// <param name="id"></param>
    public void RemoveClanTask(uint id)
    {
        if (null != m_dic_clanTaskInfos && m_dic_clanTaskInfos.ContainsKey(id))
        {
            m_dic_clanTaskInfos.Remove(id);
        }
    }

    /// <summary>
    /// 获取氏族任务ID列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetClanTaskIds()
    {
        List<uint> clanTaskList = new List<uint>();
        //如果没有请求过就去请求
        if (!m_bool_getClanTaskInfos)
        {
            m_bool_getClanTaskInfos = true;
            RequestClanTaskInfos();

            return clanTaskList;
        }

        if (null != m_dic_clanTaskInfos)
        {
            clanTaskList.AddRange(m_dic_clanTaskInfos.Keys);
            clanTaskList.Sort();
        }
        return clanTaskList;
    }

    /// <summary>
    /// 获取氏族任务信息
    /// </summary>
    /// <param name="taskId"></param>
    /// <returns></returns>
    public ClanQuestInfo GetClanQuestInfo(uint taskId)
    {
        ClanQuestInfo info = null;
        if (null != m_dic_clanTaskInfos && m_dic_clanTaskInfos.TryGetValue(taskId, out info))
        {
            return info;
        }
        return null;
    }

    /// <summary>
    /// 请求氏族任务列表
    /// </summary>
    public void RequestClanTaskInfos()
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.RequestClanTaskInfosReq();
        }
    }

    /// <summary>
    /// 请求氏族任务阶段
    /// </summary>
    public void ReqClanTaskStep()
    {
        DataManager.Instance.Sender.ReqClanTaskStep();
    }

    /// <summary>
    /// 服务器洗发氏族任务
    /// </summary>
    /// <param name="clanTaskInfos"></param>
    public void OnRequestClanTaskInfos(List<clanTaskInfo> clanTaskInfos)
    {
        m_dic_clanTaskInfos.Clear();
        m_bool_getClanTaskInfos = true;
        if (null != clanTaskInfos)
        {
            clanTaskInfos.Sort((left, right) =>
                {
                    return (int)left.task_id - (int)right.task_id;
                });
            ClanQuestInfo clanQuestInfo = null;
            for (int i = 0; i < clanTaskInfos.Count; i++)
            {
                if (!m_dic_clanTaskInfos.ContainsKey(clanTaskInfos[i].task_id))
                {
                    clanQuestInfo = new ClanQuestInfo(clanTaskInfos[i]);
                    m_dic_clanTaskInfos.Add(clanTaskInfos[i].task_id, clanQuestInfo);
                }
                else
                {
                    Engine.Utility.Log.Error("ClanManager->OnRequestClanTaskInfos errir exist taskId={0}", clanTaskInfos[i].task_id);
                }

                //今日完成次数
                this.m_todayFinishTimes = clanTaskInfos[0].times;
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANTASKCHANGED);
    }

    /// <summary>
    /// 服务器下发刷新氏族任务
    /// </summary>
    /// <param name="refreshClanTaskInfo"></param>
    public void OnRefreshClanTaskInfo(clanTaskInfo refreshClanTaskInfo)
    {
        if (null != refreshClanTaskInfo)
        {
            if (null != m_dic_clanTaskInfos)
            {
                if (m_dic_clanTaskInfos.ContainsKey(refreshClanTaskInfo.task_id))
                {
                    m_dic_clanTaskInfos[refreshClanTaskInfo.task_id] = new ClanQuestInfo(refreshClanTaskInfo);
                }
                else
                {
                    m_dic_clanTaskInfos.Add(refreshClanTaskInfo.task_id,
                        new ClanQuestInfo(refreshClanTaskInfo));
                }
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, refreshClanTaskInfo.task_id);

                //刷新数据
                RequestClanTaskInfos();
                ReqClanTaskStep();
            }
        }
    }

    /// <summary>
    /// 接受氏族任务
    /// </summary>
    /// <param name="taskId"></param>
    public void AcceptClanTask(uint taskId)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.AcceptClanTaskReq(taskId);
        }
    }

    /// <summary>
    /// 完成氏族任务
    /// </summary>
    /// <param name="taskId"></param>
    public void FinishClanTask(uint taskId)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.FinishClanTaskReq(taskId);
        }
    }

    /// <summary>
    /// 当前阶段，以及当前阶段下已经完成的数量  
    /// </summary>
    /// <param name="cmd"></param>
    public void OnClanTaskStep(stClanTaskStepScriptUserCmd_S cmd)
    {
        this.m_clanStep = cmd.step;

        for (int i = 0; i < cmd.fin_task_step.Count; i++)
        {
            if (i < m_lstClanTaskIndex.Count)
            {
                uint finishCount;
                if (m_dicClanTaskFinCount.TryGetValue(m_lstClanTaskIndex[i], out finishCount))
                {
                    m_dicClanTaskFinCount[m_lstClanTaskIndex[i]] = cmd.fin_task_step[i];
                }
                else
                {
                    m_dicClanTaskFinCount.Add(m_lstClanTaskIndex[i], cmd.fin_task_step[i]);
                }
            }

            //Engine.Utility.Log.Error("taskId= " + m_lstClanTaskIndex[i] + " finStep=" + cmd.fin_task_step[i]);
        }


        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANTASKCHANGED);
    }

    #endregion

    #region Member
    //申请进入氏族信息
    private Dictionary<uint, stRequestListClanUserCmd_S.Data> m_dic_clanApplyInfos
        = new Dictionary<uint, stRequestListClanUserCmd_S.Data>();

    /// <summary>
    /// 获取申请玩家信息
    /// </summary>
    /// <returns></returns>
    public stRequestListClanUserCmd_S.Data GetClanApplyUserInfo(uint userId)
    {
        stRequestListClanUserCmd_S.Data userInfo = null;
        if (m_dic_clanApplyInfos.TryGetValue(userId, out userInfo))
        {
            return userInfo;
        }
        return null;
    }

    /// <summary>
    /// 获取对应职位剩余的人数
    /// </summary>
    /// <param name="duty"></param>
    /// <returns></returns>
    public int GetClanLeftMemberNum(enumClanDuty duty)
    {
        if (IsJoinClan)
        {
            ClanDefine.LocalClanInfo clanInfo = ClanInfo;
            if (null != clanInfo)
            {
                ClanDefine.LocalClanMemberDB clanMemberDB = GetLocalCalnMemberDB(clanInfo.Lv);
                if (null != clanMemberDB)
                {
                    int count = 0;
                    foreach (stClanMemberInfo info in clanInfo.GetMemberInfos())
                    {
                        if (info.duty == duty)
                        {
                            count++;
                        }
                    }
                    count = Math.Max(0, ((int)clanMemberDB.GetMemberCountOfDuty(duty) - count));
                    return count;
                }
            }

        }

        return 0;
    }
    private bool m_bool_requestUsersApplyInfos = false;
    /// <summary>
    /// 获取申请加入氏族id列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetClanApplyUserIds()
    {
        List<uint> ids = new List<uint>();
        if (!m_bool_requestUsersApplyInfos)
        {
            GetClanApplyListReq();
            return ids;
        }
        ids.AddRange(m_dic_clanApplyInfos.Keys);
        return ids;
    }
    /// <summary>
    /// 获取申请进入氏族的列表信息
    /// </summary>
    public void GetClanApplyListReq()
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.GetClanApplyListReq();
        }
    }

    /// <summary>
    /// 服务器下发申请进入氏族的列表
    /// </summary>
    /// <param name="datas"></param>
    public void OnGetClanApplyList(List<stRequestListClanUserCmd_S.Data> datas, uint op)
    {
        bool remove = false;
        if (null != datas)
        {
            switch (op)
            {
                case 1:
                    //加
                    break;
                case 2:
                    //减
                    remove = true;
                    break;
                case 3:
                    //替换

                    if (m_firstLoginRequestClanList)
                    {
                        //                         m_dic_clanApplyInfos.Clear();
                        //                         m_bool_requestUsersApplyInfos = true;
                        m_firstLoginRequestClanList = false;
                    }
                    else
                    {
                        m_dic_clanApplyInfos.Clear();
                        m_bool_requestUsersApplyInfos = true;
                    }

                    break;
            }
            foreach (stRequestListClanUserCmd_S.Data data in datas)
            {
                if (remove)
                {
                    if (m_dic_clanApplyInfos.ContainsKey(data.id))
                    {
                        m_dic_clanApplyInfos.Remove(data.id);
                    }
                }
                else
                {
                    if (m_dic_clanApplyInfos.ContainsKey(data.id))
                    {
                        m_dic_clanApplyInfos[data.id] = data;
                    }
                    else
                    {
                        m_dic_clanApplyInfos.Add(data.id, data);
                    }

                }

            }







            bool showPanelRed = m_dic_clanApplyInfos.Count > 0 && GetLocalClanDutyDB(MyClanInfo.duty).CanAgreeApply;
            bool showMainPanelRed = showPanelRed || CanLearnClanSkill();

            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.Clan,
                direction = (int)WarningDirection.Left,
                bShowRed = showMainPanelRed,
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTREFRESHCLANAPPLYINFO, showPanelRed);

        }
    }
    public void RefreshSkillRedPoint()
    {
        bool hasSkillCanLearn = CanLearnClanSkill();
        bool showMainPanelRed = (m_dic_clanApplyInfos.Count > 0 && GetLocalClanDutyDB(MyClanInfo.duty).CanAgreeApply) || hasSkillCanLearn;
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Clan,
            direction = (int)WarningDirection.Left,
            bShowRed = showMainPanelRed,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANUPDATE, hasSkillCanLearn);

    }
    public bool CanLearnClanSkill()
    {
        List<uint> skills = GetClanSkillDatas();
        uint lv = 0;
        uint devLv = 0;
        for (int i = 0; i < skills.Count; i++)
        {
            lv = GetClanSkillLearnLv(skills[i]);
            devLv = GetClanSkillDevLv(skills[i]);
            if (lv < devLv)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 清空请求加入氏族列表信息
    /// </summary>
    public void ClearClanApplyListReq()
    {
        string tips = "";
        TextManager tmgr = DataManager.Manager<TextManager>();
        if (!IsJoinClan || !IsJoinFormal)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalText(LocalTextType.Local_TXT_Notice_NotInClan));
            return;
        }
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null == clanInfo)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalText(LocalTextType.Local_TXT_Notice_ClanError));
            return;
        }
        stClanMemberInfo me = clanInfo.GetMemberInfo(DataManager.Instance.UserId);
        if (null == me)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalText(LocalTextType.Local_TXT_Notice_ClanPlayerError));
            return;
        }

        ClanDefine.LocalClanDutyDB myDutyDB = GetLocalClanDutyDB(me.duty);
        if (null == myDutyDB)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalText(LocalTextType.Local_TXT_Notice_ClanPlayerDutyError));
            return;
        }

        if (!myDutyDB.CanAgreeApply)
        {
            TipsManager.Instance.ShowTips(tmgr.GetLocalText(LocalTextType.Local_TXT_Notice_NoOpRight));
            return;
        }
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.ClearClanApplyListReq();
        }
    }

    /// <summary>
    /// 服务器响应清空请求加入氏族列表信息
    /// </summary>
    public void OnClearClanApplyList(bool success)
    {
        if (success)
        {
            m_dic_clanApplyInfos.Clear();
            bool showPanelRed = m_dic_clanApplyInfos.Count > 0 && GetLocalClanDutyDB(MyClanInfo.duty).CanAgreeApply;
            bool showMainPanelRed = showPanelRed || CanLearnClanSkill();
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.Clan,
                direction = (int)WarningDirection.Left,
                bShowRed = showMainPanelRed,
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTREFRESHCLANAPPLYINFO, showPanelRed);
        }
        //else
        //{
        //    TipsManager.Instance.ShowTips("清空玩家申请列表失败");
        //}
    }

    /// <summary>
    /// 处理申请加入氏族
    /// </summary>
    /// <param name="userId">玩家id</param>
    /// <param name="agree">是否同意加入氏族</param>
    public void DealClanApplyReq(uint userId, bool agree)
    {
        if (!IsJoinClan || !IsJoinFormal)
        {
            TipsManager.Instance.ShowTips("你还没有加入氏族");
            return;
        }
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null == clanInfo)
        {
            TipsManager.Instance.ShowTips("氏族信息错误");
            return;
        }
        stClanMemberInfo me = clanInfo.GetMemberInfo(DataManager.Instance.UserId);
        if (null == me)
        {
            TipsManager.Instance.ShowTips("玩家氏族信息错误");
            return;
        }

        ClanDefine.LocalClanDutyDB myDutyDB = GetLocalClanDutyDB(me.duty);
        if (null == myDutyDB)
        {
            TipsManager.Instance.ShowTips("玩家职位信息错误");
            return;
        }

        if (!myDutyDB.CanAgreeApply)
        {
            TipsManager.Instance.ShowTips("你没有操作权限");
            return;
        }
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.DealClanApplyReq(userId, agree);
        }
    }

    /// <summary>
    /// 是否处理成功
    /// </summary>
    /// <param name="tag"></param>
    public void OnDealClanApply(bool tag, string name)
    {
        if (tag)
        {
            string tips = string.Format("你通过了{0}加入氏族的申请", name);
            TipsManager.Instance.ShowTips(tips);
        }
    }

    /// <summary>
    /// 群发氏族消息
    /// </summary>
    /// <param name="msg"></param>
    public void MassBroadCastMsg()
    {
        if (!IsJoinClan || !IsJoinFormal)
        {
            TipsManager.Instance.ShowTips("你还没有加入氏族");
            return;
        }
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null == clanInfo)
        {
            TipsManager.Instance.ShowTips("氏族信息错误");
            return;
        }
        stClanMemberInfo me = clanInfo.GetMemberInfo(DataManager.Instance.UserId);
        if (null == me)
        {
            TipsManager.Instance.ShowTips("玩家氏族信息错误");
            return;
        }

        ClanDefine.LocalClanDutyDB myDutyDB = GetLocalClanDutyDB(me.duty);
        if (null == myDutyDB)
        {
            TipsManager.Instance.ShowTips("玩家职位信息错误");
            return;
        }

        if (!myDutyDB.CanAgreeApply)
        {
            TipsManager.Instance.ShowTips("你没有操作权限");
            return;
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonInputPanel, data: new CommonInputPanel.CommonInputPanelData()
            {
                m_str_title = "群发消息",
                m_uint_maxWordsCount = 50,
                m_str_starTxt = clanInfo.GG,
                confimAction = (input) =>
                    {
                        if (string.IsNullOrEmpty(input))
                        {
                            TipsManager.Instance.ShowTips("群发消息不能为空");
                            return;
                        }
                        DataManager.Manager<ChatDataManager>().SendText(CHATTYPE.CHAT_WORLD, input);
                        //BroadCastClanNoticeReq(input, enumBroadCastType.BCT_ClanMsg);
                        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonInputPanel);
                    },

                m_str_confirmText = "发送",
            });

    }
    /// <summary>
    /// 换氏族
    /// </summary>
    public void ChangeClan()
    {
        if (!IsJoinClan || !IsJoinFormal || null == ClanInfo)
        {
            TipsManager.Instance.ShowTips("氏族数据错误");
            return;
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanCreatePanel);
    }

    /// <summary>
    /// 逐出氏族
    /// </summary>
    /// <param name="userId"></param>
    public void ExpelFromClan(uint userId)
    {
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null == clanInfo)
        {
            TipsManager.Instance.ShowTips("氏族信息错误");
            return;
        }

        stClanMemberInfo member = clanInfo.GetMemberInfo(userId);
        stClanMemberInfo me = clanInfo.GetMemberInfo(DataManager.Instance.UserId);
        if (null == member || null == me)
        {
            TipsManager.Instance.ShowTips("氏族成员信息错误");
            return;
        }
        ClanDefine.LocalClanDutyDB duty = GetLocalClanDutyDB(me.duty);
        if (null == duty)
        {
            TipsManager.Instance.ShowTips("氏族权限信息错误");
            return;
        }
        if (!duty.CanExpel)
        {
            TipsManager.Instance.ShowTips("你没有权限");
            return;
        }
        if (duty.Duty >= member.duty)
        {
            TipsManager.Instance.ShowTips("你只能逐出比你职阶低的成员");
            return;
        }
        string tips = string.Format("把{0}逐出氏族", ColorManager.GetColorString(ColorType.Red, member.name));
        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, tips, () =>
            {
                if (null != DataManager.Instance.Sender)
                {
                    DataManager.Instance.Sender.ExpelClanMemberReq(userId);
                }
            });
    }

    /// <summary>
    /// 踢出响应
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="desUserId"></param>
    /// <param name="ret"></param>
    public void OnExpelFromClan(uint userId, uint desUserId, bool ret)
    {
        string tips = "";
        if (ret)
        {
            ClanDefine.LocalClanInfo clanInfo = ClanInfo;
            string clanName = (null != clanInfo) ? clanInfo.Name : "";
            stClanMemberInfo member = (null != clanInfo) ? ClanInfo.GetMemberInfo(desUserId) : null;
            stClanMemberInfo me = (null != clanInfo) ? ClanInfo.GetMemberInfo(userId) : null;
            if (userId == DataManager.Instance.UserId
                && null != member && null != me)
            {
                tips = string.Format("你把{0}从{1}中逐出！", member.name, clanName);
                //发送氏族广播
                DataManager.Manager<ChatDataManager>().SendText(CHATTYPE.CHAT_CLAN, string.Format("{0}把{1}从{2}中逐出！", me.name, member.name, clanName));
            }
            //变更成员
//             if (null != member)
//             {
//                 UpdateClanMemberInfo(member, eChangeMemberInfo.CMI_leave);
//             }

        }
        if (!string.IsNullOrEmpty(tips))
        {
            TipsManager.Instance.ShowTips(tips);
        }
    }

    /// <summary>
    /// 转让氏族
    /// </summary>
    /// <param name="desUserId"></param>
    public void TransferClan(uint desUserId)
    {
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (!IsJoinClan
            || null == clanInfo
            || (clanInfo.ShaikhId != DataManager.Instance.UserId))
        {
            TipsManager.Instance.ShowTipsById(405046);
            return;
        }
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.TransferClanReq(desUserId);
        }
    }

    /// <summary>
    /// 转让氏族响应
    /// </summary>
    /// <param name="success"></param>
    public void OnTransferClan(bool success)
    {
        if (success)
        {
            string tips = "成功转让氏族";
            TipsManager.Instance.ShowTips(tips);
        }

    }

    /// <summary>
    /// 调整成员职位
    /// </summary>
    /// <param name="userId"></param>
    public void ChangeDuty(uint userId, enumClanDuty duty)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.SetClanDutyReq(userId, (uint)duty);
        }
    }

    /// <summary>
    /// 服务器响应职位调整
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="duty"></param>
    public void OnChangeDuty(uint ret)
    {
        string tips = "";
        switch (ret)
        {
            case (uint)enumSetDuteRet.SDR_Success:
                //成功
                tips = "任命成功！";
                break;
        }
        if (!string.IsNullOrEmpty(tips))
        {
            TipsManager.Instance.ShowTips(tips);
        }
    }

    /// <summary>
    /// 邀请玩家加入氏族
    /// </summary>
    /// <param name="invitedUserId"></param>
    public void InviteJoinClan(uint invitedUserId)
    {
        if (!IsJoinClan)
        {
            TipsManager.Instance.ShowTips("你需先加入氏族");
            return;
        }
        ClanDefine.LocalClanInfo clanInfo = ClanInfo;
        if (null == clanInfo)
        {
            TipsManager.Instance.ShowTips("氏族信息错误");
            return;
        }
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.InviteJoinClanReq(invitedUserId);
    }

    /// <summary>
    /// 被邀请加入氏族（服务器下发）
    /// </summary>
    /// <param name="inviteName">邀请者名</param>
    /// <param name="inviteUserId">邀请者id</param>
    /// <param name="clanName">氏族名</param>
    /// <param name="clanId">氏族id</param>
    public void ServerInviteJoinClan(string inviteName, uint inviteUserId, string clanName, uint clanId)
    {
        //string content = string.Format("{0}邀请你加入氏族{1}", 
        //    ColorUtil.GetColorString(ColorType.Blue,inviteName),
        //    ColorUtil.GetColorString(ColorType.Orange, clanName));
        //TipsManager.Instance.ShowTipWindow(0, 5, TipWindowType.CancelOk, content, () =>
        //    {
        //        AnswerInvite(true, clanId);
        //    },
        //    () =>
        //    {
        //        AnswerInvite(false, clanId);
        //    }, okstr: "接受",
        //    cancleStr: "拒绝");

        DataManager.Manager<FunctionPushManager>().AddSysMsg(new PushMsg()
        {
            msgType = PushMsg.MsgType.Clan,
            senderId = inviteUserId,
            name = clanName,
            sendName = inviteName,
            groupId = clanId,
            sendTime = UnityEngine.Time.realtimeSinceStartup,
            cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("ClanMsgCD"),
        });
    }

    /// <summary>
    /// 回应邀请加入氏族
    /// </summary>
    /// <param name="agree"></param>
    /// <param name="clanId"></param>
    public void AnswerInvite(bool agree, uint senderID, uint clanId)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.AnswerInvitJoinClanReq(senderID, clanId, agree);
        }
    }
    #endregion

    #region DeclareWar
    Dictionary<uint, GameCmd.stWarClanInfo> m_dic_Rivalry = null;
    /// <summary>
    /// 是否clanid在宣战列表中
    /// </summary>
    /// <param name="clanId"></param>
    /// <returns></returns>
    public bool IsClanInDeclareWar(uint clanId)
    {
        GameCmd.stWarClanInfo info = null;
        long now = DateTimeHelper.Instance.Now;
        long leftSeconds = 0;
        if (null != m_dic_Rivalry
            && m_dic_Rivalry.TryGetValue(clanId, out info)
            && ScheduleDefine.ScheduleUnit.IsInCycleDateTme(now, info.endtime, now, out leftSeconds))
        {
            return true;
        }
        return false;
    }

    public bool IsClanInDeclareWar()
    {
        return m_dic_Rivalry.Count > 0;
    }
    /// <summary>
    /// 获取氏族敌对势力信息
    /// </summary>
    /// <param name="clanId"></param>
    /// <returns></returns>
    public GameCmd.stWarClanInfo GetClanRivalryInfo(uint clanId)
    {
        GameCmd.stWarClanInfo info = null;
        if (null != m_dic_Rivalry && m_dic_Rivalry.TryGetValue(clanId, out info))
        {
            return info;
        }
        return null;
    }

    /// <summary>
    ///重置
    /// </summary>
    public void ResetDeclareWar()
    {
        //宣战
        m_bool_requestClanRivalryInfo = false;
        m_dic_Rivalry.Clear();
        m_bool_getHistory = false;
        m_dic_HistoryRivalry.Clear();
        m_dic_DeclareWarSerchInfos.Clear();
    }

    /// <summary>
    /// 是否与实体是敌对关系
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool IsRivalryRelationShip(IEntity entity)
    {
        uint clanIdLow = (uint)entity.GetProp((int)CreatureProp.ClanIdLow);
        uint clanIdHigh = (uint)entity.GetProp((int)CreatureProp.ClanIdHigh);
        uint entityClanId = (clanIdHigh << 16) | clanIdLow;

        //uint entityClanId = (uint)entity.GetProp((int)CreatureProp.ClanId);
        List<uint> rivalryList = GetClanRivalryList();
        return rivalryList.Contains(entityClanId);
    }

    /// <summary>
    /// 获取氏族敌对势力列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetClanRivalryList()
    {
        List<uint> rivalryList = new List<uint>();
        if (!m_bool_requestClanRivalryInfo && IsJoinFormal)
        {
            SendGetClanRivalryInfos();
        }
        else if (null != m_dic_Rivalry)
        {
            rivalryList.AddRange(m_dic_Rivalry.Keys);
        }
        return rivalryList;
    }

    /// <summary>
    /// 请求氏族敌对势力
    /// </summary>
    public void SendGetClanRivalryInfos()
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.SendGetClanRivalryInfosReq();
        }
    }

    //是否请求氏族敌对势力
    private bool m_bool_requestClanRivalryInfo = false;
    /// <summary>
    /// 服务器下氏族发敌对势力
    /// </summary>
    /// <param name="infos"></param>
    public void OnGetClanRivalryInfos(List<GameCmd.stWarClanInfo> infos)
    {
        m_dic_Rivalry.Clear();
        m_bool_requestClanRivalryInfo = true;
        if (null != infos)
        {
            for (int i = 0; i < infos.Count; i++)
            {
                if (!m_dic_Rivalry.ContainsKey(infos[i].clanid))
                {
                    m_dic_Rivalry.Add(infos[i].clanid, infos[i]);
                }
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANRIVALRYREFRESH);
    }

    /// <summary>
    /// 服务器下发单个敌对势力变更
    /// </summary>
    /// <param name="type"></param>
    /// <param name="info"></param>
    public void OnClanRivalryInfoChanged(int type, GameCmd.stWarClanInfo info)
    {
        switch (type)
        {
            case (int)GameCmd.eWarClanInfoType.WCIT_end:
                if (m_dic_Rivalry.ContainsKey(info.clanid))
                {
                    m_dic_Rivalry.Remove(info.clanid);
                }
                AddClanRivalryHistory(info);
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CLANDeclareInfoRemove, info.clanid);
                break;
            case (int)GameCmd.eWarClanInfoType.WCIT_request:
            case (int)GameCmd.eWarClanInfoType.WCIT_update:
            case (int)GameCmd.eWarClanInfoType.WCIT_start_war:
            case (int)GameCmd.eWarClanInfoType.WCIT_war_ed:
                if (!m_dic_Rivalry.ContainsKey(info.clanid))
                {
                    m_dic_Rivalry.Add(info.clanid, info);
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CLANDeclareInfoAdd, info.clanid);
                }
                else
                {
                    m_dic_Rivalry[info.clanid] = info;
                }
                break;
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANRIVALRYCHANGED, info.clanid);
    }

    /// <summary>
    /// 对氏族宣战
    /// </summary>
    /// <param name="clanid"></param>
    public void StartDeclareWar(uint clanid)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.StartDeclareWarReq(clanid);
        }
    }

    /// <summary>
    /// 对氏族宣战成功
    /// </summary>
    /// <param name="clanid"></param>
    public void OnStartDeclareWar(uint clanid)
    {

    }

    private Dictionary<uint, GameCmd.stWarClanInfo> m_dic_HistoryRivalry = null;
    private bool m_bool_getHistory = false;
    /// <summary>
    /// 获取氏族宣战历史记录
    /// </summary>
    public void GetDeclareWarHistoryInfos()
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.GetDeclareWarHistoryInfosReq();
        }
    }

    /// <summary>
    /// 服务器下历史宣战信息
    /// </summary>
    /// <param name="infos"></param>
    public void OnGetDeclareWarHistoryInfos(List<GameCmd.stWarClanInfo> infos)
    {
        m_dic_HistoryRivalry.Clear();
        m_bool_getHistory = true;
        if (null != infos)
        {
            for (int i = 0; i < infos.Count; i++)
            {
                if (!m_dic_HistoryRivalry.ContainsKey(infos[i].clanid))
                {
                    m_dic_HistoryRivalry.Add(infos[i].clanid, infos[i]);
                }
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CLANDeclareInfoGet);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANRIVALRYHISTORYREFRESH);
    }

    /// <summary>
    /// 获取氏族敌对势力历史信息
    /// </summary>
    /// <param name="clanId"></param>
    /// <returns></returns>
    public GameCmd.stWarClanInfo GetClanRivalryHistoryInfo(uint clanId)
    {
        GameCmd.stWarClanInfo info = null;
        if (null != m_dic_HistoryRivalry && m_dic_HistoryRivalry.TryGetValue(clanId, out info))
        {
            return info;
        }
        return null;
    }

    /// <summary>
    /// 添加敌对历史(对历史敌对做增量)
    /// </summary>
    private void AddClanRivalryHistory(GameCmd.stWarClanInfo info)
    {
        if (m_dic_HistoryRivalry.ContainsKey(info.clanid))
        {
            return;
        }
        List<GameCmd.stWarClanInfo> infos = new List<stWarClanInfo>();
        infos.AddRange(m_dic_HistoryRivalry.Values);
        infos.Add(info);
        infos.Sort((left, right) =>
            {
                if (left.endtime > right.endtime)
                {
                    return -1;
                }
                else if (left.endtime < right.endtime)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });

        List<uint> removeIds = new List<uint>();
        if (infos.Count > ClanDeclareHistoryNum)
        {
            int removeNum = infos.Count - ClanDeclareHistoryNum;
            for (int i = 0; i < removeNum; i++)
            {
                removeIds.Add(infos[infos.Count - i - 1].clanid);
            }
        }
        if (removeIds.Count > 0)
        {
            for (int i = 0; i < removeIds.Count; i++)
            {
                m_dic_HistoryRivalry.Remove(removeIds[i]);
            }
        }
    }

    /// <summary>
    /// 获取氏族敌对势力历史列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetClanRivalryHistoryList()
    {
        List<uint> rivalryHistoryList = new List<uint>();
        if (!m_bool_getHistory)
        {
            //如果没有请求过，发送请求
            GetDeclareWarHistoryInfos();
        }
        else if (null != m_dic_HistoryRivalry)
        {
            List<GameCmd.stWarClanInfo> infos = new List<stWarClanInfo>();
            infos.AddRange(m_dic_HistoryRivalry.Values);
            infos.Sort((left, right) =>
            {
                if (left.endtime > right.endtime)
                {
                    return -1;
                }
                else if (left.endtime < right.endtime)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
            for (int i = 0; i < infos.Count; i++)
            {
                rivalryHistoryList.Add(infos[i].clanid);
            }
        }
        return rivalryHistoryList;
    }

    Dictionary<uint, GameCmd.stWarClanInfo> m_dic_DeclareWarSerchInfos = null;
    /// <summary>
    /// 查询氏族
    /// </summary>
    /// <param name="key"></param>
    /// <param name="page"></param>
    public void GetDeclareWarSearchInfos(string key, int page = 1)
    {
        if (null != DataManager.Instance.Sender)
        {
            DataManager.Instance.Sender.GetDeclareWarSerchInfosReq(key, page);
        }
    }
    /// <summary>
    /// 服务器下查询氏族信息
    /// </summary>
    /// <param name="infos"></param>
    public void OnGetDeclareWarSearchInfos(List<GameCmd.stWarClanInfo> infos, int page)
    {
        m_dic_DeclareWarSerchInfos.Clear();
        if (null != infos)
        {
            for (int i = 0; i < infos.Count; i++)
            {
                if (!m_dic_DeclareWarSerchInfos.ContainsKey(infos[i].clanid))
                {
                    m_dic_DeclareWarSerchInfos.Add(infos[i].clanid, infos[i]);
                }
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANDECLARESEARCHREFRESH);
    }

    /// <summary>
    /// 获取氏族敌对势力历史信息
    /// </summary>
    /// <param name="clanId"></param>
    /// <returns></returns>
    public GameCmd.stWarClanInfo GetClanDeclareWarSerchInfo(uint clanId)
    {
        GameCmd.stWarClanInfo info = null;
        if (null != m_dic_DeclareWarSerchInfos
            && m_dic_DeclareWarSerchInfos.TryGetValue(clanId, out info))
        {
            return info;
        }
        return null;
    }

    /// <summary>
    /// 获取氏族敌对势力历史列表
    /// </summary>
    /// <returns></returns>
    public List<uint> GetDeclareWarSerchInfoList()
    {
        List<uint> clanList = new List<uint>();
        if (null != m_dic_DeclareWarSerchInfos)
        {
            clanList.AddRange(m_dic_DeclareWarSerchInfos.Keys);
        }
        return clanList;
    }

    /// <summary>
    /// 点击宣战
    /// </summary>
    public void DoDeclareWar()
    {
        if (null != ClanInfo)
        {
            stClanMemberInfo member = ClanInfo.GetMemberInfo(DataManager.Instance.UserId);
            if (null == member)
            {
                TipsManager.Instance.ShowTips("数据错误");
                return;
            }
            if ((int)member.duty > ClanDeclareWarNeedDuty)
            {
                TipsManager.Instance.ShowTips("权限不足无法宣战");
                return;
            }

            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ClanDeclareWarPanel);
        }
    }
    #endregion


    public MemberSortType curMemberSortType { set; get; }
    bool memberReverse = false;
    public List<GameCmd.stClanMemberInfo> MemberSortByType(MemberSortType type,bool changeMember =false) 
    {
        List<GameCmd.stClanMemberInfo> m_list_memberdatas = ClanInfo.GetMemberInfos();
        if (m_list_memberdatas == null)
        {
            return null;  
        }
        memberReverse = type == curMemberSortType;
        stClanMemberInfo temp = new stClanMemberInfo();
        int length = m_list_memberdatas.Count;
        for (int i = 0; i < length; i++)
        {
            bool isNeedExchange = false;
            for (int j = i; j < length; j++)
            {
                switch (type)
                {
                    case MemberSortType.Profession:
                        isNeedExchange = m_list_memberdatas[i].job < m_list_memberdatas[j].job;
                        break;
                    case MemberSortType.Name:
                        isNeedExchange = (m_list_memberdatas[i].name).CompareTo(m_list_memberdatas[j].name) < 0;
                        break;
                    case MemberSortType.Lv:
                        isNeedExchange = m_list_memberdatas[i].level < m_list_memberdatas[j].level;
                        break;
                    case MemberSortType.Duty:
                        isNeedExchange = m_list_memberdatas[i].duty < m_list_memberdatas[j].duty;
                        break;
                    case MemberSortType.Fight:
                        isNeedExchange = m_list_memberdatas[i].fight < m_list_memberdatas[j].fight;
                        break;
                    case MemberSortType.Donate:
                        isNeedExchange = m_list_memberdatas[i].credit_total < m_list_memberdatas[j].credit_total;
                        break;
                    case MemberSortType.OnLineTime:

                        if (m_list_memberdatas[i].is_online < m_list_memberdatas[j].is_online)
                        {
                            isNeedExchange = true;
                        }
                        else if (m_list_memberdatas[i].is_online == m_list_memberdatas[j].is_online)
                        {
                            if (m_list_memberdatas[i].offline_time > m_list_memberdatas[j].offline_time)
                            {
                                isNeedExchange = false;
                            }
                            else
                            {
                                isNeedExchange = true;
                            }
                        }
                        else
                        {
                            isNeedExchange = false;
                        }
                        break;
                }
                if (isNeedExchange)
                {
                    temp = m_list_memberdatas[j];
                    m_list_memberdatas[j] = m_list_memberdatas[i];
                    m_list_memberdatas[i] = temp;
                }
            }
        }
        curMemberSortType = type;
        if (memberReverse)
        {
            m_list_memberdatas.Reverse();
            curMemberSortType = MemberSortType.None;
        }
       
        return m_list_memberdatas;
    }
    List<GameCmd.stClanMemberInfo> OnLineNoneSort(List<GameCmd.stClanMemberInfo> list)
    {
        GameCmd.stClanMemberInfo temp = null;
        int length = list.Count - 1;
        bool isNoneExchange = false;
        for (int i = 0; i < length; i++)
        {
            for (int j = length; j > i; j--)
            {
                if (list[i].duty == list[j].duty)
                {
                    if (list[i].level == list[j].level)
                    {
                        if (list[i].id < list[j].id)
                        {
                            isNoneExchange = false;
                        }
                        else
                        {
                            isNoneExchange = true;
                        }
                    }
                    else if (list[i].level < list[j].level)
                    {
                        isNoneExchange = true;
                    }
                    else
                    {
                        isNoneExchange = false;
                    }
                }
                else if (list[i].duty < list[j].duty)
                {
                    isNoneExchange = false;
                }
                else if (list[i].duty > list[j].duty)
                {
                    isNoneExchange = true;
                }
                if (isNoneExchange)
                {
                    temp = list[j];
                    list[j] = list[i];
                    list[i] = temp;
                }
            }
        }

        return list;
    }


}
public enum MemberSortType
{
    None = 0,     //默认排序
    Profession = 1,
    Name = 2,
    Lv = 3,
    Duty = 4,
    Fight = 5,
    Donate = 6,
    OnLineTime = 7,
    Max = 8,
}