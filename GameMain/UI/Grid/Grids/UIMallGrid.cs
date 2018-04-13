using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIMallGrid : UIItemInfoGridBase
{
    #region property
    //价格
    private Transform costContent;
    private UILabel cost;
    private UISprite costIcon;

    //标签
    private Transform tagContent;
    private Transform mtsTagYellow;
    private Transform mtsTagPurple;
    private Transform mtsTagRed;
    private UILabel tagDes;

    //discount 
    private UILabel m_lab_discount;

    //名称
    private UILabel name;
    private UIToggle m_tg_hightlight;
    //商品id
    private uint mallItemId;
    public uint MallItemId
    {
        get
        {
            return mallItemId;
        }
    }
    Color defaultColor = Color.white;
    #endregion

   
    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        name.color = ColorManager.GetColor32OfType(ColorType.White);
        m_tg_hightlight = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
       
        costContent = CacheTransform.Find("Content/Cost");
        cost = CacheTransform.Find("Content/Cost/Cost").GetComponent<UILabel>();
        defaultColor = ColorManager.GetColor32OfType(ColorType.JZRY_Txt_Black);
        costIcon = CacheTransform.Find("Content/Cost/CostIcon").GetComponent<UISprite>();

        tagContent = CacheTransform.Find("Content/Tag");
        tagDes = CacheTransform.Find("Content/Tag/Label").GetComponent<UILabel>();
        mtsTagYellow = CacheTransform.Find("Content/Tag/Bg_huang");
        mtsTagPurple = CacheTransform.Find("Content/Tag/Bg_Purple");
        mtsTagRed = CacheTransform.Find("Content/Tag/Bg_hong");
        m_lab_discount = CacheTransform.Find("Content/Tag/Discount").GetComponent<UILabel>();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
        //SetTriggerEffect(true, (int)UIBase.UITriggerEffectType.Scale);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        ResetInfoGrid();
        if (null == data)
            return;
        this.mallItemId = (uint)data;
        MallDefine.MallLocalData localData = DataManager.Manager<MallManager>().GetMallLocalDataByMallItemId(this.mallItemId);
        if (null == localData)
        {
            Engine.Utility.Log.Error("UIMallGrid SetGridData faield,mall data errir mall:{0}", this.mallItemId);
            return;
        }
        SetIcon(true, localData.LocalItem.Icon);
        SetName(localData.LocalItem.Name);
        bool enable = (localData.LocalMall.pileNum > 1) ? true : false;
        SetNum(enable, localData.LocalMall.pileNum.ToString());
        SetBorder(true,localData.LocalItem.BorderIcon);
        SetTag(localData);

        if (localData.LocalItem.IsMuhon)
        {
            SetMuhonMask(true, Muhon.GetMuhonStarLevel(localData.LocalItem.BaseId));
            SetMuhonLv(true, Muhon.GetMuhonLv(localData.LocalItem));
        }
        else if (localData.LocalItem.IsRuneStone)
        {
            SetRuneStoneMask(true, (uint)localData.LocalItem.Grade);
        }else if (localData.LocalItem.IsChips)
        {
            SetChips(true);
        }

    }


    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != m_tg_hightlight && m_tg_hightlight.value != hightLight)
        {
            m_tg_hightlight.value = hightLight;
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
        if (null != m_priceAsynSeed)
        {
            m_priceAsynSeed.Release(true);
            m_priceAsynSeed = null;
        }
    }
    #endregion


    #region Set

    private void SetName(string name)
    {
        if (null != this.name)
        {
            this.name.text = name;
        }
    }

    CMResAsynSeedData<CMAtlas> m_priceAsynSeed = null;
    /// <summary>
    /// 价格
    /// </summary>
    /// <param name="moneyType"></param>
    /// <param name="price"></param>
    private void SetPrice(GameCmd.MoneyType moneyType, string priceTxt)
    {
        if (null != this.costIcon && null != this.cost)
        {
            string currencyIcon = MallDefine.GetCurrencyIconNameByType(moneyType);
            UIManager.GetAtlasAsyn(currencyIcon, ref m_priceAsynSeed, () =>
                    {
                        if (null != costIcon)
                        {
                            costIcon.atlas = null;
                        }
                    }, costIcon);
            this.cost.text = priceTxt;
        }
    }

    /// <summary>
    /// 设置标签
    /// </summary>
    /// <param name="tagTypeData">标签类型</param>
    private void SetTag(MallDefine.MallLocalData localData)
    {
        MallDefine.MallTagTypeData tagTypeData = localData.Tag;
        bool tagVisble = (tagTypeData.Tag != MallDefine.MallTagType.None) ? true : false;
        if (null != tagContent && tagContent.gameObject.activeSelf != tagVisble)
            tagContent.gameObject.SetActive(tagVisble);
        string pricetxt = localData.LocalMall.buyPrice.ToString();

        cost.color = defaultColor;
        if (tagVisble)
        {
            tagVisble = (tagTypeData.Tag == MallDefine.MallTagType.Discount);
            if (m_lab_discount.gameObject.activeSelf != tagVisble)
            {
                m_lab_discount.gameObject.SetActive(tagVisble);
            }
            string tagName = "";
            switch (tagTypeData.Tag)
            {
                case MallDefine.MallTagType.Discount:
                    {
                        tagName = "打折";
                        m_lab_discount.text = localData.LocalMall.offPrice.ToString();
                        pricetxt = MallDefine.GetDiscountString(localData.LocalMall.buyPrice.ToString());
                        cost.color = ColorManager.GetColor32OfType(ColorType.JZRY_Txt_White);
                    }
                    break;
                case MallDefine.MallTagType.Hot:
                    tagName = "热卖";
                    break;
                case MallDefine.MallTagType.VipNeed:
                    tagName = string.Format("VIP{0}", tagTypeData.Value);
                    break;
                case MallDefine.MallTagType.CharacterLv:
                    tagName = string.Format("Lv.{0}", tagTypeData.Value);
                    break;
            }
            bool visibleTagBg = (tagTypeData.Tag == MallDefine.MallTagType.VipNeed);
            if (null != mtsTagPurple && mtsTagPurple.gameObject.activeSelf != visibleTagBg)
            {
                mtsTagPurple.gameObject.SetActive(visibleTagBg);
            }

            visibleTagBg = (tagTypeData.Tag == MallDefine.MallTagType.CharacterLv
                || tagTypeData.Tag == MallDefine.MallTagType.Hot);
            if (null != mtsTagRed && mtsTagRed.gameObject.activeSelf != visibleTagBg)
            {
                mtsTagRed.gameObject.SetActive(visibleTagBg);
            }

            visibleTagBg = (tagTypeData.Tag == MallDefine.MallTagType.Discount);
            if (null != mtsTagYellow && mtsTagYellow.gameObject.activeSelf != visibleTagBg)
            {
                mtsTagYellow.gameObject.SetActive(visibleTagBg);
            }
            
            if (null != this.tagDes)
                this.tagDes.text = tagName;
        }
        SetPrice((GameCmd.MoneyType)localData.LocalMall.moneyType, pricetxt);
    }

    #endregion 

    #region UIEventCallBack
    protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch(eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemInfoGrid && mallItemId != 0)
                    {
                        MallDefine.MallLocalData malldata = MallDefine.MallLocalData.Create(mallItemId);
                        if (null != malldata && null != malldata.LocalItem)
                        {
                            base.InfoGridUIEventDlg(eventType, this, param);
                            TipsManager.Instance.ShowItemTips(malldata.LocalItem);
                        }

                    }
                }
                break;
        }
    }
    #endregion
}

