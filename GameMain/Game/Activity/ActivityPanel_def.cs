//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ActivityPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_OtherPanel;

    Transform            m_trans_OtherScrollView;

    UIGridCreatorBase    m_ctor_RightRoot;

    UIGridCreatorBase    m_ctor_DailyGiftRoot;

    Transform            m_trans_ToggleScrollView;

    UIGridCreatorBase    m_ctor_ListRoot;

    UIButton             m_btn_btn_close;

    UITexture            m__NormalContent;

    Transform            m_trans_TittleContent;

    Transform            m_trans_BuyContent;

    UIButton             m_btn_Buy;

    UISprite             m_sprite_Bought;

    Transform            m_trans_LabelContent;

    UILabel              m_label_TimeName;

    UILabel              m_label_TimeDescription;

    Transform            m_trans_WeekCostContent;

    UILabel              m_label_WeekCostNum;

    UILabel              m_label_ScheduleLabel;

    UITexture            m__DailyGiftContent;

    UITexture            m__Rule;

    Transform            m_trans_UIWelfareToggleGrid;

    UISprite             m_sprite_Warrning;

    Transform            m_trans_UIDailyGiftGrid;

    Transform            m_trans_UIWelfareOtherGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_OtherPanel = fastComponent.FastGetComponent<UIWidget>("OtherPanel");
       if( null == m_widget_OtherPanel )
       {
            Engine.Utility.Log.Error("m_widget_OtherPanel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_OtherScrollView = fastComponent.FastGetComponent<Transform>("OtherScrollView");
       if( null == m_trans_OtherScrollView )
       {
            Engine.Utility.Log.Error("m_trans_OtherScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_RightRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("RightRoot");
       if( null == m_ctor_RightRoot )
       {
            Engine.Utility.Log.Error("m_ctor_RightRoot 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_DailyGiftRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("DailyGiftRoot");
       if( null == m_ctor_DailyGiftRoot )
       {
            Engine.Utility.Log.Error("m_ctor_DailyGiftRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ToggleScrollView = fastComponent.FastGetComponent<Transform>("ToggleScrollView");
       if( null == m_trans_ToggleScrollView )
       {
            Engine.Utility.Log.Error("m_trans_ToggleScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ListRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("ListRoot");
       if( null == m_ctor_ListRoot )
       {
            Engine.Utility.Log.Error("m_ctor_ListRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m__NormalContent = fastComponent.FastGetComponent<UITexture>("NormalContent");
       if( null == m__NormalContent )
       {
            Engine.Utility.Log.Error("m__NormalContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TittleContent = fastComponent.FastGetComponent<Transform>("TittleContent");
       if( null == m_trans_TittleContent )
       {
            Engine.Utility.Log.Error("m_trans_TittleContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BuyContent = fastComponent.FastGetComponent<Transform>("BuyContent");
       if( null == m_trans_BuyContent )
       {
            Engine.Utility.Log.Error("m_trans_BuyContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Buy = fastComponent.FastGetComponent<UIButton>("Buy");
       if( null == m_btn_Buy )
       {
            Engine.Utility.Log.Error("m_btn_Buy 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Bought = fastComponent.FastGetComponent<UISprite>("Bought");
       if( null == m_sprite_Bought )
       {
            Engine.Utility.Log.Error("m_sprite_Bought 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LabelContent = fastComponent.FastGetComponent<Transform>("LabelContent");
       if( null == m_trans_LabelContent )
       {
            Engine.Utility.Log.Error("m_trans_LabelContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_TimeName = fastComponent.FastGetComponent<UILabel>("TimeName");
       if( null == m_label_TimeName )
       {
            Engine.Utility.Log.Error("m_label_TimeName 为空，请检查prefab是否缺乏组件");
       }
        m_label_TimeDescription = fastComponent.FastGetComponent<UILabel>("TimeDescription");
       if( null == m_label_TimeDescription )
       {
            Engine.Utility.Log.Error("m_label_TimeDescription 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WeekCostContent = fastComponent.FastGetComponent<Transform>("WeekCostContent");
       if( null == m_trans_WeekCostContent )
       {
            Engine.Utility.Log.Error("m_trans_WeekCostContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_WeekCostNum = fastComponent.FastGetComponent<UILabel>("WeekCostNum");
       if( null == m_label_WeekCostNum )
       {
            Engine.Utility.Log.Error("m_label_WeekCostNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_ScheduleLabel = fastComponent.FastGetComponent<UILabel>("ScheduleLabel");
       if( null == m_label_ScheduleLabel )
       {
            Engine.Utility.Log.Error("m_label_ScheduleLabel 为空，请检查prefab是否缺乏组件");
       }
        m__DailyGiftContent = fastComponent.FastGetComponent<UITexture>("DailyGiftContent");
       if( null == m__DailyGiftContent )
       {
            Engine.Utility.Log.Error("m__DailyGiftContent 为空，请检查prefab是否缺乏组件");
       }
        m__Rule = fastComponent.FastGetComponent<UITexture>("Rule");
       if( null == m__Rule )
       {
            Engine.Utility.Log.Error("m__Rule 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIWelfareToggleGrid = fastComponent.FastGetComponent<Transform>("UIWelfareToggleGrid");
       if( null == m_trans_UIWelfareToggleGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIWelfareToggleGrid 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Warrning = fastComponent.FastGetComponent<UISprite>("Warrning");
       if( null == m_sprite_Warrning )
       {
            Engine.Utility.Log.Error("m_sprite_Warrning 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIDailyGiftGrid = fastComponent.FastGetComponent<Transform>("UIDailyGiftGrid");
       if( null == m_trans_UIDailyGiftGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIDailyGiftGrid 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
        UIEventListener.Get(m_btn_Buy.gameObject).onClick = _onClick_Buy_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_Buy_Btn(GameObject caster)
    {
        onClick_Buy_Btn( caster );
    }


}
