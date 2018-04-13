//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TipsPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_tip;

    Transform            m_trans_root;

    Transform            m_trans_miscomplete;

    Transform            m_trans_fightPowerRoot;

    Transform            m_trans_Down;

    UILabel              m_label_powerDownLabel;

    Transform            m_trans_Up;

    UILabel              m_label_powerUpLabel;

    UILabel              m_label_powerChangeLabel;

    Transform            m_trans_fxRoot;

    Transform            m_trans_fxRootContent;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_tip = fastComponent.FastGetComponent<UISprite>("tip");
       if( null == m_sprite_tip )
       {
            Engine.Utility.Log.Error("m_sprite_tip 为空，请检查prefab是否缺乏组件");
       }
        m_trans_root = fastComponent.FastGetComponent<Transform>("root");
       if( null == m_trans_root )
       {
            Engine.Utility.Log.Error("m_trans_root 为空，请检查prefab是否缺乏组件");
       }
        m_trans_miscomplete = fastComponent.FastGetComponent<Transform>("miscomplete");
       if( null == m_trans_miscomplete )
       {
            Engine.Utility.Log.Error("m_trans_miscomplete 为空，请检查prefab是否缺乏组件");
       }
        m_trans_fightPowerRoot = fastComponent.FastGetComponent<Transform>("fightPowerRoot");
       if( null == m_trans_fightPowerRoot )
       {
            Engine.Utility.Log.Error("m_trans_fightPowerRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Down = fastComponent.FastGetComponent<Transform>("Down");
       if( null == m_trans_Down )
       {
            Engine.Utility.Log.Error("m_trans_Down 为空，请检查prefab是否缺乏组件");
       }
        m_label_powerDownLabel = fastComponent.FastGetComponent<UILabel>("powerDownLabel");
       if( null == m_label_powerDownLabel )
       {
            Engine.Utility.Log.Error("m_label_powerDownLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Up = fastComponent.FastGetComponent<Transform>("Up");
       if( null == m_trans_Up )
       {
            Engine.Utility.Log.Error("m_trans_Up 为空，请检查prefab是否缺乏组件");
       }
        m_label_powerUpLabel = fastComponent.FastGetComponent<UILabel>("powerUpLabel");
       if( null == m_label_powerUpLabel )
       {
            Engine.Utility.Log.Error("m_label_powerUpLabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_powerChangeLabel = fastComponent.FastGetComponent<UILabel>("powerChangeLabel");
       if( null == m_label_powerChangeLabel )
       {
            Engine.Utility.Log.Error("m_label_powerChangeLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_fxRoot = fastComponent.FastGetComponent<Transform>("fxRoot");
       if( null == m_trans_fxRoot )
       {
            Engine.Utility.Log.Error("m_trans_fxRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_fxRootContent = fastComponent.FastGetComponent<Transform>("fxRootContent");
       if( null == m_trans_fxRootContent )
       {
            Engine.Utility.Log.Error("m_trans_fxRootContent 为空，请检查prefab是否缺乏组件");
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
