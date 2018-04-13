//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GetWayPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UIButton             m_btn_btn_unclose;

    UIGridCreatorBase    m_ctor_GetWayScrollView;

    UIGridCreatorBase    m_ctor_GetWayRoot;

    UIButton             m_btn_btn_close;

    UILabel              m_label_DesLabel;

    Transform            m_trans_UIGetWayGrid;


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
        m_btn_btn_unclose = fastComponent.FastGetComponent<UIButton>("btn_unclose");
       if( null == m_btn_btn_unclose )
       {
            Engine.Utility.Log.Error("m_btn_btn_unclose 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_GetWayScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("GetWayScrollView");
       if( null == m_ctor_GetWayScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_GetWayScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_GetWayRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("GetWayRoot");
       if( null == m_ctor_GetWayRoot )
       {
            Engine.Utility.Log.Error("m_ctor_GetWayRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_DesLabel = fastComponent.FastGetComponent<UILabel>("DesLabel");
       if( null == m_label_DesLabel )
       {
            Engine.Utility.Log.Error("m_label_DesLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIGetWayGrid = fastComponent.FastGetComponent<Transform>("UIGetWayGrid");
       if( null == m_trans_UIGetWayGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIGetWayGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_unclose.gameObject).onClick = _onClick_Btn_unclose_Btn;
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
    }

    void _onClick_Btn_unclose_Btn(GameObject caster)
    {
        onClick_Btn_unclose_Btn( caster );
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }


}
