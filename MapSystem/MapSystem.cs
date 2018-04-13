//*************************************************************************
//	创建日期:	2016-1-18   17:42
//	文件名称:	MapSystem.cs
//  创 建 人:   Even	
//	版权所有:	Even.xu
//	说    明:	地图系统
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Client;
using Mono.Xml;
using System.Security;
using System.IO;
using table;
using Engine;
using UnityEngine.Profiling;

namespace MapSystem
{
    class MapSystem : IMapSystem, Engine.ILoadSceneCallback, Engine.Utility.ITimer
    {
        // 游戏全局对象
        public static IClientGlobal m_ClientGlobal = null;

        // NPC坐标数据(跟地图相关)
        Dictionary<int, List<NPCInfo>> m_dicNpcInfo = new Dictionary<int, List<NPCInfo>>();
        string m_strMapTexturePath;
        public MapSystem(IClientGlobal clientGlobal)
        {
            MapSystem.m_ClientGlobal = clientGlobal;
        }

        // 全局数据， 地图间传送阵信息
        private MapChangePoint m_MapChangePoint = new MapChangePoint();
        //副本区域触发
        private Dictionary<int, Rect> m_dicZones = new Dictionary<int, Rect>();

        private MapEnterZoneCallback m_enterZoneEvent = null;

        // 主角当前区域类型
        private MapAreaType m_eMapAreaType = MapAreaType.Safe;

        // 开启草地扰动标志(默认开启)
        private bool m_bGrassForce = true;

        // 加载地图
        private uint m_uMapID = 0;
        private bool m_bFirstLoad = true;
        //是否预载
        bool m_bPreLoad = false;

        public bool GetClienNpcPos(int baseid, out Vector2 pos)
        {
            pos = Vector2.zero;
            List<NPCInfo> lstInfo = null;
            if (m_dicNpcInfo.TryGetValue(baseid, out lstInfo))
            {
                if (lstInfo == null)
                {
                    Engine.Utility.Log.Error("xml表格没有找到npc id {0}的位置", baseid);
                    return false;
                }

                Client.IPlayer player = MapSystem.m_ClientGlobal.MainPlayer;
                if (player == null)
                {
                    return false;
                }

                Vector3 playerPos = player.GetPos();
                Vector2 pp = new Vector3(playerPos.x, -playerPos.z);
                float fDistance = 1000000.0f;
                for (int i = 0; i < lstInfo.Count; ++i)
                {
                    float fDis = Vector2.Distance(lstInfo[i].pos, pp);
                    if (fDis < fDistance)
                    {
                        fDistance = fDis;
                        pos = lstInfo[i].pos;
                    }
                }

                return true;
            }
            Engine.Utility.Log.Error("xml表格没有找到npc id {0}的位置", baseid);
            return false;
        }

        /// <summary>
        /// 副本专用 是否进入某一区域
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>-1 false </returns>
        public int IsEnterZone(UnityEngine.Vector3 pos)
        {
            if (m_dicZones != null)
            {
                foreach (var item in m_dicZones)
                {
                    if (item.Value.Contains(new Vector2(pos.x, -pos.z)))
                    {
                        return item.Key;
                    }
                }
            }
            return -1;
        }
        //-------------------------------------------------------------------------------------------------------
        // 获取NPC位置
        public bool GetAllNpcInfo(out List<NPCInfo> lstNpc)
        {
            lstNpc = new List<NPCInfo>();
            Dictionary<int, List<NPCInfo>>.Enumerator iter = m_dicNpcInfo.GetEnumerator();
            while (iter.MoveNext())
            {
                for (int i = 0; i < iter.Current.Value.Count; ++i)
                {
                    if (iter.Current.Value[i].bShow)
                    {
                        lstNpc.Add(iter.Current.Value[i]);
                    }
                }
            }

            return true;
        }
        public Dictionary<int, List<NPCInfo>> GetMapXmlInfo()
        {
            return m_dicNpcInfo;
        }
        public static IClientGlobal GetClientGlobal()
        {
            return m_ClientGlobal;
        }


        public bool IsFirstLoad()
        {
            return m_bFirstLoad;
        }

