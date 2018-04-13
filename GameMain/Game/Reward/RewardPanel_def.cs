//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RewardPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//发布
		FaBu = 1,
		//接取
		JieQu = 2,
		Max,
    }

   FastComponent         fastComponent;

    UILabel              m_label_lableNum;

    UIButton             m_btn_checkBtn;

    UIToggle             m_toggle_MoneyToggle;

    Transform            m_trans_PanelInfo;

    UIWidget             m_widget_rewardInfo;

    UILabel              m_label_release_Label;

    UILabel              m_label_take_Label;

    Transform            m_trans_UIRewardTaskGrid;

    UILabel              m_label_acceptExp;

    Transform            m_trans_gridroot;

    UIButton             m_btn_RewardUItips_1;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_lableNum = fastComponent.FastGetComponent<UILabel>("lableNum");
       if( null == m_label_lableNum )
       {
            Engine.Utility.Log.Error("m_label_lableNum 为空，请检查prefab是否缺乏组件");
       }
        m_btn_checkBtn = fastComponent.FastGetComponent<UIButton>("checkBtn");
       if( null == m_btn_checkBtn )
       {
            Engine.Utility.Log.Error("m_btn_checkBtn 为空，请检查prefab是否缺乏组件");
       }
        m_toggle_MoneyToggle = fastComponent.FastGetComponent<UIToggle>("MoneyToggle");
       if( null == m_toggle_MoneyToggle )
       {
            Engine.Utility.Log.Error("m_toggle_MoneyToggle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PanelInfo = fastComponent.FastGetComponent<Transform>("PanelInfo");
       if( null == m_trans_PanelInfo )
       {
            Engine.Utility.Log.Error("m_trans_PanelInfo 为空，请检查prefab是否缺乏组件");
       }
        m_widget_rewardInfo = fastComponent.FastGetComponent<UIWidget>("rewardInfo");
       if( null == m_widget_rewardInfo )
       {
            Engine.Utility.Log.Error("m_widget_rewardInfo 为空，请检查prefab是否缺乏组件");
       }
        m_label_release_Label = fastComponent.FastGetComponent<UILabel>("release_Label");
       if( null == m_label_release_Label )
       {
            Engine.Utility.Log.Error("m_label_release_Label 为空，请检查prefab是否缺乏组件");
       }
        m_label_take_Label = fastComponent.FastGetComponent<UILabel>("take_Label");
       if( null == m_label_take_Label )
       {
            Engine.Utility.Log.Error("m_label_take_Label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIRewardTaskGrid = fastComponent.FastGetComponent<Transform>("UIRewardTaskGrid");
       if( null == m_trans_UIRewardTaskGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIRewardTaskGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_acceptExp = fastComponent.FastGetComponent<UILabel>("acceptExp");
       if( null == m_label_acceptExp )
       {
            Engine.Utility.Log.Error("m_label_acceptExp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_gridroot = fastComponent.FastGetComponent<Transform>("gridroot");
       if( null == m_trans_gridroot )
       {
            Engine.Utility.Log.Error("m_trans_gridroot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_RewardUItips_1 = fastComponent.FastGetComponent<UIButton>("RewardUItips_1");
       if( null == m_btn_RewardUItips_1 )
       {
            Engine.Utility.Log.Error("m_btn_RewardUItips_1 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_checkBtn.gameObject).onClick = _onClick_CheckBtn_Btn;
        UIEventListener.Get(m_btn_RewardUItips_1.gameObject).onClick = _onClick_RewardUItips_1_Btn;
    }

    void _onClick_CheckBtn_Btn(GameObject caster)
    {
        onClick_CheckBtn_Btn( caster );
    }

    void _onClick_RewardUItips_1_Btn(GameObject caster)
    {
         DataManager.Manager<UIPanelManager>().ShowPanelTips(caster.name, this.PanelInfo);
    }


}
