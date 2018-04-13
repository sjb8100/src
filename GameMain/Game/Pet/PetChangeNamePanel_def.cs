//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetChangeNamePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_gaiming_close;

    UILabel              m_label_name;

    UIInput              m_input_input;

    UIButton             m_btn_gaiming_quxiao;

    UIButton             m_btn_gaiming_queding;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_gaiming_close = fastComponent.FastGetComponent<UIButton>("gaiming_close");
       if( null == m_btn_gaiming_close )
       {
            Engine.Utility.Log.Error("m_btn_gaiming_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_input_input = fastComponent.FastGetComponent<UIInput>("input");
       if( null == m_input_input )
       {
            Engine.Utility.Log.Error("m_input_input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_gaiming_quxiao = fastComponent.FastGetComponent<UIButton>("gaiming_quxiao");
       if( null == m_btn_gaiming_quxiao )
       {
            Engine.Utility.Log.Error("m_btn_gaiming_quxiao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_gaiming_queding = fastComponent.FastGetComponent<UIButton>("gaiming_queding");
       if( null == m_btn_gaiming_queding )
       {
            Engine.Utility.Log.Error("m_btn_gaiming_queding 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_gaiming_close.gameObject).onClick = _onClick_Gaiming_close_Btn;
        UIEventListener.Get(m_btn_gaiming_quxiao.gameObject).onClick = _onClick_Gaiming_quxiao_Btn;
        UIEventListener.Get(m_btn_gaiming_queding.gameObject).onClick = _onClick_Gaiming_queding_Btn;
    }

    void _onClick_Gaiming_close_Btn(GameObject caster)
    {
        onClick_Gaiming_close_Btn( caster );
    }

    void _onClick_Gaiming_quxiao_Btn(GameObject caster)
    {
        onClick_Gaiming_quxiao_Btn( caster );
    }

    void _onClick_Gaiming_queding_Btn(GameObject caster)
    {
        onClick_Gaiming_queding_Btn( caster );
    }


}
