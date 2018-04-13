using System;
using System.Text;
using System.Collections;
using System.Linq;
using Engine.Utility;
using Engine;
using Client;
using Common;
using UnityEngine;
using GameCmd;

/// <summary>
/// 网络褚传输压缩类型
/// </summary>
public enum ComPressType
{
    None = 0,
    Flate ,
    GZip,
    Zlib,
    Lzw,
}
partial class Protocol
{
    public static Pmd.ZoneInfoListLoginUserPmd_S ZoneList { get; private set; }

    [Execute]
    public void Execute(Pmd.ZoneInfoListLoginUserPmd_S cmd)
    {
        //区服信息列表
        //PanelManager.Me.SendMsg(PanelID.LoginPanel,UIMsgID.eZoneData,cmd);
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.LoginPanel, UIMsgID.eZoneData, cmd);
        //LoginPanel.Instance.onGetZoneList(cmd.zonelist);
    }

    [Execute]
    public void Excute(Pmd.RequestUserZoneInfoLoginUserPmd_S cmd)
    {
        Debug.Log("=====================");
        DataManager.Manager<UIPanelManager>().SendMsg(PanelID.LoginPanel, UIMsgID.eZoneData, cmd);
        //PanelManager.Me.SendMsg(PanelID.LoginPanel, UIMsgID.eZoneData, cmd);
    }

    [Execute]
    public void Execute(Pmd.ClientLogUrlLoginUserPmd_S cmd)
    {
    }

    [Execute]
    public void Execute(Pmd.ReturnClientIPLoginUserPmd_S cmd)
    {
        UserData.ClientIP = cmd.pstrip;
    }

    [Execute]
    public  void Execute(GameCmd.stServerReturnLoginFailedLogonUserCmd cmd)
    {
        Log.Error(cmd.byReturnCode.ToString());
    }

    [Execute]
    //登录区服回复
    public void Execute(Pmd.UserLoginReturnOkLoginUserPmd_S cmd)
    {
        //var iport = cmd.gatewayurl.Split(':');
        //Log.Info("帐号验证成功，准备连接到网关");
        ////UserData.GameID = cmd.gameid;
        ////UserData.ZoneID = cmd.zoneid;
        //UserData.GatewayServerIP = iport[0];
        //UserData.GatewayServerPort = int.Parse(iport[1]);
        //UserData.AccountID = cmd.accountid;
        //UserData.LoginTempID = cmd.logintempid;
        //NetService.Instance.Close();

        //Log.Info("account: " + UserData.AccountID);

        ////开始连接网关服
        //NetService.Instance.Connect(UserData.GatewayServerIP, UserData.GatewayServerPort, onConnectedGateServer);
    }

   
    
    [Execute]
    public  void Execute(GameCmd.stSetEncdecLogonUserCmd cmd)
    {
        Log.Info("网关要求的加密方式: " + cmd.enctype);
        LoginDataManager logindata = DataManager.Manager<LoginDataManager>();
        var login = new Pmd.UserLoginTokenLoginUserPmd_C()
        {
            gameid = UserData.GameID,
            zoneid = logindata.CurSelectZoneID,
            accountid = UserData.AccountID,
            logintempid = UserData.LoginTempID,
            timestamp = System.DateTime.Now.ToUnixTime(),
            encrypt = ComPressType.Zlib.ToString().ToLower(),
        };
        login.tokenmd5 = UserData.AccountID.ToString() + UserData.LoginTempID.ToString() + login.timestamp.ToString() + UserData.TokenID.ToString();
        NetService.Instance.Send(login);
    }

    [Execute]
    public void Execute(Pmd.SetServerLangLoginUserPmd_C cmd)
    {
        UserData.GameRegion = cmd.gameregion;
        var lang = cmd.lang.ToLowerInvariant();
        UserData.CmdEncoding = (lang.Contains("utf8") || lang.Contains("utf-8")) ? new System.Text.UTF8Encoding(false) : System.Text.Encoding.GetEncoding("GBK");
    }

    [Execute]
    public  void Execute(GameCmd.stSetServerLangLogonUserCmd cmd)
    {
        UserData.GameRegion = cmd.gameRegion;
        var lang = cmd.lang.ToLowerInvariant();
        UserData.CmdEncoding = (lang.Contains("utf8") || lang.Contains("utf-8")) ? new UTF8Encoding(false) : System.Text.Encoding.GetEncoding("GBK");
    }

    /// <summary>
    ///账号登陆错误
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnSeverLoginErrorRes(stServerLoginErrSelectUserCmd cmd)
    {
        Log.Error(cmd.err_val);
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
        Log.Error("bengin show tips");
        TipsManager.Instance.ShowTips(cmd.err_val);
        Log.Error("showtips end");

        Log.Error("begin logout data");
        DataManager.Manager<LoginDataManager>().LogoutData();
        DataManager.Manager<LoginDataManager>().OnRecieveServerError(cmd.error_no, cmd.err_val);
   
    }

    /// <summary>
    /// 玩家成功登陆回调
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cmd"></param>
    public void OnPlayerLoginSuccess(int eventId,object cmd)
    {
        if (eventId == (int)Client.GameEventID.PLAYER_LOGIN_SUCCESS)
        {
            //GameCmd.stFirstMainUserPosMapScreenUserCmd_S gcmd = cmd as GameCmd.stFirstMainUserPosMapScreenUserCmd_S;
//             Pmd.SetPingTimeNullUserPmd_CS pingcmd = new Pmd.SetPingTimeNullUserPmd_CS();
//             pingcmd.pingmsec = 1;
//             NetService.Instance.Send(pingcmd);
        }
    }


    //-----------------------------------------------------------------------------------------------
    
    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="char_name"></param>
    /// <param name="m_sel_profession"></param>
    /// <param name="m_sel_country"></param>
    /// <param name="m_sel_sex"></param>
    /// <param name="mask"></param>
    public void CreateSelectUser(string char_name, byte m_sel_profession, byte m_sel_country,ushort m_sel_sex,string mask = "")
    {
        Log.Info("请求创建角色: " + char_name + "  m_sel_country  :" + m_sel_country);
        LoginDataManager.createRoleName = char_name;

        NetService.Instance.Send(new stCreateSelectUserCmd()
        {
            strUserName = char_name,
            type = m_sel_profession,
            country = m_sel_country,
            wdFace = m_sel_sex,
            dwMarket = mask,
        });

        //wbw
        ///创建了角色
        StepManager.Instance.m_bIsNewRoleLogin |= StepManager.IS_NEW_ROLE_LOGIN.CREATE;
    }




    [Execute]
    public void OnRecieveServerStateChange(stServerIntoStatusLogonUserCmd cmd) 
    {
        DataManager.Manager<LoginDataManager>().OnRecieveServerStateChange(cmd.status,cmd.error_no,cmd.time);
    }
}
