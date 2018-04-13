//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CampWarResultPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_win;

    Transform            m_trans_defeat;

    UISprite             m_sprite_ResultBg;

    UILabel              m_label_scoreNum_label;

    Transform            m_trans_CampWarContent;

    UILabel              m_label_GodDemonExp;

    UILabel              m_label_CampIntegral;

    UILabel              m_label_KillNum;

    UILabel              m_label_AssistNum;

    UILabel              m_label_DeadNum;

    UIButton             m_btn_btnclose;

    UIButton             m_btn_btnList;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_win = fastComponent.FastGetComponent<Transform>("win");
       if( null == m_trans_win )
       {
            Engine.Utility.Log.Error("m_trans_win 为空，请检查prefab是否缺乏组件");
       }
        m_trans_defeat = fastComponent.FastGetComponent<Transform>("defeat");
       if( null == m_trans_defeat )
       {
            Engine.Utility.Log.Error("m_trans_defeat 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_ResultBg = fastComponent.FastGetComponent<UISprite>("ResultBg");
       if( null == m_sprite_ResultBg )
       {
            Engine.Utility.Log.Error("m_sprite_ResultBg 为空，请检查prefab是否缺乏组件");
       }
        m_label_scoreNum_label = fastComponent.FastGetComponent<UILabel>("scoreNum_label");
       if( null == m_label_scoreNum_label )
       {
            Engine.Utility.Log.Error("m_label_scoreNum_label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CampWarContent = fastComponent.FastGetComponent<Transform>("CampWarContent");
       if( null == m_trans_CampWarContent )
       {
            Engine.Utility.Log.Error("m_trans_CampWarContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_GodDemonExp = fastComponent.FastGetComponent<UILabel>("GodDemonExp");
       if( null == m_label_GodDemonExp )
       {
            Engine.Utility.Log.Error("m_label_GodDemonExp 为空，请检查prefab是否缺乏组件");
       }
        m_label_CampIntegral = fastComponent.FastGetComponent<UILabel>("CampIntegral");
       if( null == m_label_CampIntegral )
       {
            Engine.Utility.Log.Error("m_label_CampIntegral 为空，请检查prefab是否缺乏组件");
       }
        m_label_KillNum = fastComponent.FastGetComponent<UILabel>("KillNum");
       if( null == m_label_KillNum )
       {
            Engine.Utility.Log.Error("m_label_KillNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_AssistNum = fastComponent.FastGetComponent<UILabel>("AssistNum");
       if( null == m_label_AssistNum )
       {
            Engine.Utility.Log.Error("m_label_AssistNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_DeadNum = fastComponent.FastGetComponent<UILabel>("DeadNum");
       if( null == m_label_DeadNum )
       {
            Engine.Utility.Log.Error("m_label_DeadNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnclose = fastComponent.FastGetComponent<UIButton>("btnclose");
       if( null == m_btn_btnclose )
       {
            Engine.Utility.Log.Error("m_btn_btnclose 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnList = fastComponent.FastGetComponent<UIButton>("btnList");
       if( null == m_btn_btnList )
       {
            Engine.Utility.Log.Error("m_btn_btnList 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btnclose.gameObject).onClick = _onClick_Btnclose_Btn;
        UIEventListener.Get(m_btn_btnList.gameObject).onClick = _onClick_BtnList_Btn;
    }

    void _onClick_Btnclose_Btn(GameObject caster)
    {
        onClick_Btnclose_Btn( caster );
    }

    void _onClick_BtnList_Btn(GameObject caster)
    {
        onClick_BtnList_Btn( caster );
    }


}
