//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class DailyTestPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_testpanel;

    UIButton             m_btn_btn_Consume;

    UILabel              m_label_left_Label;

    UILabel              m_label_ItemAddTimes_label;

    UILabel              m_label_ItemAddNum_label;

    UISlider             m_slider_Expslider;

    UILabel              m_label_exp_percent;

    UIButton             m_btn_btn_Tips;

    Transform            m_trans_UIItemInfoGrid;

    Transform            m_trans_tipsContent;

    UILabel              m_label_tipsContentLbl;

    Transform            m_trans_UIdailytestgrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_testpanel = fastComponent.FastGetComponent<Transform>("testpanel");
       if( null == m_trans_testpanel )
       {
            Engine.Utility.Log.Error("m_trans_testpanel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Consume = fastComponent.FastGetComponent<UIButton>("btn_Consume");
       if( null == m_btn_btn_Consume )
       {
            Engine.Utility.Log.Error("m_btn_btn_Consume 为空，请检查prefab是否缺乏组件");
       }
        m_label_left_Label = fastComponent.FastGetComponent<UILabel>("left_Label");
       if( null == m_label_left_Label )
       {
            Engine.Utility.Log.Error("m_label_left_Label 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemAddTimes_label = fastComponent.FastGetComponent<UILabel>("ItemAddTimes_label");
       if( null == m_label_ItemAddTimes_label )
       {
            Engine.Utility.Log.Error("m_label_ItemAddTimes_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemAddNum_label = fastComponent.FastGetComponent<UILabel>("ItemAddNum_label");
       if( null == m_label_ItemAddNum_label )
       {
            Engine.Utility.Log.Error("m_label_ItemAddNum_label 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Expslider = fastComponent.FastGetComponent<UISlider>("Expslider");
       if( null == m_slider_Expslider )
       {
            Engine.Utility.Log.Error("m_slider_Expslider 为空，请检查prefab是否缺乏组件");
       }
        m_label_exp_percent = fastComponent.FastGetComponent<UILabel>("exp_percent");
       if( null == m_label_exp_percent )
       {
            Engine.Utility.Log.Error("m_label_exp_percent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Tips = fastComponent.FastGetComponent<UIButton>("btn_Tips");
       if( null == m_btn_btn_Tips )
       {
            Engine.Utility.Log.Error("m_btn_btn_Tips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemInfoGrid = fastComponent.FastGetComponent<Transform>("UIItemInfoGrid");
       if( null == m_trans_UIItemInfoGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemInfoGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_tipsContent = fastComponent.FastGetComponent<Transform>("tipsContent");
       if( null == m_trans_tipsContent )
       {
            Engine.Utility.Log.Error("m_trans_tipsContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_tipsContentLbl = fastComponent.FastGetComponent<UILabel>("tipsContentLbl");
       if( null == m_label_tipsContentLbl )
       {
            Engine.Utility.Log.Error("m_label_tipsContentLbl 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIdailytestgrid = fastComponent.FastGetComponent<Transform>("UIdailytestgrid");
       if( null == m_trans_UIdailytestgrid )
       {
            Engine.Utility.Log.Error("m_trans_UIdailytestgrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_Consume.gameObject).onClick = _onClick_Btn_Consume_Btn;
        UIEventListener.Get(m_btn_btn_Tips.gameObject).onClick = _onClick_Btn_Tips_Btn;
    }

    void _onClick_Btn_Consume_Btn(GameObject caster)
    {
        onClick_Btn_Consume_Btn( caster );
    }

    void _onClick_Btn_Tips_Btn(GameObject caster)
    {
        onClick_Btn_Tips_Btn( caster );
    }


}
