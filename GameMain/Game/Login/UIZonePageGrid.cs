using System;
using System.Collections.Generic;
using UnityEngine;

public class UIZonePageGrid : UIGridBase
{
    UILabel m_labelTitle = null;
    GameObject m_goSelect = null;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_labelTitle = transform.Find("Label").GetComponent<UILabel>();
        m_goSelect = transform.Find("mask").gameObject;
    }

    private int m_iIndex = 0;
    public int Index
    {
        get
        {
           return m_iIndex;
        }
    }
  
    public void SetServerPageData(string name,int index,bool select = false)
    {
        if (null != m_labelTitle)
        {
            m_labelTitle.text = name;
        }
        m_iIndex = index;
        SetHightLight(select);
    }

   public override void SetHightLight(bool hightLight)
   {
       base.SetHightLight(hightLight);
       if (m_goSelect != null)
       {
           m_goSelect.SetActive(hightLight);
       }
   }
}