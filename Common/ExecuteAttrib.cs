using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// 标志是一个可以响应消息的方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ExecuteAttribute : Attribute
    {
        #region 具有ExecuteAttribute特性函数的自动提取
        /// <summary>
        /// 得到类中所有具有<see cref="ExecuteAttribute"/>特性的静态方法
        /// </summary>
        public static IEnumerable<MethodInfo> GetStaticExecuteMethod(Type type)
        {
            return from m in GetExecuteMethod(type) where m.IsStatic select m;
        }

        /// <summary>
        /// 得到所给汇编中所有具有<see cref="ExecuteAttribute"/>特性的静态方法
        /// </summary>
        public static IEnumerable<MethodInfo> GetStaticExecuteMethod(Assembly assembly)
        {
            return (
                from type in assembly.GetAllTypes()
                select GetStaticExecuteMethod(type))
                .SelectMany(s => s);
        }

        /// <summary>
        /// 得到对象中所有具有<see cref="ExecuteAttribute"/>特性的方法
        /// </summary>
        public static IEnumerable<MethodInfo> GetInstanceExecuteMethod(Type targetType)
        {
            return from m in GetExecuteMethod(targetType) where m.IsStatic == false select m;
        }

        private static IEnumerable<MethodInfo> GetExecuteMethod(Type targetType)
        {
            return
                from method in Reflection.GetRuntimeMethods(targetType)
                where IsDefined(method)
                select method;
        }
        #endregion

        public static bool IsDefined(MethodInfo method)
        {
            return Attribute.IsDefined(method, typeof(ExecuteAttribute));
        }
    }
}
