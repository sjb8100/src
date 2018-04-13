using Client;
//*************************************************************************
//	创建日期:	2017-4-24 10:34
//	文件名称:	AutoMission.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	做任务 事件驱动
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;
public class MoveToTargetParam
{
    public int taskid;
    public UnityEngine.Vector3 pos;
    public uint mapID;
    public TaskSubType taskSubType;
    public uint paramId;
}

public class AutoMission : Singleton<AutoMission>
{
    const string TAG = "AutoMission";
    public static uint m_nDoingTaskID = 0;
    uint m_nGotoMapID = 0;
    bool m_bAddStopMoveListener = false;
    bool m_bAddChangeListener = false;
    bool m_bAddRestoreListener = false;
    bool m_bAddSkillNoneListener = false;

    uint usecommitItemID = 0;

    public override void Initialize()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_ACCEPT, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_CANSUBMIT, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONING, OnEvent);

    }

    public void Reset()
    {
        m_nDoingTaskID = 0;
        m_nGotoMapID = 0;

        if (m_bAddStopMoveListener)
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
            m_bAddStopMoveListener = false;
        }
        if (m_bAddChangeListener)
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGE, OnEvent);
            m_bAddChangeListener = false;
        }
        if (m_bAddRestoreListener)
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_RESTORE, OnEvent);
            m_bAddRestoreListener = false;
        }
        if (m_bAddSkillNoneListener)
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKILLNONESTATE_ENTER, OnEvent);
            m_bAddSkillNoneListener = false;
        }
    }


    void OnEvent(int nEventId, object param)
    {
        Client.GameEventID eid = (Client.GameEventID)nEventId;
        if (eid == Client.GameEventID.TASK_ACCEPT)
        {
            uint nTaskID = (uint)param;
            OnTaskAccept(nTaskID);
            return;
        }
        else if (eid == Client.GameEventID.TASK_CANSUBMIT)
        {
            Client.stTaskCanSubmit cs = (Client.stTaskCanSubmit)param;
            OnTaskCanCommit(cs.taskid);
        }
        else if (eid == Client.GameEventID.TASK_DONE)
        {
            Client.stTaskDone td = (Client.stTaskDone)param;
            OnTaskDone(td.taskid);
        }
        else if (eid == Client.GameEventID.TASK_DONING)
        {
            Client.stDoingTask dt = (Client.stDoingTask)param;
            DataManager.Manager<TaskDataManager>().DoingTaskID = dt.taskid;
            OnExecuteTask(dt.taskid);
        }
        else if (eid == Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE)
        {
            Client.stEntityStopMove stopEntity = (Client.stEntityStopMove)param;
            OnPlayerStopMove(ref stopEntity, nEventId);
            return;
        }
        else if (eid == Client.GameEventID.SKILLNONESTATE_ENTER)
        {
            Client.stSkillStateEnter sse = (Client.stSkillStateEnter)param;
            OnSkillPlayOver(ref sse, nEventId);
            return;
        }
        else if (eid == Client.GameEventID.ENTITYSYSTEM_CHANGE)
        {
            Client.stPlayerChange pc = (Client.stPlayerChange)param;

            if (!Client.ClientGlobal.Instance().IsMainPlayer(pc.uid))
            {
                return;
            }
            Engine.Utility.EventEngine.Instance().RemoveEventListener(nEventId, OnEvent);
            QuestTraceInfo questInfo = QuestTranceManager.Instance.GetQuestTraceInfo(m_nDoingTaskID);
            if (questInfo == null)
            {
                return;
            }
            m_bAddChangeListener = false;
            Engine.Utility.Log.Info("{0} 变身完毕 执行任务{1}", TAG, m_nDoingTaskID);
            ProcessTask(questInfo);
            m_nDoingTaskID = 0;

        }
        else if (eid == Client.GameEventID.ENTITYSYSTEM_RESTORE)
        {
            Client.stPlayerChange pc = (Client.stPlayerChange)param;
            if (!Client.ClientGlobal.Instance().IsMainPlayer(pc.uid))
            {
                return;
            }
            Engine.Utility.EventEngine.Instance().RemoveEventListener(nEventId, OnEvent);
            Engine.Utility.Log.Info("{0}变身回来 执行任务{1}", TAG, m_nDoingTaskID);
            OnTaskDone(m_nDoingTaskID);
            m_nDoingTaskID = 0;
            m_bAddRestoreListener = false;
        }
    }

    private void OnSkillPlayOver(ref Client.stSkillStateEnter sse, int nEventId)
    {
        if (!Client.ClientGlobal.Instance().IsMainPlayer(sse.uid))
        {
            return;
        }
        m_bAddSkillNoneListener = false;

        Engine.Utility.EventEngine.Instance().RemoveEventListener(nEventId, OnEvent);

        QuestTraceInfo questInfo = QuestTranceManager.Instance.GetQuestTraceInfo(m_nDoingTaskID);
        if (questInfo == null)
        {
            return;
        }
        Engine.Utility.Log.Info("{0}技能播放完毕 执行任务{1} {2}", TAG, m_nDoingTaskID, Client.ClientGlobal.Instance().MainPlayer.GetCurState());

        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }

        m_nDoingTaskID = 0;

        ProcessTask(questInfo);
    }

    private void OnPlayerStopMove(ref Client.stEntityStopMove stopEntity, int nEventId)
    {
        if (!Client.ClientGlobal.Instance().IsMainPlayer(stopEntity.uid))
        {
            return;
        }

        if (m_nDoingTaskID == 0)
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener(nEventId, OnEvent);
            m_bAddStopMoveListener = false;
            return;
        }

        QuestTraceInfo questInfo = QuestTranceManager.Instance.GetQuestTraceInfo(m_nDoingTaskID);
        if (questInfo == null)
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener(nEventId, OnEvent);
            m_bAddStopMoveListener = false;
            return;
        }

        if (EqualsMapID(questInfo.QuestTable.destMapID))
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener(nEventId, OnEvent);
            m_bAddStopMoveListener = false;

            uint npcid;
            Vector2 pos;
            if (questInfo.IsKillMonster(out npcid))
            {
                Client.IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
                if (ms.GetClienNpcPos((int)npcid, out pos))
                {
                    UnityEngine.Vector3 mainPos = Client.ClientGlobal.Instance().MainPlayer.GetPos();
                    if (mainPos.x == pos.x && mainPos.z == -pos.y)
                    {
                        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
                        if (cs != null)
                        {
                            cs.GetCombatRobot().StartWithTarget((int)npcid);
                            Engine.Utility.Log.LogGroup("ZCX", "挂机杀怪物{0}", npcid);
                        }
                    }
                }
            }
            else if (questInfo.IsMoveToTargetPos(out pos))
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);

                UnityEngine.Vector3 mainPos = Client.ClientGlobal.Instance().MainPlayer.GetPos();
                if (mainPos.x == pos.x && mainPos.z == -pos.y)
                {
                    if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainUsePanel))
                    {
                        return;
                    }

                    MainUsePanelData mainUsePanelData = new MainUsePanelData();
                    mainUsePanelData.type = 2;    //  type = 2为item
                    mainUsePanelData.Id = questInfo.QuestTable.usecommitItemID;
                    mainUsePanelData.onClick = MainUsePanelItemOnClick;

                    this.usecommitItemID = questInfo.QuestTable.usecommitItemID;

                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MainUsePanel, data: mainUsePanelData);

                    // DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MainUsePanel, data: questInfo.QuestTable.usecommitItemID);

                }

            }
        }
    }

    void MainUsePanelItemOnClick()
    {
        Client.IEntity player = Client.ClientGlobal.Instance().MainPlayer;
        if (player == null)
        {
            return;
        }

        bool ismoving = (bool)player.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
        if (ismoving)
        {
            player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
        }

        List<BaseItem> lstItems = DataManager.Manager<ItemManager>().GetItemByBaseId(this.usecommitItemID);
        if (lstItems.Count > 0)
        {
            uint m_nitemThisId = lstItems[0].QWThisID;

            DataManager.Manager<RideManager>().TryUnRide(
                (obj) =>
                {
                    Protocol.Instance.RequestUseItem(m_nitemThisId);
                },
                null);
        }
    }


    private void OnTaskAccept(uint nTaskID)
    {
        QuestTraceInfo questInfo = QuestTranceManager.Instance.GetQuestTraceInfo(nTaskID);
        if (questInfo == null)
        {
            Engine.Utility.Log.Error(" Get questinfo error {0}", nTaskID);
            return;
        }
        if (!questInfo.QuestTable.dwAuto)
        {
            return;
        }
        //动态追踪的等 追踪信息下来之后再做
        if (questInfo.dynamicTrance)
        {
            return;
        }
        ProcessTask(questInfo);
    }

    private void OnTaskCanCommit(uint nTaskID)
    {
        QuestTraceInfo questInfo = QuestTranceManager.Instance.GetQuestTraceInfo(nTaskID);
        if (questInfo == null)
        {
            Engine.Utility.Log.Error(" Get questinfo error {0}", nTaskID);
            return;
        }
        if (!questInfo.QuestTable.dwAutoCommit)
        {
            return;
        }

        Client.ISkillPart skillPart = Client.ClientGlobal.Instance().MainPlayer.GetPart(Client.EntityPart.Skill) as Client.ISkillPart;
        if (skillPart != null)
        {
            if (skillPart.GetCurSkillState() != (int)Client.SkillState.None)
            {
                m_nDoingTaskID = nTaskID;
                Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLNONESTATE_ENTER, OnEvent);
                Engine.Utility.Log.Info("等待.......技能播放完毕 执行任务{0} {1}", m_nDoingTaskID, Client.ClientGlobal.Instance().MainPlayer.GetCurState());
                StopRobot();
                return;
            }
        }

        ProcessTask(questInfo);
    }

    private void OnTaskDone(uint nTaskID)
    {
        table.QuestDataBase questDB = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(nTaskID);
        if (questDB == null)
        {
            Engine.Utility.Log.Error(" GET QUEST ERROR {0}", nTaskID);
            return;
        }
        if (questDB.Type != GameCmd.TaskType.TaskType_Normal)
        {
            //return;
        }

        if (questDB.dwSubType == (int)TaskSubType.ChangeBody)
        {
            //bool isChange = (bool)Client.ClientGlobal.Instance().MainPlayer.SendMessage(Client.EntityMessage.EntityCommand_IsChange, null);
            //if (isChange)
            if (MainPlayerIsChangeBody())
            {
                m_nDoingTaskID = questDB.dwID;
                if (m_bAddRestoreListener)
                {
                    return;
                }
                m_bAddRestoreListener = true;
                Engine.Utility.Log.Info("{0}OnTaskDone 等待.......变身回来 执行任务{1} {2}", TAG, m_nDoingTaskID, Client.ClientGlobal.Instance().MainPlayer.GetCurState());
                Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_RESTORE, OnEvent);
                return;
            }
            Engine.Utility.Log.Info("{0} 完成变身任务 已经变身回来接取任务{1}", TAG, questDB.dwID);
        }
        //主线任务自动接取
        table.QuestDataBase nextTaskDB = null;
        List<table.QuestDataBase> lstData = GameTableManager.Instance.GetTableList<table.QuestDataBase>();
        for (int i = 0, imax = lstData.Count; i < imax; i++)
        {
            if (!string.IsNullOrEmpty(lstData[i].dwPreTask))
            {
                string[] strTask = lstData[i].dwPreTask.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (strTask.Length > 0)
                {
                    for (int n = 0; n < strTask.Length; n++)
                    {
                        if (uint.Parse(strTask[n]) == nTaskID)
                        {
                            nextTaskDB = lstData[i];
                            break;
                        }
                    }

                }
            }
        }

        if (nextTaskDB != null)
        {
            if (!nextTaskDB.dwAutoAccept)
            {
                return;
            }

            if (nextTaskDB.dwBeginNpc == 0 || nextTaskDB.acceptMapID == 0)
            {
                return;
            }

            VisitNpc(nextTaskDB.dwHelpGoto, nextTaskDB.acceptMapID, nextTaskDB.dwBeginNpc, nextTaskDB.dwID);
        }
    }

    private void OnExecuteTask(uint nTaskID)
    {
        QuestTraceInfo questInfo = QuestTranceManager.Instance.GetQuestTraceInfo(nTaskID);
        if (questInfo == null)
        {
            Engine.Utility.Log.Error(" Get questinfo error {0}", nTaskID);
            return;
        }

        uint npcid = 0;
        if (questInfo.IsKillMonster(out npcid))
        {
            Client.ISkillPart skillPart = Client.ClientGlobal.Instance().MainPlayer.GetPart(Client.EntityPart.Skill) as Client.ISkillPart;
            if (skillPart != null)
            {
                if (skillPart.GetCurSkillState() != (int)Client.SkillState.None)
                {
                    m_nDoingTaskID = nTaskID;
                    if (m_bAddSkillNoneListener)
                    {
                        return;
                    }
                    m_bAddSkillNoneListener = true;
                    Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLNONESTATE_ENTER, OnEvent);
                    Engine.Utility.Log.Info("等待.......技能播放完毕 执行任务{0}  {1}", m_nDoingTaskID, Client.ClientGlobal.Instance().MainPlayer.GetCurState());
                    return;
                }
            }
        }
        ProcessTask(questInfo);
    }

    //static uint nProcessTaskID = 0;
    public void ProcessTask(QuestTraceInfo taskInfo)
    {
        //nProcessTaskID = taskInfo.taskId;

        if (taskInfo == null)
        {
            Engine.Utility.Log.Error("taskInfo is null");
            return;
        }

        GameCmd.TaskProcess process = taskInfo.GetTaskProcess();
        if (process == GameCmd.TaskProcess.TaskProcess_None)//去接任务
        {
            StopRobot();
            DoAcceptTask(taskInfo);
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_Doing)
        {
            StopRobot();

            OnDoingTask(taskInfo);
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_CanDone)
        {
            StopRobot();

            OnCommitTask(taskInfo);
            return;
        }
    }

    private void OnCommitTask(QuestTraceInfo taskInfo)
    {
        table.QuestDataBase questDB = taskInfo.QuestTable;
        if (questDB == null)
        {
            Engine.Utility.Log.Error("QuestTable is null");
            return;
        }

        if (taskInfo.taskSubType == TaskSubType.Guild)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MissionMessagePanel, data: taskInfo.taskId);
            return;
        }

        PanelID pid;
        int tab = 0;
        int copyId = 0;

        uint jumpId;
        if (taskInfo.IsOpenUI(out jumpId))
        {
            ItemManager.DoJump(jumpId);
            return;
        }
        else if (taskInfo.IsChangeBodyTask())
        {
            //bool isChange = (bool)Client.ClientGlobal.Instance().MainPlayer.SendMessage(Client.EntityMessage.EntityCommand_IsChange, null);
            //if (!isChange)
            //{
            //    m_nDoingTaskID = taskInfo.taskId;
            //    if (m_bAddChangeListener)
            //    {
            //        return;
            //    }

            //    m_bAddChangeListener = true;
            //    Engine.Utility.Log.Info("{0}CanDone 等待.......变身回来 执行任务{1}", TAG, m_nDoingTaskID);
            //    Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGE, OnEvent);
            //    return;
            //}

            m_nDoingTaskID = taskInfo.taskId;
            if (false == m_bAddChangeListener && false == MainPlayerIsChangeBody())
            {
                m_bAddChangeListener = true;
                Engine.Utility.Log.Info("{0}CanDone 等待.......变身回来 执行任务{1}", TAG, m_nDoingTaskID);
                Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGE, OnEvent);
            }

            VisitNpc(questDB.dwHelpCommit, questDB.submitMapID, questDB.dwEndNpc, questDB.dwID);
            return;
        }
        uint npcid = 0;
        if (taskInfo.IsKillMonster(out npcid))
        {
            Client.ISkillPart skillPart = Client.ClientGlobal.Instance().MainPlayer.GetPart(Client.EntityPart.Skill) as Client.ISkillPart;
            if (skillPart != null)
            {
                if (skillPart.GetCurSkillState() != (int)Client.SkillState.None)
                {
                    m_nDoingTaskID = taskInfo.taskId;
                    Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SKILLNONESTATE_ENTER, OnEvent);
                    Engine.Utility.Log.Info("等待.......技能播放完毕 执行任务{0}", m_nDoingTaskID);
                    return;
                }
            }
        }

        //令牌悬赏无交任务npc
        if (taskInfo.taskType != GameCmd.TaskType.TaskType_Token)
        {

            DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
            {
                VisitNpc(questDB.dwHelpCommit, questDB.submitMapID, questDB.dwEndNpc, questDB.dwID);
            }, null);
        }
    }
    private void OnDoingTask(QuestTraceInfo taskInfo)
    {
        DataManager.Manager<TaskDataManager>().DoingTaskID = taskInfo.taskId;

        Client.IController controller = Client.ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
        if (controller == null)
        {
            Engine.Utility.Log.Error("IController is null");

            return;
        }
        table.QuestDataBase questDB = taskInfo.QuestTable;
        if (questDB == null)
        {
            Engine.Utility.Log.Error("QuestTable is null");
            return;
        }
        UnityEngine.Vector2 pos;
        uint npcid = 0;
        PanelID pid;
        int copyID;
        int tab = 0;
        uint jumpId;

        //背包满了  无法执行任务
        if (false == taskInfo.TaskItemCanPutInKanpsack())
        {
            TipsManager.Instance.ShowTips(LocalTextType.Task_Commond_3);
            return;
        }

        if (MainPlayerIsChangeBody())
        {
            return;
        }
        else if (taskInfo.taskSubType == TaskSubType.SubmitLimit) //断档任务
        {
            if (taskInfo.IsOpenUI(out jumpId))
            {
                ItemManager.DoJump(jumpId);
            }
            else
            {
                string des = string.Format("将等级提升到{0}级继续主线任务", taskInfo.finishLevel);
                TipsManager.Instance.ShowTips(des);
            }
            return;

        }
        else if (taskInfo.IsDynamicCommitItem) //动态道具递交（蚩尤乱世除外）
        {
            taskInfo.DoJump();
            return;
        }
        else if (DataManager.Manager<ComBatCopyDataManager>().IsEnterCopy == false && taskInfo.IsOpenUI(out jumpId))
        {
            ItemManager.DoJump(jumpId);
            return;
        }
        else if (taskInfo.IsMoveToTargetPos(out pos))
        {
            //if (DataManager.Manager<TaskDataManager>().IsShowSlider)
            if (DataManager.Manager<SliderDataManager>().IsReadingSlider)
            {
                return;
            }
            Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
            if (cs == null)
            {
                return;
            }

            if (!m_bAddStopMoveListener)
            {
                m_bAddStopMoveListener = true;
                Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
            }

            if (EqualsMapID(questDB.destMapID))
            {
                cs.GetCombatRobot().Stop();
                DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
                {
                    //if (!m_bAddStopMoveListener)
                    //{
                    //    m_bAddStopMoveListener = true;
                    //    Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
                    //}
                    m_nDoingTaskID = taskInfo.taskId;
                    controller.GotoMap(questDB.destMapID, new Vector3(pos.x, 0, -pos.y));

                }, null);
                return;
            }
            else
            {
                DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
                {
                    if (questDB.dwHelpDoing)
                    {
                        m_nDoingTaskID = taskInfo.taskId;

                        //下载地图检查
                        if (!KHttpDown.Instance().SceneFileExists(questDB.destMapID))
                        {
                            //打开下载界面
                            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                            return;
                        }

                        controller.GotoMapDirectly(questDB.destMapID, new Vector3(pos.x, 0, -pos.y), questDB.dwID);
                    }
                }, null);
            }

        }
        else if (taskInfo.IsVisitCollectNpc(out npcid) || taskInfo.IsDeleverItem(out npcid))
        {
            AddCollectNpcEffect(taskInfo);

            DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
            {
                VisitNpc(questDB.dwHelpDoing, questDB.destMapID, npcid, questDB.dwID);
            }, null);
        }
        else if (taskInfo.IsDirectlyVisitCopy(taskInfo.copyId) && false == EqualsMapID(questDB.destMapID))
        {
            //直接跳副本            
            VisitCopy(taskInfo.copyId);
        }
        else if (taskInfo.IsKillMonster(out npcid))
        {
            Client.ICombatRobot robot = Client.ClientGlobal.Instance().GetControllerSystem().GetCombatRobot();
            if (robot == null)
            {
                Engine.Utility.Log.Error("robotis null");

                return;
            }
            if (robot.Status == Client.CombatRobotStatus.RUNNING && robot.TargetId == npcid)
            {
                Engine.Utility.Log.Info("已经在挂机杀怪{0}", npcid);
                return;
            }
            //TODO 优化
            bool getPos = false;
            if (EqualsMapID(questDB.destMapID))
            {
                Client.IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
                if (ms.GetClienNpcPos((int)npcid, out pos))
                {
                    getPos = true;
                    Vector3 mainPos = Client.ClientGlobal.Instance().MainPlayer.GetPos();
                    if ((mainPos - new Vector3(pos.x, mainPos.y, -pos.y)).sqrMagnitude < 5)
                    {
                        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
                        if (cs != null)
                        {
                            cs.GetCombatRobot().StartWithTarget((int)npcid);
                            Engine.Utility.Log.LogGroup("ZCX", "挂机杀怪物{0}", npcid);
                            return;
                        }
                    }
                }
            }

            DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
            {
                if (!m_bAddStopMoveListener)
                {
                    m_bAddStopMoveListener = true;
                    Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
                }
                m_nDoingTaskID = taskInfo.taskId;
                if (getPos)
                {
                    controller.MoveToTarget(new Vector3(pos.x, 0, -pos.y), null, true);
                }
                else if (EqualsMapID(questDB.destMapID))
                {
                    Client.IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
                    if (ms.GetClienNpcPos((int)npcid, out pos))
                    {
                        controller.MoveToTarget(new Vector3(pos.x, 0, -pos.y), null, true);
                    }
                }
                else
                {
                    VisitNpc(questDB.dwHelpDoing, questDB.destMapID, npcid, questDB.dwID);
                }

            }, null);
        }
    }
    private void DoAcceptTask(QuestTraceInfo taskInfo)
    {
        table.QuestDataBase questDB = taskInfo.QuestTable;
        if (questDB == null)
        {
            Engine.Utility.Log.Error("QuestTable is null");
            return;
        }
        if (taskInfo.taskType == GameCmd.TaskType.TaskType_Token)
        {
            PanelID pid;
            int tab = 0;
            int copyID;
            /* if (taskInfo.IsOpenUI(out pid, out tab,out copyID))
             {
 //                 DataManager.Manager<UIPanelManager>().ShowPanel(pid);
 //                 UIFrameManager.Instance.OnCilckTogglePanel(pid, UIPanelBase.FisrstTabsIndex, tab);
                 DataManager.Manager<UIPanelManager>().ShowPanel(pid, jumpData: new UIPanelBase.PanelJumpData() { Tabs = new int[] { tab } });

             }*/
            uint jumpId;
            if (taskInfo.IsOpenUI(out jumpId))
            {
                ItemManager.DoJump(jumpId);
            }
        }
        if (taskInfo.taskSubType == TaskSubType.Guild)
        {
            Protocol.Instance.RequestAcceptTask(taskInfo.taskId);
        }
        else
        {
            DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
            {
                VisitNpc(questDB.dwHelpGoto, questDB.acceptMapID, questDB.dwBeginNpc, questDB.dwID);
            }, null);
        }
    }

    //static uint nLastTaskID = 0;
    public void VisitNpc(bool bDirectly, uint nMapID, uint nNpcID, uint nTaskID)
    {
        //bool bProcess = true;
        //if (nLastTaskID == nTaskID && nProcessTaskID == nTaskID)
        //{
        //    bProcess = false;
        //}
        //else
        //{
        //    nLastTaskID = nTaskID;
        //}

        //if (bProcess == false)
        //    return;

        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }
        Client.IController controller = cs.GetActiveCtrl();
        if (controller == null)
        {
            Engine.Utility.Log.Error("IController is null");
            return;
        }

        //bDirectly = false;
        if (bDirectly)//是否直接跨地图去接取
        {
            if (EqualsMapID(nMapID))
            {
                controller.VisiteNPC(nMapID, nNpcID);
            }
            else
            {
                //下载地图检查
                if (!KHttpDown.Instance().SceneFileExists(nMapID))
                {
                    //打开下载界面
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                    return;
                }

                controller.VisiteNPCDirectly(nMapID, nNpcID, nTaskID, 100, 100);
            }
        }
        else
        {
            controller.VisiteNPC(nMapID, nNpcID, false);
        }
    }

    /// <summary>
    /// 直接跳副本
    /// </summary>
    public void VisitCopy(uint copyId)
    {
        DataManager.Manager<ComBatCopyDataManager>().ReqEnterCopy(copyId);
    }

    /// <summary>
    /// 为采集物添加特效
    /// </summary>
    /// <param name="taskInfo"></param>
    void AddCollectNpcEffect(QuestTraceInfo taskInfo)
    {
        if (taskInfo.taskSubType != TaskSubType.Collection)
        {
            return;
        }

        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        Client.INPC npc = es.FindNPCByBaseId((int)taskInfo.QuestTable.collect_npc);
        if (npc == null)
        {
            return;
        }

        DataManager.Manager<TaskDataManager>().AddCollectNpcEffect(npc.GetID(), 9002);
        DataManager.Manager<TaskDataManager>().AddCollectNpcEffect(npc.GetID(), 9003);
    }

    private bool EqualsMapID(uint nMapID)
    {
        Client.IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
        if (ms == null)
        {
            return false;
        }
        return ms.GetMapID() == nMapID;
    }

    private void StopRobot()
    {
        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }
        cs.GetCombatRobot().Stop();
    }

    /// <summary>
    /// 玩家是否变身
    /// </summary>
    /// <returns></returns>
    bool MainPlayerIsChangeBody()
    {
        bool isChange = false;
        IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            isChange = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommand_IsChange, null);
        }

        return isChange;

    }
}