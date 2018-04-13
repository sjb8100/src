//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class HomeControlPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_myhome;

    UIButton             m_btn_btn_leavehome;

    UIButton             m_btn_btn_gain;

    UIButton             m_btn_btn_gainall;

    UIButton             m_btn_btn_tree;

    UIButton             m_btn_btn_trade;

    UIWidget             m_widget_theirhome;

    UIButton             m_btn_btn_returnhome;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_myhome = fastComponent.FastGetComponent<UIWidget>("myhome");
       if( null == m_widget_myhome )
       {
            Engine.Utility.Log.Error("m_widget_myhome 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_leavehome = fastComponent.FastGetComponent<UIButton>("btn_leavehome");
       if( null == m_btn_btn_leavehome )
       {
            Engine.Utility.Log.Error("m_btn_btn_leavehome 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_gain = fastComponent.FastGetComponent<UIButton>("btn_gain");
       if( null == m_btn_btn_gain )
       {
            Engine.Utility.Log.Error("m_btn_btn_gain 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_gainall = fastComponent.FastGetComponent<UIButton>("btn_gainall");
       if( null == m_btn_btn_gainall )
       {
            Engine.Utility.Log.Error("m_btn_btn_gainall 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_tree = fastComponent.FastGetComponent<UIButton>("btn_tree");
       if( null == m_btn_btn_tree )
       {
            Engine.Utility.Log.Error("m_btn_btn_tree 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_trade = fastComponent.FastGetComponent<UIButton>("btn_trade");
       if( null == m_btn_btn_trade )
       {
            Engine.Utility.Log.Error("m_btn_btn_trade 为空，请检查prefab是否缺乏组件");
       }
        m_widget_theirhome = fastComponent.FastGetComponent<UIWidget>("theirhome");
       if( null == m_widget_theirhome )
       {
            Engine.Utility.Log.Error("m_widget_theirhome 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_returnhome = fastComponent.FastGetComponent<UIButton>("btn_returnhome");
       if( null == m_btn_btn_returnhome )
       {
            Engine.Utility.Log.Error("m_btn_btn_returnhome 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_leavehome.gameObject).onClick = _onClick_Btn_leavehome_Btn;
        UIEventListener.Get(m_btn_btn_gain.gameObject).onClick = _onClick_Btn_gain_Btn;
        UIEventListener.Get(m_btn_btn_gainall.gameObject).onClick = _onClick_Btn_gainall_Btn;
        UIEventListener.Get(m_btn_btn_tree.gameObject).onClick = _onClick_Btn_tree_Btn;
        UIEventListener.Get(m_btn_btn_trade.gameObject).onClick = _onClick_Btn_trade_Btn;
        UIEventListener.Get(m_btn_btn_returnhome.gameObject).onClick = _onClick_Btn_returnhome_Btn;
    }

    void _onClick_Btn_leavehome_Btn(GameObject caster)
    {
        onClick_Btn_leavehome_Btn( caster );
    }

    void _onClick_Btn_gain_Btn(GameObject caster)
    {
        onClick_Btn_gain_Btn( caster );
    }

    void _onClick_Btn_gainall_Btn(GameObject caster)
    {
        onClick_Btn_gainall_Btn( caster );
    }

    void _onClick_Btn_tree_Btn(GameObject caster)
    {
        onClick_Btn_tree_Btn( caster );
    }

    void _onClick_Btn_trade_Btn(GameObject caster)
    {
        onClick_Btn_trade_Btn( caster );
    }

    void _onClick_Btn_returnhome_Btn(GameObject caster)
    {
        onClick_Btn_returnhome_Btn( caster );
    }


}
