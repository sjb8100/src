using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



class UIGrowUpSecondTypeGrid : UIGridBase
{
    UILabel m_lblName;

    GameObject m_goSelect;

    uint m_firstKeyId;

    public uint FirstKeyId
    {
        get
        {
            return m_firstKeyId;
        }
    }


    uint m_secondKeyId;

    public uint SecondKeyId
    {
        get
        {
            return m_secondKeyId;
        }
    }
    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblName = this.transform.Find("Content/Name").GetComponent<UILabel>();

        m_goSelect = this.transform.Find("Content/Select").gameObject;
    }

    public void SetData(uint firstKeyId, uint secondKeyId)
    {
        this.m_firstKeyId = firstKeyId;

        this.m_secondKeyId = secondKeyId;

    }

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetSelect(bool select)
    {
        if (m_goSelect != null && m_goSelect.gameObject.activeSelf != select)
        {
            m_goSelect.gameObject.SetActive(select);
        }
    }
}

