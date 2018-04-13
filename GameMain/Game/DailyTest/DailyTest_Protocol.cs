using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Client;
partial class Protocol
{
    [Execute]
    public void OnFuruizhuInfo(stfuruizhuinfoPropertyUserCmd_CS cmd)
    {
        DataManager.Manager<DailyTestManager>().OnFuruizhuInfo(cmd);
    }

    //[Execute]
    //public void OnDofuruizhu(stRequestDofuruizhuPropertyUserCmd_CS cmd)
    //{
    //   // DataManager.Manager<DailyTestManager>().OnDofuruizhu(cmd);
    //}
}

