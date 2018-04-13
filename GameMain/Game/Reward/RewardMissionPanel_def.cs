//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RewardMissionPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_clostbtn;

    Transform            m_trans_missionRoot;

    UISprite             m_sprite_missiongrid;

    UILabel              m_label_processing;

    UILabel              m_label_done;

    UILabel              m_label_tips;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_clostbtn = fastComponent.FastGetComponent<UIButton>("clostbtn");
       if( null == m_btn_clostbtn )
       {
            Engine.Utility.Log.Error("m_btn_clostbtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_missionRoot = fastComponent.FastGetComponent<Transform>("missionRoot");
       if( null == m_trans_missionRoot )
       {
            Engine.Utility.Log.Error("m_trans_missionRoot 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_missiongrid = fastComponent.FastGetComponent<UISprite>("missiongrid");
       if( null == m_sprite_missiongrid )
       {
            Engine.Utility.Log.Error("m_sprite_missiongrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_processing = fastComponent.FastGetComponent<UILabel>("processing");
       if( null == m_label_processing )
       {
            Engine.Utility.Log.Error("m_label_processing 为空，请检查prefab是否缺乏组件");
       }
        m_label_done = fastComponent.FastGetComponent<UILabel>("done");
       if( null == m_label_done )
       {
            Engine.Utility.Log.Error("m_label_done 为空，请检查prefab是否缺乏组件");
       }
        m_label_tips = fastComponent.FastGetComponent<UILabel>("tips");
       if( null == m_label_tips )
       {
            Engine.Utility.Log.Error("m_label_tips 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_clostbtn.gameObject).onClick = _onClick_Clostbtn_Btn;
    }

    void _onClick_Clostbtn_Btn(GameObject caster)
    {
        onClick_Clostbtn_Btn( caster );
    }


}
