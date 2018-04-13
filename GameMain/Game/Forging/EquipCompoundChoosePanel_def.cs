//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class EquipCompoundChoosePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_Box;

    Transform            m_trans_ResultRoot;

    UIButton             m_btn_Confirm;

    Transform            m_trans_CostRoot;

    UILabel              m_label_AssistName;

    UILabel              m_label_AssistNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_Box = fastComponent.FastGetComponent<UISprite>("Box");
       if( null == m_sprite_Box )
       {
            Engine.Utility.Log.Error("m_sprite_Box 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ResultRoot = fastComponent.FastGetComponent<Transform>("ResultRoot");
       if( null == m_trans_ResultRoot )
       {
            Engine.Utility.Log.Error("m_trans_ResultRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Confirm = fastComponent.FastGetComponent<UIButton>("Confirm");
       if( null == m_btn_Confirm )
       {
            Engine.Utility.Log.Error("m_btn_Confirm 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CostRoot = fastComponent.FastGetComponent<Transform>("CostRoot");
       if( null == m_trans_CostRoot )
       {
            Engine.Utility.Log.Error("m_trans_CostRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_AssistName = fastComponent.FastGetComponent<UILabel>("AssistName");
       if( null == m_label_AssistName )
       {
            Engine.Utility.Log.Error("m_label_AssistName 为空，请检查prefab是否缺乏组件");
       }
        m_label_AssistNum = fastComponent.FastGetComponent<UILabel>("AssistNum");
       if( null == m_label_AssistNum )
       {
            Engine.Utility.Log.Error("m_label_AssistNum 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Confirm.gameObject).onClick = _onClick_Confirm_Btn;
    }

    void _onClick_Confirm_Btn(GameObject caster)
    {
        onClick_Confirm_Btn( caster );
    }


}
