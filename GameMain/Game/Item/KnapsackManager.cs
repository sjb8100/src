using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;

public class KnapsackManager : BaseModuleData, IManager, IGlobalEvent
{
    #region define
    public class UnlockPassData
    {
        //背包类型
        public uint Type = 0;
        //重连
        public bool IsReconnect = false;
    }
    #endregion

    #region property
    public const string CLASS_NAME = "KnapsackManager";
    //管理器是否准备好
    private bool ready = false;
    public bool Ready
    {
        get
        {
            return ready;
        }
    }
    //网络断开
    private bool m_IsNetWorkClose = false;
    //背包过格子布局最大列数
    public const int KNAPSACK_GRID_COLUMN_MAX = 5;
    //背包初始格子数
    public const int KNPASACK_INIT_GRID_NUM = 25;
    //本地背包解锁信息
    private Dictionary<uint, KnapsackDefine.LocalUnlockInfo> m_localUnlockInfo = null;

    /// <summary>
    /// 背包默认显示存储呢类型
    /// </summary>
    private GameCmd.PACKAGETYPE storageType = GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN;
    public GameCmd.PACKAGETYPE StorageType
    {
        get
        {
            return storageType;
        }
    }

    /// <summary>
    /// 仓库里金币存储数量
    /// </summary>
    private uint wareHouseStoreCopperNum = 0;
    public uint WareHouseStoreCopperNum
    {
        get
        {
            return wareHouseStoreCopperNum;
        }
    }
    #endregion

