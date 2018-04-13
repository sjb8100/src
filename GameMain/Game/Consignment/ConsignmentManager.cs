/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Consignment
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ConsignmentManager
 * 版本号：  V1.0.0.0
 * 创建时间：11/14/2016 9:39:40 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using table;
using GameCmd;

public enum SaleItemDispatchEvents
{
    None,
    RefreshSaleItemInfo,
    RefreshSellItemInfo,
    BuyConsignItemInfo,
    RefreshMyConsignList,
    ReFreshMyConsignMoney,
    RefreshSaleRecord,
    GetAllStarItems,//获取所有的收藏物品信息
    RefreshSingleStarState,//刷新单个的收藏信息
    RefreshItemPrePrice,//刷新物品昨天的平均价格
}

public enum ReqConsignListState
{
    None,
    PreviousPage,
    NextPage,
    ResetPage,
    Max,
}

public class MyConsignItemInfo
{
    public List<ConsignmentItem> consignmentItemList = new List<ConsignmentItem>();
}
public class MyConsignStarInfo
{
    public List<ConsignmentItem> consignmentItemList = new List<ConsignmentItem>();
//     public List<ItemPageInfo> itemInfoList = new List<ItemPageInfo>();
// 
//     public List<ItemSerialize> itemDatas = new List<ItemSerialize>();
}

class ConsignmentManager : BaseModuleData, IManager
{
    private List<ConsignmentCateIDConf> m_cateIdConfigList = new List<ConsignmentCateIDConf>();
    private Dictionary<uint, List<ConsignmentCateIDConf>> m_subCateIdConfigDic = new Dictionary<uint, List<ConsignmentCateIDConf>>();
    private List<ConsignmentFilterConf> m_filterConfigList = new List<ConsignmentFilterConf>();
    private List<ConsignmentCanSellItem> m_CanSellItemList = new List<ConsignmentCanSellItem>();
    private object wantSellItem = null;
    //private stItemSellInfoConsignmentUserCmd_S itemSellInfo = null;
    //寄售列表信息
    private MyConsignItemInfo itemSellInfo = null;
    //收藏列表信息
    private MyConsignStarInfo itemStarInfo = null;
    private stItemSellLogConsignmentUserCmd_S itemTradeLogInfo = null;
    private bool needReqItemSellLog = true;
    private Dictionary<ulong, BaseItem> m_itemInfoDic = new Dictionary<ulong, BaseItem>();
    public object WantSellItem
    {
        get
        {
            return wantSellItem;
        }
        set
        {
            wantSellItem = value;
        }
    }

    ReqConsignListState reqItemListState;
    public ReqConsignListState ReqItemListState
    {
        get
        {
            return reqItemListState;
        }
        set
        {
            reqItemListState = value;
        }
    }
    private bool isSearching = false;
    public bool IsSearching
    {
        get
        {
            return isSearching;
        }
        private set
        {
            isSearching = value;
        }
    }
    //普通物品推荐价的浮动
    public int MaxRecommendPricePercent
    {
        set;
        get;
    }
    //单次浮动量
    public int SinglePriceChageValue
    {
        set;
        get;
    }
    //最大寄卖手续费
    public int MaxSellCost
    {
        set;
        get;
    }
    public int UnitYuanBaoToJinBi 
    {
        private set;
        get;
    }
//     public uint CurStarPage
//     {
//         get;
//         private set;
//     }
//     uint allStarPage = 1;
//     public uint AllStarPage
//     {
//         get
//         {
//             return allStarPage;
//         }
//         private set
//         {
//             allStarPage = value / 8 + ((value % 8) > 0 ? 1 : (uint)0);
//             allStarPage = allStarPage > 0 ? allStarPage : 1;
//         }
//     }
    public uint CurPage
    {
        get;
        private set;
    }
    public int BeginIndex
    {
        get;
        private set;
    }
    public uint EndIndex
    {
        get;
        private set;
    }

    uint allPage = 1;
    public uint AllPage
    {
        get
        {
            return allPage;
        }
        private set
        {
            allPage = value / 8 + ((value % 8) > 0 ? 1 : (uint)0);
            allPage = allPage > 0 ? allPage : 1;
        }
    }

