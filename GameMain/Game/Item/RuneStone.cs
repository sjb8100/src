using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.Game.Item
 * 文件名：  RuneStone
 * 版本号：  V1.0.0.0
 * 唯一标识：5ee07605-10d8-4c90-90fb-cdad4ebdaef8
 * 当前的用户域：wenjunhua.zqgame
 * 电子邮箱：mcking_wen@163.com
 * 创建时间：9/27/2016 10:04:54 AM
 * 描述：
 ************************************************************************************/
public class RuneStone : BaseItem
{
    #region propery
    public table.RunestoneDataBase LocalDataBase
    {
        get
        {
            return DataManager.Manager<ItemManager>().GetLocalDataBase<table.RunestoneDataBase>(baseId);
        }
    }

    //符石名称
    public string RunStoneName
    {
        get
        {
            return (null != LocalDataBase) ? LocalDataBase.name : "";
        }
    }

    //保护概率
    public uint ProtectProp
    {
        get
        {
            return (null != LocalDataBase) ? LocalDataBase.protectProb : 0;
        }
    }


    //符石档次（注：与附加属性档次相同）
    public uint Grade
    {
        get
        {
            return IsBaseReady ? BaseData.grade : 0;
        }
    }

    //消除花费钱币
    public uint RemoveCost
    {
        get
        {
            return (null != LocalDataBase) ? LocalDataBase.removeCost : 0;
        }
    }

    //提升花费钱币
    public uint PromoteCost
    {
        get
        {
            return (null != LocalDataBase) ? LocalDataBase.promoteCost : 0;
        }
    }

    #endregion
    #region structmethod

    protected RuneStone()
    {

    }

    public RuneStone(uint baseId, GameCmd.ItemSerialize serverdata = null)
        : base(baseId, serverdata)
    {

    }
    #endregion
}