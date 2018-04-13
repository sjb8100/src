//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetAddPointPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UITexture            m__jiadian_icon;

    UILabel              m_label_jiadian_petshowname;

    UILabel              m_label_jiadian_level;

    UISprite             m_sprite_jiadian_status;

    UILabel              m_label_jiadian_qixue;

    UILabel              m_label_jiadian_qixueadd;

    UILabel              m_label_jiadian_wuligongji;

    UILabel              m_label_jiadian_wugongadd;

    UILabel              m_label_jiadian_fashugongji;

    UILabel              m_label_jiadian_fagongadd;

    UILabel              m_label_jiadian_wulifangyu;

    UILabel              m_label_jiadian_wufangadd;

    UILabel              m_label_jiadian_fashufangyu;

    UILabel              m_label_jiadian_fafangadd;

    UILabel              m_label_jiadian_wulizhimingyiji;

    UILabel              m_label_jiadian_wuzhiadd;

    UILabel              m_label_jiadian_fashuzhimingyiji;

    UILabel              m_label_jiadian_fazhiadd;

    UILabel              m_label_jiadian_mingzhong;

    UILabel              m_label_jiadian_mingzhongadd;

    UILabel              m_label_jiadian_shanbi;

    UILabel              m_label_jiadian_shanbiadd;

    UILabel              m_label_attackType;

    UIButton             m_btn_jiadianfangan;

    UIButton             m_btn_help;

    UILabel              m_label_jiadian_point;

    UISlider             m_slider_jiadian_liliang;

    UIButton             m_btn_jiadian_liliangless;

    UIButton             m_btn_jiadian_liliangadd;

    UILabel              m_label_jiadian_liliangnowpoint;

    UILabel              m_label_jiadian_liliangaddpoint;

    UISlider             m_slider_jiadian_minjie;

    UIButton             m_btn_jiadian_minjieless;

    UIButton             m_btn_jiadian_minjieadd;

    UILabel              m_label_jiadian_minjienowpoint;

    UILabel              m_label_jiadian_minjieaddpoint;

    UIButton             m_btn_jiadian_zhililess;

    UIButton             m_btn_jiadian_zhiliadd;

    UILabel              m_label_jiadian_zhilinowpoint;

    UILabel              m_label_jiadian_zhiliaddpoint;

    UISlider             m_slider_jiadian_zhili;

    UISlider             m_slider_jiadian_tili;

    UIButton             m_btn_jiadian_tililess;

    UIButton             m_btn_jiadian_tiliadd;

    UILabel              m_label_jiadian_tilinowpoint;

    UILabel              m_label_jiadian_tiliaddpoint;

    UISlider             m_slider_jiadian_jingshen;

    UIButton             m_btn_jiadian_jingshenless;

    UIButton             m_btn_jiadian_jingshenadd;

    UILabel              m_label_jiadian_jingshennowpoint;

    UILabel              m_label_jiadian_jingshenaddpoint;

    UIButton             m_btn_chongzhishuxing;

    UIButton             m_btn_quedingjiadian;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m__jiadian_icon = fastComponent.FastGetComponent<UITexture>("jiadian_icon");
       if( null == m__jiadian_icon )
       {
            Engine.Utility.Log.Error("m__jiadian_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_petshowname = fastComponent.FastGetComponent<UILabel>("jiadian_petshowname");
       if( null == m_label_jiadian_petshowname )
       {
            Engine.Utility.Log.Error("m_label_jiadian_petshowname 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_level = fastComponent.FastGetComponent<UILabel>("jiadian_level");
       if( null == m_label_jiadian_level )
       {
            Engine.Utility.Log.Error("m_label_jiadian_level 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_jiadian_status = fastComponent.FastGetComponent<UISprite>("jiadian_status");
       if( null == m_sprite_jiadian_status )
       {
            Engine.Utility.Log.Error("m_sprite_jiadian_status 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_qixue = fastComponent.FastGetComponent<UILabel>("jiadian_qixue");
       if( null == m_label_jiadian_qixue )
       {
            Engine.Utility.Log.Error("m_label_jiadian_qixue 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_qixueadd = fastComponent.FastGetComponent<UILabel>("jiadian_qixueadd");
       if( null == m_label_jiadian_qixueadd )
       {
            Engine.Utility.Log.Error("m_label_jiadian_qixueadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_wuligongji = fastComponent.FastGetComponent<UILabel>("jiadian_wuligongji");
       if( null == m_label_jiadian_wuligongji )
       {
            Engine.Utility.Log.Error("m_label_jiadian_wuligongji 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_wugongadd = fastComponent.FastGetComponent<UILabel>("jiadian_wugongadd");
       if( null == m_label_jiadian_wugongadd )
       {
            Engine.Utility.Log.Error("m_label_jiadian_wugongadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_fashugongji = fastComponent.FastGetComponent<UILabel>("jiadian_fashugongji");
       if( null == m_label_jiadian_fashugongji )
       {
            Engine.Utility.Log.Error("m_label_jiadian_fashugongji 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_fagongadd = fastComponent.FastGetComponent<UILabel>("jiadian_fagongadd");
       if( null == m_label_jiadian_fagongadd )
       {
            Engine.Utility.Log.Error("m_label_jiadian_fagongadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_wulifangyu = fastComponent.FastGetComponent<UILabel>("jiadian_wulifangyu");
       if( null == m_label_jiadian_wulifangyu )
       {
            Engine.Utility.Log.Error("m_label_jiadian_wulifangyu 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_wufangadd = fastComponent.FastGetComponent<UILabel>("jiadian_wufangadd");
       if( null == m_label_jiadian_wufangadd )
       {
            Engine.Utility.Log.Error("m_label_jiadian_wufangadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_fashufangyu = fastComponent.FastGetComponent<UILabel>("jiadian_fashufangyu");
       if( null == m_label_jiadian_fashufangyu )
       {
            Engine.Utility.Log.Error("m_label_jiadian_fashufangyu 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_fafangadd = fastComponent.FastGetComponent<UILabel>("jiadian_fafangadd");
       if( null == m_label_jiadian_fafangadd )
       {
            Engine.Utility.Log.Error("m_label_jiadian_fafangadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_wulizhimingyiji = fastComponent.FastGetComponent<UILabel>("jiadian_wulizhimingyiji");
       if( null == m_label_jiadian_wulizhimingyiji )
       {
            Engine.Utility.Log.Error("m_label_jiadian_wulizhimingyiji 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_wuzhiadd = fastComponent.FastGetComponent<UILabel>("jiadian_wuzhiadd");
       if( null == m_label_jiadian_wuzhiadd )
       {
            Engine.Utility.Log.Error("m_label_jiadian_wuzhiadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_fashuzhimingyiji = fastComponent.FastGetComponent<UILabel>("jiadian_fashuzhimingyiji");
       if( null == m_label_jiadian_fashuzhimingyiji )
       {
            Engine.Utility.Log.Error("m_label_jiadian_fashuzhimingyiji 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_fazhiadd = fastComponent.FastGetComponent<UILabel>("jiadian_fazhiadd");
       if( null == m_label_jiadian_fazhiadd )
       {
            Engine.Utility.Log.Error("m_label_jiadian_fazhiadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_mingzhong = fastComponent.FastGetComponent<UILabel>("jiadian_mingzhong");
       if( null == m_label_jiadian_mingzhong )
       {
            Engine.Utility.Log.Error("m_label_jiadian_mingzhong 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_mingzhongadd = fastComponent.FastGetComponent<UILabel>("jiadian_mingzhongadd");
       if( null == m_label_jiadian_mingzhongadd )
       {
            Engine.Utility.Log.Error("m_label_jiadian_mingzhongadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_shanbi = fastComponent.FastGetComponent<UILabel>("jiadian_shanbi");
       if( null == m_label_jiadian_shanbi )
       {
            Engine.Utility.Log.Error("m_label_jiadian_shanbi 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_shanbiadd = fastComponent.FastGetComponent<UILabel>("jiadian_shanbiadd");
       if( null == m_label_jiadian_shanbiadd )
       {
            Engine.Utility.Log.Error("m_label_jiadian_shanbiadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_attackType = fastComponent.FastGetComponent<UILabel>("attackType");
       if( null == m_label_attackType )
       {
            Engine.Utility.Log.Error("m_label_attackType 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadianfangan = fastComponent.FastGetComponent<UIButton>("jiadianfangan");
       if( null == m_btn_jiadianfangan )
       {
            Engine.Utility.Log.Error("m_btn_jiadianfangan 为空，请检查prefab是否缺乏组件");
       }
        m_btn_help = fastComponent.FastGetComponent<UIButton>("help");
       if( null == m_btn_help )
       {
            Engine.Utility.Log.Error("m_btn_help 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_point = fastComponent.FastGetComponent<UILabel>("jiadian_point");
       if( null == m_label_jiadian_point )
       {
            Engine.Utility.Log.Error("m_label_jiadian_point 为空，请检查prefab是否缺乏组件");
       }
        m_slider_jiadian_liliang = fastComponent.FastGetComponent<UISlider>("jiadian_liliang");
       if( null == m_slider_jiadian_liliang )
       {
            Engine.Utility.Log.Error("m_slider_jiadian_liliang 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_liliangless = fastComponent.FastGetComponent<UIButton>("jiadian_liliangless");
       if( null == m_btn_jiadian_liliangless )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_liliangless 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_liliangadd = fastComponent.FastGetComponent<UIButton>("jiadian_liliangadd");
       if( null == m_btn_jiadian_liliangadd )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_liliangadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_liliangnowpoint = fastComponent.FastGetComponent<UILabel>("jiadian_liliangnowpoint");
       if( null == m_label_jiadian_liliangnowpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_liliangnowpoint 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_liliangaddpoint = fastComponent.FastGetComponent<UILabel>("jiadian_liliangaddpoint");
       if( null == m_label_jiadian_liliangaddpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_liliangaddpoint 为空，请检查prefab是否缺乏组件");
       }
        m_slider_jiadian_minjie = fastComponent.FastGetComponent<UISlider>("jiadian_minjie");
       if( null == m_slider_jiadian_minjie )
       {
            Engine.Utility.Log.Error("m_slider_jiadian_minjie 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_minjieless = fastComponent.FastGetComponent<UIButton>("jiadian_minjieless");
       if( null == m_btn_jiadian_minjieless )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_minjieless 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_minjieadd = fastComponent.FastGetComponent<UIButton>("jiadian_minjieadd");
       if( null == m_btn_jiadian_minjieadd )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_minjieadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_minjienowpoint = fastComponent.FastGetComponent<UILabel>("jiadian_minjienowpoint");
       if( null == m_label_jiadian_minjienowpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_minjienowpoint 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_minjieaddpoint = fastComponent.FastGetComponent<UILabel>("jiadian_minjieaddpoint");
       if( null == m_label_jiadian_minjieaddpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_minjieaddpoint 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_zhililess = fastComponent.FastGetComponent<UIButton>("jiadian_zhililess");
       if( null == m_btn_jiadian_zhililess )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_zhililess 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_zhiliadd = fastComponent.FastGetComponent<UIButton>("jiadian_zhiliadd");
       if( null == m_btn_jiadian_zhiliadd )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_zhiliadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_zhilinowpoint = fastComponent.FastGetComponent<UILabel>("jiadian_zhilinowpoint");
       if( null == m_label_jiadian_zhilinowpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_zhilinowpoint 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_zhiliaddpoint = fastComponent.FastGetComponent<UILabel>("jiadian_zhiliaddpoint");
       if( null == m_label_jiadian_zhiliaddpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_zhiliaddpoint 为空，请检查prefab是否缺乏组件");
       }
        m_slider_jiadian_zhili = fastComponent.FastGetComponent<UISlider>("jiadian_zhili");
       if( null == m_slider_jiadian_zhili )
       {
            Engine.Utility.Log.Error("m_slider_jiadian_zhili 为空，请检查prefab是否缺乏组件");
       }
        m_slider_jiadian_tili = fastComponent.FastGetComponent<UISlider>("jiadian_tili");
       if( null == m_slider_jiadian_tili )
       {
            Engine.Utility.Log.Error("m_slider_jiadian_tili 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_tililess = fastComponent.FastGetComponent<UIButton>("jiadian_tililess");
       if( null == m_btn_jiadian_tililess )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_tililess 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_tiliadd = fastComponent.FastGetComponent<UIButton>("jiadian_tiliadd");
       if( null == m_btn_jiadian_tiliadd )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_tiliadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_tilinowpoint = fastComponent.FastGetComponent<UILabel>("jiadian_tilinowpoint");
       if( null == m_label_jiadian_tilinowpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_tilinowpoint 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_tiliaddpoint = fastComponent.FastGetComponent<UILabel>("jiadian_tiliaddpoint");
       if( null == m_label_jiadian_tiliaddpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_tiliaddpoint 为空，请检查prefab是否缺乏组件");
       }
        m_slider_jiadian_jingshen = fastComponent.FastGetComponent<UISlider>("jiadian_jingshen");
       if( null == m_slider_jiadian_jingshen )
       {
            Engine.Utility.Log.Error("m_slider_jiadian_jingshen 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_jingshenless = fastComponent.FastGetComponent<UIButton>("jiadian_jingshenless");
       if( null == m_btn_jiadian_jingshenless )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_jingshenless 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiadian_jingshenadd = fastComponent.FastGetComponent<UIButton>("jiadian_jingshenadd");
       if( null == m_btn_jiadian_jingshenadd )
       {
            Engine.Utility.Log.Error("m_btn_jiadian_jingshenadd 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_jingshennowpoint = fastComponent.FastGetComponent<UILabel>("jiadian_jingshennowpoint");
       if( null == m_label_jiadian_jingshennowpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_jingshennowpoint 为空，请检查prefab是否缺乏组件");
       }
        m_label_jiadian_jingshenaddpoint = fastComponent.FastGetComponent<UILabel>("jiadian_jingshenaddpoint");
       if( null == m_label_jiadian_jingshenaddpoint )
       {
            Engine.Utility.Log.Error("m_label_jiadian_jingshenaddpoint 为空，请检查prefab是否缺乏组件");
       }
        m_btn_chongzhishuxing = fastComponent.FastGetComponent<UIButton>("chongzhishuxing");
       if( null == m_btn_chongzhishuxing )
       {
            Engine.Utility.Log.Error("m_btn_chongzhishuxing 为空，请检查prefab是否缺乏组件");
       }
        m_btn_quedingjiadian = fastComponent.FastGetComponent<UIButton>("quedingjiadian");
       if( null == m_btn_quedingjiadian )
       {
            Engine.Utility.Log.Error("m_btn_quedingjiadian 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_jiadianfangan.gameObject).onClick = _onClick_Jiadianfangan_Btn;
        UIEventListener.Get(m_btn_help.gameObject).onClick = _onClick_Help_Btn;
        UIEventListener.Get(m_btn_jiadian_liliangless.gameObject).onClick = _onClick_Jiadian_liliangless_Btn;
        UIEventListener.Get(m_btn_jiadian_liliangadd.gameObject).onClick = _onClick_Jiadian_liliangadd_Btn;
        UIEventListener.Get(m_btn_jiadian_minjieless.gameObject).onClick = _onClick_Jiadian_minjieless_Btn;
        UIEventListener.Get(m_btn_jiadian_minjieadd.gameObject).onClick = _onClick_Jiadian_minjieadd_Btn;
        UIEventListener.Get(m_btn_jiadian_zhililess.gameObject).onClick = _onClick_Jiadian_zhililess_Btn;
        UIEventListener.Get(m_btn_jiadian_zhiliadd.gameObject).onClick = _onClick_Jiadian_zhiliadd_Btn;
        UIEventListener.Get(m_btn_jiadian_tililess.gameObject).onClick = _onClick_Jiadian_tililess_Btn;
        UIEventListener.Get(m_btn_jiadian_tiliadd.gameObject).onClick = _onClick_Jiadian_tiliadd_Btn;
        UIEventListener.Get(m_btn_jiadian_jingshenless.gameObject).onClick = _onClick_Jiadian_jingshenless_Btn;
        UIEventListener.Get(m_btn_jiadian_jingshenadd.gameObject).onClick = _onClick_Jiadian_jingshenadd_Btn;
        UIEventListener.Get(m_btn_chongzhishuxing.gameObject).onClick = _onClick_Chongzhishuxing_Btn;
        UIEventListener.Get(m_btn_quedingjiadian.gameObject).onClick = _onClick_Quedingjiadian_Btn;
    }

    void _onClick_Jiadianfangan_Btn(GameObject caster)
    {
        onClick_Jiadianfangan_Btn( caster );
    }

    void _onClick_Help_Btn(GameObject caster)
    {
        onClick_Help_Btn( caster );
    }

    void _onClick_Jiadian_liliangless_Btn(GameObject caster)
    {
        onClick_Jiadian_liliangless_Btn( caster );
    }

    void _onClick_Jiadian_liliangadd_Btn(GameObject caster)
    {
        onClick_Jiadian_liliangadd_Btn( caster );
    }

    void _onClick_Jiadian_minjieless_Btn(GameObject caster)
    {
        onClick_Jiadian_minjieless_Btn( caster );
    }

    void _onClick_Jiadian_minjieadd_Btn(GameObject caster)
    {
        onClick_Jiadian_minjieadd_Btn( caster );
    }

    void _onClick_Jiadian_zhililess_Btn(GameObject caster)
    {
        onClick_Jiadian_zhililess_Btn( caster );
    }

    void _onClick_Jiadian_zhiliadd_Btn(GameObject caster)
    {
        onClick_Jiadian_zhiliadd_Btn( caster );
    }

    void _onClick_Jiadian_tililess_Btn(GameObject caster)
    {
        onClick_Jiadian_tililess_Btn( caster );
    }

    void _onClick_Jiadian_tiliadd_Btn(GameObject caster)
    {
        onClick_Jiadian_tiliadd_Btn( caster );
    }

    void _onClick_Jiadian_jingshenless_Btn(GameObject caster)
    {
        onClick_Jiadian_jingshenless_Btn( caster );
    }

    void _onClick_Jiadian_jingshenadd_Btn(GameObject caster)
    {
        onClick_Jiadian_jingshenadd_Btn( caster );
    }

    void _onClick_Chongzhishuxing_Btn(GameObject caster)
    {
        onClick_Chongzhishuxing_Btn( caster );
    }

    void _onClick_Quedingjiadian_Btn(GameObject caster)
    {
        onClick_Quedingjiadian_Btn( caster );
    }


}
