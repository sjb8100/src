using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

/** 颜色类型 ForNGUI Color **/
public enum ColorType : int
{
    None = 0,
    //灰色 一般 = "[808080]";
    Gray = 0x808080,
    //红色 = "[FF0000]";
    Red = 0xFF0000,
    //白色 良好 = "[FFFFFF]";
    White = 0xFFFFFF,
    //绿色 精良 = "[00FF00]";
    Green = 0x00FF00,
    //淡绿色 = "[64C700]";
    Ondine = 0x64C700,
    //橙色 尖端 = "[FF4300]";
    Orange = 0xFF4300,
    //蓝色 优秀 = "[0000FF]";
    Blue = 0x0000FF,
    //紫色 完美 = "[FF00FF]";
    Purple = 0xFF00FF,
    //暗金色 黄金 = "[FFFF00]";
    DarkGolden = 0xFFFF00,
    //青色 传说= "[00B0F0]"
    Cyan = 0x00B0F0,
    //茶色 史诗 = "[948A54]";
    DarkBrown = 0x948A54,
    //暗红色 极品 = "[C00000]";
    DarkRed = 0xC00000,
    //淡黄色 = "[FFFF99]";
    FaintYellow = 0xFFFF99,
    //暗黄色 = "[C8C800]";
    DarkYellow = 0xC8C800,
    //品红 洋红色 = "[FF00FF]";
    Magenta = 0xFF00FF,
    //黄色 = "[FFFF00]";
    Yellow = 0xFFFF00,
    //粉色 = "[FF99CC]";
    Pink = 0xFF99CC,
    //棕色 = "[A05023]";
    Brown = 0xA05023,
    //淡黄
    LightYellow = 0xFFF799,
    //桔黄
    OrangeYellow = 0xF39800,
    //蓝色
    Blue_1 = 0x00A0E9,
    //亮蓝
    BrilliantBlue = 0x00FFFF,
    //紫红
    Wine = 0xE4007F,
    //浅黄
    LightYellow_1 = 0xF3A65E,
    //紫
    Purple_1 = 0x601986,
    //深褐
    DarkBrown_1 = 0x201107,
    //深绿
    DarkGreen = 0x005e15,
    //深蓝
    DarkBlue = 0x004986,
    //黄
    Yellow_1 = 0xfff45c,
    TabLight = 0x3E475A,
    TabDark = 0xfff45f,

    JZRY_Txt_Black,
    JZRY_Txt_LightBlue,
    JZRY_Txt_Red,
    JZRY_Txt_White,
    JZRY_Txt_NotMatchRed,
    JZRY_Txt_OrangeH,

    JZRY_Whitle = 0x505050,
    JZRY_Yellow,
    JZRY_Green = 0x097f1d,
    JZRY_Blue = 0x0072ff,
    JZRY_Purple = 0x9429fb,
    JZRY_Oranger = 0xce8d00,
    JZRY_Gray = 0x445a6c,

    JZRY_Tips_Light = 0xbad4dc,
    JZRY_Tips_White = 0xc6c6c6,
    JZRY_Tips_Yellow,
    JZRY_Tips_Green = 0x22dd22,
    JZRY_Tips_Blue = 0x5cb4ff,
    JZRY_Tips_Orange = 0xffba00,
    JZRY_Tips_Purple = 0xe67aff,


    //血条字体颜色
    JSXT_Enemy_Red    = 0xff3737,
    JSXT_Enemy_Yellow = 0xffff00,
    JSXT_Enemy_Gray   = 0xb6b6b6,
    JSXT_Enemy_White  = 0xffffff,

    JSXT_Clan         = 0xffd700,
    JSXT_ZhaoHuanWu   = 0x57c2ff,
    JSXT_ZhanHun      = 0xffdf7f,

    JSXT_CaiJiWu      = 0x00c6ff,
    JSXT_ZiYuanDian   = 0x00c6ff,
    JSXT_ChuanSongZhen= 0xff8a00,
    JSXT_NpcCheFu     = 0xff8a00,

