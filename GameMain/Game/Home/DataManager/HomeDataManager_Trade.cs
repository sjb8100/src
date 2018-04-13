//*******************************************************************************************
//	创建日期：	2016-9-27   11:39
//	文件名称：	HomeDataManager_Trade,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	资源回收，购买
//*******************************************************************************************

using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;

public class CropLivestock  //农业果实、家畜
{
    public uint m_nId;
    public string m_strName;
    public uint m_nNum;
    public int m_nPrice;
    public float m_fPriceChangeRate;//价格变化率
    public bool m_bUp;     //涨价
    public string m_nIcon;
    public string m_strDes;  //描述
}

public class SeedAndCub
{
    public uint id;
    public uint num;
}

partial class HomeDataManager
{
    #region property

    private Dictionary<uint, string> m_dicTabName = null;//页签名字

    public Dictionary<uint, string> TabNameDic
    {
        get
        {
            return m_dicTabName;
        }
    }

    private Dictionary<uint, List<uint>> m_dicSellItem = null;

    public Dictionary<uint, List<uint>> SellItemDic 
    {
        get 
        {
           return m_dicSellItem;
        }
    }

    private Dictionary<uint, List<uint>> m_dicBuyItem = null;

    public Dictionary<uint, List<uint>> BuyItemDic 
    {
        get 
        {
            return m_dicBuyItem;
        }
    }

    private Dictionary<uint, CropLivestock> m_dicCropLivestock = null;//农业果实、家畜

    private Dictionary<uint, int> m_dicItemPrice = null;

    bool alreadyReqHomePrice = false;//只请求一次,已经请求过，客户端不用再请求，如果价格有变动，服务器会主动下发商品价格

    private List<HomeItemData> m_lstHomeItemData = null;   //已经拥有的种子，幼崽
    public List<HomeItemData> HomeItemDataList
    {
        get
        {
            if (m_lstHomeItemData != null)
            {
                return m_lstHomeItemData;
            }
            Engine.Utility.Log.Error("Can Not Find Data !!!");
            return null;
        }
    }
    #endregion


    #region method

    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    void InitItemBagData(List<HomeItemData> list)
    {
        m_lstHomeItemData = list;
    }

