using UnityEngine;
using System.Collections;
using GameCmd;
using table;
using System;

public class UIConsignmentItemListGrid : UIItemInfoGridBase
{
    private UILabel itemName;
    private UILabel itemLevel;
    private UILabel itemPrice;
    private UIToggle btn_star;
    private UILabel clockLabel;
    private UISprite IsGongShi;
    private UILabel isGonsShiLabel;
    private UISprite moneyIcon;
    private UISprite select; 
    bool HadStard = false;
    public ulong marked_id { set; get; }
    public ItemPageInfo itemPageInfo { set; get; }
    public bool CanBuy{ set; get; }
    protected override void OnAwake()
    {
        base.OnAwake();
        itemName = CacheTransform.Find("Name").GetComponent<UILabel>();
        itemLevel = CacheTransform.Find("Level").GetComponent<UILabel>();
        InitItemInfoGrid(CacheTransform.Find("InfoGridRoot/InfoGrid"), false);
        itemPrice = CacheTransform.Find("CurrencyContent/Num").GetComponent<UILabel>();
        GameObject btnBuy = CacheTransform.Find("Btn_Star").gameObject;
        UIEventListener.Get(btnBuy).onClick = OnClickShouCangBtn;
        btn_star = CacheTransform.Find("Btn_Star").GetComponent<UIToggle>();
        clockLabel = CacheTransform.Find("Clock").GetComponent<UILabel>();
        IsGongShi = CacheTransform.Find("IsGongShi").GetComponent<UISprite>();
        isGonsShiLabel = CacheTransform.Find("IsGongShi/Label").GetComponent<UILabel>();
        moneyIcon = CacheTransform.Find("CurrencyContent/Icon").GetComponent<UISprite>();
        select = CacheTransform.Find("select").GetComponent<UISprite>();
    }

    void OnClickShouCangBtn(GameObject go)
    {
        if (marked_id != 0)
        {
            if (!DataManager.Manager<ConsignmentManager>().OverflowMaxStarItem(marked_id))
            {
                NetService.Instance.Send(new stStarConsignmentUserCmd_CS() { market_id = marked_id, star = !DataManager.Manager<ConsignmentManager>().AllStarMarkedIDs.Contains(marked_id) });
            }
            else 
            {
                SetStarValue(false);
                TipsManager.Instance.ShowTipsById(117023);
            }
           
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
    void OnBuyPanelCallBack(int num)
    {
        if (itemPageInfo != null)
        {
            DataManager.Manager<ConsignmentManager>().ReqBuyItemConsignment(itemPageInfo.market_id, (uint)num);
        }
    }

    public void UpdateItemInfo(ItemPageInfo info,ItemSerialize data = null)
    {
        itemPageInfo = info;
        ResetInfoGrid(true);
        if (itemPageInfo != null)
        {
            marked_id = info.market_id;
            BaseItem baseItem = new BaseItem(itemPageInfo.item_base_id, data);
            itemName.text = baseItem.Name;
            itemLevel.text = baseItem.UseLv.ToString();
            itemPrice.text = itemPageInfo.money.ToString();
            SetIcon(true, baseItem.Icon);
            bool enable = (itemPageInfo.item_num > 1) ? true : false;
            SetNum(enable, itemPageInfo.item_num.ToString());
            SetBorder(true, baseItem.BorderIcon);
            if (baseItem.IsMuhon)
            {
                SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
                SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
            }
            else if (baseItem.IsRuneStone)
            {
                SetRuneStoneMask(true, (uint)baseItem.Grade);
            }
            HadStard = DataManager.Manager<ConsignmentManager>().AllStarMarkedIDs.Contains(marked_id);
            SetStarValue(HadStard);
            m_l_leftSeconds =(uint)((itemPageInfo.show_time - DateTimeHelper.Instance.Now )>0 ? (itemPageInfo.show_time - DateTimeHelper.Instance.Now ) :0);
            moneyIcon.spriteName = itemPageInfo.great ? CurrencyIconData.GetCurrencyIconByMoneyType(ClientMoneyType.YuanBao).smallIconName
                                                   : CurrencyIconData.GetCurrencyIconByMoneyType(ClientMoneyType.YinLiang).smallIconName;
            IsGongShi.gameObject.SetActive(itemPageInfo.great);
        }
    }
    //更新刷新间隔
    public const float REFRESH_LEFTTIME_GAP = 1F;
    //下一次刷新剩余的时间
    private float next_refresh_left_time = 0;
    //剩余时间
    private uint m_l_leftSeconds = 0;
    
    void Update() 
    {
        if (m_l_leftSeconds > 0)
        {
            if (!clockLabel.gameObject.activeSelf)
            {
                clockLabel.gameObject.SetActive(true);
              
            }       
            CanBuy = false;
            next_refresh_left_time -= Time.deltaTime;
            if (next_refresh_left_time <= Mathf.Epsilon)
            {
                next_refresh_left_time = REFRESH_LEFTTIME_GAP;
                clockLabel.text = DateTimeHelper.ParseTimeSecondsFliter(m_l_leftSeconds);
                m_l_leftSeconds--;
            }
        }
        else 
        {
            if (clockLabel.gameObject.activeSelf)
            {
                clockLabel.gameObject.SetActive(false);
            }        
            isGonsShiLabel.text = "珍稀";
            CanBuy = true;                      
        }
          
    }
    public void SetStarValue(bool shouCang) 
    {
        if (btn_star != null)
        {
            btn_star.value = shouCang;
        }
    }

    public void SetSelect(bool value) 
    {
       if(select != null)
       {
           select.gameObject.SetActive(value);
       }
    }
    protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemInfoGrid && null != itemPageInfo)
                    {
                        DataManager.Manager<ConsignmentManager>().ReqItemInfoConsignment(itemPageInfo);
                    }
                }
                break;
        }

    }
//    #endregion
}
