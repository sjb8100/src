//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class BuyMoneyPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_Penny;

    UIButton             m_btn_Gold;

    UIButton             m_btn_close;

    UILabel              m_label_name;

    UIButton             m_btn_quxiao;

    UILabel              m_label_quxiao_Name;

    UIButton             m_btn_queding;

    UILabel              m_label_queding_Name;

    UISprite             m_sprite_MyYuanbaoIcon;

    UILabel              m_label_MyYuanbaoNum;

    UIButton             m_btn_InputBtn;

    UILabel              m_label_Des;

    Transform            m_trans_GetPennyContent;

    UISprite             m_sprite_GetPennyIcon;

    UILabel              m_label_GetPennyNum;

    Transform            m_trans_GetGoldContent;

    UISprite             m_sprite_GetGoldIcon;

    UILabel              m_label_GetGoldNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_Penny = fastComponent.FastGetComponent<UIButton>("Penny");
       if( null == m_btn_Penny )
       {
            Engine.Utility.Log.Error("m_btn_Penny 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Gold = fastComponent.FastGetComponent<UIButton>("Gold");
       if( null == m_btn_Gold )
       {
            Engine.Utility.Log.Error("m_btn_Gold 为空，请检查prefab是否缺乏组件");
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
        m_sprite_MyYuanbaoIcon = fastComponent.FastGetComponent<UISprite>("MyYuanbaoIcon");
       if( null == m_sprite_MyYuanbaoIcon )
       {
            Engine.Utility.Log.Error("m_sprite_MyYuanbaoIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyYuanbaoNum = fastComponent.FastGetComponent<UILabel>("MyYuanbaoNum");
       if( null == m_label_MyYuanbaoNum )
       {
            Engine.Utility.Log.Error("m_label_MyYuanbaoNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_InputBtn = fastComponent.FastGetComponent<UIButton>("InputBtn");
       if( null == m_btn_InputBtn )
       {
            Engine.Utility.Log.Error("m_btn_InputBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_Des = fastComponent.FastGetComponent<UILabel>("Des");
       if( null == m_label_Des )
       {
            Engine.Utility.Log.Error("m_label_Des 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GetPennyContent = fastComponent.FastGetComponent<Transform>("GetPennyContent");
       if( null == m_trans_GetPennyContent )
       {
            Engine.Utility.Log.Error("m_trans_GetPennyContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_GetPennyIcon = fastComponent.FastGetComponent<UISprite>("GetPennyIcon");
       if( null == m_sprite_GetPennyIcon )
       {
            Engine.Utility.Log.Error("m_sprite_GetPennyIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_GetPennyNum = fastComponent.FastGetComponent<UILabel>("GetPennyNum");
       if( null == m_label_GetPennyNum )
       {
            Engine.Utility.Log.Error("m_label_GetPennyNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GetGoldContent = fastComponent.FastGetComponent<Transform>("GetGoldContent");
       if( null == m_trans_GetGoldContent )
       {
            Engine.Utility.Log.Error("m_trans_GetGoldContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_GetGoldIcon = fastComponent.FastGetComponent<UISprite>("GetGoldIcon");
       if( null == m_sprite_GetGoldIcon )
       {
            Engine.Utility.Log.Error("m_sprite_GetGoldIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_GetGoldNum = fastComponent.FastGetComponent<UILabel>("GetGoldNum");
       if( null == m_label_GetGoldNum )
       {
            Engine.Utility.Log.Error("m_label_GetGoldNum 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Penny.gameObject).onClick = _onClick_Penny_Btn;
        UIEventListener.Get(m_btn_Gold.gameObject).onClick = _onClick_Gold_Btn;
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_quxiao.gameObject).onClick = _onClick_Quxiao_Btn;
        UIEventListener.Get(m_btn_queding.gameObject).onClick = _onClick_Queding_Btn;
        UIEventListener.Get(m_btn_InputBtn.gameObject).onClick = _onClick_InputBtn_Btn;
    }

    void _onClick_Penny_Btn(GameObject caster)
    {
        onClick_Penny_Btn( caster );
    }

    void _onClick_Gold_Btn(GameObject caster)
    {
        onClick_Gold_Btn( caster );
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

    void _onClick_InputBtn_Btn(GameObject caster)
    {
        onClick_InputBtn_Btn( caster );
    }


}
