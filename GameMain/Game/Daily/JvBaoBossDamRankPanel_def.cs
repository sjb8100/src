//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class JvBaoBossDamRankPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_myRanknum_label;

    UILabel              m_label_myScorenum_label;

    UIGridCreatorBase    m_ctor_BossDamRankSV;

    UIButton             m_btn_btn_close;

    UIWidget             m_widget_UIJvBaoBossDamRankGrid;

    UIButton             m_btn_BtnConfirmQuit;

    UILabel              m_label_ConfirmQuitTxt;


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
        m_ctor_BossDamRankSV = fastComponent.FastGetComponent<UIGridCreatorBase>("BossDamRankSV");
       if( null == m_ctor_BossDamRankSV )
       {
            Engine.Utility.Log.Error("m_ctor_BossDamRankSV 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_widget_UIJvBaoBossDamRankGrid = fastComponent.FastGetComponent<UIWidget>("UIJvBaoBossDamRankGrid");
       if( null == m_widget_UIJvBaoBossDamRankGrid )
       {
            Engine.Utility.Log.Error("m_widget_UIJvBaoBossDamRankGrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnConfirmQuit = fastComponent.FastGetComponent<UIButton>("BtnConfirmQuit");
       if( null == m_btn_BtnConfirmQuit )
       {
            Engine.Utility.Log.Error("m_btn_BtnConfirmQuit 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConfirmQuitTxt = fastComponent.FastGetComponent<UILabel>("ConfirmQuitTxt");
       if( null == m_label_ConfirmQuitTxt )
       {
            Engine.Utility.Log.Error("m_label_ConfirmQuitTxt 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_BtnConfirmQuit.gameObject).onClick = _onClick_BtnConfirmQuit_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_BtnConfirmQuit_Btn(GameObject caster)
    {
        onClick_BtnConfirmQuit_Btn( caster );
    }


}
