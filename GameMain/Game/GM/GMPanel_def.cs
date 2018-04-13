//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class GMPanel: UIPanelBase
{

   FastComponent         fastComponent;

    UIButton             m_btn_close;

    Transform            m_trans_left;

    UIWidget             m_widget_BaseContent;

    UIInput              m_input_Level_Input;

    UIButton             m_btn_Level_queding;

    UIInput              m_input_Exp_Input;

    UIButton             m_btn_Exp_queding;

    UIInput              m_input_ItemID_Input;

    UIInput              m_input_ItemNum_Input;

    UIButton             m_btn_Item_queding;

    UISprite             m_sprite_MoneyType;

    UIInput              m_input_GoldNum_Input;

    UIButton             m_btn_Gold_queidng;

    UIInput              m_input_SkillID_Input;

    UIInput              m_input_SkillNum_Input;

    UIButton             m_btn_Skill_queidng;

    UIInput              m_input_ProID_Input;

    UIInput              m_input_ProNum_Input;

    UIButton             m_btn_Pro_queidng;

    UIButton             m_btn_btn_ClearBag;

    UIInput              m_input_PKNum_Input;

    UIButton             m_btn_PKNum_queding;

    UIButton             m_btn_JuQing;

    UIButton             m_btn_sanshiji;

    UIButton             m_btn_yibaiji;

    UIButton             m_btn_dafuweng;

    UIButton             m_btn_LowMemory;

    UIWidget             m_widget_MissionContent;

    UIInput              m_input_AcessMission_Input;

    UIButton             m_btn_AcessMission_queidng;

    UIInput              m_input_CompleteMission_Input;

    UIButton             m_btn_CompleteMission_queidng;

    UIInput              m_input_Deliver_Input;

    UIInput              m_input_Deliver_X_Input;

    UIInput              m_input_Deliver_Y_Input;

    UIButton             m_btn_Deliver_queidng;

    UIInput              m_input_MissionAnimation_Input;

    UIButton             m_btn_MissionAnimation_queidng;

    UIWidget             m_widget_MonsterContent;

    UIInput              m_input_MonID_Input;

    UIInput              m_input_MonNum_Input;

    UIButton             m_btn_Monster_queding;

    UIButton             m_btn_KillMonster_queidng;

    UIButton             m_btn_KillAllMonster_queidng;

    UIWidget             m_widget_EquipmentContent;

    UIInput              m_input_Equip_Input;

    UIInput              m_input_HoleNum_Input;

    UIInput              m_input_Strengthen_Input;

    UIInput              m_input_Num_Input;

    UIInput              m_input_EquipPro_1_Input;

    UIInput              m_input_EquipPro_2_Input;

    UIInput              m_input_EquipPro_3_Input;

    UIInput              m_input_EquipPro_4_Input;

    UIInput              m_input_EquipPro_5_Input;

    UIButton             m_btn_Equipment_queidng;

    UIPopupList          m__Grade;

    UIPopupList          m__Job;

    UIButton             m_btn_BtnOneKeyAdd;

    UIWidget             m_widget_MuhonContent;

    UIInput              m_input_Muhon_Input;

    UIInput              m_input_Muhon_Num_Input;

    UIInput              m_input_Muhon_Level_Input;

    UIInput              m_input_MuhonPro_1_Input;

    UIInput              m_input_MuhonPro_2_Input;

    UIInput              m_input_MuhonPro_3_Input;

    UIInput              m_input_MuhonPro_4_Input;

    UIInput              m_input_MuhonPro_5_Input;

    UIButton             m_btn_Muhon_queidng;

    UIWidget             m_widget_PetContent;

    UIInput              m_input_Pet_Input;

    UIInput              m_input_Pet_Level_Input;

    UIInput              m_input_Xiuwei_Level_Input;

    UIInput              m_input_Pet_Character_Input;

    UIInput              m_input_GrowStatus_Input;

    UIButton             m_btn_Pet_queidng;

    UIInput              m_input_liliangtianfu_Input;

    UIInput              m_input_zhilitianfu_Input;

    UIInput              m_input_minjietianfu_Input;

    UIInput              m_input_tilitianfu_Input;

    UIInput              m_input_jingshentianfu_Input;

    UIWidget             m_widget_RideContent;

    UIInput              m_input_Ride_Input;

    UIInput              m_input_Ride_Level_Input;

    UIInput              m_input_Ride_Quality_Input;

    UIButton             m_btn_Ride_queidng;

    UIWidget             m_widget_MailContent;

    UIInput              m_input_MailName_Input;

    UIInput              m_input_MailNum_Input;

    UIInput              m_input_MailText_Input;

    UIInput              m_input_item_1ID_Input;

    UIInput              m_input_item_1Num_Input;

    UIInput              m_input_item_2ID_Input;

    UIInput              m_input_item_2Num_Input;

    UIInput              m_input_item_3ID_Input;

    UIInput              m_input_item_3Num_Input;

    UIInput              m_input_gold_1ID_Input;

    UIInput              m_input_gold_1Num_Input;

    UIInput              m_input_gold_2ID_Input;

    UIInput              m_input_gold_2Num_Input;

    UIButton             m_btn_Mail_queding;

    UIButton             m_btn_Base;

    UIButton             m_btn_Mission;

    UIButton             m_btn_Monster;

    UIButton             m_btn_Equipment;

    UIButton             m_btn_Muhon;

    UIButton             m_btn_Pet;

    UIButton             m_btn_Ride;

    UIButton             m_btn_Mail;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_btn_close = fastComponent.FastGetComponent<UIButton>("close");
       if( null == m_btn_close )
       {
            Engine.Utility.Log.Error("m_btn_close 为空，请检查prefab是否缺乏组件");
       }
        m_trans_left = fastComponent.FastGetComponent<Transform>("left");
       if( null == m_trans_left )
       {
            Engine.Utility.Log.Error("m_trans_left 为空，请检查prefab是否缺乏组件");
       }
        m_widget_BaseContent = fastComponent.FastGetComponent<UIWidget>("BaseContent");
       if( null == m_widget_BaseContent )
       {
            Engine.Utility.Log.Error("m_widget_BaseContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_Level_Input = fastComponent.FastGetComponent<UIInput>("Level_Input");
       if( null == m_input_Level_Input )
       {
            Engine.Utility.Log.Error("m_input_Level_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Level_queding = fastComponent.FastGetComponent<UIButton>("Level_queding");
       if( null == m_btn_Level_queding )
       {
            Engine.Utility.Log.Error("m_btn_Level_queding 为空，请检查prefab是否缺乏组件");
       }
        m_input_Exp_Input = fastComponent.FastGetComponent<UIInput>("Exp_Input");
       if( null == m_input_Exp_Input )
       {
            Engine.Utility.Log.Error("m_input_Exp_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Exp_queding = fastComponent.FastGetComponent<UIButton>("Exp_queding");
       if( null == m_btn_Exp_queding )
       {
            Engine.Utility.Log.Error("m_btn_Exp_queding 为空，请检查prefab是否缺乏组件");
       }
        m_input_ItemID_Input = fastComponent.FastGetComponent<UIInput>("ItemID_Input");
       if( null == m_input_ItemID_Input )
       {
            Engine.Utility.Log.Error("m_input_ItemID_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_ItemNum_Input = fastComponent.FastGetComponent<UIInput>("ItemNum_Input");
       if( null == m_input_ItemNum_Input )
       {
            Engine.Utility.Log.Error("m_input_ItemNum_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Item_queding = fastComponent.FastGetComponent<UIButton>("Item_queding");
       if( null == m_btn_Item_queding )
       {
            Engine.Utility.Log.Error("m_btn_Item_queding 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_MoneyType = fastComponent.FastGetComponent<UISprite>("MoneyType");
       if( null == m_sprite_MoneyType )
       {
            Engine.Utility.Log.Error("m_sprite_MoneyType 为空，请检查prefab是否缺乏组件");
       }
        m_input_GoldNum_Input = fastComponent.FastGetComponent<UIInput>("GoldNum_Input");
       if( null == m_input_GoldNum_Input )
       {
            Engine.Utility.Log.Error("m_input_GoldNum_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Gold_queidng = fastComponent.FastGetComponent<UIButton>("Gold_queidng");
       if( null == m_btn_Gold_queidng )
       {
            Engine.Utility.Log.Error("m_btn_Gold_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_input_SkillID_Input = fastComponent.FastGetComponent<UIInput>("SkillID_Input");
       if( null == m_input_SkillID_Input )
       {
            Engine.Utility.Log.Error("m_input_SkillID_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_SkillNum_Input = fastComponent.FastGetComponent<UIInput>("SkillNum_Input");
       if( null == m_input_SkillNum_Input )
       {
            Engine.Utility.Log.Error("m_input_SkillNum_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Skill_queidng = fastComponent.FastGetComponent<UIButton>("Skill_queidng");
       if( null == m_btn_Skill_queidng )
       {
            Engine.Utility.Log.Error("m_btn_Skill_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_input_ProID_Input = fastComponent.FastGetComponent<UIInput>("ProID_Input");
       if( null == m_input_ProID_Input )
       {
            Engine.Utility.Log.Error("m_input_ProID_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_ProNum_Input = fastComponent.FastGetComponent<UIInput>("ProNum_Input");
       if( null == m_input_ProNum_Input )
       {
            Engine.Utility.Log.Error("m_input_ProNum_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Pro_queidng = fastComponent.FastGetComponent<UIButton>("Pro_queidng");
       if( null == m_btn_Pro_queidng )
       {
            Engine.Utility.Log.Error("m_btn_Pro_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_btn_btn_ClearBag = fastComponent.FastGetComponent<UIButton>("btn_ClearBag");
       if( null == m_btn_btn_ClearBag )
       {
            Engine.Utility.Log.Error("m_btn_btn_ClearBag 为空，请检查prefab是否缺乏组件");
       }
        m_input_PKNum_Input = fastComponent.FastGetComponent<UIInput>("PKNum_Input");
       if( null == m_input_PKNum_Input )
       {
            Engine.Utility.Log.Error("m_input_PKNum_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_PKNum_queding = fastComponent.FastGetComponent<UIButton>("PKNum_queding");
       if( null == m_btn_PKNum_queding )
       {
            Engine.Utility.Log.Error("m_btn_PKNum_queding 为空，请检查prefab是否缺乏组件");
       }
        m_btn_JuQing = fastComponent.FastGetComponent<UIButton>("JuQing");
       if( null == m_btn_JuQing )
       {
            Engine.Utility.Log.Error("m_btn_JuQing 为空，请检查prefab是否缺乏组件");
       }
        m_btn_sanshiji = fastComponent.FastGetComponent<UIButton>("sanshiji");
       if( null == m_btn_sanshiji )
       {
            Engine.Utility.Log.Error("m_btn_sanshiji 为空，请检查prefab是否缺乏组件");
       }
        m_btn_yibaiji = fastComponent.FastGetComponent<UIButton>("yibaiji");
       if( null == m_btn_yibaiji )
       {
            Engine.Utility.Log.Error("m_btn_yibaiji 为空，请检查prefab是否缺乏组件");
       }
        m_btn_dafuweng = fastComponent.FastGetComponent<UIButton>("dafuweng");
       if( null == m_btn_dafuweng )
       {
            Engine.Utility.Log.Error("m_btn_dafuweng 为空，请检查prefab是否缺乏组件");
       }
        m_btn_LowMemory = fastComponent.FastGetComponent<UIButton>("LowMemory");
       if( null == m_btn_LowMemory )
       {
            Engine.Utility.Log.Error("m_btn_LowMemory 为空，请检查prefab是否缺乏组件");
       }
        m_widget_MissionContent = fastComponent.FastGetComponent<UIWidget>("MissionContent");
       if( null == m_widget_MissionContent )
       {
            Engine.Utility.Log.Error("m_widget_MissionContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_AcessMission_Input = fastComponent.FastGetComponent<UIInput>("AcessMission_Input");
       if( null == m_input_AcessMission_Input )
       {
            Engine.Utility.Log.Error("m_input_AcessMission_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_AcessMission_queidng = fastComponent.FastGetComponent<UIButton>("AcessMission_queidng");
       if( null == m_btn_AcessMission_queidng )
       {
            Engine.Utility.Log.Error("m_btn_AcessMission_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_input_CompleteMission_Input = fastComponent.FastGetComponent<UIInput>("CompleteMission_Input");
       if( null == m_input_CompleteMission_Input )
       {
            Engine.Utility.Log.Error("m_input_CompleteMission_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_CompleteMission_queidng = fastComponent.FastGetComponent<UIButton>("CompleteMission_queidng");
       if( null == m_btn_CompleteMission_queidng )
       {
            Engine.Utility.Log.Error("m_btn_CompleteMission_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_input_Deliver_Input = fastComponent.FastGetComponent<UIInput>("Deliver_Input");
       if( null == m_input_Deliver_Input )
       {
            Engine.Utility.Log.Error("m_input_Deliver_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Deliver_X_Input = fastComponent.FastGetComponent<UIInput>("Deliver_X_Input");
       if( null == m_input_Deliver_X_Input )
       {
            Engine.Utility.Log.Error("m_input_Deliver_X_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Deliver_Y_Input = fastComponent.FastGetComponent<UIInput>("Deliver_Y_Input");
       if( null == m_input_Deliver_Y_Input )
       {
            Engine.Utility.Log.Error("m_input_Deliver_Y_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Deliver_queidng = fastComponent.FastGetComponent<UIButton>("Deliver_queidng");
       if( null == m_btn_Deliver_queidng )
       {
            Engine.Utility.Log.Error("m_btn_Deliver_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_input_MissionAnimation_Input = fastComponent.FastGetComponent<UIInput>("MissionAnimation_Input");
       if( null == m_input_MissionAnimation_Input )
       {
            Engine.Utility.Log.Error("m_input_MissionAnimation_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_MissionAnimation_queidng = fastComponent.FastGetComponent<UIButton>("MissionAnimation_queidng");
       if( null == m_btn_MissionAnimation_queidng )
       {
            Engine.Utility.Log.Error("m_btn_MissionAnimation_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_widget_MonsterContent = fastComponent.FastGetComponent<UIWidget>("MonsterContent");
       if( null == m_widget_MonsterContent )
       {
            Engine.Utility.Log.Error("m_widget_MonsterContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_MonID_Input = fastComponent.FastGetComponent<UIInput>("MonID_Input");
       if( null == m_input_MonID_Input )
       {
            Engine.Utility.Log.Error("m_input_MonID_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_MonNum_Input = fastComponent.FastGetComponent<UIInput>("MonNum_Input");
       if( null == m_input_MonNum_Input )
       {
            Engine.Utility.Log.Error("m_input_MonNum_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Monster_queding = fastComponent.FastGetComponent<UIButton>("Monster_queding");
       if( null == m_btn_Monster_queding )
       {
            Engine.Utility.Log.Error("m_btn_Monster_queding 为空，请检查prefab是否缺乏组件");
       }
        m_btn_KillMonster_queidng = fastComponent.FastGetComponent<UIButton>("KillMonster_queidng");
       if( null == m_btn_KillMonster_queidng )
       {
            Engine.Utility.Log.Error("m_btn_KillMonster_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_btn_KillAllMonster_queidng = fastComponent.FastGetComponent<UIButton>("KillAllMonster_queidng");
       if( null == m_btn_KillAllMonster_queidng )
       {
            Engine.Utility.Log.Error("m_btn_KillAllMonster_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_widget_EquipmentContent = fastComponent.FastGetComponent<UIWidget>("EquipmentContent");
       if( null == m_widget_EquipmentContent )
       {
            Engine.Utility.Log.Error("m_widget_EquipmentContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_Equip_Input = fastComponent.FastGetComponent<UIInput>("Equip_Input");
       if( null == m_input_Equip_Input )
       {
            Engine.Utility.Log.Error("m_input_Equip_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_HoleNum_Input = fastComponent.FastGetComponent<UIInput>("HoleNum_Input");
       if( null == m_input_HoleNum_Input )
       {
            Engine.Utility.Log.Error("m_input_HoleNum_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Strengthen_Input = fastComponent.FastGetComponent<UIInput>("Strengthen_Input");
       if( null == m_input_Strengthen_Input )
       {
            Engine.Utility.Log.Error("m_input_Strengthen_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Num_Input = fastComponent.FastGetComponent<UIInput>("Num_Input");
       if( null == m_input_Num_Input )
       {
            Engine.Utility.Log.Error("m_input_Num_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_EquipPro_1_Input = fastComponent.FastGetComponent<UIInput>("EquipPro_1_Input");
       if( null == m_input_EquipPro_1_Input )
       {
            Engine.Utility.Log.Error("m_input_EquipPro_1_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_EquipPro_2_Input = fastComponent.FastGetComponent<UIInput>("EquipPro_2_Input");
       if( null == m_input_EquipPro_2_Input )
       {
            Engine.Utility.Log.Error("m_input_EquipPro_2_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_EquipPro_3_Input = fastComponent.FastGetComponent<UIInput>("EquipPro_3_Input");
       if( null == m_input_EquipPro_3_Input )
       {
            Engine.Utility.Log.Error("m_input_EquipPro_3_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_EquipPro_4_Input = fastComponent.FastGetComponent<UIInput>("EquipPro_4_Input");
       if( null == m_input_EquipPro_4_Input )
       {
            Engine.Utility.Log.Error("m_input_EquipPro_4_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_EquipPro_5_Input = fastComponent.FastGetComponent<UIInput>("EquipPro_5_Input");
       if( null == m_input_EquipPro_5_Input )
       {
            Engine.Utility.Log.Error("m_input_EquipPro_5_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Equipment_queidng = fastComponent.FastGetComponent<UIButton>("Equipment_queidng");
       if( null == m_btn_Equipment_queidng )
       {
            Engine.Utility.Log.Error("m_btn_Equipment_queidng 为空，请检查prefab是否缺乏组件");
       }
        m__Grade = fastComponent.FastGetComponent<UIPopupList>("Grade");
       if( null == m__Grade )
       {
            Engine.Utility.Log.Error("m__Grade 为空，请检查prefab是否缺乏组件");
       }
        m__Job = fastComponent.FastGetComponent<UIPopupList>("Job");
       if( null == m__Job )
       {
            Engine.Utility.Log.Error("m__Job 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnOneKeyAdd = fastComponent.FastGetComponent<UIButton>("BtnOneKeyAdd");
       if( null == m_btn_BtnOneKeyAdd )
       {
            Engine.Utility.Log.Error("m_btn_BtnOneKeyAdd 为空，请检查prefab是否缺乏组件");
       }
        m_widget_MuhonContent = fastComponent.FastGetComponent<UIWidget>("MuhonContent");
       if( null == m_widget_MuhonContent )
       {
            Engine.Utility.Log.Error("m_widget_MuhonContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_Muhon_Input = fastComponent.FastGetComponent<UIInput>("Muhon_Input");
       if( null == m_input_Muhon_Input )
       {
            Engine.Utility.Log.Error("m_input_Muhon_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Muhon_Num_Input = fastComponent.FastGetComponent<UIInput>("Muhon_Num_Input");
       if( null == m_input_Muhon_Num_Input )
       {
            Engine.Utility.Log.Error("m_input_Muhon_Num_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Muhon_Level_Input = fastComponent.FastGetComponent<UIInput>("Muhon_Level_Input");
       if( null == m_input_Muhon_Level_Input )
       {
            Engine.Utility.Log.Error("m_input_Muhon_Level_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_MuhonPro_1_Input = fastComponent.FastGetComponent<UIInput>("MuhonPro_1_Input");
       if( null == m_input_MuhonPro_1_Input )
       {
            Engine.Utility.Log.Error("m_input_MuhonPro_1_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_MuhonPro_2_Input = fastComponent.FastGetComponent<UIInput>("MuhonPro_2_Input");
       if( null == m_input_MuhonPro_2_Input )
       {
            Engine.Utility.Log.Error("m_input_MuhonPro_2_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_MuhonPro_3_Input = fastComponent.FastGetComponent<UIInput>("MuhonPro_3_Input");
       if( null == m_input_MuhonPro_3_Input )
       {
            Engine.Utility.Log.Error("m_input_MuhonPro_3_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_MuhonPro_4_Input = fastComponent.FastGetComponent<UIInput>("MuhonPro_4_Input");
       if( null == m_input_MuhonPro_4_Input )
       {
            Engine.Utility.Log.Error("m_input_MuhonPro_4_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_MuhonPro_5_Input = fastComponent.FastGetComponent<UIInput>("MuhonPro_5_Input");
       if( null == m_input_MuhonPro_5_Input )
       {
            Engine.Utility.Log.Error("m_input_MuhonPro_5_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Muhon_queidng = fastComponent.FastGetComponent<UIButton>("Muhon_queidng");
       if( null == m_btn_Muhon_queidng )
       {
            Engine.Utility.Log.Error("m_btn_Muhon_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_widget_PetContent = fastComponent.FastGetComponent<UIWidget>("PetContent");
       if( null == m_widget_PetContent )
       {
            Engine.Utility.Log.Error("m_widget_PetContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_Pet_Input = fastComponent.FastGetComponent<UIInput>("Pet_Input");
       if( null == m_input_Pet_Input )
       {
            Engine.Utility.Log.Error("m_input_Pet_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Pet_Level_Input = fastComponent.FastGetComponent<UIInput>("Pet_Level_Input");
       if( null == m_input_Pet_Level_Input )
       {
            Engine.Utility.Log.Error("m_input_Pet_Level_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Xiuwei_Level_Input = fastComponent.FastGetComponent<UIInput>("Xiuwei_Level_Input");
       if( null == m_input_Xiuwei_Level_Input )
       {
            Engine.Utility.Log.Error("m_input_Xiuwei_Level_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Pet_Character_Input = fastComponent.FastGetComponent<UIInput>("Pet_Character_Input");
       if( null == m_input_Pet_Character_Input )
       {
            Engine.Utility.Log.Error("m_input_Pet_Character_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_GrowStatus_Input = fastComponent.FastGetComponent<UIInput>("GrowStatus_Input");
       if( null == m_input_GrowStatus_Input )
       {
            Engine.Utility.Log.Error("m_input_GrowStatus_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Pet_queidng = fastComponent.FastGetComponent<UIButton>("Pet_queidng");
       if( null == m_btn_Pet_queidng )
       {
            Engine.Utility.Log.Error("m_btn_Pet_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_input_liliangtianfu_Input = fastComponent.FastGetComponent<UIInput>("liliangtianfu_Input");
       if( null == m_input_liliangtianfu_Input )
       {
            Engine.Utility.Log.Error("m_input_liliangtianfu_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_zhilitianfu_Input = fastComponent.FastGetComponent<UIInput>("zhilitianfu_Input");
       if( null == m_input_zhilitianfu_Input )
       {
            Engine.Utility.Log.Error("m_input_zhilitianfu_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_minjietianfu_Input = fastComponent.FastGetComponent<UIInput>("minjietianfu_Input");
       if( null == m_input_minjietianfu_Input )
       {
            Engine.Utility.Log.Error("m_input_minjietianfu_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_tilitianfu_Input = fastComponent.FastGetComponent<UIInput>("tilitianfu_Input");
       if( null == m_input_tilitianfu_Input )
       {
            Engine.Utility.Log.Error("m_input_tilitianfu_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_jingshentianfu_Input = fastComponent.FastGetComponent<UIInput>("jingshentianfu_Input");
       if( null == m_input_jingshentianfu_Input )
       {
            Engine.Utility.Log.Error("m_input_jingshentianfu_Input 为空，请检查prefab是否缺乏组件");
       }
        m_widget_RideContent = fastComponent.FastGetComponent<UIWidget>("RideContent");
       if( null == m_widget_RideContent )
       {
            Engine.Utility.Log.Error("m_widget_RideContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_Ride_Input = fastComponent.FastGetComponent<UIInput>("Ride_Input");
       if( null == m_input_Ride_Input )
       {
            Engine.Utility.Log.Error("m_input_Ride_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Ride_Level_Input = fastComponent.FastGetComponent<UIInput>("Ride_Level_Input");
       if( null == m_input_Ride_Level_Input )
       {
            Engine.Utility.Log.Error("m_input_Ride_Level_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_Ride_Quality_Input = fastComponent.FastGetComponent<UIInput>("Ride_Quality_Input");
       if( null == m_input_Ride_Quality_Input )
       {
            Engine.Utility.Log.Error("m_input_Ride_Quality_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Ride_queidng = fastComponent.FastGetComponent<UIButton>("Ride_queidng");
       if( null == m_btn_Ride_queidng )
       {
            Engine.Utility.Log.Error("m_btn_Ride_queidng 为空，请检查prefab是否缺乏组件");
       }
        m_widget_MailContent = fastComponent.FastGetComponent<UIWidget>("MailContent");
       if( null == m_widget_MailContent )
       {
            Engine.Utility.Log.Error("m_widget_MailContent 为空，请检查prefab是否缺乏组件");
       }
        m_input_MailName_Input = fastComponent.FastGetComponent<UIInput>("MailName_Input");
       if( null == m_input_MailName_Input )
       {
            Engine.Utility.Log.Error("m_input_MailName_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_MailNum_Input = fastComponent.FastGetComponent<UIInput>("MailNum_Input");
       if( null == m_input_MailNum_Input )
       {
            Engine.Utility.Log.Error("m_input_MailNum_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_MailText_Input = fastComponent.FastGetComponent<UIInput>("MailText_Input");
       if( null == m_input_MailText_Input )
       {
            Engine.Utility.Log.Error("m_input_MailText_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_item_1ID_Input = fastComponent.FastGetComponent<UIInput>("item_1ID_Input");
       if( null == m_input_item_1ID_Input )
       {
            Engine.Utility.Log.Error("m_input_item_1ID_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_item_1Num_Input = fastComponent.FastGetComponent<UIInput>("item_1Num_Input");
       if( null == m_input_item_1Num_Input )
       {
            Engine.Utility.Log.Error("m_input_item_1Num_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_item_2ID_Input = fastComponent.FastGetComponent<UIInput>("item_2ID_Input");
       if( null == m_input_item_2ID_Input )
       {
            Engine.Utility.Log.Error("m_input_item_2ID_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_item_2Num_Input = fastComponent.FastGetComponent<UIInput>("item_2Num_Input");
       if( null == m_input_item_2Num_Input )
       {
            Engine.Utility.Log.Error("m_input_item_2Num_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_item_3ID_Input = fastComponent.FastGetComponent<UIInput>("item_3ID_Input");
       if( null == m_input_item_3ID_Input )
       {
            Engine.Utility.Log.Error("m_input_item_3ID_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_item_3Num_Input = fastComponent.FastGetComponent<UIInput>("item_3Num_Input");
       if( null == m_input_item_3Num_Input )
       {
            Engine.Utility.Log.Error("m_input_item_3Num_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_gold_1ID_Input = fastComponent.FastGetComponent<UIInput>("gold_1ID_Input");
       if( null == m_input_gold_1ID_Input )
       {
            Engine.Utility.Log.Error("m_input_gold_1ID_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_gold_1Num_Input = fastComponent.FastGetComponent<UIInput>("gold_1Num_Input");
       if( null == m_input_gold_1Num_Input )
       {
            Engine.Utility.Log.Error("m_input_gold_1Num_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_gold_2ID_Input = fastComponent.FastGetComponent<UIInput>("gold_2ID_Input");
       if( null == m_input_gold_2ID_Input )
       {
            Engine.Utility.Log.Error("m_input_gold_2ID_Input 为空，请检查prefab是否缺乏组件");
       }
        m_input_gold_2Num_Input = fastComponent.FastGetComponent<UIInput>("gold_2Num_Input");
       if( null == m_input_gold_2Num_Input )
       {
            Engine.Utility.Log.Error("m_input_gold_2Num_Input 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Mail_queding = fastComponent.FastGetComponent<UIButton>("Mail_queding");
       if( null == m_btn_Mail_queding )
       {
            Engine.Utility.Log.Error("m_btn_Mail_queding 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Base = fastComponent.FastGetComponent<UIButton>("Base");
       if( null == m_btn_Base )
       {
            Engine.Utility.Log.Error("m_btn_Base 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Mission = fastComponent.FastGetComponent<UIButton>("Mission");
       if( null == m_btn_Mission )
       {
            Engine.Utility.Log.Error("m_btn_Mission 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Monster = fastComponent.FastGetComponent<UIButton>("Monster");
       if( null == m_btn_Monster )
       {
            Engine.Utility.Log.Error("m_btn_Monster 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Equipment = fastComponent.FastGetComponent<UIButton>("Equipment");
       if( null == m_btn_Equipment )
       {
            Engine.Utility.Log.Error("m_btn_Equipment 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Muhon = fastComponent.FastGetComponent<UIButton>("Muhon");
       if( null == m_btn_Muhon )
       {
            Engine.Utility.Log.Error("m_btn_Muhon 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Pet = fastComponent.FastGetComponent<UIButton>("Pet");
       if( null == m_btn_Pet )
       {
            Engine.Utility.Log.Error("m_btn_Pet 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Ride = fastComponent.FastGetComponent<UIButton>("Ride");
       if( null == m_btn_Ride )
       {
            Engine.Utility.Log.Error("m_btn_Ride 为空，请检查prefab是否缺乏组件");
       }
        m_btn_Mail = fastComponent.FastGetComponent<UIButton>("Mail");
       if( null == m_btn_Mail )
       {
            Engine.Utility.Log.Error("m_btn_Mail 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_close.gameObject).onClick = _onClick_Close_Btn;
        UIEventListener.Get(m_btn_Level_queding.gameObject).onClick = _onClick_Level_queding_Btn;
        UIEventListener.Get(m_btn_Exp_queding.gameObject).onClick = _onClick_Exp_queding_Btn;
        UIEventListener.Get(m_btn_Item_queding.gameObject).onClick = _onClick_Item_queding_Btn;
        UIEventListener.Get(m_btn_Gold_queidng.gameObject).onClick = _onClick_Gold_queidng_Btn;
        UIEventListener.Get(m_btn_Skill_queidng.gameObject).onClick = _onClick_Skill_queidng_Btn;
        UIEventListener.Get(m_btn_Pro_queidng.gameObject).onClick = _onClick_Pro_queidng_Btn;
        UIEventListener.Get(m_btn_btn_ClearBag.gameObject).onClick = _onClick_Btn_ClearBag_Btn;
        UIEventListener.Get(m_btn_PKNum_queding.gameObject).onClick = _onClick_PKNum_queding_Btn;
        UIEventListener.Get(m_btn_JuQing.gameObject).onClick = _onClick_JuQing_Btn;
        UIEventListener.Get(m_btn_sanshiji.gameObject).onClick = _onClick_Sanshiji_Btn;
        UIEventListener.Get(m_btn_yibaiji.gameObject).onClick = _onClick_Yibaiji_Btn;
        UIEventListener.Get(m_btn_dafuweng.gameObject).onClick = _onClick_Dafuweng_Btn;
        UIEventListener.Get(m_btn_LowMemory.gameObject).onClick = _onClick_LowMemory_Btn;
        UIEventListener.Get(m_btn_AcessMission_queidng.gameObject).onClick = _onClick_AcessMission_queidng_Btn;
        UIEventListener.Get(m_btn_CompleteMission_queidng.gameObject).onClick = _onClick_CompleteMission_queidng_Btn;
        UIEventListener.Get(m_btn_Deliver_queidng.gameObject).onClick = _onClick_Deliver_queidng_Btn;
        UIEventListener.Get(m_btn_MissionAnimation_queidng.gameObject).onClick = _onClick_MissionAnimation_queidng_Btn;
        UIEventListener.Get(m_btn_Monster_queding.gameObject).onClick = _onClick_Monster_queding_Btn;
        UIEventListener.Get(m_btn_KillMonster_queidng.gameObject).onClick = _onClick_KillMonster_queidng_Btn;
        UIEventListener.Get(m_btn_KillAllMonster_queidng.gameObject).onClick = _onClick_KillAllMonster_queidng_Btn;
        UIEventListener.Get(m_btn_Equipment_queidng.gameObject).onClick = _onClick_Equipment_queidng_Btn;
        UIEventListener.Get(m_btn_BtnOneKeyAdd.gameObject).onClick = _onClick_BtnOneKeyAdd_Btn;
        UIEventListener.Get(m_btn_Muhon_queidng.gameObject).onClick = _onClick_Muhon_queidng_Btn;
        UIEventListener.Get(m_btn_Pet_queidng.gameObject).onClick = _onClick_Pet_queidng_Btn;
        UIEventListener.Get(m_btn_Ride_queidng.gameObject).onClick = _onClick_Ride_queidng_Btn;
        UIEventListener.Get(m_btn_Mail_queding.gameObject).onClick = _onClick_Mail_queding_Btn;
        UIEventListener.Get(m_btn_Base.gameObject).onClick = _onClick_Base_Btn;
        UIEventListener.Get(m_btn_Mission.gameObject).onClick = _onClick_Mission_Btn;
        UIEventListener.Get(m_btn_Monster.gameObject).onClick = _onClick_Monster_Btn;
        UIEventListener.Get(m_btn_Equipment.gameObject).onClick = _onClick_Equipment_Btn;
        UIEventListener.Get(m_btn_Muhon.gameObject).onClick = _onClick_Muhon_Btn;
        UIEventListener.Get(m_btn_Pet.gameObject).onClick = _onClick_Pet_Btn;
        UIEventListener.Get(m_btn_Ride.gameObject).onClick = _onClick_Ride_Btn;
        UIEventListener.Get(m_btn_Mail.gameObject).onClick = _onClick_Mail_Btn;
    }

    void _onClick_Close_Btn(GameObject caster)
    {
        onClick_Close_Btn( caster );
    }

    void _onClick_Level_queding_Btn(GameObject caster)
    {
        onClick_Level_queding_Btn( caster );
    }

    void _onClick_Exp_queding_Btn(GameObject caster)
    {
        onClick_Exp_queding_Btn( caster );
    }

    void _onClick_Item_queding_Btn(GameObject caster)
    {
        onClick_Item_queding_Btn( caster );
    }

    void _onClick_Gold_queidng_Btn(GameObject caster)
    {
        onClick_Gold_queidng_Btn( caster );
    }

    void _onClick_Skill_queidng_Btn(GameObject caster)
    {
        onClick_Skill_queidng_Btn( caster );
    }

    void _onClick_Pro_queidng_Btn(GameObject caster)
    {
        onClick_Pro_queidng_Btn( caster );
    }

    void _onClick_Btn_ClearBag_Btn(GameObject caster)
    {
        onClick_Btn_ClearBag_Btn( caster );
    }

    void _onClick_PKNum_queding_Btn(GameObject caster)
    {
        onClick_PKNum_queding_Btn( caster );
    }

    void _onClick_JuQing_Btn(GameObject caster)
    {
        onClick_JuQing_Btn( caster );
    }

    void _onClick_Sanshiji_Btn(GameObject caster)
    {
        onClick_Sanshiji_Btn( caster );
    }

    void _onClick_Yibaiji_Btn(GameObject caster)
    {
        onClick_Yibaiji_Btn( caster );
    }

    void _onClick_Dafuweng_Btn(GameObject caster)
    {
        onClick_Dafuweng_Btn( caster );
    }

    void _onClick_LowMemory_Btn(GameObject caster)
    {
        onClick_LowMemory_Btn( caster );
    }

    void _onClick_AcessMission_queidng_Btn(GameObject caster)
    {
        onClick_AcessMission_queidng_Btn( caster );
    }

    void _onClick_CompleteMission_queidng_Btn(GameObject caster)
    {
        onClick_CompleteMission_queidng_Btn( caster );
    }

    void _onClick_Deliver_queidng_Btn(GameObject caster)
    {
        onClick_Deliver_queidng_Btn( caster );
    }

    void _onClick_MissionAnimation_queidng_Btn(GameObject caster)
    {
        onClick_MissionAnimation_queidng_Btn( caster );
    }

    void _onClick_Monster_queding_Btn(GameObject caster)
    {
        onClick_Monster_queding_Btn( caster );
    }

    void _onClick_KillMonster_queidng_Btn(GameObject caster)
    {
        onClick_KillMonster_queidng_Btn( caster );
    }

    void _onClick_KillAllMonster_queidng_Btn(GameObject caster)
    {
        onClick_KillAllMonster_queidng_Btn( caster );
    }

    void _onClick_Equipment_queidng_Btn(GameObject caster)
    {
        onClick_Equipment_queidng_Btn( caster );
    }

    void _onClick_BtnOneKeyAdd_Btn(GameObject caster)
    {
        onClick_BtnOneKeyAdd_Btn( caster );
    }

    void _onClick_Muhon_queidng_Btn(GameObject caster)
    {
        onClick_Muhon_queidng_Btn( caster );
    }

    void _onClick_Pet_queidng_Btn(GameObject caster)
    {
        onClick_Pet_queidng_Btn( caster );
    }

    void _onClick_Ride_queidng_Btn(GameObject caster)
    {
        onClick_Ride_queidng_Btn( caster );
    }

    void _onClick_Mail_queding_Btn(GameObject caster)
    {
        onClick_Mail_queding_Btn( caster );
    }

    void _onClick_Base_Btn(GameObject caster)
    {
        onClick_Base_Btn( caster );
    }

    void _onClick_Mission_Btn(GameObject caster)
    {
        onClick_Mission_Btn( caster );
    }

    void _onClick_Monster_Btn(GameObject caster)
    {
        onClick_Monster_Btn( caster );
    }

    void _onClick_Equipment_Btn(GameObject caster)
    {
        onClick_Equipment_Btn( caster );
    }

    void _onClick_Muhon_Btn(GameObject caster)
    {
        onClick_Muhon_Btn( caster );
    }

    void _onClick_Pet_Btn(GameObject caster)
    {
        onClick_Pet_Btn( caster );
    }

    void _onClick_Ride_Btn(GameObject caster)
    {
        onClick_Ride_Btn( caster );
    }

    void _onClick_Mail_Btn(GameObject caster)
    {
        onClick_Mail_Btn( caster );
    }


}
