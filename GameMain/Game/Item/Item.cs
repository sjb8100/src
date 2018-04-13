using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.Game.Item
 * 文件名：  Item
 * 版本号：  V1.0.0.0
 * 唯一标识：5ee07605-10d8-4c90-90fb-cdad4ebdaef8
 * 当前的用户域：wenjunhua.zqgame
 * 电子邮箱：mcking_wen@163.com
 * 创建时间：9/27/2016 10:04:54 AM
 * 描述：
 ************************************************************************************/
public class Item : BaseItem
{
    public Item(uint baseId, GameCmd.ItemSerialize serverdata = null)
        : base(baseId,serverdata)
    {

    }
}