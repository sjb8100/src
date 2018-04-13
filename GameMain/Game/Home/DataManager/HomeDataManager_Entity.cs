using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using GameCmd;
using table;
using Engine;
using Engine.Utility;
using UnityEngine;

partial class HomeDataManager
{
    //进入家园初始位置
    private const string CONST_HOMEPLAYERPOS_NAME = "HomePlayerPos";
    public static List<int> HomePlayerPos
    {
        get
        {
            return GameTableManager.Instance.GetGlobalConfigList<int>(CONST_HOMEPLAYERPOS_NAME);
        }
    }

    IPuppet mainPlayer;
    public IPuppet PupPlayer
    {
        get
        {
            return mainPlayer;
        }
    }

    HomeControlPanel HomeControlPanel 
    {
        get 
        {
            return DataManager.Manager<UIPanelManager>().GetPanel(PanelID.HomeControlPanel) as HomeControlPanel;
        }
    }

    public class HomeEntityInfo
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int index = -1;
        /// <summary>
        /// 状态
        /// </summary>
        public int state = -1;

        public long entityID = 0;

        public EntityType type = EntityType.EntityType_Null;
    }
    /// <summary>
    /// 实体状态字典 key is uid value is state
    /// </summary>
    Dictionary<long , HomeEntityInfo> entityStateDic = new Dictionary<long , HomeEntityInfo>();
    public Dictionary<long , HomeEntityInfo> EntityStateDic
    {
        get
        {
            return entityStateDic;
        }
    }
    /// <summary>
    ///植物和动物的成熟剩余时间 key is landIndex and value is left time for ripe
    /// </summary>
    Dictionary<int, uint> plantAndAnimalRemainTimeDic = new Dictionary<int, uint>();
    private Dictionary<int, uint> PlantAndAnimalRemainTimeDic
    {
        get
        {
            return plantAndAnimalRemainTimeDic;
        }
    }
    /// <summary>
    /// 动物索引 101-108  植物是1-8都放在plantRemainTimeDic
    /// </summary>
    uint animalIndexStart = 100;
    void OnEnterScene()
    {
        ShowUI();
        CreatePlayer();
        CreatePlant();
        CreateMineEntity();
        CreateAnimals();
        CreateTree();
        HomeSceneUIRoot.Instance.ShowPlantSceneUI();
        HomeControlPanel.UpdateHomeControlUI();
    }

    void ShowUI()
    {
        MainPanel mp = DataManager.Manager<UIPanelManager>().GetPanel(PanelID.MainPanel) as MainPanel;
        if (mp != null)
        {
            mp.DoHideSkill();
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.HomeControlPanel);
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.HomeFriendPanel);
    }


    void CreatePlayer()
    {
        IPlayer player = ClientGlobal.Instance().MainPlayer;
        if (player != null)
        {
            int job = player.GetProp((int)PlayerProp.Job);
            int sex = player.GetProp((int)PlayerProp.Sex);
            int index = 0;
            EntityViewProp[] propList = new EntityViewProp[(int)Client.EquipPos.EquipPos_Max];
            propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Body, 0);
            propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Weapon, (int)0);
            //propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Wing, (int)0);

            mainPlayer = HomeScene.Instance.AddPuppet(player.GetName(), job, sex, propList, true);
            if (mainPlayer != null && HomePlayerPos.Count == 2)
            {
                mainPlayer.SendMessage(EntityMessage.EntityCommand_SetPos, new Vector3(HomePlayerPos[0], 0f, -HomePlayerPos[1]));
            }

        }
    }
    #region animals
    void CreateAnimals()
    {
        //List<HomePosInfo> posList = GetPosListByModuleID( 304 );
        //if ( posList == null )
        //    return;

        List<HomePosInfo> posList = GetPosListByModuleID(animalID);//获取土地位置
        HomeLandViewDatabase db = GameTableManager.Instance.GetTableItem<HomeLandViewDatabase>(animalID);
        if (db != null)
        {
            for (int i = 0; i < posList.Count; i++)
            {
                HomePosInfo plant = posList[i];

                AddAnimalYardModel(plant, LandState.Idle);
            }
      
        }


        for (int i = 0; i < animalLandList.Count; i++)
        {
            LandData ld = animalLandList[i];

            HomePosInfo info = GetAnimalPos(i + 1);
            AddPlantAndAnimalModel(info, ld.remain_time, ld.seed_id, EntityType.EntityType_Animal);
        }

    }
    HomePosInfo GetAnimalPos(int index)
    {
        int x = 28, y = 65;
        int rd = UnityEngine.Random.Range(-5, 5);
        HomePosInfo info = new HomePosInfo();
        info.index = index + (int)animalIndexStart;//index 从101开始
        info.posX = x + rd;
        info.posZ = y + rd;
        return info;
    }
    #endregion
    #region wishtree
    void CreateTree()
    {
        IEntity en =null;
        HomeEntityInfo info=null;
        List<HomePosInfo> posList = GetPosListByModuleID( ironTreeID );
        if ( posList == null )
            return;
        foreach (var plant in posList)
        {
            en = HomeScene.Instance.AddEntity(plant.index.ToString(), EntityType.EntityType_Tree, ironTreeID, (int)TreeState.Iron);
            if (en != null)
            {
                en.SendMessage(EntityMessage.EntityCommand_SetPos, new Vector3(plant.posX, 3f, -plant.posZ));
                info = new HomeEntityInfo();
                info.index = plant.index;
                info.state = (int)TreeState.Iron;
                info.type = EntityType.EntityType_Tree;
                info.entityID = en.GetUID();
                AddHomeEntity( en.GetUID() , info );          
            }
        }

    }
    #endregion
    #region mine
    void CreateMineEntity()
    {
        List<HomePosInfo> normalList = GetPosListByModuleID(mineModuleID);

        foreach (var pos in normalList)
        {
            HomeMineState st = HomeMineState.Lock;
            if (pos.index == 1)
            {
                st = HomeMineState.CanFind;
            }
            IEntity en = HomeScene.Instance.AddEntity(pos.index.ToString(), EntityType.EntityType_Minerals, mineModuleID, (int)st);
            if (en != null)
            {
                en.SendMessage(EntityMessage.EntityCommand_SetPos, new Vector3(pos.posX, 3f, -pos.posZ));
                HomeEntityInfo info = new HomeEntityInfo();
                info.index = pos.index;
                info.state = (int)st;
                info.type = EntityType.EntityType_Minerals;
                info.entityID = en.GetUID();
                AddHomeEntity(en.GetUID(), info);
            }
        }
    }
    #endregion
    #region farm and animal
    void ShowSoilModel(HomePosInfo plant, LandData ld)
    {
        LandState st = LandState.LockCanBuy;
        if (ld != null)
        {
            if (ld.farm_id == plant.index)
            {
                st = LandState.Idle;
                if (ld.land_state == 0 && ld.plant_begin != 0)
                {
                    if (ld.remain_time == 0)
                    { 
                    st = LandState.CanGain;
                    }
                    if (ld.remain_time != 0)
                    {
                        st = LandState.Growing;
                    }
                }
            }
        }
        AddSoilModel(plant, st);
    }
    void AddSoilModel(HomePosInfo plant, LandState st)
    {
        IEntity en = HomeScene.Instance.AddEntity(plant.index.ToString(), EntityType.EntityType_Soil, landID, (int)st);
        if (en != null)
        {
            en.SendMessage(EntityMessage.EntityCommand_SetPos, new Vector3(plant.posX, 3f, -plant.posZ));
            HomeEntityInfo info = new HomeEntityInfo();
            info.index = plant.index;
            info.state = (int)st;
            info.type = EntityType.EntityType_Soil;
            info.entityID = en.GetUID();
            AddHomeEntity(en.GetUID(), info);
        }
    }

    void AddAnimalYardModel(HomePosInfo plant, LandState st)
    {
        IEntity en = HomeScene.Instance.AddEntity(plant.index.ToString(), EntityType.EntityType_Soil, animalID, (int)st);
        if (en != null)
        {
            en.SendMessage(EntityMessage.EntityCommand_SetPos, new Vector3(plant.posX, 2.966f, -plant.posZ));
            HomeEntityInfo info = new HomeEntityInfo();
            info.index = animalYardID; // plant.index;
            info.state = (int)st;
            info.type = EntityType.EntityType_Soil;
            info.entityID = en.GetUID();
            AddHomeEntity(en.GetUID(), info);
        }

    }

    CreatureSmallState GetPlantStateByLandData(uint seedID, uint leftTime)
    {
        CreatureSmallState st = CreatureSmallState.Seed;
        SeedAndCubDataBase sdb = GameTableManager.Instance.GetTableItem<SeedAndCubDataBase>(seedID);
        if (sdb != null)
        {
            uint totalTime = sdb.growTime;

            if (leftTime > totalTime * 0.7f)                                   //70% -- 100% 种子
            {
                st = CreatureSmallState.Seed;
            }
            else if (leftTime > totalTime * 0.3 && leftTime < totalTime * 0.7) //30% -- 70% 幼苗
            {
                st = CreatureSmallState.Seeding;
            }
            else if (leftTime < totalTime * 0.3f && leftTime > 0)             // 0% -- 30%  成熟
            {
                st = CreatureSmallState.Ripe;
            }
            else if (leftTime == 0)
            {
                st = CreatureSmallState.CanGain;                             //可收获
            }

        }
        return st;
    }
    void AddPlantAndAnimalModel(HomePosInfo plant, uint leftTime, uint seedID, EntityType type = EntityType.EntityType_Plant)
    {
        if (seedID != 0)
        {
            CreatureSmallState st = GetPlantStateByLandData(seedID, leftTime);
            SeedAndCubDataBase db = GameTableManager.Instance.GetTableItem<SeedAndCubDataBase>(seedID);
            if (db == null)
            {
                Log.Error("种子没找到");
                return;
            }
            IEntity sen = HomeScene.Instance.AddEntity(plant.index.ToString(), type, db.indexID, (int)st);
            if (sen != null)
            {
                sen.SendMessage(EntityMessage.EntityCommand_SetPos, new Vector3(plant.posX, 2.97f, -plant.posZ));
                HomeEntityInfo info = new HomeEntityInfo();
                info.index = plant.index;
                info.state = (int)st;
                info.type = type;
                info.entityID = sen.GetUID();
                AddHomeEntity(sen.GetUID(), info);
                AddPlantAndAnimalRemainTime(plant.index, leftTime);
            }

        }
    }
    void ShowPlantModel(HomePosInfo plant, LandData ld)
    {
        AddPlantAndAnimalModel(plant, ld.remain_time, ld.seed_id);

    }
    void CreatePlant()
    {
        List<HomePosInfo> posList = GetPosListByModuleID(landID);
        HomeLandViewDatabase db = GameTableManager.Instance.GetTableItem<HomeLandViewDatabase>(landID);
        if (db != null)
        {
            for (int i = 0; i < posList.Count; i++)
            {
                HomePosInfo plant = posList[i];
                if (i < landList.Count)
                {
                    LandData ld = landList[i];
                    ShowSoilModel(plant, ld);
                    ShowPlantModel(plant, ld);
                }
                else
                {
                    ShowSoilModel(plant, null);
                }
            }

        }

    }

    public uint GetAnimalIndexStart()
    {
        return animalIndexStart;
    }

    #endregion
    #region 剩余时间
    /// <summary>
    /// 保存动物和农场植物的剩余时间
    /// </summary>
    /// <param name="landIndex">植物索引是1-8 动物索引是101-108</param>
    /// <param name="leftTime"></param>
    public void AddPlantAndAnimalRemainTime(int landIndex, uint leftTime)
    {
        if (plantAndAnimalRemainTimeDic.ContainsKey(landIndex))
        {
            plantAndAnimalRemainTimeDic[landIndex] = leftTime;
        }
        else
        {
            plantAndAnimalRemainTimeDic.Add(landIndex, leftTime);
        }
    }
    void DeletePlantAndAnimalRemainTime(int landIndex)
    {
        if (plantAndAnimalRemainTimeDic.ContainsKey(landIndex))
        {
            plantAndAnimalRemainTimeDic.Remove(landIndex);
        }
    }
    /// <summary>
    /// 能否获取剩余时间
    /// </summary>
    /// <param name="index">植物索引是1-8 动物索引是101-108</param>
    /// <param name="leftTime"></param>
    public bool CanGetLeftTime(int index)
    {
        return plantAndAnimalRemainTimeDic.ContainsKey(index);
    }
    /// <summary>
    /// 获取剩余时间
    /// </summary>
    /// <param name="index">植物索引是1-8 动物索引是101-108</param>
    public uint GetLeftTimeByIndex(int index)
    {
        if (plantAndAnimalRemainTimeDic.ContainsKey(index))
        {
            return plantAndAnimalRemainTimeDic[index];
        }
        else
        {
            Log.Error("没有找到索引 index " + index.ToString());
            return 0;
        }
    }
    #endregion
    #region 实体管理
    void AddHomeEntity(long uid, HomeEntityInfo state)
    {
        if (entityStateDic.ContainsKey(uid))
        {
            entityStateDic[uid] = state;
        }
        else
        {
            entityStateDic.Add(uid, state);
        }
    }
    void DeleteHomeEntity(long uid)
    {
        if (entityStateDic.ContainsKey(uid))
        {
            entityStateDic.Remove(uid);
            HomeScene.Instance.RemoveEntity(uid);
        }
    }
    HomeEntityInfo GetHomeEntityByIndex(EntityType type, int index, out long entityid)
    {

        foreach (var dic in entityStateDic)
        {
            HomeEntityInfo info = dic.Value;
            if (info.type == type && info.index == index)
            {
                entityid = dic.Key;
                return info;
            }
        }
        entityid = 0;
        return null;
    }
    /// <summary>
    /// 设置家园实体状态通过实体id
    /// </summary>
    /// <param name="landIndex">实体id</param>
    /// <param name="state">状态</param>
    /// <param name="indexID">homeland.xlsx 模型配置表里对应的索引id</param>
    void SetHomeEntityState(long uid, int state, uint indexID)
    {
        if (entityStateDic.ContainsKey(uid))
        {
            HomeEntityInfo info = entityStateDic[uid];
            info.state = state;
            entityStateDic[uid] = info;
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es != null)
            {
                IEntity en = es.FindEntity(uid);
                if (en != null)
                {
                    HomeLandViewDatabase db = GameTableManager.Instance.GetTableItem<HomeLandViewDatabase>((uint)indexID, state);
                    if (db != null)
                    {
                        ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<ResourceDataBase>(db.dwModelID);
                        if (rdb != null)
                        {
                            ChangePart cp = new ChangePart();
                            cp.strPartName = "main";
                            cp.strResName = rdb.strPath;
                            en.SendMessage(EntityMessage.EntityCommand_ChangePart, cp);
                        }
                    }

                }
            }
        }
        else
        {
            Log.Error("not cotain uid " + uid.ToString());
        }
    }
    #endregion

    /// <summary>
    /// 种子幼崽 矿场 许愿树 倒计时
    /// </summary>
    /// <param name="uTimerID"></param>
    public void OnTimer(uint uTimerID) 
    {
        //农场 、牧场
        //foreach (KeyValuePair<uint, uint> kvp in seedIndexDic)
        //{
        //    if (plantAndAnimalRemainTimeDic.ContainsKey((int)kvp.Key))
        //    {
        //        if (plantAndAnimalRemainTimeDic[(int)kvp.Key]>0)
        //        {
        //            plantAndAnimalRemainTimeDic[(int)kvp.Key] -= 1;
        //        }
        //    }
        //}

        Dictionary<uint, uint>.Enumerator iter = seedIndexDic.GetEnumerator();
        while(iter.MoveNext())
        {
            int key = (int)iter.Current.Key;
            if (plantAndAnimalRemainTimeDic.ContainsKey(key))
            {
                if (plantAndAnimalRemainTimeDic[key] > 0)
                {
                    plantAndAnimalRemainTimeDic[key] -= 1;
                }
            }
        }

        //矿场
        OnMineProcess();

        //许愿树
        OnTreeProcess();
    }

}

