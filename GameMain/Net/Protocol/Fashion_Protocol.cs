/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Net.Protocol
 * 创建人：  wenjunhua.zqgame
 * 文件名：  Fashion_Protocol
 * 版本号：  V1.0.0.0
 * 创建时间：11/30/2016 9:27:39 AM
 * 描述：  时装消息
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Client;
using Common;


partial class Protocol
{
    /// <summary>
    /// 服务器推送时装数据（已拥有）
    /// </summary>
    /// <param name="datas"></param>
    [Execute]
    public void OnReceiveAllSuitData(stSendAllSuitPropertyUserCmd_S msg)
    {
        DataManager.Manager<SuitDataManager>().OnReceiveAllSuitData(msg);
    }

    [Execute]
    public void OnSendNineSuitMsg(stSendSuitToNinePropertyUserCmd_S cmd)
    {
        DataManager.Manager<SuitDataManager>().OnSendNineSuitMsg(cmd);
    }
}