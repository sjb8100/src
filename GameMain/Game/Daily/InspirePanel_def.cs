//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class InspirePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    UILabel              m_label_GoldInspEffect;

    UILabel              m_label_GoldInspLeft;

    UIButton             m_btn_BtnGoldInspire;

    UISprite             m_sprite_GoldInspireCostIcon;

    UILabel              m_label_GoldInspireCostNum;

    UILabel              m_label_BYuanInspEffect;

    UILabel              m_label_BYuanInspLeft;

    UIButton             m_btn_BtnBYuanInspire;

    UISprite             m_sprite_BYuanInspireCostIcon;

    UILabel              m_label_BYuanInspireCostNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_GoldInspEffect = fastComponent.FastGetComponent<UILabel>("GoldInspEffect");
       if( null == m_label_GoldInspEffect )
       {
            Engine.Utility.Log.Error("m_label_GoldInspEffect 为空，请检查prefab是否缺乏组件");
       }
        m_label_GoldInspLeft = fastComponent.FastGetComponent<UILabel>("GoldInspLeft");
       if( null == m_label_GoldInspLeft )
       {
            Engine.Utility.Log.Error("m_label_GoldInspLeft 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnGoldInspire = fastComponent.FastGetComponent<UIButton>("BtnGoldInspire");
       if( null == m_btn_BtnGoldInspire )
       {
            Engine.Utility.Log.Error("m_btn_BtnGoldInspire 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_GoldInspireCostIcon = fastComponent.FastGetComponent<UISprite>("GoldInspireCostIcon");
       if( null == m_sprite_GoldInspireCostIcon )
       {
            Engine.Utility.Log.Error("m_sprite_GoldInspireCostIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_GoldInspireCostNum = fastComponent.FastGetComponent<UILabel>("GoldInspireCostNum");
       if( null == m_label_GoldInspireCostNum )
       {
            Engine.Utility.Log.Error("m_label_GoldInspireCostNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_BYuanInspEffect = fastComponent.FastGetComponent<UILabel>("BYuanInspEffect");
       if( null == m_label_BYuanInspEffect )
       {
            Engine.Utility.Log.Error("m_label_BYuanInspEffect 为空，请检查prefab是否缺乏组件");
       }
        m_label_BYuanInspLeft = fastComponent.FastGetComponent<UILabel>("BYuanInspLeft");
       if( null == m_label_BYuanInspLeft )
       {
            Engine.Utility.Log.Error("m_label_BYuanInspLeft 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnBYuanInspire = fastComponent.FastGetComponent<UIButton>("BtnBYuanInspire");
       if( null == m_btn_BtnBYuanInspire )
       {
            Engine.Utility.Log.Error("m_btn_BtnBYuanInspire 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_BYuanInspireCostIcon = fastComponent.FastGetComponent<UISprite>("BYuanInspireCostIcon");
       if( null == m_sprite_BYuanInspireCostIcon )
       {
            Engine.Utility.Log.Error("m_sprite_BYuanInspireCostIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_BYuanInspireCostNum = fastComponent.FastGetComponent<UILabel>("BYuanInspireCostNum");
       if( null == m_label_BYuanInspireCostNum )
       {
            Engine.Utility.Log.Error("m_label_BYuanInspireCostNum 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_BtnGoldInspire.gameObject).onClick = _onClick_BtnGoldInspire_Btn;
        UIEventListener.Get(m_btn_BtnBYuanInspire.gameObject).onClick = _onClick_BtnBYuanInspire_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_BtnGoldInspire_Btn(GameObject caster)
    {
        onClick_BtnGoldInspire_Btn( caster );
    }

    void _onClick_BtnBYuanInspire_Btn(GameObject caster)
    {
        onClick_BtnBYuanInspire_Btn( caster );
    }


}
