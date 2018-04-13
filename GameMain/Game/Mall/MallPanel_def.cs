//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MallPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//商城
		YuanBao = 1,
		//银两
		YinLiang = 2,
		//皇令
		HuangLing = 3,
		//充值
		ChongZhi = 4,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_Mall;

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

    Transform            m_trans_MallInfoGrid;

    UIButton             m_btn_PurchaseBtn;

    UILabel              m_label_PurchaseLeft;

    UIButton             m_btn_BtnAdd;

    UIButton             m_btn_BtnRemove;

    UILabel              m_label_PurchaseNum;

    UIButton             m_btn_BtnMax;

    Transform            m_trans_PurchaseCostGrid;

    UIButton             m_btn_HandInputBtn;

    Transform            m_trans_HuangLing;

    UIGridCreatorBase    m_ctor_NobleContentRoot;

    UIButton             m_btn_TipBtn;

    UISprite             m_sprite_nobleJiHuoIcon;

    UILabel              m_label_TipsLabel;

    UISprite             m_sprite_prerogative;

    UILabel              m_label_TextContent;

    UIButton             m_btn_Container;

    Transform            m_trans_Recharge;

    Transform            m_trans_RechargeScrollView;

    UIButton             m_btn_RefreshBtn;

    Transform            m_trans_UIMallGrid;

    Transform            m_trans_UINobleGrid;

    Transform            m_trans_UIRechargeGrid;

    Transform            m_trans_UITabGrid;

    Transform            m_trans_UIBlockIndexGrid;

    Transform            m_trans_RightTabRoot;


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
        m_trans_Mall = fastComponent.FastGetComponent<Transform>("Mall");
       if( null == m_trans_Mall )
       {
            Engine.Utility.Log.Error("m_trans_Mall 为空，请检查prefab是否缺乏组件");
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
        m_trans_MallInfoGrid = fastComponent.FastGetComponent<Transform>("MallInfoGrid");
       if( null == m_trans_MallInfoGrid )
       {
            Engine.Utility.Log.Error("m_trans_MallInfoGrid 为空，请检查prefab是否缺乏组件");
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
        m_trans_HuangLing = fastComponent.FastGetComponent<Transform>("HuangLing");
       if( null == m_trans_HuangLing )
       {
            Engine.Utility.Log.Error("m_trans_HuangLing 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_NobleContentRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("NobleContentRoot");
       if( null == m_ctor_NobleContentRoot )
       {
            Engine.Utility.Log.Error("m_ctor_NobleContentRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TipBtn = fastComponent.FastGetComponent<UIButton>("TipBtn");
       if( null == m_btn_TipBtn )
       {
            Engine.Utility.Log.Error("m_btn_TipBtn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_nobleJiHuoIcon = fastComponent.FastGetComponent<UISprite>("nobleJiHuoIcon");
       if( null == m_sprite_nobleJiHuoIcon )
       {
            Engine.Utility.Log.Error("m_sprite_nobleJiHuoIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_TipsLabel = fastComponent.FastGetComponent<UILabel>("TipsLabel");
       if( null == m_label_TipsLabel )
       {
            Engine.Utility.Log.Error("m_label_TipsLabel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_prerogative = fastComponent.FastGetComponent<UISprite>("prerogative");
       if( null == m_sprite_prerogative )
       {
            Engine.Utility.Log.Error("m_sprite_prerogative 为空，请检查prefab是否缺乏组件");
       }
        m_label_TextContent = fastComponent.FastGetComponent<UILabel>("TextContent");
       if( null == m_label_TextContent )
       {
            Engine.Utility.Log.Error("m_label_TextContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Container = fastComponent.FastGetComponent<UIButton>("Container");
       if( null == m_btn_Container )
       {
            Engine.Utility.Log.Error("m_btn_Container 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Recharge = fastComponent.FastGetComponent<Transform>("Recharge");
       if( null == m_trans_Recharge )
       {
            Engine.Utility.Log.Error("m_trans_Recharge 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RechargeScrollView = fastComponent.FastGetComponent<Transform>("RechargeScrollView");
       if( null == m_trans_RechargeScrollView )
       {
            Engine.Utility.Log.Error("m_trans_RechargeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RefreshBtn = fastComponent.FastGetComponent<UIButton>("RefreshBtn");
       if( null == m_btn_RefreshBtn )
       {
            Engine.Utility.Log.Error("m_btn_RefreshBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIMallGrid = fastComponent.FastGetComponent<Transform>("UIMallGrid");
       if( null == m_trans_UIMallGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIMallGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UINobleGrid = fastComponent.FastGetComponent<Transform>("UINobleGrid");
       if( null == m_trans_UINobleGrid )
       {
            Engine.Utility.Log.Error("m_trans_UINobleGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIRechargeGrid = fastComponent.FastGetComponent<Transform>("UIRechargeGrid");
       if( null == m_trans_UIRechargeGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIRechargeGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITabGrid = fastComponent.FastGetComponent<Transform>("UITabGrid");
       if( null == m_trans_UITabGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITabGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIBlockIndexGrid = fastComponent.FastGetComponent<Transform>("UIBlockIndexGrid");
       if( null == m_trans_UIBlockIndexGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIBlockIndexGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RightTabRoot = fastComponent.FastGetComponent<Transform>("RightTabRoot");
       if( null == m_trans_RightTabRoot )
       {
            Engine.Utility.Log.Error("m_trans_RightTabRoot 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_TipBtn.gameObject).onClick = _onClick_TipBtn_Btn;
        UIEventListener.Get(m_btn_Container.gameObject).onClick = _onClick_Container_Btn;
        UIEventListener.Get(m_btn_RefreshBtn.gameObject).onClick = _onClick_RefreshBtn_Btn;
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

    void _onClick_TipBtn_Btn(GameObject caster)
    {
        onClick_TipBtn_Btn( caster );
    }

    void _onClick_Container_Btn(GameObject caster)
    {
        onClick_Container_Btn( caster );
    }

    void _onClick_RefreshBtn_Btn(GameObject caster)
    {
        onClick_RefreshBtn_Btn( caster );
    }


}
