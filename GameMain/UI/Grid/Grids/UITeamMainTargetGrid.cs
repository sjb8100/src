using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UITeamMainTargetGrid : UIGridBase
{
    UILabel m_lblName;

    GameObject m_goSelect;

    uint m_mainId = 0;

    public uint MainId 
    {
        get 
        {
            return m_mainId;
        }
    }

    #region overridemethod

    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblName = this.transform.Find("TargetType/Type_Label").GetComponent<UILabel>();

        m_goSelect = this.transform.Find("TargetType/ChoseMark").gameObject;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        m_mainId = (uint)data;
    }

    #endregion

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetSelect(bool select)
    {
        if (m_goSelect != null && m_goSelect.activeSelf != select)
        {
            m_goSelect.SetActive(select);
        }
    }
}

