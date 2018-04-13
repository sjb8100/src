//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class QuestionPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//告玩家书
		GaoWanJiaShu = 1,
		//问卷1
		WenJuan1 = 2,
		//问卷2
		WenJuan2 = 3,
		//问卷3
		WenJuan3 = 4,
		//反馈
		FanKui = 5,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_GongGao;

    UILabel              m_label_title;

    UIWidget             m_widget_root;

    UIButton             m_btn_GongGaoConfirmBtn;

    Transform            m_trans_WenJuan;

    UILabel              m_label_WenJuanText;

    Transform            m_trans_WJRewardGrid;

    UIButton             m_btn_WenJuanConfirmBtn;

    UILabel              m_label_questLabel;

    UISprite             m_sprite_questWarning;

    UIButton             m_btn_WenJuanGetRewardBtn;

    UILabel              m_label_questLabel2;

    UISprite             m_sprite_questWarning2;

    UIGridCreatorBase    m_ctor_WenJuanBtnRoot;

    Transform            m_trans_FanKui;

    UILabel              m_label_FanKuiText;

    Transform            m_trans_FKRewardGrid;

    UIButton             m_btn_FanKuiConfirmBtn;

    UILabel              m_label_feedLabel;

    UISprite             m_sprite_FeedWarning;

    UILabel              m_label_times;

    UILabel              m_label_name;

    UIButton             m_btn_close;

    Transform            m_trans_UIItemRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_GongGao = fastComponent.FastGetComponent<Transform>("GongGao");
       if( null == m_trans_GongGao )
       {
            Engine.Utility.Log.Error("m_trans_GongGao 为空，请检查prefab是否缺乏组件");
       }
        m_label_title = fastComponent.FastGetComponent<UILabel>("title");
       if( null == m_label_title )
       {
            Engine.Utility.Log.Error("m_label_title 为空，请检查prefab是否缺乏组件");
       }
        m_widget_root = fastComponent.FastGetComponent<UIWidget>("root");
       if( null == m_widget_root )
       {
            Engine.Utility.Log.Error("m_widget_root 为空，请检查prefab是否缺乏组件");
       }
        m_btn_GongGaoConfirmBtn = fastComponent.FastGetComponent<UIButton>("GongGaoConfirmBtn");
       if( null == m_btn_GongGaoConfirmBtn )
       {
            Engine.Utility.Log.Error("m_btn_GongGaoConfirmBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WenJuan = fastComponent.FastGetComponent<Transform>("WenJuan");
       if( null == m_trans_WenJuan )
       {
            Engine.Utility.Log.Error("m_trans_WenJuan 为空，请检查prefab是否缺乏组件");
       }
        m_label_WenJuanText = fastComponent.FastGetComponent<UILabel>("WenJuanText");
       if( null == m_label_WenJuanText )
       {
            Engine.Utility.Log.Error("m_label_WenJuanText 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WJRewardGrid = fastComponent.FastGetComponent<Transform>("WJRewardGrid");
       if( null == m_trans_WJRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_WJRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WenJuanConfirmBtn = fastComponent.FastGetComponent<UIButton>("WenJuanConfirmBtn");
       if( null == m_btn_WenJuanConfirmBtn )
       {
            Engine.Utility.Log.Error("m_btn_WenJuanConfirmBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_questLabel = fastComponent.FastGetComponent<UILabel>("questLabel");
       if( null == m_label_questLabel )
       {
            Engine.Utility.Log.Error("m_label_questLabel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_questWarning = fastComponent.FastGetComponent<UISprite>("questWarning");
       if( null == m_sprite_questWarning )
       {
            Engine.Utility.Log.Error("m_sprite_questWarning 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WenJuanGetRewardBtn = fastComponent.FastGetComponent<UIButton>("WenJuanGetRewardBtn");
       if( null == m_btn_WenJuanGetRewardBtn )
       {
            Engine.Utility.Log.Error("m_btn_WenJuanGetRewardBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_questLabel2 = fastComponent.FastGetComponent<UILabel>("questLabel2");
       if( null == m_label_questLabel2 )
       {
            Engine.Utility.Log.Error("m_label_questLabel2 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_questWarning2 = fastComponent.FastGetComponent<UISprite>("questWarning2");
       if( null == m_sprite_questWarning2 )
       {
            Engine.Utility.Log.Error("m_sprite_questWarning2 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_WenJuanBtnRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("WenJuanBtnRoot");
       if( null == m_ctor_WenJuanBtnRoot )
       {
            Engine.Utility.Log.Error("m_ctor_WenJuanBtnRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FanKui = fastComponent.FastGetComponent<Transform>("FanKui");
       if( null == m_trans_FanKui )
       {
            Engine.Utility.Log.Error("m_trans_FanKui 为空，请检查prefab是否缺乏组件");
       }
        m_label_FanKuiText = fastComponent.FastGetComponent<UILabel>("FanKuiText");
       if( null == m_label_FanKuiText )
       {
            Engine.Utility.Log.Error("m_label_FanKuiText 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FKRewardGrid = fastComponent.FastGetComponent<Transform>("FKRewardGrid");
       if( null == m_trans_FKRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_FKRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_FanKuiConfirmBtn = fastComponent.FastGetComponent<UIButton>("FanKuiConfirmBtn");
       if( null == m_btn_FanKuiConfirmBtn )
       {
            Engine.Utility.Log.Error("m_btn_FanKuiConfirmBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_feedLabel = fastComponent.FastGetComponent<UILabel>("feedLabel");
       if( null == m_label_feedLabel )
       {
            Engine.Utility.Log.Error("m_label_feedLabel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_FeedWarning = fastComponent.FastGetComponent<UISprite>("FeedWarning");
       if( null == m_sprite_FeedWarning )
       {
            Engine.Utility.Log.Error("m_sprite_FeedWarning 为空，请检查prefab是否缺乏组件");
       }
        m_label_times = fastComponent.FastGetComponent<UILabel>("times");
       if( null == m_label_times )
       {
            Engine.Utility.Log.Error("m_label_times 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_GongGaoConfirmBtn.gameObject).onClick = _onClick_GongGaoConfirmBtn_Btn;
        UIEventListener.Get(m_btn_WenJuanConfirmBtn.gameObject).onClick = _onClick_WenJuanConfirmBtn_Btn;
        UIEventListener.Get(m_btn_WenJuanGetRewardBtn.gameObject).onClick = _onClick_WenJuanGetRewardBtn_Btn;
        UIEventListener.Get(m_btn_FanKuiConfirmBtn.gameObject).onClick = _onClick_FanKuiConfirmBtn_Btn;
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_GongGaoConfirmBtn_Btn(GameObject caster)
    {
        onClick_GongGaoConfirmBtn_Btn( caster );
    }

    void _onClick_WenJuanConfirmBtn_Btn(GameObject caster)
    {
        onClick_WenJuanConfirmBtn_Btn( caster );
    }

    void _onClick_WenJuanGetRewardBtn_Btn(GameObject caster)
    {
        onClick_WenJuanGetRewardBtn_Btn( caster );
    }

    void _onClick_FanKuiConfirmBtn_Btn(GameObject caster)
    {
        onClick_FanKuiConfirmBtn_Btn( caster );
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
