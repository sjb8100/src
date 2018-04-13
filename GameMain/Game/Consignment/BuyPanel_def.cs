//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class BuyPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UITexture            m__Icon;

    UILabel              m_label_Name;

    UILabel              m_label_SellNum;

    UISprite             m_sprite_moneyIcon;

    UILabel              m_label_UnitPrice;

    UIButton             m_btn_Btn_Less;

    UIButton             m_btn_Btn_Add;

    UILabel              m_label_UnitNum;

    UIButton             m_btn_Btn_Max;

    UISprite             m_sprite_moneyIcon2;

    UILabel              m_label_TotalPriceNum;

    UIButton             m_btn_Btn_Buy;

    UIButton             m_btn_Btn_Canel;

    UIWidget             m_widget_ContainerBox;

    UILabel              m_label_Title;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m__Icon = fastComponent.FastGetComponent<UITexture>("Icon");
       if( null == m__Icon )
       {
            Engine.Utility.Log.Error("m__Icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_Name = fastComponent.FastGetComponent<UILabel>("Name");
       if( null == m_label_Name )
       {
            Engine.Utility.Log.Error("m_label_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_SellNum = fastComponent.FastGetComponent<UILabel>("SellNum");
       if( null == m_label_SellNum )
       {
            Engine.Utility.Log.Error("m_label_SellNum 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_moneyIcon = fastComponent.FastGetComponent<UISprite>("moneyIcon");
       if( null == m_sprite_moneyIcon )
       {
            Engine.Utility.Log.Error("m_sprite_moneyIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_UnitPrice = fastComponent.FastGetComponent<UILabel>("UnitPrice");
       if( null == m_label_UnitPrice )
       {
            Engine.Utility.Log.Error("m_label_UnitPrice 为空，请检查prefab是否缺乏组件");
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
        m_label_UnitNum = fastComponent.FastGetComponent<UILabel>("UnitNum");
       if( null == m_label_UnitNum )
       {
            Engine.Utility.Log.Error("m_label_UnitNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Max = fastComponent.FastGetComponent<UIButton>("Btn_Max");
       if( null == m_btn_Btn_Max )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Max 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_moneyIcon2 = fastComponent.FastGetComponent<UISprite>("moneyIcon2");
       if( null == m_sprite_moneyIcon2 )
       {
            Engine.Utility.Log.Error("m_sprite_moneyIcon2 为空，请检查prefab是否缺乏组件");
       }
        m_label_TotalPriceNum = fastComponent.FastGetComponent<UILabel>("TotalPriceNum");
       if( null == m_label_TotalPriceNum )
       {
            Engine.Utility.Log.Error("m_label_TotalPriceNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Buy = fastComponent.FastGetComponent<UIButton>("Btn_Buy");
       if( null == m_btn_Btn_Buy )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Buy 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Canel = fastComponent.FastGetComponent<UIButton>("Btn_Canel");
       if( null == m_btn_Btn_Canel )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Canel 为空，请检查prefab是否缺乏组件");
       }
        m_widget_ContainerBox = fastComponent.FastGetComponent<UIWidget>("ContainerBox");
       if( null == m_widget_ContainerBox )
       {
            Engine.Utility.Log.Error("m_widget_ContainerBox 为空，请检查prefab是否缺乏组件");
       }
        m_label_Title = fastComponent.FastGetComponent<UILabel>("Title");
       if( null == m_label_Title )
       {
            Engine.Utility.Log.Error("m_label_Title 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Btn_Less.gameObject).onClick = _onClick_Btn_Less_Btn;
        UIEventListener.Get(m_btn_Btn_Add.gameObject).onClick = _onClick_Btn_Add_Btn;
        UIEventListener.Get(m_btn_Btn_Max.gameObject).onClick = _onClick_Btn_Max_Btn;
        UIEventListener.Get(m_btn_Btn_Buy.gameObject).onClick = _onClick_Btn_Buy_Btn;
        UIEventListener.Get(m_btn_Btn_Canel.gameObject).onClick = _onClick_Btn_Canel_Btn;
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

    void _onClick_Btn_Buy_Btn(GameObject caster)
    {
        onClick_Btn_Buy_Btn( caster );
    }

    void _onClick_Btn_Canel_Btn(GameObject caster)
    {
        onClick_Btn_Canel_Btn( caster );
    }


}
