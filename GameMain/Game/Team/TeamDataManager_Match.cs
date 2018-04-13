using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

partial class TeamDataManager
{
    //是否在组队匹配
    bool m_bIsTeamMatch = false;

    public bool IsTeamMatch
    {
        get
        {
            return m_bIsTeamMatch;
        }
    }

    /// <summary>
    /// 是否便捷组队匹配
    /// </summary>
    bool m_bIsConvenientTeamMatch = false;

    public bool IsConvenientTeamMatch
    {
        get
        {
            return m_bIsConvenientTeamMatch;
        }
    }


    /// <summary>
    /// 自动匹配
    /// </summary>
    /// <param name="mainId"></param>
    /// <param name="indexId"></param>
    public void ReqAutoMatch(uint activityId)
    {
        stAutoMatchTeamRelationUserCmd_CS cmd = new stAutoMatchTeamRelationUserCmd_CS();
        cmd.active_id = activityId;
        NetService.Instance.Send(cmd);

    }


    /// <summary>
    /// 自动匹配返回
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAutoMatch(stAutoMatchTeamRelationUserCmd_CS cmd)
    {

        if (IsJoinTeam)
        {
            m_bIsTeamMatch = true;
        }
        else   //便捷组队
        {
            m_bIsConvenientTeamMatch = true;
            m_conveientActivityTargetId = cmd.active_id;
        }

        stTeamActivityTarget target = new stTeamActivityTarget { activityTargetId = cmd.active_id };

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eTeamMatch, target);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ConvenientTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ConvenientTeamPanel, UIMsgID.eTeamMatch, target);
        }

    }


    public void ReqMatchActivity(uint activityId)
    {
        stTeamActiveSetRelationUserCmd_CS cmd = new stTeamActiveSetRelationUserCmd_CS();
        cmd.active_id = activityId;
        NetService.Instance.Send(cmd);
    }

    public void OnMatchActivity(stTeamActiveSetRelationUserCmd_CS cmd)
    {
        m_TeamActivityTargetId = cmd.active_id;

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eTeamTargetActivity, new Client.stTeamTargetActivity { teamTargetActivityId = m_TeamActivityTargetId });
        }
    }


    /// <summary>
    /// 取消匹配
    /// </summary>
    public void ReqCancelMatch()
    {
        stQuitAutoMatchRelationUserCmd_CS cmd = new stQuitAutoMatchRelationUserCmd_CS();
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 取消匹配
    /// </summary>
    public void OnCancelMatch(stQuitAutoMatchRelationUserCmd_CS cmd)
    {
        if (IsJoinTeam)
        {
            m_bIsTeamMatch = false;
        }
        else
        {
            m_bIsConvenientTeamMatch = false;
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.TeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.TeamPanel, UIMsgID.eTeamCancleMatch, null);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.ConvenientTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.ConvenientTeamPanel, UIMsgID.eTeamCancleMatch, null);
        }
    }

}
