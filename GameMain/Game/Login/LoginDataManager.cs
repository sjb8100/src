
//*************************************************************************
//	创建日期:	2016/12/20 星期二 9:57:13
//	文件名称:	LoginDataManager
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	登陆接口相关
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using GameCmd;
using UnityEngine;
using Engine.Utility;
using Engine;

public enum LoginSteps
{
    None = 0,
    LGS_Authorize,               //1、sdk平台授权
    LGS_FetchASFilterData,       //2、拉取区服过滤数据
    LGS_Platform,                //3、登陆平台服，获取区服列表
    LGS_SelectZone,              //4、选区
    LGS_GameServer,              //5、登陆游戏服
    LGS_Ready,                   //登陆流程完成
}

public enum LoginState
{
    None = 0,
    LGState_SDKReady,
    LGState_PlatformReady,
}
public class ServerStatusParam 
{
    public ServerLimit state = ServerLimit.ServerLimit_Free;
    public string msg = "";
}
public class LoginDataManager : BaseModuleData, IManager, ITimer
{
    /*
         public class loginResult
    {
        public string token = ""; //session id
        public string uid = "";   //username or user id
        public uint pid = 0;    //platform id
        public string account = "";//for zqgame sdk
        public string szLoginAcccount = ""; //login return
        public string szLoginSession = "";
        public string szLoginDataEx = "";
        public string uiLoginPlatUserID = "";
    }
     */

    const string USERLOGIN_FIRSTKEY = "USERLOGIN";
    const string USERLOGIN_ACCOUNT = "ACCOUNT";
    const string USERLOGIN_ZONEID = "ZONEID";
    const string USERLOGIN_ZONENAME = "ZONENAME";
    public static string createRoleName = "";
    //private uint m_nLastZoneId = 0;
    uint m_loginTimerID = 1000;
    uint m_showWaitPanelTimerID = 10001;
    uint m_ReTryRequestHttpTimerID = 4000;
    //public uint LastZoneId { get { return m_nLastZoneId; } set { m_nLastZoneId = value; } }
    #region LoginResult
    private string m_loginSession;
    public string LoginSession
    {
        get
        {
            return m_loginSession;
        }
    }
    private string m_loginDataEx;
    public string LoginDataEx
    {
        get
        {
            return m_loginDataEx;
        }
    }
    private string m_loginPlatUserID;

    public string LoginPlatUserID
    {
        get
        {
            return m_loginPlatUserID;
        }
    }
    private string m_loginToken;
    public string LoginToken
    {
        get
        {
            return m_loginToken;
        }
    }

    private string m_uid;
    public string LoginUID
    {
        get
        {
            return m_uid;
        }

    }
    private uint m_uPid;
    public uint LoginPid
    {
        get
        {
            return m_uPid;
        }
    }

    private string m_acount;
    public string Acount
    {
        get
        {
            return m_acount;
        }
        set
        {
            m_acount = value;
        }
    }

    /// <summary>
    /// accountID(zqgame ios SDK enable)
    /// </summary>
    private string m_accountID = "";
    public string AccountID
    {
        get
        {
            return m_accountID;
        }
        set
        {
            m_accountID = value;
        }
    }

    private string m_loginAcount;
    public string LoginAcount
    {
        get
        {
            return m_loginAcount;
        }
    }
    private ServerStatusParam curServerState = new ServerStatusParam();
    public ServerStatusParam CurServerState
    {
        get
        {
            return curServerState;
        }
        set 
        {
            curServerState = value;
        }
    }
    #endregion

    #region property
    public string IpStr = "";
    private string m_LoginUrl = "";
    private uint m_uGameID = 3000;

    GX.Net.JsonHttp http;
    GX.Net.MessageDispatcher<object> httpDisPatcher = new GX.Net.MessageDispatcher<object>();
    bool isSdkLogin = false;
    public bool IsSDKLogin
    {
        get
        {
            return isSdkLogin;
        }
    }
    //public bool isLoginSdkDone { get; set; }
    public uint BestZone { get; set; }
    List<Pmd.ZoneInfo> m_lstZoneList = null;
    public List<Pmd.ZoneInfo> ZoneList
    {
        get
        {
            return m_lstZoneList;
        }
        set
        {

            m_lstZoneList = value;
            StructUserAreaServerInfo(m_lstZoneList);
        }
    }

    List<Pmd.UserZoneInfo> m_lstUserZoneInfo = null;
    public List<Pmd.UserZoneInfo> UserZoneInfoList
    {
        get
        {
            return m_lstUserZoneInfo;
        }
        set
        {
            m_lstUserZoneInfo = value;
        }
    }

    /// <summary>
    ///  角色列表
    /// </summary>
    List<GameCmd.SelectUserInfo> m_lstRoleList = null;
    public List<GameCmd.SelectUserInfo> RoleList { get { return m_lstRoleList; } }
    /// <summary>
    /// 上一次登录的角色id
    /// </summary>
    public uint LastLoginCharID;

    //是否为SDK登陆状态
    public bool SDKLoginEnable
    {
        get
        {
            return !Application.isEditor && GlobalConfig.Instance().SDKLogin;
        }
    }

    private bool m_isInit = false;
    public bool IsInit
    {
        get
        {
            return m_isInit;
        }
    }
    #endregion


    #region LoginSteps

    //当前选中区服索引ID
    private int m_currentSelectZoneIndex = -1;
    public int CurrentSelectZoneIndex
    {
        get
        {
            return m_currentSelectZoneIndex;
        }
    }
    //当前选中的区ID
    public uint CurSelectZoneID
    {
        get
        {
            Pmd.ZoneInfo zoneInfo = null;
            if (m_currentSelectZoneIndex != -1)
            {
                zoneInfo = GetZoneInfoByIndex(m_currentSelectZoneIndex);
            }
            return GetBestZoneID(zoneInfo);
        }
    }

    /// <summary>
    /// 当前选中区服原始区ID
    /// </summary>
    public uint CurOriginalZoneID
    {
        get
        {
            Pmd.ZoneInfo zoneInfo = null;
            if (m_currentSelectZoneIndex != -1)
            {
                zoneInfo = GetZoneInfoByIndex(m_currentSelectZoneIndex);
            }
            return GetOriginalZoneID(zoneInfo);
        }
    }

    /// <summary>
    /// 获取当前最优的区服ID
    /// </summary>
    /// <param name="zoneInfo"></param>
    /// <returns></returns>
    public static uint GetBestZoneID(Pmd.ZoneInfo zoneInfo)
    {
        uint enableZoneId = 0;
        if (null != zoneInfo)
        {
            if (zoneInfo.newzoneid != 0)
            {
                //合服产生的新的指向区ID
                enableZoneId = zoneInfo.newzoneid;
            }else
            {
                //没有合服指向的区ID
                enableZoneId = zoneInfo.zoneid;
            }
        }
        return enableZoneId;
    }

    /// <summary>
    /// 获取当前原始区ID
    /// </summary>
    /// <param name="zoneInfo"></param>
    /// <returns></returns>
    public static uint GetOriginalZoneID(Pmd.ZoneInfo zoneInfo)
    {
        uint originalZoneID = 0;
        if (null != zoneInfo)
        {
            //没有合服指向的区ID
            originalZoneID = zoneInfo.zoneid;
        }
        return originalZoneID;
    }

