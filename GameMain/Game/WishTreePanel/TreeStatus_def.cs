//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TreeStatus: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_TreeLevel;

    UILabel              m_label_Time;

    UISlider             m_slider_HelpSlider;

    UILabel              m_label_percent;

    UIButton             m_btn_BuyTree;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_TreeLevel = fastComponent.FastGetComponent<UIButton>("TreeLevel");
       if( null == m_btn_TreeLevel )
       {
            Engine.Utility.Log.Error("m_btn_TreeLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_Time = fastComponent.FastGetComponent<UILabel>("Time");
       if( null == m_label_Time )
       {
            Engine.Utility.Log.Error("m_label_Time 为空，请检查prefab是否缺乏组件");
       }
        m_slider_HelpSlider = fastComponent.FastGetComponent<UISlider>("HelpSlider");
       if( null == m_slider_HelpSlider )
       {
            Engine.Utility.Log.Error("m_slider_HelpSlider 为空，请检查prefab是否缺乏组件");
       }
        m_label_percent = fastComponent.FastGetComponent<UILabel>("percent");
       if( null == m_label_percent )
       {
            Engine.Utility.Log.Error("m_label_percent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BuyTree = fastComponent.FastGetComponent<UIButton>("BuyTree");
       if( null == m_btn_BuyTree )
       {
            Engine.Utility.Log.Error("m_btn_BuyTree 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_TreeLevel.gameObject).onClick = _onClick_TreeLevel_Btn;
        UIEventListener.Get(m_btn_BuyTree.gameObject).onClick = _onClick_BuyTree_Btn;
    }

    void _onClick_TreeLevel_Btn(GameObject caster)
    {
        onClick_TreeLevel_Btn( caster );
    }

    void _onClick_BuyTree_Btn(GameObject caster)
    {
        onClick_BuyTree_Btn( caster );
    }


}
