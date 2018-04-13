//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MsgSettingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_presetmessage_setting;

    Transform            m_trans_btn_presetmessage;

    Transform            m_trans_btn_historymessage;

    Transform            m_trans_presetmessagePanel;

    UIButton             m_btn_btn_unclose02;

    UIButton             m_btn_closeEditor;

    UILabel              m_label_name;

    UIInput              m_input_addfriend_Input;

    UIButton             m_btn_btn_addfriend_reset;

    UIInput              m_input_addfamily_Input;

    UIButton             m_btn_btn_addfamily_reset;

    UIInput              m_input_addclan_Input;

    UIButton             m_btn_btn_addclan_reset;

    UIButton             m_btn_btn_confirm;

    UIWidget             m_widget_btn_close;

    UIButton             m_btn_btn_unclose01;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_presetmessage_setting = fastComponent.FastGetComponent<UIButton>("btn_presetmessage_setting");
       if( null == m_btn_btn_presetmessage_setting )
       {
            Engine.Utility.Log.Error("m_btn_btn_presetmessage_setting 为空，请检查prefab是否缺乏组件");
       }
        m_trans_btn_presetmessage = fastComponent.FastGetComponent<Transform>("btn_presetmessage");
       if( null == m_trans_btn_presetmessage )
       {
            Engine.Utility.Log.Error("m_trans_btn_presetmessage 为空，请检查prefab是否缺乏组件");
       }
        m_trans_btn_historymessage = fastComponent.FastGetComponent<Transform>("btn_historymessage");
       if( null == m_trans_btn_historymessage )
       {
            Engine.Utility.Log.Error("m_trans_btn_historymessage 为空，请检查prefab是否缺乏组件");
       }
        m_trans_presetmessagePanel = fastComponent.FastGetComponent<Transform>("presetmessagePanel");
       if( null == m_trans_presetmessagePanel )
       {
            Engine.Utility.Log.Error("m_trans_presetmessagePanel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_unclose02 = fastComponent.FastGetComponent<UIButton>("btn_unclose02");
       if( null == m_btn_btn_unclose02 )
       {
            Engine.Utility.Log.Error("m_btn_btn_unclose02 为空，请检查prefab是否缺乏组件");
       }
        m_btn_closeEditor = fastComponent.FastGetComponent<UIButton>("closeEditor");
       if( null == m_btn_closeEditor )
       {
            Engine.Utility.Log.Error("m_btn_closeEditor 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_input_addfriend_Input = fastComponent.FastGetComponent<UIInput>("addfriend_Input");
       if( null == m_input_addfriend_Input )
       {
            Engine.Utility.Log.Error("m_input_addfriend_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_addfriend_reset = fastComponent.FastGetComponent<UIButton>("btn_addfriend_reset");
       if( null == m_btn_btn_addfriend_reset )
       {
            Engine.Utility.Log.Error("m_btn_btn_addfriend_reset 为空，请检查prefab是否缺乏组件");
       }
        m_input_addfamily_Input = fastComponent.FastGetComponent<UIInput>("addfamily_Input");
       if( null == m_input_addfamily_Input )
       {
            Engine.Utility.Log.Error("m_input_addfamily_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_addfamily_reset = fastComponent.FastGetComponent<UIButton>("btn_addfamily_reset");
       if( null == m_btn_btn_addfamily_reset )
       {
            Engine.Utility.Log.Error("m_btn_btn_addfamily_reset 为空，请检查prefab是否缺乏组件");
       }
        m_input_addclan_Input = fastComponent.FastGetComponent<UIInput>("addclan_Input");
       if( null == m_input_addclan_Input )
       {
            Engine.Utility.Log.Error("m_input_addclan_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_addclan_reset = fastComponent.FastGetComponent<UIButton>("btn_addclan_reset");
       if( null == m_btn_btn_addclan_reset )
       {
            Engine.Utility.Log.Error("m_btn_btn_addclan_reset 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_confirm = fastComponent.FastGetComponent<UIButton>("btn_confirm");
       if( null == m_btn_btn_confirm )
       {
            Engine.Utility.Log.Error("m_btn_btn_confirm 为空，请检查prefab是否缺乏组件");
       }
        m_widget_btn_close = fastComponent.FastGetComponent<UIWidget>("btn_close");
       if( null == m_widget_btn_close )
       {
            Engine.Utility.Log.Error("m_widget_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_unclose01 = fastComponent.FastGetComponent<UIButton>("btn_unclose01");
       if( null == m_btn_btn_unclose01 )
       {
            Engine.Utility.Log.Error("m_btn_btn_unclose01 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_presetmessage_setting.gameObject).onClick = _onClick_Btn_presetmessage_setting_Btn;
        UIEventListener.Get(m_btn_btn_unclose02.gameObject).onClick = _onClick_Btn_unclose02_Btn;
        UIEventListener.Get(m_btn_closeEditor.gameObject).onClick = _onClick_CloseEditor_Btn;
        UIEventListener.Get(m_btn_btn_addfriend_reset.gameObject).onClick = _onClick_Btn_addfriend_reset_Btn;
        UIEventListener.Get(m_btn_btn_addfamily_reset.gameObject).onClick = _onClick_Btn_addfamily_reset_Btn;
        UIEventListener.Get(m_btn_btn_addclan_reset.gameObject).onClick = _onClick_Btn_addclan_reset_Btn;
        UIEventListener.Get(m_btn_btn_confirm.gameObject).onClick = _onClick_Btn_confirm_Btn;
        UIEventListener.Get(m_btn_btn_unclose01.gameObject).onClick = _onClick_Btn_unclose01_Btn;
    }

    void _onClick_Btn_presetmessage_setting_Btn(GameObject caster)
    {
        onClick_Btn_presetmessage_setting_Btn( caster );
    }

    void _onClick_Btn_unclose02_Btn(GameObject caster)
    {
        onClick_Btn_unclose02_Btn( caster );
    }

    void _onClick_CloseEditor_Btn(GameObject caster)
    {
        onClick_CloseEditor_Btn( caster );
    }

    void _onClick_Btn_addfriend_reset_Btn(GameObject caster)
    {
        onClick_Btn_addfriend_reset_Btn( caster );
    }

    void _onClick_Btn_addfamily_reset_Btn(GameObject caster)
    {
        onClick_Btn_addfamily_reset_Btn( caster );
    }

    void _onClick_Btn_addclan_reset_Btn(GameObject caster)
    {
        onClick_Btn_addclan_reset_Btn( caster );
    }

    void _onClick_Btn_confirm_Btn(GameObject caster)
    {
        onClick_Btn_confirm_Btn( caster );
    }

    void _onClick_Btn_unclose01_Btn(GameObject caster)
    {
        onClick_Btn_unclose01_Btn( caster );
    }


}
