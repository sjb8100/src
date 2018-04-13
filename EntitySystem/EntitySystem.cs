using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Client;
using table;
using UnityEngine.Profiling;

namespace EntitySystem
{
    class EntitySystem : IEntitySystem
    {
        public static IClientGlobal m_ClientGlobal = null;
        Camera m_mainCamera = null;
        Camera MainCamera
        {
            get
            {
                if (m_mainCamera == null)
                {
                    Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                    if (rs != null)
                    {
                        string strMainCamera = "MainCamera";
                        Engine.ICamera cam = rs.GetCamera(ref strMainCamera);
                        if (cam != null)
                        {
                            m_mainCamera = cam.GetNode().GetTransForm().GetComponent<Camera>();
                        }
                    }
                }
                return m_mainCamera;
            }
        }
        public bool bOnlyCreateMainPlayer = false;
        /**
        @brief 设置同屏人数
        @param 
        */
        public int MaxPlayer { get; set; }

        /// <summary>
        /// 用于判断视野范围
        /// </summary>
        public const int SQRMagnitude = 400;

        public uint serverTime
        {
            get
            {
                return EntityConfig.serverTime;
            }
        }

        /// <summary>
        /// 玩家搜索范围内（跟自动挂机打怪距离一样）
        /// </summary>
        uint SearchRange;

        public EntitySystem(IClientGlobal clientGlobal)
        {
            EntitySystem.m_ClientGlobal = clientGlobal;
            MaxPlayer = 500;  // 默认设置一个较大值
            bOnlyCreateMainPlayer = PlayerPrefs.GetInt("eOnlyCreatMainPlayer", 0) == 1 ? true : false;
            SearchRange = GameTableManager.Instance.GetGlobalConfig<uint>("Auto_Combat_Rang");
        }

