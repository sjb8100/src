using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UITeamFollowGrid : MonoBehaviour
{
    GameObject m_goLeaderCallFollow;
    GameObject m_goLeaderCancelFollow;
    GameObject m_goLeaderLeaveTeam;
    GameObject m_goFollow;
    GameObject m_goCancelFollow;
    GameObject m_goLeaveTeam;

    void Awake()
    {
        if (m_goLeaderCallFollow != null)
        {
            return;
        }

        m_goLeaderCallFollow = this.transform.Find("btn_leaderCallFollow").gameObject;
        m_goLeaderCancelFollow = this.transform.Find("btn_leaderCancelFollow").gameObject;
        m_goLeaderLeaveTeam = this.transform.Find("btn_leaderLeaveTeam").gameObject;
        m_goFollow = this.transform.Find("btn_Follow").gameObject;
        m_goCancelFollow = this.transform.Find("btn_CancelFollow").gameObject;
        m_goLeaveTeam = this.transform.Find("btn_LeaveTeam").gameObject;

        UIEventListener.Get(m_goLeaderCallFollow).onClick = onClickLeaderCallFollow;
        UIEventListener.Get(m_goLeaderCancelFollow).onClick = onClickLeaderCancelFollow;
        UIEventListener.Get(m_goLeaderLeaveTeam).onClick = OnClickLeaveTeam;

        UIEventListener.Get(m_goFollow).onClick = onClickFollow;
        UIEventListener.Get(m_goCancelFollow).onClick = onClickCancelFollow;
        UIEventListener.Get(m_goLeaveTeam).onClick = OnClickLeaveTeam;
    }

    void OnEnable()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TEAM_MEMBERFOLLOW, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.TEAM_MEMBERCANCLEFOLLOW, OnEvent);
    }

    void OnDisable()
    {
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TEAM_MEMBERFOLLOW, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.TEAM_MEMBERCANCLEFOLLOW, OnEvent);
    }

    public void Init()
    {
        Awake();

        if (DataManager.Manager<TeamDataManager>().MainPlayerIsLeader())
        {
            //队长
            m_goLeaderCallFollow.SetActive(true);
            m_goLeaderCancelFollow.SetActive(true);
            m_goLeaderLeaveTeam.SetActive(true);

            //队员
            m_goFollow.SetActive(false);
            m_goCancelFollow.SetActive(false);
            m_goLeaveTeam.SetActive(false);
        }
        else
        {
            //队长
            m_goLeaderCallFollow.SetActive(false);
            m_goLeaderCancelFollow.SetActive(false);
            m_goLeaderLeaveTeam.SetActive(false);

            //队员
            m_goLeaveTeam.SetActive(true);
            if (DataManager.Manager<TeamDataManager>().IsFollow == true)
            {
                m_goFollow.SetActive(false);
                m_goCancelFollow.SetActive(true);
            }
            else
            {
                m_goFollow.SetActive(true);
                m_goCancelFollow.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 事件
    /// </summary>
    /// <param name="eventID"></param>
    /// <param name="param"></param>
    void OnEvent(int eventID, object param)
    {
        switch ((GameEventID)eventID)
        {
            case GameEventID.TEAM_MEMBERFOLLOW:
                {
                    if (false == DataManager.Manager<TeamDataManager>().MainPlayerIsLeader())
                    {
                        m_goFollow.SetActive(false);
                        m_goCancelFollow.SetActive(true);
                    }
                }
                break;
            case GameEventID.TEAM_MEMBERCANCLEFOLLOW:
                {
                    if (false == DataManager.Manager<TeamDataManager>().MainPlayerIsLeader())
                    {
                        m_goFollow.SetActive(true);
                        m_goCancelFollow.SetActive(false);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 队长召唤跟随
    /// </summary>
    /// <param name="o"></param>
    void onClickLeaderCallFollow(GameObject o)
    {
        DataManager.Manager<TeamDataManager>().ReqLeaderCallFollow();
    }

    /// <summary>
    /// 队长取消跟随
    /// </summary>
    /// <param name="o"></param>
    void onClickLeaderCancelFollow(GameObject o)
    {
        DataManager.Manager<TeamDataManager>().ReqCancleFollow();
    }

    /// <summary>
    /// 队员手动请求跟随
    /// </summary>
    /// <param name="o"></param>
    void onClickFollow(GameObject o)
    {
        DataManager.Manager<TeamDataManager>().ReqManualFollow();
    }

    /// <summary>
    /// 队员取消跟随
    /// </summary>
    /// <param name="o"></param>
    void onClickCancelFollow(GameObject o)
    {
        DataManager.Manager<TeamDataManager>().ReqCancleFollow();
    }

    /// <summary>
    /// 离开队伍
    /// </summary>
    /// <param name="o"></param>
    void OnClickLeaveTeam(GameObject o)
    {
        DataManager.Manager<TeamDataManager>().ReqLeaveTeam();
    }

}

