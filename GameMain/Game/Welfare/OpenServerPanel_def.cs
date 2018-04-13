//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class OpenServerPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIGridCreatorBase    m_ctor_dayRoot;

    UIButton             m_btn_DetailBtn;

    UIButton             m_btn_Colsebtn;

    Transform            m_trans_UIOpenServerGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_ctor_dayRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("dayRoot");
       if( null == m_ctor_dayRoot )
       {
            Engine.Utility.Log.Error("m_ctor_dayRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_DetailBtn = fastComponent.FastGetComponent<UIButton>("DetailBtn");
       if( null == m_btn_DetailBtn )
       {
            Engine.Utility.Log.Error("m_btn_DetailBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Colsebtn = fastComponent.FastGetComponent<UIButton>("Colsebtn");
       if( null == m_btn_Colsebtn )
       {
            Engine.Utility.Log.Error("m_btn_Colsebtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIOpenServerGrid = fastComponent.FastGetComponent<Transform>("UIOpenServerGrid");
       if( null == m_trans_UIOpenServerGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIOpenServerGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_DetailBtn.gameObject).onClick = _onClick_DetailBtn_Btn;
        UIEventListener.Get(m_btn_Colsebtn.gameObject).onClick = _onClick_Colsebtn_Btn;
    }

    void _onClick_DetailBtn_Btn(GameObject caster)
    {
        onClick_DetailBtn_Btn( caster );
    }

    void _onClick_Colsebtn_Btn(GameObject caster)
    {
        onClick_Colsebtn_Btn( caster );
    }


}
