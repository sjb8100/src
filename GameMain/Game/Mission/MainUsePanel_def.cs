//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MainUsePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_item_name;

    UITexture            m__icon;

    UISprite             m_sprite_iconBg;

    UISprite             m_sprite_slider;

    UISprite             m_sprite_iconCollect;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_item_name = fastComponent.FastGetComponent<UILabel>("item_name");
       if( null == m_label_item_name )
       {
            Engine.Utility.Log.Error("m_label_item_name 为空，请检查prefab是否缺乏组件");
       }
        m__icon = fastComponent.FastGetComponent<UITexture>("icon");
       if( null == m__icon )
       {
            Engine.Utility.Log.Error("m__icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_iconBg = fastComponent.FastGetComponent<UISprite>("iconBg");
       if( null == m_sprite_iconBg )
       {
            Engine.Utility.Log.Error("m_sprite_iconBg 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_slider = fastComponent.FastGetComponent<UISprite>("slider");
       if( null == m_sprite_slider )
       {
            Engine.Utility.Log.Error("m_sprite_slider 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_iconCollect = fastComponent.FastGetComponent<UISprite>("iconCollect");
       if( null == m_sprite_iconCollect )
       {
            Engine.Utility.Log.Error("m_sprite_iconCollect 为空，请检查prefab是否缺乏组件");
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
