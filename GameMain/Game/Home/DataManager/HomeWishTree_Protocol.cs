using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
partial class Protocol
{
    #region 许愿树
    [Execute]
    public void OnRecieveTreeMsg(stTreeDataHomeUserCmd_S cmd)
    {
        DataManager.Manager<HomeDataManager>().OnRecieveTreeMsg(cmd.data);
    }
    /// <summary>
    /// 买树
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnBuyTree(stBuyTreeHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnBuyTree(cmd );
    }

    /// <summary>
    /// 树开始集赞
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnTreeBegin(stTreeBeginHomeUserCmd_CS cmd) 
    {
        DataManager.Manager<HomeDataManager>().OnTreeBegin(cmd.cost_time);
    }


    [Execute]
    public void OnTreeHelp(stHelpTreeHomeUserCmd_CS cmd) 
    {
        DataManager.Manager<HomeDataManager>().OnTreeHelp(cmd.help_who,cmd.tree);
    }
    /// <summary>
    /// 树收获
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnTreeGain(stTreeGainHomeUserCmd_CS cmd) 
    {
        DataManager.Manager<HomeDataManager>().OnTreeGain(cmd.tree, cmd.help_num, cmd.exp);
    }
    
    #endregion

}

