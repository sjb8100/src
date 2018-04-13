using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.Game.Item
 * 文件名：  BaseItem
 * 版本号：  V1.0.0.0
 * 唯一标识：5ee07605-10d8-4c90-90fb-cdad4ebdaef8
 * 当前的用户域：wenjunhua.zqgame
 * 电子邮箱：mcking_wen@163.com
 * 创建时间：9/27/2016 10:04:54 AM
 * 描述：
 ************************************************************************************/
public class BaseItem
{
    #region define

    public class BaseItemIDCompare : IComparer<uint>
    {
        #region static
        private static BaseItemIDCompare m_compare = null;
        public static BaseItemIDCompare Create()
        {
            if (null == m_compare)
            {
                m_compare = new BaseItemIDCompare();
            }
            return m_compare;
        }

        private BaseItemIDCompare()
        {

        }
        #endregion

        #region ICompare
        public int Compare(uint leftID, uint rightID)
        {
            return BaseItemCompare.CompareBaseItem(leftID, rightID);
        }
        #endregion
    }

    //物品排序规则
    public class BaseItemCompare : IComparer<BaseItem>
    {
        #region static
        public static int CompareBaseItem(BaseItem left,BaseItem right)
        {
            if (null == left || null == right)
            {
                return 0;
            }

            bool isMatchJobLeft = DataManager.IsMatchPalyerJob(left.UseRole);
            bool isMatchJobRight = DataManager.IsMatchPalyerJob(right.UseRole);

            if (isMatchJobLeft && left.BaseType == GameCmd.ItemBaseType.ItemBaseType_Equip)
            {
                if (isMatchJobRight && right.BaseType == GameCmd.ItemBaseType.ItemBaseType_Equip)
                {
                    if (left.UseRole != right.UseRole)
                    {
                        //1.按照职业ID升序顺序排列
                        return left.UseRole - right.UseRole;
                    }
                    else if (left.SubType != right.SubType)
                    {
                        //2.同职业按部位ID升序顺序排列
                        return left.SubType - right.SubType;
                    }
                    else if (left.Grade != right.Grade)
                    {
                        //3.同职业、部位按照档次降序顺序排列
                        return right.Grade - left.Grade;
                    }
                    else if (left.IsBind != right.IsBind)
                    {
                        if (left.IsBind)
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }else
                {
                    return -1;
                }
                
            }
            else if (isMatchJobRight && right.BaseType == GameCmd.ItemBaseType.ItemBaseType_Equip)
            {
                return 1;
            }else if (left.SortID != right.SortID)
            {
                return left.SortID - right.SortID;
            }
            return 0;
        }

        public static int CompareBaseItem(uint leftId,uint rightId)
        {
            ItemManager imgr = DataManager.Manager<ItemManager>();
            BaseItem left = imgr.GetBaseItemByQwThisId(leftId);
            BaseItem right = imgr.GetBaseItemByQwThisId(rightId);
            
            return CompareBaseItem(left, right);
        }
        #endregion

        #region ICompare
        public int Compare(BaseItem left, BaseItem right)
        {
            return CompareBaseItem(left, right);
        }
        #endregion
    }
    #endregion

    #region property
    protected uint baseId;
    public uint BaseId
    {
        get
        {
            return baseId;
        }
    }

    private table.ItemDataBase m_baseData;
    public table.ItemDataBase BaseData
    {
        get
        {
            return m_baseData;
        }
    }

    public uint OverlayNum
    {
        get
        {
            if (null != BaseData)
            {
                return BaseData.overlayNum;
            }
            return 0;
        }
        
    }

    private bool m_bBaseReady = false;
    /// <summary>
    /// base数据是否准备好
    /// </summary>
    public bool IsBaseReady
    {
        get
        {
            return (null != BaseData) ? true : false;
        }
    }

    private int m_grade = 0;
    public int Grade
    {
        get
        {
            return m_grade;
        }
    }

    private int m_useRole = 0;
    public int UseRole
    {
        get
        {
            return m_useRole;
        }
    }

    private int m_subType = 0;
    public int SubType
    {
        get
        {
            return m_subType;
        }
    }

    private int m_sortID = 0;
    private int SortID
    {
        get
        {
            return m_sortID;
        }
    }

