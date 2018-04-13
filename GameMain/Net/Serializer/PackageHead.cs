using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using Cmd;

namespace Cmd
{
	[Flags]
	internal enum PackageFlag : ushort
	{
		None = 0,
		Compress = 0x4000,
	}

	internal class PackageHead : Common.ISerializable
	{
		public const int Size = 4;
		public const int MaxPackageSize = 0x0000FFFF;

		public PackageFlag Flag { get; set; }
		public int Length { get; set; }

		#region ISerializable 成员

		public void Serialize(System.IO.Stream s)
		{
			checked
			{
				s.Write((ushort)this.Length);
				s.Write((ushort)this.Flag);
			}
		}

		public void Deserialize(System.IO.Stream s)
		{
			ushort temp = 0;
			s.Read(ref temp);
			this.Length = temp;
			s.Read(ref temp);
			this.Flag = (PackageFlag)temp;
		}

		#endregion
	}
}
