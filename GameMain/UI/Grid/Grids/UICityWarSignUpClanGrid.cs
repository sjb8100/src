using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class UICityWarSignUpClanGrid : UIGridBase
{
    #region property
    UILabel m_lblName;


    public string m_clanName;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblName = this.transform.Find("clanNameLbl").GetComponent<UILabel>();

    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_clanName = (string)data;
    }

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }
    #endregion
}

