using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using table;

public class MallManager : BaseModuleData,IManager,IGlobalEvent
{
    #region Property
    public const string CLASS_NAME = "MallManager";
    //<mallId,malldata>
    private Dictionary<int, MallDefine.MallData> m_dic_mallDatas = null;
    //<mallId,mallLocaldata>
    private Dictionary<uint, MallDefine.MallLocalData> m_dic_mallLocalDatas = null;
    //本地下架的商品
    private List<uint> m_listLocalUnShelveItems = null;
    public List<uint> LocalUnShelveItems
    {
        get
        {
            return m_listLocalUnShelveItems;
        }
    }

    //服务器下架的商品
    private List<uint> m_listServerUnShelveItems = null;
    public List<uint> ServerUnShelveItems
    {
        get
        {
            return m_listServerUnShelveItems;
        }
    }

    //黑市当前商品列表（服务器下发）
    private Dictionary<uint, MallDefine.BlackMarketLocalData> blackMarketItems = null;
    public Dictionary<uint, MallDefine.BlackMarketLocalData> BlackMarketItems
    {
        get
        {
            return blackMarketItems;
        }
    }

    public Protocol Sender
    {
        get
        {
            return Protocol.Instance;
        }
    }

    #endregion
    #region IManager
    public void ClearData()
    {

    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        if (null == m_dic_mallDatas)
        {
            m_dic_mallDatas = new Dictionary<int, MallDefine.MallData>();
        }
        m_dic_mallDatas.Clear();

        if (null == m_dic_mallLocalDatas)
        {
            m_dic_mallLocalDatas = new Dictionary<uint, MallDefine.MallLocalData>();
        }
        m_dic_mallLocalDatas.Clear();

        if (null == m_listLocalUnShelveItems)
            m_listLocalUnShelveItems = new List<uint>();
        m_listLocalUnShelveItems.Clear();

        if (null == m_listServerUnShelveItems)
            m_listServerUnShelveItems = new List<uint>();
        m_listServerUnShelveItems.Clear();

        if (null == blackMarketItems)
            blackMarketItems = new Dictionary<uint, MallDefine.BlackMarketLocalData>();
        blackMarketItems.Clear();

    }

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            blackMarketActiveStore = CommonStore.CommonStore_Six;
            //清空服务器下发下载数据（本地下架数据不清空）
            m_listServerUnShelveItems.Clear();
            //重置购买记录
            ResetPurchaseNumData();
            CurrencyIconData.Clear();
        }
    }

    /// <summary>
    /// 每帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {
        
    }
    #endregion

    #region IGlobalEvent
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventid"></param>
    /// <param name="data"></param>
    public void GlobalEventHandler(int eventid, object data)
    {
        switch (eventid)
        {
            case (int)Client.GameEventID.RECONNECT_SUCESS:
                {
                    //1、请求下架商品
                    GetUnShelveItemList();
                    //2、请求购买记录
                    GetPurchaseRecord();
                }
                break;
            case (int)Client.GameEventID.PLAYER_LOGIN_SUCCESS:
                {
                    //1、请求下架商品
                    GetUnShelveItemList();
                }
                break;
        }
    }
    #endregion

    #region Mall Common

    private bool m_bMallReady = false;
    public bool MallReady
    {
        get
        {
            return m_bMallReady;
        }
    }

    private void InitMallData()
    {
        if (m_bMallReady)
        {
            return;
        }
        m_bMallReady = true;

        

        List<StoreDataBase> storeDataList = GameTableManager.Instance.GetTableList<StoreDataBase>();
        int storeId = 0;
        MallDefine.MallLocalData localData = null;
        if (null != storeDataList && storeDataList.Count > 0)
        {
            for (int i = 0; i < storeDataList.Count; i++)
            {
                storeId = (int)storeDataList[i].storeId;
                if (!m_dic_mallDatas.ContainsKey(storeId))
                {
                    m_dic_mallDatas.Add(storeId, MallDefine.MallData.Create(storeId));
                }
                m_dic_mallDatas[storeId].Add(storeDataList[i]);

                localData = MallDefine.MallLocalData.Create(storeDataList[i].mallItemId, OnMallItemTagTypeChanged);
                if (null == localData)
                {
                    continue;
                }
                if (!m_dic_mallLocalDatas.ContainsKey(localData.MallId))
                {
                    m_dic_mallLocalDatas.Add(localData.MallId, localData);
                }

                if (localData.IsUnshelve && !m_listLocalUnShelveItems.Contains(localData.MallId))
                {
                    m_listLocalUnShelveItems.Add(localData.MallId);
                }
            }
        }
        else
        {
            Engine.Utility.Log.Error(CLASS_NAME + " get mall data null");
        }

        RegisterGlobalEvent(true);
    }


    /// <summary>
    /// 商品标记状态改变
    /// </summary>
    /// <param name="tagTypeData"></param>
    private void OnMallItemTagTypeChanged(uint mallItemId)
    {
        //通知UI刷新
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_TAGSTATUSCHANGED, mallItemId);
    }

    /// <summary>
    /// 构造商城客户端数据结构
    /// </summary>
    /// <param name="mallItemId"></param>
    /// <returns></returns>
    public MallDefine.MallLocalData GetMallLocalDataByMallItemId(uint mallItemId)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        MallDefine.MallLocalData localData = null;
        if (null != m_dic_mallLocalDatas 
            && m_dic_mallLocalDatas.TryGetValue(mallItemId,out localData))
        {
            return localData;
        }
        return null;
    }

    /// <summary>
    /// 获取下架商品id请求
    /// </summary>
    public void GetUnShelveItemList()
    {
        if (null != Sender)
            Sender.GetUnShelveItemListReq();
    }

    /// <summary>
    /// 获取下架商品响应
    /// </summary>
    /// <param name="unshelveItemList"></param>
    public void OnGetUnShelveItemListRes(List<uint> unshelveItemList)
    {
        //this.unShelveItems.Clear();
       
        if (null != unshelveItemList)
        {
            for(int i= 0;i< m_listServerUnShelveItems.Count;i++)
            {
                if (!m_listServerUnShelveItems.Contains(unshelveItemList[i]))
                {
                    this.m_listServerUnShelveItems.Add(unshelveItemList[i]);
                }
            }
        }
    }

    /// <summary>
    /// 获取购买记录
    /// </summary>
    private void GetPurchaseRecord()
    {

    }

    /// <summary>
    /// 服务器下发购买记录
    /// </summary>
    public void OnGetPurchaseRecord()
    {

    }

    /// <summary>
    /// 对应物品的点券价值
    /// </summary>
    /// <param name="itemBaseId"></param>
    /// <returns></returns>
    public uint GetDQPriceByItem(uint itemBaseId)
    {
        table.PointConsumeDataBase database = GameTableManager.Instance.GetTableItem<table.PointConsumeDataBase>(itemBaseId);
        if (null != database)
            return database.buyPrice;
        return 0;
    }

    #region GetMallDataByType 根据商城类型获取商城数据
    //1=商店1（点券）；
    //2=商店2（文钱）；
    //3=商店3（随身）；
    //4=商店4（声望）；
    //5=商店5（积分）；
    //6=商店6（云游商人）；

    /// <summary>
    /// 获取id
    /// </summary>
    /// <param name="mallType"></param>
    /// <param name="filterEmptyTag">过滤空的页签</param>
    /// <returns></returns>
    public List<int> GetMallTagDatas(int mallType,bool filterEmptyTag = true)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        List<int> tags = new List<int>();
        List<int> copytags = new List<int>();
        if (m_dic_mallDatas.ContainsKey(mallType))
        {
            tags.AddRange(m_dic_mallDatas[mallType].GetMallTagIds());
        }
        copytags.AddRange(tags);
        if (filterEmptyTag)
        {
            for (int i = 0; i < copytags.Count; i++)
            {
                if (GetMallDatas(mallType, copytags[i]).Count == 0)
                {
                    tags.Remove(copytags[i]);
                }
            }
        }
        return tags;
    }

    /// <summary>
    /// 获取标签名称
    /// </summary>
    /// <param name="mallType"></param>
    /// <param name="mallTag"></param>
    /// <returns></returns>
    public string GetMallTagName(int mallType,int mallTag)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        if (m_dic_mallDatas.ContainsKey(mallType))
        {
            MallDefine.MallTagData tagData = m_dic_mallDatas[mallType].GetMallTagData(mallTag);
            if (null != tagData)
            {
                return tagData.TagName;
            }
        }
        return "";
    }

    /// <summary>
    /// 根据商城类型和页签获取数据
    /// </summary>
    /// <param name="mallType"></param>
    /// <param name="mallTag"></param>
    /// <param name="filterUnshelveItems">过滤下架物品</param>
    /// <returns></returns>
    public List<uint> GetMallDatas(int mallType,int mallTag,bool filterUnshelveItems = true)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        List<uint> filterData = new List<uint>();
        if (m_dic_mallDatas.ContainsKey(mallType))
        {
            MallDefine.MallTagData tagData = m_dic_mallDatas[mallType].GetMallTagData(mallTag);
            if (null != tagData)
            {
                List<uint> datas = new List<uint>();
                datas.AddRange(tagData.GetTagIdDatas());
                if (filterUnshelveItems)
                {
                    if (null != datas && datas.Count > 0)
                    {
                        List<int> removeIndex = null;
                        //移出下架物品
                        for (int i = 0; i < datas.Count; i++)
                        {
                            if (!IsMallItemUnShelve(datas[i]))
                            {
                                filterData.Add(datas[i]);
                            }
                        }
                    }
                }else
                {
                    return datas;
                }
                
            }
        }
        
        return filterData;
    }

    /// <summary>
    /// 重置购买数量
    /// </summary>
    private void ResetPurchaseNumData()
    {
        if (!MallReady)
        {
            InitMallData();
        }
        if (null != m_dic_mallLocalDatas)
        {
            List<uint> mallItemIds = new List<uint>();
            mallItemIds.AddRange(m_dic_mallLocalDatas.Keys);
            MallDefine.MallLocalData localData = null;
            for (int i = 0; i < mallItemIds.Count; i++)
            {
                if (m_dic_mallLocalDatas.TryGetValue(mallItemIds[i], out localData))
                {
                    localData.RefreshPurchaseNum(0);
                }
            }
        }
    }

    /// <summary>
    /// 服务器下发购买数据
    /// </summary>
    /// <param name="purchaseData"></param>
    public void OnRecieveServerItemPurchaseNumData(List<GameCmd.BuyItemData> purchaseData)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        Dictionary<uint,GameCmd.BuyItemData> purchaseDic = new Dictionary<uint,GameCmd.BuyItemData>();
        if (null != purchaseData)
        {
            for(int i = 0;i < purchaseData.Count;i++)
            {
                if (!purchaseDic.ContainsKey(purchaseData[i].index))
                {
                    purchaseDic.Add(purchaseData[i].index,purchaseData[i]);
                }
            }
        }

        if (null != m_dic_mallLocalDatas)
        {
            List<uint> mallItemIds = new List<uint>();
            mallItemIds.AddRange(m_dic_mallLocalDatas.Keys);
            MallDefine.MallLocalData localData = null;
            GameCmd.BuyItemData buyItem = null;
            for(int i=0;i< mallItemIds.Count;i++)
            {
                if (m_dic_mallLocalDatas.TryGetValue(mallItemIds[i],out localData))
                {
                    if (purchaseDic.TryGetValue(mallItemIds[i],out buyItem))
                    {
                        localData.RefreshPurchaseNum((int)buyItem.times);
                    }else
                    {
                        localData.RefreshPurchaseNum(0);
                    }
                }
            }
        }

        
    }
    #endregion

    /// <summary>
    /// 购买商品
    /// </summary>
    /// <param name="mallItemId">商品唯一id</param>
    /// <param name="storeType">商店类型</param>
    /// <param name="num">购买数量</param>
    public void PurchaseMallItem(uint mallItemId,uint storeType,int num,uint pos = 0)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        MallDefine.PurchaseLimitData purchaselimitData = null;
        //购买限制检测
        if (TryGetMallItemPurchaseLimitType(mallItemId, out purchaselimitData,false))
        {
            TipsManager.Instance.ShowTips(purchaselimitData.limitDes);
            return;
        }
        if (null != Sender)
            Sender.PurchaseMallItemReq(mallItemId, storeType, (uint)num, pos);
    }

    /// <summary>
    /// 购买物品响应
    /// </summary>
    /// <param name="mallItemId"></param>
    /// <param name="num"></param>
    /// <param name="errorcode"></param>
    public void OnPurchaseMallItemRes(uint mallItemId,uint mallType,uint num,uint errorcode)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        MallDefine.MallLocalData mallData = null;
        if (!(null != m_dic_mallLocalDatas && m_dic_mallLocalDatas.TryGetValue(mallItemId,out mallData)))
        {
            TipsManager.Instance.ShowTips(string.Format("商城表格数据错误id={0}", mallItemId));
            return ;
        }
        mallData.RefreshPurchaseNum(mallData.AlreadyBuyNum + (int)num);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_REFRESHPURCHASENUM, mallItemId);
        //string tips = string.Format("成功购买{0}个{1}", num, mallData.LocalItem.BaseData.itemName);
        //TipsManager.Instance.ShowTips(tips);
    }

    /// <summary>
    /// 出售当个物品
    /// </summary>
    /// <param name="qwThisId"></param>
    public void Sell(uint qwThisId)
    {
        List<uint> sellList = new List<uint>();
        sellList.Add(qwThisId);
        Sell(sellList);
    }

    /// <summary>
    /// 出售商品(列表)
    /// </summary>
    /// <param name="sellList"></param>
    public void Sell(List<uint> sellList)
    {
        if (null != Sender)
            Sender.SellItemListReq(sellList);
    }
    /// <summary>
    /// 出售商品响应
    /// </summary>
    /// <param name="sellList"></param>
    public void OnSellRes(List<uint> sellList)
    {

    }

    /// <summary>
    /// 该商品是否已经下架
    /// </summary>
    /// <param name="mallItemId"></param>
    /// <returns></returns>
    public bool IsMallItemUnShelve(uint mallItemId)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        return ((null != m_listLocalUnShelveItems && m_listLocalUnShelveItems.Contains(mallItemId))
            || (null != m_listServerUnShelveItems && m_listServerUnShelveItems.Contains(mallItemId)));
    }

    #endregion

    #region 点券 文钱

    /// <summary>
    /// 获取本地日程数据
    /// </summary>
    /// <param name="scheduleId"></param>
    /// <returns></returns>
    public table.ScheduleDataBase GetScheduleDataBase(uint scheduleId)
    {
        return GameTableManager.Instance.GetTableItem<table.ScheduleDataBase>(scheduleId);
    }

    #endregion

    #region 流动商城
    //单个页签数据量
    public const int BLACKMARKET_SINGLE_TAB_DATA_COUNT = 6;
    //刷新事件间隔
    public const string DYNASTORE_REFRESH_TIME = "dynastore_refresh_time";
    //刷新消费
    public const string DYNASTORE_REFRESH_COST = "dynastore_refresh_cost";
    //刷新消费类型
    public const string DYNASTORE_COST_TYPE = "dynastore_cost_type";
    //流动商城当前活跃的商城
    private GameCmd.CommonStore blackMarketActiveStore = CommonStore.CommonStore_Six;
    public GameCmd.CommonStore BlackMarketActiveStore
    {
        get
        {
            return blackMarketActiveStore;
        }
    }
    //当前商城活动页
    private int blackMarketActiveTabIndex = 0;
    public int BlackMarketActiveTabIndex
    {
        get
        {
            return blackMarketActiveTabIndex;
        }
    }
    //当前商城一共有多少页
    public int BlackMarketCurTabCount
    {
        get
        {
            return 0;
        }
    }


    /// <summary>
    /// 设置流动商城当前活跃的商城
    /// </summary>
    /// <param name="activeStore"></param>
    /// <param name="force">是否强制刷新</param>
    public void SetBlackMarkeActiveStore(GameCmd.CommonStore activeStore,bool force = false)
    {
        if (blackMarketActiveStore == activeStore && !force)
            return;
        this.blackMarketActiveStore = activeStore;
        //重置活动页签
        this.blackMarketActiveTabIndex = 0;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTBLACKACTIVESTORECHANGED, blackMarketActiveStore);
    }

    /// <summary>
    /// 设置当前活动页索引
    /// </summary>
    /// <param name="index"></param>
    public void SetActiveTabIndex(int index)
    {
        if (blackMarketActiveTabIndex == index || index >= BlackMarketCurTabCount)
            return;
        int preIndex = blackMarketActiveTabIndex;
        blackMarketActiveTabIndex = index;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTBLACKACTIVETABCHANGED, preIndex);
    }
    /// <summary>
    /// 获取流动商城数据
    /// </summary>
    /// <param name="mallType"> 商城类型为0表示，获取当前活动商城</param>
    /// <returns></returns>
    public List<uint> GetBlackMarketPosInfoByMallType(uint mallType = 0)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        List<uint> result = new List<uint>();
        if (mallType == 0)
            mallType = (uint)blackMarketActiveStore;
        if (blackMarketItems.ContainsKey(mallType) && null != blackMarketItems[mallType])
        {
            result.AddRange(blackMarketItems[mallType].GetMallItemPosList());
        }
        return result;
    }

    /// <summary>
    /// 获取合适数据
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="malltype"></param>
    /// <returns></returns>
    public GameCmd.DynaStorePosInfo GetBlackMarketPosInfo(uint pos, uint mallType = 0)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        if (mallType == 0)
            mallType = (uint)blackMarketActiveStore;
        GameCmd.DynaStorePosInfo info = null;
        if (blackMarketItems.ContainsKey(mallType) 
            && null != blackMarketItems[mallType]
            && blackMarketItems[mallType].TryGetMallItemPosInfo(pos, out info))
        {
            return info;
        }
        return null;
    }
    /// <summary>
    /// 刷新流动商城数据
    /// </summary>
    public void RefreshMall()
    {
        //1、检测刷新次数是否够
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.RefreshMall((uint) blackMarketActiveStore);
    }

    /// <summary>
    /// 刷新流动商城数据
    /// </summary>
    /// <param name="refreshType">刷新类型</param>
    /// <param name="infos">数据</param>
    public void OnRefreshMall(uint refreshType,List<GameCmd.DynaStoreInfo> infos)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        //重构 || 刷新
        bool create = false;
        switch(refreshType)
        {
            case (uint)GameCmd.StoreRefType.StoreRefType_Login:
            case (uint)GameCmd.StoreRefType.StoreRefType_Auto:
            case (uint)GameCmd.StoreRefType.StoreRefType_Handle:
                
                //blackMarketItems.Clear();
                create = true;
                break;
            case (uint)GameCmd.StoreRefType.StoreRefType_Buy:
                create = false;
                break;
        }

        if (null != infos && infos.Count > 0)
        {
            MallDefine.BlackMarketLocalData data = null;
            foreach(GameCmd.DynaStoreInfo info in infos)
            {
                if (!blackMarketItems.TryGetValue(info.store_id,out data))
                {
                    data = new MallDefine.BlackMarketLocalData(info);
                    blackMarketItems.Add(info.store_id, data);
                }else
                {
                    data.UpdateData(info);
                }
            }
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTBLACKMARKETREFRESH, create);
        if (refreshType == (uint)GameCmd.StoreRefType.StoreRefType_Handle)
        {
            TipsManager.Instance.ShowTips("物品已刷新");
        }
    }

    /// <summary>
    /// 是否当流动商城内是否存在该商品
    /// </summary>
    /// <param name="mallItemId"></param>
    /// <returns></returns>
    public bool IsBlackMarketContentMallItem(uint mallItemId)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        bool exist = false;
        if (null != BlackMarketItems)
        {
            foreach (MallDefine.BlackMarketLocalData info in BlackMarketItems.Values)
            {
               if (info.Contains(mallItemId))
               {
                   exist = true;
                   break;
               }
            }
        }
        
        return exist;
    }

    /// <summary>
    /// 是否该商品已经售罄
    /// </summary>
    /// <param name="mallItemId"></param>
    /// <returns></returns>
    public bool IsMallItemSoldOut(uint mallItemId,uint pos)
    {
        bool soldOut = false;

        return soldOut;
    }

    /// <summary>
    /// 获取流动商城的手动刷新次数
    /// </summary>
    /// <param name="mallType"></param>
    /// <returns></returns>
    public uint GetBlackMarketHandRefreshTimes(uint mallType = 0)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        if (mallType == 0)
        {
            mallType = (uint)blackMarketActiveStore;
        }
        return (null != blackMarketItems && blackMarketItems.ContainsKey(mallType) ? blackMarketItems[mallType].HandRefreshTimes : 0);
    }

    /// <summary>
    /// 获取流动商城刷新间隔时间
    /// </summary>
    /// <param name="mallType"></param>
    /// <returns></returns>
    public float GetBlackMarketRefreshGapTime(uint mallType)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        return (null != blackMarketItems && blackMarketItems.ContainsKey(mallType) ? blackMarketItems[mallType].AutoRefreshGapTime : 0);
    }

    /// <summary>
    /// 获取距离下次刷新剩余时间
    /// </summary>
    /// <param name="mallType"></param>
    /// <returns></returns>
    public long GetBlackMarketNextAutoRefreshTimeStamp(uint mallType)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        return (null != blackMarketItems && blackMarketItems.ContainsKey(mallType)
            ? blackMarketItems[mallType].NextAutoRefreshTimeStamp : 0);
    }

    /// <summary>
    /// 获取商品购买限制
    /// </summary>
    /// <param name="mallItemId">商品id</param>
    /// <param name="limitData">限制类型 优先级 NotEnough>UnShelve>SoldOut>Vip>CharacterLv</param>
    /// <param name="marketGridLimitDes">是否为黑市格子限制描述</param>
    /// <returns></returns>
    public bool TryGetMallItemPurchaseLimitType(uint mallItemId ,out MallDefine.PurchaseLimitData limitData
        ,bool marketGridLimitDes = true,bool packageFullCheck = true)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        bool success = false;
        limitData = new MallDefine.PurchaseLimitData();
        MallDefine.MallLocalData local = MallDefine.MallLocalData.Create(mallItemId);
        if (null == local)
            return success;
        //限制判断
        if (packageFullCheck && DataManager.Manager<KnapsackManager>().IsSimpleKanpsackFull())
        {
            limitData.limitType = MallDefine.PurchaseLimitType.NotEnough;
            //1、背包空间
            limitData.limitDes = "背包已满";
            success = true;
        }
        else if (local.LocalMall.vipLev != 0
            && Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.PlayerProp.Vip) < local.LocalMall.vipLev)
        {
            //2、vip
            limitData.limitType = MallDefine.PurchaseLimitType.Vip;
            limitData.limitDes = (marketGridLimitDes) ? string.Format("VIP{0}可购", local.LocalMall.vipLev) : string.Format("需要VIP{0}才可购买", local.LocalMall.vipLev);
            success = true;
        }
        else if (local.LocalMall.lev != 0
            && Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level) < local.LocalMall.lev)
        {
            //3、主角等级
            limitData.limitType = MallDefine.PurchaseLimitType.CharacterLv;
            limitData.limitDes = (marketGridLimitDes) ? string.Format("{0}级才可购", local.LocalMall.lev) : string.Format("需要达到{0}级才可购买", local.LocalMall.lev);
            success = true;
        }
        //else if (IsMallItemSoldOut(mallItemId.))
        //{
        //    //4、是否售罄
        //    limitData.limitType = MallDefine.PurchaseLimitType.SoldOut;
        //    limitData.limitDes = "该商品已售罄";
        //    success = true;
        //}
        else if (IsMallItemUnShelve(mallItemId))
        {
            //5、是否下架
            limitData.limitType = MallDefine.PurchaseLimitType.UnShelve;
            limitData.limitDes = "该商品已下架";
            success = true;
        }
        return success;
    }

    /// <summary>
    /// 获取流动商城下次刷新剩余时间字符串
    /// </summary>
    /// <param name="mallType">0表示当前活动商城</param>
    /// <returns></returns>
    public string GetBlackMallNextRefreshLeftTimeStr(uint mallType = 0)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        if (mallType == 0)
            mallType = (uint)blackMarketActiveStore;
        string strFormat = "下次刷新时间 {0}";
        long leftSecons = 0;
        long now = DateTimeHelper.Instance.Now;
        if (ScheduleDefine.ScheduleUnit.IsInCycleDateTme(
            now
            ,GetBlackMarketNextAutoRefreshTimeStamp(mallType)
            ,now,out leftSecons))
        {

        }
        TimeSpan ts = new TimeSpan(0, 0, 0, (int)leftSecons, 0);
        strFormat = string.Format(strFormat
            , ts.ToString());
        return strFormat;
    }

    /// <summary>
    /// 获取流动商城刷新数据
    /// </summary>
    /// <param name="mallType"></param>
    /// <returns></returns>
    public MallDefine.BlackMarketRefreshData GetBlackMallRefreshData(int mallType = 0)
    {
        if (!MallReady)
        {
            InitMallData();
        }
        GameCmd.CommonStore targetStore = (mallType == 0)? blackMarketActiveStore
            : (GameCmd.CommonStore)mallType;
        MallDefine.BlackMarketRefreshData refreshData = new MallDefine.BlackMarketRefreshData();
        table.NobleDataBase ndb =
            GameTableManager.Instance.GetTableItem<table.NobleDataBase>(DataManager.Manager<Mall_HuangLingManager>().NobleID);
        if (null == ndb)
        {
            return refreshData;
        }

        //刷新消费
        string refreshCostString =
            GameTableManager.Instance.GetGlobalConfig<string>(DYNASTORE_REFRESH_COST);
        if (null == refreshCostString)
        {
            return refreshData;
        }
        string[] refreshCostStringArray = refreshCostString.Split(new char[] { '_' });
        if (null == refreshCostStringArray && refreshCostStringArray.Length != 5)
        {
            return refreshData;
        }

        //刷新消费类型
        string refreshCostType =
            GameTableManager.Instance.GetGlobalConfig<string>(DYNASTORE_COST_TYPE); 
        if (null == refreshCostString)
        {
            return refreshData;
        }
        string[] refreshCostTypeArray = refreshCostType.Split(new char[] { '_' });
        if (null == refreshCostTypeArray && refreshCostTypeArray.Length != 5)
        {
            return refreshData;
        }

        //刷新时间间隔
        string refreshGap =
            GameTableManager.Instance.GetGlobalConfig<string>(DYNASTORE_REFRESH_TIME);
        if (null == refreshCostString)
        {
            return refreshData;
        }
        string[] refreshGapArray = refreshGap.Split(new char[] { '_' });
        if (null == refreshGapArray && refreshGapArray.Length != 5)
        {
            return refreshData;
        }
        int type = 0;
        int num = 0;
        switch (targetStore)
        {
            case GameCmd.CommonStore.CommonStore_HundredOne:
                //声望
                {
                    refreshData.RefreshCount = (int)ndb.repRefCount;
                    int.TryParse(refreshGapArray[0], out refreshData.RefreshGap);
                    int.TryParse(refreshCostTypeArray[0], out type);
                    int.TryParse(refreshCostStringArray[0], out num);
                }
                break;
            case GameCmd.CommonStore.CommonStore_HundredTwo:
                //积分
                {
                    refreshData.RefreshCount = (int)ndb.scoreRefCount;
                    int.TryParse(refreshGapArray[1], out refreshData.RefreshGap);
                    int.TryParse(refreshCostTypeArray[1], out type);
                    int.TryParse(refreshCostStringArray[1], out num);
                }
                break;
            case GameCmd.CommonStore.CommonStore_HundredThree:
                //黑市
                {
                    refreshData.RefreshCount = (int)ndb.blackRefCount;
                    int.TryParse(refreshGapArray[2], out refreshData.RefreshGap);
                    int.TryParse(refreshCostTypeArray[2], out type);
                    int.TryParse(refreshCostStringArray[2], out num);
                }
                break;
            case GameCmd.CommonStore.CommonStore_HundredFour:
                //战勋
                {
                    refreshData.RefreshCount = (int)ndb.campScoreRefCount;
                    int.TryParse(refreshGapArray[3], out refreshData.RefreshGap);
                    int.TryParse(refreshCostTypeArray[3], out type);
                    int.TryParse(refreshCostStringArray[3], out num);
                }
                break;
            case GameCmd.CommonStore.CommonStore_HundredFive:
                //战勋
                {
                    refreshData.RefreshCount = (int)ndb.shouLieScoreRefCount;
                    int.TryParse(refreshGapArray[4], out refreshData.RefreshGap);
                    int.TryParse(refreshCostTypeArray[4], out type);
                    int.TryParse(refreshCostStringArray[4], out num);
                }
                break;
        }
        refreshData.RefreshCost = new MallDefine.CurrencyData((MoneyType)
                        type, (uint)num);
        return refreshData;
    }

    #endregion


    #region 货币图标


  
   
    #endregion
}
public class CurrencyIconData 
{
     public ClientMoneyType type;
     public uint itemID;
     public string bigIconName;
     public string smallIconName;
     public uint BigIconAtlasID;
     public uint SmallIconAtlasID;