    public uint ConsignTotalJinBi
    {
        get;
        set;
    }
    public uint ConsignTotalYuanBao
    {
        get;
        set;
    }
    private List<ulong> allStarMarkedIDs = new List<ulong>();
    public List<ulong> AllStarMarkedIDs
    {
        get 
        {
            return allStarMarkedIDs;
        }
        set 
        {
            allStarMarkedIDs = value;
        }
    }
    private List<ConsignmentItem> pageConsignmentItemLists = null;
    #region IManager
    public void Initialize()
    {
       MaxRecommendPricePercent= GameTableManager.Instance.GetGlobalConfig<int>("MaxRecommendPrice");
       SinglePriceChageValue = GameTableManager.Instance.GetGlobalConfig<int>("SinglePriceChageValue");
       MaxSellCost = GameTableManager.Instance.GetGlobalConfig<int>("MaxSellCost");
       UnitYuanBaoToJinBi = GameTableManager.Instance.GetGlobalConfig<int>("UnitYuanBaoToJinBi");
       pageConsignmentItemLists =new List<ConsignmentItem>();
       maxStarItemNum = GameTableManager.Instance.GetGlobalConfig<int>("MaxStarItemCount");
    }

    public void Reset(bool depthClearData = false)
    {

    }

    public void Process(float deltaTime)
    {

    }
    public void ClearData()
    {

    }
    #endregion

    /// <summary>
    /// 设置寄售物品分类数据
    /// </summary>
    public void SetConsignmentList()
    {
        if (m_cateIdConfigList.Count > 0)
        {
            return;
        }
        List<ConsignmentCateIDConf> dataList = GameTableManager.Instance.GetTableList<ConsignmentCateIDConf>();
        if (null == dataList)
        {
            Engine.Utility.Log.Error("ConsignmentCateIDConf is null");
            return;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            ConsignmentCateIDConf item = dataList[i];
            if (null != item)
            {
                uint iD = item.ID;
                if (!m_subCateIdConfigDic.ContainsKey(iD))
                {
                    m_cateIdConfigList.Add(item);
                }
                List<ConsignmentCateIDConf> list = null;
                if (m_subCateIdConfigDic.TryGetValue(iD, out list))
                {
                    if (list != null)
                    {
                        list.Add(item);
                    }
                }
                else
                {
                    list = new List<ConsignmentCateIDConf> {
                                item
                            };
                    this.m_subCateIdConfigDic.Add(iD, list);
                }
            }
        }
        //清空一下“我的关注”的二级页签
        if (m_subCateIdConfigDic.ContainsKey(0))
        {
            m_subCateIdConfigDic[0].Clear();
        }
        m_filterConfigList = GameTableManager.Instance.GetTableList<ConsignmentFilterConf>();
        m_CanSellItemList = GameTableManager.Instance.GetTableList<ConsignmentCanSellItem>();
    }

    /// <summary>
    /// 获取寄售物品分类
    /// </summary>
    /// <param name="list">大分类</param>
    /// <param name="dic">子分类</param>
    public void GetConsignmentCateInfo(ref List<ConsignmentCateIDConf> list, ref Dictionary<uint, List<ConsignmentCateIDConf>> dic)
    {
        list = m_cateIdConfigList;
        dic = m_subCateIdConfigDic;
    }

    public ConsignmentCateIDConf GetConsignmentCateIDConf(uint ID)
    {
        for (int i = 0; i < m_cateIdConfigList.Count; i++)
        {
            if (m_cateIdConfigList[i].ID == ID)
            {
                return m_cateIdConfigList[i];
            }
        }
        return null;
    }

    public ConsignmentCateIDConf GetSubConsignmentCateIDConf(uint ID, uint SubID)
    {
        if (m_subCateIdConfigDic.ContainsKey(ID))
        {
            for (int i = 0; i < m_subCateIdConfigDic[ID].Count; i++)
            {
                if (m_subCateIdConfigDic[ID][i].SubID == SubID)
                {
                    return m_subCateIdConfigDic[ID][i];
                }
            }
        }
        return null;
    }

    public ConsignmentFilterConf GetConsignmentFilterInfo(uint ID)
    {
        return m_filterConfigList.Query(ID);
    }

    public ConsignmentCanSellItem GetConsignmentCanSellItemInfo(uint ID)
    {
        return m_CanSellItemList.Query(ID);
    }

    /// <summary>
    /// 获取寄售物品信息
    /// </summary>
    /// <returns></returns>
    public List<ConsignmentItem> GetPageItemLists()
    {
        return pageConsignmentItemLists;
;
    }

