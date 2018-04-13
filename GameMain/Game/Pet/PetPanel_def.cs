//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//属性
		ShuXing = 1,
		//洗炼
		XiLian = 2,
		//传承
		ChuanCheng = 3,
		//修灵
		XiuLing = 4,
		//图鉴
		TuJian = 5,
		Max,
    }

   FastComponent         fastComponent;

    UIWidget             m_widget_left;

    UIWidget             m_widget_leftcontent;

    UIGridCreatorBase    m_ctor_petscrollview;

    UIButton             m_btn_buzhen;

    Transform            m_trans_NullPetTipsContent;

    Transform            m_trans_shuxingcontent;

    UILabel              m_label_PetMedicanNum;

    UITexture            m__PetMedicanIcon;

    UIWidget             m_widget_PetMessageShuXing;

    UILabel              m_label_PetFightPowerLabel;

    UIButton             m_btn_changename;

    UIWidget             m_widget_detailPropcontent;

    UILabel              m_label_Pro_liliang;

    UILabel              m_label_Pro_minjie;

    UILabel              m_label_Pro_zhili;

    UILabel              m_label_Pro_tili;

    UILabel              m_label_Pro_jingshen;

    UILabel              m_label_growstate;

    UILabel              m_label_liliangzhi;

    UILabel              m_label_minjiezhi;

    UILabel              m_label_zhilizhi;

    UILabel              m_label_tilizhi;

    UILabel              m_label_jingshenzhi;

    UILabel              m_label_wulimingzhong;

    UILabel              m_label_wulibaoji;

    UILabel              m_label_fashubaoji;

    UILabel              m_label_shanbi;

    UIWidget             m_widget_basePropcontent;

    UILabel              m_label_chongwutedian;

    UILabel              m_label_xiedaidengji;

    UILabel              m_label_chengzhangdengji;

    UILabel              m_label_xiuwei;

    UIButton             m_btn_shuxingfenpei;

    Transform            m_trans_shuxingPointWarring;

    UIButton             m_btn_jinengxuexi;

    Transform            m_trans_PointWarring;

    UILabel              m_label_qixue;

    UILabel              m_label_wugong;

    UILabel              m_label_wufang;

    UILabel              m_label_huofang;

    UILabel              m_label_fagong;

    UILabel              m_label_fafang;

    UILabel              m_label_bingfang;

    UILabel              m_label_dianfang;

    UILabel              m_label_anfang;

    UIWidget             m_widget_ShuxingSkillbtnContainer;

    UIButton             m_btn_xiuxi;

    UISlider             m_slider_expscorllbar;

    UILabel              m_label_exppetlevel;

    UISlider             m_slider_lifescorllbar;

    UIButton             m_btn_addexp;

    UIButton             m_btn_addlife;

    UIButton             m_btn_diuqi;

    UIButton             m_btn_chuzhan;

    UILabel              m_label_chuanzhancd;

    Transform            m_trans_BaseProp;

    Transform            m_trans_DetailProp;

    UIButton             m_btn_PetUItips_1;

    Transform            m_trans_xinguiyuanContent;

    Transform            m_trans_PtguiyuanContent;

    UIButton             m_btn_guiyuanCommon_wanmei;

    UIButton             m_btn_tingzhiguiyuan;

    Transform            m_trans_GjguiyuanContent;

    Transform            m_trans_GjMessageContent;

    Transform            m_trans_nothingshow;

    UILabel              m_label_guiyuannotingtext;

    Transform            m_trans_contentcontainer;

    Transform            m_trans_gaojiSliderContainer;

    UILabel              m_label_GjGuiyuanGrow;

    UILabel              m_label_gaojijiebian;

    UIButton             m_btn_BtnSaveTianfu;

    UISprite             m_sprite_titlebg;

    UILabel              m_label_gaojititletext;

    Transform            m_trans_CommonContent;

    Transform            m_trans_PtMessageContent;

    Transform            m_trans_putongSliderContainer;

    Transform            m_trans_PtGuiyuanGrowCotainer;

    UILabel              m_label_PtGuiyuanGrow;

    UILabel              m_label_putongjiebian;

    Transform            m_trans_zuidajiebian;

    UILabel              m_label_maxjiebian;

    UILabel              m_label_putongtitletext;

    Transform            m_trans_CmCostBottom;

    Transform            m_trans_AssistContentRoot;

    UITexture            m__guiyuanCommon_icon;

    UISprite             m_sprite_guiyuanCommon_icon_di;

    UILabel              m_label_guiyuanCommon_name;

    UILabel              m_label_guiyuanCommon_number;

    UILabel              m_label_guiyuanCommon_dianjuanxiaohao;

    UIButton             m_btn_guiyuanCommon_zidongbuzu;

    UILabel              m_label_guiyuanCommon_xiaohaogold;

    UIButton             m_btn_kaishiguiyuan;

    UITexture            m__guiyuantopicon;

    UIWidget             m_widget_guiyuantips;

    UIButton             m_btn_PetUItips_2;

    Transform            m_trans_ChuanChengContent;

    Transform            m_trans_oldObj;

    UIButton             m_btn_btn_Old_delete;

    UILabel              m_label_Old_level_Before;

    UILabel              m_label_Old_level_After;

    UILabel              m_label_Old_skill_Before;

    UILabel              m_label_Old_skill_After;

    UILabel              m_label_Old_xiuwei_Before;

    UILabel              m_label_Old_xiuwei_After;

    UILabel              m_label_Old_name;

    UITexture            m__Old_icon;

    UISprite             m_sprite_OldIconBox;

    UIWidget             m_widget_old_select;

    Transform            m_trans_newObj;

    UIButton             m_btn_btn_New_delete;

    UILabel              m_label_New_xiuwei_Before;

    UILabel              m_label_New_xiuwei_After;

    UILabel              m_label_New_skill_Before;

    UILabel              m_label_New_skill_After;

    UILabel              m_label_New_level_Before;

    UILabel              m_label_New_level_After;

    UILabel              m_label_New_name;

    UITexture            m__New_icon;

    UISprite             m_sprite_NewIconBox;

    UIWidget             m_widget_new_select;

    UISprite             m_sprite_ptchuanchengjingyan;

    UISprite             m_sprite_inhertexpgou;

    UISprite             m_sprite_ptchuanchengjineng;

    UISprite             m_sprite_inheritskillgou;

    UISprite             m_sprite_ptchuanchengxiuwei;

    UISprite             m_sprite_inheritxiuweigou;

    Transform            m_trans_InheritCmCostBottom;

    Transform            m_trans_InheritAssistContentRoot;

    UITexture            m__ChuanChengCommon_icon;

    UISprite             m_sprite_ChuanChengCommon_icon_di;

    UILabel              m_label_ChuanChengCommon_name;

    UILabel              m_label_ChuanChengCommon_number;

    UILabel              m_label_ChuanChengCommon_dianjuanxiaohao;

    UIButton             m_btn_ChuanChengCommon_zidongbuzu;

    UILabel              m_label_ChuanChengCommon_xiaohaogold;

    UISprite             m_sprite_inheritMoneySpr;

    UIButton             m_btn_kaishiChuanCheng;

    UIButton             m_btn_PetUItips_3;

    Transform            m_trans_yinhuncontent;

    UISprite             m_sprite_Bg;

    UITexture            m__yinhuntopicon;

    UILabel              m_label_yuanadd_xiuwei_level;

    UILabel              m_label_xinadd_xiuwei_level;

    UILabel              m_label_yuanadd_liliangtianfu;

    UILabel              m_label_xinadd_liliangtianfu;

    UILabel              m_label_yuanadd_minjietianfu;

    UILabel              m_label_xinadd_minjietianfu;

    UILabel              m_label_yuanadd_zhilitianfu;

    UILabel              m_label_xinadd_zhilitianfu;

    UILabel              m_label_yuanadd_tilitianfu;

    UILabel              m_label_xinadd_tilitianfu;

    UILabel              m_label_yuanadd_jingshentianfu;

    UILabel              m_label_xinadd_jingshentianfu;

    UIWidget             m_widget_yinhun_max;

    Transform            m_trans_yuanxiuwei;

    Transform            m_trans_yuanxiuweiMessageContent;

    UILabel              m_label_yuan_xiuwei;

    UILabel              m_label_yuan_liliangtianfu;

    UILabel              m_label_yuan_minjietianfu;

    UILabel              m_label_yuan_zhilitianfu;

    UILabel              m_label_yuan_tilitianfu;

    UILabel              m_label_yuan_jingshentianfu;

    UILabel              m_label_yinhun_maxLabel;

    Transform            m_trans_xinxiuwei;

    Transform            m_trans_xinxiuweiContent;

    UILabel              m_label_xin_xiuwei;

    Transform            m_trans_xinxiuweicontentparent;

    UILabel              m_label_xin_liliangtianfu;

    UILabel              m_label_xin_minjietianfu;

    UILabel              m_label_xin_zhilitianfu;

    UILabel              m_label_xin_tilitianfu;

    UILabel              m_label_xin_jingshentianfu;

    UIWidget             m_widget_yinhun_nomax;

    Transform            m_trans_yinhunCostBottom;

    Transform            m_trans_AssistContentRootyinhun;

    UITexture            m__yinhun_icon;

    UISprite             m_sprite_yinhun_icon_di;

    UILabel              m_label_yinhun_xiaohaoname;

    UILabel              m_label_yinhun_xiaohaonumber;

    UIButton             m_btn_yinhun_xiaohaoSprite;

    UILabel              m_label_yinhun_dianjuanxiaohao;

    UILabel              m_label_yinhun_xiaohaogold;

    UIButton             m_btn_yinhunyici;

    UILabel              m_label_yinhunshici_xiaohaogold;

    UIButton             m_btn_yinhunshici;

    UISlider             m_slider_lingqizhi;

    UILabel              m_label_yinhunmanji;

    UIWidget             m_widget_xiuweishuxingtips;

    UIButton             m_btn_PetUItips_4;

    Transform            m_trans_jinengcontent;

    UILabel              m_label_jinengpetlevel;

    UILabel              m_label_jinengpetname;

    UITexture            m__jinengpeticon;

    UISprite             m_sprite_petpingzhi;

    UIWidget             m_widget_SkillbtnContainer;

    UIWidget             m_widget_SkillDescription;

    UILabel              m_label_Skillname;

    UILabel              m_label_SkillLevel;

    UILabel              m_label_SkillType;

    UILabel              m_label_NowLevel;

    UIWidget             m_widget_next;

    UILabel              m_label_NextLevel;

    UITexture            m__jineng_icon;

    UISprite             m_sprite_jineng_icon_di;

    UILabel              m_label_jineng_name;

    UILabel              m_label_jineng_number;

    UIButton             m_btn_jineng_Sprite;

    UILabel              m_label_jinengshengji_dianjuanxiaohao;

    UIButton             m_btn_skill_shengji;

    UILabel              m_label_skill_xiaohaogold;

    UILabel              m_label_skillfulltips;

    UIButton             m_btn_xuexijineng;

    UIButton             m_btn_Pettips2;

    Transform            m_trans_tujiancontent;

    Transform            m_trans_tujianbtns;

    UIGridCreatorBase    m_ctor_tujiantitlescroll;

    UISprite             m_sprite_tujiantitleItem;

    UIWidget             m_widget_PetTujianItem;

    UIGridCreatorBase    m_ctor_tujianscroll;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_left = fastComponent.FastGetComponent<UIWidget>("left");
       if( null == m_widget_left )
       {
            Engine.Utility.Log.Error("m_widget_left 为空，请检查prefab是否缺乏组件");
       }
        m_widget_leftcontent = fastComponent.FastGetComponent<UIWidget>("leftcontent");
       if( null == m_widget_leftcontent )
       {
            Engine.Utility.Log.Error("m_widget_leftcontent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_petscrollview = fastComponent.FastGetComponent<UIGridCreatorBase>("petscrollview");
       if( null == m_ctor_petscrollview )
       {
            Engine.Utility.Log.Error("m_ctor_petscrollview 为空，请检查prefab是否缺乏组件");
       }
        m_btn_buzhen = fastComponent.FastGetComponent<UIButton>("buzhen");
       if( null == m_btn_buzhen )
       {
            Engine.Utility.Log.Error("m_btn_buzhen 为空，请检查prefab是否缺乏组件");
       }
        m_trans_NullPetTipsContent = fastComponent.FastGetComponent<Transform>("NullPetTipsContent");
       if( null == m_trans_NullPetTipsContent )
       {
            Engine.Utility.Log.Error("m_trans_NullPetTipsContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_shuxingcontent = fastComponent.FastGetComponent<Transform>("shuxingcontent");
       if( null == m_trans_shuxingcontent )
       {
            Engine.Utility.Log.Error("m_trans_shuxingcontent 为空，请检查prefab是否缺乏组件");
       }
        m_label_PetMedicanNum = fastComponent.FastGetComponent<UILabel>("PetMedicanNum");
       if( null == m_label_PetMedicanNum )
       {
            Engine.Utility.Log.Error("m_label_PetMedicanNum 为空，请检查prefab是否缺乏组件");
       }
        m__PetMedicanIcon = fastComponent.FastGetComponent<UITexture>("PetMedicanIcon");
       if( null == m__PetMedicanIcon )
       {
            Engine.Utility.Log.Error("m__PetMedicanIcon 为空，请检查prefab是否缺乏组件");
       }
        m_widget_PetMessageShuXing = fastComponent.FastGetComponent<UIWidget>("PetMessageShuXing");
       if( null == m_widget_PetMessageShuXing )
       {
            Engine.Utility.Log.Error("m_widget_PetMessageShuXing 为空，请检查prefab是否缺乏组件");
       }
        m_label_PetFightPowerLabel = fastComponent.FastGetComponent<UILabel>("PetFightPowerLabel");
       if( null == m_label_PetFightPowerLabel )
       {
            Engine.Utility.Log.Error("m_label_PetFightPowerLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_changename = fastComponent.FastGetComponent<UIButton>("changename");
       if( null == m_btn_changename )
       {
            Engine.Utility.Log.Error("m_btn_changename 为空，请检查prefab是否缺乏组件");
       }
        m_widget_detailPropcontent = fastComponent.FastGetComponent<UIWidget>("detailPropcontent");
       if( null == m_widget_detailPropcontent )
       {
            Engine.Utility.Log.Error("m_widget_detailPropcontent 为空，请检查prefab是否缺乏组件");
       }
        m_label_Pro_liliang = fastComponent.FastGetComponent<UILabel>("Pro_liliang");
       if( null == m_label_Pro_liliang )
       {
            Engine.Utility.Log.Error("m_label_Pro_liliang 为空，请检查prefab是否缺乏组件");
       }
        m_label_Pro_minjie = fastComponent.FastGetComponent<UILabel>("Pro_minjie");
       if( null == m_label_Pro_minjie )
       {
            Engine.Utility.Log.Error("m_label_Pro_minjie 为空，请检查prefab是否缺乏组件");
       }
        m_label_Pro_zhili = fastComponent.FastGetComponent<UILabel>("Pro_zhili");
       if( null == m_label_Pro_zhili )
       {
            Engine.Utility.Log.Error("m_label_Pro_zhili 为空，请检查prefab是否缺乏组件");
       }
        m_label_Pro_tili = fastComponent.FastGetComponent<UILabel>("Pro_tili");
       if( null == m_label_Pro_tili )
       {
            Engine.Utility.Log.Error("m_label_Pro_tili 为空，请检查prefab是否缺乏组件");
       }
        m_label_Pro_jingshen = fastComponent.FastGetComponent<UILabel>("Pro_jingshen");
       if( null == m_label_Pro_jingshen )
       {
            Engine.Utility.Log.Error("m_label_Pro_jingshen 为空，请检查prefab是否缺乏组件");
       }
        m_label_growstate = fastComponent.FastGetComponent<UILabel>("growstate");
       if( null == m_label_growstate )
       {
            Engine.Utility.Log.Error("m_label_growstate 为空，请检查prefab是否缺乏组件");
       }
        m_label_liliangzhi = fastComponent.FastGetComponent<UILabel>("liliangzhi");
       if( null == m_label_liliangzhi )
       {
            Engine.Utility.Log.Error("m_label_liliangzhi 为空，请检查prefab是否缺乏组件");
       }
        m_label_minjiezhi = fastComponent.FastGetComponent<UILabel>("minjiezhi");
       if( null == m_label_minjiezhi )
       {
            Engine.Utility.Log.Error("m_label_minjiezhi 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhilizhi = fastComponent.FastGetComponent<UILabel>("zhilizhi");
       if( null == m_label_zhilizhi )
       {
            Engine.Utility.Log.Error("m_label_zhilizhi 为空，请检查prefab是否缺乏组件");
       }
        m_label_tilizhi = fastComponent.FastGetComponent<UILabel>("tilizhi");
       if( null == m_label_tilizhi )
       {
            Engine.Utility.Log.Error("m_label_tilizhi 为空，请检查prefab是否缺乏组件");
       }
        m_label_jingshenzhi = fastComponent.FastGetComponent<UILabel>("jingshenzhi");
       if( null == m_label_jingshenzhi )
       {
            Engine.Utility.Log.Error("m_label_jingshenzhi 为空，请检查prefab是否缺乏组件");
       }
        m_label_wulimingzhong = fastComponent.FastGetComponent<UILabel>("wulimingzhong");
       if( null == m_label_wulimingzhong )
       {
            Engine.Utility.Log.Error("m_label_wulimingzhong 为空，请检查prefab是否缺乏组件");
       }
        m_label_wulibaoji = fastComponent.FastGetComponent<UILabel>("wulibaoji");
       if( null == m_label_wulibaoji )
       {
            Engine.Utility.Log.Error("m_label_wulibaoji 为空，请检查prefab是否缺乏组件");
       }
        m_label_fashubaoji = fastComponent.FastGetComponent<UILabel>("fashubaoji");
       if( null == m_label_fashubaoji )
       {
            Engine.Utility.Log.Error("m_label_fashubaoji 为空，请检查prefab是否缺乏组件");
       }
        m_label_shanbi = fastComponent.FastGetComponent<UILabel>("shanbi");
       if( null == m_label_shanbi )
       {
            Engine.Utility.Log.Error("m_label_shanbi 为空，请检查prefab是否缺乏组件");
       }
        m_widget_basePropcontent = fastComponent.FastGetComponent<UIWidget>("basePropcontent");
       if( null == m_widget_basePropcontent )
       {
            Engine.Utility.Log.Error("m_widget_basePropcontent 为空，请检查prefab是否缺乏组件");
       }
        m_label_chongwutedian = fastComponent.FastGetComponent<UILabel>("chongwutedian");
       if( null == m_label_chongwutedian )
       {
            Engine.Utility.Log.Error("m_label_chongwutedian 为空，请检查prefab是否缺乏组件");
       }
        m_label_xiedaidengji = fastComponent.FastGetComponent<UILabel>("xiedaidengji");
       if( null == m_label_xiedaidengji )
       {
            Engine.Utility.Log.Error("m_label_xiedaidengji 为空，请检查prefab是否缺乏组件");
       }
        m_label_chengzhangdengji = fastComponent.FastGetComponent<UILabel>("chengzhangdengji");
       if( null == m_label_chengzhangdengji )
       {
            Engine.Utility.Log.Error("m_label_chengzhangdengji 为空，请检查prefab是否缺乏组件");
       }
        m_label_xiuwei = fastComponent.FastGetComponent<UILabel>("xiuwei");
       if( null == m_label_xiuwei )
       {
            Engine.Utility.Log.Error("m_label_xiuwei 为空，请检查prefab是否缺乏组件");
       }
        m_btn_shuxingfenpei = fastComponent.FastGetComponent<UIButton>("shuxingfenpei");
       if( null == m_btn_shuxingfenpei )
       {
            Engine.Utility.Log.Error("m_btn_shuxingfenpei 为空，请检查prefab是否缺乏组件");
       }
        m_trans_shuxingPointWarring = fastComponent.FastGetComponent<Transform>("shuxingPointWarring");
       if( null == m_trans_shuxingPointWarring )
       {
            Engine.Utility.Log.Error("m_trans_shuxingPointWarring 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jinengxuexi = fastComponent.FastGetComponent<UIButton>("jinengxuexi");
       if( null == m_btn_jinengxuexi )
       {
            Engine.Utility.Log.Error("m_btn_jinengxuexi 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PointWarring = fastComponent.FastGetComponent<Transform>("PointWarring");
       if( null == m_trans_PointWarring )
       {
            Engine.Utility.Log.Error("m_trans_PointWarring 为空，请检查prefab是否缺乏组件");
       }
        m_label_qixue = fastComponent.FastGetComponent<UILabel>("qixue");
       if( null == m_label_qixue )
       {
            Engine.Utility.Log.Error("m_label_qixue 为空，请检查prefab是否缺乏组件");
       }
        m_label_wugong = fastComponent.FastGetComponent<UILabel>("wugong");
       if( null == m_label_wugong )
       {
            Engine.Utility.Log.Error("m_label_wugong 为空，请检查prefab是否缺乏组件");
       }
        m_label_wufang = fastComponent.FastGetComponent<UILabel>("wufang");
       if( null == m_label_wufang )
       {
            Engine.Utility.Log.Error("m_label_wufang 为空，请检查prefab是否缺乏组件");
       }
        m_label_huofang = fastComponent.FastGetComponent<UILabel>("huofang");
       if( null == m_label_huofang )
       {
            Engine.Utility.Log.Error("m_label_huofang 为空，请检查prefab是否缺乏组件");
       }
        m_label_fagong = fastComponent.FastGetComponent<UILabel>("fagong");
       if( null == m_label_fagong )
       {
            Engine.Utility.Log.Error("m_label_fagong 为空，请检查prefab是否缺乏组件");
       }
        m_label_fafang = fastComponent.FastGetComponent<UILabel>("fafang");
       if( null == m_label_fafang )
       {
            Engine.Utility.Log.Error("m_label_fafang 为空，请检查prefab是否缺乏组件");
       }
        m_label_bingfang = fastComponent.FastGetComponent<UILabel>("bingfang");
       if( null == m_label_bingfang )
       {
            Engine.Utility.Log.Error("m_label_bingfang 为空，请检查prefab是否缺乏组件");
       }
        m_label_dianfang = fastComponent.FastGetComponent<UILabel>("dianfang");
       if( null == m_label_dianfang )
       {
            Engine.Utility.Log.Error("m_label_dianfang 为空，请检查prefab是否缺乏组件");
       }
        m_label_anfang = fastComponent.FastGetComponent<UILabel>("anfang");
       if( null == m_label_anfang )
       {
            Engine.Utility.Log.Error("m_label_anfang 为空，请检查prefab是否缺乏组件");
       }
        m_widget_ShuxingSkillbtnContainer = fastComponent.FastGetComponent<UIWidget>("ShuxingSkillbtnContainer");
       if( null == m_widget_ShuxingSkillbtnContainer )
       {
            Engine.Utility.Log.Error("m_widget_ShuxingSkillbtnContainer 为空，请检查prefab是否缺乏组件");
       }
        m_btn_xiuxi = fastComponent.FastGetComponent<UIButton>("xiuxi");
       if( null == m_btn_xiuxi )
       {
            Engine.Utility.Log.Error("m_btn_xiuxi 为空，请检查prefab是否缺乏组件");
       }
        m_slider_expscorllbar = fastComponent.FastGetComponent<UISlider>("expscorllbar");
       if( null == m_slider_expscorllbar )
       {
            Engine.Utility.Log.Error("m_slider_expscorllbar 为空，请检查prefab是否缺乏组件");
       }
        m_label_exppetlevel = fastComponent.FastGetComponent<UILabel>("exppetlevel");
       if( null == m_label_exppetlevel )
       {
            Engine.Utility.Log.Error("m_label_exppetlevel 为空，请检查prefab是否缺乏组件");
       }
        m_slider_lifescorllbar = fastComponent.FastGetComponent<UISlider>("lifescorllbar");
       if( null == m_slider_lifescorllbar )
       {
            Engine.Utility.Log.Error("m_slider_lifescorllbar 为空，请检查prefab是否缺乏组件");
       }
        m_btn_addexp = fastComponent.FastGetComponent<UIButton>("addexp");
       if( null == m_btn_addexp )
       {
            Engine.Utility.Log.Error("m_btn_addexp 为空，请检查prefab是否缺乏组件");
       }
        m_btn_addlife = fastComponent.FastGetComponent<UIButton>("addlife");
       if( null == m_btn_addlife )
       {
            Engine.Utility.Log.Error("m_btn_addlife 为空，请检查prefab是否缺乏组件");
       }
        m_btn_diuqi = fastComponent.FastGetComponent<UIButton>("diuqi");
       if( null == m_btn_diuqi )
       {
            Engine.Utility.Log.Error("m_btn_diuqi 为空，请检查prefab是否缺乏组件");
       }
        m_btn_chuzhan = fastComponent.FastGetComponent<UIButton>("chuzhan");
       if( null == m_btn_chuzhan )
       {
            Engine.Utility.Log.Error("m_btn_chuzhan 为空，请检查prefab是否缺乏组件");
       }
        m_label_chuanzhancd = fastComponent.FastGetComponent<UILabel>("chuanzhancd");
       if( null == m_label_chuanzhancd )
       {
            Engine.Utility.Log.Error("m_label_chuanzhancd 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BaseProp = fastComponent.FastGetComponent<Transform>("BaseProp");
       if( null == m_trans_BaseProp )
       {
            Engine.Utility.Log.Error("m_trans_BaseProp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailProp = fastComponent.FastGetComponent<Transform>("DetailProp");
       if( null == m_trans_DetailProp )
       {
            Engine.Utility.Log.Error("m_trans_DetailProp 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PetUItips_1 = fastComponent.FastGetComponent<UIButton>("PetUItips_1");
       if( null == m_btn_PetUItips_1 )
       {
            Engine.Utility.Log.Error("m_btn_PetUItips_1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_xinguiyuanContent = fastComponent.FastGetComponent<Transform>("xinguiyuanContent");
       if( null == m_trans_xinguiyuanContent )
       {
            Engine.Utility.Log.Error("m_trans_xinguiyuanContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PtguiyuanContent = fastComponent.FastGetComponent<Transform>("PtguiyuanContent");
       if( null == m_trans_PtguiyuanContent )
       {
            Engine.Utility.Log.Error("m_trans_PtguiyuanContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_guiyuanCommon_wanmei = fastComponent.FastGetComponent<UIButton>("guiyuanCommon_wanmei");
       if( null == m_btn_guiyuanCommon_wanmei )
       {
            Engine.Utility.Log.Error("m_btn_guiyuanCommon_wanmei 为空，请检查prefab是否缺乏组件");
       }
        m_btn_tingzhiguiyuan = fastComponent.FastGetComponent<UIButton>("tingzhiguiyuan");
       if( null == m_btn_tingzhiguiyuan )
       {
            Engine.Utility.Log.Error("m_btn_tingzhiguiyuan 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GjguiyuanContent = fastComponent.FastGetComponent<Transform>("GjguiyuanContent");
       if( null == m_trans_GjguiyuanContent )
       {
            Engine.Utility.Log.Error("m_trans_GjguiyuanContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GjMessageContent = fastComponent.FastGetComponent<Transform>("GjMessageContent");
       if( null == m_trans_GjMessageContent )
       {
            Engine.Utility.Log.Error("m_trans_GjMessageContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_nothingshow = fastComponent.FastGetComponent<Transform>("nothingshow");
       if( null == m_trans_nothingshow )
       {
            Engine.Utility.Log.Error("m_trans_nothingshow 为空，请检查prefab是否缺乏组件");
       }
        m_label_guiyuannotingtext = fastComponent.FastGetComponent<UILabel>("guiyuannotingtext");
       if( null == m_label_guiyuannotingtext )
       {
            Engine.Utility.Log.Error("m_label_guiyuannotingtext 为空，请检查prefab是否缺乏组件");
       }
        m_trans_contentcontainer = fastComponent.FastGetComponent<Transform>("contentcontainer");
       if( null == m_trans_contentcontainer )
       {
            Engine.Utility.Log.Error("m_trans_contentcontainer 为空，请检查prefab是否缺乏组件");
       }
        m_trans_gaojiSliderContainer = fastComponent.FastGetComponent<Transform>("gaojiSliderContainer");
       if( null == m_trans_gaojiSliderContainer )
       {
            Engine.Utility.Log.Error("m_trans_gaojiSliderContainer 为空，请检查prefab是否缺乏组件");
       }
        m_label_GjGuiyuanGrow = fastComponent.FastGetComponent<UILabel>("GjGuiyuanGrow");
       if( null == m_label_GjGuiyuanGrow )
       {
            Engine.Utility.Log.Error("m_label_GjGuiyuanGrow 为空，请检查prefab是否缺乏组件");
       }
        m_label_gaojijiebian = fastComponent.FastGetComponent<UILabel>("gaojijiebian");
       if( null == m_label_gaojijiebian )
       {
            Engine.Utility.Log.Error("m_label_gaojijiebian 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSaveTianfu = fastComponent.FastGetComponent<UIButton>("BtnSaveTianfu");
       if( null == m_btn_BtnSaveTianfu )
       {
            Engine.Utility.Log.Error("m_btn_BtnSaveTianfu 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_titlebg = fastComponent.FastGetComponent<UISprite>("titlebg");
       if( null == m_sprite_titlebg )
       {
            Engine.Utility.Log.Error("m_sprite_titlebg 为空，请检查prefab是否缺乏组件");
       }
        m_label_gaojititletext = fastComponent.FastGetComponent<UILabel>("gaojititletext");
       if( null == m_label_gaojititletext )
       {
            Engine.Utility.Log.Error("m_label_gaojititletext 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CommonContent = fastComponent.FastGetComponent<Transform>("CommonContent");
       if( null == m_trans_CommonContent )
       {
            Engine.Utility.Log.Error("m_trans_CommonContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PtMessageContent = fastComponent.FastGetComponent<Transform>("PtMessageContent");
       if( null == m_trans_PtMessageContent )
       {
            Engine.Utility.Log.Error("m_trans_PtMessageContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_putongSliderContainer = fastComponent.FastGetComponent<Transform>("putongSliderContainer");
       if( null == m_trans_putongSliderContainer )
       {
            Engine.Utility.Log.Error("m_trans_putongSliderContainer 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PtGuiyuanGrowCotainer = fastComponent.FastGetComponent<Transform>("PtGuiyuanGrowCotainer");
       if( null == m_trans_PtGuiyuanGrowCotainer )
       {
            Engine.Utility.Log.Error("m_trans_PtGuiyuanGrowCotainer 为空，请检查prefab是否缺乏组件");
       }
        m_label_PtGuiyuanGrow = fastComponent.FastGetComponent<UILabel>("PtGuiyuanGrow");
       if( null == m_label_PtGuiyuanGrow )
       {
            Engine.Utility.Log.Error("m_label_PtGuiyuanGrow 为空，请检查prefab是否缺乏组件");
       }
        m_label_putongjiebian = fastComponent.FastGetComponent<UILabel>("putongjiebian");
       if( null == m_label_putongjiebian )
       {
            Engine.Utility.Log.Error("m_label_putongjiebian 为空，请检查prefab是否缺乏组件");
       }
        m_trans_zuidajiebian = fastComponent.FastGetComponent<Transform>("zuidajiebian");
       if( null == m_trans_zuidajiebian )
       {
            Engine.Utility.Log.Error("m_trans_zuidajiebian 为空，请检查prefab是否缺乏组件");
       }
        m_label_maxjiebian = fastComponent.FastGetComponent<UILabel>("maxjiebian");
       if( null == m_label_maxjiebian )
       {
            Engine.Utility.Log.Error("m_label_maxjiebian 为空，请检查prefab是否缺乏组件");
       }
        m_label_putongtitletext = fastComponent.FastGetComponent<UILabel>("putongtitletext");
       if( null == m_label_putongtitletext )
       {
            Engine.Utility.Log.Error("m_label_putongtitletext 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CmCostBottom = fastComponent.FastGetComponent<Transform>("CmCostBottom");
       if( null == m_trans_CmCostBottom )
       {
            Engine.Utility.Log.Error("m_trans_CmCostBottom 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AssistContentRoot = fastComponent.FastGetComponent<Transform>("AssistContentRoot");
       if( null == m_trans_AssistContentRoot )
       {
            Engine.Utility.Log.Error("m_trans_AssistContentRoot 为空，请检查prefab是否缺乏组件");
       }
        m__guiyuanCommon_icon = fastComponent.FastGetComponent<UITexture>("guiyuanCommon_icon");
       if( null == m__guiyuanCommon_icon )
       {
            Engine.Utility.Log.Error("m__guiyuanCommon_icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_guiyuanCommon_icon_di = fastComponent.FastGetComponent<UISprite>("guiyuanCommon_icon_di");
       if( null == m_sprite_guiyuanCommon_icon_di )
       {
            Engine.Utility.Log.Error("m_sprite_guiyuanCommon_icon_di 为空，请检查prefab是否缺乏组件");
       }
        m_label_guiyuanCommon_name = fastComponent.FastGetComponent<UILabel>("guiyuanCommon_name");
       if( null == m_label_guiyuanCommon_name )
       {
            Engine.Utility.Log.Error("m_label_guiyuanCommon_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_guiyuanCommon_number = fastComponent.FastGetComponent<UILabel>("guiyuanCommon_number");
       if( null == m_label_guiyuanCommon_number )
       {
            Engine.Utility.Log.Error("m_label_guiyuanCommon_number 为空，请检查prefab是否缺乏组件");
       }
        m_label_guiyuanCommon_dianjuanxiaohao = fastComponent.FastGetComponent<UILabel>("guiyuanCommon_dianjuanxiaohao");
       if( null == m_label_guiyuanCommon_dianjuanxiaohao )
       {
            Engine.Utility.Log.Error("m_label_guiyuanCommon_dianjuanxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_guiyuanCommon_zidongbuzu = fastComponent.FastGetComponent<UIButton>("guiyuanCommon_zidongbuzu");
       if( null == m_btn_guiyuanCommon_zidongbuzu )
       {
            Engine.Utility.Log.Error("m_btn_guiyuanCommon_zidongbuzu 为空，请检查prefab是否缺乏组件");
       }
        m_label_guiyuanCommon_xiaohaogold = fastComponent.FastGetComponent<UILabel>("guiyuanCommon_xiaohaogold");
       if( null == m_label_guiyuanCommon_xiaohaogold )
       {
            Engine.Utility.Log.Error("m_label_guiyuanCommon_xiaohaogold 为空，请检查prefab是否缺乏组件");
       }
        m_btn_kaishiguiyuan = fastComponent.FastGetComponent<UIButton>("kaishiguiyuan");
       if( null == m_btn_kaishiguiyuan )
       {
            Engine.Utility.Log.Error("m_btn_kaishiguiyuan 为空，请检查prefab是否缺乏组件");
       }
        m__guiyuantopicon = fastComponent.FastGetComponent<UITexture>("guiyuantopicon");
       if( null == m__guiyuantopicon )
       {
            Engine.Utility.Log.Error("m__guiyuantopicon 为空，请检查prefab是否缺乏组件");
       }
        m_widget_guiyuantips = fastComponent.FastGetComponent<UIWidget>("guiyuantips");
       if( null == m_widget_guiyuantips )
       {
            Engine.Utility.Log.Error("m_widget_guiyuantips 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PetUItips_2 = fastComponent.FastGetComponent<UIButton>("PetUItips_2");
       if( null == m_btn_PetUItips_2 )
       {
            Engine.Utility.Log.Error("m_btn_PetUItips_2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ChuanChengContent = fastComponent.FastGetComponent<Transform>("ChuanChengContent");
       if( null == m_trans_ChuanChengContent )
       {
            Engine.Utility.Log.Error("m_trans_ChuanChengContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_oldObj = fastComponent.FastGetComponent<Transform>("oldObj");
       if( null == m_trans_oldObj )
       {
            Engine.Utility.Log.Error("m_trans_oldObj 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Old_delete = fastComponent.FastGetComponent<UIButton>("btn_Old_delete");
       if( null == m_btn_btn_Old_delete )
       {
            Engine.Utility.Log.Error("m_btn_btn_Old_delete 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_level_Before = fastComponent.FastGetComponent<UILabel>("Old_level_Before");
       if( null == m_label_Old_level_Before )
       {
            Engine.Utility.Log.Error("m_label_Old_level_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_level_After = fastComponent.FastGetComponent<UILabel>("Old_level_After");
       if( null == m_label_Old_level_After )
       {
            Engine.Utility.Log.Error("m_label_Old_level_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_skill_Before = fastComponent.FastGetComponent<UILabel>("Old_skill_Before");
       if( null == m_label_Old_skill_Before )
       {
            Engine.Utility.Log.Error("m_label_Old_skill_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_skill_After = fastComponent.FastGetComponent<UILabel>("Old_skill_After");
       if( null == m_label_Old_skill_After )
       {
            Engine.Utility.Log.Error("m_label_Old_skill_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_xiuwei_Before = fastComponent.FastGetComponent<UILabel>("Old_xiuwei_Before");
       if( null == m_label_Old_xiuwei_Before )
       {
            Engine.Utility.Log.Error("m_label_Old_xiuwei_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_xiuwei_After = fastComponent.FastGetComponent<UILabel>("Old_xiuwei_After");
       if( null == m_label_Old_xiuwei_After )
       {
            Engine.Utility.Log.Error("m_label_Old_xiuwei_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_Old_name = fastComponent.FastGetComponent<UILabel>("Old_name");
       if( null == m_label_Old_name )
       {
            Engine.Utility.Log.Error("m_label_Old_name 为空，请检查prefab是否缺乏组件");
       }
        m__Old_icon = fastComponent.FastGetComponent<UITexture>("Old_icon");
       if( null == m__Old_icon )
       {
            Engine.Utility.Log.Error("m__Old_icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_OldIconBox = fastComponent.FastGetComponent<UISprite>("OldIconBox");
       if( null == m_sprite_OldIconBox )
       {
            Engine.Utility.Log.Error("m_sprite_OldIconBox 为空，请检查prefab是否缺乏组件");
       }
        m_widget_old_select = fastComponent.FastGetComponent<UIWidget>("old_select");
       if( null == m_widget_old_select )
       {
            Engine.Utility.Log.Error("m_widget_old_select 为空，请检查prefab是否缺乏组件");
       }
        m_trans_newObj = fastComponent.FastGetComponent<Transform>("newObj");
       if( null == m_trans_newObj )
       {
            Engine.Utility.Log.Error("m_trans_newObj 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_New_delete = fastComponent.FastGetComponent<UIButton>("btn_New_delete");
       if( null == m_btn_btn_New_delete )
       {
            Engine.Utility.Log.Error("m_btn_btn_New_delete 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_xiuwei_Before = fastComponent.FastGetComponent<UILabel>("New_xiuwei_Before");
       if( null == m_label_New_xiuwei_Before )
       {
            Engine.Utility.Log.Error("m_label_New_xiuwei_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_xiuwei_After = fastComponent.FastGetComponent<UILabel>("New_xiuwei_After");
       if( null == m_label_New_xiuwei_After )
       {
            Engine.Utility.Log.Error("m_label_New_xiuwei_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_skill_Before = fastComponent.FastGetComponent<UILabel>("New_skill_Before");
       if( null == m_label_New_skill_Before )
       {
            Engine.Utility.Log.Error("m_label_New_skill_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_skill_After = fastComponent.FastGetComponent<UILabel>("New_skill_After");
       if( null == m_label_New_skill_After )
       {
            Engine.Utility.Log.Error("m_label_New_skill_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_level_Before = fastComponent.FastGetComponent<UILabel>("New_level_Before");
       if( null == m_label_New_level_Before )
       {
            Engine.Utility.Log.Error("m_label_New_level_Before 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_level_After = fastComponent.FastGetComponent<UILabel>("New_level_After");
       if( null == m_label_New_level_After )
       {
            Engine.Utility.Log.Error("m_label_New_level_After 为空，请检查prefab是否缺乏组件");
       }
        m_label_New_name = fastComponent.FastGetComponent<UILabel>("New_name");
       if( null == m_label_New_name )
       {
            Engine.Utility.Log.Error("m_label_New_name 为空，请检查prefab是否缺乏组件");
       }
        m__New_icon = fastComponent.FastGetComponent<UITexture>("New_icon");
       if( null == m__New_icon )
       {
            Engine.Utility.Log.Error("m__New_icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_NewIconBox = fastComponent.FastGetComponent<UISprite>("NewIconBox");
       if( null == m_sprite_NewIconBox )
       {
            Engine.Utility.Log.Error("m_sprite_NewIconBox 为空，请检查prefab是否缺乏组件");
       }
        m_widget_new_select = fastComponent.FastGetComponent<UIWidget>("new_select");
       if( null == m_widget_new_select )
       {
            Engine.Utility.Log.Error("m_widget_new_select 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_ptchuanchengjingyan = fastComponent.FastGetComponent<UISprite>("ptchuanchengjingyan");
       if( null == m_sprite_ptchuanchengjingyan )
       {
            Engine.Utility.Log.Error("m_sprite_ptchuanchengjingyan 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_inhertexpgou = fastComponent.FastGetComponent<UISprite>("inhertexpgou");
       if( null == m_sprite_inhertexpgou )
       {
            Engine.Utility.Log.Error("m_sprite_inhertexpgou 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_ptchuanchengjineng = fastComponent.FastGetComponent<UISprite>("ptchuanchengjineng");
       if( null == m_sprite_ptchuanchengjineng )
       {
            Engine.Utility.Log.Error("m_sprite_ptchuanchengjineng 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_inheritskillgou = fastComponent.FastGetComponent<UISprite>("inheritskillgou");
       if( null == m_sprite_inheritskillgou )
       {
            Engine.Utility.Log.Error("m_sprite_inheritskillgou 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_ptchuanchengxiuwei = fastComponent.FastGetComponent<UISprite>("ptchuanchengxiuwei");
       if( null == m_sprite_ptchuanchengxiuwei )
       {
            Engine.Utility.Log.Error("m_sprite_ptchuanchengxiuwei 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_inheritxiuweigou = fastComponent.FastGetComponent<UISprite>("inheritxiuweigou");
       if( null == m_sprite_inheritxiuweigou )
       {
            Engine.Utility.Log.Error("m_sprite_inheritxiuweigou 为空，请检查prefab是否缺乏组件");
       }
        m_trans_InheritCmCostBottom = fastComponent.FastGetComponent<Transform>("InheritCmCostBottom");
       if( null == m_trans_InheritCmCostBottom )
       {
            Engine.Utility.Log.Error("m_trans_InheritCmCostBottom 为空，请检查prefab是否缺乏组件");
       }
        m_trans_InheritAssistContentRoot = fastComponent.FastGetComponent<Transform>("InheritAssistContentRoot");
       if( null == m_trans_InheritAssistContentRoot )
       {
            Engine.Utility.Log.Error("m_trans_InheritAssistContentRoot 为空，请检查prefab是否缺乏组件");
       }
        m__ChuanChengCommon_icon = fastComponent.FastGetComponent<UITexture>("ChuanChengCommon_icon");
       if( null == m__ChuanChengCommon_icon )
       {
            Engine.Utility.Log.Error("m__ChuanChengCommon_icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_ChuanChengCommon_icon_di = fastComponent.FastGetComponent<UISprite>("ChuanChengCommon_icon_di");
       if( null == m_sprite_ChuanChengCommon_icon_di )
       {
            Engine.Utility.Log.Error("m_sprite_ChuanChengCommon_icon_di 为空，请检查prefab是否缺乏组件");
       }
        m_label_ChuanChengCommon_name = fastComponent.FastGetComponent<UILabel>("ChuanChengCommon_name");
       if( null == m_label_ChuanChengCommon_name )
       {
            Engine.Utility.Log.Error("m_label_ChuanChengCommon_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_ChuanChengCommon_number = fastComponent.FastGetComponent<UILabel>("ChuanChengCommon_number");
       if( null == m_label_ChuanChengCommon_number )
       {
            Engine.Utility.Log.Error("m_label_ChuanChengCommon_number 为空，请检查prefab是否缺乏组件");
       }
        m_label_ChuanChengCommon_dianjuanxiaohao = fastComponent.FastGetComponent<UILabel>("ChuanChengCommon_dianjuanxiaohao");
       if( null == m_label_ChuanChengCommon_dianjuanxiaohao )
       {
            Engine.Utility.Log.Error("m_label_ChuanChengCommon_dianjuanxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ChuanChengCommon_zidongbuzu = fastComponent.FastGetComponent<UIButton>("ChuanChengCommon_zidongbuzu");
       if( null == m_btn_ChuanChengCommon_zidongbuzu )
       {
            Engine.Utility.Log.Error("m_btn_ChuanChengCommon_zidongbuzu 为空，请检查prefab是否缺乏组件");
       }
        m_label_ChuanChengCommon_xiaohaogold = fastComponent.FastGetComponent<UILabel>("ChuanChengCommon_xiaohaogold");
       if( null == m_label_ChuanChengCommon_xiaohaogold )
       {
            Engine.Utility.Log.Error("m_label_ChuanChengCommon_xiaohaogold 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_inheritMoneySpr = fastComponent.FastGetComponent<UISprite>("inheritMoneySpr");
       if( null == m_sprite_inheritMoneySpr )
       {
            Engine.Utility.Log.Error("m_sprite_inheritMoneySpr 为空，请检查prefab是否缺乏组件");
       }
        m_btn_kaishiChuanCheng = fastComponent.FastGetComponent<UIButton>("kaishiChuanCheng");
       if( null == m_btn_kaishiChuanCheng )
       {
            Engine.Utility.Log.Error("m_btn_kaishiChuanCheng 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PetUItips_3 = fastComponent.FastGetComponent<UIButton>("PetUItips_3");
       if( null == m_btn_PetUItips_3 )
       {
            Engine.Utility.Log.Error("m_btn_PetUItips_3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_yinhuncontent = fastComponent.FastGetComponent<Transform>("yinhuncontent");
       if( null == m_trans_yinhuncontent )
       {
            Engine.Utility.Log.Error("m_trans_yinhuncontent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Bg = fastComponent.FastGetComponent<UISprite>("Bg");
       if( null == m_sprite_Bg )
       {
            Engine.Utility.Log.Error("m_sprite_Bg 为空，请检查prefab是否缺乏组件");
       }
        m__yinhuntopicon = fastComponent.FastGetComponent<UITexture>("yinhuntopicon");
       if( null == m__yinhuntopicon )
       {
            Engine.Utility.Log.Error("m__yinhuntopicon 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuanadd_xiuwei_level = fastComponent.FastGetComponent<UILabel>("yuanadd_xiuwei_level");
       if( null == m_label_yuanadd_xiuwei_level )
       {
            Engine.Utility.Log.Error("m_label_yuanadd_xiuwei_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_xinadd_xiuwei_level = fastComponent.FastGetComponent<UILabel>("xinadd_xiuwei_level");
       if( null == m_label_xinadd_xiuwei_level )
       {
            Engine.Utility.Log.Error("m_label_xinadd_xiuwei_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuanadd_liliangtianfu = fastComponent.FastGetComponent<UILabel>("yuanadd_liliangtianfu");
       if( null == m_label_yuanadd_liliangtianfu )
       {
            Engine.Utility.Log.Error("m_label_yuanadd_liliangtianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xinadd_liliangtianfu = fastComponent.FastGetComponent<UILabel>("xinadd_liliangtianfu");
       if( null == m_label_xinadd_liliangtianfu )
       {
            Engine.Utility.Log.Error("m_label_xinadd_liliangtianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuanadd_minjietianfu = fastComponent.FastGetComponent<UILabel>("yuanadd_minjietianfu");
       if( null == m_label_yuanadd_minjietianfu )
       {
            Engine.Utility.Log.Error("m_label_yuanadd_minjietianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xinadd_minjietianfu = fastComponent.FastGetComponent<UILabel>("xinadd_minjietianfu");
       if( null == m_label_xinadd_minjietianfu )
       {
            Engine.Utility.Log.Error("m_label_xinadd_minjietianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuanadd_zhilitianfu = fastComponent.FastGetComponent<UILabel>("yuanadd_zhilitianfu");
       if( null == m_label_yuanadd_zhilitianfu )
       {
            Engine.Utility.Log.Error("m_label_yuanadd_zhilitianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xinadd_zhilitianfu = fastComponent.FastGetComponent<UILabel>("xinadd_zhilitianfu");
       if( null == m_label_xinadd_zhilitianfu )
       {
            Engine.Utility.Log.Error("m_label_xinadd_zhilitianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuanadd_tilitianfu = fastComponent.FastGetComponent<UILabel>("yuanadd_tilitianfu");
       if( null == m_label_yuanadd_tilitianfu )
       {
            Engine.Utility.Log.Error("m_label_yuanadd_tilitianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xinadd_tilitianfu = fastComponent.FastGetComponent<UILabel>("xinadd_tilitianfu");
       if( null == m_label_xinadd_tilitianfu )
       {
            Engine.Utility.Log.Error("m_label_xinadd_tilitianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuanadd_jingshentianfu = fastComponent.FastGetComponent<UILabel>("yuanadd_jingshentianfu");
       if( null == m_label_yuanadd_jingshentianfu )
       {
            Engine.Utility.Log.Error("m_label_yuanadd_jingshentianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xinadd_jingshentianfu = fastComponent.FastGetComponent<UILabel>("xinadd_jingshentianfu");
       if( null == m_label_xinadd_jingshentianfu )
       {
            Engine.Utility.Log.Error("m_label_xinadd_jingshentianfu 为空，请检查prefab是否缺乏组件");
       }
        m_widget_yinhun_max = fastComponent.FastGetComponent<UIWidget>("yinhun_max");
       if( null == m_widget_yinhun_max )
       {
            Engine.Utility.Log.Error("m_widget_yinhun_max 为空，请检查prefab是否缺乏组件");
       }
        m_trans_yuanxiuwei = fastComponent.FastGetComponent<Transform>("yuanxiuwei");
       if( null == m_trans_yuanxiuwei )
       {
            Engine.Utility.Log.Error("m_trans_yuanxiuwei 为空，请检查prefab是否缺乏组件");
       }
        m_trans_yuanxiuweiMessageContent = fastComponent.FastGetComponent<Transform>("yuanxiuweiMessageContent");
       if( null == m_trans_yuanxiuweiMessageContent )
       {
            Engine.Utility.Log.Error("m_trans_yuanxiuweiMessageContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuan_xiuwei = fastComponent.FastGetComponent<UILabel>("yuan_xiuwei");
       if( null == m_label_yuan_xiuwei )
       {
            Engine.Utility.Log.Error("m_label_yuan_xiuwei 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuan_liliangtianfu = fastComponent.FastGetComponent<UILabel>("yuan_liliangtianfu");
       if( null == m_label_yuan_liliangtianfu )
       {
            Engine.Utility.Log.Error("m_label_yuan_liliangtianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuan_minjietianfu = fastComponent.FastGetComponent<UILabel>("yuan_minjietianfu");
       if( null == m_label_yuan_minjietianfu )
       {
            Engine.Utility.Log.Error("m_label_yuan_minjietianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuan_zhilitianfu = fastComponent.FastGetComponent<UILabel>("yuan_zhilitianfu");
       if( null == m_label_yuan_zhilitianfu )
       {
            Engine.Utility.Log.Error("m_label_yuan_zhilitianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuan_tilitianfu = fastComponent.FastGetComponent<UILabel>("yuan_tilitianfu");
       if( null == m_label_yuan_tilitianfu )
       {
            Engine.Utility.Log.Error("m_label_yuan_tilitianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_yuan_jingshentianfu = fastComponent.FastGetComponent<UILabel>("yuan_jingshentianfu");
       if( null == m_label_yuan_jingshentianfu )
       {
            Engine.Utility.Log.Error("m_label_yuan_jingshentianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_yinhun_maxLabel = fastComponent.FastGetComponent<UILabel>("yinhun_maxLabel");
       if( null == m_label_yinhun_maxLabel )
       {
            Engine.Utility.Log.Error("m_label_yinhun_maxLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_xinxiuwei = fastComponent.FastGetComponent<Transform>("xinxiuwei");
       if( null == m_trans_xinxiuwei )
       {
            Engine.Utility.Log.Error("m_trans_xinxiuwei 为空，请检查prefab是否缺乏组件");
       }
        m_trans_xinxiuweiContent = fastComponent.FastGetComponent<Transform>("xinxiuweiContent");
       if( null == m_trans_xinxiuweiContent )
       {
            Engine.Utility.Log.Error("m_trans_xinxiuweiContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_xin_xiuwei = fastComponent.FastGetComponent<UILabel>("xin_xiuwei");
       if( null == m_label_xin_xiuwei )
       {
            Engine.Utility.Log.Error("m_label_xin_xiuwei 为空，请检查prefab是否缺乏组件");
       }
        m_trans_xinxiuweicontentparent = fastComponent.FastGetComponent<Transform>("xinxiuweicontentparent");
       if( null == m_trans_xinxiuweicontentparent )
       {
            Engine.Utility.Log.Error("m_trans_xinxiuweicontentparent 为空，请检查prefab是否缺乏组件");
       }
        m_label_xin_liliangtianfu = fastComponent.FastGetComponent<UILabel>("xin_liliangtianfu");
       if( null == m_label_xin_liliangtianfu )
       {
            Engine.Utility.Log.Error("m_label_xin_liliangtianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xin_minjietianfu = fastComponent.FastGetComponent<UILabel>("xin_minjietianfu");
       if( null == m_label_xin_minjietianfu )
       {
            Engine.Utility.Log.Error("m_label_xin_minjietianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xin_zhilitianfu = fastComponent.FastGetComponent<UILabel>("xin_zhilitianfu");
       if( null == m_label_xin_zhilitianfu )
       {
            Engine.Utility.Log.Error("m_label_xin_zhilitianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xin_tilitianfu = fastComponent.FastGetComponent<UILabel>("xin_tilitianfu");
       if( null == m_label_xin_tilitianfu )
       {
            Engine.Utility.Log.Error("m_label_xin_tilitianfu 为空，请检查prefab是否缺乏组件");
       }
        m_label_xin_jingshentianfu = fastComponent.FastGetComponent<UILabel>("xin_jingshentianfu");
       if( null == m_label_xin_jingshentianfu )
       {
            Engine.Utility.Log.Error("m_label_xin_jingshentianfu 为空，请检查prefab是否缺乏组件");
       }
        m_widget_yinhun_nomax = fastComponent.FastGetComponent<UIWidget>("yinhun_nomax");
       if( null == m_widget_yinhun_nomax )
       {
            Engine.Utility.Log.Error("m_widget_yinhun_nomax 为空，请检查prefab是否缺乏组件");
       }
        m_trans_yinhunCostBottom = fastComponent.FastGetComponent<Transform>("yinhunCostBottom");
       if( null == m_trans_yinhunCostBottom )
       {
            Engine.Utility.Log.Error("m_trans_yinhunCostBottom 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AssistContentRootyinhun = fastComponent.FastGetComponent<Transform>("AssistContentRootyinhun");
       if( null == m_trans_AssistContentRootyinhun )
       {
            Engine.Utility.Log.Error("m_trans_AssistContentRootyinhun 为空，请检查prefab是否缺乏组件");
       }
        m__yinhun_icon = fastComponent.FastGetComponent<UITexture>("yinhun_icon");
       if( null == m__yinhun_icon )
       {
            Engine.Utility.Log.Error("m__yinhun_icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_yinhun_icon_di = fastComponent.FastGetComponent<UISprite>("yinhun_icon_di");
       if( null == m_sprite_yinhun_icon_di )
       {
            Engine.Utility.Log.Error("m_sprite_yinhun_icon_di 为空，请检查prefab是否缺乏组件");
       }
        m_label_yinhun_xiaohaoname = fastComponent.FastGetComponent<UILabel>("yinhun_xiaohaoname");
       if( null == m_label_yinhun_xiaohaoname )
       {
            Engine.Utility.Log.Error("m_label_yinhun_xiaohaoname 为空，请检查prefab是否缺乏组件");
       }
        m_label_yinhun_xiaohaonumber = fastComponent.FastGetComponent<UILabel>("yinhun_xiaohaonumber");
       if( null == m_label_yinhun_xiaohaonumber )
       {
            Engine.Utility.Log.Error("m_label_yinhun_xiaohaonumber 为空，请检查prefab是否缺乏组件");
       }
        m_btn_yinhun_xiaohaoSprite = fastComponent.FastGetComponent<UIButton>("yinhun_xiaohaoSprite");
       if( null == m_btn_yinhun_xiaohaoSprite )
       {
            Engine.Utility.Log.Error("m_btn_yinhun_xiaohaoSprite 为空，请检查prefab是否缺乏组件");
       }
        m_label_yinhun_dianjuanxiaohao = fastComponent.FastGetComponent<UILabel>("yinhun_dianjuanxiaohao");
       if( null == m_label_yinhun_dianjuanxiaohao )
       {
            Engine.Utility.Log.Error("m_label_yinhun_dianjuanxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_label_yinhun_xiaohaogold = fastComponent.FastGetComponent<UILabel>("yinhun_xiaohaogold");
       if( null == m_label_yinhun_xiaohaogold )
       {
            Engine.Utility.Log.Error("m_label_yinhun_xiaohaogold 为空，请检查prefab是否缺乏组件");
       }
        m_btn_yinhunyici = fastComponent.FastGetComponent<UIButton>("yinhunyici");
       if( null == m_btn_yinhunyici )
       {
            Engine.Utility.Log.Error("m_btn_yinhunyici 为空，请检查prefab是否缺乏组件");
       }
        m_label_yinhunshici_xiaohaogold = fastComponent.FastGetComponent<UILabel>("yinhunshici_xiaohaogold");
       if( null == m_label_yinhunshici_xiaohaogold )
       {
            Engine.Utility.Log.Error("m_label_yinhunshici_xiaohaogold 为空，请检查prefab是否缺乏组件");
       }
        m_btn_yinhunshici = fastComponent.FastGetComponent<UIButton>("yinhunshici");
       if( null == m_btn_yinhunshici )
       {
            Engine.Utility.Log.Error("m_btn_yinhunshici 为空，请检查prefab是否缺乏组件");
       }
        m_slider_lingqizhi = fastComponent.FastGetComponent<UISlider>("lingqizhi");
       if( null == m_slider_lingqizhi )
       {
            Engine.Utility.Log.Error("m_slider_lingqizhi 为空，请检查prefab是否缺乏组件");
       }
        m_label_yinhunmanji = fastComponent.FastGetComponent<UILabel>("yinhunmanji");
       if( null == m_label_yinhunmanji )
       {
            Engine.Utility.Log.Error("m_label_yinhunmanji 为空，请检查prefab是否缺乏组件");
       }
        m_widget_xiuweishuxingtips = fastComponent.FastGetComponent<UIWidget>("xiuweishuxingtips");
       if( null == m_widget_xiuweishuxingtips )
       {
            Engine.Utility.Log.Error("m_widget_xiuweishuxingtips 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PetUItips_4 = fastComponent.FastGetComponent<UIButton>("PetUItips_4");
       if( null == m_btn_PetUItips_4 )
       {
            Engine.Utility.Log.Error("m_btn_PetUItips_4 为空，请检查prefab是否缺乏组件");
       }
        m_trans_jinengcontent = fastComponent.FastGetComponent<Transform>("jinengcontent");
       if( null == m_trans_jinengcontent )
       {
            Engine.Utility.Log.Error("m_trans_jinengcontent 为空，请检查prefab是否缺乏组件");
       }
        m_label_jinengpetlevel = fastComponent.FastGetComponent<UILabel>("jinengpetlevel");
       if( null == m_label_jinengpetlevel )
       {
            Engine.Utility.Log.Error("m_label_jinengpetlevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_jinengpetname = fastComponent.FastGetComponent<UILabel>("jinengpetname");
       if( null == m_label_jinengpetname )
       {
            Engine.Utility.Log.Error("m_label_jinengpetname 为空，请检查prefab是否缺乏组件");
       }
        m__jinengpeticon = fastComponent.FastGetComponent<UITexture>("jinengpeticon");
       if( null == m__jinengpeticon )
       {
            Engine.Utility.Log.Error("m__jinengpeticon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_petpingzhi = fastComponent.FastGetComponent<UISprite>("petpingzhi");
       if( null == m_sprite_petpingzhi )
       {
            Engine.Utility.Log.Error("m_sprite_petpingzhi 为空，请检查prefab是否缺乏组件");
       }
        m_widget_SkillbtnContainer = fastComponent.FastGetComponent<UIWidget>("SkillbtnContainer");
       if( null == m_widget_SkillbtnContainer )
       {
            Engine.Utility.Log.Error("m_widget_SkillbtnContainer 为空，请检查prefab是否缺乏组件");
       }
        m_widget_SkillDescription = fastComponent.FastGetComponent<UIWidget>("SkillDescription");
       if( null == m_widget_SkillDescription )
       {
            Engine.Utility.Log.Error("m_widget_SkillDescription 为空，请检查prefab是否缺乏组件");
       }
        m_label_Skillname = fastComponent.FastGetComponent<UILabel>("Skillname");
       if( null == m_label_Skillname )
       {
            Engine.Utility.Log.Error("m_label_Skillname 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillLevel = fastComponent.FastGetComponent<UILabel>("SkillLevel");
       if( null == m_label_SkillLevel )
       {
            Engine.Utility.Log.Error("m_label_SkillLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillType = fastComponent.FastGetComponent<UILabel>("SkillType");
       if( null == m_label_SkillType )
       {
            Engine.Utility.Log.Error("m_label_SkillType 为空，请检查prefab是否缺乏组件");
       }
        m_label_NowLevel = fastComponent.FastGetComponent<UILabel>("NowLevel");
       if( null == m_label_NowLevel )
       {
            Engine.Utility.Log.Error("m_label_NowLevel 为空，请检查prefab是否缺乏组件");
       }
        m_widget_next = fastComponent.FastGetComponent<UIWidget>("next");
       if( null == m_widget_next )
       {
            Engine.Utility.Log.Error("m_widget_next 为空，请检查prefab是否缺乏组件");
       }
        m_label_NextLevel = fastComponent.FastGetComponent<UILabel>("NextLevel");
       if( null == m_label_NextLevel )
       {
            Engine.Utility.Log.Error("m_label_NextLevel 为空，请检查prefab是否缺乏组件");
       }
        m__jineng_icon = fastComponent.FastGetComponent<UITexture>("jineng_icon");
       if( null == m__jineng_icon )
       {
            Engine.Utility.Log.Error("m__jineng_icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_jineng_icon_di = fastComponent.FastGetComponent<UISprite>("jineng_icon_di");
       if( null == m_sprite_jineng_icon_di )
       {
            Engine.Utility.Log.Error("m_sprite_jineng_icon_di 为空，请检查prefab是否缺乏组件");
       }
        m_label_jineng_name = fastComponent.FastGetComponent<UILabel>("jineng_name");
       if( null == m_label_jineng_name )
       {
            Engine.Utility.Log.Error("m_label_jineng_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_jineng_number = fastComponent.FastGetComponent<UILabel>("jineng_number");
       if( null == m_label_jineng_number )
       {
            Engine.Utility.Log.Error("m_label_jineng_number 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jineng_Sprite = fastComponent.FastGetComponent<UIButton>("jineng_Sprite");
       if( null == m_btn_jineng_Sprite )
       {
            Engine.Utility.Log.Error("m_btn_jineng_Sprite 为空，请检查prefab是否缺乏组件");
       }
        m_label_jinengshengji_dianjuanxiaohao = fastComponent.FastGetComponent<UILabel>("jinengshengji_dianjuanxiaohao");
       if( null == m_label_jinengshengji_dianjuanxiaohao )
       {
            Engine.Utility.Log.Error("m_label_jinengshengji_dianjuanxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_skill_shengji = fastComponent.FastGetComponent<UIButton>("skill_shengji");
       if( null == m_btn_skill_shengji )
       {
            Engine.Utility.Log.Error("m_btn_skill_shengji 为空，请检查prefab是否缺乏组件");
       }
        m_label_skill_xiaohaogold = fastComponent.FastGetComponent<UILabel>("skill_xiaohaogold");
       if( null == m_label_skill_xiaohaogold )
       {
            Engine.Utility.Log.Error("m_label_skill_xiaohaogold 为空，请检查prefab是否缺乏组件");
       }
        m_label_skillfulltips = fastComponent.FastGetComponent<UILabel>("skillfulltips");
       if( null == m_label_skillfulltips )
       {
            Engine.Utility.Log.Error("m_label_skillfulltips 为空，请检查prefab是否缺乏组件");
       }
        m_btn_xuexijineng = fastComponent.FastGetComponent<UIButton>("xuexijineng");
       if( null == m_btn_xuexijineng )
       {
            Engine.Utility.Log.Error("m_btn_xuexijineng 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Pettips2 = fastComponent.FastGetComponent<UIButton>("Pettips2");
       if( null == m_btn_Pettips2 )
       {
            Engine.Utility.Log.Error("m_btn_Pettips2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_tujiancontent = fastComponent.FastGetComponent<Transform>("tujiancontent");
       if( null == m_trans_tujiancontent )
       {
            Engine.Utility.Log.Error("m_trans_tujiancontent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_tujianbtns = fastComponent.FastGetComponent<Transform>("tujianbtns");
       if( null == m_trans_tujianbtns )
       {
            Engine.Utility.Log.Error("m_trans_tujianbtns 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_tujiantitlescroll = fastComponent.FastGetComponent<UIGridCreatorBase>("tujiantitlescroll");
       if( null == m_ctor_tujiantitlescroll )
       {
            Engine.Utility.Log.Error("m_ctor_tujiantitlescroll 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_tujiantitleItem = fastComponent.FastGetComponent<UISprite>("tujiantitleItem");
       if( null == m_sprite_tujiantitleItem )
       {
            Engine.Utility.Log.Error("m_sprite_tujiantitleItem 为空，请检查prefab是否缺乏组件");
       }
        m_widget_PetTujianItem = fastComponent.FastGetComponent<UIWidget>("PetTujianItem");
       if( null == m_widget_PetTujianItem )
       {
            Engine.Utility.Log.Error("m_widget_PetTujianItem 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_tujianscroll = fastComponent.FastGetComponent<UIGridCreatorBase>("tujianscroll");
       if( null == m_ctor_tujianscroll )
       {
            Engine.Utility.Log.Error("m_ctor_tujianscroll 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_buzhen.gameObject).onClick = _onClick_Buzhen_Btn;
        UIEventListener.Get(m_btn_changename.gameObject).onClick = _onClick_Changename_Btn;
        UIEventListener.Get(m_btn_shuxingfenpei.gameObject).onClick = _onClick_Shuxingfenpei_Btn;
        UIEventListener.Get(m_btn_jinengxuexi.gameObject).onClick = _onClick_Jinengxuexi_Btn;
        UIEventListener.Get(m_btn_xiuxi.gameObject).onClick = _onClick_Xiuxi_Btn;
        UIEventListener.Get(m_btn_addexp.gameObject).onClick = _onClick_Addexp_Btn;
        UIEventListener.Get(m_btn_addlife.gameObject).onClick = _onClick_Addlife_Btn;
        UIEventListener.Get(m_btn_diuqi.gameObject).onClick = _onClick_Diuqi_Btn;
        UIEventListener.Get(m_btn_chuzhan.gameObject).onClick = _onClick_Chuzhan_Btn;
        UIEventListener.Get(m_btn_PetUItips_1.gameObject).onClick = _onClick_PetUItips_1_Btn;
        UIEventListener.Get(m_btn_guiyuanCommon_wanmei.gameObject).onClick = _onClick_GuiyuanCommon_wanmei_Btn;
        UIEventListener.Get(m_btn_tingzhiguiyuan.gameObject).onClick = _onClick_Tingzhiguiyuan_Btn;
        UIEventListener.Get(m_btn_BtnSaveTianfu.gameObject).onClick = _onClick_BtnSaveTianfu_Btn;
        UIEventListener.Get(m_btn_guiyuanCommon_zidongbuzu.gameObject).onClick = _onClick_GuiyuanCommon_zidongbuzu_Btn;
        UIEventListener.Get(m_btn_kaishiguiyuan.gameObject).onClick = _onClick_Kaishiguiyuan_Btn;
        UIEventListener.Get(m_btn_PetUItips_2.gameObject).onClick = _onClick_PetUItips_2_Btn;
        UIEventListener.Get(m_btn_btn_Old_delete.gameObject).onClick = _onClick_Btn_Old_delete_Btn;
        UIEventListener.Get(m_btn_btn_New_delete.gameObject).onClick = _onClick_Btn_New_delete_Btn;
        UIEventListener.Get(m_btn_ChuanChengCommon_zidongbuzu.gameObject).onClick = _onClick_ChuanChengCommon_zidongbuzu_Btn;
        UIEventListener.Get(m_btn_kaishiChuanCheng.gameObject).onClick = _onClick_KaishiChuanCheng_Btn;
        UIEventListener.Get(m_btn_PetUItips_3.gameObject).onClick = _onClick_PetUItips_3_Btn;
        UIEventListener.Get(m_btn_yinhun_xiaohaoSprite.gameObject).onClick = _onClick_Yinhun_xiaohaoSprite_Btn;
        UIEventListener.Get(m_btn_yinhunyici.gameObject).onClick = _onClick_Yinhunyici_Btn;
        UIEventListener.Get(m_btn_yinhunshici.gameObject).onClick = _onClick_Yinhunshici_Btn;
        UIEventListener.Get(m_btn_PetUItips_4.gameObject).onClick = _onClick_PetUItips_4_Btn;
        UIEventListener.Get(m_btn_jineng_Sprite.gameObject).onClick = _onClick_Jineng_Sprite_Btn;
        UIEventListener.Get(m_btn_skill_shengji.gameObject).onClick = _onClick_Skill_shengji_Btn;
        UIEventListener.Get(m_btn_xuexijineng.gameObject).onClick = _onClick_Xuexijineng_Btn;
        UIEventListener.Get(m_btn_Pettips2.gameObject).onClick = _onClick_Pettips2_Btn;
    }

    void _onClick_Buzhen_Btn(GameObject caster)
    {
        onClick_Buzhen_Btn( caster );
    }

    void _onClick_Changename_Btn(GameObject caster)
    {
        onClick_Changename_Btn( caster );
    }

    void _onClick_Shuxingfenpei_Btn(GameObject caster)
    {
        onClick_Shuxingfenpei_Btn( caster );
    }

    void _onClick_Jinengxuexi_Btn(GameObject caster)
    {
        onClick_Jinengxuexi_Btn( caster );
    }

    void _onClick_Xiuxi_Btn(GameObject caster)
    {
        onClick_Xiuxi_Btn( caster );
    }

    void _onClick_Addexp_Btn(GameObject caster)
    {
        onClick_Addexp_Btn( caster );
    }

    void _onClick_Addlife_Btn(GameObject caster)
    {
        onClick_Addlife_Btn( caster );
    }

    void _onClick_Diuqi_Btn(GameObject caster)
    {
        onClick_Diuqi_Btn( caster );
    }

    void _onClick_Chuzhan_Btn(GameObject caster)
    {
        onClick_Chuzhan_Btn( caster );
    }

    void _onClick_PetUItips_1_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_GuiyuanCommon_wanmei_Btn(GameObject caster)
    {
        onClick_GuiyuanCommon_wanmei_Btn( caster );
    }

    void _onClick_Tingzhiguiyuan_Btn(GameObject caster)
    {
        onClick_Tingzhiguiyuan_Btn( caster );
    }

    void _onClick_BtnSaveTianfu_Btn(GameObject caster)
    {
        onClick_BtnSaveTianfu_Btn( caster );
    }

    void _onClick_GuiyuanCommon_zidongbuzu_Btn(GameObject caster)
    {
        onClick_GuiyuanCommon_zidongbuzu_Btn( caster );
    }

    void _onClick_Kaishiguiyuan_Btn(GameObject caster)
    {
        onClick_Kaishiguiyuan_Btn( caster );
    }

    void _onClick_PetUItips_2_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_Btn_Old_delete_Btn(GameObject caster)
    {
        onClick_Btn_Old_delete_Btn( caster );
    }

    void _onClick_Btn_New_delete_Btn(GameObject caster)
    {
        onClick_Btn_New_delete_Btn( caster );
    }

    void _onClick_ChuanChengCommon_zidongbuzu_Btn(GameObject caster)
    {
        onClick_ChuanChengCommon_zidongbuzu_Btn( caster );
    }

    void _onClick_KaishiChuanCheng_Btn(GameObject caster)
    {
        onClick_KaishiChuanCheng_Btn( caster );
    }

    void _onClick_PetUItips_3_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_Yinhun_xiaohaoSprite_Btn(GameObject caster)
    {
        onClick_Yinhun_xiaohaoSprite_Btn( caster );
    }

    void _onClick_Yinhunyici_Btn(GameObject caster)
    {
        onClick_Yinhunyici_Btn( caster );
    }

    void _onClick_Yinhunshici_Btn(GameObject caster)
    {
        onClick_Yinhunshici_Btn( caster );
    }

    void _onClick_PetUItips_4_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_Jineng_Sprite_Btn(GameObject caster)
    {
        onClick_Jineng_Sprite_Btn( caster );
    }

    void _onClick_Skill_shengji_Btn(GameObject caster)
    {
        onClick_Skill_shengji_Btn( caster );
    }

    void _onClick_Xuexijineng_Btn(GameObject caster)
    {
        onClick_Xuexijineng_Btn( caster );
    }

    void _onClick_Pettips2_Btn(GameObject caster)
    {
        onClick_Pettips2_Btn( caster );
    }


}
