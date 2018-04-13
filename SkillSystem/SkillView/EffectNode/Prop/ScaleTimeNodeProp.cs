using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
     [System.Serializable]
    public class ScaleTimeNodeProp : EffectNodeProp
    {
        public float scale = 1.0f;
        public float time_len = 1.0f;

        public ScaleTimeNodeProp()
        {
            type = EF_NODE_TYPE.TIME_SCALE;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            scale = jsNode.GetFloat("scale", 1.0f);
            time_len = jsNode.GetFloat("time_len", 1.0f);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["scale"] = scale;
            jsNode["time_len"] = time_len;
        }
    }
}
