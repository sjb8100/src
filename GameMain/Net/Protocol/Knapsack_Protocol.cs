using System;
using System.Collections.Generic;
using Common;
using GameCmd;
using Client;
partial class  Protocol
{
    /// <summary>
    /// 请求整理背包
    /// </summary>
    public void TidyRequest(PACKAGETYPE pType)
    {
        stRequestSortItemPropertyUserCmd_CS cmd = new stRequestSortItemPropertyUserCmd_CS();
        cmd.packtype = pType;
        SendCmd(cmd);
    }

    /// <summary>
    /// 整理完成
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnTidyRes(stRequestSortItemPropertyUserCmd_CS msg)
    {
        DataManager.Manager<KnapsackManager>().OnTidy(msg.packtype);
    }

    /// <summary>
    /// 整理背包响应
    /// </summary>
    [Execute]
    public void OnBatchRefreshPositon(stRefreshItemPosListPropertyUserCmd_S cmd)
    {
        DataManager.Manager<KnapsackManager>().OnBatchRefreshItemPosition(cmd.data);
    }

    [Execute]
    public void OnRefreshPosition(stRefreshLocationItemPropertyUserCmd_S cmd)
    {
        DataManager.Manager<ItemManager>().OnUpdateItemPos(cmd.qwThisID, cmd.loc);
    }

    ///// <summary>
    ///// 请求修复所有装备要
    ///// </summary>
    ///// <param name="qwThisID">修理的道具唯一ID ;0表示修理全身装备</param>
    //public void RepairItemReq(uint qwThisID = 0)
    //{
    //    GameCmd.stRepairItemPropertyUserCmd_C cmd = new GameCmd.stRepairItemPropertyUserCmd_C();
    //    cmd.qwThisID = qwThisID;
    //    SendCmd(cmd);
    //}

    /// <summary>
    /// 请求解锁背包
    /// </summary>
    /// <param name="num">需要解锁的背包数量</param>
    public void UnlockKnapsackGridReq(PACKAGETYPE pType, uint num)
    {
        stUnlockGridPropertyUserCmd_CS cmd = new stUnlockGridPropertyUserCmd_CS();
        cmd.num = num;
        cmd.type = (uint)pType;
        SendCmd(cmd);
    }

    /// <summary>
    /// 解锁背包响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnUnlockKnapsackGridRes(stUnlockGridPropertyUserCmd_CS msg)
    {
        DataManager.Manager<KnapsackManager>().OnUnlockKnapsackGrid(msg.num, msg.type);
    }

    /// <summary>
    ///同步背包解锁格子
    /// </summary>
    [Execute]
    public void OnSycnUnlockKanpsackGridRes(stSetUnlockedGridNumPropertyUserCmd_S msg)
    {
        DataManager.Manager<KnapsackManager>().OnSycnUnlockKnapsackGrid(msg.unlock);
    }

    /// <summary>
    /// 购买皇令解锁仓库
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnUnlockWareHouseRes(stSetStorePackNumPropertyUserCmd msg)
    {
        DataManager.Manager<KnapsackManager>().OnUnlockWareHouse(msg.dwNum);
    }


    /// <summary>
    /// 合并道具请求
    /// </summary>
    /// <param name="qwSourceID"></param>
    /// <param name="qwTargetID"></param>
    /// <param name="num"></param>
    public void UnionItemReq(uint qwSourceID,uint qwTargetID,uint num)
    {
        stUnionItemPropertyUserCmd_C cmd = new stUnionItemPropertyUserCmd_C()
        {
            qwSrcThisID = qwSourceID,
            qwDstThisID = qwTargetID,
            dwNum = num,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 删除物品请求
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="force"></param>
    public void RemoveItemReq(uint qwThisId,uint force)
    {
        stRemoveItemPropertyUserCmd_CS cmd = new stRemoveItemPropertyUserCmd_CS()
        {
            force = force,
            qwThisID = qwThisId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 交换物品
    /// </summary>
    /// <param name="srcThisID">源id</param>
    /// <param name="desThisID">目标id</param>
    /// <param name="srcLocation">原位置</param>
    /// <param name="desLocation">目标位置</param>
    public void SwapItemReq(uint srcThisID,uint desThisID,GameCmd.tItemLocation srcLocation,GameCmd.tItemLocation desLocation)
    {
        stSwapItemPropertyUserCmd_CS cmd = new stSwapItemPropertyUserCmd_CS()
        {
            srcThisID = srcThisID,
            srcloc = srcLocation,
            dstThisID = desThisID,
            dstloc = desLocation,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 交换道具位置
    /// </summary>
    [Execute]
    public void OnSwapItemRes(stSwapItemPropertyUserCmd_CS cmd)
    {
        DataManager.Manager<ItemManager>().OnSwapItem(cmd.srcThisID, cmd.dstThisID, cmd.srcloc, cmd.dstloc);
    }

    /// <summary>
    /// 存取钱币请求
    /// </summary>
    /// <param name="num">数量</param>
    /// <param name="type">1：存 2：取</param>
    
    public void OperStoreMoneyReq(uint num,uint type)
    {
        GameCmd.stUserOperStoreMoneyPropertyUserCmd_CS cmd = new stUserOperStoreMoneyPropertyUserCmd_CS()
        {
            money = num,
            type = type,
        };
        SendCmd(cmd);

    }

    /// <summary>
    /// 存取钱响应
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnOperStoreMoneyRes(stUserOperStoreMoneyPropertyUserCmd_CS msg)
    {
        DataManager.Manager<KnapsackManager>().OnOperStoreMoney(msg.money, msg.type);
    }
}