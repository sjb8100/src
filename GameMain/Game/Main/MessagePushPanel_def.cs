//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MessagePushPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_FunctionPushContent;

    Transform            m_trans_Type1;

    Transform            m_trans_Type2;

    Transform            m_trans_MessagePushContent;

    Transform            m_trans_BtnRoot;

    UIGridCreatorBase    m_ctor_DailyPushContent;

    Transform            m_trans_UIDailyPushGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_FunctionPushContent = fastComponent.FastGetComponent<UIButton>("FunctionPushContent");
       if( null == m_btn_FunctionPushContent )
       {
            Engine.Utility.Log.Error("m_btn_FunctionPushContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Type1 = fastComponent.FastGetComponent<Transform>("Type1");
       if( null == m_trans_Type1 )
       {
            Engine.Utility.Log.Error("m_trans_Type1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Type2 = fastComponent.FastGetComponent<Transform>("Type2");
       if( null == m_trans_Type2 )
       {
            Engine.Utility.Log.Error("m_trans_Type2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MessagePushContent = fastComponent.FastGetComponent<Transform>("MessagePushContent");
       if( null == m_trans_MessagePushContent )
       {
            Engine.Utility.Log.Error("m_trans_MessagePushContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BtnRoot = fastComponent.FastGetComponent<Transform>("BtnRoot");
       if( null == m_trans_BtnRoot )
       {
            Engine.Utility.Log.Error("m_trans_BtnRoot 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_DailyPushContent = fastComponent.FastGetComponent<UIGridCreatorBase>("DailyPushContent");
       if( null == m_ctor_DailyPushContent )
       {
            Engine.Utility.Log.Error("m_ctor_DailyPushContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIDailyPushGrid = fastComponent.FastGetComponent<Transform>("UIDailyPushGrid");
       if( null == m_trans_UIDailyPushGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIDailyPushGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_FunctionPushContent.gameObject).onClick = _onClick_FunctionPushContent_Btn;
    }

    void _onClick_FunctionPushContent_Btn(GameObject caster)
    {
        onClick_FunctionPushContent_Btn( caster );
    }


}
