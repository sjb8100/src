using System.Collections;
using GameCmd;
using Common;

partial class Protocol
{
    /// <summary>
    /// 返回的寄售列表
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResponItemListConsignment(stResponItemListConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<ConsignmentManager>().OnResponItemListConsignment(cmd);
    }

    /// <summary>
    /// 寄售成功
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResponSellConsignItem(stSellItemConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<ConsignmentManager>().OnResponSellConsignItem(cmd);
    }

    /// <summary>
    /// 获取寄售物品信息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResponItemInfoConsignment(stResponItemInfoConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<ConsignmentManager>().OnResponItemInfoConsignment(cmd);
    }
    [Execute]
    public void OnExc(stItemPriceConsignmentUserCmd_CS cmd) 
    {
        DataManager.Manager<ConsignmentManager>().OnRecievePrePrice(cmd.item_baseid,cmd.price);
    }
    /// <summary>
    /// 购买成功
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResponBuyConsignItem(stBuyItemConsignmentUserCmd_CS cmd)
    {
        DataManager.Manager<ConsignmentManager>().OnResponBuyConsignItem(cmd);
    }

    /// <summary>
    /// 收到个人寄售信息列表
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResponItemSellInfo(stItemSellInfoConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<ConsignmentManager>().OnResponItemSellInfo(cmd);
    }
    /// <summary>
    /// 获取收藏列表
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnRecieveAllStarItems(stStarItemListConsignmentUserCmd_S cmd) 
    {
        DataManager.Manager<ConsignmentManager>().OnRecieveAllStarItemDatas(cmd);
    }
    [Execute]
    public void OnRecieveAllShowItems(stGreatItemInfoConsignmentUserCmd_S cmd) 
    {
        DataManager.Manager<ConsignmentManager>().OnRecieveAllGreatItemDatas(cmd);
    }
    [Execute]
    public void OnRecieveAllStarIds(stStarIDListConsignmentUserCmd_S cmd) 
    {
        DataManager.Manager<ConsignmentManager>().AllStarMarkedIDs = cmd.star_market_id_list;
    }
    [Execute]
    public void OnStarSuccess(stStarConsignmentUserCmd_CS cmd) 
    {
        DataManager.Manager<ConsignmentManager>().OnChangeStarMarkedIds(cmd.market_id,cmd.star);
    }
    /// <summary>
    /// 改变总的寄售金额
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResponItemSellMoney(stItemSellMoneyLogConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<ConsignmentManager>().OnResponItemSellMoney(cmd);
    }

    /// <summary>
    /// 下架成功
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResponCancelConsignItem(stCancalItemConsignmentUserCmd_CS cmd)
    {
        DataManager.Manager<ConsignmentManager>().OnResponCancelConsignItem(cmd);
    }

    /// <summary>
    /// 物品被人买走了
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResponRemoveConsignItem(stRemoveItemInfoConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<ConsignmentManager>().OnResponRemoveConsignItem(cmd);
    }

    /// <summary>
    /// 寄售日志
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResponItemSellLog(stItemSellLogConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<ConsignmentManager>().OnResponItemSellLog(cmd);
    }
    /// <summary>
    /// 服务器下发的寄售价格列表
    /// </summary>
    [Execute]
    public void OnRecieveSaleList(stResPriceInfoConsignmentUserCmd_S cmd) 
    {
        DataManager.Manager<ConsignmentManager>().OnRecieveSaleList(cmd);
    
    }

    [Execute]
    public void OnBuyItem(stSellInformConsignmentUserCmd_S cmd)
    {
        table.ItemDataBase item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(cmd.item_baseid);
        if (item != null)
        {
            string txt = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Talk_System_jishouhangchushouwupin, item.itemName);
            ChatDataManager.SendToChatSystem(txt);
        }
    }
}
