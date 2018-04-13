using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using GameCmd;

// 角色工具方法
class RoleUtil
{
    public static enmCharSex FaceToSex(uint face)
    {
        return face < 100 ? enmCharSex.MALE : enmCharSex.FEMALE;
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 构建实体创建数据
    @param type 实体类型
    @param data 服务器数据
    */
    public static EntityCreateData BuildCreateEntityData(EntityType type, GameCmd.t_MapUserData data)
    {
        EntityCreateData entityData = new EntityCreateData();
        // 构建EntityCreateData
        entityData.ID = data.userdata.dwUserID;

        if (UserData.IsMainRoleID(data.userdata.dwUserID))
        {
            entityData.strName = UserData.CurrentRole.name;
        }
        else
        {
            entityData.strName = "";
        }

        switch (type)
        {
            case EntityType.EntityType_Player:
                {
                    entityData.PropList = new EntityAttr[(int)PlayerProp.End - (int)EntityProp.Begin];
                    entityData.ViewList = new EntityViewProp[(int)Client.EquipPos.EquipPos_Max];
                    BuildPlayerPropList(data, ref entityData.PropList);
                    // 根据时装数据决定是否使用外观
                    BuildPlayerViewProp(data, ref entityData.ViewList);
                    break;
                }
        }

        return entityData;
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 构建实体创建数据
    @param type 实体类型
    @param data 服务器数据
    */
    public static EntityCreateData BuildCreateEntityData(EntityType type, GameCmd.t_MapNpcDataPos data, uint master_id)
    {
        if (type == EntityType.EntityTYpe_Robot)
        {
            t_NpcData npcdata = data.mapnpcdata.npcdata;
            return BuildRobotEntityData(npcdata.dwTempID, npcdata.dwBaseID, npcdata.job, npcdata.level, npcdata.name, npcdata.movespeed);
        }

        EntityCreateData entityData = new EntityCreateData();
        // 构建EntityCreateData
        entityData.ID = data.mapnpcdata.npcdata.dwTempID;
        entityData.strName = data.mapnpcdata.npcdata.name;

        switch (type)
        {
            case EntityType.EntityType_Monster:
                {
                    entityData.PropList = new EntityAttr[(int)NPCProp.End - (int)EntityProp.Begin];
                    entityData.ViewList = new EntityViewProp[(int)Client.EquipPos.EquipPos_Max];
                    BuildNPCPropList(data, ref entityData.PropList);
                    break;
                }
            case EntityType.EntityType_NPC:
                {
                    entityData.PropList = new EntityAttr[(int)NPCProp.End - (int)EntityProp.Begin];
                    entityData.ViewList = new EntityViewProp[(int)Client.EquipPos.EquipPos_Max];
                    BuildNPCPropList(data, ref entityData.PropList, master_id);
                    // 根据时装数据决定是否使用外观
                    BuildNPCViewProp(data, ref entityData.ViewList);
                    break;
                }
        }

        return entityData;
    }

    public static EntityCreateData BuildRobotEntityData(uint dwTempID, uint baseID, uint job, uint level, string name = "", uint moveSpeed = 1)
    {
        EntityCreateData entityData = new EntityCreateData();
        entityData.ID = dwTempID;
        table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(baseID);
        if (npcdb != null)
        {
            if (string.IsNullOrEmpty(name))
            {
                entityData.strName = npcdb.strName;
            }
            else
            {
                entityData.strName = name;
            }
        }
        entityData.PropList = new EntityAttr[(int)RobotProp.End - (int)EntityProp.Begin];
        entityData.ViewList = new EntityViewProp[(int)Client.EquipPos.EquipPos_Max];
        table.RobotDataBase robotdata = GameTableManager.Instance.GetTableItem<table.RobotDataBase>(job, (int)level);
        if (robotdata != null)
        {
            BuildRobotPropList(robotdata, npcdb, moveSpeed, ref entityData.PropList);
            BuildRobotViewProp(robotdata, ref entityData.ViewList);
        }
        return entityData;
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 构建实体创建数据
    @param type 实体类型
    @param data 服务器数据
    */
    public static EntityCreateData BuildCreateEntityData(EntityType type, t_MapObjectData data)
    {
        EntityCreateData entityData = new EntityCreateData();
        // 构建EntityCreateData
        entityData.ID = data.qwThisID;
        entityData.strName = "";

        switch (type)
        {
            case EntityType.EntityType_Box:
                {
                    entityData.PropList = new EntityAttr[(int)BoxProp.End - (int)EntityProp.Begin];
                    BuildBoxPropList(data, ref entityData.PropList);
                    break;
                }
        }

        return entityData;
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief 构建Box属性列表
    @param 
    */
    private static void BuildBoxPropList(t_MapObjectData data, ref EntityAttr[] propList)
    {
        if (propList.Length < 0 || propList.Length > (int)BoxProp.End - (int)EntityProp.Begin)
        {
            Engine.Utility.Log.Error("BuildPlayerPropList:属性列表长度非法");
            return;
        }

        int index = 0;
        propList[index++] = new EntityAttr((int)EntityProp.BaseID, (int)data.dwObjectID); // 玩家没有模板ID 0则使用默认数据 需要根据职业和性别来获取 需要读取配置表
        propList[index++] = new EntityAttr((int)BoxProp.Number, (int)data.wdNumber);
        propList[index++] = new EntityAttr((int)BoxProp.OwnerType, (int)data.byOwnerType);
        //propList[index++] = new EntityAttr((int)BoxProp.Owner, (int)data.dwOwner);

        //氏族ID分为两个存
        uint owner = data.dwOwner;
        int ownerLow = (int)(owner & 0x0000ffff);
        int ownerHigh = (int)(owner >> 16);
        propList[index++] = new EntityAttr((int)BoxProp.OwnerLow, ownerLow);
        propList[index++] = new EntityAttr((int)BoxProp.OwnerHigh, ownerHigh);

        uint low = (uint)ownerLow;
        uint high = (uint)ownerHigh;
        uint newhigh = high << 16;
        uint newOwner = newhigh | low;
        Engine.Utility.Log.Error("--->>> 服务器owner：" + data.dwOwner);
        Engine.Utility.Log.Error("--->>> 客户端owner：" + newOwner);
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief  构建玩家属性列表 
    @param 
    */
    private static void BuildPlayerPropList(GameCmd.t_MapUserData data, ref EntityAttr[] propList)
    {
        if (propList.Length < 0 || propList.Length > (int)PlayerProp.End - (int)EntityProp.Begin)
        {
            Engine.Utility.Log.Error("BuildPlayerPropList:属性列表长度非法");
            return;
        }
        int index = 0;
        propList[index++] = new EntityAttr((int)EntityProp.BaseID, 1); // 玩家没有模板ID 0则使用默认数据 需要根据职业和性别来获取 需要读取配置表
        propList[index++] = new EntityAttr((int)CreatureProp.Hp, (int)data.userdata.curhp);
        propList[index++] = new EntityAttr((int)CreatureProp.MaxHp, (int)data.userdata.maxhp);
        propList[index++] = new EntityAttr((int)CreatureProp.Level, (int)data.userdata.level);
        propList[index++] = new EntityAttr((int)WorldObjProp.MoveSpeed, (int)data.userdata.movespeed);
        propList[index++] = new EntityAttr((int)PlayerProp.Job, (int)data.userdata.type);
        propList[index++] = new EntityAttr((int)PlayerProp.Country, (int)1);
        propList[index++] = new EntityAttr((int)CreatureProp.Face, (int)1);
        propList[index++] = new EntityAttr((int)PlayerProp.Sex, (int)FaceToSex(1));
        propList[index++] = new EntityAttr((int)PlayerProp.GoodNess, (int)data.userdata.goodstate);
        propList[index++] = new EntityAttr((int)PlayerProp.RideBaseId, (int)data.userdata.ride_base_id);
        propList[index++] = new EntityAttr((int)PlayerProp.TransModelResId, (int)data.userdata.trans_model_id);
        propList[index++] = new EntityAttr((int)PlayerProp.StateBit, (int)0);
        propList[index++] = new EntityAttr((int)EntityProp.EntityState, (int)0);
        //氏族id
        //propList[index++] = new EntityAttr((int)CreatureProp.ClanId, (int)data.userdata.clan_id);
        propList[index++] = new EntityAttr((int)PlayerProp.TitleId, (int)data.userdata.title_id);//称号Id
        propList[index++] = new EntityAttr((int)PlayerProp.GodLevel, (int)data.userdata.god_level);//神魔等级
        propList[index++] = new EntityAttr((int)CreatureProp.Camp, (int)data.userdata.camp);
        propList[index++] = new EntityAttr((int)PlayerProp.SkillStatus, (int)(data.userdata.skill_status + 1));

        //氏族ID分为两个存
        uint clanId = data.userdata.clan_id;
        int clanIdLow = (int)(clanId & 0x0000ffff);
        int clanIdHigh = (int)(clanId >> 16);
        propList[index++] = new EntityAttr((int)CreatureProp.ClanIdLow, clanIdLow);
        propList[index++] = new EntityAttr((int)CreatureProp.ClanIdHigh, clanIdHigh);

        //验证代码

        uint low = (uint)clanIdLow;
        uint high = (uint)clanIdHigh;
        uint newhigh = high << 16;
        uint newClanId = newhigh | low;

        Engine.Utility.Log.Error("--->>> 服务器发的：" + data.userdata.clan_id);
        Engine.Utility.Log.Error("--->>> 客户端存的：" + newClanId);
        Engine.Utility.Log.Error("---客户端存的低位：" + low);
        Engine.Utility.Log.Error("---客户端存的高位：" + newhigh);
        
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief  构建玩家属性列表 
    @param 
    */
    public static void BuildPlayerPropList(GameCmd.t_UserData data, ref EntityAttr[] propList)
    {
        if (propList.Length < 0 || propList.Length > (int)PlayerProp.End - (int)EntityProp.Begin)
        {
            Engine.Utility.Log.Error("BuildPlayerPropList:属性列表长度非法");
            return;
        }

        int index = 0;
        propList[index++] = new EntityAttr((int)EntityProp.BaseID, 1); // 玩家没有模板ID 0则使用默认数据 需要根据职业和性别来获取 需要读取配置表
        propList[index++] = new EntityAttr((int)CreatureProp.Hp, (int)data.curhp);
        propList[index++] = new EntityAttr((int)CreatureProp.MaxHp, (int)data.maxhp);
        propList[index++] = new EntityAttr((int)CreatureProp.Level, (int)data.level);
        propList[index++] = new EntityAttr((int)WorldObjProp.MoveSpeed, (int)data.movespeed);
        propList[index++] = new EntityAttr((int)PlayerProp.Job, (int)data.type);
        propList[index++] = new EntityAttr((int)PlayerProp.Country, (int)1);
        propList[index++] = new EntityAttr((int)CreatureProp.Face, (int)1);
        propList[index++] = new EntityAttr((int)PlayerProp.Sex, (int)FaceToSex(1));
        propList[index++] = new EntityAttr((int)PlayerProp.StateBit, (int)0);
        propList[index++] = new EntityAttr((int)EntityProp.EntityState, (int)0);
        //propList[index++] = new EntityAttr((int)CreatureProp.ClanId, (int)data.clan_id);
        propList[index++] = new EntityAttr((int)PlayerProp.TitleId, (int)data.title_id);
        propList[index++] = new EntityAttr((int)PlayerProp.GodLevel, (int)data.god_level);
        propList[index++] = new EntityAttr((int)CreatureProp.Camp, (int)data.camp);
        propList[index++] = new EntityAttr((int)PlayerProp.TransModelResId, (int)data.trans_model_id);
        propList[index++] = new EntityAttr((int)PlayerProp.SkillStatus, (int)(data.skill_status + 1));

        //氏族ID分为两个存
        //氏族ID分为两个存
        uint clanId = data.clan_id;
        int clanIdLow = (int)(clanId & 0x0000ffff);
        int clanIdHigh = (int)(clanId >> 16);
        propList[index++] = new EntityAttr((int)CreatureProp.ClanIdLow, clanIdLow);
        propList[index++] = new EntityAttr((int)CreatureProp.ClanIdHigh, clanIdHigh);

        //验证代码
        uint low = (uint)clanIdLow;
        uint high = (uint)clanIdHigh;
        uint newhigh = high << 16;
        uint newClanId = newhigh | low;

        Engine.Utility.Log.Error("--->>> 服务器发的：" + data.clan_id);
        Engine.Utility.Log.Error("--->>> 客户端存的：" + newClanId);
        Engine.Utility.Log.Error("---客户端存的低位：" + low);
        Engine.Utility.Log.Error("---客户端存的高位：" + newhigh);
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief  构建玩家属性列表 
    @param 
    */
    public static void BuildPlayerPropList(GameCmd.t_MainUserData data, ref EntityAttr[] propList)
    {
        if (propList.Length < 0 || propList.Length > (int)PlayerProp.End - (int)EntityProp.Begin)
        {
            Engine.Utility.Log.Error("BuildPlayerPropList:属性列表长度非法");
            return;
        }
        int index = 0;
        propList[index++] = new EntityAttr((int)CreatureProp.Mp, (int)data.sp); // 法力值
        propList[index++] = new EntityAttr((int)CreatureProp.MaxMp, (int)data.maxsp); // 法力值
        propList[index++] = new EntityAttr((int)PlayerProp.Exp, (int)data.exp);      // 经验
        propList[index++] = new EntityAttr((int)PlayerProp.PkMode, (int)data.pkmode);//Pk模式
        propList[index++] = new EntityAttr((int)FightCreatureProp.Power, (int)data.power); // 战斗力
        propList[index++] = new EntityAttr((int)FightCreatureProp.Strength, (int)data.liliang);  // 力量
        propList[index++] = new EntityAttr((int)FightCreatureProp.Corporeity, (int)data.tizhi);  // 体质
        propList[index++] = new EntityAttr((int)FightCreatureProp.Intelligence, (int)data.zhili);// 智力
        propList[index++] = new EntityAttr((int)FightCreatureProp.Spirit, (int)data.jingshen);   // 精神
        propList[index++] = new EntityAttr((int)FightCreatureProp.Agility, (int)data.minjie);    // 敏捷
        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsAttack, (int)data.pdam_min);// 最小物理攻击力
        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsDefend, (int)data.pdef_min);// 最小物理防御力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicAttack, (int)data.mdam_min);// 最小法术攻击力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicDefend, (int)data.mdef_min);// 最小法术防御力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MaxPhysicsAttack, (int)data.pdam_max); // 最大物理攻击力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MaxPhysicsDefend, (int)data.pdef_max); // 最大物理防御力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MaxMagicAttack, (int)data.mdam_max);   // 最大法术攻击力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MaxMagicDefend, (int)data.mdef_max);   // 最大法术防御力
        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsCrit, (int)data.lucky);  // 物理致命一击
        //propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsCritDefend, (int)data.luckydef); // 抗暴击
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicCrit, (int)data.mlucky);  // 法术致命一击
        //propList[index++] = new EntityAttr((int)FightCreatureProp.MagicCritDefend, (int)data.mluckydef);
        propList[index++] = new EntityAttr((int)FightCreatureProp.Hit, (int)data.phit);        // 命中
        propList[index++] = new EntityAttr((int)FightCreatureProp.Dodge, (int)data.hide_per);  // 闪避

        propList[index++] = new EntityAttr((int)FightCreatureProp.IceAttack, (int)data.ssdam); // 冰攻
        propList[index++] = new EntityAttr((int)FightCreatureProp.FireAttack, (int)data.esdam);// 火攻
        propList[index++] = new EntityAttr((int)FightCreatureProp.EleAttack, (int)data.lsdam);// 电攻
        propList[index++] = new EntityAttr((int)FightCreatureProp.WitchAttack, (int)data.vsdam);// 暗攻

        propList[index++] = new EntityAttr((int)FightCreatureProp.IceDefend, (int)data.ssdef);// 冰防
        propList[index++] = new EntityAttr((int)FightCreatureProp.FireDefend, (int)data.esdef);// 火防
        propList[index++] = new EntityAttr((int)FightCreatureProp.EleDefend, (int)data.lsdef);// 电防
        propList[index++] = new EntityAttr((int)FightCreatureProp.WitchDefend, (int)data.vsdef);// 暗防


        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsAbsorb, (int)data.pabs);// 物伤吸收
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicAbsorb, (int)data.mabs);// 法伤吸收
        propList[index++] = new EntityAttr((int)FightCreatureProp.HarmDeepen, (int)data.harm_add_per);// 伤害加深
        propList[index++] = new EntityAttr((int)FightCreatureProp.HarmAbsorb, (int)data.harm_sub_per);// 伤害吸收
        propList[index++] = new EntityAttr((int)FightCreatureProp.CritiRatio, (int)data.criti_ratio);// 暴击伤害百分比



        propList[index++] = new EntityAttr((int)FightCreatureProp.Cure, (int)data.cure);    // 治疗
        propList[index++] = new EntityAttr((int)PlayerProp.LoginOutTime, (int)data.lastofftime); // 最近一次下线时间
        propList[index++] = new EntityAttr((int)PlayerProp.ExpAddBuff, (int)data.exp_add_per); // 最近一次下线时间


        // 战斗属性
    }
    //-------------------------------------------------------------------------------------------------------
    /**
    @brief  构建玩家属性列表 
    @param 
    */
    private static void BuildNPCPropList(GameCmd.t_MapNpcDataPos data, ref EntityAttr[] propList, uint master_id = 0)
    {
        if (propList.Length < 0 || propList.Length > (int)NPCProp.End - (int)EntityProp.Begin)
        {
            Engine.Utility.Log.Error("BuildNPCPropList:属性列表长度非法");
            return;
        }

        int index = 0;
        propList[index++] = new EntityAttr((int)EntityProp.BaseID, (int)data.mapnpcdata.npcdata.dwBaseID); // 怪物模板ID 0
        propList[index++] = new EntityAttr((int)CreatureProp.Hp, (int)data.mapnpcdata.npcdata.curhp);
        propList[index++] = new EntityAttr((int)CreatureProp.MaxHp, (int)data.mapnpcdata.npcdata.maxhp);
        propList[index++] = new EntityAttr((int)CreatureProp.Level, (int)data.mapnpcdata.npcdata.level);
        propList[index++] = new EntityAttr((int)CreatureProp.Camp, (int)data.mapnpcdata.npcdata.camp);

        propList[index++] = new EntityAttr((int)WorldObjProp.MoveSpeed, (int)data.mapnpcdata.npcdata.movespeed);
        propList[index++] = new EntityAttr((int)NPCProp.Job, (int)data.mapnpcdata.npcdata.job);
        propList[index++] = new EntityAttr((int)NPCProp.Sex, (int)data.mapnpcdata.npcdata.sex);
        propList[index++] = new EntityAttr((int)NPCProp.ArenaNpcType, (int)data.mapnpcdata.npcdata.arenanpctype);
        propList[index++] = new EntityAttr((int)NPCProp.Masterid, (int)master_id);
        propList[index++] = new EntityAttr((int)NPCProp.SuitID, (int)data.mapnpcdata.npcdata.suitid);
        propList[index++] = new EntityAttr((int)NPCProp.SkillStatus, (int)data.mapnpcdata.npcdata.skill_status);    // 技能形态
        propList[index++] = new EntityAttr((int)NPCProp.MasterType, (int)data.mapnpcdata.npcdata.master_type);    // 技能形态

    }

    private static void BuildRobotPropList(table.RobotDataBase data, table.NpcDataBase npcdb, uint moveSpeed, ref EntityAttr[] propList)
    {
        if (propList.Length < 0 || propList.Length > (int)RobotProp.End - (int)EntityProp.Begin)
        {
            Engine.Utility.Log.Error("BuildRobotPropList:属性列表长度非法");
            return;
        }
        int index = 0;
        propList[index++] = new EntityAttr((int)RobotProp.Sex, (int)npcdb.sex);
        propList[index++] = new EntityAttr((int)RobotProp.Job, (int)data.dwJob);
        propList[index++] = new EntityAttr((int)CreatureProp.Level, (int)data.dwLevel);
        propList[index++] = new EntityAttr((int)EntityProp.BaseID, (int)npcdb.dwID); //模型ID 0
        propList[index++] = new EntityAttr((int)CreatureProp.Mp, (int)data.mp); // 法力值
        propList[index++] = new EntityAttr((int)CreatureProp.MaxMp, (int)data.mp); // 法力值
        propList[index++] = new EntityAttr((int)CreatureProp.Hp, (int)data.hp);
        propList[index++] = new EntityAttr((int)CreatureProp.MaxHp, (int)data.hp);
        propList[index++] = new EntityAttr((int)FightCreatureProp.Power, (int)data.power); // 战斗力
        propList[index++] = new EntityAttr((int)FightCreatureProp.Strength, (int)data.liliang);  // 力量
        propList[index++] = new EntityAttr((int)FightCreatureProp.Corporeity, (int)data.tizhi);  // 体质
        propList[index++] = new EntityAttr((int)FightCreatureProp.Intelligence, (int)data.zhili);// 智力
        propList[index++] = new EntityAttr((int)FightCreatureProp.Spirit, (int)data.jingshen);   // 精神
        propList[index++] = new EntityAttr((int)FightCreatureProp.Agility, (int)data.minjie);    // 敏捷
        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsAttack, (int)(data.pdam * 0.8));// 最小物理攻击力
        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsDefend, (int)(data.pdef * 0.8));// 最小物理防御力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicAttack, (int)(data.mdam * 0.8));// 最小法术攻击力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicDefend, (int)(data.mdef * 0.8));// 最小法术防御力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MaxPhysicsAttack, (int)(data.pdam * 1.15)); // 最大物理攻击力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MaxPhysicsDefend, (int)(data.pdef * 1.15)); // 最大物理防御力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MaxMagicAttack, (int)(data.mdam * 1.15));   // 最大法术攻击力
        propList[index++] = new EntityAttr((int)FightCreatureProp.MaxMagicDefend, (int)(data.mdef * 1.15));   // 最大法术防御力
        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsCrit, (int)data.lucky);  // 物理致命一击
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicCrit, (int)data.mlucky);  // 法术致命一击
        propList[index++] = new EntityAttr((int)FightCreatureProp.Hit, (int)data.phit);        // 命中
        propList[index++] = new EntityAttr((int)FightCreatureProp.Dodge, (int)data.hide_per);  // 闪避

        propList[index++] = new EntityAttr((int)FightCreatureProp.IceAttack, (int)data.ssdam); // 冰攻
        propList[index++] = new EntityAttr((int)FightCreatureProp.FireAttack, (int)data.esdam);// 火攻
        propList[index++] = new EntityAttr((int)FightCreatureProp.EleAttack, (int)data.lsdam);// 电攻
        propList[index++] = new EntityAttr((int)FightCreatureProp.WitchAttack, (int)data.vsdam);// 暗攻

        propList[index++] = new EntityAttr((int)FightCreatureProp.IceDefend, (int)data.ssdef);// 冰防
        propList[index++] = new EntityAttr((int)FightCreatureProp.FireDefend, (int)data.esdef);// 火防
        propList[index++] = new EntityAttr((int)FightCreatureProp.EleDefend, (int)data.lsdef);// 电防
        propList[index++] = new EntityAttr((int)FightCreatureProp.WitchDefend, (int)data.vsdef);// 暗防


        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsAbsorb, (int)data.pabs);// 物伤吸收
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicAbsorb, (int)data.mabs);// 法伤吸收
        propList[index++] = new EntityAttr((int)FightCreatureProp.HarmDeepen, (int)data.harm_add_per);// 伤害加深
        propList[index++] = new EntityAttr((int)FightCreatureProp.HarmAbsorb, (int)data.harm_sub_per);// 伤害吸收
        propList[index++] = new EntityAttr((int)FightCreatureProp.CritiRatio, (int)data.criti_ratio);// 暴击伤害百分比



        propList[index++] = new EntityAttr((int)FightCreatureProp.Cure, (int)data.cure);    // 治疗
        propList[index++] = new EntityAttr((int)WorldObjProp.MoveSpeed, (int)moveSpeed);
        //  propList[index++] = new EntityAttr((int)PlayerProp.LoginOutTime, (int)data.lastofftime); // 最近一次下线时间
    }

    private static void BuildRobotViewProp(table.RobotDataBase robotdata, ref EntityViewProp[] propList)
    {
        if (propList == null)
        {
            return;
        }
        if (propList.Length < 0 || propList.Length > (int)Client.EquipPos.EquipPos_Max)
        {
            Engine.Utility.Log.Error("BuildRobotViewProp:外观列表长度非法");
            return;
        }
        List<uint> equipList = new List<uint>();
        equipList.Add(robotdata.Hat);
        equipList.Add(robotdata.Shoulder);
        equipList.Add(robotdata.Coat);
        equipList.Add(robotdata.Leg);
        equipList.Add(robotdata.Adornl_1);
        equipList.Add(robotdata.Adornl_2);
        equipList.Add(robotdata.Shield);
        equipList.Add(robotdata.Equip);
        equipList.Add(robotdata.Shoes);
        equipList.Add(robotdata.Cuff);
        equipList.Add(robotdata.Belf);
        equipList.Add(robotdata.Necklace);

        int index = 0;
        propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Body, 0);
        propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Weapon, 0);

        for (int i = 0; i < equipList.Count; i++)
        {
            if (equipList[i] != 0)
            {
                table.EquipDataBase equipdb = GameTableManager.Instance.GetTableItem<table.EquipDataBase>(equipList[i]);
                if (equipdb != null)
                {
                    if (equipdb.act_show != 0)
                    {
                        table.SuitDataBase suitDb = GameTableManager.Instance.GetTableItem<table.SuitDataBase>(equipdb.act_show, 1);
                        if (suitDb != null)
                        {

                            int pos = GetPropPos((Client.EquipPos)suitDb.base_id, propList);
                            if (pos >= 0)
                            {
                                propList[pos] = new EntityViewProp((int)suitDb.type, (int)suitDb.base_id);
                            }
                            else
                            {
                                propList[index++] = new EntityViewProp((int)suitDb.type, (int)suitDb.base_id);
                            }
                        }
                    }
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------
    private static void BuildPlayerViewProp(GameCmd.t_MapUserData data, ref EntityViewProp[] propList)
    {
        if (propList == null)
        {
            return;
        }
        if (propList.Length < 0 || propList.Length > (int)Client.EquipPos.EquipPos_Max)
        {
            Engine.Utility.Log.Error("BuildPlayerPropList:外观列表长度非法");
            return;
        }

        // 处理时装外观数据
        int index = 0;
        propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Body, 0);
        propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Weapon, 0);

        if (data.userdata.equip_suit == null)
        {
            return;
        }

        if (data.userdata.equip_suit.Count > 0)
        {
            for (int i = 0; i < data.userdata.equip_suit.Count; ++i)
            {
                if (data.userdata.equip_suit[i] == null)
                {
                    continue;
                }

                // 忽略萌宠
                if (GameCmd.EquipSuitType.Magic_Pet_Type == data.userdata.equip_suit[i].suit_type)
                {
                    continue;
                }

                int pos = GetPropPos((Client.EquipPos)data.userdata.equip_suit[i].suit_type, propList);
                if (pos >= 0)
                {
                    propList[pos] = new EntityViewProp((int)data.userdata.equip_suit[i].suit_type, (int)data.userdata.equip_suit[i].baseid);
                }
                else
                {
                    Client.EquipPos equipPos = (Client.EquipPos)data.userdata.equip_suit[i].suit_type;
                    propList[index++] = new EntityViewProp((int)equipPos, (int)data.userdata.equip_suit[i].baseid);
                }
            }
        }
    }

    private static bool IsExistProp(Client.EquipPos pos, EntityViewProp[] propList)
    {
        if (propList == null)
        {
            return false;
        }

        for (int i = 0; i < propList.Length; ++i)
        {
            if (propList[i] == null)
            {
                continue;
            }

            if (propList[i].nPos == (int)pos)
            {
                return true;
            }
        }

        return false;
    }
    private static int GetPropPos(Client.EquipPos pos, EntityViewProp[] propList)
    {
        if (propList == null)
        {
            return -1;
        }

        for (int i = 0; i < propList.Length; ++i)
        {
            if (propList[i] == null)
            {
                continue;
            }

            if (propList[i].nPos == (int)pos)
            {
                return i;
            }
        }

        return -1;
    }

    private static void BuildNPCViewProp(GameCmd.t_MapNpcDataPos data, ref EntityViewProp[] propList)
    {
        if (propList == null)
        {
            return;
        }

        if (propList.Length < 0 || propList.Length > (int)Client.EquipPos.EquipPos_Max)
        {
            Engine.Utility.Log.Error("BuildPlayerPropList:外观列表长度非法");
            return;
        }

        // 处理时装外观数据
        if (data.mapnpcdata.npcdata.arenanpctype == ArenaNpcType.ArenaNpcType_Player || data.mapnpcdata.npcdata.arenanpctype == ArenaNpcType.ArenaNpcType_Npc)
        {
            int index = 0;
            if (data.mapnpcdata.npcdata.sex == 0)
            {
                Engine.Utility.Log.Error("BuildPlayerPropList:性别数据{0}非法!", data.mapnpcdata.npcdata.sex);
                data.mapnpcdata.npcdata.sex = 1;
            }
            var table_data = table.SelectRoleDataBase.Where((GameCmd.enumProfession)data.mapnpcdata.npcdata.job, (GameCmd.enmCharSex)data.mapnpcdata.npcdata.sex);
            if (table_data == null)
            {
                Engine.Utility.Log.Error("BuildPlayerPropList:job{0}或者sex{1}数据非法!", data.mapnpcdata.npcdata.job, data.mapnpcdata.npcdata.sex);
                return;
            }
            uint uClothID = table_data.bodyPathID;
            uint uWeaponID = table_data.weaponPath;

            propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Body, (int)uClothID);
            propList[index++] = new EntityViewProp((int)Client.EquipPos.EquipPos_Weapon, (int)uWeaponID);
            for (int i = 0; i < data.mapnpcdata.npcdata.suit_data.Count; ++i)
            {
                if (data.mapnpcdata.npcdata.suit_data[i] == null)
                {
                    return;
                }

                uint nSultID = 0;
                int pos = GetPropPos((Client.EquipPos)data.mapnpcdata.npcdata.suit_data[i].suit_type, propList);
                if (pos >= 0)
                {
                    if (data.mapnpcdata.npcdata.suit_data[i].suit_type == EquipSuitType.Clothes_Type && data.mapnpcdata.npcdata.suit_data[i].suit_type == 0)
                    {
                        nSultID = uClothID;
                    }
                    else
                    {
                        nSultID = data.mapnpcdata.npcdata.suit_data[i].baseid;
                    }

                    if (data.mapnpcdata.npcdata.suit_data[i].suit_type == EquipSuitType.Qibing_Type && data.mapnpcdata.npcdata.suit_data[i].suit_type == 0)
                    {
                        nSultID = uWeaponID;
                    }
                    else
                    {
                        nSultID = data.mapnpcdata.npcdata.suit_data[i].baseid;
                    }

                    propList[pos] = new EntityViewProp((int)data.mapnpcdata.npcdata.suit_data[i].suit_type, (int)nSultID);
                }
                else
                {
                    Client.EquipPos equipPos = (Client.EquipPos)data.mapnpcdata.npcdata.suit_data[i].suit_type;
                    propList[index++] = new EntityViewProp((int)equipPos, (int)nSultID);
                }
            }
        }
        else
        {
            propList = null;
        }
    }
    /// <summary>
    /// 构建宠物属性列表
    /// </summary>
    /// <param name="data"></param>
    /// <param name="propList"></param>
    public static void BuildPetPropListByPetData(GameCmd.PetData data, ref EntityAttr[] propList)
    {
        if (propList.Length < 0 || propList.Length > (int)PetProp.End - (int)EntityProp.Begin)
        {
            Engine.Utility.Log.Error("BuildPlayerPropList:属性列表长度非法");
            return;
        }

        int index = 0;
        propList[index++] = new EntityAttr((int)CreatureProp.Hp, (int)data.hp);
        propList[index++] = new EntityAttr((int)PetProp.LevelExp, (int)data.exp);      // 经验
        propList[index++] = new EntityAttr((int)CreatureProp.Level, (int)data.lv);
        propList[index++] = new EntityAttr((int)PetProp.Sex, (data.male == true) ? 0 : 1);
        propList[index++] = new EntityAttr((int)PetProp.Character, (int)data.character);
        propList[index++] = new EntityAttr((int)PetProp.PetGuiYuanStatus, (int)data.grade);
        propList[index++] = new EntityAttr((int)PetProp.YinHunExp, (int)data.yh_exp);
        propList[index++] = new EntityAttr((int)PetProp.YinHunLevel, (int)data.yh_lv);
        propList[index++] = new EntityAttr((int)PetProp.Life, (int)data.life);
        propList[index++] = new EntityAttr((int)PetProp.MaxPoint, (int)data.max_point);
        propList[index++] = new EntityAttr((int)PetProp.BaseID, (int)data.base_id);
        //  propList[index++] = new EntityAttr( (int)FightCreatureProp.Power , (int)data.power ); // 战斗力
        //当前天赋
        propList[index++] = new EntityAttr((int)PetProp.StrengthTalent, (int)data.cur_talent.liliang);
        propList[index++] = new EntityAttr((int)PetProp.CorporeityTalent, (int)data.cur_talent.tizhi);
        propList[index++] = new EntityAttr((int)PetProp.IntelligenceTalent, (int)data.cur_talent.zhili);
        propList[index++] = new EntityAttr((int)PetProp.SpiritTalent, (int)data.cur_talent.jingshen);
        propList[index++] = new EntityAttr((int)PetProp.AgilityTalent, (int)data.cur_talent.minjie);
        propList[index++] = new EntityAttr((int)PetProp.CommonJieBianLv, (int)data.by_lv);
        propList[index++] = new EntityAttr((int)PetProp.AdvaceJieBianLv, (int)data.replace_by_lv);
        //属性
        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsAttack, (int)data.attr.pdam);
        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsDefend, (int)data.attr.pdef);
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicAttack, (int)data.attr.mdam);
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicDefend, (int)data.attr.mdef);
        propList[index++] = new EntityAttr((int)FightCreatureProp.PhysicsCrit, (int)data.attr.plucky);  // 暴击
        propList[index++] = new EntityAttr((int)FightCreatureProp.MagicCrit, (int)data.attr.mlucky);
        propList[index++] = new EntityAttr((int)FightCreatureProp.Hit, (int)data.attr.hit);
        propList[index++] = new EntityAttr((int)FightCreatureProp.Dodge, (int)data.attr.hide);
        propList[index++] = new EntityAttr((int)FightCreatureProp.FireDefend, (int)data.attr.heatdef + data.attr.mdef);
        propList[index++] = new EntityAttr((int)FightCreatureProp.IceDefend, (int)data.attr.biochdef + data.attr.mdef);
        propList[index++] = new EntityAttr((int)FightCreatureProp.EleDefend, (int)data.attr.lightdef + data.attr.mdef);
        propList[index++] = new EntityAttr((int)FightCreatureProp.WitchDefend, (int)data.attr.wavedef + data.attr.mdef);
        propList[index++] = new EntityAttr((int)FightCreatureProp.Power, (int)data.attr.fight_power);
        propList[index++] = new EntityAttr((int)CreatureProp.MaxHp, (int)data.attr.maxhp);

        // 战斗属性
    }
}