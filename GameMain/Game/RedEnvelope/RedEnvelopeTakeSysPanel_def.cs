//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RedEnvelopeTakeSysPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UITexture            m__enter_right;

    UILabel              m_label_up_label;

    UILabel              m_label_down_label;

    Transform            m_trans_redeffect;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m__enter_right = fastComponent.FastGetComponent<UITexture>("enter_right");
       if( null == m__enter_right )
       {
            Engine.Utility.Log.Error("m__enter_right 为空，请检查prefab是否缺乏组件");
       }
        m_label_up_label = fastComponent.FastGetComponent<UILabel>("up_label");
       if( null == m_label_up_label )
       {
            Engine.Utility.Log.Error("m_label_up_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_down_label = fastComponent.FastGetComponent<UILabel>("down_label");
       if( null == m_label_down_label )
       {
            Engine.Utility.Log.Error("m_label_down_label 为空，请检查prefab是否缺乏组件");
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
