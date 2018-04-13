//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class BlackMarketPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//云游
		YunYou = 1,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UIButton             m_btn_RefreshBtn;

    UILabel              m_label_RefreshTimeCountDown;

    Transform            m_trans_LeftCurrencyContent;

    UISprite             m_sprite_CurrncyIcon;

    UILabel              m_label_CurrncyNum;

    BlockGridScrollView  m_bgv_BlockScorllView;

    Transform            m_trans_UIBlackMarketGrid;

    Transform            m_trans_UIBlockIndexGrid;


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
        m_btn_RefreshBtn = fastComponent.FastGetComponent<UIButton>("RefreshBtn");
       if( null == m_btn_RefreshBtn )
       {
            Engine.Utility.Log.Error("m_btn_RefreshBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_RefreshTimeCountDown = fastComponent.FastGetComponent<UILabel>("RefreshTimeCountDown");
       if( null == m_label_RefreshTimeCountDown )
       {
            Engine.Utility.Log.Error("m_label_RefreshTimeCountDown 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LeftCurrencyContent = fastComponent.FastGetComponent<Transform>("LeftCurrencyContent");
       if( null == m_trans_LeftCurrencyContent )
       {
            Engine.Utility.Log.Error("m_trans_LeftCurrencyContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_CurrncyIcon = fastComponent.FastGetComponent<UISprite>("CurrncyIcon");
       if( null == m_sprite_CurrncyIcon )
       {
            Engine.Utility.Log.Error("m_sprite_CurrncyIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_CurrncyNum = fastComponent.FastGetComponent<UILabel>("CurrncyNum");
       if( null == m_label_CurrncyNum )
       {
            Engine.Utility.Log.Error("m_label_CurrncyNum 为空，请检查prefab是否缺乏组件");
       }
        m_bgv_BlockScorllView = fastComponent.FastGetComponent<BlockGridScrollView>("BlockScorllView");
       if( null == m_bgv_BlockScorllView )
       {
            Engine.Utility.Log.Error("m_bgv_BlockScorllView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIBlackMarketGrid = fastComponent.FastGetComponent<Transform>("UIBlackMarketGrid");
       if( null == m_trans_UIBlackMarketGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIBlackMarketGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIBlockIndexGrid = fastComponent.FastGetComponent<Transform>("UIBlockIndexGrid");
       if( null == m_trans_UIBlockIndexGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIBlockIndexGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_RefreshBtn.gameObject).onClick = _onClick_RefreshBtn_Btn;
    }

    void _onClick_RefreshBtn_Btn(GameObject caster)
    {
        onClick_RefreshBtn_Btn( caster );
    }


}
