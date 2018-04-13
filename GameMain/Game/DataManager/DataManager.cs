using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 管理器控制器
/// </summary>
public class DataManager : Singleton<DataManager>
{
    #region Property
    public const string CLASS_NAME = "DataManager";
    private Dictionary<Type, IManager> managerDic;

    private bool m_bready = false;
    public bool Ready
    {
        get
        {
            return m_bready;
        }
    }

    //MainObject
    private GameObject mainobj;
    public GameObject MainObj
    {
        get
        {
            if (null == mainobj)
                mainobj = GameObject.Find("MainObject");
            return mainobj;
        }
    }

    //主角
    public Client.IPlayer MainPlayer
    {
        get
        {
            return Client.ClientGlobal.Instance().MainPlayer;
        }
    }

    /// <summary>
    /// 当前玩家的id
    /// </summary>
    public uint UserId
    {
        get
        {
            return (null != MainPlayer) ? MainPlayer.GetID() : 0;
        }
    }

    /// <summary>
    /// 客户端唯一id
    /// </summary>
    public long UId
    {
        get
        {
            return (null != MainPlayer) ? MainPlayer.GetUID() : 0;
        }
    }

    /// <summary>
    /// 当前id是否为自己
    /// </summary>
    /// <param name="uId"></param>
    /// <returns></returns>
    public bool IsMainPlayer(long uId)
    {
        return (UId == uId) ? true : false;
    }

    //角色等级
    public int PlayerLv
    {
        get
        {
            Client.IPlayer mainPlayer = MainPlayer;
            return (null != mainPlayer) ? mainPlayer.GetProp((int)Client.CreatureProp.Level) : 0;
        }
    }

    //通讯协议
    private Protocol sender;
    public Protocol Sender
    {
        get
        {
            if (null == sender)
                sender = Protocol.Instance;
            return sender;
        }
    }

    private List<uint> m_lstTemp = null;
    public List<uint> TempUintList
    {
        get
        {
            m_lstTemp.Clear();
            return m_lstTemp;
        }
    }
    #endregion

