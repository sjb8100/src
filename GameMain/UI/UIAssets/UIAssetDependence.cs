
//*************************************************************************
//	创建日期:	2018/1/11 星期四 14:21:18
//	文件名称:	UIAssetDependence
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class UIAssetDependence : ScriptableObject, ISerializationCallbackReceiver
{
    public Dictionary<string, CMAssetList> m_dicAssetDependence = new Dictionary<string, CMAssetList>();
    [SerializeField]
    public List<CMAssetList> _listValue = new List<CMAssetList>();
    public List<string> _listKey = new List<string>();
    public void OnAfterDeserialize()
    {
        m_dicAssetDependence = new Dictionary<string, CMAssetList>();

        for (int i = 0; i != Math.Min(_listKey.Count, _listValue.Count); i++)
            m_dicAssetDependence.Add(_listKey[i], _listValue[i]);
    }

    public void OnBeforeSerialize()
    {
        _listKey.Clear();
        _listValue.Clear();

        foreach (var kvp in m_dicAssetDependence)
        {
            _listKey.Add(kvp.Key);
            _listValue.Add(kvp.Value);
        }
    }
}
