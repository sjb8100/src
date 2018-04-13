//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class rewardItemScrollView: UIPanelBase
{

    UIScrollView  m_scrollview_rewardItemScrollView;

    Transform     m_trans_itemRoot;


    //初始化控件变量
    protected override void InitControls()
    {
        m_scrollview_rewardItemScrollView = GetChildComponent<UIScrollView>("rewardItemScrollView");
       if( null == m_scrollview_rewardItemScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_rewardItemScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_itemRoot = GetChildComponent<Transform>("itemRoot");
       if( null == m_trans_itemRoot )
       {
            Engine.Utility.Log.Error("m_trans_itemRoot 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
