//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SettingPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//基础
		JiChu = 1,
		//画质
		HuaZhi = 2,
		//挂机
		GuaJi = 3,
		//快捷
		KuaiJie = 4,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_left;

    UIWidget             m_widget_BaseContent;

    UITexture            m__headicon;

    UILabel              m_label_Role_Name;

    UILabel              m_label_Server_Name;

    UILabel              m_label_Role_Id;

    UIButton             m_btn_btn_goLogin;

    UIButton             m_btn_btn_gopayback;

    UIButton             m_btn_btn_Announcement;

    UIButton             m_btn_btn_goChooseRole;

    UIButton             m_btn_btn_goforum;

    UIButton             m_btn_btn_gohomepage;

    UIButton             m_btn_btn_gohotline;

    UIButton             m_btn_btn_LockScreen;

    UIButton             m_btn_btn_OutStuck;

    UIButton             m_btn_btn_RecordVideo;

    UIButton             m_btn_btn_Bible;

    UIToggle             m_toggle_Sound_CheckSound;

    UISlider             m_slider_Sound_soundslider;

    UISlider             m_slider_Sound_SoundEffectsslider;

    UIToggle             m_toggle_Sound_Checkeffect;

    UIToggle             m_toggle_Operate_SettingFixedRocker;

    UIToggle             m_toggle_Operate_SettingMoveRocker;

    UIToggle             m_toggle_AttackTarget_SettingPlayer;

    UIToggle             m_toggle_AttackTarget_SettingMonster;

    UIToggle             m_toggle_AttackTarget_SettingNone;

    UIToggle             m_toggle_Social_SettingTeamInvite;

    UIToggle             m_toggle_Social_SettingClanInvite;

    UIToggle             m_toggle_TeamSetting_IntoTeam;

    UIToggle             m_toggle_TeamSetting_Follow;

    UIToggle             m_toggle_ShowDetail_PlayerName;

    UIToggle             m_toggle_ShowDetail_PlayerTitle;

    UIToggle             m_toggle_ShowDetail_ClanName;

    UIToggle             m_toggle_ShowDetail_MonsterName;

    UIToggle             m_toggle_ShowDetail_HpDisplay;

    UIToggle             m_toggle_ShowDetail_ExpDisplay;

    UIToggle             m_toggle_ShowDetail_HurtDisplay;

    UIToggle             m_toggle_SpecialEffects_BlockFashion;

    UIWidget             m_widget_qualityContent;

    UIToggle             m_toggle_Camera_close;

    UIToggle             m_toggle_Camera_far;

    UISlider             m_slider_OneScreen_ScreenNumberslider;

    UIToggle             m_toggle_Render_shadow;

    UIToggle             m_toggle_SpecialEffects_skillShake;

    UIToggle             m_toggle_ShakeScreen_SettingShockScreen;

    UIToggle             m_toggle_SpecialEffects_mineEffect;

    UIToggle             m_toggle_SpecialEffects_otherEffect;

    UIToggle             m_toggle_SpecialEffects_BlockPlayers;

    UIToggle             m_toggle_SpecialEffects_BlockMonster;

    UIToggle             m_toggle_SpecialEffects_Grassland;

    UISprite             m_sprite_texiao;

    UISprite             m_sprite_texiaoLevel;

    Transform            m_trans_ViewDistanceGrade;

    Transform            m_trans_GroundDetailGrade;

    Transform            m_trans_ModelPrecision;

    Transform            m_trans_ParticleGrade;

    UISlider             m_slider_PictureEffect_GriphicSlider;

    Transform            m_trans_fightingContent;

    Transform            m_trans_area_restore;

    Transform            m_trans_RoleBaseHp;

    UISlider             m_slider_MedicalSetting_Hpvalue;

    UIButton             m_btn_RoleBaseHp_icon;

    Transform            m_trans_RoleHighHp;

    UISlider             m_slider_MedicalSetting_HpAtOncevalue;

    UIButton             m_btn_RoleHighHp_icon;

    UIButton             m_btn_HpAtOnceLabel;

    Transform            m_trans_RoleBaseMp;

    UISlider             m_slider_MedicalSetting_Mpvalue;

    UIButton             m_btn_RoleBaseMp_icon;

    Transform            m_trans_PetBaseHp;

    UISlider             m_slider_MedicalSetting_PetHpvalue;

    UIButton             m_btn_PetBaseHp_icon;

    Transform            m_trans_AutoRepair;

    UISlider             m_slider_MedicalSetting_Equipvalue;

    Transform            m_trans_AutoReturn;

    UISlider             m_slider_MedicalSetting_HpLimtvalue;

    UISlider             m_slider_Pick_EquipLevel;

    Transform            m_trans_UseItemContent;

    Transform            m_trans_ItemListScrollView;

    Transform            m_trans_ItemInfoRoot;

    UILabel              m_label_ItemName;

    UILabel              m_label_ItemLevel;

    UILabel              m_label_ItemDes;

    Transform            m_trans_CoteContent;

    UILabel              m_label_SetItemCountLabel;

    UISlider             m_slider_Shortcut_SetItemCountSlider;

    UIToggle             m_toggle_UI_ScreenPriority;

    UIToggle             m_toggle_UI_PerformancePriority;

    UIToggle             m_toggle_UI_SavingPriority;

    UIToggle             m_toggle_UI_rolepos;

    UIToggle             m_toggle_UI_headname;

    UIToggle             m_toggle_UI_joystick;

    UIToggle             m_toggle_UI_Weather;

    UIToggle             m_toggle_UI_WaterReflection;

    UIToggle             m_toggle_UI_grass;

    UIToggle             m_toggle_UI_sod;

    UIToggle             m_toggle_UI_ui;

    UIToggle             m_toggle_UI_ShockScreen;

    UIToggle             m_toggle_UI_SameMold;

    UIToggle             m_toggle_UI_shadow;

    UIToggle             m_toggle_UI_face;

    UIToggle             m_toggle_UI_Filter;

    UIToggle             m_toggle_UI_AntiAliasing;

    UIToggle             m_toggle_UI_mapArea;

    UIToggle             m_toggle_UI_Model;

    UIToggle             m_toggle_UI_Normal;

    UIToggle             m_toggle_UI_Grassland;

    UIToggle             m_toggle_UI_WorldCoordinates;

    UIToggle             m_toggle_UI_map9grid;

    UIToggle             m_toggle_UI_OnMainPlayer;

    UIToggle             m_toggle_UI_IgnoreLand;

    UIToggle             m_toggle_UI_IgnoreGrass;

    UIToggle             m_toggle_UI_IgnoreStatic;

    UIToggle             m_toggle_UI_IgnoreScene;

    UIToggle             m_toggle_UI_IgnoreWater;

    UIToggle             m_toggle_UI_PreLoad;

    UIToggle             m_toggle_UI_ClearPlayerPres;

    Transform            m_trans_feedbackContent;

    Transform            m_trans_note;

    UIInput              m_input_Account;

    UIInput              m_input_Phone;

    UIInput              m_input_Suggestion;

    UIToggle             m_toggle_wenjuan;

    UIToggle             m_toggle_quexian;

    UIToggle             m_toggle_jianyi;

    UIToggle             m_toggle_chongzhi;

    UIToggle             m_toggle_qita;

    UIInput              m_input_feedbackText;

    UIButton             m_btn_confirmBtn;

    Transform            m_trans_chat;

    UIScrollView         m_scrollview_ChatScrollView;

    Transform            m_trans_ChatItemRoot;

    UIButton             m_btn_noteBtn;

    UIButton             m_btn_chatBtn;

    UISprite             m_sprite_feedBackWarning;

    Transform            m_trans_right;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_left = fastComponent.FastGetComponent<Transform>("left");
       if( null == m_trans_left )
       {
            Engine.Utility.Log.Error("m_trans_left 为空，请检查prefab是否缺乏组件");
       }
        m_widget_BaseContent = fastComponent.FastGetComponent<UIWidget>("BaseContent");
       if( null == m_widget_BaseContent )
       {
            Engine.Utility.Log.Error("m_widget_BaseContent 为空，请检查prefab是否缺乏组件");
       }
        m__headicon = fastComponent.FastGetComponent<UITexture>("headicon");
       if( null == m__headicon )
       {
            Engine.Utility.Log.Error("m__headicon 为空，请检查prefab是否缺乏组件");
       }
        m_label_Role_Name = fastComponent.FastGetComponent<UILabel>("Role_Name");
       if( null == m_label_Role_Name )
       {
            Engine.Utility.Log.Error("m_label_Role_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_Server_Name = fastComponent.FastGetComponent<UILabel>("Server_Name");
       if( null == m_label_Server_Name )
       {
            Engine.Utility.Log.Error("m_label_Server_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_Role_Id = fastComponent.FastGetComponent<UILabel>("Role_Id");
       if( null == m_label_Role_Id )
       {
            Engine.Utility.Log.Error("m_label_Role_Id 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_goLogin = fastComponent.FastGetComponent<UIButton>("btn_goLogin");
       if( null == m_btn_btn_goLogin )
       {
            Engine.Utility.Log.Error("m_btn_btn_goLogin 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_gopayback = fastComponent.FastGetComponent<UIButton>("btn_gopayback");
       if( null == m_btn_btn_gopayback )
       {
            Engine.Utility.Log.Error("m_btn_btn_gopayback 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Announcement = fastComponent.FastGetComponent<UIButton>("btn_Announcement");
       if( null == m_btn_btn_Announcement )
       {
            Engine.Utility.Log.Error("m_btn_btn_Announcement 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_goChooseRole = fastComponent.FastGetComponent<UIButton>("btn_goChooseRole");
       if( null == m_btn_btn_goChooseRole )
       {
            Engine.Utility.Log.Error("m_btn_btn_goChooseRole 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_goforum = fastComponent.FastGetComponent<UIButton>("btn_goforum");
       if( null == m_btn_btn_goforum )
       {
            Engine.Utility.Log.Error("m_btn_btn_goforum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_gohomepage = fastComponent.FastGetComponent<UIButton>("btn_gohomepage");
       if( null == m_btn_btn_gohomepage )
       {
            Engine.Utility.Log.Error("m_btn_btn_gohomepage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_gohotline = fastComponent.FastGetComponent<UIButton>("btn_gohotline");
       if( null == m_btn_btn_gohotline )
       {
            Engine.Utility.Log.Error("m_btn_btn_gohotline 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_LockScreen = fastComponent.FastGetComponent<UIButton>("btn_LockScreen");
       if( null == m_btn_btn_LockScreen )
       {
            Engine.Utility.Log.Error("m_btn_btn_LockScreen 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_OutStuck = fastComponent.FastGetComponent<UIButton>("btn_OutStuck");
       if( null == m_btn_btn_OutStuck )
       {
            Engine.Utility.Log.Error("m_btn_btn_OutStuck 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_RecordVideo = fastComponent.FastGetComponent<UIButton>("btn_RecordVideo");
       if( null == m_btn_btn_RecordVideo )
       {
            Engine.Utility.Log.Error("m_btn_btn_RecordVideo 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Bible = fastComponent.FastGetComponent<UIButton>("btn_Bible");
       if( null == m_btn_btn_Bible )
       {
            Engine.Utility.Log.Error("m_btn_btn_Bible 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Sound_CheckSound = fastComponent.FastGetComponent<UIToggle>("Sound_CheckSound");
       if( null == m_toggle_Sound_CheckSound )
       {
            Engine.Utility.Log.Error("m_toggle_Sound_CheckSound 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Sound_soundslider = fastComponent.FastGetComponent<UISlider>("Sound_soundslider");
       if( null == m_slider_Sound_soundslider )
       {
            Engine.Utility.Log.Error("m_slider_Sound_soundslider 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Sound_SoundEffectsslider = fastComponent.FastGetComponent<UISlider>("Sound_SoundEffectsslider");
       if( null == m_slider_Sound_SoundEffectsslider )
       {
            Engine.Utility.Log.Error("m_slider_Sound_SoundEffectsslider 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Sound_Checkeffect = fastComponent.FastGetComponent<UIToggle>("Sound_Checkeffect");
       if( null == m_toggle_Sound_Checkeffect )
       {
            Engine.Utility.Log.Error("m_toggle_Sound_Checkeffect 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Operate_SettingFixedRocker = fastComponent.FastGetComponent<UIToggle>("Operate_SettingFixedRocker");
       if( null == m_toggle_Operate_SettingFixedRocker )
       {
            Engine.Utility.Log.Error("m_toggle_Operate_SettingFixedRocker 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Operate_SettingMoveRocker = fastComponent.FastGetComponent<UIToggle>("Operate_SettingMoveRocker");
       if( null == m_toggle_Operate_SettingMoveRocker )
       {
            Engine.Utility.Log.Error("m_toggle_Operate_SettingMoveRocker 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_AttackTarget_SettingPlayer = fastComponent.FastGetComponent<UIToggle>("AttackTarget_SettingPlayer");
       if( null == m_toggle_AttackTarget_SettingPlayer )
       {
            Engine.Utility.Log.Error("m_toggle_AttackTarget_SettingPlayer 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_AttackTarget_SettingMonster = fastComponent.FastGetComponent<UIToggle>("AttackTarget_SettingMonster");
       if( null == m_toggle_AttackTarget_SettingMonster )
       {
            Engine.Utility.Log.Error("m_toggle_AttackTarget_SettingMonster 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_AttackTarget_SettingNone = fastComponent.FastGetComponent<UIToggle>("AttackTarget_SettingNone");
       if( null == m_toggle_AttackTarget_SettingNone )
       {
            Engine.Utility.Log.Error("m_toggle_AttackTarget_SettingNone 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Social_SettingTeamInvite = fastComponent.FastGetComponent<UIToggle>("Social_SettingTeamInvite");
       if( null == m_toggle_Social_SettingTeamInvite )
       {
            Engine.Utility.Log.Error("m_toggle_Social_SettingTeamInvite 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Social_SettingClanInvite = fastComponent.FastGetComponent<UIToggle>("Social_SettingClanInvite");
       if( null == m_toggle_Social_SettingClanInvite )
       {
            Engine.Utility.Log.Error("m_toggle_Social_SettingClanInvite 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_TeamSetting_IntoTeam = fastComponent.FastGetComponent<UIToggle>("TeamSetting_IntoTeam");
       if( null == m_toggle_TeamSetting_IntoTeam )
       {
            Engine.Utility.Log.Error("m_toggle_TeamSetting_IntoTeam 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_TeamSetting_Follow = fastComponent.FastGetComponent<UIToggle>("TeamSetting_Follow");
       if( null == m_toggle_TeamSetting_Follow )
       {
            Engine.Utility.Log.Error("m_toggle_TeamSetting_Follow 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_ShowDetail_PlayerName = fastComponent.FastGetComponent<UIToggle>("ShowDetail_PlayerName");
       if( null == m_toggle_ShowDetail_PlayerName )
       {
            Engine.Utility.Log.Error("m_toggle_ShowDetail_PlayerName 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_ShowDetail_PlayerTitle = fastComponent.FastGetComponent<UIToggle>("ShowDetail_PlayerTitle");
       if( null == m_toggle_ShowDetail_PlayerTitle )
       {
            Engine.Utility.Log.Error("m_toggle_ShowDetail_PlayerTitle 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_ShowDetail_ClanName = fastComponent.FastGetComponent<UIToggle>("ShowDetail_ClanName");
       if( null == m_toggle_ShowDetail_ClanName )
       {
            Engine.Utility.Log.Error("m_toggle_ShowDetail_ClanName 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_ShowDetail_MonsterName = fastComponent.FastGetComponent<UIToggle>("ShowDetail_MonsterName");
       if( null == m_toggle_ShowDetail_MonsterName )
       {
            Engine.Utility.Log.Error("m_toggle_ShowDetail_MonsterName 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_ShowDetail_HpDisplay = fastComponent.FastGetComponent<UIToggle>("ShowDetail_HpDisplay");
       if( null == m_toggle_ShowDetail_HpDisplay )
       {
            Engine.Utility.Log.Error("m_toggle_ShowDetail_HpDisplay 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_ShowDetail_ExpDisplay = fastComponent.FastGetComponent<UIToggle>("ShowDetail_ExpDisplay");
       if( null == m_toggle_ShowDetail_ExpDisplay )
       {
            Engine.Utility.Log.Error("m_toggle_ShowDetail_ExpDisplay 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_ShowDetail_HurtDisplay = fastComponent.FastGetComponent<UIToggle>("ShowDetail_HurtDisplay");
       if( null == m_toggle_ShowDetail_HurtDisplay )
       {
            Engine.Utility.Log.Error("m_toggle_ShowDetail_HurtDisplay 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_SpecialEffects_BlockFashion = fastComponent.FastGetComponent<UIToggle>("SpecialEffects_BlockFashion");
       if( null == m_toggle_SpecialEffects_BlockFashion )
       {
            Engine.Utility.Log.Error("m_toggle_SpecialEffects_BlockFashion 为空，请检查prefab是否缺乏组件");
       }
        m_widget_qualityContent = fastComponent.FastGetComponent<UIWidget>("qualityContent");
       if( null == m_widget_qualityContent )
       {
            Engine.Utility.Log.Error("m_widget_qualityContent 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Camera_close = fastComponent.FastGetComponent<UIToggle>("Camera_close");
       if( null == m_toggle_Camera_close )
       {
            Engine.Utility.Log.Error("m_toggle_Camera_close 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Camera_far = fastComponent.FastGetComponent<UIToggle>("Camera_far");
       if( null == m_toggle_Camera_far )
       {
            Engine.Utility.Log.Error("m_toggle_Camera_far 为空，请检查prefab是否缺乏组件");
       }
        m_slider_OneScreen_ScreenNumberslider = fastComponent.FastGetComponent<UISlider>("OneScreen_ScreenNumberslider");
       if( null == m_slider_OneScreen_ScreenNumberslider )
       {
            Engine.Utility.Log.Error("m_slider_OneScreen_ScreenNumberslider 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Render_shadow = fastComponent.FastGetComponent<UIToggle>("Render_shadow");
       if( null == m_toggle_Render_shadow )
       {
            Engine.Utility.Log.Error("m_toggle_Render_shadow 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_SpecialEffects_skillShake = fastComponent.FastGetComponent<UIToggle>("SpecialEffects_skillShake");
       if( null == m_toggle_SpecialEffects_skillShake )
       {
            Engine.Utility.Log.Error("m_toggle_SpecialEffects_skillShake 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_ShakeScreen_SettingShockScreen = fastComponent.FastGetComponent<UIToggle>("ShakeScreen_SettingShockScreen");
       if( null == m_toggle_ShakeScreen_SettingShockScreen )
       {
            Engine.Utility.Log.Error("m_toggle_ShakeScreen_SettingShockScreen 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_SpecialEffects_mineEffect = fastComponent.FastGetComponent<UIToggle>("SpecialEffects_mineEffect");
       if( null == m_toggle_SpecialEffects_mineEffect )
       {
            Engine.Utility.Log.Error("m_toggle_SpecialEffects_mineEffect 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_SpecialEffects_otherEffect = fastComponent.FastGetComponent<UIToggle>("SpecialEffects_otherEffect");
       if( null == m_toggle_SpecialEffects_otherEffect )
       {
            Engine.Utility.Log.Error("m_toggle_SpecialEffects_otherEffect 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_SpecialEffects_BlockPlayers = fastComponent.FastGetComponent<UIToggle>("SpecialEffects_BlockPlayers");
       if( null == m_toggle_SpecialEffects_BlockPlayers )
       {
            Engine.Utility.Log.Error("m_toggle_SpecialEffects_BlockPlayers 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_SpecialEffects_BlockMonster = fastComponent.FastGetComponent<UIToggle>("SpecialEffects_BlockMonster");
       if( null == m_toggle_SpecialEffects_BlockMonster )
       {
            Engine.Utility.Log.Error("m_toggle_SpecialEffects_BlockMonster 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_SpecialEffects_Grassland = fastComponent.FastGetComponent<UIToggle>("SpecialEffects_Grassland");
       if( null == m_toggle_SpecialEffects_Grassland )
       {
            Engine.Utility.Log.Error("m_toggle_SpecialEffects_Grassland 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_texiao = fastComponent.FastGetComponent<UISprite>("texiao");
       if( null == m_sprite_texiao )
       {
            Engine.Utility.Log.Error("m_sprite_texiao 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_texiaoLevel = fastComponent.FastGetComponent<UISprite>("texiaoLevel");
       if( null == m_sprite_texiaoLevel )
       {
            Engine.Utility.Log.Error("m_sprite_texiaoLevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ViewDistanceGrade = fastComponent.FastGetComponent<Transform>("ViewDistanceGrade");
       if( null == m_trans_ViewDistanceGrade )
       {
            Engine.Utility.Log.Error("m_trans_ViewDistanceGrade 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GroundDetailGrade = fastComponent.FastGetComponent<Transform>("GroundDetailGrade");
       if( null == m_trans_GroundDetailGrade )
       {
            Engine.Utility.Log.Error("m_trans_GroundDetailGrade 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ModelPrecision = fastComponent.FastGetComponent<Transform>("ModelPrecision");
       if( null == m_trans_ModelPrecision )
       {
            Engine.Utility.Log.Error("m_trans_ModelPrecision 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ParticleGrade = fastComponent.FastGetComponent<Transform>("ParticleGrade");
       if( null == m_trans_ParticleGrade )
       {
            Engine.Utility.Log.Error("m_trans_ParticleGrade 为空，请检查prefab是否缺乏组件");
       }
        m_slider_PictureEffect_GriphicSlider = fastComponent.FastGetComponent<UISlider>("PictureEffect_GriphicSlider");
       if( null == m_slider_PictureEffect_GriphicSlider )
       {
            Engine.Utility.Log.Error("m_slider_PictureEffect_GriphicSlider 为空，请检查prefab是否缺乏组件");
       }
        m_trans_fightingContent = fastComponent.FastGetComponent<Transform>("fightingContent");
       if( null == m_trans_fightingContent )
       {
            Engine.Utility.Log.Error("m_trans_fightingContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_area_restore = fastComponent.FastGetComponent<Transform>("area_restore");
       if( null == m_trans_area_restore )
       {
            Engine.Utility.Log.Error("m_trans_area_restore 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RoleBaseHp = fastComponent.FastGetComponent<Transform>("RoleBaseHp");
       if( null == m_trans_RoleBaseHp )
       {
            Engine.Utility.Log.Error("m_trans_RoleBaseHp 为空，请检查prefab是否缺乏组件");
       }
        m_slider_MedicalSetting_Hpvalue = fastComponent.FastGetComponent<UISlider>("MedicalSetting_Hpvalue");
       if( null == m_slider_MedicalSetting_Hpvalue )
       {
            Engine.Utility.Log.Error("m_slider_MedicalSetting_Hpvalue 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RoleBaseHp_icon = fastComponent.FastGetComponent<UIButton>("RoleBaseHp_icon");
       if( null == m_btn_RoleBaseHp_icon )
       {
            Engine.Utility.Log.Error("m_btn_RoleBaseHp_icon 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RoleHighHp = fastComponent.FastGetComponent<Transform>("RoleHighHp");
       if( null == m_trans_RoleHighHp )
       {
            Engine.Utility.Log.Error("m_trans_RoleHighHp 为空，请检查prefab是否缺乏组件");
       }
        m_slider_MedicalSetting_HpAtOncevalue = fastComponent.FastGetComponent<UISlider>("MedicalSetting_HpAtOncevalue");
       if( null == m_slider_MedicalSetting_HpAtOncevalue )
       {
            Engine.Utility.Log.Error("m_slider_MedicalSetting_HpAtOncevalue 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RoleHighHp_icon = fastComponent.FastGetComponent<UIButton>("RoleHighHp_icon");
       if( null == m_btn_RoleHighHp_icon )
       {
            Engine.Utility.Log.Error("m_btn_RoleHighHp_icon 为空，请检查prefab是否缺乏组件");
       }
        m_btn_HpAtOnceLabel = fastComponent.FastGetComponent<UIButton>("HpAtOnceLabel");
       if( null == m_btn_HpAtOnceLabel )
       {
            Engine.Utility.Log.Error("m_btn_HpAtOnceLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RoleBaseMp = fastComponent.FastGetComponent<Transform>("RoleBaseMp");
       if( null == m_trans_RoleBaseMp )
       {
            Engine.Utility.Log.Error("m_trans_RoleBaseMp 为空，请检查prefab是否缺乏组件");
       }
        m_slider_MedicalSetting_Mpvalue = fastComponent.FastGetComponent<UISlider>("MedicalSetting_Mpvalue");
       if( null == m_slider_MedicalSetting_Mpvalue )
       {
            Engine.Utility.Log.Error("m_slider_MedicalSetting_Mpvalue 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RoleBaseMp_icon = fastComponent.FastGetComponent<UIButton>("RoleBaseMp_icon");
       if( null == m_btn_RoleBaseMp_icon )
       {
            Engine.Utility.Log.Error("m_btn_RoleBaseMp_icon 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PetBaseHp = fastComponent.FastGetComponent<Transform>("PetBaseHp");
       if( null == m_trans_PetBaseHp )
       {
            Engine.Utility.Log.Error("m_trans_PetBaseHp 为空，请检查prefab是否缺乏组件");
       }
        m_slider_MedicalSetting_PetHpvalue = fastComponent.FastGetComponent<UISlider>("MedicalSetting_PetHpvalue");
       if( null == m_slider_MedicalSetting_PetHpvalue )
       {
            Engine.Utility.Log.Error("m_slider_MedicalSetting_PetHpvalue 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PetBaseHp_icon = fastComponent.FastGetComponent<UIButton>("PetBaseHp_icon");
       if( null == m_btn_PetBaseHp_icon )
       {
            Engine.Utility.Log.Error("m_btn_PetBaseHp_icon 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AutoRepair = fastComponent.FastGetComponent<Transform>("AutoRepair");
       if( null == m_trans_AutoRepair )
       {
            Engine.Utility.Log.Error("m_trans_AutoRepair 为空，请检查prefab是否缺乏组件");
       }
        m_slider_MedicalSetting_Equipvalue = fastComponent.FastGetComponent<UISlider>("MedicalSetting_Equipvalue");
       if( null == m_slider_MedicalSetting_Equipvalue )
       {
            Engine.Utility.Log.Error("m_slider_MedicalSetting_Equipvalue 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AutoReturn = fastComponent.FastGetComponent<Transform>("AutoReturn");
       if( null == m_trans_AutoReturn )
       {
            Engine.Utility.Log.Error("m_trans_AutoReturn 为空，请检查prefab是否缺乏组件");
       }
        m_slider_MedicalSetting_HpLimtvalue = fastComponent.FastGetComponent<UISlider>("MedicalSetting_HpLimtvalue");
       if( null == m_slider_MedicalSetting_HpLimtvalue )
       {
            Engine.Utility.Log.Error("m_slider_MedicalSetting_HpLimtvalue 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Pick_EquipLevel = fastComponent.FastGetComponent<UISlider>("Pick_EquipLevel");
       if( null == m_slider_Pick_EquipLevel )
       {
            Engine.Utility.Log.Error("m_slider_Pick_EquipLevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UseItemContent = fastComponent.FastGetComponent<Transform>("UseItemContent");
       if( null == m_trans_UseItemContent )
       {
            Engine.Utility.Log.Error("m_trans_UseItemContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemListScrollView = fastComponent.FastGetComponent<Transform>("ItemListScrollView");
       if( null == m_trans_ItemListScrollView )
       {
            Engine.Utility.Log.Error("m_trans_ItemListScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemInfoRoot = fastComponent.FastGetComponent<Transform>("ItemInfoRoot");
       if( null == m_trans_ItemInfoRoot )
       {
            Engine.Utility.Log.Error("m_trans_ItemInfoRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemName = fastComponent.FastGetComponent<UILabel>("ItemName");
       if( null == m_label_ItemName )
       {
            Engine.Utility.Log.Error("m_label_ItemName 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemLevel = fastComponent.FastGetComponent<UILabel>("ItemLevel");
       if( null == m_label_ItemLevel )
       {
            Engine.Utility.Log.Error("m_label_ItemLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemDes = fastComponent.FastGetComponent<UILabel>("ItemDes");
       if( null == m_label_ItemDes )
       {
            Engine.Utility.Log.Error("m_label_ItemDes 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CoteContent = fastComponent.FastGetComponent<Transform>("CoteContent");
       if( null == m_trans_CoteContent )
       {
            Engine.Utility.Log.Error("m_trans_CoteContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_SetItemCountLabel = fastComponent.FastGetComponent<UILabel>("SetItemCountLabel");
       if( null == m_label_SetItemCountLabel )
       {
            Engine.Utility.Log.Error("m_label_SetItemCountLabel 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Shortcut_SetItemCountSlider = fastComponent.FastGetComponent<UISlider>("Shortcut_SetItemCountSlider");
       if( null == m_slider_Shortcut_SetItemCountSlider )
       {
            Engine.Utility.Log.Error("m_slider_Shortcut_SetItemCountSlider 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_ScreenPriority = fastComponent.FastGetComponent<UIToggle>("UI_ScreenPriority");
       if( null == m_toggle_UI_ScreenPriority )
       {
            Engine.Utility.Log.Error("m_toggle_UI_ScreenPriority 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_PerformancePriority = fastComponent.FastGetComponent<UIToggle>("UI_PerformancePriority");
       if( null == m_toggle_UI_PerformancePriority )
       {
            Engine.Utility.Log.Error("m_toggle_UI_PerformancePriority 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_SavingPriority = fastComponent.FastGetComponent<UIToggle>("UI_SavingPriority");
       if( null == m_toggle_UI_SavingPriority )
       {
            Engine.Utility.Log.Error("m_toggle_UI_SavingPriority 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_rolepos = fastComponent.FastGetComponent<UIToggle>("UI_rolepos");
       if( null == m_toggle_UI_rolepos )
       {
            Engine.Utility.Log.Error("m_toggle_UI_rolepos 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_headname = fastComponent.FastGetComponent<UIToggle>("UI_headname");
       if( null == m_toggle_UI_headname )
       {
            Engine.Utility.Log.Error("m_toggle_UI_headname 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_joystick = fastComponent.FastGetComponent<UIToggle>("UI_joystick");
       if( null == m_toggle_UI_joystick )
       {
            Engine.Utility.Log.Error("m_toggle_UI_joystick 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_Weather = fastComponent.FastGetComponent<UIToggle>("UI_Weather");
       if( null == m_toggle_UI_Weather )
       {
            Engine.Utility.Log.Error("m_toggle_UI_Weather 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_WaterReflection = fastComponent.FastGetComponent<UIToggle>("UI_WaterReflection");
       if( null == m_toggle_UI_WaterReflection )
       {
            Engine.Utility.Log.Error("m_toggle_UI_WaterReflection 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_grass = fastComponent.FastGetComponent<UIToggle>("UI_grass");
       if( null == m_toggle_UI_grass )
       {
            Engine.Utility.Log.Error("m_toggle_UI_grass 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_sod = fastComponent.FastGetComponent<UIToggle>("UI_sod");
       if( null == m_toggle_UI_sod )
       {
            Engine.Utility.Log.Error("m_toggle_UI_sod 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_ui = fastComponent.FastGetComponent<UIToggle>("UI_ui");
       if( null == m_toggle_UI_ui )
       {
            Engine.Utility.Log.Error("m_toggle_UI_ui 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_ShockScreen = fastComponent.FastGetComponent<UIToggle>("UI_ShockScreen");
       if( null == m_toggle_UI_ShockScreen )
       {
            Engine.Utility.Log.Error("m_toggle_UI_ShockScreen 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_SameMold = fastComponent.FastGetComponent<UIToggle>("UI_SameMold");
       if( null == m_toggle_UI_SameMold )
       {
            Engine.Utility.Log.Error("m_toggle_UI_SameMold 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_shadow = fastComponent.FastGetComponent<UIToggle>("UI_shadow");
       if( null == m_toggle_UI_shadow )
       {
            Engine.Utility.Log.Error("m_toggle_UI_shadow 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_face = fastComponent.FastGetComponent<UIToggle>("UI_face");
       if( null == m_toggle_UI_face )
       {
            Engine.Utility.Log.Error("m_toggle_UI_face 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_Filter = fastComponent.FastGetComponent<UIToggle>("UI_Filter");
       if( null == m_toggle_UI_Filter )
       {
            Engine.Utility.Log.Error("m_toggle_UI_Filter 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_AntiAliasing = fastComponent.FastGetComponent<UIToggle>("UI_AntiAliasing");
       if( null == m_toggle_UI_AntiAliasing )
       {
            Engine.Utility.Log.Error("m_toggle_UI_AntiAliasing 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_mapArea = fastComponent.FastGetComponent<UIToggle>("UI_mapArea");
       if( null == m_toggle_UI_mapArea )
       {
            Engine.Utility.Log.Error("m_toggle_UI_mapArea 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_Model = fastComponent.FastGetComponent<UIToggle>("UI_Model");
       if( null == m_toggle_UI_Model )
       {
            Engine.Utility.Log.Error("m_toggle_UI_Model 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_Normal = fastComponent.FastGetComponent<UIToggle>("UI_Normal");
       if( null == m_toggle_UI_Normal )
       {
            Engine.Utility.Log.Error("m_toggle_UI_Normal 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_Grassland = fastComponent.FastGetComponent<UIToggle>("UI_Grassland");
       if( null == m_toggle_UI_Grassland )
       {
            Engine.Utility.Log.Error("m_toggle_UI_Grassland 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_WorldCoordinates = fastComponent.FastGetComponent<UIToggle>("UI_WorldCoordinates");
       if( null == m_toggle_UI_WorldCoordinates )
       {
            Engine.Utility.Log.Error("m_toggle_UI_WorldCoordinates 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_map9grid = fastComponent.FastGetComponent<UIToggle>("UI_map9grid");
       if( null == m_toggle_UI_map9grid )
       {
            Engine.Utility.Log.Error("m_toggle_UI_map9grid 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_OnMainPlayer = fastComponent.FastGetComponent<UIToggle>("UI_OnMainPlayer");
       if( null == m_toggle_UI_OnMainPlayer )
       {
            Engine.Utility.Log.Error("m_toggle_UI_OnMainPlayer 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_IgnoreLand = fastComponent.FastGetComponent<UIToggle>("UI_IgnoreLand");
       if( null == m_toggle_UI_IgnoreLand )
       {
            Engine.Utility.Log.Error("m_toggle_UI_IgnoreLand 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_IgnoreGrass = fastComponent.FastGetComponent<UIToggle>("UI_IgnoreGrass");
       if( null == m_toggle_UI_IgnoreGrass )
       {
            Engine.Utility.Log.Error("m_toggle_UI_IgnoreGrass 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_IgnoreStatic = fastComponent.FastGetComponent<UIToggle>("UI_IgnoreStatic");
       if( null == m_toggle_UI_IgnoreStatic )
       {
            Engine.Utility.Log.Error("m_toggle_UI_IgnoreStatic 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_IgnoreScene = fastComponent.FastGetComponent<UIToggle>("UI_IgnoreScene");
       if( null == m_toggle_UI_IgnoreScene )
       {
            Engine.Utility.Log.Error("m_toggle_UI_IgnoreScene 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_IgnoreWater = fastComponent.FastGetComponent<UIToggle>("UI_IgnoreWater");
       if( null == m_toggle_UI_IgnoreWater )
       {
            Engine.Utility.Log.Error("m_toggle_UI_IgnoreWater 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_PreLoad = fastComponent.FastGetComponent<UIToggle>("UI_PreLoad");
       if( null == m_toggle_UI_PreLoad )
       {
            Engine.Utility.Log.Error("m_toggle_UI_PreLoad 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_UI_ClearPlayerPres = fastComponent.FastGetComponent<UIToggle>("UI_ClearPlayerPres");
       if( null == m_toggle_UI_ClearPlayerPres )
       {
            Engine.Utility.Log.Error("m_toggle_UI_ClearPlayerPres 为空，请检查prefab是否缺乏组件");
       }
        m_trans_feedbackContent = fastComponent.FastGetComponent<Transform>("feedbackContent");
       if( null == m_trans_feedbackContent )
       {
            Engine.Utility.Log.Error("m_trans_feedbackContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_note = fastComponent.FastGetComponent<Transform>("note");
       if( null == m_trans_note )
       {
            Engine.Utility.Log.Error("m_trans_note 为空，请检查prefab是否缺乏组件");
       }
        m_input_Account = fastComponent.FastGetComponent<UIInput>("Account");
       if( null == m_input_Account )
       {
            Engine.Utility.Log.Error("m_input_Account 为空，请检查prefab是否缺乏组件");
       }
        m_input_Phone = fastComponent.FastGetComponent<UIInput>("Phone");
       if( null == m_input_Phone )
       {
            Engine.Utility.Log.Error("m_input_Phone 为空，请检查prefab是否缺乏组件");
       }
        m_input_Suggestion = fastComponent.FastGetComponent<UIInput>("Suggestion");
       if( null == m_input_Suggestion )
       {
            Engine.Utility.Log.Error("m_input_Suggestion 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_wenjuan = fastComponent.FastGetComponent<UIToggle>("wenjuan");
       if( null == m_toggle_wenjuan )
       {
            Engine.Utility.Log.Error("m_toggle_wenjuan 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_quexian = fastComponent.FastGetComponent<UIToggle>("quexian");
       if( null == m_toggle_quexian )
       {
            Engine.Utility.Log.Error("m_toggle_quexian 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_jianyi = fastComponent.FastGetComponent<UIToggle>("jianyi");
       if( null == m_toggle_jianyi )
       {
            Engine.Utility.Log.Error("m_toggle_jianyi 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_chongzhi = fastComponent.FastGetComponent<UIToggle>("chongzhi");
       if( null == m_toggle_chongzhi )
       {
            Engine.Utility.Log.Error("m_toggle_chongzhi 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_qita = fastComponent.FastGetComponent<UIToggle>("qita");
       if( null == m_toggle_qita )
       {
            Engine.Utility.Log.Error("m_toggle_qita 为空，请检查prefab是否缺乏组件");
       }
        m_input_feedbackText = fastComponent.FastGetComponent<UIInput>("feedbackText");
       if( null == m_input_feedbackText )
       {
            Engine.Utility.Log.Error("m_input_feedbackText 为空，请检查prefab是否缺乏组件");
       }
        m_btn_confirmBtn = fastComponent.FastGetComponent<UIButton>("confirmBtn");
       if( null == m_btn_confirmBtn )
       {
            Engine.Utility.Log.Error("m_btn_confirmBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_chat = fastComponent.FastGetComponent<Transform>("chat");
       if( null == m_trans_chat )
       {
            Engine.Utility.Log.Error("m_trans_chat 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_ChatScrollView = fastComponent.FastGetComponent<UIScrollView>("ChatScrollView");
       if( null == m_scrollview_ChatScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_ChatScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ChatItemRoot = fastComponent.FastGetComponent<Transform>("ChatItemRoot");
       if( null == m_trans_ChatItemRoot )
       {
            Engine.Utility.Log.Error("m_trans_ChatItemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_noteBtn = fastComponent.FastGetComponent<UIButton>("noteBtn");
       if( null == m_btn_noteBtn )
       {
            Engine.Utility.Log.Error("m_btn_noteBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_chatBtn = fastComponent.FastGetComponent<UIButton>("chatBtn");
       if( null == m_btn_chatBtn )
       {
            Engine.Utility.Log.Error("m_btn_chatBtn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_feedBackWarning = fastComponent.FastGetComponent<UISprite>("feedBackWarning");
       if( null == m_sprite_feedBackWarning )
       {
            Engine.Utility.Log.Error("m_sprite_feedBackWarning 为空，请检查prefab是否缺乏组件");
       }
        m_trans_right = fastComponent.FastGetComponent<Transform>("right");
       if( null == m_trans_right )
       {
            Engine.Utility.Log.Error("m_trans_right 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_goLogin.gameObject).onClick = _onClick_Btn_goLogin_Btn;
        UIEventListener.Get(m_btn_btn_gopayback.gameObject).onClick = _onClick_Btn_gopayback_Btn;
        UIEventListener.Get(m_btn_btn_Announcement.gameObject).onClick = _onClick_Btn_Announcement_Btn;
        UIEventListener.Get(m_btn_btn_goChooseRole.gameObject).onClick = _onClick_Btn_goChooseRole_Btn;
        UIEventListener.Get(m_btn_btn_goforum.gameObject).onClick = _onClick_Btn_goforum_Btn;
        UIEventListener.Get(m_btn_btn_gohomepage.gameObject).onClick = _onClick_Btn_gohomepage_Btn;
        UIEventListener.Get(m_btn_btn_gohotline.gameObject).onClick = _onClick_Btn_gohotline_Btn;
        UIEventListener.Get(m_btn_btn_LockScreen.gameObject).onClick = _onClick_Btn_LockScreen_Btn;
        UIEventListener.Get(m_btn_btn_OutStuck.gameObject).onClick = _onClick_Btn_OutStuck_Btn;
        UIEventListener.Get(m_btn_btn_RecordVideo.gameObject).onClick = _onClick_Btn_RecordVideo_Btn;
        UIEventListener.Get(m_btn_btn_Bible.gameObject).onClick = _onClick_Btn_Bible_Btn;
        UIEventListener.Get(m_btn_RoleBaseHp_icon.gameObject).onClick = _onClick_RoleBaseHp_icon_Btn;
        UIEventListener.Get(m_btn_RoleHighHp_icon.gameObject).onClick = _onClick_RoleHighHp_icon_Btn;
        UIEventListener.Get(m_btn_HpAtOnceLabel.gameObject).onClick = _onClick_HpAtOnceLabel_Btn;
        UIEventListener.Get(m_btn_RoleBaseMp_icon.gameObject).onClick = _onClick_RoleBaseMp_icon_Btn;
        UIEventListener.Get(m_btn_PetBaseHp_icon.gameObject).onClick = _onClick_PetBaseHp_icon_Btn;
        UIEventListener.Get(m_btn_confirmBtn.gameObject).onClick = _onClick_ConfirmBtn_Btn;
        UIEventListener.Get(m_btn_noteBtn.gameObject).onClick = _onClick_NoteBtn_Btn;
        UIEventListener.Get(m_btn_chatBtn.gameObject).onClick = _onClick_ChatBtn_Btn;
    }

    void _onClick_Btn_goLogin_Btn(GameObject caster)
    {
        onClick_Btn_goLogin_Btn( caster );
    }

    void _onClick_Btn_gopayback_Btn(GameObject caster)
    {
        onClick_Btn_gopayback_Btn( caster );
    }

    void _onClick_Btn_Announcement_Btn(GameObject caster)
    {
        onClick_Btn_Announcement_Btn( caster );
    }

    void _onClick_Btn_goChooseRole_Btn(GameObject caster)
    {
        onClick_Btn_goChooseRole_Btn( caster );
    }

    void _onClick_Btn_goforum_Btn(GameObject caster)
    {
        onClick_Btn_goforum_Btn( caster );
    }

    void _onClick_Btn_gohomepage_Btn(GameObject caster)
    {
        onClick_Btn_gohomepage_Btn( caster );
    }

    void _onClick_Btn_gohotline_Btn(GameObject caster)
    {
        onClick_Btn_gohotline_Btn( caster );
    }

    void _onClick_Btn_LockScreen_Btn(GameObject caster)
    {
        onClick_Btn_LockScreen_Btn( caster );
    }

    void _onClick_Btn_OutStuck_Btn(GameObject caster)
    {
        onClick_Btn_OutStuck_Btn( caster );
    }

    void _onClick_Btn_RecordVideo_Btn(GameObject caster)
    {
        onClick_Btn_RecordVideo_Btn( caster );
    }

    void _onClick_Btn_Bible_Btn(GameObject caster)
    {
        onClick_Btn_Bible_Btn( caster );
    }

    void _onClick_RoleBaseHp_icon_Btn(GameObject caster)
    {
        onClick_RoleBaseHp_icon_Btn( caster );
    }

    void _onClick_RoleHighHp_icon_Btn(GameObject caster)
    {
        onClick_RoleHighHp_icon_Btn( caster );
    }

    void _onClick_HpAtOnceLabel_Btn(GameObject caster)
    {
        onClick_HpAtOnceLabel_Btn( caster );
    }

    void _onClick_RoleBaseMp_icon_Btn(GameObject caster)
    {
        onClick_RoleBaseMp_icon_Btn( caster );
    }

    void _onClick_PetBaseHp_icon_Btn(GameObject caster)
    {
        onClick_PetBaseHp_icon_Btn( caster );
    }

    void _onClick_ConfirmBtn_Btn(GameObject caster)
    {
        onClick_ConfirmBtn_Btn( caster );
    }

    void _onClick_NoteBtn_Btn(GameObject caster)
    {
        onClick_NoteBtn_Btn( caster );
    }

    void _onClick_ChatBtn_Btn(GameObject caster)
    {
        onClick_ChatBtn_Btn( caster );
    }


}
