//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetAddPointPlanPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UILabel              m_label_BiaoTi_Label;

    UIButton             m_btn_btn_Close;

    UIButton             m_btn_btn_right;

    UILabel              m_label_totalnum;

    UIGrid               m_grid_leftgrid;

    Transform            m_trans_liliangPoint;

    Transform            m_trans_minjiePoint;

    Transform            m_trans_zhiliPoint;

    Transform            m_trans_tizhiPoint;

    Transform            m_trans_jingshenPoint;

    Transform            m_trans_lilianggou;

    Transform            m_trans_minjiegou;

    Transform            m_trans_zhiligou;

    Transform            m_trans_tizhigou;

    Transform            m_trans_jingshengou;


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
        m_btn_btn_right = fastComponent.FastGetComponent<UIButton>("btn_right");
       if( null == m_btn_btn_right )
       {
            Engine.Utility.Log.Error("m_btn_btn_right 为空，请检查prefab是否缺乏组件");
       }
        m_label_totalnum = fastComponent.FastGetComponent<UILabel>("totalnum");
       if( null == m_label_totalnum )
       {
            Engine.Utility.Log.Error("m_label_totalnum 为空，请检查prefab是否缺乏组件");
       }
        m_grid_leftgrid = fastComponent.FastGetComponent<UIGrid>("leftgrid");
       if( null == m_grid_leftgrid )
       {
            Engine.Utility.Log.Error("m_grid_leftgrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_liliangPoint = fastComponent.FastGetComponent<Transform>("liliangPoint");
       if( null == m_trans_liliangPoint )
       {
            Engine.Utility.Log.Error("m_trans_liliangPoint 为空，请检查prefab是否缺乏组件");
       }
        m_trans_minjiePoint = fastComponent.FastGetComponent<Transform>("minjiePoint");
       if( null == m_trans_minjiePoint )
       {
            Engine.Utility.Log.Error("m_trans_minjiePoint 为空，请检查prefab是否缺乏组件");
       }
        m_trans_zhiliPoint = fastComponent.FastGetComponent<Transform>("zhiliPoint");
       if( null == m_trans_zhiliPoint )
       {
            Engine.Utility.Log.Error("m_trans_zhiliPoint 为空，请检查prefab是否缺乏组件");
       }
        m_trans_tizhiPoint = fastComponent.FastGetComponent<Transform>("tizhiPoint");
       if( null == m_trans_tizhiPoint )
       {
            Engine.Utility.Log.Error("m_trans_tizhiPoint 为空，请检查prefab是否缺乏组件");
       }
        m_trans_jingshenPoint = fastComponent.FastGetComponent<Transform>("jingshenPoint");
       if( null == m_trans_jingshenPoint )
       {
            Engine.Utility.Log.Error("m_trans_jingshenPoint 为空，请检查prefab是否缺乏组件");
       }
        m_trans_lilianggou = fastComponent.FastGetComponent<Transform>("lilianggou");
       if( null == m_trans_lilianggou )
       {
            Engine.Utility.Log.Error("m_trans_lilianggou 为空，请检查prefab是否缺乏组件");
       }
        m_trans_minjiegou = fastComponent.FastGetComponent<Transform>("minjiegou");
       if( null == m_trans_minjiegou )
       {
            Engine.Utility.Log.Error("m_trans_minjiegou 为空，请检查prefab是否缺乏组件");
       }
        m_trans_zhiligou = fastComponent.FastGetComponent<Transform>("zhiligou");
       if( null == m_trans_zhiligou )
       {
            Engine.Utility.Log.Error("m_trans_zhiligou 为空，请检查prefab是否缺乏组件");
       }
        m_trans_tizhigou = fastComponent.FastGetComponent<Transform>("tizhigou");
       if( null == m_trans_tizhigou )
       {
            Engine.Utility.Log.Error("m_trans_tizhigou 为空，请检查prefab是否缺乏组件");
       }
        m_trans_jingshengou = fastComponent.FastGetComponent<Transform>("jingshengou");
       if( null == m_trans_jingshengou )
       {
            Engine.Utility.Log.Error("m_trans_jingshengou 为空，请检查prefab是否缺乏组件");
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
        UIEventListener.Get(m_btn_btn_right.gameObject).onClick = _onClick_Btn_right_Btn;
    }

    void _onClick_Btn_Close_Btn(GameObject caster)
    {
        onClick_Btn_Close_Btn( caster );
    }

    void _onClick_Btn_right_Btn(GameObject caster)
    {
        onClick_Btn_right_Btn( caster );
    }


}
