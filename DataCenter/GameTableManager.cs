using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using table;
using UnityEngine;
using System.Collections;

interface ITableData//<T>
{
    void Load();

    object Query(uint uID, int childID);
}
public sealed class GameTable<T> : ITableData where T : class,ProtoBuf.IExtensible
{

    public List<T> data = new List<T>();

    public void Load()
    {
        // 加载数据
        data = Table.Query<T>() as List<T>;
    }

    //常用表格缓存
    Dictionary<ulong, ProtoBuf.IExtensible> cacheDic = new Dictionary<ulong, ProtoBuf.IExtensible>();
    public object Query(uint uID, int childID)
    {
        if (data.Count == 0)
        {
            Load();
        }
        ProtoBuf.IExtensible pb = null;
        ulong pbkey = 0;
        uint secondID = 0;
        if (childID == -1)
        {
            secondID = 0;
        }
        else
        {
            secondID = (uint)childID;
        }
        ulong firstKey = (ulong)uID;
        pbkey = firstKey << 32;
        pbkey = pbkey | (uint)secondID;

        if (cacheDic.TryGetValue(pbkey, out pb))
        {
            return pb as T;
        }
        if (typeof(T) == typeof(QuestDataBase))
        {
            List<QuestDataBase> table = data as List<QuestDataBase>;
            pb = table.Query(uID);
        }
        if (typeof(T) == typeof(QuestItemDataBase))
        {
            List<QuestItemDataBase> table = data as List<QuestItemDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(GameGlobalConfig))
        {
            List<GameGlobalConfig> table = data as List<GameGlobalConfig>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(SkillDatabase))
        {
            List<SkillDatabase> table = data as List<SkillDatabase>;
            if (childID == -1)
            {
                pb = table.Query((ushort)uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query((ushort)uID, (ushort)childID);
            }
        }
        else if (typeof(T) == typeof(SkillDoubleHitDataBase))
        {
            List<SkillDoubleHitDataBase> table = data as List<SkillDoubleHitDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(BuffDataBase))
        {
            List<BuffDataBase> table = data as List<BuffDataBase>;
            if (childID == -1)
            {
                pb = table.Query(uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query(uID, (uint)childID);
            }

        }
        else if (typeof(T) == typeof(FxResDataBase))
        {
            List<FxResDataBase> table = data as List<FxResDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(ItemDataBase))
        {
            List<ItemDataBase> table = data as List<ItemDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(EquipDataBase))
        {
            List<EquipDataBase> table = data as List<EquipDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(MapDataBase))
        {
            List<MapDataBase> table = data as List<MapDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(ResourceDataBase))
        {
            List<ResourceDataBase> table = data as List<ResourceDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(NpcDataBase))
        {
            List<NpcDataBase> table = data as List<NpcDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(StateDataBase))
        {
            List<StateDataBase> table = data as List<StateDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(PetDataBase))
        {
            List<PetDataBase> table = data as List<PetDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(PetGuiYuanDataBase))
        {
            List<PetGuiYuanDataBase> table = data as List<PetGuiYuanDataBase>;
            if (childID == -1)
            {
                pb = table.Query(uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query(uID, (uint)childID);
            }
        }
        else if (typeof(T) == typeof(PetYinHunDataBase))
        {
            List<PetYinHunDataBase> table = data as List<PetYinHunDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(PetUpGradeDataBase))
        {
            List<PetUpGradeDataBase> table = data as List<PetUpGradeDataBase>;
            if (childID == -1)
            {
                pb = table.Query(uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query(uID, (uint)childID);
            }
        }
        else if (typeof(T) == typeof(PetSkillLearnDataBase))
        {
            List<PetSkillLearnDataBase> table = data as List<PetSkillLearnDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(PetSkillLevelUpDataBase))
        {
            List<PetSkillLevelUpDataBase> table = data as List<PetSkillLevelUpDataBase>;

            pb = table.Query(uID);


        }
        else if (typeof(T) == typeof(DeliverDatabase))
        {
            List<DeliverDatabase> table = data as List<DeliverDatabase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(StoreDataBase))
        {
            List<StoreDataBase> table = data as List<StoreDataBase>;
            pb = table.Query(uID);
        }
        else if (typeof(T) == typeof(UnlockStoreDataBase))
        {
            List<UnlockStoreDataBase> table = data as List<UnlockStoreDataBase>;
            pb = table.Query((ushort)uID, (ushort)childID);
        }
        else if (typeof(T) == typeof(EquipRefineDataBase))
        {
            List<EquipRefineDataBase> table = data as List<EquipRefineDataBase>;
            pb = table.Query((uint)uID, (byte)childID);
        }
        else if (typeof(T) == typeof(UpgradeDataBase))
        {
            List<UpgradeDataBase> table = data as List<UpgradeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(WeaponSoulUpgradeDataBase))
        {
            List<WeaponSoulUpgradeDataBase> table = data as List<WeaponSoulUpgradeDataBase>;
            pb = table.Query((uint)uID, (uint)childID);
        }
        else if (typeof(T) == typeof(EquipComposeDataBase))
        {
            List<EquipComposeDataBase> table = data as List<EquipComposeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(PetRecommendDataBase))
        {
            List<PetRecommendDataBase> table = data as List<PetRecommendDataBase>;
            pb = table.Query((ushort)uID, (ushort)childID);
        }
        else if (typeof(T) == typeof(RunestoneDataBase))
        {
            List<RunestoneDataBase> table = data as List<RunestoneDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(WeaponSoulGrowDataBase))
        {
            List<WeaponSoulGrowDataBase> table = data as List<WeaponSoulGrowDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(WeaponSoulPropertyGradeDataBase))
        {
            List<WeaponSoulPropertyGradeDataBase> table = data as List<WeaponSoulPropertyGradeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(PointConsumeDataBase))
        {
            List<PointConsumeDataBase> table = data as List<PointConsumeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RideDataBase))
        {
            List<RideDataBase> table = data as List<RideDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RideFeedData))
        {
            List<RideFeedData> table = data as List<RideFeedData>;
            pb = table.Query((uint)uID, (byte)childID);
        }
        else if (typeof(T) == typeof(RideExpandData))
        {
            List<RideExpandData> table = data as List<RideExpandData>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RideSkillData))
        {
            List<RideSkillData> table = data as List<RideSkillData>;
            if (childID == -1)
            {
                pb = table.Query(uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query(uID, (uint)childID);
            }
        }
        else if (typeof(T) == typeof(RideSkillDes))
        {
            List<RideSkillDes> table = data as List<RideSkillDes>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GemComposeDataBase))
        {
            List<GemComposeDataBase> table = data as List<GemComposeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GemPropertyDataBase))
        {
            List<GemPropertyDataBase> table = data as List<GemPropertyDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(MailIconDataBase))
        {
            List<MailIconDataBase> table = data as List<MailIconDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(HomeLandViewDatabase))
        {
            List<HomeLandViewDatabase> table = data as List<HomeLandViewDatabase>;
            if (childID == -1)
            {
                pb = table.Query((ushort)uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query((ushort)uID, (ushort)childID);
            }
        }
        else if (typeof(T) == typeof(HomeTradeDataBase))
        {
            List<HomeTradeDataBase> table = data as List<HomeTradeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(EffectFuncDataBase))
        {
            List<EffectFuncDataBase> table = data as List<EffectFuncDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RSPropertyPromoteDataBase))
        {
            List<RSPropertyPromoteDataBase> table = data as List<RSPropertyPromoteDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(MineDataBase))
        {
            List<MineDataBase> table = data as List<MineDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(WishingTreeDataBase))
        {
            List<WishingTreeDataBase> table = data as List<WishingTreeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(SeedAndCubDataBase))
        {
            List<SeedAndCubDataBase> table = data as List<SeedAndCubDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(CompassDataBase))
        {
            List<CompassDataBase> table = data as List<CompassDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(LandAndFarmDataBase))
        {
            List<LandAndFarmDataBase> table = data as List<LandAndFarmDataBase>;
            if (childID == -1)
            {
                pb = table.Query((ushort)uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query((ushort)uID, (ushort)childID);
            }
        }
        else if (typeof(T) == typeof(IncreaseDataBase))
        {
            List<IncreaseDataBase> table = data as List<IncreaseDataBase>;
            if (childID == -1)
            {
                pb = table.Query((ushort)uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query((ushort)uID, (ushort)childID);
            }
        }
        else if (typeof(T) == typeof(ArenaClearCDCostDataBase))
        {
            List<ArenaClearCDCostDataBase> table = data as List<ArenaClearCDCostDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ArenaResetCostDataBase))
        {
            List<ArenaResetCostDataBase> table = data as List<ArenaResetCostDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RankDataBase))
        {
            List<RankDataBase> table = data as List<RankDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ScheduleDataBase))
        {
            List<ScheduleDataBase> table = data as List<ScheduleDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(HomeLandRecycleDatabase))
        {
            List<HomeLandRecycleDatabase> table = data as List<HomeLandRecycleDatabase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(EquipSplitDataBase))
        {
            List<EquipSplitDataBase> table = data as List<EquipSplitDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(CopyDataBase))
        {
            List<CopyDataBase> table = data as List<CopyDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(AcceptTokenDataBase))
        {
            List<AcceptTokenDataBase> table = data as List<AcceptTokenDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(PublicTokenDataBase))
        {
            List<PublicTokenDataBase> table = data as List<PublicTokenDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(TransferDatabase))
        {
            List<TransferDatabase> table = data as List<TransferDatabase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ClanMemberDataBase))
        {
            List<ClanMemberDataBase> table = data as List<ClanMemberDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ClanDonateDataBase))
        {
            List<ClanDonateDataBase> table = data as List<ClanDonateDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ClanUpgradeDataBase))
        {
            List<ClanUpgradeDataBase> table = data as List<ClanUpgradeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ClanSkillDataBase))
        {
            List<ClanSkillDataBase> table = data as List<ClanSkillDataBase>;
            pb = table.Query((uint)uID, (uint)childID);
        }
        else if (typeof(T) == typeof(ClanDutyNameDataBase))
        {
            List<ClanDutyNameDataBase> table = data as List<ClanDutyNameDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ClanDutyPermDataBase))
        {
            List<ClanDutyPermDataBase> table = data as List<ClanDutyPermDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(LangTextDataBase))
        {
            List<LangTextDataBase> table = data as List<LangTextDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(NobleDataBase))
        {
            List<NobleDataBase> table = data as List<NobleDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(TeamActivityDatabase))
        {
            List<TeamActivityDatabase> table = data as List<TeamActivityDatabase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(TitleDataBase))
        {
            List<TitleDataBase> table = data as List<TitleDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(CompassDataBase))
        {
            List<CompassDataBase> table = data as List<CompassDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ComposeDataBase))
        {
            List<ComposeDataBase> table = data as List<ComposeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RechargeDataBase))
        {
            List<RechargeDataBase> table = data as List<RechargeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(DailyDataBase))
        {
            List<DailyDataBase> table = data as List<DailyDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(DailyAwardDataBase))
        {
            List<DailyAwardDataBase> table = data as List<DailyAwardDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(WelfareDataBase))
        {
            List<WelfareDataBase> table = data as List<WelfareDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(HeartSkillDataBase))
        {
            List<HeartSkillDataBase> table = data as List<HeartSkillDataBase>;
            if (childID == -1)
            {
                pb = table.Query((uint)uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query((uint)uID, (uint)childID);
            }
        }
        else if (typeof(T) == typeof(SensitiveWordDataBase))
        {
            List<SensitiveWordDataBase> table = data as List<SensitiveWordDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(LocalTextDataBase))
        {
            List<LocalTextDataBase> table = data as List<LocalTextDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(SevenDataBase))
        {
            List<SevenDataBase> table = data as List<SevenDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(HuntingDataBase))
        {
            List<HuntingDataBase> table = data as List<HuntingDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(StarQuestDataBase))
        {
            List<StarQuestDataBase> table = data as List<StarQuestDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(SuitDataBase)) // 时装配置表
        {
            List<SuitDataBase> table = data as List<SuitDataBase>;
            if (childID == -1)
            {
                pb = table.Query((uint)uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query((uint)uID, (uint)childID);
            }
        }
        else if (typeof(T) == typeof(NpcCampDataBase)) // 
        {
            List<NpcCampDataBase> table = data as List<NpcCampDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ItemBindDataBase))
        {
            List<ItemBindDataBase> table = data as List<ItemBindDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(MonsterWaveDatabase))
        {
            List<MonsterWaveDatabase> table = data as List<MonsterWaveDatabase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(NWGuardDataBase))
        {
            List<NWGuardDataBase> table = data as List<NWGuardDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RechargeCostDataBase))
        {
            List<RechargeCostDataBase> table = data as List<RechargeCostDataBase>;

            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(CopyDisplayDataBase))
        {
            List<CopyDisplayDataBase> table = data as List<CopyDisplayDataBase>;
            if (childID == -1)
            {
                pb = table.Query((uint)uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query((uint)uID, (uint)childID);
            }
        }
        else if (typeof(T) == typeof(AchievementDataBase))
        {
            List<AchievementDataBase> table = data as List<AchievementDataBase>;

            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ItemGetDataBase))
        {
            List<ItemGetDataBase> table = data as List<ItemGetDataBase>;

            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ItemUseTypeDataBase))
        {
            List<ItemUseTypeDataBase> table = data as List<ItemUseTypeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(LoadingTipsDatabase))
        {
            List<LoadingTipsDatabase> table = data as List<LoadingTipsDatabase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RankTypeDataBase))
        {
            List<RankTypeDataBase> table = data as List<RankTypeDataBase>;
            if (childID == -1)
            {
                pb = table.Query((uint)uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query((uint)uID, (uint)childID);
            }
        }
        else if (typeof(T) == typeof(PreloadResDataBase))
        {
            List<PreloadResDataBase> table = data as List<PreloadResDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(PreloadSkillResDataBase))
        {
            List<PreloadSkillResDataBase> table = data as List<PreloadSkillResDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(NewFUNCOpenDataBase))
        {
            List<NewFUNCOpenDataBase> table = data as List<NewFUNCOpenDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GuideDataBase))
        {
            List<GuideDataBase> table = data as List<GuideDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GuideTriggerCondiDataBase))
        {
            List<GuideTriggerCondiDataBase> table = data as List<GuideTriggerCondiDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ReliveDataBase))
        {
            List<ReliveDataBase> table = data as List<ReliveDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(SequencerData))
        {
            List<SequencerData> table = data as List<SequencerData>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ShowEffect))
        {
            List<ShowEffect> table = data as List<ShowEffect>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(SequencerDialog))
        {
            List<SequencerDialog> table = data as List<SequencerDialog>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(BibleDataBase))
        {
            List<BibleDataBase> table = data as List<BibleDataBase>;
            if (childID == -1)
            {
                pb = table.Query(uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query(uID, (uint)childID);
            }

        }
        else if (typeof(T) == typeof(HotKeyDataBase))
        {
            List<HotKeyDataBase> table = data as List<HotKeyDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(PanelInfoDataBase))
        {
            List<PanelInfoDataBase> table = data as List<PanelInfoDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ChapterDabaBase))
        {
            List<ChapterDabaBase> table = data as List<ChapterDabaBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ItemJumpDataBase))
        {
            List<ItemJumpDataBase> table = data as List<ItemJumpDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(SettingDataBase))
        {
            List<SettingDataBase> table = data as List<SettingDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RandomTargetDataBase))
        {
            List<RandomTargetDataBase> table = data as List<RandomTargetDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ArenaRankRewardDatabase))
        {
            List<ArenaRankRewardDatabase> table = data as List<ArenaRankRewardDatabase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RobotDataBase))
        {
            List<RobotDataBase> table = data as List<RobotDataBase>;
            if (childID == -1)
            {
                pb = table.Query(uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query(uID, (uint)childID);
            }

        }
        else if (typeof(T) == typeof(JumpWayDataBase))
        {
            List<JumpWayDataBase> table = data as List<JumpWayDataBase>;
            pb = table.Query((uint)uID);

        }
        else if (typeof(T) == typeof(ConsignmentCanSellItem))
        {
            List<ConsignmentCanSellItem> table = data as List<ConsignmentCanSellItem>;
            pb = table.Query((uint)uID);

        }
        else if (typeof(T) == typeof(SelectRoleDataBase))
        {
            List<SelectRoleDataBase> table = data as List<SelectRoleDataBase>;
            if (childID == -1)
            {
                pb = table.Query(uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query(uID, (uint)childID);
            }
        }
        else if (typeof(T) == typeof(GrowUpFightPowerDabaBase))
        {
            List<GrowUpFightPowerDabaBase> table = data as List<GrowUpFightPowerDabaBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GrowUpFightPowerLevelDabaBase))
        {
            List<GrowUpFightPowerLevelDabaBase> table = data as List<GrowUpFightPowerLevelDabaBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GrowUpDabaBase))
        {
            List<GrowUpDabaBase> table = data as List<GrowUpDabaBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GrowUpRecommendFightPowerDabaBase))
        {
            List<GrowUpRecommendFightPowerDabaBase> table = data as List<GrowUpRecommendFightPowerDabaBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(QuestionDataBase))
        {
            List<QuestionDataBase> table = data as List<QuestionDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(UIResourceDataBase))
        {
            List<UIResourceDataBase> table = data as List<UIResourceDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GridStrengthenDataBase))
        {
            List<GridStrengthenDataBase> table = data as List<GridStrengthenDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GridStrengthenSuitDataBase))
        {
            List<GridStrengthenSuitDataBase> table = data as List<GridStrengthenSuitDataBase>;
            pb = table.Query((uint)uID, (uint)childID);
        }
        else if (typeof(T) == typeof(NpcHeadMaskDataBase))
        {
            List<NpcHeadMaskDataBase> table = data as List<NpcHeadMaskDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RewardFindDataBase))
        {
            List<RewardFindDataBase> table = data as List<RewardFindDataBase>;
            pb = table.Query((uint)uID, (uint)childID);
        }
        else if (typeof(T) == typeof(TabFuncDataBase))
        {
            List<TabFuncDataBase> table = data as List<TabFuncDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GodDemonDataBase))
        {
            List<GodDemonDataBase> table = data as List<GodDemonDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(GemSuitDataBase))
        {
            List<GemSuitDataBase> table = data as List<GemSuitDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ColorSuitDataBase))
        {
            List<ColorSuitDataBase> table = data as List<ColorSuitDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(BossAIDataBase))
        {
            List<BossAIDataBase> table = data as List<BossAIDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(PetInHeritDataBase))
        {
            List<PetInHeritDataBase> table = data as List<PetInHeritDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(TransferChildTypeDataBase))
        {
            List<TransferChildTypeDataBase> table = data as List<TransferChildTypeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(EquipExchangeDataBase))
        {
            List<EquipExchangeDataBase> table = data as List<EquipExchangeDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(CopyTargetDataBase))
        {
            List<CopyTargetDataBase> table = data as List<CopyTargetDataBase>;
            if (childID == -1)
            {
                pb = table.Query((uint)uID).FirstOrDefault();
            }
            else
            {
                pb = table.Query((uint)uID, (uint)childID);
            }
        }
        else if (typeof(T) == typeof(CopyTargetGuideDataBase))
        {
            List<CopyTargetGuideDataBase> table = data as List<CopyTargetGuideDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(CangBaoTuDataBase))
        {
            List<CangBaoTuDataBase> table = data as List<CangBaoTuDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(UpgradeAddDataBase))
        {
            List<UpgradeAddDataBase> table = data as List<UpgradeAddDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(DailyTestDataBase))
        {
            List<DailyTestDataBase> table = data as List<DailyTestDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(DailyCalendarDataBase))
        {
            List<DailyCalendarDataBase> table = data as List<DailyCalendarDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(DailyAnswerDatabase))
        {
            List<DailyAnswerDatabase> table = data as List<DailyAnswerDatabase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(OpenServerDataBase))
        {
            List<OpenServerDataBase> table = data as List<OpenServerDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ArtifactDataBase))
        {
            List<ArtifactDataBase> table = data as List<ArtifactDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ShowModelDataBase))
        {
            List<ShowModelDataBase> table = data as List<ShowModelDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(FishingDataBase))
        {
            List<FishingDataBase> table = data as List<FishingDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(FirstRechargeRewardDataBase))
        {
            List<FirstRechargeRewardDataBase> table = data as List<FirstRechargeRewardDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(ModeDiplayDataBase))
        {
            List<ModeDiplayDataBase> table = data as List<ModeDiplayDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(BossTalkDataBase))
        {
            List<BossTalkDataBase> table = data as List<BossTalkDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(CityWarDataBase))
        {
            List<CityWarDataBase> table = data as List<CityWarDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(HoursemanShipUPDegree))
        {
            List<HoursemanShipUPDegree> table = data as List<HoursemanShipUPDegree>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(HoursemanShipUPLevel))
        {
            List<HoursemanShipUPLevel> table = data as List<HoursemanShipUPLevel>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(HoursemanShipUPLevel))
        {
            List<HoursemanShipUPLevel> table = data as List<HoursemanShipUPLevel>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(FrameEffectDataBase))
        {
            List<FrameEffectDataBase> table = data as List<FrameEffectDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(CollectWordDataBase))
        {
            List<CollectWordDataBase> table = data as List<CollectWordDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(TreasureBossDataBase))
        {
            List<TreasureBossDataBase> table = data as List<TreasureBossDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(RightOrWrongDataBase))
        {
            List<RightOrWrongDataBase> table = data as List<RightOrWrongDataBase>;
            pb = table.Query((uint)uID);
        }
        else if (typeof(T) == typeof(InspireDataBase))
        {
            List<InspireDataBase> table = data as List<InspireDataBase>;
            pb = table.Query((uint)uID, (uint)childID);
        }
        if (pb != null)
        {
            cacheDic.Add(pbkey, pb);
            return pb as T;
        }
        return default(T);
    }
}

