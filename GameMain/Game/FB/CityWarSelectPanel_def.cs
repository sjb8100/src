//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CityWarSelectPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_name;

    UIButton             m_btn_close;

    Transform            m_trans_SelectCityWar;

    UIWidget             m_widget_CityWarGrid;

    Transform            m_trans_SelectCityWarContent;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SelectCityWar = fastComponent.FastGetComponent<Transform>("SelectCityWar");
       if( null == m_trans_SelectCityWar )
       {
            Engine.Utility.Log.Error("m_trans_SelectCityWar 为空，请检查prefab是否缺乏组件");
       }
        m_widget_CityWarGrid = fastComponent.FastGetComponent<UIWidget>("CityWarGrid");
       if( null == m_widget_CityWarGrid )
       {
            Engine.Utility.Log.Error("m_widget_CityWarGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SelectCityWarContent = fastComponent.FastGetComponent<Transform>("SelectCityWarContent");
       if( null == m_trans_SelectCityWarContent )
       {
            Engine.Utility.Log.Error("m_trans_SelectCityWarContent 为空，请检查prefab是否缺乏组件");
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
