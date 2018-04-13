
using System;
using System.Collections.Generic;
using Client;
/// <summary>
/// 传送npc触发
/// </summary>
public class EntityOnTrigger : IEntityCallback
{
    const float SQRMagnitude = 1.44f;
    bool IsTrigger = false;
    public void OnMoveEnd(IEntity entity, object param = null)
    {

    }

    // 落地
    public void OnToGround(IEntity entity, object param = null)
    {

    }

    // 飞行结束
    public void OnFlyEnd(IEntity entity, object param = null)   
    {

    }

    public void OnUpdate(IEntity entity, object param = null)
    {
        if (Client.ClientGlobal.Instance().MainPlayer == null || entity == null)
        {
            return;
        }
        //TODO :等级＞VIP等级＞货币
        UnityEngine.Vector3 dis = Client.ClientGlobal.Instance().MainPlayer.GetPos() - entity.GetPos();
        if (dis.sqrMagnitude < SQRMagnitude )
        {
            if (IsTrigger)
            {
                return;
            }

            IsTrigger = true;

            uint npcid = (uint)entity.GetProp((int)Client.EntityProp.BaseID);

            table.DeliverDatabase tal = GameTableManager.Instance.GetTableItem<table.DeliverDatabase>(npcid);

            if (tal != null)
            {
                if (!KHttpDown.Instance().SceneFileExists(tal.dwDestMapID))
                {
                    //打开下载界面
                    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.DownloadPanel);
                    return;
                }
            }
            else 
            {
                Engine.Utility.Log.Error("读DeliverDatabase 表格数据为null");
            }


            if (tal != null)
            {
                for (int i = 0; i < tal.condTypeArray.Count; i++)
                {
                    switch (tal.condTypeArray[i])
                    {
                        case 1://等级
                            if (Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level) < tal.condValueArray[i])
                            {
                                //TODO 弹窗提示
                                return;
                            }
                            break;
                        case 2://vip等级
   
                            break;
                        case 3://消耗货币
 
                            break;
                        default:
                            break;
                    }
                }
            }
            Engine.Utility.Log.Trace("触发传送：{0}", entity.GetName());

            // 请求传送地图 先让主角停止
            if(Client.ClientGlobal.Instance().MainPlayer!=null)
            {
                Client.ClientGlobal.Instance().MainPlayer.SendMessage(EntityMessage.EntityCommand_StopMove, Client.ClientGlobal.Instance().MainPlayer.GetPos());
            }

            NetService.Instance.Send(new GameCmd.stClickNpcScriptUserCmd_C()
            {
                dwNpcTempID = entity.GetID(),
               // baseid = npcid,
            });
        }
        else 
        {
            IsTrigger = false;
        }
    }

    /// <summary>
    /// 发生碰撞
    /// </summary>
    /// <param name="obj">检测对象</param>
    /// <param name="obj">碰撞信息</param>
    public void OnCollider(IEntity obj, ref EntityColliderInfo entityCollierInfo)
    {

    }
}


public class BoxOnTrigger : IEntityCallback
{
    const float SQRMagnitude = 1.44f;
    
    bool IsTrigger = false;
    float m_fTriggerTime = 0;
    public BoxOnTrigger()
    {
        IsTrigger = false;
    }

    public void OnCollider(IEntity obj, ref EntityColliderInfo entityCollierInfo)
    {

    }

    public void OnFlyEnd(IEntity entity, object param = null)
    {
    }

    public void OnMoveEnd(IEntity entity, object param = null)
    {
    }

    public void OnToGround(IEntity entity, object param = null)
    {
    }

    public void OnUpdate(IEntity entity, object param = null)
    {
        if (IsTrigger)
        {
            //如果过了2秒还存在 说明没有捡起
            if (UnityEngine.Time.realtimeSinceStartup - m_fTriggerTime > 2.5f)
            {
                IsTrigger = false;
            }
            return;
        }

        Client.IEntity mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null || mainPlayer.IsDead() || entity == null)
        {
            return;
        }


        IBox box = entity as IBox;
        if (box == null )
        {
            return;
        }

        UnityEngine.Vector3 dis = mainPlayer.GetPos() - entity.GetPos();
        if (dis.sqrMagnitude < SQRMagnitude)
        {
            IsTrigger = true;
            if (box.CanPick())
            {
                m_fTriggerTime = UnityEngine.Time.realtimeSinceStartup;
                NetService.Instance.Send(new GameCmd.stPickUpItemPropertyUserCmd_C() { qwThisID = entity.GetID() });
//                 if (box.CanAutoPick())
//                 {
//                     Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_PICKUPITEM,
//                         new Client.stPickUpItem() { itemid = entity.GetID(), state = 1 });
//                 }
               // UnityEngine.Debug.Log("捡起道具 " + entity.GetID());
            }
        }
    }
}

public class CampNpcOnTrigger : IEntityCallback
{
    float SQRMagnitude = -1f;
    bool IsTrigger = false;
    uint m_nNpcID = 0;
    public void OnCollider(IEntity obj, ref EntityColliderInfo entityCollierInfo)
    {
    }

    public void OnFlyEnd(IEntity entity, object param = null)
    {
    }

    public void OnMoveEnd(IEntity entity, object param = null)
    {
    }

    public void OnToGround(IEntity entity, object param = null)
    {
    }

    public void OnUpdate(IEntity entity, object param = null)
    {
        Client.IEntity mainPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (mainPlayer == null || mainPlayer.IsDead() || entity == null)
        {
            return;
        }

        UnityEngine.Vector3 dis = mainPlayer.GetPos() - entity.GetPos();

        uint npcid = (uint)entity.GetProp((int)Client.EntityProp.BaseID);
        if (npcid != m_nNpcID || SQRMagnitude <= 0f)
        {
            m_nNpcID = npcid;
            table.NpcDataBase npcdb = GameTableManager.Instance.GetTableItem<table.NpcDataBase>(m_nNpcID);
            if (npcdb != null)
            {
                SQRMagnitude = npcdb.dwCallDis * npcdb.dwCallDis * 0.01f * 0.01f;
            }
        }

        if (dis.sqrMagnitude < SQRMagnitude)
        {
            if (IsTrigger)
            {
                return;
            }
            IsTrigger = true;
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.CAMP_ADDCOLLECTNPC,
                new stCampCollectNpc(){ npcid = entity.GetUID(), enter = true});
        }
        else
        {
            if (IsTrigger)
            {
                Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.CAMP_ADDCOLLECTNPC, 
                    new stCampCollectNpc() { npcid = entity.GetUID(), enter = false });
            }
            IsTrigger = false;
        }
    }
}