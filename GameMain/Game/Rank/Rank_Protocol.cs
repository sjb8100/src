//********************************************************************
//	创建日期:	2016-10-19   17:58
//	文件名称:	Rank_Protocol.cs
//  创 建 人:   刘超	
//	版权所有:	中青宝
//	说    明:	排行榜协议
//********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Client;
partial class Protocol
{
    /// <summary>
    /// 接受排行榜消息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void RecieveRankMsg(stAnswerOrderListRelationUserCmd_S cmd) 
    {
        DataManager.Manager<RankManager>().SetRankData(cmd);
    }
    [Execute]
    public void RecieveShakePhone(stMoneyTreePropertyUserCmd_S cmd) 
    {
        DataManager.Manager<TreasureManager>().OnBeginShakePhone(cmd.npc_id);
    }

  
}
    

