//*************************************************************************
//	创建日期:	2016/10/12 9:55:40
//	文件名称:	QuadMesh
//   创 建 人:   zhuidanyu	
//	版权所有:	中青宝
//	说    明:	小地图mesh修改
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine.Utility;
using Client;
class QuadMesh : MonoBehaviour
{
    MeshFilter m_MeshFilter;

    MapDataManager MapData
    {
        get
        {
            return DataManager.Manager<MapDataManager>();
        }
    }
    Material m_mapMat;

    private Vector2[] m_uv2 = new Vector2[4];
    private Vector2[] m_uv1 = new Vector2[4];
 
    Mesh m_quadMesh;

    public void Awake()
    {
        Vector3 locaPos = transform.localPosition;
        transform.localPosition = new Vector3( locaPos.x - MapData.m_nBgWidth / 2 , locaPos.y - MapData.m_nBgHeight / 2 , 0 );
        Mesh ms = new Mesh();

        m_uv2[0] = new Vector2( 0 , 0 );
        //右上
        m_uv2[1] = new Vector2( 1 , 1 );
        //右下
        m_uv2[2] = new Vector2( 1 , 0 );
        //左上
        m_uv2[3] = new Vector2( 0 , 1 );

        Vector3[] vecList = new Vector3[4];
        for ( int i = 0; i < 4; ++i )
        {
            vecList[i] = new Vector3( m_uv2[i].x , m_uv2[i].y , 0 );
        }
        ms.vertices = vecList;

        int[] trangles = new int[6];
        trangles[0] = 0;
        trangles[1] = 1;
        trangles[2] = 2;
        trangles[3] = 0;
        trangles[4] = 3;
        trangles[5] = 1;
        ms.triangles = trangles;

        m_uv1[0] = new Vector2(0 ,0 );
        //右上
        m_uv1[1] = new Vector2(1 , 1 );
        //右下
        m_uv1[2] = new Vector2( 1 , 0 );
        //左上
        m_uv1[3] = new Vector2( 0 , 1 );
        ms.uv = m_uv1;

        // mask uv
        ms.uv2 = m_uv2;

        m_MeshFilter = gameObject.GetComponent<MeshFilter>();
        if ( m_MeshFilter == null )
        {
            m_MeshFilter = gameObject.AddComponent<MeshFilter>();
        }
        m_MeshFilter.mesh = ms;


        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        if ( mr == null )
        {
            mr = gameObject.AddComponent<MeshRenderer>();

            Material mt = new Material( Resources.Load( "Shaders/Custom/Mask" ) as Shader );
            mr.material = mt;
        }
        m_mapMat = mr.material;
        float a = GameTableManager.Instance.GetGlobalConfig<float>("MiniMapAlpha");
        if( a == 0)
        {
            a = 0.7f;
        }
        m_mapMat.SetFloat("_Alpha", a);
    }
    public void OnGUI()
    {

    }
    float scale = 1;
    public void Start()
    {
        List<Vector3> vecList = new List<Vector3>();
        Mesh ms = m_MeshFilter.mesh;
        foreach ( var vec in ms.vertices )
        {
            Vector3 newVec = new Vector3( vec.x * MapData.m_nBgWidth , vec.y * MapData.m_nBgHeight , 0 );
            vecList.Add( newVec );
        }
        ms.SetVertices( vecList );
        m_quadMesh = ms;
    }
   
    public void Update()
    {
        if ( MapData.MapTextureSize == Vector2.zero )
        {
            return;
        }
        if(MainPlayerHelper.GetMainPlayer() == null)
        {
            return;
        }
        Vector2 playerUV = MapData.GetTextUVByTopView();

        float xDelta = MapData.m_nBgWidth *scale* 1.0f / 2 / MapData.MapTextureSize.x;
        float yDelta = MapData.m_nBgHeight * 1.0f*scale / 2 / MapData.MapTextureSize.y;
     
        // Vector2[] uvArray = new Vector2[4];
  
        //左下
     //   m_uv1[0] = new Vector2( playerUV.x - xDelta , playerUV.y - xDelta );
        m_uv1[0].x = playerUV.x - xDelta;
        m_uv1[0].y = playerUV.y - xDelta;

        //右上
       // m_uv1[1] = new Vector2( playerUV.x + xDelta , playerUV.y + xDelta );
        m_uv1[1].x = playerUV.x + xDelta;
        m_uv1[1].y = playerUV.y + xDelta;


        //右下
       // m_uv1[2] = new Vector2( playerUV.x + xDelta , playerUV.y - xDelta );
        m_uv1[2].x = playerUV.x + xDelta;
        m_uv1[2].y = playerUV.y - xDelta;


        //左上
        //m_uv1[3] = new Vector2( playerUV.x - xDelta , playerUV.y + xDelta );
        m_uv1[3].x = playerUV.x - xDelta;
        m_uv1[3].y = playerUV.y + xDelta;

        if ( m_MeshFilter != null )
        {
            if(m_quadMesh != null)
            {
                m_quadMesh.uv = null;
                m_quadMesh.uv = m_uv1;
                m_quadMesh.uv2 = m_uv2;
            }

        }

    }


}

