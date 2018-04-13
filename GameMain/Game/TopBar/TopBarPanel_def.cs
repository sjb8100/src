//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TopBarPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_MaskTop;

    UILabel              m_label_panelName;

    Transform            m_trans_currencyarea;

    UISprite             m_sprite_TopbarBg;

    UIButton             m_btn_close;

    Transform            m_trans_Help;

    UIButton             m_btn_BtnHelp;

    UISprite             m_sprite_uiPropetyPrefab;

    UILabel              m_label_Num;

    UISprite             m_sprite_MaskBottom;

    UIButton             m_btn_ShopBtn;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_MaskTop = fastComponent.FastGetComponent<UISprite>("MaskTop");
       if( null == m_sprite_MaskTop )
       {
            Engine.Utility.Log.Error("m_sprite_MaskTop 为空，请检查prefab是否缺乏组件");
       }
        m_label_panelName = fastComponent.FastGetComponent<UILabel>("panelName");
       if( null == m_label_panelName )
       {
            Engine.Utility.Log.Error("m_label_panelName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_currencyarea = fastComponent.FastGetComponent<Transform>("currencyarea");
       if( null == m_trans_currencyarea )
       {
            Engine.Utility.Log.Error("m_trans_currencyarea 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_TopbarBg = fastComponent.FastGetComponent<UISprite>("TopbarBg");
       if( null == m_sprite_TopbarBg )
       {
            Engine.Utility.Log.Error("m_sprite_TopbarBg 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Help = fastComponent.FastGetComponent<Transform>("Help");
       if( null == m_trans_Help )
       {
            Engine.Utility.Log.Error("m_trans_Help 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnHelp = fastComponent.FastGetComponent<UIButton>("BtnHelp");
       if( null == m_btn_BtnHelp )
       {
            Engine.Utility.Log.Error("m_btn_BtnHelp 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_uiPropetyPrefab = fastComponent.FastGetComponent<UISprite>("uiPropetyPrefab");
       if( null == m_sprite_uiPropetyPrefab )
       {
            Engine.Utility.Log.Error("m_sprite_uiPropetyPrefab 为空，请检查prefab是否缺乏组件");
       }
        m_label_Num = fastComponent.FastGetComponent<UILabel>("Num");
       if( null == m_label_Num )
       {
            Engine.Utility.Log.Error("m_label_Num 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_MaskBottom = fastComponent.FastGetComponent<UISprite>("MaskBottom");
       if( null == m_sprite_MaskBottom )
       {
            Engine.Utility.Log.Error("m_sprite_MaskBottom 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ShopBtn = fastComponent.FastGetComponent<UIButton>("ShopBtn");
       if( null == m_btn_ShopBtn )
       {
            Engine.Utility.Log.Error("m_btn_ShopBtn 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_BtnHelp.gameObject).onClick = _onClick_BtnHelp_Btn;
        UIEventListener.Get(m_btn_ShopBtn.gameObject).onClick = _onClick_ShopBtn_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_BtnHelp_Btn(GameObject caster)
    {
        onClick_BtnHelp_Btn( caster );
    }

    void _onClick_ShopBtn_Btn(GameObject caster)
    {
        onClick_ShopBtn_Btn( caster );
    }


}
