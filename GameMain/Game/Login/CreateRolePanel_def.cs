//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CreateRolePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_Container;

    Transform            m_trans_LeftInfos;

    UIButton             m_btn_return;

    Transform            m_trans_career_list;

    Transform            m_trans_RightInfos;

    UIWidget             m_widget_RightDesWiget;

    UIInput              m_input_input_name;

    UIButton             m_btn_random_name;

    UIButton             m_btn_create;

    Transform            m_trans_UICreateRoleGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_Container = fastComponent.FastGetComponent<UIWidget>("Container");
       if( null == m_widget_Container )
       {
            Engine.Utility.Log.Error("m_widget_Container 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LeftInfos = fastComponent.FastGetComponent<Transform>("LeftInfos");
       if( null == m_trans_LeftInfos )
       {
            Engine.Utility.Log.Error("m_trans_LeftInfos 为空，请检查prefab是否缺乏组件");
       }
        m_btn_return = fastComponent.FastGetComponent<UIButton>("return");
       if( null == m_btn_return )
       {
            Engine.Utility.Log.Error("m_btn_return 为空，请检查prefab是否缺乏组件");
       }
        m_trans_career_list = fastComponent.FastGetComponent<Transform>("career_list");
       if( null == m_trans_career_list )
       {
            Engine.Utility.Log.Error("m_trans_career_list 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RightInfos = fastComponent.FastGetComponent<Transform>("RightInfos");
       if( null == m_trans_RightInfos )
       {
            Engine.Utility.Log.Error("m_trans_RightInfos 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RightDesWiget = fastComponent.FastGetComponent<UIWidget>("RightDesWiget");
       if( null == m_widget_RightDesWiget )
       {
            Engine.Utility.Log.Error("m_widget_RightDesWiget 为空，请检查prefab是否缺乏组件");
       }
        m_input_input_name = fastComponent.FastGetComponent<UIInput>("input_name");
       if( null == m_input_input_name )
       {
            Engine.Utility.Log.Error("m_input_input_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_random_name = fastComponent.FastGetComponent<UIButton>("random_name");
       if( null == m_btn_random_name )
       {
            Engine.Utility.Log.Error("m_btn_random_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_create = fastComponent.FastGetComponent<UIButton>("create");
       if( null == m_btn_create )
       {
            Engine.Utility.Log.Error("m_btn_create 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UICreateRoleGrid = fastComponent.FastGetComponent<Transform>("UICreateRoleGrid");
       if( null == m_trans_UICreateRoleGrid )
       {
            Engine.Utility.Log.Error("m_trans_UICreateRoleGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_return.gameObject).onClick = _onClick_Return_Btn;
        UIEventListener.Get(m_btn_random_name.gameObject).onClick = _onClick_Random_name_Btn;
        UIEventListener.Get(m_btn_create.gameObject).onClick = _onClick_Create_Btn;
    }

    void _onClick_Return_Btn(GameObject caster)
    {
        onClick_Return_Btn( caster );
    }

    void _onClick_Random_name_Btn(GameObject caster)
    {
        onClick_Random_name_Btn( caster );
    }

    void _onClick_Create_Btn(GameObject caster)
    {
        onClick_Create_Btn( caster );
    }


}
