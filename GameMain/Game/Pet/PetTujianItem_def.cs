//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetTujianItem: UIGridBase
{

    Transform            m_trans_bg;

    UILabel              m_label_tujian_name;

    UISprite             m_sprite_shenshou_sign;

    UITexture            m__icon;

    UISprite             m_sprite_pingzhi_box;

    UILabel              m_label_xiedai_level;

    UISlider             m_slider_suipian_slider;

    UILabel              m_label_Percent;

    UISprite             m_sprite_nohave;

    UISprite             m_sprite_useable_mark;


    //初始化控件变量
   protected override void OnAwake()
    {
         InitControls();
         RegisterControlEvents();
    }
    private void InitControls()
    {
        m_trans_bg = GetChildComponent<Transform>("bg");
       if( null == m_trans_bg )
       {
            Engine.Utility.Log.Error("m_trans_bg 为空，请检查prefab是否缺乏组件");
       }
        m_label_tujian_name = GetChildComponent<UILabel>("tujian_name");
       if( null == m_label_tujian_name )
       {
            Engine.Utility.Log.Error("m_label_tujian_name 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_shenshou_sign = GetChildComponent<UISprite>("shenshou_sign");
       if( null == m_sprite_shenshou_sign )
       {
            Engine.Utility.Log.Error("m_sprite_shenshou_sign 为空，请检查prefab是否缺乏组件");
       }
        m__icon = GetChildComponent<UITexture>("icon");
       if( null == m__icon )
       {
            Engine.Utility.Log.Error("m__icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_pingzhi_box = GetChildComponent<UISprite>("pingzhi_box");
       if( null == m_sprite_pingzhi_box )
       {
            Engine.Utility.Log.Error("m_sprite_pingzhi_box 为空，请检查prefab是否缺乏组件");
       }
        m_label_xiedai_level = GetChildComponent<UILabel>("xiedai_level");
       if( null == m_label_xiedai_level )
       {
            Engine.Utility.Log.Error("m_label_xiedai_level 为空，请检查prefab是否缺乏组件");
       }
        m_slider_suipian_slider = GetChildComponent<UISlider>("suipian_slider");
       if( null == m_slider_suipian_slider )
       {
            Engine.Utility.Log.Error("m_slider_suipian_slider 为空，请检查prefab是否缺乏组件");
       }
        m_label_Percent = GetChildComponent<UILabel>("Percent");
       if( null == m_label_Percent )
       {
            Engine.Utility.Log.Error("m_label_Percent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_nohave = GetChildComponent<UISprite>("nohave");
       if( null == m_sprite_nohave )
       {
            Engine.Utility.Log.Error("m_sprite_nohave 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_useable_mark = GetChildComponent<UISprite>("useable_mark");
       if( null == m_sprite_useable_mark )
       {
            Engine.Utility.Log.Error("m_sprite_useable_mark 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    private void RegisterControlEvents()
    {
    }


}