    //皇令特殊色
    JZRY_Noble_Blue = 0x29538d,
    JZRY_Noble_MiddleBlue = 0x407bae,
    JZRY_Noble_LightBlue = 0x34a1b8,
    JZRY_Noble_Pink = 0x983fb0,
    JZRY_Noble_MiddlePink = 0xa854aa,
    JZRY_Noble_LightPink= 0xb16aad,
    JZRY_Noble_Yellow = 0xaa7326,
    JZRY_Noble_MiddleYellow = 0xbe882a,
    JZRY_Noble_LightYellow = 0xbf8a2e,

    
}

/** 颜色类型 ForNGUI Color **/
public enum ColorType3 : int
{
    None = 0,
    JZRY_Txt_Black ,
    JZRY_Txt_LightBlue ,
    JZRY_Txt_Red ,
    JZRY_Txt_White ,
    JZRY_Txt_NotMatchRed ,
    JZRY_Txt_OrangeH,

    JZRY_Whitle,
    JZRY_Yellow ,
    JZRY_Green,
    JZRY_Blue,
    JZRY_Purple,
    JZRY_Oranger,

    JZRY_Tips_Light,
    JZRY_Tips_White ,
    JZRY_Tips_Green,
    JZRY_Tips_Blue ,
    JZRY_Tips_Orange,
    JZRY_Tips_Purple,


    //血条字体颜色
    JSXT_Enemy_Red,
    JSXT_Enemy_Yellow,
    JSXT_Enemy_Gray,
    JSXT_Enemy_White,

    JSXT_Clan,
    JSXT_ZhaoHuanWu,
    JSXT_ZhanHun,

    JSXT_CaiJiWu,
    JSXT_ZiYuanDian,
    JSXT_ChuanSongZhen,
    JSXT_NpcCheFu,

}

public class ColorUtil
{
    #region NGUI颜色符号标示
    //颜色结束符号
    public const string COLOR_END_MASK = "[-]";
    //颜色开始符号（左边）
    public const string COLOR_START_MARK_LEFT = "[";
    //颜色开始符号（右边）
    public const string COLOR_START_MARK_RIGHT = "]";
    #endregion

    #region 颜色值获取解析
    
    /// <summary>
    /// 根据颜色类型获取NGUI颜色字符串 例：[ff0000]
    /// </summary>
    /// <param name="colorType"></param>
    /// <returns></returns>
    public static string GetNGUIColor(ColorType colorType)
    {
        return GetNGUIColor((int)colorType);
    }

    private static Dictionary<int, string> m_dicCacheNGUIColor = null;
    private static StringBuilder colorStriBuilder = new StringBuilder();
    /// <summary>
    /// 根据颜色类型获取NGUI颜色字符串 例：[ff0000]
    /// </summary>
    /// <param name="colorType"></param>
    /// <returns></returns>
    public static string GetNGUIColor(int color)
    {
        if (null == m_dicCacheNGUIColor)
            m_dicCacheNGUIColor = new Dictionary<int, string>();
        string nguiColor = "";
        if (!m_dicCacheNGUIColor.TryGetValue(color,out nguiColor))
        {
            int colorInt = color & 0xFFFFFF;
            colorStriBuilder.Remove(0, colorStriBuilder.Length);
            colorStriBuilder.Append(COLOR_START_MARK_LEFT);
            colorStriBuilder.Append(colorInt.ToString("x6"));
            colorStriBuilder.Append(COLOR_START_MARK_RIGHT);
            nguiColor = colorStriBuilder.ToString();
            m_dicCacheNGUIColor.Add(color, nguiColor);
        }
        return nguiColor;
    }

    public static void Reset()
    {
        m_dicCacheNGUIColor = null;
    }
    public static string GetColorString(ColorType type,string str)
    {
        return GetColorString((int)type,str);
    }

    public static string GetColorString (int color,string str)
    {
        string ct = GetNGUIColor(color);
        return ct + str + COLOR_END_MASK;
    }

