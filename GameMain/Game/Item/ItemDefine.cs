using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.ComponentModel;

public class ItemDefine
{
    #region define
    //空图标名称
    //public const string ICON_NULL = "icon_null";
    public const string ICON_NULL = "cm_icon_add";
    //锁
    public const string ICON_LOCK = "cm_icon_lock";
    //+
    public const string ICON_ADD = "cm_icon_add";
    //稀有装备档次
    public const int CONST_RARE_EQUIP_GRADE = 6;
    //批量分解自动选择装备档次
    public const int CONST_AUTO_SELECT_BATCH_ITEM_GRADE = 5;
    /// <summary>
    /// 跳转途径触发条件类型
    /// </summary>
    public enum ItemJumpCondiMatchType
    {
        Invalid = 0,
        Clan = 1,       //氏族
        Lv = 2,         //等级
    }

    public enum ItemDataType
    {
        BaseItem = 1,
        Equip,
        BaseEquip,
        Muhon,
        Gem,
        RuneStone
    }

    /// <summary>
    /// 消耗类型
    /// </summary>
    public enum CostType
    {
        None =0,
        Currency = 1,       //货币
        Item = 2,           //道具
        Exp =3,             //经验
    }

    /// <summary>
    /// 消耗数据类
    /// </summary>
    public class CostData
    {
        //消耗类型
        public CostType CT = CostType.None;
        //CT 类型下的ID
        public uint ID = 0;
        //消耗数量
        public uint Num = 0;
    }

    /// <summary>
    /// 跳转途径触发条件数据类
    /// </summary>
    public class ItemJumCondiMatchData
    {
        //触发类型
        public ItemJumpCondiMatchType MatchType = ItemJumpCondiMatchType.Invalid;
        //参数
        public uint MatchParam = 0;
        //不满足提示
        public string MatchNotice = "";
    }

    /// <summary>
    /// 物品跳转类型
    /// </summary>
    public enum ItemJumpType
    {
        Invalid = 0,
        Panel = 1,
        Wild = 2,
        NPC = 3,
        Describe= 4,
    }

    /// <summary>
    /// 物品tips功能类型
    /// </summary>
    public enum ItemTipsFNType
    {
        None = 0,

        //----------公共---------
        [Description("出售")]
        Sell,                       //出售
        [Description("展示")]
        Show,                       //展示
        [Description("获取")]
        Get,                        //获取途径
        [Description("跳转")]
        Jump,                       //跳转

        //----------装备---------
        //[Description("锻造")]
        //Forging,                    //锻造
        //[Description("镶嵌")]
        //Inlay ,                     //镶嵌
        [Description("修理")]
        Repair,                     //修理
        //[Description("分解")]
        //Split,                      //分解
        [Description("装备")]
        Wear,                       //穿戴
        [Description("卸下")]
        Unload,                     //卸下

        //----------圣魂---------
        //[Description("升级")]
        //Upgrade,                    //升级   
        //[Description("激活")]
        //Active,                     //激活   
        //[Description("进化")]
        //Evolve,                     //进化   
        //[Description("融合")]
        //Blend,                      //融合
     
        //----------药品---------
        [Description("使用")]
        Use,                        //使用
        [Description("携带")]
        Carry,                      //携带

        //----------坐骑---------
        [Description("解封")]
        DeArchive,                  //解封
        //----------凭证---------
        //----------技能---------
        Max,
    }

    /// <summary>
    /// 物品标识类型
    /// </summary>
    public enum ItemMaskType
    {
        PickBind = 1,       // 拾取绑定
        UseBind = 2,        //使用绑定
        DeathDrop = 3,      //死亡掉落
        Discard = 4,        //丢弃
        StallTrading = 5,   //摆摊交易
        Destroy = 6,        //销毁
        Sell = 7,           //出售
        PlaceInPersonalWareHouse = 8,       //放入个人仓库
        PlaceInKnapsackGrid = 9,            //放入背包格
        PlaceInEquipHouse = 10,             //放入装备格
        PlaceInClanWareHouse = 11,          //放入氏族厂库
    }

