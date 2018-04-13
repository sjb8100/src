/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Net.Protocol
 * 创建人：  wenjunhua.zqgame
 * 文件名：  Compose_Protocol
 * 版本号：  V1.0.0.0
 * 创建时间：11/4/2016 10:32:04 AM
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
    /// 物品合成请求
    /// </summary>
    /// <param name="itemId">物品id</param>
    /// <param name="composeType">合成类型</param>
    public void ItemComposeReq(uint itemId,uint composeType,GameCmd.EquipPos pos = EquipPos.EquipPos_None,uint gemPosIndex = 0)
    {
        stComposeItemWorkUserCmd_CS cmd = new stComposeItemWorkUserCmd_CS()
        {
            composetype = composeType,
            itemid = itemId,
            equip_pos =pos,
            gem_pos_index = gemPosIndex,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 物品合成响应
    /// </summary>
    [Execute]
    public void OnItemComposeRes(stComposeItemWorkUserCmd_CS msg)
    {
        DataManager.Manager<ComposeManager>().OnCompose(msg.itemid, msg.composenum, msg.composetype);
    }
}