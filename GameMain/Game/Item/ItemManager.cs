using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using GameCmd;
using table;
using Engine.Utility;
using UnityEngine;
using Client;

public class ItemManager : BaseModuleData, IManager, ITimer,IGlobalEvent
{
    #region Const
    public const string CLASS_NAME = "ItemManager";

    private const uint ITEMCD_TIMERID = 1000;
    #endregion

    #region property
    //物品管理器是否准备好
    private bool ready = false;
    public bool Ready
    {
        get
        {
            return ready;
        }
    }

    //物品字典
    private Dictionary<uint, BaseItem> itemDataDic;
    public Dictionary<uint, BaseItem> ItemDataDic
    {
        get
        {
            return itemDataDic;
        }
    }

    //零时对象
    private ItemDefine.UpdateItemPassData m_passData = null;
    private List<BaseItem> m_lstTempItemList = null;
    
    #endregion

    /// <summary>
    /// 获取本地表格数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="firstKey"></param>
    /// <param name="secondKey"></param>
    /// <returns></returns>
    public T GetLocalDataBase<T>(uint firstKey, int secondKey = 0) where T : class, global::ProtoBuf.IExtensible
    {
        return GameTableManager.Instance.GetTableItem<T>(firstKey, secondKey);
    }
    /// <summary>
    /// 使用物品
    /// </summary>
    /// <param name="qwThisID">物品id</param>
    /// <param name="qwTargetID">目标ID;具体意义示使用道具而定</param>
    /// <param name="count">数量</param>
    public void Use(uint qwThisID, uint num = 1/*, uint tagetId, uint targetType = 0*/)
    {
        BaseItem baseItem = GetBaseItemByQwThisId(qwThisID);
        if (null == baseItem)
        {
            return;
        }

        //是否正在CD中
        if (IsRuningItemCD(baseItem.BaseId))
        {
            TipsManager.Instance.ShowTips("物品正在使用冷却中");
            return;
        }

        if (baseItem.BaseType == ItemBaseType.ItemBaseType_Consumption
            && !CanUseConsumerItem(baseItem.BaseId,(int)num))
        {
            return;
        }
        UseNormal(baseItem, num);
    }