    /// <summary>
    /// 物品品质
    /// </summary>
    public enum ItemQualityType
    {
        White,      //白色品质
        Yellow,     //黄色
        Green,      //绿色品质
        Blue,       //蓝色品质
        Purple,     //紫色品质
        Orange,     //橙色品质
        Max,
    }

    /// <summary>
    /// 常用的icon类型 
    /// </summary>
    public enum CommonItem
    {
        /// <summary>
        /// 铜币
        /// </summary>
        MoneyType_Coupon = 1,
        /// <summary>
        /// 文钱
        /// </summary>
        MoneyType_Money = 2,
        /// <summary>
        /// 点券
        /// </summary>
        MoneyType_Cold = 3,

        Exp = 5,
    }

    /// <summary>
    /// 属性值类型
    /// </summary>
    public enum AttrValueType
    {
        Percentage = 1,
        Intvalue = 2,
        Section = 3
    }


    /// <summary>
    /// 消耗品子类
    /// </summary>
    public enum ItemConsumerSubType
    {
        None = 0,
        Hp = 1,
        Mp = 2,
        Moment =3,//瞬时药
        Pet = 4,    //宠物药
        Horn = 5,//喇叭
        Token =6,//令牌
        ZiLing = 7,//紫灵丹
        TaskUseItem =8,//任务使用类
        SoulBead = 9,//战魂珠
        MountsBead = 10,//坐骑珠
        TreasureMap = 11, //藏宝图
    }

    /// <summary>
    /// 凭证子类
    /// </summary>
    public enum ItemMaterialSubType
    {
        None = 0,
        Gem = 1,            //宝石
        Runestone = 2,      //符石
        SkillBook = 3,      //技能书
        Task = 4,           //任务
        PetChips = 5,          //战魂碎片    
        RideChips = 6,          //坐骑碎片
        FuRuiZhu =7,
        CollectWord = 8,    //集字
    }

    /// <summary>
    /// 更新物品类型
    /// </summary>
    public enum UpdateItemType
    {
        None = 0,
        Add = 1,            //添加（增加）
        Remove = 2,         //删除(减少)
        Update = 3,         //更新（刷新）
    }

    /// <summary>
    /// 本地跳转数据
    /// </summary>
    public class LocalJumpWayData
    {
        #region define
        /// <summary>
        /// 野外跳转数据
        /// </summary>
        public class ItemJumpWildData
        {
            public uint MapID = 0;
            public Vector3 DesPos = Vector3.zero;
        }

        /// <summary>
        /// 面板跳转数据
        /// </summary>
        public class ItemJumpPanelData
        {
            public PanelID PID = PanelID.None;
            public UIPanelBase.PanelJumpData PanelJData = null;
        }

        /// <summary>
        /// npc跳转数据
        /// </summary>
        public class ItemJumpNPCData
        {
            public uint MapID = 0;
            public uint NPCID = 0;
        }

        /// <summary>
        /// 纯文本描述数据
        /// </summary>
        public class ItemDescribeData 
        {
            public string des;
        }
        #endregion

        #region property
        //跳转iD
        private uint m_uintJumpID = 0;
        public uint ID
        {
            get
            {
                return m_uintJumpID;
            }
        }

        public table.JumpWayDataBase JumpData
        {
            get
            {
                return GameTableManager.Instance.GetTableItem<table.JumpWayDataBase>(m_uintJumpID);
            }
        }

        /// <summary>
        /// 是否需要外部数据填充参数3
        /// </summary>
        public bool NeedFillParam3
        {
            get
            {
                return (null != JumpData && JumpData.fillParam3 != 0 && JumpData.param3 == 0);
            }
        }

        /// <summary>
        /// 使用唯一id填充3
        /// </summary>
        public bool FillParam3WithThisID
        {
            get
            {
                return (null != JumpData && JumpData.fillParam3WithQwID != 0);
            }
        }

