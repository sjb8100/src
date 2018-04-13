using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Client;

namespace EntitySystem
{
    class Animal : HomeEntityBase,IAnimal
    {
        private Engine.Utility.SecurityInt[] m_AnimalProp = new Engine.Utility.SecurityInt[AnimalProp.End - AnimalProp.Begin];

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)AnimalProp.Begin || nPropID >= (int)AnimalProp.End)
            {
                return;
            }

            m_AnimalProp[nPropID - (int)AnimalProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)AnimalProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)AnimalProp.End)
            {
                return m_AnimalProp[nPropID - (int)AnimalProp.Begin].Number;
            }

            return 0;
        }
        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < AnimalProp.End - AnimalProp.Begin; ++i)
            {
                m_AnimalProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
        //-------------------------------------------------------------------------------------------------------
        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Animal;
        }

        //-------------------------------------------------------------------------------------------------------
        public override void CreateCommpent()
        {
            base.CreateCommpent();

            AddCommpent(EntityCommpent.EntityCommpent_Move);
        }
    }
}
