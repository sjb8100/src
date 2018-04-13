//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FishingExchangePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UIGridCreatorBase    m_ctor_ExchangeScrollView;

    UIGridCreatorBase    m_ctor_CategoryTagContent;

    Transform            m_trans_RightContent;

    Transform            m_trans_ExchangeContent;

    Transform            m_trans_ItemInfo;

    UILabel              m_label_ItemName;

    UILabel              m_label_ItemDes;

    UILabel              m_label_ItemUseLv;

    Transform            m_trans_ItemBaseGridRoot;

    Transform            m_trans_CostInfo;

    UITexture            m__CostIcon;

    UILabel              m_label_CostNum;

    Transform            m_trans_OwnInfo;

    UITexture            m__OwnIcon;

    UILabel              m_label_OwnNum;

    UIButton             m_btn_ItemGetBtn;

    UIButton             m_btn_ExchangeBtn;

    UIButton             m_btn_BtnAdd;

    UIButton             m_btn_BtnRemove;

    UILabel              m_label_ExchangeNum;

    UIButton             m_btn_BtnMax;

    UIButton             m_btn_HandInputBtn;

    UIGridCreatorBase    m_ctor_RightTabRoot;


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
        m_ctor_ExchangeScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ExchangeScrollView");
       if( null == m_ctor_ExchangeScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ExchangeScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_CategoryTagContent = fastComponent.FastGetComponent<UIGridCreatorBase>("CategoryTagContent");
       if( null == m_ctor_CategoryTagContent )
       {
            Engine.Utility.Log.Error("m_ctor_CategoryTagContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RightContent = fastComponent.FastGetComponent<Transform>("RightContent");
       if( null == m_trans_RightContent )
       {
            Engine.Utility.Log.Error("m_trans_RightContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ExchangeContent = fastComponent.FastGetComponent<Transform>("ExchangeContent");
       if( null == m_trans_ExchangeContent )
       {
            Engine.Utility.Log.Error("m_trans_ExchangeContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemInfo = fastComponent.FastGetComponent<Transform>("ItemInfo");
       if( null == m_trans_ItemInfo )
       {
            Engine.Utility.Log.Error("m_trans_ItemInfo 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemName = fastComponent.FastGetComponent<UILabel>("ItemName");
       if( null == m_label_ItemName )
       {
            Engine.Utility.Log.Error("m_label_ItemName 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemDes = fastComponent.FastGetComponent<UILabel>("ItemDes");
       if( null == m_label_ItemDes )
       {
            Engine.Utility.Log.Error("m_label_ItemDes 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemUseLv = fastComponent.FastGetComponent<UILabel>("ItemUseLv");
       if( null == m_label_ItemUseLv )
       {
            Engine.Utility.Log.Error("m_label_ItemUseLv 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemBaseGridRoot = fastComponent.FastGetComponent<Transform>("ItemBaseGridRoot");
       if( null == m_trans_ItemBaseGridRoot )
       {
            Engine.Utility.Log.Error("m_trans_ItemBaseGridRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CostInfo = fastComponent.FastGetComponent<Transform>("CostInfo");
       if( null == m_trans_CostInfo )
       {
            Engine.Utility.Log.Error("m_trans_CostInfo 为空，请检查prefab是否缺乏组件");
       }
        m__CostIcon = fastComponent.FastGetComponent<UITexture>("CostIcon");
       if( null == m__CostIcon )
       {
            Engine.Utility.Log.Error("m__CostIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_CostNum = fastComponent.FastGetComponent<UILabel>("CostNum");
       if( null == m_label_CostNum )
       {
            Engine.Utility.Log.Error("m_label_CostNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_OwnInfo = fastComponent.FastGetComponent<Transform>("OwnInfo");
       if( null == m_trans_OwnInfo )
       {
            Engine.Utility.Log.Error("m_trans_OwnInfo 为空，请检查prefab是否缺乏组件");
       }
        m__OwnIcon = fastComponent.FastGetComponent<UITexture>("OwnIcon");
       if( null == m__OwnIcon )
       {
            Engine.Utility.Log.Error("m__OwnIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_OwnNum = fastComponent.FastGetComponent<UILabel>("OwnNum");
       if( null == m_label_OwnNum )
       {
            Engine.Utility.Log.Error("m_label_OwnNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ItemGetBtn = fastComponent.FastGetComponent<UIButton>("ItemGetBtn");
       if( null == m_btn_ItemGetBtn )
       {
            Engine.Utility.Log.Error("m_btn_ItemGetBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ExchangeBtn = fastComponent.FastGetComponent<UIButton>("ExchangeBtn");
       if( null == m_btn_ExchangeBtn )
       {
            Engine.Utility.Log.Error("m_btn_ExchangeBtn 为空，请检查prefab是否缺乏组件");
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
        m_label_ExchangeNum = fastComponent.FastGetComponent<UILabel>("ExchangeNum");
       if( null == m_label_ExchangeNum )
       {
            Engine.Utility.Log.Error("m_label_ExchangeNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnMax = fastComponent.FastGetComponent<UIButton>("BtnMax");
       if( null == m_btn_BtnMax )
       {
            Engine.Utility.Log.Error("m_btn_BtnMax 为空，请检查prefab是否缺乏组件");
       }
        m_btn_HandInputBtn = fastComponent.FastGetComponent<UIButton>("HandInputBtn");
       if( null == m_btn_HandInputBtn )
       {
            Engine.Utility.Log.Error("m_btn_HandInputBtn 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_RightTabRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("RightTabRoot");
       if( null == m_ctor_RightTabRoot )
       {
            Engine.Utility.Log.Error("m_ctor_RightTabRoot 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_ItemGetBtn.gameObject).onClick = _onClick_ItemGetBtn_Btn;
        UIEventListener.Get(m_btn_ExchangeBtn.gameObject).onClick = _onClick_ExchangeBtn_Btn;
        UIEventListener.Get(m_btn_BtnAdd.gameObject).onClick = _onClick_BtnAdd_Btn;
        UIEventListener.Get(m_btn_BtnRemove.gameObject).onClick = _onClick_BtnRemove_Btn;
        UIEventListener.Get(m_btn_BtnMax.gameObject).onClick = _onClick_BtnMax_Btn;
        UIEventListener.Get(m_btn_HandInputBtn.gameObject).onClick = _onClick_HandInputBtn_Btn;
    }

    void _onClick_ItemGetBtn_Btn(GameObject caster)
    {
        onClick_ItemGetBtn_Btn( caster );
    }

    void _onClick_ExchangeBtn_Btn(GameObject caster)
    {
        onClick_ExchangeBtn_Btn( caster );
    }

    void _onClick_BtnAdd_Btn(GameObject caster)
    {
        onClick_BtnAdd_Btn( caster );
    }

    void _onClick_BtnRemove_Btn(GameObject caster)
    {
        onClick_BtnRemove_Btn( caster );
    }

    void _onClick_BtnMax_Btn(GameObject caster)
    {
        onClick_BtnMax_Btn( caster );
    }

    void _onClick_HandInputBtn_Btn(GameObject caster)
    {
        onClick_HandInputBtn_Btn( caster );
    }


}
