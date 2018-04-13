
//*************************************************************************
//	创建日期:	2018/1/25 星期四 17:36:33
//	文件名称:	SkillNodePool
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace SkillSystem
{
    class SkillNodePool<T> where T : new()
    {
        int maxCount = 100;
        Queue<T> objList = new Queue<T>();
        public T Alloc(Func<T> newAction = null)
        {

            if (objList.Count > 0)
            {
                T obj = objList.Dequeue();
                //objList.RemoveAt(0);
                return obj;
            }
            else
            {
                T obj = default(T);
                if (newAction != null)
                {
                    obj = newAction();
                }
                else
                {
                    obj = new T();
                }
                return obj;
            }
        }
        public void Free(T obj)
        {
           // objList.Add(obj);
            if(GetPoolCount() > maxCount)
            {
                Clear();
            }
            else
            {
                objList.Enqueue(obj);
            }
        }

        public void Clear()
        {
         
            objList.Clear();
        }
        public int GetPoolCount()
        {
            if(objList != null)
            {
                return objList.Count;
            }
            return 0;
        }
    }
}