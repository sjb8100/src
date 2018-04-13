using System;
using System.Collections.Generic;
using GameCmd;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
using Engine;
using Client;
public enum WelfareType
{
    None = 0,
    Month = 1,//月签到
    OnLine = 2,
    SevenDay = 3,
    RoleLevel = 4,
    OpenSever = 5,
    BindGift = 6,
    RewardFind = 7,
    FriendInvite =8,
    RushLevel = 9, //冲级礼包
    CDKey = 10,   
    CollectWord =11,
    End,
}

public class WelfareItem{
    public uint itemid;
    public uint itemNum;
    public string name = "";
}

public class WelfareBaseData 
{
//     public enum State
//     {
//         NONE = 2,//不可领取
//         GOT = 1,//已经领取
//         CANGET = 3,//可以领取
//     }
    //1福利礼包 2 新服七日活动
    public int DataType = 0;
    public uint id;
    public string title;
    public List<WelfareItem> lstWelfareItems = new List<WelfareItem>();
    public QuickLevState state = QuickLevState.QuickLevState_None;
    public WelfareType welfareType = WelfareType.None;
    /// <summary>
    /// 当前进度
    /// </summary>
    public int process;
    public int total;
}
public class RewardFindItems
{
    public uint itemID;
    public uint itemNum;
}
public class RewardFindData : IComparable<RewardFindData>
{ 
    public enum RewardFindState
    {
        CanGet =1,
        Got =2, 
    }
    public uint id;
    public uint left_time;
    public uint total_time;
    public RewardFindState state = RewardFindState.CanGet;
    public List<RewardFindItems> list = new List<RewardFindItems>();
    public int CompareTo(RewardFindData other) 
    {
        int astate = (int)state;
        int bstate = (int)other.state;
        if (astate < bstate)
        {
            return -1;
        }
        else if (astate > bstate)
        {
            return 1;
        }
        return id.CompareTo(other.id);
    }

}
public enum InviteType 
{
    None =0,
    Inviter =1,
    Invited=2,
    InvitedRecharge = 3,
}
public class WelfareData : WelfareBaseData, IComparable<WelfareData>
{
    public WelfareType type;
    public InviteType inviteType = InviteType.None;
    public CollectType collectType = CollectType.None;
    public uint param;
    public uint param2;
    //专用于玩家招募  或者  收集文字
    public uint total2;
    public List<WelfareItem> collectWords = null;
    public int CompareTo(WelfareData other)
    {
        int astate = (int)state;

        int bstate = (int)other.state;
        if (other.type != WelfareType.Month && this.type != WelfareType.Month)
        {
            if (astate > bstate)
            {
                return -1;
            }
            else if (astate < bstate)
            {
                return 1;
            }
        }
        return id.CompareTo(other.id);
    }

    public int SortID;
}
public enum CollectType
{
    None = 0,  //不限制
    All,       //总限制次数
    Day,       //日限制次数
}

/// <summary>
/// 开服七天
/// </summary>
public class SevenDayWelfare : WelfareBaseData, IComparable<SevenDayWelfare>
{
    /// <summary>
    /// 活动类型
    /// </summary>
    public uint nType = 0;
    public uint nDay = 0;
    public bool bopen = false;//是否开放
    public uint param1;
    public uint param2;
    public uint point;
    public uint SortID;
    public int CompareTo(SevenDayWelfare other)
    {
        int astate = (int)state;

        int bstate = (int)other.state;
        if (astate > bstate)
        {
            return -1;
        }
        else if (astate < bstate)
        {
            return 1;
        }
        return id.CompareTo(other.id);
    }
}

partial class WelfareManager : BaseModuleData,IManager,Engine.Utility.ITimer
{

    Dictionary<uint, List<WelfareData>> m_dicWelfare = null;
    Dictionary<uint, uint> m_dicWelfareScheduleID = null;
    Dictionary<uint, uint> m_dicTextureName = null;
    public Dictionary<uint, List<SevenDayWelfare>> m_dicSevenDay = null;
    private List<uint> m_lstWelfareGotId = new List<uint>();
   // public List<uint> m_lstWelfareCanGetId = new List<uint>();
    List<uint> welfareOnlineList = new  List<uint>();
    List<uint> welfareRoleLevelList = new List<uint>();
    List<uint> sevenDayOnlineList = new  List<uint>();

    List<OpenServerDataBase> OpenServerList = null;
    List<ArtifactDataBase> ArtifactList = null;
    const int TIMER_ID = 100;
    uint Wel_OpenLevel = 11;
    uint Sev_OpenLevel = 12;
    uint OpenSeverLevel = 17;
    bool EnterGameReadey = false;

    List<table.WelfareDataBase> lstWelfare = null;
    #region 福利

    /// <summary>
    /// 已签到天数
    /// </summary>
    public uint SignDay { get; set; }
    /// <summary>
    /// 签到当前天数
    /// </summary>
    public uint CurrDay { get; set; }
    /// <summary>
    /// 当天是否已签到
    /// </summary>
    public bool IsSignCurrDay { get; set; }

    //七天登录
    /// <summary>
    /// 玩家当天的累计在线时间
    /// </summary>
    public uint CurrDayOnlineTime { get; set; }
    ///一周内登陆天数
    public uint LoginTimesOfWeek { get; set; }
    /// <summary>
    /// 新服登录次数
    /// </summary>
    public uint NewServerLoginTimes { get; set; }

#endregion

    #region 新服七天活动
    private uint serverOpenMaxDay = 0;
    /// <summary>
    /// 新服七日最大天数
    /// </summary>
    public uint ServerOpenMaxDay 
    {
        get 
        {
            return serverOpenMaxDay -1;
        }
        set
        {
            serverOpenMaxDay = value;
        }
    
    }
    /// <summary>
    /// 新服七日当前天数
    /// </summary>
    public uint ServerOpenCurrDay { get; set; }
    /// <summary>
    /// 活动剩余时间
    /// </summary>
    public uint Remain_Time { get; set; }
    public uint onlineWelfareTime { get; set; }
    public uint onlineSevenDayTime { get; set; }
    /// <summary>
    /// 玩家当天在线时间
    /// </summary>
    public uint User_OnlineTime { get; set; }
    /// <summary>
    /// 数据发送过来时游戏运行的时间
    /// </summary>
    public int LoginTime { get; set; }
    /// <summary>
    /// 已经领取的福利
    /// </summary>
    public List<uint> m_lstServerOpenGotReward = new List<uint>();
    //新服七天倒计时回调
    public Action<uint> OnUpdateTimeEvent = null;

    //当前活跃点数
    public uint CurActiveNum { get; set; }
    //总活跃点数
    public uint TotalActiveNum { get; set; }

    //是不是已经领取
    private List<uint> BindRewardIDs = new List<uint>();
    public List<uint> m_lstBindRewardIDs
    {
        get
        {
            return BindRewardIDs;
        }

        set 
        {
            BindRewardIDs = value;
        }
    
    }

