using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UITitleCategoryGrid : UIGridBase
{

    UILabel m_lblName;
    GameObject m_select;
    GameObject m_newMark;

    public uint m_typeId;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lblName = this.transform.Find("Label").GetComponent<UILabel>();
        m_select = this.transform.Find("select").gameObject;
        m_newMark = this.transform.Find("warning").gameObject;

    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_typeId = (uint)data;
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
        if (m_select != null && m_select.activeSelf != select)
        {
            m_select.SetActive(select);
        }
    }

    public void SetNewMark(bool newMark)
    {
        if (m_newMark != null && m_newMark.activeSelf != newMark)
        {
            m_newMark.SetActive(newMark);
        }
    }


}

