//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ClanDeclareWarCostPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_Close;

    UILabel              m_label_Name;

    UILabel              m_label_DeclareWarTarget;

    UILabel              m_label_DeclareWarDur;

    UILabel              m_label_ZijinCost;

    UILabel              m_label_ZugongCost;

    UIButton             m_btn_BtnCancel;

    UILabel              m_label_CancelTxt;

    UIButton             m_btn_BtnDeclareWar;

    UILabel              m_label_ConfirmTxt;


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
        m_label_Name = fastComponent.FastGetComponent<UILabel>("Name");
       if( null == m_label_Name )
       {
            Engine.Utility.Log.Error("m_label_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_DeclareWarTarget = fastComponent.FastGetComponent<UILabel>("DeclareWarTarget");
       if( null == m_label_DeclareWarTarget )
       {
            Engine.Utility.Log.Error("m_label_DeclareWarTarget 为空，请检查prefab是否缺乏组件");
       }
        m_label_DeclareWarDur = fastComponent.FastGetComponent<UILabel>("DeclareWarDur");
       if( null == m_label_DeclareWarDur )
       {
            Engine.Utility.Log.Error("m_label_DeclareWarDur 为空，请检查prefab是否缺乏组件");
       }
        m_label_ZijinCost = fastComponent.FastGetComponent<UILabel>("ZijinCost");
       if( null == m_label_ZijinCost )
       {
            Engine.Utility.Log.Error("m_label_ZijinCost 为空，请检查prefab是否缺乏组件");
       }
        m_label_ZugongCost = fastComponent.FastGetComponent<UILabel>("ZugongCost");
       if( null == m_label_ZugongCost )
       {
            Engine.Utility.Log.Error("m_label_ZugongCost 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnCancel = fastComponent.FastGetComponent<UIButton>("BtnCancel");
       if( null == m_btn_BtnCancel )
       {
            Engine.Utility.Log.Error("m_btn_BtnCancel 为空，请检查prefab是否缺乏组件");
       }
        m_label_CancelTxt = fastComponent.FastGetComponent<UILabel>("CancelTxt");
       if( null == m_label_CancelTxt )
       {
            Engine.Utility.Log.Error("m_label_CancelTxt 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnDeclareWar = fastComponent.FastGetComponent<UIButton>("BtnDeclareWar");
       if( null == m_btn_BtnDeclareWar )
       {
            Engine.Utility.Log.Error("m_btn_BtnDeclareWar 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConfirmTxt = fastComponent.FastGetComponent<UILabel>("ConfirmTxt");
       if( null == m_label_ConfirmTxt )
       {
            Engine.Utility.Log.Error("m_label_ConfirmTxt 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_BtnCancel.gameObject).onClick = _onClick_BtnCancel_Btn;
        UIEventListener.Get(m_btn_BtnDeclareWar.gameObject).onClick = _onClick_BtnDeclareWar_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_BtnCancel_Btn(GameObject caster)
    {
        onClick_BtnCancel_Btn( caster );
    }

    void _onClick_BtnDeclareWar_Btn(GameObject caster)
    {
        onClick_BtnDeclareWar_Btn( caster );
    }


}
