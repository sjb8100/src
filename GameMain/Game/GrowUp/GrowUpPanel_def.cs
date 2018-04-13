//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GrowUpPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_MypowerNum;

    UILabel              m_label_Recommendpower;

    UILabel              m_label_RecommendpowerNum;

    UISprite             m_sprite_ScoreNum;

    UIButton             m_btn_btn_analysis;

    UILabel              m_label_btn_analysis_label;

    UIButton             m_btn_btn_fightPower;

    UILabel              m_label_btn_label;

    UIScrollView         m_scrollview_growleftscrollview;

    Transform            m_trans_growrightscrollview;

    Transform            m_trans_growFightPowerContent;

    Transform            m_trans_growFightPowerrightscrollview;

    Transform            m_trans_GoUpWight;

    UISprite             m_sprite_goUp;

    UILabel              m_label_goUpDes;

    UIButton             m_btn_goUpBtn;

    UIButton             m_btn_goUpCloseBtn;

    UISprite             m_sprite_UIGrowUpFightPowergrid;

    UISprite             m_sprite_UIGrowUpGrid;

    Transform            m_trans_UIGrowUpSecondTypeGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_MypowerNum = fastComponent.FastGetComponent<UILabel>("MypowerNum");
       if( null == m_label_MypowerNum )
       {
            Engine.Utility.Log.Error("m_label_MypowerNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_Recommendpower = fastComponent.FastGetComponent<UILabel>("Recommendpower");
       if( null == m_label_Recommendpower )
       {
            Engine.Utility.Log.Error("m_label_Recommendpower 为空，请检查prefab是否缺乏组件");
       }
        m_label_RecommendpowerNum = fastComponent.FastGetComponent<UILabel>("RecommendpowerNum");
       if( null == m_label_RecommendpowerNum )
       {
            Engine.Utility.Log.Error("m_label_RecommendpowerNum 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_ScoreNum = fastComponent.FastGetComponent<UISprite>("ScoreNum");
       if( null == m_sprite_ScoreNum )
       {
            Engine.Utility.Log.Error("m_sprite_ScoreNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_analysis = fastComponent.FastGetComponent<UIButton>("btn_analysis");
       if( null == m_btn_btn_analysis )
       {
            Engine.Utility.Log.Error("m_btn_btn_analysis 为空，请检查prefab是否缺乏组件");
       }
        m_label_btn_analysis_label = fastComponent.FastGetComponent<UILabel>("btn_analysis_label");
       if( null == m_label_btn_analysis_label )
       {
            Engine.Utility.Log.Error("m_label_btn_analysis_label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_fightPower = fastComponent.FastGetComponent<UIButton>("btn_fightPower");
       if( null == m_btn_btn_fightPower )
       {
            Engine.Utility.Log.Error("m_btn_btn_fightPower 为空，请检查prefab是否缺乏组件");
       }
        m_label_btn_label = fastComponent.FastGetComponent<UILabel>("btn_label");
       if( null == m_label_btn_label )
       {
            Engine.Utility.Log.Error("m_label_btn_label 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_growleftscrollview = fastComponent.FastGetComponent<UIScrollView>("growleftscrollview");
       if( null == m_scrollview_growleftscrollview )
       {
            Engine.Utility.Log.Error("m_scrollview_growleftscrollview 为空，请检查prefab是否缺乏组件");
       }
        m_trans_growrightscrollview = fastComponent.FastGetComponent<Transform>("growrightscrollview");
       if( null == m_trans_growrightscrollview )
       {
            Engine.Utility.Log.Error("m_trans_growrightscrollview 为空，请检查prefab是否缺乏组件");
       }
        m_trans_growFightPowerContent = fastComponent.FastGetComponent<Transform>("growFightPowerContent");
       if( null == m_trans_growFightPowerContent )
       {
            Engine.Utility.Log.Error("m_trans_growFightPowerContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_growFightPowerrightscrollview = fastComponent.FastGetComponent<Transform>("growFightPowerrightscrollview");
       if( null == m_trans_growFightPowerrightscrollview )
       {
            Engine.Utility.Log.Error("m_trans_growFightPowerrightscrollview 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GoUpWight = fastComponent.FastGetComponent<Transform>("GoUpWight");
       if( null == m_trans_GoUpWight )
       {
            Engine.Utility.Log.Error("m_trans_GoUpWight 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_goUp = fastComponent.FastGetComponent<UISprite>("goUp");
       if( null == m_sprite_goUp )
       {
            Engine.Utility.Log.Error("m_sprite_goUp 为空，请检查prefab是否缺乏组件");
       }
        m_label_goUpDes = fastComponent.FastGetComponent<UILabel>("goUpDes");
       if( null == m_label_goUpDes )
       {
            Engine.Utility.Log.Error("m_label_goUpDes 为空，请检查prefab是否缺乏组件");
       }
        m_btn_goUpBtn = fastComponent.FastGetComponent<UIButton>("goUpBtn");
       if( null == m_btn_goUpBtn )
       {
            Engine.Utility.Log.Error("m_btn_goUpBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_goUpCloseBtn = fastComponent.FastGetComponent<UIButton>("goUpCloseBtn");
       if( null == m_btn_goUpCloseBtn )
       {
            Engine.Utility.Log.Error("m_btn_goUpCloseBtn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UIGrowUpFightPowergrid = fastComponent.FastGetComponent<UISprite>("UIGrowUpFightPowergrid");
       if( null == m_sprite_UIGrowUpFightPowergrid )
       {
            Engine.Utility.Log.Error("m_sprite_UIGrowUpFightPowergrid 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UIGrowUpGrid = fastComponent.FastGetComponent<UISprite>("UIGrowUpGrid");
       if( null == m_sprite_UIGrowUpGrid )
       {
            Engine.Utility.Log.Error("m_sprite_UIGrowUpGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIGrowUpSecondTypeGrid = fastComponent.FastGetComponent<Transform>("UIGrowUpSecondTypeGrid");
       if( null == m_trans_UIGrowUpSecondTypeGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIGrowUpSecondTypeGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_analysis.gameObject).onClick = _onClick_Btn_analysis_Btn;
        UIEventListener.Get(m_btn_btn_fightPower.gameObject).onClick = _onClick_Btn_fightPower_Btn;
        UIEventListener.Get(m_btn_goUpBtn.gameObject).onClick = _onClick_GoUpBtn_Btn;
        UIEventListener.Get(m_btn_goUpCloseBtn.gameObject).onClick = _onClick_GoUpCloseBtn_Btn;
    }

    void _onClick_Btn_analysis_Btn(GameObject caster)
    {
        onClick_Btn_analysis_Btn( caster );
    }

    void _onClick_Btn_fightPower_Btn(GameObject caster)
    {
        onClick_Btn_fightPower_Btn( caster );
    }

    void _onClick_GoUpBtn_Btn(GameObject caster)
    {
        onClick_GoUpBtn_Btn( caster );
    }

    void _onClick_GoUpCloseBtn_Btn(GameObject caster)
    {
        onClick_GoUpCloseBtn_Btn( caster );
    }


}