    /// <summary>
    /// 设置当前选中的区服索引
    /// </summary>
    /// <param name="index"></param>
    public void SetCurSelectZoneIndex(int index)
    {
        m_currentSelectZoneIndex = index;
    }

    /// <summary>
    /// 清除当前选中区服索引
    /// </summary>
    public void ClearCurSelectZoneIndex()
    {
        SetCurSelectZoneIndex(-1);
    }

    //当前登陆流程状态
    private LoginSteps m_curStep = LoginSteps.None;
    public LoginSteps CurStep
    {
        get
        {
            return m_curStep;
        }
    }

    /// <summary>
    /// 登陆流程loginStep是否已完成
    /// </summary>
    /// <param name="loginStep"></param>
    /// <returns></returns>
    public bool IsLgoinStepsReady(LoginSteps loginStep)
    {
        return CurStep > loginStep;
    }

    /// <summary>
    /// 登陆流程改变
    /// </summary>
    /// <param name="next"></param>
    /// <param name="cur"></param>
    private void OnLoginStepsChanged(LoginSteps next,LoginSteps cur)
    {
        switch(next)
        {
            case LoginSteps.None:
                break;
            case LoginSteps.LGS_Authorize:
                break;
            case LoginSteps.LGS_FetchASFilterData:
                break;
            case LoginSteps.LGS_Platform:
                break;
            case LoginSteps.LGS_SelectZone:
                break;
            case LoginSteps.LGS_GameServer:
                break;
            case LoginSteps.LGS_Ready:
                break;
        }
    }

    /// <summary>
    /// 设置当前登陆流程
    /// </summary>
    /// <param name="loginStep"></param>
    public void SetLoginSteps(LoginSteps loginStep)
    {
        if (loginStep != m_curStep)
        {
            LoginSteps tempCur = m_curStep;
            m_curStep = loginStep;
            OnLoginStepsChanged(loginStep, tempCur);
        }
        
    }

