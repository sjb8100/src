//********************************************************************
//	创建日期:	2016-12-2   15:32
//	文件名称:	ActivePanel.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	运营活动---充值消费活动
//********************************************************************
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using table;
using Engine;
using Client;
using GameCmd;
using System.Text;
using Common;


public enum ActivityType
{
    None                    = 0 ,
    GrowthFund              = 1 ,
    SingleRechargeSingleDay = 2 ,
    AllRechargeSingleDay    = 3 ,
    AllCostSingleDay        = 4 ,
    AllRecharge             = 5 ,
    AllCost                 = 6 ,
    DailyGift               = 7 ,
    Max                     = 8 ,
};
partial class ActivityPanel : UIPanelBase
{

    List<ActivityType> m_lst_Type = null;
    ActivityType selectType = ActivityType.GrowthFund;
    List<ActivityData> activityList = new List<ActivityData>();

    List<uint> m_lst_dailyGiftIDs = new List<uint>();

    protected override void OnLoading()
    {
        base.OnLoading();
        RegisterGlobalEvent(true);
        List<RechargeCostDataBase> datas = GameTableManager.Instance.GetTableList<RechargeCostDataBase>();
        if (datas != null)
        {
            for (int i = 0; i < datas.Count; i ++ )
            {               
                if (datas[i].type == (uint)ActivityType.DailyGift)
                {
                    if (!m_lst_dailyGiftIDs.Contains(datas[i].ID))
                    {
                        m_lst_dailyGiftIDs.Add(datas[i].ID);
                    }
                }
            }
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_ListRoot != null)
        {
            m_ctor_ListRoot.Release(depthRelease);
        }
        if (m_ctor_RightRoot != null)
        {
            m_ctor_RightRoot.Release(depthRelease);
        }
        if (m_ctor_DailyGiftRoot != null)
        {
            m_ctor_DailyGiftRoot.Release(depthRelease);
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        Release();
        DataManager.Manager<ActivityManager>().ValueUpdateEvent -= OnUpdateList;
    }
    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECHARGESINGLEDATA, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECHARGEGETREWARD, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ISBUYGROWTH, EventCallBack);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ISGETREWARD, EventCallBack);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECHARGESINGLEDATA, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECHARGEGETREWARD, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ISBUYGROWTH, EventCallBack);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ISGETREWARD, EventCallBack);
        }
    }
    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        RegisterGlobalEvent(false);
    }
    void EventCallBack(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameEventID.RECHARGESINGLEDATA)
        {
            
            CreatUILeft(selectType);
        }
        if (nEventID == (int)Client.GameEventID.RECHARGEGETREWARD)
        {
            CreatUILeft(selectType);
        }
        if (nEventID == (int)Client.GameEventID.ISBUYGROWTH)
        {
            CreatUILeft(selectType);
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Local_TXT_BuyGrowthFundSuccess);
            //DataManager.Manager<TextManager>().GetLocalText();
        }
        if (nEventID == (int)Client.GameEventID.ISGETREWARD)
        {
            CreatUILeft(selectType);
        }

    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_lst_Type = DataManager.Manager<ActivityManager>().GetTypeList();
        InitPanel();
        ToggleType(selectType,true);
        DataManager.Manager<ActivityManager>().ValueUpdateEvent += OnUpdateList;
    }
    void OnUpdateList(object obj, ValueUpdateEventArgs value)
    {
        if (value.key.Equals("OnBuyDailyGift"))
        {
            if (value.newValue != null )
            {
                uint id = (uint)value.newValue;
                if (m_lst_dailyGiftIDs.Contains(id))
                {
                    m_ctor_DailyGiftRoot.UpdateData(m_lst_dailyGiftIDs.IndexOf(id));
                }
            }
        }
    }

    void InitPanel()
    {
        if (null != m_ctor_ListRoot)
        {
            m_ctor_ListRoot.RefreshCheck();
            m_ctor_ListRoot.Initialize<UIActiveToggleGrid>(m_trans_UIWelfareToggleGrid.gameObject, OnActivityListUpdate, OnActivityGridUIEvent);
            m_ctor_ListRoot.CreateGrids(m_lst_Type.Count);
           
        }
        if (null != m_ctor_DailyGiftRoot)
        {
            m_ctor_DailyGiftRoot.RefreshCheck();
            m_ctor_DailyGiftRoot.Initialize<UIDailyGiftGrid>(m_trans_UIDailyGiftGrid.gameObject, OnActivityListUpdate, OnActivityGridUIEvent);

        }
        if (null != m_ctor_RightRoot)
        {
            m_ctor_RightRoot.RefreshCheck();
            m_ctor_RightRoot.Initialize<UIActiveOtherGrid>(m_trans_UIWelfareOtherGrid.gameObject, OnActivityListUpdate, OnActivityGridUIEvent);
        }   
    }


    void CreatUILeft(ActivityType type)
    {
        if (null != m_ctor_ListRoot)
        {
            m_ctor_ListRoot.CreateGrids(m_lst_Type.Count);
            if (activityList == null)
            {
                activityList = new List<ActivityData>();
            }
            activityList = DataManager.Manager<ActivityManager>().GetAchieveDataByType(type);
            List<uint> gridIds = DataManager.Manager<ActivityManager>().IDS;
            if (gridIds == null)
            {
                Engine.Utility.Log.Error("gridIds==null");
                return;
            }
            bool isDailyGift = selectType == ActivityType.DailyGift;
            m__NormalContent.gameObject.SetActive(!isDailyGift);
            m_ctor_RightRoot.gameObject.SetActive(!isDailyGift);

            m__DailyGiftContent.gameObject.SetActive(isDailyGift);      
            m_ctor_DailyGiftRoot.gameObject.SetActive(isDailyGift);
            if (isDailyGift)
            {
                m_ctor_DailyGiftRoot.CreateGrids(activityList.Count);
            }
            else 
            {
                m_ctor_RightRoot.CreateGrids(activityList.Count);
            } 
                 
        }
        m_trans_ToggleScrollView.GetComponent<UICacheScrollView>().ResetPosition();
        m_trans_OtherScrollView.GetComponent<UICacheScrollView>().ResetPosition();

        m_label_ScheduleLabel.gameObject.SetActive(type == ActivityType.SingleRechargeSingleDay);
        if(type == ActivityType.SingleRechargeSingleDay)
        {
            m_label_ScheduleLabel.text = DataManager.Manager<ActivityManager>().GetScheduleIDByType(type);
        }     
        bool bShowBuyBtn = DataManager.Manager<ActivityManager>().IsBuy;
        m_btn_Buy.gameObject.SetActive(!bShowBuyBtn);
        m_sprite_Bought.gameObject.SetActive(bShowBuyBtn);

        m_trans_WeekCostContent.gameObject.SetActive(type == ActivityType.AllCost);
        m_trans_BuyContent.gameObject.SetActive(type == ActivityType.GrowthFund);


        ChangeTexture(type);
    }
    void OnActivityListUpdate(UIGridBase grid, int index)
    {
        if (grid is UIActiveToggleGrid)
        {
            UIActiveToggleGrid toggleGrid = grid as UIActiveToggleGrid;
            if (index < m_lst_Type.Count)
            {
                toggleGrid.SetGridData(m_lst_Type[index]);
            }                
        }
        if (grid is UIActiveOtherGrid)
        {
            UIActiveOtherGrid otherGrid = grid as UIActiveOtherGrid;
            if (index < activityList.Count)
            {
                otherGrid.SetGridData(activityList[index]);
            }         
        }
        if (grid is UIDailyGiftGrid)
        {
            UIDailyGiftGrid giftGrid = grid as UIDailyGiftGrid;
            if (index < activityList.Count)
            {
                giftGrid.SetGridData(activityList[index]);
            }
        }
    }
    void OnActivityGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIActiveToggleGrid)
                {
                    UIActiveToggleGrid toggleGrid = data as UIActiveToggleGrid;
                    if (toggleGrid != null)
                    {
                        ToggleType(toggleGrid.m_tp_ActivityType);
                    }
                }
                break;
        }
    }

    void ToggleType(ActivityType type,bool force =false) 
    {
        if (selectType == type && !force)
        {
            return;
        }
        UIActiveToggleGrid grid = m_ctor_ListRoot.GetGrid<UIActiveToggleGrid>(m_lst_Type.IndexOf(selectType));
        if (null != grid)
        {
            grid.SetSelect(false);
        }
        grid = m_ctor_ListRoot.GetGrid<UIActiveToggleGrid>(m_lst_Type.IndexOf(type));
        if (null != grid)
        {
            grid.SetSelect(true);
        }
        selectType = type;
        CreatUILeft(selectType);
      
        m_label_WeekCostNum.text = DataManager.Manager<ActivityManager>().AllCost.ToString();

    }

    CMResAsynSeedData<CMTexture> iuiIconAtlas = null;
    void ChangeTexture(ActivityType type)
    {
        uint resID = DataManager.Manager<ActivityManager>().GetTextureNameByActivityType((uint)type);
        UIManager.GetTextureAsyn(resID, ref iuiIconAtlas,
                   () =>
                   {
                       if (m__Rule != null) { m__Rule.mainTexture = null; }
                   },
               m__Rule);
    }

    void onClick_Buy_Btn(GameObject catser)
    {
        DataManager.Manager<ActivityManager>().BuyFoundation();
    }
    void onClick_Btn_close_Btn(GameObject caster)
    {
        HideSelf();
    }


    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
        int secondTabData = -1;
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];
        }
        else
        {
            firstTabData = 1;
        }
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 1)
        {
            secondTabData = jumpData.Tabs[1];
            if (m_lst_Type != null)
            {
                if (secondTabData < (int)ActivityType.Max)
                {
                    if (m_lst_Type.Contains((ActivityType)secondTabData))
                    {
                        ToggleType((ActivityType)secondTabData, true);
                    }
                }
            }
        }

    }

}
