using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using UnityEngine;
using table;



class UIDailyCalendarGrid : UIGridBase
{
    UILabel name;
    UILabel time;
    UISprite select;
    UIToggle bg;

    public uint dailyID   
    {
        set;
        get;
    }
    protected override void OnAwake()
    {
        base.OnAwake();
        bg = CacheTransform.Find("bg").GetComponent<UIToggle>();
        name =CacheTransform.Find("name").GetComponent<UILabel>();
        time = CacheTransform.Find("time").GetComponent<UILabel>();
        select = CacheTransform.Find("select").GetComponent<UISprite>();
        select.gameObject.SetActive(false);
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        this.dailyID = (uint)data;
        DailyDataBase tab = GameTableManager.Instance.GetTableItem<DailyDataBase>(dailyID);
        if (tab != null)
        {
            name.text = tab.name;
            time.text = tab.activityTime;
            //time.text = DataManager.Manager<DailyManager>().GetActiveAndEndTimeByID(tab, 2).ToString();
        }
        else 
        {
            Engine.Utility.Log.Error("null" + data.ToString());
        
        }
    }

    public void SetSelect(bool value) 
    {
        if (select != null)
       {
           select.gameObject.SetActive(value);
       }
    }
    public void SetBg(bool value) 
    {
        if (bg != null)
        {
            bg.value = value;
        }
    }
}
