//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SawySkillPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    UIButton             m_btn_btn_cancel;

    UIButton             m_btn_btn_lingwu;

    UILabel              m_label_skill_effect_Label;

    UISprite             m_sprite_xiaohao_icon;

    UISprite             m_sprite_itemqua;

    UILabel              m_label_xiaohao_name;

    UILabel              m_label_xiaohao_number;

    UIToggle             m_toggle_xiaohao_Sprite;

    UILabel              m_label_xiaohao_dianjuan;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_cancel = fastComponent.FastGetComponent<UIButton>("btn_cancel");
       if( null == m_btn_btn_cancel )
       {
            Engine.Utility.Log.Error("m_btn_btn_cancel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_lingwu = fastComponent.FastGetComponent<UIButton>("btn_lingwu");
       if( null == m_btn_btn_lingwu )
       {
            Engine.Utility.Log.Error("m_btn_btn_lingwu 为空，请检查prefab是否缺乏组件");
       }
        m_label_skill_effect_Label = fastComponent.FastGetComponent<UILabel>("skill_effect_Label");
       if( null == m_label_skill_effect_Label )
       {
            Engine.Utility.Log.Error("m_label_skill_effect_Label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_xiaohao_icon = fastComponent.FastGetComponent<UISprite>("xiaohao_icon");
       if( null == m_sprite_xiaohao_icon )
       {
            Engine.Utility.Log.Error("m_sprite_xiaohao_icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_itemqua = fastComponent.FastGetComponent<UISprite>("itemqua");
       if( null == m_sprite_itemqua )
       {
            Engine.Utility.Log.Error("m_sprite_itemqua 为空，请检查prefab是否缺乏组件");
       }
        m_label_xiaohao_name = fastComponent.FastGetComponent<UILabel>("xiaohao_name");
       if( null == m_label_xiaohao_name )
       {
            Engine.Utility.Log.Error("m_label_xiaohao_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_xiaohao_number = fastComponent.FastGetComponent<UILabel>("xiaohao_number");
       if( null == m_label_xiaohao_number )
       {
            Engine.Utility.Log.Error("m_label_xiaohao_number 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_xiaohao_Sprite = fastComponent.FastGetComponent<UIToggle>("xiaohao_Sprite");
       if( null == m_toggle_xiaohao_Sprite )
       {
            Engine.Utility.Log.Error("m_toggle_xiaohao_Sprite 为空，请检查prefab是否缺乏组件");
       }
        m_label_xiaohao_dianjuan = fastComponent.FastGetComponent<UILabel>("xiaohao_dianjuan");
       if( null == m_label_xiaohao_dianjuan )
       {
            Engine.Utility.Log.Error("m_label_xiaohao_dianjuan 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_cancel.gameObject).onClick = _onClick_Btn_cancel_Btn;
        UIEventListener.Get(m_btn_btn_lingwu.gameObject).onClick = _onClick_Btn_lingwu_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Btn_cancel_Btn(GameObject caster)
    {
        onClick_Btn_cancel_Btn( caster );
    }

    void _onClick_Btn_lingwu_Btn(GameObject caster)
    {
        onClick_Btn_lingwu_Btn( caster );
    }


}
