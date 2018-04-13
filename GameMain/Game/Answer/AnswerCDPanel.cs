using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


partial class AnswerCDPanel : ITimer
{
    int m_CDTime = 3;  //开始倒计时

    private const int ANSWERCD_TIMERID = 1000;

    TweenScale m_tweenScale;

    #region override

    protected override void OnLoading()
    {
        base.OnLoading();
        m_tweenScale = m_label_CD.GetComponent<TweenScale>();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object data)
    {
        float questionTime = DataManager.Manager<AnswerManager>().QuestionTime;
        if (questionTime > 0 && questionTime <= 5)
        {
            this.m_CDTime = Mathf.CeilToInt(questionTime);

            m_label_CD.gameObject.SetActive(true);

            m_label_CD.text = m_CDTime.ToString();
            m_tweenScale.ResetToBeginning();
            m_tweenScale.from = new Vector3(1f, 1f, 1f);
            m_tweenScale.to = new Vector3(0.1f, 0.1f, 0.1f);
            m_tweenScale.duration = 1f;
            m_tweenScale.PlayForward();

            TimerAxis.Instance().KillTimer(ANSWERCD_TIMERID, this);
            TimerAxis.Instance().SetTimer(ANSWERCD_TIMERID, 1000, this);
        }
        else
        {
            m_label_CD.gameObject.SetActive(false);
        }
    }

    protected override void OnHide()
    {
        base.OnHide();

        this.m_CDTime = 0;
        TimerAxis.Instance().KillTimer(ANSWERCD_TIMERID, this);
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();

    }

    #endregion

    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == ANSWERCD_TIMERID)
        {
            this.m_CDTime--;
            if (this.m_CDTime > 0)
            {
                m_label_CD.text = this.m_CDTime.ToString();

                m_tweenScale.ResetToBeginning();

                m_tweenScale.from = new Vector3(1f, 1f, 1f);
                m_tweenScale.to = new Vector3(0.1f, 0.1f, 0.1f);
                m_tweenScale.duration = 1f;

                m_tweenScale.PlayForward();
            }
            else
            {
                if (this.m_CDTime <= 0)
                {
                    //结束3、2、1 正式开始战斗
                    m_label_CD.gameObject.SetActive(false);

                    TimerAxis.Instance().KillTimer(ANSWERCD_TIMERID, this);
                }

            }
        }
    }

}

