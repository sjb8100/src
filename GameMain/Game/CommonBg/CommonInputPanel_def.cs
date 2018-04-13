//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CommonInputPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_TitleText;

    UIButton             m_btn_BtnConfirm;

    UILabel              m_label_ConfirmText;

    UIButton             m_btn_BtnEM;

    UIInput              m_input_ContentArea;

    UILabel              m_label_InputTips;


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
        m_label_TitleText = fastComponent.FastGetComponent<UILabel>("TitleText");
       if( null == m_label_TitleText )
       {
            Engine.Utility.Log.Error("m_label_TitleText 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnConfirm = fastComponent.FastGetComponent<UIButton>("BtnConfirm");
       if( null == m_btn_BtnConfirm )
       {
            Engine.Utility.Log.Error("m_btn_BtnConfirm 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConfirmText = fastComponent.FastGetComponent<UILabel>("ConfirmText");
       if( null == m_label_ConfirmText )
       {
            Engine.Utility.Log.Error("m_label_ConfirmText 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnEM = fastComponent.FastGetComponent<UIButton>("BtnEM");
       if( null == m_btn_BtnEM )
       {
            Engine.Utility.Log.Error("m_btn_BtnEM 为空，请检查prefab是否缺乏组件");
       }
        m_input_ContentArea = fastComponent.FastGetComponent<UIInput>("ContentArea");
       if( null == m_input_ContentArea )
       {
            Engine.Utility.Log.Error("m_input_ContentArea 为空，请检查prefab是否缺乏组件");
       }
        m_label_InputTips = fastComponent.FastGetComponent<UILabel>("InputTips");
       if( null == m_label_InputTips )
       {
            Engine.Utility.Log.Error("m_label_InputTips 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_BtnConfirm.gameObject).onClick = _onClick_BtnConfirm_Btn;
        UIEventListener.Get(m_btn_BtnEM.gameObject).onClick = _onClick_BtnEM_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_BtnConfirm_Btn(GameObject caster)
    {
        onClick_BtnConfirm_Btn( caster );
    }

    void _onClick_BtnEM_Btn(GameObject caster)
    {
        onClick_BtnEM_Btn( caster );
    }


}
