//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ChatItem_system: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    Transform     m_trans_ChatItem_system;


    //初始化控件变量
    protected override void InitControls()
    {
        m_trans_ChatItem_system = GetChildComponent<Transform>("ChatItem_system");
       if( null == m_trans_ChatItem_system )
       {
            Engine.Utility.Log.Error("m_trans_ChatItem_system 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
