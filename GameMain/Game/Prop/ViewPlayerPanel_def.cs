//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ViewPlayerPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//人物
		RenWu = 1,
		//珍兽
		ZhanHun = 2,
		//坐骑
		ZuoQi = 3,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_PlayerContent;

    UITexture            m__modelTexture;

    UIWidget             m_widget_Equipment;

    Transform            m_trans_EquipmentGridRoot;

    UISpriteEx           m_spriteEx_equipfashionbtn;

    UIWidget             m_widget_Fashion;

    UILabel              m_label_powerLabel;

    Transform            m_trans_propRoot;

    UILabel              m_label_ActiveGridSuitLv;

    UIButton             m_btn_BtnGridSuitNormal;

    UIButton             m_btn_BtnGridSuitActive;

    UILabel              m_label_ActiveColorSuitLv;

    UIButton             m_btn_BtnColorSuitNormal;

    UIButton             m_btn_BtnColorSuitActive;

    UILabel              m_label_ActiveStoneSuitLv;

    UIButton             m_btn_BtnStoneSuitNormal;

    UIButton             m_btn_BtnStoneSuitActive;

    Transform            m_trans_RideContent;

    UIGridCreatorBase    m_ctor_Ridescrollview;

    UISprite             m_sprite_RidePropContent;

    UITexture            m__model_bg;

    UISprite             m_sprite_line;

    UITexture            m__rideModel;

    UILabel              m_label_speed;

    UILabel              m_label_level;

    UILabel              m_label_Ride_Name;

    UIGridCreatorBase    m_ctor_RideSkill;

    Transform            m_trans_PetContent;

    UIGridCreatorBase    m_ctor_petscrollview;

    Transform            m_trans_PetPropRoot;

    UILabel              m_label_typeName;

    UITexture            m__PetModel;

    UILabel              m_label_fightingLabel;

    UILabel              m_label_petshowname;

    UIGridCreatorBase    m_ctor_SkillRoot;

    Transform            m_trans_UIPetRideGrid;

    Transform            m_trans_UISkillGrid;

    Transform            m_trans_UIEquipGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_PlayerContent = fastComponent.FastGetComponent<Transform>("PlayerContent");
       if( null == m_trans_PlayerContent )
       {
            Engine.Utility.Log.Error("m_trans_PlayerContent 为空，请检查prefab是否缺乏组件");
       }
        m__modelTexture = fastComponent.FastGetComponent<UITexture>("modelTexture");
       if( null == m__modelTexture )
       {
            Engine.Utility.Log.Error("m__modelTexture 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Equipment = fastComponent.FastGetComponent<UIWidget>("Equipment");
       if( null == m_widget_Equipment )
       {
            Engine.Utility.Log.Error("m_widget_Equipment 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EquipmentGridRoot = fastComponent.FastGetComponent<Transform>("EquipmentGridRoot");
       if( null == m_trans_EquipmentGridRoot )
       {
            Engine.Utility.Log.Error("m_trans_EquipmentGridRoot 为空，请检查prefab是否缺乏组件");
       }
        m_spriteEx_equipfashionbtn = fastComponent.FastGetComponent<UISpriteEx>("equipfashionbtn");
       if( null == m_spriteEx_equipfashionbtn )
       {
            Engine.Utility.Log.Error("m_spriteEx_equipfashionbtn 为空，请检查prefab是否缺乏组件");
       }
        m_widget_Fashion = fastComponent.FastGetComponent<UIWidget>("Fashion");
       if( null == m_widget_Fashion )
       {
            Engine.Utility.Log.Error("m_widget_Fashion 为空，请检查prefab是否缺乏组件");
       }
        m_label_powerLabel = fastComponent.FastGetComponent<UILabel>("powerLabel");
       if( null == m_label_powerLabel )
       {
            Engine.Utility.Log.Error("m_label_powerLabel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_propRoot = fastComponent.FastGetComponent<Transform>("propRoot");
       if( null == m_trans_propRoot )
       {
            Engine.Utility.Log.Error("m_trans_propRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveGridSuitLv = fastComponent.FastGetComponent<UILabel>("ActiveGridSuitLv");
       if( null == m_label_ActiveGridSuitLv )
       {
            Engine.Utility.Log.Error("m_label_ActiveGridSuitLv 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnGridSuitNormal = fastComponent.FastGetComponent<UIButton>("BtnGridSuitNormal");
       if( null == m_btn_BtnGridSuitNormal )
       {
            Engine.Utility.Log.Error("m_btn_BtnGridSuitNormal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnGridSuitActive = fastComponent.FastGetComponent<UIButton>("BtnGridSuitActive");
       if( null == m_btn_BtnGridSuitActive )
       {
            Engine.Utility.Log.Error("m_btn_BtnGridSuitActive 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveColorSuitLv = fastComponent.FastGetComponent<UILabel>("ActiveColorSuitLv");
       if( null == m_label_ActiveColorSuitLv )
       {
            Engine.Utility.Log.Error("m_label_ActiveColorSuitLv 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnColorSuitNormal = fastComponent.FastGetComponent<UIButton>("BtnColorSuitNormal");
       if( null == m_btn_BtnColorSuitNormal )
       {
            Engine.Utility.Log.Error("m_btn_BtnColorSuitNormal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnColorSuitActive = fastComponent.FastGetComponent<UIButton>("BtnColorSuitActive");
       if( null == m_btn_BtnColorSuitActive )
       {
            Engine.Utility.Log.Error("m_btn_BtnColorSuitActive 为空，请检查prefab是否缺乏组件");
       }
        m_label_ActiveStoneSuitLv = fastComponent.FastGetComponent<UILabel>("ActiveStoneSuitLv");
       if( null == m_label_ActiveStoneSuitLv )
       {
            Engine.Utility.Log.Error("m_label_ActiveStoneSuitLv 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnStoneSuitNormal = fastComponent.FastGetComponent<UIButton>("BtnStoneSuitNormal");
       if( null == m_btn_BtnStoneSuitNormal )
       {
            Engine.Utility.Log.Error("m_btn_BtnStoneSuitNormal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnStoneSuitActive = fastComponent.FastGetComponent<UIButton>("BtnStoneSuitActive");
       if( null == m_btn_BtnStoneSuitActive )
       {
            Engine.Utility.Log.Error("m_btn_BtnStoneSuitActive 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RideContent = fastComponent.FastGetComponent<Transform>("RideContent");
       if( null == m_trans_RideContent )
       {
            Engine.Utility.Log.Error("m_trans_RideContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_Ridescrollview = fastComponent.FastGetComponent<UIGridCreatorBase>("Ridescrollview");
       if( null == m_ctor_Ridescrollview )
       {
            Engine.Utility.Log.Error("m_ctor_Ridescrollview 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_RidePropContent = fastComponent.FastGetComponent<UISprite>("RidePropContent");
       if( null == m_sprite_RidePropContent )
       {
            Engine.Utility.Log.Error("m_sprite_RidePropContent 为空，请检查prefab是否缺乏组件");
       }
        m__model_bg = fastComponent.FastGetComponent<UITexture>("model_bg");
       if( null == m__model_bg )
       {
            Engine.Utility.Log.Error("m__model_bg 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_line = fastComponent.FastGetComponent<UISprite>("line");
       if( null == m_sprite_line )
       {
            Engine.Utility.Log.Error("m_sprite_line 为空，请检查prefab是否缺乏组件");
       }
        m__rideModel = fastComponent.FastGetComponent<UITexture>("rideModel");
       if( null == m__rideModel )
       {
            Engine.Utility.Log.Error("m__rideModel 为空，请检查prefab是否缺乏组件");
       }
        m_label_speed = fastComponent.FastGetComponent<UILabel>("speed");
       if( null == m_label_speed )
       {
            Engine.Utility.Log.Error("m_label_speed 为空，请检查prefab是否缺乏组件");
       }
        m_label_level = fastComponent.FastGetComponent<UILabel>("level");
       if( null == m_label_level )
       {
            Engine.Utility.Log.Error("m_label_level 为空，请检查prefab是否缺乏组件");
       }
        m_label_Ride_Name = fastComponent.FastGetComponent<UILabel>("Ride_Name");
       if( null == m_label_Ride_Name )
       {
            Engine.Utility.Log.Error("m_label_Ride_Name 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_RideSkill = fastComponent.FastGetComponent<UIGridCreatorBase>("RideSkill");
       if( null == m_ctor_RideSkill )
       {
            Engine.Utility.Log.Error("m_ctor_RideSkill 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PetContent = fastComponent.FastGetComponent<Transform>("PetContent");
       if( null == m_trans_PetContent )
       {
            Engine.Utility.Log.Error("m_trans_PetContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_petscrollview = fastComponent.FastGetComponent<UIGridCreatorBase>("petscrollview");
       if( null == m_ctor_petscrollview )
       {
            Engine.Utility.Log.Error("m_ctor_petscrollview 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PetPropRoot = fastComponent.FastGetComponent<Transform>("PetPropRoot");
       if( null == m_trans_PetPropRoot )
       {
            Engine.Utility.Log.Error("m_trans_PetPropRoot 为空，请检查prefab是否缺乏组件");
       }
        m_label_typeName = fastComponent.FastGetComponent<UILabel>("typeName");
       if( null == m_label_typeName )
       {
            Engine.Utility.Log.Error("m_label_typeName 为空，请检查prefab是否缺乏组件");
       }
        m__PetModel = fastComponent.FastGetComponent<UITexture>("PetModel");
       if( null == m__PetModel )
       {
            Engine.Utility.Log.Error("m__PetModel 为空，请检查prefab是否缺乏组件");
       }
        m_label_fightingLabel = fastComponent.FastGetComponent<UILabel>("fightingLabel");
       if( null == m_label_fightingLabel )
       {
            Engine.Utility.Log.Error("m_label_fightingLabel 为空，请检查prefab是否缺乏组件");
       }
        m_label_petshowname = fastComponent.FastGetComponent<UILabel>("petshowname");
       if( null == m_label_petshowname )
       {
            Engine.Utility.Log.Error("m_label_petshowname 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_SkillRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("SkillRoot");
       if( null == m_ctor_SkillRoot )
       {
            Engine.Utility.Log.Error("m_ctor_SkillRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIPetRideGrid = fastComponent.FastGetComponent<Transform>("UIPetRideGrid");
       if( null == m_trans_UIPetRideGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIPetRideGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UISkillGrid = fastComponent.FastGetComponent<Transform>("UISkillGrid");
       if( null == m_trans_UISkillGrid )
       {
            Engine.Utility.Log.Error("m_trans_UISkillGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIEquipGrid = fastComponent.FastGetComponent<Transform>("UIEquipGrid");
       if( null == m_trans_UIEquipGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIEquipGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BtnGridSuitNormal.gameObject).onClick = _onClick_BtnGridSuitNormal_Btn;
        UIEventListener.Get(m_btn_BtnGridSuitActive.gameObject).onClick = _onClick_BtnGridSuitActive_Btn;
        UIEventListener.Get(m_btn_BtnColorSuitNormal.gameObject).onClick = _onClick_BtnColorSuitNormal_Btn;
        UIEventListener.Get(m_btn_BtnColorSuitActive.gameObject).onClick = _onClick_BtnColorSuitActive_Btn;
        UIEventListener.Get(m_btn_BtnStoneSuitNormal.gameObject).onClick = _onClick_BtnStoneSuitNormal_Btn;
        UIEventListener.Get(m_btn_BtnStoneSuitActive.gameObject).onClick = _onClick_BtnStoneSuitActive_Btn;
    }

    void _onClick_BtnGridSuitNormal_Btn(GameObject caster)
    {
        onClick_BtnGridSuitNormal_Btn( caster );
    }

    void _onClick_BtnGridSuitActive_Btn(GameObject caster)
    {
        onClick_BtnGridSuitActive_Btn( caster );
    }

    void _onClick_BtnColorSuitNormal_Btn(GameObject caster)
    {
        onClick_BtnColorSuitNormal_Btn( caster );
    }

    void _onClick_BtnColorSuitActive_Btn(GameObject caster)
    {
        onClick_BtnColorSuitActive_Btn( caster );
    }

    void _onClick_BtnStoneSuitNormal_Btn(GameObject caster)
    {
        onClick_BtnStoneSuitNormal_Btn( caster );
    }

    void _onClick_BtnStoneSuitActive_Btn(GameObject caster)
    {
        onClick_BtnStoneSuitActive_Btn( caster );
    }


}
