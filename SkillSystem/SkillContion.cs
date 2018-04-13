using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using EntitySystem;
using table;
using UnityEngine;
using Engine.Utility;
namespace SkillSystem
{
    /// <summary>
    /// 攻击距离的圆形范围
    /// </summary>
    class SkillCricleContion:IFindCondition<IPlayer>
    {
        public bool Conform(IPlayer en)
        {
            IClientGlobal cg = SkillSystem.GetClientGlobal();
            IEntity player = cg.MainPlayer;
            ISkillPart skillPart = (ISkillPart)player.GetPart( EntityPart.Skill );
            SkillDatabase db = skillPart.GetCurSkillDataBase();
            if(db != null)
            {
                float dis = EntitySystem.EntityHelper.GetEntityDistance( player , en );
                if ( dis <= db.dwAttackDis )
                    return true;
            }
            return false;
        }
    }
    class SkillCricleContionNPC : IFindCondition<INPC>
    {
        public bool Conform(INPC en)
        {
            IClientGlobal cg = SkillSystem.GetClientGlobal();
            IEntity player = cg.MainPlayer;
            ISkillPart skillPart = (ISkillPart)player.GetPart( EntityPart.Skill );
            SkillDatabase db = skillPart.GetCurSkillDataBase();
            if ( db != null )
            {
                float dis = EntitySystem.EntityHelper.GetEntityDistance( player , en );
                if ( dis <= db.dwAttackDis )
                    return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 技能范围类型作为条件
    /// </summary>
    class SkillAreaContion:IFindCondition<IPlayer>
    {
        /*
         * 技能范围 对园和扇形是半径 对于矩形是长
         * 技能范围参数 对园和扇形是角度 对于矩形是宽
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="en">技能目标</param>
        /// <returns></returns>
        public bool Conform(IPlayer en)
        {
            if(en == null)
            {
                Log.LogGroup("ZDY", " confirm player is null");
                return false;
            }
            IClientGlobal cg = SkillSystem.GetClientGlobal();
            IEntity player = cg.MainPlayer;
            ISkillPart skillPart = (ISkillPart)player.GetPart( EntityPart.Skill );
            SkillDatabase db = skillPart.GetCurSkillDataBase();

            Vector3 mytempPos = player.GetPos();
            Vector3 targettempPos = en.GetPos();
            Vector2 mypos = new Vector2( mytempPos.x , mytempPos.z );
            Vector2 targetpos = new Vector2( targettempPos.x , targettempPos.z );
            float dis = EntitySystem.EntityHelper.GetEntityDistance( player , en );
            if ( db != null )
            {
                if(db.areaType == 0)
                {
                    //点
                }
                else if(db.areaType == 1)
                {
                    //矩形
                    Log.Error( "not implementations " );
                    return false;
                }
                else if(db.areaType == 2)
                {
                    Transform trans = player.GetTransForm();
                    if(trans == null)
                    {
                        return false;
                    }
                    //扇形
                    Vector2 myDir = new Vector2(trans.forward.x, trans.forward.z);
                    Vector2 dir = targetpos - mypos;
                    float angle = Vector2.Angle( myDir.normalized , dir.normalized );
                    float dbAngle = db.skillAreaParam/2;
                    if(dbAngle > angle)
                    {
                        if(dis<db.skillArea)
                         return true;
                    }
                    return false;
                }
                else if(db.areaType == 3)
                {
                    //园
                  
                    if ( dis < db.skillArea )
                        return true;
                }
               
            }
            return false;
        }
    }
}
