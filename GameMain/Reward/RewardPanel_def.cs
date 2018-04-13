//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RewardPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UIWidget      m_widget_hallContent;

    UIWidget      m_widget_parentContent;

    UIWidget      m_widget_childContent;

    UIWidget      m_widget_release_status;

    UIWidget      m_widget_access_status;

    Transform     m_trans_itemRoot;

    UIWidget      m_widget_mineContent;

    UIWidget      m_widget_Mine_Release;

    UIWidget      m_widget_Mine_Access;

    UIWidget      m_widget_NoRelease_status;

    UIButton      m_btn_btn_GoRelease;

    UIWidget      m_widget_NoAccess_status;

    UIButton      m_btn_btn_GoAccess;

    UIWidget      m_widget_Releasing_status;

    UIWidget      m_widget_Accessing_status;

    UIWidget      m_widget_ReleaseEnd_status;

    UIWidget      m_widget_AccessEnd_status;

    UIButton      m_btn_Release;

    UIButton      m_btn_Access;

    UIButton      m_btn_hall;

    UIButton      m_btn_mine;


    //初始化控件变量
    protected override void InitControls()
    {
        m_widget_hallContent = GetChildComponent<UIWidget>("hallContent");
       if( null == m_widget_hallContent )
       {
            Engine.Utility.Log.Error("m_widget_hallContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_parentContent = GetChildComponent<UIWidget>("parentContent");
       if( null == m_widget_parentContent )
       {
            Engine.Utility.Log.Error("m_widget_parentContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_childContent = GetChildComponent<UIWidget>("childContent");
       if( null == m_widget_childContent )
       {
            Engine.Utility.Log.Error("m_widget_childContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_release_status = GetChildComponent<UIWidget>("release_status");
       if( null == m_widget_release_status )
       {
            Engine.Utility.Log.Error("m_widget_release_status 为空，请检查prefab是否缺乏组件");
       }
        m_widget_access_status = GetChildComponent<UIWidget>("access_status");
       if( null == m_widget_access_status )
       {
            Engine.Utility.Log.Error("m_widget_access_status 为空，请检查prefab是否缺乏组件");
       }
        m_trans_itemRoot = GetChildComponent<Transform>("itemRoot");
       if( null == m_trans_itemRoot )
       {
            Engine.Utility.Log.Error("m_trans_itemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_widget_mineContent = GetChildComponent<UIWidget>("mineContent");
       if( null == m_widget_mineContent )
       {
            Engine.Utility.Log.Error("m_widget_mineContent 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Mine_Release = GetChildComponent<UIWidget>("Mine_Release");
       if( null == m_widget_Mine_Release )
       {
            Engine.Utility.Log.Error("m_widget_Mine_Release 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Mine_Access = GetChildComponent<UIWidget>("Mine_Access");
       if( null == m_widget_Mine_Access )
       {
            Engine.Utility.Log.Error("m_widget_Mine_Access 为空，请检查prefab是否缺乏组件");
       }
        m_widget_NoRelease_status = GetChildComponent<UIWidget>("NoRelease_status");
       if( null == m_widget_NoRelease_status )
       {
            Engine.Utility.Log.Error("m_widget_NoRelease_status 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_GoRelease = GetChildComponent<UIButton>("btn_GoRelease");
       if( null == m_btn_btn_GoRelease )
       {
            Engine.Utility.Log.Error("m_btn_btn_GoRelease 为空，请检查prefab是否缺乏组件");
       }
        m_widget_NoAccess_status = GetChildComponent<UIWidget>("NoAccess_status");
       if( null == m_widget_NoAccess_status )
       {
            Engine.Utility.Log.Error("m_widget_NoAccess_status 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_GoAccess = GetChildComponent<UIButton>("btn_GoAccess");
       if( null == m_btn_btn_GoAccess )
       {
            Engine.Utility.Log.Error("m_btn_btn_GoAccess 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Releasing_status = GetChildComponent<UIWidget>("Releasing_status");
       if( null == m_widget_Releasing_status )
       {
            Engine.Utility.Log.Error("m_widget_Releasing_status 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Accessing_status = GetChildComponent<UIWidget>("Accessing_status");
       if( null == m_widget_Accessing_status )
       {
            Engine.Utility.Log.Error("m_widget_Accessing_status 为空，请检查prefab是否缺乏组件");
       }
        m_widget_ReleaseEnd_status = GetChildComponent<UIWidget>("ReleaseEnd_status");
       if( null == m_widget_ReleaseEnd_status )
       {
            Engine.Utility.Log.Error("m_widget_ReleaseEnd_status 为空，请检查prefab是否缺乏组件");
       }
        m_widget_AccessEnd_status = GetChildComponent<UIWidget>("AccessEnd_status");
       if( null == m_widget_AccessEnd_status )
       {
            Engine.Utility.Log.Error("m_widget_AccessEnd_status 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Release = GetChildComponent<UIButton>("Release");
       if( null == m_btn_Release )
       {
            Engine.Utility.Log.Error("m_btn_Release 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Access = GetChildComponent<UIButton>("Access");
       if( null == m_btn_Access )
       {
            Engine.Utility.Log.Error("m_btn_Access 为空，请检查prefab是否缺乏组件");
       }
        m_btn_hall = GetChildComponent<UIButton>("hall");
       if( null == m_btn_hall )
       {
            Engine.Utility.Log.Error("m_btn_hall 为空，请检查prefab是否缺乏组件");
       }
        m_btn_mine = GetChildComponent<UIButton>("mine");
       if( null == m_btn_mine )
       {
            Engine.Utility.Log.Error("m_btn_mine 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_GoRelease.gameObject).onClick = _onClick_Btn_GoRelease_Btn;
        UIEventListener.Get(m_btn_btn_GoAccess.gameObject).onClick = _onClick_Btn_GoAccess_Btn;
        UIEventListener.Get(m_btn_Release.gameObject).onClick = _onClick_Release_Btn;
        UIEventListener.Get(m_btn_Access.gameObject).onClick = _onClick_Access_Btn;
        UIEventListener.Get(m_btn_hall.gameObject).onClick = _onClick_Hall_Btn;
        UIEventListener.Get(m_btn_mine.gameObject).onClick = _onClick_Mine_Btn;
    }

    void _onClick_Btn_GoRelease_Btn(GameObject caster)
    {
        onClick_Btn_GoRelease_Btn( caster );
    }

    void _onClick_Btn_GoAccess_Btn(GameObject caster)
    {
        onClick_Btn_GoAccess_Btn( caster );
    }

    void _onClick_Release_Btn(GameObject caster)
    {
        onClick_Release_Btn( caster );
    }

    void _onClick_Access_Btn(GameObject caster)
    {
        onClick_Access_Btn( caster );
    }

    void _onClick_Hall_Btn(GameObject caster)
    {
        onClick_Hall_Btn( caster );
    }

    void _onClick_Mine_Btn(GameObject caster)
    {
        onClick_Mine_Btn( caster );
    }


}
