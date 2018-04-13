using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using Engine.Utility;
using Engine;
using Client;
using Common;
using UnityEngine;
using GameCmd;


partial class Protocol
{

    [Execute]
    public void Execute(GameCmd.stSetChannelAndAccidSelectUserCmd cmd)
    {
        UserData.Accid = cmd.accid;
        UserData.Channel = cmd.channel;
    }

    [Execute]
    public void Execute(GameCmd.stSetServerNameLogonUserCmd cmd)
    {
        //UserData.ZoneName = cmd.servername;
    }

    [Execute]
    public void Execute(GameCmd.stIsServerDebugLogonUserCmd cmd)
    {
        UserData.ServerDebug = cmd.byDebug != 0;
    }

    [Execute]
    public void Execute(GameCmd.stGameTimeTimerUserCmd cmd)
    {
        DateTimeHelper.Instance.ServerTime = (long)cmd.qwGameTime;
        UserData.ReConnTempID = cmd.dwTempID;
    }

    [Execute]
    public void Execute(GameCmd.stRequestUserGameTimeTimerUserCmd cmd)
    {
        NetService.Instance.Send(cmd);
    }

    [Execute]
    public void Execute(GameCmd.stSendClientIpSelectUserCmd cmd)
    {
        UserData.ClientIP = MyConvert.ToIPString(cmd.ip);
    }

    [Execute]
    public void Execute(GameCmd.stServerTimeTimerUserCmd cmd)
    {
        UserData.LoginTempID = cmd.dwServerTime;
        DateTimeHelper.Instance.ServerTime = (long)cmd.dwServerTime;
    }

    [Execute]
    public void Execute(GameCmd.stCountryInfoSelectUserCmd cmd)
    {
        UserData.CountryList = cmd.countryinfo;
        //Log.Info("国家列表:\n" + string.Join("\n", (from i in UserData.CountryList select i.ToString()).ToArray()));
        Log.Info("国家列表:");

        foreach (GameCmd.Country_Info contry_info in UserData.CountryList)
        {
            Log.Info("{0} {1} {2}", contry_info.id, contry_info.pstrName, contry_info.pstrShortName);
        }
    }

    [Execute]
    public void Execute(GameCmd.stRequestCharListLogonUserCmd cmd)
    {
    }

    
    [Execute]
    public void Execute(Pmd.UserLoginReturnFailLoginUserPmd_S cmd)
    {
        var error = string.Empty;
        switch (cmd.retcode)
        {
            case Pmd.LoginReturnFailReason.Password:
                error = "密码错误";
                break;
            case Pmd.LoginReturnFailReason.ServerShutdown:
                error = "区服务器已关闭";
                break;
            case Pmd.LoginReturnFailReason.VersionTooLow:
                error = "客户端游戏版本号太低";
                break;
            case Pmd.LoginReturnFailReason.UserTokenFind:
                {
                    error = "没有找到登陆token,需要重新平台验证";
                }
                break;
            case Pmd.LoginReturnFailReason.UserTokenTempId:
                {
                    error = "token错误";
                }
                break;
            case Pmd.LoginReturnFailReason.UserTokenTimeOut:
                {
                    error = "token已过期";
                }
                break;
            case Pmd.LoginReturnFailReason.LoginDulicate:
                error = "重复登陆";
                break;
            case Pmd.LoginReturnFailReason.NoGatewaytDown:
                error = "没有可用网关";
                break;
            default: break;
        }
        TipsManager.Instance.ShowTips(error);
        DataManager.Manager<LoginDataManager>().OnLogout(true);
       
        Debug.LogError(string.Format("登陆LoginServer失败: #{0} {1}", cmd.retcode, error));
    }

