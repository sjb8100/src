//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class HornPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_horn_close;

    UILabel              m_label_horn_name;

    UIInput              m_input_Input;

    UIButton             m_btn_btn_send;

    UIButton             m_btn_btn_emoji;

    UIButton             m_btn_btn_history;

    UILabel              m_label_characterNum;

    UILabel              m_label_num;

    UIButton             m_btn_addbtn;

    UILabel              m_label_goldNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_horn_close = fastComponent.FastGetComponent<UIButton>("horn_close");
       if( null == m_btn_horn_close )
       {
            Engine.Utility.Log.Error("m_btn_horn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_horn_name = fastComponent.FastGetComponent<UILabel>("horn_name");
       if( null == m_label_horn_name )
       {
            Engine.Utility.Log.Error("m_label_horn_name 为空，请检查prefab是否缺乏组件");
       }
        m_input_Input = fastComponent.FastGetComponent<UIInput>("Input");
       if( null == m_input_Input )
       {
            Engine.Utility.Log.Error("m_input_Input 为空，请检查prefab是否缺乏组件");
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
        m_btn_btn_history = fastComponent.FastGetComponent<UIButton>("btn_history");
       if( null == m_btn_btn_history )
       {
            Engine.Utility.Log.Error("m_btn_btn_history 为空，请检查prefab是否缺乏组件");
       }
        m_label_characterNum = fastComponent.FastGetComponent<UILabel>("characterNum");
       if( null == m_label_characterNum )
       {
            Engine.Utility.Log.Error("m_label_characterNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_num = fastComponent.FastGetComponent<UILabel>("num");
       if( null == m_label_num )
       {
            Engine.Utility.Log.Error("m_label_num 为空，请检查prefab是否缺乏组件");
       }
        m_btn_addbtn = fastComponent.FastGetComponent<UIButton>("addbtn");
       if( null == m_btn_addbtn )
       {
            Engine.Utility.Log.Error("m_btn_addbtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_goldNum = fastComponent.FastGetComponent<UILabel>("goldNum");
       if( null == m_label_goldNum )
       {
            Engine.Utility.Log.Error("m_label_goldNum 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_horn_close.gameObject).onClick = _onClick_Horn_close_Btn;
        UIEventListener.Get(m_btn_btn_send.gameObject).onClick = _onClick_Btn_send_Btn;
        UIEventListener.Get(m_btn_btn_emoji.gameObject).onClick = _onClick_Btn_emoji_Btn;
        UIEventListener.Get(m_btn_btn_history.gameObject).onClick = _onClick_Btn_history_Btn;
        UIEventListener.Get(m_btn_addbtn.gameObject).onClick = _onClick_Addbtn_Btn;
    }

    void _onClick_Horn_close_Btn(GameObject caster)
    {
        onClick_Horn_close_Btn( caster );
    }

    void _onClick_Btn_send_Btn(GameObject caster)
    {
        onClick_Btn_send_Btn( caster );
    }

    void _onClick_Btn_emoji_Btn(GameObject caster)
    {
        onClick_Btn_emoji_Btn( caster );
    }

    void _onClick_Btn_history_Btn(GameObject caster)
    {
        onClick_Btn_history_Btn( caster );
    }

    void _onClick_Addbtn_Btn(GameObject caster)
    {
        onClick_Addbtn_Btn( caster );
    }


}
