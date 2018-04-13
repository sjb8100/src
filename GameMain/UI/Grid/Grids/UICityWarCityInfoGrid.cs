using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UICityWarCityInfoGrid : UIGridBase
{
    UITexture m_textureBg;

    UILabel m_lblName;

    UILabel m_lblTime;

    UILabel m_lblDay;

    GameObject m_goLock;

    private CMResAsynSeedData<CMTexture> m_iuiBgSeed = null;

    uint m_copyId;

    public uint CopyId
    {
        get
        {
            return m_copyId;
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iuiBgSeed)
        {
            m_iuiBgSeed.Release(true);
            m_iuiBgSeed = null;
        }
    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        Release();
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        m_textureBg = this.transform.Find("bgtexture").GetComponent<UITexture>();
        m_lblName = this.transform.Find("FB_name").GetComponent<UILabel>();
        m_lblTime = this.transform.Find("FB_status/time").GetComponent<UILabel>();
        m_goLock = this.transform.Find("FB_status/status_lock").gameObject;
        m_lblDay = this.transform.Find("FB_status/day").GetComponent<UILabel>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_copyId = (uint)data;
    }


    public void SetName(string name)
    {
        if (m_lblName != null)
        {
            m_lblName.text = name;
        }
    }

    public void SetBg(uint bgId)
    {
        if (m_textureBg != null)
        {
            UIManager.GetTextureAsyn(bgId, ref m_iuiBgSeed, () => { if (m_textureBg != null) { m_textureBg.mainTexture = null; } }, m_textureBg, false);
        }
    }

    public void SetDay(string day) 
    {
        if (m_lblDay != null)
        {
            m_lblDay.text = day;
        }
    }

    public void SetTime(string Time)
    {
        if (m_lblTime != null)
        {
            m_lblTime.text = Time;
        }
    }

}