    #region IManager Method
    private List<Type> GetMgrData()
    {
        List<Type> mgrDatas = new List<Type>();
        mgrDatas.Add(typeof(CMObjPoolManager));
        mgrDatas.Add(typeof(CMAssetBundleLoaderMgr));
        mgrDatas.Add(typeof(CMResourceMgr));
        mgrDatas.Add(typeof(LoginDataManager));
        mgrDatas.Add(typeof(UIManager));
        mgrDatas.Add(typeof(UIPanelManager));
        mgrDatas.Add(typeof(ItemManager));
        mgrDatas.Add(typeof(KnapsackManager));
        mgrDatas.Add(typeof(EquipManager));
        mgrDatas.Add(typeof(PetDataManager));
        mgrDatas.Add(typeof(MallManager));
        mgrDatas.Add(typeof(TaskDataManager));
        mgrDatas.Add(typeof(ChatDataManager));
        mgrDatas.Add(typeof(RenderTextureManager));
        mgrDatas.Add(typeof(LearnSkillDataManager));
        mgrDatas.Add(typeof(RelationManager));
        mgrDatas.Add(typeof(MailManager));
        mgrDatas.Add(typeof(ReLiveDataManager));
        mgrDatas.Add(typeof(MapDataManager));
        mgrDatas.Add(typeof(TeamDataManager));
        mgrDatas.Add(typeof(RideManager));
        mgrDatas.Add(typeof(HomeDataManager));
        mgrDatas.Add(typeof(ArenaManager));
        mgrDatas.Add(typeof(RankManager));
        mgrDatas.Add(typeof(SkillCDManager));
        mgrDatas.Add(typeof(ClanManger));
        mgrDatas.Add(typeof( Mall_HuangLingManager));
        mgrDatas.Add(typeof(ComBatCopyDataManager));
        mgrDatas.Add(typeof(BoxTimeUpManager));
        mgrDatas.Add(typeof(SaleMoneyDataManager));
        mgrDatas.Add(typeof(BuffDataManager));
        mgrDatas.Add(typeof(TitleManager));
        mgrDatas.Add(typeof(ComposeManager));
        mgrDatas.Add(typeof(ConsignmentManager));
        mgrDatas.Add(typeof(TextManager));
        mgrDatas.Add(typeof(DailyManager));
        mgrDatas.Add(typeof(WelfareManager));
        mgrDatas.Add(typeof(HuntingManager));
        mgrDatas.Add(typeof(HeartSkillManager));
        mgrDatas.Add(typeof(ArenaSetSkillManager));
        mgrDatas.Add(typeof(CampCombatManager));
        mgrDatas.Add(typeof(AchievementManager));
        mgrDatas.Add(typeof(ActivityManager));
        mgrDatas.Add(typeof(NvWaManager));
        mgrDatas.Add(typeof(CommonPushDataManager));
        mgrDatas.Add(typeof(FunctionPushManager));
        mgrDatas.Add(typeof( CommonWaitDataManager));
        mgrDatas.Add(typeof(SuitDataManager));
        mgrDatas.Add(typeof(GuideManager));
        mgrDatas.Add(typeof(CityWarManager));
        mgrDatas.Add(typeof(Mall_NpcShopManager));
        mgrDatas.Add(typeof(GrowUpManager));
        mgrDatas.Add(typeof(QuestionManager));
        mgrDatas.Add(typeof(OfflineManager));
        mgrDatas.Add(typeof(TipsManager));
        mgrDatas.Add(typeof(SkillModuleData));
        mgrDatas.Add(typeof(SliderDataManager));
        mgrDatas.Add(typeof(TreasureManager));
        mgrDatas.Add(typeof(RoleStateBarManager));
        mgrDatas.Add(typeof(DailyTestManager));
        mgrDatas.Add(typeof(DailyAnswerManager));
        mgrDatas.Add(typeof(FishingManager));
        mgrDatas.Add(typeof(RechargeManager));
        mgrDatas.Add(typeof(RedEnvelopeDataManager));
        mgrDatas.Add(typeof(ForgingManager));
        mgrDatas.Add(typeof(AnswerManager));
        mgrDatas.Add(typeof(FunctionShieldManager));
        //其他数据管理请写在Setting上面，保证SettingMgr是最后一个初始化的
        mgrDatas.Add(typeof(Client.SettingManager));
        mgrDatas.Add(typeof(EffectDisplayManager));
        mgrDatas.Add(typeof(JvBaoBossWorldManager));
        
        return mgrDatas;
    }
    public void Create(Action<float> progress = null)
    {
        Engine.Utility.Log.Info(CLASS_NAME + "-> Initialize");
        managerDic = new Dictionary<Type, IManager>();

        List<Type> mgrTypes = GetMgrData();
        int count = 0;
        for (int i = 0, max = mgrTypes.Count; i < max;i++ )
        {
            Add(mgrTypes[i]);
        }

        Action<float> subProgressAction = (subProgress) =>
            {
                if (null != progress)
                {
                    progress.Invoke(0.1f + 0.9f * subProgress);
                }
            };
        Initialize(subProgressAction);
    }

    /// <summary>
    ///初始化
    /// </summary>
    public void Initialize(Action<float> progress = null)
    {
        int totalCount = managerDic.Count;
        int initCount = 0;
        m_lstTemp = new List<uint>();
        foreach (IManager mgr in managerDic.Values)
        {
            mgr.Initialize();
            totalCount++;
            if (null != progress)
            {
                progress.Invoke(initCount / (float)totalCount);
            }
        }
        m_bready = true;
    }

    /// <summary>
    /// 重置管理器
    /// </summary>
    /// <param name="depthClearData"></param>
    /// <param name="ignoreType">忽略管理器</param>
    public void Reset(bool depthClearData = false,List<Type> ignoreType = null)
    {
        if (!Ready)
        {
            return;
        }
        Engine.Utility.Log.Info(CLASS_NAME + "-> Reset");
        foreach (IManager mgr in managerDic.Values)
        {
            if (null != ignoreType && ignoreType.Contains(mgr.GetType()))
            {
                continue;
            }
            mgr.ClearData();
            mgr.Reset(depthClearData);
        }
        if (depthClearData)
        {
            mainobj = null;
            sender = null;
        }
    }

    public void ClearData()
    {
        if (!Ready)
        {
            return;
        }
        foreach (IManager mgr in managerDic.Values)
        {
            mgr.ClearData();
        }
    }
    public void ResetByReconnect()
    {
        List<Type> ignoreList = new List<Type>();
        ignoreList.Add(typeof(LoginDataManager));
        ignoreList.Add(typeof(CommonWaitDataManager));
        ignoreList.Add(typeof(UIPanelManager));
        Reset(true, ignoreList);
    }
    public void Process(float deltaTime)
    {
        if (!Ready)
        {
            return;
        }
        if (managerDic != null)
        {
          //  foreach (IManager mgr in managerDic.Values) 去foreach modify by dianyu
            var iter = managerDic.GetEnumerator();
            while(iter.MoveNext())
            {
                var mgr = iter.Current.Value;
                mgr.Process(deltaTime);
            }
        }
    }

