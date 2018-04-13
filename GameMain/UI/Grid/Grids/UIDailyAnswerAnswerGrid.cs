using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIDailyAnswerAnswerGrid : UIGridBase
{
    UILabel m_AnswerLbl;

    GameObject m_right;

    GameObject m_wrong;

    GameObject m_select;

    public DailyAnswerAnswer m_data;

    protected override void OnAwake()
    {
        base.OnAwake();

        m_AnswerLbl = this.transform.Find("questionText_label").GetComponent<UILabel>();

        m_right = this.transform.Find("right_sprite").gameObject;

        m_wrong = this.transform.Find("wrong_sprite").gameObject;

        m_select = this.transform.Find("choose").gameObject;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_data = (DailyAnswerAnswer)data;
    }

    public void SetAnswerDes(string des)
    {
        if (m_AnswerLbl != null)
        {
            m_AnswerLbl.text = des;
        }
    }

    public void SetRightOrWrong(bool b)
    {
        if (m_right != null && m_wrong != null)
        {
            if (b)
            {
                m_right.SetActive(true);
                m_wrong.SetActive(false);
            }
            else
            {
                m_right.SetActive(false);
                m_wrong.SetActive(true);
            }
        }
    }

    public void ReSetRightOrWrong()
    {
        if (m_right != null && m_wrong != null)
        {
            m_right.SetActive(false);
            m_wrong.SetActive(false);
        }
    }


    public void SetSelect(bool b)
    {
        if (m_select != null)
        {
            m_select.SetActive(b);
        }
    }
}

