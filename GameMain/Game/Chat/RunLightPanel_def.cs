//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RunLightPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_TOP;

    Transform            m_trans_root;

    UIWidget             m_widget_xmlText;

    UILabel              m_label_Label;

    Transform            m_trans_Bottom;

    UISprite             m_sprite_buttombg;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_TOP = fastComponent.FastGetComponent<Transform>("TOP");
       if( null == m_trans_TOP )
       {
            Engine.Utility.Log.Error("m_trans_TOP 为空，请检查prefab是否缺乏组件");
       }
        m_trans_root = fastComponent.FastGetComponent<Transform>("root");
       if( null == m_trans_root )
       {
            Engine.Utility.Log.Error("m_trans_root 为空，请检查prefab是否缺乏组件");
       }
        m_widget_xmlText = fastComponent.FastGetComponent<UIWidget>("xmlText");
       if( null == m_widget_xmlText )
       {
            Engine.Utility.Log.Error("m_widget_xmlText 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Bottom = fastComponent.FastGetComponent<Transform>("Bottom");
       if( null == m_trans_Bottom )
       {
            Engine.Utility.Log.Error("m_trans_Bottom 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_buttombg = fastComponent.FastGetComponent<UISprite>("buttombg");
       if( null == m_sprite_buttombg )
       {
            Engine.Utility.Log.Error("m_sprite_buttombg 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
