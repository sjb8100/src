using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    class Minerals : HomeEntityBase,IMinerals
    {
        private Engine.Utility.SecurityInt[] m_MineralsProp = new Engine.Utility.SecurityInt[MineralsProp.End - MineralsProp.Begin];

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)MineralsProp.Begin || nPropID >= (int)MineralsProp.End)
            {
                return;
            }

            m_MineralsProp[nPropID - (int)MineralsProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)MineralsProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)MineralsProp.End)
            {
                return m_MineralsProp[nPropID - (int)MineralsProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < MineralsProp.End - MineralsProp.Begin; ++i)
            {
                m_MineralsProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Minerals;
        }
    }
}
