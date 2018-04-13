//*************************************************************************
//	创建日期:	2016-11-30 21:20
//	文件名称:	CampWarResultPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	阵营战 战斗结果
//*************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

partial class CampWarResultPanel : UIPanelBase
{
    protected override void OnLoading()
    {
        base.OnLoading();
        AdjustUI();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        GameCmd.stEndCampUserCmd_S fightResult = (GameCmd.stEndCampUserCmd_S)data;

        CampCombatManager m_dataMgr = DataManager.Manager<CampCombatManager>();
        CampCombatResultInfo.CampCombatPlayerInfo myInfo = m_dataMgr.CampCombatResultData.m_MyCampCombatInfo;
        if (null == myInfo)
            return;

        bool iswin = fightResult.is_win == 1;

        if (null != m_trans_win && m_trans_win.gameObject.activeSelf != iswin)
        {
            m_trans_win.gameObject.SetActive(iswin);
        }

        if (null != m_trans_defeat && m_trans_defeat.gameObject.activeSelf == iswin)
        {
            m_trans_defeat.gameObject.SetActive(!iswin);
        }

        //得分
        if (null != m_label_scoreNum_label)
        {
            m_label_scoreNum_label.text = myInfo.nScore.ToString();
        }

        //杀敌
        if (null != m_label_KillNum)
        {
            m_label_KillNum.text = myInfo.nKill.ToString();
        }
        //死亡
        if (null != m_label_DeadNum)
        {
            m_label_DeadNum.text = myInfo.nDead.ToString();
        }
        //助攻
        if (null != m_label_AssistNum)
        {
            m_label_AssistNum.text = myInfo.nAssist.ToString();
        }

        //神魔经验
        m_label_GodDemonExp.text = fightResult.camp_exp.ToString();
        //阵营战积分
        m_label_CampIntegral.text = fightResult.camp_coin.ToString();
    }

    private void AdjustUI()
    {
        if (null != m_sprite_ResultBg)
        {
            m_sprite_ResultBg.width = (int)UIRootHelper.Instance.TargetSize.x;
        }
    }

    void onClick_Btnclose_Btn(GameObject caster)
    {
        // 需要等待结算完成
        //HideSelf();
        DataManager.Manager<ComBatCopyDataManager>().ReqExitCopy();
    }

    void onClick_BtnList_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CampWarBattleListPanel, prePanelData :GetPanelData());
    }
}