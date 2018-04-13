using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using Common;
using Engine.Utility;
using System.Text;


public class Table
{
    class TypeCompare : Comparer<Type>
    {
        public override int Compare(Type x, Type y)
        {
            return x.FullName.CompareTo(y.FullName);
        }
    }


    private static readonly SortedDictionary<Type, WeakReference> cache = new SortedDictionary<Type, WeakReference>(new TypeCompare());

    /// <summary>
    /// 清空
    /// </summary>
    public static void Clear()
    {
        cache.Clear();
    }

    public static List<T> Query<T>(string tab_name = null) where T : ProtoBuf.IExtensible
    {
        if(typeof(T) == typeof(table.UIResourceDataBase))
        {
            int a = 10;
        }
        WeakReference wr;
        if (cache.TryGetValue(typeof(T), out wr) == false)
            cache[typeof(T)] = wr = new WeakReference(null);
        var ret = wr.Target as List<T>;
        if (ret == null)
        {
            if (string.IsNullOrEmpty(tab_name))
                tab_name = typeof(T).Name;

            wr.Target = ret = new List<T>(Load<T>(tab_name));
        }
        return ret;
    }

    public static IEnumerable<T> Load<T>(string tab_name) where T : ProtoBuf.IExtensible
    {
        string path = m_data_path + tab_name + ".bytes";

        var oper = (ProtoBuf.IMessageOperator)typeof(T).GetField("MessageOperator").GetValue(null);

        byte[] data;
        Engine.Utility.FileUtils.Instance().GetBinaryFileBuff(path, out data);
        if (data != null)
        {
            using (var mem = new MemoryStream(data))
            {
                // 忽略前面的 google.protobuf.FileDescriptorSet 元数据定义信息
                uint descriptor = SilentOrbit.ProtocolBuffers.ProtocolParser.ReadUInt32(mem);
                mem.Seek(descriptor, SeekOrigin.Current);
                while (mem.Position < mem.Length)
                {
                    yield return (T)oper.DeserializeLengthDelimited(mem);
                }
            }
        }
        data = null;
        Log.Trace("load tab: " + path);
    }

    public static void SetDataPath(string path)
    {
        m_data_path = path;
    }
    static StringBuilder m_globalPath = new StringBuilder();
    public static string GetGlobalPath()
    {
        if( m_globalPath.Length == 0)
        {
            m_globalPath.Append(m_data_path);
            m_globalPath.Append("Global.json");
        }

        return m_globalPath.ToString();
    }
    static string m_data_path;
}