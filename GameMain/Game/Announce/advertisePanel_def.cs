//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class advertisePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_citywar;

    UIButton             m_btn_Colsebtn;

    Transform            m_trans_huangling;

    UIButton             m_btn_Colsebtn2;

    UIButton             m_btn_checkbtn;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_citywar = fastComponent.FastGetComponent<Transform>("citywar");
       if( null == m_trans_citywar )
       {
            Engine.Utility.Log.Error("m_trans_citywar 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Colsebtn = fastComponent.FastGetComponent<UIButton>("Colsebtn");
       if( null == m_btn_Colsebtn )
       {
            Engine.Utility.Log.Error("m_btn_Colsebtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_huangling = fastComponent.FastGetComponent<Transform>("huangling");
       if( null == m_trans_huangling )
       {
            Engine.Utility.Log.Error("m_trans_huangling 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Colsebtn2 = fastComponent.FastGetComponent<UIButton>("Colsebtn2");
       if( null == m_btn_Colsebtn2 )
       {
            Engine.Utility.Log.Error("m_btn_Colsebtn2 为空，请检查prefab是否缺乏组件");
       }
        m_btn_checkbtn = fastComponent.FastGetComponent<UIButton>("checkbtn");
       if( null == m_btn_checkbtn )
       {
            Engine.Utility.Log.Error("m_btn_checkbtn 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Colsebtn.gameObject).onClick = _onClick_Colsebtn_Btn;
        UIEventListener.Get(m_btn_Colsebtn2.gameObject).onClick = _onClick_Colsebtn2_Btn;
        UIEventListener.Get(m_btn_checkbtn.gameObject).onClick = _onClick_Checkbtn_Btn;
    }

    void _onClick_Colsebtn_Btn(GameObject caster)
    {
        onClick_Colsebtn_Btn( caster );
    }

    void _onClick_Colsebtn2_Btn(GameObject caster)
    {
        onClick_Colsebtn2_Btn( caster );
    }

    void _onClick_Checkbtn_Btn(GameObject caster)
    {
        onClick_Checkbtn_Btn( caster );
    }


}
