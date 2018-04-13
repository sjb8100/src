/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Net.Protocol
 * 创建人：  wenjunhua.zqgame
 * 文件名：  Recharge_Protocol
 * 版本号：  V1.0.0.0
 * 创建时间：12/5/2017 4:42:51 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;

partial class Protocol
{
    /// <summary>
    /// 请求充值订单信息
    /// </summary>
    /// <param name="rechargeID"></param>
    public void RechargeOrderReq(uint rechargeID,uint num = 1)
    {
        stCreatePlatOrderPropertyUserCmd_C cmd = new stCreatePlatOrderPropertyUserCmd_C()
        {
           goodid = rechargeID,
           goodnum = num,
           data = new PlatBaseData(),
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 充值订单获取响应
    /// </summary>
    [Execute]
    public void OnRechargeOrderRes(stCreatePlatOrderPropertyUserCmd_S msg)
    {
        DataManager.Manager<RechargeManager>().OnRequestRechargeOrder(msg);
    }

    /// <summary>
    /// 充值结果响应
    /// </summary>
    [Execute]
    public void OnRechargeRuslt(stRechargeResultPropertyUserCmd_S msg)
    {
        bool success = (msg.result == 0);
        DataManager.Manager<RechargeManager>().OnServerBackPayResult(success, msg.goodid,msg.goodnum);
    }

    /// <summary>
    /// 查询充值结果
    /// </summary>
    public void QueryRechargeResultReq()
    {
        stRechargeReqResultPropertyUserCmd_C cmd = new stRechargeReqResultPropertyUserCmd_C();
        SendCmd(cmd);
    }
}