//*************************************************************************
//	创建日期:	2017-5-19 17:22
//	文件名称:	QuestTranceManager.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	主界面任务追踪数据管理
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Cmd;
public class QuestTranceManager : Singleton<QuestTranceManager>
{
    //已经接取的任务
    private List<QuestTraceInfo> m_lstReceivedTask = null;
    //可接取任务
    private List<QuestTraceInfo> m_lstCanReceive = null;
    //所有追踪的任务 包括已接跟可接
    private List<QuestTraceInfo> m_traceTask = null;

    public bool m_bResetData = false;

    public override void Initialize()
    {
        base.Initialize();

        m_lstReceivedTask = new List<QuestTraceInfo>(5);

        m_lstCanReceive = new List<QuestTraceInfo>(5);
        m_traceTask = new List<QuestTraceInfo>(10);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
        Reset();
        m_lstCanReceive = null;
        m_lstReceivedTask = null;
        m_traceTask = null;
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
    }

    public void Reset()
    {
        if (m_lstCanReceive != null)
        {
            m_lstCanReceive.Clear();
        }
        if (m_lstReceivedTask != null)
        {
            m_lstReceivedTask.Clear();
        }
        if (m_traceTask != null)
        {
            m_traceTask.Clear();
        }

        this.m_bResetData = true;
    }
    //-----------------------------------------------------------------------------------------------
    /// <summary>
    /// 添加接受的任务
    /// </summary>
    /// <param name="taskid"></param>
    /// <param name="state"></param>
    /// <param name="operate"></param>
    public QuestTraceInfo AddReceivedTask(uint taskid, uint state, uint operate, bool bUpdateCanReceive = true)
    {
        table.QuestDataBase questdb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(taskid);
        if (questdb == null)
        {
            Engine.Utility.Log.Error(" get quest null {0}", taskid);
            return null;
        }

        for (int k = 0; k < m_lstCanReceive.Count; k++)
        {
            if (m_lstCanReceive[k].taskId == taskid)
            {
                m_lstCanReceive.RemoveAt(k);
                break;
            }
        }

        QuestTraceInfo cri = new QuestTraceInfo(questdb, state, operate, true);
        if (cri.dynamicTrance)
        {
            Protocol.Instance.RequestTaskTip(taskid);
        }
        else
        {
            cri.UpdateDesc();
        }

        //请求任务奖励
        Protocol.Instance.RequestTaskReward(taskid);

        Engine.Utility.Log.LogGroup("LCY", "添加的  taskId ={0}", taskid);

        m_lstReceivedTask.Add(cri);
        if (bUpdateCanReceive)
        {
            UpdateCanReceiveTask();
        }
        return cri;
    }

    public bool RemoveTaskTrack(uint nTaskID)
    {
        if (m_lstReceivedTask == null)
        {
            return false;
        }
        for (int i = 0; i < m_lstReceivedTask.Count; i++)
        {
            if (m_lstReceivedTask[i].taskId == nTaskID)
            {
                m_lstReceivedTask.RemoveAt(i);
                UpdateCanReceiveTask();
                break;
            }
        }


        //检查魔族急袭可接任务
        table.QuestDataBase qDb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(nTaskID);
        if (qDb != null)
        {
            if (qDb.dwType == (uint)GameCmd.TaskType.TaskType_Demons)
            {
                Dictionary<uint, uint> completeTaskDic = DataManager.Manager<TaskDataManager>().CompleteTask;
                Dictionary<uint, uint>.Enumerator etr = completeTaskDic.GetEnumerator();

                uint count = 0;
                while (etr.MoveNext())
                {
                    table.QuestDataBase tempDb = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(etr.Current.Key);
                    if (tempDb.dwType == (uint)GameCmd.TaskType.TaskType_Demons)
                    {
                        count++;
                    }
                }

                if (count == 0)
                {
                    m_lstCanReceive.RemoveAll((d) => { return d.taskType == GameCmd.TaskType.TaskType_Demons; });
                    UpdateCanReceiveTask();
                    return true;
                }

                // 2、
                List<QuestTraceInfo> list = m_lstCanReceive.FindAll((data) => { return data.taskType == GameCmd.TaskType.TaskType_Demons; });
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    //前置任务没完成  从可接任务中删除
                    if (false == DataManager.Manager<TaskDataManager>().CheckTaskFinished(list[i].PreTask))
                    {
                        Engine.Utility.Log.LogGroup("LCY", "手动删除可接任务的  taskId ={0}", list[i].taskId);
                        m_lstCanReceive.Remove(list[i]);

                        UpdateCanReceiveTask();
                    }
                }
            }
        }

