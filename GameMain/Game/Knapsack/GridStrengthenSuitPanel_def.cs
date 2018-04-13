//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GridStrengthenSuitPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UISprite             m_sprite_Box;

    UILabel              m_label_StrengthenTxt;

    Transform            m_trans_Offset;

    UIGridCreatorBase    m_ctor_ScrollView;

    UIScrollView         m_scrollview_ColorScroll;

    UILabel              m_label_TextLabel;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_Content = fastComponent.FastGetComponent<Transform>("Content");
       if( null == m_trans_Content )
       {
            Engine.Utility.Log.Error("m_trans_Content 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Box = fastComponent.FastGetComponent<UISprite>("Box");
       if( null == m_sprite_Box )
       {
            Engine.Utility.Log.Error("m_sprite_Box 为空，请检查prefab是否缺乏组件");
       }
        m_label_StrengthenTxt = fastComponent.FastGetComponent<UILabel>("StrengthenTxt");
       if( null == m_label_StrengthenTxt )
       {
            Engine.Utility.Log.Error("m_label_StrengthenTxt 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Offset = fastComponent.FastGetComponent<Transform>("Offset");
       if( null == m_trans_Offset )
       {
            Engine.Utility.Log.Error("m_trans_Offset 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ScrollView");
       if( null == m_ctor_ScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_ColorScroll = fastComponent.FastGetComponent<UIScrollView>("ColorScroll");
       if( null == m_scrollview_ColorScroll )
       {
            Engine.Utility.Log.Error("m_scrollview_ColorScroll 为空，请检查prefab是否缺乏组件");
       }
        m_label_TextLabel = fastComponent.FastGetComponent<UILabel>("TextLabel");
       if( null == m_label_TextLabel )
       {
            Engine.Utility.Log.Error("m_label_TextLabel 为空，请检查prefab是否缺乏组件");
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
