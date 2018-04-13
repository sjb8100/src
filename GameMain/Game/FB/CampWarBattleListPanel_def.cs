//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class CampWarBattleListPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_Close;

    UILabel              m_label_GodScore;

    UILabel              m_label_CampWarGodBossNum;

    UILabel              m_label_CampWarGodReLivePointNum;

    UILabel              m_label_DemonScore;

    UILabel              m_label_CampWarDemonBossNum;

    UILabel              m_label_CampWarDemonReLivePointNum;

    UIGridCreatorBase    m_ctor_GodScrollView;

    UIGridCreatorBase    m_ctor_DemonScrollView;

    Transform            m_trans_MyRankMessage;

    UILabel              m_label_MyRank;

    UILabel              m_label_MyName;

    UILabel              m_label_MyScore;

    UILabel              m_label_MyKill;

    UILabel              m_label_MyDead;

    UILabel              m_label_MyAssists;

    UILabel              m_label_GodNum;

    UILabel              m_label_DemonNum;

    UILabel              m_label_BattleTimeNum;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_Close = fastComponent.FastGetComponent<UIButton>("Close");
       if( null == m_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_label_GodScore = fastComponent.FastGetComponent<UILabel>("GodScore");
       if( null == m_label_GodScore )
       {
            Engine.Utility.Log.Error("m_label_GodScore 为空，请检查prefab是否缺乏组件");
       }
        m_label_CampWarGodBossNum = fastComponent.FastGetComponent<UILabel>("CampWarGodBossNum");
       if( null == m_label_CampWarGodBossNum )
       {
            Engine.Utility.Log.Error("m_label_CampWarGodBossNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_CampWarGodReLivePointNum = fastComponent.FastGetComponent<UILabel>("CampWarGodReLivePointNum");
       if( null == m_label_CampWarGodReLivePointNum )
       {
            Engine.Utility.Log.Error("m_label_CampWarGodReLivePointNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_DemonScore = fastComponent.FastGetComponent<UILabel>("DemonScore");
       if( null == m_label_DemonScore )
       {
            Engine.Utility.Log.Error("m_label_DemonScore 为空，请检查prefab是否缺乏组件");
       }
        m_label_CampWarDemonBossNum = fastComponent.FastGetComponent<UILabel>("CampWarDemonBossNum");
       if( null == m_label_CampWarDemonBossNum )
       {
            Engine.Utility.Log.Error("m_label_CampWarDemonBossNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_CampWarDemonReLivePointNum = fastComponent.FastGetComponent<UILabel>("CampWarDemonReLivePointNum");
       if( null == m_label_CampWarDemonReLivePointNum )
       {
            Engine.Utility.Log.Error("m_label_CampWarDemonReLivePointNum 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_GodScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("GodScrollView");
       if( null == m_ctor_GodScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_GodScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_DemonScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("DemonScrollView");
       if( null == m_ctor_DemonScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_DemonScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MyRankMessage = fastComponent.FastGetComponent<Transform>("MyRankMessage");
       if( null == m_trans_MyRankMessage )
       {
            Engine.Utility.Log.Error("m_trans_MyRankMessage 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyRank = fastComponent.FastGetComponent<UILabel>("MyRank");
       if( null == m_label_MyRank )
       {
            Engine.Utility.Log.Error("m_label_MyRank 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyName = fastComponent.FastGetComponent<UILabel>("MyName");
       if( null == m_label_MyName )
       {
            Engine.Utility.Log.Error("m_label_MyName 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyScore = fastComponent.FastGetComponent<UILabel>("MyScore");
       if( null == m_label_MyScore )
       {
            Engine.Utility.Log.Error("m_label_MyScore 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyKill = fastComponent.FastGetComponent<UILabel>("MyKill");
       if( null == m_label_MyKill )
       {
            Engine.Utility.Log.Error("m_label_MyKill 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyDead = fastComponent.FastGetComponent<UILabel>("MyDead");
       if( null == m_label_MyDead )
       {
            Engine.Utility.Log.Error("m_label_MyDead 为空，请检查prefab是否缺乏组件");
       }
        m_label_MyAssists = fastComponent.FastGetComponent<UILabel>("MyAssists");
       if( null == m_label_MyAssists )
       {
            Engine.Utility.Log.Error("m_label_MyAssists 为空，请检查prefab是否缺乏组件");
       }
        m_label_GodNum = fastComponent.FastGetComponent<UILabel>("GodNum");
       if( null == m_label_GodNum )
       {
            Engine.Utility.Log.Error("m_label_GodNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_DemonNum = fastComponent.FastGetComponent<UILabel>("DemonNum");
       if( null == m_label_DemonNum )
       {
            Engine.Utility.Log.Error("m_label_DemonNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_BattleTimeNum = fastComponent.FastGetComponent<UILabel>("BattleTimeNum");
       if( null == m_label_BattleTimeNum )
       {
            Engine.Utility.Log.Error("m_label_BattleTimeNum 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_Close.gameObject).onClick = _onClick_Close_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }


}
