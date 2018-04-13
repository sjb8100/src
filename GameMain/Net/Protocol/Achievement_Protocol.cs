using System.Collections;
using GameCmd;
using Common;

partial class Protocol
{
    [Execute]
    public void OnResponAchieveData(stAchieveDataDataUserCmd_S cmd)
    {
        DataManager.Manager<AchievementManager>().OnResponAchieveData(cmd);
    }

    [Execute]
    public void OnResponRefreshAchieveData(stRefAchieveDataUserCmd_S cmd)
    {
        DataManager.Manager<AchievementManager>().OnResponRefreshAchieveData(cmd.data,cmd.achieve_num);
    }

    [Execute]
    public void OnResponGetAchieveReward(stGetAchieveRewardDataUserCmd_CS cmd)
    {
        DataManager.Manager<AchievementManager>().OnResponGetAchieveReward(cmd);
    }
}
