//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class HistoryMsgPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_close;

    UIWidget             m_widget_btn_unclose;

    UISprite             m_sprite_btn_historymessage;


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
        m_widget_btn_unclose = fastComponent.FastGetComponent<UIWidget>("btn_unclose");
       if( null == m_widget_btn_unclose )
       {
            Engine.Utility.Log.Error("m_widget_btn_unclose 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btn_historymessage = fastComponent.FastGetComponent<UISprite>("btn_historymessage");
       if( null == m_sprite_btn_historymessage )
       {
            Engine.Utility.Log.Error("m_sprite_btn_historymessage 为空，请检查prefab是否缺乏组件");
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
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }


}
