//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ChooseServerPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UISprite             m_sprite_page_btnPrefab;

    UIGridCreatorBase    m_ctor_LeftPanel;

    UISprite             m_sprite_SeverItemPrefab;

    UISprite             m_sprite_Tip;

    UIButton             m_btn_closeBtn;

    UILabel              m_label_name;

    Transform            m_trans_recommond;

    Transform            m_trans_topRoot;

    UIGridCreatorBase    m_ctor_RightPanel;

    Transform            m_trans_ServerlistRoot;

    UIGridCreatorBase    m_ctor_ServerlistPanel;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_sprite_page_btnPrefab = fastComponent.FastGetComponent<UISprite>("page_btnPrefab");
       if( null == m_sprite_page_btnPrefab )
       {
            Engine.Utility.Log.Error("m_sprite_page_btnPrefab 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_LeftPanel = fastComponent.FastGetComponent<UIGridCreatorBase>("LeftPanel");
       if( null == m_ctor_LeftPanel )
       {
            Engine.Utility.Log.Error("m_ctor_LeftPanel 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_SeverItemPrefab = fastComponent.FastGetComponent<UISprite>("SeverItemPrefab");
       if( null == m_sprite_SeverItemPrefab )
       {
            Engine.Utility.Log.Error("m_sprite_SeverItemPrefab 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Tip = fastComponent.FastGetComponent<UISprite>("Tip");
       if( null == m_sprite_Tip )
       {
            Engine.Utility.Log.Error("m_sprite_Tip 为空，请检查prefab是否缺乏组件");
       }
        m_btn_closeBtn = fastComponent.FastGetComponent<UIButton>("closeBtn");
       if( null == m_btn_closeBtn )
       {
            Engine.Utility.Log.Error("m_btn_closeBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_trans_recommond = fastComponent.FastGetComponent<Transform>("recommond");
       if( null == m_trans_recommond )
       {
            Engine.Utility.Log.Error("m_trans_recommond 为空，请检查prefab是否缺乏组件");
       }
        m_trans_topRoot = fastComponent.FastGetComponent<Transform>("topRoot");
       if( null == m_trans_topRoot )
       {
            Engine.Utility.Log.Error("m_trans_topRoot 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_RightPanel = fastComponent.FastGetComponent<UIGridCreatorBase>("RightPanel");
       if( null == m_ctor_RightPanel )
       {
            Engine.Utility.Log.Error("m_ctor_RightPanel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ServerlistRoot = fastComponent.FastGetComponent<Transform>("ServerlistRoot");
       if( null == m_trans_ServerlistRoot )
       {
            Engine.Utility.Log.Error("m_trans_ServerlistRoot 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ServerlistPanel = fastComponent.FastGetComponent<UIGridCreatorBase>("ServerlistPanel");
       if( null == m_ctor_ServerlistPanel )
       {
            Engine.Utility.Log.Error("m_ctor_ServerlistPanel 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_closeBtn.gameObject).onClick = _onClick_CloseBtn_Btn;
    }

    void _onClick_CloseBtn_Btn(GameObject caster)
    {
        onClick_CloseBtn_Btn( caster );
    }


}
