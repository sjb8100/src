using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using UnityEngine;
using table;
using Engine.Utility;

class BuffUICountDown:MonoBehaviour,ITimer
{
    UISprite m_overLaySpr;
    UILabel m_labelLeft;
    uint m_nLeftTime;
    private readonly uint m_uBuffUITimerID = 1000;
    BuffDataBase m_db;
    CMResAsynSeedData<CMTexture> m_buffTextureSeed = null;
    UITexture m_texture;
    void Awake()
    {
        Transform overTrans = transform.Find("Overlay");
        if(overTrans != null)
        {
            m_overLaySpr = overTrans.GetComponent<UISprite>();
        }
        Transform labelTrans = transform.Find("Label");
        if(labelTrans != null)
        {
            m_labelLeft = labelTrans.GetComponent<UILabel>();
        }
        m_texture = this.transform.GetComponent<UITexture>();
       
    }

    public void OnDestroy()
    {
        if (TimerAxis.Instance().IsExist(m_uBuffUITimerID, this))
        {
            TimerAxis.Instance().KillTimer(m_uBuffUITimerID,this);
        }
    }

    public void OnDisable()
    {
        if (TimerAxis.Instance().IsExist(m_uBuffUITimerID, this))
        {
            TimerAxis.Instance().KillTimer(m_uBuffUITimerID,this);
        }
    }

    public void OnEnable()
    {
        TimerAxis.Instance().SetTimer(m_uBuffUITimerID, 1000, this);
    }
  
    public void InitLeftTime(uint leftTime,BuffDataBase db)
    {
        m_nLeftTime = leftTime/1000;
        if(db != null)
        {
            m_db = db;

            if (m_texture != null)
            {
                UIManager.GetTextureAsyn(db.buffIcon,
                       ref m_buffTextureSeed, () =>
                       {
                           if (null != m_texture)
                           {
                               m_texture.mainTexture = null;
                           }
                       }, m_texture, false);

            }
        }
    }
    void Update()
    {
        if(m_nLeftTime < 5&&m_nLeftTime >= 0)
        {
            m_overLaySpr.fillAmount = m_nLeftTime * 1.0f / 5;
        }
    }
    
    public void OnTimer(uint uTimerID)
    {
        if(m_nLeftTime > 0)
        {
            m_nLeftTime -= 1;
            if(m_nLeftTime < 5)
            {
                if(m_db != null)
                {
                    if(m_db.forever == 1)
                    {
                        m_labelLeft.text = "";
                    }
                    else
                    {
                        m_labelLeft.text = m_nLeftTime.ToString();
                    }
                }
            }
        }
    }
}

