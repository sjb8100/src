using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;

partial class Protocol
{
    /// <summary>
    /// 获取物品列表请求
    /// </summary>
    public void GetItemListReq()
    {

    }

    /// <summary>
    /// 清除物品响应
    /// </summary>
    /// <param name="id"></param>
    [Execute]
    public void OnClearItemRes(stDeleteItemPropertyUserCmd cmd)
    {
        DataManager.Manager<ItemManager>().OnUpdateItemDataNum(cmd.qwThisID, 0);
    }

    [Execute]
    public void OnRemoveItemRes( stRemoveItemPropertyUserCmd_CS cmd)
    {
        DataManager.Manager<ItemManager>().OnUpdateItemDataNum(cmd.qwThisID, 0);
    }
    /// <summary>
    /// 更新物品数量响应
    /// </summary>
    [Execute]
    public void OnRfreshItemCountRes(stRefreshCountItemPropertyUserCmd_S cmd)
    {
        DataManager.Manager<ItemManager>().OnUpdateItemDataNum(cmd.qwThisID, cmd.dwNum, cmd.action);
    }

    /// <summary>
    /// 修理装备
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnRefreshDurable(stRefreshDurabilityPropertyUserCmd_S cmd)
    {
        DataManager.Manager<ItemManager>().OnUpdateItemDurable(cmd.dwThisID, cmd.dwDur, cmd.dwMaxDur);
    }

    /// <summary>
    /// 批量添加用户道具数据
    /// </summary>
    [Execute]
    public void OnAddListItemRes(stAddItemListPropertyUserCmd_S cmd)
    {
        DataManager.Manager<ItemManager>().OnAddListItemData(cmd.itemList);
    }

    /// <summary>
    /// 添加单个物品
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnAddItemRes(stAddItemPropertyUserCmd_S cmd)
    {
        DataManager.Manager<ItemManager>().OnAddItemData(cmd.obj,action:cmd.action);
    }

    /// <summary>
    /// 拾取地图上物品请求
    /// </summary>
    /// <param name="qwThisId"></param>
    public void PickUpItemReq(uint qwThisId)
    {
        stPickUpItemPropertyUserCmd_C cmd = new stPickUpItemPropertyUserCmd_C()
        {
            qwThisID = qwThisId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 捡起物品响应
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnPickUpItemRes(GameCmd.stPickUpItemReturnPropertyUserCmd_S cmd)
    {
        if (cmd.state == 0)
        {
            Engine.Utility.Log.Error("PICK UP ERROR ID:{0}", cmd.qwThisID);
        }
        else
        {
            if (!DataManager.Manager<TeamDataManager>().IsJoinTeam)
            {
                return;
            }
            table.ItemDataBase db = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(cmd.baseid);
            if (db != null)
            {
                if (db.IsBroad)
                {
                    CHATTYPE chatType = CHATTYPE.CHAT_TEAM;
                    string name = "[" + db.itemName + "]";
                    string endTxt = ChatDataManager.GetItemHrefString(name, MainPlayerHelper.GetPlayerID(), cmd.qwThisID, db.quality);
                    string info = DataManager.Manager<TextManager>().GetLocalFormatText(LocalTextType.Team_Recruit_woshiqu, endTxt);
                    stWildChannelCommonChatUserCmd_CS wild = new stWildChannelCommonChatUserCmd_CS();
                    wild.byChatType = chatType;
                    wild.szInfo = info;
                    wild.dwOPDes = 0;
                    wild.timestamp = (uint)DateTimeHelper.Instance.Now;
                    NetService.Instance.Send(wild);
                }
            }
        }
  
    
    
    }

    [Execute]
    public void OnUseItemReturnCdInfo(GameCmd.stUseItemReturnCDPropertyUserCmd_S cmd)
    {
        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs != null)
	    {
		    Client.IControllerHelper ch = cs.GetControllerHelper();
            if (ch != null)
                ch.SetMedicineCDInfo(cmd.baseid,cmd.cd_info);
	    }

        DataManager.Manager<ItemManager>().AddItemCDDataToDic(cmd.baseid, cmd.cd_info);
    }


    public void UseItem(uint targetID, uint targetType,uint thisID,uint num)
    {
        GameCmd.stUseItemPropertyUserCmd_CS cmd = new GameCmd.stUseItemPropertyUserCmd_CS();
        cmd.qwTargetID = targetID;
        cmd.qwThisID = thisID;
        cmd.dwNumber = num;
        cmd.qwTargetType = targetType;
        SendCmd(cmd);
    }

    /// <summary>
    /// 使用物品
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnUseItemProperty(GameCmd.stUseItemPropertyUserCmd_CS msg)
    {
        DataManager.Manager<ItemManager>().OnItemUse(msg);
    }

    /// <summary>
    /// 服务器下发物品使用次数
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnServerItemUseGet(GameCmd.stGetItemUseTimesDataUserCmd_S msg)
    {
        DataManager.Manager<ItemManager>().OnServerItemUseGet(msg);
    }

    [Execute]
    public void OnAddItemTip(stAddItemTipPropertyUserCmd_S cmd)
    {
        //统一获取
        //if (cmd.byActionType == (int)AddItemAction.AddItemAction_Task_Add)
        //{
        //    table.ItemDataBase itemdb = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(cmd.qwBaseID);
        //    if (itemdb != null)
        //    {
        //        TipsManager.Instance.ShowTips(string.Format("获得{0}",itemdb.itemName));
        //    }
        //}
    }
}

   