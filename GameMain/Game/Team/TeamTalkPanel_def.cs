//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TeamTalkPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_close;

    UILabel              m_label_TitleLabel;

    UIInput              m_input_input;

    UILabel              m_label_wordnumber;

    Transform            m_trans_selectBtns;

    UIButton             m_btn_btn_send;

    UIButton             m_btn_btn_emoji;

    UIButton             m_btn_btn_joke;


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
        m_label_TitleLabel = fastComponent.FastGetComponent<UILabel>("TitleLabel");
       if( null == m_label_TitleLabel )
       {
            Engine.Utility.Log.Error("m_label_TitleLabel 为空，请检查prefab是否缺乏组件");
       }
        m_input_input = fastComponent.FastGetComponent<UIInput>("input");
       if( null == m_input_input )
       {
            Engine.Utility.Log.Error("m_input_input 为空，请检查prefab是否缺乏组件");
       }
        m_label_wordnumber = fastComponent.FastGetComponent<UILabel>("wordnumber");
       if( null == m_label_wordnumber )
       {
            Engine.Utility.Log.Error("m_label_wordnumber 为空，请检查prefab是否缺乏组件");
       }
        m_trans_selectBtns = fastComponent.FastGetComponent<Transform>("selectBtns");
       if( null == m_trans_selectBtns )
       {
            Engine.Utility.Log.Error("m_trans_selectBtns 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_send = fastComponent.FastGetComponent<UIButton>("btn_send");
       if( null == m_btn_btn_send )
       {
            Engine.Utility.Log.Error("m_btn_btn_send 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_emoji = fastComponent.FastGetComponent<UIButton>("btn_emoji");
       if( null == m_btn_btn_emoji )
       {
            Engine.Utility.Log.Error("m_btn_btn_emoji 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_joke = fastComponent.FastGetComponent<UIButton>("btn_joke");
       if( null == m_btn_btn_joke )
       {
            Engine.Utility.Log.Error("m_btn_btn_joke 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_send.gameObject).onClick = _onClick_Btn_send_Btn;
        UIEventListener.Get(m_btn_btn_emoji.gameObject).onClick = _onClick_Btn_emoji_Btn;
        UIEventListener.Get(m_btn_btn_joke.gameObject).onClick = _onClick_Btn_joke_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }

    void _onClick_Btn_send_Btn(GameObject caster)
    {
        onClick_Btn_send_Btn( caster );
    }

    void _onClick_Btn_emoji_Btn(GameObject caster)
    {
        onClick_Btn_emoji_Btn( caster );
    }

    void _onClick_Btn_joke_Btn(GameObject caster)
    {
        onClick_Btn_joke_Btn( caster );
    }


}
