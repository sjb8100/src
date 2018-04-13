using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Engine;
using Engine.Utility;
using Client;
using table;


partial class HomeDataManager
{
    public enum LandState
    {
        Idle = 1,//空闲
        LockCanBuy,//可以扩建 未解锁
        LockNotBuy,//不可以扩建 未解锁
        CanGain,
        Growing,
    }
    public enum CreatureSmallState
    {
        None = 0,//没有植物 
        Seed = 1,//种子 幼崽
        Seeding,//幼苗 牲畜成长中
        Ripe,//成熟
        CanGain,//可以收获
    }
    //public enum CreatureBigState
    //{
    //    None ,
    //    Growing ,//成长
    //    CanGain ,//可以收获
    //}
    #region 属性
    FarmData m_farmData;
    int landunlockNum = 0;
    public int LandUnlockNum
    {
        get
        {
            return landunlockNum;
        }
        set
        {
            landunlockNum = value;
        }
    }

    uint plantfastripeNum = 0;
    public uint PlantFastRipeNum
    {
        get
        {
            return plantfastripeNum;
        }
        set
        {
            plantfastripeNum = value;
        }
    }

    LandState landState = LandState.Idle;
    public LandState HomeLandState
    {
        get
        {
            return landState;
        }
        set
        {
            landState = value;
        }
    }
    CreatureSmallState plantState = CreatureSmallState.None;
    public CreatureSmallState HomePlantState
    {
        get
        {
            return plantState;
        }
        set
        {
            plantState = value;
        }
    }
    List<LandData> landList = new List<LandData>();

    int selectLandIndex = 1;
    public int SelectLandIndex
    {
        get
        {
            return selectLandIndex;
        }
    }
    /// <summary>
    /// 自动播种
    /// </summary>
    bool autoPlant = false;
    public bool bAutoPlant
    {
        get
        {
            return autoPlant;
        }
        set
        {
            autoPlant = value;
        }
    }
    /// <summary>
    /// key是土地索引（1-8） 或者是动物索引 （101-108） value是种子id
    /// </summary>
    Dictionary<uint, uint> seedIndexDic = new Dictionary<uint, uint>();

    public Dictionary<uint, uint> SeedIndexDic 
    {
        get
        {
            return seedIndexDic;
        }
    }

    List<int> canGainLandList = new List<int>();
    #endregion
    void InitFarmData(FarmData data)
    {
        m_farmData = data;
        RefreshScene();
    }

    void RefreshScene()
    {
        if (m_farmData == null)
            return;
       // seedIndexDic.Clear();
        LandUnlockNum = m_farmData.unlocked;
        PlantFastRipeNum = m_farmData.fast_ripe;
        landList = m_farmData.land_list;
        foreach (var data in landList)
        {
            AddSeedToDic(data.farm_id, data.seed_id);
        }
    }
    public void GainOnePlant(int index=0)
    {
        
        if (canGainLandList.Count > 0)
        {
            
            stGainFramHomeUserCmd_CS cmd = new stGainFramHomeUserCmd_CS();

            if (index != 0)
            {
                cmd.land_id = (uint)index;
            }
            else 
            {
                cmd.land_id = (uint)canGainLandList[0];
            }
            NetService.Instance.Send(cmd);
        }
        else
        {
            TipsManager.Instance.ShowTips("没有东西可收获");
        }
    }
    public void GainAllPlant()
    {
        List<int> allIndexList = canGainLandList;
        //此处必须用一个变量缓存收获列表 因为在收获成功后 会删除canGainLandList 造成数据混乱
        for (int i = 0; i < allIndexList.Count; i++)
        {
            stGainFramHomeUserCmd_CS cmd = new stGainFramHomeUserCmd_CS();
            cmd.land_id = (uint)allIndexList[i];
            NetService.Instance.Send(cmd);
        }
    }

    public void AddCanGainLandIndex(int landIndex)
    {
        if (canGainLandList.Contains(landIndex))
        {
            //Log.Error("has cotain index " + landIndex.ToString());
        }
        else
        {
            canGainLandList.Add(landIndex);
        }
    }
    public void DeleteCanCainLand(int landIndex)
    {
        if (canGainLandList.Contains(landIndex))
        {
            canGainLandList.Remove(landIndex);
        }
        else
        {
            Log.Error("not cotain index " + landIndex.ToString());
        }
    }
    void AddSeedToDic(uint index, uint seedID)
    {
        if (seedIndexDic.ContainsKey(index))
        {
            seedIndexDic[index] = seedID;
        }
        else
        {
            seedIndexDic.Add(index, seedID);
        }
    }
    void DeleteSeedInDic(uint index)
    {
        if (seedIndexDic.ContainsKey(index))
        {
            seedIndexDic.Remove(index);
        }
    }
    
