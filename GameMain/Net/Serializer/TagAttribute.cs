using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cmd
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public class CmdTagAttribute : Attribute
	{
		public byte Value { get; private set; }

		public CmdTagAttribute(byte cmd)
		{
			this.Value = cmd;
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public class ParamTagAttribute : Attribute
	{
		public byte Value { get; private set; }

		public ParamTagAttribute(byte param)
		{
			this.Value = param;
		}
	}

	/// <summary>
	/// 用数字表示的消息类型
	/// </summary>
	public struct NetMessageType : Common.ISerializable, IComparable<NetMessageType>, IEquatable<NetMessageType>
	{
		public static readonly NetMessageType Empty = new NetMessageType();

		private byte cmd;
		public byte Cmd { get { return cmd; } set { cmd = value; } }
		private byte param;
		public byte Param { get { return param; } set { param = value; } }

		#region Equatable
		public static bool operator ==(NetMessageType a, NetMessageType b)
		{
			if (System.Object.ReferenceEquals(a, b))
				return true;
			if (((object)a == null) || ((object)b == null))
				return false;
			return a.Cmd == b.Cmd && a.Param == b.Param;
		}
		public static bool operator !=(NetMessageType a, NetMessageType b)
		{
			return !(a == b);
		}

		#region IEquatable<NetMessageType> 成员

		public bool Equals(NetMessageType other)
		{
			return this == other;
		}

		#endregion

		public override bool Equals(object obj)
		{
			return obj is NetMessageType ? this == (NetMessageType)obj : false;
		}

		public override int GetHashCode()
		{
			return ((int)this.Cmd << 8) | ((int)this.Param & 0x00FF);
		}
		#endregion

		#region IComparable<NetMessageType> 成员

		public int CompareTo(NetMessageType other)
		{
			if (this.Cmd > other.Cmd)
				return 1;
			else if (this.Cmd < other.Cmd)
				return -1;

			if (this.Param > other.Param)
				return 1;
			else if (this.Param < other.Param)
				return -1;

			return 0;
		}

		#endregion

		public override string ToString()
		{
			return string.Format("{0},{1} 0x{2:X4}", this.Cmd, this.Param, this.GetHashCode());
		}

		#region ISerializable 成员

		public void Serialize(System.IO.Stream s)
		{
			s.Write(this.Cmd);
			s.Write(this.Param);
		}

		public void Deserialize(System.IO.Stream s)
		{
			s.Read(ref this.cmd);
			s.Read(ref this.param);
		}

		#endregion
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public class MessageTagAttribute : Attribute
	{
		public NetMessageType NetMessageType { get; set; }

		public MessageTagAttribute(byte cmd, byte param)
		{
			this.NetMessageType = new NetMessageType() { Cmd = cmd, Param = param };
		}

		public override string ToString()
		{
			return this.NetMessageType.ToString();
		}
	}
}
