//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PlantingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_SeedItem;

    UILabel              m_label_tittle;

    UIButton             m_btn_btnAutoplay;

    UILabel              m_label_autoPlayLabel;

    UIScrollView         m_scrollview_SeedSrollView;

    UIGrid               m_grid_seedgrid;

    UIWidget             m_widget_close;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_SeedItem = fastComponent.FastGetComponent<UISprite>("SeedItem");
       if( null == m_sprite_SeedItem )
       {
            Engine.Utility.Log.Error("m_sprite_SeedItem 为空，请检查prefab是否缺乏组件");
       }
        m_label_tittle = fastComponent.FastGetComponent<UILabel>("tittle");
       if( null == m_label_tittle )
       {
            Engine.Utility.Log.Error("m_label_tittle 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnAutoplay = fastComponent.FastGetComponent<UIButton>("btnAutoplay");
       if( null == m_btn_btnAutoplay )
       {
            Engine.Utility.Log.Error("m_btn_btnAutoplay 为空，请检查prefab是否缺乏组件");
       }
        m_label_autoPlayLabel = fastComponent.FastGetComponent<UILabel>("autoPlayLabel");
       if( null == m_label_autoPlayLabel )
       {
            Engine.Utility.Log.Error("m_label_autoPlayLabel 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_SeedSrollView = fastComponent.FastGetComponent<UIScrollView>("SeedSrollView");
       if( null == m_scrollview_SeedSrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_SeedSrollView 为空，请检查prefab是否缺乏组件");
       }
        m_grid_seedgrid = fastComponent.FastGetComponent<UIGrid>("seedgrid");
       if( null == m_grid_seedgrid )
       {
            Engine.Utility.Log.Error("m_grid_seedgrid 为空，请检查prefab是否缺乏组件");
       }
        m_widget_close = fastComponent.FastGetComponent<UIWidget>("close");
       if( null == m_widget_close )
       {
            Engine.Utility.Log.Error("m_widget_close 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btnAutoplay.gameObject).onClick = _onClick_BtnAutoplay_Btn;
    }

    void _onClick_BtnAutoplay_Btn(GameObject caster)
    {
        onClick_BtnAutoplay_Btn( caster );
    }


}
