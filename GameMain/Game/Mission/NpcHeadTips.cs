//*************************************************************************
//	创建日期:	2017-3-29 20:45
//	文件名称:	NpcHeadTips.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	npc 头顶任务状态提示
//*************************************************************************
using System;
using System.Collections.Generic;

public class NpcTips
{
    public uint npcid;
    public uint taskid;
    public int effectid;
    public GameCmd.TaskProcess process = GameCmd.TaskProcess.TaskProcess_Max;
}
class NpcHeadTipsManager : Singleton<NpcHeadTipsManager>
{
    const int EFFECT_CANACCEPT = 10;//黄色感叹号
    const int EFFECT_CANCOMMIT = 11; //黄色问号
    const int EFFECT_DOING = 12;//白色问号
 

    Dictionary<uint, NpcTips> m_DictNpceffect;
    public override void Initialize()
    {
        base.Initialize();
        //采用2d任务标示，旧的任务标示系统弃用
        return;
        m_DictNpceffect = new Dictionary<uint, NpcTips>();

        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_ACCEPT, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONE, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_CANSUBMIT, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_CANACCEPT, OnEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONE, OnEvent);

    }

    public override void UnInitialize()
    {
        base.UnInitialize();
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_ACCEPT, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DELETE, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_CANSUBMIT, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_CANACCEPT, OnEvent);
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DONE, OnEvent);

    }

    public void Reset()
    {
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            if (m_DictNpceffect != null)
            {
                foreach (var item in m_DictNpceffect)
                {
                    Client.IEntity en = es.FindNPC(item.Key);
                    if (en != null)
                    {
                        en.SendMessage(Client.EntityMessage.EntityCommand_RemoveLinkEffect, item.Value.effectid);
                    }
                }
                m_DictNpceffect.Clear();
            }
        }
    }

    public void OnEvent(int nEventId, object param)
    {
        if (nEventId == (int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY)
        {
            Client.stCreateEntity createEntity = (Client.stCreateEntity)param;
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }

            Client.IEntity en = es.FindEntity(createEntity.uid);
            if (en != null && en is Client.INPC)
            {
                List<QuestTraceInfo> traceTask;
                DataManager.Manager<TaskDataManager>().GetAllQuestTraceInfo(out traceTask);
                int index;
                Client.INPC npc = en as Client.INPC;
                if (IsNeedSetTip(npc, ref traceTask, out index))
                {
                    SetNpcTipsByTraceInfo(traceTask[index]);
                }
            }
        }
        else if (nEventId == (int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY)
        {
            Client.stRemoveEntity removeEntiy = (Client.stRemoveEntity)param;
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }

            Client.IEntity en = es.FindEntity(removeEntiy.uid);
            if (en != null && en is Client.INPC)
            {
                Client.INPC npc = en as Client.INPC;
                DeleteEffectByNpc(npc);
            }
        }
        else if (nEventId == (int)Client.GameEventID.TASK_ACCEPT)
        {
            uint taskId = (uint)param;
            SetNpcTipsByTaskID(taskId,true);
        }
        else if (nEventId == (int)Client.GameEventID.TASK_DELETE)
        {
            uint taskId = (uint)param;
            DeleteEffectByTaskID(taskId);
        }
        else if (nEventId == (int)Client.GameEventID.TASK_DONE)
        {
            Client.stTaskDone td = (Client.stTaskDone)param;
            DeleteEffectByTaskID(td.taskid);
        }
        else if (nEventId == (int)Client.GameEventID.TASK_CANSUBMIT)
        {
            Client.stTaskCanSubmit tcs = (Client.stTaskCanSubmit)param;
            SetNpcTipsByTaskID(tcs.taskid,false);
        }
        else if (nEventId == (int)Client.GameEventID.SYSTEM_GAME_READY)
        {
            RefreshAllNpcTips();
        }
        else if (nEventId == (int)Client.GameEventID.TASK_CANACCEPT)
        {
            uint taskId = (uint)param;
            SetNpcTipsByTaskID(taskId,false);
        }
    }

    /// <summary>
    /// 刷新任务npc头顶特效 删除任务 接受任务 可提交任务的时候调用下
    /// </summary>
    private void RefreshAllNpcTips()
    {
        List<QuestTraceInfo> traceTask = null;
        DataManager.Manager<TaskDataManager>().GetAllQuestTraceInfo(out traceTask);
        for (int n = 0; n < traceTask.Count; n++)
        {
            SetNpcTipsByTraceInfo(traceTask[n]);
        }
    }

    public bool IsHaveTips(Client.INPC npc)
    {
        if (null == npc)
            return false;
        List<QuestTraceInfo> traceTask;
        DataManager.Manager<TaskDataManager>().GetAllQuestTraceInfo(out traceTask);
        int index;
        return IsNeedSetTip(npc, ref traceTask, out index);
    }

    bool IsNeedSetTip(Client.INPC en, ref List<QuestTraceInfo> traceTask,out int index)
    {
        index = -1;
        if (en == null)
        {
            return false;
        }

        if (en.IsMonster())
        {
            return false;
        }
        QuestTraceInfo tranceInfo = null;
        GameCmd.TaskProcess process = GameCmd.TaskProcess.TaskProcess_Max;
        for (int i = 0; i < traceTask.Count; i++)
        {
            int npcid = en.GetProp((int)Client.EntityProp.BaseID);
            tranceInfo = traceTask[i];
            process = tranceInfo.GetTaskProcess() ;
            if (process == GameCmd.TaskProcess.TaskProcess_None)
            {
                if (tranceInfo.beginNpc == npcid)
                {
                    index = i;
                    return true;
                }
            }
            else if (process == GameCmd.TaskProcess.TaskProcess_CanDone ||
                process == GameCmd.TaskProcess.TaskProcess_Doing)
            {
                if (tranceInfo.endNpc == npcid)
                {
                    index = i;
                    return true;
                }
            }
        }
        return false;
    }

    void AddNpcTip(Client.IEntity entity, uint effectId,uint nTaskId,GameCmd.TaskProcess process)
    {
        NpcTips tips;
        if (m_DictNpceffect.TryGetValue(entity.GetID(),out tips))
        {
            if (tips.process == process)
            {
                return;
            }
        }

        table.FxResDataBase edb = GameTableManager.Instance.GetTableItem<table.FxResDataBase>(effectId);
        if (edb != null)
        {
            Client.AddLinkEffect node = new Client.AddLinkEffect();
            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(edb.resPath);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("找不到特效资源路径配置{0}", edb.resPath);
            }
            node.strEffectName = resDB.strPath;
            node.strLinkName = edb.attachNode;
            node.nFollowType = (int)edb.flowType;
            node.rotate = new UnityEngine.Vector3(edb.rotate[0], edb.rotate[1], edb.rotate[2]);
            node.vOffset = new UnityEngine.Vector3(edb.offset[0], edb.offset[1], edb.offset[2]);
            node.strEffectName = resDB.strPath;
            node.strLinkName = edb.attachNode;
            if (node.strEffectName.Length != 0)
            {
                int eId = (int)entity.SendMessage(Client.EntityMessage.EntityCommand_AddLinkEffect, node);
                if (m_DictNpceffect.ContainsKey(entity.GetID()))
                {
                    entity.SendMessage(Client.EntityMessage.EntityCommand_RemoveLinkEffect, m_DictNpceffect[entity.GetID()].effectid);
                    m_DictNpceffect[entity.GetID()].effectid = eId;
                    m_DictNpceffect[entity.GetID()].process = process;
                }
                else
                {
                    m_DictNpceffect.Add(entity.GetID(), new NpcTips() { effectid = eId, npcid = entity.GetID(), taskid = nTaskId, process = process });
                }
            }
        }
    }


    void SetNpcTipsByTraceInfo(QuestTraceInfo questInfo)
    {
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("实体系统为null");
            return;
        }

        GameCmd.TaskProcess process = questInfo.GetTaskProcess();


        uint npcid = 0;
        uint effectID = 0;
        if (process == GameCmd.TaskProcess.TaskProcess_None)//可接
        {
            npcid = questInfo.beginNpc;
            effectID = EFFECT_CANACCEPT;
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_CanDone)
        {
            npcid = questInfo.endNpc;
            effectID = EFFECT_CANCOMMIT;
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_Doing)
        {
            npcid = questInfo.endNpc;
            effectID = EFFECT_DOING;
        }

        Client.INPC npc = es.FindNPCByBaseId((int)npcid);

        if (npc == null)
        {
            Engine.Utility.Log.Info("查找不到npc{0}", npcid);
            return;
        }

        AddNpcTip(npc, effectID, questInfo.taskId,process);
    }

    void SetNpcTipsByTaskID(uint nTaskID,bool checkAcceptNpc)
    {
        QuestTraceInfo questInfo = QuestTranceManager.Instance.GetQuestTraceInfo(nTaskID);
        if (questInfo == null)
        {
            return;
        }
        SetNpcTipsByTraceInfo(questInfo);

        //如果是接受了任务 检测提交跟接受npc一不一样 不一样移除接受npc特效
        if (checkAcceptNpc)
        {
            if (questInfo.endNpc != questInfo.beginNpc)
            {
                Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
                if (es == null)
                {
                    Engine.Utility.Log.Error("实体系统为null");
                    return;
                }
                Client.INPC npc = es.FindNPCByBaseId((int)questInfo.beginNpc);

                DeleteEffectByNpc(npc);
            }
        }
    }

    /// <summary>
    /// 移除特效
    /// </summary>
    /// <param name="nTaskID"></param>
    void DeleteEffectByTaskID(uint nTaskID)
    {
        table.QuestDataBase questDB = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(nTaskID);
        if (questDB == null)
        {
            return;
        }
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            Engine.Utility.Log.Error("实体系统为null");
            return;
        }
        Client.INPC npc = es.FindNPCByBaseId((int)questDB.dwEndNpc);

        DeleteEffectByNpc(npc);
    }

    void DeleteEffectByNpc(Client.INPC npc)
    {
        if (npc == null)
        {
            return;
        }
        if (m_DictNpceffect.ContainsKey(npc.GetID()))
        {
            npc.SendMessage(Client.EntityMessage.EntityCommand_RemoveLinkEffect, m_DictNpceffect[npc.GetID()].effectid);
            m_DictNpceffect.Remove(npc.GetID());
        }
    }
}
