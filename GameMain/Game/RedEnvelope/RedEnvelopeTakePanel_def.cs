//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RedEnvelopeTakePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UITexture            m__enter_left;

    Transform            m_trans_redeffect;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m__enter_left = fastComponent.FastGetComponent<UITexture>("enter_left");
       if( null == m__enter_left )
       {
            Engine.Utility.Log.Error("m__enter_left 为空，请检查prefab是否缺乏组件");
       }
        m_trans_redeffect = fastComponent.FastGetComponent<Transform>("redeffect");
       if( null == m_trans_redeffect )
       {
            Engine.Utility.Log.Error("m_trans_redeffect 为空，请检查prefab是否缺乏组件");
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