        private void OnEvent(int nEventID, object param)
        {
            // 地表没有创建成功的时候，就已经收到了角色和NPC的创建　这里重新设置位置和地表做碰撞
            if (nEventID == (int)GameEventID.SYSTEM_LOADSCENECOMPELETE)
            {
                //List<IPlayer> lstPlayer;
                FindAllEntity<IPlayer>(ref lstPlayer);
                for (int i = 0; i < lstPlayer.Count; ++i)
                {
                    Vector3 pos = lstPlayer[i].GetPos();
                    lstPlayer[i].SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                }
                lstPlayer.Clear();

                //List<INPC> lstNPC;
                FindAllEntity<INPC>(ref lstNPC);
                for (int i = 0; i < lstNPC.Count; ++i)
                {
                    Vector3 pos = lstNPC[i].GetPos();
                    lstNPC[i].SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                }
                lstNPC.Clear();

                //List<IBox> lstBox;
                FindAllEntity<IBox>(ref lstBox);
                for (int i = 0; i < lstBox.Count; ++i)
                {
                    Vector3 pos = lstBox[i].GetPos();
                    pos.y += 0.2f;
                    lstBox[i].SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                }
                lstBox.Clear();

                //List<IRobot> lstRobot;
                FindAllEntity<IRobot>(ref lstRobot);
                for (int i = 0; i < lstRobot.Count; ++i)
                {
                    Vector3 pos = lstRobot[i].GetPos();
                    lstRobot[i].SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                }
                lstRobot.Clear();
                // 请求同步移动时间
                ReqMoveServerTime();
            }
            else if (nEventID == (int)GameEventID.RECONNECT_SUCESS) // 重新连接成功
            {
                stReconnectSucess st = (stReconnectSucess)param;
                if (st.isLogin)
                {
                    Clear(false); // 不清理除主角
                    // 请求同步移动时间
                    ReqMoveServerTime();
                }
                else
                {//强制同步
                    GameCmd.stUserMoveMoveUserCmd_C cmd = new GameCmd.stUserMoveMoveUserCmd_C();
                    cmd.client_time = 0;
                    List<GameCmd.Pos> list = new List<GameCmd.Pos>();
                    GameCmd.Pos pp = new GameCmd.Pos();
                    pp.x = 0;
                    pp.y = 0;
                    list.Add(pp);
                    cmd.poslist.AddRange(list);
                    m_ClientGlobal.netService.Send(cmd);
                }

            }
            else if (nEventID == (int)GameEventID.NETWORK_CONNECTE_CLOSE)// 网络断开
            {
                if (m_ClientGlobal.MainPlayer != null)
                {
                    m_ClientGlobal.MainPlayer.SendMessage(EntityMessage.EntityCommand_RemoveLinkAllEffect);
                }
            }
            else if (nEventID == (int)GameEventID.ENTITYSYSTEM_ENTITYDEAD) // 实体死亡
            {
                stEntityDead ed = (stEntityDead)param;
                if (m_ClientGlobal.IsMainPlayer(ed.uid))
                {
                    Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                    if (rs != null)
                    {
                        rs.EnableGray(true);
                    }
                }
                IEntity en = FindEntity(ed.uid);
                if (en != null)
                {
                    AddEffectOnEntity(en, false);
                }
            }
            else if (nEventID == (int)GameEventID.ENTITYSYSTEM_RELIVE)
            {
                stEntityRelive ed = (stEntityRelive)param;
                if (m_ClientGlobal.IsMainPlayer(ed.uid))
                {
                    Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                    if (rs != null)
                    {
                        rs.EnableGray(false);
                    }
                }

                IEntity en = FindEntity(ed.uid);
                if (en != null)
                {
                    AddEffectOnEntity(en, true);
                }
            }
            else if (nEventID == (int)GameEventID.PLAYER_LOGIN_SUCCESS)
            {
                Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
                if (rs != null)
                {
                    rs.EnableGray(false);
                }

                // 请求同步移动时间
                ReqMoveServerTime();
            }
            else if (nEventID == (int)GameEventID.ENTITYSYSTEM_LEAVEMAP) // 离开地图
            {
                // 清理地图上所有对象
                Clear(false);

                // 切地图时让主角停止移动
                IPlayer mainPlayer = EntitySystem.m_ClientGlobal.MainPlayer;
                if (mainPlayer != null)
                {
                    bool ismoving = (bool)mainPlayer.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
                    if (ismoving)
                    {
                        mainPlayer.SendMessage(Client.EntityMessage.EntityCommand_StopMove, mainPlayer.GetPos());
                    }
                }
            }
            else if (nEventID == (int)GameEventID.ENTITYSYSTEM_CHANGEAREA) // 区域变化 动作处理
            {
                stEntityChangeArea changeArea = (stEntityChangeArea)param;
                IEntity en = FindEntity(changeArea.uid);
                if (en != null && en.GetEntityType() == EntityType.EntityType_Player)
                {
                    string strCurAni = (string)en.SendMessage(EntityMessage.EntityCommand_GetCurAni, null);
                    bool bChangeArea = false;
                    if (changeArea.eType == MapAreaType.Safe || changeArea.eType == MapAreaType.Fish)
                    {
                        if (strCurAni == EntityAction.Stand_Combat)
                        {
                            strCurAni = EntityAction.Stand;
                            bChangeArea = true;
                        }

                        if (strCurAni == EntityAction.Run_Combat)
                        {
                            strCurAni = EntityAction.Run;
                            bChangeArea = true;
                        }
                    }
                    else
                    {
                        if (strCurAni == EntityAction.Stand)
                        {
                            strCurAni = EntityAction.Stand_Combat;
                            bChangeArea = true;
                        }

                        if (strCurAni == EntityAction.Run)
                        {
                            strCurAni = EntityAction.Run_Combat;
                            bChangeArea = true;
                        }
                    }

                    if (bChangeArea)
                    {
                        PlayAni anim_param = new PlayAni();
                        anim_param.strAcionName = strCurAni;
                        anim_param.fSpeed = 1;
                        anim_param.nStartFrame = 0;
                        anim_param.nLoop = -1;
                        anim_param.fBlendTime = 0.1f;
                        en.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
                    }
                }
            }
            else if (nEventID == (int)GameEventID.ENTITYSYSTEM_CREATEENTITY)
            {
                stCreateEntity create = (stCreateEntity)param;
                IEntity en = FindEntity(create.uid);
                if (en != null)
                {
                    EntityManager.Instance().OnCreateEntity(en);
                }
            }
        }

