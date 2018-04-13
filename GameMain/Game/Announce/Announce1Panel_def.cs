//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class Announce1Panel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btnclose;

    UILabel              m_label_title;

    UISprite             m_sprite_icon;

    UILabel              m_label_desc;

    Transform            m_trans_item;

    UILabel              m_label_conditiondesc;

    UISpriteEx           m_spriteEx_btnget;

    UILabel              m_label_btnLabel;

    Transform            m_trans_UIItemRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btnclose = fastComponent.FastGetComponent<UIButton>("btnclose");
       if( null == m_btn_btnclose )
       {
            Engine.Utility.Log.Error("m_btn_btnclose 为空，请检查prefab是否缺乏组件");
       }
        m_label_title = fastComponent.FastGetComponent<UILabel>("title");
       if( null == m_label_title )
       {
            Engine.Utility.Log.Error("m_label_title 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_icon = fastComponent.FastGetComponent<UISprite>("icon");
       if( null == m_sprite_icon )
       {
            Engine.Utility.Log.Error("m_sprite_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_desc = fastComponent.FastGetComponent<UILabel>("desc");
       if( null == m_label_desc )
       {
            Engine.Utility.Log.Error("m_label_desc 为空，请检查prefab是否缺乏组件");
       }
        m_trans_item = fastComponent.FastGetComponent<Transform>("item");
       if( null == m_trans_item )
       {
            Engine.Utility.Log.Error("m_trans_item 为空，请检查prefab是否缺乏组件");
       }
        m_label_conditiondesc = fastComponent.FastGetComponent<UILabel>("conditiondesc");
       if( null == m_label_conditiondesc )
       {
            Engine.Utility.Log.Error("m_label_conditiondesc 为空，请检查prefab是否缺乏组件");
       }
        m_spriteEx_btnget = fastComponent.FastGetComponent<UISpriteEx>("btnget");
       if( null == m_spriteEx_btnget )
       {
            Engine.Utility.Log.Error("m_spriteEx_btnget 为空，请检查prefab是否缺乏组件");
       }
        m_label_btnLabel = fastComponent.FastGetComponent<UILabel>("btnLabel");
       if( null == m_label_btnLabel )
       {
            Engine.Utility.Log.Error("m_label_btnLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIItemRewardGrid = fastComponent.FastGetComponent<Transform>("UIItemRewardGrid");
       if( null == m_trans_UIItemRewardGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIItemRewardGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btnclose.gameObject).onClick = _onClick_Btnclose_Btn;
    }

    void _onClick_Btnclose_Btn(GameObject caster)
    {
        onClick_Btnclose_Btn( caster );
    }


}
