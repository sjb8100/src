using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum ItemType
{
    ItemType_None = 0,		/**< 无类型*/
    ItemType_TujiE = 1,		/**< 突击攻击装备 战士*/
    ItemType_TegongE = 2,		/**< 特工攻击装备 刺客*/
    ItemType_JujiE = 3,		/**< 狙击攻击装备*/
    ItemType_BaopoE = 4,		/**< 爆破攻击装备*/
    ItemType_WuNiangE = 5,		/**< 舞娘攻击装备 法师*/
    ItemType_YiShengE = 6,		/**< 医生攻击装备 牧师*/
    ItemType_Weapon = 7,		/**< 所有武器装备*/
    ItemType_Necklace = 11,		/**< 项链*/
    ItemType_Medal = 12,		/**< 勋章*/
    ItemType_Ring = 13,		/**< 戒指*/
    ItemType_Cloth = 14,		/**< 衣服*/
    ItemType_Hat = 15,		/**< 帽子*/
    ItemType_Shoes = 16,		/**< 鞋子*/
    ItemType_Glove = 17,		/**< 护腕*/
    ItemType_Bracelet = 18,		/**< 手镯*/
    ItemType_Belt = 19,		/**< 腰带*/
    ItemType_SkillBook = 20,		/**< 技能书*/
    ItemType_Chart = 21,		/**< 图纸*/
    ItemType_Machine = 22,		/**< 仪器*/
    ItemType_Drug = 23,		/**< 状态药品*/
    ItemType_PitchTool = 24,		/**< 采集工具*/
    ItemType_Suit = 25,		/**< 时装 */
    ItemType_Stone = 26,		/**< 宝石*/
    ItemType_NormalMate = 27,		/**< 普通材料*/
    ItemType_TimeMate = 28,		/**< 时效材料*/
    ItemType_Pack = 29,		/**< 背包*/
    ItemType_Function = 30,		/**< 功能道具*/
    ItemType_Medicine = 31,		/**< 恢复药剂 */
    ItemType_LeachStar = 32,		///< 萃取液
    ItemType_DropSkill = 33,		/**< 对技能按键使用道具 */
    ItemType_HuodongMate = 34,		/**< 活动材料 */
    ItemType_Task = 35,		/**< 任务道具 */
    ItemType_JudgeStone = 36,		/**< 鉴定宝石 */
    ItemType_LuckyStone = 37,		/**< 幸运石 */
    ItemType_Bind = 38,		/**< 绑定道具 */
    ItemType_CommonMate = 39,		/**< 普通材料*/
    ItemType_Mend = 40,		/**< 修理道具*/
    ItemType_Demon = 41,		/**< 骑异兽*/
    ItemType_Demon_Medicien = 42,		/**< 奇异兽药品*/
    ItemType_LotteryTicket = 43,		/**< 彩票 */
    ItemType_Task2 = 44,		/**< 任务道具2，名称暂定，对NPC使用 */
    ItemType_Demon_Tool = 45,		/**< 奇异兽玩具*/
    ItemType_TaskCompose = 46,		/**< 任务合成道具，不可使用*/
    ItemType_TaskCompose2 = 47,		/**< 任务合成道具，可使用*/
    ItemType_MoneyBox = 48,		/**< 物资箱*/
    ItemType_Task3 = 49,		/**< 一种卷轴 */
    ItemType_StarCube = 51,		/**< 星星合金 */
    ItemType_BossFood = 52,		/**< 团养殖食物*/
    ItemType_Seed = 53,		/**< 种子*/
    ItemType_LifeEquip = 54,		/**< 生命存储仪*/
    ItemType_BossCard = 55,		/**< BOSS卡片*/
    ItemType_ColorItem = 56,		/**< 装备染色剂*/
    ItemType_Horse = 57,		/**< 马匹*/
    ItemType_Horse_Equip = 58,		/**< 马匹装备*/
    ItemType_Face = 59,		/**< 脸型*/
    ItemType_Hair = 60,		/**< 发型*/
    ItemType_Back = 61,		/**< 背部装备*/
    ItemType_RoleSell = 62,		/**< 角色流转a*/
    ItemType_RoleRentOut = 63,		/**< 代练交易道具*/
    ItemType_RoleRentIn = 64,		/**< 代练回收道具*/
    ItemType_Douqishi = 65,		/**< 斗气石*/
    ItemType_FishingRod = 66,		/**< 鱼竿*/
    ItemType_Horse_Equip1 = 67,		/**< 马匹装备 盾纹章*/
    ItemType_Horse_Equip2 = 68,		/**< 马匹装备 铠纹章*/
    ItemType_Horse_Equip3 = 69,		/**< 马匹装备 左翼纹章*/
    ItemType_Horse_Equip4 = 70,		/**< 马匹装备 右翼纹章*/
    ItemType_Horse_Equip5 = 71,		/**< 马匹装备 血纹章*/
    ItemType_Suit_Female = 72,		/**< 女性时装*/
    ItemType_Suit_Male = 73,		/**< 男性时装*/
    ItemType_Horse_Suit = 74,		/**< 马匹外形*/
    ItemType_Mobile_Card = 75,		/**< 充值卡*/
    ItemType_Horse_Soul = 76,		/**< 骑魂*/
    ItemType_Wing_Soul = 77,		/**< 羽灵*/
    ItemType_Wing_Spirit = 78,		/**< 翼魂*/
    ItemType_Wing_Oracle = 79,		/**< 神谕*/
    ItemType_FootBall = 80,		/**< 足球*/
    ItemType_Brand = 81,		/**< 传奇烙印*/
    ItemType_Rock = 82,		/**< 抗性水晶*/
    ItemType_Demon_SkillCard = 83,		/**< 宠物技能卡*/
    ItemType_Demon_Model = 84,		/**< 宠物幻化道具*/
    ItemType_Sacred_Pdam = 85,		/**< 圣石物攻*/
    ItemType_Sacred_Mdam = 86,		/**< 圣石魔攻*/
    ItemType_Sacred_Pdef = 87,		/**< 圣石物防*/
    ItemType_Sacred_Mdef = 88,		/**< 圣石魔防*/
    ItemType_Sacred_Maxhp = 89,		/**< 圣石生命*/
    ItemType_Horse_Stone_Dam = 90,		/**< 攻击玉髓*/
    ItemType_Horse_Stone_Pdef = 91,		/**< 物防玉髓*/
    ItemType_Horse_Stone_Mdef = 92,		/**< 魔防玉髓*/
    ItemType_Horse_Stone_Maxhp = 93,		/**< 生命玉髓*/
    ItemType_Hammer = 95,		/**< 圣诞活动砸彩蛋的锤头*/
    ItemType_ShenbingHat = 96,		/**< 神兵部件-头部*/
    ItemType_ShenbingClothP = 97,		/**< 神兵部件-护甲(物)*/
    ItemType_ShenbingMedal = 98,		/**< 神兵部件-勋章*/
    ItemType_ShenbingGlove = 99,		/**< 神兵部件-护腕*/
    ItemType_ShenbingRing = 100,		/**< 神兵部件-戒指*/
    ItemType_ShenbingBelt = 101,		/**< 神兵部件-腰带*/
    ItemType_ShenbingShoes = 102,		/**< 神兵部件-鞋子*/
    ItemType_ShenbingNecklace = 103,		/**< 神兵部件-项链*/
    ItemType_ShenbingUnder = 104,		/**< 神兵部件-内衣*/
    ItemType_ShenbingSoldier = 105,		/**< 神兵部件-战士武器*/
    ItemType_ShenbingMaster = 106,		/**< 神兵部件-法师武器*/
    ItemType_ShenbingAssassin = 107,		/**< 神兵部件-刺客武器*/
    ItemType_ShenbingMinister = 108,		/**< 神兵部件-牧师武器*/
    ItemType_ShenbingClothM = 109,       /**< 神兵部件-护甲(魔)*/
    ItemType_ShenbingGun = 110,		 /**< 神兵部件-枪手武器*/
    ItemType_ShenbingCannon = 111,		 /**< 神兵部件-魔炮武器*/
    ItemType_Title = 113,  /**头顶称号**/
}

