
//*************************************************************************
//	创建日期:	2017/5/8 星期一 15:10:37
//	文件名称:	UITabGridGroup
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UITabGridGroup:MonoBehaviour
{
    Dictionary<string, UITabGrid> m_dicGroup = new Dictionary<string, UITabGrid>();
    void Awake()
    {
        InitGroup();
    }
    public void InitGroup()
    {
        foreach (var tab in transform.GetComponentsInChildren<UITabGrid>())
        {
            if (!m_dicGroup.ContainsKey(tab.gameObject.name))
            {
                m_dicGroup.Add(tab.gameObject.name, tab);
            }
        }
    }
    public void SetHighLight(UITabGrid tab)
    {
        foreach(var dic in m_dicGroup)
        {
            if(dic.Value == tab)
            {
                tab.SetHightLight(true);
            }
            else
            {
                dic.Value.SetHightLight(false);
            }
        }
    }
    public void SetHighLightTabGameObject(GameObject go)
    {
        UITabGrid tab = go.GetComponent<UITabGrid>();
        if(tab == null)
        {
            Engine.Utility.Log.Error(go.name + " has no UITabGrid Script");
            return;
        }
        SetHighLight(tab);
    }
}
