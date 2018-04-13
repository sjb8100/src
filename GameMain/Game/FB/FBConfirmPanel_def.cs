//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FBConfirmPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_FB_name;

    Transform            m_trans_Member;

    UISlider             m_slider_Countdownslider;

    UILabel              m_label_slidertime;

    UIButton             m_btn_btn_queding;

    UIButton             m_btn_btn_quxiao;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_FB_name = fastComponent.FastGetComponent<UILabel>("FB_name");
       if( null == m_label_FB_name )
       {
            Engine.Utility.Log.Error("m_label_FB_name 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Member = fastComponent.FastGetComponent<Transform>("Member");
       if( null == m_trans_Member )
       {
            Engine.Utility.Log.Error("m_trans_Member 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Countdownslider = fastComponent.FastGetComponent<UISlider>("Countdownslider");
       if( null == m_slider_Countdownslider )
       {
            Engine.Utility.Log.Error("m_slider_Countdownslider 为空，请检查prefab是否缺乏组件");
       }
        m_label_slidertime = fastComponent.FastGetComponent<UILabel>("slidertime");
       if( null == m_label_slidertime )
       {
            Engine.Utility.Log.Error("m_label_slidertime 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_queding = fastComponent.FastGetComponent<UIButton>("btn_queding");
       if( null == m_btn_btn_queding )
       {
            Engine.Utility.Log.Error("m_btn_btn_queding 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_quxiao = fastComponent.FastGetComponent<UIButton>("btn_quxiao");
       if( null == m_btn_btn_quxiao )
       {
            Engine.Utility.Log.Error("m_btn_btn_quxiao 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_btn_queding.gameObject).onClick = _onClick_Btn_queding_Btn;
        UIEventListener.Get(m_btn_btn_quxiao.gameObject).onClick = _onClick_Btn_quxiao_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Btn_queding_Btn(GameObject caster)
    {
        onClick_Btn_queding_Btn( caster );
    }

    void _onClick_Btn_quxiao_Btn(GameObject caster)
    {
        onClick_Btn_quxiao_Btn( caster );
    }


}
