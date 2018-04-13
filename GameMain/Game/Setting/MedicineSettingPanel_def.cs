//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MedicineSettingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_itemprefab;

    UISprite             m_sprite_icon;

    Transform            m_trans_root;

    UIButton             m_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_itemprefab = fastComponent.FastGetComponent<UIWidget>("itemprefab");
       if( null == m_widget_itemprefab )
       {
            Engine.Utility.Log.Error("m_widget_itemprefab 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_icon = fastComponent.FastGetComponent<UISprite>("icon");
       if( null == m_sprite_icon )
       {
            Engine.Utility.Log.Error("m_sprite_icon 为空，请检查prefab是否缺乏组件");
       }
        m_trans_root = fastComponent.FastGetComponent<Transform>("root");
       if( null == m_trans_root )
       {
            Engine.Utility.Log.Error("m_trans_root 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
