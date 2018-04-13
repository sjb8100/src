using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIAccumulativeDailyGrid : UIGridBase
{
    UILabel m_lblName;

    GameObject m_goSelect;

    public uint Id;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lblName = this.transform.Find("Name").GetComponent<UILabel>();

        m_goSelect = this.transform.Find("Select").gameObject;
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.Id = (uint)data;
    }

    public void SetName(string name)
    {
        if (m_lblName !=  null)
        {
            m_lblName.text = name;
        }
    }

    public void SetSelect(bool b) 
    {
        if (m_goSelect != null)
        {
            m_goSelect.SetActive(b);
        }
    }
}

