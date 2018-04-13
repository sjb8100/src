//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RichTextLabel2: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_RichTextLabel2;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_RichTextLabel2 = fastComponent.FastGetComponent<UILabel>("RichTextLabel2");
       if( null == m_label_RichTextLabel2 )
       {
            Engine.Utility.Log.Error("m_label_RichTextLabel2 为空，请检查prefab是否缺乏组件");
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
