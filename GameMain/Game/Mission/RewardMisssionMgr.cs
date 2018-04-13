using System;
using System.Collections.Generic;

public class StarTaskData
{
    public uint id;
    public uint star;
    public uint gold_refresh;// 使用金币刷新次数
    public uint all_refresh;//当前星级刷新次数
}

public class RewardMisssionInfo
{
    public uint id;
    public uint nType;
    public string strIcon;
    public string strName;
    public uint nExp;
    public uint ntaskid;
    /// <summary>
    /// 0 未发布 1 已发布 2 已接取 3完成
    /// </summary>
    public int nstate;
    public float nleftTime;
}
public class RewardMisssionMgr
{
    List<RewardMisssionInfo> m_lstReleaseReward = null;
    public List<RewardMisssionInfo> ReleaseRewardList { get { return m_lstReleaseReward; } }

    public RewardMisssionInfo receiveReward { get; set; }

    //任务池
    Dictionary<uint, uint> m_dictRewardTaskLeftNum = null;
    public Dictionary<uint, uint> RewardTaskLeftNum { get { return m_dictRewardTaskLeftNum; } }
    public uint RewardAcceptTimes { get; set; }//令牌任务已经接收次数
    public uint RewardReleaseTimes { get; set; }//令牌任务已经发布次数

    uint m_rewardAcceptAllTimes = 5;            //令牌悬赏任务每天最多接取数
    public uint RewardAcceptAllTimes
    {
        get
        {
            return m_rewardAcceptAllTimes;
        }
    }


    Dictionary<uint, StarTaskData> m_dicStarTaskData = null;

    public RewardMisssionMgr()
    {
        m_lstReleaseReward = new List<RewardMisssionInfo>();
        m_dictRewardTaskLeftNum = new Dictionary<uint, uint>();
        m_dicStarTaskData = new Dictionary<uint, StarTaskData>();
        List<table.AcceptTokenDataBase> lstReceive = GameTableManager.Instance.GetTableList<table.AcceptTokenDataBase>();
        for (int i = 0; i < lstReceive.Count; i++)
        {
            m_dictRewardTaskLeftNum[lstReceive[i].id] = 0;
        }
    }

    public void Reset()
    {
        receiveReward = null;
        m_lstReleaseReward.Clear();
        m_dictRewardTaskLeftNum.Clear();
        m_dicStarTaskData.Clear();

        List<table.AcceptTokenDataBase> lstReceive = GameTableManager.Instance.GetTableList<table.AcceptTokenDataBase>();
        for (int i = 0; i < lstReceive.Count; i++)
        {
            m_dictRewardTaskLeftNum[lstReceive[i].id] = 0;
        }
    }

