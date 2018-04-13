//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RedEnvelopeRecordPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_Close;

    UIButton             m_btn_btn_confirm;

    UILabel              m_label_takeNum_label;

    UILabel              m_label_takeNumber_label;

    UILabel              m_label_sendNumber_label;

    UILabel              m_label_sendNum_label;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_Close = fastComponent.FastGetComponent<UIButton>("btn_Close");
       if( null == m_btn_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_confirm = fastComponent.FastGetComponent<UIButton>("btn_confirm");
       if( null == m_btn_btn_confirm )
       {
            Engine.Utility.Log.Error("m_btn_btn_confirm 为空，请检查prefab是否缺乏组件");
       }
        m_label_takeNum_label = fastComponent.FastGetComponent<UILabel>("takeNum_label");
       if( null == m_label_takeNum_label )
       {
            Engine.Utility.Log.Error("m_label_takeNum_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_takeNumber_label = fastComponent.FastGetComponent<UILabel>("takeNumber_label");
       if( null == m_label_takeNumber_label )
       {
            Engine.Utility.Log.Error("m_label_takeNumber_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_sendNumber_label = fastComponent.FastGetComponent<UILabel>("sendNumber_label");
       if( null == m_label_sendNumber_label )
       {
            Engine.Utility.Log.Error("m_label_sendNumber_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_sendNum_label = fastComponent.FastGetComponent<UILabel>("sendNum_label");
       if( null == m_label_sendNum_label )
       {
            Engine.Utility.Log.Error("m_label_sendNum_label 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_Close.gameObject).onClick = _onClick_Btn_Close_Btn;
        UIEventListener.Get(m_btn_btn_confirm.gameObject).onClick = _onClick_Btn_confirm_Btn;
    }

    void _onClick_Btn_Close_Btn(GameObject caster)
    {
        onClick_Btn_Close_Btn( caster );
    }

    void _onClick_Btn_confirm_Btn(GameObject caster)
    {
        onClick_Btn_confirm_Btn( caster );
    }


}
