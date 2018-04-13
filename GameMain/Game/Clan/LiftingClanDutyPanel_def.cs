//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class LiftingClanDutyPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_TitleText;

    UIButton             m_btn_BtnCancel;

    UILabel              m_label_CancelText;

    Transform            m_trans_ContentArea;

    UIButton             m_btn_BtnTransferZZ;

    UIButton             m_btn_BtnAppointFZZ;

    UIButton             m_btn_BtnAppointNormal;


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
        m_btn_BtnCancel = fastComponent.FastGetComponent<UIButton>("BtnCancel");
       if( null == m_btn_BtnCancel )
       {
            Engine.Utility.Log.Error("m_btn_BtnCancel 为空，请检查prefab是否缺乏组件");
       }
        m_label_CancelText = fastComponent.FastGetComponent<UILabel>("CancelText");
       if( null == m_label_CancelText )
       {
            Engine.Utility.Log.Error("m_label_CancelText 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ContentArea = fastComponent.FastGetComponent<Transform>("ContentArea");
       if( null == m_trans_ContentArea )
       {
            Engine.Utility.Log.Error("m_trans_ContentArea 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnTransferZZ = fastComponent.FastGetComponent<UIButton>("BtnTransferZZ");
       if( null == m_btn_BtnTransferZZ )
       {
            Engine.Utility.Log.Error("m_btn_BtnTransferZZ 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnAppointFZZ = fastComponent.FastGetComponent<UIButton>("BtnAppointFZZ");
       if( null == m_btn_BtnAppointFZZ )
       {
            Engine.Utility.Log.Error("m_btn_BtnAppointFZZ 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnAppointNormal = fastComponent.FastGetComponent<UIButton>("BtnAppointNormal");
       if( null == m_btn_BtnAppointNormal )
       {
            Engine.Utility.Log.Error("m_btn_BtnAppointNormal 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_BtnCancel.gameObject).onClick = _onClick_BtnCancel_Btn;
        UIEventListener.Get(m_btn_BtnTransferZZ.gameObject).onClick = _onClick_BtnTransferZZ_Btn;
        UIEventListener.Get(m_btn_BtnAppointFZZ.gameObject).onClick = _onClick_BtnAppointFZZ_Btn;
        UIEventListener.Get(m_btn_BtnAppointNormal.gameObject).onClick = _onClick_BtnAppointNormal_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_BtnCancel_Btn(GameObject caster)
    {
        onClick_BtnCancel_Btn( caster );
    }

    void _onClick_BtnTransferZZ_Btn(GameObject caster)
    {
        onClick_BtnTransferZZ_Btn( caster );
    }

    void _onClick_BtnAppointFZZ_Btn(GameObject caster)
    {
        onClick_BtnAppointFZZ_Btn( caster );
    }

    void _onClick_BtnAppointNormal_Btn(GameObject caster)
    {
        onClick_BtnAppointNormal_Btn( caster );
    }


}
