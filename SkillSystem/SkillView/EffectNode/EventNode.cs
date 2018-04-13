using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

namespace SkillSystem
{
    //事件点，触发某个事件
    public class EventNode : EffectNode
    {
        public string event_name = "";
        public string param = "";

        public EventNode()
        {
           // m_NodeProp = ScriptableObject.CreateInstance<EffectNodeProp>();// new EffectNodeProp();
        }

        //public override string Name { get { return "触发事件"; } }

        //public override void Read(JsonData jsNode)
        //{
        //    base.Read(jsNode);
        //    event_name = jsNode.GetString("event_name", "");
        //    param = jsNode.GetString("param", "");
        //}

        //public override void Write(JsonData jsNode)
        //{
        //    base.Write(jsNode);
        //    jsNode["event_name"] = event_name;
        //    jsNode["param"] = param;
        //}
        public override void Stop()
        {
            
        }
        public override void Update(float dTime) { }
        public override void Dead()
        {

        }
        public override void Play(ISkillAttacker attacker, SkillEffect se)
        {
            //player.animation.CrossFade(name, 0.2);
            //caster.SendMessage(event_name, param);
        }
    }
}