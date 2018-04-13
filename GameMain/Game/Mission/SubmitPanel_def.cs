//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SubmitPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_close;

    UIButton             m_btn_btn_submit;

    UIGrid               m_grid_root;

    Transform            m_trans_select;


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
        m_btn_btn_submit = fastComponent.FastGetComponent<UIButton>("btn_submit");
       if( null == m_btn_btn_submit )
       {
            Engine.Utility.Log.Error("m_btn_btn_submit 为空，请检查prefab是否缺乏组件");
       }
        m_grid_root = fastComponent.FastGetComponent<UIGrid>("root");
       if( null == m_grid_root )
       {
            Engine.Utility.Log.Error("m_grid_root 为空，请检查prefab是否缺乏组件");
       }
        m_trans_select = fastComponent.FastGetComponent<Transform>("select");
       if( null == m_trans_select )
       {
            Engine.Utility.Log.Error("m_trans_select 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_submit.gameObject).onClick = _onClick_Btn_submit_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_Btn_submit_Btn(GameObject caster)
    {
        onClick_Btn_submit_Btn( caster );
    }


}