    //七天乐开启状态  --  true   符合开启条件   false  -- 七天乐不再开启
    public bool SevenDayOpenState { get; set; }
    public bool CanGetBindReward { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public bool CanGetRushLevel { get; set; }
    //已经被招募了哇
    public bool HadBeenInvited { get; set; }

    public uint Other_Total_Recharge { get; set; }
    //招募人的id
    public uint InviterID { get; set; }
    public uint InviterLv { get; set; }
      public uint InviterProfession { get; set; }
      public string InviterName { get; set; }

    //已经满足条件的id集合
    public List<uint> canGetIDs = new List<uint>();
    public List<uint> CanGetIDs
    {
        get
        {
            return canGetIDs;
        }

        set
        {
            canGetIDs = value;
        }

    }


#endregion

    #region 奖励找回
    /// <summary>
    /// 彼时等级
    /// </summary>
    public uint Previous_Lv { get; set; }
    private List<RewardFindData> m_lstReward = new List<RewardFindData>();
    public List<RewardFindData> M_lstReward 
    {
        get 
        {
            return m_lstReward;
        }
        set 
        {
            m_lstReward = value;
        }   
    }

    private List<BaseUserInfo> inviterInfos = new List<BaseUserInfo>();
    public List<BaseUserInfo> InviterInfos
    {
        get 
        {
            return inviterInfos;
        }
        set 
        {
            inviterInfos = value;
        }
    }

//     private List<CollectData> m_lstCollect = new List<CollectData>();
//     public List<CollectData> M_lstCollect
//     {
//         get
//         {
//             return m_lstCollect;
//         }
//         set
//         {
//             m_lstCollect = value;
//         }
//     }
    #endregion

    public void ClearData()
    {
        RegisterEvent(false);
    }
    public void Initialize()
    {
        LoginTimesOfWeek = 0;
        CurrDayOnlineTime = 0;
        NewServerLoginTimes = 0;
        IsSignCurrDay = false;
        SignDay = 0;
        CurrDay = 0;
        ServerOpenCurrDay = 0;
        CurActiveNum = 0;
        m_dicWelfare = new Dictionary<uint, List<WelfareData>>();
        m_dicWelfareScheduleID = new Dictionary<uint, uint>();
        m_dicTextureName = new Dictionary<uint, uint>();
        m_dicSevenDay = new Dictionary<uint, List<SevenDayWelfare>>();     
        m_lstWelfareGotId.Clear();
        welfareOnlineList.Clear(); 
        welfareRoleLevelList.Clear(); 
        sevenDayOnlineList.Clear();
        m_lstServerOpenGotReward.Clear();
        BindRewardIDs.Clear();
        canGetIDs.Clear();
        openPackageList.Clear();
        InitTable();
        RegisterEvent(true);
    }


    public void GlobalEventHandler(int nEventID, object param) 
    {
        switch (nEventID)
        {
            case (int)Client.GameEventID.SYSTEM_GAME_READY:
                {
                    EnterGameReadey = true;
                    if(lstWelfare != null)
                    {
                        for (int i = 0; i < lstWelfare.Count;i++ )
                        {
                            if (lstWelfare[i].scheduleID != 0)
                            {
                                uint schedule = lstWelfare[i].scheduleID;
                                if (!m_dicWelfareScheduleID.ContainsKey(lstWelfare[i].dwType))
                                {
                                    m_dicWelfareScheduleID.Add(lstWelfare[i].dwType, schedule);
                                }
                                if (m_dicWelfare.ContainsKey(lstWelfare[i].dwType))
                                {
                                    if (!DataManager.Manager<DailyManager>().InSchedule(schedule))
                                    {
                                        m_dicWelfare.Remove(lstWelfare[i].dwType);
                                        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
                                        {
                                            modelID = (int)WarningEnum.WELFARE,
                                            direction = (int)WarningDirection.None,
                                            bShowRed = HasRewardCanGet(1) || HasRewardCanReBack(),
                                        };
                                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
                                    }
                                }
                            }
                        }
                    }
                }
                break;
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                {
                    if (null != param && param is ItemDefine.UpdateItemPassData)
                    {
                        ItemDefine.UpdateItemPassData passData = param as ItemDefine.UpdateItemPassData;
                        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(passData.BaseId);
                        if (baseItem.IsCollectWord)
                        {
                            if (m_lst_CollectWordData != null)
                            {
                                 OnRecieveCollectData(m_lst_CollectWordData);
                                 ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateCollectWord", null, null);
                                 DispatchValueUpdateEvent(arg);
                                 stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
                                 {
                                     modelID = (int)WarningEnum.WELFARE,
                                     direction = (int)WarningDirection.Right,
                                     bShowRed = HasRewardInType(1, (uint)WelfareType.CollectWord),
                                 };
                                 Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
                            }
                            
                        }
                    }
                }
                break;
        }
    }
    public void Reset(bool depthClearData = false)
    {
        Engine.Utility.TimerAxis.Instance().KillTimer(TIMER_ID, this);
        Initialize();
    }

    public void Process(float deltaTime)
    {

    }

    void InitTable()
    {
        lstWelfare = GameTableManager.Instance.GetTableList<table.WelfareDataBase>();
        for (int i = 0,imax = lstWelfare.Count; i < imax; i++)
        {
            table.WelfareDataBase welfaredata = lstWelfare[i];
            uint type = welfaredata.dwType ;
            if (!m_dicWelfare.ContainsKey(type))
            {
                m_dicWelfare.Add(type, new List<WelfareData>());
            }
         
            WelfareData data = new WelfareData()
            {
                DataType = 1,
                id = welfaredata.dwID,
                type = (WelfareType)welfaredata.dwType,
                welfareType = (WelfareType)welfaredata.dwType,
                inviteType = (InviteType)welfaredata.childType,
                param = welfaredata.dwParam,
                param2 = welfaredata.dwParam2,
                title = welfaredata.strDes,
                total = (WelfareType)welfaredata.dwType == WelfareType.OnLine ? (int)(welfaredata.dwParam) : (int)welfaredata.dwParam,
                total2 = (InviteType)welfaredata.childType != InviteType.None ? (welfaredata.dwParam2) :0,
                SortID =(int)welfaredata.sortID,

            };

            ParaseReward(welfaredata.strReward,ref data.lstWelfareItems);
            if ((WelfareType)type == WelfareType.CollectWord)
            {
                if (data.collectWords == null)
                {
                    data.collectWords = new List<WelfareItem>();
                }
                ParseCollectWordData(welfaredata.dwParam, ref data.collectWords, ref data.collectType,ref data.param2);
            }
            m_dicWelfare[type].Add(data);

             if (!m_dicTextureName.ContainsKey(type))
             {
                 m_dicTextureName.Add(type, welfaredata.textureResID);
             }
         
        }      
        //sevenDay

        List<table.SevenDataBase> lstSevenDay = GameTableManager.Instance.GetTableList<table.SevenDataBase>();
        uint num = 0;
        for (int i = 0,imax = lstSevenDay.Count; i < imax; i++)
        {
            if (!m_dicSevenDay.ContainsKey(lstSevenDay[i].DateID))
            {
                m_dicSevenDay.Add(lstSevenDay[i].DateID, new List<SevenDayWelfare>());
            }

            SevenDayWelfare data = new SevenDayWelfare()
            {
                DataType = 2,
                nType = lstSevenDay[i].TypeID,
                title = lstSevenDay[i].strDes,
                SortID = lstSevenDay[i].SortID,
                id = lstSevenDay[i].ID,
                nDay = lstSevenDay[i].DateID,
                param1 = lstSevenDay[i].parameter1,
                param2 = lstSevenDay[i].parameter2,
                total = (int)lstSevenDay[i].parameter1,
                point =  lstSevenDay[i].point,
            };
            ParaseReward(lstSevenDay[i].reward, ref data.lstWelfareItems);
            m_dicSevenDay[lstSevenDay[i].DateID].Add(data);

            if (serverOpenMaxDay < lstSevenDay[i].DateID)
            {
                serverOpenMaxDay = lstSevenDay[i].DateID;
            }
            num += lstSevenDay[i].point;
        }
        TotalActiveNum = num;
        Wel_OpenLevel = GameTableManager.Instance.GetGlobalConfig<uint>("Welfare_OpenLevel");
        Sev_OpenLevel = GameTableManager.Instance.GetGlobalConfig<uint>("Seven_OpenLevel");
        OpenSeverLevel = GameTableManager.Instance.GetGlobalConfig<uint>("OpenServerLevel");

        OpenServerList = GameTableManager.Instance.GetTableList<OpenServerDataBase>();
        ArtifactList = GameTableManager.Instance.GetTableList<ArtifactDataBase>();
    }
/// <summary>
/// 
/// </summary>
/// <param name="collectid">福利表的参数1对应集字映射表格</param>
/// <param name="wordsData">兑换列表</param>
/// <param name="type">兑换限制类型</param>
/// <param name="gotTimes">可以兑换这个几次</param>
    void ParseCollectWordData(uint collectid,ref  List<WelfareItem> wordsData,ref CollectType type,ref uint gotTimes) 
    {
   
        CollectWordDataBase tab = GameTableManager.Instance.GetTableItem<CollectWordDataBase>(collectid);
        if (tab != null)
        {
            ParaseReward(tab.rewardsList,ref wordsData);
            type = (CollectType)tab.type;
            gotTimes = tab.times;
        }
    }
    public bool HasRewardCanGet(int type)
    {
        Client.IPlayer player = MainPlayerHelper.GetMainPlayer();
        int level = 0;
        if (player != null)
        {
            level = player.GetProp((int)Client.CreatureProp.Level);
            if (type == 1 && level < Wel_OpenLevel || type == 2 && level < Sev_OpenLevel)
            {
                return false;
            }

        }
        if (type == 1)
        {
            bool hasWelfareRewardCanGet = false;
            if (m_dicWelfare.Count > 0)
            {
                foreach (var iSevenData in m_dicWelfare)
                {
                    for (int i = 0; i < iSevenData.Value.Count; i++)
                    {
                        if (iSevenData.Value[i].state == QuickLevState.QuickLevState_CanGet)
                        {
                            hasWelfareRewardCanGet = true;
                        }
                    }
                }
            }
            return hasWelfareRewardCanGet;
        }
        else if(type ==2) 
        {
            bool hasSevenRewardCanGet = false;
            if (m_dicSevenDay.Count>0)
            {
                foreach (var iSevenData in m_dicSevenDay)
                {
                    if (iSevenData.Key <= DataManager.Manager<WelfareManager>().ServerOpenCurrDay || iSevenData.Key == 6)
                    {
                        for (int i = 0; i < iSevenData.Value.Count;i++ )
                        {
                            if (iSevenData.Value[i].state == QuickLevState.QuickLevState_CanGet)
                            {
                                hasSevenRewardCanGet = true;
                            }
                        }
                  
                    }
                   
                }           
            }
            return hasSevenRewardCanGet;
        }

        return false;
    }
    public bool HasRewardInType(int type,uint dicIndex) 
    {
        Client.IPlayer player = MainPlayerHelper.GetMainPlayer();
        int level = 0;
        if (player != null)
        {
            level = player.GetProp((int)Client.CreatureProp.Level);
            if (type == 1 && level < Wel_OpenLevel || type == 2 && level < Sev_OpenLevel)
            {
                return false;
            }

        }
     
        if (type == 1)
        {
            if (!m_dicWelfare.ContainsKey(dicIndex))
            {
                return false;
            }

            bool hasWelfareRewardCanGet = false;
            List<WelfareData> list = m_dicWelfare[dicIndex];
            if (list.Count > 0)
            {             
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].state == QuickLevState.QuickLevState_CanGet)
                        {
                            hasWelfareRewardCanGet = true;
                        }
                    }               
            }
            return hasWelfareRewardCanGet;
        }
        else if (type == 2)
        {
            if (!m_dicSevenDay.ContainsKey(dicIndex))
            {
                return false;
            }
            bool hasSevenRewardCanGet = false;
            List<SevenDayWelfare> list = m_dicSevenDay[dicIndex];
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].state == QuickLevState.QuickLevState_CanGet)
                        {
                            hasSevenRewardCanGet = true;
                        }
                    }             
            }
            return hasSevenRewardCanGet;
        }
        return false;
    }
    public void SetNewServerWelfareClose(bool close)
    {
        if (close)
        {
            if (m_dicWelfare.ContainsKey((uint)WelfareType.OpenSever))
            {
                m_dicWelfare.Remove((uint)WelfareType.OpenSever);
            }
        }
    }

    public void StartTimer()
    {
        Engine.Utility.TimerAxis.Instance().SetTimer(TIMER_ID, 1000, this);
    }

    private void ParaseReward(string strReward, ref List<WelfareItem> lstReward)
    {
        string[] items = strReward.Split(';');
        if (items != null)
        {
            for (int k = 0; k < items.Length; ++k)
            {
                string[] stritem = items[k].Split('_');
                if (stritem.Length == 2)
                {
                    uint tempItemID = 0,tempNum = 0;
                    if(!uint.TryParse(stritem[0],out tempItemID))
                    {
                        Engine.Utility.Log.Error("stritem[0 error");
                    }
                    if (!uint.TryParse(stritem[1], out tempNum))
                    {
                        Engine.Utility.Log.Error("stritem[1 error");
                    }
                    WelfareItem item = new WelfareItem()
                    {
                        itemid = tempItemID,
                        itemNum = tempNum
                    };
                    lstReward.Add(item);
                }
            }
        }
    }
    private void ParaseRewardReward(string strReward, ref List<RewardFindItems> lstReward)
    {
        string[] items = strReward.Split(';');
        if (items != null)
        {
            for (int k = 0; k < items.Length; ++k)
            {
                string[] stritem = items[k].Split('_');
                if (stritem.Length == 2)
                {
                    uint tempItemID = 0, tempNum = 0;
                    if (!uint.TryParse(stritem[0], out tempItemID))
                    {
                        Engine.Utility.Log.Error("stritem[0 error");
                    }
                    if (!uint.TryParse(stritem[1], out tempNum))
                    {
                        Engine.Utility.Log.Error("stritem[1 error");
                    }
                    RewardFindItems item = new RewardFindItems()
                    {
                        itemID = tempItemID,
                        itemNum = tempNum
                    };
                    lstReward.Add(item);
                }
            }
        }
    }

    public bool GetAllWelfareType(ref List<WelfareType> lst)
    {
        if (m_dicWelfare != null)
        {
            List<WelfareData> list = new List<WelfareData>();
            foreach (var item in m_dicWelfare.Keys)
            {
                if (m_dicWelfare[item].Count > 0)
                {
                   WelfareData data =m_dicWelfare[item][0];
                   list.Add(data);
                }              
            }
            list.Sort(TypeSort);
            for (int i = 0; i < list.Count;i++ )
            {
                lst.Add((WelfareType)list[i].type);
            }
        }
        return false;
    }

    int TypeSort(WelfareData a,WelfareData b)
    {
        return a.SortID - b.SortID;
    }
    /// <summary>
    /// 福利礼包
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<WelfareData> GetWelfareDatasByType(WelfareType type)
    {
        if (m_dicWelfare.ContainsKey((uint)type))
        {
            return m_dicWelfare[(uint)type];
        }
        return null;
    }

    public List<WelfareData> GetWelfareDatasBy2Type(WelfareType type,InviteType iType)
    {
        List<WelfareData> temp = new List<WelfareData>();
        if (m_dicWelfare.ContainsKey((uint)type))
        {
            for (int i = 0; i < m_dicWelfare[(uint)type].Count; i++)
            {
                if (m_dicWelfare[(uint)type][i].inviteType == iType)
                 {
                     temp.Add(m_dicWelfare[(uint)type][i]);
                 }
            }
            return temp;
        }
        return null;
    }
    public List<SevenDayWelfare> GetSevenDayWelfareByDay(uint day)
    {
        if (m_dicSevenDay.ContainsKey(day))
        {
            return m_dicSevenDay[day];
        }
        else
        {
            Engine.Utility.Log.Error("获取七日福利数据失败 day ：{0}", day);
        }
        return null;
    }


    /// <summary>
    /// 更新七日福利获取id
    /// </summary>
    /// <param name="nRewardId"></param>
    public void UpdateServerOpenGotReward(uint nRewardId, bool updateUI = false)
    {
        bool isDiscountGift = false;
        bool isRewardBoxGot = false;
        if (!m_lstServerOpenGotReward.Contains(nRewardId) && nRewardId != 0)
        {
            m_lstServerOpenGotReward.Add(nRewardId);
            
            foreach (var item in m_dicSevenDay.Values)
            {
                bool found = false;
                for (int i = 0; i < item.Count; i++)
                {
                    if (item[i].id != nRewardId)
                    {
                        continue;
                    }
                    item[i].state = QuickLevState.QuickLevState_HaveGet;
                    found = true;
                    if (item[i].nType == 10)
                    {
                        isDiscountGift = true;
                    }
                    if (item[i].nType == 13)
                    {
                        isRewardBoxGot = true;
                    }
                    break;
                }
                if (found)
                {
                    break;
                }
            }
        }
        UpdateWelfareState(2);
        if (updateUI)
        {
            if (isDiscountGift)
            {
                ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateDisCount", null, null);
                DispatchValueUpdateEvent(arg);
                TipsManager.Instance.ShowTips("购买成功");
            }
            else
            {

                TipsManager.Instance.ShowTips("领取成功");
                ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateList", null, null);
                DispatchValueUpdateEvent(arg);
            }
            if (isRewardBoxGot)
            {
                ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateFinalTargetData", null, null);
                DispatchValueUpdateEvent(arg);
            }
           
        }

        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.SEVENDAY,
            direction = (int)WarningDirection.Right,
            bShowRed = HasRewardCanGet(2),
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }
    public void ClearDataList() 
    {
        m_lstWelfareGotId.Clear();
    }
    /// <summary>
    /// 更新福利获取id
    /// </summary>
    /// <param name="nRewardId"></param>
    public void UpdateWelfaregGotReward(uint nRewardId,bool updateUI = false)
    {
        bool isMonth = false;
        bool isFriendInvite = false;
        bool isCollectWord = false;
        foreach (var item in m_dicWelfare.Values)
        {
            bool found = false;
            for (int i = 0; i < item.Count; i++)
            {
                if (item[i].id != nRewardId)
                {
                    continue;
                }
                item[i].state = QuickLevState.QuickLevState_HaveGet;
                found = true;
                if (item[i].type == WelfareType.Month)
                {
                    isMonth = true;
                }
                else if (item[i].type == WelfareType.FriendInvite)
                {
                    isFriendInvite = true;
                }
                else if (item[i].type == WelfareType.CollectWord)
                {
                    isCollectWord = true;
                }
                break;
            }
            if (found)
            {
                break;
            }
        }
        if (!m_lstWelfareGotId.Contains(nRewardId) && nRewardId != 0 && !isCollectWord)
        {
            m_lstWelfareGotId.Add(nRewardId);       
        }
        UpdateWelfareState(1);
        if (updateUI)
        {
            if (!isMonth)
            {
                if (isFriendInvite)
                {
                    ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateInviteList", null, null);
                    DispatchValueUpdateEvent(arg);
                }
                else if (isCollectWord)
                {
                    ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateCollectWord", null, null);
                    DispatchValueUpdateEvent(arg);
                }
                else
                {
                    TipsManager.Instance.ShowTips("奖励领取成功!");
                    ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateList", null, null);
                    DispatchValueUpdateEvent(arg);    
                }
                  
            }        
            else
            {
                if (!IsSignCurrDay)
                {
                    IsSignCurrDay = true;
                    TipsManager.Instance.ShowTips("签到成功!");
                }
                else
                {
                    TipsManager.Instance.ShowTips("补签成功！");
                }
                //服務器数据后来 所以客户端先加
                SignDay++;
                ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateMonth", null, nRewardId);
                DispatchValueUpdateEvent(arg);
            }            
        }
        if (DataManager.Manager<DailyManager>().isBindFinish)
        {
            ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateBindPhone", null, null);
            DispatchValueUpdateEvent(arg);
        }
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.WELFARE,
            direction = (int)WarningDirection.None,
            bShowRed = HasRewardCanGet(1) || HasRewardCanReBack(),
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
    }

    private void CheckEquipEnough(ref SevenDayWelfare data, ref List<BaseEquip> lstEquip1,ref List<BaseEquip> lstEquip2)
    {
        data.state = QuickLevState.QuickLevState_None;
        data.process = 0;
        int num = 0;
        for (int k = 0, kmax = lstEquip1.Count; k < kmax; k++)
        {
            uint nLevel = 0;
            switch (data.nType)
            {
                case  3:
                    nLevel = (uint)lstEquip1[k].Grade;
                    break;
                case 4:
                    if (lstEquip1[k] is Equip)
                        {
                            Equip equip = lstEquip1[k] as Equip;
                            if (equip != null)
                            {
                                nLevel = equip.RefineLv;
                            }
                            else
                            {
                                Engine.Utility.Log.Error("equip is null k :{0} lst ? null {1}", k, lstEquip1[k].GetType().ToString());
                            }
                        }
                   
                    break;
                default:
                    break;
            }

            if (nLevel == data.param2)
            {
                num++;
                data.process = num;
                if (num >= data.param1)
                {
                    data.state = QuickLevState.QuickLevState_CanGet;
                    return;
                }
            }
        }

        if (data.state == QuickLevState.QuickLevState_None)
        {
            for (int k = 0, kmax = lstEquip2.Count; k < kmax; k++)
            {
                uint nLevel = 0;
                switch (data.nType)
                {
                    case 3:
                        nLevel = (uint)lstEquip2[k].Grade;
                        break;
                    case 4:
                        if (lstEquip2[k] is Equip)
                        {
                            Equip equip = lstEquip2[k] as Equip;
                            if (equip != null)
                            {
                                nLevel = equip.RefineLv;
                            }
                            else
                            {
                                Engine.Utility.Log.Error("equip is null k :{0} lst ? null {1}", k, lstEquip2[k].GetType().ToString());
                            }
                        }
                        break;
                    default:
                        break;
                }
                if (nLevel == (int)data.param2)
                {
                    num++;
                    data.process = num;
                    if (num >= data.param1)
                    {
                        data.state = QuickLevState.QuickLevState_CanGet;
                        return;
                    }
                }
            }
        }
    }
    void LocalTimeRunUpdatePanel() 
    {
        onlineWelfareTime = (uint)(((int)UnityEngine.Time.realtimeSinceStartup - LoginTime + (int)CurrDayOnlineTime) / 60.0f);
        onlineSevenDayTime = (uint)(((int)UnityEngine.Time.realtimeSinceStartup - LoginTime + (int)User_OnlineTime) / 60.0f);
        if (m_dicWelfare[2] != null)
        {
            for (int i = 0; i < m_dicWelfare[2].Count; i++)
            {
                WelfareData welfardData = m_dicWelfare[2][i];
                if (welfardData != null)
                {
                    welfardData.process = (int)onlineWelfareTime;
                    if (onlineWelfareTime >= welfardData.param && !welfareOnlineList.Contains(welfardData.id) && !m_lstWelfareGotId.Contains(welfardData.id))
                    {
                        welfardData.state = QuickLevState.QuickLevState_CanGet;                   
                        welfareOnlineList.Add(welfardData.id);
                        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateList", null, null);
                        DispatchValueUpdateEvent(arg);



                        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
                        {
                            modelID = (int)WarningEnum.WELFARE,
                            direction = (int)WarningDirection.None,
                            bShowRed = HasRewardInType(1, 2),
                        };
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);              
                    }
                }
            }
        }
        if (m_dicWelfare[4] != null)
        {
            if( Client.ClientGlobal.Instance().MainPlayer  == null)
            {
                return;
            }
            int roleLevel = Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level);
            for (int i = 0; i < m_dicWelfare[4].Count; i++)
            {
                WelfareData welfardData = m_dicWelfare[4][i];
                if (welfardData != null)
                {
                    welfardData.process = (int)roleLevel;
                    if (roleLevel >= welfardData.param && !welfareRoleLevelList.Contains(welfardData.id)&& !m_lstWelfareGotId.Contains(welfardData.id))
                    {
                        welfardData.state = QuickLevState.QuickLevState_CanGet;
                      
                        welfareRoleLevelList.Add(welfardData.id);
                        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateList", null, null);
                        DispatchValueUpdateEvent(arg);


                        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
                        {
                            modelID = (int)WarningEnum.WELFARE,
                            direction = (int)WarningDirection.None,
                            bShowRed = HasRewardInType(1, 4),
                        };
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);   
                    }
                }
            }
        }
        if (m_dicSevenDay[1] != null)
        {
            for (int j = 0; j < m_dicSevenDay[1].Count; j++)
            {
                SevenDayWelfare wldfareData = m_dicSevenDay[1][j];
                if (wldfareData != null && wldfareData.nType == 2)
                {
                    wldfareData.process = (int)onlineSevenDayTime;
                    if (onlineSevenDayTime >= wldfareData.param1 && !sevenDayOnlineList.Contains(wldfareData.id) && !m_lstServerOpenGotReward.Contains(wldfareData.id))
                    {
                        wldfareData.state = QuickLevState.QuickLevState_CanGet;                     
                        sevenDayOnlineList.Add(wldfareData.id);
                        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateList", null, null);
                        DispatchValueUpdateEvent(arg);


                        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
                        {
                            modelID = (int)WarningEnum.SEVENDAY,
                            direction = (int)WarningDirection.Right,
                            bShowRed = HasRewardInType(2, 1) && wldfareData.nDay <= DataManager.Manager<WelfareManager>().ServerOpenCurrDay,
                        };
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st); 
                    }

                }
            }

        }


    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="welfareType">1-福利礼包 2-新服七日福利</param>
    public void UpdateWelfareState(int welfareType)
    {
        if (Client.ClientGlobal.Instance().MainPlayer == null)
        {
            return;
        }
        int roleLevel = Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level);
        if (welfareType == 1)
        {
            foreach (var lstdata in m_dicWelfare.Values)
            {
                for (int i = 0; i < lstdata.Count; i++)
                {
                    WelfareData welfardData = lstdata[i];
                    if (m_lstWelfareGotId.Contains(welfardData.id))
                    {
                        welfardData.state = QuickLevState.QuickLevState_HaveGet;
                        continue;
                    }                  
                    //TODO 
                    switch (welfardData.type)
                    {
                        case WelfareType.RoleLevel:
                            {
                                welfardData.process = roleLevel;
                                welfardData.state = roleLevel >= lstdata[i].param ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                            }
                            break;
                        case WelfareType.OnLine:
                            {
                                int onlineTime = (int)(((int)UnityEngine.Time.realtimeSinceStartup - LoginTime + (int)CurrDayOnlineTime)/60.0f);
                                welfardData.state = onlineTime >= ((int)welfardData.param) ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                                welfardData.process = onlineTime;
                            }
                            break;
                        case WelfareType.SevenDay:
                            {

                                welfardData.state = LoginTimesOfWeek >= (int)welfardData.param ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                                welfardData.process =(int)LoginTimesOfWeek;
                            }
                            break;
                        case WelfareType.OpenSever:
                            welfardData.process = (int)NewServerLoginTimes;
                            welfardData.state = NewServerLoginTimes >= (int)welfardData.param ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                            break;
                        case WelfareType.Month:
                            welfardData.state = QuickLevState.QuickLevState_None;
                            if (!IsSignCurrDay)
                            {
                                if (SignDay + 1 == welfardData.param)
                                {
                                    welfardData.state = QuickLevState.QuickLevState_CanGet;
                                }
                            }
                            break;
                        case WelfareType.BindGift:
                            welfardData.state = CanGetBindReward ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                            break;
                        case WelfareType.FriendInvite:
                             int process = 0;
                            if (welfardData.inviteType == InviteType.Inviter)
                            {
                                    for (int m = 0; m < inviterInfos.Count;m++ )
                                    {                             
                                        if (inviterInfos[m].lv >= welfardData.param2)
                                        {
                                            process++;
                                        }
                                    }
                                    welfardData.state = (process >= (int)welfardData.param) ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                             }
                             else if (welfardData.inviteType == InviteType.InvitedRecharge)
                             {
                                 process =(int)Other_Total_Recharge;
                                 welfardData.state = (process >= (int)welfardData.param) ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                             }
                             else
                             {
                                 process = (int)MainPlayerHelper.GetPlayerLevel();
                                 welfardData.state = (process >= (int)welfardData.param && HadBeenInvited) ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                             }                        
                             welfardData.process = process;
                            break;

                        case WelfareType.CollectWord:
                            if (m_lst_CollectWordData != null)
                            {
                                OnRecieveCollectData(m_lst_CollectWordData);
                            }
                            break;
                    }
                }
            }
        }else if (welfareType == 2)
        {
/*
1-主角等级
2-累计在线时长
3-N件X档装备
4-N件精炼X级装备
 */
            List<BaseEquip> lstEquip1 = DataManager.Manager<EquipManager>().GetEquipsByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP);
            List<BaseEquip> lstEquip2 = DataManager.Manager<EquipManager>().GetEquipsByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
            CurActiveNum = 0;
            List<uint> lstSoul = DataManager.Manager<EquipManager>().GetWeaponSoulDataList();
            foreach (var lstdata in m_dicSevenDay.Values)
            {
                for (int i = 0; i < lstdata.Count; i++)
                {
                    SevenDayWelfare wldfareData = lstdata[i];
                    if (m_lstServerOpenGotReward.Contains(wldfareData.id))
                    {
                        wldfareData.state = QuickLevState.QuickLevState_HaveGet;
                        CurActiveNum += wldfareData.point;
                    }
                    if (wldfareData.state == QuickLevState.QuickLevState_HaveGet)
                    {
                        if (CanGetIDs.Contains(wldfareData.id)) 
                        {
                            CanGetIDs.Remove(wldfareData.id);
                        }
                        continue;
                    }
                    if (CanGetIDs.Contains(wldfareData.id))
                    {
                        wldfareData.process = (int)wldfareData.param1;
                        wldfareData.state = QuickLevState.QuickLevState_CanGet;
                        continue;
                    }

                    //TODO 
                    switch (wldfareData.nType)
                    {

                        case 1:
                            {
                                wldfareData.state = roleLevel >= (int)wldfareData.param1 ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                                wldfareData.process = roleLevel; 
                            }
                            break;
                        case 2:
                            {
                                int onlineTime = (int)(((int)UnityEngine.Time.realtimeSinceStartup - LoginTime + (int)User_OnlineTime)/60.0f);
                                wldfareData.state = onlineTime >= (int)wldfareData.param1 ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                                wldfareData.process = onlineTime;
                            }
                            break;
                        case 3:
                        case 4:
                            {
                                CheckEquipEnough(ref wldfareData, ref lstEquip1, ref lstEquip2);
                            }
                            break;
/*
5-1个X等级战魂
6-1个携带等级为X级战魂
7-武斗场排行X名
8-N个X星级圣魂
9-主角总战力达到X
10-打折礼包*/
                        case 5:
                            {
                                int num = 0;
                                wldfareData.state = QuickLevState.QuickLevState_None;
                                Dictionary<uint, Client.IPet> dicPet = DataManager.Manager<PetDataManager>().GetPetDic();
                                foreach (Client.IPet pet in dicPet.Values)
                                {
                                    int level = pet.GetProp((int)Client.CreatureProp.Level);
                                    if (level >= wldfareData.param2)
                                    {
                                        num++;
                                        wldfareData.process = num;
                                        if (num >= wldfareData.param1)
                                        {
                                            wldfareData.state = QuickLevState.QuickLevState_CanGet;
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        case 6:
                            {
                                int num = 0;
                                wldfareData.state = QuickLevState.QuickLevState_None;
                                Dictionary<uint, Client.IPet> dicPet = DataManager.Manager<PetDataManager>().GetPetDic();
                                foreach (Client.IPet pet in dicPet.Values)
                                {
                                    table.PetDataBase pb = DataManager.Manager<PetDataManager>().GetPetDataBase(pet.PetBaseID);
                                    if (pb != null)
                                    {
                                        int level = (int)pb.carryLevel;
                                        if (level == wldfareData.param2)
                                        {
                                            num++;
                                            wldfareData.process = num;

                                            if (num >= wldfareData.param1)
                                            {
                                                wldfareData.state = QuickLevState.QuickLevState_CanGet;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case 7:
                            {
                                int rank = (int)DataManager.Manager<ArenaManager>().Rank;
                                if (rank != 0)
                                {
                                    wldfareData.state = rank <= wldfareData.param1 ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                                }
                                else
                                {
                                    wldfareData.state = QuickLevState.QuickLevState_None;
                                }
                                wldfareData.process = rank;
                            }
                            break;
                        case 8:
                            {
                                wldfareData.state = QuickLevState.QuickLevState_None;

                                int num = 0;
                                for (int k = 0; k < lstSoul.Count; k++)
                                {
                                    Muhon muhon = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(lstSoul[k]);
                                    if (muhon != null)
                                    {
                                        if (muhon.StartLevel >= (int)wldfareData.param2)
                                        {
                                            num++;
                                            wldfareData.process = num;
                                            if (num >= (int)wldfareData.param1)
                                            {
                                                wldfareData.state = QuickLevState.QuickLevState_CanGet;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case 9:
                            {
                                int power = Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.FightCreatureProp.Power);
                                wldfareData.state = power >= wldfareData.param1 ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                                wldfareData.process = power;

                            }
                            break;
                        case 10:
                        case 11:
                            {
                                Dictionary<int, uint> gemInlayData = DataManager.Manager<EquipManager>().GemInlayData;
                                uint value = 0;
                                if (gemInlayData.Count>0)
                                {
                                   foreach(var data in gemInlayData)
                                   {
                                       uint itemID = data.Value;
                                       table.ItemDataBase item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemID);
                                       if (item != null)
                                       {
                                           value += item.grade;
                                       }
                                   }
                                   wldfareData.state = value >= wldfareData.param1 ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                                   wldfareData.process = (int)value;
                                }
                            }
                            break;
                        case 12: 
                            {
                                wldfareData.state = QuickLevState.QuickLevState_None;

                                int num = 0;
                                for (int k = 0; k < lstSoul.Count; k++)
                                {
                                    Muhon muhon = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(lstSoul[k]);
                                    if (muhon != null)
                                    {
                                        if (muhon.Level >= (int)wldfareData.param2)
                                        {
                                            num++;
                                            wldfareData.process = num;
                                            if (num >= (int)wldfareData.param1)
                                            {
                                                wldfareData.state = QuickLevState.QuickLevState_CanGet;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case 13:
                            {
                                uint value = 0;
                                for (int n = 0; n < m_lstServerOpenGotReward.Count; n++)
                                {
                                    table.SevenDataBase data = GameTableManager.Instance.GetTableItem<table.SevenDataBase>(m_lstServerOpenGotReward[n]);
                                    if (data != null)
                                    {
                                        value += data.point;
                                    }
                                }
                                bool b = value >= wldfareData.param1;
                                wldfareData.state = b ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
                              
                                if (b)
                                {
                                        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateFinalTargetData", null, null);
                                        DispatchValueUpdateEvent(arg);                                  
                                }
                               
                            }
                            break;
                    }
                    if (wldfareData.state == QuickLevState.QuickLevState_CanGet && (!CanGetIDs.Contains(wldfareData.id)))
                    {
                        CanGetIDs.Add(wldfareData.id);
                    }
                }
            }

            if (CanGetIDs.Count>0)
            {
                stCanGetRewardIdPropertyUserCmd_C cmd = new stCanGetRewardIdPropertyUserCmd_C();
                cmd.id.AddRange(CanGetIDs);
                NetService.Instance.Send(cmd);
            }
        }
    }    
    public void ReceiveRewardFindState(uint pre_Lv,List<FindReward> list) 
    {
        m_lstReward = new List<RewardFindData>();     
        Previous_Lv = pre_Lv;
        for (int i = 0; i < list.Count; i++ )
        {
            RewardFindDataBase tab = GameTableManager.Instance.GetTableItem<RewardFindDataBase>(pre_Lv, (int)list[i].reward_id);
            if (tab != null)
            {       
                string rewards = tab.rewards;
                if(!string.IsNullOrEmpty(rewards))
                {
                    RewardFindData find = new RewardFindData()
                    {
                        id = list[i].reward_id,
                        left_time = list[i].find_time,
                        total_time = tab.maxTimes,
                        state = list[i].find_time > 0 ? RewardFindData.RewardFindState.CanGet : RewardFindData.RewardFindState.Got,
                    };
                    ParaseRewardReward(rewards, ref find.list);
                    if (find != null)
                    {
                        m_lstReward.Add(find);
                    }
                }                      
            }                   
        }
        if (!HasRewardCanReBack())
        {
            ValueUpdateEventArgs arg1 = new ValueUpdateEventArgs("OnHideAllRewardFind", null, null);
            DispatchValueUpdateEvent(arg1);
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.WELFARE,
                direction = (int)WarningDirection.None,
                bShowRed = HasRewardCanGet(1) || HasRewardCanReBack(),
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        }

 
    }
    public void RefreshRewardFindState(uint id , uint left_time) 
    {
        for (int i = 0; i < m_lstReward.Count; i ++ )
        {
            if (m_lstReward[i].id == id)
            {
                m_lstReward[i].left_time = left_time;
                m_lstReward[i].state = left_time > 0 ? RewardFindData.RewardFindState.CanGet : RewardFindData.RewardFindState.Got;
                if (m_lstReward[i].state == RewardFindData.RewardFindState.CanGet)
                {
                    ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateSingleRewardFind", null, m_lstReward[i]);
                    DispatchValueUpdateEvent(arg);
                }
                else
                {
                    ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateAllRewardFind", null, null);
                    DispatchValueUpdateEvent(arg);
                }            
            }
        }
        if (!HasRewardCanReBack())
        {
            ValueUpdateEventArgs arg1 = new ValueUpdateEventArgs("OnHideAllRewardFind", null, null);
            DispatchValueUpdateEvent(arg1);
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.WELFARE,
                direction = (int)WarningDirection.None,
                bShowRed = HasRewardCanGet(1) ||  HasRewardCanReBack(),
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        }
    }
    public bool HasRewardCanReBack() 
    {
        for (int i = 0; i < m_lstReward.Count;i++ )
        {
            if (m_lstReward[i].state == RewardFindData.RewardFindState.CanGet)
            {
                return true;
            }
        }
        return false;
    }
    public void PlayerHadGotInvited(uint inviter_id,bool isOver,uint lv, uint profession,string name) 
    {
        HadBeenInvited = isOver;
        InviterID = inviter_id;
        InviterLv = lv;
        InviterProfession = profession;
        InviterName = name;
        UpdateWelfareState(1);
        ValueUpdateEventArgs arg= new ValueUpdateEventArgs("OnChangeInviteState", null, null);
        DispatchValueUpdateEvent(arg);
        if (inviter_id != 0)
        {
            TipsManager.Instance.ShowTips(string.Format("你成功被{0}招募", name));
        }
        
    }
    public void RecieveInvitedPlayerData(uint total_recharge,List<BaseUserInfo> datas) 
    {
        Other_Total_Recharge = total_recharge;
        InviterInfos = datas;
        UpdateWelfareState(1);
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("RecieveInvitedPlayerData", null, null);
        DispatchValueUpdateEvent(arg);
    }
    public void RefreshInvitedPlayerData(uint uid,uint level, uint recharge, uint total_recharge) 
    {
        for (int i = 0; i < InviterInfos.Count;i++ )
        {
            if (InviterInfos[i].uid == uid)
            {
                InviterInfos[i].recharge = recharge;
                InviterInfos[i].lv = level;
                Other_Total_Recharge = total_recharge;
            }
        }
        UpdateWelfareState(1);
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("RecieveInvitedPlayerData", null, null);
        DispatchValueUpdateEvent(arg);
    }

    int deltaTime = 0;
    public void OnTimer(uint uTimerID)
    {
        if (!EnterGameReadey)
        {
            return;
        }
       
        if (Remain_Time > 0)
        {
            Remain_Time--;
            if (OnUpdateTimeEvent != null)
            {
                OnUpdateTimeEvent(Remain_Time);
            }
        }
        else
        {
            Engine.Utility.TimerAxis.Instance().KillTimer(TIMER_ID, this);
            SevenDayOpenState = false;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SEVENDAYOPENSTATUS, SevenDayOpenState);
        }

     

        if (deltaTime <= 30)
        {
            deltaTime++;
        }
        else
        {
            LocalTimeRunUpdatePanel();
            deltaTime = 0;
            ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateList", null, null);
            DispatchValueUpdateEvent(arg);
            stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.WELFARE,
                direction = (int)WarningDirection.None,
                bShowRed = HasRewardCanGet(1) || HasRewardCanReBack(),
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);

            stShowMainPanelRedPoint st2 = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.SEVENDAY,
                direction = (int)WarningDirection.Right,
                bShowRed = HasRewardCanGet(2),
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st2);

            stShowMainPanelRedPoint st3 = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.GodWeapen,
                direction = (int)WarningDirection.Right,
                bShowRed = CheckShowGodWeapenWarning(),
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st3);

            stShowMainPanelRedPoint st4 = new stShowMainPanelRedPoint()
            {
                modelID = (int)WarningEnum.OpenServer,
                direction = (int)WarningDirection.Right,
                bShowRed = HasOpenPackageCanGet(),
            };
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st4);  
        }

    }



    #region 开服豪礼
    List<uint> openPackageList = new List<uint>();
    public List<uint> OpenPackageList 
    {
        set 
        {
            openPackageList = value;
        }
        get 
        {
            return openPackageList;
        }
    }
    public bool IsOpenServerGiftFinished
    {
        set;
        get;
    }
    public void OnOpenPackageSign(List<uint> ids) 
    {
        openPackageList = ids;
        List<uint> m_lst_dataIDs = new List<uint>();
        IsOpenServerGiftFinished =OpenServerList.Count == openPackageList.Count;        
        //开服豪礼领完，需要关闭主界面按钮      
        if (IsOpenServerGiftFinished)
        {
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.OpenServerPanel))
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.OpenServerPanel);
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.OPENSERVERGIFTSTATUS, IsOpenServerGiftFinished);
        }   


        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.OpenServer,
            direction = (int)WarningDirection.Right,
            bShowRed = HasOpenPackageCanGet(),
        };    
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
       
    }

    public void OnOpenPackageSign(uint id)
    {
        if (!openPackageList.Contains(id))
        {
            openPackageList.Add(id);
        }
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateOpenPackage", null, null);
        DispatchValueUpdateEvent(arg);
        IsOpenServerGiftFinished = OpenServerList.Count == openPackageList.Count;   
       


        if (IsOpenServerGiftFinished)
        {
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.OpenServerPanel))
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.OpenServerPanel);
            }
            //开服豪礼领完，需要关闭主界面按钮
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.OPENSERVERGIFTSTATUS, IsOpenServerGiftFinished);
        }      

        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.OpenServer,
            direction = (int)WarningDirection.Right,
            bShowRed = HasOpenPackageCanGet(),
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);   
    }
 
    bool HasOpenPackageCanGet() 
    {
        bool value = false;
       
        bool open =  MainPlayerHelper.GetPlayerLevel() >= OpenSeverLevel;
        for (int i = 0; i < OpenServerList.Count; i++)
        {
            uint curDay = OpenServerList[i].ID;
            if (!openPackageList.Contains(curDay) && curDay <= NewServerLoginTimes && open)
            {
                value = true;
            }
        }
       
        return value;
    }
    #endregion 

    public uint GetTextureNameByWelfareType(uint type) 
    {
        uint  resID =0;
        if (m_dicTextureName.ContainsKey(type))
       {
           resID = m_dicTextureName[type];
       }
        return resID;
    }


    #region 神兵

    List<uint> m_lst_GwRecord = new List<uint>();
    public List<uint> GodWeapenRecord 
    {
        set
        {
            m_lst_GwRecord = value;
        }
        get 
        {
            return m_lst_GwRecord;
        }
     }
    /// <summary>
    /// 是否已经激活了神器
    /// </summary>
    public bool IsGodWeapenActivate { set; get; }

    /// <summary>
    /// 可以激活神器
    /// </summary>
    public bool CanJiHuoGodWeapen { set; get; }
    public void OnRecieceArtifact(List<uint> data, bool isActivate, bool canActivate) 
    {
        IsGodWeapenActivate = isActivate;
        m_lst_GwRecord = data;
        CanJiHuoGodWeapen = canActivate;
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateArticfact", null, null);
        DispatchValueUpdateEvent(arg);

        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.GodWeapen,
            direction = (int)WarningDirection.Right,
            bShowRed = CheckShowGodWeapenWarning(),
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);   
    }
    public void GetArticfactReward(uint id, bool canActivate) 
    {
        if (!m_lst_GwRecord.Contains(id))
        {
            m_lst_GwRecord.Add(id);
        }
        CanJiHuoGodWeapen = canActivate;
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("OnUpdateArticfact", null, null);
        DispatchValueUpdateEvent(arg);
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.GodWeapen,
            direction = (int)WarningDirection.Right,
            bShowRed = CheckShowGodWeapenWarning(),
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);   
    }

    public void OnJiHuoShenQi() 
    {
        IsGodWeapenActivate = true;
       if(DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.GodWeapenPanel))
       {
           DataManager.Manager<UIPanelManager>().HidePanel(PanelID.GodWeapenPanel);
       }

       //神兵已经激活，需要关闭主界面按钮
       Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.GODWEAPENSTATUS, IsGodWeapenActivate);
    }
    bool CheckShowGodWeapenWarning() 
    {
        bool showRed = false;       
        int level = MainPlayerHelper.GetPlayerLevel();
        for (int i = 0; i < ArtifactList.Count; i++)
        {
            bool isOk = level >= ArtifactList[i].open_level && DataManager.Manager<TaskDataManager>().CheckTaskFinished(ArtifactList[i].taskid);
            if ((isOk && !m_lst_GwRecord.Contains(ArtifactList[i].ID)) || (CanJiHuoGodWeapen && !IsGodWeapenActivate))
            {
                showRed = true;
            }
        }
        return showRed;
    
    }

    #endregion

    #region 冲级礼包
    List<QuickLevData> m_lst_QuickLv = new List<QuickLevData>();
    public void OnRecieveQuickLvState(List<QuickLevData> data) 
    {
        if (data != null && m_lst_QuickLv != null)
       {
           m_lst_QuickLv.Clear();
           m_lst_QuickLv.AddRange(data);
           if (m_dicWelfare.ContainsKey((uint)WelfareType.RushLevel))
           {
               List<WelfareData> lst = m_dicWelfare[(uint)WelfareType.RushLevel];
               if (lst != null)
               {
                   for (int i = 0; i < lst.Count; i++)
                   {
                       for (int j = 0; j < m_lst_QuickLv.Count; j++)
                       {
                           if (lst[i].id == m_lst_QuickLv[j].id)
                           {
                               lst[i].state = m_lst_QuickLv[j].state;
                               lst[i].process =(int)m_lst_QuickLv[j].nums;
                           }
                       }
                   }
               }
           }
       }
    }
    public void OnRecieveSingleQuickLvState(uint id,QuickLevState state) 
    {
          if (m_dicWelfare.ContainsKey((uint)WelfareType.RushLevel))
           {
               List<WelfareData> lst = m_dicWelfare[(uint)WelfareType.RushLevel];
               if (lst != null)
               {
                   for (int i = 0; i < lst.Count; i++)
                   {
                       if (lst[i].id == id)
                       {
                           lst[i].state = state;
                       }
                   }
               }
            }
          ValueUpdateEventArgs args = new ValueUpdateEventArgs("OnUpdateRushLevel", null, id);
          DispatchValueUpdateEvent(args);
    }

    public void OnRecieveQuickLvNum(List<LevGiftNums> datas)
    {
        if (m_dicWelfare.ContainsKey((uint)WelfareType.RushLevel))
        {
            List<WelfareData> lst = m_dicWelfare[(uint)WelfareType.RushLevel];
            if (lst != null)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    for (int j = 0; j < datas.Count;j++ )
                    {
                        if (lst[i].id == datas[j].id)
                        {
                            lst[i].process =(int)datas[j].nums;
                        }
                    }                 
                }           
            }
        }
        ValueUpdateEventArgs args = new ValueUpdateEventArgs("OnUpdateRushLevel", null, null);
        DispatchValueUpdateEvent(args);
    }

    public void OnGetRushLv(uint id) 
    {
        if (m_dicWelfare.ContainsKey((uint)WelfareType.RushLevel))
        {
            List<WelfareData> lst = m_dicWelfare[(uint)WelfareType.RushLevel];
            if (lst != null)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].id == id)
                    {
                        lst[i].state = QuickLevState.QuickLevState_HaveGet;
                    }

                }
            }
        }
        ValueUpdateEventArgs args = new ValueUpdateEventArgs("OnUpdateRushLevel", null, id);
        DispatchValueUpdateEvent(args);
    }
    #endregion

    #region 集字活动
    List<CollectWordData> m_lst_CollectWordData = null;
    /// <summary>
    /// 接到消息，本地缓存集字数据并初始化
    /// </summary>
    /// <param name="list"></param>
    public void OnCollectWord(List<CollectWordData> list) 
    {
        if (m_lst_CollectWordData == null)
        {
            m_lst_CollectWordData = new List<CollectWordData>();
        }
        if (m_lst_CollectWordData != null)
        {
            m_lst_CollectWordData = list;

            OnRecieveCollectData(m_lst_CollectWordData);
        }
  
    }

    /// <summary>
    /// 刷新本地所有的集字相关的数据
    /// </summary>
    /// <param name="list"></param>
    void OnRecieveCollectData(List<CollectWordData> list) 
    {
        if (m_dicWelfare != null)
        {
            uint key = (uint)WelfareType.CollectWord;
            if (m_dicWelfare.ContainsKey(key))
            {
                List<WelfareData> data = m_dicWelfare[key];
                if (data != null)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            if (data[i].param == list[j].id)
                            {
                                OnUpdateCollectData(data[i], list[j]);
                            }
                        }
                    }
                }

            }
        }
    }
    /// <summary>
    /// 刷新集字状态
    /// </summary>
    /// <param name="data"></param>
    /// <param name="list"></param>
    void OnUpdateCollectData(WelfareData data, CollectWordData list) 
    {      
        if (data.collectType == CollectType.Day)
        {
            data.process = (int)list.times;
        }
        else if (data.collectType == CollectType.All)
        {
            data.process = (int)list.total_times;
        }
        else
        {
            data.process = 0;
        }     
        //判断状态是不是可以领取 
        //条件1  集齐字没有   条件2  是不是超过限制了
        bool inNum =data.collectType== CollectType.None || data.process < data.param2;
        bool overTime = RefreshCurrectWordDatas(data);
        data.state = (inNum && overTime) ? QuickLevState.QuickLevState_CanGet : QuickLevState.QuickLevState_None;
    }

    bool RefreshCurrectWordDatas(WelfareData data)
    {
        int collectedWordNum = 0;
        bool haveCollectAllWord = false;
        if (data.collectWords != null)
        {
            for (int i = 0; i < data.collectWords.Count; i++)
            {
                uint ItemId = data.collectWords[i].itemid;
                uint itemNum = data.collectWords[i].itemNum;
                int ownNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(ItemId);
                bool match = ownNum >= itemNum;
                string needNumStr = ColorManager.GetColorString(ColorType.JZRY_Txt_Black, string.Format("/{0}", itemNum));
                string ownNumStr = match ? ColorManager.GetColorString(ColorType.JZRY_Green, ownNum.ToString()) : ColorManager.GetColorString(ColorType.JZRY_Txt_Red, ownNum.ToString());
                data.collectWords[i].name = string.Format("{0}{1}", ownNumStr, needNumStr);
                if (match)
                {
                    collectedWordNum++;
                }
            }
            haveCollectAllWord = collectedWordNum == data.collectWords.Count;
        }

        return haveCollectAllWord;
    }
    #endregion

    void RegisterEvent(bool reg)
    {
        if (reg)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.SYSTEM_GAME_READY, GlobalEventHandler);
        }
    }
    /// <summary>
    /// 移除被屏蔽的功能数据 cdk 手机绑定
    /// </summary>
    public void RemoveSpecialKey(bool cdkclose, bool phonebindclose) 
    {
       if(cdkclose)
       {
           uint key = (uint)WelfareType.CDKey;
           if (m_dicWelfare.ContainsKey(key))
           {
              m_dicWelfare.Remove(key);
           }
       }
       if (phonebindclose)
       {
            uint key = (uint)WelfareType.BindGift;
            if (m_dicWelfare.ContainsKey(key))
            {
                m_dicWelfare.Remove(key);
            }
       }
    }

    public string GetScheduleByType(uint welfaretype) 
    {
        string msg = "";
        if (m_dicWelfareScheduleID != null)
        {
            if (m_dicWelfareScheduleID.ContainsKey(welfaretype))
            {
                uint sche = m_dicWelfareScheduleID[welfaretype];
                msg = DataManager.Manager<DailyManager>().GetScheduleStrByID(sche);
            }
        }
        return msg;
    }
}
