
//*************************************************************************
//	创建日期:	2017/7/12 星期三 11:35:39
//	文件名称:	SliderDataManager
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.Utility;
using Client;
using UnityEngine;
public enum SliderDataEnum
{
    Begin,
    ShowProcess,
    End,
}


class SliderDataManager : BaseModuleData, IManager, ITimer
{
    bool bReadingSlider = false;//是否正在读条

    public bool IsReadingSlider
    {
        get
        {
            return bReadingSlider;
        }
        set
        {
            bReadingSlider = value;
        }
    }
    bool bbreak = false;
    public bool IsBreak
    {
        get
        {
            return bbreak;
        }
        set
        {
            bbreak = value;
        }
    }
    public float SliderProcess
    {
        get;
        set;
    }
    bool bOver = false;
    float m_duration = 0;
    uint m_uSliderTimerID = 1000;
    uint m_uInterval = 30;//100ms 刷新一次
    float m_uCurrentProcess = 0;//当前进度时间

    float m_startTime = 0;
    GameCmd.UninterruptActionType m_UninterruptActionType = GameCmd.UninterruptActionType.UninterruptActionType_None;


    /// <summary>
    /// 终端进度条 或  完成进度条代理
    /// </summary>
    Action m_progessBreakDel;
    Action m_progessEndDel;

    public void Initialize()
    {
        EventEngine.Instance().AddEventListener((int)GameEventID.UISHOWSLIDEREVENT, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.UIHIDESLIDEREVENT, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, OnEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.NETWORK_CONNECTE_CLOSE, OnEvent);

        EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, SkillEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, SkillEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, SkillEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, SkillEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, SkillEvent);

