//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class AddpointExplanationPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_close;

    UILabel              m_label_addpointexplanation_name;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_close = fastComponent.FastGetComponent<UIWidget>("close");
       if( null == m_widget_close )
       {
            Engine.Utility.Log.Error("m_widget_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_addpointexplanation_name = fastComponent.FastGetComponent<UILabel>("addpointexplanation_name");
       if( null == m_label_addpointexplanation_name )
       {
            Engine.Utility.Log.Error("m_label_addpointexplanation_name 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
