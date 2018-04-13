using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cmd.NullCmd
{
	/// <summary>
	/// 包含时间戳的命令基类
	/// </summary>
	/// <remarks>不过出于性能上的考虑现在服务器没有填写该字段</remarks>
	public abstract class stNullUserCmd : t_NullCmd
	{
		/// <summary>
		/// 命令的时间戳
		/// </summary>
		public uint dwTimestamp;

		public override void Serialize(System.IO.Stream s)
		{
			base.Serialize(s);
			s.Write(dwTimestamp);
		}

		public override void Deserialize(System.IO.Stream s)
		{
			base.Deserialize(s);
			s.Read(ref this.dwTimestamp);
		}

	}

	/// <summary>
	/// 没有时戳的protobuf消息
	/// </summary>
    //[CmdTag(0)]
    //[ParamTag(4)]
	public sealed class stProtobufNullUserCmd : stNullUserCmd
	{
		public byte[] data = new byte[0];

		public override void Serialize(System.IO.Stream s)
		{
			base.Serialize(s);
			s.Write((ushort)this.data.Length);
			s.Write(this.data);
		}
		public override void Deserialize(System.IO.Stream s)
		{
			base.Deserialize(s);
			ushort len = 0;
			s.Read(ref len);
			this.data = new byte[len];
			s.Read(ref data);
		}
	}

    //[CmdTag(0)]
    //[ParamTag(1)]
	public class t_ClientNullCmd : t_NullCmd
	{
		public override void Serialize(System.IO.Stream s)
		{
			base.Serialize(s);
		}

		public override void Deserialize(System.IO.Stream s)
		{
			base.Deserialize(s);
		}
	}

	/// <summary>
	/// 所有命令的基类
	/// </summary>
	// 消息号和t_ClientNullCmd重复，都是0,0
	//[CmdTag(0)]
	//[ParamTag(0)]
	public class t_NullCmd : Common.ISerializable
	{
		#region ISerializable 成员

		public virtual void Serialize(System.IO.Stream s)
		{
		}

		public virtual void Deserialize(System.IO.Stream s)
		{
		}

		#endregion
	}

	/// <summary>
	/// 压缩打包消息
	/// </summary>
    //[CmdTag(0)]
    //[ParamTag(2)]
	public class t_ZipCmdPackNullCmd : t_NullCmd
	{
		byte[] data;

		public override void Serialize(System.IO.Stream s)
		{
			base.Serialize(s);
			ushort size = data == null ? (ushort)0 : (ushort)data.Length;
			s.Write(size);
			if (data != null)
				s.Write(data);
		}

		public override void Deserialize(System.IO.Stream s)
		{
			base.Deserialize(s);
			ushort size = 0;
			s.Read(ref size);
			this.data = new byte[size];
			s.Read(ref this.data);
		}


		public IEnumerable<Common.ISerializable> Parse(CommandSerializer cs)
		{
			if (data == null)
				yield break;
			var packages = new List<ArraySegment<byte>>();
			using (var stream = new MemoryStream(data))
			{
				var ps = new PackageSerializer();
				ps.Package += (s, e) => packages.Add(e.Data);
				ps.Run(stream);
			}

			foreach (var p in packages)
			{
				using (var mem = new MemoryStream(p.Array, p.Offset, p.Count, false))
				{
					var cmd = cs.DeserializeCommand(mem);
					if (cmd != null)
						yield return cmd;
				}
			}
		}
	}

	/// <summary>
	/// 没有时戳的protobuf消息
	/// </summary>
    //[CmdTag(0)]
    //[ParamTag(3)]
	public class t_ProtobufNullCmd : t_NullCmd
	{
		public byte[] data = new byte[0];

		public override void Serialize(System.IO.Stream s)
		{
			base.Serialize(s);
			var buf = this.data ?? new byte[0];
			s.Write((ushort)buf.Length);
			s.Write(buf);
		}

		public override void Deserialize(System.IO.Stream s)
		{
			base.Deserialize(s);
			ushort len = 0;
			s.Read(ref len);
			data = new byte[len];
			s.Read(ref data);
		}
	}
}
