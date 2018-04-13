using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIBlackMarketGrid : UIItemInfoGridBase
{
    #region property
    //价格
    private Transform costContent;
    private UILabel cost;
    private UISprite costIcon;

    //售罄tag
    private UISprite soldOutTag;
    //limit
    private UILabel limit;

    //名称
    private UILabel m_labName;

    //商品id
    private GameCmd.DynaStorePosInfo mallInfo;
    public GameCmd.DynaStorePosInfo MallInfo
    {
        get
        {
            return mallInfo;
        }
    }
    private GameObject purchaseBtn;
    #endregion
    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_labName = CacheTransform.Find("Content/Name").GetComponent<UILabel>();

        m_labName.color = ColorManager.GetColor32OfType(ColorType.White);
        cost = CacheTransform.Find("Content/PurchaseBtn/Cost/Content/Num").GetComponent<UILabel>();
        costIcon = CacheTransform.Find("Content/PurchaseBtn/Cost/Content/Icon").GetComponent<UISprite>();

        soldOutTag = CacheTransform.Find("Content/SoldMask").GetComponent<UISprite>();
        limit = CacheTransform.Find("Content/Limit").GetComponent<UILabel>();

        purchaseBtn = CacheTransform.Find("Content/PurchaseBtn").gameObject;
        if (null != purchaseBtn)
        {
            UIEventListener.Get(purchaseBtn).onClick = (obj) =>
                {
                    OnPurchase();
                };
        }

        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    private CMResAsynSeedData<CMAtlas> m_costCASD = null;
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        ResetInfoGrid();
        if (null == data)
            return;
        this.mallInfo = (GameCmd.DynaStorePosInfo)data;
        MallDefine.MallLocalData localData = DataManager.Manager<MallManager>().GetMallLocalDataByMallItemId(mallInfo.baseid);
        if (null == localData)
        {
            Debug.LogError(string.Format("UIBlackMarketGrid SetGridData faield,localData errir baseId:{0}", mallInfo.baseid));
            return;
        }

        MallDefine.PurchaseLimitData limitData = null;
        string limitDes = "";
        if (DataManager.Manager<MallManager>().TryGetMallItemPurchaseLimitType(localData.MallId, out limitData,packageFullCheck:false)
            && (limitData.limitType == MallDefine.PurchaseLimitType.CharacterLv
            || limitData.limitType == MallDefine.PurchaseLimitType.Vip))
        {
            limitDes = limitData.limitDes;
        }
        if (null != m_labName)
        {
            m_labName.text = localData.LocalItem.Name;
        }

        //bool fightPowerUp = false;
        //if (localData.LocalItem.IsEquip
        //    && DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(localData.LocalItem.BaseId, out fightPowerUp))
        //{
        //    SetFightPower(true, fightPowerUp);
        //}

        SetBorder(true, localData.LocalItem.BorderIcon);
        SetIcon(true, localData.LocalItem.Icon);
        bool enable = (localData.LocalMall.pileNum > 1) ? true : false;
        SetNum(enable, localData.LocalMall.pileNum.ToString());
        SetLimit(limitDes);
        SetSoldOut((mallInfo.buy_flag == 1) ? true : false);
        if (null != this.cost && null != this.costIcon)
        {
            MallDefine.CurrencyData cost 
                = new MallDefine.CurrencyData((GameCmd.MoneyType)localData.LocalMall.moneyType, localData.LocalMall.buyPrice);
            string costIconName = (null != cost) ? MallDefine.GetCurrencyIconNameByType(cost.CType) : "";
            UIManager.GetAtlasAsyn(costIconName, ref m_costCASD, () =>
                {
                    if (null != this.costIcon)
                    {
                        this.costIcon.atlas = null;
                    }
                }, this.costIcon);
            string costString = (null != cost) ? localData.LocalMall.buyPrice.ToString() : "0";
            this.cost.text = costString;
        }

        //SetFightUp(false);
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

    #endregion

    #region Set

    private void SetLimit(string limit)
    {
        if (null != this.limit)
        {
            this.limit.text = (!string.IsNullOrEmpty(limit)) ? ColorManager.GetColorString(ColorType.Red, limit) : "";
        }
    }

    private void SetSoldOut(bool soldOut)
    {
        if (null != this.soldOutTag && this.soldOutTag.gameObject.activeSelf != soldOut)
        {
            this.soldOutTag.gameObject.SetActive(soldOut);
            if (soldOut && !this.soldOutTag.gameObject.activeSelf)
            {
                this.soldOutTag.gameObject.SetActive(true);
            }
        }
    }

    private void OnPurchase()
    {
        InvokeUIDlg(UIEventType.Click, this, mallInfo);
    }



    #endregion

    #region Release
    
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }

        if (null != m_costCASD)
        {
            m_costCASD.Release(true);
            m_costCASD = null;
        }
        mallInfo = null;
    }
    #endregion
}