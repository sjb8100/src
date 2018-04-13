//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class Achievementpoppanel: UIPanelBase
{

    UILabel              m_label_achievementName_label;

    UILabel              m_label_takelook_label;

    UIButton             m_btn_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
        m_label_achievementName_label = GetChildComponent<UILabel>("achievementName_label");
       if( null == m_label_achievementName_label )
       {
            Engine.Utility.Log.Error("m_label_achievementName_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_takelook_label = GetChildComponent<UILabel>("takelook_label");
       if( null == m_label_takelook_label )
       {
            Engine.Utility.Log.Error("m_label_takelook_label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = GetChildComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
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
