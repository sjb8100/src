using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using GameCmd;
using table;

partial class Protocol
{
    /// <summary>
    /// 请求下架商品列表
    /// </summary>
    public void GetUnShelveItemListReq()
    {
        stUserReqOutItemPropertyUserCmd_CS cmd = new stUserReqOutItemPropertyUserCmd_CS();
        SendCmd(cmd);
    }

    /// <summary>
    /// 获取下架商品响应
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void UnSelveItemListRes(stUserReqOutItemPropertyUserCmd_CS cmd)
    {
        DataManager.Manager<MallManager>().OnGetUnShelveItemListRes(cmd.index);
    }

    public void SellItemListReq(List<uint> sellList)
    {
        GameCmd.stUserCommonSellItemPropertyUserCmd_CS cmd = new GameCmd.stUserCommonSellItemPropertyUserCmd_CS();
        cmd.thisid.AddRange(sellList);
        SendCmd(cmd);
    }

    [Execute]
    public void OnSellItemListRes(stUserCommonSellItemPropertyUserCmd_CS msg)
    {
        DataManager.Manager<MallManager>().OnSellRes(msg.thisid);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mallItemId"></param>
    /// <param name="storeType"></param>
    /// <param name="num"></param>
    /// <param name="pos"></param>
    public void PurchaseMallItemReq(uint mallItemId,uint storeType,uint num,uint pos = 0)
    {
        stUserCommonBuyItemPropertyUserCmd_CS cmd = new stUserCommonBuyItemPropertyUserCmd_CS();
        cmd.index = mallItemId;
        cmd.type = storeType;
        cmd.num = num;
        cmd.pos = pos;
        SendCmd(cmd);
    }

    [Execute]
    public void OnPurchaseMallItem(stUserCommonBuyItemPropertyUserCmd_CS msg)
    {
        DataManager.Manager<MallManager>().OnPurchaseMallItemRes(msg.index, msg.type, msg.num, msg.ret);
    }

    /// <summary>
    /// 刷新商城数据
    /// </summary>
    /// <param name="mallType">商城类型</param>
    public void RefreshMall(uint mallType)
    {
        stUserReqRefDynaStorePropertyUserCmd_C cmd = new stUserReqRefDynaStorePropertyUserCmd_C()
        {
            store_id = mallType,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 刷新商城响应
    /// </summary>
    [Execute]
    public void OnRefreshMallRes(stUserSendDynaStorePropertyUserCmd_S msg)
    {
        DataManager.Manager<MallManager>().OnRefreshMall(msg.type,msg.all_store_info);
    }

    /// <summary>
    /// 服务器下发商城购买数据
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnRecieveServerItemPurchaseNumDataRes(stSendBuyStorePropertyUserCmd_S msg)
    {
        DataManager.Manager<MallManager>().OnRecieveServerItemPurchaseNumData(msg.all_buy_info);
    }
}