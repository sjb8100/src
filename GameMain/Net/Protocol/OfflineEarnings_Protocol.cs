/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Net.Protocol
 * 创建人：  wenjunhua.zqgame
 * 文件名：  OfflineEarnings_Protocol
 * 版本号：  V1.0.0.0
 * 创建时间：5/27/2017 11:24:22 AM
 * 描述：
 ************************************************************************************/

using Client;
using Common;
using Engine;
using GameCmd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
using Engine.Utility;


partial class Protocol
{
    /// <summary>
    /// 发送领取离线奖励请求
    /// </summary>
    public void GetOfflineRewardReq(bool doubleReward)
    {
        stGetOffRewardPropertyUserCmd_CS cmd = new stGetOffRewardPropertyUserCmd_CS()
        {
            use_noble = doubleReward,
        };
        SendCmd(cmd);
    }

    [Execute]
    public void OnGetOfflineRewardRes(stGetOffRewardPropertyUserCmd_CS msg)
    {
        DataManager.Manager<OfflineManager>().OnGetOfflineReward();
    }


    /// <summary>
    /// 服务器下发离线奖励
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnSendOfflineRewardRes(stOfflineRewardPropertyUserCmd_S msg)
    {

        DataManager.Manager<OfflineManager>().OnSendOfflineReward(msg.item_data, msg.offline_time, msg.offline_xs);
    }
}

