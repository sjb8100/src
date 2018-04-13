using System;
using System.Collections.Generic;
using GameCmd;
using Cmd;
using System.Xml.Linq;
using System.Linq;
using Client;
using table;
using Engine.Utility;


public class TaskDataManager : BaseModuleData, IManager, ITimer
{

    private RewardMisssionMgr m_RewardMisssionMgr = null;
    public RewardMisssionMgr RewardMisssionData
    {
        get
        {
            return m_RewardMisssionMgr;
        }
    }
    /// <summary>
    /// 已经完成的任务
    /// </summary>
    public Dictionary<uint, uint> CompleteTask;

    uint m_currvisitNpc;
    /// <summary>
    /// 最后一次访问npcid
    /// </summary>
    public uint CurrVisitNpcID { get { return m_currvisitNpc; } }
    Dictionary<uint, int> m_DictNpceffect;

    public bool FirstLoginSuccess = false;

    /// <summary>
    /// 重连
    /// </summary>
    bool m_bReconnect = false;

    //public bool IsShowSlider
    //{
    //    get;
    //    set;
    //}

    public uint DoingTaskID
    {
        get;
        set;
    }
    private uint m_nAcceptTaskId = 0;

    public int Ring
    {
        get;
        set;
    }

    /// <summary>
    /// 采集物 key: npcId , value:effect  (用于判断是否在npc上添加特效)
    /// </summary>
    Dictionary<uint, List<int>> m_dicCollectNpcEffectId = new Dictionary<uint, List<int>>();

    /// <summary>
    /// 采集物 key:npcId , value:linkId  (用于移除npc身上特效)
    /// </summary>
    Dictionary<uint, List<int>> m_dicCollectNpcEffectLinkId = new Dictionary<uint, List<int>>();

    private const int TOKENTASK_TIMERID = 2000;

    #region Interface

