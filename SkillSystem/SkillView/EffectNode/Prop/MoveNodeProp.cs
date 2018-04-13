using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace SkillSystem
{
    // 移动节点
     [System.Serializable]
    public class MoveNodeProp : EffectNodeProp
    {
        public float speed = 0.0f;
        public float dist = 3.0f;
        public float angle = 0.0f;
        public float time_len = 0.0f;

        public MoveNodeProp()
        {
            type = EF_NODE_TYPE.MOVE;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            speed = jsNode.GetFloat("speed", 15.0f);
            dist = jsNode.GetFloat("dist", 3.0f);
            angle = jsNode.GetFloat("angle", 0.0f);
            time_len = jsNode.GetFloat("time_len", 0.2f);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["speed"] = speed;
            jsNode["dist"] = dist;
            jsNode["angle"] = angle;
            jsNode["time_len"] = time_len;
        }
    }
}
