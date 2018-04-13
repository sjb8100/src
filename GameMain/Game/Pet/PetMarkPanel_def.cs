//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetMarkPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    UIWidget             m_widget_PetMessage;

    UIWidget             m_widget_SliderContaier;

    UILabel              m_label_liliang;

    UILabel              m_label_minjie;

    UILabel              m_label_zhili;

    UILabel              m_label_tili;

    UILabel              m_label_jingshen;

    UILabel              m_label_xiedaidengji;

    UILabel              m_label_leixing;

    UILabel              m_label_showname;

    UILabel              m_label_getway;

    UISlider             m_slider_suipian_scorllbar;

    UILabel              m_label_score;

    UISprite             m_sprite_btn_huoqu;

    UIButton             m_btn_btn_hecheng;

    UIButton             m_btn_btn_shizhuang;

    UIWidget             m_widget_noclick;

    UIButton             m_btn_btn_right;

    UIButton             m_btn_btn_left;


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
        m_widget_PetMessage = fastComponent.FastGetComponent<UIWidget>("PetMessage");
       if( null == m_widget_PetMessage )
       {
            Engine.Utility.Log.Error("m_widget_PetMessage 为空，请检查prefab是否缺乏组件");
       }
        m_widget_SliderContaier = fastComponent.FastGetComponent<UIWidget>("SliderContaier");
       if( null == m_widget_SliderContaier )
       {
            Engine.Utility.Log.Error("m_widget_SliderContaier 为空，请检查prefab是否缺乏组件");
       }
        m_label_liliang = fastComponent.FastGetComponent<UILabel>("liliang");
       if( null == m_label_liliang )
       {
            Engine.Utility.Log.Error("m_label_liliang 为空，请检查prefab是否缺乏组件");
       }
        m_label_minjie = fastComponent.FastGetComponent<UILabel>("minjie");
       if( null == m_label_minjie )
       {
            Engine.Utility.Log.Error("m_label_minjie 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhili = fastComponent.FastGetComponent<UILabel>("zhili");
       if( null == m_label_zhili )
       {
            Engine.Utility.Log.Error("m_label_zhili 为空，请检查prefab是否缺乏组件");
       }
        m_label_tili = fastComponent.FastGetComponent<UILabel>("tili");
       if( null == m_label_tili )
       {
            Engine.Utility.Log.Error("m_label_tili 为空，请检查prefab是否缺乏组件");
       }
        m_label_jingshen = fastComponent.FastGetComponent<UILabel>("jingshen");
       if( null == m_label_jingshen )
       {
            Engine.Utility.Log.Error("m_label_jingshen 为空，请检查prefab是否缺乏组件");
       }
        m_label_xiedaidengji = fastComponent.FastGetComponent<UILabel>("xiedaidengji");
       if( null == m_label_xiedaidengji )
       {
            Engine.Utility.Log.Error("m_label_xiedaidengji 为空，请检查prefab是否缺乏组件");
       }
        m_label_leixing = fastComponent.FastGetComponent<UILabel>("leixing");
       if( null == m_label_leixing )
       {
            Engine.Utility.Log.Error("m_label_leixing 为空，请检查prefab是否缺乏组件");
       }
        m_label_showname = fastComponent.FastGetComponent<UILabel>("showname");
       if( null == m_label_showname )
       {
            Engine.Utility.Log.Error("m_label_showname 为空，请检查prefab是否缺乏组件");
       }
        m_label_getway = fastComponent.FastGetComponent<UILabel>("getway");
       if( null == m_label_getway )
       {
            Engine.Utility.Log.Error("m_label_getway 为空，请检查prefab是否缺乏组件");
       }
        m_slider_suipian_scorllbar = fastComponent.FastGetComponent<UISlider>("suipian_scorllbar");
       if( null == m_slider_suipian_scorllbar )
       {
            Engine.Utility.Log.Error("m_slider_suipian_scorllbar 为空，请检查prefab是否缺乏组件");
       }
        m_label_score = fastComponent.FastGetComponent<UILabel>("score");
       if( null == m_label_score )
       {
            Engine.Utility.Log.Error("m_label_score 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btn_huoqu = fastComponent.FastGetComponent<UISprite>("btn_huoqu");
       if( null == m_sprite_btn_huoqu )
       {
            Engine.Utility.Log.Error("m_sprite_btn_huoqu 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_hecheng = fastComponent.FastGetComponent<UIButton>("btn_hecheng");
       if( null == m_btn_btn_hecheng )
       {
            Engine.Utility.Log.Error("m_btn_btn_hecheng 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_shizhuang = fastComponent.FastGetComponent<UIButton>("btn_shizhuang");
       if( null == m_btn_btn_shizhuang )
       {
            Engine.Utility.Log.Error("m_btn_btn_shizhuang 为空，请检查prefab是否缺乏组件");
       }
        m_widget_noclick = fastComponent.FastGetComponent<UIWidget>("noclick");
       if( null == m_widget_noclick )
       {
            Engine.Utility.Log.Error("m_widget_noclick 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_right = fastComponent.FastGetComponent<UIButton>("btn_right");
       if( null == m_btn_btn_right )
       {
            Engine.Utility.Log.Error("m_btn_btn_right 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_left = fastComponent.FastGetComponent<UIButton>("btn_left");
       if( null == m_btn_btn_left )
       {
            Engine.Utility.Log.Error("m_btn_btn_left 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_hecheng.gameObject).onClick = _onClick_Btn_hecheng_Btn;
        UIEventListener.Get(m_btn_btn_shizhuang.gameObject).onClick = _onClick_Btn_shizhuang_Btn;
        UIEventListener.Get(m_btn_btn_right.gameObject).onClick = _onClick_Btn_right_Btn;
        UIEventListener.Get(m_btn_btn_left.gameObject).onClick = _onClick_Btn_left_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Btn_hecheng_Btn(GameObject caster)
    {
        onClick_Btn_hecheng_Btn( caster );
    }

    void _onClick_Btn_shizhuang_Btn(GameObject caster)
    {
        onClick_Btn_shizhuang_Btn( caster );
    }

    void _onClick_Btn_right_Btn(GameObject caster)
    {
        onClick_Btn_right_Btn( caster );
    }

    void _onClick_Btn_left_Btn(GameObject caster)
    {
        onClick_Btn_left_Btn( caster );
    }


}
