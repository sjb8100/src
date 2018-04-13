//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class NpcDialogPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_DialogBg;

    UITexture            m__NpcTexture;

    UILabel              m_label_nameLabel;

    UILabel              m_label_LabelNext;

    UIWidget             m_widget_RichText;

    UILabel              m_label_normalText;

    UIWidget             m_widget_dragModel;

    UISprite             m_sprite_select;

    UIButton             m_btn_btn;

    Transform            m_trans_btnroot;

    UIButton             m_btn_btn_jump;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_DialogBg = fastComponent.FastGetComponent<UISprite>("DialogBg");
       if( null == m_sprite_DialogBg )
       {
            Engine.Utility.Log.Error("m_sprite_DialogBg 为空，请检查prefab是否缺乏组件");
       }
        m__NpcTexture = fastComponent.FastGetComponent<UITexture>("NpcTexture");
       if( null == m__NpcTexture )
       {
            Engine.Utility.Log.Error("m__NpcTexture 为空，请检查prefab是否缺乏组件");
       }
        m_label_nameLabel = fastComponent.FastGetComponent<UILabel>("nameLabel");
       if( null == m_label_nameLabel )
       {
            Engine.Utility.Log.Error("m_label_nameLabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_LabelNext = fastComponent.FastGetComponent<UILabel>("LabelNext");
       if( null == m_label_LabelNext )
       {
            Engine.Utility.Log.Error("m_label_LabelNext 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RichText = fastComponent.FastGetComponent<UIWidget>("RichText");
       if( null == m_widget_RichText )
       {
            Engine.Utility.Log.Error("m_widget_RichText 为空，请检查prefab是否缺乏组件");
       }
        m_label_normalText = fastComponent.FastGetComponent<UILabel>("normalText");
       if( null == m_label_normalText )
       {
            Engine.Utility.Log.Error("m_label_normalText 为空，请检查prefab是否缺乏组件");
       }
        m_widget_dragModel = fastComponent.FastGetComponent<UIWidget>("dragModel");
       if( null == m_widget_dragModel )
       {
            Engine.Utility.Log.Error("m_widget_dragModel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_select = fastComponent.FastGetComponent<UISprite>("select");
       if( null == m_sprite_select )
       {
            Engine.Utility.Log.Error("m_sprite_select 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn = fastComponent.FastGetComponent<UIButton>("btn");
       if( null == m_btn_btn )
       {
            Engine.Utility.Log.Error("m_btn_btn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_btnroot = fastComponent.FastGetComponent<Transform>("btnroot");
       if( null == m_trans_btnroot )
       {
            Engine.Utility.Log.Error("m_trans_btnroot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_jump = fastComponent.FastGetComponent<UIButton>("btn_jump");
       if( null == m_btn_btn_jump )
       {
            Engine.Utility.Log.Error("m_btn_btn_jump 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn.gameObject).onClick = _onClick_Btn_Btn;
        UIEventListener.Get(m_btn_btn_jump.gameObject).onClick = _onClick_Btn_jump_Btn;
    }

    void _onClick_Btn_Btn(GameObject caster)
    {
        onClick_Btn_Btn( caster );
    }

    void _onClick_Btn_jump_Btn(GameObject caster)
    {
        onClick_Btn_jump_Btn( caster );
    }


}
