//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FBCard: UIGridBase
{

    UIWidget             m_widget_Panel;

    UIWidget             m_widget_childContent;

    UILabel              m_label_reward_word;

    Transform            m_trans_iconContent;

    Transform            m_trans_WardScrollView;

    Transform            m_trans_WardItem;

    UIButton             m_btn_btn_enter;

    UILabel              m_label_name;

    UILabel              m_label_description;

    UILabel              m_label_frequency;

    UILabel              m_label_MemberLimit;

    UIButton             m_btn_btn_Two;

    UIButton             m_btn_btn_CityWarDes;

    Transform            m_trans_enterinfo;

    UISprite             m_sprite_itembg;

    UILabel              m_label_enteritemname;

    UILabel              m_label_enterneedcount;

    Transform            m_trans_lingpai;

    UITexture            m__bgtexture;

    UILabel              m_label_FB_name;

    UISprite             m_sprite_FB_icon;

    UILabel              m_label_lock_level;

    UILabel              m_label_Label;


    //初始化控件变量
   protected override void OnAwake()
    {
         InitControls();
         RegisterControlEvents();
    }
    private void InitControls()
    {
        m_widget_Panel = GetChildComponent<UIWidget>("Panel");
       if( null == m_widget_Panel )
       {
            Engine.Utility.Log.Error("m_widget_Panel 为空，请检查prefab是否缺乏组件");
       }
        m_widget_childContent = GetChildComponent<UIWidget>("childContent");
       if( null == m_widget_childContent )
       {
            Engine.Utility.Log.Error("m_widget_childContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_reward_word = GetChildComponent<UILabel>("reward_word");
       if( null == m_label_reward_word )
       {
            Engine.Utility.Log.Error("m_label_reward_word 为空，请检查prefab是否缺乏组件");
       }
        m_trans_iconContent = GetChildComponent<Transform>("iconContent");
       if( null == m_trans_iconContent )
       {
            Engine.Utility.Log.Error("m_trans_iconContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WardScrollView = GetChildComponent<Transform>("WardScrollView");
       if( null == m_trans_WardScrollView )
       {
            Engine.Utility.Log.Error("m_trans_WardScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WardItem = GetChildComponent<Transform>("WardItem");
       if( null == m_trans_WardItem )
       {
            Engine.Utility.Log.Error("m_trans_WardItem 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_enter = GetChildComponent<UIButton>("btn_enter");
       if( null == m_btn_btn_enter )
       {
            Engine.Utility.Log.Error("m_btn_btn_enter 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = GetChildComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_description = GetChildComponent<UILabel>("description");
       if( null == m_label_description )
       {
            Engine.Utility.Log.Error("m_label_description 为空，请检查prefab是否缺乏组件");
       }
        m_label_frequency = GetChildComponent<UILabel>("frequency");
       if( null == m_label_frequency )
       {
            Engine.Utility.Log.Error("m_label_frequency 为空，请检查prefab是否缺乏组件");
       }
        m_label_MemberLimit = GetChildComponent<UILabel>("MemberLimit");
       if( null == m_label_MemberLimit )
       {
            Engine.Utility.Log.Error("m_label_MemberLimit 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Two = GetChildComponent<UIButton>("btn_Two");
       if( null == m_btn_btn_Two )
       {
            Engine.Utility.Log.Error("m_btn_btn_Two 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_CityWarDes = GetChildComponent<UIButton>("btn_CityWarDes");
       if( null == m_btn_btn_CityWarDes )
       {
            Engine.Utility.Log.Error("m_btn_btn_CityWarDes 为空，请检查prefab是否缺乏组件");
       }
        m_trans_enterinfo = GetChildComponent<Transform>("enterinfo");
       if( null == m_trans_enterinfo )
       {
            Engine.Utility.Log.Error("m_trans_enterinfo 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_itembg = GetChildComponent<UISprite>("itembg");
       if( null == m_sprite_itembg )
       {
            Engine.Utility.Log.Error("m_sprite_itembg 为空，请检查prefab是否缺乏组件");
       }
        m_label_enteritemname = GetChildComponent<UILabel>("enteritemname");
       if( null == m_label_enteritemname )
       {
            Engine.Utility.Log.Error("m_label_enteritemname 为空，请检查prefab是否缺乏组件");
       }
        m_label_enterneedcount = GetChildComponent<UILabel>("enterneedcount");
       if( null == m_label_enterneedcount )
       {
            Engine.Utility.Log.Error("m_label_enterneedcount 为空，请检查prefab是否缺乏组件");
       }
        m_trans_lingpai = GetChildComponent<Transform>("lingpai");
       if( null == m_trans_lingpai )
       {
            Engine.Utility.Log.Error("m_trans_lingpai 为空，请检查prefab是否缺乏组件");
       }
        m__bgtexture = GetChildComponent<UITexture>("bgtexture");
       if( null == m__bgtexture )
       {
            Engine.Utility.Log.Error("m__bgtexture 为空，请检查prefab是否缺乏组件");
       }
        m_label_FB_name = GetChildComponent<UILabel>("FB_name");
       if( null == m_label_FB_name )
       {
            Engine.Utility.Log.Error("m_label_FB_name 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_FB_icon = GetChildComponent<UISprite>("FB_icon");
       if( null == m_sprite_FB_icon )
       {
            Engine.Utility.Log.Error("m_sprite_FB_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_lock_level = GetChildComponent<UILabel>("lock_level");
       if( null == m_label_lock_level )
       {
            Engine.Utility.Log.Error("m_label_lock_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = GetChildComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    private void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_enter.gameObject).onClick = _onClick_Btn_enter_Btn;
        UIEventListener.Get(m_btn_btn_Two.gameObject).onClick = _onClick_Btn_Two_Btn;
        UIEventListener.Get(m_btn_btn_CityWarDes.gameObject).onClick = _onClick_Btn_CityWarDes_Btn;
    }

    void _onClick_Btn_enter_Btn(GameObject caster)
    {
        onClick_Btn_enter_Btn( caster );
    }

    void _onClick_Btn_Two_Btn(GameObject caster)
    {
        onClick_Btn_Two_Btn( caster );
    }

    void _onClick_Btn_CityWarDes_Btn(GameObject caster)
    {
        onClick_Btn_CityWarDes_Btn( caster );
    }


}
