using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using table;
using UnityEngine.Profiling;

namespace Client
{
    // 实体创建器（网络消息）
    class EntityCreator //: Engine.Utility.ITimer
    {
        static EntityCreator s_Inst = null;
        public static EntityCreator Instance()
        {
            if (null == s_Inst)
            {
                s_Inst = new EntityCreator();
            }

            return s_Inst;
        }

        PostRenderHeatDistortionEffect postHeatDistortionEffect = null;

        class stUserData
        {
            public t_MapUserData data;
            public bool bNoPos;
            public Vector3 pos;
            public uint dir;
        }

        // t_MapUserData tempID为key
        private Dictionary<uint, stUserData> m_dicUserData = new Dictionary<uint, stUserData>();
        private ObjPool<stUserData> m_UserDataPool = new ObjPool<stUserData>();

        class stNPCData
        {
            public uint master_id;
            public t_MapNpcDataPos data;
        }
        private Dictionary<uint, stNPCData> m_dicNPCData = new Dictionary<uint, stNPCData>();
        private ObjPool<stNPCData> m_NPCDataPool = new ObjPool<stNPCData>();

        class stPetData
        {
            public PetData data;
        }
        Dictionary<uint, stPetData> m_dicPetData = new Dictionary<uint, stPetData>();
        private ObjPool<stPetData> m_PetDataPool = new ObjPool<stPetData>();


        // 是否延迟加载
        private bool m_bDelayLoad = true;  // 

        public void Init()
        {
          
            //Engine.Utility.TimerAxis.Instance().SetTimer(0, 300, this, Engine.Utility.TimerAxis.INFINITY_CALL, "EntityCreator::Init");
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnLoadSceneCompelete);
        }

        public void Destroy()
        {
            //Engine.Utility.TimerAxis.Instance().KillTimer(0, this);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, OnLoadSceneCompelete);

            m_UserDataPool.Clear();
            m_NPCDataPool.Clear();
            m_PetDataPool.Clear();
        }


