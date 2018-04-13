using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.Game.Item
 * 文件名：  Muhon
 * 版本号：  V1.0.0.0
 * 唯一标识：5ee07605-10d8-4c90-90fb-cdad4ebdaef8
 * 当前的用户域：wenjunhua.zqgame
 * 电子邮箱：mcking_wen@163.com
 * 创建时间：9/27/2016 10:04:54 AM
 * 描述：
 ************************************************************************************/
public class Muhon : BaseEquip
{
    #region property
    //如果服务端数据为null Level取值level
    private int level;
    //圣魂等级
    public int Level
    {
        get
        {
            return (null != ServerData) ? (int)GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_WeaponSoul_Level) : level;
        }
    }

    //是否激活
    public bool IsActive
    {
        get
        {
            return (AdditionAttrCount > 0) ? true : false;
        }
    }

    #region 圣魂养成
    /// <summary>
    /// 圣魂类型
    /// </summary>
    public uint Type
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.type : 0;
        }
    }

    /// <summary>
    ///星级别 
    /// </summary>
    public uint StartLevel
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.starLevel : 0;
        }
    }

    /// <summary>
    /// 是否达到最高星级
    /// </summary>
    public bool IsMaxStarLv
    {
        get
        {
            return (StartLevel >= EquipDefine.MUHON_MAX_STAR_LV);
        }
    }

    //融合消费
    public uint BlendCost
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.blendCost : 0;
        }
    }

    //融合需要物品
    public uint BlenNeedItemId
    {
        get
        {
           return (null != LocalGrowDataBase) ? LocalGrowDataBase.blendMaterialId : 0;
        }
    }

    //融合所需物品数量
    public uint BlenNeedItemNum
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.blendMaterialNum : 0; 
        }
    }

    //进化需要角色等级
    public uint EvolveNeedPlayerLv
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.evolveNeedPlayerLevel : 0;
        }
    }

    //进化花费钱币
    public uint EvolveCost
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.evolveCost : 0;
        }
    }

    //进化需要物品
    public uint EvolveNeedItemId
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.evolveMaterialId : 0;
        }
    }

    /// <summary>
    /// 进化消耗物品数量
    /// </summary>
    public uint EvolveNeedItemNum
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.evolveMaterialNum : 0;
        }
    }

    //上一级圣魂
    public Muhon Pre
    {
        get
        {
            return (null != LocalGrowDataBase && LocalGrowDataBase.preWeaponSoulId != 0) 
                ? new Muhon(LocalGrowDataBase.preWeaponSoulId) : null; 
        }
    }

    //下一级圣魂
    public Muhon Next
    {
        get
        {
            return (null != LocalGrowDataBase && LocalGrowDataBase.nextWeaponSoulId != 0)
                ? new Muhon(LocalGrowDataBase.nextWeaponSoulId) : null;
        }
    }

    //激活所需花费钱币
    public uint ActiveCost
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.activeCost : 0;
        }
    }

    //激活所需物品
    public uint ActiveNeedItemId
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.activeMaterialId : 0;
        }
    }

    /// <summary>
    /// 激活消耗数量
    /// </summary>
    public uint ActiveNeedItemNum
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.activeMaterialNum : 0;
        }
    }


    //消除花费钱币
    public uint RemoveCost
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.removeCost : 0;
        }
    }

    //消除所需物品
    public uint RemoveNeedItemId
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.removeMaterialId : 0;
        }
    }

    /// <summary>
    ///消除需要物品数量
    /// </summary>
    public uint RemoveNeedItemNum
    {
        get
        {
            return (null != LocalGrowDataBase) ? LocalGrowDataBase.removeMaterialNum : 0;
        }
    }

    /// <summary>
    /// 成长信息
    /// </summary>
    /// <returns></returns>
    public table.WeaponSoulGrowDataBase LocalGrowDataBase
    {
        get
        {
            return DataManager.Manager<ItemManager>().GetLocalDataBase<table.WeaponSoulGrowDataBase>(baseId);
        }
    }

    public int EvolveNeedMuhonNum
    {
        get
        {
            if (StartLevel == 0)
                return 1;
            return (int)StartLevel;
        }
    }

    public int MuhonAttrUpLimit
    {
        get
        {
            return (StartLevel == 0) ? 1 : (int)StartLevel;
        }
    }

    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public static int GetMuhonAttrUpLimit(uint baseId)
    {
        table.WeaponSoulGrowDataBase db 
            = DataManager.Manager<ItemManager>().GetLocalDataBase<table.WeaponSoulGrowDataBase>(baseId);
        int attrLimitNum = (db.starLevel == 0) ? 1 : (int)db.starLevel;
        return attrLimitNum;
    }

    public static int GetMuhonAttrUpLimit(Muhon muhon)
    {
        int attrLimitNum = (muhon.StartLevel == 0) ? 1 : (int)muhon.StartLevel;
        return attrLimitNum;
    }

    /// <summary>
    /// 获取星级
    /// </summary>
    /// <param name="baseId"></param>
    /// <returns></returns>
    public static uint GetMuhonStarLevel(uint baseId)
    {
        table.WeaponSoulGrowDataBase db 
            = DataManager.Manager<ItemManager>().GetLocalDataBase<table.WeaponSoulGrowDataBase>(baseId);
        return (null != db) ? db.starLevel : 0;
    }

    public static string GetMuhonLv(uint baseID)
    {
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseID);
        return GetMuhonLv(baseItem);
    }

    public static string GetMuhonLv(BaseItem baseItem)
    {
        if (!baseItem.IsMuhon)
        {
            return string.Empty;
        }

        int level = (null != baseItem.ServerData) 
            ? (int)baseItem.GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_WeaponSoul_Level) : 1;
        table.WeaponSoulUpgradeDataBase upgrade =
            DataManager.Manager<ItemManager>().GetLocalDataBase<table.WeaponSoulUpgradeDataBase>(baseItem.BaseId, level);

        if (null == upgrade)
        {
            return string.Empty;
        }

        int max = (int)upgrade.maxLevel;
        if (level < max)
        {
            return level.ToString();
        }else
        {
            return "满";
        }
    }

    #endregion

    #region 圣魂升级

    /// <summary>
    ///升级信息
    /// </summary>
    public table.WeaponSoulUpgradeDataBase LocalUpgradeDataBase
    {
        get
        {
            return DataManager.Manager<ItemManager>().GetLocalDataBase<table.WeaponSoulUpgradeDataBase>(baseId, Level);
        }
    }

    //最大等级
    public uint MaxLv
    {
        get
        {
            return (null != LocalUpgradeDataBase) ? LocalUpgradeDataBase.maxLevel : 0;
        }
    }

    //是否为最大等级
    public bool IsMaxLv
    {
        get
        {
            return (null != LocalUpgradeDataBase && (MaxLv == Level)) ? true : false;
        }
    }

    //穿戴等级
    public uint WearLv
    {
        get
        {
            return (null != LocalUpgradeDataBase) ? LocalUpgradeDataBase.apparelLevel : 0;
        }
    }

    //升级经验
    public uint UpgradeExp
    {
        get
        {
            return (null != LocalUpgradeDataBase) ? LocalUpgradeDataBase.upgradeExperience : 0;
        }
    }

    //当前圣魂经验
    public uint Exp
    {
        get
        {
            return (null != ServerData) ? GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_WeaponSoul_Exp) : 0;
        }
    }

    //经验百分比
    public float ExpPercentage
    {
        get
        {
            return (IsMaxLv) ? 1f : (float)Exp / UpgradeExp;
        }
    }

    //经验百分比format
    public string ExpPercentageFormat
    {
        get
        {
            return string.Format("{0}%", (ExpPercentage * 100).ToString("f2"));
        }
    }

    //物理攻击
    public uint PhyAttack
    {
        get
        {
            return (null != LocalUpgradeDataBase) ? LocalUpgradeDataBase.phyAttack : 0;
        }
    }

    //物理防御
    public uint PhyDef
    {
        get
        {
            return (null != LocalUpgradeDataBase) ? LocalUpgradeDataBase.phyDef : 0;
        }
    }

    //法术攻击
    public uint MagicAttack
    {
        get
        {
            return (null != LocalUpgradeDataBase) ? LocalUpgradeDataBase.magicAttack : 0;
        }
    }

    //法术防御
    public uint MagicDef
    {
        get
        {
            return (null != LocalUpgradeDataBase) ? LocalUpgradeDataBase.magicDef : 0;
        }
    }

    #endregion

    #endregion

    #region structmethod

    protected Muhon()
    {

    }

    public Muhon (uint baseId,GameCmd.ItemSerialize serverdata = null,int level= 1) 
        : base(baseId,serverdata)
    {
        this.level = level;
    }
    #endregion

    #region Op

    public bool IsMatchEvolve(Muhon muhon,bool depthCheck = true)
    {

        if (null != muhon && QWThisID != muhon.QWThisID
            //&& Type == muhon.Type       //同类型
            && StartLevel == muhon.StartLevel   //同星级
            //&& (!depthCheck || (Level == muhon.Level          //同等级
            //&& muhon.AdditionAttrCount > 0))
            )    //激活
        {
            return true;
        }
        return false;
    }

    public bool IsMatchEvolve(uint qwThisId, bool depthCheck = true)
    {
        Muhon muhon = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(qwThisId);
        return IsMatchEvolve(muhon, depthCheck);
    }


    public bool IsMatchBlend(Muhon muhon, bool depthCheck = true)
    {
        if (null != muhon && QWThisID != muhon.QWThisID
            && Type == muhon.Type       //同类型
            && StartLevel == muhon.StartLevel   //同星级
            && (!depthCheck || muhon.AdditionAttrCount > 0)
            )    //激活
        {
            return true;
        }
        return false;
    }

    public bool IsMatchBlend(uint qwThisId, bool depthCheck = true)
    {
        Muhon muhon = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId<Muhon>(qwThisId);
        return IsMatchBlend(muhon, depthCheck);
    }

    /// <summary>
    /// 更新等级（有服务端数据的无效,适用于本地数据）
    /// </summary>
    /// <param name="level"></param>
    public void UpdateLv(int level)
    {
        if (null != ServerData)
            return;
        this.level = level;
    }
   
    #endregion
}