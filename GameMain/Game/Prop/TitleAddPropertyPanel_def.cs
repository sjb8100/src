//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class TitleAddPropertyPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_close;

    UILabel              m_label_TitleLabel;

    UILabel              m_label_allAddfightInfo;

    Transform            m_trans_foreverContent;

    Transform            m_trans_activateContent;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_close = fastComponent.FastGetComponent<UIButton>("btn_close");
       if( null == m_btn_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_label_TitleLabel = fastComponent.FastGetComponent<UILabel>("TitleLabel");
       if( null == m_label_TitleLabel )
       {
            Engine.Utility.Log.Error("m_label_TitleLabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_allAddfightInfo = fastComponent.FastGetComponent<UILabel>("allAddfightInfo");
       if( null == m_label_allAddfightInfo )
       {
            Engine.Utility.Log.Error("m_label_allAddfightInfo 为空，请检查prefab是否缺乏组件");
       }
        m_trans_foreverContent = fastComponent.FastGetComponent<Transform>("foreverContent");
       if( null == m_trans_foreverContent )
       {
            Engine.Utility.Log.Error("m_trans_foreverContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_activateContent = fastComponent.FastGetComponent<Transform>("activateContent");
       if( null == m_trans_activateContent )
       {
            Engine.Utility.Log.Error("m_trans_activateContent 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_close.gameObject).onClick = _onClick_Btn_close_Btn;
    }

    void _onClick_Btn_close_Btn(GameObject caster)
    {
        onClick_Btn_close_Btn( caster );
    }


}
