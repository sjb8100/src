using System;
using System;
using GameCmd;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Client;


partial class Protocol
{
    [Execute]
    public void OnAnswerEnter(stRightWrongEnterCopyUserCmd_CS cmd)
    {
        DataManager.Manager<AnswerManager>().OnAnswerEnter(cmd);
    }

    [Execute]
    public void OnAnswerPreStart(stRightWrongPreStartCopyUserCmd_S cmd)
    {
        DataManager.Manager<AnswerManager>().OnAnswerPreStart(cmd);
    }

    [Execute]
    public void OnAnswerQuestion(stRightWrongQuestionCopyUserCmd_S cmd)
    {
        DataManager.Manager<AnswerManager>().OnAnswerQuestion(cmd);
    }

    [Execute]
    public void OnAnswerAns(stRightWrongAnsCopyUserCmd_S cmd)
    {
        DataManager.Manager<AnswerManager>().OnAnswerAns(cmd);
    }

    [Execute]
    public void OnAnswerResult(stRightWrongResultCopyUserCmd_S cmd)
    {
        DataManager.Manager<AnswerManager>().OnAnswerResult(cmd);
    }


    [Execute]
    public void OnAnswerCurInfo(stRightWrongCurInfoCopyUserCmd_S cmd)
    {
        DataManager.Manager<AnswerManager>().OnAnswerCurInfo(cmd);
    }

    [Execute]
    public void OnAnswerPlayer(stRightWrongPlayerCopyUserCmd_S cmd)
    {
        DataManager.Manager<AnswerManager>().OnAnswerPlayer(cmd);
    }

    [Execute]
    public void OnAnswerReward(stRightWrongRewardCopyUserCmd_CS cmd)
    {
        DataManager.Manager<AnswerManager>().OnAnswerReward(cmd);
    }

    [Execute]
    public void OnAnswerFinish(stRightWrongFinishCopyUserCmd_S cmd)
    {
        DataManager.Manager<AnswerManager>().OnAnswerFinish(cmd);
    }
}
