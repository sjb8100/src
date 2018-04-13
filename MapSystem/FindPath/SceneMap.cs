using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using Common;
using System.Runtime.InteropServices;
using UnityEngine.Profiling;

namespace SceneMap
{
    #region 基础数据结构定义
    /// <summary>
    /// 地图文件头结构定义
    /// </summary>
    public class stMapFileHeader : ISerializable
    {
        public const uint MAP_VERSION = 6;
        public static readonly uint MAP_MAGIC = BitConverter.ToUInt32(System.Text.Encoding.ASCII.GetBytes("SPMX"), 0);

        /// <summary>
        /// 文件标识  MAP_MAGIC
        /// </summary>
        public uint magic = MAP_MAGIC;
        /// <summary>
        /// 版本 MAP_VERSION
        /// </summary>
        public uint ver = MAP_VERSION;
        public uint width;
        public uint height;

        #region ISerializable 成员

        public void Serialize(System.IO.Stream s)
        {
            s.Write(magic);
            s.Write(ver);
            s.Write(width);
            s.Write(height);
        }

        public void Deserialize(System.IO.Stream s)
        {
            s.Read(ref magic);
            s.Read(ref ver);
            s.Read(ref width);
            s.Read(ref height);
        }

        #endregion
    }

    /// <summary>
    /// 格子阻挡
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct stSrvMapBlock// : ISerializable
    {
        public ushort block; 
        public TileType Tile
        {
            get { return (TileType)block; }
            set { block = (ushort)value; }
        }
        /// <summary>
        /// 格子阻挡属性
        /// </summary>
        public byte flags
        {
            get { return (byte)((block & 0xFF00) >> 8); }
            set { block = (ushort)((block & ~0xFF00) | ((value << 8) & 0xFF00)); }
        }
        public byte flags2
        {
            get { return (byte)((block & 0x00FF) >> 0); }
            set { block = (ushort)((block & ~0x00FF) | ((value << 0) & 0x00FF)); }
        }

        #region ISerializable 成员

        public void Serialize(System.IO.Stream s)
        {
            s.Write(block);

        }

        public void Deserialize(System.IO.Stream s)
        {
            s.Read(ref block);
        }

        #endregion
    }

    /// <summary>
    /// 格字数据结构
    /// </summary>
    public class stSrvMapZone : ISerializable
    {
        public byte resid0;
        public byte resid1;
        /// <summary>
        /// 格子功能属性
        /// </summary>
        public uint type;

        #region ISerializable 成员

        public void Serialize(System.IO.Stream s)
        {
            s.Write(resid0);
            s.Write(resid1);
            s.Write(type);
        }

        public void Deserialize(System.IO.Stream s)
        {
            s.Read(ref resid0);
            s.Read(ref resid1);
            s.Read(ref type);
        }

        #endregion
    }

    /// <summary>
    /// 格字数据结构
    /// </summary>
    public class stSrvMapTile : ISerializable
    {
        public stSrvMapBlock block = new stSrvMapBlock();
        public stSrvMapZone zone = new stSrvMapZone();

        #region ISerializable 成员

        public void Serialize(System.IO.Stream s)
        {
            s.Write(block);
            s.Write(zone);
        }

        public void Deserialize(System.IO.Stream s)
        {
            s.Read(ref block);
            s.Read(ref zone);
        }

        #endregion
    }

    public class Point : ISerializable
    {
        public uint x;
        public uint y;
        #region ISerializable 成员

        public void Serialize(System.IO.Stream s)
        {
            s.Write(x);
            s.Write(y);
        }

        public void Deserialize(System.IO.Stream s)
        {
            s.Read(ref x);
            s.Read(ref y);
        }

        #endregion
    }

    /// <summary>
    /// 快速寻路点
    /// </summary>
    public class GuidePoints : ISerializable
    {
        public class stGuidePos : ISerializable
        {
            public Point pos = new Point();
            public Repeated<uint, Point> link = new Repeated<uint, Point>();

            #region ISerializable 成员

            public void Serialize(System.IO.Stream s)
            {
                s.Write(pos);
                s.Write(link);
            }

            public void Deserialize(System.IO.Stream s)
            {
                s.Read(ref pos);
                s.Read(ref link);
            }

