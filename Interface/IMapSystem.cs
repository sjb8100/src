//*************************************************************************
//	创建日期:	2016-1-18   17:45
//	文件名称:	imapsystem.cs
//  创 建 人:   Even	
//	版权所有:	Even.xu
//	说    明:	地图系统接口
//*************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Client
{
    // NPC数据
    public class NPCInfo
    {
        public int npcID = 0;
        public string name = "";
        public Vector2 pos = Vector2.zero;
        public bool bShow = false;
        /// <summary>
        /// 0是小类型 1是大类型
        /// </summary>
        public int type = 0;
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MapAreaInfo
    {
        public ushort x;
        public ushort z;
        public byte areaType;
    }

    // 阻挡类型定义
    [System.Flags]
    public enum TileType// : ushort // Unity not support
    {
	    #region 服务器使用
	    /// <summary>
	    /// 固体阻挡点
	    /// </summary>
	    TILE_SOLID_BLOCK = 0x01,
	    /// <summary>
	    /// 飞行阻挡点
	    /// </summary>
	    TILE_FLY_BLOCK = 0x02,
	    /// <summary>
	    /// 人物或者Npc阻挡
	    /// </summary>
	    TILE_ENTRY_BLOCK = 0x04,
	    /// <summary>
	    /// 道具阻挡
	    /// </summary>
	    TILE_OBJECT_BLOCK = 0x08,
	    #endregion

	    #region 非服务器使用
	    /// <summary>
	    /// 陆地交通运输
	    /// </summary>
	    TILE_ROAD_TRAFFIC_BLOCK = 0x100,
	    /// <summary>
	    /// 水上交通运输
	    /// </summary>
	    TILE_NAVY_TRAFFIC_BLOCK = 0x200,
	    /// <summary>
	    /// 跳跃
	    /// </summary>
	    TILE_JUMP_BLOCK = 0x400,
	    /// <summary>
	    /// 2D遮罩
	    /// </summary>
	    TILE_GROUND_BLOCK = 0x800,
	    #endregion


	    TileType_None = 0,
	    TileType_Walk_BLOCK = TILE_SOLID_BLOCK | TILE_FLY_BLOCK,
    }

    public delegate void MapEnterZoneCallback(int nZoneID);

    // 地图系统
    public interface IMapSystem
    {
        // 释放接口
        void Release();

        /**
        @brief 进入地图
        @param nMapID 地图ID
        @param pos 主角位置
        */
        void EnterMap(uint nMapID, Vector3 pos);

        /**
        @brief 退出当前场景
        @param 
        */
        void ExitMap();

        /**
        @brief 获取MapID
        */
        uint GetMapID();

        // 获取地图名称
        string GetMapName();

        bool IsFirstLoad();

        string GetMapTextureName();
        /**
        @brief FindPath 寻路算法
        @param curPos 当前位置
        @param tarPos 目标位置
        @param[out] path 输出路径
        */
        bool FindPath(Vector2 curPos, Vector2 tarPos, out List<Vector2> path);

        // 寻路回调
        void AddFindPathCallback(Action<List<Vector2>> findPath);
        // 寻路回调
        void RemoveFindPathCallback(Action<List<Vector2>> findPath);

        /**
        @brief 获取地图上两点距离(计算阻挡)
        @param curPos 当前位置 使用地图坐标
        @param tarPos 目标位置 使用地图坐标
        @param tileType 
        */
        bool CalcDistance(Vector2 curPos, Vector2 tarPos, out float fDistance, TileType tileType = TileType.TileType_None);

        /**
        @brief 屏幕点击位置转换到场景坐标
        @param pos 屏幕位置
        @param scenePos 场景位置
        */
        bool GetScenePos(Vector2 pos, out Engine.TerrainInfo terrainInfo);

        /**
        @brief 检测是否可以行走
        @param pos 场景位置
        */
        bool CanWalk(Vector3 pos);

        // 加载阻挡数据
        bool LoadObstacle(string path);

        //获取地图大小
        Vector2 GetMapSize();

        // 获取地图加载进度
        float Process { get; set; }

        // 获取NPC位置
        bool GetClienNpcPos(int baseid,out Vector2 pos);

        // 获取NPC位置
        bool GetAllNpcInfo(out List<NPCInfo> lstNpc);
        /// <summary>
        /// 副本专用 是否进入某一区域
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>-1 false </returns>
        int IsEnterZone(UnityEngine.Vector3 pos);
        /**
        @brief 添加草扰动力
        @param pos 力中心点位置
        @param fForce 力大小
        @param fRadius 力影响范围
        */
        void AddGrassWaveForce(Vector3 pos, float fForce, float fRadius);

        /**
        @brief 开启草地扰动(全局设置)
        @param 
        */
        void EnableGrassForce(bool bEnable);

        /**
        @brief 查找地图间连接点
        @param srcMapID 源地图
        @param destMapID 目标地图
        @param[out] 地图中连接传送点位置
        @return 成功与否
        */
        bool FindMapLinkPoint(uint srcMapID, uint destMapID, out Vector3 pos);


        MapAreaType GetAreaTypeByPos(Vector3 pos);

        void SetMapAreaVisible(bool vivible);

        void SetMap9Grid(bool vivible);

        int GetMapGridId(Vector3 pos);

        //请求服务器跳转地图
        void RequestEnterMap(uint nmapId,uint cost =0);

        void Update(float dt);

        void SetEnterZoneCallback(MapEnterZoneCallback callback);

        float GetTerrainHeight(Vector3 pos);
        /// <summary>
        /// 获取地图的xml信息
        /// </summary>
        /// <returns></returns>
        Dictionary<int, List<NPCInfo>> GetMapXmlInfo();

        void TraceServerPos(bool add, int x, int y);
    }
}
