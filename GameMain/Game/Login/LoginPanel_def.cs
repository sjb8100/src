//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class LoginPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_version;

    Transform            m_trans_LoginContent;

    Transform            m_trans_loginAccount;

    UIButton             m_btn_btnLoginAcont;

    UIInput              m_input_accunt;

    UIButton             m_btn_btnNotice;

    UIButton             m_btn_btnAccount;

    UIButton             m_btn_btnRepare;

    UIButton             m_btn_btnFeedback;

    UIButton             m_btn_btnback;

    Transform            m_trans_loginServer;

    UISprite             m_sprite_ZoneInfoContent;

    UISpriteEx           m_spriteEx_status;

    UIButton             m_btn_btnServerList;

    UILabel              m_label_LabelServer;

    UIButton             m_btn_btnStartGame;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_version = fastComponent.FastGetComponent<UILabel>("version");
       if( null == m_label_version )
       {
            Engine.Utility.Log.Error("m_label_version 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LoginContent = fastComponent.FastGetComponent<Transform>("LoginContent");
       if( null == m_trans_LoginContent )
       {
            Engine.Utility.Log.Error("m_trans_LoginContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_loginAccount = fastComponent.FastGetComponent<Transform>("loginAccount");
       if( null == m_trans_loginAccount )
       {
            Engine.Utility.Log.Error("m_trans_loginAccount 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnLoginAcont = fastComponent.FastGetComponent<UIButton>("btnLoginAcont");
       if( null == m_btn_btnLoginAcont )
       {
            Engine.Utility.Log.Error("m_btn_btnLoginAcont 为空，请检查prefab是否缺乏组件");
       }
        m_input_accunt = fastComponent.FastGetComponent<UIInput>("accunt");
       if( null == m_input_accunt )
       {
            Engine.Utility.Log.Error("m_input_accunt 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnNotice = fastComponent.FastGetComponent<UIButton>("btnNotice");
       if( null == m_btn_btnNotice )
       {
            Engine.Utility.Log.Error("m_btn_btnNotice 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnAccount = fastComponent.FastGetComponent<UIButton>("btnAccount");
       if( null == m_btn_btnAccount )
       {
            Engine.Utility.Log.Error("m_btn_btnAccount 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnRepare = fastComponent.FastGetComponent<UIButton>("btnRepare");
       if( null == m_btn_btnRepare )
       {
            Engine.Utility.Log.Error("m_btn_btnRepare 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnFeedback = fastComponent.FastGetComponent<UIButton>("btnFeedback");
       if( null == m_btn_btnFeedback )
       {
            Engine.Utility.Log.Error("m_btn_btnFeedback 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnback = fastComponent.FastGetComponent<UIButton>("btnback");
       if( null == m_btn_btnback )
       {
            Engine.Utility.Log.Error("m_btn_btnback 为空，请检查prefab是否缺乏组件");
       }
        m_trans_loginServer = fastComponent.FastGetComponent<Transform>("loginServer");
       if( null == m_trans_loginServer )
       {
            Engine.Utility.Log.Error("m_trans_loginServer 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_ZoneInfoContent = fastComponent.FastGetComponent<UISprite>("ZoneInfoContent");
       if( null == m_sprite_ZoneInfoContent )
       {
            Engine.Utility.Log.Error("m_sprite_ZoneInfoContent 为空，请检查prefab是否缺乏组件");
       }
        m_spriteEx_status = fastComponent.FastGetComponent<UISpriteEx>("status");
       if( null == m_spriteEx_status )
       {
            Engine.Utility.Log.Error("m_spriteEx_status 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnServerList = fastComponent.FastGetComponent<UIButton>("btnServerList");
       if( null == m_btn_btnServerList )
       {
            Engine.Utility.Log.Error("m_btn_btnServerList 为空，请检查prefab是否缺乏组件");
       }
        m_label_LabelServer = fastComponent.FastGetComponent<UILabel>("LabelServer");
       if( null == m_label_LabelServer )
       {
            Engine.Utility.Log.Error("m_label_LabelServer 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnStartGame = fastComponent.FastGetComponent<UIButton>("btnStartGame");
       if( null == m_btn_btnStartGame )
       {
            Engine.Utility.Log.Error("m_btn_btnStartGame 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btnLoginAcont.gameObject).onClick = _onClick_BtnLoginAcont_Btn;
        UIEventListener.Get(m_btn_btnNotice.gameObject).onClick = _onClick_BtnNotice_Btn;
        UIEventListener.Get(m_btn_btnAccount.gameObject).onClick = _onClick_BtnAccount_Btn;
        UIEventListener.Get(m_btn_btnRepare.gameObject).onClick = _onClick_BtnRepare_Btn;
        UIEventListener.Get(m_btn_btnFeedback.gameObject).onClick = _onClick_BtnFeedback_Btn;
        UIEventListener.Get(m_btn_btnback.gameObject).onClick = _onClick_Btnback_Btn;
        UIEventListener.Get(m_btn_btnServerList.gameObject).onClick = _onClick_BtnServerList_Btn;
        UIEventListener.Get(m_btn_btnStartGame.gameObject).onClick = _onClick_BtnStartGame_Btn;
    }

    void _onClick_BtnLoginAcont_Btn(GameObject caster)
    {
        onClick_BtnLoginAcont_Btn( caster );
    }

    void _onClick_BtnNotice_Btn(GameObject caster)
    {
        onClick_BtnNotice_Btn( caster );
    }

    void _onClick_BtnAccount_Btn(GameObject caster)
    {
        onClick_BtnAccount_Btn( caster );
    }

    void _onClick_BtnRepare_Btn(GameObject caster)
    {
        onClick_BtnRepare_Btn( caster );
    }

    void _onClick_BtnFeedback_Btn(GameObject caster)
    {
        onClick_BtnFeedback_Btn( caster );
    }

    void _onClick_Btnback_Btn(GameObject caster)
    {
        onClick_Btnback_Btn( caster );
    }

    void _onClick_BtnServerList_Btn(GameObject caster)
    {
        onClick_BtnServerList_Btn( caster );
    }

    void _onClick_BtnStartGame_Btn(GameObject caster)
    {
        onClick_BtnStartGame_Btn( caster );
    }


}
