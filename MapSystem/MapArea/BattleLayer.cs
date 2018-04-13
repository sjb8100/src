using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace MapSystem
{
    public class BattleLayer : BaseLayer
    {
        public BattleLayer(Transform parent)
            : base(BaseLayer.LayerType.eLayerBattle, parent)
        {

        }


        public override void SaveData(ref string path)
        {
            base.SaveData(ref path);
            // mps 阻挡数据
            ///LayerData blockData = m_layerData;
            if (m_layerData == null)
            {
                return;
            }

            int nOffset = 0;
            FileStream fs = new FileStream(path, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);

            for (int z = 0; z < m_layerData.GridZ; z++)
            {
                for (int x = 0; x < m_layerData.GridX; x++)
                {
                    GridInfo pTemp = m_layerData.GetGrid(x, z);

                    bw.Write(BitConverter.GetBytes(pTemp.IsMask ? (ushort)Layer : (ushort)0), nOffset, 2);
                }
            }
            bw.Close();
            fs.Close();
        }

    }
}