        return true;
    }

    public bool Empty()
    {
        return this.m_traceTask.Count <= 0;
    }

    void OnEvent(int nEventId, object param)
    {
        if (nEventId == (int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE)
        {
            Client.stPropUpdate prop = (Client.stPropUpdate)param;
            if (Client.ClientGlobal.Instance().IsMainPlayer(prop.uid))
            {
                if (prop.nPropIndex != (int)Client.CreatureProp.Level)
                {
                    return;
                }
                UpdateCanReceiveTask();

                for (int i = 0, imax = m_lstReceivedTask.Count; i < imax; i++)
                {
                    if (m_lstReceivedTask[i].taskSubType == TaskSubType.SubmitLimit)
                    {
                        m_lstReceivedTask[i].UpdateDesc();
                    }
                }

                ////更新任务追踪界面
                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
                {
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateTaskList, null);
                }
            }
        }
    }

    public void GetQuestTraceInfoList(bool bReceived, ref List<QuestTraceInfo> retList)
    {
        if (bReceived)
        {
            retList.AddRange(m_lstReceivedTask);
        }
        else
        {
            retList.AddRange(m_lstCanReceive);
        }
    }

    /// <summary>
    /// 获取当前追踪任务
    /// </summary>
    /// <param name="taskid"></param>
    /// <returns></returns>
    public QuestTraceInfo GetQuestTraceInfo(uint taskid)
    {
        for (int i = 0; i < m_lstReceivedTask.Count; i++)
        {
            if (m_lstReceivedTask[i].taskId == taskid)
            {
                return m_lstReceivedTask[i];
            }
        }
        for (int i = 0; i < m_lstCanReceive.Count; i++)
        {
            if (m_lstCanReceive[i].taskId == taskid)
            {
                return m_lstCanReceive[i];
            }
        }
        return null;
    }

    public void SetTaskTrackReward(uint nTaskID, List<uint> itembaseid, List<uint> itemNum, uint gold, uint money, uint exp, uint zj, uint zg, uint rep)
    {
        QuestTraceInfo data = QuestTranceManager.Instance.GetQuestTraceInfo(nTaskID);
        if (data == null)
        {
            Engine.Utility.Log.Error("get task error {0}", nTaskID);
            return;
        }
        data.Items.Clear();
        data.Items.AddRange(itembaseid);
        data.ItemNum.Clear();
        data.ItemNum.AddRange(itemNum);

        data.gold = gold;
        data.money = money;
        data.exp = exp;
        data.clanZG = zg;
        data.clanZJ = zj;
        data.clanrep = rep;
        //氏族奖励更新
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLANTASKCHANGED, nTaskID);
    }

    public string GetQuestCompleteState(uint id, string questName, uint operate, uint state)
    {
        string partOne = string.Format("<color value=\"#FAF5C8\">{0}</color>", questName);
        return string.Format("<n>{0}\t</n>", partOne);

    }

    /// <summary>
    /// 升级 删除 或者接取任务的时候刷新可以接取任务
    /// </summary>
    public void UpdateCanReceiveTask()
    {
        if (m_lstCanReceive == null)
        {
            m_lstCanReceive = new List<QuestTraceInfo>();
        }
        //m_lstCanReceive.Clear();

        if (Client.ClientGlobal.Instance().MainPlayer == null)
        {
            return;
        }
        int playerLevel = Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level);

        int job = Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.PlayerProp.Job);

        List<table.QuestDataBase> allTableQuest = GameTableManager.Instance.GetTableList<table.QuestDataBase>();

        table.QuestDataBase stTask = null;
        TaskDataManager taskMgr = DataManager.Manager<TaskDataManager>();
        for (int i = 0, imax = allTableQuest.Count; i < allTableQuest.Count; i++)
        {
            stTask = allTableQuest[i];

            bool contain = false;

            for (int k = 0; k < m_lstCanReceive.Count; k++)
            {
                if (m_lstCanReceive[k].taskId == stTask.dwID)
                {
                    contain = true;
                    break;
                }
            }

            if (contain)
            {
                continue;
            }

            if (taskMgr.CheckTaskFinished(stTask.dwID))
            {
                continue;
            }

            //职业限制
            if (stTask.job != 0 && stTask.job != job)
            {
                continue;
            }

            //不能接的任务
            if (playerLevel < stTask.dwMinLevel || playerLevel > stTask.dwMaxLevel)
            {
                continue;
            }

            //前置任务没完成直接跳过
            if (!string.IsNullOrEmpty(stTask.dwPreTask))
            {
                if (stTask.Type != GameCmd.TaskType.TaskType_Loop && !taskMgr.CheckTaskFinished(stTask.dwPreTask))
                {
                    continue;
                }
            }

            //环任务不主动添加，由服务器下发
            if (stTask.Type == GameCmd.TaskType.TaskType_Loop)
            {
                continue;
            }

            //已经接了
            if (CheckTaskAccepted(stTask.dwID))
            {
                continue;
            }
            if (stTask.dwMinLevel > playerLevel)
                continue;
            if (taskMgr.CheckTaskFinished(stTask.dwID)) //完成过的任务
                continue;

            QuestTraceInfo questTranceInfo = new QuestTraceInfo(stTask, 0, 0, false);
            questTranceInfo.UpdateDesc();

            m_lstCanReceive.Add(questTranceInfo);

            if (DataManager.Manager<TaskDataManager>().FirstLoginSuccess)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_CANACCEPT, stTask.dwID);
            }
            //引导任务
            if (stTask.dwSubType == (uint)TaskSubType.Guild)
            {
                if (stTask.startStatus == 2)//主动接取
                {
                    Protocol.Instance.RequestAcceptTask(stTask.dwID);
                }
            }
        }
    }

    private string FormateTitle(GameCmd.TaskType type, string taskTitle)
    {
        string title = taskTitle;
        if (type == GameCmd.TaskType.TaskType_Normal)
        {
            title = string.Format("<n><color value=\"green\">[主]</color></n>{0}", title);
        }
        else if (type == GameCmd.TaskType.TaskType_Sub)
        {
            title = string.Format("<n><color value=\"green\">[支]</color></n>{0}", title);
        }
        else if (type == GameCmd.TaskType.TaskType_Demons)
        {
            title = string.Format("<n><color value=\"green\">[魔]</color></n>{0}", title);
        }
        else if (type == GameCmd.TaskType.TaskType_Clan)
        {
            title = string.Format("<n><color value=\"green\">[{0}]</color></n>{1}",
                DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Clan)
                , title);
        }
        else
        {
            title = string.Format("<n><color value=\"green\">[日]</color></n>{0}", title);
        }
        return title;
    }

    public bool CheckTaskAccepted(uint taskid)
    {
        for (int i = 0, imax = m_lstReceivedTask.Count; i < imax; i++)
        {
            if (m_lstReceivedTask[i].taskId == taskid)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 过滤魔族急袭(过滤前一天的魔族急袭任务,已经接过的不显示)
    /// </summary>
    public void FilterTaskListForDemons(ref List<QuestTraceInfo> questTraceInfoList)
    {
        //取出list中所有魔族任务
        List<QuestTraceInfo> tempQuest = questTraceInfoList.FindAll((data) => { return data.taskType == GameCmd.TaskType.TaskType_Demons; });

        if (tempQuest == null)
        {
            return;
        }

        uint tempTaskId = 0;
        for (int i = 0; i < tempQuest.Count; i++)
        {
            uint taskId = tempQuest[i].taskId;
            if (m_lstReceivedTask.Exists((data) => { return data.taskId == taskId; }))
            {
                Engine.Utility.Log.LogGroup("LCY", "存在没做完的魔族急袭任务  taskId = {0}", taskId);
                tempTaskId = taskId;
            }
        }

        //存在没做完的魔族急袭任务
        if (tempTaskId != 0 && tempQuest.Count >= 2)
        {
            //删除其他魔族急袭任务，继续没做完的魔族急袭任务
            questTraceInfoList.RemoveAll((data) => { return data.taskId != tempTaskId && data.taskType == GameCmd.TaskType.TaskType_Demons; });
        }
    }
}
