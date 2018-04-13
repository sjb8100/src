using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UINvWaGuardGrid : UIGridBase
{
    UISprite m_spIcon;
    UISprite m_spIconBg;
    UILabel m_lblname;
    UILabel m_lblNum;
    UIButton m_btnRecruit;
    UIButton m_btnUpgrade;
    UILabel m_lblRecruit;
    UILabel m_lblUpgrade;

    public NvWaManager.GuardData m_guardData;

    protected override void OnAwake()
    {
        base.OnAwake();

        m_spIcon = this.transform.Find("Icon").GetComponent<UISprite>();
        m_spIconBg = this.transform.Find("Icon/IconBg").GetComponent<UISprite>();
        //m_lblname = this.transform.Find("Name").GetComponent<UILabel>();
        m_lblNum = this.transform.Find("Num").GetComponent<UILabel>();
        m_btnRecruit = this.transform.Find("BtnRecruit").GetComponent<UIButton>();
        m_btnUpgrade = this.transform.Find("BtnUpgrade").GetComponent<UIButton>();
        m_lblRecruit = this.transform.Find("BtnRecruit/Label").GetComponent<UILabel>();
        m_lblUpgrade = this.transform.Find("BtnUpgrade/Label").GetComponent<UILabel>();

        UIEventListener.Get(m_btnRecruit.gameObject).onClick = OnClickRecruit;
        UIEventListener.Get(m_btnUpgrade.gameObject).onClick = OnClickUpgrade;
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        OnAwake();

        this.m_guardData = data as NvWaManager.GuardData;

    }

    /// <summary>
    /// 设置名字
    /// </summary>
    //public void SetName(string name)
    //{
    //    if (m_lblname != null)
    //    {
    //        m_lblname.text = name;
    //    }
    //    else 
    //    {
    //        Engine.Utility.Log.Error("m_lblname 为空！！！");
    //    }
    //}

    public void SetIcon(uint iconId)
    {
        if (m_spIcon != null)
        {
        }
    }

    public void SetIconBg(string str)
    {
        if (m_spIconBg != null)
        {
            m_spIconBg.spriteName = str;
        }
    }

    public void SetNum(uint num)
    {
        if (m_lblNum != null)
        {
            m_lblNum.text = num.ToString();
        }
    }

    public void SetRecruitBtnLbl(string btnName)
    {
        if (m_lblRecruit != null)
        {
            m_lblRecruit.text = btnName;
        }
        else
        {
            Engine.Utility.Log.Error("m_lblRecruit 为空！！！");
        }
    }

    public void SetLvUpBtnLbl(string btnName)
    {
        if (m_lblUpgrade != null)
        {
            m_lblUpgrade.text = btnName;
        }
        else
        {
            Engine.Utility.Log.Error("m_lblUpgrade 为空！！！");
        }
    }

    public void SetLvUpBtnEnable(bool b)
    {
        if (m_btnUpgrade != null)
        {
            m_btnUpgrade.enabled = b;
        }
        else
        {
            Engine.Utility.Log.Error("m_btnUpgrade 为空！！！");
        }
    }


    void OnClickRecruit(GameObject go)
    {
        uint btnOne = 1;

        InvokeUIDlg(UIEventType.Click, this, btnOne);
    }

    void OnClickUpgrade(GameObject go)
    {
        uint btnTwo = 2;

        InvokeUIDlg(UIEventType.Click, this, btnTwo);
    }


}

