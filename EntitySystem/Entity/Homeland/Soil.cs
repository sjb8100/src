using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    class Soil : HomeEntityBase,ISoil
    {
        private Engine.Utility.SecurityInt[] m_SoilProp = new Engine.Utility.SecurityInt[SoilProp.End - SoilProp.Begin];

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)SoilProp.Begin || nPropID >= (int)SoilProp.End)
            {
                return;
            }

            m_SoilProp[nPropID - (int)SoilProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)SoilProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)SoilProp.End)
            {
                return m_SoilProp[nPropID - (int)SoilProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < SoilProp.End - SoilProp.Begin; ++i)
            {
                m_SoilProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Soil;
        }
    }
}
