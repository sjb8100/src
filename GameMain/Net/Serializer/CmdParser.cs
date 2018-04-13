using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Cmd
{
	/// <summary>
	/// 提供Cmd和字符数组之间相互装换的实用类
	/// 可以将字符串数组解析为Cmd对象
	/// </summary>
	public static class CmdParser
	{
		internal static CommandSerializer Serializer { get; private set; }
		/// <summary>
		/// 对数据包进行解析，但当发现有数据包的损坏情况时则完全的抛弃该报中当前位置以后的所有的数据
		/// </summary>
		/// <param name=param name="stream"></param>
		/// <returns></returns>
		public static Common.ISerializable Parse(System.IO.Stream s)
		{
			var ret = Serializer.DeserializeCommand(s);
			Debug.Assert(ret != null && s.Position == s.Length);
			return ret;
		}

		/// <summary>
		/// 通用命令的注册
		/// </summary>
		static CmdParser()
		{
			Serializer = new CommandSerializer();
			Serializer.Register(Assembly.GetExecutingAssembly());
			Serializer.Register(Assembly.GetEntryAssembly());
		}
	}
}
