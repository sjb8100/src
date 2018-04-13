using System;
using System.Collections.Generic;
using Client;

public class KNpcTemplate
{
    public uint[] dwSkillIDList = new uint[KNpc.MAX_NPC_AI_SKILL];
    public int[] nSkillCastInterval = new int[KNpc.MAX_NPC_AI_SKILL];	//AI技能间隔
};

public class KNpc : KCharacter
{
    public static int MAX_NPC_AI_SKILL = 8;

    public int m_nSkillSelectIndex = 0;
    public int m_nSkillCommomCD = 0;
    public int[] m_nSkillCastFrame = new int[MAX_NPC_AI_SKILL];

    public KNpcTemplate m_pTemplate = new KNpcTemplate();

    public KNpc()
    {

    }

    public override int Init()
    {
        base.Init();

        for (int i = 0; i < MAX_NPC_AI_SKILL; i++)
        {
            m_nSkillCastFrame[i] = 0;
        }

        return 1;
    }

    public override void UnInit()
    {
        base.UnInit();
    }

    public override int Activate()
    {
        base.Activate();

        return 1;
    }

    public int SetDisplayData(uint uID)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();

        table.NpcDataBase npctable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(uID);
        if (npctable == null)
        {
            return 0;
        }

        GameCmd.t_MapNpcDataPos npcdata = new GameCmd.t_MapNpcDataPos();
        npcdata.mapnpcdata = new GameCmd.t_MapNpcData();
        npcdata.mapnpcdata.npcdata = new GameCmd.t_NpcData();

        npcdata.mapnpcdata.npcdata.dwBaseID = uID;

        npcdata.mapnpcdata.npcdata.dwTempID = this.m_dwID;
        npcdata.mapnpcdata.npcdata.movespeed = 600;

        EntityCreateData data = RoleUtil.BuildCreateEntityData(EntityType.EntityType_NPC, npcdata, 0);

        IEntity npc = es.CreateEntity(EntityType.EntityType_NPC, data, true) as INPC;

        //ISkillPart skillPart = npc.GetPart(EntityPart.Skill) as ISkillPart;
        //skillPart.ReqNpcUseSkill(208);

        m_pEntity = npc;

        return 1;
    }

    public int DestroyDisplayData()
    {

        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();

        if (m_pEntity != null)
        {
            es.RemoveEntity(m_pEntity);
            m_pEntity = null;
        }
        return 1;
    }


    public void SetPosition(UnityEngine.Vector3 pos)
    {
        if (m_pEntity != null)
        {
            m_pEntity.SendMessage(EntityMessage.EntityCommand_SetPos, pos);
        }

    }


}

