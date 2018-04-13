//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ClanTaskPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_BoxRoot;

    UISprite             m_sprite_tips_btn;

    UILabel              m_label_stepDes;

    UILabel              m_label_times;

    Transform            m_trans_clantaskscrollview;

    Transform            m_trans_UIClanTaskGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_BoxRoot = fastComponent.FastGetComponent<Transform>("BoxRoot");
       if( null == m_trans_BoxRoot )
       {
            Engine.Utility.Log.Error("m_trans_BoxRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_tips_btn = fastComponent.FastGetComponent<UISprite>("tips_btn");
       if( null == m_sprite_tips_btn )
       {
            Engine.Utility.Log.Error("m_sprite_tips_btn 为空，请检查prefab是否缺乏组件");
       }
        m_label_stepDes = fastComponent.FastGetComponent<UILabel>("stepDes");
       if( null == m_label_stepDes )
       {
            Engine.Utility.Log.Error("m_label_stepDes 为空，请检查prefab是否缺乏组件");
       }
        m_label_times = fastComponent.FastGetComponent<UILabel>("times");
       if( null == m_label_times )
       {
            Engine.Utility.Log.Error("m_label_times 为空，请检查prefab是否缺乏组件");
       }
        m_trans_clantaskscrollview = fastComponent.FastGetComponent<Transform>("clantaskscrollview");
       if( null == m_trans_clantaskscrollview )
       {
            Engine.Utility.Log.Error("m_trans_clantaskscrollview 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanTaskGrid = fastComponent.FastGetComponent<Transform>("UIClanTaskGrid");
       if( null == m_trans_UIClanTaskGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanTaskGrid 为空，请检查prefab是否缺乏组件");
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