        void OnLoadSceneCompelete(int nEventID, object param)
        {
            // 设置镜头
            SetupMainCamera(ClientGlobal.Instance().MainPlayer);

            if (GameObject.Find("DirectionalLight1") == null)
            {
                // 灯光
                GameObject pObject = new GameObject();
                pObject.name = "DirectionalLight1";
                pObject.transform.position = new Vector3(57.74f, 35.87f, -53);

                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(30f, 145f, 0f);
                pObject.transform.rotation = rot;

                Light pLight = pObject.AddComponent<Light>();

                pLight.shadows = LightShadows.None;
                pLight.type = LightType.Directional;
                pLight.color = new Color(1, 1, 1, 1);


                pLight.intensity = 0.75f;
                if (Application.platform == RuntimePlatform.Android)
                {
                    //pLight.intensity = 0.75f / 2f;
                }

                pLight.bounceIntensity = 1;
                pLight.cullingMask = (1 << LayerMask.NameToLayer("ShowModel"));

                //阴影
                pLight.shadowStrength = 0.39f;

                pLight.shadowBias = 0.05f;
                pLight.shadowNormalBias = 0.4f;
                pLight.shadowNearPlane = 0.2f;

                pLight.cookieSize = 10;
                pLight.renderMode = LightRenderMode.Auto;

            }

            if (GameObject.Find("DirectionalLight2") == null)
            {
                // 灯光
                GameObject pObjectLight2 = new GameObject();
                pObjectLight2.name = "DirectionalLight2";
                pObjectLight2.transform.position = new Vector3(57.74f, 35.87f, -53);

                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(15f, -95f, 0f);
                pObjectLight2.transform.rotation = rot;

                Light pLight = pObjectLight2.AddComponent<Light>();

                pLight.shadows = LightShadows.None;
                pLight.type = LightType.Directional;
                pLight.color = new Color(74 / 255f, 99f / 255f, 180 / 255f, 1);

                pLight.intensity = 0.75f;
                if (Application.platform == RuntimePlatform.Android)
                {
                    //pLight.intensity = 0.75f / 2f;
                }

                pLight.bounceIntensity = 1f;
                pLight.cullingMask = (1 << LayerMask.NameToLayer("ShowModel"));

                //阴影
                pLight.shadowStrength = 0.39f;

                pLight.shadowBias = 0.05f;
                pLight.shadowNormalBias = 0.4f;
                pLight.shadowNearPlane = 0.2f;

                pLight.cookieSize = 10;
                pLight.renderMode = LightRenderMode.Auto;
            }

            if (GameObject.Find("DirectionalLight3") == null)
            {
                // 灯光
                GameObject pObjectLight3 = new GameObject();
                pObjectLight3.name = "DirectionalLight3";
                pObjectLight3.transform.position = new Vector3(57.74f, 35.87f, -53);

                Quaternion rot = new Quaternion();
                rot.eulerAngles = new Vector3(20f, 10f, 0f);
                pObjectLight3.transform.rotation = rot;

                Light pLight = pObjectLight3.AddComponent<Light>();

                pLight.shadows = LightShadows.Hard;
                pLight.type = LightType.Directional;
                pLight.color = new Color(1, 223f / 255f, 201f / 255f, 1);
                pLight.intensity = 0.85f;
                if (Application.platform == RuntimePlatform.Android)
                {
                    //pLight.intensity = 0.85f / 2f;
                }
                pLight.bounceIntensity = 1;
                pLight.cullingMask = (1 << LayerMask.NameToLayer("ShowModel"));

                //阴影
                pLight.shadowStrength = 0.5f;

                pLight.shadowBias = 0.05f;
                pLight.shadowNormalBias = 0.4f;
                pLight.shadowNearPlane = 0.2f;

                pLight.cookieSize = 10;
                pLight.renderMode = LightRenderMode.Auto;
            }

            IMapSystem mapSys = ClientGlobal.Instance().GetMapSystem();
            if (mapSys != null)
            {
                uint uMapID = mapSys.GetMapID();
                table.MapDataBase mapDB = GameTableManager.Instance.GetTableItem<table.MapDataBase>(uMapID);
                if (mapDB == null)
                {
                    Engine.Utility.Log.Error("MapSystem:找不到地图配置数据{0}", uMapID);
                    return;
                }

                if (mapDB.WaveEffect == 1)
                {
                    Camera cam = Camera.main;
                    if (cam != null)
                    {
                        postHeatDistortionEffect = cam.GetComponent<PostRenderHeatDistortionEffect>();
                        if (postHeatDistortionEffect == null)
                        {
                            postHeatDistortionEffect = cam.transform.gameObject.AddComponent<PostRenderHeatDistortionEffect>();
                        }
                        else
                        {
                            postHeatDistortionEffect.enabled = true;
                        }
                    }
                }
                else
                {
                    Camera cam = Camera.main;
                    if (cam != null)
                    {
                        postHeatDistortionEffect = cam.GetComponent<PostRenderHeatDistortionEffect>();
                        if (postHeatDistortionEffect != null)
                        {
                            postHeatDistortionEffect.enabled = false;
                        }
                    }
                }
            }

        }


        public void EnableHeatDistortionEffect(bool bEnable)
        {
            if (postHeatDistortionEffect)
            {
                postHeatDistortionEffect.enabled = bEnable;
            }
        }

        public void AddPlayer(t_MapUserData data, Vector3 pos, uint dir, List<GameCmd.Pos> lstPos, bool bHavePos = false)
        {
            if (data == null)
            {
                return;
            }

            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }

            IPlayer player = null;
            if (es.FindPlayer(data.userdata.dwUserID) != null) // 更新数据
            {
                player = CreatePlayer(data);
                if (player != null && bHavePos)
                {
                    player.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                    //Vector3 rot = GameUtil.S2CDirection(dir);
                    //player.SendMessage(EntityMessage.EntityCommand_SetRotate, (object)rot);
                }
                return;
            }

            // 主角立即创建
            if (UserData.IsMainRoleID(data.userdata.dwUserID))
            {
                player = CreatePlayer(data);
                if (player != null && bHavePos)
                {
                    player.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                    //Vector3 rot = GameUtil.S2CDirection(dir);
                    //player.SendMessage(EntityMessage.EntityCommand_SetRotate, (object)rot);
                }
                return;
            }

