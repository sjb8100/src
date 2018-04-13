//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ArenaBattlelogPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_BiaoTi_Label;

    UIButton             m_btn_close;

    Transform            m_trans_UIBattlelogScrollView;

    Transform            m_trans_UIArenaBattlelogGrid;


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
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIBattlelogScrollView = fastComponent.FastGetComponent<Transform>("UIBattlelogScrollView");
       if( null == m_trans_UIBattlelogScrollView )
       {
            Engine.Utility.Log.Error("m_trans_UIBattlelogScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIArenaBattlelogGrid = fastComponent.FastGetComponent<Transform>("UIArenaBattlelogGrid");
       if( null == m_trans_UIArenaBattlelogGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIArenaBattlelogGrid 为空，请检查prefab是否缺乏组件");
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
