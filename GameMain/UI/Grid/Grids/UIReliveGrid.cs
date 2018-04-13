
using Engine.Utility;
using GameCmd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;


class UIReliveGrid : UIGridBase
{

    #region Property

    UILabel m_reliveDes;

    UISprite m_moneyIcon;

    UILabel m_cost;    //消耗多少钱币

    UIButton m_Btn;

    string m_btnDes;

    public ReLiveDataManager.ReLiveData m_info;
    #endregion


    #region mono
    float tempTime = 0f;
    void Update()
    {
        if (this.m_info.reliveCd < 0)
        {
            return;
        }

        tempTime += Time.deltaTime;
        if (tempTime > 0.95f)
        {
            if (this.m_info.reliveCd > 0)
            {
                m_reliveDes.text = string.Format("{0}({1})", this.m_btnDes, this.m_info.reliveCd);
            }
            else
            {
                m_reliveDes.text = this.m_btnDes;
                m_Btn.isEnabled = true;
            }
        }
    }


    #endregion

    #region override
    protected override void OnAwake()
    {
        base.OnAwake();

        m_reliveDes = this.transform.Find("reliveDes").GetComponent<UILabel>();

        m_moneyIcon = this.transform.Find("relive_cost/Sprite").GetComponent<UISprite>();

        m_cost = this.transform.Find("relive_cost").GetComponent<UILabel>();

        m_Btn = this.transform.GetComponent<UIButton>();

        // UIEventListener.Get(m_Btn.gameObject).onClick = OnClickBtn;
    }

    public override void SetGridData(object data)
    {
        OnAwake();

        base.SetGridData(data);

        m_info = data as ReLiveDataManager.ReLiveData;
        ReliveDataBase db = GameTableManager.Instance.GetTableItem<ReliveDataBase>(m_info.reliveId);

        if (m_info == null || db == null)
        {
            Engine.Utility.Log.Error("--->>>数据出错！！！");
            return;
        }

        //按钮倒计时
        if (db.btnEnable == 0)
        {
            m_Btn.isEnabled = false;
        }
        else if (db.btnEnable == 1)
        {
            m_Btn.isEnabled = true;
        }
        SetBtnDes(db.strName, m_info.reliveCd);

        ReLiveDataManager RDMgr = DataManager.Manager<ReLiveDataManager>();

        //消耗货币
        uint costNum = 0;
        costNum = db.costStart + db.costAdd * RDMgr.ReliveTimes;
        if (costNum > db.costMax)
        {
            costNum = db.costMax;
        }
        SetCost(costNum);
        string moneyIcon = MainPlayerHelper.GetMoneyIconByType((ClientMoneyType)db.moneyType);
        SetMoneyIcon(moneyIcon);
    }
    #endregion

    #region method

    public void SetMoneyIcon(string moneyIcon)
    {
        if (m_moneyIcon != null)
        {
            m_moneyIcon.spriteName = moneyIcon;
        }
    }

    public void SetBtnDes(string des, int cd = 0)
    {
        if (m_reliveDes != null)
        {
            if (cd > 0)
            {
                this.m_btnDes = des;
                m_reliveDes.text = string.Format("{0}({1})", des, cd);
            }
            else
            {
                m_reliveDes.text = des;
            }
        }
    }

    public void SetCost(uint cost)
    {
        if (m_cost != null)
        {
            if (cost > 0)
            {
                m_cost.gameObject.SetActive(true);
                m_cost.text = cost.ToString();
            }
            else
            {
                m_cost.gameObject.SetActive(false);
            }
        }
    }

    #endregion
}

