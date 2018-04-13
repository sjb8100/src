//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RidePanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//马厩
		MaJiu = 1,
		//图鉴
		TuJian = 4,
		//骑术
		QiShu = 5,
		Max,
    }

   FastComponent         fastComponent;

    UIWidget             m_widget_content;

    Transform            m_trans_Ridescrollview;

    Transform            m_trans_middle;

    Transform            m_trans_RidePropContent;

    UITexture            m__model_bg;

    UITexture            m__rideModel;

    UILabel              m_label_Ride_Name;

    UILabel              m_label_RideSpeedLabel;

    UIButton             m_btn_btn_fight;

    UILabel              m_label_speed;

    UILabel              m_label_level;

    UILabel              m_label_life;

    UIButton             m_btn_btn_satiation_setting;

    UILabel              m_label_Repletion;

    UIButton             m_btn_btn_seal;

    UIButton             m_btn_btn_diuqi;

    UISlider             m_slider_ExpSlider;

    UILabel              m_label_expLabel;

    UIButton             m_btn_btn_addExp;

    UILabel              m_label_maxLevel;

    Transform            m_trans_RideStrategeContent;

    UIButton             m_btn_RideUItips_1;

    Transform            m_trans_left;

    UITexture            m__qishumodel_bg;

    UILabel              m_label_Ride_Name2;

    UITexture            m__rideModel2;

    UIButton             m_btn_RideQuality;

    UITexture            m__selecthorse;

    UISprite             m_sprite_biankuang;

    UISprite             m_sprite_jiahao;

    Transform            m_trans_Particle;

    Transform            m_trans_right;

    UILabel              m_label_knightfightNumber;

    UIGrid               m_grid_attributeGrid;

    UILabel              m_label_liliang;

    UILabel              m_label_liliangNum;

    UILabel              m_label_tili;

    UILabel              m_label_tiliNum;

    UILabel              m_label_minjie;

    UILabel              m_label_minjieNum;

    UILabel              m_label_zhili;

    UILabel              m_label_zhiliNum;

    UILabel              m_label_jingli;

    UILabel              m_label_jingliNum;

    UILabel              m_label_sudu;

    UILabel              m_label_suduNum;

    UILabel              m_label_zhuanghua;

    UILabel              m_label_zhuanghuaNum;

    UILabel              m_label_qishushuoming;

    UILabel              m_label_qishushuoming2;

    Transform            m_trans_Max;

    Transform            m_trans_LevelUp;

    UISlider             m_slider_Exp2Slider;

    UILabel              m_label_exp2Label;

    UILabel              m_label_levelNum;

    Transform            m_trans_ItemRoot;

    UILabel              m_label_exe1Num;

    UILabel              m_label_exe2Num;

    UILabel              m_label_exe3Num;

    Transform            m_trans_LevelBreak;

    Transform            m_trans_breakitem;

    UIButton             m_btn_BreakBtn;

    Transform            m_trans_SkillContent;

    UITexture            m__Icon;

    UILabel              m_label_Skill_RideName;

    UILabel              m_label_Skill_RideLevel;

    Transform            m_trans_SkillScrollview;

    UIButton             m_btn_RideUItips_2;

    Transform            m_trans_AdorationContent;

    Transform            m_trans_oldObj;

    UIButton             m_btn_btn_Old_delete;

    UILabel              m_label_Old_speed_Before;

    UILabel              m_label_Old_speed_After;

    UILabel              m_label_Old_level_Before;

    UILabel              m_label_Old_level_After;

    UILabel              m_label_Old_name;

    UITexture            m__Old_icon;

    UIWidget             m_widget_old_select;

    Transform            m_trans_newObj;

    UIButton             m_btn_btn_New_delete;

    UILabel              m_label_New_speed_Before;

    UILabel              m_label_New_speed_After;

    UILabel              m_label_New_level_Before;

    UILabel              m_label_New_level_After;

    UILabel              m_label_New_name;

    UITexture            m__New_icon;

    UIWidget             m_widget_new_select;

    UIToggle             m_toggle_ptchuancheng;

    UILabel              m_label_transExpNormal;

    UILabel              m_label_PTxiaohao;

    UIToggle             m_toggle_wmchuancheng;

    UILabel              m_label_transExpPerfect;

    UILabel              m_label_WMxiaohao;

    UIButton             m_btn_btn_Adoration;

    UIButton             m_btn_RideUItips_3;

    UIWidget             m_widget_tujiancontent;

    UIGridCreatorBase    m_ctor_tujianscroll;

    UIGridCreatorBase    m_ctor_rideQRoot;

    UISprite             m_sprite_UIRideGrid;

    Transform            m_trans_UIRideSkillGrid;

    UIWidget             m_widget_UIRideViewGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_content = fastComponent.FastGetComponent<UIWidget>("content");
       if( null == m_widget_content )
       {
            Engine.Utility.Log.Error("m_widget_content 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Ridescrollview = fastComponent.FastGetComponent<Transform>("Ridescrollview");
       if( null == m_trans_Ridescrollview )
       {
            Engine.Utility.Log.Error("m_trans_Ridescrollview 为空，请检查prefab是否缺乏组件");
       }
        m_trans_middle = fastComponent.FastGetComponent<Transform>("middle");
       if( null == m_trans_middle )
       {
            Engine.Utility.Log.Error("m_trans_middle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RidePropContent = fastComponent.FastGetComponent<Transform>("RidePropContent");
       if( null == m_trans_RidePropContent )
       {
            Engine.Utility.Log.Error("m_trans_RidePropContent 为空，请检查prefab是否缺乏组件");
       }
        m__model_bg = fastComponent.FastGetComponent<UITexture>("model_bg");
       if( null == m__model_bg )
       {
            Engine.Utility.Log.Error("m__model_bg 为空，请检查prefab是否缺乏组件");
       }
        m__rideModel = fastComponent.FastGetComponent<UITexture>("rideModel");
       if( null == m__rideModel )
       {
            Engine.Utility.Log.Error("m__rideModel 为空，请检查prefab是否缺乏组件");
       }
        m_label_Ride_Name = fastComponent.FastGetComponent<UILabel>("Ride_Name");
       if( null == m_label_Ride_Name )
       {
            Engine.Utility.Log.Error("m_label_Ride_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_RideSpeedLabel = fastComponent.FastGetComponent<UILabel>("RideSpeedLabel");
       if( null == m_label_RideSpeedLabel )
       {
            Engine.Utility.Log.Error("m_label_RideSpeedLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_fight = fastComponent.FastGetComponent<UIButton>("btn_fight");
       if( null == m_btn_btn_fight )
       {
            Engine.Utility.Log.Error("m_btn_btn_fight 为空，请检查prefab是否缺乏组件");
       }
        m_label_speed = fastComponent.FastGetComponent<UILabel>("speed");
       if( null == m_label_speed )
       {
            Engine.Utility.Log.Error("m_label_speed 为空，请检查prefab是否缺乏组件");
       }
        m_label_level = fastComponent.FastGetComponent<UILabel>("level");
       if( null == m_label_level )
       {
            Engine.Utility.Log.Error("m_label_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_life = fastComponent.FastGetComponent<UILabel>("life");
       if( null == m_label_life )
       {
            Engine.Utility.Log.Error("m_label_life 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_satiation_setting = fastComponent.FastGetComponent<UIButton>("btn_satiation_setting");
       if( null == m_btn_btn_satiation_setting )
       {
            Engine.Utility.Log.Error("m_btn_btn_satiation_setting 为空，请检查prefab是否缺乏组件");
       }
        m_label_Repletion = fastComponent.FastGetComponent<UILabel>("Repletion");
       if( null == m_label_Repletion )
       {
            Engine.Utility.Log.Error("m_label_Repletion 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_seal = fastComponent.FastGetComponent<UIButton>("btn_seal");
       if( null == m_btn_btn_seal )
       {
            Engine.Utility.Log.Error("m_btn_btn_seal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_diuqi = fastComponent.FastGetComponent<UIButton>("btn_diuqi");
       if( null == m_btn_btn_diuqi )
       {
            Engine.Utility.Log.Error("m_btn_btn_diuqi 为空，请检查prefab是否缺乏组件");
       }
        m_slider_ExpSlider = fastComponent.FastGetComponent<UISlider>("ExpSlider");
       if( null == m_slider_ExpSlider )
       {
            Engine.Utility.Log.Error("m_slider_ExpSlider 为空，请检查prefab是否缺乏组件");
       }
        m_label_expLabel = fastComponent.FastGetComponent<UILabel>("expLabel");
       if( null == m_label_expLabel )
       {
            Engine.Utility.Log.Error("m_label_expLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_addExp = fastComponent.FastGetComponent<UIButton>("btn_addExp");
       if( null == m_btn_btn_addExp )
       {
            Engine.Utility.Log.Error("m_btn_btn_addExp 为空，请检查prefab是否缺乏组件");
       }
        m_label_maxLevel = fastComponent.FastGetComponent<UILabel>("maxLevel");
       if( null == m_label_maxLevel )
       {
            Engine.Utility.Log.Error("m_label_maxLevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RideStrategeContent = fastComponent.FastGetComponent<Transform>("RideStrategeContent");
       if( null == m_trans_RideStrategeContent )
       {
            Engine.Utility.Log.Error("m_trans_RideStrategeContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RideUItips_1 = fastComponent.FastGetComponent<UIButton>("RideUItips_1");
       if( null == m_btn_RideUItips_1 )
       {
            Engine.Utility.Log.Error("m_btn_RideUItips_1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_left = fastComponent.FastGetComponent<Transform>("left");
       if( null == m_trans_left )
       {
            Engine.Utility.Log.Error("m_trans_left 为空，请检查prefab是否缺乏组件");
       }
        m__qishumodel_bg = fastComponent.FastGetComponent<UITexture>("qishumodel_bg");
       if( null == m__qishumodel_bg )
       {
            Engine.Utility.Log.Error("m__qishumodel_bg 为空，请检查prefab是否缺乏组件");
       }
        m_label_Ride_Name2 = fastComponent.FastGetComponent<UILabel>("Ride_Name2");
       if( null == m_label_Ride_Name2 )
       {
            Engine.Utility.Log.Error("m_label_Ride_Name2 为空，请检查prefab是否缺乏组件");
       }
        m__rideModel2 = fastComponent.FastGetComponent<UITexture>("rideModel2");
       if( null == m__rideModel2 )
       {
            Engine.Utility.Log.Error("m__rideModel2 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RideQuality = fastComponent.FastGetComponent<UIButton>("RideQuality");
       if( null == m_btn_RideQuality )
       {
            Engine.Utility.Log.Error("m_btn_RideQuality 为空，请检查prefab是否缺乏组件");
       }
        m__selecthorse = fastComponent.FastGetComponent<UITexture>("selecthorse");
       if( null == m__selecthorse )
       {
            Engine.Utility.Log.Error("m__selecthorse 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_biankuang = fastComponent.FastGetComponent<UISprite>("biankuang");
       if( null == m_sprite_biankuang )
       {
            Engine.Utility.Log.Error("m_sprite_biankuang 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_jiahao = fastComponent.FastGetComponent<UISprite>("jiahao");
       if( null == m_sprite_jiahao )
       {
            Engine.Utility.Log.Error("m_sprite_jiahao 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Particle = fastComponent.FastGetComponent<Transform>("Particle");
       if( null == m_trans_Particle )
       {
            Engine.Utility.Log.Error("m_trans_Particle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_right = fastComponent.FastGetComponent<Transform>("right");
       if( null == m_trans_right )
       {
            Engine.Utility.Log.Error("m_trans_right 为空，请检查prefab是否缺乏组件");
       }
        m_label_knightfightNumber = fastComponent.FastGetComponent<UILabel>("knightfightNumber");
       if( null == m_label_knightfightNumber )
       {
            Engine.Utility.Log.Error("m_label_knightfightNumber 为空，请检查prefab是否缺乏组件");
       }
        m_grid_attributeGrid = fastComponent.FastGetComponent<UIGrid>("attributeGrid");
       if( null == m_grid_attributeGrid )
       {
            Engine.Utility.Log.Error("m_grid_attributeGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_liliang = fastComponent.FastGetComponent<UILabel>("liliang");
       if( null == m_label_liliang )
       {
            Engine.Utility.Log.Error("m_label_liliang 为空，请检查prefab是否缺乏组件");
       }
        m_label_liliangNum = fastComponent.FastGetComponent<UILabel>("liliangNum");
       if( null == m_label_liliangNum )
       {
            Engine.Utility.Log.Error("m_label_liliangNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_tili = fastComponent.FastGetComponent<UILabel>("tili");
       if( null == m_label_tili )
       {
            Engine.Utility.Log.Error("m_label_tili 为空，请检查prefab是否缺乏组件");
       }
        m_label_tiliNum = fastComponent.FastGetComponent<UILabel>("tiliNum");
       if( null == m_label_tiliNum )
       {
            Engine.Utility.Log.Error("m_label_tiliNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_minjie = fastComponent.FastGetComponent<UILabel>("minjie");
       if( null == m_label_minjie )
       {
            Engine.Utility.Log.Error("m_label_minjie 为空，请检查prefab是否缺乏组件");
       }
        m_label_minjieNum = fastComponent.FastGetComponent<UILabel>("minjieNum");
       if( null == m_label_minjieNum )
       {
            Engine.Utility.Log.Error("m_label_minjieNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhili = fastComponent.FastGetComponent<UILabel>("zhili");
       if( null == m_label_zhili )
       {
            Engine.Utility.Log.Error("m_label_zhili 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhiliNum = fastComponent.FastGetComponent<UILabel>("zhiliNum");
       if( null == m_label_zhiliNum )
       {
            Engine.Utility.Log.Error("m_label_zhiliNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_jingli = fastComponent.FastGetComponent<UILabel>("jingli");
       if( null == m_label_jingli )
       {
            Engine.Utility.Log.Error("m_label_jingli 为空，请检查prefab是否缺乏组件");
       }
        m_label_jingliNum = fastComponent.FastGetComponent<UILabel>("jingliNum");
       if( null == m_label_jingliNum )
       {
            Engine.Utility.Log.Error("m_label_jingliNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_sudu = fastComponent.FastGetComponent<UILabel>("sudu");
       if( null == m_label_sudu )
       {
            Engine.Utility.Log.Error("m_label_sudu 为空，请检查prefab是否缺乏组件");
       }
        m_label_suduNum = fastComponent.FastGetComponent<UILabel>("suduNum");
       if( null == m_label_suduNum )
       {
            Engine.Utility.Log.Error("m_label_suduNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhuanghua = fastComponent.FastGetComponent<UILabel>("zhuanghua");
       if( null == m_label_zhuanghua )
       {
            Engine.Utility.Log.Error("m_label_zhuanghua 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhuanghuaNum = fastComponent.FastGetComponent<UILabel>("zhuanghuaNum");
       if( null == m_label_zhuanghuaNum )
       {
            Engine.Utility.Log.Error("m_label_zhuanghuaNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_qishushuoming = fastComponent.FastGetComponent<UILabel>("qishushuoming");
       if( null == m_label_qishushuoming )
       {
            Engine.Utility.Log.Error("m_label_qishushuoming 为空，请检查prefab是否缺乏组件");
       }
        m_label_qishushuoming2 = fastComponent.FastGetComponent<UILabel>("qishushuoming2");
       if( null == m_label_qishushuoming2 )
       {
            Engine.Utility.Log.Error("m_label_qishushuoming2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Max = fastComponent.FastGetComponent<Transform>("Max");
       if( null == m_trans_Max )
       {
            Engine.Utility.Log.Error("m_trans_Max 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LevelUp = fastComponent.FastGetComponent<Transform>("LevelUp");
       if( null == m_trans_LevelUp )
       {
            Engine.Utility.Log.Error("m_trans_LevelUp 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Exp2Slider = fastComponent.FastGetComponent<UISlider>("Exp2Slider");
       if( null == m_slider_Exp2Slider )
       {
            Engine.Utility.Log.Error("m_slider_Exp2Slider 为空，请检查prefab是否缺乏组件");
       }
        m_label_exp2Label = fastComponent.FastGetComponent<UILabel>("exp2Label");
       if( null == m_label_exp2Label )
       {
            Engine.Utility.Log.Error("m_label_exp2Label 为空，请检查prefab是否缺乏组件");
       }
        m_label_levelNum = fastComponent.FastGetComponent<UILabel>("levelNum");
       if( null == m_label_levelNum )
       {
            Engine.Utility.Log.Error("m_label_levelNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemRoot = fastComponent.FastGetComponent<Transform>("ItemRoot");
       if( null == m_trans_ItemRoot )
       {
            Engine.Utility.Log.Error("m_trans_ItemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_exe1Num = fastComponent.FastGetComponent<UILabel>("exe1Num");
       if( null == m_label_exe1Num )
       {
            Engine.Utility.Log.Error("m_label_exe1Num 为空，请检查prefab是否缺乏组件");
       }
        m_label_exe2Num = fastComponent.FastGetComponent<UILabel>("exe2Num");
       if( null == m_label_exe2Num )
       {
            Engine.Utility.Log.Error("m_label_exe2Num 为空，请检查prefab是否缺乏组件");
       }
        m_label_exe3Num = fastComponent.FastGetComponent<UILabel>("exe3Num");
       if( null == m_label_exe3Num )
       {
            Engine.Utility.Log.Error("m_label_exe3Num 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LevelBreak = fastComponent.FastGetComponent<Transform>("LevelBreak");
       if( null == m_trans_LevelBreak )
       {
            Engine.Utility.Log.Error("m_trans_LevelBreak 为空，请检查prefab是否缺乏组件");
       }
        m_trans_breakitem = fastComponent.FastGetComponent<Transform>("breakitem");
       if( null == m_trans_breakitem )
       {
            Engine.Utility.Log.Error("m_trans_breakitem 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BreakBtn = fastComponent.FastGetComponent<UIButton>("BreakBtn");
       if( null == m_btn_BreakBtn )
       {
            Engine.Utility.Log.Error("m_btn_BreakBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SkillContent = fastComponent.FastGetComponent<Transform>("SkillContent");
       if( null == m_trans_SkillContent )
       {
            Engine.Utility.Log.Error("m_trans_SkillContent 为空，请检查prefab是否缺乏组件");
       }
        m__Icon = fastComponent.FastGetComponent<UITexture>("Icon");
       if( null == m__Icon )
       {
            Engine.Utility.Log.Error("m__Icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_Skill_RideName = fastComponent.FastGetComponent<UILabel>("Skill_RideName");
       if( null == m_label_Skill_RideName )
       {
            Engine.Utility.Log.Error("m_label_Skill_RideName 为空，请检查prefab是否缺乏组件");
       }
        m_label_Skill_RideLevel = fastComponent.FastGetComponent<UILabel>("Skill_RideLevel");
       if( null == m_label_Skill_RideLevel )
       {
            Engine.Utility.Log.Error("m_label_Skill_RideLevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SkillScrollview = fastComponent.FastGetComponent<Transform>("SkillScrollview");
       if( null == m_trans_SkillScrollview )
       {
            Engine.Utility.Log.Error("m_trans_SkillScrollview 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RideUItips_2 = fastComponent.FastGetComponent<UIButton>("RideUItips_2");
       if( null == m_btn_RideUItips_2 )
       {
            Engine.Utility.Log.Error("m_btn_RideUItips_2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AdorationContent = fastComponent.FastGetComponent<Transform>("AdorationContent");
       if( null == m_trans_AdorationContent )
       {
            Engine.Utility.Log.Error("m_trans_AdorationContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_oldObj = fastComponent.FastGetComponent<Transform>("oldObj");
       if( null == m_trans_oldObj )
       {
            Engine.Utility.Log.Error("m_trans_oldObj 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Old_delete = fastComponent.FastGetComponent<UIButton>("btn_Old_delete");
       if( null == m_btn_btn_Old_delete )
       {
            Engine.Utility.Log.Error("m_btn_btn_Old_delete 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_speed_Before = fastComponent.FastGetComponent<UILabel>("Old_speed_Before");
       if( null == m_label_Old_speed_Before )
       {
            Engine.Utility.Log.Error("m_label_Old_speed_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_speed_After = fastComponent.FastGetComponent<UILabel>("Old_speed_After");
       if( null == m_label_Old_speed_After )
       {
            Engine.Utility.Log.Error("m_label_Old_speed_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_level_Before = fastComponent.FastGetComponent<UILabel>("Old_level_Before");
       if( null == m_label_Old_level_Before )
       {
            Engine.Utility.Log.Error("m_label_Old_level_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_level_After = fastComponent.FastGetComponent<UILabel>("Old_level_After");
       if( null == m_label_Old_level_After )
       {
            Engine.Utility.Log.Error("m_label_Old_level_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_name = fastComponent.FastGetComponent<UILabel>("Old_name");
       if( null == m_label_Old_name )
       {
            Engine.Utility.Log.Error("m_label_Old_name 为空，请检查prefab是否缺乏组件");
       }
        m__Old_icon = fastComponent.FastGetComponent<UITexture>("Old_icon");
       if( null == m__Old_icon )
       {
            Engine.Utility.Log.Error("m__Old_icon 为空，请检查prefab是否缺乏组件");
       }
        m_widget_old_select = fastComponent.FastGetComponent<UIWidget>("old_select");
       if( null == m_widget_old_select )
       {
            Engine.Utility.Log.Error("m_widget_old_select 为空，请检查prefab是否缺乏组件");
       }
        m_trans_newObj = fastComponent.FastGetComponent<Transform>("newObj");
       if( null == m_trans_newObj )
       {
            Engine.Utility.Log.Error("m_trans_newObj 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_New_delete = fastComponent.FastGetComponent<UIButton>("btn_New_delete");
       if( null == m_btn_btn_New_delete )
       {
            Engine.Utility.Log.Error("m_btn_btn_New_delete 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_speed_Before = fastComponent.FastGetComponent<UILabel>("New_speed_Before");
       if( null == m_label_New_speed_Before )
       {
            Engine.Utility.Log.Error("m_label_New_speed_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_speed_After = fastComponent.FastGetComponent<UILabel>("New_speed_After");
       if( null == m_label_New_speed_After )
       {
            Engine.Utility.Log.Error("m_label_New_speed_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_level_Before = fastComponent.FastGetComponent<UILabel>("New_level_Before");
       if( null == m_label_New_level_Before )
       {
            Engine.Utility.Log.Error("m_label_New_level_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_level_After = fastComponent.FastGetComponent<UILabel>("New_level_After");
       if( null == m_label_New_level_After )
       {
            Engine.Utility.Log.Error("m_label_New_level_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_name = fastComponent.FastGetComponent<UILabel>("New_name");
       if( null == m_label_New_name )
       {
            Engine.Utility.Log.Error("m_label_New_name 为空，请检查prefab是否缺乏组件");
       }
        m__New_icon = fastComponent.FastGetComponent<UITexture>("New_icon");
       if( null == m__New_icon )
       {
            Engine.Utility.Log.Error("m__New_icon 为空，请检查prefab是否缺乏组件");
       }
        m_widget_new_select = fastComponent.FastGetComponent<UIWidget>("new_select");
       if( null == m_widget_new_select )
       {
            Engine.Utility.Log.Error("m_widget_new_select 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_ptchuancheng = fastComponent.FastGetComponent<UIToggle>("ptchuancheng");
       if( null == m_toggle_ptchuancheng )
       {
            Engine.Utility.Log.Error("m_toggle_ptchuancheng 为空，请检查prefab是否缺乏组件");
       }
        m_label_transExpNormal = fastComponent.FastGetComponent<UILabel>("transExpNormal");
       if( null == m_label_transExpNormal )
       {
            Engine.Utility.Log.Error("m_label_transExpNormal 为空，请检查prefab是否缺乏组件");
       }
        m_label_PTxiaohao = fastComponent.FastGetComponent<UILabel>("PTxiaohao");
       if( null == m_label_PTxiaohao )
       {
            Engine.Utility.Log.Error("m_label_PTxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_wmchuancheng = fastComponent.FastGetComponent<UIToggle>("wmchuancheng");
       if( null == m_toggle_wmchuancheng )
       {
            Engine.Utility.Log.Error("m_toggle_wmchuancheng 为空，请检查prefab是否缺乏组件");
       }
        m_label_transExpPerfect = fastComponent.FastGetComponent<UILabel>("transExpPerfect");
       if( null == m_label_transExpPerfect )
       {
            Engine.Utility.Log.Error("m_label_transExpPerfect 为空，请检查prefab是否缺乏组件");
       }
        m_label_WMxiaohao = fastComponent.FastGetComponent<UILabel>("WMxiaohao");
       if( null == m_label_WMxiaohao )
       {
            Engine.Utility.Log.Error("m_label_WMxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Adoration = fastComponent.FastGetComponent<UIButton>("btn_Adoration");
       if( null == m_btn_btn_Adoration )
       {
            Engine.Utility.Log.Error("m_btn_btn_Adoration 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RideUItips_3 = fastComponent.FastGetComponent<UIButton>("RideUItips_3");
       if( null == m_btn_RideUItips_3 )
       {
            Engine.Utility.Log.Error("m_btn_RideUItips_3 为空，请检查prefab是否缺乏组件");
       }
        m_widget_tujiancontent = fastComponent.FastGetComponent<UIWidget>("tujiancontent");
       if( null == m_widget_tujiancontent )
       {
            Engine.Utility.Log.Error("m_widget_tujiancontent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_tujianscroll = fastComponent.FastGetComponent<UIGridCreatorBase>("tujianscroll");
       if( null == m_ctor_tujianscroll )
       {
            Engine.Utility.Log.Error("m_ctor_tujianscroll 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_rideQRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("rideQRoot");
       if( null == m_ctor_rideQRoot )
       {
            Engine.Utility.Log.Error("m_ctor_rideQRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UIRideGrid = fastComponent.FastGetComponent<UISprite>("UIRideGrid");
       if( null == m_sprite_UIRideGrid )
       {
            Engine.Utility.Log.Error("m_sprite_UIRideGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIRideSkillGrid = fastComponent.FastGetComponent<Transform>("UIRideSkillGrid");
       if( null == m_trans_UIRideSkillGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIRideSkillGrid 为空，请检查prefab是否缺乏组件");
       }
        m_widget_UIRideViewGrid = fastComponent.FastGetComponent<UIWidget>("UIRideViewGrid");
       if( null == m_widget_UIRideViewGrid )
       {
            Engine.Utility.Log.Error("m_widget_UIRideViewGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_fight.gameObject).onClick = _onClick_Btn_fight_Btn;
        UIEventListener.Get(m_btn_btn_satiation_setting.gameObject).onClick = _onClick_Btn_satiation_setting_Btn;
        UIEventListener.Get(m_btn_btn_seal.gameObject).onClick = _onClick_Btn_seal_Btn;
        UIEventListener.Get(m_btn_btn_diuqi.gameObject).onClick = _onClick_Btn_diuqi_Btn;
        UIEventListener.Get(m_btn_btn_addExp.gameObject).onClick = _onClick_Btn_addExp_Btn;
        UIEventListener.Get(m_btn_RideUItips_1.gameObject).onClick = _onClick_RideUItips_1_Btn;
        UIEventListener.Get(m_btn_RideQuality.gameObject).onClick = _onClick_RideQuality_Btn;
        UIEventListener.Get(m_btn_BreakBtn.gameObject).onClick = _onClick_BreakBtn_Btn;
        UIEventListener.Get(m_btn_RideUItips_2.gameObject).onClick = _onClick_RideUItips_2_Btn;
        UIEventListener.Get(m_btn_btn_Old_delete.gameObject).onClick = _onClick_Btn_Old_delete_Btn;
        UIEventListener.Get(m_btn_btn_New_delete.gameObject).onClick = _onClick_Btn_New_delete_Btn;
        UIEventListener.Get(m_btn_btn_Adoration.gameObject).onClick = _onClick_Btn_Adoration_Btn;
        UIEventListener.Get(m_btn_RideUItips_3.gameObject).onClick = _onClick_RideUItips_3_Btn;
    }

    void _onClick_Btn_fight_Btn(GameObject caster)
    {
        onClick_Btn_fight_Btn( caster );
    }

    void _onClick_Btn_satiation_setting_Btn(GameObject caster)
    {
        onClick_Btn_satiation_setting_Btn( caster );
    }

    void _onClick_Btn_seal_Btn(GameObject caster)
    {
        onClick_Btn_seal_Btn( caster );
    }

    void _onClick_Btn_diuqi_Btn(GameObject caster)
    {
        onClick_Btn_diuqi_Btn( caster );
    }

    void _onClick_Btn_addExp_Btn(GameObject caster)
    {
        onClick_Btn_addExp_Btn( caster );
    }

    void _onClick_RideUItips_1_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_RideQuality_Btn(GameObject caster)
    {
        onClick_RideQuality_Btn( caster );
    }

    void _onClick_BreakBtn_Btn(GameObject caster)
    {
        onClick_BreakBtn_Btn( caster );
    }

    void _onClick_RideUItips_2_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_Btn_Old_delete_Btn(GameObject caster)
    {
        onClick_Btn_Old_delete_Btn( caster );
    }

    void _onClick_Btn_New_delete_Btn(GameObject caster)
    {
        onClick_Btn_New_delete_Btn( caster );
    }

    void _onClick_Btn_Adoration_Btn(GameObject caster)
    {
        onClick_Btn_Adoration_Btn( caster );
    }

    void _onClick_RideUItips_3_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }


}
