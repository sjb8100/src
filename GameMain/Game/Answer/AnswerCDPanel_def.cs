//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class AnswerCDPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_CD;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_CD = fastComponent.FastGetComponent<UILabel>("CD");
       if( null == m_label_CD )
       {
            Engine.Utility.Log.Error("m_label_CD 为空，请检查prefab是否缺乏组件");
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
