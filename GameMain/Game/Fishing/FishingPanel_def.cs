//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FishingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_FishingContent;

    UIButton             m_btn_StartFishingBtn;

    UISprite             m_sprite_StartFishingBtnBg;

    Transform            m_trans_PointerContent;

    UISprite             m_sprite_Pointer;

    UISprite             m_sprite_PointerBg1;

    UISprite             m_sprite_PointerBg2;

    Transform            m_trans_FishTimeContent;

    UISlider             m_slider_FishTimeBg;

    UISprite             m_sprite_FishTimeSlider;

    UIButton             m_btn_ExitFishingBtn;

    UIButton             m_btn_rank_btn;

    UILabel              m_label_score_num;

    UILabel              m_label_rank_num;

    Transform            m_trans_Center;

    UILabel              m_label_fishname_label;

    UITexture            m__icon;

    UISprite             m_sprite_qualitybox;

    UILabel              m_label_score_label;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_FishingContent = fastComponent.FastGetComponent<Transform>("FishingContent");
       if( null == m_trans_FishingContent )
       {
            Engine.Utility.Log.Error("m_trans_FishingContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_StartFishingBtn = fastComponent.FastGetComponent<UIButton>("StartFishingBtn");
       if( null == m_btn_StartFishingBtn )
       {
            Engine.Utility.Log.Error("m_btn_StartFishingBtn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_StartFishingBtnBg = fastComponent.FastGetComponent<UISprite>("StartFishingBtnBg");
       if( null == m_sprite_StartFishingBtnBg )
       {
            Engine.Utility.Log.Error("m_sprite_StartFishingBtnBg 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PointerContent = fastComponent.FastGetComponent<Transform>("PointerContent");
       if( null == m_trans_PointerContent )
       {
            Engine.Utility.Log.Error("m_trans_PointerContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Pointer = fastComponent.FastGetComponent<UISprite>("Pointer");
       if( null == m_sprite_Pointer )
       {
            Engine.Utility.Log.Error("m_sprite_Pointer 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_PointerBg1 = fastComponent.FastGetComponent<UISprite>("PointerBg1");
       if( null == m_sprite_PointerBg1 )
       {
            Engine.Utility.Log.Error("m_sprite_PointerBg1 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_PointerBg2 = fastComponent.FastGetComponent<UISprite>("PointerBg2");
       if( null == m_sprite_PointerBg2 )
       {
            Engine.Utility.Log.Error("m_sprite_PointerBg2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FishTimeContent = fastComponent.FastGetComponent<Transform>("FishTimeContent");
       if( null == m_trans_FishTimeContent )
       {
            Engine.Utility.Log.Error("m_trans_FishTimeContent 为空，请检查prefab是否缺乏组件");
       }
        m_slider_FishTimeBg = fastComponent.FastGetComponent<UISlider>("FishTimeBg");
       if( null == m_slider_FishTimeBg )
       {
            Engine.Utility.Log.Error("m_slider_FishTimeBg 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_FishTimeSlider = fastComponent.FastGetComponent<UISprite>("FishTimeSlider");
       if( null == m_sprite_FishTimeSlider )
       {
            Engine.Utility.Log.Error("m_sprite_FishTimeSlider 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ExitFishingBtn = fastComponent.FastGetComponent<UIButton>("ExitFishingBtn");
       if( null == m_btn_ExitFishingBtn )
       {
            Engine.Utility.Log.Error("m_btn_ExitFishingBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_rank_btn = fastComponent.FastGetComponent<UIButton>("rank_btn");
       if( null == m_btn_rank_btn )
       {
            Engine.Utility.Log.Error("m_btn_rank_btn 为空，请检查prefab是否缺乏组件");
       }
        m_label_score_num = fastComponent.FastGetComponent<UILabel>("score_num");
       if( null == m_label_score_num )
       {
            Engine.Utility.Log.Error("m_label_score_num 为空，请检查prefab是否缺乏组件");
       }
        m_label_rank_num = fastComponent.FastGetComponent<UILabel>("rank_num");
       if( null == m_label_rank_num )
       {
            Engine.Utility.Log.Error("m_label_rank_num 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Center = fastComponent.FastGetComponent<Transform>("Center");
       if( null == m_trans_Center )
       {
            Engine.Utility.Log.Error("m_trans_Center 为空，请检查prefab是否缺乏组件");
       }
        m_label_fishname_label = fastComponent.FastGetComponent<UILabel>("fishname_label");
       if( null == m_label_fishname_label )
       {
            Engine.Utility.Log.Error("m_label_fishname_label 为空，请检查prefab是否缺乏组件");
       }
        m__icon = fastComponent.FastGetComponent<UITexture>("icon");
       if( null == m__icon )
       {
            Engine.Utility.Log.Error("m__icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_qualitybox = fastComponent.FastGetComponent<UISprite>("qualitybox");
       if( null == m_sprite_qualitybox )
       {
            Engine.Utility.Log.Error("m_sprite_qualitybox 为空，请检查prefab是否缺乏组件");
       }
        m_label_score_label = fastComponent.FastGetComponent<UILabel>("score_label");
       if( null == m_label_score_label )
       {
            Engine.Utility.Log.Error("m_label_score_label 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_StartFishingBtn.gameObject).onClick = _onClick_StartFishingBtn_Btn;
        UIEventListener.Get(m_btn_ExitFishingBtn.gameObject).onClick = _onClick_ExitFishingBtn_Btn;
        UIEventListener.Get(m_btn_rank_btn.gameObject).onClick = _onClick_Rank_btn_Btn;
    }

    void _onClick_StartFishingBtn_Btn(GameObject caster)
    {
        onClick_StartFishingBtn_Btn( caster );
    }

    void _onClick_ExitFishingBtn_Btn(GameObject caster)
    {
        onClick_ExitFishingBtn_Btn( caster );
    }

    void _onClick_Rank_btn_Btn(GameObject caster)
    {
        onClick_Rank_btn_Btn( caster );
    }


}
