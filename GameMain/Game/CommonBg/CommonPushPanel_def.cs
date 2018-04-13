//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CommonPushPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_NormalRoot;

    UITexture            m__Icon;

    UISprite             m_sprite_qualityIcon;

    UILabel              m_label_Name;

    Transform            m_trans_EffectContent;

    UILabel              m_label_Effect;

    UILabel              m_label_power;

    UISprite             m_sprite_UpArrpw;

    UISprite             m_sprite_DownArrpw;

    UIButton             m_btn_BtnUse;

    UILabel              m_label_btnLable;

    UIButton             m_btn_BtnClose;

    Transform            m_trans_AchievementRoot;

    UILabel              m_label_AchievementName;

    UILabel              m_label_ClickLabel;

    UIButton             m_btn_AchievementClose;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_NormalRoot = fastComponent.FastGetComponent<Transform>("NormalRoot");
       if( null == m_trans_NormalRoot )
       {
            Engine.Utility.Log.Error("m_trans_NormalRoot 为空，请检查prefab是否缺乏组件");
       }
        m__Icon = fastComponent.FastGetComponent<UITexture>("Icon");
       if( null == m__Icon )
       {
            Engine.Utility.Log.Error("m__Icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_qualityIcon = fastComponent.FastGetComponent<UISprite>("qualityIcon");
       if( null == m_sprite_qualityIcon )
       {
            Engine.Utility.Log.Error("m_sprite_qualityIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_Name = fastComponent.FastGetComponent<UILabel>("Name");
       if( null == m_label_Name )
       {
            Engine.Utility.Log.Error("m_label_Name 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EffectContent = fastComponent.FastGetComponent<Transform>("EffectContent");
       if( null == m_trans_EffectContent )
       {
            Engine.Utility.Log.Error("m_trans_EffectContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_Effect = fastComponent.FastGetComponent<UILabel>("Effect");
       if( null == m_label_Effect )
       {
            Engine.Utility.Log.Error("m_label_Effect 为空，请检查prefab是否缺乏组件");
       }
        m_label_power = fastComponent.FastGetComponent<UILabel>("power");
       if( null == m_label_power )
       {
            Engine.Utility.Log.Error("m_label_power 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_UpArrpw = fastComponent.FastGetComponent<UISprite>("UpArrpw");
       if( null == m_sprite_UpArrpw )
       {
            Engine.Utility.Log.Error("m_sprite_UpArrpw 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_DownArrpw = fastComponent.FastGetComponent<UISprite>("DownArrpw");
       if( null == m_sprite_DownArrpw )
       {
            Engine.Utility.Log.Error("m_sprite_DownArrpw 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnUse = fastComponent.FastGetComponent<UIButton>("BtnUse");
       if( null == m_btn_BtnUse )
       {
            Engine.Utility.Log.Error("m_btn_BtnUse 为空，请检查prefab是否缺乏组件");
       }
        m_label_btnLable = fastComponent.FastGetComponent<UILabel>("btnLable");
       if( null == m_label_btnLable )
       {
            Engine.Utility.Log.Error("m_label_btnLable 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnClose = fastComponent.FastGetComponent<UIButton>("BtnClose");
       if( null == m_btn_BtnClose )
       {
            Engine.Utility.Log.Error("m_btn_BtnClose 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AchievementRoot = fastComponent.FastGetComponent<Transform>("AchievementRoot");
       if( null == m_trans_AchievementRoot )
       {
            Engine.Utility.Log.Error("m_trans_AchievementRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_AchievementName = fastComponent.FastGetComponent<UILabel>("AchievementName");
       if( null == m_label_AchievementName )
       {
            Engine.Utility.Log.Error("m_label_AchievementName 为空，请检查prefab是否缺乏组件");
       }
        m_label_ClickLabel = fastComponent.FastGetComponent<UILabel>("ClickLabel");
       if( null == m_label_ClickLabel )
       {
            Engine.Utility.Log.Error("m_label_ClickLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_AchievementClose = fastComponent.FastGetComponent<UIButton>("AchievementClose");
       if( null == m_btn_AchievementClose )
       {
            Engine.Utility.Log.Error("m_btn_AchievementClose 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BtnUse.gameObject).onClick = _onClick_BtnUse_Btn;
        UIEventListener.Get(m_btn_BtnClose.gameObject).onClick = _onClick_BtnClose_Btn;
        UIEventListener.Get(m_btn_AchievementClose.gameObject).onClick = _onClick_AchievementClose_Btn;
    }

    void _onClick_BtnUse_Btn(GameObject caster)
    {
        onClick_BtnUse_Btn( caster );
    }

    void _onClick_BtnClose_Btn(GameObject caster)
    {
        onClick_BtnClose_Btn( caster );
    }

    void _onClick_AchievementClose_Btn(GameObject caster)
    {
        onClick_AchievementClose_Btn( caster );
    }


}
