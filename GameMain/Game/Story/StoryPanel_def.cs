//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class StoryPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UIButton             m_btn_Skip;

    UILabel              m_label_StoryContent;

    UITexture            m__TextureBg;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_Content = fastComponent.FastGetComponent<Transform>("Content");
       if( null == m_trans_Content )
       {
            Engine.Utility.Log.Error("m_trans_Content 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Skip = fastComponent.FastGetComponent<UIButton>("Skip");
       if( null == m_btn_Skip )
       {
            Engine.Utility.Log.Error("m_btn_Skip 为空，请检查prefab是否缺乏组件");
       }
        m_label_StoryContent = fastComponent.FastGetComponent<UILabel>("StoryContent");
       if( null == m_label_StoryContent )
       {
            Engine.Utility.Log.Error("m_label_StoryContent 为空，请检查prefab是否缺乏组件");
       }
        m__TextureBg = fastComponent.FastGetComponent<UITexture>("TextureBg");
       if( null == m__TextureBg )
       {
            Engine.Utility.Log.Error("m__TextureBg 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Skip.gameObject).onClick = _onClick_Skip_Btn;
    }

    void _onClick_Skip_Btn(GameObject caster)
    {
        onClick_Skip_Btn( caster );
    }


}
