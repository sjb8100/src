//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ArenaBattlePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_start;

    UILabel              m_label_startTime;

    Transform            m_trans_result;

    UISprite             m_sprite_bg;

    UISprite             m_sprite_win;

    UISprite             m_sprite_defeat;

    UILabel              m_label_Rank_Change;

    UISprite             m_sprite_add;

    UISprite             m_sprite_less;

    UILabel              m_label_integral;

    UILabel              m_label_close_time;

    UIWidget             m_widget_mask;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_start = fastComponent.FastGetComponent<Transform>("start");
       if( null == m_trans_start )
       {
            Engine.Utility.Log.Error("m_trans_start 为空，请检查prefab是否缺乏组件");
       }
        m_label_startTime = fastComponent.FastGetComponent<UILabel>("startTime");
       if( null == m_label_startTime )
       {
            Engine.Utility.Log.Error("m_label_startTime 为空，请检查prefab是否缺乏组件");
       }
        m_trans_result = fastComponent.FastGetComponent<Transform>("result");
       if( null == m_trans_result )
       {
            Engine.Utility.Log.Error("m_trans_result 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_bg = fastComponent.FastGetComponent<UISprite>("bg");
       if( null == m_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_win = fastComponent.FastGetComponent<UISprite>("win");
       if( null == m_sprite_win )
       {
            Engine.Utility.Log.Error("m_sprite_win 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_defeat = fastComponent.FastGetComponent<UISprite>("defeat");
       if( null == m_sprite_defeat )
       {
            Engine.Utility.Log.Error("m_sprite_defeat 为空，请检查prefab是否缺乏组件");
       }
        m_label_Rank_Change = fastComponent.FastGetComponent<UILabel>("Rank_Change");
       if( null == m_label_Rank_Change )
       {
            Engine.Utility.Log.Error("m_label_Rank_Change 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_add = fastComponent.FastGetComponent<UISprite>("add");
       if( null == m_sprite_add )
       {
            Engine.Utility.Log.Error("m_sprite_add 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_less = fastComponent.FastGetComponent<UISprite>("less");
       if( null == m_sprite_less )
       {
            Engine.Utility.Log.Error("m_sprite_less 为空，请检查prefab是否缺乏组件");
       }
        m_label_integral = fastComponent.FastGetComponent<UILabel>("integral");
       if( null == m_label_integral )
       {
            Engine.Utility.Log.Error("m_label_integral 为空，请检查prefab是否缺乏组件");
       }
        m_label_close_time = fastComponent.FastGetComponent<UILabel>("close_time");
       if( null == m_label_close_time )
       {
            Engine.Utility.Log.Error("m_label_close_time 为空，请检查prefab是否缺乏组件");
       }
        m_widget_mask = fastComponent.FastGetComponent<UIWidget>("mask");
       if( null == m_widget_mask )
       {
            Engine.Utility.Log.Error("m_widget_mask 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
