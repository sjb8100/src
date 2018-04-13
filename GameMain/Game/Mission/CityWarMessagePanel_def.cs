//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CityWarMessagePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_bottomRoot;

    UIButton             m_btn_btn_bottom;

    Transform            m_trans_signUpRoot;

    UIScrollView         m_scrollview_signUpClanScrollView;

    Transform            m_trans_clanName;

    Transform            m_trans_IntroductionRoot;

    UIWidget             m_widget_normalTask;

    UIScrollView         m_scrollview_rewardItemScrollView;

    UILabel              m_label_title_desc;

    UILabel              m_label_title_sighup;

    UIButton             m_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_bottomRoot = fastComponent.FastGetComponent<Transform>("bottomRoot");
       if( null == m_trans_bottomRoot )
       {
            Engine.Utility.Log.Error("m_trans_bottomRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_bottom = fastComponent.FastGetComponent<UIButton>("btn_bottom");
       if( null == m_btn_btn_bottom )
       {
            Engine.Utility.Log.Error("m_btn_btn_bottom 为空，请检查prefab是否缺乏组件");
       }
        m_trans_signUpRoot = fastComponent.FastGetComponent<Transform>("signUpRoot");
       if( null == m_trans_signUpRoot )
       {
            Engine.Utility.Log.Error("m_trans_signUpRoot 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_signUpClanScrollView = fastComponent.FastGetComponent<UIScrollView>("signUpClanScrollView");
       if( null == m_scrollview_signUpClanScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_signUpClanScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_clanName = fastComponent.FastGetComponent<Transform>("clanName");
       if( null == m_trans_clanName )
       {
            Engine.Utility.Log.Error("m_trans_clanName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_IntroductionRoot = fastComponent.FastGetComponent<Transform>("IntroductionRoot");
       if( null == m_trans_IntroductionRoot )
       {
            Engine.Utility.Log.Error("m_trans_IntroductionRoot 为空，请检查prefab是否缺乏组件");
       }
        m_widget_normalTask = fastComponent.FastGetComponent<UIWidget>("normalTask");
       if( null == m_widget_normalTask )
       {
            Engine.Utility.Log.Error("m_widget_normalTask 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_rewardItemScrollView = fastComponent.FastGetComponent<UIScrollView>("rewardItemScrollView");
       if( null == m_scrollview_rewardItemScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_rewardItemScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_label_title_desc = fastComponent.FastGetComponent<UILabel>("title_desc");
       if( null == m_label_title_desc )
       {
            Engine.Utility.Log.Error("m_label_title_desc 为空，请检查prefab是否缺乏组件");
       }
        m_label_title_sighup = fastComponent.FastGetComponent<UILabel>("title_sighup");
       if( null == m_label_title_sighup )
       {
            Engine.Utility.Log.Error("m_label_title_sighup 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_bottom.gameObject).onClick = _onClick_Btn_bottom_Btn;
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Btn_bottom_Btn(GameObject caster)
    {
        onClick_Btn_bottom_Btn( caster );
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
