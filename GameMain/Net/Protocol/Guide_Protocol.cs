/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Net.Protocol
 * 创建人：  wenjunhua.zqgame
 * 文件名：  Guide_Protocol
 * 版本号：  V1.0.0.0
 * 创建时间：2/28/2017 10:06:41 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Client;
using Common;
partial class Protocol
{
    /// <summary>
    /// 发送引导完成ID
    /// </summary>
    /// <param name="guideGroupID"></param>
    public void SendGuideCompleteReq(uint guideGroupID)
    {
        stGuideFuncPropertyUserCmd_C cmd = new stGuideFuncPropertyUserCmd_C()
        {
            groupid = guideGroupID,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 服务器下发引导信息
    /// </summary>
    [Execute]
    public void OnGuideInfoRes(stSendGuideFuncPropertyUserCmd_S msg)
    {
        DataManager.Manager<GuideManager>().OnGuideInfoGet(msg.groupid);
    }


    /// <summary>
    /// 发送新功能开启完成
    /// </summary>
    /// <param name="functionId"></param>
    public void SendNewFuncNoticeCompleteReq(uint functionId)
    {
        stGuideFuncOpnePropertyUserCmd_C cmd = new stGuideFuncOpnePropertyUserCmd_C()
        {
            gid = functionId,
        };
        SendCmd(cmd);
    }

    /// <summary>
    /// 服务器下发新功能开启信息
    /// </summary>
    /// <param name="msg"></param>
    [Execute]
    public void OnNewFuncNoticeInfoRes(stSendGuideFuncOpnePropertyUserCmd_S msg)
    {
        DataManager.Manager<GuideManager>().OnNewFuncOpenInfoGet(msg.gid);
    }
}