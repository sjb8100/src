using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

partial class Protocol
{
    /// <summary>
    /// 心法list
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnHeartSkillList(stSendGodDataUserCmd_S cmd)
    {
        DataManager.Manager<HeartSkillManager>().OnHeartSkillList(cmd);
    }

    /// <summary>
    /// 刷新心法数据
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnRefreshHeartSkillData(stRefreshGodDataUserCmd_S cmd) 
    {
        DataManager.Manager<HeartSkillManager>().OnRefreshHeartSkillData(cmd);
    }

    /// <summary>
    /// 升级心法数据
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnUpgradeHeartSkill(stUpSkillGodDataUserCmd_CS cmd)
    {
        DataManager.Manager<HeartSkillManager>().OnUpgradeHeartSkill(cmd);
    }

    /// <summary>
    /// 重置心法
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnResetHeartSkill(stResetSkillGodDataUserCmd_CS cmd)
    {
        DataManager.Manager<HeartSkillManager>().OnResetHeartSkill(cmd);
    }
}
