//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class VoiceSetting: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_call;

    UILabel              m_label_timeLabel;

    UIButton             m_btn_close;

    UISlider             m_slider_slidertop;

    UISlider             m_slider_sliderbuttom;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_call = fastComponent.FastGetComponent<UIButton>("call");
       if( null == m_btn_call )
       {
            Engine.Utility.Log.Error("m_btn_call 为空，请检查prefab是否缺乏组件");
       }
        m_label_timeLabel = fastComponent.FastGetComponent<UILabel>("timeLabel");
       if( null == m_label_timeLabel )
       {
            Engine.Utility.Log.Error("m_label_timeLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_slider_slidertop = fastComponent.FastGetComponent<UISlider>("slidertop");
       if( null == m_slider_slidertop )
       {
            Engine.Utility.Log.Error("m_slider_slidertop 为空，请检查prefab是否缺乏组件");
       }
        m_slider_sliderbuttom = fastComponent.FastGetComponent<UISlider>("sliderbuttom");
       if( null == m_slider_sliderbuttom )
       {
            Engine.Utility.Log.Error("m_slider_sliderbuttom 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_call.gameObject).onClick = _onClick_Call_Btn;
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Call_Btn(GameObject caster)
    {
        onClick_Call_Btn( caster );
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
