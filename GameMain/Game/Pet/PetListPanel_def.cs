//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetListPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_PetScrollView;

    Transform            m_trans_Root;

    UIButton             m_btn_btn_queding;

    UIWidget             m_widget_box1;

    UIWidget             m_widget_box2;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_PetScrollView = fastComponent.FastGetComponent<Transform>("PetScrollView");
       if( null == m_trans_PetScrollView )
       {
            Engine.Utility.Log.Error("m_trans_PetScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Root = fastComponent.FastGetComponent<Transform>("Root");
       if( null == m_trans_Root )
       {
            Engine.Utility.Log.Error("m_trans_Root 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_queding = fastComponent.FastGetComponent<UIButton>("btn_queding");
       if( null == m_btn_btn_queding )
       {
            Engine.Utility.Log.Error("m_btn_btn_queding 为空，请检查prefab是否缺乏组件");
       }
        m_widget_box1 = fastComponent.FastGetComponent<UIWidget>("box1");
       if( null == m_widget_box1 )
       {
            Engine.Utility.Log.Error("m_widget_box1 为空，请检查prefab是否缺乏组件");
       }
        m_widget_box2 = fastComponent.FastGetComponent<UIWidget>("box2");
       if( null == m_widget_box2 )
       {
            Engine.Utility.Log.Error("m_widget_box2 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_queding.gameObject).onClick = _onClick_Btn_queding_Btn;
    }

    void _onClick_Btn_queding_Btn(GameObject caster)
    {
        onClick_Btn_queding_Btn( caster );
    }


}
