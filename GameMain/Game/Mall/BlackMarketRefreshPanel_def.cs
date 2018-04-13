//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class BlackMarketRefreshPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    Transform            m_trans_RefreshCost;

    UILabel              m_label_RefreshTimes;

    UIButton             m_btn_BtnRefresh;

    UIButton             m_btn_BtnCancel;


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
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RefreshCost = fastComponent.FastGetComponent<Transform>("RefreshCost");
       if( null == m_trans_RefreshCost )
       {
            Engine.Utility.Log.Error("m_trans_RefreshCost 为空，请检查prefab是否缺乏组件");
       }
        m_label_RefreshTimes = fastComponent.FastGetComponent<UILabel>("RefreshTimes");
       if( null == m_label_RefreshTimes )
       {
            Engine.Utility.Log.Error("m_label_RefreshTimes 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnRefresh = fastComponent.FastGetComponent<UIButton>("BtnRefresh");
       if( null == m_btn_BtnRefresh )
       {
            Engine.Utility.Log.Error("m_btn_BtnRefresh 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnCancel = fastComponent.FastGetComponent<UIButton>("BtnCancel");
       if( null == m_btn_BtnCancel )
       {
            Engine.Utility.Log.Error("m_btn_BtnCancel 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_BtnRefresh.gameObject).onClick = _onClick_BtnRefresh_Btn;
        UIEventListener.Get(m_btn_BtnCancel.gameObject).onClick = _onClick_BtnCancel_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_BtnRefresh_Btn(GameObject caster)
    {
        onClick_BtnRefresh_Btn( caster );
    }

    void _onClick_BtnCancel_Btn(GameObject caster)
    {
        onClick_BtnCancel_Btn( caster );
    }


}
