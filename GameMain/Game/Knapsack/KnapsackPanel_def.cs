//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class KnapsackPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//背包
		BeiBao = 1,
		//仓库
		CangKu = 2,
		//合成
		HeCheng = 4,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_RightContent;

    Transform            m_trans_bookmarktoggle;

    Transform            m_trans_ItemAll;

    Transform            m_trans_ItemEquipment;

    Transform            m_trans_ItemUseable;

    Transform            m_trans_ItemProps;

    Transform            m_trans_StorageToggle;

    UIGridCreatorBase    m_ctor_ItemGridScrollView;

    UIButton             m_btn_CarryShopSellBtn;

    UIButton             m_btn_arrange;

    UIButton             m_btn_Split;

    UIButton             m_btn_repairs;

    UIButton             m_btn_BackToPkg;

    UILabel              m_label_CapacityNum;

    UILabel              m_label_LongClickTips;

    Transform            m_trans_PlayerContent;

    UILabel              m_label_viplabel;

    UILabel              m_label_namelabel;

    UILabel              m_label_lvlabel;

    UILabel              m_label_fighelabel;

    UIButton             m_btn_btn_gaiming;

    UITexture            m__CharacterRenderTexture;

    Transform            m_trans_EquipmentGridRoot;

    UILabel              m_label_ActiveGridSuitLv;

    UIButton             m_btn_BtnGridSuitNormal;

    UIButton             m_btn_BtnGridSuitActive;

    UILabel              m_label_ActiveColorSuitLv;

    UIButton             m_btn_BtnColorSuitNormal;

    UIButton             m_btn_BtnColorSuitActive;

    UILabel              m_label_ActiveStoneSuitLv;

    UIButton             m_btn_BtnStoneSuitNormal;

    UIButton             m_btn_BtnStoneSuitActive;

    Transform            m_trans_SellShopConetent;

    Transform            m_trans_SellFilter;

    Transform            m_trans_SellFilterContent;

    UIButton             m_btn_SellFillterCollider;

    UISprite             m_sprite_QualityFilterContent;

    UIButton             m_btn_QualityAll;

    UIButton             m_btn_QualityNone;

    UIButton             m_btn_Quality1;

    UIButton             m_btn_Quality2;

    UIButton             m_btn_Quality3;

    UIButton             m_btn_Quality4;

    UIButton             m_btn_Quality5;

    UISprite             m_sprite_GradeFilterContent;

    UIButton             m_btn_GradeAll;

    UIButton             m_btn_Grade2;

    UIButton             m_btn_Grade3;

    UIButton             m_btn_Grade4;

    UIButton             m_btn_Grade5;

    UIButton             m_btn_Grade6;

    UIButton             m_btn_Grade7;

    UIButton             m_btn_Grade1;

    UIButton             m_btn_GradeFilterBtn;

    UIButton             m_btn_QualityFilterBtn;

    UIGridCreatorBase    m_ctor_SellShopGridScrollView;

    UIButton             m_btn_SellShopConfirmSellBtn;

    UISprite             m_sprite_SellShopGain;

    UILabel              m_label_SellShopGainNum;

    Transform            m_trans_WareHouseContent;

    UIGridCreatorBase    m_ctor_WareHouseItemGridScrollView;

    UIButton             m_btn_WareHouseArrangeBtn;

    UIButton             m_btn_WareHouseStoreCopperBtn;

    UISprite             m_sprite_WareHouseStoreCopperIcon;

    UILabel              m_label_WareHouseStoreCopperNum;

    UILabel              m_label_WareHouseCapacityNum;

    Transform            m_trans_SplitContent;

    UIGridCreatorBase    m_ctor_SplitScrollView;

    UIButton             m_btn_BtnDoSplit;

    UILabel              m_label_SpliteSelectEquipLab;

    UIButton             m_btn_AutoSelectEquip;

    UILabel              m_label_NoneSplitEquipNotice;

    Transform            m_trans_CarryShopConetent;

    UIGridCreatorBase    m_ctor_CarryShopGridScrollView;

    UIGridCreatorBase    m_ctor_CarryShopTabScrollView;

    Transform            m_trans_CarryShopRightContent;

    Transform            m_trans_PurchaseContent;

    Transform            m_trans_MallItemInfo;

    UILabel              m_label_MallItemName;

    UILabel              m_label_MallItemDes;

    UILabel              m_label_MallItemUseLv;

    UILabel              m_label_ChooseMallItemNotice;

    Transform            m_trans_MallBaseGridRoot;

    UIButton             m_btn_PurchaseBtn;

    UILabel              m_label_PurchaseLeft;

    UIButton             m_btn_BtnAdd;

    UIButton             m_btn_BtnRemove;

    UILabel              m_label_PurchaseNum;

    UIButton             m_btn_BtnMax;

    UILabel              m_label_DiscountLeftTime;

    Transform            m_trans_PurchaseCostGrid;

    UIButton             m_btn_HandInputBtn;

    Transform            m_trans_UIMallGrid;

    Transform            m_trans_UITabGrid;

    Transform            m_trans_UISplitGetGrid;

    Transform            m_trans_UIItemGrid;


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
        m_trans_bookmarktoggle = fastComponent.FastGetComponent<Transform>("bookmarktoggle");
       if( null == m_trans_bookmarktoggle )
       {
            Engine.Utility.Log.Error("m_trans_bookmarktoggle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemAll = fastComponent.FastGetComponent<Transform>("ItemAll");
       if( null == m_trans_ItemAll )
       {
            Engine.Utility.Log.Error("m_trans_ItemAll 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemEquipment = fastComponent.FastGetComponent<Transform>("ItemEquipment");
       if( null == m_trans_ItemEquipment )
       {
            Engine.Utility.Log.Error("m_trans_ItemEquipment 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemUseable = fastComponent.FastGetComponent<Transform>("ItemUseable");
       if( null == m_trans_ItemUseable )
       {
            Engine.Utility.Log.Error("m_trans_ItemUseable 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemProps = fastComponent.FastGetComponent<Transform>("ItemProps");
       if( null == m_trans_ItemProps )
       {
            Engine.Utility.Log.Error("m_trans_ItemProps 为空，请检查prefab是否缺乏组件");
       }
        m_trans_StorageToggle = fastComponent.FastGetComponent<Transform>("StorageToggle");
       if( null == m_trans_StorageToggle )
       {
            Engine.Utility.Log.Error("m_trans_StorageToggle 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ItemGridScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ItemGridScrollView");
       if( null == m_ctor_ItemGridScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ItemGridScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CarryShopSellBtn = fastComponent.FastGetComponent<UIButton>("CarryShopSellBtn");
       if( null == m_btn_CarryShopSellBtn )
       {
            Engine.Utility.Log.Error("m_btn_CarryShopSellBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_arrange = fastComponent.FastGetComponent<UIButton>("arrange");
       if( null == m_btn_arrange )
       {
            Engine.Utility.Log.Error("m_btn_arrange 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Split = fastComponent.FastGetComponent<UIButton>("Split");
       if( null == m_btn_Split )
       {
            Engine.Utility.Log.Error("m_btn_Split 为空，请检查prefab是否缺乏组件");
       }
        m_btn_repairs = fastComponent.FastGetComponent<UIButton>("repairs");
       if( null == m_btn_repairs )
       {
            Engine.Utility.Log.Error("m_btn_repairs 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BackToPkg = fastComponent.FastGetComponent<UIButton>("BackToPkg");
       if( null == m_btn_BackToPkg )
       {
            Engine.Utility.Log.Error("m_btn_BackToPkg 为空，请检查prefab是否缺乏组件");
       }
        m_label_CapacityNum = fastComponent.FastGetComponent<UILabel>("CapacityNum");
       if( null == m_label_CapacityNum )
       {
            Engine.Utility.Log.Error("m_label_CapacityNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_LongClickTips = fastComponent.FastGetComponent<UILabel>("LongClickTips");
       if( null == m_label_LongClickTips )
       {
            Engine.Utility.Log.Error("m_label_LongClickTips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PlayerContent = fastComponent.FastGetComponent<Transform>("PlayerContent");
       if( null == m_trans_PlayerContent )
       {
            Engine.Utility.Log.Error("m_trans_PlayerContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_viplabel = fastComponent.FastGetComponent<UILabel>("viplabel");
       if( null == m_label_viplabel )
       {
            Engine.Utility.Log.Error("m_label_viplabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_namelabel = fastComponent.FastGetComponent<UILabel>("namelabel");
       if( null == m_label_namelabel )
       {
            Engine.Utility.Log.Error("m_label_namelabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_lvlabel = fastComponent.FastGetComponent<UILabel>("lvlabel");
       if( null == m_label_lvlabel )
       {
            Engine.Utility.Log.Error("m_label_lvlabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_fighelabel = fastComponent.FastGetComponent<UILabel>("fighelabel");
       if( null == m_label_fighelabel )
       {
            Engine.Utility.Log.Error("m_label_fighelabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_gaiming = fastComponent.FastGetComponent<UIButton>("btn_gaiming");
       if( null == m_btn_btn_gaiming )
       {
            Engine.Utility.Log.Error("m_btn_btn_gaiming 为空，请检查prefab是否缺乏组件");
       }
        m__CharacterRenderTexture = fastComponent.FastGetComponent<UITexture>("CharacterRenderTexture");
       if( null == m__CharacterRenderTexture )
       {
            Engine.Utility.Log.Error("m__CharacterRenderTexture 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EquipmentGridRoot = fastComponent.FastGetComponent<Transform>("EquipmentGridRoot");
       if( null == m_trans_EquipmentGridRoot )
       {
            Engine.Utility.Log.Error("m_trans_EquipmentGridRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveGridSuitLv = fastComponent.FastGetComponent<UILabel>("ActiveGridSuitLv");
       if( null == m_label_ActiveGridSuitLv )
       {
            Engine.Utility.Log.Error("m_label_ActiveGridSuitLv 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnGridSuitNormal = fastComponent.FastGetComponent<UIButton>("BtnGridSuitNormal");
       if( null == m_btn_BtnGridSuitNormal )
       {
            Engine.Utility.Log.Error("m_btn_BtnGridSuitNormal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnGridSuitActive = fastComponent.FastGetComponent<UIButton>("BtnGridSuitActive");
       if( null == m_btn_BtnGridSuitActive )
       {
            Engine.Utility.Log.Error("m_btn_BtnGridSuitActive 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveColorSuitLv = fastComponent.FastGetComponent<UILabel>("ActiveColorSuitLv");
       if( null == m_label_ActiveColorSuitLv )
       {
            Engine.Utility.Log.Error("m_label_ActiveColorSuitLv 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnColorSuitNormal = fastComponent.FastGetComponent<UIButton>("BtnColorSuitNormal");
       if( null == m_btn_BtnColorSuitNormal )
       {
            Engine.Utility.Log.Error("m_btn_BtnColorSuitNormal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnColorSuitActive = fastComponent.FastGetComponent<UIButton>("BtnColorSuitActive");
       if( null == m_btn_BtnColorSuitActive )
       {
            Engine.Utility.Log.Error("m_btn_BtnColorSuitActive 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveStoneSuitLv = fastComponent.FastGetComponent<UILabel>("ActiveStoneSuitLv");
       if( null == m_label_ActiveStoneSuitLv )
       {
            Engine.Utility.Log.Error("m_label_ActiveStoneSuitLv 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnStoneSuitNormal = fastComponent.FastGetComponent<UIButton>("BtnStoneSuitNormal");
       if( null == m_btn_BtnStoneSuitNormal )
       {
            Engine.Utility.Log.Error("m_btn_BtnStoneSuitNormal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnStoneSuitActive = fastComponent.FastGetComponent<UIButton>("BtnStoneSuitActive");
       if( null == m_btn_BtnStoneSuitActive )
       {
            Engine.Utility.Log.Error("m_btn_BtnStoneSuitActive 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellShopConetent = fastComponent.FastGetComponent<Transform>("SellShopConetent");
       if( null == m_trans_SellShopConetent )
       {
            Engine.Utility.Log.Error("m_trans_SellShopConetent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellFilter = fastComponent.FastGetComponent<Transform>("SellFilter");
       if( null == m_trans_SellFilter )
       {
            Engine.Utility.Log.Error("m_trans_SellFilter 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellFilterContent = fastComponent.FastGetComponent<Transform>("SellFilterContent");
       if( null == m_trans_SellFilterContent )
       {
            Engine.Utility.Log.Error("m_trans_SellFilterContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_SellFillterCollider = fastComponent.FastGetComponent<UIButton>("SellFillterCollider");
       if( null == m_btn_SellFillterCollider )
       {
            Engine.Utility.Log.Error("m_btn_SellFillterCollider 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_QualityFilterContent = fastComponent.FastGetComponent<UISprite>("QualityFilterContent");
       if( null == m_sprite_QualityFilterContent )
       {
            Engine.Utility.Log.Error("m_sprite_QualityFilterContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_QualityAll = fastComponent.FastGetComponent<UIButton>("QualityAll");
       if( null == m_btn_QualityAll )
       {
            Engine.Utility.Log.Error("m_btn_QualityAll 为空，请检查prefab是否缺乏组件");
       }
        m_btn_QualityNone = fastComponent.FastGetComponent<UIButton>("QualityNone");
       if( null == m_btn_QualityNone )
       {
            Engine.Utility.Log.Error("m_btn_QualityNone 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Quality1 = fastComponent.FastGetComponent<UIButton>("Quality1");
       if( null == m_btn_Quality1 )
       {
            Engine.Utility.Log.Error("m_btn_Quality1 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Quality2 = fastComponent.FastGetComponent<UIButton>("Quality2");
       if( null == m_btn_Quality2 )
       {
            Engine.Utility.Log.Error("m_btn_Quality2 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Quality3 = fastComponent.FastGetComponent<UIButton>("Quality3");
       if( null == m_btn_Quality3 )
       {
            Engine.Utility.Log.Error("m_btn_Quality3 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Quality4 = fastComponent.FastGetComponent<UIButton>("Quality4");
       if( null == m_btn_Quality4 )
       {
            Engine.Utility.Log.Error("m_btn_Quality4 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Quality5 = fastComponent.FastGetComponent<UIButton>("Quality5");
       if( null == m_btn_Quality5 )
       {
            Engine.Utility.Log.Error("m_btn_Quality5 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_GradeFilterContent = fastComponent.FastGetComponent<UISprite>("GradeFilterContent");
       if( null == m_sprite_GradeFilterContent )
       {
            Engine.Utility.Log.Error("m_sprite_GradeFilterContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_GradeAll = fastComponent.FastGetComponent<UIButton>("GradeAll");
       if( null == m_btn_GradeAll )
       {
            Engine.Utility.Log.Error("m_btn_GradeAll 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Grade2 = fastComponent.FastGetComponent<UIButton>("Grade2");
       if( null == m_btn_Grade2 )
       {
            Engine.Utility.Log.Error("m_btn_Grade2 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Grade3 = fastComponent.FastGetComponent<UIButton>("Grade3");
       if( null == m_btn_Grade3 )
       {
            Engine.Utility.Log.Error("m_btn_Grade3 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Grade4 = fastComponent.FastGetComponent<UIButton>("Grade4");
       if( null == m_btn_Grade4 )
       {
            Engine.Utility.Log.Error("m_btn_Grade4 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Grade5 = fastComponent.FastGetComponent<UIButton>("Grade5");
       if( null == m_btn_Grade5 )
       {
            Engine.Utility.Log.Error("m_btn_Grade5 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Grade6 = fastComponent.FastGetComponent<UIButton>("Grade6");
       if( null == m_btn_Grade6 )
       {
            Engine.Utility.Log.Error("m_btn_Grade6 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Grade7 = fastComponent.FastGetComponent<UIButton>("Grade7");
       if( null == m_btn_Grade7 )
       {
            Engine.Utility.Log.Error("m_btn_Grade7 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Grade1 = fastComponent.FastGetComponent<UIButton>("Grade1");
       if( null == m_btn_Grade1 )
       {
            Engine.Utility.Log.Error("m_btn_Grade1 为空，请检查prefab是否缺乏组件");
       }
        m_btn_GradeFilterBtn = fastComponent.FastGetComponent<UIButton>("GradeFilterBtn");
       if( null == m_btn_GradeFilterBtn )
       {
            Engine.Utility.Log.Error("m_btn_GradeFilterBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_QualityFilterBtn = fastComponent.FastGetComponent<UIButton>("QualityFilterBtn");
       if( null == m_btn_QualityFilterBtn )
       {
            Engine.Utility.Log.Error("m_btn_QualityFilterBtn 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_SellShopGridScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("SellShopGridScrollView");
       if( null == m_ctor_SellShopGridScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_SellShopGridScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_SellShopConfirmSellBtn = fastComponent.FastGetComponent<UIButton>("SellShopConfirmSellBtn");
       if( null == m_btn_SellShopConfirmSellBtn )
       {
            Engine.Utility.Log.Error("m_btn_SellShopConfirmSellBtn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_SellShopGain = fastComponent.FastGetComponent<UISprite>("SellShopGain");
       if( null == m_sprite_SellShopGain )
       {
            Engine.Utility.Log.Error("m_sprite_SellShopGain 为空，请检查prefab是否缺乏组件");
       }
        m_label_SellShopGainNum = fastComponent.FastGetComponent<UILabel>("SellShopGainNum");
       if( null == m_label_SellShopGainNum )
       {
            Engine.Utility.Log.Error("m_label_SellShopGainNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WareHouseContent = fastComponent.FastGetComponent<Transform>("WareHouseContent");
       if( null == m_trans_WareHouseContent )
       {
            Engine.Utility.Log.Error("m_trans_WareHouseContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_WareHouseItemGridScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("WareHouseItemGridScrollView");
       if( null == m_ctor_WareHouseItemGridScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_WareHouseItemGridScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WareHouseArrangeBtn = fastComponent.FastGetComponent<UIButton>("WareHouseArrangeBtn");
       if( null == m_btn_WareHouseArrangeBtn )
       {
            Engine.Utility.Log.Error("m_btn_WareHouseArrangeBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WareHouseStoreCopperBtn = fastComponent.FastGetComponent<UIButton>("WareHouseStoreCopperBtn");
       if( null == m_btn_WareHouseStoreCopperBtn )
       {
            Engine.Utility.Log.Error("m_btn_WareHouseStoreCopperBtn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_WareHouseStoreCopperIcon = fastComponent.FastGetComponent<UISprite>("WareHouseStoreCopperIcon");
       if( null == m_sprite_WareHouseStoreCopperIcon )
       {
            Engine.Utility.Log.Error("m_sprite_WareHouseStoreCopperIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_WareHouseStoreCopperNum = fastComponent.FastGetComponent<UILabel>("WareHouseStoreCopperNum");
       if( null == m_label_WareHouseStoreCopperNum )
       {
            Engine.Utility.Log.Error("m_label_WareHouseStoreCopperNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_WareHouseCapacityNum = fastComponent.FastGetComponent<UILabel>("WareHouseCapacityNum");
       if( null == m_label_WareHouseCapacityNum )
       {
            Engine.Utility.Log.Error("m_label_WareHouseCapacityNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SplitContent = fastComponent.FastGetComponent<Transform>("SplitContent");
       if( null == m_trans_SplitContent )
       {
            Engine.Utility.Log.Error("m_trans_SplitContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_SplitScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("SplitScrollView");
       if( null == m_ctor_SplitScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_SplitScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnDoSplit = fastComponent.FastGetComponent<UIButton>("BtnDoSplit");
       if( null == m_btn_BtnDoSplit )
       {
            Engine.Utility.Log.Error("m_btn_BtnDoSplit 为空，请检查prefab是否缺乏组件");
       }
        m_label_SpliteSelectEquipLab = fastComponent.FastGetComponent<UILabel>("SpliteSelectEquipLab");
       if( null == m_label_SpliteSelectEquipLab )
       {
            Engine.Utility.Log.Error("m_label_SpliteSelectEquipLab 为空，请检查prefab是否缺乏组件");
       }
        m_btn_AutoSelectEquip = fastComponent.FastGetComponent<UIButton>("AutoSelectEquip");
       if( null == m_btn_AutoSelectEquip )
       {
            Engine.Utility.Log.Error("m_btn_AutoSelectEquip 为空，请检查prefab是否缺乏组件");
       }
        m_label_NoneSplitEquipNotice = fastComponent.FastGetComponent<UILabel>("NoneSplitEquipNotice");
       if( null == m_label_NoneSplitEquipNotice )
       {
            Engine.Utility.Log.Error("m_label_NoneSplitEquipNotice 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CarryShopConetent = fastComponent.FastGetComponent<Transform>("CarryShopConetent");
       if( null == m_trans_CarryShopConetent )
       {
            Engine.Utility.Log.Error("m_trans_CarryShopConetent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_CarryShopGridScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("CarryShopGridScrollView");
       if( null == m_ctor_CarryShopGridScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_CarryShopGridScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_CarryShopTabScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("CarryShopTabScrollView");
       if( null == m_ctor_CarryShopTabScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_CarryShopTabScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CarryShopRightContent = fastComponent.FastGetComponent<Transform>("CarryShopRightContent");
       if( null == m_trans_CarryShopRightContent )
       {
            Engine.Utility.Log.Error("m_trans_CarryShopRightContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PurchaseContent = fastComponent.FastGetComponent<Transform>("PurchaseContent");
       if( null == m_trans_PurchaseContent )
       {
            Engine.Utility.Log.Error("m_trans_PurchaseContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MallItemInfo = fastComponent.FastGetComponent<Transform>("MallItemInfo");
       if( null == m_trans_MallItemInfo )
       {
            Engine.Utility.Log.Error("m_trans_MallItemInfo 为空，请检查prefab是否缺乏组件");
       }
        m_label_MallItemName = fastComponent.FastGetComponent<UILabel>("MallItemName");
       if( null == m_label_MallItemName )
       {
            Engine.Utility.Log.Error("m_label_MallItemName 为空，请检查prefab是否缺乏组件");
       }
        m_label_MallItemDes = fastComponent.FastGetComponent<UILabel>("MallItemDes");
       if( null == m_label_MallItemDes )
       {
            Engine.Utility.Log.Error("m_label_MallItemDes 为空，请检查prefab是否缺乏组件");
       }
        m_label_MallItemUseLv = fastComponent.FastGetComponent<UILabel>("MallItemUseLv");
       if( null == m_label_MallItemUseLv )
       {
            Engine.Utility.Log.Error("m_label_MallItemUseLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_ChooseMallItemNotice = fastComponent.FastGetComponent<UILabel>("ChooseMallItemNotice");
       if( null == m_label_ChooseMallItemNotice )
       {
            Engine.Utility.Log.Error("m_label_ChooseMallItemNotice 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MallBaseGridRoot = fastComponent.FastGetComponent<Transform>("MallBaseGridRoot");
       if( null == m_trans_MallBaseGridRoot )
       {
            Engine.Utility.Log.Error("m_trans_MallBaseGridRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PurchaseBtn = fastComponent.FastGetComponent<UIButton>("PurchaseBtn");
       if( null == m_btn_PurchaseBtn )
       {
            Engine.Utility.Log.Error("m_btn_PurchaseBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_PurchaseLeft = fastComponent.FastGetComponent<UILabel>("PurchaseLeft");
       if( null == m_label_PurchaseLeft )
       {
            Engine.Utility.Log.Error("m_label_PurchaseLeft 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnAdd = fastComponent.FastGetComponent<UIButton>("BtnAdd");
       if( null == m_btn_BtnAdd )
       {
            Engine.Utility.Log.Error("m_btn_BtnAdd 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnRemove = fastComponent.FastGetComponent<UIButton>("BtnRemove");
       if( null == m_btn_BtnRemove )
       {
            Engine.Utility.Log.Error("m_btn_BtnRemove 为空，请检查prefab是否缺乏组件");
       }
        m_label_PurchaseNum = fastComponent.FastGetComponent<UILabel>("PurchaseNum");
       if( null == m_label_PurchaseNum )
       {
            Engine.Utility.Log.Error("m_label_PurchaseNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnMax = fastComponent.FastGetComponent<UIButton>("BtnMax");
       if( null == m_btn_BtnMax )
       {
            Engine.Utility.Log.Error("m_btn_BtnMax 为空，请检查prefab是否缺乏组件");
       }
        m_label_DiscountLeftTime = fastComponent.FastGetComponent<UILabel>("DiscountLeftTime");
       if( null == m_label_DiscountLeftTime )
       {
            Engine.Utility.Log.Error("m_label_DiscountLeftTime 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PurchaseCostGrid = fastComponent.FastGetComponent<Transform>("PurchaseCostGrid");
       if( null == m_trans_PurchaseCostGrid )
       {
            Engine.Utility.Log.Error("m_trans_PurchaseCostGrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_HandInputBtn = fastComponent.FastGetComponent<UIButton>("HandInputBtn");
       if( null == m_btn_HandInputBtn )
       {
            Engine.Utility.Log.Error("m_btn_HandInputBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIMallGrid = fastComponent.FastGetComponent<Transform>("UIMallGrid");
       if( null == m_trans_UIMallGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIMallGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITabGrid = fastComponent.FastGetComponent<Transform>("UITabGrid");
       if( null == m_trans_UITabGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITabGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UISplitGetGrid = fastComponent.FastGetComponent<Transform>("UISplitGetGrid");
       if( null == m_trans_UISplitGetGrid )
       {
            Engine.Utility.Log.Error("m_trans_UISplitGetGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemGrid = fastComponent.FastGetComponent<Transform>("UIItemGrid");
       if( null == m_trans_UIItemGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_CarryShopSellBtn.gameObject).onClick = _onClick_CarryShopSellBtn_Btn;
        UIEventListener.Get(m_btn_arrange.gameObject).onClick = _onClick_Arrange_Btn;
        UIEventListener.Get(m_btn_Split.gameObject).onClick = _onClick_Split_Btn;
        UIEventListener.Get(m_btn_repairs.gameObject).onClick = _onClick_Repairs_Btn;
        UIEventListener.Get(m_btn_BackToPkg.gameObject).onClick = _onClick_BackToPkg_Btn;
        UIEventListener.Get(m_btn_btn_gaiming.gameObject).onClick = _onClick_Btn_gaiming_Btn;
        UIEventListener.Get(m_btn_BtnGridSuitNormal.gameObject).onClick = _onClick_BtnGridSuitNormal_Btn;
        UIEventListener.Get(m_btn_BtnGridSuitActive.gameObject).onClick = _onClick_BtnGridSuitActive_Btn;
        UIEventListener.Get(m_btn_BtnColorSuitNormal.gameObject).onClick = _onClick_BtnColorSuitNormal_Btn;
        UIEventListener.Get(m_btn_BtnColorSuitActive.gameObject).onClick = _onClick_BtnColorSuitActive_Btn;
        UIEventListener.Get(m_btn_BtnStoneSuitNormal.gameObject).onClick = _onClick_BtnStoneSuitNormal_Btn;
        UIEventListener.Get(m_btn_BtnStoneSuitActive.gameObject).onClick = _onClick_BtnStoneSuitActive_Btn;
        UIEventListener.Get(m_btn_SellFillterCollider.gameObject).onClick = _onClick_SellFillterCollider_Btn;
        UIEventListener.Get(m_btn_QualityAll.gameObject).onClick = _onClick_QualityAll_Btn;
        UIEventListener.Get(m_btn_QualityNone.gameObject).onClick = _onClick_QualityNone_Btn;
        UIEventListener.Get(m_btn_Quality1.gameObject).onClick = _onClick_Quality1_Btn;
        UIEventListener.Get(m_btn_Quality2.gameObject).onClick = _onClick_Quality2_Btn;
        UIEventListener.Get(m_btn_Quality3.gameObject).onClick = _onClick_Quality3_Btn;
        UIEventListener.Get(m_btn_Quality4.gameObject).onClick = _onClick_Quality4_Btn;
        UIEventListener.Get(m_btn_Quality5.gameObject).onClick = _onClick_Quality5_Btn;
        UIEventListener.Get(m_btn_GradeAll.gameObject).onClick = _onClick_GradeAll_Btn;
        UIEventListener.Get(m_btn_Grade2.gameObject).onClick = _onClick_Grade2_Btn;
        UIEventListener.Get(m_btn_Grade3.gameObject).onClick = _onClick_Grade3_Btn;
        UIEventListener.Get(m_btn_Grade4.gameObject).onClick = _onClick_Grade4_Btn;
        UIEventListener.Get(m_btn_Grade5.gameObject).onClick = _onClick_Grade5_Btn;
        UIEventListener.Get(m_btn_Grade6.gameObject).onClick = _onClick_Grade6_Btn;
        UIEventListener.Get(m_btn_Grade7.gameObject).onClick = _onClick_Grade7_Btn;
        UIEventListener.Get(m_btn_Grade1.gameObject).onClick = _onClick_Grade1_Btn;
        UIEventListener.Get(m_btn_GradeFilterBtn.gameObject).onClick = _onClick_GradeFilterBtn_Btn;
        UIEventListener.Get(m_btn_QualityFilterBtn.gameObject).onClick = _onClick_QualityFilterBtn_Btn;
        UIEventListener.Get(m_btn_SellShopConfirmSellBtn.gameObject).onClick = _onClick_SellShopConfirmSellBtn_Btn;
        UIEventListener.Get(m_btn_WareHouseArrangeBtn.gameObject).onClick = _onClick_WareHouseArrangeBtn_Btn;
        UIEventListener.Get(m_btn_WareHouseStoreCopperBtn.gameObject).onClick = _onClick_WareHouseStoreCopperBtn_Btn;
        UIEventListener.Get(m_btn_BtnDoSplit.gameObject).onClick = _onClick_BtnDoSplit_Btn;
        UIEventListener.Get(m_btn_AutoSelectEquip.gameObject).onClick = _onClick_AutoSelectEquip_Btn;
        UIEventListener.Get(m_btn_PurchaseBtn.gameObject).onClick = _onClick_PurchaseBtn_Btn;
        UIEventListener.Get(m_btn_BtnAdd.gameObject).onClick = _onClick_BtnAdd_Btn;
        UIEventListener.Get(m_btn_BtnRemove.gameObject).onClick = _onClick_BtnRemove_Btn;
        UIEventListener.Get(m_btn_BtnMax.gameObject).onClick = _onClick_BtnMax_Btn;
        UIEventListener.Get(m_btn_HandInputBtn.gameObject).onClick = _onClick_HandInputBtn_Btn;
    }

    void _onClick_CarryShopSellBtn_Btn(GameObject caster)
    {
        onClick_CarryShopSellBtn_Btn( caster );
    }

    void _onClick_Arrange_Btn(GameObject caster)
    {
        onClick_Arrange_Btn( caster );
    }

    void _onClick_Split_Btn(GameObject caster)
    {
        onClick_Split_Btn( caster );
    }

    void _onClick_Repairs_Btn(GameObject caster)
    {
        onClick_Repairs_Btn( caster );
    }

    void _onClick_BackToPkg_Btn(GameObject caster)
    {
        onClick_BackToPkg_Btn( caster );
    }

    void _onClick_Btn_gaiming_Btn(GameObject caster)
    {
        onClick_Btn_gaiming_Btn( caster );
    }

    void _onClick_BtnGridSuitNormal_Btn(GameObject caster)
    {
        onClick_BtnGridSuitNormal_Btn( caster );
    }

    void _onClick_BtnGridSuitActive_Btn(GameObject caster)
    {
        onClick_BtnGridSuitActive_Btn( caster );
    }

    void _onClick_BtnColorSuitNormal_Btn(GameObject caster)
    {
        onClick_BtnColorSuitNormal_Btn( caster );
    }

    void _onClick_BtnColorSuitActive_Btn(GameObject caster)
    {
        onClick_BtnColorSuitActive_Btn( caster );
    }

    void _onClick_BtnStoneSuitNormal_Btn(GameObject caster)
    {
        onClick_BtnStoneSuitNormal_Btn( caster );
    }

    void _onClick_BtnStoneSuitActive_Btn(GameObject caster)
    {
        onClick_BtnStoneSuitActive_Btn( caster );
    }

    void _onClick_SellFillterCollider_Btn(GameObject caster)
    {
        onClick_SellFillterCollider_Btn( caster );
    }

    void _onClick_QualityAll_Btn(GameObject caster)
    {
        onClick_QualityAll_Btn( caster );
    }

    void _onClick_QualityNone_Btn(GameObject caster)
    {
        onClick_QualityNone_Btn( caster );
    }

    void _onClick_Quality1_Btn(GameObject caster)
    {
        onClick_Quality1_Btn( caster );
    }

    void _onClick_Quality2_Btn(GameObject caster)
    {
        onClick_Quality2_Btn( caster );
    }

    void _onClick_Quality3_Btn(GameObject caster)
    {
        onClick_Quality3_Btn( caster );
    }

    void _onClick_Quality4_Btn(GameObject caster)
    {
        onClick_Quality4_Btn( caster );
    }

    void _onClick_Quality5_Btn(GameObject caster)
    {
        onClick_Quality5_Btn( caster );
    }

    void _onClick_GradeAll_Btn(GameObject caster)
    {
        onClick_GradeAll_Btn( caster );
    }

    void _onClick_Grade2_Btn(GameObject caster)
    {
        onClick_Grade2_Btn( caster );
    }

    void _onClick_Grade3_Btn(GameObject caster)
    {
        onClick_Grade3_Btn( caster );
    }

    void _onClick_Grade4_Btn(GameObject caster)
    {
        onClick_Grade4_Btn( caster );
    }

    void _onClick_Grade5_Btn(GameObject caster)
    {
        onClick_Grade5_Btn( caster );
    }

    void _onClick_Grade6_Btn(GameObject caster)
    {
        onClick_Grade6_Btn( caster );
    }

    void _onClick_Grade7_Btn(GameObject caster)
    {
        onClick_Grade7_Btn( caster );
    }

    void _onClick_Grade1_Btn(GameObject caster)
    {
        onClick_Grade1_Btn( caster );
    }

    void _onClick_GradeFilterBtn_Btn(GameObject caster)
    {
        onClick_GradeFilterBtn_Btn( caster );
    }

    void _onClick_QualityFilterBtn_Btn(GameObject caster)
    {
        onClick_QualityFilterBtn_Btn( caster );
    }

    void _onClick_SellShopConfirmSellBtn_Btn(GameObject caster)
    {
        onClick_SellShopConfirmSellBtn_Btn( caster );
    }

    void _onClick_WareHouseArrangeBtn_Btn(GameObject caster)
    {
        onClick_WareHouseArrangeBtn_Btn( caster );
    }

    void _onClick_WareHouseStoreCopperBtn_Btn(GameObject caster)
    {
        onClick_WareHouseStoreCopperBtn_Btn( caster );
    }

    void _onClick_BtnDoSplit_Btn(GameObject caster)
    {
        onClick_BtnDoSplit_Btn( caster );
    }

    void _onClick_AutoSelectEquip_Btn(GameObject caster)
    {
        onClick_AutoSelectEquip_Btn( caster );
    }

    void _onClick_PurchaseBtn_Btn(GameObject caster)
    {
        onClick_PurchaseBtn_Btn( caster );
    }

    void _onClick_BtnAdd_Btn(GameObject caster)
    {
        onClick_BtnAdd_Btn( caster );
    }

    void _onClick_BtnRemove_Btn(GameObject caster)
    {
        onClick_BtnRemove_Btn( caster );
    }

    void _onClick_BtnMax_Btn(GameObject caster)
    {
        onClick_BtnMax_Btn( caster );
    }

    void _onClick_HandInputBtn_Btn(GameObject caster)
    {
        onClick_HandInputBtn_Btn( caster );
    }


}
