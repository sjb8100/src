using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class MainPanel
{
    AnswerManager.eAnswerState m_answerState;

    AnswerManager m_answerManager;

    /// <summary>
    /// 初始化
    /// </summary>
    void InitAnswerUI()
    {
        this.m_answerManager = DataManager.Manager<AnswerManager>();

        if (m_answerManager.InAnswerCopy == true)
        {
            m_trans_AnswerRoot.gameObject.SetActive(true);

            this.m_answerState = m_answerManager.AnswerState;

            InitAnswerStateUI(this.m_answerState);
        }
        else
        {
            m_trans_AnswerRoot.gameObject.SetActive(false);
        }


    }

    float m_answerTempTime;
    float m_answerTempTime2;
    void UpdateAnswerUI()
    {
        if (m_answerManager.InAnswerCopy == false)
        {
            return;
        }

        this.m_answerTempTime2 += Time.deltaTime;
        if (m_answerTempTime2 > 0.95f)
        {
            m_answerTempTime2 = 0;
            if (this.m_answerManager.AnswerState == AnswerManager.eAnswerState.BeforeAnswerRule)
            {
                int leftTime = (int)this.m_answerManager.GetPreStartLeftTime();
                m_label_AnswerRuleLeftTime.text = string.Format("距本轮活动开始 {0:D2}:{1:D2}", leftTime / 60, leftTime % 60);
                if (leftTime <= 0)
                {
                    m_label_AnswerRuleLeftTime.gameObject.SetActive(false);
                }
            }
        }

        this.m_answerTempTime += Time.deltaTime;
        if (this.m_answerTempTime > 0.1f)
        {
            this.m_answerTempTime = 0;

            //开始答题前倒计时5、4、3、2、1、开始答题
            if (this.m_answerManager.AnswerState == AnswerManager.eAnswerState.BeforeAnswerDescCD)
            {
                if (this.m_answerManager.QuestionIndex == 0)
                {
                    int preStartCD = (int)this.m_answerManager.PreStartTime;
                    if (preStartCD == 0)
                    {
                        m_label_AnswerReadyCDNum.text = "开始答题";
                    }
                    else
                    {
                        m_label_AnswerReadyCDNum.text = preStartCD.ToString();
                    }
                }
                else
                {
                    m_label_AnswerReadyCDNum.text = Mathf.CeilToInt(this.m_answerManager.TrueOrFalseTime).ToString();
                }
            }

            //答题20s
            if (this.m_answerManager.AnswerState == AnswerManager.eAnswerState.InQuestion)
            {
                m_label_QuestionCDLabel.text = Mathf.CeilToInt(this.m_answerManager.QuestionTime).ToString();
            }


            //即将进入下一题 3、2、1
            if (this.m_answerManager.AnswerState == AnswerManager.eAnswerState.AnswerNextCD)
            {
                m_label_AnswerReadyCDNum.text = Mathf.CeilToInt(this.m_answerManager.TrueOrFalseTime).ToString();
            }
        }

        //答题20s
        if (this.m_answerManager.AnswerState == AnswerManager.eAnswerState.InQuestion)
        {
            float questionLeftTime = this.m_answerManager.QuestionTime;

            if (questionLeftTime > 5)
            {
                if (m_sprite_QuestionCD.gameObject.activeSelf == false)
                {
                    m_sprite_QuestionCD.gameObject.SetActive(true);
                }

                if (m_sprite_QuestionCD.gameObject.activeSelf == false)
                {
                    m_sprite_QuestionCD.gameObject.SetActive(true);
                }


                float fillAmount = questionLeftTime / 20f;
                m_sprite_QuestionCD.fillAmount = fillAmount;

            }
            else
            {
                if (m_sprite_QuestionCD.gameObject.activeSelf == true)
                {
                    m_sprite_QuestionCD.gameObject.SetActive(false);
                }
            }

        }
    }

    void InitAnswerStateUI(AnswerManager.eAnswerState answerState)
    {
        //还未开始答题， > 活动开启10s
        if (answerState == AnswerManager.eAnswerState.BeforeAnswerRule)
        {
            m_trans_AnswerRuleRoot.gameObject.SetActive(true);
            m_label_AnswerReady.gameObject.SetActive(false);
            m_trans_AnswerReadyCD.gameObject.SetActive(false);
            m_trans_AnswerQuestion.gameObject.SetActive(false);
            m_trans_AnswerAnswer.gameObject.SetActive(false);

            table.LangTextDataBase langtextDb = GameTableManager.Instance.GetTableItem<table.LangTextDataBase>(160002);
            m_label_AnswerRuleDesc.text = langtextDb.strText;

            int leftTime = (int)this.m_answerManager.GetPreStartLeftTime();
            if (leftTime > 0)
            {
                m_label_AnswerRuleLeftTime.gameObject.SetActive(true);
                m_label_AnswerRuleLeftTime.text = string.Format("距本轮活动开始 {0:D2}:{1:D2}", leftTime / 60, leftTime % 60);
            }
            else
            {
                m_label_AnswerRuleLeftTime.gameObject.SetActive(false);
            }

        }

        else if (answerState == AnswerManager.eAnswerState.BeforeAnswerDesc)
        {
            m_trans_AnswerRuleRoot.gameObject.SetActive(false);
            m_label_AnswerReady.gameObject.SetActive(true);
            m_trans_AnswerReadyCD.gameObject.SetActive(false);
            m_trans_AnswerQuestion.gameObject.SetActive(false);
            m_trans_AnswerAnswer.gameObject.SetActive(false);
        }

        else if (answerState == AnswerManager.eAnswerState.BeforeAnswerDescCD)
        {
            m_trans_AnswerRuleRoot.gameObject.SetActive(false);
            m_label_AnswerReady.gameObject.SetActive(false);
            m_trans_AnswerReadyCD.gameObject.SetActive(true);
            m_trans_AnswerQuestion.gameObject.SetActive(false);
            m_trans_AnswerAnswer.gameObject.SetActive(false);

            if (this.m_answerManager.QuestionIndex == 0)
            {
                m_label_AnswerReadyCDTitle.text = "即将开始答题";
                float PreStartleftTime = this.m_answerManager.PreStartTime;
                m_label_AnswerReadyCDNum.text = ((int)PreStartleftTime).ToString();
            }
            else
            {
                m_label_AnswerReadyCDTitle.text = "即将进入下一题";
                m_label_AnswerReadyCDNum.text = Mathf.CeilToInt(this.m_answerManager.TrueOrFalseTime).ToString();
            }

        }

        else if (answerState == AnswerManager.eAnswerState.InQuestion)
        {
            m_trans_AnswerRuleRoot.gameObject.SetActive(false);
            m_label_AnswerReady.gameObject.SetActive(false);
            m_trans_AnswerReadyCD.gameObject.SetActive(false);
            m_trans_AnswerQuestion.gameObject.SetActive(true);
            m_trans_AnswerAnswer.gameObject.SetActive(false);

            m_sprite_QuestionCD.gameObject.SetActive(true);

            uint index = DataManager.Manager<AnswerManager>().QuestionIndex;
            uint maxNum = DataManager.Manager<AnswerManager>().MaxNum;
            string title = string.Format("第{0}/{1}题", index, maxNum);
            m_label_QuestionTitle.text = title;

            uint questionId = DataManager.Manager<AnswerManager>().QuestionId;
            table.RightOrWrongDataBase db = GameTableManager.Instance.GetTableItem<table.RightOrWrongDataBase>(questionId);
            if (db != null)
            {
                m_label_QuestionDesc.text = db.answer;
            }


            float questionLeftTime = this.m_answerManager.QuestionTime;

            float fillAmount = questionLeftTime / 20f;
            m_sprite_QuestionCD.fillAmount = fillAmount;
            m_label_QuestionCDLabel.text = Mathf.CeilToInt(questionLeftTime).ToString();
        }

        else if (answerState == AnswerManager.eAnswerState.InQuestionCD)
        {
            m_trans_AnswerRuleRoot.gameObject.SetActive(false);
            m_label_AnswerReady.gameObject.SetActive(false);
            m_trans_AnswerReadyCD.gameObject.SetActive(false);
            m_trans_AnswerQuestion.gameObject.SetActive(true);
            m_trans_AnswerAnswer.gameObject.SetActive(false);

            m_sprite_QuestionCD.gameObject.SetActive(false);

            //中间弹出5、4、3、2、1

        }

        else if (answerState == AnswerManager.eAnswerState.AnswerAnswer)//答案  o  x
        {
            m_trans_AnswerRuleRoot.gameObject.SetActive(false);
            m_label_AnswerReady.gameObject.SetActive(false);
            m_trans_AnswerReadyCD.gameObject.SetActive(false);
            m_trans_AnswerQuestion.gameObject.SetActive(false);
            m_trans_AnswerAnswer.gameObject.SetActive(true);

            bool trueOrFalse = DataManager.Manager<AnswerManager>().AnswerTrueOrFalse;
            m_sprite_True.gameObject.SetActive(false);
            m_sprite_False.gameObject.SetActive(false);

            //添加特效
            m_widget_answerEffectRoot.gameObject.SetActive(true);
            AddTrueOrFalseEffect(trueOrFalse);
        }

        else if (answerState == AnswerManager.eAnswerState.AnswerNextCD)
        {
            m_trans_AnswerRuleRoot.gameObject.SetActive(false);
            m_label_AnswerReady.gameObject.SetActive(false);
            m_trans_AnswerReadyCD.gameObject.SetActive(true);
            m_trans_AnswerQuestion.gameObject.SetActive(false);
            m_trans_AnswerAnswer.gameObject.SetActive(false);

            m_label_AnswerReadyCDTitle.text = "即将进入下一题";
            m_label_AnswerReadyCDNum.text = Mathf.CeilToInt(this.m_answerManager.TrueOrFalseTime).ToString();//3、2、1
        }

        else
        {
            m_trans_AnswerRuleRoot.gameObject.SetActive(true);
            m_label_AnswerReady.gameObject.SetActive(false);
            m_trans_AnswerReadyCD.gameObject.SetActive(false);
            m_trans_AnswerQuestion.gameObject.SetActive(false);
            m_trans_AnswerAnswer.gameObject.SetActive(false);

            table.LangTextDataBase langtextDb = GameTableManager.Instance.GetTableItem<table.LangTextDataBase>(160002);
            m_label_AnswerRuleDesc.text = langtextDb.strText;
        }
    }

    void AddTrueOrFalseEffect(bool b)
    {
        UIParticleWidget wight = m_widget_answerEffectRoot.GetComponent<UIParticleWidget>();
        if (null == wight)
        {
            wight = m_widget_answerEffectRoot.gameObject.AddComponent<UIParticleWidget>();
            wight.depth = 100;
        }

        if (wight != null)
        {
            wight.ReleaseParticle();
            if (b)
            {
                wight.AddParticle(50052);
            }
            else
            {
                wight.AddParticle(50051);
            }

        }
    }


}

