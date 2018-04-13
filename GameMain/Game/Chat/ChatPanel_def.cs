//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ChatPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		TabWorld,
		TabTeam,
		TabClan,
		TabNear,
		TabGhost,
		TabSystem,
		TabRed,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_OffsetContent;

    UISprite             m_sprite_Spriteout;

    UISprite             m_sprite_chatBg_di;

    Transform            m_trans_newmessage_warning;

    UIScrollView         m_scrollview_ChatScrollView;

    Transform            m_trans_ChatItemRoot;

    Transform            m_trans_NoClan;

    UIButton             m_btn_btn_joinClan;

    UILabel              m_label_bottomTips;

    Transform            m_trans_ChanelTabs;

    UIButton             m_btn_TabWorld;

    UIButton             m_btn_TabTeam;

    UIButton             m_btn_TabClan;

    UIButton             m_btn_TabNear;

    UIButton             m_btn_TabGhost;

    UIButton             m_btn_TabSystem;

    UIButton             m_btn_TabRed;

    UIButton             m_btn_btnback;

    UIButton             m_btn_hornBtn;

    UIButton             m_btn_settingBtn;

    Transform            m_trans_inputvoice;

    UIButton             m_btn_btnVoicetoteext;

    UIButton             m_btn_voice_input;

    Transform            m_trans_inputnormal;

    UIButton             m_btn_btnVoice;

    UIButton             m_btn_btnMessage;

    UIButton             m_btn_BtnSend;

    UIButton             m_btn_btnEmoji;

    UIInput              m_input_Input;

    Transform            m_trans_voiceMicTips;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_OffsetContent = fastComponent.FastGetComponent<Transform>("OffsetContent");
       if( null == m_trans_OffsetContent )
       {
            Engine.Utility.Log.Error("m_trans_OffsetContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Spriteout = fastComponent.FastGetComponent<UISprite>("Spriteout");
       if( null == m_sprite_Spriteout )
       {
            Engine.Utility.Log.Error("m_sprite_Spriteout 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_chatBg_di = fastComponent.FastGetComponent<UISprite>("chatBg_di");
       if( null == m_sprite_chatBg_di )
       {
            Engine.Utility.Log.Error("m_sprite_chatBg_di 为空，请检查prefab是否缺乏组件");
       }
        m_trans_newmessage_warning = fastComponent.FastGetComponent<Transform>("newmessage_warning");
       if( null == m_trans_newmessage_warning )
       {
            Engine.Utility.Log.Error("m_trans_newmessage_warning 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_ChatScrollView = fastComponent.FastGetComponent<UIScrollView>("ChatScrollView");
       if( null == m_scrollview_ChatScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_ChatScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ChatItemRoot = fastComponent.FastGetComponent<Transform>("ChatItemRoot");
       if( null == m_trans_ChatItemRoot )
       {
            Engine.Utility.Log.Error("m_trans_ChatItemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NoClan = fastComponent.FastGetComponent<Transform>("NoClan");
       if( null == m_trans_NoClan )
       {
            Engine.Utility.Log.Error("m_trans_NoClan 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_joinClan = fastComponent.FastGetComponent<UIButton>("btn_joinClan");
       if( null == m_btn_btn_joinClan )
       {
            Engine.Utility.Log.Error("m_btn_btn_joinClan 为空，请检查prefab是否缺乏组件");
       }
        m_label_bottomTips = fastComponent.FastGetComponent<UILabel>("bottomTips");
       if( null == m_label_bottomTips )
       {
            Engine.Utility.Log.Error("m_label_bottomTips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ChanelTabs = fastComponent.FastGetComponent<Transform>("ChanelTabs");
       if( null == m_trans_ChanelTabs )
       {
            Engine.Utility.Log.Error("m_trans_ChanelTabs 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabWorld = fastComponent.FastGetComponent<UIButton>("TabWorld");
       if( null == m_btn_TabWorld )
       {
            Engine.Utility.Log.Error("m_btn_TabWorld 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabTeam = fastComponent.FastGetComponent<UIButton>("TabTeam");
       if( null == m_btn_TabTeam )
       {
            Engine.Utility.Log.Error("m_btn_TabTeam 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabClan = fastComponent.FastGetComponent<UIButton>("TabClan");
       if( null == m_btn_TabClan )
       {
            Engine.Utility.Log.Error("m_btn_TabClan 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabNear = fastComponent.FastGetComponent<UIButton>("TabNear");
       if( null == m_btn_TabNear )
       {
            Engine.Utility.Log.Error("m_btn_TabNear 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabGhost = fastComponent.FastGetComponent<UIButton>("TabGhost");
       if( null == m_btn_TabGhost )
       {
            Engine.Utility.Log.Error("m_btn_TabGhost 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabSystem = fastComponent.FastGetComponent<UIButton>("TabSystem");
       if( null == m_btn_TabSystem )
       {
            Engine.Utility.Log.Error("m_btn_TabSystem 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabRed = fastComponent.FastGetComponent<UIButton>("TabRed");
       if( null == m_btn_TabRed )
       {
            Engine.Utility.Log.Error("m_btn_TabRed 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnback = fastComponent.FastGetComponent<UIButton>("btnback");
       if( null == m_btn_btnback )
       {
            Engine.Utility.Log.Error("m_btn_btnback 为空，请检查prefab是否缺乏组件");
       }
        m_btn_hornBtn = fastComponent.FastGetComponent<UIButton>("hornBtn");
       if( null == m_btn_hornBtn )
       {
            Engine.Utility.Log.Error("m_btn_hornBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_settingBtn = fastComponent.FastGetComponent<UIButton>("settingBtn");
       if( null == m_btn_settingBtn )
       {
            Engine.Utility.Log.Error("m_btn_settingBtn 为空，请检查prefab是否缺乏组件");
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
        m_trans_inputnormal = fastComponent.FastGetComponent<Transform>("inputnormal");
       if( null == m_trans_inputnormal )
       {
            Engine.Utility.Log.Error("m_trans_inputnormal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnVoice = fastComponent.FastGetComponent<UIButton>("btnVoice");
       if( null == m_btn_btnVoice )
       {
            Engine.Utility.Log.Error("m_btn_btnVoice 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnMessage = fastComponent.FastGetComponent<UIButton>("btnMessage");
       if( null == m_btn_btnMessage )
       {
            Engine.Utility.Log.Error("m_btn_btnMessage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSend = fastComponent.FastGetComponent<UIButton>("BtnSend");
       if( null == m_btn_BtnSend )
       {
            Engine.Utility.Log.Error("m_btn_BtnSend 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnEmoji = fastComponent.FastGetComponent<UIButton>("btnEmoji");
       if( null == m_btn_btnEmoji )
       {
            Engine.Utility.Log.Error("m_btn_btnEmoji 为空，请检查prefab是否缺乏组件");
       }
        m_input_Input = fastComponent.FastGetComponent<UIInput>("Input");
       if( null == m_input_Input )
       {
            Engine.Utility.Log.Error("m_input_Input 为空，请检查prefab是否缺乏组件");
       }
        m_trans_voiceMicTips = fastComponent.FastGetComponent<Transform>("voiceMicTips");
       if( null == m_trans_voiceMicTips )
       {
            Engine.Utility.Log.Error("m_trans_voiceMicTips 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_joinClan.gameObject).onClick = _onClick_Btn_joinClan_Btn;
        UIEventListener.Get(m_btn_TabWorld.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_TabTeam.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_TabClan.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_TabNear.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_TabGhost.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_TabSystem.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_TabRed.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_btnback.gameObject).onClick = _onClick_Btnback_Btn;
        UIEventListener.Get(m_btn_hornBtn.gameObject).onClick = _onClick_HornBtn_Btn;
        UIEventListener.Get(m_btn_settingBtn.gameObject).onClick = _onClick_SettingBtn_Btn;
        UIEventListener.Get(m_btn_btnVoicetoteext.gameObject).onClick = _onClick_BtnVoicetoteext_Btn;
        UIEventListener.Get(m_btn_voice_input.gameObject).onClick = _onClick_Voice_input_Btn;
        UIEventListener.Get(m_btn_btnVoice.gameObject).onClick = _onClick_BtnVoice_Btn;
        UIEventListener.Get(m_btn_btnMessage.gameObject).onClick = _onClick_BtnMessage_Btn;
        UIEventListener.Get(m_btn_BtnSend.gameObject).onClick = _onClick_BtnSend_Btn;
        UIEventListener.Get(m_btn_btnEmoji.gameObject).onClick = _onClick_BtnEmoji_Btn;
    }

    void _onClick_Btn_joinClan_Btn(GameObject caster)
    {
        onClick_Btn_joinClan_Btn( caster );
    }

    void _OnBtnsClick(GameObject caster)
    {
        BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), caster.name);
        OnBtnsClick( btntype );
    }

    void _onClick_Btnback_Btn(GameObject caster)
    {
        onClick_Btnback_Btn( caster );
    }

    void _onClick_HornBtn_Btn(GameObject caster)
    {
        onClick_HornBtn_Btn( caster );
    }

    void _onClick_SettingBtn_Btn(GameObject caster)
    {
        onClick_SettingBtn_Btn( caster );
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

    void _onClick_BtnMessage_Btn(GameObject caster)
    {
        onClick_BtnMessage_Btn( caster );
    }

    void _onClick_BtnSend_Btn(GameObject caster)
    {
        onClick_BtnSend_Btn( caster );
    }

    void _onClick_BtnEmoji_Btn(GameObject caster)
    {
        onClick_BtnEmoji_Btn( caster );
    }


}
