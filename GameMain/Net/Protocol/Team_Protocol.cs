using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Engine.Utility;
using Client;

partial class Protocol
{
    //组队


    [Execute]
    public void Execute(GameCmd.stAnswerTeamRelationUserCmd_CS cmd)
    {
        Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "---1 >>>dwRequestUserID = {0}", cmd.dwRequestUserID.ToString());
    }



    //已经是成员的，收到新加入的人的ID
    [Execute]
    public void Execute(GameCmd.stAddTeamMemberRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnAddTeamMember(cmd);
    }


    //新加入的人收到所有人的list
    [Execute]
    public void Execute(GameCmd.stAddListTeamMemberRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnAddTeamMemberList(cmd);
    }


    ////创建队伍成功
    //[Execute]
    //public void Execute(GameCmd.stRequestMessageRelationUserCmd_S cmd)
    //{
    //    Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "---4 >>>创建队伍成功 teamId = {0}", cmd.teamId);
    //    DataManager.Manager<TeamDataManager>().OnTeamId(cmd);
    //}

    //收到队长的邀请信息
    [Execute]
    public void Execute(GameCmd.stRequestTeamRelationUserCmd_CS cmd)
    {
        if (DataManager.Manager<TeamDataManager>().AutoRefuseTeamInvite == false)
        {
            Engine.Utility.Log.LogGroup(GameDefine.LogGroup.User_LCY, "---5 >>>收到:{0}组队邀请信息", cmd.byAnswerName);

            DataManager.Manager<FunctionPushManager>().AddSysMsg(new PushMsg()
            {
                msgType = PushMsg.MsgType.TeamLeaderInvite,
                senderId = cmd.dwAnswerUserID,
                name = cmd.byTeamName,
                sendName = cmd.byAnswerName,
                sendTime = UnityEngine.Time.realtimeSinceStartup,
                cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("TeamLeaderInviteMsgCD"),
            });
        }
        else  //自动拒绝组队邀请
        {
            GameCmd.stAnswerTeamRelationUserCmd_CS sendCmd = new GameCmd.stAnswerTeamRelationUserCmd_CS();
            sendCmd.dwRequestUserID = cmd.dwAnswerUserID;
            sendCmd.byAgree = 0;
            NetService.Instance.Send(sendCmd);
        }
    }

    //申请人被队长拒绝
   [Execute]
    public void Execute( stAnswerJoinTeamRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnLeaderAnswerJoin(cmd);
    }


    //收到队员的邀请组队信息
    [Execute]
    public void Execute(GameCmd.stTeamMemInviteRelationUserCmd_CS cmd)
    {
        if (DataManager.Manager<TeamDataManager>().AutoRefuseTeamInvite == false)
        {
            DataManager.Manager<FunctionPushManager>().AddSysMsg(new PushMsg()
            {
                msgType = PushMsg.MsgType.TeamMemberInvite,
                senderId = cmd.userid,
                name = cmd.teamname,
                sendName = cmd.name,
                sendTime = UnityEngine.Time.realtimeSinceStartup,
                cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("TeamMemberInviteMsgCD"),
            });
        }
        else 
        {
            GameCmd.stTeamMemInviteRelationUserCmd_CS sendCmd = new stTeamMemInviteRelationUserCmd_CS();
            sendCmd.teamid = cmd.teamid;
            sendCmd.ret = false;  //拒绝
            NetService.Instance.Send(sendCmd);
        }

    }

    //接收解散队伍消息（所有人都要接收）
    [Execute]
    public void Execute(GameCmd.stRemoveTeamRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnRemoveTeam(cmd);
    }

    //接收到离开队伍消息（只有点击离开队伍的人才会接收,被踢的人也会接收这条消息）
    [Execute]
    public void Execute(GameCmd.stLeaveTeamRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnLeaveTeam(cmd);
    }
    //踢出队员

    //通知客户端删除队员（所有客户端都要接收）
    [Execute]
    public void Execute(GameCmd.stRemoveTeamMemberRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnRemoveTeamMember(cmd);
    }

    /// <summary>
    /// 队长收到别人的申请入队（暂时没用）
    /// </summary>
    /// <param name="cmd"></param>
    //[Execute]
    //public void Execute(GameCmd.stRequestJoinTeamRelationUserCmd_CS cmd)
    //{
    //    DataManager.Manager<TeamDataManager>().OnLeaderReciveJoin(cmd);
    //}


    ////申请成功
    //[Execute]
    //public void Execute(GameCmd.stAnswerJoinTeamRequestUserCmd_CS cmd)
    //{
    //    TipsManager.Instance.ShowTipsById(615);//申请成功
    //}


    //收到申请列表
    [Execute]
    public void Execute(GameCmd.stRequestTeamListRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnApplyList(cmd);
    }

    /// <summary>
    /// 增加申请人
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stAddRequestListRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnAddApply(cmd);
    }

    /// <summary>
    /// 删除申请人
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stDelRequestListRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnDeleteApply(cmd);
    }
    

    //队长给出队长职务，所有人都接收
    [Execute]
    public void Execute(GameCmd.stChangeLeaderTeamRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnChangeLeader(cmd);
    }

    //清空申请列表
    [Execute]
    public void Execute(GameCmd.stClearTeamListRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnCleanApplyList(cmd);
    }

    //同步队员mapId
    [Execute]
    public void OnTeamMemberMapId(GameCmd.stSendSelfMapTeamRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnTeamMemberMapId(cmd);
    }

    [Execute]
    public void OnTeamMemberState(GameCmd.stRefreshStateTeamRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnTeamMemberState(cmd);
    }

    [Execute]
    public void OnTeamMemberSynLv(stSynTeamMemLevRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnTeamMemberSynLv(cmd);
    }

    //
    //---------------------------便捷组队---------------------------------//
    //

    //请求已经存在的队伍list
    [Execute]
    public void Execute(GameCmd.stGetTeamListRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnConvenientTeamList(cmd);
    }


    //-------------------------------------------跟随-----------------------------------------------------------
    /// <summary>
    /// 队长要求跟随返回
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnLeaderCallFollow(stCallFollowRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnLeaderCallFollow(cmd);
    }

    /// <summary>
    /// 队员手动跟随返回
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnManualFollow(GameCmd.stManualFollowRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnManualFollow(cmd);

    }

    /// <summary>
    /// 队员取消跟随
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnCancelFollow(GameCmd.stCancelFollowRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnCancelFollow();

    }

    /// <summary>
    /// 向队员同步队员位置
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnFollowPos(stFollowPosResultRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnFollowPos(cmd);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAutoMatch(stAutoMatchTeamRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnAutoMatch(cmd);
    }

    /// <summary>
    /// 收到活动目标
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnMatchActivity(stTeamActiveSetRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnMatchActivity(cmd);
    }

    /// <summary>
    /// 取消匹配
    /// </summary>
    [Execute]
    public void OnCancelMatch(stQuitAutoMatchRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnCancelMatch(cmd);
    }


    /// <summary>
    /// 从服务器获取人员列表，用于添加到队伍中
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnInviteTeammateRelation(stInviteTeammateRelationUserCmd_S cmd)
    {
        DataManager.Manager<TeamDataManager>().OnPeopleListByType(cmd);
    }

    /// <summary>
    /// 拾取模式
    /// </summary>
    /// <param name="cmd"></param>
     [Execute]
    public void OnPickMode(stChangeTeamModeRelationUserCmd_CS cmd)
    {
        DataManager.Manager<TeamDataManager>().OnPickMode(cmd);
    }

}

