//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class DownloadPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_citywar;

    UIButton             m_btn_Colsebtn;

    UILabel              m_label_label3;

    UIButton             m_btn_btn_takeReward;

    UISlider             m_slider_percentBar;

    UILabel              m_label_Percent;

    UIButton             m_btn_btn_Pause;

    UIButton             m_btn_btn_Continue;

    UILabel              m_label_chuanzhancd;

    UIGridCreatorBase    m_ctor_itemRoot;

    Transform            m_trans_UIItemRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_citywar = fastComponent.FastGetComponent<Transform>("citywar");
       if( null == m_trans_citywar )
       {
            Engine.Utility.Log.Error("m_trans_citywar 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Colsebtn = fastComponent.FastGetComponent<UIButton>("Colsebtn");
       if( null == m_btn_Colsebtn )
       {
            Engine.Utility.Log.Error("m_btn_Colsebtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_label3 = fastComponent.FastGetComponent<UILabel>("label3");
       if( null == m_label_label3 )
       {
            Engine.Utility.Log.Error("m_label_label3 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_takeReward = fastComponent.FastGetComponent<UIButton>("btn_takeReward");
       if( null == m_btn_btn_takeReward )
       {
            Engine.Utility.Log.Error("m_btn_btn_takeReward 为空，请检查prefab是否缺乏组件");
       }
        m_slider_percentBar = fastComponent.FastGetComponent<UISlider>("percentBar");
       if( null == m_slider_percentBar )
       {
            Engine.Utility.Log.Error("m_slider_percentBar 为空，请检查prefab是否缺乏组件");
       }
        m_label_Percent = fastComponent.FastGetComponent<UILabel>("Percent");
       if( null == m_label_Percent )
       {
            Engine.Utility.Log.Error("m_label_Percent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Pause = fastComponent.FastGetComponent<UIButton>("btn_Pause");
       if( null == m_btn_btn_Pause )
       {
            Engine.Utility.Log.Error("m_btn_btn_Pause 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Continue = fastComponent.FastGetComponent<UIButton>("btn_Continue");
       if( null == m_btn_btn_Continue )
       {
            Engine.Utility.Log.Error("m_btn_btn_Continue 为空，请检查prefab是否缺乏组件");
       }
        m_label_chuanzhancd = fastComponent.FastGetComponent<UILabel>("chuanzhancd");
       if( null == m_label_chuanzhancd )
       {
            Engine.Utility.Log.Error("m_label_chuanzhancd 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_Colsebtn.gameObject).onClick = _onClick_Colsebtn_Btn;
        UIEventListener.Get(m_btn_btn_takeReward.gameObject).onClick = _onClick_Btn_takeReward_Btn;
        UIEventListener.Get(m_btn_btn_Pause.gameObject).onClick = _onClick_Btn_Pause_Btn;
        UIEventListener.Get(m_btn_btn_Continue.gameObject).onClick = _onClick_Btn_Continue_Btn;
    }

    void _onClick_Colsebtn_Btn(GameObject caster)
    {
        onClick_Colsebtn_Btn( caster );
    }

    void _onClick_Btn_takeReward_Btn(GameObject caster)
    {
        onClick_Btn_takeReward_Btn( caster );
    }

    void _onClick_Btn_Pause_Btn(GameObject caster)
    {
        onClick_Btn_Pause_Btn( caster );
    }

    void _onClick_Btn_Continue_Btn(GameObject caster)
    {
        onClick_Btn_Continue_Btn( caster );
    }


}
