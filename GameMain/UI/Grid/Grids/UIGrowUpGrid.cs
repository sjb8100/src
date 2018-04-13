using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIGrowUpGrid : UIGridBase
{
    UILabel m_lblName;

    UILabel m_lblDes;

    UISprite m_spIcon;

    UISlider m_slider;

    GameObject m_goGoto;

    UILabel m_lblGoto;

    public uint Id;

    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblName = this.transform.Find("Name_label").GetComponent<UILabel>();
        m_lblDes = this.transform.Find("Desc_label").GetComponent<UILabel>();
        m_spIcon = this.transform.Find("Icon").GetComponent<UISprite>();
        m_slider = this.transform.Find("StarSlider").GetComponent<UISlider>();
        m_goGoto = this.transform.Find("btn_analysis").gameObject;
        m_lblGoto = this.transform.Find("btn_analysis/btn_label").GetComponent<UILabel>();

        UIEventListener.Get(m_goGoto).onClick = SetGotoBtn;
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.Id = (uint)data;
    }

    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetIcon(string icon)
    {
        if (m_spIcon != null)
        {
            m_spIcon.spriteName = icon;
        }
    }

    public void SetStar(uint starNum)
    {
        if (m_slider != null)
        {
            m_slider.fillDirection = UIProgressBar.FillDirection.LeftToRight;
            if (starNum > 0 && starNum < 6)
            {
                m_slider.value = starNum / 5.0f;
            }
        }
    }

    public void SetDes(string des)
    {
        if (m_lblDes != null)
        {
            m_lblDes.text = des;
        }
    }

    public void SetGotoBtnEnable(bool enable, uint openLv)
    {
        if (m_goGoto != null && m_lblGoto != null)
        {
            UIButton button = m_goGoto.GetComponent<UIButton>();
            if (button != null)
            {
                button.isEnabled = enable;
            }

            if (enable)
            {
                m_lblGoto.text = "立即前往";
            }
            else
            {
                m_lblGoto.text = string.Format("{0}级开启", openLv);
            }
        }
    }

    void SetGotoBtn(GameObject go)
    {
        int btnIndex = 1;
        InvokeUIDlg(UIEventType.Click, this, btnIndex);
    }
}

