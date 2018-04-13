//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ChapterPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_chapterPanel;

    UILabel              m_label_title;

    UILabel              m_label_desc;

    UISprite             m_sprite_Bg;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_chapterPanel = fastComponent.FastGetComponent<Transform>("chapterPanel");
       if( null == m_trans_chapterPanel )
       {
            Engine.Utility.Log.Error("m_trans_chapterPanel 为空，请检查prefab是否缺乏组件");
       }
        m_label_title = fastComponent.FastGetComponent<UILabel>("title");
       if( null == m_label_title )
       {
            Engine.Utility.Log.Error("m_label_title 为空，请检查prefab是否缺乏组件");
       }
        m_label_desc = fastComponent.FastGetComponent<UILabel>("desc");
       if( null == m_label_desc )
       {
            Engine.Utility.Log.Error("m_label_desc 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Bg = fastComponent.FastGetComponent<UISprite>("Bg");
       if( null == m_sprite_Bg )
       {
            Engine.Utility.Log.Error("m_sprite_Bg 为空，请检查prefab是否缺乏组件");
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
