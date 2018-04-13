//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ReLivePanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_ReliveContent;

    UISprite             m_sprite_bg;

    UIGrid               m_grid_Grid;

    UIButton             m_btn_UIReliveGrid;

    UILabel              m_label_relive_cost;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_ReliveContent = fastComponent.FastGetComponent<Transform>("ReliveContent");
       if( null == m_trans_ReliveContent )
       {
            Engine.Utility.Log.Error("m_trans_ReliveContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_bg = fastComponent.FastGetComponent<UISprite>("bg");
       if( null == m_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
        m_grid_Grid = fastComponent.FastGetComponent<UIGrid>("Grid");
       if( null == m_grid_Grid )
       {
            Engine.Utility.Log.Error("m_grid_Grid 为空，请检查prefab是否缺乏组件");
       }
        m_btn_UIReliveGrid = fastComponent.FastGetComponent<UIButton>("UIReliveGrid");
       if( null == m_btn_UIReliveGrid )
       {
            Engine.Utility.Log.Error("m_btn_UIReliveGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_relive_cost = fastComponent.FastGetComponent<UILabel>("relive_cost");
       if( null == m_label_relive_cost )
       {
            Engine.Utility.Log.Error("m_label_relive_cost 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_UIReliveGrid.gameObject).onClick = _onClick_UIReliveGrid_Btn;
    }

    void _onClick_UIReliveGrid_Btn(GameObject caster)
    {
        onClick_UIReliveGrid_Btn( caster );
    }


}
