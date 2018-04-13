using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using Engine.Utility;
using table;
using EntitySystem;
using UnityEngine;
namespace SkillSystem
{
    class SkillAttackState : SkillStateBase
    {
        ISkillPart skillPart = null;
        float m_fTotalTime = 0;
        SkillDatabase m_skillDatabase = null;
        SkillDoubleHitDataBase m_skillDoubleDb;
        //readonly float fastMoveSpeed = 5;

        uint m_uDbJiangzhiTime = 0;
   
        public SkillAttackState(StateMachine<ISkillPart> machine, ISkillPart caster)
            : base(machine, caster)
        {
            m_nStateID = (int)SkillState.Attack;


        }

        // 进入状态
        public override void Enter(object param)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, OnEvent);
         
            m_skillDoubleDb = null;
            m_fTotalTime = 0;
            skillPart = m_SkillPart;
            // m_skillDatabase = skillPart.GetCurSkillDataBase();

            if (skillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
            {
                PlayerSkillPart playerSkill = skillPart as PlayerSkillPart;
                //m_nDoubleHitSkillId = (uint)playerSkill.NextSkillID;
                if (playerSkill.IsMainPlayer())
                {
                    Log.LogGroup("ZDY", "mainpalyer enter skillattackstate================");
                }
                m_skillDatabase = GameTableManager.Instance.GetTableItem<SkillDatabase>(playerSkill.CurSkillID, 1);
      

                if (m_skillDatabase == null)
                {
                    m_Statemachine.ChangeState((int)SkillState.None, null);
                    return;
                }
                m_uDbJiangzhiTime = m_skillDatabase.wdStiffTime;
                playerSkill.SkillStiffTime = 0;
                m_skillDoubleDb = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>(playerSkill.CurSkillID);
                if (IsMainPlayer())
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_USESKILL, playerSkill.CurSkillID);
               
                }

