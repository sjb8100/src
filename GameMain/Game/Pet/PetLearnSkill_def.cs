//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetLearnSkill: UIPanelBase
{

   FastComponent         fastComponent;

    UIGridCreatorBase    m_ctor_skill_scrollview;

    UIButton             m_btn_skill_item;

    Transform            m_trans_tuijian;

    Transform            m_trans_zhudong;

    Transform            m_trans_shoudong;

    Transform            m_trans_beidong;

    UILabel              m_label_petlevel;

    UILabel              m_label_petshowname;

    UITexture            m__peticon;

    UIWidget             m_widget_SkillbtnContainer;

    UILabel              m_label_yixuejineng;

    UIButton             m_btn_jinengsuoding;

    UILabel              m_label_jinengsuoding_number;

    UIButton             m_btn_suodingqueding;

    UIButton             m_btn_shengji;

    UILabel              m_label_xuejineng_Skillname;

    UILabel              m_label_xuejineng_SkillLevel;

    UISprite             m_sprite_bg;

    UITexture            m__skilldes_xuejineng_icon;

    UILabel              m_label_xuejineng_NowLevel;

    UISprite             m_sprite_xuejineng_icon;

    UILabel              m_label_xuejineng_name;

    UILabel              m_label_xuejineng_number;

    UILabel              m_label_xuejineng_goldxiaohao;

    UIButton             m_btn_xuejineng_zidongbuzu;

    UILabel              m_label_xuejineng_dianjuanxiaohao;

    UISprite             m_sprite_suoding_xiaohaoicon;

    UILabel              m_label_suoding_xiaohao;

    UILabel              m_label_skilltype;

    UIButton             m_btn_PetskillUItips_1;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_ctor_skill_scrollview = fastComponent.FastGetComponent<UIGridCreatorBase>("skill_scrollview");
       if( null == m_ctor_skill_scrollview )
       {
            Engine.Utility.Log.Error("m_ctor_skill_scrollview 为空，请检查prefab是否缺乏组件");
       }
        m_btn_skill_item = fastComponent.FastGetComponent<UIButton>("skill_item");
       if( null == m_btn_skill_item )
       {
            Engine.Utility.Log.Error("m_btn_skill_item 为空，请检查prefab是否缺乏组件");
       }
        m_trans_tuijian = fastComponent.FastGetComponent<Transform>("tuijian");
       if( null == m_trans_tuijian )
       {
            Engine.Utility.Log.Error("m_trans_tuijian 为空，请检查prefab是否缺乏组件");
       }
        m_trans_zhudong = fastComponent.FastGetComponent<Transform>("zhudong");
       if( null == m_trans_zhudong )
       {
            Engine.Utility.Log.Error("m_trans_zhudong 为空，请检查prefab是否缺乏组件");
       }
        m_trans_shoudong = fastComponent.FastGetComponent<Transform>("shoudong");
       if( null == m_trans_shoudong )
       {
            Engine.Utility.Log.Error("m_trans_shoudong 为空，请检查prefab是否缺乏组件");
       }
        m_trans_beidong = fastComponent.FastGetComponent<Transform>("beidong");
       if( null == m_trans_beidong )
       {
            Engine.Utility.Log.Error("m_trans_beidong 为空，请检查prefab是否缺乏组件");
       }
        m_label_petlevel = fastComponent.FastGetComponent<UILabel>("petlevel");
       if( null == m_label_petlevel )
       {
            Engine.Utility.Log.Error("m_label_petlevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_petshowname = fastComponent.FastGetComponent<UILabel>("petshowname");
       if( null == m_label_petshowname )
       {
            Engine.Utility.Log.Error("m_label_petshowname 为空，请检查prefab是否缺乏组件");
       }
        m__peticon = fastComponent.FastGetComponent<UITexture>("peticon");
       if( null == m__peticon )
       {
            Engine.Utility.Log.Error("m__peticon 为空，请检查prefab是否缺乏组件");
       }
        m_widget_SkillbtnContainer = fastComponent.FastGetComponent<UIWidget>("SkillbtnContainer");
       if( null == m_widget_SkillbtnContainer )
       {
            Engine.Utility.Log.Error("m_widget_SkillbtnContainer 为空，请检查prefab是否缺乏组件");
       }
        m_label_yixuejineng = fastComponent.FastGetComponent<UILabel>("yixuejineng");
       if( null == m_label_yixuejineng )
       {
            Engine.Utility.Log.Error("m_label_yixuejineng 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jinengsuoding = fastComponent.FastGetComponent<UIButton>("jinengsuoding");
       if( null == m_btn_jinengsuoding )
       {
            Engine.Utility.Log.Error("m_btn_jinengsuoding 为空，请检查prefab是否缺乏组件");
       }
        m_label_jinengsuoding_number = fastComponent.FastGetComponent<UILabel>("jinengsuoding_number");
       if( null == m_label_jinengsuoding_number )
       {
            Engine.Utility.Log.Error("m_label_jinengsuoding_number 为空，请检查prefab是否缺乏组件");
       }
        m_btn_suodingqueding = fastComponent.FastGetComponent<UIButton>("suodingqueding");
       if( null == m_btn_suodingqueding )
       {
            Engine.Utility.Log.Error("m_btn_suodingqueding 为空，请检查prefab是否缺乏组件");
       }
        m_btn_shengji = fastComponent.FastGetComponent<UIButton>("shengji");
       if( null == m_btn_shengji )
       {
            Engine.Utility.Log.Error("m_btn_shengji 为空，请检查prefab是否缺乏组件");
       }
        m_label_xuejineng_Skillname = fastComponent.FastGetComponent<UILabel>("xuejineng_Skillname");
       if( null == m_label_xuejineng_Skillname )
       {
            Engine.Utility.Log.Error("m_label_xuejineng_Skillname 为空，请检查prefab是否缺乏组件");
       }
        m_label_xuejineng_SkillLevel = fastComponent.FastGetComponent<UILabel>("xuejineng_SkillLevel");
       if( null == m_label_xuejineng_SkillLevel )
       {
            Engine.Utility.Log.Error("m_label_xuejineng_SkillLevel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_bg = fastComponent.FastGetComponent<UISprite>("bg");
       if( null == m_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
        m__skilldes_xuejineng_icon = fastComponent.FastGetComponent<UITexture>("skilldes_xuejineng_icon");
       if( null == m__skilldes_xuejineng_icon )
       {
            Engine.Utility.Log.Error("m__skilldes_xuejineng_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_xuejineng_NowLevel = fastComponent.FastGetComponent<UILabel>("xuejineng_NowLevel");
       if( null == m_label_xuejineng_NowLevel )
       {
            Engine.Utility.Log.Error("m_label_xuejineng_NowLevel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_xuejineng_icon = fastComponent.FastGetComponent<UISprite>("xuejineng_icon");
       if( null == m_sprite_xuejineng_icon )
       {
            Engine.Utility.Log.Error("m_sprite_xuejineng_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_xuejineng_name = fastComponent.FastGetComponent<UILabel>("xuejineng_name");
       if( null == m_label_xuejineng_name )
       {
            Engine.Utility.Log.Error("m_label_xuejineng_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_xuejineng_number = fastComponent.FastGetComponent<UILabel>("xuejineng_number");
       if( null == m_label_xuejineng_number )
       {
            Engine.Utility.Log.Error("m_label_xuejineng_number 为空，请检查prefab是否缺乏组件");
       }
        m_label_xuejineng_goldxiaohao = fastComponent.FastGetComponent<UILabel>("xuejineng_goldxiaohao");
       if( null == m_label_xuejineng_goldxiaohao )
       {
            Engine.Utility.Log.Error("m_label_xuejineng_goldxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_xuejineng_zidongbuzu = fastComponent.FastGetComponent<UIButton>("xuejineng_zidongbuzu");
       if( null == m_btn_xuejineng_zidongbuzu )
       {
            Engine.Utility.Log.Error("m_btn_xuejineng_zidongbuzu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xuejineng_dianjuanxiaohao = fastComponent.FastGetComponent<UILabel>("xuejineng_dianjuanxiaohao");
       if( null == m_label_xuejineng_dianjuanxiaohao )
       {
            Engine.Utility.Log.Error("m_label_xuejineng_dianjuanxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_suoding_xiaohaoicon = fastComponent.FastGetComponent<UISprite>("suoding_xiaohaoicon");
       if( null == m_sprite_suoding_xiaohaoicon )
       {
            Engine.Utility.Log.Error("m_sprite_suoding_xiaohaoicon 为空，请检查prefab是否缺乏组件");
       }
        m_label_suoding_xiaohao = fastComponent.FastGetComponent<UILabel>("suoding_xiaohao");
       if( null == m_label_suoding_xiaohao )
       {
            Engine.Utility.Log.Error("m_label_suoding_xiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_label_skilltype = fastComponent.FastGetComponent<UILabel>("skilltype");
       if( null == m_label_skilltype )
       {
            Engine.Utility.Log.Error("m_label_skilltype 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PetskillUItips_1 = fastComponent.FastGetComponent<UIButton>("PetskillUItips_1");
       if( null == m_btn_PetskillUItips_1 )
       {
            Engine.Utility.Log.Error("m_btn_PetskillUItips_1 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_skill_item.gameObject).onClick = _onClick_Skill_item_Btn;
        UIEventListener.Get(m_btn_jinengsuoding.gameObject).onClick = _onClick_Jinengsuoding_Btn;
        UIEventListener.Get(m_btn_suodingqueding.gameObject).onClick = _onClick_Suodingqueding_Btn;
        UIEventListener.Get(m_btn_shengji.gameObject).onClick = _onClick_Shengji_Btn;
        UIEventListener.Get(m_btn_xuejineng_zidongbuzu.gameObject).onClick = _onClick_Xuejineng_zidongbuzu_Btn;
        UIEventListener.Get(m_btn_PetskillUItips_1.gameObject).onClick = _onClick_PetskillUItips_1_Btn;
    }

    void _onClick_Skill_item_Btn(GameObject caster)
    {
        onClick_Skill_item_Btn( caster );
    }

    void _onClick_Jinengsuoding_Btn(GameObject caster)
    {
        onClick_Jinengsuoding_Btn( caster );
    }

    void _onClick_Suodingqueding_Btn(GameObject caster)
    {
        onClick_Suodingqueding_Btn( caster );
    }

    void _onClick_Shengji_Btn(GameObject caster)
    {
        onClick_Shengji_Btn( caster );
    }

    void _onClick_Xuejineng_zidongbuzu_Btn(GameObject caster)
    {
        onClick_Xuejineng_zidongbuzu_Btn( caster );
    }

    void _onClick_PetskillUItips_1_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }


}