    /// <summary>
    /// 清除家园大管家数据
    /// </summary>
    void CleanHomeTradeData()
    {
        CommonData.SafeClearDic(m_dicSellItem);
        CommonData.SafeClearDic(m_dicBuyItem);
        CommonData.SafeClearDic(m_dicCropLivestock);
        CommonData.SafeClearDic(m_dicItemPrice);
        CommonData.SafeClearList(m_lstHomeItemData);
        //m_dicSellItem.Clear();
        //m_dicBuyItem.Clear();
        //m_dicCropLivestock.Clear();//农业果实、家畜
        //m_dicItemPrice.Clear();
        //m_lstHomeItemData.Clear();   //已经拥有的种子，幼崽
        //m_dicSellItem = null;
        //m_dicBuyItem = null;
        //m_dicCropLivestock = null;//农业果实、家畜
        //m_dicItemPrice = null;
        //m_lstHomeItemData = null;   //已经拥有的种子，幼崽
        alreadyReqHomePrice = false;
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="tradeType"></param>
    /// <returns></returns>
    public List<uint> GetTabList(HomeTradePanel.ETradeType tradeType) 
    {
        List<HomeTradeDataBase> homeTradeDataBaseList = GameTableManager.Instance.GetTableList<HomeTradeDataBase>();

        if (tradeType == HomeTradePanel.ETradeType.ETradeType_Buy) //买
        {
           homeTradeDataBaseList = homeTradeDataBaseList.FindAll((data) => { return data.mallID == 1; });//1为买
        }

        if (tradeType == HomeTradePanel.ETradeType.ETradeType_Sell) //卖
        {
            homeTradeDataBaseList = homeTradeDataBaseList.FindAll((data) => { return data.mallID == 2; });//2为卖
        }

        List<uint> tabList = new List<uint>();
        m_dicTabName = new Dictionary<uint, string>();
        for (int i = 0; i < homeTradeDataBaseList.Count;i++)
        {
            if (!tabList.Exists((data) => { return data == homeTradeDataBaseList[i].itemType;}))
            {
                tabList.Add(homeTradeDataBaseList[i].itemType);
                m_dicTabName.Add(homeTradeDataBaseList[i].itemType, homeTradeDataBaseList[i].itemTypeName);
            }
        }

        return tabList;
    }


    public void InitTradeItemDic()
    {
        List<HomeTradeDataBase> homeTradeDataBaseList = GameTableManager.Instance.GetTableList<HomeTradeDataBase>();

        m_dicBuyItem = new Dictionary<uint, List<uint>>();
        m_dicSellItem = new Dictionary<uint, List<uint>>();

        //买
        List<HomeTradeDataBase> buyList = homeTradeDataBaseList.FindAll((data) => { return data.mallID == 1; });  //买
        for (int i = 0; i < buyList.Count;i++)
        {
            if (m_dicBuyItem.ContainsKey(buyList[i].itemType) == false)
            {
                List<HomeTradeDataBase> tempList = buyList.FindAll((data) => { return data.itemType == buyList[i].itemType;});
                List<uint> indexList = new List<uint>();
                for (int j = 0; j < tempList.Count;j++)
                {
                    indexList.Add(tempList[j].indexId);
                }

                m_dicBuyItem.Add(buyList[i].itemType, indexList);// 页签,  对应的list
            }
        }

        //卖
        List<HomeTradeDataBase> sellList = homeTradeDataBaseList.FindAll((data) => { return data.mallID == 2; }); //卖
        for (int i = 0; i < sellList.Count; i++)
        {
            if (m_dicSellItem.ContainsKey(sellList[i].itemType) == false)
            {
                List<HomeTradeDataBase> tempList = sellList.FindAll((data) => { return data.itemType == sellList[i].itemType; });
                List<uint> indexList = new List<uint>();
                for (int j = 0; j < tempList.Count; j++)
                {
                    indexList.Add(tempList[j].indexId);
                }

                m_dicSellItem.Add(sellList[i].itemType, indexList);
            }
        }
    }


    /// <summary>
    /// 种子或者幼崽的存量
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public uint GetBuyItemCount(uint id)
    {
        HomeItemData itemData = DataManager.Manager<HomeDataManager>().HomeItemDataList.Find((HomeItemData data) => data.base_id == id);
        if (itemData != null)
        {
            return itemData.num;
        }
        else
        {
            return 0;
        }
    }



    /// <summary>
    /// 获取出售作物dic ,key为类型
    /// </summary>
    /// <returns></returns>
    public Dictionary<uint, List<uint>> GetSellItemDic()
    {
        m_dicSellItem = new Dictionary<uint, List<uint>>();

        List<SeedAndCubDataBase> list = GameTableManager.Instance.GetTableList<SeedAndCubDataBase>();

        for (int i = 0; i < list.Count; i++)
        {
            if (m_dicSellItem.ContainsKey(list[i].type) == false)
            {
                List<uint> sellItemList = GetSellItemListByType(list[i].type);
                m_dicSellItem.Add(list[i].type, sellItemList);
            }
        }

        return m_dicSellItem;
    }

    /**
    @brief 获取type相同的物品IDList，如获取收获的农产品list 
    @param type
    */
    private List<uint> GetSellItemListByType(uint type)
    {
        List<SeedAndCubDataBase> list = GameTableManager.Instance.GetTableList<SeedAndCubDataBase>();

        List<uint> typeList = new List<uint>();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].type == type)
            {
                if (list[i].ProductOneID != 0)
                {
                    typeList.Add(list[i].ProductOneID);
                }

                if (list[i].ProductTwoID != 0)
                {
                    typeList.Add(list[i].ProductTwoID);
                }
            }
        }

        return typeList;
    }


    /// <summary>
    /// 获取购买作物dic ,key为类型
    /// </summary>
    /// <returns></returns>
    public Dictionary<uint, List<uint>> GetBuyItemDic()
    {
        Dictionary<uint, List<uint>> m_dicBuyItem = new Dictionary<uint, List<uint>>();

        List<SeedAndCubDataBase> list = GameTableManager.Instance.GetTableList<SeedAndCubDataBase>();

        for (int i = 0; i < list.Count; i++)
        {
            if (m_dicBuyItem.ContainsKey(list[i].type) == false)
            {
                List<uint> buyItemList = GetBuyItemListByType(list[i].type);
                m_dicBuyItem.Add(list[i].type, buyItemList);
            }
        }

        return m_dicBuyItem;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private List<uint> GetBuyItemListByType(uint type)
    {
        List<SeedAndCubDataBase> list = GameTableManager.Instance.GetTableList<SeedAndCubDataBase>();

        List<uint> typeList = new List<uint>();
        for (int i = 0; i < list.Count; i++)
        {
            if (type == list[i].type)
            {
                //typeList.Add(list[i].indexID);          //这里用索引ID，道具ID会删掉
                typeList.Add(list[i].itemID);             //暂时现这样做
            }
        }

        return typeList;
    }

    public Dictionary<uint, CropLivestock> GetCropLivestockDic()
    {
        m_dicCropLivestock = new Dictionary<uint, CropLivestock>();

        List<HomeLandRecycleDatabase> list = GameTableManager.Instance.GetTableList<HomeLandRecycleDatabase>();
        for (int i = 0; i < list.Count; i++)
        {
            ItemDataBase itemData = GameTableManager.Instance.GetTableItem<ItemDataBase>(list[i].wdID);

            CropLivestock cropLivestock = new CropLivestock();
            cropLivestock.m_nId = list[i].wdID;
            cropLivestock.m_strName = itemData.itemName;
            cropLivestock.m_nNum = (uint)DataManager.Manager<ItemManager>().GetItemNumByBaseId(list[i].wdID); //数量

            if (m_dicItemPrice != null)
            {
                if (m_dicItemPrice[list[i].wdID] >= list[i].recyclePrice)
                {
                    cropLivestock.m_bUp = true;  //价格上涨
                    cropLivestock.m_fPriceChangeRate = (float)(m_dicItemPrice[list[i].wdID] - list[i].recyclePrice) / list[i].recyclePrice;//价格变化率
                }
                else
                {
                    cropLivestock.m_bUp = false;
                    cropLivestock.m_fPriceChangeRate = (float)(list[i].recyclePrice - m_dicItemPrice[list[i].wdID]) / list[i].recyclePrice;
                }

                cropLivestock.m_nPrice = m_dicItemPrice[list[i].wdID];
            }
            cropLivestock.m_nIcon = itemData.itemIcon;
            cropLivestock.m_strDes = itemData.description;  //描述

            m_dicCropLivestock.Add(list[i].wdID, cropLivestock);
        }

        return m_dicCropLivestock;
    }
    #endregion


    #region net

    /// <summary>
    /// 购买家园物品
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    public void ReqBuySeedAndCub(uint id, uint num)
    {
        stBuyHomeUserCmd_C cmd = new stBuyHomeUserCmd_C();
        cmd.buy_base_id = id;
        cmd.buy_num = num;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 出售家园物品
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="num">数量</param>
    public void ReqSellSeedAndCub(uint id, uint num)
    {
        stSellHomeUserCmd_C cmd = new stSellHomeUserCmd_C();
        cmd.sell_base_id = id;
        cmd.sell_num = num;
        NetService.Instance.Send(cmd);
    }


    /// <summary>
    /// 商品价格 客户端请求服务器下发
    /// </summary>
    /// 
    
    public void ReqHomeTradePrice()
    {
        if (alreadyReqHomePrice == false)
        {
            stProductPriceHomeUserCmd_CS cmd = new stProductPriceHomeUserCmd_CS();
            NetService.Instance.Send(cmd);
        }
    }



    /// <summary>
    /// 商品价格 客户端请求服务器下发
    /// </summary>
    /// <param name="cmd"></param>
    public void OnHomeTradePrice(stProductPriceHomeUserCmd_CS cmd)
    {
        m_dicItemPrice = new Dictionary<uint, int>();
        for (int i = 0; i < cmd.product_base_id.Count; i++)
        {
            uint id = cmd.product_base_id[i];  //商品ID
            int price = cmd.price[i];         //对应的商品价格
            m_dicItemPrice.Add(id, price);
        }
        alreadyReqHomePrice = true;
    }

    /// <summary>
    /// 设置种子数量
    /// </summary>
    /// <param name="cmd"></param>
    public void OnHomeItem(stSetItemHomeUserCmd_S cmd)
    {
        uint itemId = cmd.base_id;  //种子或幼崽的ID
        uint itemNum = cmd.num;         //此ID的数量

        //if (m_dicCropLivestock.ContainsKey(itemId))
        //{
        //    m_dicCropLivestock[itemId].m_nNum = num;
        //}
        HomeItemData homeItemData = m_lstHomeItemData.Find((HomeItemData data) => { return data.base_id == itemId; });
        if (homeItemData != null)
        {
            homeItemData.num = itemNum;
        }
        else
        {
            m_lstHomeItemData.Add(new HomeItemData() { base_id = itemId, num = itemNum });
        }

        SeedAndCub seedAndCub = new SeedAndCub();
        seedAndCub.id = itemId;
        seedAndCub.num = itemNum;

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HOMELAND_BUYSEEDCUB, seedAndCub);
    }

    #endregion
}

