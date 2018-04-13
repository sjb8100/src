//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CityWarPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_CityWarPanel;

    UIButton             m_btn_unlock_close;

    UILabel              m_label_unlock_name;

    Transform            m_trans_WarInfoScrollView;

    Transform            m_trans_TotemScrollView;

    UILabel              m_label_MyKill;

    UILabel              m_label_num;

    UIButton             m_btn_ExitBtn;

    UILabel              m_label_ExitBtnLabel;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_CityWarPanel = fastComponent.FastGetComponent<Transform>("CityWarPanel");
       if( null == m_trans_CityWarPanel )
       {
            Engine.Utility.Log.Error("m_trans_CityWarPanel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_unlock_close = fastComponent.FastGetComponent<UIButton>("unlock_close");
       if( null == m_btn_unlock_close )
       {
            Engine.Utility.Log.Error("m_btn_unlock_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_unlock_name = fastComponent.FastGetComponent<UILabel>("unlock_name");
       if( null == m_label_unlock_name )
       {
            Engine.Utility.Log.Error("m_label_unlock_name 为空，请检查prefab是否缺乏组件");
       }
        m_trans_WarInfoScrollView = fastComponent.FastGetComponent<Transform>("WarInfoScrollView");
       if( null == m_trans_WarInfoScrollView )
       {
            Engine.Utility.Log.Error("m_trans_WarInfoScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TotemScrollView = fastComponent.FastGetComponent<Transform>("TotemScrollView");
       if( null == m_trans_TotemScrollView )
       {
            Engine.Utility.Log.Error("m_trans_TotemScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyKill = fastComponent.FastGetComponent<UILabel>("MyKill");
       if( null == m_label_MyKill )
       {
            Engine.Utility.Log.Error("m_label_MyKill 为空，请检查prefab是否缺乏组件");
       }
        m_label_num = fastComponent.FastGetComponent<UILabel>("num");
       if( null == m_label_num )
       {
            Engine.Utility.Log.Error("m_label_num 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ExitBtn = fastComponent.FastGetComponent<UIButton>("ExitBtn");
       if( null == m_btn_ExitBtn )
       {
            Engine.Utility.Log.Error("m_btn_ExitBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_ExitBtnLabel = fastComponent.FastGetComponent<UILabel>("ExitBtnLabel");
       if( null == m_label_ExitBtnLabel )
       {
            Engine.Utility.Log.Error("m_label_ExitBtnLabel 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_unlock_close.gameObject).onClick = _onClick_Unlock_close_Btn;
        UIEventListener.Get(m_btn_ExitBtn.gameObject).onClick = _onClick_ExitBtn_Btn;
    }

    void _onClick_Unlock_close_Btn(GameObject caster)
    {
        onClick_Unlock_close_Btn( caster );
    }

    void _onClick_ExitBtn_Btn(GameObject caster)
    {
        onClick_ExitBtn_Btn( caster );
    }


}
