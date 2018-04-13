//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ReplenishSignPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btnclose;

    UILabel              m_label_LabelDes;

    UIButton             m_btn_btn_1;

    UIButton             m_btn_btn_2;

    UILabel              m_label_LabelOne;

    UILabel              m_label_LabelAll;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btnclose = fastComponent.FastGetComponent<UIButton>("btnclose");
       if( null == m_btn_btnclose )
       {
            Engine.Utility.Log.Error("m_btn_btnclose 为空，请检查prefab是否缺乏组件");
       }
        m_label_LabelDes = fastComponent.FastGetComponent<UILabel>("LabelDes");
       if( null == m_label_LabelDes )
       {
            Engine.Utility.Log.Error("m_label_LabelDes 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_1 = fastComponent.FastGetComponent<UIButton>("btn_1");
       if( null == m_btn_btn_1 )
       {
            Engine.Utility.Log.Error("m_btn_btn_1 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_2 = fastComponent.FastGetComponent<UIButton>("btn_2");
       if( null == m_btn_btn_2 )
       {
            Engine.Utility.Log.Error("m_btn_btn_2 为空，请检查prefab是否缺乏组件");
       }
        m_label_LabelOne = fastComponent.FastGetComponent<UILabel>("LabelOne");
       if( null == m_label_LabelOne )
       {
            Engine.Utility.Log.Error("m_label_LabelOne 为空，请检查prefab是否缺乏组件");
       }
        m_label_LabelAll = fastComponent.FastGetComponent<UILabel>("LabelAll");
       if( null == m_label_LabelAll )
       {
            Engine.Utility.Log.Error("m_label_LabelAll 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btnclose.gameObject).onClick = _onClick_Btnclose_Btn;
        UIEventListener.Get(m_btn_btn_1.gameObject).onClick = _onClick_Btn_1_Btn;
        UIEventListener.Get(m_btn_btn_2.gameObject).onClick = _onClick_Btn_2_Btn;
    }

    void _onClick_Btnclose_Btn(GameObject caster)
    {
        onClick_Btnclose_Btn( caster );
    }

    void _onClick_Btn_1_Btn(GameObject caster)
    {
        onClick_Btn_1_Btn( caster );
    }

    void _onClick_Btn_2_Btn(GameObject caster)
    {
        onClick_Btn_2_Btn( caster );
    }


}
