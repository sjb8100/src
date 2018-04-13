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
    public void OnAnswerData(stAnswerDataDataUserCmd_S cmd)
    {
        DataManager.Manager<DailyAnswerManager>().OnAnswerData(cmd);
    }

    [Execute]
    public void OnNewQuestionData(stNewQuestionDataUserCmd_CS cmd)
    {
        DataManager.Manager<DailyAnswerManager>().OnNewQuestionData(cmd);
    }

    [Execute]
    public void OnGetAnswerReward(stGetAnswerRewardDataUserCmd_CS cmd)
    {
        DataManager.Manager<DailyAnswerManager>().OnGetAnswerReward(cmd);
    }

    [Execute]
    public void OnGetCoinAndExpReward(stRefreshAnswerDataUserCmd_S cmd)
    {
        DataManager.Manager<DailyAnswerManager>().OnGetCoinAndExpReward(cmd);
    }

     [Execute]
    public void OnCommitAnswer(stCommitAnswerQDataUserCmd_CS cmd)
    {
        DataManager.Manager<DailyAnswerManager>().OnCommitAnswer(cmd);
    }
}

