using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;
using System.Reflection;
using Client;
namespace SkillSystem
{


    public class HitNodeProp : EffectNodeProp
    {
        public int hit_index = 0;
        public bool hit_back = true;
        public bool hit_fly = true;
        public string hit_fx = "";

        //释放时的技能 ID
        public int skill_id = 0;

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            hit_back = jsNode.GetBool("hit_back", false);
            hit_fly = jsNode.GetBool("hit_fly", false);
            hit_fx = jsNode.GetString("hit_fx", "");
            skill_id = jsNode.GetInt32("skill_id", 0);
            //JsonData range_node_list = null;
            //range_list.Clear();

            //try
            //{
            //    range_node_list = jsNode["range_list"];
            //}
            //catch(Exception e)
            //{
            //    range_node_list = null;
            //}

            //if (range_node_list != null && range_node_list.Count > 0)
            //{
            //    for (int i = 0; i < range_node_list.Count; i++)
            //    {
            //        HitRange.Type type = (HitRange.Type)range_node_list[i].GetInt32("type", 0);
            //        HitRange range = null;

            //        if (type != HitRange.Type.NONE)
            //        {
            //            range = HitRange.Create(type);
            //            range.Read(range_node_list[i]);

            //        }
            //        range_list[i] = range;
            //    }
            //}
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            if (hit_back)
                jsNode["hit_back"] = hit_back;

            if (hit_fly)
                jsNode["hit_fly"] = hit_fly;

            if (string.IsNullOrEmpty(hit_fx) == false)
                jsNode["hit_fx"] = hit_fx;

        
        }
    }

    public class CastOverNodeProp : EffectNodeProp
    {
        public CastOverNodeProp()
        {
            type = EF_NODE_TYPE.CastOverNode;
        }

        public override void Read(JsonData jsNode)
        {
        }

        public override void Write(JsonData jsNode)
        {
        }
    }

}
