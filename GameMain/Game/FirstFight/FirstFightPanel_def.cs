//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class FirstFightPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIWidget             m_widget_leftCollider;

    UIWidget             m_widget_BottomCollider;

    UIWidget             m_widget_MainCollider;

    UIWidget             m_widget_joystick;

    UISprite             m_sprite_joystickBg;

    UISprite             m_sprite_joystickThumb;

    Transform            m_trans_joystickPos;

    UIWidget             m_widget_SkillBtns;

    UIButton             m_btn_TAB;

    Transform            m_trans_Btn_shortcut;

    Transform            m_trans_ShortcutItemGridCache;

    UIGrid               m_grid_FixedItemGridsRoot;

    Transform            m_trans_ShortcutMovablePanel;

    Transform            m_trans_shortcutContent;

    UIButton             m_btn_BtnArrow;

    UIWidget             m_widget_shortcutRect;

    UIGrid               m_grid_MovableItemGridsRoot;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_widget_leftCollider = fastComponent.FastGetComponent<UIWidget>("leftCollider");
       if( null == m_widget_leftCollider )
       {
            Engine.Utility.Log.Error("m_widget_leftCollider 为空，请检查prefab是否缺乏组件");
       }
        m_widget_BottomCollider = fastComponent.FastGetComponent<UIWidget>("BottomCollider");
       if( null == m_widget_BottomCollider )
       {
            Engine.Utility.Log.Error("m_widget_BottomCollider 为空，请检查prefab是否缺乏组件");
       }
        m_widget_MainCollider = fastComponent.FastGetComponent<UIWidget>("MainCollider");
       if( null == m_widget_MainCollider )
       {
            Engine.Utility.Log.Error("m_widget_MainCollider 为空，请检查prefab是否缺乏组件");
       }
        m_widget_joystick = fastComponent.FastGetComponent<UIWidget>("joystick");
       if( null == m_widget_joystick )
       {
            Engine.Utility.Log.Error("m_widget_joystick 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_joystickBg = fastComponent.FastGetComponent<UISprite>("joystickBg");
       if( null == m_sprite_joystickBg )
       {
            Engine.Utility.Log.Error("m_sprite_joystickBg 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_joystickThumb = fastComponent.FastGetComponent<UISprite>("joystickThumb");
       if( null == m_sprite_joystickThumb )
       {
            Engine.Utility.Log.Error("m_sprite_joystickThumb 为空，请检查prefab是否缺乏组件");
       }
        m_trans_joystickPos = fastComponent.FastGetComponent<Transform>("joystickPos");
       if( null == m_trans_joystickPos )
       {
            Engine.Utility.Log.Error("m_trans_joystickPos 为空，请检查prefab是否缺乏组件");
       }
        m_widget_SkillBtns = fastComponent.FastGetComponent<UIWidget>("SkillBtns");
       if( null == m_widget_SkillBtns )
       {
            Engine.Utility.Log.Error("m_widget_SkillBtns 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TAB = fastComponent.FastGetComponent<UIButton>("TAB");
       if( null == m_btn_TAB )
       {
            Engine.Utility.Log.Error("m_btn_TAB 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Btn_shortcut = fastComponent.FastGetComponent<Transform>("Btn_shortcut");
       if( null == m_trans_Btn_shortcut )
       {
            Engine.Utility.Log.Error("m_trans_Btn_shortcut 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ShortcutItemGridCache = fastComponent.FastGetComponent<Transform>("ShortcutItemGridCache");
       if( null == m_trans_ShortcutItemGridCache )
       {
            Engine.Utility.Log.Error("m_trans_ShortcutItemGridCache 为空，请检查prefab是否缺乏组件");
       }
        m_grid_FixedItemGridsRoot = fastComponent.FastGetComponent<UIGrid>("FixedItemGridsRoot");
       if( null == m_grid_FixedItemGridsRoot )
       {
            Engine.Utility.Log.Error("m_grid_FixedItemGridsRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ShortcutMovablePanel = fastComponent.FastGetComponent<Transform>("ShortcutMovablePanel");
       if( null == m_trans_ShortcutMovablePanel )
       {
            Engine.Utility.Log.Error("m_trans_ShortcutMovablePanel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_shortcutContent = fastComponent.FastGetComponent<Transform>("shortcutContent");
       if( null == m_trans_shortcutContent )
       {
            Engine.Utility.Log.Error("m_trans_shortcutContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnArrow = fastComponent.FastGetComponent<UIButton>("BtnArrow");
       if( null == m_btn_BtnArrow )
       {
            Engine.Utility.Log.Error("m_btn_BtnArrow 为空，请检查prefab是否缺乏组件");
       }
        m_widget_shortcutRect = fastComponent.FastGetComponent<UIWidget>("shortcutRect");
       if( null == m_widget_shortcutRect )
       {
            Engine.Utility.Log.Error("m_widget_shortcutRect 为空，请检查prefab是否缺乏组件");
       }
        m_grid_MovableItemGridsRoot = fastComponent.FastGetComponent<UIGrid>("MovableItemGridsRoot");
       if( null == m_grid_MovableItemGridsRoot )
       {
            Engine.Utility.Log.Error("m_grid_MovableItemGridsRoot 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_TAB.gameObject).onClick = _onClick_TAB_Btn;
        UIEventListener.Get(m_btn_BtnArrow.gameObject).onClick = _onClick_BtnArrow_Btn;
    }

    void _onClick_TAB_Btn(GameObject caster)
    {
        onClick_TAB_Btn( caster );
    }

    void _onClick_BtnArrow_Btn(GameObject caster)
    {
        onClick_BtnArrow_Btn( caster );
    }


}
