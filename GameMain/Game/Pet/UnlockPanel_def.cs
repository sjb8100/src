//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class UnlockPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_unlock_close;

    UILabel              m_label_unlock_name;

    UILabel              m_label_unlock_item_name;

    UISprite             m_sprite_unlock_item_icon;

    UIButton             m_btn_unlock_quxiao;

    UIButton             m_btn_unlock_queding;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_unlock_close = fastComponent.FastGetComponent<UIButton>("unlock_close");
       if( null == m_btn_unlock_close )
       {
            Engine.Utility.Log.Error("m_btn_unlock_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_unlock_name = fastComponent.FastGetComponent<UILabel>("unlock_name");
       if( null == m_label_unlock_name )
       {
            Engine.Utility.Log.Error("m_label_unlock_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_unlock_item_name = fastComponent.FastGetComponent<UILabel>("unlock_item_name");
       if( null == m_label_unlock_item_name )
       {
            Engine.Utility.Log.Error("m_label_unlock_item_name 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_unlock_item_icon = fastComponent.FastGetComponent<UISprite>("unlock_item_icon");
       if( null == m_sprite_unlock_item_icon )
       {
            Engine.Utility.Log.Error("m_sprite_unlock_item_icon 为空，请检查prefab是否缺乏组件");
       }
        m_btn_unlock_quxiao = fastComponent.FastGetComponent<UIButton>("unlock_quxiao");
       if( null == m_btn_unlock_quxiao )
       {
            Engine.Utility.Log.Error("m_btn_unlock_quxiao 为空，请检查prefab是否缺乏组件");
       }
        m_btn_unlock_queding = fastComponent.FastGetComponent<UIButton>("unlock_queding");
       if( null == m_btn_unlock_queding )
       {
            Engine.Utility.Log.Error("m_btn_unlock_queding 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_unlock_close.gameObject).onClick = _onClick_Unlock_close_Btn;
        UIEventListener.Get(m_btn_unlock_quxiao.gameObject).onClick = _onClick_Unlock_quxiao_Btn;
        UIEventListener.Get(m_btn_unlock_queding.gameObject).onClick = _onClick_Unlock_queding_Btn;
    }

    void _onClick_Unlock_close_Btn(GameObject caster)
    {
        onClick_Unlock_close_Btn( caster );
    }

    void _onClick_Unlock_quxiao_Btn(GameObject caster)
    {
        onClick_Unlock_quxiao_Btn( caster );
    }

    void _onClick_Unlock_queding_Btn(GameObject caster)
    {
        onClick_Unlock_queding_Btn( caster );
    }


}