        /// <summary>
        /// 途径类型
        /// </summary>
        public ItemJumpType IJT
        {
            get
            {
                return (null != JumpData) ? (ItemJumpType)JumpData.jumpTypeID : ItemJumpType.Invalid;
            }
        }

        /// <summary>
        /// 是否触发要匹配条件
        /// </summary>
        public bool NeedMatchCondi
        {
            get
            {
                return (null != CondiMatchData && CondiMatchData.Count > 0);
            }
        }

        /// <summary>
        /// 二级跳转
        /// </summary>
        public uint SecondJumpWayId
        {
            get
            {
                return (null != JumpData) ? JumpData.secondJumpWayId : 0; 
            }
        }

        #endregion

        #region Op
        private LocalJumpWayData(uint jumpId)
        {
            m_uintJumpID = jumpId;
            ParseTabData();
        }

        public static LocalJumpWayData Create(uint jumpId)
        {
            return new LocalJumpWayData(jumpId);
        }

        public ItemJumpPanelData PanelData = null;

        public ItemJumpWildData WildData = null;

        public ItemJumpNPCData NpcData = null;

        public ItemDescribeData DesData = null;

        public List<ItemJumCondiMatchData> CondiMatchData = null;

        /// <summary>
        /// 解析表格数据
        /// </summary>
        private void ParseTabData()
        {
            table.JumpWayDataBase jumpWayData = JumpData;
            if (null != jumpWayData)
            {
                switch (IJT)
                {
                    case ItemJumpType.Panel:
                        {
                            PanelID panelID = (PanelID)Enum.Parse(typeof(PanelID), jumpWayData.param1);
                            string[] strTabs = jumpWayData.param2.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                            PanelData = new ItemJumpPanelData();
                            PanelData.PID = panelID;
                            PanelData.PanelJData = new UIPanelBase.PanelJumpData();
                            PanelData.PanelJData.Tabs = new int[strTabs.Length];
                            for (int i = 0; i < strTabs.Length; i++)
                            {
                                PanelData.PanelJData.Tabs[i] = int.Parse(strTabs[i]);
                            }
                            if (jumpWayData.param3 != 0)
                            {
                                PanelData.PanelJData.Param = jumpWayData.param3;
                            }
                        }
                        break;
                    case ItemJumpType.Wild:
                        {
                            uint mapID = 0;
                            if (!string.IsNullOrEmpty(jumpWayData.param1) && uint.TryParse(jumpWayData.param1, out mapID)
                                && !string.IsNullOrEmpty(jumpWayData.param2))
                            {
                                Vector3 pos = Vector3.zero;
                                string[] posStr = jumpWayData.param2.Split(new char[] { '_' });
                                if (null != posStr && posStr.Length == 2)
                                {
                                    if (float.TryParse(posStr[0].Trim(), out pos.x)
                                        && float.TryParse(posStr[1].Trim(), out pos.z))
                                    {
                                        pos.z = -pos.z;
                                        WildData = new ItemJumpWildData()
                                        {
                                            MapID = mapID,
                                            DesPos = pos,
                                        };
                                    }
                                }
                            }
                        }
                        break;
                    case ItemJumpType.NPC:
                        {
                            NpcData = new ItemJumpNPCData()
                            {
                                MapID = (!string.IsNullOrEmpty(jumpWayData.param1)) ? uint.Parse(jumpWayData.param1) : 0 ,
                                NPCID = (!string.IsNullOrEmpty(jumpWayData.param2)) ? uint.Parse(jumpWayData.param2) : 0 ,
                            };
                        }
                        break;
                    case ItemJumpType.Describe:
                        {
                            DesData = new ItemDescribeData()
                            {
                                   des = string.Format(jumpWayData.param1),
                            };
                        }
                        break;

                }

                if (null == CondiMatchData)
                {
                    CondiMatchData = new List<ItemJumCondiMatchData>();
                    ItemJumCondiMatchData jumpMatchData = null;
                    if (jumpWayData.matchCondi1 != (int)ItemJumpCondiMatchType.Invalid)
                    {
                        jumpMatchData = new ItemJumCondiMatchData()
                        {
                            MatchType = (ItemJumpCondiMatchType)jumpWayData.matchCondi1,
                            MatchParam = jumpWayData.matchCondiParam1,
                            MatchNotice = jumpWayData.matchCondiNotice1,
                        };
                        CondiMatchData.Add(jumpMatchData);
                    }

                    if (jumpWayData.matchCondi2 != (int)ItemJumpCondiMatchType.Invalid)
                    {
                        jumpMatchData = new ItemJumCondiMatchData()
                        {
                            MatchType = (ItemJumpCondiMatchType)jumpWayData.matchCondi2,
                            MatchParam = jumpWayData.matchCondiParam2,
                            MatchNotice = jumpWayData.matchCondiNotice2,
                        };
                        CondiMatchData.Add(jumpMatchData);
                    }

                    if (jumpWayData.matchCondi3 != (int)ItemJumpCondiMatchType.Invalid)
                    {
                        jumpMatchData = new ItemJumCondiMatchData()
                        {
                            MatchType = (ItemJumpCondiMatchType)jumpWayData.matchCondi3,
                            MatchParam = jumpWayData.matchCondiParam3,
                            MatchNotice = jumpWayData.matchCondiNotice3,
                        };
                        CondiMatchData.Add(jumpMatchData);
                    }
                }
            }

        }
        #endregion
    }


