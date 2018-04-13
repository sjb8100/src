using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Client;
partial class Protocol
{
    [Execute]
    public void OnRecieveFeedBackMsg(stFeedbackGMPropertyUserCmd_CS cmd) 
    {
        DataManager.Manager<SettingManager>().OnRecieveFeedBackMsg(cmd);
    }

    [Execute]
    public void OnRecieveFeedBackWarningMsg(stFeedbackNoticePropertyUserCmd_S cmd) 
    {

        DataManager.Manager<SettingManager>().OnRecieveFeedBackWarning(cmd);
    }

}
