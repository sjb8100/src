/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ZqgameSDKController
 * 版本号：  V1.0.0.0
 * 创建时间：9/13/2017 7:24:40 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

class ZqgameSDKController : Singleton<ZqgameSDKController>
{

    #region sdkparam
    //public const string gameId = "227";
    //public const string gameMark = "227";
    //public const string appkey = "16ULgnUf6GBKpSoPEwjo23I26AF2vW0l";
    //public const string adverInfo = "";
    //public const string remark = "0";
    string gameId = "";
    string gameMark = "";
    string appkey = "";
    string adverInfo = "";
    string remark = "";
    #endregion

    #region property
    public bool PlatformEnable
    {
        get
        {
            return Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }
    #endregion

    #region sdkdelegate
    public delegate void SDK_LOGINBACK_CALLBACK(bool success, string passID, string accountName, string token);

    public delegate void SDK_PAYBACK_CALLBACK(bool success, string transaction, string order);

    public delegate void SDK_LOGOUTBACK_CALLBACK(bool success);

    public delegate void SDK_RIGISTERBACK_CALLBACK(bool success);

    public delegate void SDK_BACKGAME_CALLBACK(bool isLogout);

    public delegate void SDK_PLAYLOGOANIMFINISH_CALLBACK();

    public delegate void SDK_COMMENT_CALLBACK(int action);

    public delegate void SDK_DISSTATEMENT_CALLBACK(int action);

    #endregion

    #region sdkdelegateimp

    /// <summary>
    /// 登陆sdk回调
    /// </summary>
    /// <param name="success"></param>
    /// <param name="passID"></param>
    /// <param name="accountName"></param>
    /// <param name="token"></param>
    [MonoPInvokeCallback(typeof(SDK_LOGINBACK_CALLBACK))]
    private static void SDKLoginCallback(bool success, string passID, string accountName, string token)
    {
        Debug.Log(string.Format("Zqgame->SDKLoginCallback success:{0} passID:{1} accountName:{2} token:{3}", success, passID, accountName, token));

        if (success)
        {
            CommonSDKPlaform.Instance.LoginResult.token = token;
            CommonSDKPlaform.Instance.LoginResult.uid = accountName;
            CommonSDKPlaform.Instance.LoginResult.account = accountName;
            CommonSDKPlaform.Instance.LoginResult.accountID = passID;
            CommonSDKPlaform.Instance.LoginResult.pid = 17;
        }
        DataManager.Manager<LoginDataManager>().OnLogin(success);
    }

    /// <summary>
    /// 支付回调
    /// </summary>
    /// <param name="success"></param>
    /// <param name="transaction"></param>
    /// <param name="order"></param>
    [MonoPInvokeCallback(typeof(SDK_PAYBACK_CALLBACK))]
    private static void SDKPayCallback(bool success, string transaction, string order)
    {
        Debug.Log(string.Format("Zqgame->SDKPayCallback success:{0} transaction:{1} order:{2}", success, transaction, order));
        DataManager.Manager<RechargeManager>().OnPlatformPayResult(success, order, transaction);
    }

    /// <summary>
    /// 登出回调
    /// </summary>
    /// <param name="success"></param>
    [MonoPInvokeCallback(typeof(SDK_LOGOUTBACK_CALLBACK))]
    private static void SDKLogoutCallback(bool success)
    {
        Debug.Log(string.Format("Zqgame->SDKLogoutCallback success:{0}", success));
        DataManager.Manager<LoginDataManager>().OnLogout(success);
    }

    /// <summary>
    /// 注册回调
    /// </summary>
    /// <param name="success"></param>
    [MonoPInvokeCallback(typeof(SDK_RIGISTERBACK_CALLBACK))]
    private static void SDKRegisterCallback(bool success)
    {
        Debug.Log(string.Format("Zqgame->SDKRegisterCallback success:{0}", success));
    }

    /// <summary>
    /// 登出回调
    /// </summary>
    /// <param name="isLogout">注销</param>
    [MonoPInvokeCallback(typeof(SDK_BACKGAME_CALLBACK))]
    private static void SDKBackGameCallback(bool isLogout)
    {
        Debug.Log(string.Format("Zqgame->SDKBackGameCallback isLogout:{0}", isLogout));
    }

    /// <summary>
    /// 播放logo动画回调
    /// </summary>
    [MonoPInvokeCallback(typeof(SDK_PLAYLOGOANIMFINISH_CALLBACK))]
    private static void SDKPlayLogoAnimFinishCallback()
    {
        Debug.Log("Zqgame->SDK_PlayLogoAnimFinishCallback");
    }

    [MonoPInvokeCallback(typeof(SDK_COMMENT_CALLBACK))]
    private static void SDKCommentCallback(int action)
    {
        Debug.Log(string.Format("Zqgame->SDKCommentCallback action:{0}", action));
    }

    [MonoPInvokeCallback(typeof(SDK_DISSTATEMENT_CALLBACK))]
    private static void SDKDisStatementCallback(int action)
    {
        Debug.Log(string.Format("Zqgame->SDKDisStatementCallback action:{0}", action));
    }
    #endregion

    #region outCall
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        if (!PlatformEnable)
        {
            Debug.Log("ZqgameSDKController->Initialize PlatformEnable not fit");
            return;
        }
        Debug.Log("ZqgameSDKController->Initialize");
        Delegate_Init(SDKLoginCallback
                , SDKPayCallback
                , SDKLogoutCallback
                , SDKRegisterCallback
                , SDKBackGameCallback
                , SDKPlayLogoAnimFinishCallback
                , SDKCommentCallback
                , SDKDisStatementCallback);
        zqgameSDK_SetDebugMode(true);
        gameId = GlobalConfig.Instance().IOSGameID;
        gameMark = GlobalConfig.Instance().IOSgameMark;
        appkey = GlobalConfig.Instance().IOSappkey;
        //adverInfo = GlobalConfig.Instance().IOSadverInfo;
        adverInfo = "";
        remark = GlobalConfig.Instance().IOSremark;
        zqgameSDK_Init(gameId, appkey, adverInfo, gameMark, remark);
    }

