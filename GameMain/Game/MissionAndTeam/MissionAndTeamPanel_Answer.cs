using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class MissionAndTeamPanel
{
    /// <summary>
    /// 答题
    /// </summary>
    void InitAnswer()
    {
        m_label_JingBiNum.text = DataManager.Manager<AnswerManager>().AllMoney.ToString();

        m_label_PlayerNum.text = DataManager.Manager<AnswerManager>().PlayerNum.ToString();

        //复活卡数量
        int num = DataManager.Manager<AnswerManager>().GetFuHuoNum();
        m_label_reliveLbl.text = string.Format("复活卡x{0}", num);

        if (num > 0)
        {
            m_sprite_reliveIconTrue.gameObject.SetActive(true);
            m_sprite_reliveIconFalse.gameObject.SetActive(false);
        }
        else
        {
            //复活卡不足置灰
            m_sprite_reliveIconTrue.gameObject.SetActive(false);
            m_sprite_reliveIconFalse.gameObject.SetActive(true);
        }

    }

}

