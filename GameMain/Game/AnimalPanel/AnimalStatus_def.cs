//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class AnimalStatus: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_Mature_status;

    UIWidget             m_widget_Grow_status;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_Mature_status = fastComponent.FastGetComponent<UIWidget>("Mature_status");
       if( null == m_widget_Mature_status )
       {
            Engine.Utility.Log.Error("m_widget_Mature_status 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Grow_status = fastComponent.FastGetComponent<UIWidget>("Grow_status");
       if( null == m_widget_Grow_status )
       {
            Engine.Utility.Log.Error("m_widget_Grow_status 为空，请检查prefab是否缺乏组件");
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
