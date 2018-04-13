using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


partial class ArenaCheckRewardPanel : UIPanelBase
{


    UIGridCreatorBase m_rewardGridCreator;

    List<ArenaRankRewardDatabase> m_rewardList;

    #region override
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    //在窗口第一次加载时，调用
    protected override void OnLoading()
    {
        m_rewardList = GameTableManager.Instance.GetTableList<ArenaRankRewardDatabase>();
        InitWidget();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    /// <summary>
    /// 界面显示回调
    /// </summary>
    /// <param name="data"></param>
    protected override void OnShow(object data)
    {
        base.OnShow(data);

        CreateMainPlayerReward();

        CreateRewardRank();
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {

        return true;
    }

    protected override void OnHide()
    {
        Release();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_rewardGridCreator != null)
        {
            m_rewardGridCreator.Release(depthRelease);
        }

    }


    #endregion

    /// <summary>
    /// 初始化组件
    /// </summary>
    void InitWidget()
    {
        m_rewardGridCreator = m_trans_listScrollView.gameObject.GetComponent<UIGridCreatorBase>();
        if (m_rewardGridCreator == null)
        {
            m_rewardGridCreator = m_trans_listScrollView.gameObject.AddComponent<UIGridCreatorBase>();
        }

       // GameObject obj = UIManager.GetResGameObj(GridID.Uiarenarewardgrid) as GameObject;

        m_rewardGridCreator.gridContentOffset = new Vector2(0, 0);
        m_rewardGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Vertical;
        m_rewardGridCreator.gridWidth = 740;
        m_rewardGridCreator.gridHeight = 61;

        m_rewardGridCreator.RefreshCheck();
        m_rewardGridCreator.Initialize<UIArenaRewardGrid>(m_trans_UIArenaRewardGrid.gameObject, OnGridUpdateData, null);
    }

    void CreateRewardRank()
    {
        if (m_rewardGridCreator != null)
        {
            m_rewardGridCreator.CreateGrids(m_rewardList != null ? m_rewardList.Count : 0);
        }
    }

    void CreateMainPlayerReward()
    {
        uint myRank = DataManager.Manager<ArenaManager>().Rank;

        ArenaRankRewardDatabase rewardDb = m_rewardList.Find((data) => { return myRank >= data.rank_cap && myRank <= data.rank_floor; });

        if (rewardDb != null)
        {
            string des = string.Empty;
            if (rewardDb.rank_cap == rewardDb.rank_floor)
            {
                des = string.Format("保持当前名次（第{0}名）可获得奖励：", rewardDb.rank_cap);
            }
            else
            {
                des = string.Format("保持当前名次（第{0}名 - 第{1}名）可获得奖励：", rewardDb.rank_cap, rewardDb.rank_floor);
            }

            m_label_ArenaRewardInfoLbl.text = des;
            m_label_wenqian_label.text = rewardDb.ticket_reward.ToString();
            m_label_jingbi_label.text = rewardDb.gold_reward.ToString();
            m_label_gongxian_label.text = rewardDb.score_reward.ToString();
            m_label_Exp_label.text = rewardDb.experience_reward.ToString();
        }
        else
        {
            m_label_ArenaRewardInfoLbl.text = string.Format("您的当前排名低于第{0}名，无法获得奖励。再接再厉！", m_rewardList[m_rewardList.Count - 1].rank_floor);
            m_label_wenqian_label.text = "0";
            m_label_jingbi_label.text = "0";
            m_label_gongxian_label.text = "0";
            m_label_Exp_label.text = "0";
        }

    }

    void OnGridUpdateData(UIGridBase data, int index)
    {
        if (m_rewardList != null && index < m_rewardList.Count)
        {
            UIArenaRewardGrid grid = data as UIArenaRewardGrid;
            if (grid == null)
            {
                return;
            }

            grid.SetGridData(m_rewardList[index].id);

            if (index < 3)
            {
                grid.SetRewardRankSpr(m_rewardList[index].rank_cap);
            }
            else
            {
                grid.SetRewardRankLbl(m_rewardList[index].rank_cap, m_rewardList[index].rank_floor);
            }

            grid.SetRewardOne(m_rewardList[index].ticket_reward);
            grid.SetRewardTwo(m_rewardList[index].gold_reward);
            grid.SetRewardThree(m_rewardList[index].score_reward);
            grid.SetRewardFour(m_rewardList[index].experience_reward);
        }
    }


    void onClick_Close_Btn(GameObject caster)
    {
        HideSelf();
    }
}

