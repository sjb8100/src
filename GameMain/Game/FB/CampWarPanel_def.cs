//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CampWarPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_Close;

    UILabel              m_label_LvSection;

    UILabel              m_label_name;

    UIGridCreatorBase    m_ctor_SignScrollView;

    UIButton             m_btn_BtnRefresh;

    UILabel              m_label_LeftNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_Close = fastComponent.FastGetComponent<UIButton>("Close");
       if( null == m_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_label_LvSection = fastComponent.FastGetComponent<UILabel>("LvSection");
       if( null == m_label_LvSection )
       {
            Engine.Utility.Log.Error("m_label_LvSection 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_SignScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("SignScrollView");
       if( null == m_ctor_SignScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_SignScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnRefresh = fastComponent.FastGetComponent<UIButton>("BtnRefresh");
       if( null == m_btn_BtnRefresh )
       {
            Engine.Utility.Log.Error("m_btn_BtnRefresh 为空，请检查prefab是否缺乏组件");
       }
        m_label_LeftNum = fastComponent.FastGetComponent<UILabel>("LeftNum");
       if( null == m_label_LeftNum )
       {
            Engine.Utility.Log.Error("m_label_LeftNum 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_BtnRefresh.gameObject).onClick = _onClick_BtnRefresh_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_BtnRefresh_Btn(GameObject caster)
    {
        onClick_BtnRefresh_Btn( caster );
    }


}
