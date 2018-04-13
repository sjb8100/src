//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ChannelSettingPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		world,
		clan,
		team,
		near,
		demon,
		sys,
		world_auto,
		clan_auto,
		team_auto,
		near_auto,
		demon_auto,
		liuliang,
		Max,
    }

   FastComponent         fastComponent;

    UILabel              m_label_name;

    UIButton             m_btn_close;

    UIButton             m_btn_world;

    UIButton             m_btn_clan;

    UIButton             m_btn_team;

    UIButton             m_btn_near;

    UIButton             m_btn_demon;

    UIButton             m_btn_sys;

    UIButton             m_btn_world_auto;

    UIButton             m_btn_clan_auto;

    UIButton             m_btn_team_auto;

    UIButton             m_btn_near_auto;

    UIButton             m_btn_demon_auto;

    UIButton             m_btn_liuliang;

    UIButton             m_btn_btn_cancel;

    UIButton             m_btn_btn_sure;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_world = fastComponent.FastGetComponent<UIButton>("world");
       if( null == m_btn_world )
       {
            Engine.Utility.Log.Error("m_btn_world 为空，请检查prefab是否缺乏组件");
       }
        m_btn_clan = fastComponent.FastGetComponent<UIButton>("clan");
       if( null == m_btn_clan )
       {
            Engine.Utility.Log.Error("m_btn_clan 为空，请检查prefab是否缺乏组件");
       }
        m_btn_team = fastComponent.FastGetComponent<UIButton>("team");
       if( null == m_btn_team )
       {
            Engine.Utility.Log.Error("m_btn_team 为空，请检查prefab是否缺乏组件");
       }
        m_btn_near = fastComponent.FastGetComponent<UIButton>("near");
       if( null == m_btn_near )
       {
            Engine.Utility.Log.Error("m_btn_near 为空，请检查prefab是否缺乏组件");
       }
        m_btn_demon = fastComponent.FastGetComponent<UIButton>("demon");
       if( null == m_btn_demon )
       {
            Engine.Utility.Log.Error("m_btn_demon 为空，请检查prefab是否缺乏组件");
       }
        m_btn_sys = fastComponent.FastGetComponent<UIButton>("sys");
       if( null == m_btn_sys )
       {
            Engine.Utility.Log.Error("m_btn_sys 为空，请检查prefab是否缺乏组件");
       }
        m_btn_world_auto = fastComponent.FastGetComponent<UIButton>("world_auto");
       if( null == m_btn_world_auto )
       {
            Engine.Utility.Log.Error("m_btn_world_auto 为空，请检查prefab是否缺乏组件");
       }
        m_btn_clan_auto = fastComponent.FastGetComponent<UIButton>("clan_auto");
       if( null == m_btn_clan_auto )
       {
            Engine.Utility.Log.Error("m_btn_clan_auto 为空，请检查prefab是否缺乏组件");
       }
        m_btn_team_auto = fastComponent.FastGetComponent<UIButton>("team_auto");
       if( null == m_btn_team_auto )
       {
            Engine.Utility.Log.Error("m_btn_team_auto 为空，请检查prefab是否缺乏组件");
       }
        m_btn_near_auto = fastComponent.FastGetComponent<UIButton>("near_auto");
       if( null == m_btn_near_auto )
       {
            Engine.Utility.Log.Error("m_btn_near_auto 为空，请检查prefab是否缺乏组件");
       }
        m_btn_demon_auto = fastComponent.FastGetComponent<UIButton>("demon_auto");
       if( null == m_btn_demon_auto )
       {
            Engine.Utility.Log.Error("m_btn_demon_auto 为空，请检查prefab是否缺乏组件");
       }
        m_btn_liuliang = fastComponent.FastGetComponent<UIButton>("liuliang");
       if( null == m_btn_liuliang )
       {
            Engine.Utility.Log.Error("m_btn_liuliang 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_cancel = fastComponent.FastGetComponent<UIButton>("btn_cancel");
       if( null == m_btn_btn_cancel )
       {
            Engine.Utility.Log.Error("m_btn_btn_cancel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_sure = fastComponent.FastGetComponent<UIButton>("btn_sure");
       if( null == m_btn_btn_sure )
       {
            Engine.Utility.Log.Error("m_btn_btn_sure 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_world.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_clan.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_team.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_near.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_demon.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_sys.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_world_auto.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_clan_auto.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_team_auto.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_near_auto.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_demon_auto.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_liuliang.gameObject).onClick = _OnBtnsClick;
        UIEventListener.Get(m_btn_btn_cancel.gameObject).onClick = _onClick_Btn_cancel_Btn;
        UIEventListener.Get(m_btn_btn_sure.gameObject).onClick = _onClick_Btn_sure_Btn;
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

    void _onClick_Btn_cancel_Btn(GameObject caster)
    {
        onClick_Btn_cancel_Btn( caster );
    }

    void _onClick_Btn_sure_Btn(GameObject caster)
    {
        onClick_Btn_sure_Btn( caster );
    }


}
