using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Controller;


partial class TeamDataManager
{
    /// <summary>
    /// 是否在跟随
    /// </summary>
    private bool m_bIsFollow = false;
    public bool IsFollow
    {
        get
        {
            return m_bIsFollow;
        }
    }


    void DoGameEvent(int eventID, object param)
    {
        //ROBOTCOMBAT_START = 8001,           // 开始挂机
        //ROBOTCOMBAT_STOP,                   // 停止挂机
        //ROBOTCOMBAT_PAUSE,                  // 暂停挂机

        if (eventID == (int)GameEventID.SKILLBUTTON_CLICK || eventID == (int)GameEventID.ROBOTCOMBAT_START || eventID == (int)GameEventID.JOYSTICK_PRESS)
        {
            TeamMemberCheckAndCancelFollow();
        }
    }

    /// <summary>
    /// 当队员操作时，取消跟随
    /// </summary>
    public void TeamMemberCheckAndCancelFollow()
    {
        if (!MainPlayerIsLeader() && m_bIsFollow == true)
        {
            ReqCancleFollow();
        }
    }

    /// <summary>
    /// 队长召唤跟随
    /// </summary>
    public void ReqLeaderCallFollow()
    {
        stCallFollowRelationUserCmd_CS cmd = new stCallFollowRelationUserCmd_CS();

        NetService.Instance.Send(cmd);

        TipsManager.Instance.ShowTips(LocalTextType.Team_Follow_duizhangzhaohuanchenggong);
    }

    /// <summary>
    /// 队长召唤跟随  只有队员接收此消息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnLeaderCallFollow(stCallFollowRelationUserCmd_CS cmd)
    {
        if (m_memberAutoAllowTeamFollow)
        {
            DataManager.Manager<TeamDataManager>().ReqManualFollow();
        }
        else
        {
            DataManager.Manager<FunctionPushManager>().AddSysMsg(new PushMsg()
            {
                msgType = PushMsg.MsgType.TeamLeaderCallFollow,
                senderId = m_leaderId,
                //senderId = cmd.dwAnswerUserID,
                //name = cmd.byTeamName,
                //sendName = cmd.byAnswerName,
                sendTime = UnityEngine.Time.realtimeSinceStartup,
                cd = (float)GameTableManager.Instance.GetGlobalConfig<int>("TeamLeaderCallFollowMsgCD"),
            });
        }

    }

    /// <summary>
    /// 队长或队员取消跟随
    /// </summary>
    public void ReqCancleFollow()
    {
        MainPlayStop();//周一过来处理， 需要停止 和不需要停止的处理

        stCancelFollowRelationUserCmd_CS cmd = new stCancelFollowRelationUserCmd_CS();

        NetService.Instance.Send(cmd);

        if (DataManager.Manager<TeamDataManager>().MainPlayerIsLeader())
        {
            TipsManager.Instance.ShowTips(LocalTextType.Team_Follow_duizhangquxiaozhaohuanchenggong);
        }
    }

    /// <summary>
    /// 队员取消跟随响应
    /// </summary>
    public void OnCancelFollow()
    {
        m_bIsFollow = false;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.TEAM_MEMBERCANCLEFOLLOW, null);

    }

    public void MainPlayStop()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            player.SendMessage(EntityMessage.EntityCommand_StopMove, player.GetPos());
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);//关闭自动寻路中
        }
        CmdManager.Instance().Clear();//清除寻路
    }

    /// <summary>
    /// 队员请求跟随
    /// </summary>
    public void ReqManualFollow()
    {
        stManualFollowRelationUserCmd_CS cmd = new stManualFollowRelationUserCmd_CS();

        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 队员跟随
    /// </summary>
    public void OnManualFollow(GameCmd.stManualFollowRelationUserCmd_CS cmd)
    {
        m_bIsFollow = true;

        isManualFollow = true;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.TEAM_MEMBERFOLLOW, null);
    }


    uint dataLimit = 2;   //控制寻路频率
    uint dataCount = 0;
    bool isManualFollow = false;

    public void OnFollowPos(stFollowPosResultRelationUserCmd_S cmd)
    {
        uint leaderMapId = cmd.mapid;
        uint x = cmd.x;
        uint y = cmd.y;


        //相同地图
        if (IsSameMap(leaderMapId))
        {
            //9屏内
            if (IsIn9Screen())
            {
                GotoMap(leaderMapId, x, y);

                dataCount = 0;
            }

            //9屏外
            else
            {
                dataCount++;

                if (isManualFollow) //第一条消息直接寻路
                {
                    isManualFollow = false;
                    GotoMap(leaderMapId, x, y);
                }

                if (dataCount >= dataLimit)
                {
                    GotoMap(leaderMapId, x, y);
                    dataCount = 0;
                }
            }

        }

        //不同地图
        else
        {
            GotoMap(leaderMapId, x, y);

            dataCount = 0;
        }
    }

    /// <summary>
    /// 是否与玩家自己是同一张地图
    /// </summary>
    /// <param name="mapId"></param>
    /// <returns></returns>
    bool IsSameMap(uint leaderMapId)
    {
        IMapSystem ms = ClientGlobal.Instance().GetMapSystem();
        if (ms != null)
        {
            uint myMapId = ms.GetMapID();

            return myMapId == leaderMapId;
        }
        else
        {
            Engine.Utility.Log.Error("IMapSystem 为 null !!!");
            return false;
        }
    }

    /// <summary>
    /// 队长是否在9屏内
    /// </summary>
    /// <returns></returns>
    bool IsIn9Screen()
    {
        uint leaderId = DataManager.Manager<TeamDataManager>().LeaderId;

        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();

        if (es != null)
        {
            IPlayer leaderPlaer = es.FindPlayer(leaderId);

            return leaderPlaer != null;  //在9屏 返回true, 否则 false
        }
        else
        {
            Engine.Utility.Log.Error("IEntitySystem 为 null !!!");
            return false;
        }
    }

    /// <summary>
    /// 寻路
    /// </summary>
    /// <param name="mapId"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void GotoMap(uint mapId, uint x, uint y)
    {
        IController ctrl = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
        if (ctrl != null)
        {
            ctrl.GotoMap(mapId, new UnityEngine.Vector3(x, 0, -y));
        }
    }


}

