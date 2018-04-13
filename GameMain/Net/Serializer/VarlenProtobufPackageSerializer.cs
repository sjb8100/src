using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Common;
using UnityEngine;
//using Cmd;

namespace Common.Net
{
	public class VarlenProtobufPackageSerializer
	{
		public event EventHandler<EventArgs<Exception>> Error;

		static VarlenProtobufPackageSerializer()
		{
            //ProtoBuf.Serializer.PrepareSerializer<Pmd.ForwardNullUserPmd_CS>();
		}  
		
		/// <summary>
		/// 第一条消息不压缩不加密
		/// </summary>
		private uint count = 0;

        public bool Write(Pmd.ForwardNullUserPmd_CS package, Engine.INetLink netLink,bool isFirst)
		{
            if (netLink==null)
            {
                Engine.Utility.Log.Error("严重错误：NetLink为空。");
                return false;
            }

			count++;
            if(isFirst)
            {
                Engine.Utility.Log.Error("isfirst bitmask Bitmask_Reconnect。");
                package.bitmask |= (int)Pmd.FrameHeader.Bitmask_Reconnect;
            }
			// 消息压缩
			if (package.data.Length > 32 && count != 1)
			{
                //var zip = Common.ZlibCodec.Encode(package.data);
                //if (zip.Length < package.data.Length)
                //{
                //    package.bitmask |= (int)Pmd.FrameHeader.Bitmask_Compress;
                //    package.data = zip;
                //}
			}

			using (var mem = new MemoryStream())
			{
                package.SerializeLengthDelimited(mem);

				//Console.WriteLine("SEND: {0}\t{1}", mem.Length, BitConverter.ToString(mem.ToArray()));
               // Engine.Utility.Log.Error("SEND: {0}\t{1}", mem.Length, BitConverter.ToString(mem.ToArray()));
                
                Engine.PackageOut pack = new Engine.PackageOut(mem,(int)package.byCmd);
                netLink.SendMsg(pack);
			}

			return true;
		}

        internal virtual void OnError(Exception ex)
        {
            if (Error != null)
                Error(this, new EventArgs<Exception>() { Data = ex });
        }
	}
}
