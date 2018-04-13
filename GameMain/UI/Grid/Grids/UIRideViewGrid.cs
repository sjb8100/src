using System;
using System.Collections.Generic;
using UnityEngine;

class UIRideViewGrid :UIGridBase
{
    UILabel lableName;
    UILabel lableRate;
    UILabel lableSpeed;
    UITexture icon;
    UISprite border;
    UISprite m_spriteGetTip = null;
    table.RideDataBase m_database;
    public table.RideDataBase Data { get { return m_database; } }

    protected override void OnAwake()
    {
        base.OnAwake();
     
        lableName = transform.Find("tujian_name").GetComponent<UILabel>();
        lableRate = transform.Find("rarity").GetComponent<UILabel>();
        lableSpeed = transform.Find("speed").GetComponent<UILabel>();
        icon = transform.Find("icon").GetComponent<UITexture>();
        border = transform.Find("IconBg").GetComponent<UISprite>();
        m_spriteGetTip = transform.Find("nohave").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);

        if (data is table.RideDataBase)
        {
            m_database = (table.RideDataBase)data;

            if (lableName != null)
            {
                lableName.text = m_database.name;
            }

            if (lableRate != null)
            {
                lableRate.text = DataManager.Manager<RideManager>().GetRideQualityStr(m_database.quality);
            }

            if (lableSpeed != null)
            {
                table.RideFeedData feeddata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(m_database.rideID, 0);
                if (feeddata != null)
                {
                    float value = (feeddata.speed / 100.0f);
                    if (lableSpeed != null) lableSpeed.text = value.ToString() + "%";
                }
            }

            if (icon != null)
            {
                UIManager.GetTextureAsyn(m_database.icon
                    , ref iuiIconAtlas, () =>
                {
                    if (null != icon)
                    {
                        icon.mainTexture = null;
                    }
                }, icon, true);
            }

            if (border != null)
            {
                string borderIconName = ItemDefine.GetItemBorderIcon(m_database.quality);
                UIManager.GetAtlasAsyn(borderIconName
                    , ref iuiBorderAtlas, () =>
                    {
                        if (null != border)
                        {
                            border.atlas = null;
                        }
                    }, border, true);
            }

            if (m_spriteGetTip != null)
            {
                m_spriteGetTip.enabled = !DataManager.Manager<RideManager>().ContainRide(m_database.rideID);
            }
        }

    }

    #region AssetRelease
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
    }
    #endregion
}