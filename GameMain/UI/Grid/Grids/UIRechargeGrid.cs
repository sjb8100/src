//********************************************************************
//	创建日期:	2016-11-7   11:26
//	文件名称:	UIRechargeGrid.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	充值面板中的Grid
//********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
using table;
class UIRechargeGrid : UIGridBase
{

    private UILabel cost;
    private UISprite costIcon;


    //limit
    private UILabel limit;

//     //名称
     private UILabel name;
    //图标
    private UISprite icon;
    //num
    private UILabel num;
    private uint id;
     public uint RechargeID
    {
        get 
        {
            return id;
        }
    }
     private int index ;
    private int Index
     {
         set 
         {
             index = value;
         }
         get 
         {
             return index;
         }
     }

    protected override void OnAwake()
    {
        base.OnAwake();
        name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        icon = CacheTransform.Find("Content/Icon").GetComponent<UISprite>();
        num = CacheTransform.Find("Content/Num").GetComponent<UILabel>();
        limit = CacheTransform.Find("Content/Limit").GetComponent<UILabel>();
        cost = CacheTransform.Find("Content/PurchaseBtn/Cost/Content/Num").GetComponent<UILabel>();
        costIcon = CacheTransform.Find("Content/PurchaseBtn/Cost/Content/Icon").GetComponent<UISprite>();
    }


    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if(data != null && data is uint)
        {
            id = (uint)data;
        }
        RechargeDataBase info = GameTableManager.Instance.GetTableItem<RechargeDataBase>(id);
        if (info == null)
        {
            return;
        }
        this.id = info.dwID;
        name.text = info.rechargeName;
        cost.text = info.money.ToString();
        if (icon != null)
        {
            icon.spriteName = info.icon;
        }     
         if (!DataManager.Manager<Mall_HuangLingManager>().AlreadyFirstRecharge.Contains(id))
         {
             limit.text =info.firstRechargeDes;
         }
         else 
         {
             limit.text =info.rechargeDes;
         }
         num.gameObject.SetActive(false);
    }

}
