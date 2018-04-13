using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;

public class GameUtil
{
    /// <summary>
    /// 根据脸谱判断角色性别
    /// </summary>
    /// <param name="face"></param>
    /// <returns></returns>
    public static enmCharSex FaceToSex(uint face)
    {
        return face < 100 ? enmCharSex.MALE : enmCharSex.FEMALE;
    }

    public static bool GetWalkHeight(float x, float z, out float y)
    {
        y = 0.0f;
        return true;
        //const int VERY_HIGH = 1000;
        //var startPos = new Vector3(x, VERY_HIGH, z);
        //RaycastHit hit;
        //if (Physics.Raycast(startPos, Vector3.down, out hit, Mathf.Infinity, 1 << 14)) // 射线碰撞仅与地形生效
        //{
        //    y = hit.point.y;
        //    return true;
        //}

        //y = -1000;
        //return false;
    }

    public static string GetModelPath(uint itemID, uint faceID, bool ifJudgeSex = true)
    {
        var modelPath = "";
        var tbl = Table.Query<table.EquipDataBase>().FirstOrDefault(i => i.equipID == itemID);
        if (tbl == null) return modelPath;

        table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>((uint)tbl.apperMale);
        string male = (null != resDB) ? resDB.strPath : ""; ;
        resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>((uint)tbl.apperFemale);
        string female = (null != resDB) ? resDB.strPath : ""; ;
        if (ifJudgeSex)
        {
            modelPath = GameUtil.FaceToSex(faceID) == enmCharSex.MALE ? male : female;
        }
        else
        {
            if (tbl != null)
            {
                modelPath = male;
            }
        }
        return modelPath;
    }

    public static Vector3 S2CDirection(uint byDir)
    {
//         var rotation = (float)byDir / 255 * 360;
//         rotation = (90 + 360 - rotation) % 360;
        var rotation = (float)byDir / 256 * 360;
        rotation = 360 - rotation;
        return new Vector3(0, rotation+90, 0);
    }

    public static byte C2SDirection(Vector3 vecDir)
    {
        var az = vecDir.y;
        az = (90 + 360 - az) % 360;
        byte byDir = (byte)(az / 360.0f * 255);
        return byDir;
    }

    /// <summary>
    /// 临时速度换算
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static float GetSpeed(ushort speed)
    {
        return speed * 0.0038f;
    }

    // 浮点数非精确比较精度
    public const float treshold = 0.01f; 
    /// <summary>
    /// 非精确比较：大于
    /// </summary>
    /// <returns></returns>
    public static bool RoughlyGreatThan(float a, float b)
    {
        return a - b > treshold;
    }

    /// <summary>
    /// 非精确比较：等于
    /// </summary>
    /// <returns></returns>
    public static bool RoughlyEqual(float a, float b)
    {
        return (Math.Abs(a - b) < treshold);
    }
}

public class LayerUtil
{
    public static int ui = 5;
    public static int walkable = 9;
    public static int friend_role = 10;
    public static int enemy_role = 11;
    public static int friend_npc = 12;
    public static int enemy_npc = 13;
    public static int block = 14;

}
