using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class StringUtil
{
    /// <summary>
    /// 获取显示的字符串
    /// </summary>
    /// <param name="num">拥有的道具数量</param>
    /// <param name="need">需要的道具数量</param>
    /// <returns></returns>
    static public string GetNumNeedString(int num, int need)
    {

        if (num < need)
        {
            string numstr = ColorManager.GetColorString(ColorType.Red, num.ToString());
            numstr += "/" + need.ToString();
            return numstr;
        }
        else
        {
            return num.ToString() + "/" + need.ToString();
        }
    }
    static public string GetNumNeedString(uint num, uint need)
    {
        return GetNumNeedString((int)num, (int)need);
    }
    static public string GetNumNeedString(int num, uint need)
    {
        return GetNumNeedString((int)num, (int)need);
    }
    static public string GetNumNeedString(uint num, int need)
    {
        return GetNumNeedString((int)num, (int)need);
    }
    /// <summary>
    /// 获取带颜色的字符
    /// </summary>
    /// <param name="needNum">需要解锁的等级</param>
    /// <param name="hasNum">拥有的等级</param>
    /// <param name="noColor">不满足的颜色</param>
    /// <param name="yesColor">满足的颜色</param>
    /// 字符内容
    /// <returns></returns>
    static public string GetColorString(int needNum, int hasNum, ColorType noColor, ColorType yesColor, string content)
    {
        ColorType c = noColor;
        if (needNum <= hasNum)
        {
            c = yesColor;
        }
        return ColorManager.GetColorString(c, content);
    }
    /// <summary>
    /// 通过秒获取fomat时间 	{0:hh:mm:ss}
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    static public string GetStringBySeconds(uint t)
    {

        int day, hour, minute, second;
        hour = Convert.ToInt16(t / 3600);
        minute = Convert.ToInt16((t % 3600) / 60);
        second = Convert.ToInt16(t % 3600 % 60);

        string str = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        return str;
    }
    static public string GetAdjustStringBySeconds(uint t)
    {

        int day, hour, minute, second;
        hour = Convert.ToInt16(t / 3600);
        minute = Convert.ToInt16((t % 3600) / 60);
        second = Convert.ToInt16(t % 3600 % 60);
        string str = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
        if (hour == 0)
        {
            if (minute == 0)
            {
                str = string.Format("{0:D2}", second);
            }
            else
            {
                str = string.Format("{0:D2}:{1:D2}", minute, second);
            }
        }
        return str;

    }
    /// <summary>
    /// 根据1970年到现在的秒 获取时间 
    /// </summary>
    /// <param name="t"></param>
    /// <returns>08-22 09:22</returns>
    static public string GetStringSince1970(uint t)
    {
        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(t);
        return dt.ToString("MM-dd HH：mm");

    }
    /// <summary>
    /// 获取大数字符串 加上w
    /// </summary>
    /// <param name="num">17000</param>
    /// <returns>1.7w</returns>
    static public string GetBigMoneyStr(uint num)
    {
        if (num > 10000)
        {
            float v = num * 1.0f / 10000;
            return v.ToString("F4") + "w";
        }
        else
        {
            return num.ToString("F2");
        }
    }
    static public string GetBigMoneyStr(float num)
    {
        if (num > 10000)
        {
            float v = num * 1.0f / 10000;
            return v.ToString("F4") + "w";
        }
        else
        {
            return num.ToString("F2");
        }
    }
    static public string GetBigMoneyStr(int num)
    {
        return GetBigMoneyStr((uint)num);
    }

    public static string GetLeftTimeStringMin(int t)
    {
        int day = Convert.ToInt32(t / 86400);
        int hour = Convert.ToInt32((t % 86400) / 3600);
        int min = Convert.ToInt32((t % 86400 % 3600) / 60);
        string str = string.Empty;

        if (t >= 86400)
        {
            str = str + string.Format("{0}天", day.ToString());
        }
        else if (t >= 3600)
        {
            str = string.Format("{0}:{1}", hour.ToString("D2"), min.ToString("D2"));
        }
        else if (t > 60)
        {
            str = string.Format("00:{1}", hour.ToString("D2"), min.ToString("D2"));
        }
        if (!string.IsNullOrEmpty(str))
        {
            return str;
        }
        if (t > 0)
        {
            return CommonData.GetLocalString("1分钟内");
        }

        return CommonData.GetLocalString("已过期");
    }

    /// <summary>
    /// x天x时x分
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static string GetLeftTimeStringMin2(int t)
    {
        int day = Convert.ToInt32(t / 86400);
        int hour = Convert.ToInt32((t % 86400) / 3600);
        int min = Convert.ToInt32((t % 86400 % 3600) / 60);
        string str = string.Empty;

        if (t > 60)
        {
            str = string.Format("{0}天{1}时{2}分", day.ToString("D2"), hour.ToString("D2"), min.ToString("D2"));
        }
        if (!string.IsNullOrEmpty(str))
        {
            return str;
        }
        if (t > 0)
        {
            return CommonData.GetLocalString("1分钟内");
        }

        return CommonData.GetLocalString("已过期");
    }

    public string GetLeftTimeStringMinute(int nTimeSce)
    {
        if (nTimeSce == 0)
        {
            return CommonData.GetLocalString("永久");
        }
        int num4 = ((int)nTimeSce) / 86400;
        int num5 = (((int)nTimeSce) - (num4 * 86400)) / 3600;
        string str = string.Empty;
        if (num4 > 0)
        {
            str = str + string.Format("{0}", num4.ToString() + CommonData.GetLocalString("天"));
        }
        if (num5 > 0)
        {
            str = str + string.Format("{0}", num5.ToString() + CommonData.GetLocalString("时"));
        }
        if (num5 < 1)
        {
            int num6 = ((((int)nTimeSce) - (num4 * 86400)) - (num5 * 3600)) / 60;
            if (num6 > 0)
            {
                str = str + string.Format("{0}", num6.ToString() + CommonData.GetLocalString("分"));
            }
        }

        if (!string.IsNullOrEmpty(str))
        {
            return str;
        }
        if (nTimeSce > 0)
        {
            return CommonData.GetLocalString("1分钟内");
        }

        return CommonData.GetLocalString("已过期");
    }
    /// <summary>
    /// 获取字符串分割列表
    /// </summary>
    /// <typeparam name="T">返回泛型类型</typeparam>
    /// <param name="srcStr">源字符串</param>
    /// <param name="splitChar">分割符</param>
    /// <returns></returns>
    public static List<T> GetSplitStringList<T>(string srcStr, params char[] splitChar)
    {
        List<T> resultList = new List<T>();
        if (string.IsNullOrEmpty(srcStr))
        {
            return resultList;
        }
        string[] strArray = srcStr.Split(splitChar);
        Type t = typeof(T);
        for (int i = 0; i < strArray.Length; i++)
        {
            string str = strArray[i];
            if (string.IsNullOrEmpty(str))
            {
                continue;
            }
            T ot = (T)Convert.ChangeType(str, t);
            if (ot == null)
            {
                Engine.Utility.Log.Error("changetype error str is " + str + " t is " + t.Name);
                continue;
            }
            resultList.Add(ot);

        }
        return resultList;
    }
}

