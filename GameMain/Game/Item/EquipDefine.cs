using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class EquipDefine
{
    
    #region Define
    public const int ATTR_PERCENTAGE_VALUE_SCALE = 10000;
    //圣魂最高星级
    public const int MUHON_MAX_STAR_LV = 5;
    /// <summary>
    /// 装备属性类型
    /// </summary>
    public enum EquipPropertyType
    {
        Base = 1,               //基础属性
        Additive = 2,           //附加属性
        Advanced = 3,           //高级属性
        Mosaic = 4,             //镶嵌属性
    }

    /// <summary>
    /// 装备养成标识
    /// </summary>
    public enum EquipGrowType
    {
        Refine = 1,
        Compound = 2,
        Split = 4,
    }

    /// <summary>
    /// 属性索引
    /// </summary>
    public enum AttrIndex
    {
        None = 0,
        First = 1,          //第一条
        Second = 2,         //第二条
        Third = 3,          //第三条
        Fourth = 4,         //第四条
        Fifth = 5,          //第五条
        Max,
    }

    /// <summary>
    /// 装备基础属性
    /// </summary>
    public enum EquipBasePropertyType
    {
        [Description("物理攻击")]
        PhyAttack = 1,      //物攻
        [Description("物理防御")]
        PhyDef = 2,         //物防
        [Description("法术攻击")]
        MagicAttack = 3,    //法攻
        [Description("法术防御")]
        MagicDef = 4,       //法防
    }

    /// <summary>
    /// 装备档次类型
    /// </summary>
    public enum EquipGradeType
    {
        None,
        One,        //1档
        Two,        //2档
        Three,      //3档
        Four,       //4档
        Five,       //5档
        Six,        //6档
        Seven,      //7档
        Max,        
    }
    public enum EquipColor 
    {
        None=0,
        White,
        Green,
        Blue,
        Purple,
        Orange,
        Max,
    }
    /// <summary>
    /// 格子强化本地数据
    /// </summary>
    public class LocalGridStrengthenData
    {
        private uint m_uId;
        public uint ID
        {
            get
            {
                return m_uId;
            }
        }

        //强化后增加基础属性
        private List<EquipBasePropertyData> m_lstBaseProp = null;
        public List<EquipBasePropertyData> BaseProp
        {
            get
            {
                return m_lstBaseProp;
            }
        }

        public table.GridStrengthenDataBase TableData
        {
            get
            {
                return GameTableManager.Instance.GetTableItem<table.GridStrengthenDataBase>(m_uId);
            }
        }

        //货币消耗数据类
        private ItemDefine.CostData m_moneyCostData = null;
        public ItemDefine.CostData MoneyCostData
        {
            get
            {
                return m_moneyCostData;
            }
        }

        //道具消耗数据类
        private List<ItemDefine.CostData> m_lstItemCostDatas = null;
        public List<ItemDefine.CostData> ItemCostDatas
        {
            get
            {
                return m_lstItemCostDatas;
            }
        }

        /// <summary>
        /// 解析表格数据
        /// </summary>
        private void ParseTableData()
        {
            table.GridStrengthenDataBase gt = TableData;
            if (null != gt)
            {
                //基础属性
                //m_lstBaseProp = new List<GameCmd.PairNumber>();
                m_lstBaseProp = new List<EquipBasePropertyData>();
                //GameCmd.PairNumber pair = null;
                EquipBasePropertyData baseData = null;
                if (gt.phyAttack != 0)
                {
                    //物攻
                    baseData = new EquipBasePropertyData()
                    {
                        popertyType = EquipBasePropertyType.PhyAttack,
                        valueMax = gt.phyAttack,
                        valueMin = gt.phyAttack,
                    };
                    m_lstBaseProp.Add(baseData);
                    //pair = new GameCmd.PairNumber()
                    //{
                    //    id = (uint)GameCmd.eItemAttribute.Item_Attribute_Pdam,
                    //    value = gt.phyAttack,
                    //};
                    //m_lstBaseProp.Add(pair);
                }
                if (gt.magicAttack != 0)
                {
                    //法攻
                    baseData = new EquipBasePropertyData()
                    {
                        popertyType = EquipBasePropertyType.MagicAttack,
                        valueMax = gt.magicAttack,
                        valueMin = gt.magicAttack,
                    };
                    m_lstBaseProp.Add(baseData);
                    //pair = new GameCmd.PairNumber()
                    //{
                    //    id = (uint)GameCmd.eItemAttribute.Item_Attribute_Mdam,
                    //    value = gt.magicAttack,
                    //};
                    //m_lstBaseProp.Add(pair);
                }
                if (gt.phyDef != 0)
                {
                    //物防
                    baseData = new EquipBasePropertyData()
                    {
                        popertyType = EquipBasePropertyType.PhyDef,
                        valueMax = gt.phyDef,
                        valueMin = gt.phyDef,
                    };
                    m_lstBaseProp.Add(baseData);
                    //pair = new GameCmd.PairNumber()
                    //{
                    //    id = (uint)GameCmd.eItemAttribute.Item_Attribute_Pdef,
                    //    value = gt.phyDef,
                    //};
                    //m_lstBaseProp.Add(pair);
                }
                if (gt.magicDef != 0)
                {
                    //法防
                    baseData = new EquipBasePropertyData()
                    {
                        popertyType = EquipBasePropertyType.MagicDef,
                        valueMax = gt.magicDef,
                        valueMin = gt.magicDef,
                    };
                    m_lstBaseProp.Add(baseData);
                    //pair = new GameCmd.PairNumber()
                    //{
                    //    id = (uint)GameCmd.eItemAttribute.Item_Attribute_Mdef,
                    //    value = gt.magicDef,
                    //};
                    //m_lstBaseProp.Add(pair);
                }

                //货币
                ItemDefine.CostData costData = new ItemDefine.CostData()
                {
                    CT = ItemDefine.CostType.Currency,
                    ID = gt.costMoneyType,
                    Num = gt.costMoneyNum,
                };
                m_moneyCostData = costData;

                //消耗道具
                m_lstItemCostDatas = new List<ItemDefine.CostData>();
                if (gt.costItem1 != 0 && gt.costItemNum1 > 0)
                {
                    costData = new ItemDefine.CostData()
                    {
                        CT = ItemDefine.CostType.Item,
                        ID = gt.costItem1,
                        Num =gt.costItemNum1,
                    };
                    m_lstItemCostDatas.Add(costData);
                }
                if (gt.costItem2 != 0 && gt.costItemNum2 > 0)
                {
                    costData = new ItemDefine.CostData()
                    {
                        CT = ItemDefine.CostType.Item,
                        ID = gt.costItem2,
                        Num =gt.costItemNum2,
                    };
                    m_lstItemCostDatas.Add(costData);
                }
                if (gt.costItem3 != 0 && gt.costItemNum3 > 0)
                {
                    costData = new ItemDefine.CostData()
                    {
                        CT = ItemDefine.CostType.Item,
                        ID = gt.costItem3,
                        Num =gt.costItemNum3,
                    };
                    m_lstItemCostDatas.Add(costData);
                }
            }
        }

        private LocalGridStrengthenData(uint id)
        {
            m_uId = id;
            ParseTableData();
        }

        public static LocalGridStrengthenData Create(uint job,uint pos,uint lv)
        {
            return Create(BuildGridStrengthId(job, pos, lv));
        }

        public static LocalGridStrengthenData Create(uint id)
        {
            return new LocalGridStrengthenData(id);
        }
    }

    /// <summary>
    /// 格子强化套装属性组
    /// </summary>
    public class LocalGridStrengthenGroupSuitData
    {
        private uint m_ujob;
        public uint Job
        {
            get
            {
                return m_ujob;
            }
        }
        //激活套装属性需要同时满足强化等级的部位的数量
        private uint m_uMatchNeedNum = 0;
        public uint MatchNeedNum
        {
            get
            {
                return m_uMatchNeedNum;
            }
        }

        //最大套装强化等级
        private uint m_imaxStrengthenSuitLv = 0;
        public uint MaxStrengthenSuitLv
        {
            get
            {
                return m_imaxStrengthenSuitLv;
            }
        }

        private List<uint> datas = null;
        public void AddData(uint strengthLv)
        {
            if (!datas.Contains(strengthLv))
            {
                datas.Add(strengthLv);
                if (m_imaxStrengthenSuitLv < strengthLv)
                {
                    m_imaxStrengthenSuitLv = strengthLv;
                }
            }
        }

        public void Sort()
        {
            datas.Sort();
        }

        /// <summary>
        /// 获取强化等级数据
        /// </summary>
        /// <returns></returns>
        public List<uint> GetStrengthenLvDatas()
        {
            return datas;
        }

        public LocalGridStrengthenSuitData GetLocalStrengthenData(uint lv)
        {
            return LocalGridStrengthenSuitData.Create(m_ujob, lv);
        }

        private LocalGridStrengthenGroupSuitData(uint job, uint matchNeedNum)
        {
            datas = new List<uint>();
            m_ujob = job;
            m_uMatchNeedNum = matchNeedNum;
        }

        public static LocalGridStrengthenGroupSuitData Create(uint job,uint matchNeedNum)
        {
            return new LocalGridStrengthenGroupSuitData(job,matchNeedNum);
        }
    }

    /// <summary>
    /// 格子强化套装属性
    /// </summary>
    public class LocalGridStrengthenSuitData
    {
        private uint m_ujob = 0;
        public uint Job
        {
            get
            {
                return m_ujob;
            }
        }

        private int m_ulv = 0;
        public int Lv
        {
            get
            {
                return m_ulv;
            }
        }

        public table.GridStrengthenSuitDataBase TableData
        {
            get
            {
                return GameTableManager.Instance.GetTableItem<table.GridStrengthenSuitDataBase>(m_ujob, m_ulv);
            }
        }


        public string Des
        {
            get
            {
                return (null != TableData) ? TableData.des : "";
            }
        }

        //触发部位数
        public uint TriggerPosNum
        {
            get
            {
                return (null != TableData) ? TableData.triggerPosNum : 0;
            }
        }

        //触发强化等级
        public uint TriggerStrengLv
        {
            get
            {
                return (null != TableData) ? TableData.triggerStrengLv : 0;
            }
        }
        // 强化套装增加基础属性
        private List<GameCmd.PairNumber> m_lstSuitProp = null;
        public List<GameCmd.PairNumber> SuitProp
        {
            get
            {
                return m_lstSuitProp;
            }
        }

        private void ParseTableData()
        {
            table.GridStrengthenSuitDataBase gt = TableData;
            if (null != gt)
            {
                //套装
                m_lstSuitProp = new List<GameCmd.PairNumber>();
                GameCmd.PairNumber pair = null;
                if (gt.phyAttack != 0)
                {
                    //物攻
                    pair = new GameCmd.PairNumber()
                    {
                        id = (uint)GameCmd.eItemAttribute.Item_Attribute_Pdam,
                        value = gt.phyAttack,
                    };
                    m_lstSuitProp.Add(pair);
                }
                if (gt.magicAttack != 0)
                {
                    //法攻
                    pair = new GameCmd.PairNumber()
                    {
                        id = (uint)GameCmd.eItemAttribute.Item_Attribute_Mdam,
                        value = gt.magicAttack,
                    };
                    m_lstSuitProp.Add(pair);
                }
                if (gt.phyDef != 0)
                {
                    //物防
                    pair = new GameCmd.PairNumber()
                    {
                        id = (uint)GameCmd.eItemAttribute.Item_Attribute_Pdef,
                        value = gt.phyDef,
                    };
                    m_lstSuitProp.Add(pair);
                }
                if (gt.magicDef != 0)
                {
                    //法防
                    pair = new GameCmd.PairNumber()
                    {
                        id = (uint)GameCmd.eItemAttribute.Item_Attribute_Mdef,
                        value = gt.magicDef,
                    };
                    m_lstSuitProp.Add(pair);
                }

                if (gt.hp != 0)
                {
                    //生命值
                    pair = new GameCmd.PairNumber()
                    {
                        id = (uint)GameCmd.eItemAttribute.Item_Attribute_HPMax,
                        value = gt.hp,
                    };
                    m_lstSuitProp.Add(pair);
                }

                if (gt.magic != 0)
                {
                    //法术值
                    pair = new GameCmd.PairNumber()
                    {
                        id = (uint)GameCmd.eItemAttribute.Item_Attribute_MPMax,
                        value = gt.magic,
                    };
                    m_lstSuitProp.Add(pair);
                }
            }

        }

        private LocalGridStrengthenSuitData(uint job,uint lv)
        {
            m_ujob = job;
            m_ulv = (int)lv;
            ParseTableData();
        }


        public static LocalGridStrengthenSuitData Create(uint job,uint lv)
        {
            return new LocalGridStrengthenSuitData(job, lv);
        }

        //Item_Attribute_HPMax 生命
        //Item_Attribute_MPMax  法术
    }

    /// <summary>
    /// 本地属性数据类型
    /// </summary>
    public class LocalAttrData
    {
        #region property
        //属性
        private GameCmd.PairNumber attr;
        public GameCmd.PairNumber Attr
        {
            get
            {
                return attr;
            }
        }
        //属性id
        public uint AttrId
        {
            get
            {
                return (null != attr) ? attr.id : 0;
            }
        }
        //属性值
        public uint AttrValue
        {
            get
            {
                return (null != attr) ? attr.value : 0;
            }
        }

        public string AttrDes
        {
            get
            {
                return DataManager.Manager<EquipManager>().GetAttrDes(attr);
            }
        }

        //属性档次
        public uint Grade
        {
            get
            {
                return DataManager.Manager<EquipManager>().GetAttrGrade(attr);
            }
        }

        /// <summary>
        /// 是否为最大档次
        /// </summary>
        public bool IsMaxGrade
        {
            get
            {
                return DataManager.Manager<EquipManager>().IsAttrGradeMax(attr);
            }
        }
        //效果id
        private uint m_uint_effectId = 0;
        /// <summary>
        /// effectid
        /// </summary>
        public uint EffectId
        { 
            get
            {
                return DataManager.Manager<EquipManager>().TransformAttrToEffectId(attr);
            }
        }

        /// <summary>
        /// 下一级id
        /// </summary>
        public uint NextEffectId
        {
            get
            {
                return DataManager.Manager<EquipManager>().GetNextEffectId(EffectId);
            }
        }


        #endregion

        #region 构造方法
        private LocalAttrData(GameCmd.PairNumber attr)
        {
            this.attr = attr;
        }
        #endregion

        #region CreateInstance
        /// <summary>
        /// 根据effectid创建本地属性数据
        /// </summary>
        /// <param name="effectId"></param>
        /// <returns></returns>
        public static LocalAttrData Create(uint effectId)
        {
            return Create(DataManager.Manager<EquipManager>().GetAttrPairNumberById(effectId));
        }
        /// <summary>
        /// 根据键值对创建本地属性数据
        /// </summary>
        /// <param name="effectId"></param>
        /// <returns></returns>
        public static LocalAttrData Create(GameCmd.PairNumber attr)
        {
            if (null == attr)
            {
                return null;
            }
            return new LocalAttrData(attr);
        }
        #endregion
    }

    /// <summary>
    /// 装备基础属性数据类
    /// </summary>
    public class EquipBasePropertyData
    {
        //基础属性类型
        public EquipBasePropertyType popertyType = EquipBasePropertyType.MagicAttack;
        //值 区间 valueMin - valueMax
        public uint valueMin;
        public uint valueMax;
        //名称
        public string Name
        {
            get
            {
                return DataManager.Manager<TextManager>().GetBasePropertyName(popertyType);
            }
        }
        public override string ToString()
        {
            string str = "";
            if (valueMax == valueMin)
                str = "" + valueMin;
            else
                str = valueMin + "-" + valueMax;
            return str;
        }
    }
    #endregion

    #region StaticData

    /// <summary>
    /// 生成格子强化数据ID
    /// </summary>
    /// <param name="job"></param>
    /// <param name="pos"></param>
    /// <param name="lv"></param>
    /// <returns></returns>
    public static uint BuildGridStrengthId(uint job, uint pos, uint lv)
    {
        return job * 20000 + pos * 1000 + lv;
    }

    /// <summary>
    /// 构建装备格唯一id=（pos-1）*3 + equipGridIndex
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="equipGridIndex"></param>
    /// <returns></returns>
    public static int BuildEquipGridId(GameCmd.EquipPos pos, int equipGridIndex)
    {
        return ((int)pos - 1) * 3 + equipGridIndex;
    }

    /// <summary>
    /// 装备部位名称
    /// </summary>
    public static Dictionary<GameCmd.EquipType, LocalTextType> equipTypeNameDic = new Dictionary<GameCmd.EquipType, LocalTextType>()
    {
        {GameCmd.EquipType.EquipType_Hat,LocalTextType.Local_TXT_Hat},
        {GameCmd.EquipType.EquipType_Shoulder,LocalTextType.Local_TXT_Shoulder},
        {GameCmd.EquipType.EquipType_Coat,LocalTextType.Local_TXT_Coat},
        {GameCmd.EquipType.EquipType_Leg,LocalTextType.Local_TXT_Leg},
        {GameCmd.EquipType.EquipType_AdornlOne,LocalTextType.Local_TXT_Adornl},
        {GameCmd.EquipType.EquipType_Shield,LocalTextType.Local_TXT_Shield},
        {GameCmd.EquipType.EquipType_Equip,LocalTextType.Local_TXT_Equip},
        {GameCmd.EquipType.EquipType_Shoes,LocalTextType.Local_TXT_Shoes},
        {GameCmd.EquipType.EquipType_Cuff,LocalTextType.Local_TXT_Cuff},
        {GameCmd.EquipType.EquipType_Belf,LocalTextType.Local_TXT_Belf},
        {GameCmd.EquipType.EquipType_Capes,LocalTextType.Local_TXT_Capes},
        {GameCmd.EquipType.EquipType_Necklace,LocalTextType.Local_TXT_Necklace},
        {GameCmd.EquipType.EquipType_Office,LocalTextType.Local_TXT_Office},
        {GameCmd.EquipType.EquipType_SoulOne,LocalTextType.Local_TXT_Soul},
    };
    #endregion

    #region Static Method

    /// <summary>
    /// 装备位置名称
    /// </summary>
    /// <param name="equipType">装备类型</param>
    /// <returns></returns>
    public static string GetEquipPosName(GameCmd.EquipPos equipPos)
    {
        return GetEquipTypeName(TransformEquipPos2Type(equipPos));
    }

    public static string GetEquipTypeName(GameCmd.EquipType equipType)
    {
        LocalTextType key = LocalTextType.LocalText_None;
        if (equipTypeNameDic.TryGetValue(equipType, out key))
        {
            return DataManager.Manager<TextManager>().GetLocalText(key);
        }
        return "Unknow";
    }

    /// <summary>
    /// 获取时装名称
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetSuitPosName(GameCmd.EquipSuitType type)
    {
        LocalTextType key = LocalTextType.LocalText_None;
        switch (type)
        {
            case GameCmd.EquipSuitType.Back_Type:
                key = LocalTextType.Local_TXT_BackShow;
                break;
            case GameCmd.EquipSuitType.Clothes_Type:
                key = LocalTextType.Local_TXT_Clothes;
                break;
            case GameCmd.EquipSuitType.Face_Type:
                key = LocalTextType.Local_TXT_FaceShow;
                break;
            case GameCmd.EquipSuitType.Magic_Pet_Type:
                key = LocalTextType.Local_TXT_SweetPet;
                break;
            case GameCmd.EquipSuitType.Qibing_Type:
                key = LocalTextType.Local_TXT_Jones;
                break;
            case GameCmd.EquipSuitType.Unknow_Type:
                break;
            default:
                break;
        }
        if (key == LocalTextType.LocalText_None)
        {
            return "Unknow";
        }
        return DataManager.Manager<TextManager>().GetLocalText(key);
    }

    public static string GetSuitIcon(GameCmd.EquipSuitType type)
    {
        string iconName = "tubiaodi";

        switch (type)
        {
            case GameCmd.EquipSuitType.Back_Type:
                break;
            case GameCmd.EquipSuitType.Clothes_Type:
                break;
            case GameCmd.EquipSuitType.Face_Type:
                break;
            case GameCmd.EquipSuitType.Magic_Pet_Type:
                break;
            case GameCmd.EquipSuitType.Qibing_Type:
                break;
            default:
                break;
        }

        return iconName;
    }
    /// <summary>
    /// 获取部位Icon
    /// </summary>
    /// <param name="equipPos"></param>
    /// <returns></returns>
    public static string GetEquipPartIcon(GameCmd.EquipPos equipPos)
    {
        string iconName = "tubiaodi";
        switch(equipPos)
        {
            case GameCmd.EquipPos.EquipPos_Hat:
                iconName = "tubiao_di_toubu";
                break;
            case GameCmd.EquipPos.EquipPos_Shoulder:
                iconName = "tubiao_di_jianbu";
                break;
            case GameCmd.EquipPos.EquipPos_Coat:
                iconName = "tubiao_di_xiongjia";
                break;
            case GameCmd.EquipPos.EquipPos_Leg:
                iconName = "tubiao_di_hutui";
                break;
            case GameCmd.EquipPos.EquipPos_AdornlOne:
            case GameCmd.EquipPos.EquipPos_AdornlTwo:
                iconName = "tubiao_di_jiezhi";
                break;
            case GameCmd.EquipPos.EquipPos_Shield:
                iconName = "tubiao_di_dunpai";
                break;
            case GameCmd.EquipPos.EquipPos_Shoes:
                iconName = "tubiao_di_jiaobu";
                break;
            case GameCmd.EquipPos.EquipPos_Cuff:
                iconName = "tubiao_di_shouwan";
                break;
            case GameCmd.EquipPos.EquipPos_Belf:
                iconName = "tubiao_di_yaobu";
                break;
            //case GameCmd.EquipPos.EquipPos_Capes:
            //    iconName = "tubiao_di_jiezhi";
            //    break;
            case GameCmd.EquipPos.EquipPos_SoulOne:
            case GameCmd.EquipPos.EquipPos_SoulTwo:
                iconName = "tubiao_di_wuhun";
                break;
            case GameCmd.EquipPos.EquipPos_Necklace:
                iconName = "tubiao_di_xianglian";
                break;
            case GameCmd.EquipPos.EquipPos_Equip:
                iconName = "tubiao_di_wuqi";
                break;
            //case GameCmd.EquipPos.EquipPos_Office:
            //    iconName = "tubiao_di_jiezhi";
            //    break;
        }
        return iconName;
    }

    public static GameCmd.EquipType TransformEquipPos2Type(GameCmd.EquipPos equipPos)
    {
        GameCmd.EquipType eType = GameCmd.EquipType.EquipType_None;
        switch (equipPos)
        {
                
            case GameCmd.EquipPos.EquipPos_Hat:
                eType = GameCmd.EquipType.EquipType_Hat;
                break;
            case GameCmd.EquipPos.EquipPos_Shoulder:
                eType = GameCmd.EquipType.EquipType_Shoulder;
                break;
            case GameCmd.EquipPos.EquipPos_Coat:
                eType = GameCmd.EquipType.EquipType_Coat;
                break;
            case GameCmd.EquipPos.EquipPos_Leg:
                eType = GameCmd.EquipType.EquipType_Leg;
                break;
            case GameCmd.EquipPos.EquipPos_AdornlOne:
            case GameCmd.EquipPos.EquipPos_AdornlTwo:
                eType = GameCmd.EquipType.EquipType_AdornlOne;
                break;
            case GameCmd.EquipPos.EquipPos_Shield:
                eType = GameCmd.EquipType.EquipType_Shield;
                break;
            case GameCmd.EquipPos.EquipPos_Equip:
                eType = GameCmd.EquipType.EquipType_Equip;
                break;
            case GameCmd.EquipPos.EquipPos_Shoes:
                eType = GameCmd.EquipType.EquipType_Shoes;
                break;
            case GameCmd.EquipPos.EquipPos_Cuff:
                eType = GameCmd.EquipType.EquipType_Cuff;
                break;

            case GameCmd.EquipPos.EquipPos_Belf:
                eType = GameCmd.EquipType.EquipType_Belf;
                break;
            case GameCmd.EquipPos.EquipPos_Capes:
                eType = GameCmd.EquipType.EquipType_Capes;
                break;

            case GameCmd.EquipPos.EquipPos_Necklace:
                eType = GameCmd.EquipType.EquipType_Necklace;
                break;

            case GameCmd.EquipPos.EquipPos_Office:
                eType = GameCmd.EquipType.EquipType_Office;
                break;

            case GameCmd.EquipPos.EquipPos_SoulOne:
            case GameCmd.EquipPos.EquipPos_SoulTwo:
                eType = GameCmd.EquipType.EquipType_SoulOne;
                break;
        }
        return eType;
    }

    /// <summary>
    /// 获取装备位置
    /// </summary>
    /// <param name="equipType">装备类型</param>
    /// <returns></returns>
    public static GameCmd.EquipPos [] GetEquipPosByEquipType(GameCmd.EquipType equipType)
    {
        GameCmd.EquipPos[] equipPos = null;
        switch(equipType)
        {
            case GameCmd.EquipType.EquipType_Hat:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Hat;
                break;
            case GameCmd.EquipType.EquipType_Shoulder:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Shoulder;
                break;
            case GameCmd.EquipType.EquipType_Coat:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Coat;
                break;
            case GameCmd.EquipType.EquipType_Leg:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Leg;
                break;
            case GameCmd.EquipType.EquipType_AdornlOne:
                equipPos = new GameCmd.EquipPos[2];
                equipPos[0] = GameCmd.EquipPos.EquipPos_AdornlOne;
                equipPos[1] = GameCmd.EquipPos.EquipPos_AdornlTwo;
                break;
            case GameCmd.EquipType.EquipType_Shield:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Shield;
                break;
            case GameCmd.EquipType.EquipType_Equip:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Equip;
                break;
            case GameCmd.EquipType.EquipType_Shoes:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Shoes;
                break;
            case GameCmd.EquipType.EquipType_Cuff:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Cuff;
                break;
            case GameCmd.EquipType.EquipType_Belf:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Belf;
                break;
            case GameCmd.EquipType.EquipType_Capes:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Capes;
                break;
            case GameCmd.EquipType.EquipType_Necklace:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Necklace;
                break;
            case GameCmd.EquipType.EquipType_Office:
                equipPos = new GameCmd.EquipPos[1];
                equipPos[0] = GameCmd.EquipPos.EquipPos_Office;
                break;
            case GameCmd.EquipType.EquipType_SoulOne:
                equipPos = new GameCmd.EquipPos[2];
                equipPos[0] = GameCmd.EquipPos.EquipPos_SoulOne;
                equipPos[1] = GameCmd.EquipPos.EquipPos_SoulTwo;
                break;
        }
        return equipPos;
    }

    public ColorType GetEquipPropertyNGUIColorOfType(EquipPropertyType pTpe)
    {
        ColorType colorType = ColorType.None;
        switch (pTpe)
        {
            case EquipPropertyType.Base:
                colorType = ColorType.JZRY_Tips_Light;
                break;
            case EquipPropertyType.Additive:
                colorType = ColorType.JZRY_Tips_Blue;
                break;
            case EquipPropertyType.Advanced:
                colorType = ColorType.Orange;
                break;
            case EquipPropertyType.Mosaic:
                colorType = ColorType.JZRY_Tips_Light;
                break;
        }
        return colorType;
    }

    /// <summary>
    /// 是否养成标识满足growType
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="growType"></param>
    /// <returns></returns>
    public static bool IsEquipGrowMaskMatchType(uint mask,EquipGrowType growType)
    {
        return ((mask & (uint)growType) != 0);
    }

    public static bool CanPosStrengthen(GameCmd.EquipPos pos)
    {
        if (pos != GameCmd.EquipPos.EquipPos_SoulOne 
            && pos != GameCmd.EquipPos.EquipPos_SoulTwo)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Exchange(兑换)

    /// <summary>
    /// 生成兑换分类ID
    /// </summary>
    /// <param name="fCateID">一级分类</param>
    /// <param name="sCateID">二级</param>
    /// <returns></returns>
    public static uint BuildExchangeCateID(uint fCateID,uint sCateID)
    {
        return (fCateID << 16) | sCateID;
    }
    public class LocalExchangeDB
    {
        #region define
        public class LocalExchangeDBCompare : IComparer<uint>
        {
            public int Compare(uint left, uint right)
            {
                table.EquipExchangeDataBase leftDB 
                    = GameTableManager.Instance.GetTableItem<table.EquipExchangeDataBase>(left);
                table.EquipExchangeDataBase rightDB
                    = GameTableManager.Instance.GetTableItem<table.EquipExchangeDataBase>(right);
                if (null == leftDB || null == rightDB)
                    return 0;
                return (int)leftDB.sortID - (int)rightDB.sortID;
            }
        }
        #endregion
        #region property
        public uint exchangeID;
        public uint ExchangeID
        {
            get
            {
                return exchangeID;
            }
        }

        public table.EquipExchangeDataBase BaseDB
        {
            get
            {
                return (exchangeID != 0) ?
                    GameTableManager.Instance.GetTableItem<table.EquipExchangeDataBase>(exchangeID) : null;
            }
        }

        public uint FCateID
        {
            get
            {
                return (null != BaseDB) ? BaseDB.fCateID : 0;
            }
        }

        public string FCateName
        {
            get
            {
                return (null != BaseDB) ? BaseDB.fCateName : string.Empty;
            }
        }

        public uint SCateID
        {
            get
            {
                return (null != BaseDB) ? BaseDB.sCateID : 0;
            }
        }

        public string SCateName
        {
            get
            {
                return (null != BaseDB) ? BaseDB.sCateName : string.Empty;
            }
        } 

        public uint TargetID
        {
            get
            {
                return (null != BaseDB) ? BaseDB.targetID : 0;
            }
        }
        public uint TargetNum
        {
            get
            {
                return (null != BaseDB) ? BaseDB.targetNum : 0;
            }
        }

        public uint CostID
        {
            get
            {
                return (null != BaseDB) ? BaseDB.costID : 0;
            }
        }

        public uint CostNum
        {
            get
            {
                return (null != BaseDB) ? BaseDB.costNum : 0;
            }
        }

        public uint SorID
        {
            get
            {
                return (null != BaseDB) ? BaseDB.sortID : 0;
            }
        }
        #endregion

        #region Create
        private LocalExchangeDB (uint exchangeID)
        {
            this.exchangeID = exchangeID;
        }
        public static LocalExchangeDB Create(uint exchangeID)
        {
            return new LocalExchangeDB(exchangeID);
        }
        #endregion
    }
    #endregion
}