            player = CreatePlayer(data);
            if (player != null && bHavePos)
            {
                player.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                Vector3 rot = GameUtil.S2CDirection(dir);
                player.SendMessage(EntityMessage.EntityCommand_SetRotate, (object)rot);
                //m_UserDataPool.Free(iter.Current.Value);
                // m_dicUserData.Remove(iter.Current.Value.data.userdata.dwUserID);
                // break;

                if (lstPos != null)
                {
                    Move move = new Move();
                    move.m_speed = player.GetProp((int)WorldObjProp.MoveSpeed) * EntityConst.MOVE_SPEED_RATE; // 速度为测试速度
                    move.strRunAct = Client.EntityAction.Run;
                    move.path = new List<Vector3>();
                    // 添加起点
                    //move.path.Add(new Vector3(cmd.begin_pos_x * 0.01f, 0, -cmd.begin_pos_y * 0.01f));
                    for (int i = 0; i < lstPos.Count; ++i)
                    {
                        //if (i > 0)
                        //{
                        //    move.path.Add(new Vector3(cmd.poslist[i].x, 0, -cmd.poslist[i].y));
                        //}
                        //else
                        {
                            move.path.Add(new Vector3(lstPos[i].x * 0.01f, 0, -lstPos[i].y * 0.01f));
                        }
                    }
                    player.SendMessage(EntityMessage.EntityCommand_MovePath, (object)move);
                }
            }
        }

        public void RemovePlayer(uint uUserID)
        {
            m_dicUserData.Remove(uUserID);
        }

        public void AddNPC(t_MapNpcDataPos data, uint master_id)
        {
            Profiler.BeginSample("AddNpc");
            if (data == null)
            {
                return;
            }

            Engine.Utility.Log.Trace("添加NPC:{0}", data.mapnpcdata.npcdata.dwBaseID);

            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }

            table.NpcDataBase npctable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)data.mapnpcdata.npcdata.dwBaseID);
            if (npctable == null)
            {
                Engine.Utility.Log.Error("严重错误：not fount npc baseid {0}", data.mapnpcdata.npcdata.dwBaseID);
                return;
            }

            if (npctable.dwType == (uint)GameCmd.enumNpcType.NPC_TYPE_ROBOT)
            {

                if (es.FindRobot(data.mapnpcdata.npcdata.dwTempID) != null)
                {
                    CreateRobot(data, npctable.job); // 更新数据
                    return;
                }

                if (m_dicNPCData.ContainsKey(data.mapnpcdata.npcdata.dwTempID))
                {
                    return;
                }

                CreateRobot(data, npctable.job);
            }
            else
            {
                if (es.FindNPC(data.mapnpcdata.npcdata.dwTempID) != null)
                {
                    CreateNPC(data, master_id); // 更新数据
                    return;
                }

                if (m_dicNPCData.ContainsKey(data.mapnpcdata.npcdata.dwTempID))
                {
                    return;
                }

                CreateNPC(data, master_id);
            }
            Profiler.EndSample();
        }

        public void AddRobot(t_MapNpcDataPos data, uint master_id)
        {
            if (data == null)
            {
                return;
            }

            Engine.Utility.Log.Trace("添加ROBOT:{0}", data.mapnpcdata.npcdata.dwBaseID);

            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }

            if (es.FindRobot(data.mapnpcdata.npcdata.dwTempID) != null)
            {
                CreateNPC(data, master_id); // 更新数据
                return;
            }

            if (m_dicNPCData.ContainsKey(data.mapnpcdata.npcdata.dwTempID))
            {
                return;
            }

            CreateNPC(data, master_id);
        }

        public void RemoveNPC(uint uNPCID)
        {
            m_dicNPCData.Remove(uNPCID);
        }

        public void AddPet(PetData petdata)
        {
            if (petdata == null)
            {
                return;
            }

            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }

            if (es.FindPet(petdata.id) == null)
            {
                AddPetEntity(petdata);
                return;
            }

        }
        IPet AddPetEntity(PetData petdata)
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return null;
            }
            EntityCreateData entityData = new EntityCreateData();
            entityData.ID = petdata.id;
            entityData.PropList = new EntityAttr[(int)PetProp.End - (int)EntityProp.Begin];
            RoleUtil.BuildPetPropListByPetData(petdata, ref entityData.PropList);
            IPet pet = es.CreateEntity(EntityType.EntityType_Pet, entityData) as IPet;
            if (pet != null)
            {
                pet.SetExtraData(petdata);
                return pet;
            }

            return null;
        }

        public IBox AddBox(t_MapObjectData BoxData, uint nlefttime)
        {
            if (BoxData == null)
            {
                return null;
            }
            //Engine.Utility.Log.Info("创建box{0}", BoxData.dwObjectID);
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return null;
            }

