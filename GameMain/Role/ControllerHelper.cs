using System;
using System.Collections.Generic;
using Client;


public class UseItemFinalCD
{
    public uint baseid;
    public float selfcd;
    public float startTime;
    public uint groupid;
    public float groupcd;
}

public class ControllerHelper : IControllerHelper
{
    List<UseItemFinalCD> m_lstUseItemFinalCD = new List<UseItemFinalCD>();
    Dictionary<AutoRecoverGrid.MedicalType, UseMedicalInfo> m_dicMedical = new Dictionary<AutoRecoverGrid.MedicalType, UseMedicalInfo>();

    Dictionary<uint, uint> m_dicHpMedicineItem = new Dictionary<uint, uint>();
    List<uint> m_lstHpKey = new List<uint>(10);
    Dictionary<uint, uint> m_dicMpMedicineItem = new Dictionary<uint, uint>();
    List<uint> m_lstMpKey = new List<uint>(10);
    float m_nhpsendTime = 0;
    float m_nmpsendTime = 0;
    class UseMedicalInfo
    {
        public AutoRecoverGrid.MedicalType Type;
        public float triggerValue;
        public uint itemid;
        public bool select;

    }

    public ControllerHelper()
    {
        InitMedicine();
        InitSetting();
    }

    public bool HasEnoughItem(uint itemBaseID, int needCount)
    {
        int count = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemBaseID);
        if (count < needCount)
        {
            return false;
        }
        return true;

    }

    void InitMedicine()
    {
        List<table.ItemDataBase> lstdata = GameTableManager.Instance.GetTableList<table.ItemDataBase>();

        table.ItemDataBase itemdata = null;
        for (int i = 0, imax = lstdata.Count; i < imax; i++)
        {
            itemdata = lstdata[i];
            if (itemdata.baseType != 2)
            {
                continue;
            }

            if (itemdata.subType == (int)ItemDefine.ItemConsumerSubType.Hp)
            {
                if (!m_dicHpMedicineItem.ContainsKey(itemdata.useLevel))
                {
                    m_dicHpMedicineItem.Add(itemdata.useLevel, itemdata.itemID);
                    m_lstHpKey.Add(itemdata.useLevel);
                }
            }
            else if (itemdata.subType == (int)ItemDefine.ItemConsumerSubType.Mp)
            {
                if (!m_dicMpMedicineItem.ContainsKey(itemdata.useLevel))
                {
                    m_dicMpMedicineItem.Add(itemdata.useLevel, itemdata.itemID);
                    m_lstMpKey.Add(itemdata.useLevel);
                }
            }
        }
    }

    public void InitSetting()
    {
        Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
        if (option == null) return;


        for (int i = (int)AutoRecoverGrid.MedicalType.Hp; i < (int)AutoRecoverGrid.MedicalType.Max; ++i)
        {
            AutoRecoverGrid.MedicalType mtype = (AutoRecoverGrid.MedicalType)i;

            bool select = option.GetInt("MedicalSetting", mtype.ToString(), 1) == 1;

            float value = option.GetInt("MedicalSetting", mtype.ToString() + "value", 50) * 0.01f;

            uint itemid = (uint)option.GetInt("MedicalSetting", mtype.ToString() + "itemid", 0);

            if (!m_dicMedical.ContainsKey(mtype))
            {
                m_dicMedical.Add(mtype, new UseMedicalInfo());
            }

            m_dicMedical[mtype].select = select;
            m_dicMedical[mtype].Type = mtype;
            m_dicMedical[mtype].triggerValue = value;
            m_dicMedical[mtype].itemid = itemid;
        }

    }

    /// <summary>
    /// //类型 0-物品本身cd  1-组cd
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseid"></param>
    /// <param name="cd">sec</param>
    public void SetMedicineCDInfo(uint baseid, List<GameCmd.CDInfo> cdinfos)
    {
        UseItemFinalCD cddata = new UseItemFinalCD();
        cddata.baseid = baseid;
        for (int i = 0; i < cdinfos.Count; i++)
        {
            if (cdinfos[i].type == 0)
            {
                cddata.selfcd = cdinfos[i].cd;
            }
            else if (cdinfos[i].type == 1)
            {
                cddata.groupcd = cdinfos[i].cd;

                table.ItemDataBase itemdata = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(baseid);
                if (itemdata != null)
                {
                    table.EffectFuncDataBase effectdata = GameTableManager.Instance.GetTableItem<table.EffectFuncDataBase>(itemdata.func_id);
                    if (effectdata != null)
                    {
                        cddata.groupid = effectdata.group_id;
                    }
                }
            }
        }
        cddata.startTime = UnityEngine.Time.realtimeSinceStartup;
        m_lstUseItemFinalCD.Add(cddata);
        if (UnityEngine.Application.isEditor)
        {
            //string msg = string.Format("使用物品返回{0} selfcd{1} groupcd{2}  startTime{3}", baseid, cddata.selfcd, cddata.groupcd, cddata.startTime);
            //UnityEngine.Debug.Log(msg);
        }
        //Engine.Utility.Log.LogGroup("ZCX", "使用物品返回{0} cd1{1} cd2{2}", baseid,cddata.selfcd,cddata.groupcd);
    }

    public float GetItemCDByThisId(uint qwThisId)
    {
        BaseItem baseitem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(qwThisId);
        if (baseitem != null)
        {
            for (int i = 0; i < m_lstUseItemFinalCD.Count; i++)
            {
                if (m_lstUseItemFinalCD[i].baseid == baseitem.BaseData.itemID)
                {
                    if (m_lstUseItemFinalCD[i].selfcd > m_lstUseItemFinalCD[i].groupcd)
                    {
                        return m_lstUseItemFinalCD[i].selfcd - (UnityEngine.Time.realtimeSinceStartup - m_lstUseItemFinalCD[i].startTime);
                    }
                    else
                    {
                        return m_lstUseItemFinalCD[i].groupcd - (UnityEngine.Time.realtimeSinceStartup - m_lstUseItemFinalCD[i].startTime);
                    }
                }
            }
        }
        return 0f;
    }
    void CutDownCd()
    {
        List<UseItemFinalCD> toRemove = new List<UseItemFinalCD>();
        foreach (var item in m_lstUseItemFinalCD)
        {
            if (item.selfcd > 0 && UnityEngine.Time.realtimeSinceStartup - item.startTime >= item.selfcd)
            {
                item.selfcd = 0;
            }

            if (item.groupcd > 0 && UnityEngine.Time.realtimeSinceStartup - item.startTime >= item.groupcd)
            {
                item.groupcd = 0;
            }

            if (item.groupcd == 0 && item.selfcd == 0)
            {
                toRemove.Add(item);
            }
        }

        for (int i = 0; i < toRemove.Count; i++)
        {
            m_lstUseItemFinalCD.Remove(toRemove[i]);
            //             if (UnityEngine.Application.isEditor)
            //             {
            //                 string msg = string.Format("使用物品cd 时间到{0} startTime{1} now{2}", toRemove[i].baseid, toRemove[i].startTime, UnityEngine.Time.realtimeSinceStartup);
            //                 UnityEngine.Debug.Log(msg);
            //             }
            //Engine.Utility.Log.LogGroup("ZCX", );

        }
    }


    public void CheckUseMedicine()
    {
        Engine.Utility.Log.LogGroup("ZCX", "CheckUseMedicine");
        //UnityEngine.Profiler.BeginSample("CutDownCd");
        CutDownCd();
        //UnityEngine.Profiler.EndSample();

        UseMedicine();

    }
    /// <summary>
    /// 使用瞬药
    /// </summary>
    public void UseAtOnceMedicine()
    {
        bool canRunning = Engine.Utility.EventEngine.Instance().DispatchVote((int)GameVoteEventID.AUTORECOVER, null);
        if (!canRunning)
        {
            Engine.Utility.Log.LogGroup("ZCX", "canRunning false");
            return;
        }

        //是否可以使用瞬药
        bool canUseAtOnce = Engine.Utility.EventEngine.Instance().DispatchVote((int)GameVoteEventID.AUTOAtOnceRECOVER, null);
        if (!canUseAtOnce)
        {
            Engine.Utility.Log.LogGroup("ZCX", "canUseAtOnce false");
            return;
        }

        Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null || mainPlayer.IsDead())
        {
            Engine.Utility.Log.LogGroup("ZCX", "player dead");
            return;
        }
        Dictionary<AutoRecoverGrid.MedicalType, UseMedicalInfo>.Enumerator iter = m_dicMedical.GetEnumerator();
        while (iter.MoveNext())
        {
            UseMedicalInfo medical = iter.Current.Value;
            if (medical != null && medical.select)
            {
                AutoRecoverGrid.MedicalType mtype = medical.Type;
                float rate = 1f;
                if (AutoRecoverGrid.MedicalType.HpAtOnce == mtype)
                {//瞬药 单独处理
                    rate = mainPlayer.GetProp((int)CreatureProp.Hp) * 1f / mainPlayer.GetProp((int)CreatureProp.MaxHp);

                    if (rate < medical.triggerValue)
                    {
                        uint itemid = medical.itemid;
                        if (itemid != 0 && CanUse(itemid))
                        {
                            List<BaseItem> itemdataList = DataManager.Manager<ItemManager>().GetItemByBaseId(itemid);
                            if (itemdataList.Count > 0)
                            {
                                DataManager.Instance.Sender.UseItem(
                                   mainPlayer.GetID(),
                                   (uint)GameCmd.SceneEntryType.SceneEntry_Player,
                                   itemdataList[0].QWThisID, 1);
                            }
                        }
                    }
                }
            }
        }
    }
    private void UseMedicine()
    {
        bool canRunning = Engine.Utility.EventEngine.Instance().DispatchVote((int)GameVoteEventID.AUTORECOVER, null);
        if (!canRunning)
        {
            Engine.Utility.Log.LogGroup("ZCX", "canRunning false");
            return;
        }

        Client.IPlayer mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null || mainPlayer.IsDead())
        {
            Engine.Utility.Log.LogGroup("ZCX", "player dead");
            return;
        }
        Dictionary<AutoRecoverGrid.MedicalType, UseMedicalInfo>.Enumerator iter = m_dicMedical.GetEnumerator();
        while (iter.MoveNext())
        {
            UseMedicalInfo medical = iter.Current.Value;
            if (medical != null && medical.select)
            {
                AutoRecoverGrid.MedicalType mtype = medical.Type;
                float rate = 1f;
                if (AutoRecoverGrid.MedicalType.HpAtOnce == mtype)
                {//瞬药 单独处理
                    continue;
                }
                if (AutoRecoverGrid.MedicalType.Hp == mtype || (AutoRecoverGrid.MedicalType.HpAtOnce == mtype))
                {
                    rate = mainPlayer.GetProp((int)CreatureProp.Hp) * 1f / mainPlayer.GetProp((int)CreatureProp.MaxHp);
                }
                else if (AutoRecoverGrid.MedicalType.Mp == mtype)
                {
                    rate = mainPlayer.GetProp((int)CreatureProp.Mp) * 1f / mainPlayer.GetProp((int)CreatureProp.MaxMp);
                    // string log = string.Format("rate = {0} trigger = {1} mp = {2} max= {3}", rate, medical.triggerValue, mainPlayer.GetProp((int)CreatureProp.Mp), mainPlayer.GetProp((int)CreatureProp.MaxMp));
                    //UnityEngine.Debug.LogError(log);
                }
                else if (AutoRecoverGrid.MedicalType.PetHp == mtype)
                {
                    if (DataManager.Manager<PetDataManager>().CurFightingPet == 0)
                    {
                        continue;
                    }
                    Client.INPC pet = DataManager.Manager<PetDataManager>().GetNpcByPetID(DataManager.Manager<PetDataManager>().CurFightingPet);
                    if (pet != null)
                    {
                        int a = pet.GetProp((int)CreatureProp.Hp);
                        rate = pet.GetProp((int)CreatureProp.Hp) * 1f / pet.GetProp((int)CreatureProp.MaxHp);
                    }
                }

                if (rate < medical.triggerValue)
                {
                    uint itemid = medical.itemid;
                    if (AutoRecoverGrid.MedicalType.Hp == mtype)
                    {
                        if (UnityEngine.Time.realtimeSinceStartup - m_nhpsendTime < 5f)
                        {
                            continue;
                        }
                        itemid = GetUseItemId(mtype);

                    }
                    else if (AutoRecoverGrid.MedicalType.Mp == mtype)
                    {
                        if (UnityEngine.Time.realtimeSinceStartup - m_nmpsendTime < 5f)
                        {
                            continue;
                        }
                        itemid = GetUseItemId(mtype);
                    }
                    if (itemid != 0 && CanUse(itemid))
                    {
                        List<BaseItem> itemdataList = DataManager.Manager<ItemManager>().GetItemByBaseId(itemid);
                        if (itemdataList.Count > 0)
                        {
                            if (AutoRecoverGrid.MedicalType.PetHp == mtype)
                            {
                                uint petID = DataManager.Manager<PetDataManager>().GetNpcIDByPetID(DataManager.Manager<PetDataManager>().CurFightingPet);
                                DataManager.Instance.Sender.UseItem(
                                    petID,
                                    (uint)GameCmd.SceneEntryType.SceneEntry_NPC,
                                    itemdataList[0].QWThisID, 1);
                            }
                            else
                            {
                                DataManager.Instance.Sender.UseItem(
                                    mainPlayer.GetID(),
                                    (uint)GameCmd.SceneEntryType.SceneEntry_Player,
                                    itemdataList[0].QWThisID, 1);

                                if (AutoRecoverGrid.MedicalType.Hp == mtype)
                                {
                                    m_nhpsendTime = UnityEngine.Time.realtimeSinceStartup;
                                }
                                else if (AutoRecoverGrid.MedicalType.Mp == mtype)
                                {
                                    m_nmpsendTime = UnityEngine.Time.realtimeSinceStartup;
                                }
                            }

                            //                            if (UnityEngine.Application.isEditor)
                            //                           {
                            //                               UnityEngine.Debug.Log(string.Format("使用物品 {0}" , itemid));
                            //                          }
                        }
                    }
                }
            }
        }
    }

    uint GetUseItemId(AutoRecoverGrid.MedicalType type)
    {
        Dictionary<uint, uint> dicMedicineItem = null;
        List<uint> lstKeys = null;
        if (type == AutoRecoverGrid.MedicalType.Hp)
        {
            lstKeys = m_lstHpKey;
            dicMedicineItem = m_dicHpMedicineItem;
        }
        else if (type == AutoRecoverGrid.MedicalType.Mp)
        {
            lstKeys = m_lstMpKey;
            dicMedicineItem = m_dicMpMedicineItem;
        }


        if (dicMedicineItem != null && lstKeys != null)
        {
            int playerLevel = MainPlayerHelper.GetPlayerLevel();
            for (int i = lstKeys.Count - 1; i >= 0; i--)
            {
                if (playerLevel >= lstKeys[i])//可以使用
                {
                    if (dicMedicineItem.ContainsKey(lstKeys[i]))
                    {
                        uint id = dicMedicineItem[lstKeys[i]];
                        int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(id);
                        if (num > 0)
                        {
                            return id;
                        }
                    }
                }
            }
        }

        return 0;
    }
    bool CanUse(uint baseid)
    {
        //bool canUse = Engine.Utility.EventEngine.Instance().DispatchVote((int)GameVoteEventID.USE_MEDECAL_ITEM, baseid);

        //if (!canUse)
        //{
        //    return false;
        //}

        table.ItemDataBase itemdata = GameTableManager.Instance.GetTableItem<table.ItemDataBase>(baseid);
        if (itemdata != null)
        {
            if (itemdata.useLevel > MainPlayerHelper.GetPlayerLevel())
            {
                return false;
            }


            table.EffectFuncDataBase effectdata = GameTableManager.Instance.GetTableItem<table.EffectFuncDataBase>(itemdata.func_id);
            if (effectdata != null)
            {
                bool canuse = true;
                for (int i = 0; i < m_lstUseItemFinalCD.Count; i++)
                {
                    if (m_lstUseItemFinalCD[i].groupid == effectdata.group_id)
                    {
                        if (m_lstUseItemFinalCD[i].groupcd > 0)
                        {
                            canuse = false;
                            break;
                        }
                    }

                    if (m_lstUseItemFinalCD[i].baseid == baseid)
                    {
                        if (m_lstUseItemFinalCD[i].selfcd > 0)
                        {
                            canuse = false;
                            break;
                        }
                    }
                }
                return canuse;
            }
        }
        return false;
    }
    public bool IsCampFight()
    {
        if (DataManager.Manager<ComBatCopyDataManager>().GetCurCopyType() == CopyTypeTable.Camp)
        {
            return true;
        }
        return false;
    }
    public bool IsSameCamp(IEntity target)
    {//此接口未实现
        //同一个氏族 是同一个阵营 不同氏族通过氏族宣战关系来判断是敌对关系就能打 不是敌对关系就不能打
        return false;
    }
    //是不是敌对阵营 来判断能不能攻击
    public bool IsEnemyCamp(IEntity target)
    {
        if (DataManager.Manager<ComBatCopyDataManager>().GetCurCopyType() == CopyTypeTable.Camp)
        {
            //阵营判断
            GameCmd.eCamp mycamp = (GameCmd.eCamp)MainPlayerHelper.GetMainPlayer().GetProp((int)CreatureProp.Camp);
            GameCmd.eCamp targetcamp = (GameCmd.eCamp)target.GetProp((int)CreatureProp.Camp);

            if (mycamp == GameCmd.eCamp.CF_None || targetcamp == GameCmd.eCamp.CF_None)
            {
                Engine.Utility.Log.LogGroup("ZDY", "阵营判断 非敌对阵营");
                return false;
            }
            else if (targetcamp == GameCmd.eCamp.CF_Enemy)
            {
                Engine.Utility.Log.LogGroup("ZDY", "阵营判断 中立");
                return false;
            }
            else
            {
                return mycamp != targetcamp;
            }
        }
        else
        {
            //同一个氏族 是同一个阵营 不同氏族通过氏族宣战关系来判断是敌对关系就能打 不是敌对关系就不能打
            if (IsSameFamily(target))
            {
                return false;
            }
            else
            {
                bool isEnemy = DataManager.Manager<ClanManger>().IsRivalryRelationShip(target);
                Engine.Utility.Log.LogGroup("ZDY", "非敌对关系");
                return isEnemy;
            }
        }
        return false;
    }
    public bool IsSameFamily(IEntity target)
    {
        if (target is ICreature)
        {
            if (ClanManger.IsSameClan((ICreature)target, MainPlayerHelper.GetMainPlayer()))
            {
                return true;
            }
        }
        return false;
    }

    public uint GetMyPetNpcID()
    {
        PetDataManager petData = DataManager.Manager<PetDataManager>();
        if (petData != null)
        {
            return petData.GetNpcIDByPetID(petData.CurFightingPet);
        }
        return 0;
    }
    public bool NpcIsMyPet(IEntity npc)
    {
        PetDataManager petData = DataManager.Manager<PetDataManager>();
        if (petData != null)
        {
            return petData.NpcIsPet(npc.GetID());
        }
        return false;
    }
    public bool IsMyFrientd(IEntity target)
    {
        RelationManager relaManager = DataManager.Manager<RelationManager>();
        if (relaManager != null)
        {
            return relaManager.IsMyFriend(target.GetID());
        }
        return false;
    }
    public bool IsSameTeam(IEntity target)
    {
        TeamDataManager teamData = DataManager.Manager<TeamDataManager>();
        if (teamData != null)
        {
            //if (teamData.TeamState == TeamState.Alone)
            if (teamData.IsJoinTeam == false)
            {
                return false;
            }

            return teamData.IsMember(target.GetID());
        }
        return false;
    }
    public bool IsSameTeam(uint nteamid)
    {
        TeamDataManager teamData = DataManager.Manager<TeamDataManager>();
        if (teamData != null)
        {
            //if (teamData.TeamState == TeamState.Alone)
            if (teamData.IsJoinTeam == false)
            {
                return false;
            }

            return teamData.TeamId == nteamid;
        }
        return false;
    }

    public bool IsTeamerCanPick(uint nteamid)
    {
        TeamDataManager teamData = DataManager.Manager<TeamDataManager>();
        if (teamData != null)
        {
            //if (teamData.TeamState == TeamState.Alone)
            if (teamData.IsJoinTeam == false)
            {
                return false;
            }

            if (teamData.TeamItemMode == GameCmd.TeamItemMode.TeamItemMode_Leader)
            {
                if (teamData.TeamId == nteamid)
                {
                    return teamData.LeaderId == MainPlayerHelper.GetPlayerID();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }
        return false;
    }

    public bool TryUnRide(Action<object> callback, object param)
    {
        Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            return DataManager.Manager<RideManager>().TryUnRide(callback, param);
        }
        return false;
    }

    public bool TryRide(Action<object> callback, object param)
    {
        Client.IPlayer mainPlayer = ClientGlobal.Instance().MainPlayer;
        if (mainPlayer != null)
        {
            bool bRide = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommond_IsRide, null);
            if (!bRide)
            {

                return DataManager.Manager<RideManager>().TryUsingRide(callback, param);
            }
        }
        return false;
    }

    public ITipsManager GetTipsManager()
    {
        if (DataManager.Manager<UIPanelManager>() != null)
        {
            return TipsManager.Instance;
        }
        return null;
    }
    public uint GetCommonSkillID()
    {
        return DataManager.Manager<LearnSkillDataManager>().GetCommonSkillIDByJob();
    }


    public uint GetCopyLastKillWave()
    {
        return DataManager.Manager<ComBatCopyDataManager>().LaskSkillWave;
    }

    private List<INPC> m_npcList = new List<INPC>();
    /// <summary>
    /// 通过玩家id获取所属的npc
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public void GetOwnNpcByPlayerID(uint playerID, ref List<INPC> lstNPC)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        if (lstNPC == null)
        {
            return;
        }

        lstNPC.Clear();
        es.FindAllEntity<INPC>(ref m_npcList);
        for (int i = 0; i < m_npcList.Count; i++)
        {
            INPC npc = m_npcList[i];
            if (npc != null)
            {
                int masterID = npc.GetProp((int)NPCProp.Masterid);
                if (masterID == playerID)
                {
                    lstNPC.Add(npc);
                }
            }
        }
        m_npcList.Clear();
    }
    /// <summary>
    /// 通过玩家id获取所属宠物
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public INPC GetOwnPetByPlayerID(uint playerID)
    {
        List<INPC> npcList = new List<INPC>();
        GetOwnNpcByPlayerID(playerID, ref npcList);
        for (int i = 0; i < npcList.Count; i++)
        {
            INPC npc = npcList[i];
            if (npc.IsPet())
            {
                return npc;
            }
        }
        npcList.Clear();
        return null;
    }
    /// <summary>
    /// 通过玩家id获取所属召唤物
    /// </summary>
    /// <param name="playerID"></param>
    /// <returns></returns>
    public INPC GetOwnSummonByPlayerID(uint playerID)
    {
        List<INPC> npcList = new List<INPC>();
        GetOwnNpcByPlayerID(playerID, ref npcList);
        for (int i = 0; i < npcList.Count; i++)
        {
            INPC npc = npcList[i];
            if (npc.IsSummon())
            {
                return npc;
            }
        }
        npcList.Clear();
        return null;
    }

    public bool CanPutInKanpsack(uint itemBaseId, uint itemNum)
    {
        return DataManager.Manager<KnapsackManager>().CanPutInKanpsack(itemBaseId, itemNum);
    }

    public uint GetAttackPriority()
    {
        return (uint)DataManager.Manager<SettingManager>().AttackPriority;
    }

    /// <summary>
    /// 是否在跟服务器对时
    /// </summary>
    /// <returns></returns>
    public bool IsCheckingTime()
    {
        return NetService.Instance.BCheckingTime;
    }

    /// <summary>
    /// 获取玩家自己的clanId
    /// </summary>
    /// <returns></returns>
    public uint GetMainPlayerClanId() 
    {
        return DataManager.Manager<ClanManger>().ClanId;
    }

}
