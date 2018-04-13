using System;
using System.Collections.Generic;
using System.Text;
using Client;

namespace EntitySystem
{
    class EquipPart : IEquipPart
    {
        private IEntity m_Master = null;

        private Dictionary<int, int> m_dicEquip = new Dictionary<int, int>();

        /**
        @brief 
        @param 
        */
        public void Release()
        {
            m_dicEquip.Clear();
        }

        /**
        @brief 获取PartID
        */
        public EntityPart GetPartID()
        {
            return EntityPart.Equip;
        }

        /**
        @brief 创建
        @param 
        */
        public bool Create(IEntity master)
        {
            m_Master = master;
            return true;
        }
        // 更新
        public void Update(float dt)
        { 
        }

        // 换装数据
        public void ChangeEquip(int nPos, int id)
        {
            if(m_dicEquip.ContainsKey(nPos))
            {
                m_dicEquip[nPos] = id;
            }
            else
            {
                m_dicEquip.Add(nPos, id);
            }
        }
        //-------------------------------------------------------------------------------------------------------
        public int GetEquip(int nPos)
        {
            int id = 0;
            if(m_dicEquip.TryGetValue(nPos,out id))
            {
                return id;
            }

            return 0;
        }
    }
}
