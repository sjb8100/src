//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GuidePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_Constraint;

    Transform            m_trans_ConstraintPointBox;

    Transform            m_trans_ConstraintAnimContent;

    Transform            m_trans_ArrowContent;

    Transform            m_trans_ConstraintDirection;

    Transform            m_trans_PointBoxContent;

    Transform            m_trans_PBLeftContent;

    UILabel              m_label_GuideLeftContent;

    Transform            m_trans_PBRightContent;

    UILabel              m_label_GuideRightContent;

    Transform            m_trans_DynamicMaskContent;

    UIButton             m_btn_BtnMask;

    Transform            m_trans_ConstraintAttachRoot;

    UIButton             m_btn_Skip;


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
        m_trans_Constraint = fastComponent.FastGetComponent<Transform>("Constraint");
       if( null == m_trans_Constraint )
       {
            Engine.Utility.Log.Error("m_trans_Constraint 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ConstraintPointBox = fastComponent.FastGetComponent<Transform>("ConstraintPointBox");
       if( null == m_trans_ConstraintPointBox )
       {
            Engine.Utility.Log.Error("m_trans_ConstraintPointBox 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ConstraintAnimContent = fastComponent.FastGetComponent<Transform>("ConstraintAnimContent");
       if( null == m_trans_ConstraintAnimContent )
       {
            Engine.Utility.Log.Error("m_trans_ConstraintAnimContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ArrowContent = fastComponent.FastGetComponent<Transform>("ArrowContent");
       if( null == m_trans_ArrowContent )
       {
            Engine.Utility.Log.Error("m_trans_ArrowContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ConstraintDirection = fastComponent.FastGetComponent<Transform>("ConstraintDirection");
       if( null == m_trans_ConstraintDirection )
       {
            Engine.Utility.Log.Error("m_trans_ConstraintDirection 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PointBoxContent = fastComponent.FastGetComponent<Transform>("PointBoxContent");
       if( null == m_trans_PointBoxContent )
       {
            Engine.Utility.Log.Error("m_trans_PointBoxContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PBLeftContent = fastComponent.FastGetComponent<Transform>("PBLeftContent");
       if( null == m_trans_PBLeftContent )
       {
            Engine.Utility.Log.Error("m_trans_PBLeftContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_GuideLeftContent = fastComponent.FastGetComponent<UILabel>("GuideLeftContent");
       if( null == m_label_GuideLeftContent )
       {
            Engine.Utility.Log.Error("m_label_GuideLeftContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PBRightContent = fastComponent.FastGetComponent<Transform>("PBRightContent");
       if( null == m_trans_PBRightContent )
       {
            Engine.Utility.Log.Error("m_trans_PBRightContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_GuideRightContent = fastComponent.FastGetComponent<UILabel>("GuideRightContent");
       if( null == m_label_GuideRightContent )
       {
            Engine.Utility.Log.Error("m_label_GuideRightContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DynamicMaskContent = fastComponent.FastGetComponent<Transform>("DynamicMaskContent");
       if( null == m_trans_DynamicMaskContent )
       {
            Engine.Utility.Log.Error("m_trans_DynamicMaskContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnMask = fastComponent.FastGetComponent<UIButton>("BtnMask");
       if( null == m_btn_BtnMask )
       {
            Engine.Utility.Log.Error("m_btn_BtnMask 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ConstraintAttachRoot = fastComponent.FastGetComponent<Transform>("ConstraintAttachRoot");
       if( null == m_trans_ConstraintAttachRoot )
       {
            Engine.Utility.Log.Error("m_trans_ConstraintAttachRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Skip = fastComponent.FastGetComponent<UIButton>("Skip");
       if( null == m_btn_Skip )
       {
            Engine.Utility.Log.Error("m_btn_Skip 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BtnMask.gameObject).onClick = _onClick_BtnMask_Btn;
        UIEventListener.Get(m_btn_Skip.gameObject).onClick = _onClick_Skip_Btn;
    }

    void _onClick_BtnMask_Btn(GameObject caster)
    {
        onClick_BtnMask_Btn( caster );
    }

    void _onClick_Skip_Btn(GameObject caster)
    {
        onClick_Skip_Btn( caster );
    }


}