        EventEngine.Instance().AddVoteListener((int)GameVoteEventID.SKILL_CANUSE, VoteEvent);
    }

    public void Reset(bool depthClearData = false)
    {
        Initialize();
    }

    public void Process(float deltaTime)
    {

    }

    public void ClearData()
    {

        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, MoveCancel);
        EventEngine.Instance().RemoveEventListener((int)GameEventID.UISHOWSLIDEREVENT, OnEvent);
        EventEngine.Instance().RemoveEventListener((int)GameEventID.UIHIDESLIDEREVENT, OnEvent);

        EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSSTART, SkillEvent);
        EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSEND, SkillEvent);
        EventEngine.Instance().RemoveEventListener((int)GameEventID.SKILLGUIDE_PROGRESSBREAK, SkillEvent);
        EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, SkillEvent);
        EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, SkillEvent);


        EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.SKILL_CANUSE, VoteEvent);
    }
    bool VoteEvent(int eventID, object param)
    {
        if (eventID == (int)GameVoteEventID.SKILL_CANUSE)
        {
            if(m_UninterruptActionType == GameCmd.UninterruptActionType.UninterruptActionType_DEMON)
            {
                //骑马进度条显示中 不能使用技能
                if(DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.SliderPanel))
                {
                    Log.Error("正在骑马，不能使用技能");
                    return false;
                }
            }
        }
        return true;
    }
    void SkillEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.SKILLGUIDE_PROGRESSSTART)
        {
            Client.stUninterruptMagic evenparam = (Client.stUninterruptMagic)param;
            if (ClientGlobal.Instance().IsMainPlayer(evenparam.uid))
            {
                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.SliderPanel) == false)
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.SliderPanel, panelShowAction: (panel) =>
                    {
                        DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = SliderDataEnum.Begin.ToString() });
                    });
                }
                float dur = (float)(evenparam.time * 1f / 1000);
                if (dur > 0)
                {
                    DataManager.Manager<SliderDataManager>().StartSliderByEvent(dur, evenparam.type);
                }
                else
                {
                    Log.Error("进度条时间小于 0");
                }
            }

            if (evenparam.type == GameCmd.UninterruptActionType.UninterruptActionType_CJ ||
                evenparam.type == GameCmd.UninterruptActionType.UninterruptActionType_CampCJ ||
                evenparam.type == GameCmd.UninterruptActionType.UninterruptActionType_SYDJ)
            {

                Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();

                if (es == null)
                {
                    return;
                }

                Client.IEntity player = es.FindEntity(evenparam.uid);

                if (player == null)
                {
                    return;
                }
                bool moving = (bool)player.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
                if (moving)
                {
                    player.SendMessage(EntityMessage.EntityCommand_StopMove);
                }

                Client.INPC npc = es.FindNPC(evenparam.npcId);
                if (npc != null)
                {
                    player.SendMessage(EntityMessage.EntityCommand_LookTarget, npc.GetPos());
                }
                PlayAni(player, EntityAction.Collect);
            }
            else if (evenparam.type == GameCmd.UninterruptActionType.UninterruptActionType_CangBaoTuCJ)
            {
                //采集
                Log.Info("开始挖宝...");
                Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();

                if (es == null)
                {
                    return;
                }
                Client.IEntity player = es.FindEntity(evenparam.uid);

                if (player == null)
                {
                    return;
                }
                bool moving = (bool)player.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
                if (moving)
                {
                    player.SendMessage(EntityMessage.EntityCommand_StopMove);
                }

                Client.INPC npc = es.FindNPC(evenparam.npcId);
                if (npc != null)
                {
                    player.SendMessage(EntityMessage.EntityCommand_LookTarget, npc.GetPos());
                }
                //                PlayAni(player, EntityAction.Collect);
                //播放挖宝动作
                PlayAni(player, EntityAction.Mining);

                //更换武器模型
                DataManager.Manager<SuitDataManager>().OnMiningTreasureMapToChangeWeapon(player);
            }

        }
        else if (eventID == (int)GameEventID.SKILLGUIDE_PROGRESSBREAK)
        {
            if (param != null)
            {
                stGuildBreak guildBreak = (stGuildBreak)param;
                if (ClientGlobal.Instance().IsMainPlayer(guildBreak.uid))
                {
                    DataManager.Manager<SliderDataManager>().HideSlider(guildBreak.action);
                    DataManager.Manager<SliderDataManager>().IsBreak = true;
                    DataManager.Manager<SliderDataManager>().IsReadingSlider = false;
                    //DataManager.Manager<TaskDataManager>().IsShowSlider = false;
                }

                if (guildBreak.action == GameCmd.UninterruptActionType.UninterruptActionType_CJ ||
                    guildBreak.action == GameCmd.UninterruptActionType.UninterruptActionType_CampCJ)
                {
                    //打断采集
                    Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
                    if (es == null)
                    {
                        return;
                    }
                    Client.IEntity entity = es.FindPlayer(guildBreak.uid);
                    if (entity != null)
                    {
                        bool moving = (bool)entity.SendMessage(EntityMessage.EntityCommand_IsMove, null);
                        if (!moving)
                        {
                            PlayAni(guildBreak.uid, EntityAction.Stand);
                        }
                    }
                }

            }
        }
        else if (eventID == (int)GameEventID.SKILLGUIDE_PROGRESSEND)
        {
            DataManager.Manager<SliderDataManager>().IsReadingSlider = false;
            //DataManager.Manager<TaskDataManager>().IsShowSlider = false;
            stGuildEnd guildEnd = (stGuildEnd)param;
            if (ClientGlobal.Instance().IsMainPlayer(guildEnd.uid))
            {
                //坐骑读条完毕后上马
                if (guildEnd.action == GameCmd.UninterruptActionType.UninterruptActionType_DEMON)
                {
                    DataManager.Instance.Sender.RideUsingRide();
                }
                else
                {
                    if (guildEnd.action != GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ)
                    {
                        PlayStand();
                    }
                }
            }
            else
            {
                //播站立动作
                PlayAni(guildEnd.uid, EntityAction.Stand);
            }
        }
        else if (eventID == (int)GameEventID.ENTITYSYSTEM_ENTITYDEAD)
        {
            stEntityDead ed = (stEntityDead)param;
            if (ClientGlobal.Instance().IsMainPlayer(ed.uid))
            {
                // HideSprite();
                DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = SliderDataEnum.End.ToString(), oldValue = m_UninterruptActionType });
                DataManager.Manager<SliderDataManager>().IsBreak = true;
                DataManager.Manager<SliderDataManager>().IsReadingSlider = false;
            }
        }
    }
    #region playani

    void PlayStand()
    {
        PlayAni anim_param = new PlayAni();
        anim_param.strAcionName = EntityAction.Stand;
        anim_param.fSpeed = 1;
        anim_param.nStartFrame = 0;
        anim_param.nLoop = -1;
        anim_param.fBlendTime = 0.2f;
        ClientGlobal.Instance().MainPlayer.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
    }

    void PlayAni(Client.IEntity player, string entityAction)
    {
        if (player == null)
        {
            return;
        }

        //处理
        PlayAni anim_param = new PlayAni();
        anim_param.strAcionName = entityAction;
        anim_param.fSpeed = 1;
        anim_param.nStartFrame = 0;
        anim_param.nLoop = -1;
        anim_param.fBlendTime = 0.2f;
        player.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
    }

    void PlayAni(uint uid, string entityAction)
    {
        Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();

        if (es == null)
        {
            return;
        }

        Client.IPlayer player = es.FindPlayer(uid);

        if (player == null)
        {
            return;
        }

        //处理
        PlayAni anim_param = new PlayAni();
        anim_param.strAcionName = entityAction;
        anim_param.fSpeed = 1;
        anim_param.nStartFrame = 0;
        anim_param.nLoop = -1;
        anim_param.fBlendTime = 0.2f;
        player.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
    }
    #endregion
    void OnEvent(int nEventID, object param)
    {
        if (nEventID == (int)GameEventID.UISHOWSLIDEREVENT)
        {
            stSliderBeginEvent st = (stSliderBeginEvent)param;
            StartSliderByEvent(st.dur, (GameCmd.UninterruptActionType)st.sliderType);
        }
        else if (nEventID == (int)GameEventID.UIHIDESLIDEREVENT)
        {
            HideSlider();
        }
        else if (nEventID == (int)GameEventID.ENTITYSYSTEM_ENTITYDEAD) // 实体死亡
        {
            stEntityDead ed = (stEntityDead)param;
            if (Client.ClientGlobal.Instance().IsMainPlayer(ed.uid))
            {
                HideSlider();
            }
        }
        else if (nEventID == (int)GameEventID.NETWORK_CONNECTE_CLOSE)
        {
            HideSlider();
        }
    }
    void MoveCancel(int nEventId, object param)
    {
        if ((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE == nEventId)
        {
            stEntityBeginMove move = (stEntityBeginMove)param;
            if (move.uid != Client.ClientGlobal.Instance().MainPlayer.GetUID())
            {
                return;
            }
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, MoveCancel);
            DataManager.Manager<SliderDataManager>().IsBreak = true;
            DataManager.Manager<SliderDataManager>().IsReadingSlider = false;
            HideSlider();

            if (IsClickNpcAction(m_UninterruptActionType))
            {
                GameCmd.stNotifyUninterruptEventMagicUserCmd_CS cmd = new GameCmd.stNotifyUninterruptEventMagicUserCmd_CS();
                cmd.etype = GameCmd.stNotifyUninterruptEventMagicUserCmd_CS.EventType.EventType_Break;
                cmd.actiontype = (uint)m_UninterruptActionType;
                cmd.desid = ClientGlobal.Instance().MainPlayer.GetID();
                NetService.Instance.Send(cmd);
            }
        }
    }
    bool IsClickNpcAction(GameCmd.UninterruptActionType action)
    {
        return action == GameCmd.UninterruptActionType.UninterruptActionType_CampCJ
            || action == GameCmd.UninterruptActionType.UninterruptActionType_SYDJ ||
             action == GameCmd.UninterruptActionType.UninterruptActionType_CJ;
    }
    public void StartSliderByEvent(float duration, GameCmd.UninterruptActionType type)
    {
        if (DataManager.Manager<SliderDataManager>().IsReadingSlider && type == m_UninterruptActionType)
        {
            if (type == GameCmd.UninterruptActionType.UninterruptActionType_GOHOME)
            {
                m_duration = duration;
                m_startTime = UnityEngine.Time.realtimeSinceStartup;
                return;
            }
            Log.LogGroup("ZDY", "StartSliderByEvent  在读条中" + type.ToString());
            return;
        }

        if (type == GameCmd.UninterruptActionType.UninterruptActionType_DEMON)
        {
            bool canRide = Engine.Utility.EventEngine.Instance().DispatchVote((int)Client.GameVoteEventID.RIDE_CANRIDE, null);
            if (!canRide)
            {
                m_UninterruptActionType = GameCmd.UninterruptActionType.UninterruptActionType_None;
                return;
            }
        }
        else if (type == GameCmd.UninterruptActionType.UninterruptActionType_GOHOME)
        {


        }
        m_UninterruptActionType = type;




        //DataManager.Manager<SliderDataManager>().IsReadingSlider = true;

        IsReadingSlider = true;

        if (type != GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, MoveCancel);
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.SliderPanel, panelShowAction: (panel) =>
        {
            DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = SliderDataEnum.Begin.ToString() });
        });


        //Log.Info("slider duration is " + duration.ToString());
        //DataManager.Manager<SliderDataManager>().IsBreak = false;
        bbreak = false;

        bOver = false;
        m_duration = duration;
        //DataManager.Manager<TaskDataManager>().IsShowSlider = true;
        m_startTime = UnityEngine.Time.realtimeSinceStartup;
        m_uCurrentProcess = 0;
        TimerAxis.Instance().SetTimer(m_uSliderTimerID, m_uInterval, this);
    }
    public void HideSlider(GameCmd.UninterruptActionType type = GameCmd.UninterruptActionType.UninterruptActionType_None)
    {
        if (type == GameCmd.UninterruptActionType.UninterruptActionType_None)
        {
            type = m_UninterruptActionType;
        }
        if (type == m_UninterruptActionType)
        {
            DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = SliderDataEnum.End.ToString(), oldValue = type });
            TimerAxis.Instance().KillTimer(m_uSliderTimerID, this);
            CleanSliderDelagateData();
            m_UninterruptActionType = GameCmd.UninterruptActionType.UninterruptActionType_None;
        }

    }
    void EndSlider()
    {
        if (!bOver)
        {
            bOver = true;
            if (m_UninterruptActionType == GameCmd.UninterruptActionType.UninterruptActionType_DEMON || m_UninterruptActionType == GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ
              || m_UninterruptActionType == GameCmd.UninterruptActionType.UninterruptActionType_CangBaoTuCJ)
            {
                EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSEND,
                    new stGuildEnd() { action = m_UninterruptActionType, uid = MainPlayerHelper.GetPlayerID() });
            }
            else if (m_UninterruptActionType != GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ)
            {
                GameCmd.stNotifyUninterruptEventMagicUserCmd_CS cmd = new GameCmd.stNotifyUninterruptEventMagicUserCmd_CS();
                cmd.etype = GameCmd.stNotifyUninterruptEventMagicUserCmd_CS.EventType.EventType_Over;
                cmd.desid = MainPlayerHelper.GetPlayerID();
                cmd.actiontype = (uint)m_UninterruptActionType;
                NetService.Instance.Send(cmd);
            }
            //DataManager.Manager<TaskDataManager>().IsShowSlider = false;
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, MoveCancel);
            DispatchValueUpdateEvent(new ValueUpdateEventArgs() { key = SliderDataEnum.End.ToString(), oldValue = m_UninterruptActionType });
            m_UninterruptActionType = GameCmd.UninterruptActionType.UninterruptActionType_None;
            IsReadingSlider = false;

            //倒计时截至  代理中处理
            if (m_progessEndDel != null)
            {
                m_progessEndDel.Invoke();
                m_progessEndDel = null;
                //DataManager.Manager<TaskDataManager>().IsShowSlider = false;
            }
        }
    }


    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == m_uSliderTimerID)
        {
            m_uCurrentProcess = Time.realtimeSinceStartup - m_startTime;
            if (m_uCurrentProcess >= (m_duration))
            {
                TimerAxis.Instance().KillTimer(m_uSliderTimerID, this);
                EndSlider();
            }
            else
            {
                if (m_duration != 0)
                {

                    SliderProcess = (float)(m_uCurrentProcess * 1.0f / m_duration);
                }

            }
        }
    }

    /// <summary>
    /// 进度条代理
    /// </summary>
    /// <param name="progessEndDel"></param>
    /// <param name="progessBreakDel"></param>
    /// <param name="duration"></param>
    public void SetSliderDelagate(Action progessEndDel, Action progessBreakDel, float duration)
    {
        StartSliderByEvent(duration, GameCmd.UninterruptActionType.UninterruptActionType_None);

        if (progessEndDel != null)
        {
            this.m_progessEndDel = progessEndDel;
        }

        if (progessBreakDel != null)
        {
            this.m_progessBreakDel = progessBreakDel;
        }
    }

    /// <summary>
    /// 清进度条数据
    /// </summary>
    public void CleanSliderDelagateData()
    {
        if (this.m_progessEndDel != null)
        {
            this.m_progessEndDel = null;
        }

        if (this.m_progessBreakDel != null)
        {
            this.m_progessBreakDel = null;
        }
    }

}
