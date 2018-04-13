//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TipsContainerPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    Transform     m_trans_Content;

    UIButton      m_btn_TipsWidnowGrid;

    UIGrid        m_grid_Grid;


    //初始化控件变量
    protected override void InitControls()
    {
        m_trans_Content = GetChildComponent<Transform>("Content");
       if( null == m_trans_Content )
       {
            Engine.Utility.Log.Error("m_trans_Content 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TipsWidnowGrid = GetChildComponent<UIButton>("TipsWidnowGrid");
       if( null == m_btn_TipsWidnowGrid )
       {
            Engine.Utility.Log.Error("m_btn_TipsWidnowGrid 为空，请检查prefab是否缺乏组件");
       }
        m_grid_Grid = GetChildComponent<UIGrid>("Grid");
       if( null == m_grid_Grid )
       {
            Engine.Utility.Log.Error("m_grid_Grid 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_TipsWidnowGrid.gameObject).onClick = _onClick_TipsWidnowGrid_Btn;
    }

    void _onClick_TipsWidnowGrid_Btn(GameObject caster)
    {
        onClick_TipsWidnowGrid_Btn( caster );
    }


}
