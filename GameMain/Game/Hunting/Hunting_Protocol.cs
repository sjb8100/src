//********************************************************************
//	创建日期:	2016-12-5   11:13
//	文件名称:	Hunting_Protocol.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	狩猎Protocol
//********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Client;
using table;
partial class Protocol
{
    [Execute]
    public void RecBossLeftTime(stReqBossRefTimeScriptUserCmd_CS cmd) 
    {
        DataManager.Manager<HuntingManager>().RecieveBossLeftTime(cmd);
    }
    [Execute]
    public void RecieveAllBossInfo(stReqAllBossRefTimeScriptUserCmd_S  cmd) 
    {
        DataManager.Manager<HuntingManager>().RecieveAllBossInfo(cmd);
    }
    [Execute]
    public void HuntingTimes(stRefreshNobleFreeTransTimsPropertyUserCmd_S cmd) 
    {
        DataManager.Manager<HuntingManager>().GetUsedTime(cmd);
    }
//    [Execute]
//     public void RefreshBossState(stReqBossRefTimeScriptUserCmd_CS cmd) 
//     {
//         DataManager.Manager<HuntingManager>().RefreshBossState(cmd);
//     }
    //[Execute]
    //public void UsingMoneyTrans(stRequestTransScriptUserCmd_CS cmd) 
    //{
    //    //table.HuntingDataBase hunt = GameTableManager.Instance.GetTableItem<HuntingDataBase>(cmd.boss_index);
    //    //IController ctrl = ClientGlobal.Instance().GetControllerSystem().GetActiveCtrl();
    //    //ctrl.EnterMapImmediacy(hunt.mapID, 0, hunt.transmitCoordinateX, hunt.transmitCoordinateY);
    //}
}
   
