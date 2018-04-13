//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ActiveTakePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_Title;

    UIButton             m_btn_btn_close;

    Transform            m_trans_DailyRewardContent;

    UIScrollView         m_scrollview_ScrollView;

    Transform            m_trans_RewardRoot;

    UILabel              m_label_Des_Label;

    UIButton             m_btn_btn_Take;

    Transform            m_trans_BindPhoneContent;

    UIInput              m_input_PhoneNumber;

    UIInput              m_input_VerifyNumber;

    UIButton             m_btn_VerifyBtn;

    UILabel              m_label_VerifyBtnLabel;

    UIButton             m_btn_BindBtn;

    Transform            m_trans_UIItemRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_Title = fastComponent.FastGetComponent<UILabel>("Title");
       if( null == m_label_Title )
       {
            Engine.Utility.Log.Error("m_label_Title 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DailyRewardContent = fastComponent.FastGetComponent<Transform>("DailyRewardContent");
       if( null == m_trans_DailyRewardContent )
       {
            Engine.Utility.Log.Error("m_trans_DailyRewardContent 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_ScrollView = fastComponent.FastGetComponent<UIScrollView>("ScrollView");
       if( null == m_scrollview_ScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_ScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RewardRoot = fastComponent.FastGetComponent<Transform>("RewardRoot");
       if( null == m_trans_RewardRoot )
       {
            Engine.Utility.Log.Error("m_trans_RewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_Des_Label = fastComponent.FastGetComponent<UILabel>("Des_Label");
       if( null == m_label_Des_Label )
       {
            Engine.Utility.Log.Error("m_label_Des_Label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Take = fastComponent.FastGetComponent<UIButton>("btn_Take");
       if( null == m_btn_btn_Take )
       {
            Engine.Utility.Log.Error("m_btn_btn_Take 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BindPhoneContent = fastComponent.FastGetComponent<Transform>("BindPhoneContent");
       if( null == m_trans_BindPhoneContent )
       {
            Engine.Utility.Log.Error("m_trans_BindPhoneContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_PhoneNumber = fastComponent.FastGetComponent<UIInput>("PhoneNumber");
       if( null == m_input_PhoneNumber )
       {
            Engine.Utility.Log.Error("m_input_PhoneNumber 为空，请检查prefab是否缺乏组件");
       }
        m_input_VerifyNumber = fastComponent.FastGetComponent<UIInput>("VerifyNumber");
       if( null == m_input_VerifyNumber )
       {
            Engine.Utility.Log.Error("m_input_VerifyNumber 为空，请检查prefab是否缺乏组件");
       }
        m_btn_VerifyBtn = fastComponent.FastGetComponent<UIButton>("VerifyBtn");
       if( null == m_btn_VerifyBtn )
       {
            Engine.Utility.Log.Error("m_btn_VerifyBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_VerifyBtnLabel = fastComponent.FastGetComponent<UILabel>("VerifyBtnLabel");
       if( null == m_label_VerifyBtnLabel )
       {
            Engine.Utility.Log.Error("m_label_VerifyBtnLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BindBtn = fastComponent.FastGetComponent<UIButton>("BindBtn");
       if( null == m_btn_BindBtn )
       {
            Engine.Utility.Log.Error("m_btn_BindBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_Take.gameObject).onClick = _onClick_Btn_Take_Btn;
        UIEventListener.Get(m_btn_VerifyBtn.gameObject).onClick = _onClick_VerifyBtn_Btn;
        UIEventListener.Get(m_btn_BindBtn.gameObject).onClick = _onClick_BindBtn_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_Btn_Take_Btn(GameObject caster)
    {
        onClick_Btn_Take_Btn( caster );
    }

    void _onClick_VerifyBtn_Btn(GameObject caster)
    {
        onClick_VerifyBtn_Btn( caster );
    }

    void _onClick_BindBtn_Btn(GameObject caster)
    {
        onClick_BindBtn_Btn( caster );
    }


}