public enum itemBind
{
    unbingState = 0,
    bindState,
}
public enum itemColor
{
    White = 0,
    Blue = 1,
    Yellow = 2,
    Green = 3,
    Purple = 4,

}

/// <summary>
/// 对就图标对照表的类型
/// </summary>
public enum IconContrast
{
    IconItem = 1,
    IconSkill,
    IconBuff,
    IconBuy,
}

/// <summary>
/// 成就分类枚举
/// </summary>
public enum ACHIEVEMENTTYPE
{
    Type_NULL = 0,
    /// <summary>
    /// 总览
    /// </summary>
    Type_Pandect = 1,
    /// <summary>
    /// 综合
    /// </summary>
    Type_Synthesize = 2,
    /// <summary>
    /// 上下两种类型的分隔类型，以区分UI显示
    /// </summary>
    Separate,
    Type_Ability = 10,
    Type_Growth = 11,
    Type_Battle = 12,
    Type_Sociation = 13,
}

public class Layer
{
    public readonly int UI = 5;
    public readonly int ShowModel = 6;
}
public enum PLAYERPKMODEL
{
    [System.ComponentModel.Description("无效")]
    PKMODE_M_NONE = 0,
    [System.ComponentModel.Description("和平")]
    PKMODE_M_NORMAL = 1,
    [System.ComponentModel.Description("自由")]
    PKMODE_M_FREEDOM = 2,
    [System.ComponentModel.Description("团队")]
    PKMODE_M_TEAM = 3,
    [System.ComponentModel.Description("氏族")]
    PKMODE_M_FAMILY = 4,
    [System.ComponentModel.Description("阵营")]
    PKMODE_M_CAMP = 5,
    [System.ComponentModel.Description("善恶")]
    PKMODE_M_JUSTICE = 6,
}

/// <summary>
/// 区域类型
/// </summary>
public enum MapAreaType
{
    [System.ComponentModel.Description("普通区")]
    Normal = 0,
    [System.ComponentModel.Description("对战区")]
    Battle = 1,
    [System.ComponentModel.Description("安全区")]
    Safe = 2,
    [System.ComponentModel.Description("阻挡区")]
    Block = 3,
    [System.ComponentModel.Description("竞技区")]
    PK = 4,
    [System.ComponentModel.Description("BOSS区")]
    Boss = 8,
    [System.ComponentModel.Description("钓鱼区")]
    Fish = 34,
}

public enum TaskSubType
{
    Talk=1,
    Collection = 2,
    KillMonster = 3,
    KillMonsterCollect = 4,//杀怪收集
    UseItem = 5,//使用道具
    DeliverItem = 6,//递交道具
    Convoy = 7,//护送
    ChangeBody = 8,//变身
    CallMonster = 9,//使用道具召唤怪物
    SubmitLimit = 10,//提交等级限制
    Guild = 11,     //引导任务
}

/// <summary>
/// 阵营类型
/// </summary>
public enum CampType
{
    Firendly,//友方
    Enemy,//敌方
    FriendlyBody,//友方尸体
}

