//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class InviteFriendPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_Title;

    UIButton             m_btn_btn_close;

    UILabel              m_label_AlreadyNum;

    UILabel              m_label_label;

    Transform            m_trans_Content;

    UIGridCreatorBase    m_ctor_PlayListScroll;

    Transform            m_trans_NullInvitedContent;

    UISprite             m_sprite_UIWelfareInviteFriendGrid;

    UILabel              m_label_Name;

    UILabel              m_label_Profession;

    UILabel              m_label_Level;

    UILabel              m_label_RechargeNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_Title = fastComponent.FastGetComponent<UILabel>("Title");
       if( null == m_label_Title )
       {
            Engine.Utility.Log.Error("m_label_Title 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_AlreadyNum = fastComponent.FastGetComponent<UILabel>("AlreadyNum");
       if( null == m_label_AlreadyNum )
       {
            Engine.Utility.Log.Error("m_label_AlreadyNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_label = fastComponent.FastGetComponent<UILabel>("label");
       if( null == m_label_label )
       {
            Engine.Utility.Log.Error("m_label_label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Content = fastComponent.FastGetComponent<Transform>("Content");
       if( null == m_trans_Content )
       {
            Engine.Utility.Log.Error("m_trans_Content 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_PlayListScroll = fastComponent.FastGetComponent<UIGridCreatorBase>("PlayListScroll");
       if( null == m_ctor_PlayListScroll )
       {
            Engine.Utility.Log.Error("m_ctor_PlayListScroll 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NullInvitedContent = fastComponent.FastGetComponent<Transform>("NullInvitedContent");
       if( null == m_trans_NullInvitedContent )
       {
            Engine.Utility.Log.Error("m_trans_NullInvitedContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UIWelfareInviteFriendGrid = fastComponent.FastGetComponent<UISprite>("UIWelfareInviteFriendGrid");
       if( null == m_sprite_UIWelfareInviteFriendGrid )
       {
            Engine.Utility.Log.Error("m_sprite_UIWelfareInviteFriendGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_Name = fastComponent.FastGetComponent<UILabel>("Name");
       if( null == m_label_Name )
       {
            Engine.Utility.Log.Error("m_label_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_Profession = fastComponent.FastGetComponent<UILabel>("Profession");
       if( null == m_label_Profession )
       {
            Engine.Utility.Log.Error("m_label_Profession 为空，请检查prefab是否缺乏组件");
       }
        m_label_Level = fastComponent.FastGetComponent<UILabel>("Level");
       if( null == m_label_Level )
       {
            Engine.Utility.Log.Error("m_label_Level 为空，请检查prefab是否缺乏组件");
       }
        m_label_RechargeNum = fastComponent.FastGetComponent<UILabel>("RechargeNum");
       if( null == m_label_RechargeNum )
       {
            Engine.Utility.Log.Error("m_label_RechargeNum 为空，请检查prefab是否缺乏组件");
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
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }


}
