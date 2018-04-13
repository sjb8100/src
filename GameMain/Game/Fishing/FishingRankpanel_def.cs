//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FishingRankPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_myRanknum_label;

    UILabel              m_label_myScorenum_label;

    Transform            m_trans_fishingrankscrollview;

    UIButton             m_btn_btn_close;

    UIWidget             m_widget_UIFishingRankGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_myRanknum_label = fastComponent.FastGetComponent<UILabel>("myRanknum_label");
       if( null == m_label_myRanknum_label )
       {
            Engine.Utility.Log.Error("m_label_myRanknum_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_myScorenum_label = fastComponent.FastGetComponent<UILabel>("myScorenum_label");
       if( null == m_label_myScorenum_label )
       {
            Engine.Utility.Log.Error("m_label_myScorenum_label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_fishingrankscrollview = fastComponent.FastGetComponent<Transform>("fishingrankscrollview");
       if( null == m_trans_fishingrankscrollview )
       {
            Engine.Utility.Log.Error("m_trans_fishingrankscrollview 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_widget_UIFishingRankGrid = fastComponent.FastGetComponent<UIWidget>("UIFishingRankGrid");
       if( null == m_widget_UIFishingRankGrid )
       {
            Engine.Utility.Log.Error("m_widget_UIFishingRankGrid 为空，请检查prefab是否缺乏组件");
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
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }


}
