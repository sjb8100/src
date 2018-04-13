//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ExchangeMoneyPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_ChangeGoldRoot;

    UIButton             m_btn_YuanBao;

    UIButton             m_btn_YinLiang;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    UIButton             m_btn_quxiao;

    UILabel              m_label_quxiao_Name;

    UIButton             m_btn_queding;

    UILabel              m_label_queding_Name;

    UILabel              m_label_CostNum;

    UISprite             m_sprite_CostIcon;

    UILabel              m_label_MoneyNum;

    UISprite             m_sprite_MoneyIcon;

    UIButton             m_btn_Btn_Less;

    UIButton             m_btn_Btn_Add;

    UIButton             m_btn_InputBtnArea;

    UILabel              m_label_UnitNum;

    UIButton             m_btn_Btn_Max;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_ChangeGoldRoot = fastComponent.FastGetComponent<Transform>("ChangeGoldRoot");
       if( null == m_trans_ChangeGoldRoot )
       {
            Engine.Utility.Log.Error("m_trans_ChangeGoldRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_YuanBao = fastComponent.FastGetComponent<UIButton>("YuanBao");
       if( null == m_btn_YuanBao )
       {
            Engine.Utility.Log.Error("m_btn_YuanBao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_YinLiang = fastComponent.FastGetComponent<UIButton>("YinLiang");
       if( null == m_btn_YinLiang )
       {
            Engine.Utility.Log.Error("m_btn_YinLiang 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_quxiao = fastComponent.FastGetComponent<UIButton>("quxiao");
       if( null == m_btn_quxiao )
       {
            Engine.Utility.Log.Error("m_btn_quxiao 为空，请检查prefab是否缺乏组件");
       }
        m_label_quxiao_Name = fastComponent.FastGetComponent<UILabel>("quxiao_Name");
       if( null == m_label_quxiao_Name )
       {
            Engine.Utility.Log.Error("m_label_quxiao_Name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_queding = fastComponent.FastGetComponent<UIButton>("queding");
       if( null == m_btn_queding )
       {
            Engine.Utility.Log.Error("m_btn_queding 为空，请检查prefab是否缺乏组件");
       }
        m_label_queding_Name = fastComponent.FastGetComponent<UILabel>("queding_Name");
       if( null == m_label_queding_Name )
       {
            Engine.Utility.Log.Error("m_label_queding_Name 为空，请检查prefab是否缺乏组件");
       }
        m_label_CostNum = fastComponent.FastGetComponent<UILabel>("CostNum");
       if( null == m_label_CostNum )
       {
            Engine.Utility.Log.Error("m_label_CostNum 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_CostIcon = fastComponent.FastGetComponent<UISprite>("CostIcon");
       if( null == m_sprite_CostIcon )
       {
            Engine.Utility.Log.Error("m_sprite_CostIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_MoneyNum = fastComponent.FastGetComponent<UILabel>("MoneyNum");
       if( null == m_label_MoneyNum )
       {
            Engine.Utility.Log.Error("m_label_MoneyNum 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_MoneyIcon = fastComponent.FastGetComponent<UISprite>("MoneyIcon");
       if( null == m_sprite_MoneyIcon )
       {
            Engine.Utility.Log.Error("m_sprite_MoneyIcon 为空，请检查prefab是否缺乏组件");
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
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_YuanBao.gameObject).onClick = _onClick_YuanBao_Btn;
        UIEventListener.Get(m_btn_YinLiang.gameObject).onClick = _onClick_YinLiang_Btn;
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_quxiao.gameObject).onClick = _onClick_Quxiao_Btn;
        UIEventListener.Get(m_btn_queding.gameObject).onClick = _onClick_Queding_Btn;
        UIEventListener.Get(m_btn_Btn_Less.gameObject).onClick = _onClick_Btn_Less_Btn;
        UIEventListener.Get(m_btn_Btn_Add.gameObject).onClick = _onClick_Btn_Add_Btn;
        UIEventListener.Get(m_btn_InputBtnArea.gameObject).onClick = _onClick_InputBtnArea_Btn;
        UIEventListener.Get(m_btn_Btn_Max.gameObject).onClick = _onClick_Btn_Max_Btn;
    }

    void _onClick_YuanBao_Btn(GameObject caster)
    {
        onClick_YuanBao_Btn( caster );
    }

    void _onClick_YinLiang_Btn(GameObject caster)
    {
        onClick_YinLiang_Btn( caster );
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Quxiao_Btn(GameObject caster)
    {
        onClick_Quxiao_Btn( caster );
    }

    void _onClick_Queding_Btn(GameObject caster)
    {
        onClick_Queding_Btn( caster );
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


}
