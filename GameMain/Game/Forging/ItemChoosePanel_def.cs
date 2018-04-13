//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ItemChoosePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_Title;

    UIButton             m_btn_Close;

    UIScrollView         m_scrollview_SelectScrollView;

    UIGrid               m_grid_GridContent;

    UIButton             m_btn_SelectBtn;

    UILabel              m_label_Des;

    UILabel              m_label_NullTips;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_Title = fastComponent.FastGetComponent<UILabel>("Title");
       if( null == m_label_Title )
       {
            Engine.Utility.Log.Error("m_label_Title 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Close = fastComponent.FastGetComponent<UIButton>("Close");
       if( null == m_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_SelectScrollView = fastComponent.FastGetComponent<UIScrollView>("SelectScrollView");
       if( null == m_scrollview_SelectScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_SelectScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_grid_GridContent = fastComponent.FastGetComponent<UIGrid>("GridContent");
       if( null == m_grid_GridContent )
       {
            Engine.Utility.Log.Error("m_grid_GridContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_SelectBtn = fastComponent.FastGetComponent<UIButton>("SelectBtn");
       if( null == m_btn_SelectBtn )
       {
            Engine.Utility.Log.Error("m_btn_SelectBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_Des = fastComponent.FastGetComponent<UILabel>("Des");
       if( null == m_label_Des )
       {
            Engine.Utility.Log.Error("m_label_Des 为空，请检查prefab是否缺乏组件");
       }
        m_label_NullTips = fastComponent.FastGetComponent<UILabel>("NullTips");
       if( null == m_label_NullTips )
       {
            Engine.Utility.Log.Error("m_label_NullTips 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_SelectBtn.gameObject).onClick = _onClick_SelectBtn_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_SelectBtn_Btn(GameObject caster)
    {
        onClick_SelectBtn_Btn( caster );
    }


}
