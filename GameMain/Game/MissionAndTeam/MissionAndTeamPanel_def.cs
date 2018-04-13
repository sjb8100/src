//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MissionAndTeamPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_QuestTraceItem;

    UISpriteEx           m_spriteEx_mutebtn;

    UIButton             m_btn_statebg;

    UILabel              m_label_roomname;

    UISprite             m_sprite_statearrow;

    UISprite             m_sprite_stateani;

    UISpriteEx           m_spriteEx_voicesetting;

    Transform            m_trans_channelContainer;

    UIButton             m_btn_teamchannel;

    UIButton             m_btn_homechannel;

    UIButton             m_btn_commandchannel;

    UIWidget             m_widget_btnclose;

    UIButton             m_btn_btnArrow;

    Transform            m_trans_ArrowPosShow;

    Transform            m_trans_ArrowPosHide;

    UIWidget             m_widget_Offset;

    UISpriteEx           m_spriteEx_btnMission;

    UILabel              m_label_missionlabel;

    UISpriteEx           m_spriteEx_btnTeam;

    UILabel              m_label_TeamLbl;

    UILabel              m_label_TeamNum;

    UISprite             m_sprite_btnTeam_warrning;

    UIWidget             m_widget_team;

    Transform            m_trans_TeamChoiceBtn;

    UIButton             m_btn_btn_createteam;

    UIButton             m_btn_btn_convenientteam;

    Transform            m_trans_MemberListRoot;

    UIGrid               m_grid_MemberListGrid;

    Transform            m_trans_MemberGridCache;

    Transform            m_trans_TeamMemberBtnRoot;

    UISprite             m_sprite_teamMember_bg;

    UIButton             m_btn_btn_sendmessage;

    UIButton             m_btn_btn_lookmessage;

    UIButton             m_btn_btn_addfriend;

    UIButton             m_btn_btn_giveleader;

    UIButton             m_btn_btn_kickedoutteam;

    UIWidget             m_widget_TeamMemberBtnClose;

    Transform            m_trans_mission;

    Transform            m_trans_ScrollViewRoot;

    Transform            m_trans_grid;

    Transform            m_trans_QuestTraceItemRoot;

    UISprite             m_sprite_arrow;

    Transform            m_trans_copyTarget;

    Transform            m_trans_copyTargetScrollViewRoot;

    UIScrollView         m_scrollview_copyTargetScrollView;

    Transform            m_trans_copyTargetGrid;

    UILabel              m_label_copyTargetTitle;

    Transform            m_trans_copyTargetGridCache;

    Transform            m_trans_copyTargetEffectRoot;

    Transform            m_trans_copyBattleInfo;

    Transform            m_trans_copyBattleInfoRoot;

    UILabel              m_label_shenLabel0;

    UILabel              m_label_moLabel0;

    UILabel              m_label_shenLabel1;

    UILabel              m_label_moLabel1;

    UILabel              m_label_shenLabel2;

    UILabel              m_label_moLabel2;

    UILabel              m_label_copyBattleInfoTitle;

    Transform            m_trans_nvWa;

    Transform            m_trans_RecruitContent;

    UILabel              m_label_RecruitNum;

    Transform            m_trans_GuardContent;

    UIGrid               m_grid_GridRoot;

    Transform            m_trans_RecruitGrid1;

    Transform            m_trans_RecruitGrid2;

    Transform            m_trans_RecruitGrid3;

    Transform            m_trans_RecruitGrid4;

    UILabel              m_label_MebelNum;

    Transform            m_trans_answer;

    Transform            m_trans_answerRoot;

    UILabel              m_label_JingBiNum;

    UILabel              m_label_PlayerNum;

    UILabel              m_label_reliveLbl;

    UISprite             m_sprite_reliveIconTrue;

    UISprite             m_sprite_reliveIconFalse;

    Transform            m_trans_OffsetPosHide;

    Transform            m_trans_OffsetPosShow;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_QuestTraceItem = fastComponent.FastGetComponent<UISprite>("QuestTraceItem");
       if( null == m_sprite_QuestTraceItem )
       {
            Engine.Utility.Log.Error("m_sprite_QuestTraceItem 为空，请检查prefab是否缺乏组件");
       }
        m_spriteEx_mutebtn = fastComponent.FastGetComponent<UISpriteEx>("mutebtn");
       if( null == m_spriteEx_mutebtn )
       {
            Engine.Utility.Log.Error("m_spriteEx_mutebtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_statebg = fastComponent.FastGetComponent<UIButton>("statebg");
       if( null == m_btn_statebg )
       {
            Engine.Utility.Log.Error("m_btn_statebg 为空，请检查prefab是否缺乏组件");
       }
        m_label_roomname = fastComponent.FastGetComponent<UILabel>("roomname");
       if( null == m_label_roomname )
       {
            Engine.Utility.Log.Error("m_label_roomname 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_statearrow = fastComponent.FastGetComponent<UISprite>("statearrow");
       if( null == m_sprite_statearrow )
       {
            Engine.Utility.Log.Error("m_sprite_statearrow 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_stateani = fastComponent.FastGetComponent<UISprite>("stateani");
       if( null == m_sprite_stateani )
       {
            Engine.Utility.Log.Error("m_sprite_stateani 为空，请检查prefab是否缺乏组件");
       }
        m_spriteEx_voicesetting = fastComponent.FastGetComponent<UISpriteEx>("voicesetting");
       if( null == m_spriteEx_voicesetting )
       {
            Engine.Utility.Log.Error("m_spriteEx_voicesetting 为空，请检查prefab是否缺乏组件");
       }
        m_trans_channelContainer = fastComponent.FastGetComponent<Transform>("channelContainer");
       if( null == m_trans_channelContainer )
       {
            Engine.Utility.Log.Error("m_trans_channelContainer 为空，请检查prefab是否缺乏组件");
       }
        m_btn_teamchannel = fastComponent.FastGetComponent<UIButton>("teamchannel");
       if( null == m_btn_teamchannel )
       {
            Engine.Utility.Log.Error("m_btn_teamchannel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_homechannel = fastComponent.FastGetComponent<UIButton>("homechannel");
       if( null == m_btn_homechannel )
       {
            Engine.Utility.Log.Error("m_btn_homechannel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_commandchannel = fastComponent.FastGetComponent<UIButton>("commandchannel");
       if( null == m_btn_commandchannel )
       {
            Engine.Utility.Log.Error("m_btn_commandchannel 为空，请检查prefab是否缺乏组件");
       }
        m_widget_btnclose = fastComponent.FastGetComponent<UIWidget>("btnclose");
       if( null == m_widget_btnclose )
       {
            Engine.Utility.Log.Error("m_widget_btnclose 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnArrow = fastComponent.FastGetComponent<UIButton>("btnArrow");
       if( null == m_btn_btnArrow )
       {
            Engine.Utility.Log.Error("m_btn_btnArrow 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ArrowPosShow = fastComponent.FastGetComponent<Transform>("ArrowPosShow");
       if( null == m_trans_ArrowPosShow )
       {
            Engine.Utility.Log.Error("m_trans_ArrowPosShow 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ArrowPosHide = fastComponent.FastGetComponent<Transform>("ArrowPosHide");
       if( null == m_trans_ArrowPosHide )
       {
            Engine.Utility.Log.Error("m_trans_ArrowPosHide 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Offset = fastComponent.FastGetComponent<UIWidget>("Offset");
       if( null == m_widget_Offset )
       {
            Engine.Utility.Log.Error("m_widget_Offset 为空，请检查prefab是否缺乏组件");
       }
        m_spriteEx_btnMission = fastComponent.FastGetComponent<UISpriteEx>("btnMission");
       if( null == m_spriteEx_btnMission )
       {
            Engine.Utility.Log.Error("m_spriteEx_btnMission 为空，请检查prefab是否缺乏组件");
       }
        m_label_missionlabel = fastComponent.FastGetComponent<UILabel>("missionlabel");
       if( null == m_label_missionlabel )
       {
            Engine.Utility.Log.Error("m_label_missionlabel 为空，请检查prefab是否缺乏组件");
       }
        m_spriteEx_btnTeam = fastComponent.FastGetComponent<UISpriteEx>("btnTeam");
       if( null == m_spriteEx_btnTeam )
       {
            Engine.Utility.Log.Error("m_spriteEx_btnTeam 为空，请检查prefab是否缺乏组件");
       }
        m_label_TeamLbl = fastComponent.FastGetComponent<UILabel>("TeamLbl");
       if( null == m_label_TeamLbl )
       {
            Engine.Utility.Log.Error("m_label_TeamLbl 为空，请检查prefab是否缺乏组件");
       }
        m_label_TeamNum = fastComponent.FastGetComponent<UILabel>("TeamNum");
       if( null == m_label_TeamNum )
       {
            Engine.Utility.Log.Error("m_label_TeamNum 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btnTeam_warrning = fastComponent.FastGetComponent<UISprite>("btnTeam_warrning");
       if( null == m_sprite_btnTeam_warrning )
       {
            Engine.Utility.Log.Error("m_sprite_btnTeam_warrning 为空，请检查prefab是否缺乏组件");
       }
        m_widget_team = fastComponent.FastGetComponent<UIWidget>("team");
       if( null == m_widget_team )
       {
            Engine.Utility.Log.Error("m_widget_team 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TeamChoiceBtn = fastComponent.FastGetComponent<Transform>("TeamChoiceBtn");
       if( null == m_trans_TeamChoiceBtn )
       {
            Engine.Utility.Log.Error("m_trans_TeamChoiceBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_createteam = fastComponent.FastGetComponent<UIButton>("btn_createteam");
       if( null == m_btn_btn_createteam )
       {
            Engine.Utility.Log.Error("m_btn_btn_createteam 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_convenientteam = fastComponent.FastGetComponent<UIButton>("btn_convenientteam");
       if( null == m_btn_btn_convenientteam )
       {
            Engine.Utility.Log.Error("m_btn_btn_convenientteam 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberListRoot = fastComponent.FastGetComponent<Transform>("MemberListRoot");
       if( null == m_trans_MemberListRoot )
       {
            Engine.Utility.Log.Error("m_trans_MemberListRoot 为空，请检查prefab是否缺乏组件");
       }
        m_grid_MemberListGrid = fastComponent.FastGetComponent<UIGrid>("MemberListGrid");
       if( null == m_grid_MemberListGrid )
       {
            Engine.Utility.Log.Error("m_grid_MemberListGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberGridCache = fastComponent.FastGetComponent<Transform>("MemberGridCache");
       if( null == m_trans_MemberGridCache )
       {
            Engine.Utility.Log.Error("m_trans_MemberGridCache 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TeamMemberBtnRoot = fastComponent.FastGetComponent<Transform>("TeamMemberBtnRoot");
       if( null == m_trans_TeamMemberBtnRoot )
       {
            Engine.Utility.Log.Error("m_trans_TeamMemberBtnRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_teamMember_bg = fastComponent.FastGetComponent<UISprite>("teamMember_bg");
       if( null == m_sprite_teamMember_bg )
       {
            Engine.Utility.Log.Error("m_sprite_teamMember_bg 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_sendmessage = fastComponent.FastGetComponent<UIButton>("btn_sendmessage");
       if( null == m_btn_btn_sendmessage )
       {
            Engine.Utility.Log.Error("m_btn_btn_sendmessage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_lookmessage = fastComponent.FastGetComponent<UIButton>("btn_lookmessage");
       if( null == m_btn_btn_lookmessage )
       {
            Engine.Utility.Log.Error("m_btn_btn_lookmessage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_addfriend = fastComponent.FastGetComponent<UIButton>("btn_addfriend");
       if( null == m_btn_btn_addfriend )
       {
            Engine.Utility.Log.Error("m_btn_btn_addfriend 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_giveleader = fastComponent.FastGetComponent<UIButton>("btn_giveleader");
       if( null == m_btn_btn_giveleader )
       {
            Engine.Utility.Log.Error("m_btn_btn_giveleader 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_kickedoutteam = fastComponent.FastGetComponent<UIButton>("btn_kickedoutteam");
       if( null == m_btn_btn_kickedoutteam )
       {
            Engine.Utility.Log.Error("m_btn_btn_kickedoutteam 为空，请检查prefab是否缺乏组件");
       }
        m_widget_TeamMemberBtnClose = fastComponent.FastGetComponent<UIWidget>("TeamMemberBtnClose");
       if( null == m_widget_TeamMemberBtnClose )
       {
            Engine.Utility.Log.Error("m_widget_TeamMemberBtnClose 为空，请检查prefab是否缺乏组件");
       }
        m_trans_mission = fastComponent.FastGetComponent<Transform>("mission");
       if( null == m_trans_mission )
       {
            Engine.Utility.Log.Error("m_trans_mission 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ScrollViewRoot = fastComponent.FastGetComponent<Transform>("ScrollViewRoot");
       if( null == m_trans_ScrollViewRoot )
       {
            Engine.Utility.Log.Error("m_trans_ScrollViewRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_grid = fastComponent.FastGetComponent<Transform>("grid");
       if( null == m_trans_grid )
       {
            Engine.Utility.Log.Error("m_trans_grid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_QuestTraceItemRoot = fastComponent.FastGetComponent<Transform>("QuestTraceItemRoot");
       if( null == m_trans_QuestTraceItemRoot )
       {
            Engine.Utility.Log.Error("m_trans_QuestTraceItemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_arrow = fastComponent.FastGetComponent<UISprite>("arrow");
       if( null == m_sprite_arrow )
       {
            Engine.Utility.Log.Error("m_sprite_arrow 为空，请检查prefab是否缺乏组件");
       }
        m_trans_copyTarget = fastComponent.FastGetComponent<Transform>("copyTarget");
       if( null == m_trans_copyTarget )
       {
            Engine.Utility.Log.Error("m_trans_copyTarget 为空，请检查prefab是否缺乏组件");
       }
        m_trans_copyTargetScrollViewRoot = fastComponent.FastGetComponent<Transform>("copyTargetScrollViewRoot");
       if( null == m_trans_copyTargetScrollViewRoot )
       {
            Engine.Utility.Log.Error("m_trans_copyTargetScrollViewRoot 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_copyTargetScrollView = fastComponent.FastGetComponent<UIScrollView>("copyTargetScrollView");
       if( null == m_scrollview_copyTargetScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_copyTargetScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_copyTargetGrid = fastComponent.FastGetComponent<Transform>("copyTargetGrid");
       if( null == m_trans_copyTargetGrid )
       {
            Engine.Utility.Log.Error("m_trans_copyTargetGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_copyTargetTitle = fastComponent.FastGetComponent<UILabel>("copyTargetTitle");
       if( null == m_label_copyTargetTitle )
       {
            Engine.Utility.Log.Error("m_label_copyTargetTitle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_copyTargetGridCache = fastComponent.FastGetComponent<Transform>("copyTargetGridCache");
       if( null == m_trans_copyTargetGridCache )
       {
            Engine.Utility.Log.Error("m_trans_copyTargetGridCache 为空，请检查prefab是否缺乏组件");
       }
        m_trans_copyTargetEffectRoot = fastComponent.FastGetComponent<Transform>("copyTargetEffectRoot");
       if( null == m_trans_copyTargetEffectRoot )
       {
            Engine.Utility.Log.Error("m_trans_copyTargetEffectRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_copyBattleInfo = fastComponent.FastGetComponent<Transform>("copyBattleInfo");
       if( null == m_trans_copyBattleInfo )
       {
            Engine.Utility.Log.Error("m_trans_copyBattleInfo 为空，请检查prefab是否缺乏组件");
       }
        m_trans_copyBattleInfoRoot = fastComponent.FastGetComponent<Transform>("copyBattleInfoRoot");
       if( null == m_trans_copyBattleInfoRoot )
       {
            Engine.Utility.Log.Error("m_trans_copyBattleInfoRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_shenLabel0 = fastComponent.FastGetComponent<UILabel>("shenLabel0");
       if( null == m_label_shenLabel0 )
       {
            Engine.Utility.Log.Error("m_label_shenLabel0 为空，请检查prefab是否缺乏组件");
       }
        m_label_moLabel0 = fastComponent.FastGetComponent<UILabel>("moLabel0");
       if( null == m_label_moLabel0 )
       {
            Engine.Utility.Log.Error("m_label_moLabel0 为空，请检查prefab是否缺乏组件");
       }
        m_label_shenLabel1 = fastComponent.FastGetComponent<UILabel>("shenLabel1");
       if( null == m_label_shenLabel1 )
       {
            Engine.Utility.Log.Error("m_label_shenLabel1 为空，请检查prefab是否缺乏组件");
       }
        m_label_moLabel1 = fastComponent.FastGetComponent<UILabel>("moLabel1");
       if( null == m_label_moLabel1 )
       {
            Engine.Utility.Log.Error("m_label_moLabel1 为空，请检查prefab是否缺乏组件");
       }
        m_label_shenLabel2 = fastComponent.FastGetComponent<UILabel>("shenLabel2");
       if( null == m_label_shenLabel2 )
       {
            Engine.Utility.Log.Error("m_label_shenLabel2 为空，请检查prefab是否缺乏组件");
       }
        m_label_moLabel2 = fastComponent.FastGetComponent<UILabel>("moLabel2");
       if( null == m_label_moLabel2 )
       {
            Engine.Utility.Log.Error("m_label_moLabel2 为空，请检查prefab是否缺乏组件");
       }
        m_label_copyBattleInfoTitle = fastComponent.FastGetComponent<UILabel>("copyBattleInfoTitle");
       if( null == m_label_copyBattleInfoTitle )
       {
            Engine.Utility.Log.Error("m_label_copyBattleInfoTitle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_nvWa = fastComponent.FastGetComponent<Transform>("nvWa");
       if( null == m_trans_nvWa )
       {
            Engine.Utility.Log.Error("m_trans_nvWa 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RecruitContent = fastComponent.FastGetComponent<Transform>("RecruitContent");
       if( null == m_trans_RecruitContent )
       {
            Engine.Utility.Log.Error("m_trans_RecruitContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_RecruitNum = fastComponent.FastGetComponent<UILabel>("RecruitNum");
       if( null == m_label_RecruitNum )
       {
            Engine.Utility.Log.Error("m_label_RecruitNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GuardContent = fastComponent.FastGetComponent<Transform>("GuardContent");
       if( null == m_trans_GuardContent )
       {
            Engine.Utility.Log.Error("m_trans_GuardContent 为空，请检查prefab是否缺乏组件");
       }
        m_grid_GridRoot = fastComponent.FastGetComponent<UIGrid>("GridRoot");
       if( null == m_grid_GridRoot )
       {
            Engine.Utility.Log.Error("m_grid_GridRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RecruitGrid1 = fastComponent.FastGetComponent<Transform>("RecruitGrid1");
       if( null == m_trans_RecruitGrid1 )
       {
            Engine.Utility.Log.Error("m_trans_RecruitGrid1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RecruitGrid2 = fastComponent.FastGetComponent<Transform>("RecruitGrid2");
       if( null == m_trans_RecruitGrid2 )
       {
            Engine.Utility.Log.Error("m_trans_RecruitGrid2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RecruitGrid3 = fastComponent.FastGetComponent<Transform>("RecruitGrid3");
       if( null == m_trans_RecruitGrid3 )
       {
            Engine.Utility.Log.Error("m_trans_RecruitGrid3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RecruitGrid4 = fastComponent.FastGetComponent<Transform>("RecruitGrid4");
       if( null == m_trans_RecruitGrid4 )
       {
            Engine.Utility.Log.Error("m_trans_RecruitGrid4 为空，请检查prefab是否缺乏组件");
       }
        m_label_MebelNum = fastComponent.FastGetComponent<UILabel>("MebelNum");
       if( null == m_label_MebelNum )
       {
            Engine.Utility.Log.Error("m_label_MebelNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_answer = fastComponent.FastGetComponent<Transform>("answer");
       if( null == m_trans_answer )
       {
            Engine.Utility.Log.Error("m_trans_answer 为空，请检查prefab是否缺乏组件");
       }
        m_trans_answerRoot = fastComponent.FastGetComponent<Transform>("answerRoot");
       if( null == m_trans_answerRoot )
       {
            Engine.Utility.Log.Error("m_trans_answerRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_JingBiNum = fastComponent.FastGetComponent<UILabel>("JingBiNum");
       if( null == m_label_JingBiNum )
       {
            Engine.Utility.Log.Error("m_label_JingBiNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_PlayerNum = fastComponent.FastGetComponent<UILabel>("PlayerNum");
       if( null == m_label_PlayerNum )
       {
            Engine.Utility.Log.Error("m_label_PlayerNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_reliveLbl = fastComponent.FastGetComponent<UILabel>("reliveLbl");
       if( null == m_label_reliveLbl )
       {
            Engine.Utility.Log.Error("m_label_reliveLbl 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_reliveIconTrue = fastComponent.FastGetComponent<UISprite>("reliveIconTrue");
       if( null == m_sprite_reliveIconTrue )
       {
            Engine.Utility.Log.Error("m_sprite_reliveIconTrue 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_reliveIconFalse = fastComponent.FastGetComponent<UISprite>("reliveIconFalse");
       if( null == m_sprite_reliveIconFalse )
       {
            Engine.Utility.Log.Error("m_sprite_reliveIconFalse 为空，请检查prefab是否缺乏组件");
       }
        m_trans_OffsetPosHide = fastComponent.FastGetComponent<Transform>("OffsetPosHide");
       if( null == m_trans_OffsetPosHide )
       {
            Engine.Utility.Log.Error("m_trans_OffsetPosHide 为空，请检查prefab是否缺乏组件");
       }
        m_trans_OffsetPosShow = fastComponent.FastGetComponent<Transform>("OffsetPosShow");
       if( null == m_trans_OffsetPosShow )
       {
            Engine.Utility.Log.Error("m_trans_OffsetPosShow 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_statebg.gameObject).onClick = _onClick_Statebg_Btn;
        UIEventListener.Get(m_btn_teamchannel.gameObject).onClick = _onClick_Teamchannel_Btn;
        UIEventListener.Get(m_btn_homechannel.gameObject).onClick = _onClick_Homechannel_Btn;
        UIEventListener.Get(m_btn_commandchannel.gameObject).onClick = _onClick_Commandchannel_Btn;
        UIEventListener.Get(m_btn_btnArrow.gameObject).onClick = _onClick_BtnArrow_Btn;
        UIEventListener.Get(m_btn_btn_createteam.gameObject).onClick = _onClick_Btn_createteam_Btn;
        UIEventListener.Get(m_btn_btn_convenientteam.gameObject).onClick = _onClick_Btn_convenientteam_Btn;
        UIEventListener.Get(m_btn_btn_sendmessage.gameObject).onClick = _onClick_Btn_sendmessage_Btn;
        UIEventListener.Get(m_btn_btn_lookmessage.gameObject).onClick = _onClick_Btn_lookmessage_Btn;
        UIEventListener.Get(m_btn_btn_addfriend.gameObject).onClick = _onClick_Btn_addfriend_Btn;
        UIEventListener.Get(m_btn_btn_giveleader.gameObject).onClick = _onClick_Btn_giveleader_Btn;
        UIEventListener.Get(m_btn_btn_kickedoutteam.gameObject).onClick = _onClick_Btn_kickedoutteam_Btn;
    }

    void _onClick_Statebg_Btn(GameObject caster)
    {
        onClick_Statebg_Btn( caster );
    }

    void _onClick_Teamchannel_Btn(GameObject caster)
    {
        onClick_Teamchannel_Btn( caster );
    }

    void _onClick_Homechannel_Btn(GameObject caster)
    {
        onClick_Homechannel_Btn( caster );
    }

    void _onClick_Commandchannel_Btn(GameObject caster)
    {
        onClick_Commandchannel_Btn( caster );
    }

    void _onClick_BtnArrow_Btn(GameObject caster)
    {
        onClick_BtnArrow_Btn( caster );
    }

    void _onClick_Btn_createteam_Btn(GameObject caster)
    {
        onClick_Btn_createteam_Btn( caster );
    }

    void _onClick_Btn_convenientteam_Btn(GameObject caster)
    {
        onClick_Btn_convenientteam_Btn( caster );
    }

    void _onClick_Btn_sendmessage_Btn(GameObject caster)
    {
        onClick_Btn_sendmessage_Btn( caster );
    }

    void _onClick_Btn_lookmessage_Btn(GameObject caster)
    {
        onClick_Btn_lookmessage_Btn( caster );
    }

    void _onClick_Btn_addfriend_Btn(GameObject caster)
    {
        onClick_Btn_addfriend_Btn( caster );
    }

    void _onClick_Btn_giveleader_Btn(GameObject caster)
    {
        onClick_Btn_giveleader_Btn( caster );
    }

    void _onClick_Btn_kickedoutteam_Btn(GameObject caster)
    {
        onClick_Btn_kickedoutteam_Btn( caster );
    }


}
