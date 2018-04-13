using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIAccumulativeWeekGrid : UIGridBase
{
    UILabel m_lblName;

    UITexture m_icon;

    UILabel m_lblNum;

    UISprite m_spBorder;

    GameObject m_goBtn;

    GameObject m_goBtnCanNotGet;

    GameObject m_goBtnCanGet;

    GameObject m_goBtnAlreadyGet;

    GameObject m_goSelect;

    GameObject m_goItem;

    public uint Id;

    private CMResAsynSeedData<CMTexture> iuiIconAtlas = null;

    private CMResAsynSeedData<CMAtlas> iuiBorderAtlas = null;

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != iuiIconAtlas)
        {
            iuiIconAtlas.Release(true);
            iuiIconAtlas = null;
        }

        if (null != iuiBorderAtlas)
        {
            iuiBorderAtlas.Release(true);
            iuiBorderAtlas = null;
        }

        if (m_goSelect != null)
        {
            UIParticleWidget p = m_goSelect.GetComponent<UIParticleWidget>();
            if (p != null)
            {
                p.ReleaseParticle();
            }
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        m_lblName = this.transform.Find("Title").GetComponent<UILabel>();

        m_icon = this.transform.Find("Item/Content/Icon").GetComponent<UITexture>();

        m_lblNum = this.transform.Find("Item/Content/Num").GetComponent<UILabel>();

        m_spBorder = this.transform.Find("Item/Content/Border").GetComponent<UISprite>();

        m_goItem = this.transform.Find("Item").gameObject;

        m_goBtnCanNotGet = this.transform.Find("Btn/CanNotGet").gameObject;

        m_goBtnCanGet = this.transform.Find("Btn/CanGet").gameObject;

        m_goBtnAlreadyGet = this.transform.Find("Btn/AlreadyGet").gameObject;

        m_goSelect = this.transform.Find("Btn/CanGet/Effect").gameObject;

        m_goBtn = this.transform.Find("Btn").gameObject;

        UIEventListener.Get(m_goItem).onClick = ItemClick;
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

    public void SetIcon(string iconName)
    {
        if (m_icon != null)
        {
            UIManager.GetTextureAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(false, iconName, false), ref iuiIconAtlas, () =>
            {
                if (null != m_icon)
                {
                    m_icon.mainTexture = null;
                }
            }, m_icon, false);
        }
    }

    public void SetBorder(string iconName)
    {
        if (m_spBorder != null)
        {
            UIManager.GetAtlasAsyn(DataManager.Manager<UIManager>().GetResIDByFileName(true, iconName, false)
                   , ref iuiBorderAtlas
                   , () =>
                   {
                       if (null != m_spBorder)
                       {
                           m_spBorder.atlas = null;
                       }
                   }, m_spBorder
                   , UIManager.GetIconName(iconName, false)
                   , false);
        }
    }

    public void SetNum(uint num)
    {
        if (m_lblNum != null)
        {
            m_lblNum.text = num.ToString();
        }
    }

    public void SetBtnSelect(int state)
    {
        if (m_goBtnCanNotGet != null && m_goBtnCanGet != null && m_goBtnAlreadyGet != null)
        {
            if (state == 1)
            {
                m_goBtnCanNotGet.SetActive(true);
                m_goBtnCanGet.SetActive(false);
                m_goBtnAlreadyGet.SetActive(false);
                if (m_goBtn != null)
                {
                    UIEventListener.Get(m_goBtn).onClick = null;
                }
            }
            else if (state == 2)
            {
                m_goBtnCanNotGet.SetActive(false);
                m_goBtnCanGet.SetActive(true);
                m_goBtnAlreadyGet.SetActive(false);
                if (m_goBtn != null)
                {
                    UIEventListener.Get(m_goBtn).onClick = BtnClick;
                }

                RefreshSigne();
            }
            else if (state == 3)
            {
                m_goBtnCanNotGet.SetActive(false);
                m_goBtnCanGet.SetActive(false);
                m_goBtnAlreadyGet.SetActive(true);
                if (m_goBtn != null)
                {
                    UIEventListener.Get(m_goBtn).onClick = null;
                }
            }

        }
    }

    public void RefreshSigne()
    {
        if (m_goSelect != null)
        {         
            m_goSelect.SetActive(true);
            UIParticleWidget p = m_goSelect.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = m_goSelect.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 20;
            }
            if (p != null)
            {
                p.SetDimensions(150 , 50);
                p.ReleaseParticle();
                p.AddRoundParticle();
            }
        }
    }

    void BtnClick(GameObject go)
    {
        int btnIndex = 1;
        object param = btnIndex;
        InvokeUIDlg(UIEventType.Click, this, param);
    }

    void ItemClick(GameObject go)
    {
        int btnIndex = 2;
        object param = btnIndex;
        InvokeUIDlg(UIEventType.Click, this, param);
    }

}

