
//*************************************************************************
//	创建日期:	2017/11/2 星期四 9:17:41
//	文件名称:	SkillEffectTable
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace SkillSystem
{
   [System.Serializable]
    public class SkillEffectTable :ScriptableObject, ISerializationCallbackReceiver
    {
        public Dictionary<string, SkillEffectProp> m_effectDict = new Dictionary<string, SkillEffectProp>();
        #region ISerializationCallbackReceiver
        public List<SkillEffectProp> _listValue = new List<SkillEffectProp>();
        public List<string> _listKey = new List<string>();
        // 摘要: 
        //     Implement this method to receive a callback after Unity deserializes your
        //     object.
        public void OnAfterDeserialize()
        {
            m_effectDict = new Dictionary<string, SkillEffectProp>();

            for (int i = 0; i != Math.Min(_listKey.Count, _listValue.Count); i++)
                m_effectDict.Add(_listKey[i], _listValue[i]);
        }

        public void OnBeforeSerialize()
        {
            _listKey.Clear();
            _listValue.Clear();

            foreach (var kvp in m_effectDict)
            {
                _listKey.Add(kvp.Key);
                _listValue.Add(kvp.Value);
            }
        }
        #endregion
        public string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Dictionary<string, SkillEffectProp> Get()
        {
            return m_effectDict;
        }



        public void GatherResFile(ref List<string> resItemList, string skill_name, bool gather_all)
        {
            SkillEffectProp effect;
            if (m_effectDict.TryGetValue(skill_name, out effect))
            {
                effect.GatherResFile(ref resItemList, gather_all);
            }
        }

        public bool Add(SkillEffectProp e)
        {
            if (m_effectDict.ContainsKey(e.Name) == true)
            {
                return false; //重名
            }

            m_effectDict.Add(e.Name, e);
            return true;
        }

        public SkillEffectProp Add(string name)
        {
            SkillEffectProp e = ScriptableObject.CreateInstance<SkillEffectProp>();// new SkillEffectProp();
            e.Name = name;

            if (Add(e) == false)
            {
                return null;
            }

            return e;
        }

        public SkillEffectProp Get(string name)
        {
            SkillEffectProp e;
            if (m_effectDict.TryGetValue(name, out e) == false)
            {
                return null;
            }

            return e;
        }

        public void Remove(SkillEffectProp e)
        {
            m_effectDict.Remove(e.Name);
        }

        public bool Load(JsonData json_root)
        {
            #region 以名字为key
            name = json_root.GetString("name", "");

            JsonData effect_nodes = json_root["effects"];

            if (effect_nodes != null)
            {
                for (int n = 0; n < effect_nodes.Count; n++)
                {
                    SkillEffectProp e = ScriptableObject.CreateInstance<SkillEffectProp>(); //new SkillEffectProp();
                    e.Load(effect_nodes[n]);
                    Add(e);
                }
            }
            #endregion

            return true;
        }

        public bool Save(JsonData json_root)
        {
            string lowName = name.ToLower();
            json_root["name"] = lowName;
            JsonData effect_nodes = new JsonData();
            effect_nodes.SetJsonType(JsonType.Array);
            json_root["effects"] = effect_nodes;

            foreach (KeyValuePair<string, SkillEffectProp> pair in m_effectDict)
            {
                JsonData effect_node = new JsonData();
                effect_nodes.Add(effect_node);

                pair.Value.Save(effect_node);
            }

            return true;
        }
    }
}