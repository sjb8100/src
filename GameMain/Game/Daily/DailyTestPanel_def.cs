//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class DailyTestPanel: UIPanelBase
{

    UIButton      m_btn_btn_Consume;

    UIButton      m_btn_btn_Start;

    UILabel       m_label_lastTime_label;

    UISlider      m_slider_Expslider;

    UILabel       m_label_exp_percent;

    UIButton      m_btn_btn_Tips;


    //初始化控件变量
    protected override void InitControls()
    {
        m_btn_btn_Consume = GetChildComponent<UIButton>("btn_Consume");
       if( null == m_btn_btn_Consume )
       {
            Engine.Utility.Log.Error("m_btn_btn_Consume 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Start = GetChildComponent<UIButton>("btn_Start");
       if( null == m_btn_btn_Start )
       {
            Engine.Utility.Log.Error("m_btn_btn_Start 为空，请检查prefab是否缺乏组件");
       }
        m_label_lastTime_label = GetChildComponent<UILabel>("lastTime_label");
       if( null == m_label_lastTime_label )
       {
            Engine.Utility.Log.Error("m_label_lastTime_label 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Expslider = GetChildComponent<UISlider>("Expslider");
       if( null == m_slider_Expslider )
       {
            Engine.Utility.Log.Error("m_slider_Expslider 为空，请检查prefab是否缺乏组件");
       }
        m_label_exp_percent = GetChildComponent<UILabel>("exp_percent");
       if( null == m_label_exp_percent )
       {
            Engine.Utility.Log.Error("m_label_exp_percent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Tips = GetChildComponent<UIButton>("btn_Tips");
       if( null == m_btn_btn_Tips )
       {
            Engine.Utility.Log.Error("m_btn_btn_Tips 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_Consume.gameObject).onClick = _onClick_Btn_Consume_Btn;
        UIEventListener.Get(m_btn_btn_Start.gameObject).onClick = _onClick_Btn_Start_Btn;
        UIEventListener.Get(m_btn_btn_Tips.gameObject).onClick = _onClick_Btn_Tips_Btn;
    }

    void _onClick_Btn_Consume_Btn(GameObject caster)
    {
        onClick_Btn_Consume_Btn( caster );
    }

    void _onClick_Btn_Start_Btn(GameObject caster)
    {
        onClick_Btn_Start_Btn( caster );
    }

    void _onClick_Btn_Tips_Btn(GameObject caster)
    {
        onClick_Btn_Tips_Btn( caster );
    }


}
