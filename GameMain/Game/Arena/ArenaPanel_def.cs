//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ArenaPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UITexture            m__MainPlayerIcon;

    UILabel              m_label_MainPlayerLevel;

    UILabel              m_label_MainPlayerName;

    UILabel              m_label_MyRanking;

    UILabel              m_label_MyFighting;

    UIButton             m_btn_btn_report;

    UIButton             m_btn_btn_store;

    UIButton             m_btn_btn_ranklist;

    UIButton             m_btn_btn_setskill;

    UILabel              m_label_number_challenge;

    UIButton             m_btn_add_challenge;

    Transform            m_trans_OpponentContent;

    UIScrollView         m_scrollview_panel;

    Transform            m_trans_gridContent;

    UIGrid               m_grid_challengeThreeContent;

    UIWidget             m_widget_player_01;

    UIWidget             m_widget_player_02;

    UIWidget             m_widget_player_03;

    Transform            m_trans_down;

    UIButton             m_btn_btn_change;

    UILabel              m_label_Challenge_Time;

    UIButton             m_btn_NowChalleng;

    UILabel              m_label_NowChalleng_price;

    UIGrid               m_grid_TopThreeContent;

    Transform            m_trans_Top_01;

    Transform            m_trans_Top_02;

    Transform            m_trans_Top_03;

    UILabel              m_label_Des_label;

    UIButton             m_btn_CheckRewardBtn;

    UILabel              m_label_check_label;

    UIButton             m_btn_leftArrow;

    UIButton             m_btn_rightArrow;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m__MainPlayerIcon = fastComponent.FastGetComponent<UITexture>("MainPlayerIcon");
       if( null == m__MainPlayerIcon )
       {
            Engine.Utility.Log.Error("m__MainPlayerIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_MainPlayerLevel = fastComponent.FastGetComponent<UILabel>("MainPlayerLevel");
       if( null == m_label_MainPlayerLevel )
       {
            Engine.Utility.Log.Error("m_label_MainPlayerLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_MainPlayerName = fastComponent.FastGetComponent<UILabel>("MainPlayerName");
       if( null == m_label_MainPlayerName )
       {
            Engine.Utility.Log.Error("m_label_MainPlayerName 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyRanking = fastComponent.FastGetComponent<UILabel>("MyRanking");
       if( null == m_label_MyRanking )
       {
            Engine.Utility.Log.Error("m_label_MyRanking 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyFighting = fastComponent.FastGetComponent<UILabel>("MyFighting");
       if( null == m_label_MyFighting )
       {
            Engine.Utility.Log.Error("m_label_MyFighting 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_report = fastComponent.FastGetComponent<UIButton>("btn_report");
       if( null == m_btn_btn_report )
       {
            Engine.Utility.Log.Error("m_btn_btn_report 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_store = fastComponent.FastGetComponent<UIButton>("btn_store");
       if( null == m_btn_btn_store )
       {
            Engine.Utility.Log.Error("m_btn_btn_store 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_ranklist = fastComponent.FastGetComponent<UIButton>("btn_ranklist");
       if( null == m_btn_btn_ranklist )
       {
            Engine.Utility.Log.Error("m_btn_btn_ranklist 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_setskill = fastComponent.FastGetComponent<UIButton>("btn_setskill");
       if( null == m_btn_btn_setskill )
       {
            Engine.Utility.Log.Error("m_btn_btn_setskill 为空，请检查prefab是否缺乏组件");
       }
        m_label_number_challenge = fastComponent.FastGetComponent<UILabel>("number_challenge");
       if( null == m_label_number_challenge )
       {
            Engine.Utility.Log.Error("m_label_number_challenge 为空，请检查prefab是否缺乏组件");
       }
        m_btn_add_challenge = fastComponent.FastGetComponent<UIButton>("add_challenge");
       if( null == m_btn_add_challenge )
       {
            Engine.Utility.Log.Error("m_btn_add_challenge 为空，请检查prefab是否缺乏组件");
       }
        m_trans_OpponentContent = fastComponent.FastGetComponent<Transform>("OpponentContent");
       if( null == m_trans_OpponentContent )
       {
            Engine.Utility.Log.Error("m_trans_OpponentContent 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_panel = fastComponent.FastGetComponent<UIScrollView>("panel");
       if( null == m_scrollview_panel )
       {
            Engine.Utility.Log.Error("m_scrollview_panel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_gridContent = fastComponent.FastGetComponent<Transform>("gridContent");
       if( null == m_trans_gridContent )
       {
            Engine.Utility.Log.Error("m_trans_gridContent 为空，请检查prefab是否缺乏组件");
       }
        m_grid_challengeThreeContent = fastComponent.FastGetComponent<UIGrid>("challengeThreeContent");
       if( null == m_grid_challengeThreeContent )
       {
            Engine.Utility.Log.Error("m_grid_challengeThreeContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_player_01 = fastComponent.FastGetComponent<UIWidget>("player_01");
       if( null == m_widget_player_01 )
       {
            Engine.Utility.Log.Error("m_widget_player_01 为空，请检查prefab是否缺乏组件");
       }
        m_widget_player_02 = fastComponent.FastGetComponent<UIWidget>("player_02");
       if( null == m_widget_player_02 )
       {
            Engine.Utility.Log.Error("m_widget_player_02 为空，请检查prefab是否缺乏组件");
       }
        m_widget_player_03 = fastComponent.FastGetComponent<UIWidget>("player_03");
       if( null == m_widget_player_03 )
       {
            Engine.Utility.Log.Error("m_widget_player_03 为空，请检查prefab是否缺乏组件");
       }
        m_trans_down = fastComponent.FastGetComponent<Transform>("down");
       if( null == m_trans_down )
       {
            Engine.Utility.Log.Error("m_trans_down 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_change = fastComponent.FastGetComponent<UIButton>("btn_change");
       if( null == m_btn_btn_change )
       {
            Engine.Utility.Log.Error("m_btn_btn_change 为空，请检查prefab是否缺乏组件");
       }
        m_label_Challenge_Time = fastComponent.FastGetComponent<UILabel>("Challenge_Time");
       if( null == m_label_Challenge_Time )
       {
            Engine.Utility.Log.Error("m_label_Challenge_Time 为空，请检查prefab是否缺乏组件");
       }
        m_btn_NowChalleng = fastComponent.FastGetComponent<UIButton>("NowChalleng");
       if( null == m_btn_NowChalleng )
       {
            Engine.Utility.Log.Error("m_btn_NowChalleng 为空，请检查prefab是否缺乏组件");
       }
        m_label_NowChalleng_price = fastComponent.FastGetComponent<UILabel>("NowChalleng_price");
       if( null == m_label_NowChalleng_price )
       {
            Engine.Utility.Log.Error("m_label_NowChalleng_price 为空，请检查prefab是否缺乏组件");
       }
        m_grid_TopThreeContent = fastComponent.FastGetComponent<UIGrid>("TopThreeContent");
       if( null == m_grid_TopThreeContent )
       {
            Engine.Utility.Log.Error("m_grid_TopThreeContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Top_01 = fastComponent.FastGetComponent<Transform>("Top_01");
       if( null == m_trans_Top_01 )
       {
            Engine.Utility.Log.Error("m_trans_Top_01 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Top_02 = fastComponent.FastGetComponent<Transform>("Top_02");
       if( null == m_trans_Top_02 )
       {
            Engine.Utility.Log.Error("m_trans_Top_02 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Top_03 = fastComponent.FastGetComponent<Transform>("Top_03");
       if( null == m_trans_Top_03 )
       {
            Engine.Utility.Log.Error("m_trans_Top_03 为空，请检查prefab是否缺乏组件");
       }
        m_label_Des_label = fastComponent.FastGetComponent<UILabel>("Des_label");
       if( null == m_label_Des_label )
       {
            Engine.Utility.Log.Error("m_label_Des_label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CheckRewardBtn = fastComponent.FastGetComponent<UIButton>("CheckRewardBtn");
       if( null == m_btn_CheckRewardBtn )
       {
            Engine.Utility.Log.Error("m_btn_CheckRewardBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_check_label = fastComponent.FastGetComponent<UILabel>("check_label");
       if( null == m_label_check_label )
       {
            Engine.Utility.Log.Error("m_label_check_label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_leftArrow = fastComponent.FastGetComponent<UIButton>("leftArrow");
       if( null == m_btn_leftArrow )
       {
            Engine.Utility.Log.Error("m_btn_leftArrow 为空，请检查prefab是否缺乏组件");
       }
        m_btn_rightArrow = fastComponent.FastGetComponent<UIButton>("rightArrow");
       if( null == m_btn_rightArrow )
       {
            Engine.Utility.Log.Error("m_btn_rightArrow 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_report.gameObject).onClick = _onClick_Btn_report_Btn;
        UIEventListener.Get(m_btn_btn_store.gameObject).onClick = _onClick_Btn_store_Btn;
        UIEventListener.Get(m_btn_btn_ranklist.gameObject).onClick = _onClick_Btn_ranklist_Btn;
        UIEventListener.Get(m_btn_btn_setskill.gameObject).onClick = _onClick_Btn_setskill_Btn;
        UIEventListener.Get(m_btn_add_challenge.gameObject).onClick = _onClick_Add_challenge_Btn;
        UIEventListener.Get(m_btn_btn_change.gameObject).onClick = _onClick_Btn_change_Btn;
        UIEventListener.Get(m_btn_NowChalleng.gameObject).onClick = _onClick_NowChalleng_Btn;
        UIEventListener.Get(m_btn_CheckRewardBtn.gameObject).onClick = _onClick_CheckRewardBtn_Btn;
        UIEventListener.Get(m_btn_leftArrow.gameObject).onClick = _onClick_LeftArrow_Btn;
        UIEventListener.Get(m_btn_rightArrow.gameObject).onClick = _onClick_RightArrow_Btn;
    }

    void _onClick_Btn_report_Btn(GameObject caster)
    {
        onClick_Btn_report_Btn( caster );
    }

    void _onClick_Btn_store_Btn(GameObject caster)
    {
        onClick_Btn_store_Btn( caster );
    }

    void _onClick_Btn_ranklist_Btn(GameObject caster)
    {
        onClick_Btn_ranklist_Btn( caster );
    }

    void _onClick_Btn_setskill_Btn(GameObject caster)
    {
        onClick_Btn_setskill_Btn( caster );
    }

    void _onClick_Add_challenge_Btn(GameObject caster)
    {
        onClick_Add_challenge_Btn( caster );
    }

    void _onClick_Btn_change_Btn(GameObject caster)
    {
        onClick_Btn_change_Btn( caster );
    }

    void _onClick_NowChalleng_Btn(GameObject caster)
    {
        onClick_NowChalleng_Btn( caster );
    }

    void _onClick_CheckRewardBtn_Btn(GameObject caster)
    {
        onClick_CheckRewardBtn_Btn( caster );
    }

    void _onClick_LeftArrow_Btn(GameObject caster)
    {
        onClick_LeftArrow_Btn( caster );
    }

    void _onClick_RightArrow_Btn(GameObject caster)
    {
        onClick_RightArrow_Btn( caster );
    }


}
