//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class BuffItemInfo: UIGridBase
{

    UITexture            m__BuffIcon;

    UILabel              m_label_BuffName;

    UILabel              m_label_BuffTime;

    UILabel              m_label_BuffDes;


    //初始化控件变量
   protected override void OnAwake()
    {
         InitControls();
         RegisterControlEvents();
    }
    private void InitControls()
    {
        m__BuffIcon = GetChildComponent<UITexture>("BuffIcon");
       if( null == m__BuffIcon )
       {
            Engine.Utility.Log.Error("m__BuffIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_BuffName = GetChildComponent<UILabel>("BuffName");
       if( null == m_label_BuffName )
       {
            Engine.Utility.Log.Error("m_label_BuffName 为空，请检查prefab是否缺乏组件");
       }
        m_label_BuffTime = GetChildComponent<UILabel>("BuffTime");
       if( null == m_label_BuffTime )
       {
            Engine.Utility.Log.Error("m_label_BuffTime 为空，请检查prefab是否缺乏组件");
       }
        m_label_BuffDes = GetChildComponent<UILabel>("BuffDes");
       if( null == m_label_BuffDes )
       {
            Engine.Utility.Log.Error("m_label_BuffDes 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    private void RegisterControlEvents()
    {
    }


}
