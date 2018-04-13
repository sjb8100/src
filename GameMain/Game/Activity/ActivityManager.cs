//********************************************************************
//	创建日期:	2016-12-5   16:49
//	文件名称:	ActivityManager.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	运营活动_充值数据处理
//********************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Engine;
using UnityEngine;
using GameCmd;
using Client;
using table;
using Engine.Utility;
using System;
public enum ActivityState
{
    CanGet = 3,
    None = 2,
    Got = 1,

}
public class ActivityData : IComparable<ActivityData>
{
    public uint id;
    public uint process;
    public uint total;
    public string title;
    public string rewards;
    public string desc;
    public ActivityType type;
    public ActivityState state = ActivityState.None;
    public int CompareTo(ActivityData other)
    {
        //         if (type == ActivityType.DailyGift)
        //         {
        //             return 0;
        //         }
        int astate = (int)state;
        int bstate = (int)other.state;
        if (astate < bstate)
        {
            return 1;
        }
        else if (astate > bstate)
        {
            return -1;
        }
        return id.CompareTo(other.id);
    }
}
public class ActivityManager : BaseModuleData, IManager
{
    List<RechargeCostDataBase> lstActivityTabData = null;
    Dictionary<uint, uint> m_dicTextureName = null;
    public List<uint> IDS = new List<uint>();
    public List<uint> overidList = new List<uint>();

    public Dictionary<uint, List<ActivityData>> m_dic_Activity = new Dictionary<uint, List<ActivityData>>();

    Dictionary<uint, List<RechargeCostDataBase>> m_dicTabData = new Dictionary<uint, List<RechargeCostDataBase>>();
    public List<uint> OverIDList
    {
        get
        {
            return overidList;
        }
        set
        {
            overidList = value;
        }
    }

    public List<uint> moneyAreCost = new List<uint>();//今日充值钱(类型)列表 针对每日单笔充值
    public List<uint> MoneyAreCost
    {
        get
        {
            return moneyAreCost;
        }
        set
        {
            moneyAreCost = value;
        }
    }
    public int daySignal = 0;  //单日单笔
    public int DaySignal
    {
        get
        {
            return daySignal;
        }
        set
        {
            daySignal = value;
        }
    }
    public uint dayCost = 0;    //单日累计消费
    public uint DayCost
    {
        get
        {
            return dayCost;
        }
        set
        {
            dayCost = value;
        }
    }
    public uint dayRecharge = 0;//今日充值总额
    public uint DayRecharge
    {
        get
        {
            return dayRecharge;
        }
        set
        {
            dayRecharge = value;
        }
    }
    public uint allRecharge = 0;//累计充值
    public uint AllRecharge
    {
        get
        {
            return allRecharge;
        }
        set
        {
            allRecharge = value;
        }
    }
    public uint allCost = 0;    //累计消费
    public uint AllCost
    {
        get
        {
            return allCost;
        }
        set
        {
            allCost = value;
        }
    }

    uint m_dayAllRMB = 0;
    public uint DayAllRMB
    {
        get
        {
            return m_dayAllRMB;
        }
    }

    //周充值
    List<GameCmd.WeekRechTimes> m_lstWeekRech = new List<GameCmd.WeekRechTimes>();
    public List<GameCmd.WeekRechTimes> WeekRechList
    {
        get
        {
            return m_lstWeekRech;
        }
    }
    public bool isBuy = false;
    public bool IsBuy
    {
        get
        {
            return isBuy;
        }
        set
        {
            isBuy = value;
        }
    }

