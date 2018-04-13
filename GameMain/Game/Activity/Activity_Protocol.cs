using System;
using System.Collections.Generic;
//********************************************************************
//	创建日期:	2016-12-5   16:58
//	文件名称:	Activity_Protocol.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	运营活动_充值pro
//********************************************************************
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Client;
partial class Protocol
{
    [Execute]
    public void RecieveAllRechargeData(stRechargeRewardListPropertyUserCmd_S cmd)
    {
        DataManager.Manager<ActivityManager>().RecieveRechargeData(cmd);
    }
    [Execute]
    public void BugGrowth(stBuyGrowGoldPropertyUserCmd_CS cmd) 
    {
        DataManager.Manager<ActivityManager>().IsBuyGrowth(cmd);    
    }
    [Execute]
    public void RecieveSingleActiveData(stRequstRechargePropertyUserCmd_CS cmd) 
    {
        DataManager.Manager<ActivityManager>().RecieveSingleData(cmd);
    }
    [Execute]
    public void GetRewardSuccess(stRequstRechargeRewardPropertyUserCmd_CS cmd) 
    {
        DataManager.Manager<ActivityManager>().OnGetRewardSuccess(cmd);
    }

    [Execute]
    public void OnDailyBugGiftData(stDailyBuyGiftDataUserCmd_S  cmd) 
    {
        DataManager.Manager<ActivityManager>().OnRecieveDailyGiftData(cmd.id);
    }
    [Execute]
    public void OnBuyDailyGift(stBuyDailyGiftDataUserCmd_CS cmd) 
    {
        DataManager.Manager<ActivityManager>().OnBugDailyGift(cmd.id);
    }

    [Execute]
    public void OnFirstRecharge(stFirstRechargePropertyUserCmd_CS cmd)
    {
        DataManager.Manager<ActivityManager>().OnGetFirstRechargeReward(cmd.id);
    }


    [Execute]
    public void Execute(stRechargeRewRetListPropertyUserCmd_S cmd) 
    {
        DataManager.Manager<ActivityManager>().OnRechargeRewRet(cmd);
    }
    [Execute]
    public void Execute(stGetRechargeRewRetPropertyUserCmd_CS cmd)
    {
        DataManager.Manager<ActivityManager>().OnGetRechargeRewRet(cmd.rewardid,cmd.ret);
    }
}