        // 投票处理
        private bool OnVote(int nEventID, object param)
        {
            if (nEventID == (int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE)
            {
                return !EntityConfig.m_bForceMove;
            }
            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        // 请求移动对时
        private void ReqMoveServerTime()
        {
            GameCmd.stTimeTickMoveUserCmd_C cmd = new GameCmd.stTimeTickMoveUserCmd_C
            {
                client_time = (uint)(Time.realtimeSinceStartup * 1000)
            };
            Engine.Utility.Log.LogGroup("XXF", "MoveTIck:{0} {1}", cmd.client_time, EntityConfig.serverTime);
            if (cmd != null && m_ClientGlobal.netService != null)
            {
                m_ClientGlobal.netService.SendCheckTime(cmd);
            }
        }

        public void Create()
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_RELIVE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.PLAYER_LOGIN_SUCCESS, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_CHANGEAREA, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.RECONNECT_SUCESS, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.NETWORK_CONNECTE_CLOSE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddVoteListener((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, OnVote);

            EntityViewCreator.Instance.Init();
            EntityPreLoadManager.Instance.InitPreLoadManager();
        }
        // 游戏退出时使用
        public void Release()
        {
            // 清理事件
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYDEAD, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_RELIVE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.PLAYER_LOGIN_SUCCESS, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_CHANGEAREA, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.RECONNECT_SUCESS, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.NETWORK_CONNECTE_CLOSE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveVoteListener((int)GameVoteEventID.ENTITYSYSTEM_VOTE_ENTITYMOVE, OnVote);

            // 清理所有资源
            Clear(true);
            EntityPreLoadManager.Instance.ClearPreLoadManager();
            EntityViewCreator.Instance.Close();
        }

        //-------------------------------------------------------------------------------------------------------
        // 创建实体
        /**
        @brief 创建实体
        @param etype 实体类型
        @param data 实体数据
        @param bImmediate 是否立即创建
        */
        public IEntity CreateEntity(EntityType etype, EntityCreateData data, bool bImmediate = false)
        {
            data.bImmediate = bImmediate;
            if (bOnlyCreateMainPlayer)
            {
                if (data.bMainPlayer)
                {
                    return EntityFactory.Instance().CreateEntity(etype, data);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return EntityFactory.Instance().CreateEntity(etype, data);
            }

        }

        // 删除实体
        public void RemoveEntity(IEntity entity)
        {
            EntityFactory.Instance().RemoveEntity(entity);
        }

        public void RemoveEntity(long uid)
        {
            EntityFactory.Instance().RemoveEntity(uid);
        }

        #region 查找实体方法
        // 各种查询方法
        public IPlayer FindPlayer(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Player, uid);
            return (IPlayer)EntityManager.Instance().GetEntity(id);
        }
        public IPet FindPet(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Pet, uid);
            return (IPet)EntityManager.Instance().GetEntity(id);
        }
        public IMonster FindMonster(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Monster, uid);
            return (IMonster)EntityManager.Instance().GetEntity(id);
        }

        public IPuppet FindPuppet(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Puppet, uid);
            return (IPuppet)EntityManager.Instance().GetEntity(id);
        }

