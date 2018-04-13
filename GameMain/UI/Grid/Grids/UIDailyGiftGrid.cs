using System;
using table;
using System.Collections;
using UnityEngine;
using GameCmd;
using System.Collections.Generic;
public class UIDailyGiftGrid : UIGridBase
{
    UILabel m_lab_title;
    UILabel m_lab_description;
    Transform m_trans_rewardRoot;
    UIButton m_spr_buyBtn;
    UILabel m_label_desc;
    uint id;
    Transform m_trans_UIItemRewardGrid;
    UIGridCreatorBase m_ctor;

    ActivityData activityData;
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_title        = CacheTransform.Find("title").GetComponent<UILabel>();
        m_lab_description  = CacheTransform.Find("description").GetComponent<UILabel>();
        m_trans_rewardRoot = CacheTransform.Find("RewardRoot");
        m_spr_buyBtn = CacheTransform.Find("buy").GetComponent<UIButton>();
        m_label_desc = CacheTransform.Find("buy/Label").GetComponent<UILabel>();
        if (m_spr_buyBtn != null)
        {
           UIEventListener.Get(m_spr_buyBtn.gameObject).onClick = OnBuyDailyGift;
        }
        m_trans_UIItemRewardGrid = CacheTransform.Find("UIItemRewardGrid");
        AddCreator(m_trans_rewardRoot);
    }
    public override void SetGridData(object data)
    {
        base.SetGridData(data);
         if (data == null)
        {
            return;
        }
         if (data is ActivityData)
         {
             activityData = (ActivityData)data;
             m_lab_description.text = activityData.desc;
             m_lab_title.text = activityData.title;
             ParseReward(activityData.rewards);
             SetBtnState(activityData.state);
         }    
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
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
                    itemShow.SetGridData(itemID, num, false,false);
                }
            }
        }
    }
    void ParseReward(string args) 
    {
        if(string.IsNullOrEmpty(args))
        {
            return;
        }
        list.Clear();
        string[] param = args.Split('_');
        if (param.Length == 2)
        {
            uint itemid = 0;
            uint num = 0;
            if (uint.TryParse(param[0], out itemid) && uint.TryParse(param[1],out num))
            {
                list.Add(new UIItemRewardData()
                {
                    itemID = itemid,
                    num = num,
                });
            }
        }
        m_ctor.CreateGrids(list.Count);
    }
    public void SetBtnState(ActivityState state) 
    {
        bool hadbuy = state == ActivityState.Got;
        m_spr_buyBtn.isEnabled = !hadbuy;
        m_label_desc.text = hadbuy ? "今日已购" : "立即购买";
    }
    void OnBuyDailyGift(GameObject caster) 
    {

        DataManager.Manager<ActivityManager>().BuyDailyGift(activityData.id);
    }
}