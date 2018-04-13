//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class NpcShopPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_NpcShop;

    UIGridCreatorBase    m_ctor_MallScrollView;

    UIGridCreatorBase    m_ctor_CategoryTagContent;

    Transform            m_trans_RightContent;

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

    Transform            m_trans_OwnMoneyGrid;

    UIButton             m_btn_HandInputBtn;

    UIGridCreatorBase    m_ctor_RightTabRoot;

    Transform            m_trans_UITabGrid;

    Transform            m_trans_TogglePanel;

    Transform            m_trans_UIMallGrid;

    Transform            m_trans_UIItemInfoGrid;


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
        m_trans_NpcShop = fastComponent.FastGetComponent<Transform>("NpcShop");
       if( null == m_trans_NpcShop )
       {
            Engine.Utility.Log.Error("m_trans_NpcShop 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_MallScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("MallScrollView");
       if( null == m_ctor_MallScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_MallScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_CategoryTagContent = fastComponent.FastGetComponent<UIGridCreatorBase>("CategoryTagContent");
       if( null == m_ctor_CategoryTagContent )
       {
            Engine.Utility.Log.Error("m_ctor_CategoryTagContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RightContent = fastComponent.FastGetComponent<Transform>("RightContent");
       if( null == m_trans_RightContent )
       {
            Engine.Utility.Log.Error("m_trans_RightContent 为空，请检查prefab是否缺乏组件");
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
        m_trans_OwnMoneyGrid = fastComponent.FastGetComponent<Transform>("OwnMoneyGrid");
       if( null == m_trans_OwnMoneyGrid )
       {
            Engine.Utility.Log.Error("m_trans_OwnMoneyGrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_HandInputBtn = fastComponent.FastGetComponent<UIButton>("HandInputBtn");
       if( null == m_btn_HandInputBtn )
       {
            Engine.Utility.Log.Error("m_btn_HandInputBtn 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_RightTabRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("RightTabRoot");
       if( null == m_ctor_RightTabRoot )
       {
            Engine.Utility.Log.Error("m_ctor_RightTabRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITabGrid = fastComponent.FastGetComponent<Transform>("UITabGrid");
       if( null == m_trans_UITabGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITabGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TogglePanel = fastComponent.FastGetComponent<Transform>("TogglePanel");
       if( null == m_trans_TogglePanel )
       {
            Engine.Utility.Log.Error("m_trans_TogglePanel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIMallGrid = fastComponent.FastGetComponent<Transform>("UIMallGrid");
       if( null == m_trans_UIMallGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIMallGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemInfoGrid = fastComponent.FastGetComponent<Transform>("UIItemInfoGrid");
       if( null == m_trans_UIItemInfoGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemInfoGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_PurchaseBtn.gameObject).onClick = _onClick_PurchaseBtn_Btn;
        UIEventListener.Get(m_btn_BtnAdd.gameObject).onClick = _onClick_BtnAdd_Btn;
        UIEventListener.Get(m_btn_BtnRemove.gameObject).onClick = _onClick_BtnRemove_Btn;
        UIEventListener.Get(m_btn_BtnMax.gameObject).onClick = _onClick_BtnMax_Btn;
        UIEventListener.Get(m_btn_HandInputBtn.gameObject).onClick = _onClick_HandInputBtn_Btn;
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
