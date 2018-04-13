//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class DailyPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//日常
		RiChang = 1,
		//狩猎
		ShouLie = 2,
		//聚宝
		JuBao = 3,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_DailyPage;

    UIGridCreatorBase    m_ctor_TabRoot;

    Transform            m_trans_DailyNormalRoot;

    UILabel              m_label_Name;

    UISprite             m_sprite_Icon;

    Transform            m_trans_RewardGrid;

    UILabel              m_label_Times;

    UILabel              m_label_Active;

    UILabel              m_label_Require;

    UILabel              m_label_ActivityTime;

    UILabel              m_label_ActivityDesc;

    UIGridCreatorBase    m_ctor_ContentScrollView;

    UILabel              m_label_MyActive_Num;

    UISlider             m_slider_Activeslider;

    UIGridCreatorBase    m_ctor_DailyRewardRoot;

    Transform            m_trans_CalendarPage;

    UIGridCreatorBase    m_ctor_Monday;

    UIGridCreatorBase    m_ctor_Tuesday;

    UIGridCreatorBase    m_ctor_Wednesday;

    UIGridCreatorBase    m_ctor_Thursday;

    UIGridCreatorBase    m_ctor_Friday;

    UIGridCreatorBase    m_ctor_Saturday;

    UIGridCreatorBase    m_ctor_Sunday;

    Transform            m_trans_DetailContent;

    UIButton             m_btn_ColliderBg;

    UISprite             m_sprite_Sign;

    UISprite             m_sprite_CalendarIcon;

    Transform            m_trans_CalendarRewardGrid;

    UILabel              m_label_miaoshu;

    UILabel              m_label_shijian;

    UILabel              m_label_xingshi;

    UIButton             m_btn_JoinBtn;

    UILabel              m_label_huodongmingcheng;

    UILabel              m_label_cishu;

    UILabel              m_label_huoyuezhi;

    Transform            m_trans_TreasureBossPage;

    UIButton             m_btn_Dailytips_1;

    UIGridCreatorBase    m_ctor_TreasureScrollView;

    UIGridCreatorBase    m_ctor_TreasureTypeRoot;

    UILabel              m_label_TBName;

    Transform            m_trans_PersonalRewardContent;

    UIGridCreatorBase    m_ctor_PersonalRewardRoot;

    UILabel              m_label_reward_time;

    UILabel              m_label_RecommandPowerValue;

    Transform            m_trans_WorldRewardContent;

    UIGridCreatorBase    m_ctor_WorldRewardRoot;

    UIGridCreatorBase    m_ctor_FinalKillRewardRoot;

    UILabel              m_label_WorldBossOpenDes;

    UILabel              m_label_PreRoundLastAttackName;

    UILabel              m_label_PreRoundMaxDamName;

    UIButton             m_btn_TBBtn_Go;

    UILabel              m_label_TBMapLabel;

    UIButton             m_btn_TBBtn_shop;

    Transform            m_trans_HuntingPage;

    UIGridCreatorBase    m_ctor_ListScrollView;

    UIGridCreatorBase    m_ctor_HuntingTypeRoot;

    UILabel              m_label_BossName;

    UISprite             m_sprite_RewardBg;

    Transform            m_trans_RewardRoot;

    UIButton             m_btn_Btn_Go;

    UILabel              m_label_mapLabel;

    UIButton             m_btn_btn_shop;

    UILabel              m_label_LieHunLabel;

    UILabel              m_label_LieHunNum;

    UIButton             m_btn_infoBtn;

    UIButton             m_btn_infoContent;

    UILabel              m_label_tipsContent;

    UIButton             m_btn_Dailytips_2;

    UITexture            m__Model;

    Transform            m_trans_UIDailyGrid;

    UILabel              m_label_Frequency_Num;

    UILabel              m_label_Active_Num;

    UILabel              m_label_Label;

    Transform            m_trans_UITabGrid;

    Transform            m_trans_UIDailyCalendarGrid;

    UIToggle             m_toggle_UIHuntingToggleGrid;

    UIToggle             m_toggle_UIHuntingListGrid;

    Transform            m_trans_UIItemRewardGrid;

    Transform            m_trans_UIDailyRewardGrid;

    UIToggle             m_toggle_UITreasureBossGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_DailyPage = fastComponent.FastGetComponent<Transform>("DailyPage");
       if( null == m_trans_DailyPage )
       {
            Engine.Utility.Log.Error("m_trans_DailyPage 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_TabRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("TabRoot");
       if( null == m_ctor_TabRoot )
       {
            Engine.Utility.Log.Error("m_ctor_TabRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DailyNormalRoot = fastComponent.FastGetComponent<Transform>("DailyNormalRoot");
       if( null == m_trans_DailyNormalRoot )
       {
            Engine.Utility.Log.Error("m_trans_DailyNormalRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_Name = fastComponent.FastGetComponent<UILabel>("Name");
       if( null == m_label_Name )
       {
            Engine.Utility.Log.Error("m_label_Name 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Icon = fastComponent.FastGetComponent<UISprite>("Icon");
       if( null == m_sprite_Icon )
       {
            Engine.Utility.Log.Error("m_sprite_Icon 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RewardGrid = fastComponent.FastGetComponent<Transform>("RewardGrid");
       if( null == m_trans_RewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_RewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_Times = fastComponent.FastGetComponent<UILabel>("Times");
       if( null == m_label_Times )
       {
            Engine.Utility.Log.Error("m_label_Times 为空，请检查prefab是否缺乏组件");
       }
        m_label_Active = fastComponent.FastGetComponent<UILabel>("Active");
       if( null == m_label_Active )
       {
            Engine.Utility.Log.Error("m_label_Active 为空，请检查prefab是否缺乏组件");
       }
        m_label_Require = fastComponent.FastGetComponent<UILabel>("Require");
       if( null == m_label_Require )
       {
            Engine.Utility.Log.Error("m_label_Require 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActivityTime = fastComponent.FastGetComponent<UILabel>("ActivityTime");
       if( null == m_label_ActivityTime )
       {
            Engine.Utility.Log.Error("m_label_ActivityTime 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActivityDesc = fastComponent.FastGetComponent<UILabel>("ActivityDesc");
       if( null == m_label_ActivityDesc )
       {
            Engine.Utility.Log.Error("m_label_ActivityDesc 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ContentScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ContentScrollView");
       if( null == m_ctor_ContentScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ContentScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyActive_Num = fastComponent.FastGetComponent<UILabel>("MyActive_Num");
       if( null == m_label_MyActive_Num )
       {
            Engine.Utility.Log.Error("m_label_MyActive_Num 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Activeslider = fastComponent.FastGetComponent<UISlider>("Activeslider");
       if( null == m_slider_Activeslider )
       {
            Engine.Utility.Log.Error("m_slider_Activeslider 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_DailyRewardRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("DailyRewardRoot");
       if( null == m_ctor_DailyRewardRoot )
       {
            Engine.Utility.Log.Error("m_ctor_DailyRewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CalendarPage = fastComponent.FastGetComponent<Transform>("CalendarPage");
       if( null == m_trans_CalendarPage )
       {
            Engine.Utility.Log.Error("m_trans_CalendarPage 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Monday = fastComponent.FastGetComponent<UIGridCreatorBase>("Monday");
       if( null == m_ctor_Monday )
       {
            Engine.Utility.Log.Error("m_ctor_Monday 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Tuesday = fastComponent.FastGetComponent<UIGridCreatorBase>("Tuesday");
       if( null == m_ctor_Tuesday )
       {
            Engine.Utility.Log.Error("m_ctor_Tuesday 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Wednesday = fastComponent.FastGetComponent<UIGridCreatorBase>("Wednesday");
       if( null == m_ctor_Wednesday )
       {
            Engine.Utility.Log.Error("m_ctor_Wednesday 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Thursday = fastComponent.FastGetComponent<UIGridCreatorBase>("Thursday");
       if( null == m_ctor_Thursday )
       {
            Engine.Utility.Log.Error("m_ctor_Thursday 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Friday = fastComponent.FastGetComponent<UIGridCreatorBase>("Friday");
       if( null == m_ctor_Friday )
       {
            Engine.Utility.Log.Error("m_ctor_Friday 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Saturday = fastComponent.FastGetComponent<UIGridCreatorBase>("Saturday");
       if( null == m_ctor_Saturday )
       {
            Engine.Utility.Log.Error("m_ctor_Saturday 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Sunday = fastComponent.FastGetComponent<UIGridCreatorBase>("Sunday");
       if( null == m_ctor_Sunday )
       {
            Engine.Utility.Log.Error("m_ctor_Sunday 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailContent = fastComponent.FastGetComponent<Transform>("DetailContent");
       if( null == m_trans_DetailContent )
       {
            Engine.Utility.Log.Error("m_trans_DetailContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ColliderBg = fastComponent.FastGetComponent<UIButton>("ColliderBg");
       if( null == m_btn_ColliderBg )
       {
            Engine.Utility.Log.Error("m_btn_ColliderBg 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Sign = fastComponent.FastGetComponent<UISprite>("Sign");
       if( null == m_sprite_Sign )
       {
            Engine.Utility.Log.Error("m_sprite_Sign 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_CalendarIcon = fastComponent.FastGetComponent<UISprite>("CalendarIcon");
       if( null == m_sprite_CalendarIcon )
       {
            Engine.Utility.Log.Error("m_sprite_CalendarIcon 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CalendarRewardGrid = fastComponent.FastGetComponent<Transform>("CalendarRewardGrid");
       if( null == m_trans_CalendarRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_CalendarRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_miaoshu = fastComponent.FastGetComponent<UILabel>("miaoshu");
       if( null == m_label_miaoshu )
       {
            Engine.Utility.Log.Error("m_label_miaoshu 为空，请检查prefab是否缺乏组件");
       }
        m_label_shijian = fastComponent.FastGetComponent<UILabel>("shijian");
       if( null == m_label_shijian )
       {
            Engine.Utility.Log.Error("m_label_shijian 为空，请检查prefab是否缺乏组件");
       }
        m_label_xingshi = fastComponent.FastGetComponent<UILabel>("xingshi");
       if( null == m_label_xingshi )
       {
            Engine.Utility.Log.Error("m_label_xingshi 为空，请检查prefab是否缺乏组件");
       }
        m_btn_JoinBtn = fastComponent.FastGetComponent<UIButton>("JoinBtn");
       if( null == m_btn_JoinBtn )
       {
            Engine.Utility.Log.Error("m_btn_JoinBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_huodongmingcheng = fastComponent.FastGetComponent<UILabel>("huodongmingcheng");
       if( null == m_label_huodongmingcheng )
       {
            Engine.Utility.Log.Error("m_label_huodongmingcheng 为空，请检查prefab是否缺乏组件");
       }
        m_label_cishu = fastComponent.FastGetComponent<UILabel>("cishu");
       if( null == m_label_cishu )
       {
            Engine.Utility.Log.Error("m_label_cishu 为空，请检查prefab是否缺乏组件");
       }
        m_label_huoyuezhi = fastComponent.FastGetComponent<UILabel>("huoyuezhi");
       if( null == m_label_huoyuezhi )
       {
            Engine.Utility.Log.Error("m_label_huoyuezhi 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TreasureBossPage = fastComponent.FastGetComponent<Transform>("TreasureBossPage");
       if( null == m_trans_TreasureBossPage )
       {
            Engine.Utility.Log.Error("m_trans_TreasureBossPage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Dailytips_1 = fastComponent.FastGetComponent<UIButton>("Dailytips_1");
       if( null == m_btn_Dailytips_1 )
       {
            Engine.Utility.Log.Error("m_btn_Dailytips_1 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_TreasureScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("TreasureScrollView");
       if( null == m_ctor_TreasureScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_TreasureScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_TreasureTypeRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("TreasureTypeRoot");
       if( null == m_ctor_TreasureTypeRoot )
       {
            Engine.Utility.Log.Error("m_ctor_TreasureTypeRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_TBName = fastComponent.FastGetComponent<UILabel>("TBName");
       if( null == m_label_TBName )
       {
            Engine.Utility.Log.Error("m_label_TBName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PersonalRewardContent = fastComponent.FastGetComponent<Transform>("PersonalRewardContent");
       if( null == m_trans_PersonalRewardContent )
       {
            Engine.Utility.Log.Error("m_trans_PersonalRewardContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_PersonalRewardRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("PersonalRewardRoot");
       if( null == m_ctor_PersonalRewardRoot )
       {
            Engine.Utility.Log.Error("m_ctor_PersonalRewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_reward_time = fastComponent.FastGetComponent<UILabel>("reward_time");
       if( null == m_label_reward_time )
       {
            Engine.Utility.Log.Error("m_label_reward_time 为空，请检查prefab是否缺乏组件");
       }
        m_label_RecommandPowerValue = fastComponent.FastGetComponent<UILabel>("RecommandPowerValue");
       if( null == m_label_RecommandPowerValue )
       {
            Engine.Utility.Log.Error("m_label_RecommandPowerValue 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WorldRewardContent = fastComponent.FastGetComponent<Transform>("WorldRewardContent");
       if( null == m_trans_WorldRewardContent )
       {
            Engine.Utility.Log.Error("m_trans_WorldRewardContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_WorldRewardRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("WorldRewardRoot");
       if( null == m_ctor_WorldRewardRoot )
       {
            Engine.Utility.Log.Error("m_ctor_WorldRewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_FinalKillRewardRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("FinalKillRewardRoot");
       if( null == m_ctor_FinalKillRewardRoot )
       {
            Engine.Utility.Log.Error("m_ctor_FinalKillRewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_WorldBossOpenDes = fastComponent.FastGetComponent<UILabel>("WorldBossOpenDes");
       if( null == m_label_WorldBossOpenDes )
       {
            Engine.Utility.Log.Error("m_label_WorldBossOpenDes 为空，请检查prefab是否缺乏组件");
       }
        m_label_PreRoundLastAttackName = fastComponent.FastGetComponent<UILabel>("PreRoundLastAttackName");
       if( null == m_label_PreRoundLastAttackName )
       {
            Engine.Utility.Log.Error("m_label_PreRoundLastAttackName 为空，请检查prefab是否缺乏组件");
       }
        m_label_PreRoundMaxDamName = fastComponent.FastGetComponent<UILabel>("PreRoundMaxDamName");
       if( null == m_label_PreRoundMaxDamName )
       {
            Engine.Utility.Log.Error("m_label_PreRoundMaxDamName 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TBBtn_Go = fastComponent.FastGetComponent<UIButton>("TBBtn_Go");
       if( null == m_btn_TBBtn_Go )
       {
            Engine.Utility.Log.Error("m_btn_TBBtn_Go 为空，请检查prefab是否缺乏组件");
       }
        m_label_TBMapLabel = fastComponent.FastGetComponent<UILabel>("TBMapLabel");
       if( null == m_label_TBMapLabel )
       {
            Engine.Utility.Log.Error("m_label_TBMapLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TBBtn_shop = fastComponent.FastGetComponent<UIButton>("TBBtn_shop");
       if( null == m_btn_TBBtn_shop )
       {
            Engine.Utility.Log.Error("m_btn_TBBtn_shop 为空，请检查prefab是否缺乏组件");
       }
        m_trans_HuntingPage = fastComponent.FastGetComponent<Transform>("HuntingPage");
       if( null == m_trans_HuntingPage )
       {
            Engine.Utility.Log.Error("m_trans_HuntingPage 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ListScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ListScrollView");
       if( null == m_ctor_ListScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ListScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_HuntingTypeRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("HuntingTypeRoot");
       if( null == m_ctor_HuntingTypeRoot )
       {
            Engine.Utility.Log.Error("m_ctor_HuntingTypeRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_BossName = fastComponent.FastGetComponent<UILabel>("BossName");
       if( null == m_label_BossName )
       {
            Engine.Utility.Log.Error("m_label_BossName 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_RewardBg = fastComponent.FastGetComponent<UISprite>("RewardBg");
       if( null == m_sprite_RewardBg )
       {
            Engine.Utility.Log.Error("m_sprite_RewardBg 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RewardRoot = fastComponent.FastGetComponent<Transform>("RewardRoot");
       if( null == m_trans_RewardRoot )
       {
            Engine.Utility.Log.Error("m_trans_RewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Go = fastComponent.FastGetComponent<UIButton>("Btn_Go");
       if( null == m_btn_Btn_Go )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Go 为空，请检查prefab是否缺乏组件");
       }
        m_label_mapLabel = fastComponent.FastGetComponent<UILabel>("mapLabel");
       if( null == m_label_mapLabel )
       {
            Engine.Utility.Log.Error("m_label_mapLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_shop = fastComponent.FastGetComponent<UIButton>("btn_shop");
       if( null == m_btn_btn_shop )
       {
            Engine.Utility.Log.Error("m_btn_btn_shop 为空，请检查prefab是否缺乏组件");
       }
        m_label_LieHunLabel = fastComponent.FastGetComponent<UILabel>("LieHunLabel");
       if( null == m_label_LieHunLabel )
       {
            Engine.Utility.Log.Error("m_label_LieHunLabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_LieHunNum = fastComponent.FastGetComponent<UILabel>("LieHunNum");
       if( null == m_label_LieHunNum )
       {
            Engine.Utility.Log.Error("m_label_LieHunNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_infoBtn = fastComponent.FastGetComponent<UIButton>("infoBtn");
       if( null == m_btn_infoBtn )
       {
            Engine.Utility.Log.Error("m_btn_infoBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_infoContent = fastComponent.FastGetComponent<UIButton>("infoContent");
       if( null == m_btn_infoContent )
       {
            Engine.Utility.Log.Error("m_btn_infoContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_tipsContent = fastComponent.FastGetComponent<UILabel>("tipsContent");
       if( null == m_label_tipsContent )
       {
            Engine.Utility.Log.Error("m_label_tipsContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Dailytips_2 = fastComponent.FastGetComponent<UIButton>("Dailytips_2");
       if( null == m_btn_Dailytips_2 )
       {
            Engine.Utility.Log.Error("m_btn_Dailytips_2 为空，请检查prefab是否缺乏组件");
       }
        m__Model = fastComponent.FastGetComponent<UITexture>("Model");
       if( null == m__Model )
       {
            Engine.Utility.Log.Error("m__Model 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIDailyGrid = fastComponent.FastGetComponent<Transform>("UIDailyGrid");
       if( null == m_trans_UIDailyGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIDailyGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_Frequency_Num = fastComponent.FastGetComponent<UILabel>("Frequency_Num");
       if( null == m_label_Frequency_Num )
       {
            Engine.Utility.Log.Error("m_label_Frequency_Num 为空，请检查prefab是否缺乏组件");
       }
        m_label_Active_Num = fastComponent.FastGetComponent<UILabel>("Active_Num");
       if( null == m_label_Active_Num )
       {
            Engine.Utility.Log.Error("m_label_Active_Num 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITabGrid = fastComponent.FastGetComponent<Transform>("UITabGrid");
       if( null == m_trans_UITabGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITabGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIDailyCalendarGrid = fastComponent.FastGetComponent<Transform>("UIDailyCalendarGrid");
       if( null == m_trans_UIDailyCalendarGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIDailyCalendarGrid 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UIHuntingToggleGrid = fastComponent.FastGetComponent<UIToggle>("UIHuntingToggleGrid");
       if( null == m_toggle_UIHuntingToggleGrid )
       {
            Engine.Utility.Log.Error("m_toggle_UIHuntingToggleGrid 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UIHuntingListGrid = fastComponent.FastGetComponent<UIToggle>("UIHuntingListGrid");
       if( null == m_toggle_UIHuntingListGrid )
       {
            Engine.Utility.Log.Error("m_toggle_UIHuntingListGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIDailyRewardGrid = fastComponent.FastGetComponent<Transform>("UIDailyRewardGrid");
       if( null == m_trans_UIDailyRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIDailyRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UITreasureBossGrid = fastComponent.FastGetComponent<UIToggle>("UITreasureBossGrid");
       if( null == m_toggle_UITreasureBossGrid )
       {
            Engine.Utility.Log.Error("m_toggle_UITreasureBossGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_ColliderBg.gameObject).onClick = _onClick_ColliderBg_Btn;
        UIEventListener.Get(m_btn_JoinBtn.gameObject).onClick = _onClick_JoinBtn_Btn;
        UIEventListener.Get(m_btn_Dailytips_1.gameObject).onClick = _onClick_Dailytips_1_Btn;
        UIEventListener.Get(m_btn_TBBtn_Go.gameObject).onClick = _onClick_TBBtn_Go_Btn;
        UIEventListener.Get(m_btn_TBBtn_shop.gameObject).onClick = _onClick_TBBtn_shop_Btn;
        UIEventListener.Get(m_btn_Btn_Go.gameObject).onClick = _onClick_Btn_Go_Btn;
        UIEventListener.Get(m_btn_btn_shop.gameObject).onClick = _onClick_Btn_shop_Btn;
        UIEventListener.Get(m_btn_infoBtn.gameObject).onClick = _onClick_InfoBtn_Btn;
        UIEventListener.Get(m_btn_infoContent.gameObject).onClick = _onClick_InfoContent_Btn;
        UIEventListener.Get(m_btn_Dailytips_2.gameObject).onClick = _onClick_Dailytips_2_Btn;
    }

    void _onClick_ColliderBg_Btn(GameObject caster)
    {
        onClick_ColliderBg_Btn( caster );
    }

    void _onClick_JoinBtn_Btn(GameObject caster)
    {
        onClick_JoinBtn_Btn( caster );
    }

    void _onClick_Dailytips_1_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_TBBtn_Go_Btn(GameObject caster)
    {
        onClick_TBBtn_Go_Btn( caster );
    }

    void _onClick_TBBtn_shop_Btn(GameObject caster)
    {
        onClick_TBBtn_shop_Btn( caster );
    }

    void _onClick_Btn_Go_Btn(GameObject caster)
    {
        onClick_Btn_Go_Btn( caster );
    }

    void _onClick_Btn_shop_Btn(GameObject caster)
    {
        onClick_Btn_shop_Btn( caster );
    }

    void _onClick_InfoBtn_Btn(GameObject caster)
    {
        onClick_InfoBtn_Btn( caster );
    }

    void _onClick_InfoContent_Btn(GameObject caster)
    {
        onClick_InfoContent_Btn( caster );
    }

    void _onClick_Dailytips_2_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }


}
