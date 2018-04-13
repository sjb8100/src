//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CommonSingleUseItemPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_unlock_close;

    UILabel              m_label_title;

    UILabel              m_label_contentdes;

    UILabel              m_label_unlock_item_name;

    UISprite             m_sprite_unlock_item_icon;

    UILabel              m_label_neednum;

    Transform            m_trans_moneycontent;

    UISprite             m_sprite_moneyspr;

    UILabel              m_label_moneynum;

    UIButton             m_btn_cancel;

    UILabel              m_label_canceltext;

    UIButton             m_btn_ok;

    UILabel              m_label_oktext;

    Transform            m_trans_autobuy;

    UISprite             m_sprite_biankuang;

    UISprite             m_sprite_autoFlag;

    UILabel              m_label_autobuydes;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_unlock_close = fastComponent.FastGetComponent<UIButton>("unlock_close");
       if( null == m_btn_unlock_close )
       {
            Engine.Utility.Log.Error("m_btn_unlock_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_title = fastComponent.FastGetComponent<UILabel>("title");
       if( null == m_label_title )
       {
            Engine.Utility.Log.Error("m_label_title 为空，请检查prefab是否缺乏组件");
       }
        m_label_contentdes = fastComponent.FastGetComponent<UILabel>("contentdes");
       if( null == m_label_contentdes )
       {
            Engine.Utility.Log.Error("m_label_contentdes 为空，请检查prefab是否缺乏组件");
       }
        m_label_unlock_item_name = fastComponent.FastGetComponent<UILabel>("unlock_item_name");
       if( null == m_label_unlock_item_name )
       {
            Engine.Utility.Log.Error("m_label_unlock_item_name 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_unlock_item_icon = fastComponent.FastGetComponent<UISprite>("unlock_item_icon");
       if( null == m_sprite_unlock_item_icon )
       {
            Engine.Utility.Log.Error("m_sprite_unlock_item_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_neednum = fastComponent.FastGetComponent<UILabel>("neednum");
       if( null == m_label_neednum )
       {
            Engine.Utility.Log.Error("m_label_neednum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_moneycontent = fastComponent.FastGetComponent<Transform>("moneycontent");
       if( null == m_trans_moneycontent )
       {
            Engine.Utility.Log.Error("m_trans_moneycontent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_moneyspr = fastComponent.FastGetComponent<UISprite>("moneyspr");
       if( null == m_sprite_moneyspr )
       {
            Engine.Utility.Log.Error("m_sprite_moneyspr 为空，请检查prefab是否缺乏组件");
       }
        m_label_moneynum = fastComponent.FastGetComponent<UILabel>("moneynum");
       if( null == m_label_moneynum )
       {
            Engine.Utility.Log.Error("m_label_moneynum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_cancel = fastComponent.FastGetComponent<UIButton>("cancel");
       if( null == m_btn_cancel )
       {
            Engine.Utility.Log.Error("m_btn_cancel 为空，请检查prefab是否缺乏组件");
       }
        m_label_canceltext = fastComponent.FastGetComponent<UILabel>("canceltext");
       if( null == m_label_canceltext )
       {
            Engine.Utility.Log.Error("m_label_canceltext 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ok = fastComponent.FastGetComponent<UIButton>("ok");
       if( null == m_btn_ok )
       {
            Engine.Utility.Log.Error("m_btn_ok 为空，请检查prefab是否缺乏组件");
       }
        m_label_oktext = fastComponent.FastGetComponent<UILabel>("oktext");
       if( null == m_label_oktext )
       {
            Engine.Utility.Log.Error("m_label_oktext 为空，请检查prefab是否缺乏组件");
       }
        m_trans_autobuy = fastComponent.FastGetComponent<Transform>("autobuy");
       if( null == m_trans_autobuy )
       {
            Engine.Utility.Log.Error("m_trans_autobuy 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_biankuang = fastComponent.FastGetComponent<UISprite>("biankuang");
       if( null == m_sprite_biankuang )
       {
            Engine.Utility.Log.Error("m_sprite_biankuang 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_autoFlag = fastComponent.FastGetComponent<UISprite>("autoFlag");
       if( null == m_sprite_autoFlag )
       {
            Engine.Utility.Log.Error("m_sprite_autoFlag 为空，请检查prefab是否缺乏组件");
       }
        m_label_autobuydes = fastComponent.FastGetComponent<UILabel>("autobuydes");
       if( null == m_label_autobuydes )
       {
            Engine.Utility.Log.Error("m_label_autobuydes 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_unlock_close.gameObject).onClick = _onClick_Unlock_close_Btn;
        UIEventListener.Get(m_btn_cancel.gameObject).onClick = _onClick_Cancel_Btn;
        UIEventListener.Get(m_btn_ok.gameObject).onClick = _onClick_Ok_Btn;
    }

    void _onClick_Unlock_close_Btn(GameObject caster)
    {
        onClick_Unlock_close_Btn( caster );
    }

    void _onClick_Cancel_Btn(GameObject caster)
    {
        onClick_Cancel_Btn( caster );
    }

    void _onClick_Ok_Btn(GameObject caster)
    {
        onClick_Ok_Btn( caster );
    }


}
