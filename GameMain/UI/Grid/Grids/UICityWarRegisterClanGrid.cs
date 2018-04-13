using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UICityWarRegisterClanGrid : UIGridBase
{
    UILabel m_lblRank;

    UILabel m_lblClanName;

    UILabel m_lblClanLeader;

    UILabel m_lblNum;

    GameObject m_goFirst;

    GameObject m_goSecond;

    GameObject m_goThird;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lblRank = this.transform.Find("Rank_label").GetComponent<UILabel>();
        m_lblClanName = this.transform.Find("ClanName_label").GetComponent<UILabel>();
        m_lblClanLeader = this.transform.Find("Player_label").GetComponent<UILabel>();
        m_lblNum = this.transform.Find("Number_label").GetComponent<UILabel>();
        m_goFirst = this.transform.Find("Rank_label/First").gameObject;
        m_goSecond = this.transform.Find("Rank_label/Second").gameObject;
        m_goThird = this.transform.Find("Rank_label/Third").gameObject;

    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
    }

    public void SetRank(uint rank)
    {
        if (m_lblRank != null && m_goFirst != null && m_goSecond != null && m_goThird != null)
        {
            if (rank == 1)
            {
                m_goFirst.SetActive(true);
                m_goSecond.SetActive(false);
                m_goThird.SetActive(false);
                m_lblRank.text = "";
            }
            else if (rank == 2)
            {
                m_goFirst.SetActive(false);
                m_goSecond.SetActive(true);
                m_goThird.SetActive(false);
                m_lblRank.text = "";
            }
            else if (rank == 3)
            {
                m_goFirst.SetActive(false);
                m_goSecond.SetActive(false);
                m_goThird.SetActive(true);
                m_lblRank.text = "";
            }
            else
            {
                m_goFirst.SetActive(false);
                m_goSecond.SetActive(false);
                m_goThird.SetActive(false);
                m_lblRank.text = rank.ToString();
            }
            
        }
    }

    public void SetClanName(string clanName)
    {
        if (m_lblClanName != null)
        {
            m_lblClanName.text = clanName.ToString();
        }
    }

    public void SetClanLeader(string name)
    {
        if (m_lblClanLeader != null)
        {
            m_lblClanLeader.text = name;
        }
    }

    public void SetNum(uint num)
    {
        if (m_lblNum != null)
        {
            m_lblNum.text = num.ToString();
        }
    }
}

