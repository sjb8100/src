//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using System;
using System.ComponentModel;


public enum LocalTextType
{
	LocalText_None = 0,
	//当前角色已经在删除倒计时中
	Login_Server_InDelete =23005,
	//{0}包含敏感词{1}
	Local_TXT_Warning_FM_Sensitive =300000,
	//{0}包含非法字符{1}
	Local_TXT_Warning_FM_IllegalChar =300001,
	//{0}不能为空
	Local_TXT_Warning_FM_Empty =300002,
	//{0}存在非法颜色字符{1}
	Local_TXT_Warning_FM_NGUIColor =300003,
	//{0}不能为纯数字
	Local_TXT_Warning_FM_Digital =300004,
	//{0}名称长度应在{1}字内
	Local_TXT_Warning_FM_NameLengthLimit =300005,
	//角色名称已存在!
	Local_TXT_Warning_NameExist =300006,
	//蛮武
	Local_TXT_Profession_Soldier =300007,
	//百草
	Local_TXT_Profession_Spy =300008,
	//弓手
	Local_TXT_Profession_Gunman =300009,
	//魔炮
	Local_TXT_Profession_Blast =300010,
	//龙灵
	Local_TXT_Profession_Freeman =300011,
	//巫魅
	Local_TXT_Profession_Doctor =300012,
	//公告
	Local_TXT_Announcement =300013,
	//人物
	Local_TXT_Person =300014,
	//氏族
	Local_TXT_Clan =300015,
	//{0}等级{1}
	Local_TXT_FM_Lv =300016,
	//名称
	Local_TXT_Name =300017,
	//学习
	Local_TXT_Learn =300018,
	//研发
	Local_TXT_Dev =300019,
	//{0}下一等级需要{1}
	Local_TXT_NextLvNeed =300020,
	//已升至最高等级
	Local_TXT_UpgradeMax =300021,
	//在线礼包
	Local_TXT_WelfareOnline =300022,
	//七天福利
	Local_TXT_Welfare7Day =300023,
	//等级礼包
	Local_TXT_WelfareLeve =300024,
	//开服礼包
	Local_TXT_WelfareSever =300025,
	//月签
	Local_TXT_WelfareMonth =300026,
	//主角等级
	Local_TXT_Welfare7Day_1 =300027,
	//累计在线
	Local_TXT_Welfare7Day_2 =300028,
	//装备档次
	Local_TXT_Welfare7Day_3 =300029,
	//装备精炼
	Local_TXT_Welfare7Day_4 =300030,
	//珍兽等级
	Local_TXT_Welfare7Day_5 =300031,
	//珍兽携带等级
	Local_TXT_Welfare7Day_6 =300032,
	//竞技场
	Local_TXT_Welfare7Day_7 =300033,
	//圣魂星级
	Local_TXT_Welfare7Day_8 =300034,
	//战斗力
	Local_TXT_Welfare7Day_9 =300035,
	//打折礼包
	Local_TXT_Welfare7Day_10 =300036,
	//宝石总等级
	Local_TXT_Welfare7Day_11 =300037,
	//圣魂等级
	Local_TXT_Welfare7Day_12 =300038,
	//活动总进度
	Local_TXT_Welfare7Day_13 =300039,
	//主
	Local_TXT_TaskTitle_Main =300040,
	//支
	Local_TXT_TaskTitle_Sub =300041,
	//日常
	Local_TXT_TaskTitle_Daily =300042,
	//氏族
	Local_TXT_TaskTitle_Clan =300043,
	//本月已签到{0}天
	Local_TXT_FM_WelfareMonth_Sign =300044,
	//本月当前你可补签{0}天
	Local_TXT_FM_WelfareMonth_ReSign =300045,
	//活动剩余时间：
	Local_TXT_NewServerRemainTime =300046,
	//升级成功
	Local_TXT_Notice_Upgrade =300047,
	//氏族信息错误
	Local_TXT_Notice_ClanError =300048,
	//玩家氏族信息错误
	Local_TXT_Notice_ClanPlayerError =300049,
	//玩家职位信息错误
	Local_TXT_Notice_ClanPlayerDutyError =300050,
	//你没有操作权限
	Local_TXT_Notice_NoOpRight =300051,
	//你还没有加入氏族
	Local_TXT_Notice_NotInClan =300052,
	//最多可以输入{0}个字,还剩{1}个字
	Local_TXT_InputLimit =300053,
	//公告因该在50个字内
	Local_TXT_ClanGGLimit =300054,
	//氏族名称长度应在2-6个字内
	Local_TXT_ClanNameLimit =300055,
	//今日任务已做完
	Local_TXT_TodayTaskComplete =300056,
	//修改成功
	Local_TXT_Notice_UpdateSuccess =300057,
	//接受
	Local_TXT_Accept =300058,
	//提交
	Local_TXT_Submit =300059,
	//进行中
	Local_TXT_InProgress =300060,
	//前往
	Local_TXT_HeadFor =300061,
	//今日可完成{0}次
	Local_TXT_TodayCompleteLimit =300062,
	//奖励
	Local_TXTAward =300063,
	//已装备
	Local_TXT_Equipment =300064,
	//背包
	Local_TXT_Knapsack =300065,
	//攻击
	Local_TXT_Attack =300066,
	//防御
	Local_TXT_Defend =300067,
	//法术攻击
	Local_TXT_Mattack =300068,
	//法术防御
	Local_TXT_Mdefend =300069,
	//物理攻击
	Local_TXT_Pattack =300070,
	//物理防御
	Local_TXT_Pdefend =300071,
	//生命提升
	Local_TXT_HpPromote =300072,
	//经验加成
	Local_TXT_ExpAdd =300073,
	//头盔
	Local_TXT_Hat =300074,
	//护肩
	Local_TXT_Shoulder =300075,
	//胸甲
	Local_TXT_Coat =300076,
	//护腿
	Local_TXT_Leg =300077,
	//戒指
	Local_TXT_Adornl =300078,
	//副手
	Local_TXT_Shield =300079,
	//武器
	Local_TXT_Equip =300080,
	//鞋子
	Local_TXT_Shoes =300081,
	//手腕
	Local_TXT_Cuff =300082,
	//腰部
	Local_TXT_Belf =300083,
	//披风
	Local_TXT_Capes =300084,
	//项链
	Local_TXT_Necklace =300085,
	//圣魂
	Local_TXT_Soul =300086,
	//官印
	Local_TXT_Office =300087,
	//可升级
	Local_TXT_Can_Upgrade =300088,
	//可镶嵌
	Local_TXT_Can_Inlay =300089,
	//角色{0}级
	Local_TXT_FM_CharacterLv =300090,
	//合成后宝石使用等级将超过主角等级,是否继续？
	Local_TXT_InlayGemUpgradeNotice =300091,
	//继续
	Local_TXT_Continue =300092,
	//取消
	Local_TXT_Cancel =300093,
	//确定
	Local_TXT_Confirm =300094,
	//提示
	Local_TXT_Tips =300095,
	//合成费用为{0},是否继续?
	Local_TXT_FM_ComposeCostNotice =300096,
	//金币
	Local_TXT_Gold =300097,
	//绑元
	Local_TXT_Penney =300098,
	//元宝
	Local_TXT_YuanBao =300099,
	//声望
	Local_TXT_Rep =300100,
	//资金
	Local_TXT_ZJ =300101,
	//族贡
	Local_TXT_ZG =300102,
	//云游
	Local_TXT_Mall_YunYou =300103,
	//积分
	Local_TXT_JiFen =300104,
	//{0}金币/次({1})
	Local_TXT_FM_Startask_money =300105,
	//{0}绑元/次
	Local_TXT_FM_Startask_coin =300106,
	//服饰
	Local_TXT_Clothes =300107,
	//背饰
	Local_TXT_BackShow =300108,
	//奇兵
	Local_TXT_Jones =300109,
	//萌宠
	Local_TXT_SweetPet =300110,
	//脸饰
	Local_TXT_FaceShow =300111,
	//图鉴
	Local_TXT_TJ =300112,
	//已拥有
	Local_TXT_Ower =300113,
	//属性
	Local_TXT_Attr =300114,
	//称号
	Local_TXT_Title =300115,
	//时装
	Local_TXT_Fashion =300116,
	//丢弃
	Local_TXT_Discard =300117,
	//续费
	Local_TXT_Renew =300118,
	//获得
	Local_TXT_Buy =300119,
	//装备
	Local_TXT_Wear =300120,
	//卸下
	Local_TXT_Unload =300121,
	//领取
	Local_TXT_LingQu =300122,
	//获取
	Local_TXT_Get =300123,
	//活跃度不足,无法领取
	Local_TXT_Notice_NoEnoughLiveness =300124,
	//奖励已领取
	Local_TXT_Notice_AlreadyOpenBox =300125,
	//获得经验x{0}
	Local_TXT_Notice_GetExp =300126,
	//获得金币x{0}
	Local_TXT_Notice_GetJinQian =300127,
	//获得绑元x{0}
	Local_TXT_Notice_GetWenQian =300128,
	//获得道具{0}x{1}
	Local_TXT_Notice_GetItem =300129,
	//切换形态需要达到32级
	Local_TXT_Notice_CannotChangeSkillState =300130,
	//矿石可收获
	Local_TXT_Notice_HomeMineCanGain =300131,
	//未分配任何潜修点
	Local_TXT_Notice_NoAllotQianXiuPoint =300132,
	//重置成功
	Local_TXT_Notice_ResetPointOver =300133,
	//分配成功
	Local_TXT_Notice_AllotPointOver =300134,
	//已无可重置的潜修点
	Local_TXT_Notice_AlreadyResetPoint =300135,
	//剩余潜修点不足
	Local_TXT_Notice_NoEnoughQianXiuPoint =300136,
	//剩余传送次数不足
	Local_TXT_Notice_NoEnoughTransmitTime =300137,
	//失败等级不归0
	Local_TXT_forging_Refine_Tgdes =300138,
	//失败等级不变
	Local_TXT_forging_Refine_Xydes =300139,
	//合成机会额外+1
	Local_TXT_forging_Compound_Lhdes =300140,
	//合成机会额外+2
	Local_TXT_forging_Compound_Cldes =300141,
	//{0}
	Local_TXT_FM_Tips_UnlockActivation =300142,
	//[AAAAAAFF]{0}
	Local_TXT_FM_Tips_UnLockInActivation =300143,
	//[AAAAAAFF]未镶嵌
	Local_TXT_Tips_NoneActivation =300144,
	//[AAAAAAFF]未镶嵌
	Local_TXT_Tips_NoneInActivation =300145,
	//[AAAAAAFF]{0}级开启
	Local_TXT_Tips_LockActivation =300146,
	//[AAAAAAFF]{0}级开启
	Local_TXT_Tips_LockInActivation =300147,
	//[FFF8DDFF]主角达到[-][FF2D2DFF]{0}级
	Local_TXT_FM_Trailer_Level =300148,
	//完成任务\n[FF2D2DFF]{0}
	Local_TXT_FM_Trailer_Task =300149,
	//获得称号{0}
	Local_TXT_Notice_GetTitle =300150,
	//1、对氏族宣战后,该氏族将被加入敌对氏族{0}小时（可累加）\n2、击杀处于敌对氏族列表中的成员不会增加PK值\n3、敌对氏族成员名字将会标记为粉红色\n4、使用阵营模式可以攻击敌对氏族玩家,而不会误伤其他玩家\n5、选战需要消耗氏族资金{1},族贡{2}
	Local_TXT_Clan_DeclareDes =300151,
	//[AAAAAAFF]注：[00FF16FF]绿色[AAAAAAFF]属性融合后保留；[FF0000FF]红色[AAAAAAFF]属性融合后剔除
	Local_TXT_Muhon_BlendBlendTips =300152,
	//{0}使用等级:{1}
	Local_TXT_Mall_UselevelDescribe =300153,
	//{0}开启
	Local_TXT_Notice_Daily_Begin =300154,
	//参加
	Local_TXT_Notice_Daily_Join =300155,
	//已完成
	Local_TXT_Notice_Daily_Finish =300156,
	//已接
	Local_TXT_Notice_Daily_Get =300157,
	//成就
	Local_TXT_Notice_Achieve_Title =300158,
	//充值
	Local_TXT_Notice_Recharge_Title =300159,
	//连续{0}天\n每天可返
	Local_TXT_Notice_Noble_EveryDayReturn =300160,
	//有效期:{0}天{1}小时
	Local_TXT_Notice_Noble_avaiTimeDayAndHour =300161,
	//有效期:{0}天
	Local_TXT_Notice_Noble_avaiTimeDay =300162,
	//有效期:{0}小时
	Local_TXT_Notice_Noble_avaiTimeHour =300163,
	//您想以￥{0}的价格购买一个{1}元宝吗?
	Local_TXT_Notice_Recharge_RechargeNum =300164,
	//每日兑换限额：{0}元宝
	Local_TXT_Notice_Exchange_DayLimit =300165,
	//[808080]无附加属性
	Local_TXT_Soul_Attribute =300166,
	//未激活,可激活
	Local_TXT_Soul_Activate =300167,
	//[FFFF00]圣魂激活可随机获得一条附加属性
	Local_TXT_Soul_Describe1 =300168,
	//已激活,可解除
	Local_TXT_Soul_Relieve =300169,
	//[FFFF00]勾选需解除的附加属性
	Local_TXT_Soul_Describe2 =300170,
	//今日可购{0}组
	Local_TXT_Store_Limit =300171,
	//[FFFF00]该圣魂已达到最高星级
	Local_TXT_Soul_Describe =300172,
	//获得圣魂经验x{0}
	Local_TXT_Soul_GetMuhonExpTips =300173,
	//您还未拥有圣魂
	Local_TXT_Soul_OwnNoneNotice =300174,
	//主圣魂需激活获得附加属性才可融合
	Local_TXT_Soul_ActiveTips =300175,
	//该圣魂没有附加属性，不可选！
	Local_TXT_Soul_UnactiveNotice =300176,
	//注：可选圣魂需已激活
	Local_TXT_Soul_ChooseActiveMuhonNotice =300177,
	//无效圣魂，请重新选择
	Local_TXT_Soul_InvalidMuhonReChoose =300178,
	//请选择需要融合的圣魂
	Local_TXT_Soul_ChooseBlendMuhonTips =300179,
	//圣魂需达到当前星级满级才可升星
	Local_TXT_Soul_MuhonNeedMaxLvTips =300181,
	//选择圣魂
	Local_TXT_Soul_ChooseMuhon =300182,
	//注：可选圣魂需当前等级满级、且已激活
	Local_TXT_Soul_ChooseMuhonNotice =300183,
	//该圣魂未达当前等级满级，不可选
	Local_TXT_Soul_NotMatchLvTips =300184,
	//该圣魂未激活且未达当前等级满级，不可选
	Local_TXT_Soul_NotMatchLvActiveTips =300185,
	//无效圣魂ID,无法进行进化操作
	Local_TXT_Soul_InvalidMuhonTips =300186,
	//请选择进化所需消耗的圣魂
	Local_TXT_Soul_ChooseEvolveMuhonTips =300187,
	//[808080]注：[-][00FF00]绿色[-][808080]属性融合后保留[-];[FF0000]红色[-][808080]属性融合后剔除[-]
	Local_TXT_Soul_BlendNotice =300188,
	//升星
	Local_TXT_Soul_UpgradeStar =300189,
	//{0}等级：{1}{2}/{3}
	Local_TXT_Soul_ColorLv =300190,
	//道具不足时，自动使用元宝
	Local_TXT_Soul_PropNotEnough =300191,
	//零星
	Local_TXT_Soul_ZeroStar =300192,
	//一星
	Local_TXT_Soul_OneStar =300193,
	//二星
	Local_TXT_Soul_TwoStar =300194,
	//三星
	Local_TXT_Soul_ThreeStar =300195,
	//四星
	Local_TXT_Soul_FourStar =300196,
	//五星
	Local_TXT_Soul_FiveStar =300197,
	//{0}圣魂升星需达{1}主角{2}级
	Local_TXT_Soul_UpgardePlayerLvNotice =300198,
	//数量：{0}/{1}
	Local_TXT_Soul_ChooseNum =300199,
	//星级：
	Local_TXT_Soul_StarLv =300200,
	//等级上限：
	Local_TXT_Soul_LvUpperLimit =300201,
	//附加属性上限：
	Local_TXT_Soul_AttrUpperLimit =300202,
	//{0}条
	Local_TXT_Soul_Num =300203,
	//升星成功
	Local_TXT_Soul_UpgradeStarSuccess =300204,
	//二星开启
	Local_TXT_Soul_TwoStarOpen =300205,
	//三星开启
	Local_TXT_Soul_ThreeStarOpen =300206,
	//四星开启
	Local_TXT_Soul_FourStarOpen =300207,
	//五星开启
	Local_TXT_Soul_FiveStarOpen =300208,
	//你当前未拥有圣魂
	Local_TXT_Soul_OweNone =300209,
	//当前等级：{0}/{1}   {2}需求等级：{3}
	Local_TXT_Soul_Level =300210,
	//修灵等级已满！
	Local_Txt_Pet_yinhunmanjineirong =300211,
	//劫变等级不影响珍兽天赋,天赋越高珍兽越强
	Local_Txt_Pet_guiyuanmanjineirong =300212,
	//成长状态越好\n天赋上限越高\n额外潜能点越多\n完美>卓越>杰出>优秀>普通
	Local_Txt_Pet_guiyuanweimanjineirong =300213,
	//已锁定技能
	Local_Txt_Pet_yisuodingjineng =300214,
	//请点击想要锁定的技能
	Local_Txt_Pet_qingdianjisuodingjineng =300215,
	//获得{0}活跃可领取以下奖励
	Local_Txt_Daily_BoxTips =300216,
	//剧情加载中
	Local_Txt_Story_juqingjiazaizhong =300217,
	//场景加载中
	Local_Txt_Story_changjingjiazaizhong =300218,
	//主角{0}级开启
	Local_Txt_Trailer_1 =300219,
	//完成任务-{0}
	Local_Txt_Trailer_2 =300220,
	//[30f130]已达成\n点击领奖
	Local_Txt_Trailer_3 =300221,
	//[30f130]点击领奖
	Local_Txt_Trailer_4 =300222,
	//[ffff00]即将解锁
	Local_Txt_Trailer_5 =300223,
	//槽位1:{0}
	Local_Txt_Set_1 =300224,
	//槽位2:{0}
	Local_Txt_Set_2 =300225,
	//槽位3:{0}
	Local_Txt_Set_3 =300226,
	//{0}级
	Local_Txt_Set_4 =300227,
	//成就点
	Local_TXT_ChengJiuDian =300232,
	//战勋
	Local_TXT_ZhanXun =300233,
	//请点击选择想要出售的物品
	Local_TXT_Sell_1 =300234,
	//猎魂
	Local_TXT_LieHun =300235,
	//所选符石档次需高于勾选属性档次
	Local_TXT_Rune_1 =300236,
	//所选符石档次不可低于勾选属性档次
	Local_TXT_Rune_2 =300237,
	//绑定有礼
	Local_TXT_BindGift =300238,
	//奖励找回
	Local_TXT_RewardFind =300239,
	//好友招募
	Local_TXT_FriendInvite =300240,
	//冲级礼包
	Local_TXT_RushLevel =300241,
	//礼包兑换
	Local_TXT_CDKey =300242,
	//鱼币
	Local_TXT_YuBi =300243,
	//集字送好礼
	Local_TXT_CollectWord =300244,
	//银两
	Local_TXT_YinLiang =300245,
	//{0}年{1}月{2}日{3}时{4}分--{5}年{6}月{7}日{8}时{9}分
	Local_TXT_WelfareScheduleStr =300246,
	//成长基金
	Local_TXT_GrowthFund =300301,
	//单日单笔充值
	Local_TXT_SingleRechargeSingleDay =300302,
	//单日累计充值
	Local_TXT_AllRechargeSingleDay =300303,
	//单日消费
	Local_TXT_AllCostSingleDay =300304,
	//累计充值
	Local_TXT_AllRecharge =300305,
	//每周回馈
	Local_TXT_AllCost =300306,
	//每日礼包
	Local_TXT_DailyGift =300307,
	//恭喜你成功购买成长基金!
	Local_TXT_BuyGrowthFundSuccess =300308,
	//活动期间，每日单笔充值达到要求可领奖
	Local_ActivityTips_SingleRechargeSingleDay =300309,
	//活动期间，每日累计充值达到要求可领奖
	Local_ActivityTips_AllRechargeSingleDay =300310,
	//活动期间，每日累计消费达到要求可领奖
	Local_ActivityTips_AllCostSingleDay =300311,
	//活动期间，累计充值达到要求可领奖
	Local_ActivityTips_AllRecharge =300312,
	//活动期间，累计消费达到要求可领奖
	Local_ActivityTips_AllCost =300313,
	//升级可领取总价[b][FFD700]{0}元宝[/b]!
	Local_ActivityTips_GrowthFund =300314,
	//温馨提示：三种皇令可叠加购买，每天最多领取[FFFF00]300[-]绑元，购买任意皇令可激活特权
	Local_TXT_Noble_1 =300315,
	//内测返利
	Local_TXT_Rebate_1 =300316,
	//感谢您积极参与内测，根据您内测期间累计充值金额以及参与等级和反馈建议BUG活动的情况。
	Local_TXT_Rebate_2 =300317,
	//您可领取的返利奖励如下
	Local_TXT_Rebate_3 =300318,
	//同一账号下只有一个角色可领取该奖励，确认领取？
	Local_TXT_Rebate_4 =300319,
	//{0}上线了
	Friend_Friend_shangxiantishi =401001,
	//{0}下线了
	Friend_Friend_xiaxiantishi =401002,
	//成功添加{0}为好友
	Friend_Friend_tianjiachenggong =401003,
	//好友数量超过上限
	Friend_Friend_haoyoushuliangchaoguoshangxian =401004,
	//该好友已删除
	Friend_Friend_haoyouyibeishanchu =401005,
	//不能添加自己为好友
	Friend_Friend_bunengtianjiaziji =401006,
	//屏蔽成功
	Friend_BlackList_pingbichenggong =401007,
	//黑名单人数达到上限
	Friend_BlackList_heimingdandadaoshangxian =401008,
	//已取消屏蔽
	Friend_BlackList_yiquxiaopingbi =401009,
	//不能屏蔽自己
	Friend_BlackList_bunengpingbiziji =401010,
	//对方已经在自己的黑名单中了
	Friend_BlackList_duifangyijingzaiheimingdanzhong =401011,
	//已删除仇敌
	Friend_Commond_yishanchuchoudi =401012,
	//移出成功
	Friend_Commond_yichuliebiaochenggong =401013,
	//对方不在线
	Friend_Commond_duifangbuzaixian =401014,
	//不能添加自己为仇人
	Friend_Commond_bunengtianjiazijiweichouren =401015,
	//对方已经在自己的仇人列表中了
	Friend_Commond_duifangyijingzaichourenliebiaozhongle =401016,
	//该仇人已经被移除
	Friend_Commond_yichuchourenchenggong =401017,
	//下单成功
	Trading_Currency_xiadanchenggong =402022,
	//取出金币X{0},绑元X{1}
	Trading_Currency_quchuzhanghuzhongdejinbiXwenqianX =402023,
	//金币不足
	Trading_Currency_jinbibuzuxiadanshibai =402024,
	//绑元不足
	Trading_Currency_wenqianbuzuxiadanshibai =402025,
	//已关注
	Trading_IsConcerned =402026,
	//已取消关注
	Trading_CancleConcerned =402027,
	//当前价格不能超过上限或者下限
	Trading_IsOutOfLimit =402028,
	//消耗了{0}金币
	Talk_World_shijiepindaofayanxiaohaojinbitishi =403001,
	//发送冷却中,请稍后再试
	Talk_Commond_fasongxiaoxiCD =403002,
	//你还没有加入氏族
	Talk_ActualTime_haiweijiarushizuwufajinruliaotianshi =403003,
	//你还没有加入队伍
	Talk_ActualTime_haiweijiaruduiwuwufajinruliaotianshi =403004,
	//您已进入氏族聊天室
	Talk_ActualTime_jinrushizuliaotianshi =403005,
	//您已进入队伍聊天室
	Talk_ActualTime_jinruduiwuliaotianshi =403006,
	//您已退出聊天室
	Talk_ActualTime_tuichusuoyouliaotianshi =403007,
	//{0}级开启氏族聊天室
	Talk_ActualTime_Xjikaiqishizuliaotianshi =403008,
	//{0}级开启队伍聊天室
	Talk_ActualTime_Xjikaiqiduiwuliaotianshi =403009,
	//召集成功
	Talk_ActualTime_zhaojichenggong =403010,
	//氏族管理员召集你进入氏族聊天室
	Talk_ActualTime_shizuzhaojixiaoxitishi =403011,
	//在聊天室中无法播放语音消息
	Talk_ActualTime_liaotianshizhongwufabofangyuyinxiaoxi =403012,
	//在聊天室中时无法发送语音消息
	Talk_ActualTime_liaotianshizhongwufafasongyuyinxiaoxi =403013,
	//主播正在赶来的路上
	Talk_ActualTime_zhubozhengzaiganlaidelushang =403014,
	//角色到达{0}级时才能在世界频道发言。
	Talk_World_LvLimit =403015,
	//你获得了<color value="#78ff00">{0}</color>经验
	Talk_System_huodejingyan =403100,
	//你获得了<color value="#78ff00">{0}</color>金币
	Talk_System_huodejinbi =403101,
	//恭喜你升到了<color value="#78ff00">{0}</color>级
	Talk_System_renwushengji =403102,
	//你的{0}获得了<color value="#78ff00">{1}</color>经验
	Talk_System_wuhunhuodejingyan =403103,
	//恭喜你的{0}升到了<color value="#78ff00">{1}</color>级
	Talk_System_wuhunshengji =403104,
	//你的{0}获得了<color value="#78ff00">{1}</color>经验
	Talk_System_zhanghunhuodejingyan =403105,
	//恭喜你的{0}升到了<color value="#78ff00">{1}</color>级
	Talk_System_zhanhunshengji =403106,
	//你击杀了玩家{0}
	Talk_System_jishawanjia =403107,
	//你被玩家{0}击杀了
	Talk_System_beiwanjiajisha =403108,
	//你获得了物品{0}
	Talk_System_huodewupin =403109,
	//你获得了{0}珍兽
	Talk_System_huodezhanghun =403110,
	//你封印了{0}珍兽
	Talk_System_fengyinzhanhun =403111,
	//你获得了{0}坐骑
	Talk_System_huodezuoqi =403112,
	//你封印了{0}坐骑
	Talk_System_fengyinzuoqi =403113,
	//你在寄售行出售了物品[{0}]
	Talk_System_jishouhangchushouwupin =403114,
	//你在寄售行购买了<color value="#78ff00">{0}</color>绑元
	Talk_System_jishouhanggoumawenqian =403115,
	//你在寄售行出售了<color value="#78ff00">{0}</color>绑元
	Talk_System_jishouhangchushouwenqian =403116,
	//你的离线时间为<color value="#78ff00">{0}</color>
	Talk_System_lixianshijian =403117,
	//你获得了<color value="#78ff00">{0}</color>绑元
	Talk_System_huodewenqian =403118,
	//该地图不支持切换到{0}模式
	Talk_System_buzhichigaimoshi =403119,
	//你获得了<color value="#78ff00">{0}</color>声望
	Talk_System_huodejianshedu =403120,
	//你获得了<color value="#78ff00">{0}</color>鱼币
	Talk_System_huodeyubi =403121,
	//您的等级不足{0}级，无法发送私聊消息
	Talk_System_NotMatchChatLevel =403122,
	//玩家【{0}】已将你加入黑名单中
	Talk_System_InOtherBlack =403123,
	//玩家【{0}】在黑名单中
	Talk_System_InMyBlack =403124,
	//加黑名单
	Talk_System_AddBlackTxt =403125,
	//移除黑名单
	Talk_System_RmoveBlackTxt =403126,
	//捐献成功
	Clan_Donate_juanxianchenggong =405001,
	//氏族名称已经存在
	Clan_Commond_shizumingchengyijingcunzai =405002,
	//25级开启氏族功能
	Clan_Commond_shizugongnengweikaiqi =405003,
	//金币不足
	Clan_Commond_tongqianbuzu =405004,
	//绑元不足
	Clan_Commond_yinyuanbuzu =405005,
	//元宝不足
	Clan_Commond_yuanbaobuzu =405006,
	//你已经加入了一个氏族
	Clan_Commond_niyijingjiaruleyigeshizu =405007,
	//拒绝加入请求
	Clan_Commond_jujujejiaruqingqiu =405008,
	//氏族已经达到了人员上限
	Clan_Commond_shizuyiman =405009,
	//已经向该氏族发出申请
	Clan_Commond_yijingfachujiarushenqing =405010,
	//你还没有加入氏族
	Clan_Commond_haiweijiarushizu =405011,
	//今日捐献次数耗尽
	Clan_Donate_juanxiancishubuzu =405012,
	//建设度不足
	Clan_Commond_zugongbuzu =405013,
	//氏族资金不足
	Clan_Commond_shizuzijinbuzu =405014,
	//你正在建立氏族
	Clan_Commond_yijingzhengzaijianlishizule =405015,
	//技能等级已满
	Clan_Commond_jinengdengjiyiman =405016,
	//氏族技能研发等级不足
	Clan_Commond_shizujinengyanfadengjibuzu =405017,
	//你不是族长,无法转让氏族
	Clan_Commond_bushizuzhangwufazhuanrangshizu =405018,
	//搜索不到符合条件的氏族
	Clan_Commond_sousuobudaofuhetiaojiandeshizu =405019,
	//目前还没有人创建氏族
	Clan_Commond_muqianhaimeiyourenchuangjianshizu =405020,
	//{0}【{1}】
	Clan_Commond_shizumingzi =405021,
	//恭迎四海八荒的天命者进驻氏族！
	Clan_Commond_shizugonggao =405022,
	//创建要求：\n1.创建者角色等级达到25级；\n2.创建氏族时需消耗10万金币和200元宝。\n\n创建流程：\n1.氏族创建时为临时氏族，临时氏族需要在2小时内获得2名玩家的支持才能成为正式氏族。如果支持者人数不足，则创建失败，创建费用不予退回。\n2.创建成功后，氏族创建者自动成为氏族长，支持者自动成为氏族成员。
	Clan_Commond_chuangjianshizugonggao =405023,
	//你不是氏族长不能转让
	Clan_Commond_bushishizuzhangbunengzhuanrang =405046,
	//珍兽已经出战
	Local_TXT_Notice_Pet_HasChuzhan =406001,
	//珍兽寿命已满
	Pet_Age_zhanhunshoumingyiman =406002,
	//请补充珍兽寿命
	Pet_Age_zhanhunshoumingbuzuwufachuzhan =406003,
	//珍兽寿命{1}
	Pet_Age_zhanhunshoumingzengjianleX =406004,
	//无法封印出战中的珍兽
	Pet_Commond_wufafengyinchuzhanzhongdezhanhun =406005,
	//出战成功
	Pet_Commond_zhanhunchuzhanchenggong =406006,
	//每30秒仅能更改名字1次~请稍后再试~
	Pet_Commond_xiugaimingziCDzhong =406007,
	//{0}级开启珍兽功能
	Pet_Commond_Xjikaiqizhanhungongneng =406008,
	//珍兽可从新手任务或拍卖市场中获取
	Pet_Commond_meiyouzhanhunwufadakaishuxingjiemian =406009,
	//珍兽碎片不足无法合成
	Pet_Compose_zhanhunsuipianbuzuwufahecheng =406010,
	//完美品质后开启高级洗炼
	Pet_GuiYuan_haibunengjinxinggaojiguiyuan =406011,
	//洗炼成功
	Pet_GuiYuan_guiyuanchenggong =406012,
	//珍兽等级不能超过人物等级+修为等级
	Pet_Rank_zhanhundengjiyimanzanshiwufajixushengji =406013,
	//已经学会该技能
	Pet_Skill_yijingxuehuilegaijineng =406014,
	//技能学习成功
	Pet_Skill_jinengxuexichenggong =406015,
	//技能升级成功
	Pet_Skill_jinengshengjichenggong =406016,
	//最多锁定{0}个技能
	Pet_Skill_zuiduosuodingXgejineng =406017,
	//已经学会手动技能,继续学习【{0}】将会被替换掉
	Pet_Skill_shoudongjinengfugaixuexitishi =406018,
	//学会{0}个技能后开启锁定功能
	Pet_Skill_xuehuiXgejinengcaikaiqisuodinggongneng =406019,
	//修为已满级
	Pet_YinHun_xiuweiyimanjibunengjixuyinhunle =406020,
	//获得灵气{0}
	Pet_YinHun_yinhunchenggonghuodelingqiX =406021,
	//修灵成功,修灵等级+1
	Pet_YinHun_yinhundengjitisheng =406022,
	//珍兽寿命低于{0}无法封印
	Pet_Commond_zhanhunshoumingbuzuwufafengyin =406023,
	//自己或珍兽状态良好，不能吃药
	Pet_Commond_zijihuochongwuzhuangtailianghaobunengchiyao =406024,
	//当前珍兽已经接受过1次传承,无法再传承
	Pet_Next_zhenshouzhinengchuanchengyici =406025,
	//目标珍兽的品质需大于或等于材料珍兽
	Pet_Next_mubiaozhenshoudepinzhixudayudengyucailiaozhenshou =406026,
	//材料珍兽没有经验可传承
	Pet_Next_cailiaozhenshoumeiyoujingyanchuancheng =406027,
	//传承后目标珍兽的经验有溢出,是否继续
	Pet_Next_chuanchenghoumubiaozhenshouyoujingyanyichu =406028,
	//材料珍兽没有技能可传承
	Pet_Next_cailiaozhenshoumeiyoujinengchuancheng =406029,
	//目标珍兽上有技能,传承后将被材料珍兽的技能替换,是否继续
	Pet_Next_mubiaozhenshoudejinengjiangtihuancailiaozhenshou =406030,
	//材料珍兽没有修为可传承
	Pet_Next_cailiaozhenshoudexiuweiyouyichu =406031,
	//传承后目标珍兽的修为有溢出,是否继续
	Pet_Next_chuanchenghoumubiaozhenshouyouxiuweiyichu =406032,
	//需要消耗的{0}道具不足
	Pet_Next_chuanchengxiaohaodedaojubuzu =406033,
	//需要消耗的金币不足
	Pet_Next_chuanchengxiaohaodejingbibuzu =406034,
	//死亡的珍兽无法出战
	Pet_Fight_siwangedezhenshouwufachuzhan =406035,
	//没有对应的珍兽时装
	Pet_Suit_meiyouduiyingdezhenshoushizhuang =406036,
	//没有珍兽
	Pet_Commond_meiyouzhenshou =406037,
	//请勾选要传承的选项
	Pet_Commond_qinggouxuanyaochuanchengxuanxiang =406038,
	//珍兽技能学习{0}级开放
	Pet_Commond_kaifangchongwujinengxitong =406039,
	//加点方案保存成功
	Pet_Commond_jihuabaocunchenggong =406040,
	//珍兽剩余{0}次免费重置次数,之后重置需要消耗1颗轮回甘露,是否重置？
	Pet_Commond_chongzhixiaohaotips =406041,
	//{0}级后开启组队功能
	Team_Open_kaiqizudui =407001,
	//对方已下线
	Team_Limit_duifangyixiaxian =407002,
	//不能邀请{0}级以下的玩家组队
	Team_Limit_bunengyaoqingjiyixiawanjia =407003,
	//对方已经加入了一个队伍
	Team_Limit_duifangyijingjiaruleyigeduiwu =407004,
	//对方已经将你屏蔽
	Team_Limit_dongfangyijingjiangnipingbi =407005,
	//对方不接受陌生人的组队邀请
	Team_Limit_dongfangbujieshoumoshengrendezudongyaoqing =407006,
	//您的队伍人数已满
	Team_Limit_nindedongwurenshuyiman =407007,
	//你已经加入了一个队伍
	Team_Join_niyijingjiaruliaoyigedongwu =407008,
	//邀请已过期
	Team_Invite_yaoqingyiguoqi =407009,
	//队伍已满,无法加入
	Team_Join_dongwuyimanwufajiaru =407010,
	//成功加入队伍
	Team_Join_chenggongjiarudongwu =407011,
	//已发出申请,请稍候
	Team_Apply_yifachushenqingqingshaohou =407012,
	//{0}加入了队伍
	Team_Member_Xjiaruliaodongwu =407013,
	//只有队长才能解散队伍
	Team_Leader_zhiyoudongchangcainengjiesandongwu =407014,
	//队长解散了队伍
	Team_Leader_dongchangjiesanliaodongwu =407015,
	//只有队长才能转让队长
	Team_Leader_zhiyoudongchangcainengzhuanrangdongchang =407016,
	//{0}成为了新的队长
	Team_Leader_Xchengweiliaoxindedongchang =407017,
	//只有队长才能踢出队员
	Team_Leader_zhiyoudongchangcainengtichudongyuan =407018,
	//{0}被踢出了队伍
	Team_Member_Xbeitichuliaodongwu =407019,
	//{0}离开了队伍
	Team_Member_Xlikailiaodongwu =407020,
	//组队功能10级开启
	Team_Open_zudonggongneng10jikaiqi =407021,
	//对方已下线
	Team_Oppo_dongfangyixiaxian =407022,
	//对方不足10级
	Team_Limit_dongfangbuzu10ji =407023,
	//对方已经在队伍中了
	Team_Limit_dongfangyijingzaidongwuzhongliao =407024,
	//你已经被对方屏蔽
	Team_Limit_niyijingbeidongfangpingbi =407025,
	//队伍人数已满
	Team_Limit_dongwurenshuyiman =407026,
	//你已经在队伍中了
	Team_My_niyijingzaidongwuzhongliao =407027,
	//该请已过期
	Team_Apply_gaiqingyiguoqi =407028,
	//对方不在队伍中
	Team_Oppo_dongfangbuzaidongwuzhong =407029,
	//你不是队长
	Team_My_nibushidongchang =407030,
	//该队伍不存在
	Team_State_gaidongwubucunzai =407031,
	//请先选择活动目标
	Team_Activity_qingxianxuanzehuodongmubiao =407032,
	//离开成功
	Team_My_likaichenggong =407033,
	//你被踢出了队伍
	Team_My_nibeitichuliaodongwu =407034,
	//对方拒绝了你的邀请
	Team_My_dongfangjujueliaonideyaoqing =407035,
	//申请成功
	Team_Apply_shenqingchenggong =407036,
	//邀请成功
	Team_Invite_yaoqingchenggong =407037,
	//跟随失败
	Team_Follow_gensuishibai =407038,
	//确定要解散当前队伍？
	Team_Leader_jiesanduiwutishikuang =407039,
	//确定要离开当前队伍？
	Team_My_likaiduiwutishikuang =407040,
	//已召唤队员跟随自己
	Team_Follow_duizhangzhaohuanchenggong =407041,
	//已取消所有队员的跟随状态
	Team_Follow_duizhangquxiaozhaohuanchenggong =407042,
	//已发送招募信息至{0}
	Team_Recruit_zhaomuxinxifasongchenggong =407043,
	//招募
	Team_Recruit_zhaomu =407044,
	//已改为自由拾取模式
	Team_Recruit_ziyoushiqu =407045,
	//已改为队长拾取模式
	Team_Recruit_duizhangshiqu =407046,
	//我拾取了{0}
	Team_Recruit_woshiqu =407047,
	//丰厚奖励在等着你！小伙伴们快上车~
	Team_Recruit_hanhua =407048,
	//坐骑栏已满
	Ride_Commond_zuoqilanyiman =408001,
	//无法封印出战中的坐骑
	Ride_Commond_wufafengyinchuzhanzhongdezuoqi =408002,
	//坐骑已休息
	Ride_Commond_zuoqiyijingjinrulexiuxizhuangtai =408003,
	//寿命耗尽,无法封印
	Ride_Commond_shoumingwei0wufafengyin =408004,
	//当前地图无法骑乘坐骑
	Ride_Commond_dangqiandituwufaqichengzuoqi =408005,
	//解锁需要消耗{0}元宝
	Ride_Commond_jiesuoxuyaoxiaohaoXdianjuan =408006,
	//成功解封
	Ride_Commond_chenggongjiefeng =408007,
	//你已经没有{0}了
	Ride_Commond_niyijingmeiXjule =408008,
	//扩充成功
	Ride_Commond_kuochongchenggong =408009,
	//{0}成功出战
	Ride_Commond_Xchenggongchuzhan =408010,
	//{0}回去休息了
	Ride_Commond_Xhuiquxiuxile =408011,
	//设置已保存
	Ride_Commond_shezhiyijingbaocun =408012,
	//封印成功
	Ride_Commond_fengyinchenggong =408013,
	//还未设置出战坐骑
	Ride_Commond_haiweishezhichuzhanzuoqiwufaqicheng =408014,
	//你还没有坐骑,不能打开马厩界面
	Ride_Commond_nihaimeiyouzuoqibunengdakaizuoqijiemian =408015,
	//变身状态下无法骑乘
	Ride_Commond_bianshenzhuangtaixiawufaqicheng =408016,
	//正在释放技能
	Ride_Commond_shifangjinengzhongwufashangma =408017,
	//扩充坐骑栏需要{0}元宝
	Ride_Commond_kuochongzuoqilantishi =408018,
	//出战中的坐骑无法传承
	Ride_Inherit_chuzhanzhongdezuoqiwufachuancheng =408019,
	//传承坐骑没有任何经验值
	Ride_Inherit_chuanchengzuoqimeiyoujingyanzhi =408020,
	//满级坐骑无法接受传承
	Ride_Inherit_manjizuoqiwufajieshouchuancheng =408021,
	//请先取消想要更换的坐骑
	Ride_Inherit_qingxianquxiaoxiangyaogenghuandezuoqi =408022,
	//请选择继承的坐骑
	Ride_Inherit_qingxuanzejichengdezuoqi =408023,
	//请选择进行传承的坐骑
	Ride_Inherit_qingxuanzejinxingchuanchengdezuoqi =408024,
	//不能选择同一坐骑传承
	Ride_Inherit_bunengxuanzetongyigezuoqijinxingchuancheng =408025,
	//传承成功
	Ride_Inherit_chuanchengchenggong =408026,
	//坐骑等级已满
	Ride_Rank_zuoqidengjiyiman =408027,
	//今日已无法使用经验丹
	Ride_Rank_jintianyijingwufazaishiyongzhegedanyaole =408028,
	//使用成功,获得{0}经验
	Ride_Rank_shiyongjingyandanchenggonghuodeXjingyan =408029,
	//坐骑成功升到{0}级
	Ride_Rank_shengjichenggong =408030,
	//等级不足,无法领悟该技能
	Ride_Skill_dengjibuzuwufalingwugaijineng =408031,
	//很遗憾,技能领悟失败
	Ride_Skill_jinenglingwushibai =408032,
	//成功领悟新的技能
	Ride_Skill_jinenglingwuchenggong =408033,
	//饱食度不足
	Ride_Skill_baoshidubuzuwufashifangjineng =408034,
	//普通
	Ride_Illustrated_1 =408035,
	//稀少
	Ride_Illustrated_2 =408036,
	//罕见
	Ride_Illustrated_3 =408037,
	//珍异
	Ride_Illustrated_4 =408038,
	//绝世
	Ride_Illustrated_5 =408039,
	//{0}级解锁
	Ride_Skill_dengjijiesuo =408040,
	//未解锁
	Ride_Skill_weijiesuo =408041,
	//学习前置技能解锁
	Ride_Skill_xuexiqianzhijienengjiesuo =408042,
	//未领悟
	Ride_Skill_weilingwu =408043,
	//更多炫酷坐骑可通过商城获得
	Ride_Commond_gengduoxuankuzuoqiketongguoshangchenghuode =408044,
	//坐骑转化比即坐骑属性转化为角色属性的比例
	Ride_Commond_zuoqizhuanhuabi =408045,
	//恭喜您，骑术升级成功！
	Ride_Commond_qishushengji =408046,
	//获得{0}[31ee31]{1}
	knapsack_Commond_1 =409001,
	//获得{0}×[31ee31]{1}
	knapsack_Commond_2 =409002,
	//是否消耗{0}[31ee31]{1},解锁[31ee31]{2}[-]个格子
	knapsack_Commond_3 =409003,
	//背包空间不足
	knapsack_Commond_4 =409004,
	//该装备无法分解
	knapsack_Decompose_5 =409005,
	//装备分解后将无法找回,确定要将{0}分解吗？
	knapsack_Decompose_6 =409006,
	//所选装备中包含稀有装备,分解后将无法找回,是否继续分解？
	knapsack_Decompose_7 =409007,
	//是否消耗{0}[31ee31]{1}[-]修理该装备耐久度？
	knapsack_Repair_8 =409008,
	//成功修理{0}
	knapsack_Repair_9 =409009,
	//是否消耗{0}[31ee31]{1}[-]修理已穿戴全部装备？
	knapsack_Repair_10 =409010,
	//成功修理全部已穿戴装备
	knapsack_Repair_11 =409011,
	//该装备耐久度已满,无需修理！
	knapsack_Repair_12 =409012,
	//已穿戴的全部装备无需修理
	knapsack_Repair_13 =409013,
	//仓库空间不足
	knapsack_Warehouse_14 =409014,
	//操作的金币数超过最大拥有数,请重新输入
	knapsack_Warehouse_15 =409015,
	//仓库成功存入{0}[31ee31]{1}
	knapsack_Warehouse_16 =409016,
	//仓库成功取出{0}[31ee31]{1}
	knapsack_Warehouse_17 =409017,
	//购买[31ee31]{0}[-]可解锁[31ee31]仓库{1}[-]
	knapsack_Warehouse_18 =409018,
	//该道具无法出售
	knapsack_Shop_19 =409019,
	//出售该道具可获得{0}[31ee31]{1}[-],是否继续出售？
	knapsack_Shop_20 =409020,
	//道具出售后将无法找回,确定要将所选道具全部出售吗？
	knapsack_Shop_21 =409021,
	//成功购买{0}×[31ee31]{1}
	knapsack_Shop_22 =409022,
	//确定将所选道具全部出售吗？
	knapsack_Shop_23 =409023,
	//等级:{0}/{1}
	knapsack_Commond_24 =409024,
	//成长度:{0}
	knapsack_Commond_25 =409025,
	//合成所需道具数量不足
	knapsack_Synthesis_26 =409026,
	//今日使用次数已达上限
	knapsack_Experience_1 =409027,
	//[1c2832]副装备选择：[097f1d]职业-{0}，部位-{1}
	forging_compose_deputy_notice =411001,
	//[FF0000]该主装备无法参与装备合成
	forging_compose_Incompose_notice =411002,
	//选择结果
	forging_compose_ChooseResult =411003,
	//结果一
	forging_compose_Result1 =411004,
	//结果二
	forging_compose_Result2 =411005,
	//结果三
	forging_compose_Result3 =411006,
	//选择一种合成结果作为装备附加属性
	forging_compose_ChooseNotice =411007,
	//请先选择一组合成结果
	forging_compose_ChooseTips =411008,
	//合成完成
	forging_compose_Complete =411009,
	//获得附加属性
	forging_compose_GetAttr =411010,
	//确认选择
	forging_compose_ConfirmChoose =411011,
	//该装备没有附加属性，不可选！
	forging_compose_1 =411012,
	//宝石列表需至少展开一列
	Gemstone_Commond_1 =412001,
	//背包中没有该属性宝石
	Gemstone_Commond_2 =412002,
	//主角{0}级开启该槽位
	Gemstone_Commond_3 =412003,
	//目前没有可镶嵌入该槽位的宝石
	Gemstone_Commond_4 =412004,
	//该部位槽位已镶满宝石
	Gemstone_Commond_5 =412005,
	//该宝石需[FF0000]主角{0}级[-]才可使用！
	Gemstone_Commond_6 =412006,
	//该符石档次未高于勾选属性档次，不可选！
	Rune_Promote1 =413001,
	//该符石档次低于勾选属性档次，不可选！
	Rune_Eliminate1 =413002,
	//背包中未找到升星所需{0}圣魂
	Soul_Star_1 =414001,
	//主角{0}级开启竞技场
	Arena_Commond_1 =416001,
	//21:55-22:05为竞技场关闭时间
	Arena_Commond_2 =416002,
	//挑战次数不足,消耗{0}[31ee31]{1}[-]重置挑战次数？
	Arena_Commond_3 =416003,
	//挑战CD中,消耗{0}[31ee31]{1}[-]清除CD？
	Arena_Commond_4 =416004,
	//今日重置挑战次数已达上限
	Arena_Commond_5 =416005,
	//消耗{0}[31ee31]{1}[-]清除CD立即挑战？
	Arena_Commond_6 =416006,
	//消耗{0}[31ee31]{1}[-]重置全部挑战次数？
	Arena_Commond_7 =416007,
	//成功清除挑战CD！
	Arena_Commond_8 =416008,
	//成功重置挑战次数！
	Arena_Commond_9 =416009,
	//挑战对手不存在
	Arena_Commond_10 =416010,
	//对手已下线
	Arena_Commond_11 =416011,
	//创建地图失败
	Arena_Commond_12 =416012,
	//竞技场无法挑战自己
	Arena_Commond_13 =416013,
	//战斗中无法重复挑战
	Arena_Commond_14 =416014,
	//数据已经过时了
	Arena_Commond_15 =416015,
	//邀请超时
	Arena_Commond_16 =416016,
	//对手数据错误
	Arena_Commond_17 =416017,
	//声望不足,参与氏族活动可获取
	Wandering_Commond_1 =417001,
	//积分不足,参与竞技场可获取
	Wandering_Commond_2 =417002,
	//物品已售罄
	Wandering_Commond_3 =417003,
	//今日刷新次数已达上限
	Wandering_Commond_4 =417004,
	//物品刷新成功
	Wandering_Commond_5 =417005,
	//成功购买{0}×[31ee31]{1}
	Wandering_Commond_6 =417006,
	//战勋不足,参与神魔大战可获取
	Wandering_Commond_7 =417007,
	//神魔大战未到报名时间
	ZYZ_Commond_zhengyinzhanweidaobaomingshijian =419001,
	//华夏城战即将开始,是否进入参加？
	CityWar_huaxiachengzhankaishi =420001,
	//华夏城战
	CityWar_huaxiachengzhan =420002,
	//即将离开副本,是否继续？
	CityWar_jijianglikaifuben =420003,
	//华夏城战报名成功
	CityWar_huaxiachengzhanbaomingchenggong =420004,
	//副本次数已达上限
	Copy_Commond_fubencishuyijdashangxian =421001,
	//玩家：[FFFF00]{0}[-]的副本次数已达上限
	Copy_Commond_xfubencishuyijdashangxian =421002,
	//副本不存在
	Copy_Commond_fubenbucunzai =421003,
	//创建副本失败
	Copy_Commond_chuangjianfubenshibai =421004,
	//不在副本中
	Copy_Commond_buzaifubenzhong =421005,
	//不是队伍副本
	Copy_Commond_bushiduiwufuben =421006,
	//不是单人副本
	Copy_Commond_bushidanrenfuben =421007,
	//不是帮会副本
	Copy_Commond_bushibanghuifuben =421008,
	//不在一个等级区间
	Copy_Commond_buzaiyigedengjiqujian =421009,
	//队伍人数不足
	Copy_Commond_duiwurenshubuzu =421010,
	//玩家：[FFFF00]{0}[-]等级不在区间无法进入
	Copy_Commond_xdengjibuzaiqujianwufajinru =421011,
	//玩家：[FFFF00]{0}[-]等级过低
	Copy_Commond_xrenwudengjiguodi =421012,
	//玩家：[FFFF00]{0}[-]跟队长不在同一地图
	Copy_Commond_xgenduizhangbuzaitongyiditu =421013,
	//队员：[FFFF00]{0}[-]没有跟随
	Copy_Commond_xduiyuanmeiyougensui =421014,
	//玩家：[FFFF00]{0}[-]不在安全区
	Copy_Commond_xbuzaianquanqu =421015,
	//玩家：[FFFF00]{0}[-]未解锁该副本
	Copy_Commond_xweijiesuogaifuben =421016,
	//玩家：[FFFF00]{0}[-]未同意进入副本
	Copy_Commond_xweitongyijinrufuben =421017,
	//副本不在开放时间内
	Copy_Commond_fubenbuzaikaifangshijiannei =421018,
	//玩家：[FFFF00]{0}[-]没有凭证
	Copy_Commond_xmeiyoupingzheng =421019,
	//只有队长才能进副本哦
	Copy_Commond_zhiyouduizhangcainengjinfubeno =421020,
	//您已通关，副本将在{0}秒后关闭
	Copy_Commond_fubenguanbitips =421021,
	//您已失败，副本将在{0}秒后关闭
	Copy_Commond_fubenguanbitips2 =421022,
	//该挑战难度较高，建议组队，是否依然进行单人挑战？
	Copy_Commond_fubenjinrutips =421023,
	//你切换到{0}{1}线
	Copy_Commond_changeline =421024,
	//你无法切换到当前地图，请下载完成更新资源包
	Copy_Commond_xiazaigengxin =421025,
	//法术值不足
	Skill_Commond_fashuzhibuzu =422001,
	//当前状态不能使用技能
	Skill_Commond_dangqianzhuangtaibunengshiyongjineng =422002,
	//
	Skill_Commond_yingzhishijianneibunengshiyongjineng =422003,
	//你需要选择一个目标才能使用
	Skill_Commond_shifanggaijinengxuyaoxuanzemubiao =422004,
	//切换技能形态需要达到30级
	Skill_Commond_dengjibuzubunengqiehuanxingtai =422005,
	//人物等级不足,不能升级
	Skill_Commond_renwudengjibuzubunnegshengji =422006,
	//人物经验不足,不能升级
	Skill_Commond_renwujingyanbuzubunengshengji =422007,
	//技能使用成功
	Skill_Commond_jinengshiyongchengong =422008,
	//技能被打断
	Skill_Commond_jinengbeidaduan =422009,
	//技能正在使用
	Skill_Commond_jinengzhengzaishiyong =422010,
	//变身状态下不能使用技能
	Skill_Commond_bianshenzhuangtaixiabunengshiyongjineng =422011,
	//珍兽未出战,不能使用技能
	Skill_Commond_chongwuweichuzhanbunengshiyongjineng =422012,
	//技能不存在
	Skill_Commond_jinengbucunzai =422013,
	//技能未知错误
	Skill_Commond_jinengweizhicuowu =422014,
	//目标超出攻击距离
	Skill_Commond_mubiaochaochugongjijuli =422015,
	//当前目标无法攻击
	Skill_Commond_dangqianmubiaowufagongji =422016,
	//当前目标无法攻击
	Skill_Commond_mubiaoxuanzecuowu =422017,
	//技能冷却中
	Skill_Commond_jinenglengquezhong =422018,
	//当前技能状态错误
	Skill_Commond_dangqianjinengzhuangtaicuowu =422019,
	//使用宠物技能所需道具不足
	Skill_Commond_shiyongchongwujinnegsuoxudaojubuzu =422020,
	//骑乘状态无法使用主角技能
	Skill_Commond_qichengzhuangtaiwufashiyongzhujuejineng =422021,
	//当前状态不能使用该技能
	Skill_Commond_dangqianzhuangtaibunengshiyonggaijineng =422022,
	//请选中一个可以替换的技能
	Skill_Commond_qingxuanzhongyigekeyitihuandejineng =422023,
	//杀破狼
	Skill_Commond_xushi =422024,
	//磐石固
	Skill_Commond_panshi =422025,
	//妙手春
	Skill_Commond_zhiliao =422026,
	//辣手花
	Skill_Commond_huanhua =422027,
	//独人行
	Skill_Commond_duxing =422028,
	//一当百
	Skill_Commond_qugong =422029,
	//驭灵魔
	Skill_Commond_zhaohuan =422030,
	//巫魅蛊
	Skill_Commond_zuzhou =422031,
	//同一个队伍不能攻击
	Skill_Commond_tongyigeduiwubunenggongji =422032,
	//同一个氏族不能攻击
	Skill_Commond_tongyigeshizubunenggongji =422033,
	//同一个阵营不能攻击
	Skill_Commond_tongyigezhenyingbunenggongji =422034,
	//目标不符合善恶规则,不能攻击
	Skill_Commond_mubiaobufuheshaneguizebunenggongji =422035,
	//安全区内无法攻击
	Skill_Commond_anquanquneiwufagongji =422036,
	//战斗模式不对
	Skill_Commond_zhandoumoshubudui =422037,
	//找不到可攻击的目标
	Skill_Commond_zhaobudaokegongjidemubiao =422038,
	//友方目标无法攻击
	Skill_Commond_youfangmubiaowufagongji =422039,
	//不能攻击自己的珍兽或宝宝
	Skill_Commond_zijiwufagongjiziji =422040,
	//新手保护玩家无法攻击
	Skill_Commond_xinshoubaohuwanjiawufagongji =422041,
	//找不到自己的宝宝,无法使用
	Skill_Commond_zhaobudaozijidebaobaowufashiyong =422042,
	//只能对自己的宝宝使用
	Skill_Commond_zhinnegduizijidebaobaoshiyong =422043,
	//找不到自己的珍兽,无法使用
	Skill_Commond_zhaobudaozijidezhanhunwufashiyong =422044,
	//只能对自己的珍兽使用
	Skill_Commond_zhinengduizijidezhanhunshiyong =422045,
	//找不到友方尸体,无法使用
	Skill_Commond_zhaobudaoyoufangshitiwufashiyong =422046,
	//只能对友方尸体使用
	Skill_Commond_zhinengduiduifangshitishiyog =422047,
	//非敌对阵营不能攻击
	Skill_Commond_feididuizhenyingbunenggongji =422048,
	//心法丸数量不足
	Skill_Commond_xinfawanshuliangbuzu =422049,
	//当前不需要重置
	Skill_Commond_dangqianbuxuyaochongzhi =422050,
	//一键升级会消耗大量的金币，是否继续？
	Skill_Commond_yijianshengji =422051,
	//主人有目标时才能使用
	Skill_Commond_zhanhunshoudongjineng =422052,
	//保存成功
	Skill_Commond_jinengshezhibaocunchenggong =422053,
	//当前可以免费重置心法{0}次，返还100%金币和已分配的心法点，是否重置？
	HeartSkill_Commond_mianfeichongzhixingfa =422100,
	//采集中...
	Task_Status_1 =424001,
	//道具使用中…
	Task_Status_2 =424002,
	//挂机中…
	Task_Status_3 =424003,
	//自动寻路中…
	Task_Status_4 =424004,
	//今日已完成10次每日卜卦任务，还有10次高额奖励任务可完成，是否继续？
	Task_Rings_1 =424005,
	//背包空间不足，无法接取任务
	Task_Commond_1 =424006,
	//背包空间不足，无法领取任务奖励
	Task_Commond_2 =424007,
	//背包空间不足，无法执行任务
	Task_Commond_3 =424008,
	//传送中…
	Task_Status_5 =424009,
	//购买[31ee31]{0}[-]会使当前已购皇令失效,是否继续?
	Local_TXT_Notice_Noble_BtnTips =425001,
	//交易失败！您的账户额度不足,请先向账户充值后再进行操作!
	Local_TXT_Notice_Noble_ToRecharge =425002,
	//您想以[31ee31]￥{0}元[-]的价格购买一个[31ee31]{1}[-]吗?
	Local_TXT_Notice_Noble_BuyNoble =425003,
	//成功购买[31ee31]{0}[-]
	Noble_Commond_1 =425004,
	//您不在队伍中
	PK_Commond_1 =426001,
	//您不在氏族中
	PK_Commond_2 =426002,
	//您没有敌对阵营
	PK_Commond_3 =426003,
	//等级无变化,再试一次？
	MZ_Commond_1 =427001,
	//等级升到2星了,不过你就这点追求？
	MZ_Commond_2 =427002,
	//等级升到3星了,再来再来！
	MZ_Commond_3 =427003,
	//等级升到4星了,还有一步之遥！
	MZ_Commond_4 =427004,
	//等级升到5星了,赶快接取任务吧O(∩_∩)O
	MZ_Commond_5 =427005,
	//金币不足
	MZ_Commond_6 =427006,
	//绑元不足
	MZ_Commond_7 =427007,
	//元宝不足
	MZ_Commond_8 =427008,
	//未检测到网络连接,是否重试?
	Update_Commond_NoNetLink =428001,
	//检测到更新,本次更新大小为{0}兆,是否更新?
	Update_Commond_IncrementUpdate =428002,
	//检测到新版客户端({0}兆),是否前往下载?
	Update_Commond_WholePackageUpdate =428003,
	//当前为非WIFI环境,建议连接WIFI后前往下载!
	Update_Commond_NoWIFI =428004,
	//登录前，请先选择一个区服
	Login_SelectZoneAtFirst =428100,
	//本区还没开放
	Login_ZoneIsClosed =428101,
	//{0}{1}区
	Login_Server_AreaServerDisplayName =428102,
	//推荐服务器
	Login_Server_RecommondSever =428103,
	//我的区服
	Login_Server_MyServer =428104,
	//推荐区服
	Login_Server_RecommonAreaServer =428105,
	//选择服务器
	Login_Server_ChooseServer =428106,
	//火爆
	Login_Server_Hot =428107,
	//拥挤
	Login_Server_Crowd =428108,
	//畅通
	Login_Server_Smooth =428109,
	//维护
	Login_Server_Down =428110,
	//你已成功提交，请耐心等待回复
	FeedBack_Tips =429001,
	//手机号绑定成功
	FeedBack_BindPhoneSuccess =429002,
	//
	Imformation_Commond_meiyouzhanhun =430001,
	//奖励领取成功！
	Trailer_Commond_1 =431001,
	//{0}功能{1}级开启
	Trailer_Commond_xitongyeqiankaiqi =431002,
	//{0}级开放狩猎系统
	DailyHuntingOpen_Level =432001,
	//1.与首领等级差在20级内\n2.在首领仇恨列表中且未死亡\n3.每日获得猎魂上限为600\n5.每天4:00刷新猎魂上限\n5.击杀首领可获得额外的猎魂\n6.首领在10:00-23:00刷新且0点回收
	DailyHuntingText_TipsContent =432002,
	//佩戴称号{0}
	Title_Commond_peidaichenghao =433001,
	//激活称号{0}
	Title_Commond_jihuochenghao =433002,
	//收益时间：{0}小时{1}分 （满收益{2}小时）
	Offline_Earning_Time =434001,
	//经验收益：{0}
	Offline_Earning_Exp =434002,
	//道具收益：
	Offline_Earning_GetItem =434003,
	//1倍收益
	Offline_Earning_OneGetTxt =434004,
	//2倍收益\n（任一皇令领取）
	Offline_Earning_TwoGetTxt =434005,
	//离线收益
	Offline_Earning_Title =434006,
	//登录中，请稍候...
	Net_Logining =435001,
	//请求区服列表...
	Net_RequestZoneList =435002,
	//账号已被登录，正在处理...
	Net_AccountHasLogin =435003,
	//正在建立连接.....
	Net_Connecting =435004,
	//账号在别处登录
	Net_OtherLogin =435005,
	//数据异常2，请尝试重新登录或联系客服。
	Net_2 =435006,
	//lua错误4，请尝试重新登录或联系客服。
	Net_4 =435007,
	//无法获区服列表5，请尝试重新登录或联系客服。
	Net_5 =435008,
	//签名校验失败11，请尝试重新登录或联系客服。
	Net_11 =435009,
	//呀，服务器关闭了，如果现在不是维护时段，请尽快联系客服
	Net_12 =435010,
	//Json错误13，请尝试重新登录或联系客服。
	Net_13 =435011,
	//Json错误14，请尝试重新登录或联系客服。
	Net_14 =435012,
	//token验证失败15，请尝试重新登录或联系客服。
	Net_15 =435013,
	//登录帐号不一致16，请尝试重新登录或联系客服。
	Net_16 =435014,
	//呀，服务器关闭了，如果现在不是维护时段，请尽快联系客服
	Net_17 =435015,
	//找不到sdk服务器18，请尝试重新登录或联系客服。
	Net_18 =435017,
	//签名错误19，请尝试重新登录或联系客服。
	Net_19 =435018,
	//第三方服务器验证错误20，请尝试重新登录或联系客服。
	Net_20 =435019,
	//proto解析错误21，请尝试重新登录或联系客服。
	Net_21 =435020,
	//网关错误22，请尝试重新登录或联系客服。
	Net_22 =435021,
	//响应超时23，请尝试重新登录或联系客服。
	Net_23 =435022,
	//帐号上次未正常退出24，请尝试重新登录或联系客服。
	Net_24 =435023,
	//platid＝0被限制25，请尝试重新登录或联系客服。
	Net_25 =435024,
	//需要等级：{0}
	ItemTips_NeedLv =436001,
	//战斗力：{0}
	ItemTips_Fpower =436002,
	//职业：{0}
	ItemTips_Pro =436003,
	//部位：{0}
	ItemTips_Part =436004,
	//耐久：{0}/{1}
	ItemTips_Dur =436005,
	//基础属性
	ItemTips_Base =436006,
	//附加属性
	ItemTips_Additive =436007,
	//宝石属性
	ItemTips_Gem =436008,
	//未镶嵌
	ItemTips_UnInlay =436009,
	//{0}级解锁
	ItemTips_UnlockLv =436010,
	//档次：{0}
	ItemTips_Grade =436011,
	//等级：{0}/{1}
	ItemTips_Lv =436012,
	//成长度：{0}%
	ItemTips_Growth =436013,
	//已拥有：{0}
	ItemTips_Own =436014,
	//物理攻击：
	ItemTips_PhyAttack =436015,
	//法术攻击：
	ItemTips_MagicAttack =436016,
	//物理防御：
	ItemTips_PhyDefend =436017,
	//法术防御：
	ItemTips_MagicDefend =436018,
	//挖宝中...
	Cangbaotu_Status_wabaozhong =437001,
	//摇钱树获得{0}钱
	CangBaotu_ShakeMoneyTree_Num =437002,
	//今日使用次数已达上限
	Cangbaotu_Commond_shiyongshangxian =437003,
	//角色等级超过30级，无法被招募
	Invite_Commond_wufabeizhaomu =438001,
	//1.使用福瑞珠可以提升5倍的刷怪效率,每个限100只怪物生效,每天最多使用4个\n2.高倍经验怪物数量每天4：00重置\n3.与自身等级正负差3级内的怪物经验最高\n4.队伍成员共享计数
	TanHao_Commond_meirishilian =439001,
	//1.使用福瑞珠可以提升5倍的刷怪效率,每个限100只怪物生效,每天最多使用4个\n2.高倍经验怪物数量每天4：00重置\n3.与自身等级正负差3级内的怪物经验最高\n4.队伍成员共享计数
	TanHao_Commond_chongwushuxin =439002,
	//1.使用福瑞珠可以提升5倍的刷怪效率,每个限100只怪物生效,每天最多使用4个\n2.高倍经验怪物数量每天4：00重置\n3.与自身等级正负差3级内的怪物经验最高\n4.队伍成员共享计数
	TanHao_Commond_chongwujineng =439003,
	//{0}级才能发放红包！
	Red_Tips_njicainengfafanghongbao =440001,
	//很遗憾，红包已经被抢完了！
	Red_Tips_henyihanhongbaoyijingbeiqiangwanle =440002,
	//你还没有加入氏族
	Red_Tips_nihaimeiyoujiarushizu =440003,
	//刷新频率过快
	Red_Tips_shuaxinpinlvguokuai =440004,
	//收到{0}的红包，谢谢老板！
	Red_Tips_xiexielaoban =440005,
	//今日领取红包个数已达上限！
	Red_Tips_hongbaoyidashangxian =440006,
	//是否花费{0}元宝发放到{1}频道？
	Red_Tips_querenfafanghongbao =440007,
	//大战即将开始.
	Camp_Notice_Begin1 =450001,
	//大战即将开始..
	Camp_Notice_Begin2 =450002,
	//大战即将开始…
	Camp_Notice_Begin3 =450003,
	//大战已经打响，勇士们策马向前冲吧！
	Camp_Notice_Start =450004,
	//[1c2832]当游戏界面显示异常，无法登录游戏，或者游戏资源加载出错时，可以尝试进行修复。[-][0072FF]该操作会清除本地补丁并重新下载，请在良好的网络环境下进行，并注意手动重启客户端。[-]
	Repair_kehuduanxiufu =460001,
	//[097f1d]可挑战[-]
	HuntingBoss_KeTiaoZhan =461001,
	//[ff2424]未解锁[-]
	HuntingBoss_WeiJieSuo =461002,
	//[097f1d]已刷新[-]
	HuntingBoss_YiShuaXin =461003,
	//[0072ff]刷新时间 {0}[-]
	HuntingBoss_ShuaXinShiJian =461004,
	//[445a6c]已休息[-]
	HuntingBoss_YiXiuXi =461005,
	//[ff2424]攻击中[-]
	HuntingBoss_GongJiZhong =461006,
	//[646464]未开启[-]
	HuntingBoss_NotOpen =461007,
	//[0072ff]{0}/{1}[-][1c2832]次
	HuntingBoss_InspireLeftTimesEnough =461008,
	//[ff2424]{0}/{1}[-][1c2832]次
	HuntingBoss_InspireLeftTimesNotEnough =461009,
	//[097f1d]{0}[-]
	HuntingBoss_InspireFormatTxt =461010,
	//[646464]已结束[-]
	HuntingBoss_End =461011,
	//世界首领出现在了聚宝窟中，是否前往挑战？
	HuntingBoss_BeginNotice =461012,
	//当前时间段世界首领暂未开启！
	HuntingBoss_weikaiqi =461013,
	//您确认删除[00ec00]{0}[-]吗，角色删除后将无法恢复！请慎重操作！
	DeleteRole_Tips_1 =470001,
	//确认要删除[00ec00]{0}[-]此角色吗？[ff2424]{1}小时后系统正式删除该角色，{2}小时内用该角色登录游戏即可恢复角色[-]
	DeleteRole_Tips_2 =470002,
	//[ff2424]{0}小时后删除[-]
	DeleteRole_Tips_3 =470003,
	//只有一个角色，无法删除
	DeleteRole_Tips_4 =470004,
	//是否恢复角色[00ec00]{0}[-]并进入游戏？
	RecoverRole_Tips_5 =470005,
	//{0}小时后删除
	DeleteRole_Tips_5 =470006,
}
