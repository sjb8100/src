//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MiningPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_Mine_Name;

    UILabel              m_label_all_income;

    UILabel              m_label_reward_time;

    UITexture            m__icon;

    UILabel              m_label_name;

    UILabel              m_label_num;

    UILabel              m_label_multiple;

    UIButton             m_btn_btn_quxiao;

    UIButton             m_btn_btn_queding;


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
        m_label_Mine_Name = fastComponent.FastGetComponent<UILabel>("Mine_Name");
       if( null == m_label_Mine_Name )
       {
            Engine.Utility.Log.Error("m_label_Mine_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_all_income = fastComponent.FastGetComponent<UILabel>("all_income");
       if( null == m_label_all_income )
       {
            Engine.Utility.Log.Error("m_label_all_income 为空，请检查prefab是否缺乏组件");
       }
        m_label_reward_time = fastComponent.FastGetComponent<UILabel>("reward_time");
       if( null == m_label_reward_time )
       {
            Engine.Utility.Log.Error("m_label_reward_time 为空，请检查prefab是否缺乏组件");
       }
        m__icon = fastComponent.FastGetComponent<UITexture>("icon");
       if( null == m__icon )
       {
            Engine.Utility.Log.Error("m__icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_num = fastComponent.FastGetComponent<UILabel>("num");
       if( null == m_label_num )
       {
            Engine.Utility.Log.Error("m_label_num 为空，请检查prefab是否缺乏组件");
       }
        m_label_multiple = fastComponent.FastGetComponent<UILabel>("multiple");
       if( null == m_label_multiple )
       {
            Engine.Utility.Log.Error("m_label_multiple 为空，请检查prefab是否缺乏组件");
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
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_btn_quxiao.gameObject).onClick = _onClick_Btn_quxiao_Btn;
        UIEventListener.Get(m_btn_btn_queding.gameObject).onClick = _onClick_Btn_queding_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Btn_quxiao_Btn(GameObject caster)
    {
        onClick_Btn_quxiao_Btn( caster );
    }

    void _onClick_Btn_queding_Btn(GameObject caster)
    {
        onClick_Btn_queding_Btn( caster );
    }


}
