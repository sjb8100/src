using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

namespace MapSystem
{
    // 地图间切换的点
    // 记录地图间的拓扑关系
    class MapChangePoint
    {
        // 加载传送阵表格，建立地图关系拓扑图
        // 传送点数据
        private List<DeliverDatabase> m_lstDeliverData = new List<DeliverDatabase>();

        // 地图传送点
        class MapDeliverPoint
        {
            public uint mapid = 0;                  // 所在地图ID
            public Vector2 pos = new Vector2();     // 所在地图坐标
            public uint destmapid = 0;              // 目标地图ID
        }
        // 地图上的传送点列表  地图(Key)连通数据 可以直接通的地图列表
        private Dictionary<uint, List<MapDeliverPoint>> m_dicMapDeliver = new Dictionary<uint, List<MapDeliverPoint>>();

        // 地图节点
        class MapPoint
        {
            public uint mapid = 0;                          // 所在地图ID
            public List<uint> nextMap = new List<uint>();   // 连通地图
            public List<float> nextDis = new List<float>();   // 连通点距离 与地图ID列表对应
        }
        private Dictionary<uint, MapPoint> m_dicMapPoint = new Dictionary<uint, MapPoint>();

        // 寻路算法产生的结果
        // 地图连接信息
        class MapPath
        {
            public List<uint> links = new List<uint>();
            public float fDistance = 0.0f;
        }
        class MapConnex
        {
            public uint mapid = 0;      // 地图ID
            public Dictionary<uint, MapPath> connex = new Dictionary<uint, MapPath>(); // 地图连通点
        }
        private Dictionary<uint, MapConnex> m_dicMapConnex = new Dictionary<uint, MapConnex>();

        public bool Create()
        {
            m_lstDeliverData = Table.Query<DeliverDatabase>();
            if(m_lstDeliverData==null)
            {
                Engine.Utility.Log.Error("地图传送表没有找到!");
                return true;
            }
            
            // 建立地图拓扑关系
            BuildMapConnex();

            return true;
        }

