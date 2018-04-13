using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;
using Engine;
using table;

namespace EntitySystem
{
    class EntityView
    {
        // 血条资源
        private static string BAR_RES = "other/meshs/hpbar.rm";

        private Vector3 m_pos = new Vector3();          // 位置
        private Vector3 m_rot = new Vector3();          // 旋转
       // private Engine.Node m_RenderObjNode = null;     // 渲染对象节点
        private Engine.IRenderObj m_RenderObj = null;   // 渲染对象
        private string m_strAniName = "";               // 动画名称
        private bool m_bImmediate = false;              // 立即加载

        private Engine.IRenderObj m_RideObj = null; // 坐骑对象
        private Engine.IRenderObj m_RideCharObj = null;//上馬之後的人物對象
        private int m_nRideLinkID = 0;          // 坐骑连接对象ID
        private bool m_bRide = false;           // 坐骑状态
        private string m_strEntityName = "";    // 实体名称
        private Entity m_Owner = null;          // 实体宿主
        private Action<object> m_ChangeEventHandler = null;
        private object m_ChangeParam;
        Engine.CreateRenderObjEvent m_Createcallback = null;

        float m_suitModleScale = 1;
        // 可见性
        private bool m_bVisible = true;
        private bool m_bShowModel = true;

        // 服务器位置显示
        private int m_Coordinate = 0;
        private GameObject m_ServerPosCube = null;
        private bool m_bChange = false;//变身状态

        private Engine.RenderLayer m_eLayer = RenderLayer.RenderObj;    // 对象层

        private EntityCreateData m_EntityCreateData = null;

        // 装备挂接方式数据
        public class EquipData
        {
            public int m_nResID = 0;    // 资源ID
            public int m_nSuitID = 0;   // 时装ID
            public int m_nLinkObj = 0;  // 挂接对象
            public string strLocatorName = "";
            public Vector3 offset = Vector3.zero;
            public bool bReady = false; // 针对主体做标识
            public Engine.IRenderObj m_obj = null;
            public Engine.CreateRenderObjEvent evt = null;          // 回调事件
            public object param = null;                             // 回调参数
        }
        private Dictionary<EquipPos, EquipData> m_dicEquip = new Dictionary<EquipPos, EquipData>();

        // 连接对象
        private int m_nNameID = 0;  // 名称对象ID
        private List<int> m_RenderTextID = new List<int>();

        // 血条
        private int m_nHPBarID = 0;

        public EntityView(Entity owner, EntityCreateData data)
        {
            m_EntityCreateData = data;
            m_Owner = owner;
            m_bShowModel = EntityConfig.m_bShowEntity;
        }

        // 渲染对象
        public Engine.IRenderObj renderObj
        {
            get 
            {
                if(m_bRide)
                {
                    return m_RideCharObj;
                }
                else
                {
                    return m_RenderObj;
                }
            }
        }

        public bool IsExistSuit(EquipPos pos, int nSuitID, int nResID)
        {
            EquipData data = null;
            if (m_dicEquip.TryGetValue(pos, out data))
            {
                if (data.m_nSuitID == nSuitID && data.m_nResID==nResID)
                {
                    return true;
                }
            }

            return false;
        }

        public EquipData AddSuitData(EquipPos pos, int nSuitID, int nResID, IRenderObj obj, Engine.CreateRenderObjEvent evt = null, object param = null)
        {
            EquipData data = null;
            if (m_dicEquip.TryGetValue(pos, out data))
            {
                if (data.m_nSuitID == nSuitID)
                {
                    if (data.m_obj == null)
                    {
                        data.m_obj = obj;
                    }

                    return data;
                }

                if(pos==EquipPos.EquipPos_Body)
                {
                    data.bReady = false;
                }

                data.m_nSuitID = nSuitID;
                data.m_nResID = nResID;

                if (data.m_obj == null)
                {
                    data.m_obj = obj;
                }
            }
            else
            {
                // 直接添加新部位
                data = new EquipData();
                data.m_nSuitID = nSuitID;
                data.m_nResID = nResID;
                data.m_obj = obj;
                m_dicEquip.Add(pos, data);
            }

            if ((pos == EquipPos.EquipPos_Body||pos == EquipPos.EquipPos_Weapon) && data.evt==null)
            {
                data.evt = evt;
            }
            data.param = param;

            if (pos == EquipPos.EquipPos_Weapon && param == null)
            {
                data.param = obj;
            }

            return data;
        }

        // 获取时装数据
        public void GetSuit(out List<GameCmd.SuitData> lstSuit)
        {
            lstSuit = new List<GameCmd.SuitData>();
            Dictionary<EquipPos, EquipData>.Enumerator iter = m_dicEquip.GetEnumerator();
            while(iter.MoveNext())
            {
                if(iter.Current.Value!=null)
                {
                    GameCmd.SuitData data = new GameCmd.SuitData();
                    data.suit_type = (GameCmd.EquipSuitType)iter.Current.Key;
                    data.baseid = (uint)iter.Current.Value.m_nSuitID;
                    lstSuit.Add(data);
                }
            }
        }

        public void SetRenderObj(Engine.IRenderObj obj) { m_RenderObj = obj; }

        public void RefrushServerPos(Vector3 pos)
        {
            if (m_RenderObj == null || m_Owner == null)
            {
                return;
            }

            if(!EntityConfig.m_bShowMainPlayerServerPosCube)
            {
                return;
            }

            Vector3 pp = m_Owner.GetPos();
            if (m_ServerPosCube != null)
            {
                m_ServerPosCube.transform.localPosition = new Vector3(pos.x,pp.y+1,pos.z);
            }

            pp.y = 0;
            Vector3 dt = pos - pp;
            if (dt.magnitude > 5)
            {
                Engine.Utility.Log.Error("距离超大!!!!!!!!!!!!!!!!!!!");
            }
        }

        public void CheckConfig()
        {
            if(m_Owner==null)
            {
                return;
            }
            if(EntityConfig.m_bShowMainPlayerServerPosCube)
            {
                if (m_ServerPosCube == null && m_Owner.GetEntityType() == EntityType.EntityType_Player)
                {
                    m_ServerPosCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //m_ServerPosCube.transform.parent = m_RenderObjNode.GetTransForm(); O
                }
            }
            else
            {
                if (m_ServerPosCube != null && m_Owner.GetEntityType() == EntityType.EntityType_Player)
                {
                    m_ServerPosCube.transform.parent = null;
                    GameObject.DestroyImmediate(m_ServerPosCube,true);
                    m_ServerPosCube = null;
                }
            }

            // 画世界坐标
            if (m_Owner.GetEntityType() == EntityType.EntityType_Player) // EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner)
            {
                if (EntityConfig.m_bShowWorldCoordinate)
                {
                    if (m_Coordinate == 0)
                    {
                        // 画坐标系
                        string strObjName = "other/coordinate/coordinate.obj";
                        string strLocatorName = "Root";
                        m_Coordinate = m_Owner.AddLinkObj(ref strObjName, ref strLocatorName, new Vector3(0, 0.08f, 0), Quaternion.identity, Engine.LinkFollowType.LinkFollowType_Translate);
                    }
                }
                else
                {
                    if(m_Coordinate>0)
                    {
                        m_Owner.RemoveLinkObj(m_Coordinate);
                        m_Coordinate = 0;
                    }
                }
            }
        }
   
 
        /**
        @brief 创建实体外观(主体)
        @param 
        */
        public bool Create(ref string strEntityName, Engine.CreateRenderObjEvent callback, bool bImmediate = false)
        {

             // 变身状态，直接创建
            if(m_Owner!=null)
            {
                if(m_Owner.GetEntityType()==EntityType.EntityType_Player)
                {
                    if(EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner.GetUID()))
                    {
                        Engine.Utility.Log.LogGroup("ZDY", "mainpalyer create");
                    }
                    int nTransModelID = m_Owner.GetProp((int)PlayerProp.TransModelResId);
                    if (nTransModelID != 0)
                    {
                        ChangeBody changeBody = new ChangeBody();
                        changeBody.resId = nTransModelID;
                        changeBody.callback = OnChangeHandler;
                        changeBody.param = null;
                        m_Createcallback = callback;
                        Change(changeBody);
                        return true;
                    }
                }
            }
            
