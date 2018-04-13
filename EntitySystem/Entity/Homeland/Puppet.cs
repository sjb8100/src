using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;

namespace EntitySystem
{
    class Puppet : WorldObj,IPuppet
    {
        private EntityCreateData m_Data = null;

        private Engine.Utility.SecurityInt[] m_PuppetProp = new Engine.Utility.SecurityInt[PuppetProp.End - PuppetProp.Begin];

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)PuppetProp.Begin || nPropID >= (int)PuppetProp.End)
            {
                return;
            }

            m_PuppetProp[nPropID - (int)PuppetProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)PuppetProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)PuppetProp.End)
            {
                return m_PuppetProp[nPropID - (int)PuppetProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < PuppetProp.End - PuppetProp.Begin; ++i)
            {
                m_PuppetProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Puppet;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void CreateCommpent()
        {
            base.CreateCommpent(); // 只添加外观组件
            AddCommpent(EntityCommpent.EntityCommpent_Move); // 支持移动组件
        }
        //-------------------------------------------------------------------------------------------------------
        public override bool CreateEntityView(EntityCreateData data)
        {
            base.CreateEntityView(data);

            m_Data = data;

            if (GetEntityType() != EntityType.EntityType_Puppet)
            {
                Engine.Utility.Log.Error("CreateEntityView failed:{0}", GetName());
                return false;
            }

            //int nID = GetProp((int)EntityProp.BaseID);
            string strResName = "";
            //if (nID == 0)
            //{
            //    // 则使用默认数据
            //    strResName = data.strName;
            //}
            //else
            {
                if (data.ViewList != null)
                {
                    //Profiler.BeginSample("CreateEntityView:Table:Query");
                    for (int i = 0; i < data.ViewList.Length; ++i)
                    {
                        if (data.ViewList[i] == null)
                        {
                            continue;
                        }

                        if (data.ViewList[i].nPos == (int)EquipPos.EquipPos_Body)
                        {
                            var table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)data.ViewList[i].value);
                            if (table_data == null)
                            {
                                var role_data = table.SelectRoleDataBase.Where((GameCmd.enumProfession)GetProp((int)PuppetProp.Job), (GameCmd.enmCharSex)GetProp((int)PuppetProp.Sex));
                                if (role_data == null)
                                {
                                    Engine.Utility.Log.Error("CreateEntityView:job{0}或者sex{1}数据非法!", GetProp((int)PlayerProp.Job), GetProp((int)PlayerProp.Sex));
                                    return false;
                                }

                                table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)role_data.bodyPathID);
                                data.ViewList[i].value = (int)role_data.bodyPathID;
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
                            else
                            {
                                strResName = "";
                            }

                            
                            //if (m_EntityView != null)
                            //{
                            //    m_EntityView.AddSuitData(EquipPos.EquipPos_Body, data.ViewList[i].value, (int)table_data.resid);
                            //}
                        }
                    }
                    //Profiler.EndSample();
                }
            }

            // 设置名称
            SetName(data.strName);

            //Profiler.BeginSample("EntityView:Create");
            // TODO: 根据配置表读取数据               
            if (!m_EntityView.Create(ref strResName, OnCreateRenderObj, m_Data.bImmediate))
            {
                Engine.Utility.Log.Error("CreateEntityView failed:{0}", strResName);
            }
            //Profiler.EndSample();
            //Profiler.EndSample();

            return true;
        }

        private void OnCreateRenderObj(Engine.IRenderObj obj, object param)
        {
            // 加载部件
            if (m_EntityView == null)
            {
                return;
            }
            //if (m_Data == null)
            //{
            //    Engine.Utility.Log.Error("m_data is null");
            //    return;
            //}
            //if (m_Data.ViewList == null)
            //{
            //    return;
            //}

            ////string strResName = "";
            //for (int i = 0; i < m_Data.ViewList.Length; ++i)
            //{
            //    if (m_Data.ViewList[i] == null)
            //    {
            //        continue;
            //    }

            //    if (m_Data.ViewList[i].nPos == (int)EquipPos.EquipPos_Body)
            //    {
            //        var table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)m_Data.ViewList[i].value);
            //        m_EntityView.AddSuitData(EquipPos.EquipPos_Body, m_Data.ViewList[i].value, table_data == null ? 0 : (int)table_data.resid, obj);
            //    }

            //    if (m_Data.ViewList[i].nPos != (int)EquipPos.EquipPos_Body)
            //    {
            //        // 读取时装配置表，挂接装备
            //        ChangeEquip changeEquip = new ChangeEquip();
            //        changeEquip.pos = (EquipPos)m_Data.ViewList[i].nPos;

            //        // 武器
            //        table.SuitDataBase table_data = null;
            //        int nSuitID = m_Data.ViewList[i].value;
            //        if (changeEquip.pos == EquipPos.EquipPos_Weapon)
            //        {
            //            if (m_Data.ViewList[i].value == 0)
            //            {
            //                table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)m_Data.ViewList[i].value);
            //                if (table_data == null)
            //                {
            //                    var role_data = table.SelectRoleDataBase.Where((GameCmd.enumProfession)GetProp((int)PuppetProp.Job), (GameCmd.enmCharSex)GetProp((int)PuppetProp.Sex));
            //                    if (role_data == null)
            //                    {
            //                        Engine.Utility.Log.Error("OnCreateRenderObj:job{0}或者sex{1}数据非法!", GetProp((int)PuppetProp.Job), GetProp((int)PuppetProp.Sex));
            //                        return;
            //                    }

            //                    table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)role_data.weaponPath);
            //                }
            //            }
            //            else
            //            {
            //                table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)m_Data.ViewList[i].value);
            //            }
            //        }
            //        else
            //        {
            //            table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)m_Data.ViewList[i].value);
            //        }

            //        if (table_data != null)
            //        {
            //            nSuitID = (int)table_data.base_id;
            //        }
            //        else
            //        {
            //            Engine.Utility.Log.Error("OnCreateRenderObj:{0}位置 资源{1}找不到资源数据!", changeEquip.pos, m_Data.ViewList[i].value);
            //            return;
            //        }

            //        changeEquip.nSuitID = nSuitID;
            //        changeEquip.nLayer = m_Data.nLayer;
            //        SendMessage(EntityMessage.EntityCommand_ChangeEquip, changeEquip);
            //    }
            //}
            m_Data = null; // 释放资源

            // 根据全局配置设置阴影
            int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1);
            m_EntityView.SetShadowLevel(nShadowLevel);

            // 播放下动画
            PlayAction(m_EntityView.GetCurAniName());

            Client.stCreateEntity createEntity = new Client.stCreateEntity();
            createEntity.uid = GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, createEntity);
        }
    }
}