using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Client;
using UnityEngine;
using table;
using UnityEngine.Profiling;

public class TeamMemberInfo
{
    public uint id;
    public string name;
    public uint lv;
    public uint job;
    public bool onLine;   //是否在线
    public uint faceId;  //用于创建角色模型，100前为男性，100后为女性
    public uint lineId;  //分线
    public List<SuitData> suitdData;  //角色外观数据


    public uint mapId;//队员当前的mapId;

    public bool isFollow;//是否在跟随

    public TeamMemberInfo(uint id, string name, uint lv, uint job, bool onLine, uint faceId, List<SuitData> suitdData)
    {
        this.id = id;
        this.name = name;
        this.lv = lv;
        this.job = job;
        this.faceId = faceId;
        this.onLine = onLine;
        this.suitdData = suitdData;
    }
}


partial class TeamDataManager : BaseModuleData, IManager
{
    #region property

    /// <summary>
    /// 队伍最多人数
    /// </summary>
    public static int TeamMemberMax = 5;

    /// <summary>
    /// 是否加入队伍
    /// </summary>
    private bool m_bIsJoinTeam = false;

    public bool IsJoinTeam
    {
        get
        {
            return m_bIsJoinTeam;
        }
    }

    /// <summary>
    /// 活动目标
    /// </summary>
    uint m_TeamActivityTargetId = 0;
    public uint TeamActivityTargetId
    {
        get
        {
            return m_TeamActivityTargetId;
        }
    }

    /// <summary>
    /// 是否有新的申请
    /// </summary>
    private bool m_haveNewApplyMember = false;
    public bool HaveNewApplyMember
    {
        set
        {
            m_haveNewApplyMember = value;
        }
        get
        {
            return m_haveNewApplyMember;
        }
    }

    /// <summary>
    /// 队伍ID
    /// </summary>
    /// 
    private uint m_teamId;
    public uint TeamId
    {
        get { return m_teamId; }
    }

    /// <summary>
    /// 队长名
    /// </summary>
    /// 
    private string m_leaderName;
    public string LeaderName
    {
        set { m_leaderName = value; }
        get { return m_leaderName; }
    }

    /// <summary>
    /// 队长ID
    /// </summary>
    /// 
    private uint m_leaderId;
    public uint LeaderId
    {
        get { return m_leaderId; }
    }

    /// <summary>
    /// 队伍的拾取道具模式
    /// </summary>
    GameCmd.TeamItemMode m_teamItemMode;
    public GameCmd.TeamItemMode TeamItemMode
    {
        get
        {
            return m_teamItemMode;
        }
    }

    /// <summary>
    /// 队伍成员list
    /// </summary>
    /// 
    private List<TeamMemberInfo> m_listTeamMember = new List<TeamMemberInfo>();//我的队伍成员
    public List<TeamMemberInfo> TeamMemberList
    {
        get
        {
            return m_listTeamMember;
        }
    }

    /// <summary>
    ///申请列表
    /// </summary>
    /// 
    private List<TeamMemberInfo> m_listApplyMember = new List<TeamMemberInfo>();//申请的成员
    public List<TeamMemberInfo> ApplyMemberList
    {
        get
        {
            return m_listApplyMember;
        }
    }

    /// <summary>
    /// 设置   自动拒绝组队邀请
    /// </summary>
    private bool m_autoRefuseTeamInvite = false;
    public bool AutoRefuseTeamInvite
    {
        get
        {
            return m_autoRefuseTeamInvite;
        }
    }

    /// <summary>
    /// 允许陌生人组队邀请  (这个UI上暂时没有   先不管  )
    /// </summary>
    private bool m_allowStrangerTeamInvite = true;

    public bool AllowStrangerTeamInvite
    {
        get
        {
            return m_allowStrangerTeamInvite;
        }
    }

    /// <summary>
    /// 自动同意入队申请 
    /// </summary>
    /// 
    private bool m_leaderAutoAgreeTeamApply = false;
    public bool LeaderAutoAgreeTeamApply
    {
        get
        {
            return m_leaderAutoAgreeTeamApply;
        }
    }

    /// <summary>
    /// 自动接收组队跟随
    /// </summary>

    private bool m_memberAutoAllowTeamFollow = false;
    public bool MemberAutoAllowTeamFollow
    {
        get
        {
            return m_memberAutoAllowTeamFollow;
        }
    }

