//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class VipTransmissionPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		dibiao,
		dixiacheng,
		Boss,
		Max,
    }

    UIButton      m_btn_close;

    UIButton      m_btn_dibiao;

    UIButton      m_btn_dixiacheng;

    UIButton      m_btn_Boss;

    Transform     m_trans_Root;

    Transform     m_trans_UIVipTransmissionGrid;


    //初始化控件变量
    protected override void InitControls()
    {
        m_btn_close = GetChildComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_dibiao = GetChildComponent<UIButton>("dibiao");
       if( null == m_btn_dibiao )
       {
            Engine.Utility.Log.Error("m_btn_dibiao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_dixiacheng = GetChildComponent<UIButton>("dixiacheng");
       if( null == m_btn_dixiacheng )
       {
            Engine.Utility.Log.Error("m_btn_dixiacheng 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Boss = GetChildComponent<UIButton>("Boss");
       if( null == m_btn_Boss )
       {
            Engine.Utility.Log.Error("m_btn_Boss 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Root = GetChildComponent<Transform>("Root");
       if( null == m_trans_Root )
       {
            Engine.Utility.Log.Error("m_trans_Root 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIVipTransmissionGrid = GetChildComponent<Transform>("UIVipTransmissionGrid");
       if( null == m_trans_UIVipTransmissionGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIVipTransmissionGrid 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_dibiao.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_dixiacheng.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_Boss.gameObject).onClick = _OnBtnsClick;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _OnBtnsClick(GameObject caster)
    {
        BtnType btntype = (BtnType)System.Enum.Parse(typeof(BtnType), caster.name);
        OnBtnsClick( btntype );
    }


}
