//*************************************************************************
//	创建日期:	2016/11/3 11:09:36
//	文件名称:	SaleMoney_Protocol
//   创 建 人:   zhudianyu	
//	版权所有:	中青宝
//	说    明:	文钱交易协议
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Client;
using table;
using System.Collections;
using UnityEngine;

partial class Protocol
{
    [Execute]
    public void OnRecieveSaleInfo(stSMinBMaxOrderConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<SaleMoneyDataManager>().OnRecieveSaleInfo(cmd);
    }

    [Execute]
    public void OnRecieveAccoutInfo(stUserAccountConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<SaleMoneyDataManager>().OnRecieveAccoutInfo(cmd);
    }
    [Execute]
    public void OnRecentOrderInfo(stSuccessBuyLogConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<SaleMoneyDataManager>().OnRecentOrderInfo(cmd);
    }
    [Execute]
    public void OnReceiveRecordInfo(stDealRecordConsignmentUserCmd_S cmd)
    {
        DataManager.Manager<SaleMoneyDataManager>().OnReceiveRecordInfo(cmd);
    }
    [Execute]
    public void OnReceiveCancleOrder(stCancelOrderConsignmentUserCmd_CS cmd)
    {
        DataManager.Manager<SaleMoneyDataManager>().OnReceiveCancleOrder(cmd);
    }

    [Execute]
    public void OnBuyOrderFinish(stBuyMoneyConsignmentUserCmd_CS cmd)
    {
        if (cmd.ret == 0)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Trading_Currency_xiadanchenggong);
            // TipsManager.Instance.ShowTipsById( 117501 );
        }

    }
    [Execute]
    public void OnSellOrderFinish(stSellMoneyConsignmentUserCmd_CS cmd)
    {
        if (cmd.ret == 0)
        {
            TipsManager.Instance.ShowTips(LocalTextType.Trading_Currency_xiadanchenggong);
            // TipsManager.Instance.ShowTipsById( 117501 );
        }
    }

    [Execute]
    public void OnGetAccountMoney(stExtractMoneyConsignmentUserCmd_CS cmd)
    {
        DataManager.Manager<SaleMoneyDataManager>().OnGetAccountMoney(cmd);
    }


    /// <summary>
    /// 接收到服务器下发的文钱交易结果的消息飘字
    /// </summary>
    [Execute]
    public void OnReceiveRemindText(stSendInfoReminderChatUserCmd_S cmd)
    {
        uint text_id = cmd.id;
        List<string> param = cmd.szInfo;
        table.LangTextDataBase langText = GameTableManager.Instance.GetTableItem<table.LangTextDataBase>(text_id);
        object[] objs = null;
        if (param.Count != 0)
        {
            objs = new object[param.Count];
            for (int i = 0; i < param.Count; i++)
            {
                objs[i] = param[i];
            }
        }

        if (langText != null)
        {
            DataManager.Manager<ChatDataManager>().SeverSendToChatSystem(cmd, langText, objs);

        }

    }


}
