//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RideAbandonPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    UIButton             m_btn_btn_cancel;

    UIButton             m_btn_btn_diuqi;

    UISprite             m_sprite_icon;

    UILabel              m_label_rarity;

    UILabel              m_label_level;

    UILabel              m_label_speed;

    UISlider             m_slider_satiationscorllbar;

    UILabel              m_label_life;

    UIWidget             m_widget_qiuqi;

    UILabel              m_label_diuqi_Label;

    UIWidget             m_widget_fengyin;

    UILabel              m_label_change_life;

    UILabel              m_label_fengyin_Label;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_cancel = fastComponent.FastGetComponent<UIButton>("btn_cancel");
       if( null == m_btn_btn_cancel )
       {
            Engine.Utility.Log.Error("m_btn_btn_cancel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_diuqi = fastComponent.FastGetComponent<UIButton>("btn_diuqi");
       if( null == m_btn_btn_diuqi )
       {
            Engine.Utility.Log.Error("m_btn_btn_diuqi 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_icon = fastComponent.FastGetComponent<UISprite>("icon");
       if( null == m_sprite_icon )
       {
            Engine.Utility.Log.Error("m_sprite_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_rarity = fastComponent.FastGetComponent<UILabel>("rarity");
       if( null == m_label_rarity )
       {
            Engine.Utility.Log.Error("m_label_rarity 为空，请检查prefab是否缺乏组件");
       }
        m_label_level = fastComponent.FastGetComponent<UILabel>("level");
       if( null == m_label_level )
       {
            Engine.Utility.Log.Error("m_label_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_speed = fastComponent.FastGetComponent<UILabel>("speed");
       if( null == m_label_speed )
       {
            Engine.Utility.Log.Error("m_label_speed 为空，请检查prefab是否缺乏组件");
       }
        m_slider_satiationscorllbar = fastComponent.FastGetComponent<UISlider>("satiationscorllbar");
       if( null == m_slider_satiationscorllbar )
       {
            Engine.Utility.Log.Error("m_slider_satiationscorllbar 为空，请检查prefab是否缺乏组件");
       }
        m_label_life = fastComponent.FastGetComponent<UILabel>("life");
       if( null == m_label_life )
       {
            Engine.Utility.Log.Error("m_label_life 为空，请检查prefab是否缺乏组件");
       }
        m_widget_qiuqi = fastComponent.FastGetComponent<UIWidget>("qiuqi");
       if( null == m_widget_qiuqi )
       {
            Engine.Utility.Log.Error("m_widget_qiuqi 为空，请检查prefab是否缺乏组件");
       }
        m_label_diuqi_Label = fastComponent.FastGetComponent<UILabel>("diuqi_Label");
       if( null == m_label_diuqi_Label )
       {
            Engine.Utility.Log.Error("m_label_diuqi_Label 为空，请检查prefab是否缺乏组件");
       }
        m_widget_fengyin = fastComponent.FastGetComponent<UIWidget>("fengyin");
       if( null == m_widget_fengyin )
       {
            Engine.Utility.Log.Error("m_widget_fengyin 为空，请检查prefab是否缺乏组件");
       }
        m_label_change_life = fastComponent.FastGetComponent<UILabel>("change_life");
       if( null == m_label_change_life )
       {
            Engine.Utility.Log.Error("m_label_change_life 为空，请检查prefab是否缺乏组件");
       }
        m_label_fengyin_Label = fastComponent.FastGetComponent<UILabel>("fengyin_Label");
       if( null == m_label_fengyin_Label )
       {
            Engine.Utility.Log.Error("m_label_fengyin_Label 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_btn_cancel.gameObject).onClick = _onClick_Btn_cancel_Btn;
        UIEventListener.Get(m_btn_btn_diuqi.gameObject).onClick = _onClick_Btn_diuqi_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Btn_cancel_Btn(GameObject caster)
    {
        onClick_Btn_cancel_Btn( caster );
    }

    void _onClick_Btn_diuqi_Btn(GameObject caster)
    {
        onClick_Btn_diuqi_Btn( caster );
    }


}
