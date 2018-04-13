using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
     [System.Serializable]
    public class PlaceFxNodeProp : EffectNodeProp
    {
        public string fx_name = "";

        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
        public float scale = 1.0f;

        public bool by_target = false;

        public PlaceFxNodeProp()
        {
            type = EF_NODE_TYPE.PLACE_FX;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            fx_name = jsNode.GetString("fx_name", "");
            //dist = jsNode.GetFloat("dist", 0.0f);
            position.x = jsNode.GetFloat("px", 0.0f);
            position.y = jsNode.GetFloat("py", 0.0f);
            position.z = jsNode.GetFloat("pz", 0.0f);

            rotation.x = jsNode.GetFloat("rx", 0.0f);
            rotation.y = jsNode.GetFloat("ry", 0.0f);
            rotation.z = jsNode.GetFloat("rz", 0.0f);

            by_target = jsNode.GetBool("by_target", false);

            scale = jsNode.GetFloat("scale", 1.0f);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["fx_name"] = EffectUtil.GetFxPath(fx_name);

            if (position.x != 0.0f)
                jsNode["px"] = position.x;

            if (position.y != 0.0f)
                jsNode["py"] = position.y;

            if (position.z != 0.0f)
                jsNode["pz"] = position.z;

            if (rotation.x != 0.0f)
                jsNode["rx"] = rotation.x;

            if (rotation.y != 0.0f)
                jsNode["ry"] = rotation.y;

            if (rotation.z != 0.0f)
                jsNode["rz"] = rotation.z;

            if (by_target)
                jsNode["by_target"] = by_target;

            if (scale != 1.0f)
                jsNode["scale"] = scale;
        }

        public override void GatherResFile(ref List<string> resItemList)
        {
            //EffectDatabase.Me.GatherResFile("skill/" + fx_name, resItemList);
            string strEffectName = "effect/skill/" + fx_name;
            if (!resItemList.Contains(strEffectName))
            {
                resItemList.Add(strEffectName);
            }
        }
    }
}
