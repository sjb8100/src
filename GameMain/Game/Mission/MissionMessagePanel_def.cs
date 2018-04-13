//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MissionMessagePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_bottom;

    UILabel              m_label_btn_bottom_Label;

    UILabel              m_label_RichText;

    UIWidget             m_widget_normalTask;

    Transform            m_trans_rewardItemScrollView;

    Transform            m_trans_itemRoot;

    UILabel              m_label_normaltargetLabel;

    UIWidget             m_widget_demonTask;

    UISprite             m_sprite_star;

    UILabel              m_label_lableRefreshCoin;

    UILabel              m_label_demontargetLabel;

    Transform            m_trans_DemonFresh;

    UILabel              m_label_lablerefreshmoney;

    UILabel              m_label_remainmoney;

    UILabel              m_label_labledemonexp;

    UIButton             m_btn_btn_refresh;

    UIButton             m_btn_btn_FiveStar;

    UILabel              m_label_title;

    UIButton             m_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_bottom = fastComponent.FastGetComponent<UIButton>("btn_bottom");
       if( null == m_btn_btn_bottom )
       {
            Engine.Utility.Log.Error("m_btn_btn_bottom 为空，请检查prefab是否缺乏组件");
       }
        m_label_btn_bottom_Label = fastComponent.FastGetComponent<UILabel>("btn_bottom_Label");
       if( null == m_label_btn_bottom_Label )
       {
            Engine.Utility.Log.Error("m_label_btn_bottom_Label 为空，请检查prefab是否缺乏组件");
       }
        m_label_RichText = fastComponent.FastGetComponent<UILabel>("RichText");
       if( null == m_label_RichText )
       {
            Engine.Utility.Log.Error("m_label_RichText 为空，请检查prefab是否缺乏组件");
       }
        m_widget_normalTask = fastComponent.FastGetComponent<UIWidget>("normalTask");
       if( null == m_widget_normalTask )
       {
            Engine.Utility.Log.Error("m_widget_normalTask 为空，请检查prefab是否缺乏组件");
       }
        m_trans_rewardItemScrollView = fastComponent.FastGetComponent<Transform>("rewardItemScrollView");
       if( null == m_trans_rewardItemScrollView )
       {
            Engine.Utility.Log.Error("m_trans_rewardItemScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_itemRoot = fastComponent.FastGetComponent<Transform>("itemRoot");
       if( null == m_trans_itemRoot )
       {
            Engine.Utility.Log.Error("m_trans_itemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_normaltargetLabel = fastComponent.FastGetComponent<UILabel>("normaltargetLabel");
       if( null == m_label_normaltargetLabel )
       {
            Engine.Utility.Log.Error("m_label_normaltargetLabel 为空，请检查prefab是否缺乏组件");
       }
        m_widget_demonTask = fastComponent.FastGetComponent<UIWidget>("demonTask");
       if( null == m_widget_demonTask )
       {
            Engine.Utility.Log.Error("m_widget_demonTask 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_star = fastComponent.FastGetComponent<UISprite>("star");
       if( null == m_sprite_star )
       {
            Engine.Utility.Log.Error("m_sprite_star 为空，请检查prefab是否缺乏组件");
       }
        m_label_lableRefreshCoin = fastComponent.FastGetComponent<UILabel>("lableRefreshCoin");
       if( null == m_label_lableRefreshCoin )
       {
            Engine.Utility.Log.Error("m_label_lableRefreshCoin 为空，请检查prefab是否缺乏组件");
       }
        m_label_demontargetLabel = fastComponent.FastGetComponent<UILabel>("demontargetLabel");
       if( null == m_label_demontargetLabel )
       {
            Engine.Utility.Log.Error("m_label_demontargetLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DemonFresh = fastComponent.FastGetComponent<Transform>("DemonFresh");
       if( null == m_trans_DemonFresh )
       {
            Engine.Utility.Log.Error("m_trans_DemonFresh 为空，请检查prefab是否缺乏组件");
       }
        m_label_lablerefreshmoney = fastComponent.FastGetComponent<UILabel>("lablerefreshmoney");
       if( null == m_label_lablerefreshmoney )
       {
            Engine.Utility.Log.Error("m_label_lablerefreshmoney 为空，请检查prefab是否缺乏组件");
       }
        m_label_remainmoney = fastComponent.FastGetComponent<UILabel>("remainmoney");
       if( null == m_label_remainmoney )
       {
            Engine.Utility.Log.Error("m_label_remainmoney 为空，请检查prefab是否缺乏组件");
       }
        m_label_labledemonexp = fastComponent.FastGetComponent<UILabel>("labledemonexp");
       if( null == m_label_labledemonexp )
       {
            Engine.Utility.Log.Error("m_label_labledemonexp 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_refresh = fastComponent.FastGetComponent<UIButton>("btn_refresh");
       if( null == m_btn_btn_refresh )
       {
            Engine.Utility.Log.Error("m_btn_btn_refresh 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_FiveStar = fastComponent.FastGetComponent<UIButton>("btn_FiveStar");
       if( null == m_btn_btn_FiveStar )
       {
            Engine.Utility.Log.Error("m_btn_btn_FiveStar 为空，请检查prefab是否缺乏组件");
       }
        m_label_title = fastComponent.FastGetComponent<UILabel>("title");
       if( null == m_label_title )
       {
            Engine.Utility.Log.Error("m_label_title 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_bottom.gameObject).onClick = _onClick_Btn_bottom_Btn;
        UIEventListener.Get(m_btn_btn_refresh.gameObject).onClick = _onClick_Btn_refresh_Btn;
        UIEventListener.Get(m_btn_btn_FiveStar.gameObject).onClick = _onClick_Btn_FiveStar_Btn;
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Btn_bottom_Btn(GameObject caster)
    {
        onClick_Btn_bottom_Btn( caster );
    }

    void _onClick_Btn_refresh_Btn(GameObject caster)
    {
        onClick_Btn_refresh_Btn( caster );
    }

    void _onClick_Btn_FiveStar_Btn(GameObject caster)
    {
        onClick_Btn_FiveStar_Btn( caster );
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
