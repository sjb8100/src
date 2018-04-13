using Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace EntitySystem
{
    class Monster : Creature, IMonster
    {
        // 属性数组
        private Engine.Utility.SecurityInt[] m_MonsterProp = new Engine.Utility.SecurityInt[MonsterProp.End - MonsterProp.Begin];
 
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)MonsterProp.Begin || nPropID >= (int)MonsterProp.End)
            {
                return;
            }

            m_MonsterProp[nPropID - (int)MonsterProp.Begin].Number = nValue;
        }

        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Monster;
        }
        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)MonsterProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)MonsterProp.End)
            {
                return m_MonsterProp[nPropID - (int)MonsterProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < MonsterProp.End - MonsterProp.Begin; ++i)
            {
                m_MonsterProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        public override bool CreateEntityView(EntityCreateData data)
        {
            base.CreateEntityView(data);
            if (GetEntityType() != EntityType.EntityType_Monster)
            {
                Engine.Utility.Log.Error("CreateEntityView failed:{0}", GetName());
                return false;
            }

            int nID = GetProp((int)EntityProp.BaseID);
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

                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(table_data.dwModelSet);
                if (resDB == null)
                {
                    Engine.Utility.Log.Error("NPC:找不到NPC模型资源路径配置{0}", table_data.dwModelSet);
                    return false;
                }
                strResName = resDB.strPath;
                SetName(table_data.strName);
            }

            string strPath, strFileName, strExt, strFileNameNoExt;
            Engine.Utility.StringUtility.ParseFileName(ref strResName, out strPath, out strFileName, out strFileNameNoExt, out strExt);
            m_strObjName = strFileNameNoExt;

            // TODO: 根据配置表读取数据               
            //if (!m_EntityView.Create(ref strResName, OnCreateRenderObj))
            //{
            //    Engine.Utility.Log.Error("CreateEntityView failed:{0}", strResName);
            //}
            EntityViewCreator.Instance.AddView(GetUID(), m_EntityView, ref strResName, OnCreateRenderObj);
            
            return false;
        }

        //-------------------------------------------------------------------------------------------------------
        private void OnCreateRenderObj(Engine.IRenderObj obj, object param)
        {
            if (m_EntityView == null)
            {
                return;
            }

            // 根据全局配置设置阴影
            int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1);
            m_EntityView.SetShadowLevel(nShadowLevel);

            Vector3 currPos = GetPos();
            SetPos(ref currPos);

            Client.stCreateEntity createEntity = new Client.stCreateEntity();
            createEntity.uid = GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, createEntity);
        }
    }
}
