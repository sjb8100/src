//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ChinaBiblePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_closeBtn;

    UIWidget             m_widget_root;

    UIScrollView         m_scrollview_TypeScrollView;

    Transform            m_trans_UICtrTypeGrid;

    UIWidget             m_widget_UISecondTypeGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_closeBtn = fastComponent.FastGetComponent<UIButton>("closeBtn");
       if( null == m_btn_closeBtn )
       {
            Engine.Utility.Log.Error("m_btn_closeBtn 为空，请检查prefab是否缺乏组件");
       }
        m_widget_root = fastComponent.FastGetComponent<UIWidget>("root");
       if( null == m_widget_root )
       {
            Engine.Utility.Log.Error("m_widget_root 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_TypeScrollView = fastComponent.FastGetComponent<UIScrollView>("TypeScrollView");
       if( null == m_scrollview_TypeScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_TypeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UICtrTypeGrid = fastComponent.FastGetComponent<Transform>("UICtrTypeGrid");
       if( null == m_trans_UICtrTypeGrid )
       {
            Engine.Utility.Log.Error("m_trans_UICtrTypeGrid 为空，请检查prefab是否缺乏组件");
       }
        m_widget_UISecondTypeGrid = fastComponent.FastGetComponent<UIWidget>("UISecondTypeGrid");
       if( null == m_widget_UISecondTypeGrid )
       {
            Engine.Utility.Log.Error("m_widget_UISecondTypeGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_closeBtn.gameObject).onClick = _onClick_CloseBtn_Btn;
    }

    void _onClick_CloseBtn_Btn(GameObject caster)
    {
        onClick_CloseBtn_Btn( caster );
    }


}
