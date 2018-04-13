//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RealTimeVoicePanel: UIPanelBase
{

    UIButton      m_btn_jointeam;

    UIButton      m_btn_joinnation;

    UIInput       m_input_Inputroom;

    UIToggle      m_toggle_mic;

    UIToggle      m_toggle_speaker;

    UILabel       m_label_rolestate;

    UILabel       m_label_roleid;

    UIToggle      m_toggle_nationrole;

    UIButton      m_btn_quitroom;

    UIButton      m_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
        m_btn_jointeam = GetChildComponent<UIButton>("jointeam");
       if( null == m_btn_jointeam )
       {
            Engine.Utility.Log.Error("m_btn_jointeam 为空，请检查prefab是否缺乏组件");
       }
        m_btn_joinnation = GetChildComponent<UIButton>("joinnation");
       if( null == m_btn_joinnation )
       {
            Engine.Utility.Log.Error("m_btn_joinnation 为空，请检查prefab是否缺乏组件");
       }
        m_input_Inputroom = GetChildComponent<UIInput>("Inputroom");
       if( null == m_input_Inputroom )
       {
            Engine.Utility.Log.Error("m_input_Inputroom 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_mic = GetChildComponent<UIToggle>("mic");
       if( null == m_toggle_mic )
       {
            Engine.Utility.Log.Error("m_toggle_mic 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_speaker = GetChildComponent<UIToggle>("speaker");
       if( null == m_toggle_speaker )
       {
            Engine.Utility.Log.Error("m_toggle_speaker 为空，请检查prefab是否缺乏组件");
       }
        m_label_rolestate = GetChildComponent<UILabel>("rolestate");
       if( null == m_label_rolestate )
       {
            Engine.Utility.Log.Error("m_label_rolestate 为空，请检查prefab是否缺乏组件");
       }
        m_label_roleid = GetChildComponent<UILabel>("roleid");
       if( null == m_label_roleid )
       {
            Engine.Utility.Log.Error("m_label_roleid 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_nationrole = GetChildComponent<UIToggle>("nationrole");
       if( null == m_toggle_nationrole )
       {
            Engine.Utility.Log.Error("m_toggle_nationrole 为空，请检查prefab是否缺乏组件");
       }
        m_btn_quitroom = GetChildComponent<UIButton>("quitroom");
       if( null == m_btn_quitroom )
       {
            Engine.Utility.Log.Error("m_btn_quitroom 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = GetChildComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_jointeam.gameObject).onClick = _onClick_Jointeam_Btn;
        UIEventListener.Get(m_btn_joinnation.gameObject).onClick = _onClick_Joinnation_Btn;
        UIEventListener.Get(m_btn_quitroom.gameObject).onClick = _onClick_Quitroom_Btn;
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Jointeam_Btn(GameObject caster)
    {
        onClick_Jointeam_Btn( caster );
    }

    void _onClick_Joinnation_Btn(GameObject caster)
    {
        onClick_Joinnation_Btn( caster );
    }

    void _onClick_Quitroom_Btn(GameObject caster)
    {
        onClick_Quitroom_Btn( caster );
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
