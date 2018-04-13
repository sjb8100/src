using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UICopyTargetGrid : UIGridBase
{
    #region property

    UILabel m_lblTarget;

    GameObject m_goSelect;

    GameObject m_goStar;

    BoxCollider m_boxCollider;

    public CopyTargetInfo copyTarget;

    #endregion



    #region override
    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblTarget = this.transform.Find("Target_Label").GetComponent<UILabel>();

        m_goSelect = this.transform.Find("ChoseMark").gameObject;

        m_boxCollider = this.transform.GetComponent<BoxCollider>();
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.copyTarget = data as CopyTargetInfo;
    }

    #endregion


    #region method

    public void SetName(string name)
    {
        if (m_lblTarget != null)
        {
            m_lblTarget.text = name;
        }
    }

    //public void SetNameColor(bool b)
    //{
    //    if (m_lblTarget != null)
    //    {
    //        if (b)
    //        {
    //            //绿色
    //            m_lblTarget.color = new Color(34f/255, 221f/255, 34f/255);
    //        }
    //        else
    //        {
    //            //白色
    //            m_lblTarget.color = new Color(1, 1, 1);
    //        }
    //    }
    //}

    public void SetBoxColiderY(float y)
    {
        if (m_boxCollider != null)
        {
            m_boxCollider.size = new Vector3(m_boxCollider.size.x, y, m_boxCollider.size.z);
        }
    }

    #endregion
}

