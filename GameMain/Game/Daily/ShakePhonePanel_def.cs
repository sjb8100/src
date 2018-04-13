//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ShakePhonePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    UIButton             m_btn_shakeBtn;


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
        m_btn_shakeBtn = fastComponent.FastGetComponent<UIButton>("shakeBtn");
       if( null == m_btn_shakeBtn )
       {
            Engine.Utility.Log.Error("m_btn_shakeBtn 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_shakeBtn.gameObject).onClick = _onClick_ShakeBtn_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_ShakeBtn_Btn(GameObject caster)
    {
        onClick_ShakeBtn_Btn( caster );
    }


}