    public const string CONST_CURRENCY_ICON = "CurrencyIcon";
    private static Dictionary<int, CurrencyIconData> m_dicCacheIconData = null;
    public static CurrencyIconData GetCurrencyIconByMoneyType(ClientMoneyType type) 
    {
        if (null == m_dicCacheIconData)
        {
            m_dicCacheIconData = new Dictionary<int, CurrencyIconData>();
        }
        int moneyType = (int)type;
        CurrencyIconData data = null;
        if (!m_dicCacheIconData.TryGetValue(moneyType,out data))
        {
            string stringKeys = GameTableManager.Instance.GetGlobalConfig<string>(CONST_CURRENCY_ICON, ((uint)type).ToString());
            if (!string.IsNullOrEmpty(stringKeys))
            {
                string[] args = stringKeys.Split('|');
                if (args.Length != 2 || string.IsNullOrEmpty(args[0]))
                {
                    Engine.Utility.Log.Error("全局配置表中的货币图标参数无法解析");
                }
                else
                {
                    data = new CurrencyIconData();
                    data.type = type;
                    data.smallIconName = args[0];
                    data.SmallIconAtlasID = DataManager.Manager<UIManager>().GetResIDByFileName(true, args[0]);
                    if (args[1] != "")
                    {
                        uint itemID = 0;
                        if (uint.TryParse(args[1], out itemID))
                        {
                            ItemDataBase item = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemID);
                            if (item != null)
                            {
                                data.itemID = itemID;
                                data.bigIconName = item.itemIcon;
                                data.BigIconAtlasID = DataManager.Manager<UIManager>().GetResIDByFileName(true, item.itemIcon);
                            }
                        }
                    }
                    m_dicCacheIconData.Add(moneyType, data);
                }
            }
        }
        return data;
    }

    public static void Clear()
    {
        m_dicCacheIconData = null;
    }
}