        public bool Create(bool bFlag = false)
        {
            if (!bFlag)
            {
                if (m_MapChangePoint != null)
                {
                    if (!m_MapChangePoint.Create())
                    {
                        Engine.Utility.Log.Error("创建地图传送点数据出错！");
                        return false;
                    }
                }
            }

            // 注册事件
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)GameEventID.SYSTEM_GAME_READY, OnEvent);

            bSettingNotPreLoad = PlayerPrefs.GetInt("ePreLoad", 0) == 1 ? true : false;
            return true;
        }

        //清空资源
        public void Release()
        {
            // 清理地图资源
            if (m_MapChangePoint != null)
            {
                m_MapChangePoint.Close();
            }

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)GameEventID.SYSTEM_GAME_READY, OnEvent);
        }

        void OnEvent(int nEventID, object param)
        {
            if (m_ClientGlobal == null)
            {
                return;
            }

            IEntitySystem es = m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                return;
            }

            GameEventID evt = (GameEventID)nEventID;
            long uid = 0;
            switch (evt)
            {
                case GameEventID.ENTITYSYSTEM_ENTITYMOVE:
                    {
                        stEntityMove move = (stEntityMove)param;
                        uid = move.uid;
                        break;
                    }
                case GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE:
                    {
                        stEntityStopMove move = (stEntityStopMove)param;
                        uid = move.uid;
                        break;
                    }
                case GameEventID.SYSTEM_GAME_READY:
                    {
                        PreLoadAllResource();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            IEntity en = es.FindEntity(uid);
            if (en != null && en.GetEntityType() == EntityType.EntityType_Player)
            {
                Vector3 pos = en.GetPos();
                MapAreaType type = GetAreaTypeByPos(pos);
                if (m_eMapAreaType != type)
                {
                    stEntityChangeArea changeArea = new stEntityChangeArea();
                    changeArea.uid = uid;
                    changeArea.eType = type;
                    m_eMapAreaType = type;
                    Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.ENTITYSYSTEM_CHANGEAREA, changeArea);
                }
            }
        }

        public uint mapID
        {
            get { return m_uMapID; }
            set
            {
                if (m_uMapID != 0)
                {
                    m_bFirstLoad = false;
                }
                m_uMapID = value;
            }
        }

        // 地图名称
        public string mapName
        {
            set;
            get;
        }
     
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 进入地图
        @param nMapID 地图ID
        */
        public void EnterMap(uint nMapID, Vector3 pos)
        {
            // 读取地图配置表
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            Engine.Utility.Log.Info("EnterMapStart");
            if (rs != null)
            {
                if (nMapID == mapID)
                {
                    return;
                }
             
                Engine.Utility.Log.Info("EnterMapStart mapID");
                // 停止背景音乐
                Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
                if (audio != null)
                {
                    audio.StopMusic();
                }

                // 离开当前场景
                Process = 0f;
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, mapID);
                //进入下一个地图
                Engine.Utility.Log.Info("EnterMapStart 离开当前场景");
                table.MapDataBase mapDB = GameTableManager.Instance.GetTableItem<table.MapDataBase>(nMapID);
                if (mapDB == null)
                {
                    Engine.Utility.Log.Error("MapSystem:找不到地图配置数据{0}", nMapID);
                    return;
                }
                Engine.Utility.Log.Info("EnterMapStart 离开当前场景 table.MapDataBase mapDB ");
                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(mapDB.dwResPath);
                if (resDB == null)
                {
                    Engine.Utility.Log.Error("MapSystem:找不到地图资源路径配置{0}", mapDB.dwResPath);
                    return;
                }
                temptime = Time.realtimeSinceStartup;
                lastInfo = "none";
                // 创建寻路模块
                mapID = nMapID;
                // 预加载
                mapName = mapDB.strName;// string.Format("{0}({1})", mapDB.strName, uCountryid);

                // 不是中转空场景
                if (nMapID != GameTableManager.Instance.GetClientGlobalConst<int>("map", "emptyid"))
                {
                    Engine.Utility.Log.Info("EnterMapStart 读取地图配置表 ");
                    // 读取地图配置表
                    string strMapName = resDB.strPath.ToLower();
                    m_strMapTexturePath = strMapName.Replace(".map", "").Substring(strMapName.LastIndexOf('/') + 1);
                    Engine.IScene scene = rs.EnterScene(ref strMapName, this);
                    if (scene != null)
                    {
                        scene.StartLoad(pos);
                    }

                    Engine.Utility.Log.Info("EnterMapStart EnterScene name:{0} ", strMapName);
                    // 场景npc配置
                    LoadClientMap(mapDB.miniMapInfo.ToLower());

                    //LoadClienMapArea(strMapName.ToLower().Replace(".map", ".ar"));

                    //SpanUtil.Start();
                    //if (Application.platform == RuntimePlatform.WindowsEditor)
                    //{
                    Profiler.BeginSample("LoadARFile");
                    MapAreaDisplay.Instance.LoadARFile(strMapName.ToLower().Replace(".map", ".ar"));
                    Profiler.EndSample();
                    //}
                    //SpanUtil.Stop(strMapName + "  ar  ");

                    string strMPSName = strMapName.Replace(".map", ".mps").ToLower();
                    // 读取地表障碍信息
                    LoadObstacle(strMPSName);
                    //SpanUtil.Stop(strMapName + "  地表障碍信息  ");
                }

            }
            else
            {
                Engine.Utility.Log.Error("MapSystem:IRenderSystem为空");
            }
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)(int)Client.GameEventID.ENTITYSYSTEM_ENTERMAP, mapID);
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 退出当前场景
        @param 
        */
        public void ExitMap()
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs != null)
            {
                rs.ExitScene();
            }
            mapID = 0;
            m_bFirstLoad = true; // 清空标识
            m_dicNpcInfo.Clear();
            m_strMapTexturePath = "";
        }
        //-------------------------------------------------------------------------------------------------------
        // 读取地表障碍信息
        public bool LoadObstacle(string path)
        {
            MapObstacle.Instance.Load(path);
            return true;
        }

        public Vector2 GetMapSize()
        {
            return new Vector2(MapObstacle.Instance.Width, MapObstacle.Instance.Height);
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 获取MapID
        @param 
        */
        public uint GetMapID()
        {
            return mapID;
        }
        //-------------------------------------------------------------------------------------------------------
        // 获取地图名称
        public string GetMapName()
        {
            return mapName;
        }

        public string GetMapTextureName()
        {
            return m_strMapTexturePath;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief FindPath 寻路算法
        @param curPos 当前位置
        @param tarPos 目标位置
        @param[out] path 输出路径
        */
        public bool FindPath(Vector2 curPos, Vector2 tarPos, out List<Vector2> path)
        {
            path = null;
            //UnityEngine.Profiler.BeginSample("FindPath");
            PathMove.Instance().findPath(new Vector2(curPos.x, -curPos.y), new Vector2(tarPos.x, -tarPos.y), 0.0f, out path);
            //UnityEngine.Profiler.EndSample();
            if (path.Count <= 0)
            {
                return false;
            }

            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 寻路回调
        @param 
        */
        public void AddFindPathCallback(Action<List<Vector2>> findPath)
        {
            PathMove.Instance().PathFind += findPath;
        }
        //-------------------------------------------------------------------------------------------------------
        // 删除寻路回调
        public void RemoveFindPathCallback(Action<List<Vector2>> findPath)
        {
            PathMove.Instance().PathFind -= findPath;
        }
        //-------------------------------------------------------------------------------------------------------
        // 获取地图上距离(计算阻挡)
        public bool CalcDistance(Vector2 curPos, Vector2 tarPos, out float fDistance, TileType tileType = TileType.TileType_None)
        {
            if (tileType == TileType.TileType_None) // 直接计算直线距离
            {
                fDistance = Vector2.Distance(curPos, tarPos);
                return true;
            }

            //if(MapObstacle.Instance.Check())
            float ShortestMoveDst = 0.1f;
            MapGrid dstPt = MapGrid.GetMapGrid(new MapVector2(tarPos.x, tarPos.y));

            Vector2 dir = tarPos - curPos;
            dir.Normalize();
            dir *= ShortestMoveDst;

            MapGrid curPt = new MapGrid(0, 0);

            var vecDst = tarPos;
            bool bBlock = false; // 校验
            for (; (vecDst - curPos).magnitude > ShortestMoveDst; curPos += dir)
            {
                var tmp = MapGrid.GetMapGrid(new MapVector2(curPos.x, curPos.y));
                if (curPt != tmp)
                {
                    curPt = tmp;
                }
                else
                {
                    continue;
                }

                if (!MapObstacle.Instance.Check((int)curPt.x, (int)curPt.z, tileType))
                {
                    bBlock = true;
                    break;
                }

                if (curPt == dstPt)
                {
                    break;
                }
            }

            fDistance = 0.0f;
            if (!bBlock)
            {
                fDistance = Vector2.Distance(curPos, tarPos);
            }
            else
            {
                List<Vector2> path = null;
                if (!PathMove.Instance().findPath(new Vector2(curPos.x, curPos.y), new Vector2(tarPos.x, tarPos.y), 0.0f, out path))
                {
                    return false;
                }

                Vector2 lastPos = curPos;
                for (int i = 0; i < path.Count; ++i)
                {
                    fDistance += Vector2.Distance(lastPos, path[i]);
                    lastPos = path[i];
                }
            }

            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 屏幕点击位置转换到场景坐标
        @param pos 屏幕位置
        @param scenePos 场景位置
        */
        public bool GetScenePos(Vector2 pos, out Engine.TerrainInfo terrainInfo)
        {
            terrainInfo = null;
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return false;
            }

            // 再计算移动速度
            Engine.IScene scene = rs.GetActiveScene();
            if (scene == null)
            {
                return false;
            }

            return scene.GetPickupPos(pos, out terrainInfo);
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 添加草扰动力
        @param pos 力中心点位置
        @param fForce 力大小
        @param fRadius 力影响范围
        */
        public void AddGrassWaveForce(Vector3 pos, float fForce, float fRadius)
        {
            if (!m_bGrassForce)
            {
                return;
            }

            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            // 再计算移动速度
            Engine.IScene scene = rs.GetActiveScene();
            if (scene == null)
            {
                return;
            }

            scene.AddGrassWaveForce(pos, fForce, fRadius);
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 开启草地扰动(全局设置)
        @param 
        */
        public void EnableGrassForce(bool bEnable)
        {
            m_bGrassForce = bEnable;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 查找地图间连接点
        @param srcMapID 源地图
        @param destMapID 目标地图
        @param[out] 地图中连接传送点位置
        @return 成功与否
        */
        public bool FindMapLinkPoint(uint srcMapID, uint destMapID, out Vector3 pos)
        {
            pos = Vector3.zero;
            if (m_MapChangePoint != null)
            {
                return m_MapChangePoint.FindMapLinkPoint(srcMapID, destMapID, out pos);
            }

            return false;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 检测是否可以行走
        @param pos 场景位置
        */
        public bool CanWalk(Vector3 pos)
        {
            return MapObstacle.Instance.CheckObstacle(new Vector2(pos.x, -pos.z), 0);
        }
        //-------------------------------------------------------------------------------------------------------

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //-------------------------------------------------------------------------------------------------------
        // 加载进度
        string lastInfo = "none";
        float temptime = 0;
        public void OnProgress(float fProgress,string debugInfo)
        {
            // 进度条
            Process = fProgress;
            if(debugInfo != lastInfo)
            {
                float dt = Time.realtimeSinceStartup - temptime;

                //Debug.LogError("<color=cyan>[MAPTIME]" + lastInfo + "==================================== 消耗时间 是 " + dt + "</color>");
                temptime = Time.realtimeSinceStartup;
                lastInfo = debugInfo;
            }
         
            // 处理加载进度
        }

        public float Process { get; set; }
        // 加载完成
        public void OnComplete()
        {
            // 加载完成处理
            Client.stLoadSceneComplete loadScene = new Client.stLoadSceneComplete();
            loadScene.nMapID = (int)mapID;
            loadScene.bFirstLoadScene = m_bFirstLoad; // 第一次加载场景
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.SYSTEM_LOADSCENECOMPELETE, loadScene);

            // 播放背景音乐
            PlayBGMusic();

            Engine.Utility.Log.Info("EnterMapStart PreLoadRes");
            // 预加载资源
            //PreLoadResource();   //移到了 登录成功，就会预加载
            Engine.Utility.Log.Info("EnterMapStart PreLoadRes end");
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 播放背景音乐
        @param 
        */
        private void PlayBGMusic()
        {
            table.MapDataBase mapDB = GameTableManager.Instance.GetTableItem<table.MapDataBase>(mapID);
            if (mapDB == null)
            {
                Engine.Utility.Log.Error("MapSystem:找不到地图配置数据{0}", mapID);
                return;
            }

            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(mapDB.dwbgm);
            if (resDB == null)
            {
                Engine.Utility.Log.Error("MapSystem:找不到地图资源路径配置{0}", mapDB.dwResPath);
                return;
            }

            Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
            if (audio != null)
            {
                audio.PlayMusic(resDB.strPath);
            }
        }

        /// <summary>
        /// 设置主角进入区域的回调
        /// </summary>
        /// <param name="callback"></param>
        public void SetEnterZoneCallback(MapEnterZoneCallback callback)
        {
            m_enterZoneEvent = callback;
        }

        public float GetTerrainHeight(Vector3 pos)
        {
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return 0.0f;
            }

            // 再计算移动速度
            Engine.IScene curScene = rs.GetActiveScene();
            if (curScene == null)
            {
                return 0.0f;
            }


            Engine.TerrainInfo info;
            if (!curScene.GetTerrainInfo(ref pos, out info))
            {
                return 0.0f;
            }

            return info.pos.y;
        }

        public void Update(float dt)
        {
            if (m_ClientGlobal.MainPlayer == null)
            {
                return;
            }

            if (m_enterZoneEvent != null)
            {
                Client.IEntity mainPlayer = m_ClientGlobal.MainPlayer;
                if (mainPlayer != null)
                {
                    int zoneid = this.IsEnterZone(mainPlayer.GetPos());
                    if (zoneid != -1)
                    {
                        m_enterZoneEvent(zoneid);
                    }
                }
            }

            // 检测遮挡物
            Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            if (rs == null)
            {
                return;
            }

            // 再计算移动速度
            Engine.IScene scene = rs.GetActiveScene();
            if (scene == null)
            {
                return;
            }

            scene.CheckOcclusion(m_ClientGlobal.MainPlayer.GetPos());
        }

        // 跨地图寻路
        //public Vector2 FindMapChangePoint(uint uSrcMapID, uint uDestMapID)
        //{

        //}

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /**
        @brief 预加载资源
        @param 
        */

        private void PreLoadResource()
        {
            //Engine.IRenderSystem rs = Engine.RareEngine.Instance().GetRenderSystem();
            //if (rs == null)
            //{
            //    return;
            //}

            // 预加载文件列表 要改成读取配置表
            string strHitEffect = "effect/skill/ZhuJue/ZhanShi_Nan/EF_ZhanShi_Nan_@Hit.fx";
            Engine.RareEngine.Instance().PreloadEffectObj(strHitEffect.ToLower(), 8);
            strHitEffect = "effect/skill/ZhuJue/Boss_YuNv/EF_Boss_YuNv_@Hit.fx";
            Engine.RareEngine.Instance().PreloadEffectObj(strHitEffect.ToLower(), 5);

            string strEffect = "effect/ui/ef_buff_@mubiaoXuanze001.fx";
            Engine.RareEngine.Instance().PreloadEffectObj(strEffect.ToLower(), 1);
            strEffect = "effect/UI/EF_Buff_@MuBiaoXuanZe002.fx";
            strEffect = strEffect.ToLower();
            Engine.RareEngine.Instance().PreloadEffectObj(strEffect.ToLower(), 1);
            strEffect = "effect/UI/EF_Buff_@MuBiaoXuanZe004.fx";
            strEffect = strEffect.ToLower();
            Engine.RareEngine.Instance().PreloadEffectObj(strEffect.ToLower(), 1);
        }


        private void LoadClientMap(string mapPath)
        {
            m_dicNpcInfo.Clear();
            m_dicZones.Clear();
            if (string.IsNullOrEmpty(mapPath))
            {
                return;
            }

            try
            {
                byte[] bytes = Engine.Utility.FileUtils.Instance().ReadFile(mapPath);
                if (bytes == null)
                {
                    return;
                }
                string xml = System.Text.Encoding.UTF8.GetString(bytes);
                SecurityElement seRoot = XmlParser.Parser(xml);
                if (seRoot != null)
                {
                    SecurityElement npcse = seRoot.SearchForChildByTag("Npc");
                    if (npcse != null)
                    {
                        if (npcse.Children != null)
                        {

                            for (int i = 0; i < npcse.Children.Count; i++)
                            {
                                SecurityElement item = (SecurityElement)npcse.Children[i];
                                NPCInfo info = new NPCInfo();
                                info.npcID = int.Parse(item.Attribute("id"));
                                float x = float.Parse(item.Attribute("x"));
                                if (!string.IsNullOrEmpty(item.Attribute("tx")))
                                {
                                    x = float.Parse(item.Attribute("tx"));
                                }
                                float z = float.Parse(item.Attribute("y"));
                                if (!string.IsNullOrEmpty(item.Attribute("ty")))
                                {
                                    z = float.Parse(item.Attribute("ty"));
                                }
                                bool bShow = true;
                                if (!string.IsNullOrEmpty(item.Attribute("visible")))
                                {
                                    int n = int.Parse(item.Attribute("visible"));
                                    bShow = n == 1 ? true : false;
                                }
                                info.name = item.Attribute("name");
                                info.pos = new Vector2(x, z);
                                info.bShow = bShow;
                                List<NPCInfo> lstInfo = null;
                                if (!m_dicNpcInfo.TryGetValue(info.npcID, out lstInfo))
                                {
                                    lstInfo = new List<NPCInfo>();

                                    m_dicNpcInfo.Add(info.npcID, lstInfo);
                                }

                                if (lstInfo != null)
                                {
                                    lstInfo.Add(info);
                                }
                            }
                        }
                    }
                    //副本区域
                    SecurityElement zonese = seRoot.SearchForChildByTag("Zone");
                    if (zonese != null && zonese.Children != null)
                    {
                        for (int i = 0; i < npcse.Children.Count; i++)
                        {
                            SecurityElement item = (SecurityElement)npcse.Children[i];
                            int id = int.Parse(item.Attribute("id"));
                            int width = int.Parse(item.Attribute("width"));
                            int height = int.Parse(item.Attribute("height"));
                            int x = int.Parse(item.Attribute("x"));
                            int y = int.Parse(item.Attribute("y"));
                            if (!m_dicZones.ContainsKey(id))
                            {
                                m_dicZones.Add(id, new Rect(x, y, width, height));
                            }
                        }
                    }
                }
                bytes = null;
                Engine.Utility.Log.Trace("LoadXmlNPC:" + m_dicNpcInfo.Count);
            }
            catch (Exception e)
            {

                Engine.Utility.Log.Error(e.ToString());
            }

        }

        public MapAreaType GetAreaTypeByPos(Vector3 pos)
        {
            return MapAreaDisplay.Instance.GetAreaTypeByPos(pos, GetMapSize());
        }
        public void SetMapAreaVisible(bool vivible)
        {
            if (vivible)
            {
                MapAreaDisplay.Instance.Show();
            }
            else
            {
                MapAreaDisplay.Instance.Hide();
            }
        }

        public void SetMap9Grid(bool vivible)
        {
            if (vivible)
            {
                MapAreaDisplay.Instance.Show9Grid();
            }
            else
            {
                MapAreaDisplay.Instance.Hide9Grid();
            }
        }

        public int GetMapGridId(Vector3 pos)
        {
            int gridWidth = (int)GameCmd.GameCmdConst.SCREEN_GRID_WIDTH;
            int gridHeight = (int)GameCmd.GameCmdConst.SCREEN_GRID_HEIGHT;
            // posi = ((screenWH.x + GameCmd::SCREEN_GRID_WIDTH - 1) / GameCmd::SCREEN_GRID_WIDTH) * (pos.y / GameCmd::SCREEN_GRID_HEIGHT) + (pos.x / GameCmd::SCREEN_GRID_WIDTH);
            int posi = (int)(((2048 + gridWidth - 1) / gridWidth) * (-(int)pos.z / gridHeight) + ((int)pos.x / gridWidth));
            return posi;
        }

        public void RequestEnterMap(uint nmapId,uint cost = 0)
        {
            if (nmapId == mapID)
            {
                Client.ITipsManager tips = m_ClientGlobal.GetTipsManager();
                if (tips != null)
                {
                    tips.ShowTipsById(514);
                }
                return;
            }

            if (m_ClientGlobal.netService != null)
            {
                // 请求传送地图 先让主角停止
                if (m_ClientGlobal.MainPlayer != null)
                {
                    m_ClientGlobal.MainPlayer.SendMessage(EntityMessage.EntityCommand_StopMove, m_ClientGlobal.MainPlayer.GetPos());
                }

                m_ClientGlobal.netService.Send(new GameCmd.stChangeMapScreenUserCmd_CS()
                {
                    map_id = nmapId,
                    type = cost,
                });
            }
        }
        #region Preloadres
        bool bSettingNotPreLoad = true;//不执行预加载

        List<PreloadResDataBase> m_preloadList = new List<PreloadResDataBase>();
        List<PreloadSkillResDataBase> m_preloadSkillList = new List<PreloadSkillResDataBase>();
        uint m_uPreLoadTimeID = 1002;
        public void OnTimer(uint uTimerID)
        {
        
            if(uTimerID == m_uPreLoadTimeID)
            {
                if(m_preloadList.Count > 0)
                {
                    PreloadResDataBase pdb = m_preloadList[0];
                    if(pdb != null)
                    {

                        if (pdb.type == 1)//特效资源
                        {
                            Engine.RareEngine.Instance().PreloadEffectObj(pdb.path.ToLower(), (int)pdb.num);
                        }

                        if (pdb.type == 2) //obj资源
                        {
                            Engine.RareEngine.Instance().PreloadRenderObj(pdb.path.ToLower(), (int)pdb.num);
                        }
                    }
                    m_preloadList.RemoveAt(0);
                }

                if(m_preloadSkillList.Count > 0)
                {
                    PreloadSkillResDataBase db = m_preloadSkillList[0];
                    if (db != null)
                    {
                        string[] pathArr = db.pathArr.Split(';');
                        for (int j = 0; j < pathArr.Length; j++)
                        {
                            Engine.RareEngine.Instance().PreloadEffectObj(pathArr[j].ToLower(), 1);
                        }
                     
                    }
                    m_preloadSkillList.RemoveAt(0); 
                }

                if(m_preloadSkillList.Count == 0 && m_preloadList.Count == 0)
                {
                    Engine.Utility.TimerAxis.Instance().KillTimer(m_uPreLoadTimeID, this);
                }
            }
         
        }
        /// <summary>
        /// 预加载资源
        /// </summary>
        private void PreLoadAllResource()
        {
            if (bSettingNotPreLoad)
            {
                return;
            }
            if (m_bPreLoad)
            {
                return;
            }
            Profiler.BeginSample("PreLoadAllResource");
            if (m_ClientGlobal.MainPlayer == null)
            {
                return;
            }
            int job = m_ClientGlobal.MainPlayer.GetProp((int)PlayerProp.Job);
            //资源预加载
            m_preloadList = GameTableManager.Instance.GetTableList<PreloadResDataBase>();
            m_preloadList = m_preloadList.FindAll(x => x.job == job);
            //技能特效资源预加载

            m_preloadSkillList = GameTableManager.Instance.GetTableList<PreloadSkillResDataBase>();
            m_preloadSkillList = m_preloadSkillList.FindAll(x => x.job == job);
            Engine.Utility.TimerAxis.Instance().SetTimer(m_uPreLoadTimeID, 500, this);
           
            m_bPreLoad = true;
            Profiler.EndSample();
        }
        #endregion
        public void TraceServerPos(bool add, int x, int y)
        {
            ShowTraceSeverPos.Instance.RecieveData(add, x, y);
        }
    }

    /// <summary>
    /// 地图系统创建
    /// </summary>
    public class MapSystemCreator
    {
        private static MapSystem m_MapSys = null;
        public static IMapSystem CreateMapSystem(IClientGlobal clientGlobal, bool bEditor = false)
        {
            if (m_MapSys == null)
            {
                m_MapSys = new MapSystem(clientGlobal);
                if (!m_MapSys.Create(bEditor))
                {
                    return null;
                }
            }

            return m_MapSys;
        }


    }
}

