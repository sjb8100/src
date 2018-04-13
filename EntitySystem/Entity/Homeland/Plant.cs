using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    class Plant : HomeEntityBase,IPlant
    {
        private Engine.Utility.SecurityInt[] m_PlantProp = new Engine.Utility.SecurityInt[PlantProp.End - PlantProp.Begin];

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)PlantProp.Begin || nPropID >= (int)PlantProp.End)
            {
                return;
            }

            m_PlantProp[nPropID - (int)PlantProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)PlantProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)PlantProp.End)
            {
                return m_PlantProp[nPropID - (int)PlantProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < PlantProp.End - PlantProp.Begin; ++i)
            {
                m_PlantProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Plant;
        }
    }
}
