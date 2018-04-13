//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TeamTargetPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_Title;

    UIButton             m_btn_btn_close;

    UISprite             m_sprite_Bg_left;

    Transform            m_trans_left_Panel;

    UISprite             m_sprite_Bg_right;

    Transform            m_trans_right_Panel;

    UILabel              m_label_BiaoTi_Label;

    UIWidget             m_widget_ContainerBox;

    UIButton             m_btn_Confirm_btn;

    Transform            m_trans_AutoMatch;

    UIToggle             m_toggle_Match_Box;

    Transform            m_trans_UITeamIndexTargetGrid;

    Transform            m_trans_UITeamMainTargetGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_Title = fastComponent.FastGetComponent<UILabel>("Title");
       if( null == m_label_Title )
       {
            Engine.Utility.Log.Error("m_label_Title 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Bg_left = fastComponent.FastGetComponent<UISprite>("Bg_left");
       if( null == m_sprite_Bg_left )
       {
            Engine.Utility.Log.Error("m_sprite_Bg_left 为空，请检查prefab是否缺乏组件");
       }
        m_trans_left_Panel = fastComponent.FastGetComponent<Transform>("left_Panel");
       if( null == m_trans_left_Panel )
       {
            Engine.Utility.Log.Error("m_trans_left_Panel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Bg_right = fastComponent.FastGetComponent<UISprite>("Bg_right");
       if( null == m_sprite_Bg_right )
       {
            Engine.Utility.Log.Error("m_sprite_Bg_right 为空，请检查prefab是否缺乏组件");
       }
        m_trans_right_Panel = fastComponent.FastGetComponent<Transform>("right_Panel");
       if( null == m_trans_right_Panel )
       {
            Engine.Utility.Log.Error("m_trans_right_Panel 为空，请检查prefab是否缺乏组件");
       }
        m_label_BiaoTi_Label = fastComponent.FastGetComponent<UILabel>("BiaoTi_Label");
       if( null == m_label_BiaoTi_Label )
       {
            Engine.Utility.Log.Error("m_label_BiaoTi_Label 为空，请检查prefab是否缺乏组件");
       }
        m_widget_ContainerBox = fastComponent.FastGetComponent<UIWidget>("ContainerBox");
       if( null == m_widget_ContainerBox )
       {
            Engine.Utility.Log.Error("m_widget_ContainerBox 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Confirm_btn = fastComponent.FastGetComponent<UIButton>("Confirm_btn");
       if( null == m_btn_Confirm_btn )
       {
            Engine.Utility.Log.Error("m_btn_Confirm_btn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AutoMatch = fastComponent.FastGetComponent<Transform>("AutoMatch");
       if( null == m_trans_AutoMatch )
       {
            Engine.Utility.Log.Error("m_trans_AutoMatch 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_Match_Box = fastComponent.FastGetComponent<UIToggle>("Match_Box");
       if( null == m_toggle_Match_Box )
       {
            Engine.Utility.Log.Error("m_toggle_Match_Box 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITeamIndexTargetGrid = fastComponent.FastGetComponent<Transform>("UITeamIndexTargetGrid");
       if( null == m_trans_UITeamIndexTargetGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITeamIndexTargetGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITeamMainTargetGrid = fastComponent.FastGetComponent<Transform>("UITeamMainTargetGrid");
       if( null == m_trans_UITeamMainTargetGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITeamMainTargetGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
        UIEventListener.Get(m_btn_Confirm_btn.gameObject).onClick = _onClick_Confirm_btn_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_Confirm_btn_Btn(GameObject caster)
    {
        onClick_Confirm_btn_Btn( caster );
    }


}
