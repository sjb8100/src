//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ActivityRewardPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_BiaoTi_Label;

    UILabel              m_label_text;

    UILabel              m_label_title;

    UILabel              m_label_description;

    UIButton             m_btn_close;

    UIGridCreatorBase    m_ctor_itemRoot;

    UIButton             m_btn_lingquBtn;

    UILabel              m_label_Label;

    UISprite             m_sprite_Status_Received;

    Transform            m_trans_UIItemRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_BiaoTi_Label = fastComponent.FastGetComponent<UILabel>("BiaoTi_Label");
       if( null == m_label_BiaoTi_Label )
       {
            Engine.Utility.Log.Error("m_label_BiaoTi_Label 为空，请检查prefab是否缺乏组件");
       }
        m_label_text = fastComponent.FastGetComponent<UILabel>("text");
       if( null == m_label_text )
       {
            Engine.Utility.Log.Error("m_label_text 为空，请检查prefab是否缺乏组件");
       }
        m_label_title = fastComponent.FastGetComponent<UILabel>("title");
       if( null == m_label_title )
       {
            Engine.Utility.Log.Error("m_label_title 为空，请检查prefab是否缺乏组件");
       }
        m_label_description = fastComponent.FastGetComponent<UILabel>("description");
       if( null == m_label_description )
       {
            Engine.Utility.Log.Error("m_label_description 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_itemRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("itemRoot");
       if( null == m_ctor_itemRoot )
       {
            Engine.Utility.Log.Error("m_ctor_itemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_lingquBtn = fastComponent.FastGetComponent<UIButton>("lingquBtn");
       if( null == m_btn_lingquBtn )
       {
            Engine.Utility.Log.Error("m_btn_lingquBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Status_Received = fastComponent.FastGetComponent<UISprite>("Status_Received");
       if( null == m_sprite_Status_Received )
       {
            Engine.Utility.Log.Error("m_sprite_Status_Received 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_lingquBtn.gameObject).onClick = _onClick_LingquBtn_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_LingquBtn_Btn(GameObject caster)
    {
        onClick_LingquBtn_Btn( caster );
    }


}
