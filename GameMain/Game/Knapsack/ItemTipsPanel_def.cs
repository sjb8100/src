//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ItemTipsPanel: UIPanelBase
{

   FastComponent         fastComponent;

    Transform            m_trans_ItemTipsContent;

    Transform            m_trans_TipsContent;

    UITexture            m__ItemGradeBgNormal;

    UISprite             m_sprite_Short;

    UISprite             m_sprite_Short_Box;

    UISprite             m_sprite_Normal;

    UISprite             m_sprite_Normal_Box;

    UIScrollView         m_scrollview_InfoLinesScrollView;

    UIWidget             m_widget_DragWidget;

    Transform            m_trans_BaseAttrContent;

    Transform            m_trans_AdditiveAttrContent;

    Transform            m_trans_AdvancedAttrContent;

    Transform            m_trans_GemAttrContent;

    Transform            m_trans_PartEquip;

    UILabel              m_label_EquipPart;

    UILabel              m_label_EquipGrade;

    UILabel              m_label_EquipPro;

    UILabel              m_label_EquipDur;

    Transform            m_trans_PartMuhon;

    UILabel              m_label_MuhonGrow;

    UILabel              m_label_MuhonCurLv;

    Transform            m_trans_PartSkill;

    UILabel              m_label_SkillDes;

    UILabel              m_label_SkillUnlock;

    Transform            m_trans_PartMaterial;

    UILabel              m_label_MaterialDes;

    UILabel              m_label_MaterialGrade;

    UILabel              m_label_MaterialNeedLevel;

    Transform            m_trans_PartConsumption;

    UILabel              m_label_ConsumptionNeedLevel;

    UILabel              m_label_ConsumptionGrade;

    UILabel              m_label_ConsumptionDes;

    UILabel              m_label_ConsumptionUseLeft;

    Transform            m_trans_PartMounts;

    UILabel              m_label_MountsSpeedAdd;

    UILabel              m_label_MountsLife;

    UILabel              m_label_MountsSkil;

    UILabel              m_label_MountsDes;

    Transform            m_trans_PartMuhonMountsEgg;

    UIWidget             m_widget_skil;

    Transform            m_trans_skilllabels;

    UIWidget             m_widget_talent;

    Transform            m_trans_talentlabels;

    UILabel              m_label_petGradeValue;

    UILabel              m_label_variableLevel;

    UILabel              m_label_petCharacter;

    UILabel              m_label_petYhLv;

    UILabel              m_label_petLift;

    UILabel              m_label_InheritingNumber;

    Transform            m_trans_FuctionBtns;

    UISprite             m_sprite_FNBGShort;

    UISprite             m_sprite_FNBGNormal;

    Transform            m_trans_TopPart;

    UILabel              m_label_ItemName;

    Transform            m_trans_IconContent;

    UILabel              m_label_StrengthenLv;

    Transform            m_trans_Consumption;

    UILabel              m_label_ConsumptionOwn;

    Transform            m_trans_Material;

    UILabel              m_label_MaterialOwn;

    Transform            m_trans_Muhon;

    UILabel              m_label_MuhonNeedLevel;

    UILabel              m_label_MuhonFightingPower;

    Transform            m_trans_MuhonEquipMask;

    Transform            m_trans_Mounts;

    UILabel              m_label_MountGrade;

    UILabel              m_label_MountLevel;

    Transform            m_trans_Equip;

    UILabel              m_label_EquipNeedLevel;

    UILabel              m_label_EquipFightingPower;

    Transform            m_trans_EquipMask;

    Transform            m_trans_PowerContent;

    UISprite             m_sprite_PowerPromote;

    UISprite             m_sprite_PowerFallOff;

    Transform            m_trans_MountsEgg;

    UILabel              m_label_MountEggQuality;

    UILabel              m_label_MountEggLevel;

    Transform            m_trans_MuhonEgg;

    UILabel              m_label_MuhonEggNeedLevel;

    UILabel              m_label_MuhonEggAttackType;

    UILabel              m_label_MuhonEggLevel;

    Transform            m_trans_TipsContentCompare;

    UIButton             m_btn_SwitchCompare;

    UITexture            m__ItemGradeBgCompare;

    UISprite             m_sprite_NormalCompare;

    UISprite             m_sprite_NormalCompare_Box;

    UIScrollView         m_scrollview_InfoLinesScrollViewCp;

    UIWidget             m_widget_DragWidgetCp;

    Transform            m_trans_AdvancedAttrContentCp;

    Transform            m_trans_BaseAttrContentCp;

    Transform            m_trans_GemAttrContentCp;

    Transform            m_trans_AdditiveAttrContentCp;

    Transform            m_trans_PartEquipCp;

    UILabel              m_label_EquipPartCp;

    UILabel              m_label_EquipGradeCp;

    UILabel              m_label_EquipProCp;

    UILabel              m_label_EquipDurCp;

    Transform            m_trans_PartMuhonCp;

    UILabel              m_label_MuhonGrowCp;

    UILabel              m_label_MuhonCurLvCp;

    Transform            m_trans_TopPartCp;

    Transform            m_trans_PowerContentCp;

    UISprite             m_sprite_PowerPromoteCp;

    UISprite             m_sprite_PowerFallOffCp;

    UILabel              m_label_ItemNameCp;

    Transform            m_trans_IconContentCompare;

    UILabel              m_label_StrengthenLvCp;

    Transform            m_trans_MuhonCp;

    Transform            m_trans_MuhonEquipMaskCp;

    UILabel              m_label_MuhonNeedLevelCp;

    UILabel              m_label_MuhonFightingPowerCp;

    Transform            m_trans_EquipCp;

    UILabel              m_label_EquipNeedLevelCp;

    UILabel              m_label_EquipFightingPowerCp;

    Transform            m_trans_EquipMaskCp;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_ItemTipsContent = fastComponent.FastGetComponent<Transform>("ItemTipsContent");
       if( null == m_trans_ItemTipsContent )
       {
            Engine.Utility.Log.Error("m_trans_ItemTipsContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TipsContent = fastComponent.FastGetComponent<Transform>("TipsContent");
       if( null == m_trans_TipsContent )
       {
            Engine.Utility.Log.Error("m_trans_TipsContent 为空，请检查prefab是否缺乏组件");
       }
        m__ItemGradeBgNormal = fastComponent.FastGetComponent<UITexture>("ItemGradeBgNormal");
       if( null == m__ItemGradeBgNormal )
       {
            Engine.Utility.Log.Error("m__ItemGradeBgNormal 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Short = fastComponent.FastGetComponent<UISprite>("Short");
       if( null == m_sprite_Short )
       {
            Engine.Utility.Log.Error("m_sprite_Short 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Short_Box = fastComponent.FastGetComponent<UISprite>("Short_Box");
       if( null == m_sprite_Short_Box )
       {
            Engine.Utility.Log.Error("m_sprite_Short_Box 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Normal = fastComponent.FastGetComponent<UISprite>("Normal");
       if( null == m_sprite_Normal )
       {
            Engine.Utility.Log.Error("m_sprite_Normal 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_Normal_Box = fastComponent.FastGetComponent<UISprite>("Normal_Box");
       if( null == m_sprite_Normal_Box )
       {
            Engine.Utility.Log.Error("m_sprite_Normal_Box 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_InfoLinesScrollView = fastComponent.FastGetComponent<UIScrollView>("InfoLinesScrollView");
       if( null == m_scrollview_InfoLinesScrollView )
       {
            Engine.Utility.Log.Error("m_scrollview_InfoLinesScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_widget_DragWidget = fastComponent.FastGetComponent<UIWidget>("DragWidget");
       if( null == m_widget_DragWidget )
       {
            Engine.Utility.Log.Error("m_widget_DragWidget 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BaseAttrContent = fastComponent.FastGetComponent<Transform>("BaseAttrContent");
       if( null == m_trans_BaseAttrContent )
       {
            Engine.Utility.Log.Error("m_trans_BaseAttrContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AdditiveAttrContent = fastComponent.FastGetComponent<Transform>("AdditiveAttrContent");
       if( null == m_trans_AdditiveAttrContent )
       {
            Engine.Utility.Log.Error("m_trans_AdditiveAttrContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AdvancedAttrContent = fastComponent.FastGetComponent<Transform>("AdvancedAttrContent");
       if( null == m_trans_AdvancedAttrContent )
       {
            Engine.Utility.Log.Error("m_trans_AdvancedAttrContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GemAttrContent = fastComponent.FastGetComponent<Transform>("GemAttrContent");
       if( null == m_trans_GemAttrContent )
       {
            Engine.Utility.Log.Error("m_trans_GemAttrContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartEquip = fastComponent.FastGetComponent<Transform>("PartEquip");
       if( null == m_trans_PartEquip )
       {
            Engine.Utility.Log.Error("m_trans_PartEquip 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipPart = fastComponent.FastGetComponent<UILabel>("EquipPart");
       if( null == m_label_EquipPart )
       {
            Engine.Utility.Log.Error("m_label_EquipPart 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipGrade = fastComponent.FastGetComponent<UILabel>("EquipGrade");
       if( null == m_label_EquipGrade )
       {
            Engine.Utility.Log.Error("m_label_EquipGrade 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipPro = fastComponent.FastGetComponent<UILabel>("EquipPro");
       if( null == m_label_EquipPro )
       {
            Engine.Utility.Log.Error("m_label_EquipPro 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipDur = fastComponent.FastGetComponent<UILabel>("EquipDur");
       if( null == m_label_EquipDur )
       {
            Engine.Utility.Log.Error("m_label_EquipDur 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartMuhon = fastComponent.FastGetComponent<Transform>("PartMuhon");
       if( null == m_trans_PartMuhon )
       {
            Engine.Utility.Log.Error("m_trans_PartMuhon 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonGrow = fastComponent.FastGetComponent<UILabel>("MuhonGrow");
       if( null == m_label_MuhonGrow )
       {
            Engine.Utility.Log.Error("m_label_MuhonGrow 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonCurLv = fastComponent.FastGetComponent<UILabel>("MuhonCurLv");
       if( null == m_label_MuhonCurLv )
       {
            Engine.Utility.Log.Error("m_label_MuhonCurLv 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartSkill = fastComponent.FastGetComponent<Transform>("PartSkill");
       if( null == m_trans_PartSkill )
       {
            Engine.Utility.Log.Error("m_trans_PartSkill 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillDes = fastComponent.FastGetComponent<UILabel>("SkillDes");
       if( null == m_label_SkillDes )
       {
            Engine.Utility.Log.Error("m_label_SkillDes 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillUnlock = fastComponent.FastGetComponent<UILabel>("SkillUnlock");
       if( null == m_label_SkillUnlock )
       {
            Engine.Utility.Log.Error("m_label_SkillUnlock 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartMaterial = fastComponent.FastGetComponent<Transform>("PartMaterial");
       if( null == m_trans_PartMaterial )
       {
            Engine.Utility.Log.Error("m_trans_PartMaterial 为空，请检查prefab是否缺乏组件");
       }
        m_label_MaterialDes = fastComponent.FastGetComponent<UILabel>("MaterialDes");
       if( null == m_label_MaterialDes )
       {
            Engine.Utility.Log.Error("m_label_MaterialDes 为空，请检查prefab是否缺乏组件");
       }
        m_label_MaterialGrade = fastComponent.FastGetComponent<UILabel>("MaterialGrade");
       if( null == m_label_MaterialGrade )
       {
            Engine.Utility.Log.Error("m_label_MaterialGrade 为空，请检查prefab是否缺乏组件");
       }
        m_label_MaterialNeedLevel = fastComponent.FastGetComponent<UILabel>("MaterialNeedLevel");
       if( null == m_label_MaterialNeedLevel )
       {
            Engine.Utility.Log.Error("m_label_MaterialNeedLevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartConsumption = fastComponent.FastGetComponent<Transform>("PartConsumption");
       if( null == m_trans_PartConsumption )
       {
            Engine.Utility.Log.Error("m_trans_PartConsumption 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConsumptionNeedLevel = fastComponent.FastGetComponent<UILabel>("ConsumptionNeedLevel");
       if( null == m_label_ConsumptionNeedLevel )
       {
            Engine.Utility.Log.Error("m_label_ConsumptionNeedLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConsumptionGrade = fastComponent.FastGetComponent<UILabel>("ConsumptionGrade");
       if( null == m_label_ConsumptionGrade )
       {
            Engine.Utility.Log.Error("m_label_ConsumptionGrade 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConsumptionDes = fastComponent.FastGetComponent<UILabel>("ConsumptionDes");
       if( null == m_label_ConsumptionDes )
       {
            Engine.Utility.Log.Error("m_label_ConsumptionDes 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConsumptionUseLeft = fastComponent.FastGetComponent<UILabel>("ConsumptionUseLeft");
       if( null == m_label_ConsumptionUseLeft )
       {
            Engine.Utility.Log.Error("m_label_ConsumptionUseLeft 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartMounts = fastComponent.FastGetComponent<Transform>("PartMounts");
       if( null == m_trans_PartMounts )
       {
            Engine.Utility.Log.Error("m_trans_PartMounts 为空，请检查prefab是否缺乏组件");
       }
        m_label_MountsSpeedAdd = fastComponent.FastGetComponent<UILabel>("MountsSpeedAdd");
       if( null == m_label_MountsSpeedAdd )
       {
            Engine.Utility.Log.Error("m_label_MountsSpeedAdd 为空，请检查prefab是否缺乏组件");
       }
        m_label_MountsLife = fastComponent.FastGetComponent<UILabel>("MountsLife");
       if( null == m_label_MountsLife )
       {
            Engine.Utility.Log.Error("m_label_MountsLife 为空，请检查prefab是否缺乏组件");
       }
        m_label_MountsSkil = fastComponent.FastGetComponent<UILabel>("MountsSkil");
       if( null == m_label_MountsSkil )
       {
            Engine.Utility.Log.Error("m_label_MountsSkil 为空，请检查prefab是否缺乏组件");
       }
        m_label_MountsDes = fastComponent.FastGetComponent<UILabel>("MountsDes");
       if( null == m_label_MountsDes )
       {
            Engine.Utility.Log.Error("m_label_MountsDes 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartMuhonMountsEgg = fastComponent.FastGetComponent<Transform>("PartMuhonMountsEgg");
       if( null == m_trans_PartMuhonMountsEgg )
       {
            Engine.Utility.Log.Error("m_trans_PartMuhonMountsEgg 为空，请检查prefab是否缺乏组件");
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
        m_trans_FuctionBtns = fastComponent.FastGetComponent<Transform>("FuctionBtns");
       if( null == m_trans_FuctionBtns )
       {
            Engine.Utility.Log.Error("m_trans_FuctionBtns 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_FNBGShort = fastComponent.FastGetComponent<UISprite>("FNBGShort");
       if( null == m_sprite_FNBGShort )
       {
            Engine.Utility.Log.Error("m_sprite_FNBGShort 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_FNBGNormal = fastComponent.FastGetComponent<UISprite>("FNBGNormal");
       if( null == m_sprite_FNBGNormal )
       {
            Engine.Utility.Log.Error("m_sprite_FNBGNormal 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TopPart = fastComponent.FastGetComponent<Transform>("TopPart");
       if( null == m_trans_TopPart )
       {
            Engine.Utility.Log.Error("m_trans_TopPart 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemName = fastComponent.FastGetComponent<UILabel>("ItemName");
       if( null == m_label_ItemName )
       {
            Engine.Utility.Log.Error("m_label_ItemName 为空，请检查prefab是否缺乏组件");
       }
        m_trans_IconContent = fastComponent.FastGetComponent<Transform>("IconContent");
       if( null == m_trans_IconContent )
       {
            Engine.Utility.Log.Error("m_trans_IconContent 为空，请检查prefab是否缺乏组件");
       }
        m_label_StrengthenLv = fastComponent.FastGetComponent<UILabel>("StrengthenLv");
       if( null == m_label_StrengthenLv )
       {
            Engine.Utility.Log.Error("m_label_StrengthenLv 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Consumption = fastComponent.FastGetComponent<Transform>("Consumption");
       if( null == m_trans_Consumption )
       {
            Engine.Utility.Log.Error("m_trans_Consumption 为空，请检查prefab是否缺乏组件");
       }
        m_label_ConsumptionOwn = fastComponent.FastGetComponent<UILabel>("ConsumptionOwn");
       if( null == m_label_ConsumptionOwn )
       {
            Engine.Utility.Log.Error("m_label_ConsumptionOwn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Material = fastComponent.FastGetComponent<Transform>("Material");
       if( null == m_trans_Material )
       {
            Engine.Utility.Log.Error("m_trans_Material 为空，请检查prefab是否缺乏组件");
       }
        m_label_MaterialOwn = fastComponent.FastGetComponent<UILabel>("MaterialOwn");
       if( null == m_label_MaterialOwn )
       {
            Engine.Utility.Log.Error("m_label_MaterialOwn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Muhon = fastComponent.FastGetComponent<Transform>("Muhon");
       if( null == m_trans_Muhon )
       {
            Engine.Utility.Log.Error("m_trans_Muhon 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonNeedLevel = fastComponent.FastGetComponent<UILabel>("MuhonNeedLevel");
       if( null == m_label_MuhonNeedLevel )
       {
            Engine.Utility.Log.Error("m_label_MuhonNeedLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonFightingPower = fastComponent.FastGetComponent<UILabel>("MuhonFightingPower");
       if( null == m_label_MuhonFightingPower )
       {
            Engine.Utility.Log.Error("m_label_MuhonFightingPower 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MuhonEquipMask = fastComponent.FastGetComponent<Transform>("MuhonEquipMask");
       if( null == m_trans_MuhonEquipMask )
       {
            Engine.Utility.Log.Error("m_trans_MuhonEquipMask 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Mounts = fastComponent.FastGetComponent<Transform>("Mounts");
       if( null == m_trans_Mounts )
       {
            Engine.Utility.Log.Error("m_trans_Mounts 为空，请检查prefab是否缺乏组件");
       }
        m_label_MountGrade = fastComponent.FastGetComponent<UILabel>("MountGrade");
       if( null == m_label_MountGrade )
       {
            Engine.Utility.Log.Error("m_label_MountGrade 为空，请检查prefab是否缺乏组件");
       }
        m_label_MountLevel = fastComponent.FastGetComponent<UILabel>("MountLevel");
       if( null == m_label_MountLevel )
       {
            Engine.Utility.Log.Error("m_label_MountLevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Equip = fastComponent.FastGetComponent<Transform>("Equip");
       if( null == m_trans_Equip )
       {
            Engine.Utility.Log.Error("m_trans_Equip 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipNeedLevel = fastComponent.FastGetComponent<UILabel>("EquipNeedLevel");
       if( null == m_label_EquipNeedLevel )
       {
            Engine.Utility.Log.Error("m_label_EquipNeedLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipFightingPower = fastComponent.FastGetComponent<UILabel>("EquipFightingPower");
       if( null == m_label_EquipFightingPower )
       {
            Engine.Utility.Log.Error("m_label_EquipFightingPower 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EquipMask = fastComponent.FastGetComponent<Transform>("EquipMask");
       if( null == m_trans_EquipMask )
       {
            Engine.Utility.Log.Error("m_trans_EquipMask 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PowerContent = fastComponent.FastGetComponent<Transform>("PowerContent");
       if( null == m_trans_PowerContent )
       {
            Engine.Utility.Log.Error("m_trans_PowerContent 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_PowerPromote = fastComponent.FastGetComponent<UISprite>("PowerPromote");
       if( null == m_sprite_PowerPromote )
       {
            Engine.Utility.Log.Error("m_sprite_PowerPromote 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_PowerFallOff = fastComponent.FastGetComponent<UISprite>("PowerFallOff");
       if( null == m_sprite_PowerFallOff )
       {
            Engine.Utility.Log.Error("m_sprite_PowerFallOff 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MountsEgg = fastComponent.FastGetComponent<Transform>("MountsEgg");
       if( null == m_trans_MountsEgg )
       {
            Engine.Utility.Log.Error("m_trans_MountsEgg 为空，请检查prefab是否缺乏组件");
       }
        m_label_MountEggQuality = fastComponent.FastGetComponent<UILabel>("MountEggQuality");
       if( null == m_label_MountEggQuality )
       {
            Engine.Utility.Log.Error("m_label_MountEggQuality 为空，请检查prefab是否缺乏组件");
       }
        m_label_MountEggLevel = fastComponent.FastGetComponent<UILabel>("MountEggLevel");
       if( null == m_label_MountEggLevel )
       {
            Engine.Utility.Log.Error("m_label_MountEggLevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MuhonEgg = fastComponent.FastGetComponent<Transform>("MuhonEgg");
       if( null == m_trans_MuhonEgg )
       {
            Engine.Utility.Log.Error("m_trans_MuhonEgg 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonEggNeedLevel = fastComponent.FastGetComponent<UILabel>("MuhonEggNeedLevel");
       if( null == m_label_MuhonEggNeedLevel )
       {
            Engine.Utility.Log.Error("m_label_MuhonEggNeedLevel 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonEggAttackType = fastComponent.FastGetComponent<UILabel>("MuhonEggAttackType");
       if( null == m_label_MuhonEggAttackType )
       {
            Engine.Utility.Log.Error("m_label_MuhonEggAttackType 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonEggLevel = fastComponent.FastGetComponent<UILabel>("MuhonEggLevel");
       if( null == m_label_MuhonEggLevel )
       {
            Engine.Utility.Log.Error("m_label_MuhonEggLevel 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TipsContentCompare = fastComponent.FastGetComponent<Transform>("TipsContentCompare");
       if( null == m_trans_TipsContentCompare )
       {
            Engine.Utility.Log.Error("m_trans_TipsContentCompare 为空，请检查prefab是否缺乏组件");
       }
        m_btn_SwitchCompare = fastComponent.FastGetComponent<UIButton>("SwitchCompare");
       if( null == m_btn_SwitchCompare )
       {
            Engine.Utility.Log.Error("m_btn_SwitchCompare 为空，请检查prefab是否缺乏组件");
       }
        m__ItemGradeBgCompare = fastComponent.FastGetComponent<UITexture>("ItemGradeBgCompare");
       if( null == m__ItemGradeBgCompare )
       {
            Engine.Utility.Log.Error("m__ItemGradeBgCompare 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_NormalCompare = fastComponent.FastGetComponent<UISprite>("NormalCompare");
       if( null == m_sprite_NormalCompare )
       {
            Engine.Utility.Log.Error("m_sprite_NormalCompare 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_NormalCompare_Box = fastComponent.FastGetComponent<UISprite>("NormalCompare_Box");
       if( null == m_sprite_NormalCompare_Box )
       {
            Engine.Utility.Log.Error("m_sprite_NormalCompare_Box 为空，请检查prefab是否缺乏组件");
       }
        m_scrollview_InfoLinesScrollViewCp = fastComponent.FastGetComponent<UIScrollView>("InfoLinesScrollViewCp");
       if( null == m_scrollview_InfoLinesScrollViewCp )
       {
            Engine.Utility.Log.Error("m_scrollview_InfoLinesScrollViewCp 为空，请检查prefab是否缺乏组件");
       }
        m_widget_DragWidgetCp = fastComponent.FastGetComponent<UIWidget>("DragWidgetCp");
       if( null == m_widget_DragWidgetCp )
       {
            Engine.Utility.Log.Error("m_widget_DragWidgetCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AdvancedAttrContentCp = fastComponent.FastGetComponent<Transform>("AdvancedAttrContentCp");
       if( null == m_trans_AdvancedAttrContentCp )
       {
            Engine.Utility.Log.Error("m_trans_AdvancedAttrContentCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_BaseAttrContentCp = fastComponent.FastGetComponent<Transform>("BaseAttrContentCp");
       if( null == m_trans_BaseAttrContentCp )
       {
            Engine.Utility.Log.Error("m_trans_BaseAttrContentCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GemAttrContentCp = fastComponent.FastGetComponent<Transform>("GemAttrContentCp");
       if( null == m_trans_GemAttrContentCp )
       {
            Engine.Utility.Log.Error("m_trans_GemAttrContentCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_AdditiveAttrContentCp = fastComponent.FastGetComponent<Transform>("AdditiveAttrContentCp");
       if( null == m_trans_AdditiveAttrContentCp )
       {
            Engine.Utility.Log.Error("m_trans_AdditiveAttrContentCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartEquipCp = fastComponent.FastGetComponent<Transform>("PartEquipCp");
       if( null == m_trans_PartEquipCp )
       {
            Engine.Utility.Log.Error("m_trans_PartEquipCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipPartCp = fastComponent.FastGetComponent<UILabel>("EquipPartCp");
       if( null == m_label_EquipPartCp )
       {
            Engine.Utility.Log.Error("m_label_EquipPartCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipGradeCp = fastComponent.FastGetComponent<UILabel>("EquipGradeCp");
       if( null == m_label_EquipGradeCp )
       {
            Engine.Utility.Log.Error("m_label_EquipGradeCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipProCp = fastComponent.FastGetComponent<UILabel>("EquipProCp");
       if( null == m_label_EquipProCp )
       {
            Engine.Utility.Log.Error("m_label_EquipProCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipDurCp = fastComponent.FastGetComponent<UILabel>("EquipDurCp");
       if( null == m_label_EquipDurCp )
       {
            Engine.Utility.Log.Error("m_label_EquipDurCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PartMuhonCp = fastComponent.FastGetComponent<Transform>("PartMuhonCp");
       if( null == m_trans_PartMuhonCp )
       {
            Engine.Utility.Log.Error("m_trans_PartMuhonCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonGrowCp = fastComponent.FastGetComponent<UILabel>("MuhonGrowCp");
       if( null == m_label_MuhonGrowCp )
       {
            Engine.Utility.Log.Error("m_label_MuhonGrowCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonCurLvCp = fastComponent.FastGetComponent<UILabel>("MuhonCurLvCp");
       if( null == m_label_MuhonCurLvCp )
       {
            Engine.Utility.Log.Error("m_label_MuhonCurLvCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TopPartCp = fastComponent.FastGetComponent<Transform>("TopPartCp");
       if( null == m_trans_TopPartCp )
       {
            Engine.Utility.Log.Error("m_trans_TopPartCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_PowerContentCp = fastComponent.FastGetComponent<Transform>("PowerContentCp");
       if( null == m_trans_PowerContentCp )
       {
            Engine.Utility.Log.Error("m_trans_PowerContentCp 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_PowerPromoteCp = fastComponent.FastGetComponent<UISprite>("PowerPromoteCp");
       if( null == m_sprite_PowerPromoteCp )
       {
            Engine.Utility.Log.Error("m_sprite_PowerPromoteCp 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_PowerFallOffCp = fastComponent.FastGetComponent<UISprite>("PowerFallOffCp");
       if( null == m_sprite_PowerFallOffCp )
       {
            Engine.Utility.Log.Error("m_sprite_PowerFallOffCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_ItemNameCp = fastComponent.FastGetComponent<UILabel>("ItemNameCp");
       if( null == m_label_ItemNameCp )
       {
            Engine.Utility.Log.Error("m_label_ItemNameCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_IconContentCompare = fastComponent.FastGetComponent<Transform>("IconContentCompare");
       if( null == m_trans_IconContentCompare )
       {
            Engine.Utility.Log.Error("m_trans_IconContentCompare 为空，请检查prefab是否缺乏组件");
       }
        m_label_StrengthenLvCp = fastComponent.FastGetComponent<UILabel>("StrengthenLvCp");
       if( null == m_label_StrengthenLvCp )
       {
            Engine.Utility.Log.Error("m_label_StrengthenLvCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MuhonCp = fastComponent.FastGetComponent<Transform>("MuhonCp");
       if( null == m_trans_MuhonCp )
       {
            Engine.Utility.Log.Error("m_trans_MuhonCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MuhonEquipMaskCp = fastComponent.FastGetComponent<Transform>("MuhonEquipMaskCp");
       if( null == m_trans_MuhonEquipMaskCp )
       {
            Engine.Utility.Log.Error("m_trans_MuhonEquipMaskCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonNeedLevelCp = fastComponent.FastGetComponent<UILabel>("MuhonNeedLevelCp");
       if( null == m_label_MuhonNeedLevelCp )
       {
            Engine.Utility.Log.Error("m_label_MuhonNeedLevelCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_MuhonFightingPowerCp = fastComponent.FastGetComponent<UILabel>("MuhonFightingPowerCp");
       if( null == m_label_MuhonFightingPowerCp )
       {
            Engine.Utility.Log.Error("m_label_MuhonFightingPowerCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EquipCp = fastComponent.FastGetComponent<Transform>("EquipCp");
       if( null == m_trans_EquipCp )
       {
            Engine.Utility.Log.Error("m_trans_EquipCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipNeedLevelCp = fastComponent.FastGetComponent<UILabel>("EquipNeedLevelCp");
       if( null == m_label_EquipNeedLevelCp )
       {
            Engine.Utility.Log.Error("m_label_EquipNeedLevelCp 为空，请检查prefab是否缺乏组件");
       }
        m_label_EquipFightingPowerCp = fastComponent.FastGetComponent<UILabel>("EquipFightingPowerCp");
       if( null == m_label_EquipFightingPowerCp )
       {
            Engine.Utility.Log.Error("m_label_EquipFightingPowerCp 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EquipMaskCp = fastComponent.FastGetComponent<Transform>("EquipMaskCp");
       if( null == m_trans_EquipMaskCp )
       {
            Engine.Utility.Log.Error("m_trans_EquipMaskCp 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_SwitchCompare.gameObject).onClick = _onClick_SwitchCompare_Btn;
    }

    void _onClick_SwitchCompare_Btn(GameObject caster)
    {
        onClick_SwitchCompare_Btn( caster );
    }


}
