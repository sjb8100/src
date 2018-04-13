using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
namespace MapSystem
{
    public class BlockLayer : BaseLayer
    {

        public BlockLayer(Transform parent)
            : base(BaseLayer.LayerType.eLayerBlock, parent)
        {

        }

        //     public override void LoadData(ref BinaryReader br)
        //     {
        //         for (int nIndex = 0; nIndex < m_layerData.VertexCountX * m_layerData.VertexCountZ; nIndex++)
        //         {
        //             ushort uValue = br.ReadUInt16();
        //             if (uValue == (ushort)AreaType.eBlock)
        //             {
        //                 m_layerData.SetFlag(nIndex, true);
        //             }
        //             else
        //             {
        //                 m_layerData.SetFlag(nIndex, false);
        //             }
        //         }    
        //   }

        public override void SaveData(ref string path)
        {
            // mps 阻挡数据
            LayerData layerData = m_layerData;
            uint MAP_VERSION = 6;
            uint MAP_MAGIC = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("SPMX"), 0);

            int nOffset = 0;
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(BitConverter.GetBytes(MAP_MAGIC), nOffset, 4);
            bw.Write(BitConverter.GetBytes(MAP_VERSION), nOffset, 4);
            bw.Write(BitConverter.GetBytes(layerData.GridX), nOffset, 4);
            bw.Write(BitConverter.GetBytes(layerData.GridZ), nOffset, 4);

            for (int z = 0; z < layerData.GridZ; z++)
            {
                for (int x = 0; x < layerData.GridX; x++)
                {
                    GridInfo pTemp = layerData.GetGrid(x, z);

                    bw.Write(BitConverter.GetBytes(pTemp.IsMask ? (ushort)Layer : (ushort)0), nOffset, 2);
                }
            }
            bw.Close();
            fs.Close();
        }
    }
}