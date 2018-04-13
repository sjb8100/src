//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class InvitePanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UILabel       m_label_tittle_name;

    UIButton      m_btn_btn_close;

    UISprite      m_sprite_icon;

    UILabel       m_label_level;

    UILabel       m_label_name;

    UIButton      m_btn_btn_invite;


    //初始化控件变量
    protected override void InitControls()
    {
        m_label_tittle_name = GetChildComponent<UILabel>("tittle_name");
       if( null == m_label_tittle_name )
       {
            Engine.Utility.Log.Error("m_label_tittle_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = GetChildComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_icon = GetChildComponent<UISprite>("icon");
       if( null == m_sprite_icon )
       {
            Engine.Utility.Log.Error("m_sprite_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_level = GetChildComponent<UILabel>("level");
       if( null == m_label_level )
       {
            Engine.Utility.Log.Error("m_label_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = GetChildComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_invite = GetChildComponent<UIButton>("btn_invite");
       if( null == m_btn_btn_invite )
       {
            Engine.Utility.Log.Error("m_btn_btn_invite 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
        UIEventListener.Get(m_btn_btn_invite.gameObject).onClick = _onClick_Btn_invite_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_Btn_invite_Btn(GameObject caster)
    {
        onClick_Btn_invite_Btn( caster );
    }


}
