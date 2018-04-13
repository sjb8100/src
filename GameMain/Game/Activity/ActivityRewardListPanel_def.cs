//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ActivityRewardListPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_BiaoTi_Label;

    UILabel              m_label_title;

    UIButton             m_btn_close;

    UIGridCreatorBase    m_ctor_ScrollView;

    Transform            m_trans_UIActivityRewardListGrid;


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
        m_label_title = fastComponent.FastGetComponent<UILabel>("title");
       if( null == m_label_title )
       {
            Engine.Utility.Log.Error("m_label_title 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ScrollView");
       if( null == m_ctor_ScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIActivityRewardListGrid = fastComponent.FastGetComponent<Transform>("UIActivityRewardListGrid");
       if( null == m_trans_UIActivityRewardListGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIActivityRewardListGrid 为空，请检查prefab是否缺乏组件");
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
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