            m_strEntityName = strEntityName;
            m_bImmediate = bImmediate;
            m_eLayer = (Engine.RenderLayer)m_EntityCreateData.nLayer;

            if (m_EntityCreateData.ViewList != null)
            {
                for (int i = 0; i < m_EntityCreateData.ViewList.Length; ++i)
                {
                    if (m_EntityCreateData.ViewList[i] == null)
                    {
                        continue;
                    }

                    if ((EquipPos)m_EntityCreateData.ViewList[i].nPos == EquipPos.EquipPos_Body)
                    {
                        Client.ChangeEquip changeEquip = new Client.ChangeEquip();
                        changeEquip.pos = (EquipPos)m_EntityCreateData.ViewList[i].nPos;
                        changeEquip.nSuitID = m_EntityCreateData.ViewList[i].value;
                        changeEquip.nLayer = (int)m_eLayer;
                        changeEquip.evt = callback;
                        changeEquip.nStateType = (int)m_EntityCreateData.eSkillState;
                        ChangeEquip(changeEquip);
                    }
                }
            }
            else
            {
                Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                if (rs == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(strEntityName))
                {
                    return false;
                }
                m_Createcallback = callback;
                m_RenderObj = null;
                
                rs.CreateRenderObj(ref strEntityName, ref m_RenderObj, OnCreateRenderObjEvent, null, m_bImmediate ? TaskPriority.TaskPriority_Immediate : TaskPriority.TaskPriority_Normal);
                if (m_RenderObj == null)
                {
                    return false;
                }
             
                m_RenderObj.SetLayer((int)m_eLayer);
            }

            return true;
        }

        void OnCreateRenderObjEvent(IRenderObj obj, object param)
        {
            if (obj == null)
            {
                return;
            }
            m_RenderObj = obj;

            m_RenderObj.SetVisible(m_bVisible);
            m_RenderObj.ShowModel(m_bShowModel);

            if (m_Createcallback != null)
            {
                m_Createcallback(obj, param);
                m_Createcallback = null;
            }

            m_RenderObj.Play(m_strAniName);
        }

        public void Close()
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            if(m_bRide)
            {
                UnRide();
                m_bRide = false;
            }

            if (m_Coordinate > 0)
            {
                m_Owner.RemoveLinkObj(m_Coordinate);
                m_Coordinate = 0;
            }

            if (m_ServerPosCube != null)
            {
                GameObject.DestroyImmediate(m_ServerPosCube, true);
                m_ServerPosCube = null;
            }

