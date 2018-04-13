//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class NpcDeliverPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_name;

    UIButton             m_btn_close;

    UIGridCreatorBase    m_ctor_ScrollView;

    UIGridCreatorBase    m_ctor_Right;

    Transform            m_trans_DeliverGrid;

    UISprite             m_sprite_Icon;

    UILabel              m_label_Cost_label;

    UISprite             m_sprite_Chose;

    Transform            m_trans_TabGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ScrollView");
       if( null == m_ctor_ScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Right = fastComponent.FastGetComponent<UIGridCreatorBase>("Right");
       if( null == m_ctor_Right )
       {
            Engine.Utility.Log.Error("m_ctor_Right 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DeliverGrid = fastComponent.FastGetComponent<Transform>("DeliverGrid");
       if( null == m_trans_DeliverGrid )
       {
            Engine.Utility.Log.Error("m_trans_DeliverGrid 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Icon = fastComponent.FastGetComponent<UISprite>("Icon");
       if( null == m_sprite_Icon )
       {
            Engine.Utility.Log.Error("m_sprite_Icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_Cost_label = fastComponent.FastGetComponent<UILabel>("Cost_label");
       if( null == m_label_Cost_label )
       {
            Engine.Utility.Log.Error("m_label_Cost_label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Chose = fastComponent.FastGetComponent<UISprite>("Chose");
       if( null == m_sprite_Chose )
       {
            Engine.Utility.Log.Error("m_sprite_Chose 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TabGrid = fastComponent.FastGetComponent<Transform>("TabGrid");
       if( null == m_trans_TabGrid )
       {
            Engine.Utility.Log.Error("m_trans_TabGrid 为空，请检查prefab是否缺乏组件");
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
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
