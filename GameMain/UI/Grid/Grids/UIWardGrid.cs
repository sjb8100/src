//*************************************************************************
//	创建日期:	2016/10/20 11:14:33
//	文件名称:	UIWardGrid
//   创 建 人:   zhuidanyu	
//	版权所有:	中青宝
//	说    明:	副本界面掉落icon 
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class UIWardGrid : UIGridBase
{
    uint itemID;
    public uint ItemID
    {
        set
        {
            itemID = value;
            SetQua();
            SetIcon();
        }
        get
        {
            return itemID;
        }
    }
    //图标
    private UITexture icon;
    private UISprite m_sprQua;
    protected override void OnAwake()
    {
        icon = transform.Find("icon").GetComponent<UITexture>();
        m_sprQua = transform.Find("qua").GetComponent<UISprite>();
    }
    void SetQua()
    {
        table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemID);

        if (itemdb != null)
        {
            if(m_sprQua == null)
            {
                m_sprQua = transform.Find("qua").GetComponent<UISprite>();
            }
            if(m_sprQua != null)
            {
                UIManager.GetQualityAtlasAsyn(itemdb.quality, ref m_curQualityAsynSeed, () =>
                {
                    if (null != m_sprQua)
                    {
                        m_sprQua.atlas = null;
                    }
                }, m_sprQua);
            }
        }
    }
    /// <summary>
    /// 图标
    /// </summary>
    /// <param name="iconName"></param>
    void SetIcon()
    {
        gameObject.SetActive(true);
        table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemID);

        if (itemdb != null)
        {
            if (null != this.icon)
            {
          
                UIManager.GetTextureAsyn(itemdb.itemIcon, ref m_curIconAsynSeed, () =>
                {
                    if (null != icon)
                    {
                        icon.mainTexture = null;
                    }
                }, icon,false);
            }
        }
    }
    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_curQualityAsynSeed = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
        if (m_curQualityAsynSeed != null)
        {
            m_curQualityAsynSeed.Release(depthRelease);
            m_curQualityAsynSeed = null;
        }
    }
}

