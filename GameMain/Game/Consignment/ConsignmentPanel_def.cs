//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ConsignmentPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//物品
		WuPin = 1,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_CurrencyContent;

    Transform            m_trans_up;

    Transform            m_trans_CommonContent;

    Transform            m_trans_UIcurrencyListGrid;

    UIButton             m_btn_TypeGrid_sell;

    UISprite             m_sprite_sellspr;

    Transform            m_trans_SellContainer;

    Transform            m_trans_SellListGrid_0;

    UIButton             m_btn_TypeGrid_buy;

    Transform            m_trans_BuyListGrid_0;

    UISprite             m_sprite_buyspr;

    Transform            m_trans_BuyContainer;

    UISprite             m_sprite_Num_Input;

    UIButton             m_btn_Num_less;

    UIButton             m_btn_Num_add;

    UISprite             m_sprite_TotalPrice_Input;

    UIButton             m_btn_TotalPrice_less;

    UIButton             m_btn_TotalPrice_add;

    UILabel              m_label_UnitPriceNum;

    UILabel              m_label_FormalitiesPriceNum;

    UIButton             m_btn_btn_Order;

    Transform            m_trans_BuyMoneyContent;

    Transform            m_trans_SellMoneyContent;

    Transform            m_trans_AccountContent;

    Transform            m_trans_LeftUITransactionListGrid;

    Transform            m_trans_MySaleList;

    Transform            m_trans_UITransactionListGrid;

    Transform            m_trans_LastSaleList;

    UILabel              m_label_OwnGoldNum;

    UILabel              m_label_OwnPennyNum;

    UIButton             m_btn_btn_AllTakeOut;

    UIButton             m_btn_btn_TransactionRecord;

    Transform            m_trans_ItemContent;

    Transform            m_trans_BuyContent;

    UIButton             m_btn_JobPopupList;

    UILabel              m_label_JobLabel;

    UIButton             m_btn_JobFillterCollider;

    UISprite             m_sprite_JobContent;

    UIButton             m_btn_GradePopupList;

    UILabel              m_label_GradeLabel;

    UISprite             m_sprite_GradeContent;

    UIButton             m_btn_GradeFillterCollider;

    UIButton             m_btn_ColourPopupList;

    UIButton             m_btn_BtnSearch;

    UIInput              m_input_SearchInput;

    UIScrollView         m_scrollview_TypeScrollView;

    UIGridCreatorBase    m_ctor_ItemListGrid;

    UIButton             m_btn_ItemLevel;

    UIButton             m_btn_ItemUnitPrice;

    UIButton             m_btn_Btn_PreviousPage;

    UIButton             m_btn_Btn_NextPage;

    UILabel              m_label_PageNum;

    UIButton             m_btn_BuyItemBtn;

    UILabel              m_label_NoBuyTime;

    Transform            m_trans_SellContent;

    UIButton             m_btn_Btn_Change;

    UILabel              m_label_Label_Change;

    Transform            m_trans_SellListContent;

    UILabel              m_label_SaleTitleName;

    UIGridCreatorBase    m_ctor_SellListScrollViewContent;

    UILabel              m_label_MyConsignYuanBao;

    UILabel              m_label_MyConsignJinBi;

    UIButton             m_btn_Btn_TakeOut;

    UIButton             m_btn_Btn_SalesRecords;

    Transform            m_trans_SellingContent;

    Transform            m_trans_SellingItem;

    UILabel              m_label_SellingItemHint;

    UILabel              m_label_SellingItemName;

    UILabel              m_label_SellingItemLevel;

    UILabel              m_label_UnitSellPrice;

    UIButton             m_btn_Btn_Less;

    UIButton             m_btn_Btn_Add;

    UILabel              m_label_UnitSellNum;

    UIButton             m_btn_Btn_Max;

    UILabel              m_label_TotalPriceNum;

    UIButton             m_btn_Btn_Sell;

    UILabel              m_label_FactorageNum;

    UILabel              m_label_GuidancePrices;

    Transform            m_trans_TreasureContent;

    Transform            m_trans_CommonItemContent;

    UIButton             m_btn_CommonBtn_Less;

    UIButton             m_btn_CommonBtn_Add;

    UILabel              m_label_RecommondPrice;

    UILabel              m_label_FaxNum;

    UIGridCreatorBase    m_ctor_ItemGridScrollView;

    UIGridCreatorBase    m_ctor_PetGridScrollView;

    UIGridCreatorBase    m_ctor_SellItemPriceList;

    UIButton             m_btn_ListCloseBtn;

    Transform            m_trans_bookmarktoggle;

    UIButton             m_btn_ItemAll;

    UIButton             m_btn_ItemEquipment;

    UIButton             m_btn_ItemUseable;

    UIButton             m_btn_ItemProps;

    UIGridCreatorBase    m_ctor_Toggles;

    Transform            m_trans_UITabGrid;

    Transform            m_trans_UIItemGrid;

    Transform            m_trans_UICtrTypeGrid;

    UIWidget             m_widget_UISecondTypeGrid;

    Transform            m_trans_UIConsignmentItemListGrid;

    Transform            m_trans_UIConsignPetListGrid;

    UISprite             m_sprite_Icon;

    UISprite             m_sprite_UISellItemPriceGrid;

    Transform            m_trans_UIConsignmentSellListGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_CurrencyContent = fastComponent.FastGetComponent<Transform>("CurrencyContent");
       if( null == m_trans_CurrencyContent )
       {
            Engine.Utility.Log.Error("m_trans_CurrencyContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_up = fastComponent.FastGetComponent<Transform>("up");
       if( null == m_trans_up )
       {
            Engine.Utility.Log.Error("m_trans_up 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CommonContent = fastComponent.FastGetComponent<Transform>("CommonContent");
       if( null == m_trans_CommonContent )
       {
            Engine.Utility.Log.Error("m_trans_CommonContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIcurrencyListGrid = fastComponent.FastGetComponent<Transform>("UIcurrencyListGrid");
       if( null == m_trans_UIcurrencyListGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIcurrencyListGrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TypeGrid_sell = fastComponent.FastGetComponent<UIButton>("TypeGrid_sell");
       if( null == m_btn_TypeGrid_sell )
       {
            Engine.Utility.Log.Error("m_btn_TypeGrid_sell 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_sellspr = fastComponent.FastGetComponent<UISprite>("sellspr");
       if( null == m_sprite_sellspr )
       {
            Engine.Utility.Log.Error("m_sprite_sellspr 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellContainer = fastComponent.FastGetComponent<Transform>("SellContainer");
       if( null == m_trans_SellContainer )
       {
            Engine.Utility.Log.Error("m_trans_SellContainer 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellListGrid_0 = fastComponent.FastGetComponent<Transform>("SellListGrid_0");
       if( null == m_trans_SellListGrid_0 )
       {
            Engine.Utility.Log.Error("m_trans_SellListGrid_0 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TypeGrid_buy = fastComponent.FastGetComponent<UIButton>("TypeGrid_buy");
       if( null == m_btn_TypeGrid_buy )
       {
            Engine.Utility.Log.Error("m_btn_TypeGrid_buy 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BuyListGrid_0 = fastComponent.FastGetComponent<Transform>("BuyListGrid_0");
       if( null == m_trans_BuyListGrid_0 )
       {
            Engine.Utility.Log.Error("m_trans_BuyListGrid_0 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_buyspr = fastComponent.FastGetComponent<UISprite>("buyspr");
       if( null == m_sprite_buyspr )
       {
            Engine.Utility.Log.Error("m_sprite_buyspr 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BuyContainer = fastComponent.FastGetComponent<Transform>("BuyContainer");
       if( null == m_trans_BuyContainer )
       {
            Engine.Utility.Log.Error("m_trans_BuyContainer 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Num_Input = fastComponent.FastGetComponent<UISprite>("Num_Input");
       if( null == m_sprite_Num_Input )
       {
            Engine.Utility.Log.Error("m_sprite_Num_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Num_less = fastComponent.FastGetComponent<UIButton>("Num_less");
       if( null == m_btn_Num_less )
       {
            Engine.Utility.Log.Error("m_btn_Num_less 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Num_add = fastComponent.FastGetComponent<UIButton>("Num_add");
       if( null == m_btn_Num_add )
       {
            Engine.Utility.Log.Error("m_btn_Num_add 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_TotalPrice_Input = fastComponent.FastGetComponent<UISprite>("TotalPrice_Input");
       if( null == m_sprite_TotalPrice_Input )
       {
            Engine.Utility.Log.Error("m_sprite_TotalPrice_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TotalPrice_less = fastComponent.FastGetComponent<UIButton>("TotalPrice_less");
       if( null == m_btn_TotalPrice_less )
       {
            Engine.Utility.Log.Error("m_btn_TotalPrice_less 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TotalPrice_add = fastComponent.FastGetComponent<UIButton>("TotalPrice_add");
       if( null == m_btn_TotalPrice_add )
       {
            Engine.Utility.Log.Error("m_btn_TotalPrice_add 为空，请检查prefab是否缺乏组件");
       }
        m_label_UnitPriceNum = fastComponent.FastGetComponent<UILabel>("UnitPriceNum");
       if( null == m_label_UnitPriceNum )
       {
            Engine.Utility.Log.Error("m_label_UnitPriceNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_FormalitiesPriceNum = fastComponent.FastGetComponent<UILabel>("FormalitiesPriceNum");
       if( null == m_label_FormalitiesPriceNum )
       {
            Engine.Utility.Log.Error("m_label_FormalitiesPriceNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Order = fastComponent.FastGetComponent<UIButton>("btn_Order");
       if( null == m_btn_btn_Order )
       {
            Engine.Utility.Log.Error("m_btn_btn_Order 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BuyMoneyContent = fastComponent.FastGetComponent<Transform>("BuyMoneyContent");
       if( null == m_trans_BuyMoneyContent )
       {
            Engine.Utility.Log.Error("m_trans_BuyMoneyContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellMoneyContent = fastComponent.FastGetComponent<Transform>("SellMoneyContent");
       if( null == m_trans_SellMoneyContent )
       {
            Engine.Utility.Log.Error("m_trans_SellMoneyContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AccountContent = fastComponent.FastGetComponent<Transform>("AccountContent");
       if( null == m_trans_AccountContent )
       {
            Engine.Utility.Log.Error("m_trans_AccountContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LeftUITransactionListGrid = fastComponent.FastGetComponent<Transform>("LeftUITransactionListGrid");
       if( null == m_trans_LeftUITransactionListGrid )
       {
            Engine.Utility.Log.Error("m_trans_LeftUITransactionListGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MySaleList = fastComponent.FastGetComponent<Transform>("MySaleList");
       if( null == m_trans_MySaleList )
       {
            Engine.Utility.Log.Error("m_trans_MySaleList 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITransactionListGrid = fastComponent.FastGetComponent<Transform>("UITransactionListGrid");
       if( null == m_trans_UITransactionListGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITransactionListGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LastSaleList = fastComponent.FastGetComponent<Transform>("LastSaleList");
       if( null == m_trans_LastSaleList )
       {
            Engine.Utility.Log.Error("m_trans_LastSaleList 为空，请检查prefab是否缺乏组件");
       }
        m_label_OwnGoldNum = fastComponent.FastGetComponent<UILabel>("OwnGoldNum");
       if( null == m_label_OwnGoldNum )
       {
            Engine.Utility.Log.Error("m_label_OwnGoldNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_OwnPennyNum = fastComponent.FastGetComponent<UILabel>("OwnPennyNum");
       if( null == m_label_OwnPennyNum )
       {
            Engine.Utility.Log.Error("m_label_OwnPennyNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_AllTakeOut = fastComponent.FastGetComponent<UIButton>("btn_AllTakeOut");
       if( null == m_btn_btn_AllTakeOut )
       {
            Engine.Utility.Log.Error("m_btn_btn_AllTakeOut 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_TransactionRecord = fastComponent.FastGetComponent<UIButton>("btn_TransactionRecord");
       if( null == m_btn_btn_TransactionRecord )
       {
            Engine.Utility.Log.Error("m_btn_btn_TransactionRecord 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemContent = fastComponent.FastGetComponent<Transform>("ItemContent");
       if( null == m_trans_ItemContent )
       {
            Engine.Utility.Log.Error("m_trans_ItemContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BuyContent = fastComponent.FastGetComponent<Transform>("BuyContent");
       if( null == m_trans_BuyContent )
       {
            Engine.Utility.Log.Error("m_trans_BuyContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_JobPopupList = fastComponent.FastGetComponent<UIButton>("JobPopupList");
       if( null == m_btn_JobPopupList )
       {
            Engine.Utility.Log.Error("m_btn_JobPopupList 为空，请检查prefab是否缺乏组件");
       }
        m_label_JobLabel = fastComponent.FastGetComponent<UILabel>("JobLabel");
       if( null == m_label_JobLabel )
       {
            Engine.Utility.Log.Error("m_label_JobLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_JobFillterCollider = fastComponent.FastGetComponent<UIButton>("JobFillterCollider");
       if( null == m_btn_JobFillterCollider )
       {
            Engine.Utility.Log.Error("m_btn_JobFillterCollider 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_JobContent = fastComponent.FastGetComponent<UISprite>("JobContent");
       if( null == m_sprite_JobContent )
       {
            Engine.Utility.Log.Error("m_sprite_JobContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_GradePopupList = fastComponent.FastGetComponent<UIButton>("GradePopupList");
       if( null == m_btn_GradePopupList )
       {
            Engine.Utility.Log.Error("m_btn_GradePopupList 为空，请检查prefab是否缺乏组件");
       }
        m_label_GradeLabel = fastComponent.FastGetComponent<UILabel>("GradeLabel");
       if( null == m_label_GradeLabel )
       {
            Engine.Utility.Log.Error("m_label_GradeLabel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_GradeContent = fastComponent.FastGetComponent<UISprite>("GradeContent");
       if( null == m_sprite_GradeContent )
       {
            Engine.Utility.Log.Error("m_sprite_GradeContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_GradeFillterCollider = fastComponent.FastGetComponent<UIButton>("GradeFillterCollider");
       if( null == m_btn_GradeFillterCollider )
       {
            Engine.Utility.Log.Error("m_btn_GradeFillterCollider 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ColourPopupList = fastComponent.FastGetComponent<UIButton>("ColourPopupList");
       if( null == m_btn_ColourPopupList )
       {
            Engine.Utility.Log.Error("m_btn_ColourPopupList 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSearch = fastComponent.FastGetComponent<UIButton>("BtnSearch");
       if( null == m_btn_BtnSearch )
       {
            Engine.Utility.Log.Error("m_btn_BtnSearch 为空，请检查prefab是否缺乏组件");
       }
        m_input_SearchInput = fastComponent.FastGetComponent<UIInput>("SearchInput");
       if( null == m_input_SearchInput )
       {
            Engine.Utility.Log.Error("m_input_SearchInput 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_TypeScrollView = fastComponent.FastGetComponent<UIScrollView>("TypeScrollView");
       if( null == m_scrollview_TypeScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_TypeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ItemListGrid = fastComponent.FastGetComponent<UIGridCreatorBase>("ItemListGrid");
       if( null == m_ctor_ItemListGrid )
       {
            Engine.Utility.Log.Error("m_ctor_ItemListGrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ItemLevel = fastComponent.FastGetComponent<UIButton>("ItemLevel");
       if( null == m_btn_ItemLevel )
       {
            Engine.Utility.Log.Error("m_btn_ItemLevel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ItemUnitPrice = fastComponent.FastGetComponent<UIButton>("ItemUnitPrice");
       if( null == m_btn_ItemUnitPrice )
       {
            Engine.Utility.Log.Error("m_btn_ItemUnitPrice 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_PreviousPage = fastComponent.FastGetComponent<UIButton>("Btn_PreviousPage");
       if( null == m_btn_Btn_PreviousPage )
       {
            Engine.Utility.Log.Error("m_btn_Btn_PreviousPage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_NextPage = fastComponent.FastGetComponent<UIButton>("Btn_NextPage");
       if( null == m_btn_Btn_NextPage )
       {
            Engine.Utility.Log.Error("m_btn_Btn_NextPage 为空，请检查prefab是否缺乏组件");
       }
        m_label_PageNum = fastComponent.FastGetComponent<UILabel>("PageNum");
       if( null == m_label_PageNum )
       {
            Engine.Utility.Log.Error("m_label_PageNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BuyItemBtn = fastComponent.FastGetComponent<UIButton>("BuyItemBtn");
       if( null == m_btn_BuyItemBtn )
       {
            Engine.Utility.Log.Error("m_btn_BuyItemBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_NoBuyTime = fastComponent.FastGetComponent<UILabel>("NoBuyTime");
       if( null == m_label_NoBuyTime )
       {
            Engine.Utility.Log.Error("m_label_NoBuyTime 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellContent = fastComponent.FastGetComponent<Transform>("SellContent");
       if( null == m_trans_SellContent )
       {
            Engine.Utility.Log.Error("m_trans_SellContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Change = fastComponent.FastGetComponent<UIButton>("Btn_Change");
       if( null == m_btn_Btn_Change )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Change 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label_Change = fastComponent.FastGetComponent<UILabel>("Label_Change");
       if( null == m_label_Label_Change )
       {
            Engine.Utility.Log.Error("m_label_Label_Change 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellListContent = fastComponent.FastGetComponent<Transform>("SellListContent");
       if( null == m_trans_SellListContent )
       {
            Engine.Utility.Log.Error("m_trans_SellListContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_SaleTitleName = fastComponent.FastGetComponent<UILabel>("SaleTitleName");
       if( null == m_label_SaleTitleName )
       {
            Engine.Utility.Log.Error("m_label_SaleTitleName 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_SellListScrollViewContent = fastComponent.FastGetComponent<UIGridCreatorBase>("SellListScrollViewContent");
       if( null == m_ctor_SellListScrollViewContent )
       {
            Engine.Utility.Log.Error("m_ctor_SellListScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyConsignYuanBao = fastComponent.FastGetComponent<UILabel>("MyConsignYuanBao");
       if( null == m_label_MyConsignYuanBao )
       {
            Engine.Utility.Log.Error("m_label_MyConsignYuanBao 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyConsignJinBi = fastComponent.FastGetComponent<UILabel>("MyConsignJinBi");
       if( null == m_label_MyConsignJinBi )
       {
            Engine.Utility.Log.Error("m_label_MyConsignJinBi 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_TakeOut = fastComponent.FastGetComponent<UIButton>("Btn_TakeOut");
       if( null == m_btn_Btn_TakeOut )
       {
            Engine.Utility.Log.Error("m_btn_Btn_TakeOut 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_SalesRecords = fastComponent.FastGetComponent<UIButton>("Btn_SalesRecords");
       if( null == m_btn_Btn_SalesRecords )
       {
            Engine.Utility.Log.Error("m_btn_Btn_SalesRecords 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellingContent = fastComponent.FastGetComponent<Transform>("SellingContent");
       if( null == m_trans_SellingContent )
       {
            Engine.Utility.Log.Error("m_trans_SellingContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellingItem = fastComponent.FastGetComponent<Transform>("SellingItem");
       if( null == m_trans_SellingItem )
       {
            Engine.Utility.Log.Error("m_trans_SellingItem 为空，请检查prefab是否缺乏组件");
       }
        m_label_SellingItemHint = fastComponent.FastGetComponent<UILabel>("SellingItemHint");
       if( null == m_label_SellingItemHint )
       {
            Engine.Utility.Log.Error("m_label_SellingItemHint 为空，请检查prefab是否缺乏组件");
       }
        m_label_SellingItemName = fastComponent.FastGetComponent<UILabel>("SellingItemName");
       if( null == m_label_SellingItemName )
       {
            Engine.Utility.Log.Error("m_label_SellingItemName 为空，请检查prefab是否缺乏组件");
       }
        m_label_SellingItemLevel = fastComponent.FastGetComponent<UILabel>("SellingItemLevel");
       if( null == m_label_SellingItemLevel )
       {
            Engine.Utility.Log.Error("m_label_SellingItemLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_UnitSellPrice = fastComponent.FastGetComponent<UILabel>("UnitSellPrice");
       if( null == m_label_UnitSellPrice )
       {
            Engine.Utility.Log.Error("m_label_UnitSellPrice 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Less = fastComponent.FastGetComponent<UIButton>("Btn_Less");
       if( null == m_btn_Btn_Less )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Less 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Add = fastComponent.FastGetComponent<UIButton>("Btn_Add");
       if( null == m_btn_Btn_Add )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Add 为空，请检查prefab是否缺乏组件");
       }
        m_label_UnitSellNum = fastComponent.FastGetComponent<UILabel>("UnitSellNum");
       if( null == m_label_UnitSellNum )
       {
            Engine.Utility.Log.Error("m_label_UnitSellNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Max = fastComponent.FastGetComponent<UIButton>("Btn_Max");
       if( null == m_btn_Btn_Max )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Max 为空，请检查prefab是否缺乏组件");
       }
        m_label_TotalPriceNum = fastComponent.FastGetComponent<UILabel>("TotalPriceNum");
       if( null == m_label_TotalPriceNum )
       {
            Engine.Utility.Log.Error("m_label_TotalPriceNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Sell = fastComponent.FastGetComponent<UIButton>("Btn_Sell");
       if( null == m_btn_Btn_Sell )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Sell 为空，请检查prefab是否缺乏组件");
       }
        m_label_FactorageNum = fastComponent.FastGetComponent<UILabel>("FactorageNum");
       if( null == m_label_FactorageNum )
       {
            Engine.Utility.Log.Error("m_label_FactorageNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_GuidancePrices = fastComponent.FastGetComponent<UILabel>("GuidancePrices");
       if( null == m_label_GuidancePrices )
       {
            Engine.Utility.Log.Error("m_label_GuidancePrices 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TreasureContent = fastComponent.FastGetComponent<Transform>("TreasureContent");
       if( null == m_trans_TreasureContent )
       {
            Engine.Utility.Log.Error("m_trans_TreasureContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CommonItemContent = fastComponent.FastGetComponent<Transform>("CommonItemContent");
       if( null == m_trans_CommonItemContent )
       {
            Engine.Utility.Log.Error("m_trans_CommonItemContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CommonBtn_Less = fastComponent.FastGetComponent<UIButton>("CommonBtn_Less");
       if( null == m_btn_CommonBtn_Less )
       {
            Engine.Utility.Log.Error("m_btn_CommonBtn_Less 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CommonBtn_Add = fastComponent.FastGetComponent<UIButton>("CommonBtn_Add");
       if( null == m_btn_CommonBtn_Add )
       {
            Engine.Utility.Log.Error("m_btn_CommonBtn_Add 为空，请检查prefab是否缺乏组件");
       }
        m_label_RecommondPrice = fastComponent.FastGetComponent<UILabel>("RecommondPrice");
       if( null == m_label_RecommondPrice )
       {
            Engine.Utility.Log.Error("m_label_RecommondPrice 为空，请检查prefab是否缺乏组件");
       }
        m_label_FaxNum = fastComponent.FastGetComponent<UILabel>("FaxNum");
       if( null == m_label_FaxNum )
       {
            Engine.Utility.Log.Error("m_label_FaxNum 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ItemGridScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ItemGridScrollView");
       if( null == m_ctor_ItemGridScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ItemGridScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_PetGridScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("PetGridScrollView");
       if( null == m_ctor_PetGridScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_PetGridScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_SellItemPriceList = fastComponent.FastGetComponent<UIGridCreatorBase>("SellItemPriceList");
       if( null == m_ctor_SellItemPriceList )
       {
            Engine.Utility.Log.Error("m_ctor_SellItemPriceList 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ListCloseBtn = fastComponent.FastGetComponent<UIButton>("ListCloseBtn");
       if( null == m_btn_ListCloseBtn )
       {
            Engine.Utility.Log.Error("m_btn_ListCloseBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_bookmarktoggle = fastComponent.FastGetComponent<Transform>("bookmarktoggle");
       if( null == m_trans_bookmarktoggle )
       {
            Engine.Utility.Log.Error("m_trans_bookmarktoggle 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ItemAll = fastComponent.FastGetComponent<UIButton>("ItemAll");
       if( null == m_btn_ItemAll )
       {
            Engine.Utility.Log.Error("m_btn_ItemAll 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ItemEquipment = fastComponent.FastGetComponent<UIButton>("ItemEquipment");
       if( null == m_btn_ItemEquipment )
       {
            Engine.Utility.Log.Error("m_btn_ItemEquipment 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ItemUseable = fastComponent.FastGetComponent<UIButton>("ItemUseable");
       if( null == m_btn_ItemUseable )
       {
            Engine.Utility.Log.Error("m_btn_ItemUseable 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ItemProps = fastComponent.FastGetComponent<UIButton>("ItemProps");
       if( null == m_btn_ItemProps )
       {
            Engine.Utility.Log.Error("m_btn_ItemProps 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Toggles = fastComponent.FastGetComponent<UIGridCreatorBase>("Toggles");
       if( null == m_ctor_Toggles )
       {
            Engine.Utility.Log.Error("m_ctor_Toggles 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITabGrid = fastComponent.FastGetComponent<Transform>("UITabGrid");
       if( null == m_trans_UITabGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITabGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemGrid = fastComponent.FastGetComponent<Transform>("UIItemGrid");
       if( null == m_trans_UIItemGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemGrid 为空，请检查prefab是否缺乏组件");
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
        m_trans_UIConsignmentItemListGrid = fastComponent.FastGetComponent<Transform>("UIConsignmentItemListGrid");
       if( null == m_trans_UIConsignmentItemListGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIConsignmentItemListGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIConsignPetListGrid = fastComponent.FastGetComponent<Transform>("UIConsignPetListGrid");
       if( null == m_trans_UIConsignPetListGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIConsignPetListGrid 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Icon = fastComponent.FastGetComponent<UISprite>("Icon");
       if( null == m_sprite_Icon )
       {
            Engine.Utility.Log.Error("m_sprite_Icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UISellItemPriceGrid = fastComponent.FastGetComponent<UISprite>("UISellItemPriceGrid");
       if( null == m_sprite_UISellItemPriceGrid )
       {
            Engine.Utility.Log.Error("m_sprite_UISellItemPriceGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIConsignmentSellListGrid = fastComponent.FastGetComponent<Transform>("UIConsignmentSellListGrid");
       if( null == m_trans_UIConsignmentSellListGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIConsignmentSellListGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_TypeGrid_sell.gameObject).onClick = _onClick_TypeGrid_sell_Btn;
        UIEventListener.Get(m_btn_TypeGrid_buy.gameObject).onClick = _onClick_TypeGrid_buy_Btn;
        UIEventListener.Get(m_btn_Num_less.gameObject).onClick = _onClick_Num_less_Btn;
        UIEventListener.Get(m_btn_Num_add.gameObject).onClick = _onClick_Num_add_Btn;
        UIEventListener.Get(m_btn_TotalPrice_less.gameObject).onClick = _onClick_TotalPrice_less_Btn;
        UIEventListener.Get(m_btn_TotalPrice_add.gameObject).onClick = _onClick_TotalPrice_add_Btn;
        UIEventListener.Get(m_btn_btn_Order.gameObject).onClick = _onClick_Btn_Order_Btn;
        UIEventListener.Get(m_btn_btn_AllTakeOut.gameObject).onClick = _onClick_Btn_AllTakeOut_Btn;
        UIEventListener.Get(m_btn_btn_TransactionRecord.gameObject).onClick = _onClick_Btn_TransactionRecord_Btn;
        UIEventListener.Get(m_btn_JobPopupList.gameObject).onClick = _onClick_JobPopupList_Btn;
        UIEventListener.Get(m_btn_JobFillterCollider.gameObject).onClick = _onClick_JobFillterCollider_Btn;
        UIEventListener.Get(m_btn_GradePopupList.gameObject).onClick = _onClick_GradePopupList_Btn;
        UIEventListener.Get(m_btn_GradeFillterCollider.gameObject).onClick = _onClick_GradeFillterCollider_Btn;
        UIEventListener.Get(m_btn_ColourPopupList.gameObject).onClick = _onClick_ColourPopupList_Btn;
        UIEventListener.Get(m_btn_BtnSearch.gameObject).onClick = _onClick_BtnSearch_Btn;
        UIEventListener.Get(m_btn_ItemLevel.gameObject).onClick = _onClick_ItemLevel_Btn;
        UIEventListener.Get(m_btn_ItemUnitPrice.gameObject).onClick = _onClick_ItemUnitPrice_Btn;
        UIEventListener.Get(m_btn_Btn_PreviousPage.gameObject).onClick = _onClick_Btn_PreviousPage_Btn;
        UIEventListener.Get(m_btn_Btn_NextPage.gameObject).onClick = _onClick_Btn_NextPage_Btn;
        UIEventListener.Get(m_btn_BuyItemBtn.gameObject).onClick = _onClick_BuyItemBtn_Btn;
        UIEventListener.Get(m_btn_Btn_Change.gameObject).onClick = _onClick_Btn_Change_Btn;
        UIEventListener.Get(m_btn_Btn_TakeOut.gameObject).onClick = _onClick_Btn_TakeOut_Btn;
        UIEventListener.Get(m_btn_Btn_SalesRecords.gameObject).onClick = _onClick_Btn_SalesRecords_Btn;
        UIEventListener.Get(m_btn_Btn_Less.gameObject).onClick = _onClick_Btn_Less_Btn;
        UIEventListener.Get(m_btn_Btn_Add.gameObject).onClick = _onClick_Btn_Add_Btn;
        UIEventListener.Get(m_btn_Btn_Max.gameObject).onClick = _onClick_Btn_Max_Btn;
        UIEventListener.Get(m_btn_Btn_Sell.gameObject).onClick = _onClick_Btn_Sell_Btn;
        UIEventListener.Get(m_btn_CommonBtn_Less.gameObject).onClick = _onClick_CommonBtn_Less_Btn;
        UIEventListener.Get(m_btn_CommonBtn_Add.gameObject).onClick = _onClick_CommonBtn_Add_Btn;
        UIEventListener.Get(m_btn_ListCloseBtn.gameObject).onClick = _onClick_ListCloseBtn_Btn;
        UIEventListener.Get(m_btn_ItemAll.gameObject).onClick = _onClick_ItemAll_Btn;
        UIEventListener.Get(m_btn_ItemEquipment.gameObject).onClick = _onClick_ItemEquipment_Btn;
        UIEventListener.Get(m_btn_ItemUseable.gameObject).onClick = _onClick_ItemUseable_Btn;
        UIEventListener.Get(m_btn_ItemProps.gameObject).onClick = _onClick_ItemProps_Btn;
    }

    void _onClick_TypeGrid_sell_Btn(GameObject caster)
    {
        onClick_TypeGrid_sell_Btn( caster );
    }

    void _onClick_TypeGrid_buy_Btn(GameObject caster)
    {
        onClick_TypeGrid_buy_Btn( caster );
    }

    void _onClick_Num_less_Btn(GameObject caster)
    {
        onClick_Num_less_Btn( caster );
    }

    void _onClick_Num_add_Btn(GameObject caster)
    {
        onClick_Num_add_Btn( caster );
    }

    void _onClick_TotalPrice_less_Btn(GameObject caster)
    {
        onClick_TotalPrice_less_Btn( caster );
    }

    void _onClick_TotalPrice_add_Btn(GameObject caster)
    {
        onClick_TotalPrice_add_Btn( caster );
    }

    void _onClick_Btn_Order_Btn(GameObject caster)
    {
        onClick_Btn_Order_Btn( caster );
    }

    void _onClick_Btn_AllTakeOut_Btn(GameObject caster)
    {
        onClick_Btn_AllTakeOut_Btn( caster );
    }

    void _onClick_Btn_TransactionRecord_Btn(GameObject caster)
    {
        onClick_Btn_TransactionRecord_Btn( caster );
    }

    void _onClick_JobPopupList_Btn(GameObject caster)
    {
        onClick_JobPopupList_Btn( caster );
    }

    void _onClick_JobFillterCollider_Btn(GameObject caster)
    {
        onClick_JobFillterCollider_Btn( caster );
    }

    void _onClick_GradePopupList_Btn(GameObject caster)
    {
        onClick_GradePopupList_Btn( caster );
    }

    void _onClick_GradeFillterCollider_Btn(GameObject caster)
    {
        onClick_GradeFillterCollider_Btn( caster );
    }

    void _onClick_ColourPopupList_Btn(GameObject caster)
    {
        onClick_ColourPopupList_Btn( caster );
    }

    void _onClick_BtnSearch_Btn(GameObject caster)
    {
        onClick_BtnSearch_Btn( caster );
    }

    void _onClick_ItemLevel_Btn(GameObject caster)
    {
        onClick_ItemLevel_Btn( caster );
    }

    void _onClick_ItemUnitPrice_Btn(GameObject caster)
    {
        onClick_ItemUnitPrice_Btn( caster );
    }

    void _onClick_Btn_PreviousPage_Btn(GameObject caster)
    {
        onClick_Btn_PreviousPage_Btn( caster );
    }

    void _onClick_Btn_NextPage_Btn(GameObject caster)
    {
        onClick_Btn_NextPage_Btn( caster );
    }

    void _onClick_BuyItemBtn_Btn(GameObject caster)
    {
        onClick_BuyItemBtn_Btn( caster );
    }

    void _onClick_Btn_Change_Btn(GameObject caster)
    {
        onClick_Btn_Change_Btn( caster );
    }

    void _onClick_Btn_TakeOut_Btn(GameObject caster)
    {
        onClick_Btn_TakeOut_Btn( caster );
    }

    void _onClick_Btn_SalesRecords_Btn(GameObject caster)
    {
        onClick_Btn_SalesRecords_Btn( caster );
    }

    void _onClick_Btn_Less_Btn(GameObject caster)
    {
        onClick_Btn_Less_Btn( caster );
    }

    void _onClick_Btn_Add_Btn(GameObject caster)
    {
        onClick_Btn_Add_Btn( caster );
    }

    void _onClick_Btn_Max_Btn(GameObject caster)
    {
        onClick_Btn_Max_Btn( caster );
    }

    void _onClick_Btn_Sell_Btn(GameObject caster)
    {
        onClick_Btn_Sell_Btn( caster );
    }

    void _onClick_CommonBtn_Less_Btn(GameObject caster)
    {
        onClick_CommonBtn_Less_Btn( caster );
    }

    void _onClick_CommonBtn_Add_Btn(GameObject caster)
    {
        onClick_CommonBtn_Add_Btn( caster );
    }

    void _onClick_ListCloseBtn_Btn(GameObject caster)
    {
        onClick_ListCloseBtn_Btn( caster );
    }

    void _onClick_ItemAll_Btn(GameObject caster)
    {
        onClick_ItemAll_Btn( caster );
    }

    void _onClick_ItemEquipment_Btn(GameObject caster)
    {
        onClick_ItemEquipment_Btn( caster );
    }

    void _onClick_ItemUseable_Btn(GameObject caster)
    {
        onClick_ItemUseable_Btn( caster );
    }

    void _onClick_ItemProps_Btn(GameObject caster)
    {
        onClick_ItemProps_Btn( caster );
    }


}
