using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//*******************************************************************************************
//	创建日期：	2017-8-16   16:35
//	文件名称：	MissionAndTeamPanel_CopyBattleInfo,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	ZQB
//	说    明：	副本战况
//*******************************************************************************************


partial class MissionAndTeamPanel
{

    const uint m_shenMoCopyId1 = 4001; //神魔大战（有boss）
    const uint m_shenMoCopyId2 = 4003; //神魔大战（无boss）

    void OnValueUpdateEventArgs(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("CampKillInfo") || value.key.Equals("RefreshBattleData"))
        {
            RefreshBattleList();
        }
    }

    void UpdateCopyBattleInfo()
    {
        DataManager.Manager<CampCombatManager>().ValueUpdateEvent += OnValueUpdateEventArgs;

        RefreshBattleList();
    }

    void InitCopyBattleInfoWidget()
    {
        uint enterCopyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;

        int childCount = m_trans_copyBattleInfoRoot.childCount;
        for (int i = 0; i < m_trans_copyBattleInfoRoot.childCount; i++)
        {
            Transform ts = m_trans_copyBattleInfoRoot.GetChild(i);

            if (enterCopyId == m_shenMoCopyId1)
            {
                ts.gameObject.SetActive(true);
                ts.localPosition = new Vector3(ts.localPosition.x, -70 * i, ts.localPosition.z);
            }


            if (enterCopyId == m_shenMoCopyId2)
            {
                if (i == 0)
                {
                    ts.gameObject.SetActive(true);
                    ts.localPosition = new Vector3(ts.localPosition.x, -50, ts.localPosition.z);
                }
                else
                {
                    ts.gameObject.SetActive(false);
                }
            }


        }
    }

    void RefreshBattleList()
    {
        CampCombatResultInfo campCombatResultInfo = DataManager.Manager<CampCombatManager>().CampCombatResultData;
        if (campCombatResultInfo == null)
        {
            return;
        }

        uint enterCopyId = DataManager.Manager<ComBatCopyDataManager>().EnterCopyID;

        CampCombatResultInfo.CampCombatResult campCombatResult = campCombatResultInfo.m_camp_Green;
        if (campCombatResult != null)
        {
            //击退玩家
            uint killSum = 0;
            for (int i = 0; i < campCombatResult.m_lstCampCombatPlayers.Count; i++)
            {
                killSum += campCombatResult.m_lstCampCombatPlayers[i].nKill;
            }
            m_label_shenLabel0.text = killSum.ToString();

            //复活点占领
            uint reliveNum = campCombatResult.nReliveNum > 0 ? campCombatResult.nReliveNum - 1 : 0;
            m_label_shenLabel1.text = reliveNum.ToString();

            //击退首领
            m_label_shenLabel2.text = campCombatResult.nKillBossNum.ToString();
        }

        campCombatResult = campCombatResultInfo.m_camp_Red;
        if (campCombatResult != null)
        {
            //击退玩家
            uint killSum = 0;
            for (int i = 0; i < campCombatResult.m_lstCampCombatPlayers.Count; i++)
            {
                killSum += campCombatResult.m_lstCampCombatPlayers[i].nKill;
            }
            m_label_moLabel0.text = killSum.ToString();

            //复活点占领
            uint reliveNum = campCombatResult.nReliveNum > 0 ? campCombatResult.nReliveNum - 1 : 0;
            m_label_moLabel1.text = reliveNum.ToString();

            //击退首领
            m_label_moLabel2.text = campCombatResult.nKillBossNum.ToString();

            //m_lstCampCombatDemon = campCombatResult.m_lstCampCombatPlayers;
            //m_label_DemonScore.text = campCombatResult.nScore.ToString();
            //m_label_CampWarDemonBossNum.text = campCombatResult.nKillBossNum.ToString();
            //m_label_CampWarDemonReLivePointNum.text = campCombatResult.nReliveNum.ToString();
            //m_label_DemonNum.text = (null != campCombatResult.m_lstCampCombatPlayers)
            //    ? campCombatResult.m_lstCampCombatPlayers.Count.ToString() : "0";
        }

    }
}