    /// <summary>
    /// 登陆流程执行完成
    /// </summary>
    /// <param name="success">是否成功</param>
    /// <param name="step">失败</param>
    public void OnLoginStepComplete(bool success ,LoginSteps step)
    {
        switch (step)
        {
            case LoginSteps.None:
                break;
            case LoginSteps.LGS_Authorize:
                OnAuthorizeComplete(success);
                break;
            case LoginSteps.LGS_FetchASFilterData:
                OnFetchASFilterDataComplete(success);
                break;
            case LoginSteps.LGS_Platform:
                OnPlatformComplete(success);
                break;
            case LoginSteps.LGS_SelectZone:
                OnSelectZoneComplete(success);
                break;
            case LoginSteps.LGS_GameServer:
                OnGameServerComplete(success);
                break;
            case LoginSteps.LGS_Ready:
                {
                    if (success)
                    {
                        m_curStep = LoginSteps.LGS_Ready;
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 授权流程完成
    /// </summary>
    /// <param name="success">是否成功</param>
    private void OnAuthorizeComplete(bool success)
    {
        
    }

    /// <summary>
    /// 拉取区服过滤数据完成
    /// </summary>
    /// <param name="success">是否成功</param>
    private void OnFetchASFilterDataComplete(bool success)
    {

    }

    /// <summary>
    /// 登陆平台
    /// </summary>
    /// <param name="success">是否成功</param>
    private void OnPlatformComplete(bool success)
    {

    }

    /// <summary>
    /// 选区
    /// </summary>
    /// <param name="success">是否成功</param>
    private void OnSelectZoneComplete(bool success)
    {

    }

    /// <summary>
    /// 登陆游戏服
    /// </summary>
    /// <param name="success">是否成功</param>
    private void OnGameServerComplete(bool success)
    {

    }

    private LoginState m_curState = LoginState.None;
    public LoginState CurState
    {
        get
        {
            return m_curState;
        }
    }

    /// <summary>
    /// 是否登陆当前登陆状态准备好了
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool IsLoginStateReady(LoginState state)
    {
        return m_curState >= state;
    }

    public void SetLoginState(LoginState state)
    {
        m_curState = state;
    }

    public StateMachine<LoginDataManager> StateMachine
    {
        get
        {
            return stateMachine;
        }
    }
    StateMachine<LoginDataManager> stateMachine;
    void CreateStateMachine()
    {
        stateMachine = new StateMachine<LoginDataManager>(this);
        stateMachine.RegisterState(new LoginStepNone(stateMachine, http));
        stateMachine.RegisterState(new LoginStepAuthorize(stateMachine, http));
        stateMachine.RegisterState(new LoginStepFetchASFilterData(stateMachine, http));
        stateMachine.RegisterState(new LoginStepPlatform(stateMachine, http));
        stateMachine.RegisterState(new LoginStepSelectZone(stateMachine, http));
        stateMachine.RegisterState(new LoginStepGameServer(stateMachine, http));
    }

    #endregion
    public void ClearData()
    {

    }
    public void Initialize()
    {
        m_lstRoleList = new List<GameCmd.SelectUserInfo>();
        HttpProtocol.Instance.Init();
        SetLoginState(LoginState.None);
        httpDisPatcher.Register(HttpProtocol.Instance);
        isSdkLogin = SDKLoginEnable;
        if (IsSDKLogin)
        {
            //SDK初始化
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                //SDK初始化
                CommonSDKPlaform.Instance.Init();
            }
        }

        try
        {
            m_LoginUrl = GlobalConfig.Instance().LoginUrl;
        }catch(Exception e)
        {
            Engine.Utility.Log.Error(e.Message);
        }

        if (string.IsNullOrEmpty(m_LoginUrl))
        {
            Engine.Utility.Log.Error("严重错误 loigurl is null");
            return;
        }
        int ptype = (int)Pmd.PlatType.PlatType_Normal;
        m_acount = GetCacheAccount();

        m_uGameID = (uint)GlobalConfig.Instance().GameID;
        if (m_uGameID == 0)
        {
            Engine.Utility.Log.Error("严重错误 m_uGameID is 0");
            return;
        }
        http = new GX.Net.JsonHttp(m_LoginUrl, m_uGameID, 0, httpDisPatcher);
        if (!isSdkLogin)
        {
            http.PlatInfoFactory = () => new Pmd.PlatInfo()
            {
                uid = m_uid,
                platid = ptype,
                sign = m_loginSession,
                account = m_acount,
            };
        }
        http.OnResponse += http_OnResponse;
        http.OnRequest += http_OnRequest;
        CreateStateMachine();
        m_isInit = true;
    }

    /// <summary>
    /// 登陆平台服务器
    /// </summary>
    public void LoginPlatform()
    {
        if (!IsInit)
        {
            Engine.Utility.Log.Error("SDK init failed");
            return;
        }
        Engine.Utility.Log.Error("OnSdKLogin");

        http.PlatInfoFactory = () => new Pmd.PlatInfo()
        {
            uid = m_uid,
            platid =(int) m_uPid,
            sign = m_loginToken,
            account = (isSdkLogin) ? "" : m_acount,
        };
        Action act = new Action(() =>
        {
            //加载完毕，显示登录界面
            if(LaunchPanel.Me != null)
            {
                LaunchPanel.Me.Visible = false;
            }
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginPanel, data: LoginPanel.ShowUIEnum.StartGame);
          
        });
        DataManager.Manager<LoginDataManager>().StateMachine.ChangeState((int)LoginSteps.LGS_Platform, act);
    }


    public void Reset(bool depthClearData = false)
    {
        m_lstRoleList.Clear();
        ResetServerStatusParam();
    }

    public void Process(float deltaTime)
    {

    }

    private string GetCacheAccount()
    {
        string straccount = "";
        if (Application.isEditor && System.IO.File.Exists("account.cache"))
        {
            straccount = System.IO.File.ReadAllText("account.cache");
        }
        else
        {
            straccount = PlayerPrefs.GetString(USERLOGIN_ACCOUNT, "");
        }

        if (string.IsNullOrEmpty(straccount) && !IsSDKLogin)
        {
            straccount = (Mathf.Abs(SystemInfo.deviceUniqueIdentifier.GetHashCode()) % 1000).ToString().PadLeft(3, '0');
        }

        return straccount;
    }

    private uint GetCacheZoneId()
    {
        return (uint)PlayerPrefs.GetInt(USERLOGIN_ZONEID, 0);
    }

    private string GetCacheZoneName()
    {
        return PlayerPrefs.GetString(USERLOGIN_ZONENAME, "");
    }

    private void SaveLoginData()
    {
        if (Application.isEditor)
        {
            System.IO.File.WriteAllText("account.cache", m_acount);
        }
        else
        {
            PlayerPrefs.SetString(USERLOGIN_ACCOUNT, m_acount);
        }
        Pmd.ZoneInfo curZoneInfo = GetZoneInfo();
        if (null != curZoneInfo)
        {
            PlayerPrefs.SetInt(USERLOGIN_ZONEID, (int)CurSelectZoneID);
            PlayerPrefs.SetString(USERLOGIN_ZONENAME, curZoneInfo.zonename);
        }
        //Debug.LogError("LoginDataManager -> SaveLoginData m_acount:" + m_acount + " m_nLastZoneId :" + m_nLastZoneId);

        PlayerPrefs.Save();
    }

    public string m_strPreAcount;
    

    /// <summary>
    /// 3 连接进入游戏
    /// </summary>
    public void StartGame()
    {
        if (isSdkLogin && !IsLoginStateReady(LoginState.LGState_PlatformReady))
        {
            string account = DataManager.Manager<LoginDataManager>().Acount;
            Debug.LogError("account is "+ account);

            DoLoginStep(LoginSteps.LGS_Authorize);
        }
        else
        {
            OnSelectZone();
        }
    }

    /// <summary>
    /// 选区 获取ip和端口
    /// </summary>
    public void OnSelectZone()
    {
        if (!CurAreaServerEnable)
        {
            TipsManager.Instance.ShowTips("当前无可用区服");
            return;
        }
        http.ZoneID = CurSelectZoneID;
        stateMachine.ChangeState((int)LoginSteps.LGS_SelectZone, null);
    }

    /// <summary>
    /// 请求登录角色信息
    /// </summary>
    void RequestLoginUserInfo(Action<Pmd.RequestUserZoneInfoLoginUserPmd_S> userInfo)
    {
        http.RequestLoginUserInfo(userInfo);
    }


    Pmd.UserLoginTokenLoginUserPmd_C loginMsg;
    public void onConnectedGameServer(bool ret)
    {
    
        if (ret)
        {
            NetService.Instance.FirstMsg = true;
            Protocol.Instance.StopReconnectSever = false;
            Engine.Utility.Log.Info("切换tcpsoket成功 连接游戏服{0}成功", UserData.GameID);
            SaveLoginData();
            loginMsg = new Pmd.UserLoginTokenLoginUserPmd_C();
            loginMsg.gameid = (uint)http.GameID;
            loginMsg.zoneid = (uint)http.ZoneID;
            ulong accountID = 0;
            if (ulong.TryParse(http.UID, out accountID))
            {
                loginMsg.accountid = accountID;
            }

            loginMsg.logintempid = 0;
            long timeSeconds = DateTime.Parse("1970-01-01 00:00:00").Ticks;
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            loginMsg.timestamp = (uint)(ts.TotalMilliseconds / 1000);
            loginMsg.compress = "zlib";
            loginMsg.version = (uint)Pmd.Config.Version.Version_Gateway;
            //MD5赋值测试
            string md5_str = accountID.ToString() + loginMsg.logintempid.ToString() + loginMsg.timestamp.ToString() + http.PlatKey;//+platKey
            if (loginMsg.logintempid == 0)
            {
                md5_str = accountID.ToString() + loginMsg.timestamp.ToString() + http.PlatKey;//+platKey
            }
            loginMsg.tokenmd5 = Common.MD5.ComputeHashString(md5_str);

            Engine.Utility.Log.Info("发送登录{0}消息", UserData.GameID);
            NetService.Instance.Send(loginMsg);
            //  if(!Application.isEditor/*&&!Protocol.Instance.IsDebug*/)
            {
                Protocol.Instance.StartHeartBeat();
            }

            Protocol.Instance.SetTimetickOut();
        }
        else
        {
            Engine.Utility.Log.Error("连接游戏服失败");
        }
    }

    public void ReconnectGameSever()
    {
        try
        {
          //  if(NetService.Instance.IsDisconnect())
            {
                Engine.Utility.Log.Error("重连游戏服。。。。");
                NetService.Instance.Connect(UserData.GatewayServerIP, UserData.GatewayServerPort, onReConnectedGameServer);
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }
    private void onReConnectedGameServer(bool ret)
    {
        Protocol.Instance.StopReconnectServerTimer();
        if (ret)
        {
            Engine.Utility.Log.Info("重新连接游戏服成功 {0}", UserData.GameID);
        
            NetService.Instance.FirstMsg = true;
            if (Protocol.Instance.IsReconnecting)
            {//请求历史数据 （可以选择不要 lastseq = 0)
                uint lastSeq = NetService.Instance.MsgSeq;
             
                if (MainPlayerHelper.GetMainPlayer() == null)
                {
                    lastSeq = 0;
                }
                else
                {
                    if (MainPlayerHelper.GetMainPlayer().IsDead())
                    {
                        lastSeq = 0;
                    }
                }
      
                loginMsg.lastseq = lastSeq; //等于0走重新登录流程
                //loginMsg.lastseq = 0;   //测试代码，打开为大重连，正式版本要注释
                loginMsg.charid = LastLoginCharID;
                Engine.Utility.Log.Error("lastseq is {0} lastcharid is {1}", loginMsg.lastseq, LastLoginCharID);
                NetService.Instance.Send(loginMsg);
             //   Protocol.Instance.IsReconnecting = false;
            }
            //  



        }
        else
        {
           Protocol.Instance.StartReconnectServerTimer();
            Engine.Utility.Log.Error("重新连接游戏服失败");
        }
    }
    
    public void ChangeAcount()
    {
        //isLoginSdkDone = false;
        if (isSdkLogin)
        {
            CommonSDKPlaform.Instance.Logout();
        }else
        {
            Logout(LoginPanel.ShowUIEnum.LoginAccout);
        }
    }

    public void OnChangeAcountResult(bool success)
    {
        Engine.Utility.Log.Error("OnChangeAcountResult result:{0}", success);
        if (success)
        {
            m_lstRoleList.Clear();
            SetLoginState(LoginState.None);
            RefreshLoginData();
            Logout(LoginPanel.ShowUIEnum.Authorize, () =>
            {
                if (IsSDKLogin)
                    DoLoginStep(LoginSteps.LGS_Platform);
            });
        }
    }

    /// <summary>
    /// 断开连接
    /// </summary>
    public void LogoutData()
    {
        Protocol.Instance.StopReconnectSever = true;
        Pmd.UserLogoutTokenLoginUserPmd_C cmd = new Pmd.UserLogoutTokenLoginUserPmd_C();
        NetService.Instance.Send(cmd);
        Protocol.Instance.CloseHeartMsg();
        ResetServerStatusParam();
    }

    public void Logout(LoginPanel.ShowUIEnum uienum = LoginPanel.ShowUIEnum.Authorize,Action act = null)
    {
        LogoutData();
        GameMain.Instance.GoToLogin(uienum,act);
    }

    void http_OnRequest(object sender, GX.Net.HttpRequestEventArgs e)
    {
        ShowWaitPanel("登录中...");
    }
    void ShowWaitPanel(string des)
    {
        //if(!TimerAxis.Instance().IsExist(m_showWaitPanelTimerID,this))
        //{
        //    TimerAxis.Instance().SetTimer(m_showWaitPanelTimerID, 2000, this, 1);
        //}

    }
    void ShowReconnectTips()
    {
     
    }
    void http_OnResponse(object sender, GX.Net.HttpResponseEventArgs e)
    {
        if (e != null)
        {
            if (http != null)
            {
        
                var recv = GX.Net.JsonHttp.HttpPackageReturn<Pmd.PlatTokenLoginReturn>.Create(e.WWW, http);
                if (recv != null)
                {
                    if (recv.errno != Pmd.HttpReturnCode.HttpReturnCode_Null)
                    {
                        string errorName = Enum.GetName(typeof(Pmd.HttpReturnCode), recv.errno);
                        Engine.Utility.Log.Error("error HttpReturnCode is {0}", errorName);

                    }


                    string str = "";
                    if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_Null)
                    {
                        string errorName = Enum.GetName(typeof(Pmd.HttpReturnCode), recv.errno);
                        //DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
                        KillLoginTimer();
                        return;
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_DbError)
                    {
                        str = GetErrorStr(LocalTextType.Net_2);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_LuaScriptError)
                    {
                        str = GetErrorStr(LocalTextType.Net_4);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_GameZoneListError)
                    {
                        str = GetErrorStr(LocalTextType.Net_5);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_SignError)
                    {
                        str = GetErrorStr(LocalTextType.Net_11);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_ServerShutDown)
                    {
                        str = GetErrorStr(LocalTextType.Net_12);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_JsonSyntaxError)
                    {
                        str = GetErrorStr(LocalTextType.Net_13);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_JsonMessageError)
                    {
                        str = GetErrorStr(LocalTextType.Net_14);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_TokenValueError)
                    {
                        str = GetErrorStr(LocalTextType.Net_15);

                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_WaiGuaUidError)
                    {
                        str = GetErrorStr(LocalTextType.Net_16);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_NoGatewaytDown)
                    {
                        str = GetErrorStr(LocalTextType.Net_17);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_NoGatewayDown)
                    {
                        str = GetErrorStr(LocalTextType.Net_17);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_NoSdkServer)
                    {
                        str = GetErrorStr(LocalTextType.Net_18);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_SdkCheckSignErr)
                    {
                        str = GetErrorStr(LocalTextType.Net_19);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_Sdk3PartyServerErr)
                    {
                        str = GetErrorStr(LocalTextType.Net_20);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_ProtobufErr)
                    {
                        str = GetErrorStr(LocalTextType.Net_21);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_GatewayErr)
                    {
                        str = GetErrorStr(LocalTextType.Net_22);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_Timeout)
                    {
                        str = GetErrorStr(LocalTextType.Net_23);
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_AccountUsing)
                    {
                        str = GetErrorStr(LocalTextType.Net_24);
                        if (!NetService.Instance.IsDisconnect())
                        {
                            LogoutData();
                        }
                    }
                    else if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_OnlinePlatidErr)
                    {
                        str = GetErrorStr(LocalTextType.Net_25);
                    }
                    if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_AccountUsing)
                    {
                        KillLoginTimer();
                        string des = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Net_AccountHasLogin);

                        stateMachine.ChangeState((int)LoginSteps.LGS_SelectZone, des);
                        TimerAxis.Instance().SetTimer(m_loginTimerID, 3000, this, 3);
                        
                    }
                    else
                    {
                        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
                        TipsManager.Instance.ShowTips(str);
                    }
                }
                else
                {
                    DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
                    TipsManager.Instance.ShowTips("没有可用网络");
                }
            }

        }
    }
    string GetErrorStr(LocalTextType type)
    {
       return DataManager.Manager<TextManager>().GetLocalText(type);
    }
    public void OnTimer(uint uTimerID)
    {
        if (m_loginTimerID == uTimerID)
        {
            Log.Info("login timer -======================");
            string des = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Net_AccountHasLogin);

            stateMachine.ChangeState((int)LoginSteps.LGS_SelectZone, des);
           
        }
        else if(uTimerID == 555)
        {
            NetService.Instance.Send(loginMsg);
        }
        else if (uTimerID == m_ReTryRequestHttpTimerID)
        {
            Action act = new Action(
                () => { });
            StateMachine.ChangeState((int)LoginSteps.LGS_Platform, act);
            Engine.Utility.Log.Error("time-------StartCheckServerStateTimer"); ;
        }
        //else if(m_showWaitPanelTimerID == uTimerID)
        //{
        //    WaitPanelShowData data = new WaitPanelShowData();
        //    data.type = WaitPanelType.Login;
        //    data.cdTime = (int)20;
        //    data.des = CommonData.GetLocalString("登录中...");
        //    data.timeOutDel = ShowReconnectTips;
        //    data.useBoxMask = true;
        //    data.showTimer = false;
        //    DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.CommonWaitingPanel, data: data);
        //}
    }

    
   public  void StartCheckServerStateTimer()
    {
        if (!CurAreaServerEnable)
        {
            return;
        }
        KillCheckServerStateTimer();

        if (GetZoneInfo().state == Pmd.ZoneState.Shutdown)
        {
            TimerAxis.Instance().SetTimer(m_ReTryRequestHttpTimerID, 5000, this);
        }

    }
    public void KillCheckServerStateTimer() 
    {
        TimerAxis.Instance().KillTimer(m_ReTryRequestHttpTimerID, this);
        Engine.Utility.Log.Error("stop-------KillCheckServerStateTimer"); ;
    }
    void KillLoginTimer()
    {
        TimerAxis.Instance().KillTimer(m_loginTimerID, this);
    }
    void KillWaitTimer()
    {
        TimerAxis.Instance().KillTimer(m_showWaitPanelTimerID, this);
    }

    public void OnRecieveServerError(uint err_id, string err_text) 
    {
        TipsManager.Instance.ShowTips(DataManager.Manager<TextManager>().GetLocalText((int)err_id));
    }

    public void OnRecieveServerStateChange(ServerLimit state, uint err_id,uint time)
    {       
         string tips  = "";
         if (time == 0)
         {
             tips = DataManager.Manager<TextManager>().GetLocalText((int)err_id);
         }
         else 
         {
             DateTime dt = DateTimeHelper.TransformServerTime2DateTime((long)time);
             try 
             {
                 string time_txt = dt.Year + "年" +
                                  dt.Month + "月" +
                                  dt.Day + "日" +
                                  dt.Hour + "时" +
                                  dt.Minute + "分";
                 tips = string.Format(DataManager.Manager<TextManager>().GetLocalText((int)err_id), time_txt);
             }
             catch(Exception e)
             {
                 Engine.Utility.Log.Error("此id{0}对应的文本内容不支持插入数据！！！", err_id);
             }
            
         }     
        curServerState = new ServerStatusParam()
        {
                state = state,
                msg = tips,
        };
       
    }

    public void ResetServerStatusParam()
    {
        curServerState = new ServerStatusParam();
    }

    #region Login
    /// <summary>
    /// 应用宝
    /// </summary>
    public bool IsYingYongBao
    {
        get
        {
            return ChannelID == CommonSDKPlaform.QQ_CHANNEL_ID
                || ChannelID == CommonSDKPlaform.WECHAT_CHANNEL_ID;
        }
    }

    //SDK是否初始化完成
    private bool m_bSDKReady = false;
    public bool SDKReady
    {
        get
        {
            return m_bSDKReady;
        }
    }
    /// <summary>
    /// sdk初始化完成
    /// </summary>
    /// <param name="success"></param>
    public void OnSDKInitComplete(bool success)
    {
        if (success)
        {
            m_bSDKReady = true;
        }
    }

    /// <summary>
    /// SDK授权
    /// </summary>
    public void StartAuthorize()
    {
        Debug.Log("SDK 授权");
        //DataManager.Manager<UIPanelManager>().SendMsg(PanelID.LoginPanel, UIMsgID.eLoginAuthorizeState, false);
        CoroutineMgr.Instance.DelayInvokeMethod(0, () =>
        {
            CommonSDKPlaform.Instance.Login();
        }, true);
    }

    /// <summary>
    /// QQ登陆
    /// </summary>
    public void DoLoginQQ()
    {
        if (IsSDKLogin && IsYingYongBao)
        {
            CommonSDKPlaform.Instance.LoginQQ();
        }
        else
        {
            DoLogin();
        }
    }

    /// <summary>
    /// 微信登陆
    /// </summary>
    public void DoLoginWechat()
    {
        if (IsSDKLogin && IsYingYongBao)
        {
            CommonSDKPlaform.Instance.LoginWechat();
        }else
        {
            DoLogin();
        }
    }

    /// <summary>
    /// 通用登陆
    /// </summary>
    public void DoLogin()
    {
        if (IsSDKLogin)
        {
            CommonSDKPlaform.Instance.Login();
        }else
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginPanel, data: LoginPanel.ShowUIEnum.LoginAccout);
        }
    }

    public void GotoLogin(LoginPanel.ShowUIEnum uienum, Action action = null)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginPanel, data: uienum, panelShowAction: (panel) =>
                    {
                        if (action != null)
                        {
                            action.Invoke();
                        }
                    });
    }

    /// <summary>
    /// 开始登陆流程
    /// </summary>
    public void Login()
    {
        if (IsSDKLogin)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginPanel, data: LoginPanel.ShowUIEnum.Authorize
            , panelShowAction: (pb) =>
            {
                DoLoginStep(LoginSteps.LGS_Authorize);
            });
        }
        else
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.LoginPanel, data: LoginPanel.ShowUIEnum.LoginAccout);
        }
    }

    /// <summary>
    /// 拉取区服过滤数据
    /// </summary>
    /// <param name="callback"></param>
    public void FetchAreaSeverFilterData(Action<bool,string> callback = null)
    {
        Action<WWW> getAreaServerDataCallback = (www) =>
        {
            if (www.isDone)
            {
                bool success = false;
                if (string.IsNullOrEmpty(www.error))
                {
                    success = true;
                    ParseAreaServerJson(System.Text.UTF8Encoding.UTF8.GetString(www.bytes));
                }
                if (null != callback)
                {
                    callback.Invoke(success, www.error);
                }
                
            }
        };
        CoroutineMgr.Instance.StartCorountine(GetServerAreaDataFromServer(getAreaServerDataCallback));
    }

    /// <summary>
    /// sdk登出
    /// </summary>
    public void DoSDKLoginout()
    {
        if (IsSDKLogin)
        {
            CommonSDKPlaform.Instance.Logout();
        }
    }

    /// <summary>
    /// 切换登陆状态
    /// </summary>
    /// <param name="state"></param>
    /// <param name="param"></param>
    public void DoLoginStep(LoginSteps state,object param = null)
    {
        if (null == stateMachine)
        {
            Engine.Utility.Log.Error("LoginDataManager->DoLoginStep error,stateMachine null");
            return;
        }
        stateMachine.ChangeState((int)state,param);
    }

    
    #endregion

    #region LoginData
    //角色ID
    public long RoleID
    {
        get
        {
            if (null != UserData.CurrentRole)
            {
                return UserData.CurrentRole.id;
            }
            return 0;
        }
    }

    //角色等级
    public int RoleLevel
    {
        get
        {
            return MainPlayerHelper.GetPlayerLevel();
        }
    }

    //角色名称
    public string RoleName
    {
        get
        {
            if (null != UserData.CurrentRole)
            {
                return UserData.CurrentRole.name;
            }
            return "";
        }
    }

    //角色创建时间
    public string RoleCreateUnixTime
    { 
        get
        {
            if (null != UserData.CurrentRole)
            {
                uint unixTime = 0;
                if (UserData.CurrentRole.regtime > 28800)
                {
                    unixTime = UserData.CurrentRole.regtime - 28800;
                }
                return unixTime.ToString();
            }
            return "";
        }
    }

    //服务器编号
    public string ServerNo
    {
        get
        {
            Pmd.ZoneInfo zone = GetZoneInfo();
            if (null == zone)
            {
                return "";
            }
            return GetBestZoneID(zone).ToString();
        }
    }

    //服务器名称
    public string ServerName
    {
        get
        {
            Pmd.ZoneInfo zone = GetZoneInfo();
            if (null == zone)
            {
                return "";
            }
            return zone.zonename;
        }
    }
    #endregion

    #region SDKStat(SDK统计)

    /// <summary>
    /// 进入游戏统计
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="playerName"></param>
    public void EnterGame()
    {
        if (IsSDKLogin)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                int lv = 1;
                if (null != UserData.CurrentRole)
                {
                    lv = (int)UserData.CurrentRole.level;
                }
                CommonSDKPlaform.Instance.EnterGame(ServerNo, ServerName, RoleID, RoleCreateUnixTime, lv, RoleName);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {

            }
        }
    }

    /// <summary>
    /// 创建角色统计
    /// </summary>
    /// <param name="roleName"></param>
    public void CreateRoleToSDK()
    {
        if (IsSDKLogin)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                CommonSDKPlaform.Instance.CreateRoleToSDK(ServerNo, ServerName, RoleID, RoleName, RoleCreateUnixTime);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ZqgameSDKController.Instance.CreateRole(ServerNo, RoleID.ToString());
            }
        }
    }

    /// <summary>
    /// 用户升级
    /// </summary>
    /// <param name="level"></param>
    public void UserUpLevel(int level)
    {
        if (IsSDKLogin)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                CommonSDKPlaform.Instance.UserUpLever(ServerNo, ServerName, RoleID, RoleCreateUnixTime, RoleName, level);
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                
            }
        }
    }
    #endregion

    #region SDKCallback

    /// <summary>
    /// 平台授权成功
    /// </summary>
    public void DoLoginPlatform()
    {
        m_strPreAcount = m_acount;

        SetLoginState(LoginState.LGState_SDKReady);
        
        //登陆平台服务器
        LoginPlatform();
    }

    /// <summary>
    ///刷新登陆信息
    /// </summary>
    private void RefreshLoginData()
    {
        CommonSDKPlaform.loginResult lr = CommonSDKPlaform.Instance.LoginResult;
        m_loginToken = lr.token;
        m_uid = lr.uid;
        m_uPid = lr.pid;
        m_acount = lr.account;
        m_accountID = lr.accountID;             //Zqgame IOS SDK enable
        m_loginAcount = lr.szLoginAcccount;
        m_loginSession = lr.szLoginSession;
        m_loginDataEx = lr.szLoginDataEx;
        m_loginPlatUserID = lr.uiLoginPlatUserID;
    }

    /// <summary>
    /// 登陆回调
    /// </summary>
    /// <param name="success"></param>
    public void OnLogin(bool success)
    {
        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.CommonWaitingPanel);
        if (success)
        {
            RefreshLoginData();
            DoLoginStep(LoginSteps.LGS_FetchASFilterData);
        }
        else
        {
            DoLoginOut();
        }
    }

    private void DoLoginOut()
    {
        //重置当前选中区服
        //SetCurZoneId(0);
        SetLoginState(LoginState.None);
        Logout(LoginPanel.ShowUIEnum.Authorize, () =>
            {
                if (IsSDKLogin)
                    DoLoginStep(LoginSteps.LGS_Authorize);
            });
    }

    /// <summary>
    /// 退出登陆
    /// </summary>
    /// <param name="success"></param>
    public void OnLogout(bool success)
    {
        if (success)
        {
            Debug.Log("SDK logout");
            DoLoginOut();
        }
    }
    #endregion

    #region AreaServer
    /// <summary>
    /// 可用区服信息想
    /// </summary>
    public class EnableZoneInfo
    {
        //区服ZoneID
        public uint ZoneID = 0;
        //在区服列表里面的索引
        public int Index = 0;
    }


    //母包ID
    public uint MainPackageId
    {
        get
        {
            return GlobalConfig.Instance().MainPackageID;
        }
    }

    //渠道ID
    public int ChannelID
    {
        get
        {
            return GlobalConfig.Instance().ChannelID;
        }
    }

    //是否需要过滤区服
    public bool NeedFilterAreaServer
    {
        get
        {
            //渠道号为0表示不过滤
            return ChannelID != 0 && MainPackageId != 0;
        }
    }

    private List<uint> m_lstMatchAreaServerData = null;

    private bool isInitAreaServerData = false;
    public bool IsInitAreaServerData
    {
        get
        {
            return isInitAreaServerData;
        }
    }

    /// <summary>
    /// 区服列表是否可用
    /// </summary>
    public bool CurAreaServerEnable
    {
        get 
        {
            return CurSelectZoneID != 0 && GetZoneInfo() != null;
        }
    }
    /// <summary>
    /// 解析服务器区服列表数据
    /// </summary>
    /// <param name="areaServerJson"></param>
    private void ParseAreaServerJson(string areaServerJson)
    {
        Engine.JsonNode jsonRoot = Engine.RareJson.ParseJson(areaServerJson);
        if (jsonRoot == null)
        {
            Engine.Utility.Log.Error("LoginDataManger ParseAreaServerJson 解析{0}文件失败!", areaServerJson);
            return;
        }

        if (null != m_lstMatchAreaServerData)
            m_lstMatchAreaServerData.Clear();
        List<uint> tempResultData = null;

        Dictionary<uint, AreaServerData> tempAreaServerData = new Dictionary<uint, AreaServerData>();
        try
        {
            //1、解析区服类型数据
            JsonObject jsonObj = null;
            JsonArray areaServerDataBase = (JsonArray)jsonRoot["AreaServerDataBase"];
            for (int i = 0; i < areaServerDataBase.Count; i++)
            {
                jsonObj = (JsonObject)areaServerDataBase[i];
                if (jsonObj != null)
                {
                    uint areaServerId = (uint)jsonObj["areaServerId"];
                    string areaServerName = jsonObj["areaServerName"];
                    string serverSection = jsonObj["serverSection"];
                    if (!tempAreaServerData.ContainsKey(areaServerId))
                    {
                        tempAreaServerData.Add(areaServerId, new AreaServerData(areaServerId, areaServerName, serverSection));
                    }
                }
            }

            JsonArray areaServerDisplayDataBase = (JsonArray)jsonRoot["AreaServerDisplayDataBase"];
            for (int i = 0; i < areaServerDisplayDataBase.Count; i++)
            {
                jsonObj = (JsonObject)areaServerDisplayDataBase[i];
                if (jsonObj != null)
                {
                    uint id = (uint)jsonObj["id"];
                    uint mainPackageId = (uint)jsonObj["mainPackageId"];
                    int channelId = (int)jsonObj["platformId"];
                    //母包ID过滤
                    if (mainPackageId != MainPackageId)
                    {
                        continue;
                    }

                    //渠道ID过滤
                    if (channelId != ChannelID)
                    {
                        continue;
                    }
                    string displayServer = jsonObj["displayServer"];
                    if (AreaServerDisplayData.TryParseAreaServerSecions(displayServer, ref tempResultData))
                    {
                        for (int j = 0, maxj = tempResultData.Count; j < maxj; j++)
                        {
                            if (null == m_lstMatchAreaServerData)
                                m_lstMatchAreaServerData = new List<uint>();
                            if (m_lstMatchAreaServerData.Contains(tempResultData[j]))
                                continue;
                            m_lstMatchAreaServerData.Add(tempResultData[j]);
                        }
                    }
                    break;

                }
            }
        }
        catch(Exception e)
        {
            Engine.Utility.Log.Error("LoginDataManger ParseAreaServerJson Error：{0}!", e.ToString());
        }

        isInitAreaServerData = true;
        if (null != m_lstMatchAreaServerData)
        {
            uint serverAreaId = 0;
            AreaServerData tempData = null;
            for (int i = 0, max = m_lstMatchAreaServerData.Count; i < max; i++)
            {
                if (null == m_localAreaServerDatas)
                {
                    m_localAreaServerDatas = new Dictionary<uint, AreaServerData>();
                }
                serverAreaId = m_lstMatchAreaServerData[i];
                if (m_localAreaServerDatas.ContainsKey(serverAreaId))
                    continue;
                if (!tempAreaServerData.TryGetValue(serverAreaId, out tempData))
                {
                    continue;
                }
                m_localAreaServerDatas.Add(serverAreaId, tempData);
                if (!isInitAreaServerData)
                {
                    isInitAreaServerData = true;
                }
            }
        }
        
    }

    private Dictionary<uint, AreaServerData> m_localAreaServerDatas = null;
    /// <summary>
    ///从服务器拉取区服数据
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    private System.Collections.IEnumerator GetServerAreaDataFromServer(Action<WWW> callback)
    {
        WWW www = new WWW(GlobalConfig.Instance().AreaServerUrl);
        yield return www;
        if (null != callback)
        {
            callback.Invoke(www);
        }
    }

    /// <summary>
    /// 构建玩家区服数据
    /// </summary>
    /// <param name="userZoneInfos"></param>
    private void StructUserAreaServerInfo(List<Pmd.ZoneInfo> userZoneInfos)
    {
        if (null != m_lstAreaServerPages)
            m_lstAreaServerPages.Clear();
        else
            m_lstAreaServerPages = new List<AreaServerPageData>();

        if (NeedFilterAreaServer && (m_lstMatchAreaServerData == null || m_lstMatchAreaServerData.Count == 0))
        {
            //清除当前选中区服数据索引
            ClearCurSelectZoneIndex();
            return;
        }
            
        uint cacheLastZoneID = GetCacheZoneId();
        string cacheLastZoneName = GetCacheZoneName();

        EnableZoneInfo cacheZone = null;

        Dictionary<uint, List<EnableZoneInfo>> enableZone = new Dictionary<uint, List<EnableZoneInfo>>();
        Pmd.ZoneInfo zoneInfo;
        AreaServerData tempASD = null;
        bool isFullMathchLastZone = false;
        EnableZoneInfo tempEnableZoneInfo = null;
        uint bestZoneID = 0;
        uint orginalZoneID = 0;
        for (int i = 0, max = ZoneList.Count; i < max; i++)
        {
            zoneInfo = ZoneList[i];
            if (null == zoneInfo)
                continue;
            bestZoneID = GetBestZoneID(zoneInfo);
            orginalZoneID = GetOriginalZoneID(zoneInfo);
            if (NeedFilterAreaServer)
            {
                for (int j = 0, maxj = m_lstMatchAreaServerData.Count; j < maxj; j++)
                {
                    if (!m_localAreaServerDatas.TryGetValue(m_lstMatchAreaServerData[j], out tempASD))
                    {
                        continue;
                    }

                    if (tempASD.IsContainID(orginalZoneID))
                    {
                        if (cacheLastZoneID == bestZoneID)
                        {
                            if (null == cacheZone)
                            {
                                cacheZone = new EnableZoneInfo()
                                {
                                    Index = i,
                                    ZoneID = zoneInfo.zoneid,
                                };
                            }
                            if (!string.IsNullOrEmpty(cacheLastZoneName) && !isFullMathchLastZone && cacheLastZoneName.Equals(zoneInfo.zonename))
                            {
                                isFullMathchLastZone = true;
                                cacheZone.Index = i;
                                cacheZone.ZoneID = zoneInfo.zoneid;
                            }
                        }

                        if (!enableZone.ContainsKey(tempASD.ID))
                        {
                            enableZone.Add(tempASD.ID, new List<EnableZoneInfo>());
                        }
                        tempEnableZoneInfo = new EnableZoneInfo()
                        {
                            ZoneID = zoneInfo.zoneid,
                            Index = i,
                        };
                        enableZone[tempASD.ID].Add(tempEnableZoneInfo);
                        break;
                    }
                }
            }
            else
            {
                if (!enableZone.ContainsKey(0))
                {
                    enableZone.Add(0, new List<EnableZoneInfo>());
                }
                tempEnableZoneInfo = new EnableZoneInfo()
                {
                    ZoneID = zoneInfo.zoneid,
                    Index = i,
                };
                enableZone[0].Add(tempEnableZoneInfo);
                if (cacheLastZoneID == bestZoneID)
                {
                    if (null == cacheZone)
                    {
                        cacheZone = new EnableZoneInfo()
                        {
                            Index = i,
                            ZoneID = zoneInfo.zoneid,
                        };
                    }
                    if (!string.IsNullOrEmpty(cacheLastZoneName) && !isFullMathchLastZone && cacheLastZoneName.Equals(zoneInfo.zonename))
                    {
                        isFullMathchLastZone = true;
                        cacheZone.Index = i;
                        cacheZone.ZoneID = zoneInfo.zoneid;
                    }
                }
            }
        }

        TextManager tmgr = DataManager.Manager<TextManager>();
        //推荐列表(每个区选一个最新的 注：ZoneID最大)
        List<EnableZoneInfo> recommondZones = new List<EnableZoneInfo>();
        if (enableZone.Count > 0)
        {
            var iemurator = enableZone.GetEnumerator();
            while(iemurator.MoveNext())
            {
                if (iemurator.Current.Value.Count > 0)
                {
                    //降序排序
                    iemurator.Current.Value.Sort((left, right) =>
                        {
                            return ((int)right.ZoneID - (int)left.ZoneID);
                        });
                    if (!recommondZones.Contains(iemurator.Current.Value[0]))
                    {
                        recommondZones.Add(iemurator.Current.Value[0]);
                    }
                }
            }

            List<uint> enaleArea = new List<uint>();
            enaleArea.AddRange(enableZone.Keys);
            if (NeedFilterAreaServer)
            {
                enaleArea.Sort((left, right) =>
                {
                    int leftIndex = m_lstMatchAreaServerData.Contains(left) ? m_lstMatchAreaServerData.IndexOf(left) : 0;
                    int rightIndex = m_lstMatchAreaServerData.Contains(right) ? m_lstMatchAreaServerData.IndexOf(left) : 0;
                    return leftIndex - rightIndex;
                });
            }
           

            List<EnableZoneInfo> tempZones = null;
            AreaServerPageData tempPageData = null;
            string tempName = "";
            int tempCount = 0;
            int tempPageCount = 0;
            int tempIndex = 0;
            //区排序完成构建数据
            for(int i=0,max = enaleArea.Count;i < max;i++)
            {
                if (NeedFilterAreaServer)
                {
                    if (!m_localAreaServerDatas.TryGetValue(enaleArea[i], out tempASD))
                        continue;
                    if (!enableZone.TryGetValue(enaleArea[i], out tempZones))
                        continue;
                }
                else
                {
                    if (!enableZone.TryGetValue(0, out tempZones))
                        continue;
                }
                tempPageCount = ((tempZones.Count % PAGE_MAX_NUM) != 0) ? (tempZones.Count / PAGE_MAX_NUM + 1) : (tempZones.Count / PAGE_MAX_NUM);
                tempIndex = 0;
                for (int j = tempPageCount; j >= 1; j--)
                {
                    tempCount = (j * PAGE_MAX_NUM > tempZones.Count) ? tempZones.Count - (j - 1) * PAGE_MAX_NUM : PAGE_MAX_NUM;
                    tempName = tmgr.GetLocalFormatText(LocalTextType.Login_Server_AreaServerDisplayName, (NeedFilterAreaServer ? tempASD.Name : ""), j);
                    tempPageData = new AreaServerPageData(tempName, tempZones.GetRange(tempIndex, tempCount));
                    tempIndex = tempIndex + tempCount;
                    m_lstAreaServerPages.Add(tempPageData);
                }
                if (!NeedFilterAreaServer)
                {
                    break;
                }
            }
        }
        
        //推荐数据排序（存在的话）
        if (recommondZones.Count> 0)
        {
            recommondZones.Sort((left, right) =>
            {
                return ((int)right.ZoneID - (int)left.ZoneID);
            });
            if (null != cacheZone)
            {
                SetCurSelectZoneIndex(cacheZone.Index);
            }else
            {
                SetCurSelectZoneIndex(recommondZones[0].Index);
            }
            AreaServerPageData recommondPage = new AreaServerPageData(tmgr.GetLocalText(LocalTextType.Login_Server_RecommonAreaServer), recommondZones, true);
            m_lstAreaServerPages.Insert(0, recommondPage);
        }else
        {
            ClearCurSelectZoneIndex();
        }

    }

    /// <summary>
    /// 获取玩家区服数据
    /// </summary>
    /// <returns></returns>
    public int GetAreaServerPagesCount()
    {
        if (null != m_lstAreaServerPages)
        {
            return m_lstAreaServerPages.Count;
        }
        return 0;
    }

    //最大推荐数量
    private const int RECOMMOND_SERVER_MAX_NUM = 3;
    //每页最大数量
    private const int PAGE_MAX_NUM = 10;
    //服务器页数据
    private List<AreaServerPageData> m_lstAreaServerPages = null;
    /// <summary>
    /// 区服页数据
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public AreaServerPageData GetPageData(int index)
    {
        if (null != m_lstAreaServerPages && m_lstAreaServerPages.Count > index)
        {
            return m_lstAreaServerPages[index];
        }
        return null;
    }

    /// <summary>
    /// 获取当前区服信息
    /// </summary>
    /// <returns></returns>
    public Pmd.ZoneInfo GetZoneInfo()
    {
        return GetZoneInfoByIndex(CurrentSelectZoneIndex);
    }

    /// <summary>
    /// 根据zoneid获取区信息
    /// </summary>
    /// <param name="zoneId"></param>
    /// <returns></returns>
    public Pmd.ZoneInfo GetZoneInfo(uint zoneId)
    {
        Pmd.ZoneInfo result = null;
        if (null != ZoneList)
        {
            Pmd.ZoneInfo zoneInfo = null;
            for(int i = 0,max = ZoneList.Count;i < max;i ++)
            {
                zoneInfo = ZoneList[i];
                if (null == zoneInfo)
                    continue;
                if (zoneInfo.zoneid == zoneId)
                {
                    result = zoneInfo;
                    break;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 根据区Index获取区服信息
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Pmd.ZoneInfo GetZoneInfoByIndex(int index)
    {
        Pmd.ZoneInfo result = null;
        if (null != ZoneList && ZoneList.Count > index)
        {
            result = ZoneList[index];
        }
        return result;
    }
    #endregion

    string[] preNameList = null;
    string[] maleList = null;
    string[] femalList = null;
    /// <summary>
    /// 获取随机名字
    /// </summary>
    /// <param name="sex"></param>
    /// <returns></returns>
    public string GetRandomName(enmCharSex sex)
    {
        string preStr = string.Empty;
        string maleStr = string.Empty;
        string femaleStr = string.Empty;
        table.RandomNameDataBase db = null;
        if (preNameList == null)
        {
            db = GameTableManager.Instance.GetTableList<table.RandomNameDataBase>()[0];
            if (db != null)
            {
                string str = db.namePrefix;
                preNameList = str.Split('_');
            }
        }

        if (null == maleList && sex == enmCharSex.MALE)
        {
            if (null == db)
            {
                db = GameTableManager.Instance.GetTableList<table.RandomNameDataBase>()[0];
            }
            if (db != null)
            {
                maleList = db.maleName.Split('_');
            }
        }

        if (null == femalList && sex == enmCharSex.FEMALE)
        {
            if (null == db)
            {
                db = GameTableManager.Instance.GetTableList<table.RandomNameDataBase>()[0];
            }
            if (db != null)
            {
                femalList = db.femaleName.Split('_');
            }
        }
        int index = 0;
        if (null != preNameList && preNameList.Length > 0)
        {
            index = UnityEngine.Random.Range(0, preNameList.Length - 1);
            preStr = preNameList[index];
        }


        if (maleList != null && sex == enmCharSex.MALE)
        {
            index = UnityEngine.Random.Range(0, maleList.Length - 1);
            maleStr = maleList[index];
        }
        else if (femalList != null && sex == enmCharSex.FEMALE)
        {
            index = UnityEngine.Random.Range(0, femalList.Length - 1);
            femaleStr = femalList[index];
        }

        if (string.IsNullOrEmpty(preStr) && string.IsNullOrEmpty(maleStr) && string.IsNullOrEmpty(femaleStr))
        {
            Engine.Utility.Log.Error("随机错误");
            return string.Empty;
        }
        string name = preStr + maleStr + femaleStr;
        return name;
    }
    #region loginnotice
    List<Notice> m_lstNotice = new List<Notice>();
    public List<Notice> NoticeList
    {
        get
        {
            return m_lstNotice;
        }
    }
    public void LoadFromUrl()
    {
        if (Application.isEditor && GameEntry.Instance().NetDebug)
        {
            ParesFromFile();
        }
        else
        {
            string strURL = GlobalConfig.Instance().NoticeUrl;
            Engine.Utility.FileUtils.Instance().LoadHttpURL(strURL, LoadNoticeFileFinish, GameEntry.Instance().gameObject);
        }
    
    }
    void ParesFromFile()
    {

        string strFilePath = "notice.json";
        Engine.JsonNode root = Engine.RareJson.ParseJsonFile(strFilePath);
        if (root == null)
        {
            Engine.Utility.Log.Warning("LoginNotice 解析{0}文件失败!", strFilePath);
            return;
        }

        Engine.JsonArray noticeArray = (Engine.JsonArray)root["notice"];
        for (int i = 0, imax = noticeArray.Count; i < imax; i++)
        {
            Engine.JsonObject noticeObj = (Engine.JsonObject)noticeArray[i];
            if (noticeObj == null)
            {
                continue;
            }

            Notice notice = new Notice();
            notice.index = int.Parse(noticeObj["index"]);
            notice.title = noticeObj["title"];
            notice.content = noticeObj["content"];
            m_lstNotice.Add(notice);
        }
        DispatchValueUpdateEvent("RefreshNotice", null, null);
    }
    private void LoadNoticeFileFinish(WWW www, object param = null)
    {
        m_lstNotice.Clear();
        if (www.error != null || www.text == string.Empty || www.text.Length == 0)
        {
            UnityEngine.Debug.LogError("LoadNoticeFileFinish----error: " + www.error + "---www.text :" + www.text + "---www.text.Length:" + www.text.Length);

            ParesFromFile();
            return;
        }

        Engine.JsonNode root = Engine.RareJson.ParseJson(www.text);
        if (root == null)
        {
            Engine.Utility.Log.Warning("LoginNotice 解析{0}文件失败!", www.text);
            return;
        }

        Engine.JsonArray noticeArray = (Engine.JsonArray)root["notice"];
        for (int i = 0, imax = noticeArray.Count; i < imax; i++)
        {
            Engine.JsonObject noticeObj = (Engine.JsonObject)noticeArray[i];
            if (noticeObj == null)
            {
                continue;
            }

            Notice notice = new Notice();
            notice.index = int.Parse(noticeObj["index"]);
            notice.title = noticeObj["title"];
            notice.content = noticeObj["content"];
            m_lstNotice.Add(notice);
        }
        DispatchValueUpdateEvent("RefreshNotice", null, null);
    }
    #endregion
}
public class Notice
{
    public int index;
    public string title;
    public string content;
}