    #endregion


    #region Interface
    public void ClearData()
    {
        ClearTeamData();
    }
    public void Initialize()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLBUTTON_CLICK, DoGameEvent); //点技能按钮
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ROBOTCOMBAT_START, DoGameEvent); //
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.JOYSTICK_PRESS, DoGameEvent);
    }

    public void Reset(bool depthClearData = false)
    {

    }

    public void Process(float deltaTime)
    {

    }
    #endregion


    #region method
    /// <summary>
    /// 添加队员
    /// </summary>
    /// <param name="memberInfo"></param>
    public void AddTeamMember(TeamMemberInfo memberInfo)
    {
        if (IsMember(memberInfo.id) == false)
        {
            if (m_listTeamMember.Count < 5)
            {
                m_listTeamMember.Add(memberInfo);
                m_bIsJoinTeam = true;
            }
            else
            {
                UnityEngine.Debug.LogError("---超出队伍成员总数");
            }
        }

        CheckAndDeleteApplyMember(memberInfo.id); //如果是申请列表中成员，删除掉
    }

    /// <summary>
    /// 删除队员
    /// </summary>
    /// <param name="id"></param>
    public void DeleteTeamMember(uint id)
    {
        for (int i = m_listTeamMember.Count - 1; i >= 0; i--)
        {
            if (m_listTeamMember[i].id == id)
            {
                m_listTeamMember.Remove(m_listTeamMember[i]);
            }
        }

        // GC 优化
        //TeamMemberInfo teamMember = teamMemberList.Find((data) => { return data.id == id; });
        //if (teamMember != null)
        //{
        //    teamMemberList.Remove(teamMember);   //删除队员
        //}

        if (m_listTeamMember.Count <= 0)
        {
            ClearTeamData();//清除队伍数据
        }

    }


    /// <summary>
    /// 解散队伍
    /// </summary>
    public void ClearTeamData()
    {
        m_listTeamMember.Clear();

        if (m_bIsFollow == true)
        {
            IPlayer player = ClientGlobal.Instance().MainPlayer;
            if (player != null)
            {
                player.SendMessage(EntityMessage.EntityCommand_StopMove);
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TEAM_LEAVE, null);

        m_teamId = 0;
        m_leaderId = 0;

        m_bIsJoinTeam = false;
        m_bIsFollow = false;
        m_bIsTeamMatch = false;

        m_haveNewApplyMember = false;

        m_TeamActivityTargetId = 0;
    }

    /// <summary>
    /// 获取队员数量
    /// </summary>
    /// <returns></returns>
    public int GetMemberCount()
    {
        return m_listTeamMember.Count;
    }

    /// <summary>
    /// 队长一人
    /// </summary>
    /// <returns></returns>
    bool LeaderOnly()
    {
        return m_listTeamMember.Count == 1 ? true : false;
    }

    /// <summary>
    /// 是否是队员
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsMember(uint id)
    {//去除谓词表达式gc  modify by dainyu
        Profiler.BeginSample("IsMember");
        //  bool isMem = teamMemberList.Exists((TeamMemberInfo data) => { return data.id == id ? true : false; });
        bool isMem = false;
        for (int i = 0; i < m_listTeamMember.Count; i++)
        {
            TeamMemberInfo data = m_listTeamMember[i];
            if (data.id == id)
            {
                isMem = true;
                break;
            }
        }
        Profiler.EndSample();
        return isMem;
    }

    /// <summary>
    /// 是否是队长
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool IsLeader(uint id)
    {
        return id == m_leaderId ? true : false;
    }

    /// <summary>
    /// 本玩家是不是队长
    /// </summary>
    /// <returns></returns>
    public bool MainPlayerIsLeader()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            uint myId = mainPlayer.GetID();
            return myId == m_leaderId ? true : false;
        }
        else
        {
            Engine.Utility.Log.Error("mainPlayer 为 null !!!");
            return false;
        }


    }

    /// <summary>
    /// 本玩家是否已经是队员
    /// </summary>
    /// <returns></returns>
    public bool MainPlayerIsMember()
    {
        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            uint myId = mainPlayer.GetID();
            return IsMember(myId);
        }
        else
        {
            Engine.Utility.Log.Error("mainPlayer 为 null !!!");
            return false;
        }


    }

    public bool IsApplyMember(uint id)
    {
        for (int i = 0; i < m_listApplyMember.Count; i++)
        {
            if (id == m_listApplyMember[i].id)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 删除申请
    /// </summary>
    /// <param name="memberInfo"></param>
    public void CheckAndDeleteApplyMember(uint id)
    {
        for (int i = m_listApplyMember.Count - 1; i >= 0; i--)
        {
            if (m_listApplyMember[i].id == id)
            {
                m_listApplyMember.Remove(m_listApplyMember[i]);
            }
        }

        // gc 优化
        //TeamMemberInfo applyMember = applyMemberList.Find((data) => { return data.id == memberInfo.id; });
        //if (applyMember != null)
        //{
        //    applyMemberList.Remove(applyMember);
        //}
    }

    /// <summary>
    /// 除我之外队员
    /// </summary>
    /// <param name="list"></param>
    public void GetMemberListWithoutMe(ref List<TeamMemberInfo> list)
    {
        list.Clear();

        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            uint myId = mainPlayer.GetID();

            for (int i = 0; i < m_listTeamMember.Count; i++)
            {
                if (m_listTeamMember[i].id != myId)
                {
                    list.Add(m_listTeamMember[i]);
                }
            }
        }
    }

    /// <summary>
    /// 获取成员信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TeamMemberInfo GetTeamMember(uint id)
    {
        for (int i = 0; i < m_listTeamMember.Count; i++)
        {
            if (m_listTeamMember[i].id == id)
            {
                return m_listTeamMember[i];
            }
        }

        return null;

        ////gc 优化
        //return teamMemberList.Find((data) => { return data.id == id; });
    }

    /// <summary>
    /// 用于设置  是否接受组队邀请
    /// </summary>
    /// <param name="allow"></param>
    public void SetAutoRefuseTeamInvite(bool autoRefuse)
    {
        m_autoRefuseTeamInvite = autoRefuse;
    }

    /// <summary>
    ///  用于设置 自动接收入队申请 
    /// </summary>
    public void SetLeaderAutoAgreeTeamApply(bool autoAgree)
    {
        m_leaderAutoAgreeTeamApply = autoAgree;
    }

    /// <summary>
    /// 用于设置 自动接收组队跟随
    /// </summary>
    public void SetMemberAutoAllowTeamFollow(bool autoAllow)
    {
        m_memberAutoAllowTeamFollow = autoAllow;
    }

    /// <summary>
    /// 允许陌生人组队邀请
    /// </summary>
    /// <param name="allow"></param>
    public void SetAllowStrangerTeamInvite(bool allow)
    {
        m_allowStrangerTeamInvite = allow;
    }

    #endregion


    #region Net

    /// <summary>
    /// 创建队伍
    /// </summary>
    public void ReqCreateTeam()
    {
        Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "创建队伍");

        GameCmd.stRequestTeamRelationUserCmd_CS cmd = new GameCmd.stRequestTeamRelationUserCmd_CS();

        IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            cmd.dwAnswerUserID = mainPlayer.GetID();
            cmd.byAnswerName = mainPlayer.GetName();
            NetService.Instance.Send(cmd);
        }
    }

    /// <summary>
    /// 解散队伍
    /// </summary>
    public void ReqDisbandteam()
    {
        Action disbandTeam = delegate
        {
            if (MainPlayerIsLeader())
            {
                GameCmd.stRemoveTeamRelationUserCmd_CS cmd = new GameCmd.stRemoveTeamRelationUserCmd_CS();
                NetService.Instance.Send(cmd);
            }
            else
            {
                TipsManager.Instance.ShowTips(LocalTextType.Team_Leader_zhiyoudongchangcainengjiesandongwu);//只有队长才能解散队伍
            }
        };

        TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, "是否解散当前队伍？", disbandTeam, null);
    }

    /// <summary>
    /// 离开队伍
    /// </summary>
    public void ReqLeaveTeam()
    {
        Action leaveTeam = delegate
        {
            GameCmd.stLeaveTeamRelationUserCmd_CS cmd = new GameCmd.stLeaveTeamRelationUserCmd_CS();
            NetService.Instance.Send(cmd);
        };

        TipsManager.Instance.ShowTipWindow(TipWindowType.YesNO, "是否离开当前队伍？", leaveTeam, null);
    }

    /// <summary>
    /// 申请入队
    /// </summary>
    /// <param name="leaderId">队长Id</param>
    /// <param name="joinId">队伍的Id</param>
    public void ReqJoinTeam(uint leaderId, uint teamId = 0)
    {
        GameCmd.stRequestJoinTeamRelationUserCmd_CS cmd = new GameCmd.stRequestJoinTeamRelationUserCmd_CS();
        cmd.user_id = leaderId;
        cmd.team_id = teamId;
        NetService.Instance.Send(cmd);
    }

    //队长同意/拒绝别人的申请
    public void ReqLeaderAnswerJoin(uint joinId, bool b)
    {
        GameCmd.stAnswerJoinTeamRelationUserCmd_CS cmd = new GameCmd.stAnswerJoinTeamRelationUserCmd_CS();
        cmd.id = joinId;
        cmd.answer = b;   //true同意  false拒绝
        NetService.Instance.Send(cmd);
    }

    //申请人被队长拒绝
    public void OnLeaderAnswerJoin(GameCmd.stAnswerJoinTeamRelationUserCmd_CS cmd)
    {
        if (cmd.answer == false)
        {
            //申请人被队长拒绝
            //TipsManager.Instance.ShowTipsById();
        }
    }

    /// <summary>
    /// 邀请组队(1、 队长组队邀请  2、队员邀请其他人入队)
    /// </summary>
    public void ReqInviteTeam(uint answerId, string answerName = null)
    {
        if (m_listTeamMember.Count < 5)
        {
            //无队伍情况创建队伍
            if (IsJoinTeam == false)
            {
                GameCmd.stRequestTeamRelationUserCmd_CS cmd = new GameCmd.stRequestTeamRelationUserCmd_CS();
                cmd.dwAnswerUserID = answerId;
                cmd.byAnswerName = answerName;
                cmd.byTeamName = "";
                NetService.Instance.Send(cmd);
            }
            //有队伍
            else if (IsJoinTeam == true)
            {
                // 1、队长组队邀请
                if (MainPlayerIsLeader())
                {
                    GameCmd.stRequestTeamRelationUserCmd_CS cmd = new GameCmd.stRequestTeamRelationUserCmd_CS();
                    cmd.dwAnswerUserID = answerId;
                    cmd.byAnswerName = answerName;
                    cmd.byTeamName = "";
                    NetService.Instance.Send(cmd);
                }
                // 2、 队员邀请其他人。先要其他人同意，再走一个申请入队流程
                else
                {
                    GameCmd.stTeamMemReqInviteRelationUserCmd_C cmd = new GameCmd.stTeamMemReqInviteRelationUserCmd_C();
                    cmd.id = answerId;
                    NetService.Instance.Send(cmd);
                }
            }
        }
        else
        {
            TipsManager.Instance.ShowTips(LocalTextType.Team_Limit_nindedongwurenshuyiman);//您的队伍人数已满
        }
    }


    /// <summary>
    /// 已经是成员的，收到新加入的人的ID
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAddTeamMember(GameCmd.stAddTeamMemberRelationUserCmd_S cmd)
    {
        TeamMemberInfo memberInfo = new TeamMemberInfo(cmd.dwUserID, cmd.userName, cmd.byLevel, cmd.byProfession, cmd.byOnline, cmd.wdFace, cmd.suit_data);

        DataManager.Manager<TeamDataManager>().AddTeamMember(memberInfo);


        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eUpdateMyTeamList, null);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateMyTeamList, null);
        }

        if (ClientGlobal.Instance().IsMainPlayer(cmd.dwUserID))
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Team_Member_Xjiaruliaodongwu, "你");//｛0｝加入了队伍
        }
        else
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Team_Member_Xjiaruliaodongwu, cmd.userName);//｛0｝加入了队伍
        }
    }


    /// <summary>
    ///  新加入的人收到所有人的list
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAddTeamMemberList(GameCmd.stAddListTeamMemberRelationUserCmd_S cmd)
    {
        CleanConvenientTeamData();//清便捷组队数据
        if (m_bIsJoinTeam)
        {
            ClearTeamData();  //清之前的队伍数据
        }

        m_bIsJoinTeam = true;
        m_teamId = cmd.dwTeamID;
        m_leaderName = cmd.byTeamName;
        m_leaderId = cmd.dwLeaderID;
        m_teamItemMode = (GameCmd.TeamItemMode)cmd.byItemMode;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TEAM_JOIN, new Client.stTeamJoin() { teamId = cmd.dwTeamID, teamName = cmd.byTeamName });

        for (int i = 0; i < cmd.data.Count; i++)
        {
            TeamMemberInfo memberInfo = new TeamMemberInfo(cmd.data[i].dwUserID, cmd.data[i].byUserName, cmd.data[i].byLevel, cmd.data[i].byProfession, cmd.data[i].byOnline, cmd.data[i].wdFace, cmd.data[i].suit_data);
            DataManager.Manager<TeamDataManager>().AddTeamMember(memberInfo);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateMyTeamList, null);
        }

        if (!MainPlayerIsLeader())
        {
            TipsManager.Instance.ShowTips(LocalTextType.Team_Join_chenggongjiarudongwu);//成功加入了队伍

            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ConvenientTeamPanel))
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ConvenientTeamPanel);//如果打开了便捷组队，现在关闭
            }
        }

        if (MainPlayerIsLeader())
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.TeamPanel);
        }
    }


    /// <summary>
    /// 解散队伍消息
    /// </summary>
    public void OnRemoveTeam(GameCmd.stRemoveTeamRelationUserCmd_CS cmd)
    {
        Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "---6 >>>收到解散队伍消息");
        ClearTeamData();

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eDisbandTeam, null);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eDisbandTeam, null);
        }
        TipsManager.Instance.ShowTips(LocalTextType.Team_Leader_dongchangjiesanliaodongwu);//队长解散了队伍
    }

    /// <summary>
    /// 离开队伍消息
    /// </summary>
    public void OnLeaveTeam(GameCmd.stLeaveTeamRelationUserCmd_CS cmd)
    {
        ClearTeamData();

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eDisbandTeam, null);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eDisbandTeam, null);
        }
    }

    /// <summary>
    /// 踢出队员 通知客户端删除队员（所有客户端都要接收）
    /// </summary>
    /// <param name="cmd"></param>
    public void OnRemoveTeamMember(GameCmd.stRemoveTeamMemberRelationUserCmd_S cmd)
    {
        Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "---7 >>>通知客户端删除队员id = {0}", cmd.dwUserID);

        if (Client.ClientGlobal.Instance().IsMainPlayer(cmd.dwUserID)) //是玩家
        {
            if (cmd.rmType == (uint)GameCmd.TeamRemoveType.TeamRemoveType_Leave)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Team_My_likaichenggong);//离开成功
            }
            else if (cmd.rmType == (uint)GameCmd.TeamRemoveType.TeamRemoveType_Kick)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Team_My_nibeitichuliaodongwu);//你被踢出了队伍
            }

            ClearTeamData();//清除队伍数据
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.TeamPanel);
            }
        }
        else
        {
            TeamMemberInfo member = m_listTeamMember.Find((TeamMemberInfo data) => { return data.id == cmd.dwUserID; });
            if (member == null)
            {
                return;
            }

            if (cmd.rmType == (uint)GameCmd.TeamRemoveType.TeamRemoveType_Leave)
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Team_Member_Xlikailiaodongwu, member.name);//{0}离开了队伍
            }
            else if (cmd.rmType == (uint)GameCmd.TeamRemoveType.TeamRemoveType_Kick)
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Team_Member_Xbeitichuliaodongwu, member.name);//{0}被踢出了队伍
            }

            DeleteTeamMember(cmd.dwUserID);//删除队员
        }



        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eUpdateMyTeamList, null);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateMyTeamList, null);
        }
    }

    /// <summary>
    /// 申请列表
    /// </summary>
    public void ReqApplyList()
    {
        if (IsLeader(ClientGlobal.Instance().MainPlayer.GetID()))
        {
            GameCmd.stRequestTeamListRelationUserCmd_C cmd = new GameCmd.stRequestTeamListRelationUserCmd_C();
            NetService.Instance.Send(cmd);
        }
    }

    /// <summary>
    /// 收到申请列表
    /// </summary>
    public void OnApplyList(GameCmd.stRequestTeamListRelationUserCmd_S cmd)
    {
        int needCount = TeamMemberMax - m_listTeamMember.Count;  //我的队伍里面还差几个人

        this.m_listApplyMember.Clear();
        for (int i = 0; i < cmd.data.Count; i++)
        {
            TeamMemberInfo teamMemberInfo = new TeamMemberInfo(cmd.data[i].userid, cmd.data[i].username, cmd.data[i].byLevel, cmd.data[i].byProfession, cmd.data[i].byOnline, cmd.data[i].wdFace, cmd.data[i].suit_data);
            if (m_leaderAutoAgreeTeamApply && i < needCount)  //自动同意入队
            {
                ReqLeaderAnswerJoin(teamMemberInfo.id, true);
            }
            else
            {
                this.m_listApplyMember.Add(teamMemberInfo);
            }
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eUpdateApplyList, null);
        }
    }

    /// <summary>
    /// 增加申请人员
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAddApply(stAddRequestListRelationUserCmd_S cmd)
    {
        //自动同意入队
        int needCount = TeamMemberMax - m_listTeamMember.Count;  //我的队伍里面还差几个人
        if (m_leaderAutoAgreeTeamApply && needCount > 0)  //自动同意入队
        {
            ReqLeaderAnswerJoin(cmd.id, true);  //同意
            return;
        }

        //手动同意入队
        TeamMemberInfo applyMemberInfo = m_listApplyMember.Find((data) => { return data.id == cmd.id; });
        if (applyMemberInfo == null)
        {
            TeamMemberInfo newApplyMemberInfo = new TeamMemberInfo(cmd.id, cmd.username, cmd.byLevel, cmd.byProfession, cmd.byOnline, cmd.wdFace, cmd.suit_data);
            m_listApplyMember.Add(newApplyMemberInfo);

            m_haveNewApplyMember = true;
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eUpdateApplyList, null);
        }

        //红点提示
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eTeamNewApply, null);
        }
    }

    /// <summary>
    /// 删除申请人员
    /// </summary>
    /// <param name="cmd"></param>
    public void OnDeleteApply(stDelRequestListRelationUserCmd_S cmd)
    {
        TeamMemberInfo applyMemberInfo = m_listApplyMember.Find((data) => { return data.id == cmd.id; });
        if (applyMemberInfo != null)
        {
            m_listApplyMember.Remove(applyMemberInfo);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eUpdateApplyList, null);
        }
    }

    /// <summary>
    /// 清空申请列表
    /// </summary>
    /// <param name="cmd"></param>
    public void OnCleanApplyList(GameCmd.stClearTeamListRelationUserCmd_CS cmd)
    {
        m_listApplyMember.Clear();
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eUpdateApplyList, null);
        }
    }

    /// <summary>
    /// 踢人
    /// </summary>
    /// <param name="id"></param>
    public void ReqKickTeamMember(uint id)
    {
        if (MainPlayerIsLeader())
        {
            GameCmd.stKickTeamMemberRelationUserCmd_C cmd = new GameCmd.stKickTeamMemberRelationUserCmd_C();
            cmd.dwUserID = id;
            NetService.Instance.Send(cmd);
        }
        else
        {
            TipsManager.Instance.ShowTips(LocalTextType.Team_Leader_zhiyoudongchangcainengtichudongyuan);//只有队长才能踢出队员
        }
    }

    /// <summary>
    /// 队长给其他人队长位置
    /// </summary>
    /// <param name="id"></param>
    public void ReqChangeLeader(uint id)
    {
        GameCmd.stChangeLeaderTeamRelationUserCmd_CS cmd = new stChangeLeaderTeamRelationUserCmd_CS();
        cmd.dwNewLeader = id;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    ///队长给其他人队长位置，所有人都接收
    /// </summary>
    public void OnChangeLeader(GameCmd.stChangeLeaderTeamRelationUserCmd_CS cmd)
    {
        m_leaderId = cmd.dwNewLeader;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eUpdateMyTeamList, null);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateMyTeamList, null);
        }

        TeamMemberInfo leader = m_listTeamMember.Find((TeamMemberInfo data) => { return data.id == m_leaderId; });
        if (leader == null)
        {
            return;
        }

        TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Team_Leader_Xchengweiliaoxindedongchang, leader.name);//{0}成为了新的队长
    }


    /// <summary>
    /// 清空申请列表
    /// </summary>
    public void ReqCleanApplyList()
    {
        if (IsLeader(ClientGlobal.Instance().MainPlayer.GetID()))
        {
            GameCmd.stClearTeamListRelationUserCmd_CS cmd = new GameCmd.stClearTeamListRelationUserCmd_CS();
            NetService.Instance.Send(cmd);
        }
    }

    /// <summary>
    /// 设置拾取模式
    /// </summary>
    /// <param name="pickMode"></param>
    public void ReqPickMode(GameCmd.TeamItemMode pickMode)
    {
        stChangeTeamModeRelationUserCmd_CS cmd = new stChangeTeamModeRelationUserCmd_CS();
        cmd.byItemMode = (uint)pickMode;
        NetService.Instance.Send(cmd);
    }

    public void OnPickMode(stChangeTeamModeRelationUserCmd_CS cmd)
    {
        m_teamItemMode = (GameCmd.TeamItemMode)cmd.byItemMode;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eTeamItemMode, null);
        }

        if (m_teamItemMode == GameCmd.TeamItemMode.TeamItemMode_Free)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Team_Recruit_ziyoushiqu);//自由拾取模式
        }
        else if (m_teamItemMode == GameCmd.TeamItemMode.TeamItemMode_Leader)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Team_Recruit_duizhangshiqu);//队长分配模式
        }
    }


    /// <summary>
    /// 同步队员状态 
    /// </summary>
    /// <param name="cmd"></param>
    //TeamMemberStatus_Died		= 0;	// Died
    //TeamMemberStatus_Offline	= 1;	// Offline
    //TeamMemberStatus_Online		= 2;	// Online
    //TeamMemberStatus_FarAway	= 3;	// Far away (default)
    //TeamMemberStatus_Normal		= 4;	// Among 9 screen
    //TeamMemberStatus_Max		= 5;
    public void OnTeamMemberState(GameCmd.stRefreshStateTeamRelationUserCmd_S cmd)
    {
        TeamMemberInfo teamMemberInfo = m_listTeamMember.Find((data) => { return data.id == cmd.dwUserID; });
        if (teamMemberInfo != null)
        {

            switch ((TeamMemberStatus)cmd.byState)
            {
                //在线  下线
                case TeamMemberStatus.TeamMemberStatus_Online:
                    {
                        teamMemberInfo.onLine = true;
                    }
                    break;
                case TeamMemberStatus.TeamMemberStatus_Offline:
                    {
                        teamMemberInfo.onLine = false;
                    }
                    break;

                //不跟随  跟随
                case TeamMemberStatus.TeamMemberStatus_Normal:
                    {
                        teamMemberInfo.isFollow = false;
                    }
                    break;
                case TeamMemberStatus.TeamMemberStatus_Follow:
                    {
                        teamMemberInfo.isFollow = true;
                    }
                    break;
            }

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TEAM_MEMBERSTATE, teamMemberInfo);
        }
    }

    /// <summary>
    /// 同步玩家mapId  LineId(分线)
    /// </summary>
    public void OnTeamMemberMapId(GameCmd.stSendSelfMapTeamRelationUserCmd_S cmd)
    {
        TeamMemberInfo memberInfo = m_listTeamMember.Find((data) => { return data.id == cmd.user_id; });
        if (memberInfo != null)
        {
            memberInfo.mapId = cmd.map_id;
            memberInfo.lineId = cmd.line_id;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TEAM_MEMBERMAPID, memberInfo);
        }
    }

    /// <summary>
    /// 同步玩家等级
    /// </summary>
    /// <param name="cmd"></param>
    public void OnTeamMemberSynLv(stSynTeamMemLevRelationUserCmd_S cmd)
    {
        TeamMemberInfo memberInfo = m_listTeamMember.Find((data) => { return data.id == cmd.id; });
        if (memberInfo != null)
        {
            memberInfo.lv = cmd.level;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TEAM_MEMBERLV, memberInfo);
        }
    }

    /// <summary>
    /// 前往目标
    /// </summary>
    public void GoToTarget()
    {
        if (m_TeamActivityTargetId == 0)
        {
            return;
        }

        TeamActivityDatabase db = GameTableManager.Instance.GetTableItem<TeamActivityDatabase>(m_TeamActivityTargetId);
        if (db == null)
        {
            return;
        }

        ItemManager.DoJump(db.jumpId);

    }

}
    #endregion



