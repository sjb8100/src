//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class LearnSkillPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//技能
		JiNeng = 1,
		//心法
		XinFa = 2,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_LearnSkillContent;

    UIWidget             m_widget_currency;

    UIWidget             m_widget_state_1;

    UILabel              m_label_StateOneLabel;

    UIWidget             m_widget_state_2;

    UILabel              m_label_StateTwoLabel;

    Transform            m_trans_up;

    Transform            m_trans_skill_shengji;

    Transform            m_trans_skill_setting;

    Transform            m_trans_skill_shengji_area;

    Transform            m_trans_WeiJieSuoContent;

    UILabel              m_label_WeiJieSuoLocklevel;

    Transform            m_trans_JieSuoContent;

    UILabel              m_label_Gold_Cost;

    UILabel              m_label_Exp_Now;

    UILabel              m_label_Exp_Cost;

    UIButton             m_btn_Btn_LevelUp;

    UIButton             m_btn_Btn_AllLevelUp;

    UILabel              m_label_Locklevel;

    Transform            m_trans_MaxContent;

    UILabel              m_label_Formneed;

    UILabel              m_label_Mp;

    UILabel              m_label_Name;

    UILabel              m_label_CD;

    UILabel              m_label_Level;

    UILabel              m_label_Describe;

    Transform            m_trans_MaxHideContent;

    UILabel              m_label_Describe_NextLevel;

    UIWidget             m_widget_skill_setting_area;

    UILabel              m_label_skill_setting_statename;

    UILabel              m_label_CurStateLabel;

    UITexture            m__stateSpr;

    UISprite             m_sprite_statelock;

    UILabel              m_label_state_unlocklevel;

    UILabel              m_label_zhu_Label;

    UIWidget             m_widget_SkillContainer;

    UIButton             m_btn_btnStatus1;

    UIButton             m_btn_btnStatus2;

    UIButton             m_btn_btnClearSkillSet;

    UIButton             m_btn_AutoSkillSet;

    Transform            m_trans_HeartSkillContent;

    UILabel              m_label_GhostLevelNum;

    UILabel              m_label_HeartPointNum;

    UIButton             m_btn_Btn_HeartPointAdd;

    UIButton             m_btn_Btn_Reset;

    UIGrid               m_grid_heartSkillGridContent;

    UIWidget             m_widget_HeartSkill_shengji_area;

    UIWidget             m_widget_HeartSkill_unlock;

    UILabel              m_label_HeartSkill_unlock_level_now;

    UILabel              m_label_HeartSkill_unlock_name_now;

    UILabel              m_label_HeartSkill_unlock_describe_now;

    UIWidget             m_widget_HeartSkill_NextLevel;

    UILabel              m_label_HeartSkill_unlock_describe_nextlevel;

    UILabel              m_label_HeartSkill_unlock_locklevel_nextlevel;

    UILabel              m_label_HeartSkill_unlock_locklevel_PreSkills;

    UILabel              m_label_HeartSkill_consume;

    UILabel              m_label_HeartSkill_unlock_xiaohao_Num;

    UIButton             m_btn_HeartSkill_btn_shengji;

    UIWidget             m_widget_HeartSkill_max;

    Transform            m_trans_HeartSkill_btn_Max;

    UIWidget             m_widget_HeartSkill__lock;

    UILabel              m_label_HeartSkill_lock_name;

    UILabel              m_label_HeartSkill_lock_level;

    UILabel              m_label_HeartSkill_lock_locklevel;

    UILabel              m_label_HeartSkill_lock_describe;

    UILabel              m_label_HeartSkill_lock_xiaohao_Num;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_LearnSkillContent = fastComponent.FastGetComponent<Transform>("LearnSkillContent");
       if( null == m_trans_LearnSkillContent )
       {
            Engine.Utility.Log.Error("m_trans_LearnSkillContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_currency = fastComponent.FastGetComponent<UIWidget>("currency");
       if( null == m_widget_currency )
       {
            Engine.Utility.Log.Error("m_widget_currency 为空，请检查prefab是否缺乏组件");
       }
        m_widget_state_1 = fastComponent.FastGetComponent<UIWidget>("state_1");
       if( null == m_widget_state_1 )
       {
            Engine.Utility.Log.Error("m_widget_state_1 为空，请检查prefab是否缺乏组件");
       }
        m_label_StateOneLabel = fastComponent.FastGetComponent<UILabel>("StateOneLabel");
       if( null == m_label_StateOneLabel )
       {
            Engine.Utility.Log.Error("m_label_StateOneLabel 为空，请检查prefab是否缺乏组件");
       }
        m_widget_state_2 = fastComponent.FastGetComponent<UIWidget>("state_2");
       if( null == m_widget_state_2 )
       {
            Engine.Utility.Log.Error("m_widget_state_2 为空，请检查prefab是否缺乏组件");
       }
        m_label_StateTwoLabel = fastComponent.FastGetComponent<UILabel>("StateTwoLabel");
       if( null == m_label_StateTwoLabel )
       {
            Engine.Utility.Log.Error("m_label_StateTwoLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_up = fastComponent.FastGetComponent<Transform>("up");
       if( null == m_trans_up )
       {
            Engine.Utility.Log.Error("m_trans_up 为空，请检查prefab是否缺乏组件");
       }
        m_trans_skill_shengji = fastComponent.FastGetComponent<Transform>("skill_shengji");
       if( null == m_trans_skill_shengji )
       {
            Engine.Utility.Log.Error("m_trans_skill_shengji 为空，请检查prefab是否缺乏组件");
       }
        m_trans_skill_setting = fastComponent.FastGetComponent<Transform>("skill_setting");
       if( null == m_trans_skill_setting )
       {
            Engine.Utility.Log.Error("m_trans_skill_setting 为空，请检查prefab是否缺乏组件");
       }
        m_trans_skill_shengji_area = fastComponent.FastGetComponent<Transform>("skill_shengji_area");
       if( null == m_trans_skill_shengji_area )
       {
            Engine.Utility.Log.Error("m_trans_skill_shengji_area 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WeiJieSuoContent = fastComponent.FastGetComponent<Transform>("WeiJieSuoContent");
       if( null == m_trans_WeiJieSuoContent )
       {
            Engine.Utility.Log.Error("m_trans_WeiJieSuoContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_WeiJieSuoLocklevel = fastComponent.FastGetComponent<UILabel>("WeiJieSuoLocklevel");
       if( null == m_label_WeiJieSuoLocklevel )
       {
            Engine.Utility.Log.Error("m_label_WeiJieSuoLocklevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_JieSuoContent = fastComponent.FastGetComponent<Transform>("JieSuoContent");
       if( null == m_trans_JieSuoContent )
       {
            Engine.Utility.Log.Error("m_trans_JieSuoContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_Gold_Cost = fastComponent.FastGetComponent<UILabel>("Gold_Cost");
       if( null == m_label_Gold_Cost )
       {
            Engine.Utility.Log.Error("m_label_Gold_Cost 为空，请检查prefab是否缺乏组件");
       }
        m_label_Exp_Now = fastComponent.FastGetComponent<UILabel>("Exp_Now");
       if( null == m_label_Exp_Now )
       {
            Engine.Utility.Log.Error("m_label_Exp_Now 为空，请检查prefab是否缺乏组件");
       }
        m_label_Exp_Cost = fastComponent.FastGetComponent<UILabel>("Exp_Cost");
       if( null == m_label_Exp_Cost )
       {
            Engine.Utility.Log.Error("m_label_Exp_Cost 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_LevelUp = fastComponent.FastGetComponent<UIButton>("Btn_LevelUp");
       if( null == m_btn_Btn_LevelUp )
       {
            Engine.Utility.Log.Error("m_btn_Btn_LevelUp 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_AllLevelUp = fastComponent.FastGetComponent<UIButton>("Btn_AllLevelUp");
       if( null == m_btn_Btn_AllLevelUp )
       {
            Engine.Utility.Log.Error("m_btn_Btn_AllLevelUp 为空，请检查prefab是否缺乏组件");
       }
        m_label_Locklevel = fastComponent.FastGetComponent<UILabel>("Locklevel");
       if( null == m_label_Locklevel )
       {
            Engine.Utility.Log.Error("m_label_Locklevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MaxContent = fastComponent.FastGetComponent<Transform>("MaxContent");
       if( null == m_trans_MaxContent )
       {
            Engine.Utility.Log.Error("m_trans_MaxContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_Formneed = fastComponent.FastGetComponent<UILabel>("Formneed");
       if( null == m_label_Formneed )
       {
            Engine.Utility.Log.Error("m_label_Formneed 为空，请检查prefab是否缺乏组件");
       }
        m_label_Mp = fastComponent.FastGetComponent<UILabel>("Mp");
       if( null == m_label_Mp )
       {
            Engine.Utility.Log.Error("m_label_Mp 为空，请检查prefab是否缺乏组件");
       }
        m_label_Name = fastComponent.FastGetComponent<UILabel>("Name");
       if( null == m_label_Name )
       {
            Engine.Utility.Log.Error("m_label_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_CD = fastComponent.FastGetComponent<UILabel>("CD");
       if( null == m_label_CD )
       {
            Engine.Utility.Log.Error("m_label_CD 为空，请检查prefab是否缺乏组件");
       }
        m_label_Level = fastComponent.FastGetComponent<UILabel>("Level");
       if( null == m_label_Level )
       {
            Engine.Utility.Log.Error("m_label_Level 为空，请检查prefab是否缺乏组件");
       }
        m_label_Describe = fastComponent.FastGetComponent<UILabel>("Describe");
       if( null == m_label_Describe )
       {
            Engine.Utility.Log.Error("m_label_Describe 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MaxHideContent = fastComponent.FastGetComponent<Transform>("MaxHideContent");
       if( null == m_trans_MaxHideContent )
       {
            Engine.Utility.Log.Error("m_trans_MaxHideContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_Describe_NextLevel = fastComponent.FastGetComponent<UILabel>("Describe_NextLevel");
       if( null == m_label_Describe_NextLevel )
       {
            Engine.Utility.Log.Error("m_label_Describe_NextLevel 为空，请检查prefab是否缺乏组件");
       }
        m_widget_skill_setting_area = fastComponent.FastGetComponent<UIWidget>("skill_setting_area");
       if( null == m_widget_skill_setting_area )
       {
            Engine.Utility.Log.Error("m_widget_skill_setting_area 为空，请检查prefab是否缺乏组件");
       }
        m_label_skill_setting_statename = fastComponent.FastGetComponent<UILabel>("skill_setting_statename");
       if( null == m_label_skill_setting_statename )
       {
            Engine.Utility.Log.Error("m_label_skill_setting_statename 为空，请检查prefab是否缺乏组件");
       }
        m_label_CurStateLabel = fastComponent.FastGetComponent<UILabel>("CurStateLabel");
       if( null == m_label_CurStateLabel )
       {
            Engine.Utility.Log.Error("m_label_CurStateLabel 为空，请检查prefab是否缺乏组件");
       }
        m__stateSpr = fastComponent.FastGetComponent<UITexture>("stateSpr");
       if( null == m__stateSpr )
       {
            Engine.Utility.Log.Error("m__stateSpr 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_statelock = fastComponent.FastGetComponent<UISprite>("statelock");
       if( null == m_sprite_statelock )
       {
            Engine.Utility.Log.Error("m_sprite_statelock 为空，请检查prefab是否缺乏组件");
       }
        m_label_state_unlocklevel = fastComponent.FastGetComponent<UILabel>("state_unlocklevel");
       if( null == m_label_state_unlocklevel )
       {
            Engine.Utility.Log.Error("m_label_state_unlocklevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhu_Label = fastComponent.FastGetComponent<UILabel>("zhu_Label");
       if( null == m_label_zhu_Label )
       {
            Engine.Utility.Log.Error("m_label_zhu_Label 为空，请检查prefab是否缺乏组件");
       }
        m_widget_SkillContainer = fastComponent.FastGetComponent<UIWidget>("SkillContainer");
       if( null == m_widget_SkillContainer )
       {
            Engine.Utility.Log.Error("m_widget_SkillContainer 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnStatus1 = fastComponent.FastGetComponent<UIButton>("btnStatus1");
       if( null == m_btn_btnStatus1 )
       {
            Engine.Utility.Log.Error("m_btn_btnStatus1 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnStatus2 = fastComponent.FastGetComponent<UIButton>("btnStatus2");
       if( null == m_btn_btnStatus2 )
       {
            Engine.Utility.Log.Error("m_btn_btnStatus2 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnClearSkillSet = fastComponent.FastGetComponent<UIButton>("btnClearSkillSet");
       if( null == m_btn_btnClearSkillSet )
       {
            Engine.Utility.Log.Error("m_btn_btnClearSkillSet 为空，请检查prefab是否缺乏组件");
       }
        m_btn_AutoSkillSet = fastComponent.FastGetComponent<UIButton>("AutoSkillSet");
       if( null == m_btn_AutoSkillSet )
       {
            Engine.Utility.Log.Error("m_btn_AutoSkillSet 为空，请检查prefab是否缺乏组件");
       }
        m_trans_HeartSkillContent = fastComponent.FastGetComponent<Transform>("HeartSkillContent");
       if( null == m_trans_HeartSkillContent )
       {
            Engine.Utility.Log.Error("m_trans_HeartSkillContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_GhostLevelNum = fastComponent.FastGetComponent<UILabel>("GhostLevelNum");
       if( null == m_label_GhostLevelNum )
       {
            Engine.Utility.Log.Error("m_label_GhostLevelNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartPointNum = fastComponent.FastGetComponent<UILabel>("HeartPointNum");
       if( null == m_label_HeartPointNum )
       {
            Engine.Utility.Log.Error("m_label_HeartPointNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_HeartPointAdd = fastComponent.FastGetComponent<UIButton>("Btn_HeartPointAdd");
       if( null == m_btn_Btn_HeartPointAdd )
       {
            Engine.Utility.Log.Error("m_btn_Btn_HeartPointAdd 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Reset = fastComponent.FastGetComponent<UIButton>("Btn_Reset");
       if( null == m_btn_Btn_Reset )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Reset 为空，请检查prefab是否缺乏组件");
       }
        m_grid_heartSkillGridContent = fastComponent.FastGetComponent<UIGrid>("heartSkillGridContent");
       if( null == m_grid_heartSkillGridContent )
       {
            Engine.Utility.Log.Error("m_grid_heartSkillGridContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_HeartSkill_shengji_area = fastComponent.FastGetComponent<UIWidget>("HeartSkill_shengji_area");
       if( null == m_widget_HeartSkill_shengji_area )
       {
            Engine.Utility.Log.Error("m_widget_HeartSkill_shengji_area 为空，请检查prefab是否缺乏组件");
       }
        m_widget_HeartSkill_unlock = fastComponent.FastGetComponent<UIWidget>("HeartSkill_unlock");
       if( null == m_widget_HeartSkill_unlock )
       {
            Engine.Utility.Log.Error("m_widget_HeartSkill_unlock 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_unlock_level_now = fastComponent.FastGetComponent<UILabel>("HeartSkill_unlock_level_now");
       if( null == m_label_HeartSkill_unlock_level_now )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_unlock_level_now 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_unlock_name_now = fastComponent.FastGetComponent<UILabel>("HeartSkill_unlock_name_now");
       if( null == m_label_HeartSkill_unlock_name_now )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_unlock_name_now 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_unlock_describe_now = fastComponent.FastGetComponent<UILabel>("HeartSkill_unlock_describe_now");
       if( null == m_label_HeartSkill_unlock_describe_now )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_unlock_describe_now 为空，请检查prefab是否缺乏组件");
       }
        m_widget_HeartSkill_NextLevel = fastComponent.FastGetComponent<UIWidget>("HeartSkill_NextLevel");
       if( null == m_widget_HeartSkill_NextLevel )
       {
            Engine.Utility.Log.Error("m_widget_HeartSkill_NextLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_unlock_describe_nextlevel = fastComponent.FastGetComponent<UILabel>("HeartSkill_unlock_describe_nextlevel");
       if( null == m_label_HeartSkill_unlock_describe_nextlevel )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_unlock_describe_nextlevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_unlock_locklevel_nextlevel = fastComponent.FastGetComponent<UILabel>("HeartSkill_unlock_locklevel_nextlevel");
       if( null == m_label_HeartSkill_unlock_locklevel_nextlevel )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_unlock_locklevel_nextlevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_unlock_locklevel_PreSkills = fastComponent.FastGetComponent<UILabel>("HeartSkill_unlock_locklevel_PreSkills");
       if( null == m_label_HeartSkill_unlock_locklevel_PreSkills )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_unlock_locklevel_PreSkills 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_consume = fastComponent.FastGetComponent<UILabel>("HeartSkill_consume");
       if( null == m_label_HeartSkill_consume )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_consume 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_unlock_xiaohao_Num = fastComponent.FastGetComponent<UILabel>("HeartSkill_unlock_xiaohao_Num");
       if( null == m_label_HeartSkill_unlock_xiaohao_Num )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_unlock_xiaohao_Num 为空，请检查prefab是否缺乏组件");
       }
        m_btn_HeartSkill_btn_shengji = fastComponent.FastGetComponent<UIButton>("HeartSkill_btn_shengji");
       if( null == m_btn_HeartSkill_btn_shengji )
       {
            Engine.Utility.Log.Error("m_btn_HeartSkill_btn_shengji 为空，请检查prefab是否缺乏组件");
       }
        m_widget_HeartSkill_max = fastComponent.FastGetComponent<UIWidget>("HeartSkill_max");
       if( null == m_widget_HeartSkill_max )
       {
            Engine.Utility.Log.Error("m_widget_HeartSkill_max 为空，请检查prefab是否缺乏组件");
       }
        m_trans_HeartSkill_btn_Max = fastComponent.FastGetComponent<Transform>("HeartSkill_btn_Max");
       if( null == m_trans_HeartSkill_btn_Max )
       {
            Engine.Utility.Log.Error("m_trans_HeartSkill_btn_Max 为空，请检查prefab是否缺乏组件");
       }
        m_widget_HeartSkill__lock = fastComponent.FastGetComponent<UIWidget>("HeartSkill__lock");
       if( null == m_widget_HeartSkill__lock )
       {
            Engine.Utility.Log.Error("m_widget_HeartSkill__lock 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_lock_name = fastComponent.FastGetComponent<UILabel>("HeartSkill_lock_name");
       if( null == m_label_HeartSkill_lock_name )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_lock_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_lock_level = fastComponent.FastGetComponent<UILabel>("HeartSkill_lock_level");
       if( null == m_label_HeartSkill_lock_level )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_lock_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_lock_locklevel = fastComponent.FastGetComponent<UILabel>("HeartSkill_lock_locklevel");
       if( null == m_label_HeartSkill_lock_locklevel )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_lock_locklevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_lock_describe = fastComponent.FastGetComponent<UILabel>("HeartSkill_lock_describe");
       if( null == m_label_HeartSkill_lock_describe )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_lock_describe 为空，请检查prefab是否缺乏组件");
       }
        m_label_HeartSkill_lock_xiaohao_Num = fastComponent.FastGetComponent<UILabel>("HeartSkill_lock_xiaohao_Num");
       if( null == m_label_HeartSkill_lock_xiaohao_Num )
       {
            Engine.Utility.Log.Error("m_label_HeartSkill_lock_xiaohao_Num 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Btn_LevelUp.gameObject).onClick = _onClick_Btn_LevelUp_Btn;
        UIEventListener.Get(m_btn_Btn_AllLevelUp.gameObject).onClick = _onClick_Btn_AllLevelUp_Btn;
        UIEventListener.Get(m_btn_btnStatus1.gameObject).onClick = _onClick_BtnStatus1_Btn;
        UIEventListener.Get(m_btn_btnStatus2.gameObject).onClick = _onClick_BtnStatus2_Btn;
        UIEventListener.Get(m_btn_btnClearSkillSet.gameObject).onClick = _onClick_BtnClearSkillSet_Btn;
        UIEventListener.Get(m_btn_AutoSkillSet.gameObject).onClick = _onClick_AutoSkillSet_Btn;
        UIEventListener.Get(m_btn_Btn_HeartPointAdd.gameObject).onClick = _onClick_Btn_HeartPointAdd_Btn;
        UIEventListener.Get(m_btn_Btn_Reset.gameObject).onClick = _onClick_Btn_Reset_Btn;
        UIEventListener.Get(m_btn_HeartSkill_btn_shengji.gameObject).onClick = _onClick_HeartSkill_btn_shengji_Btn;
    }

    void _onClick_Btn_LevelUp_Btn(GameObject caster)
    {
        onClick_Btn_LevelUp_Btn( caster );
    }

    void _onClick_Btn_AllLevelUp_Btn(GameObject caster)
    {
        onClick_Btn_AllLevelUp_Btn( caster );
    }

    void _onClick_BtnStatus1_Btn(GameObject caster)
    {
        onClick_BtnStatus1_Btn( caster );
    }

    void _onClick_BtnStatus2_Btn(GameObject caster)
    {
        onClick_BtnStatus2_Btn( caster );
    }

    void _onClick_BtnClearSkillSet_Btn(GameObject caster)
    {
        onClick_BtnClearSkillSet_Btn( caster );
    }

    void _onClick_AutoSkillSet_Btn(GameObject caster)
    {
        onClick_AutoSkillSet_Btn( caster );
    }

    void _onClick_Btn_HeartPointAdd_Btn(GameObject caster)
    {
        onClick_Btn_HeartPointAdd_Btn( caster );
    }

    void _onClick_Btn_Reset_Btn(GameObject caster)
    {
        onClick_Btn_Reset_Btn( caster );
    }

    void _onClick_HeartSkill_btn_shengji_Btn(GameObject caster)
    {
        onClick_HeartSkill_btn_shengji_Btn( caster );
    }


}
