//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetEmbattlePanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_BiaoTi_Label;

    UIButton             m_btn_btn_Close;

    UIGrid               m_grid_UpRoot;

    UIGrid               m_grid_DownRoot;

    UIButton             m_btn_guiyuanCommon_zidongbuzu;

    UISprite             m_sprite_gouSprite;

    Transform            m_trans_PetEmbatteGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_label_BiaoTi_Label = fastComponent.FastGetComponent<UILabel>("BiaoTi_Label");
       if( null == m_label_BiaoTi_Label )
       {
            Engine.Utility.Log.Error("m_label_BiaoTi_Label 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_Close = fastComponent.FastGetComponent<UIButton>("btn_Close");
       if( null == m_btn_btn_Close )
       {
            Engine.Utility.Log.Error("m_btn_btn_Close 为空，请检查prefab是否缺乏组件");
       }
        m_grid_UpRoot = fastComponent.FastGetComponent<UIGrid>("UpRoot");
       if( null == m_grid_UpRoot )
       {
            Engine.Utility.Log.Error("m_grid_UpRoot 为空，请检查prefab是否缺乏组件");
       }
        m_grid_DownRoot = fastComponent.FastGetComponent<UIGrid>("DownRoot");
       if( null == m_grid_DownRoot )
       {
            Engine.Utility.Log.Error("m_grid_DownRoot 为空，请检查prefab是否缺乏组件");
       }
        m_btn_guiyuanCommon_zidongbuzu = fastComponent.FastGetComponent<UIButton>("guiyuanCommon_zidongbuzu");
       if( null == m_btn_guiyuanCommon_zidongbuzu )
       {
            Engine.Utility.Log.Error("m_btn_guiyuanCommon_zidongbuzu 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_gouSprite = fastComponent.FastGetComponent<UISprite>("gouSprite");
       if( null == m_sprite_gouSprite )
       {
            Engine.Utility.Log.Error("m_sprite_gouSprite 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PetEmbatteGrid = fastComponent.FastGetComponent<Transform>("PetEmbatteGrid");
       if( null == m_trans_PetEmbatteGrid )
       {
            Engine.Utility.Log.Error("m_trans_PetEmbatteGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_Close.gameObject).onClick = _onClick_Btn_Close_Btn;
        UIEventListener.Get(m_btn_guiyuanCommon_zidongbuzu.gameObject).onClick = _onClick_GuiyuanCommon_zidongbuzu_Btn;
    }

    void _onClick_Btn_Close_Btn(GameObject caster)
    {
        onClick_Btn_Close_Btn( caster );
    }

    void _onClick_GuiyuanCommon_zidongbuzu_Btn(GameObject caster)
    {
        onClick_GuiyuanCommon_zidongbuzu_Btn( caster );
    }


}
