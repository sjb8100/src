using Client;
using Common;
using Engine;
using GameCmd;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
using Engine.Utility;

partial class Protocol
{

    public IBuffPart GetBuffPart(GameCmd.SceneEntryType et,uint userID)
    {
        IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        if ( es == null )
        {
            Engine.Utility.Log.Error( "GetEntitySystem failed!" );
            return null;
        }

        IEntity entity = EntitySystem.EntityHelper.GetEntity(et, userID);
        //IPlayer player = es.FindPlayer( userID );
        if (entity == null)
        {
            Engine.Utility.Log.Error( "es.FindEntity  useid is !"+userID.ToString() );
            return null;
        }

        IBuffPart buffPart = entity.GetPart(EntityPart.Buff) as IBuffPart;
        return buffPart;
    }
    /// <summary>
    /// 状态信息下行消息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void ReciveBuffState(stSendStateIcoListMagicUserCmd_S cmd)
    {
        IBuffPart bp = GetBuffPart((GameCmd.SceneEntryType)cmd.byType, cmd.dwTempID);
        if(bp != null)
        {
            bp.ReciveBuffState( cmd );
        }
    }
    /// <summary>
    /// 取消某个对象身上的状态图标
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void CancleBuffState(stCancelStateIcoListMagicUserCmd_S cmd)
    {
        BuffDataBase db = GameTableManager.Instance.GetTableItem<BuffDataBase>(cmd.wdState);
        if(db != null)
        {
    
            IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
            if (es == null)
            {
                Engine.Utility.Log.Error("GetEntitySystem failed!");
             
            }

            IEntity entity = EntitySystem.EntityHelper.GetEntity(cmd.byType, cmd.dwTempID);
       
            if (entity == null)
            {
                Engine.Utility.Log.Error("es.FindEntity  useid is !" + cmd.dwTempID.ToString());
             
            }
            else
            {
                Log.LogGroup("ZDY","取消{0}身上的buff {1}===========" , db.strName,entity.GetName());
            }
        }
        else
        {
            Log.Error("找不到buff ===========" + cmd.wdState);
        }
        IBuffPart bp = GetBuffPart((GameCmd.SceneEntryType)cmd.byType, cmd.dwTempID);
        if ( bp != null )
        {
            bp.CancleBuffState( cmd );
        }
    }
    /// <summary>
    /// 手动取消自身状态图标
    /// </summary>
    /// <param name="cmd"></param>
    
    void SendSelfCancleBuffState(stCancelBuffStateMagicUserCmd_C cmd)
    {

    }
}

