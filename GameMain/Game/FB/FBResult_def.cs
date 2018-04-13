//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FBResult: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_ResultContent;

    UISprite             m_sprite_win;

    UISprite             m_sprite_defeat;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_ResultContent = fastComponent.FastGetComponent<Transform>("ResultContent");
       if( null == m_trans_ResultContent )
       {
            Engine.Utility.Log.Error("m_trans_ResultContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_win = fastComponent.FastGetComponent<UISprite>("win");
       if( null == m_sprite_win )
       {
            Engine.Utility.Log.Error("m_sprite_win 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_defeat = fastComponent.FastGetComponent<UISprite>("defeat");
       if( null == m_sprite_defeat )
       {
            Engine.Utility.Log.Error("m_sprite_defeat 为空，请检查prefab是否缺乏组件");
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
