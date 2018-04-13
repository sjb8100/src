//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class LoginNoticePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_ListRoot;

    Transform            m_trans_ListContent;

    UIButton             m_btn_BtnClose;

    UILabel              m_label_NoticeName;

    UIScrollView         m_scrollview_view;

    UIWidget             m_widget_NoticeMessage;

    UILabel              m_label_name;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_ListRoot = fastComponent.FastGetComponent<Transform>("ListRoot");
       if( null == m_trans_ListRoot )
       {
            Engine.Utility.Log.Error("m_trans_ListRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ListContent = fastComponent.FastGetComponent<Transform>("ListContent");
       if( null == m_trans_ListContent )
       {
            Engine.Utility.Log.Error("m_trans_ListContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnClose = fastComponent.FastGetComponent<UIButton>("BtnClose");
       if( null == m_btn_BtnClose )
       {
            Engine.Utility.Log.Error("m_btn_BtnClose 为空，请检查prefab是否缺乏组件");
       }
        m_label_NoticeName = fastComponent.FastGetComponent<UILabel>("NoticeName");
       if( null == m_label_NoticeName )
       {
            Engine.Utility.Log.Error("m_label_NoticeName 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_view = fastComponent.FastGetComponent<UIScrollView>("view");
       if( null == m_scrollview_view )
       {
            Engine.Utility.Log.Error("m_scrollview_view 为空，请检查prefab是否缺乏组件");
       }
        m_widget_NoticeMessage = fastComponent.FastGetComponent<UIWidget>("NoticeMessage");
       if( null == m_widget_NoticeMessage )
       {
            Engine.Utility.Log.Error("m_widget_NoticeMessage 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BtnClose.gameObject).onClick = _onClick_BtnClose_Btn;
    }

    void _onClick_BtnClose_Btn(GameObject caster)
    {
        onClick_BtnClose_Btn( caster );
    }


}
