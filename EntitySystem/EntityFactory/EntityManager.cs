
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;
using table;
using UnityEngine.Profiling;

namespace EntitySystem
{
    /// <summary>
    /// 实体查询管理器
    /// </summary>
    class EntityManager
    {
        static EntityManager s_Inst = null;
        public static EntityManager Instance()
        {
            if (null == s_Inst)
            {
                s_Inst = new EntityManager();
            }

            return s_Inst;
        }

        // 所有实体
        private Dictionary<long, IEntity> m_dicEntity = new Dictionary<long, IEntity>();

        // 限制玩家数量显示
        // 实际显示的对象
        private Dictionary<long, IEntity> m_dicShowEntity = new Dictionary<long, IEntity>();
        // 角色ID
        private List<long> m_lstUID = new List<long>();
        private int m_nMaxPlayerNum = 0;

        #region EntityList
        class EntityList
        {
            private Dictionary<long, IEntity> m_dicEntity = new Dictionary<long, IEntity>();

            public bool AddEntity(IEntity en)
            {
                if (m_dicEntity.ContainsKey(en.GetUID()))
                {
                    Engine.Utility.Log.Error("已经存在uid:{0}的实体", en.GetUID());
                    return false;
                }

                m_dicEntity.Add(en.GetUID(), en);

                return true;
            }

            public IEntity GetEntity(long uid)
            {
                IEntity en = null;
                m_dicEntity.TryGetValue(uid, out en);
                return en;
            }

            public int Count
            {
                get { return m_dicEntity == null ? 0 : m_dicEntity.Count; }
            }

            public void RemoveEntity(IEntity en)
            {
                if (en == null)
                {
                    return;
                }

                RemoveEntity(en.GetUID());
            }
            List<IEntity> entityArray = new List<IEntity>(20);
            public List<IEntity> ToArray()
            {//去掉m_dicEntity.Values.ToArray() gc modify by dianyu
                Profiler.BeginSample("toarray");
                entityArray.Clear();
                var iter = m_dicEntity.GetEnumerator();
                while(iter.MoveNext())
                {
                    entityArray.Add(iter.Current.Value);
                }
             /*   IEntity[] arr = m_dicEntity.Values.ToArray();*/
            
                Profiler.EndSample();
                return entityArray;
            }

            public void RemoveEntity(long uid)
            {
                m_dicEntity.Remove(uid);
            }
        }
        #endregion

        // 实体
        private EntityList[] EntityMap = new EntityList[(int)EntityType.EntityType_Max];

        public long[] entityID
        {
            get { return m_dicEntity.Keys.ToArray<long>(); }
        }

        public void AddEntity(IEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            if (!m_dicEntity.ContainsKey(entity.GetUID()))
            {
                m_dicEntity.Add(entity.GetUID(), entity);
            }
            else
            {
                Engine.Utility.Log.Error("重复添加实体对象:{0}", entity.GetName());
            }

            EntityType type = entity.GetEntityType();
            if (EntityMap[(int)type] == null)
            {
                EntityMap[(int)type] = new EntityList();
            }

            EntityMap[(int)type].AddEntity(entity);


        }

        public void OnCreateEntity(IEntity en)
        {
            if (en == null)
            {
                return;
            }
            if (!(en.GetEntityType() == EntityType.EntityType_Player))
            {
                return;
            }

            if (m_dicShowEntity.ContainsKey(en.GetUID()))
            {
                return;
            }

            if (EntitySystem.m_ClientGlobal.IsMainPlayer(en))
            {
                return;
            }

            if (m_dicShowEntity.Count >= m_nMaxPlayerNum)
            {
                if ((bool)en.SendMessage(EntityMessage.EntityCommand_IsShowModel, null))
                {
                    ShowPlayerAndSuite((IPlayer)en, false);
                }
            }
            else
            {
                m_dicShowEntity.Add(en.GetUID(), en);
            }
        }

        public void RemoveEntity(IEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            EntityType t = entity.GetEntityType();
            EntityMap[(int)t].RemoveEntity(entity);

            // 删除显示的列表
            m_dicShowEntity.Remove(entity.GetUID());

            m_dicEntity.Remove(entity.GetUID());

            entity.Release();
            entity = null;
        }

        /// <summary>
        /// 获取当前显示的实体UID列表
        /// </summary>
        /// <returns></returns>
        public List<long> GetEntityUids()
        {
            if (null != m_dicEntity)
            {
                return m_dicEntity.Keys.ToList();
            }
            return null;
        }

