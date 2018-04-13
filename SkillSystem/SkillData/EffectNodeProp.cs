
//*************************************************************************
//	创建日期:	2017/11/2 星期四 9:26:38
//	文件名称:	EffectNodeProp
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LitJson;
namespace SkillSystem
{
  
    public class EffectNodeProp : UnityEngine.ScriptableObject
    {
        public EF_NODE_TYPE type = EF_NODE_TYPE.ACTION;
        public float time = 0.0f;
        public bool only_main_role = false;
        public bool is_hit_node = false;
        public bool bActive = false;
        public int nodeID = 0;
        public string nodeState = "None";//Client.SkillState.None.ToString();
        public virtual void Read(JsonData jsNode)
        {
            type = (EF_NODE_TYPE)(jsNode.GetInt32("type", 0));
            time = jsNode.GetFloat("time", 0.0f);
            bActive = jsNode.GetBool("enable", true);
            only_main_role = jsNode.GetBool("only_main_role", false);
            nodeID = jsNode.GetInt32("nodeID", 0);
            // nodeState = jsNode.GetString("nodeState", Client.SkillState.None.ToString());
            nodeState = jsNode.GetString("nodeState", "None");
        }

        public virtual void Write(JsonData jsNode)
        {
            jsNode["type"] = (int)type;
            jsNode["time"] = time;
            jsNode["enable"] = bActive;
            jsNode["only_main_role"] = only_main_role;
            jsNode["nodeID"] = nodeID;
            jsNode["nodeState"] = nodeState;
        }
        public EffectNodeProp Clone()
        {

            Type t = GetType();
            object obj = Activator.CreateInstance(t);
            System.Reflection.FieldInfo[] infoArray = t.GetFields();
            for (int i = 0; i < infoArray.Length; i++)
            {
                var info = infoArray[i];
                object tempValue = info.GetValue(this);
                info.SetValue(obj, tempValue);
            }
            return obj as EffectNodeProp;
        }
        public virtual void GatherResFile(ref List<string> resItemList)
        {

        }
    }
}