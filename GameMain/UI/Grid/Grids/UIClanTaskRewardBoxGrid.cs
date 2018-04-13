using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIClanTaskRewardBoxGrid : UIGridBase
{
    GameObject m_goIcon;

    GameObject m_goLock;

    uint m_itemId;

    public uint ItemId 
    {
        get 
        {
            return m_itemId;
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        m_goIcon = this.transform.Find("icon").gameObject;

        m_goLock = this.transform.Find("lock").gameObject;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_itemId = (uint)data;
    }

    public void SetLock(bool b)
    {
        if (m_goIcon != null && m_goLock != null)
        {
            if (b)
            {
                m_goLock.SetActive(true);
            }
            else
            {
                m_goLock.SetActive(false);
            }
        }

    }

}

