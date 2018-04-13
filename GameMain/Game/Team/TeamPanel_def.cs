//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TeamPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//队伍
		DuiWu = 1,
		//申请
		ShengQing = 2,
		Max,
    }

   FastComponent         fastComponent;

    UIWidget             m_widget_myteamPanel;

    UIScrollView         m_scrollview_scrollView;

    UIGrid               m_grid_grid;

    Transform            m_trans_Member_01;

    Transform            m_trans_Member_02;

    Transform            m_trans_Member_03;

    Transform            m_trans_Member_04;

    Transform            m_trans_Member_05;

    Transform            m_trans_up;

    UIButton             m_btn_bg_target;

    UILabel              m_label_Label;

    UILabel              m_label_index_name;

    UIButton             m_btn_btn_talk;

    UIButton             m_btn_btn_pickup;

    UILabel              m_label_itemModeLbl;

    UISprite             m_sprite_pickuppanle;

    UIButton             m_btn_btn_free;

    UIButton             m_btn_btn_distribution;

    UIButton             m_btn_btn_pickuppanleClose;

    UIButton             m_btn_btn_leaveteam;

    UIButton             m_btn_btn_disbandteam;

    UIButton             m_btn_btn_match;

    UIButton             m_btn_btn_cancelMatch;

    UIButton             m_btn_btn_enter;

    UIWidget             m_widget_applyListPanel;

    UIGrid               m_grid_applyListContent;

    Transform            m_trans_Apply_01;

    Transform            m_trans_Apply_02;

    Transform            m_trans_Apply_03;

    Transform            m_trans_Apply_04;

    Transform            m_trans_Apply_05;

    UIGrid               m_grid_applyListbg;

    UIButton             m_btn_btn_previouspage;

    UIButton             m_btn_btn_nextpage;

    UIButton             m_btn_btn_clean;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_myteamPanel = fastComponent.FastGetComponent<UIWidget>("myteamPanel");
       if( null == m_widget_myteamPanel )
       {
            Engine.Utility.Log.Error("m_widget_myteamPanel 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_scrollView = fastComponent.FastGetComponent<UIScrollView>("scrollView");
       if( null == m_scrollview_scrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_scrollView 为空，请检查prefab是否缺乏组件");
       }
        m_grid_grid = fastComponent.FastGetComponent<UIGrid>("grid");
       if( null == m_grid_grid )
       {
            Engine.Utility.Log.Error("m_grid_grid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Member_01 = fastComponent.FastGetComponent<Transform>("Member_01");
       if( null == m_trans_Member_01 )
       {
            Engine.Utility.Log.Error("m_trans_Member_01 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Member_02 = fastComponent.FastGetComponent<Transform>("Member_02");
       if( null == m_trans_Member_02 )
       {
            Engine.Utility.Log.Error("m_trans_Member_02 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Member_03 = fastComponent.FastGetComponent<Transform>("Member_03");
       if( null == m_trans_Member_03 )
       {
            Engine.Utility.Log.Error("m_trans_Member_03 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Member_04 = fastComponent.FastGetComponent<Transform>("Member_04");
       if( null == m_trans_Member_04 )
       {
            Engine.Utility.Log.Error("m_trans_Member_04 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Member_05 = fastComponent.FastGetComponent<Transform>("Member_05");
       if( null == m_trans_Member_05 )
       {
            Engine.Utility.Log.Error("m_trans_Member_05 为空，请检查prefab是否缺乏组件");
       }
        m_trans_up = fastComponent.FastGetComponent<Transform>("up");
       if( null == m_trans_up )
       {
            Engine.Utility.Log.Error("m_trans_up 为空，请检查prefab是否缺乏组件");
       }
        m_btn_bg_target = fastComponent.FastGetComponent<UIButton>("bg_target");
       if( null == m_btn_bg_target )
       {
            Engine.Utility.Log.Error("m_btn_bg_target 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_label_index_name = fastComponent.FastGetComponent<UILabel>("index_name");
       if( null == m_label_index_name )
       {
            Engine.Utility.Log.Error("m_label_index_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_talk = fastComponent.FastGetComponent<UIButton>("btn_talk");
       if( null == m_btn_btn_talk )
       {
            Engine.Utility.Log.Error("m_btn_btn_talk 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_pickup = fastComponent.FastGetComponent<UIButton>("btn_pickup");
       if( null == m_btn_btn_pickup )
       {
            Engine.Utility.Log.Error("m_btn_btn_pickup 为空，请检查prefab是否缺乏组件");
       }
        m_label_itemModeLbl = fastComponent.FastGetComponent<UILabel>("itemModeLbl");
       if( null == m_label_itemModeLbl )
       {
            Engine.Utility.Log.Error("m_label_itemModeLbl 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_pickuppanle = fastComponent.FastGetComponent<UISprite>("pickuppanle");
       if( null == m_sprite_pickuppanle )
       {
            Engine.Utility.Log.Error("m_sprite_pickuppanle 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_free = fastComponent.FastGetComponent<UIButton>("btn_free");
       if( null == m_btn_btn_free )
       {
            Engine.Utility.Log.Error("m_btn_btn_free 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_distribution = fastComponent.FastGetComponent<UIButton>("btn_distribution");
       if( null == m_btn_btn_distribution )
       {
            Engine.Utility.Log.Error("m_btn_btn_distribution 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_pickuppanleClose = fastComponent.FastGetComponent<UIButton>("btn_pickuppanleClose");
       if( null == m_btn_btn_pickuppanleClose )
       {
            Engine.Utility.Log.Error("m_btn_btn_pickuppanleClose 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_leaveteam = fastComponent.FastGetComponent<UIButton>("btn_leaveteam");
       if( null == m_btn_btn_leaveteam )
       {
            Engine.Utility.Log.Error("m_btn_btn_leaveteam 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_disbandteam = fastComponent.FastGetComponent<UIButton>("btn_disbandteam");
       if( null == m_btn_btn_disbandteam )
       {
            Engine.Utility.Log.Error("m_btn_btn_disbandteam 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_match = fastComponent.FastGetComponent<UIButton>("btn_match");
       if( null == m_btn_btn_match )
       {
            Engine.Utility.Log.Error("m_btn_btn_match 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_cancelMatch = fastComponent.FastGetComponent<UIButton>("btn_cancelMatch");
       if( null == m_btn_btn_cancelMatch )
       {
            Engine.Utility.Log.Error("m_btn_btn_cancelMatch 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_enter = fastComponent.FastGetComponent<UIButton>("btn_enter");
       if( null == m_btn_btn_enter )
       {
            Engine.Utility.Log.Error("m_btn_btn_enter 为空，请检查prefab是否缺乏组件");
       }
        m_widget_applyListPanel = fastComponent.FastGetComponent<UIWidget>("applyListPanel");
       if( null == m_widget_applyListPanel )
       {
            Engine.Utility.Log.Error("m_widget_applyListPanel 为空，请检查prefab是否缺乏组件");
       }
        m_grid_applyListContent = fastComponent.FastGetComponent<UIGrid>("applyListContent");
       if( null == m_grid_applyListContent )
       {
            Engine.Utility.Log.Error("m_grid_applyListContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Apply_01 = fastComponent.FastGetComponent<Transform>("Apply_01");
       if( null == m_trans_Apply_01 )
       {
            Engine.Utility.Log.Error("m_trans_Apply_01 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Apply_02 = fastComponent.FastGetComponent<Transform>("Apply_02");
       if( null == m_trans_Apply_02 )
       {
            Engine.Utility.Log.Error("m_trans_Apply_02 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Apply_03 = fastComponent.FastGetComponent<Transform>("Apply_03");
       if( null == m_trans_Apply_03 )
       {
            Engine.Utility.Log.Error("m_trans_Apply_03 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Apply_04 = fastComponent.FastGetComponent<Transform>("Apply_04");
       if( null == m_trans_Apply_04 )
       {
            Engine.Utility.Log.Error("m_trans_Apply_04 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Apply_05 = fastComponent.FastGetComponent<Transform>("Apply_05");
       if( null == m_trans_Apply_05 )
       {
            Engine.Utility.Log.Error("m_trans_Apply_05 为空，请检查prefab是否缺乏组件");
       }
        m_grid_applyListbg = fastComponent.FastGetComponent<UIGrid>("applyListbg");
       if( null == m_grid_applyListbg )
       {
            Engine.Utility.Log.Error("m_grid_applyListbg 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_previouspage = fastComponent.FastGetComponent<UIButton>("btn_previouspage");
       if( null == m_btn_btn_previouspage )
       {
            Engine.Utility.Log.Error("m_btn_btn_previouspage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_nextpage = fastComponent.FastGetComponent<UIButton>("btn_nextpage");
       if( null == m_btn_btn_nextpage )
       {
            Engine.Utility.Log.Error("m_btn_btn_nextpage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_clean = fastComponent.FastGetComponent<UIButton>("btn_clean");
       if( null == m_btn_btn_clean )
       {
            Engine.Utility.Log.Error("m_btn_btn_clean 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_bg_target.gameObject).onClick = _onClick_Bg_target_Btn;
        UIEventListener.Get(m_btn_btn_talk.gameObject).onClick = _onClick_Btn_talk_Btn;
        UIEventListener.Get(m_btn_btn_pickup.gameObject).onClick = _onClick_Btn_pickup_Btn;
        UIEventListener.Get(m_btn_btn_free.gameObject).onClick = _onClick_Btn_free_Btn;
        UIEventListener.Get(m_btn_btn_distribution.gameObject).onClick = _onClick_Btn_distribution_Btn;
        UIEventListener.Get(m_btn_btn_pickuppanleClose.gameObject).onClick = _onClick_Btn_pickuppanleClose_Btn;
        UIEventListener.Get(m_btn_btn_leaveteam.gameObject).onClick = _onClick_Btn_leaveteam_Btn;
        UIEventListener.Get(m_btn_btn_disbandteam.gameObject).onClick = _onClick_Btn_disbandteam_Btn;
        UIEventListener.Get(m_btn_btn_match.gameObject).onClick = _onClick_Btn_match_Btn;
        UIEventListener.Get(m_btn_btn_cancelMatch.gameObject).onClick = _onClick_Btn_cancelMatch_Btn;
        UIEventListener.Get(m_btn_btn_enter.gameObject).onClick = _onClick_Btn_enter_Btn;
        UIEventListener.Get(m_btn_btn_previouspage.gameObject).onClick = _onClick_Btn_previouspage_Btn;
        UIEventListener.Get(m_btn_btn_nextpage.gameObject).onClick = _onClick_Btn_nextpage_Btn;
        UIEventListener.Get(m_btn_btn_clean.gameObject).onClick = _onClick_Btn_clean_Btn;
    }

    void _onClick_Bg_target_Btn(GameObject caster)
    {
        onClick_Bg_target_Btn( caster );
    }

    void _onClick_Btn_talk_Btn(GameObject caster)
    {
        onClick_Btn_talk_Btn( caster );
    }

    void _onClick_Btn_pickup_Btn(GameObject caster)
    {
        onClick_Btn_pickup_Btn( caster );
    }

    void _onClick_Btn_free_Btn(GameObject caster)
    {
        onClick_Btn_free_Btn( caster );
    }

    void _onClick_Btn_distribution_Btn(GameObject caster)
    {
        onClick_Btn_distribution_Btn( caster );
    }

    void _onClick_Btn_pickuppanleClose_Btn(GameObject caster)
    {
        onClick_Btn_pickuppanleClose_Btn( caster );
    }

    void _onClick_Btn_leaveteam_Btn(GameObject caster)
    {
        onClick_Btn_leaveteam_Btn( caster );
    }

    void _onClick_Btn_disbandteam_Btn(GameObject caster)
    {
        onClick_Btn_disbandteam_Btn( caster );
    }

    void _onClick_Btn_match_Btn(GameObject caster)
    {
        onClick_Btn_match_Btn( caster );
    }

    void _onClick_Btn_cancelMatch_Btn(GameObject caster)
    {
        onClick_Btn_cancelMatch_Btn( caster );
    }

    void _onClick_Btn_enter_Btn(GameObject caster)
    {
        onClick_Btn_enter_Btn( caster );
    }

    void _onClick_Btn_previouspage_Btn(GameObject caster)
    {
        onClick_Btn_previouspage_Btn( caster );
    }

    void _onClick_Btn_nextpage_Btn(GameObject caster)
    {
        onClick_Btn_nextpage_Btn( caster );
    }

    void _onClick_Btn_clean_Btn(GameObject caster)
    {
        onClick_Btn_clean_Btn( caster );
    }


}
