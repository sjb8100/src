//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class AccumulativeRechargePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_close;

    Transform            m_trans_DailyRoot;

    Transform            m_trans_ItemListRoot;

    Transform            m_trans_WeekRoot;

    UILabel              m_label_centerLabel;

    UILabel              m_label_rightLabel;

    UIButton             m_btn_byeBtn;

    Transform            m_trans_UIItemRewardGrid;

    Transform            m_trans_UIAccumulativeDailyGrid;

    UIWidget             m_widget_UIAccumulativeWeekGrid;

    UIWidget             m_widget_Btn;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DailyRoot = fastComponent.FastGetComponent<Transform>("DailyRoot");
       if( null == m_trans_DailyRoot )
       {
            Engine.Utility.Log.Error("m_trans_DailyRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemListRoot = fastComponent.FastGetComponent<Transform>("ItemListRoot");
       if( null == m_trans_ItemListRoot )
       {
            Engine.Utility.Log.Error("m_trans_ItemListRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WeekRoot = fastComponent.FastGetComponent<Transform>("WeekRoot");
       if( null == m_trans_WeekRoot )
       {
            Engine.Utility.Log.Error("m_trans_WeekRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_centerLabel = fastComponent.FastGetComponent<UILabel>("centerLabel");
       if( null == m_label_centerLabel )
       {
            Engine.Utility.Log.Error("m_label_centerLabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_rightLabel = fastComponent.FastGetComponent<UILabel>("rightLabel");
       if( null == m_label_rightLabel )
       {
            Engine.Utility.Log.Error("m_label_rightLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_byeBtn = fastComponent.FastGetComponent<UIButton>("byeBtn");
       if( null == m_btn_byeBtn )
       {
            Engine.Utility.Log.Error("m_btn_byeBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIAccumulativeDailyGrid = fastComponent.FastGetComponent<Transform>("UIAccumulativeDailyGrid");
       if( null == m_trans_UIAccumulativeDailyGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIAccumulativeDailyGrid 为空，请检查prefab是否缺乏组件");
       }
        m_widget_UIAccumulativeWeekGrid = fastComponent.FastGetComponent<UIWidget>("UIAccumulativeWeekGrid");
       if( null == m_widget_UIAccumulativeWeekGrid )
       {
            Engine.Utility.Log.Error("m_widget_UIAccumulativeWeekGrid 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Btn = fastComponent.FastGetComponent<UIWidget>("Btn");
       if( null == m_widget_Btn )
       {
            Engine.Utility.Log.Error("m_widget_Btn 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
        UIEventListener.Get(m_btn_byeBtn.gameObject).onClick = _onClick_ByeBtn_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_ByeBtn_Btn(GameObject caster)
    {
        onClick_ByeBtn_Btn( caster );
    }


}
