//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class InvitePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_TitleText;

    UIButton             m_btn_close;

    Transform            m_trans_ContentArea;

    Transform            m_trans_ScrollView;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_TitleText = fastComponent.FastGetComponent<UILabel>("TitleText");
       if( null == m_label_TitleText )
       {
            Engine.Utility.Log.Error("m_label_TitleText 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ContentArea = fastComponent.FastGetComponent<Transform>("ContentArea");
       if( null == m_trans_ContentArea )
       {
            Engine.Utility.Log.Error("m_trans_ContentArea 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ScrollView = fastComponent.FastGetComponent<Transform>("ScrollView");
       if( null == m_trans_ScrollView )
       {
            Engine.Utility.Log.Error("m_trans_ScrollView 为空，请检查prefab是否缺乏组件");
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
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
