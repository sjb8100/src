//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;


partial class ClanPanel: UIPanelBase
{

    public enum TabMode{
		None = 0,
		//信息
		XinXi = 1,
		//成员
		ChengYuan = 2,
		//活动
		HuoDong = 3,
		//技能
		JiNeng = 4,
		Max,
    }

   FastComponent         fastComponent;

    Transform            m_trans_Content;

    Transform            m_trans_LeftContent;

    Transform            m_trans_ActivityScrollViewContent;

    UIGridCreatorBase    m_ctor_ActivityScrollView;

    UIGridCreatorBase    m_ctor_LeftBtnRoot;

    Transform            m_trans_DonateContent;

    Transform            m_trans_DonateScrollViewContent;

    UIGridCreatorBase    m_ctor_DonateScrollView;

    Transform            m_trans_UpgradeContent;

    Transform            m_trans_UpgradeNormal;

    UIButton             m_btn_BtnUpgrade;

    Transform            m_trans_UpgradeCur;

    Transform            m_trans_UpgradeNext;

    Transform            m_trans_ClanUpgradeCost;

    Transform            m_trans_CostZJ;

    UILabel              m_label_ClanUpgradeCostZJ;

    Transform            m_trans_CostZG;

    UILabel              m_label_ClanUpgradeCostZG;

    Transform            m_trans_UpgradeMax;

    Transform            m_trans_Info;

    UILabel              m_label_UpgradeMaxInfo;

    Transform            m_trans_Max;

    UILabel              m_label_UpgradeMaxLv;

    UILabel              m_label_ClanMaxLv;

    UILabel              m_label_clanlevelnum;

    Transform            m_trans_DeclareWarContent;

    UIButton             m_btn_BtnDeclareWar;

    UIGridCreatorBase    m_ctor_DecareWarListScoll;

    UISprite             m_sprite_DeclareTitle;

    Transform            m_trans_ActivityCompleteNotice;

    UIButton             m_btn_BtnRefresh;

    Transform            m_trans_UICGRefreshCost;

    Transform            m_trans_InfoContent;

    Transform            m_trans_InfoRight;

    UILabel              m_label_DetailClanTitle;

    Transform            m_trans_DetailInfoClanId;

    Transform            m_trans_DetailInfoShaikh;

    Transform            m_trans_DetailInfoClanLv;

    Transform            m_trans_DetailInfoClanMoney;

    Transform            m_trans_DetailInfoClanConb;

    Transform            m_trans_DetailInfoClanFgt;

    Transform            m_trans_DetailInfoClanSpT;

    Transform            m_trans_DetailInfoClanNum;

    Transform            m_trans_DetailInfoClanDaySpT;

    UILabel              m_label_DetailGGInfo;

    UIButton             m_btn_BtnEditorGG;

    UIButton             m_btn_ShengWangStoreBtn;

    UILabel              m_label_ShengWangLabel;

    UIButton             m_btn_LingDiBtn;

    Transform            m_trans_SkillScrollViewContent;

    UIGridCreatorBase    m_ctor_SkillScrollView;

    Transform            m_trans_TabSkillLearn;

    Transform            m_trans_TabSkillDev;

    Transform            m_trans_Cost;

    UIButton             m_btn_BtnLearn;

    UIButton             m_btn_BtnDev;

    Transform            m_trans_SkillCost1;

    Transform            m_trans_SkillCost2;

    Transform            m_trans_SkillRemain;

    UISprite             m_sprite_SkillRemainIcon;

    UILabel              m_label_SkillRemainNum;

    UILabel              m_label_SkillEffectCur;

    UILabel              m_label_SkillEffectNext;

    UILabel              m_label_SkillName;

    UILabel              m_label_SkillLv;

    UILabel              m_label_SkillTips;

    Transform            m_trans_MemberTab;

    Transform            m_trans_ApplyTab;

    Transform            m_trans_EventTab;

    Transform            m_trans_MemberArea_Member;

    Transform            m_trans_MemberScrollViewContent;

    Transform            m_trans_MemberScrollView;

    Transform            m_trans_MemberApplyScrollViewContent;

    Transform            m_trans_MemberApplyScrollView;

    Transform            m_trans_MemberTitleM;

    UIButton             m_btn_TabProfBtn;

    UIButton             m_btn_TabNameBtn;

    UIButton             m_btn_TabLvBtn;

    UIButton             m_btn_TabHonorBtn;

    UIButton             m_btn_TabDutyBtn;

    UIButton             m_btn_TabJoinTimeBtn;

    UIButton             m_btn_TabOutLineTimeBtn;

    UILabel              m_label_MemberONT;

    Transform            m_trans_MemberTitleA;

    UIButton             m_btn_applyProfBtn;

    UIButton             m_btn_applyNameBtn;

    UIButton             m_btn_applyLvBtn;

    UIButton             m_btn_applyFightBtn;

    Transform            m_trans_MemberArea_Event;

    UIGridCreatorBase    m_ctor_ClanHonorScrollView;

    UIButton             m_btn_BtnQuitClan;

    UIButton             m_btn_BtnChangeClan;

    UIButton             m_btn_BtnInvite;

    UIButton             m_btn_BtnMassSendMsg;

    UIButton             m_btn_BtnClear;

