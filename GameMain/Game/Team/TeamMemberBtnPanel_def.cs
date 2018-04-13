//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TeamMemberBtnPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_close;

    Transform            m_trans_content;

    UISprite             m_sprite_bg;

    UIButton             m_btn_btn_sendmessage;

    UIButton             m_btn_btn_lookmessage;

    UIButton             m_btn_btn_addfriend;

    UIButton             m_btn_btn_giveleader;

    UIButton             m_btn_btn_kickedoutteam;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_close = fastComponent.FastGetComponent<UIWidget>("close");
       if( null == m_widget_close )
       {
            Engine.Utility.Log.Error("m_widget_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_content = fastComponent.FastGetComponent<Transform>("content");
       if( null == m_trans_content )
       {
            Engine.Utility.Log.Error("m_trans_content 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_bg = fastComponent.FastGetComponent<UISprite>("bg");
       if( null == m_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_sendmessage = fastComponent.FastGetComponent<UIButton>("btn_sendmessage");
       if( null == m_btn_btn_sendmessage )
       {
            Engine.Utility.Log.Error("m_btn_btn_sendmessage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_lookmessage = fastComponent.FastGetComponent<UIButton>("btn_lookmessage");
       if( null == m_btn_btn_lookmessage )
       {
            Engine.Utility.Log.Error("m_btn_btn_lookmessage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_addfriend = fastComponent.FastGetComponent<UIButton>("btn_addfriend");
       if( null == m_btn_btn_addfriend )
       {
            Engine.Utility.Log.Error("m_btn_btn_addfriend 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_giveleader = fastComponent.FastGetComponent<UIButton>("btn_giveleader");
       if( null == m_btn_btn_giveleader )
       {
            Engine.Utility.Log.Error("m_btn_btn_giveleader 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_kickedoutteam = fastComponent.FastGetComponent<UIButton>("btn_kickedoutteam");
       if( null == m_btn_btn_kickedoutteam )
       {
            Engine.Utility.Log.Error("m_btn_btn_kickedoutteam 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_lookmessage.gameObject).onClick = _onClick_Btn_lookmessage_Btn;
        UIEventListener.Get(m_btn_btn_addfriend.gameObject).onClick = _onClick_Btn_addfriend_Btn;
        UIEventListener.Get(m_btn_btn_giveleader.gameObject).onClick = _onClick_Btn_giveleader_Btn;
        UIEventListener.Get(m_btn_btn_kickedoutteam.gameObject).onClick = _onClick_Btn_kickedoutteam_Btn;
    }

    void _onClick_Btn_sendmessage_Btn(GameObject caster)
    {
        onClick_Btn_sendmessage_Btn( caster );
    }

    void _onClick_Btn_lookmessage_Btn(GameObject caster)
    {
        onClick_Btn_lookmessage_Btn( caster );
    }

    void _onClick_Btn_addfriend_Btn(GameObject caster)
    {
        onClick_Btn_addfriend_Btn( caster );
    }

    void _onClick_Btn_giveleader_Btn(GameObject caster)
    {
        onClick_Btn_giveleader_Btn( caster );
    }

    void _onClick_Btn_kickedoutteam_Btn(GameObject caster)
    {
        onClick_Btn_kickedoutteam_Btn( caster );
    }


}