    /// <summary>
    /// 登陆
    /// </summary>
    public void Login(bool ignoreAutoLogin)
    {
        if (!PlatformEnable)
        {
            Debug.Log("ZqgameSDKController->Login PlatformEnable not fit");
            return;
        }
        zqgameSDK_Login(ignoreAutoLogin);
    }

    /// <summary>
    /// 登出
    /// </summary>
    public void LogOut()
    {
        if (!PlatformEnable)
        {
            Debug.Log("ZqgameSDKController->Login PlatformEnable not fit");
            return;
        }
        zqgameSDK_Logout();
    }

    /// <summary>
    /// 创建角色统计
    /// </summary>
    /// <param name="zoneId"></param>
    /// <param name="roleId"></param>
    public void CreateRole(string zoneId, string roleId)
    {
        if (!PlatformEnable)
        {
            Debug.Log("ZqgameSDKController->CreateRole PlatformEnable not fit");
            return;
        }
        zqgameSDK_createRole(zoneId, roleId);
    }


    public void Pay(string money, string account,
        string roleID, string gameOrder,
        string accountId, string session,
        string productIAP, string serverNo,
        string lv)
    {
        if (!PlatformEnable)
        {
            Debug.Log("ZqgameSDKController->Pay PlatformEnable not fit");
            return;
        }
        string remark = string.Format("{0}|{1}|{2}", gameOrder, roleID, money);
        Debug.LogError(string.Format("IOS payInfo: gameId:{0},money:{1},account:{2},roleID:{3},gameOrder:{4},accountId:{5},session:{6},productIAP:{7},serverNo:{8},lv:{9},remark:{10}", gameId, money, account,roleID,gameOrder, accountId, session, productIAP, serverNo, lv, remark));
        //zqgameSDK_changePayStyleWithGameId(gameId, money, account, roleID
        //    , gameOrder, accountId, session
        //    , productIAP, serverNo, lv, remark);
        zqgameSDK_iapPay(accountId, account, session, gameOrder, "", productIAP);
    }
    #endregion

    #region sdkInternal

    [DllImport("__Internal")]
    public static extern void Delegate_Init(SDK_LOGINBACK_CALLBACK loginCallback
        , SDK_PAYBACK_CALLBACK payCallback
        , SDK_LOGOUTBACK_CALLBACK logoutCallback
        , SDK_RIGISTERBACK_CALLBACK registerCallback
        , SDK_BACKGAME_CALLBACK backgameCallback
        , SDK_PLAYLOGOANIMFINISH_CALLBACK logAnimCallback
        , SDK_COMMENT_CALLBACK commentCallback
        , SDK_DISSTATEMENT_CALLBACK disstatementCallback);

    /// <summary>
    /// Sets the hello.
    /// </summary>
    /// <param name="str">String.</param>
    [DllImport("__Internal")]
    private static extern void zqgameSDK_Init(string gameId, string appKey, string adverInfo, string gameMark, string remark);

    [DllImport("__Internal")]
    private static extern void zqgameSDK_SetDebugMode(bool debug);

    [DllImport("__Internal")]
    private static extern void zqgameSDK_Login(bool ignoreAutoLogin);

    [DllImport("__Internal")]
    private static extern void zqgameSDK_Logout();

    [DllImport("__Internal")]
    private static extern void zqgameSDK_createRole(string zoneID, string roleID);

    /*! @brief 移动充值
    *
    * @param gameId        中青宝游戏平台分配的gameId
    * @param accountId     游戏账号id
    * @param account       游戏登录时的账号名
    * @param roleID        角色id （可传空字符串 如 @""，建议传入相关参数，用于深度控制切换支付方式）
    *
    * @param session       渠道session或者access_token
    * @param gameOrder     游戏订单号
    * @param ProductID_IAP 内购商品id
    * @param money         充值的金额( 这里金额的单位是 元)
    * @param serverNo      游戏自身服务器id  （可传空字符串 如 @""，建议传入相关参数，用于深度控制切换支付方式）
    *
    * @param level         level 人物等级  （可传空字符串 如 @""，建议传入相关参数，用于深度控制切换支付方式）
    *
    * @param remark  remark = 订单号|roleID|money （按此格式传入，这里的money单位是分，如有更多自由参数，后面  如：jyfs_11_57d6|001|1000|3000|.....）
    *
    */

    [DllImport("__Internal")]
    private static extern void zqgameSDK_changePayStyleWithGameId(
        string gameId, string money, string account,
        string roleID, string gameOrder,
        string accountId, string session,
        string productIAP, string serverNo,
        string lv, string remark);

    /*! @brief 移动充值(IAP内购方式一)
     *
     * @param delegate 代理类
     * @param accountId 游戏账号id
     * @param account 游戏账号名 可以为空，或不传此参数
     * @param session 渠道session或者access_token
     * @param gameOrder 游戏订单号
     * @param ext 可不填
     * @param ProductID_IAP 内购商品ID
     */
    [DllImport("__Internal")]
    private static extern void zqgameSDK_iapPay(string accountId,string account,string session,string gameOrder,string ext,string productIAP);

    #endregion
}
