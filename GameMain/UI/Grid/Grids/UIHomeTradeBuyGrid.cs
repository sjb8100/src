using Client;
using Engine.Utility;
//*******************************************************************************************
//	创建日期：	2016-9-27   17:04
//	文件名称：	UIHomeTradeBuyGrid,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	家园买入物品grid
//*******************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIHomeTradeBuyGrid : UIGridBase
{
    //价格
    private Transform costContent;
    private UILabel cost;
    private UISprite costIcon;

    //标签
    private Transform tagContent;
    private UISprite tagBg;
    private UILabel tagDes;

    //格子背景
    private UISprite bg;

    //名称
    private UILabel name;
    //数量
    private UILabel num;
    //图标
    private UITexture icon;

    //折扣
    private Transform discountContent;
    private UILabel m_lab_discount;

    //商品id  不是物品ID
    private uint mallItemId;
    public uint MallItemId
    {
        get
        {
            return mallItemId;
        }
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        num = CacheTransform.Find("Content/Num").GetComponent<UILabel>();
        icon = CacheTransform.Find("Content/IconContent/Icon").GetComponent<UITexture>();
        bg = CacheTransform.Find("Content/Bg").GetComponent<UISprite>();

        costContent = CacheTransform.Find("Content/Cost");
        cost = CacheTransform.Find("Content/Cost/Cost").GetComponent<UILabel>();
        costIcon = CacheTransform.Find("Content/Cost/CostIcon").GetComponent<UISprite>();

        tagContent = CacheTransform.Find("Content/Tag");
        tagDes = CacheTransform.Find("Content/Tag/Label").GetComponent<UILabel>();
        tagBg = CacheTransform.Find("Content/Tag/Bg").GetComponent<UISprite>();
        m_lab_discount = CacheTransform.Find("Content/Tag/Discount").GetComponent<UILabel>();
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data)
            return;
        this.mallItemId = (uint)data;
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        string icon = (hightLight) ? UIDefine.GRID_BG_HIGHTLIGHT : UIDefine.GRID_BG_DARK;
        UIManager.GetAtlasAsyn(icon, ref m_BgaCASD, () =>
        {
            if (null != bg)
            {
                bg.atlas = null;
            }
        }, bg);
       
    }
    CMResAsynSeedData<CMTexture> m_iconCASD = null;
    CMResAsynSeedData<CMAtlas> m_BgaCASD = null;
    CMResAsynSeedData<CMAtlas> m_currencyCASD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);

        if (m_iconCASD != null)
        {
            m_iconCASD.Release(depthRelease);
            m_iconCASD = null;
        }

        if (m_BgaCASD != null)
        {
            m_BgaCASD.Release(depthRelease);
            m_BgaCASD = null;
        }

        if (m_currencyCASD != null)
        {
            m_currencyCASD.Release(depthRelease);
            m_currencyCASD = null;
        }
    }

    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
        Release();
    }

    /// <summary>
    /// 名称
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (null != this.name)
            this.name.text = name;
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="num"></param>
    public void SetNum(uint num)
    {
        if (null != this.num)
            this.num.text = (num > 1) ? ("" + num) : "";
    }

    /// <summary>
    /// 图标
    /// </summary>
    /// <param name="iconName"></param>
    public void SetIcon(string iconName)
    {
        UIManager.GetTextureAsyn(iconName, ref m_iconCASD, () =>
        {
            if (null != icon)
            {
                icon.mainTexture = null;
            }
        }, icon);
       
    }

    /// <summary>
    /// 价格
    /// </summary>
    /// <param name="moneyType"></param>
    /// <param name="price"></param>
    public void SetPrice(GameCmd.MoneyType moneyType, uint price)
    {
        if (null != this.costIcon && null != this.cost)
        {
            string currencyIcon = MallDefine.GetCurrencyIconNameByType(moneyType);
            UIManager.GetAtlasAsyn(currencyIcon, ref m_BgaCASD, () =>
            {
                if (null != costIcon)
                {
                    costIcon.atlas = null;
                }
            }, costIcon);
        
            this.cost.text = "" + price;
        }
    }

    ///// <summary>
    ///// 设置选中
    ///// </summary>
    ///// <param name="select"></param>
    //public void SetSelect(bool select)
    //{
    //    if (null != bg)
    //    {
    //        bg.spriteName = (select) ? "button_green_liang" : "button_green_an";
    //    }
    //}

    /// <summary>
    /// 设置标签
    /// </summary>
    /// <param name="tagType">标签类型</param>
    /// <param name="des">描述</param>
    public void SetTag(MallDefine.MallTagType tagType, string des)
    {
        bool tagVisble = (tagType != MallDefine.MallTagType.None) ? true : false;

        if (null != tagContent && tagContent.gameObject.activeSelf != tagVisble)
            tagContent.gameObject.SetActive(tagVisble);
        if (tagType != MallDefine.MallTagType.None)
        {
            string tagBgName = "";
            switch (tagType)
            {
                case MallDefine.MallTagType.Discount:
                    tagBgName = "tag_bg_yellow";
                    break;
                case MallDefine.MallTagType.Hot:
                    tagBgName = "tag_bg_red";
                    break;
                case MallDefine.MallTagType.VipNeed:
                    tagBgName = "tag_bg_purple";
                    break;
            }

            if (null != this.tagBg)
                this.tagBg.spriteName = tagBgName;
            if (null != this.tagDes)
                this.tagDes.text = des;
        }
    }

}