        public INPC FindNPC(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_NPC, uid);
            return (INPC)EntityManager.Instance().GetEntity(id);
        }

        public IBox FindBox(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Box, uid);
            return (IBox)EntityManager.Instance().GetEntity(id);
        }

        public IRobot FindRobot(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityTYpe_Robot, uid);
            return (IRobot)EntityManager.Instance().GetEntity(id);
        }

        // NPC列表
        private List<INPC> allNpcList = new List<INPC>();
        private List<INPC> npcList = new List<INPC>();
        public INPC FindNPCByBaseId(int baseId, bool nearest = false)
        {
            allNpcList.Clear();
            npcList.Clear();

            EntityManager.Instance().FindAllEntity<INPC>(ref allNpcList);

            for (int i = 0; i < allNpcList.Count; ++i)
            {
                INPC npc = allNpcList[i];
                if (npc.GetProp((int)EntityProp.BaseID) == baseId && !npc.IsDead())
                {
                    if (!nearest)
                    {
                        return npc;
                    }
                    else
                    {
                        npcList.Add(npc);
                    }
                }
            }
            if (m_ClientGlobal.MainPlayer == null)
            {
                return null;
            }

            Vector3 pos = m_ClientGlobal.MainPlayer.GetPos();
            INPC getNpc = null;
            float fMinDis = 5000000.0f;
            for (int i = 0; i < npcList.Count; i++)
            {
                float sqrlen = (npcList[i].GetPos() - pos).sqrMagnitude;
                if (sqrlen < fMinDis)
                {
                    fMinDis = sqrlen;
                    getNpc = npcList[i];
                }
            }
            return getNpc;
        }

        public IItem FindItem(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Item, uid);
            return (IItem)EntityManager.Instance().GetEntity(id);
        }

        public IPlant FindPlant(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Plant, uid);
            return (IPlant)EntityManager.Instance().GetEntity(id);
        }

        public IAnimal FindAnimal(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Animal, uid);
            return (IAnimal)EntityManager.Instance().GetEntity(id);
        }

        public ITree FindTree(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Tree, uid);
            return (ITree)EntityManager.Instance().GetEntity(id);
        }

        public IMinerals FindMinerals(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Minerals, uid);
            return (IMinerals)EntityManager.Instance().GetEntity(id);
        }

        public ISoil FindSoil(uint uid)
        {
            long id = EntityFactory.Instance().MakeUID(EntityType.EntityType_Soil, uid);
            return (ISoil)EntityManager.Instance().GetEntity(id);
        }


        public IEntity FindEntity(long uid)
        {
            return EntityManager.Instance().GetEntity(uid);
        }

        /// <summary>
        /// 挂机寻怪接口
        /// </summary>
        /// <param name="center"></param>
        /// <param name="rang"></param>
        /// <param name="filterTarget"></param>
        /// <returns></returns>
        public IEntity FindEntityByArea_PkModel(MapAreaType areaType, PLAYERPKMODEL pkmodel, UnityEngine.Vector3 center, uint rang, uint attackPriority, long filterTarget = 0)
        {
            IEntity e = null;
            if (areaType == MapAreaType.Safe || pkmodel == PLAYERPKMODEL.PKMODE_M_NORMAL)
            {
                e = FindNearstEntity<INPC>(new MonsterCondition(center, rang, filterTarget));
            }
            else
            {
                //先砍人
                if (attackPriority == 1)
                {
                    e = FindNearstEntity<IPlayer>(new PlayerCondition(center, rang, filterTarget));
                    if (e == null)
                    {
                        //没人砍怪
                        e = FindNearstEntity<INPC>(new MonsterCondition(center, rang, filterTarget));
                    }

                    Engine.Utility.Log.Error("--->>> 优先人！！！");
                }

                else if (attackPriority == 2)
                {
                    //先砍怪
                    e = FindNearstEntity<INPC>(new MonsterCondition(center, rang, filterTarget));
                    if (e == null)
                    {
                        //没怪砍人
                        e = FindNearstEntity<IPlayer>(new PlayerCondition(center, rang, filterTarget));
                    }

                    Engine.Utility.Log.Error("--->>> 优先怪！！！");
                }

                else if (attackPriority == 3)
                {
                    //不限
                    e = FindNearstEntity<ICreature>(new CreatureCondition(center, rang, filterTarget));
                    Engine.Utility.Log.Error("--->>> 不限 ！！！");
                }

            }



            return e;
        }
        List<IPlayer> m_minHpPlayerList = new List<IPlayer>();
        public IEntity FindMinHpTeamer(uint range)
        {
            IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                return null;
            }
            IControllerHelper ch = cs.GetControllerHelper();
            if (ch == null)
            {
                return null;
            }
            m_minHpPlayerList.Clear();
            FindAllEntity<IPlayer>(ref m_minHpPlayerList);
            List<IPlayer> myTeamList = new List<IPlayer>();
            for (int i = 0; i < m_minHpPlayerList.Count; i++)
            {
                IPlayer player = m_minHpPlayerList[i];

                if (ch.IsSameTeam(player))
                {
                    IPlayer mainPlayer = m_ClientGlobal.MainPlayer;
                    if (mainPlayer != null)
                    {
                        ISkillPart sp = mainPlayer.GetPart(EntityPart.Skill) as ISkillPart;
                        if (sp != null)
                        {
                            int error = 0;
                            if (!sp.CheckCanAttackTarget(player, out error))
                            {
                                myTeamList.Add(player);
                            }
                        }
                    }
                }
            }
            IEntity en = null;
            for (int i = 0; i < myTeamList.Count; i++)
            {
                IPlayer player = myTeamList[i];
                if (en == null)
                {
                    en = player;
                }
                int maxHp = en.GetProp((int)CreatureProp.MaxHp);
                int curHP = en.GetProp((int)CreatureProp.Hp);
                float oldScale = (maxHp - curHP) * 1.0f / maxHp;
                int playermaxHp = player.GetProp((int)CreatureProp.MaxHp);
                int playercurHP = player.GetProp((int)CreatureProp.Hp);
                float newScale = (playermaxHp - playercurHP) * 1.0f / playermaxHp;
                if (newScale > oldScale)
                {
                    en = player;
                }


            }
            return en;

        }
        /// <summary>
        /// 查找一定范围的实体 通过阵营类型
        /// </summary>
        /// <param name="camtype">阵营</param>
        /// <param name="range">半径</param>
        /// <returns></returns>
        public IEntity FindEntityByCampType(CampType camtype, uint range)
        {
            if (camtype == CampType.Firendly)
            {
                INPC npc = FindNearstEntity<INPC>(new FriendlyNPCCondition(range));
                IPlayer player = FindNearstEntity<IPlayer>(new FriendlyPlayerCondition(range));
                return GetNearstEntity(npc, player);
            }
            if (camtype == CampType.Enemy)
            {
                INPC npc = FindNearstEntity<INPC>(new EnemyNPCCondition(range));
                IPlayer player = FindNearstEntity<IPlayer>(new EnemyPlayerCondition(range));
                IRobot robot = FindNearstEntity<IRobot>(new EnemyRobotCondition(range));
                IEntity en = GetNearstEntity(player, robot);
                return GetNearstEntity(npc, en);
            }
            if (camtype == CampType.FriendlyBody)
            {
                INPC npc = FindNearstEntity<INPC>(new FriendlyNPCBodyCondition(range));
                IPlayer player = FindNearstEntity<IPlayer>(new FriendlyPlayerBodyCondition(range));
                return GetNearstEntity(npc, player);
            }
            return null;
        }
        public IEntity FindTeamerByRange(uint range)
        {
            INPC npc = FindNearstEntity<INPC>(new TeamNPCCondition(range));
            IPlayer player = FindNearstEntity<IPlayer>(new TeamPlayerCondition(range));
            return GetNearstEntity(npc, player);
        }
        public IEntity FindSummonedByRange(uint range)
        {
            INPC npc = FindNearstEntity<INPC>(new SummonNPCCondition(range));

            return npc;
        }
        public IEntity FindPetByRange(uint range)
        {
            INPC npc = FindNearstEntity<INPC>(new PetNPCCondition(range));

            return npc;
        }
        public IEntity FindAllCreatureByRange(uint range)
        {
            IEntity en = FindNearstEntity<ICreature>(new AllCreatureCondition(range));
            return en;
        }
        IEntity GetNearstEntity(IEntity a, IEntity b)
        {
            IEntity mainPlayer = m_ClientGlobal.MainPlayer;
            if (mainPlayer == null)
            {
                return null;
            }
            if (a != null && b != null)
            {
                float disA = EntityHelper.GetEntityDistance(mainPlayer, a);
                float disB = EntityHelper.GetEntityDistance(mainPlayer, b);
                if (disA < disB)
                {
                    return a;
                }
                else
                {
                    return b;
                }
            }
            if (a == null)
            {
                return b;
            }
            if (b == null)
            {
                return a;
            }
            return null;
        }
        public T FindEntity<T>(uint uid)
        {
            Type t = typeof(T);
            if (t == typeof(IPlayer))
            {
                return (T)FindPlayer(uid);
            }
            else if (t == typeof(IMonster))
            {
                return (T)FindMonster(uid);
            }
            else if (t == typeof(INPC))
            {
                return (T)FindNPC(uid);
            }
            else if (t == typeof(IItem))
            {
                return (T)FindItem(uid);
            }
            else if (t == typeof(IPet))
            {
                return (T)FindPet(uid);
            }
            else if (t == typeof(IPlant))
            {
                return (T)FindPlant(uid);
            }
            else if (t == typeof(IAnimal))
            {
                return (T)FindAnimal(uid);
            }
            else if (t == typeof(ITree))
            {
                return (T)FindTree(uid);
            }
            else if (t == typeof(IMinerals))
            {
                return (T)FindMinerals(uid);
            }
            else if (t == typeof(ISoil))
            {
                return (T)FindSoil(uid);
            }
            else if (t == typeof(IPuppet))
            {
                return (T)FindPuppet(uid);
            }
            else if (t == typeof(IRobot))
            {
                return (T)FindRobot(uid);
            }
            return default(T);
        }

        public void FindAllEntity<T>(ref List<T> lst)
        {
            EntityManager.Instance().FindAllEntity<T>(ref lst);
        }

        public List<long> GetEntityUids()
        {
            return EntityManager.Instance().GetEntityUids();
        }

        // 查找与主角最近的对象
        public T FindNearstEntity<T>(IFindCondition<T> condition)
        {
            if (m_ClientGlobal == null)
            {
                return default(T);
            }
            if (m_ClientGlobal.MainPlayer == null)
            {
                return default(T);
            }

            Vector3 pos = m_ClientGlobal.MainPlayer.GetPos();

            List<T> lstEntity = new List<T>();
            FindAllEntity<T>(ref lstEntity);

            float fMinDis = 5000000.0f;
            T ret = default(T);
            for (int i = 0; i < lstEntity.Count; ++i)
            {
                if (lstEntity[i] == null)
                {
                    continue;
                }
                IEntity en = lstEntity[i] as IEntity;
                if (en != null)
                {
                    if (en.GetUID() == m_ClientGlobal.MainPlayer.GetUID())
                    {
                        continue;
                    }
                }
                if (condition != null)
                {
                    if (!condition.Conform(lstEntity[i]))
                    {
                        continue;
                    }
                }

                if (en != null)
                {
                    if (en.GetUID() != m_ClientGlobal.MainPlayer.GetUID())
                    {
                        float fDis = Vector3.SqrMagnitude(pos - en.GetPos());
                        if (fDis < fMinDis)
                        {
                            fMinDis = fDis;
                            ret = lstEntity[i];
                        }
                    }
                }
            }
            lstEntity.Clear();
            lstEntity = null;

            return ret;
        }

        // 查找最近的对象
        public T FindNearstEntity<T>()
        {
            if (m_ClientGlobal == null)
            {
                return default(T);
            }
            if (m_ClientGlobal.MainPlayer == null)
            {
                return default(T);
            }

            Vector3 pos = m_ClientGlobal.MainPlayer.GetPos();

            List<T> lstEntity = new List<T>();
            FindAllEntity<T>(ref lstEntity);

            float fMinDis = 1000000.0f;
            T ret = default(T);
            for (int i = 0; i < lstEntity.Count; ++i)
            {
                if (lstEntity[i] == null)
                {
                    continue;
                }

                IEntity en = lstEntity[i] as IEntity;
                if (en != null)
                {
                    if (en.GetUID() != m_ClientGlobal.MainPlayer.GetUID())
                    {
                        float fDis = Vector3.Distance(pos, en.GetPos());
                        if (fDis < fMinDis)
                        {
                            fMinDis = fDis;
                            ret = lstEntity[i];
                        }
                    }
                }
            }
            lstEntity.Clear();
            lstEntity = null;

            return ret;
        }

        /// <summary>
        /// 获得最近的player  list  由近到远(有判断视野)
        /// </summary>
        /// <param name="lstPlayer"></param>
        public void FindNearstPlayer(ref List<IPlayer> lstPlayer)
        {
            if (m_ClientGlobal == null)
            {
                return;
            }
            if (m_ClientGlobal.MainPlayer == null)
            {
                return;
            }

            lstPlayer.Clear();
            FindAllEntity<IPlayer>(ref lstPlayer);

            //检测视野(策划要求不做视野检测)
            //for (int i = lstPlayer.Count - 1; i >= 0; i--)
            //{
            //    if (false == CheckEntityVisible(lstPlayer[i]))
            //    {
            //        lstPlayer.Remove(lstPlayer[i]);
            //    }
            //}

            //检测是否在搜索范围内
            for (int i = lstPlayer.Count - 1; i >= 0; i--)
            {
                if (false == CheckSearchRange(lstPlayer[i]))
                {
                    lstPlayer.Remove(lstPlayer[i]);
                }
            }

            //由近到远排序
            lstPlayer.Sort(new PlayerDisCompare());
        }


        public void FindNearstEntity<T>(ref List<T> lstEntity)
        {
            if (m_ClientGlobal == null)
            {
                return;
            }
            if (m_ClientGlobal.MainPlayer == null)
            {
                return;
            }

            lstEntity.Clear();
            FindAllEntity<T>(ref lstEntity);

            lstEntity.Sort(new EntityDisCompare<T>());
        }



        class EntityDisCompare<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                Type t = typeof(T);
                if (t == typeof(IPlayer))
                {
                    IPlayer xPlayer = x as IPlayer;
                    IPlayer yPlayer = y as IPlayer;

                    float dis1 = GetFastDistance(xPlayer);
                    float dis2 = GetFastDistance(yPlayer);

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
                else if (t == typeof(IMonster))
                {
                    IMonster xMonster = x as IMonster;
                    IMonster yMonster = y as IMonster;

                    float dis1 = GetFastDistance(xMonster);
                    float dis2 = GetFastDistance(yMonster);

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


                return 0;
            }

            float GetFastDistance(IEntity en)
            {
                //IEntity en = EntityManager.Instance().GetEntity(id);
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

        class PlayerDisCompare : IComparer<IPlayer>
        {
            public int Compare(IPlayer x, IPlayer y)
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

            float GetFastDistance(IEntity en)
            {
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

        /// <summary>
        /// 检测是否在视野范围
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool CheckEntityVisible(IEntity e)
        {
            if (e != null)
            {
                if (EntitySystem.m_ClientGlobal.MainPlayer == null)
                {
                    return false;
                }

                Vector3 distance = EntitySystem.m_ClientGlobal.MainPlayer.GetPos() - e.GetPos();
                if (distance.sqrMagnitude > SQRMagnitude)
                {
                    return false;
                }
                //GetCamera();
                Engine.Node node = e.GetNode();
                if (node != null)
                {
                    Vector3 pos = MainCamera.WorldToViewportPoint(node.GetWorldPosition());
                    return (MainCamera.orthographic || pos.z > 0f) && (pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f);
                }
                else
                {
                    Engine.Utility.Log.Error("{0}node is null", e.GetObjName());
                }
            }
            return false;
        }

        /// <summary>
        /// 检测玩家是否在搜索范围内（跟自动挂机打怪距离一样）
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool CheckSearchRange(IEntity e)
        {
            if (e != null)
            {
                if (EntitySystem.m_ClientGlobal.MainPlayer == null)
                {
                    return false;
                }

                Vector3 distance = EntitySystem.m_ClientGlobal.MainPlayer.GetPos() - e.GetPos();
                if (distance.sqrMagnitude > SearchRange * SearchRange)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        Dictionary<Type, object> m_dicObj = new Dictionary<Type, object>();
        // 根据一定的范围查找对象 范围判定在IFindCondition里面实现
        public void FindEntityRange<T>(IFindCondition<T> condition, ref List<T> lst)
        {
            Profiler.BeginSample("FindEntityRange");
            lst.Clear();
            if (m_ClientGlobal == null)
            {
                return;
            }

            if (m_ClientGlobal.MainPlayer == null)
            {
                return;
            }
            Profiler.BeginSample("typeof");
            Type key = typeof(T);
            Profiler.EndSample();
            object lstObj = null;
            if (!m_dicObj.TryGetValue(key, out lstObj))
            {//代码优化 (有问题可以先还原)去掉每次3k的开销 modify by zhudianyu 
                lstObj = new List<T>(100);
                m_dicObj.Add(key, lstObj);
            }
            List<T> lstEntity = lstObj as List<T>;
            lstEntity.Clear();
            Profiler.BeginSample("FindAllEntity");
            FindAllEntity<T>(ref lstEntity);
            Profiler.EndSample();
            for (int i = 0; i < lstEntity.Count; ++i)
            {
                if (lstEntity[i] == null)
                {
                    continue;
                }
                IEntity en = lstEntity[i] as IEntity;
                if (en != null)
                {
                    if (en.GetUID() == m_ClientGlobal.MainPlayer.GetUID())
                    {
                        continue;
                    }
                }
                if (condition != null)
                {
                    Profiler.BeginSample("condition");
                    // 符合条件，加入输出列表
                    if (condition.Conform(lstEntity[i]))
                    {
                        lst.Add(lstEntity[i]);
                    }
                    Profiler.EndSample();
                }
            }
            lstEntity.Clear();
            // lstEntity = null;
            Profiler.EndSample();
        }

        // 显示实体
        public void ShowEntity(bool bShow)
        {
            EntityManager.Instance().ShowEntity(bShow);
        }

        #endregion

        #region 实体碰撞

        public struct stEntityColliderInfo
        {
            public IEntity en;
            public float dis;
        }

        // 比较接口
        public class EntityColliderComparer : IComparer<stEntityColliderInfo>
        {
            //实现按距离升序排列
            public int Compare(stEntityColliderInfo lhs, stEntityColliderInfo rhs)
            {
                if (lhs.dis > rhs.dis)
                    return 1;
                else if (lhs.dis == rhs.dis)
                    return 0;
                else
                    return -1;
            }
        }

        //-------------------------------------------------------------------------------------------------------
        // 拾取查询实体临时使用列表
        List<IPlayer> lstPlayer = new List<IPlayer>();
        List<INPC> lstNPC = new List<INPC>();
        List<IPlant> lstPlant = new List<IPlant>();
        List<IAnimal> lstAnimal = new List<IAnimal>();
        List<ITree> lstTree = new List<ITree>();
        List<IMinerals> lstMinerals = new List<IMinerals>();
        List<ISoil> lstSoil = new List<ISoil>();
        List<IPuppet> lstPuppet = new List<IPuppet>();
        List<IBox> lstBox = new List<IBox>();
        List<IRobot> lstRobot = new List<IRobot>();
        //-------------------------------------------------------------------------------------------------------
        private void PickupEntity<T>(Ray r, ref List<T> lstEntity, ref List<stEntityColliderInfo> enInfo) where T : IEntity
        {
            RaycastHit rayHit;
            FindAllEntity<T>(ref lstEntity);
            for (int i = 0; i < lstEntity.Count; ++i)
            {
                if (lstEntity[i].HitTest(r, out rayHit))
                {
                    stEntityColliderInfo ei = new stEntityColliderInfo();
                    ei.dis = rayHit.distance;
                    ei.en = lstEntity[i];
                    enInfo.Add(ei);
                }
            }
        }

        // 屏幕点选实体
        /**
        @brief 实体拾取方法 只查找生物类实体 按碰撞距离排序
        @param 根据屏幕上点，返回点中的实体列表
        */
        public bool PickupEntity(Vector2 pos, ref List<IEntity> lstEntity)
        {
            if (MainCamera == null || !MainCamera.enabled || lstEntity == null)
            {
                return false;
            }

            lstEntity.Clear();

            Ray r = MainCamera.ScreenPointToRay(new Vector3(pos.x, pos.y, 0));
            List<stEntityColliderInfo> enInfo = new List<stEntityColliderInfo>();
            // 根据r 碰撞实体 做类型筛选
            // player
            //List<IPlayer> lstPlayer = new List<IPlayer>();
            PickupEntity(r, ref lstPlayer, ref enInfo);
            lstPlayer.Clear();

            //List<INPC> lstNPC = new List<INPC>();
            PickupEntity(r, ref lstNPC, ref enInfo);
            lstNPC.Clear();

            //List<IPlant> lstPlant = new List<IPlant>();
            PickupEntity(r, ref lstPlant, ref enInfo);
            lstPlant.Clear();

            //List<IAnimal> lstAnimal = new List<IAnimal>();
            PickupEntity(r, ref lstAnimal, ref enInfo);
            lstAnimal.Clear();

            //List<ITree> lstTree = new List<ITree>();
            PickupEntity(r, ref lstTree, ref enInfo);
            lstTree.Clear();

            //List<IMinerals> lstMinerals = new List<IMinerals>();
            PickupEntity(r, ref lstMinerals, ref enInfo);
            lstMinerals.Clear();

            //List<ISoil> lstSoil = new List<ISoil>();
            PickupEntity(r, ref lstSoil, ref enInfo);
            lstSoil.Clear();

            //List<IPuppet> lstPuppet = new List<IPuppet>();
            PickupEntity(r, ref lstPuppet, ref enInfo);
            lstPuppet.Clear();

            //List<IBox> lstBox = new List<IBox>();
            PickupEntity(r, ref lstBox, ref enInfo);
            lstBox.Clear();

            //List<IRobot> lstRobot = new List<IRobot>();
            PickupEntity(r, ref lstRobot, ref enInfo);
            lstRobot.Clear();

            if (enInfo.Count == 0)
            {
                return false;
            }

            enInfo.Sort(new EntityColliderComparer());

            for (int i = 0; i < enInfo.Count; ++i)
            {
                lstEntity.Add(enInfo[i].en);
            }

            return true;
        }

        #endregion

        //系统更新
        public void Update(float dt)
        {
            EntityManager.Instance().Update(dt);

            // 根据同屏人数设置人物
            EntityManager.Instance().UpdateMaxPlayer(MaxPlayer);

            // 更新强制移动标识
            UpdateForceMove();
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 清理 退出时清理数据
        @param bFlag 标识是否清理主角 true清理主角
        */
        public void Clear(bool bFlag = false)
        {
            if (m_ClientGlobal == null)
            {
                return;
            }

            long[] ids = EntityManager.Instance().entityID;
            if (ids == null)
            {
                return;
            }

            if (m_ClientGlobal.MainPlayer == null)
            {
                return;
            }

            long mainid = m_ClientGlobal.MainPlayer.GetUID();
            for (int i = 0; i < ids.Length; ++i)
            {
                if (ids[i] == mainid && !bFlag)
                {
                    continue;
                }

                // 物品类不删除
                IEntity en = FindEntity(ids[i]);
                // EntityType_Pet 是宠物数据对象，只是自己的
                if (!bFlag && (en.GetEntityType() == EntityType.EntityType_Item || en.GetEntityType() == EntityType.EntityType_Puppet || en.GetEntityType() == EntityType.EntityType_Pet)) //忽略EntityType_Puppet因为RenderTextuerObj中有使用Puppet
                {
                    continue;
                }

                // 删除实体
                RemoveEntity(ids[i]);
            }

        }

        void AddEffectOnEntity(IEntity en, bool bBorn)
        {
            if (en == null)
            {
                return;
            }
            if (en.GetEntityType() == EntityType.EntityType_Player)
            {
                int job = en.GetProp((int)PlayerProp.Job);
                int sex = en.GetProp((int)PlayerProp.Sex);
                SelectRoleDataBase sdb = table.SelectRoleDataBase.Where((GameCmd.enumProfession)job, (GameCmd.enmCharSex)sex);
                if (sdb != null)
                {
                    if (bBorn)
                    {
                        AddEffect(en, sdb.borneffect);
                    }
                    else
                    {
                        AddEffect(en, sdb.deadeffect);
                    }

                }
            }
            else if (en.GetEntityType() == EntityType.EntityType_NPC
                || en.GetEntityType() == EntityType.EntityType_Monster)
            {
                int modelID = en.GetProp((int)EntityProp.BaseID);
                table.NpcDataBase ndb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)modelID);
                if (ndb != null)
                {
                    uint effectID = ndb.strBirthMagic;

                    if (bBorn)
                    {
                        AddEffect(en, ndb.strBirthMagic);
                    }
                    else
                    {
                        AddEffect(en, ndb.strDeathMagic);
                    }
                }
            }
        }
        void AddEffect(IEntity target, uint effectID)
        {
            FxResDataBase edb = GameTableManager.Instance.GetTableItem<FxResDataBase>(effectID);
            if (edb != null && target != null)
            {
                AddLinkEffect node = new AddLinkEffect();
                node.nFollowType = (int)edb.flowType;
                node.rotate = new Vector3(edb.rotate[0], edb.rotate[1], edb.rotate[2]);
                node.vOffset = new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]);

                // 使用资源配置表
                ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<ResourceDataBase>(edb.resPath);
                if (resDB == null)
                {
                    Engine.Utility.Log.Error("EffectViewFactory:找不到特效资源路径配置{0}", edb.resPath);
                    return;
                }
                node.strEffectName = resDB.strPath;
                node.strLinkName = edb.attachNode;
                if (edb.bFollowRole)
                {
                    if (node.strEffectName.Length != 0)
                    {
                        target.SendMessage(EntityMessage.EntityCommand_AddLinkEffect, node);

                    }
                }
            }
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void UpdateForceMove()
        {
            float fCurTime = Time.realtimeSinceStartup;
            if (fCurTime - EntityConfig.m_fStartTime > 1.5f && EntityConfig.m_bForceMove) // 1.5s后解除
            {
                EntityConfig.m_bForceMove = false;
            }
        }
    }

    // 实体系统创建
    public class EntitySystemCreator
    {
        private static EntitySystem m_EntitySys = null;
        public static IEntitySystem CreateEntitySystem(IClientGlobal clientGlobal)
        {
            if (m_EntitySys == null)
            {
                m_EntitySys = new EntitySystem(clientGlobal);
            }

            return m_EntitySys;
        }
    }


}
