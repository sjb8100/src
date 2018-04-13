//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ShowModelPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UIWidget             m_widget_noclick;

    UILabel              m_label_Name;

    UILabel              m_label_Des;

    UITexture            m__Model;

    UILabel              m_label_BiaoTi;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_widget_noclick = fastComponent.FastGetComponent<UIWidget>("noclick");
       if( null == m_widget_noclick )
       {
            Engine.Utility.Log.Error("m_widget_noclick 为空，请检查prefab是否缺乏组件");
       }
        m_label_Name = fastComponent.FastGetComponent<UILabel>("Name");
       if( null == m_label_Name )
       {
            Engine.Utility.Log.Error("m_label_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_Des = fastComponent.FastGetComponent<UILabel>("Des");
       if( null == m_label_Des )
       {
            Engine.Utility.Log.Error("m_label_Des 为空，请检查prefab是否缺乏组件");
       }
        m__Model = fastComponent.FastGetComponent<UITexture>("Model");
       if( null == m__Model )
       {
            Engine.Utility.Log.Error("m__Model 为空，请检查prefab是否缺乏组件");
       }
        m_label_BiaoTi = fastComponent.FastGetComponent<UILabel>("BiaoTi");
       if( null == m_label_BiaoTi )
       {
            Engine.Utility.Log.Error("m_label_BiaoTi 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
