using System;
using System.Collections.Generic;
using Client;
using GameCmd;
using table;
public class RideData
{

    public uint index;

    public uint id;//thisid
    public uint baseid;
    public int level;
    public int life;//寿命
    public int exp;
    public int repletion;//饱食度
    public int fight_power;

    public uint maxRepletion;//饱食度上限

    public List<int> skill_ids = new List<int>();
    public string name;
    public string icon;
    //封印道具ID
    //public uint sealID;
    public uint modelid;
    //封印减少寿命
    public uint subLife;
    //骑乘读条时长
    public uint spellTime;
    public float modelScale;
    public uint quality;
    public string QualityBorderIcon
    {
        get
        {
            return ItemDefine.GetItemBorderIcon(quality);
        }
    }

    public float GetSpeed()
    {
        return GetSpeedById_Level(baseid, level);
    }

    public static float GetSpeedById_Level(uint baseid, int level)
    {
        table.RideFeedData feeddata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(baseid, level);
        if (feeddata != null)
        {
            float value = (feeddata.speed / 10000.0f) * 100;
            return value;
        }
        return 0f;
    }

    public uint GetLevelUpExp()
    {
        table.RideFeedData feeddata = GameTableManager.Instance.GetTableItem<table.RideFeedData>(baseid, level);
        if (feeddata != null)
        {
            return feeddata.upExp;
        }
        return 0;
    }
}

/// <summary>
/// 坐骑管理
/// </summary>
public class RideManager : BaseModuleData, IManager
{
    uint m_using_ride;
    /// <summary>
    /// 正在使用的坐骑thisid
    /// </summary>
    public uint UsingRide
    {
        get { return m_using_ride; }
        set
        {
            m_using_ride = value;
            //  m_auto_ride = value;

        }
    }

    uint m_auto_ride;
    public uint Auto_Ride
    {
        get { return m_auto_ride; }
    }
    private int m_maxRideNum;
    public int MaxRideNum { get { return m_maxRideNum; } }
    /// <summary>
    /// 扩充的次数
    /// </summary>
    public int ExpandNum { get { return m_maxRideNum - GameTableManager.Instance.GetGlobalConfig<int>("MaxRideInit") + 1; } }
    /// <summary>
    /// 吃经验次数
    /// </summary>
    private Dictionary<uint, int> m_dicAddExpTimes = new Dictionary<uint, int>();

    List<RideData> m_lstRides;

    uint m_feedItemId;
    uint m_feedReletion;
    float m_lastFeedTime;
    RideData m_usingRide = null;

    public delegate void OnRidePropetyUpdate(RideData ridedata);
    OnRidePropetyUpdate m_onRidePropetyUpdate;
    public OnRidePropetyUpdate RidePropUpdateCallback;
    public Action<object> UnRideCallback { get; set; }
    public object UnRideCallbackParam { get; set; }
    public Action<object> UsingRideCallback { get; set; }

    public object UsingRideCallbackParam { get; set; }

    private float m_fRide_Feed_Interval = 0.1f;

    public bool bCheckFeed = false;
    public long nCheckFeedEndTime = 0;
    private List<uint> m_lstOwnRide = new List<uint>();

    bool m_bIsRide = false;//是否骑乘状态 true是在马上
    public bool IsRide
    {
        get
        {
            return m_bIsRide;
        }
    }
    public void ClearData()
    {

    }
    public void Initialize()
    {
        m_maxRideNum = 0;
        m_using_ride = 0;
        m_lstRides = new List<RideData>();
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option == null) return;
        m_feedItemId = (uint)option.GetInt("MedicalSetting", "RideFeedId", 0);
        if (m_feedItemId != 0)
        {
            m_feedReletion = GameTableManager.Instance.GetGlobalConfig<uint>("FeedItemRide", m_feedItemId.ToString());
        }


