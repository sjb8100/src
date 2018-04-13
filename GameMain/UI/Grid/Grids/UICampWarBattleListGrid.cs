using System;
using System.Collections.Generic;
using UnityEngine;

class UICampWarBattleListGrid : UIGridBase
{
    UILabel m_labelRank = null;
    UILabel m_labelName = null;
    UILabel m_labelScore = null;
    UILabel m_labelKill = null;
    UILabel m_labelDead = null;
    UILabel m_labelAssists = null;

    UISprite m_spGodBg = null;
    UISprite m_spDemonBg = null;

    CampCombatResultInfo.CampCombatPlayerInfo m_playerInfo = null;
    protected override void OnAwake()
    {
        base.OnAwake();

        Transform title = transform.Find("Tittle");
        if (title != null)
        {
            m_labelRank = title.Find("Rank").GetComponent<UILabel>();
            m_labelName = title.Find("Name").GetComponent<UILabel>();
            m_labelScore = title.Find("Score").GetComponent<UILabel>();
            m_labelKill = title.Find("Kill").GetComponent<UILabel>();
            m_labelDead = title.Find("Dead").GetComponent<UILabel>();
            m_labelAssists = title.Find("Assists").GetComponent<UILabel>();

            
        }

        m_spGodBg = transform.Find("bg/god").GetComponent<UISprite>();
        m_spDemonBg = transform.Find("bg/Demon").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data is CampCombatResultInfo.CampCombatPlayerInfo)
        {
            m_playerInfo = data as CampCombatResultInfo.CampCombatPlayerInfo;
            bool visible = (m_playerInfo.camp == GameCmd.eCamp.CF_Green) ;
            if (null != m_spGodBg && m_spGodBg.gameObject.activeSelf != visible)
            {
                m_spGodBg.gameObject.SetActive(visible);
            }

            visible = !visible;
            if (null != m_spDemonBg && m_spDemonBg.gameObject.activeSelf != visible)
            {
                m_spDemonBg.gameObject.SetActive(visible);
            }

            if (m_labelRank != null)
            {
                m_labelRank.text = m_playerInfo.nRank.ToString();
            } 
            if (m_labelName != null)
            {
                m_labelName.text = m_playerInfo.strName;
            }
            if (m_labelKill != null)
            {
                m_labelKill.text = m_playerInfo.nKill.ToString();
            }

            if (m_labelDead != null)
            {
                m_labelDead.text = m_playerInfo.nDead.ToString();
            }

            if (m_labelScore != null)
            {
                m_labelScore.text = m_playerInfo.nScore.ToString();
            }
            if (m_labelAssists != null)
            {
                m_labelAssists.text = m_playerInfo.nAssist.ToString();
            }
        }
    }
}