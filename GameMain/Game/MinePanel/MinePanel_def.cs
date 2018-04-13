//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MinePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UIButton             m_btn_Normal;

    UIButton             m_btn_High;

    UIWidget             m_widget_ItemContent;

    UIButton             m_btn_zidongbuzu;

    UILabel              m_label_dianjuanxiaohao;

    UILabel              m_label_status;

    UIWidget             m_widget_NoMine;

    UIButton             m_btn_btn_NoMine_change;

    UIWidget             m_widget_Mine;

    UISprite             m_sprite_icon_find;

    UILabel              m_label_stonename;

    UILabel              m_label_grade;

    UISlider             m_slider_find_num;

    UILabel              m_label_all_income;

    UILabel              m_label_robbed_income;

    UILabel              m_label_reward_time;

    UIWidget             m_widget_status_NoMining;

    UIButton             m_btn_btn_mining;

    UIButton             m_btn_btn_change;

    UIWidget             m_widget_status_Mining;

    UIButton             m_btn_btn_nowgain;

    UIWidget             m_widget_status_MiningEnd;

    UIButton             m_btn_btn_gain;

    UILabel              m_label_GrabNum;

    UIButton             m_btn_btn_battlelist;

    UIButton             m_btn_Mining;

    UIButton             m_btn_GrabMine;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Normal = fastComponent.FastGetComponent<UIButton>("Normal");
       if( null == m_btn_Normal )
       {
            Engine.Utility.Log.Error("m_btn_Normal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_High = fastComponent.FastGetComponent<UIButton>("High");
       if( null == m_btn_High )
       {
            Engine.Utility.Log.Error("m_btn_High 为空，请检查prefab是否缺乏组件");
       }
        m_widget_ItemContent = fastComponent.FastGetComponent<UIWidget>("ItemContent");
       if( null == m_widget_ItemContent )
       {
            Engine.Utility.Log.Error("m_widget_ItemContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_zidongbuzu = fastComponent.FastGetComponent<UIButton>("zidongbuzu");
       if( null == m_btn_zidongbuzu )
       {
            Engine.Utility.Log.Error("m_btn_zidongbuzu 为空，请检查prefab是否缺乏组件");
       }
        m_label_dianjuanxiaohao = fastComponent.FastGetComponent<UILabel>("dianjuanxiaohao");
       if( null == m_label_dianjuanxiaohao )
       {
            Engine.Utility.Log.Error("m_label_dianjuanxiaohao 为空，请检查prefab是否缺乏组件");
       }
        m_label_status = fastComponent.FastGetComponent<UILabel>("status");
       if( null == m_label_status )
       {
            Engine.Utility.Log.Error("m_label_status 为空，请检查prefab是否缺乏组件");
       }
        m_widget_NoMine = fastComponent.FastGetComponent<UIWidget>("NoMine");
       if( null == m_widget_NoMine )
       {
            Engine.Utility.Log.Error("m_widget_NoMine 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_NoMine_change = fastComponent.FastGetComponent<UIButton>("btn_NoMine_change");
       if( null == m_btn_btn_NoMine_change )
       {
            Engine.Utility.Log.Error("m_btn_btn_NoMine_change 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Mine = fastComponent.FastGetComponent<UIWidget>("Mine");
       if( null == m_widget_Mine )
       {
            Engine.Utility.Log.Error("m_widget_Mine 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_icon_find = fastComponent.FastGetComponent<UISprite>("icon_find");
       if( null == m_sprite_icon_find )
       {
            Engine.Utility.Log.Error("m_sprite_icon_find 为空，请检查prefab是否缺乏组件");
       }
        m_label_stonename = fastComponent.FastGetComponent<UILabel>("stonename");
       if( null == m_label_stonename )
       {
            Engine.Utility.Log.Error("m_label_stonename 为空，请检查prefab是否缺乏组件");
       }
        m_label_grade = fastComponent.FastGetComponent<UILabel>("grade");
       if( null == m_label_grade )
       {
            Engine.Utility.Log.Error("m_label_grade 为空，请检查prefab是否缺乏组件");
       }
        m_slider_find_num = fastComponent.FastGetComponent<UISlider>("find_num");
       if( null == m_slider_find_num )
       {
            Engine.Utility.Log.Error("m_slider_find_num 为空，请检查prefab是否缺乏组件");
       }
        m_label_all_income = fastComponent.FastGetComponent<UILabel>("all_income");
       if( null == m_label_all_income )
       {
            Engine.Utility.Log.Error("m_label_all_income 为空，请检查prefab是否缺乏组件");
       }
        m_label_robbed_income = fastComponent.FastGetComponent<UILabel>("robbed_income");
       if( null == m_label_robbed_income )
       {
            Engine.Utility.Log.Error("m_label_robbed_income 为空，请检查prefab是否缺乏组件");
       }
        m_label_reward_time = fastComponent.FastGetComponent<UILabel>("reward_time");
       if( null == m_label_reward_time )
       {
            Engine.Utility.Log.Error("m_label_reward_time 为空，请检查prefab是否缺乏组件");
       }
        m_widget_status_NoMining = fastComponent.FastGetComponent<UIWidget>("status_NoMining");
       if( null == m_widget_status_NoMining )
       {
            Engine.Utility.Log.Error("m_widget_status_NoMining 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_mining = fastComponent.FastGetComponent<UIButton>("btn_mining");
       if( null == m_btn_btn_mining )
       {
            Engine.Utility.Log.Error("m_btn_btn_mining 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_change = fastComponent.FastGetComponent<UIButton>("btn_change");
       if( null == m_btn_btn_change )
       {
            Engine.Utility.Log.Error("m_btn_btn_change 为空，请检查prefab是否缺乏组件");
       }
        m_widget_status_Mining = fastComponent.FastGetComponent<UIWidget>("status_Mining");
       if( null == m_widget_status_Mining )
       {
            Engine.Utility.Log.Error("m_widget_status_Mining 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_nowgain = fastComponent.FastGetComponent<UIButton>("btn_nowgain");
       if( null == m_btn_btn_nowgain )
       {
            Engine.Utility.Log.Error("m_btn_btn_nowgain 为空，请检查prefab是否缺乏组件");
       }
        m_widget_status_MiningEnd = fastComponent.FastGetComponent<UIWidget>("status_MiningEnd");
       if( null == m_widget_status_MiningEnd )
       {
            Engine.Utility.Log.Error("m_widget_status_MiningEnd 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_gain = fastComponent.FastGetComponent<UIButton>("btn_gain");
       if( null == m_btn_btn_gain )
       {
            Engine.Utility.Log.Error("m_btn_btn_gain 为空，请检查prefab是否缺乏组件");
       }
        m_label_GrabNum = fastComponent.FastGetComponent<UILabel>("GrabNum");
       if( null == m_label_GrabNum )
       {
            Engine.Utility.Log.Error("m_label_GrabNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_battlelist = fastComponent.FastGetComponent<UIButton>("btn_battlelist");
       if( null == m_btn_btn_battlelist )
       {
            Engine.Utility.Log.Error("m_btn_btn_battlelist 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Mining = fastComponent.FastGetComponent<UIButton>("Mining");
       if( null == m_btn_Mining )
       {
            Engine.Utility.Log.Error("m_btn_Mining 为空，请检查prefab是否缺乏组件");
       }
        m_btn_GrabMine = fastComponent.FastGetComponent<UIButton>("GrabMine");
       if( null == m_btn_GrabMine )
       {
            Engine.Utility.Log.Error("m_btn_GrabMine 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_Normal.gameObject).onClick = _onClick_Normal_Btn;
        UIEventListener.Get(m_btn_High.gameObject).onClick = _onClick_High_Btn;
        UIEventListener.Get(m_btn_zidongbuzu.gameObject).onClick = _onClick_Zidongbuzu_Btn;
        UIEventListener.Get(m_btn_btn_NoMine_change.gameObject).onClick = _onClick_Btn_NoMine_change_Btn;
        UIEventListener.Get(m_btn_btn_mining.gameObject).onClick = _onClick_Btn_mining_Btn;
        UIEventListener.Get(m_btn_btn_change.gameObject).onClick = _onClick_Btn_change_Btn;
        UIEventListener.Get(m_btn_btn_nowgain.gameObject).onClick = _onClick_Btn_nowgain_Btn;
        UIEventListener.Get(m_btn_btn_gain.gameObject).onClick = _onClick_Btn_gain_Btn;
        UIEventListener.Get(m_btn_btn_battlelist.gameObject).onClick = _onClick_Btn_battlelist_Btn;
        UIEventListener.Get(m_btn_Mining.gameObject).onClick = _onClick_Mining_Btn;
        UIEventListener.Get(m_btn_GrabMine.gameObject).onClick = _onClick_GrabMine_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Normal_Btn(GameObject caster)
    {
        onClick_Normal_Btn( caster );
    }

    void _onClick_High_Btn(GameObject caster)
    {
        onClick_High_Btn( caster );
    }

    void _onClick_Zidongbuzu_Btn(GameObject caster)
    {
        onClick_Zidongbuzu_Btn( caster );
    }

    void _onClick_Btn_NoMine_change_Btn(GameObject caster)
    {
        onClick_Btn_NoMine_change_Btn( caster );
    }

    void _onClick_Btn_mining_Btn(GameObject caster)
    {
        onClick_Btn_mining_Btn( caster );
    }

    void _onClick_Btn_change_Btn(GameObject caster)
    {
        onClick_Btn_change_Btn( caster );
    }

    void _onClick_Btn_nowgain_Btn(GameObject caster)
    {
        onClick_Btn_nowgain_Btn( caster );
    }

    void _onClick_Btn_gain_Btn(GameObject caster)
    {
        onClick_Btn_gain_Btn( caster );
    }

    void _onClick_Btn_battlelist_Btn(GameObject caster)
    {
        onClick_Btn_battlelist_Btn( caster );
    }

    void _onClick_Mining_Btn(GameObject caster)
    {
        onClick_Mining_Btn( caster );
    }

    void _onClick_GrabMine_Btn(GameObject caster)
    {
        onClick_GrabMine_Btn( caster );
    }


}
