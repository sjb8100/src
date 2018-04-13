//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PlayerOpreatePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_sprite_bg;

    UISprite             m_sprite_head_di;

    UITexture            m__icon_head;

    UILabel              m_label_playername;

    UILabel              m_label_playerClan;

    UILabel              m_label_playerLevel;

    UIButton             m_btn_btn_sendmessage;

    UIButton             m_btn_btn_checkmessage;

    UIButton             m_btn_btn_pinvite;

    UIButton             m_btn_btn_apply;

    UIButton             m_btn_btn_addfriend;

    UIButton             m_btn_btn_visit;

    UIButton             m_btn_btn_remove;

    UIButton             m_btn_btn_shield;

    UIButton             m_btn_btn_changeDuty;

    UIButton             m_btn_btn_expel;

    UIButton             m_btn_btnClose;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_sprite_bg = fastComponent.FastGetComponent<UISprite>("sprite_bg");
       if( null == m_sprite_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_head_di = fastComponent.FastGetComponent<UISprite>("head_di");
       if( null == m_sprite_head_di )
       {
            Engine.Utility.Log.Error("m_sprite_head_di 为空，请检查prefab是否缺乏组件");
       }
        m__icon_head = fastComponent.FastGetComponent<UITexture>("icon_head");
       if( null == m__icon_head )
       {
            Engine.Utility.Log.Error("m__icon_head 为空，请检查prefab是否缺乏组件");
       }
        m_label_playername = fastComponent.FastGetComponent<UILabel>("playername");
       if( null == m_label_playername )
       {
            Engine.Utility.Log.Error("m_label_playername 为空，请检查prefab是否缺乏组件");
       }
        m_label_playerClan = fastComponent.FastGetComponent<UILabel>("playerClan");
       if( null == m_label_playerClan )
       {
            Engine.Utility.Log.Error("m_label_playerClan 为空，请检查prefab是否缺乏组件");
       }
        m_label_playerLevel = fastComponent.FastGetComponent<UILabel>("playerLevel");
       if( null == m_label_playerLevel )
       {
            Engine.Utility.Log.Error("m_label_playerLevel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_sendmessage = fastComponent.FastGetComponent<UIButton>("btn_sendmessage");
       if( null == m_btn_btn_sendmessage )
       {
            Engine.Utility.Log.Error("m_btn_btn_sendmessage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_checkmessage = fastComponent.FastGetComponent<UIButton>("btn_checkmessage");
       if( null == m_btn_btn_checkmessage )
       {
            Engine.Utility.Log.Error("m_btn_btn_checkmessage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_pinvite = fastComponent.FastGetComponent<UIButton>("btn_pinvite");
       if( null == m_btn_btn_pinvite )
       {
            Engine.Utility.Log.Error("m_btn_btn_pinvite 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_apply = fastComponent.FastGetComponent<UIButton>("btn_apply");
       if( null == m_btn_btn_apply )
       {
            Engine.Utility.Log.Error("m_btn_btn_apply 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_addfriend = fastComponent.FastGetComponent<UIButton>("btn_addfriend");
       if( null == m_btn_btn_addfriend )
       {
            Engine.Utility.Log.Error("m_btn_btn_addfriend 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_visit = fastComponent.FastGetComponent<UIButton>("btn_visit");
       if( null == m_btn_btn_visit )
       {
            Engine.Utility.Log.Error("m_btn_btn_visit 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_remove = fastComponent.FastGetComponent<UIButton>("btn_remove");
       if( null == m_btn_btn_remove )
       {
            Engine.Utility.Log.Error("m_btn_btn_remove 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_shield = fastComponent.FastGetComponent<UIButton>("btn_shield");
       if( null == m_btn_btn_shield )
       {
            Engine.Utility.Log.Error("m_btn_btn_shield 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_changeDuty = fastComponent.FastGetComponent<UIButton>("btn_changeDuty");
       if( null == m_btn_btn_changeDuty )
       {
            Engine.Utility.Log.Error("m_btn_btn_changeDuty 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_expel = fastComponent.FastGetComponent<UIButton>("btn_expel");
       if( null == m_btn_btn_expel )
       {
            Engine.Utility.Log.Error("m_btn_btn_expel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnClose = fastComponent.FastGetComponent<UIButton>("btnClose");
       if( null == m_btn_btnClose )
       {
            Engine.Utility.Log.Error("m_btn_btnClose 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_sendmessage.gameObject).onClick = _onClick_Btn_sendmessage_Btn;
        UIEventListener.Get(m_btn_btn_checkmessage.gameObject).onClick = _onClick_Btn_checkmessage_Btn;
        UIEventListener.Get(m_btn_btn_pinvite.gameObject).onClick = _onClick_Btn_pinvite_Btn;
        UIEventListener.Get(m_btn_btn_apply.gameObject).onClick = _onClick_Btn_apply_Btn;
        UIEventListener.Get(m_btn_btn_addfriend.gameObject).onClick = _onClick_Btn_addfriend_Btn;
        UIEventListener.Get(m_btn_btn_visit.gameObject).onClick = _onClick_Btn_visit_Btn;
        UIEventListener.Get(m_btn_btn_remove.gameObject).onClick = _onClick_Btn_remove_Btn;
        UIEventListener.Get(m_btn_btn_shield.gameObject).onClick = _onClick_Btn_shield_Btn;
        UIEventListener.Get(m_btn_btn_changeDuty.gameObject).onClick = _onClick_Btn_changeDuty_Btn;
        UIEventListener.Get(m_btn_btn_expel.gameObject).onClick = _onClick_Btn_expel_Btn;
        UIEventListener.Get(m_btn_btnClose.gameObject).onClick = _onClick_BtnClose_Btn;
    }

    void _onClick_Btn_sendmessage_Btn(GameObject caster)
    {
        onClick_Btn_sendmessage_Btn( caster );
    }

    void _onClick_Btn_checkmessage_Btn(GameObject caster)
    {
        onClick_Btn_checkmessage_Btn( caster );
    }

    void _onClick_Btn_pinvite_Btn(GameObject caster)
    {
        onClick_Btn_pinvite_Btn( caster );
    }

    void _onClick_Btn_apply_Btn(GameObject caster)
    {
        onClick_Btn_apply_Btn( caster );
    }

    void _onClick_Btn_addfriend_Btn(GameObject caster)
    {
        onClick_Btn_addfriend_Btn( caster );
    }

    void _onClick_Btn_visit_Btn(GameObject caster)
    {
        onClick_Btn_visit_Btn( caster );
    }

    void _onClick_Btn_remove_Btn(GameObject caster)
    {
        onClick_Btn_remove_Btn( caster );
    }

    void _onClick_Btn_shield_Btn(GameObject caster)
    {
        onClick_Btn_shield_Btn( caster );
    }

    void _onClick_Btn_changeDuty_Btn(GameObject caster)
    {
        onClick_Btn_changeDuty_Btn( caster );
    }

    void _onClick_Btn_expel_Btn(GameObject caster)
    {
        onClick_Btn_expel_Btn( caster );
    }

    void _onClick_BtnClose_Btn(GameObject caster)
    {
        onClick_BtnClose_Btn( caster );
    }


}
