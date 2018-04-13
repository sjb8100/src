//********************************************************************
//	创建日期:	2016-12-5   14:29
//	文件名称:	UIActiveToggleGrid.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	运营活动Grid
//********************************************************************
using System;
using table;
using System.Collections.Generic;
using System.Linq;
using GameCmd;
using UnityEngine;
class UIActiveToggleGrid : UIGridBase
{
    UISprite bg;
    UISprite select;
    UILabel name;
    UILabel level;
    UISprite warning;
    UILabel time;
    RechargeCostDataBase costData;
    public ActivityType m_tp_ActivityType
    {
        set;
        get;
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        bg = CacheTransform.Find("Bg").GetComponent<UISprite>();
        select = CacheTransform.Find("Select").GetComponent<UISprite>();
        name = CacheTransform.Find("Name").GetComponent<UILabel>();
        warning = CacheTransform.Find("Warrning").GetComponent<UISprite>();
        select.enabled = false;
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data is ActivityType)
        {
            m_tp_ActivityType = (ActivityType)data;
            name.text = GetName(m_tp_ActivityType);
            bool showRed = DataManager.Manager<ActivityManager>().HaveRewardCanGetByType(m_tp_ActivityType);
            warning.gameObject.SetActive(showRed);
        }         
    }

    string GetName(ActivityType t) 
    {
            LocalTextType type = LocalTextType.LocalText_None;
            switch (t)
            {
                case ActivityType.GrowthFund:
                    type = LocalTextType.Local_TXT_GrowthFund;
                    break;
                case ActivityType.SingleRechargeSingleDay:
                    type = LocalTextType.Local_TXT_SingleRechargeSingleDay;
                    break;
                case ActivityType.AllRechargeSingleDay:
                    type = LocalTextType.Local_TXT_AllRechargeSingleDay;
                    break;
                case ActivityType.AllCostSingleDay:
                    type = LocalTextType.Local_TXT_AllCostSingleDay;
                    break;
                case ActivityType.AllRecharge:
                    type = LocalTextType.Local_TXT_AllRecharge;
                    break;
                case ActivityType.AllCost:
                    type = LocalTextType.Local_TXT_AllCost;
                    break;
                case ActivityType.DailyGift:
                    type = LocalTextType.Local_TXT_DailyGift;
                    break;
            }
            return DataManager.Manager<TextManager>().GetLocalText(type);
    }

    public void SetSelect(bool highLight) 
    {
       if (select != null)
       {
           select.enabled = highLight;
       }
    }
}