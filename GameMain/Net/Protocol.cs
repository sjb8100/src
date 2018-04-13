using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
using Common;
using Engine.Utility;
using Client;
public partial class Protocol : MonoBehaviour, ITimer
{

    //接收消息 主类
    static Protocol instance;
    public static Protocol Instance
    {
        get
        {
            if (instance == null)
            {
                var gb = new GameObject("NetProtocol");
                instance = gb.AddComponent<Protocol>();
            }
            return instance;
        }
    }
    public bool IsDebug = false;

    CommonWaitDataManager m_waitData
    {
        get
        {
            return DataManager.Manager<CommonWaitDataManager>();
        }
    }
    void OnEnable()
    {
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, OnPlayerLoginSuccess);
    }

    void OnDisable()
    {
        Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, OnPlayerLoginSuccess);
    }
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    #region 心跳
    /// <summary>
    /// 是否正在重连
    /// </summary>
    bool bReconnect = false;
    public bool IsReconnecting
    {
        get
        {
            return bReconnect;
        }
        set
        {
            bReconnect = value;
            if (!bReconnect)
            {
                Protocol.Instance.StartHeartBeat();
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
                TipsManager.Instance.HideTipWindow();
            }
            else
            {
                ShowWaitPanel();
            }
        }
    }
  public  void ShowWaitPanel()
    {
        WaitPanelShowData data = new WaitPanelShowData();
        data.type = WaitPanelType.Reconnect;
        data.cdTime =(int) m_waitData.WaitPanelCDTime;
        data.des = "重连中...";
        data.timeOutDel = ShowReconnectTips;
        data.useBoxMask = true;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: data);
    }
    void ShowReconnectTips()
    {
        StopReconnectServerTimer();
        TipsManager.Instance.ShowTipWindow(0, m_waitData.WaitReconnectCDTime, Client.TipWindowType.CancelOk, "是否继续连接服务器",
            () =>
            {
                if(IsReconnecting)
                {
                    if (DataManager.Manager<CommonWaitDataManager>().ReconnectTime == m_waitData.ShowWaitPanelTimes)
                    {
                        DataManager.Manager<CommonWaitDataManager>().ReconnectTime = 0;
                        DataManager.Manager<LoginDataManager>().Logout(LoginPanel.ShowUIEnum.StartGame);
                    }
                    else
                    {
                        ShowWaitPanel();
                        StartReconnectServerTimer();
                    }
                }
        

            }, () =>
            {
                if(IsReconnecting)
                {
                    CloseHeartMsg();
                    DataManager.Manager<LoginDataManager>().Logout(LoginPanel.ShowUIEnum.StartGame);
                }
            });
    }
    /// <summary>
    /// 设置重连标志
    /// </summary>
    /// <param name="bRec">false 是重连成功，ture表示开始重连 在重连状态中</param>
    /// islogin 表示是否重新走登录流程
    /// <param name="?"></param>
    public void SetReconnect(bool bRec, bool isLogin = false)
    {
        IsReconnecting = bRec;
        if (!bRec)
        {
            if(isLogin)
            {
                Log.Error("重连成功 清空除ui和登录数据  ResetByReconnect");
                DataManager.Instance.ResetByReconnect();
            }
            else
            {
                Log.Error("重连成功 不清除数据");
            }
         
            stReconnectSucess rs = new stReconnectSucess();
            rs.isLogin = isLogin;
            EventEngine.Instance().DispatchEvent((int)Client.GameEventID.RECONNECT_SUCESS, rs);
        }
     
    }
    /// <summary>
    /// 心跳间隔时间（ms)
    /// </summary>
    //readonly uint heartTime = 2000;
    //重连服务器间隔 2s
   // readonly uint reconnectSeverTime = 2000;


    public bool StopReconnectSever
    {
        get
        {
            return bStopReconnectSever;
        }
        set
        {
            bStopReconnectSever = value;
        }
    }
    //断线时是否停止连接服务器
    bool bStopReconnectSever = false;
    //是否发送心跳
    bool bSendHeart = true;
    /// <summary>
    /// 开始心跳探测
    /// </summary>
    public void StartHeartBeat()
    {
        bSendHeart = true;
        Debug.LogError("开启心跳探测。。。。");
        Engine.Utility.TimerAxis.Instance().SetTimer(m_uHeartTimerID, m_waitData.NetHeartTime, this);
    }

    public uint SetHeartBeat(uint uHeartTime)
    {
        Engine.Utility.TimerAxis.Instance().SetTimer(m_uHeartTimerID, uHeartTime, this);
        return m_waitData.NetHeartTime;
    }
    /// <summary>
    /// 停止心跳
    /// </summary>
    public void StopSendHeartMsg()
    {
        Debug.LogError("停止心跳发送。。。。");
        Engine.Utility.TimerAxis.Instance().KillTimer(m_uHeartTimerID, this);
    }
    /// <summary>
    /// 停止重连服务器
    /// </summary>
    public void StopReconnectServerTimer()
    {
        Debug.LogError("停止重连游戏服。。。。");
        Engine.Utility.TimerAxis.Instance().KillTimer(m_uReconnectTimerID, this);
    }
    /// <summary>
    /// 开启重连服务器定时器
    /// </summary>
    public void StartReconnectServerTimer()
    {
        uint callTime = m_waitData.WaitPanelCDTime * 1000 / m_waitData.NetReconnectSeverTime;// 10*1000/2000
        Log.Error("重连次数" + callTime);
        Engine.Utility.TimerAxis.Instance().SetTimer(m_uReconnectTimerID, m_waitData.NetReconnectSeverTime, this, callTime);
    }
    void OnApplicationPause(bool pauseStatus)
    {
        Engine.Utility.Log.Error("OnApplicationPause " + pauseStatus);
        if (pauseStatus)
        {

        }
        else
        {
            Debug.LogError("应用激活，检测心跳");
            //进入前台
            //OnTimerSendHeart();

            //TimerAxis.Instance().SetTimer(m_uActiveTimerID, 1000, this, 1);
        }
    }
    void OnTimerSendHeart()
    {
        if (bSendHeart)
        {
            bSendHeart = false;

            NetService.Instance.Send(new Pmd.TickRequestNullUserPmd_CS()
            {
                requesttime = DateTime.Now.ToUnixTime(),
            });
        }
        else
        {
            OnExcuteCloseSocket();
        }

    }
    public void OnExcuteCloseSocket()
    {
        CloseHeartMsg();
        DataManager.Manager<CommonWaitDataManager>().ReconnectTime = 0;
        Debug.LogError("断线了。。。。");
        IsReconnecting = true;
        EventEngine.Instance().DispatchEvent((int)GameEventID.NETWORK_CONNECTE_CLOSE);
        if (!StopReconnectSever)
        {
            ////重连游戏服
            DataManager.Manager<LoginDataManager>().ReconnectGameSever();
            //重连5次
            StartReconnectServerTimer();
        }
    }
    void OnTimerReconnectGameSever()
    {
        if (IsReconnecting)
        {
            //重连游戏服
            DataManager.Manager<LoginDataManager>().ReconnectGameSever();
        }
        else
        {
           
            StopReconnectServerTimer();
        }
    }

    public void CloseHeartMsg()
    {
        StopSendHeartMsg();
        NetService.Instance.Close();

    }
    public uint pingValue = 0;
    [Execute]
    public void OnPing(Pmd.SetPingTimeNullUserPmd_CS cmd)
    {
        pingValue = cmd.pingmsec;
    }


    [Execute]
    public void Execute(Pmd.TickReturnNullUserPmd_CS cmd)
    {
        bSendHeart = true;

    }
    [Execute]
    public void Execute(Pmd.TickRequestNullUserPmd_CS cmd)
    {
        bSendHeart = true;
        NetService.Instance.Send(new Pmd.TickReturnNullUserPmd_CS()
        {
            requesttime = cmd.requesttime,
            mytime = DateTime.Now.ToUnixTime(),
        });
    }
    /// <summary>
    /// 设置心跳时间 和 踢出时间 （心跳时间*次数）
    /// </summary>
    public void SetTimetickOut()
    {
        Pmd.SetTickTimeoutNullUserPmd_CS cmd = new Pmd.SetTickTimeoutNullUserPmd_CS();
        cmd.sec = (uint)(m_waitData.NetHeartTime * 0.001f);
        if (Application.isEditor)
        {
            cmd.times = m_waitData.NetTimeTickOutTime;
        }
        else
        {
            cmd.times = m_waitData.NetTimeTickOutTime;
        }
        NetService.Instance.Send(cmd);
    }

    #endregion
    /// <summary>
    /// 发送网络消息
    /// </summary>
    /// <param name="cmd"></param>
    public void SendCmd(ProtoBuf.IExtensible cmd)
    {
        Client.ClientGlobal.Instance().netService.Send(cmd);
    }

    /// <summary>
    /// 游戏服通知重连成功 重新登录
    /// </summary>
    /// <param name="cmd"></param>
    //[Execute]
    //public void OnReconnectSucess(stReConnectedDataUserCmd_S cmd)
    //{
    //    //Log.Error("重新登录 清空所有数据");
    //    //DataManager.Instance.Reset();
    //    //stReconnectSucess rs = new stReconnectSucess();
    //    //rs.isLogin = true;
    //    //EventEngine.Instance().DispatchEvent((int)Client.GameEventID.RECONNECT_SUCESS, rs);
    //}
    /// <summary>
    /// 反盗号提示
    /// </summary>
    /// <param name="cmd"></param>
    [Execute]
    public void OnRelogin(stUserReLoginLogonUserCmd cmd)
    {
        Log.Error("该账号在其他地方登陆");
      
        DataManager.Manager<LoginDataManager>().Logout(LoginPanel.ShowUIEnum.StartGame,act: () => {
            TipsManager.Instance.ShowTips("该账号在其他地方登陆");
        });
      
    }



    [Execute]
    public void ShowErrorMsg(stCommonErrorRequestUserCmd_S cmd)
    {
        if (cmd.error_no == (int)GameCmd.ErrorEnum.NotEnoughCoin)
        {
            DataManager.Manager<Mall_HuangLingManager>().ShowRecharge((uint)cmd.error_no, "提示", "去充值", "取消");
        }
        else if (cmd.error_no == (int)GameCmd.ErrorEnum.NotEnoughGold)
        {
            DataManager.Manager<Mall_HuangLingManager>().ShowExchange((uint)cmd.error_no, ClientMoneyType.Gold, "提示", "去兑换", "取消");
        }
        else if (cmd.error_no == (int)GameCmd.ErrorEnum.NotEnoughMoney)
        {
            DataManager.Manager<Mall_HuangLingManager>().ShowExchange((uint)cmd.error_no, ClientMoneyType.Wenqian, "提示", "去兑换", "取消");
        }
        else if (cmd.error_no == (int)GameCmd.ErrorEnum.LackTradeGold)
        {
            DataManager.Manager<Mall_HuangLingManager>().ShowExchange((uint)cmd.error_no, ClientMoneyType.YinLiang, "提示", "去兑换", "取消");
        }
        else
        {
            if (string.IsNullOrEmpty(cmd.err_val))
            {
                TipsManager.Instance.ShowTipsById((uint)cmd.error_no);
            }
            else
            {
                TipsManager.Instance.ShowTipsById((uint)cmd.error_no, cmd.err_val);
            }
        }
    }
}

