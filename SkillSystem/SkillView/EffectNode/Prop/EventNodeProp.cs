using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace SkillSystem
{
     [System.Serializable]
    public class EventNodeProp : EffectNodeProp
    {
        public string event_name = "";
        public string param = "";

        public EventNodeProp()
        {
            type = EF_NODE_TYPE.EVENT;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);
            event_name = jsNode.GetString("event_name", "");
            param = jsNode.GetString("param", "");
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);
            jsNode["event_name"] = event_name;
            jsNode["param"] = param;
        }
    }
}
