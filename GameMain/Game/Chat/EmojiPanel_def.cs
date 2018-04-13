//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class EmojiPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btnClose;

    Transform            m_trans_btn;

    UISprite             m_sprite_Emoji;

    Transform            m_trans_Root;

    Transform            m_trans_ItemGridScrollView;

    UIWidget             m_widget_btn_close;

    UIWidget             m_widget_btn_close2;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btnClose = fastComponent.FastGetComponent<UIButton>("btnClose");
       if( null == m_btn_btnClose )
       {
            Engine.Utility.Log.Error("m_btn_btnClose 为空，请检查prefab是否缺乏组件");
       }
        m_trans_btn = fastComponent.FastGetComponent<Transform>("btn");
       if( null == m_trans_btn )
       {
            Engine.Utility.Log.Error("m_trans_btn 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Emoji = fastComponent.FastGetComponent<UISprite>("Emoji");
       if( null == m_sprite_Emoji )
       {
            Engine.Utility.Log.Error("m_sprite_Emoji 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Root = fastComponent.FastGetComponent<Transform>("Root");
       if( null == m_trans_Root )
       {
            Engine.Utility.Log.Error("m_trans_Root 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemGridScrollView = fastComponent.FastGetComponent<Transform>("ItemGridScrollView");
       if( null == m_trans_ItemGridScrollView )
       {
            Engine.Utility.Log.Error("m_trans_ItemGridScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_widget_btn_close = fastComponent.FastGetComponent<UIWidget>("btn_close");
       if( null == m_widget_btn_close )
       {
            Engine.Utility.Log.Error("m_widget_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_widget_btn_close2 = fastComponent.FastGetComponent<UIWidget>("btn_close2");
       if( null == m_widget_btn_close2 )
       {
            Engine.Utility.Log.Error("m_widget_btn_close2 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btnClose.gameObject).onClick = _onClick_BtnClose_Btn;
    }

    void _onClick_BtnClose_Btn(GameObject caster)
    {
        onClick_BtnClose_Btn( caster );
    }


}
