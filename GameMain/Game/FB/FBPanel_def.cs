//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FBPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_down;

    UILabel              m_label_frequency_Plot;

    UILabel              m_label_frequency_label;

    UIScrollView         m_scrollview_FbScrollView;

    UIGrid               m_grid_FbGrid;

    UIWidget             m_widget_FBCard;

    Transform            m_trans_Temp;

    UIButton             m_btn_EquipRechange;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_down = fastComponent.FastGetComponent<Transform>("down");
       if( null == m_trans_down )
       {
            Engine.Utility.Log.Error("m_trans_down 为空，请检查prefab是否缺乏组件");
       }
        m_label_frequency_Plot = fastComponent.FastGetComponent<UILabel>("frequency_Plot");
       if( null == m_label_frequency_Plot )
       {
            Engine.Utility.Log.Error("m_label_frequency_Plot 为空，请检查prefab是否缺乏组件");
       }
        m_label_frequency_label = fastComponent.FastGetComponent<UILabel>("frequency_label");
       if( null == m_label_frequency_label )
       {
            Engine.Utility.Log.Error("m_label_frequency_label 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_FbScrollView = fastComponent.FastGetComponent<UIScrollView>("FbScrollView");
       if( null == m_scrollview_FbScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_FbScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_grid_FbGrid = fastComponent.FastGetComponent<UIGrid>("FbGrid");
       if( null == m_grid_FbGrid )
       {
            Engine.Utility.Log.Error("m_grid_FbGrid 为空，请检查prefab是否缺乏组件");
       }
        m_widget_FBCard = fastComponent.FastGetComponent<UIWidget>("FBCard");
       if( null == m_widget_FBCard )
       {
            Engine.Utility.Log.Error("m_widget_FBCard 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Temp = fastComponent.FastGetComponent<Transform>("Temp");
       if( null == m_trans_Temp )
       {
            Engine.Utility.Log.Error("m_trans_Temp 为空，请检查prefab是否缺乏组件");
       }
        m_btn_EquipRechange = fastComponent.FastGetComponent<UIButton>("EquipRechange");
       if( null == m_btn_EquipRechange )
       {
            Engine.Utility.Log.Error("m_btn_EquipRechange 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_EquipRechange.gameObject).onClick = _onClick_EquipRechange_Btn;
    }

    void _onClick_EquipRechange_Btn(GameObject caster)
    {
        onClick_EquipRechange_Btn( caster );
    }


}
