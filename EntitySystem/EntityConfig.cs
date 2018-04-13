using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace EntitySystem
{
    public class EntityConfig
    {
        public static bool m_bShowMainPlayerServerPosCube = false;          // 显示主角服务器位置盒子
        public static bool m_bShowWorldCoordinate = false;                  // 显示世界坐标系
        public static bool m_bShowSkillEffectExcludeMainPlayer = true;      // 显示除主角外的技能特效
        public static bool m_bBattleLowModel = false;                       // 战场简模  系统内定 自动开启(只针对角色)
        public static bool m_bShowEntity = true;                            // 显示实体  隐藏所有实体包括已有和之后创建的

        // 服务器同步时间
        public static uint m_uServerTime = 0;
        public static float m_fStartTime = 0;
        public static bool m_bForceMove = false;

        // 服务器移动同步时间
        public static uint serverTime
        {
            get
            {
                return m_uServerTime + (uint)((Time.realtimeSinceStartup - m_fStartTime )* 1000);
            }
            set
            {
                m_uServerTime = value;
                m_fStartTime = Time.realtimeSinceStartup;
            }
        }
    }
}
