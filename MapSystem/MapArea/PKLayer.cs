using UnityEngine;
using System.Collections;

using System.IO;
using System;
namespace MapSystem
{
    public class PKLayer : BaseLayer
    {

        public PKLayer(Transform parent)
            : base(BaseLayer.LayerType.eLayerPK, parent)
        {

        }

        //     public override void LoadData(ref BinaryReader br)
        //     {
        //         for (int nIndex = 0; nIndex < m_layerData.VertexCountX * m_layerData.VertexCountZ; nIndex++)
        //         {
        //             ushort uValue = br.ReadUInt16();
        //             if (uValue == (ushort)AreaType.epk)
        //             {
        //                 m_layerData.SetFlag(nIndex, true);
        //             }
        //             else
        //             {
        //                 m_layerData.SetFlag(nIndex, false);
        //             }
        //         }    
        //     }

        public override void SaveData(ref string path)
        {
            // mps 阻挡数据
            LayerData layerData = m_layerData;
            int nOffset = 0;
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

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