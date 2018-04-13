//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ChooseRolePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UITexture            m__bg;

    UIWidget             m_widget_Container;

    Transform            m_trans_RightInfos;

    UIButton             m_btn_BtnDelete;

    UIWidget             m_widget_DesWidget;

    UIButton             m_btn_enter;

    Transform            m_trans_LeftInfos;

    UIButton             m_btn_backBtn;

    Transform            m_trans_role_list;

    Transform            m_trans_UISelectRoleGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m__bg = fastComponent.FastGetComponent<UITexture>("bg");
       if( null == m__bg )
       {
            Engine.Utility.Log.Error("m__bg 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Container = fastComponent.FastGetComponent<UIWidget>("Container");
       if( null == m_widget_Container )
       {
            Engine.Utility.Log.Error("m_widget_Container 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RightInfos = fastComponent.FastGetComponent<Transform>("RightInfos");
       if( null == m_trans_RightInfos )
       {
            Engine.Utility.Log.Error("m_trans_RightInfos 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnDelete = fastComponent.FastGetComponent<UIButton>("BtnDelete");
       if( null == m_btn_BtnDelete )
       {
            Engine.Utility.Log.Error("m_btn_BtnDelete 为空，请检查prefab是否缺乏组件");
       }
        m_widget_DesWidget = fastComponent.FastGetComponent<UIWidget>("DesWidget");
       if( null == m_widget_DesWidget )
       {
            Engine.Utility.Log.Error("m_widget_DesWidget 为空，请检查prefab是否缺乏组件");
       }
        m_btn_enter = fastComponent.FastGetComponent<UIButton>("enter");
       if( null == m_btn_enter )
       {
            Engine.Utility.Log.Error("m_btn_enter 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LeftInfos = fastComponent.FastGetComponent<Transform>("LeftInfos");
       if( null == m_trans_LeftInfos )
       {
            Engine.Utility.Log.Error("m_trans_LeftInfos 为空，请检查prefab是否缺乏组件");
       }
        m_btn_backBtn = fastComponent.FastGetComponent<UIButton>("backBtn");
       if( null == m_btn_backBtn )
       {
            Engine.Utility.Log.Error("m_btn_backBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_role_list = fastComponent.FastGetComponent<Transform>("role_list");
       if( null == m_trans_role_list )
       {
            Engine.Utility.Log.Error("m_trans_role_list 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UISelectRoleGrid = fastComponent.FastGetComponent<Transform>("UISelectRoleGrid");
       if( null == m_trans_UISelectRoleGrid )
       {
            Engine.Utility.Log.Error("m_trans_UISelectRoleGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BtnDelete.gameObject).onClick = _onClick_BtnDelete_Btn;
        UIEventListener.Get(m_btn_enter.gameObject).onClick = _onClick_Enter_Btn;
        UIEventListener.Get(m_btn_backBtn.gameObject).onClick = _onClick_BackBtn_Btn;
    }

    void _onClick_BtnDelete_Btn(GameObject caster)
    {
        onClick_BtnDelete_Btn( caster );
    }

    void _onClick_Enter_Btn(GameObject caster)
    {
        onClick_Enter_Btn( caster );
    }

    void _onClick_BackBtn_Btn(GameObject caster)
    {
        onClick_BackBtn_Btn( caster );
    }


}
