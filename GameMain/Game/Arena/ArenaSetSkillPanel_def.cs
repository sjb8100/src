//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ArenaSetSkillPanel: UIPanelBase
{

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
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btnStatus1.gameObject).onClick = _onClick_BtnStatus1_Btn;
        UIEventListener.Get(m_btn_btnStatus2.gameObject).onClick = _onClick_BtnStatus2_Btn;
        UIEventListener.Get(m_btn_btnClearSkillSet.gameObject).onClick = _onClick_BtnClearSkillSet_Btn;
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


}
