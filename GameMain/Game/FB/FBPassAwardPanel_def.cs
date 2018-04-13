//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FBPassAwardPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Wing;

    UISprite             m_sprite_Bg;

    Transform            m_trans_AwardContent;

    Transform            m_trans_UIAwardItemGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_Wing = fastComponent.FastGetComponent<Transform>("Wing");
       if( null == m_trans_Wing )
       {
            Engine.Utility.Log.Error("m_trans_Wing 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Bg = fastComponent.FastGetComponent<UISprite>("Bg");
       if( null == m_sprite_Bg )
       {
            Engine.Utility.Log.Error("m_sprite_Bg 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AwardContent = fastComponent.FastGetComponent<Transform>("AwardContent");
       if( null == m_trans_AwardContent )
       {
            Engine.Utility.Log.Error("m_trans_AwardContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIAwardItemGrid = fastComponent.FastGetComponent<Transform>("UIAwardItemGrid");
       if( null == m_trans_UIAwardItemGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIAwardItemGrid 为空，请检查prefab是否缺乏组件");
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
