//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MuhonBlendCompletePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_Close;

    UIButton             m_btn_Confirm;

    UILabel              m_label_Label;

    UIGrid               m_grid_AdditiveContent;

    UILabel              m_label_EquipName;

    Transform            m_trans_InfoGridRoot;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_Close = fastComponent.FastGetComponent<UIButton>("Close");
       if( null == m_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Confirm = fastComponent.FastGetComponent<UIButton>("Confirm");
       if( null == m_btn_Confirm )
       {
            Engine.Utility.Log.Error("m_btn_Confirm 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_grid_AdditiveContent = fastComponent.FastGetComponent<UIGrid>("AdditiveContent");
       if( null == m_grid_AdditiveContent )
       {
            Engine.Utility.Log.Error("m_grid_AdditiveContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipName = fastComponent.FastGetComponent<UILabel>("EquipName");
       if( null == m_label_EquipName )
       {
            Engine.Utility.Log.Error("m_label_EquipName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_InfoGridRoot = fastComponent.FastGetComponent<Transform>("InfoGridRoot");
       if( null == m_trans_InfoGridRoot )
       {
            Engine.Utility.Log.Error("m_trans_InfoGridRoot 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_Confirm.gameObject).onClick = _onClick_Confirm_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Confirm_Btn(GameObject caster)
    {
        onClick_Confirm_Btn( caster );
    }


}
