//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ClanCreatePanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//申请
		ShenQing = 1,
		//创建
		ChuangJian = 2,
		//支持
		ZhiChi = 3,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_FucContentClip;

    Transform            m_trans_FucContent;

    UILabel              m_label_TabLv;

    UIInput              m_input_SearchInput;

    UIButton             m_btn_BtnApplySearch;

    UIButton             m_btn_BtnApply;

    UIButton             m_btn_BtnApplyQuick;

    UIButton             m_btn_BtnApplyContactShaikh;

    UILabel              m_label_ApplyGGInfo;

    UIGridCreatorBase    m_ctor_ClanApplyScrollView;

    Transform            m_trans_ClanNoticeName;

    UIInput              m_input_ClanNameInput;

    UILabel              m_label_TmpClanName;

    Transform            m_trans_CreateTipsContent;

    UILabel              m_label_CreateTipsTipsInfo;

    Transform            m_trans_TempClanInfo;

    UILabel              m_label_TempClanSupNum;

    UILabel              m_label_TempClanLeftTime;

    UIButton             m_btn_BtnCreateTempClan;

    UIButton             m_btn_BtnNotice;

    Transform            m_trans_ClanCost;

    Transform            m_trans_TempClanCost;

    Transform            m_trans_TempClanCostWQ;

    Transform            m_trans_TempClanCosCoin;

    Transform            m_trans_ClanCostItemRoot;

    UILabel              m_label_CreateGGInfo;

    UIButton             m_btn_ClanNoticeEditBtn;

    UIInput              m_input_SupportSearchInput;

    UIButton             m_btn_BtnSupportSearch;

    UILabel              m_label_SupportGGInfo;

    UIButton             m_btn_BtnSupportContactShaikh;

    UIButton             m_btn_BtnSupport;

    UIButton             m_btn_BtnSupportCancel;

    UILabel              m_label_TabLeftTime;

    UIGridCreatorBase    m_ctor_ClanSupportScrollView;

    Transform            m_trans_FunctioToggles;

    Transform            m_trans_UIClanSupportGrid;

    Transform            m_trans_UIClanGrid;

    Transform            m_trans_UIItemGrowCostGrid;

    Transform            m_trans_CostDQ;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_Content = fastComponent.FastGetComponent<Transform>("Content");
       if( null == m_trans_Content )
       {
            Engine.Utility.Log.Error("m_trans_Content 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FucContentClip = fastComponent.FastGetComponent<Transform>("FucContentClip");
       if( null == m_trans_FucContentClip )
       {
            Engine.Utility.Log.Error("m_trans_FucContentClip 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FucContent = fastComponent.FastGetComponent<Transform>("FucContent");
       if( null == m_trans_FucContent )
       {
            Engine.Utility.Log.Error("m_trans_FucContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_TabLv = fastComponent.FastGetComponent<UILabel>("TabLv");
       if( null == m_label_TabLv )
       {
            Engine.Utility.Log.Error("m_label_TabLv 为空，请检查prefab是否缺乏组件");
       }
        m_input_SearchInput = fastComponent.FastGetComponent<UIInput>("SearchInput");
       if( null == m_input_SearchInput )
       {
            Engine.Utility.Log.Error("m_input_SearchInput 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnApplySearch = fastComponent.FastGetComponent<UIButton>("BtnApplySearch");
       if( null == m_btn_BtnApplySearch )
       {
            Engine.Utility.Log.Error("m_btn_BtnApplySearch 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnApply = fastComponent.FastGetComponent<UIButton>("BtnApply");
       if( null == m_btn_BtnApply )
       {
            Engine.Utility.Log.Error("m_btn_BtnApply 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnApplyQuick = fastComponent.FastGetComponent<UIButton>("BtnApplyQuick");
       if( null == m_btn_BtnApplyQuick )
       {
            Engine.Utility.Log.Error("m_btn_BtnApplyQuick 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnApplyContactShaikh = fastComponent.FastGetComponent<UIButton>("BtnApplyContactShaikh");
       if( null == m_btn_BtnApplyContactShaikh )
       {
            Engine.Utility.Log.Error("m_btn_BtnApplyContactShaikh 为空，请检查prefab是否缺乏组件");
       }
        m_label_ApplyGGInfo = fastComponent.FastGetComponent<UILabel>("ApplyGGInfo");
       if( null == m_label_ApplyGGInfo )
       {
            Engine.Utility.Log.Error("m_label_ApplyGGInfo 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ClanApplyScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ClanApplyScrollView");
       if( null == m_ctor_ClanApplyScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ClanApplyScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ClanNoticeName = fastComponent.FastGetComponent<Transform>("ClanNoticeName");
       if( null == m_trans_ClanNoticeName )
       {
            Engine.Utility.Log.Error("m_trans_ClanNoticeName 为空，请检查prefab是否缺乏组件");
       }
        m_input_ClanNameInput = fastComponent.FastGetComponent<UIInput>("ClanNameInput");
       if( null == m_input_ClanNameInput )
       {
            Engine.Utility.Log.Error("m_input_ClanNameInput 为空，请检查prefab是否缺乏组件");
       }
        m_label_TmpClanName = fastComponent.FastGetComponent<UILabel>("TmpClanName");
       if( null == m_label_TmpClanName )
       {
            Engine.Utility.Log.Error("m_label_TmpClanName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CreateTipsContent = fastComponent.FastGetComponent<Transform>("CreateTipsContent");
       if( null == m_trans_CreateTipsContent )
       {
            Engine.Utility.Log.Error("m_trans_CreateTipsContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_CreateTipsTipsInfo = fastComponent.FastGetComponent<UILabel>("CreateTipsTipsInfo");
       if( null == m_label_CreateTipsTipsInfo )
       {
            Engine.Utility.Log.Error("m_label_CreateTipsTipsInfo 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TempClanInfo = fastComponent.FastGetComponent<Transform>("TempClanInfo");
       if( null == m_trans_TempClanInfo )
       {
            Engine.Utility.Log.Error("m_trans_TempClanInfo 为空，请检查prefab是否缺乏组件");
       }
        m_label_TempClanSupNum = fastComponent.FastGetComponent<UILabel>("TempClanSupNum");
       if( null == m_label_TempClanSupNum )
       {
            Engine.Utility.Log.Error("m_label_TempClanSupNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_TempClanLeftTime = fastComponent.FastGetComponent<UILabel>("TempClanLeftTime");
       if( null == m_label_TempClanLeftTime )
       {
            Engine.Utility.Log.Error("m_label_TempClanLeftTime 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnCreateTempClan = fastComponent.FastGetComponent<UIButton>("BtnCreateTempClan");
       if( null == m_btn_BtnCreateTempClan )
       {
            Engine.Utility.Log.Error("m_btn_BtnCreateTempClan 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnNotice = fastComponent.FastGetComponent<UIButton>("BtnNotice");
       if( null == m_btn_BtnNotice )
       {
            Engine.Utility.Log.Error("m_btn_BtnNotice 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ClanCost = fastComponent.FastGetComponent<Transform>("ClanCost");
       if( null == m_trans_ClanCost )
       {
            Engine.Utility.Log.Error("m_trans_ClanCost 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TempClanCost = fastComponent.FastGetComponent<Transform>("TempClanCost");
       if( null == m_trans_TempClanCost )
       {
            Engine.Utility.Log.Error("m_trans_TempClanCost 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TempClanCostWQ = fastComponent.FastGetComponent<Transform>("TempClanCostWQ");
       if( null == m_trans_TempClanCostWQ )
       {
            Engine.Utility.Log.Error("m_trans_TempClanCostWQ 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TempClanCosCoin = fastComponent.FastGetComponent<Transform>("TempClanCosCoin");
       if( null == m_trans_TempClanCosCoin )
       {
            Engine.Utility.Log.Error("m_trans_TempClanCosCoin 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ClanCostItemRoot = fastComponent.FastGetComponent<Transform>("ClanCostItemRoot");
       if( null == m_trans_ClanCostItemRoot )
       {
            Engine.Utility.Log.Error("m_trans_ClanCostItemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_CreateGGInfo = fastComponent.FastGetComponent<UILabel>("CreateGGInfo");
       if( null == m_label_CreateGGInfo )
       {
            Engine.Utility.Log.Error("m_label_CreateGGInfo 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ClanNoticeEditBtn = fastComponent.FastGetComponent<UIButton>("ClanNoticeEditBtn");
       if( null == m_btn_ClanNoticeEditBtn )
       {
            Engine.Utility.Log.Error("m_btn_ClanNoticeEditBtn 为空，请检查prefab是否缺乏组件");
       }
        m_input_SupportSearchInput = fastComponent.FastGetComponent<UIInput>("SupportSearchInput");
       if( null == m_input_SupportSearchInput )
       {
            Engine.Utility.Log.Error("m_input_SupportSearchInput 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSupportSearch = fastComponent.FastGetComponent<UIButton>("BtnSupportSearch");
       if( null == m_btn_BtnSupportSearch )
       {
            Engine.Utility.Log.Error("m_btn_BtnSupportSearch 为空，请检查prefab是否缺乏组件");
       }
        m_label_SupportGGInfo = fastComponent.FastGetComponent<UILabel>("SupportGGInfo");
       if( null == m_label_SupportGGInfo )
       {
            Engine.Utility.Log.Error("m_label_SupportGGInfo 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSupportContactShaikh = fastComponent.FastGetComponent<UIButton>("BtnSupportContactShaikh");
       if( null == m_btn_BtnSupportContactShaikh )
       {
            Engine.Utility.Log.Error("m_btn_BtnSupportContactShaikh 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSupport = fastComponent.FastGetComponent<UIButton>("BtnSupport");
       if( null == m_btn_BtnSupport )
       {
            Engine.Utility.Log.Error("m_btn_BtnSupport 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSupportCancel = fastComponent.FastGetComponent<UIButton>("BtnSupportCancel");
       if( null == m_btn_BtnSupportCancel )
       {
            Engine.Utility.Log.Error("m_btn_BtnSupportCancel 为空，请检查prefab是否缺乏组件");
       }
        m_label_TabLeftTime = fastComponent.FastGetComponent<UILabel>("TabLeftTime");
       if( null == m_label_TabLeftTime )
       {
            Engine.Utility.Log.Error("m_label_TabLeftTime 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ClanSupportScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ClanSupportScrollView");
       if( null == m_ctor_ClanSupportScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ClanSupportScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FunctioToggles = fastComponent.FastGetComponent<Transform>("FunctioToggles");
       if( null == m_trans_FunctioToggles )
       {
            Engine.Utility.Log.Error("m_trans_FunctioToggles 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanSupportGrid = fastComponent.FastGetComponent<Transform>("UIClanSupportGrid");
       if( null == m_trans_UIClanSupportGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanSupportGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanGrid = fastComponent.FastGetComponent<Transform>("UIClanGrid");
       if( null == m_trans_UIClanGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemGrowCostGrid = fastComponent.FastGetComponent<Transform>("UIItemGrowCostGrid");
       if( null == m_trans_UIItemGrowCostGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemGrowCostGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CostDQ = fastComponent.FastGetComponent<Transform>("CostDQ");
       if( null == m_trans_CostDQ )
       {
            Engine.Utility.Log.Error("m_trans_CostDQ 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BtnApplySearch.gameObject).onClick = _onClick_BtnApplySearch_Btn;
        UIEventListener.Get(m_btn_BtnApply.gameObject).onClick = _onClick_BtnApply_Btn;
        UIEventListener.Get(m_btn_BtnApplyQuick.gameObject).onClick = _onClick_BtnApplyQuick_Btn;
        UIEventListener.Get(m_btn_BtnApplyContactShaikh.gameObject).onClick = _onClick_BtnApplyContactShaikh_Btn;
        UIEventListener.Get(m_btn_BtnCreateTempClan.gameObject).onClick = _onClick_BtnCreateTempClan_Btn;
        UIEventListener.Get(m_btn_BtnNotice.gameObject).onClick = _onClick_BtnNotice_Btn;
        UIEventListener.Get(m_btn_ClanNoticeEditBtn.gameObject).onClick = _onClick_ClanNoticeEditBtn_Btn;
        UIEventListener.Get(m_btn_BtnSupportSearch.gameObject).onClick = _onClick_BtnSupportSearch_Btn;
        UIEventListener.Get(m_btn_BtnSupportContactShaikh.gameObject).onClick = _onClick_BtnSupportContactShaikh_Btn;
        UIEventListener.Get(m_btn_BtnSupport.gameObject).onClick = _onClick_BtnSupport_Btn;
        UIEventListener.Get(m_btn_BtnSupportCancel.gameObject).onClick = _onClick_BtnSupportCancel_Btn;
    }

    void _onClick_BtnApplySearch_Btn(GameObject caster)
    {
        onClick_BtnApplySearch_Btn( caster );
    }

    void _onClick_BtnApply_Btn(GameObject caster)
    {
        onClick_BtnApply_Btn( caster );
    }

    void _onClick_BtnApplyQuick_Btn(GameObject caster)
    {
        onClick_BtnApplyQuick_Btn( caster );
    }

    void _onClick_BtnApplyContactShaikh_Btn(GameObject caster)
    {
        onClick_BtnApplyContactShaikh_Btn( caster );
    }

    void _onClick_BtnCreateTempClan_Btn(GameObject caster)
    {
        onClick_BtnCreateTempClan_Btn( caster );
    }

    void _onClick_BtnNotice_Btn(GameObject caster)
    {
        onClick_BtnNotice_Btn( caster );
    }

    void _onClick_ClanNoticeEditBtn_Btn(GameObject caster)
    {
        onClick_ClanNoticeEditBtn_Btn( caster );
    }

    void _onClick_BtnSupportSearch_Btn(GameObject caster)
    {
        onClick_BtnSupportSearch_Btn( caster );
    }

    void _onClick_BtnSupportContactShaikh_Btn(GameObject caster)
    {
        onClick_BtnSupportContactShaikh_Btn( caster );
    }

    void _onClick_BtnSupport_Btn(GameObject caster)
    {
        onClick_BtnSupport_Btn( caster );
    }

    void _onClick_BtnSupportCancel_Btn(GameObject caster)
    {
        onClick_BtnSupportCancel_Btn( caster );
    }


}
