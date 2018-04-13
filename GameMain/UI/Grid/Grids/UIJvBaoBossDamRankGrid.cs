/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIJvBaoBossDamRankGrid
 * 版本号：  V1.0.0.0
 * 创建时间：3/26/2018 11:56:17 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIJvBaoBossDamRankGrid : UIGridBase
{
    #region property

    UILabel m_lblRank;

    UILabel m_lblName;

    UILabel m_lblClanName;

    UILabel m_lblPro;

    UILabel m_lblDam;

    GameObject first;

    GameObject second;

    GameObject third;

    GameObject m_goWihte;

    GameObject m_goBlue;

    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblRank = this.transform.Find("rank_label").GetComponent<UILabel>();

        m_lblName = this.transform.Find("name_label").GetComponent<UILabel>();

        m_lblClanName = this.transform.Find("clan_label").GetComponent<UILabel>();

        m_lblPro = this.transform.Find("pro_label").GetComponent<UILabel>();

        m_lblDam = this.transform.Find("dam_label").GetComponent<UILabel>();

        first = this.transform.Find("top3/first").gameObject;

        second = this.transform.Find("top3/second").gameObject;

        third = this.transform.Find("top3/third").gameObject;

        m_goWihte = this.transform.Find("bg/wihte").gameObject;

        m_goBlue = this.transform.Find("bg/blue").gameObject;

    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null != data && data is GameCmd.BossDamRank)
        {
            GameCmd.BossDamRank bossDamRank = data as GameCmd.BossDamRank;
            SetRank(bossDamRank.rank);
            SetName(bossDamRank.name);
            SetClanName(bossDamRank.clan);
            SetPro(bossDamRank.job);
            SetDam(bossDamRank.damage);
        }
    }

    /// <summary>
    /// 排名
    /// </summary>
    /// <param name="rank"></param>
    private void SetRank(uint rank)
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

    /// <summary>
    /// 名称
    /// </summary>
    /// <param name="name"></param>
    private void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    /// <summary>
    /// 背框
    /// </summary>
    /// <param name="rank"></param>
    private void SetBg(uint rank)
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

    /// <summary>
    /// 氏族名称
    /// </summary>
    /// <param name="clanName"></param>
    private void SetClanName(string clanName)
    {
        if (m_lblClanName != null)
        {
            m_lblClanName.text = clanName;
        }
    }

    /// <summary>
    /// 伤害
    /// </summary>
    /// <param name="num"></param>
    private void SetDam(uint num)
    {
        if (m_lblDam != null)
        {
            m_lblDam.text = num.ToString();
        }

    }

    /// <summary>
    /// 职业
    /// </summary>
    /// <param name="pro"></param>
    private void SetPro(uint pro)
    {
        if (m_lblPro != null)
        {
            string proName =  DataManager.Manager<TextManager>().GetProfessionName((GameCmd.enumProfession)pro);
            m_lblPro.text = proName;
        }
    }

    #endregion
}