    public class BasePassData
    {
        //改变数量(大于0表示增加，小于0标示减少，等于0标示不变)
        public int ChangeNum
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 刷新钱币传递参数
    /// </summary>
    public class UpdateCurrecyPassData : BasePassData
    {
        //钱币类型
        public GameCmd.MoneyType MoneyType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 更新物品全局事件传递数据（参数）
    /// </summary>
    public class UpdateItemPassData : BasePassData
    {
        //针对UpdateItemType = Add有效
        public GameCmd.AddItemAction AddItemAction
            = GameCmd.AddItemAction.AddItemAction_Normal;
        //更新类型
        public UpdateItemType UpdateType = UpdateItemType.None;

        //物品唯一id
        public uint QWThisId;
        //物品表格id
        public uint BaseId;
        public void Reset()
        {
            AddItemAction = GameCmd.AddItemAction.AddItemAction_Normal;
            UpdateType = UpdateItemType.None;
            QWThisId = 0;
            BaseId = 0;
            ChangeNum = 0;
        }

    }

    /// <summary>
    /// 公共物品格子数据
    /// </summary>
    public class UIItemCommonData
    {
        public UIItemCommonData()
        {
            ShowGetWay = true;
        }
        //物品表里面的id
        public uint DwObjectId
        {
            get;
            set;
        }

        public uint ItemThisId { get; set; }
        //数量
        public uint Num
        {
            get;
            set;
        }
        /// <summary>
        /// ICON名称
        /// </summary>
        public string IconName
        {
            get;
            set;
        }

        public bool ShowGetWay
        {
            get;
            set;
        }

        public uint Qulity
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
        //需要的数量
        public uint NeedNum;

    }

    /// <summary>
    /// 本地位置数据类型
    /// </summary>
    public class ItemLocation
    {
        /// <summary>
        /// 背包类型
        /// </summary>
        public GameCmd.PACKAGETYPE PackType = GameCmd.PACKAGETYPE.PACKAGETYPE_NONE;
        /// <summary>
        /// 背包中的位置
        /// </summary>
        public UnityEngine.Vector2 Position = UnityEngine.Vector2.zero;
    }
    #endregion

    #region Static Method


    /// <summary>
    /// 根据物品品质获取外框（公共图集）
    /// </summary>
    /// <param name="num"> (装备为附加属性条目数，其他物品为附加品质)</param>
    /// <returns></returns>
    public static string GetItemBorderIcon(ItemDefine.ItemQualityType quality)
    {
        return GetItemBorderIcon((uint)quality);
    }

