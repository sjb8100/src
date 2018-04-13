using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    public enum EntityProp
    {
        Begin = 0,
        /// <summary>
        /// baseid
        /// </summary>
        BaseID = Begin,             // 模板ID baseid
        EntityState ,               // 物件的状态
        End,
    
    }

    // 可以进入场景的对象
    public enum WorldObjProp
    {
        Begin = EntityProp.End,
        [System.ComponentModel.Description("移动速度")]
        MoveSpeed  = Begin,             // 移动速度
        End,
    }

    public enum CreatureProp
    {
        [System.ComponentModel.Description("等级")]
        Begin = WorldObjProp.End,
        [System.ComponentModel.Description("等级")]
        Level = Begin,                     // 等级 
        [System.ComponentModel.Description("头像")]
        Face,                              // 头像 
        [System.ComponentModel.Description("血量")]      
        Hp,                                // 血量
        [System.ComponentModel.Description("血量最大值")]
        MaxHp,                             // 血量最大值
        [System.ComponentModel.Description("魔法值")]
        Mp,                                // 魔法值
        [System.ComponentModel.Description("魔法最大值")]
        MaxMp,                             // 魔法最大值
        [System.ComponentModel.Description("阵营")]
        Camp,
        //[System.ComponentModel.Description("氏族id")]
        //ClanId,
        [System.ComponentModel.Description("氏族id高位")]
        ClanIdHigh,
        [System.ComponentModel.Description("氏族id低位")]
        ClanIdLow,

        End,
    }

    // 战斗生物
    /// <summary>
    /// 
    /// </summary>
    public enum FightCreatureProp
    {
        Begin = CreatureProp.End,
        [System.ComponentModel.Description("力量")]
        Strength = Begin,                         // 力量
        [System.ComponentModel.Description("体质")]
        Corporeity,                               // 体质
        [System.ComponentModel.Description("智力")]
        Intelligence,                             // 智力
        [System.ComponentModel.Description("精神")]
        Spirit,                                   // 精神
        [System.ComponentModel.Description("敏捷")]
        Agility,                                  // 敏捷
        [System.ComponentModel.Description("最小物攻")]
        PhysicsAttack,                            // 最小物攻
        [System.ComponentModel.Description("最小物防")]
        PhysicsDefend,                            // 最小物防
        [System.ComponentModel.Description("最小法攻")]
        MagicAttack,                              // 最小法攻
        [System.ComponentModel.Description("最小法防")]
        MagicDefend,                              // 最小法防
        [System.ComponentModel.Description("最大物攻")]
        MaxPhysicsAttack,                            // 最大物攻
        [System.ComponentModel.Description("最大物防")]
        MaxPhysicsDefend,                            // 最大物防
        [System.ComponentModel.Description("最大法攻")]
        MaxMagicAttack,                              // 最大法攻
        [System.ComponentModel.Description("最大法防")]
        MaxMagicDefend,                              // 最大法防
        [System.ComponentModel.Description("物理致命一击")]
        PhysicsCrit,                              // 物理致命一击
        [System.ComponentModel.Description("法术致命一击")]
        MagicCrit,                                // 法术致命一击
        //[System.ComponentModel.Description("物理致命抵抗")]
        //PhysicsCritDefend,                        // 物理致命抵抗 ??
        //[System.ComponentModel.Description("法术致命抵抗")]
        //MagicCritDefend,                          // 法术致命抵抗 ??
        [System.ComponentModel.Description("命中")]
        Hit,                                      // 命中
        [System.ComponentModel.Description("闪避")]
        Dodge,                                    // 闪避
        [System.ComponentModel.Description("治疗")]
        Cure,                                     // 治疗
        [System.ComponentModel.Description("火攻")]
        FireAttack,                               // 火攻
        [System.ComponentModel.Description("冰攻")]
        IceAttack,                                // 冰攻
        [System.ComponentModel.Description("电攻")]
        EleAttack,                                // 电攻
        [System.ComponentModel.Description("暗攻")]
        WitchAttack,                              // 暗攻
        [System.ComponentModel.Description("火防")]
        FireDefend,                               // 火防
        [System.ComponentModel.Description("冰防")]
        IceDefend,                                // 冰防
        [System.ComponentModel.Description("电防")]
        EleDefend,                                // 电防
        [System.ComponentModel.Description("暗防")]
        WitchDefend,                              // 暗防
        [System.ComponentModel.Description("战斗力")]
        Power,                                    // 战斗力
        [System.ComponentModel.Description("物伤吸收")]
        PhysicsAbsorb,
        [System.ComponentModel.Description("法伤吸收")]
        MagicAbsorb,
        [System.ComponentModel.Description("伤害加深")]
        HarmDeepen,
        [System.ComponentModel.Description("伤害吸收")]
        HarmAbsorb,
        [System.ComponentModel.Description("暴击伤害系数")]
        CritiRatio, 
        [System.ComponentModel.Description("战斗生物属性结束")]
        End,                                      // 战斗生物属性结束
    }

    public enum PlayerProp
    {
        [System.ComponentModel.Description("职业")]
        Begin = FightCreatureProp.End,
        [System.ComponentModel.Description("职业")]
        Job = Begin,              // 职业
        [System.ComponentModel.Description("性别")]
        Sex,                                 // 性别
        [System.ComponentModel.Description("阵营")]
        Country,                             // 国家（阵营）
        [System.ComponentModel.Description("绑元")]
        Money,                               // 游戏金币(游戏产出)
        [System.ComponentModel.Description( "金币" )]
        Coupon,                                // 代充值币 （系统赠送）
        [System.ComponentModel.Description("元宝")]
        Cold,                                // 充值币
        [System.ComponentModel.Description("积分")]
        Score,                                
        [System.ComponentModel.Description("声望")]
        Reputation,
        [System.ComponentModel.Description("战勋")]
        CampCoin,
        [System.ComponentModel.Description("狩猎积分")]
        ShouLieScore,
        [System.ComponentModel.Description("鱼币")]
        FishingMoney,
        [System.ComponentModel.Description("银两")]
        YinLiang,
        [System.ComponentModel.Description("经验")]
        Exp,                                 // 经验
        [System.ComponentModel.Description("pk模式")]
        PkMode,                              // pk模式
        [System.ComponentModel.Description("pk值")]
        PKValue,
        [System.ComponentModel.Description("恶人状态")]
        GoodNess,
        RideBaseId,                            //坐骑baseID
        TransModelResId,                        //变身资源id 
        LoginOutTime,                        // 最近一次下线时间
        [System.ComponentModel.Description("Vip等级")]
        Vip,
        [System.ComponentModel.Description( "主角标志位" )]
        StateBit,
        [System.ComponentModel.Description("称号id")]
        TitleId,
        [System.ComponentModel.Description("神魔等级")]
        GodLevel,
        [System.ComponentModel.Description("成就")]
        AchievePoint,
        [System.ComponentModel.Description("技能形态")]
        SkillStatus,
         [System.ComponentModel.Description("经验加成")]
        ExpAddBuff,
        End,
    }

    public enum RobotProp
    {
        [System.ComponentModel.Description("职业")]
        Begin = FightCreatureProp.End,
        [System.ComponentModel.Description("职业")]
        Job = Begin,
        [System.ComponentModel.Description("性别")]
        Sex,

        End,
    }
    public enum MonsterProp
    {
        Begin = CreatureProp.End,
        //MonsterProp_Money = MonsterProp_Begin,          // 游戏金币
        //MonsterProp_Gold,                               // 充值币

        End,
    }

    public enum NPCProp
    {
        Begin = CreatureProp.End,
        [System.ComponentModel.Description("职业")]
        Job = Begin,                         // 职业
        [System.ComponentModel.Description("性别")]
        Sex,                                 // 性别
        ArenaNpcType,                        // 竞技场NPC类别
        Masterid,
        SuitID,                              //时装id
        [System.ComponentModel.Description("技能形态")]
        SkillStatus,                        // 技能形态
        MasterType,                         //归属者类型
        End,
    }

    public enum ItemProp
    {
        Begin = EntityProp.End,
        Grice = Begin,                // 价格

        End,
    }

    public enum BoxProp
    {
        Begin = WorldObjProp.End,
        Number = Begin,                 // 数量
        OwnerType,                      // 拥有者类型
        //Owner,                          // 拥有者ID

        OwnerHigh,                      // 拥有者ID高位
        OwnerLow,                       // 拥有者ID低位

        End,
    }

    public enum PetProp
    {
        Begin = FightCreatureProp.End ,
        Job = Begin ,              // 职业
        [System.ComponentModel.Description( "力量天赋" )]
        StrengthTalent ,                       // 力量天赋
        [System.ComponentModel.Description( "体质天赋" )]
        CorporeityTalent ,                               // 体质
        [System.ComponentModel.Description( "智力天赋" )]
        IntelligenceTalent ,                             // 智力
        [System.ComponentModel.Description( "精神天赋" )]
        SpiritTalent ,                                   // 精神
        [System.ComponentModel.Description( "敏捷天赋" )]
        AgilityTalent ,                                  // 敏捷
        Sex ,                                 // 性别
        Country ,                             // 国家（阵营）
        LevelExp ,                            // 升级经验
        PkMode ,                              // pk模式
        PetType,                              //类型
        PetGuiYuanStatus,                     //宠物归元状态
        Character,                            //宠物性格
        YinHunExp,                            //引魂经验
        YinHunLevel,                          //引魂等级
        Life,                                 //寿命
        BaseID,                               //baseid
        MaxPoint,
        CommonJieBianLv,                      //替换前劫变
        AdvaceJieBianLv,                      //替换后劫变
        End ,
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 家园对象
    public enum HomeProp
    {
        Begin = WorldObjProp.End,
        State = Begin,
        End,
    }
    // 植物
    public enum PlantProp
    {
        Begin = HomeProp.End,
        // 添加其它属性
        End,
    }

    // 动物
    public enum AnimalProp
    {
        Begin = HomeProp.End,
        // 添加其它属性
        End,
    }

    // 许愿树
    public enum TreeProp
    {
        Begin = HomeProp.End,
        // 添加其它属性
        End,
    }

    // 矿产
    public enum MineralsProp
    {
        Begin = HomeProp.End,
        // 添加其它属性
        End,
    }

    // 土地
    public enum SoilProp
    {
        Begin = HomeProp.End,
        // 添加其它属性
        End,
    }

    // 木偶（玩家）
    public enum PuppetProp
    {
        Begin = WorldObjProp.End,
        // 添加其它属性
        [System.ComponentModel.Description("职业")]
        Job = Begin,                        // 职业
        [System.ComponentModel.Description("性别")]
        Sex,                                // 性别
        End,
    }
}
