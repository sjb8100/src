using System;

public enum UIMsgID
{
    eNone,
    eShowUI,
    eZoneData, //服务器列表
    eLoadingTips,
    eLoadingProcess,
    eSetRoleProperty,
    //登陆界面
    eLoginAuthorizeState,
    //主界面
    eUpdateTargetHp,
    eRefreshEnemyList,//更新仇恨列表
    eRefreshNpcBelong, //刷新bos归属

    eLoginCheckName,
    eServerListStateRefresh,
    //任务
    eUpdateTaskList,
    eDeleteTask,
    eClearRichText,
    eRefreshTaskDesc,
    eRefreshStarTask,
    eSetPropRoleID,
    eRewardTaskLeftNum,
    eRewardTaskListRefresh,
    eRewardTaskCardNum,
    eTask_Refresh_QuestInfo,

    eOpenNpcDialogData,//打开npc对话数据
    eUpdateCurrencyUI,
    eResetChatWindowPosX,
    eResetChatWindowPosY,
    eRefreshSendBtnLable,

    //技能
    eSkillBtnRefresh,
    eSkillChangeState,
    eSkillShowPetSkill,

    //快捷使用道具
    eShortcutList,    //快捷使用道具
    eShortcutRect,    //快捷使用道具区域
    eUseItemRefresh,    //
    //好友
    eUpdateFriendList,
    eUpdateFriendMsgTips,
    eChatWithPlayer,    //和玩家聊天
    //
    eHideSimpleChat,
    eShowSimpleChat,
    //joystick
    eJoystickStable,

    //组队
    eUpdateMyTeamList,//更新我的队伍
    eUpdateApplyList, //申请列表
    eTeamNewApply,         //有新的人申请入队
    eDisbandTeam,     //解散队伍
    eUpdateExistedTeamList,//已经存在的队伍list
    eTeamInvitePeopleList, //可邀请的人员列表
    eTeamTargetActivity,    //当前活动目标
    eTeamItemMode,         //拾取物品模式
    eTeamMatch,             //自定匹配
    eTeamCancleMatch,       //取消匹配
    eTeamMemberBtn,         //显示队友操作按钮

    eReplaceMedical,

    //武斗场
    eArenaMainData, //武斗场主信息
    eArenaTopThree, //前三名
    eArenaRivalThree,    //三对手
    eArenaCDUpdate,     //刷新剩余挑战CD
    eArenaTimesUpdate,  //刷行挑战次数
    eArenaRank,     //武斗场排行榜
    eArenaBattlelog,//武斗场战报
    eArenaEnter,    //进入武斗场战斗
    eArenaBattleInit, //初始化武斗场界面
    eArenaStartBattleCD,//开始3、2、1
    eArenaBattleResult,  //战斗结算
    eArenaExit,          //退出武斗场

    eRideUpdateAutoCost,
    //许愿树
    eTreeHelp,       //树点赞
    eTreeBegin,      //树刷新时间
    eHomeFriendUpdate, //家园好友跟新
    eRideUpdateLearnSkill,

    //minimap
    eShowCopyInfo,

    eCopyEnter,    //副本进入初始化

    eCopyExit,         //副本退出

    eCopyGold,         //副本获得金币数

    //pet
    eShowQuickSettingBtn,

    //称号
    eTitleListUpdate,  //称号数据

    //buff
    stShowBuff,

    //nvwa
    eNvWaInit,      //女娲界面初始化
    eNvWaStart,     //女娲开始
    eNvWaExit,      //女娲退出
    eNvWaNewWave,   //新的一波怪物
    eNvWaDesCD,        //倒计时
    eNvWaRecruit,   //招募
    eNvWaLvUp,      //升级
    eNvWaCap,       //女娲徽章
    eNvWaReslut,    //女娲结算
    eNvWaGuardNumUpdate, //女娲守卫剩余数量

    //城战
    eCityWarInfoUpdate,
    eCityWarSelfInfoUpdate,  //自己的城战信息
    eCityWarFinish,         //城战结算
    eCityWarSignUpClan,     //城战报名
    eCityWarFrameInfo,      //城战报名信息
    eCityWarSubmitStatue,   //城战报名上交物品

    eFightPowerChange,     //战斗力变化

    eOnlyCreatMainPlayer,  //只创建主角
    eIgnoreLand,           //过滤地形
    eIgnoreGrass,          //过滤草
    eIgnoreStatic,         //过滤静态物体
    eIgnoreScene,          //过滤场景特效
    eIgnoreWater,          //过滤水
    ePreLoad,              //预加载

    eRefreshGetWayParam, // 刷新获取面板

    eUpdateDailyTest,          //每日试炼刷新
    eUpdateDailyTestExpDouble, //每日试炼双倍经验刷新

    eDailyAnswerNewQ,         //每日答题 新题
    eDailyAnswerReward,       //每日答题 奖励领取
    eDailyAnswerEachReward,   //每日答题 每答对一道题的奖励

    eFishingGetOne,           //有一条鱼上钩
    eFishingSuccess,          //鱼钓上来了
    eFishingMyRank,           //我的排行
    eFishingRank,             //排行

    eAccumulativeRecharge,    //累计充值

    eAnswerState,             //答题状态
    eAnswerCurInfo,           //答题人数，金币等信息

}

public struct stCopyInfo
{
    public bool bShow;
    public bool bShowBattleInfoBtn;//是否显示详情按钮
}


public struct stShowBuffInfo
{
    public bool IsMainRole;
}

public struct stCopyBattleInfo
{
    public string des;
    public uint cd;
}

public struct stGetWayDescription 
{
    public bool bShow;
    public string des;
}

public struct stRefreshNPCBelongParam 
{
    public long npcid;
    public uint teamid;
    public uint ownerid;
    public uint clanid;
    public string ownerName;
}

