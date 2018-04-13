
//*************************************************************************
//	创建日期:	2017/11/1 星期三 15:03:18
//	文件名称:	SkillConfig
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace SkillSystem
{
    public class SkillConfig : ScriptableObject,ISerializationCallbackReceiver
    {
        public Dictionary<string, SkillEffectTable> cofinData = new Dictionary<string, SkillEffectTable>();
        public List<SkillEffectTable> _listValue = new List<SkillEffectTable>();
        public List<string> _listKey = new List<string>();
        // 摘要: 
        //     Implement this method to receive a callback after Unity deserializes your
        //     object.
        public void OnAfterDeserialize()
        {
            
            cofinData = new Dictionary<string, SkillEffectTable>();

            for (int i = 0; i != Math.Min(_listKey.Count, _listValue.Count); i++)
                cofinData.Add(_listKey[i], _listValue[i]);
        }
 
       public void OnBeforeSerialize()
        {
            _listKey.Clear();
            _listValue.Clear();

            foreach (var kvp in cofinData)
            {
                _listKey.Add(kvp.Key);
                _listValue.Add(kvp.Value);
            }
        }
    }
}