    void OnClickPlantEntity(IEntity et)
    {
        if (et == null)
        {
            Log.Error("et is null");
            return;
        }
        long uid = et.GetUID();
        if (EntityStateDic.ContainsKey(uid))
        {
            HomeEntityInfo info = EntityStateDic[uid];
            selectLandIndex = info.index;
            if (info.type == EntityType.EntityType_Soil)
            {
                if (info.index == animalYardID)
                {
                    OnClickAnimalYard();//点击牧场土地
                }
                else
                {
                    OnClickLand(info);
                    //土地
                }
            }
            if (info.type == EntityType.EntityType_Plant)
            {
                OnClickPlant(info);
            }

        }
    }
    #region protocol
    public void OnUnlockLand(stUnlockFarmHomeUserCmd_CS cmd)
    {
        TipsManager.Instance.ShowTipsById(114504);
        int index = cmd.unlock_index;
        LandUnlockNum = landunlockNum + 1;
        SetLandEntityState(index, (int)LandState.Idle);
        RefreshLandUI();
    }


    public void OnSowLand(stSowHomeUserCmd_CS cmd)
    {
        AddSeedToDic(cmd.land_id, cmd.seed_id);
        HomePosInfo pos = GetPosInfoByIndex(landID, (int)cmd.land_id);
        if (pos != null)
        {
            SeedAndCubDataBase db = GameTableManager.Instance.GetTableItem<SeedAndCubDataBase>(cmd.seed_id);
            if (db != null)
            {
                ItemDataBase idb = GameTableManager.Instance.GetTableItem<ItemDataBase>(cmd.seed_id);
                if (idb != null)
                {
                    TipsManager.Instance.ShowTipsById(114505, idb.itemName);
                    AddPlantAndAnimalModel(pos, db.growTime, cmd.seed_id);
                    SetLandEntityState((int)cmd.land_id, (int)LandState.Growing);
                    RefreshLandUI();
                }
            }
        }
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.PlantingPanel);

    }

    public void OnGainPlant(stGainFramHomeUserCmd_CS cmd)
    {
        ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>((uint)cmd.fruit_id);
        if (db != null)
        {
            TipsManager.Instance.ShowTipsById(114502, db.itemName, cmd.num);
            int seedID = (int)cmd.seed_id;
            int landIndex = (int)cmd.land_id;
            long entityID = 0;
            SetLandEntityState(landIndex, (int)LandState.Idle);
            HomeEntityInfo info = GetHomeEntityByIndex(EntityType.EntityType_Plant, landIndex, out entityID);
            if (entityID != 0)
            {
                DeleteHomeEntity(entityID);
            }
            DeleteCanCainLand(landIndex);
            DeletePlantAndAnimalRemainTime(landIndex);
        }
        RefreshLandUI();
    }

    public void OnPlantRipeAtOnce(stImmediRipeHomeUserCmd_CS cmd)
    {

        PlantFastRipeNum = (uint)cmd.fast_ripe;
        AddPlantAndAnimalRemainTime((int)cmd.land_id, 0);
        SetPlantAndAnimalEntityState((int)cmd.land_id, (int)CreatureSmallState.CanGain, EntityType.EntityType_Plant);
        SetPlantAndAnimalEntityState((int)cmd.land_id, (int)LandState.CanGain, EntityType.EntityType_Soil);
        TipsManager.Instance.ShowTipsById(114501);
        RefreshLandUI();
    }
    #endregion
    /// <summary>
    /// 设置土地状态通过土地索引
    /// </summary>
    /// <param name="landIndex">土地索引</param>
    /// <param name="state">状态</param>
    void SetLandEntityState(int landIndex, int state)
    {
        foreach (var land in entityStateDic)
        {
            HomeEntityInfo info = land.Value;
            if (info.type == EntityType.EntityType_Soil)
            {
                if (info.index == landIndex)
                {
                    SetHomeEntityState(land.Key, state, landID);
                    return;
                }
            }

        }
    }
    /// <summary>
    /// 设置植物 土地和动物的状态
    /// </summary>
    /// <param name="landIndex"></param>
    /// <param name="state"></param>
    void SetPlantAndAnimalEntityState(int landIndex, int state, EntityType type)
    {
        foreach (var land in entityStateDic)
        {
            HomeEntityInfo info = land.Value;

            if (info.index == landIndex)
            {
                if (type == EntityType.EntityType_Plant || type == EntityType.EntityType_Animal)
                {
                    uint seedID = 0;
                    if (seedIndexDic.TryGetValue((uint)landIndex, out seedID))
                    {
                        SeedAndCubDataBase sdb = GameTableManager.Instance.GetTableItem<SeedAndCubDataBase>(seedID);
                        if (sdb != null)
                        {
                            SetHomeEntityState(land.Key, state, sdb.indexID);
                            return;
                        }
                    }
                }
                else if (type == EntityType.EntityType_Soil)
                {
                    SetHomeEntityState(land.Key, state, landID);
                    return;
                }

            }

        }
    }
    void RefreshLandUI()
    {
        HomeSceneUIRoot.Instance.ShowPlantSceneUI();
    }
    void OnClickPlant(HomeEntityInfo info)
    {
        uint index = (uint)info.index;
        if (info.state != (int)CreatureSmallState.CanGain && info.state != (int)CreatureSmallState.None)
        {
            uint seedID = 0;
            if (seedIndexDic.TryGetValue(index, out seedID))
            {
                SeedAndCubDataBase sdb = GameTableManager.Instance.GetTableItem<SeedAndCubDataBase>(seedID);
                if (sdb != null && sdb.type == 0)
                {
                    uint leftTime = 0;
                    if (plantAndAnimalRemainTimeDic.TryGetValue((int)index, out leftTime))
                    {
                        
                        Double c = (double)leftTime / sdb.growUnitTime;
                        c = c * sdb.growUnitCostCoupons;
                        IncreaseDataBase idb = GameTableManager.Instance.GetTableItem<IncreaseDataBase>(1, (int)PlantFastRipeNum);
                        int count =0;
                        if (idb != null)
                        {
                            count = (int)Math.Ceiling(c * idb.increase);
                           
                        }
                        Engine.Utility.Log.LogGroup("LC","剩余时间："+leftTime.ToString()
                                                    + "    立即成熟单位时长:" + sdb.growUnitTime
                                                    + "    基础单位费用:" + sdb.growUnitCostCoupons
                                                    + "    使用次数：" + PlantFastRipeNum
                                                    + "    涨  幅：" + idb.increase.ToString()
                                                    + "    元宝数：" + count.ToString());
                        if (HasEnoughDianJuan(count))
                        {
                            string tips = DataManager.Manager<TextManager>().GetLocalFormatText(114533, count);
                            TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, tips, () =>
                            {
                                stImmediRipeHomeUserCmd_CS cmd = new stImmediRipeHomeUserCmd_CS();
                                cmd.land_id = index;
                                NetService.Instance.Send(cmd);
                                info.state = (int)CreatureSmallState.CanGain;
                            });
                        }
                        else 
                        {
                            TipsManager.Instance.ShowTips("目前还没有通用充值界面，此处后期再做处理");
                        }
                    }
                }
            }
        }
        else if (info.state == (int)CreatureSmallState.CanGain)
        {
            this.GainOnePlant(info.index);

        }
    }
    void OnClickLand(HomeEntityInfo info)
    {
        if (info.state == (int)LandState.Idle)
        {
            object data = EntityType.EntityType_Plant;
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.PlantingPanel,data:data);
        }
        else if (info.state == (int)LandState.LockCanBuy)
        {
            if (info.index == LandUnlockNum + 1)
            {
                LandAndFarmDataBase db = GameTableManager.Instance.GetTableItem<LandAndFarmDataBase>(landID, info.index);
                if (db != null)
                {
                    uint money = db.needMoneyNum;
                    string str = MainPlayerHelper.GetMoneyNameByType(db.costType);
                    if (!MainPlayerHelper.IsHasEnoughMoney(db.costType, money))
                    {
                        return;
                    }
                    if (!MainPlayerHelper.HasEnoughVipLevel(db.vipLimitLevel))
                    {
                        TipsManager.Instance.ShowTipsById(114503, db.vipLimitLevel);
                        return;
                    }

                    string tips = DataManager.Manager<TextManager>().GetLocalFormatText(114532, str, money);
                    TipsManager.Instance.ShowTipWindow(TipWindowType.CancelOk, tips, () =>
                    {
                        stUnlockFarmHomeUserCmd_CS cmd = new stUnlockFarmHomeUserCmd_CS();
                        cmd.unlock_index = info.index;
                        NetService.Instance.Send(cmd);
                    });

                }

            }

        }
    }

}

