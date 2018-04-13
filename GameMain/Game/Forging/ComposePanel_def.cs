//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ComposePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UIScrollView         m_scrollview_TypeScrollView;

    UISprite             m_sprite_bg;

    Transform            m_trans_RightContent;

    UIButton             m_btn_SingleCompound;

    Transform            m_trans_SingleCompoundCurrency;

    UIButton             m_btn_CompounAll;

    Transform            m_trans_CompoundAllCurrency;

    Transform            m_trans_UIIFGCost1;

    Transform            m_trans_UIIFGCost2;

    Transform            m_trans_UIIFGCost3;

    Transform            m_trans_UIIFGCost4;

    Transform            m_trans_UIIFGCost5;

    Transform            m_trans_UIIFGTarget;

    UIGridCreatorBase    m_ctor_ComposeDatasScrollview;

    Transform            m_trans_UIComposeGrid;


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
        m_scrollview_TypeScrollView = fastComponent.FastGetComponent<UIScrollView>("TypeScrollView");
       if( null == m_scrollview_TypeScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_TypeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_bg = fastComponent.FastGetComponent<UISprite>("bg");
       if( null == m_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RightContent = fastComponent.FastGetComponent<Transform>("RightContent");
       if( null == m_trans_RightContent )
       {
            Engine.Utility.Log.Error("m_trans_RightContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_SingleCompound = fastComponent.FastGetComponent<UIButton>("SingleCompound");
       if( null == m_btn_SingleCompound )
       {
            Engine.Utility.Log.Error("m_btn_SingleCompound 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SingleCompoundCurrency = fastComponent.FastGetComponent<Transform>("SingleCompoundCurrency");
       if( null == m_trans_SingleCompoundCurrency )
       {
            Engine.Utility.Log.Error("m_trans_SingleCompoundCurrency 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CompounAll = fastComponent.FastGetComponent<UIButton>("CompounAll");
       if( null == m_btn_CompounAll )
       {
            Engine.Utility.Log.Error("m_btn_CompounAll 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CompoundAllCurrency = fastComponent.FastGetComponent<Transform>("CompoundAllCurrency");
       if( null == m_trans_CompoundAllCurrency )
       {
            Engine.Utility.Log.Error("m_trans_CompoundAllCurrency 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIIFGCost1 = fastComponent.FastGetComponent<Transform>("UIIFGCost1");
       if( null == m_trans_UIIFGCost1 )
       {
            Engine.Utility.Log.Error("m_trans_UIIFGCost1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIIFGCost2 = fastComponent.FastGetComponent<Transform>("UIIFGCost2");
       if( null == m_trans_UIIFGCost2 )
       {
            Engine.Utility.Log.Error("m_trans_UIIFGCost2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIIFGCost3 = fastComponent.FastGetComponent<Transform>("UIIFGCost3");
       if( null == m_trans_UIIFGCost3 )
       {
            Engine.Utility.Log.Error("m_trans_UIIFGCost3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIIFGCost4 = fastComponent.FastGetComponent<Transform>("UIIFGCost4");
       if( null == m_trans_UIIFGCost4 )
       {
            Engine.Utility.Log.Error("m_trans_UIIFGCost4 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIIFGCost5 = fastComponent.FastGetComponent<Transform>("UIIFGCost5");
       if( null == m_trans_UIIFGCost5 )
       {
            Engine.Utility.Log.Error("m_trans_UIIFGCost5 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIIFGTarget = fastComponent.FastGetComponent<Transform>("UIIFGTarget");
       if( null == m_trans_UIIFGTarget )
       {
            Engine.Utility.Log.Error("m_trans_UIIFGTarget 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ComposeDatasScrollview = fastComponent.FastGetComponent<UIGridCreatorBase>("ComposeDatasScrollview");
       if( null == m_ctor_ComposeDatasScrollview )
       {
            Engine.Utility.Log.Error("m_ctor_ComposeDatasScrollview 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIComposeGrid = fastComponent.FastGetComponent<Transform>("UIComposeGrid");
       if( null == m_trans_UIComposeGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIComposeGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_SingleCompound.gameObject).onClick = _onClick_SingleCompound_Btn;
        UIEventListener.Get(m_btn_CompounAll.gameObject).onClick = _onClick_CompounAll_Btn;
    }

    void _onClick_SingleCompound_Btn(GameObject caster)
    {
        onClick_SingleCompound_Btn( caster );
    }

    void _onClick_CompounAll_Btn(GameObject caster)
    {
        onClick_CompounAll_Btn( caster );
    }


}
