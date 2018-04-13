using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using System.Reflection;

public static partial class Extensions
{
    public static T Read<T>(this System.IO.Stream s) where T : new()
    {
        var t = new T();
        Read<T>(s, ref t);
        return t;
    }

    public static void Read<T>(this System.IO.Stream s, ref T value)
    {
        var t = typeof(T);
        if (t == typeof(bool)) { bool r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(byte)) { byte r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(sbyte)) { sbyte r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(ushort)) { ushort r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(short)) { short r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(uint)) { uint r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(int)) { int r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(ulong)) { ulong r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(long)) { long r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(float)) { float r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(double)) { double r; Common.Extensions.Read(s, out r); value = (T)(object)r; return; }
        if (t == typeof(byte[])) { Common.Extensions.Read(s, value as byte[]); return; }
        if (typeof(Common.ISerializable).IsAssignableFrom(t))
        {
            if (value == null)
                value = (T)Activator.CreateInstance(t);
            (value as Common.ISerializable).Deserialize(s);
            return;
        }

        var list = value as IList;
        if (list != null)
        {
            Type type = null;
            if (t.IsArray)
                type = t.GetElementType();
            else if (t.IsGenericType)
                type = t.GetGenericArguments().FirstOrDefault();

            if (type != null)
            {
                var method = (MethodInfo.GetCurrentMethod() as MethodInfo).MakeGenericMethod(type);
                for (var i = 0; i < list.Count; i++)
                {
                    var args = new object[] { s, Activator.CreateInstance(type) };
                    method.Invoke(null, args);
                    list[i] = args[1];
                }
                return;
            }
        }

        throw new NotImplementedException(t.FullName);
    }

    public static void Read(this System.IO.Stream s, ref string value, int fixLength)
    {
        value = MyConvert.ToString(Common.Extensions.ReadBytes(s, fixLength));
    }

    public static void Write<T>(this System.IO.Stream s, T value)
    {
        Write<T>(s, (object)value);
    }

    public static void Write<T>(this System.IO.Stream s, object value)
    {
        var t = typeof(T);
        if (t == typeof(bool)) { Common.Extensions.Write(s, Convert.ToBoolean(value)); return; }
        if (t == typeof(byte)) { Common.Extensions.Write(s, Convert.ToByte(value)); return; }
        if (t == typeof(sbyte)) { Common.Extensions.Write(s, Convert.ToSByte(value)); return; }
        if (t == typeof(ushort)) { Common.Extensions.Write(s, Convert.ToUInt16(value)); return; }
        if (t == typeof(short)) { Common.Extensions.Write(s, Convert.ToInt16(value)); return; }
        if (t == typeof(uint)) { Common.Extensions.Write(s, Convert.ToUInt32(value)); return; }
        if (t == typeof(int)) { Common.Extensions.Write(s, Convert.ToInt32(value)); return; }
        if (t == typeof(ulong)) { Common.Extensions.Write(s, Convert.ToUInt64(value)); return; }
        if (t == typeof(long)) { Common.Extensions.Write(s, Convert.ToInt64(value)); return; }
        if (t == typeof(float)) { Common.Extensions.Write(s, Convert.ToSingle(value)); return; }
        if (t == typeof(double)) { Common.Extensions.Write(s, Convert.ToDouble(value)); return; }
        if (t == typeof(byte[])) { Common.Extensions.Write(s, value as byte[]); return; }
        if (typeof(Common.ISerializable).IsAssignableFrom(t)) { ((value ?? Activator.CreateInstance(t)) as Common.ISerializable).Serialize(s); return; }

        var list = value as IList;
        if (list != null)
        {
            Type type = null;
            if (t.IsArray)
                type = t.GetElementType();
            else if (t.IsGenericType)
                type = t.GetGenericArguments().FirstOrDefault();

            if (type != null)
            {

                var method = (MethodInfo.GetCurrentMethod() as MethodInfo).MakeGenericMethod(type);
                for (var i = 0; i < list.Count; i++)
                {
                    var args = new object[] { s, list[0] };
                    method.Invoke(null, args);
                }
                return;
            }
        }

        throw new NotImplementedException(t.FullName);
    }

    public static void Write(this System.IO.Stream s, string value, int fixLength)
    {
        s.Write(MyConvert.ToBytes(value, fixLength));
    }

    /// <summary>
    /// 稳定的插入排序算法
    /// ref: http://www.csharp411.com/c-stable-sort/
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="comparison"></param>
    public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
    {
        if (list == null)
            return;
        if (comparison == null)
            throw new ArgumentNullException("comparison");

        int count = list.Count;
        for (int j = 1; j < count; j++)
        {
            T key = list[j];

            int i = j - 1;
            for (; i >= 0 && comparison(list[i], key) > 0; i--)
            {
                list[i + 1] = list[i];
            }
            list[i + 1] = key;
        }
    }
    /// <summary>
    /// 稳定的插入排序算法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void InsertionSort<T>(this IList<T> list) where T : IComparable<T>
    {
        var compare = new Comparison<T>((a, b) => a == null ? (b == null ? 0 : -1) : a.CompareTo(b));
        InsertionSort(list, compare);
    }


    public static string GetEnumDescription(this Enum enumValue)
    {
        string str = enumValue.ToString();
        System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
        object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
        if (objs == null || objs.Length == 0) return str;
        System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
        return da.Description;
    }

    #region XML
    /// <summary>
    /// 获取指定名称的xml属性值
    /// </summary>
    /// <param name="e"></param>
    /// <param name="name"></param>
    /// <returns>失败返回null</returns>
    public static string AttributeValue(this System.Xml.Linq.XElement e, System.Xml.Linq.XName name)
    {
        if (e != null)
        {
            var a = e.Attribute(name);
            if (a != null)
                return a.Value;
        }
        return null;
    }
    #endregion
}
