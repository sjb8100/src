//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class WelfarePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_CheckInPanel;

    UIGridCreatorBase    m_ctor_CheckInScrollView;

    UILabel              m_label_CheckDayNum;

    UIButton             m_btn_Btn_Check;

    UIWidget             m_widget_OtherPanel;

    UIGridCreatorBase    m_ctor_OtherScrollView;

    UIWidget             m_widget_RewardFindPanel;

    UIGridCreatorBase    m_ctor_RewardFindScroll;

    Transform            m_trans_NullRewardTipsContent;

    UIWidget             m_widget_BindContent;

    UIButton             m_btn_GetBtn;

    UISprite             m_sprite_m_bind_red;

    UIButton             m_btn_BindBtn;

    Transform            m_trans_BindRewardRoot;

    UISprite             m_sprite_Status_Received;

    UIWidget             m_widget_CDkeyContent;

    UIInput              m_input_keyInput;

    UIButton             m_btn_pasteBtn;

    UIButton             m_btn_convertBtn;

    UIWidget             m_widget_FriendInviteContent;

    UIGridCreatorBase    m_ctor_FriendInviteScroll;

    UIButton             m_btn_InviterBtn;

    UIButton             m_btn_InvitedBtn;

    UIButton             m_btn_InvitedRechargeBtn;

    Transform            m_trans_InviterRoot;

    UIButton             m_btn_CopyBtn;

    UILabel              m_label_InviteCode;

    UIButton             m_btn_ShareBtn;

    UIButton             m_btn_InviteListBtn;

    Transform            m_trans_InvitedRoot;

    Transform            m_trans_NoInviter;

    UIInput              m_input_MyInviteCode;

    UIButton             m_btn_ConfirmInviteBtn;

    Transform            m_trans_HasInviter;

    UILabel              m_label_InviterInfo;

    UIButton             m_btn_ContactInviterBtn;

    UIButton             m_btn_WelfareUItips_1;

    UIWidget             m_widget_CollectWordContent;

    UIGridCreatorBase    m_ctor_CollectWordScrollView;

    UILabel              m_label_ScheduleLabel;

    UIGridCreatorBase    m_ctor_ToggleScrollView;

    UITexture            m__huodong_beijing;

    UITexture            m__wenzi;

    UIButton             m_btn_btn_close;

    Transform            m_trans_UIWelfareToggleGrid;

    UISprite             m_sprite_Warrning;

    Transform            m_trans_UIWelfareOtherGrid;

    Transform            m_trans_UIWelfareCheckGrid;

    Transform            m_trans_UIItemRewardGrid;

    Transform            m_trans_UIRewardFindGrid;

    Transform            m_trans_UICollectionWordGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_CheckInPanel = fastComponent.FastGetComponent<UIWidget>("CheckInPanel");
       if( null == m_widget_CheckInPanel )
       {
            Engine.Utility.Log.Error("m_widget_CheckInPanel 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_CheckInScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("CheckInScrollView");
       if( null == m_ctor_CheckInScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_CheckInScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_label_CheckDayNum = fastComponent.FastGetComponent<UILabel>("CheckDayNum");
       if( null == m_label_CheckDayNum )
       {
            Engine.Utility.Log.Error("m_label_CheckDayNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Check = fastComponent.FastGetComponent<UIButton>("Btn_Check");
       if( null == m_btn_Btn_Check )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Check 为空，请检查prefab是否缺乏组件");
       }
        m_widget_OtherPanel = fastComponent.FastGetComponent<UIWidget>("OtherPanel");
       if( null == m_widget_OtherPanel )
       {
            Engine.Utility.Log.Error("m_widget_OtherPanel 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_OtherScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("OtherScrollView");
       if( null == m_ctor_OtherScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_OtherScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RewardFindPanel = fastComponent.FastGetComponent<UIWidget>("RewardFindPanel");
       if( null == m_widget_RewardFindPanel )
       {
            Engine.Utility.Log.Error("m_widget_RewardFindPanel 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_RewardFindScroll = fastComponent.FastGetComponent<UIGridCreatorBase>("RewardFindScroll");
       if( null == m_ctor_RewardFindScroll )
       {
            Engine.Utility.Log.Error("m_ctor_RewardFindScroll 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NullRewardTipsContent = fastComponent.FastGetComponent<Transform>("NullRewardTipsContent");
       if( null == m_trans_NullRewardTipsContent )
       {
            Engine.Utility.Log.Error("m_trans_NullRewardTipsContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_BindContent = fastComponent.FastGetComponent<UIWidget>("BindContent");
       if( null == m_widget_BindContent )
       {
            Engine.Utility.Log.Error("m_widget_BindContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_GetBtn = fastComponent.FastGetComponent<UIButton>("GetBtn");
       if( null == m_btn_GetBtn )
       {
            Engine.Utility.Log.Error("m_btn_GetBtn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_m_bind_red = fastComponent.FastGetComponent<UISprite>("m_bind_red");
       if( null == m_sprite_m_bind_red )
       {
            Engine.Utility.Log.Error("m_sprite_m_bind_red 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BindBtn = fastComponent.FastGetComponent<UIButton>("BindBtn");
       if( null == m_btn_BindBtn )
       {
            Engine.Utility.Log.Error("m_btn_BindBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BindRewardRoot = fastComponent.FastGetComponent<Transform>("BindRewardRoot");
       if( null == m_trans_BindRewardRoot )
       {
            Engine.Utility.Log.Error("m_trans_BindRewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Status_Received = fastComponent.FastGetComponent<UISprite>("Status_Received");
       if( null == m_sprite_Status_Received )
       {
            Engine.Utility.Log.Error("m_sprite_Status_Received 为空，请检查prefab是否缺乏组件");
       }
        m_widget_CDkeyContent = fastComponent.FastGetComponent<UIWidget>("CDkeyContent");
       if( null == m_widget_CDkeyContent )
       {
            Engine.Utility.Log.Error("m_widget_CDkeyContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_keyInput = fastComponent.FastGetComponent<UIInput>("keyInput");
       if( null == m_input_keyInput )
       {
            Engine.Utility.Log.Error("m_input_keyInput 为空，请检查prefab是否缺乏组件");
       }
        m_btn_pasteBtn = fastComponent.FastGetComponent<UIButton>("pasteBtn");
       if( null == m_btn_pasteBtn )
       {
            Engine.Utility.Log.Error("m_btn_pasteBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_convertBtn = fastComponent.FastGetComponent<UIButton>("convertBtn");
       if( null == m_btn_convertBtn )
       {
            Engine.Utility.Log.Error("m_btn_convertBtn 为空，请检查prefab是否缺乏组件");
       }
        m_widget_FriendInviteContent = fastComponent.FastGetComponent<UIWidget>("FriendInviteContent");
       if( null == m_widget_FriendInviteContent )
       {
            Engine.Utility.Log.Error("m_widget_FriendInviteContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_FriendInviteScroll = fastComponent.FastGetComponent<UIGridCreatorBase>("FriendInviteScroll");
       if( null == m_ctor_FriendInviteScroll )
       {
            Engine.Utility.Log.Error("m_ctor_FriendInviteScroll 为空，请检查prefab是否缺乏组件");
       }
        m_btn_InviterBtn = fastComponent.FastGetComponent<UIButton>("InviterBtn");
       if( null == m_btn_InviterBtn )
       {
            Engine.Utility.Log.Error("m_btn_InviterBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_InvitedBtn = fastComponent.FastGetComponent<UIButton>("InvitedBtn");
       if( null == m_btn_InvitedBtn )
       {
            Engine.Utility.Log.Error("m_btn_InvitedBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_InvitedRechargeBtn = fastComponent.FastGetComponent<UIButton>("InvitedRechargeBtn");
       if( null == m_btn_InvitedRechargeBtn )
       {
            Engine.Utility.Log.Error("m_btn_InvitedRechargeBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_InviterRoot = fastComponent.FastGetComponent<Transform>("InviterRoot");
       if( null == m_trans_InviterRoot )
       {
            Engine.Utility.Log.Error("m_trans_InviterRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CopyBtn = fastComponent.FastGetComponent<UIButton>("CopyBtn");
       if( null == m_btn_CopyBtn )
       {
            Engine.Utility.Log.Error("m_btn_CopyBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_InviteCode = fastComponent.FastGetComponent<UILabel>("InviteCode");
       if( null == m_label_InviteCode )
       {
            Engine.Utility.Log.Error("m_label_InviteCode 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ShareBtn = fastComponent.FastGetComponent<UIButton>("ShareBtn");
       if( null == m_btn_ShareBtn )
       {
            Engine.Utility.Log.Error("m_btn_ShareBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_InviteListBtn = fastComponent.FastGetComponent<UIButton>("InviteListBtn");
       if( null == m_btn_InviteListBtn )
       {
            Engine.Utility.Log.Error("m_btn_InviteListBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_InvitedRoot = fastComponent.FastGetComponent<Transform>("InvitedRoot");
       if( null == m_trans_InvitedRoot )
       {
            Engine.Utility.Log.Error("m_trans_InvitedRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NoInviter = fastComponent.FastGetComponent<Transform>("NoInviter");
       if( null == m_trans_NoInviter )
       {
            Engine.Utility.Log.Error("m_trans_NoInviter 为空，请检查prefab是否缺乏组件");
       }
        m_input_MyInviteCode = fastComponent.FastGetComponent<UIInput>("MyInviteCode");
       if( null == m_input_MyInviteCode )
       {
            Engine.Utility.Log.Error("m_input_MyInviteCode 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ConfirmInviteBtn = fastComponent.FastGetComponent<UIButton>("ConfirmInviteBtn");
       if( null == m_btn_ConfirmInviteBtn )
       {
            Engine.Utility.Log.Error("m_btn_ConfirmInviteBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_HasInviter = fastComponent.FastGetComponent<Transform>("HasInviter");
       if( null == m_trans_HasInviter )
       {
            Engine.Utility.Log.Error("m_trans_HasInviter 为空，请检查prefab是否缺乏组件");
       }
        m_label_InviterInfo = fastComponent.FastGetComponent<UILabel>("InviterInfo");
       if( null == m_label_InviterInfo )
       {
            Engine.Utility.Log.Error("m_label_InviterInfo 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ContactInviterBtn = fastComponent.FastGetComponent<UIButton>("ContactInviterBtn");
       if( null == m_btn_ContactInviterBtn )
       {
            Engine.Utility.Log.Error("m_btn_ContactInviterBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WelfareUItips_1 = fastComponent.FastGetComponent<UIButton>("WelfareUItips_1");
       if( null == m_btn_WelfareUItips_1 )
       {
            Engine.Utility.Log.Error("m_btn_WelfareUItips_1 为空，请检查prefab是否缺乏组件");
       }
        m_widget_CollectWordContent = fastComponent.FastGetComponent<UIWidget>("CollectWordContent");
       if( null == m_widget_CollectWordContent )
       {
            Engine.Utility.Log.Error("m_widget_CollectWordContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_CollectWordScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("CollectWordScrollView");
       if( null == m_ctor_CollectWordScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_CollectWordScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_label_ScheduleLabel = fastComponent.FastGetComponent<UILabel>("ScheduleLabel");
       if( null == m_label_ScheduleLabel )
       {
            Engine.Utility.Log.Error("m_label_ScheduleLabel 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ToggleScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ToggleScrollView");
       if( null == m_ctor_ToggleScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ToggleScrollView 为空，请检查prefab是否缺乏组件");
       }
        m__huodong_beijing = fastComponent.FastGetComponent<UITexture>("huodong_beijing");
       if( null == m__huodong_beijing )
       {
            Engine.Utility.Log.Error("m__huodong_beijing 为空，请检查prefab是否缺乏组件");
       }
        m__wenzi = fastComponent.FastGetComponent<UITexture>("wenzi");
       if( null == m__wenzi )
       {
            Engine.Utility.Log.Error("m__wenzi 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
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
        m_trans_UIWelfareOtherGrid = fastComponent.FastGetComponent<Transform>("UIWelfareOtherGrid");
       if( null == m_trans_UIWelfareOtherGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIWelfareOtherGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIWelfareCheckGrid = fastComponent.FastGetComponent<Transform>("UIWelfareCheckGrid");
       if( null == m_trans_UIWelfareCheckGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIWelfareCheckGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIRewardFindGrid = fastComponent.FastGetComponent<Transform>("UIRewardFindGrid");
       if( null == m_trans_UIRewardFindGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIRewardFindGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UICollectionWordGrid = fastComponent.FastGetComponent<Transform>("UICollectionWordGrid");
       if( null == m_trans_UICollectionWordGrid )
       {
            Engine.Utility.Log.Error("m_trans_UICollectionWordGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Btn_Check.gameObject).onClick = _onClick_Btn_Check_Btn;
        UIEventListener.Get(m_btn_GetBtn.gameObject).onClick = _onClick_GetBtn_Btn;
        UIEventListener.Get(m_btn_BindBtn.gameObject).onClick = _onClick_BindBtn_Btn;
        UIEventListener.Get(m_btn_pasteBtn.gameObject).onClick = _onClick_PasteBtn_Btn;
        UIEventListener.Get(m_btn_convertBtn.gameObject).onClick = _onClick_ConvertBtn_Btn;
        UIEventListener.Get(m_btn_InviterBtn.gameObject).onClick = _onClick_InviterBtn_Btn;
        UIEventListener.Get(m_btn_InvitedBtn.gameObject).onClick = _onClick_InvitedBtn_Btn;
        UIEventListener.Get(m_btn_InvitedRechargeBtn.gameObject).onClick = _onClick_InvitedRechargeBtn_Btn;
        UIEventListener.Get(m_btn_CopyBtn.gameObject).onClick = _onClick_CopyBtn_Btn;
        UIEventListener.Get(m_btn_ShareBtn.gameObject).onClick = _onClick_ShareBtn_Btn;
        UIEventListener.Get(m_btn_InviteListBtn.gameObject).onClick = _onClick_InviteListBtn_Btn;
        UIEventListener.Get(m_btn_ConfirmInviteBtn.gameObject).onClick = _onClick_ConfirmInviteBtn_Btn;
        UIEventListener.Get(m_btn_ContactInviterBtn.gameObject).onClick = _onClick_ContactInviterBtn_Btn;
        UIEventListener.Get(m_btn_WelfareUItips_1.gameObject).onClick = _onClick_WelfareUItips_1_Btn;
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
    }

    void _onClick_Btn_Check_Btn(GameObject caster)
    {
        onClick_Btn_Check_Btn( caster );
    }

    void _onClick_GetBtn_Btn(GameObject caster)
    {
        onClick_GetBtn_Btn( caster );
    }

    void _onClick_BindBtn_Btn(GameObject caster)
    {
        onClick_BindBtn_Btn( caster );
    }

    void _onClick_PasteBtn_Btn(GameObject caster)
    {
        onClick_PasteBtn_Btn( caster );
    }

    void _onClick_ConvertBtn_Btn(GameObject caster)
    {
        onClick_ConvertBtn_Btn( caster );
    }

    void _onClick_InviterBtn_Btn(GameObject caster)
    {
        onClick_InviterBtn_Btn( caster );
    }

    void _onClick_InvitedBtn_Btn(GameObject caster)
    {
        onClick_InvitedBtn_Btn( caster );
    }

    void _onClick_InvitedRechargeBtn_Btn(GameObject caster)
    {
        onClick_InvitedRechargeBtn_Btn( caster );
    }

    void _onClick_CopyBtn_Btn(GameObject caster)
    {
        onClick_CopyBtn_Btn( caster );
    }

    void _onClick_ShareBtn_Btn(GameObject caster)
    {
        onClick_ShareBtn_Btn( caster );
    }

    void _onClick_InviteListBtn_Btn(GameObject caster)
    {
        onClick_InviteListBtn_Btn( caster );
    }

    void _onClick_ConfirmInviteBtn_Btn(GameObject caster)
    {
        onClick_ConfirmInviteBtn_Btn( caster );
    }

    void _onClick_ContactInviterBtn_Btn(GameObject caster)
    {
        onClick_ContactInviterBtn_Btn( caster );
    }

    void _onClick_WelfareUItips_1_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }


}
