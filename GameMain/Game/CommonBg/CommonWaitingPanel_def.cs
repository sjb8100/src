//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CommonWaitingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_Des;

    UILabel              m_label_Time;

    UILabel              m_label_countdown;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_Des = fastComponent.FastGetComponent<UILabel>("Des");
       if( null == m_label_Des )
       {
            Engine.Utility.Log.Error("m_label_Des 为空，请检查prefab是否缺乏组件");
       }
        m_label_Time = fastComponent.FastGetComponent<UILabel>("Time");
       if( null == m_label_Time )
       {
            Engine.Utility.Log.Error("m_label_Time 为空，请检查prefab是否缺乏组件");
       }
        m_label_countdown = fastComponent.FastGetComponent<UILabel>("countdown");
       if( null == m_label_countdown )
       {
            Engine.Utility.Log.Error("m_label_countdown 为空，请检查prefab是否缺乏组件");
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
