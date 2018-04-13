//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MuhonPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//升级
		ShengJi = 1,
		//激活
		JiHuo = 2,
		//升星
		JinHua = 3,
		//融合
		RongHe = 4,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_FunctioToggles;

    UIGridCreatorBase    m_ctor_MohonGridScrollView;

    Transform            m_trans_RightContent;

    Transform            m_trans_FunctionConent;

    Transform            m_trans_PromoteProperty;

    Transform            m_trans_BaseProperty1;

    Transform            m_trans_BaseProperty2;

    Transform            m_trans_BaseProperty3;

    Transform            m_trans_BaseProperty4;

    Transform            m_trans_BasePropertyLevel;

    UIGrid               m_grid_PromoteExpRoot;

    UILabel              m_label_Level;

    UILabel              m_label_PromoteExpPencentage;

    UISlider             m_slider_PromoteExpProgress;

    Transform            m_trans_PromoteItemGrowRoot;

    UILabel              m_label_ActiveRemoveStateTips1;

    Transform            m_trans_ActiveRemovePage;

    UILabel              m_label_Title_label;

    UIGrid               m_grid_ActivePropertyRoot;

    UILabel              m_label_ActiveRemoveStateTips2;

    Transform            m_trans_ActiveRemoveItemGrowRoot;

    UILabel              m_label_ActiveRemoveName;

    UIButton             m_btn_MuhonUItips_3;

    Transform            m_trans_EvolveInfos;

    UIButton             m_btn_BtnEvolvePre;

    Transform            m_trans_EvolvePreview;

    UISlider             m_slider_EvolveCurStarLv;

    UISlider             m_slider_EvolveNextStarLv;

    UILabel              m_label_EvolveCurLv;

    UILabel              m_label_EvolveNextLv;

    UILabel              m_label_EvolveCurAttrNum;

    UILabel              m_label_EvolveNextAttrNum;

    UIWidget             m_widget_PreviewCollider;

    Transform            m_trans_EvolveGrowRoot;

    UILabel              m_label_EvolveMuhonLv;

    Transform            m_trans_EvolveTargeParticle;

    Transform            m_trans_CostEvolveMuhon;

    Transform            m_trans_CostEvolveStar;

    Transform            m_trans_EvolveStarLightParticle;

    UILabel              m_label_EvolveEffetDepthLimitMask;

    UILabel              m_label_EvolvePlayerLvLmit;

    Transform            m_trans_EvolveMax;

    Transform            m_trans_EvolveMaxGrowRoot;

    UISprite             m_sprite_EvolveAttrTitle;

    UISprite             m_sprite_EvolveAttr1;

    UISprite             m_sprite_EvolveAttr2;

    UISprite             m_sprite_EvolveAttr3;

    UISprite             m_sprite_EvolveAttr4;

    UIButton             m_btn_MuhonUItips_2;

    UIGrid               m_grid_CurrentAdditiveAttrContent;

    Transform            m_trans_BlendCurGrowRoot;

    Transform            m_trans_BlendCurShowGrid;

    Transform            m_trans_BlendNextGrowRoot;

    UIButton             m_btn_BlendUnload;

    Transform            m_trans_BlendNextShowGrid;

    UIGrid               m_grid_BlendAdditiveAttrContent;

    Transform            m_trans_BlendTips;

    UIButton             m_btn_MuhonUItips_4;

    Transform            m_trans_CmCostBottom;

    Transform            m_trans_AssistContentRoot;

    UIButton             m_btn_AutoUseDQ;

    Transform            m_trans_OPCost;

    UIButton             m_btn_BlendBtn;

    UIButton             m_btn_ActiveBtn;

    UIButton             m_btn_RemoveBtn;

    UIButton             m_btn_EvolveBtn;

    Transform            m_trans_NullMuhonTipsContent;

    Transform            m_trans_SXInfos;

    UILabel              m_label_SXTips;

    Transform            m_trans_SXGrowRoot;

    UILabel              m_label_SXLv;

    Transform            m_trans_StarRoot;

    Transform            m_trans_DeputyMuhonRoot;

    Transform            m_trans_SXMax;

    Transform            m_trans_UIWeaponSoulInfoGrid;

    Transform            m_trans_UIMuhonExpGrid;


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
        m_trans_FunctioToggles = fastComponent.FastGetComponent<Transform>("FunctioToggles");
       if( null == m_trans_FunctioToggles )
       {
            Engine.Utility.Log.Error("m_trans_FunctioToggles 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_MohonGridScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("MohonGridScrollView");
       if( null == m_ctor_MohonGridScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_MohonGridScrollView 为空，请检查prefab是否缺乏组件");
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
        m_trans_PromoteProperty = fastComponent.FastGetComponent<Transform>("PromoteProperty");
       if( null == m_trans_PromoteProperty )
       {
            Engine.Utility.Log.Error("m_trans_PromoteProperty 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BaseProperty1 = fastComponent.FastGetComponent<Transform>("BaseProperty1");
       if( null == m_trans_BaseProperty1 )
       {
            Engine.Utility.Log.Error("m_trans_BaseProperty1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BaseProperty2 = fastComponent.FastGetComponent<Transform>("BaseProperty2");
       if( null == m_trans_BaseProperty2 )
       {
            Engine.Utility.Log.Error("m_trans_BaseProperty2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BaseProperty3 = fastComponent.FastGetComponent<Transform>("BaseProperty3");
       if( null == m_trans_BaseProperty3 )
       {
            Engine.Utility.Log.Error("m_trans_BaseProperty3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BaseProperty4 = fastComponent.FastGetComponent<Transform>("BaseProperty4");
       if( null == m_trans_BaseProperty4 )
       {
            Engine.Utility.Log.Error("m_trans_BaseProperty4 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BasePropertyLevel = fastComponent.FastGetComponent<Transform>("BasePropertyLevel");
       if( null == m_trans_BasePropertyLevel )
       {
            Engine.Utility.Log.Error("m_trans_BasePropertyLevel 为空，请检查prefab是否缺乏组件");
       }
        m_grid_PromoteExpRoot = fastComponent.FastGetComponent<UIGrid>("PromoteExpRoot");
       if( null == m_grid_PromoteExpRoot )
       {
            Engine.Utility.Log.Error("m_grid_PromoteExpRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_Level = fastComponent.FastGetComponent<UILabel>("Level");
       if( null == m_label_Level )
       {
            Engine.Utility.Log.Error("m_label_Level 为空，请检查prefab是否缺乏组件");
       }
        m_label_PromoteExpPencentage = fastComponent.FastGetComponent<UILabel>("PromoteExpPencentage");
       if( null == m_label_PromoteExpPencentage )
       {
            Engine.Utility.Log.Error("m_label_PromoteExpPencentage 为空，请检查prefab是否缺乏组件");
       }
        m_slider_PromoteExpProgress = fastComponent.FastGetComponent<UISlider>("PromoteExpProgress");
       if( null == m_slider_PromoteExpProgress )
       {
            Engine.Utility.Log.Error("m_slider_PromoteExpProgress 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PromoteItemGrowRoot = fastComponent.FastGetComponent<Transform>("PromoteItemGrowRoot");
       if( null == m_trans_PromoteItemGrowRoot )
       {
            Engine.Utility.Log.Error("m_trans_PromoteItemGrowRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveRemoveStateTips1 = fastComponent.FastGetComponent<UILabel>("ActiveRemoveStateTips1");
       if( null == m_label_ActiveRemoveStateTips1 )
       {
            Engine.Utility.Log.Error("m_label_ActiveRemoveStateTips1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ActiveRemovePage = fastComponent.FastGetComponent<Transform>("ActiveRemovePage");
       if( null == m_trans_ActiveRemovePage )
       {
            Engine.Utility.Log.Error("m_trans_ActiveRemovePage 为空，请检查prefab是否缺乏组件");
       }
        m_label_Title_label = fastComponent.FastGetComponent<UILabel>("Title_label");
       if( null == m_label_Title_label )
       {
            Engine.Utility.Log.Error("m_label_Title_label 为空，请检查prefab是否缺乏组件");
       }
        m_grid_ActivePropertyRoot = fastComponent.FastGetComponent<UIGrid>("ActivePropertyRoot");
       if( null == m_grid_ActivePropertyRoot )
       {
            Engine.Utility.Log.Error("m_grid_ActivePropertyRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveRemoveStateTips2 = fastComponent.FastGetComponent<UILabel>("ActiveRemoveStateTips2");
       if( null == m_label_ActiveRemoveStateTips2 )
       {
            Engine.Utility.Log.Error("m_label_ActiveRemoveStateTips2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ActiveRemoveItemGrowRoot = fastComponent.FastGetComponent<Transform>("ActiveRemoveItemGrowRoot");
       if( null == m_trans_ActiveRemoveItemGrowRoot )
       {
            Engine.Utility.Log.Error("m_trans_ActiveRemoveItemGrowRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveRemoveName = fastComponent.FastGetComponent<UILabel>("ActiveRemoveName");
       if( null == m_label_ActiveRemoveName )
       {
            Engine.Utility.Log.Error("m_label_ActiveRemoveName 为空，请检查prefab是否缺乏组件");
       }
        m_btn_MuhonUItips_3 = fastComponent.FastGetComponent<UIButton>("MuhonUItips_3");
       if( null == m_btn_MuhonUItips_3 )
       {
            Engine.Utility.Log.Error("m_btn_MuhonUItips_3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EvolveInfos = fastComponent.FastGetComponent<Transform>("EvolveInfos");
       if( null == m_trans_EvolveInfos )
       {
            Engine.Utility.Log.Error("m_trans_EvolveInfos 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnEvolvePre = fastComponent.FastGetComponent<UIButton>("BtnEvolvePre");
       if( null == m_btn_BtnEvolvePre )
       {
            Engine.Utility.Log.Error("m_btn_BtnEvolvePre 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EvolvePreview = fastComponent.FastGetComponent<Transform>("EvolvePreview");
       if( null == m_trans_EvolvePreview )
       {
            Engine.Utility.Log.Error("m_trans_EvolvePreview 为空，请检查prefab是否缺乏组件");
       }
        m_slider_EvolveCurStarLv = fastComponent.FastGetComponent<UISlider>("EvolveCurStarLv");
       if( null == m_slider_EvolveCurStarLv )
       {
            Engine.Utility.Log.Error("m_slider_EvolveCurStarLv 为空，请检查prefab是否缺乏组件");
       }
        m_slider_EvolveNextStarLv = fastComponent.FastGetComponent<UISlider>("EvolveNextStarLv");
       if( null == m_slider_EvolveNextStarLv )
       {
            Engine.Utility.Log.Error("m_slider_EvolveNextStarLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_EvolveCurLv = fastComponent.FastGetComponent<UILabel>("EvolveCurLv");
       if( null == m_label_EvolveCurLv )
       {
            Engine.Utility.Log.Error("m_label_EvolveCurLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_EvolveNextLv = fastComponent.FastGetComponent<UILabel>("EvolveNextLv");
       if( null == m_label_EvolveNextLv )
       {
            Engine.Utility.Log.Error("m_label_EvolveNextLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_EvolveCurAttrNum = fastComponent.FastGetComponent<UILabel>("EvolveCurAttrNum");
       if( null == m_label_EvolveCurAttrNum )
       {
            Engine.Utility.Log.Error("m_label_EvolveCurAttrNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_EvolveNextAttrNum = fastComponent.FastGetComponent<UILabel>("EvolveNextAttrNum");
       if( null == m_label_EvolveNextAttrNum )
       {
            Engine.Utility.Log.Error("m_label_EvolveNextAttrNum 为空，请检查prefab是否缺乏组件");
       }
        m_widget_PreviewCollider = fastComponent.FastGetComponent<UIWidget>("PreviewCollider");
       if( null == m_widget_PreviewCollider )
       {
            Engine.Utility.Log.Error("m_widget_PreviewCollider 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EvolveGrowRoot = fastComponent.FastGetComponent<Transform>("EvolveGrowRoot");
       if( null == m_trans_EvolveGrowRoot )
       {
            Engine.Utility.Log.Error("m_trans_EvolveGrowRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_EvolveMuhonLv = fastComponent.FastGetComponent<UILabel>("EvolveMuhonLv");
       if( null == m_label_EvolveMuhonLv )
       {
            Engine.Utility.Log.Error("m_label_EvolveMuhonLv 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EvolveTargeParticle = fastComponent.FastGetComponent<Transform>("EvolveTargeParticle");
       if( null == m_trans_EvolveTargeParticle )
       {
            Engine.Utility.Log.Error("m_trans_EvolveTargeParticle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CostEvolveMuhon = fastComponent.FastGetComponent<Transform>("CostEvolveMuhon");
       if( null == m_trans_CostEvolveMuhon )
       {
            Engine.Utility.Log.Error("m_trans_CostEvolveMuhon 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CostEvolveStar = fastComponent.FastGetComponent<Transform>("CostEvolveStar");
       if( null == m_trans_CostEvolveStar )
       {
            Engine.Utility.Log.Error("m_trans_CostEvolveStar 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EvolveStarLightParticle = fastComponent.FastGetComponent<Transform>("EvolveStarLightParticle");
       if( null == m_trans_EvolveStarLightParticle )
       {
            Engine.Utility.Log.Error("m_trans_EvolveStarLightParticle 为空，请检查prefab是否缺乏组件");
       }
        m_label_EvolveEffetDepthLimitMask = fastComponent.FastGetComponent<UILabel>("EvolveEffetDepthLimitMask");
       if( null == m_label_EvolveEffetDepthLimitMask )
       {
            Engine.Utility.Log.Error("m_label_EvolveEffetDepthLimitMask 为空，请检查prefab是否缺乏组件");
       }
        m_label_EvolvePlayerLvLmit = fastComponent.FastGetComponent<UILabel>("EvolvePlayerLvLmit");
       if( null == m_label_EvolvePlayerLvLmit )
       {
            Engine.Utility.Log.Error("m_label_EvolvePlayerLvLmit 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EvolveMax = fastComponent.FastGetComponent<Transform>("EvolveMax");
       if( null == m_trans_EvolveMax )
       {
            Engine.Utility.Log.Error("m_trans_EvolveMax 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EvolveMaxGrowRoot = fastComponent.FastGetComponent<Transform>("EvolveMaxGrowRoot");
       if( null == m_trans_EvolveMaxGrowRoot )
       {
            Engine.Utility.Log.Error("m_trans_EvolveMaxGrowRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_EvolveAttrTitle = fastComponent.FastGetComponent<UISprite>("EvolveAttrTitle");
       if( null == m_sprite_EvolveAttrTitle )
       {
            Engine.Utility.Log.Error("m_sprite_EvolveAttrTitle 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_EvolveAttr1 = fastComponent.FastGetComponent<UISprite>("EvolveAttr1");
       if( null == m_sprite_EvolveAttr1 )
       {
            Engine.Utility.Log.Error("m_sprite_EvolveAttr1 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_EvolveAttr2 = fastComponent.FastGetComponent<UISprite>("EvolveAttr2");
       if( null == m_sprite_EvolveAttr2 )
       {
            Engine.Utility.Log.Error("m_sprite_EvolveAttr2 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_EvolveAttr3 = fastComponent.FastGetComponent<UISprite>("EvolveAttr3");
       if( null == m_sprite_EvolveAttr3 )
       {
            Engine.Utility.Log.Error("m_sprite_EvolveAttr3 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_EvolveAttr4 = fastComponent.FastGetComponent<UISprite>("EvolveAttr4");
       if( null == m_sprite_EvolveAttr4 )
       {
            Engine.Utility.Log.Error("m_sprite_EvolveAttr4 为空，请检查prefab是否缺乏组件");
       }
        m_btn_MuhonUItips_2 = fastComponent.FastGetComponent<UIButton>("MuhonUItips_2");
       if( null == m_btn_MuhonUItips_2 )
       {
            Engine.Utility.Log.Error("m_btn_MuhonUItips_2 为空，请检查prefab是否缺乏组件");
       }
        m_grid_CurrentAdditiveAttrContent = fastComponent.FastGetComponent<UIGrid>("CurrentAdditiveAttrContent");
       if( null == m_grid_CurrentAdditiveAttrContent )
       {
            Engine.Utility.Log.Error("m_grid_CurrentAdditiveAttrContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BlendCurGrowRoot = fastComponent.FastGetComponent<Transform>("BlendCurGrowRoot");
       if( null == m_trans_BlendCurGrowRoot )
       {
            Engine.Utility.Log.Error("m_trans_BlendCurGrowRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BlendCurShowGrid = fastComponent.FastGetComponent<Transform>("BlendCurShowGrid");
       if( null == m_trans_BlendCurShowGrid )
       {
            Engine.Utility.Log.Error("m_trans_BlendCurShowGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BlendNextGrowRoot = fastComponent.FastGetComponent<Transform>("BlendNextGrowRoot");
       if( null == m_trans_BlendNextGrowRoot )
       {
            Engine.Utility.Log.Error("m_trans_BlendNextGrowRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BlendUnload = fastComponent.FastGetComponent<UIButton>("BlendUnload");
       if( null == m_btn_BlendUnload )
       {
            Engine.Utility.Log.Error("m_btn_BlendUnload 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BlendNextShowGrid = fastComponent.FastGetComponent<Transform>("BlendNextShowGrid");
       if( null == m_trans_BlendNextShowGrid )
       {
            Engine.Utility.Log.Error("m_trans_BlendNextShowGrid 为空，请检查prefab是否缺乏组件");
       }
        m_grid_BlendAdditiveAttrContent = fastComponent.FastGetComponent<UIGrid>("BlendAdditiveAttrContent");
       if( null == m_grid_BlendAdditiveAttrContent )
       {
            Engine.Utility.Log.Error("m_grid_BlendAdditiveAttrContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BlendTips = fastComponent.FastGetComponent<Transform>("BlendTips");
       if( null == m_trans_BlendTips )
       {
            Engine.Utility.Log.Error("m_trans_BlendTips 为空，请检查prefab是否缺乏组件");
       }
        m_btn_MuhonUItips_4 = fastComponent.FastGetComponent<UIButton>("MuhonUItips_4");
       if( null == m_btn_MuhonUItips_4 )
       {
            Engine.Utility.Log.Error("m_btn_MuhonUItips_4 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CmCostBottom = fastComponent.FastGetComponent<Transform>("CmCostBottom");
       if( null == m_trans_CmCostBottom )
       {
            Engine.Utility.Log.Error("m_trans_CmCostBottom 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AssistContentRoot = fastComponent.FastGetComponent<Transform>("AssistContentRoot");
       if( null == m_trans_AssistContentRoot )
       {
            Engine.Utility.Log.Error("m_trans_AssistContentRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_AutoUseDQ = fastComponent.FastGetComponent<UIButton>("AutoUseDQ");
       if( null == m_btn_AutoUseDQ )
       {
            Engine.Utility.Log.Error("m_btn_AutoUseDQ 为空，请检查prefab是否缺乏组件");
       }
        m_trans_OPCost = fastComponent.FastGetComponent<Transform>("OPCost");
       if( null == m_trans_OPCost )
       {
            Engine.Utility.Log.Error("m_trans_OPCost 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BlendBtn = fastComponent.FastGetComponent<UIButton>("BlendBtn");
       if( null == m_btn_BlendBtn )
       {
            Engine.Utility.Log.Error("m_btn_BlendBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ActiveBtn = fastComponent.FastGetComponent<UIButton>("ActiveBtn");
       if( null == m_btn_ActiveBtn )
       {
            Engine.Utility.Log.Error("m_btn_ActiveBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RemoveBtn = fastComponent.FastGetComponent<UIButton>("RemoveBtn");
       if( null == m_btn_RemoveBtn )
       {
            Engine.Utility.Log.Error("m_btn_RemoveBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_EvolveBtn = fastComponent.FastGetComponent<UIButton>("EvolveBtn");
       if( null == m_btn_EvolveBtn )
       {
            Engine.Utility.Log.Error("m_btn_EvolveBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NullMuhonTipsContent = fastComponent.FastGetComponent<Transform>("NullMuhonTipsContent");
       if( null == m_trans_NullMuhonTipsContent )
       {
            Engine.Utility.Log.Error("m_trans_NullMuhonTipsContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SXInfos = fastComponent.FastGetComponent<Transform>("SXInfos");
       if( null == m_trans_SXInfos )
       {
            Engine.Utility.Log.Error("m_trans_SXInfos 为空，请检查prefab是否缺乏组件");
       }
        m_label_SXTips = fastComponent.FastGetComponent<UILabel>("SXTips");
       if( null == m_label_SXTips )
       {
            Engine.Utility.Log.Error("m_label_SXTips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SXGrowRoot = fastComponent.FastGetComponent<Transform>("SXGrowRoot");
       if( null == m_trans_SXGrowRoot )
       {
            Engine.Utility.Log.Error("m_trans_SXGrowRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_SXLv = fastComponent.FastGetComponent<UILabel>("SXLv");
       if( null == m_label_SXLv )
       {
            Engine.Utility.Log.Error("m_label_SXLv 为空，请检查prefab是否缺乏组件");
       }
        m_trans_StarRoot = fastComponent.FastGetComponent<Transform>("StarRoot");
       if( null == m_trans_StarRoot )
       {
            Engine.Utility.Log.Error("m_trans_StarRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DeputyMuhonRoot = fastComponent.FastGetComponent<Transform>("DeputyMuhonRoot");
       if( null == m_trans_DeputyMuhonRoot )
       {
            Engine.Utility.Log.Error("m_trans_DeputyMuhonRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SXMax = fastComponent.FastGetComponent<Transform>("SXMax");
       if( null == m_trans_SXMax )
       {
            Engine.Utility.Log.Error("m_trans_SXMax 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIWeaponSoulInfoGrid = fastComponent.FastGetComponent<Transform>("UIWeaponSoulInfoGrid");
       if( null == m_trans_UIWeaponSoulInfoGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIWeaponSoulInfoGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIMuhonExpGrid = fastComponent.FastGetComponent<Transform>("UIMuhonExpGrid");
       if( null == m_trans_UIMuhonExpGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIMuhonExpGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_MuhonUItips_3.gameObject).onClick = _onClick_MuhonUItips_3_Btn;
        UIEventListener.Get(m_btn_BtnEvolvePre.gameObject).onClick = _onClick_BtnEvolvePre_Btn;
        UIEventListener.Get(m_btn_MuhonUItips_2.gameObject).onClick = _onClick_MuhonUItips_2_Btn;
        UIEventListener.Get(m_btn_BlendUnload.gameObject).onClick = _onClick_BlendUnload_Btn;
        UIEventListener.Get(m_btn_MuhonUItips_4.gameObject).onClick = _onClick_MuhonUItips_4_Btn;
        UIEventListener.Get(m_btn_AutoUseDQ.gameObject).onClick = _onClick_AutoUseDQ_Btn;
        UIEventListener.Get(m_btn_BlendBtn.gameObject).onClick = _onClick_BlendBtn_Btn;
        UIEventListener.Get(m_btn_ActiveBtn.gameObject).onClick = _onClick_ActiveBtn_Btn;
        UIEventListener.Get(m_btn_RemoveBtn.gameObject).onClick = _onClick_RemoveBtn_Btn;
        UIEventListener.Get(m_btn_EvolveBtn.gameObject).onClick = _onClick_EvolveBtn_Btn;
    }

    void _onClick_MuhonUItips_3_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_BtnEvolvePre_Btn(GameObject caster)
    {
        onClick_BtnEvolvePre_Btn( caster );
    }

    void _onClick_MuhonUItips_2_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_BlendUnload_Btn(GameObject caster)
    {
        onClick_BlendUnload_Btn( caster );
    }

    void _onClick_MuhonUItips_4_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_AutoUseDQ_Btn(GameObject caster)
    {
        onClick_AutoUseDQ_Btn( caster );
    }

    void _onClick_BlendBtn_Btn(GameObject caster)
    {
        onClick_BlendBtn_Btn( caster );
    }

    void _onClick_ActiveBtn_Btn(GameObject caster)
    {
        onClick_ActiveBtn_Btn( caster );
    }

    void _onClick_RemoveBtn_Btn(GameObject caster)
    {
        onClick_RemoveBtn_Btn( caster );
    }

    void _onClick_EvolveBtn_Btn(GameObject caster)
    {
        onClick_EvolveBtn_Btn( caster );
    }


}
