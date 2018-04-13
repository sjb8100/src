//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FashionTips: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_tipssuitname;

    UILabel              m_label_tipsdes;

    Transform            m_trans_AttrRoot;

    Transform            m_trans_getwaycontent;

    UILabel              m_label_getwaydes;

    UIButton             m_btn_Equip;

    UIButton             m_btn_AddTime;

    UIButton             m_btn_buy;

    UIButton             m_btn_unload;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_tipssuitname = fastComponent.FastGetComponent<UILabel>("tipssuitname");
       if( null == m_label_tipssuitname )
       {
            Engine.Utility.Log.Error("m_label_tipssuitname 为空，请检查prefab是否缺乏组件");
       }
        m_label_tipsdes = fastComponent.FastGetComponent<UILabel>("tipsdes");
       if( null == m_label_tipsdes )
       {
            Engine.Utility.Log.Error("m_label_tipsdes 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AttrRoot = fastComponent.FastGetComponent<Transform>("AttrRoot");
       if( null == m_trans_AttrRoot )
       {
            Engine.Utility.Log.Error("m_trans_AttrRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_getwaycontent = fastComponent.FastGetComponent<Transform>("getwaycontent");
       if( null == m_trans_getwaycontent )
       {
            Engine.Utility.Log.Error("m_trans_getwaycontent 为空，请检查prefab是否缺乏组件");
       }
        m_label_getwaydes = fastComponent.FastGetComponent<UILabel>("getwaydes");
       if( null == m_label_getwaydes )
       {
            Engine.Utility.Log.Error("m_label_getwaydes 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Equip = fastComponent.FastGetComponent<UIButton>("Equip");
       if( null == m_btn_Equip )
       {
            Engine.Utility.Log.Error("m_btn_Equip 为空，请检查prefab是否缺乏组件");
       }
        m_btn_AddTime = fastComponent.FastGetComponent<UIButton>("AddTime");
       if( null == m_btn_AddTime )
       {
            Engine.Utility.Log.Error("m_btn_AddTime 为空，请检查prefab是否缺乏组件");
       }
        m_btn_buy = fastComponent.FastGetComponent<UIButton>("buy");
       if( null == m_btn_buy )
       {
            Engine.Utility.Log.Error("m_btn_buy 为空，请检查prefab是否缺乏组件");
       }
        m_btn_unload = fastComponent.FastGetComponent<UIButton>("unload");
       if( null == m_btn_unload )
       {
            Engine.Utility.Log.Error("m_btn_unload 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Equip.gameObject).onClick = _onClick_Equip_Btn;
        UIEventListener.Get(m_btn_AddTime.gameObject).onClick = _onClick_AddTime_Btn;
        UIEventListener.Get(m_btn_buy.gameObject).onClick = _onClick_Buy_Btn;
        UIEventListener.Get(m_btn_unload.gameObject).onClick = _onClick_Unload_Btn;
    }

    void _onClick_Equip_Btn(GameObject caster)
    {
        onClick_Equip_Btn( caster );
    }

    void _onClick_AddTime_Btn(GameObject caster)
    {
        onClick_AddTime_Btn( caster );
    }

    void _onClick_Buy_Btn(GameObject caster)
    {
        onClick_Buy_Btn( caster );
    }

    void _onClick_Unload_Btn(GameObject caster)
    {
        onClick_Unload_Btn( caster );
    }


}