    [Execute]
    public void Execute(GameCmd.stUserInfoSelectUserCmd cmd)
    {
        UserData.ProvinceCity = cmd.area;

//         UserData.RoleList.Clear();
//         for (int i = 0; i < cmd.charInfo.Count; i++)
//         {
//             if (cmd.charInfo[i].lastofftime == 0)
//             {
//                 UserData.CurrentRole = cmd.charInfo[i];
//                 CommonSDKPlaform.Instance.CreateRoleToSDK(cmd.charInfo[i].name);
//                 UserData.RoleList.Insert(0, cmd.charInfo[i]);
//             }
//             else
//             {
//                 UserData.RoleList.Add(cmd.charInfo[i]);
//             }
//         }
     //   UserData.RoleList.AddRange(from i in cmd.charInfo where i.id != 0 select i);

        List<GameCmd.SelectUserInfo> resultList = new List<SelectUserInfo>();
        List<GameCmd.SelectUserInfo> tempRoleList = new List<SelectUserInfo>();

        for (int i = 0; i < cmd.charInfo.Count; i++)
        {
            if (cmd.charInfo[i].lastofftime == 0)
            {
                resultList.Add(cmd.charInfo[i]);
            }
            else
            {
                tempRoleList.Add(cmd.charInfo[i]);
            }
        }

        if (tempRoleList.Count >0)
        {
            tempRoleList.Sort(delegate(GameCmd.SelectUserInfo a, GameCmd.SelectUserInfo b)
            {
                return (int)((long)b.lastofftime - (long)a.lastofftime);
            });
            resultList.Add(tempRoleList[0]);
            tempRoleList.RemoveAt(0);
        }

        if (tempRoleList.Count > 0)
        {
            tempRoleList.Sort(delegate(GameCmd.SelectUserInfo a, GameCmd.SelectUserInfo b)
            {
                return (int)((long)b.level - (long)a.level);
            });
            resultList.AddRange(tempRoleList);
        }

        DataManager.Manager<LoginDataManager>().RoleList.Clear();
        DataManager.Manager<LoginDataManager>().RoleList.AddRange(resultList);
        if(Protocol.instance.IsReconnecting)
        {
            Engine.Utility.Log.Error("重新登录 。。。。");
            Protocol.Instance.SetReconnect(false);
        }
        else
        {
            StepManager.Instance.AddLoginScene(StepManager.CHOOSEROLESCENE, UnityEngine.SceneManagement.LoadSceneMode.Single, OnLoadScene);
        }
   
    }
    void OnLoadScene(GameObject go)
    {

        // 隐藏登录界面
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.LoginPanel);
        // 隐藏等待窗体
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
        
