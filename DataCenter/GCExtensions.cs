
//*************************************************************************
//	创建日期:	2017/8/3 星期四 10:53:44
//	文件名称:	GCExtensions
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	对一些通用函数的非gc实现
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

public static class GCExtensions
{
    public static void Reverse_NoHeapAlloc<T>(this List<T> list)
    {
        int count = list.Count;

        for (int i = 0; i < count / 2; i++)
        {
            T tmp = list[i];
            list[i] = list[count - i - 1];
            list[count - i - 1] = tmp;
        }
    }
}