    public static string GetColorString(ushort r, ushort g, ushort b, ushort a, string str)
    {
        string R = Convert.ToString(r, 16);
        if (R == "0")
            R = "00";
        string G = Convert.ToString(g, 16);
        if (G == "0")
            G = "00";
        string B = Convert.ToString(b, 16);
        if (B == "0")
            B = "00";
        string A = Convert.ToString(a, 16);
        if (A == "0")
            A = "00";
        string HexColor = COLOR_START_MARK_LEFT + R + G + B + A + COLOR_START_MARK_RIGHT;
        return HexColor + str + COLOR_END_MASK;
    }
    public static string GetColorString(Color c ,string str)
    {
        return GetColorString((ushort)c.r, (ushort)c.g, (ushort)c.b, (ushort)c.a, str);
    }
    /// <summary>
    /// 根据颜色类型获取Unity Color
    /// </summary>
    /// <param name="colorType"></param>
    /// <returns></returns>
    public static UnityEngine.Color GetColor32OfType(ColorType colorType)
    {
        return GetColor32OfColorInt((int)colorType);
    }

    /// <summary>
    /// 根据颜色类型获取Unity Color
    /// </summary>
    /// <param name="colorInt"></param>
    /// <returns></returns>
    public static UnityEngine.Color GetColor32OfColorInt(int colorInt)
    {
        float inv = 1f / 255f;
        UnityEngine.Color c = UnityEngine.Color.black;
        c.r = inv * ((colorInt >> 16) & 0xFF);
        c.g = inv * ((colorInt >> 8) & 0xFF);
        c.b = inv * ((colorInt >> 0) & 0xFF);
        c.a = inv * 255f;
        return c;
    }

    /// <summary>
    /// 变换unity颜色为ngui色值
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static string TrasnformUnityColor2NGUIColor(UnityEngine.Color color)
    {
      
        uint colorInt = (((uint)(color.r * 255)) << 24) | (((uint)(color.g * 255)) << 16) | ((uint)(color.b * 255)) << 8 | ((uint)(color.a * 255));
        colorInt = colorInt & 0xFFFFFF;
        string nguiString = COLOR_START_MARK_LEFT + colorInt.ToString("X6") + COLOR_START_MARK_RIGHT;
        return nguiString;
    }

    /// <summary>
    /// 变换ngui颜色值为unity颜色
    /// </summary>
    /// <param name="nguiColor"></param>
    /// <returns></returns>
    public static UnityEngine.Color TransformNGUIColor2UnityColor(string nguiColor)
    {
        string temp = nguiColor.Trim();
        if (!string.IsNullOrEmpty(temp) && temp.StartsWith("[") && temp.EndsWith("]"))
        {
            temp = temp.Substring(1, temp.Length - 2);
            if (IsHexColorString("0x" + temp))
            {
                int colorInt = 0;
                for(int i = 0;i<6;i++)
                {
                    colorInt += (int)(Math.Pow(16, i) * (int)temp[5 - i]);
                }

                return GetColor32OfColorInt(colorInt);
            }
        }
        return UnityEngine.Color.white;
    }


    /// <summary>
    /// 生成NGUI适用颜色字符串
    /// </summary>
    /// <param name="hexColor"></param>
    /// <returns></returns>
    public static string PaseHex2NGUIColor(string hexColor)
    {
        if (IsHexColorString(hexColor))
            return null;
        return COLOR_START_MARK_LEFT + hexColor.Trim().Substring(2,6) + COLOR_START_MARK_RIGHT;
    }

    /// <summary>
    /// 是否为16进制颜色
    /// </summary>
    /// <param name="hexColor"></param>
    /// <returns></returns>
    public static bool IsHexColorString(string hexColor)
    {
        if(string.IsNullOrEmpty(hexColor))
            return false;
        string hexColorTrim = hexColor.Trim();
        if(hexColorTrim.StartsWith("0x",true,null) && hexColorTrim.Length == 8)
        {
            Regex regex = new Regex("[0-9a-fA-F]{6}");
            Match match = regex.Match(hexColorTrim, 0, hexColorTrim.Length);
            if(match.Success)
                return true;
        }
        return false;
    }

    #endregion
}
