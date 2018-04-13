using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
    //单次伤害节点
    public class SingleHitNode : HitNode
    {
        public SingleHitNode()
        {
           // m_NodeProp = ScriptableObject.CreateInstance<SingleHitNodeProp>();//  new SingleHitNodeProp();
        }

        //public override void Read(JsonData jsNode)
        //{
        //    base.Read(jsNode);
        //}

        //public override void Write(JsonData jsNode)
        //{
        //    base.Write(jsNode);
        //}
        public override void Dead()
        {

        }
        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            //player.animation.CrossFade(name, 0.2);
            //通知释法者，计算伤害
            //Debug.Log("OnComputeDamage hit ");
            //caster.SendMessage("OnComputeDamage", id, SendMessageOptions.DontRequireReceiver);
            //skill_id = attacker.GetCurSkillId();
            skill_id = se.CurSkillID;
            m_uDamageID = attacker.GetDamageID();
            attacker.OnComputeHit(this);
        }

        public override void Stop() { }
        public override void Update(float dTime) { }

        public override void GatherResFile(ref List<string> resItemList)
        {
          
        }
    }
}