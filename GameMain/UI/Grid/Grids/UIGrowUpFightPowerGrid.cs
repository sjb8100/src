using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIGrowUpFightPowerGrid : UIGridBase
{
    UILabel m_lblName;

    UISprite m_spIcon;

    UILabel m_lblDes;

    UISlider m_slider;

    UILabel m_lblPercent;

    GameObject m_goGoto;

    UISprite m_spProgressBg;  //背景进度

    uint m_id;
    public uint Id
    {
        get
        {
            return m_id;
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        m_lblName = this.transform.Find("Name_label").GetComponent<UILabel>();
        m_lblDes = this.transform.Find("Desc_label").GetComponent<UILabel>();
        m_spIcon = this.transform.Find("Icon").GetComponent<UISprite>();
        m_slider = this.transform.Find("progressbar").GetComponent<UISlider>();
        m_lblPercent = this.transform.Find("progressbar/Percent").GetComponent<UILabel>();
        m_goGoto = this.transform.Find("btn_analysis").gameObject;
        m_spProgressBg = this.transform.Find("progressBg").GetComponent<UISprite>();

        UIEventListener.Get(m_goGoto).onClick = SetGotoBtn;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_id = (uint)data;

        SetAnalysisProgress(0);
    }

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetIcon(string iconName)
    {
        if (m_spIcon != null)
        {
            m_spIcon.spriteName = iconName;
        }
    }

    public void SetDes(string des)
    {
        if (m_lblDes != null)
        {
            m_lblDes.text = des;
        }
    }

    public void SetSliderAndPercent(float value)
    {
        if (m_slider != null)
        {
            m_slider.fillDirection = UIProgressBar.FillDirection.LeftToRight;
            m_slider.value = value;
        }

        if (m_lblPercent != null)
        {
            int percent = (int)(value * 100);
            m_lblPercent.text = string.Format("{0}%", percent);
        }
    }

    public void SetAnalysisProgress(float progress)
    {
        if (m_spProgressBg != null && progress <= 1)
        {
            m_spProgressBg.type = UIBasicSprite.Type.Filled;
            m_spProgressBg.fillDirection = UIBasicSprite.FillDirection.Horizontal;
            m_spProgressBg.fillAmount = progress;
        }
    }

    public void SetGotoBtn(GameObject go)
    {
        int btnIndex = 1;
        InvokeUIDlg(UIEventType.Click, this, btnIndex);
    }
}

