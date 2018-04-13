//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CommonUseItemPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_close;

    UILabel              m_label_TitleLabel;

    Transform            m_trans_ItemRoot;

    UILabel              m_label_Name;

    UILabel              m_label_Description;

    UIButton             m_btn_Btn_Less;

    UIButton             m_btn_Btn_Add;

    UIButton             m_btn_InputBtnArea;

    UILabel              m_label_UnitNum;

    UIButton             m_btn_Btn_Max;

    Transform            m_trans_buzu;

    UIButton             m_btn_zidongbuzu;

    Transform            m_trans_Times;

    UILabel              m_label_TimesNum;

    UIButton             m_btn_Btn_Use;

    UIButton             m_btn_Btn_Canel;

    Transform            m_trans_UseCost;

    UILabel              m_label_UseCostNum;


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
        m_label_TitleLabel = fastComponent.FastGetComponent<UILabel>("TitleLabel");
       if( null == m_label_TitleLabel )
       {
            Engine.Utility.Log.Error("m_label_TitleLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemRoot = fastComponent.FastGetComponent<Transform>("ItemRoot");
       if( null == m_trans_ItemRoot )
       {
            Engine.Utility.Log.Error("m_trans_ItemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_Name = fastComponent.FastGetComponent<UILabel>("Name");
       if( null == m_label_Name )
       {
            Engine.Utility.Log.Error("m_label_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_Description = fastComponent.FastGetComponent<UILabel>("Description");
       if( null == m_label_Description )
       {
            Engine.Utility.Log.Error("m_label_Description 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Less = fastComponent.FastGetComponent<UIButton>("Btn_Less");
       if( null == m_btn_Btn_Less )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Less 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Add = fastComponent.FastGetComponent<UIButton>("Btn_Add");
       if( null == m_btn_Btn_Add )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Add 为空，请检查prefab是否缺乏组件");
       }
        m_btn_InputBtnArea = fastComponent.FastGetComponent<UIButton>("InputBtnArea");
       if( null == m_btn_InputBtnArea )
       {
            Engine.Utility.Log.Error("m_btn_InputBtnArea 为空，请检查prefab是否缺乏组件");
       }
        m_label_UnitNum = fastComponent.FastGetComponent<UILabel>("UnitNum");
       if( null == m_label_UnitNum )
       {
            Engine.Utility.Log.Error("m_label_UnitNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Max = fastComponent.FastGetComponent<UIButton>("Btn_Max");
       if( null == m_btn_Btn_Max )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Max 为空，请检查prefab是否缺乏组件");
       }
        m_trans_buzu = fastComponent.FastGetComponent<Transform>("buzu");
       if( null == m_trans_buzu )
       {
            Engine.Utility.Log.Error("m_trans_buzu 为空，请检查prefab是否缺乏组件");
       }
        m_btn_zidongbuzu = fastComponent.FastGetComponent<UIButton>("zidongbuzu");
       if( null == m_btn_zidongbuzu )
       {
            Engine.Utility.Log.Error("m_btn_zidongbuzu 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Times = fastComponent.FastGetComponent<Transform>("Times");
       if( null == m_trans_Times )
       {
            Engine.Utility.Log.Error("m_trans_Times 为空，请检查prefab是否缺乏组件");
       }
        m_label_TimesNum = fastComponent.FastGetComponent<UILabel>("TimesNum");
       if( null == m_label_TimesNum )
       {
            Engine.Utility.Log.Error("m_label_TimesNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Use = fastComponent.FastGetComponent<UIButton>("Btn_Use");
       if( null == m_btn_Btn_Use )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Use 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Btn_Canel = fastComponent.FastGetComponent<UIButton>("Btn_Canel");
       if( null == m_btn_Btn_Canel )
       {
            Engine.Utility.Log.Error("m_btn_Btn_Canel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UseCost = fastComponent.FastGetComponent<Transform>("UseCost");
       if( null == m_trans_UseCost )
       {
            Engine.Utility.Log.Error("m_trans_UseCost 为空，请检查prefab是否缺乏组件");
       }
        m_label_UseCostNum = fastComponent.FastGetComponent<UILabel>("UseCostNum");
       if( null == m_label_UseCostNum )
       {
            Engine.Utility.Log.Error("m_label_UseCostNum 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_Btn_Less.gameObject).onClick = _onClick_Btn_Less_Btn;
        UIEventListener.Get(m_btn_Btn_Add.gameObject).onClick = _onClick_Btn_Add_Btn;
        UIEventListener.Get(m_btn_InputBtnArea.gameObject).onClick = _onClick_InputBtnArea_Btn;
        UIEventListener.Get(m_btn_Btn_Max.gameObject).onClick = _onClick_Btn_Max_Btn;
        UIEventListener.Get(m_btn_zidongbuzu.gameObject).onClick = _onClick_Zidongbuzu_Btn;
        UIEventListener.Get(m_btn_Btn_Use.gameObject).onClick = _onClick_Btn_Use_Btn;
        UIEventListener.Get(m_btn_Btn_Canel.gameObject).onClick = _onClick_Btn_Canel_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_Btn_Less_Btn(GameObject caster)
    {
        onClick_Btn_Less_Btn( caster );
    }

    void _onClick_Btn_Add_Btn(GameObject caster)
    {
        onClick_Btn_Add_Btn( caster );
    }

    void _onClick_InputBtnArea_Btn(GameObject caster)
    {
        onClick_InputBtnArea_Btn( caster );
    }

    void _onClick_Btn_Max_Btn(GameObject caster)
    {
        onClick_Btn_Max_Btn( caster );
    }

    void _onClick_Zidongbuzu_Btn(GameObject caster)
    {
        onClick_Zidongbuzu_Btn( caster );
    }

    void _onClick_Btn_Use_Btn(GameObject caster)
    {
        onClick_Btn_Use_Btn( caster );
    }

    void _onClick_Btn_Canel_Btn(GameObject caster)
    {
        onClick_Btn_Canel_Btn( caster );
    }


}
