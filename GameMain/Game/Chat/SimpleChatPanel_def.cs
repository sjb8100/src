//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SimpleChatPanel: UIPanelBase
{

    Transform     m_trans_offset;

    UISprite      m_sprite_Background;

    Transform     m_trans_chatItemRoot;

    UIWidget      m_widget_ChatItem;

    Transform     m_trans_PushContent;

    Transform     m_trans_PushRoot;

    UIWidget      m_widget_BtnPush;

    UISprite      m_sprite_btn_chat_shrink;

    UIButton      m_btn_btn_chat_setting;

    UIButton      m_btn_btn_ShowWindow;


    //初始化控件变量
    protected override void InitControls()
    {
        m_trans_offset = GetChildComponent<Transform>("offset");
       if( null == m_trans_offset )
       {
            Engine.Utility.Log.Error("m_trans_offset 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Background = GetChildComponent<UISprite>("Background");
       if( null == m_sprite_Background )
       {
            Engine.Utility.Log.Error("m_sprite_Background 为空，请检查prefab是否缺乏组件");
       }
        m_trans_chatItemRoot = GetChildComponent<Transform>("chatItemRoot");
       if( null == m_trans_chatItemRoot )
       {
            Engine.Utility.Log.Error("m_trans_chatItemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_widget_ChatItem = GetChildComponent<UIWidget>("ChatItem");
       if( null == m_widget_ChatItem )
       {
            Engine.Utility.Log.Error("m_widget_ChatItem 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PushContent = GetChildComponent<Transform>("PushContent");
       if( null == m_trans_PushContent )
       {
            Engine.Utility.Log.Error("m_trans_PushContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PushRoot = GetChildComponent<Transform>("PushRoot");
       if( null == m_trans_PushRoot )
       {
            Engine.Utility.Log.Error("m_trans_PushRoot 为空，请检查prefab是否缺乏组件");
       }
        m_widget_BtnPush = GetChildComponent<UIWidget>("BtnPush");
       if( null == m_widget_BtnPush )
       {
            Engine.Utility.Log.Error("m_widget_BtnPush 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btn_chat_shrink = GetChildComponent<UISprite>("btn_chat_shrink");
       if( null == m_sprite_btn_chat_shrink )
       {
            Engine.Utility.Log.Error("m_sprite_btn_chat_shrink 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_chat_setting = GetChildComponent<UIButton>("btn_chat_setting");
       if( null == m_btn_btn_chat_setting )
       {
            Engine.Utility.Log.Error("m_btn_btn_chat_setting 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_ShowWindow = GetChildComponent<UIButton>("btn_ShowWindow");
       if( null == m_btn_btn_ShowWindow )
       {
            Engine.Utility.Log.Error("m_btn_btn_ShowWindow 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_chat_setting.gameObject).onClick = _onClick_Btn_chat_setting_Btn;
        UIEventListener.Get(m_btn_btn_ShowWindow.gameObject).onClick = _onClick_Btn_ShowWindow_Btn;
    }

    void _onClick_Btn_chat_setting_Btn(GameObject caster)
    {
        onClick_Btn_chat_setting_Btn( caster );
    }

    void _onClick_Btn_ShowWindow_Btn(GameObject caster)
    {
        onClick_Btn_ShowWindow_Btn( caster );
    }


}
