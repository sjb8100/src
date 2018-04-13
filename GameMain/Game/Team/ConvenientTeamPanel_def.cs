//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ConvenientTeamPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIScrollView         m_scrollview_TargetScrollView;

    UITable              m__uitable;

    UIButton             m_btn_btn_refresh;

    UIButton             m_btn_btn_automatch;

    UILabel              m_label_btn_automatchLabel;

    UIButton             m_btn_btn_createteam;

    UIButton             m_btn_btn_changeTarget;

    Transform            m_trans_TeamList;

    Transform            m_trans_TeamListScrollView;

    UIWidget             m_widget_UITeamActivityChildGrid;

    Transform            m_trans_UITitleCtrTypeGrid;

    UISprite             m_sprite_UIExistedTeamGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_scrollview_TargetScrollView = fastComponent.FastGetComponent<UIScrollView>("TargetScrollView");
       if( null == m_scrollview_TargetScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_TargetScrollView 为空，请检查prefab是否缺乏组件");
       }
        m__uitable = fastComponent.FastGetComponent<UITable>("uitable");
       if( null == m__uitable )
       {
            Engine.Utility.Log.Error("m__uitable 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_refresh = fastComponent.FastGetComponent<UIButton>("btn_refresh");
       if( null == m_btn_btn_refresh )
       {
            Engine.Utility.Log.Error("m_btn_btn_refresh 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_automatch = fastComponent.FastGetComponent<UIButton>("btn_automatch");
       if( null == m_btn_btn_automatch )
       {
            Engine.Utility.Log.Error("m_btn_btn_automatch 为空，请检查prefab是否缺乏组件");
       }
        m_label_btn_automatchLabel = fastComponent.FastGetComponent<UILabel>("btn_automatchLabel");
       if( null == m_label_btn_automatchLabel )
       {
            Engine.Utility.Log.Error("m_label_btn_automatchLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_createteam = fastComponent.FastGetComponent<UIButton>("btn_createteam");
       if( null == m_btn_btn_createteam )
       {
            Engine.Utility.Log.Error("m_btn_btn_createteam 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_changeTarget = fastComponent.FastGetComponent<UIButton>("btn_changeTarget");
       if( null == m_btn_btn_changeTarget )
       {
            Engine.Utility.Log.Error("m_btn_btn_changeTarget 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TeamList = fastComponent.FastGetComponent<Transform>("TeamList");
       if( null == m_trans_TeamList )
       {
            Engine.Utility.Log.Error("m_trans_TeamList 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TeamListScrollView = fastComponent.FastGetComponent<Transform>("TeamListScrollView");
       if( null == m_trans_TeamListScrollView )
       {
            Engine.Utility.Log.Error("m_trans_TeamListScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_widget_UITeamActivityChildGrid = fastComponent.FastGetComponent<UIWidget>("UITeamActivityChildGrid");
       if( null == m_widget_UITeamActivityChildGrid )
       {
            Engine.Utility.Log.Error("m_widget_UITeamActivityChildGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITitleCtrTypeGrid = fastComponent.FastGetComponent<Transform>("UITitleCtrTypeGrid");
       if( null == m_trans_UITitleCtrTypeGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITitleCtrTypeGrid 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UIExistedTeamGrid = fastComponent.FastGetComponent<UISprite>("UIExistedTeamGrid");
       if( null == m_sprite_UIExistedTeamGrid )
       {
            Engine.Utility.Log.Error("m_sprite_UIExistedTeamGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_refresh.gameObject).onClick = _onClick_Btn_refresh_Btn;
        UIEventListener.Get(m_btn_btn_automatch.gameObject).onClick = _onClick_Btn_automatch_Btn;
        UIEventListener.Get(m_btn_btn_createteam.gameObject).onClick = _onClick_Btn_createteam_Btn;
        UIEventListener.Get(m_btn_btn_changeTarget.gameObject).onClick = _onClick_Btn_changeTarget_Btn;
    }

    void _onClick_Btn_refresh_Btn(GameObject caster)
    {
        onClick_Btn_refresh_Btn( caster );
    }

    void _onClick_Btn_automatch_Btn(GameObject caster)
    {
        onClick_Btn_automatch_Btn( caster );
    }

    void _onClick_Btn_createteam_Btn(GameObject caster)
    {
        onClick_Btn_createteam_Btn( caster );
    }

    void _onClick_Btn_changeTarget_Btn(GameObject caster)
    {
        onClick_Btn_changeTarget_Btn( caster );
    }


}