            if (m_RenderObj != null)
            {
                for (int i = 0; i < m_RenderTextID.Count; ++i)
                {
                    m_RenderObj.RemoveRenderText(m_RenderTextID[i]);
                }

                rs.RemoveRenderObj(m_RenderObj);
                if(EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner.GetUID()))
                {
                    Engine.Utility.Log.LogGroup("ZDY", "m_RenderObjNode == null");
                }
                m_RenderObj = null;
            }

            Dictionary<EquipPos, EquipData>.Enumerator iter = m_dicEquip.GetEnumerator();
            while(iter.MoveNext())
            {
                if(iter.Current.Value==null)
                {
                    continue;
                }

                if(iter.Current.Value.m_obj!=null)
                {
                    rs.RemoveRenderObj(iter.Current.Value.m_obj);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 实体变身
        @param nResID 资源ID
        */
        public void Change(ChangeBody changeInfo)
        {
            if(m_bRide)
            {
                UnRide();
                m_bRide = false;
            }

            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            if(m_RenderObj!=null)
            {
                rs.RemoveRenderObj(m_RenderObj);
                if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner.GetUID()))
                {
                    Engine.Utility.Log.LogGroup("ZDY", "m_RenderObjNode == null");
                }
                m_RenderObj = null;
            }

            uint nResID = (uint)changeInfo.resId;
            m_ChangeEventHandler = changeInfo.callback;
            m_ChangeParam = changeInfo.param;
            m_suitModleScale = changeInfo.modleScale;
            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(nResID);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("NPC:找不到NPC模型资源路径配置{0}", nResID);
                return;
            }
            
            string strResName = resDB.strPath;

            // 创建新的外观
            if (string.IsNullOrEmpty(strResName))
            {
                return;
            }

            m_bChange = true;
            m_RenderObj = null;
            rs.CreateRenderObj(ref strResName, ref m_RenderObj, OnChangeRenderObj);
            if (m_RenderObj == null)
            {
                return;
            }

         
            m_RenderObj.SetLayer((int)m_eLayer);
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 
        @param 
        */
        public void Restore(Client.ChangeBody changeInfo)
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            if (m_RenderObj != null)
            {
                rs.RemoveRenderObj(m_RenderObj);
                m_RenderObj = null;
            }
            m_bChange = false;
            m_Owner.Restore(changeInfo);
        }

        public bool IsChange()
        {
            return m_bChange;
        }
        //-------------------------------------------------------------------------------------------------------
        // 第一次创建时，变身的回调
        private void OnChangeHandler(object param)
        {
            if(m_Createcallback!=null)
            {
                m_Createcallback(m_RenderObj,null);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 换装实现
        @param 
        */
        public void ChangeEquip(Client.ChangeEquip changeEquip)
        {
            if(changeEquip==null)
            {
                return;
            }

            if(m_EntityCreateData==null)
            {
                return;
            }

            //Engine.Utility.Log.Error("换时装 {0} {1}", changeEquip.pos, changeEquip.nSuitID);

            // 对默认时装进行SuitID修正
            int nJob = 0;
            int nSex = 0;
            var table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)changeEquip.nSuitID);
            if (table_data == null)
            {
                // 查找默认时装
                if (changeEquip.pos == EquipPos.EquipPos_Body || changeEquip.pos == EquipPos.EquipPos_Weapon)
                {
                    EntityType t = m_Owner.GetEntityType();
                    switch (t)
                    {
                        case EntityType.EntityType_NPC:
                            {
                                nJob = m_Owner.GetProp((int)NPCProp.Job);
                                nSex = m_Owner.GetProp((int)NPCProp.Sex);
                                break;
                            }
                        case EntityType.EntityType_Player:
                            {
                                nJob = m_Owner.GetProp((int)PlayerProp.Job);
                                nSex = m_Owner.GetProp((int)PlayerProp.Sex);
                                break;
                            }
                        case EntityType.EntityType_Puppet:
                            {
                                nJob = m_Owner.GetProp((int)PuppetProp.Job);
                                nSex = m_Owner.GetProp((int)PuppetProp.Sex);
                                break;
                            }
                        case EntityType.EntityTYpe_Robot:
                            {
                                nJob = m_Owner.GetProp((int)RobotProp.Job);
                                nSex = m_Owner.GetProp((int)RobotProp.Sex);
                                break;
                            }
                    }

                    var role_data = table.SelectRoleDataBase.Where((GameCmd.enumProfession)nJob, (GameCmd.enmCharSex)nSex);
                    if (role_data == null)
                    {
                        Engine.Utility.Log.Error("CreateEntityView:job{0}或者sex{1}数据非法!", nJob, nSex);
                        return;
                    }
                    if (changeEquip.pos == EquipPos.EquipPos_Body)
                    {
                        changeEquip.nSuitID = (int)role_data.bodyPathID;
                        table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)role_data.bodyPathID);
                    }
                    else if (changeEquip.pos == EquipPos.EquipPos_Weapon)
                    {
                        changeEquip.nSuitID = (int)role_data.weaponPath;
                        int status = m_Owner.GetProp((int)PlayerProp.SkillStatus);
                        table_data = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)role_data.weaponPath);
                    }
                }
            }

            if(table_data==null)
            {
                Engine.Utility.Log.Error("找不到位置({0})时装({1})的配置数据，请策划检查配置表！", changeEquip.pos, changeEquip.nSuitID);
                return;
            }

            ////////////////////////////////////////////////////////////////////////
            // 修改 观察模式 使用观察模型
            int nResID = 0;
            if(m_EntityCreateData.bViewModel)
            {
                nResID = (int)table_data.viewresid;
                if (changeEquip.nStateType == (int)Client.SkillSettingState.StateTwo) // 第二种形态
                {
                    nResID = (int)table_data.viewresid2;
                }
            }
            else
            {
                nResID = (int)table_data.resid;
                if (changeEquip.nStateType == (int)Client.SkillSettingState.StateTwo) // 第二种形态
                {
                    nResID = (int)table_data.resid2;
                }
            }

            ////////////////////////////////////////////////////////////////////////

            // 对幻师职业 形状2的特殊处理
            if (changeEquip.pos == EquipPos.EquipPos_Weapon)
            {
                if(nJob == 0)
                {
                    nJob = m_Owner.GetProp((int)RobotProp.Job);
                }
                if (nJob == (int)GameCmd.enumProfession.Profession_Spy && changeEquip.nStateType == (int)Client.SkillSettingState.StateTwo && table_data.typeMask != 1) // 非时装
                {
                    nResID = 0;
                }
            }

            // 修改EntityCreateData中ViewList数据
            AddViewToCreateData(changeEquip.pos, changeEquip.nSuitID);
            if (changeEquip.pos != EquipPos.EquipPos_Body && !IsBodyReady())
            {
                // 主体对象没有创建完毕，挂件要稍后再创建
                return;
            }

            // 变身状态只保存数据，不换外观
            if(m_bChange)
            {
                return;
            }

            // 同样的时装忽略
            if (IsExistSuit(changeEquip.pos, changeEquip.nSuitID, nResID))
            {
                Engine.Utility.Log.Error("同样的时装忽略 {0} {1}", changeEquip.pos, changeEquip.nSuitID);
                return;
            }

            if (changeEquip.pos <= EquipPos.EquipPos_Null || changeEquip.pos >= EquipPos.EquipPos_Max)
            {
                Engine.Utility.Log.Error("装备位置非法 {0} {1}", changeEquip.pos, changeEquip.nSuitID);
                return;
            }

            EquipData data = AddSuitData(changeEquip.pos, changeEquip.nSuitID, nResID, null, changeEquip.evt, changeEquip.param);
            if(data==null)
            {
                Engine.Utility.Log.Error("ChangeEquip:找不到时装数据EquipData!");
                return;
            }

            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            // 卸下之前的外观
            if (data.m_obj != null)
            {
                if (changeEquip.pos == EquipPos.EquipPos_Body)
                {
                    if (m_bRide)
                    {
                        m_RideCharObj.RemoveAllLinkObj();
                        if (m_EntityCreateData.bMainPlayer)
                        {
                            // 主角 修改材质
                            data.m_obj.ChangeMaterial("Custom/myshader_Normal_Spec");
                        }
                        
                        m_RideCharObj = null;
                        
                        m_RideObj.RemoveLinkObj(m_nRideLinkID);
                        rs.RemoveRenderObj(data.m_obj);
                        data.m_obj = null;
                        m_nRideLinkID = 0;

                    }
                    else
                    {
                        data.m_obj.RemoveAllLinkObj();
                        if (m_EntityCreateData.bMainPlayer)
                        {
                            // 主角 修改材质
                            data.m_obj.ChangeMaterial("Custom/myshader_Normal_Spec");
                        }
                        rs.RemoveRenderObj(data.m_obj);
                        data.m_obj = null;
                        if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner.GetUID()))
                        {
                            Engine.Utility.Log.LogGroup("ZDY", "m_RenderObjNode == null");
                        }
                        m_RenderObj = null;
                      
                    }

                    m_dicEquip.Clear();

                    data = AddSuitData(changeEquip.pos, changeEquip.nSuitID, nResID, null, changeEquip.evt, changeEquip.param);
                }
                else
                {
                    Engine.IRenderObj obj = null;
                    if (m_bRide)
                    {
                        obj = m_RideCharObj.RemoveLinkObj(data.m_nLinkObj, false);
                    }
                    else
                    {
                        obj = m_RenderObj.RemoveLinkObj(data.m_nLinkObj,false);
                    }

                    if (m_EntityCreateData.bMainPlayer && obj !=null)
                    {
                        // 主角 修改材质
                        obj.ChangeMaterial("Custom/myshader_Normal_Spec");
                    }
                    rs.RemoveRenderObj(data.m_obj);
                    data.m_obj = null;
                }
            }

            // 表明没有新的外观要换上
            if(table_data==null)
            {
                Engine.Utility.Log.Error("ChangeEquip:找不到时装数据{0}!", changeEquip.nSuitID);
                return;
            }

            // 换上新的外观
            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>((uint)nResID);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("Equip:找不到Equip模型资源路径配置{0}", data.m_nResID);

                return;
            }

            string strResName = resDB.strPath;
            // 创建新的外观
            if (string.IsNullOrEmpty(strResName))
            {
                return;
            }

            data.strLocatorName = table_data.locator_name;
            string strOffset = table_data.offset.Replace('_', ',');
            data.offset = Engine.Utility.StringUtility.ParseVector3(strOffset); // 约定格式
            if (changeEquip.pos == EquipPos.EquipPos_Body)
            {
                if (m_bRide)
                {
                    rs.CreateRenderObj(ref strResName, ref m_RideCharObj, OnCreateEquipRenderObj, changeEquip.pos,
                        m_bImmediate ? TaskPriority.TaskPriority_Immediate : TaskPriority.TaskPriority_Normal, true, data.m_nSuitID);
                    if (m_RideCharObj != null)
                    {
                        m_RideCharObj.SetLayer((int)m_eLayer);
                    }
                }
                else
                {
                    rs.CreateRenderObj(ref strResName, ref m_RenderObj, OnCreateEquipRenderObj, changeEquip.pos, 
                        m_bImmediate ? TaskPriority.TaskPriority_Immediate : TaskPriority.TaskPriority_Normal, true, data.m_nSuitID);
                    if (m_RenderObj != null)
                    {
                        m_RenderObj.SetLayer((int)m_eLayer);
                    }
                }
            }
            else
            {
                rs.CreateRenderObj(ref strResName, ref data.m_obj, OnCreateEquipRenderObj, changeEquip.pos, 
                    m_bImmediate ? TaskPriority.TaskPriority_Immediate : TaskPriority.TaskPriority_Normal, true, data.m_nSuitID);
                if (data.m_obj != null)
                {
                    data.m_obj.SetLayer((int)m_eLayer);
                }
            }

        }
        //-------------------------------------------------------------------------------------------------------
        // 主体是否创建完毕
        private bool IsBodyReady()
        {
            EquipData data = null;
            if (!m_dicEquip.TryGetValue(EquipPos.EquipPos_Body,out data))
            {
                return false;
            }

            if(data==null)
            {
                return false;
            }

            return data.bReady;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 添加EntityCreateData中ViewList数据
        @param 
        */
        private void AddViewToCreateData(EquipPos pos, int nSuiltID)
        {
            if(m_EntityCreateData==null)
            {
                return;
            }

            if (m_EntityCreateData.ViewList==null)
            {
                return;
            }

            if(m_EntityCreateData.ViewList.Length<(int)Client.EquipPos.EquipPos_Max)
            {
                return;
            }

            bool bExist = false;
            bool bIndex = false;
            int nIndex = -1;
            for(int i = 0; i < m_EntityCreateData.ViewList.Length; ++i)
            {
                if(m_EntityCreateData.ViewList[i]==null)
                {
                    nIndex = i;
                    bIndex = true;
                    continue;
                }

                if (pos == (EquipPos)m_EntityCreateData.ViewList[i].nPos)
                {
                    m_EntityCreateData.ViewList[i].value = nSuiltID;
                    bExist = true;
                }

                if (!bIndex)
                {
                    nIndex++;
                }
            }

            if(!bExist)
            {
                EntityViewProp prop = new EntityViewProp((int)pos, nSuiltID);
                m_EntityCreateData.ViewList[nIndex] = prop;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        // 对象名称
        public void AddName(string strText, Engine.FontID fontName, int nFontSize, float fCharSize,Color color, string strLocatorName, Vector3 offset)
        {
            m_nNameID = AddRenderText(strText, fontName, nFontSize, fCharSize, color, strLocatorName,offset);
        }
        public void ChangeNameColor(Color color)
        {
            ModifyColor(m_nNameID, color);
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 添加文字
        @param 
        */
        public int AddRenderText(string strText, Engine.FontID fontName, int nFontSize, float fCharSize,Color color, string strLocatorName, Vector3 offset)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    int id = m_RideCharObj.AddRenderText(strText, fontName, nFontSize, fCharSize,color, offset, strLocatorName, LinkFollowType.LinkFollowType_Translate);
                    m_RenderTextID.Add(id);
                    return id;
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    int id = m_RenderObj.AddRenderText(strText, fontName, nFontSize, fCharSize, color, offset, strLocatorName, LinkFollowType.LinkFollowType_Translate);
                    m_RenderTextID.Add(id);
                    return id;
                }
            }
            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 删除绑定文字
        @param 
        */
        public void RemoveRenderText(int id)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_RenderTextID.Remove(id);
                    m_RideCharObj.RemoveRenderText(id);
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    m_RenderTextID.Remove(id);
                    m_RenderObj.RemoveRenderText(id);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public void ModifyText(int nLinkID, string strText)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    IRenderText rt = m_RideCharObj.GetLinkText(nLinkID);
                    if(rt!=null)
                    {
                        rt.SetText(strText);
                    }
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    IRenderText rt = m_RenderObj.GetLinkText(nLinkID);
                    if (rt != null)
                    {
                        rt.SetText(strText);
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------
        // 头顶血条
        public int AddHPBar(float fProgess, string strLocatorName, Vector3 offset)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_nHPBarID = m_RideCharObj.AddRenderBar(BAR_RES, fProgess, strLocatorName, offset);
                    return m_nHPBarID;
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    m_nHPBarID = m_RenderObj.AddRenderBar(BAR_RES, fProgess, strLocatorName, offset);
                    return m_nHPBarID;
                }
            }

            return 0;
        }

        public void ModifyHPProgress(float fProgress)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    IRenderBar bar = m_RideCharObj.GetRenderBar(m_nHPBarID);
                    if (bar != null)
                    {
                        bar.SetProgress(fProgress);
                    }
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    IRenderBar bar = m_RenderObj.GetRenderBar(m_nHPBarID);
                    if (bar != null)
                    {
                        bar.SetProgress(fProgress);
                    }
                }
            }
        }

        public void RemoveHPBar()
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_RideCharObj.RemoveRenderBar(m_nHPBarID);
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    m_RenderObj.RemoveRenderBar(m_nHPBarID);
                }
            }

            m_nHPBarID = 0;
        }

        //-------------------------------------------------------------------------------------------------------
        public void ModifyColor(int nLinkID, Color color)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    IRenderText rt = m_RideCharObj.GetLinkText(nLinkID);
                    if (rt != null)
                    {
                        rt.SetColor(color);
                    }
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    IRenderText rt = m_RenderObj.GetLinkText(nLinkID);
                    if (rt != null)
                    {
                        rt.SetColor(color);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        private void OnCreateEquipRenderObj(Engine.IRenderObj obj, object param)
        {
            if(obj==null || m_EntityCreateData == null)
            {
                return;
            }

            IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if(rs==null)
            {
                return;
            }

            EquipPos pos = (EquipPos)param;
            EquipData data = null;
            if (!m_dicEquip.TryGetValue(pos, out data))
            {
                return;
            }

            int nObjSuitID = (int)obj.GetData();
            if (data.m_nSuitID!=nObjSuitID)
            {
                rs.RemoveRenderObj(obj);
                // 重复的对象已经删除，不需要走下面的挂接
                return;
            }

            data.m_obj = obj;

            obj.SetVisible(m_bVisible);
            obj.ShowModel(m_bShowModel);

            if(m_EntityCreateData.bMainPlayer)
            {
                // 主角 修改材质
                obj.ChangeMaterial("Custom/myshader_Normal_Spec_Xary");
            }

            // addLinkObj
            if(pos==EquipPos.EquipPos_Body) // 换主体
            {
                if (m_bRide && m_RideObj.IsValid())
                {
                    // 设置上马前的环境
                    m_RenderObj = obj;
                   
                    m_RenderObj.GetNode().GetTransForm().parent = m_RideObj.GetNode().GetTransForm().parent;

                    OnCreateRide(m_RideObj, null); // 上马操作
                }
                else
                {
                    m_RenderObj = obj;
                  

                    obj.GetNode().SetLocalPosition(m_pos);
                    Quaternion rot = new Quaternion();
                    rot.eulerAngles = m_rot;
                    obj.GetNode().SetLocalRotate(rot);
                }

                m_Owner.PlayAction(m_strAniName);
                data.bReady = true;

                // 根据全局配置设置阴影
                int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1);
                SetShadowLevel(nShadowLevel);

                // 其它部位挂接
                for (int i = 0; i < m_EntityCreateData.ViewList.Length; ++i)
                {
                    if (m_EntityCreateData.ViewList[i]==null)
                    {
                        continue;
                    }

                    if (m_EntityCreateData.ViewList[i].nPos != (int)EquipPos.EquipPos_Body)
                    {
                        Client.ChangeEquip changeEquip = new Client.ChangeEquip();
                        changeEquip.pos = (EquipPos)m_EntityCreateData.ViewList[i].nPos;
                        changeEquip.nSuitID = m_EntityCreateData.ViewList[i].value;
                        changeEquip.nLayer = (int)m_eLayer;
                        if (m_Owner.GetEntityType() == EntityType.EntityType_Player)
                        {
                            changeEquip.nStateType = m_Owner.GetProp((int)PlayerProp.SkillStatus);
                        }
                        else if (m_Owner.GetEntityType() == EntityType.EntityType_Puppet)
                        {
                            changeEquip.nStateType = (int)m_EntityCreateData.eSkillState;
                        }
                        ChangeEquip(changeEquip);
                    }
                }
            }
            else
            {
                //挂接装备
                LinkEquip(obj, pos, data.strLocatorName, data.offset);               
            }

            if(data.m_obj!=null)
            {
                data.m_obj.SetVisible(m_bVisible);
                data.m_obj.ShowModel(m_bShowModel);
            }

            if (m_Owner.GetEntityType() == EntityType.EntityType_Player)
            {
                if (!EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner.GetUID()))
                {
                    int nSuitID = data.m_nSuitID;

                    uint nMapID = EntitySystem.m_ClientGlobal.GetMapSystem().GetMapID();
                    table.MapDataBase mapDB = GameTableManager.Instance.GetTableItem<table.MapDataBase>(nMapID);
                    if (mapDB != null)
                    {
                        if (mapDB.dwMaterial == 0)
                        {
                            table.SuitDataBase suitData = GameTableManager.Instance.GetTableItem<table.SuitDataBase>((uint)data.m_nSuitID);
                            if (suitData == null)
                                return;
                            //修改 有报错再改回来
                            //if (suitData.defaultMaterial == 0)
                            //    return;
                            if (suitData.defaultMaterial != 0)//换成了这个
                            {
                                table.ResourceDataBase res = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(suitData.defaultMaterial);
                                if (res == null)
                                    return;

                                if (res.strPath == "")
                                    return;

                                Engine.IRenderObj renderObj = data.m_obj;
                                if (renderObj != null)
                                {
                                    renderObj.ApplyMaterial(res.strPath);
                                }
                            }
                        }
                        else
                        {
                            table.ResourceDataBase res = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(mapDB.dwMaterial);
                            if (res == null)
                                return;

                            if (res.strPath == "")
                                return;

                            Engine.IRenderObj renderObj = data.m_obj;
                            if (renderObj != null)
                            {
                                renderObj.ApplyMaterial(res.strPath);
                            }

                        }
                    }

                    
                }
                
            }


            if (data.evt != null)
            {
                string name = obj.GetName();
                if (name.Contains("yugan"))
                {
                    data.evt(m_RenderObj, obj);
                }
                else 
                {
                    data.evt(m_RenderObj, data.param);
                }
                
                data.evt = null;
            }
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 挂接装备
        @param pos 装备位置
        @param strLocatorName 绑定点名称
        */
        private void LinkEquip(IRenderObj equipObj, EquipPos pos, string strLocatorName, Vector3 offset)
        {
            if(equipObj==null)
            {
                return;
            }

            if(pos==EquipPos.EquipPos_Body)
            {
                return;
            }

            EquipData data = null;
            if (!m_dicEquip.TryGetValue(pos, out data))
            {
                return;
            }

            if(m_bRide)
            {
                if(m_RideCharObj!=null)
                {
                    data.m_nLinkObj = m_RideCharObj.AddLinkObj(equipObj, ref strLocatorName, offset, Quaternion.identity);
                }
            }
            else
            {
                if(m_RenderObj!=null)
                {
                    data.m_nLinkObj = m_RenderObj.AddLinkObj(equipObj, ref strLocatorName, offset, Quaternion.identity);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 创建Renderobj回调
        @param 
        */
        private void OnChangeRenderObj(Engine.IRenderObj obj, object param)
        {
            m_RenderObj = obj;

            if(m_RenderObj==null)
            {
                return;
            }

            // 根据全局配置设置阴影
            int nShadowLevel = EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1);
            SetShadowLevel(nShadowLevel);

       

            m_Owner.SetPos(ref m_pos);

            m_RenderObj.SetVisible(m_bVisible);
            m_RenderObj.ShowModel(m_bShowModel);
            if(m_suitModleScale == 0)
            {
                m_suitModleScale = 1;
            }
            if(m_RenderObj.GetNode() != null)
            {
                if(m_RenderObj.GetNode().GetTransForm() != null)
                {
                    m_RenderObj.GetNode().GetTransForm().localScale = new Vector3(m_suitModleScale, m_suitModleScale, m_suitModleScale);
                }
            }
            if (m_ChangeEventHandler != null)
            {
                m_ChangeEventHandler(m_ChangeParam);
                m_ChangeEventHandler = null;
                m_ChangeParam = null;
            }

            m_dicEquip.Clear();

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_CHANGE,
                new Client.stPlayerChange() { status = 1, uid = m_Owner.GetID() });

            // 根据外部设置来设置
            m_Owner.SendMessage(EntityMessage.EntityCommand_SetVisible, EntityConfig.m_bShowEntity);
        }
        //-------------------------------------------------------------------------------------------------------


        public Engine.Node GetNode()
        {
            if(m_RenderObj == null)
            {
                return null;
            }
            return m_RenderObj.GetNode() ;
        }

        public AnimationState GetAnimationState(string strAniName)
        {
            if(m_RenderObj==null)
            {
                return null;
            }

            return m_RenderObj.GetAnimationState(strAniName);
        }

        public void SetPos(ref Vector3 pos)
        {
            m_pos = pos;
            if (m_RenderObj != null )
            {
                m_RenderObj.GetNode().SetLocalPosition(pos);
            }
        }

        public Vector3 GetPos()
        {
            //if (m_RenderObj != null && m_RenderObjNode != null)
            //{
            //    return m_RenderObjNode.GetLocalPosition();
            //}
            return m_pos;
        }


        // 获取方向
        // 规定美术制作时，面向z轴负方向;
        public Vector3 GetDir()
        {
            if (m_RenderObj != null )
            {
                Quaternion rot = m_RenderObj.GetNode().GetLocalRotate();
                Matrix4x4 mat = new Matrix4x4();
                mat.SetTRS(Vector3.zero, rot, Vector3.one);
                return mat.GetColumn(2);
            }

            // Unity使用左手系 unity中对于矩阵的使用
            //Quaternion rotate = new Quaternion();
            //rotate.eulerAngles = new Vector3(0, 90, 0);
            //Matrix4x4 mat = new Matrix4x4();
            //mat.SetTRS(Vector3.zero, rotate, Vector3.one);

            //Quaternion rot = new Quaternion();
            //Vector4 up = mat.GetColumn(1);
            //Vector4 look = mat.GetColumn(2);
            //rot.SetLookRotation(look, up);

            return Vector3.forward;
        }

        /// unity 默认的旋转顺序 yxz
        /// <summary>
        /// 绕X轴旋转
        /// </summary>
        /// <param name="fAngle">角度值</param>
        public void RotateX(float fDeltaAngle)
        {
            if (m_RenderObj != null )
            {
                Quaternion rot = m_RenderObj.GetNode().GetLocalRotate();
                m_rot = rot.eulerAngles;
                m_rot.x += fDeltaAngle;
                rot.eulerAngles = m_rot;
                m_RenderObj.GetNode().SetLocalRotate(rot);
            }
        }

        /// unity 默认的旋转顺序 yxz
        /// <summary>
        /// 绕X轴旋转
        /// </summary>
        /// <param name="fAngle">角度值</param>
        public void RotateToX(float fAngle)
        {
            if (m_RenderObj != null)
            {
                Quaternion rot = m_RenderObj.GetNode().GetLocalRotate();
                m_rot = rot.eulerAngles;
                m_rot.x = fAngle;
                rot.eulerAngles = m_rot;
                m_RenderObj.GetNode().SetLocalRotate(rot);
            }
        }

        /// <summary>
        /// 绕Y轴旋转
        /// </summary>
        /// <param name="fAngle">角度值</param>
        public void RotateY(float fDeltaAngle)
        {
            if (m_RenderObj != null )
            {
                Quaternion rot = m_RenderObj.GetNode().GetLocalRotate();
                m_rot = rot.eulerAngles;
                m_rot.y += fDeltaAngle;
                rot.eulerAngles = m_rot;
                m_RenderObj.GetNode().SetLocalRotate(rot);
            }
        }

        /// <summary>
        /// 绕Y轴旋转
        /// </summary>
        /// <param name="fAngle">角度值</param>
        public void RotateToY(float fAngle)
        {
            m_rot = new Vector3(0,fAngle,0);
            if (m_RenderObj != null )
            {
                Quaternion rot = m_RenderObj.GetNode().GetLocalRotate();
                rot.eulerAngles = m_rot;
                m_RenderObj.GetNode().SetLocalRotate(rot);
            }
        }
         

        // 获取旋转欧拉角
        public Vector3 GetRotate()
        {
//             if (m_RenderObj != null && m_RenderObjNode != null)
//             {
//                 Quaternion rot = m_RenderObjNode.GetLocalRotate();
//                 return rot.eulerAngles;
//             }

            return m_rot;
        }

        // 设置旋转欧拉角
        public void SetRotate(Vector3 eulerAngle)
        {
            m_rot = eulerAngle;
            if (m_RenderObj != null )
            {
                Quaternion rot = new Quaternion();
                rot.eulerAngles = eulerAngle;
                m_RenderObj.GetNode().SetLocalRotate(rot);
            }
        }

        public void LookAt(Vector3 target, Vector3 up)
        {
            if (m_RenderObj != null )
            {
                m_RenderObj.GetNode().LookAt(target, up);
            }
        }

        /// <summary>
        /// 播放动作 
        /// </summary>
        /// <param name="strActionName"></param>
        public void PlayAction(string strActionName, int nStartFrame = 0, float fSpeed = 1f, float fBlendTime = -1.0f, int nLoop = -1, Engine.ActionEvent callback = null, object param = null)
        {
            m_strAniName = strActionName;

            // 默认值为负数，需要计算CrossTime
            if(fBlendTime<0.0f)
            {
                fBlendTime = CalcAnimationCrossTime(strActionName);
            }

            // 目前简单播放动画
            if (m_RenderObj != null)
            {
                //如果骑乘后这里是播放坐骑的动作
                if (callback != null) m_RenderObj.SetAniCallback(callback, param);
                m_RenderObj.Play(strActionName, nStartFrame, fSpeed, fBlendTime, nLoop, m_bRide ? false : true ); // 动作不影响连接对象
            }

            if(m_bRide)
            {
                if(m_RideCharObj!=null)
                {
                    //这里播放人物的动作
                    string strAniName = strActionName + "_Mount";
                    m_RideCharObj.Play(strAniName, nStartFrame, fSpeed, fBlendTime, nLoop);
                }
            }
        }

        public bool IsPlayingAnim(string clip)
        {
            if(m_RenderObj == null)
            {
                return false;
            }
            return m_RenderObj.IsPlaying(clip);
        }
        /// <summary>
        /// 播放动作 
        /// </summary>
        /// <param name="strActionName"></param>
        public void PauseAni()
        {
            // 目前简单播放动画
            if (m_RenderObj != null)
            {
                m_RenderObj.Pause();
            }

            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_RideCharObj.Pause();
                }
            }
        }

        public string GetCurAniName()
        {
            if (m_RenderObj != null)
            {
                return m_RenderObj.GetCurAni();
            }

            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    return m_RideCharObj.GetCurAni();
                }
            }

            return "";
        }
        /// <summary>
        /// 播放动作 
        /// </summary>
        /// <param name="strActionName"></param>
        public void ResumeAni()
        {
            // 目前简单播放动画
            if (m_RenderObj != null)
            {
                m_RenderObj.Resume();
            }

            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_RideCharObj.Resume();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public void ClearAniCallback()
        {
            if (m_RenderObj != null)
            {
                m_RenderObj.SetAniCallback(null);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public void SetAniSpeed(float fSpeed)
        {
            if (m_RenderObj == null || m_Owner == null)
            {
                return;
            }

            bool bMoveing = (bool)m_Owner.SendMessage(EntityMessage.EntityCommand_IsMove,null);
            float speedFact = 1.0f;
            if (bMoveing)
            {
                EntityType et = m_Owner.GetEntityType();
                if (et == EntityType.EntityType_Player || et == EntityType.EntityTYpe_Robot)
                {
                    // 速度修正
                    uint speed = GameTableManager.Instance.GetGlobalConfig<uint>("Base_Move_Speed");
                    if (speed == 0)
                    {
                        speed = 1;
                    }
                    speedFact = (float)m_Owner.GetProp((int)WorldObjProp.MoveSpeed) / (float)speed;
                }
                else if (et == EntityType.EntityType_NPC)
                {
                    // 速度修正
                    int nNpcID = m_Owner.GetProp((int)EntityProp.BaseID);
                    var table_data = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)nNpcID);
                    if (table_data != null)
                    {
                        uint speed = table_data.dwRunSpeed;
                        if (speed == 0)
                        {
                            speed = 1;
                        }
                        speedFact = (float)m_Owner.GetProp((int)WorldObjProp.MoveSpeed) / (float)speed;
                    }
                }
            }

            m_RenderObj.SetAniSpeed(fSpeed*speedFact);
        }
        //-------------------------------------------------------------------------------------------------------
        //public void SetaniCallback()
        /// <summary>
        /// 修改部件
        /// </summary>
        /// <param name="strPartName">部件名称</param>
        /// <param name="strResourceName">资源名称</param>
        public void ChangePart(ref string strPartName, ref string strResourceName, string strMaterialName = "")
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_RideCharObj.ChangePart(ref strPartName, ref strResourceName, strMaterialName);
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    m_RenderObj.ChangePart(ref strPartName, ref strResourceName, strMaterialName);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public void EnableShadow(bool bEnable)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_RideCharObj.EnableShadow(bEnable);
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    m_RenderObj.EnableShadow(bEnable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        // 添加LinkObj
        /// <summary>
        /// 添加link对象
        /// </summary>
        /// <param name="pRenderObj">挂接对象</param>
        /// <param name="strLocatorName">挂接点名称，如果为空，则默认在原点处</param>
        /// <param name="vOffset">相对挂接点偏移</param>
        /// <param name="rotate">相对挂接点旋转</param>
        /// <param name="eFollowType">跟随类型</param>
        /// <returns>linkID,删除时使用</returns>
        public int AddLinkObj(Engine.IRenderObj pRenderObj, ref string strLocatorName, Vector3 vOffset, Quaternion rotate, Engine.LinkFollowType eFollowType = Engine.LinkFollowType.LinkFollowType_ALL)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    return m_RideCharObj.AddLinkObj(pRenderObj, ref strLocatorName, vOffset, rotate, eFollowType);
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    return m_RenderObj.AddLinkObj(pRenderObj, ref strLocatorName, vOffset, rotate, eFollowType);
                }
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        // 添加LinkObj
        /// <summary>
        /// 添加link对象
        /// </summary>
        /// <param name="strRenderObj">挂接对象资源名</param>
        /// <param name="strLocatorName">挂接点名称，如果为空，则默认在原点处</param>
        /// <param name="vOffset">相对挂接点偏移</param>
        /// <param name="rotate">相对挂接点旋转</param>
        /// <param name="eFollowType">跟随类型</param>
        /// <returns>linkID,删除时使用</returns>
        public int AddLinkObj(ref string strRenderObj, ref string strLocatorName, Vector3 vOffset, Quaternion rotate, Engine.LinkFollowType eFollowType = Engine.LinkFollowType.LinkFollowType_ALL)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    return m_RideCharObj.AddLinkObj(ref strRenderObj, ref strLocatorName, vOffset, rotate, eFollowType);
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    return m_RenderObj.AddLinkObj(ref strRenderObj, ref strLocatorName, vOffset, rotate, eFollowType);
                }
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 删除挂接对象
        /// </summary>
        /// <param name="nLinkID">挂接对象ID</param>
        /// <param name="bRemove">是否删除对象标志</param>
        /// <returns></returns>
        public Engine.IRenderObj RemoveLinkObj(int nLinkID, bool bRemove = true)
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    Engine.IRenderObj obj = m_RideCharObj.RemoveLinkObj(nLinkID, bRemove);
                    if(obj==null)
                    {
                        obj = m_RenderObj.RemoveLinkObj(nLinkID, bRemove);
                    }

                    return obj;
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    return m_RenderObj.RemoveLinkObj(nLinkID, bRemove);
                }
            }

            return null;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 删除所有LinkObj
        */
        public void RemoveAllLinkObj()
        {
            if (m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_RideCharObj.RemoveAllLinkObj();
                }
            }
            //else
            {
                if (m_RenderObj != null)
                {
                    m_RenderObj.RemoveAllLinkObj();
                }
            }
        }


        //-------------------------------------------------------------------------------------------------------
        // 添加LinkEffect
        /// <summary>
        /// 添加link对象
        /// </summary>
        /// <param name="strEffect">挂接对象资源名</param>
        /// <param name="strLocatorName">挂接点名称，如果为空，则默认在原点处</param>
        /// <param name="vOffset">相对挂接点偏移</param>
        /// <param name="rotate">相对挂接点旋转</param>
        /// <param name="eFollowType">跟随类型</param>
        /// <returns>linkID,删除时使用</returns>
        public int AddLinkEffect(ref string strEffect, ref string strLocatorName, Vector3 vOffset, Quaternion rotate,Vector3 scale, Engine.LinkFollowType eFollowType = Engine.LinkFollowType.LinkFollowType_ALL, bool bIgnoreRide = true, int nLevel = 0, Engine.EffectCallback cb = null, object param = null)
        {
            if (m_bRide)
            {
                if(!bIgnoreRide)
                {
                    if (m_RenderObj != null)
                    {
                        return m_RenderObj.AddLinkEffect(ref strEffect, ref strLocatorName, vOffset, rotate, scale, eFollowType, nLevel,cb,param);
                    }
                }
                else
                {
                    if (m_RideCharObj != null)
                    {
                        int ret = m_RideCharObj.AddLinkEffect(ref strEffect, ref strLocatorName, vOffset, rotate, scale, eFollowType, nLevel, cb, param);
                        if(IsRide())
                        {
                            m_RideCharObj.ForceUpdateLinkEffect(true);
                        }
                        return ret;
                    }
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    return m_RenderObj.AddLinkEffect(ref strEffect, ref strLocatorName, vOffset, rotate, scale, eFollowType, nLevel, cb, param);
                }
            }

            return 0;
        }

        /// <summary>
        /// 删除挂接对象
        /// </summary>
        /// <param name="nLinkID">挂接对象ID</param>
        /// <param name="bRemove">是否删除对象标志</param>
        /// <returns></returns>
        public Engine.IEffect RemoveLinkEffect(int nLinkID, bool bRemove = true)
        {
            // nLinkID 是全局唯一的
            if(m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    Engine.IEffect effect = m_RideCharObj.RemoveEffect(nLinkID, bRemove);
                    if (effect == null)
                    {
                        effect = m_RenderObj.RemoveEffect(nLinkID, bRemove);
                    }

                    return effect;
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    return m_RenderObj.RemoveEffect(nLinkID, bRemove);
                }
            }

            return null;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 删除所有LinkEffect
        @param 
        */
        public void RemoveAllLinkEffect()
        {
            if(m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_RideCharObj.RemoveAllLinkEffect();
                }
            }
            //else
            {
                if (m_RenderObj != null)
                {
                    m_RenderObj.RemoveAllLinkEffect();
                }
            }

        }
        //-------------------------------------------------------------------------------------------------------
        public void SetVisible(bool bVisible)
        {
            m_bVisible = bVisible;
            if (m_RenderObj != null)
            {
                m_RenderObj.SetVisible(bVisible);
            }
            if(m_bRide)
            {
                if(m_RideObj!=null)
                {
                    m_RideObj.SetVisible(bVisible);
                }
            }

            if (bVisible)
            {
                // 恢复下动作播放
                // Visible是通过SetActive来实现的，我们动画没有设置默认动作<unity在SetActive(true)的时候，统一播放默认动作（很坑）>
                // 所以这里我们自己播放下动作
                if (m_Owner != null)
                {
                    bool isMove = (bool)m_Owner.SendMessage(EntityMessage.EntityCommand_IsMove, null);

                    if (isMove)
                    {
                        m_Owner.PlayAction(EntityAction.Run);
                    }
                    else
                    {
                        m_Owner.PlayAction(EntityAction.Stand);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public bool IsVisible()
        {
            //if (m_RenderObj != null)
            //{
            //    return m_RenderObj.IsVisible();
            //}

            return m_bVisible;
        }
        //-------------------------------------------------------------------------------------------------------
        public void ShowModel(bool bEnable)
        {
            m_bShowModel = bEnable;
            if (m_RenderObj != null)
            {
                m_RenderObj.ShowModel(bEnable);
            }
            if (m_bRide)
            {
                if (m_RideObj != null)
                {
                    m_RideObj.ShowModel(bEnable);
                }
            }

            if (bEnable)
            {
                // 恢复下动作播放
                // Visible是通过SetActive来实现的，我们动画没有设置默认动作<unity在SetActive(true)的时候，统一播放默认动作（很坑）>
                // 所以这里我们自己播放下动作
                if (m_Owner != null)
                {
                    bool isMove = (bool)m_Owner.SendMessage(EntityMessage.EntityCommand_IsMove, null);

                    if (isMove)
                    {
                        m_Owner.PlayAction(EntityAction.Run);
                    }
                    else
                    {
                        m_Owner.PlayAction(EntityAction.Stand);
                    }
                }
            }
        }
        public bool IsShowModel()
        {
            //if (m_RenderObj != null)
            //{
            //    return m_RenderObj.IsShowModel();
            //}

            return m_bShowModel;
        }
        public void SetAlpha(float alpha)
        {
            bool bMainPlayer = EntitySystem.m_ClientGlobal.IsMainPlayer(m_Owner);
//            if (bMainPlayer)
            {
                if (alpha < 1f)
                {
                    if (m_RenderObj != null)
                        m_RenderObj.ChangeMaterial("Custom/myshader_Normal_Spec_Alpha");

                    if (m_bRide)
                    {
                        if (m_RideObj != null)
                            m_RideObj.ChangeMaterial("Custom/myshader_Normal_Spec_Alpha");
                    }


                    Dictionary<EquipPos, EquipData>.Enumerator it = m_dicEquip.GetEnumerator();
                    while (it.MoveNext())
                    {
                        EquipData data = it.Current.Value;
                        if (data != null)
                        {
                            if (data.m_obj != null)
                                data.m_obj.ChangeMaterial("Custom/myshader_Normal_Spec_Alpha");
                           
                        }
                    }

                }
                else
                {
                    if (m_RenderObj != null)
                        m_RenderObj.ChangeMaterial("Custom/myshader_Normal_Spec_Xary");

                    if (m_bRide)
                    {
                        if (m_RideObj != null)
                            m_RideObj.ChangeMaterial("Custom/myshader_Normal_Spec_Xary");
                    }

                    Dictionary<EquipPos, EquipData>.Enumerator it = m_dicEquip.GetEnumerator();
                    while (it.MoveNext())
                    {
                        EquipData data = it.Current.Value;
                        if (data != null)
                        {
                            if (data.m_obj != null)
                                data.m_obj.ChangeMaterial("Custom/myshader_Normal_Spec_Xary");

                        }
                    }
                }
                    
            }
            
            if(m_RenderObj != null)
            {
                m_RenderObj.SetAlpha( alpha );
            }
            if (m_bRide)
            {
                if (m_RideObj != null)
                {
                    m_RideObj.SetAlpha(alpha);
                }
            }
        }
        public void SetColor(Color color)
        {
            if(color == null)
            {
                return;
            }
            if (m_RenderObj != null)
            {
                m_RenderObj.SetColor(color);
            }
            if (m_bRide)
            {
                if (m_RideObj != null)
                {
                    m_RideObj.SetColor(color);
                }
            }
        }
        public void SetFlashColor(Color color,float fLift)
        {
            if (m_RenderObj != null)
            {
                m_RenderObj.FlashColor(color, fLift);
            }
            if (m_bRide)
            {
                if (m_RideObj != null)
                {
                    m_RideObj.FlashColor(color, fLift);
                }
            }
        }
        public void ChangeMaterial(string strShaderName)
        {
            if (m_RenderObj != null)
            {
                m_RenderObj.ChangeMaterial(strShaderName);
            }
            if (m_bRide)
            {
                if (m_RideObj != null)
                {
                    m_RideObj.ChangeMaterial(strShaderName);
                }
            }
        }

        public void ApplySharedMaterial(Material mat)
        {
            if (m_RenderObj != null)
            {
                m_RenderObj.ApplySharedMaterial(mat);
            }
        }

        public Material GetMaterial()
        {
            if (m_RenderObj != null)
            {
                return m_RenderObj.GetMaterial();
            }
            return null;
        }

        //-------------------------------------------------------------------------------------------------------
        public void SetShadowLevel(int nLevel)
        {
            if(m_RenderObj!=null)
            {
                if (nLevel < 2)
                {
                    m_RenderObj.EnablePlaneShadow(true);
                }
                else
                {
                    m_RenderObj.EnablePlaneShadow(false);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public bool HitTest(Ray ray,out RaycastHit rayHit)
        {
            if(m_RenderObj!= null)
            {
                return m_RenderObj.HitTest(ray,out rayHit);
            }

            rayHit = new RaycastHit();
            return false;
        }
        public void GetLocatorPos(string strLocatorName , Vector3 vOffset , Quaternion rot, ref Vector3 pos, bool bWorld = false)
        {
            if(m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    m_RideCharObj.GetLocatorPos(strLocatorName, vOffset, rot, ref pos, bWorld);
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    m_RenderObj.GetLocatorPos(strLocatorName, vOffset, rot, ref pos, bWorld);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------
        // 获取半径
        public float GetRadius()
        {
            if(m_bRide)
            {
                if (m_RideCharObj != null)
                {
                    return m_RideCharObj.GetRadius();
                }
            }
            else
            {
                if (m_RenderObj != null)
                {
                    return m_RenderObj.GetRadius();
                }
            }

            return 0.0f;
        }

        public void SetShowGhost(bool isShow)
        {
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 上马
        @param nRideID 坐骑ID
        */
        public void Ride(int nRideID)
        {
            if (m_bRide)
            {
                return;
            }
            if(nRideID==0)
            {
                return;
            }
            
            // 变身状态不允许上马
            if (m_bChange)
            {
                return;
            }

            if(m_RideObj!=null)
            {
                UnRide();
            }

            // 创建坐骑
            var table_data = GameTableManager.Instance.GetTableItem<table.RideDataBase>((uint)nRideID);
            if (table_data == null)
            {
                Engine.Utility.Log.Error("不存在坐骑: " + nRideID);
                return;
            }

            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(table_data.resid);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("Entity:找不到坐骑模型资源路径配置{0}", table_data.resid);
                return;
            }
            string strResName = resDB.strPath;
            // TODO: 根据配置表读取数据               
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(strResName))
            {
                Engine.Utility.Log.Error("资源id{0}资源路径为空", table_data.resid);
                return;
            }

            m_RideObj = null;
            m_bRide = true;
            m_RideCharObj = m_RenderObj;
            rs.CreateRenderObj(ref strResName, ref m_RideObj, OnCreateRide);
            if (m_RideObj == null)
            {
                Engine.Utility.Log.Error("创建坐骑{0}失败,资源路径{1}", nRideID, strResName);
                return;
            }
            m_RideObj.SetLayer((int)m_eLayer);
        }

        //-------------------------------------------------------------------------------------------------------
        public void UnRide()
        {
            if(!m_bRide)
            {
                return;
            }

            if(m_RideObj==null)
            {
                Engine.Utility.Log.Error("严重错误：坐骑对象为空！");
                return;
            }

            // 变身状态不允许下马
            if (m_bChange)
            {
                return;
            }

            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            Vector3 pos = GetPos();
            string strCurAni = m_RideObj.GetCurAni();
            if(m_RideObj.GetNode() == null)
            {
                return;
            }
            if(m_RideObj.GetNode().GetTransForm() == null)
            {
                return;
            }
            Transform parent = m_RideObj.GetNode().GetTransForm().parent;
            if (m_nRideLinkID != 0)
            {
                m_RenderObj = m_RideObj.RemoveLinkObj(m_nRideLinkID, false);
                m_nRideLinkID = 0;
            }

            if (m_RenderObj != null && parent!=null)
            {
                if (m_RenderObj.GetNode() == null)
                {
                    return;
                }
                if (m_RenderObj.GetNode().GetTransForm() == null)
                {
                    return;
                }
                m_RenderObj.GetNode().GetTransForm().parent = parent;
            }

            SetShadowLevel(EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1));
            
            if (m_EntityCreateData.bMainPlayer)
            {
                // 主角 修改材质
                m_RideObj.ChangeMaterial("Custom/myshader_Normal_Spec");
            }

            rs.RemoveRenderObj(m_RideObj);

            m_RideCharObj = null;
            m_RideObj = null;
            m_bRide = false;

            if (m_Owner == null)
            {
                Engine.Utility.Log.Error("实体宿主 为 null ！!!");
                return;
            }

            m_Owner.PlayAction(strCurAni,0,1.0f,0.0f); // 下马不做动作融合
            m_Owner.SetPos(ref pos);

            stEntityUnRide unride = new stEntityUnRide();
            unride.uid = m_Owner.GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_UNRIDE, unride);
            if (m_RenderObj != null)
            {
                m_RenderObj.ForceUpdateLinkEffect(false);
            }      
        }
        //-------------------------------------------------------------------------------------------------------
        public bool IsRide()
        {
            return m_bRide && m_nRideLinkID != 0;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnCreateRide(Engine.IRenderObj obj, object param)
        {
            m_RideObj = obj;
            if(m_RideObj==null)
            {
                //    Engine.Utility.Log.Error("{0} CreateRide ", m_RideObj.GetID());
                Engine.Utility.Log.Error(" CreateRide m_RideObj is null");
                return;
            }
            if (m_RenderObj == null)
            {
                Engine.Utility.Log.Error("{0} CreateRide", m_RideObj.GetID());
                return;
            }
           
            if (m_RideObj.GetID() == m_RenderObj.GetID())
            {
                return;
            }
       
            if (!m_RenderObj.IsValid())
            {
                return;
            }
            stEntityBeginRide beginRide = new stEntityBeginRide();
            beginRide.uid = m_Owner.GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_BEGINRIDE, beginRide);

            // 设置位置
            Vector3 pos = GetPos();
            Vector3 rot = GetRotate();
            //获取当前主角的动作
            string strCurAni = m_RenderObj.GetCurAni();

            if (strCurAni.Equals(EntityAction.Stand_Combat))
            {
                strCurAni = EntityAction.Stand;
            }
            if (strCurAni.Equals(EntityAction.Run_Combat))
            {
                strCurAni = EntityAction.Run;
            }

            string strLinkName = "Mount";
            // 挂接对象
            m_nRideLinkID = m_RideObj.AddLinkObj(m_RenderObj, ref strLinkName, Vector3.zero, Quaternion.identity);

            m_RenderObj.EnablePlaneShadow(false);

            Engine.Utility.Log.LogGroup("ZCX", " {0} AddLinkObj {1} id {2}", m_RideObj.GetID(), m_RenderObj.GetID(),m_nRideLinkID);

            if (m_EntityCreateData.bMainPlayer)
            {
                // 主角 修改材质
                m_RideObj.ChangeMaterial("Custom/myshader_Normal_Spec_Xary");
            }
           
            //主角
            if(m_RenderObj.GetNode() != null)
            {
                if(m_RenderObj.GetNode().GetTransForm() != null)
                {
                    Transform parent = m_RenderObj.GetNode().GetTransForm().parent;
                    m_RideCharObj = m_RenderObj;
                    //骑乘后m_RenderObj 为坐骑了
                    m_RenderObj = m_RideObj;
                    if(parent != null)
                    {
                        if (m_RenderObj != null)
                        {
                            if (m_RenderObj.GetNode() != null)
                            {
                                if (m_RenderObj.GetNode().GetTransForm() != null)
                                {
                                    m_RenderObj.GetNode().GetTransForm().parent = parent;
                                }
                                else
                                {
                                    Engine.Utility.Log.Error("m_RenderObj.GetNode()GetTransForm is null");
                                }
                            }
                            else
                            {
                                Engine.Utility.Log.Error("m_RenderObj.GetNode() is null");
                            }

                            //m_RenderObj.SetLayer(LayerMask.NameToLayer("Horse"));
                        }
                    }
                    else
                    {
                        Engine.Utility.Log.Error("m_RenderObj.GetNode()GetTransForm parent is null");
                    }
            
                }
                else
                {
                    Engine.Utility.Log.Error("m_RenderObj.GetNode()GetTransForm is null");
                }
            }
            else
            {
                Engine.Utility.Log.Error("m_RenderObj.GetNode() is null");
            }

            SetShadowLevel(EntitySystem.m_ClientGlobal.gameOption.GetInt("Render", "shadow", 1));
            m_RideObj.SetVisible(m_bVisible);
            m_RideObj.ShowModel(m_bShowModel);

            m_Owner.PlayAction(strCurAni,0,1.0f,0.0f);
            m_Owner.SetPos(ref pos);
            m_Owner.SetRotate(rot);
            stEntityRide ride = new stEntityRide();
            ride.uid = m_Owner.GetUID();
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_RIDE, ride);

            // 根据外部设置来设置
            m_Owner.SendMessage(EntityMessage.EntityCommand_EnableShowModel, EntityConfig.m_bShowEntity);

            m_RideCharObj.ForceUpdateLinkEffect(true);
        }

        private float CalcAnimationCrossTime(string strCurAni)
        {
            if(m_RenderObj==null)
            {
                return 0.0f;
            }

            string strCurAniName = m_RenderObj.GetCurAni();
            if (string.Compare(strCurAniName, EntityAction.Stand) == 0 && string.Compare(strCurAni, EntityAction.Run) == 0)
            {
                return 0.1f;
            }

            if (string.Compare(strCurAniName, EntityAction.Stand_Combat) == 0 && string.Compare(strCurAni, EntityAction.Run_Combat) == 0)
            {
                return 0.1f;
            }

            float len = m_RenderObj.GetCurAniLength();
            AnimationState anistate = m_RenderObj.GetAnimationState(strCurAni);
            float lenNew = 0.0f;
            if(anistate!=null)
            {
                lenNew = anistate.length;
            }
            return (len + lenNew) * 0.1f;
        }
    }
}
