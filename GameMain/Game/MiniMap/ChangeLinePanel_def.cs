//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ChangeLinePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_BiaoTi_Label;

    UIButton             m_btn_btn_Close;

    UIGridCreatorBase    m_ctor_LineScrollView;

    Transform            m_trans_UIChangeLineGrid;

    Transform            m_trans_Sigh;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_BiaoTi_Label = fastComponent.FastGetComponent<UILabel>("BiaoTi_Label");
       if( null == m_label_BiaoTi_Label )
       {
            Engine.Utility.Log.Error("m_label_BiaoTi_Label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Close = fastComponent.FastGetComponent<UIButton>("btn_Close");
       if( null == m_btn_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_LineScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("LineScrollView");
       if( null == m_ctor_LineScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_LineScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIChangeLineGrid = fastComponent.FastGetComponent<Transform>("UIChangeLineGrid");
       if( null == m_trans_UIChangeLineGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIChangeLineGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Sigh = fastComponent.FastGetComponent<Transform>("Sigh");
       if( null == m_trans_Sigh )
       {
            Engine.Utility.Log.Error("m_trans_Sigh 为空，请检查prefab是否缺乏组件");
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
    }

    void _onClick_Btn_Close_Btn(GameObject caster)
    {
        onClick_Btn_Close_Btn( caster );
    }


}
