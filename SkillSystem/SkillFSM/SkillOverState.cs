using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using Engine.Utility;
using table;
namespace SkillSystem
{
    class SkillOverState : SkillStateBase
    {
        float totalTime = 0;
        public SkillOverState(StateMachine<ISkillPart> machine, ISkillPart caster)
            : base(machine, caster)
        {
            m_nStateID = (int)SkillState.Over;
        }
        ISkillPart skillPart = null;
        // 进入状态
        public override void Enter(object param)
        {
            totalTime = 0;
            skillPart = m_SkillPart;
            if (skillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
            {
                PlayerSkillPart playerSkill = skillPart as PlayerSkillPart;
                if (playerSkill.AttackAnimState != null)
                {
                    //  playerSkill.PlaySkillAnim("Skill005", false);
                    //收招
                    string aniName = playerSkill.AttackAnimState.name + "_over";
                    if (playerSkill.Master.GetAnimationState(aniName) != null)
                    {
                        playerSkill.PlaySkillAnim(aniName, false);
                    }
                }
                else
                {
                    //Engine.Utility.Log.Trace("SkillOverState.Enter {0}技能收招动作为空！", playerSkill.GetMaster().GetName());
                }
                playerSkill.Master.SendMessage(EntityMessage.EntityCommond_IgnoreMoveAction, false);
                if (playerSkill.IsMainPlayer())
                {
                    stNextSkill st = new stNextSkill();
                    IControllerSystem cs = playerSkill.GetCtrollerSys();
                    if (cs == null)
                    {
                        Log.Error("ExecuteCmd: ControllerSystem is null");
                        return;
                    }

                    if (cs.GetCombatRobot().Status == CombatRobotStatus.RUNNING)
                    {
                        if (playerSkill.IsCombo(playerSkill.CurSkillID))
                        {//挂机连击
                            st.curSkillID = playerSkill.CurSkillID;
                            SkillDoubleHitDataBase db = GameTableManager.Instance.GetTableItem<SkillDoubleHitDataBase>((uint)playerSkill.CurSkillID);
                            if (db != null)
                            {
                                st.nextSkillID = db.nextskillid;
                                if(db.nextskillid == db.beginskillid)
                                {//挂机第三招要收招
                                    st.nextSkillID = 0;
                                }
                                EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_STIFFTIMEOVER, st);
                            }
                        }
                    }
                    else
                    {//非挂机时  插入其他技能
                        st.nextSkillID = 0;
                        EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLSYSTEM_STIFFTIMEOVER, st);
                    }
                }

            }

        }

        // 退出状态
        public override void Leave()
        {
            PlayerSkillPart playerSkill = m_SkillPart as PlayerSkillPart;
            if (playerSkill.IsMainPlayer())
            {
                Log.LogGroup("ZDY", " main player leave skilloverstate");
            }
            //SkillEffect skillEffect = m_caster.EffectNode;
            //if(skillEffect != null)
            //{//此处调用stop 会影响sing技能的放置特效 此处无意义 如果以后要调用stop 可以加break函数区分stop 
            //    skillEffect.Stop();
            //}
            playerSkill.Master.SendMessage(EntityMessage.EntityCommand_ChangeMoveSpeedFact, (object)1f);

            if (SkillSystem.GetClientGlobal().IsMainPlayer(playerSkill.GetMaster()))
            {
                stForbiddenJoystick info = new stForbiddenJoystick();
                info.playerID = playerSkill.Master.GetUID();
                info.bFobidden = false;
                EventEngine.Instance().DispatchEvent((int)GameEventID.SKILL_FORBIDDENJOYSTICK, info);
            }


        }

        public override void Update(float dt)
        {
            if(m_caster != null)
            {
                m_caster.Update(dt);
            }
            totalTime += dt;
            if (skillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
            {
                PlayerSkillPart playerSkill = skillPart as PlayerSkillPart;
                if (playerSkill.AttackAnimState == null)
                {
                    m_Statemachine.ChangeState((int)SkillState.None, null);
                    return;
                }
                else if (playerSkill.AttackAnimState.time >= playerSkill.AttackAnimState.length)
                {

                    //收招完了，结束战斗
                    m_Statemachine.ChangeState((int)SkillState.None, null);
                    return;
                }
                //else if(playerSkill.AttackAnimState.time == 0)
                //{
                //    m_Statemachine.ChangeState((int)SkillState.None, null);
                //    return;
                //}

            }
            if (totalTime > m_caster.GetAniTotalTime())
            {
                // Log.Error("over state name is " + m_caster.GetAniState().name);
                m_Statemachine.ChangeState((int)SkillState.None, null);
            }

        }

        public override void OnEvent(int nEventID, object param)
        {
            if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_ENTITYBEGINMOVE)
            {
                Log.LogGroup("ZDY", " over状态 收到移动 切换到none");
                // over状态 收到移动
                m_Statemachine.ChangeState((int)SkillState.None, null);
                return;
            }
        }
    }
}
