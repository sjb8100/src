//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class StoryLoadingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_Content;

    UILabel              m_label_LoadingContent;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_Content = fastComponent.FastGetComponent<UIWidget>("Content");
       if( null == m_widget_Content )
       {
            Engine.Utility.Log.Error("m_widget_Content 为空，请检查prefab是否缺乏组件");
       }
        m_label_LoadingContent = fastComponent.FastGetComponent<UILabel>("LoadingContent");
       if( null == m_label_LoadingContent )
       {
            Engine.Utility.Log.Error("m_label_LoadingContent 为空，请检查prefab是否缺乏组件");
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
