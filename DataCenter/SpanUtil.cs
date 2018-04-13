
//*************************************************************************
//	创建日期:	2017/8/22 星期二 17:37:32
//	文件名称:	SpanUtil
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
//测量代码段执行时间消耗
public class SpanUtil
{
    static long start_tick = 0;

    static public void Start(string tag = null)
    {
        start_tick = DateTime.UtcNow.Ticks;

        if (tag != null)
        {
            Log.Error("Span Start: " + tag);
        }
    }

    static public void Stop(string tag)
    {
        Log.Error("Span Stop, " + tag + ":  " + (DateTime.UtcNow.Ticks - start_tick) / 10000);
        start_tick = DateTime.UtcNow.Ticks;
    }
}