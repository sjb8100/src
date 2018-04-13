using System;
//********************************************************************
//	创建日期:	2016-11-15   14:48
//	文件名称:	DailyManager.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	日常系统数据管理
//********************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Engine;
using UnityEngine;
using GameCmd;
using table;
using Client;
public enum DailyDataType
{
    RiChangHuoDong = 1,
    XianShiHuoDong = 2,
    JiJiangKaiShi = 3,
    ZhouLi = 4,

}
public class DailyManager : BaseModuleData, IManager
{
    public List<string> DailyTabValues = new List<string>()
    {
        "日常活动","限时活动","即将开始","周历",
    };
    List<DailyDataBase> m_lstAllDaily = null;

    public Dictionary<uint, List<uint>> m_dicCalendar = new Dictionary<uint, List<uint>>();

    private uint dayHuntingCoin = 0;
    public uint DayHuntingCoin
    {
        set
        {
            dayHuntingCoin = value;
        }
        get
        {
            return dayHuntingCoin;
        }
    }
    public LivenessData liveness = new LivenessData();
    public LivenessData Liveness
    {
        get
        {
            return liveness;
        }
        set
        {
            liveness = value;
        }
    }
    public List<LivenessData> activeList = new List<LivenessData>();
    //     public List<LivenessData> ActiveList
    //     {
    //         get
    //         {
    //             return activeList;
    //         }
    //         set
    //         {
    //             activeList = value;
    //         }
    //     }
    //活动的ID
    public uint dataID = 0;
    public uint DataID
    {
        get
        {
            return dataID;
        }
        set
        {
            dataID = value;
        }
    }
    //完成次数
    public uint completedNum = 0;
    public uint CompleteNum
    {
        get
        {
            return completedNum;
        }
        set
        {
            completedNum = value;
        }
    }

    //单个活动活跃值
    public uint activeSingleValue = 0;
    public uint ActiveSingleValue
    {
        get
        {
            return activeSingleValue;
        }
        set
        {
            activeSingleValue = value;
        }
    }

    //活跃值总数
    public uint activeTotalValue = 0;
    public uint ActiveTotalValue
    {
        get
        {
            return activeTotalValue;
        }
        set
        {
            activeTotalValue = value;
        }
    }

    public uint rewardID = 0;
    public uint RewardID
    {

        get
        {
            return rewardID;
        }
        set
        {
            rewardID = value;
        }
    }
    public List<uint> rewardBoxList = new List<uint>();
    public List<uint> RewardBoxList
    {
        get
        {
            return rewardBoxList;
        }
        set
        {
            rewardBoxList = value;
        }
    }
    private List<uint> canGetBoxList = new List<uint>();
    public List<uint> CanGetBoxList
    {
        get
        {
            return canGetBoxList;
        }
        set
        {
            canGetBoxList = value;
        }

    }
    public Dictionary<uint, LivenessData> ActiveDic = new Dictionary<uint, LivenessData>();

    public float Verify_CD { get; set; }
    public long LeftScheduleTime { get; set; }

    public uint HuntingCoinLimit { get; set; }
    public bool isBindFinish { get; set; }
    public void Reset(bool depthClearData = false)
    {
        liveness = new LivenessData();
        activeList.Clear();
        dataID = 0;
        completedNum = 0;
        activeSingleValue = 0;
        activeTotalValue = 0;
        rewardID = 0;
        rewardBoxList.Clear();
        canGetBoxList.Clear();
        ActiveDic.Clear();
    }
    public void ClearData()
    {

    }
    public void Initialize()
    {
        m_lstAllDaily = GameTableManager.Instance.GetTableList<DailyDataBase>();
        HuntingCoinLimit = GameTableManager.Instance.GetGlobalConfig<uint>("HuntingCoinLimit");
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, OnPlayerLoginSuccess);
        InitCalendar();

