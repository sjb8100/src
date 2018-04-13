using System;
using System.Collections.Generic;
using UnityEngine;
using Client;

namespace EntitySystem
{
    class Box : WorldObj, IBox
    {

        // 属性数组
        private Engine.Utility.SecurityInt[] m_ItemProp = new Engine.Utility.SecurityInt[BoxProp.End - BoxProp.Begin];

        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Box;
        }

        public override bool Create(EntityCreateData data, ColliderCheckType colliderCheckType)
        {
            // 设置属性
            //InitProp();
            //ApplyProp(data);

            //// 创建外观对象
            //CreateEntityView(data);

            return base.Create(data, colliderCheckType);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)BoxProp.Begin || nPropID >= (int)BoxProp.End)
            {
                return;
            }

            m_ItemProp[nPropID - (int)BoxProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)BoxProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)BoxProp.End)
            {
                return m_ItemProp[nPropID - (int)BoxProp.Begin].Number;
            }

            return 0;
        }

        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < BoxProp.End - BoxProp.Begin; ++i)
            {
                m_ItemProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        public override bool CreateEntityView(EntityCreateData data)
        {
            base.CreateEntityView(data);
            if (GetEntityType() != EntityType.EntityType_Box)
            {
                Engine.Utility.Log.Error("Box:CreateEntityView failed:{0}", GetName());
                return false;
            }


            int nID = GetProp((int)EntityProp.BaseID);
            string strResName = "";
            var table_data = GameTableManager.Instance.GetTableItem<table.ItemDataBase>((uint)nID);
            if (table_data == null)
            {
                Engine.Utility.Log.Error("不存在这个NPC: " + nID);
                return false;
            }
            if (nID == 0)
            {
                SetName(table_data.itemName + "X" + GetProp((int)BoxProp.Number));
            }
            else
            {
                SetName(table_data.itemName);
            }

            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(table_data.dropIcon);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("Box:找不到Box模型资源路径配置{0}", table_data.dropIcon);
                return false;
            }
            strResName = resDB.strPath;

            string strPath, strFileName, strExt, strFileNameNoExt;
            Engine.Utility.StringUtility.ParseFileName(ref strResName, out strPath, out strFileName, out strFileNameNoExt, out strExt);
            m_strObjName = strFileNameNoExt;

            if (data.bImmediate)
            {
                // 立即创建
                if (!m_EntityView.Create(ref strResName, OnCreateRenderObj))
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

        public bool CanAutoPick()
        {
            if (EntitySystem.m_ClientGlobal.gameOption == null)
            {
                return false;
            }
            int canPickUp = EntitySystem.m_ClientGlobal.gameOption.GetInt("Pick", "PickMedecal", 1);
            if (canPickUp != 1)
            {
                return false;
            }
            int pickLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Pick", "EquipLevel", 1);
            int nID = GetProp((int)EntityProp.BaseID);
            var table_data = GameTableManager.Instance.GetTableItem<table.ItemDataBase>((uint)nID);
            if (table_data == null)
            {
                return false;
            }

            if (table_data.grade < pickLevel)
            {
                return false;
            }

            Client.IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
            if (cs != null)
            {
                Client.IControllerHelper controllerhelper = cs.GetControllerHelper();
                if (controllerhelper != null)
                {
                    int itemId = GetProp((int)EntityProp.BaseID);
                    int itemNum = GetProp((int)BoxProp.Number);

                    //1、捡到的item  不为金币（金币不进背包，不做背包检测）
                    if (itemId != 60001)
                    {
                        //2、不可放入背包
                        if (false == cs.GetControllerHelper().CanPutInKanpsack((uint)itemId, (uint)itemNum))
                        {
                            return false;
                        }
                    }
                }
            }

            return CanPick();
        }

        public bool CanPick()
        {
            GameCmd.SceneItemOwnerType ownerType = (GameCmd.SceneItemOwnerType)GetProp((int)Client.BoxProp.OwnerType);
            //int owernId = (int)GetProp((int)Client.BoxProp.Owner);

            uint owernIdLow = (uint)GetProp((int)Client.BoxProp.OwnerLow);
            uint owernIdHigh = (uint)GetProp((int)Client.BoxProp.OwnerHigh);
            uint owernId = (owernIdHigh << 16) | owernIdLow;

            if (ownerType == GameCmd.SceneItemOwnerType.SceneItemOwnerType_Null)
            {
                return true;
            }
            else if (ownerType == GameCmd.SceneItemOwnerType.SceneItemOwnerType_User)
            {
                return owernId == (int)EntitySystem.m_ClientGlobal.MainPlayer.GetID();
            }
            else if (ownerType == GameCmd.SceneItemOwnerType.SceneItemOwnerType_Clan)
            {
                Client.IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
                if (cs != null)
                {
                    Client.IControllerHelper controllerhelper = cs.GetControllerHelper();
                    if (controllerhelper != null)
                    {
                        return owernId == controllerhelper.GetMainPlayerClanId();
                    }
                }
            }
            else if (ownerType == GameCmd.SceneItemOwnerType.SceneItemOwnerType_Team)
            {
                Client.IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
                if (cs != null)
                {
                    return cs.GetControllerHelper().IsTeamerCanPick((uint)owernId);
                }
            }
            return false;
        }

        private void OnCreateRenderObj(Engine.IRenderObj obj, object param)
        {
            if (m_EntityView == null)
            {
                return;
            }

            if (m_EntityView.GetNode() == null)
            {
                Engine.Utility.Log.Error("OnCreateRenderObj  m_EntityView.GetNode() is null");
                return;
            }
            int nID = GetProp((int)EntityProp.BaseID);
            var table_data = GameTableManager.Instance.GetTableItem<table.ItemDataBase>((uint)nID);
            if (table_data != null)
            {
                m_EntityView.GetNode().SetScale(UnityEngine.Vector3.one * table_data.scale * 0.01f);
            }

            if (nID == 60001)
            {
                UnityEngine.Vector3 pos = GetPos();
                pos.y += 0.05f;
                SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
            }
            // 根据全局配置设置阴影
            //int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1);
            //m_EntityView.SetShadowLevel(nShadowLevel);


            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }
            // 取下地表高度
            // 再计算移动速度
            Engine.IScene curScene = rs.GetActiveScene();
            if (curScene == null)
            {
                return;
            }
            Material mat = curScene.GetBoxMat();
            if (mat != null)
            {
                if (m_EntityView.GetMaterial() == null)
                {
                    return;
                }
                Texture tex = m_EntityView.GetMaterial().mainTexture;
                mat.mainTexture = tex;
                m_EntityView.ApplySharedMaterial(mat);
            }


            Vector3 currPos = GetPos();
            SetPos(ref currPos);

            Client.stCreateEntity createEntity = new Client.stCreateEntity();
            createEntity.uid = GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, createEntity);
            //             if (!m_sendPickMsg && CanAutoPick())
            //             {
            //                 m_sendPickMsg = true;
            //                 Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_PICKUPITEM, new Client.stPickUpItem() { itemid = GetID(), state = 0 });
            //             }
        }

        public void AddTrigger(IEntityCallback callback)
        {
            if (CanPick())
            {
                SetCallback(callback);
            }
            else
            {
                SetCallback(null);
            }
        }

        public new void Release()
        {
            base.Release();
            //             Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_PICKUPITEM,
            //                 new Client.stPickUpItem() { itemid = GetID(), state = 1 });
            //            Debug.Log("删除道具 " + GetID());
        }
    }
}
