using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace MapSystem
{
    public class BaseLayer
    {
        //不能改
        public enum LayerType
        {
            eLayerNormal = 0,
            eLayerBattle = 1,
            eLayerSafe = 2,
            eLayerBlock = 3,
            eLayerPK = 4,
            eLayerBoss = 8,
            eLayer9Grid = 16, 
            eLayerMax,
        }

        protected GameObject m_obj;


        protected LayerType m_layerType = LayerType.eLayerNormal;
        public LayerType Layer { get { return m_layerType; } }
        protected LayerData m_layerData;

        public LayerData Data { get { return m_layerData; } }
        TerrainBlock[] m_TerrainBlocks;
        public BaseLayer(LayerType ltype, Transform parent)
        {
            if (ltype == LayerType.eLayerMax)
            {
                Debug.LogError("LayerType error");
                return;
            }

            m_layerType = ltype;
            m_obj = new GameObject(m_layerType.ToString());
            m_obj.transform.parent = parent;
        }

        public void Create(uint blockNumX, uint blockNumZ)
        {
            m_layerData = new LayerData(m_layerType, blockNumX, blockNumZ);
            CreateTerrainBlocks(m_layerData);
        }

        void CreateTerrainBlocks(LayerData data)
        {
            m_TerrainBlocks = new TerrainBlock[data.BlockNumX * data.BlockNumZ];

            for (int z = 0; z < data.BlockNumZ; z++)
            {
                for (int x = 0; x < data.BlockNumX; x++)
                {
                    TerrainBlock grid = new TerrainBlock(data, m_obj.transform, x, z);
                    m_TerrainBlocks[x + z * (int)data.BlockNumX] = grid;
                }
            }
            OnTerrain();
        }

        public void SetActive(bool active)
        {
            m_obj.SetActive(active);
        }
        /// <summary>
        /// 贴地
        /// </summary>
        public void OnTerrain()
        {
            for (int z = 0; z < m_layerData.VertexCountZ; z++)
            {
                for (int x = 0; x < m_layerData.VertexCountX; x++)
                {
                    RaycastHit hit;

                    // 一象限变四象限
                    Vector3 origin = new Vector3(x, 1000, -z);

                    if (Physics.Raycast(origin, -Vector3.up, out hit, 100000, 1 << 20))
                    {
                        float fHeight = hit.point.y;
                        m_layerData.SetHeight(x, z, fHeight);
                    }
                    else
                    {
                        m_layerData.SetHeight(x, z, 0.0f);
                    }
                }
            }
        }

        public void Update()
        {
            if (m_obj.activeSelf == false)
            {
                return;
            }

            if (m_layerData == null)
            {
                return;
            }

            for (int height = 0; height < m_layerData.BlockNumZ; height++)
            {
                for (int width = 0; width < m_layerData.BlockNumX; width++)
                {
                    TerrainBlock grid = m_TerrainBlocks[width + height * (int)m_layerData.BlockNumX];
                    if (grid != null)
                    {
                        grid.UpdateTerrainBlock();
                    }
                }
            }
        }

        public virtual void LoadData(ref BinaryReader br)
        {
            for (int nIndex = 0; nIndex < m_layerData.GridX * m_layerData.GridZ; nIndex++)
            {
                ushort uValue = br.ReadUInt16();
                if (uValue == (ushort)Layer)
                {
                    m_layerData.SetFlag(nIndex, true);
                }
                else
                {
                    m_layerData.SetFlag(nIndex, false);
                }
            }
        }

        public virtual void RefreshGridColor()
        {
            for (int z = 0; z < m_layerData.GridZ; z++)
            {
                for (int x = 0; x < m_layerData.GridX; x++) //x
                {
                    if (Layer == LayerType.eLayerBlock)
                    {
                        bool block = MapSystem.m_ClientGlobal.GetMapSystem().CanWalk(new Vector3(x, 0, -z));
                        m_layerData.SetFlag(x + z * (int)m_layerData.GridX, !block);
                    }else if (Layer == LayerType.eLayer9Grid)
                    {
                        int width = x % 16;
                        int height = z *(int)m_layerData.GridX % 16;

                        m_layerData.SetFlag(x + z * (int)m_layerData.GridX,  true);
                    }
                    else
                    {
                        int layer = (int)MapAreaDisplay.Instance.GetAreaTypeByGrid(x, z);
                        if ((layer & (int)Layer) == (int)Layer)
                        {
                            m_layerData.SetFlag(x + z * (int)m_layerData.GridX, true);
                        }
                        else
                        {
                            m_layerData.SetFlag(x + z * (int)m_layerData.GridX, false);
                        }
                    }
                }
            }
        }

        public virtual void SaveData(ref string path)
        {

        }

        public void SetFlag(float fX, float fZ, bool flag)
        {
            if (!m_obj.activeSelf)
            {
                Debug.LogError("操作失败 layertype：" + m_layerType);
                return;
            }
            int nX = (int)(fX / TerrainBlock.GRID_LEN);
            int nZ = (int)(fZ / TerrainBlock.GRID_LEN);

            if (nX < 0 || nX > m_layerData.GridX)
                return;
            if (nZ < 0 || nZ > m_layerData.GridZ)
                return;

            GridInfo grid = m_layerData.GetGrid(nX, nZ);
            grid.IsMask = flag;

            /* Debug.Log("set flag x" + nX + " z" + nZ);*/

            SetNodeNeedUpdate(nX, nZ);
        }

        void SetNodeNeedUpdate(int nX, int nZ)
        {
            if (nX < 0 || nX > m_layerData.GridX)
                return;
            if (nZ < 0 || nZ > m_layerData.GridZ)
                return;

            for (int height = 0; height < m_layerData.BlockNumZ; height++)
            {
                for (int width = 0; width < m_layerData.BlockNumX; width++)
                {
                    int nStartX = 0;
                    int nEndX = 0;
                    int nStartZ = 0;
                    int nEndZ = 0;
                    TerrainBlock.GetNodeBound(m_layerData, width, height, ref nStartX, ref nEndX, ref nStartZ, ref nEndZ);
                    if (nX >= nStartX && nX <= nEndX && nZ >= nStartZ && nZ <= nEndZ)
                    {
                        m_TerrainBlocks[height * (int)m_layerData.BlockNumX + width].bUpdate = true;
                    }
                }
            }
        }

    }
}