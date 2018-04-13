using System;
using System.Collections.Generic;
using UnityEngine;
using GameCmd;
using table;
class UIRewardFindGrid : UIGridBase
{
    UILabel titleName = null;
    UILabel leftTime = null;
    GameObject m_goRecieve = null;
    GameObject m_btnRoot = null;
    GameObject ordinaryBtn = null;
    GameObject prefectBtn = null;
    Transform itemRoot = null;
    RewardFindData m_rewardData = null;
    UISprite sp1 = null;
    UISprite sp2 = null;
    UILabel lb1 = null;
    UILabel lb2 = null;

    Transform m_trans_UIItemRewardGrid; 
    UIGridCreatorBase m_ctor;
    public uint RewardFindID
    {
        set;
        get;
    }


    protected override void OnAwake()
    {
        base.OnAwake();
        titleName = transform.Find("Tittle/TittleName").GetComponent<UILabel>();
        leftTime = transform.Find("Tittle/leftTime").GetComponent<UILabel>();
        m_goRecieve = transform.Find("Status_Received").gameObject;
        m_btnRoot = transform.Find("BtnRoot").gameObject;
        ordinaryBtn = transform.Find("BtnRoot/OrdinaryBtn").gameObject;
        prefectBtn = transform.Find("BtnRoot/PrefectBtn").gameObject;
        itemRoot = transform.Find("offset/ItemRoot");
        sp1 = transform.Find("BtnRoot/OrdinaryBtn/moneyType").GetComponent<UISprite>();
        sp2 = transform.Find("BtnRoot/PrefectBtn/moneyType").GetComponent<UISprite>();
        lb1 = transform.Find("BtnRoot/OrdinaryBtn/moneyNum").GetComponent<UILabel>();
        lb2 = transform.Find("BtnRoot/PrefectBtn/moneyNum").GetComponent<UILabel>();
        if (ordinaryBtn != null)
        {
            UIEventListener.Get(ordinaryBtn).onClick = OnGetOrdinaryReward;
        }
        if (prefectBtn != null)
        {
            UIEventListener.Get(prefectBtn).onClick = OnGetPrefectReward;
        }

        m_trans_UIItemRewardGrid = transform.Find("UIItemRewardGrid");
        AddCreator(itemRoot);
    }

    void OnGetOrdinaryReward(GameObject go) 
    {
        if (m_rewardData == null)
       {
           return;
       }
        NetService.Instance.Send(new stFindRewardDataUserCmd_CS() { find_id = m_rewardData.id, find_type = FindType.Ordinary_Find });
    }
    void OnGetPrefectReward(GameObject go)
    {
        if (m_rewardData == null)
        {
            return;
        }
        NetService.Instance.Send(new stFindRewardDataUserCmd_CS() { find_id = m_rewardData.id, find_type = FindType.Prefect_Find });
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (data is RewardFindData)
        {

            m_rewardData = (RewardFindData)data;
            RewardFindID = m_rewardData.id;
            if (titleName != null)
            {
                DailyDataBase daily = GameTableManager.Instance.GetTableItem<DailyDataBase>(m_rewardData.id);
                if(daily != null)
                {
                    titleName.text = daily.name;
                }               
            }
            if (leftTime != null)
            {
                leftTime.text = m_rewardData.left_time.ToString();
            }
            SetButtonState();
            RefreshItems();
            SetMoneyContent();
        }

    }
    CMResAsynSeedData<CMAtlas> m_priceAsynSeed = null;
    CMResAsynSeedData<CMAtlas> m_priceAsynSeed2 = null;
    private void SetMoneyContent() 
    {
        uint level = DataManager.Manager<WelfareManager>().Previous_Lv;
        RewardFindDataBase tab = GameTableManager.Instance.GetTableItem<RewardFindDataBase>(level,(int)m_rewardData.id);
        if (tab != null)
        {
             CurrencyIconData d1 = CurrencyIconData.GetCurrencyIconByMoneyType((ClientMoneyType)tab.ord_Type);
             CurrencyIconData d2=  CurrencyIconData.GetCurrencyIconByMoneyType((ClientMoneyType)tab.pre_Type);

             UIManager.GetAtlasAsyn(d1.smallIconName, ref m_priceAsynSeed, () =>
             {
                 if (null != sp1)
                 {
                     sp1.atlas = null;
                 }
             }, sp1);
             UIManager.GetAtlasAsyn(d2.smallIconName, ref m_priceAsynSeed2, () =>
             {
                 if (null != sp2)
                 {
                     sp2.atlas = null;
                 }
             }, sp2);

             lb1.text = tab.ord_Num.ToString();
             lb2.text = tab.pre_Num.ToString();
        }
           
        
    }
    private void SetButtonState()
    {
        if (m_goRecieve == null || itemRoot == null)
        {
            return;
        }

        bool enable = m_rewardData.state != RewardFindData.RewardFindState.Got;
        m_btnRoot.gameObject.SetActive(enable);
        m_goRecieve.SetActive(!enable);
    }





    void RefreshItems()
    {
        list.Clear();
        for (int i = 0; i < m_rewardData.list.Count; i++)
        {
            list.Add(new UIItemRewardData() 
            {
                itemID = m_rewardData.list[i].itemID,
                num =  m_rewardData.list[i].itemNum,
            });
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
           m_ctor.Initialize<UIItemRewardGrid>(m_trans_UIItemRewardGrid.gameObject, OnUpdateGridData,null);
       }
    }
    void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemRewardGrid)
        {
            UIItemRewardGrid itemShow = grid as UIItemRewardGrid;
            if (itemShow != null)
            {
                if(index < list.Count)
                {
                    UIItemRewardData data = list[index];
                    uint itemID = data.itemID;
                    uint num = data.num;
                    itemShow.SetGridData(itemID, num, false);
                }                            
            }
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_rewardData != null)
        {
            m_rewardData = null;
        }
        if (m_priceAsynSeed != null)
        {
            m_priceAsynSeed.Release(true);
            m_priceAsynSeed = null;
        }
        if (m_priceAsynSeed2 != null)
        {
            m_priceAsynSeed2.Release(true);
            m_priceAsynSeed2 = null;
        }
    }
}