//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class NewFuncOpenPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_Content;

    UISprite             m_sprite_Icon;

    UILabel              m_label_NewFuncOpenTxt;

    UILabel              m_label_NewFuncName;

    UITexture            m__TexIcon;

    UIButton             m_btn_ClickEventObj;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_Content = fastComponent.FastGetComponent<UIWidget>("Content");
       if( null == m_widget_Content )
       {
            Engine.Utility.Log.Error("m_widget_Content 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Icon = fastComponent.FastGetComponent<UISprite>("Icon");
       if( null == m_sprite_Icon )
       {
            Engine.Utility.Log.Error("m_sprite_Icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_NewFuncOpenTxt = fastComponent.FastGetComponent<UILabel>("NewFuncOpenTxt");
       if( null == m_label_NewFuncOpenTxt )
       {
            Engine.Utility.Log.Error("m_label_NewFuncOpenTxt 为空，请检查prefab是否缺乏组件");
       }
        m_label_NewFuncName = fastComponent.FastGetComponent<UILabel>("NewFuncName");
       if( null == m_label_NewFuncName )
       {
            Engine.Utility.Log.Error("m_label_NewFuncName 为空，请检查prefab是否缺乏组件");
       }
        m__TexIcon = fastComponent.FastGetComponent<UITexture>("TexIcon");
       if( null == m__TexIcon )
       {
            Engine.Utility.Log.Error("m__TexIcon 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ClickEventObj = fastComponent.FastGetComponent<UIButton>("ClickEventObj");
       if( null == m_btn_ClickEventObj )
       {
            Engine.Utility.Log.Error("m_btn_ClickEventObj 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_ClickEventObj.gameObject).onClick = _onClick_ClickEventObj_Btn;
    }

    void _onClick_ClickEventObj_Btn(GameObject caster)
    {
        onClick_ClickEventObj_Btn( caster );
    }


}
