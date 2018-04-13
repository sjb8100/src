//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetGaoJieGuiYuan: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UIWidget      m_widget_PetMessage;

    UIButton      m_btn_gjguiyuan_leftturn;

    UIButton      m_btn_gjguiyuan_rightturn;

    UISprite      m_sprite_gjguiyuan_icon;

    UILabel       m_label_gjguiyuan_name;

    UILabel       m_label_gjguiyuan_number;

    UILabel       m_label_GJguiyuan_dianjuanxiaohao;

    UIButton      m_btn_gjguiyuan_zidongbuzu;

    UILabel       m_label_gjguiyuan_xiaohaogold;

    UIButton      m_btn_gaojiguiyuan_gaojiguiyuan;

    UIWidget      m_widget_guiyuanqian;

    UIWidget      m_widget_SliderContaier;

    UILabel       m_label_gjguiyuan_growstate;

    UIWidget      m_widget_guiyuanhou;

    UILabel       m_label_gjguiyuanxin_growstate;

    UILabel       m_label_gjguiyuanyuan_growstate;

    UIWidget      m_widget_GuiyuanQianSliderContaier;

    UIWidget      m_widget_GuiyuanHouSliderContaier;

    UIButton      m_btn_baocuntianfu;


    //初始化控件变量
    protected override void InitControls()
    {
        m_widget_PetMessage = GetChildComponent<UIWidget>("PetMessage");
       if( null == m_widget_PetMessage )
       {
            Engine.Utility.Log.Error("m_widget_PetMessage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_gjguiyuan_leftturn = GetChildComponent<UIButton>("gjguiyuan_leftturn");
       if( null == m_btn_gjguiyuan_leftturn )
       {
            Engine.Utility.Log.Error("m_btn_gjguiyuan_leftturn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_gjguiyuan_rightturn = GetChildComponent<UIButton>("gjguiyuan_rightturn");
       if( null == m_btn_gjguiyuan_rightturn )
       {
            Engine.Utility.Log.Error("m_btn_gjguiyuan_rightturn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_gjguiyuan_icon = GetChildComponent<UISprite>("gjguiyuan_icon");
       if( null == m_sprite_gjguiyuan_icon )
       {
            Engine.Utility.Log.Error("m_sprite_gjguiyuan_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_gjguiyuan_name = GetChildComponent<UILabel>("gjguiyuan_name");
       if( null == m_label_gjguiyuan_name )
       {
            Engine.Utility.Log.Error("m_label_gjguiyuan_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_gjguiyuan_number = GetChildComponent<UILabel>("gjguiyuan_number");
       if( null == m_label_gjguiyuan_number )
       {
            Engine.Utility.Log.Error("m_label_gjguiyuan_number 为空，请检查prefab是否缺乏组件");
       }
        m_label_GJguiyuan_dianjuanxiaohao = GetChildComponent<UILabel>("GJguiyuan_dianjuanxiaohao");
       if( null == m_label_GJguiyuan_dianjuanxiaohao )
       {
            Engine.Utility.Log.Error("m_label_GJguiyuan_dianjuanxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_gjguiyuan_zidongbuzu = GetChildComponent<UIButton>("gjguiyuan_zidongbuzu");
       if( null == m_btn_gjguiyuan_zidongbuzu )
       {
            Engine.Utility.Log.Error("m_btn_gjguiyuan_zidongbuzu 为空，请检查prefab是否缺乏组件");
       }
        m_label_gjguiyuan_xiaohaogold = GetChildComponent<UILabel>("gjguiyuan_xiaohaogold");
       if( null == m_label_gjguiyuan_xiaohaogold )
       {
            Engine.Utility.Log.Error("m_label_gjguiyuan_xiaohaogold 为空，请检查prefab是否缺乏组件");
       }
        m_btn_gaojiguiyuan_gaojiguiyuan = GetChildComponent<UIButton>("gaojiguiyuan_gaojiguiyuan");
       if( null == m_btn_gaojiguiyuan_gaojiguiyuan )
       {
            Engine.Utility.Log.Error("m_btn_gaojiguiyuan_gaojiguiyuan 为空，请检查prefab是否缺乏组件");
       }
        m_widget_guiyuanqian = GetChildComponent<UIWidget>("guiyuanqian");
       if( null == m_widget_guiyuanqian )
       {
            Engine.Utility.Log.Error("m_widget_guiyuanqian 为空，请检查prefab是否缺乏组件");
       }
        m_widget_SliderContaier = GetChildComponent<UIWidget>("SliderContaier");
       if( null == m_widget_SliderContaier )
       {
            Engine.Utility.Log.Error("m_widget_SliderContaier 为空，请检查prefab是否缺乏组件");
       }
        m_label_gjguiyuan_growstate = GetChildComponent<UILabel>("gjguiyuan_growstate");
       if( null == m_label_gjguiyuan_growstate )
       {
            Engine.Utility.Log.Error("m_label_gjguiyuan_growstate 为空，请检查prefab是否缺乏组件");
       }
        m_widget_guiyuanhou = GetChildComponent<UIWidget>("guiyuanhou");
       if( null == m_widget_guiyuanhou )
       {
            Engine.Utility.Log.Error("m_widget_guiyuanhou 为空，请检查prefab是否缺乏组件");
       }
        m_label_gjguiyuanxin_growstate = GetChildComponent<UILabel>("gjguiyuanxin_growstate");
       if( null == m_label_gjguiyuanxin_growstate )
       {
            Engine.Utility.Log.Error("m_label_gjguiyuanxin_growstate 为空，请检查prefab是否缺乏组件");
       }
        m_label_gjguiyuanyuan_growstate = GetChildComponent<UILabel>("gjguiyuanyuan_growstate");
       if( null == m_label_gjguiyuanyuan_growstate )
       {
            Engine.Utility.Log.Error("m_label_gjguiyuanyuan_growstate 为空，请检查prefab是否缺乏组件");
       }
        m_widget_GuiyuanQianSliderContaier = GetChildComponent<UIWidget>("GuiyuanQianSliderContaier");
       if( null == m_widget_GuiyuanQianSliderContaier )
       {
            Engine.Utility.Log.Error("m_widget_GuiyuanQianSliderContaier 为空，请检查prefab是否缺乏组件");
       }
        m_widget_GuiyuanHouSliderContaier = GetChildComponent<UIWidget>("GuiyuanHouSliderContaier");
       if( null == m_widget_GuiyuanHouSliderContaier )
       {
            Engine.Utility.Log.Error("m_widget_GuiyuanHouSliderContaier 为空，请检查prefab是否缺乏组件");
       }
        m_btn_baocuntianfu = GetChildComponent<UIButton>("baocuntianfu");
       if( null == m_btn_baocuntianfu )
       {
            Engine.Utility.Log.Error("m_btn_baocuntianfu 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_gjguiyuan_leftturn.gameObject).onClick = _onClick_Gjguiyuan_leftturn_Btn;
        UIEventListener.Get(m_btn_gjguiyuan_rightturn.gameObject).onClick = _onClick_Gjguiyuan_rightturn_Btn;
        UIEventListener.Get(m_btn_gjguiyuan_zidongbuzu.gameObject).onClick = _onClick_Gjguiyuan_zidongbuzu_Btn;
        UIEventListener.Get(m_btn_gaojiguiyuan_gaojiguiyuan.gameObject).onClick = _onClick_Gaojiguiyuan_gaojiguiyuan_Btn;
        UIEventListener.Get(m_btn_baocuntianfu.gameObject).onClick = _onClick_Baocuntianfu_Btn;
    }

    void _onClick_Gjguiyuan_leftturn_Btn(GameObject caster)
    {
        onClick_Gjguiyuan_leftturn_Btn( caster );
    }

    void _onClick_Gjguiyuan_rightturn_Btn(GameObject caster)
    {
        onClick_Gjguiyuan_rightturn_Btn( caster );
    }

    void _onClick_Gjguiyuan_zidongbuzu_Btn(GameObject caster)
    {
        onClick_Gjguiyuan_zidongbuzu_Btn( caster );
    }

    void _onClick_Gaojiguiyuan_gaojiguiyuan_Btn(GameObject caster)
    {
        onClick_Gaojiguiyuan_gaojiguiyuan_Btn( caster );
    }

    void _onClick_Baocuntianfu_Btn(GameObject caster)
    {
        onClick_Baocuntianfu_Btn( caster );
    }


}
