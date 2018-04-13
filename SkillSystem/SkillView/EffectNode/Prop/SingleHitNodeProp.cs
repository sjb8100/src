using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace SkillSystem
{
     [System.Serializable]
    public class SingleHitNodeProp : HitNodeProp
    {
        public SingleHitNodeProp()
        {
            type = EF_NODE_TYPE.SINGLE_HIT;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);
        }

        public override void GatherResFile(ref List<string> resItemList)
        {
            //if (hit_fx.Length > 0)
            //    EffectDatabase.Me.GatherResFile("hit/" + hit_fx, resItemList);
        }
    }
}