    /// <summary>
    /// 服务器下发数据初始化
    /// </summary>
    /// <param name="data"></param>
    /// <param name="acceptTaskRemain"></param>
    /// <param name="publicTaskRemain"></param>
    public void InitTask(List<GameCmd.TokenTaskInfo> data, uint acceptTaskRemain, uint publicTaskRemain)
    {
        RewardAcceptTimes = acceptTaskRemain;//这里应当是已经接受的次数 下面也是
        RewardReleaseTimes = publicTaskRemain;
        ReleaseRewardList.Clear();
        receiveReward = null;
        RewardMisssionInfo missioninfo = null;
        for (int i = 0; i < data.Count; ++i)
        {
            missioninfo = new RewardMisssionInfo();
            //接受任务
            if (data[i].state > (int)GameCmd.TokenTaskState.TOKEN_STATE_FINISH)
            {
                table.AcceptTokenDataBase acceptData = GameTableManager.Instance.GetTableItem<table.AcceptTokenDataBase>(data[i].tokentaskid);
                if (acceptData != null)
                {
                    missioninfo.id = acceptData.id;
                    missioninfo.strIcon = acceptData.icon;
                    missioninfo.strName = acceptData.title;
                    missioninfo.nType = 2;
                    missioninfo.ntaskid = acceptData.taskid;
                    table.QuestDataBase quest = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(acceptData.taskid);
                    if (quest != null)
                    {
                        missioninfo.nExp = quest.dwRewardExp;
                        long lpasstime = DateTimeHelper.Instance.Now - data[i].time;
                        missioninfo.nleftTime = quest.dwLimitTime * 60 * 60 - lpasstime;
                    }
                    missioninfo.nstate = (int)data[i].state;

                    QuestTraceInfo squest = QuestTranceManager.Instance.GetQuestTraceInfo(missioninfo.ntaskid);
                    if (squest != null)
                    {
                        missioninfo.nstate = squest.operate == squest.state ? (int)GameCmd.TokenTaskState.TOKEN_STATE_FINISH : (int)GameCmd.TokenTaskState.TOKEN_STATE_ACCEPT;
                    }
                    receiveReward = missioninfo;
                }
                continue;
            }

            table.PublicTokenDataBase reward = GameTableManager.Instance.GetTableItem<table.PublicTokenDataBase>(data[i].tokentaskid);
            if (reward != null)
            {
                missioninfo.id = reward.id;
                missioninfo.strIcon = reward.smallicon;
                missioninfo.strName = reward.title;
                missioninfo.nType = 1;
                missioninfo.ntaskid = reward.taskid;
                Engine.Utility.Log.LogGroup("ZCX", "id :" + data[i].tokentaskid);
                table.QuestDataBase quest = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(reward.taskid);
                if (quest != null)
                {
                    missioninfo.nExp = quest.dwRewardExp;
                    long lpasstime = DateTimeHelper.Instance.Now - data[i].time;
                    missioninfo.nleftTime = quest.dwLimitTime * 60 * 60 - lpasstime;
                }
                missioninfo.nstate = (int)data[i].state;
                ReleaseRewardList.Add(missioninfo);
            }
        }
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RewardPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.RewardPanel, UIMsgID.eRewardTaskListRefresh, null);
        }
        else 
        {
            for (int i = 0; i < ReleaseRewardList.Count;i++)
            {
                if (ReleaseRewardList[i].nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_FINISH && true ==  DataManager.Manager<TaskDataManager>().FirstLoginSuccess)
                {
                    DataManager.Manager<FunctionPushManager>().AddSysMsg(new PushMsg()
                    {
                        msgType = PushMsg.MsgType.TokenTaskReward,
                        senderId = Client.ClientGlobal.Instance().MainPlayer.GetID(), //m_leaderId,
                        //senderId = cmd.dwAnswerUserID,
                        //name = cmd.byTeamName,
                        //sendName = cmd.byAnswerName,
                        sendTime = UnityEngine.Time.realtimeSinceStartup,
                        cd = 100000,
                    });
                }
            }     
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RewardMissionPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.RewardMissionPanel, UIMsgID.eRewardTaskListRefresh, null);
        }

        //悬赏任务更新当前显示环数
        UpdateQuestTraceItemInfoByType(GameCmd.TaskType.TaskType_Token);
    }

    /// <summary>
    /// 更新特定任务item的info
    /// </summary>
    /// <param name="taskType"></param>
    void UpdateQuestTraceItemInfoByType(GameCmd.TaskType taskType)
    {
        //悬赏任务更新当前显示环数
        List<QuestTraceInfo> traceTask;
        DataManager.Manager<TaskDataManager>().GetAllQuestTraceInfo(out traceTask, null);
        if (traceTask != null)
        {
            QuestTraceInfo questTraceInfo = traceTask.Find((d) => { return d.taskType == taskType; });
            if (questTraceInfo == null)
            {
                return;
            }
            questTraceInfo.FormatXmlName();

            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateTaskList, null);
            }
        }
    }

    public void DeleteTask(uint taskid)
    {
        if (receiveReward != null)
        {
            if (receiveReward.ntaskid == taskid)
            {
                receiveReward = null;
                RefreshUI();
                return;
            }
        }

        for (int i = 0; i < ReleaseRewardList.Count; ++i)
        {
            if (ReleaseRewardList[i].ntaskid == taskid)
            {
                ReleaseRewardList.RemoveAt(i);
                break;
            }
        }
        RefreshUI();
    }

    void RefreshUI()
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RewardPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.RewardPanel, UIMsgID.eRewardTaskListRefresh, null);
        }
    }
    public StarTaskData GetStarTask(uint taskid)
    {
        StarTaskData data;
        m_dicStarTaskData.TryGetValue(taskid, out data);
        return data;
    }
    public void AddStarTask(StarTaskData data)
    {
        if (!m_dicStarTaskData.ContainsKey(data.id))
        {
            m_dicStarTaskData.Add(data.id, data);
        }
        else
        {
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionMessagePanel))
            {
                if (m_dicStarTaskData[data.id].star == data.star)
                {
                    TipsManager.Instance.ShowTips(LocalTextType.MZ_Commond_1);
                }
                else if (m_dicStarTaskData[data.id].star < data.star)
                {
                    if (data.star == 2)
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.MZ_Commond_2);
                    }
                    else if (data.star == 3)
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.MZ_Commond_3);
                    }
                    else if (data.star == 4)
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.MZ_Commond_4);
                    }
                    else if (data.star == 5)
                    {
                        TipsManager.Instance.ShowTips(LocalTextType.MZ_Commond_5);
                    }
                }
            }

            m_dicStarTaskData[data.id] = data;
        }
    }
    public void RemoveAllStarTask()
    {
        m_dicStarTaskData.Clear();
    }

    public void RemoveStarTask(uint taskid)
    {
        if (m_dicStarTaskData.ContainsKey(taskid))
        {
            m_dicStarTaskData.Remove(taskid);
        }
    }

    public void Process(float deltaTime)
    {
        if (m_lstReleaseReward.Count > 0)
        {
            for (int i = 0; i < m_lstReleaseReward.Count; ++i)
            {
                m_lstReleaseReward[i].nleftTime -= deltaTime;
                if (m_lstReleaseReward[i].nleftTime <= 0)
                {
                    m_lstReleaseReward[i].nleftTime = 0;
                }
            }
        }
        if (receiveReward != null)
        {
            receiveReward.nleftTime -= deltaTime;
            if (receiveReward.nleftTime <= 0)
            {
                receiveReward.nleftTime = 0;
            }
        }
    }
}
