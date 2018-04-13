//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FriendPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		btn_search,
		Max,
    }

    public enum TabMode{
		None = 0,
		//最近
		ZuiJin = 1,
		//好友
		HaoYou = 2,
		//添加
		TianJia = 3,
		//邮箱
		YouXiang = 4,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_FriendContent;

    Transform            m_trans_SecondTab;

    Transform            m_trans_addContent;

    Transform            m_trans_list;

    UIButton             m_btn_btn_clear;

    UIGridCreatorBase    m_ctor_FriendScrollview;

    UILabel              m_label_labletips;

    Transform            m_trans_timeTips;

    Transform            m_trans_bg_search;

    UIButton             m_btn_btn_search;

    UIInput              m_input_Input;

    UISprite             m_sprite_bg_searchresults;

    Transform            m_trans_chatroot;

    Transform            m_trans_PanelInput;

    Transform            m_trans_voiceMicTips;

    Transform            m_trans_inputvoice;

    UIButton             m_btn_btnVoicetoteext;

    UIButton             m_btn_voice_input;

    Transform            m_trans_textinput;

    UIButton             m_btn_btnVoice;

    UIButton             m_btn_btnEmoji;

    UIButton             m_btn_BtnSend;

    UIButton             m_btn_btnMessage;

    UIInput              m_input_InputMsg;

    Transform            m_trans_MailContent;

    UIButton             m_btn_one_btn_get;

    UISprite             m_sprite_dividing_line_up;

    UILabel              m_label_mail_num;

    UIGridCreatorBase    m_ctor_MailScroll;

    Transform            m_trans_mail_text_area;

    UILabel              m_label_mail_text_name;

    UILabel              m_label_zhuti_text;

    UILabel              m_label_mail_text;

    UISprite             m_sprite_dividing_line_bottom;

    Transform            m_trans_mail_text_scroll;

    UISprite             m_sprite_arrow_right;

    UISprite             m_sprite_arrow_left;

    UIButton             m_btn_lingqu_btn;

    UILabel              m_label_recieve_time;

    UILabel              m_label_fujian_label;

    Transform            m_trans_background_text_area;

    UILabel              m_label_mark_message;

    UIButton             m_btn_DeleteBtn;

    Transform            m_trans_RightTabRoot;

    Transform            m_trans_UIMailGrid;

    UISprite             m_sprite_red_dot;

    UILabel              m_label_left_time;

    UISprite             m_sprite_mail_icon;

    Transform            m_trans_UIChatItemGrid;

    UILabel              m_label_envelopeType;

    Transform            m_trans_UIFriendGrid;

    UILabel              m_label_online;

    Transform            m_trans_UIItemRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_FriendContent = fastComponent.FastGetComponent<Transform>("FriendContent");
       if( null == m_trans_FriendContent )
       {
            Engine.Utility.Log.Error("m_trans_FriendContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SecondTab = fastComponent.FastGetComponent<Transform>("SecondTab");
       if( null == m_trans_SecondTab )
       {
            Engine.Utility.Log.Error("m_trans_SecondTab 为空，请检查prefab是否缺乏组件");
       }
        m_trans_addContent = fastComponent.FastGetComponent<Transform>("addContent");
       if( null == m_trans_addContent )
       {
            Engine.Utility.Log.Error("m_trans_addContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_list = fastComponent.FastGetComponent<Transform>("list");
       if( null == m_trans_list )
       {
            Engine.Utility.Log.Error("m_trans_list 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_clear = fastComponent.FastGetComponent<UIButton>("btn_clear");
       if( null == m_btn_btn_clear )
       {
            Engine.Utility.Log.Error("m_btn_btn_clear 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_FriendScrollview = fastComponent.FastGetComponent<UIGridCreatorBase>("FriendScrollview");
       if( null == m_ctor_FriendScrollview )
       {
            Engine.Utility.Log.Error("m_ctor_FriendScrollview 为空，请检查prefab是否缺乏组件");
       }
        m_label_labletips = fastComponent.FastGetComponent<UILabel>("labletips");
       if( null == m_label_labletips )
       {
            Engine.Utility.Log.Error("m_label_labletips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_timeTips = fastComponent.FastGetComponent<Transform>("timeTips");
       if( null == m_trans_timeTips )
       {
            Engine.Utility.Log.Error("m_trans_timeTips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_bg_search = fastComponent.FastGetComponent<Transform>("bg_search");
       if( null == m_trans_bg_search )
       {
            Engine.Utility.Log.Error("m_trans_bg_search 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_search = fastComponent.FastGetComponent<UIButton>("btn_search");
       if( null == m_btn_btn_search )
       {
            Engine.Utility.Log.Error("m_btn_btn_search 为空，请检查prefab是否缺乏组件");
       }
        m_input_Input = fastComponent.FastGetComponent<UIInput>("Input");
       if( null == m_input_Input )
       {
            Engine.Utility.Log.Error("m_input_Input 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_bg_searchresults = fastComponent.FastGetComponent<UISprite>("bg_searchresults");
       if( null == m_sprite_bg_searchresults )
       {
            Engine.Utility.Log.Error("m_sprite_bg_searchresults 为空，请检查prefab是否缺乏组件");
       }
        m_trans_chatroot = fastComponent.FastGetComponent<Transform>("chatroot");
       if( null == m_trans_chatroot )
       {
            Engine.Utility.Log.Error("m_trans_chatroot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PanelInput = fastComponent.FastGetComponent<Transform>("PanelInput");
       if( null == m_trans_PanelInput )
       {
            Engine.Utility.Log.Error("m_trans_PanelInput 为空，请检查prefab是否缺乏组件");
       }
        m_trans_voiceMicTips = fastComponent.FastGetComponent<Transform>("voiceMicTips");
       if( null == m_trans_voiceMicTips )
       {
            Engine.Utility.Log.Error("m_trans_voiceMicTips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_inputvoice = fastComponent.FastGetComponent<Transform>("inputvoice");
       if( null == m_trans_inputvoice )
       {
            Engine.Utility.Log.Error("m_trans_inputvoice 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnVoicetoteext = fastComponent.FastGetComponent<UIButton>("btnVoicetoteext");
       if( null == m_btn_btnVoicetoteext )
       {
            Engine.Utility.Log.Error("m_btn_btnVoicetoteext 为空，请检查prefab是否缺乏组件");
       }
        m_btn_voice_input = fastComponent.FastGetComponent<UIButton>("voice_input");
       if( null == m_btn_voice_input )
       {
            Engine.Utility.Log.Error("m_btn_voice_input 为空，请检查prefab是否缺乏组件");
       }
        m_trans_textinput = fastComponent.FastGetComponent<Transform>("textinput");
       if( null == m_trans_textinput )
       {
            Engine.Utility.Log.Error("m_trans_textinput 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnVoice = fastComponent.FastGetComponent<UIButton>("btnVoice");
       if( null == m_btn_btnVoice )
       {
            Engine.Utility.Log.Error("m_btn_btnVoice 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnEmoji = fastComponent.FastGetComponent<UIButton>("btnEmoji");
       if( null == m_btn_btnEmoji )
       {
            Engine.Utility.Log.Error("m_btn_btnEmoji 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSend = fastComponent.FastGetComponent<UIButton>("BtnSend");
       if( null == m_btn_BtnSend )
       {
            Engine.Utility.Log.Error("m_btn_BtnSend 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnMessage = fastComponent.FastGetComponent<UIButton>("btnMessage");
       if( null == m_btn_btnMessage )
       {
            Engine.Utility.Log.Error("m_btn_btnMessage 为空，请检查prefab是否缺乏组件");
       }
        m_input_InputMsg = fastComponent.FastGetComponent<UIInput>("InputMsg");
       if( null == m_input_InputMsg )
       {
            Engine.Utility.Log.Error("m_input_InputMsg 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MailContent = fastComponent.FastGetComponent<Transform>("MailContent");
       if( null == m_trans_MailContent )
       {
            Engine.Utility.Log.Error("m_trans_MailContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_one_btn_get = fastComponent.FastGetComponent<UIButton>("one_btn_get");
       if( null == m_btn_one_btn_get )
       {
            Engine.Utility.Log.Error("m_btn_one_btn_get 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_dividing_line_up = fastComponent.FastGetComponent<UISprite>("dividing_line_up");
       if( null == m_sprite_dividing_line_up )
       {
            Engine.Utility.Log.Error("m_sprite_dividing_line_up 为空，请检查prefab是否缺乏组件");
       }
        m_label_mail_num = fastComponent.FastGetComponent<UILabel>("mail_num");
       if( null == m_label_mail_num )
       {
            Engine.Utility.Log.Error("m_label_mail_num 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_MailScroll = fastComponent.FastGetComponent<UIGridCreatorBase>("MailScroll");
       if( null == m_ctor_MailScroll )
       {
            Engine.Utility.Log.Error("m_ctor_MailScroll 为空，请检查prefab是否缺乏组件");
       }
        m_trans_mail_text_area = fastComponent.FastGetComponent<Transform>("mail_text_area");
       if( null == m_trans_mail_text_area )
       {
            Engine.Utility.Log.Error("m_trans_mail_text_area 为空，请检查prefab是否缺乏组件");
       }
        m_label_mail_text_name = fastComponent.FastGetComponent<UILabel>("mail_text_name");
       if( null == m_label_mail_text_name )
       {
            Engine.Utility.Log.Error("m_label_mail_text_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhuti_text = fastComponent.FastGetComponent<UILabel>("zhuti_text");
       if( null == m_label_zhuti_text )
       {
            Engine.Utility.Log.Error("m_label_zhuti_text 为空，请检查prefab是否缺乏组件");
       }
        m_label_mail_text = fastComponent.FastGetComponent<UILabel>("mail_text");
       if( null == m_label_mail_text )
       {
            Engine.Utility.Log.Error("m_label_mail_text 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_dividing_line_bottom = fastComponent.FastGetComponent<UISprite>("dividing_line_bottom");
       if( null == m_sprite_dividing_line_bottom )
       {
            Engine.Utility.Log.Error("m_sprite_dividing_line_bottom 为空，请检查prefab是否缺乏组件");
       }
        m_trans_mail_text_scroll = fastComponent.FastGetComponent<Transform>("mail_text_scroll");
       if( null == m_trans_mail_text_scroll )
       {
            Engine.Utility.Log.Error("m_trans_mail_text_scroll 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_arrow_right = fastComponent.FastGetComponent<UISprite>("arrow_right");
       if( null == m_sprite_arrow_right )
       {
            Engine.Utility.Log.Error("m_sprite_arrow_right 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_arrow_left = fastComponent.FastGetComponent<UISprite>("arrow_left");
       if( null == m_sprite_arrow_left )
       {
            Engine.Utility.Log.Error("m_sprite_arrow_left 为空，请检查prefab是否缺乏组件");
       }
        m_btn_lingqu_btn = fastComponent.FastGetComponent<UIButton>("lingqu_btn");
       if( null == m_btn_lingqu_btn )
       {
            Engine.Utility.Log.Error("m_btn_lingqu_btn 为空，请检查prefab是否缺乏组件");
       }
        m_label_recieve_time = fastComponent.FastGetComponent<UILabel>("recieve_time");
       if( null == m_label_recieve_time )
       {
            Engine.Utility.Log.Error("m_label_recieve_time 为空，请检查prefab是否缺乏组件");
       }
        m_label_fujian_label = fastComponent.FastGetComponent<UILabel>("fujian_label");
       if( null == m_label_fujian_label )
       {
            Engine.Utility.Log.Error("m_label_fujian_label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_background_text_area = fastComponent.FastGetComponent<Transform>("background_text_area");
       if( null == m_trans_background_text_area )
       {
            Engine.Utility.Log.Error("m_trans_background_text_area 为空，请检查prefab是否缺乏组件");
       }
        m_label_mark_message = fastComponent.FastGetComponent<UILabel>("mark_message");
       if( null == m_label_mark_message )
       {
            Engine.Utility.Log.Error("m_label_mark_message 为空，请检查prefab是否缺乏组件");
       }
        m_btn_DeleteBtn = fastComponent.FastGetComponent<UIButton>("DeleteBtn");
       if( null == m_btn_DeleteBtn )
       {
            Engine.Utility.Log.Error("m_btn_DeleteBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RightTabRoot = fastComponent.FastGetComponent<Transform>("RightTabRoot");
       if( null == m_trans_RightTabRoot )
       {
            Engine.Utility.Log.Error("m_trans_RightTabRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIMailGrid = fastComponent.FastGetComponent<Transform>("UIMailGrid");
       if( null == m_trans_UIMailGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIMailGrid 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_red_dot = fastComponent.FastGetComponent<UISprite>("red_dot");
       if( null == m_sprite_red_dot )
       {
            Engine.Utility.Log.Error("m_sprite_red_dot 为空，请检查prefab是否缺乏组件");
       }
        m_label_left_time = fastComponent.FastGetComponent<UILabel>("left_time");
       if( null == m_label_left_time )
       {
            Engine.Utility.Log.Error("m_label_left_time 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_mail_icon = fastComponent.FastGetComponent<UISprite>("mail_icon");
       if( null == m_sprite_mail_icon )
       {
            Engine.Utility.Log.Error("m_sprite_mail_icon 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIChatItemGrid = fastComponent.FastGetComponent<Transform>("UIChatItemGrid");
       if( null == m_trans_UIChatItemGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIChatItemGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_envelopeType = fastComponent.FastGetComponent<UILabel>("envelopeType");
       if( null == m_label_envelopeType )
       {
            Engine.Utility.Log.Error("m_label_envelopeType 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIFriendGrid = fastComponent.FastGetComponent<Transform>("UIFriendGrid");
       if( null == m_trans_UIFriendGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIFriendGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_online = fastComponent.FastGetComponent<UILabel>("online");
       if( null == m_label_online )
       {
            Engine.Utility.Log.Error("m_label_online 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_clear.gameObject).onClick = _onClick_Btn_clear_Btn;
        UIEventListener.Get(m_btn_btn_search.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_btnVoicetoteext.gameObject).onClick = _onClick_BtnVoicetoteext_Btn;
        UIEventListener.Get(m_btn_voice_input.gameObject).onClick = _onClick_Voice_input_Btn;
        UIEventListener.Get(m_btn_btnVoice.gameObject).onClick = _onClick_BtnVoice_Btn;
        UIEventListener.Get(m_btn_btnEmoji.gameObject).onClick = _onClick_BtnEmoji_Btn;
        UIEventListener.Get(m_btn_BtnSend.gameObject).onClick = _onClick_BtnSend_Btn;
        UIEventListener.Get(m_btn_btnMessage.gameObject).onClick = _onClick_BtnMessage_Btn;
        UIEventListener.Get(m_btn_one_btn_get.gameObject).onClick = _onClick_One_btn_get_Btn;
        UIEventListener.Get(m_btn_lingqu_btn.gameObject).onClick = _onClick_Lingqu_btn_Btn;
        UIEventListener.Get(m_btn_DeleteBtn.gameObject).onClick = _onClick_DeleteBtn_Btn;
    }

    void _onClick_Btn_clear_Btn(GameObject caster)
    {
        onClick_Btn_clear_Btn( caster );
    }

    void _OnBtnsClick(GameObject caster)
    {
        BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), caster.name);
        OnBtnsClick( btntype );
    }

    void _onClick_BtnVoicetoteext_Btn(GameObject caster)
    {
        onClick_BtnVoicetoteext_Btn( caster );
    }

    void _onClick_Voice_input_Btn(GameObject caster)
    {
        onClick_Voice_input_Btn( caster );
    }

    void _onClick_BtnVoice_Btn(GameObject caster)
    {
        onClick_BtnVoice_Btn( caster );
    }

    void _onClick_BtnEmoji_Btn(GameObject caster)
    {
        onClick_BtnEmoji_Btn( caster );
    }

    void _onClick_BtnSend_Btn(GameObject caster)
    {
        onClick_BtnSend_Btn( caster );
    }

    void _onClick_BtnMessage_Btn(GameObject caster)
    {
        onClick_BtnMessage_Btn( caster );
    }

    void _onClick_One_btn_get_Btn(GameObject caster)
    {
        onClick_One_btn_get_Btn( caster );
    }

    void _onClick_Lingqu_btn_Btn(GameObject caster)
    {
        onClick_Lingqu_btn_Btn( caster );
    }

    void _onClick_DeleteBtn_Btn(GameObject caster)
    {
        onClick_DeleteBtn_Btn( caster );
    }


}
