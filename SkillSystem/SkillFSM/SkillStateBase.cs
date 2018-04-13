using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Client;
namespace SkillSystem
{
    class SkillStateBase:Engine.Utility.State
    {
          protected Engine.Utility.StateMachine<ISkillPart> m_Statemachine = null;

          protected SkillCaster m_caster = null;

          protected ISkillPart m_SkillPart = null;
          bool m_isMainPlayer = false;
        public SkillStateBase(Engine.Utility.StateMachine<ISkillPart> machine,ISkillPart skillpart)
        {
            m_Statemachine = machine;
            m_SkillPart = skillpart;
            if(skillpart.GetSkillPartType() == SkillPartType.SKILL_PLAYERPART)
            {
                PlayerSkillPart skill = skillpart as PlayerSkillPart;

                m_isMainPlayer = SkillSystem.GetClientGlobal().IsMainPlayer(skill.GetMaster());
                m_caster = skill.Caster;
            }
           
        }

        public bool IsMainPlayer()
        {
            return m_isMainPlayer;
        }
    }
}
