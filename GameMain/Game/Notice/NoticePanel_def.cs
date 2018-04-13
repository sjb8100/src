//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class NoticePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UIButton             m_btn_Confirm;

    UILabel              m_label_ConfirmText;

    UIButton             m_btn_Cancel;

    UILabel              m_label_CancelText;

    UIButton             m_btn_Center;

    UILabel              m_label_CenterText;

    UILabel              m_label_Message;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_Content = fastComponent.FastGetComponent<Transform>("Content");
       if( null == m_trans_Content )
       {
            Engine.Utility.Log.Error("m_trans_Content 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Confirm = fastComponent.FastGetComponent<UIButton>("Confirm");
       if( null == m_btn_Confirm )
       {
            Engine.Utility.Log.Error("m_btn_Confirm 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConfirmText = fastComponent.FastGetComponent<UILabel>("ConfirmText");
       if( null == m_label_ConfirmText )
       {
            Engine.Utility.Log.Error("m_label_ConfirmText 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Cancel = fastComponent.FastGetComponent<UIButton>("Cancel");
       if( null == m_btn_Cancel )
       {
            Engine.Utility.Log.Error("m_btn_Cancel 为空，请检查prefab是否缺乏组件");
       }
        m_label_CancelText = fastComponent.FastGetComponent<UILabel>("CancelText");
       if( null == m_label_CancelText )
       {
            Engine.Utility.Log.Error("m_label_CancelText 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Center = fastComponent.FastGetComponent<UIButton>("Center");
       if( null == m_btn_Center )
       {
            Engine.Utility.Log.Error("m_btn_Center 为空，请检查prefab是否缺乏组件");
       }
        m_label_CenterText = fastComponent.FastGetComponent<UILabel>("CenterText");
       if( null == m_label_CenterText )
       {
            Engine.Utility.Log.Error("m_label_CenterText 为空，请检查prefab是否缺乏组件");
       }
        m_label_Message = fastComponent.FastGetComponent<UILabel>("Message");
       if( null == m_label_Message )
       {
            Engine.Utility.Log.Error("m_label_Message 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Confirm.gameObject).onClick = _onClick_Confirm_Btn;
        UIEventListener.Get(m_btn_Cancel.gameObject).onClick = _onClick_Cancel_Btn;
        UIEventListener.Get(m_btn_Center.gameObject).onClick = _onClick_Center_Btn;
    }

    void _onClick_Confirm_Btn(GameObject caster)
    {
        onClick_Confirm_Btn( caster );
    }

    void _onClick_Cancel_Btn(GameObject caster)
    {
        onClick_Cancel_Btn( caster );
    }

    void _onClick_Center_Btn(GameObject caster)
    {
        onClick_Center_Btn( caster );
    }


}
