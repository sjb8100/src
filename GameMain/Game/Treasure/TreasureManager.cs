using System;
using System.Collections.Generic;
using System.Linq;
using Client;
using UnityEngine;
using System.Text;
using table;

public class TreasureManager : BaseModuleData, IManager
{
    BaseItem curTreasureMap = null;
    bool arriveTargetStopMove = false;
    float ShakePhoneCD = 0f;
    public bool ShakingPhone { set; get; }
    #region 
   public void Initialize()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
      
    }
   public void Reset(bool depthClearData = false) 
    {
       // Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE, OnEvent);
    }



   public void ClearData() 
    {
      
    }

   void OnEvent(int nEventId, object param) 
   {
       Client.GameEventID eid = (Client.GameEventID)nEventId;
       if (eid == Client.GameEventID.ENTITYSYSTEM_ENTITYSTOPMOVE)
       {
           Client.stEntityStopMove stopEntity = (Client.stEntityStopMove)param;
           OnPlayerStopMove(ref stopEntity, nEventId);
           return;
       }
   
   }
    #endregion


   public void UseTreasureMap(BaseItem item) 
    {
        if (item.IsTreasureMap)
        {          
            curTreasureMap = item;
            curTreasureMap.MapSortID = 1;
            arriveTargetStopMove = false;
            GotoMap((int)item.MapID, item.QWThisID, item.TargetPosition);                
        }
        else 
        {
            Engine.Utility.Log.Error("此物品不属于藏宝图类型");
        }
    }

    void GotoMap(int mapID,uint qwThisID,Vector2 vec)
    {
        if (mapID != 0)
        {
            IController ctrl = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
            IMapSystem mapSystem = Client.ClientGlobal.Instance().GetMapSystem();
             Vector3 tarPos3 = new Vector3(vec.x, 0, -vec.y);
             if (mapSystem.GetMapID() != mapID)
             {
                 DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
                 {
                     ctrl.GoToMapPosition((uint)mapID, qwThisID, tarPos3);
                 }, null);
                
             }
             else 
             {
                 DataManager.Manager<RideManager>().TryUsingRide(delegate(object o)
                 {
                     ctrl.GoToMapPosition((uint)mapID, qwThisID, tarPos3);
                 }, null);
                
             }
        }
    }
 

     void OnPlayerStopMove(ref Client.stEntityStopMove stopEntity, int nEventId) 
    {
         if(curTreasureMap == null)
         {
             return;
         }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.ROBOTCOMBAT_SEARCHPATH, false);
        Vector3 mainPos = Client.ClientGlobal.Instance().MainPlayer.GetPos();
        if (mainPos.x == curTreasureMap.TargetPosition.x && mainPos.z == -curTreasureMap.TargetPosition.y)
        {
            if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.MainUsePanel))
            {
                return;
            }
            if (arriveTargetStopMove)
            {
                return;
            }         
            MainUsePanelData mainUsePanelData = new MainUsePanelData();
            mainUsePanelData.type = 2;
            mainUsePanelData.Id = curTreasureMap.BaseId;
            mainUsePanelData.onClick = MainUsePanelItemOnClick;
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MainUsePanel, data: mainUsePanelData);
            arriveTargetStopMove = true;
        }
    }
    void MainUsePanelItemOnClick()
    {
        Client.IEntity player = Client.ClientGlobal.Instance().MainPlayer;
        if (player == null)
        {
            return;
        }
        bool ismoving = (bool)player.SendMessage(Client.EntityMessage.EntityCommand_IsMove, null);
        if (ismoving)
        {
            player.SendMessage(Client.EntityMessage.EntityCommand_StopMove, player.GetPos());
        }
        if(curTreasureMap != null)
        {


            DataManager.Manager<RideManager>().TryUnRide(
              (obj) =>
              {
                  VerifyPlayAni();
              },
              null);                  
        }
    }

    void VerifyPlayAni() 
    {
        DataManager.Manager<SliderDataManager>().SetSliderDelagate(() =>
        {
            DataManager.Instance.Sender.UseItem(MainPlayerHelper.GetPlayerID(), (uint)GameCmd.SceneEntryType.SceneEntry_Player, curTreasureMap.QWThisID, 1);
        }, null, 3f);
        Client.stUninterruptMagic stparam = new Client.stUninterruptMagic();
        stparam.time = 2000;
        stparam.type = GameCmd.UninterruptActionType.UninterruptActionType_CangBaoTuCJ;
        stparam.uid = MainPlayerHelper.GetPlayerUID();
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SKILLGUIDE_PROGRESSSTART, stparam);
         

    }

    public void UseTreasureMapOver() 
    {
        if (curTreasureMap == null)
        {
            Engine.Utility.Log.Error("没有使用过的藏宝图，无法进行下一次自动挖宝");
            return ;
        }
        BaseItem nextMap = null;
        List<BaseItem> m_lst_maps = DataManager.Manager<ItemManager>().GetItemListByItemType();
        if (m_lst_maps.Count >0)
        {
            m_lst_maps.Add(curTreasureMap);
            m_lst_maps.Sort(MapSort);
            int index = m_lst_maps.IndexOf(curTreasureMap);
            if (index + 1 < m_lst_maps.Count)
            {
                nextMap = m_lst_maps[index + 1];
            }
            else if (index + 1 == m_lst_maps.Count)
            {
                nextMap = m_lst_maps[0];
            }
            if (nextMap != null)
            {
                UseTreasureMap(nextMap);
            }
        }           
    }

    int MapSort(BaseItem left , BaseItem right) 
    {
        if (left.MapID != right.MapID)
        {
            return (int)left.MapID - (int)right.MapID;
        }
        else 
        {
            if (left.MapIndex != right.MapIndex)
            {
                return (int)left.MapIndex - (int)right.MapIndex;
            }
            else 
            {
                //保证当前在挖的这个图是这个地方的所有图中最优先的
                return right.MapSortID - left.MapSortID;
            }
        }
    }


  

    public void OnBeginShakePhone(uint treeEntityID)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ShakePhonePanel, data: treeEntityID);
        ShakePhoneCD = GameTableManager.Instance.GetGlobalConfig<uint>("ShakePhoneCD")*1.0f;
        ShakingPhone = true;
    }
    public void Process(float deltaTime)
    {
        if (ShakingPhone)
        {
            ShakePhoneCD -= deltaTime;
            if (ShakePhoneCD <= 0)
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.ShakePhonePanel);
                ShakingPhone = false;
            }
        }
    }
}
