//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class SeedItem: UIGridBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UISprite      m_sprite_icon;

    UILabel       m_label_seedname;

    UILabel       m_label_ripetime;

    UILabel       m_label_lockwarring;


    //初始化控件变量
    protected override void OnAwake()
    {
         InitControls();
         RegisterControlEvents();
    }
    private void InitControls()
    {
        m_sprite_icon = GetChildComponent<UISprite>("icon");
       if( null == m_sprite_icon )
       {
            Engine.Utility.Log.Error("m_sprite_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_seedname = GetChildComponent<UILabel>("seedname");
       if( null == m_label_seedname )
       {
            Engine.Utility.Log.Error("m_label_seedname 为空，请检查prefab是否缺乏组件");
       }
        m_label_ripetime = GetChildComponent<UILabel>("ripetime");
       if( null == m_label_ripetime )
       {
            Engine.Utility.Log.Error("m_label_ripetime 为空，请检查prefab是否缺乏组件");
       }
        m_label_lockwarring = GetChildComponent<UILabel>("lockwarring");
       if( null == m_label_lockwarring )
       {
            Engine.Utility.Log.Error("m_label_lockwarring 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    private void RegisterControlEvents()
    {
    }


}
