//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PropPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//属性
		ShuXing = 1,
		//称号
		ChengHao = 2,
		//时装
		ShiZhuang = 3,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_PropAndFashionContent;

    UILabel              m_label_Vip;

    UITexture            m__modelTexture;

    Transform            m_trans_PropContent;

    Transform            m_trans_roleinfo;

    UILabel              m_label_Name;

    UILabel              m_label_Level;

    UILabel              m_label_fight;

    UILabel              m_label_PKzhi;

    UITexture            m__headicon;

    UILabel              m_label_codeNum;

    UILabel              m_label_bianhao;

    UILabel              m_label_PKNum;

    UIButton             m_btn_Sprite;

    UILabel              m_label_RemainTimeLabel;

    UIButton             m_btn_BgCollider;

    UILabel              m_label_clanName;

    UILabel              m_label_WorldLevel;

    UILabel              m_label_extraExp;

    UIButton             m_btn_basePropContent;

    UILabel              m_label_speed;

    UIButton             m_btn_detailPropContent;

    UILabel              m_label_Label;

    UIScrollView         m_scrollview_detaiContent;

    UILabel              m_label_liliang2;

    UILabel              m_label_minjie2;

    UILabel              m_label_zhili2;

    UILabel              m_label_tili2;

    UILabel              m_label_jingli2;

    UILabel              m_label_shengmingzhi;

    UILabel              m_label_fashuzhi;

    UILabel              m_label_wuligongji2;

    UILabel              m_label_fashugongji2;

    UILabel              m_label_wulifangyu2;

    UILabel              m_label_fashufangyu2;

    UILabel              m_label_zhiliao2;

    UILabel              m_label_mingzhong;

    UILabel              m_label_shanbi;

    UILabel              m_label_wulibaoji;

    UILabel              m_label_fashubaoji;

    UILabel              m_label_baojishanghai;

    UILabel              m_label_wushangxishou;

    UILabel              m_label_fashangxishou;

    UILabel              m_label_shanghaijiashen;

    UILabel              m_label_shanghaixishou;

    UILabel              m_label_huogong;

    UILabel              m_label_binggong;

    UILabel              m_label_diangong;

    UILabel              m_label_angong;

    UILabel              m_label_huofang;

    UILabel              m_label_bingfang;

    UILabel              m_label_dianfang;

    UILabel              m_label_anfang;

    UIScrollView         m_scrollview_basePropTextContainer;

    UIButton             m_btn_PropUItips_1;

    UISlider             m_slider_Hpslider;

    UILabel              m_label_hp_percent;

    UISlider             m_slider_Mpslider;

    UILabel              m_label_mp_percent;

    UISlider             m_slider_Expslider;

    UILabel              m_label_exp_percent;

    UILabel              m_label_liliang;

    UILabel              m_label_wuligongji;

    UILabel              m_label_minjie;

    UILabel              m_label_fashugongji;

    UILabel              m_label_zhili;

    UILabel              m_label_wulifangyu;

    UILabel              m_label_tili;

    UILabel              m_label_fashufangyu;

    UILabel              m_label_jingli;

    UILabel              m_label_zhiliao;

    Transform            m_trans_FashionContent;

    Transform            m_trans_FashionLeftContent;

    UIButton             m_btn_btn_reset;

    UIScrollView         m_scrollview_TableContent;

    UIGrid               m_grid_suitgrid;

    Transform            m_trans_SuitBtnItem;

    Transform            m_trans_MessageContent;

    UILabel              m_label_FashionName;

    UILabel              m_label_FashionDescription;

    UIButton             m_btn_BtnStatus;

    UILabel              m_label_LabelStatus;

    UIButton             m_btn_FashionTipsClose;

    Transform            m_trans_UpContent;

    Transform            m_trans_FashionScrollView;

    Transform            m_trans_TitleContent;

    UIButton             m_btn_btn_ViewProp;

    UIScrollView         m_scrollview_TitleListScrollView;

    Transform            m_trans_TitleTypeGridCacheRoot;

    Transform            m_trans_TitleSecondTypeGridCacheRoot;

    UITable              m__TitleTable;

    Transform            m_trans_TitleFx;

    UILabel              m_label_TitleLabel;

    UIWidget             m_widget_NumAndTime;

    UILabel              m_label_UseNumberLbl;

    UIButton             m_btn_btn_UseNumAdd;

    UILabel              m_label_UseTimeLbl;

    UIWidget             m_widget_Permanent;

    UIWidget             m_widget_LimitedTime;

    UILabel              m_label_LimitedTimeLbl;

    UIWidget             m_widget_LimitedNumber;

    UILabel              m_label_LimitedNumberLbl;

    UIButton             m_btn_btn_LimitedNumberAdd;

    UILabel              m_label_TitleDescription;

    UILabel              m_label_AttachedFighting;

    Transform            m_trans_AttachedScrollViewContent;

    UILabel              m_label_UseFighting;

    Transform            m_trans_UseScrollViewContent;

    UIButton             m_btn_btn_WearTitle;

    UIButton             m_btn_btn_UseTitle;

    UIButton             m_btn_btn_WearAndUseTitle;

    Transform            m_trans_AddFrequency;

    UIButton             m_btn_btn_quxiao;

    UIButton             m_btn_btn_queding;

    UIButton             m_btn_ptguiyuan_icon;

    UILabel              m_label_ptguiyuan_name;

    UILabel              m_label_ptguiyuan_number;

    UILabel              m_label_PTguiyuan_dianjuanxiaohao;

    UIButton             m_btn_zidongbuzu;

    UILabel              m_label_description;

    UIGridCreatorBase    m_ctor_TitleCategoryScrollview;

    Transform            m_trans_UITitleCategorygrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_PropAndFashionContent = fastComponent.FastGetComponent<Transform>("PropAndFashionContent");
       if( null == m_trans_PropAndFashionContent )
       {
            Engine.Utility.Log.Error("m_trans_PropAndFashionContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_Vip = fastComponent.FastGetComponent<UILabel>("Vip");
       if( null == m_label_Vip )
       {
            Engine.Utility.Log.Error("m_label_Vip 为空，请检查prefab是否缺乏组件");
       }
        m__modelTexture = fastComponent.FastGetComponent<UITexture>("modelTexture");
       if( null == m__modelTexture )
       {
            Engine.Utility.Log.Error("m__modelTexture 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PropContent = fastComponent.FastGetComponent<Transform>("PropContent");
       if( null == m_trans_PropContent )
       {
            Engine.Utility.Log.Error("m_trans_PropContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_roleinfo = fastComponent.FastGetComponent<Transform>("roleinfo");
       if( null == m_trans_roleinfo )
       {
            Engine.Utility.Log.Error("m_trans_roleinfo 为空，请检查prefab是否缺乏组件");
       }
        m_label_Name = fastComponent.FastGetComponent<UILabel>("Name");
       if( null == m_label_Name )
       {
            Engine.Utility.Log.Error("m_label_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_Level = fastComponent.FastGetComponent<UILabel>("Level");
       if( null == m_label_Level )
       {
            Engine.Utility.Log.Error("m_label_Level 为空，请检查prefab是否缺乏组件");
       }
        m_label_fight = fastComponent.FastGetComponent<UILabel>("fight");
       if( null == m_label_fight )
       {
            Engine.Utility.Log.Error("m_label_fight 为空，请检查prefab是否缺乏组件");
       }
        m_label_PKzhi = fastComponent.FastGetComponent<UILabel>("PKzhi");
       if( null == m_label_PKzhi )
       {
            Engine.Utility.Log.Error("m_label_PKzhi 为空，请检查prefab是否缺乏组件");
       }
        m__headicon = fastComponent.FastGetComponent<UITexture>("headicon");
       if( null == m__headicon )
       {
            Engine.Utility.Log.Error("m__headicon 为空，请检查prefab是否缺乏组件");
       }
        m_label_codeNum = fastComponent.FastGetComponent<UILabel>("codeNum");
       if( null == m_label_codeNum )
       {
            Engine.Utility.Log.Error("m_label_codeNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_bianhao = fastComponent.FastGetComponent<UILabel>("bianhao");
       if( null == m_label_bianhao )
       {
            Engine.Utility.Log.Error("m_label_bianhao 为空，请检查prefab是否缺乏组件");
       }
        m_label_PKNum = fastComponent.FastGetComponent<UILabel>("PKNum");
       if( null == m_label_PKNum )
       {
            Engine.Utility.Log.Error("m_label_PKNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Sprite = fastComponent.FastGetComponent<UIButton>("Sprite");
       if( null == m_btn_Sprite )
       {
            Engine.Utility.Log.Error("m_btn_Sprite 为空，请检查prefab是否缺乏组件");
       }
        m_label_RemainTimeLabel = fastComponent.FastGetComponent<UILabel>("RemainTimeLabel");
       if( null == m_label_RemainTimeLabel )
       {
            Engine.Utility.Log.Error("m_label_RemainTimeLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BgCollider = fastComponent.FastGetComponent<UIButton>("BgCollider");
       if( null == m_btn_BgCollider )
       {
            Engine.Utility.Log.Error("m_btn_BgCollider 为空，请检查prefab是否缺乏组件");
       }
        m_label_clanName = fastComponent.FastGetComponent<UILabel>("clanName");
       if( null == m_label_clanName )
       {
            Engine.Utility.Log.Error("m_label_clanName 为空，请检查prefab是否缺乏组件");
       }
        m_label_WorldLevel = fastComponent.FastGetComponent<UILabel>("WorldLevel");
       if( null == m_label_WorldLevel )
       {
            Engine.Utility.Log.Error("m_label_WorldLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_extraExp = fastComponent.FastGetComponent<UILabel>("extraExp");
       if( null == m_label_extraExp )
       {
            Engine.Utility.Log.Error("m_label_extraExp 为空，请检查prefab是否缺乏组件");
       }
        m_btn_basePropContent = fastComponent.FastGetComponent<UIButton>("basePropContent");
       if( null == m_btn_basePropContent )
       {
            Engine.Utility.Log.Error("m_btn_basePropContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_speed = fastComponent.FastGetComponent<UILabel>("speed");
       if( null == m_label_speed )
       {
            Engine.Utility.Log.Error("m_label_speed 为空，请检查prefab是否缺乏组件");
       }
        m_btn_detailPropContent = fastComponent.FastGetComponent<UIButton>("detailPropContent");
       if( null == m_btn_detailPropContent )
       {
            Engine.Utility.Log.Error("m_btn_detailPropContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_detaiContent = fastComponent.FastGetComponent<UIScrollView>("detaiContent");
       if( null == m_scrollview_detaiContent )
       {
            Engine.Utility.Log.Error("m_scrollview_detaiContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_liliang2 = fastComponent.FastGetComponent<UILabel>("liliang2");
       if( null == m_label_liliang2 )
       {
            Engine.Utility.Log.Error("m_label_liliang2 为空，请检查prefab是否缺乏组件");
       }
        m_label_minjie2 = fastComponent.FastGetComponent<UILabel>("minjie2");
       if( null == m_label_minjie2 )
       {
            Engine.Utility.Log.Error("m_label_minjie2 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhili2 = fastComponent.FastGetComponent<UILabel>("zhili2");
       if( null == m_label_zhili2 )
       {
            Engine.Utility.Log.Error("m_label_zhili2 为空，请检查prefab是否缺乏组件");
       }
        m_label_tili2 = fastComponent.FastGetComponent<UILabel>("tili2");
       if( null == m_label_tili2 )
       {
            Engine.Utility.Log.Error("m_label_tili2 为空，请检查prefab是否缺乏组件");
       }
        m_label_jingli2 = fastComponent.FastGetComponent<UILabel>("jingli2");
       if( null == m_label_jingli2 )
       {
            Engine.Utility.Log.Error("m_label_jingli2 为空，请检查prefab是否缺乏组件");
       }
        m_label_shengmingzhi = fastComponent.FastGetComponent<UILabel>("shengmingzhi");
       if( null == m_label_shengmingzhi )
       {
            Engine.Utility.Log.Error("m_label_shengmingzhi 为空，请检查prefab是否缺乏组件");
       }
        m_label_fashuzhi = fastComponent.FastGetComponent<UILabel>("fashuzhi");
       if( null == m_label_fashuzhi )
       {
            Engine.Utility.Log.Error("m_label_fashuzhi 为空，请检查prefab是否缺乏组件");
       }
        m_label_wuligongji2 = fastComponent.FastGetComponent<UILabel>("wuligongji2");
       if( null == m_label_wuligongji2 )
       {
            Engine.Utility.Log.Error("m_label_wuligongji2 为空，请检查prefab是否缺乏组件");
       }
        m_label_fashugongji2 = fastComponent.FastGetComponent<UILabel>("fashugongji2");
       if( null == m_label_fashugongji2 )
       {
            Engine.Utility.Log.Error("m_label_fashugongji2 为空，请检查prefab是否缺乏组件");
       }
        m_label_wulifangyu2 = fastComponent.FastGetComponent<UILabel>("wulifangyu2");
       if( null == m_label_wulifangyu2 )
       {
            Engine.Utility.Log.Error("m_label_wulifangyu2 为空，请检查prefab是否缺乏组件");
       }
        m_label_fashufangyu2 = fastComponent.FastGetComponent<UILabel>("fashufangyu2");
       if( null == m_label_fashufangyu2 )
       {
            Engine.Utility.Log.Error("m_label_fashufangyu2 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhiliao2 = fastComponent.FastGetComponent<UILabel>("zhiliao2");
       if( null == m_label_zhiliao2 )
       {
            Engine.Utility.Log.Error("m_label_zhiliao2 为空，请检查prefab是否缺乏组件");
       }
        m_label_mingzhong = fastComponent.FastGetComponent<UILabel>("mingzhong");
       if( null == m_label_mingzhong )
       {
            Engine.Utility.Log.Error("m_label_mingzhong 为空，请检查prefab是否缺乏组件");
       }
        m_label_shanbi = fastComponent.FastGetComponent<UILabel>("shanbi");
       if( null == m_label_shanbi )
       {
            Engine.Utility.Log.Error("m_label_shanbi 为空，请检查prefab是否缺乏组件");
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
        m_label_baojishanghai = fastComponent.FastGetComponent<UILabel>("baojishanghai");
       if( null == m_label_baojishanghai )
       {
            Engine.Utility.Log.Error("m_label_baojishanghai 为空，请检查prefab是否缺乏组件");
       }
        m_label_wushangxishou = fastComponent.FastGetComponent<UILabel>("wushangxishou");
       if( null == m_label_wushangxishou )
       {
            Engine.Utility.Log.Error("m_label_wushangxishou 为空，请检查prefab是否缺乏组件");
       }
        m_label_fashangxishou = fastComponent.FastGetComponent<UILabel>("fashangxishou");
       if( null == m_label_fashangxishou )
       {
            Engine.Utility.Log.Error("m_label_fashangxishou 为空，请检查prefab是否缺乏组件");
       }
        m_label_shanghaijiashen = fastComponent.FastGetComponent<UILabel>("shanghaijiashen");
       if( null == m_label_shanghaijiashen )
       {
            Engine.Utility.Log.Error("m_label_shanghaijiashen 为空，请检查prefab是否缺乏组件");
       }
        m_label_shanghaixishou = fastComponent.FastGetComponent<UILabel>("shanghaixishou");
       if( null == m_label_shanghaixishou )
       {
            Engine.Utility.Log.Error("m_label_shanghaixishou 为空，请检查prefab是否缺乏组件");
       }
        m_label_huogong = fastComponent.FastGetComponent<UILabel>("huogong");
       if( null == m_label_huogong )
       {
            Engine.Utility.Log.Error("m_label_huogong 为空，请检查prefab是否缺乏组件");
       }
        m_label_binggong = fastComponent.FastGetComponent<UILabel>("binggong");
       if( null == m_label_binggong )
       {
            Engine.Utility.Log.Error("m_label_binggong 为空，请检查prefab是否缺乏组件");
       }
        m_label_diangong = fastComponent.FastGetComponent<UILabel>("diangong");
       if( null == m_label_diangong )
       {
            Engine.Utility.Log.Error("m_label_diangong 为空，请检查prefab是否缺乏组件");
       }
        m_label_angong = fastComponent.FastGetComponent<UILabel>("angong");
       if( null == m_label_angong )
       {
            Engine.Utility.Log.Error("m_label_angong 为空，请检查prefab是否缺乏组件");
       }
        m_label_huofang = fastComponent.FastGetComponent<UILabel>("huofang");
       if( null == m_label_huofang )
       {
            Engine.Utility.Log.Error("m_label_huofang 为空，请检查prefab是否缺乏组件");
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
        m_scrollview_basePropTextContainer = fastComponent.FastGetComponent<UIScrollView>("basePropTextContainer");
       if( null == m_scrollview_basePropTextContainer )
       {
            Engine.Utility.Log.Error("m_scrollview_basePropTextContainer 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PropUItips_1 = fastComponent.FastGetComponent<UIButton>("PropUItips_1");
       if( null == m_btn_PropUItips_1 )
       {
            Engine.Utility.Log.Error("m_btn_PropUItips_1 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Hpslider = fastComponent.FastGetComponent<UISlider>("Hpslider");
       if( null == m_slider_Hpslider )
       {
            Engine.Utility.Log.Error("m_slider_Hpslider 为空，请检查prefab是否缺乏组件");
       }
        m_label_hp_percent = fastComponent.FastGetComponent<UILabel>("hp_percent");
       if( null == m_label_hp_percent )
       {
            Engine.Utility.Log.Error("m_label_hp_percent 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Mpslider = fastComponent.FastGetComponent<UISlider>("Mpslider");
       if( null == m_slider_Mpslider )
       {
            Engine.Utility.Log.Error("m_slider_Mpslider 为空，请检查prefab是否缺乏组件");
       }
        m_label_mp_percent = fastComponent.FastGetComponent<UILabel>("mp_percent");
       if( null == m_label_mp_percent )
       {
            Engine.Utility.Log.Error("m_label_mp_percent 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Expslider = fastComponent.FastGetComponent<UISlider>("Expslider");
       if( null == m_slider_Expslider )
       {
            Engine.Utility.Log.Error("m_slider_Expslider 为空，请检查prefab是否缺乏组件");
       }
        m_label_exp_percent = fastComponent.FastGetComponent<UILabel>("exp_percent");
       if( null == m_label_exp_percent )
       {
            Engine.Utility.Log.Error("m_label_exp_percent 为空，请检查prefab是否缺乏组件");
       }
        m_label_liliang = fastComponent.FastGetComponent<UILabel>("liliang");
       if( null == m_label_liliang )
       {
            Engine.Utility.Log.Error("m_label_liliang 为空，请检查prefab是否缺乏组件");
       }
        m_label_wuligongji = fastComponent.FastGetComponent<UILabel>("wuligongji");
       if( null == m_label_wuligongji )
       {
            Engine.Utility.Log.Error("m_label_wuligongji 为空，请检查prefab是否缺乏组件");
       }
        m_label_minjie = fastComponent.FastGetComponent<UILabel>("minjie");
       if( null == m_label_minjie )
       {
            Engine.Utility.Log.Error("m_label_minjie 为空，请检查prefab是否缺乏组件");
       }
        m_label_fashugongji = fastComponent.FastGetComponent<UILabel>("fashugongji");
       if( null == m_label_fashugongji )
       {
            Engine.Utility.Log.Error("m_label_fashugongji 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhili = fastComponent.FastGetComponent<UILabel>("zhili");
       if( null == m_label_zhili )
       {
            Engine.Utility.Log.Error("m_label_zhili 为空，请检查prefab是否缺乏组件");
       }
        m_label_wulifangyu = fastComponent.FastGetComponent<UILabel>("wulifangyu");
       if( null == m_label_wulifangyu )
       {
            Engine.Utility.Log.Error("m_label_wulifangyu 为空，请检查prefab是否缺乏组件");
       }
        m_label_tili = fastComponent.FastGetComponent<UILabel>("tili");
       if( null == m_label_tili )
       {
            Engine.Utility.Log.Error("m_label_tili 为空，请检查prefab是否缺乏组件");
       }
        m_label_fashufangyu = fastComponent.FastGetComponent<UILabel>("fashufangyu");
       if( null == m_label_fashufangyu )
       {
            Engine.Utility.Log.Error("m_label_fashufangyu 为空，请检查prefab是否缺乏组件");
       }
        m_label_jingli = fastComponent.FastGetComponent<UILabel>("jingli");
       if( null == m_label_jingli )
       {
            Engine.Utility.Log.Error("m_label_jingli 为空，请检查prefab是否缺乏组件");
       }
        m_label_zhiliao = fastComponent.FastGetComponent<UILabel>("zhiliao");
       if( null == m_label_zhiliao )
       {
            Engine.Utility.Log.Error("m_label_zhiliao 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FashionContent = fastComponent.FastGetComponent<Transform>("FashionContent");
       if( null == m_trans_FashionContent )
       {
            Engine.Utility.Log.Error("m_trans_FashionContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FashionLeftContent = fastComponent.FastGetComponent<Transform>("FashionLeftContent");
       if( null == m_trans_FashionLeftContent )
       {
            Engine.Utility.Log.Error("m_trans_FashionLeftContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_reset = fastComponent.FastGetComponent<UIButton>("btn_reset");
       if( null == m_btn_btn_reset )
       {
            Engine.Utility.Log.Error("m_btn_btn_reset 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_TableContent = fastComponent.FastGetComponent<UIScrollView>("TableContent");
       if( null == m_scrollview_TableContent )
       {
            Engine.Utility.Log.Error("m_scrollview_TableContent 为空，请检查prefab是否缺乏组件");
       }
        m_grid_suitgrid = fastComponent.FastGetComponent<UIGrid>("suitgrid");
       if( null == m_grid_suitgrid )
       {
            Engine.Utility.Log.Error("m_grid_suitgrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SuitBtnItem = fastComponent.FastGetComponent<Transform>("SuitBtnItem");
       if( null == m_trans_SuitBtnItem )
       {
            Engine.Utility.Log.Error("m_trans_SuitBtnItem 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MessageContent = fastComponent.FastGetComponent<Transform>("MessageContent");
       if( null == m_trans_MessageContent )
       {
            Engine.Utility.Log.Error("m_trans_MessageContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_FashionName = fastComponent.FastGetComponent<UILabel>("FashionName");
       if( null == m_label_FashionName )
       {
            Engine.Utility.Log.Error("m_label_FashionName 为空，请检查prefab是否缺乏组件");
       }
        m_label_FashionDescription = fastComponent.FastGetComponent<UILabel>("FashionDescription");
       if( null == m_label_FashionDescription )
       {
            Engine.Utility.Log.Error("m_label_FashionDescription 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnStatus = fastComponent.FastGetComponent<UIButton>("BtnStatus");
       if( null == m_btn_BtnStatus )
       {
            Engine.Utility.Log.Error("m_btn_BtnStatus 为空，请检查prefab是否缺乏组件");
       }
        m_label_LabelStatus = fastComponent.FastGetComponent<UILabel>("LabelStatus");
       if( null == m_label_LabelStatus )
       {
            Engine.Utility.Log.Error("m_label_LabelStatus 为空，请检查prefab是否缺乏组件");
       }
        m_btn_FashionTipsClose = fastComponent.FastGetComponent<UIButton>("FashionTipsClose");
       if( null == m_btn_FashionTipsClose )
       {
            Engine.Utility.Log.Error("m_btn_FashionTipsClose 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UpContent = fastComponent.FastGetComponent<Transform>("UpContent");
       if( null == m_trans_UpContent )
       {
            Engine.Utility.Log.Error("m_trans_UpContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FashionScrollView = fastComponent.FastGetComponent<Transform>("FashionScrollView");
       if( null == m_trans_FashionScrollView )
       {
            Engine.Utility.Log.Error("m_trans_FashionScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TitleContent = fastComponent.FastGetComponent<Transform>("TitleContent");
       if( null == m_trans_TitleContent )
       {
            Engine.Utility.Log.Error("m_trans_TitleContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_ViewProp = fastComponent.FastGetComponent<UIButton>("btn_ViewProp");
       if( null == m_btn_btn_ViewProp )
       {
            Engine.Utility.Log.Error("m_btn_btn_ViewProp 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_TitleListScrollView = fastComponent.FastGetComponent<UIScrollView>("TitleListScrollView");
       if( null == m_scrollview_TitleListScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_TitleListScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TitleTypeGridCacheRoot = fastComponent.FastGetComponent<Transform>("TitleTypeGridCacheRoot");
       if( null == m_trans_TitleTypeGridCacheRoot )
       {
            Engine.Utility.Log.Error("m_trans_TitleTypeGridCacheRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TitleSecondTypeGridCacheRoot = fastComponent.FastGetComponent<Transform>("TitleSecondTypeGridCacheRoot");
       if( null == m_trans_TitleSecondTypeGridCacheRoot )
       {
            Engine.Utility.Log.Error("m_trans_TitleSecondTypeGridCacheRoot 为空，请检查prefab是否缺乏组件");
       }
        m__TitleTable = fastComponent.FastGetComponent<UITable>("TitleTable");
       if( null == m__TitleTable )
       {
            Engine.Utility.Log.Error("m__TitleTable 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TitleFx = fastComponent.FastGetComponent<Transform>("TitleFx");
       if( null == m_trans_TitleFx )
       {
            Engine.Utility.Log.Error("m_trans_TitleFx 为空，请检查prefab是否缺乏组件");
       }
        m_label_TitleLabel = fastComponent.FastGetComponent<UILabel>("TitleLabel");
       if( null == m_label_TitleLabel )
       {
            Engine.Utility.Log.Error("m_label_TitleLabel 为空，请检查prefab是否缺乏组件");
       }
        m_widget_NumAndTime = fastComponent.FastGetComponent<UIWidget>("NumAndTime");
       if( null == m_widget_NumAndTime )
       {
            Engine.Utility.Log.Error("m_widget_NumAndTime 为空，请检查prefab是否缺乏组件");
       }
        m_label_UseNumberLbl = fastComponent.FastGetComponent<UILabel>("UseNumberLbl");
       if( null == m_label_UseNumberLbl )
       {
            Engine.Utility.Log.Error("m_label_UseNumberLbl 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_UseNumAdd = fastComponent.FastGetComponent<UIButton>("btn_UseNumAdd");
       if( null == m_btn_btn_UseNumAdd )
       {
            Engine.Utility.Log.Error("m_btn_btn_UseNumAdd 为空，请检查prefab是否缺乏组件");
       }
        m_label_UseTimeLbl = fastComponent.FastGetComponent<UILabel>("UseTimeLbl");
       if( null == m_label_UseTimeLbl )
       {
            Engine.Utility.Log.Error("m_label_UseTimeLbl 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Permanent = fastComponent.FastGetComponent<UIWidget>("Permanent");
       if( null == m_widget_Permanent )
       {
            Engine.Utility.Log.Error("m_widget_Permanent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_LimitedTime = fastComponent.FastGetComponent<UIWidget>("LimitedTime");
       if( null == m_widget_LimitedTime )
       {
            Engine.Utility.Log.Error("m_widget_LimitedTime 为空，请检查prefab是否缺乏组件");
       }
        m_label_LimitedTimeLbl = fastComponent.FastGetComponent<UILabel>("LimitedTimeLbl");
       if( null == m_label_LimitedTimeLbl )
       {
            Engine.Utility.Log.Error("m_label_LimitedTimeLbl 为空，请检查prefab是否缺乏组件");
       }
        m_widget_LimitedNumber = fastComponent.FastGetComponent<UIWidget>("LimitedNumber");
       if( null == m_widget_LimitedNumber )
       {
            Engine.Utility.Log.Error("m_widget_LimitedNumber 为空，请检查prefab是否缺乏组件");
       }
        m_label_LimitedNumberLbl = fastComponent.FastGetComponent<UILabel>("LimitedNumberLbl");
       if( null == m_label_LimitedNumberLbl )
       {
            Engine.Utility.Log.Error("m_label_LimitedNumberLbl 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_LimitedNumberAdd = fastComponent.FastGetComponent<UIButton>("btn_LimitedNumberAdd");
       if( null == m_btn_btn_LimitedNumberAdd )
       {
            Engine.Utility.Log.Error("m_btn_btn_LimitedNumberAdd 为空，请检查prefab是否缺乏组件");
       }
        m_label_TitleDescription = fastComponent.FastGetComponent<UILabel>("TitleDescription");
       if( null == m_label_TitleDescription )
       {
            Engine.Utility.Log.Error("m_label_TitleDescription 为空，请检查prefab是否缺乏组件");
       }
        m_label_AttachedFighting = fastComponent.FastGetComponent<UILabel>("AttachedFighting");
       if( null == m_label_AttachedFighting )
       {
            Engine.Utility.Log.Error("m_label_AttachedFighting 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AttachedScrollViewContent = fastComponent.FastGetComponent<Transform>("AttachedScrollViewContent");
       if( null == m_trans_AttachedScrollViewContent )
       {
            Engine.Utility.Log.Error("m_trans_AttachedScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_UseFighting = fastComponent.FastGetComponent<UILabel>("UseFighting");
       if( null == m_label_UseFighting )
       {
            Engine.Utility.Log.Error("m_label_UseFighting 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UseScrollViewContent = fastComponent.FastGetComponent<Transform>("UseScrollViewContent");
       if( null == m_trans_UseScrollViewContent )
       {
            Engine.Utility.Log.Error("m_trans_UseScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_WearTitle = fastComponent.FastGetComponent<UIButton>("btn_WearTitle");
       if( null == m_btn_btn_WearTitle )
       {
            Engine.Utility.Log.Error("m_btn_btn_WearTitle 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_UseTitle = fastComponent.FastGetComponent<UIButton>("btn_UseTitle");
       if( null == m_btn_btn_UseTitle )
       {
            Engine.Utility.Log.Error("m_btn_btn_UseTitle 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_WearAndUseTitle = fastComponent.FastGetComponent<UIButton>("btn_WearAndUseTitle");
       if( null == m_btn_btn_WearAndUseTitle )
       {
            Engine.Utility.Log.Error("m_btn_btn_WearAndUseTitle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AddFrequency = fastComponent.FastGetComponent<Transform>("AddFrequency");
       if( null == m_trans_AddFrequency )
       {
            Engine.Utility.Log.Error("m_trans_AddFrequency 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_quxiao = fastComponent.FastGetComponent<UIButton>("btn_quxiao");
       if( null == m_btn_btn_quxiao )
       {
            Engine.Utility.Log.Error("m_btn_btn_quxiao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_queding = fastComponent.FastGetComponent<UIButton>("btn_queding");
       if( null == m_btn_btn_queding )
       {
            Engine.Utility.Log.Error("m_btn_btn_queding 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ptguiyuan_icon = fastComponent.FastGetComponent<UIButton>("ptguiyuan_icon");
       if( null == m_btn_ptguiyuan_icon )
       {
            Engine.Utility.Log.Error("m_btn_ptguiyuan_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_ptguiyuan_name = fastComponent.FastGetComponent<UILabel>("ptguiyuan_name");
       if( null == m_label_ptguiyuan_name )
       {
            Engine.Utility.Log.Error("m_label_ptguiyuan_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_ptguiyuan_number = fastComponent.FastGetComponent<UILabel>("ptguiyuan_number");
       if( null == m_label_ptguiyuan_number )
       {
            Engine.Utility.Log.Error("m_label_ptguiyuan_number 为空，请检查prefab是否缺乏组件");
       }
        m_label_PTguiyuan_dianjuanxiaohao = fastComponent.FastGetComponent<UILabel>("PTguiyuan_dianjuanxiaohao");
       if( null == m_label_PTguiyuan_dianjuanxiaohao )
       {
            Engine.Utility.Log.Error("m_label_PTguiyuan_dianjuanxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_zidongbuzu = fastComponent.FastGetComponent<UIButton>("zidongbuzu");
       if( null == m_btn_zidongbuzu )
       {
            Engine.Utility.Log.Error("m_btn_zidongbuzu 为空，请检查prefab是否缺乏组件");
       }
        m_label_description = fastComponent.FastGetComponent<UILabel>("description");
       if( null == m_label_description )
       {
            Engine.Utility.Log.Error("m_label_description 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_TitleCategoryScrollview = fastComponent.FastGetComponent<UIGridCreatorBase>("TitleCategoryScrollview");
       if( null == m_ctor_TitleCategoryScrollview )
       {
            Engine.Utility.Log.Error("m_ctor_TitleCategoryScrollview 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITitleCategorygrid = fastComponent.FastGetComponent<Transform>("UITitleCategorygrid");
       if( null == m_trans_UITitleCategorygrid )
       {
            Engine.Utility.Log.Error("m_trans_UITitleCategorygrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Sprite.gameObject).onClick = _onClick_Sprite_Btn;
        UIEventListener.Get(m_btn_BgCollider.gameObject).onClick = _onClick_BgCollider_Btn;
        UIEventListener.Get(m_btn_basePropContent.gameObject).onClick = _onClick_BasePropContent_Btn;
        UIEventListener.Get(m_btn_detailPropContent.gameObject).onClick = _onClick_DetailPropContent_Btn;
        UIEventListener.Get(m_btn_PropUItips_1.gameObject).onClick = _onClick_PropUItips_1_Btn;
        UIEventListener.Get(m_btn_btn_reset.gameObject).onClick = _onClick_Btn_reset_Btn;
        UIEventListener.Get(m_btn_BtnStatus.gameObject).onClick = _onClick_BtnStatus_Btn;
        UIEventListener.Get(m_btn_FashionTipsClose.gameObject).onClick = _onClick_FashionTipsClose_Btn;
        UIEventListener.Get(m_btn_btn_ViewProp.gameObject).onClick = _onClick_Btn_ViewProp_Btn;
        UIEventListener.Get(m_btn_btn_UseNumAdd.gameObject).onClick = _onClick_Btn_UseNumAdd_Btn;
        UIEventListener.Get(m_btn_btn_LimitedNumberAdd.gameObject).onClick = _onClick_Btn_LimitedNumberAdd_Btn;
        UIEventListener.Get(m_btn_btn_WearTitle.gameObject).onClick = _onClick_Btn_WearTitle_Btn;
        UIEventListener.Get(m_btn_btn_UseTitle.gameObject).onClick = _onClick_Btn_UseTitle_Btn;
        UIEventListener.Get(m_btn_btn_WearAndUseTitle.gameObject).onClick = _onClick_Btn_WearAndUseTitle_Btn;
        UIEventListener.Get(m_btn_btn_quxiao.gameObject).onClick = _onClick_Btn_quxiao_Btn;
        UIEventListener.Get(m_btn_btn_queding.gameObject).onClick = _onClick_Btn_queding_Btn;
        UIEventListener.Get(m_btn_ptguiyuan_icon.gameObject).onClick = _onClick_Ptguiyuan_icon_Btn;
        UIEventListener.Get(m_btn_zidongbuzu.gameObject).onClick = _onClick_Zidongbuzu_Btn;
    }

    void _onClick_Sprite_Btn(GameObject caster)
    {
        onClick_Sprite_Btn( caster );
    }

    void _onClick_BgCollider_Btn(GameObject caster)
    {
        onClick_BgCollider_Btn( caster );
    }

    void _onClick_BasePropContent_Btn(GameObject caster)
    {
        onClick_BasePropContent_Btn( caster );
    }

    void _onClick_DetailPropContent_Btn(GameObject caster)
    {
        onClick_DetailPropContent_Btn( caster );
    }

    void _onClick_PropUItips_1_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }

    void _onClick_Btn_reset_Btn(GameObject caster)
    {
        onClick_Btn_reset_Btn( caster );
    }

    void _onClick_BtnStatus_Btn(GameObject caster)
    {
        onClick_BtnStatus_Btn( caster );
    }

    void _onClick_FashionTipsClose_Btn(GameObject caster)
    {
        onClick_FashionTipsClose_Btn( caster );
    }

    void _onClick_Btn_ViewProp_Btn(GameObject caster)
    {
        onClick_Btn_ViewProp_Btn( caster );
    }

    void _onClick_Btn_UseNumAdd_Btn(GameObject caster)
    {
        onClick_Btn_UseNumAdd_Btn( caster );
    }

    void _onClick_Btn_LimitedNumberAdd_Btn(GameObject caster)
    {
        onClick_Btn_LimitedNumberAdd_Btn( caster );
    }

    void _onClick_Btn_WearTitle_Btn(GameObject caster)
    {
        onClick_Btn_WearTitle_Btn( caster );
    }

    void _onClick_Btn_UseTitle_Btn(GameObject caster)
    {
        onClick_Btn_UseTitle_Btn( caster );
    }

    void _onClick_Btn_WearAndUseTitle_Btn(GameObject caster)
    {
        onClick_Btn_WearAndUseTitle_Btn( caster );
    }

    void _onClick_Btn_quxiao_Btn(GameObject caster)
    {
        onClick_Btn_quxiao_Btn( caster );
    }

    void _onClick_Btn_queding_Btn(GameObject caster)
    {
        onClick_Btn_queding_Btn( caster );
    }

    void _onClick_Ptguiyuan_icon_Btn(GameObject caster)
    {
        onClick_Ptguiyuan_icon_Btn( caster );
    }

    void _onClick_Zidongbuzu_Btn(GameObject caster)
    {
        onClick_Zidongbuzu_Btn( caster );
    }


}
