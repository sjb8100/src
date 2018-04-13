using UnityEngine;
using System.Collections;
using GameCmd;
using table;
using System;

public class UIConsignmentSellListGrid : UIItemInfoGridBase
{
    private UILabel itemName;
    private UILabel itemPrice;
    private UILabel remainTime;
    private Transform currecyContent;
    private UISprite yuanbaoicon;
    private UISprite jinbiicon;
    private ItemPageInfo itemData = null;
    private ItemSellTimeInfo timeInfo = null;

    private UILabel gongShiLabel = null;


    protected override void OnAwake()
    {
        base.OnAwake();
        itemName = CacheTransform.Find("Name").GetComponent<UILabel>();
        itemPrice = CacheTransform.Find("CurrencyContent/Num").GetComponent<UILabel>();
        currecyContent = CacheTransform.Find("CurrencyContent");
        yuanbaoicon = CacheTransform.Find("CurrencyContent/YuanBaoIcon").GetComponent<UISprite>();
        jinbiicon = CacheTransform.Find("CurrencyContent/JinBiIcon").GetComponent<UISprite>();
        remainTime = CacheTransform.Find("Time").GetComponent<UILabel>();
        gongShiLabel = CacheTransform.Find("IsGongShi").GetComponent<UILabel>();
        InitItemInfoGrid(CacheTransform.Find("InfoGridRoot/InfoGrid"), false);
        GameObject btnBuy = CacheTransform.Find("Btn").gameObject;
        UIEventListener.Get(btnBuy).onClick = OnClickGetBackBtn;
    }

    public void UpdateItemInfo(ItemPageInfo itemData, ItemSellTimeInfo timeInfo, ItemSerialize data = null)
    {
        this.itemData = itemData;
        this.timeInfo = timeInfo;
        ResetInfoGrid(true);
        if (itemData != null)
        {
            BaseItem baseItem = new BaseItem(itemData.item_base_id,data);
            itemName.text = baseItem.Name;

            SetIcon(true, baseItem.Icon);
            bool enable = (itemData.item_num > 1) ? true : false;
            SetNum(enable, itemData.item_num.ToString());
            SetBorder(true, baseItem.BorderIcon);
            if (baseItem.IsMuhon)
            {
                SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
                
            }
            else if (baseItem.IsRuneStone)
            {
                SetRuneStoneMask(true, (uint)baseItem.Grade);
            }
            yuanbaoicon.gameObject.SetActive(itemData.great);
            jinbiicon.gameObject.SetActive(!itemData.great);  
            itemPrice.text = itemData.money.ToString();

             int showTime  = 0;
            if (baseItem.IsTreasure)
            {
                showTime = GameTableManager.Instance.GetGlobalConfig<int>("GreatShowTime");
                m_l_GongShiSeconds = (int)(timeInfo.sell_time + showTime - DateTimeHelper.Instance.Now);
            }
            int unSalingTime  = GameTableManager.Instance.GetGlobalConfig<int>("SellItemUnSalingTime");
            m_l_XiaJiaSeconds = (int)(timeInfo.sell_time + showTime + unSalingTime - DateTimeHelper.Instance.Now);



            remainTime.gameObject.SetActive((itemData.great && m_l_GongShiSeconds > 0));

          

                  
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
    }

    //更新刷新间隔
    public const float REFRESH_LEFTTIME_GAP = 1F;
    //下一次刷新剩余的时间
    private float next_refresh_left_time = 0;
    //剩余公示时间
    private int m_l_GongShiSeconds = 0;
    //剩余下架时间
    private int m_l_XiaJiaSeconds = 0;

    void Update()
    {
        next_refresh_left_time -= Time.deltaTime;
        if (next_refresh_left_time <= Mathf.Epsilon)
        {
            next_refresh_left_time = REFRESH_LEFTTIME_GAP;
            m_l_GongShiSeconds--;
            m_l_XiaJiaSeconds--;
            if (itemData != null)
            {
                if (itemData.great)
                {
                    if (m_l_GongShiSeconds > 0)
                    {
                        if (!gongShiLabel.gameObject.activeSelf)
                        {
                            gongShiLabel.gameObject.SetActive(true);
                            gongShiLabel.text = "公示";
                        }
                        remainTime.text = DateTimeHelper.ParseTimeSecondsFliter((int)m_l_GongShiSeconds);
                    }
                    else
                    {
                        gongShiLabel.text = "珍稀";
                        remainTime.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (gongShiLabel.gameObject.activeSelf)
                    {
                        gongShiLabel.gameObject.SetActive(false);
                    }
                }
            }          
            if (m_l_XiaJiaSeconds <= 0)
            {
                if (!remainTime.gameObject.activeSelf)
                {
                    remainTime.text = "已过期";
                    remainTime.gameObject.SetActive(true);
                }

            }
        }               
    }
    DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0);
    void OnClickGetBackBtn(GameObject go)
    {
        if (itemData != null)
        {
            DataManager.Manager<ConsignmentManager>().ReqCancelConsignItem(itemData.market_id);
        }
    }

    void OnClick()
    {
        if (itemData != null)
        {
            DataManager.Manager<ConsignmentManager>().ReqItemInfoConsignment(itemData);
        }
    }
    #region UIEventCallBack
    protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemInfoGrid && null != itemData)
                    {
                        DataManager.Manager<ConsignmentManager>().ReqItemInfoConsignment(itemData);
                    }
                }
                break;
        }

    }
    #endregion
}