    private bool hadGotFirstRechargeReward = false;
    public bool HadGotFirstRechargeReward
    {
        get
        {
            return hadGotFirstRechargeReward;
        }
        set
        {
            hadGotFirstRechargeReward = value;
        }
    }
    public void Reset(bool depthClearData = false)
    {
        IDS.Clear();
        overidList.Clear();
        moneyAreCost.Clear();
        daySignal = 0;
        dayCost = 0;
        dayRecharge = 0;
        allRecharge = 0;
        allCost = 0;
        isBuy = false;

    }
    public void ClearData()
    {
        RegisterEvent(false);
    }
    void OnEvent(int eventID, object param)
    {
        if (eventID == (int)GameEventID.ENTITYSYSTEM_PROPUPDATE)
        {
            stPropUpdate prop = (stPropUpdate)param;
            if (prop.uid == MainPlayerHelper.GetPlayerUID())
            {
                int lv = MainPlayerHelper.GetPlayerLevel();
                uint type = (uint)ActivityType.GrowthFund;
                if (m_dic_Activity.ContainsKey(type))
                {
                    List<ActivityData> lst = m_dic_Activity[type];
                    for (int i = 0; i < lst.Count; i++)
                    {
                        ActivityData data = lst[i];
                        if (OverIDList.Contains(data.id))
                        {
                            data.state = ActivityState.Got;
                            continue;
                        }
                        if (dailyGiftList.Contains(data.id))
                        {
                            data.state = ActivityState.Got;
                            continue;
                        }

                        data.process = (uint)lv;
                        data.state = (lv >= data.total && isBuy) ? ActivityState.CanGet : ActivityState.None;
                    }
                    if (type != (uint)ActivityType.DailyGift)
                    {
                        lst.Sort();
                    }

                }
            }
        }
        else if (eventID == (int)GameEventID.SYSTEM_GAME_READY)
        {
            if (lstActivityTabData != null)
           {
               for (int i = 0; i < lstActivityTabData.Count;i++ )
               {                  
                   uint schedule = lstActivityTabData[i].scheduleID;
                   uint type = lstActivityTabData[i].type;                
                   if (schedule != 0)
                   {                    
                       if (m_dic_Activity.ContainsKey(type))
                       {
                           if (!DataManager.Manager<DailyManager>().InSchedule(schedule))
                          {
                              m_dic_Activity.Remove(type);
                              if (m_lstType.Contains((ActivityType)type))
                              {
                                  m_lstType.Remove((ActivityType)type);
                              }
                              if (m_dicTabData.ContainsKey(type))
                               {
                                   m_dicTabData.Remove(type);
                               }
                          }
                          
                       }
                   }
               }
           }
        }
    }
    void RegisterEvent(bool reg)
    {
        if (reg)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, OnEvent);
        }
    }

    public void OnGetRewardSuccess(stRequstRechargeRewardPropertyUserCmd_CS cmd)
    {
        if (cmd.state == 1)
        {
            TipsManager.Instance.ShowTips("奖励领取成功");
        }
    }
    public void IsBuyGrowth(stBuyGrowGoldPropertyUserCmd_CS cmd)
    {
        bool value = cmd.state == 1;
        IsBuy = value;
        UpdateActivityData();
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ISBUYGROWTH, null);
    }
    public void RecieveSingleData(stRequstRechargePropertyUserCmd_CS cmd)
    {
        //消费
        if (cmd.type == 0)
        {
            DayCost = cmd.daymoney;
            AllCost = cmd.allmoney;
        }
        else if (cmd.type == 1)
        {
            MoneyAreCost.Add(cmd.signalmoney);
            DayRecharge = cmd.daymoney;
            AllRecharge = cmd.allmoney;

            //单日累计充值人民币
            this.m_dayAllRMB = cmd.dayrmb;

            //周充值
            for (int i = 0; i < cmd.week.Count; i++)
            {
                for (int j = 0; j < this.m_lstWeekRech.Count; j++)
                {
                    if (this.m_lstWeekRech[j].money == cmd.week[i].money)
                    {
                        this.m_lstWeekRech[j].times = cmd.week[i].times;
                    }
                }
            }
        }


        UpdateActivityData();

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.RECHARGESINGLEDATA, null);
        if (cmd.IsFirst == 1)
        {
            Client.stFirstRecharge fir = new Client.stFirstRecharge();
            fir.rechargeId = cmd.id;
            DataManager.Manager<Mall_HuangLingManager>().AlreadyFirstRecharge.Add(cmd.id);
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ISFIRSTRECHARGE, fir);
        }

        //累充
        if (IsWeekRechargeReach())
        {
            //有累计奖励达成
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.Accumulative,
                direction = (int)WarningDirection.None,
                bShowRed = true,
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        }
    }

    /// <summary>
    /// 周累计充值达到领取条件
    /// </summary>
    /// <returns></returns>
    public bool IsWeekRechargeReach()
    {
        bool reach = false;

        List<uint> weekRechLocalList = GameTableManager.Instance.GetGlobalConfigList<uint>("WeekAccumulative");
        for (int i = 0; i < weekRechLocalList.Count; i++)
        {
            table.RechargeCostDataBase rcDb = GameTableManager.Instance.GetTableItem<table.RechargeCostDataBase>(weekRechLocalList[i]);
            if (i < this.m_lstWeekRech.Count)
            {
                if (this.m_lstWeekRech[i].times >= rcDb.parameter2 && false == overidList.Contains(weekRechLocalList[i]))
                {
                    reach = true;
                }
            }
        }

        return reach;
    }

    public void RecieveRechargeData(stRechargeRewardListPropertyUserCmd_S cmd)
    {
        if (IDS != null)
        {
            IDS.Clear();
        }
        OverIDList = cmd.data;
        //是否购买了成长基金
        IsBuy = cmd.BuyGrowGold == 1;
        //今日充值列表
        MoneyAreCost = cmd.money;
        //今日充值总额
        DayRecharge = cmd.DayAllMoney;
        //今日累积消费
        DayCost = cmd.DayAccrued_Cost;
        //累积充值总额
        AllRecharge = cmd.Recharge_num;
        //累积消费总额
        AllCost = cmd.TimeAccrued_Cost;

        //今日充值人民币
        this.m_dayAllRMB = cmd.DayAllRMB;

        //周充值
        for (int i = 0; i < cmd.Week.Count; i++)
        {
            for (int j = 0; j < this.m_lstWeekRech.Count; j++)
            {
                if (this.m_lstWeekRech[j].money == cmd.Week[i].money)
                {
                    this.m_lstWeekRech[j].times = cmd.Week[i].times;
                }
            }
        }

        //已经领取的ID
        List<RechargeCostDataBase> allList = GameTableManager.Instance.GetTableList<RechargeCostDataBase>();
        List<uint> tList1 = new List<uint>();
        List<uint> tList2 = new List<uint>();
        for (int i = 0; i < allList.Count; i++)
        {
            if (allList[i].isPutInAccumulative)
            {
                continue;
            }
            if (!OverIDList.Contains(allList[i].ID))
            {
                tList1.Add(allList[i].ID);
            }
            else
            {
                tList2.Add(allList[i].ID);
            }
        }
        if (tList1.Count > 0)
        {
            IDS.AddRange(tList1);
        }
        if (tList2.Count > 0)
        {
            IDS.AddRange(tList2);
        }

        UpdateActivityData();

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.RECHARGEGETREWARD, null);


    }

    public void OnGetFirstRechargeReward(uint id)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.FirstRechargePanel))
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.FirstRechargePanel);
        }
        hadGotFirstRechargeReward = true;
        //神兵已经激活，需要关闭主界面按钮
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.FIRSTRECHARGESTATUS, hadGotFirstRechargeReward);
    }
    public void Initialize()
    {
        m_dicTextureName = new Dictionary<uint, uint>();
        lstActivityTabData = GameTableManager.Instance.GetTableList<RechargeCostDataBase>();
        for (int i = 0; i < lstActivityTabData.Count; i++)
        {
            if (lstActivityTabData[i].isPutInAccumulative)
            {
                continue;
            }
            uint type = lstActivityTabData[i].type;
            if (m_dicTabData.ContainsKey(type))
            {
                m_dicTabData[type].Add(lstActivityTabData[i]);
            }
            else
            {
                List<RechargeCostDataBase> lst = new List<RechargeCostDataBase>();
                lst.Add(lstActivityTabData[i]);
                m_dicTabData.Add(type, lst);
            }
            if (!m_dicTextureName.ContainsKey(type))
            {
                m_dicTextureName.Add(type, lstActivityTabData[i].textureResID);
            }

            if (!m_dic_Activity.ContainsKey(type))
            {
                m_dic_Activity.Add(type, new List<ActivityData>());
            }
            ActivityData data = new ActivityData()
            {
                id = lstActivityTabData[i].ID,
                total = lstActivityTabData[i].parameter,
                type = (ActivityType)lstActivityTabData[i].type,
                title = lstActivityTabData[i].title,
                rewards = lstActivityTabData[i].reward,
                desc = lstActivityTabData[i].desc,
            };
            m_dic_Activity[type].Add(data);
        }

        List<uint> weekRechLocalList = GameTableManager.Instance.GetGlobalConfigList<uint>("WeekAccumulative");
        for (int i = 0; i < weekRechLocalList.Count; i++)
        {
            table.RechargeCostDataBase rcDb = GameTableManager.Instance.GetTableItem<table.RechargeCostDataBase>(weekRechLocalList[i]);
            GameCmd.WeekRechTimes wrt = new GameCmd.WeekRechTimes();
            wrt.money = rcDb.parameter;
            wrt.times = 0;
            m_lstWeekRech.Add(wrt);
        }
        InitTypeList();
        RegisterEvent(true);
    }
    List<ActivityType> m_lstType = null;
    public void  InitTypeList()
    {
       if(m_dicTabData != null)
       {
           if (m_lstType == null)
           {
               m_lstType = new List<ActivityType>();
           }
           List<RechargeCostDataBase> list = new List<RechargeCostDataBase>();
           foreach (var i in m_dicTabData.Values)
           {
               if (i.Count >0)
              {
                  list.Add(i[0]);
              }
           }
           list.Sort(TypeSort);
           for (int i = 0; i < list.Count;i++ )
           {
               if (!m_lstType.Contains((ActivityType)list[i].type))
                 {
                     m_lstType.Add((ActivityType)list[i].type);
                 }
           }
       }
    }
    public List<ActivityType> GetTypeList() 
    {
        if (m_lstType == null)
        {
            m_lstType = new List<ActivityType>();
       
       }
        return m_lstType;
    }

    int TypeSort(RechargeCostDataBase a, RechargeCostDataBase b)
    {
        return (int)a.sortID - (int)b.sortID;
    }
    public void Process(float deltime)
    { }

    public uint GetTextureNameByActivityType(uint type)
    {
        uint resID = 0;
        if (m_dicTextureName.ContainsKey(type))
        {
            resID = m_dicTextureName[type];
        }
        return resID;
    }

    public void UpdateActivityData()
    {
        foreach (var it in m_dic_Activity)
        {
            uint key = it.Key;
            List<ActivityData> lstData = it.Value;
            for (int i = 0; i < lstData.Count; i++)
            {
                ActivityData data = lstData[i];
                if (OverIDList.Contains(data.id))
                {
                    data.state = ActivityState.Got;
                    continue;
                }
                if (dailyGiftList.Contains(data.id))
                {
                    data.state = ActivityState.Got;
                    continue;
                }
                bool matched = false;
                switch (data.type)
                {
                    case ActivityType.GrowthFund:
                        uint lv = (uint)MainPlayerHelper.GetPlayerLevel();
                        matched = lv >= data.total && isBuy;
                        data.process = lv;
                        break;
                    case ActivityType.SingleRechargeSingleDay:
                        matched = moneyAreCost.Contains(data.total);
                        data.process = moneyAreCost.Count != 0 ? data.total : 0;
                        break;
                    case ActivityType.AllRechargeSingleDay:
                        matched = dayRecharge >= data.total;
                        data.process = dayRecharge;
                        break;
                    case ActivityType.AllCostSingleDay:
                        matched = dayCost >= data.total;
                        data.process = dayCost;
                        break;
                    case ActivityType.AllRecharge:
                        matched = allRecharge >= data.total;
                        data.process = allRecharge;
                        break;
                    case ActivityType.AllCost:
                        matched = allCost >= data.total;
                        data.process = allCost;
                        break;
                }
                data.state = matched ? ActivityState.CanGet : ActivityState.None;
            }
            if (key != (uint)ActivityType.DailyGift)
            {
                lstData.Sort();
            }

        }
    }

    public bool HaveRewardCanGet()
    {
        Dictionary<uint, List<ActivityData>>.Enumerator iter = m_dic_Activity.GetEnumerator();
        while (iter.MoveNext())
        {
            List<ActivityData> lst = iter.Current.Value;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].state == ActivityState.CanGet)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public List<ActivityData> GetAchieveDataByType(ActivityType type)
    {
        if (m_dic_Activity.ContainsKey((uint)type))
        {
            return m_dic_Activity[(uint)type];
        }
        return new List<ActivityData>();
    }

    public bool HaveRewardCanGetByType(ActivityType type)
    {
        if (m_dic_Activity.ContainsKey((uint)type))
        {
            List<ActivityData> lsy = m_dic_Activity[(uint)type];
            for (int i = 0; i < lsy.Count; i++)
            {
                if (lsy[i].state == ActivityState.CanGet)
                {
                    return true;
                }
            }
        }
        return false;
    }
    #region 每日礼包
    List<uint> dailyGiftList = new List<uint>();
    public List<uint> DailyGiftList
    {
        set
        {
            value = dailyGiftList;
        }
        get
        {
            return dailyGiftList;

        }
    }
    public void OnRecieveDailyGiftData(List<uint> ids)
    {
        dailyGiftList = ids;
        UpdateActivityData();
    }
    public void OnBugDailyGift(uint id)
    {
        if (!dailyGiftList.Contains(id))
        {
            dailyGiftList.Add(id);
            UpdateActivityData();
            ValueUpdateEventArgs args = new ValueUpdateEventArgs("OnBuyDailyGift", null, id);
            DispatchValueUpdateEvent(args);
        }
    }

    /// <summary>
    /// 充值礼包
    /// </summary>
    /// <param name="giftID"></param>
    public void BuyDailyGift(uint giftID)
    {
        if (Application.isEditor)
        {
            NetService.Instance.Send(new stBuyDailyGiftDataUserCmd_CS() { id = giftID });
        }
        else
        {
            table.RechargeCostDataBase rechargeCostDb = GameTableManager.Instance.GetTableItem<table.RechargeCostDataBase>(giftID);
            if (null != rechargeCostDb)
            {
                DataManager.Manager<RechargeManager>().DoRecharge(rechargeCostDb.rechargeID);
            }
        }
    }

    /// <summary>
    ///充值基金
    /// </summary>
    public void BuyFoundation()
    {
        List<uint> foundationRechargeIDs = DataManager.Manager<RechargeManager>().GetRechargeIDsByType(RechargeManager.RechargeType.RT_Foundation);
        if (null == foundationRechargeIDs && foundationRechargeIDs.Count != 0)
        {
            TipsManager.Instance.ShowTips("购买基金失败，没有找到数据");
            return;
        }
        uint foundationRechargeID = foundationRechargeIDs[0];
        table.RechargeDataBase rechargeDB = GameTableManager.Instance.GetTableItem<table.RechargeDataBase>(foundationRechargeID);
        if (null == rechargeDB)
        {
            TipsManager.Instance.ShowTips("购买基金失败，没有找到数据");
            return;
        }
        if (Application.isEditor)
        {
            NetService.Instance.Send(new stBuyGrowGoldPropertyUserCmd_CS() { yuanbao = rechargeDB.kehuodianquan });
        }
        else
        {
            DataManager.Manager<RechargeManager>().DoRecharge(rechargeDB.dwID);
        }
    }
    #endregion

    private List<RecRetRewardData> rewRetReward = new List<RecRetRewardData>();
    public List<RecRetRewardData> RewRetReward
    {
        set
        {
            rewRetReward = value;
        }
        get
        {
            return rewRetReward;
        }
    }

    public bool RechargeRewRetClose
    {
        set;
        get;
    }
    #region  公测返利奖励
    public void OnRechargeRewRet(stRechargeRewRetListPropertyUserCmd_S cmd)
    {
        rewRetReward = cmd.data;
        int count = 0;
        for (int i = 0; i < rewRetReward.Count; i++)
        {
            if (rewRetReward[i].state == 1)
            {
                count++;
            }
        }
        RechargeRewRetClose = count == rewRetReward.Count;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.RETREWARDSTATUS, RechargeRewRetClose);
    }
    public void OnGetRechargeRewRet(ulong rewardid, uint ret)
    {
        bool changed = false;
        int count = 0;
        for (int i = 0; i < rewRetReward.Count; i++)
        {
            if (rewRetReward[i].rewardid == rewardid)
            {
                rewRetReward[i].state = ret;
                changed = true;
            }
            if (rewRetReward[i].state == 1)
            {
                count++; ;
            }
        }
        RechargeRewRetClose = count == rewRetReward.Count;
        if (RechargeRewRetClose)
        {
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.RETREWARDSTATUS, RechargeRewRetClose);
        }
        if (changed)
        {
            ValueUpdateEventArgs args = new ValueUpdateEventArgs("OnGetRechargeRewRet", rewardid, ret);
            DispatchValueUpdateEvent(args);
        }

    }

    public string GetScheduleIDByType(ActivityType type) 
    {
        string msg = "";
        if(m_dicTabData != null)
        {
            uint key = (uint)type;
            if (m_dicTabData.ContainsKey(key))
           {
               if (m_dicTabData[key].Count >0)
               {
                   uint sechdule = m_dicTabData[key][0].scheduleID;
                   msg = DataManager.Manager<DailyManager>().GetScheduleStrByID(sechdule);
               }
           }
        }
        return msg;
    }
    #endregion
}
