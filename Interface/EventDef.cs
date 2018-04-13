using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Client
{
    // 事件
    public enum GameEventID
    {
        Unknow =0,
        // 系统事件ID  100 ~ 999  100 以下为GameDev中使用
        NETWORK_CONNECTE_CLOSE = 100,       // 网络断开
        PLAYBTNVOICE = 101,                 // 按钮点击播放音效

        RECONNECT_SUCESS,                   // 重连成功 stReconnectSucess
        APPLICATION_LOWMEMORY,              //内存警告

        // 加载场景完毕事件
        SYSTEM_LOADSCENECOMPELETE = 900,    // 场景加载完毕
        PLAYER_LOGIN_SUCCESS = 901,         // 角色成功登陆服务器
        SYSTEM_LOADUICOMPELETE,             // 加载UI成功
        SYSTEM_GAME_READY,                  // 游戏数据准备完毕
        STORY_PLAY_OVER,
        LOGINSUCESSANDRECEIVEALLINFO,    	// 登录成功并且受到所有相关消息 只发一次

        CHECKINGTIME,          //玩家和服务器对时

        // 逻辑模块事件 (模块ID * 1000 + 事件序号)
        // 实体系统
        ENTITYSYSTEM_CREATEENTITY = 1001,   // 创建实体
        ENTITYSYSTEM_REMOVEENTITY,          // 删除实体
        ENTITYSYSTEM_ENTITYBEGINMOVE,       // 开始移动
        ENTITYSYSTEM_ENTITYMOVE,            // 实体移动
        ENTITYSYSTEM_ENTITYSTOPMOVE,        // 停止移动
        ENTITYSYSTEM_TARGETCHANGE,          // 目标改变
        ENTITYSYSTEM_PROPUPDATE,            // 属性更新
        ENTITYSYSTEM_HPUPDATE,              //血量更新 （不再走属性更新的消息）
        ENTITYSYSTEM_MPUPDATE,              //蓝量更新 （不再走属性更新的消息）
        ENTITYSYSTEM_ENTITYBEGINDEAD,       // 实体开始死亡 stEngityBeginDead
        ENTITYSYSTEM_ENTITYDEAD,            // 实体死亡
        ENTITYSYSTEM_RELIVE,                // 实体复活
        ENTITYSYSTEM_MAINPLAYERRELIVE,      //玩家主角复活
        ENTITYSYSTEM_ENTERMAP,              // 进入地图
        ENTITYSYSTEM_LEAVEMAP,              // 离开地图
        ENTITYSYSTEM_CHANGENAME,            // 名称改变
        ENTITYSYSTEM_SETHIDE ,              // 隐身
        ENTITYSYSTEM_BEGINRIDE,             // 开始上马
        ENTITYSYSTEM_RIDE,                  // 上马完成
        ENTITYSYSTEM_UNRIDE,                // 下马
        ENTITYSYSTEM_RESETROTATION,         // 恢复npc
        ENTITYSYSTEM_CHANGEAREA,            // 区域类型改变 stEntityChangeArea
        ENTITYSYSTEM_CHANGERENDEROBJ,       // 刷新renderobj stRefreshRenderObj
        ENTITYSYSTEM_CHANGE,                // 变身完成
        ENTITYSYSTEM_RESTORE,               // 恢复变身
        ENTITYSYSTEM_LEVELUP,               // 等级提升 stEntityLevelUp
        ENTITYSYSTEM_MAINPLAYERCREATE,      // 创建主角成功
        ENTITYSYSTEM_NPCHEADSTATUSCHANGED,  //npc头顶状态改变
        ENTITYSYSTEM_NEWNAME,               //mainplayer的 新名字

        PLAYER_FIGHTPOWER_REFRESH,          // 角色战斗力刷新

        //技能系统
        SKILLSYSTEM_SHOWDAMAGE = 2001,      //显示伤害
        SKILLSYSTEM_SHOWBUFFDAMAGE,         //显示buff伤害
        SKILLSYSTEM_USESKILL,               //使用技能
        SKILLGUIDE_PROGRESSSTART ,          //引导进度条开始
        SKILLGUIDE_PROGRESSBREAK,           //进度条打断
        SKILLGUIDE_PROGRESSBACK,            //进度条回退
        SKILLGUIDE_PROGRESSEND,             //引导进度条结束
        SKILLSYSYTEM_TAB,                   //按下tab键
        SKILLINFO_REFRESH,                  //技能信息刷新（设置面板位置信息，等级改变）
        SKILLNONESTATE_ENTER,               //技能状态改变
        SKILLCD_REFRESH,                    //技能cd刷新
        SKLL_LONGPRESS,                     //长按终止
        SKILLCD_BEGIN,                      //开始cd  stSkillCDChange
        SKILL_RELIVE,                       //技能复活
        SKILL_SHOWPETSKILL,                 //宠物技能显示
        SKILLSTATEINFO,                     //技能状态信息
        SKILL_FORBIDDENJOYSTICK,            //禁用摇杆
        SKILLSYSTEM_ADDSKILL,               //添加技能
        SKILLSYSTEM_REUSESKILLLONGATTACK,   //长按触发普攻
        SKILLSYSTEM_SETSKILLPOS,            //设置技能位置 stSetSkillPos
        SKILLSYSTEM_SKILLSTATECHANE,        //技能形态变化
        SKILLSYSTEM_SKILLLISTCHANE,         //技能列表变化
        SKILLSYSTEM_PETGETSKILL,            //宠物获取技能 stPetGetSkill
        SKILLSYSTEM_CHANGEMODEL,            //切换模型   stSkillChangeModel
        SKILLSYSTEM_CDEND ,                //技能CD结束
        SKILLSYSTEM_ADDSKILLCMD,            //添加技能指令到队列 stSkillCommond
        SKILLSYSTEM_CLEARSKILLCMD,          //清除技能指令队列
        SKILLSYSTEM_ONUSESKILL,             //点击某个技能 stOnSkillUse
        SKILLSYSTEM_STIFFTIMEOVER,          //技能僵直时间走完
        //buff
        BUFF_GETEFFECTTYPE = 3001,                     //获取buff效果类型
        //BUFF_ADDTOMAINROLEMAINPANEL = 3002,            //添加一个主角buff图标
        //BUFF_DELETEMAINROLEPANEL ,                     //删除一个主角buff图标
        BUFF_ADDTOTARGETBUFF,               // 添加Buff
        BUFF_DELETETARGETBUFF,              // 删除Buff
        BUFF_UPDATEARGETBUFF,               // buff数据 更新


        //邮件 4
        MAIL_DELATTACHMENT,                //成功领取后删除附件 
        MAIL_ADDNEWMAIL,                    //添加新邮件
        MAIL_STATECHANGE,                  // 邮件状态改变
        MAIL_STATECHANGENOATTACH,                //邮件列表更新
        MAIL_HIDETABWARNING,              //隐藏页签红点
        FRIEND_ADDNEWMSG,                 //好友发来新的信息
        //邮件和反馈
        SETTING_RECIEVEFEEDBACKNOTICE,      //设置面板的新反馈通知
        // 任务5
        TASK_VISITNPC = 5000,        //正在访问的NPC  
        TASK_DONING = 5001,          //开始做任务  可能去接取任务 也可能去执行当前接取的任务 或者去完成任务
        TASK_STATECHANEGE = 5002,     //任务状态改变
        TASK_ATTACKMONSTER = 5003,     //攻击怪物     
        TASK_DONE = 5004,           //任务完成
        TASK_CANSUBMIT = 5005, //任务可以提交
        TASK_ACCEPT = 5006,          //接受任务
        CHAPTER_EFFECT_END,
        TASK_DELETE ,           //任务删除 UINT 任务id
        TASK_CANACCEPT,           //任务可接 UINT 任务id
        TASK_MAIN_ARROWHIDE,        //主线任务追踪箭头隐藏
        TASK_ITEM_COLLECT_USE,          //任务里 采集物品或使用道具
        TASK_HELPJUMP_PROGRESSSTART,    //瞬间跨地图跳转

        //问卷调查
        REFRESHQUESTIONPANEL,

        //绑定手机
        GETVERIFYNUM,
        BINDPHONESUCESS,
        GOTBINDREWARD,

        
        //挂机8
        ROBOTCOMBAT_START = 8001,           // 开始挂机
        ROBOTCOMBAT_STOP,                   // 停止挂机

        ROBOTCOMBAT_PAUSE,                  // 暂停挂机
        ROBOTCOMBAT_USESKILLFAILEDPAUSE,    // 挂机使用技能失败
        ROBOTCOMBAT_NEXTCMD,                // 挂机技能连招
        ROBOTCOMBAT_PICKUPITEM,             // 挂机拾取
        ROBOTCOMBAT_COPYKILLWAVE,           // 挂机 副本进度
        ROBOTCOMBAT_SEARCHPATH,             // 寻路
        ROBOTCOMBAT_VISITNPC,               // 访问Npc
        COMBOT_ENTER_EXIT,                  //进入副本或者退出

        COPY_REWARDEXP,                      //副本阶段奖励经验总和

        //主界面9
        MAINBUTTON_ADD = 9001,              //添加主界面功能按钮
        JOYSTICK_PRESS = 9002,              //摇杆按下
        JOYSTICK_UNPRESS = 9003,            //摇杆松开
        SKILLBUTTON_CLICK = 9004,//点击主动释放技能按钮
        MAINBUTTON_STATUSCHANGE = 9005,//功能按钮状态改变
        MAINBUTTON_REDTIPS = 9006,//按钮红点 提示
        MAINRIGHTBTN_TOGGLE = 9007,//右边功能按钮显示状态
        MAINLEFTTBTN_TOGGLE = 9008,//左边功能按钮显示状态
        MAINBTN_ONTOGGLE = 9009,          //功能按钮显示状态
        HOTKEYPRESSDOWN = 9010,          //热键按下  stHotKeyDown

        REFRESHTRANSMITPUSHMSGSTATUS     ,      //刷新主界面传送推送类消息
        REFRESHINVITEPUSHMSGSTATUS,              //刷新主界面邀请推送类消息
        REFRESHDAILYPUSHSTATUS    ,             //刷新主界面日常预告
        REFRESHFUNCTIONPUSHOPEN,                //开启新功能
        
        // 家园 10
        HOMELAND_CLICKENTITY = 10001,   // 点击实体
        HOMELAND_BUYSEEDCUB = 10002,     
        HOMELAND_UPDATEANIMAL = 10003,

        HOME_BUYTREE,                   //买树
        HOME_TREEBEGIN,                 //开始集赞
        HOME_TREEGAIN,                  //树收获
        HOME_HELPSUCCESS,               //点赞成功

        REFRESHITEMDURABLE,             //刷新装备耐久

       
        //武斗场11
        ARENAENTERMAP = 12001,      //进入武斗场地图
        ARENASTARTBATTLECD = 12002,   //开始战斗CD
        ARENABATTLEEND = 12003,     //战斗结束
		RANKDATAREFRESH ,           //排行榜数据接收成功
        ARENAEXIT,                  //退出武斗场

        //组队
        TEAM_LEADERCALLFOLLOW = 13000, //队长召唤跟随        
        TEAM_LEADERCANCLEFOLLOW,       //队长取消跟随
        TEAM_MEMBERFOLLOW,             //队员手动跟随
        TEAM_MEMBERCANCLEFOLLOW,       //队员取消跟随
        TEAM_MATCH,                    //匹配
        TEAM_CANCLEMATCH,              //取消匹配
        TEAM_JOIN,                      //加入队伍
        TEAM_LEAVE,                     //离开队伍
        TEAM_MEMBERMAPID,               //同步队员mapId
        TEAM_MEMBERSTATE,               //同步队员状态
        TEAM_MEMBERLV,                  //同步队员等级


        //tips
        TIPS_EVENT,                   //tips 提示

         //称号
        TITLE_TIMEOUT = 14000,         //倒数计时为0
        TITLE_WEAR,                    //佩戴称号
        TITLE_ACTIVATE,                //激活称号
        TITLE_NEWTITLE,                //获得新的称号
        TITLE_USETIMES,                //增加使用次数
        TITLE_DELETE,                  //删除称号

           //VIP
        BUYNOBLESUCCESS,              //购买皇令成功
        GETNOBLEMONEYSUCCESS,         //领取皇令的每日文钱
        //氏族
        CLANQUIT = 16000,               //退出氏族
        CLANJOIN,                       //加入氏族
        CLANREFRESHID,                  //加入氏族刷新氏族id
        CLANDeclareInfoGet,             //服务器下发宣战信息   
        CLANDeclareInfoAdd,             //宣战增加增加
        CLANDeclareInfoRemove,          //宣战氏族移除
        
        CITYWARTOTEMCLANNAMECHANGE,     //图腾氏族
        CITYWARWINERCLANID,             //城战胜利者clanId


        DAILY_RESALLDATA = 17000,        //返回全部日常数据成功
        DAILY_RESSINGLEDATA,             //接收单个日常活动的反馈
        DAILY_GETREWARDBOXOVER,          //领取宝箱成功


        WELFARESTATEUPDATE = 18000,     //福利领取成功

        HEARTSKILLUPGRADE = 19000,      //心法升级
        HEARTSKILLRESET,                //重置心法
        HEARTSKILLGODDATA,              //心法神魔数据跟新

        USER_FASHION_WARR = 20000,      //玩家穿上时装
        USER_FASHION_TAKEOFF ,          //玩家脱下时装

        NVWASTART = 21000,              //女娲开始

        //充值活动
        RECHARGESINGLEDATA=22000,       //充值或消费返回
        RECHARGEGETREWARD,             //活动领奖后返回所有数据成功
        ISBUYGROWTH,                   //已买成长基金
        ISGETREWARD,                   //领取奖励成功
        ISFIRSTRECHARGE,               //首充成功

        //推送
        PUSH_ADDITEM = 23000,             //道具有增加
        HUNT_CHANGEBOSSSTATE= 24000,      //狩猎全部boss数据返回

        CAMP_ADDCOLLECTNPC,              //阵营战 采集npc
        //聊天
        CHAT_VOICE_PLAY = 26000,            //播放语音
        CHAT_JOINROOM,                      //加入房间
        CHAT_LEVELROOM,                      //离开房间
        CHAT_SETMICBTN,                        //设置麦克风开关 可见
        CHAT_SPEEKERNOW,                        //正在说话的玩家名称
        CHAT_MICKSTATE,                        //开麦克风 bool
        //寄售
        SHOWCONSIGNMENTLIST = 27000,          //唤醒寄售面板的物品售价信息列表

        //1000000以后给UI事件，客户端事件 100万以前 
        //UIEventStart
        //面板状态改变事件
        UIEVENT_PANELSTATUS = 1000000,
        //UI状态改变
        UIEVENTUISTATECHANGED,

        //UIpanelmangaer处理完面板状态发送改变事件
        UIEVENT_PANELSTATUSDATACHANGED,
        //面板获取焦点状态改变
        UIEVENT_PANELFOCUSSTATUSCHANGED,
        //钱币刷新
        UIEVENT_REFRESHCURRENCYNUM,
        //背包mask改变
        UIEVENT_KNAPSACKMASKCHANGED,
        //物品相关
        UIEVENT_UPDATEITEM,
        //活动仓库改变
        UIEVENT_ACTIVEWAREHOUSECHANGED,
        //解锁仓库
        UIEVENT_UNLOCKWAREHOSUE,
        //活动存放钱币改变
        UIEVENT_WAREHOUSESTORECOPPERCHANGED,
        //背包解锁状态改变
        UIEVENT_KNAPSACKUNLOCKINFOCHANGED,
        //商城相关
        UIEVENT_REFRESHINPUTMAXNUM,
        //商城购买次数改变
        UIEVENT_REFRESHPURCHASENUM,
        //商城标记状态改变
        UIEVENT_TAGSTATUSCHANGED,

        //宝石镶嵌状态改变
        UIEVENTGEMINLAYCHANGED,
        //装备合成完毕
        UIEVENTEQUIPCOMPOUNDCOMPLETE,
        //打开选中结果
        UIEVENTEQUIPCOMPOUNDOPENRESULT,
        //属性消除
        UIEVENTEQUIPROPERTYPREMOVE,
        //属性提升
        UIEVENTEQUIPROPERTYPPROMOTE,
        //激活符石
        UIEVENTRUNESTONEACTIVE,
        //装备精炼完成
        UIEVENTEQUIPREFINECOMPLETE,

        //item CD 使用物品cd
        UIEVENT_USEITEMCD,

        //圣魂进化成功
        UIEVENTMUHONEVOLUTION,
        //圣魂融合
        UIEVENTMUHONBLEND,
        //圣魂升级
        UIEVENTMUHONUPGRADE,
        //吃经验丹
        UIEVENT_REFRESHMUHONEXP,

        //装备格强化
        //装备格强化等级改变
        UIEVENT_GRIDSTRENGTHENLVCHANGED,
        //装备格强化激活套装属性改变
        UIEVENT_STRENGTHENACTIVESUITCHANGED,
        //颜色套装属性变化
        UIEVENT_EQUIPCOLORSUITCHANGE,
        //宝石套装属性变化
        UIEVENT_EQUIPSTONESUITCHANGE,

        //黑市刷新事件
        UIEVENTBLACKMARKETREFRESH,
        //黑市刷新倒计时
        UIEVENTBLACKMARKETREFRESHTIMECOUNTDOWN,
        //黑市商品售罄
        UIEVENTBLACKMARKETITEMSOLDOUT,
        //黑市活动页签改变
        UIEVENTBLACKACTIVETABCHANGED,
        //黑市活动商城改变
        UIEVENTBLACKACTIVESTORECHANGED,

        //退出氏族
        UIEVENTQUITCLAN,
        //加入氏族
        UIEVENTJOINCLAN,
        //氏族解散
        UIEVENTDISSOLVECLAN,
        //氏族转正
        UIEVENTPOSITIVECLAN,
        //临时氏族生命周期改变
        UIEVENTTEMPCLANEXISTTIMECHANGED,
        //临时氏族支持人数改变
        UIEVENTTEMPCLANSUPNUMCHANGED,
        //临时氏族列表改变
        UIEVENTCLANLISTCHANGED,
        //氏族创建
        UIEVENTCLANCREATE,
        //刷新氏族
        UIEVENTCLANUPDATE,
        //更新公告
        UIEVENTCLANGGUPDATE,
        //更新成员信息
        UIEVENTCLANMEMBERUPDATE,
        //氏族捐献成功
        UIEVENTCLANDONATESUCCESS,
        //刷新氏族捐献数据
        UIEVENTREFRESHCLANDONATEDATAS,
        //从服务的同步氏族荣誉信息
        UIEVENTGETCLANHONORDATAS,
        //氏族荣誉信息变更
        UIEVENTGETCLANHONORCHANGED,
        //氏族升级
        UIEVENTCLANUPGRADE,
        //氏族申请信息改变
        UIEVENTCLANAPPLYINFOCHANGED,
        //刷新氏族申请列表
        UIEVENTREFRESHCLANAPPLYINFO,
        //用户申请氏族列表改变
        UIEVENTUSERAPPLYCLANLISTCHANGED,
        //氏族学习技能改变
        UIEVENTUSERLEARNCLANSKILLCHANGED,
        //氏族研发技能改变
        UIEVENTCLANDEVSKILLCHANGED,
        //氏族成员操作界面关闭
        UIEVENTCLANMEMBEROPDIALOGCLOSED,
        //氏族任务状态改变
        UIEVENTCLANTASKCHANGED,
        //刷新氏族敌对势力
        UIEVENTCLANRIVALRYREFRESH,
        //变更氏族宣战信息
        UIEVENTCLANRIVALRYCHANGED,
        //氏族宣战历史信息
        UIEVENTCLANRIVALRYHISTORYREFRESH,
        //氏族宣战历史信息
        UIEVENTCLANDECLARESEARCHREFRESH,
        //发起氏族站
        UIEVENTCLANSTARTDECLAREWAR,

        //时装丢弃
        UIEVENTFASHIONDISCARD,
        //时装数据改变
        UIEVENTFASHIONCHANGED,
        //穿戴時裝
        UIEVENTFASHIONWEAR,
        //穿默认装
        UIEVENTFASHIONWEARDEFAULT,
        //脫下時裝
        UIEVENTFASHIONTAKEOFF,
        //时装数据改变
        UIEVENTFASHIONADD,
        //刷新时装数据
        UIEVENTFASHIONDATA,

        //新功能开启
        //执行新功能开启提示
        UIEVENTDONEWFUNCOPENNOTICE,
        //新功能开启添加
        UIEVENTNEWFUNCOPENADD,
        //新功能READ
        UIEVENTNEWFUNCOPENREAD,
        //新功能开启初始化完成
        //UIEVENTNEWFUNCOPENDATAINITCOMPLETE,
        //新功能开启
        UIEVENTNEWFUNCOPEN,
        //新功能开启展示完成
        UIEVENTNEWFUNCCOMPLETE,
        //页签功能开启
        UIEVENTTABFUNCOPEN,

        //游戏对象移动过可见状态变化（为引导设计）
        UIEVENTGAMEOBJMOVESTATUSCHANGED,
        //刷新非强制引导状态
        UIEVENTREFRESHUNCONSTRAINTGUIDESTATUS,

        //新引导添加
        UIEVENTGUIDEADD,
        //引导数据初始化完成
        //UIEVENTGUIDEDATAINITCOMPLETE,
        //执行引导提示
        //UIEVENTDOGUIDENOTICE,
        //引导展示完成
        UIEVENTGUIDECOMPLETE,
        UIEVENTGUIDESKIP,
        UIEVENTGUIDESHOWOUT,
        
        UIEVENTGUIDEHIDE,
        //重置引导步骤
        UIEVENTGUIDERESET,
        //重复触发引导
        UIEVENTGUIDERECYCLETRIGGER,

        //进行引导工作流检测
        UIEVENTDOGUIDEWORKFLOWCHECK,

        //工作流检测完成
        UIEVENTGUIDEWORKFLOWCHECKCOMPLETE,
        //执行引导流
        UIEVENTDOGUIDEWORKFLOW,
        
        //引导数据获取焦点
        UIEVENTGUIDEITEMFOCUS,
        //屏幕中央效果
        UIEVENTADDDISPLAYEFFECT,
        //清空屏幕中央效果缓存
        UIEVENTCLEARDISPLAYCACHE,
        //屏幕中央效果展示
        UIEVENTPlayDISPLAYEFFECT,
        //屏幕中央粒子效果完成
        UIEVENTDISPLAYEFFECTCOMPLETE,
        //阵营战报名信息列表刷新
        UIEVENTCAMPSIGNINFOSREFRESH,
        //单个阵营战报名信息改变
        UIEVENTCAMPSIGNINFOCHANGED,
        //刷新阵营战场次
        UIEVENTCAMPLEFTTIMESREFRESH,

        //跟新主界面快捷使用道具UI布局
        UIEVENT_SHORTCUTITEMUI,
        //世界Boss伤害排名刷新
        UIEVENT_WORLDBOSSDAMRANKREFRESH,
        //刷新世界Boss状态信息
        UIEVENT_WORLDBOSSSTATUSREFRESH,
        //刷新单个世界Boss状态信息
        UIEVENT_WORLDBOSSSINGLESTATUSREFRESH,
        //刷新世界Boss鼓舞状态
        UIEVENT_WORLDBOSSINSPIREREFRESH,
        //同步服务器时间
        SYNSERVERTIME,


        MAINPANEL_SHOWREDWARING,             //显示红点  stShowMainPanelRedPoint

        HIDETEXIAO_OTHER,               //是否显示其他玩家特效
        HIDETEXIAO_MINE,                //是否显示自己特效
        HIDEFASHION,                    //是否显示时装
        HIDEOTHERPLAYER,                //屏蔽其他玩家
        HIDEMONSTER,                    //屏蔽怪物

        UISHOWSLIDEREVENT,                  //显示进度条 stSliderBeginEvent
        UIHIDESLIDEREVENT,                  //隐藏进度条


        SEVENDAYOPENSTATUS,                 //七天乐是否开启

        GODWEAPENSTATUS,                   //神兵是否已经激活了

        FIRSTRECHARGESTATUS,               //首充是否已经完成

        OPENSERVERGIFTSTATUS,              //开服豪礼是否已完成

        RETREWARDSTATUS,                   //内测返利是否已完成

        QUESTIONSTATUS,                   //问卷是否需要屏蔽

        RFRESHENTITYHEADSTATUS,             //实体头顶状态刷新

        SHAKEINGPHONEMSG,                   //玩家摇晃手机

        REFRESHWORLDLEVEL,                 //刷新世界等级

        EJOYSTICKSTABLE,                   //摇杆控制

        REFRESHACHIEVEMENTPUSH,          //刷新成就推送

        REFRESHPKVALUEREMAINTIME,         //刷新pk值剩余时间

        SHOWHTTPDOWNUI,
        CAMERA_MOVE_END,

    } 

    // 投票事件 询问是否可以执行
    public enum GameVoteEventID
    {
        // 投票事件按系统分类
        ENTITYSYSTEM_VOTE_ENTITYMOVE = 1001, // 移动投票事件
        SKILL_CANUSE ,                      //是否能够使用技能
        SKILL_CDING,                        //skill in cd?
        CHANGE_ANI,                         //切换动作
        SKILL_FASTMOVE,                     //是否在执行快速移动技能（疾风斩）
        SEND_STOPMOVE,                      //是否发送stopmove消息
        AUTORECOVER,                        //自动吃药（普通药和瞬药）
        AUTOAtOnceRECOVER,                  //自动吃瞬药（只有瞬药）
        USE_MEDECAL_ITEM,                   //是否能使用物品
        RIDE_CANRIDE,                       //是否能骑乘
        TASK_VISITNPC_COLLECT,            //是否播放采集动作
        MAINPANEL_SHOWWARNING,              //是否显示主界面各个模块的红点

        
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    // GameEventID 事件参数
    // SYSTEM_LOADSCENECOMLETE  // 场景加载完毕
    public struct stLoadSceneComplete
    {
        public int nMapID;  // 地图ID
        public bool bFirstLoadScene;
    }
      //    SKILL_SHOWPETSKILL,                 //宠物技能显示
    public struct stShowPetSkill
    {
        public bool bShow;
    }
    // ENTITYSYSTEM_CREATEENTITY
    public struct stCreateEntity
    {
        //public Client.EntityType type; // 实体类型
        //public uint uid;   // 实体ID;
        public long uid;        // 实体ID;
        public uint boxTime; //box 归属变化时间
    }
    public struct stSkillRelive
    {
        public uint id;
    }
    // ENTITYSYSTEM_REMOVEENTITY
    public struct stRemoveEntity
    {
        //public Client.EntityType type; // 实体类型
        //public uint uid;   // 实体ID;
        public long uid;
    }

    // ENTITYSYSTEM_ENTITYBEGINMOVE
    public struct stEntityBeginMove
    {
        public long uid;    // 实体ID
    }
    //ENTITYSYSTEM_SETHIDE
    public struct stEntityHide
    {
        public long uid;
        public bool bHide;
    }
    // ENTITYSYSTEM_ENTITYMOVE
    public struct stEntityMove
    {
        public long uid;        // 实体ID;
        public Vector2 pos;     // 实体位置
    }

    // ENTITYSYSTEM_ENTITYSTOPMOVE
    public class stEntityStopMove
    {
        public long uid = 0;                // 实体ID;
        public Vector2 pos = Vector2.zero;  // 实体位置
        public bool bExternal = true;       // 外部因素
    }

    // ENTITYSYSTEM_TARGETCHANGE
    public struct stTargetChange
    {
        public IEntity target;   // 目标实体
    }

    // ENTITYSYSTEM_PROPUPDATE 属性更新
    public struct stPropUpdate
    {
        public long uid;
        public int nPropIndex;
        public int oldValue;
        public int newValue;
    }

    /// <summary>
    /// 新名字
    /// </summary>
    public struct stNewName 
    {
        public string newName;
    }

    // ENTITYSYSTEM_ENTITYDEAD 实体死亡
    public struct stEntityDead
    {
        public long uid;
    }


    // ENTITYSYSTEM_ENTITYDEAD 实体复活
    public struct stEntityRelive
    {
        public long uid;
    }

    // ENTITYSYSTEM_CHANGENAME 实体死亡
    public struct stEntityChangename
    {
        public long uid;
    }

    // ENTITYSYSTEM_BEGINRIDE
    public struct stEntityBeginRide
    {
        public long uid;
    }

    // ENTITYSYSTEM_RIDE 上马
    public struct stEntityRide
    {
        public long uid;
    }

    // ENTITYSYSTEM_UNRIDE 下马
    public struct stEntityUnRide
    {
        public long uid;
    }

    // ENTITYSYSTEM_CHANGEAREA
    public struct stEntityChangeArea
    {
        public long uid;
        public MapAreaType eType;
    }

    // ENTITYSYSTEM_LEVELUP
    public struct stEntityLevelUp
    {
        public long uid;
        public int nLastLevel;
        public int nLevel;
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Buff System
    // BUFF_ADDTOTARGETBUFF
    public struct stAddBuff
    {
        public long uid;        // 对象ID
        public uint buffid;     // buff baseid
        public uint level;      // 等级
        public long lTime;      // 持续时间
        public uint buffThisID; //后台唯一id
    }
    // BUFF_DELETETARGETBUFF
    public struct stRemoveBuff
    {
        public long uid;
        public uint buffid;
        public uint buffThisID; //后台唯一id
    }
    //BUFF_UPDATEARGETBUFF
    public struct stUpdateBuff
    {
        public long uid;
        public uint buffid;
        public uint level;
        public long lTime;
        public uint buffThisID; //后台唯一id 
    }
    //进度条
    public struct stUninterruptMagic
    {
        public GameCmd.UninterruptActionType type;
        public long uid;
        public uint time;
        public uint npcId;
    }

    //MAIL_ADDNEWMAIL     有新邮件
    public struct stMailAddNew
    {
        public uint mailid;
    }
    // MAIL_STATECHANGE   这是有附件的邮件状态改变
    public struct stMailStateChange
    {
        public uint mailid;
        public int index;
    }

    //MAIL_STATECHANGENOATTACH    这是没有附件的邮件状态改变
    public struct stMailStateChangeNoAttach
    {
        public uint mailid;
        public int index;
    }
    //
  
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    // ENTITYSYSTEM_VOTE_ENTITYMOVE
    public struct stVoteEntityMove
    {
        public long uid;        // 实体ID;
    }


    public struct stSkillStateEnter
    {
        public long uid;//entity id
        public int state;
    }

    public struct stGuildBreak
    {
        public uint uid;
        public GameCmd.UninterruptActionType action;
        public uint skillid;
        public string msg;
        public uint npcId;
    }
    public struct stGuildEnd
    {
        public uint uid;
        public GameCmd.UninterruptActionType action;
        public uint npcId;
    }
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    // 家园
    // HOMELAND_CLICKENTITY
    public struct stClickEntity
    {
        public long uid;        // 实体ID;
    }
    public struct stSkillCDChange
    {
        public uint skillid;
        public int cd;//-1代表cd要读取配置
    }

    ////////////////////任务
    public struct stDoingTask
    {
        public uint taskid;
        public uint state;
        public uint oprate;
        public string desc;
    }

    public struct stTipsEvent
    {
        public uint errorID;
        public string tips;
    }

    public struct stTaskDone
    {
        public uint taskid;
    }

    public struct stTaskCanSubmit
    {
        public uint taskid;
        public string desc;
        public uint state;
        public uint oprate;
    }

    public struct stTeamTargetActivity 
    {
        public uint teamTargetActivityId;
        
    }
    public struct stSkillStateInfo
    {
        public long casterID;//施法者iD
        public SkillState stateID;//技能状态
        public int actionID;//enter = 1 update = 2,levave = 3
    }

    public struct stForbiddenJoystick
    {
        public long playerID;
        public bool bFobidden;//是否禁用
    }

    public struct stMainButtonStatus
    {
        public int btnID;
        public int status; //1 开放 2 关闭 
    }

    public struct stMainButtonRedTips
    {
        public int btnID;
        public int status; //1 添加 0 移除 
    }
    public struct stEntityBeginDead
    {
        public long uid;
    }
    public struct stShowDemage
    {
        public long uid;
        public uint skillid;
        public uint attackID;//攻击者id
        public uint attacktype;
        public List<uint> filterList;//忽略扣血列表
        public List<GameCmd.stMultiAttackDownMagicUserCmd_S.stDefender> defenerList;
    }
    public struct stBuffDamage
    {
        public long uid;
        public uint damagetype;//HPChangeType
        public int changeValue;
        public int entityType;
        public uint curHp;
    }

    public struct stPickUpItem
    {
        public uint itemid;
        public int state;//0--待捡起 1--捡起动作已执行
    }

    public struct stSkillDoubleHit
    {
        public uint skillID;
        public bool doubleHitEnd;
    }

    public struct stCopySkillWave
    {
        public uint waveId;
        public uint posIndex;
    }

    public struct stCopyRewardExp 
    {
        public uint exp;
    }

    public struct stCampCollectNpc
    {
        public long npcid;
        public bool enter;
    }
    public struct stFirstRecharge 
    {
        public uint rechargeId;
    }
    /// <summary>
    /// 长按事件
    /// </summary>
    public struct stSkillLongPress
    {
    //    public long userID;//uid
        public bool bLongPress;//true 表示长按触发
    }

    public struct stVisitNpc
    {
        public uint npcid;
        public bool state;
    }

    //PLAYER_FIGHTPOWER_REFRESH 事件参数
    public struct stRefreshPowerParams
    {
        //老的战斗力值
        public int PreFightPower;
        //当前战斗力值
        public int CurFightPower;
    }

    public struct stMainBtnSet
    {
        public int pos; // 1 left 2 right
        public bool isShow;
    }

    public struct stRefreshRenderObj
    {
        public uint userID;//用户id
        public uint suitID;//时装id
        public int suitType;//时装类型
    }
    public struct stVoicePlay
    {
        public string fileId;
    }

    public struct stVoiceJoinRoom
    {
        public bool succ;
        public string name;
        public int memberid;
    }
    public struct stVoiceLevelRoom
    {
        public bool succ;
    }

    public struct stRankType
    {
        public uint rankId;
    }

    public struct stTeamJoin
    {
        public uint teamId;
        public string teamName;
    }

    public struct stClanJoin
    {
        public long uid;
        public uint clanId;
        public string clanName;
    }
    public struct stClanQuit
    {
        public long uid;
        public uint clanId;
        public string clanName;
    }

    public struct stClanUpdate
    {
        public long uid;
        public uint clanId;
    }

    public struct stPlayerChange
    {
        public int status;//1 变身 2 恢复
        public uint uid;
    }
    public struct stSetSkillPos
    {
        public uint pos;//技能位置
        public uint skillid;//技能id
    }
    public struct stDailyBoxParam 
    {
        public uint boxID;
    }
    public struct stHotKeyDown
    {
        public uint type;//模块类型
        public uint pos;//按钮位置
    }
    public struct stReconnectSucess
    {
        public bool isLogin;//表示重连成功后是否走登录流程，true表示走登录流程
    }
    public struct stNobleTempIndex 
    {
        public uint nobleID;
    }
    public struct stPetGetSkill
    {
        public uint petID;
        public uint skillID;
    }

    public struct stTeamActivityTarget 
    {
        public uint activityTargetId;
    }

    public struct stTitleWear 
    {
        public uint uid;
        public uint titleId;
    }
    public struct stSkillChangeModel
    {
        public uint userID;
        public int skillStatus;
    }

    public struct stUseItemCD 
    {
        public uint itemBaseId;
    }

    public struct stCombotCopy
    {
        public uint copyid;
        public bool enter;
        public bool exit;
    }

    public struct stTaskNpcItem
    {
        public uint type;  //类型  1为npc  2为item
        public uint Id;    //id
    }

    public struct stShowMainPanelRedPoint
    {
        public int direction;//左侧还是右侧
        public int modelID;//WarningEnum
        public bool bShowRed;//
    }

    public struct stDoNextSkill
    {
        public uint skillID;
    }

    public struct stSkillCommond
    {
        public uint type;//0是技能 1是移动 2是挂机
        public uint skillID;//技能id
    }

    public struct stTeamMemberBtn 
    {
        public uint id;     //队伍成员ID
        public string name; //name
        public float pos_y;  
    }

    public struct stSliderBeginEvent
    {
        public float dur;//时间长度
        public uint sliderType;// 0-7被UninterruptActionType 占用 自定义的请放在后面
    }
    public struct stOnSkillUse //使用某个技能  SKILLSYSTEM_ONUSESKILL
    {
        public uint skillID;//技能id 
    }

    public struct stNextSkill
    {
        public uint curSkillID;
        public uint nextSkillID;
    }
}