//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ReNamePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_Close;

    UIButton             m_btn_btn_right;

    UIButton             m_btn_btn_left;

    UILabel              m_label_DesLbl;

    UIInput              m_input_Input;

    UILabel              m_label_ItemName;

    UILabel              m_label_ItemCount;

    Transform            m_trans_ItemGridRoot;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_Close = fastComponent.FastGetComponent<UIButton>("btn_Close");
       if( null == m_btn_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_right = fastComponent.FastGetComponent<UIButton>("btn_right");
       if( null == m_btn_btn_right )
       {
            Engine.Utility.Log.Error("m_btn_btn_right 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_left = fastComponent.FastGetComponent<UIButton>("btn_left");
       if( null == m_btn_btn_left )
       {
            Engine.Utility.Log.Error("m_btn_btn_left 为空，请检查prefab是否缺乏组件");
       }
        m_label_DesLbl = fastComponent.FastGetComponent<UILabel>("DesLbl");
       if( null == m_label_DesLbl )
       {
            Engine.Utility.Log.Error("m_label_DesLbl 为空，请检查prefab是否缺乏组件");
       }
        m_input_Input = fastComponent.FastGetComponent<UIInput>("Input");
       if( null == m_input_Input )
       {
            Engine.Utility.Log.Error("m_input_Input 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemName = fastComponent.FastGetComponent<UILabel>("ItemName");
       if( null == m_label_ItemName )
       {
            Engine.Utility.Log.Error("m_label_ItemName 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemCount = fastComponent.FastGetComponent<UILabel>("ItemCount");
       if( null == m_label_ItemCount )
       {
            Engine.Utility.Log.Error("m_label_ItemCount 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ItemGridRoot = fastComponent.FastGetComponent<Transform>("ItemGridRoot");
       if( null == m_trans_ItemGridRoot )
       {
            Engine.Utility.Log.Error("m_trans_ItemGridRoot 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_Close.gameObject).onClick = _onClick_Btn_Close_Btn;
        UIEventListener.Get(m_btn_btn_right.gameObject).onClick = _onClick_Btn_right_Btn;
        UIEventListener.Get(m_btn_btn_left.gameObject).onClick = _onClick_Btn_left_Btn;
    }

    void _onClick_Btn_Close_Btn(GameObject caster)
    {
        onClick_Btn_Close_Btn( caster );
    }

    void _onClick_Btn_right_Btn(GameObject caster)
    {
        onClick_Btn_right_Btn( caster );
    }

    void _onClick_Btn_left_Btn(GameObject caster)
    {
        onClick_Btn_left_Btn( caster );
    }


}