        m_dicDailyData.Add(DailyDataType.RiChangHuoDong, new List<LivenessData>());
        m_dicDailyData.Add(DailyDataType.XianShiHuoDong, new List<LivenessData>());
        m_dicDailyData.Add(DailyDataType.JiJiangKaiShi, new List<LivenessData>());
        m_dicDailyData.Add(DailyDataType.ZhouLi, new List<LivenessData>());
    }

    public void ResetCD()
    {
        int cd = GameTableManager.Instance.GetGlobalConfig<int>("PhoneCodeReqCD");
        Verify_CD = (float)cd;
    }
    private void OnPlayerLoginSuccess(int eventid, object cmd)
    {
        if (eventid == (int)Client.GameEventID.PLAYER_LOGIN_SUCCESS)
        {
            NetService.Instance.Send(new stRequestLivenessDataDataUserCmd_C());

        }
    }
    public void GetRewardBox(uint id)
    {
        RewardID = id;
        RewardBoxList.Add(id);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.DAILY_GETREWARDBOXOVER, null);
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Daily,
            direction = (int)WarningDirection.None,
            bShowRed = HaveRewardBoxCanGet(),
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);

    }
    public void RefreshSingleActivity(stRefreshLivenessDataUserCmd_S cmd)
    {
        DataID = cmd.type;
        ActiveTotalValue = cmd.liveness;
        LivenessData info = new LivenessData()
        {
            type = cmd.type,
            liveness_num = cmd.liveness_num,
            time = cmd.times
        };
        DailyDataBase table = GameTableManager.Instance.GetTableItem<DailyDataBase>(DataID);
        ListSort(activeList);
        ActiveDic[cmd.type] = info;
        for (int i = 0; i < activeList.Count; i++)
        {
            if (activeList[i].type == info.type)
            {
                activeList[i] = info;
            }
        }

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.DAILY_RESSINGLEDATA, null);
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Daily,
            direction = (int)WarningDirection.None,
            bShowRed = HaveRewardBoxCanGet(),
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }

    public void GetAllLivenessData(stLivenessDataDataUserCmd_S cmd)
    {
        Reset();
        activeList = cmd.data;
        ActiveTotalValue = cmd.liveness;
        RewardBoxList = cmd.case_id;
        ListSort(activeList);
        FormDailyData(DailyDataType.RiChangHuoDong);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.DAILY_RESALLDATA, null);
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Daily,
            direction = (int)WarningDirection.None,
            bShowRed = HaveRewardBoxCanGet(),
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }




    Dictionary<DailyDataType, List<LivenessData>> m_dicDailyData = new Dictionary<DailyDataType, List<LivenessData>>();
    public List<LivenessData> FormDailyData(DailyDataType type)
    {
        m_dicDailyData[DailyDataType.RiChangHuoDong].Clear();
        m_dicDailyData[DailyDataType.XianShiHuoDong].Clear();
        m_dicDailyData[DailyDataType.JiJiangKaiShi].Clear();
        m_dicDailyData[DailyDataType.ZhouLi].Clear();
        for (int i = 0; i < activeList.Count; i++)
        {
            DailyDataBase data = GameTableManager.Instance.GetTableItem<DailyDataBase>(activeList[i].type);
            if (data != null)
            {
                int level = MainPlayerHelper.GetPlayerLevel();
                if (level >= data.minLevel)
                {
                    if (data.type == 1)
                    {
                        if (m_dicDailyData.ContainsKey(DailyDataType.RiChangHuoDong))
                        {
                            m_dicDailyData[DailyDataType.RiChangHuoDong].Add(activeList[i]);
                        }
                    }
                    else if (data.type == 2)
                    {
                        m_dicDailyData[DailyDataType.XianShiHuoDong].Add(activeList[i]);
                    }
                    else
                    {
                        if (IsDailyScheduleInToday(data))
                        {
                            m_dicDailyData[DailyDataType.XianShiHuoDong].Add(activeList[i]);
                        }
                        else
                        {
                            m_dicDailyData[DailyDataType.JiJiangKaiShi].Add(activeList[i]);
                        }
                    }
                }
                else
                {
                    if (m_dicDailyData.ContainsKey(DailyDataType.JiJiangKaiShi))
                    {
                        m_dicDailyData[DailyDataType.JiJiangKaiShi].Add(activeList[i]);
                    }
                }
            }
        }

        return m_dicDailyData[type];
    }
    public DailyDataType GetDailyTypeByID(uint id)
    {
        Dictionary<DailyDataType, List<LivenessData>>.Enumerator iter = m_dicDailyData.GetEnumerator();
        while (iter.MoveNext())
        {
            List<LivenessData> list = iter.Current.Value;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].type == id)
                {
                    return iter.Current.Key;
                }
            }
        }
        return DailyDataType.RiChangHuoDong;
    }
    public bool IsNotFinished(DailyDataBase data)
    {
        if (ActiveDic.ContainsKey(data.id))
        {
            if (data.MaxTimes == 0 || (data.MaxTimes > ActiveDic[data.id].time && data.MaxTimes != 0))
            {
                return true;
            }
        }
        return false;
    }
    public void ExecuteSort()
    {
        ListSort(activeList);
    }

    List<LivenessData> tempList1 = new List<LivenessData>();
    List<LivenessData> tempList2 = new List<LivenessData>();
    List<LivenessData> tempList3 = new List<LivenessData>();
    List<LivenessData> tempList4 = new List<LivenessData>();
    List<LivenessData> tempList6 = new List<LivenessData>();


    List<LivenessData> openList = new List<LivenessData>();
    List<LivenessData> closeList = new List<LivenessData>();
    public void ListSort(List<LivenessData> list)
    {
        tempList1.Clear();
        tempList2.Clear();
        tempList3.Clear();
        tempList4.Clear();
        tempList6.Clear();
        openList.Clear();
        closeList.Clear();
        for (int i = 0; i < list.Count; i++)
        {
            DailyDataBase data = GameTableManager.Instance.GetTableItem<DailyDataBase>(list[i].type);
            if (data != null)
            {
                int level = MainPlayerHelper.GetPlayerLevel();
                if (level >= data.minLevel)
                {
                    openList.Add(list[i]);
                }
                else
                {
                    closeList.Add(list[i]);
                }
                if (!ActiveDic.ContainsKey(list[i].type))
                {
                    ActiveDic.Add(list[i].type, list[i]);
                }
            }

        }
        long leftSeconds = 0;
        for (int x = 0; x < openList.Count; x++)
        {
            DailyDataBase data = GameTableManager.Instance.GetTableItem<DailyDataBase>(openList[x].type);
            if (data != null)
            {
                if (data.MaxTimes == 0 || (data.MaxTimes > openList[x].time && data.MaxTimes != 0))
                {
                    bool InSchedule = DataManager.Manager<DailyManager>().UpdateDataLeftTime(data, out leftSeconds);
                    if (InSchedule)
                    {
                        if (data.recommend == 1)
                        {
                            tempList1.Add(openList[x]);
                        }
                        else if (data.recommend == 2)
                        {
                            tempList2.Add(openList[x]);
                        }
                        else
                        {
                            tempList3.Add(openList[x]);
                        }
                    }
                    else
                    {
                        tempList4.Add(openList[x]);
                    }
                }
                else
                {
                    tempList6.Add(openList[x]);
                }

            }
        }
        LivenessData temp = new LivenessData();
        for (int a = 0; a < closeList.Count; a++)
        {
            DailyDataBase data1 = GameTableManager.Instance.GetTableItem<DailyDataBase>(closeList[a].type);
            if (data1 != null)
            {
                for (int b = closeList.Count - 1; b > a; b--)
                {
                    DailyDataBase data2 = GameTableManager.Instance.GetTableItem<DailyDataBase>(closeList[b].type);
                    if (data2 != null)
                    {
                        if (data1.minLevel > data2.minLevel)
                        {
                            temp = closeList[a];
                            closeList[a] = closeList[b];
                            closeList[b] = temp;
                        }
                    }
                }
            }

        }
        activeList.Clear();
        activeList.AddRange(tempList1);
        activeList.AddRange(tempList2);
        activeList.AddRange(tempList3);
        activeList.AddRange(tempList4);
        activeList.AddRange(tempList6);
        activeList.AddRange(closeList);
    }
    bool HaveRewardBoxCanGet()
    {
        List<DailyAwardDataBase> l = GameTableManager.Instance.GetTableList<DailyAwardDataBase>();
        for (int i = 0; i < l.Count; i++)
        {

            if (ActiveTotalValue >= l[i].liveness)
            {
                canGetBoxList.Add(l[i].ID);
                if (!RewardBoxList.Contains(l[i].ID))
                {
                    return true;
                }
                else
                {
                    canGetBoxList.Remove(l[i].ID);
                }

            }
        }
        return false;
    }

    public void GetBindPhoneRes(stBindPhoneNumDataUserCmd_CS cmd)
    {
        Verify_CD = cmd.cd;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.GETVERIFYNUM, null);
        //绑定成功
        if (Verify_CD == 0 && cmd.type == BindPhoneCode.BindPhoneCode_Ret)
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ActiveTakePanel);
            TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText(LocalTextType.FeedBack_BindPhoneSuccess));
            isBindFinish = true;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.BINDPHONESUCESS, true);

        }
    }
    public Action<float> OnUpdateTimeEvent = null;
    public const float DAILYTIMEUPDATEGAP = 1f;
    private float next_refresh_left_time = 0;

    public void Process(float deltime)
    {
        if (Verify_CD > 0)
        {
            Verify_CD -= deltime;
            if (OnUpdateTimeEvent != null)
            {
                OnUpdateTimeEvent(Verify_CD);
            }
        }
        if (LeftScheduleTime > 0)
        {
            next_refresh_left_time -= deltime;
            if (next_refresh_left_time <= UnityEngine.Mathf.Epsilon)
            {
                LeftScheduleTime--;
                next_refresh_left_time = DAILYTIMEUPDATEGAP;
                ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateTime", null, LeftScheduleTime);
                DispatchValueUpdateEvent(arg);
            }
        }
    }




    /// <summary>
    /// 构建日程信息
    /// </summary>
    public void StructScheduleInfo(DailyDataBase daily)
    {
        scheduleInfos = new List<ScheduleDefine.ScheduleLocalData>();
        table.DailyDataBase dailyData = daily;
        if (null != dailyData && !string.IsNullOrEmpty(dailyData.ScheduleId))
        {
            string[] scheduleIdArray = dailyData.ScheduleId.Split(new char[] { '_' });
            if (null != scheduleIdArray)
            {

                ScheduleDefine.ScheduleLocalData scheduleTemp = null;
                for (int i = 0; i < scheduleIdArray.Length; i++)
                {
                    scheduleTemp = new ScheduleDefine.ScheduleLocalData(uint.Parse(scheduleIdArray[i].Trim()));
                    scheduleInfos.Add(scheduleTemp);
                }

                if (!m_scheduleDic.ContainsKey(daily.id))
                {
                    m_scheduleDic.Add(daily.id, scheduleInfos);
                }
            }
        }
    }

    /// <summary>
    /// 日程信息
    /// </summary>
    private List<ScheduleDefine.ScheduleLocalData> scheduleInfos = new List<ScheduleDefine.ScheduleLocalData>();

    public bool IsTimeInSchedule(uint dailyID, long seconds, out long leftSeconds)
    {
        return IsTimeInSchedule(dailyID, DateTimeHelper.TransformServerTime2DateTime(seconds), out leftSeconds);
    }

    /// <summary>
    /// 是否时间seconds在当前日常日程内
    /// </summary>
    /// <param name="date">当前时间</param>
    /// <param name="leftSeconds">如果在日程内，返回最大剩余时间
    /// ，如果不在日程内返回下一个日程最小等待时间</param>
    /// <returns></returns>
    public bool IsTimeInSchedule(uint dailyID, System.DateTime date, out long leftSeconds)
    {
        bool inSchedule = false;
        leftSeconds = 0;
        long scheduleLeftTime = 0;
        long nextScheduleTime = 0;
        if (!m_scheduleDic.ContainsKey(dailyID))
        {
            DailyDataBase data = GameTableManager.Instance.GetTableItem<DailyDataBase>(dailyID);
            StructScheduleInfo(data);
        }
        else
        {
            scheduleInfos = m_scheduleDic[dailyID];
        }

        if (null != scheduleInfos && scheduleInfos.Count > 0)
        {
            for (int i = 0; i < scheduleInfos.Count; i++)
            {
                if (scheduleInfos[i].IsInSchedule(date, out leftSeconds))
                {
                    inSchedule = true;
                    if (leftSeconds > scheduleLeftTime)
                    {
                        scheduleLeftTime = leftSeconds;
                    }
                }
                else if (leftSeconds > 0 && leftSeconds > nextScheduleTime)
                {
                    nextScheduleTime = leftSeconds;
                }
            }
        }
        leftSeconds = (inSchedule) ? scheduleLeftTime : nextScheduleTime;
        //Engine.Utility.Log.Error("ID:" + dailyID + "---bool:" + inSchedule + "---DateTime:" + date + "---leftSeconds:" + leftSeconds);
        return inSchedule;
    }


    /// <summary>
    /// 刷新日常的日程时间
    /// </summary>
    public bool UpdateDataLeftTime(DailyDataBase data, out long leftSeconds)
    {
        StructScheduleInfo(data);
        bool isInSchedule = IsTimeInSchedule(data.id, DateTimeHelper.Instance.Now, out leftSeconds);
        if (isInSchedule)
        {
            LeftScheduleTime = 0;
        }
        else
        {
            LeftScheduleTime = leftSeconds;
        }

        return isInSchedule;
    }

    private Dictionary<uint, List<ScheduleDefine.ScheduleLocalData>> m_scheduleDic = new Dictionary<uint, List<ScheduleDefine.ScheduleLocalData>>();

    public string GetCloserScheduleTimeByID(uint dailyID)
    {
        string closerTime = "";
        if (m_scheduleDic.ContainsKey(dailyID))
        {
            if (m_scheduleDic[dailyID] != null)
            {
                List<ScheduleDefine.ScheduleLocalData> list = m_scheduleDic[dailyID];
                long leftSeconds = 0;
                long tempSeconds = 0;

                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].IsInSchedule(DateTimeHelper.Instance.Now, out tempSeconds))
                    {
                    }
                    if (leftSeconds > tempSeconds || leftSeconds == 0)
                    {
                        leftSeconds = tempSeconds;
                        if (leftSeconds / 86400 >= 1)
                        {
                            closerTime = string.Format("{0}天后开启", leftSeconds / 86400);
                        }
                        else
                        {
                            DateTime one = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(DateTimeHelper.Instance.Now + leftSeconds);
                            DateTime two = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(DateTimeHelper.Instance.Now);
                            if (one.Day != two.Day)
                            {
                                closerTime = "1天后开启";
                            }
                            else
                            {
                                closerTime = string.Format("{0:D2}:{1:D2}开启", list[i].TabData.startHour, list[i].TabData.startMin);
                            }


                        }

                    }
                }
            }
        }
        return closerTime;
    }
    public bool IsDailyScheduleInToday(DailyDataBase data)
    {
        if (m_scheduleDic.ContainsKey(data.id))
        {
            if (m_scheduleDic[data.id] != null)
            {
                DateTime dt = DateTimeHelper.TransformServerTime2DateTime(DateTimeHelper.Instance.Now);
                List<ScheduleDefine.ScheduleLocalData> list = m_scheduleDic[data.id];
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].TabData.week == (uint)dt.DayOfWeek && list[i].TabData.cycleType == 3)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    void InitCalendar()
    {
        List<DailyCalendarDataBase> tables = GameTableManager.Instance.GetTableList<DailyCalendarDataBase>();
        if (tables != null)
        {

            for (int i = 0; i < tables.Count; i++)
            {
                if (!m_dicCalendar.ContainsKey(tables[i].dayIndex))
                {
                    string actIDs = tables[i].active;
                    if (!string.IsNullOrEmpty(actIDs))
                    {
                        uint dailyID = 0;
                        string[] args = actIDs.Split('_');

                        for (uint j = 0; j < args.Length; j++)
                        {
                            if (uint.TryParse(args[j], out dailyID))
                            {
                                if (m_dicCalendar.ContainsKey(tables[i].dayIndex))
                                {
                                    m_dicCalendar[tables[i].dayIndex].Add(dailyID);
                                }
                                else
                                {
                                    List<uint> list = new List<uint>();
                                    list.Add(dailyID);
                                    m_dicCalendar.Add(tables[i].dayIndex, list);
                                }
                            }
                        }

                    }

                }
            }
        }
    }

    public List<uint> GetDailyActivityIDs(uint dayIndex)
    {
        if (m_dicCalendar.ContainsKey(dayIndex))
        {
            return m_dicCalendar[dayIndex];
        }
        return new List<uint>();
    }

    /// <summary>
    /// 判断当前时刻是否在日程内
    /// </summary>
    /// <param name="schedulID"></param>
    /// <returns></returns>
    public bool BoolNowIsInSchedule(ScheduleDefine.ScheduleLocalData scheduleTemp, out long leftSeconds)
    {
        bool isInSchedule = false;
        leftSeconds = 0;
        long scheduleLeftTime = 0;
        long nextScheduleTime = 0;
        if (scheduleTemp != null)
        {
            if (scheduleTemp.IsInSchedule(DateTimeHelper.Instance.Now, out leftSeconds))
            {
                isInSchedule = true;
                if (leftSeconds > scheduleLeftTime)
                {
                    scheduleLeftTime = leftSeconds;
                }
            }
            else if (leftSeconds > 0 && leftSeconds > nextScheduleTime)
            {
                nextScheduleTime = leftSeconds;
            }
        }
        leftSeconds = (isInSchedule) ? scheduleLeftTime : nextScheduleTime;
        return isInSchedule;
    }


    #region 聚宝boss

    Dictionary<int, List<uint>> m_dicTreasureBoss = null;

    public List<uint> GetTreasureBossList(int tab)
    {
        if (m_dicTreasureBoss == null)
        {
            m_dicTreasureBoss = new Dictionary<int, List<uint>>();
        }
        if (!m_dicTreasureBoss.ContainsKey(tab))
        {
            List<TreasureBossDataBase> list = GameTableManager.Instance.GetTableList<TreasureBossDataBase>();
            for (int i = 0; i < list.Count; i++)
            {
                if (!m_dicTreasureBoss.ContainsKey((int)list[i].bossType))
                {
                    List<uint> result = new List<uint>();
                    result.Add(list[i].ID);
                    m_dicTreasureBoss.Add((int)list[i].bossType, result);
                }
                else
                {
                    m_dicTreasureBoss[(int)list[i].bossType].Add(list[i].ID);
                }
            }
        }
        return m_dicTreasureBoss.ContainsKey(tab) ? m_dicTreasureBoss[tab] : new List<uint>();
    }

    #endregion



    Dictionary<uint, ScheduleDefine.ScheduleLocalData> m_dicScheduleData = null;
    public string GetScheduleStrByID(uint scheduleid)
    {
        string str = "";
        if (m_dicScheduleData == null)
        {
            m_dicScheduleData = new Dictionary<uint, ScheduleDefine.ScheduleLocalData>();
        }
        if (m_dicScheduleData != null)
        {
            ScheduleDefine.ScheduleLocalData schedule = null;
            if (!m_dicScheduleData.ContainsKey(scheduleid))
            {
                schedule = new ScheduleDefine.ScheduleLocalData(scheduleid);
                m_dicScheduleData.Add(scheduleid, schedule);
            }
            else
            {
                schedule = m_dicScheduleData[scheduleid];
            }
            if (schedule != null)
            {
                if (schedule.TabData != null)
                {
                    str = string.Format("{0}年{1}月{2}日{3}时{4}分--{5}年{6}月{7}日{8}时{9}分",
                      schedule.TabData.startYear,
                      schedule.TabData.startMonth,
                      schedule.TabData.startDay,
                      schedule.TabData.startHour,
                      schedule.TabData.startMin,

                      schedule.TabData.endYear,
                      schedule.TabData.endMonth,
                      schedule.TabData.endDay,
                      schedule.TabData.endHour,
                      schedule.TabData.endMin
                      );
                }

            }
        }
        return str;
    }

    Dictionary<uint, ScheduleDefine.ScheduleLocalData> m_dic_schedule = null;
    public bool InSchedule(uint scheduleid)
    {
        long leftTime;
        return InSchedule(scheduleid, out leftTime);
    }

    public bool InSchedule(uint scheduleid, out long leftSeconds)
    {
        leftSeconds = 0;
        bool inSche = false;
        if (m_dic_schedule == null)
        {
            m_dic_schedule = new Dictionary<uint, ScheduleDefine.ScheduleLocalData>();
        }
        ScheduleDefine.ScheduleLocalData scheduleInfos = null;
        if (m_dic_schedule.ContainsKey(scheduleid))
        {
            scheduleInfos = m_dic_schedule[scheduleid];
        }
        else
        {
            scheduleInfos = new ScheduleDefine.ScheduleLocalData(scheduleid);
            m_dic_schedule.Add(scheduleid, scheduleInfos);
        }
        if (scheduleInfos != null)
        {
            inSche = scheduleInfos.IsInSchedule(DateTimeHelper.Instance.Now, out leftSeconds);
        }
        return inSche;
    }

    /// <summary>
    /// 获取当天日程总时间
    /// </summary>
    /// <param name="scheduleid"></param>
    /// <returns></returns>
    public long GetScheduleTotleSecondsByID(uint scheduleid)
    {
        if (m_dicScheduleData == null)
        {
            m_dicScheduleData = new Dictionary<uint, ScheduleDefine.ScheduleLocalData>();
        }

        if (m_dicScheduleData != null)
        {
            ScheduleDefine.ScheduleLocalData schedule = null;
            if (!m_dicScheduleData.ContainsKey(scheduleid))
            {
                schedule = new ScheduleDefine.ScheduleLocalData(scheduleid);
                m_dicScheduleData.Add(scheduleid, schedule);
            }
            else
            {
                schedule = m_dicScheduleData[scheduleid];
            }
            if (schedule != null)
            {
                TimeSpan startTime = new TimeSpan((int)schedule.TabData.startHour, (int)schedule.TabData.startMin, 0);
                TimeSpan endTime = new TimeSpan((int)schedule.TabData.endHour, (int)schedule.TabData.endMin, 0);
                TimeSpan totleTime = endTime - startTime;
                return (long)totleTime.TotalSeconds;
            }
        }

        return 0;
    }
}