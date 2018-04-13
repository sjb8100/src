//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FirstRechargePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_close;

    UIButton             m_btn_btn_recharge;

    UIButton             m_btn_btn_getReward;

    UISprite             m_sprite_warning;

    UIGridCreatorBase    m_ctor_itemRoot;

    Transform            m_trans_UIItemRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_recharge = fastComponent.FastGetComponent<UIButton>("btn_recharge");
       if( null == m_btn_btn_recharge )
       {
            Engine.Utility.Log.Error("m_btn_btn_recharge 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_getReward = fastComponent.FastGetComponent<UIButton>("btn_getReward");
       if( null == m_btn_btn_getReward )
       {
            Engine.Utility.Log.Error("m_btn_btn_getReward 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_warning = fastComponent.FastGetComponent<UISprite>("warning");
       if( null == m_sprite_warning )
       {
            Engine.Utility.Log.Error("m_sprite_warning 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_itemRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("itemRoot");
       if( null == m_ctor_itemRoot )
       {
            Engine.Utility.Log.Error("m_ctor_itemRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
        UIEventListener.Get(m_btn_btn_recharge.gameObject).onClick = _onClick_Btn_recharge_Btn;
        UIEventListener.Get(m_btn_btn_getReward.gameObject).onClick = _onClick_Btn_getReward_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_Btn_recharge_Btn(GameObject caster)
    {
        onClick_Btn_recharge_Btn( caster );
    }

    void _onClick_Btn_getReward_Btn(GameObject caster)
    {
        onClick_Btn_getReward_Btn( caster );
    }


}