    /// <summary>
    /// 返回寄售列表
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponItemListConsignment(stResponItemListConsignmentUserCmd_S cmd)
    {
        //处理页数
        switch (ReqItemListState)
        {
            case ReqConsignListState.PreviousPage:
                CurPage = CurPage > 1 ? CurPage - 1 : 1;
                break;
            case ReqConsignListState.NextPage:
                CurPage += 1;
                break;
            default:
                CurPage = 1;
                break;
        }
        ReqItemListState = ReqConsignListState.None;
        BeginIndex = (int)cmd.begin_index;
        EndIndex = cmd.end_index;
        AllPage = cmd.all_item;
        if (cmd.init_page)
        {
            CurPage = 1;
        }
        pageConsignmentItemLists.Clear();
        if (cmd.item_list.Count == cmd.item_data.Count)
        {
            ItemSerialize itemData = new ItemSerialize();
            for (int i = 0; i < cmd.item_data.Count; i++)
            {
                if (cmd.item_data[i] != null)
                {
                    itemData = ItemSerialize.Deserialize(cmd.item_data[i]);
                    ConsignmentItem consignment = new ConsignmentItem(cmd.item_list[i].market_id, cmd.item_list[i], itemData);
                    pageConsignmentItemLists.Add(consignment);
                }
            }
        }
        else 
        {
            //堆叠物品  下发的itemList和ItemData不对等  需要自己赋值
            ItemSerialize itemData = new ItemSerialize();
            Dictionary<uint,ItemSerialize> tempDic = new Dictionary<uint,ItemSerialize>();
            for (int i = 0; i < cmd.item_data.Count; i++)
            {
                if (cmd.item_data[i] != null)
                {
                    itemData = ItemSerialize.Deserialize(cmd.item_data[i]);
                    if (itemData != null)
                    {
                        if (!tempDic.ContainsKey(itemData.dwObjectID))
                        {
                            tempDic.Add(itemData.dwObjectID, itemData);
                        }
                    }
                }
            }

            for (int i = 0; i < cmd.item_list.Count; i++)
            {
                if (cmd.item_list[i] != null)
                {
                    if (tempDic.ContainsKey(cmd.item_list[i].item_base_id))
                    {
                        ConsignmentItem consignment = new ConsignmentItem(cmd.item_list[i].market_id, cmd.item_list[i], tempDic[cmd.item_list[i].item_base_id]);
                        pageConsignmentItemLists.Add(consignment);
                    }
                  
                }
            }
        }
        pageConsignmentItemLists.Sort(CompareConsignment);
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.RefreshSaleItemInfo.ToString(), null, null));
    }

    /// <summary>
    /// 请求寄售列表
    /// </summary>
    /// <param name="sub_type"></param>
    /// <param name="filter_0"></param>
    /// <param name="filter_1"></param>
    /// <param name="sortType"></param>
    /// <param name="begin_index"></param>
    /// <param name="page_up_or_down"></param>
    public void ReqConsignmentItemList(uint sub_type, uint filter_0, uint filter_1, ItemSortType sortType, int begin_index, uint page_up_or_down,ShowWhatItem showType = ShowWhatItem.ShowNormalItem)
    {
        stRequestItemListConsignmentUserCmd_C cmd = new stRequestItemListConsignmentUserCmd_C();
        cmd.sub_type = sub_type;
        cmd.filter_0 = filter_0;
        cmd.filter_1 = filter_1;
        cmd.sort = sortType;
        cmd.begin_index = begin_index;
        cmd.page_up_or_down = page_up_or_down;
        cmd.what_item = showType;
        NetService.Instance.Send(cmd);
        IsSearching = false;
    }

    /// <summary>
    /// 寄售成功
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponSellConsignItem(stSellItemConsignmentUserCmd_S cmd)
    {
        if (itemSellInfo == null)
        {
            itemSellInfo = new MyConsignItemInfo();
        }
        ItemSerialize  data = ItemSerialize.Deserialize(cmd.item_data);
        ConsignmentItem consignment = new ConsignmentItem(cmd.page_info.market_id, cmd.page_info, data, cmd.time_info);
        itemSellInfo.consignmentItemList.Add(consignment);
        itemSellInfo.consignmentItemList.Sort(CompareConsignment);
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.RefreshSellItemInfo.ToString(), null, null));
    }

    /// <summary>
    /// 是否可寄售
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public bool IsConsignableItem(BaseItem itemData)
    {
        if (itemData != null && !itemData.IsBind && m_CanSellItemList != null && m_CanSellItemList.Query(itemData.BaseData.itemID) != null)
        {
            ItemBindDataBase itemBindData = GameTableManager.Instance.GetTableItem<ItemBindDataBase>(itemData.BaseData.bindMask);
            if (itemBindData != null && itemBindData.shop_flag > 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取寄售物品的详细信息
    /// </summary>
    /// <param name="market_id"></param>
    public void ReqItemInfoConsignment(ItemPageInfo itemInfo)
    {
        if (itemInfo == null)
        {
            return;
        }
        if (m_itemInfoDic.ContainsKey(itemInfo.market_id))
        {
            TipsManager.Instance.ShowItemTips(m_itemInfoDic[itemInfo.market_id]);
            return;
        }
        else
        {
            BaseItem itemData = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(itemInfo.item_base_id);
            if (itemData != null && itemData.OverlayNum > 1)
            {
                TipsManager.Instance.ShowItemTips(itemData);
                return;
            }
        }
        stRequestItemInfoConsignmentUserCmd_C cmd = new stRequestItemInfoConsignmentUserCmd_C();
        cmd.market_id = itemInfo.market_id;
        NetService.Instance.Send(cmd);
    }


    /// <summary>
    /// 返回寄售物品信息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponItemInfoConsignment(stResponItemInfoConsignmentUserCmd_S cmd)
    {
        ItemSerialize itemData = ItemSerialize.Deserialize(cmd.item_data);
        if (itemData != null)
        {
            BaseItem item = new BaseItem(itemData.dwObjectID, itemData);
            TipsManager.Instance.ShowItemTips(item);
            if (!m_itemInfoDic.ContainsKey(cmd.market_id))
            {
                m_itemInfoDic.Add(cmd.market_id, item);
            }
        }
    }

    /// <summary>
    /// 清空缓存的物品信息
    /// </summary>
    public void ClearItemInfo()
    {
        if (m_itemInfoDic != null)
        {
            m_itemInfoDic.Clear();
        }
    }

    /// <summary>
    /// 购买寄售物品
    /// </summary>
    /// <param name="market_id"></param>
    /// <param name="item_num"></param>
    public void ReqBuyItemConsignment(ulong market_id, uint item_num)
    {
        stBuyItemConsignmentUserCmd_CS cmd = new stBuyItemConsignmentUserCmd_CS();
        cmd.market_id = market_id;
        cmd.item_num = item_num;
        //cmd.money_type = great ? (uint)ClientMoneyType.YuanBao :  (uint)ClientMoneyType.Gold; 
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 购买成功
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponBuyConsignItem(stBuyItemConsignmentUserCmd_CS cmd)
    {
        uint curNum = 0;
        if (itemSellInfo != null)
        {
            for (int i = 0; i < itemSellInfo.consignmentItemList.Count; i++)
            {
                if (itemSellInfo.consignmentItemList[i].Market_ID == cmd.market_id)
                {
                    curNum = itemSellInfo.consignmentItemList[i].page_info.item_num;
//                     if (curNum <= 0)
//                     {
//                         itemSellInfo.consignmentItemList.RemoveAt(i);
//                     }
//                     else 
//                     {
//                         itemSellInfo.consignmentItemList[i].page_info.item_num = curNum;
//                     }               
                }
            }      
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.BuyConsignItemInfo.ToString(), curNum, cmd.market_id));
    }

    /// <summary>
    /// 收到个人寄售信息列表
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponItemSellInfo(stItemSellInfoConsignmentUserCmd_S cmd)
    {
        if (cmd != null)
        {
            if (itemSellInfo == null)
            {
                itemSellInfo = new MyConsignItemInfo();
            }
            itemSellInfo.consignmentItemList.Clear();
            ItemSerialize itData = new ItemSerialize();
            for (int i = 0; i < cmd.item_data.Count; i++)
            {
                itData = ItemSerialize.Deserialize(cmd.item_data[i]);
                if (i < cmd.item_time_list.Count && i < cmd.item_list.Count)
                {
                    ConsignmentItem consignment = new ConsignmentItem(cmd.item_list[i].market_id, cmd.item_list[i], itData, cmd.item_time_list[i]);
                    itemSellInfo.consignmentItemList.Add(consignment);
                }
            }
            itemSellInfo.consignmentItemList.Sort(CompareConsignment);
            ConsignTotalJinBi = cmd.gold;
            ConsignTotalYuanBao = cmd.coin;
        }
    }
    public MyConsignItemInfo GetItemSellInfo()
    {
        if (itemSellInfo == null)
        {
            itemSellInfo = new MyConsignItemInfo();
        }
        return itemSellInfo;
    }
    /// <summary>
    /// 获取所有的收藏物品的信息
    /// </summary>
    public void OnRecieveAllStarItemDatas(stStarItemListConsignmentUserCmd_S cmd) 
    {
       if(cmd != null)
       {
           if (itemStarInfo == null)
           {
               itemStarInfo = new MyConsignStarInfo();
           }
           itemStarInfo.consignmentItemList.Clear();
           ItemSerialize itData = new ItemSerialize();
           for (int i = 0; i < cmd.item_data.Count; i++)
           {
               itData = ItemSerialize.Deserialize(cmd.item_data[i]);
               if (i < cmd.item_list.Count)
               {
                   ConsignmentItem consignment = new ConsignmentItem(cmd.item_list[i].market_id, cmd.item_list[i], itData);
                   itemStarInfo.consignmentItemList.Add(consignment);
               }           
           }
           itemStarInfo.consignmentItemList.Sort(CompareConsignment);

           AllPage =(uint)cmd.item_list.Count;
       }
       DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.GetAllStarItems.ToString(), null, null));
    }
    /// <summary>
    /// 获取所有的公示物品信息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnRecieveAllGreatItemDatas(stGreatItemInfoConsignmentUserCmd_S cmd)
    {
        if (cmd != null)
        {
            if (itemStarInfo == null)
            {
                itemStarInfo = new MyConsignStarInfo();
            }
            itemStarInfo.consignmentItemList.Clear();
            ItemSerialize itData = new ItemSerialize();
            for (int i = 0; i < cmd.item_data.Count; i++)
            {
                itData = ItemSerialize.Deserialize(cmd.item_data[i]);
                if (i < cmd.item_list.Count)
                {
                    ConsignmentItem consignment = new ConsignmentItem(cmd.item_list[i].market_id, cmd.item_list[i], itData);
                    itemStarInfo.consignmentItemList.Add(consignment);
                }
            }
            itemStarInfo.consignmentItemList.Sort(CompareConsignment);
            AllPage = (uint)cmd.item_list.Count;  
        }
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.GetAllStarItems.ToString(), null, null));
    }

    public MyConsignStarInfo GetItemStarInfo()
    {
        if (itemStarInfo == null)
        {
            itemStarInfo = new MyConsignStarInfo();
        }
        return itemStarInfo;
    }
    public void OnChangeStarMarkedIds(ulong marked_id,bool star) 
    {
        if (star)
        {
            if (!AllStarMarkedIDs.Contains(marked_id))
            {
                AllStarMarkedIDs.Add(marked_id);
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Trading_IsConcerned);
            }
          
        }
        else 
        {
            if (AllStarMarkedIDs.Contains(marked_id))
            {
                AllStarMarkedIDs.Remove(marked_id);
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Trading_CancleConcerned);
            }
        }
       
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.RefreshSingleStarState.ToString(),marked_id, star));   
    }
    /// <summary>
    /// 取回寄售的物品
    /// </summary>
    /// <param name="cmd"></param>
    public void ReqCancelConsignItem(ulong market_id)
    {
        stCancalItemConsignmentUserCmd_CS cmd = new stCancalItemConsignmentUserCmd_CS();
        cmd.market_id = market_id;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 下架成功
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponCancelConsignItem(stCancalItemConsignmentUserCmd_CS cmd)
    {
        if (itemSellInfo != null && cmd != null)
        {
            for (int i = 0; i < itemSellInfo.consignmentItemList.Count; i++)
            {
                if (itemSellInfo.consignmentItemList[i].Market_ID == cmd.market_id)
                {
                    itemSellInfo.consignmentItemList.RemoveAt(i);
                    DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.RefreshMyConsignList.ToString(), null, null));
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 取出寄售的钱
    /// </summary>
    public void ReqGetSellMoneyConsignment()
    {
        stGetSellMoneyConsignmentUserCmd_C cmd = new stGetSellMoneyConsignmentUserCmd_C();
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 改变自己寄售的钱
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponItemSellMoney(stItemSellMoneyLogConsignmentUserCmd_S cmd)
    {
        if (cmd != null)
        {
            ConsignTotalJinBi = cmd.gold;
            ConsignTotalYuanBao = cmd.coin;
            DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.ReFreshMyConsignMoney.ToString(), null, null));
        }
    }

    /// <summary>
    /// 物品被人买走了
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponRemoveConsignItem(stRemoveItemInfoConsignmentUserCmd_S cmd)
    {
        needReqItemSellLog = true;
        BaseItem item = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId((uint)cmd.market_id);
        if (item != null)
        {
            string txt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_jishouhangchushouwupin, item.Name);
            ChatDataManager.SendToChatSystem(txt);
        }

        if (itemSellInfo != null && cmd != null)
        {
            for (int i = 0; i < itemSellInfo.consignmentItemList.Count; i++)
            {
                if (itemSellInfo.consignmentItemList[i].Market_ID == cmd.market_id)
                {
                    itemSellInfo.consignmentItemList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 请求自己的寄售记录
    /// </summary>
    public void ReqItemSellLog()
    {
//         if (needReqItemSellLog)
//         {
        stRequestSellLogConsignmentUserCmd_C cmd = new stRequestSellLogConsignmentUserCmd_C();
        NetService.Instance.Send(cmd);
//         }
//         else
//         {
//             DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.RefreshSaleRecord.ToString(), null, null));
//         }
    }

    /// <summary>
    /// 获取寄售日志
    /// </summary>
    /// <param name="cmd"></param>
    public void OnResponItemSellLog(stItemSellLogConsignmentUserCmd_S cmd)
    {
        needReqItemSellLog = false;
        itemTradeLogInfo = cmd;
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.RefreshSaleRecord.ToString(), null, null));
    }

    public stItemSellLogConsignmentUserCmd_S GetItemSellLogInfo()
    {
        return itemTradeLogInfo;
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="key_words"></param>
    public void ReqSearchConsignment(string key_words, ItemSortType sortType)
    {
        if (string.IsNullOrEmpty(key_words))
        {
            return;
        }
        stSearchConsignmentUserCmd_C cmd = new stSearchConsignmentUserCmd_C();
        cmd.key_words = key_words;
        cmd.sort = sortType;
        NetService.Instance.Send(cmd);
        IsSearching = true;
    }

    public void ReqSearchPageConsignment(int begin_index, ItemSortType sortType, uint page_up_or_down)
    {
        stSearchPageConsignmentUserCmd_C cmd = new stSearchPageConsignmentUserCmd_C();
        cmd.begin_index = begin_index;
        cmd.sort = sortType;
        cmd.page_up_or_down = page_up_or_down;
        NetService.Instance.Send(cmd);
    }


    public void OnRecieveSaleList(stResPriceInfoConsignmentUserCmd_S cmd)
    {
        ItemPriceParam par = new ItemPriceParam();
        par.baseid = cmd.item_base_id;
        par.list = cmd.data;
        par.list.Sort((left, right) =>
        {
            if (left.price > right.price)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        });
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SHOWCONSIGNMENTLIST, par);
    }
    /// <summary>
    /// 收到某件物品昨天的价格
    /// </summary>
    public void OnRecievePrePrice(uint baseid ,uint price) 
    {
        DispatchValueUpdateEvent(new ValueUpdateEventArgs(SaleItemDispatchEvents.RefreshItemPrePrice.ToString(), baseid,(int)price));
    }

    public static int  CompareConsignment(ConsignmentItem left,ConsignmentItem right)
    {
        int leftNum = 0;
        int rightNum = 0;
        if (left.page_info != null)
        {
            bool great = left.page_info.great;
            int money = (int)left.page_info.money;
            leftNum = great ? money * DataManager.Manager<ConsignmentManager>().UnitYuanBaoToJinBi : money;
        }
        if (right.page_info != null)
        {
            bool great = right.page_info.great;
            int money = (int)right.page_info.money;
            rightNum = great ? money * DataManager.Manager<ConsignmentManager>().UnitYuanBaoToJinBi : money;
        }
        return leftNum - rightNum;
    }
    int maxStarItemNum = 16;
    public bool OverflowMaxStarItem(ulong marketId) 
    {
        if (AllStarMarkedIDs.Count >= maxStarItemNum)
        {
            if (!AllStarMarkedIDs.Contains(marketId))
            {
                return true;
            }        
        }
        return false;
    }
}
public class ItemPriceParam
{
    public uint baseid;
    public List<ItemPriceInfo> list;
}