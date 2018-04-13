using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIFishingRankGrid : UIGridBase
{

    #region property

    UILabel m_lblRank;

    UILabel m_lblName;

    UILabel m_lblClanName;

    UILabel m_lblNum;

    UILabel m_lblScore;

    GameObject first;

    GameObject second;

    GameObject third;

    GameObject m_goWihte;

    GameObject m_goBlue;

    public FishingRankInfo info;

    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblRank = this.transform.Find("rank_label").GetComponent<UILabel>();

        m_lblName = this.transform.Find("name_label").GetComponent<UILabel>();

        m_lblClanName = this.transform.Find("clan_label").GetComponent<UILabel>();

        m_lblNum = this.transform.Find("num_label").GetComponent<UILabel>();

        m_lblScore = this.transform.Find("score_label").GetComponent<UILabel>();

        first = this.transform.Find("top3/first").gameObject;

        second = this.transform.Find("top3/second").gameObject;

        third = this.transform.Find("top3/third").gameObject;

        m_goWihte = this.transform.Find("bg/wihte").gameObject;

        m_goBlue = this.transform.Find("bg/blue").gameObject;

    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.info = data as FishingRankInfo;
    }

    public void SetRank(uint rank)
    {
        if (m_lblRank != null && first != null && second != null && third != null)
        {
            if (rank == 1)
            {
                first.SetActive(true);
                second.SetActive(false);
                third.SetActive(false);
                m_lblRank.text = "";
            }
            else if (rank == 2)
            {
                first.SetActive(false);
                second.SetActive(true);
                third.SetActive(false);
                m_lblRank.text = "";
            }
            else if (rank == 3)
            {
                first.SetActive(false);
                second.SetActive(false);
                third.SetActive(true);
                m_lblRank.text = "";
            }
            else
            {
                first.SetActive(false);
                second.SetActive(false);
                third.SetActive(false);
                m_lblRank.text = rank.ToString();
            }
        }

        SetBg(rank);
    }

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetBg(uint rank)
    {
        if (m_goWihte != null && m_goBlue != null)
        {
            if (rank % 2 == 0)
            {
                m_goWihte.SetActive(true);
                m_goBlue.SetActive(false);
            }
            else
            {
                m_goWihte.SetActive(false);
                m_goBlue.SetActive(true);
            }
        }
    }


    public void SetClanName(string clanName)
    {
        if (m_lblClanName != null)
        {
            m_lblClanName.text = clanName;
        }
    }

    public void SetNum(uint num)
    {
        if (m_lblNum != null)
        {
            m_lblNum.text = num.ToString();
        }

    }

    public void SetScore(uint score)
    {
        if (m_lblScore != null)
        {
            m_lblScore.text = score.ToString();
        }
    }

    #endregion

}