    #endregion

    /// <summary>
    /// 获取管理器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Manager<T>() where T : IManager
    {
        IManager mgr = null;
        if (Instance.managerDic.TryGetValue(typeof(T), out mgr))
        {
            return (T)mgr;
        }
        return default(T);
    }

    /// <summary>
    /// 当前帧数量对rint取余是否等于0
    /// </summary>
    /// <param name="rInt"></param>
    /// <returns></returns>
    public static bool IsFrameCountRemainderEq0(int rInt = 5)
    {
        bool match = false;
        if (rInt > 0 && Time.frameCount % rInt == 0)
        {
            match = true;
        }
        return match;
    }

    /// <summary>
    /// 添加管理器
    /// </summary>
    /// <param name="mgr"></param>
    /// <param name="needInit">是否需要初始化（注：一般外部调用）</param>
    public void Add(IManager mgr, bool needInit = false)
    {
        Type t = mgr.GetType();
        if (managerDic.ContainsKey(t))
        {
            Engine.Utility.Log.Warning(CLASS_NAME + "-> Add MGR Faield,exist {0}", t.Name);
            return;
        }
        managerDic.Add(t, mgr);
        if (needInit)
            mgr.Initialize();
    }

    public void Add(Type mgrType,bool needInit = false)
    {
        IManager imgr = (IManager)Activator.CreateInstance(mgrType);
        Add(imgr, needInit);
    }
    /// <summary>
    /// 根据货币类型获取当前拥有数量
    /// </summary>
    /// <param name="mType"></param>
    /// <returns></returns>
    public int GetCurrencyNumByType(GameCmd.MoneyType mType)
    {
        int num = 0;
        Client.IPlayer player = MainPlayer;
        if (null != player)
        {
            switch (mType)
            {
                case GameCmd.MoneyType.MoneyType_Gold:
                    num = player.GetProp((int)Client.PlayerProp.Coupon);
                    break;
                case GameCmd.MoneyType.MoneyType_Coin:
                    num = player.GetProp((int)Client.PlayerProp.Cold);
                    break;
                case GameCmd.MoneyType.MoneyType_MoneyTicket:
                    num = player.GetProp((int)Client.PlayerProp.Money);
                    break;
                case GameCmd.MoneyType.MoneyType_Reputation:
                    num = player.GetProp((int)Client.PlayerProp.Reputation);
                    break;
                case GameCmd.MoneyType.MoneyType_Score:
                    num = player.GetProp((int)Client.PlayerProp.Score);
                    break;
            }
        }
        return num;
    }

    /// <summary>
    /// 当前职业是否匹配
    /// </summary>
    /// <param name="job"></param>
    /// <returns></returns>
    public static bool IsMatchPalyerJob(int job)
    {
        Client.IPlayer player = DataManager.Instance.MainPlayer;
        if (null == player)
        {
            return false;
        }
        int pjob = player.GetProp((int)Client.PlayerProp.Job);
        return (job == 0 || (job == pjob));
    }

    /// <summary>
    /// 职业
    /// </summary>
    /// <returns></returns>
    public static int Job()
    {
        Client.IPlayer player = DataManager.Instance.MainPlayer;
        if (null == player)
        {
            return 0;
        }
        int pjob = player.GetProp((int)Client.PlayerProp.Job);
        return pjob;
    }


    /// <summary>
    /// 是否满足等级要求
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static bool IsMatchPalyerLv(int lv)
    {
        Client.IPlayer player = DataManager.Instance.MainPlayer;
        if (null == player)
        {
            return false;
        }
        int plv = player.GetProp((int)Client.CreatureProp.Level);
        return (plv >= lv);
    }

    /// <summary>
    /// 获取对应职业的图标
    /// </summary>
    /// <param name="profession"></param>
    /// <returns></returns>
    public static string GetIconByProfession(uint profession)
    {
        string icon = "";
        switch (profession)
        {
            //战士
            case (uint)GameCmd.enumProfession.Profession_Soldier:
                icon = "tubiao_zhanshi";
                break;
            //暗巫
            case (uint)GameCmd.enumProfession.Profession_Doctor:
                icon = "tubiao_anwu";
                break;
            //刺客(幻师)
            case (uint)GameCmd.enumProfession.Profession_Spy:
                icon = "tubiao_huanshi";
                break;
            //法师
            case (uint)GameCmd.enumProfession.Profession_Freeman:
                icon = "tubiao_fashi";
                break;
        }
        return icon;
    }

}