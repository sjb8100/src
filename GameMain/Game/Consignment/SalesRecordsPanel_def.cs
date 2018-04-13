//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SalesRecordsPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_BiaoTi_Label;

    Transform            m_trans_SellListContent;

    UIGridCreatorBase    m_ctor_SellListScrollViewContent;

    UILabel              m_label_Title;

    UIButton             m_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_BiaoTi_Label = fastComponent.FastGetComponent<UILabel>("BiaoTi_Label");
       if( null == m_label_BiaoTi_Label )
       {
            Engine.Utility.Log.Error("m_label_BiaoTi_Label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SellListContent = fastComponent.FastGetComponent<Transform>("SellListContent");
       if( null == m_trans_SellListContent )
       {
            Engine.Utility.Log.Error("m_trans_SellListContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_SellListScrollViewContent = fastComponent.FastGetComponent<UIGridCreatorBase>("SellListScrollViewContent");
       if( null == m_ctor_SellListScrollViewContent )
       {
            Engine.Utility.Log.Error("m_ctor_SellListScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_Title = fastComponent.FastGetComponent<UILabel>("Title");
       if( null == m_label_Title )
       {
            Engine.Utility.Log.Error("m_label_Title 为空，请检查prefab是否缺乏组件");
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
