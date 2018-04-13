using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class AnswerBreakPanel : ITimer
{
    List<RewardItemData> m_lstRewardItem;

    private const int AnswerBreak_TIMERID = 1000;

    int m_answerBreakCD = 8;

    #region override

    protected override void OnLoading()
    {
        base.OnLoading();

    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        uint qIndex = DataManager.Manager<AnswerManager>().QuestionIndex;

        m_label_BiaoTi_Label.text = string.Format("闯关到第{0}题", qIndex);

        m_lstRewardItem = data as List<RewardItemData>;
        if (m_lstRewardItem == null)
        {
            Engine.Utility.Log.Error("奖励数据为 null ");
            return;
        }

        m_ctor_itemRoot.RefreshCheck();
        m_ctor_itemRoot.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateRewardGrid, null);

        m_ctor_itemRoot.CreateGrids(m_lstRewardItem.Count);

        m_answerBreakCD = 8;
        TimerAxis.Instance().SetTimer(AnswerBreak_TIMERID, 1000, this);
    }

    protected override void OnHide()
    {
        base.OnHide();

        TimerAxis.Instance().KillTimer(AnswerBreak_TIMERID, this);
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

    }

    #endregion

    #region method

    private void OnUpdateRewardGrid(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid data = grid as UIItemRewardGrid;
            if (index < m_lstRewardItem.Count)
            {
                data.SetGridData(m_lstRewardItem[index].itemId, m_lstRewardItem[index].itemNum, false, false);
            }
        }
    }

    #endregion

    #region btn

    void onClick_Btn_Close_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_Btn_right_Btn(GameObject caster)
    {
        HideSelf();
    }

    #endregion

    public void OnTimer(uint uTimerID)
    {
        if (AnswerBreak_TIMERID == uTimerID)
        {
            m_answerBreakCD--;
            if (m_answerBreakCD > 0)
            {
                m_label_right_Label.text = "点击关闭（"+ m_answerBreakCD.ToString()+"）";
            }
            else
            {
                HideSelf();
                TimerAxis.Instance().KillTimer(AnswerBreak_TIMERID, this);
            }
        }
    }

}

