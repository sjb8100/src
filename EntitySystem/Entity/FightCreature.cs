using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    class FightCreature : Creature
    {
        private Engine.Utility.SecurityInt[] m_FightCreatureProp = new Engine.Utility.SecurityInt[FightCreatureProp.End - FightCreatureProp.Begin];

        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)FightCreatureProp.Begin || nPropID >= (int)FightCreatureProp.End)
            {
                return;
            }

            m_FightCreatureProp[nPropID - (int)FightCreatureProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)FightCreatureProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)FightCreatureProp.End)
            {
                return m_FightCreatureProp[nPropID - (int)FightCreatureProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < FightCreatureProp.End - FightCreatureProp.Begin; ++i)
            {
                m_FightCreatureProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
    }
}
