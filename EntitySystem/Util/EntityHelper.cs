using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Client;
using Engine.Utility;

namespace EntitySystem
{


    // 实体相关的辅助方法
    public class EntityHelper
    {
        /// <summary>
        /// 获取实体贴地的位置
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Vector3 GetCloseTerrainPos( IEntity entity )
        {
            return Vector3.zero;
        }

        // 判断实体是否是主角
        public static bool IsMainPlayer(IEntity en)
        {
            if (EntitySystem.m_ClientGlobal==null)
            {
                return false;
            }

            return EntitySystem.m_ClientGlobal.IsMainPlayer(en);
        }

        public static IEntity GetEntity(long uid)
        {
            if (EntitySystem.m_ClientGlobal == null)
            {
                return null;
            }

            IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                return null;
            }

            return es.FindEntity(uid);
        }
        public static int AddEffect(IEntity en, int nEffectID)
        {
            if(en==null)
            {
                return 0;
            }
            //RemoveEffect(en, nEffectID);
            table.FxResDataBase edb = GameTableManager.Instance.GetTableItem<table.FxResDataBase>((uint)nEffectID);
            if (edb != null)
            {
                AddLinkEffect node = new AddLinkEffect();
                table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(edb.resPath);
                if (resDB == null)
                {
                    Engine.Utility.Log.Error("找不到特效资源路径配置{0}", edb.resPath);
                }
                node.strEffectName = resDB.strPath;
                node.strLinkName = edb.attachNode;
                node.nFollowType = (int)edb.flowType;
                node.rotate = new Vector3(edb.rotate[0], edb.rotate[1], edb.rotate[2]);
                node.vOffset = new Vector3(edb.offset[0], edb.offset[1], edb.offset[2]);
                node.strEffectName = resDB.strPath;
                node.strLinkName = edb.attachNode;
                node.bIgnoreRide = false; // 特效要挂在坐骑上
                node.scale = Vector3.one;
                return (int)en.SendMessage(EntityMessage.EntityCommand_AddLinkEffect, node);    
            }

            return 0;
        }

        public static void RemoveEffect(IEntity en, int nLinkID)
        {
            if (en == null && nLinkID == 0)
            {
                return;
            }
            en.SendMessage(EntityMessage.EntityCommand_RemoveLinkEffect, nLinkID);
        }

        // 根据服务器类型和id获取实体对象
        public static IEntity GetEntity(GameCmd.SceneEntryType type,uint id)
        {
            Client.EntityType t = GetEntityEtype(type);
            
            Client.IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
            if(es==null)
            {
                return null;
            }

            switch(t)
            {
                case EntityType.EntityType_Player:
                    {
                        return es.FindPlayer(id);
                    }
                case EntityType.EntityType_NPC:
                    {
                        IEntity en = es.FindNPC(id);
                        if(en == null)
                        {
                            en = es.FindRobot(id);
                        }
                        return en;
                    }
                case EntityType.EntityType_Monster:
                    {
                        return es.FindMonster(id);
                    }
                case EntityType.EntityType_Item:
                    {
                        return es.FindItem(id);
                    }
            }

            return null;
        }

        public static string GetModelPath(uint itemID, Cmd.enmCharSex sex, bool ifJudgeSex = true)
        {
            var modelPath = "";
            var tbl = Table.Query<table.EquipDataBase>().FirstOrDefault(i => i.equipID == itemID);
            if (tbl == null) return modelPath;

            table.ResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>((uint)tbl.apperMale);
            string male = (null != resDB) ? resDB.strPath : ""; ;
            resDB = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>((uint)tbl.apperFemale);
            string female = (null != resDB) ? resDB.strPath : ""; ;
            if (ifJudgeSex)
            {
                modelPath = sex == Cmd.enmCharSex.MALE ? male : female;
            }
            else
            {
                if (tbl != null)
                {
                    modelPath = male;
                }
            }
            return modelPath;
        }

        private static List<IPlayer> lstPlayer = new List<IPlayer>();
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 显示和隐藏其它玩家
        @param 
        */
        public static void ShowOtherPlayer(bool bShow)
        {
            if (EntitySystem.m_ClientGlobal == null)
            {
                return;
            }

            IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                return;
            }

