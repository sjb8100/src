using System;
using System.Collections.Generic;
using Common;
using Engine.Utility;
using GameCmd;

partial class Protocol
{

    /// <summary>
    /// 登录时发的
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(GameCmd.stUserSendAllSevenDayPropertyUserCmd_S cmd)
    {
        WelfareManager manager = DataManager.Manager<WelfareManager>();
        if (manager != null)
        {
            manager.User_OnlineTime = cmd.User_OnlineTime;
            manager.Remain_Time = cmd.remain_time;
            manager.ServerOpenCurrDay = cmd.index;
            manager.CanGetIDs = cmd.id;
            manager.LoginTime = (int)UnityEngine.Time.realtimeSinceStartup;
            if (cmd.data.Count == 0)
            {
                manager.UpdateServerOpenGotReward(0);
            }
            else 
            {
                for (int i = 0; i < cmd.data.Count; i++)
                {
                    manager.UpdateServerOpenGotReward(cmd.data[i].Welfare_id);
                }
            }        
            manager.StartTimer();
            if (cmd.remain_time > 0)
            {
                manager.SevenDayOpenState = true;
            }
        }
    }

    [Execute]
    public void Execute(GameCmd.stServerOpenWelfarePropertyUserCmd_CS cmd)
    {
        if (cmd.receive_state == 1)//領取成功
        {
            DataManager.Manager<WelfareManager>().UpdateServerOpenGotReward(cmd.id, true);
        }
    }
    [Execute]
    public void Execute(stRecruitListRelationUserCmd_S cmd)
    {
        DataManager.Manager<WelfareManager>().RecieveInvitedPlayerData(cmd.other_total_recharge,cmd.list);
    }
    [Execute]
    public void Execute(stRefRecruitRechargeRelationUserCmd_S cmd)
    {
        DataManager.Manager<WelfareManager>().RefreshInvitedPlayerData(cmd.uid,cmd.level, cmd.recharge,cmd.other_total_recharge);
    }

    [Execute]
    public void Execute(GameCmd.stRequestRecruitRelationUserCmd_CS cmd) 
    {
        DataManager.Manager<WelfareManager>().PlayerHadGotInvited(cmd.user_id, cmd.isok == 1,cmd.lv,cmd.profession,cmd.name);

    }

    /// <summary>
    /// 请求领取新服七日活动奖励
    /// </summary>
    /// <param name="nId"></param>
   [Execute]
    public void RequestGetServerOpenReward(uint nId)
    {
        NetService.Instance.Send(new GameCmd.stServerOpenWelfarePropertyUserCmd_CS() { id = nId });
    }


    #region     福利礼包

    [Execute]
    public void Execute(GameCmd.stSendRewardDataUserCmd_S cmd)
    {
        DataManager.Manager<WelfareManager>().LoginTimesOfWeek = cmd.login_times_week;
        DataManager.Manager<WelfareManager>().CurrDayOnlineTime = cmd.online_time;
        DataManager.Manager<WelfareManager>().NewServerLoginTimes = cmd.login_times_ser;
        DataManager.Manager<WelfareManager>().SetNewServerWelfareClose(cmd.login_times_ser == 0);
        DataManager.Manager<WelfareManager>().SignDay = cmd.sign_times_month;
        DataManager.Manager<WelfareManager>().CurrDay = cmd.current_day;
        DataManager.Manager<WelfareManager>().IsSignCurrDay = cmd.sign_today;
        DataManager.Manager<WelfareManager>().LoginTime = (int)UnityEngine.Time.realtimeSinceStartup;
        DataManager.Manager<WelfareManager>().CanGetBindReward = (!cmd.isRewardBind) && cmd.bind_phone;
        DataManager.Manager<DailyManager>().isBindFinish = cmd.bind_phone;
        Engine.Utility.Log.LogGroup("ZCX", "login_times_week:{0}online_time:{1}login_times_ser{2}", cmd.login_times_week, cmd.online_time, cmd.login_times_ser);

        Engine.Utility.Log.LogGroup("ZCX", "SignDay:{0}currday:{1}issign{2}", cmd.sign_times_month, cmd.current_day, cmd.sign_today);
        DataManager.Manager<WelfareManager>().ClearDataList();
        if (cmd.data.Count == 0)
        {
            DataManager.Manager<WelfareManager>().UpdateWelfaregGotReward(0);
        }
        else 
        {
            for (int i = 0; i < cmd.data.Count; i++)
            {
                DataManager.Manager<WelfareManager>().UpdateWelfaregGotReward(cmd.data[i]);
            }   
        }                  
    }

