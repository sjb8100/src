using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
    //移动节点，作用于施法者
    public class MoveNode : EffectNode
    {
        //public float speed = 0.0f;
        //public float dist = 3.0f;
        //public float angle = 0.0f;
        //public float time_len = 0.0f;


        public MoveNode()
        {
          //  m_NodeProp = ScriptableObject.CreateInstance<MoveNodeProp>();// new MoveNodeProp();
        }

        //public override string Name { get { return "位置变换"; } }

        //public override void Read(JsonData jsNode)
        //{
        //    base.Read(jsNode);

        //    speed = jsNode.GetFloat("speed", 15.0f);
        //    dist = jsNode.GetFloat("dist", 3.0f);
        //    angle = jsNode.GetFloat("angle", 0.0f);
        //    time_len = jsNode.GetFloat("time_len", 0.2f);
        //}

        //public override void Write(JsonData jsNode)
        //{
        //    base.Write(jsNode);

        //    jsNode["speed"] = speed;
        //    jsNode["dist"] = dist;
        //    jsNode["angle"] = angle;
        //    jsNode["time_len"] = time_len;
        //}

        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            //player.animation.CrossFade(name, 0.2);
            // caster.SendMessage("OnStartSkillMove", this);

            attacker.OnStartSkillMove(this);
        }
        public override void Dead()
        {

        }
        public override void Stop() { }
        public override void Update(float dTime) { }

        public float GetAcc()
        {
            MoveNodeProp prop = m_NodeProp as MoveNodeProp;
            if(prop==null)
            {
                return 0.0f;
            }

            if (prop.dist <= 0.0001f)
                return 0.0f;

            return (prop.dist - prop.speed * prop.time_len) / (prop.time_len * prop.time_len);
        }
    }

}