    private string m_strBorderIcon = "";
    /// <summary>
    /// 物品外框（装备，圣魂按附加属性条目，其他物品按品质）
    /// </summary>
    public string BorderIcon
    {
        get
        {
            return m_strBorderIcon;
        }
    }

    private TextureID m_TipsTipIcon = TextureID.Baishougu;
    /// <summary>
    /// tip顶部icon
    /// </summary>
    public TextureID TipsTopIcon
    {
        get
        {
            return m_TipsTipIcon;
        }
    }

    /// <summary>
    /// 名称NGUI颜色（装备，圣魂按附加属性条目，其他物品按品质）
    /// </summary>
    /// <param name="tips"></param>
    /// <returns></returns>
    public string GetNameNGUIColor(bool tips = false)
    {
        return ColorManager.GetNGUIColorByQuality(QualityType,tips);
    }

    private ItemDefine.ItemQualityType m_QulityType = ItemDefine.ItemQualityType.White;
    /// <summary>
    /// 品质
    /// </summary>
    public ItemDefine.ItemQualityType QualityType
    {
        get
        {
            return m_QulityType;
        }
    }

    /// <summary>
    /// 使用等级
    /// </summary>
    public uint UseLv
    {
        get
        {
            return  BaseData.useLevel;
        }
    }

    /// <summary>
    /// 是否需要跳转
    /// </summary>
    public bool NeedJump
    {
        get
        {
            return (!string.IsNullOrEmpty(BaseData.itemJump));
        }
    }

    /// <summary>
    /// 跳转ID
    /// </summary>
    public List<uint> JumpWayIDs
    {
        get
        {
            List<uint> jumpWayIds = new List<uint>();
            if (NeedJump)
            {
                string[] jumpArrays = BaseData.itemJump.Split(new char[] { '_' });
                if (null != jumpArrays && jumpArrays.Length > 0)
                {
                    uint tempID = 0;
                    for(int i = 0; i < jumpArrays.Length;i++)
                    {
                        if (uint.TryParse(jumpArrays[i].Trim(),out tempID)
                            && !jumpWayIds.Contains(tempID))
                        {
                            jumpWayIds.Add(tempID);
                        }
                    }
                }
            }
            return jumpWayIds;
        }
    }

