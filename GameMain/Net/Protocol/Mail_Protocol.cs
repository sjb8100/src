using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using UnityEngine;
using GameCmd;
using table;

partial class Protocol
{

    /// <summary>
    /// 添加邮件列表	
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void GetAllMail(stAddListListMailUserCmd_S cmd)
    {
        //存储邮件列表  接收消息
        DataManager.Manager<MailManager>().SetMailDic(cmd.data);
    }


    /// <summary>
    /// 阅读邮件时向服务器发送通知后返回的消息
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void ReadMailResponse(stReadTollMailUserCmd_CS cmd)
    {
        DataManager.Manager<MailManager>().ReadMailFinish(cmd.mailid);
    }



    /// <summary>
    /// 删除附件--->删除邮件
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void DeleteMail(stRemoveItemMailUserCmd_S cmd) {
        DataManager.Manager<MailManager>().OnCollectAttachment(cmd.mailid);
    }


    [Execute]
    public void AddNewMail(stNotifyNewMailUserCmd_S cmd) 
    {
        DataManager.Manager<MailManager>().GetNewMail(cmd);
    }

    [Execute]
    public void RecieveDelMail(stDelMailUserCmd_CS cmd) 
    {
        DataManager.Manager<MailManager>().DelMail(cmd.mailid);
    }

    [Execute]
    public void Execute(stBroadcastWorldLevelRelationUserCmd_S cmd)
    {
        DataManager.Manager<MailManager>().OnRecWorldLevel(cmd.world_level);
    }
}