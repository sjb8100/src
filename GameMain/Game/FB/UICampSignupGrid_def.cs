//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class UICampSignupGrid: UIGridBase
{

    UILabel       m_label_WarSequence;

    UILabel       m_label_ApplyTime;

    UILabel       m_label_StartTime;

    UILabel       m_label_PeopleNum;

    Transform     m_trans_Btn;

    UIButton      m_btn_BtnOver;

    UIButton      m_btn_BtnStart;

    UIButton      m_btn_BtnSign;

    UIButton      m_btn_BtnNotStart;

    UIButton      m_btn_BtnCancleSign;

    UIButton      m_btn_BtnReady;

    UILabel       m_label_EndTime;


    //初始化控件变量
   protected override void OnAwake()
    {
         InitControls();
         RegisterControlEvents();
    }
    private void InitControls()
    {
        m_label_WarSequence = GetChildComponent<UILabel>("WarSequence");
       if( null == m_label_WarSequence )
       {
            Engine.Utility.Log.Error("m_label_WarSequence 为空，请检查prefab是否缺乏组件");
       }
        m_label_ApplyTime = GetChildComponent<UILabel>("ApplyTime");
       if( null == m_label_ApplyTime )
       {
            Engine.Utility.Log.Error("m_label_ApplyTime 为空，请检查prefab是否缺乏组件");
       }
        m_label_StartTime = GetChildComponent<UILabel>("StartTime");
       if( null == m_label_StartTime )
       {
            Engine.Utility.Log.Error("m_label_StartTime 为空，请检查prefab是否缺乏组件");
       }
        m_label_PeopleNum = GetChildComponent<UILabel>("PeopleNum");
       if( null == m_label_PeopleNum )
       {
            Engine.Utility.Log.Error("m_label_PeopleNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Btn = GetChildComponent<Transform>("Btn");
       if( null == m_trans_Btn )
       {
            Engine.Utility.Log.Error("m_trans_Btn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnOver = GetChildComponent<UIButton>("BtnOver");
       if( null == m_btn_BtnOver )
       {
            Engine.Utility.Log.Error("m_btn_BtnOver 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnStart = GetChildComponent<UIButton>("BtnStart");
       if( null == m_btn_BtnStart )
       {
            Engine.Utility.Log.Error("m_btn_BtnStart 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnSign = GetChildComponent<UIButton>("BtnSign");
       if( null == m_btn_BtnSign )
       {
            Engine.Utility.Log.Error("m_btn_BtnSign 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnNotStart = GetChildComponent<UIButton>("BtnNotStart");
       if( null == m_btn_BtnNotStart )
       {
            Engine.Utility.Log.Error("m_btn_BtnNotStart 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnCancleSign = GetChildComponent<UIButton>("BtnCancleSign");
       if( null == m_btn_BtnCancleSign )
       {
            Engine.Utility.Log.Error("m_btn_BtnCancleSign 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnReady = GetChildComponent<UIButton>("BtnReady");
       if( null == m_btn_BtnReady )
       {
            Engine.Utility.Log.Error("m_btn_BtnReady 为空，请检查prefab是否缺乏组件");
       }
        m_label_EndTime = GetChildComponent<UILabel>("EndTime");
       if( null == m_label_EndTime )
       {
            Engine.Utility.Log.Error("m_label_EndTime 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    private void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BtnOver.gameObject).onClick = _onClick_BtnOver_Btn;
        UIEventListener.Get(m_btn_BtnStart.gameObject).onClick = _onClick_BtnStart_Btn;
        UIEventListener.Get(m_btn_BtnSign.gameObject).onClick = _onClick_BtnSign_Btn;
        UIEventListener.Get(m_btn_BtnNotStart.gameObject).onClick = _onClick_BtnNotStart_Btn;
        UIEventListener.Get(m_btn_BtnCancleSign.gameObject).onClick = _onClick_BtnCancleSign_Btn;
        UIEventListener.Get(m_btn_BtnReady.gameObject).onClick = _onClick_BtnReady_Btn;
    }

    void _onClick_BtnOver_Btn(GameObject caster)
    {
        onClick_BtnOver_Btn( caster );
    }

    void _onClick_BtnStart_Btn(GameObject caster)
    {
        onClick_BtnStart_Btn( caster );
    }

    void _onClick_BtnSign_Btn(GameObject caster)
    {
        onClick_BtnSign_Btn( caster );
    }

    void _onClick_BtnNotStart_Btn(GameObject caster)
    {
        onClick_BtnNotStart_Btn( caster );
    }

    void _onClick_BtnCancleSign_Btn(GameObject caster)
    {
        onClick_BtnCancleSign_Btn( caster );
    }

    void _onClick_BtnReady_Btn(GameObject caster)
    {
        onClick_BtnReady_Btn( caster );
    }


}
