//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class WishTreePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_Label;

    UIButton             m_btn_close;

    UIWidget             m_widget_tree;

    UIButton             m_btn_btn_buy_1;

    UIButton             m_btn_btn_buy_2;

    UIButton             m_btn_btn_buy_3;

    UIButton             m_btn_btn_buy_4;

    UISprite             m_sprite_reminder;

    UIButton             m_btn_cancel;

    UIButton             m_btn_confirm;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_widget_tree = fastComponent.FastGetComponent<UIWidget>("tree");
       if( null == m_widget_tree )
       {
            Engine.Utility.Log.Error("m_widget_tree 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_buy_1 = fastComponent.FastGetComponent<UIButton>("btn_buy_1");
       if( null == m_btn_btn_buy_1 )
       {
            Engine.Utility.Log.Error("m_btn_btn_buy_1 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_buy_2 = fastComponent.FastGetComponent<UIButton>("btn_buy_2");
       if( null == m_btn_btn_buy_2 )
       {
            Engine.Utility.Log.Error("m_btn_btn_buy_2 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_buy_3 = fastComponent.FastGetComponent<UIButton>("btn_buy_3");
       if( null == m_btn_btn_buy_3 )
       {
            Engine.Utility.Log.Error("m_btn_btn_buy_3 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_buy_4 = fastComponent.FastGetComponent<UIButton>("btn_buy_4");
       if( null == m_btn_btn_buy_4 )
       {
            Engine.Utility.Log.Error("m_btn_btn_buy_4 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_reminder = fastComponent.FastGetComponent<UISprite>("reminder");
       if( null == m_sprite_reminder )
       {
            Engine.Utility.Log.Error("m_sprite_reminder 为空，请检查prefab是否缺乏组件");
       }
        m_btn_cancel = fastComponent.FastGetComponent<UIButton>("cancel");
       if( null == m_btn_cancel )
       {
            Engine.Utility.Log.Error("m_btn_cancel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_confirm = fastComponent.FastGetComponent<UIButton>("confirm");
       if( null == m_btn_confirm )
       {
            Engine.Utility.Log.Error("m_btn_confirm 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_btn_buy_1.gameObject).onClick = _onClick_Btn_buy_1_Btn;
        UIEventListener.Get(m_btn_btn_buy_2.gameObject).onClick = _onClick_Btn_buy_2_Btn;
        UIEventListener.Get(m_btn_btn_buy_3.gameObject).onClick = _onClick_Btn_buy_3_Btn;
        UIEventListener.Get(m_btn_btn_buy_4.gameObject).onClick = _onClick_Btn_buy_4_Btn;
        UIEventListener.Get(m_btn_cancel.gameObject).onClick = _onClick_Cancel_Btn;
        UIEventListener.Get(m_btn_confirm.gameObject).onClick = _onClick_Confirm_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Btn_buy_1_Btn(GameObject caster)
    {
        onClick_Btn_buy_1_Btn( caster );
    }

    void _onClick_Btn_buy_2_Btn(GameObject caster)
    {
        onClick_Btn_buy_2_Btn( caster );
    }

    void _onClick_Btn_buy_3_Btn(GameObject caster)
    {
        onClick_Btn_buy_3_Btn( caster );
    }

    void _onClick_Btn_buy_4_Btn(GameObject caster)
    {
        onClick_Btn_buy_4_Btn( caster );
    }

    void _onClick_Cancel_Btn(GameObject caster)
    {
        onClick_Cancel_Btn( caster );
    }

    void _onClick_Confirm_Btn(GameObject caster)
    {
        onClick_Confirm_Btn( caster );
    }


}
