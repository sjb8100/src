//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class HomeTradePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_ScrollViewContent;

    Transform            m_trans_CategoryTagContent;

    UIWidget             m_widget_RefreshTime;

    UILabel              m_label_refresh_time;

    Transform            m_trans_RightContent;

    Transform            m_trans_FunctioToggles;

    UIButton             m_btn_BuyToggle;

    UIButton             m_btn_SellToggle;

    Transform            m_trans_PurchaseContent;

    Transform            m_trans_MallItemInfo;

    UILabel              m_label_MallItemName;

    UILabel              m_label_MallItemDes;

    UILabel              m_label_ChooseMallItemNotice;

    UIButton             m_btn_PurchaseBtn;

    UILabel              m_label_Btn_name;

    UILabel              m_label_Hold_number;

    UILabel              m_label_AddRemove_Text;

    UIButton             m_btn_BtnAdd;

    UIButton             m_btn_BtnRemove;

    UILabel              m_label_PurchaseNum;

    Transform            m_trans_PurchaseCostGrid;

    UILabel              m_label_Obtain_Text;


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
        m_trans_ScrollViewContent = fastComponent.FastGetComponent<Transform>("ScrollViewContent");
       if( null == m_trans_ScrollViewContent )
       {
            Engine.Utility.Log.Error("m_trans_ScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CategoryTagContent = fastComponent.FastGetComponent<Transform>("CategoryTagContent");
       if( null == m_trans_CategoryTagContent )
       {
            Engine.Utility.Log.Error("m_trans_CategoryTagContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RefreshTime = fastComponent.FastGetComponent<UIWidget>("RefreshTime");
       if( null == m_widget_RefreshTime )
       {
            Engine.Utility.Log.Error("m_widget_RefreshTime 为空，请检查prefab是否缺乏组件");
       }
        m_label_refresh_time = fastComponent.FastGetComponent<UILabel>("refresh_time");
       if( null == m_label_refresh_time )
       {
            Engine.Utility.Log.Error("m_label_refresh_time 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RightContent = fastComponent.FastGetComponent<Transform>("RightContent");
       if( null == m_trans_RightContent )
       {
            Engine.Utility.Log.Error("m_trans_RightContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FunctioToggles = fastComponent.FastGetComponent<Transform>("FunctioToggles");
       if( null == m_trans_FunctioToggles )
       {
            Engine.Utility.Log.Error("m_trans_FunctioToggles 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BuyToggle = fastComponent.FastGetComponent<UIButton>("BuyToggle");
       if( null == m_btn_BuyToggle )
       {
            Engine.Utility.Log.Error("m_btn_BuyToggle 为空，请检查prefab是否缺乏组件");
       }
        m_btn_SellToggle = fastComponent.FastGetComponent<UIButton>("SellToggle");
       if( null == m_btn_SellToggle )
       {
            Engine.Utility.Log.Error("m_btn_SellToggle 为空，请检查prefab是否缺乏组件");
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
        m_label_ChooseMallItemNotice = fastComponent.FastGetComponent<UILabel>("ChooseMallItemNotice");
       if( null == m_label_ChooseMallItemNotice )
       {
            Engine.Utility.Log.Error("m_label_ChooseMallItemNotice 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PurchaseBtn = fastComponent.FastGetComponent<UIButton>("PurchaseBtn");
       if( null == m_btn_PurchaseBtn )
       {
            Engine.Utility.Log.Error("m_btn_PurchaseBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_Btn_name = fastComponent.FastGetComponent<UILabel>("Btn_name");
       if( null == m_label_Btn_name )
       {
            Engine.Utility.Log.Error("m_label_Btn_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_Hold_number = fastComponent.FastGetComponent<UILabel>("Hold_number");
       if( null == m_label_Hold_number )
       {
            Engine.Utility.Log.Error("m_label_Hold_number 为空，请检查prefab是否缺乏组件");
       }
        m_label_AddRemove_Text = fastComponent.FastGetComponent<UILabel>("AddRemove_Text");
       if( null == m_label_AddRemove_Text )
       {
            Engine.Utility.Log.Error("m_label_AddRemove_Text 为空，请检查prefab是否缺乏组件");
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
        m_trans_PurchaseCostGrid = fastComponent.FastGetComponent<Transform>("PurchaseCostGrid");
       if( null == m_trans_PurchaseCostGrid )
       {
            Engine.Utility.Log.Error("m_trans_PurchaseCostGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_Obtain_Text = fastComponent.FastGetComponent<UILabel>("Obtain_Text");
       if( null == m_label_Obtain_Text )
       {
            Engine.Utility.Log.Error("m_label_Obtain_Text 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BuyToggle.gameObject).onClick = _onClick_BuyToggle_Btn;
        UIEventListener.Get(m_btn_SellToggle.gameObject).onClick = _onClick_SellToggle_Btn;
        UIEventListener.Get(m_btn_PurchaseBtn.gameObject).onClick = _onClick_PurchaseBtn_Btn;
        UIEventListener.Get(m_btn_BtnAdd.gameObject).onClick = _onClick_BtnAdd_Btn;
        UIEventListener.Get(m_btn_BtnRemove.gameObject).onClick = _onClick_BtnRemove_Btn;
    }

    void _onClick_BuyToggle_Btn(GameObject caster)
    {
        onClick_BuyToggle_Btn( caster );
    }

    void _onClick_SellToggle_Btn(GameObject caster)
    {
        onClick_SellToggle_Btn( caster );
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


}
