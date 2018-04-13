//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ForgingPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//合成
		HeCheng = 2,
		//加工
		JiaGong = 3,
		//镶嵌
		XiangQian = 4,
		//强化
		QiangHua = 5,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_RightContent;

    Transform            m_trans_FunctionConent;

    Transform            m_trans_RefineInfoRoot;

    UISlider             m_slider_RefineSuccessSlider;

    UILabel              m_label_RefineSuccessProp;

    Transform            m_trans_RefineEquipPropertyContent;

    UILabel              m_label_RefineCurLv;

    UILabel              m_label_RefineTargetLv;

    UILabel              m_label_RefineMaxLv;

    Transform            m_trans_RefineEquipProperty1;

    Transform            m_trans_RefineEquipProperty2;

    Transform            m_trans_RefineCost;

    UIGrid               m_grid_CostGridContent;

    Transform            m_trans_RefineAssist;

    UIButton             m_btn_UseDQToggleRefine;

    UIGrid               m_grid_AssistGridContent;

    UIButton             m_btn_BtnRefine;

    Transform            m_trans_UICGRefine;

    Transform            m_trans_RefineMax;

    UILabel              m_label_RefineMaxActiveProp;

    Transform            m_trans_RefineDisable;

    Transform            m_trans_RefineDisableProperty1;

    Transform            m_trans_EquipGridInLayContent;

    UILabel              m_label_InlayGemAttrDes;

    UIScrollView         m_scrollview_GemScrollView;

    UISprite             m_sprite_bg;

    UIButton             m_btn_BtnItemCompound;

    UIButton             m_btn_BtnItemBuy;

    UIButton             m_btn_ForgingUItips_2;

    UILabel              m_label_CompoundWarningTxt;

    Transform            m_trans_CompoundGrowShowRoot;

    Transform            m_trans_CompoundZFRoot;

    UIButton             m_btn_CompoundBtnZF;

    Transform            m_trans_ZFRoot;

    Transform            m_trans_CompoundMainAttrRoot;

    Transform            m_trans_CompoundDeputyContent;

    UILabel              m_label_CompoundNoticeTxt;

    UIButton             m_btn_BtnCompound;

    Transform            m_trans_UICGCompoundCost;

    Transform            m_trans_CompoundZFContent;

    Transform            m_trans_ProtectScrollView;

    UIButton             m_btn_ForgingUItips_3;

    Transform            m_trans_ProccessBottom;

    Transform            m_trans_ProccessCost;

    UIButton             m_btn_BtnFetch;

    UIButton             m_btn_BtnPromote;

    UIButton             m_btn_BtnRemove;

    Transform            m_trans_ProccessPropertyContentRoot;

    UIButton             m_btn_OneCB;

    UIButton             m_btn_TwoCB;

    UIButton             m_btn_ThreeCB;

    UIButton             m_btn_FourCB;

    UIButton             m_btn_FiveCB;

    Transform            m_trans_ProccessTag;

    Transform            m_trans_ArrowPromote;

    UILabel              m_label_ArrowPromoteTxt;

    UILabel              m_label_ProccessTips;

    Transform            m_trans_ProccessAssist;

    UILabel              m_label_ProcessCostTxt;

    Transform            m_trans_ProcessCostRoot;

    Transform            m_trans_ProcessInfoRoot;

    Transform            m_trans_ProcessTabs;

    UILabel              m_label_ProccessWarningTxt;

    UIButton             m_btn_ForgingUItips_4;

    Transform            m_trans_ScorllAreaEquip;

    UIGridCreatorBase    m_ctor_EquipScrollView;

    Transform            m_trans_ListTab;

    UISprite             m_sprite_EquipFilterArrow;

    Transform            m_trans_EquipFilter;

    UIGridCreatorBase    m_ctor_EquipFilterScrollView;

    UISprite             m_sprite_EquipFilterBg;

    Transform            m_trans_ScorllAreaInlay;

    UIGridCreatorBase    m_ctor_InlayScrollView;

    Transform            m_trans_PartTab;

    Transform            m_trans_NullEquipTipsContent;

    Transform            m_trans_StrengthenInfoRoot;

    UILabel              m_label_StrengthenPosName;

    UISprite             m_sprite_StrengthenPosIcon;

    Transform            m_trans_StrengthenEquipPropertyContent;

    UILabel              m_label_StrengthenCurLv;

    UILabel              m_label_StrengthenTargetLv;

    UILabel              m_label_StrengthenMaxLv;

    Transform            m_trans_StrengthenEquipProperty1;

    Transform            m_trans_StrengthenEquipProperty2;

    Transform            m_trans_StrengthenCost;

    UIGrid               m_grid_StrengthenCostGridContent;

    Transform            m_trans_StrengthenMax;

    Transform            m_trans_StrengthenBtns;

    UIButton             m_btn_AllSrengthen;

    Transform            m_trans_AllSrengthenCurrency;

    UIButton             m_btn_SingleStrengthen;

    Transform            m_trans_SingleStrengthenCurrency;

    UISprite             m_sprite_SingleStrengthenRedPoint;

    UIButton             m_btn_StrengthenSuitBtn;

    UILabel              m_label_ActiveSuitLvTxt;

    UIButton             m_btn_ForgingUItips_1;

    Transform            m_trans_UIEquipInfoGrid;

    Transform            m_trans_UIEquipPropertyProtectGrid;

    UIButton             m_btn_Toggle;

    Transform            m_trans_UIEquipFilterGrid;

    Transform            m_trans_UIEquipPosStatusGrid;

    Transform            m_trans_UIItemGrowCostGrid;

    Transform            m_trans_CostDQ;

    Transform            m_trans_UIZFGrid;

    Transform            m_trans_UIRSInfoSelectGrid;


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
        m_trans_RightContent = fastComponent.FastGetComponent<Transform>("RightContent");
       if( null == m_trans_RightContent )
       {
            Engine.Utility.Log.Error("m_trans_RightContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FunctionConent = fastComponent.FastGetComponent<Transform>("FunctionConent");
       if( null == m_trans_FunctionConent )
       {
            Engine.Utility.Log.Error("m_trans_FunctionConent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefineInfoRoot = fastComponent.FastGetComponent<Transform>("RefineInfoRoot");
       if( null == m_trans_RefineInfoRoot )
       {
            Engine.Utility.Log.Error("m_trans_RefineInfoRoot 为空，请检查prefab是否缺乏组件");
       }
        m_slider_RefineSuccessSlider = fastComponent.FastGetComponent<UISlider>("RefineSuccessSlider");
       if( null == m_slider_RefineSuccessSlider )
       {
            Engine.Utility.Log.Error("m_slider_RefineSuccessSlider 为空，请检查prefab是否缺乏组件");
       }
        m_label_RefineSuccessProp = fastComponent.FastGetComponent<UILabel>("RefineSuccessProp");
       if( null == m_label_RefineSuccessProp )
       {
            Engine.Utility.Log.Error("m_label_RefineSuccessProp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefineEquipPropertyContent = fastComponent.FastGetComponent<Transform>("RefineEquipPropertyContent");
       if( null == m_trans_RefineEquipPropertyContent )
       {
            Engine.Utility.Log.Error("m_trans_RefineEquipPropertyContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_RefineCurLv = fastComponent.FastGetComponent<UILabel>("RefineCurLv");
       if( null == m_label_RefineCurLv )
       {
            Engine.Utility.Log.Error("m_label_RefineCurLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_RefineTargetLv = fastComponent.FastGetComponent<UILabel>("RefineTargetLv");
       if( null == m_label_RefineTargetLv )
       {
            Engine.Utility.Log.Error("m_label_RefineTargetLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_RefineMaxLv = fastComponent.FastGetComponent<UILabel>("RefineMaxLv");
       if( null == m_label_RefineMaxLv )
       {
            Engine.Utility.Log.Error("m_label_RefineMaxLv 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefineEquipProperty1 = fastComponent.FastGetComponent<Transform>("RefineEquipProperty1");
       if( null == m_trans_RefineEquipProperty1 )
       {
            Engine.Utility.Log.Error("m_trans_RefineEquipProperty1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefineEquipProperty2 = fastComponent.FastGetComponent<Transform>("RefineEquipProperty2");
       if( null == m_trans_RefineEquipProperty2 )
       {
            Engine.Utility.Log.Error("m_trans_RefineEquipProperty2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefineCost = fastComponent.FastGetComponent<Transform>("RefineCost");
       if( null == m_trans_RefineCost )
       {
            Engine.Utility.Log.Error("m_trans_RefineCost 为空，请检查prefab是否缺乏组件");
       }
        m_grid_CostGridContent = fastComponent.FastGetComponent<UIGrid>("CostGridContent");
       if( null == m_grid_CostGridContent )
       {
            Engine.Utility.Log.Error("m_grid_CostGridContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefineAssist = fastComponent.FastGetComponent<Transform>("RefineAssist");
       if( null == m_trans_RefineAssist )
       {
            Engine.Utility.Log.Error("m_trans_RefineAssist 为空，请检查prefab是否缺乏组件");
       }
        m_btn_UseDQToggleRefine = fastComponent.FastGetComponent<UIButton>("UseDQToggleRefine");
       if( null == m_btn_UseDQToggleRefine )
       {
            Engine.Utility.Log.Error("m_btn_UseDQToggleRefine 为空，请检查prefab是否缺乏组件");
       }
        m_grid_AssistGridContent = fastComponent.FastGetComponent<UIGrid>("AssistGridContent");
       if( null == m_grid_AssistGridContent )
       {
            Engine.Utility.Log.Error("m_grid_AssistGridContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnRefine = fastComponent.FastGetComponent<UIButton>("BtnRefine");
       if( null == m_btn_BtnRefine )
       {
            Engine.Utility.Log.Error("m_btn_BtnRefine 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UICGRefine = fastComponent.FastGetComponent<Transform>("UICGRefine");
       if( null == m_trans_UICGRefine )
       {
            Engine.Utility.Log.Error("m_trans_UICGRefine 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefineMax = fastComponent.FastGetComponent<Transform>("RefineMax");
       if( null == m_trans_RefineMax )
       {
            Engine.Utility.Log.Error("m_trans_RefineMax 为空，请检查prefab是否缺乏组件");
       }
        m_label_RefineMaxActiveProp = fastComponent.FastGetComponent<UILabel>("RefineMaxActiveProp");
       if( null == m_label_RefineMaxActiveProp )
       {
            Engine.Utility.Log.Error("m_label_RefineMaxActiveProp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefineDisable = fastComponent.FastGetComponent<Transform>("RefineDisable");
       if( null == m_trans_RefineDisable )
       {
            Engine.Utility.Log.Error("m_trans_RefineDisable 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefineDisableProperty1 = fastComponent.FastGetComponent<Transform>("RefineDisableProperty1");
       if( null == m_trans_RefineDisableProperty1 )
       {
            Engine.Utility.Log.Error("m_trans_RefineDisableProperty1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EquipGridInLayContent = fastComponent.FastGetComponent<Transform>("EquipGridInLayContent");
       if( null == m_trans_EquipGridInLayContent )
       {
            Engine.Utility.Log.Error("m_trans_EquipGridInLayContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_InlayGemAttrDes = fastComponent.FastGetComponent<UILabel>("InlayGemAttrDes");
       if( null == m_label_InlayGemAttrDes )
       {
            Engine.Utility.Log.Error("m_label_InlayGemAttrDes 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_GemScrollView = fastComponent.FastGetComponent<UIScrollView>("GemScrollView");
       if( null == m_scrollview_GemScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_GemScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_bg = fastComponent.FastGetComponent<UISprite>("bg");
       if( null == m_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnItemCompound = fastComponent.FastGetComponent<UIButton>("BtnItemCompound");
       if( null == m_btn_BtnItemCompound )
       {
            Engine.Utility.Log.Error("m_btn_BtnItemCompound 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnItemBuy = fastComponent.FastGetComponent<UIButton>("BtnItemBuy");
       if( null == m_btn_BtnItemBuy )
       {
            Engine.Utility.Log.Error("m_btn_BtnItemBuy 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ForgingUItips_2 = fastComponent.FastGetComponent<UIButton>("ForgingUItips_2");
       if( null == m_btn_ForgingUItips_2 )
       {
            Engine.Utility.Log.Error("m_btn_ForgingUItips_2 为空，请检查prefab是否缺乏组件");
       }
        m_label_CompoundWarningTxt = fastComponent.FastGetComponent<UILabel>("CompoundWarningTxt");
       if( null == m_label_CompoundWarningTxt )
       {
            Engine.Utility.Log.Error("m_label_CompoundWarningTxt 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CompoundGrowShowRoot = fastComponent.FastGetComponent<Transform>("CompoundGrowShowRoot");
       if( null == m_trans_CompoundGrowShowRoot )
       {
            Engine.Utility.Log.Error("m_trans_CompoundGrowShowRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CompoundZFRoot = fastComponent.FastGetComponent<Transform>("CompoundZFRoot");
       if( null == m_trans_CompoundZFRoot )
       {
            Engine.Utility.Log.Error("m_trans_CompoundZFRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CompoundBtnZF = fastComponent.FastGetComponent<UIButton>("CompoundBtnZF");
       if( null == m_btn_CompoundBtnZF )
       {
            Engine.Utility.Log.Error("m_btn_CompoundBtnZF 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ZFRoot = fastComponent.FastGetComponent<Transform>("ZFRoot");
       if( null == m_trans_ZFRoot )
       {
            Engine.Utility.Log.Error("m_trans_ZFRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CompoundMainAttrRoot = fastComponent.FastGetComponent<Transform>("CompoundMainAttrRoot");
       if( null == m_trans_CompoundMainAttrRoot )
       {
            Engine.Utility.Log.Error("m_trans_CompoundMainAttrRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CompoundDeputyContent = fastComponent.FastGetComponent<Transform>("CompoundDeputyContent");
       if( null == m_trans_CompoundDeputyContent )
       {
            Engine.Utility.Log.Error("m_trans_CompoundDeputyContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_CompoundNoticeTxt = fastComponent.FastGetComponent<UILabel>("CompoundNoticeTxt");
       if( null == m_label_CompoundNoticeTxt )
       {
            Engine.Utility.Log.Error("m_label_CompoundNoticeTxt 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnCompound = fastComponent.FastGetComponent<UIButton>("BtnCompound");
       if( null == m_btn_BtnCompound )
       {
            Engine.Utility.Log.Error("m_btn_BtnCompound 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UICGCompoundCost = fastComponent.FastGetComponent<Transform>("UICGCompoundCost");
       if( null == m_trans_UICGCompoundCost )
       {
            Engine.Utility.Log.Error("m_trans_UICGCompoundCost 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CompoundZFContent = fastComponent.FastGetComponent<Transform>("CompoundZFContent");
       if( null == m_trans_CompoundZFContent )
       {
            Engine.Utility.Log.Error("m_trans_CompoundZFContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ProtectScrollView = fastComponent.FastGetComponent<Transform>("ProtectScrollView");
       if( null == m_trans_ProtectScrollView )
       {
            Engine.Utility.Log.Error("m_trans_ProtectScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ForgingUItips_3 = fastComponent.FastGetComponent<UIButton>("ForgingUItips_3");
       if( null == m_btn_ForgingUItips_3 )
       {
            Engine.Utility.Log.Error("m_btn_ForgingUItips_3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ProccessBottom = fastComponent.FastGetComponent<Transform>("ProccessBottom");
       if( null == m_trans_ProccessBottom )
       {
            Engine.Utility.Log.Error("m_trans_ProccessBottom 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ProccessCost = fastComponent.FastGetComponent<Transform>("ProccessCost");
       if( null == m_trans_ProccessCost )
       {
            Engine.Utility.Log.Error("m_trans_ProccessCost 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnFetch = fastComponent.FastGetComponent<UIButton>("BtnFetch");
       if( null == m_btn_BtnFetch )
       {
            Engine.Utility.Log.Error("m_btn_BtnFetch 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnPromote = fastComponent.FastGetComponent<UIButton>("BtnPromote");
       if( null == m_btn_BtnPromote )
       {
            Engine.Utility.Log.Error("m_btn_BtnPromote 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnRemove = fastComponent.FastGetComponent<UIButton>("BtnRemove");
       if( null == m_btn_BtnRemove )
       {
            Engine.Utility.Log.Error("m_btn_BtnRemove 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ProccessPropertyContentRoot = fastComponent.FastGetComponent<Transform>("ProccessPropertyContentRoot");
       if( null == m_trans_ProccessPropertyContentRoot )
       {
            Engine.Utility.Log.Error("m_trans_ProccessPropertyContentRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_OneCB = fastComponent.FastGetComponent<UIButton>("OneCB");
       if( null == m_btn_OneCB )
       {
            Engine.Utility.Log.Error("m_btn_OneCB 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TwoCB = fastComponent.FastGetComponent<UIButton>("TwoCB");
       if( null == m_btn_TwoCB )
       {
            Engine.Utility.Log.Error("m_btn_TwoCB 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ThreeCB = fastComponent.FastGetComponent<UIButton>("ThreeCB");
       if( null == m_btn_ThreeCB )
       {
            Engine.Utility.Log.Error("m_btn_ThreeCB 为空，请检查prefab是否缺乏组件");
       }
        m_btn_FourCB = fastComponent.FastGetComponent<UIButton>("FourCB");
       if( null == m_btn_FourCB )
       {
            Engine.Utility.Log.Error("m_btn_FourCB 为空，请检查prefab是否缺乏组件");
       }
        m_btn_FiveCB = fastComponent.FastGetComponent<UIButton>("FiveCB");
       if( null == m_btn_FiveCB )
       {
            Engine.Utility.Log.Error("m_btn_FiveCB 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ProccessTag = fastComponent.FastGetComponent<Transform>("ProccessTag");
       if( null == m_trans_ProccessTag )
       {
            Engine.Utility.Log.Error("m_trans_ProccessTag 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ArrowPromote = fastComponent.FastGetComponent<Transform>("ArrowPromote");
       if( null == m_trans_ArrowPromote )
       {
            Engine.Utility.Log.Error("m_trans_ArrowPromote 为空，请检查prefab是否缺乏组件");
       }
        m_label_ArrowPromoteTxt = fastComponent.FastGetComponent<UILabel>("ArrowPromoteTxt");
       if( null == m_label_ArrowPromoteTxt )
       {
            Engine.Utility.Log.Error("m_label_ArrowPromoteTxt 为空，请检查prefab是否缺乏组件");
       }
        m_label_ProccessTips = fastComponent.FastGetComponent<UILabel>("ProccessTips");
       if( null == m_label_ProccessTips )
       {
            Engine.Utility.Log.Error("m_label_ProccessTips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ProccessAssist = fastComponent.FastGetComponent<Transform>("ProccessAssist");
       if( null == m_trans_ProccessAssist )
       {
            Engine.Utility.Log.Error("m_trans_ProccessAssist 为空，请检查prefab是否缺乏组件");
       }
        m_label_ProcessCostTxt = fastComponent.FastGetComponent<UILabel>("ProcessCostTxt");
       if( null == m_label_ProcessCostTxt )
       {
            Engine.Utility.Log.Error("m_label_ProcessCostTxt 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ProcessCostRoot = fastComponent.FastGetComponent<Transform>("ProcessCostRoot");
       if( null == m_trans_ProcessCostRoot )
       {
            Engine.Utility.Log.Error("m_trans_ProcessCostRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ProcessInfoRoot = fastComponent.FastGetComponent<Transform>("ProcessInfoRoot");
       if( null == m_trans_ProcessInfoRoot )
       {
            Engine.Utility.Log.Error("m_trans_ProcessInfoRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ProcessTabs = fastComponent.FastGetComponent<Transform>("ProcessTabs");
       if( null == m_trans_ProcessTabs )
       {
            Engine.Utility.Log.Error("m_trans_ProcessTabs 为空，请检查prefab是否缺乏组件");
       }
        m_label_ProccessWarningTxt = fastComponent.FastGetComponent<UILabel>("ProccessWarningTxt");
       if( null == m_label_ProccessWarningTxt )
       {
            Engine.Utility.Log.Error("m_label_ProccessWarningTxt 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ForgingUItips_4 = fastComponent.FastGetComponent<UIButton>("ForgingUItips_4");
       if( null == m_btn_ForgingUItips_4 )
       {
            Engine.Utility.Log.Error("m_btn_ForgingUItips_4 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ScorllAreaEquip = fastComponent.FastGetComponent<Transform>("ScorllAreaEquip");
       if( null == m_trans_ScorllAreaEquip )
       {
            Engine.Utility.Log.Error("m_trans_ScorllAreaEquip 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_EquipScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("EquipScrollView");
       if( null == m_ctor_EquipScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_EquipScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ListTab = fastComponent.FastGetComponent<Transform>("ListTab");
       if( null == m_trans_ListTab )
       {
            Engine.Utility.Log.Error("m_trans_ListTab 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_EquipFilterArrow = fastComponent.FastGetComponent<UISprite>("EquipFilterArrow");
       if( null == m_sprite_EquipFilterArrow )
       {
            Engine.Utility.Log.Error("m_sprite_EquipFilterArrow 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EquipFilter = fastComponent.FastGetComponent<Transform>("EquipFilter");
       if( null == m_trans_EquipFilter )
       {
            Engine.Utility.Log.Error("m_trans_EquipFilter 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_EquipFilterScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("EquipFilterScrollView");
       if( null == m_ctor_EquipFilterScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_EquipFilterScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_EquipFilterBg = fastComponent.FastGetComponent<UISprite>("EquipFilterBg");
       if( null == m_sprite_EquipFilterBg )
       {
            Engine.Utility.Log.Error("m_sprite_EquipFilterBg 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ScorllAreaInlay = fastComponent.FastGetComponent<Transform>("ScorllAreaInlay");
       if( null == m_trans_ScorllAreaInlay )
       {
            Engine.Utility.Log.Error("m_trans_ScorllAreaInlay 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_InlayScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("InlayScrollView");
       if( null == m_ctor_InlayScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_InlayScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartTab = fastComponent.FastGetComponent<Transform>("PartTab");
       if( null == m_trans_PartTab )
       {
            Engine.Utility.Log.Error("m_trans_PartTab 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NullEquipTipsContent = fastComponent.FastGetComponent<Transform>("NullEquipTipsContent");
       if( null == m_trans_NullEquipTipsContent )
       {
            Engine.Utility.Log.Error("m_trans_NullEquipTipsContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_StrengthenInfoRoot = fastComponent.FastGetComponent<Transform>("StrengthenInfoRoot");
       if( null == m_trans_StrengthenInfoRoot )
       {
            Engine.Utility.Log.Error("m_trans_StrengthenInfoRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_StrengthenPosName = fastComponent.FastGetComponent<UILabel>("StrengthenPosName");
       if( null == m_label_StrengthenPosName )
       {
            Engine.Utility.Log.Error("m_label_StrengthenPosName 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_StrengthenPosIcon = fastComponent.FastGetComponent<UISprite>("StrengthenPosIcon");
       if( null == m_sprite_StrengthenPosIcon )
       {
            Engine.Utility.Log.Error("m_sprite_StrengthenPosIcon 为空，请检查prefab是否缺乏组件");
       }
        m_trans_StrengthenEquipPropertyContent = fastComponent.FastGetComponent<Transform>("StrengthenEquipPropertyContent");
       if( null == m_trans_StrengthenEquipPropertyContent )
       {
            Engine.Utility.Log.Error("m_trans_StrengthenEquipPropertyContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_StrengthenCurLv = fastComponent.FastGetComponent<UILabel>("StrengthenCurLv");
       if( null == m_label_StrengthenCurLv )
       {
            Engine.Utility.Log.Error("m_label_StrengthenCurLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_StrengthenTargetLv = fastComponent.FastGetComponent<UILabel>("StrengthenTargetLv");
       if( null == m_label_StrengthenTargetLv )
       {
            Engine.Utility.Log.Error("m_label_StrengthenTargetLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_StrengthenMaxLv = fastComponent.FastGetComponent<UILabel>("StrengthenMaxLv");
       if( null == m_label_StrengthenMaxLv )
       {
            Engine.Utility.Log.Error("m_label_StrengthenMaxLv 为空，请检查prefab是否缺乏组件");
       }
        m_trans_StrengthenEquipProperty1 = fastComponent.FastGetComponent<Transform>("StrengthenEquipProperty1");
       if( null == m_trans_StrengthenEquipProperty1 )
       {
            Engine.Utility.Log.Error("m_trans_StrengthenEquipProperty1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_StrengthenEquipProperty2 = fastComponent.FastGetComponent<Transform>("StrengthenEquipProperty2");
       if( null == m_trans_StrengthenEquipProperty2 )
       {
            Engine.Utility.Log.Error("m_trans_StrengthenEquipProperty2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_StrengthenCost = fastComponent.FastGetComponent<Transform>("StrengthenCost");
       if( null == m_trans_StrengthenCost )
       {
            Engine.Utility.Log.Error("m_trans_StrengthenCost 为空，请检查prefab是否缺乏组件");
       }
        m_grid_StrengthenCostGridContent = fastComponent.FastGetComponent<UIGrid>("StrengthenCostGridContent");
       if( null == m_grid_StrengthenCostGridContent )
       {
            Engine.Utility.Log.Error("m_grid_StrengthenCostGridContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_StrengthenMax = fastComponent.FastGetComponent<Transform>("StrengthenMax");
       if( null == m_trans_StrengthenMax )
       {
            Engine.Utility.Log.Error("m_trans_StrengthenMax 为空，请检查prefab是否缺乏组件");
       }
        m_trans_StrengthenBtns = fastComponent.FastGetComponent<Transform>("StrengthenBtns");
       if( null == m_trans_StrengthenBtns )
       {
            Engine.Utility.Log.Error("m_trans_StrengthenBtns 为空，请检查prefab是否缺乏组件");
       }
        m_btn_AllSrengthen = fastComponent.FastGetComponent<UIButton>("AllSrengthen");
       if( null == m_btn_AllSrengthen )
       {
            Engine.Utility.Log.Error("m_btn_AllSrengthen 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AllSrengthenCurrency = fastComponent.FastGetComponent<Transform>("AllSrengthenCurrency");
       if( null == m_trans_AllSrengthenCurrency )
       {
            Engine.Utility.Log.Error("m_trans_AllSrengthenCurrency 为空，请检查prefab是否缺乏组件");
       }
        m_btn_SingleStrengthen = fastComponent.FastGetComponent<UIButton>("SingleStrengthen");
       if( null == m_btn_SingleStrengthen )
       {
            Engine.Utility.Log.Error("m_btn_SingleStrengthen 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SingleStrengthenCurrency = fastComponent.FastGetComponent<Transform>("SingleStrengthenCurrency");
       if( null == m_trans_SingleStrengthenCurrency )
       {
            Engine.Utility.Log.Error("m_trans_SingleStrengthenCurrency 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_SingleStrengthenRedPoint = fastComponent.FastGetComponent<UISprite>("SingleStrengthenRedPoint");
       if( null == m_sprite_SingleStrengthenRedPoint )
       {
            Engine.Utility.Log.Error("m_sprite_SingleStrengthenRedPoint 为空，请检查prefab是否缺乏组件");
       }
        m_btn_StrengthenSuitBtn = fastComponent.FastGetComponent<UIButton>("StrengthenSuitBtn");
       if( null == m_btn_StrengthenSuitBtn )
       {
            Engine.Utility.Log.Error("m_btn_StrengthenSuitBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveSuitLvTxt = fastComponent.FastGetComponent<UILabel>("ActiveSuitLvTxt");
       if( null == m_label_ActiveSuitLvTxt )
       {
            Engine.Utility.Log.Error("m_label_ActiveSuitLvTxt 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ForgingUItips_1 = fastComponent.FastGetComponent<UIButton>("ForgingUItips_1");
       if( null == m_btn_ForgingUItips_1 )
       {
            Engine.Utility.Log.Error("m_btn_ForgingUItips_1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIEquipInfoGrid = fastComponent.FastGetComponent<Transform>("UIEquipInfoGrid");
       if( null == m_trans_UIEquipInfoGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIEquipInfoGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIEquipPropertyProtectGrid = fastComponent.FastGetComponent<Transform>("UIEquipPropertyProtectGrid");
       if( null == m_trans_UIEquipPropertyProtectGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIEquipPropertyProtectGrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Toggle = fastComponent.FastGetComponent<UIButton>("Toggle");
       if( null == m_btn_Toggle )
       {
            Engine.Utility.Log.Error("m_btn_Toggle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIEquipFilterGrid = fastComponent.FastGetComponent<Transform>("UIEquipFilterGrid");
       if( null == m_trans_UIEquipFilterGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIEquipFilterGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIEquipPosStatusGrid = fastComponent.FastGetComponent<Transform>("UIEquipPosStatusGrid");
       if( null == m_trans_UIEquipPosStatusGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIEquipPosStatusGrid 为空，请检查prefab是否缺乏组件");
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
        m_trans_UIZFGrid = fastComponent.FastGetComponent<Transform>("UIZFGrid");
       if( null == m_trans_UIZFGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIZFGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIRSInfoSelectGrid = fastComponent.FastGetComponent<Transform>("UIRSInfoSelectGrid");
       if( null == m_trans_UIRSInfoSelectGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIRSInfoSelectGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_UseDQToggleRefine.gameObject).onClick = _onClick_UseDQToggleRefine_Btn;
        UIEventListener.Get(m_btn_BtnRefine.gameObject).onClick = _onClick_BtnRefine_Btn;
        UIEventListener.Get(m_btn_BtnItemCompound.gameObject).onClick = _onClick_BtnItemCompound_Btn;
        UIEventListener.Get(m_btn_BtnItemBuy.gameObject).onClick = _onClick_BtnItemBuy_Btn;
        UIEventListener.Get(m_btn_ForgingUItips_2.gameObject).onClick = _onClick_ForgingUItips_2_Btn;
        UIEventListener.Get(m_btn_CompoundBtnZF.gameObject).onClick = _onClick_CompoundBtnZF_Btn;
        UIEventListener.Get(m_btn_BtnCompound.gameObject).onClick = _onClick_BtnCompound_Btn;
        UIEventListener.Get(m_btn_ForgingUItips_3.gameObject).onClick = _onClick_ForgingUItips_3_Btn;
        UIEventListener.Get(m_btn_BtnFetch.gameObject).onClick = _onClick_BtnFetch_Btn;
        UIEventListener.Get(m_btn_BtnPromote.gameObject).onClick = _onClick_BtnPromote_Btn;
        UIEventListener.Get(m_btn_BtnRemove.gameObject).onClick = _onClick_BtnRemove_Btn;
        UIEventListener.Get(m_btn_OneCB.gameObject).onClick = _onClick_OneCB_Btn;
        UIEventListener.Get(m_btn_TwoCB.gameObject).onClick = _onClick_TwoCB_Btn;
        UIEventListener.Get(m_btn_ThreeCB.gameObject).onClick = _onClick_ThreeCB_Btn;
        UIEventListener.Get(m_btn_FourCB.gameObject).onClick = _onClick_FourCB_Btn;
        UIEventListener.Get(m_btn_FiveCB.gameObject).onClick = _onClick_FiveCB_Btn;
        UIEventListener.Get(m_btn_ForgingUItips_4.gameObject).onClick = _onClick_ForgingUItips_4_Btn;
        UIEventListener.Get(m_btn_AllSrengthen.gameObject).onClick = _onClick_AllSrengthen_Btn;
        UIEventListener.Get(m_btn_SingleStrengthen.gameObject).onClick = _onClick_SingleStrengthen_Btn;
        UIEventListener.Get(m_btn_StrengthenSuitBtn.gameObject).onClick = _onClick_StrengthenSuitBtn_Btn;
        UIEventListener.Get(m_btn_ForgingUItips_1.gameObject).onClick = _onClick_ForgingUItips_1_Btn;
        UIEventListener.Get(m_btn_Toggle.gameObject).onClick = _onClick_Toggle_Btn;
    }

    void _onClick_UseDQToggleRefine_Btn(GameObject caster)
    {
        onClick_UseDQToggleRefine_Btn( caster );
    }

    void _onClick_BtnRefine_Btn(GameObject caster)
    {
        onClick_BtnRefine_Btn( caster );
    }

    void _onClick_BtnItemCompound_Btn(GameObject caster)
    {
        onClick_BtnItemCompound_Btn( caster );
    }

    void _onClick_BtnItemBuy_Btn(GameObject caster)
    {
        onClick_BtnItemBuy_Btn( caster );
    }

    void _onClick_ForgingUItips_2_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_CompoundBtnZF_Btn(GameObject caster)
    {
        onClick_CompoundBtnZF_Btn( caster );
    }

    void _onClick_BtnCompound_Btn(GameObject caster)
    {
        onClick_BtnCompound_Btn( caster );
    }

    void _onClick_ForgingUItips_3_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_BtnFetch_Btn(GameObject caster)
    {
        onClick_BtnFetch_Btn( caster );
    }

    void _onClick_BtnPromote_Btn(GameObject caster)
    {
        onClick_BtnPromote_Btn( caster );
    }

    void _onClick_BtnRemove_Btn(GameObject caster)
    {
        onClick_BtnRemove_Btn( caster );
    }

    void _onClick_OneCB_Btn(GameObject caster)
    {
        onClick_OneCB_Btn( caster );
    }

    void _onClick_TwoCB_Btn(GameObject caster)
    {
        onClick_TwoCB_Btn( caster );
    }

    void _onClick_ThreeCB_Btn(GameObject caster)
    {
        onClick_ThreeCB_Btn( caster );
    }

    void _onClick_FourCB_Btn(GameObject caster)
    {
        onClick_FourCB_Btn( caster );
    }

    void _onClick_FiveCB_Btn(GameObject caster)
    {
        onClick_FiveCB_Btn( caster );
    }

    void _onClick_ForgingUItips_4_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_AllSrengthen_Btn(GameObject caster)
    {
        onClick_AllSrengthen_Btn( caster );
    }

    void _onClick_SingleStrengthen_Btn(GameObject caster)
    {
        onClick_SingleStrengthen_Btn( caster );
    }

    void _onClick_StrengthenSuitBtn_Btn(GameObject caster)
    {
        onClick_StrengthenSuitBtn_Btn( caster );
    }

    void _onClick_ForgingUItips_1_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_Toggle_Btn(GameObject caster)
    {
        onClick_Toggle_Btn( caster );
    }


}
