using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EntitySystem
{
    class NPC : Creature, INPC
    {
        // 实体创建数据
        private EntityCreateData m_Data = null;

        // 属性数组
        private Engine.Utility.SecurityInt[] m_NPCProp = new Engine.Utility.SecurityInt[NPCProp.End - NPCProp.Begin];
        bool m_bisMonster = false;
        bool m_bBelongOther = false;
        bool m_bIsSummon = false;//是否是召唤物
        bool m_bIsPet = false;//是不是宠物
        bool m_bIsRobot = false; //是不是机器人
        bool m_bIsMercenary = false;//是不是雇佣兵
        bool m_whetherShowHeadTips = false;
        bool m_bIsTrap = false;//是不是陷阱
        uint teamid = 0; //怪物所归属的队伍ID
        uint ownerid = 0; //怪物归属的玩家ID
        bool isBoss = false; // 是不是boss

        //从属关系  通过masterid和类型判断 是不是从属主角
        bool m_isMySlave = false;
        //变身恢复回调
        private Action<object> m_ChangeEventHandler = null;
        private object m_ChangeParam;
        private bool m_bChange = false;

        private bool m_bCanSelect = false;
        public bool BelongOther
        {
            get
            {
                return m_bBelongOther;
            }
            set
            {
                m_bBelongOther = value;
            }
        }
        //belongme 只对宠物和召唤物生效
        bool m_bBelongMe = false;
        public bool BelongMe
        {
            set
            {
                if (value)
                {
                    if (IsPet() || IsSummon())
                    {
                        m_bBelongMe = value;
                    }
                    else
                    {
                        m_bBelongMe = false;
                    }
                }

            }
            get
            {
                return m_bBelongMe;
            }
        }
        public bool CanSelect()
        {
            return m_bCanSelect;
        }
        public override bool HitTest(Ray ray, out RaycastHit rayHit)
        {
            if (!CanSelect())
            {
                rayHit = new RaycastHit();
                return false;
            }

            return base.HitTest(ray, out rayHit);
        }

        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_NPC;
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)NPCProp.Begin || nPropID >= (int)NPCProp.End)
            {
                return;
            }

            m_NPCProp[nPropID - (int)NPCProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)NPCProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)NPCProp.End)
            {
                return m_NPCProp[nPropID - (int)NPCProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < NPCProp.End - NPCProp.Begin; ++i)
            {
                m_NPCProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        public override bool CreateEntityView(EntityCreateData data)
        {
            m_Data = data;

            base.CreateEntityView(data);
            if (GetEntityType() != EntityType.EntityType_NPC)
            {
                Engine.Utility.Log.Error("NPC:CreateEntityView failed:{0}", GetName());
                return false;
            }

            int nID = GetProp((int)EntityProp.BaseID);
            int suitID = GetProp((int)NPCProp.SuitID);
            int masterID = GetProp((int)NPCProp.Masterid);
            int type = GetProp((int)NPCProp.MasterType);
            IEntity en = EntityHelper.GetEntity((GameCmd.SceneEntryType)type, (uint)masterID);
            if (en != null)
            {
                if (EntityHelper.IsMainPlayer(en))
                {
                    m_isMySlave = true;
                }
            }
            string strResName = "";
            if (nID == 0)
            {
                // 则使用默认数据
                // strResName = data.strName;
            }
            else
            {
                var table_data = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)nID);
                if (table_data == null)
                {
                    Engine.Utility.Log.Error("不存在这个NPC: " + nID);
                    return false;
                }
                m_bCanSelect = table_data.dwCanBeSelect == 1;
                if (m_Data.ViewList == null)
                {
                    uint modelID = table_data.dwModelSet;
                    if (suitID != 0)
                    {
                        table.SuitDataBase sdb = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)suitID, 1);
                        if (sdb != null)
                        {
                            if (sdb.resid != 0)
                            {
                                modelID = sdb.resid;
                            }
                        }
                    }
                    table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(modelID);
                    if (resDB == null)
                    {
                        Engine.Utility.Log.Error("NPC:找不到NPC模型资源路径配置{0}", modelID);
                        return false;
                    }
                    strResName = resDB.strPath;
                }
                else
                {
                    for (int i = 0; i < data.ViewList.Length; ++i)
                    {
                        if (data.ViewList[i] == null)
                        {
                            continue;
                        }

                        if (data.ViewList[i].nPos == (int)EquipPos.EquipPos_Body)
                        {
                            //if(data.ViewList[i].)
                            var suit_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)data.ViewList[i].value);
                            if (suit_data != null)
                            {
                                table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(suit_data.resid);
                                if (db == null)
                                {
                                    Debug.LogError("ResourceDataBase is null bodypahtid is " + suit_data.resid + "  请检查resource 表格");
                                    return false;
                                }
                                //string bodyPath = db.strPath;
                                //if (data.ViewList[i] != null)
                                //{
                                //    strResName = data.ViewList[i].value == 0 ? bodyPath : EntityHelper.GetModelPath((uint)data.ViewList[i].value, (Cmd.enmCharSex)GetProp((int)PlayerProp.Sex));
                                //}
                                strResName = db.strPath;
                            }
                            else
                            {
                                strResName = "";
                            }

                        }
                    }
                }

                // 技能形态
                m_Data.eSkillState = (SkillSettingState)GetProp((int)NPCProp.SkillStatus);

                if ((int)GameCmd.ArenaNpcType.ArenaNpcType_Player == GetProp((int)NPCProp.ArenaNpcType))
                {
                    SetName(m_Data.strName);
                }
                else
                {
                    if (string.IsNullOrEmpty(data.strName))
                    {
                        SetName(table_data.strName);
                    }
                    else
                    {
                        SetName(data.strName);
                    }
                }
                if (IsSummon())
                {
                    IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
                    if (es != null)
                    {
                        IPlayer player = es.FindEntity<IPlayer>((uint)masterID);
                        if (player != null)
                        {
                            string playerName = player.GetName();
                            string summonName = GetName();
                            string tempName = playerName + "的" + summonName;
                            SetName(tempName);
                        }
                    }
                }
            }

            // TODO: 根据配置表读取数据               
            //if ( !m_EntityView.Create( ref strResName , OnCreateRenderObj ) )
            //{
            //    Engine.Utility.Log.Error( "CreateEntityView failed:{0}" , strResName );
            //}

            string strPath, strFileName, strExt, strFileNameNoExt;
            Engine.Utility.StringUtility.ParseFileName(ref strResName, out strPath, out strFileName, out strFileNameNoExt, out strExt);
            m_strObjName = strFileNameNoExt;

            // 功能NPC立即创建
            if (!IsMonster() && !IsPet() && !IsSummon())
            {
                m_Data.bImmediate = true;
            }

            if (m_Data.bImmediate)
            {
                // 立即创建
                if (!m_EntityView.Create(ref strResName, OnCreateRenderObj, false))
                {
                    Engine.Utility.Log.Error("CreateEntityView failed:{0}", strResName);
                }
            }
            else
            {
                EntityViewCreator.Instance.AddView(GetUID(), m_EntityView, ref strResName, OnCreateRenderObj);
            }
   
       
            return false;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 变身还原
        @param 
        */
        public override void Restore(object param)
        {
            if (param != null)
            {
                Client.ChangeBody changeParam = (Client.ChangeBody)param;
                m_ChangeEventHandler = changeParam.callback;
                m_ChangeParam = changeParam.param;
            }

            m_bChange = true;
            CreateEntityView(m_Data);

            //解决变身回来后会播放一个无关动作的问题
            PlayAni anim_param = new PlayAni();
            anim_param.strAcionName = EntityAction.Stand;
            anim_param.fSpeed = 1;
            anim_param.nStartFrame = 0;
            anim_param.nLoop = -1;
            anim_param.fBlendTime = 0.1f;
            SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_RESTORE,
             new Client.stPlayerChange() { status = 2, uid = GetID() });
        }

        private void OnCreateRenderObj(Engine.IRenderObj obj, object param)
        {
            if (m_EntityView == null)
            {
                m_bChange = false;
                return;
            }

            if (m_Data == null)
            {
                m_bChange = false;
                return;
            }

            int nID = GetProp((int)EntityProp.BaseID);
            var table_data = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)nID);
            if (table_data != null)
            {
                m_EntityView.GetNode().SetScale(UnityEngine.Vector3.one * table_data.scale * 0.01f);//100为基数
            }
            int suitID = GetProp((int)NPCProp.SuitID);
            if(suitID != 0)
            {
                var suitData = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)suitID);
                if(suitData != null)
                {
                    m_EntityView.GetNode().SetScale(UnityEngine.Vector3.one * suitData.sceneModleScale);//100为基数
                }
            }
            // 时装位置
            //string strResName = "";
            if (m_Data.ViewList != null)
            {
                for (int i = 0; i < m_Data.ViewList.Length; ++i)
                {
                    if (m_Data.ViewList[i] == null)
                    {
                        continue;
                    }

                    if (m_Data.ViewList[i].nPos != (int)EquipPos.EquipPos_Body)
                    {
                        // 读取时装配置表，挂接时装
                        ChangeEquip changeEquip = new ChangeEquip();
                        changeEquip.pos = (EquipPos)m_Data.ViewList[i].nPos;
                        changeEquip.nSuitID = m_Data.ViewList[i].value;
                        changeEquip.nLayer = m_Data.nLayer;
                        SendMessage(EntityMessage.EntityCommand_ChangeEquip, changeEquip);
                    }
                }
            }

            if (m_ChangeEventHandler != null)
            {
                m_ChangeEventHandler(m_ChangeParam);
                m_ChangeEventHandler = null;
                m_ChangeParam = null;
            }

            // 根据全局配置设置阴影
            int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1);
            m_EntityView.SetShadowLevel(nShadowLevel);

            Vector3 currPos = GetPos();
            SetPos(ref currPos);

            Vector3 dir = GetRotate();
            SetRotate(dir);

            // 根据外部设置来设置
            SendMessage(EntityMessage.EntityCommond_SetColor, new Color(1, 1, 1));
            SendMessage(EntityMessage.EntityCommand_EnableShowModel, EntityConfig.m_bShowEntity);

            PlayAction(m_EntityView.GetCurAniName());

            if (!m_bChange)
            {
                Client.stCreateEntity createEntity = new Client.stCreateEntity();
                createEntity.uid = GetUID();
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, createEntity);

                m_bChange = false;
            }
        }

        public override bool CreatePart()
        {
            // 读取配置表
            int modelid = GetProp((int)EntityProp.BaseID);
            var tbl = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)modelid);
            if (tbl == null)
            {
                Engine.Utility.Log.Error("AddNPC：读取NPC{0}配置出错,配置数据不存在!", modelid);
                return false;
            }
            if (tbl.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_SUMMON)
            {
                m_bIsSummon = true;
            }
            if (tbl.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_PET)
            {
                m_bIsPet = true;
            }
            else if (tbl.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_ROBOT)
            {
                m_bIsRobot = true;
            }
            else if (tbl.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_NONE)
            {
                if (tbl.npcCamp == 7)
                {
                    m_bIsMercenary = true;
                }
            }
            else if (tbl.dwType == (int)GameCmd.enumNpcType.NPC_TYPE_TRAP)
            {
                m_bIsTrap = true;
            }
            if (tbl.dwShowType == 1)
            {
                m_whetherShowHeadTips = true;
            }
            //怪物才添加
            if ((tbl.dwType == 0 && tbl.dwMonsterType != 0))
            {
                m_bisMonster = true;
            }
            //else if ( tbl.dwType == 4 || tbl.dwType == 8 )
            //{
            //    m_bisMonster = true;
            //}
            else
            {
                m_bisMonster = false;
            }


            if (tbl.dwMonsterType == 3)
            {
                isBoss = true;
            }
            else
            {
                isBoss = false;
            }
            AddPart(EntityPart.Skill);
            AddPart(EntityPart.Buff);
            return true;
        }
        public bool IsMainPlayerSlave()
        {
            return m_isMySlave;
        }
        public bool IsSummon()
        {
            return m_bIsSummon;
        }
        public bool IsMonster()
        {
            return m_bisMonster;
        }
        public bool IsPet()
        {
            return m_bIsPet;
        }
        public bool IsRobot()
        {
            return m_bIsRobot;
        }
        public bool IsMercenary()
        {
            return m_bIsMercenary;
        }
        public bool IsTrap()
        {
            return m_bIsTrap;
        }
        public bool IsBoss()
        {
            return isBoss;
        }
        public bool IsSlaveEntity(IEntity en)
        {
            if (en == null)
            {
                return false;
            }
            int masterID = GetProp((int)NPCProp.Masterid);
            int type = GetProp((int)NPCProp.MasterType);
            if (masterID == en.GetID() && type == (int)en.GetEntityType())
            {
                return true;
            }
            return false;
        }
        public bool IsCanAttackNPC()
        {

            int modelid = GetProp((int)EntityProp.BaseID);
            var tbl = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)modelid);
            if (tbl == null)
            {
                Engine.Utility.Log.Error("AddNPC：读取NPC{0}配置出错,配置数据不存在!", modelid);
                return false;
            }
            return tbl.dwAttackType == 1 ? true : false;
        }

        public bool WhetherShowHeadTips()
        {
            return m_whetherShowHeadTips;
        }

        public uint TeamID
        {
            get
            {
                return teamid;
            }
            set
            {
                teamid = value;
            }
        }
        public uint OwnerID
        {
            get
            {
                return ownerid;
            }
            set
            {
                ownerid = value;
            }
        }
    }
}
