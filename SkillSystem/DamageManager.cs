using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Utility;
using GameCmd;
namespace SkillSystem
{

    class DamageManager
    {
        public DamageManager()
        {

        }
        Dictionary<long , List<stMultiAttackDownMagicUserCmd_S>> damageDic = new Dictionary<long , List<stMultiAttackDownMagicUserCmd_S>>();

        public void AddDamage(GameCmd.stMultiAttackDownMagicUserCmd_S cmd , long uid)
        {
            if(damageDic.ContainsKey(uid))
            {
                List<stMultiAttackDownMagicUserCmd_S> damageList = damageDic[uid];
                if ( !damageList.Contains( cmd ) )
                    damageList.Add( cmd );
            }
            else
            {
                List<stMultiAttackDownMagicUserCmd_S> damageList = new List<stMultiAttackDownMagicUserCmd_S>();
                damageList.Add( cmd );
                damageDic.Add( uid , damageList );
            }
        }
        public void RemoveDamage(stMultiAttackDownMagicUserCmd_S data , long uid)
        {
            if ( damageDic.ContainsKey( uid ) )
            {
                List<stMultiAttackDownMagicUserCmd_S> damageList = damageDic[uid];
                if ( damageList.Contains( data ) )
                    damageList.Remove( data );
            }
            
        }
        public bool IsContain(stMultiAttackDownMagicUserCmd_S data , long uid)
        {
            if(damageDic.ContainsKey(uid))
            {
                List<stMultiAttackDownMagicUserCmd_S> damageList = damageDic[uid];
                return damageList.Contains( data );
            }
            return false;
        }
        public bool HasContain(HitNode node , long uid)
        {
            if ( damageDic.ContainsKey( uid ) )
            {
                List<stMultiAttackDownMagicUserCmd_S> damageList = damageDic[uid];
                for ( int i = 0; i < damageList.Count; i++ )
                {
                    stMultiAttackDownMagicUserCmd_S cmd = damageList[i];
                    if (cmd.tmpid == node.m_uDamageID)
                    {
                        //RemoveDamage( cmd ,uid);
                        return true;
                    }
                }
            }
           
            return false;
        }
        public stMultiAttackDownMagicUserCmd_S GetDamage(HitNode node,long uid)
        {
            if ( damageDic.ContainsKey( uid ) )
            {
                List<stMultiAttackDownMagicUserCmd_S> damageList = damageDic[uid];
                foreach ( var info in damageList )
                {
                    if (info.tmpid == node.m_uDamageID && info.wdSkillID == node.skill_id)
                    {
                        return info;
                    }
                }
                if ( damageList.Count > 20 )
                    damageList.Clear();
                return null;
            }
            return null;
        }

        public List<stMultiAttackDownMagicUserCmd_S> GetDamageList(long uid)
        {
            if ( damageDic.ContainsKey( uid ) )
            {
                return damageDic[uid];
            }
            
          //  Log.Error( "damagelist is null" );
            return null;
        }
    }
}
