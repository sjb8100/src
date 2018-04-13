//*************************************************************************
//	创建日期:	2016/10/18 16:43:07
//	文件名称:	CombatCopy_Protocol
//   创 建 人:   zhuidanyu	
//	版权所有:	中青宝
//	说    明:	CombatCopy_Protocol
//*************************************************************************

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

    [Execute]
    public void OnEnterCopy(stEntertCopyUserCmd_S cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnEnterCopy(cmd);
    }

    [Execute]
    public void OnAskTeamrCopy(stAskTeamrCopyUserCmd_CS cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnAskTeamrCopy(cmd);
    }

    [Execute]
    public void OnAnsTeamCopy(stAnsTeamCopyUserCmd_CS cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnAnsTeamCopy(cmd);
    }


    [Execute]
    public void OnExitCopy(stExitCopyUserCmd_CS cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnExitCopy(cmd);
    }

    [Execute]
    public void OnCompeleteCopy(stCompleteCopyUserCmd_S cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnCompeleteCopy(cmd);
    }
    [Execute]
    public void OnDieInCopy(stDieInCopyUserCmd_S cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnDieInCopy(cmd);
    }
    [Execute]
    public void OnCountDownCopy(stCountDownCopyUserCmd_S cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnCountDownCopy(cmd);
    }

    //[Execute]
    //public void OnAllCopyData(stDataCopyUserCmd_S cmd)
    //{
    //    DataManager.Manager<ComBatCopyDataManager>().OnAllCopyData(cmd);
    //}

    [Execute]
    public void OnCopyNumChange(stNumCopyUserCmd_S cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnCopyNumChange(cmd);
    }

    [Execute]
    public void OnEnterZoneCopyUser(stEnterZoneCopyUserCmd_CS cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnEnterZoneCopyUser(cmd);
    }

    [Execute]
    public void OnNvWaCap(stRefreshCapCopyUserCmd_S cmd)
    {
        DataManager.Manager<NvWaManager>().OnNvWaCap(cmd);
    }

    [Execute]
    public void OnNvWaLvUpGuard(stLvUpGuardCopyUserCmd_CS cmd)
    {
        DataManager.Manager<NvWaManager>().OnNvWaLvUpGuard(cmd);
    }

    [Execute]
    public void OnNvWaBuyGuard(stBuyGuardCopyUserCmd_CS cmd)
    {
        DataManager.Manager<NvWaManager>().OnNvWaBuyGuard(cmd);
    }

    [Execute]
    public void OnNvWaGuardNum(stNWGuardNumCopyUserCmd_S cmd)
    {
        DataManager.Manager<NvWaManager>().OnNvWaGuardNum(cmd);
    }

    [Execute]
    public void OnStartNvWaCopy(stStarNWCopyUserCmd_CS cmd)
    {
        DataManager.Manager<NvWaManager>().OnStartNvWaCopy(cmd);
    }

    [Execute]
    public void OnNvWaResult(stFinishNWCopyUserCmd_S cmd)
    {
        DataManager.Manager<NvWaManager>().OnNvWaResult(cmd);
    }

    [Execute]
    public void OnWaveStart(stWaveStartCopyUserCmd_S cmd)
    {
        DataManager.Manager<NvWaManager>().OnWaveStart(cmd);
    }

    [Execute]
    public void OnWaveFinish(stWaveFinishCopyUserCmd_S cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnWaveFinish(cmd);
    }

    [Execute]
    public void OnJumpCopy(stJumpCopyUserCmd_S cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnJumpCopy(cmd);
    }

    [Execute]
    public void OnRequestEnterCopy(stRequestEnterCopyUserCmd_C cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().ReqEnterCopy(cmd.copy_base_id);
    }

    [Execute]
    public void OnAllRewardExpCopy(stAllRewardExpCopyUserCmd_S cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnAllRewardExpCopy(cmd);
    }
    
    [Execute]
    public void OnAddGoldCopyMoney(stAddGoldCopyMoneyPropertyUserCmd_S cmd)
    {
        DataManager.Manager<ComBatCopyDataManager>().OnAddGoldCopyMoney(cmd);
    }
    
}