    /// <summary>
    /// 使用一般物品
    /// </summary>
    /// <param name="baseItem"></param>
    /// <param name="num"></param>
    private void UseNormal(BaseItem baseItem, uint num = 1)
    {
        //if (baseItem.BaseType == GameCmd.ItemBaseType.ItemBaseType_Consumption) //注释此行，凭证类也可以使用此方法
        {
            if (baseItem.BaseData.subType == (uint)ItemDefine.ItemConsumerSubType.Token)//悬赏任务令牌
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RewardPanel);
            }
            else if (baseItem.BaseData.subType == (uint)ItemDefine.ItemConsumerSubType.Horn)//)//喇叭
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.HornPanel);
            }
            else if (baseItem.BaseData.subType == (uint)ItemDefine.ItemConsumerSubType.Pet)
            {
                if (DataManager.Manager<PetDataManager>().CurFightingPet != 0)
                {
                    uint petID = DataManager.Manager<PetDataManager>().GetNpcIDByPetID(DataManager.Manager<PetDataManager>().CurFightingPet);
                    DataManager.Instance.Sender.UseItem(petID, (uint)GameCmd.SceneEntryType.SceneEntry_NPC, baseItem.QWThisID, num);
                }
            }
            else if (baseItem.BaseData.subType == (uint)ItemDefine.ItemConsumerSubType.TaskUseItem)
            {
                OnTaskUse(baseItem,num);
            }
            else if (baseItem.BaseData.subType == (uint)ItemDefine.ItemConsumerSubType.TreasureMap)
            {
                //藏宝图预留
                DataManager.Manager<TreasureManager>().UseTreasureMap(baseItem);
            }
            else
            {
                if (baseItem.BaseData.func_id != 0)
                {
                    table.EffectFuncDataBase effectFunc = GameTableManager.Instance.GetTableItem<table.EffectFuncDataBase>(baseItem.BaseData.func_id);
                    if (effectFunc.type == 5 && effectFunc.jumpId != 0)
                    {
                        OnOpenPanelUse(effectFunc.jumpId, baseItem.BaseId);
                        return;
                    }
                }
                DataManager.Instance.Sender.UseItem(Client.ClientGlobal.Instance().MainPlayer.GetID(), (uint)GameCmd.SceneEntryType.SceneEntry_Player, baseItem.QWThisID, num);
            }
        }
    }

    private void OnTaskUse(BaseItem baseItem,uint num)
    {
        List<QuestTraceInfo> questList = new List<QuestTraceInfo>();
        QuestTranceManager.Instance.GetQuestTraceInfoList(true, ref questList);
        bool bHaveItemTask = false;
        for (int i = 0; i < questList.Count; i++)
        {
            //if (questList[i].QuestTable.dwSubmitItem == baseItem.BaseId)
            if (questList[i].QuestTable.usecommitItemID == baseItem.BaseId)
            {
                bHaveItemTask = true;
                if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.KnapsackPanel))
                {
                    DataManager.Manager<UIPanelManager>().HidePanel(PanelID.KnapsackPanel);
                }
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.TASK_DONING,
                    new Client.stDoingTask() { taskid = questList[i].taskId });
                break;
            }
        }

        //如果没有这个任务，就发送useItem消息(环任务中任务可能删除，但任务道具没有删除，发这个消息用于删除无用任务道具)
        if (bHaveItemTask == false)
        {
            DataManager.Instance.Sender.UseItem(Client.ClientGlobal.Instance().MainPlayer.GetID(), (uint)GameCmd.SceneEntryType.SceneEntry_Player, baseItem.QWThisID, num);
        }
        
    }
    /// <summary>
    /// 打开界面使用 TODO 背包页签
    /// </summary>
    /// <param name="baseItem"></param>
    private void OnOpenPanelUse(uint jumpId, uint baseId)
    {
        table.ItemGetDataBase getDb = GameTableManager.Instance.GetTableItem<table.ItemGetDataBase>(jumpId);
        table.JumpWayDataBase jumpDb = GameTableManager.Instance.GetTableItem<table.JumpWayDataBase>(getDb.jumpID);
        if (getDb != null)
        {
            string param1 = jumpDb.param1;//panel
            string param2 = jumpDb.param2;//tabs
            //string param3 = getDb.param3;//mallItemid

            PanelID panelID = (PanelID)Enum.Parse(typeof(PanelID), param1);
            string[] strTabs = param2.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            ReturnBackUIMsg uimsg = new ReturnBackUIMsg();
            uimsg.tabs = new int[strTabs.Length];
            for (int i = 0; i < strTabs.Length; i++)
            {
                uimsg.tabs[i] = int.Parse(strTabs[i]);
            }

            uimsg.param = baseId;

            PanelID panelId = UIFrameManager.Instance.CurrShowPanelID;
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(panelId))
            {
                UIPanelBase panelBase = DataManager.Manager<UIPanelManager>().GetPanel(panelId);
                //Client.UIPanelInfo uidata = panelBase.PanelShowInfo;
                UIPanelManager.LocalPanelInfo uidata = panelBase.PanelInfo;
                if (uidata != null && uidata.NeedBg)
                {
                    int[] tabs = new int[panelBase.dicActiveTabGrid.Count];
                    for (int i = 0; i < tabs.Length; i++)
                    {
                        tabs[i] = panelBase.dicActiveTabGrid.ElementAt(i).Value;
                    }
                    ReturnBackUIData[] returnUI = new ReturnBackUIData[] { new ReturnBackUIData() };
                    returnUI[0].msgid = UIMsgID.eShowUI;
                    returnUI[0].panelid = panelId;
                    returnUI[0].param = new ReturnBackUIMsg()
                    {
                        tabs = tabs,
                        //param = selectItem,
                    };
                    DataManager.Manager<UIPanelManager>().ShowPanel(panelID);
                    //DataManager.Manager<UIPanelManager>().ShowPanel(panelID, null, null, returnBackUIData: returnUI);
                    //DataManager.Manager<UIPanelManager>().SendMsg(panelID, UIMsgID.eShowUI, uimsg);
                }
                else
                {
                    DataManager.Manager<UIPanelManager>().ShowPanel(panelID);
                    //DataManager.Manager<UIPanelManager>().SendMsg(panelID, UIMsgID.eShowUI, uimsg);
                }
            }
            else
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(panelID);
                //DataManager.Manager<UIPanelManager>().SendMsg(panelID, UIMsgID.eShowUI, uimsg);
            }

            if (uimsg.tabs.Length > 1)
            {
                UIFrameManager.Instance.OnCilckTogglePanel(panelID, 1, uimsg.tabs[0]);
            }
        }
    }

    /// <summary>
    /// 拾取物品
    /// </summary>
    /// <param name="qwThisId"></param>
    public void PickUpItemInMap(uint qwThisId)
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.PickUpItemReq(qwThisId);
    }

    /// <summary>
    /// 获取物品
    /// </summary>
    public void GetItemList()
    {
        if (null != DataManager.Instance.Sender)
            DataManager.Instance.Sender.GetItemListReq();
    }

    /// <summary>
    /// 新增物品列表
    /// </summary>
    /// <param name="itemList">物品列表</param>
    /// <param name="needClear">是否需要清空</param>
    /// <param name="action">1:加载 2：刷新</param>
    public void OnAddListItemData(List<GameCmd.ItemSerialize> itemList)
    {
        //1、新增
        Dictionary<uint, GameCmd.ItemSerialize> addItems = new Dictionary<uint, ItemSerialize>();
        List<uint> removeIds = new List<uint>();
        List<uint> updateItems = new List<uint>();
        if (null != itemList && itemList.Count != 0)
        {
            for (int i = 0, max = itemList.Count; i < max; i++)
            {
                addItems.Add(itemList[i].qwThisID, itemList[i]);
            }
            updateItems.AddRange(addItems.Keys);
        }

        //2、比对
        foreach(KeyValuePair<uint,BaseItem> pair in itemDataDic)
        {
            if (!addItems.ContainsKey(pair.Key))
            {
                removeIds.Add(pair.Key);
            }
        }

        //3、更新
        uint addAction = (uint)GameCmd.AddItemAction.AddItemAction_Load;
        for(int i = 0,max = updateItems.Count;i < max;i++)
        {
            OnAddItemData(addItems[updateItems[i]], addAction);
        }
        //删除
        for(int i = 0,max = removeIds.Count;i < max;i++)
        {
            OnUpdateItemDataNum(removeIds[i],0);
        }
    }


    /// <summary>
    /// 获取物品信息
    /// </summary>
    /// <param name="qwThisID">物品唯一</param>
    /// <returns></returns>
    public T GetBaseItemByQwThisId<T>(uint qwThisID) where T : BaseItem
    {
        if (!ready)
            return null;
        BaseItem baseItem = null;
        if (itemDataDic.TryGetValue(qwThisID, out baseItem))
        {
            if (baseItem is T)
            {
                return (T)baseItem;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据唯一id获取物品id
    /// </summary>
    /// <param name="qwThisID"></param>
    /// <returns></returns>
    public BaseItem GetBaseItemByQwThisId(uint qwThisID)
    {
        return GetBaseItemByQwThisId<BaseItem>(qwThisID);
    }

    /// <summary>
    /// 尝试获取物品
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="baseItem"></param>
    /// <returns></returns>
    public bool TryGetBaseItemByQwThisId(uint qwThisId, out BaseItem baseItem)
    {
        return itemDataDic.TryGetValue(qwThisId, out baseItem);
    }

    /// <summary>
    /// 是否为一个物品
    /// </summary>
    /// <param name="dwobjectId">物品表格里面id</param>
    /// <returns></returns>
    public bool IsItem(uint dwobjectId)
    {
        return (null != GetLocalDataBase<table.ItemDataBase>(dwobjectId)) ? true : false;
    }

    /// <summary>
    /// 获取该类型物品列表
    /// </summary>
    /// <param name="iType">物品大类型</param>
    /// <param name="subType">筛选小类</param>
    /// <param name="pType">背包</param>
    /// <returns></returns>
    public List<uint> GetItemByType(ItemBaseType iType, List<uint> subType, GameCmd.PACKAGETYPE pType)
    {
        if (!ready)
            return null;

        List<uint> result = DataManager.Instance.TempUintList;
        bool needMatchSubType = (null != subType && subType.Count != 0) ? true : false;
        var enumerator = itemDataDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (null == enumerator.Current.Value)
                continue;
            if ((enumerator.Current.Value.BaseType == iType && enumerator.Current.Value.PackType == pType)
                && (!needMatchSubType || (needMatchSubType && subType.Contains(enumerator.Current.Value.BaseData.subType))))

            {
                result.Add(enumerator.Current.Key);
            }
        }

        return result;
    }

    /// <summary>
    /// 根据打类，小类，获取对应物品列表
    /// </summary>
    /// <param name="iType"></param>
    /// <param name="subType"></param>
    /// <param name="pType"></param>
    /// <returns></returns>
    public List<BaseItem> GetBaseItemByType(ItemBaseType iType, List<uint> subType, GameCmd.PACKAGETYPE pType)
    {
        if (!ready)
            return null;
        m_lstTempItemList.Clear();
        bool needMatchSubType = (null != subType && subType.Count != 0) ? true : false;
        var enumerator = itemDataDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (null == enumerator.Current.Value)
                continue;
            if ((enumerator.Current.Value.BaseType == iType && enumerator.Current.Value.PackType == pType)
                && (!needMatchSubType || (needMatchSubType && subType.Contains(enumerator.Current.Value.BaseData.subType))))
            {
                m_lstTempItemList.Add(enumerator.Current.Value);
            }
        }
        return m_lstTempItemList;
    }

    /// <summary>
    /// 获取该类型物品列表
    /// </summary>
    /// <param name="iType"></param>
    /// <param name="pType"></param>
    /// <returns></returns>
    public List<uint> GetItemByType(ItemBaseType iType, GameCmd.PACKAGETYPE pType = PACKAGETYPE.PACKAGETYPE_MAIN)
    {
        return GetItemByType(iType, null, pType);
    }

    /// <summary>
    /// 根据表格获取相应物品数量
    /// </summary>
    /// <param name="baseid">表格id</param>
    /// <param name="pType">背包类型</param>
    /// <returns></returns>
    public int GetItemNumByBaseId(uint baseid, PACKAGETYPE pType = PACKAGETYPE.PACKAGETYPE_MAIN)
    {
        List<BaseItem> itemDatas = GetItemByBaseId(baseid, pType);
        uint num = 0;
        if (null != itemDatas)
        {
            for (int i = 0; i < itemDatas.Count; i++)
            {
                num += itemDatas[i].Num;
            }
        }

        //获取非绑定的物品id数量
        table.ItemDataBase db = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(baseid);
        if (db != null && db.EqualsId != 0)
        {
            itemDatas = GetItemByBaseId(db.EqualsId, pType);
            if (null != itemDatas)
            {
                for (int i = 0; i < itemDatas.Count; i++)
                {
                    num += itemDatas[i].Num;
                }
            }
        }
        return (int)num;
    }


    /// <summary>
    /// 根据表格id获取相应物品列表
    /// </summary>
    /// <param name="baseid">表格id</param>
    /// <param name="pType">背包类型 (注意获取所有背包内的需要pType = PACKAGETYPE.PACKAGETYPE_NONE)</param>
    /// <returns></returns>
    public List<BaseItem> GetItemByBaseId(uint baseid, PACKAGETYPE pType = PACKAGETYPE.PACKAGETYPE_MAIN)
    {
        var enumerator = itemDataDic.GetEnumerator();
        m_lstTempItemList.Clear();
        while (enumerator.MoveNext())
        {
            if (null == enumerator.Current.Value)
                continue;
            if (pType != PACKAGETYPE.PACKAGETYPE_NONE && pType != enumerator.Current.Value.PackType)
            {
                continue;
            }

            if (enumerator.Current.Value.BaseId != baseid)
            {
                continue;
            }
            m_lstTempItemList.Add(enumerator.Current.Value);

        }
        return m_lstTempItemList;
    }

    /// <summary>
    /// 根据表格id获取相应物品唯一id列表
    /// </summary>
    /// <param name="baseid"></param>
    /// <param name="pType"></param>
    /// <returns></returns>
    public List<uint> GetItemListByBaseId(uint baseid, PACKAGETYPE pType = PACKAGETYPE.PACKAGETYPE_MAIN)
    {
        var enumerator = itemDataDic.GetEnumerator();
        List<uint> result = DataManager.Instance.TempUintList;
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Value.BaseId == baseid
                && enumerator.Current.Value.PackType == pType)
            {
                result.Add(enumerator.Current.Key);
            }
        }
        return result;
    }

    public List<BaseItem> GetItemListByItemType()
    {
        var enumerator = itemDataDic.GetEnumerator();
        List<BaseItem> result = new List<BaseItem>();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Value.IsTreasureMap && ExistItem(enumerator.Current.Value.QWThisID) 
                && enumerator.Current.Value.PackType == PACKAGETYPE.PACKAGETYPE_MAIN
                && (!enumerator.Current.Value.HasDailyUseLimit || enumerator.Current.Value.HasDailyUseLimit
                && GetItemLeftUseNum(enumerator.Current.Value.BaseId) > 0))
           {
               result.Add(enumerator.Current.Value);
           }
        }
        return result;
    }

    /// <summary>
    /// 当前背包是否存在该物品
    /// </summary>
    /// <param name="qwthisID"></param>
    /// <returns></returns>
    public bool ExistItem(uint qwthisID)
    {
        return itemDataDic.ContainsKey(qwthisID);
    }

    /// <summary>
    /// 获取对应背包里面的物品类表
    /// </summary>
    /// <param name="packageType">背包类型</param>
    /// <returns></returns>
    public List<uint> GetItemDataByPackageType(PACKAGETYPE packageType)
    {
        if (!ready)
            return null;

        List<uint> result = DataManager.Instance.TempUintList;
        var enumerator = itemDataDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Value.PackType != packageType)
                continue;
            result.Add(enumerator.Current.Key);
        }
        return result;
    }

    /// <summary>
    /// 是否过滤
    /// </summary>
    /// <param name="typeMask">背包模式</param>
    /// <returns></returns>
    public static bool IsFilter(int typeMask)
    {
        return (typeMask & (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Material)) != 0
                && (typeMask & (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Equip)) != 0
                && (typeMask & (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Consumption)) != 0 ? false : true;
    }

    /// <summary>
    /// 物品排序
    /// </summary>
    /// <param name="thisIds"></param>
    public static void SortItemListBySortId(ref List<uint> thisIds)
    {
        thisIds.Sort(BaseItem.BaseItemIDCompare.Create());
    }

    /// <summary>
    /// 背包过滤
    /// </summary>
    /// <param name="pType">背包类型</param>
    /// <param name="typeMask">过滤Mask</param>
    public List<uint> DoFilterItemData(PACKAGETYPE pType, int typeMask)
    {
        List<uint> result = DataManager.Instance.TempUintList;
        var enumerator = itemDataDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if ((((1 << (int)enumerator.Current.Value.BaseType) & typeMask) != 0)
                && enumerator.Current.Value.PackType == pType)

            {
                result.Add(enumerator.Current.Key);
            }
        }
        if (result.Count > 0)
        {
            SortItemListBySortId(ref result);
        }
        return result;
    }

    /// <summary>
    /// 选取pType背包中baseTpe类型的物品
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public List<uint> DoFilterItemData(PACKAGETYPE pType, ItemBaseType baseType)
    {
        List<uint> result = DataManager.Instance.TempUintList;
        var enumerator = itemDataDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Value.BaseType == baseType
                && enumerator.Current.Value.PackType == pType)
            {
                result.Add(enumerator.Current.Key);
            }
        }
        if (result.Count > 0)
        {
            SortItemListBySortId(ref result);
        }
        return result;
    }

    public List<BaseItem> DoFilterItemDataByType(PACKAGETYPE pType, ItemBaseType baseType)
    {
        m_lstTempItemList.Clear();
        var enumerator = itemDataDic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Value.BaseType == baseType
                && enumerator.Current.Value.PackType == pType)
            {
                m_lstTempItemList.Add(enumerator.Current.Value);
            }
        }
        return m_lstTempItemList;
    }

    public bool TryGetWayIdListByBaseId(uint itemId, out List<uint> wayIdList)
    {
        wayIdList = new List<uint>();

        List<table.ItemGetDataBase> i_list = GameTableManager.Instance.GetTableList<table.ItemGetDataBase>();
        table.ItemDataBase item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemId);
        if (item == null)
        {
            Engine.Utility.Log.Error("道具表内无法找到此物品!");
            return false;
        }
        string m_sItemName = item.itemName;
        string wayIDs = item.wayIDs;

        if (wayIDs != null)
        {
            string[] ids = wayIDs.Split('_');
            for (int i = 0; i < ids.Length; i++)
            {
                uint id;
                if (uint.TryParse(ids[i], out id))
                {
                    if (i_list.Exists((data) => { return data.ID == id ? true : false; }))
                    {
                        wayIdList.Add(id);
                    }
                }
            }
        }

        if (wayIdList.Count == 0)
        {
            return false;
        }
        return true;
    }


    /// <summary>
    /// 更新数据(消息回调)
    /// </summary>
    /// <param name="qwThisID">id</param>
    /// <param name="num">数量</param>
    public void OnUpdateItemDataNum(uint qwThisID, uint num,uint action = 0)
    {
        if (!ready)
            return;
        bool clear = (num <= 0) ? true : false;
        BaseItem data = null;
        GameCmd.PACKAGETYPE pTyp = PACKAGETYPE.PACKAGETYPE_MAIN;
        m_passData.Reset();
        m_passData.QWThisId = qwThisID;
        m_passData.AddItemAction = (GameCmd.AddItemAction)action;
        //1、更新本地数据缓存
        if (clear && itemDataDic.ContainsKey(qwThisID))
        {
            m_passData.UpdateType = ItemDefine.UpdateItemType.Remove;
            m_passData.ChangeNum = -((int)itemDataDic[qwThisID].Num);
            data = itemDataDic[qwThisID];
            data.Num = 0;
            pTyp = data.PackType;
            itemDataDic.Remove(qwThisID);
            m_passData.BaseId = data.BaseId;
            m_passData.UpdateType = ItemDefine.UpdateItemType.Remove;

        }
        else if (!clear)
        {
            data = itemDataDic[qwThisID];
            uint tempNum = data.Num;
            m_passData.UpdateType = ItemDefine.UpdateItemType.Update;
            if (tempNum < num)
            {
                m_passData.ChangeNum = (int)(num - tempNum);
                if (m_passData.ChangeNum > 0)
                {
                    DispatchPushMessage(data);
                }
            }

            itemDataDic[qwThisID].Num = num;
            data = itemDataDic[qwThisID];
            pTyp = data.PackType;
            m_passData.BaseId = data.BaseId;
        }
        //2、更新UI
        DispatchRefreshItemEvent(m_passData);
    }

    /// <summary>
    /// 更新装备耐久
    /// </summary>
    /// <param name="qwThisId">id</param>
    /// <param name="curDurable">当前耐久</param>
    /// <param name="maxDurable">最大耐久</param>
    public void OnUpdateItemDurable(uint qwThisID, uint curDurable, uint maxDurable)
    {
        if (!ready)
            return;
        if (!itemDataDic.ContainsKey(qwThisID))
        {
            Engine.Utility.Log.Error(CLASS_NAME + "-> OnUpdateItemDurable Failed,qwThisId = {0} don't exist!", qwThisID);
            return;
        }
        itemDataDic[qwThisID].UpdateItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Dur, curDurable);
        itemDataDic[qwThisID].UpdateItemAttribute(GameCmd.eItemAttribute.Item_Attribute_MaxDur, maxDurable);
        m_passData.Reset();
        m_passData.UpdateType = ItemDefine.UpdateItemType.Update;
        m_passData.QWThisId = qwThisID;
        m_passData.BaseId = itemDataDic[qwThisID].BaseId;
        //刷新UI
        DispatchRefreshItemDurableEvent(m_passData);
    }



    /// <summary>
    /// 交换物品位置
    /// </summary>
    /// <param name="srcThisID"></param>
    /// <param name="desThisID"></param>
    /// <param name="srcLocation"></param>
    /// <param name="desLocation"></param>
    public void OnSwapItem(uint srcThisID, uint desThisID, GameCmd.tItemLocation srcLocation, GameCmd.tItemLocation desLocation)
    {
        BaseItem dataTarget = GetBaseItemByQwThisId(desThisID);
        if (null != dataTarget && dataTarget.CanPile)
        {
            //可堆叠物品合并特殊处理
            return;
        }
        BaseItem datasrc = GetBaseItemByQwThisId(srcThisID);
        if (null != datasrc)
            OnUpdateItemPos(srcThisID, desLocation);
        if (null != dataTarget)
            OnUpdateItemPos(desThisID, srcLocation);
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_EQUIPCOLORSUITCHANGE, null);
    }

    /// <summary>
    /// 刷新背包中的位置
    /// </summary>
    /// <param name="qwThisID"></param>
    /// <param name="loc"></param>
    public void OnUpdateItemPos(uint qwThisID, GameCmd.tItemLocation loc)
    {
        if (!ready)
            return;
        if (!itemDataDic.ContainsKey(qwThisID))
        {
            Engine.Utility.Log.Error(CLASS_NAME + "-> OnUpdateItemPos Failed,qwThisId = {0} don't exist!", qwThisID);
            return;
        }
        itemDataDic[qwThisID].UpdateServerLocation(loc.loc);
        m_passData.Reset();
        m_passData.UpdateType = ItemDefine.UpdateItemType.Update;
        m_passData.QWThisId = qwThisID;
        m_passData.BaseId = itemDataDic[qwThisID].BaseId;
        //刷新UI
        DispatchRefreshItemEvent(m_passData);

    }

    /// <summary>
    /// 更新本地物品数据
    /// </summary>
    /// <param name="data"></param>
    public void OnAddItemData(ItemSerialize data, uint action = 1, bool SoulLevelUp = false)
    {
        BaseItem tempData = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(data.dwObjectID);
        BaseItem itemData = null;
        m_passData.Reset();
        if (tempData.BaseType == ItemBaseType.ItemBaseType_Equip)
        {
            if (tempData.SubType == (uint)GameCmd.EquipType.EquipType_SoulOne)
            {
                itemData = new Muhon(data.dwObjectID, data);
                Muhon muhon = itemData as Muhon;
                if (tempData.IsBaseReady)
                {
                    if (SoulLevelUp && muhon.Level > 1)
                    {
                        stWildChannelCommonChatUserCmd_CS cmd = new stWildChannelCommonChatUserCmd_CS();
                        cmd.byChatType = CHATTYPE.CHAT_SYS;
                        cmd.szInfo = string.Format("{0}升级到{1}级", itemData.LocalName, muhon.Level);
                        cmd.dwOPDes = 0;
                        cmd.timestamp = (uint)DateTimeHelper.Instance.Now;
                        ChatChannel channel = DataManager.Manager<ChatDataManager>().GetChannelByType(CHATTYPE.CHAT_SYS);
                        if (channel != null)
                        {
                            channel.Add(channel.ToChatInfo(cmd));
                        }
                    }

                }
            }
            else if (tempData.BaseData.subType != (uint)GameCmd.EquipType.EquipType_Office)
                itemData = new Equip(data.dwObjectID, data);
        }
        else if (tempData.BaseType == ItemBaseType.ItemBaseType_Material)
        {
            if (tempData.BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.Gem)
            {
                itemData = new Gem(data.dwObjectID, data);
            }
            else if (tempData.BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.Runestone)
            {
                itemData = new RuneStone(data.dwObjectID, data);
            }
        }

        if (null == itemData)
        {
            itemData = new BaseItem(data.dwObjectID, data);
        }
        bool bPush = false;

        if (!ExistItem(itemData.QWThisID)
            && action != (uint)AddItemAction.AddItemAction_Load)
        {
            //
            bPush = true;
        }

        if (action == (uint)AddItemAction.AddItemAction_TaskReward)
        {
            //统一获取物品处理
            //TipsManager.Instance.ShowTips(string.Format("获得{0}", itemData.Name));
        }
        //1、添加到物品列表
        if (itemDataDic.ContainsKey(data.qwThisID)
            && action != (uint)AddItemAction.AddItemAction_Refresh)
        {
            Engine.Utility.Log.Warning(CLASS_NAME + "-> OnGetItemList Add Faild,exist QwThisID=[{0}]", data.qwThisID);
            return;
        }
        m_passData.QWThisId = itemData.QWThisID;
        m_passData.BaseId = itemData.BaseId;
        m_passData.AddItemAction = (AddItemAction)action;
        if (action != (uint)AddItemAction.AddItemAction_Refresh)
        {
            m_passData.UpdateType = ItemDefine.UpdateItemType.Add;
            m_passData.ChangeNum = (int)itemData.Num;
            itemDataDic.Add(itemData.QWThisID, itemData);
        }
        else if (itemDataDic.ContainsKey(itemData.QWThisID))
        {
            m_passData.UpdateType = ItemDefine.UpdateItemType.Update;
            m_passData.ChangeNum = (int)itemData.Num - (int)itemDataDic[itemData.QWThisID].Num;
            itemDataDic[itemData.QWThisID] = itemData;
        }

        //3、刷新UI
        DispatchRefreshItemEvent(m_passData);
        if (bPush)
        {
            DispatchPushMessage(itemData);
        }
    }
    void DispatchPushMessage(BaseItem item)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.PUSH_ADDITEM, item);
    }

    /// <summary>
    /// 发送刷新物品数据消息
    /// </summary>
    /// <param name="passData"></param>
    public void DispatchRefreshItemEvent(ItemDefine.UpdateItemPassData passData)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_UPDATEITEM, passData);
    }

    /// <summary>
    /// 刷新装备耐久
    /// </summary>
    /// <param name="passData"></param>
    public void DispatchRefreshItemDurableEvent(ItemDefine.UpdateItemPassData passData)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.REFRESHITEMDURABLE, passData);
    }

    /// <summary>
    /// UI界面点击Item响应
    /// </summary>
    /// <param name="gridData"></param>
    public void OnUIItemGridClicked(uint qwThisId)
    {
        TipsManager.Instance.ShowItemTips(qwThisId, needCompare: true);
    }

    #region ItemGetAnim
    public class ItemGetData
    {
        public uint BaseId;
        public int Num = 0;
        public int SortId = 0;
        public float AddTime = 0;
    }

    public class ItemAimGridFlyData
    {
        public int Index = 0;
        public UIItemGetAnimGrid AnimGrid; 
        public float DelayTime = 0;
        public float CreateTime = 0;
    }

    private int m_iItemGetAnimGetSortIdStart = 0;
    private List<UIItemGetAnimGrid> m_lstActiveGetAnimGrid = new List<UIItemGetAnimGrid>();
    private List<UIItemGetAnimGrid> m_lstInActiveGetAnimGrid = new List<UIItemGetAnimGrid>();
    private Dictionary<uint, ItemGetData> m_dicItemGetAnimCacheData = new Dictionary<uint, ItemGetData>();
    private List<ItemAimGridFlyData> m_lstItemGetAnimList = new List<ItemAimGridFlyData>();

    //是否可以把动画添加等待执行到列表中
    private bool m_bCanAddItemGetAnim = true;
    //两个动画间位置偏移
    private Vector3 m_v3Offset = new Vector3(-5f,5f,0);
    //相邻两个动画之间间隔
    public const float ITEM_GET_ANIM_GAP = 0.1f;
    //物品动画需求添加多久没执行就自动放弃
    public const float ITEM_GET_ANIM_OUTLINE_TIME = 10f;
    //
    public const int ITEM_GET_ANIM_END_DEPTH = 300;
    private void ResetItemGetAnim()
    {
        m_iItemGetAnimGetSortIdStart = 0;
        m_lstActiveGetAnimGrid.Clear();
        int count = m_lstItemGetAnimList.Count;
        UIItemGetAnimGrid grid = null;
        if (count > 0)
        {
            for(int i= 0;i < count;i++)
            {
                grid = m_lstItemGetAnimList[i].AnimGrid;
                if (null == grid)
                {
                    continue;
                }
                if (m_lstActiveGetAnimGrid.Contains(grid))
                {
                    m_lstActiveGetAnimGrid.Remove(grid);
                }
                if (!m_lstInActiveGetAnimGrid.Contains(grid))
                {
                    grid.SetVisible(false);
                    m_lstInActiveGetAnimGrid.Add(grid);
                }
            }
        }
        m_lstItemGetAnimList.Clear();

        count = m_lstActiveGetAnimGrid.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                grid = m_lstActiveGetAnimGrid[i];
                if (null == grid)
                {
                    continue;
                }

                if (m_lstActiveGetAnimGrid.Contains(grid))
                {
                    m_lstActiveGetAnimGrid.Remove(grid);
                }
                if (!m_lstInActiveGetAnimGrid.Contains(grid))
                {
                    grid.SetVisible(false);
                    m_lstInActiveGetAnimGrid.Add(grid);
                }
            }
        }
        m_lstActiveGetAnimGrid.Clear();
    }

    private void ProccessItemAnim(float deltaTime)
    {
        if (m_bCanAddItemGetAnim)
        {
            StructItemGetAnimData();
        }
        CheckItemGetAnimStart();
    }

    private UIItemGetAnimGrid GetItemGetAnimGrid()
    {
        UIItemGetAnimGrid grid = null;

        if (null != m_lstInActiveGetAnimGrid && m_lstInActiveGetAnimGrid.Count > 0)
        {
            grid = m_lstInActiveGetAnimGrid[0];
            m_lstInActiveGetAnimGrid.Remove(grid);
        }
        else
        {
            GameObject tempObj = UIManager.GetResGameObj(GridID.Uiitemgetanimgrid) as GameObject;
            if (null != tempObj)
            {
                GameObject clone = NGUITools.AddChild(UIRootHelper.Instance.StretchTransRoot.gameObject, tempObj);
                if (null != clone)
                {
                    grid = clone.GetComponent<UIItemGetAnimGrid>();
                    if (null == grid)
                    {
                        grid = clone.AddComponent<UIItemGetAnimGrid>();
                    }
                }
            }
        }
        if (null != grid)
        {
            if (!grid.Visible)
                grid.SetVisible(true);
            m_lstActiveGetAnimGrid.Add(grid);
        }
        return grid;
    }

    

    /// <summary>
    /// 执行物品获取动画
    /// </summary>
    private void StructItemGetAnimData()
    {
        if (m_dicItemGetAnimCacheData.Count == 0)
        {
            return;
        }
        GameObject target = GetItemGetAnimFlyTarget();
        if (null == target)
        {
            m_dicItemGetAnimCacheData.Clear();
            return ;
        }

        int lastIndex = (null != m_lstItemGetAnimList && m_lstItemGetAnimList.Count != 0)
            ? (m_lstItemGetAnimList[m_lstItemGetAnimList.Count - 1].Index + 1) : 0;
        List<ItemGetData> list = new List<ItemGetData>(m_dicItemGetAnimCacheData.Values);
        list.Sort((left, right) =>
            {
                return left.SortId - right.SortId;
            });

        Vector3 sourcePos = Vector3.zero;
        Vector3 targetPos = UIRootHelper.Instance.StretchTransRoot.InverseTransformPoint(
            target.transform.TransformPoint(target.transform.localPosition));
        targetPos = UIRootHelper.Instance.StretchTransRoot.TransformPoint(targetPos);
        UIItemGetAnimGrid grid = null;
        
        float delayTime = 0;
        ItemGetData itemGetData = null;
        for(int i = 0,max = list.Count ;i < max;i++)
        {
            itemGetData = list[i];
            if (RealTime.time- itemGetData.AddTime >= ITEM_GET_ANIM_OUTLINE_TIME)
            {
                continue;
            }
            delayTime = lastIndex * ITEM_GET_ANIM_GAP;
            sourcePos = lastIndex * m_v3Offset;

            grid = GetItemGetAnimGrid();
            grid.transform.position = UIRootHelper.Instance.StretchTransRoot.TransformPoint(sourcePos);
            grid.InitGrid(list[i].BaseId, list[i].Num, ITEM_GET_ANIM_END_DEPTH - lastIndex
                ,UIRootHelper.Instance.StretchTransRoot.TransformPoint(sourcePos)
                ,target.transform.position,OnItemGetAnimComplete);

            m_lstItemGetAnimList.Add(new ItemAimGridFlyData()
                {
                    Index = lastIndex,
                    AnimGrid = grid,
                    CreateTime = RealTime.time,
                    DelayTime = delayTime,
                });
            lastIndex++;
        }

        m_dicItemGetAnimCacheData.Clear();
    }

    List<ItemAimGridFlyData> canStart = new List<ItemAimGridFlyData>();
    /// <summary>
    /// 检测列表中的动画组件是否可以开始动画
    /// </summary>
    private void CheckItemGetAnimStart()
    {
        int count = m_lstItemGetAnimList.Count;
        if (count == 0)
        {
            return;
        }
        canStart.Clear();
        
        ItemAimGridFlyData flyData = null;
        for(int i = 0; i < count;i++)
        {
            flyData = m_lstItemGetAnimList[i];
            if (null == flyData)
            {
                continue;
            }
            if (null == flyData.AnimGrid)
            {
                canStart.Add(flyData);
                continue;
            }
            if (RealTime.time - flyData.CreateTime >= flyData.DelayTime)
            {
                canStart.Add(flyData);
            }
        }

        count = canStart.Count;
        for (int i = 0; i < count; i++)
        {
            flyData = canStart[i];
            m_lstItemGetAnimList.Remove(flyData);
            if (null != flyData.AnimGrid)
            {
                flyData.AnimGrid.StartFlyToTarget();
            }
        }
    }

    /// <summary>
    /// 物品获取动画完毕
    /// </summary>
    /// <param name="itemId"></param>
    /// <param name="num"></param>
    private void OnItemGetAnimComplete(uint itemId,int num,UIItemGetAnimGrid grid)
    {
        if (m_lstActiveGetAnimGrid.Contains(grid))
        {
            m_lstActiveGetAnimGrid.Remove(grid);
        }
        if (!m_lstInActiveGetAnimGrid.Contains(grid))
        {
            grid.SetVisible(false);
            m_lstInActiveGetAnimGrid.Add(grid);
        }
        GameObject target = GetItemGetAnimFlyTarget();
        if (null != target)
        {
            TweenScale ts = target.GetComponent<TweenScale>();
            if (null != ts &&　!ts.enabled)
            {
                ts.ResetToBeginning();
                ts.enabled = true;
            }
        }
        
    }

    /// <summary>
    /// 获取物品获取动画目标位置对象
    /// </summary>
    /// <returns></returns>
    private GameObject GetItemGetAnimFlyTarget()
    {
        GameObject target = null;
        MainPanel mp = DataManager.Manager<UIPanelManager>().GetPanel<MainPanel>(PanelID.MainPanel);
        if (null != mp)
        {
            Transform ts = mp.CacheTransform.Find("AnchorBottom/bag");
            if (null != ts)
            {
                target = ts.gameObject;
            }
        }
        return target;
    }

    /// <summary>
    /// 添加一个物品获取动画
    /// </summary>
    /// <param name="baseId"></param>
    /// <param name="num"></param>
    public void AddItemGetAnim(uint baseId,int num)
    {
        if (m_dicItemGetAnimCacheData.ContainsKey(baseId))
        {
            m_dicItemGetAnimCacheData[baseId].Num += num;
        }
        else
        {
            m_dicItemGetAnimCacheData.Add(baseId, new ItemGetData()
                {
                    BaseId = baseId,
                    Num = num,
                    SortId = m_iItemGetAnimGetSortIdStart,
                });
        }
        m_dicItemGetAnimCacheData[baseId].AddTime = RealTime.time;
    }

    #endregion

    #region Commmon
    //baseItem缓存
    private Dictionary<uint, BaseItem> m_dicTempBaseItemCache = null;
    //最大缓存数量
    private int cacheTempBaseMaxNum = 50;
    public bool GetTT<T>(uint baseID, out T data) where T : BaseItem
    {
        data = null;
        if (null == m_dicTempBaseItemCache)
            m_dicTempBaseItemCache = new Dictionary<uint, BaseItem>();
        BaseItem temp = null;
        bool needCreate = false;
        if (!m_dicTempBaseItemCache.TryGetValue(baseID, out temp))
        {
            needCreate = true;
        }
        else if (!(temp is T))
        {
            m_dicTempBaseItemCache.Remove(baseID);
            needCreate = true;
        }

        if (needCreate)
        {
            if (data is BaseItem)
                data = (T)new BaseItem(baseID);
            if (null != temp)
            {
                m_dicTempBaseItemCache.Add(baseID, temp);
                return false;
            }
        }
        return false;
    }
    public const int CACHE_BASE_ITEM_NUM = 200;
    /// <summary>
    /// 获取一个零时对象
    /// </summary>
    /// <param name="baseID"></param>
    /// <returns></returns>
    public T GetTempBaseItemByBaseID<T>(uint baseID,ItemDefine.ItemDataType dType = ItemDefine.ItemDataType.BaseItem) where T : BaseItem
    {
        if (null == m_dicTempBaseItemCache)
            m_dicTempBaseItemCache = new Dictionary<uint, BaseItem>();
        BaseItem temp = null;
        bool needCreate = false;
        if (!m_dicTempBaseItemCache.TryGetValue(baseID,out temp))
        {
            needCreate = true;
        }
        else if (!(temp is T))
        {
            m_dicTempBaseItemCache.Remove(baseID);
            needCreate = true;
        }

        if (needCreate)
        {
            switch(dType)
            {
                case ItemDefine.ItemDataType.BaseItem:
                    temp = new BaseItem(baseID);
                    break;
                case ItemDefine.ItemDataType.BaseEquip:
                    temp = new BaseEquip(baseID, null);
                    break;
                case ItemDefine.ItemDataType.Equip:
                    temp = new Equip(baseID);
                    break;
                case ItemDefine.ItemDataType.Gem:
                    temp = new Gem(baseID);
                    break;
                case ItemDefine.ItemDataType.RuneStone:
                    temp = new RuneStone(baseID);
                    break;
            }
            if (m_dicTempBaseItemCache.Count >= CACHE_BASE_ITEM_NUM)
            {
                m_dicTempBaseItemCache.Clear();
            }
            if (null != temp)
                m_dicTempBaseItemCache.Add(baseID, temp);
        }
        if (null != temp)
        {
            if (!needCreate)
                temp.UpdateData(baseID);
            return (T)temp;
        }
        return default(T);
    }
    /// <summary>
    /// 获取物品单日使用最大次数 0： 无限制 >0:具体数量
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public int GetItemDailyUseTimes(uint baseId)
    {
        return GetItemDailyUseTimes(DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseId));
    }

    /// <summary>
    /// 获取物品单日使用最大次数 0： 无限制 >0:具体数量
    /// </summary>
    /// <param name="baseItem"></param>
    /// <returns></returns>
    public int GetItemDailyUseTimes(BaseItem baseItem)
    {
        return (null != baseItem) ? baseItem.DailyUseNum : 0;
    }
    #endregion

    #region 消耗品使用次数

    //单个物品使用次数
    private Dictionary<uint, int> m_dicItemUseData = null;
    //物品类别使用次数
    private Dictionary<uint, int> m_dicItemUseTypeData = null;

    /// <summary>
    ///物品使用
    /// </summary>
    /// <param name="baseId"></param>
    /// <param name="num"></param>
    private void OnItemUse(uint baseId,int num)
    {
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseId);
        if (baseItem.HasDailyUseLimit)
        {
            if (m_dicItemUseData.ContainsKey(baseId))
            {
                m_dicItemUseData[baseId] += num;
            }
            else
            {
                m_dicItemUseData.Add(baseId, num);
            }

            //使用类别限制
            if (baseItem.IsUseTypeEnable)
            {
                if (m_dicItemUseTypeData.ContainsKey(baseItem.UseTypeId))
                {
                    m_dicItemUseTypeData[baseItem.UseTypeId] += num;
                }
                else
                {
                    m_dicItemUseTypeData.Add(baseItem.UseTypeId, num);
                }
            }
        }
    }
    /// <summary>
    /// 使用物品返回
    /// </summary>
    /// <param name="cmd"></param>
    public void OnItemUse(GameCmd.stUseItemPropertyUserCmd_CS msg)
    {
        OnItemUse(msg.qwBaseID, (int)msg.dwNumber);
    }
    /// <summary>
    /// 服务器下发物品使用次数
    /// </summary>
    public void OnServerItemUseGet(stGetItemUseTimesDataUserCmd_S msg)
    {
        m_dicItemUseData.Clear();
        m_dicItemUseTypeData.Clear();
        if (null != msg.use_item_data)
        {
            int limitNum = 0;
            for(int i = 0 ,max = msg.use_item_data.Count;i < max;i++)
            {
                if (msg.use_item_data[i].times == 0)
                {
                    continue;
                }
                OnItemUse(msg.use_item_data[i].base_id, (int)msg.use_item_data[i].times);
            }
        }
    }

    /// <summary>
    /// 是否有每日使用限制
    /// </summary>
    /// <param name="baseId">表格id</param>
    /// <param name="limitNum">每日限制数量</param>
    /// <returns></returns>
    public static bool HasDailyUseLimit(uint baseId,out int limitNum)
    {
        limitNum = 0;
        table.ItemDataBase itemDB = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(baseId);
        if (null != itemDB)
        {
            if (itemDB.useTypeId != 0)
            {
                table.ItemUseTypeDataBase uDB = GameTableManager.Instance.GetTableItem<table.ItemUseTypeDataBase>(itemDB.useTypeId);
                if (null != uDB)
                {
                    limitNum = (int)uDB.dayUseTimes;
                }
                return true;
            }else if (itemDB.maxUseTimes != 0)
            {
                limitNum = (int)itemDB.maxUseTimes;
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 是否可以使用该消耗品
    /// </summary>
    /// <param name="baseId"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public bool CanUseConsumerItem(uint baseId,int num = 1)
    {
        BaseItem baseItem = GetTempBaseItemByBaseID<BaseItem>(baseId);
        if (baseItem.BaseType != ItemBaseType.ItemBaseType_Consumption)
        {
            //return false;//注释此行，凭证类也可以使用此方法
        }
        int limitNum = 0;
        bool canUse = true;
        if (HasDailyUseLimit(baseId, out limitNum))
        {
            int useNum = GetItemAlreadyUseNum(baseId);
            if (limitNum < (useNum + num))
            {
                canUse = false;
                //道具剩余使用为0   客户端主动截断,并飘字  下面这个枚举对应的文本是“今日使用次数已达上限”
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Cangbaotu_Commond_shiyongshangxian);
            }
        }
        return canUse;
    }

    /// <summary>
    /// 获取已经使用次数
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public int GetItemAlreadyUseNum(uint baseId)
    {
        int useNum = 0;
        BaseItem baseItem = GetTempBaseItemByBaseID<BaseItem>(baseId);
        baseItem.UpdateData(baseId);
        if (baseItem.IsUseTypeEnable && m_dicItemUseTypeData.ContainsKey(baseItem.UseTypeId))
        {
            useNum = m_dicItemUseTypeData[baseItem.UseTypeId];
        }
        else if (baseItem.MaxUseNum > 0 && m_dicItemUseData.ContainsKey(baseId))
        {
            useNum = m_dicItemUseData[baseId];
        }
        return useNum;
    }

    /// <summary>
    /// 获取剩余使用次数
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public int GetItemLeftUseNum(uint baseId)
    {
        int useNum = GetItemAlreadyUseNum(baseId);
        BaseItem baseItem = GetTempBaseItemByBaseID<BaseItem>(baseId);
        int leftNum = baseItem.DailyUseNum - useNum;
        return Mathf.Max(0, leftNum);
    }


    #endregion

    #region 跳转(Jump)

    public static bool DoJump(ItemDefine.LocalJumpWayData localJump, object param3 = null)
    {
        bool success = false;
        if (null != localJump)
        {
            if (localJump.NeedMatchCondi)
            {
                bool warning = false;
                List<ItemDefine.ItemJumCondiMatchData> jumCondDatas = localJump.CondiMatchData;

                ItemDefine.ItemJumCondiMatchData tempMatchData = null;
                for(int i =0 ; i < jumCondDatas.Count;i++)
                {
                    tempMatchData = jumCondDatas[i];
                    switch(tempMatchData.MatchType)
                    {
                        case ItemDefine.ItemJumpCondiMatchType.Lv:
                            int playerLv = DataManager.Instance.PlayerLv;
                            if (playerLv < tempMatchData.MatchParam)
                            {
                                warning = true;
                            }
                            break;
                        case ItemDefine.ItemJumpCondiMatchType.Clan:
                            {
                                int clanStatus = DataManager.Manager<ClanManger>().IsJoinFormal ? 1 : 0;
                                if (tempMatchData.MatchParam != clanStatus)
                                {
                                    warning = true;
                                }
                            }
                            break;
                    }

                    if (warning)
                    {
                        break;
                    }
                }

                if (warning)
                {
                    if (localJump.SecondJumpWayId == 0)
                    {
                        TipsManager.Instance.ShowTips(tempMatchData.MatchNotice);
                        return false;
                    }else
                    {
                        return DoJump(localJump.SecondJumpWayId, param3);
                    }
                }
            }
            if (localJump.IJT != ItemDefine.ItemJumpType.Describe)
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.GetWayPanel);
            }
            switch (localJump.IJT)
            {
                case ItemDefine.ItemJumpType.Wild:
                    {
                        if (null != localJump.WildData)
                        {
                            Client.IController ctrl = Client.ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
                            if (null != ctrl)
                            {
                                DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
                                {
                                    ctrl.GotoMap(localJump.WildData.MapID, localJump.WildData.DesPos);
                                    success = true;
                                }, null);
                               
                            }
                        }
                    }
                    break;
                case ItemDefine.ItemJumpType.Panel:
                    {
                        if (null != localJump.PanelData)
                        {
                            if (localJump.NeedFillParam3)
                            {
                                localJump.PanelData.PanelJData.Param = param3;
                            }
                            success = true;
                            DataManager.Manager<UIPanelManager>().ShowPanel(localJump.PanelData.PID, jumpData: localJump.PanelData.PanelJData);
                        }
                    }
                    break;
                case ItemDefine.ItemJumpType.NPC:
                    {
                        if (null != localJump.NpcData)
                        {
                            Client.IController ctrl = Client.ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
                            if (null != ctrl)
                            {
                                IMapSystem mapSystem = Client.ClientGlobal.Instance().GetMapSystem();
                                if (mapSystem.GetMapID() != localJump.NpcData.MapID)
                                {
                                    if (!KHttpDown.Instance().SceneFileExists(localJump.NpcData.MapID))
                                    {
                                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                                        return true;
                                    }

                                    DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
                                    {
                                        ctrl.VisiteNPC(localJump.NpcData.MapID, localJump.NpcData.NPCID, true);                                  
                                    }, null);
                                 
                                }
                                else
                                {
                                    DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
                                    {
                                        ctrl.VisiteNPC(localJump.NpcData.MapID, localJump.NpcData.NPCID);
                                        success = true;
                                    }, null);

                                }                             
                            }
                        }
                    }
                    break;
                case ItemDefine.ItemJumpType.Describe:
                    {
                        if (null != localJump.DesData)
                        {
                            string des = localJump.DesData.des;
                            stGetWayDescription desData = new stGetWayDescription()
                            {
                                bShow = true,
                                des = des,
                            };
                            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.GetWayPanel, UIMsgID.eRefreshGetWayParam, desData);
                            success = true;
                        }
                    }
                    break;
            }
        }
        return success;
    }

    public static bool DoJump(uint jumpWayId, object param3 = null)
    {
        ItemDefine.LocalJumpWayData localJump = ItemDefine.LocalJumpWayData.Create(jumpWayId);
        return DoJump(localJump, param3);
    }

    public static bool DoGetJump(uint getId, uint id)
    {
        table.ItemGetDataBase ig = GameTableManager.Instance.GetTableItem<table.ItemGetDataBase>(getId);
        if (null == ig)
        {
            return false;
        }
        return DoJump(ig.jumpID, id);
    }

    /// <summary>
    /// 物品跳转
    /// </summary>
    /// <param name="jumpID"></param>
    /// <param name="baseId">物品id</param>
    public static bool DoItemJump(uint jumpID, uint baseId = 0, uint qwThisId = 0)
    {
        table.ItemJumpDataBase ij = GameTableManager.Instance.GetTableItem<table.ItemJumpDataBase>(jumpID);
        if (null == ij)
        {
            return false;
        }
        uint jumpWayID = 0;
        if (ij.proLimit == 1)
        {
            //职业限定
            int job = DataManager.Job();
            switch(job)
            {
                //战士
                case (int)GameCmd.enumProfession.Profession_Soldier:
                    jumpWayID = ij.mwJumpID;
                    break;
                //暗巫
                case (int)GameCmd.enumProfession.Profession_Doctor:
                    jumpWayID = ij.wmJumpID;
                    break;
                //刺客(幻师)
                case (int)GameCmd.enumProfession.Profession_Spy:
                    jumpWayID = ij.bcJumpID;
                    break;
                //法师
                case (int)GameCmd.enumProfession.Profession_Freeman:
                    jumpWayID = ij.llJumpID;
                    break;
            }
        }
        else
        {
            jumpWayID = ij.jumpID;
        }
        ItemDefine.LocalJumpWayData localJump = ItemDefine.LocalJumpWayData.Create(jumpWayID);
        if (null != localJump)
        {
            uint params3 = 0;
            if (localJump.NeedFillParam3)
            {
                params3 = (localJump.FillParam3WithThisID) ? qwThisId : baseId;
            }
            return DoJump(localJump, params3);
        }
        return false;
    }
    #endregion

    #region IManager Method
    public void Initialize()
    {
        itemDataDic = new Dictionary<uint, BaseItem>();

        ready = true;
        RegisterGlobalEvent(true);
        
        m_passData = new ItemDefine.UpdateItemPassData();
        m_lstTempItemList = new List<BaseItem>();
        
        TimerAxis.Instance().KillTimer(ITEMCD_TIMERID, this);
        TimerAxis.Instance().SetTimer(ITEMCD_TIMERID, 1000, this);

        m_dicItemUseData = new Dictionary<uint, int>();
        m_dicItemUseTypeData = new Dictionary<uint, int>();

        m_dicTempBaseItemCache = new Dictionary<uint, BaseItem>();
    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            //重登清空本地物品信息
            itemDataDic.Clear();
            m_dicItemUseData.Clear();
            m_dicItemUseTypeData.Clear();
            m_dicTempBaseItemCache = null;
        }

        m_lstTempItemList = new List<BaseItem>();
        
        //CD
        m_dicItemCDByBaseId.Clear();
        m_dicItemCDByGroupId.Clear();

        ResetItemGetAnim();
    }

    public void Process(float deltaTime)
    {
        if (ready)
        {
            ProccessItemAnim(deltaTime);
        }
    }

    public void ClearData()
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
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
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
            case (int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM:
                {
                    ItemDefine.UpdateCurrecyPassData passData = null;
                    if (null != data && data is ItemDefine.UpdateCurrecyPassData)
                    {
                        passData = data as ItemDefine.UpdateCurrecyPassData;
                    }
                    OnCurrencyChanged(passData);
                }
                break;

            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                {
                    ItemDefine.UpdateItemPassData passData = null;
                    if (null != data && data is ItemDefine.UpdateItemPassData)
                    {
                        passData = data as ItemDefine.UpdateItemPassData;
                    }
                    OnItemChanged(passData);
                }
                break;
        }
    }

    /// <summary>
    /// 钱币改变
    /// </summary>
    /// <param name="moneyType">钱币类型</param>
    /// <param name="changeNum">改变数量</param>
    private void OnCurrencyChanged(ItemDefine.UpdateCurrecyPassData passData)
    {
        if (null != passData)
        {
            string formatTips = "获得{0}x{1}";
            if (passData.ChangeNum > 0)
            {
                formatTips = string.Format(formatTips
                    , DataManager.Manager<TextManager>().GetCurrencyNameByType(passData.MoneyType)
                    , passData.ChangeNum);
                TipsManager.Instance.ShowTips(formatTips);
            }
        }
    }

    /// <summary>
    /// 物品改变
    /// </summary>
    /// <param name="passData"></param>
    private void OnItemChanged(ItemDefine.UpdateItemPassData passData)
    {
        if (null != passData)
        {
            if (passData.AddItemAction != GameCmd.AddItemAction.AddItemAction_Load
                && passData.AddItemAction != GameCmd.AddItemAction.AddItemAction_Sort
                && passData.AddItemAction != GameCmd.AddItemAction.AddItemAction_Union
                && passData.ChangeNum > 0
                && (passData.UpdateType == ItemDefine.UpdateItemType.Add
                || passData.UpdateType == ItemDefine.UpdateItemType.Update))
            {
                BaseItem baseItem = GetBaseItemByQwThisId(passData.QWThisId);
                //提示
                if (null != baseItem 
                    && passData.AddItemAction != AddItemAction.AddItemAction_EquipExchange)
                {
                    string formatTips = "获得{0}x{1}";
                    formatTips = string.Format(formatTips
                     , baseItem.LocalName
                    , passData.ChangeNum);
                    TipsManager.Instance.ShowTips(formatTips);
                }

                //获取动画
                AddItemGetAnim(passData.BaseId, passData.ChangeNum);
            }
        }
    }
    #endregion

    #region ItemCD
    public class CDData
    {
        public float totalCD;  //总cd
        public float remainCD; //剩余时间cd 
        public float startTime;//开始的时间
    }

    Dictionary<uint, CDData> m_dicItemCDByBaseId = new Dictionary<uint, CDData>(); //没有组  baseId   cd

    Dictionary<uint, CDData> m_dicItemCDByGroupId = new Dictionary<uint, CDData>();  //有组  groupId    cd

    /// <summary>
    /// 添加item  CD数据到本地
    /// </summary>
    /// <param name="baseid"></param>
    /// <param name="cdinfos"></param>
    public void AddItemCDDataToDic(uint baseid, List<GameCmd.CDInfo> cdinfos)
    {
        for (int i = 0; i < cdinfos.Count; i++)
        {
            //物品本身cd
            if (cdinfos[i].type == 0)
            {
                CDData cdData = new CDData() { totalCD = cdinfos[i].cd, remainCD = cdinfos[i].cd, startTime = UnityEngine.Time.realtimeSinceStartup };

                if (m_dicItemCDByBaseId.ContainsKey(baseid) == false)
                {
                    m_dicItemCDByBaseId.Add(baseid, cdData);
                }
                else
                {
                    m_dicItemCDByBaseId[baseid] = cdData;
                }
            }

            //组cd
            if (cdinfos[i].type == 1)
            {
                table.ItemDataBase itemdata = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(baseid);
                if (itemdata == null)
                {
                    continue;
                }
                table.EffectFuncDataBase effectdata = GameTableManager.Instance.GetTableItem<table.EffectFuncDataBase>(itemdata.func_id);
                if (effectdata == null)
                {
                    continue;
                }

                CDData cdData = new CDData() { totalCD = cdinfos[i].cd, remainCD = cdinfos[i].cd, startTime = UnityEngine.Time.realtimeSinceStartup };
                if (m_dicItemCDByGroupId.ContainsKey(effectdata.group_id))
                {
                    m_dicItemCDByGroupId.Add(effectdata.group_id, cdData);
                }
                else
                {
                    m_dicItemCDByGroupId[effectdata.group_id] = cdData;
                }
            }
        }

        Client.stUseItemCD useItemCd = new Client.stUseItemCD { itemBaseId = baseid };

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_USEITEMCD, useItemCd);
    }


    public bool TryGetItemCDByBaseId(uint baseId, out  CDData cdData)
    {
        table.ItemDataBase itemdata = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(baseId);
        if (itemdata == null)
        {
            cdData = null;
            return false;
        }

        table.EffectFuncDataBase effectdata = GameTableManager.Instance.GetTableItem<table.EffectFuncDataBase>(itemdata.func_id);
        if (effectdata == null)
        {
            cdData = null;
            return false;
        }

        uint groupid = effectdata.group_id;

        CDData groupIdCdData;
        CDData baseIdCdData;
        if (m_dicItemCDByGroupId.TryGetValue(effectdata.group_id, out groupIdCdData) && m_dicItemCDByBaseId.TryGetValue(baseId, out baseIdCdData))
        {
            groupIdCdData.remainCD = groupIdCdData.totalCD - (UnityEngine.Time.realtimeSinceStartup - groupIdCdData.startTime);
            baseIdCdData.remainCD = baseIdCdData.totalCD - (UnityEngine.Time.realtimeSinceStartup - baseIdCdData.startTime);

            if (groupIdCdData.remainCD > 0 && groupIdCdData.remainCD >= baseIdCdData.remainCD)
            {
                cdData = groupIdCdData;
                return true;
            }

            if (baseIdCdData.remainCD > 0 && baseIdCdData.remainCD > groupIdCdData.remainCD)
            {
                cdData = baseIdCdData;
                return true;
            }
        }

        //组cd
        if (m_dicItemCDByGroupId.TryGetValue(effectdata.group_id, out cdData))
        {
            cdData.remainCD = cdData.totalCD - (UnityEngine.Time.realtimeSinceStartup - cdData.startTime);
            if (cdData.remainCD > 0)
            {
                return true;
            }
            else
            {
                m_dicItemCDByGroupId.Remove(effectdata.group_id);
                return false;
            }
        }

        //物品CD
        if (m_dicItemCDByBaseId.TryGetValue(baseId, out cdData))
        {
            cdData.remainCD = cdData.totalCD - (UnityEngine.Time.realtimeSinceStartup - cdData.startTime);
            if (cdData.remainCD > 0)
            {
                return true;
            }
            else
            {
                m_dicItemCDByBaseId.Remove(baseId);
                return false;
            }
        }

        cdData = null;
        return false;
    }

    /// <summary>
    /// 是否正在跑cd
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public bool IsRuningItemCD(uint baseId)
    {
        CDData cdData;
        if (TryGetItemCDByBaseId(baseId, out cdData))
        {
            return true;
        }
        return false;
    }

    public bool TryGetItemCDByThisId(uint qwThisId, out  CDData cdData)
    {
        BaseItem baseitem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(qwThisId);
        if (baseitem == null)
        {
            cdData = null;
            return false;
        }

        if (TryGetItemCDByBaseId(baseitem.BaseId, out cdData))
        {
            return true;
        }

        cdData = null;
        return false;
    }

    public void OnTimer(uint uTimerID)
    {
        if (ITEMCD_TIMERID == uTimerID)
        {
            List<uint> removeBaseId = new List<uint>();
            Dictionary<uint, CDData>.Enumerator baseEtor = m_dicItemCDByBaseId.GetEnumerator();
            while (baseEtor.MoveNext())
            {
                CDData cdData = baseEtor.Current.Value;
                if (UnityEngine.Time.realtimeSinceStartup - cdData.startTime >= cdData.totalCD)
                {
                    removeBaseId.Add(baseEtor.Current.Key);
                }
            }

            //移除时间到的
            for (int i = 0; i < removeBaseId.Count; i++)
            {
                m_dicItemCDByBaseId.Remove(removeBaseId[i]);
            }

            List<uint> removeGroupId = new List<uint>();
            Dictionary<uint, CDData>.Enumerator groupEtor = m_dicItemCDByGroupId.GetEnumerator();
            while (groupEtor.MoveNext())
            {
                CDData cdData = groupEtor.Current.Value;
                if (UnityEngine.Time.realtimeSinceStartup - cdData.startTime >= cdData.totalCD)
                {
                    removeGroupId.Add(groupEtor.Current.Key);
                }
            }

            //移除时间到的
            for (int i = 0; i < removeGroupId.Count; i++)
            {
                m_dicItemCDByGroupId.Remove(removeGroupId[i]);
            }

        }
    }

    #endregion

}