        string strRide_Feed_Interval = GameTableManager.Instance.GetGlobalConfig<string>("Ride_Feed_Interval");
        if (!string.IsNullOrEmpty(strRide_Feed_Interval))
        {
            float.TryParse(strRide_Feed_Interval, out m_fRide_Feed_Interval);
        }
        RegisterEvent(true);
    }
    void RegisterEvent(bool bReg)
    {
        if (bReg)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_RIDE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_UNRIDE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_MAINPLAYERCREATE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnEvent);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_RIDE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_UNRIDE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_MAINPLAYERCREATE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnEvent);
        }
    }

    public void Reset(bool depthClearData = false)
    {
        bCheckFeed = false;

        m_maxRideNum = 0;
        m_using_ride = 0;
        m_lstRides.Clear();
        m_dicAddExpTimes.Clear();
        m_lstOwnRide.Clear();

        UsingRideCallback = null;
        UsingRideCallbackParam = null;
        UnRideCallback = null;
        UnRideCallbackParam = null;

        m_bIsRide = false;
        //         Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        //         if (mainPlayer != null)
        //         {
        //             bool isChangeBody = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommand_IsChange, null);
        //             if (isChangeBody)
        //             {
        //                 mainPlayer.SetProp((int)Client.PlayerProp.TransModelResId, 0);
        //                 mainPlayer.SendMessage(Client.EntityMessage.EntityCommand_Restore, new Client.ChangeBody()
        //                 {
        //                     param = 0,
        //                     callback = null,
        //                 });
        //                 return ;
        //             }
        // 
        //         }
    }

    public void Process(float deltaTime)
    {
        //         if (bCheckFeed )
        //         {
        //             if (nCheckFeedEndTime > DateTimeHelper.Instance.ServerTime)
        //             {
        //                 return;
        //             }
        //             bCheckFeed = false;
        //         }
        //         if (m_using_ride != 0 && m_feedItemId != 0)
        //         {
        //             if (m_usingRide == null || m_usingRide.id != m_using_ride)
        //             {
        //                 m_usingRide = GetRideDataById((uint)m_using_ride);
        //             }
        // 
        //             if (m_usingRide != null)
        //             {
        //                 if (DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_feedItemId) <= 0)
        //                 {
        //                     return;
        //                 }
        //                 if ((m_usingRide.maxRepletion - m_usingRide.repletion) >= m_feedReletion && UnityEngine.Time.realtimeSinceStartup - m_lastFeedTime > m_fRide_Feed_Interval)
        //                 {
        //                     m_lastFeedTime = UnityEngine.Time.realtimeSinceStartup;
        //                     DataManager.Instance.Sender.RideFeedRideUser(m_usingRide.id, m_feedItemId);
        //                 }
        //             }
        //         }

    }

    void OnEvent(int eventID, object param)
    {
        if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_CREATEENTITY)
        {
            Client.stCreateEntity ce = (Client.stCreateEntity)param;
            Client.IEntitySystem es = Client.ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                return;
            }
            Client.IEntity en = es.FindEntity(ce.uid);
            if (en != null)
            {
                int rideId = en.GetProp((int)Client.PlayerProp.RideBaseId);
                if (rideId != 0)
                {
                    //if (Client.ClientGlobal.Instance().IsMainPlayer(en))
                    //{
                    //    UsingRide = (uint)rideId;
                    //}
                    bool isride = (bool)en.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
                    if (!isride)
                    {
                        en.SendMessage(Client.EntityMessage.EntityCommond_Ride, rideId);
                    }
                    //                    Engine.Utility.Log.LogGroup("ZCX", "EntityCommond_Ride {0}",en.GetID());
                }

                //int transModelId = en.GetProp((int)Client.PlayerProp.TransModelResId);
                //if (transModelId != 0)
                //{
                //    en.SendMessage(Client.EntityMessage.EntityCommand_Change, new Client.ChangeBody() {  resId = transModelId});
                //}
            }
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_RIDE)
        {
            Client.stEntityRide ride = (Client.stEntityRide)param;
            if (Client.ClientGlobal.Instance().IsMainPlayer(ride.uid))
            {
                if (UsingRideCallback != null)
                {
                    UsingRideCallback(UsingRideCallbackParam);
                    UsingRideCallback = null;
                    UsingRideCallbackParam = null;
                }
            }
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_UNRIDE)
        {
            Client.stEntityUnRide ride = (Client.stEntityUnRide)param;
            if (Client.ClientGlobal.Instance().IsMainPlayer(ride.uid))
            {
                if (UnRideCallback != null)
                {
                    UnRideCallback(UnRideCallbackParam);
                    UnRideCallback = null;
                    UnRideCallbackParam = null;
                }
            }
        }
        else if (eventID == (int)Client.GameEventID.ENTITYSYSTEM_MAINPLAYERCREATE)
        {
            Client.IEntity mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
            if (mainPlayer == null)
            {
                return;
            }
            //int rideId = mainPlayer.GetProp((int)Client.PlayerProp.RideBaseId);
            //if (rideId != 0)
            //{

            //    UsingRide = (uint)rideId;
            //    bool isride = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
            //    if (!isride)
            //    {
            //        mainPlayer.SendMessage(Client.EntityMessage.EntityCommond_Ride, rideId);
            //    }
            //}
        }
        else if (eventID == (int)Client.GameEventID.UIEVENT_UPDATEITEM)
        {

            List<string> tempIDList = GameTableManager.Instance.GetGlobalConfigKeyList("Knight_ExpItem");


            uint breakItemID = GameTableManager.Instance.GetGlobalConfig<uint>("KngithRankItem");

            ItemDefine.UpdateItemPassData data = (ItemDefine.UpdateItemPassData)param;
            if (data != null)
            {
                if (data.BaseId == breakItemID || tempIDList.Contains(data.BaseId.ToString()))
                {
                    DisPatchRideRedPoint();
                }
            }
        }

    }


    /// <summary>
    /// 请求读条上马
    /// </summary>
    public bool TryUsingRide(Action<object> callback, object param)
    {
        Client.IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();
        if (ms == null)
        {
            return false;
        }
        table.MapDataBase mapdata = GameTableManager.Instance.GetTableItem<table.MapDataBase>(ms.GetMapID());
        if (mapdata == null)
        {
            return false;
        }

        if (mapdata.canUsingRide != 1)
        {
            TipsManager.Instance.ShowTips("该地图不能上马");
            if (callback != null)
            {
                callback(param);
            }
            return false;
        }
        Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            Client.ISkillPart skillpart = mainPlayer.GetPart(Client.EntityPart.Skill) as Client.ISkillPart;
            if (skillpart != null)
            {
                if (skillpart.GetCurSkillState() != (int)Client.SkillState.None)
                {
                    TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_shifangjinengzhongwufashangma);
                    if (callback != null)
                    {
                        callback(param);
                    }
                    return false;
                }
            }
            bool isChangeBody = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommand_IsChange, null);
            if (isChangeBody)
            {
                if (callback != null)
                {
                    callback(param);
                }
                return false;
            }
            bool bRide = DataManager.Manager<RideManager>().IsRide;
            if (bRide)
            {
                if (callback != null)
                {
                    callback(param);
                }
                return false;
            }
            else if (Auto_Ride == 0)
            {
                if (callback != null)
                {
                    callback(param);
                }
                return false;
            }
        }

        UsingRideCallback = callback;
        UsingRideCallbackParam = param;
        if (Auto_Ride != 0)
        {
            //先发送读条 读条 结束在上马
            Client.stUninterruptMagic stparam = new Client.stUninterruptMagic();
            if (GetRideDataById(Auto_Ride) != null)
            {
                stparam.time = GetRideDataById(Auto_Ride).spellTime;
            }
            stparam.type = GameCmd.UninterruptActionType.UninterruptActionType_DEMON;
            stparam.uid = MainPlayerHelper.GetPlayerUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLGUIDE_PROGRESSSTART, stparam);
            //NetService.Instance.Send(new stUsingRideUserCmd_C());
        }
        return true;
    }

    /// <summary>
    /// true 上马了发送请求 false 没有上马
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public bool TryUnRide(Action<object> callback = null, object param = null)
    {
        Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            if (!m_bIsRide)
            {
                if (callback != null)
                {
                    callback(param);
                }
                return false;
            }
            else
            {
                UnRideCallback = callback;
                UnRideCallbackParam = param;
                NetService.Instance.Send(new GameCmd.stDownRideUserCmd_C() { });
            }
        }

        return true;
    }

    public void InitRideUserData(GameCmd.RideUserData data)
    {
        m_using_ride = (uint)data.using_ride;
        m_maxRideNum = data.max_ride;
        m_auto_ride = data.auto_ride;
        m_lstRides.Clear();
        m_lstOwnRide.Clear();
        m_lstOwnRide.AddRange(data.ride_atlas);

        for (int i = 0; i < data.ride_list.Count; i++)
        {
            AddNewRide(data.ride_list[i]);
        }
        KnightBreakLevel = data.knight_rank;
        KnightLevel = data.knight_lv;
        RideTalent = data.knight_telant;
        KnightExp = data.knight_exp;
        //for (int i = 0; i < data.ride_skill_cd.Count; i++)
        //{
        //    DataManager.Manager<SkillCDManager>().AddSkillCD(data.ride_skill_cd[i].skill, (int)data.ride_skill_cd[i].cd);
        //}
    }

    public void ExpandeMaxNum(int num)
    {
        m_maxRideNum = num;
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("ExpandeMaxNum", null, null);
        DispatchValueUpdateEvent(arg);
        TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_kuochongchenggong);
    }

    public void SetFeedConfig(uint feedid)
    {
        m_feedItemId = feedid;
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option == null) return;
        option.WriteInt("MedicalSetting", "RideFeedId", (int)m_feedItemId);
        m_feedReletion = GameTableManager.Instance.GetGlobalConfig<uint>("FeedItemRide", m_feedItemId.ToString());
    }


    public List<RideData> GetRideList()
    {
        List<RideData> lstData = new List<RideData>();
        lstData.AddRange(m_lstRides);
        return lstData;
    }

    public RideData GetCurrRideData()
    {
        for (int i = 0; i < m_lstRides.Count; i++)
        {
            if (m_lstRides[i].id == m_using_ride)
            {
                return m_lstRides[i];
            }
        }
        return null;
    }

    public RideData GetRideDataById(uint id)
    {
        for (int i = 0; i < m_lstRides.Count; i++)
        {
            if (m_lstRides[i].id == id)
            {
                return m_lstRides[i];
            }
        }
        return null;
    }

    void AddNewRide(GameCmd.RideData data)
    {
        table.RideDataBase tabledata = GameTableManager.Instance.GetTableItem<table.RideDataBase>(data.base_id);
        if (tabledata != null)
        {
            m_lstRides.Add(new RideData()
            {
                id = data.id,
                // level = data.level,
                //life = data.life,
                // exp = data.exp,
                // fight_power = data.fight_power,
                // repletion = data.repletion,
                // skill_ids = data.skill_list,
                baseid = data.base_id,
                name = tabledata.name,
                icon = tabledata.icon.ToString(),
                modelid = tabledata.viewresid,  // 使用观察ID
                spellTime = tabledata.spellTime,
                quality = tabledata.quality,
                maxRepletion = tabledata.maxRepletion,
                subLife = tabledata.subLife,
                modelScale = tabledata.modelScale * 0.01f,
            });
        }
        else
        {
            Engine.Utility.Log.Error("Not Found ride data id:{0}", data.id);
        }
    }
    public void AddRide(GameCmd.RideData data, GameCmd.AddRideAction action)
    {
        if (!m_lstOwnRide.Contains(data.base_id))
        {
            m_lstOwnRide.Add(data.base_id);
        }

        AddNewRide(data);
        UpdateRideList();
    }

    public void UpdateRideList()
    {
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("RideUpdatList", null, null);
        DispatchValueUpdateEvent(arg);
    }

    public void TransExp()
    {
        UpdateRideList();
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("ResetAdoration", null, null);
        DispatchValueUpdateEvent(arg);
    }
    public void RemoveRide(uint id)
    {
        for (int i = 0; i < m_lstRides.Count; i++)
        {
            if (m_lstRides[i].id == id)
            {
                m_lstRides.RemoveAt(i);
                UpdateRideList();
                break;
            }
        }
    }

    public int GetUseitemNum(uint itemid)
    {
        if (m_dicAddExpTimes.ContainsKey(itemid))
        {
            return m_dicAddExpTimes[itemid];
        }
        return 0;
    }

    //public void AddExp(GameCmd.stExpRideUserCmd_S cmd)
    //{
    //    //        m_dicAddExpTimes[] = addExpNum;
    //    m_dicAddExpTimes[cmd.item_id] = cmd.add_num;
    //    for (int i = 0; i < m_lstRides.Count; i++)
    //    {
    //        if (m_lstRides[i].id == cmd.id)
    //        {
    //            m_lstRides[i].exp += cmd.exp;
    //            if (RidePropUpdateCallback != null)
    //            {
    //                RidePropUpdateCallback(m_lstRides[i]);
    //            }
    //            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Ride_Rank_shiyongjingyandanchenggonghuodeXjingyan, cmd.exp);
    //            break;
    //        }
    //    }
    //}

    public void LevelUp(uint rid, int level, int exp)
    {
        for (int i = 0; i < m_lstRides.Count; i++)
        {
            if (m_lstRides[i].id == rid)
            {
                m_lstRides[i].level = level;
                m_lstRides[i].exp = exp;

                if (RidePropUpdateCallback != null)
                {
                    RidePropUpdateCallback(m_lstRides[i]);
                }
                break;
            }
        }
    }

    public bool ContainRide(uint ridebaseid)
    {
        if (m_lstOwnRide != null)
        {
            for (int i = 0; i < m_lstOwnRide.Count; i++)
            {
                if (m_lstOwnRide[i] == ridebaseid)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void SetFightRide(uint id)
    {
        m_auto_ride = id;
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("RideFightState", id, m_auto_ride);
        DispatchValueUpdateEvent(arg);
        TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Ride_Commond_Xchenggongchuzhan, m_lstRides.Find(C => C.id == id).name);
        Client.ICombatRobot robot = Client.ClientGlobal.Instance().GetControllerSystem().GetCombatRobot();
        if (robot == null)
        {
            Engine.Utility.Log.Error("robotis null");

            return;
        }
        if (robot.Status == Client.CombatRobotStatus.RUNNING)
        {
            Engine.Utility.Log.LogGroup("ZDY", "正在挂机杀怪，不直接上坐骑");
            return;
        }
        NetService.Instance.Send(new GameCmd.stUsingRideUserCmd_C());
    }

    public void CallBackRide(uint id)
    {
        m_auto_ride = 0;
        ValueUpdateEventArgs arg = new ValueUpdateEventArgs("RideFightState", id, id);
        DispatchValueUpdateEvent(arg);
        TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Ride_Commond_Xhuiquxiuxile, m_lstRides.Find(C => C.id == id).name);
    }

    public void RefreshRepletion(uint rid, int repletion)
    {
        for (int i = 0; i < m_lstRides.Count; i++)
        {
            if (m_lstRides[i].id == rid)
            {
                m_lstRides[i].repletion = repletion;

                if (RidePropUpdateCallback != null)
                {
                    RidePropUpdateCallback(m_lstRides[i]);
                    ValueUpdateEventArgs arg = new ValueUpdateEventArgs("RideUsePanelRefresh", null, repletion);
                    DispatchValueUpdateEvent(arg);
                }
                break;
            }
        }
    }
    public void RefreshRideAttr(GameCmd.RideData data)
    {
        RideData rdata = m_lstRides.Find(C => C.id == data.id);
        if (rdata == null)
        {
            Engine.Utility.Log.Error("Can Not Found ride id:{0}", data.id);
            return;
        }
        // rdata.level = data.level;
        // rdata.life = data.life;
        //rdata.repletion = data.repletion;
        //rdata.exp = data.exp;
        // rdata.fight_power = data.fight_power;
        //rdata.skill_ids = data.skill_list;

        if (RidePropUpdateCallback != null)
        {
            RidePropUpdateCallback(rdata);
        }
    }

    public void InitSkills(uint rid, List<int> skills)
    {
        RideData rdata = m_lstRides.Find(C => C.id == rid);
        if (rdata == null)
        {
            Engine.Utility.Log.Error("Can Not Found ride id:{0}", rid);
            return;
        }
        rdata.skill_ids.Clear();
        rdata.skill_ids.AddRange(skills);
    }

    public void LearnSkill(uint rid, int skillid)
    {
        RideData rdata = m_lstRides.Find(C => C.id == rid);
        if (rdata == null)
        {
            Engine.Utility.Log.Error("Can Not Found ride id:{0}", rid);
            return;
        }

        rdata.skill_ids.Add(skillid);
        if (RidePropUpdateCallback != null)
        {
            RidePropUpdateCallback(rdata);
        }

        TipsManager.Instance.ShowTips(LocalTextType.Ride_Skill_jinenglingwuchenggong);
    }
    //1－新手   2－普通
    //3－稀少   4－罕见
    //5－珍异   6－绝世
    public string GetRideQualityStr(uint quality)
    {
        LocalTextType tType = (LocalTextType)Enum.Parse(typeof(LocalTextType), "Ride_Illustrated_" + quality.ToString());
        return DataManager.Manager<TextManager>().GetLocalText(tType);

    }
    /// <summary>
    /// 标识自己上马状态
    /// </summary>
    /// <param name="cmd"></param>
    public void OnRideStatus(GameCmd.stStatusRideUserCmd_S cmd)
    {//rideid 是thisid
        if (UsingRide == cmd.ride_id && cmd.ride_id != 0)
        {
            m_bIsRide = true;
            return;
        }
        UsingRide = cmd.ride_id;
        IPlayer mainPlayer = MainPlayerHelper.GetMainPlayer();
        if (mainPlayer == null)
        {
            return;
        }
        bool isride = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
        if (cmd.ride_id == 0)
        {
            m_bIsRide = false;

            if (isride)
            {
                mainPlayer.SendMessage(Client.EntityMessage.EntityCommond_UnRide);
            }
        }
        else
        {
            m_bIsRide = true;
            if (isride)
            {
                mainPlayer.SendMessage(Client.EntityMessage.EntityCommond_UnRide);
            }
            mainPlayer.SendMessage(Client.EntityMessage.EntityCommond_Ride, GetBaseIDByThisID(cmd.ride_id));
        }
    }
    public int GetBaseIDByThisID(uint thisID)
    {
        foreach (var item in m_lstRides)
        {
            if (item.id == thisID)
            {
                return (int)item.baseid;
            }
        }
        return 0;
    }
    #region 骑术相关
    public enum RideTalentEnum
    {
        zhili,
        minjie,
        tizhi,
        liliang,
        jingshen,
    }
    public enum RideDispatchEnum
    {
        RefreshKnightExp,//刷新骑术经验
        RefreshKnightLevel,//刷新骑术等级
        RefreshKnightTalent,//刷新资质
        RefreshKnightBreakLevel,//刷新等阶
        RefreshKnightPower,//刷新坐骑战斗力
        RefreshRedPoint,//刷新红点
    }
    uint m_knightExp = 0;
    //骑术经验
    public uint KnightExp
    {
        get
        {
            return m_knightExp;
        }
        set
        {
            m_knightExp = value;
            DispatchValueUpdateEvent(RideDispatchEnum.RefreshKnightExp.ToString(), null, null);
        }
    }
    //骑术等级
    uint m_knightLevel = 0;
    public uint KnightLevel
    {
        get
        {
            return m_knightLevel;
        }
        set
        {
            m_knightLevel = value;
            if (m_knightLevel == 0)
            {
                m_knightLevel = 1;
            }
            DispatchValueUpdateEvent(RideDispatchEnum.RefreshKnightLevel.ToString(), null, null);
        }
    }
    uint m_knightBreakLevel = 0;
    //突破等阶
    public uint KnightBreakLevel
    {
        get
        {
            return m_knightBreakLevel;
        }
        set
        {
            m_knightBreakLevel = value;
            if (m_knightBreakLevel == 0)
            {
                m_knightBreakLevel = 1;
            }
            DispatchValueUpdateEvent(RideDispatchEnum.RefreshKnightBreakLevel.ToString(), null, null);
        }
    }
    //骑术资质
    public KnightTelant RideTalent
    {
        get;
        set;
    }
    uint m_uKnightPower = 0;
    //坐骑战斗力
    public uint KnightPower
    {
        get
        {
            return m_uKnightPower;
        }
        set
        {
            m_uKnightPower = value;
            DispatchValueUpdateEvent(RideDispatchEnum.RefreshKnightPower.ToString(), null, null);
        }
    }
    public void OnReceiveKnightExp(stSetKnightExpRideUserCmd_S cmd)
    {
        KnightExp = cmd.exp;
    }
    public void OnReceiveKnightLevel(stSetKnightLevelRideUserCmd_S cmd)
    {
        KnightLevel = cmd.level;
        TipsManager.Instance.ShowTips(LocalTextType.Ride_Commond_qishushengji);
    }
    public void OnReveiveKnightBreak(stSetKnightRankRideUserCmd_S cmd)
    {
        KnightBreakLevel = cmd.rank;

    }
    public void OnReveiveKnightTalent(stSetKnightTelantRideUserCmd_S cmd)
    {
        RideTalent = cmd.knight_telant;
        DispatchValueUpdateEvent(RideDispatchEnum.RefreshKnightTalent.ToString(), null, null);
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.UseItemCommonPanel))
        {
            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.UseItemCommonPanel, UIMsgID.eUseItemRefresh, UseItemCommonPanel.UseItemEnum.RideTalent);
        }
    }
    public void OnReveiveKnightPower(stSetKnightFightPowerRideUserCmd_S cmd)
    {
        KnightPower = cmd.fight_power;
    }

    #endregion
    #region redpoint
    public void DisPatchRideRedPoint()
    {
        bool bShow = false;
        if (IsShowRideRedPoint())
        {
            bShow = true;
        }
        stShowMainPanelRedPoint st = new stShowMainPanelRedPoint()
        {
            modelID = (int)WarningEnum.Ride,
            direction = (int)WarningDirection.Left,
            bShowRed = bShow,
        };
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.MAINPANEL_SHOWREDWARING, st);
        DispatchValueUpdateEvent(RideDispatchEnum.RefreshRedPoint.ToString(), null, null);
    }
    public bool IsShowRideRedPoint()
    {
        uint rank = DataManager.Manager<RideManager>().KnightBreakLevel;
        uint curExp = DataManager.Manager<RideManager>().KnightExp;
        uint lv = DataManager.Manager<RideManager>().KnightLevel;

        HoursemanShipUPLevel updb = GameTableManager.Instance.GetTableItem<HoursemanShipUPLevel>(lv+1);
        if(updb == null)
        {
            return false;
        }
        HoursemanShipUPDegree nextdb = GameTableManager.Instance.GetTableItem<HoursemanShipUPDegree>(rank + 1);
        if (nextdb != null && updb.breakLevel != 0 && curExp >= updb.uplevelexp)
        {
            return IsKnightCanBreak();
        }
        else
        {
            return IsKnightCanUP();
        }
     
    }
    public bool IsKnightCanBreak()
    {
        bool bCanBreak = false;
        uint rank = DataManager.Manager<RideManager>().KnightBreakLevel;
        uint curExp = DataManager.Manager<RideManager>().KnightExp;
        uint lv = DataManager.Manager<RideManager>().KnightLevel;

        HoursemanShipUPLevel updb = GameTableManager.Instance.GetTableItem<HoursemanShipUPLevel>(lv);
        HoursemanShipUPDegree nextdb = GameTableManager.Instance.GetTableItem<HoursemanShipUPDegree>(rank + 1);
        if (nextdb != null && updb.breakLevel != 0 && curExp >= updb.uplevelexp)
        {
            int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(nextdb.breakItemID);
            if (num >= nextdb.itemNum)
            {
                bCanBreak = true;
            }
        }
        return bCanBreak;
    }
    List<string> m_knightLevelUPIdList = null;
    public bool IsKnightCanUP()
    {
        bool bCanUP = false;
        uint lv = DataManager.Manager<RideManager>().KnightLevel;

        HoursemanShipUPLevel updb = GameTableManager.Instance.GetTableItem<HoursemanShipUPLevel>(lv+1);
        if(updb == null)
        {//最高等级
            return false;
        }
        if (m_knightLevelUPIdList == null)
        {
            m_knightLevelUPIdList = GameTableManager.Instance.GetGlobalConfigKeyList("Knight_ExpItem");

        }
        if (m_knightLevelUPIdList != null)
        {
            foreach (var item in m_knightLevelUPIdList)
            {
                uint itemID = 0;
                if (uint.TryParse(item, out itemID))
                {
                    int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemID);
                    if (num > 0)
                    {
                        bCanUP = true;
                        break;
                    }
                }
            }
        }
        return bCanUP;
    }
    #endregion
}
