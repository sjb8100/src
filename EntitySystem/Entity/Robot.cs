using System;
using System.Collections.Generic;
using UnityEngine;
using Client;

namespace EntitySystem
{
    class Robot : FightCreature,IRobot
    {
        private EntityCreateData m_Data = null;
        // 属性数组
        private Engine.Utility.SecurityInt[] m_RobotProp = new Engine.Utility.SecurityInt[RobotProp.End - RobotProp.Begin];

        public override EntityType GetEntityType()
        {
            return EntityType.EntityTYpe_Robot;
        }
        public override void InitProp()
        {
            for (int i = 0; i < RobotProp.End - RobotProp.Begin; ++i)
            {
                m_RobotProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)RobotProp.Begin || nPropID >= (int)RobotProp.End)
            {
                return;
            }

            m_RobotProp[nPropID - (int)RobotProp.Begin].Number = nValue;
        }

        public void GetSuit(out List<GameCmd.SuitData> lstSuit)
        {
            lstSuit = null;
            if (m_EntityView != null)
            {
                m_EntityView.GetSuit(out lstSuit);
            }
        }

        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)RobotProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)RobotProp.End)
            {
                return m_RobotProp[nPropID - (int)RobotProp.Begin].Number;
            }

            return 0;
        }

        public override bool CreateEntityView(EntityCreateData data)
        {
            m_Data = data;
            base.CreateEntityView(data);
            if (GetEntityType() != EntityType.EntityTYpe_Robot)
            {
                Engine.Utility.Log.Error("CreateEntityView failed:{0}", GetName());
                return false;
            }

            string strResName = "";
            int nID = GetProp((int)EntityProp.BaseID);
            if (nID != 0)
            {
                table.NpcDataBase npc_data = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)nID);
                if (npc_data != null)
                {
                    table.ResourceDataBase resData = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(npc_data.dwModelSet);
                    if (resData != null)
                    {
                        strResName = resData.strPath;
                    }
                }
            }

            table.SuitDataBase table_data = null;
            if (string.IsNullOrEmpty(strResName) && data.ViewList != null)
            {
                for (int i = 0; i < data.ViewList.Length; ++i)
                {
                    if (data.ViewList[i] == null)
                    {
                        continue;
                    }

                    if (data.ViewList[i].nPos == (int)EquipPos.EquipPos_Body)
                    {
                        table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)data.ViewList[i].value);
                        if (table_data == null)
                        {
                            GameCmd.enumProfession profession = (GameCmd.enumProfession)GetProp((int)PlayerProp.Job);
                            GameCmd.enmCharSex sex = (GameCmd.enmCharSex)GetProp((int)PlayerProp.Sex);
                            var role_data = table.SelectRoleDataBase.Where(profession, sex);
                            if (role_data == null)
                            {
                                Engine.Utility.Log.Error("CreateEntityView:job{0}或者sex{1}数据非法!", profession, sex);
                                return false;
                            }

                            table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)role_data.bodyPathID);
                        }
                        if (table_data != null)
                        {
                            table.ResourceDataBase db = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(table_data.resid);
                            if (db == null)
                            {
                                Debug.LogError("ResourceDataBase is null bodypahtid is " + table_data.resid + "  请检查resource 表格");
                                return false;
                            }
                            strResName = db.strPath;
                        }
    

                    }
                }
            }

            // 修改技能形态值
            m_Data.eSkillState = (SkillSettingState)GetProp((int)PlayerProp.SkillStatus);

            // 设置名称
            SetName(data.strName);
            string strPath, strFileName, strExt, strFileNameNoExt;
            Engine.Utility.StringUtility.ParseFileName(ref strResName, out strPath, out strFileName, out strFileNameNoExt, out strExt);
            m_strObjName = strFileNameNoExt;

            if (m_Data.bImmediate)
            {
                // 立即创建
                if (!m_EntityView.Create(ref strResName, OnCreateRenderObj, m_Data.bImmediate))
                {
                    Engine.Utility.Log.Error("CreateEntityView failed:{0}", strResName);
                    return false;
                }
            }
            else
            {
                EntityViewCreator.Instance.AddView(GetUID(), m_EntityView, ref strResName, OnCreateRenderObj);
            }

            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        public override bool CreatePart()
        {
            AddPart(EntityPart.Skill);
            AddPart(EntityPart.Buff);
            return true;
        }

        private void OnCreateRenderObj(Engine.IRenderObj obj, object param)
        {
            // 加载部件
            if (m_EntityView == null)
            {
                return;
            }
            
            // 根据全局配置设置阴影
            int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1);
            m_EntityView.SetShadowLevel(nShadowLevel);
            Vector3 currPos = GetPos();
            SetPos(ref currPos);
            Vector3 dir = GetRotate();
            SetRotate(dir);


            // 根据外部设置来设置
            SendMessage(EntityMessage.EntityCommand_EnableShowModel, EntityConfig.m_bShowEntity);

            SendMessage(EntityMessage.EntityCommond_SetColor, new Color(1, 1, 1));

            // 先做回调
            Client.stCreateEntity createEntity = new Client.stCreateEntity();
            createEntity.uid = GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, createEntity);

        }
    }
}
