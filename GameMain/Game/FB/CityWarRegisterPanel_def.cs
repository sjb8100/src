//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CityWarRegisterPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_name;

    UIButton             m_btn_close;

    Transform            m_trans_CityWarRegister;

    Transform            m_trans_Left;

    UITexture            m__bgtexture;

    UILabel              m_label_CityWar_name;

    UILabel              m_label_Playerdes;

    UILabel              m_label_ClanNamedes;

    UILabel              m_label_Registertime_label;

    UILabel              m_label_Registertime;

    UILabel              m_label_Starttime;

    UISprite             m_sprite_OpenLimit;

    Transform            m_trans_clanNameRoot;

    Transform            m_trans_ClanRoot;

    UISprite             m_sprite_cityWarDefend;

    UILabel              m_label_ApplyTitle;

    Transform            m_trans_CityWarRegisterScrollView;

    UIWidget             m_widget_CityWarRegisterClan;

    UIButton             m_btn_btn_left;

    UIButton             m_btn_btn_right;

    UIButton             m_btn_btn_tips;

    Transform            m_trans_TipsScrollViewRoot;

    Transform            m_trans_Tips1;

    Transform            m_trans_Tips2;

    Transform            m_trans_Tips3;

    Transform            m_trans_Tips4;

    UIButton             m_btn_TipsContentClose;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CityWarRegister = fastComponent.FastGetComponent<Transform>("CityWarRegister");
       if( null == m_trans_CityWarRegister )
       {
            Engine.Utility.Log.Error("m_trans_CityWarRegister 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Left = fastComponent.FastGetComponent<Transform>("Left");
       if( null == m_trans_Left )
       {
            Engine.Utility.Log.Error("m_trans_Left 为空，请检查prefab是否缺乏组件");
       }
        m__bgtexture = fastComponent.FastGetComponent<UITexture>("bgtexture");
       if( null == m__bgtexture )
       {
            Engine.Utility.Log.Error("m__bgtexture 为空，请检查prefab是否缺乏组件");
       }
        m_label_CityWar_name = fastComponent.FastGetComponent<UILabel>("CityWar_name");
       if( null == m_label_CityWar_name )
       {
            Engine.Utility.Log.Error("m_label_CityWar_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_Playerdes = fastComponent.FastGetComponent<UILabel>("Playerdes");
       if( null == m_label_Playerdes )
       {
            Engine.Utility.Log.Error("m_label_Playerdes 为空，请检查prefab是否缺乏组件");
       }
        m_label_ClanNamedes = fastComponent.FastGetComponent<UILabel>("ClanNamedes");
       if( null == m_label_ClanNamedes )
       {
            Engine.Utility.Log.Error("m_label_ClanNamedes 为空，请检查prefab是否缺乏组件");
       }
        m_label_Registertime_label = fastComponent.FastGetComponent<UILabel>("Registertime_label");
       if( null == m_label_Registertime_label )
       {
            Engine.Utility.Log.Error("m_label_Registertime_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_Registertime = fastComponent.FastGetComponent<UILabel>("Registertime");
       if( null == m_label_Registertime )
       {
            Engine.Utility.Log.Error("m_label_Registertime 为空，请检查prefab是否缺乏组件");
       }
        m_label_Starttime = fastComponent.FastGetComponent<UILabel>("Starttime");
       if( null == m_label_Starttime )
       {
            Engine.Utility.Log.Error("m_label_Starttime 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_OpenLimit = fastComponent.FastGetComponent<UISprite>("OpenLimit");
       if( null == m_sprite_OpenLimit )
       {
            Engine.Utility.Log.Error("m_sprite_OpenLimit 为空，请检查prefab是否缺乏组件");
       }
        m_trans_clanNameRoot = fastComponent.FastGetComponent<Transform>("clanNameRoot");
       if( null == m_trans_clanNameRoot )
       {
            Engine.Utility.Log.Error("m_trans_clanNameRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ClanRoot = fastComponent.FastGetComponent<Transform>("ClanRoot");
       if( null == m_trans_ClanRoot )
       {
            Engine.Utility.Log.Error("m_trans_ClanRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_cityWarDefend = fastComponent.FastGetComponent<UISprite>("cityWarDefend");
       if( null == m_sprite_cityWarDefend )
       {
            Engine.Utility.Log.Error("m_sprite_cityWarDefend 为空，请检查prefab是否缺乏组件");
       }
        m_label_ApplyTitle = fastComponent.FastGetComponent<UILabel>("ApplyTitle");
       if( null == m_label_ApplyTitle )
       {
            Engine.Utility.Log.Error("m_label_ApplyTitle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CityWarRegisterScrollView = fastComponent.FastGetComponent<Transform>("CityWarRegisterScrollView");
       if( null == m_trans_CityWarRegisterScrollView )
       {
            Engine.Utility.Log.Error("m_trans_CityWarRegisterScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_widget_CityWarRegisterClan = fastComponent.FastGetComponent<UIWidget>("CityWarRegisterClan");
       if( null == m_widget_CityWarRegisterClan )
       {
            Engine.Utility.Log.Error("m_widget_CityWarRegisterClan 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_left = fastComponent.FastGetComponent<UIButton>("btn_left");
       if( null == m_btn_btn_left )
       {
            Engine.Utility.Log.Error("m_btn_btn_left 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_right = fastComponent.FastGetComponent<UIButton>("btn_right");
       if( null == m_btn_btn_right )
       {
            Engine.Utility.Log.Error("m_btn_btn_right 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_tips = fastComponent.FastGetComponent<UIButton>("btn_tips");
       if( null == m_btn_btn_tips )
       {
            Engine.Utility.Log.Error("m_btn_btn_tips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TipsScrollViewRoot = fastComponent.FastGetComponent<Transform>("TipsScrollViewRoot");
       if( null == m_trans_TipsScrollViewRoot )
       {
            Engine.Utility.Log.Error("m_trans_TipsScrollViewRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Tips1 = fastComponent.FastGetComponent<Transform>("Tips1");
       if( null == m_trans_Tips1 )
       {
            Engine.Utility.Log.Error("m_trans_Tips1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Tips2 = fastComponent.FastGetComponent<Transform>("Tips2");
       if( null == m_trans_Tips2 )
       {
            Engine.Utility.Log.Error("m_trans_Tips2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Tips3 = fastComponent.FastGetComponent<Transform>("Tips3");
       if( null == m_trans_Tips3 )
       {
            Engine.Utility.Log.Error("m_trans_Tips3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Tips4 = fastComponent.FastGetComponent<Transform>("Tips4");
       if( null == m_trans_Tips4 )
       {
            Engine.Utility.Log.Error("m_trans_Tips4 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TipsContentClose = fastComponent.FastGetComponent<UIButton>("TipsContentClose");
       if( null == m_btn_TipsContentClose )
       {
            Engine.Utility.Log.Error("m_btn_TipsContentClose 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_btn_left.gameObject).onClick = _onClick_Btn_left_Btn;
        UIEventListener.Get(m_btn_btn_right.gameObject).onClick = _onClick_Btn_right_Btn;
        UIEventListener.Get(m_btn_btn_tips.gameObject).onClick = _onClick_Btn_tips_Btn;
        UIEventListener.Get(m_btn_TipsContentClose.gameObject).onClick = _onClick_TipsContentClose_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Btn_left_Btn(GameObject caster)
    {
        onClick_Btn_left_Btn( caster );
    }

    void _onClick_Btn_right_Btn(GameObject caster)
    {
        onClick_Btn_right_Btn( caster );
    }

    void _onClick_Btn_tips_Btn(GameObject caster)
    {
        onClick_Btn_tips_Btn( caster );
    }

    void _onClick_TipsContentClose_Btn(GameObject caster)
    {
        onClick_TipsContentClose_Btn( caster );
    }


}
