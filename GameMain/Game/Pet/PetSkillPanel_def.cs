//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class PetSkillPanel: UIPanelBase
{

    public enum BtnType{
		None = 0,
		Max,
    }

    UISprite      m_sprite_skill_icon;

    UILabel       m_label_skill_name;

    UILabel       m_label_skill_result;


    //初始化控件变量
    protected override void InitControls()
    {
        m_sprite_skill_icon = GetChildComponent<UISprite>("skill_icon");
       if( null == m_sprite_skill_icon )
       {
            Engine.Utility.Log.Error("m_sprite_skill_icon 为空，请检查prefab是否缺乏组件");
       }
        m_label_skill_name = GetChildComponent<UILabel>("skill_name");
       if( null == m_label_skill_name )
       {
            Engine.Utility.Log.Error("m_label_skill_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_skill_result = GetChildComponent<UILabel>("skill_result");
       if( null == m_label_skill_result )
       {
            Engine.Utility.Log.Error("m_label_skill_result 为空，请检查prefab是否缺乏组件");
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
