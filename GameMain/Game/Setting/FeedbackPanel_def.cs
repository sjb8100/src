//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FeedbackPanel: UIPanelBase
{

    Transform            m_trans_FeedbackPanel;

    Transform            m_trans_note;

    UIInput              m_input_Account;

    UIInput              m_input_Phone;

    UIInput              m_input_Suggestion;

    UIToggle             m_toggle_wenjuan;

    UIToggle             m_toggle_quexian;

    UIToggle             m_toggle_jianyi;

    UIToggle             m_toggle_chongzhi;

    UIToggle             m_toggle_qita;

    UIInput              m_input_feedbackText;

    UIButton             m_btn_confirmBtn;

    Transform            m_trans_chat;

    UIScrollView         m_scrollview_ChatScrollView;

    Transform            m_trans_ChatItemRoot;

    UIButton             m_btn_noteBtn;

    UIButton             m_btn_chatBtn;

    UISprite             m_sprite_feedBackWarning;

    UIButton             m_btn_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
        m_trans_FeedbackPanel = GetChildComponent<Transform>("FeedbackPanel");
       if( null == m_trans_FeedbackPanel )
       {
            Engine.Utility.Log.Error("m_trans_FeedbackPanel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_note = GetChildComponent<Transform>("note");
       if( null == m_trans_note )
       {
            Engine.Utility.Log.Error("m_trans_note 为空，请检查prefab是否缺乏组件");
       }
        m_input_Account = GetChildComponent<UIInput>("Account");
       if( null == m_input_Account )
       {
            Engine.Utility.Log.Error("m_input_Account 为空，请检查prefab是否缺乏组件");
       }
        m_input_Phone = GetChildComponent<UIInput>("Phone");
       if( null == m_input_Phone )
       {
            Engine.Utility.Log.Error("m_input_Phone 为空，请检查prefab是否缺乏组件");
       }
        m_input_Suggestion = GetChildComponent<UIInput>("Suggestion");
       if( null == m_input_Suggestion )
       {
            Engine.Utility.Log.Error("m_input_Suggestion 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_wenjuan = GetChildComponent<UIToggle>("wenjuan");
       if( null == m_toggle_wenjuan )
       {
            Engine.Utility.Log.Error("m_toggle_wenjuan 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_quexian = GetChildComponent<UIToggle>("quexian");
       if( null == m_toggle_quexian )
       {
            Engine.Utility.Log.Error("m_toggle_quexian 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_jianyi = GetChildComponent<UIToggle>("jianyi");
       if( null == m_toggle_jianyi )
       {
            Engine.Utility.Log.Error("m_toggle_jianyi 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_chongzhi = GetChildComponent<UIToggle>("chongzhi");
       if( null == m_toggle_chongzhi )
       {
            Engine.Utility.Log.Error("m_toggle_chongzhi 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_qita = GetChildComponent<UIToggle>("qita");
       if( null == m_toggle_qita )
       {
            Engine.Utility.Log.Error("m_toggle_qita 为空，请检查prefab是否缺乏组件");
       }
        m_input_feedbackText = GetChildComponent<UIInput>("feedbackText");
       if( null == m_input_feedbackText )
       {
            Engine.Utility.Log.Error("m_input_feedbackText 为空，请检查prefab是否缺乏组件");
       }
        m_btn_confirmBtn = GetChildComponent<UIButton>("confirmBtn");
       if( null == m_btn_confirmBtn )
       {
            Engine.Utility.Log.Error("m_btn_confirmBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_chat = GetChildComponent<Transform>("chat");
       if( null == m_trans_chat )
       {
            Engine.Utility.Log.Error("m_trans_chat 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_ChatScrollView = GetChildComponent<UIScrollView>("ChatScrollView");
       if( null == m_scrollview_ChatScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_ChatScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ChatItemRoot = GetChildComponent<Transform>("ChatItemRoot");
       if( null == m_trans_ChatItemRoot )
       {
            Engine.Utility.Log.Error("m_trans_ChatItemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_noteBtn = GetChildComponent<UIButton>("noteBtn");
       if( null == m_btn_noteBtn )
       {
            Engine.Utility.Log.Error("m_btn_noteBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_chatBtn = GetChildComponent<UIButton>("chatBtn");
       if( null == m_btn_chatBtn )
       {
            Engine.Utility.Log.Error("m_btn_chatBtn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_feedBackWarning = GetChildComponent<UISprite>("feedBackWarning");
       if( null == m_sprite_feedBackWarning )
       {
            Engine.Utility.Log.Error("m_sprite_feedBackWarning 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = GetChildComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_confirmBtn.gameObject).onClick = _onClick_ConfirmBtn_Btn;
        UIEventListener.Get(m_btn_noteBtn.gameObject).onClick = _onClick_NoteBtn_Btn;
        UIEventListener.Get(m_btn_chatBtn.gameObject).onClick = _onClick_ChatBtn_Btn;
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
    }

    void _onClick_ConfirmBtn_Btn(GameObject caster)
    {
        onClick_ConfirmBtn_Btn( caster );
    }

    void _onClick_NoteBtn_Btn(GameObject caster)
    {
        onClick_NoteBtn_Btn( caster );
    }

    void _onClick_ChatBtn_Btn(GameObject caster)
    {
        onClick_ChatBtn_Btn( caster );
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }


}
