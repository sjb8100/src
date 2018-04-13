//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class HomeFriendPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_close;

    UIButton             m_btn_FriendList;

    UIButton             m_btn_SearchList;

    UIButton             m_btn_btn_refresh;

    UIInput              m_input_Level_Input;

    UIButton             m_btn_btn_search;

    Transform            m_trans_listContent;

    Transform            m_trans_listScrollView;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_FriendList = fastComponent.FastGetComponent<UIButton>("FriendList");
       if( null == m_btn_FriendList )
       {
            Engine.Utility.Log.Error("m_btn_FriendList 为空，请检查prefab是否缺乏组件");
       }
        m_btn_SearchList = fastComponent.FastGetComponent<UIButton>("SearchList");
       if( null == m_btn_SearchList )
       {
            Engine.Utility.Log.Error("m_btn_SearchList 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_refresh = fastComponent.FastGetComponent<UIButton>("btn_refresh");
       if( null == m_btn_btn_refresh )
       {
            Engine.Utility.Log.Error("m_btn_btn_refresh 为空，请检查prefab是否缺乏组件");
       }
        m_input_Level_Input = fastComponent.FastGetComponent<UIInput>("Level_Input");
       if( null == m_input_Level_Input )
       {
            Engine.Utility.Log.Error("m_input_Level_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_search = fastComponent.FastGetComponent<UIButton>("btn_search");
       if( null == m_btn_btn_search )
       {
            Engine.Utility.Log.Error("m_btn_btn_search 为空，请检查prefab是否缺乏组件");
       }
        m_trans_listContent = fastComponent.FastGetComponent<Transform>("listContent");
       if( null == m_trans_listContent )
       {
            Engine.Utility.Log.Error("m_trans_listContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_listScrollView = fastComponent.FastGetComponent<Transform>("listScrollView");
       if( null == m_trans_listScrollView )
       {
            Engine.Utility.Log.Error("m_trans_listScrollView 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
        UIEventListener.Get(m_btn_FriendList.gameObject).onClick = _onClick_FriendList_Btn;
        UIEventListener.Get(m_btn_SearchList.gameObject).onClick = _onClick_SearchList_Btn;
        UIEventListener.Get(m_btn_btn_refresh.gameObject).onClick = _onClick_Btn_refresh_Btn;
        UIEventListener.Get(m_btn_btn_search.gameObject).onClick = _onClick_Btn_search_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_FriendList_Btn(GameObject caster)
    {
        onClick_FriendList_Btn( caster );
    }

    void _onClick_SearchList_Btn(GameObject caster)
    {
        onClick_SearchList_Btn( caster );
    }

    void _onClick_Btn_refresh_Btn(GameObject caster)
    {
        onClick_Btn_refresh_Btn( caster );
    }

    void _onClick_Btn_search_Btn(GameObject caster)
    {
        onClick_Btn_search_Btn( caster );
    }


}
