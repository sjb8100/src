//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CityWarFightingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_CityWarWarContent;

    UIGrid               m_grid_TotemGridRoot;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_CityWarWarContent = fastComponent.FastGetComponent<Transform>("CityWarWarContent");
       if( null == m_trans_CityWarWarContent )
       {
            Engine.Utility.Log.Error("m_trans_CityWarWarContent 为空，请检查prefab是否缺乏组件");
       }
        m_grid_TotemGridRoot = fastComponent.FastGetComponent<UIGrid>("TotemGridRoot");
       if( null == m_grid_TotemGridRoot )
       {
            Engine.Utility.Log.Error("m_grid_TotemGridRoot 为空，请检查prefab是否缺乏组件");
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