    /// <summary>
    /// 是否有每日使用限制
    /// </summary>
    public bool HasDailyUseLimit
    {
        get
        {
            table.ItemDataBase itemDB = BaseData;
            if (null != itemDB)
            {
                if (itemDB.useTypeId != 0)
                {
                    return true;
                }
                else if (itemDB.maxUseTimes != 0)
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 使用类别是否可用
    /// </summary>
    public bool IsUseTypeEnable
    {
        get
        {
            return (UseTypeId != 0);
        }
    }

    /// <summary>
    /// 使用类型id
    /// </summary>
    public uint UseTypeId
    {
        get
        {
            return (null != BaseData) ? BaseData.useTypeId : 0;
        }
    }

    /// <summary>
    /// 单日最大使用最大次数(0:无限制 >0：当前数量)
    /// </summary>
    public int MaxUseNum
    {
        get
        {
            return (int)((null != BaseData) ? BaseData.maxUseTimes : 0);
        }
    }

    /// <summary>
    /// 单日可使用数量(0:无限制 >0：当前数量)
    /// 如果使用类型UseTypeId不为0则使用类型数量，反之使用表格最大数量MaxUseNum
    /// </summary>
    public int DailyUseNum
    {
        get
        {
            int num = 0;
            if (IsUseTypeEnable)
            {
                table.ItemUseTypeDataBase udb
                    = GameTableManager.Instance.GetTableItem<table.ItemUseTypeDataBase>(UseTypeId);
                if (null != udb)
                {
                    num = (int)udb.dayUseTimes;
                }
            }else
            {
                num = MaxUseNum;
            }
            return num;
        }
    }

    /// <summary>
    /// 物品图标
    /// </summary>
    public string Icon
    {
        get
        {
            return (null != BaseData) ? BaseData.itemIcon : "";
        }
    }

    /// <summary>
    /// 掉落图标
    /// </summary>
    public uint DropIcon
    {
        get
        {
            return (null != BaseData) ? BaseData.dropIcon : 0;
        }
    }

    //描述
    public string Des
    {
        get
        {
            return (null != BaseData) ? BaseData.description : "";
        }
    }
    //描述
    public string DesNoColor
    {
        get
        {
            string des = Des;
            TextManager.ClearStrNGUIColor(ref des);
            return des;
        }
    }

    /// <summary>
    /// 是否为装备
    /// </summary>
    public bool IsEquip
    {
        get
        {
            return (BaseType == GameCmd.ItemBaseType.ItemBaseType_Equip) ? true : false;
        }
    }


    //是否可以堆叠
    public bool CanPile
    {
        get
        {
            return !IsEquip;
        }
    }

    /// <summary>
    /// 最大堆叠数
    /// </summary>
    public uint MaxPileNum 
    {
        get 
        {
            return BaseData.overlayNum;
        }
    }

    /// <summary>
    /// 是否为可以锻造装备
    /// </summary>
    /// <returns></returns>
    public bool IsForgingEquip
    {
        get
        {
            return IsEquip && !(IsMuhon ||IsOffice);
        }
    }

    /// <summary>
    /// 是否为圣魂
    /// </summary>
    public bool IsMuhon
    {
        get
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Equip)
                && (BaseData.subType == (uint)GameCmd.EquipType.EquipType_SoulOne)) ? true : false;
        }
    }
    public bool IsTreasureMap 
    {
        get 
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Consumption)
                 && (BaseData.subType == (uint)ItemDefine.ItemConsumerSubType.TreasureMap)) ? true : false;
        }
    }
   /// <summary>
   /// 是不是珍稀物品
   /// </summary>
    public bool IsTreasure 
    {
        get 
        {
            bool isTreasure = false;

            if (BaseData.great == 1)
            {
                isTreasure = true;
            }
            else 
            {
                if (IsEquip)
                {
                    if (GetAdditiveAttr().Count >= 4)
                    {
                        isTreasure = true;
                    }
                }
                else if (IsPetBall)
                {
                    if (ContainAttribute(GameCmd.eItemAttribute.Item_Attribute_Pet_Great))
                    {
                        isTreasure = true;
                    }
                }
            }
          
            return isTreasure;
        }
    }
    public bool IsPetBall
    {
        get
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Consumption)
                 && (BaseData.subType == (uint)ItemDefine.ItemConsumerSubType.SoulBead)) ? true : false;
        }
    }
    public bool IsRideBall
    {
        get
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Consumption)
                 && (BaseData.subType == (uint)ItemDefine.ItemConsumerSubType.MountsBead)) ? true : false;
        }
    }

    /// <summary>
    ///是否为官印
    /// </summary>
    public bool IsOffice
    {
        get
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Equip) &&
                (BaseData.subType == (uint)GameCmd.EquipType.EquipType_Office)) ? true : false;
        }
    }

    /// <summary>
    /// 是否为碎片
    /// </summary>
    public bool IsChips
    {
        get
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Material) &&
                (BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.RideChips || BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.PetChips)) ? true : false;
        }
    }
    public bool IsSkillBook
    {
        get
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Material) &&
                (BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.SkillBook)) ? true : false;
        }
    }

    /// <summary>
    /// 是否为宝石
    /// </summary>
    public bool IsGem
    {
        get
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Material) &&
                (BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.Gem)) ? true : false; ;
        }
    }

    public bool IsCollectWord 
    {
        get
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Material) &&
                (BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.CollectWord)) ? true : false; ;
        }
    }

    /// <summary>
    /// 是否为符石
    /// </summary>
    public bool IsRuneStone
    {
        get
        {
            return ((BaseType == GameCmd.ItemBaseType.ItemBaseType_Material) &&
                (BaseData.subType == (uint)ItemDefine.ItemMaterialSubType.Runestone)) ? true : false; ;
        }
    }

    private GameCmd.ItemBaseType m_baseType = GameCmd.ItemBaseType.ItemBaseType_None;
    /// <summary>
    /// 物品大类型
    /// </summary>
    public GameCmd.ItemBaseType BaseType
    {
        get
        {
            return m_baseType;
        }
    }

    /// <summary>
    /// 表格名称
    /// </summary>
    public string LocalName
    {
        get
        {
            return BaseData.itemName;
        }
    }
    private string m_strName = "";
    //显示名称（装备包括精炼等级和颜色）
    public string Name
    {
        get
        {
            return m_strName;
        }
    }

    private string m_strNameForTips = "";
    //显示名称（装备包括精炼等级和颜色） tips专用
    public string NameForTips
    {
        get
        {
            return m_strNameForTips;
        }
    }

   
    #endregion

    #region structmethod
    public BaseItem() 
    {

    }

    public BaseItem(uint baseId, GameCmd.ItemSerialize serverdata = null) 
    {
        UpdateData(baseId, serverdata);
    }
    #endregion 

    #region ServerData

    public uint BindMask
    {
        get
        {
            return (null != serverData) ? GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Bind) : 0;
        }
    }

    private bool m_bIsBind = false;
    //是否绑定
    public bool IsBind
    {
        get
        {
            return m_bIsBind;
        }
    }

    private bool m_bCanSell2NPC = false;
    //是否可出售给npc
    public bool CanSell2NPC
    {
        get
        {
            return m_bCanSell2NPC;
        }
    }

    private bool m_bCanStore2WareHouse = false;
    //是否可存到仓库
    public bool CanStore2WareHouse
    {
        get
        {
            return m_bCanStore2WareHouse;
        }
    }

    private bool m_bCanAuction = false;
    //是否可进入拍卖行交易
    public bool CanAuction
    {
        get
        {
            return m_bCanAuction;
        }
    }
    private GameCmd.ItemSerialize serverData;
    public GameCmd.ItemSerialize ServerData
    {
        get
        {
            return serverData;
        }
    }

    private GameCmd.PACKAGETYPE m_packType = GameCmd.PACKAGETYPE.PACKAGETYPE_NONE;
    /// <summary>
    /// 背包类型
    /// </summary>
    public GameCmd.PACKAGETYPE PackType
    {
        get
        {
            return m_packType;
            //return (GameCmd.PACKAGETYPE)((null != location) ? location.PackType : GameCmd.PACKAGETYPE.PACKAGETYPE_NONE);
        }
    }

    /// <summary>
    /// 背包中位置
    /// </summary>
    public UnityEngine.Vector2 Position
    {
        get
        {
            return location.Position;
        }
    }

    /// <summary>
    /// 唯一Id
    /// </summary>
    public uint QWThisID
    {
        get
        {
            return (null != serverData) ? serverData.qwThisID : 0;
        }
    }

    /// <summary>
    /// 创建ID
    /// </summary>
    public ulong CreateId
    {
        get
        {
            return (null != serverData) ? serverData.createid : 0;
        }
    }


    /// <summary>
    /// 当前持有数量
    /// </summary>
    public uint Num
    {
        get
        {
            return (null != serverData) ? serverData.dwNum : 0;
        }
        set
        {
            if (null != serverData)
            {
                serverData.dwNum = (uint)UnityEngine.Mathf.Max(0, value);
            }
        }
    }

    public bool UpdateData(uint baseId,GameCmd.ItemSerialize serverdata=null)
    {
        bool success = false;
        if (null != serverdata)
        {
            this.serverData = serverdata;
            StructServerItemAttribute();
            if (serverdata.pos != null)
            {
                UpdateServerLocation(serverdata.pos.loc);
            }
            this.baseId = serverdata.dwObjectID;
        }else
        {
            this.baseId = baseId;
        }

        m_baseData = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(baseId);
        if (null != BaseData)
            success = true;
        
        m_bBaseReady = null != m_baseData;
        if (!m_bBaseReady)
            return false;
        //tab attr
        m_sortID = (int)BaseData.sortID;
        m_subType = (int)BaseData.subType;
        m_useRole = (int)BaseData.useRole;
        m_grade = (int)BaseData.grade;
        m_baseType = (GameCmd.ItemBaseType)BaseData.baseType;

        //serverattr
        UpdateServerProperty();
        OnUpdateData();
        return success;
    }

    private void UpdateServerProperty()
    {
        uint bindMask = BindMask;
        m_bCanAuction = ItemDefine.isBindMaskEnable(GameCmd.enumItemBindType.BindItemType_Shop, bindMask);
        m_bCanSell2NPC = ItemDefine.isBindMaskEnable(GameCmd.enumItemBindType.BindItemType_Sell, bindMask);
        m_bCanStore2WareHouse = ItemDefine.isBindMaskEnable(GameCmd.enumItemBindType.BindItemType_Store, bindMask);
        m_bIsBind = bindMask != 0 && ItemDefine.IsBind(bindMask);

        m_QulityType = ItemDefine.ItemQualityType.White;
        int quality = 0;
        if (IsEquip)
        {
            quality = AdditionAttrCount;
        }
        else
        {
            quality = (int)BaseData.quality;
        }
        if (quality > (int)ItemDefine.ItemQualityType.White
            && quality < (int)ItemDefine.ItemQualityType.Max)
        {
            m_QulityType = (ItemDefine.ItemQualityType)quality;
        }

        m_strBorderIcon = ItemDefine.GetItemBorderIcon(QualityType);

        m_strName = GetNameNGUIColor() + LocalName;
        if (IsForgingEquip)
        {
            uint refineLevel = GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_RefineLevel);
            if (refineLevel > 0)
                m_strName = m_strName + "+" + refineLevel;
        }

        m_strNameForTips = GetNameNGUIColor(true) + LocalName;
        if (IsForgingEquip)
        {
            uint refineLevel = GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_RefineLevel);
            if (refineLevel > 0)
                m_strNameForTips = m_strNameForTips + "+" + refineLevel;
        }

        m_TipsTipIcon = ItemDefine.GetItemTipsTopIcon(QualityType);
        UpdateLocation();
    }

    protected virtual void OnUpdateData()
    {

    }


    protected virtual void OnUpdateAttr(GameCmd.eItemAttribute attr, uint value)
    {

    }

    protected virtual void OnUpdateLocation(uint loc)
    {

    }

    private void UpdateLocation()
    {
        m_packType = (GameCmd.PACKAGETYPE)((null != location) ? location.PackType : GameCmd.PACKAGETYPE.PACKAGETYPE_NONE);
    }
   

    /// <summary>
    /// 本地位置数据类
    /// </summary>
    private ItemDefine.ItemLocation location = null;
    /// <summary>
    /// 服务端位置
    /// </summary>
    public uint ServerLocaltion
    {
        get
        {
            return (null != serverData) ? serverData.pos.loc : 0;
        }
    }
    /// <summary>
    /// 更新物品在背包中的位置
    /// </summary>
    /// <param name="loc"></param>
    public void UpdateServerLocation(uint loc)
    {
        ServerData.pos.loc = loc;
        location = ItemDefine.TransformServerLocation2Local(loc);
        UpdateLocation();
        OnUpdateLocation(loc);
    }



    /// <summary>
    /// 更新属性值
    /// </summary>
    /// <param name="attr">属性</param>
    /// <param name="value">值</param>
    public void UpdateItemAttribute(GameCmd.eItemAttribute attr, uint value)
    {
        uint attrInt = (uint)attr;
        bool exist = itemServerAttributeDic.ContainsKey(attrInt);
        if (exist)
        {
            itemServerAttributeDic[attrInt] = value;
            for (int i = 0; i < ServerData.numbers.Count; i++)
            {
                if (ServerData.numbers[i].id == attrInt)
                {
                    ServerData.numbers[i].value = value;
                }
            }
        }
        else
        {
            itemServerAttributeDic.Add(attrInt, value);
            ServerData.numbers.Add(new GameCmd.PairNumber()
            {
                id = attrInt,
                value = value,
            });
        }

        OnUpdateAttr(attr, value);
    }

    /// <summary>
    /// 获取物品属性值
    /// </summary>
    /// <param name="attr">属性</param>
    /// <param name="value">值</param>
    /// <returns>是否成功获取</returns>
    public bool TryGetItemAttribute(GameCmd.eItemAttribute attr, out uint value)
    {
        uint attrInt = (uint)attr;
        bool exist = itemServerAttributeDic.ContainsKey(attrInt);
        value = (exist) ? itemServerAttributeDic[attrInt] : 0;
        return exist;
    }

    /// <summary>
    /// 是否包含属性
    /// </summary>
    /// <param name="attr"></param>
    /// <returns></returns>
    public bool ContainAttribute(GameCmd.eItemAttribute attr)
    {
        return itemServerAttributeDic.ContainsKey((uint)attr);
    }

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="attr"></param>
    /// <returns></returns>
    public uint GetItemAttribute(GameCmd.eItemAttribute attr)
    {
        return (itemServerAttributeDic.ContainsKey((uint)attr)) ? itemServerAttributeDic[(uint)attr] : 0;
    }

    /// <summary>
    /// 获取附加属性
    /// </summary>
    /// <returns></returns>
    public List<GameCmd.PairNumber> GetAdditiveAttr()
    {
        List<GameCmd.PairNumber> result = new List<GameCmd.PairNumber>();
        if (null != serverData && null != serverData.numbers)
        {
            for (int i=0;i<ServerData.numbers.Count;i++)
            {
                GameCmd.PairNumber pair = ServerData.numbers[i];
                if (!ItemDefine.IsAdditiveAttr(pair.id))
                    continue;
                result.Add(pair);
            }
        }
        return result;
    }
    /// <summary>
    /// 获取附加属性id
    /// </summary>
    /// <returns></returns>
    public List<uint> GetAdditiveAttrIds()
    {
        List<uint> result = new List<uint>();
        if (null != serverData && null != serverData.numbers)
        {
            foreach (GameCmd.PairNumber pair in ServerData.numbers)
            {
                if (!ItemDefine.IsAdditiveAttr(pair.id))
                    continue;
                result.Add(pair.id);
            }
        }
        return result;
    }

    //附加属性数量
    public int AdditionAttrCount
    {
        get
        {
            List<GameCmd.PairNumber> addtiveAttr = GetAdditiveAttr();
            return (null != addtiveAttr) ? addtiveAttr.Count : 0;
        }
    }

    public EquipDefine.EquipColor EquipColor 
    {
        get
        {
            return (EquipDefine.EquipColor)AdditionAttrCount;
        }
    }

    private Dictionary<uint, uint> itemServerAttributeDic = new Dictionary<uint, uint>();
    /// <summary>
    /// 构建本地属性数据结构
    /// </summary>
    public void StructServerItemAttribute()
    {
        itemServerAttributeDic.Clear();
        if (null != ServerData && null == ServerData.numbers || ServerData.numbers.Count == 0)
            return;
        foreach (GameCmd.PairNumber pair in ServerData.numbers)
        {
            if (itemServerAttributeDic.ContainsKey(pair.id))
            {
                Engine.Utility.Log.Error("ItemData-> StructLocalItemAttribute Error,exist attrId = {0}!", pair.id);
                continue;
            }
            itemServerAttributeDic.Add(pair.id, pair.value);
        }
    }

    #endregion

    #region static method

    ///// <summary>
    ///// 创建baseItem
    ///// </summary>
    ///// <param name="baseId"></param>
    ///// <param name="serverdata"></param>
    ///// <returns></returns>
    //public static T Create<T>(uint baseId,GameCmd.ItemSerialize serverdata = null) where T : BaseItem,new ()
    //{
    //    T baseItem = new T();
    //    baseItem.UpdateSeverData(baseId,serverdata);
    //    return (null != baseItem.BaseData) ? baseItem : null;
    //}
    #endregion

    #region TreasureMap
    public CangBaoTuDataBase tabData
    {
        private set;
        get;
    }
    public uint MapID
    {
        get
        {
            return (null != serverData) ? GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Treasure_Use_Map) : 0;
        }
    }
    public uint MapIndex
    {
        get
        {
            return (null != serverData) ? GetItemAttribute(GameCmd.eItemAttribute.Item_Attribute_Treasure_Use_Pos) : 0;
        }
    }
    public Vector2 TargetPosition
    {
        get
        {
            return GetMapPos(1);
        }
    }
    public Vector2 TransferPosition
    {
        get
        {
            return GetMapPos(2);
        }
    }

    Vector2 GetMapPos(int type)
    {
        Vector2 vec = Vector2.zero;
        if (IsTreasureMap)
        {
            CangBaoTuDataBase tabData = GameTableManager.Instance.GetTableItem<CangBaoTuDataBase>(MapID);
            if (tabData != null)
            {
                string[] str = tabData.pos_list.Split(';');
                if (MapIndex < str.Length)
                {
                    string targetPos = str[(int)MapIndex];
                    string[] vecStr = targetPos.Split('_');
                    string[] transPos = tabData.goto_pos.Split('_');
                    if (vecStr.Length == 2)
                    {
                        if (type == 1)
                        {
                            float x = float.Parse(vecStr[0]);
                            float y = float.Parse(vecStr[1]);
                            vec = new Vector2(x, y);
                        }
                        else
                        {
                            float tx = float.Parse(transPos[0]);
                            float ty = float.Parse(transPos[1]);
                            vec = new Vector2(tx, ty);
                        }
                    }
                }
            }

        }

        return vec;
    }

    public int MapSortID
    {
        set;
        get;
    }
    #endregion
}