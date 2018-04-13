//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RankPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIScrollView         m_scrollview_TypeScrollView;

    UILabel              m_label_Label_1;

    UILabel              m_label_Label_2;

    UILabel              m_label_Label_3;

    UILabel              m_label_Label_4;

    UILabel              m_label_Label_5;

    UILabel              m_label_rank_label;

    UILabel              m_label_power_label;

    UIGridCreatorBase    m_ctor_RankScroll;

    Transform            m_trans_UIRankGrid;

    Transform            m_trans_UICtrTypeGrid;

    UIWidget             m_widget_UISecondTypeGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_scrollview_TypeScrollView = fastComponent.FastGetComponent<UIScrollView>("TypeScrollView");
       if( null == m_scrollview_TypeScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_TypeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label_1 = fastComponent.FastGetComponent<UILabel>("Label_1");
       if( null == m_label_Label_1 )
       {
            Engine.Utility.Log.Error("m_label_Label_1 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label_2 = fastComponent.FastGetComponent<UILabel>("Label_2");
       if( null == m_label_Label_2 )
       {
            Engine.Utility.Log.Error("m_label_Label_2 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label_3 = fastComponent.FastGetComponent<UILabel>("Label_3");
       if( null == m_label_Label_3 )
       {
            Engine.Utility.Log.Error("m_label_Label_3 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label_4 = fastComponent.FastGetComponent<UILabel>("Label_4");
       if( null == m_label_Label_4 )
       {
            Engine.Utility.Log.Error("m_label_Label_4 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label_5 = fastComponent.FastGetComponent<UILabel>("Label_5");
       if( null == m_label_Label_5 )
       {
            Engine.Utility.Log.Error("m_label_Label_5 为空，请检查prefab是否缺乏组件");
       }
        m_label_rank_label = fastComponent.FastGetComponent<UILabel>("rank_label");
       if( null == m_label_rank_label )
       {
            Engine.Utility.Log.Error("m_label_rank_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_power_label = fastComponent.FastGetComponent<UILabel>("power_label");
       if( null == m_label_power_label )
       {
            Engine.Utility.Log.Error("m_label_power_label 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_RankScroll = fastComponent.FastGetComponent<UIGridCreatorBase>("RankScroll");
       if( null == m_ctor_RankScroll )
       {
            Engine.Utility.Log.Error("m_ctor_RankScroll 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIRankGrid = fastComponent.FastGetComponent<Transform>("UIRankGrid");
       if( null == m_trans_UIRankGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIRankGrid 为空，请检查prefab是否缺乏组件");
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
    }


}
