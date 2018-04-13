//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CampFightingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_CampWarContent;

    UILabel              m_label_GodScore;

    UILabel              m_label_DemonScore;

    UILabel              m_label_AttributionName;

    UIButton             m_btn_BtnBattle;

    UILabel              m_label_RankNum;

    UILabel              m_label_ScoreNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_CampWarContent = fastComponent.FastGetComponent<Transform>("CampWarContent");
       if( null == m_trans_CampWarContent )
       {
            Engine.Utility.Log.Error("m_trans_CampWarContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_GodScore = fastComponent.FastGetComponent<UILabel>("GodScore");
       if( null == m_label_GodScore )
       {
            Engine.Utility.Log.Error("m_label_GodScore 为空，请检查prefab是否缺乏组件");
       }
        m_label_DemonScore = fastComponent.FastGetComponent<UILabel>("DemonScore");
       if( null == m_label_DemonScore )
       {
            Engine.Utility.Log.Error("m_label_DemonScore 为空，请检查prefab是否缺乏组件");
       }
        m_label_AttributionName = fastComponent.FastGetComponent<UILabel>("AttributionName");
       if( null == m_label_AttributionName )
       {
            Engine.Utility.Log.Error("m_label_AttributionName 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnBattle = fastComponent.FastGetComponent<UIButton>("BtnBattle");
       if( null == m_btn_BtnBattle )
       {
            Engine.Utility.Log.Error("m_btn_BtnBattle 为空，请检查prefab是否缺乏组件");
       }
        m_label_RankNum = fastComponent.FastGetComponent<UILabel>("RankNum");
       if( null == m_label_RankNum )
       {
            Engine.Utility.Log.Error("m_label_RankNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_ScoreNum = fastComponent.FastGetComponent<UILabel>("ScoreNum");
       if( null == m_label_ScoreNum )
       {
            Engine.Utility.Log.Error("m_label_ScoreNum 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BtnBattle.gameObject).onClick = _onClick_BtnBattle_Btn;
    }

    void _onClick_BtnBattle_Btn(GameObject caster)
    {
        onClick_BtnBattle_Btn( caster );
    }


}
