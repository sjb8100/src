//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CommonTipsPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_bg;

    UILabel              m_label_text_label;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_bg = fastComponent.FastGetComponent<UISprite>("bg");
       if( null == m_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
        m_label_text_label = fastComponent.FastGetComponent<UILabel>("text_label");
       if( null == m_label_text_label )
       {
            Engine.Utility.Log.Error("m_label_text_label 为空，请检查prefab是否缺乏组件");
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
