//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GodWeapenPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    Transform            m_trans_labelRoot;

    UITexture            m__backGround;

    UIButton             m_btn_preview_btn;

    Transform            m_trans_rewardRoot;

    UIButton             m_btn_FinalRewardBtn;

    UITexture            m__Model;

    Transform            m_trans_UIItemRewardGrid;


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
        m_trans_labelRoot = fastComponent.FastGetComponent<Transform>("labelRoot");
       if( null == m_trans_labelRoot )
       {
            Engine.Utility.Log.Error("m_trans_labelRoot 为空，请检查prefab是否缺乏组件");
       }
        m__backGround = fastComponent.FastGetComponent<UITexture>("backGround");
       if( null == m__backGround )
       {
            Engine.Utility.Log.Error("m__backGround 为空，请检查prefab是否缺乏组件");
       }
        m_btn_preview_btn = fastComponent.FastGetComponent<UIButton>("preview_btn");
       if( null == m_btn_preview_btn )
       {
            Engine.Utility.Log.Error("m_btn_preview_btn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_rewardRoot = fastComponent.FastGetComponent<Transform>("rewardRoot");
       if( null == m_trans_rewardRoot )
       {
            Engine.Utility.Log.Error("m_trans_rewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_FinalRewardBtn = fastComponent.FastGetComponent<UIButton>("FinalRewardBtn");
       if( null == m_btn_FinalRewardBtn )
       {
            Engine.Utility.Log.Error("m_btn_FinalRewardBtn 为空，请检查prefab是否缺乏组件");
       }
        m__Model = fastComponent.FastGetComponent<UITexture>("Model");
       if( null == m__Model )
       {
            Engine.Utility.Log.Error("m__Model 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_preview_btn.gameObject).onClick = _onClick_Preview_btn_Btn;
        UIEventListener.Get(m_btn_FinalRewardBtn.gameObject).onClick = _onClick_FinalRewardBtn_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Preview_btn_Btn(GameObject caster)
    {
        onClick_Preview_btn_Btn( caster );
    }

    void _onClick_FinalRewardBtn_Btn(GameObject caster)
    {
        onClick_FinalRewardBtn_Btn( caster );
    }


}
