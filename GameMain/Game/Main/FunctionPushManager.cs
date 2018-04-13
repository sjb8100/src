using System;
using System.Collections.Generic;
using Client;
using table;

public class PushMsg
{
    public enum MsgType
    {
        TeamLeaderInvite = 0,           //组队，队长邀请其他人加入队伍
        TeamMemberInvite,               //组队，队员邀请其他人加入队伍
        TeamLeaderCallFollow,           //组队跟随，队长召唤队友跟随
        Arena,                          //武斗场
        Clan,                           //氏族
        TeamTransmit,
        ClanTransmit,
        CoupleTransmit,
        CityWarClan,
        CityWarTeam,
        TokenTaskReward,                //悬赏任务完成，下发奖励推送
    }

    public MsgType msgType;
    public uint senderId;
    public uint groupId;
    public string name;
    public string sendName;
    public float sendTime;
    public float cd;
    public string profession;
    public string vector;
    public int level;
    public string map;
    public uint mapId;
    public float leftTime;
}


class FunctionPushManager : BaseModuleData, IManager
{
    //已经开放的功能列表
    private List<uint> m_lstOpenedFunc = new List<uint>();
    private List<table.Trailerdatabase> m_lstdata = null;
    private List<table.Trailerdatabase> _m_lstdata 
    {
        set 
        {
            m_lstdata = value;
        }
        get
        {
            if (m_lstdata == null)
           {
               m_lstdata = GameTableManager.Instance.GetTableList<table.Trailerdatabase>();
               m_lstdata.Sort(SortDB);
           }
            return m_lstdata;
        }
    }

    //邀请类数据容器
    Dictionary<PushMsg.MsgType, List<PushMsg>> m_dicInviteMsg = new Dictionary<PushMsg.MsgType, List<PushMsg>>();
    public Dictionary<PushMsg.MsgType, List<PushMsg>> M_dicInviteMsg
    {
        set
        {
            m_dicInviteMsg = value;
        }
        get
        {
            return m_dicInviteMsg;
        }
    }

    //传送类数据容器
    private Dictionary<PushMsg.MsgType, Dictionary<uint, PushMsg>> m_dicTransmitMsg = new Dictionary<PushMsg.MsgType, Dictionary<uint, PushMsg>>();


    public Dictionary<PushMsg.MsgType, Dictionary<uint, PushMsg>> M_dicTransmitMsg
    {
        set
        {
            m_dicTransmitMsg = value;
        }
        get
        {
            return m_dicTransmitMsg;
        }
    }
    List<PushMsg> m_lstTransmit = new List<PushMsg>();
    public List<PushMsg> M_lstTransmit
    {
        set
        {
            m_lstTransmit = value;
        }
        get
        {
            return m_lstTransmit;
        }
    }
    float m_fDisableTime = 10;

    public void Initialize()
    {
      
        m_fDisableTime = (float)GameTableManager.Instance.GetGlobalConfig<int>("Max_Reject_Time");
         dailys = GameTableManager.Instance.GetTableList<DailyDataBase>();
        RegisterEvent(true);       
    }

