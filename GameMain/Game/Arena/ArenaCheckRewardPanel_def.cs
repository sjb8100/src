//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ArenaCheckRewardPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_Close;

    UILabel              m_label_ArenaRewardInfoLbl;

    Transform            m_trans_ArenaReward;

    UISprite             m_sprite_Bg;

    UISprite             m_sprite_wenqian;

    UILabel              m_label_wenqian_label;

    UISprite             m_sprite_jingbi;

    UILabel              m_label_jingbi_label;

    UISprite             m_sprite_gongxian;

    UILabel              m_label_gongxian_label;

    UISprite             m_sprite_Exp;

    UILabel              m_label_Exp_label;

    UISprite             m_sprite_bg_di;

    UILabel              m_label_Title_label;

    UILabel              m_label_Des_label;

    Transform            m_trans_listScrollView;

    Transform            m_trans_UIArenaRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_Close = fastComponent.FastGetComponent<UIButton>("Close");
       if( null == m_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_label_ArenaRewardInfoLbl = fastComponent.FastGetComponent<UILabel>("ArenaRewardInfoLbl");
       if( null == m_label_ArenaRewardInfoLbl )
       {
            Engine.Utility.Log.Error("m_label_ArenaRewardInfoLbl 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ArenaReward = fastComponent.FastGetComponent<Transform>("ArenaReward");
       if( null == m_trans_ArenaReward )
       {
            Engine.Utility.Log.Error("m_trans_ArenaReward 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Bg = fastComponent.FastGetComponent<UISprite>("Bg");
       if( null == m_sprite_Bg )
       {
            Engine.Utility.Log.Error("m_sprite_Bg 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_wenqian = fastComponent.FastGetComponent<UISprite>("wenqian");
       if( null == m_sprite_wenqian )
       {
            Engine.Utility.Log.Error("m_sprite_wenqian 为空，请检查prefab是否缺乏组件");
       }
        m_label_wenqian_label = fastComponent.FastGetComponent<UILabel>("wenqian_label");
       if( null == m_label_wenqian_label )
       {
            Engine.Utility.Log.Error("m_label_wenqian_label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_jingbi = fastComponent.FastGetComponent<UISprite>("jingbi");
       if( null == m_sprite_jingbi )
       {
            Engine.Utility.Log.Error("m_sprite_jingbi 为空，请检查prefab是否缺乏组件");
       }
        m_label_jingbi_label = fastComponent.FastGetComponent<UILabel>("jingbi_label");
       if( null == m_label_jingbi_label )
       {
            Engine.Utility.Log.Error("m_label_jingbi_label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_gongxian = fastComponent.FastGetComponent<UISprite>("gongxian");
       if( null == m_sprite_gongxian )
       {
            Engine.Utility.Log.Error("m_sprite_gongxian 为空，请检查prefab是否缺乏组件");
       }
        m_label_gongxian_label = fastComponent.FastGetComponent<UILabel>("gongxian_label");
       if( null == m_label_gongxian_label )
       {
            Engine.Utility.Log.Error("m_label_gongxian_label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Exp = fastComponent.FastGetComponent<UISprite>("Exp");
       if( null == m_sprite_Exp )
       {
            Engine.Utility.Log.Error("m_sprite_Exp 为空，请检查prefab是否缺乏组件");
       }
        m_label_Exp_label = fastComponent.FastGetComponent<UILabel>("Exp_label");
       if( null == m_label_Exp_label )
       {
            Engine.Utility.Log.Error("m_label_Exp_label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_bg_di = fastComponent.FastGetComponent<UISprite>("bg_di");
       if( null == m_sprite_bg_di )
       {
            Engine.Utility.Log.Error("m_sprite_bg_di 为空，请检查prefab是否缺乏组件");
       }
        m_label_Title_label = fastComponent.FastGetComponent<UILabel>("Title_label");
       if( null == m_label_Title_label )
       {
            Engine.Utility.Log.Error("m_label_Title_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_Des_label = fastComponent.FastGetComponent<UILabel>("Des_label");
       if( null == m_label_Des_label )
       {
            Engine.Utility.Log.Error("m_label_Des_label 为空，请检查prefab是否缺乏组件");
       }
        m_trans_listScrollView = fastComponent.FastGetComponent<Transform>("listScrollView");
       if( null == m_trans_listScrollView )
       {
            Engine.Utility.Log.Error("m_trans_listScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIArenaRewardGrid = fastComponent.FastGetComponent<Transform>("UIArenaRewardGrid");
       if( null == m_trans_UIArenaRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIArenaRewardGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