            if (lstPlayer==null)
            {
                lstPlayer = new List<IPlayer>();
            }
            lstPlayer.Clear();
            es.FindAllEntity<IPlayer>(ref lstPlayer);

            for(int i = 0; i < lstPlayer.Count;++i)
            {
                if(lstPlayer[i]==null)
                {
                    continue;
                }

                if(EntitySystem.m_ClientGlobal.IsMainPlayer(lstPlayer[i]))
                {
                    continue;
                }

                lstPlayer[i].SendMessage(EntityMessage.EntityCommand_SetVisible, bShow);
            }
            lstPlayer.Clear();
        }

        private static List<INPC> lstMonster = new List<INPC>();
        //-------------------------------------------------------------------------------------------------------
        /**
        @brief 显示和隐藏怪物
        @param 
        */
        public static void ShowMonster(bool bShow)
        {
            if (EntitySystem.m_ClientGlobal == null)
            {
                return;
            }

            IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                return;
            }

            List<INPC> lstMonster = null;
            if(lstMonster==null)
            {
                lstMonster = new List<INPC>();
            }
            lstMonster.Clear();
            es.FindAllEntity<INPC>(ref lstMonster);

            for (int i = 0; i < lstMonster.Count; ++i)
            {
                if (lstMonster[i] == null)
                {
                    continue;
                }

                if (!lstMonster[i].IsMonster())
                {
                    continue;
                }

                lstMonster[i].SendMessage(EntityMessage.EntityCommand_SetVisible, bShow);
            }
            lstMonster.Clear();
        }

        //-------------------------------------------------------------------------------------------------------
        // 客户端方向转成服务器
        public static byte C2SDirection(Vector3 vecDir)
        {
            var az = vecDir.y;
            az = (90 + 360 - az) % 360;
            byte byDir = (byte)(az / 360.0f * 255);
            return byDir;
        }
        /// <summary>
        /// 获取实体类型
        /// </summary>
        /// <param name="type">proto里面定义的类型</param>
        /// <returns></returns>
        public static EntityType GetEntityEtype(GameCmd.SceneEntryType type)
        {
            if ( type == GameCmd.SceneEntryType.SceneEntry_Player )
            {
                return EntityType.EntityType_Player;
            }
            else if ( type == GameCmd.SceneEntryType.SceneEntry_NPC )
            {
                return EntityType.EntityType_NPC;
            }
            else if ( type == GameCmd.SceneEntryType.SceneEntry_Object )
            {
                return EntityType.EntityType_Null;
            }
            else
            {
                return EntityType.EntityType_Null;
            }
        }
        public static EntityType GetEntityEtype(uint type)
        {
            if ( type == 0 )
            {
                return EntityType.EntityType_Player;
            }
            else if ( type == 1 )
            {
                return EntityType.EntityType_NPC;
            }
            else if ( type == 2 )
            {
                return EntityType.EntityType_Null;
            }
            else
            {
                return EntityType.EntityType_Null;
            }
        }
        public static float GetEntityDistance(IEntity selfEntity,IEntity target)
        {
            if(selfEntity == null)
            {
                Log.Error( "GetEntityDistance  selfEntity is null" );
                return 0;
            }
            if(target == null)
            {
                Log.Error( "GetEntityDistance  target is null" );
                return 0;
            }
            Vector3 selfVec = selfEntity.GetPos();
            Vector3 targerVec = target.GetPos();
            selfVec.Set( selfVec.x , 0 , selfVec.z );
            targerVec.Set( targerVec.x , 0 , targerVec.z );
            return Vector3.Distance( selfVec , targerVec );
        }

        public static long MakeUID(EntityType type,uint id)
        {
            return EntityFactory.Instance().MakeUID(type, id);
        }

        static CirclePlayerContion playercon = new CirclePlayerContion(0);
        /// <summary>
        /// 获取圆形范围内的player
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="list">playerlist集合</param>
        public static void GetPlayerListByCricle(float radius,ref List<IPlayer> list)
        {
            Client.IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
            if ( es == null )
            {
                return ;
            }
            playercon.m_radius = radius;
            es.FindEntityRange<IPlayer>(playercon, ref list);
        }
        static CircleRobotContion robotcon = new CircleRobotContion(0);
        /// <summary>
        /// 获取圆形范围内的robot
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="list">robotlist集合</param>
        public static void GetRobotListByCricle(float radius, ref List<IRobot> list)
        {
            Client.IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
            if (es == null)
            {
                return;
            }
            robotcon.m_radius = radius;
            es.FindEntityRange<IRobot>(robotcon, ref list);
        }

        static CircleMonsterContion monstercon = new CircleMonsterContion(0);
        /// <summary>
        /// 获取圆形范围内的monster
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="list">monsterlist集合</param>
        public static void GetMonsterListByCricle(float radius , ref List<IMonster> list)
        {
            Client.IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
            if ( es == null )
            {
                return;
            }
            monstercon.m_radius = radius;
            es.FindEntityRange<IMonster>(monstercon, ref list);
        }

        static CircleNpcContion npccon = new CircleNpcContion(0);
        /// <summary>
        /// 获取圆形范围内的npc
        /// </summary>
        /// <param name="radius">半径</param>
        /// <param name="list">npclist集合</param>
        public static void GetNpcListByCricle(float radius , ref List<INPC> list)
        {
            Client.IEntitySystem es = EntitySystem.m_ClientGlobal.GetEntitySystem();
            if ( es == null )
            {
                return;
            }
            npccon.m_radius = radius;
            es.FindEntityRange<INPC>(npccon, ref list);
        }
        class CirclePlayerContion : IFindCondition<IPlayer>
        {
           public float m_radius = 0;
            public CirclePlayerContion(float radius)
            {
                m_radius = radius;
            }
            public bool Conform(IPlayer en)
            {
     /*           Vector3 mytempPos = en.GetPos();*/
                IPlayer mainPalyer = EntitySystem.m_ClientGlobal.MainPlayer;
//                 Vector3 targettempPos = mainPalyer.GetPos();
//                 Vector2 mypos = new Vector2( mytempPos.x , mytempPos.z );
//                 Vector2 targetpos = new Vector2( targettempPos.x , targettempPos.z );
                float dis = GetEntityDistance( mainPalyer , en );
                if ( dis < m_radius )
                    return true;
                return false;
            }
        }
        class CircleRobotContion : IFindCondition<IRobot>
        {
            public float m_radius = 0;
            public CircleRobotContion(float radius)
            {
                m_radius = radius;
            }
            public bool Conform(IRobot en)
            {
             /*   Vector3 mytempPos = en.GetPos();*/
                IPlayer mainPalyer = EntitySystem.m_ClientGlobal.MainPlayer;
//                 Vector3 targettempPos = mainPalyer.GetPos();
//                 Vector2 mypos = new Vector2(mytempPos.x, mytempPos.z);
//                 Vector2 targetpos = new Vector2(targettempPos.x, targettempPos.z);
                float dis = GetEntityDistance(mainPalyer, en);
                if (dis < m_radius)
                    return true;
                return false;
            }
        }
        class CircleMonsterContion : IFindCondition<IMonster>
        {
           public float m_radius = 0;
            public CircleMonsterContion(float radius)
            {
                m_radius = radius;
            }
            public bool Conform(IMonster en)
            {
               
                IPlayer mainPalyer = EntitySystem.m_ClientGlobal.MainPlayer;
                float dis = GetEntityDistance( mainPalyer , en );
                if ( dis < m_radius )
                    return true;
                return false;
            }
        }

        class CircleNpcContion : IFindCondition<INPC>
        {
           public float m_radius = 0;
            public CircleNpcContion(float radius)
            {
                m_radius = radius;
            }
            public bool Conform(INPC en)
            {

                IPlayer mainPalyer = EntitySystem.m_ClientGlobal.MainPlayer;
                float dis = GetEntityDistance( mainPalyer , en );
                if ( dis < m_radius )
                    return true;
                return false;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        //private static GameObject s_WorldCoordinate = null;
        //// 画世界坐标系
        //public static void DrawWorldCoordinate(Vector3 pos)
        //{
        //    if(s_WorldCoordinate==null)
        //    {
        //        s_WorldCoordinate = new GameObject("Coordinate");
        //    }


        //}
    }
}
