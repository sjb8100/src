using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UICityWarFightTotemGrid : UIGridBase
{
    UILabel m_lblClanName;

    UISlider m_sliderHp;

    UILabel m_lblHpNum;

    GameObject m_goSelect;

    UISprite m_spIcon;

    CityWarTotem m_cityWarTotem;

    public CityWarTotem CityWarTotemData
    {
        get
        {
            return m_cityWarTotem;
        }
    }

    #region property


    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lblClanName = this.transform.Find("ClanName").GetComponent<UILabel>();
        m_sliderHp = this.transform.Find("Bg_Jingdutiao").GetComponent<UISlider>();
        m_lblHpNum = this.transform.Find("Bg_Jingdutiao/Number").GetComponent<UILabel>();
        m_goSelect = this.transform.Find("Bg_xuanzhong").gameObject;
        m_spIcon = this.transform.Find("Sprite").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        OnAwake();

        this.m_cityWarTotem = data as CityWarTotem;
    }

    public void SetClanName(string clanName)
    {
        if (m_lblClanName != null)
        {
            m_lblClanName.text = clanName;
        }
    }

    public void SetIcon(string iconName)
    {
        if (m_spIcon != null)
        {
            m_spIcon.spriteName = iconName;
        }
    }

    public void SetSelect(bool select)
    {
        if (m_goSelect != null && m_goSelect.activeSelf != select)
        {
            m_goSelect.SetActive(select);
        }
    }

    public void SetHp(uint hp, uint maxHp)
    {
        if (m_sliderHp != null && hp >= 0 && maxHp > 0)
        {
            m_sliderHp.value = (float)hp / maxHp;
        }

        if (m_lblHpNum != null)
        {
            m_lblHpNum.text = string.Format("{0}/{1}", hp, maxHp);
        }
    }

    #endregion
}

