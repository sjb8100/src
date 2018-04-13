//********************************************************************
//	创建日期:	2016-12-5   17:41
//	文件名称:	UIActiveOtherGrid.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:   右侧列表
//********************************************************************
using System;
using table;
using System.Collections.Generic;
using System.Linq;
using GameCmd;
using UnityEngine;
class UIActiveOtherGrid : UIGridBase
{
    UILabel title;
    UILabel process;
    Transform itemRoot;
    UIButton btn;
    UISprite warning;
    UISprite Status_Received;
    UISprite Status_NoMatch;
    UILabel TitleName2;
    int level;      //等级
    int daySignal;  //单日单笔
    int dayCost;    //单日消费
    int allRecharge;//累计充值
    int allCost;    //累计消费 
    Transform m_trans_UIItemRewardGrid;
    UIGridCreatorBase m_ctor;

    ActivityData activityData;
    protected override void OnAwake()
    {
        base.OnAwake();
        title = CacheTransform.Find("Tittle/TittleName").GetComponent<UILabel>();
        process = CacheTransform.Find("process").GetComponent<UILabel>();
        itemRoot = CacheTransform.Find("offset/ItemRoot").GetComponent<Transform>();   
        btn = CacheTransform.Find("Btn_Receive").GetComponent<UIButton>();
        Status_Received = CacheTransform.Find("Status_Received").GetComponent<UISprite>();
        Status_NoMatch = CacheTransform.Find("Status_NoMatch").GetComponent<UISprite>();
        TitleName2 = CacheTransform.Find("Tittle/TittleName2").GetComponent<UILabel>();
        m_trans_UIItemRewardGrid = CacheTransform.Find("UIItemRewardGrid");
        warning = CacheTransform.Find("Btn_Receive/warning").GetComponent<UISprite>();
        AddCreator(itemRoot);
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data == null)
        {
            return;
        }
        if(data is ActivityData)
        {
            activityData = (ActivityData)data;
            title.text = activityData.title;
            if (!string.IsNullOrEmpty(activityData.rewards))
            {
                SetItem(activityData.rewards);
            }
            SetButton(activityData);
        }     
    }
    void SetItem(string reward) 
    {
        string[] items = reward.Split(';');
        list.Clear();
        for (int i = 0; i < items.Length; i++)
        {
            string[] singleItem = items[i].Split('_');
            uint itemID = 0;
            uint num = 0;
            if (uint.TryParse(singleItem[0], out itemID) && uint.TryParse(singleItem[1], out num))
            {
                 list.Add(new UIItemRewardData() 
                   {
                       itemID = itemID,
                       num = num,
                   });
            }          
        }
          m_ctor.CreateGrids(list.Count);
    }

    List<UIItemRewardData> list = new List<UIItemRewardData>();
    void AddCreator(Transform parent)
    {
        if (parent != null)
        {
            m_ctor = parent.GetComponent<UIGridCreatorBase>();
            if (m_ctor == null)
            {
                m_ctor = parent.gameObject.AddComponent<UIGridCreatorBase>();
            }
            m_ctor.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_ctor.gridWidth = 90;
            m_ctor.gridHeight = 90;
            m_ctor.RefreshCheck();
            m_ctor.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData, null);
        }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if (index < list.Count)
                {
                    UIItemRewardData data = list[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num, false);
                }
            }
        }
    }
    /// <summary>
    ///   got  是不是领了   
    /// </summary>
    /// <param name="type"></param>
    /// <param name="got"></param>
    void SetButton(ActivityData data) 
    {
        string msg = "";
        bool canGet =  data.state == ActivityState.CanGet;
        bool got = data.state == ActivityState.Got;
        Status_NoMatch.gameObject.SetActive(false);
        TitleName2.gameObject.SetActive(false);
        Status_Received.gameObject.SetActive(got);
        btn.gameObject.SetActive(!got);
        btn.isEnabled = canGet;
        warning.gameObject.SetActive(canGet);
        process.gameObject.SetActive(data.type != ActivityType.SingleRechargeSingleDay);
        if (!got)
        {
            msg = string.Format("{0}/{1}", data.process, data.total);
            UIEventListener.Get(btn.gameObject).onClick = OnGetReward;
        }
        process.text = data.state == ActivityState.CanGet ? ColorManager.GetColorString(ColorType.JZRY_Green, msg) : ColorManager.GetColorString(ColorType.JZRY_Txt_Red, msg); 
    }


    void OnGetReward(GameObject caster) 
    {
        if (activityData != null)
        {
            NetService.Instance.Send(new stRequstRechargeRewardPropertyUserCmd_CS() { id = activityData.id });
        }
    
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }
}