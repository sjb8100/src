using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
using table;
class UISellItemPriceGrid : UIItemInfoGridBase
{
    UILabel name;
    Transform icon;
    UILabel price;
    UISprite money;
    ItemPriceInfo info = null;
    ItemSerialize itData = null;
    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("name").GetComponent<UILabel>();    
        price = CacheTransform.Find("Label").GetComponent<UILabel>();
        icon = CacheTransform.Find("icon").GetComponent<Transform>();
        money = CacheTransform.Find("Label/money").GetComponent<UISprite>();
        InitItemInfoGrid(CacheTransform.Find("InfoGridRoot/InfoGrid"), false);
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        info = data as ItemPriceInfo;
        if (info != null)
        {
            if (money != null)
            {
                CurrencyIconData currency = CurrencyIconData.GetCurrencyIconByMoneyType((ClientMoneyType)info.money_type);
                if (currency != null)
                {
                    money.spriteName = currency.smallIconName;
                }
            }
             itData = new ItemSerialize();
             itData = ItemSerialize.Deserialize(info.item_data);
             price.text = info.price.ToString();
             if (itData != null)
            {
                ResetInfoGrid(true);

                BaseItem baseItem = new BaseItem(itData.dwObjectID, itData);
                name.text = baseItem.Name;

                SetIcon(true, baseItem.Icon);
                bool enable = (info.num > 1) ? true : false;
                SetNum(enable, info.num.ToString());
                SetBorder(true, baseItem.BorderIcon);
                if (baseItem.IsMuhon)
                {
                    SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));

                }
                else if (baseItem.IsRuneStone)
                {
                    SetRuneStoneMask(true, (uint)baseItem.Grade);
                }
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
  protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
  {
      switch (eventType)
      {
          case UIEventType.Click:
              {
                  if (data is UIItemInfoGrid && null != itData)
                  {
                      stRequestItemInfoConsignmentUserCmd_C cmd = new stRequestItemInfoConsignmentUserCmd_C();
                      cmd.market_id = itData.createid;
                      NetService.Instance.Send(cmd);
                  }
              }
              break;
      }

  }
}