public sealed class GameTableManager
{
    private const string CLINET_GLOBAL_CONST = "common/tab/ClientGlobal.ini";

    private static GameTableManager instance = null;
    public bool IsConnectBwt = true;

    class TableDesc
    {
        public Type t;
        public ITableData data;
    }
    // 类型初始化
    private Dictionary<Type, TableDesc> m_dicTableDesc = new Dictionary<Type, TableDesc>();
    // 客户端全局常量配置
    private Engine.Utility.IniFile m_ClientGlobalConst = new Engine.Utility.IniFile();
    // 服务器通用全局常量配置
    private Engine.JsonNode m_GlobalConstRoot = null;



    private GameTableManager()
    {
        m_dicTableDesc.Add(typeof(QuestDataBase), new TableDesc { t = typeof(QuestDataBase), data = new GameTable<QuestDataBase>() });
        m_dicTableDesc.Add(typeof(QuestItemDataBase), new TableDesc { t = typeof(QuestItemDataBase), data = new GameTable<QuestItemDataBase>() });
        m_dicTableDesc.Add(typeof(GameGlobalConfig), new TableDesc { t = typeof(GameGlobalConfig), data = new GameTable<GameGlobalConfig>() });
        m_dicTableDesc.Add(typeof(SkillDatabase), new TableDesc { t = typeof(SkillDatabase), data = new GameTable<SkillDatabase>() });
        m_dicTableDesc.Add(typeof(SelectRoleDataBase), new TableDesc { t = typeof(SelectRoleDataBase), data = new GameTable<SelectRoleDataBase>() });
        m_dicTableDesc.Add(typeof(SkillDoubleHitDataBase), new TableDesc { t = typeof(SkillDoubleHitDataBase), data = new GameTable<SkillDoubleHitDataBase>() });
        m_dicTableDesc.Add(typeof(BuffDataBase), new TableDesc { t = typeof(BuffDataBase), data = new GameTable<BuffDataBase>() });
        m_dicTableDesc.Add(typeof(FxResDataBase), new TableDesc { t = typeof(FxResDataBase), data = new GameTable<FxResDataBase>() });
        m_dicTableDesc.Add(typeof(ItemDataBase), new TableDesc { t = typeof(ItemDataBase), data = new GameTable<ItemDataBase>() });
        m_dicTableDesc.Add(typeof(EquipDataBase), new TableDesc { t = typeof(EquipDataBase), data = new GameTable<EquipDataBase>() });
        m_dicTableDesc.Add(typeof(MapDataBase), new TableDesc { t = typeof(MapDataBase), data = new GameTable<MapDataBase>() });
        m_dicTableDesc.Add(typeof(ResourceDataBase), new TableDesc { t = typeof(ResourceDataBase), data = new GameTable<ResourceDataBase>() });
        m_dicTableDesc.Add(typeof(NpcDataBase), new TableDesc { t = typeof(NpcDataBase), data = new GameTable<NpcDataBase>() });
        m_dicTableDesc.Add(typeof(StateDataBase), new TableDesc { t = typeof(StateDataBase), data = new GameTable<StateDataBase>() });
        m_dicTableDesc.Add(typeof(DeliverDatabase), new TableDesc { t = typeof(DeliverDatabase), data = new GameTable<DeliverDatabase>() });
        m_dicTableDesc.Add(typeof(PetDataBase), new TableDesc { t = typeof(PetDataBase), data = new GameTable<PetDataBase>() });
        m_dicTableDesc.Add(typeof(PetGuiYuanDataBase), new TableDesc { t = typeof(PetGuiYuanDataBase), data = new GameTable<PetGuiYuanDataBase>() });
        m_dicTableDesc.Add(typeof(PetYinHunDataBase), new TableDesc { t = typeof(PetYinHunDataBase), data = new GameTable<PetYinHunDataBase>() });
        m_dicTableDesc.Add(typeof(PetUpGradeDataBase), new TableDesc { t = typeof(PetUpGradeDataBase), data = new GameTable<PetUpGradeDataBase>() });
        m_dicTableDesc.Add(typeof(PetSkillLearnDataBase), new TableDesc { t = typeof(PetSkillLearnDataBase), data = new GameTable<PetSkillLearnDataBase>() });
        m_dicTableDesc.Add(typeof(PetSkillLevelUpDataBase), new TableDesc { t = typeof(PetSkillLevelUpDataBase), data = new GameTable<PetSkillLevelUpDataBase>() });
        m_dicTableDesc.Add(typeof(StoreDataBase), new TableDesc { t = typeof(StoreDataBase), data = new GameTable<StoreDataBase>() });
        m_dicTableDesc.Add(typeof(UnlockStoreDataBase), new TableDesc { t = typeof(UnlockStoreDataBase), data = new GameTable<UnlockStoreDataBase>() });
        m_dicTableDesc.Add(typeof(EquipRefineDataBase), new TableDesc { t = typeof(EquipRefineDataBase), data = new GameTable<EquipRefineDataBase>() });
        m_dicTableDesc.Add(typeof(UpgradeDataBase), new TableDesc { t = typeof(UpgradeDataBase), data = new GameTable<UpgradeDataBase>() });
        m_dicTableDesc.Add(typeof(WeaponSoulUpgradeDataBase), new TableDesc { t = typeof(WeaponSoulUpgradeDataBase), data = new GameTable<WeaponSoulUpgradeDataBase>() });
        m_dicTableDesc.Add(typeof(EquipComposeDataBase), new TableDesc { t = typeof(EquipComposeDataBase), data = new GameTable<EquipComposeDataBase>() });
        m_dicTableDesc.Add(typeof(PetRecommendDataBase), new TableDesc { t = typeof(PetRecommendDataBase), data = new GameTable<PetRecommendDataBase>() });
        m_dicTableDesc.Add(typeof(RunestoneDataBase), new TableDesc { t = typeof(RunestoneDataBase), data = new GameTable<RunestoneDataBase>() });
        m_dicTableDesc.Add(typeof(WeaponSoulGrowDataBase), new TableDesc { t = typeof(WeaponSoulGrowDataBase), data = new GameTable<WeaponSoulGrowDataBase>() });
        m_dicTableDesc.Add(typeof(RideDataBase), new TableDesc { t = typeof(RideDataBase), data = new GameTable<RideDataBase>() });
        m_dicTableDesc.Add(typeof(RideFeedData), new TableDesc { t = typeof(RideFeedData), data = new GameTable<RideFeedData>() });
        m_dicTableDesc.Add(typeof(RideExpandData), new TableDesc { t = typeof(RideExpandData), data = new GameTable<RideExpandData>() });
        m_dicTableDesc.Add(typeof(RideSkillData), new TableDesc { t = typeof(RideSkillData), data = new GameTable<RideSkillData>() });
        m_dicTableDesc.Add(typeof(RideSkillDes), new TableDesc { t = typeof(RideSkillDes), data = new GameTable<RideSkillDes>() });
        m_dicTableDesc.Add(typeof(WeaponSoulPropertyGradeDataBase), new TableDesc { t = typeof(WeaponSoulPropertyGradeDataBase), data = new GameTable<WeaponSoulPropertyGradeDataBase>() });
        m_dicTableDesc.Add(typeof(PointConsumeDataBase), new TableDesc { t = typeof(PointConsumeDataBase), data = new GameTable<PointConsumeDataBase>() });
        m_dicTableDesc.Add(typeof(GemComposeDataBase), new TableDesc { t = typeof(GemComposeDataBase), data = new GameTable<GemComposeDataBase>() });
        m_dicTableDesc.Add(typeof(GemPropertyDataBase), new TableDesc { t = typeof(GemPropertyDataBase), data = new GameTable<GemPropertyDataBase>() });
        m_dicTableDesc.Add(typeof(MailIconDataBase), new TableDesc { t = typeof(MailIconDataBase), data = new GameTable<MailIconDataBase>() });
        m_dicTableDesc.Add(typeof(HomeLandViewDatabase), new TableDesc { t = typeof(HomeLandViewDatabase), data = new GameTable<HomeLandViewDatabase>() });
        m_dicTableDesc.Add(typeof(EffectFuncDataBase), new TableDesc { t = typeof(EffectFuncDataBase), data = new GameTable<EffectFuncDataBase>() });
        m_dicTableDesc.Add(typeof(RSPropertyPromoteDataBase), new TableDesc { t = typeof(RSPropertyPromoteDataBase), data = new GameTable<RSPropertyPromoteDataBase>() });
        m_dicTableDesc.Add(typeof(MineDataBase), new TableDesc { t = typeof(MineDataBase), data = new GameTable<MineDataBase>() });
        m_dicTableDesc.Add(typeof(WishingTreeDataBase), new TableDesc { t = typeof(WishingTreeDataBase), data = new GameTable<WishingTreeDataBase>() });
        m_dicTableDesc.Add(typeof(SeedAndCubDataBase), new TableDesc { t = typeof(SeedAndCubDataBase), data = new GameTable<SeedAndCubDataBase>() });
        m_dicTableDesc.Add(typeof(LandAndFarmDataBase), new TableDesc { t = typeof(LandAndFarmDataBase), data = new GameTable<LandAndFarmDataBase>() });
        m_dicTableDesc.Add(typeof(CompassDataBase), new TableDesc { t = typeof(CompassDataBase), data = new GameTable<CompassDataBase>() });
        m_dicTableDesc.Add(typeof(IncreaseDataBase), new TableDesc { t = typeof(IncreaseDataBase), data = new GameTable<IncreaseDataBase>() });
        m_dicTableDesc.Add(typeof(ArenaClearCDCostDataBase), new TableDesc { t = typeof(ArenaClearCDCostDataBase), data = new GameTable<ArenaClearCDCostDataBase>() });
        m_dicTableDesc.Add(typeof(ArenaResetCostDataBase), new TableDesc { t = typeof(ArenaResetCostDataBase), data = new GameTable<ArenaResetCostDataBase>() });
        m_dicTableDesc.Add(typeof(ScheduleDataBase), new TableDesc { t = typeof(ScheduleDataBase), data = new GameTable<ScheduleDataBase>() });
        m_dicTableDesc.Add(typeof(HomeLandRecycleDatabase), new TableDesc { t = typeof(HomeLandRecycleDatabase), data = new GameTable<HomeLandRecycleDatabase>() });
        m_dicTableDesc.Add(typeof(HomeTradeDataBase), new TableDesc { t = typeof(HomeTradeDataBase), data = new GameTable<HomeTradeDataBase>() });
        m_dicTableDesc.Add(typeof(EquipSplitDataBase), new TableDesc { t = typeof(EquipSplitDataBase), data = new GameTable<EquipSplitDataBase>() });
        m_dicTableDesc.Add(typeof(CopyDataBase), new TableDesc { t = typeof(CopyDataBase), data = new GameTable<CopyDataBase>() });
        m_dicTableDesc.Add(typeof(PublicTokenDataBase), new TableDesc { t = typeof(PublicTokenDataBase), data = new GameTable<PublicTokenDataBase>() });
        m_dicTableDesc.Add(typeof(AcceptTokenDataBase), new TableDesc { t = typeof(AcceptTokenDataBase), data = new GameTable<AcceptTokenDataBase>() });
        m_dicTableDesc.Add(typeof(RankDataBase), new TableDesc { t = typeof(RankDataBase), data = new GameTable<RankDataBase>() });
        m_dicTableDesc.Add(typeof(TransferDatabase), new TableDesc { t = typeof(TransferDatabase), data = new GameTable<TransferDatabase>() });
        m_dicTableDesc.Add(typeof(ClanMemberDataBase), new TableDesc { t = typeof(ClanMemberDataBase), data = new GameTable<ClanMemberDataBase>() });
        m_dicTableDesc.Add(typeof(ClanDonateDataBase), new TableDesc { t = typeof(ClanDonateDataBase), data = new GameTable<ClanDonateDataBase>() });
        m_dicTableDesc.Add(typeof(ClanUpgradeDataBase), new TableDesc { t = typeof(ClanUpgradeDataBase), data = new GameTable<ClanUpgradeDataBase>() });
        m_dicTableDesc.Add(typeof(ClanSkillDataBase), new TableDesc { t = typeof(ClanSkillDataBase), data = new GameTable<ClanSkillDataBase>() });
        m_dicTableDesc.Add(typeof(ClanDutyNameDataBase), new TableDesc { t = typeof(ClanDutyNameDataBase), data = new GameTable<ClanDutyNameDataBase>() });
        m_dicTableDesc.Add(typeof(ClanDutyPermDataBase), new TableDesc { t = typeof(ClanDutyPermDataBase), data = new GameTable<ClanDutyPermDataBase>() });
        m_dicTableDesc.Add(typeof(LangTextDataBase), new TableDesc { t = typeof(LangTextDataBase), data = new GameTable<LangTextDataBase>() });
        m_dicTableDesc.Add(typeof(NobleDataBase), new TableDesc { t = typeof(NobleDataBase), data = new GameTable<NobleDataBase>() });
        m_dicTableDesc.Add(typeof(TeamActivityDatabase), new TableDesc { t = typeof(TeamActivityDatabase), data = new GameTable<TeamActivityDatabase>() });
        m_dicTableDesc.Add(typeof(TitleDataBase), new TableDesc { t = typeof(TitleDataBase), data = new GameTable<TitleDataBase>() });
        m_dicTableDesc.Add(typeof(ComposeDataBase), new TableDesc { t = typeof(ComposeDataBase), data = new GameTable<ComposeDataBase>() });
        m_dicTableDesc.Add(typeof(RechargeDataBase), new TableDesc { t = typeof(RechargeDataBase), data = new GameTable<RechargeDataBase>() });
        m_dicTableDesc.Add(typeof(DailyDataBase), new TableDesc { t = typeof(DailyDataBase), data = new GameTable<DailyDataBase>() });
        m_dicTableDesc.Add(typeof(DailyAwardDataBase), new TableDesc { t = typeof(DailyAwardDataBase), data = new GameTable<DailyAwardDataBase>() });
        m_dicTableDesc.Add(typeof(WelfareDataBase), new TableDesc { t = typeof(WelfareDataBase), data = new GameTable<WelfareDataBase>() });
        m_dicTableDesc.Add(typeof(SensitiveWordDataBase), new TableDesc { t = typeof(SensitiveWordDataBase), data = new GameTable<SensitiveWordDataBase>() });
        m_dicTableDesc.Add(typeof(HeartSkillDataBase), new TableDesc { t = typeof(HeartSkillDataBase), data = new GameTable<HeartSkillDataBase>() });
        m_dicTableDesc.Add(typeof(LocalTextDataBase), new TableDesc { t = typeof(LocalTextDataBase), data = new GameTable<LocalTextDataBase>() });
        m_dicTableDesc.Add(typeof(SevenDataBase), new TableDesc { t = typeof(SevenDataBase), data = new GameTable<SevenDataBase>() });
        m_dicTableDesc.Add(typeof(HuntingDataBase), new TableDesc { t = typeof(HuntingDataBase), data = new GameTable<HuntingDataBase>() });
        m_dicTableDesc.Add(typeof(StarQuestDataBase), new TableDesc { t = typeof(StarQuestDataBase), data = new GameTable<StarQuestDataBase>() });
        m_dicTableDesc.Add(typeof(SuitDataBase), new TableDesc { t = typeof(SuitDataBase), data = new GameTable<SuitDataBase>() });
        m_dicTableDesc.Add(typeof(NpcCampDataBase), new TableDesc { t = typeof(NpcCampDataBase), data = new GameTable<NpcCampDataBase>() });
        m_dicTableDesc.Add(typeof(ItemBindDataBase), new TableDesc { t = typeof(ItemBindDataBase), data = new GameTable<ItemBindDataBase>() });
        m_dicTableDesc.Add(typeof(MonsterWaveDatabase), new TableDesc { t = typeof(MonsterWaveDatabase), data = new GameTable<MonsterWaveDatabase>() });
        m_dicTableDesc.Add(typeof(NWGuardDataBase), new TableDesc { t = typeof(NWGuardDataBase), data = new GameTable<NWGuardDataBase>() });
        m_dicTableDesc.Add(typeof(RechargeCostDataBase), new TableDesc { t = typeof(RechargeCostDataBase), data = new GameTable<RechargeCostDataBase>() });
        m_dicTableDesc.Add(typeof(CopyDisplayDataBase), new TableDesc { t = typeof(CopyDisplayDataBase), data = new GameTable<CopyDisplayDataBase>() });
        m_dicTableDesc.Add(typeof(AchievementDataBase), new TableDesc { t = typeof(AchievementDataBase), data = new GameTable<AchievementDataBase>() });
        m_dicTableDesc.Add(typeof(ItemGetDataBase), new TableDesc { t = typeof(ItemGetDataBase), data = new GameTable<ItemGetDataBase>() });
        m_dicTableDesc.Add(typeof(ItemUseTypeDataBase), new TableDesc { t = typeof(ItemUseTypeDataBase), data = new GameTable<ItemUseTypeDataBase>() });
        m_dicTableDesc.Add(typeof(LoadingTipsDatabase), new TableDesc { t = typeof(LoadingTipsDatabase), data = new GameTable<LoadingTipsDatabase>() });
        m_dicTableDesc.Add(typeof(RankTypeDataBase), new TableDesc { t = typeof(RankTypeDataBase), data = new GameTable<RankTypeDataBase>() });
        m_dicTableDesc.Add(typeof(PreloadResDataBase), new TableDesc { t = typeof(PreloadResDataBase), data = new GameTable<PreloadResDataBase>() });
        m_dicTableDesc.Add(typeof(PreloadSkillResDataBase), new TableDesc { t = typeof(PreloadSkillResDataBase), data = new GameTable<PreloadSkillResDataBase>() });
        m_dicTableDesc.Add(typeof(NewFUNCOpenDataBase), new TableDesc { t = typeof(NewFUNCOpenDataBase), data = new GameTable<NewFUNCOpenDataBase>() });
        m_dicTableDesc.Add(typeof(GuideDataBase), new TableDesc { t = typeof(GuideDataBase), data = new GameTable<GuideDataBase>() });
        m_dicTableDesc.Add(typeof(GuideTriggerCondiDataBase), new TableDesc { t = typeof(GuideTriggerCondiDataBase), data = new GameTable<GuideTriggerCondiDataBase>() });
        m_dicTableDesc.Add(typeof(ReliveDataBase), new TableDesc { t = typeof(ReliveDataBase), data = new GameTable<ReliveDataBase>() });
        m_dicTableDesc.Add(typeof(SequencerData), new TableDesc { t = typeof(SequencerData), data = new GameTable<SequencerData>() });
        m_dicTableDesc.Add(typeof(ShowEffect), new TableDesc { t = typeof(ShowEffect), data = new GameTable<ShowEffect>() });
        m_dicTableDesc.Add(typeof(SequencerDialog), new TableDesc { t = typeof(SequencerDialog), data = new GameTable<SequencerDialog>() });
        m_dicTableDesc.Add(typeof(BibleDataBase), new TableDesc { t = typeof(BibleDataBase), data = new GameTable<BibleDataBase>() });
        m_dicTableDesc.Add(typeof(HotKeyDataBase), new TableDesc { t = typeof(HotKeyDataBase), data = new GameTable<HotKeyDataBase>() });
        m_dicTableDesc.Add(typeof(PanelInfoDataBase), new TableDesc { t = typeof(PanelInfoDataBase), data = new GameTable<PanelInfoDataBase>() });
        m_dicTableDesc.Add(typeof(ChapterDabaBase), new TableDesc { t = typeof(ChapterDabaBase), data = new GameTable<ChapterDabaBase>() });
        m_dicTableDesc.Add(typeof(ItemJumpDataBase), new TableDesc { t = typeof(ItemJumpDataBase), data = new GameTable<ItemJumpDataBase>() });
        m_dicTableDesc.Add(typeof(SettingDataBase), new TableDesc { t = typeof(SettingDataBase), data = new GameTable<SettingDataBase>() });
        m_dicTableDesc.Add(typeof(RandomTargetDataBase), new TableDesc { t = typeof(RandomTargetDataBase), data = new GameTable<RandomTargetDataBase>() });
        m_dicTableDesc.Add(typeof(ArenaRankRewardDatabase), new TableDesc { t = typeof(ArenaRankRewardDatabase), data = new GameTable<ArenaRankRewardDatabase>() });
        m_dicTableDesc.Add(typeof(RobotDataBase), new TableDesc { t = typeof(RobotDataBase), data = new GameTable<RobotDataBase>() });
        m_dicTableDesc.Add(typeof(JumpWayDataBase), new TableDesc { t = typeof(JumpWayDataBase), data = new GameTable<JumpWayDataBase>() });
        m_dicTableDesc.Add(typeof(ConsignmentCanSellItem), new TableDesc { t = typeof(ConsignmentCanSellItem), data = new GameTable<ConsignmentCanSellItem>() });
        m_dicTableDesc.Add(typeof(GrowUpFightPowerDabaBase), new TableDesc { t = typeof(GrowUpFightPowerDabaBase), data = new GameTable<GrowUpFightPowerDabaBase>() });
        m_dicTableDesc.Add(typeof(GrowUpFightPowerLevelDabaBase), new TableDesc { t = typeof(GrowUpFightPowerLevelDabaBase), data = new GameTable<GrowUpFightPowerLevelDabaBase>() });
        m_dicTableDesc.Add(typeof(GrowUpDabaBase), new TableDesc { t = typeof(GrowUpDabaBase), data = new GameTable<GrowUpDabaBase>() });
        m_dicTableDesc.Add(typeof(GrowUpRecommendFightPowerDabaBase), new TableDesc { t = typeof(GrowUpRecommendFightPowerDabaBase), data = new GameTable<GrowUpRecommendFightPowerDabaBase>() });
        m_dicTableDesc.Add(typeof(QuestionDataBase), new TableDesc { t = typeof(QuestionDataBase), data = new GameTable<QuestionDataBase>() });
        m_dicTableDesc.Add(typeof(UIResourceDataBase), new TableDesc { t = typeof(UIResourceDataBase), data = new GameTable<UIResourceDataBase>() });
        m_dicTableDesc.Add(typeof(GridStrengthenDataBase), new TableDesc { t = typeof(GridStrengthenDataBase), data = new GameTable<GridStrengthenDataBase>() });
        m_dicTableDesc.Add(typeof(GridStrengthenSuitDataBase), new TableDesc { t = typeof(GridStrengthenSuitDataBase), data = new GameTable<GridStrengthenSuitDataBase>() });
        m_dicTableDesc.Add(typeof(NpcHeadMaskDataBase), new TableDesc { t = typeof(NpcHeadMaskDataBase), data = new GameTable<NpcHeadMaskDataBase>() });
        m_dicTableDesc.Add(typeof(RewardFindDataBase), new TableDesc { t = typeof(RewardFindDataBase), data = new GameTable<RewardFindDataBase>() });
        m_dicTableDesc.Add(typeof(TabFuncDataBase), new TableDesc { t = typeof(TabFuncDataBase), data = new GameTable<TabFuncDataBase>() });
        m_dicTableDesc.Add(typeof(GodDemonDataBase), new TableDesc { t = typeof(GodDemonDataBase), data = new GameTable<GodDemonDataBase>() });
        m_dicTableDesc.Add(typeof(ColorSuitDataBase), new TableDesc { t = typeof(ColorSuitDataBase), data = new GameTable<ColorSuitDataBase>() });
        m_dicTableDesc.Add(typeof(GemSuitDataBase), new TableDesc { t = typeof(GemSuitDataBase), data = new GameTable<GemSuitDataBase>() });
        m_dicTableDesc.Add(typeof(BossAIDataBase), new TableDesc { t = typeof(BossAIDataBase), data = new GameTable<BossAIDataBase>() });
        m_dicTableDesc.Add(typeof(PetInHeritDataBase), new TableDesc { t = typeof(PetInHeritDataBase), data = new GameTable<PetInHeritDataBase>() });
        m_dicTableDesc.Add(typeof(TransferChildTypeDataBase), new TableDesc { t = typeof(TransferChildTypeDataBase), data = new GameTable<TransferChildTypeDataBase>() });
        m_dicTableDesc.Add(typeof(EquipExchangeDataBase), new TableDesc { t = typeof(EquipExchangeDataBase), data = new GameTable<EquipExchangeDataBase>() });
        m_dicTableDesc.Add(typeof(CopyTargetDataBase), new TableDesc { t = typeof(CopyTargetDataBase), data = new GameTable<CopyTargetDataBase>() });
        m_dicTableDesc.Add(typeof(CangBaoTuDataBase), new TableDesc { t = typeof(CangBaoTuDataBase), data = new GameTable<CangBaoTuDataBase>() });
        m_dicTableDesc.Add(typeof(UpgradeAddDataBase), new TableDesc { t = typeof(UpgradeAddDataBase), data = new GameTable<UpgradeAddDataBase>() });
        m_dicTableDesc.Add(typeof(DailyTestDataBase), new TableDesc { t = typeof(DailyTestDataBase), data = new GameTable<DailyTestDataBase>() });
        m_dicTableDesc.Add(typeof(DailyCalendarDataBase), new TableDesc { t = typeof(DailyCalendarDataBase), data = new GameTable<DailyCalendarDataBase>() });
        m_dicTableDesc.Add(typeof(DailyAnswerDatabase), new TableDesc { t = typeof(DailyAnswerDatabase), data = new GameTable<DailyAnswerDatabase>() });
        m_dicTableDesc.Add(typeof(OpenServerDataBase), new TableDesc { t = typeof(OpenServerDataBase), data = new GameTable<OpenServerDataBase>() });
        m_dicTableDesc.Add(typeof(ArtifactDataBase), new TableDesc { t = typeof(ArtifactDataBase), data = new GameTable<ArtifactDataBase>() });
        m_dicTableDesc.Add(typeof(ShowModelDataBase), new TableDesc { t = typeof(ShowModelDataBase), data = new GameTable<ShowModelDataBase>() });
        m_dicTableDesc.Add(typeof(FishingDataBase), new TableDesc { t = typeof(FishingDataBase), data = new GameTable<FishingDataBase>() });
        m_dicTableDesc.Add(typeof(FirstRechargeRewardDataBase), new TableDesc { t = typeof(FirstRechargeRewardDataBase), data = new GameTable<FirstRechargeRewardDataBase>() });
        m_dicTableDesc.Add(typeof(BossTalkDataBase), new TableDesc { t = typeof(BossTalkDataBase), data = new GameTable<BossTalkDataBase>() });
        m_dicTableDesc.Add(typeof(ModeDiplayDataBase), new TableDesc { t = typeof(ModeDiplayDataBase), data = new GameTable<ModeDiplayDataBase>() });
        m_dicTableDesc.Add(typeof(CityWarDataBase), new TableDesc { t = typeof(CityWarDataBase), data = new GameTable<CityWarDataBase>() });
        m_dicTableDesc.Add(typeof(CopyTargetGuideDataBase), new TableDesc { t = typeof(CopyTargetGuideDataBase), data = new GameTable<CopyTargetGuideDataBase>() });
        m_dicTableDesc.Add(typeof(HoursemanShipUPDegree), new TableDesc { t = typeof(HoursemanShipUPDegree), data = new GameTable<HoursemanShipUPDegree>() });
        m_dicTableDesc.Add(typeof(HoursemanShipUPLevel), new TableDesc { t = typeof(HoursemanShipUPLevel), data = new GameTable<HoursemanShipUPLevel>() });
        m_dicTableDesc.Add(typeof(FrameEffectDataBase), new TableDesc { t = typeof(FrameEffectDataBase), data = new GameTable<FrameEffectDataBase>() });
        m_dicTableDesc.Add(typeof(CollectWordDataBase), new TableDesc { t = typeof(CollectWordDataBase), data = new GameTable<CollectWordDataBase>() });
        m_dicTableDesc.Add(typeof(TreasureBossDataBase), new TableDesc { t = typeof(TreasureBossDataBase), data = new GameTable<TreasureBossDataBase>() });
        m_dicTableDesc.Add(typeof(InspireDataBase), new TableDesc { t = typeof(InspireDataBase), data = new GameTable<InspireDataBase>() });
        m_dicTableDesc.Add(typeof(RightOrWrongDataBase), new TableDesc { t = typeof(RightOrWrongDataBase), data = new GameTable<RightOrWrongDataBase>() });
    }

