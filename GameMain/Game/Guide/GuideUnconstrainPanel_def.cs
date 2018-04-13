//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GuideUnconstrainPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_UnconstrainRoot;

    Transform            m_trans_UIUnconstraintGrid;

    Transform            m_trans_UnconstrainPointBox;

    Transform            m_trans_UnArrowContent;

    Transform            m_trans_PBLeftContent;

    UILabel              m_label_UnGuideContentLeft;

    Transform            m_trans_PBRightContent;

    UILabel              m_label_UnGuideContentRight;


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
        m_trans_UnconstrainRoot = fastComponent.FastGetComponent<Transform>("UnconstrainRoot");
       if( null == m_trans_UnconstrainRoot )
       {
            Engine.Utility.Log.Error("m_trans_UnconstrainRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIUnconstraintGrid = fastComponent.FastGetComponent<Transform>("UIUnconstraintGrid");
       if( null == m_trans_UIUnconstraintGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIUnconstraintGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UnconstrainPointBox = fastComponent.FastGetComponent<Transform>("UnconstrainPointBox");
       if( null == m_trans_UnconstrainPointBox )
       {
            Engine.Utility.Log.Error("m_trans_UnconstrainPointBox 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UnArrowContent = fastComponent.FastGetComponent<Transform>("UnArrowContent");
       if( null == m_trans_UnArrowContent )
       {
            Engine.Utility.Log.Error("m_trans_UnArrowContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PBLeftContent = fastComponent.FastGetComponent<Transform>("PBLeftContent");
       if( null == m_trans_PBLeftContent )
       {
            Engine.Utility.Log.Error("m_trans_PBLeftContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_UnGuideContentLeft = fastComponent.FastGetComponent<UILabel>("UnGuideContentLeft");
       if( null == m_label_UnGuideContentLeft )
       {
            Engine.Utility.Log.Error("m_label_UnGuideContentLeft 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PBRightContent = fastComponent.FastGetComponent<Transform>("PBRightContent");
       if( null == m_trans_PBRightContent )
       {
            Engine.Utility.Log.Error("m_trans_PBRightContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_UnGuideContentRight = fastComponent.FastGetComponent<UILabel>("UnGuideContentRight");
       if( null == m_label_UnGuideContentRight )
       {
            Engine.Utility.Log.Error("m_label_UnGuideContentRight 为空，请检查prefab是否缺乏组件");
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
