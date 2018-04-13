using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
     [System.Serializable]
    public class MultHitNodeProp : HitNodeProp
    {
        public int hit_times = 1;
        public float delta_time = 0.5f;
        public Vector3 position = Vector3.zero;
        //public float radius = 2.0f;
        public bool attach = false;

        public MultHitNodeProp()
        {
            type = EF_NODE_TYPE.MULT_HIT;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            hit_times = jsNode.GetInt32("hit_times", 1);
            delta_time = jsNode.GetFloat("delta_time", 0.5f);

            position.x = jsNode.GetFloat("px", 0.0f);
            position.y = jsNode.GetFloat("py", 0.0f);
            position.z = jsNode.GetFloat("pz", 0.0f);

            //radius = jsNode.GetFloat("r", 0.5f);
            attach = jsNode.GetBool("attach", false);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["hit_times"] = hit_times;
            jsNode["delta_time"] = delta_time;

            if (position.x != 0.0f)
            {
                jsNode["px"] = position.x;
            }

            if (position.y != 0.0f)
            {
                jsNode["py"] = position.y;
            }

            if (position.z != 0.0f)
            {
                jsNode["pz"] = position.z;
            }

            //jsNode["r"] = radius;

            if (attach)
            {
                jsNode["attach"] = attach;
            }
        }

        public override void GatherResFile(ref List<string> resItemList)
        {
            //if (hit_fx.Length > 0)
            //    EffectDatabase.Me.GatherResFile("hit/" + hit_fx, resItemList);
        }
    }
}
