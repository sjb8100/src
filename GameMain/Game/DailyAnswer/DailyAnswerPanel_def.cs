//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class DailyAnswerPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UITexture            m__BoxClose;

    UIButton             m_btn_BoxOpen;

    UILabel              m_label_title_label;

    UILabel              m_label_bottomtitle_label;

    UITexture            m__BoxAlreadyOpen;

    UILabel              m_label_lasttime_label;

    UILabel              m_label_right_label;

    UILabel              m_label_exp_label;

    UILabel              m_label_gold_label;

    Transform            m_trans_right;

    UILabel              m_label_questNum_label;

    UILabel              m_label_question_label;

    UIGrid               m_grid_Grid;

    UILabel              m_label_tips_lable;

    Transform            m_trans_complated;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m__BoxClose = fastComponent.FastGetComponent<UITexture>("BoxClose");
       if( null == m__BoxClose )
       {
            Engine.Utility.Log.Error("m__BoxClose 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BoxOpen = fastComponent.FastGetComponent<UIButton>("BoxOpen");
       if( null == m_btn_BoxOpen )
       {
            Engine.Utility.Log.Error("m_btn_BoxOpen 为空，请检查prefab是否缺乏组件");
       }
        m_label_title_label = fastComponent.FastGetComponent<UILabel>("title_label");
       if( null == m_label_title_label )
       {
            Engine.Utility.Log.Error("m_label_title_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_bottomtitle_label = fastComponent.FastGetComponent<UILabel>("bottomtitle_label");
       if( null == m_label_bottomtitle_label )
       {
            Engine.Utility.Log.Error("m_label_bottomtitle_label 为空，请检查prefab是否缺乏组件");
       }
        m__BoxAlreadyOpen = fastComponent.FastGetComponent<UITexture>("BoxAlreadyOpen");
       if( null == m__BoxAlreadyOpen )
       {
            Engine.Utility.Log.Error("m__BoxAlreadyOpen 为空，请检查prefab是否缺乏组件");
       }
        m_label_lasttime_label = fastComponent.FastGetComponent<UILabel>("lasttime_label");
       if( null == m_label_lasttime_label )
       {
            Engine.Utility.Log.Error("m_label_lasttime_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_right_label = fastComponent.FastGetComponent<UILabel>("right_label");
       if( null == m_label_right_label )
       {
            Engine.Utility.Log.Error("m_label_right_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_exp_label = fastComponent.FastGetComponent<UILabel>("exp_label");
       if( null == m_label_exp_label )
       {
            Engine.Utility.Log.Error("m_label_exp_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_gold_label = fastComponent.FastGetComponent<UILabel>("gold_label");
       if( null == m_label_gold_label )
       {
            Engine.Utility.Log.Error("m_label_gold_label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_right = fastComponent.FastGetComponent<Transform>("right");
       if( null == m_trans_right )
       {
            Engine.Utility.Log.Error("m_trans_right 为空，请检查prefab是否缺乏组件");
       }
        m_label_questNum_label = fastComponent.FastGetComponent<UILabel>("questNum_label");
       if( null == m_label_questNum_label )
       {
            Engine.Utility.Log.Error("m_label_questNum_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_question_label = fastComponent.FastGetComponent<UILabel>("question_label");
       if( null == m_label_question_label )
       {
            Engine.Utility.Log.Error("m_label_question_label 为空，请检查prefab是否缺乏组件");
       }
        m_grid_Grid = fastComponent.FastGetComponent<UIGrid>("Grid");
       if( null == m_grid_Grid )
       {
            Engine.Utility.Log.Error("m_grid_Grid 为空，请检查prefab是否缺乏组件");
       }
        m_label_tips_lable = fastComponent.FastGetComponent<UILabel>("tips_lable");
       if( null == m_label_tips_lable )
       {
            Engine.Utility.Log.Error("m_label_tips_lable 为空，请检查prefab是否缺乏组件");
       }
        m_trans_complated = fastComponent.FastGetComponent<Transform>("complated");
       if( null == m_trans_complated )
       {
            Engine.Utility.Log.Error("m_trans_complated 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BoxOpen.gameObject).onClick = _onClick_BoxOpen_Btn;
    }

    void _onClick_BoxOpen_Btn(GameObject caster)
    {
        onClick_BoxOpen_Btn( caster );
    }


}