    public void Initialize()
    {
        CompleteTask = new Dictionary<uint, uint>();

        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_VISITNPC, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_LOADUICOMPELETE, OnEvent);
        //Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.NETWORK_CONNECTE_CLOSE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, OnEvent);
        Engine.Utility.EventEngine.Instance().AddVoteListener((int)Client.GameVoteEventID.TASK_VISITNPC_COLLECT, OnVote);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_ITEM_COLLECT_USE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_CANSUBMIT, OnEvent);

        m_DictNpceffect = new Dictionary<uint, int>();
        //IsShowSlider = false;

        QuestTranceManager.Instance.Initialize();
        NpcHeadTipsManager.Instance.Initialize();
        m_RewardMisssionMgr = new RewardMisssionMgr();
        AutoMission.Instance.Initialize();
    }
    public void ClearData()
    {

    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData == true)
        {
            LoginReset();
            if (m_RewardMisssionMgr != null)
            {
                m_RewardMisssionMgr.Reset();
            }
        }
    }

    public void LoginReset()
    {
        CompleteTask.Clear();
        //IsShowSlider = false;
        NpcHeadTipsManager.Instance.Reset();

        QuestTranceManager.Instance.Reset();
        AutoMission.Instance.Reset();

        //切换帐号或者切换角色 相当于重新登录一次，重置第一次登录成功标志
        //FirstLoginSuccess = false;
    }
    public void Process(float deltaTime)
    {
        if (m_RewardMisssionMgr != null)
        {
            m_RewardMisssionMgr.Process(deltaTime);
        }
    }
    #endregion
    public Client.INPC GetNPCByTraceInfo(QuestTraceInfo questInfo)
    {
        if (null == questInfo)
        {
            return null;
        }
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("实体系统为null");
            return null;
        }

        GameCmd.TaskProcess process = questInfo.GetTaskProcess();


        uint npcid = 0;
        uint effectID = 0;
        if (process == GameCmd.TaskProcess.TaskProcess_None)//可接
        {
            npcid = questInfo.beginNpc;
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_CanDone)
        {
            npcid = questInfo.endNpc;
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_Doing)
        {
            npcid = questInfo.endNpc;
        }
        Client.INPC npc = es.FindNPCByBaseId((int)npcid);
        return npc;

    }
    public bool OnVote(int nEvenid, object param)
    {
        if (nEvenid == (int)Client.GameVoteEventID.TASK_VISITNPC_COLLECT)//是否播放采集动作
        {
            //if (IsShowSlider)
            if (DataManager.Manager<SliderDataManager>().IsReadingSlider)
            {
                return false;
            }
            uint npcid = (uint)param;
            List<QuestTraceInfo> list = new List<QuestTraceInfo>();
            QuestTranceManager.Instance.GetQuestTraceInfoList(true, ref list);
            string strDesc = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].taskSubType != TaskSubType.Collection)
                {
                    continue;
                }
                if (npcid == list[i].QuestTable.collect_npc)
                {
                    return true;
                }
            }
            if (DataManager.Manager<CampCombatManager>().isEnterScene)
            {
                return true;
            }
        }
        return false;
    }

    public void OnEvent(int eventid, object param)
    {
        if (eventid == (int)Client.GameEventID.TASK_VISITNPC)
        {
            m_currvisitNpc = (uint)param;
        }
        else if (eventid == (int)Client.GameEventID.TASK_ITEM_COLLECT_USE)
        {
            Client.stTaskNpcItem stData = (Client.stTaskNpcItem)param;
            TaskItemCollect(stData);
        }
        else if (eventid == (int)Client.GameEventID.SYSTEM_LOADUICOMPELETE)
        {
            if (!FirstLoginSuccess)
            {
                FirstLoginSuccess = true;

                if (m_nAcceptTaskId != 0)
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_ACCEPT, m_nAcceptTaskId);
                    m_nAcceptTaskId = 0;
                }
            }
        }
        else if ((int)Client.GameEventID.RECONNECT_SUCESS == eventid)
        {
            this.FirstLoginSuccess = true;

            //大重连  服务器会重发所有消息
            Client.stReconnectSucess rs = (Client.stReconnectSucess)param;
            if (rs.isLogin)
            {
                this.m_bReconnect = true;

                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.NpcDialogPanel))
                {
                    DataManager.Manager<UIPanelManager>().HidePanel(PanelID.NpcDialogPanel);
                    DataManager.Manager<UIPanelManager>().ShowMain();
                }
                Engine.Utility.Log.Info("重连成功关闭任务追踪界面");
                //DataManager.Manager<UIPanelManager>().HidePanel(PanelID.MissionAndTeamPanel);
            }
        }
        else if ((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY == eventid)
        {
            Client.stCreateEntity createEntity = (Client.stCreateEntity)param;
            AddEntityEffect(createEntity);
        }
        else if ((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY == eventid)
        {
            Client.stRemoveEntity removeEntity = (Client.stRemoveEntity)param;
            RemoveEntityEffect(removeEntity);
        }
        else if ((int)Client.GameEventID.TASK_CANSUBMIT == eventid)
        {
            Client.stTaskCanSubmit taskSubmit = (Client.stTaskCanSubmit)param;
            RemoveCollectNpcEffectByTaskId(taskSubmit.taskid);
        }

    }




    public bool CheckTaskFinished(string strtaskid)
    {
        string[] taskids = strtaskid.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < taskids.Length; i++)
        {
            if (string.IsNullOrEmpty(taskids[i]))
            {
                continue;
            }
            uint taskindex = 0;
            if (uint.TryParse(taskids[i], out taskindex))
            {
                if (taskindex != 0 && this.CompleteTask.ContainsKey(taskindex))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckTaskFinished(uint taskid)
    {
        return this.CompleteTask.ContainsKey(taskid);
    }

    public void UpdateTaskTraceUI(uint taskid)
    {
        //更新任务追踪界面
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateTaskList, null);
        }
    }

    /// <summary>
    /// 获取到所有在追踪的任务（包括可以接受，在做）
    /// </summary>
    /// <param name="traceTask"></param>
    public void GetAllQuestTraceInfo(out List<QuestTraceInfo> traceTask, List<GameCmd.TaskType> lstfiliterTaskType = null)
    {
        /// 用于全部任务 首先显示其中可接任务的主线任务
        traceTask = new List<QuestTraceInfo>();
        QuestTranceManager.Instance.GetQuestTraceInfoList(true, ref traceTask);
        QuestTranceManager.Instance.GetQuestTraceInfoList(false, ref traceTask);

        //过滤魔族急袭
        QuestTranceManager.Instance.FilterTaskListForDemons(ref traceTask);
    }

    public void ShowRewardTips(uint taskid)
    {
        QuestTraceInfo questTrace = QuestTranceManager.Instance.GetQuestTraceInfo(taskid);
        if (questTrace != null)
        {
            if (questTrace.gold > 0)
            {
                //进背包飘字,先注释
                //TipsManager.Instance.ShowTips("获得金币X" + questTrace.gold);
            }
            if (questTrace.money > 0)
            {
                //进背包飘字,先注释
                //TipsManager.Instance.ShowTips("获得文钱X" + questTrace.money);
            }
            if (questTrace.Items.Count > 0)
            {
                for (int i = 0; i < questTrace.Items.Count; ++i)
                {
                    table.ItemDataBase itemDb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(questTrace.Items[i]);
                    if (itemDb != null)
                    {
                        //进背包飘字,先注释
                        //TipsManager.Instance.ShowTips("获得" + itemDb.itemName + "X" + questTrace.ItemNum[i]);
                    }
                }
            }
        }
    }

    public void AddStarTask(StarTaskData data)
    {
        m_RewardMisssionMgr.AddStarTask(data);
    }

    public void RemoveStarTask(uint taskid)
    {
        m_RewardMisssionMgr.RemoveStarTask(taskid);
    }

    public void RemoveAllStarTask()
    {
        m_RewardMisssionMgr.RemoveAllStarTask();
    }

    public StarTaskData GetStarTask(uint taskid)
    {
        return m_RewardMisssionMgr.GetStarTask(taskid);
    }

    //-----------------------------------------------------------------------------------------------

    public void InitTasks(List<GameCmd.SerializeTask> dataList)
    {
        LoginReset();

        GameCmd.SerializeTask stTask = null;
        for (int i = 0, imax = dataList.Count; i < imax; i++)
        {
            stTask = dataList[i];

            if (stTask.getTaskProcess() == GameCmd.TaskProcess.TaskProcess_Done)
            {
                Engine.Utility.Log.Info("完成的 taskId = " + stTask.id);

                AddCompleteTask(stTask.id, false);
            }
            else
            {
                QuestTraceInfo quest = QuestTranceManager.Instance.AddReceivedTask(stTask.id, stTask.state, stTask.operate, false);

                if (quest != null)
                {
                    Engine.Utility.Log.Info("初始化任务数据 {0} {1} {2}", quest.QuestTable.strName, quest.operate, quest.state);
                }

            }
        }
        QuestTranceManager.Instance.UpdateCanReceiveTask();

        if (FirstLoginSuccess)
        {
            if (!DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
            {
                Engine.Utility.Log.Info("重连游戏 显示 任务界面");
                if (false == DataManager.Manager<NvWaManager>().EnterNvWa)
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MissionAndTeamPanel);
                }
            }
        }
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
        {
            Engine.Utility.Log.Info("重连游戏 刷新 任务界面");
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateTaskList, null);
        }


    }

    public void AddTask(uint nTaskID, uint nState, uint nOperate)
    {
        QuestTraceInfo questInfo = QuestTranceManager.Instance.AddReceivedTask(nTaskID, nState, nOperate);

        if (questInfo == null)
        {
            return;
        }

        //变身不立即执行
        if (questInfo.taskSubType != TaskSubType.ChangeBody && !questInfo.dynamicTrance)
        {
            if (!FirstLoginSuccess)
            {
                m_nAcceptTaskId = nTaskID;
                return;
            }
            Engine.Utility.Log.LogGroup("ZCX", "doing task {0}", nTaskID);
        }

        //动态追踪的等 消息回来再刷险
        if (!questInfo.dynamicTrance)
        {
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateTaskList, null);
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_ACCEPT, nTaskID);
    }

    public void AddCompleteTask(uint nTaskID, bool bDispatch = true)
    {
        if (!CompleteTask.ContainsKey(nTaskID))
            CompleteTask.Add(nTaskID, nTaskID);
        if (!FirstLoginSuccess)
        {
            return;
        }
        if (bDispatch)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONE, new Client.stTaskDone() { taskid = nTaskID });
        }
    }

    public void DeleteTaskByTaskID(uint taskid)
    {
        if (CompleteTask.ContainsKey(taskid))
        {
            CompleteTask.Remove(taskid);
        }
        QuestTranceManager.Instance.RemoveTaskTrack(taskid);

        UpdateTaskTraceUI(taskid);

        table.QuestDataBase qtable = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(taskid);
        if (qtable != null)
        {
            if (qtable.dwType == (int)GameCmd.TaskType.TaskType_Token)
            {
                m_RewardMisssionMgr.DeleteTask(taskid);
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DELETE, taskid);
        //TODO 刷新UI
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionPanel, UIMsgID.eDeleteTask, taskid);
        }
    }

    public void UpdateTaskOperate(uint nTaskID, uint nOperate)
    {
        QuestTraceInfo quest = QuestTranceManager.Instance.GetQuestTraceInfo(nTaskID);
        if (quest == null)
        {
            Engine.Utility.Log.Error("GET QuestTrace is null {0}", nTaskID);
            return;
        }
        quest.operate = nOperate;
        {
            quest.time = UnityEngine.Time.realtimeSinceStartup;
            //必须刷新追踪描述 才能做下一步
            quest.UpdateDesc();
            if (quest.GetTaskProcess() == TaskProcess.TaskProcess_CanDone)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_CANSUBMIT,
                             new Client.stTaskCanSubmit() { taskid = nTaskID, desc = quest.strDesc, state = quest.state, oprate = quest.operate });
            }

            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eTask_Refresh_QuestInfo, nTaskID);
            }
        }
    }

    public void UpdateTaskState(uint nTaskID, uint nState)
    {
        QuestTraceInfo quest = QuestTranceManager.Instance.GetQuestTraceInfo(nTaskID);
        if (quest == null)
        {
            Engine.Utility.Log.Error("GET QuestTrace is null {0}", nTaskID);
            return;
        }
        quest.state = nState;
        //必须刷新追踪描述
        if (quest.state != 0)
        {
            quest.UpdateDesc();
        }
        else
        {
            ShowRewardTips(nTaskID);
            AddCompleteTask(nTaskID, true);
            QuestTranceManager.Instance.RemoveTaskTrack(nTaskID);

            if (quest.taskType == TaskType.TaskType_Token)
            {
                m_RewardMisssionMgr.DeleteTask(nTaskID);
            }
        }

        //更新追踪任务UI
        UpdateTaskTraceUI(nTaskID);
    }

    /// <summary>
    /// 更新动态任务追踪
    /// </summary>
    public void UpdateTaskTrace(uint nTaskID, uint nTextID, List<string> lstParams)
    {
        QuestTraceInfo quest = QuestTranceManager.Instance.GetQuestTraceInfo(nTaskID);
        if (quest != null)
        {
            string strDesc = LangTalkData.GetTextById(nTextID);
            strDesc = strDesc.Replace("\n", "");
            if (quest.taskSubType == TaskSubType.DeliverItem)
            {
                if (lstParams.Count > (int)GameCmd.RandomParamType.SubmitItemID - 1)
                {
                    quest.QuestTable.usecommitItemID = uint.Parse(lstParams[(int)GameCmd.RandomParamType.SubmitItemID - 1]);
                    table.QuestItemDataBase questItemDb = GameTableManager.Instance.GetTableItem<table.QuestItemDataBase>(quest.QuestTable.usecommitItemID);
                    if (questItemDb != null && questItemDb.dwSubType == (uint)TaskSubType.DeliverItem)
                    {
                        quest.QuestTable.destMapID = questItemDb.destMapID;
                        quest.QuestTable.monster_npc = questItemDb.monster_npc;
                        quest.QuestTable.dwDoingNpc = questItemDb.monster_npc;
                    }
                }
            }
            else if (quest.taskSubType == TaskSubType.UseItem)
            {
                if (lstParams.Count > (int)GameCmd.RandomParamType.UseItemID - 1)
                {
                    quest.QuestTable.usecommitItemID = uint.Parse(lstParams[(int)GameCmd.RandomParamType.UseItemID - 1]);
                }

                quest.QuestTable.destMapID = uint.Parse(lstParams[(int)GameCmd.RandomParamType.JumpMap - 1]);
            }
            else if (quest.taskSubType == TaskSubType.KillMonster /*|| quest.taskSubType == TaskSubType.KillMonsterCollect */|| quest.taskSubType == TaskSubType.CallMonster)
            {
                if (lstParams.Count > (int)GameCmd.RandomParamType.KillNpcMap - 1)
                {
                    quest.QuestTable.destMapID = uint.Parse(lstParams[(int)GameCmd.RandomParamType.KillNpcMap - 1]);
                }
                if (lstParams.Count > (int)GameCmd.RandomParamType.KillNpcID - 1)
                {
                    quest.QuestTable.monster_npc = uint.Parse(lstParams[(int)GameCmd.RandomParamType.KillNpcID - 1]);
                }
            }
            else if (quest.taskSubType == TaskSubType.KillMonsterCollect)
            {
                if (quest.taskType == TaskType.TaskType_Loop)
                {
                    quest.QuestTable.destMapID = uint.Parse(lstParams[(int)GameCmd.RandomParamType.JumpMap - 1]);
                    quest.QuestTable.usecommitItemID = uint.Parse(lstParams[(int)GameCmd.RandomParamType.KillDropItemID - 1]);
                    quest.QuestTable.monster_npc = uint.Parse(lstParams[(int)GameCmd.RandomParamType.KillDropNpcID - 1]);
                    //uint.Parse(lstParams[(int)GameCmd.RandomParamType.KillDropNum - 1]);
                }
                else
                {
                    if (lstParams.Count > (int)GameCmd.RandomParamType.KillNpcMap - 1)
                    {
                        quest.QuestTable.destMapID = uint.Parse(lstParams[(int)GameCmd.RandomParamType.KillNpcMap - 1]);
                    }
                    if (lstParams.Count > (int)GameCmd.RandomParamType.KillNpcID - 1)
                    {
                        quest.QuestTable.monster_npc = uint.Parse(lstParams[(int)GameCmd.RandomParamType.KillNpcID - 1]);
                    }
                }
            }
            else if (quest.taskSubType == TaskSubType.Collection)
            {
                if (lstParams.Count > (int)GameCmd.RandomParamType.CollectNpcMap - 1)
                {
                    quest.QuestTable.destMapID = uint.Parse(lstParams[(int)GameCmd.RandomParamType.CollectNpcMap - 1]);
                }
                if (lstParams.Count > (int)GameCmd.RandomParamType.CollectNpcID - 1)
                {
                    quest.QuestTable.collect_npc = uint.Parse(lstParams[(int)GameCmd.RandomParamType.CollectNpcID - 1]);
                }

                //if (lstParams.Count>2)
                //{
                //    quest.QuestTable.coll
                //}    
            }
            else if (quest.taskSubType == TaskSubType.Talk)
            {
                if (lstParams.Count > (int)GameCmd.RandomParamType.TalkNpcMap - 1)
                {
                    quest.QuestTable.submitMapID = uint.Parse(lstParams[(int)GameCmd.RandomParamType.TalkNpcMap - 1]);
                }
                if (lstParams.Count > (int)GameCmd.RandomParamType.TalkNpcID - 1)
                {
                    quest.QuestTable.dwEndNpc = uint.Parse(lstParams[(int)GameCmd.RandomParamType.TalkNpcID - 1]);
                }

            }

            quest.dynamicTranceUpdate = true;
            quest.QuestTable.strTaskTraceBegin = strDesc;
            quest.UpdateDesc();

            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MissionAndTeamPanel))
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MissionAndTeamPanel, UIMsgID.eUpdateTaskList, null);
            }

            if (FirstLoginSuccess)
            {
                if (!quest.QuestTable.dwAuto)//如果非自动 则不发送做任务消息
                {
                    return;
                }

                if (m_bReconnect)
                {
                    m_bReconnect = false;
                    return;
                }
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                new Client.stDoingTask() { taskid = quest.taskId, state = quest.state, oprate = quest.operate, desc = strDesc });
            }
        }
    }

    public void OnTalkData(LangTalkData langtalkdata, string strStep)
    {
        if (langtalkdata == null)
        {
            return;
        }
        if (langtalkdata.npcType == LangTalkData.NpcType.MissionTalk)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NpcDialogPanel, data: langtalkdata);
        }
        //if (langtalkdata.npcType == LangTalkData.NpcType.CityWarOnly)
        //{
        //    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NpcDialogPanel, data: langtalkdata);
        //}
        else if (langtalkdata.npcType == LangTalkData.NpcType.TalkOnly)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NpcDialogPanel, data: langtalkdata);
        }
        else if (langtalkdata.npcType == LangTalkData.NpcType.MissionNoTalk)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MissionMessagePanel, data: langtalkdata);
        }
        else if (langtalkdata.npcType == LangTalkData.NpcType.TransferNpc)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.NpcDeliverPanel, data: langtalkdata);

        }
        else if (langtalkdata.npcType == LangTalkData.NpcType.Transmit)
        {
            NetService.Instance.Send(new GameCmd.stDialogSelectScriptUserCmd_C()
            {
                step = strStep,
                dwChose = 1,
            });


            DataManager.Manager<UIPanelManager>().ShowLoading();

            Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
            if (cs == null)
            {
                Engine.Utility.Log.Error("ExecuteCmd: ControllerSystem is null");
                return;
            }

            if (cs.GetCombatRobot().Status != Client.CombatRobotStatus.STOP)
            {
                cs.GetCombatRobot().Stop();
            }
        }
        else if (langtalkdata.npcType == LangTalkData.NpcType.MissionAutoReceive)
        {
            NetService.Instance.Send(new GameCmd.stDialogSelectScriptUserCmd_C()
            {
                step = strStep,
                dwChose = 1,
            });
        }
    }

    public void GetCanReceiveQuest(ref List<QuestTraceInfo> retList)
    {
        QuestTranceManager.Instance.GetQuestTraceInfoList(false, ref retList);
    }

    /// <summary>
    /// 变身完毕接着完成任务
    /// </summary>
    /// <param name="nTaskID"></param>
    public void OnRestoreCallBack(uint nTaskID)
    {
        QuestTraceInfo questInfo = QuestTranceManager.Instance.GetQuestTraceInfo(nTaskID);
        if (questInfo != null)
        {
            //Engine.Utility.Log.LogGroup("ZCX", "restore over task{0} {1}", taskid,player.GetPos());

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                new Client.stDoingTask() { taskid = nTaskID, state = questInfo.state, oprate = questInfo.operate });
        }
    }

    public void OnReceiveTokenTask(uint nTokenTaskID)
    {
        RewardMisssionInfo missioninfo = new RewardMisssionInfo();
        table.AcceptTokenDataBase reward = GameTableManager.Instance.GetTableItem<table.AcceptTokenDataBase>(nTokenTaskID);
        if (reward != null)
        {
            missioninfo.id = reward.id;
            missioninfo.strIcon = reward.icon;
            missioninfo.strName = reward.title;
            missioninfo.nType = 2;
            missioninfo.ntaskid = reward.taskid;
            table.QuestDataBase quest = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(reward.taskid);
            if (quest != null)
            {
                missioninfo.nExp = quest.dwRewardExp;
                missioninfo.nleftTime = quest.dwLimitTime * 60 * 60.0f;
            }
            missioninfo.nstate = (int)TokenTaskState.TOKEN_STATE_ACCEPT;
            m_RewardMisssionMgr.receiveReward = missioninfo;
            m_RewardMisssionMgr.RewardAcceptTimes++;

            //直接去做悬赏任务
            //GoToDoTokenTask();
            //延时去做悬赏任务（服务器的悬赏任务数据有可能还没下发）
            TimerAxis.Instance().KillTimer(TOKENTASK_TIMERID, this);
            TimerAxis.Instance().SetTimer(TOKENTASK_TIMERID, 200, this);
        }

        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RewardPanel))
        {
            //DataManager.Manager<UIPanelManager>().SendMsg(PanelID.RewardPanel, UIMsgID.eRewardTaskListRefresh, (int)nTokenTaskID);
            //策划要求直接关掉悬赏界面
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.RewardPanel);
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


    /// <summary>
    /// 接到悬赏任务后去做任务
    /// </summary>
    /// <param name="receiveTaskId"></param>
    void GoToDoTokenTask()
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.RewardPanel);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.DailyPanel);
        if (m_RewardMisssionMgr.receiveReward != null)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
