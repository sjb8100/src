using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace SkillSystem
{
     [System.Serializable]
    public class ArrowFxNodeProp : HitNodeProp
    {
        public string fx_name = "";
        public int fx_num = 1;
        public int angle = 30;
        public float speed = 10.0f;
        public float acce = 0.0f;
        public float range = 10.0f;
        public float radius = 0.5f;
        public float pitch = 0.0f;
        public float height = 1.0f;
        public float z_offset = 0.0f;

        public ArrowFxNodeProp()
        {
            type = EF_NODE_TYPE.ARROW_FX;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            fx_name = jsNode.GetString("fx_name", "");
            fx_num = jsNode.GetInt32("fx_num", 1);
            angle = jsNode.GetInt32("angle", 30);
            speed = jsNode.GetFloat("speed", 0.0f);
            acce = jsNode.GetFloat("acce", 0.0f);
            range = jsNode.GetFloat("range", 0.0f);
            radius = jsNode.GetFloat("radius", 0.5f);
            pitch = jsNode.GetFloat("pitch", 0.0f);
            height = jsNode.GetFloat("h", 1.0f);
            z_offset = jsNode.GetFloat("z_offset", 0.0f);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["fx_name"] = fx_name;
            jsNode["fx_num"] = fx_num;
            jsNode["angle"] = angle;
            jsNode["speed"] = speed;
            jsNode["acce"] = acce;
            jsNode["range"] = range;
            jsNode["radius"] = radius;
            jsNode["pitch"] = pitch;

            if (height != 0)
                jsNode["h"] = height;

            //if (z_offset > 0.001f)
            jsNode["z_offset"] = z_offset;
        }

        public override void GatherResFile(ref List<string> resItemList)
        {
            //EffectDatabase.Me.GatherResFile("skill/" + fx_name, resItemList);
            //if (hit_fx.Length > 0)
            //    EffectDatabase.Me.GatherResFile("hit/" + hit_fx, resItemList);

            string strEffectName = "effect/skill/" + fx_name;
            if (!resItemList.Contains(strEffectName))
            {
                resItemList.Add(strEffectName);
            }
        }
    }
}
