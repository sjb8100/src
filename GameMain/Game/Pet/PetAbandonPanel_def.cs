//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetAbandonPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_diuqi_close;

    UILabel              m_label_name;

    UITexture            m__Icon;

    UILabel              m_label_diuqi_petshowname;

    UILabel              m_label_Level;

    UILabel              m_label_life;

    UILabel              m_label_costlife;

    UIButton             m_btn_tanchuang_quxiao;

    UIButton             m_btn_tanchuang_diuqi;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_diuqi_close = fastComponent.FastGetComponent<UIButton>("diuqi_close");
       if( null == m_btn_diuqi_close )
       {
            Engine.Utility.Log.Error("m_btn_diuqi_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m__Icon = fastComponent.FastGetComponent<UITexture>("Icon");
       if( null == m__Icon )
       {
            Engine.Utility.Log.Error("m__Icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_diuqi_petshowname = fastComponent.FastGetComponent<UILabel>("diuqi_petshowname");
       if( null == m_label_diuqi_petshowname )
       {
            Engine.Utility.Log.Error("m_label_diuqi_petshowname 为空，请检查prefab是否缺乏组件");
       }
        m_label_Level = fastComponent.FastGetComponent<UILabel>("Level");
       if( null == m_label_Level )
       {
            Engine.Utility.Log.Error("m_label_Level 为空，请检查prefab是否缺乏组件");
       }
        m_label_life = fastComponent.FastGetComponent<UILabel>("life");
       if( null == m_label_life )
       {
            Engine.Utility.Log.Error("m_label_life 为空，请检查prefab是否缺乏组件");
       }
        m_label_costlife = fastComponent.FastGetComponent<UILabel>("costlife");
       if( null == m_label_costlife )
       {
            Engine.Utility.Log.Error("m_label_costlife 为空，请检查prefab是否缺乏组件");
       }
        m_btn_tanchuang_quxiao = fastComponent.FastGetComponent<UIButton>("tanchuang_quxiao");
       if( null == m_btn_tanchuang_quxiao )
       {
            Engine.Utility.Log.Error("m_btn_tanchuang_quxiao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_tanchuang_diuqi = fastComponent.FastGetComponent<UIButton>("tanchuang_diuqi");
       if( null == m_btn_tanchuang_diuqi )
       {
            Engine.Utility.Log.Error("m_btn_tanchuang_diuqi 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_diuqi_close.gameObject).onClick = _onClick_Diuqi_close_Btn;
        UIEventListener.Get(m_btn_tanchuang_quxiao.gameObject).onClick = _onClick_Tanchuang_quxiao_Btn;
        UIEventListener.Get(m_btn_tanchuang_diuqi.gameObject).onClick = _onClick_Tanchuang_diuqi_Btn;
    }

    void _onClick_Diuqi_close_Btn(GameObject caster)
    {
        onClick_Diuqi_close_Btn( caster );
    }

    void _onClick_Tanchuang_quxiao_Btn(GameObject caster)
    {
        onClick_Tanchuang_quxiao_Btn( caster );
    }

    void _onClick_Tanchuang_diuqi_Btn(GameObject caster)
    {
        onClick_Tanchuang_diuqi_Btn( caster );
    }


}