    void RegisterEvent(bool bReg)
    {
        if (bReg)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_GAME_READY, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SYSTEM_GAME_READY, OnEvent);
        }
    }
    void OnEvent(int eventID, object param)
    {
        switch ((Client.GameEventID)eventID)
        {
            case GameEventID.SYSTEM_GAME_READY:
                StructDailyPushMsg();
                break;
        }
    }
    int SortDB(table.Trailerdatabase a, table.Trailerdatabase b)
    {
        return (int)(a.showLevel - b.showLevel);
    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            m_dicTransmitMsg.Clear();
            m_dicInviteMsg.Clear();
            m_lstOpenedFunc.Clear();
            m_lstTransmit.Clear();
            RegisterEvent(true);
        }
        
    }
    public void ClearData()
    {
        RegisterEvent(false);
    }
    bool StillHasThisTypeLeft(List<PushMsg> list, PushMsg.MsgType type)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].msgType == type)
            {
                return true;
            }
        }
        return false;
    }
    public void Process(float deltaTime)
    {      
        if (dailyPushList.Count > 0)
        {       
            bool CanReduction = false;
            next_refresh_left_time -= deltaTime;
            if (next_refresh_left_time <= UnityEngine.Mathf.Epsilon)
            {               
                next_refresh_left_time = DAILYTIMEUPDATEGAP;
                CanReduction = true;
            }
            if (!CanReduction)
            {
                return;
            }


            bool update = false;
            for (int i = 0; i < dailyPushList.Count; i++)
            {
               
                dailyPushList[i].leftSeconds--;
                long leftSeconds = dailyPushList[i].leftSeconds;
                if (dailyPushList[i].InSchedule)
                {
                    //在日程内是还剩多久结束
                    if (leftSeconds > 0)
                    {
                        if (dailyPushList[i].curState != DailyPushMsgState.Showing)
                        {
                            dailyPushList[i].curState = DailyPushMsgState.Showing;
                            update = true;
                        }
                    }
                    else
                    {
                        if (dailyPushList[i].curState != DailyPushMsgState.Closed)
                        {
                            dailyPushList[i].curState = DailyPushMsgState.Closed;
                            update = true;
                        }
                    }
                }
                else
                {
                    //一开始不在日程内，时间推移需要重置一下进入日程后的状态
                    if (leftSeconds <= 0)
                    {
                        if (dailyPushList[i].curState == DailyPushMsgState.PreShow)
                        {

                            DailyDataBase data = GameTableManager.Instance.GetTableItem<DailyDataBase>(dailyPushList[i].dailyID);
                            if (data != null)
                            {
                                dailyPushList[i] = ConstructDailyPushData(data);
                                update = true;
                            }
                        }
                        else
                        {
                            if (dailyPushList[i].curState != DailyPushMsgState.Closed)
                            {
                                dailyPushList[i].curState = DailyPushMsgState.Closed;
                                update = true;
                            }
                        }
                    }
                    else if (leftSeconds <= gap)
                    {
                        if (dailyPushList[i].curState != DailyPushMsgState.PreShow)
                        {
                            dailyPushList[i].curState = DailyPushMsgState.PreShow;
                            update = true;
                        }
                    }
                    else
                    {
                        if (dailyPushList[i].curState != DailyPushMsgState.Closed)
                        {
                            dailyPushList[i].curState = DailyPushMsgState.Closed;
                            update = true;
                        }
                    }
                }
            }
            if (update)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHDAILYPUSHSTATUS, null);
            }
        }

    }
    public const float DAILYTIMEUPDATEGAP = 1f;
    private float next_refresh_left_time = 0;


    public bool IsPushMessageRightPos(PushMsg.MsgType type)
    {
        bool right = true;
        switch (type)
        {
            case PushMsg.MsgType.ClanTransmit:
            case PushMsg.MsgType.TeamTransmit:
            case PushMsg.MsgType.CoupleTransmit:
            case PushMsg.MsgType.CityWarClan:
            case PushMsg.MsgType.CityWarTeam:
                {
                    right = false;
                }
                break;
        }
        return right;
    }

    public void AddSysMsg(PushMsg msg)
    {
        if (IsPushMessageRightPos(msg.msgType))
        {
            if (m_dicInviteMsg.ContainsKey(msg.msgType))
            {
                for (int i = 0; i < m_dicInviteMsg[msg.msgType].Count; i++)
                {
                    if (m_dicInviteMsg[msg.msgType][i].senderId == msg.senderId)
                    {
                        m_dicInviteMsg[msg.msgType].RemoveAt(i);
                    }
                }
                m_dicInviteMsg[msg.msgType].Add(msg);
            }
            else
            {
                List<PushMsg> list = new List<PushMsg>();
                list.Add(msg);
                m_dicInviteMsg.Add(msg.msgType, list);
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHINVITEPUSHMSGSTATUS, null);
        }
        else
        {
            if (!m_dicTransmitMsg.ContainsKey(msg.msgType))
            {
                m_dicTransmitMsg.Add(msg.msgType, new Dictionary<uint, PushMsg>());
            }
            if (m_dicTransmitMsg[msg.msgType].ContainsKey(msg.senderId))
            {
                m_dicTransmitMsg[msg.msgType][msg.senderId] = msg;
                for (int i = 0; i < m_lstTransmit.Count; i++)
                {
                    if (m_lstTransmit[i].msgType == msg.msgType && m_lstTransmit[i].senderId == msg.senderId)
                    {
                        m_lstTransmit[i] = msg;
                    }
                }
            }
            else
            {
                m_dicTransmitMsg[msg.msgType].Add(msg.senderId, msg);
                m_lstTransmit.Add(msg);
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHTRANSMITPUSHMSGSTATUS, null);

        }


        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MessagePushPanel) == false)
        {
            if (!DataManager.Manager<ComBatCopyDataManager>().IsEnterCopy)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MessagePushPanel);
            }            
        }
     

    }

    public void RemoveFirstSysMsg(PushMsg msg)
    {
        if (m_dicInviteMsg.ContainsKey(msg.msgType))
        {
            if (m_dicInviteMsg[msg.msgType].Contains(msg))
            {
                m_dicInviteMsg[msg.msgType].Remove(msg);
                if (m_dicInviteMsg[msg.msgType].Count == 0)
                {
                    m_dicInviteMsg.Remove(msg.msgType);
                }
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHINVITEPUSHMSGSTATUS, null);

    }


    public void RemoveAllMsg(PushMsg.MsgType type)
    {
        if (m_dicInviteMsg.ContainsKey(type))
        {
            m_dicInviteMsg[type].Clear();
            m_dicInviteMsg.Remove(type);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHINVITEPUSHMSGSTATUS, null);

        }

    }
    //传送类消息取单个数据
    public PushMsg GetPushMsg(PushMsg.MsgType type, uint sender_id = 0)
    {
        if (IsPushMessageRightPos(type))
        {
            if (m_dicInviteMsg.ContainsKey(type))
            {
                if (m_dicInviteMsg[type].Count > 0)
                {
                    return m_dicInviteMsg[type][m_dicInviteMsg[type].Count - 1];
                }
            }
        }
        else
        {
            if (M_dicTransmitMsg.ContainsKey(type))
            {
                if (M_dicTransmitMsg[type].Count > 0)
                {
                    if (M_dicTransmitMsg[type].ContainsKey(sender_id))
                    {
                        return M_dicTransmitMsg[type][sender_id];
                    }
                }
            }
        }

        return null;
    }
    //获取邀请类的个数
    public int GetMsgNum(PushMsg.MsgType type)
    {
        if (m_dicInviteMsg.ContainsKey(type))
        {
            return m_dicInviteMsg[type].Count;
        }
        return 0;
    }

    //-----------------------------------------------------------------------------------------------
    public void AddTransmitMsg(GameCmd.stInviteGoMapRequestUserCmd_CS cmd)
    {
        PushMsg msg = new PushMsg();
        if (cmd.type == (int)GameCmd.FeiLeiType.FeiLeiType_Clan)
        {
            msg.msgType = PushMsg.MsgType.ClanTransmit;
            msg.cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("ClanTransmitMsgCD");
        }
        else if (cmd.type == (int)GameCmd.FeiLeiType.FeiLeiType_Couple)
        {
            msg.msgType = PushMsg.MsgType.CoupleTransmit;
            msg.cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("CoupleTransmitMsgCD");
        }
        else if (cmd.type == (int)GameCmd.FeiLeiType.FeiLeiType_Team)
        {
            msg.msgType = PushMsg.MsgType.TeamTransmit;
            msg.cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("TeamTransmitMsgCD");
        }
        else if (cmd.type == (int)GameCmd.FeiLeiType.CallUp_CityWarClan)
        {
            msg.msgType = PushMsg.MsgType.CityWarClan;
            msg.cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("ClanTransmitMsgCD");
        }
        else if (cmd.type == (int)GameCmd.FeiLeiType.CallUp_CityWarTeam)
        {
            msg.msgType = PushMsg.MsgType.CityWarTeam;
            msg.cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("ClanTransmitMsgCD");
        }

        msg.senderId = cmd.userid;
        msg.sendName = cmd.username;
        msg.sendTime = UnityEngine.Time.realtimeSinceStartup;
        msg.vector = cmd.pos;
        table.MapDataBase mdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(cmd.mapid);
        if (mdb != null)
        {
            msg.map = mdb.strName;
            msg.mapId = cmd.mapid;
        }
        if (DataManager.Manager<ClanManger>().ClanInfo != null)
        {
            GameCmd.stClanMemberInfo clanInfo = DataManager.Manager<ClanManger>().ClanInfo.GetMemberInfo(cmd.userid);
            if (clanInfo != null)
            {
                msg.level = (int)clanInfo.level;
                SelectRoleDataBase data = table.SelectRoleDataBase.Where((GameCmd.enumProfession)clanInfo.job, (GameCmd.enmCharSex)1);
                if (data != null)
                {
                    msg.profession = data.professionName;
                }
            }
        }
        TeamMemberInfo info = DataManager.Manager<TeamDataManager>().GetTeamMember(cmd.userid);
        if (info != null)
        {
            msg.level = (int)info.lv;
            SelectRoleDataBase data = table.SelectRoleDataBase.Where((GameCmd.enumProfession)info.job, (GameCmd.enmCharSex)1);
            if (data != null)
            {
                msg.profession = data.professionName;
            }
        }
        AddSysMsg(msg);
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MessagePushPanel) == false)
        {
            if (!DataManager.Manager<ComBatCopyDataManager>().IsEnterCopy)
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MessagePushPanel);
            }  
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHTRANSMITPUSHMSGSTATUS, null);
      
    }

    public void RemoveTransmitMsg(PushMsg msg)
    {
        if (M_dicTransmitMsg.ContainsKey(msg.msgType))
        {
            if (M_dicTransmitMsg[msg.msgType].ContainsKey(msg.senderId))
            {
                PushMsg.MsgType type = msg.msgType;
                uint senderid = msg.senderId;
                if (m_lstTransmit.Contains(msg))
                {
                    m_lstTransmit.Remove(msg);
                    M_dicTransmitMsg[msg.msgType].Remove(msg.senderId);
                }
                if (!StillHasThisTypeLeft(m_lstTransmit, msg.msgType))
                {
                    M_dicTransmitMsg[msg.msgType].Clear();
                    M_dicTransmitMsg.Remove(msg.msgType);
                }
            }

        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHTRANSMITPUSHMSGSTATUS, null);
    }





    #region  系统预告
    public List<table.Trailerdatabase> GetDataList()
    {
        return _m_lstdata;
    }

    /// <summary>
    /// 服务器已经记录的开启
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsOpened(uint id)
    {
        return m_lstOpenedFunc.Contains(id);
    }
    public bool IsAllOpened() 
    {
        return m_lstOpenedFunc.Count == _m_lstdata.Count && m_lstOpenedFunc.Count != 0;
    }
    public bool CanOpen(table.Trailerdatabase trailerdata)
    {
        int level = MainPlayerHelper.GetPlayerLevel();
        if (level < trailerdata.level)
        {
            return false;
        }
        if (trailerdata.task != 0)
        {
            return DataManager.Manager<TaskDataManager>().CheckTaskFinished(trailerdata.task);
        }
        return true;
    }

    public void InitOpenList(List<uint> lstId)
    {
        m_lstOpenedFunc.Clear();
        m_lstOpenedFunc.AddRange(lstId);
    }
    public void AddOpenSys(uint id)
    {
        m_lstOpenedFunc.Add(id);
        //刷新
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHFUNCTIONPUSHOPEN, null);
    }

    public void SendMsg()
    {
        table.Trailerdatabase trailerdata = null;
        for (int i = 0; i < m_lstdata.Count; i++)
        {
            trailerdata = m_lstdata[i];
            if (IsOpened(trailerdata.dwId))
            {
                continue;
            }

            if (CanOpen(trailerdata))
            {
                m_lstOpenedFunc.Add(trailerdata.dwId);
                NetService.Instance.Send(new GameCmd.stAddContectPushRelationUserCmd_C() { id = trailerdata.dwId });
            }
        }
    }
    #endregion 



    #region 日常推送
    private List<DailyPushData> dailyPushList = new List<DailyPushData>();
    public List<DailyPushData> DailyPushList
    {
        set
        {
            dailyPushList = value;
        }
        get
        {
            return dailyPushList;
        }

    }
    uint gap = GameTableManager.Instance.GetGlobalConfig<uint>("DailyPreShowGap");
    /// <summary>
    /// 日常活动的推送数据
    /// </summary>
    List<DailyDataBase> dailys = null;
    public void StructDailyPushMsg()
    {
        if (dailyPushList != null)
        {
            dailyPushList.Clear();
        }
        DailyPushData pushDaily = null;
        if (dailys != null)
        {
            for (int i = 0; i < dailys.Count; i++)
            {
                if (dailys[i].isOpenPush == 1 && DataManager.Manager<DailyManager>().IsNotFinished(dailys[i]))
                {
                    pushDaily = ConstructDailyPushData(dailys[i]);
                    dailyPushList.Add(pushDaily);
                }
            }
      
      }
       
        dailyPushList.Sort();
    }

    DailyPushData ConstructDailyPushData(DailyDataBase data)
    {
        long leftSeconds = 0;
        bool inSchedule = DataManager.Manager<DailyManager>().UpdateDataLeftTime(data, out leftSeconds);
        DailyPushMsgState curState = DailyPushMsgState.Closed;
        if (inSchedule)
        {
            if (leftSeconds > 0)
            {
                curState = DailyPushMsgState.Showing;
            }
            else
            {
                curState = DailyPushMsgState.Closed;
            }
        }
        else
        {
            if (leftSeconds <= 0 || leftSeconds > gap)
            {
                curState = DailyPushMsgState.Closed;
            }
            else if (leftSeconds <= gap)
            {
                curState = DailyPushMsgState.PreShow;
            }
            else
            {
                curState = DailyPushMsgState.Showing;
            }
        }

        DailyPushData pushDaily = new DailyPushData()
          {
              dailyID = data.id,
              dailyName = data.name,
              iconName = data.pushIcon,
              leftSeconds = leftSeconds,
              InSchedule = inSchedule,
              curState = curState,
          };
        return pushDaily;
    }


}
public enum DailyPushMsgState
{
    Closed = 0,
    PreShow = 1,
    Showing = 2,
}
public class DailyPushData : IComparable<DailyPushData>
{
    public DailyPushMsgState curState = DailyPushMsgState.Closed;
    public uint dailyID = 0;
    public string dailyName = "";
    public string iconName = "";
    public bool InSchedule = false;
    public long leftSeconds = 0;
    public int CompareTo(DailyPushData other)
    {
        int aleftTime = (int)leftSeconds;
        int bleftTime = (int)other.leftSeconds;
        if (aleftTime < bleftTime)
        {
            return -1;
        }
        else if (aleftTime > bleftTime)
        {
            return 1;
        }
        return dailyID.CompareTo(other.dailyID);
    }

}

    #endregion