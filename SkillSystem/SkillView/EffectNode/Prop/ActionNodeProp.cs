using System;
using System.Collections.Generic;
using System.Text;
using LitJson;

namespace SkillSystem
{
     [System.Serializable]
    public class ActionNodeProp : EffectNodeProp
    {
        public string name = "";
        public float speed = 1.0f;
        public float offset = 0.0f;
        public int loopCount = 1;
        public bool IsAttackSatte = true;
        public ActionNodeProp()
        {
            type = EF_NODE_TYPE.ACTION;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);

            name = jsNode.GetString("name", "");
            speed = jsNode.GetFloat("speed", 1.0f);
            offset = jsNode.GetFloat("offset", 0.0f);
            loopCount = jsNode.GetInt32("loopCount", 1);
            IsAttackSatte = jsNode.GetBool( "IsAttackSatte" , true );
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);

            jsNode["name"] = name;
            jsNode["speed"] = speed;
            jsNode["offset"] = offset;
            jsNode["loopCount"] = loopCount;
            jsNode["IsAttackSatte"] = IsAttackSatte;
        }
    }
}
