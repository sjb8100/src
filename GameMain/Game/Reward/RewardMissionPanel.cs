//*************************************************************************
//	创建日期:	2017-2-10 14:18
//	文件名称:	RewardMissionPanel.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	悬赏任务接取详情面板
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

partial class RewardMissionPanel : UIPanelBase
{
    List<RewardMissionGrid> m_lstRewardMissionGrid = new List<RewardMissionGrid>(5);
    protected override void OnLoading()
    {
        m_sprite_missiongrid.gameObject.SetActive(false);
        base.OnLoading();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        List<RewardMisssionInfo> lstMission = DataManager.Manager<TaskDataManager>().RewardMisssionData.ReleaseRewardList;

        m_label_tips.enabled = lstMission.Count <= 0;

        RewardMissionGrid grid = null;
        for (int i = 0; i < lstMission.Count; i++)
        {
            if (i >= m_lstRewardMissionGrid.Count)
            {
                GameObject go = NGUITools.AddChild(m_trans_missionRoot.gameObject, m_sprite_missiongrid.gameObject);
                if (go == null)
                {
                    break;
                }
                grid = go.AddComponent<RewardMissionGrid>();
                m_lstRewardMissionGrid.Add(grid);
            }
            else
            {
                grid = m_lstRewardMissionGrid[i];
            }
            grid.gameObject.SetActive(true);
            grid.Init(lstMission[i]);
            grid.transform.localPosition = new UnityEngine.Vector3(0, -i * 102, 0);
        }

        for (int i = lstMission.Count; i < m_lstRewardMissionGrid.Count; i++)
        {
            m_lstRewardMissionGrid[i].gameObject.SetActive(false);
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eRewardTaskListRefresh)
        {
            OnShow(null);
        }

        return base.OnMsg(msgid, param);
    }
    void onClick_Clostbtn_Btn(GameObject caster)
    {
        this.HideSelf();
    }
}