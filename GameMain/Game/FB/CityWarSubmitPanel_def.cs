//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CityWarSubmitPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_queding;

    UILabel              m_label_ItemNameLabel;

    UILabel              m_label_ItemNum;

    UIButton             m_btn_BtnAdd;

    UIButton             m_btn_BtnRemove;

    UIButton             m_btn_takeOutNumInput;

    UILabel              m_label_takeOutNum;

    UIButton             m_btn_BtnMax;

    Transform            m_trans_IconRoot;

    UILabel              m_label_name;

    UIButton             m_btn_close;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_queding = fastComponent.FastGetComponent<UIButton>("btn_queding");
       if( null == m_btn_btn_queding )
       {
            Engine.Utility.Log.Error("m_btn_btn_queding 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemNameLabel = fastComponent.FastGetComponent<UILabel>("ItemNameLabel");
       if( null == m_label_ItemNameLabel )
       {
            Engine.Utility.Log.Error("m_label_ItemNameLabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemNum = fastComponent.FastGetComponent<UILabel>("ItemNum");
       if( null == m_label_ItemNum )
       {
            Engine.Utility.Log.Error("m_label_ItemNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnAdd = fastComponent.FastGetComponent<UIButton>("BtnAdd");
       if( null == m_btn_BtnAdd )
       {
            Engine.Utility.Log.Error("m_btn_BtnAdd 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnRemove = fastComponent.FastGetComponent<UIButton>("BtnRemove");
       if( null == m_btn_BtnRemove )
       {
            Engine.Utility.Log.Error("m_btn_BtnRemove 为空，请检查prefab是否缺乏组件");
       }
        m_btn_takeOutNumInput = fastComponent.FastGetComponent<UIButton>("takeOutNumInput");
       if( null == m_btn_takeOutNumInput )
       {
            Engine.Utility.Log.Error("m_btn_takeOutNumInput 为空，请检查prefab是否缺乏组件");
       }
        m_label_takeOutNum = fastComponent.FastGetComponent<UILabel>("takeOutNum");
       if( null == m_label_takeOutNum )
       {
            Engine.Utility.Log.Error("m_label_takeOutNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnMax = fastComponent.FastGetComponent<UIButton>("BtnMax");
       if( null == m_btn_BtnMax )
       {
            Engine.Utility.Log.Error("m_btn_BtnMax 为空，请检查prefab是否缺乏组件");
       }
        m_trans_IconRoot = fastComponent.FastGetComponent<Transform>("IconRoot");
       if( null == m_trans_IconRoot )
       {
            Engine.Utility.Log.Error("m_trans_IconRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_queding.gameObject).onClick = _onClick_Btn_queding_Btn;
        UIEventListener.Get(m_btn_BtnAdd.gameObject).onClick = _onClick_BtnAdd_Btn;
        UIEventListener.Get(m_btn_BtnRemove.gameObject).onClick = _onClick_BtnRemove_Btn;
        UIEventListener.Get(m_btn_takeOutNumInput.gameObject).onClick = _onClick_TakeOutNumInput_Btn;
        UIEventListener.Get(m_btn_BtnMax.gameObject).onClick = _onClick_BtnMax_Btn;
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Btn_queding_Btn(GameObject caster)
    {
        onClick_Btn_queding_Btn( caster );
    }

    void _onClick_BtnAdd_Btn(GameObject caster)
    {
        onClick_BtnAdd_Btn( caster );
    }

    void _onClick_BtnRemove_Btn(GameObject caster)
    {
        onClick_BtnRemove_Btn( caster );
    }

    void _onClick_TakeOutNumInput_Btn(GameObject caster)
    {
        onClick_TakeOutNumInput_Btn( caster );
    }

    void _onClick_BtnMax_Btn(GameObject caster)
    {
        onClick_BtnMax_Btn( caster );
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
