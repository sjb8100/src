using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class DateTimeHelper : Singleton<DateTimeHelper>
{
    private long server_time;
    private float m_fStartServerTime = 0;   // 与服务器对应的客户端起始时间

    public long ServerTime
    {
        get { return server_time; }
        set
        {
            server_time = value;
            ////修改服务器本地时间为UTC时间减去8小时
            //if (server_time > 28800)
            //    server_time -= 28800;
            m_fStartServerTime = Time.realtimeSinceStartup;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SYNSERVERTIME);
        }
    }

    public long Now
    {
        get
        {
           return server_time + (long)(Time.realtimeSinceStartup - m_fStartServerTime);
        }
    }

    public string TickToString(long timeStamp,string format)
    {
        DateTime cur = new DateTime( 1970 , 1 , 1 , 0 , 0 , 0 , 0 ).AddSeconds( timeStamp );
        return cur.ToString(format);
    }

    public string NowString
    {
        get
        {
            DateTime cur = new DateTime( 1970 , 1 , 1 , 0 , 0 , 0 , 0 ).AddSeconds( Now );
            return cur.ToString("yyyy-MM-dd HH:mm::ss");
        }
    }

    public string NowToString(string format)
    {
        DateTime cur = new DateTime( 1970 , 1 , 1 , 0 , 0 , 0 , 0 ).AddSeconds( Now );
        return cur.ToString(format);
    }


    ///<summary>
    ///由秒数得到日期几天几小时。。。
    ///</summary
    ///<param name="t">秒数</param>
    ///<param name="type">0：转换后带秒，1:转换后不带秒</param>
    ///<returns>几天几小时几分几秒</returns>
    public static string ParseTimeSeconds(int t, int type = 0)
    {
        string r = "";
        int day, hour, minute, second;
        if (t >= 86400) //天,
        {
            day = (int)(t / 86400);
            hour = (int)((t % 86400) / 3600);
            minute = (int)((t % 86400 % 3600) / 60);
            second = (int)(t % 86400 % 3600 % 60);
            if (type == 0){
                r = string.Format("{0}天{1}:{2}:{3}",day,hour.ToString("D2"),minute.ToString("D2"),second.ToString("D2"));
            }
            else
            {
                r = string.Format("{0}天{1}:{2}", day, hour.ToString("D2"), minute.ToString("D2"));
            }
        }
        else if (t >= 3600)//时,
        {
            hour = (int)(t / 3600);
            minute = (int)((t % 3600) / 60);
            second = (int)(t % 3600 % 60);
            if (type == 0)
                r = string.Format("0天{0}:{1}:{2}", hour.ToString("D2"), minute.ToString("D2"), second.ToString("D2"));
            else
                r = string.Format("0天{0}:{1}", hour.ToString("D2"), minute.ToString("D2"));
        }
        else if (t >= 60)//分
        {
            minute = (int)(t / 60);
            second = (int)(t % 60);
            r = string.Format("0天00:{0}:{1}", minute.ToString("D2"), second.ToString("D2"));
        }
        else 
        {
            second = (int)(t);
            if (second > 0)
            {
                r = string.Format("0天00:00:{0}", second.ToString("D2"));
            }
            else
            {
                r = "已过期";
            }
           
        }
        return r;
    }
    public static string ParseTimeSecondsFliter(float t, int type = 0)
    {
        string r = "";
        int day, hour, minute, second;
        if (t >= 86400) //天,
        {
            day = (int)(t / 86400);
            hour = (int)((t % 86400) / 3600);
            minute = (int)((t % 86400 % 3600) / 60);
            second = (int)(t % 86400 % 3600 % 60);
            if (type == 0)
            {
                r = string.Format("{0}天{1}:{2}:{3}", day, hour.ToString("D2"), minute.ToString("D2"), second.ToString("D2"));
            }
            else
            {
                r = string.Format("{0}天{1}:{2}", day, hour.ToString("D2"), minute.ToString("D2"));
            }
        }
        else if (t >= 3600)//时,
        {
            hour = (int)(t / 3600);
            minute = (int)((t % 3600) / 60);
            second = (int)(t % 3600 % 60);
            if (type == 0)
                r = string.Format("{0}:{1}:{2}", hour.ToString("D2"), minute.ToString("D2"), second.ToString("D2"));
            else
                r = string.Format("{0}:{1}", hour.ToString("D2"), minute.ToString("D2"));
        }
        else if (t >= 60)//分
        {
            minute = (int)(t / 60);
            second = (int)(t % 60);
            r = string.Format("{0}:{1}",minute.ToString("D2"), second.ToString("D2"));
        }
        else
        {
            second = (int)(t);
            if (second > 0)
            {
                r = string.Format("{0}", second.ToString("D2"));
            }
            else
            {
                r = "已过期";
            }      
        }
        return r;
    }

    /// <summary>
    /// 变换Ticks（毫微秒）到秒
    /// （注：）100秒 = Math.pow(10,9)毫微秒
    /// 1（秒） = Math.pow(10,7)（毫微秒）
    /// 毫微秒DateTime中Ticks的单位为（毫微秒）
    /// </summary>
    /// <param name="ticks"></param>
    /// <returns></returns>
    public static long TransformTicks2Seconds(long ticks)
    {
        long sec = ticks / 10000000;
        return sec;
    }

    /// <summary>
    /// 变换秒到（毫微秒）
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static long TransfromSeconds2Ticks(long seconds)
    {
        long ticks = seconds * 10000000;
        return ticks;
    }

    /// <summary>
    /// 转化服务器时间到日期
    /// </summary>
    /// <param name="secnds">服务器下发时间戳</param>
    /// <param name="start1970">服务器下发从1970.1.1.0.0.0.0</param>
    /// <returns></returns>
    public static DateTime TransformServerTime2DateTime(long secnds,bool start1970 = true)
    {
        DateTime dt ;
        if (start1970) 
        {
            dt= new DateTime( 1970 , 1 , 1 , 0 , 0 , 0 , 0 );
            dt = dt.AddSeconds(secnds);
        }else
        {
            dt= new DateTime(TransfromSeconds2Ticks(secnds));
        }
        
        return dt;
    }

    /// <summary>
    /// datetime是否在当前日程循环内
    /// </summary>
    /// <param name="start">开始时间</param>
    /// <param name="end">结束时间</param>
    /// <param name="dateTime">当前时间</param>
    /// <returns></returns>
    public static bool IsDateTimeBetweenStartAndEnd(DateTime start, DateTime end, DateTime dateTime)
    {
        return (dateTime >= start && dateTime <= end);
    }
    /// <summary>
    /// 判断当前年是不是闰年
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public static bool IsLeapYear(int year)
    {
        if (year % 4  == 0 && year % 100 != 0  || year % 400 == 0)  
            return true;  
        else  
            return false;  
    }

    /// <summary>
    /// 获取一个月中的天数
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    public static int GetDayCountInMonth(int year,int month)
    {
        int days = 0;
        switch(month)
        {
            case 2:
                days = IsLeapYear(year) ? 28 : 29;
                break;
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                days = 31;
                break;
            case 4:
            case 6:
            case 9:
            case 11:
                days = 30;
                break;
        }
        return days;
    }
}

