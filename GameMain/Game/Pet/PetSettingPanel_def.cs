//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetSettingPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_btn_setting;

    Transform            m_trans_petGroup;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_btn_setting = fastComponent.FastGetComponent<UIButton>("btn_setting");
       if( null == m_btn_btn_setting )
       {
            Engine.Utility.Log.Error("m_btn_btn_setting 为空，请检查prefab是否缺乏组件");
       }
        m_trans_petGroup = fastComponent.FastGetComponent<Transform>("petGroup");
       if( null == m_trans_petGroup )
       {
            Engine.Utility.Log.Error("m_trans_petGroup 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_btn_setting.gameObject).onClick = _onClick_Btn_setting_Btn;
    }

    void _onClick_Btn_setting_Btn(GameObject caster)
    {
        onClick_Btn_setting_Btn( caster );
    }


}
