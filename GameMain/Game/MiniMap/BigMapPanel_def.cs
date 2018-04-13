//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class BigMapPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_MapName;

    UILabel              m_label_line_label;

    UIButton             m_btn_close;

    UIButton             m_btn_wordmap;

    UIButton             m_btn_currentmap;

    UIWidget             m_widget_BigMapContent;

    Transform            m_trans_IconContainer;

    Transform            m_trans_npcgrid;

    UIButton             m_btn_npcitem;

    UIButton             m_btn_monster;

    UIButton             m_btn_transmit;

    UIScrollView         m_scrollview_npcscrollview;

    UISprite             m_sprite_btn_child;

    Transform            m_trans_npcitemContainer;

    Transform            m_trans_monsterContainer;

    Transform            m_trans_transmitContainer;

    UISprite             m_sprite_iconprefab;

    UIWidget             m_widget_WorldMapContent;

    UITexture            m__wordBg;

    UIButton             m_btn_changshengdian;

    UIButton             m_btn_jiuyoudigong;

    UIButton             m_btn_wanjieku;

    UIButton             m_btn_wuwanghai;

    UILabel              m_label_ChildPrefab;

    UIButton             m_btn_ColliderMask;

    UIGridCreatorBase    m_ctor_bgContent;

    UISprite             m_sprite_TipBg;

    Transform            m_trans_UITeammateMapInfoGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_MapName = fastComponent.FastGetComponent<UILabel>("MapName");
       if( null == m_label_MapName )
       {
            Engine.Utility.Log.Error("m_label_MapName 为空，请检查prefab是否缺乏组件");
       }
        m_label_line_label = fastComponent.FastGetComponent<UILabel>("line_label");
       if( null == m_label_line_label )
       {
            Engine.Utility.Log.Error("m_label_line_label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_wordmap = fastComponent.FastGetComponent<UIButton>("wordmap");
       if( null == m_btn_wordmap )
       {
            Engine.Utility.Log.Error("m_btn_wordmap 为空，请检查prefab是否缺乏组件");
       }
        m_btn_currentmap = fastComponent.FastGetComponent<UIButton>("currentmap");
       if( null == m_btn_currentmap )
       {
            Engine.Utility.Log.Error("m_btn_currentmap 为空，请检查prefab是否缺乏组件");
       }
        m_widget_BigMapContent = fastComponent.FastGetComponent<UIWidget>("BigMapContent");
       if( null == m_widget_BigMapContent )
       {
            Engine.Utility.Log.Error("m_widget_BigMapContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_IconContainer = fastComponent.FastGetComponent<Transform>("IconContainer");
       if( null == m_trans_IconContainer )
       {
            Engine.Utility.Log.Error("m_trans_IconContainer 为空，请检查prefab是否缺乏组件");
       }
        m_trans_npcgrid = fastComponent.FastGetComponent<Transform>("npcgrid");
       if( null == m_trans_npcgrid )
       {
            Engine.Utility.Log.Error("m_trans_npcgrid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_npcitem = fastComponent.FastGetComponent<UIButton>("npcitem");
       if( null == m_btn_npcitem )
       {
            Engine.Utility.Log.Error("m_btn_npcitem 为空，请检查prefab是否缺乏组件");
       }
        m_btn_monster = fastComponent.FastGetComponent<UIButton>("monster");
       if( null == m_btn_monster )
       {
            Engine.Utility.Log.Error("m_btn_monster 为空，请检查prefab是否缺乏组件");
       }
        m_btn_transmit = fastComponent.FastGetComponent<UIButton>("transmit");
       if( null == m_btn_transmit )
       {
            Engine.Utility.Log.Error("m_btn_transmit 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_npcscrollview = fastComponent.FastGetComponent<UIScrollView>("npcscrollview");
       if( null == m_scrollview_npcscrollview )
       {
            Engine.Utility.Log.Error("m_scrollview_npcscrollview 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btn_child = fastComponent.FastGetComponent<UISprite>("btn_child");
       if( null == m_sprite_btn_child )
       {
            Engine.Utility.Log.Error("m_sprite_btn_child 为空，请检查prefab是否缺乏组件");
       }
        m_trans_npcitemContainer = fastComponent.FastGetComponent<Transform>("npcitemContainer");
       if( null == m_trans_npcitemContainer )
       {
            Engine.Utility.Log.Error("m_trans_npcitemContainer 为空，请检查prefab是否缺乏组件");
       }
        m_trans_monsterContainer = fastComponent.FastGetComponent<Transform>("monsterContainer");
       if( null == m_trans_monsterContainer )
       {
            Engine.Utility.Log.Error("m_trans_monsterContainer 为空，请检查prefab是否缺乏组件");
       }
        m_trans_transmitContainer = fastComponent.FastGetComponent<Transform>("transmitContainer");
       if( null == m_trans_transmitContainer )
       {
            Engine.Utility.Log.Error("m_trans_transmitContainer 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_iconprefab = fastComponent.FastGetComponent<UISprite>("iconprefab");
       if( null == m_sprite_iconprefab )
       {
            Engine.Utility.Log.Error("m_sprite_iconprefab 为空，请检查prefab是否缺乏组件");
       }
        m_widget_WorldMapContent = fastComponent.FastGetComponent<UIWidget>("WorldMapContent");
       if( null == m_widget_WorldMapContent )
       {
            Engine.Utility.Log.Error("m_widget_WorldMapContent 为空，请检查prefab是否缺乏组件");
       }
        m__wordBg = fastComponent.FastGetComponent<UITexture>("wordBg");
       if( null == m__wordBg )
       {
            Engine.Utility.Log.Error("m__wordBg 为空，请检查prefab是否缺乏组件");
       }
        m_btn_changshengdian = fastComponent.FastGetComponent<UIButton>("changshengdian");
       if( null == m_btn_changshengdian )
       {
            Engine.Utility.Log.Error("m_btn_changshengdian 为空，请检查prefab是否缺乏组件");
       }
        m_btn_jiuyoudigong = fastComponent.FastGetComponent<UIButton>("jiuyoudigong");
       if( null == m_btn_jiuyoudigong )
       {
            Engine.Utility.Log.Error("m_btn_jiuyoudigong 为空，请检查prefab是否缺乏组件");
       }
        m_btn_wanjieku = fastComponent.FastGetComponent<UIButton>("wanjieku");
       if( null == m_btn_wanjieku )
       {
            Engine.Utility.Log.Error("m_btn_wanjieku 为空，请检查prefab是否缺乏组件");
       }
        m_btn_wuwanghai = fastComponent.FastGetComponent<UIButton>("wuwanghai");
       if( null == m_btn_wuwanghai )
       {
            Engine.Utility.Log.Error("m_btn_wuwanghai 为空，请检查prefab是否缺乏组件");
       }
        m_label_ChildPrefab = fastComponent.FastGetComponent<UILabel>("ChildPrefab");
       if( null == m_label_ChildPrefab )
       {
            Engine.Utility.Log.Error("m_label_ChildPrefab 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ColliderMask = fastComponent.FastGetComponent<UIButton>("ColliderMask");
       if( null == m_btn_ColliderMask )
       {
            Engine.Utility.Log.Error("m_btn_ColliderMask 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_bgContent = fastComponent.FastGetComponent<UIGridCreatorBase>("bgContent");
       if( null == m_ctor_bgContent )
       {
            Engine.Utility.Log.Error("m_ctor_bgContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_TipBg = fastComponent.FastGetComponent<UISprite>("TipBg");
       if( null == m_sprite_TipBg )
       {
            Engine.Utility.Log.Error("m_sprite_TipBg 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UITeammateMapInfoGrid = fastComponent.FastGetComponent<Transform>("UITeammateMapInfoGrid");
       if( null == m_trans_UITeammateMapInfoGrid )
       {
            Engine.Utility.Log.Error("m_trans_UITeammateMapInfoGrid 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_wordmap.gameObject).onClick = _onClick_Wordmap_Btn;
        UIEventListener.Get(m_btn_currentmap.gameObject).onClick = _onClick_Currentmap_Btn;
        UIEventListener.Get(m_btn_npcitem.gameObject).onClick = _onClick_Npcitem_Btn;
        UIEventListener.Get(m_btn_monster.gameObject).onClick = _onClick_Monster_Btn;
        UIEventListener.Get(m_btn_transmit.gameObject).onClick = _onClick_Transmit_Btn;
        UIEventListener.Get(m_btn_changshengdian.gameObject).onClick = _onClick_Changshengdian_Btn;
        UIEventListener.Get(m_btn_jiuyoudigong.gameObject).onClick = _onClick_Jiuyoudigong_Btn;
        UIEventListener.Get(m_btn_wanjieku.gameObject).onClick = _onClick_Wanjieku_Btn;
        UIEventListener.Get(m_btn_wuwanghai.gameObject).onClick = _onClick_Wuwanghai_Btn;
        UIEventListener.Get(m_btn_ColliderMask.gameObject).onClick = _onClick_ColliderMask_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Wordmap_Btn(GameObject caster)
    {
        onClick_Wordmap_Btn( caster );
    }

    void _onClick_Currentmap_Btn(GameObject caster)
    {
        onClick_Currentmap_Btn( caster );
    }

    void _onClick_Npcitem_Btn(GameObject caster)
    {
        onClick_Npcitem_Btn( caster );
    }

    void _onClick_Monster_Btn(GameObject caster)
    {
        onClick_Monster_Btn( caster );
    }

    void _onClick_Transmit_Btn(GameObject caster)
    {
        onClick_Transmit_Btn( caster );
    }

    void _onClick_Changshengdian_Btn(GameObject caster)
    {
        onClick_Changshengdian_Btn( caster );
    }

    void _onClick_Jiuyoudigong_Btn(GameObject caster)
    {
        onClick_Jiuyoudigong_Btn( caster );
    }

    void _onClick_Wanjieku_Btn(GameObject caster)
    {
        onClick_Wanjieku_Btn( caster );
    }

    void _onClick_Wuwanghai_Btn(GameObject caster)
    {
        onClick_Wuwanghai_Btn( caster );
    }

    void _onClick_ColliderMask_Btn(GameObject caster)
    {
        onClick_ColliderMask_Btn( caster );
    }


}