                if (m_skillDatabase.dwMoveType == (uint)SkillMoveType.FastMove)
                {
                    //处理位移技能
                    Move move = new Move();
                    Vector3 targePos = playerSkill.Master.GetPos();
                    if (playerSkill.GetSkillTarget() != null)
                    {
                        targePos = playerSkill.GetSkillTarget().GetPos();
                        Vector3 dir = targePos - playerSkill.Master.GetPos();
                        targePos = targePos - dir.normalized * 1f;
                        move.m_target = targePos;
                        // Vector3 lookat = playerSkill.GetSkillTarget().GetNode().GetTransForm().forward;

                    }
                    else
                    {
                        m_Statemachine.ChangeState((int)SkillState.None, playerSkill);
                        Log.LogGroup("ZDY", "skilltarget is null");
                        return;
                    }

                    move.m_ignoreStand = true;
                    if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                    {
                        Log.LogGroup("ZDY", "冲锋 目标位置 " + targePos);
                    }
                    AnimationState ani = playerSkill.AttackAnimState;
                    if(ani == null)
                    {
                        m_Statemachine.ChangeState((int)SkillState.None, playerSkill);
                        Log.LogGroup("ZDY", "ani is null");
                        return;
                    }
                    move.strRunAct = ani.name;
                    int curSpeed = playerSkill.Master.GetProp((int)WorldObjProp.MoveSpeed);
                    uint flySpeed = m_skillDatabase.flyspeed;
                    float speedFact = 1;
                    if (flySpeed != 0)
                    {
                        speedFact = flySpeed * 1.0f / curSpeed;
                    }
                    Log.LogGroup("ZDY", "冲锋倍数 " + speedFact);
                    playerSkill.Master.SendMessage(EntityMessage.EntityCommond_IgnoreMoveAction, true);
                    playerSkill.Master.SendMessage(EntityMessage.EntityCommand_ChangeMoveSpeedFact, (object)speedFact);

                    float dis = Vector3.Distance(targePos, playerSkill.Master.GetPos());
                    if (dis > 1)
                    {
                        Log.LogGroup("ZDY", " send moveto ");
                        playerSkill.Master.SendMessage(EntityMessage.EntityCommand_MoveTo, (object)move);
                    }

                    playerSkill.gotoPos = targePos;
                }
                else
                {
                    playerSkill.Master.SendMessage(EntityMessage.EntityCommand_ChangeMoveSpeedFact, (object)1f);
                }
                if (m_skillDatabase.useSkillType == (int)UseSkillType.GuideNoSlider)
                {
                    if (m_skillDatabase.dwMoveType == (int)SkillMoveType.SkillOverMove)
                    {
                        playerSkill.Master.SendMessage(EntityMessage.EntityCommand_SetVisible, false);
                    }
                    if (m_skillDatabase.wdStiffTime == 0)
                    {//狂扫八方的技能
                        playerSkill.Master.SendMessage(EntityMessage.EntityCommond_IgnoreMoveAction, true);
                    }

                }
                else if (m_skillDatabase.useSkillType == (int)UseSkillType.GuideSlider)
                {
                    if (playerSkill.IsMainPlayer())
                    {
                        Client.stUninterruptMagic evenparam = new Client.stUninterruptMagic();
                        evenparam.time = m_skillDatabase.dwGuideTime;
                        evenparam.type = GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ;
                        evenparam.uid = SkillSystem.GetClientGlobal().MainPlayer.GetUID();
                        EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSSTART, evenparam);
                    }
                    playerSkill.Master.SendMessage(EntityMessage.EntityCommond_IgnoreMoveAction, true);
                }
                //
                if (m_skillDatabase.dwUseMethod == (int)UseMethod.ContinueLock)
                {
                    playerSkill.Master.SendMessage(EntityMessage.EntityCommand_SetVisible, false);
                }


                INPC npc = playerSkill.Master as INPC;
                if (npc != null)
                {
                    int masterID = npc.GetProp((int)NPCProp.Masterid);
                    if (masterID == PlayerSkillPart.m_ClientGlobal.MainPlayer.GetID())
                    {
                   /*     Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLCD_BEGIN, playerSkill.CurSkillID);*/
                    }
                }
            }
        }

        // 退出状态
        public override void Leave()
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE, OnEvent);
            if (IsMainPlayer())
            {
                Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SKLL_LONGPRESS, OnEvent);
            }
            PlayerSkillPart playerSkill = skillPart as PlayerSkillPart;

            if (skillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
            {
                SkillDatabase database = playerSkill.GetCurSkillDataBase();
                if (database == null)
                {
                    return;
                }
                if (database.dwUseMethod == (int)UseMethod.ContinueLock)
                {
                    playerSkill.Master.SendMessage(EntityMessage.EntityCommand_SetVisible, true);
                }
            }
           // playerSkill.SkillStiffTime = 0;

            //if (SkillSystem.GetClientGlobal().IsMainPlayer(playerSkill.GetMaster()))
            //{
            //    Log.Trace("Leave ..." + this.GetType().Name);
            //}
            //Log.Trace("skill attack state leave");
        }

        public override void Update(float dt)
        {
            IEntity casetr = skillPart.GetMaster();
            if (m_caster == null)
            {
                m_Statemachine.ChangeState((int)SkillState.Over, null);
                return;
            }

            m_caster.Update(dt);
            m_fTotalTime += dt;
            if (skillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
            {
                PlayerSkillPart playerSkill = skillPart as PlayerSkillPart;
              //  if (m_caster.IsCasting == false)
                {
                    if (m_skillDatabase.useSkillType == (int)UseSkillType.GuideSlider || m_skillDatabase.useSkillType == (int)UseSkillType.GuideNoSlider)
                    {
                        if (m_skillDatabase.dwGuideTime > 0)
                        {
                            if (m_fTotalTime * 1000 > m_skillDatabase.dwGuideTime)
                            {
                                //接收招动作
                                m_Statemachine.ChangeState((int)SkillState.Over, playerSkill);
                            }
                            return;
                        }
                        else
                        {
                            Log.LogGroup("ZDY", "引导类型技能 {0} 的引导时间为0", m_skillDatabase.wdID);
                        }
                    }
                    float tmeptime = Time.realtimeSinceStartup;
                    float delta = tmeptime - playerSkill.SkillStartPlayTime;
                    uint skillPlaytime = (uint)(delta * 1000);

                    uint skillRunJiangzhiTime = (uint)(playerSkill.SkillStiffTime * 1000);
                    IEntity target = skillPart.GetSkillTarget();
                    if (target == null)
                    {
                        if (skillRunJiangzhiTime > m_uDbJiangzhiTime) // 播放时间大于僵直时间
                        {
                            m_Statemachine.ChangeState((int)SkillState.Over, playerSkill);
                        }

                        return;
                    }
                    if (target.GetCurState() == CreatureState.Dead)
                    {
                        Log.LogGroup("ZDY", "target dead 去除连击时无法切换状态的bug ");
                        m_Statemachine.ChangeState((int)SkillState.Over, null);
                        return;
                    }
                    playerSkill.OnDoLongPressNextSkillID();
                    if ((playerSkill.NextSkillID != 0))
                    {
                        //CurSkillID  

                     //   m_skillDoubleDb = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>(playerSkill.CurSkillID);
                        SkillDoubleHitDataBase db = m_skillDoubleDb;
                        if (db != null)
                        {
                        //    if (playerSkill.NextSkillID == 101)
                        //    {
                        //        Log.Error("连击 nextskillid " + playerSkill.NextSkillID.ToString() + " skillPlay time is " + skillPlaytime +
                        //"  db.beginChangeTime " + db.beginChangeTime + " db.doublehitend " + db.doublehitend);
                        //    }

                            if (skillPlaytime >= db.beginChangeTime && skillPlaytime <= db.doublehitend)
                            {
                                //大于僵直时间 并且小于连击生效时间可以变招
                                SkillEffect skillEffect = m_caster.EffectNode;
                                if (skillEffect != null)
                                {
                                  //  skillEffect.Stop();
                                }
                                //    Log.LogGroup("ZDY", "连击 nextskillid " + playerSkill.NextSkillID.ToString());
                                playerSkill.SkillStiffTime = 0;
                                //如果已经触发连击，接着播放连击技能
                                DoCastSkill((uint)playerSkill.NextSkillID);
                                playerSkill.SkillStartPlayTime = Time.realtimeSinceStartup;
                                playerSkill.SyncSkill((uint)playerSkill.CurSkillID, target, Vector3.zero);
                                SkillDatabase curDataBase = m_skillDatabase;// GameTableManager.Instance.GetTableItem<SkillDatabase>(playerSkill.CurSkillID, 1);
                                if (curDataBase != null)
                                {
                                    m_uDbJiangzhiTime = curDataBase.wdStiffTime;
                                }
                                else
                                {
                                    if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                                    {
                                        Log.LogGroup("ZDY", "curdatabase is null skill id is " + playerSkill.CurSkillID);
                                    }
                                }
                                //普攻没有cd 不需要派发
                                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_USESKILL, playerSkill.CurSkillID);
                            }
                            else
                            {

                                if (skillPlaytime > db.doublehitend) // 播放时间大于连击生效时间
                                {
                                    if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                                    {
                                        Log.LogGroup("ZDY", "播放时间大于连击生效时间 切换到over skillPlaytime is " + skillPlaytime + " 连击生效时间 " + db.doublehitend);
                                    }
                                    //接收招动作
                                    m_Statemachine.ChangeState((int)SkillState.Over, playerSkill);
                                }

                            }
                        }
                    }
                    else
                    {
                        if (skillRunJiangzhiTime > m_uDbJiangzhiTime) // 大于僵直时间
                        {
                            if (Engine.Utility.Log.MaxLogLevel >= Engine.Utility.LogLevel.LogLevel_Group)
                            {
                                Log.LogGroup("ZDY", "达到僵直时间切换到over runjiangzhitime is " + skillRunJiangzhiTime);
                            }
                            //接收招动作
                            m_Statemachine.ChangeState((int)SkillState.Over, playerSkill);
                        }
                        else
                        {//连击不要在这里触发 
                         //   m_skillDoubleDb = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>(playerSkill.CurSkillID);
                           /* if (m_skillDoubleDb != null && IsMainPlayer())
                            {
                                IControllerSystem cs = playerSkill.GetCtrollerSys();
                                if (cs == null)
                                {
                                    Log.Error("ExecuteCmd: ControllerSystem is null");
                                    return;
                                }

                                if (cs.GetCombatRobot().Status != CombatRobotStatus.STOP)
                                {
                                    //   Log.LogGroup("ZDY", "======================= skillPlaytime is " + skillPlaytime + " start " + m_skillDoubleDb.doublehitBegin + " end " + m_skillDoubleDb.doublehitend);
                                    if (skillPlaytime >= m_skillDoubleDb.doublehitBegin && skillPlaytime <= m_skillDoubleDb.doublehitend && m_bSendDoubleHitCmd == false)
                                    {
                                        m_bSendDoubleHitCmd = true;
                                        Engine.Utility.Log.LogGroup("ZCX", "连击{0} next ", playerSkill.CurSkillID, playerSkill.NextSkillID);
                                        SkillDoubleHitDataBase skilldoubleDb = m_skillDoubleDb;// GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>(playerSkill.CurSkillID);

                                        if (skilldoubleDb != null)
                                        {
                                            stSkillDoubleHit skillDhHit = new stSkillDoubleHit();
                                            skillDhHit.doubleHitEnd = skilldoubleDb.beginskillid == skilldoubleDb.nextskillid;
                                            skillDhHit.skillID = skilldoubleDb.nextskillid;
                                            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ROBOTCOMBAT_NEXTCMD, skillDhHit);
                                        }
                                    }
                                }

                            }*/
                        }
                    }

                }
                //else
                //{

                //}

            }
        }

        public override void OnEvent(int nEventID, object param)
        {
            IEntity casetr = skillPart.GetMaster();
            if (casetr == null)
            {
                return;
            }

            if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE)
            {
                Client.stEntityStopMove stopEntity = (Client.stEntityStopMove)param;
                if (m_skillDatabase != null && stopEntity.uid == skillPart.GetMaster().GetUID())
                {
                    if (m_skillDatabase.dwMoveType == (uint)SkillMoveType.FastMove) // 冲锋类技能
                    {
                        PlayerSkillPart playerSkill = skillPart as PlayerSkillPart;
                        if (playerSkill != null)
                        {
                            if (playerSkill.gotoPos != Vector3.zero)
                            {
                                playerSkill.Master.SendMessage(EntityMessage.EntityCommand_SetPos, (object)playerSkill.gotoPos);
                                Client.IEntity target = playerSkill.SkillTarget;
                                if (target != null)
                                {
                                    if (m_skillDatabase.targetType != (int)SkillTargetType.TargetForwardPoint)
                                    {
                                        playerSkill.Master.SendMessage(EntityMessage.EntityCommand_LookTarget, target.GetPos());
                                    }

                                }
                            }
                        }

                        m_Statemachine.ChangeState((int)SkillState.Over, null);
                        // 接冲锋动作后半段
                        //PlayAni anim_param = new PlayAni();
                        //string strAniName = m_caster.GetAniState().name;
                        //anim_param.strAcionName = strAniName + "_Over";
                        //anim_param.fSpeed = 1;
                        //anim_param.nStartFrame = 0;
                        //anim_param.nLoop = 1;
                        //anim_param.fBlendTime = 0.2f;
                        //anim_param.aniCallback = OnAnimationPlayEnd;
                        //anim_param.callbackParam = casetr.GetUID();
                        //// 播放攻击动作
                        //playerSkill.Player.SendMessage( EntityMessage.EntityCommand_PlayAni , anim_param );
                    }
                }
            }
            else if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE)
            {
                // 攻击状态中收到移动事件，则切换到OVer状态

            }
           
        }

        void DoCastSkill(uint skill_id)
        {
            if (skillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
            {
                PlayerSkillPart playerSkill = skillPart as PlayerSkillPart;
                SkillEffectProp skillEffectProp;
                if (playerSkill.SkillEffectDic.TryGetValue((int)skill_id, out skillEffectProp) == false)
                {
                    return;
                }
                IEntity target = skillPart.GetSkillTarget();
                // 朝向目标
                if (target != null)
                {
                    if (m_skillDatabase != null)
                    {
                        if (m_skillDatabase.targetType != (int)SkillTargetType.TargetForwardPoint)
                        {
                            skillPart.GetMaster().SendMessage(EntityMessage.EntityCommand_LookTarget, target.GetPos());
                        }
                    }
                }
                else
                {
                    Engine.Utility.Log.LogGroup("ZCX", "朝向   目标  为空");
                }
                playerSkill.NextSkillID = 0;
                playerSkill.CurSkillID = skill_id;
                m_skillDoubleDb = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>(playerSkill.CurSkillID);
                m_skillDatabase = GameTableManager.Instance.GetTableItem<SkillDatabase>(playerSkill.CurSkillID, 1);
             

                //  m_attackState = SkillState.Attack
                //切换连招时AttackAnimState = null 会导致动作结束判断错误
                //playerSkill.AttackAnimState = null;

                SkillEffect skillEffect = m_caster.CreateSkillEffect(skillEffectProp);
                m_caster.Cast(skillEffect, skill_id);
            }
        }

        void OnAnimationPlayEnd(ref string strEventName, ref string strAnimationName, float time, object param)
        {
            IEntity casetr = skillPart.GetMaster();
            if (casetr == null)
            {
                return;
            }

            if ((long)param == casetr.GetUID() && strEventName == "end") // 动画播放结束
            {
                m_Statemachine.ChangeState((int)SkillState.Over, null);
                casetr.SendMessage(EntityMessage.EntityCommand_ClearAniCallback, null);
            }
        }

    }
}
