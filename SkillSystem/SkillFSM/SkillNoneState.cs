using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using Engine.Utility;
using UnityEngine;
namespace SkillSystem
{
    class SkillNoneState : SkillStateBase
    {

        public SkillNoneState(StateMachine<ISkillPart> machine, ISkillPart caster)
            : base(machine, caster)
        {
            m_nStateID = (int)SkillState.None;
        }
        // 进入状态
        public override void Enter(object param)
        {
            if (m_SkillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
            {
                PlayerSkillPart playerSkill = m_SkillPart as PlayerSkillPart;

                if (SkillSystem.GetClientGlobal().IsMainPlayer(playerSkill.GetMaster()))
                {

                    // Log.LogGroup("ZDY", "mainpalyer enter skilloverstate");
                
                    if (playerSkill != null)
                    {
                        playerSkill.CurSkillID = 0;
                    }
                    //Log.LogGroup("ZCX", "Enter ..." + this.GetType().Name + playerSkill.CurSkillID);
                    stForbiddenJoystick info = new stForbiddenJoystick();
                    info.playerID = playerSkill.Master.GetUID();
                    info.bFobidden = false;
                    EventEngine.Instance().DispatchEvent((int)GameEventID.SKILL_FORBIDDENJOYSTICK, info);
                }
                IEntity casetr = playerSkill.GetMaster();
                if (casetr == null)
                {
                    return;
                }
                playerSkill.Master.SendMessage(EntityMessage.EntityCommond_IgnoreMoveAction, false);
                playerSkill.Master.SendMessage(EntityMessage.EntityCommand_ChangeMoveSpeedFact, (object)1f);
                CreatureState playerState = playerSkill.GetMaster().GetCurState();
                if (playerState != CreatureState.Dead)
                {


                    // 不能立即切换到Normal状态
                    PlayAni anim_param = new PlayAni();
                    bool isMove = (bool)m_SkillPart.GetMaster().SendMessage(EntityMessage.EntityCommand_IsMove, null);
                    if (isMove)
                    {
                        anim_param.strAcionName = EntityAction.Run;
                    }
                    else
                    {
                        if (casetr.GetEntityType() == EntityType.EntityType_Player)
                        {
                            anim_param.strAcionName = EntityAction.Stand_Combat;
                        }
                        else
                        {
                            anim_param.strAcionName = EntityAction.Stand;
                        }
                    }

                    anim_param.fSpeed = 1;
                    anim_param.nStartFrame = 0;
                    anim_param.nLoop = -1;
                    anim_param.fBlendTime = 0.2f;
                    m_SkillPart.GetMaster().SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);


                    //如果在挂机 不能立即切换到Normal状态 主角才派发事件
                    if (IsMainPlayer())
                    {
                        IControllerSystem cs = playerSkill.GetCtrollerSys();
                        if (cs == null)
                        {
                            Log.Error("ExecuteCmd: ControllerSystem is null");
                            return;
                        }

                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLNONESTATE_ENTER,
                        new stSkillStateEnter() { state = this.m_nStateID, uid = playerSkill.GetMaster().GetUID() });

                    }
                }


            }
        }

        // 退出状态
        public override void Leave()
        {
            PlayerSkillPart playerSkill = m_SkillPart as PlayerSkillPart;
            if (SkillSystem.GetClientGlobal().IsMainPlayer(playerSkill.GetMaster()))
            {
              //  Log.Trace("Leave ..." + this.GetType().Name);
            }
        }

        public override void Update(float dt)
        {
            if (m_SkillPart == null)
            {
                return;
            }
            IEntity casetr = m_SkillPart.GetMaster();
            if (m_caster != null)
            {
                m_caster.Update(dt);
           
                return;
            }


        }

        public override void OnEvent(int nEventID, object param)
        {

        }

    }

}
