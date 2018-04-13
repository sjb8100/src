//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RedEnvelopeSendPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_btn_Close;

    UIToggle             m_toggle_wolrdselected;

    UIToggle             m_toggle_clanselected;

    UIButton             m_btn_ConsumeBtnAdd;

    UIButton             m_btn_ConsumeBtnRemove;

    UISprite             m_sprite_consumeInput;

    UILabel              m_label_ConsumePurchaseNum;

    UISprite             m_sprite_redmoneyicon;

    UILabel              m_label_goldNum_label;

    UIButton             m_btn_BtnAdd;

    UIButton             m_btn_BtnRemove;

    UISprite             m_sprite_redenvelopenum;

    UILabel              m_label_PurchaseNum;

    UIInput              m_input_blessmassageinput;

    UILabel              m_label_wordText;

    UIButton             m_btn_btn_Send;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_btn_Close = fastComponent.FastGetComponent<UISprite>("btn_Close");
       if( null == m_sprite_btn_Close )
       {
            Engine.Utility.Log.Error("m_sprite_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_wolrdselected = fastComponent.FastGetComponent<UIToggle>("wolrdselected");
       if( null == m_toggle_wolrdselected )
       {
            Engine.Utility.Log.Error("m_toggle_wolrdselected 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_clanselected = fastComponent.FastGetComponent<UIToggle>("clanselected");
       if( null == m_toggle_clanselected )
       {
            Engine.Utility.Log.Error("m_toggle_clanselected 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ConsumeBtnAdd = fastComponent.FastGetComponent<UIButton>("ConsumeBtnAdd");
       if( null == m_btn_ConsumeBtnAdd )
       {
            Engine.Utility.Log.Error("m_btn_ConsumeBtnAdd 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ConsumeBtnRemove = fastComponent.FastGetComponent<UIButton>("ConsumeBtnRemove");
       if( null == m_btn_ConsumeBtnRemove )
       {
            Engine.Utility.Log.Error("m_btn_ConsumeBtnRemove 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_consumeInput = fastComponent.FastGetComponent<UISprite>("consumeInput");
       if( null == m_sprite_consumeInput )
       {
            Engine.Utility.Log.Error("m_sprite_consumeInput 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConsumePurchaseNum = fastComponent.FastGetComponent<UILabel>("ConsumePurchaseNum");
       if( null == m_label_ConsumePurchaseNum )
       {
            Engine.Utility.Log.Error("m_label_ConsumePurchaseNum 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_redmoneyicon = fastComponent.FastGetComponent<UISprite>("redmoneyicon");
       if( null == m_sprite_redmoneyicon )
       {
            Engine.Utility.Log.Error("m_sprite_redmoneyicon 为空，请检查prefab是否缺乏组件");
       }
        m_label_goldNum_label = fastComponent.FastGetComponent<UILabel>("goldNum_label");
       if( null == m_label_goldNum_label )
       {
            Engine.Utility.Log.Error("m_label_goldNum_label 为空，请检查prefab是否缺乏组件");
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
        m_sprite_redenvelopenum = fastComponent.FastGetComponent<UISprite>("redenvelopenum");
       if( null == m_sprite_redenvelopenum )
       {
            Engine.Utility.Log.Error("m_sprite_redenvelopenum 为空，请检查prefab是否缺乏组件");
       }
        m_label_PurchaseNum = fastComponent.FastGetComponent<UILabel>("PurchaseNum");
       if( null == m_label_PurchaseNum )
       {
            Engine.Utility.Log.Error("m_label_PurchaseNum 为空，请检查prefab是否缺乏组件");
       }
        m_input_blessmassageinput = fastComponent.FastGetComponent<UIInput>("blessmassageinput");
       if( null == m_input_blessmassageinput )
       {
            Engine.Utility.Log.Error("m_input_blessmassageinput 为空，请检查prefab是否缺乏组件");
       }
        m_label_wordText = fastComponent.FastGetComponent<UILabel>("wordText");
       if( null == m_label_wordText )
       {
            Engine.Utility.Log.Error("m_label_wordText 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Send = fastComponent.FastGetComponent<UIButton>("btn_Send");
       if( null == m_btn_btn_Send )
       {
            Engine.Utility.Log.Error("m_btn_btn_Send 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_ConsumeBtnAdd.gameObject).onClick = _onClick_ConsumeBtnAdd_Btn;
        UIEventListener.Get(m_btn_ConsumeBtnRemove.gameObject).onClick = _onClick_ConsumeBtnRemove_Btn;
        UIEventListener.Get(m_btn_BtnAdd.gameObject).onClick = _onClick_BtnAdd_Btn;
        UIEventListener.Get(m_btn_BtnRemove.gameObject).onClick = _onClick_BtnRemove_Btn;
        UIEventListener.Get(m_btn_btn_Send.gameObject).onClick = _onClick_Btn_Send_Btn;
    }

    void _onClick_ConsumeBtnAdd_Btn(GameObject caster)
    {
        onClick_ConsumeBtnAdd_Btn( caster );
    }

    void _onClick_ConsumeBtnRemove_Btn(GameObject caster)
    {
        onClick_ConsumeBtnRemove_Btn( caster );
    }

    void _onClick_BtnAdd_Btn(GameObject caster)
    {
        onClick_BtnAdd_Btn( caster );
    }

    void _onClick_BtnRemove_Btn(GameObject caster)
    {
        onClick_BtnRemove_Btn( caster );
    }

    void _onClick_Btn_Send_Btn(GameObject caster)
    {
        onClick_Btn_Send_Btn( caster );
    }


}
