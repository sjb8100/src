//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class BusinessmanPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UIButton      m_btn_close;

    UIButton      m_btn_buy;

    UIButton      m_btn_sell;

    UISprite      m_sprite_Animal;

    UILabel       m_label_Animal_name;

    UISprite      m_sprite_Fram;

    UILabel       m_label_Fram_name;

    UIWidget      m_widget_RefreshTime;

    UILabel       m_label_refresh_time;


    //初始化控件变量
    protected override void InitControls()
    {
        m_btn_close = GetChildComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_buy = GetChildComponent<UIButton>("buy");
       if( null == m_btn_buy )
       {
            Engine.Utility.Log.Error("m_btn_buy 为空，请检查prefab是否缺乏组件");
       }
        m_btn_sell = GetChildComponent<UIButton>("sell");
       if( null == m_btn_sell )
       {
            Engine.Utility.Log.Error("m_btn_sell 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Animal = GetChildComponent<UISprite>("Animal");
       if( null == m_sprite_Animal )
       {
            Engine.Utility.Log.Error("m_sprite_Animal 为空，请检查prefab是否缺乏组件");
       }
        m_label_Animal_name = GetChildComponent<UILabel>("Animal_name");
       if( null == m_label_Animal_name )
       {
            Engine.Utility.Log.Error("m_label_Animal_name 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Fram = GetChildComponent<UISprite>("Fram");
       if( null == m_sprite_Fram )
       {
            Engine.Utility.Log.Error("m_sprite_Fram 为空，请检查prefab是否缺乏组件");
       }
        m_label_Fram_name = GetChildComponent<UILabel>("Fram_name");
       if( null == m_label_Fram_name )
       {
            Engine.Utility.Log.Error("m_label_Fram_name 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RefreshTime = GetChildComponent<UIWidget>("RefreshTime");
       if( null == m_widget_RefreshTime )
       {
            Engine.Utility.Log.Error("m_widget_RefreshTime 为空，请检查prefab是否缺乏组件");
       }
        m_label_refresh_time = GetChildComponent<UILabel>("refresh_time");
       if( null == m_label_refresh_time )
       {
            Engine.Utility.Log.Error("m_label_refresh_time 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_buy.gameObject).onClick = _onClick_Buy_Btn;
        UIEventListener.Get(m_btn_sell.gameObject).onClick = _onClick_Sell_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Buy_Btn(GameObject caster)
    {
        onClick_Buy_Btn( caster );
    }

    void _onClick_Sell_Btn(GameObject caster)
    {
        onClick_Sell_Btn( caster );
    }


}
