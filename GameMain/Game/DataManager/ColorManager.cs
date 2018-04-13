/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.DataManager
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ColorManager
 * 版本号：  V1.0.0.0
 * 创建时间：7/13/2017 11:34:07 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ColorManager : IManager
{
    #region IManager Method
    public void Initialize()
    {

    }

    public void Reset(bool depthClearData = false)
    {
        if (depthClearData)
        {
            m_dicColorCache = null;
            ColorUtil.Reset();
        }
    }

    public void Process(float deltaTime)
    {
    
    }

    public void ClearData()
    {

    }
    #endregion


    #region OP
    #endregion

    #region ItemQuality 物品品质

    /// <summary>
    /// 根据物品品质获取名称颜色值
    /// </summary>
    /// <param name="qType"></param>
    /// <param name="tips"></param>
    /// <returns></returns>
    public static string GetNGUIColorByQuality(int quality, bool tips = false)
    {
        ItemDefine.ItemQualityType qType = ItemDefine.ItemQualityType.White;
        if (quality > (int)ItemDefine.ItemQualityType.White && quality < (int)ItemDefine.ItemQualityType.Max)
        {
            qType = (ItemDefine.ItemQualityType) quality;
        }
        return GetNGUIColorByQuality(qType,tips);
    }

    public static Color GetColorByQuality(ItemDefine.ItemQualityType qType)
    {
        Color color = Color.white;
        switch (qType)
        {
            case ItemDefine.ItemQualityType.White:
                {
                    color = ColorManager.GetColor32OfType(ColorType.JZRY_Whitle);
                }
                break;
            case ItemDefine.ItemQualityType.Yellow:
                {
                    color = ColorManager.GetColor32OfType(ColorType.JZRY_Yellow);
                }
                break;
            case ItemDefine.ItemQualityType.Green:
                {
                    color = ColorManager.GetColor32OfType(ColorType.JZRY_Green);
                }
                break;
            case ItemDefine.ItemQualityType.Blue:
                {
                    color = ColorManager.GetColor32OfType(ColorType.JZRY_Blue);
                }
                break;
            case ItemDefine.ItemQualityType.Purple:
                {
                    color = ColorManager.GetColor32OfType(ColorType.Purple);
                }
                break;
            case ItemDefine.ItemQualityType.Orange:
                {
                    color = ColorManager.GetColor32OfType(ColorType.Orange);
                }
                break;
        }
        return color;
    }

    /// <summary>
    /// 根据物品品质获取名称颜色值
    /// </summary>
    /// <param name="qType"></param>
    /// <param name="tips"></param>
    /// <returns></returns>
    public static string GetNGUIColorByQuality(ItemDefine.ItemQualityType qType, bool tips = false)
    {
        string nguiColor = "";
        switch (qType)
        {
            case ItemDefine.ItemQualityType.White:
                {
                    if (!tips)
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Whitle);
                    }
                    else
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Tips_White);
                    }
                }
                break;
            case ItemDefine.ItemQualityType.Yellow:
                {
                    if (!tips)
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Yellow);
                    }
                    else
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Tips_Yellow);
                    }
                }
                break;
            case ItemDefine.ItemQualityType.Green:
                {
                    if (!tips)
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Green);
                    }
                    else
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Tips_Green);
                    }
                }
                break;
            case ItemDefine.ItemQualityType.Blue:
                {
                    if (!tips)
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Blue);
                    }
                    else
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Tips_Blue);
                    }
                }
                break;
            case ItemDefine.ItemQualityType.Purple:
                {
                    if (!tips)
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Purple);
                    }
                    else
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Tips_Purple);
                    }
                }
                break;
            case ItemDefine.ItemQualityType.Orange:
                {
                    if (!tips)
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Oranger);
                    }
                    else
                    {
                        nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Tips_Orange);
                    }
                }
                break;
            default:
                nguiColor = ColorManager.GetNGUIColorOfType(ColorType.JZRY_Whitle);
                break;
        }
        return nguiColor;
    }

    #endregion

    #region Static
    private static Dictionary<int, int> m_dicColorCache = null; 
    /// <summary>
    /// 根据类型读取颜色配置值
    /// </summary>
    /// <param name="cType"></param>
    /// <returns></returns>
    public static int GetColorIntByType(ColorType cType)
    {
        if (null == m_dicColorCache)
            m_dicColorCache = new Dictionary<int, int>();
        int colorInt = 0;
        int colorType = (int)cType;
        if (!m_dicColorCache.TryGetValue(colorType,out colorInt))
        {
            colorInt = GameTableManager.Instance.GetGlobalConfig<int>("GameColor", cType.ToString());
            m_dicColorCache.Add(colorType, colorInt);
        }
        return colorInt;
    }

    /// <summary>
    /// 根据颜色值回去颜色
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color GetColorOfColorInt(int color)
    {
        return ColorUtil.GetColor32OfColorInt(color);
    }

    /// <summary>
    /// 根据颜色类型获取color
    /// </summary>
    /// <param name="cType"></param>
    /// <returns></returns>
    public static Color GetColor32OfType(ColorType cType)
    {
        return GetColorOfColorInt(GetColorIntByType(cType));
    }

    /// <summary>
    /// 根据颜色类型获取NGUI颜色值
    /// </summary>
    /// <param name="cType"></param>
    /// <returns></returns>
    public static string GetNGUIColorOfType(ColorType cType)
    {
        return ColorUtil.GetNGUIColor(GetColorIntByType(cType));
    }

    /// <summary>
    /// 获取文本颜色
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetColorString(ushort r, ushort g, ushort b, ushort a, string str)
    {
        return ColorUtil.GetColorString(r, g, b, a, str);
    }

    /// <summary>
    /// 获取文本颜色
    /// </summary>
    /// <param name="cType"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetColorString(ColorType cType,string str)
    {
        return ColorUtil.GetColorString(GetColorIntByType(cType),str);
    }

    /// <summary>
    /// 获取文本颜色
    /// </summary>
    /// <param name="c"></param>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetColorString(Color c, string str)
    {
        return GetColorString((ushort)c.r, (ushort)c.g, (ushort)c.b, (ushort)c.a, str);
    }
    #endregion

}