new Client.stDoingTask() { taskid = m_RewardMisssionMgr.receiveReward.ntaskid });
        }
    }

    /// <summary>
    /// 悬赏任务数量
    /// </summary>
    /// <param name="nTokentaskid"></param>
    /// <param name="nNum"></param>
    public void OnGetTokenTaskNum(List<uint> lstTaskNum)
    {
        Dictionary<uint, uint> dict = RewardMisssionData.RewardTaskLeftNum;

        if (dict != null)
        {
            List<uint> lstKey = new List<uint>(dict.Keys);
            lstKey.Sort();
            for (int i = 0; i < lstKey.Count; i++)
            {
                uint num = dict[lstKey[i]];
                dict[lstKey[i]] = lstTaskNum[i];
                if (dict[lstKey[i]] != num)
                {
                    if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RewardPanel))
                    {
                        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.RewardPanel, UIMsgID.eRewardTaskCardNum, (int)lstKey[i]);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 环任务刷新  0表示没开始 -1表示第一轮已经结束 第二轮还没开始 其他表示正在做某一环	
    /// </summary>
    /// <param name="ring"></param>
    public void OnRefreshRingTask(int ring)
    {
        this.Ring = ring;

        // 0表示没开始
        if (ring == 0)
        {

        }

        // -11表示第一轮已经结束 第二轮还没开始
        else if (ring == -11)
        {
            //继续接
            Action agree = delegate
            {
                uint jumpId = GameTableManager.Instance.GetGlobalConfig<uint>("RingTaskContinueJumpId");
                ItemManager.DoJump(jumpId);
            };

            Action refuse = delegate
            {

            };

            string des = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Task_Rings_1);
            TipsManager.Instance.ShowTipWindow(10, 0, Client.TipWindowType.CancelOk, des, agree, refuse, okstr: "继续任务",
               cancleStr: "稍后继续", title: "组队邀请");
        }

        // 其他表示正在做某一环
        else if (ring > 0)
        {
            //日环任务更新当前显示环数
            UpdateQuestTraceItemInfoByType(GameCmd.TaskType.TaskType_Loop);
        }
    }


    public bool CanGetReward()
    {
        List<RewardMisssionInfo> lstMission = RewardMisssionData.ReleaseRewardList;
        for (int i = 0; i < lstMission.Count; i++)
        {
            if (lstMission[i].nstate == (int)GameCmd.TokenTaskState.TOKEN_STATE_FINISH)
            {
                return true;
            }
        }
        return false;
    }

    uint tempNpcId = 0;

    void TaskItemCollect(Client.stTaskNpcItem stData)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainUsePanel))
        {
            return;
        }

        MainUsePanelData mainUsePanelData = new MainUsePanelData();
        mainUsePanelData.type = stData.type;
        mainUsePanelData.Id = stData.Id;
        mainUsePanelData.onClick = MainUsePanelItemOnClick;

        this.tempNpcId = stData.Id;

        bool enterCityWar = DataManager.Manager<CityWarManager>().EnterCityWar;
        bool enterCamp = DataManager.Manager<CampCombatManager>().isEnterScene;

        //城战，阵营战不出采集，这两个副本是占领塔
        if (enterCityWar || enterCamp)
        {
            return;
        }

        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MainUsePanel, data: mainUsePanelData);

    }

    void MainUsePanelItemOnClick()
    {
        DataManager.Manager<RideManager>().TryUnRide(
                (obj) =>
                {
                    GameCmd.stClickNpcScriptUserCmd_C msg = new GameCmd.stClickNpcScriptUserCmd_C();
                    msg.dwNpcTempID = this.tempNpcId;
                    NetService.Instance.Send(msg);
                },
                null);
    }


    /// <summary>
    /// 为采集物添加特效
    /// </summary>
    /// <param name="npcEntity">npc</param>
    void AddEntityEffect(Client.stCreateEntity npcEntity)
    {
        QuestDataBase questDb = GameTableManager.Instance.GetTableItem<QuestDataBase>(DoingTaskID);
        if (questDb == null)
        {
            return;
        }

        if (questDb.dwSubType != (uint)TaskSubType.Collection)
        {
            return;
        }

        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        IEntity npc = es.FindEntity(npcEntity.uid);
        if (npc == null)
        {
            return;
        }

        //List<QuestTraceInfo> traceTask;
        //GetAllQuestTraceInfo(out traceTask, null);
        //if (traceTask == null)
        //{
        //    return;
        //}

        ////正在采集为   要加特效
        //QuestTraceInfo questTraceInfo = traceTask.Find((data) => { return data.taskId == DoingTaskID && data.taskSubType == TaskSubType.Collection; });

        //if (questTraceInfo == null)
        //{
        //    RemoveCollectNpcEffectAndCleanDataByNpcId(npc.GetID());
        //    return;
        //}

        uint npcBaseId = (uint)npc.GetProp((int)EntityProp.BaseID);
        if (questDb.collect_npc != npcBaseId)
        {
            RemoveCollectNpcEffectAndCleanDataByNpcId(npc.GetID());
            return;
        }

        AddCollectNpcEffect(npc.GetID(), 9002);
        AddCollectNpcEffect(npc.GetID(), 9003);

    }

    /// <summary>
    /// 移除采集物特效（当npc移除时同时移除特效）
    /// </summary>
    /// <param name="removeEntity"></param>
    void RemoveEntityEffect(Client.stRemoveEntity removeEntity)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        IEntity npc = es.FindEntity(removeEntity.uid);
        if (npc == null)
        {
            return;
        }

        RemoveCollectNpcEffectAndCleanDataByNpcId(npc.GetID());
    }

    /// <summary>
    /// 为采集物添加特效
    /// </summary>
    /// <param name="npcId">npcId</param>
    /// <param name="effectId">特效ID</param>
    public void AddCollectNpcEffect(uint npcId, int effectId)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        INPC npc = es.FindNPC(npcId);
        if (npc == null)
        {
            return;
        }

        List<int> effectIdList;
        if (m_dicCollectNpcEffectId.TryGetValue(npcId, out effectIdList))
        {
            if (false == effectIdList.Contains(effectId))
            {
                effectIdList.Add(effectId);

                List<int> linkIdList;
                if (m_dicCollectNpcEffectLinkId.TryGetValue(npcId, out linkIdList))
                {
                    int linkId = EntitySystem.EntityHelper.AddEffect(npc, effectId);
                    linkIdList.Add(linkId);
                }
            }
        }
        else
        {
            effectIdList = new List<int>();
            effectIdList.Add(effectId);
            m_dicCollectNpcEffectId.Add(npcId, effectIdList);

            int linkId = EntitySystem.EntityHelper.AddEffect(npc, effectId);
            List<int> linkIdList = new List<int>();
            linkIdList.Add(linkId);
            m_dicCollectNpcEffectLinkId.Add(npcId, linkIdList);
        }
    }

    /// <summary>
    /// 移除npc身上特效
    /// </summary>
    /// <param name="npcId"></param>
    public bool RemoveCollectNpcEffectByNpcId(uint npcId)
    {
        List<int> linkIdList;
        if (m_dicCollectNpcEffectLinkId.TryGetValue(npcId, out linkIdList))
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                return false;
            }

            INPC npc = es.FindNPC(npcId);
            if (npc == null)
            {
                return false;
            }

            for (int i = 0; i < linkIdList.Count; i++)
            {
                EntitySystem.EntityHelper.RemoveEffect(npc, linkIdList[i]);
            }
            return true;
        }

        return false;
    }

    /// <summary>
    /// 清特效数据
    /// </summary>
    void CleanCollectNpcEffectData()
    {
        m_dicCollectNpcEffectLinkId.Clear();
        m_dicCollectNpcEffectId.Clear();
    }

    /// <summary>
    /// 移除npc身上特效同时清除数据
    /// </summary>
    /// <param name="npcId"></param>
    public void RemoveCollectNpcEffectAndCleanDataByNpcId(uint npcId)
    {
        if (RemoveCollectNpcEffectByNpcId(npcId))
        {
            CleanCollectNpcEffectData();
        }
    }

    /// <summary>
    /// 移除npc身上特效（采集完成可提交时）
    /// </summary>
    /// <param name="taskId"></param>
    public void RemoveCollectNpcEffectByTaskId(uint taskId)
    {
        table.QuestDataBase taskInfo = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(taskId);

        if (taskInfo == null)
        {
            return;
        }

        if (taskInfo.dwSubType != (uint)TaskSubType.Collection)
        {
            return;
        }

        Dictionary<uint, List<int>>.Enumerator etr = m_dicCollectNpcEffectLinkId.GetEnumerator();
        while (etr.MoveNext())
        {
            RemoveCollectNpcEffectByNpcId(etr.Current.Key);
        }
        CleanCollectNpcEffectData();
    }


    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == TOKENTASK_TIMERID)
        {
            GoToDoTokenTask();
            TimerAxis.Instance().KillTimer(TOKENTASK_TIMERID, this);
        }
    }
}


