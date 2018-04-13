//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RideMarkPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UIWidget             m_widget_RideMessage;

    UITexture            m__RideModel;

    UILabel              m_label_speed;

    UILabel              m_label_rarity;

    UILabel              m_label_showname;

    UILabel              m_label_getway;

    UIWidget             m_widget_noclick;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RideMessage = fastComponent.FastGetComponent<UIWidget>("RideMessage");
       if( null == m_widget_RideMessage )
       {
            Engine.Utility.Log.Error("m_widget_RideMessage 为空，请检查prefab是否缺乏组件");
       }
        m__RideModel = fastComponent.FastGetComponent<UITexture>("RideModel");
       if( null == m__RideModel )
       {
            Engine.Utility.Log.Error("m__RideModel 为空，请检查prefab是否缺乏组件");
       }
        m_label_speed = fastComponent.FastGetComponent<UILabel>("speed");
       if( null == m_label_speed )
       {
            Engine.Utility.Log.Error("m_label_speed 为空，请检查prefab是否缺乏组件");
       }
        m_label_rarity = fastComponent.FastGetComponent<UILabel>("rarity");
       if( null == m_label_rarity )
       {
            Engine.Utility.Log.Error("m_label_rarity 为空，请检查prefab是否缺乏组件");
       }
        m_label_showname = fastComponent.FastGetComponent<UILabel>("showname");
       if( null == m_label_showname )
       {
            Engine.Utility.Log.Error("m_label_showname 为空，请检查prefab是否缺乏组件");
       }
        m_label_getway = fastComponent.FastGetComponent<UILabel>("getway");
       if( null == m_label_getway )
       {
            Engine.Utility.Log.Error("m_label_getway 为空，请检查prefab是否缺乏组件");
       }
        m_widget_noclick = fastComponent.FastGetComponent<UIWidget>("noclick");
       if( null == m_widget_noclick )
       {
            Engine.Utility.Log.Error("m_widget_noclick 为空，请检查prefab是否缺乏组件");
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
