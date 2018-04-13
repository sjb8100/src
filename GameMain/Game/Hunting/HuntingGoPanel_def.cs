//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class HuntingGoPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    UILabel              m_label_Label;

    UILabel              m_label_Frequency;

    UILabel              m_label_RemindLabel;

    UIButton             m_btn_Btn_move;

    UIButton             m_btn_Btn_transmission;


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
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_label_Frequency = fastComponent.FastGetComponent<UILabel>("Frequency");
       if( null == m_label_Frequency )
       {
            Engine.Utility.Log.Error("m_label_Frequency 为空，请检查prefab是否缺乏组件");
       }
        m_label_RemindLabel = fastComponent.FastGetComponent<UILabel>("RemindLabel");
       if( null == m_label_RemindLabel )
       {
            Engine.Utility.Log.Error("m_label_RemindLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_move = fastComponent.FastGetComponent<UIButton>("Btn_move");
       if( null == m_btn_Btn_move )
       {
            Engine.Utility.Log.Error("m_btn_Btn_move 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_transmission = fastComponent.FastGetComponent<UIButton>("Btn_transmission");
       if( null == m_btn_Btn_transmission )
       {
            Engine.Utility.Log.Error("m_btn_Btn_transmission 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_Btn_move.gameObject).onClick = _onClick_Btn_move_Btn;
        UIEventListener.Get(m_btn_Btn_transmission.gameObject).onClick = _onClick_Btn_transmission_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Btn_move_Btn(GameObject caster)
    {
        onClick_Btn_move_Btn( caster );
    }

    void _onClick_Btn_transmission_Btn(GameObject caster)
    {
        onClick_Btn_transmission_Btn( caster );
    }


}
