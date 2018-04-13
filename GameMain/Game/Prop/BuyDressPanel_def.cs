//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class BuyDressPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_unlock_close;

    UILabel              m_label_unlock_name;

    UIButton             m_btn_unlock_queding;

    UILabel              m_label_suitdes;

    UITexture            m__DressIcon;

    UILabel              m_label_suitName;

    Transform            m_trans_BtnGroup;

    Transform            m_trans_CostGroup;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
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
        m_btn_unlock_queding = fastComponent.FastGetComponent<UIButton>("unlock_queding");
       if( null == m_btn_unlock_queding )
       {
            Engine.Utility.Log.Error("m_btn_unlock_queding 为空，请检查prefab是否缺乏组件");
       }
        m_label_suitdes = fastComponent.FastGetComponent<UILabel>("suitdes");
       if( null == m_label_suitdes )
       {
            Engine.Utility.Log.Error("m_label_suitdes 为空，请检查prefab是否缺乏组件");
       }
        m__DressIcon = fastComponent.FastGetComponent<UITexture>("DressIcon");
       if( null == m__DressIcon )
       {
            Engine.Utility.Log.Error("m__DressIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_suitName = fastComponent.FastGetComponent<UILabel>("suitName");
       if( null == m_label_suitName )
       {
            Engine.Utility.Log.Error("m_label_suitName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BtnGroup = fastComponent.FastGetComponent<Transform>("BtnGroup");
       if( null == m_trans_BtnGroup )
       {
            Engine.Utility.Log.Error("m_trans_BtnGroup 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CostGroup = fastComponent.FastGetComponent<Transform>("CostGroup");
       if( null == m_trans_CostGroup )
       {
            Engine.Utility.Log.Error("m_trans_CostGroup 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_unlock_queding.gameObject).onClick = _onClick_Unlock_queding_Btn;
    }

    void _onClick_Unlock_close_Btn(GameObject caster)
    {
        onClick_Unlock_close_Btn( caster );
    }

    void _onClick_Unlock_queding_Btn(GameObject caster)
    {
        onClick_Unlock_queding_Btn( caster );
    }


}
