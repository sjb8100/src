//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class AutoSkillPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_autoskillitem;

    UISprite             m_sprite_choosemark;

    UISprite             m_sprite_btn_Close;

    UIButton             m_btn_btn_right;

    Transform            m_trans_currency;

    Transform            m_trans_state_1;

    UILabel              m_label_StateOneLabel;

    Transform            m_trans_state_2;

    UILabel              m_label_StateTwoLabel;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_autoskillitem = fastComponent.FastGetComponent<UISprite>("autoskillitem");
       if( null == m_sprite_autoskillitem )
       {
            Engine.Utility.Log.Error("m_sprite_autoskillitem 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_choosemark = fastComponent.FastGetComponent<UISprite>("choosemark");
       if( null == m_sprite_choosemark )
       {
            Engine.Utility.Log.Error("m_sprite_choosemark 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btn_Close = fastComponent.FastGetComponent<UISprite>("btn_Close");
       if( null == m_sprite_btn_Close )
       {
            Engine.Utility.Log.Error("m_sprite_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_right = fastComponent.FastGetComponent<UIButton>("btn_right");
       if( null == m_btn_btn_right )
       {
            Engine.Utility.Log.Error("m_btn_btn_right 为空，请检查prefab是否缺乏组件");
       }
        m_trans_currency = fastComponent.FastGetComponent<Transform>("currency");
       if( null == m_trans_currency )
       {
            Engine.Utility.Log.Error("m_trans_currency 为空，请检查prefab是否缺乏组件");
       }
        m_trans_state_1 = fastComponent.FastGetComponent<Transform>("state_1");
       if( null == m_trans_state_1 )
       {
            Engine.Utility.Log.Error("m_trans_state_1 为空，请检查prefab是否缺乏组件");
       }
        m_label_StateOneLabel = fastComponent.FastGetComponent<UILabel>("StateOneLabel");
       if( null == m_label_StateOneLabel )
       {
            Engine.Utility.Log.Error("m_label_StateOneLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_state_2 = fastComponent.FastGetComponent<Transform>("state_2");
       if( null == m_trans_state_2 )
       {
            Engine.Utility.Log.Error("m_trans_state_2 为空，请检查prefab是否缺乏组件");
       }
        m_label_StateTwoLabel = fastComponent.FastGetComponent<UILabel>("StateTwoLabel");
       if( null == m_label_StateTwoLabel )
       {
            Engine.Utility.Log.Error("m_label_StateTwoLabel 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_right.gameObject).onClick = _onClick_Btn_right_Btn;
    }

    void _onClick_Btn_right_Btn(GameObject caster)
    {
        onClick_Btn_right_Btn( caster );
    }


}
