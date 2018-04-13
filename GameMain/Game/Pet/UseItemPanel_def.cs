//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class UseItemPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UILabel       m_label_name;

    UIWidget      m_widget_itemprefab;

    UISprite      m_sprite_icon;

    UILabel       m_label_Times;

    UIButton      m_btn_btn_unclose;

    UIScrollView  m_scrollview_ItemScrollView;

    UIGrid        m_grid_ItemGrid;


    //初始化控件变量
    protected override void InitControls()
    {
        m_label_name = GetChildComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_widget_itemprefab = GetChildComponent<UIWidget>("itemprefab");
       if( null == m_widget_itemprefab )
       {
            Engine.Utility.Log.Error("m_widget_itemprefab 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_icon = GetChildComponent<UISprite>("icon");
       if( null == m_sprite_icon )
       {
            Engine.Utility.Log.Error("m_sprite_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_Times = GetChildComponent<UILabel>("Times");
       if( null == m_label_Times )
       {
            Engine.Utility.Log.Error("m_label_Times 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_unclose = GetChildComponent<UIButton>("btn_unclose");
       if( null == m_btn_btn_unclose )
       {
            Engine.Utility.Log.Error("m_btn_btn_unclose 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_ItemScrollView = GetChildComponent<UIScrollView>("ItemScrollView");
       if( null == m_scrollview_ItemScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_ItemScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_grid_ItemGrid = GetChildComponent<UIGrid>("ItemGrid");
       if( null == m_grid_ItemGrid )
       {
            Engine.Utility.Log.Error("m_grid_ItemGrid 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_unclose.gameObject).onClick = _onClick_Btn_unclose_Btn;
    }

    void _onClick_Btn_unclose_Btn(GameObject caster)
    {
        //onClick_Btn_unclose_Btn( caster );
    }


}
