using UnityEngine;
using System.Collections;
//using Cmd.MoveUserCmd;

/// <summary>
/// 地图格子，主要用来做阻挡判断和寻路。
/// 和服务器之间的位置通信用<see cref="Cmd.Pos"/>
/// </summary>
public struct MapGrid : System.IEquatable<MapGrid>
{
    /// <summary>
    /// 每个格子<see cref="MapGrid"/>宽度
    /// </summary>
    public const float Width = 1f;
    /// <summary>
    /// 每个格子<see cref="MapGrid"/>高度
    /// </summary>
    public const float Height = Width;

    public int x;
    public int z;

    public MapGrid(Vector3 worldPosition)
    {
        this.x = (int)(worldPosition.x / MapGrid.Width);
        this.z = (int)(worldPosition.z / MapGrid.Height);
    }

    // 防止报错，先保留老接口
    //public MapGrid(Cmd.Pos poscm)
    //{
    //	this.x = (int)(poscm.x / (100 * Width));
    //	this.z = (int)(poscm.y / (100 * Height));
    //}

    public MapGrid(int x, int y)
    {
        this.x = x;
        this.z = y;
    }

    // 转换成地图坐标
    public static MapVector2 GetMapPos(MapGrid grid)
    {
        return new MapVector2(
            (Mathf.Clamp(grid.x, 0, MapObstacle.Instance.Width - 1) + 0.5f) * Width,
            (Mathf.Clamp(grid.z, 0, MapObstacle.Instance.Height - 1) + 0.5f) * Height);
    }

    public static Vector2 GetMapPosV2(MapGrid grid)
    {
        return new Vector2(
            (Mathf.Clamp(grid.x, 0, MapObstacle.Instance.Width - 1) + 0.5f) * Width,
            (Mathf.Clamp(grid.z, 0, MapObstacle.Instance.Height - 1) + 0.5f) * Height);
    }

    // 转换成格子坐标
    public static MapGrid GetMapGrid(MapVector2 pos)
    {
        return new MapGrid() { x = (int)(pos.x / Width), z = (int)(pos.y / Height) };
    }

    public static void GetMapGrid(Vector2 pos, ref MapGrid grid)
    {
        if (grid != null)
        {
            grid.x = (int)(pos.x / Width);
            grid.z = (int)(pos.y / Height);
        }
    }
    public static void GetMapGrid(Vector2 pos, ref int x, ref int z)
    {

        x = (int)(pos.x / Width);
        z = (int)(pos.y / Height);

    }
    //public static implicit operator Cmd.Pos(MapGrid grid)
    //{
    //	return new Cmd.Pos()
    //	{
    //		x = (int)((grid.x + 0.5f) * (100 * Width)),
    //		y = (int)((grid.z + 0.5f) * (100 * Height))
    //	};
    //}

    #region Equatable
    public static bool operator ==(MapGrid a, MapGrid b)
    {
        //if (!(a is MapGrid) || !(b is MapGrid))
        //{
        //    return false;
        //}
        //if (System.Object.ReferenceEquals(a, b))
        //{
        //    return true;
        //}
        //if (((object)a == null) || ((object)b == null))
        //    return false;
        return a.x == b.x && a.z == b.z;
    }
    public static bool operator !=(MapGrid a, MapGrid b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return obj is MapGrid ? this == (MapGrid)obj : false;
    }

    public override int GetHashCode()
    {
        return (int)(this.x ^ this.z);
    }

    #region IEquatable<Pos> Members

    public bool Equals(MapGrid other)
    {
        return this == other;
    }

    #endregion

    #endregion

    public override string ToString()
    {
        return string.Format("Grid({0}, {1})", x, z);
    }
}
