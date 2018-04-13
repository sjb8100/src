using System;
using System.Collections.Generic;
using Client;
using UnityEngine;

namespace EntitySystem
{
    static class CommonCondition
    {
        /// <summary>
        /// 是宠物或者召唤物
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static bool IsPetAndSummon(IEntity en)
        {
            IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                return false;
            }

            //IControllerHelper ch = cs.GetControllerHelper();
            //if ( ch == null )
            //{
            //    return false;
            //}
            //if ( ch.NpcIsPet( en ) )
            //{
            //    return true;
            //}
            INPC npc = en as INPC;
            if (npc != null)
            {
                if (npc.IsPet())
                {
                    return true;
                }
                if (npc.IsSummon())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断npc是不是能攻击 根据表格
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public static bool IsNpcCanAttackByTable(IEntity en)
        {
            INPC npc = en as INPC;
            if (npc != null)
            {
                int modelID = npc.GetProp((int)EntityProp.BaseID);
                table.NpcDataBase ndb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>((uint)modelID);
                if (ndb != null)
                {
                    if (ndb.dwAttackType == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static bool IsTeam(IEntity en)
        {
            IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                return false;
            }

            IControllerHelper ch = cs.GetControllerHelper();
            if (ch == null)
            {
                return false;
            }
            if (ch.IsSameTeam(en))
            {
                return true;
            }
            return false;
        }
        public static IEntity GetMaster(int masterID)
        {
            IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
            if (es != null)
            {
                IPlayer p = es.FindEntity<IPlayer>((uint)masterID);
                return p;
            }
            return null;
        }
    }

    class AllCreatureCondition : IFindCondition<ICreature>
    {

        uint m_nRange;
        public AllCreatureCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(ICreature en)
        {
            if (en.IsDead())
            {
                return false;
            }
            if (CommonCondition.IsPetAndSummon(en))
            {
                return false;
            }
            if (!CommonCondition.IsNpcCanAttackByTable(en))
            {
                return false;
            }
            if (en.IsHide())
            {
                return false;
            }
            INPC npc = en as INPC;
            if (npc != null)
            {
                if (!npc.IsCanAttackNPC())
                {
                    return false;
                }
            }
            if (CommonCondition.IsPetAndSummon(en))
            {
                return false;
            }
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;

            float dis = EntityHelper.GetEntityDistance(player, en);
            if (dis <= m_nRange)
            {
                return true;
            }


            return false;
        }
    }
    class SummonNPCCondition : IFindCondition<INPC>
    {

        uint m_nRange;
        public SummonNPCCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(INPC en)
        {
            if (en.IsDead())
            {
                return false;
            }

            if (en.IsSummon())
            {
                if (!en.BelongOther)
                {
                    return true;
                }
            }

            return false;
        }
    }
    class PetNPCCondition : IFindCondition<INPC>
    {

        uint m_nRange;
        public PetNPCCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(INPC en)
        {
            if (en.IsDead())
            {
                return false;
            }
            //if ( !en.IsCanAttackNPC() )
            //{
            //    return false;
            //}
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                return false;
            }

            IControllerHelper ch = cs.GetControllerHelper();
            if (ch == null)
            {
                return false;
            }
            if (ch.NpcIsMyPet(en))
            {
                float dis = EntityHelper.GetEntityDistance(player, en);
                if (dis <= m_nRange)
                {
                    return true;
                }
            }

            return false;
        }
    }
    class TeamNPCCondition : IFindCondition<INPC>
    {

        uint m_nRange;
        public TeamNPCCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(INPC en)
        {
            if (en.IsDead())
            {
                return false;
            }
            if (!en.IsCanAttackNPC())
            {
                return false;
            }
            if (!CommonCondition.IsNpcCanAttackByTable(en))
            {
                return false;
            }
            if (CommonCondition.IsPetAndSummon(en))
            {
                return false;
            }
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                return false;
            }

            IControllerHelper ch = cs.GetControllerHelper();
            if (ch == null)
            {
                return false;
            }
            if (ch.IsSameTeam(en))
            {
                float dis = EntityHelper.GetEntityDistance(player, en);
                if (dis <= m_nRange)
                {
                    return true;
                }
            }

            return false;
        }
    }
    class TeamPlayerCondition : IFindCondition<IPlayer>
    {

        uint m_nRange;
        public TeamPlayerCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(IPlayer en)
        {
            if (en.IsDead())
            {
                return false;
            }
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            IControllerSystem cs = EntitySystem.m_ClientGlobal.GetControllerSystem();
            if (cs == null)
            {
                return false;
            }

            IControllerHelper ch = cs.GetControllerHelper();
            if (ch == null)
            {
                return false;
            }
            if (ch.IsSameTeam(en))
            {
                float dis = EntityHelper.GetEntityDistance(player, en);
                if (dis <= m_nRange)
                {
                    return true;
                }
            }

            return false;
        }
    }
    class FriendlyNPCBodyCondition : IFindCondition<INPC>
    {

        uint m_nRange;
        public FriendlyNPCBodyCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(INPC en)
        {
            if (en.IsCanAttackNPC())
            {
                return false;
            }
            if (CommonCondition.IsPetAndSummon(en))
            {
                return false;
            }
            if (!CommonCondition.IsNpcCanAttackByTable(en))
            {
                return false;
            }
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart != null)
            {
                int error = 0;
                if (m_skillPart.CheckCanAttackTarget(en, out error))
                {
                    return false;
                }
                else
                {
                    float dis = EntityHelper.GetEntityDistance(player, en);
                    if (dis <= m_nRange && en.IsDead())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    class FriendlyPlayerBodyCondition : IFindCondition<IPlayer>
    {

        uint m_nRange;
        public FriendlyPlayerBodyCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(IPlayer en)
        {
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart != null)
            {
                int error = 0;
                if (m_skillPart.CheckCanAttackTarget(en, out error, true))
                {
                    return false;
                }
                else
                {
                    float dis = EntityHelper.GetEntityDistance(player, en);
                    if (dis <= m_nRange && en.IsDead())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    class FriendlyNPCCondition : IFindCondition<INPC>
    {

        uint m_nRange;
        public FriendlyNPCCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(INPC en)
        {
            if (en.IsDead())
            {
                return false;
            }
            if (!en.IsCanAttackNPC())
            {
                return false;
            }
            if (!CommonCondition.IsNpcCanAttackByTable(en))
            {
                return false;
            }
            if (CommonCondition.IsPetAndSummon(en))
            {
                return false;
            }
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart != null)
            {
                int error = 0;
                if (m_skillPart.CheckCanAttackTarget(en, out error))
                {
                    return false;
                }
                else
                {
                    float dis = EntityHelper.GetEntityDistance(player, en);
                    if (dis <= m_nRange)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    class FriendlyPlayerCondition : IFindCondition<IPlayer>
    {

        uint m_nRange;
        public FriendlyPlayerCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(IPlayer en)
        {
            if (en.IsDead())
            {
                return false;
            }
            if (!CommonCondition.IsTeam(en))
            {
                if (en.IsHide())
                {
                    return false;
                }
            }
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart != null)
            {
                int error = 0;
                if (m_skillPart.CheckCanAttackTarget(en, out error))
                {
                    return false;
                }
                else
                {
                    float dis = EntityHelper.GetEntityDistance(player, en);
                    if (dis <= m_nRange)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    class EnemyNPCCondition : IFindCondition<INPC>
    {

        uint m_nRange;
        public EnemyNPCCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(INPC en)
        {
            if (en.IsDead())
            {
                return false;
            }
            if (!en.IsCanAttackNPC())
            {
                return false;
            }
            if (!CommonCondition.IsNpcCanAttackByTable(en))
            {
                return false;
            }
            if (CommonCondition.IsPetAndSummon(en))
            {//通过判断主人能不能打判断
                return false;
            }
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart != null)
            {
                int error = 0;
                if (!m_skillPart.CheckCanAttackTarget(en, out error))
                {
                    return false;
                }
                else
                {
                    float dis = EntityHelper.GetEntityDistance(player, en);
                    if (dis <= m_nRange)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    class EnemyRobotCondition : IFindCondition<IRobot>
    {

        uint m_nRange;
        public EnemyRobotCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(IRobot en)
        {
            if (en.IsDead())
            {
                return false;
            }
            if (en.IsHide())
            {
                return false;
            }
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart != null)
            {
                int error = 0;
                if (!m_skillPart.CheckCanAttackTarget(en, out error))
                {
                    return false;
                }
                else
                {
                    float dis = EntityHelper.GetEntityDistance(player, en);
                    if (dis <= m_nRange && !en.IsDead())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    class EnemyPlayerCondition : IFindCondition<IPlayer>
    {

        uint m_nRange;
        public EnemyPlayerCondition(uint findRange)
        {
            m_nRange = findRange;
        }
        public bool Conform(IPlayer en)
        {
            if (en.IsDead())
            {
                return false;
            }
            if (en.IsHide())
            {
                return false;
            }
            IPlayer player = EntitySystem.m_ClientGlobal.MainPlayer;
            ISkillPart m_skillPart = player.GetPart(Client.EntityPart.Skill) as ISkillPart;
            if (m_skillPart != null)
            {
                int error = 0;
                if (!m_skillPart.CheckCanAttackTarget(en, out error))
                {
                    return false;
                }
                else
                {
                    float dis = EntityHelper.GetEntityDistance(player, en);
                    if (dis <= m_nRange && !en.IsDead())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    class MonsterCondition : IFindCondition<INPC>
    {
        uint m_rang;
        UnityEngine.Vector3 m_centerpos;
        long m_fileterTarget;
        ISkillPart m_skillPart;

        public MonsterCondition(UnityEngine.Vector3 center, uint rang, long fileterTarget)
        {
            m_centerpos = center;
            m_rang = rang;
            m_fileterTarget = fileterTarget;
            m_skillPart = EntitySystem.m_ClientGlobal.MainPlayer.GetPart(Client.EntityPart.Skill) as ISkillPart;
        }

        public bool Conform(INPC en)
        {
            int error = 0;
            if (!CommonCondition.IsNpcCanAttackByTable(en))
            {
                return false;
            }
            if (en.IsPet() || en.IsSummon())
            {
                int masterID = en.GetProp((int)NPCProp.Masterid);
                IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
                if (es != null)
                {
                    IPlayer p = es.FindEntity<IPlayer>((uint)masterID);
                    if (p != null)
                    {
                        if (EntitySystem.m_ClientGlobal.MainPlayer.GetUID() == p.GetUID())
                        {
                            return false;
                        }
                    }
                }
            }
            if (m_skillPart != null && m_skillPart.CheckCanAttackTarget(en, out error) == false)
            {
                return false;
            }

            float fDis = Vector3.Distance(m_centerpos, en.GetPos());
            if (fDis > m_rang)
            {
                return false;
            }
            return true;
        }
    }

    class PlayerCondition : IFindCondition<IPlayer>
    {
        uint m_rang;
        UnityEngine.Vector3 m_centerpos;
        IPlayer m_mainPlayer;

        ISkillPart m_skillPart;
        public PlayerCondition(UnityEngine.Vector3 center, uint rang, long fileterTarget)
        {
            m_rang = rang;
            m_centerpos = center;
            m_mainPlayer = EntitySystem.m_ClientGlobal.MainPlayer;
            m_skillPart = m_mainPlayer.GetPart(Client.EntityPart.Skill) as ISkillPart;
        }

        public bool Conform(IPlayer en)
        {
            if (en == null)
            {
                return false;
            }
            if (en.IsDead())
            {
                return false;
            }
            int error = 0;
            if (m_skillPart != null && m_skillPart.CheckCanAttackTarget(en, out error) == false)
            {
                return false;
            }

            float fDis = Vector3.Distance(m_centerpos, en.GetPos());
            if (fDis > m_rang)
            {
                return false;
            }
            return !en.IsDead();
        }
    }

    class CreatureCondition : IFindCondition<ICreature>
    {
        uint m_rang;
        UnityEngine.Vector3 m_centerpos;
        IPlayer m_mainPlayer;

        ISkillPart m_skillPart;
        public CreatureCondition(UnityEngine.Vector3 center, uint rang, long fileterTarget)
        {
            m_rang = rang;
            m_centerpos = center;
            m_mainPlayer = EntitySystem.m_ClientGlobal.MainPlayer;
            m_skillPart = m_mainPlayer.GetPart(Client.EntityPart.Skill) as ISkillPart;
        }

        public bool Conform(ICreature en)
        {
            if (en == null)
            {
                return false;
            }
            if (en.IsDead())
            {
                return false;
            }
            int error = 0;
            if (m_skillPart != null && m_skillPart.CheckCanAttackTarget(en, out error) == false)
            {
                return false;
            }

            float fDis = Vector3.Distance(m_centerpos, en.GetPos());
            if (fDis > m_rang)
            {
                return false;
            }
            return !en.IsDead();
        }
    }

}
