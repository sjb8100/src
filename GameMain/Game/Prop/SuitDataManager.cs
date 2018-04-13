
//*************************************************************************
//	创建日期:	2017/2/20 星期一 17:05:43
//	文件名称:	SuitDataManager
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using Engine;
using Client;
using GameCmd;
using Engine.Utility;
using UnityEngine;
public struct ClientSuitData
{//baseid
    public uint suitBaseID;
    //剩余时间
    public uint leftTime;
    //时装状态
    public SuitState suitState;
}
public enum SuitState
{
    None = 0,
    Show,//展示
    Equip,//装备
    HasBuy,//拥有
    NoEffect,//失效
    NoBuy,//未拥有
    Active,//激活
}
class SuitDataManager : BaseModuleData, IManager
{
    SuitDataBase m_curSuitDataBase = null;
    //当前选中的时装
    public SuitDataBase CurSuitDataBase
    {
        get
        {
            return m_curSuitDataBase;
        }
        set
        {
            m_curSuitDataBase = value;
        }
    }

    GameCmd.EquipSuitType m_curSuitType = EquipSuitType.Clothes_Type;
    public GameCmd.EquipSuitType CurSuitType
    {
        set
        {
            m_curSuitType = value;
        }
        get
        {
            return m_curSuitType;
        }
    }

    //用以记录被替换之前的武器id
    public uint CurWeaponSuitID
    {
        set;
        get;
    }
    //已经装备的时装id列表
    List<uint> hasEquipSuitList = new List<uint>();
    /// <summary>
    /// 保存所有时装数据 key是时装baseid
    /// </summary>

    Dictionary<uint, Dictionary<uint, ClientSuitData>> m_dicSuitData = new Dictionary<uint, Dictionary<uint, ClientSuitData>>();

    #region IManager
    public void Initialize()
    {
        EventEngine.Instance().AddEventListener((int)GameEventID.PLAYER_LOGIN_SUCCESS, OnSuitEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.SKILLSYSTEM_CHANGEMODEL, OnSuitEvent);
        EventEngine.Instance().AddEventListener((int)GameEventID.LOGINSUCESSANDRECEIVEALLINFO, OnSuitEvent);
    }

    public void Reset(bool depthClearData = false)
    {

    }

