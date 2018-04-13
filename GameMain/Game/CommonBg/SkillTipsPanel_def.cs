//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SkillTipsPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_bg;

    UISprite             m_sprite_icon;

    UILabel              m_label_name;

    UILabel              m_label_result;

    UILabel              m_label_unlockLevel;

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
        m_sprite_icon = fastComponent.FastGetComponent<UISprite>("icon");
       if( null == m_sprite_icon )
       {
            Engine.Utility.Log.Error("m_sprite_icon 为空，请检查prefab是否缺乏组件");
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
        m_label_unlockLevel = fastComponent.FastGetComponent<UILabel>("unlockLevel");
       if( null == m_label_unlockLevel )
       {
            Engine.Utility.Log.Error("m_label_unlockLevel 为空，请检查prefab是否缺乏组件");
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
