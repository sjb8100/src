
//*************************************************************************
//	创建日期:	2017/12/8 星期五 16:29:54
//	文件名称:	Protocol_RedEnvelope
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using Client;
using GameCmd;
using Common;

partial class Protocol
{
    [Execute]
    public void OnReciveAllRedData(stAllRedPacketChatUserCmd_S cmd)
    {
        DataManager.Manager<RedEnvelopeDataManager>().OnReciveAllRedData(cmd);
    }
    [Execute]
    public void OnNoticeRedEnveLopeInfo(stNoticeRedPacketChatUserCmd_S cmd)
    {
        DataManager.Manager<RedEnvelopeDataManager>().OnNoticeRedEnveLopeInfo(cmd);
    }
    [Execute]
    public void OnNoticeRedEnvelopeEmpty(stRedPacketEmptyChatUserCmd_S cmd)
    {
        DataManager.Manager<RedEnvelopeDataManager>().OnNoticeRedEnvelopeEmpty(cmd);
    }

    [Execute]
    public void OnReciveGrapRedEnvelopeInfo(stRecvRedPacketChatUserCmd_CS cmd)
    {
        DataManager.Manager<RedEnvelopeDataManager>().OnReciveGrapRedEnvelopeInfo(cmd);

    }

    [Execute]
    public void OnReciveRedPacketDetails(stRedPacketInfoChatUserCmd_S cmd)
    {
        DataManager.Manager<RedEnvelopeDataManager>().OnReciveRedPacketDetails(cmd);
    }
    [Execute]
    public void OnRedEnvelopeLog(stRedPacketLogChatUserCmd_S cmd)
    {
        DataManager.Manager<RedEnvelopeDataManager>().OnRedEnvelopeLog(cmd);
    }

    [Execute]
    public void OnAddSysRedPacket(stAddSysRedPacketChatUserCmd_S cmd)
    {
        DataManager.Manager<RedEnvelopeDataManager>().OnAddSysRedPacket(cmd);
    }
    [Execute]
    public void OnRemoveSysRedPacket(stRemoveSysRedPacketChatUserCmd_S cmd)
    {
        DataManager.Manager<RedEnvelopeDataManager>().OnRemoveSysRedPacket(cmd);
    }
}
