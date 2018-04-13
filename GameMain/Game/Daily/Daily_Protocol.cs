using System;
//********************************************************************
//	创建日期:	2016-11-15   14:46
//	文件名称:	Daily_Protocol.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	日常系统
//********************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Client;
partial class Protocol
{
    /// <summary>
    /// 请求活动奖励(宝箱)
    /// </summary>
    [Execute]
    public void SendDailyRequest(stRequestLivenessRewardDataUserCmd_CS cmd)
    {
        DataManager.Manager<DailyManager>().GetRewardBox(cmd.rewardid);
    }

    /// <summary>
    /// 刷新单个活跃
    /// </summary>
    [Execute]
    public void RecieveSingleDailyResponse(stRefreshLivenessDataUserCmd_S cmd) 
    {
        DataManager.Manager<DailyManager>().RefreshSingleActivity(cmd);
    }
    /// <summary>
    /// 返回全部活动活跃度数据
    /// </summary>
    [Execute]
    public void RecieveDailyReponse(stLivenessDataDataUserCmd_S cmd)
    {
        DataManager.Manager<DailyManager>().GetAllLivenessData(cmd);
    }

    [Execute]
    public void OnBindPhoneRes(stBindPhoneNumDataUserCmd_CS cmd) 
    {
        DataManager.Manager<DailyManager>().GetBindPhoneRes(cmd);
    }
    [Execute]
    public void OnUseTreasureMapOver(stUseCangBaoTuPropertyUserCmd_S cmd) 
    {
        DataManager.Manager<TreasureManager>().UseTreasureMapOver();
    }
    [Execute]
    public void Execute(stFreshDayHuntingCoinPropertyUserCmd_CS cmd) 
    {
        DataManager.Manager<DailyManager>().DayHuntingCoin = cmd.hunting_coin;
    }

    #region 聚宝-世界Boss
    /// <summary>
    /// 鼓舞玩家
    /// </summary>
    /// <param name="inspireType"></param>
    public void InspirePlayerReq(InspireType inspireType)
    {
        stInspirePlayerCopyUserCmd_CS cmd = new stInspirePlayerCopyUserCmd_CS()
        {
            inspire_type = (uint)inspireType,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 鼓舞成功
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnInspirePlayerRes(stInspirePlayerCopyUserCmd_CS msg)
    {
        DataManager.Manager<JvBaoBossWorldManager>().OnInspirePlayer((InspireType)msg.inspire_type, msg.times);
    }

    /// <summary>
    /// 服务器下发当前鼓舞数据
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnInspirePlayerDataReceive(stInspirePlayerDataCopyUserCmd_S msg)
    {
        DataManager.Manager<JvBaoBossWorldManager>().OnInspirePlayerDataReceive(msg.data);
    }

    /// <summary>
    /// 聚宝世界boss开启通知
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnWorldBossBeginNotice(stWorldBossBeginCopyUserCmd_CS msg)
    {
        DataManager.Manager<JvBaoBossWorldManager>().OnWorldBossBeginNotice(msg.id);
    }

    /// <summary>
    /// 同意参加世界Boss聚宝
    /// </summary>
    public void JoinWorldBossReq(uint worldBosssTabID)
    {
        stWorldBossBeginCopyUserCmd_CS cmd = new stWorldBossBeginCopyUserCmd_CS()
        {
            id = worldBosssTabID,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 世界Boss出现剩余时间
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnWorldBossSpawnLeftTime(stWorldBossTimeCopyUserCmd_S msg)
    {
        DataManager.Manager<JvBaoBossWorldManager>().OnWorldJvBaoBossSpawnLeftTime(msg.remain_time);
    }

    /// <summary>
    /// 世界Boss出现副本开始
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnWorldBossAppearCopyStart(stWorldBossAppearCopyUserCmd_S msg)
    {
        DataManager.Manager<JvBaoBossWorldManager>().OnWorldBossSpawnCopyStart();
    }

    /// <summary>
    /// 世界Boss排行
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnWorldBossDamRans(stWorldBossDamRankCopyUserCmd_S msg)
    {
        DataManager.Manager<JvBaoBossWorldManager>().OnWorldJvBaoDamRankRefresh(msg.data, msg.self_rank, msg.self_damage);
    }

    /// <summary>
    /// 请求世界Boss状态信息
    /// </summary>
    public void WorldBossStatusInfoReq()
    {
        stReqWorldBossInfoCopyUserCmd_CS cmd = new stReqWorldBossInfoCopyUserCmd_CS();
        SendCmd(cmd);
    }

    /// <summary>
    /// 服务器下发世界Boss信息
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnWorldBossStatusInfoRes(stReqWorldBossInfoCopyUserCmd_CS msg)
    {
        DataManager.Manager<JvBaoBossWorldManager>().OnWordBossStatusInfo(msg.info);
    }

    /// <summary>
    /// 刷新单个Boss状态信息
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnRefreshSingleBossStatusInfosRes(stRefWorldBossInfoCopyUserCmd_S msg)
    {
        DataManager.Manager<JvBaoBossWorldManager>().OnSingleWorldBossStatusChanged(msg.info, true);
    }

    /// <summary>
    /// 世界聚宝boss被击杀
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnJvBaoBossDead(stWorldBossDeadCopyUserCmd_S msg)
    {
        DataManager.Manager<JvBaoBossWorldManager>().OnJvBaoBossDead();
    }
    #endregion
}
