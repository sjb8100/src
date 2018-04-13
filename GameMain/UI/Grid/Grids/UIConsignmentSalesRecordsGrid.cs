using UnityEngine;
using System.Collections;
using table;
using GameCmd;

public class UIConsignmentSalesRecordsGrid : UIGridBase
{
    private UITexture itemIcon;
    private UILabel itemName;
    private UILabel itemNum;
    private UILabel itemPrice;
    private UILabel remainTime;
    private UILabel totalPrice;
    ItemTradeLog itemLogData;
    UISprite HuoBiIcon1;//单价货币图标
    UISprite HuoBiIcon2;//总价货币图标

    private CMResAsynSeedData<CMAtlas> m_huobi1CASD = null;
    private CMResAsynSeedData<CMAtlas> m_huobi2CASD = null;
    private CMResAsynSeedData<CMTexture> m_iconCASD = null;


    protected override void OnAwake()
    {
        base.OnAwake();
        itemIcon = CacheTransform.Find("IconContent/Icon").GetComponent<UITexture>();
        itemName = CacheTransform.Find("Name").GetComponent<UILabel>();
        itemNum = CacheTransform.Find("IconContent/ItemNum").GetComponent<UILabel>();
        itemPrice = CacheTransform.Find("CurrencyContent/Num").GetComponent<UILabel>();
        remainTime = CacheTransform.Find("Time").GetComponent<UILabel>();
        totalPrice = CacheTransform.Find("TotalPriceContent/Num").GetComponent<UILabel>();
        HuoBiIcon1 = CacheTransform.Find("CurrencyContent/Icon").GetComponent<UISprite>();
        HuoBiIcon2 = CacheTransform.Find("TotalPriceContent/Icon").GetComponent<UISprite>();
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data == null)
        {
            itemLogData = null;
            return;
        }

        itemLogData = data as ItemTradeLog;
        ItemDataBase baseData = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemLogData.item_base_id);
        if (baseData != null)
        {
            UIManager.GetTextureAsyn(baseData.itemIcon, ref m_iconCASD, () =>
                {
                    if (null != itemIcon)
                    {
                        itemIcon.mainTexture = null;
                    }
                }, itemIcon);
            itemName.text = baseData.itemName;
            itemNum.text = itemLogData.item_num.ToString();

            uint saleMoney = 0;
            string iconName = "";
            if (itemLogData.gold > 0 )
            {
                CurrencyIconData Currdata = CurrencyIconData.GetCurrencyIconByMoneyType(ClientMoneyType.YinLiang);
                iconName = Currdata.smallIconName;
                saleMoney = itemLogData.gold;
            }
            else if(itemLogData.coin >0)
            {
                CurrencyIconData Currdata = CurrencyIconData.GetCurrencyIconByMoneyType(ClientMoneyType.YuanBao);
                saleMoney = itemLogData.coin;
                iconName = Currdata.smallIconName;
            }

            UIManager.GetAtlasAsyn(iconName, ref m_huobi1CASD, () =>
            {
                if (null != HuoBiIcon1)
                {
                    HuoBiIcon1.atlas = null;
                }
            }, HuoBiIcon1);

            UIManager.GetAtlasAsyn(iconName, ref m_huobi2CASD, () =>
            {
                if (null != HuoBiIcon2)
                {
                    HuoBiIcon2.atlas = null;
                }
            }, HuoBiIcon2);

            itemPrice.text = (saleMoney / itemLogData.item_num).ToString();
            totalPrice.text = saleMoney.ToString();
            remainTime.text = StringUtil.GetStringSince1970(itemLogData.sell_time);
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_iconCASD)
        {
            m_iconCASD.Release(true);
            m_iconCASD = null;
        }

        if (null != m_huobi1CASD)
        {
            m_huobi1CASD.Release(true);
            m_huobi1CASD = null;
        }

        if (null != m_huobi2CASD)
        {
            m_huobi2CASD.Release(true);
            m_huobi2CASD = null;
        }

        itemLogData = null;
    }
}
