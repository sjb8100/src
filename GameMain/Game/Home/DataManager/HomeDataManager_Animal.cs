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
    FarmData m_AnimalData;
    List<LandData> animalLandList = new List<LandData>();
    /// <summary>
    /// 动物解锁空间
    /// </summary>
    int animalunlockNum = 0;
    public int AnimalUnlockNum
    {
        get
        {
            return animalunlockNum;
        }
        set
        {
            animalunlockNum = value;
        }
    }
    uint animalfastripeNum = 0;
    public uint AnimalFastRipeNum
    {
        get
        {
            return animalfastripeNum;
        }
        set
        {
            animalfastripeNum = value;
        }
    }


    /// <summary>
    /// 可以收获的动物列表  注意索引是101开始  发给后台要减去animalIndexStart
    /// </summary>
    List<int> canGainAnimalList = new List<int>();
    void OnClickAnimalEntity(IEntity et)
    {

    }


    void InitAnimalData(FarmData data)
    {
        m_AnimalData = data;
        animalLandList = data.land_list;
        AnimalUnlockNum = data.unlocked;
        AnimalFastRipeNum = data.fast_ripe;
        foreach (var animal in animalLandList)
        {
            AddSeedToDic(animal.farm_id + animalIndexStart, animal.seed_id);
        }
    }
    #region protocol
    public void OnInitAnimalInfo(stPastureDataHomeUserCmd_S cmd)
    {
        InitAnimalData(cmd.data);
    }


    public void OnUnLockAnimalIndex(stUnlockPastureHomeUserCmd_CS cmd)
    {
        AnimalUnlockNum = (int)cmd.unlock_index;
    }


    public void OnFeedAnimal(stFeedHomeUserCmd_CS cmd)
    {
        uint animalIndex = (uint)(cmd.land_id + animalIndexStart);
        AddSeedToDic(animalIndex, cmd.seed_id);
        HomePosInfo pos = GetAnimalPos((int)cmd.land_id);
        if (pos != null)
        {
            SeedAndCubDataBase db = GameTableManager.Instance.GetTableItem<SeedAndCubDataBase>(cmd.seed_id);
            if (db != null)
            {
                ItemDataBase idb = GameTableManager.Instance.GetTableItem<ItemDataBase>(cmd.seed_id);
                if (idb != null)
                {
                    TipsManager.Instance.ShowTipsById(114505, idb.itemName);
                    AddPlantAndAnimalModel(pos, db.growTime, cmd.seed_id, EntityType.EntityType_Animal);
                    RefreshLandUI();
                }

            }

        }
    }


    public void OnGainAnimal(stGainAnimalHomeUserCmd_CS cmd)
    {
        ItemDataBase db = GameTableManager.Instance.GetTableItem<ItemDataBase>((uint)cmd.item_id1);
        if (db != null)
        {
            TipsManager.Instance.ShowTipsById(114502, db.itemName, cmd.num1);
        }
        ItemDataBase db2 = GameTableManager.Instance.GetTableItem<ItemDataBase>((uint)cmd.item_id2);
        if (db2 != null)
        {
            TipsManager.Instance.ShowTipsById(114502, db2.itemName, cmd.num2);
        }
        int seedID = (int)cmd.seed_id;
        int landIndex = (int)(cmd.land_id + animalIndexStart);
        long entityID = 0;
        HomeEntityInfo info = GetHomeEntityByIndex(EntityType.EntityType_Animal, landIndex, out entityID);
        if (entityID != 0)
        {
            DeleteHomeEntity(entityID);
        }
        DeleteCanCainLand(landIndex);
        DeletePlantAndAnimalRemainTime(landIndex);
        RefreshLandUI();
    }

    public void OnGainAnimalAtOnce(stImmediGrowHomeUserCmd_CS cmd)
    {
        AnimalFastRipeNum = (uint)cmd.fast_ripe;
        int animalIndex = (int)(cmd.land_id + animalIndexStart);
        AddPlantAndAnimalRemainTime(animalIndex, 0);
        AddCanGainLandIndex(animalIndex);
        SetPlantAndAnimalEntityState(animalIndex, (int)CreatureSmallState.CanGain, EntityType.EntityType_Animal);
        RefreshLandUI();
    }


    /// <summary>
    /// 请求扩展牧场土地
    /// </summary>
    /// <param name="id">牧场土地ID</param>
    public void ReqUnLockAnimalLand(uint landId)
    {
        stUnlockPastureHomeUserCmd_CS cmd = new stUnlockPastureHomeUserCmd_CS();
        cmd.unlock_index = landId;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 请求收获牲畜
    /// </summary>
    /// <param name="seedId"></param>
    /// <param name="landId"></param>
    public void ReqGainAnimal(uint seedId, uint landId)
    {
        stGainAnimalHomeUserCmd_CS cmd = new stGainAnimalHomeUserCmd_CS();
        cmd.seed_id = seedId;
        cmd.land_id = landId;
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 立即成熟
    /// </summary>
    /// <param name="landId"></param>
    public void ReqAtOnceGrow(uint landId)
    {
        stImmediGrowHomeUserCmd_CS cmd = new stImmediGrowHomeUserCmd_CS();
        cmd.land_id = landId;
        NetService.Instance.Send(cmd);
    }

    public void ReqGainAllAnimal() 
    {
        
    }

    #endregion

    public void AddCanGainAnimalIndex(int landIndex)
    {
        if (canGainAnimalList.Contains(landIndex))
        {
            Log.Error("has cotain index " + landIndex.ToString());
        }
        else
        {
            canGainAnimalList.Add(landIndex);
        }
    }
    void DeleteCanAnimalLand(int landIndex)
    {
        if (canGainAnimalList.Contains(landIndex))
        {
            canGainAnimalList.Remove(landIndex);
        }
        else
        {
            Log.Error("not cotain index " + landIndex.ToString());
        }
    }
    public List<HomeEntityInfo> GetAnimalList()
    {
        List<HomeEntityInfo> list = new List<HomeEntityInfo>();
        foreach (var info in entityStateDic)
        {
            if (info.Value.type == EntityType.EntityType_Animal)
            {
                list.Add(info.Value);
            }
        }
        return list;
    }

    /// <summary>
    /// 点击牧场
    /// </summary>
    void OnClickAnimalYard()
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.HomeAnimalPanel);
    }

    public void SetSelectLandIndex(int select)
    {
        selectLandIndex = select;
    }

    /// <summary>
    /// 获取牧场土地状态
    /// </summary>
    /// <param name="landId"></param>
    /// <returns></returns>
    public LandState GetAnimalLandState(uint farmId)
    {
        if (seedIndexDic.ContainsKey(farmId + animalIndexStart))
        {
            if (CanGetLeftTime((int)(farmId + animalIndexStart)))
            {
                return LandState.CanGain;
            }
            else
            {
                return LandState.Idle;
            }
        }
        else
        {
            if (farmId < animalunlockNum + 1)
            {
                return LandState.Idle;
            }
            else if (farmId == animalunlockNum + 1)
            {
                return LandState.LockCanBuy;
            }
            else
            {
                return LandState.LockNotBuy;
            }
        }
    }

}