        List < GameCmd.SelectUserInfo > lstRoleList = DataManager.Manager<LoginDataManager>().RoleList;
        if (lstRoleList.Count <= 0)
        {
            //创建界面
            //PanelManager.Me.ShowPanel(PanelID.CreateRolePanel);
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CreateRolePanel);
        }
        else
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CreateRolePanel);
            SelectUserInfo userinfo = lstRoleList[0];
            ServerStatusParam status = DataManager.Manager<LoginDataManager>().CurServerState;
            //角色选择界面
            if (userinfo.lastofftime == 0 && userinfo.name.Equals(LoginDataManager.createRoleName) && status.state == ServerLimit.ServerLimit_Free)
            {
                UserData.CurrentRole = userinfo;
                
                DataManager.Manager<LoginDataManager>().CreateRoleToSDK();

                StepManager.Instance.OnBeginStep(Step.LOAD);

                NetService.Instance.Send(new stSendPhysicalAddressSelectUserCmd()
                {
                    mac = Cmd.ConstDefine.DEFAULT_MAC,
                });

                NetService.Instance.Send(new stClientMachineInfoSelectUserCmd()
                {
                    adapterinfo64 = "",
                    cpuinfo128 = "",
                    meminfo64 = "",
                });

                NetService.Instance.Send(new stLoginSelectUserCmd()
                {
                    data = new ImageCheckData(),
                    charid = userinfo.id,
                    mapid = 1,
                });
                DataManager.Manager<LoginDataManager>().LastLoginCharID = userinfo.id;

                //wbw
                ///创建了角色
                StepManager.Instance.m_bIsNewRoleLogin |= StepManager.IS_NEW_ROLE_LOGIN.LOGIN;

                LoginDataManager.createRoleName = "";
            }
            else
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ChooseRolePanel);
                if (status.state != ServerLimit.ServerLimit_Free)
                {
                    TipsManager.Instance.ShowTips(status.msg);
                    Engine.Utility.Log.Error(status.msg);

                }
            }
        }
    }

    [Execute]
    public void Execute(GameCmd.stCheckNameSelectUserCmd cmd)
    {
        Engine.Utility.Log.Info("Enter Execute stCheckNameSelectUserCmd");

        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.CreateRolePanel, UIMsgID.eLoginCheckName, cmd);
    }

    /// <summary>
    /// 登录步骤
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stLoginStepSelectUserCmd cmd)
    {
        switch (cmd.step)
        {
            case LoginStep.LOGIN_GATEWAY:
                {
                    Log.Info("登录流程到:GATEWAY");
                }
                break;
            case LoginStep.LOGIN_SESSION:
                Log.Info("登录流程到:SESSION");
                break;
            case LoginStep.LOGIN_SUPER:
                Log.Info("登录流程到:SUPER");
                break;
            case LoginStep.LOGIN_RECORD:
                Log.Info("登录流程到:RECORD");
                break;
            case LoginStep.LOGIN_SCENE:
                {
                    Log.Info("登录流程到:SCENE");
                    DataManager.Manager<LoginDataManager>().EnterGame();
                }
             
                break;
            case LoginStep.LOGIN_DONE:
                {
                   
                    Log.Info("登录流程到:DONE");
                    NetService.Instance.Send(new stClientFinishLoadingRequestUserCmd());
                }
              
                break;
        }
    }

    [Execute]
    public void Execute(GameCmd.stDeleteSelectUserCmd cmd)
    {
        // string[] errorMessage = new string[] 
        //{ 
        //    "",
        //    "您是帮主，请解散帮会后再删除角色",
        //    "您是师尊，请解散师门后再删除角色",
        //    "您是族长，请解散家族后再删除角色",
        //    "数字密码错误",
        //    "操作异常，请重试",
        //    "您是会长，不允许删除角色",
        //    "您是团长,请转让职务再删号",
        //    "您是军长,请转让职务再删号",
        //    "您是网吧管理员，不允许删号",
        //    "您处于被禁言状态，不允许删号",
        //    "您的角色正在旅游或已回国，不许从此处删号"
        //};

        //Log.Error(errorMessage[cmd.err_code]);
    }

    [Execute]
    public void Execute(stCurrentLoginSelectUserCmd cmd)
    {
        List<GameCmd.SelectUserInfo> roleList = DataManager.Manager<LoginDataManager>().RoleList;
        for (int i = 0; i < roleList.Count; i++)
        {
            if (roleList[i].id == cmd.charid)
            {
                UserData.CurrentRole = roleList[i];
            }
        }
    }

    //[Execute]
    //public void Execute(GameCmd.stAnswerCountryInfoRelationUserCmd cmd)
    //{
    //    Log.Info("[21,87]发送国家相关的消息");
    //    UserData.CountryState = cmd;
    //}


    /// <summary>
    /// 新手首次登陆
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void Execute(stEndOfInitDataDataUserCmd_CS cmd)
    {
        Log.Info("登录成功，收到所有消息");
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)GameEventID.LOGINSUCESSANDRECEIVEALLINFO);
       // Debug.LogWarning("新手首次登陆 stEndOfInitDataDataUserCmd 未实现：" + cmd.Dump());
    }

    ///// <summary>
    ///// 设置小包裹个数
    ///// </summary>
    ///// <param name="cmd"></param>
    //[Execute]
    //public static void Execute(GameCmd.stSetSmallPackNumUserCmd cmd)
    //{
    //    Debug.LogWarning("该消息为设置小包裹个数，手游无需使用");
    //}

    /// <summary>
    /// 设置被激活仓库数量
    /// </summary>
    /// <param name="cmd"></param>
    //[Execute]
    //public void Execute(GameCmd.stSetStorePackNumPropertyUserCmd cmd)
    //{
    //    Debug.LogWarning("该消息为设置激活仓库数量，手游不适用");
    //}

    // 国家信息
    [Execute]
    public void Execute(GameCmd.stSceneCountryDataDataUserCmd cmd)
    {
        UserData.CurrentCountryID = cmd.countryid;
    }
}

