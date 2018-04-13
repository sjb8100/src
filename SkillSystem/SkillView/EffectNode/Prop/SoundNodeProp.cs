using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace SkillSystem
{
     [System.Serializable]
    public class SoundNodeProp : EffectNodeProp
    {
        public string snd_name = "";
        public bool bloop = false;
        public float endTime = 1;

        public SoundNodeProp()
        {
            type = EF_NODE_TYPE.SOUND;
        }

        public override void Read(JsonData jsNode)
        {
            base.Read(jsNode);
            snd_name = jsNode.GetString("snd_name", "");
            bloop = jsNode.GetBool("bloop", false);
            endTime = jsNode.GetFloat("endTime", 1);
        }

        public override void Write(JsonData jsNode)
        {
            base.Write(jsNode);
            jsNode["snd_name"] = snd_name;
            jsNode["bloop"] = bloop;
            jsNode["endTime"] = endTime;
        }

        public override void GatherResFile(ref List<string> resItemList)
        {
            if (snd_name == "")
                return;

            //Log.Error("sound: " + snd_name);
            //ResItem item = new ResItem(ResType.sound, snd_name, 0);
            //resItemList.Add(item);
        }
    }
}
