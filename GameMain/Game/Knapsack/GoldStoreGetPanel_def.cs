//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GoldStoreGetPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UIButton             m_btn_WareHouseStoreCopperBtnClose;

    UIButton             m_btn_WareHouseStoreCopperBtnStore;

    UIButton             m_btn_WareHouseStoreCopperBtnTakeOut;

    UISprite             m_sprite_KnapsackOwnCopperIcon;

    UILabel              m_label_KnapsackOwnCopperStoreNum;

    UISprite             m_sprite_WareHouseCurStoreCoppeIcon;

    UILabel              m_label_WareHouseCurStoreCopperNum;

    UIButton             m_btn_WareHouseStoreCopperAdd;

    UIButton             m_btn_WareHouseStoreCopperSub;

    UILabel              m_label_WareHouseStoreTakeCopperNum;

    UIButton             m_btn_WareHouseStoreCopperHandInput;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_Content = fastComponent.FastGetComponent<Transform>("Content");
       if( null == m_trans_Content )
       {
            Engine.Utility.Log.Error("m_trans_Content 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WareHouseStoreCopperBtnClose = fastComponent.FastGetComponent<UIButton>("WareHouseStoreCopperBtnClose");
       if( null == m_btn_WareHouseStoreCopperBtnClose )
       {
            Engine.Utility.Log.Error("m_btn_WareHouseStoreCopperBtnClose 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WareHouseStoreCopperBtnStore = fastComponent.FastGetComponent<UIButton>("WareHouseStoreCopperBtnStore");
       if( null == m_btn_WareHouseStoreCopperBtnStore )
       {
            Engine.Utility.Log.Error("m_btn_WareHouseStoreCopperBtnStore 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WareHouseStoreCopperBtnTakeOut = fastComponent.FastGetComponent<UIButton>("WareHouseStoreCopperBtnTakeOut");
       if( null == m_btn_WareHouseStoreCopperBtnTakeOut )
       {
            Engine.Utility.Log.Error("m_btn_WareHouseStoreCopperBtnTakeOut 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_KnapsackOwnCopperIcon = fastComponent.FastGetComponent<UISprite>("KnapsackOwnCopperIcon");
       if( null == m_sprite_KnapsackOwnCopperIcon )
       {
            Engine.Utility.Log.Error("m_sprite_KnapsackOwnCopperIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_KnapsackOwnCopperStoreNum = fastComponent.FastGetComponent<UILabel>("KnapsackOwnCopperStoreNum");
       if( null == m_label_KnapsackOwnCopperStoreNum )
       {
            Engine.Utility.Log.Error("m_label_KnapsackOwnCopperStoreNum 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_WareHouseCurStoreCoppeIcon = fastComponent.FastGetComponent<UISprite>("WareHouseCurStoreCoppeIcon");
       if( null == m_sprite_WareHouseCurStoreCoppeIcon )
       {
            Engine.Utility.Log.Error("m_sprite_WareHouseCurStoreCoppeIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_WareHouseCurStoreCopperNum = fastComponent.FastGetComponent<UILabel>("WareHouseCurStoreCopperNum");
       if( null == m_label_WareHouseCurStoreCopperNum )
       {
            Engine.Utility.Log.Error("m_label_WareHouseCurStoreCopperNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WareHouseStoreCopperAdd = fastComponent.FastGetComponent<UIButton>("WareHouseStoreCopperAdd");
       if( null == m_btn_WareHouseStoreCopperAdd )
       {
            Engine.Utility.Log.Error("m_btn_WareHouseStoreCopperAdd 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WareHouseStoreCopperSub = fastComponent.FastGetComponent<UIButton>("WareHouseStoreCopperSub");
       if( null == m_btn_WareHouseStoreCopperSub )
       {
            Engine.Utility.Log.Error("m_btn_WareHouseStoreCopperSub 为空，请检查prefab是否缺乏组件");
       }
        m_label_WareHouseStoreTakeCopperNum = fastComponent.FastGetComponent<UILabel>("WareHouseStoreTakeCopperNum");
       if( null == m_label_WareHouseStoreTakeCopperNum )
       {
            Engine.Utility.Log.Error("m_label_WareHouseStoreTakeCopperNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_WareHouseStoreCopperHandInput = fastComponent.FastGetComponent<UIButton>("WareHouseStoreCopperHandInput");
       if( null == m_btn_WareHouseStoreCopperHandInput )
       {
            Engine.Utility.Log.Error("m_btn_WareHouseStoreCopperHandInput 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_WareHouseStoreCopperBtnClose.gameObject).onClick = _onClick_WareHouseStoreCopperBtnClose_Btn;
        UIEventListener.Get(m_btn_WareHouseStoreCopperBtnStore.gameObject).onClick = _onClick_WareHouseStoreCopperBtnStore_Btn;
        UIEventListener.Get(m_btn_WareHouseStoreCopperBtnTakeOut.gameObject).onClick = _onClick_WareHouseStoreCopperBtnTakeOut_Btn;
        UIEventListener.Get(m_btn_WareHouseStoreCopperAdd.gameObject).onClick = _onClick_WareHouseStoreCopperAdd_Btn;
        UIEventListener.Get(m_btn_WareHouseStoreCopperSub.gameObject).onClick = _onClick_WareHouseStoreCopperSub_Btn;
        UIEventListener.Get(m_btn_WareHouseStoreCopperHandInput.gameObject).onClick = _onClick_WareHouseStoreCopperHandInput_Btn;
    }

    void _onClick_WareHouseStoreCopperBtnClose_Btn(GameObject caster)
    {
        onClick_WareHouseStoreCopperBtnClose_Btn( caster );
    }

    void _onClick_WareHouseStoreCopperBtnStore_Btn(GameObject caster)
    {
        onClick_WareHouseStoreCopperBtnStore_Btn( caster );
    }

    void _onClick_WareHouseStoreCopperBtnTakeOut_Btn(GameObject caster)
    {
        onClick_WareHouseStoreCopperBtnTakeOut_Btn( caster );
    }

    void _onClick_WareHouseStoreCopperAdd_Btn(GameObject caster)
    {
        onClick_WareHouseStoreCopperAdd_Btn( caster );
    }

    void _onClick_WareHouseStoreCopperSub_Btn(GameObject caster)
    {
        onClick_WareHouseStoreCopperSub_Btn( caster );
    }

    void _onClick_WareHouseStoreCopperHandInput_Btn(GameObject caster)
    {
        onClick_WareHouseStoreCopperHandInput_Btn( caster );
    }


}
