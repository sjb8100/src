//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RideQualityPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_BiaoTi_Label;

    UIButton             m_btn_btn_Close;

    Transform            m_trans_liliangzizhi;

    UIButton             m_btn_Btn_Go1;

    Transform            m_trans_mingjie;

    UIButton             m_btn_Btn_Go2;

    Transform            m_trans_zhili;

    UIButton             m_btn_Btn_Go3;

    Transform            m_trans_tili;

    UIButton             m_btn_Btn_Go4;

    Transform            m_trans_jingshen;

    UIButton             m_btn_Btn_Go5;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_BiaoTi_Label = fastComponent.FastGetComponent<UILabel>("BiaoTi_Label");
       if( null == m_label_BiaoTi_Label )
       {
            Engine.Utility.Log.Error("m_label_BiaoTi_Label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Close = fastComponent.FastGetComponent<UIButton>("btn_Close");
       if( null == m_btn_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_liliangzizhi = fastComponent.FastGetComponent<Transform>("liliangzizhi");
       if( null == m_trans_liliangzizhi )
       {
            Engine.Utility.Log.Error("m_trans_liliangzizhi 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Go1 = fastComponent.FastGetComponent<UIButton>("Btn_Go1");
       if( null == m_btn_Btn_Go1 )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Go1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_mingjie = fastComponent.FastGetComponent<Transform>("mingjie");
       if( null == m_trans_mingjie )
       {
            Engine.Utility.Log.Error("m_trans_mingjie 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Go2 = fastComponent.FastGetComponent<UIButton>("Btn_Go2");
       if( null == m_btn_Btn_Go2 )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Go2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_zhili = fastComponent.FastGetComponent<Transform>("zhili");
       if( null == m_trans_zhili )
       {
            Engine.Utility.Log.Error("m_trans_zhili 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Go3 = fastComponent.FastGetComponent<UIButton>("Btn_Go3");
       if( null == m_btn_Btn_Go3 )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Go3 为空，请检查prefab是否缺乏组件");
       }
        m_trans_tili = fastComponent.FastGetComponent<Transform>("tili");
       if( null == m_trans_tili )
       {
            Engine.Utility.Log.Error("m_trans_tili 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Go4 = fastComponent.FastGetComponent<UIButton>("Btn_Go4");
       if( null == m_btn_Btn_Go4 )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Go4 为空，请检查prefab是否缺乏组件");
       }
        m_trans_jingshen = fastComponent.FastGetComponent<Transform>("jingshen");
       if( null == m_trans_jingshen )
       {
            Engine.Utility.Log.Error("m_trans_jingshen 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Go5 = fastComponent.FastGetComponent<UIButton>("Btn_Go5");
       if( null == m_btn_Btn_Go5 )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Go5 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_Close.gameObject).onClick = _onClick_Btn_Close_Btn;
        UIEventListener.Get(m_btn_Btn_Go1.gameObject).onClick = _onClick_Btn_Go1_Btn;
        UIEventListener.Get(m_btn_Btn_Go2.gameObject).onClick = _onClick_Btn_Go2_Btn;
        UIEventListener.Get(m_btn_Btn_Go3.gameObject).onClick = _onClick_Btn_Go3_Btn;
        UIEventListener.Get(m_btn_Btn_Go4.gameObject).onClick = _onClick_Btn_Go4_Btn;
        UIEventListener.Get(m_btn_Btn_Go5.gameObject).onClick = _onClick_Btn_Go5_Btn;
    }

    void _onClick_Btn_Close_Btn(GameObject caster)
    {
        onClick_Btn_Close_Btn( caster );
    }

    void _onClick_Btn_Go1_Btn(GameObject caster)
    {
        onClick_Btn_Go1_Btn( caster );
    }

    void _onClick_Btn_Go2_Btn(GameObject caster)
    {
        onClick_Btn_Go2_Btn( caster );
    }

    void _onClick_Btn_Go3_Btn(GameObject caster)
    {
        onClick_Btn_Go3_Btn( caster );
    }

    void _onClick_Btn_Go4_Btn(GameObject caster)
    {
        onClick_Btn_Go4_Btn( caster );
    }

    void _onClick_Btn_Go5_Btn(GameObject caster)
    {
        onClick_Btn_Go5_Btn( caster );
    }


}
