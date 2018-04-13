using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
    //时间缩放节点，慢镜头效果等
    public class ScaleTimeNode : EffectNode
    {
//         public float scale = 1.0f;
//         public float time_len = 1.0f;

        public ScaleTimeNode()
        {
         //   m_NodeProp = ScriptableObject.CreateInstance<ScaleTimeNodeProp>();// new ScaleTimeNodeProp();
        }

//         public override void Read(JsonData jsNode)
//         {
//             base.Read(jsNode);
// 
//             scale = jsNode.GetFloat("scale", 1.0f);
//             time_len = jsNode.GetFloat("time_len", 1.0f);
//         }
// 
//         public override void Write(JsonData jsNode)
//         {
//             base.Write(jsNode);
// 
//             jsNode["scale"] = scale;
//             jsNode["time_len"] = time_len;
//         }
        public override void Dead()
        {

        }
        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            ScaleTimeNodeProp prop = m_NodeProp as ScaleTimeNodeProp;
            if (prop == null)
            {
                return;
            }

            // 不建议使用直接修改 timeScale的方法
            Time.timeScale = prop.scale;
        }

        public override void Stop() { }
        public override void Update(float dTime) { }
    }
}