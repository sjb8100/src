//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class AnswerSuccessPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_ResultContent;

    UISprite             m_sprite_resultBg;

    UIButton             m_btn_resultBgCenter;

    UISprite             m_sprite_win;

    Transform            m_trans_effectRoot;

    UISprite             m_sprite_itemIcon;

    UILabel              m_label_itemCount;

    UILabel              m_label_right_Label;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_ResultContent = fastComponent.FastGetComponent<Transform>("ResultContent");
       if( null == m_trans_ResultContent )
       {
            Engine.Utility.Log.Error("m_trans_ResultContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_resultBg = fastComponent.FastGetComponent<UISprite>("resultBg");
       if( null == m_sprite_resultBg )
       {
            Engine.Utility.Log.Error("m_sprite_resultBg 为空，请检查prefab是否缺乏组件");
       }
        m_btn_resultBgCenter = fastComponent.FastGetComponent<UIButton>("resultBgCenter");
       if( null == m_btn_resultBgCenter )
       {
            Engine.Utility.Log.Error("m_btn_resultBgCenter 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_win = fastComponent.FastGetComponent<UISprite>("win");
       if( null == m_sprite_win )
       {
            Engine.Utility.Log.Error("m_sprite_win 为空，请检查prefab是否缺乏组件");
       }
        m_trans_effectRoot = fastComponent.FastGetComponent<Transform>("effectRoot");
       if( null == m_trans_effectRoot )
       {
            Engine.Utility.Log.Error("m_trans_effectRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_itemIcon = fastComponent.FastGetComponent<UISprite>("itemIcon");
       if( null == m_sprite_itemIcon )
       {
            Engine.Utility.Log.Error("m_sprite_itemIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_itemCount = fastComponent.FastGetComponent<UILabel>("itemCount");
       if( null == m_label_itemCount )
       {
            Engine.Utility.Log.Error("m_label_itemCount 为空，请检查prefab是否缺乏组件");
       }
        m_label_right_Label = fastComponent.FastGetComponent<UILabel>("right_Label");
       if( null == m_label_right_Label )
       {
            Engine.Utility.Log.Error("m_label_right_Label 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_resultBgCenter.gameObject).onClick = _onClick_ResultBgCenter_Btn;
    }

    void _onClick_ResultBgCenter_Btn(GameObject caster)
    {
        onClick_ResultBgCenter_Btn( caster );
    }


}
