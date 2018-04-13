
//*************************************************************************
//	创建日期:	2017/11/2 星期四 9:24:23
//	文件名称:	EffectNodeConfig
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
    
    public class EffectNodeConfig : ScriptableObject
    {
        public List<EffectNodeProp> configlist = new List<EffectNodeProp>();
        public void AddNodeConfig(EffectNodeProp node)
        {
            if (!configlist.Contains(node))
            {
                configlist.Add(node);
            }
        }
        public void RemoveNodeConfig(EffectNodeProp node)
        {
            if (configlist.Contains(node))
            {
                configlist.Remove(node);
            }
        }
    }
}