    public void Process(float deltaTime)
    {

    }
    public void ClearData()
    {

    }
    #endregion
    void OnSuitEvent(int nEventID, object param)
    {
        if (nEventID == (int)GameEventID.PLAYER_LOGIN_SUCCESS)
        {

            //             stOpSuitPropertyUserCmd_C cmd = new stOpSuitPropertyUserCmd_C();
            //             cmd.type = SuitOPType.SuitOPType_Login;
            //             NetService.Instance.Send(cmd);
        }
        else if (nEventID == (int)GameEventID.LOGINSUCESSANDRECEIVEALLINFO)
        {
            InitShowDic();
            stOpSuitPropertyUserCmd_C cmd = new stOpSuitPropertyUserCmd_C();
            cmd.type = SuitOPType.SuitOPType_Login;
            NetService.Instance.Send(cmd);
        }
        else if (nEventID == (int)GameEventID.SKILLSYSTEM_CHANGEMODEL)
        {
            stSkillChangeModel st = (stSkillChangeModel)param;
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                return;
            }
            IPlayer player = es.FindEntity<IPlayer>(st.userID);
            if (player == null)
            {
                return;
            }
            uint suitID = 0;
            if (!IsPlayerEquipCloth(player, out suitID))
            {
                ChangeEquip ce = new ChangeEquip();
                ce.nSuitID = (int)suitID;
                ce.pos = Client.EquipPos.EquipPos_Body;
                ce.nStateType = player.GetProp((int)PlayerProp.SkillStatus);
                if (this != null)
                {//EntityCommand_PlayAni
                    PlayAni anim_param = new PlayAni();
                    anim_param.strAcionName = EntityAction.Stand;
                    anim_param.fSpeed = 1;
                    anim_param.nStartFrame = 0;

                    player.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
                    player.SendMessage(EntityMessage.EntityCommand_ChangeEquip, ce);
                }

            }
        }
    }
    List<SuitDataBase> GetAllSuitByJob(int job)
    {
        List<SuitDataBase> allData = GameTableManager.Instance.GetTableList<SuitDataBase>();
        if (allData != null)
        {
            return allData.FindAll((x) => { return (x.job == job || x.job == 0) && x.suitLv == 1 && x.typeMask != 0; });
        }
        return null;
    }
    void InitShowDic()
    {
        int job = MainPlayerHelper.GetMainPlayerJob();
        List<SuitDataBase> allList = GetAllSuitByJob(job);
        List<SuitDataBase> advList = allList.FindAll((x) => { return x.typeMask == 2; });
        List<ClientSuitData> showList = new List<ClientSuitData>();
        Dictionary<uint, ClientSuitData> showDic = new Dictionary<uint, ClientSuitData>();
        uint userID = MainPlayerHelper.GetPlayerID();
        for (int i = 0; i < advList.Count; i++)
        {
            uint baseID = advList[i].base_id;
            if (!m_dicSuitData.ContainsKey(userID))
            {
                ClientSuitData cd = new ClientSuitData();
                cd.suitState = SuitState.Show;
                cd.leftTime = 0;
                cd.suitBaseID = advList[i].base_id;

                AddSuitData(userID, cd.suitBaseID, cd);
            }
            else
            {
                Dictionary<uint, ClientSuitData> dic = m_dicSuitData[userID];
                if (!dic.ContainsKey(baseID))
                {
                    ClientSuitData cd = new ClientSuitData();
                    cd.suitState = SuitState.Show;
                    cd.leftTime = 0;
                    cd.suitBaseID = advList[i].base_id;

                    AddSuitData(userID, cd.suitBaseID, cd);
                }
            }

        }

    }
    public List<ClientSuitData> GetSortListData()
    {
        int job = MainPlayerHelper.GetMainPlayerJob();
        List<SuitDataBase> allList = GetAllSuitByJob(job);

        List<ClientSuitData> showList = new List<ClientSuitData>();

        List<ClientSuitData> noEffectList = new List<ClientSuitData>();
        List<ClientSuitData> effectList = new List<ClientSuitData>();
        List<ClientSuitData> equipList = new List<ClientSuitData>();
        if (MainPlayerHelper.GetMainPlayer() == null)
        {

            return null;
        }

        Dictionary<uint, ClientSuitData> myDic = m_dicSuitData[MainPlayerHelper.GetPlayerID()];
        var iter = myDic.GetEnumerator();
        while (iter.MoveNext())
        {
            uint baseID = iter.Current.Key;
            ClientSuitData data = iter.Current.Value;
            if (data.suitState == SuitState.NoEffect)
            {
                if (!noEffectList.Contains(data))
                {
                    noEffectList.Add(data);
                }
            }
            else if (data.suitState == SuitState.HasBuy)
            {
                if (!effectList.Contains(data))
                {
                    effectList.Add(data);
                }
            }
            else if (data.suitState == SuitState.Equip)
            {
                if (!equipList.Contains(data))
                {
                    equipList.Add(data);
                }
            }
            else if (data.suitState == SuitState.Show || data.suitState == SuitState.Active)
            {
                if (!showList.Contains(data))
                {
                    showList.Add(data);
                }
            }
        }
        List<ClientSuitData> allClientList = new List<ClientSuitData>();
        allClientList.AddRange(showList);
        allClientList.AddRange(equipList);
        allClientList.AddRange(effectList);
        allClientList.AddRange(noEffectList);
        List<ClientSuitData> noHaveList = new List<ClientSuitData>();
        for (int i = 0; i < allList.Count; i++)
        {
            SuitDataBase db = allList[i];
            bool bContain = false;
            for (int j = 0; j < allClientList.Count; j++)
            {
                ClientSuitData sd = allClientList[j];
                if (sd.suitBaseID == db.base_id)
                {
                    bContain = true;
                }
            }
            if (!bContain)
            {
                ClientSuitData cd = new ClientSuitData();
                cd.suitBaseID = db.base_id;
                cd.leftTime = db.time;
                cd.suitState = SuitState.NoBuy;
                if (!noHaveList.Contains(cd))
                {
                    noHaveList.Add(cd);
                }
            }
        }
        allClientList.AddRange(noHaveList);
        List<ClientSuitData> sortList = new List<ClientSuitData>();
        for (int i = 0; i < allClientList.Count; i++)
        {
            ClientSuitData cd = allClientList[i];
            SuitDataBase db = GameTableManager.Instance.GetTableItem<SuitDataBase>(cd.suitBaseID, 1);
            if (db != null)
            {
                if (db.type == (uint)m_curSuitType)
                {
                    sortList.Add(cd);

                }

            }
        }
        return sortList;
    }
    public void SetSuitDataOnChangePos()
    {
        List<ClientSuitData> list = GetSortListData();
        if (list != null)
        {
            if (list.Count > 0)
            {
                ClientSuitData cd = list[0];
                SuitDataBase db = GameTableManager.Instance.GetTableItem<SuitDataBase>(cd.suitBaseID, 1);
                if (db != null)
                {
                    CurSuitDataBase = db;
                }
            }
        }
    }

    /// <summary>
    /// 判断当前时装是否是永久
    /// </summary>
    /// <param name="baseID">时装的baseid</param>
    /// <returns></returns>
    public bool IsSuitForever(uint baseID)
    {

        uint userID = MainPlayerHelper.GetPlayerID();
        if (m_dicSuitData.ContainsKey(userID))
        {
            Dictionary<uint, ClientSuitData> dic = m_dicSuitData[userID];
            if (dic.ContainsKey(baseID))
            {
                ClientSuitData sd = dic[baseID];
                if (sd.suitState == SuitState.HasBuy && sd.leftTime == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public ClientSuitData GetSuitData(uint baseID)
    {

        uint userID = MainPlayerHelper.GetPlayerID();
        if (m_dicSuitData.ContainsKey(userID))
        {
            Dictionary<uint, ClientSuitData> dic = m_dicSuitData[userID];
            if (dic.ContainsKey(baseID))
            {
                ClientSuitData sd = dic[baseID];
                return sd;
            }
        }
        return new ClientSuitData();
    }
    public SuitState GetSuitState(uint baseID)
    {

        uint userID = MainPlayerHelper.GetPlayerID();
        if (m_dicSuitData.ContainsKey(userID))
        {
            Dictionary<uint, ClientSuitData> dic = m_dicSuitData[userID];
            if (dic.ContainsKey(baseID))
            {
                ClientSuitData sd = dic[baseID];
                return sd.suitState;
            }
            else
            {
                return SuitState.NoBuy;
            }
        }
        else
        {
            return SuitState.NoBuy;
        }
    }
    public void AddSuitData(uint userID, uint baseID, ClientSuitData data)
    {
        if (data.suitBaseID == 5001)
        {
            Log.LogGroup("ZDY", "suitstate is " + data.suitState);
        }
        if (m_dicSuitData.ContainsKey(userID))
        {
            Dictionary<uint, ClientSuitData> dic = m_dicSuitData[userID];
            if (dic.ContainsKey(baseID))
            {
                dic[baseID] = data;
            }
            else
            {
                dic.Add(baseID, data);
            }
        }
        else
        {
            Dictionary<uint, ClientSuitData> dic = new Dictionary<uint, ClientSuitData>();
            dic.Add(baseID, data);
            m_dicSuitData.Add(userID, dic);
        }

    }
    public string GetLeftTimeStringMin(int nTimeSce)
    {
        if (nTimeSce == 0)
        {
            return CommonData.GetLocalString("永久");
        }
        int num4 = ((int)nTimeSce) / 86400;
        int num5 = (((int)nTimeSce) - (num4 * 86400)) / 3600;
        string str = string.Empty;
        if (num4 > 0)
        {
            str = str + string.Format("{0}", num4.ToString() + CommonData.GetLocalString("天"));
        }
        if (num5 > 0)
        {
            str = str + string.Format("{0}", num5.ToString() + CommonData.GetLocalString("时"));
        }
        if (num5 < 1)
        {
            int num6 = ((((int)nTimeSce) - (num4 * 86400)) - (num5 * 3600)) / 60;
            if (num6 > 0)
            {
                str = str + string.Format("{0}", num6.ToString() + CommonData.GetLocalString("分"));
            }
        }

        if (!string.IsNullOrEmpty(str))
        {
            return str;
        }
        if (nTimeSce > 0)
        {
            return CommonData.GetLocalString("1分钟内");
        }

        return CommonData.GetLocalString("已过期");
    }
    void SendChangeBody(int suitType, uint baseID, IEntity entity)
    {
        if (entity != null)
        {

            if (suitType == (int)GameCmd.EquipSuitType.Magic_Pet_Type)
            {

            }
            else
            {
                //Engine.Utility.Log.Error("111 >>>entity Id = " + entity.GetID() + "   " + entity.GetObjName());
                ChangeEquip ce = new ChangeEquip();
                ce.nSuitID = (int)baseID;
                ce.pos = GetPosBySuitType((uint)suitType);
                ce.evt = CreateRenderObjCallBackEvent;
                ce.nStateType = (int)DataManager.Manager<LearnSkillDataManager>().CurState;
                entity.SendMessage(EntityMessage.EntityCommand_ChangeEquip, ce);


            }

        }
    }
    // 通知主角的时装变化，主要用于UI上模型的变化
    public void SendChangeRenderObj(uint suitID, int suitType, uint entityID)
    {
        if (!ClientGlobal.Instance().IsMainPlayer(entityID))
        {
            return;
        }

        Client.stRefreshRenderObj obj = new stRefreshRenderObj();
        obj.suitID = suitID;
        obj.userID = entityID;
        obj.suitType = suitType;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CHANGERENDEROBJ, obj);

    }
    public Client.EquipPos GetPosBySuitType(uint type)
    {
        /*
          0.初始外观 1.服装 2.奇兵 3.背饰 4.脸饰 100.萌宠
          */
        Client.EquipPos pos = Client.EquipPos.EquipPos_Null;
        if (type == 1)
        {
            pos = Client.EquipPos.EquipPos_Body;
        }
        else if (type == 2)
        {
            pos = Client.EquipPos.EquipPos_Weapon;
        }
        else if (type == 3)
        {
            pos = Client.EquipPos.EquipPos_Back;
        }
        else if (type == 4)
        {
            pos = Client.EquipPos.EquipPos_Face;
        }
        else if (type == 0)
        {
            pos = Client.EquipPos.EquipPos_Null;
        }
        return pos;
    }
    #region msg
    public void OnReceiveAllSuitData(stSendAllSuitPropertyUserCmd_S msg)
    {
        IPlayer player = MainPlayerHelper.GetMainPlayer();
        if (player == null)
        {
            Log.Error("收到所有时装数据 但是还没有主角创建");
            return;
        }
        uint userID = player.GetID();
        List<stSuitData> suitList = msg.data;
        string suitName = "";
        for (int i = 0; i < suitList.Count; i++)
        {
            stSuitData sd = suitList[i];
            ClientSuitData csd = new ClientSuitData();
            csd.suitBaseID = sd.base_id;
            csd.leftTime = sd.time;
            Client.GameEventID eventtype = Client.GameEventID.Unknow;
            if (sd.status == SuitStatus.SuitStatus_Common)
            {
                csd.suitState = SuitState.HasBuy;
                AddSuitData(userID, sd.base_id, csd);
                //直接装备
                if (msg.type == SuitOPType.SuitOPType_Buy || msg.type == SuitOPType.SuitOPType_Renew)
                {
                    stOpSuitPropertyUserCmd_C cmd = new stOpSuitPropertyUserCmd_C();
                    cmd.type = SuitOPType.SuitOPType_Equip;
                    //固定3当装备
                    cmd.suit_id = (sd.base_id << 16) + 3;
                    NetService.Instance.Send(cmd);
                }


            }
            else if (sd.status == SuitStatus.SuitStatus_Equip)
            {
                csd.suitState = SuitState.Equip;
                AddSuitData(userID, sd.base_id, csd);

            }
            else if (sd.status == SuitStatus.SuitStatus_Overdue)
            {
                csd.suitState = SuitState.NoEffect;
                AddSuitData(userID, sd.base_id, csd);
            }
            else if (sd.status == SuitStatus.SuitStatus_Acti)
            {
                csd.suitState = SuitState.Active;
                AddSuitData(userID, sd.base_id, csd);
            }
            else if (sd.status == SuitStatus.SuitStatus_UNActi)
            {
                csd.suitState = SuitState.Show;
                AddSuitData(userID, sd.base_id, csd);
            }
            table.SuitDataBase db = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(sd.base_id, 1);
            if (db != null)
            {
                suitName = db.name;
            }

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTFASHIONDATA, sd.base_id);
            //SendChangeBody((int)sd.type, sd.base_id, player);

        }

        ShowTips(msg.type, suitName);
    }
    void ShowTips(SuitOPType type, string suitName)
    {
        LocalTextType key = LocalTextType.LocalText_None;
        if (type == SuitOPType.SuitOPType_Equip)
        {
            key = LocalTextType.Local_TXT_Wear;
        }
        else if (type == SuitOPType.SuitOPType_Renew)
        {
            key = LocalTextType.Local_TXT_Renew;
        }
        else if (type == SuitOPType.SuitOPType_Unequip)
        {
            key = LocalTextType.Local_TXT_Unload;
        }
        else if (type == SuitOPType.SuitOPType_Buy)
        {
            key = LocalTextType.Local_TXT_Buy;
        }
        if (key != LocalTextType.LocalText_None)
        {
            string op = DataManager.Manager<TextManager>().GetLocalText(key);
            TipsManager.Instance.ShowTips(op + suitName);
        }
    }
    bool IsRefreshFashionData(uint baseID)
    {
        return false;
    }
    public void OnSendNineSuitMsg(stSendSuitToNinePropertyUserCmd_S cmd)
    {
        uint userID = cmd.userid;
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            if (cmd.type == EquipSuitType.Magic_Pet_Type)
            {
                ChangePetBody(cmd.suitid, cmd.petid);
                SendChangeRenderObj(cmd.suitid, (int)cmd.type, userID);
            }
            else
            {
                IPlayer player = es.FindEntity<IPlayer>(userID);
                if (player == null)
                {
                    return;
                }

                //if(ClientGlobal.Instance().IsMainPlayer(player))
                //{
                //    return;
                //}

                if (player != null)
                {
                    SendChangeBody((int)cmd.type, cmd.suitid, player);
                    SendChangeRenderObj(cmd.suitid, (int)cmd.type, userID);
                }
            }

        }
    }

    public void OnMiningTreasureMapToChangeWeapon(IEntity en)
    {

        enumProfession job = (enumProfession)en.GetProp((int)PlayerProp.Job);
        uint suitid = 0;
        switch (job)
        {
            //战士
            case enumProfession.Profession_Soldier:
                suitid = 2511;
                break;
            //幻师
            case enumProfession.Profession_Spy:
                int statues = en.GetProp((int)PlayerProp.SkillStatus);
                //魔医形态用战士的时装id 
                if (statues == 1)
                {
                    suitid = 2512;
                }
                else
                {
                    suitid = 2511;
                }
                break;
            //法师
            case enumProfession.Profession_Freeman:
                suitid = 2513;
                break;
            //暗巫
            case enumProfession.Profession_Doctor:
                suitid = 2514;
                break;
        }
        IPlayer player = en as IPlayer;
        if (player == null)
        {
            return;
        }
        List<GameCmd.SuitData> suitList = null;
        player.GetSuit(out suitList);
        if (suitList != null)
        {
            for (int i = 0; i < suitList.Count; i++)
            {
                SuitData data = suitList[i];
                if (data.suit_type == EquipSuitType.Qibing_Type)
                {
                    if (data.baseid != 0)
                    {
                        CurWeaponSuitID = data.baseid;

                    }
                }
            }
        }
        SendChangeBody(2, suitid, player);
        SendChangeRenderObj(suitid, 2, player.GetID());

    }

    void ChangePetBody(uint suitID, uint petID)
    {
        PetDataManager pm = DataManager.Manager<PetDataManager>();
        if (pm != null)
        {
            //if (pm.CurFightingPet != 0)
            {
                IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
                if (es != null)
                {
                    INPC npc = es.FindNPC(petID);
                    if (npc != null)
                    {
                        if (suitID != 0)
                        {
                            SuitDataBase sdb = GameTableManager.Instance.GetTableItem<SuitDataBase>(suitID, 1);
                            if (sdb != null)
                            {
                                Client.ChangeBody cb = new Client.ChangeBody();
                                cb.resId = (int)sdb.resid;
                                cb.modleScale = sdb.sceneModleScale;
                                npc.SendMessage(EntityMessage.EntityCommand_Change, cb);
                            }
                        }
                        else
                        {
                            npc.SendMessage(EntityMessage.EntityCommand_Restore);
                        }
                    }
                }
            }
        }
    }
    #endregion
    public bool IsPlayerEquipCloth(IPlayer player, out uint suitID)
    {
        suitID = 0;
        if (player == null)
        {
            return false;
        }
        List<GameCmd.SuitData> suitList = null;
        player.GetSuit(out suitList);
        bool bEquip = false;
        if (suitList != null)
        {
            for (int i = 0; i < suitList.Count; i++)
            {
                SuitData data = suitList[i];
                if (data.suit_type == EquipSuitType.Clothes_Type)
                {
                    suitID = data.baseid;
                    if (data.baseid != 0)
                    {
                        SuitDataBase sdb = GameTableManager.Instance.GetTableItem<SuitDataBase>(data.baseid);
                        if (sdb != null)
                        {
                            if (sdb.typeMask == 1)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public void RebackWeaponSuit()
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            IEntity en = es.FindEntity(MainPlayerHelper.GetPlayerUID());
            if (en == null)
            {
                return;
            }
            IPlayer player = en as IPlayer;
            if (player == null)
            {
                return;
            }
            //if (CurWeaponSuitID != 0)
            {
                SendChangeBody(2, CurWeaponSuitID, player);
                SendChangeRenderObj(CurWeaponSuitID, 2, player.GetID());
            }
        }
    }

    public void RebackWeaponSuit(uint uid)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        IPlayer player = es.FindPlayer(uid);
        if (player == null)
        {
            return;
        }

        uint suitId;
        if (m_playerSuitDic.TryGetValue(player.GetID(), out suitId))
        {
            SendChangeBody(2, suitId, player);
            SendChangeRenderObj(suitId, 2, player.GetID());
        }
    }

    public void RebackWeaponSuitAndCleanData(IPlayer player)
    {
        if (player == null)
        {
            return;
        }

        uint suitId;
        if (m_playerSuitDic.TryGetValue(player.GetID(), out suitId))
        {
            SendChangeBody(2, suitId, player);
            SendChangeRenderObj(suitId, 2, player.GetID());
            m_playerSuitDic.Remove(player.GetID());
            m_playerFishingDic.Remove(player.GetID());
        }
    }

    public void RebackWeaponSuitAndCleanData(uint uid)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es == null)
        {
            return;
        }

        IPlayer player = es.FindPlayer(uid);
        if (player == null)
        {
            return;
        }

        uint suitId;
        if (m_playerSuitDic.TryGetValue(player.GetID(), out suitId))
        {
            SendChangeBody(2, suitId, player);
            SendChangeRenderObj(suitId, 2, player.GetID());
            m_playerSuitDic.Remove(player.GetID());
            m_playerFishingDic.Remove(player.GetID());
        }
    }

    /// <summary>
    /// 钓鱼，使用鱼竿
    /// </summary>
    public void OnFishingRodSuit(IEntity en)
    {
        enumProfession job = (enumProfession)en.GetProp((int)PlayerProp.Job);
        uint suitid = 0;
        switch (job)
        {
            //战士
            case enumProfession.Profession_Soldier:
                suitid = 2521;
                break;
            //幻师
            case enumProfession.Profession_Spy:
                int statues = en.GetProp((int)PlayerProp.SkillStatus);
                //魔医形态用战士的时装id 
                if (statues == 1)
                {
                    suitid = 2522;
                }
                else
                {
                    suitid = 2522;
                }
                break;
            //法师
            case enumProfession.Profession_Freeman:
                suitid = 2523;
                break;
            //暗巫
            case enumProfession.Profession_Doctor:
                suitid = 2524;
                break;
        }
        IPlayer player = en as IPlayer;
        if (player == null)
        {
            return;
        }

        //需要保存的suitId
        uint saveSuitId = 0;

        List<GameCmd.SuitData> suitList = null;
        player.GetSuit(out suitList);
        if (suitList != null)
        {
            for (int i = 0; i < suitList.Count; i++)
            {
                SuitData data = suitList[i];
                if (data.suit_type == EquipSuitType.Qibing_Type)
                {
                    //if (data.baseid != 0)
                    {
                        //CurWeaponSuitID = data.baseid;
                        saveSuitId = data.baseid;
                    }
                }
            }
        }

        if (m_playerSuitDic.ContainsKey(player.GetID()))
        {
            m_playerSuitDic.Remove(player.GetID());
        }
        m_playerSuitDic.Add(player.GetID(), saveSuitId);

        //鱼竿数据
        if (m_playerFishingDic.ContainsKey(player.GetID()))
        {
            m_playerFishingDic.Remove(player.GetID());
        }
        m_fishingPlayerId = player.GetID();

        SendChangeBody(2, suitid, player);
        SendChangeRenderObj(suitid, 2, player.GetID());
    }

    /// <summary>
    /// 存钓鱼鱼竿装备ID  key : 玩家uid  value: 鱼竿装备ID
    /// </summary>
    Dictionary<uint, uint> m_playerSuitDic = new Dictionary<uint, uint>();
    public Dictionary<uint, uint> PlayerSuitDic
    {
        get
        {
            return m_playerSuitDic;
        }
    }


    uint m_fishingPlayerId = 0;

    /// <summary>
    /// 存放鱼竿 key : 玩家uid  value: 鱼竿渲染对象
    /// </summary>
    Dictionary<uint, IRenderObj> m_playerFishingDic = new Dictionary<uint, IRenderObj>();

    public Dictionary<uint, IRenderObj> PlayerFishingDic
    {
        get
        {
            return m_playerFishingDic;
        }
    }

    public void CleanFisingData() 
    {
        m_playerSuitDic.Clear();
        m_playerFishingDic.Clear();
    }

    void CreateRenderObjCallBackEvent(IRenderObj obj, object param)
    {
      //  Engine.Utility.Log.Error("222>>>objID = " + obj.GetID() + "  " + obj.GetName());

        IRenderObj yuganObj = param as IRenderObj;
        if (yuganObj == null)
        {
         //   Engine.Utility.Log.Error("--->>> param 为 null !!!");
            return;
        }

        if (false == m_playerFishingDic.ContainsKey(m_fishingPlayerId))
        {
            m_playerFishingDic.Add(m_fishingPlayerId, yuganObj);
        }
        
       // Engine.Utility.Log.Error("--->>> 成功更换武器：" + yuganObj.GetName());
    }

    /// <summary>
    /// 现在是否是钓鱼的装备
    /// </summary>
    /// <param name="uid"></param>
    public bool IsFishingSuit(uint uid)
    {
        return m_playerSuitDic.ContainsKey(uid);
    }

    
}
