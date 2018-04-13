//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class NvWaPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_BattleContent;

    Transform            m_trans_Round;

    UILabel              m_label_NvWaRoundNum;

    UILabel              m_label_NvWaExpNum;

    Transform            m_trans_DesAndCD;

    UILabel              m_label_battleDes;

    UILabel              m_label_NvWaTimeNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_BattleContent = fastComponent.FastGetComponent<Transform>("BattleContent");
       if( null == m_trans_BattleContent )
       {
            Engine.Utility.Log.Error("m_trans_BattleContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Round = fastComponent.FastGetComponent<Transform>("Round");
       if( null == m_trans_Round )
       {
            Engine.Utility.Log.Error("m_trans_Round 为空，请检查prefab是否缺乏组件");
       }
        m_label_NvWaRoundNum = fastComponent.FastGetComponent<UILabel>("NvWaRoundNum");
       if( null == m_label_NvWaRoundNum )
       {
            Engine.Utility.Log.Error("m_label_NvWaRoundNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_NvWaExpNum = fastComponent.FastGetComponent<UILabel>("NvWaExpNum");
       if( null == m_label_NvWaExpNum )
       {
            Engine.Utility.Log.Error("m_label_NvWaExpNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DesAndCD = fastComponent.FastGetComponent<Transform>("DesAndCD");
       if( null == m_trans_DesAndCD )
       {
            Engine.Utility.Log.Error("m_trans_DesAndCD 为空，请检查prefab是否缺乏组件");
       }
        m_label_battleDes = fastComponent.FastGetComponent<UILabel>("battleDes");
       if( null == m_label_battleDes )
       {
            Engine.Utility.Log.Error("m_label_battleDes 为空，请检查prefab是否缺乏组件");
       }
        m_label_NvWaTimeNum = fastComponent.FastGetComponent<UILabel>("NvWaTimeNum");
       if( null == m_label_NvWaTimeNum )
       {
            Engine.Utility.Log.Error("m_label_NvWaTimeNum 为空，请检查prefab是否缺乏组件");
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
