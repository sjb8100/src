//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class UseItemCommonPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_title;

    UISlider             m_slider_Slider;

    UISprite             m_sprite_Foreground;

    UILabel              m_label_process;

    Transform            m_trans_ItemScrollView;

    UIButton             m_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_title = fastComponent.FastGetComponent<UILabel>("title");
       if( null == m_label_title )
       {
            Engine.Utility.Log.Error("m_label_title 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Slider = fastComponent.FastGetComponent<UISlider>("Slider");
       if( null == m_slider_Slider )
       {
            Engine.Utility.Log.Error("m_slider_Slider 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Foreground = fastComponent.FastGetComponent<UISprite>("Foreground");
       if( null == m_sprite_Foreground )
       {
            Engine.Utility.Log.Error("m_sprite_Foreground 为空，请检查prefab是否缺乏组件");
       }
        m_label_process = fastComponent.FastGetComponent<UILabel>("process");
       if( null == m_label_process )
       {
            Engine.Utility.Log.Error("m_label_process 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemScrollView = fastComponent.FastGetComponent<Transform>("ItemScrollView");
       if( null == m_trans_ItemScrollView )
       {
            Engine.Utility.Log.Error("m_trans_ItemScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
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
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
