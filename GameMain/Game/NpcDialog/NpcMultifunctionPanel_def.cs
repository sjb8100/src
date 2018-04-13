//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class NpcMultifunctionPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UILabel       m_label_Npc_Name;

    UIButton      m_btn_close;

    UIWidget      m_widget_RichText;

    UIButton      m_btn_btn;

    UILabel       m_label_area_1_name;

    Transform     m_trans_BtnRoot;


    //初始化控件变量
    protected override void InitControls()
    {
        m_label_Npc_Name = GetChildComponent<UILabel>("Npc_Name");
       if( null == m_label_Npc_Name )
       {
            Engine.Utility.Log.Error("m_label_Npc_Name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = GetChildComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RichText = GetChildComponent<UIWidget>("RichText");
       if( null == m_widget_RichText )
       {
            Engine.Utility.Log.Error("m_widget_RichText 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn = GetChildComponent<UIButton>("btn");
       if( null == m_btn_btn )
       {
            Engine.Utility.Log.Error("m_btn_btn 为空，请检查prefab是否缺乏组件");
       }
        m_label_area_1_name = GetChildComponent<UILabel>("area_1_name");
       if( null == m_label_area_1_name )
       {
            Engine.Utility.Log.Error("m_label_area_1_name 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BtnRoot = GetChildComponent<Transform>("BtnRoot");
       if( null == m_trans_BtnRoot )
       {
            Engine.Utility.Log.Error("m_trans_BtnRoot 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_btn.gameObject).onClick = _onClick_Btn_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Btn_Btn(GameObject caster)
    {
        onClick_Btn_Btn( caster );
    }


}
