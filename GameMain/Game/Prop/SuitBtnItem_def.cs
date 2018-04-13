//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SuitBtnItem: UIGridBase
{

    UIButton             m_btn_suitItem;

    UILabel              m_label_nameLabel;

    UISprite             m_sprite_highlight;

    UISprite             m_sprite_open;

    UISprite             m_sprite_Checkmark;


    //初始化控件变量
   protected override void OnAwake()
    {
         InitControls();
         RegisterControlEvents();
    }
    private void InitControls()
    {
        m_btn_suitItem = GetChildComponent<UIButton>("suitItem");
       if( null == m_btn_suitItem )
       {
            Engine.Utility.Log.Error("m_btn_suitItem 为空，请检查prefab是否缺乏组件");
       }
        m_label_nameLabel = GetChildComponent<UILabel>("nameLabel");
       if( null == m_label_nameLabel )
       {
            Engine.Utility.Log.Error("m_label_nameLabel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_highlight = GetChildComponent<UISprite>("highlight");
       if( null == m_sprite_highlight )
       {
            Engine.Utility.Log.Error("m_sprite_highlight 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_open = GetChildComponent<UISprite>("open");
       if( null == m_sprite_open )
       {
            Engine.Utility.Log.Error("m_sprite_open 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Checkmark = GetChildComponent<UISprite>("Checkmark");
       if( null == m_sprite_Checkmark )
       {
            Engine.Utility.Log.Error("m_sprite_Checkmark 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    private void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_suitItem.gameObject).onClick = _onClick_SuitItem_Btn;
    }

    void _onClick_SuitItem_Btn(GameObject caster)
    {
        onClick_SuitItem_Btn( caster );
    }


}
