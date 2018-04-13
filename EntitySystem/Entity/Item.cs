using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;

namespace EntitySystem
{
    class Item : Entity, IItem
    {
        // 属性数组
        private Engine.Utility.SecurityInt[] m_ItemProp = new Engine.Utility.SecurityInt[ItemProp.End - ItemProp.Begin];

        public override EntityType GetEntityType()
        {
            return EntityType.EntityType_Item;
        }

        public override bool Create(EntityCreateData data, ColliderCheckType colliderCheckType)
        {
            // 设置属性
            InitProp();
            ApplyProp(data);

            // 创建外观对象
            CreateEntityView(data);

            return base.Create(data, colliderCheckType);
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="nPropID">属性ID</param>
        /// <param name="nValue">属性值</param>
        public override void SetProp(int nPropID, int nValue)
        {
            base.SetProp(nPropID, nValue);

            if (nPropID < (int)ItemProp.Begin || nPropID >= (int)ItemProp.End)
            {
                return;
            }

            m_ItemProp[nPropID - (int)ItemProp.Begin].Number = nValue;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="nPropID">属性ID<</param>
        /// <returns></returns>
        public override int GetProp(int nPropID)
        {
            if (nPropID < (int)ItemProp.Begin)
            {
                return base.GetProp(nPropID);
            }

            if (nPropID < (int)ItemProp.End)
            {
                return m_ItemProp[nPropID - (int)ItemProp.Begin].Number;
            }

            return 0;
        }

        //-------------------------------------------------------------------------------------------------------
        public override void InitProp()
        {
            for (int i = 0; i < ItemProp.End - ItemProp.Begin; ++i)
            {
                m_ItemProp[i] = new Engine.Utility.SecurityInt(0);
            }
            base.InitProp();
        }
    }
}