            #endregion
        }
        public Repeated<uint, stGuidePos> points = new Repeated<uint, stGuidePos>();

        #region ISerializable 成员

        public void Serialize(System.IO.Stream s)
        {
            s.Write(points);
        }

        public void Deserialize(System.IO.Stream s)
        {
            s.Read(ref points);
        }

        #endregion
    }
    #endregion


    public partial class MPS : ISerializable
    {
        public stMapFileHeader head = new stMapFileHeader();
        public stSrvMapBlock[,] tiles = new stSrvMapBlock[0, 0];

        #region ISerializable 成员

        public virtual void Serialize(System.IO.Stream s)
        {
            head.width = (uint)tiles.GetLength(0);
            head.height = (uint)tiles.GetLength(1);
            s.Write(head);

            for (var y = 0; y < head.height; y++)
            {
                for (var x = 0; x < head.width; x++)
                {
                    s.Write(tiles[x, y]);
                }
            }
        }

        public virtual void Deserialize(System.IO.Stream s)
        {
            s.Read(ref head);

            tiles = new stSrvMapBlock[head.width, head.height];


            for (var y = 0; y < head.height; y++)
            {
                for (var x = 0; x < head.width; x++)
                {
                    s.Read(ref tiles[x, y]);
                }
            }


        }

        public void Deserialize_MemoryStream(System.IO.MemoryStream s)
        {

            byte[] buff = new byte[4];

            s.Read(buff, 0, 4);
            head.magic = BitConverter.ToUInt32(buff, 0);

            s.Read(buff, 0, 4);
            head.ver = BitConverter.ToUInt32(buff, 0);
  
            s.Read(buff, 0, 4);
            head.width = BitConverter.ToUInt32(buff, 0);

            s.Read(buff, 0, 4);
            head.height = BitConverter.ToUInt32(buff, 0);

            //s.Read(out head.magic);
            //s.Read(out head.ver);
            //s.Read(out head.width);
            //s.Read(out head.height);

            tiles = new stSrvMapBlock[head.width, head.height];

            //Profiler.BeginSample("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
            //ushort[,] xxx = new ushort[head.width, head.height];
            //Profiler.EndSample();
                 
            byte[] ushortBuff = new byte[2];
            for (var y = 0; y < head.height; y++)
            {
                for (var x = 0; x < head.width; x++)
                {
                    //s.Read(out tiles[x, y].block);
                    s.Read(ushortBuff, 0, 2);
                    tiles[x, y].block = BitConverter.ToUInt16(ushortBuff, 0);
                }
            }

        }

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < this.head.height; y++)
            {
                for (var x = 0; x < this.head.width; x++)
                {
                    var tile = this.tiles[x, y];
                    sb.Append(tile.flags2 == 0x03 ? "03" : "..");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public class WPS : MPS
    {
        public GuidePoints guide = new GuidePoints();

        #region ISerializable 成员

        public override void Serialize(System.IO.Stream s)
        {
            base.Serialize(s);
            s.Write(guide);
        }

        public override void Deserialize(System.IO.Stream s)
        {
            base.Deserialize(s);
            s.Read(ref guide);
        }

        #endregion
    }

    public class AR : ISerializable
    {
        public stMapFileHeader head = new stMapFileHeader();
        public stSrvMapZone[,] tiles = new stSrvMapZone[0, 0];

        #region ISerializable 成员

        public void Serialize(System.IO.Stream s)
        {
            head.width = (uint)tiles.GetLength(0);
            head.height = (uint)tiles.GetLength(1);
            s.Write(head);
            for (var j = tiles.GetLowerBound(1); j <= tiles.GetUpperBound(1); j++)
                for (var i = tiles.GetLowerBound(0); i <= tiles.GetUpperBound(0); i++)
                    s.Write(tiles[i, j]);
        }

        public void Deserialize(System.IO.Stream s)
        {
            s.Read(ref head);
            tiles = new stSrvMapZone[head.width, head.height];
            for (var j = tiles.GetLowerBound(1); j <= tiles.GetUpperBound(1); j++)
                for (var i = tiles.GetLowerBound(0); i <= tiles.GetUpperBound(0); i++)
                    s.Read(ref tiles[i, j]);
        }

        #endregion
    }
}