    #region IManager Method
    public void Initialize()
    {
        if (null == m_localUnlockInfo)
        {
            m_localUnlockInfo = new Dictionary<uint, KnapsackDefine.LocalUnlockInfo>();
        }

        m_dicTidyCD = new Dictionary<PACKAGETYPE, long>();
        ready = true;
        RegisterGlobalEvent(true);
    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            wareHouseStoreCopperNum = 0;
            m_localUnlockInfo.Clear();
            storageType = GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN;
            m_dicTidyCD.Clear();
            m_IsNetWorkClose = false;
        }
    }
    public void ClearData()
    {

    }
    public void Process(float deltaTime)
    {

    }
    #endregion
    #region IGlobalEvent

    /// <summary>
    /// 事件注册
    /// </summary>
    /// <param name="regster"></param>
    public void RegisterGlobalEvent(bool regster)
    {
        if (regster)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.NETWORK_CONNECTE_CLOSE, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.NETWORK_CONNECTE_CLOSE, GlobalEventHandler);
        }
    }

    /// <summary>
    /// 全局UI事件处理器
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    public void GlobalEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.RECONNECT_SUCESS:
                {

                }
                break;

            case (int)Client.GameEventID.NETWORK_CONNECTE_CLOSE:
                {
                    m_IsNetWorkClose = true;
                }
                break;
        }
    }
    #endregion
    #region Knapsack Operator
    private Dictionary<GameCmd.PACKAGETYPE, long> m_dicTidyCD = null;
    private const string TIDY_CD_TIME_NAME = "TidyCDTime";
    public static int TidyCDTime
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfig<int>(TIDY_CD_TIME_NAME);
        }
    }

    public bool IsTidyCD(GameCmd.PACKAGETYPE pType)
    {
        long lastTidyTime = 0;
        if (m_dicTidyCD.TryGetValue(pType, out lastTidyTime)
            && DateTimeHelper.Instance.Now - lastTidyTime < TidyCDTime)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 整理背包
    /// </summary>
    /// <param name="pType">背包类型</param>
    public void Tidy(GameCmd.PACKAGETYPE pType = GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN)
    {
        if (IsTidyCD(pType))
        {
            TipsManager.Instance.ShowTips("整理功能冷却中,请稍后再试！");
            return;
        }
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.TidyRequest(pType);
    }

    /// <summary>
    /// 整理
    /// </summary>
    /// <param name="pType"></param>
    public void OnTidy(GameCmd.PACKAGETYPE pType)
    {
        if (pType != PACKAGETYPE.PACKAGETYPE_MAIN
            && pType != PACKAGETYPE.PACKAGETYPE_STORE1
            && pType != PACKAGETYPE.PACKAGETYPE_STORE2
            && pType != PACKAGETYPE.PACKAGETYPE_STORE3)
        {
            return;
        }
        TipsManager.Instance.ShowTips("整理完成");
        DoTidyCDRecord(pType);
    }

    /// <summary>
    /// 记录上一次整理时间
    /// </summary>
    /// <param name="pType"></param>
    public void DoTidyCDRecord(GameCmd.PACKAGETYPE pType)
    {
        if (!m_dicTidyCD.ContainsKey(pType))
        {
            m_dicTidyCD.Add(pType, DateTimeHelper.Instance.Now);
        }
        else
        {
            m_dicTidyCD[pType] = DateTimeHelper.Instance.Now;
        }
    }

    /// <summary>
    /// 整理背包回调
    /// </summary>
    public void OnBatchRefreshItemPosition(List<GameCmd.stRefreshItemPosListPropertyUserCmd_S.Item> datalist)
    {
        if (null != datalist && datalist.Count > 0)
        {
            foreach (GameCmd.stRefreshItemPosListPropertyUserCmd_S.Item item in datalist)
            {
                DataManager.Manager<ItemManager>().OnUpdateItemPos(item.itemid, item.loc);
            }
        }
    }

    #region Unlock 背包解锁
    /// <summary>
    /// 根据背包类型获取背包容量（背包数量）
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    public int GetMaxGridHaveByPackageType(GameCmd.PACKAGETYPE pType)
    {
        table.UnlockStoreDataBase firstData = GetUnlockStoreDataBase((uint)pType, 0);
        return (int)((null != firstData) ? firstData.maxNum : 0);
    }

    /// <summary>
    /// 获取pType背包解锁的格子数量
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    public int GetUnlockGridByPackageType(GameCmd.PACKAGETYPE pType)
    {
        KnapsackDefine.LocalUnlockInfo info = GetUnlockInfoByPackageType(pType);
        return (null != info) ? (int)info.UnlockNum : 0;
    }


    /// <summary>
    /// 获取pType背包金钱解锁的格子数量
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    public int GetMoneyUnlockGridByPackType(GameCmd.PACKAGETYPE pType)
    {
        KnapsackDefine.LocalUnlockInfo info = GetUnlockInfoByPackageType(pType);
        return (null != info) ? info.MoneyUnlockNum : 0;
    }

    /// <summary>
    /// 获取pType背包等级解锁的格子数量
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    public int GetLevelUnlockGridByPackType(GameCmd.PACKAGETYPE pType)
    {
        KnapsackDefine.LocalUnlockInfo info = GetUnlockInfoByPackageType(pType);
        return (null != info) ? info.LvUnlockNum : 0;
    }

    /// <summary>
    /// 获取背包初始解锁数量
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    public int GetInitGridByPackageType(GameCmd.PACKAGETYPE pType)
    {
        table.UnlockStoreDataBase firstData = GetUnlockStoreDataBase((uint)pType, 0);
        return (int)((null != firstData) ? firstData.initNum : 0);
    }

    /// <summary>
    /// 获取背包解锁表格数据
    /// </summary>
    /// <param name="packId"></param>
    /// <param name="unlockIndex"></param>
    /// <returns></returns>
    public table.UnlockStoreDataBase GetUnlockStoreDataBase(uint packId, int unlockIndex)
    {
        return GameTableManager.Instance.GetTableItem<table.UnlockStoreDataBase>(packId, unlockIndex);
    }

    /// <summary>
    /// 解锁背包格子
    /// </summary>
    /// <param name="pType">解锁背包类型</param>
    /// <param name="unlockNum">解锁数量</param>
    public void UnlockKnapsackGrid(PACKAGETYPE pType, int unlockNum)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.UnlockKnapsackGridReq(pType, (uint)unlockNum);
    }

    /// <summary>
    /// 获取解锁花费描述
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="needUnlockNum"></param>
    /// <returns></returns>
    public string GetUnlockGridCostString(GameCmd.PACKAGETYPE pType, int needUnlockNum)
    {
        Dictionary<uint, uint> costDic = GetUnlockGridCost(pType, needUnlockNum);
        if (null == costDic || costDic.Count == 0)
        {
            return string.Empty;
        }
        StringBuilder builder = new StringBuilder();
        List<uint> moneyType = new List<uint>();
        moneyType.AddRange(costDic.Keys);
        moneyType.Sort((left, right) =>
            {
                return (int)right - (int)left;
            });
        for (int i = 0; i < moneyType.Count; i++)
        {
            builder.Append(string.Format("{0}{1}"
                , costDic[moneyType[i]]
                , DataManager.Manager<TextManager>().GetCurrencyNameByType((MoneyType)moneyType[i])));
        }
        return builder.ToString();
    }

    /// <summary>
    /// 获取解锁背包格消耗钱币
    /// </summary>
    /// <param name="pType">背包类型</param>
    /// <param name="needUnlockNum">当前需要解锁的数量</param>
    /// <returns></returns>
    public Dictionary<uint, uint> GetUnlockGridCost(GameCmd.PACKAGETYPE pType, int needUnlockNum)
    {
        Dictionary<uint, uint> costDic = new Dictionary<uint, uint>();
        if (needUnlockNum <= 0)
            return costDic;
        KnapsackDefine.LocalUnlockInfo unlockInfo = GetUnlockInfoByPackageType(pType);
        int initNum = GetInitGridByPackageType(pType);
        if (null != unlockInfo)
        {
            if (unlockInfo.UnlockNum - initNum >= 0)
            {
                int startIndex = unlockInfo.UnlockNum - initNum + 1;
                uint cost = 0;
                table.UnlockStoreDataBase storeTableData = null;
                for (int i = startIndex; i < startIndex + needUnlockNum; i++)
                {
                    storeTableData = GetUnlockStoreDataBase((uint)pType, i);
                    if (null != storeTableData)
                    {
                        if (costDic.ContainsKey(storeTableData.moneyType))
                        {
                            costDic[storeTableData.moneyType] += storeTableData.moneyNum;
                        }
                        else
                        {
                            costDic.Add(storeTableData.moneyType, storeTableData.moneyNum);
                        }
                    }
                }
            }
        }
        return costDic;
    }

    /// <summary>
    /// 解锁背包格子回调
    /// </summary>
    /// <param name="num">解锁数量</param>
    /// <param name="pType">背包类型</param>
    public void OnUnlockKnapsackGrid(uint num, uint pType)
    {
        KnapsackDefine.LocalUnlockInfo info = null;
        if (!m_localUnlockInfo.TryGetValue(pType, out info))
        {
            Engine.Utility.Log.Error("Unlock Info Error");
            return;
        }
        int oldCount = info.UnlockNum;
        info.UnlockNum = (int)(oldCount + num);

        //刷新UI
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_KNAPSACKUNLOCKINFOCHANGED, new UnlockPassData()
        {
            Type = pType,
        });
    }

    /// <summary>
    /// 同步背包解锁格子
    /// </summary>
    /// <param name="unlockInfos"></param>
    public void OnSycnUnlockKnapsackGrid(List<UnlockInfo> unlockInfos)
    {
        if (null == unlockInfos || unlockInfos.Count == 0)
        {
            Engine.Utility.Log.Error(CLASS_NAME + "->OnSycnUnlockKnapsackGrid failed,unlockinfo null");
            return;
        }
        m_localUnlockInfo.Clear();
        //获取上一次解锁格子数量（用于刷新UI）
        KnapsackDefine.LocalUnlockInfo localUnlockInfo = null;
        foreach (UnlockInfo info in unlockInfos)
        {
            if (m_localUnlockInfo.ContainsKey(info.type))
            {
                Engine.Utility.Log.Error(CLASS_NAME + "->OnSycnUnlockKnapsackGrid Error,already exist unlockinfo with packageType = {0}", ((PACKAGETYPE)info.type).ToString());
                continue;
            }
            localUnlockInfo = KnapsackDefine.LocalUnlockInfo.Create((GameCmd.PACKAGETYPE)info.type, info);
            if (null == localUnlockInfo)
            {
                continue;
            }
            m_localUnlockInfo.Add(info.type, localUnlockInfo);
            //刷新UI
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_KNAPSACKUNLOCKINFOCHANGED, new UnlockPassData()
            {
                Type = info.type,
                IsReconnect = m_IsNetWorkClose,
            });
        }
        m_IsNetWorkClose = false;
    }

    /// <summary>
    /// 服务器下发解锁仓库数量
    /// </summary>
    /// <param name="unlockNum"></param>
    public void OnUnlockWareHouse(uint unlockNum)
    {
        if (unlockNum >= 1)
        {
            //解锁仓库1
            UnlockWareHouse(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE1);
        }
        if (unlockNum >= 2)
        {
            //解锁仓库2
            UnlockWareHouse(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE2);
        }

        if (unlockNum >= 3)
        {
            //解锁仓库3
            UnlockWareHouse(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE3);
        }
    }

    /// <summary>
    /// 解锁长裤
    /// </summary>
    /// <param name="wareHouse"></param>
    private void UnlockWareHouse(GameCmd.PACKAGETYPE wareHouse)
    {
        KnapsackDefine.LocalUnlockInfo info = null;
        if (null != m_localUnlockInfo
            && m_localUnlockInfo.TryGetValue((uint)wareHouse, out info))
        {
            info.IsUnlock = true;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_UNLOCKWAREHOSUE, wareHouse);
        }
    }

    /// <summary>
    /// 根据背包类型获取格子解锁信息
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    public KnapsackDefine.LocalUnlockInfo GetUnlockInfoByPackageType(PACKAGETYPE pType)
    {
        return (m_localUnlockInfo.ContainsKey((uint)pType)) ? m_localUnlockInfo[(uint)pType] : null;
    }

    /// <summary>
    /// 当前背包位置是否已经解锁
    /// </summary>
    /// <param name="location">服务端中背包位置</param>
    /// <returns></returns>
    public bool IsGridUnlock(uint location)
    {
        ItemDefine.ItemLocation itemLocation = ItemDefine.TransformServerLocation2Local(location);
        if (null == itemLocation ||
             !(itemLocation.PackType == PACKAGETYPE.PACKAGETYPE_MAIN
             || IsWareHosue(itemLocation.PackType)))
        {
            //不是背包，仓库忽略
            return false;
        }

        KnapsackDefine.LocalUnlockInfo localUnlockInfo = GetUnlockInfoByPackageType(itemLocation.PackType);
        if (null == localUnlockInfo)
        {
            Engine.Utility.Log.Error("GetUnlockInfoByPackageType null ,packtype = {0}",itemLocation.PackType);
            return false;
        }
        int packUnlockGrid = localUnlockInfo.UnlockNum;
        uint maxLocation = ItemDefine.TransformLocal2ServerLocation(itemLocation.PackType, new UnityEngine.Vector2(0f, Math.Max(packUnlockGrid - 1, 0)));
        return (maxLocation >= location && packUnlockGrid != 0) ? true : false;
    }
    #endregion

    #region WareHouse 仓库
    /// <summary>
    /// pType是否为仓库
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    public bool IsWareHosue(GameCmd.PACKAGETYPE pType)
    {
        return (pType == PACKAGETYPE.PACKAGETYPE_STORE1
            || pType == PACKAGETYPE.PACKAGETYPE_STORE2
            || pType == PACKAGETYPE.PACKAGETYPE_STORE3);
    }

    /// <summary>
    /// 把金币存入仓库
    /// </summary>
    /// <param name="storeNum">存入数量</param>
    /// <param name="callback">操作回调</param>
    public void StoreCopperInWareHouse(uint storeNum, Action<bool> callback)
    {
        OperStoreMoney(storeNum, 1, callback);
    }

    /// <summary>
    /// 从仓库中取出金币
    /// </summary>
    /// <param name="takeOutNum">取出数量</param>
    /// <param name="callback">操作回调</param>
    public void TakeOutCopperFromWareHouse(uint takeOutNum, Action<bool> callback)
    {
        OperStoreMoney(takeOutNum, 2, callback);
    }

    /// <summary>
    /// 金币操作
    /// </summary>
    /// <param name="num"></param>
    /// <param name="type"></param>
    /// <param name="callback">操作回调</param>
    private void OperStoreMoney(uint num, uint type, Action<bool> callback)
    {
        if (null != DataManager.Instance.Sender)
        {
            string msg = "";
            bool warning = false;
            if (type == 1 || type == 2)
            {
                if (num <= 0)
                {
                    msg = "存取操作金币数量不能为0，请重新输入";
                    warning = true;
                }
                else if ((type == 1 && num > UserData.Coupon) || (type == 2 && num > wareHouseStoreCopperNum))
                {
                    msg = "存取操作金币数量超过当前拥有数，请重新输入";
                    warning = true;
                }
            }
            if (null != callback)
                callback.Invoke(!warning);

            if (!warning)
                DataManager.Instance.Sender.OperStoreMoneyReq(num, type);
            else
                TipsManager.Instance.ShowTips(msg);


        }
    }

    /// <summary>
    /// 存取钱币响应
    /// </summary>
    /// <param name="storeNum">数量</param>
    /// <param name="type">0：服务端首次同步 1：存 2：取</param>
    public void OnOperStoreMoney(uint num, uint type)
    {
        string msg = "";
        bool needRereshInputMax = false;
        switch (type)
        {
            case 0:
                wareHouseStoreCopperNum = num;
                break;
            case 1:
                msg = string.Format("仓库金币+{0}", num);
                wareHouseStoreCopperNum += num;
                needRereshInputMax = true;
                break;
            case 2:
                msg = string.Format("仓库金币-{0}", num);
                wareHouseStoreCopperNum = Math.Max(0, (wareHouseStoreCopperNum -= num));
                needRereshInputMax = true;
                break;
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_WAREHOUSESTORECOPPERCHANGED, wareHouseStoreCopperNum);
        //if (null != UI)
        //    UI.UpdateWareHouseStoreCopperNum(wareHouseStoreCopperNum);
        if (!string.IsNullOrEmpty(msg))
            TipsManager.Instance.ShowTips(msg);
        if (needRereshInputMax)
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_REFRESHINPUTMAXNUM);
    }

    /// <summary>
    /// 从仓库提取物品到背包中
    /// </summary>
    /// <param name="takeOutList"></param>
    public void TakeOutItemsFromMainPackage(List<uint> takeOutList)
    {
        if (null == takeOutList || takeOutList.Count == 0)
            return;
        uint emptyLocation = 0;
        for (int i = 0; i < takeOutList.Count; i++)
        {
            if (takeOutList[i] == 0)
                continue;

        }
    }

    /// <summary>
    /// 移动列表到目标背包中
    /// </summary>
    /// <param name="moveList">需要移动的物品列表</param>
    /// <param name="targetPackage">目标背包</param>
    public void MoveItems(List<uint> moveList, GameCmd.PACKAGETYPE targetPackage)
    {
        if (null == moveList || moveList.Count == 0)
            return;
        for (int i = 0; i < moveList.Count; i++)
        {
            if (moveList[i] == 0)
                continue;
            MoveItems(moveList[i], targetPackage);
        }
    }

    /// <summary>
    /// 移动物品到目标背包2
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="targetPackage"></param>
    public void MoveItems(uint qwThisId, GameCmd.PACKAGETYPE targetPackage)
    {
        uint targetLocation = 0;
        BaseItem data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(qwThisId);
        if (null == data)
        {
            Engine.Utility.Log.Error(CLASS_NAME + "-> Fail move item={0} to {1} data null", qwThisId, targetPackage.ToString());
            return;
        }
        uint targetQwThisID = 0;
        if (TryGetPileGridInPackage(qwThisId, out targetQwThisID, targetPackage, out targetLocation)
            || TryGetEmptyGridInPackage(targetPackage, out targetLocation))
        {
            DataManager.Instance.Sender.SwapItemReq(data.QWThisID, targetQwThisID, new tItemLocation()
            {
                loc = data.ServerLocaltion,
            }, new tItemLocation()
            {
                loc = targetLocation,
            });
        }
        else
        {
            if (targetPackage == PACKAGETYPE.PACKAGETYPE_STORE1 ||
                targetPackage == PACKAGETYPE.PACKAGETYPE_STORE2 ||
                targetPackage == PACKAGETYPE.PACKAGETYPE_STORE3)
            {
                TipsManager.Instance.ShowTips("仓库已满");
            }
            else if (targetPackage == PACKAGETYPE.PACKAGETYPE_MAIN)
            {
                TipsManager.Instance.ShowTips("背包空间不足，无法取出");
            }
        }
    }

    #endregion

    /// <summary>
    /// 是否背包已满
    /// </summary>
    /// <returns></returns>
    public bool IsKanpsackFull()
    {
        return (GetKnapsackEmptyGrid() == 0) ? true : false;
    }

    /// <summary>
    /// 简单判断背包是否已满
    /// </summary>
    /// <returns></returns>
    public bool IsSimpleKanpsackFull()
    {
        //KnapsackDefine.LocalUnlockInfo info = GetUnlockInfoByPackageType(PACKAGETYPE.PACKAGETYPE_MAIN);
        //if (null == info)
        //    return true;
        //int itemCount = DataManager.Manager<ItemManager>().ItemDataDic.Count;
        //return (info.UnlockNum - itemCount) > 0 ? false : true;

        int max = GetUnlockGridByPackageType(PACKAGETYPE.PACKAGETYPE_MAIN);
        List<uint> itemsInPackage = DataManager.Manager<ItemManager>().GetItemDataByPackageType(PACKAGETYPE.PACKAGETYPE_MAIN);

        return max - itemsInPackage.Count > 0 ? false : true;
    }
    /// <summary>
    /// 获取背包剩余空间
    /// </summary>
    /// <param name="pType">背包类型 （默认为主背包）</param>
    /// <returns></returns>
    public int GetKnapsackEmptyGrid(GameCmd.PACKAGETYPE pType = PACKAGETYPE.PACKAGETYPE_MAIN)
    {
        List<uint> emptyList = GetEmptyGridInPackage(pType);
        return (null != emptyList) ? emptyList.Count : 0;
    }

    /// <summary>
    /// 获取背包中空列表
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    public List<uint> GetEmptyGridInPackage(GameCmd.PACKAGETYPE pType)
    {
        List<uint> emptyGridPosList = new List<uint>();
        int max = GetUnlockGridByPackageType(pType);
        List<uint> itemsInPackage = DataManager.Manager<ItemManager>().GetItemDataByPackageType(pType);
        BaseItem data = null;
        uint tempLocation = 0;
        for (int i = 0; i < max; i++)
        {
            data = null;
            tempLocation = ItemDefine.TransformLocal2ServerLocation(pType, new UnityEngine.Vector2(0f, i));

            if (null != itemsInPackage && itemsInPackage.Count > 0)
            {
                uint matchId = 0;
                for (int j = 0; j < itemsInPackage.Count; j++)
                {
                    matchId = 0;
                    data = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(itemsInPackage[j]);
                    if (null != data && data.ServerLocaltion == tempLocation)
                    {
                        matchId = data.QWThisID;
                        break;
                    }

                }
                if (matchId != 0)
                {
                    itemsInPackage.Remove(matchId);
                    continue;
                }
            }
            emptyGridPosList.Add(tempLocation);
        }
        return emptyGridPosList;
    }

    /// <summary>
    /// 尝试获取各背包中的空位置
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="emptyLocation"></param>
    /// <returns></returns>
    public bool TryGetEmptyGridInPackage(GameCmd.PACKAGETYPE pType, out uint emptyLocation)
    {
        emptyLocation = 0;
        if (!(pType == PACKAGETYPE.PACKAGETYPE_MAIN || IsWareHosue(pType)))
            return false;
        List<uint> emptyList = GetEmptyGridInPackage(pType);
        if (null != emptyList && emptyList.Count != 0)
        {
            emptyLocation = emptyList[0];
            return true;
        }
        return false;
    }

    /// <summary>
    /// 尝试获取各背包中的空位置
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="pileLocation"></param>
    /// <returns></returns>
    public bool TryGetPileGridInPackage(uint qwThisId, out uint targetQwThisID, GameCmd.PACKAGETYPE pType, out uint pileLocation)
    {
        pileLocation = 0;
        targetQwThisID = 0;
        bool success = false;
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<BaseItem>(qwThisId);
        if (null == baseItem || !baseItem.CanPile
            || !(pType == PACKAGETYPE.PACKAGETYPE_MAIN || IsWareHosue(pType)))
            return success;
        List<BaseItem> items = DataManager.Manager<ItemManager>().GetItemByBaseId(baseItem.BaseId, pType);
        uint pileNum = baseItem.MaxPileNum;
        if (null != items)
        {
            for (int i = 0, max = items.Count; i < max; i++)
            {
                if (baseItem.Num + items[i].Num <= pileNum)
                {
                    pileLocation = items[i].ServerLocaltion;
                    targetQwThisID = items[i].QWThisID;
                    success = true;
                    break;
                }

            }
        }

        return success;
    }

    /// <summary>
    /// 是否可以放入背包
    /// </summary>
    /// <returns></returns>
    public bool CanPutInKanpsack(uint itemBaseId, uint itemNum)
    {
        if (IsSimpleKanpsackFull() == false)
        {
            return true;
        }
        else
        {
            int itemSum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemBaseId);    //item总数
            List<BaseItem> itemList = DataManager.Manager<ItemManager>().GetItemByBaseId(itemBaseId);//item组总数
            if (itemList != null && itemList.Count > 0)
            {
                BaseItem baseItem = itemList[0];

                //可以堆叠
                if (baseItem.CanPile && baseItem.MaxPileNum > 0)
                {
                    if ((itemSum + itemNum) <= baseItem.MaxPileNum * itemList.Count)//未超过堆叠数(刚好相等)
                    {
                        return true;
                    }

                    //if ((itemSum + itemNum) / baseItem.MaxPileNum < itemList.Count)//未超过堆叠数
                    //{
                    //    return true;
                    //}

                    //if ((itemSum + itemNum) == baseItem.MaxPileNum * itemList.Count)//未超过堆叠数(刚好相等)
                    //{
                    //    return true;
                    //}
                }

                //不可堆叠
                return false;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    #endregion
}