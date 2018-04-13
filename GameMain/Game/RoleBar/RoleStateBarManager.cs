/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.RoleBar
 * 创建人：  wenjunhua.zqgame
 * 文件名：  RoleStateBarManager1
 * 版本号：  V1.0.0.0
 * 创建时间：7/26/2017 3:29:21 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;
using Engine.Utility;
public enum HeadStatusType
{
    None = 0,
    Hp,             //血量
    Name,           //姓名
    Clan,           //氏族
    Title,          //称号
    Collect,        //采集
    HeadMaskIcon,   //头顶图标标识
    TaskStatus,
    CampMask,
    BossSay,       //
    Max
}
partial class RoleStateBarManager : IManager, IGlobalEvent, ITimer
{
    #region IManager Method
    public void Initialize()
    {
        m_bClanNameEnable = ClientGlobal.Instance().gameOption.GetInt("ShowDetail", "ClanName", 1) == 1;
        m_bPlayerTitleEnable = ClientGlobal.Instance().gameOption.GetInt("ShowDetail", "PlayerTitle", 1) == 1;
        m_bPlayerNameEnable = ClientGlobal.Instance().gameOption.GetInt("ShowDetail", "PlayerName", 1) == 1;
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

    #region SettingProperty
    private static bool m_bClanNameEnable = false;
    //氏族名称是否可用
    public static bool ClanNameEnable
    {
        get
        {
            return m_bClanNameEnable;
        }
    }
    private static bool m_bPlayerTitleEnable = false;
    //角色称号是否可用
    public static bool PlayerTitleEnable
    {
        get
        {
            return m_bPlayerTitleEnable;
        }
    }
    private static bool m_bPlayerNameEnable = false;
    //角色名称是否可用
    public static bool PlayerNameEnable
    {
        get
        {
            return m_bPlayerNameEnable;
        }
    }

    static Dictionary<int, string> HpSpriteName_Dic = new Dictionary<int, string>();
    #endregion
    #region Op
    uint m_uTalkingBossID = 0;

    public void SetTalkingBossID(uint bossID)
    {
        IEntity en = RoleStateBarManager.GetEntityByUserID<INPC>(bossID);
        if (en != null)
        {
            m_uTalkingBossID = (uint)en.GetProp((int)EntityProp.BaseID);
        }
        else
        {
            m_uTalkingBossID = 0;
        }
        if (m_uTalkingBossID != 0)
        {
            table.BossTalkDataBase db = GameTableManager.Instance.GetTableItem<table.BossTalkDataBase>((uint)m_uTalkingBossID);
            if (db != null)
            {
                table.LangTextDataBase ldb = GameTableManager.Instance.GetTableItem<table.LangTextDataBase>(db.textID);
                if (ldb != null)
                {
                    if (!string.IsNullOrEmpty(ldb.talkVoice))
                    {
                        List<uint> idList = StringUtil.GetSplitStringList<uint>(ldb.talkVoice);
                        if (idList != null && idList.Count > 0)
                        {
                            uint audioID = idList[0];
                            if (audioID != 0)
                            {
                                DataManager.Manager<PetDataManager>().PlayCreateAudio(audioID);
                            }
                        }
                    }

                }
                if (TimerAxis.Instance().IsExist(m_uBossTalkTimerID, this))
                {
                    TimerAxis.Instance().KillTimer(m_uBossTalkTimerID, this);
                }
                TimerAxis.Instance().SetTimer(m_uBossTalkTimerID, db.aliveTime * 1000, this, 1);
                TimerAxis.Instance().SetTimer(m_uRefreshStatusTimerID, 1000, this, 1);
            }
        }
        RefreshAllRoleBarHeadStatus(HeadStatusType.BossSay);
    }
    public bool GetBossTalkVisible()
    {
        return m_uTalkingBossID == 0 ? false : true;
    }
    public uint GetTalkingBossID()
    {
        return m_uTalkingBossID;
    }
    /// <summary>
    /// 设置称号是否可以见
    /// </summary>
    /// <param name="visible"></param>
    public static void SetTitleNameVisible(bool visible)
    {
        m_bPlayerTitleEnable = visible;
        RefreshAllRoleBarHeadStatus(HeadStatusType.Title);
        //RoleStateBarManagerOld.Instance().SetPlayerTitleVisible(visible);
    }

    /// <summary>
    /// 设置氏族名称是否可见
    /// </summary>
    /// <param name="visible"></param>
    public static void SetClanNameVisible(bool visible)
    {
        m_bClanNameEnable = visible;
        RefreshAllRoleBarHeadStatus(HeadStatusType.Clan);
        //RoleStateBarManagerOld.Instance().SetPlayerClanNameVisible(visible);
    }

    /// <summary>
    /// 设置玩家名称是否可见
    /// </summary>
    /// <param name="visible"></param>
    public static void SetPlayerNameVisible(bool visible)
    {
        m_bPlayerNameEnable = visible;
        RefreshAllRoleBarHeadStatus(HeadStatusType.Name);
        //RoleStateBarManagerOld.Instance().SetPlayerNameVisible(visible);
    }

    /// <summary>
    /// 设置npc名称是否可见
    /// </summary>
    /// <param name="visible"></param>
    public static void SetNpcNameVisible(bool visible)
    {
        RefreshAllRoleBarHeadStatus(HeadStatusType.Name);
        //RoleStateBarManagerOld.Instance().SetNpcNameVisible(visible);
    }

    /// <summary>
    /// 设置血条是否可见
    /// </summary>
    /// <param name="visible"></param>
    public static void SetHpSliderVisible(bool visible)
    {
        RefreshAllRoleBarHeadStatus(HeadStatusType.Hp);
        //RoleStateBarManagerOld.Instance().SetHpSliderVisible(visible);
    }

    /// <summary>
    /// 显示实体顶部状态面板
    /// </summary>
    public static void ShowHeadStatus()
    {
        if (!DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RoleStateBarPanel) && PlayerNameEnable)
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RoleStateBarPanel);
        else
        {
            HideHeadStatus();
        }
    }

    /// <summary>
    /// 隐藏实体顶部状态面板
    /// </summary>
    public static void HideHeadStatus()
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.RoleStateBarPanel);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    public static void RefreshAllRoleBarHeadStatus(HeadStatusType type)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.RFRESHENTITYHEADSTATUS, type);
    }

    /// <summary>
    /// 根据UID获取实体
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static IEntity GetEntity(long uid)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            return es.FindEntity(uid);
        }
        return null;
    }

    public static T GetEntityByUserID<T>(uint userId) where T : IEntity
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            return es.FindEntity<T>(userId);
        }
        return default(T);
    }
    /// <summary>
    /// 根据baseID获取npc
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    public static INPC GetNPCByBaseID(uint uid)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if (es != null)
        {
            return es.FindNPCByBaseId((int)uid);
        }
        return null;
    }

    /// <summary>
    /// 获取状态条在世界坐标的位置
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="strLocatorName"></param>
    /// <param name="offsetY"></param>
    /// <returns></returns>
    public Vector3 GetNodeWorldPos(long uid, string strLocatorName, float offsetY = 1)
    {
        Vector3 pos = Vector3.zero;
        IEntity entity = GetEntity(uid);
        if (entity == null)
        {
            Engine.Utility.Log.Error("GetNodeWorldPos ： entity is null");
            return pos;
        }

        Engine.Node node = entity.GetNode();
        if (node == null)
        {
            //Engine.Utility.Log.Error("GetNodeWorldPos ： entity node is null");
            return pos;
        }

        Transform trans = node.GetTransForm();
        if (trans == null)
        {
            Engine.Utility.Log.Error("GetNodeWorldPos ： entity transform is null");
            return pos;
        }

        entity.GetLocatorPos(strLocatorName, Vector3.zero, Quaternion.identity, ref pos, true);

        pos.y += offsetY * trans.lossyScale.y;
        return pos;
    }


    #endregion

    #region Hp（血条）
    public static string GetHpSpriteName(HpSpriteType type)
    {
        int key = (int)type;
        if (HpSpriteName_Dic.ContainsKey(key))
        {
            return HpSpriteName_Dic[key];
        }
        else
        {
            string value = GameTableManager.Instance.GetGlobalConfig<string>("HpSpriteName", type.ToString());
            HpSpriteName_Dic.Add(key, value);
            return value;
        }
        return "";
    }

    public static bool IsNeedHpSlider(Client.IEntity entity)
    {
        if (entity.GetEntityType() == EntityType.EntityType_NPC)
        {
            INPC npc = entity as INPC;
            //是可以攻击的NPC
            if (npc.IsCanAttackNPC())
            {
                Client.ISkillPart skillPart = MainPlayerHelper.GetMainPlayer().GetPart(EntityPart.Skill) as Client.ISkillPart;
                bool canAttack = true;
                if (skillPart != null)
                {
                    int skillerror = 0;
                    canAttack = skillPart.CheckCanAttackTarget(entity, out skillerror);
                }
                if (npc.IsPet())
                {
                    if (npc.IsMainPlayerSlave())
                    {
                        return true;
                    }
                    else
                    {
                        if (canAttack)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else if (npc.IsSummon())
                {
                    if (npc.IsMainPlayerSlave())
                    {
                        return true;
                    }
                    else
                    {
                        if (canAttack)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                else if (npc.IsMonster())
                {
                    int hp = entity.GetProp((int)CreatureProp.Hp);
                    int maxhp = entity.GetProp((int)CreatureProp.MaxHp);
                    return hp < maxhp;
                }
            }
            else
            {
                table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)entity.GetProp((int)Client.EntityProp.BaseID));
                if (npcdb != null)
                {
                    if (npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_TRANSFER || npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_COLLECT_PLANT
                        || npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_FUNCTION)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    static bool EntityCanAttack(IEntity entity)
    {
        bool canAttack = true;
        Client.ISkillPart skillPart = MainPlayerHelper.GetMainPlayer().GetPart(EntityPart.Skill) as Client.ISkillPart;
        if (skillPart != null)
        {
            int skillerror = 0;
            canAttack = skillPart.CheckCanAttackTarget(entity, out skillerror);
        }
        return canAttack;
    }
    /// <summary>
    /// 获取实体血条
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static EntityHpSprite GetEntityHpSpData(IEntity entity)
    {
        EntityHpSprite spriteParams = new EntityHpSprite();
        spriteParams.spriteType = UISprite.Type.Filled;
        if (entity.GetEntityType() == EntityType.EntityType_Player)
        {
            GameCmd.eCamp mycamp = (GameCmd.eCamp)MainPlayerHelper.GetMainPlayer().GetProp((int)CreatureProp.Camp);
            GameCmd.eCamp camp = (GameCmd.eCamp)entity.GetProp((int)CreatureProp.Camp);
            spriteParams.bgSpriteName = GetHpSpriteName(HpSpriteType.PlayerBg);
            if (entity == MainPlayerHelper.GetMainPlayer())
            {
                spriteParams.spriteName = GetHpSpriteName(HpSpriteType.Me);
            }
            else
            {
                if (EntityCanAttack(entity))
                {
                    spriteParams.spriteName = GetHpSpriteName(HpSpriteType.Enemy);
                }
                else
                {
                    spriteParams.spriteName = GetHpSpriteName(HpSpriteType.Friend);
                }
            }
            spriteParams.bShow = true;

        }
        else if (entity.GetEntityType() == EntityType.EntityType_NPC)
        {
            INPC npc = entity as INPC;
            //任意NPC先给一个默认的血条
            spriteParams.bgSpriteName = GetHpSpriteName(HpSpriteType.NoneBg);
            spriteParams.spriteName = GetHpSpriteName(HpSpriteType.None);
            spriteParams.bShow = IsNeedHpSlider(entity);
            if (npc.WhetherShowHeadTips())
            {

                //是可以攻击的NPC
                if (npc.IsCanAttackNPC())
                {
                    //是不是佣兵
                    if (npc.IsMercenary())
                    {
                        spriteParams.spriteName = GetHpSpriteName(HpSpriteType.Friend);
                        spriteParams.bgSpriteName = GetHpSpriteName(HpSpriteType.PlayerBg);
                        spriteParams.bShow = true;
                    }
                    else
                    {
                        if (npc.IsPet())
                        {
                            spriteParams.bgSpriteName = GetHpSpriteName(HpSpriteType.PetBg);
                            if (npc.IsMainPlayerSlave())
                            {
                                spriteParams.spriteName = GetHpSpriteName(HpSpriteType.MyPet);
                            }
                            else
                            {
                                if (EntityCanAttack(entity))
                                {
                                    spriteParams.spriteName = GetHpSpriteName(HpSpriteType.EnemyPet);
                                }
                            }
                        }
                        else if (npc.IsSummon())
                        {
                            spriteParams.bgSpriteName = GetHpSpriteName(HpSpriteType.SummonBg);
                            if (npc.IsMainPlayerSlave())
                            {
                                spriteParams.spriteName = GetHpSpriteName(HpSpriteType.MySummon);
                            }
                            else
                            {
                                if (EntityCanAttack(entity))
                                {
                                    spriteParams.spriteName = GetHpSpriteName(HpSpriteType.EnemySummon);
                                }
                            }
                        }
                        else if (npc.IsMonster())
                        {
                            spriteParams.bgSpriteName = GetHpSpriteName(HpSpriteType.MonsterBg);
                            spriteParams.spriteName = GetHpSpriteName(HpSpriteType.Monster);
                            spriteParams.spriteType = UISprite.Type.Simple;
                        }
                    }

                }
            }
            else
            {
                spriteParams.bShow = false;
            }

        }
        else if (entity.GetEntityType() == EntityType.EntityTYpe_Robot)
        {

            GameCmd.eCamp mycamp = (GameCmd.eCamp)MainPlayerHelper.GetMainPlayer().GetProp((int)CreatureProp.Camp);
            GameCmd.eCamp camp = (GameCmd.eCamp)entity.GetProp((int)CreatureProp.Camp);
            spriteParams.bgSpriteName = GetHpSpriteName(HpSpriteType.PlayerBg);
            if (entity == MainPlayerHelper.GetMainPlayer())
            {
                spriteParams.spriteName = GetHpSpriteName(HpSpriteType.Me);
            }
            else
            {
                if (EntityCanAttack(entity))
                {
                    spriteParams.spriteName = GetHpSpriteName(HpSpriteType.Enemy);
                }
                else
                {
                    spriteParams.spriteName = GetHpSpriteName(HpSpriteType.Friend);
                }
            }
            spriteParams.bShow = true;
        }

        return spriteParams;
    }

    /// <summary>
    /// 是否实体顶部栏可用
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="statusType"></param>
    /// <returns></returns>
    public static bool IsEntityHeadStatusTypeEnable(IEntity entity, HeadStatusType statusType)
    {
        bool enable = false;
        if (null != entity)
        {
            switch (statusType)
            {
                case HeadStatusType.Hp:
                    {
                        EntityHpSprite hpSp = RoleStateBarManager.GetEntityHpSpData(entity);
                        if (hpSp != null)
                        {
                            enable = hpSp.bShow;
                        }
                    }
                    break;
                case HeadStatusType.Name:
                    {
                        enable = PlayerNameEnable;
                        if (entity.GetEntityType() == EntityType.EntityType_NPC)
                        {
                            INPC npc = entity as INPC;
                            enable = npc.WhetherShowHeadTips();
                        }
                    }
                    break;
                case HeadStatusType.Clan:
                    {
                        uint clanIdLow = (uint)entity.GetProp((int)CreatureProp.ClanIdLow);
                        uint clanIdHigh = (uint)entity.GetProp((int)CreatureProp.ClanIdHigh);
                        uint clanid = (clanIdHigh << 16) | clanIdLow;
                        enable = ClanNameEnable && (clanid != 0);

                        //enable = ClanNameEnable && ((uint)entity.GetProp((int)CreatureProp.ClanId) != 0);
                    }
                    break;
                case HeadStatusType.Title:
                    {
                        enable = PlayerTitleEnable && (entity.GetEntityType() == EntityType.EntityType_Player
                            && (uint)entity.GetProp((int)PlayerProp.TitleId) != 0);
                    }
                    break;
                case HeadStatusType.Collect:
                    {

                    }
                    break;
                case HeadStatusType.HeadMaskIcon:
                    {
                        enable = IsEntityHaveHeadIconMask(entity);
                    }
                    break;
                case HeadStatusType.TaskStatus:
                    {
                        if (entity.GetEntityType() == EntityType.EntityType_NPC)
                        {
                            QuestTraceInfo info = null;
                            if (TryGetNPCTraceInfo(entity, out info))
                            {
                                enable = true;
                            }
                        }
                    }
                    break;
                case HeadStatusType.CampMask:
                    {
                        int camp = entity.GetProp((int)Client.CreatureProp.Camp);
                        if (camp == (int)GameCmd.eCamp.CF_Green || camp == (int)GameCmd.eCamp.CF_Red)
                        {
                            enable = true;
                        }
                    }
                    break;
                case HeadStatusType.BossSay:
                    {
                        int baseID = entity.GetProp((int)EntityProp.BaseID);
                        if (baseID == DataManager.Manager<RoleStateBarManager>().GetTalkingBossID())
                        {
                            enable = DataManager.Manager<RoleStateBarManager>().GetBossTalkVisible();
                        }
                        else
                        {
                            enable = false;
                        }
                    }
                    break;
            }
        }
        return enable;
    }



    /// <summary>
    /// 是否实体顶部栏可用
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="statusType"></param>
    /// <returns></returns>
    public static bool IsEntityHeadStatusTypeEnable(long uid, HeadStatusType statusType)
    {
        return IsEntityHeadStatusTypeEnable(GetEntity(uid), statusType);
    }

    #endregion

    #region 名称（Name）
    public ColorType GetNameColor(IEntity entity)
    {
        ColorType nameColor = ColorType.White;
        if (entity.GetEntityType() == EntityType.EntityType_Player || entity.GetEntityType() == EntityType.EntityTYpe_Robot)
        {
            nameColor = GetPlayerNameColor(entity);
        }
        else if (entity.GetEntityType() == EntityType.EntityType_NPC)
        {
            nameColor = GetNpcNameColor(entity);
        }
        else if (entity.GetEntityType() == EntityType.EntityType_Pet)
        {
            nameColor = ColorType.JSXT_ZhanHun;
        }
        return nameColor;
    }

    public ColorType GetPlayerNameColor(IEntity en)
    {
        ColorType color = ColorType.White;
        GameCmd.eCamp mycamp = (GameCmd.eCamp)MainPlayerHelper.GetMainPlayer().GetProp((int)CreatureProp.Camp);
        GameCmd.eCamp camp = (GameCmd.eCamp)en.GetProp((int)CreatureProp.Camp);
        if (camp != GameCmd.eCamp.CF_None && mycamp != GameCmd.eCamp.CF_None)
        {
            if (camp != mycamp)
            {
                int pkvalue = en.GetProp((int)PlayerProp.PKValue);
                switch (pkvalue)
                {
                    case 0:
                        color = ColorType.JSXT_Enemy_White;
                        break;
                    case 1:
                        color = ColorType.JSXT_Enemy_Gray;
                        break;
                    case 2:
                        color = ColorType.JSXT_Enemy_Yellow;
                        break;
                    case 3:
                        color = ColorType.JSXT_Enemy_Red;
                        break;
                }
                return color;

            }
            else if (camp == mycamp)
            {
                return ColorType.White;
            }
        }

        GameCmd.enumGoodNess goodNess = (GameCmd.enumGoodNess)en.GetProp((int)PlayerProp.GoodNess);

        switch (goodNess)
        {
            case GameCmd.enumGoodNess.GoodNess_Badman:
                color = ColorType.JSXT_Enemy_Yellow;
                break;
            case GameCmd.enumGoodNess.GoodNess_Evil:
                color = ColorType.JSXT_Enemy_Red;
                break;
            case GameCmd.enumGoodNess.GoodNess_Hero:
                break;
            case GameCmd.enumGoodNess.GoodNess_Normal:
                color = ColorType.JSXT_Enemy_White;
                break;
            case GameCmd.enumGoodNess.GoodNess_Rioter:
                color = ColorType.JSXT_Enemy_Gray;
                break;
            default:
                break;
        }

        return color;
    }

    public ColorType GetNpcNameColor(IEntity entity)
    {
        ColorType nameColor = ColorType.JSXT_NpcCheFu;
        INPC npc = entity as INPC;
        if (npc.IsCanAttackNPC())
        {
            if (npc.IsMercenary())
            {
                nameColor = ColorType.JSXT_Enemy_White;
            }
            else
            {
                if (npc.IsPet())
                {
                    nameColor = ColorType.JSXT_ZhanHun;
                }
                else if (npc.IsSummon())
                {
                    nameColor = ColorType.JSXT_ZhaoHuanWu;
                }
                else
                {
                    int level = MainPlayerHelper.GetPlayerLevel();
                    int monsterLv = entity.GetProp((int)CreatureProp.Level);
                    int levelGap = GameTableManager.Instance.GetGlobalConfig<int>("MonsterWarningLevelGap");
                    if (level + levelGap < monsterLv)
                    {
                        nameColor = ColorType.JSXT_Enemy_Red;
                    }
                    else
                    {
                        nameColor = ColorType.JSXT_Enemy_White;
                    }
                }
            }
        }
        else
        {
            table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)entity.GetProp((int)Client.EntityProp.BaseID));
            if (npcdb != null)
            {
                if (npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_COLLECT_PLANT)
                {
                    nameColor = ColorType.JSXT_CaiJiWu;
                }
                else if (npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_TRANSFER || npcdb.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_FUNCTION)
                {
                    nameColor = ColorType.JSXT_ChuanSongZhen;
                }
                else
                {
                    nameColor = ColorType.White;
                }
            }

        }

        return nameColor;
    }

    #endregion

    #region 氏族（Clan）

    public static void GetRoleBarClanName(IEntity entity, Action<string, int> nameBackDlg, int getNameSeed)
    {
        if (entity == null)
        {
            Engine.Utility.Log.Error("AddClanName entity is null");
        }
        EntityType entityType = entity.GetEntityType();
        if (entityType == EntityType.EntityType_Player)
        {
            uint clanIdLow = (uint)entity.GetProp((int)CreatureProp.ClanIdLow);
            uint clanIdHigh = (uint)entity.GetProp((int)CreatureProp.ClanIdHigh);
            uint clanId = (clanIdHigh << 16) | clanIdLow;

            //int clanId = entity.GetProp((int)Client.CreatureProp.ClanId);
            if (clanId != 0)
            {
                DataManager.Manager<ClanManger>().GetClanName((uint)clanId, (namedata) =>
                {
                    string winerCityName = string.Empty;
                    string name = string.Empty;
                    if (DataManager.Manager<CityWarManager>().GetWinerOfCityWarCityName((uint)clanId, out winerCityName))
                    {
                        //name = winerCityName +  namedata.ClanName ;
                        name = string.Format("{0}【{1}】", winerCityName, namedata.ClanName);
                    }
                    else
                    {
                        //name = namedata.ClanName;
                        name = string.Format("【{0}】", namedata.ClanName);
                    }
                    if (null != nameBackDlg)
                    {
                        nameBackDlg.Invoke(name, getNameSeed);
                    }
                });
            }
        }
        else if (entity.GetEntityType() == EntityType.EntityType_NPC)
        {
            CityWarTotem cityWarTotemInfo = null;
            if (DataManager.Manager<CityWarManager>().GetCityWarTotemClanName(
                (uint)entity.GetProp((int)Client.EntityProp.BaseID), out cityWarTotemInfo))
            {
                if (null != nameBackDlg)
                {
                    nameBackDlg.Invoke(cityWarTotemInfo.clanName, getNameSeed);
                }
            }
        }
    }
    /// <summary>
    /// 获取氏族名称颜色
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static ColorType GetClanNameColor(IEntity entity)
    {
        ColorType nameColor = ColorType.JSXT_Clan;
        if (DataManager.Manager<ClanManger>().ClanInfo != null)
        {
            uint clanid = 0;
            if (entity.GetEntityType() == EntityType.EntityType_NPC)
            {
                CityWarTotem cityWarTotemInfo = null;
                if (DataManager.Manager<CityWarManager>().GetCityWarTotemClanName((uint)entity.GetProp((int)Client.EntityProp.BaseID)
                    , out cityWarTotemInfo))
                {
                    clanid = cityWarTotemInfo.clanId;
                }
            }
            else if (entity.GetEntityType() == EntityType.EntityType_Player)
            {
                uint clanIdLow = (uint)entity.GetProp((int)CreatureProp.ClanIdLow);
                uint clanIdHigh = (uint)entity.GetProp((int)CreatureProp.ClanIdHigh);
                clanid = (clanIdHigh << 16) | clanIdLow;

                //clanid = (uint)entity.GetProp((int)Client.CreatureProp.ClanId);
            }

            if (clanid == DataManager.Manager<ClanManger>().ClanInfo.Id)
            {
                nameColor = ColorType.JSXT_Clan;
            }
            else if (DataManager.Manager<ClanManger>().GetClanRivalryInfo((uint)clanid) != null)
            {
                nameColor = ColorType.JSXT_Enemy_Red;
            }
        }
        return nameColor;
    }
    #endregion

    #region 称号（Title）
    public static table.TitleDataBase GetTitleText(IEntity entity)
    {
        int titleId = entity.GetProp((int)PlayerProp.TitleId);
        string titleText = "";
        if (titleId != 0)
        {
            table.TitleDataBase tdb = GameTableManager.Instance.GetTableItem<table.TitleDataBase>((uint)titleId);
            return tdb;
        }

        return null;
    }
    #endregion

    #region 采集（Collect）

    #endregion

    #region 头顶功能标识（HeadMaskIcon）
    /// <summary>
    /// 获取NPC头顶标识数据
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static table.NpcHeadMaskDataBase GetNPCHeadMaskDB(Client.IEntity entity)
    {
        table.NpcHeadMaskDataBase db = null;
        if (null != entity
            //&& entity.GetEntityType() == EntityType.EntityType_NPC
            )
        {
            db = GetNPCHeadMaskDB((uint)entity.GetProp((int)Client.EntityProp.BaseID));
        }

        return db;
    }

    /// <summary>
    /// 获取NPC头顶标识数据
    /// </summary>
    /// <param name="npcBaseId"></param>
    /// <returns></returns>
    public static table.NpcHeadMaskDataBase GetNPCHeadMaskDB(uint npcBaseId)
    {
        table.NpcHeadMaskDataBase db = null;
        if (npcBaseId != 0
            //&& entity.GetEntityType() == EntityType.EntityType_NPC
            )
        {
            table.NpcDataBase npcdb
                = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(npcBaseId);
            if (null != npcdb && npcdb.npcHeadMaskID != 0)
            {
                db = GameTableManager.Instance.GetTableItem<table.NpcHeadMaskDataBase>(npcdb.npcHeadMaskID);
            }
        }

        return db;
    }

    /// <summary>
    /// 是否有头顶标识
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static bool IsEntityHaveHeadIconMask(Client.IEntity entity)
    {
        return (GetNPCHeadMaskDB(entity) != null);
    }

    /// <summary>
    /// 是否有头顶标识
    /// </summary>
    /// <param name="npcBaseID"></param>
    /// <returns></returns>
    public static bool IsEntityHaveHeadIconMask(uint npcBaseID)
    {
        return (GetNPCHeadMaskDB(npcBaseID) != null);
    }
    #endregion

    #region 任务状态（TaskStatus)

    public static string GetQuestStatusIcon(GameCmd.TaskProcess process)
    {
        string icon = "";
        if (process == GameCmd.TaskProcess.TaskProcess_CanDone)
        {
            icon = "tubiao_wenhao_huang";
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_None)//可接
        {
            icon = "tubiao_tanhao_huang";
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_Doing)
        {
            icon = "tubiao_wenhao_hui";
        }
        return icon;
    }
    public static bool TryGetQuestStatusIcon(QuestTraceInfo info, out long uid, out string icon)
    {
        icon = string.Empty;
        uid = 0;
        if (null == info)
        {
            return false;
        }

        GameCmd.TaskProcess process = info.GetTaskProcess();

        uint npcid = 0;
        icon = GetQuestStatusIcon(process);
        if (process == GameCmd.TaskProcess.TaskProcess_CanDone)
        {
            npcid = info.endNpc;
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_None)//可接
        {
            npcid = info.beginNpc;
        }
        else if (process == GameCmd.TaskProcess.TaskProcess_Doing)
        {
            npcid = info.endNpc;
        }

        Client.INPC npc = GetNPCByBaseID(npcid);
        if (npc == null)
        {
            Engine.Utility.Log.Info("查找不到npc{0}", npcid);
            return false;
        }
        uid = npc.GetUID();
        return true;

    }


    public static bool TryGetQuestStatusIcon(uint taskId, out long uid, out string icon)
    {
        return TryGetQuestStatusIcon(QuestTranceManager.Instance.GetQuestTraceInfo(taskId), out uid, out icon);
    }

    /// <summary>
    /// 尝试获取npc任务信息
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="info"></param>
    /// <returns></returns>
    public static bool TryGetNPCTraceInfo(IEntity entity, out QuestTraceInfo info)
    {
        info = null;
        if (null == entity || entity.GetEntityType() != EntityType.EntityType_NPC/* || !(entity is Client.INPC)*/)
        {
            return false;
        }
        //INPC npc = (INPC)entity;
        //if (npc.IsMonster())
        //    return null;
        List<QuestTraceInfo> traceTask;
        DataManager.Manager<TaskDataManager>().GetAllQuestTraceInfo(out traceTask);

        int npcBaseID = entity.GetProp((int)Client.EntityProp.BaseID);
        GameCmd.TaskProcess process = GameCmd.TaskProcess.TaskProcess_Max;
        for (int i = 0; i < traceTask.Count; i++)
        {
            info = traceTask[i];
            process = info.GetTaskProcess();
            if (process == GameCmd.TaskProcess.TaskProcess_CanDone ||
                process == GameCmd.TaskProcess.TaskProcess_Doing)
            {
                if (info.endNpc == npcBaseID)
                {
                    return true;
                }
            }
            else if (process == GameCmd.TaskProcess.TaskProcess_None)
            {
                if (info.beginNpc == npcBaseID)
                {
                    return true;
                }
            }
        }
        return false;
    }
    #endregion

    uint m_uBossTalkTimerID = 100002;
    uint m_uRefreshStatusTimerID = 100003;
    public void OnTimer(uint uTimerID)
    {
        if (m_uBossTalkTimerID == uTimerID)
        {
            SetTalkingBossID(0);
        }
        else if (m_uRefreshStatusTimerID == uTimerID)
        {
            RefreshAllRoleBarHeadStatus(HeadStatusType.BossSay);
        }
    }
}