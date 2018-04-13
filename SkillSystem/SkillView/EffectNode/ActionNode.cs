using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace SkillSystem
{

    //角色动作节点
    public class ActionNode : EffectNode
    {
        ISkillAttacker m_attacker;
        SkillEffect m_ef = null;

        public ActionNode()
        {

        }

        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            m_ef = se;
            ActionNodeProp prop = m_NodeProp as ActionNodeProp;
            if (prop != null)
            {
                m_attacker = attacker;
                if (attacker != null)
                {
                    attacker.PlaySkillAnim(prop.name, prop.IsAttackSatte, prop.loopCount, prop.speed, prop.offset);

                }
            }
        }
        public override void FreeToNodePool()
        {
            if (m_ef != null)
            {
                m_ef.FreeSkillNode(this);
            }
        }
        public override void Stop()
        {
          //  FreeToNodePool();
        }
        public override void Dead()
        {
            FreeToNodePool();
        }
        public override void Update(float dTime)
        {

        }
        public static ActionNode Create()
        {
            return new ActionNode();
        }
    }
}