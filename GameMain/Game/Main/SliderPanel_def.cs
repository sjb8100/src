//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SliderPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISlider             m_slider_SkillProgressBar;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_slider_SkillProgressBar = fastComponent.FastGetComponent<UISlider>("SkillProgressBar");
       if( null == m_slider_SkillProgressBar )
       {
            Engine.Utility.Log.Error("m_slider_SkillProgressBar 为空，请检查prefab是否缺乏组件");
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
