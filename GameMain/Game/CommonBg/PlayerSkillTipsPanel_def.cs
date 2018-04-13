//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PlayerSkillTipsPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_bg;

    UILabel              m_label_name;

    UILabel              m_label_result;

    UILabel              m_label_CDNum;

    UILabel              m_label_ManaLabel;

    UITexture            m__iconspr;

    UILabel              m_label_level;

    UILabel              m_label_skillstate;

    UILabel              m_label_attackdis;

    UIButton             m_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_bg = fastComponent.FastGetComponent<UISprite>("bg");
       if( null == m_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_result = fastComponent.FastGetComponent<UILabel>("result");
       if( null == m_label_result )
       {
            Engine.Utility.Log.Error("m_label_result 为空，请检查prefab是否缺乏组件");
       }
        m_label_CDNum = fastComponent.FastGetComponent<UILabel>("CDNum");
       if( null == m_label_CDNum )
       {
            Engine.Utility.Log.Error("m_label_CDNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_ManaLabel = fastComponent.FastGetComponent<UILabel>("ManaLabel");
       if( null == m_label_ManaLabel )
       {
            Engine.Utility.Log.Error("m_label_ManaLabel 为空，请检查prefab是否缺乏组件");
       }
        m__iconspr = fastComponent.FastGetComponent<UITexture>("iconspr");
       if( null == m__iconspr )
       {
            Engine.Utility.Log.Error("m__iconspr 为空，请检查prefab是否缺乏组件");
       }
        m_label_level = fastComponent.FastGetComponent<UILabel>("level");
       if( null == m_label_level )
       {
            Engine.Utility.Log.Error("m_label_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_skillstate = fastComponent.FastGetComponent<UILabel>("skillstate");
       if( null == m_label_skillstate )
       {
            Engine.Utility.Log.Error("m_label_skillstate 为空，请检查prefab是否缺乏组件");
       }
        m_label_attackdis = fastComponent.FastGetComponent<UILabel>("attackdis");
       if( null == m_label_attackdis )
       {
            Engine.Utility.Log.Error("m_label_attackdis 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
