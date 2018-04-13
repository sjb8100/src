//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class NvWaStartPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_right;

    UILabel              m_label_rightBtn_Label;

    UIButton             m_btn_btn_left;

    UILabel              m_label_leftbtn_Label;

    UISprite             m_sprite_Item;

    UISprite             m_sprite_itemProgressbar;

    UITexture            m__itemIcon;

    UILabel              m_label_itemName;

    UILabel              m_label_itemNum;

    UILabel              m_label_GoldNum;

    UISprite             m_sprite_Gold;

    UIButton             m_btn_yinhun_xiaohaoSprite;

    Transform            m_trans_UseCost;

    UILabel              m_label_UseCostNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_right = fastComponent.FastGetComponent<UIButton>("btn_right");
       if( null == m_btn_btn_right )
       {
            Engine.Utility.Log.Error("m_btn_btn_right 为空，请检查prefab是否缺乏组件");
       }
        m_label_rightBtn_Label = fastComponent.FastGetComponent<UILabel>("rightBtn_Label");
       if( null == m_label_rightBtn_Label )
       {
            Engine.Utility.Log.Error("m_label_rightBtn_Label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_left = fastComponent.FastGetComponent<UIButton>("btn_left");
       if( null == m_btn_btn_left )
       {
            Engine.Utility.Log.Error("m_btn_btn_left 为空，请检查prefab是否缺乏组件");
       }
        m_label_leftbtn_Label = fastComponent.FastGetComponent<UILabel>("leftbtn_Label");
       if( null == m_label_leftbtn_Label )
       {
            Engine.Utility.Log.Error("m_label_leftbtn_Label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Item = fastComponent.FastGetComponent<UISprite>("Item");
       if( null == m_sprite_Item )
       {
            Engine.Utility.Log.Error("m_sprite_Item 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_itemProgressbar = fastComponent.FastGetComponent<UISprite>("itemProgressbar");
       if( null == m_sprite_itemProgressbar )
       {
            Engine.Utility.Log.Error("m_sprite_itemProgressbar 为空，请检查prefab是否缺乏组件");
       }
        m__itemIcon = fastComponent.FastGetComponent<UITexture>("itemIcon");
       if( null == m__itemIcon )
       {
            Engine.Utility.Log.Error("m__itemIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_itemName = fastComponent.FastGetComponent<UILabel>("itemName");
       if( null == m_label_itemName )
       {
            Engine.Utility.Log.Error("m_label_itemName 为空，请检查prefab是否缺乏组件");
       }
        m_label_itemNum = fastComponent.FastGetComponent<UILabel>("itemNum");
       if( null == m_label_itemNum )
       {
            Engine.Utility.Log.Error("m_label_itemNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_GoldNum = fastComponent.FastGetComponent<UILabel>("GoldNum");
       if( null == m_label_GoldNum )
       {
            Engine.Utility.Log.Error("m_label_GoldNum 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Gold = fastComponent.FastGetComponent<UISprite>("Gold");
       if( null == m_sprite_Gold )
       {
            Engine.Utility.Log.Error("m_sprite_Gold 为空，请检查prefab是否缺乏组件");
       }
        m_btn_yinhun_xiaohaoSprite = fastComponent.FastGetComponent<UIButton>("yinhun_xiaohaoSprite");
       if( null == m_btn_yinhun_xiaohaoSprite )
       {
            Engine.Utility.Log.Error("m_btn_yinhun_xiaohaoSprite 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UseCost = fastComponent.FastGetComponent<Transform>("UseCost");
       if( null == m_trans_UseCost )
       {
            Engine.Utility.Log.Error("m_trans_UseCost 为空，请检查prefab是否缺乏组件");
       }
        m_label_UseCostNum = fastComponent.FastGetComponent<UILabel>("UseCostNum");
       if( null == m_label_UseCostNum )
       {
            Engine.Utility.Log.Error("m_label_UseCostNum 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_right.gameObject).onClick = _onClick_Btn_right_Btn;
        UIEventListener.Get(m_btn_btn_left.gameObject).onClick = _onClick_Btn_left_Btn;
        UIEventListener.Get(m_btn_yinhun_xiaohaoSprite.gameObject).onClick = _onClick_Yinhun_xiaohaoSprite_Btn;
    }

    void _onClick_Btn_right_Btn(GameObject caster)
    {
        onClick_Btn_right_Btn( caster );
    }

    void _onClick_Btn_left_Btn(GameObject caster)
    {
        onClick_Btn_left_Btn( caster );
    }

    void _onClick_Yinhun_xiaohaoSprite_Btn(GameObject caster)
    {
        onClick_Yinhun_xiaohaoSprite_Btn( caster );
    }


}
