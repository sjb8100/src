using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class AnswerSuccessPanel : ITimer
{
    private const int AnswerSuccess_TIMERID = 1000;

    int m_answerSuccessCD = 8;
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
        RewardItemData itemData = data as RewardItemData;

        m_label_itemCount.text = itemData.itemNum.ToString();

        //通关特效
        AddEffect();

        m_answerSuccessCD = 8;
        TimerAxis.Instance().SetTimer(AnswerSuccess_TIMERID, 1000, this);
    }

    protected override void OnHide()
    {
        base.OnHide();
        TimerAxis.Instance().KillTimer(AnswerSuccess_TIMERID, this);
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

    }

    /// <summary>
    /// //通关特效
    /// </summary>
    void AddEffect()
    {
        UIParticleWidget wight = m_trans_effectRoot.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = m_trans_effectRoot.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        if (wight != null)
        {
            wight.ReleaseParticle();
            wight.AddParticle(50050);
        }
    }

    #endregion


    void onClick_Btn_right_Btn(GameObject caster)
    {
        HideSelf();
    }

    void onClick_ResultBgCenter_Btn(GameObject caster)
    {
        HideSelf();

    }


    public void OnTimer(uint uTimerID)
    {
        if (AnswerSuccess_TIMERID == uTimerID)
        {
            m_answerSuccessCD--;
            if (m_answerSuccessCD > 0)
            {
                m_label_right_Label.text = "点击屏幕关闭(" + m_answerSuccessCD.ToString() + ")";
            }
            else
            {
                HideSelf();
                TimerAxis.Instance().KillTimer(AnswerSuccess_TIMERID, this);
            }
        }
    }
}

