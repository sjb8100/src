//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PurchaseNumPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_CarryShopPurchaseDialogClose;

    UIButton             m_btn_CarryShopPurchaseDialogConfirm;

    UIButton             m_btn_CarryShopPurchaseDialogCancel;

    UISprite             m_sprite_CarryShopPurchaseIcoBg;

    UILabel              m_label_CarryShopPurchaseName;

    UILabel              m_label_CarryShopPurchaseUseLv;

    UILabel              m_label_CarryShopPurchaseUseDesc;

    Transform            m_trans_PurchaseItemBaseGrid;

    UIButton             m_btn_CarryShopPurchaseDialogAdd;

    UIButton             m_btn_CarryShopPurchaseDialogSub;

    UIButton             m_btn_CarryShopPurchaseHandInput;

    UILabel              m_label_CarryShopPurchaseNum;

    UIButton             m_btn_CarryShopPurchaseMax;

    UILabel              m_label_CarryShopPurchaseQuotaNum;

    UILabel              m_label_CarryShopPurchaseTotalGainNum;

    UISprite             m_sprite_CarryShopPurchaseTotalGainIcon;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_CarryShopPurchaseDialogClose = fastComponent.FastGetComponent<UIButton>("CarryShopPurchaseDialogClose");
       if( null == m_btn_CarryShopPurchaseDialogClose )
       {
            Engine.Utility.Log.Error("m_btn_CarryShopPurchaseDialogClose 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CarryShopPurchaseDialogConfirm = fastComponent.FastGetComponent<UIButton>("CarryShopPurchaseDialogConfirm");
       if( null == m_btn_CarryShopPurchaseDialogConfirm )
       {
            Engine.Utility.Log.Error("m_btn_CarryShopPurchaseDialogConfirm 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CarryShopPurchaseDialogCancel = fastComponent.FastGetComponent<UIButton>("CarryShopPurchaseDialogCancel");
       if( null == m_btn_CarryShopPurchaseDialogCancel )
       {
            Engine.Utility.Log.Error("m_btn_CarryShopPurchaseDialogCancel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_CarryShopPurchaseIcoBg = fastComponent.FastGetComponent<UISprite>("CarryShopPurchaseIcoBg");
       if( null == m_sprite_CarryShopPurchaseIcoBg )
       {
            Engine.Utility.Log.Error("m_sprite_CarryShopPurchaseIcoBg 为空，请检查prefab是否缺乏组件");
       }
        m_label_CarryShopPurchaseName = fastComponent.FastGetComponent<UILabel>("CarryShopPurchaseName");
       if( null == m_label_CarryShopPurchaseName )
       {
            Engine.Utility.Log.Error("m_label_CarryShopPurchaseName 为空，请检查prefab是否缺乏组件");
       }
        m_label_CarryShopPurchaseUseLv = fastComponent.FastGetComponent<UILabel>("CarryShopPurchaseUseLv");
       if( null == m_label_CarryShopPurchaseUseLv )
       {
            Engine.Utility.Log.Error("m_label_CarryShopPurchaseUseLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_CarryShopPurchaseUseDesc = fastComponent.FastGetComponent<UILabel>("CarryShopPurchaseUseDesc");
       if( null == m_label_CarryShopPurchaseUseDesc )
       {
            Engine.Utility.Log.Error("m_label_CarryShopPurchaseUseDesc 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PurchaseItemBaseGrid = fastComponent.FastGetComponent<Transform>("PurchaseItemBaseGrid");
       if( null == m_trans_PurchaseItemBaseGrid )
       {
            Engine.Utility.Log.Error("m_trans_PurchaseItemBaseGrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CarryShopPurchaseDialogAdd = fastComponent.FastGetComponent<UIButton>("CarryShopPurchaseDialogAdd");
       if( null == m_btn_CarryShopPurchaseDialogAdd )
       {
            Engine.Utility.Log.Error("m_btn_CarryShopPurchaseDialogAdd 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CarryShopPurchaseDialogSub = fastComponent.FastGetComponent<UIButton>("CarryShopPurchaseDialogSub");
       if( null == m_btn_CarryShopPurchaseDialogSub )
       {
            Engine.Utility.Log.Error("m_btn_CarryShopPurchaseDialogSub 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CarryShopPurchaseHandInput = fastComponent.FastGetComponent<UIButton>("CarryShopPurchaseHandInput");
       if( null == m_btn_CarryShopPurchaseHandInput )
       {
            Engine.Utility.Log.Error("m_btn_CarryShopPurchaseHandInput 为空，请检查prefab是否缺乏组件");
       }
        m_label_CarryShopPurchaseNum = fastComponent.FastGetComponent<UILabel>("CarryShopPurchaseNum");
       if( null == m_label_CarryShopPurchaseNum )
       {
            Engine.Utility.Log.Error("m_label_CarryShopPurchaseNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CarryShopPurchaseMax = fastComponent.FastGetComponent<UIButton>("CarryShopPurchaseMax");
       if( null == m_btn_CarryShopPurchaseMax )
       {
            Engine.Utility.Log.Error("m_btn_CarryShopPurchaseMax 为空，请检查prefab是否缺乏组件");
       }
        m_label_CarryShopPurchaseQuotaNum = fastComponent.FastGetComponent<UILabel>("CarryShopPurchaseQuotaNum");
       if( null == m_label_CarryShopPurchaseQuotaNum )
       {
            Engine.Utility.Log.Error("m_label_CarryShopPurchaseQuotaNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_CarryShopPurchaseTotalGainNum = fastComponent.FastGetComponent<UILabel>("CarryShopPurchaseTotalGainNum");
       if( null == m_label_CarryShopPurchaseTotalGainNum )
       {
            Engine.Utility.Log.Error("m_label_CarryShopPurchaseTotalGainNum 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_CarryShopPurchaseTotalGainIcon = fastComponent.FastGetComponent<UISprite>("CarryShopPurchaseTotalGainIcon");
       if( null == m_sprite_CarryShopPurchaseTotalGainIcon )
       {
            Engine.Utility.Log.Error("m_sprite_CarryShopPurchaseTotalGainIcon 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_CarryShopPurchaseDialogClose.gameObject).onClick = _onClick_CarryShopPurchaseDialogClose_Btn;
        UIEventListener.Get(m_btn_CarryShopPurchaseDialogConfirm.gameObject).onClick = _onClick_CarryShopPurchaseDialogConfirm_Btn;
        UIEventListener.Get(m_btn_CarryShopPurchaseDialogCancel.gameObject).onClick = _onClick_CarryShopPurchaseDialogCancel_Btn;
        UIEventListener.Get(m_btn_CarryShopPurchaseDialogAdd.gameObject).onClick = _onClick_CarryShopPurchaseDialogAdd_Btn;
        UIEventListener.Get(m_btn_CarryShopPurchaseDialogSub.gameObject).onClick = _onClick_CarryShopPurchaseDialogSub_Btn;
        UIEventListener.Get(m_btn_CarryShopPurchaseHandInput.gameObject).onClick = _onClick_CarryShopPurchaseHandInput_Btn;
        UIEventListener.Get(m_btn_CarryShopPurchaseMax.gameObject).onClick = _onClick_CarryShopPurchaseMax_Btn;
    }

    void _onClick_CarryShopPurchaseDialogClose_Btn(GameObject caster)
    {
        onClick_CarryShopPurchaseDialogClose_Btn( caster );
    }

    void _onClick_CarryShopPurchaseDialogConfirm_Btn(GameObject caster)
    {
        onClick_CarryShopPurchaseDialogConfirm_Btn( caster );
    }

    void _onClick_CarryShopPurchaseDialogCancel_Btn(GameObject caster)
    {
        onClick_CarryShopPurchaseDialogCancel_Btn( caster );
    }

    void _onClick_CarryShopPurchaseDialogAdd_Btn(GameObject caster)
    {
        onClick_CarryShopPurchaseDialogAdd_Btn( caster );
    }

    void _onClick_CarryShopPurchaseDialogSub_Btn(GameObject caster)
    {
        onClick_CarryShopPurchaseDialogSub_Btn( caster );
    }

    void _onClick_CarryShopPurchaseHandInput_Btn(GameObject caster)
    {
        onClick_CarryShopPurchaseHandInput_Btn( caster );
    }

    void _onClick_CarryShopPurchaseMax_Btn(GameObject caster)
    {
        onClick_CarryShopPurchaseMax_Btn( caster );
    }


}