    private static Dictionary<uint, string> m_dicBorderIconCache = new Dictionary<uint, string>();
    /// <summary>
    /// 根据物品品质获取外框（公共图集）
    /// </summary>
    /// <param name="num"> (装备为附加属性条目数，其他物品为附加品质)</param>
    /// <returns></returns>
    public static string GetItemBorderIcon(uint quality)
    {
        string borderName = "";
        if (m_dicBorderIconCache.TryGetValue(quality, out borderName))
        {
            return borderName;
        }else
        {
            borderName = GameTableManager.Instance.GetGlobalConfig<string>("ItemQualityBorderName"
                , ((ItemDefine.ItemQualityType)quality).ToString());
            m_dicBorderIconCache.Add(quality, borderName);
        }
        return borderName;
    }
    private static Dictionary<uint, string> m_dicFangBorderIconCache = new Dictionary<uint, string>();
    /// <summary>
    /// 根据道具id获取外框（公共图集） 方框
    /// </summary>
    /// <param name="num"> (装备为附加属性条目数，其他物品为附加品质)</param>
    /// <returns></returns>
    public static string GetItemFangBorderIconByItemID(uint itemID)
    {
        string borderName = "";
        if (m_dicFangBorderIconCache.TryGetValue(itemID, out borderName))
        {
            return borderName;
        }
        else
        {
            table.ItemDataBase item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemID);
            if (item != null)
            {
                borderName = GameTableManager.Instance.GetGlobalConfig<string>("ItemQualityBorderName"
               , ((ItemDefine.ItemQualityType)item.quality).ToString());
                m_dicFangBorderIconCache.Add(itemID, borderName);
            }            
        }
        return borderName;
    }

    private static Dictionary<uint, string> m_dicBgYuanBorderIconCache = new Dictionary<uint, string>();
    /// <summary>
    /// 根据道具id获取外框（公共图集） 圆框
    /// </summary>
    /// <param name="num"> (装备为附加属性条目数，其他物品为附加品质)</param>
    /// <returns></returns>
    public static string GetItemBgYuanBorderIconByItemID(uint itemID)
    {
        string borderName = "";
        if (m_dicBgYuanBorderIconCache.TryGetValue(itemID, out borderName))
        {
            return borderName;
        }
        else
        {
            table.ItemDataBase item = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(itemID);
            if (item != null)
            {
                borderName = GameTableManager.Instance.GetGlobalConfig<string>("ItemQualityBgYuanBorderName"
               , ((ItemDefine.ItemQualityType)item.quality).ToString());
                m_dicBgYuanBorderIconCache.Add(itemID, borderName);
            }            
        }
        return borderName;
    }
    /// <summary>
    /// 根据物品品质获取tips顶部背景（公共图集）
    /// </summary>
    /// <param name="num"> (装备为附加属性条目数，其他物品为附加品质)</param>
    /// <returns></returns>
    public static TextureID GetItemTipsTopIcon(ItemQualityType qType)
    {
        TextureID textureId = TextureID.Biankuang_tips_hui;
        switch (qType)
        {
            case ItemDefine.ItemQualityType.White:
                textureId = TextureID.Biankuang_tips_hui;
                break;
            case ItemDefine.ItemQualityType.Yellow:
                textureId = TextureID.Biankuang_tips_danhuang;
                break;
            case ItemDefine.ItemQualityType.Green:
                textureId = TextureID.Biankuang_tips_lv;
                break;
            case ItemDefine.ItemQualityType.Blue:
                textureId = TextureID.Biankuang_tips_lan;
                break;
            case ItemDefine.ItemQualityType.Purple:
                textureId = TextureID.Biankuang_tips_zi;
                break;
            case ItemDefine.ItemQualityType.Orange:
                textureId = TextureID.Biankuang_tips_huang;
                break;
            default:
                textureId = TextureID.Biankuang_tips_hui;
                break;
        }
        return textureId;
    }

