using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// 程序逻辑层使用的坐标
/// </summary>
public class MapVector2 : IEquatable<MapVector2>
{
    const float SMALL_POS_OFFSET = 100.0f;
    public static readonly MapVector2 Invalid = new MapVector2(-1, -1);

    public float x { get; private set; }
    public float y { get; private set; }

    public static MapVector2 C2SCoordinate(Vector3 pos3)
    {
        return new MapVector2(pos3.x, -pos3.z);
    }
    public static Vector3 S2CCoordinate(MapVector2 pos2, float y)
    {
        return new Vector3(pos2.x, y, -pos2.y);
    }
    public static Vector3 S2CCoordinate(Vector2 pos2, float y)
    {
        return new Vector3(pos2.x, y, -pos2.y);
    }

    public static MapVector2 FromCoordinate(GameCmd.Coordinate x, GameCmd.Coordinate y)
    {
        float xx = x.integral + (float)x.mydecimal / SMALL_POS_OFFSET;
        float yy = y.integral + (float)y.mydecimal / SMALL_POS_OFFSET;

        return new MapVector2(xx, yy);
    }

    public static GameCmd.Coordinate ToCoordinate(float v)
    {
        var val = new GameCmd.Coordinate();
		val.integral = (ushort)v;
		val.mydecimal = (byte)((v - (float)val.integral) * SMALL_POS_OFFSET);
        return val;
    }

    public MapVector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public Vector2 ToVector2()
    {
        return new Vector2(this.x, this.y);
    }

    public static float Distance(MapVector2 a, MapVector2 b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
    }

    #region 运算符重载
    // 一元运算符
    public static MapVector2 operator -(MapVector2 me)
    {
        return new MapVector2(-me.x, -me.y);
    }

    // 二元运算符
    public static MapVector2 operator -(MapVector2 a, MapVector2 b)
    {
        return new MapVector2(a.x - b.x, a.y - b.y);
    }
    public static MapVector2 operator +(MapVector2 a, MapVector2 b)
    {
        return new MapVector2(a.x + b.x, a.y + b.y);
    }
    public static MapVector2 operator +(MapVector2 a, Vector2 b)
    {
        return new MapVector2(a.x + b.x, a.y + b.y);
    }
    public static MapVector2 operator +(Vector2 a, MapVector2 b)
    {
        return new MapVector2(a.x + b.x, a.y + b.y);
    }
    public static MapVector2 operator *(float d, MapVector2 a)
    {
        return new MapVector2(a.x * d, a.y * d);
    }
    public static MapVector2 operator *(MapVector2 a, float d)
    {
        return d * a;
    }

    // 逻辑运算符
    public static bool operator ==(MapVector2 me, MapVector2 other)
    {
        if (!object.Equals(me, null) && !object.Equals(other, null))
        {
            return me.x == other.x && me.y == other.y;
        }
        else
        {
            return (object.Equals(me, null) && object.Equals(other, null));
        }
    }
    public static bool operator !=(MapVector2 me, MapVector2 other)
    {
        return !(me == other);
    }
    #endregion

    public override int GetHashCode()
    {
        return this.x.GetHashCode() ^ this.y.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        if (object.ReferenceEquals(this, obj))
            return true;
        var other = obj as MapVector2;
        return this.Equals(other);
    }

    #region IEquatable<MapVector2> 成员

    public bool Equals(MapVector2 other)
    {
        return other != null && this.x == other.x && this.y == other.y;
    }

    #endregion

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }
}
