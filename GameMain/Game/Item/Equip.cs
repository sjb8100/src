using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.Game.Item
 * 文件名：  Equip
 * 版本号：  V1.0.0.0
 * 唯一标识：5ee07605-10d8-4c90-90fb-cdad4ebdaef8
 * 当前的用户域：wenjunhua.zqgame
 * 电子邮箱：mcking_wen@163.com
 * 创建时间：9/27/2016 10:04:54 AM
 * 描述：
 ************************************************************************************/
public class Equip : BaseEquip
{
    #region property

    #region 精炼
    //精炼等级（如果服务端数据为null RefineLv取refineLv）
    private uint refineLv = 0;
    /// <summary>
    /// 精炼等级
    /// </summary>
    public uint RefineLv
    {
        get
        {
            return (null != ServerData) ? GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_RefineLevel) : refineLv;
        }
    }

    //精炼本地数据
    public table.EquipRefineDataBase LocalRefineDataBase
    {
        get
        {
            return DataManager.Manager<ItemManager>().GetLocalDataBase<table.EquipRefineDataBase>(baseId,(int)RefineLv);
        }
    }

    //下一级精炼本地数据
    public table.EquipRefineDataBase NextRefineDataBase
    {
        get
        {
            return (RefineLv + 1<= EquipManager.REFINE_MAX_LEVEL) ? DataManager.Manager<ItemManager>().GetLocalDataBase<table.EquipRefineDataBase>(baseId, (int)(RefineLv + 1)) : null;
        }
    }

   
    //精炼花费钱币
    public uint RefineCost
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costConey : 0;
        }
    }


    //消耗材料1
    public uint MaterialId1
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costItem1 : 0;;
        }
    }
    
    //消耗材料1数量
    public uint MaterilaNum1
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costNum1 : 0;
        }
    }

    //消耗材料2
    public uint MaterialId2
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costItem2 : 0;
        }
    }

    //消耗材料2数量
    public uint MaterilaNum2
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costNum2 : 0;
        }
    }

    //消耗材料3
    public uint MaterialId3
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costItem3 : 0;
        }
    }

    //消耗材料3数量
    public uint MaterilaNum3
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costNum3 : 0;
        }
    }

    //消耗材料4
    public uint MaterialId4
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costItem4 : 0;
        }
    }

    //消耗材料4数量
    public uint MaterilaNum4
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costNum4 : 0;
        }
    }

    //消耗材料5
    public uint MaterialId5
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costItem5 : 0;
        }
    }

    /// <summary>
    /// 消耗材料5数量
    /// </summary>
    public uint MaterilaNum5
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.costNum5 : 0; ;
        }
    }

    //辅助材料1
    public uint RefAssistItemId1
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.assistId1 : 0; 
        }
    }

    //辅助材料1数量
    public uint RefAssistItemNum1
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.assistNum1 : 0; 
        }
    }

    //辅助材料2
    public uint RefAssisItemId2
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.assistId2 : 0; 
        }
    }

    //辅助材料2数量
    public uint RefAssistemItemNum2
    {
        get
        {
            return (null != LocalRefineDataBase) ? LocalRefineDataBase.assistNum2 : 0;
        }
    }

    //是否精炼最高等级
    public bool IsRefineMax
    {
        get
        {
            return (null != LocalRefineDataBase && (RefineLv == EquipManager.REFINE_MAX_LEVEL)) ? true : false;
        }
    }

    #endregion

    #region 合成
    //合成数据
    public table.EquipComposeDataBase LocalComposeDataBase
    {
        get
        {
            return DataManager.Manager<ItemManager>().GetLocalDataBase<table.EquipComposeDataBase>(baseId);
        }

    }

    //低一级装备
    public Equip Pre
    {
        get
        {
            return null;
        }
    }

    //高一级装备
    public Equip Next
    {
        get
        {
            return null; 
        }
    }

    //合成花费钱币
    public uint CompCost
    {
        get
        {
            return (null != LocalComposeDataBase) ? LocalComposeDataBase.costConey : 0;
        }
    }

    //合成辅助道具1
    public uint CompAssistItem
    {
        get
        {
            return (null != LocalComposeDataBase) ? LocalComposeDataBase.assistId : 0;
        }
    }

    //合成辅助道具1数量
    public uint CompAssistItemNum
    {
        get
        {
            return (null != LocalComposeDataBase) ? LocalComposeDataBase.assistNum : 0;
        }
    }

    #endregion

    #region 分解

    //本地分解数据
    public table.EquipSplitDataBase LocalSplitDataBase
    {
        get
        {
            return (IsBaseReady) ? DataManager.Manager<ItemManager>().GetLocalDataBase<table.EquipSplitDataBase>(BaseId) : null;
        }
    }

    //是否可分解
    public bool CanSplit
    {
        get
        {
            return (null != LocalSplitDataBase) ? true : false;
        }
    }

    //是否可以进行加工
    public bool CanProccess
    {
        get
        {
            return (AdditionAttrCount != 0);
        }
    }

    //分解获得物品
    public uint SplitItemId
    {
        get
        {
            return (CanSplit) ? LocalSplitDataBase.splitGetItem : 0;
        }
    }

    //分解获得物品数量
    public uint SplitItemNum
    {
        get
        {
            return (CanSplit) ? LocalSplitDataBase.splitGetItemNum : 0;
        }
    }

    //分解获得金币数量
    public uint SplitGoldNum
    {
        get
        {
            return (CanSplit) ? LocalSplitDataBase.splitGetMoney : 0;
        }
    }
    #endregion

    /// <summary>
    /// 最大耐久
    /// </summary>
    public uint MaxDur
    {
        get
        {
            return (null != LocalDataBase) ? LocalDataBase.maxDurable : 0;
        }
    }
    /// <summary>
    /// tips显示值
    /// </summary>
    public int MaxDisplayDur
    {
        get
        {
            return ItemDefine.GetDisplayDruable(MaxDur);
        }
    }
    /// <summary>
    /// 当前耐久
    /// </summary>
    public uint CurDur
    {
        get
        {
            return (null != ServerData) ?  GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Dur) : MaxDur;
        }
    }

    /// <summary>
    /// tips显示值
    /// </summary>
    public int CurDisplayDur
    {
        get
        {
            return ItemDefine.GetDisplayDruable(CurDur);
        }
    }
    public bool HaveDurable
    {
        get
        {
            return CurDur > 0;
        }

    }  

    #endregion

    #region structmethod

    protected Equip()
    {

    }

    public Equip(uint baseId, GameCmd.ItemSerialize serverdata = null, uint refineLv = 1)
        : base(baseId, serverdata)
    {
        this.refineLv = refineLv;
    }
    #endregion
}