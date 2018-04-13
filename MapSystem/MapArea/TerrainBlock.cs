using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace MapSystem
{
    public class TerrainBlock
    {
        public static uint GRID_NUM = 32;
        public static float GRID_LEN = 1.0f;

        private GameObject m_GameObject = null;
        private Mesh m_Mesh = null;
        protected MeshFilter m_meshFilter;

        LayerData m_layerData;
        int m_x;
        int m_z;

        public bool bUpdate = false;
        private readonly float m_TerrainBlockHeight = 0.3f;
        public TerrainBlock(LayerData data, Transform parent, int x, int z)
        {
            m_layerData = data;
            m_GameObject = new GameObject();
            m_GameObject.name = string.Format("x{0}_z{1}", x, z);
            m_GameObject.transform.parent = parent;
            m_GameObject.hideFlags = HideFlags.NotEditable;

            m_meshFilter = m_GameObject.AddComponent<MeshFilter>();
            m_meshFilter.mesh = m_Mesh = CreatePlaneMesh(1, 1);
            m_GameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Custom/ForMapEditor"));
            m_GameObject.AddComponent<MeshCollider>();

            m_x = x;
            m_z = z;
            Vector3 pos = Vector3.zero;
            pos.x = GetOffsetX(x);
            pos.z = -GetOffsetX(z);
            m_GameObject.transform.position = pos;

            bUpdate = true;
        }

        float GetOffsetX(int x)
        {
            return (x + 0.5f) * TerrainBlock.GRID_LEN * TerrainBlock.GRID_NUM;
        }

        float GetOffsetZ(int z)
        {
            return (z + 0.5f) * TerrainBlock.GRID_LEN * TerrainBlock.GRID_NUM;
        }

        Mesh CreatePlaneMesh(int nWidth, int nHeight)
        {
            Mesh mesh = new Mesh();
            /*     1_____2
                   |     |
                   |     |
                   |_____|
                   0     3
           */
            float width = nWidth * 0.5f;
            float height = nHeight * 0.5f;

            //顶点坐标
            Vector3[] vertices = new Vector3[]
        {
            new Vector3(-width, 0,  -height),
            new Vector3( -width, 0, height),
            new Vector3(width, 0,  height),
            new Vector3(width, 0, -height),
        };
            //UV坐标
            Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0),
        };
            //三角形索引
            int[] triangles = new int[]
        {
            0, 1, 3,
            1, 2, 3,
        };


            mesh.Clear();
            mesh.vertices = vertices;

            mesh.uv = uv;
            mesh.triangles = triangles;


            //重置法线
            mesh.RecalculateNormals();
            //重置范围
            mesh.RecalculateBounds();
            return mesh;
        }

        public void UpdateTerrainBlock()
        {
            if (bUpdate == false)
            {
                return;
            }

            if (m_layerData == null)
            {
                return;
            }
            int index = m_x + m_z * (int)m_layerData.BlockNumX;

            float offsetx = GetOffsetX(m_x);
            float offsetz = -GetOffsetX(m_z);
            int nStartX = 0;
            int nEndX = 0;
            int nStartZ = 0;
            int nEndZ = 0;
            GetNodeBound(m_layerData, m_x, m_z, ref nStartX, ref nEndX, ref nStartZ, ref nEndZ);

            Clear();
            Begin();

            //  Debug.Log("UpdateBlock set flag x" + m_x + " z" + m_z);

            List<Vector3> vertices = new List<Vector3>();

            List<Vector2> uv = new List<Vector2>();
            List<int> triangles = new List<int>();

            int verCount = 0;
            int triCount = 0;
            int uvCount = 0;

            float m_uGridLenX = TerrainBlock.GRID_LEN;
            float m_uGridLenZ = TerrainBlock.GRID_LEN;

            for (int z = nStartZ; z < nEndZ; z++)
            {
                for (int x = nStartX; x < nEndX; x++)
                {
                    GridInfo grid = m_layerData.GetGrid(x, z);
                    if (grid.IsMask == false)
                    {
                        continue;
                    }
                    float fHeight1 = m_layerData.GetHeight(x, z);
                    float fHeight2 = m_layerData.GetHeight(x, z + 1);
                    float fHeight3 = m_layerData.GetHeight(x + 1, z + 1);
                    float fHeight4 = m_layerData.GetHeight(x + 1, z);

                    // 上面去一点地形不容易搓出来
                    fHeight1 += m_TerrainBlockHeight;
                    fHeight2 += m_TerrainBlockHeight;
                    fHeight3 += m_TerrainBlockHeight;
                    fHeight4 += m_TerrainBlockHeight;

                    /* 
                       0------1
                       |      |
                       |      |
                       3------2
                    */
                    // 一象限变四象限
                    Vector3 pos1 = new Vector3(x * m_uGridLenX - offsetx, fHeight1, (-z) * m_uGridLenZ - offsetz);

                    Vector3 pos2 = new Vector3((x + 1) * m_uGridLenX - offsetx, fHeight4, (-z) * m_uGridLenZ - offsetz);
                    Vector3 pos3 = new Vector3((x + 1) * m_uGridLenX - offsetx, fHeight3, -(z + 1) * m_uGridLenZ - offsetz);
                    Vector3 pos4 = new Vector3(x * m_uGridLenX - offsetx, fHeight2, -(z + 1) * m_uGridLenZ - offsetz);


                    vertices.Add(pos1);
                    vertices.Add(pos2);
                    vertices.Add(pos3);
                    vertices.Add(pos4);

                    Vector2 uv1 = new Vector2(0, 0);
                    Vector2 uv2 = new Vector2(0, 1 / GridDef.GridSize);
                    Vector2 uv3 = new Vector2(1 / GridDef.GridSize, 1 / GridDef.GridSize);
                    Vector2 uv4 = new Vector2(1 / GridDef.GridSize, 0);
                    uv.Add(uv1);
                    uv.Add(uv2);
                    uv.Add(uv3);
                    uv.Add(uv4);


                    // 一象限变四象限

                    triangles.Add(verCount + 2);
                    triangles.Add(verCount);
                    triangles.Add(verCount + 1);
                    triangles.Add(verCount + 3);
                    triangles.Add(verCount);
                    triangles.Add(verCount + 2);


                    verCount += 4;
                    uvCount += 4;
                    triCount += 6;
                }
            }
            Color color = Color.white;
            if (m_layerData.layerType == BaseLayer.LayerType.eLayerBlock)
            {
                color = Color.red;
            }
            else if (m_layerData.layerType == BaseLayer.LayerType.eLayerPK)
            {
                color = Color.blue;
            }
            else if (m_layerData.layerType == BaseLayer.LayerType.eLayerBoss)
            {
                color = Color.yellow;
            }
            else if (m_layerData.layerType == BaseLayer.LayerType.eLayerSafe)
            {
                color = Color.green;
            }
            else if (m_layerData.layerType == BaseLayer.LayerType.eLayerBattle)
            {
                color = Color.cyan;
            }else if (m_layerData.layerType == BaseLayer.LayerType.eLayer9Grid)
            {
                if (m_x % 2 == 0 )
                {
                    if (m_z % 2 == 0)
                    {
                        color = Color.green;
                    }
                    else
                    {
                        color = Color.red;
                    }
                }
                else
                {
                    if (m_z % 2 == 0)
                    {
                        color = Color.red;
                    }
                    else
                    {
                        color = Color.green;
                    }
                }
            }
            color.a = 0.5f;

            SetColor(color);
            Position(ref vertices);
            Triangles(ref triangles);
            End();
            bUpdate = false;
        }

        public static bool GetNodeBound(LayerData layerData, int width, int height, ref int nStartX, ref int nEndX, ref int nStartZ, ref int nEndZ)
        {
            if (height < 0 || height > layerData.BlockNumZ)
                return false;
            if (width < 0 || width > layerData.BlockNumX)
                return false;

            nStartX = width * (int)TerrainBlock.GRID_NUM;
            nStartZ = height * (int)TerrainBlock.GRID_NUM;
            nEndX = nStartX + (int)TerrainBlock.GRID_NUM;
            nEndZ = nStartZ + (int)TerrainBlock.GRID_NUM;

            return true;
        }

        void Clear()
        {
            if (m_Mesh)
            {
                m_Mesh.Clear();
                if (m_GameObject != null)
                {
                    MeshCollider meshCollider = m_GameObject.GetComponent<MeshCollider>();
                    if (meshCollider != null)
                        GameObject.DestroyImmediate(meshCollider);
                }
            }
        }

        public void Begin(string material = "", string texture = "")
        {
            if (m_GameObject == null)
            {
                return;
            }
            m_GameObject.GetComponent<MeshFilter>().mesh = m_Mesh;

            if (material != "")
            {
                MeshRenderer pMeshRenderer = m_GameObject.GetComponent<MeshRenderer>();
                if (pMeshRenderer.sharedMaterial.name != material)
                {
                    pMeshRenderer.sharedMaterial.shader = Shader.Find(material);
                }
            }
        }
        public void SetColor(Color c)
        {
            MeshRenderer pMeshRenderer = m_GameObject.GetComponent<MeshRenderer>();
            pMeshRenderer.sharedMaterial.SetColor("_Color", c);
        }

        public void Position(ref Vector3[] vertices)
        {
            if (m_Mesh != null)
            {
                m_Mesh.vertices = vertices;
            }
        }
        public void Position(ref List<Vector3> vertices)
        {
            if (m_Mesh != null)
            {
                m_Mesh.vertices = vertices.ToArray();
            }
        }
        public void TextureCoord(ref List<Vector2> uv)
        {
            if (m_Mesh != null)
            {
                m_Mesh.uv = uv.ToArray();
            }
        }
        public void TextureCoord(ref Vector2[] uv)
        {
            if (m_Mesh != null)
            {
                m_Mesh.uv = uv;
            }
        }
        public void Triangles(ref int[] triangles)
        {
            if (m_Mesh != null)
            {
                m_Mesh.triangles = triangles;
            }
        }

        public void Triangles(ref List<int> triangles)
        {
            if (m_Mesh != null)
            {
                m_Mesh.triangles = triangles.ToArray();
            }
        }
        public void End()
        {
            if (m_Mesh != null)
            {
                m_Mesh.RecalculateNormals();
                m_Mesh.RecalculateBounds();

                MeshCollider meshCollider = m_GameObject.GetComponent<MeshCollider>();
                if (meshCollider == null)
                {
                    m_GameObject.AddComponent<MeshCollider>();
                }
                else
                {
                    Debug.Log("error");
                }
            }
        }
    }
}