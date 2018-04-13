//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SevenDayPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Content_1,
		Content_2,
		Content_3,
		Content_4,
		Max,
    }

   FastComponent         fastComponent;

    UILabel              m_label_TimeLeftNum;

    Transform            m_trans_DayContent;

    UIGridCreatorBase    m_ctor_ToggleScrollView;

    UIButton             m_btn_FinalTarget;

    UISprite             m_sprite_Warrning;

    Transform            m_trans_DayRoot;

    UIWidget             m_widget_GiftBagPanel;

    UITexture            m__Icon;

    UILabel              m_label_GiftBagName;

    UILabel              m_label_OldNum;

    UILabel              m_label_NewNum;

    UIButton             m_btn_Btn_Buy;

    Transform            m_trans_ToggleContent;

    UIButton             m_btn_Content_1;

    UIButton             m_btn_Content_2;

    UIButton             m_btn_Content_3;

    UIButton             m_btn_Content_4;

    UIWidget             m_widget_RightContainer;

    UIWidget             m_widget_LevelAndTimePanel;

    UIGridCreatorBase    m_ctor_LevelAndTimeScrollView;

    Transform            m_trans_TargetRoot;

    UILabel              m_label_HuoDongDianLabel;

    UISlider             m_slider_HuoDongSlider;

    UITexture            m__Model;

    UIGridCreatorBase    m_ctor_RewardRoot;

    UIButton             m_btn_btn_close;

    Transform            m_trans_UIItemRewardGrid;

    Transform            m_trans_UIWelfareRewardGrid;

    Transform            m_trans_UIWelfareToggleGrid;

    Transform            m_trans_UIWelfareOtherGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_TimeLeftNum = fastComponent.FastGetComponent<UILabel>("TimeLeftNum");
       if( null == m_label_TimeLeftNum )
       {
            Engine.Utility.Log.Error("m_label_TimeLeftNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DayContent = fastComponent.FastGetComponent<Transform>("DayContent");
       if( null == m_trans_DayContent )
       {
            Engine.Utility.Log.Error("m_trans_DayContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ToggleScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ToggleScrollView");
       if( null == m_ctor_ToggleScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ToggleScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_FinalTarget = fastComponent.FastGetComponent<UIButton>("FinalTarget");
       if( null == m_btn_FinalTarget )
       {
            Engine.Utility.Log.Error("m_btn_FinalTarget 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Warrning = fastComponent.FastGetComponent<UISprite>("Warrning");
       if( null == m_sprite_Warrning )
       {
            Engine.Utility.Log.Error("m_sprite_Warrning 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DayRoot = fastComponent.FastGetComponent<Transform>("DayRoot");
       if( null == m_trans_DayRoot )
       {
            Engine.Utility.Log.Error("m_trans_DayRoot 为空，请检查prefab是否缺乏组件");
       }
        m_widget_GiftBagPanel = fastComponent.FastGetComponent<UIWidget>("GiftBagPanel");
       if( null == m_widget_GiftBagPanel )
       {
            Engine.Utility.Log.Error("m_widget_GiftBagPanel 为空，请检查prefab是否缺乏组件");
       }
        m__Icon = fastComponent.FastGetComponent<UITexture>("Icon");
       if( null == m__Icon )
       {
            Engine.Utility.Log.Error("m__Icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_GiftBagName = fastComponent.FastGetComponent<UILabel>("GiftBagName");
       if( null == m_label_GiftBagName )
       {
            Engine.Utility.Log.Error("m_label_GiftBagName 为空，请检查prefab是否缺乏组件");
       }
        m_label_OldNum = fastComponent.FastGetComponent<UILabel>("OldNum");
       if( null == m_label_OldNum )
       {
            Engine.Utility.Log.Error("m_label_OldNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_NewNum = fastComponent.FastGetComponent<UILabel>("NewNum");
       if( null == m_label_NewNum )
       {
            Engine.Utility.Log.Error("m_label_NewNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Buy = fastComponent.FastGetComponent<UIButton>("Btn_Buy");
       if( null == m_btn_Btn_Buy )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Buy 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ToggleContent = fastComponent.FastGetComponent<Transform>("ToggleContent");
       if( null == m_trans_ToggleContent )
       {
            Engine.Utility.Log.Error("m_trans_ToggleContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Content_1 = fastComponent.FastGetComponent<UIButton>("Content_1");
       if( null == m_btn_Content_1 )
       {
            Engine.Utility.Log.Error("m_btn_Content_1 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Content_2 = fastComponent.FastGetComponent<UIButton>("Content_2");
       if( null == m_btn_Content_2 )
       {
            Engine.Utility.Log.Error("m_btn_Content_2 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Content_3 = fastComponent.FastGetComponent<UIButton>("Content_3");
       if( null == m_btn_Content_3 )
       {
            Engine.Utility.Log.Error("m_btn_Content_3 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Content_4 = fastComponent.FastGetComponent<UIButton>("Content_4");
       if( null == m_btn_Content_4 )
       {
            Engine.Utility.Log.Error("m_btn_Content_4 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RightContainer = fastComponent.FastGetComponent<UIWidget>("RightContainer");
       if( null == m_widget_RightContainer )
       {
            Engine.Utility.Log.Error("m_widget_RightContainer 为空，请检查prefab是否缺乏组件");
       }
        m_widget_LevelAndTimePanel = fastComponent.FastGetComponent<UIWidget>("LevelAndTimePanel");
       if( null == m_widget_LevelAndTimePanel )
       {
            Engine.Utility.Log.Error("m_widget_LevelAndTimePanel 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_LevelAndTimeScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("LevelAndTimeScrollView");
       if( null == m_ctor_LevelAndTimeScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_LevelAndTimeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TargetRoot = fastComponent.FastGetComponent<Transform>("TargetRoot");
       if( null == m_trans_TargetRoot )
       {
            Engine.Utility.Log.Error("m_trans_TargetRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_HuoDongDianLabel = fastComponent.FastGetComponent<UILabel>("HuoDongDianLabel");
       if( null == m_label_HuoDongDianLabel )
       {
            Engine.Utility.Log.Error("m_label_HuoDongDianLabel 为空，请检查prefab是否缺乏组件");
       }
        m_slider_HuoDongSlider = fastComponent.FastGetComponent<UISlider>("HuoDongSlider");
       if( null == m_slider_HuoDongSlider )
       {
            Engine.Utility.Log.Error("m_slider_HuoDongSlider 为空，请检查prefab是否缺乏组件");
       }
        m__Model = fastComponent.FastGetComponent<UITexture>("Model");
       if( null == m__Model )
       {
            Engine.Utility.Log.Error("m__Model 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_RewardRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("RewardRoot");
       if( null == m_ctor_RewardRoot )
       {
            Engine.Utility.Log.Error("m_ctor_RewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIWelfareRewardGrid = fastComponent.FastGetComponent<Transform>("UIWelfareRewardGrid");
       if( null == m_trans_UIWelfareRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIWelfareRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIWelfareToggleGrid = fastComponent.FastGetComponent<Transform>("UIWelfareToggleGrid");
       if( null == m_trans_UIWelfareToggleGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIWelfareToggleGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIWelfareOtherGrid = fastComponent.FastGetComponent<Transform>("UIWelfareOtherGrid");
       if( null == m_trans_UIWelfareOtherGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIWelfareOtherGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_FinalTarget.gameObject).onClick = _onClick_FinalTarget_Btn;
        UIEventListener.Get(m_btn_Btn_Buy.gameObject).onClick = _onClick_Btn_Buy_Btn;
        UIEventListener.Get(m_btn_Content_1.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_Content_2.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_Content_3.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_Content_4.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
    }

    void _onClick_FinalTarget_Btn(GameObject caster)
    {
        onClick_FinalTarget_Btn( caster );
    }

    void _onClick_Btn_Buy_Btn(GameObject caster)
    {
        onClick_Btn_Buy_Btn( caster );
    }

    void _OnBtnsClick(GameObject caster)
    {
        BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), caster.name);
        OnBtnsClick( btntype );
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }


}