//            if (es.FindBox(BoxData.qwThisID) == null)
            {
                //AddPetEntity(petdata);
                // 创建box
                //MapVector2 mapPos = MapVector2.FromCoordinate(BoxData.x, BoxData.y);
                Vector3 pos = new Vector3(BoxData.cur_pos.x * 0.01f, 0, -BoxData.cur_pos.y * 0.01f); // 服务器到客户端坐标转换

                EntityCreateData data = RoleUtil.BuildCreateEntityData(EntityType.EntityType_Box, BoxData);
                IBox box = es.FindBox(BoxData.qwThisID);
                if (box != null)
                {
                    box.UpdateProp(data);
                }
                else
                {
                    box = es.CreateEntity(EntityType.EntityType_Box, data, true) as IBox;


                }

                box.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                box.AddTrigger(new BoxOnTrigger());
                return box;
            }
            return null;
        }

        public void RemoveBox(uint id)
        {
            return;
        }

        // 定时器去处理实体创建
        public void OnTimer(uint uTimerID)
        {
            //Profiler.BeginSample("EntityCreator:OnTimer");
            // 怪物优先级高于角色
            if (m_dicNPCData.Count > 0)
            {
                Dictionary<uint, stNPCData>.Enumerator iter = m_dicNPCData.GetEnumerator();
                while (iter.MoveNext())
                {
                    CreateNPC(iter.Current.Value.data, iter.Current.Value.master_id);
                    m_NPCDataPool.Free(iter.Current.Value);
                    m_dicNPCData.Remove(iter.Current.Value.data.mapnpcdata.npcdata.dwTempID);
                    break;
                }
            }
            else
            {
                Dictionary<uint, stUserData>.Enumerator iter = m_dicUserData.GetEnumerator();
                while (iter.MoveNext())
                {
                    IPlayer player = CreatePlayer(iter.Current.Value.data);
                    if (player != null && !iter.Current.Value.bNoPos)
                    {
                        player.SendMessage(EntityMessage.EntityCommand_SetPos, (object)iter.Current.Value.pos);
                        Vector3 rot = GameUtil.S2CDirection(iter.Current.Value.dir);
                        player.SendMessage(EntityMessage.EntityCommand_SetRotate, (object)rot);
                        m_UserDataPool.Free(iter.Current.Value);
                        m_dicUserData.Remove(iter.Current.Value.data.userdata.dwUserID);
                        break;
                    }
                }
            }
            //Profiler.EndSample();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        private Client.IPlayer CreatePlayer(GameCmd.t_MapUserData userData)
        {
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return null;
            }

            EntityCreateData data = RoleUtil.BuildCreateEntityData(EntityType.EntityType_Player, userData);
            IPlayer player = es.FindPlayer(userData.userdata.dwUserID);
            if (player != null)
            {
                uint clanIdLow = (uint)player.GetProp((int)CreatureProp.ClanIdLow);
                uint clanIdHigh = (uint)player.GetProp((int)CreatureProp.ClanIdHigh);
                uint clanid = (clanIdHigh << 16) | clanIdLow;

                //
                //uint clanid = (uint)player.GetProp((int)CreatureProp.ClanId);


                //玩家死亡 服务器会通过这里刷新 所以这里更新下玩家血条
                if (userData.userdata.curhp <= 0)
                {
                    stPropUpdate prop = new stPropUpdate();
                    prop.uid = player.GetUID();
                    prop.nPropIndex = (int)CreatureProp.Hp;
                    prop.oldValue = player.GetProp((int)CreatureProp.Hp);

                    player.UpdateProp(data);

                    prop.newValue = player.GetProp((int)CreatureProp.Hp);

                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_HPUPDATE, prop);
                }
                else
                {
                    stPropUpdate prop = new stPropUpdate();
                    prop.uid = player.GetUID();
                    prop.nPropIndex = (int)CreatureProp.Hp;
                    prop.oldValue = player.GetProp((int)CreatureProp.Hp);

                    player.UpdateProp(data);

                    prop.newValue = player.GetProp((int)CreatureProp.Hp);
                    if (prop.oldValue < prop.newValue)
                    {
                        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_HPUPDATE, prop);
                    }
                }

                if (userData.userdata.clan_id != clanid)
                {
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CLANREFRESHID,
                        new Client.stClanUpdate() { uid = player.GetUID(), clanId = userData.userdata.clan_id });
                }
            }
            else
            {
                if (UserData.IsMainRoleID(userData.userdata.dwUserID))
                {
                    data.nLayer = LayerMask.NameToLayer("MainPlayer");
                    data.bMainPlayer = true; // 设置主角标识
                    player = es.CreateEntity(EntityType.EntityType_Player, data, true) as IPlayer;
                    if (player == null)
                    {
                        Engine.Utility.Log.Error("CreatePlayer:创建角色失败!");
                        return null;
                    }
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_MAINPLAYERCREATE,
                        new Client.stCreateEntity() { uid = userData.userdata.dwUserID });

                    // 设置控制对象
                    Client.IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
                    if (cs != null)
                    {
                        cs.GetActiveCtrl().SetHost(player);
                    }

                    // 补丁 服务器没有金钱没有放在角色属性上
                    {
                        player.SetProp((int)PlayerProp.Money, (int)UserData.Money);
                        player.SetProp((int)PlayerProp.Coupon, (int)UserData.Coupon);
                        player.SetProp((int)PlayerProp.Cold, (int)UserData.Cold);
                        player.SetProp((int)PlayerProp.Reputation, (int)UserData.Reputation);
                        player.SetProp((int)PlayerProp.Score, (int)UserData.Score);
                        player.SetProp((int)PlayerProp.CampCoin, (int)UserData.CampCoin);
                        player.SetProp((int)PlayerProp.AchievePoint, (int)UserData.AchievePoint);
                        player.SetProp((int)PlayerProp.ShouLieScore, (int)UserData.ShouLieScore);
                        player.SetProp((int)PlayerProp.FishingMoney, (int)UserData.FishingMoney);
                        player.SetProp((int)PlayerProp.YinLiang, (int)UserData.YinLiang);
                    }

                    // 设置主像机参数
                    SetupMainCamera(player);

                    //玩家成功登陆回调
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, null);

                    //Client.stCreateEntity createEntity = new Client.stCreateEntity();
                    //createEntity.uid = player.GetUID();
                    //Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CREATEENTITY, createEntity);
                    //set hp
                    DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eSetRoleProperty, player);

                    //预加载资源
                  //  PreLoadResource(player.GetProp((int)PlayerProp.Job));
                }
                else
                {
                    //Profiler.BeginSample("CreatePlayer");
                    player = es.CreateEntity(EntityType.EntityType_Player, data, !m_bDelayLoad) as IPlayer;
                    //Profiler.EndSample();
                    Protocol.Instance.RequestName(userData.userdata.dwUserID);
                }

                if (player != null)
                {
                    PlayAni anim_param = new PlayAni();
                    anim_param.strAcionName = EntityAction.Stand;
                    anim_param.fSpeed = 1;
                    anim_param.nStartFrame = 0;
                    anim_param.nLoop = -1;
                    anim_param.fBlendTime = 0.2f;
                    player.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
                }
            }
            IBuffPart buffpart = player.GetPart(EntityPart.Buff) as IBuffPart;
            if(buffpart != null)
            {
                buffpart.ReceiveBuffList(userData.statelist.data);
            }
            return player;
        }

        private void CreateRobot(GameCmd.t_MapNpcDataPos npcdata,uint job)
        {
            Profiler.BeginSample("CreateRobot");
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }

            Vector3 pos = new Vector3(npcdata.cur_pos.x * 0.01f, 0, -npcdata.cur_pos.y * 0.01f); // 服务器到客户端坐标转换

            npcdata.mapnpcdata.npcdata.job = job;

            EntityCreateData data = RoleUtil.BuildCreateEntityData(EntityType.EntityTYpe_Robot, npcdata,0);
            IRobot robot = es.FindRobot(npcdata.mapnpcdata.npcdata.dwTempID);
            if (robot != null)
            {
                robot.UpdateProp(data);
            }
            else
            {
                robot = es.CreateEntity(EntityType.EntityTYpe_Robot, data, !m_bDelayLoad) as IRobot;
                // 发送事件 CreateEntity
                if (robot != null)
                {
                    PlayAni anim_param = new PlayAni();
                    anim_param.strAcionName = EntityAction.Stand;
                    anim_param.fSpeed = 1;
                    anim_param.nStartFrame = 0;
                    anim_param.nLoop = -1;
                    anim_param.fBlendTime = 0.2f;
                    robot.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);

                    if (!string.IsNullOrEmpty(npcdata.mapnpcdata.npcdata.name) && !Application.isEditor)
                    {
                        robot.SendMessage(EntityMessage.EntityCommond_SetName, npcdata.mapnpcdata.npcdata.name);
                    }
                    else
                    {
                        string strName = string.Format("{0}(AI)", npcdata.mapnpcdata.npcdata.name);
                        robot.SendMessage(EntityMessage.EntityCommond_SetName, npcdata.mapnpcdata.npcdata.name);
                    }
                }
            }

            Engine.Utility.Log.Info("创建机器人 {0} pos {1}",robot.GetID(), pos);

            robot.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
            Vector3 rot = GameUtil.S2CDirection(npcdata.mapnpcdata.npcdata.byDirect);
            robot.SendMessage(EntityMessage.EntityCommand_SetRotate, (object)rot);
            Profiler.EndSample();
        }
        //------------------------------------------------------------------------------------------------

        private void CreateNPC(GameCmd.t_MapNpcDataPos npcdata, uint master_id)
        {
            Profiler.BeginSample("CreateNPC");
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("严重错误：EntitySystem is null!");
                return;
            }
            
            //MapVector2 mapPos = MapVector2.FromCoordinate(npcdata.cur_pos.x * 0.01f, npcdata.cur_pos*0.01f);
            Vector3 pos = new Vector3(npcdata.cur_pos.x * 0.01f, 0, -npcdata.cur_pos.y * 0.01f); // 服务器到客户端坐标转换
            

            EntityCreateData data = RoleUtil.BuildCreateEntityData(EntityType.EntityType_NPC, npcdata, master_id);
            INPC npc = es.FindNPC(npcdata.mapnpcdata.npcdata.dwTempID);
            if (npc != null)
            {
                npc.UpdateProp(data);
                NpcAscription.Instance.UpdateBelong(npc, npcdata.mapnpcdata.npcdata.owernuserid, npcdata.mapnpcdata.npcdata.owernteamid, npcdata.mapnpcdata.npcdata.owernclanid, npcdata.mapnpcdata.npcdata.ownername);
            }
            else
            {
                npc = es.CreateEntity(EntityType.EntityType_NPC, data, !m_bDelayLoad) as INPC;
                // 发送事件 CreateEntity
                if (npc != null)
                {
                    NpcAscription.Instance.UpdateBelong(npc, npcdata.mapnpcdata.npcdata.owernuserid, npcdata.mapnpcdata.npcdata.owernteamid, npcdata.mapnpcdata.npcdata.owernclanid, npcdata.mapnpcdata.npcdata.ownername);
                    table.NpcDataBase npctable = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)npcdata.mapnpcdata.npcdata.dwBaseID);
                    if (npctable != null)
                    {
                        //Debug.LogError("********** npctable.dwID = " + npctable.dwID);
                        if (npcdata.mapnpcdata.npcdata.arenanpctype == ArenaNpcType.ArenaNpcType_Player)//离线的真实玩家名字，用服务器的
                        {
                            npc.SendMessage(EntityMessage.EntityCommond_SetName, npcdata.mapnpcdata.npcdata.name);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(npcdata.mapnpcdata.npcdata.name))
                            {
                                npc.SendMessage(EntityMessage.EntityCommond_SetName, npctable.strName);
                            }
                        }
                        if (npctable.dwType == (uint)GameCmd.enumNpcType.NPC_TYPE_TRANSFER)//传送阵
                        {
                            EntityOnTrigger callback = new EntityOnTrigger();
                            npc.SetCallback(callback);
                        }
                    }
                    if (npc != null)
                    {
                        PlayAni anim_param = new PlayAni();
                        anim_param.strAcionName = EntityAction.Stand;
                        anim_param.fSpeed = 1;
                        anim_param.nStartFrame = 0;
                        anim_param.nLoop = -1;
                        anim_param.fBlendTime = 0.2f;
                        npc.SendMessage(EntityMessage.EntityCommand_PlayAni, anim_param);
                    }
                }
            }
            if(npc != null)
            {
                npc.SendMessage(EntityMessage.EntityCommand_SetPos, (object)pos);
                Vector3 rot = GameUtil.S2CDirection(npcdata.mapnpcdata.npcdata.byDirect);
                npc.SendMessage(EntityMessage.EntityCommand_SetRotate, (object)rot);
                IBuffPart buffpart = npc.GetPart(EntityPart.Buff) as IBuffPart;
                if(buffpart != null)
                {
                    buffpart.ReceiveBuffList(npcdata.mapnpcdata.statelist.data);
                }
            }
          
            Profiler.EndSample();
        }


        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 设置MainCamera
        @param 
        */
        public bool isFarDisCamera = false;
        public void SetupMainCamera(IPlayer player)
        {
            float fRotateX = 38.0f;
            float fRotateY = 45.0f;
            table.MapDataBase mapdb = null;
            IMapSystem mapSys = Client.ClientGlobal.Instance().GetMapSystem();
            if (mapSys != null)
            {
                mapdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(mapSys.GetMapID());
                if (mapdb != null)
                {
                    fRotateX = mapdb.RotateX;
                    fRotateY = mapdb.RotateY;
                }
            }

            // 发送事件 CreateEntity
            string strCameraName = "MainCamera";
            Engine.ICamera cam = Engine.RareEngine.Instance().GetRenderSystem().GetCamera(ref strCameraName);
            if (cam != null)
            {

                CameraFollow.Instance.camera = cam;
                CameraFollow.Instance.target = player;
                CameraFollow.Instance.SetCameraOffset(fRotateX, fRotateY, isFarDisCamera ? mapdb.CamDis : mapdb.farDistance);
                cam.SetCameraCtrl(CameraFollow.Instance);


            }

            if (cam != null)
            {
                if (cam.GetNode().GetTransForm().gameObject.GetComponent<PostRenderOcclusionEffect>() == null)
                {
                    //PostRenderOcclusionEffect pEffect = cam.GetNode().GetTransForm().gameObject.AddComponent<PostRenderOcclusionEffect>();
                    //SeeThroughSystem pEffect = cam.GetNode().GetTransForm().gameObject.AddComponent<SeeThroughSystem>();
                }


                if (cam.GetNode().GetTransForm().gameObject.GetComponent<PostRenderOutlineEffect>() == null)
                {
                    //PostRenderOutlineEffect pEffect = cam.GetNode().GetTransForm().gameObject.AddComponent<PostRenderOutlineEffect>();
                    //SeeThroughSystem pEffect = cam.GetNode().GetTransForm().gameObject.AddComponent<SeeThroughSystem>();
                }

            }



            // 设置场景方向光参数 需要根据Camera来
            Shader.SetGlobalColor("g_LightColor", Color.white);
            // 设置方向
            Shader.SetGlobalVector("g_LightDirection", -GetMainCamDir());
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 获取主Camera方向
        @param 
        */
        Vector3 GetMainCamDir()
        {
            if (Camera.main != null)
            {
                Matrix4x4 mat = new Matrix4x4();
                mat.SetTRS(Vector3.zero, Camera.main.transform.rotation, Vector3.one);
                return mat.GetColumn(2); // 镜头方向
            }

            return Vector3.forward;
        }
        public void SetCameraDistance()
        {
            table.MapDataBase mapdb = null;
            IMapSystem mapSys = Client.ClientGlobal.Instance().GetMapSystem();
            if (mapSys != null)
            {
                mapdb = GameTableManager.Instance.GetTableItem<table.MapDataBase>(mapSys.GetMapID());
                CameraFollow.Instance.SetCameraDis(isFarDisCamera ? mapdb.farDistance : mapdb.CamDis);
            }
        }


        /// <summary>
        /// 当前任务技能
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        private List<uint> GetSkillListByJob(int job)
        {
            List<uint> skillList = new List<uint>();
            List<SkillDatabase> list = GameTableManager.Instance.GetTableList<SkillDatabase>();

            for (int i = 0; i < list.Count; i++)
            {
                SkillDatabase db = list[i];
                if (db.dwJob == job)
                {
                    if (!skillList.Contains(db.wdID))
                    {
                        if (db.wdID != 0)
                        {
                            skillList.Add(db.wdID);
                        }

                    }
                }
            }

            return skillList;
        }
    }
}
