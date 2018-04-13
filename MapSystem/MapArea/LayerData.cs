using UnityEngine;
using System.Collections;
namespace MapSystem
{
    public sealed class GridDef
    {
        public static uint GridSize = 32;
        public const uint GridLen = 1;

        public const int GRID_SIZE = 32;
        public const float GRID_LEN = 1;
    }

    /// <summary>
    /// 顶点信息
    /// </summary>
    public class VertexInfo
    {
        public float height;
        public float x;
        public float z;
    }

    /// <summary>
    /// 顶点信息
    /// </summary>
    public class GridInfo
    {
        public int x;
        public int z;
        public bool IsMask = false;
    }

    public class LayerData
    {
        public uint VertexCountX { get; set; }

        public uint VertexCountZ { get; set; }

        public uint BlockNumX { get; set; }

        public uint BlockNumZ { get; set; }

        public uint GridX { get; set; }
        public uint GridZ { get; set; }


        private uint m_uTotalBlock = 0;//总的大格子数

        private VertexInfo[] m_TerrainVertices = null;

        private GridInfo[] m_TerranGrids = null;

        public BaseLayer.LayerType layerType = BaseLayer.LayerType.eLayerNormal;

        public LayerData(BaseLayer.LayerType type, uint blockNumX, uint blockNumZ)
        {
            layerType = type;

            BlockNumX = blockNumX;
            BlockNumZ = blockNumZ;

            GridX = TerrainBlock.GRID_NUM * blockNumX;
            GridZ = TerrainBlock.GRID_NUM * blockNumZ;

            VertexCountX = GridX + 1;
            VertexCountZ = GridZ + 1;

            m_uTotalBlock = blockNumX * blockNumZ;

            //Debug.Log(BlockNumX + " " + BlockNumZ);
            //Debug.Log(VertexCountX + " " + VertexCountZ);

            m_TerrainVertices = new VertexInfo[VertexCountX * VertexCountZ];
            for (int z = 0; z < VertexCountZ; z++)
            {
                for (int x = 0; x < VertexCountX; x++)
                {
                    VertexInfo pTemp = new VertexInfo();
                    int index = x + (int)VertexCountX * z;
                    pTemp.x = x * TerrainBlock.GRID_LEN;
                    pTemp.z = z * TerrainBlock.GRID_LEN;
                    pTemp.height = 0;
                    m_TerrainVertices[index] = pTemp;

                }
            }

            m_TerranGrids = new GridInfo[(GridX) * (GridZ)];
            for (int z = 0; z < GridZ; ++z)
            {
                for (int x = 0; x < GridX; ++x)
                {
                    GridInfo pTemp = new GridInfo();
                    int index = x + (int)GridX * z;
                    pTemp.x = x;
                    pTemp.z = z;
                    pTemp.IsMask = type == BaseLayer.LayerType.eLayerNormal ? true : false;
                    m_TerranGrids[index] = pTemp;
                }
            }
        }

        public VertexInfo GetVertex(int x, int z)
        {
            return m_TerrainVertices[x + z * VertexCountX];
        }

        public GridInfo GetGrid(int x, int z)
        {
            return m_TerranGrids[x + z * GridX];
        }

        public void SetHeight(int x, int z, float h)
        {
            VertexInfo v = GetVertex(x, z);
            if (v != null)
            {
                v.height = h;
            }
            else
            {
                Debug.LogError("set  height x :" + x + " z :" + z + "falid");
            }
        }

        public float GetHeight(int x, int z)
        {
            VertexInfo v = GetVertex(x, z);
            if (v != null)
            {
                return v.height;
            }
            else
            {
                Debug.LogError("set  height x :" + x + " z :" + z + "falid");
            }

            return 0;
        }

        public void SetFlag(int index, bool bflag)
        {
            GridInfo v = m_TerranGrids[index];
            if (v != null)
            {
                v.IsMask = bflag;
            }
            else
            {
                Debug.LogError("set  index  :" + index);
            }
        }

    }


}