//*******************************************************************************************
//	创建日期：	2016-9-27   17:04
//	文件名称：	UIHomeTradeSellGrid,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	家园卖出物品grid
//*******************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class UIHomeTradeSellGrid : UIGridBase
{
    UISprite m_spBg;

    UITexture m_spIcon;

    UILabel m_lblCount;

    UILabel m_lblName;

    UILabel m_lblPrice;

    UISprite m_spMoneyIcon;

    UILabel m_lblChangeNum;

    GameObject m_goAdd;

    GameObject m_goLess;

    //商品id
    private uint mallItemId;
    public uint MallItemId
    {
        get
        {
            return mallItemId;
        }
    }
    private CMResAsynSeedData<CMTexture> iuiBorderAtlas = null;
    private CMResAsynSeedData<CMAtlas> iuiCurrencyCSAD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != iuiBorderAtlas)
        {
            iuiBorderAtlas.Release(true);
            iuiBorderAtlas = null;
        }

        if (null != iuiCurrencyCSAD)
        {
            iuiCurrencyCSAD.Release(true);
            iuiCurrencyCSAD = null;
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

        m_spBg = this.transform.Find("bg").GetComponent<UISprite>();
        m_spIcon = this.transform.Find("icon").GetComponent<UITexture>();
        m_lblCount = this.transform.Find("icon/Num").GetComponent<UILabel>();
        m_lblName = this.transform.Find("name").GetComponent<UILabel>();
        m_lblPrice = this.transform.Find("price").GetComponent<UILabel>();
        m_spMoneyIcon = this.transform.Find("price/Sprite").GetComponent<UISprite>();
        m_lblChangeNum = this.transform.Find("change_Num").GetComponent<UILabel>();
        m_goAdd = this.transform.Find("change_Num/add").gameObject;
        m_goLess = this.transform.Find("change_Num/less").gameObject;
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
        if (null != m_spBg)
        {
            /*string icon = (hightLight) ? UIDefine.GRID_BG_HIGHTLIGHT : UIDefine.GRID_BG_DARK;
            UIAtlas atlas = DataManager.Manager<UIManager>().GetAtlasByIconName(DynamicAtlasType.DTAcommon, icon);
            if (null != atlas)
            {
                m_spBg.atlas = atlas;
            }
            m_spBg.spriteName = icon;*/
        }
    }

    /// <summary>
    /// 名称
    /// </summary>
    /// <param name="name"></param>
    public void SetName(string name)
    {
        if (null != this.name)
            this.m_lblName.text = name;
    }

    /// <summary>
    /// 设置数量
    /// </summary>
    /// <param name="num"></param>
    public void SetNum(uint num)
    {
        if (null != this.m_lblCount)
            this.m_lblCount.text = (num > 1) ? ("" + num) : "";
    }

    /// <summary>
    /// 图标
    /// </summary>
    /// <param name="iconName"></param>
    public void SetIcon(string iconName)
    {
        UIManager.GetTextureAsyn(iconName, ref iuiBorderAtlas, () =>
        {
            if (null != m_spIcon)
            {
                m_spIcon.mainTexture = null;
            }
        }, m_spIcon);
       
    }

    public void SetPrice(GameCmd.MoneyType moneyType, uint price, float changeRate, bool up)
    {
        if (null != this.m_spMoneyIcon && null != this.m_lblPrice)
        {
            string currencyIcon =  MallDefine.GetCurrencyIconNameByType(moneyType);
            UIManager.GetAtlasAsyn(currencyIcon, ref iuiCurrencyCSAD, () =>
            {
                if (null != m_spMoneyIcon)
                {
                    m_spMoneyIcon.atlas = null;
                }
            }, m_spMoneyIcon);
            this.m_lblPrice.text = "" + price;
        }

        m_lblChangeNum.gameObject.SetActive(true);
        m_lblChangeNum.text = changeRate.ToString("P");//价格变化率
        if(up)
        {
            m_goAdd.SetActive(true);
            m_goLess.SetActive(false);
        }
        else
        {
            m_goLess.SetActive(true);
            m_goAdd.SetActive(false);
        }     
    }

    /// <summary>
    /// 设置选中
    /// </summary>
    /// <param name="select"></param>
    //public void SetSelect(bool select)
    //{
    //    if (null != m_spBg)
    //    {
    //        m_spBg.spriteName = (select) ? "button_green_liang" : "button_green_an";
    //    }
    //}

    /// <summary>
    /// 设置标签
    /// </summary>
    /// <param name="des">描述</param>
    public void SetTag(string des)
    {
    }
}

