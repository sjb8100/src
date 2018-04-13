using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using Client;
using table;
using UnityEngine;
namespace SkillSystem
{
    class SkillPrepareState : SkillStateBase
    {
        ISkillPart skillPart = null;
        float preparetime = 0;
        SkillDatabase database = null;
        public SkillPrepareState(StateMachine<ISkillPart> statemachine, ISkillPart caster)
            : base(statemachine, caster)
        {
            m_nStateID = (int)SkillState.Prepare;
            skillPart = m_SkillPart;
        }
        // 进入状态
        public override void Enter(object param)
        {
            IEntity target = skillPart.GetSkillTarget();

            // 只有主角会进入战斗状态
            if (skillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART && EntitySystem.EntityHelper.IsMainPlayer(skillPart.GetMaster()))
            {
                PlayerSkillPart playerSkill = skillPart as PlayerSkillPart;
                if (playerSkill != null)
                {
                    playerSkill.SkillStartPlayTime = Time.realtimeSinceStartup;
                    playerSkill.SetFighting(true);
                }

            }
            SkillDatabase skilltable = skillPart.GetCurSkillDataBase();
            if (skilltable == null)
            {
                return;
            }
            // 朝向目标
            if (target != null)
            {
                if (skilltable.targetType != (int)SkillTargetType.TargetForwardPoint)
                {
                    skillPart.GetMaster().SendMessage(EntityMessage.EntityCommand_LookTarget, target.GetPos());
                }
            }

            database = skilltable;
            //准备时间结束 发送结束消息
            if (SkillSystem.GetClientGlobal().IsMainPlayer(skillPart.GetMaster()) && skilltable.dwReadTime > 0) // 非瞬发技能
            {//读条技能
                Client.stUninterruptMagic evenparam = new Client.stUninterruptMagic();
                evenparam.time = database.dwReadTime;
                evenparam.type = GameCmd.UninterruptActionType.UninterruptActionType_SkillCJ;
                evenparam.uid = skillPart.GetMaster().GetUID();
                EventEngine.Instance().DispatchEvent((int)GameEventID.SKILLGUIDE_PROGRESSSTART, evenparam);
            }

            preparetime = 0;


        }

        // 退出状态
        public override void Leave()
        {

        }

        public override void Update(float dt)
        {
            if (database == null)
            {
                Log.LogGroup("ZDY", "database is null");
                m_Statemachine.ChangeState((int)SkillState.None, null);
            }

            m_caster.Update(dt);
            preparetime += dt;
            if (skillPart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
            {
                PlayerSkillPart playerSkill = skillPart as PlayerSkillPart;

                if (database != null)
                {//准备阶段 包含准备时间和读条时间
                    if (preparetime * 1000 > database.dwPerpareTime + database.dwReadTime)
                    {
                        m_Statemachine.ChangeState((int)SkillState.Attack, null);
                    }
                }

            }
        }

        public override void OnEvent(int nEventID, object param) { }


    }
}