        public void FindAllEntity<T>(ref List<T> list)
        {
            if(list == null)
            {
                return;
            }

            list.Clear();
            Type t = typeof(T);
            if (t == typeof(IPlayer))
            {
                if (EntityMap[(int)EntityType.EntityType_Player] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Player].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(IMonster))
            {
                if (EntityMap[(int)EntityType.EntityType_Monster] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Monster].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {

                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(INPC))
            {
                if (EntityMap[(int)EntityType.EntityType_NPC] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_NPC].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(IRobot))
            {
                if (EntityMap[(int)EntityType.EntityTYpe_Robot] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityTYpe_Robot].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(IItem))
            {
                if (EntityMap[(int)EntityType.EntityType_Item] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Item].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(IPlant))
            {
                if (EntityMap[(int)EntityType.EntityType_Plant] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Plant].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(IAnimal))
            {
                if (EntityMap[(int)EntityType.EntityType_Animal] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Animal].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(IMinerals))
            {
                if (EntityMap[(int)EntityType.EntityType_Minerals] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Minerals].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(ITree))
            {
                if (EntityMap[(int)EntityType.EntityType_Tree] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Tree].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(ISoil))
            {
                if (EntityMap[(int)EntityType.EntityType_Soil] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Soil].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(IPuppet))
            {
                if (EntityMap[(int)EntityType.EntityType_Puppet] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Puppet].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(IBox))
            {
                if (EntityMap[(int)EntityType.EntityType_Box] != null)
                {
                    List<IEntity> enlist = EntityMap[(int)EntityType.EntityType_Box].ToArray();
                    for (int i = 0; i < enlist.Count; ++i)
                    {
                        list.Add((T)enlist[i]);
                    }
                }
            }
            else if (t == typeof(ICreature))
            {
                var dic = m_dicEntity.GetEnumerator();
                while(dic.MoveNext())
                {
                    IEntity en = dic.Current.Value;
                    Type ent = en.GetType();
                    if (typeof(ICreature).IsAssignableFrom(ent))
                    {
                        list.Add((T)en);
                    }
                }
            }
        }

        public IEntity GetEntity(long uid)
        {
            IEntity en = null;
            if (m_dicEntity.TryGetValue(uid, out en))
            {
                return en;
            }
            return null;
        }

        public void ShowEntity(bool bShow)
        {
            EntityConfig.m_bShowEntity = bShow;
            Dictionary<long, IEntity>.Enumerator iter = m_dicEntity.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current.Value.GetEntityType() == EntityType.EntityType_Puppet)
                {
                    continue;
                }
                iter.Current.Value.SendMessage(EntityMessage.EntityCommand_EnableShowModel, bShow);
            }
        }

        class PlayerDisCompare : IComparer<long>
        {
            public int Compare(long x, long y)
            {
                float dis1 = GetFastDistance(x);
                float dis2 = GetFastDistance(y);

                if (dis1 > dis2)
                {
                    return 1;
                }
                else if (dis1 < dis2)
                {
                    return -1;
                }

                return 0;
            }

