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
    public void RecieveRankMsg(stSendRewardQuestionPropertyUserCmd_S cmd)
    {
        DataManager.Manager<QuestionManager>().RecieveQuestionData(cmd.question_info,cmd.feedbook_status,cmd.feedbook_num);
    }
}