    [Execute]
    public void Execute(GameCmd.stReqGetRewardDataUserCmd_CS cmd)
    {
        if (cmd.is_get)
        {
            for (int i = 0; i < cmd.id.Count; i++)
            {
                DataManager.Manager<WelfareManager>().UpdateWelfaregGotReward(cmd.id[i], i == cmd.id.Count - 1 ? true : false);                
            }
        }
    }

    public void ReqGetReward(ref List<uint> lstIds)
    {
        stReqGetRewardDataUserCmd_CS cmd = new GameCmd.stReqGetRewardDataUserCmd_CS();
        cmd.id.AddRange(lstIds);
        NetService.Instance.Send(cmd);
    }
    public void ReqGetInviteReward(uint lstIds)
    {
        stRecruitRewardRelationUserCmd_CS cmd = new GameCmd.stRecruitRewardRelationUserCmd_CS();
        cmd.reward_id = lstIds;
        NetService.Instance.Send(cmd);
    }

    [Execute]
    public void OnRefreshSevenDayPoint(GameCmd.stSendAllGetWelfareIdPropertyUserCmd_S cmd) 
    {
        WelfareManager manager = DataManager.Manager<WelfareManager>();
        if (manager != null)
        {
            if (cmd.data.Count == 0)
            {
                manager.UpdateServerOpenGotReward(0);
            }
            else
            {
                for (int i = 0; i < cmd.data.Count; i++)
                {
                    manager.UpdateServerOpenGotReward(cmd.data[i].Welfare_id);
                }
            }
        }
    }
    [Execute]
    public void OnFindReward(stSendFindRewardDataUserCmd_S cmd) 
    {
        WelfareManager manager = DataManager.Manager<WelfareManager>();
        if (manager != null)
        {
            manager.ReceiveRewardFindState(cmd.pre_level,cmd.data);
        }
    }
    [Execute]
    public void OnRefreshFindRewardData(stFindRewardDataUserCmd_CS cmd) 
    {
        WelfareManager manager = DataManager.Manager<WelfareManager>();
        if (manager != null)
        {
            manager.RefreshRewardFindState(cmd.find_id,cmd.find_time);
        }
    }
#endregion
    [Execute]
    public void OnOpenPackageSign(stOpenPackageSignDataUserCmd_CS cmd) 
    {
        DataManager.Manager<WelfareManager>().OnOpenPackageSign(cmd.id);
    }
    [Execute]
    public void OnOpenPackageSign(stOpenPackageRewardDataUserCmd_S cmd)
    {
        DataManager.Manager<WelfareManager>().OnOpenPackageSign(cmd.id);
    }




    [Execute]
    public void OnRecieceArtifact(stArtifactRewardDataDataUserCmd_S cmd) 
    {
        DataManager.Manager<WelfareManager>().OnRecieceArtifact(cmd.id,cmd.have_activate,cmd.can_activate);
    }

    [Execute]
    public void OnGetArticfactReward(stGetArticfactRewardDataUserCmd_CS cmd) 
    {
        DataManager.Manager<WelfareManager>().GetArticfactReward(cmd.id,cmd.can_activate);
    }

    [Execute]
    public void OnGetArticfactReward(stActivateArtifactDataUserCmd_CS cmd)
    {
        DataManager.Manager<WelfareManager>().OnJiHuoShenQi();
    }
    [Execute]
    public void OnRecieveQuickLvState(stQuickLevDataDataUserCmd_S cmd)
    {
        DataManager.Manager<WelfareManager>().OnRecieveQuickLvState(cmd.data);
    }
    [Execute]
    public void OnRefreshQuickLvState(stQuickLevWelfareDataUserCmd_S cmd) 
    {
        DataManager.Manager<WelfareManager>().OnRecieveSingleQuickLvState(cmd.id,cmd.state);
    }
    [Execute]
    public void OnRecieveQuickLvNum(stRefLevGiftNumsDataUserCmd_S cmd)
    {
        DataManager.Manager<WelfareManager>().OnRecieveQuickLvNum(cmd.data);
    }

    [Execute]
    public void OnGetRushLv(stGetQuickLevRewardDataUserCmd_CS cmd) 
    {
        DataManager.Manager<WelfareManager>().OnGetRushLv(cmd.id);
    }


    [Execute]
    public void OnrecieveCDKey(stCDKeyExchangeDataUserCmd_CS cmd) 
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.CommonWaitingPanel))
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
        }
    }
    [Execute]
    public void OnCollectWord(stCollectWordDataUserCmd_S cmd)
    {
        DataManager.Manager<WelfareManager>().OnCollectWord(cmd.data);
    }

    [Execute]
    public void OnRecieveFunctionShieldMsg(stsendControlFuctionStatePropertyUserCmd_CS cmd) 
    {
        DataManager.Manager<FunctionShieldManager>().OnRecieveServerShied(cmd.data);
    }
}