//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class MissionPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//已接
		YiJie = 1,
		//可接
		KeJie = 2,
		Max,
    }

   FastComponent         fastComponent;

    UISprite             m_sprite_missionTile;

    Transform            m_trans_detailInfo;

    UILabel              m_label_des_label;

    UILabel              m_label_taskName;

    UIWidget             m_widget_taskDesc;

    UILabel              m_label_taskReward;

    UIButton             m_btn_btnGiveUp;

    UIButton             m_btn_btnGoOn;

    Transform            m_trans_taskRewardRoot;

    UITable              m__missonRoot;

    UIScrollView         m_scrollview_alreadyDoMission;

    UIScrollView         m_scrollview_canDoMission;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_missionTile = fastComponent.FastGetComponent<UISprite>("missionTile");
       if( null == m_sprite_missionTile )
       {
            Engine.Utility.Log.Error("m_sprite_missionTile 为空，请检查prefab是否缺乏组件");
       }
        m_trans_detailInfo = fastComponent.FastGetComponent<Transform>("detailInfo");
       if( null == m_trans_detailInfo )
       {
            Engine.Utility.Log.Error("m_trans_detailInfo 为空，请检查prefab是否缺乏组件");
       }
        m_label_des_label = fastComponent.FastGetComponent<UILabel>("des_label");
       if( null == m_label_des_label )
       {
            Engine.Utility.Log.Error("m_label_des_label 为空，请检查prefab是否缺乏组件");
       }
        m_label_taskName = fastComponent.FastGetComponent<UILabel>("taskName");
       if( null == m_label_taskName )
       {
            Engine.Utility.Log.Error("m_label_taskName 为空，请检查prefab是否缺乏组件");
       }
        m_widget_taskDesc = fastComponent.FastGetComponent<UIWidget>("taskDesc");
       if( null == m_widget_taskDesc )
       {
            Engine.Utility.Log.Error("m_widget_taskDesc 为空，请检查prefab是否缺乏组件");
       }
        m_label_taskReward = fastComponent.FastGetComponent<UILabel>("taskReward");
       if( null == m_label_taskReward )
       {
            Engine.Utility.Log.Error("m_label_taskReward 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnGiveUp = fastComponent.FastGetComponent<UIButton>("btnGiveUp");
       if( null == m_btn_btnGiveUp )
       {
            Engine.Utility.Log.Error("m_btn_btnGiveUp 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btnGoOn = fastComponent.FastGetComponent<UIButton>("btnGoOn");
       if( null == m_btn_btnGoOn )
       {
            Engine.Utility.Log.Error("m_btn_btnGoOn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_taskRewardRoot = fastComponent.FastGetComponent<Transform>("taskRewardRoot");
       if( null == m_trans_taskRewardRoot )
       {
            Engine.Utility.Log.Error("m_trans_taskRewardRoot 为空，请检查prefab是否缺乏组件");
       }
        m__missonRoot = fastComponent.FastGetComponent<UITable>("missonRoot");
       if( null == m__missonRoot )
       {
            Engine.Utility.Log.Error("m__missonRoot 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_alreadyDoMission = fastComponent.FastGetComponent<UIScrollView>("alreadyDoMission");
       if( null == m_scrollview_alreadyDoMission )
       {
            Engine.Utility.Log.Error("m_scrollview_alreadyDoMission 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_canDoMission = fastComponent.FastGetComponent<UIScrollView>("canDoMission");
       if( null == m_scrollview_canDoMission )
       {
            Engine.Utility.Log.Error("m_scrollview_canDoMission 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btnGiveUp.gameObject).onClick = _onClick_BtnGiveUp_Btn;
        UIEventListener.Get(m_btn_btnGoOn.gameObject).onClick = _onClick_BtnGoOn_Btn;
    }

    void _onClick_BtnGiveUp_Btn(GameObject caster)
    {
        onClick_BtnGiveUp_Btn( caster );
    }

    void _onClick_BtnGoOn_Btn(GameObject caster)
    {
        onClick_BtnGoOn_Btn( caster );
    }


}
