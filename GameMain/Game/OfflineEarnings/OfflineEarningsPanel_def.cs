//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class OfflineEarningsPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIGridCreatorBase    m_ctor_ItemGridScrollView;

    UILabel              m_label_EarningTime;

    UILabel              m_label_EXpEarning;

    UIButton             m_btn_Get;

    UIButton             m_btn_Jump;

    UIButton             m_btn_OneCheck;

    UIButton             m_btn_TwoCheck;

    Transform            m_trans_UIOfflineRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_ctor_ItemGridScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ItemGridScrollView");
       if( null == m_ctor_ItemGridScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ItemGridScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_label_EarningTime = fastComponent.FastGetComponent<UILabel>("EarningTime");
       if( null == m_label_EarningTime )
       {
            Engine.Utility.Log.Error("m_label_EarningTime 为空，请检查prefab是否缺乏组件");
       }
        m_label_EXpEarning = fastComponent.FastGetComponent<UILabel>("EXpEarning");
       if( null == m_label_EXpEarning )
       {
            Engine.Utility.Log.Error("m_label_EXpEarning 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Get = fastComponent.FastGetComponent<UIButton>("Get");
       if( null == m_btn_Get )
       {
            Engine.Utility.Log.Error("m_btn_Get 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Jump = fastComponent.FastGetComponent<UIButton>("Jump");
       if( null == m_btn_Jump )
       {
            Engine.Utility.Log.Error("m_btn_Jump 为空，请检查prefab是否缺乏组件");
       }
        m_btn_OneCheck = fastComponent.FastGetComponent<UIButton>("OneCheck");
       if( null == m_btn_OneCheck )
       {
            Engine.Utility.Log.Error("m_btn_OneCheck 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TwoCheck = fastComponent.FastGetComponent<UIButton>("TwoCheck");
       if( null == m_btn_TwoCheck )
       {
            Engine.Utility.Log.Error("m_btn_TwoCheck 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIOfflineRewardGrid = fastComponent.FastGetComponent<Transform>("UIOfflineRewardGrid");
       if( null == m_trans_UIOfflineRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIOfflineRewardGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Get.gameObject).onClick = _onClick_Get_Btn;
        UIEventListener.Get(m_btn_Jump.gameObject).onClick = _onClick_Jump_Btn;
        UIEventListener.Get(m_btn_OneCheck.gameObject).onClick = _onClick_OneCheck_Btn;
        UIEventListener.Get(m_btn_TwoCheck.gameObject).onClick = _onClick_TwoCheck_Btn;
    }

    void _onClick_Get_Btn(GameObject caster)
    {
        onClick_Get_Btn( caster );
    }

    void _onClick_Jump_Btn(GameObject caster)
    {
        onClick_Jump_Btn( caster );
    }

    void _onClick_OneCheck_Btn(GameObject caster)
    {
        onClick_OneCheck_Btn( caster );
    }

    void _onClick_TwoCheck_Btn(GameObject caster)
    {
        onClick_TwoCheck_Btn( caster );
    }


}
