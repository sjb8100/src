//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GiftbagGetPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_Title_Label;

    UIButton             m_btn_btn_Close;

    UIButton             m_btn_btn_take;

    UILabel              m_label_right_Label;

    UISprite             m_sprite_btn_take_waring;

    UISprite             m_sprite_btn_notake;

    UISprite             m_sprite_btn_alreadyTake;

    UILabel              m_label_BiaoTi_Label;

    UIGrid               m_grid_rewardroot;

    Transform            m_trans_UIItemRewardGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_Title_Label = fastComponent.FastGetComponent<UILabel>("Title_Label");
       if( null == m_label_Title_Label )
       {
            Engine.Utility.Log.Error("m_label_Title_Label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Close = fastComponent.FastGetComponent<UIButton>("btn_Close");
       if( null == m_btn_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_take = fastComponent.FastGetComponent<UIButton>("btn_take");
       if( null == m_btn_btn_take )
       {
            Engine.Utility.Log.Error("m_btn_btn_take 为空，请检查prefab是否缺乏组件");
       }
        m_label_right_Label = fastComponent.FastGetComponent<UILabel>("right_Label");
       if( null == m_label_right_Label )
       {
            Engine.Utility.Log.Error("m_label_right_Label 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btn_take_waring = fastComponent.FastGetComponent<UISprite>("btn_take_waring");
       if( null == m_sprite_btn_take_waring )
       {
            Engine.Utility.Log.Error("m_sprite_btn_take_waring 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btn_notake = fastComponent.FastGetComponent<UISprite>("btn_notake");
       if( null == m_sprite_btn_notake )
       {
            Engine.Utility.Log.Error("m_sprite_btn_notake 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_btn_alreadyTake = fastComponent.FastGetComponent<UISprite>("btn_alreadyTake");
       if( null == m_sprite_btn_alreadyTake )
       {
            Engine.Utility.Log.Error("m_sprite_btn_alreadyTake 为空，请检查prefab是否缺乏组件");
       }
        m_label_BiaoTi_Label = fastComponent.FastGetComponent<UILabel>("BiaoTi_Label");
       if( null == m_label_BiaoTi_Label )
       {
            Engine.Utility.Log.Error("m_label_BiaoTi_Label 为空，请检查prefab是否缺乏组件");
       }
        m_grid_rewardroot = fastComponent.FastGetComponent<UIGrid>("rewardroot");
       if( null == m_grid_rewardroot )
       {
            Engine.Utility.Log.Error("m_grid_rewardroot 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_Close.gameObject).onClick = _onClick_Btn_Close_Btn;
        UIEventListener.Get(m_btn_btn_take.gameObject).onClick = _onClick_Btn_take_Btn;
    }

    void _onClick_Btn_Close_Btn(GameObject caster)
    {
        onClick_Btn_Close_Btn( caster );
    }

    void _onClick_Btn_take_Btn(GameObject caster)
    {
        onClick_Btn_take_Btn( caster );
    }


}
