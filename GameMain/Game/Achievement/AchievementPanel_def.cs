//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class AchievementPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIScrollView         m_scrollview_TypeScrollView;

    UIGridCreatorBase    m_ctor_AchievementScrollView;

    UIToggle             m_toggle_HideAchievement;

    UILabel              m_label_CompletedAchievement;

    UIButton             m_btn_Btn_AllReceive;

    Transform            m_trans_UICtrTypeGrid;

    UIWidget             m_widget_UISecondTypeGrid;

    Transform            m_trans_UIAchievemenGrid;

    UILabel              m_label_AchievementName;

    Transform            m_trans_RewardRoot;

    UILabel              m_label_Describe;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_scrollview_TypeScrollView = fastComponent.FastGetComponent<UIScrollView>("TypeScrollView");
       if( null == m_scrollview_TypeScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_TypeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_AchievementScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("AchievementScrollView");
       if( null == m_ctor_AchievementScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_AchievementScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_HideAchievement = fastComponent.FastGetComponent<UIToggle>("HideAchievement");
       if( null == m_toggle_HideAchievement )
       {
            Engine.Utility.Log.Error("m_toggle_HideAchievement 为空，请检查prefab是否缺乏组件");
       }
        m_label_CompletedAchievement = fastComponent.FastGetComponent<UILabel>("CompletedAchievement");
       if( null == m_label_CompletedAchievement )
       {
            Engine.Utility.Log.Error("m_label_CompletedAchievement 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_AllReceive = fastComponent.FastGetComponent<UIButton>("Btn_AllReceive");
       if( null == m_btn_Btn_AllReceive )
       {
            Engine.Utility.Log.Error("m_btn_Btn_AllReceive 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UICtrTypeGrid = fastComponent.FastGetComponent<Transform>("UICtrTypeGrid");
       if( null == m_trans_UICtrTypeGrid )
       {
            Engine.Utility.Log.Error("m_trans_UICtrTypeGrid 为空，请检查prefab是否缺乏组件");
       }
        m_widget_UISecondTypeGrid = fastComponent.FastGetComponent<UIWidget>("UISecondTypeGrid");
       if( null == m_widget_UISecondTypeGrid )
       {
            Engine.Utility.Log.Error("m_widget_UISecondTypeGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIAchievemenGrid = fastComponent.FastGetComponent<Transform>("UIAchievemenGrid");
       if( null == m_trans_UIAchievemenGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIAchievemenGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_AchievementName = fastComponent.FastGetComponent<UILabel>("AchievementName");
       if( null == m_label_AchievementName )
       {
            Engine.Utility.Log.Error("m_label_AchievementName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RewardRoot = fastComponent.FastGetComponent<Transform>("RewardRoot");
       if( null == m_trans_RewardRoot )
       {
            Engine.Utility.Log.Error("m_trans_RewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_Describe = fastComponent.FastGetComponent<UILabel>("Describe");
       if( null == m_label_Describe )
       {
            Engine.Utility.Log.Error("m_label_Describe 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Btn_AllReceive.gameObject).onClick = _onClick_Btn_AllReceive_Btn;
    }

    void _onClick_Btn_AllReceive_Btn(GameObject caster)
    {
        onClick_Btn_AllReceive_Btn( caster );
    }


}
