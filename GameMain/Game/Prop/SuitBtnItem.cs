//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;
using System;

partial class SuitBtnItem: UIGridBase
{
    Action<string> m_callBack = null;
    bool m_bOpen = true;
    UISprite m_spr = null;
    public void InitSuitBtnItem(bool bOpen,string iconName,string hilightName,string btnName,Action<string> click)
    {
        m_bOpen = bOpen;
        if(m_sprite_open != null)
        {
            m_sprite_open.gameObject.SetActive(!bOpen);
        }
        if(m_sprite_Checkmark != null)
        {
            m_sprite_Checkmark.spriteName = iconName;
        }

        m_spr = m_btn_suitItem.GetComponent<UISprite>();
        if (m_spr != null)
        {
            m_spr.spriteName = iconName;
        }
        if(m_label_nameLabel != null)
        {
            m_label_nameLabel.text = btnName;
        }
        if(m_sprite_highlight != null)
        {
            m_sprite_highlight.spriteName = hilightName;
        }
        m_sprite_Checkmark.alpha = 1;
        m_sprite_open.alpha = 1;
        if(click != null)
        {
            if(!bOpen)
            {
                m_callBack = null;
            }
            else
            {
                m_callBack = click;
            }
        }
    }
    public void SetSprHightLight(bool bHigh)
    {
       
        if (m_sprite_highlight != null)
        {
            m_sprite_highlight.gameObject.SetActive(bHigh);
        }
        if (m_sprite_Checkmark != null)
        {
            m_sprite_Checkmark.gameObject.SetActive(!bHigh);
        }
        if (!m_bOpen)
        {
            m_sprite_Checkmark.alpha = 1;
            m_sprite_open.alpha = 1;
        }
    }
    void onClick_SuitItem_Btn(GameObject caster)
    {
        if(m_callBack != null)
        {
       
            if (m_spr != null)
            {
                if (!m_bOpen)
                {
                    m_spr.alpha = 1;
                }
            }
            m_callBack(this.transform.name);
        }
    }


}