    public static GameTableManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameTableManager();
            }
            return instance;
        }
    }

    public void LoadGloabalConst()
    {
        if (m_ClientGlobalConst == null)
        {
            return;
        }

        if (!m_ClientGlobalConst.Open(CLINET_GLOBAL_CONST))
        {
            Engine.Utility.Log.Error("ClientGlobal加载失败!!");
        }

        string path = Table.GetGlobalPath();
        m_GlobalConstRoot = Engine.RareJson.ParseJsonFile(path);
        if (m_GlobalConstRoot == null)
        {
            Engine.Utility.Log.Error("GlobalConst加载失败!!");
        }
    }

    // 加载配置表
    public void LoadTable(Action<float> progress = null)
    {
        Dictionary<Type, TableDesc>.Enumerator iter = m_dicTableDesc.GetEnumerator();
        int totalNum = m_dicTableDesc.Count;
        int loadCount = 0;
        if (totalNum == 0 && null != progress)
        {
            progress.Invoke(1f);
            return;
        }
        while (iter.MoveNext())
        {
            if (iter.Current.Value.data != null)
            {
                //iter.Current.Value.data.Load();
            }
            loadCount++;
            if (null != progress)
            {
                progress.Invoke(loadCount / (float)totalNum);
            }
        }

        Engine.CorotinueInstance.Instance.StartCoroutine(LoadPreData());

    }
    List<Type> m_perLoadDataList = new List<Type>
    {
        typeof(SkillDatabase),
        typeof(EquipRefineDataBase),
        typeof(RandomNameDataBase),
        typeof(ItemDataBase),
        typeof(UIResourceDataBase),
        typeof(GridStrengthenDataBase),
        typeof(QuestDataBase),
        typeof(StateDataBase),
        typeof(LangTextDataBase),
        typeof(NpcDataBase),
        typeof(BuffDataBase),
    };
    IEnumerator LoadPreData()
    {
        foreach (var t in m_perLoadDataList)
        {
            if (!m_dicTableDesc.ContainsKey(t))
            {
                continue;
            }
            var item = m_dicTableDesc[t];
           
            item.data.Load();
            yield return new WaitForSeconds(1f);
        }

    }
    Dictionary<Type, IList> m_listDic = new Dictionary<Type, IList>();
    /// <summary>
    /// 获取表格列表
    /// </summary>
    /// <typeparam name="T">表格类型</typeparam>
    /// <param name="name">表格名字</param>
    /// <returns></returns>
    public List<T> GetTableList<T>() where T : ProtoBuf.IExtensible
    {
     
        Type t = typeof(T);
        if(m_listDic.ContainsKey(t))
        {
            return m_listDic[t] as List<T>;
        }
        else
        {
            List<T> list = Table.Query<T>();
            m_listDic.Add(t, list);
            return list;
        }
    }
    /// <summary>
    ///二分查找 获取一列数据
    /// </summary>
    /// <typeparam name="T">表格类型</typeparam>
    /// <param name="nIndex">表格索引</param>
    /// <param name="childID"></param> 第二Key
    /// <returns></returns>
    public T GetTableItem<T>(uint nIndex, int childID = -1) where T : class,ProtoBuf.IExtensible
    {
        Type t = typeof(T);
        TableDesc desc = null;
        if (!m_dicTableDesc.TryGetValue(t, out desc))
        {
            Log.Error("表格{0}未找到索引 {1}", t.Name, nIndex.ToString());
            return default(T);
        }

        return desc.data.Query(nIndex, childID) as T;
    }

    /**
    @brief 获取客户端全局常量 只支持整数和字符串
    @param strKey 系统模块key
    @param strName 常量名称
    */


    //本地缓存数据
    private Dictionary<string, Dictionary<string, object>> m_dicTypeData = new Dictionary<string, Dictionary<string, object>>();
    private Dictionary<string, object> m_dicTempType = new Dictionary<string, object>();
    public T GetClientGlobalConst<T>(string strKey, string strName)
    {
        if (m_ClientGlobalConst == null)
        {
            return default(T);
        }
        Type t = typeof(T);
        object value = null;
        if (m_dicTypeData.ContainsKey(strKey))
        {
            if (m_dicTypeData[strKey].ContainsKey(strName))
            {
                value = m_dicTypeData[strKey][strName];
            }
            else
            {
                if (t == typeof(int))
                {
                    int v = m_ClientGlobalConst.GetInt(strKey, strName, 0);
                    value = Convert.ChangeType(v, t);
                }
                else if (t == typeof(uint))
                {
                    uint v = (uint)m_ClientGlobalConst.GetInt(strKey, strName, 0);
                    value = Convert.ChangeType(v, t);
                }
                else if (t == typeof(string))
                {
                    string v = m_ClientGlobalConst.GetString(strKey, strName, "");
                    value = Convert.ChangeType(v, t);
                }
                m_dicTypeData[strKey].Add(strName, value);
            }
        }
        else
        {
            m_dicTempType.Clear();
            if (t == typeof(int))
            {
                int v = m_ClientGlobalConst.GetInt(strKey, strName, 0);
                value = Convert.ChangeType(v, t);
            }
            else if (t == typeof(uint))
            {
                uint v = (uint)m_ClientGlobalConst.GetInt(strKey, strName, 0);
                value = Convert.ChangeType(v, t);
            }
            else if (t == typeof(string))
            {
                string v = m_ClientGlobalConst.GetString(strKey, strName, "");
                value = Convert.ChangeType(v, t);
            }
            m_dicTempType.Add(strName, value);
            m_dicTypeData.Add(strKey, m_dicTempType);
        }
        if (value != null)
        {
            return (T)value;
        }
        return default(T);
    }


    public List<string> GetGlobalConfigKeyList(string firstKey)
    {
        string path = Table.GetGlobalPath();
        List<string> keyList = null;
        Engine.JsonNode node = Engine.RareJson.ParseJsonFile(path);

        Engine.JsonNode jv = node[firstKey];
        if (jv != null)
        {
            Engine.JsonObject obj = jv as Engine.JsonObject;
            keyList = new List<string>(obj.Keys);

        }


        return keyList;
    }
    /// <summary>
    /// 获取json全局配置数组类型
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="firstKey"></param>
    /// <param name="secondKey"></param>
    /// <returns></returns>
    /// 
    Dictionary<string, List<object>> m_dicObj = new Dictionary<string, List<object>>();
    Dictionary<string, Dictionary<string, List<object>>> m_DicTwoKey = new Dictionary<string, Dictionary<string, List<object>>>();

    List<T> TranslateJsonToObject<T>(string firstKey, string secondKey = null)
    {
        string path = Table.GetGlobalPath();
        List<object> objList = null;
        Type t = typeof(T);
        object value = null;
        List<T> tList = null;
        Engine.JsonNode node = m_GlobalConstRoot;
        if (node == null)
        {
            Engine.Utility.Log.Error("GlobalConfig:{0}不存在！", path);
        }
        if (string.IsNullOrEmpty(secondKey))
        {
            if (m_dicObj.ContainsKey(firstKey))
            {
                objList = m_dicObj[firstKey];
            }
            else
            {
                Engine.JsonNode jv = node[firstKey];
                if (jv != null)
                {
                    if (jv.Type == Engine.JsonValueType.JsonValueType_Array)
                    {
                        objList = new List<object>(10);
                        Engine.JsonArray ja = jv.AsArray;
                        for (int i = 0; i < ja.Count; i++)
                        {
                            value = Convert.ChangeType(ja[i].Value, t);
                            objList.Add(value);
                        }
                        m_dicObj.Add(firstKey, objList);
                    }
                    else
                    {
                        Log.Error("not json array");
                    }
                }
                else
                {
                    Log.Error("global json don't contain firstkey " + firstKey);
                }
            }
            tList = new List<T>();
            for (int i = 0; i < objList.Count; i++)
            {
                tList.Add((T)objList[i]);
            }
            return tList;
        }
        else
        {
            if (m_DicTwoKey.ContainsKey(firstKey))
            {
                if (m_DicTwoKey[firstKey].ContainsKey(secondKey))
                {
                    objList = m_DicTwoKey[firstKey][secondKey];
                }
                else
                {
                    Engine.JsonNode fv = node[firstKey];
                    if (fv != null)
                    {
                        Engine.JsonNode jv = fv[secondKey];
                        if (jv != null)
                        {
                            if (jv.Type == Engine.JsonValueType.JsonValueType_Array)
                            {
                                Engine.JsonArray ja = jv.AsArray;
                                objList = new List<object>(10);
                                for (int i = 0; i < ja.Count; i++)
                                {
                                    value = Convert.ChangeType(ja[i].Value, t);
                                    objList.Add(value);
                                }
                                m_DicTwoKey[firstKey].Add(secondKey, objList);
                            }
                        }
                    }
                }
            }
            else
            {
                Engine.JsonNode fv = node[firstKey];
                if (fv != null)
                {
                    Engine.JsonNode jv = fv[secondKey];
                    if (jv != null)
                    {
                        if (jv.Type == Engine.JsonValueType.JsonValueType_Array)
                        {
                            Engine.JsonArray ja = jv.AsArray;
                            objList = new List<object>(10);
                            for (int i = 0; i < ja.Count; i++)
                            {
                                value = Convert.ChangeType(ja[i].Value, t);
                                objList.Add(value);
                            }
                        }
                    }

                }
                m_DicTwoKey.Add(firstKey, new Dictionary<string, List<object>> { { secondKey, objList } });
            }
            tList = new List<T>();
            for (int i = 0; i < objList.Count; i++)
            {
                tList.Add((T)objList[i]);
            }
            return tList;
        }
    }
    public List<T> GetGlobalConfigList<T>(string firstKey, string secondKey = null)
    {
        return TranslateJsonToObject<T>(firstKey, secondKey);
    }
    /// <summary>
    ///  获取json全局配置
    /// </summary>
    /// <typeparam name="T">返回值类型</typeparam>
    /// <param name="firstKey"></param>
    /// <param name="secondKey"></param>
    /// <returns></returns>
    /// 
    Dictionary<string, Dictionary<string, object>> m_dicAct = new Dictionary<string, Dictionary<string, object>>();
    Dictionary<string, object> m_tempDic = new Dictionary<string, object>();
    public T GetGlobalConfig<T>(string firstKey, string secondKey = null)
    {
        string path = Table.GetGlobalPath();
        Engine.JsonNode node = m_GlobalConstRoot;
        object value = null;
        Type t = typeof(T);
        if (node == null)
        {
            Engine.Utility.Log.Error("GlobalConfig:{0}不存在！", path);
            return default(T);
        }

        if (string.IsNullOrEmpty(secondKey))
        {
            if (m_tempDic.ContainsKey(firstKey))
            {
                value = m_tempDic[firstKey];
            }
            else
            {
                Engine.JsonNode jv = node[firstKey];
                if (jv != null)
                {
                    if (jv.Type != Engine.JsonValueType.JsonValueType_Array)
                    {
                        value = Convert.ChangeType(jv.Value, t);
                        m_tempDic.Add(firstKey, value);
                    }
                }
                else
                {
                    Log.Error("global json don't contain firstkey " + firstKey);
                }
            }
            if (value != null)
            {
                return (T)value;
            }
        }
        else
        {
            if (m_dicAct.ContainsKey(firstKey))
            {
                if (m_dicAct[firstKey].ContainsKey(secondKey))
                {
                    value = m_dicAct[firstKey][secondKey];
                }
                else
                {
                    Engine.JsonNode fv = node[firstKey];
                    if (fv != null)
                    {
                        Engine.JsonNode jv = fv[secondKey];
                        if (jv != null)
                        {

                            if (jv.Type != Engine.JsonValueType.JsonValueType_Array)
                            {

                                value = Convert.ChangeType(jv.Value, t);
                                m_dicAct[firstKey].Add(secondKey, value);
                            }
                        }
                    }
                }

            }
            else
            {
                Engine.JsonNode fv = node[firstKey];
                if (fv != null)
                {
                    Engine.JsonNode jv = fv[secondKey];
                    if (jv != null)
                    {
                        if (jv.Type != Engine.JsonValueType.JsonValueType_Array)
                        {
                            value = Convert.ChangeType(jv.Value, t);
                            m_dicAct.Add(firstKey, new Dictionary<string, object>() 
                            {
                                 {secondKey,value}                         
                            });
                        }
                    }
                    else
                    {
                        Log.Error("global json don't contain secondKey " + secondKey);
                    }
                }
                else
                {
                    Log.Error("global json don't contain firstKey " + firstKey);
                }
            }
            try
            {
                if (value != null)
                {
                    return (T)value;
                }
            }
            catch (Exception e)
            {
                //通过字典取出来的obj,之前存的是int,你第二次用同样的key来取一个uint?
                Engine.Utility.Log.Error("拆箱失败，目标类型和装箱之前的原始数据类型不对应");
            }



        }
        return default(T);
    }
}

