using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Cmd;
using System.Reflection;
using UnityEngine;
using Cmd.NullCmd;

namespace Cmd
{
	public class CommandSerializer : IEnumerable<KeyValuePair<Type, CommandTag>>
	{
		#region Type ←→ Tag
		private readonly Dictionary<CommandTag, Type> m_TagTypeTable = new Dictionary<CommandTag, Type>();
		private readonly Dictionary<Type, CommandTag> m_TypeTagTable = new Dictionary<Type, CommandTag>();

		public CommandTag this[Type type]
		{
			get
			{
				CommandTag tag;
				return m_TypeTagTable.TryGetValue(type, out tag) ? tag : null;
			}
		}

		public Type this[CommandTag tag]
		{
			get
			{
				Type type;
				return m_TagTypeTable.TryGetValue(tag, out type) ? type : null;
			}
		}

		public void Register(CommandTag tag, Type type)
		{
			if (tag == null || type == null)
				throw new ArgumentNullException();
#if UNITY_EDITOR
			if (type.GetInterfaces().Contains(typeof(Common.ISerializable)) == false)
				throw new ArgumentException(string.Format("{0} must implement interface {1}.",
					type.FullName, typeof(Common.ISerializable).FullName));
			if (m_TagTypeTable.ContainsKey(tag))
				throw new ArgumentException(string.Format("element already exists: [{0}, {1}] and [{2}, {3}]",
					tag, m_TagTypeTable[tag].FullName,
					tag, type.FullName));
#endif
			m_TagTypeTable.Add(tag, type);
			m_TypeTagTable.Add(type, tag);
		}

		public void Register(Assembly assembly)
		{
			if (assembly == null)
				return;
			var types =
				from t in assembly.GetTypes()
				let tag = CommandTag.Extract(t)
				where tag != null
				select new { Type = t, Tag = tag };
			foreach (var t in types)
			{
				Register(t.Tag, t.Type);
			}
		}
		#endregion

		public Common.ISerializable DeserializeCommand(Stream stream)
		{
			if (stream.Length - stream.Position < CommandTag.Size)
				return null;
			var tag = new CommandTag();

			try
			{
				tag.Deserialize(stream);
				var type = this[tag];
				if (type != null)
				{
					var data = Activator.CreateInstance(type) as Common.ISerializable;
					if (data != null)
					{
						data.Deserialize(stream);
						return data;
					}
				}
				else if (tag.Equals(new CommandTag()) && stream.Length == stream.Position)
				{
					// t_NullCmd消息号和t_ClientNullCmd重复，都是(0,0)，但总长度只有2字节
                    var data = new t_NullCmd();
                    data.Deserialize(stream);
                    return data;
				}
			}
			catch (Exception ex)
			{
				Debug.Log(tag);
				Debug.LogException(ex);
			}
			Debug.LogWarning(string.Format("无法识别的消息号: {0}", tag));
			return null;
		}

		public Common.ISerializable DeserializeCommand(byte[] data)
		{
			using (var mem = new MemoryStream(data))
			{
				return DeserializeCommand(mem);
			}
		}

		public void SerializeCommand(Common.ISerializable data, Stream stream)
		{
			var type = data.GetType();
            if (type == typeof(t_NullCmd))
            {
                new CommandTag().Serialize(stream);
                new t_NullCmd().Serialize(stream);
                return;
            }
			var tag = this[type];
			if (tag == null)
				throw new KeyNotFoundException(type.FullName);
			tag.Serialize(stream);
			data.Serialize(stream);
		}

		public byte[] SerializeCommand(Common.ISerializable data)
		{
			if (data == null)
				return new byte[0];
			using (var mem = new MemoryStream())
			{
				SerializeCommand(data, mem);
				mem.Flush();
				return mem.ToArray();
			}
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			foreach (var c in from i in this group i by i.Value.Cmd into g orderby g.Key select g)
			{
				sb.AppendFormat("({0}, *)", c.Key).AppendLine();
				foreach (var p in from i in c orderby i.Value.Param select i)
					sb.AppendFormat("    {0}: {1}", p.Value, p.Key.Name).AppendLine();
			}
			return sb.ToString();
		}

		#region IEnumerable<KeyValuePair<Type,CommandTag>> 成员

		public IEnumerator<KeyValuePair<Type, CommandTag>> GetEnumerator()
		{
			return m_TypeTagTable.GetEnumerator();
		}

		#endregion

		#region IEnumerable 成员

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion
	}
}
