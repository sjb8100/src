using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cmd;

namespace Cmd
{
	public class CommandTag : IComparable<CommandTag>, IEquatable<CommandTag>, Common.ISerializable
	{
		public const int Size = 2;

		private byte cmd;
		public byte Cmd { get { return cmd; } set { cmd = value; } }
		private byte param;
		public byte Param { get { return param; } set { param = value; } }

		public override int GetHashCode()
		{
			return (Cmd << 8) | Param;
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(this, obj))
				return true;
			var other = obj as CommandTag;
			if (other != null)
				return this.Equals(other);
			return base.Equals(obj);
		}

		#region IEquatable<CommandTag> 成员

		public bool Equals(CommandTag other)
		{
			return this.CompareTo(other) == 0;
		}

		#endregion

		#region IComparable<CommandTag> 成员

		public int CompareTo(CommandTag other)
		{
			if (other == null)
				return 1;
			return this.GetHashCode().CompareTo(other.GetHashCode());
		}

		#endregion

		#region ISerializable 成员

		public void Serialize(System.IO.Stream s)
		{
			s.Write(Cmd);
			s.Write(Param);
		}

		public void Deserialize(System.IO.Stream s)
		{
			s.Read(ref cmd);
			s.Read(ref param);
		}

		#endregion

		public override string ToString()
		{
			return string.Format("({0},{1})", Cmd, Param);
		}

		/// <summary>
		/// 通过<see cref="ParamTagAttribute"/>和<see cref="ParamTagAttribute"/>特性提取<see cref="CommandTag"/>
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static CommandTag Extract(Type type)
		{
			var cmd = type.GetCustomAttributes(typeof(CmdTagAttribute), true).FirstOrDefault() as CmdTagAttribute;
			var param = type.GetCustomAttributes(typeof(ParamTagAttribute), true).FirstOrDefault() as ParamTagAttribute;
			if (cmd != null && param != null)
				return new CommandTag() { Cmd = cmd.Value, Param = param.Value };
			return null;
		}
	}
}
