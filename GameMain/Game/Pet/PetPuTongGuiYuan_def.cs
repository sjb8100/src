//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetPuTongGuiYuan: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UIButton      m_btn_ptguiyuan_leftturn;

    UIButton      m_btn_ptguiyuan_rightturn;

    UILabel       m_label_ptguiyuan_growstate;

    UISprite      m_sprite_ptguiyuan_icon;

    UILabel       m_label_ptguiyuan_name;

    UILabel       m_label_ptguiyuan_number;

    UILabel       m_label_PTguiyuan_dianjuanxiaohao;

    UIButton      m_btn_ptguiyuan_wanmei;

    UIButton      m_btn_ptguiyuan_zidongbuzu;

    UILabel       m_label_ptguiyuan_xiaohaogold;

    UIButton      m_btn_kaishiguiyuan;

    UIWidget      m_widget_SliderContaier;

    UIWidget      m_widget_PetMessage;

    UIButton      m_btn_tingzhiguiyuan;


    //初始化控件变量
    protected override void InitControls()
    {
        m_btn_ptguiyuan_leftturn = GetChildComponent<UIButton>("ptguiyuan_leftturn");
       if( null == m_btn_ptguiyuan_leftturn )
       {
            Engine.Utility.Log.Error("m_btn_ptguiyuan_leftturn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ptguiyuan_rightturn = GetChildComponent<UIButton>("ptguiyuan_rightturn");
       if( null == m_btn_ptguiyuan_rightturn )
       {
            Engine.Utility.Log.Error("m_btn_ptguiyuan_rightturn 为空，请检查prefab是否缺乏组件");
       }
        m_label_ptguiyuan_growstate = GetChildComponent<UILabel>("ptguiyuan_growstate");
       if( null == m_label_ptguiyuan_growstate )
       {
            Engine.Utility.Log.Error("m_label_ptguiyuan_growstate 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_ptguiyuan_icon = GetChildComponent<UISprite>("ptguiyuan_icon");
       if( null == m_sprite_ptguiyuan_icon )
       {
            Engine.Utility.Log.Error("m_sprite_ptguiyuan_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_ptguiyuan_name = GetChildComponent<UILabel>("ptguiyuan_name");
       if( null == m_label_ptguiyuan_name )
       {
            Engine.Utility.Log.Error("m_label_ptguiyuan_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_ptguiyuan_number = GetChildComponent<UILabel>("ptguiyuan_number");
       if( null == m_label_ptguiyuan_number )
       {
            Engine.Utility.Log.Error("m_label_ptguiyuan_number 为空，请检查prefab是否缺乏组件");
       }
        m_label_PTguiyuan_dianjuanxiaohao = GetChildComponent<UILabel>("PTguiyuan_dianjuanxiaohao");
       if( null == m_label_PTguiyuan_dianjuanxiaohao )
       {
            Engine.Utility.Log.Error("m_label_PTguiyuan_dianjuanxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ptguiyuan_wanmei = GetChildComponent<UIButton>("ptguiyuan_wanmei");
       if( null == m_btn_ptguiyuan_wanmei )
       {
            Engine.Utility.Log.Error("m_btn_ptguiyuan_wanmei 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ptguiyuan_zidongbuzu = GetChildComponent<UIButton>("ptguiyuan_zidongbuzu");
       if( null == m_btn_ptguiyuan_zidongbuzu )
       {
            Engine.Utility.Log.Error("m_btn_ptguiyuan_zidongbuzu 为空，请检查prefab是否缺乏组件");
       }
        m_label_ptguiyuan_xiaohaogold = GetChildComponent<UILabel>("ptguiyuan_xiaohaogold");
       if( null == m_label_ptguiyuan_xiaohaogold )
       {
            Engine.Utility.Log.Error("m_label_ptguiyuan_xiaohaogold 为空，请检查prefab是否缺乏组件");
       }
        m_btn_kaishiguiyuan = GetChildComponent<UIButton>("kaishiguiyuan");
       if( null == m_btn_kaishiguiyuan )
       {
            Engine.Utility.Log.Error("m_btn_kaishiguiyuan 为空，请检查prefab是否缺乏组件");
       }
        m_widget_SliderContaier = GetChildComponent<UIWidget>("SliderContaier");
       if( null == m_widget_SliderContaier )
       {
            Engine.Utility.Log.Error("m_widget_SliderContaier 为空，请检查prefab是否缺乏组件");
       }
        m_widget_PetMessage = GetChildComponent<UIWidget>("PetMessage");
       if( null == m_widget_PetMessage )
       {
            Engine.Utility.Log.Error("m_widget_PetMessage 为空，请检查prefab是否缺乏组件");
       }
        m_btn_tingzhiguiyuan = GetChildComponent<UIButton>("tingzhiguiyuan");
       if( null == m_btn_tingzhiguiyuan )
       {
            Engine.Utility.Log.Error("m_btn_tingzhiguiyuan 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_ptguiyuan_leftturn.gameObject).onClick = _onClick_Ptguiyuan_leftturn_Btn;
        UIEventListener.Get(m_btn_ptguiyuan_rightturn.gameObject).onClick = _onClick_Ptguiyuan_rightturn_Btn;
        UIEventListener.Get(m_btn_ptguiyuan_wanmei.gameObject).onClick = _onClick_Ptguiyuan_wanmei_Btn;
        UIEventListener.Get(m_btn_ptguiyuan_zidongbuzu.gameObject).onClick = _onClick_Ptguiyuan_zidongbuzu_Btn;
        UIEventListener.Get(m_btn_kaishiguiyuan.gameObject).onClick = _onClick_Kaishiguiyuan_Btn;
        UIEventListener.Get(m_btn_tingzhiguiyuan.gameObject).onClick = _onClick_Tingzhiguiyuan_Btn;
    }

    void _onClick_Ptguiyuan_leftturn_Btn(GameObject caster)
    {
        onClick_Ptguiyuan_leftturn_Btn( caster );
    }

    void _onClick_Ptguiyuan_rightturn_Btn(GameObject caster)
    {
        onClick_Ptguiyuan_rightturn_Btn( caster );
    }

    void _onClick_Ptguiyuan_wanmei_Btn(GameObject caster)
    {
        onClick_Ptguiyuan_wanmei_Btn( caster );
    }

    void _onClick_Ptguiyuan_zidongbuzu_Btn(GameObject caster)
    {
        onClick_Ptguiyuan_zidongbuzu_Btn( caster );
    }

    void _onClick_Kaishiguiyuan_Btn(GameObject caster)
    {
        onClick_Kaishiguiyuan_Btn( caster );
    }

    void _onClick_Tingzhiguiyuan_Btn(GameObject caster)
    {
        onClick_Tingzhiguiyuan_Btn( caster );
    }


}
