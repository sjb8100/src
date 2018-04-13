//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ChatItem: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    Transform     m_trans_ChatItem;


    //初始化控件变量
    protected override void InitControls()
    {
        m_trans_ChatItem = GetChildComponent<Transform>("ChatItem");
       if( null == m_trans_ChatItem )
       {
            Engine.Utility.Log.Error("m_trans_ChatItem 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
