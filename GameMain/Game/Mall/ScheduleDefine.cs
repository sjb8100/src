/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Mall
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ScheduleDefine
 * 版本号：  V1.0.0.0
 * 创建时间：12/18/2016 4:20:35 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ScheduleDefine
{
    #region define
    /// <summary>
    /// 日程循环类型
    /// </summary>
    public enum ScheduleCycleType
    {
        None = 1,
        Day = 2,            //日循环
        Week = 3,           //周循环
        Month = 4,          //月循环
        Year = 5,           //年循环
    }

    /// <summary>
    /// 日程单元
    /// </summary>
    public class ScheduleUnit
    {
        #region property
        //日程循环类型
        private ScheduleCycleType cycleType = ScheduleCycleType.None;
        public ScheduleCycleType CycleType
        {
            get
            {
                return cycleType;
            }
        }

        //日程中星期（循环类型为周循环有效）
        private int week = 0;
        public int Week
        {
            get
            {
                return week;
            }
        }

        /// <summary>
        /// 有效星期值（0-6）
        /// 0：表示周日 1：表示周一 以此类推
        /// </summary>
        public int ValidWeek
        {
            get
            {
                return week % 7;
            }
        }

        //日程单元开始时间
        private System.DateTime start;
        public System.DateTime Start
        {
            get
            {
                return start;
            }
        }

        //日程单元结束时间
        private System.DateTime end;
        public System.DateTime End
        {
            get
            {
                return end;
            }
        }
        #endregion

        /// <summary>
        /// 创建日程单元
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="cycleType">循环类型</param>
        /// <param name="week">星期 cycleType为周循环有效</param>
        /// <returns></returns>
        public static ScheduleUnit Create(DateTime start, DateTime end
            , ScheduleCycleType cycleType = ScheduleCycleType.None,int week = 0)
        {
            ScheduleUnit unit = new ScheduleUnit();
            return (unit.UpdateData(start, end,cycleType,week)) ? unit : null;
        }

        /// <summary>
        /// 更新日程
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="cycleType"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public bool UpdateData(DateTime start, DateTime end, ScheduleCycleType cycleType,int week = 0)
        {
            if (null == start || null == end
                || end.Ticks - start.Ticks < 0)
                return false;
            this.start = start;
            this.end = end;
            this.cycleType = cycleType;
            this.week = week;
            return true;
        }

        /// <summary>
        /// 是否seconds在日程单元内
        /// </summary>
        /// <param name="seconds"></param>
        /// <param name="leftSeconds">>如果在日程内leftseonds表示本次日程剩余时间，
        /// 反之距离下次日程时间，如果为0:表示不在日程内并且没有下个日程</param>
        /// <returns></returns>
        public bool IsInSchedule(long seconds, out long leftSeconds)
        {
            return IsInSchedule(DateTimeHelper.TransformServerTime2DateTime(seconds),out leftSeconds);
        }

        /// <summary>
        /// 是否dateTime在日程单元内
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="leftSeconds">>如果在日程内leftseonds表示本次日程剩余时间，
        /// 反之距离下次日程时间，如果为0:表示不在日程内并且没有下个日程</param>
        /// <returns></returns>
        public bool IsInSchedule(DateTime dateTime, out long leftSeconds)
        {
            return ScheduleUnit.IsInCycleDateTme(start, end, dateTime, out leftSeconds, cycleType, (DayOfWeek)ValidWeek);
        }

        #region CycleHandler

        /// <summary>
        /// dateTime是否在循环日程中
        /// </summary>
        /// <param name="start">日程开始时间</param>
        /// <param name="end">日程结束时间</param>
        /// <param name="dateTime">当前时间</param>
        /// <param name="leftseconds">如果在日程内leftseonds表示本次日程剩余时间，
        /// 反之距离下次日程时间，如果为0:表示不在日程内并且没有下个日程</param>
        /// <param name="cycleType"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public static bool IsInCycleDateTme(long start,long end,long datetime,out long leftseconds,ScheduleCycleType cycleType = ScheduleCycleType.None,DayOfWeek week = 0)
        {
            return IsInCycleDateTme(DateTimeHelper.TransformServerTime2DateTime(start)
                , DateTimeHelper.TransformServerTime2DateTime(end)
                , DateTimeHelper.TransformServerTime2DateTime(datetime)
                , out leftseconds, cycleType, week);
        }

        /// <summary>
        /// dateTime是否在循环日程中
        /// </summary>
        /// <param name="start">日程开始时间</param>
        /// <param name="end">日程结束时间</param>
        /// <param name="dateTime">当前时间</param>
        /// <param name="leftseconds">如果在日程内leftseonds表示本次日程剩余时间，
        /// 反之距离下次日程时间，如果为0:表示不在日程内并且没有下个日程</param>
        /// <param name="cycleType"></param>
        /// <param name="week"></param>
        /// <returns></returns>
        public static bool IsInCycleDateTme(DateTime start,DateTime end
            , DateTime dateTime, out long leftseconds
            ,ScheduleCycleType cycleType = ScheduleCycleType.None,DayOfWeek week = 0)
        {
            leftseconds = 0;
            bool inschedule = false;
            if (DateTimeHelper.IsDateTimeBetweenStartAndEnd(start, end, dateTime))
            {
                DateTime startTemp;
                DateTime endTemp;
                switch (cycleType)
                {
                    case ScheduleCycleType.None:
                        {
                            inschedule = true;
                            leftseconds = (long)((end - dateTime).TotalSeconds);
                        }
                        break;
                    case ScheduleCycleType.Day:
                        {
                            startTemp = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, start.Hour, start.Minute, 0, 0, 0);
                            endTemp = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, end.Hour, end.Minute, 0, 0, 0);
                            if (startTemp > endTemp)
                            {
                                Engine.Utility.Log.Error("ScheduleUnit-> IsInSchedule ScheduleCycleType.Day error startTemp>endTemp");
                            }
                            else
                            {
                                if (DateTimeHelper.IsDateTimeBetweenStartAndEnd(startTemp, endTemp, dateTime))
                                {
                                    inschedule = true;
                                    leftseconds = (long)((endTemp - dateTime).TotalSeconds);
                                }
                                else if (dateTime > end)
                                {
                                    startTemp = startTemp.AddDays(1);
                                    if (dateTime < startTemp)
                                    {
                                        //距离下一个日程
                                        inschedule = false;
                                        leftseconds = (long)((startTemp - dateTime).TotalSeconds);
                                    }

                                }
                                else if (dateTime < startTemp)
                                {
                                    inschedule = false;
                                    leftseconds = (long)((startTemp - dateTime).TotalSeconds);
                                }
                            }
                        }
                        break;
                    case ScheduleCycleType.Week:
                        {
                            startTemp = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, start.Hour, start.Minute, 0, 0, 0);
                            endTemp = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, end.Hour, end.Minute, 0, 0, 0);
                            DayOfWeek dw = dateTime.DayOfWeek;
                            if (dw == week)
                            {
                                if (DateTimeHelper.IsDateTimeBetweenStartAndEnd(startTemp, endTemp, dateTime))
                                {
                                    inschedule = true;
                                    leftseconds = (long)((endTemp - dateTime).TotalSeconds);
                                }else if (dateTime < startTemp)
                                {
                                    inschedule = false;
                                    leftseconds = (long)((startTemp - dateTime).TotalSeconds);
                                }else
                                {
                                    //往后推一星期
                                    startTemp = startTemp.AddDays(7);
                                    if (DateTimeHelper.IsDateTimeBetweenStartAndEnd(start, end, startTemp))
                                    {
                                        inschedule = false;
                                        leftseconds = (long)((startTemp - dateTime).TotalSeconds);
                                    }
                                }
                            }
                            else
                            {
                                //直接找下一个满足条件的星期
                                int gapDay = week - dw;
                                if (gapDay <=0)
                                {
                                    gapDay += 7;
                                }
                                startTemp = startTemp.AddDays(gapDay);
                                if (DateTimeHelper.IsDateTimeBetweenStartAndEnd(start, end, startTemp))
                                {
                                    inschedule = false;
                                    leftseconds = (long)((startTemp - dateTime).TotalSeconds);
                                }
                            }
                        }
                        break;
                    case ScheduleCycleType.Month:
                        {
                            startTemp = new DateTime(dateTime.Year, dateTime.Month, start.Day, start.Hour, start.Minute, 0, 0, 0);
                            endTemp = new DateTime(dateTime.Year, dateTime.Month, end.Day, end.Hour, end.Minute, 0, 0, 0);
                            if (startTemp > endTemp)
                            {
                                Engine.Utility.Log.Error("ScheduleUnit-> HandlerScheduleMonth error startTemp>endTemp");
                            }
                            else
                            {
                                if (DateTimeHelper.IsDateTimeBetweenStartAndEnd(startTemp, endTemp, dateTime))
                                {
                                    inschedule = true;
                                    leftseconds = (long)((endTemp - dateTime).TotalSeconds);
                                }
                                else if (dateTime > end)
                                {
                                    startTemp = startTemp.AddMonths(1);
                                    if (DateTimeHelper.IsDateTimeBetweenStartAndEnd(start, end, startTemp)
                                        && dateTime < startTemp)
                                    {
                                        //距离下一个日程
                                        inschedule = false;
                                        leftseconds = (long)((startTemp - dateTime).TotalSeconds);
                                    }

                                }
                                else if (dateTime < startTemp)
                                {
                                    inschedule = false;
                                    leftseconds = (long)((startTemp - dateTime).TotalSeconds);
                                }
                            }
                        }
                        break;
                    case ScheduleCycleType.Year:
                        {
                            startTemp = new DateTime(dateTime.Year, start.Month, start.Day, start.Hour, start.Minute, 0, 0, 0);
                            endTemp = new DateTime(dateTime.Year, end.Month, end.Day, end.Hour, end.Minute, 0, 0, 0);
                            if (startTemp > endTemp)
                            {
                                Engine.Utility.Log.Error("ScheduleUnit-> HandlerScheduleYear error startTemp>endTemp");
                            }
                            else
                            {
                                if (DateTimeHelper.IsDateTimeBetweenStartAndEnd(startTemp, endTemp, dateTime))
                                {
                                    inschedule = true;
                                    leftseconds = (long)((endTemp - dateTime).TotalSeconds);
                                }
                                else if (dateTime > end)
                                {
                                    startTemp = startTemp.AddYears(1);
                                    if (DateTimeHelper.IsDateTimeBetweenStartAndEnd(start, end, startTemp)
                                        && dateTime < startTemp)
                                    {
                                        //距离下一个日程
                                        inschedule = false;
                                        leftseconds = (long)((startTemp - dateTime).TotalSeconds);
                                    }
                                }
                                else if (dateTime < startTemp)
                                {
                                    inschedule = false;
                                    leftseconds = (long)((startTemp - dateTime).TotalSeconds);
                                }
                            }
                        }
                        break;
                }
            }
            else if (dateTime < start)
            {
                leftseconds = (long)((start - dateTime).TotalSeconds);
            }
            return inschedule;
        }
        #endregion
    }



    /// <summary>
    /// 日程数据类
    /// </summary>
    public class ScheduleLocalData
    {
        //日程id
        public uint scheduleId;
        public uint ScheduleId
        {
            get
            {
                return scheduleId;
            }
        }

        /// <summary>
        /// 日程循环类型
        /// </summary>
        public ScheduleCycleType CycleType
        {
            get
            {
                return (null != TabData) ? (ScheduleCycleType)TabData.cycleType : ScheduleCycleType.None;
            }
        }

        //日程表格数据
        public table.ScheduleDataBase TabData
        {
            get
            {
                return DataManager.Manager<MallManager>().GetScheduleDataBase(scheduleId);
            }
        }

        private ScheduleUnit m_scheduleUint = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="scheduleId"></param>
        public ScheduleLocalData(uint scheduleId)
        {
            this.scheduleId = scheduleId;
            table.ScheduleDataBase scheData = TabData;
            if (null != scheData)
            {
                m_scheduleUint = ScheduleUnit.Create(new System.DateTime((int)scheData.startYear, (int)scheData.startMonth, (int)scheData.startDay
                    , (int)scheData.startHour, (int)scheData.startMin, 0)
               , new System.DateTime((int)scheData.endYear, (int)scheData.endMonth, (int)scheData.endDay
                   , (int)scheData.endHour, (int)scheData.endMin, 0)
                   , CycleType
                   ,(int)scheData.week);
            }
        }
        
        /// <summary>
        /// 是否当前时间timeSeconds 满足日程条件
        /// </summary>
        /// <param name="timeSeconds">距离1970.1.1.0.0.0</param>
        /// <param name="leftSeconds">剩余时间</param>
        /// <returns></returns>
        public bool IsInSchedule(long timeSeconds, out long leftSeconds)
        {
            return IsInSchedule(DateTimeHelper.TransformServerTime2DateTime(timeSeconds),out leftSeconds);
        }

        /// <summary>
        /// 是否日期data满足日程条件
        /// </summary>
        /// <param name="date"></param>
        /// <param name="leftSeconds">剩余时间</param>
        /// <returns></returns>
        public bool IsInSchedule(System.DateTime date, out long leftSeconds)
        {
            leftSeconds = 0;
            return m_scheduleUint.IsInSchedule(date, out leftSeconds);
        }
    }
    #endregion
}