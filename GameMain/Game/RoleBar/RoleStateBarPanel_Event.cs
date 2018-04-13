/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.RoleBar
 * 创建人：  wenjunhua.zqgame
 * 文件名：  RoleStateBarPanel_Event
 * 版本号：  V1.0.0.0
 * 创建时间：7/27/2017 10:13:50 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

partial class RoleStateBarPanel: IGlobalEvent
{
    #region IGlobalEvent
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            // 注册事件
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_SETHIDE, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANQUIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANJOIN, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANREFRESHID, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANDeclareInfoRemove, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CLANDeclareInfoAdd, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TITLE_WEAR, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CITYWARWINERCLANID, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.CAMERA_MOVE_END, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RFRESHENTITYHEADSTATUS, GlobalEventHandler);

            //任务
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_ACCEPT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DELETE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_DONING, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_CANSUBMIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.TASK_CANACCEPT, GlobalEventHandler);

            //实体移动
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYMOVE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, GlobalEventHandler);

            //骑乘
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_RIDE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_UNRIDE, GlobalEventHandler);

            //外观改变
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGE, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_SETHIDE, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANQUIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANJOIN, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANREFRESHID, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANDeclareInfoRemove, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CLANDeclareInfoAdd, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TITLE_WEAR, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CITYWARWINERCLANID, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.CAMERA_MOVE_END, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RFRESHENTITYHEADSTATUS, GlobalEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_ACCEPT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DELETE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DONE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_DONING, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_CANSUBMIT, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.TASK_CANACCEPT, GlobalEventHandler);

            //实体移动
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYMOVE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, GlobalEventHandler);

            //骑乘
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_RIDE, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_UNRIDE, GlobalEventHandler);

            //外观改变
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGE, GlobalEventHandler);
        }
    }

    /// <summary>
    /// 事件处理
    /// </summary>
    /// <param name="nEventID"></param>
    /// <param name="param"></param>
    public void GlobalEventHandler(int eventID, object param)
    {
        switch (eventID)
        {
            case (int)Client.GameEventID.ENTITYSYSTEM_CHANGE:
                {
                    if (null != param && param is Client.stPlayerChange)
                    {
                        Client.stPlayerChange change = (Client.stPlayerChange)param;
                        IEntity entity = RoleStateBarManager.GetEntityByUserID<IPlayer>(change.uid);
                        if (null == entity)
                        {
                            entity = RoleStateBarManager.GetEntityByUserID<INPC>(change.uid);
                        }

                        if (null != entity)
                        {
                            LateUpdateChangePos(entity.GetUID());
                        }
                    }
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_UNRIDE:
                {
                    if (null != param && param is stEntityUnRide)
                    {
                        stEntityUnRide unride = (stEntityUnRide)param;
                        LateUpdateChangePos(unride.uid);
                    }
                }
                break;

            case (int)Client.GameEventID.ENTITYSYSTEM_RIDE:
                {
                    if (null != param && param is stEntityRide)
                    {
                        stEntityRide ride = (stEntityRide)param;
                        LateUpdateChangePos(ride.uid);
                    }
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE:
                {
                    if (null != param && param is stEntityBeginMove)
                    {
                        stEntityBeginMove move = (stEntityBeginMove)param;
                        OnEntityMoving(move.uid);
                    }
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_ENTITYMOVE:
                {
                    if (null != param && param is stEntityMove)
                    {
                        stEntityMove move = (stEntityMove)param;
                        OnEntityMoving(move.uid);
                    }
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE:
                {
                    if (null != param && param is stEntityStopMove)
                    {
                        stEntityStopMove move = (stEntityStopMove)param;
                        OnEntityMoving(move.uid);
                    }
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY:
                {
                    //实体创建
                    Client.stCreateEntity ce = (Client.stCreateEntity)param;
                    OnCretateEntity(ce);
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_REMOVEENTITY:
                {
                    //实体删除
                    Client.stRemoveEntity removeEntiy = (Client.stRemoveEntity)param;
                    RemoveRoleBar(removeEntiy.uid);
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE:
                {
                    //实体属性变更
                    stPropUpdate prop = (stPropUpdate)param;
                    OnPropUpdate(ref prop);
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_HPUPDATE:
                {
                    //实体属性变更
                    stPropUpdate prop = (stPropUpdate)param;
                    OnPropUpdate(ref prop);
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_CHANGENAME:
                {
                    //实体名称改变
                    stEntityChangename e = (stEntityChangename)param;
                    UpdateHeadStaus(e.uid, HeadStatusType.Name);
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_SETHIDE:
                {
                    //实体名称改变
                    stEntityHide st = (stEntityHide)param;
                    //UpdateHeadStaus(e.uid, HeadStatusType.Name);
                    OnSetEntityHide(ref st);
                }
                break;
            case (int)Client.GameEventID.TITLE_WEAR:
                {
                    Client.stTitleWear data = (Client.stTitleWear)param;
                    IPlayer player = RoleStateBarManager.GetEntityByUserID<IPlayer>(data.uid);
                    if (null != player)
                    {
                        //佩戴称号
                        UpdateHeadStaus(player.GetUID(), HeadStatusType.Title);
                    }
                    
                }
                break;
            case (int)Client.GameEventID.SKILLGUIDE_PROGRESSSTART:
                {
                    //引导技能开始
                }
                break;
            case (int)Client.GameEventID.SKILLGUIDE_PROGRESSBREAK:
                {
                    //引导技能中断
                }
                break;
            case (int)Client.GameEventID.SKILLGUIDE_PROGRESSEND:
                {
                    //引导技能结束
                }
                break;
            case (int)Client.GameEventID.CLANQUIT:
            case (int)Client.GameEventID.CLANJOIN:
            case (int)Client.GameEventID.CLANREFRESHID:
            case (int)Client.GameEventID.CITYWARWINERCLANID:
            case (int)Client.GameEventID.CLANDeclareInfoAdd:
            case (int)Client.GameEventID.CLANDeclareInfoRemove:
                {
                    //氏族状态改变
                    OnRefreshAllClan();
                }
                break;
            case (int)Client.GameEventID.CITYWARTOTEMCLANNAMECHANGE:
                {
                    long uid = EntitySystem.EntityHelper.MakeUID(EntityType.EntityType_NPC, (uint)param);
                    UpdateHeadStaus(uid, HeadStatusType.Clan);
                }
                break;
            case (int)Client.GameEventID.SYSTEM_GAME_READY:
                {
                    OnRefresAllHp();
                }
                break;
            case (int)Client.GameEventID.CAMERA_MOVE_END:
                {
                    UpdateAllPos();
                }
                break;
            case (int)Client.GameEventID.RFRESHENTITYHEADSTATUS:
                {
                    HeadStatusType status = (HeadStatusType)param;
                    RefreshAllHeadStatus(status);
                }
                break;
                //npc头顶任务状态
            case (int)Client.GameEventID.TASK_ACCEPT:
                {
                    uint taskId = (uint)param;
                    OnUpdateNpcTaskStatus(taskId, Client.GameEventID.TASK_ACCEPT);
                }
                break;
            case (int)Client.GameEventID.TASK_DELETE:
                {
                    uint taskId = (uint)param;
                    OnUpdateNpcTaskStatus(taskId, Client.GameEventID.TASK_DELETE);
                }
                break;
            case (int)Client.GameEventID.TASK_DONE:
                {
                    Client.stTaskDone td = (Client.stTaskDone)param;
                    OnUpdateNpcTaskStatus(td.taskid,Client.GameEventID.TASK_DONE);
                }
                break;
            case (int)Client.GameEventID.TASK_CANSUBMIT:
                {
                    Client.stTaskCanSubmit tcs = (Client.stTaskCanSubmit)param;
                    OnUpdateNpcTaskStatus(tcs.taskid,Client.GameEventID.TASK_CANSUBMIT);
                }
                break;
            case (int)Client.GameEventID.TASK_CANACCEPT:
                {
                    uint taskId = (uint)param;
                    OnUpdateNpcTaskStatus(taskId,Client.GameEventID.TASK_CANACCEPT);
                }
                break;
        }
    }
    #endregion
    #region EventCall
    IEntity GetEntity(long uid)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            return es.FindEntity(uid);
        }
        return null;
    }
    private void OnSetEntityHide(ref stEntityHide st)
    {
        IEntity entity = GetEntity(st.uid);
        if (entity == null)
        {
            Engine.Utility.Log.Error("找不到对象------------" + st.uid);
            return;
        }
        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        if (cs == null)
        {
            return;
        }
        if (st.bHide)
        {
            IEntity currtarget = cs.GetActiveCtrl().GetCurTarget();
            if (currtarget != null)
            {
                if (currtarget.GetUID() == entity.GetUID())
                {
                    Client.stTargetChange targetChange = new Client.stTargetChange();
                    targetChange.target = null;
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_TARGETCHANGE, targetChange);
                }
            }

        }

        IPlayer mainPalyer = MainPlayerHelper.GetMainPlayer();
        if (entity.GetEntityType() == EntityType.EntityType_Player && mainPalyer != null)
        {
            if (mainPalyer.GetUID() != st.uid)
            {
                IControllerHelper ch = cs.GetControllerHelper();
                if (ch == null)
                {
                    return;
                }
                if (st.bHide)
                {
                    if (ch.IsSameTeam(entity))
                    {
                        SetBarVisible(entity.GetUID(), true, 0.3f);
                        entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 0.3f);
                    }
                    else
                    {
                       SetBarVisible(entity.GetUID(), false);
                        entity.SendMessage(EntityMessage.EntityCommand_SetVisible, false);
                    }
                }
                else
                {
                    SetBarVisible(entity.GetUID(), true);
                    entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 1.0f);
                    entity.SendMessage(EntityMessage.EntityCommand_SetVisible, true);
                }

            }
            else
            {
                if (st.bHide)
                {
                    SetBarVisible(entity.GetUID(), true, 0.3f);
                    entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 0.3f);
                }
                else
                {
                    SetBarVisible(entity.GetUID(), true);
                    entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 1.0f);
                    entity.SendMessage(EntityMessage.EntityCommand_SetVisible, true);
                }
            }
        }
        else
        {
            if (st.bHide)
            {
                SetBarVisible(entity.GetUID(), false);
                entity.SendMessage(EntityMessage.EntityCommand_SetVisible, false);
            }
            else
            {
                SetBarVisible(entity.GetUID(), true);
                entity.SendMessage(EntityMessage.EntityCommond_SetAlpha, 1.0f);
                entity.SendMessage(EntityMessage.EntityCommand_SetVisible, true);
            }
        }
    }
    private void OnEntityMoving(long uid)
    {
        if (uid == DataManager.Instance.UId)
        {
            var enumerator = m_dicActiveRoleStateBar.GetEnumerator();
            while(enumerator.MoveNext())
            {
                LateUpdateChangePos(enumerator.Current.Key);
            }
        }
        else
        {
            LateUpdateChangePos(uid);
        }
    }

    private void UpdateAllPos()
    {
        var enumerator = m_dicActiveRoleStateBar.GetEnumerator();
        while (enumerator.MoveNext())
        {
            LateUpdateChangePos(enumerator.Current.Key);
        }
    }

    private void LateUpdateChangePos(List<long> uids)
    {
        if (null != uids)
        {
            for(int i = 0,max = uids.Count;i < max;i++)
            {
                LateUpdateChangePos(uids[i]);
            }
        }
    }
    private void LateUpdateChangePos(long uid)
    {
        if (!m_lstChangePosIds.Contains(uid))
        {
            m_lstChangePosIds.Add(uid);
        }
    }
    /// <summary>
    /// 刷新npc任务状态表示
    /// </summary>
    /// <param name="taskId"></param>
    private void OnUpdateNpcTaskStatus(uint taskId,Client.GameEventID eventID)
    {
        long uid = 0;
        string icon= "";
        bool enable = false;
        switch (eventID)
        {
            case GameEventID.TASK_ACCEPT:
            case GameEventID.TASK_CANACCEPT:
            case GameEventID.TASK_DONING:
            case GameEventID.TASK_CANSUBMIT:
                {
                    enable = RoleStateBarManager.TryGetQuestStatusIcon(taskId, out uid, out icon);
                    if (eventID == GameEventID.TASK_ACCEPT)
                    {
                        table.QuestDataBase questDB = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(taskId);
                        if (questDB != null && questDB.dwBeginNpc != questDB.dwEndNpc)
                        {
                            //交接任务不在同一个npc移除任务标示
                            Client.INPC npc = RoleStateBarManager.GetNPCByBaseID(questDB.dwBeginNpc);
                            if (null != npc)
                            {
                                UpdateNpcTaskStatus(npc.GetUID(), false);
                            }
                        }
                    }
                }
                break;
            case GameEventID.TASK_DELETE:
            case GameEventID.TASK_DONE:
                {
                    table.QuestDataBase questDB = GameTableManager.Instance.GetTableItem<table.QuestDataBase>(taskId);
                    if (questDB == null)
                    {
                        return;
                    }
                    Client.INPC npc = RoleStateBarManager.GetNPCByBaseID(questDB.dwEndNpc);
                    if (null != npc)
                    {
                        uid = npc.GetUID();
                    }
                }
                break;
        }
        if (uid != 0)
        {
            UpdateNpcTaskStatus(uid, enable,icon);
        }
    }
    /// <summary>
    /// 更新npc任务状态
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="enable"></param>
    /// <param name="statusIcon"></param>
    private void UpdateNpcTaskStatus(long uid, bool enable, string statusIcon = "")
    {
        UIRoleStateBar bar = GetRoleBar(uid);
        if (null != bar)
        {
            bar.UpdateTaskUI(enable, statusIcon);
        }
    }

    /// <summary>
    /// 实体创建
    /// </summary>
    /// <param name="ce"></param>
    private void OnCretateEntity(Client.stCreateEntity ce)
    {
        IEntity entity = RoleStateBarManager.GetEntity(ce.uid);
        if (entity == null)
        {
            Engine.Utility.Log.Error("找不到对象------------{0}", ce.uid);
            return;
        }

        if (entity.GetEntityType() == EntityType.EntityType_Pet)
        {
            return;
        }

        if (entity.GetEntityType() == EntityType.EntityType_NPC)
        {
            table.NpcDataBase npctable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)entity.GetProp((int)Client.EntityProp.BaseID));
            if (npctable != null)
            {
                if (npctable.dwType == (uint)GameCmd.enumNpcType.NPC_TYPE_TRAP)
                {
                    return;
                }
            }
        }
        AddRoleBar(entity);
    }

    /// <summary>
    /// 属性变更
    /// </summary>
    /// <param name="prop"></param>
    private void OnPropUpdate(ref stPropUpdate prop)
    {
        Client.IEntity entity = ClientGlobal.Instance().GetEntitySystem().FindEntity(prop.uid);
        if (prop.nPropIndex == (int)CreatureProp.Hp || prop.nPropIndex == (int)CreatureProp.MaxHp)
        {
            UpdateHeadStaus(prop.uid, HeadStatusType.Hp);
        }
        else if (prop.nPropIndex == (int)PlayerProp.GoodNess
            || prop.nPropIndex == (int)CreatureProp.Camp)
        {
            UpdateHeadStaus(prop.uid, HeadStatusType.Name);
            if (prop.nPropIndex == (int)Client.CreatureProp.Camp)
            {
                UpdateHeadStaus(prop.uid, HeadStatusType.CampMask);
            }
        }
    }

    /// <summary>
    /// 刷新氏族顶部名称
    /// </summary>
    private void OnRefreshAllClan()
    {
        RefreshAllHeadStatus(HeadStatusType.Clan);
    }

    /// <summary>
    /// 刷新血条
    /// </summary>
    private void OnRefresAllHp()
    {
        RefreshAllHeadStatus(HeadStatusType.Hp);
    }

    /// <summary>
    /// 刷新头顶所有状态
    /// </summary>
    /// <param name="status"></param>
    private void RefreshAllHeadStatus(HeadStatusType status)
    {
        IEntity entity = null;
        long uid = 0;
        var enumerator = m_dicActiveRoleStateBar.GetEnumerator();
        while(enumerator.MoveNext())
        {
            if (null == enumerator.Current.Value)
            {
                continue;
            }
            entity = RoleStateBarManager.GetEntity(enumerator.Current.Key);
            if (null == entity)
            {
                continue;
            }
            switch (status)
            {
                case HeadStatusType.Hp:
                    break;
                case HeadStatusType.Name:
                    break;
                case HeadStatusType.Clan:
                    {
                        if (entity.GetEntityType() != EntityType.EntityType_Player
                            && entity.GetEntityType() != EntityType.EntityType_NPC)
                        {
                            continue;
                        }
                    }
                    break;
                case HeadStatusType.Title:
                    {
                        if (entity.GetEntityType() != EntityType.EntityType_Player)
                        {
                            continue;
                        }
                    }
                    break;
                case HeadStatusType.Collect:
                    break;
                case HeadStatusType.HeadMaskIcon:
                case HeadStatusType.TaskStatus:
                    {
                        if (entity.GetEntityType() != EntityType.EntityType_NPC)
                        {
                            continue;
                        }
                    }
                    break;
            }
            enumerator.Current.Value.UpdateHeadStatus(status);
        }
    }
    #endregion
}