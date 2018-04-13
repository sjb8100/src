//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class LoadingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UITexture            m__texture;

    UISprite             m_sprite_TopBorder;

    UISprite             m_sprite_BottomBorder;

    UISprite             m_sprite_LeftBorder;

    UISprite             m_sprite_RigtBorder;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m__texture = fastComponent.FastGetComponent<UITexture>("texture");
       if( null == m__texture )
       {
            Engine.Utility.Log.Error("m__texture 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_TopBorder = fastComponent.FastGetComponent<UISprite>("TopBorder");
       if( null == m_sprite_TopBorder )
       {
            Engine.Utility.Log.Error("m_sprite_TopBorder 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_BottomBorder = fastComponent.FastGetComponent<UISprite>("BottomBorder");
       if( null == m_sprite_BottomBorder )
       {
            Engine.Utility.Log.Error("m_sprite_BottomBorder 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_LeftBorder = fastComponent.FastGetComponent<UISprite>("LeftBorder");
       if( null == m_sprite_LeftBorder )
       {
            Engine.Utility.Log.Error("m_sprite_LeftBorder 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_RigtBorder = fastComponent.FastGetComponent<UISprite>("RigtBorder");
       if( null == m_sprite_RigtBorder )
       {
            Engine.Utility.Log.Error("m_sprite_RigtBorder 为空，请检查prefab是否缺乏组件");
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
