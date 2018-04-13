//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class HandInputPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    UIGrid               m_grid_KeyBoard;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_Content = fastComponent.FastGetComponent<Transform>("Content");
       if( null == m_trans_Content )
       {
            Engine.Utility.Log.Error("m_trans_Content 为空，请检查prefab是否缺乏组件");
       }
        m_grid_KeyBoard = fastComponent.FastGetComponent<UIGrid>("KeyBoard");
       if( null == m_grid_KeyBoard )
       {
            Engine.Utility.Log.Error("m_grid_KeyBoard 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
