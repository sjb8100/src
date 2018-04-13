using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIDailyTestGrid : UIGridBase
{
    UITexture m_bgTexture;
    UILabel m_lblTitleName;
    UILabel m_lblExp;
    UILabel m_lblLv;
    Transform m_goRecommendMark;

    uint m_Id;
    public uint Id
    {
        get
        {
            return m_Id;
        }
    }

    CMResAsynSeedData<CMTexture> m_iconSeedData = null;

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconSeedData)
        {
            m_iconSeedData.Release(true);
            m_iconSeedData = null;
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
        m_bgTexture = this.transform.Find("bg").GetComponent<UITexture>();
        m_lblTitleName = this.transform.Find("title").GetComponent<UILabel>();
        m_lblExp = this.transform.Find("bottom_label").GetComponent<UILabel>();
        m_lblLv = this.transform.Find("bottom_label2").GetComponent<UILabel>();
        m_goRecommendMark = this.transform.Find("recommend");
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        this.m_Id = (uint)data;
    }

    public void SetBg(uint bgId)
    {
        UIManager.GetTextureAsyn(bgId, ref m_iconSeedData, () =>
        {
            if (m_bgTexture != null)
            {
                m_bgTexture.mainTexture = null;
            }
        }, m_bgTexture);
    }

    public void SetTitleName(string titleName)
    {
        if (m_lblTitleName != null)
        {
            m_lblTitleName.text = titleName;
        }
    }

    public void SetExpDesc(string expDesc)
    {
        if (m_lblExp != null)
        {
            m_lblExp.text = expDesc;
        }
    }

    public void SetLvDesc(string lvDesc)
    {
        if (m_lblLv != null)
        {
            m_lblLv.text = lvDesc;
        }
    }

    public void SetMark(bool b)
    {
        if (m_goRecommendMark != null)
        {
            m_goRecommendMark.gameObject.SetActive(b);
        }
    }

}

