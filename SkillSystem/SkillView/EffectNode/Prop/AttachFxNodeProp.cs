using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
     [System.Serializable]
    public class AttachFxNodeProp : EffectNodeProp
    {
        public string attach_name = "Root";
        public string fx_name = "";

        public Vector3 offset_pos = Vector3.zero;
        public float scale = 1.0f;

        public AttachFxNodeProp()
        {
            type = EF_NODE_TYPE.ATTACH_FX;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            attach_name = jsNode.GetString("attach_name", "");
            fx_name = jsNode.GetString("fx_name", "");

            offset_pos.x = jsNode.GetFloat("x", 0);
            offset_pos.y = jsNode.GetFloat("y", 0);
            offset_pos.z = jsNode.GetFloat("z", 0);

            scale = jsNode.GetFloat("scale", 1.0f);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["attach_name"] = attach_name;
            jsNode["fx_name"] = EffectUtil.GetFxPath(fx_name);

            if (only_main_role == true)
                jsNode["only_main_role"] = only_main_role;

            if (offset_pos.x != 0.0f)
                jsNode["x"] = offset_pos.x;

            if (offset_pos.y != 0.0f)
                jsNode["y"] = offset_pos.y;

            if (offset_pos.z != 0.0f)
                jsNode["z"] = offset_pos.z;

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
