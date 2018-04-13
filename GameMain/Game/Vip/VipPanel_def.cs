//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class VipPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UIWidget      m_widget_VipContent;

    UIButton      m_btn_PrivilegeCard_1;

    UIButton      m_btn_buy_1;

    Transform     m_trans_NoBuyStatus_1;

    Transform     m_trans_BoughtStatus_1;

    UILabel       m_label_valid_Time_1;

    UIButton      m_btn_PrivilegeCard_2;

    UIButton      m_btn_buy_2;

    Transform     m_trans_NoBuyStatus_2;

    Transform     m_trans_BoughtStatus_2;

    UILabel       m_label_valid_Time_2;

    UIButton      m_btn_PrivilegeCard_3;

    UIButton      m_btn_buy_3;

    Transform     m_trans_NoBuyStatus_3;

    Transform     m_trans_BoughtStatus_3;

    UILabel       m_label_valid_Time_3;

    UIWidget      m_widget_childContent;

    UILabel       m_label_Vip_Tittle;

    UILabel       m_label_detail;

    UIWidget      m_widget_RechargeContent;

    UIScrollView  m_scrollview_RechargeScrollView;

    UIGrid        m_grid_Grid;

    UISprite      m_sprite_childitem;

    UILabel       m_label_IncomeNum;

    UILabel       m_label_Price;

    UIButton      m_btn_Vip;

    UIButton      m_btn_Recharge;


    //初始化控件变量
    protected override void InitControls()
    {
        m_widget_VipContent = GetChildComponent<UIWidget>("VipContent");
       if( null == m_widget_VipContent )
       {
            Engine.Utility.Log.Error("m_widget_VipContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PrivilegeCard_1 = GetChildComponent<UIButton>("PrivilegeCard_1");
       if( null == m_btn_PrivilegeCard_1 )
       {
            Engine.Utility.Log.Error("m_btn_PrivilegeCard_1 为空，请检查prefab是否缺乏组件");
       }
        m_btn_buy_1 = GetChildComponent<UIButton>("buy_1");
       if( null == m_btn_buy_1 )
       {
            Engine.Utility.Log.Error("m_btn_buy_1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NoBuyStatus_1 = GetChildComponent<Transform>("NoBuyStatus_1");
       if( null == m_trans_NoBuyStatus_1 )
       {
            Engine.Utility.Log.Error("m_trans_NoBuyStatus_1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BoughtStatus_1 = GetChildComponent<Transform>("BoughtStatus_1");
       if( null == m_trans_BoughtStatus_1 )
       {
            Engine.Utility.Log.Error("m_trans_BoughtStatus_1 为空，请检查prefab是否缺乏组件");
       }
        m_label_valid_Time_1 = GetChildComponent<UILabel>("valid_Time_1");
       if( null == m_label_valid_Time_1 )
       {
            Engine.Utility.Log.Error("m_label_valid_Time_1 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PrivilegeCard_2 = GetChildComponent<UIButton>("PrivilegeCard_2");
       if( null == m_btn_PrivilegeCard_2 )
       {
            Engine.Utility.Log.Error("m_btn_PrivilegeCard_2 为空，请检查prefab是否缺乏组件");
       }
        m_btn_buy_2 = GetChildComponent<UIButton>("buy_2");
       if( null == m_btn_buy_2 )
       {
            Engine.Utility.Log.Error("m_btn_buy_2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NoBuyStatus_2 = GetChildComponent<Transform>("NoBuyStatus_2");
       if( null == m_trans_NoBuyStatus_2 )
       {
            Engine.Utility.Log.Error("m_trans_NoBuyStatus_2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BoughtStatus_2 = GetChildComponent<Transform>("BoughtStatus_2");
       if( null == m_trans_BoughtStatus_2 )
       {
            Engine.Utility.Log.Error("m_trans_BoughtStatus_2 为空，请检查prefab是否缺乏组件");
       }
        m_label_valid_Time_2 = GetChildComponent<UILabel>("valid_Time_2");
       if( null == m_label_valid_Time_2 )
       {
            Engine.Utility.Log.Error("m_label_valid_Time_2 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PrivilegeCard_3 = GetChildComponent<UIButton>("PrivilegeCard_3");
       if( null == m_btn_PrivilegeCard_3 )
       {
            Engine.Utility.Log.Error("m_btn_PrivilegeCard_3 为空，请检查prefab是否缺乏组件");
       }
        m_btn_buy_3 = GetChildComponent<UIButton>("buy_3");
       if( null == m_btn_buy_3 )
       {
            Engine.Utility.Log.Error("m_btn_buy_3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NoBuyStatus_3 = GetChildComponent<Transform>("NoBuyStatus_3");
       if( null == m_trans_NoBuyStatus_3 )
       {
            Engine.Utility.Log.Error("m_trans_NoBuyStatus_3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BoughtStatus_3 = GetChildComponent<Transform>("BoughtStatus_3");
       if( null == m_trans_BoughtStatus_3 )
       {
            Engine.Utility.Log.Error("m_trans_BoughtStatus_3 为空，请检查prefab是否缺乏组件");
       }
        m_label_valid_Time_3 = GetChildComponent<UILabel>("valid_Time_3");
       if( null == m_label_valid_Time_3 )
       {
            Engine.Utility.Log.Error("m_label_valid_Time_3 为空，请检查prefab是否缺乏组件");
       }
        m_widget_childContent = GetChildComponent<UIWidget>("childContent");
       if( null == m_widget_childContent )
       {
            Engine.Utility.Log.Error("m_widget_childContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_Vip_Tittle = GetChildComponent<UILabel>("Vip_Tittle");
       if( null == m_label_Vip_Tittle )
       {
            Engine.Utility.Log.Error("m_label_Vip_Tittle 为空，请检查prefab是否缺乏组件");
       }
        m_label_detail = GetChildComponent<UILabel>("detail");
       if( null == m_label_detail )
       {
            Engine.Utility.Log.Error("m_label_detail 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RechargeContent = GetChildComponent<UIWidget>("RechargeContent");
       if( null == m_widget_RechargeContent )
       {
            Engine.Utility.Log.Error("m_widget_RechargeContent 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_RechargeScrollView = GetChildComponent<UIScrollView>("RechargeScrollView");
       if( null == m_scrollview_RechargeScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_RechargeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_grid_Grid = GetChildComponent<UIGrid>("Grid");
       if( null == m_grid_Grid )
       {
            Engine.Utility.Log.Error("m_grid_Grid 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_childitem = GetChildComponent<UISprite>("childitem");
       if( null == m_sprite_childitem )
       {
            Engine.Utility.Log.Error("m_sprite_childitem 为空，请检查prefab是否缺乏组件");
       }
        m_label_IncomeNum = GetChildComponent<UILabel>("IncomeNum");
       if( null == m_label_IncomeNum )
       {
            Engine.Utility.Log.Error("m_label_IncomeNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_Price = GetChildComponent<UILabel>("Price");
       if( null == m_label_Price )
       {
            Engine.Utility.Log.Error("m_label_Price 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Vip = GetChildComponent<UIButton>("Vip");
       if( null == m_btn_Vip )
       {
            Engine.Utility.Log.Error("m_btn_Vip 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Recharge = GetChildComponent<UIButton>("Recharge");
       if( null == m_btn_Recharge )
       {
            Engine.Utility.Log.Error("m_btn_Recharge 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_PrivilegeCard_1.gameObject).onClick = _onClick_PrivilegeCard_1_Btn;
        UIEventListener.Get(m_btn_buy_1.gameObject).onClick = _onClick_Buy_1_Btn;
        UIEventListener.Get(m_btn_PrivilegeCard_2.gameObject).onClick = _onClick_PrivilegeCard_2_Btn;
        UIEventListener.Get(m_btn_buy_2.gameObject).onClick = _onClick_Buy_2_Btn;
        UIEventListener.Get(m_btn_PrivilegeCard_3.gameObject).onClick = _onClick_PrivilegeCard_3_Btn;
        UIEventListener.Get(m_btn_buy_3.gameObject).onClick = _onClick_Buy_3_Btn;
        UIEventListener.Get(m_btn_Vip.gameObject).onClick = _onClick_Vip_Btn;
        UIEventListener.Get(m_btn_Recharge.gameObject).onClick = _onClick_Recharge_Btn;
    }

    void _onClick_PrivilegeCard_1_Btn(GameObject caster)
    {
        onClick_PrivilegeCard_1_Btn( caster );
    }

    void _onClick_Buy_1_Btn(GameObject caster)
    {
        onClick_Buy_1_Btn( caster );
    }

    void _onClick_PrivilegeCard_2_Btn(GameObject caster)
    {
        onClick_PrivilegeCard_2_Btn( caster );
    }

    void _onClick_Buy_2_Btn(GameObject caster)
    {
        onClick_Buy_2_Btn( caster );
    }

    void _onClick_PrivilegeCard_3_Btn(GameObject caster)
    {
        onClick_PrivilegeCard_3_Btn( caster );
    }

    void _onClick_Buy_3_Btn(GameObject caster)
    {
        onClick_Buy_3_Btn( caster );
    }

    void _onClick_Vip_Btn(GameObject caster)
    {
        onClick_Vip_Btn( caster );
    }

    void _onClick_Recharge_Btn(GameObject caster)
    {
        onClick_Recharge_Btn( caster );
    }


}
