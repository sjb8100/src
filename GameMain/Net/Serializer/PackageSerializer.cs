using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common;
using UnityEngine;

namespace Cmd
{
	internal class PackageSerializer : IDisposable
	{
		public event EventHandler<EventArgs<Exception>> Error;
		public event EventHandler<EventArgs<ArraySegment<byte>>> Package;

		public Stream BaseStream { get; private set; }

		public void Run(Stream stream)
		{
			this.BaseStream = stream;
			Parse();
		}

		private void Parse()
		{
			// 解析定长消息头
			try
			{
				var buf = new byte[PackageHead.Size];
				BaseStream.ReadAsync(buf, 0, buf.Length, () =>
					{
						var head = new PackageHead();
						head.Deserialize(buf);
						// 解析变长消息体
						buf = new byte[head.Length];
						BaseStream.ReadAsync(buf, 0, buf.Length, () =>
						{
							var data = new ArraySegment<byte>((head.Flag & PackageFlag.Compress) != 0 ? Common.ZlibCodec.Decode(buf) : buf);
							OnPackage(data);
							// 继续下一条消息解析
							Parse();
						}, OnError);
					}, OnError);
			}
			catch (Exception ex)
			{
				OnError(ex);
			}
		}

		protected virtual void OnPackage(ArraySegment<byte> data)
		{
			if (Package != null)
				Package(this, new EventArgs<ArraySegment<byte>>() { Data = data });
		}

		protected virtual void OnError(Exception ex)
		{
			if (Error != null)
				Error(this, new EventArgs<Exception>() { Data = ex });
		}

		public bool Write(ArraySegment<byte> data)
		{
			return WritePackage(data, BaseStream);
		}

		private static bool WritePackage(ArraySegment<byte> data, Stream stream)
		{
			if (stream == null || stream.CanWrite == false)
				return false;
			var head = new PackageHead();
			if (data.Count > 32)
			{
				head.Flag = PackageFlag.Compress;
				data = new ArraySegment<byte>(Common.ZlibCodec.Encode(data));
			}

			head.Length = data.Count;

			// 因“多个同时进行的异步请求会使请求完成顺序不确定”，此处采用同步写
			// http://msdn.microsoft.com/zh-cn/library/system.io.stream.beginwrite
			var buf = head.Serialize();
			stream.Write(buf, 0, buf.Length);
			stream.Write(data.Array, data.Offset, data.Count);
			return true;
		}

		#region IDisposable 成员

		public void Dispose()
		{
			Error = null; // 析构过程中的报错无视
		}

		#endregion
	}
}
