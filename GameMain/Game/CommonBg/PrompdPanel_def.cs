//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PrompdPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    UILabel              m_label_content_Label;

    UIButton             m_btn_quxiao;

    UILabel              m_label_quxiao_Name;

    UIButton             m_btn_queding;

    UILabel              m_label_queding_Name;


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
        m_label_content_Label = fastComponent.FastGetComponent<UILabel>("content_Label");
       if( null == m_label_content_Label )
       {
            Engine.Utility.Log.Error("m_label_content_Label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_quxiao = fastComponent.FastGetComponent<UIButton>("quxiao");
       if( null == m_btn_quxiao )
       {
            Engine.Utility.Log.Error("m_btn_quxiao 为空，请检查prefab是否缺乏组件");
       }
        m_label_quxiao_Name = fastComponent.FastGetComponent<UILabel>("quxiao_Name");
       if( null == m_label_quxiao_Name )
       {
            Engine.Utility.Log.Error("m_label_quxiao_Name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_queding = fastComponent.FastGetComponent<UIButton>("queding");
       if( null == m_btn_queding )
       {
            Engine.Utility.Log.Error("m_btn_queding 为空，请检查prefab是否缺乏组件");
       }
        m_label_queding_Name = fastComponent.FastGetComponent<UILabel>("queding_Name");
       if( null == m_label_queding_Name )
       {
            Engine.Utility.Log.Error("m_label_queding_Name 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_quxiao.gameObject).onClick = _onClick_Quxiao_Btn;
        UIEventListener.Get(m_btn_queding.gameObject).onClick = _onClick_Queding_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Quxiao_Btn(GameObject caster)
    {
        onClick_Quxiao_Btn( caster );
    }

    void _onClick_Queding_Btn(GameObject caster)
    {
        onClick_Queding_Btn( caster );
    }


}