            float GetFastDistance(long id)
            {
                IEntity en = EntityManager.Instance().GetEntity(id);
                if (en == null)
                {
                    return 0.0f;
                }

                if (EntitySystem.m_ClientGlobal.MainPlayer == null)
                {
                    return 0.0f;
                }

                Vector3 pos = EntitySystem.m_ClientGlobal.MainPlayer.GetPos();
                pos.y = 0.0f;
                Vector3 otherPos = en.GetPos();
                otherPos.y = 0.0f;

                float dx = pos.x - otherPos.x;
                float dz = pos.z - otherPos.z;
                return dx * dx + dz * dz;
            }
        }
        /**
        @brief 同步同屏玩家
        @param 
        */
        public void UpdateMaxPlayer(int nMax)
        {
            m_nMaxPlayerNum = nMax;

            if (EntitySystem.m_ClientGlobal == null)
            {
                return;
            }

            if (EntityMap[(int)EntityType.EntityType_Player] == null)
            {
                return;
            }

            int nIndex = 0;
            if (nMax < m_dicShowEntity.Count)
            {
                m_lstUID = m_dicShowEntity.Keys.ToList();
                m_lstUID.Sort(new PlayerDisCompare());

                for (int i = m_lstUID.Count - 1; i >= 0; --i)
                {
                    if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_lstUID[i]))
                    {
                        continue;
                    }

                    IPlayer pp = GetEntity(m_lstUID[i]) as IPlayer;
                    if (pp == null)
                    {
                        continue;
                    }

                    //pp.SendMessage(EntityMessage.EntityCommand_EnableShowModel, false);
                    ShowPlayerAndSuite(pp, false);
                    m_dicShowEntity.Remove(m_lstUID[i]);

                    nIndex++;
                    if (nIndex >= nMax)
                    {
                        break;
                    }
                }
                return;
            }

            if (m_dicShowEntity.Count >= nMax)
            {
                return;
            }

            m_lstUID.Clear();
            List<IEntity> playerArr = EntityMap[(int)EntityType.EntityType_Player].ToArray();
            for (int i = 0; i < playerArr.Count; ++i)
            {
                if (playerArr[i] == null)
                {
                    continue;
                }

                if (EntitySystem.m_ClientGlobal.IsMainPlayer(playerArr[i]))
                {
                    continue;
                }

                if (!m_dicShowEntity.ContainsKey(playerArr[i].GetUID()))
                {
                    m_lstUID.Add(playerArr[i].GetUID());
                }
            }

            if (m_lstUID.Count <= 0)
            {
                return;
            }

            m_lstUID.Sort(new PlayerDisCompare());
            while (m_dicShowEntity.Count < nMax && m_lstUID.Count > 0)
            {
                IPlayer pp = GetEntity(m_lstUID[0]) as IPlayer;
                if (pp == null)
                {
                    m_lstUID.RemoveAt(0);
                    continue;
                }

                if (!(bool)pp.SendMessage(EntityMessage.EntityCommand_IsShowModel, null))
                {
                    //pp.SendMessage(EntityMessage.EntityCommand_EnableShowModel, true);
                    ShowPlayerAndSuite(pp, true);
                }

                nIndex++;
                m_dicShowEntity.Add(m_lstUID[0], pp);
                m_lstUID.RemoveAt(0);

                if (nIndex >= nMax)
                {
                    break;
                }
            }

            for (int i = 0; i < m_lstUID.Count; ++i)
            {
                if (EntitySystem.m_ClientGlobal.IsMainPlayer(m_lstUID[i]))
                {
                    continue;
                }

                IPlayer pp = GetEntity(m_lstUID[i]) as IPlayer;
                if (pp == null)
                {
                    continue;
                }

                //pp.SendMessage(EntityMessage.EntityCommand_EnableShowModel, false);
                ShowPlayerAndSuite(pp, false);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        //系统更新
        public void Update(float dt)
        {
            Dictionary<long, IEntity>.Enumerator iter = m_dicEntity.GetEnumerator();
            while (iter.MoveNext())
            {
                iter.Current.Value.Update(dt);
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        // 查找玩家相关对象 宠物和召唤物
        List<INPC> lstNpc = new List<INPC>();
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 显示玩家以及其附属宠物和召唤物
        @param 
        */
        private void ShowPlayerAndSuite(IPlayer player, bool bShow)
        {
            if (player == null)
            {
                return;
            }

            player.SendMessage(EntityMessage.EntityCommand_EnableShowModel, bShow);
            if (bShow)
            {
                PlayAnimation(player, EntityAction.Stand);
            }

            IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                return;
            }

            IControllerHelper ch = cs.GetControllerHelper();
            if (ch == null)
            {
                return;
            }

            ch.GetOwnNpcByPlayerID(player.GetID(), ref lstNpc);
            if (lstNpc == null)
            {
                return;
            }

            for (int i = 0; i < lstNpc.Count; ++i)
            {
                if (lstNpc[i] != null)
                {
                    lstNpc[i].SendMessage(EntityMessage.EntityCommand_EnableShowModel, bShow);
                }
            }
            lstNpc.Clear();
        }

        private void PlayAnimation(IEntity en, string strAniName)
        {
            if (en == null)
            {
                return;
            }

            PlayAni anim_param = new PlayAni();
            anim_param.strAcionName = strAniName;
            anim_param.fSpeed = 1;
            anim_param.nStartFrame = 0;
            anim_param.nLoop = -1;
            anim_param.fBlendTime = 0.2f;
            en.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
        }
    }
}
