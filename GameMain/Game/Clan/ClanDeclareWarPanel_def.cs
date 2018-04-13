//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ClanDeclareWarPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    Transform            m_trans_ToggleContent;

    UIGridCreatorBase    m_ctor_SearchScrollView;

    UIInput              m_input_Input;

    UIButton             m_btn_BtnSearch;

    UIGridCreatorBase    m_ctor_HistoryScrollView;


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
        m_trans_ToggleContent = fastComponent.FastGetComponent<Transform>("ToggleContent");
       if( null == m_trans_ToggleContent )
       {
            Engine.Utility.Log.Error("m_trans_ToggleContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_SearchScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("SearchScrollView");
       if( null == m_ctor_SearchScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_SearchScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_input_Input = fastComponent.FastGetComponent<UIInput>("Input");
       if( null == m_input_Input )
       {
            Engine.Utility.Log.Error("m_input_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSearch = fastComponent.FastGetComponent<UIButton>("BtnSearch");
       if( null == m_btn_BtnSearch )
       {
            Engine.Utility.Log.Error("m_btn_BtnSearch 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_HistoryScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("HistoryScrollView");
       if( null == m_ctor_HistoryScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_HistoryScrollView 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_BtnSearch.gameObject).onClick = _onClick_BtnSearch_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_BtnSearch_Btn(GameObject caster)
    {
        onClick_BtnSearch_Btn( caster );
    }


}
