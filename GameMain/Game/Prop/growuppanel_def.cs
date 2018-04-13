//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class growuppanel: UIPanelBase
{

    UILabel       m_label_Num;

    UILabel       m_label_btn_label;

    UISprite      m_sprite_UIanalysis1grid;

    UISprite      m_sprite_ChooseMark;

    UISprite      m_sprite_arrow;

    UISprite      m_sprite_UIanalysis2grid;

    UISprite      m_sprite_Icon;

    UILabel       m_label_Name_label;

    UILabel       m_label_Desc_label;

    UISlider      m_slider_progressbar;

    UILabel       m_label_Percent;

    UISlider      m_slider_StarSlider;


    //初始化控件变量
    protected override void InitControls()
    {
        m_label_Num = GetChildComponent<UILabel>("Num");
       if( null == m_label_Num )
       {
            Engine.Utility.Log.Error("m_label_Num 为空，请检查prefab是否缺乏组件");
       }
        m_label_btn_label = GetChildComponent<UILabel>("btn_label");
       if( null == m_label_btn_label )
       {
            Engine.Utility.Log.Error("m_label_btn_label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UIanalysis1grid = GetChildComponent<UISprite>("UIanalysis1grid");
       if( null == m_sprite_UIanalysis1grid )
       {
            Engine.Utility.Log.Error("m_sprite_UIanalysis1grid 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_ChooseMark = GetChildComponent<UISprite>("ChooseMark");
       if( null == m_sprite_ChooseMark )
       {
            Engine.Utility.Log.Error("m_sprite_ChooseMark 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_arrow = GetChildComponent<UISprite>("arrow");
       if( null == m_sprite_arrow )
       {
            Engine.Utility.Log.Error("m_sprite_arrow 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UIanalysis2grid = GetChildComponent<UISprite>("UIanalysis2grid");
       if( null == m_sprite_UIanalysis2grid )
       {
            Engine.Utility.Log.Error("m_sprite_UIanalysis2grid 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Icon = GetChildComponent<UISprite>("Icon");
       if( null == m_sprite_Icon )
       {
            Engine.Utility.Log.Error("m_sprite_Icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_Name_label = GetChildComponent<UILabel>("Name_label");
       if( null == m_label_Name_label )
       {
            Engine.Utility.Log.Error("m_label_Name_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_Desc_label = GetChildComponent<UILabel>("Desc_label");
       if( null == m_label_Desc_label )
       {
            Engine.Utility.Log.Error("m_label_Desc_label 为空，请检查prefab是否缺乏组件");
       }
        m_slider_progressbar = GetChildComponent<UISlider>("progressbar");
       if( null == m_slider_progressbar )
       {
            Engine.Utility.Log.Error("m_slider_progressbar 为空，请检查prefab是否缺乏组件");
       }
        m_label_Percent = GetChildComponent<UILabel>("Percent");
       if( null == m_label_Percent )
       {
            Engine.Utility.Log.Error("m_label_Percent 为空，请检查prefab是否缺乏组件");
       }
        m_slider_StarSlider = GetChildComponent<UISlider>("StarSlider");
       if( null == m_slider_StarSlider )
       {
            Engine.Utility.Log.Error("m_slider_StarSlider 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
