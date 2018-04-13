using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.Game.Item
 * 文件名：  Gem
 * 版本号：  V1.0.0.0
 * 唯一标识：5ee07605-10d8-4c90-90fb-cdad4ebdaef8
 * 当前的用户域：wenjunhua.zqgame
 * 电子邮箱：mcking_wen@163.com
 * 创建时间：9/27/2016 10:04:54 AM
 * 描述：
 ************************************************************************************/
public class Gem : BaseItem
{
    #region propery

    #region 宝石合成
    /// <summary>
    /// 宝石合成数据
    /// </summary>
    private table.GemComposeDataBase localComposeDataBase;
    public table.GemComposeDataBase LocalComposeDataBase
    {
        get
        {
            if (localComposeDataBase == null)
            {
               localComposeDataBase = GameTableManager.Instance.GetTableItem<table.GemComposeDataBase>(baseId); 
            }
            return localComposeDataBase;        
        }
    }

    /// <summary>
    ///下一级宝石
    /// </summary>
    public Gem Next
    {
        get
        {
            return (null != LocalComposeDataBase) ? new Gem(LocalComposeDataBase.nextGemBaseId) : null;
        }
    }

    //合成需要数量
    public uint ComposeNum
    {
        get
        {
            return (null != LocalComposeDataBase) ? LocalComposeDataBase.needNum : 0;
        }
    }

    //合成花费钱币
    public uint Cost
    {
        get
        {
            return (null != LocalComposeDataBase) ? LocalComposeDataBase.costConey : 0;
        }
    }

    //宝石类型
    public GameCmd.GemType Type
    {
        get
        {
            return (null != LocalComposeDataBase) ? (GameCmd.GemType)LocalComposeDataBase.gemType : GameCmd.GemType.GemType_None;
        }
    }

    //宝石名称
    public string GemName
    {
        get
        {
            return (null != LocalComposeDataBase) ? LocalComposeDataBase.name : "";
        }
    }

    #endregion

    #region 宝石属性
    /// <summary>
    /// 宝石属性
    /// </summary>
    public table.GemPropertyDataBase LocalPropertyDataBase
    {
        get
        {
            return DataManager.Manager<ItemManager>().GetLocalDataBase<table.GemPropertyDataBase>(baseId);
        }
    }

    /// <summary>
    /// 宝石属性描述
    /// </summary>
    public string AttrDes
    {
        get
        {
            return (null != LocalPropertyDataBase) ? DataManager.Manager<EquipManager>().GetAttrDes(LocalPropertyDataBase.effectId) : "";
        }
    }

    //效果id
    public uint EffectId
    {
        get
        {
            return (null != LocalPropertyDataBase) ? LocalPropertyDataBase.effectId : 0;
        }
    }

    public uint FightPowerNum 
    {
        get
        {
            return (null != LocalPropertyDataBase) ? LocalPropertyDataBase.fightPower : 0;
        }
    }
    #endregion

    /// <summary>
    /// 是否为最高级别
    /// </summary>
    public bool Max
    {
        get
        {
            return (null != LocalComposeDataBase && LocalComposeDataBase.nextGemBaseId == 0) ? true :
               false;
        }
    }

    #endregion
    #region structmethod

    protected Gem()
    {

    }
    public Gem(uint baseId, GameCmd.ItemSerialize serverdata = null)
        : base(baseId,serverdata)
    {

    }
    #endregion
}