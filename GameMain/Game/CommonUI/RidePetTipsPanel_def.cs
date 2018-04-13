//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class RidePetTipsPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UITexture            m__icon;

    UISprite             m_sprite_qulity;

    UILabel              m_label_name;

    UILabel              m_label_level;

    UILabel              m_label_Label_2;

    UILabel              m_label_Label_3;

    UILabel              m_label_petGradeValue;

    UILabel              m_label_variableLevel;

    UILabel              m_label_petCharacter;

    UILabel              m_label_petYhLv;

    UILabel              m_label_petLift;

    UILabel              m_label_InheritingNumber;

    UIWidget             m_widget_talent;

    Transform            m_trans_talentlabels;

    UIWidget             m_widget_skil;

    Transform            m_trans_skilllabels;

    UISprite             m_sprite_bg;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m__icon = fastComponent.FastGetComponent<UITexture>("icon");
       if( null == m__icon )
       {
            Engine.Utility.Log.Error("m__icon 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_qulity = fastComponent.FastGetComponent<UISprite>("qulity");
       if( null == m_sprite_qulity )
       {
            Engine.Utility.Log.Error("m_sprite_qulity 为空，请检查prefab是否缺乏组件");
       }
        m_label_name = fastComponent.FastGetComponent<UILabel>("name");
       if( null == m_label_name )
       {
            Engine.Utility.Log.Error("m_label_name 为空，请检查prefab是否缺乏组件");
       }
        m_label_level = fastComponent.FastGetComponent<UILabel>("level");
       if( null == m_label_level )
       {
            Engine.Utility.Log.Error("m_label_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label_2 = fastComponent.FastGetComponent<UILabel>("Label_2");
       if( null == m_label_Label_2 )
       {
            Engine.Utility.Log.Error("m_label_Label_2 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label_3 = fastComponent.FastGetComponent<UILabel>("Label_3");
       if( null == m_label_Label_3 )
       {
            Engine.Utility.Log.Error("m_label_Label_3 为空，请检查prefab是否缺乏组件");
       }
        m_label_petGradeValue = fastComponent.FastGetComponent<UILabel>("petGradeValue");
       if( null == m_label_petGradeValue )
       {
            Engine.Utility.Log.Error("m_label_petGradeValue 为空，请检查prefab是否缺乏组件");
       }
        m_label_variableLevel = fastComponent.FastGetComponent<UILabel>("variableLevel");
       if( null == m_label_variableLevel )
       {
            Engine.Utility.Log.Error("m_label_variableLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_petCharacter = fastComponent.FastGetComponent<UILabel>("petCharacter");
       if( null == m_label_petCharacter )
       {
            Engine.Utility.Log.Error("m_label_petCharacter 为空，请检查prefab是否缺乏组件");
       }
        m_label_petYhLv = fastComponent.FastGetComponent<UILabel>("petYhLv");
       if( null == m_label_petYhLv )
       {
            Engine.Utility.Log.Error("m_label_petYhLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_petLift = fastComponent.FastGetComponent<UILabel>("petLift");
       if( null == m_label_petLift )
       {
            Engine.Utility.Log.Error("m_label_petLift 为空，请检查prefab是否缺乏组件");
       }
        m_label_InheritingNumber = fastComponent.FastGetComponent<UILabel>("InheritingNumber");
       if( null == m_label_InheritingNumber )
       {
            Engine.Utility.Log.Error("m_label_InheritingNumber 为空，请检查prefab是否缺乏组件");
       }
        m_widget_talent = fastComponent.FastGetComponent<UIWidget>("talent");
       if( null == m_widget_talent )
       {
            Engine.Utility.Log.Error("m_widget_talent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_talentlabels = fastComponent.FastGetComponent<Transform>("talentlabels");
       if( null == m_trans_talentlabels )
       {
            Engine.Utility.Log.Error("m_trans_talentlabels 为空，请检查prefab是否缺乏组件");
       }
        m_widget_skil = fastComponent.FastGetComponent<UIWidget>("skil");
       if( null == m_widget_skil )
       {
            Engine.Utility.Log.Error("m_widget_skil 为空，请检查prefab是否缺乏组件");
       }
        m_trans_skilllabels = fastComponent.FastGetComponent<Transform>("skilllabels");
       if( null == m_trans_skilllabels )
       {
            Engine.Utility.Log.Error("m_trans_skilllabels 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_bg = fastComponent.FastGetComponent<UISprite>("bg");
       if( null == m_sprite_bg )
       {
            Engine.Utility.Log.Error("m_sprite_bg 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
    }


}
