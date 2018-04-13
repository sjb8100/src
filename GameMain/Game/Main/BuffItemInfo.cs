using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using table;
using Engine.Utility;
partial class BuffItemInfo : UIGridBase,ITimer
{
    long m_nLeftTime = 0;
    BuffDataBase m_db = null;
    private readonly uint m_uBuffItemTimerID = 100;
    public void InitBuffItemInfo(BuffDataBase db,long leftTime)
    {
        if(db == null)
        {
            return;
        }
        m_db = db;
    
        m_nLeftTime = leftTime;

        InitUI();
     
    }
    protected override void OnStart()
    {
        base.OnStart();
        TimerAxis.Instance().SetTimer(m_uBuffItemTimerID, 1000, this);
       // InitControls();
        InitUI();
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(true);
            m_curIconAsynSeed = null;
        }
    }
    void InitUI()
    {
        if (m_db == null)
        {
            return;
        }
        if(m__BuffIcon != null)
        {
            UIManager.GetTextureAsyn(m_db.buffIcon, ref m_curIconAsynSeed, () =>
            {
                if (null != m__BuffIcon)
                {
                    m__BuffIcon.mainTexture = null;
                }
            }, m__BuffIcon);
        }
        if (m_label_BuffName != null)
        {
            m_label_BuffName.text = m_db.strName;
        }
        if (m_label_BuffDes != null)
        {
            m_label_BuffDes.text = m_db.strDesc;
        }
        if (m_label_BuffTime != null)
        {
            if (m_db.forever == 1)
            {
                m_label_BuffTime.text = CommonData.GetLocalString("永久");
            }
            else
            {
                if(m_nLeftTime < 0)
                {
                    m_nLeftTime = 0;
                }
                uint left = (uint)m_nLeftTime / 1000;
                m_label_BuffTime.text = StringUtil.GetAdjustStringBySeconds(left);
            }
        }
       
    }
    void OnEnable()
    {
        InitUI();
    }
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == m_uBuffItemTimerID)
        {
            if(m_nLeftTime < 1000)
            {
                return;
            }
            m_nLeftTime -= 1000;
            uint left = (uint)m_nLeftTime / 1000;
            if(left <= 0)
            {
                left = 0;
            }
            if (m_label_BuffTime != null)
            {
                if(m_db.forever != 1)
                {
                    m_label_BuffTime.text = StringUtil.GetAdjustStringBySeconds(left);
                }
            }
        
        }
    }

 
    public void OnDestroy()
    {
        TimerAxis.Instance().KillTimer(m_uBuffItemTimerID, this);
    }

    
        
}

