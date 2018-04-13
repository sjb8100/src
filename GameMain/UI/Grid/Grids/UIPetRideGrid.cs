//*************************************************************************
//	创建日期:	2017-4-5 14:59
//	文件名称:	uipetridegrid.cs
//  创 建 人:   	Chengxue.Zhao
//	版权所有:	中青宝
//	说    明:	查看别人宠物 坐骑列表
//*************************************************************************
using System;
using System.Collections.Generic;
using UnityEngine;

class UIPetRideGrid : UIGridBase
{
    private UITexture m_spriteIcon = null;
    private UILabel m_labelName = null;
    private UILabel m_labelLevel = null;
    private UISprite m_spriteBorader = null;
    private UISprite m_spriteSelect = null;

    private GameCmd.PetData m_data = null;
    public GameCmd.PetData PetData
    {
        get
        {
            return m_data;
        }
    }
    private RideData m_rideData = null;
    public RideData RideData
    {
        get { return m_rideData; }
    }
    protected override void OnAwake()
    {
 	    base.OnAwake();
        m_spriteIcon = CacheTransform.Find("icon").GetComponent<UITexture>();
        m_labelName = CacheTransform.Find("name").GetComponent<UILabel>();
        m_labelLevel = CacheTransform.Find("level").GetComponent<UILabel>();
        m_spriteBorader = CacheTransform.Find("frame").GetComponent<UISprite>();
        m_spriteSelect = CacheTransform.Find("select").GetComponent<UISprite>();
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        m_data = null;

        if (data is GameCmd.PetData)
        {
            m_data = (GameCmd.PetData)data;

            if (!string.IsNullOrEmpty(m_data.name))
            {
                SetName(m_data.name);
            }

            table.PetDataBase petdataDb = GameTableManager.Instance.GetTableItem<table.PetDataBase>(m_data.base_id);
            if (petdataDb != null)
            {
                UIManager.GetTextureAsyn(petdataDb.icon, ref m_curIconAsynSeed, () =>
                {
                    if (null != m_spriteIcon)
                    {
                        m_spriteIcon.mainTexture = null;
                    }
                }, m_spriteIcon);
                string borderName = ItemDefine.GetItemFangBorderIconByItemID(petdataDb.ItemID);
               SetFrame(borderName);
                if (string.IsNullOrEmpty(m_data.name))
                {
                    SetName(petdataDb.petName);
                }
            }

            SetLevel(m_data.lv.ToString());
        }else if (data is  RideData)
        {
            m_rideData = (RideData)data;
            UIManager.GetTextureAsyn(m_rideData.icon, ref m_curIconAsynSeed, () =>
            {
                if (null != m_spriteIcon)
                {
                    m_spriteIcon.mainTexture = null;
                }
            }, m_spriteIcon);
            SetFrame(m_rideData.QualityBorderIcon);
            SetName(m_rideData.name);
            SetLevel(DataManager.Manager<RideManager>().GetRideQualityStr(m_rideData.quality));
        }
    }

    CMResAsynSeedData<CMTexture> m_curIconAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_curIconAsynSeed_Bg = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_curIconAsynSeed != null)
        {
            m_curIconAsynSeed.Release(depthRelease);
            m_curIconAsynSeed = null;
        }
        if (m_curIconAsynSeed_Bg != null)
        {
            m_curIconAsynSeed_Bg.Release(depthRelease);
            m_curIconAsynSeed_Bg = null;
        }
    }

    void SetFrame(string textureName)
    {
        UIManager.GetAtlasAsyn(textureName, ref m_curIconAsynSeed_Bg, () =>
        {
            if (null != m_spriteBorader)
            {
                m_spriteBorader.atlas = null;
            }
        }, m_spriteBorader);
    }

    void SetName(string name)
    {
        if (m_labelName != null)
        {
            m_labelName.text = name;
        }
    }

    void SetLevel(string level)
    {
        if (m_labelLevel != null)
        {
            m_labelLevel.text = level;
        }
    }

    public override void SetHightLight(bool enable)
    {
        if (m_spriteSelect != null && m_spriteSelect.enabled != enable)
        {
            m_spriteSelect.enabled = enable;
        }
    }
}