using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
     [System.Serializable]
    public class ShakeCameraNodeProp : EffectNodeProp
    {
        public float duration = 0.2f;//持续时间
        public float strength = 0.5f;//振幅
        public int vibrato = 10;//频率

        public ShakeCameraNodeProp()
        {
            type = EF_NODE_TYPE.CAMERA;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            duration = jsNode.GetFloat("duration", 0.2f);
            strength = jsNode.GetFloat("strength", 0.5f);
            vibrato = jsNode.GetInt32("vibrato", 10);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["duration"] = duration;
            jsNode["strength"] = strength;
            jsNode["vibrato"] = vibrato;
        }
    }
}