    /// <summary>
    /// 绑定标志位是否生效
    /// </summary>
    /// <param name="bindType"></param>
    /// <param name="bindValue"></param>
    /// <returns></returns>
    public static bool isBindMaskEnable(GameCmd.enumItemBindType bindType,uint bindValue)
    {
        return ((((int)bindValue >> (int)bindType) & 1) == 1);
    }

    /// <summary>
    /// 是否绑定
    /// </summary>
    /// <param name="bindValue"></param>
    /// <returns></returns>
    public static bool IsBind(uint bindValue)
    {
        if (isBindMaskEnable(GameCmd.enumItemBindType.BindItemType_Pickup, bindValue) ||
             isBindMaskEnable(GameCmd.enumItemBindType.BindItemType_Use, bindValue))
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 变换表格数据到真实百分比
    /// </summary>
    /// <param name="tableValue"></param>
    /// <returns></returns>
    public static float TransformTableValue2RealPercentage(float tableValue)
    {
        float realValue = tableValue / 100f;
        return realValue;
    }
    /// <summary>
    /// 是否当前物品标识位为true
    /// </summary>
    /// <param name="mask">标识码</param>
    /// <param name="itemMaskType">物品标识类型</param>
    /// <returns></returns>
    public static bool IsItemMaskEnable(uint mask,ItemMaskType itemMaskType)
    {
        return (mask & ( 1 << ((int)itemMaskType - 1))) != 0 ? true : false;
    }

    /// <summary>
    /// 设置物品标识
    /// </summary>
    /// <param name="mask">标识码</param>
    /// <param name="itemMaskType">物品标识类型</param>
    /// <param name="status">标识状态</param>
    public static void SetItemMask(ref uint mask,ItemMaskType itemMaskType,bool status = true)
    {
        if (status)
            mask |= (uint)(1 << ((int)itemMaskType - 1));
        else
            mask &= (uint)(~(1 << ((int)itemMaskType - 1)));
    }

    /// <summary>
    /// 解析服务端位置为本地数据
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    public static ItemLocation TransformServerLocation2Local(uint location)
    {
        ItemLocation result = new ItemLocation()
        {
            PackType = (GameCmd.PACKAGETYPE)(((0xFFFFFFFF << 20) & location) >> 20),
            Position = new UnityEngine.Vector2
                (
                (0xFFFFFFFF >> 22) & location
                , (((0xFFFFFFFF >> 12) & location) & ((0xFFFFFFFF << 10)) & location) >> 10
                ),
        };
        return result;
    }

    /// <summary>
    /// 合并本地位置信息为服务端位置
    /// </summary>
    /// <returns></returns>
    public static uint TransformLocal2ServerLocation(GameCmd.PACKAGETYPE pType,Vector2 localPos)
    {
        uint serverLocation = (
            (uint)pType << 20)          //PType
            | (uint)localPos.y << 10    //Y Pos
            | (uint)localPos.x;         //X pos
        return serverLocation;
    }

    /// <summary>
    /// 获取换算后的耐久=tablevalue/256，向下取整
    /// </summary>
    /// <param name="tablevalue"></param>
    /// <returns></returns>
    public static int GetDisplayDruable(uint tablevalue)
    {
        return Mathf.CeilToInt(tablevalue / 256f);
    }

    /// <summary>
    /// 是否为通用物品
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool IsCommonItem(uint id)
    {
        return (id >= (int)CommonItem.MoneyType_Coupon && id <= (int)CommonItem.Exp) ? true : false;
    }

    /// <summary>
    /// 该属性id是否为附加属性
    /// </summary>
    /// <param name="id">属性id</param>
    /// <returns></returns>
    public static bool IsAdditiveAttr(uint id)
    {
        return (id >= (int)GameCmd.eItemAttribute.Item_Attribute_Liliang 
            && id <= (int)GameCmd.eItemAttribute.Item_Attribute_Mul_Exp) ? true : false;
    }

    /// <summary>
    ///构建字符串 例：1/3 当需要数量大于当前拥有数量,当前拥有数量标记为红色，反之为绿色
    /// </summary>
    /// <param name="holdNum">当前拥有数量</param>
    /// <param name="needNum">需要数量</param>
    /// <returns></returns>
    public static string BuilderStringByHoldAndNeedNum(uint holdNum,uint needNum)
    {
        string builderText = "";
        builderText = (holdNum >= needNum) ? ColorManager.GetColorString(ColorType.JZRY_Green, holdNum + "")
            : ColorManager.GetColorString(ColorType.JZRY_Txt_Red, holdNum + "");
        builderText += ("/" + needNum);
        return builderText;
    }

    /// <summary>
    /// 根据tipsType获取功能类型列表
    /// </summary>
    /// <param name="tipsType">tip类型</param>
    /// <param name="rejectType">过滤掉某项功能</param>
    /// <returns></returns>
    public static List<ItemTipsFNType> GetItemTipsFNTyps(UIDefine.UITipsType tipsType, List<ItemTipsFNType> rejectTypes = null)
    {
        List<ItemTipsFNType> tipsFnTypes = new List<ItemTipsFNType>();
        
        switch (tipsType)
        {
            case UIDefine.UITipsType.Equip:
                tipsFnTypes.Add(ItemTipsFNType.Repair);
                //tipsFnTypes.Add(ItemTipsFNType.Split);
                tipsFnTypes.Add(ItemTipsFNType.Sell);
                //tipsFnTypes.Add(ItemTipsFNType.Show);
                tipsFnTypes.Add(ItemTipsFNType.Wear);
                tipsFnTypes.Add(ItemTipsFNType.Unload);
                break;
            case UIDefine.UITipsType.Material:
            case UIDefine.UITipsType.MissionItem:
                tipsFnTypes.Add(ItemTipsFNType.Sell);
                tipsFnTypes.Add(ItemTipsFNType.Get);
                //tipsFnTypes.Add(ItemTipsFNType.Show);
                break;
            case UIDefine.UITipsType.Consumption:
                tipsFnTypes.Add(ItemTipsFNType.Use);
                tipsFnTypes.Add(ItemTipsFNType.Carry);
                tipsFnTypes.Add(ItemTipsFNType.Sell);
                tipsFnTypes.Add(ItemTipsFNType.Get);
                //tipsFnTypes.Add(ItemTipsFNType.Show);
                break;
            case UIDefine.UITipsType.Muhon:
                //tipsFnTypes.Add(ItemTipsFNType.Show);
                tipsFnTypes.Add(ItemTipsFNType.Sell);
                tipsFnTypes.Add(ItemTipsFNType.Wear);
                tipsFnTypes.Add(ItemTipsFNType.Unload);
                break;
            case UIDefine.UITipsType.Skill:
                break;
            case UIDefine.UITipsType.Mounts:
                tipsFnTypes.Add(ItemTipsFNType.DeArchive);
                tipsFnTypes.Add(ItemTipsFNType.Sell);
                //tipsFnTypes.Add(ItemTipsFNType.Get);
                //tipsFnTypes.Add(ItemTipsFNType.Show);
                break;
        }
        
        if (null != rejectTypes && rejectTypes.Count > 0)
        {
            if (!rejectTypes.Contains(ItemTipsFNType.Jump))
            {
                tipsFnTypes.Add(ItemTipsFNType.Jump);
            }
            for (int i = 0; i < rejectTypes.Count; i++)
            {
                if (tipsFnTypes.Contains(rejectTypes[i]))
                {
                    tipsFnTypes.Remove(rejectTypes[i]);
                }
            }
        }else
        {
            tipsFnTypes.Add(ItemTipsFNType.Jump);
        }
        return tipsFnTypes;
    }

    /// <summary>
    /// 获取tip功能btn名称
    /// </summary>
    /// <param name="fnType"></param>
    /// <returns></returns>
    public static string GetItemTipsFNName(ItemTipsFNType fnType)
    {
        string des = fnType.GetEnumDescription();
        if (string.IsNullOrEmpty(des))
            des = "";
        return des;
    }
    
    #endregion
}