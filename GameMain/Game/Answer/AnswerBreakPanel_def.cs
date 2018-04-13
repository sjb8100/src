//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class AnswerBreakPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_BiaoTi_Label;

    UIButton             m_btn_btn_Close;

    UIButton             m_btn_btn_right;

    UILabel              m_label_right_Label;

    Transform            m_trans_UIItemRewardGrid;

    UIGridCreatorBase    m_ctor_itemRoot;


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
        m_btn_btn_right = fastComponent.FastGetComponent<UIButton>("btn_right");
       if( null == m_btn_btn_right )
       {
            Engine.Utility.Log.Error("m_btn_btn_right 为空，请检查prefab是否缺乏组件");
       }
        m_label_right_Label = fastComponent.FastGetComponent<UILabel>("right_Label");
       if( null == m_label_right_Label )
       {
            Engine.Utility.Log.Error("m_label_right_Label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_itemRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("itemRoot");
       if( null == m_ctor_itemRoot )
       {
            Engine.Utility.Log.Error("m_ctor_itemRoot 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_right.gameObject).onClick = _onClick_Btn_right_Btn;
    }

    void _onClick_Btn_Close_Btn(GameObject caster)
    {
        onClick_Btn_Close_Btn( caster );
    }

    void _onClick_Btn_right_Btn(GameObject caster)
    {
        onClick_Btn_right_Btn( caster );
    }


}
