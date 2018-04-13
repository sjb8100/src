//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RoleStateBarPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_RoleStateBarRoot;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_RoleStateBarRoot = fastComponent.FastGetComponent<UIWidget>("RoleStateBarRoot");
       if( null == m_widget_RoleStateBarRoot )
       {
            Engine.Utility.Log.Error("m_widget_RoleStateBarRoot 为空，请检查prefab是否缺乏组件");
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