    Transform            m_trans_MemmberInviteContent;

    UIButton             m_btn_BtnFriend;

    UIButton             m_btn_BtnNearby;

    UIButton             m_btn_MemberInviteCollider;

    Transform            m_trans_RightContent;

    Transform            m_trans_FunctioToggles;

    Transform            m_trans_UIClanTaskGrid;

    UILabel              m_label_Label;

    UILabel              m_label_Times;

    UISlider             m_slider_Expslider;

    UILabel              m_label_RewardTxt;

    UILabel              m_label_RewardTxt2;

    UILabel              m_label_RewardTxt3;

    UISprite             m_sprite_star;

    Transform            m_trans_UIClanSkillGrid;

    Transform            m_trans_UIClanApplyGrid;

    Transform            m_trans_UIClanTabGrid;

    Transform            m_trans_UIClanDonateGrid;

    Transform            m_trans_GetSW;

    Transform            m_trans_GetZG;

    Transform            m_trans_GetZJ;

    Transform            m_trans_UIClanHonorGrid;

    Transform            m_trans_UIClanDeclareWarGrid;

    Transform            m_trans_UIClanMemberGrid;


    //初始化控件变量
    protected override void InitControls()
    {
       fastComponent = GetComponent<FastComponent>();
       fastComponent.BuildFastComponents();
        m_trans_Content = fastComponent.FastGetComponent<Transform>("Content");
       if( null == m_trans_Content )
       {
            Engine.Utility.Log.Error("m_trans_Content 为空，请检查prefab是否缺乏组件");
       }
        m_trans_LeftContent = fastComponent.FastGetComponent<Transform>("LeftContent");
       if( null == m_trans_LeftContent )
       {
            Engine.Utility.Log.Error("m_trans_LeftContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ActivityScrollViewContent = fastComponent.FastGetComponent<Transform>("ActivityScrollViewContent");
       if( null == m_trans_ActivityScrollViewContent )
       {
            Engine.Utility.Log.Error("m_trans_ActivityScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ActivityScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ActivityScrollView");
       if( null == m_ctor_ActivityScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ActivityScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_LeftBtnRoot = fastComponent.FastGetComponent<UIGridCreatorBase>("LeftBtnRoot");
       if( null == m_ctor_LeftBtnRoot )
       {
            Engine.Utility.Log.Error("m_ctor_LeftBtnRoot 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DonateContent = fastComponent.FastGetComponent<Transform>("DonateContent");
       if( null == m_trans_DonateContent )
       {
            Engine.Utility.Log.Error("m_trans_DonateContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DonateScrollViewContent = fastComponent.FastGetComponent<Transform>("DonateScrollViewContent");
       if( null == m_trans_DonateScrollViewContent )
       {
            Engine.Utility.Log.Error("m_trans_DonateScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_DonateScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("DonateScrollView");
       if( null == m_ctor_DonateScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_DonateScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UpgradeContent = fastComponent.FastGetComponent<Transform>("UpgradeContent");
       if( null == m_trans_UpgradeContent )
       {
            Engine.Utility.Log.Error("m_trans_UpgradeContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UpgradeNormal = fastComponent.FastGetComponent<Transform>("UpgradeNormal");
       if( null == m_trans_UpgradeNormal )
       {
            Engine.Utility.Log.Error("m_trans_UpgradeNormal 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnUpgrade = fastComponent.FastGetComponent<UIButton>("BtnUpgrade");
       if( null == m_btn_BtnUpgrade )
       {
            Engine.Utility.Log.Error("m_btn_BtnUpgrade 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UpgradeCur = fastComponent.FastGetComponent<Transform>("UpgradeCur");
       if( null == m_trans_UpgradeCur )
       {
            Engine.Utility.Log.Error("m_trans_UpgradeCur 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UpgradeNext = fastComponent.FastGetComponent<Transform>("UpgradeNext");
       if( null == m_trans_UpgradeNext )
       {
            Engine.Utility.Log.Error("m_trans_UpgradeNext 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ClanUpgradeCost = fastComponent.FastGetComponent<Transform>("ClanUpgradeCost");
       if( null == m_trans_ClanUpgradeCost )
       {
            Engine.Utility.Log.Error("m_trans_ClanUpgradeCost 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CostZJ = fastComponent.FastGetComponent<Transform>("CostZJ");
       if( null == m_trans_CostZJ )
       {
            Engine.Utility.Log.Error("m_trans_CostZJ 为空，请检查prefab是否缺乏组件");
       }
        m_label_ClanUpgradeCostZJ = fastComponent.FastGetComponent<UILabel>("ClanUpgradeCostZJ");
       if( null == m_label_ClanUpgradeCostZJ )
       {
            Engine.Utility.Log.Error("m_label_ClanUpgradeCostZJ 为空，请检查prefab是否缺乏组件");
       }
        m_trans_CostZG = fastComponent.FastGetComponent<Transform>("CostZG");
       if( null == m_trans_CostZG )
       {
            Engine.Utility.Log.Error("m_trans_CostZG 为空，请检查prefab是否缺乏组件");
       }
        m_label_ClanUpgradeCostZG = fastComponent.FastGetComponent<UILabel>("ClanUpgradeCostZG");
       if( null == m_label_ClanUpgradeCostZG )
       {
            Engine.Utility.Log.Error("m_label_ClanUpgradeCostZG 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UpgradeMax = fastComponent.FastGetComponent<Transform>("UpgradeMax");
       if( null == m_trans_UpgradeMax )
       {
            Engine.Utility.Log.Error("m_trans_UpgradeMax 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Info = fastComponent.FastGetComponent<Transform>("Info");
       if( null == m_trans_Info )
       {
            Engine.Utility.Log.Error("m_trans_Info 为空，请检查prefab是否缺乏组件");
       }
        m_label_UpgradeMaxInfo = fastComponent.FastGetComponent<UILabel>("UpgradeMaxInfo");
       if( null == m_label_UpgradeMaxInfo )
       {
            Engine.Utility.Log.Error("m_label_UpgradeMaxInfo 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Max = fastComponent.FastGetComponent<Transform>("Max");
       if( null == m_trans_Max )
       {
            Engine.Utility.Log.Error("m_trans_Max 为空，请检查prefab是否缺乏组件");
       }
        m_label_UpgradeMaxLv = fastComponent.FastGetComponent<UILabel>("UpgradeMaxLv");
       if( null == m_label_UpgradeMaxLv )
       {
            Engine.Utility.Log.Error("m_label_UpgradeMaxLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_ClanMaxLv = fastComponent.FastGetComponent<UILabel>("ClanMaxLv");
       if( null == m_label_ClanMaxLv )
       {
            Engine.Utility.Log.Error("m_label_ClanMaxLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_clanlevelnum = fastComponent.FastGetComponent<UILabel>("clanlevelnum");
       if( null == m_label_clanlevelnum )
       {
            Engine.Utility.Log.Error("m_label_clanlevelnum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DeclareWarContent = fastComponent.FastGetComponent<Transform>("DeclareWarContent");
       if( null == m_trans_DeclareWarContent )
       {
            Engine.Utility.Log.Error("m_trans_DeclareWarContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnDeclareWar = fastComponent.FastGetComponent<UIButton>("BtnDeclareWar");
       if( null == m_btn_BtnDeclareWar )
       {
            Engine.Utility.Log.Error("m_btn_BtnDeclareWar 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_DecareWarListScoll = fastComponent.FastGetComponent<UIGridCreatorBase>("DecareWarListScoll");
       if( null == m_ctor_DecareWarListScoll )
       {
            Engine.Utility.Log.Error("m_ctor_DecareWarListScoll 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_DeclareTitle = fastComponent.FastGetComponent<UISprite>("DeclareTitle");
       if( null == m_sprite_DeclareTitle )
       {
            Engine.Utility.Log.Error("m_sprite_DeclareTitle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ActivityCompleteNotice = fastComponent.FastGetComponent<Transform>("ActivityCompleteNotice");
       if( null == m_trans_ActivityCompleteNotice )
       {
            Engine.Utility.Log.Error("m_trans_ActivityCompleteNotice 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnRefresh = fastComponent.FastGetComponent<UIButton>("BtnRefresh");
       if( null == m_btn_BtnRefresh )
       {
            Engine.Utility.Log.Error("m_btn_BtnRefresh 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UICGRefreshCost = fastComponent.FastGetComponent<Transform>("UICGRefreshCost");
       if( null == m_trans_UICGRefreshCost )
       {
            Engine.Utility.Log.Error("m_trans_UICGRefreshCost 为空，请检查prefab是否缺乏组件");
       }
        m_trans_InfoContent = fastComponent.FastGetComponent<Transform>("InfoContent");
       if( null == m_trans_InfoContent )
       {
            Engine.Utility.Log.Error("m_trans_InfoContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_InfoRight = fastComponent.FastGetComponent<Transform>("InfoRight");
       if( null == m_trans_InfoRight )
       {
            Engine.Utility.Log.Error("m_trans_InfoRight 为空，请检查prefab是否缺乏组件");
       }
        m_label_DetailClanTitle = fastComponent.FastGetComponent<UILabel>("DetailClanTitle");
       if( null == m_label_DetailClanTitle )
       {
            Engine.Utility.Log.Error("m_label_DetailClanTitle 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailInfoClanId = fastComponent.FastGetComponent<Transform>("DetailInfoClanId");
       if( null == m_trans_DetailInfoClanId )
       {
            Engine.Utility.Log.Error("m_trans_DetailInfoClanId 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailInfoShaikh = fastComponent.FastGetComponent<Transform>("DetailInfoShaikh");
       if( null == m_trans_DetailInfoShaikh )
       {
            Engine.Utility.Log.Error("m_trans_DetailInfoShaikh 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailInfoClanLv = fastComponent.FastGetComponent<Transform>("DetailInfoClanLv");
       if( null == m_trans_DetailInfoClanLv )
       {
            Engine.Utility.Log.Error("m_trans_DetailInfoClanLv 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailInfoClanMoney = fastComponent.FastGetComponent<Transform>("DetailInfoClanMoney");
       if( null == m_trans_DetailInfoClanMoney )
       {
            Engine.Utility.Log.Error("m_trans_DetailInfoClanMoney 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailInfoClanConb = fastComponent.FastGetComponent<Transform>("DetailInfoClanConb");
       if( null == m_trans_DetailInfoClanConb )
       {
            Engine.Utility.Log.Error("m_trans_DetailInfoClanConb 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailInfoClanFgt = fastComponent.FastGetComponent<Transform>("DetailInfoClanFgt");
       if( null == m_trans_DetailInfoClanFgt )
       {
            Engine.Utility.Log.Error("m_trans_DetailInfoClanFgt 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailInfoClanSpT = fastComponent.FastGetComponent<Transform>("DetailInfoClanSpT");
       if( null == m_trans_DetailInfoClanSpT )
       {
            Engine.Utility.Log.Error("m_trans_DetailInfoClanSpT 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailInfoClanNum = fastComponent.FastGetComponent<Transform>("DetailInfoClanNum");
       if( null == m_trans_DetailInfoClanNum )
       {
            Engine.Utility.Log.Error("m_trans_DetailInfoClanNum 为空，请检查prefab是否缺乏组件");
       }
        m_trans_DetailInfoClanDaySpT = fastComponent.FastGetComponent<Transform>("DetailInfoClanDaySpT");
       if( null == m_trans_DetailInfoClanDaySpT )
       {
            Engine.Utility.Log.Error("m_trans_DetailInfoClanDaySpT 为空，请检查prefab是否缺乏组件");
       }
        m_label_DetailGGInfo = fastComponent.FastGetComponent<UILabel>("DetailGGInfo");
       if( null == m_label_DetailGGInfo )
       {
            Engine.Utility.Log.Error("m_label_DetailGGInfo 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnEditorGG = fastComponent.FastGetComponent<UIButton>("BtnEditorGG");
       if( null == m_btn_BtnEditorGG )
       {
            Engine.Utility.Log.Error("m_btn_BtnEditorGG 为空，请检查prefab是否缺乏组件");
       }
        m_btn_ShengWangStoreBtn = fastComponent.FastGetComponent<UIButton>("ShengWangStoreBtn");
       if( null == m_btn_ShengWangStoreBtn )
       {
            Engine.Utility.Log.Error("m_btn_ShengWangStoreBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_ShengWangLabel = fastComponent.FastGetComponent<UILabel>("ShengWangLabel");
       if( null == m_label_ShengWangLabel )
       {
            Engine.Utility.Log.Error("m_label_ShengWangLabel 为空，请检查prefab是否缺乏组件");
       }
        m_btn_LingDiBtn = fastComponent.FastGetComponent<UIButton>("LingDiBtn");
       if( null == m_btn_LingDiBtn )
       {
            Engine.Utility.Log.Error("m_btn_LingDiBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SkillScrollViewContent = fastComponent.FastGetComponent<Transform>("SkillScrollViewContent");
       if( null == m_trans_SkillScrollViewContent )
       {
            Engine.Utility.Log.Error("m_trans_SkillScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_SkillScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("SkillScrollView");
       if( null == m_ctor_SkillScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_SkillScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TabSkillLearn = fastComponent.FastGetComponent<Transform>("TabSkillLearn");
       if( null == m_trans_TabSkillLearn )
       {
            Engine.Utility.Log.Error("m_trans_TabSkillLearn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_TabSkillDev = fastComponent.FastGetComponent<Transform>("TabSkillDev");
       if( null == m_trans_TabSkillDev )
       {
            Engine.Utility.Log.Error("m_trans_TabSkillDev 为空，请检查prefab是否缺乏组件");
       }
        m_trans_Cost = fastComponent.FastGetComponent<Transform>("Cost");
       if( null == m_trans_Cost )
       {
            Engine.Utility.Log.Error("m_trans_Cost 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnLearn = fastComponent.FastGetComponent<UIButton>("BtnLearn");
       if( null == m_btn_BtnLearn )
       {
            Engine.Utility.Log.Error("m_btn_BtnLearn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnDev = fastComponent.FastGetComponent<UIButton>("BtnDev");
       if( null == m_btn_BtnDev )
       {
            Engine.Utility.Log.Error("m_btn_BtnDev 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SkillCost1 = fastComponent.FastGetComponent<Transform>("SkillCost1");
       if( null == m_trans_SkillCost1 )
       {
            Engine.Utility.Log.Error("m_trans_SkillCost1 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SkillCost2 = fastComponent.FastGetComponent<Transform>("SkillCost2");
       if( null == m_trans_SkillCost2 )
       {
            Engine.Utility.Log.Error("m_trans_SkillCost2 为空，请检查prefab是否缺乏组件");
       }
        m_trans_SkillRemain = fastComponent.FastGetComponent<Transform>("SkillRemain");
       if( null == m_trans_SkillRemain )
       {
            Engine.Utility.Log.Error("m_trans_SkillRemain 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_SkillRemainIcon = fastComponent.FastGetComponent<UISprite>("SkillRemainIcon");
       if( null == m_sprite_SkillRemainIcon )
       {
            Engine.Utility.Log.Error("m_sprite_SkillRemainIcon 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillRemainNum = fastComponent.FastGetComponent<UILabel>("SkillRemainNum");
       if( null == m_label_SkillRemainNum )
       {
            Engine.Utility.Log.Error("m_label_SkillRemainNum 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillEffectCur = fastComponent.FastGetComponent<UILabel>("SkillEffectCur");
       if( null == m_label_SkillEffectCur )
       {
            Engine.Utility.Log.Error("m_label_SkillEffectCur 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillEffectNext = fastComponent.FastGetComponent<UILabel>("SkillEffectNext");
       if( null == m_label_SkillEffectNext )
       {
            Engine.Utility.Log.Error("m_label_SkillEffectNext 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillName = fastComponent.FastGetComponent<UILabel>("SkillName");
       if( null == m_label_SkillName )
       {
            Engine.Utility.Log.Error("m_label_SkillName 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillLv = fastComponent.FastGetComponent<UILabel>("SkillLv");
       if( null == m_label_SkillLv )
       {
            Engine.Utility.Log.Error("m_label_SkillLv 为空，请检查prefab是否缺乏组件");
       }
        m_label_SkillTips = fastComponent.FastGetComponent<UILabel>("SkillTips");
       if( null == m_label_SkillTips )
       {
            Engine.Utility.Log.Error("m_label_SkillTips 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberTab = fastComponent.FastGetComponent<Transform>("MemberTab");
       if( null == m_trans_MemberTab )
       {
            Engine.Utility.Log.Error("m_trans_MemberTab 为空，请检查prefab是否缺乏组件");
       }
        m_trans_ApplyTab = fastComponent.FastGetComponent<Transform>("ApplyTab");
       if( null == m_trans_ApplyTab )
       {
            Engine.Utility.Log.Error("m_trans_ApplyTab 为空，请检查prefab是否缺乏组件");
       }
        m_trans_EventTab = fastComponent.FastGetComponent<Transform>("EventTab");
       if( null == m_trans_EventTab )
       {
            Engine.Utility.Log.Error("m_trans_EventTab 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberArea_Member = fastComponent.FastGetComponent<Transform>("MemberArea_Member");
       if( null == m_trans_MemberArea_Member )
       {
            Engine.Utility.Log.Error("m_trans_MemberArea_Member 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberScrollViewContent = fastComponent.FastGetComponent<Transform>("MemberScrollViewContent");
       if( null == m_trans_MemberScrollViewContent )
       {
            Engine.Utility.Log.Error("m_trans_MemberScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberScrollView = fastComponent.FastGetComponent<Transform>("MemberScrollView");
       if( null == m_trans_MemberScrollView )
       {
            Engine.Utility.Log.Error("m_trans_MemberScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberApplyScrollViewContent = fastComponent.FastGetComponent<Transform>("MemberApplyScrollViewContent");
       if( null == m_trans_MemberApplyScrollViewContent )
       {
            Engine.Utility.Log.Error("m_trans_MemberApplyScrollViewContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberApplyScrollView = fastComponent.FastGetComponent<Transform>("MemberApplyScrollView");
       if( null == m_trans_MemberApplyScrollView )
       {
            Engine.Utility.Log.Error("m_trans_MemberApplyScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberTitleM = fastComponent.FastGetComponent<Transform>("MemberTitleM");
       if( null == m_trans_MemberTitleM )
       {
            Engine.Utility.Log.Error("m_trans_MemberTitleM 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabProfBtn = fastComponent.FastGetComponent<UIButton>("TabProfBtn");
       if( null == m_btn_TabProfBtn )
       {
            Engine.Utility.Log.Error("m_btn_TabProfBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabNameBtn = fastComponent.FastGetComponent<UIButton>("TabNameBtn");
       if( null == m_btn_TabNameBtn )
       {
            Engine.Utility.Log.Error("m_btn_TabNameBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabLvBtn = fastComponent.FastGetComponent<UIButton>("TabLvBtn");
       if( null == m_btn_TabLvBtn )
       {
            Engine.Utility.Log.Error("m_btn_TabLvBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabHonorBtn = fastComponent.FastGetComponent<UIButton>("TabHonorBtn");
       if( null == m_btn_TabHonorBtn )
       {
            Engine.Utility.Log.Error("m_btn_TabHonorBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabDutyBtn = fastComponent.FastGetComponent<UIButton>("TabDutyBtn");
       if( null == m_btn_TabDutyBtn )
       {
            Engine.Utility.Log.Error("m_btn_TabDutyBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabJoinTimeBtn = fastComponent.FastGetComponent<UIButton>("TabJoinTimeBtn");
       if( null == m_btn_TabJoinTimeBtn )
       {
            Engine.Utility.Log.Error("m_btn_TabJoinTimeBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_TabOutLineTimeBtn = fastComponent.FastGetComponent<UIButton>("TabOutLineTimeBtn");
       if( null == m_btn_TabOutLineTimeBtn )
       {
            Engine.Utility.Log.Error("m_btn_TabOutLineTimeBtn 为空，请检查prefab是否缺乏组件");
       }
        m_label_MemberONT = fastComponent.FastGetComponent<UILabel>("MemberONT");
       if( null == m_label_MemberONT )
       {
            Engine.Utility.Log.Error("m_label_MemberONT 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberTitleA = fastComponent.FastGetComponent<Transform>("MemberTitleA");
       if( null == m_trans_MemberTitleA )
       {
            Engine.Utility.Log.Error("m_trans_MemberTitleA 为空，请检查prefab是否缺乏组件");
       }
        m_btn_applyProfBtn = fastComponent.FastGetComponent<UIButton>("applyProfBtn");
       if( null == m_btn_applyProfBtn )
       {
            Engine.Utility.Log.Error("m_btn_applyProfBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_applyNameBtn = fastComponent.FastGetComponent<UIButton>("applyNameBtn");
       if( null == m_btn_applyNameBtn )
       {
            Engine.Utility.Log.Error("m_btn_applyNameBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_applyLvBtn = fastComponent.FastGetComponent<UIButton>("applyLvBtn");
       if( null == m_btn_applyLvBtn )
       {
            Engine.Utility.Log.Error("m_btn_applyLvBtn 为空，请检查prefab是否缺乏组件");
       }
        m_btn_applyFightBtn = fastComponent.FastGetComponent<UIButton>("applyFightBtn");
       if( null == m_btn_applyFightBtn )
       {
            Engine.Utility.Log.Error("m_btn_applyFightBtn 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemberArea_Event = fastComponent.FastGetComponent<Transform>("MemberArea_Event");
       if( null == m_trans_MemberArea_Event )
       {
            Engine.Utility.Log.Error("m_trans_MemberArea_Event 为空，请检查prefab是否缺乏组件");
       }
        m_ctor_ClanHonorScrollView = fastComponent.FastGetComponent<UIGridCreatorBase>("ClanHonorScrollView");
       if( null == m_ctor_ClanHonorScrollView )
       {
            Engine.Utility.Log.Error("m_ctor_ClanHonorScrollView 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnQuitClan = fastComponent.FastGetComponent<UIButton>("BtnQuitClan");
       if( null == m_btn_BtnQuitClan )
       {
            Engine.Utility.Log.Error("m_btn_BtnQuitClan 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnChangeClan = fastComponent.FastGetComponent<UIButton>("BtnChangeClan");
       if( null == m_btn_BtnChangeClan )
       {
            Engine.Utility.Log.Error("m_btn_BtnChangeClan 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnInvite = fastComponent.FastGetComponent<UIButton>("BtnInvite");
       if( null == m_btn_BtnInvite )
       {
            Engine.Utility.Log.Error("m_btn_BtnInvite 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnMassSendMsg = fastComponent.FastGetComponent<UIButton>("BtnMassSendMsg");
       if( null == m_btn_BtnMassSendMsg )
       {
            Engine.Utility.Log.Error("m_btn_BtnMassSendMsg 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnClear = fastComponent.FastGetComponent<UIButton>("BtnClear");
       if( null == m_btn_BtnClear )
       {
            Engine.Utility.Log.Error("m_btn_BtnClear 为空，请检查prefab是否缺乏组件");
       }
        m_trans_MemmberInviteContent = fastComponent.FastGetComponent<Transform>("MemmberInviteContent");
       if( null == m_trans_MemmberInviteContent )
       {
            Engine.Utility.Log.Error("m_trans_MemmberInviteContent 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnFriend = fastComponent.FastGetComponent<UIButton>("BtnFriend");
       if( null == m_btn_BtnFriend )
       {
            Engine.Utility.Log.Error("m_btn_BtnFriend 为空，请检查prefab是否缺乏组件");
       }
        m_btn_BtnNearby = fastComponent.FastGetComponent<UIButton>("BtnNearby");
       if( null == m_btn_BtnNearby )
       {
            Engine.Utility.Log.Error("m_btn_BtnNearby 为空，请检查prefab是否缺乏组件");
       }
        m_btn_MemberInviteCollider = fastComponent.FastGetComponent<UIButton>("MemberInviteCollider");
       if( null == m_btn_MemberInviteCollider )
       {
            Engine.Utility.Log.Error("m_btn_MemberInviteCollider 为空，请检查prefab是否缺乏组件");
       }
        m_trans_RightContent = fastComponent.FastGetComponent<Transform>("RightContent");
       if( null == m_trans_RightContent )
       {
            Engine.Utility.Log.Error("m_trans_RightContent 为空，请检查prefab是否缺乏组件");
       }
        m_trans_FunctioToggles = fastComponent.FastGetComponent<Transform>("FunctioToggles");
       if( null == m_trans_FunctioToggles )
       {
            Engine.Utility.Log.Error("m_trans_FunctioToggles 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanTaskGrid = fastComponent.FastGetComponent<Transform>("UIClanTaskGrid");
       if( null == m_trans_UIClanTaskGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanTaskGrid 为空，请检查prefab是否缺乏组件");
       }
        m_label_Label = fastComponent.FastGetComponent<UILabel>("Label");
       if( null == m_label_Label )
       {
            Engine.Utility.Log.Error("m_label_Label 为空，请检查prefab是否缺乏组件");
       }
        m_label_Times = fastComponent.FastGetComponent<UILabel>("Times");
       if( null == m_label_Times )
       {
            Engine.Utility.Log.Error("m_label_Times 为空，请检查prefab是否缺乏组件");
       }
        m_slider_Expslider = fastComponent.FastGetComponent<UISlider>("Expslider");
       if( null == m_slider_Expslider )
       {
            Engine.Utility.Log.Error("m_slider_Expslider 为空，请检查prefab是否缺乏组件");
       }
        m_label_RewardTxt = fastComponent.FastGetComponent<UILabel>("RewardTxt");
       if( null == m_label_RewardTxt )
       {
            Engine.Utility.Log.Error("m_label_RewardTxt 为空，请检查prefab是否缺乏组件");
       }
        m_label_RewardTxt2 = fastComponent.FastGetComponent<UILabel>("RewardTxt2");
       if( null == m_label_RewardTxt2 )
       {
            Engine.Utility.Log.Error("m_label_RewardTxt2 为空，请检查prefab是否缺乏组件");
       }
        m_label_RewardTxt3 = fastComponent.FastGetComponent<UILabel>("RewardTxt3");
       if( null == m_label_RewardTxt3 )
       {
            Engine.Utility.Log.Error("m_label_RewardTxt3 为空，请检查prefab是否缺乏组件");
       }
        m_sprite_star = fastComponent.FastGetComponent<UISprite>("star");
       if( null == m_sprite_star )
       {
            Engine.Utility.Log.Error("m_sprite_star 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanSkillGrid = fastComponent.FastGetComponent<Transform>("UIClanSkillGrid");
       if( null == m_trans_UIClanSkillGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanSkillGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanApplyGrid = fastComponent.FastGetComponent<Transform>("UIClanApplyGrid");
       if( null == m_trans_UIClanApplyGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanApplyGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanTabGrid = fastComponent.FastGetComponent<Transform>("UIClanTabGrid");
       if( null == m_trans_UIClanTabGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanTabGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanDonateGrid = fastComponent.FastGetComponent<Transform>("UIClanDonateGrid");
       if( null == m_trans_UIClanDonateGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanDonateGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GetSW = fastComponent.FastGetComponent<Transform>("GetSW");
       if( null == m_trans_GetSW )
       {
            Engine.Utility.Log.Error("m_trans_GetSW 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GetZG = fastComponent.FastGetComponent<Transform>("GetZG");
       if( null == m_trans_GetZG )
       {
            Engine.Utility.Log.Error("m_trans_GetZG 为空，请检查prefab是否缺乏组件");
       }
        m_trans_GetZJ = fastComponent.FastGetComponent<Transform>("GetZJ");
       if( null == m_trans_GetZJ )
       {
            Engine.Utility.Log.Error("m_trans_GetZJ 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanHonorGrid = fastComponent.FastGetComponent<Transform>("UIClanHonorGrid");
       if( null == m_trans_UIClanHonorGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanHonorGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanDeclareWarGrid = fastComponent.FastGetComponent<Transform>("UIClanDeclareWarGrid");
       if( null == m_trans_UIClanDeclareWarGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanDeclareWarGrid 为空，请检查prefab是否缺乏组件");
       }
        m_trans_UIClanMemberGrid = fastComponent.FastGetComponent<Transform>("UIClanMemberGrid");
       if( null == m_trans_UIClanMemberGrid )
       {
            Engine.Utility.Log.Error("m_trans_UIClanMemberGrid 为空，请检查prefab是否缺乏组件");
       }
       if( null != fastComponent)
       {
            GameObject.Destroy(fastComponent);
       }
    }


    //注册控件事件处理函数
    protected override void RegisterControlEvents()
    {
        UIEventListener.Get(m_btn_BtnUpgrade.gameObject).onClick = _onClick_BtnUpgrade_Btn;
        UIEventListener.Get(m_btn_BtnDeclareWar.gameObject).onClick = _onClick_BtnDeclareWar_Btn;
        UIEventListener.Get(m_btn_BtnRefresh.gameObject).onClick = _onClick_BtnRefresh_Btn;
        UIEventListener.Get(m_btn_BtnEditorGG.gameObject).onClick = _onClick_BtnEditorGG_Btn;
        UIEventListener.Get(m_btn_ShengWangStoreBtn.gameObject).onClick = _onClick_ShengWangStoreBtn_Btn;
        UIEventListener.Get(m_btn_LingDiBtn.gameObject).onClick = _onClick_LingDiBtn_Btn;
        UIEventListener.Get(m_btn_BtnLearn.gameObject).onClick = _onClick_BtnLearn_Btn;
        UIEventListener.Get(m_btn_BtnDev.gameObject).onClick = _onClick_BtnDev_Btn;
        UIEventListener.Get(m_btn_TabProfBtn.gameObject).onClick = _onClick_TabProfBtn_Btn;
        UIEventListener.Get(m_btn_TabNameBtn.gameObject).onClick = _onClick_TabNameBtn_Btn;
        UIEventListener.Get(m_btn_TabLvBtn.gameObject).onClick = _onClick_TabLvBtn_Btn;
        UIEventListener.Get(m_btn_TabHonorBtn.gameObject).onClick = _onClick_TabHonorBtn_Btn;
        UIEventListener.Get(m_btn_TabDutyBtn.gameObject).onClick = _onClick_TabDutyBtn_Btn;
        UIEventListener.Get(m_btn_TabJoinTimeBtn.gameObject).onClick = _onClick_TabJoinTimeBtn_Btn;
        UIEventListener.Get(m_btn_TabOutLineTimeBtn.gameObject).onClick = _onClick_TabOutLineTimeBtn_Btn;
        UIEventListener.Get(m_btn_applyProfBtn.gameObject).onClick = _onClick_ApplyProfBtn_Btn;
        UIEventListener.Get(m_btn_applyNameBtn.gameObject).onClick = _onClick_ApplyNameBtn_Btn;
        UIEventListener.Get(m_btn_applyLvBtn.gameObject).onClick = _onClick_ApplyLvBtn_Btn;
        UIEventListener.Get(m_btn_applyFightBtn.gameObject).onClick = _onClick_ApplyFightBtn_Btn;
        UIEventListener.Get(m_btn_BtnQuitClan.gameObject).onClick = _onClick_BtnQuitClan_Btn;
        UIEventListener.Get(m_btn_BtnChangeClan.gameObject).onClick = _onClick_BtnChangeClan_Btn;
        UIEventListener.Get(m_btn_BtnInvite.gameObject).onClick = _onClick_BtnInvite_Btn;
        UIEventListener.Get(m_btn_BtnMassSendMsg.gameObject).onClick = _onClick_BtnMassSendMsg_Btn;
        UIEventListener.Get(m_btn_BtnClear.gameObject).onClick = _onClick_BtnClear_Btn;
        UIEventListener.Get(m_btn_BtnFriend.gameObject).onClick = _onClick_BtnFriend_Btn;
        UIEventListener.Get(m_btn_BtnNearby.gameObject).onClick = _onClick_BtnNearby_Btn;
        UIEventListener.Get(m_btn_MemberInviteCollider.gameObject).onClick = _onClick_MemberInviteCollider_Btn;
    }

    void _onClick_BtnUpgrade_Btn(GameObject caster)
    {
        onClick_BtnUpgrade_Btn( caster );
    }

    void _onClick_BtnDeclareWar_Btn(GameObject caster)
    {
        onClick_BtnDeclareWar_Btn( caster );
    }

    void _onClick_BtnRefresh_Btn(GameObject caster)
    {
        onClick_BtnRefresh_Btn( caster );
    }

    void _onClick_BtnEditorGG_Btn(GameObject caster)
    {
        onClick_BtnEditorGG_Btn( caster );
    }

    void _onClick_ShengWangStoreBtn_Btn(GameObject caster)
    {
        onClick_ShengWangStoreBtn_Btn( caster );
    }

    void _onClick_LingDiBtn_Btn(GameObject caster)
    {
        onClick_LingDiBtn_Btn( caster );
    }

    void _onClick_BtnLearn_Btn(GameObject caster)
    {
        onClick_BtnLearn_Btn( caster );
    }

    void _onClick_BtnDev_Btn(GameObject caster)
    {
        onClick_BtnDev_Btn( caster );
    }

    void _onClick_TabProfBtn_Btn(GameObject caster)
    {
        onClick_TabProfBtn_Btn( caster );
    }

    void _onClick_TabNameBtn_Btn(GameObject caster)
    {
        onClick_TabNameBtn_Btn( caster );
    }

    void _onClick_TabLvBtn_Btn(GameObject caster)
    {
        onClick_TabLvBtn_Btn( caster );
    }

    void _onClick_TabHonorBtn_Btn(GameObject caster)
    {
        onClick_TabHonorBtn_Btn( caster );
    }

    void _onClick_TabDutyBtn_Btn(GameObject caster)
    {
        onClick_TabDutyBtn_Btn( caster );
    }

    void _onClick_TabJoinTimeBtn_Btn(GameObject caster)
    {
        onClick_TabJoinTimeBtn_Btn( caster );
    }

    void _onClick_TabOutLineTimeBtn_Btn(GameObject caster)
    {
        onClick_TabOutLineTimeBtn_Btn( caster );
    }

    void _onClick_ApplyProfBtn_Btn(GameObject caster)
    {
        onClick_ApplyProfBtn_Btn( caster );
    }

    void _onClick_ApplyNameBtn_Btn(GameObject caster)
    {
        onClick_ApplyNameBtn_Btn( caster );
    }

    void _onClick_ApplyLvBtn_Btn(GameObject caster)
    {
        onClick_ApplyLvBtn_Btn( caster );
    }

    void _onClick_ApplyFightBtn_Btn(GameObject caster)
    {
        onClick_ApplyFightBtn_Btn( caster );
    }

    void _onClick_BtnQuitClan_Btn(GameObject caster)
    {
        onClick_BtnQuitClan_Btn( caster );
    }

    void _onClick_BtnChangeClan_Btn(GameObject caster)
    {
        onClick_BtnChangeClan_Btn( caster );
    }

    void _onClick_BtnInvite_Btn(GameObject caster)
    {
        onClick_BtnInvite_Btn( caster );
    }

    void _onClick_BtnMassSendMsg_Btn(GameObject caster)
    {
        onClick_BtnMassSendMsg_Btn( caster );
    }

    void _onClick_BtnClear_Btn(GameObject caster)
    {
        onClick_BtnClear_Btn( caster );
    }

    void _onClick_BtnFriend_Btn(GameObject caster)
    {
        onClick_BtnFriend_Btn( caster );
    }

    void _onClick_BtnNearby_Btn(GameObject caster)
    {
        onClick_BtnNearby_Btn( caster );
    }

    void _onClick_MemberInviteCollider_Btn(GameObject caster)
    {
        onClick_MemberInviteCollider_Btn( caster );
    }


}
