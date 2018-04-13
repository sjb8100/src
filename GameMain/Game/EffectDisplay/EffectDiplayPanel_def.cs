//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class EffectDiplayPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_MapNameRoot;

    UILabel              m_label_MapName;

    Transform            m_trans_ParticleEffectRoot;

    Transform            m_trans_TipsEffectRoot;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_MapNameRoot = fastComponent.FastGetComponent<Transform>("MapNameRoot");
       if( null == m_trans_MapNameRoot )
       {
            Engine.Utility.Log.Error("m_trans_MapNameRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_MapName = fastComponent.FastGetComponent<UILabel>("MapName");
       if( null == m_label_MapName )
       {
            Engine.Utility.Log.Error("m_label_MapName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ParticleEffectRoot = fastComponent.FastGetComponent<Transform>("ParticleEffectRoot");
       if( null == m_trans_ParticleEffectRoot )
       {
            Engine.Utility.Log.Error("m_trans_ParticleEffectRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TipsEffectRoot = fastComponent.FastGetComponent<Transform>("TipsEffectRoot");
       if( null == m_trans_TipsEffectRoot )
       {
            Engine.Utility.Log.Error("m_trans_TipsEffectRoot 为空，请检查prefab是否缺乏组件");
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
