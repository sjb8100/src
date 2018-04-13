using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



class UICityWarMemberInfoGrid : UIGridBase
{

    #region property
    UILabel m_lbLRank;
    UILabel m_lblName;
    UILabel m_lblClanName;
    UILabel m_lblKillNum;
    UILabel m_lblDeathNum;

    CityWarHero m_cityWarHero;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lbLRank = this.transform.Find("Content/num").GetComponent<UILabel>();
        m_lblName = this.transform.Find("Content/name").GetComponent<UILabel>();
        m_lblClanName = this.transform.Find("Content/clan").GetComponent<UILabel>();
        m_lblKillNum = this.transform.Find("Content/kill").GetComponent<UILabel>();
        m_lblDeathNum = this.transform.Find("Content/death").GetComponent<UILabel>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_cityWarHero = data as CityWarHero;
    }

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetClanName( string clanName)
    {
        if (m_lblClanName != null)
        {
            m_lblClanName.text = clanName;
        }
    }

    public void SetRank(int rank)
    {
        if (m_lbLRank != null)
        {
            m_lbLRank.text = rank.ToString();
        }
    }

    public void SetKillNum(uint killNum)
    {
        if (m_lblKillNum != null)
        {
            m_lblKillNum.text = killNum.ToString();
        }
    }

    public void SetDeathNum(uint deathNum)
    {
        if  (m_lblDeathNum != null)
        {
            m_lblDeathNum.text = deathNum.ToString();
        }
    }

    #endregion
}

