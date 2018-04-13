//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RedEnvelopePanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//世界
		ShiJie = 1,
		//氏族
		ShiZu = 2,
		Max,
    }

   FastComponent         fastComponent;

    UIGridCreatorBase    m_ctor_RedEnvelopeScrollView;

    Transform            m_trans_nohave;

    UIButton             m_btn_btn_history;

    UIButton             m_btn_btn_fresh;

    UIButton             m_btn_btn_Send;

    Transform            m_trans_uiRedEnvelopeGrid;

    UILabel              m_label_name_label;

    UILabel              m_label_des_label;

    UILabel              m_label_title_label;

    UILabel              m_label_text_label;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_ctor_RedEnvelopeScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("RedEnvelopeScrollView");
       if( null == m_ctor_RedEnvelopeScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_RedEnvelopeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_nohave = fastComponent.FastGetComponent<Transform>("nohave");
       if( null == m_trans_nohave )
       {
            Engine.Utility.Log.Error("m_trans_nohave 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_history = fastComponent.FastGetComponent<UIButton>("btn_history");
       if( null == m_btn_btn_history )
       {
            Engine.Utility.Log.Error("m_btn_btn_history 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_fresh = fastComponent.FastGetComponent<UIButton>("btn_fresh");
       if( null == m_btn_btn_fresh )
       {
            Engine.Utility.Log.Error("m_btn_btn_fresh 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Send = fastComponent.FastGetComponent<UIButton>("btn_Send");
       if( null == m_btn_btn_Send )
       {
            Engine.Utility.Log.Error("m_btn_btn_Send 为空，请检查prefab是否缺乏组件");
       }
        m_trans_uiRedEnvelopeGrid = fastComponent.FastGetComponent<Transform>("uiRedEnvelopeGrid");
       if( null == m_trans_uiRedEnvelopeGrid )
       {
            Engine.Utility.Log.Error("m_trans_uiRedEnvelopeGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_name_label = fastComponent.FastGetComponent<UILabel>("name_label");
       if( null == m_label_name_label )
       {
            Engine.Utility.Log.Error("m_label_name_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_des_label = fastComponent.FastGetComponent<UILabel>("des_label");
       if( null == m_label_des_label )
       {
            Engine.Utility.Log.Error("m_label_des_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_title_label = fastComponent.FastGetComponent<UILabel>("title_label");
       if( null == m_label_title_label )
       {
            Engine.Utility.Log.Error("m_label_title_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_text_label = fastComponent.FastGetComponent<UILabel>("text_label");
       if( null == m_label_text_label )
       {
            Engine.Utility.Log.Error("m_label_text_label 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_history.gameObject).onClick = _onClick_Btn_history_Btn;
        UIEventListener.Get(m_btn_btn_fresh.gameObject).onClick = _onClick_Btn_fresh_Btn;
        UIEventListener.Get(m_btn_btn_Send.gameObject).onClick = _onClick_Btn_Send_Btn;
    }

    void _onClick_Btn_history_Btn(GameObject caster)
    {
        onClick_Btn_history_Btn( caster );
    }

    void _onClick_Btn_fresh_Btn(GameObject caster)
    {
        onClick_Btn_fresh_Btn( caster );
    }

    void _onClick_Btn_Send_Btn(GameObject caster)
    {
        onClick_Btn_Send_Btn( caster );
    }


}