        //-------------------------------------------------------------------------------------------------------
        public void Close()
        {
            m_lstDeliverData.Clear();
            m_dicMapDeliver.Clear();
            m_dicMapConnex.Clear();
        }

        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 查找地图间连接点
        @param srcMapID 源地图
        @param destMapID 目标地图
        @return 地图中连接传送点位置
        */
        public bool FindMapLinkPoint(uint srcMapID, uint destMapID, out Vector3 pos)
        {
            pos = Vector3.zero;

            // 查找路径
            MapConnex connex = null;
            if(!m_dicMapConnex.TryGetValue(srcMapID,out connex))
            {
                return false;
            }

            MapPath mp = null;
            if (!connex.connex.TryGetValue(destMapID,out mp))
            {
                return false;
            }

            if(mp.links.Count<2)
            {
                return false;
            }

            // 根据路径上的点，查找传送点坐标
            List<MapDeliverPoint> lstPoint = null;
            if(!m_dicMapDeliver.TryGetValue(srcMapID,out lstPoint))
            {
                return false;
            }

            for (int i = 0; i < lstPoint.Count; ++i)
            {
                if(lstPoint[i].destmapid == mp.links[1])
                {
                    pos.x = lstPoint[i].pos.x;
                    pos.y = 0;
                    pos.z = lstPoint[i].pos.y;
                    return true;
                }
            }

            return false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        void BuildMapConnex()
        {
            // 创建地图传送点
            BuildMapDeliverPoint();

            uint[] mapids = m_dicMapDeliver.Keys.ToArray();
            for(int i = 0; i < mapids.Length; ++i)
            {
                MapConnex connex = new MapConnex();
                connex.mapid = mapids[i];

                for(int j = 0; j < mapids.Length;++j)
                {
                    if(j==i)
                    {
                        continue;
                    }

                    List<uint> map = null;

                    float fDistance = 0.0f;
                    if (!FindMapConnex(mapids[i], mapids[j], ref fDistance, out map))
                    {
                        //Engine.Utility.Log.Error("找不到地图{0}到{1}通路", mapids[i], mapids[j]);
                        continue;
                    }

                    MapPath mp = new MapPath();
                    mp.fDistance = fDistance;
                    mp.links = map;
                    connex.mapid = mapids[j];
                    connex.connex.Add(mapids[j], mp);
                }

                m_dicMapConnex.Add(mapids[i], connex);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        bool FindMapConnex(uint srcMap, uint destMap, ref float fDistance, out List<uint> maps)
        {
            maps = new List<uint>();
            
            // 寻路算法
            List<uint> lstExclude = new List<uint>();
            if(!FindMapPath(srcMap, destMap, ref fDistance, ref maps, ref lstExclude))
            {
                //Engine.Utility.Log.Error("FindMapConnex:查找地图通路失败！");
                return false;
            }

            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 查找地图节点  (路点寻路算法)
        @param srcMap 源地图
        @param destMap 目标地图
        @param lstExclude 需要排除的点
        @param lstPath 路径列表
        */
        bool FindMapPath(uint srcMap, uint destMap, ref float fDis, ref List<uint> lstPath,ref List<uint> lstExclude)
        {
            // 源已经在排除列表中，则直接返回
            if(lstExclude.Contains(srcMap))
            {
                return false;
            }
            // 源已经在路径列表中，则返回
            if (lstPath.Contains(srcMap))
            {
                return false;
            }

            MapPoint mp = null;
            if (!m_dicMapPoint.TryGetValue(srcMap,out mp))
            {
                Engine.Utility.Log.Error("FindMapPath:没有找到地图节点{0}", srcMap);
                return false;
            }

            MapPath path = IsCachePath(srcMap, destMap);
            if(path!=null)
            {
                // 判断路径是否有效
                bool bPath = true;
                for (int i = 0; i < path.links.Count; ++i)
                {
                    if(lstExclude.Contains(path.links[i]))
                    {
                        bPath = false;
                    }
                }

                if (bPath)
                {
                    for (int i = 0; i < path.links.Count; ++i)
                    {
                        lstPath.Add(path.links[i]);
                    }

                    fDis += path.fDistance;
                }
                return true;
            }

            if (!lstExclude.Contains(srcMap))
            {
                lstExclude.Add(srcMap);
            }

            int nNext = -1;
            float fMinDistance = 100000.0f; // 最小距离
            for (int i = 0; i < mp.nextMap.Count; ++i)
            {
                if (mp.nextMap[i] == destMap)
                {
                    nNext = i;
                    lstPath.Insert(0, mp.nextMap[i]);
                    break;
                }
                else
                {
                    if (!FindMapPath(mp.nextMap[i], destMap, ref fDis, ref lstPath, ref lstExclude))
                    {
                        if (!lstExclude.Contains(mp.nextMap[i]))
                        {
                            lstExclude.Add(mp.nextMap[i]);
                        }
                        continue;
                    }
                    else
                    {
                        if (fDis < fMinDistance)
                        {
                            fMinDistance = fDis;
                            nNext = i;
                        }
                        else
                        {
                            if (!lstExclude.Contains(mp.nextMap[i]))
                            {
                                lstExclude.Add(mp.nextMap[i]);
                            }
                        }
                    }
                }
            }

            if (nNext == -1)
            {
                return false;
            }

            lstPath.Insert(0, srcMap);
            fDis += mp.nextDis[nNext];

            return true;
        }
        //-------------------------------------------------------------------------------------------------------
        void AddMapPath(uint srcMapID, uint descMapID, float fDistance, List<uint> mapPaths)
        {
            MapConnex connex = null;
            if (!m_dicMapConnex.TryGetValue(srcMapID, out connex))
            // 添加已经找到的路径
            {
                connex = new MapConnex();
                m_dicMapConnex.Add(srcMapID, connex);
            }
            connex.mapid = srcMapID;
            if (!connex.connex.ContainsKey(descMapID))
            {
                MapPath mapPath = new MapPath();
                mapPath.fDistance = fDistance;
                mapPath.links = mapPaths;
                connex.connex.Add(descMapID, mapPath);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 是否有缓存的路径
        @param 
        */
        MapPath IsCachePath(uint srcMapID, uint descMapID)
        {
            MapConnex mc = null;
            if(!m_dicMapConnex.TryGetValue(srcMapID,out mc))
            {
                return null;
            }

            MapPath mp = null;
            if (!mc.connex.TryGetValue(descMapID, out mp))
            {
                return null;
            }

            return mp;
        }
        //-------------------------------------------------------------------------------------------------------
        // 构建 MapDeliverPoint
        void BuildMapDeliverPoint()
        {
            if(m_lstDeliverData==null)
            {
                return;
            }

            for(int i = 0; i < m_lstDeliverData.Count;++i)
            {
                MapDeliverPoint point = new MapDeliverPoint();
                point.mapid = m_lstDeliverData[i].dwMapID;
                if (m_lstDeliverData[i].pos.Count >= 2)
                {
                    point.pos = new Vector2(m_lstDeliverData[i].pos[0], m_lstDeliverData[i].pos[1]);
                }
                else 
                {
                    Engine.Utility.Log.Error("数组越界！！！");
                }
               
                point.destmapid = m_lstDeliverData[i].dwDestMapID;

                List<MapDeliverPoint> lstPoint = null;
                if (!m_dicMapDeliver.TryGetValue(point.mapid, out lstPoint))
                {
                    lstPoint = new List<MapDeliverPoint>();
                    m_dicMapDeliver.Add(point.mapid, lstPoint);
                }

                // 添加传送点
                lstPoint.Add(point);

                /////////////////////////////////////////////////////////////////////////////////////////////////////////
                // 构建MapPoint
                MapPoint mapPoint = null;
                if(!m_dicMapPoint.TryGetValue(point.mapid, out mapPoint))
                {
                    mapPoint = new MapPoint();
                    m_dicMapPoint.Add(point.mapid, mapPoint);
                }

                // 添加连通地图
                if(!mapPoint.nextMap.Contains(point.destmapid))
                {
                    mapPoint.nextMap.Add(point.destmapid);
                    mapPoint.nextDis.Add(1f); // 默认距离为1 路点寻路时需要计算实际距离
                }
            }
        }
    }
}
