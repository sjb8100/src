//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TeamInvitePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_close;

    UILabel              m_label_TitleLabel;

    UIButton             m_btn_friend;

    UIButton             m_btn_clan;

    UIButton             m_btn_near;

    Transform            m_trans_scrollView;

    Transform            m_trans_GridRoot;

    UISprite             m_sprite_UITeamInviteGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_TitleLabel = fastComponent.FastGetComponent<UILabel>("TitleLabel");
       if( null == m_label_TitleLabel )
       {
            Engine.Utility.Log.Error("m_label_TitleLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_friend = fastComponent.FastGetComponent<UIButton>("friend");
       if( null == m_btn_friend )
       {
            Engine.Utility.Log.Error("m_btn_friend 为空，请检查prefab是否缺乏组件");
       }
        m_btn_clan = fastComponent.FastGetComponent<UIButton>("clan");
       if( null == m_btn_clan )
       {
            Engine.Utility.Log.Error("m_btn_clan 为空，请检查prefab是否缺乏组件");
       }
        m_btn_near = fastComponent.FastGetComponent<UIButton>("near");
       if( null == m_btn_near )
       {
            Engine.Utility.Log.Error("m_btn_near 为空，请检查prefab是否缺乏组件");
       }
        m_trans_scrollView = fastComponent.FastGetComponent<Transform>("scrollView");
       if( null == m_trans_scrollView )
       {
            Engine.Utility.Log.Error("m_trans_scrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GridRoot = fastComponent.FastGetComponent<Transform>("GridRoot");
       if( null == m_trans_GridRoot )
       {
            Engine.Utility.Log.Error("m_trans_GridRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UITeamInviteGrid = fastComponent.FastGetComponent<UISprite>("UITeamInviteGrid");
       if( null == m_sprite_UITeamInviteGrid )
       {
            Engine.Utility.Log.Error("m_sprite_UITeamInviteGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
        UIEventListener.Get(m_btn_friend.gameObject).onClick = _onClick_Friend_Btn;
        UIEventListener.Get(m_btn_clan.gameObject).onClick = _onClick_Clan_Btn;
        UIEventListener.Get(m_btn_near.gameObject).onClick = _onClick_Near_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_Friend_Btn(GameObject caster)
    {
        onClick_Friend_Btn( caster );
    }

    void _onClick_Clan_Btn(GameObject caster)
    {
        onClick_Clan_Btn( caster );
    }

    void _onClick_Near_Btn(GameObject caster)
    {
        onClick_Near_Btn( caster );
    }


}
