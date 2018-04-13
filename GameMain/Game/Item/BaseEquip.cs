using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.Game.Item
 * 文件名：  BaseEquip
 * 版本号：  V1.0.0.0
 * 唯一标识：5ee07605-10d8-4c90-90fb-cdad4ebdaef8
 * 当前的用户域：wenjunhua.zqgame
 * 电子邮箱：mcking_wen@163.com
 * 创建时间：9/27/2016 10:04:54 AM
 * 描述：
 ************************************************************************************/
public class BaseEquip : BaseItem
{
    #region property
    private GameCmd.EquipPos ePos = GameCmd.EquipPos.EquipPos_None;
    //装备部位（装备特有）
    public GameCmd.EquipPos EPos
    {
        get
        {
            return ePos;
            
        }
    }

    private GameCmd.EquipType eType = GameCmd.EquipType.EquipType_None;
    /// <summary>
    /// 装备位置
    /// </summary>
    public GameCmd.EquipType EType
    {
        get
        {
            return eType;
        }
    }

    private bool m_bIsWear = false;
    //是否该装备已经装备
    public bool IsWear
    {
        get
        {
            return m_bIsWear;
        }
    }

    private EquipDefine.EquipGradeType m_equipGrade = EquipDefine.EquipGradeType.None;
    //装备档次
    public EquipDefine.EquipGradeType EquipGrade
    {
        get
        {
            return m_equipGrade;
        }
    }


    private table.EquipDataBase m_localDataBase = null;
    //本地表格数据
    public table.EquipDataBase LocalDataBase
    {
        get
        {
            return m_localDataBase;
        }
    }

    private uint m_power = 0;
    /// <summary>
    /// 战力
    /// </summary>
    public uint Power
    {
        get
        {
            return m_power;
        }
    }

    private bool m_bCanRefine = false;
    /// <summary>
    /// 是否可以参与精炼
    /// </summary>
    public bool CanRefine
    {
        get
        {
            return m_bCanRefine;
        }
    }

    private bool m_bCanSplit = false;
    /// <summary>
    /// 是否可以参与分解
    /// </summary>
    public bool CanSplit
    {
        get
        {
            return m_bCanSplit;
        }
    }

    private bool m_bCanCompound = false;
    /// <summary>
    /// 是否可以参与合成
    /// </summary>
    public bool CanCompound
    {
        get
        {
            return m_bCanCompound;
        }
    }

    private bool m_bCompoundMaskEnable = false;
    public bool CompoundMaskEnable
    {
        get
        {
            return m_bCompoundMaskEnable;
        }
    }
    #endregion

    #region structmethod
    protected BaseEquip()
        : base()
    {
        
    }

    public BaseEquip(uint baseId, GameCmd.ItemSerialize serverdata)
        : base(baseId, serverdata)
    {
       
    }

    protected override void OnUpdateAttr(GameCmd.eItemAttribute attr, uint value)
    {
        base.OnUpdateAttr(attr, value);
        BaseEquipUpdateAttr();
    }

    protected override void OnUpdateData()
    {
        base.OnUpdateData();
        if (!IsEquip)
        {
            return;
        }
        m_localDataBase = DataManager.Manager<ItemManager>().GetLocalDataBase<table.EquipDataBase>(baseId);
        if (null == m_localDataBase)
        {
            Engine.Utility.Log.Error("BaseEquip->OnUpdateData m_localDataBase null baseId:{0}",baseId);
            return;
        }
        m_bCompoundMaskEnable = EquipDefine.IsEquipGrowMaskMatchType(LocalDataBase.growMask, EquipDefine.EquipGrowType.Compound);

        m_bCanCompound = EquipDefine.IsEquipGrowMaskMatchType(LocalDataBase.growMask, EquipDefine.EquipGrowType.Compound)
                && AdditionAttrCount > 0;

        m_bCanSplit = EquipDefine.IsEquipGrowMaskMatchType(LocalDataBase.growMask, EquipDefine.EquipGrowType.Split)
                && AdditionAttrCount > 0;

        m_bCanRefine = EquipDefine.IsEquipGrowMaskMatchType(LocalDataBase.growMask, EquipDefine.EquipGrowType.Refine);

        if (null == BaseData)
        {
            Engine.Utility.Log.Error("BaseEquip->OnUpdateData BaseData null baseId:{0}", baseId);
            return;
        }
        eType = (GameCmd.EquipType)BaseData.subType;

        if (BaseData.grade > (int)EquipDefine.EquipGradeType.None && BaseData.grade < (int)EquipDefine.EquipGradeType.Max)
        {
            m_equipGrade = (EquipDefine.EquipGradeType)BaseData.grade;
        }

        BaseEquipUpdateAttr();

        BaseEquipUpdateLocation();
    }

    private void BaseEquipUpdateAttr()
    {
        if (!IsEquip)
        {
            return;
        }
        if (null != ServerData)
        {
            m_power = GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_FightPower);
        }
        else
        {
            m_power = LocalDataBase.fightPower;
        }
    }

    private void BaseEquipUpdateLocation()
    {
        if (!IsEquip)
        {
            return;
        }

        if (ServerData != null
                && PackType == GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP)
        {
            ePos = (GameCmd.EquipPos)((int)Position.y);
        }
        else
        {
            GameCmd.EquipPos[] equipPos = EquipDefine.GetEquipPosByEquipType((GameCmd.EquipType)BaseData.subType);
            if (null != equipPos && equipPos.Length > 0)
            {
                ePos = equipPos[0];
            }
        }

        m_bIsWear = DataManager.Manager<EquipManager>().IsWearEquip(QWThisID);
    }

    protected override void OnUpdateLocation(uint loc)
    {
        base.OnUpdateLocation(loc);
        if (!IsEquip)
        {
            return;
        }
        BaseEquipUpdateLocation();
    }
    #endregion
}