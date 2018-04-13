using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameCmd;
using Common;
using Client;
partial class Protocol
{
    #region 许愿树

    #endregion
    #region 挖矿
    [Execute]
    public void OnFindMine(stFindMineHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnFindMine( cmd );
    }
    /// <summary>
    /// 挖矿成功
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnDigMineSuccess(stHoleDataHomeUserCmd_S cmd)
    {
        DataManager.Manager<HomeDataManager>().OnDigMineSuccess( cmd );
    }

    [Execute]
    public void OnRobReport(stNewRobReportHomeUserCmd_S cmd)
    {
        DataManager.Manager<HomeDataManager>().OnRobReport( cmd );
    }

    [Execute]
    public void OnGetMineAtOnce(stImmediMineHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnGetMineAtOnce( cmd );
    }
    [Execute]
    public void OnGainMine(stGainMineHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnGainMine( cmd );
    }

    [Execute]
    public void OpenVipHole(stUnlockMineHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OpenVipHole( cmd );
    }

    [Execute]
    public void OnTreeHelpStatus(stReqFriendTreeListHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnHomeFriendStatusChange(cmd);
    }
    #endregion

    #region 农场
    [Execute]
    public void OnUnlockLand(stUnlockFarmHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnUnlockLand( cmd );
    }
    [Execute]
    public void OnSowLand(stSowHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnSowLand( cmd );
    }

    [Execute]
    public void OnGainPlant(stGainFramHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnGainPlant( cmd );
    }
    [Execute]
    public void OnPlantRipeAtOnce(stImmediRipeHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnPlantRipeAtOnce( cmd );
    }
    #endregion

    #region 牧场
    [Execute]
    public void OnInitAnimalInfo(stPastureDataHomeUserCmd_S cmd)
    {
        DataManager.Manager<HomeDataManager>().OnInitAnimalInfo( cmd );
    }

    [Execute]
    public void OnUnLockAnimalIndex(stUnlockPastureHomeUserCmd_CS  cmd)
    {
        DataManager.Manager<HomeDataManager>().OnUnLockAnimalIndex( cmd );

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HOMELAND_UPDATEANIMAL, null);
    }

    [Execute]
    public void OnFeedAnimal(stFeedHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnFeedAnimal( cmd );
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HOMELAND_UPDATEANIMAL, null);
    }

    [Execute]
    public void OnGainAnimal(stGainAnimalHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnGainAnimal( cmd );

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HOMELAND_UPDATEANIMAL, null);
    }
    [Execute]
    public void OnGainAnimalAtOnce(stImmediGrowHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnGainAnimalAtOnce( cmd );

        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.HOMELAND_UPDATEANIMAL, null);
    }
    #endregion

    #region 通用消息
    [Execute]
    public void OnErrorMessage(stErrorHomeUserCmd_S cmd)
    {
        if ( cmd.val_1 == 0 )
        {
            TipsManager.Instance.ShowTipsById( (uint)cmd.error_no );
        }
        else
        {
            TipsManager.Instance.ShowTipsById( (uint)cmd.error_no , cmd.val_1 );
        }
    }

    [Execute]
    public void OnHomeData(stDataHomeUserCmd_S cmd)
    {
        DataManager.Manager<HomeDataManager>().OnHomeData( cmd );
    }

    //商品价格 客户端请求服务器下发
    [Execute]
    public void OnHomeTradePrice(stProductPriceHomeUserCmd_CS cmd)
    {
        DataManager.Manager<HomeDataManager>().OnHomeTradePrice(cmd);
    }

    //设置种子数量
    [Execute]
    public void OnHomeItem(stSetItemHomeUserCmd_S cmd)
    {
        DataManager.Manager<HomeDataManager>().OnHomeItem(cmd);
    }

    #endregion
}

