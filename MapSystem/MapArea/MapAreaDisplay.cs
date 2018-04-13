using System;
using System.Collections.Generic;
using Engine.Utility;
using Client;
using System.IO;
using UnityEngine;
namespace MapSystem
{
    public class MapAreaDisplay : Singleton<MapAreaDisplay>
    {
        MapAreaInfo[] m_MapAreaInfoArrar;
        Dictionary<BaseLayer.LayerType, BaseLayer> m_dictLayers = new Dictionary<BaseLayer.LayerType, BaseLayer>();
        _9GridLayer m_9GridLayer = null;
        const uint MAXGRID = 16;

        uint ar_width;
        uint ar_height;

        bool m_show = false;

        string m_strFilePath = "";

        public void LoadARFile(string filePath)
        {
            if (!string.IsNullOrEmpty(m_strFilePath) && m_strFilePath == filePath)
            {
                Engine.Utility.Log.Error("Reload Error {0}", m_strFilePath);
                return;
            }
            m_MapAreaInfoArrar = null;

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            m_strFilePath = filePath;
            try
            {
                byte[] bytes = Engine.Utility.FileUtils.Instance().ReadFile(filePath);
                if (bytes != null)
                {
                    MemoryStream ms = new MemoryStream(bytes);
                    BinaryReader br = new BinaryReader(ms);

                    uint ar_magic = br.ReadUInt32();
                    uint ar_ver = br.ReadUInt32();
                    ar_width = br.ReadUInt32();//X
                    ar_height = br.ReadUInt32();//Z

                    m_MapAreaInfoArrar = new MapAreaInfo[ar_height * ar_width];
                    for (int z = 0; z < ar_height; z++)
                    {
                        for (int x = 0; x < ar_width; x++) //x
                        {
                            byte ar_resid0 = br.ReadByte();
                            byte ar_resid1 = br.ReadByte();
                            uint ar_type = br.ReadUInt32();
                            m_MapAreaInfoArrar[x + z * ar_width] = new MapAreaInfo() { x = (ushort)x, z = (ushort)z, areaType = (byte)ar_type };
                        }
                    }
                }
                else
                {
                    Engine.Utility.Log.Error("Load AR FILE {0} Failed ", filePath);
                }
                bytes = null;
            }
            catch (System.Exception ex)
            {
                Engine.Utility.Log.Error("Load AR FILE {0} :{1} ", filePath, ex.ToString());
            }
        }

        public void Show()
        {
            m_show = true;
            CreateTerrain();
        }

        public void Hide()
        {
            m_show = false;
            GameObject terrainRoot = GameObject.Find("TerrainBlocks");
            if (terrainRoot != null)
            {
                GameObject.Destroy(terrainRoot);
            }
        }

        public MapAreaType GetAreaTypeByGrid(int x, int z)
        {
            return GetAreaTypeByPos(new Vector3(x, 0, -z), new Vector2(ar_width, ar_height));
        }

        public MapAreaType GetAreaTypeByPos(Vector3 pos, Vector2 mapSize)
        {
            int x = (int)pos.x;
            int z = -(int)pos.z;
            int index = x + z * (int)ar_width;

            if (m_MapAreaInfoArrar != null && index >= 0 && index < m_MapAreaInfoArrar.Length)
            {
                MapAreaInfo mapArea = m_MapAreaInfoArrar[index];
                //if (mapArea != null)
                {
                    return (MapAreaType)mapArea.areaType;                    
                }
                Engine.Utility.Log.Error("获取区域类型失败");
            }
            return MapAreaType.Normal;
        }

        void CreateTerrain()
        {
            GameObject terrainRoot = GameObject.Find("TerrainBlocks");
            if (terrainRoot != null)
            {
                GameObject.Destroy(terrainRoot);
            }
            terrainRoot = new GameObject("TerrainBlocks");
            m_dictLayers.Clear();

            TerrainBlock.GRID_NUM = 32;
            GridDef.GridSize = 32;

            for (int i = 0; i < (int)BaseLayer.LayerType.eLayerMax; i++)
            {
                BaseLayer.LayerType type = (BaseLayer.LayerType)i;
                BaseLayer bl = null;
                if (type == BaseLayer.LayerType.eLayerNormal)
                {
                    bl = new BaseLayer(type, terrainRoot.transform);
                }
                else if (type == BaseLayer.LayerType.eLayerBlock)
                {
                    bl = new BlockLayer(terrainRoot.transform);
                }
                else if (type == BaseLayer.LayerType.eLayerPK)
                {
                    bl = new PKLayer(terrainRoot.transform);
                }
                else if (type == BaseLayer.LayerType.eLayerSafe)
                {
                    bl = new SafeLayer(terrainRoot.transform);
                }
                else if (type == BaseLayer.LayerType.eLayerBoss)
                {
                    bl = new BossLayer(terrainRoot.transform);
                }
                else if (type == BaseLayer.LayerType.eLayerBattle)
                {
                    bl = new BattleLayer(terrainRoot.transform);
                }
                if (bl != null)
                {
                    m_dictLayers.Add((BaseLayer.LayerType)i, bl);
                    bl.Create(ar_width / GridDef.GridSize, ar_height / GridDef.GridSize);
                    bl.RefreshGridColor();
                }
            }
            if (terrainRoot.GetComponent<MapAreaRoot>() == null)
            {
                terrainRoot.AddComponent<MapAreaRoot>().Init(ar_width, ar_width, m_strFilePath);
            }
        }

        void SetLayer(Transform parent)
        {
            parent.gameObject.layer = LayerMask.NameToLayer("TerrainBlock");

            if (parent.childCount > 0)
            {
                foreach (Transform item in parent)
                {
                    SetLayer(item);
                }
            }
        }


        public void Show9Grid()
        {
//             int width = (int)(ar_width / MAXGRID);
//             if (ar_width % MAXGRID != 0)
//             {
//                 width++;
//             }
// 
//             int height = (int)(ar_height / MAXGRID);
//             if (ar_height % MAXGRID != 0)
//             {
//                 height++;
//             }

            GameObject terrainRoot = GameObject.Find("9Grids");
            if (terrainRoot != null)
            {
                GameObject.Destroy(terrainRoot);
            }

            terrainRoot = new GameObject("9Grids");
            uint gridWidth = (uint)GameCmd.GameCmdConst.SCREEN_GRID_WIDTH;
            TerrainBlock.GRID_NUM = gridWidth;
            GridDef.GridSize = gridWidth;
            m_9GridLayer = new _9GridLayer(terrainRoot.transform);
            m_9GridLayer.Create(ar_width / gridWidth, ar_height / gridWidth);
            m_9GridLayer.RefreshGridColor();
            if (terrainRoot.GetComponent<MapAreaRoot>() == null)
            {
                terrainRoot.AddComponent<MapAreaRoot>().Init(ar_width, ar_width, m_strFilePath);
            }
        }

        public void Hide9Grid()
        {
            m_9GridLayer = null;
            GameObject terrainRoot = GameObject.Find("9Grids");
            if (terrainRoot != null)
            {
                GameObject.Destroy(terrainRoot);
            }
        }

        public void Update()
        {
            if (m_dictLayers != null && m_show)
            {
                foreach (var item in m_dictLayers.Values)
                {
                    item.Update();
                }
            }

            if (m_9GridLayer != null)
            {
                m_9GridLayer.Update();
            }
        }
    }
}