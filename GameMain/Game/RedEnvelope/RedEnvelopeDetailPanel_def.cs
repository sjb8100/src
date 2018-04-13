//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RedEnvelopeDetailPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_btn_Close;

    UISprite             m_sprite_btn_take;

    UILabel              m_label_xiexielaoban;

    UITexture            m__icon;

    UILabel              m_label_playername_label;

    UILabel              m_label_text_label;

    UISprite             m_sprite_goldicon;

    UISprite             m_sprite_goldicon2;

    UILabel              m_label_procure_label;

    UILabel              m_label_procureNum_label;

    UILabel              m_label_take_label;

    UILabel              m_label_takeNum_label;

    UILabel              m_label_totalNum_label;

    UIGridCreatorBase    m_ctor_redEnvelopeDetailScrowView;

    Transform            m_trans_uiRedEnvelopeDetailGrid;

    UILabel              m_label_total_label;

    UILabel              m_label_best_label;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_btn_Close = fastComponent.FastGetComponent<UISprite>("btn_Close");
       if( null == m_sprite_btn_Close )
       {
            Engine.Utility.Log.Error("m_sprite_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btn_take = fastComponent.FastGetComponent<UISprite>("btn_take");
       if( null == m_sprite_btn_take )
       {
            Engine.Utility.Log.Error("m_sprite_btn_take 为空，请检查prefab是否缺乏组件");
       }
        m_label_xiexielaoban = fastComponent.FastGetComponent<UILabel>("xiexielaoban");
       if( null == m_label_xiexielaoban )
       {
            Engine.Utility.Log.Error("m_label_xiexielaoban 为空，请检查prefab是否缺乏组件");
       }
        m__icon = fastComponent.FastGetComponent<UITexture>("icon");
       if( null == m__icon )
       {
            Engine.Utility.Log.Error("m__icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_playername_label = fastComponent.FastGetComponent<UILabel>("playername_label");
       if( null == m_label_playername_label )
       {
            Engine.Utility.Log.Error("m_label_playername_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_text_label = fastComponent.FastGetComponent<UILabel>("text_label");
       if( null == m_label_text_label )
       {
            Engine.Utility.Log.Error("m_label_text_label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_goldicon = fastComponent.FastGetComponent<UISprite>("goldicon");
       if( null == m_sprite_goldicon )
       {
            Engine.Utility.Log.Error("m_sprite_goldicon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_goldicon2 = fastComponent.FastGetComponent<UISprite>("goldicon2");
       if( null == m_sprite_goldicon2 )
       {
            Engine.Utility.Log.Error("m_sprite_goldicon2 为空，请检查prefab是否缺乏组件");
       }
        m_label_procure_label = fastComponent.FastGetComponent<UILabel>("procure_label");
       if( null == m_label_procure_label )
       {
            Engine.Utility.Log.Error("m_label_procure_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_procureNum_label = fastComponent.FastGetComponent<UILabel>("procureNum_label");
       if( null == m_label_procureNum_label )
       {
            Engine.Utility.Log.Error("m_label_procureNum_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_take_label = fastComponent.FastGetComponent<UILabel>("take_label");
       if( null == m_label_take_label )
       {
            Engine.Utility.Log.Error("m_label_take_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_takeNum_label = fastComponent.FastGetComponent<UILabel>("takeNum_label");
       if( null == m_label_takeNum_label )
       {
            Engine.Utility.Log.Error("m_label_takeNum_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_totalNum_label = fastComponent.FastGetComponent<UILabel>("totalNum_label");
       if( null == m_label_totalNum_label )
       {
            Engine.Utility.Log.Error("m_label_totalNum_label 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_redEnvelopeDetailScrowView = fastComponent.FastGetComponent<UIGridCreatorBase>("redEnvelopeDetailScrowView");
       if( null == m_ctor_redEnvelopeDetailScrowView )
       {
            Engine.Utility.Log.Error("m_ctor_redEnvelopeDetailScrowView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_uiRedEnvelopeDetailGrid = fastComponent.FastGetComponent<Transform>("uiRedEnvelopeDetailGrid");
       if( null == m_trans_uiRedEnvelopeDetailGrid )
       {
            Engine.Utility.Log.Error("m_trans_uiRedEnvelopeDetailGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_total_label = fastComponent.FastGetComponent<UILabel>("total_label");
       if( null == m_label_total_label )
       {
            Engine.Utility.Log.Error("m_label_total_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_best_label = fastComponent.FastGetComponent<UILabel>("best_label");
       if( null == m_label_best_label )
       {
            Engine.Utility.Log.Error("m_label_best_label 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
