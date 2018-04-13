
//*************************************************************************
//	创建日期:	2016/12/19 星期一 16:16:42
//	文件名称:	CommonSDKPlaform
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client;
using UnityEngine;
using System.Collections;
using LitJson;
public class CommonSDKPlaform : SingletonMono<CommonSDKPlaform>
{
    public class loginResult
    {
        public string token = ""; //session id
        public string uid = "";   //username or user id
        public uint pid = 0;    //platform id
        public string account = "";//for zqgame sdk
        public string accountID = ""; //for zqgame ios sdk
        public string szLoginAcccount = ""; //login return
        public string szLoginSession = "";
        public string szLoginDataEx = "";
        public string uiLoginPlatUserID = "";
    }


    //  public static CommonSDKPlaform Instance { private set; get; }

#if CommonSDK
    //mono初始化会直接调用这个来初始化sdk
    AndroidJavaObject mainApplication = null;
    AndroidJavaObject sdkBase = null;
    AndroidJavaObject sdkObject = null;
    AndroidJavaObject gameActivity = null;
    AndroidJavaObject umengAnalytics = null;

#endif
    //static string tempText = "msg";
    static string sendText = "edit";

    loginResult m_loginResult = new loginResult();//登陆数据

    public loginResult LoginResult
    {
        get
        {
            return m_loginResult;
        }
    }
    //mono初始化会直接调用这个来初始化sdk
    void Awake()
    {

    }

    public void Init()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ZqgameSDKController.Instance.Initialize();
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            //Debug.LogError("Awake CommonSDKPlaform:" + gameObject.name);
            //Debug.LogError("Awake IsSDKLogin:" + DataManager.Manager<LoginDataManager>().IsSDKLogin);

            //设置屏幕正方向在Home键右边
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            mainApplication = new AndroidJavaClass("com.talkingsdk.MainApplication").CallStatic<AndroidJavaObject>("getInstance");
            sdkBase = mainApplication.Call<AndroidJavaObject>("getSdkInstance");

            sdkBase.Call("setUnityGameObject", gameObject.name);

            AndroidJavaObject umengPush = new AndroidJavaClass("com.talkingsdk.plugin.ZQBPush").CallStatic<AndroidJavaObject>("getInstance");
            umengPush.Call("startPush");
            //Debug.LogError("Awake:" + gameObject.name);
        }
        //Debug.Log("init commonsdk ");
    }



    void Start()
    {
        //设置屏幕自动旋转， 并置支持的方向
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.LogError("Input.GetKeyDown(KeyCode.Escape)");
            KeyBack();
        }
        /*
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.LogError("Input.GetKeyUp(KeyCode.Escape)");
            //KeyBack();
        }


        if (Input.GetKey(KeyCode.Home))
        {
            Debug.LogError("KeyCode.Home");
            KeyBack();
        }
         */
    }

    //qq渠道号
    public const int QQ_CHANNEL_ID = 27;
    //微信
    public const int WECHAT_CHANNEL_ID = 35;
    /// <summary>
    /// QQ登陆
    /// </summary>
    /// <param name="ignoreAutoLogin"></param>
    public void LoginQQ(bool ignoreAutoLogin = false)
    {
        if (!DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("CommonSDKPlatform->Login failed sdklogin state error");
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (sdkBase == null)
            {
                Debug.LogError("sdkBase is null............................");
                return;
            }
            sdkBase.Call("login", QQ_CHANNEL_ID);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ZqgameSDKController.Instance.Login(ignoreAutoLogin);
        }
    }

    /// <summary>
    /// 微信登陆
    /// </summary>
    /// <param name="ignoreAutoLogin"></param>
    public void LoginWechat(bool ignoreAutoLogin = false)
    {
        if (!DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("CommonSDKPlatform->Login failed sdklogin state error");
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (sdkBase == null)
            {
                Debug.LogError("sdkBase is null............................");
                return;
            }
            sdkBase.Call("login", WECHAT_CHANNEL_ID);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ZqgameSDKController.Instance.Login(ignoreAutoLogin);
        }
    }

    /// <summary>
    /// 登陆接口
    /// </summary>
    public void Login(bool ignoreAutoLogin = false)
    {
        if (!DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("CommonSDKPlatform->Login failed sdklogin state error");
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (sdkBase == null)
            {
                Debug.LogError("sdkBase is null............................");
                return;
            }
            sdkBase.Call("login");
        }else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            ZqgameSDKController.Instance.Login(ignoreAutoLogin);
        }
        Debug.LogError("go to login....");
    }

    /// <summary>
    /// 微信登陆接口
    /// </summary>
    public void LoginWeiXin()
    {
        Debug.LogError("go to login....");
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            sdkBase.Call("loginWeiXin");
        }
    }

    #region Recharge(充值)
    string payResult = "";
    string orderID = "";
    /// <summary>
    /// 支付接口
    /// </summary>
    public void Pay(GameCmd.stCreatePlatOrderPropertyUserCmd_S orderMsg,
        uint characterId, int characterlv,string roleName, string clanName
        , Pmd.ZoneInfo zoneInfo, string currencyName, float currencyRatio
        ,table.RechargeDataBase rechargeDB)
    {
        if (!DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("Pay Faield,SDK not login!");
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (null == rechargeDB)
            {
                Debug.LogError("充值失败，数据错误");
                return;
            }

            AndroidJavaObject payData = new AndroidJavaObject("com.talkingsdk.models.PayData");
            AndroidJavaObject hashmap = new AndroidJavaObject("java.util.HashMap");
            System.IntPtr methodPut = AndroidJNIHelper.GetMethodID(hashmap.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");

            Debug.Log("m_loginResult.uid :" + m_loginResult.uid);


            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add("UserId", m_loginResult.szLoginAcccount); //uid
            //dict.Add("UserId", m_loginResult.uid); //uid
            dict.Add("UserBalance", "0"); //用户余额
            dict.Add("UserGamerVip", "1"); //vip 等级
            dict.Add("UserLevel", characterlv.ToString()); //角色等级
            if (string.IsNullOrEmpty(clanName))
            {
                clanName = "testName";
            }
            dict.Add("UserPartyName", clanName); //工会，帮派
            if (string.IsNullOrEmpty(roleName))
            {
                roleName = "Underworld";
            }
            dict.Add("UserRoleName", roleName); //角色名称
            dict.Add("UserRoleId", characterId.ToString()); //角色id
            

            dict.Add("LoginAccount", m_loginResult.szLoginAcccount);
            dict.Add("LoginDataEx", m_loginResult.szLoginDataEx);
            dict.Add("LoginSession", m_loginResult.szLoginSession);

            dict.Add("AccessKey", orderMsg.sign); //拓展字段

            dict.Add("OutOrderID", orderMsg.platorder); //平台订单号
            dict.Add("NoticeUrl", orderMsg.noticeurl); //支付回调地址

            //UserServerId
            string zoneIdStr = "";
            string serverNameStr = "Underworld";
            if (null != zoneInfo)
            {
                //合服后使用目标服务器
                zoneIdStr = LoginDataManager.GetBestZoneID(zoneInfo).ToString();
                serverNameStr = zoneInfo.zonename;
            }

            dict.Add("UserServerId", zoneIdStr);
            dict.Add("UserServerName", serverNameStr); //
            int payMoney = (int)rechargeDB.money;
            int gameMoneyAmout = payMoney * 10;
            dict.Add("GameMoneyAmount", gameMoneyAmout.ToString());
            dict.Add("GameMoneyName", currencyName);


            object[] args = new object[2];
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key);
                AndroidJavaObject v = new AndroidJavaObject("java.lang.String", kvp.Value);
                args[0] = k;
                args[1] = v;
                AndroidJNI.CallObjectMethod(hashmap.GetRawObject(), methodPut, AndroidJNIHelper.CreateJNIArgArray(args));
            }

            Debug.LogError("go to pay....  setMyOrderId " + orderMsg.gameorder +
                        " payGoodsData.pay_info.out_order: " + orderMsg.platorder +
                        " rmb: " + payMoney +
                        " setProductCount :" + orderMsg.goodnum +
                        " setProductId :" + orderMsg.appgoodid +
                        " setProductName :" + rechargeDB.rechargeName +
                        " setSubmitTime:" + orderMsg.createtime +
                        " extData:" + orderMsg.extdata +
                        " AccessKey:" + orderMsg.sign +
                        " userID:" + m_loginResult.szLoginAcccount +
                        " LoginAcccount:" + m_loginResult.szLoginAcccount
                        );

            //string 类型
            payData.Call("setMyOrderId", orderMsg.gameorder);
            payData.Call("setProductId", orderMsg.appgoodid.ToString());//"1");//
            payData.Call("setSubmitTime", orderMsg.createtime);
            payData.Call("setDescription", rechargeDB.desc);
            payData.Call("setProductName", rechargeDB.rechargeName);

            //int 类型 ,SDK 这边统一以分为单位
            
            int fenPayMoney = payMoney * 100;
            payData.Call("setProductRealPrice", fenPayMoney);
            payData.Call("setProductIdealPrice", fenPayMoney);
            payData.Call("setProductCount", (int)orderMsg.goodnum);
            payData.Call("setEx", hashmap);

            sdkBase.Call("pay", payData);

        }else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {

        }
    }

    /// <summary>
    /// 充值结果回调
    /// </summary>
    /// <param name="result"></param>
    void OnPayResult(string result)
    {
        Debug.LogError("OnPayResult:" + result);    
        JsonData jd = JsonMapper.ToObject(UnderWorld.Utils.JSON.JsonEncode(UnderWorld.Utils.JSON.JsonDecode(result)));
        if (((IDictionary)jd).Contains("MyOrderId"))
        {
            orderID = (string)jd["MyOrderId"];
        }
        bool success = false;
        string status = "";
        //如果有额外的平台ID，则给它重新赋值
        if (((IDictionary)jd).Contains("Ext"))
        {
            JsonData ext = jd["Ext"];
            if (((IDictionary)ext).Contains("PayResult"))
            {
                Debug.Log("PlatformId Reset :" + (string)ext["PayResult"]);
                payResult = (string)ext["PayResult"];
            }
            if (((IDictionary)ext).Contains("status"))
            {
                status = (string)ext["status"];
                int statusInt = 0;
                if (int.TryParse(status,out statusInt) && statusInt == 10)
                {
                    success = true;
                }
            }
        }
        DataManager.Manager<RechargeManager>().OnPlatformPayResult(success,orderID);
     }

    
    /// <summary>
    /// 发送充值凭证，这个只用于验证测试。
    /// </summary>
    public void SendMessageToTestPayResultOrder()
    {
        if (orderID != "")
        {
            //PayNetWork.GetInstance().SendMessagePayResultOrder(m_loginResult.pid, orderID, payResult);
        }
        else
        {
            Debug.Log("U have not recharge , please check again !");
        }
    }
            
    #endregion





    







    


    string getProductionNameByID(int id)
    {

        return "一块冲值！";
    }

    public void SetUserID(string id)
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.Log("CommonSDK Platform  SetUserID:" + id.ToString());
            m_loginResult.uid = id;
        }
    }

    /// <summary>
    /// 设置登陆字段
    /// </summary>
    /// <param name="loginAccount"></param>
    /// <param name="loginSession"></param>
    /// <param name="extData"></param>
    public void SetLoginSuccessData(string loginAccount,string loginSession,string extData)
    {
        m_loginResult.szLoginAcccount = loginAccount;
        m_loginResult.szLoginSession = loginSession;
        m_loginResult.szLoginDataEx = extData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jsonData"></param>
    public void SetLoginData(string jsonData)
    {
        /*
        Result :{
        "nPlatformID" : 0,
        "szAcccount" : "whtest3",
        "szDataEx" : "whtest3",
        "szSession" : "",
        "uiPlatUserID" : 0
        }
        */

        Debug.LogError("Result :" + jsonData);

        JsonData jd = JsonMapper.ToObject(UnderWorld.Utils.JSON.JsonEncode(UnderWorld.Utils.JSON.JsonDecode(jsonData)));

        /*
        if (((IDictionary)jd).Contains("nPlatformID"))
        {
            Debug.LogError("nPlatformID :" + (jd["nPlatformID"]).ToString());
        }
        */


        if (((IDictionary)jd).Contains("szAcccount"))
        {
            Debug.LogError("szAcccount :" + (jd["szAcccount"]).ToString());
            m_loginResult.szLoginAcccount = (jd["szAcccount"]).ToString();
        }

        if (((IDictionary)jd).Contains("szDataEx"))
        {
            Debug.LogError("szDataEx :" + (jd["szDataEx"]).ToString());
            m_loginResult.szLoginDataEx = (jd["szDataEx"]).ToString();
        }

        if (((IDictionary)jd).Contains("szSession"))
        {
            Debug.LogError("szSession :" + (jd["szSession"]).ToString());
            m_loginResult.szLoginSession = (jd["szSession"]).ToString();
        }

        if (((IDictionary)jd).Contains("uiPlatUserID"))
        {
            Debug.LogError("uiPlatUserID :" + (jd["uiPlatUserID"]).ToString());
            m_loginResult.uiLoginPlatUserID = (jd["uiPlatUserID"]).ToString();
        }


    }

    public uint GetPlatformID()
    {
        return m_loginResult.pid;
    }

    /// <summary>
    /// 显示Tool Bar
    /// </summary>
    public void ShowToolBar()
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("go to showToolBar....");
            sdkBase.Call("showToolBar");
        }
    }

    /// <summary>
    /// 隐藏Tool Bar
    /// </summary>
    public void DestroyToolBar()
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("go to destroyToolBar....");
            sdkBase.Call("destroyToolBar");
        }
    }

    /// <summary>
    /// 显示用户中心
    /// </summary>
    public void ShowUserCenter()
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("go to showUserCenter....");
            sdkBase.Call("showUserCenter");
        }
    }


    /// <summary>
    /// 切换帐号
    /// </summary>
    public void ChangeAccount()
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("go to change account....");
            sdkBase.Call("changeAccount");
        
        }
    }


    /// <summary>
    /// 登出
    /// </summary>
    public void Logout()
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {

            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.LogError("go to logout....");
                sdkBase.Call("logout");
            }else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                ZqgameSDKController.Instance.LogOut();
            }
            
         //   umengAnalytics.Call("logout");
        }
    }


    #region SDKStat(SDK统计)
    /// <summary>
    /// 通知SDK创建角色
    /// </summary>
    /// <param name="zoneID">区服ID</param>
    /// <param name="roleId">角色ID</param>
    /// <param name="roleName">角色名称</param>
    /// <param name="zoneName">区服名称</param>
    public void CreateRoleToSDK(string serverNo, string serverName,long roleId, string roleName, string createTime)
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("go to UserUpLever  setServerNo:" + serverNo + "   setLevel:" + 1 + "   setRoleName:" + roleName);
            AndroidJavaObject playerData = new AndroidJavaObject("com.talkingsdk.models.PlayerData");
            playerData.Call("setServerNo", serverNo);
            playerData.Call("setLevel", (int)1);
            playerData.Call("setRoleId", roleId);
            playerData.Call("setRoleName", roleName);
            playerData.Call("setServerName", serverName);

            AndroidJavaObject hashmap = new AndroidJavaObject("java.util.HashMap");
            System.IntPtr methodPut = AndroidJNIHelper.GetMethodID(hashmap.GetRawClass(), "put"
            , "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string logStr = "";
            dict.Add("roleCTime", createTime);//角色创建时间	
            object[] args = new object[2];
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                logStr += kvp.Key + ":" + kvp.Value;
                AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key);
                AndroidJavaObject v = new AndroidJavaObject("java.lang.String", kvp.Value);
                args[0] = k;
                args[1] = v;
                AndroidJNI.CallObjectMethod(hashmap.GetRawObject(), methodPut, AndroidJNIHelper.CreateJNIArgArray(args));
            }
            Debug.Log("CreateRoleToSDK data to sdk->" + logStr);
            playerData.Call("setEx", hashmap);

            sdkBase.Call("createRole", playerData);
        }
    }
    /// <summary>
    /// 进入游戏回调
    /// </summary>
    /// <param name="serverNo"></param>
    /// <param name="playerID"></param>
    /// <param name="playerName"></param>
    public void EnterGame(string serverNo, string serverName,long roleId,string roleCreateTime,int roleLv, string roleName)
    {
        Debug.Log("go to EnterGam  serverNo: " + serverNo + "   setRoleId:" + roleId.ToString() + "   playerName:" + roleName);
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            AndroidJavaObject playerData = new AndroidJavaObject("com.talkingsdk.models.PlayerData");
            playerData.Call("setServerNo", serverNo);
            playerData.Call("setRoleId", roleId);
            playerData.Call("setRoleName", roleName);
            playerData.Call("setLevel", roleLv); //role level
            playerData.Call("setServerName", serverName);
            AndroidJavaObject hashmap = new AndroidJavaObject("java.util.HashMap");
            System.IntPtr methodPut = AndroidJNIHelper.GetMethodID(hashmap.GetRawClass(), "put"
            , "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string logStr = "";
            dict.Add("roleCTime", roleCreateTime);//角色创建时间	
            object[] args = new object[2];
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                logStr += kvp.Key + ":" + kvp.Value;
                AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key);
                AndroidJavaObject v = new AndroidJavaObject("java.lang.String", kvp.Value);
                args[0] = k;
                args[1] = v;
                AndroidJNI.CallObjectMethod(hashmap.GetRawObject(), methodPut, AndroidJNIHelper.CreateJNIArgArray(args));
            }
            Debug.Log("EnterGame data to sdk->" + logStr);
            playerData.Call("setEx", hashmap);

            sdkBase.Call("enterGame", playerData);

        }
    }


    /// <summary>
    /// 玩家升级
    /// </summary>
    public void UserUpLever(string serverNo, string serverName, long roleId,string createTime ,string roleName, int level)
    {
        Debug.LogError("go to UserUpLever  setServerNo:" + serverNo + "   setLevel:" + level.ToString() + "   setRoleName:" + roleName);
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            AndroidJavaObject playerData = new AndroidJavaObject("com.talkingsdk.models.PlayerData");
            playerData.Call("setServerNo", serverNo);
            playerData.Call("setLevel", level);
            playerData.Call("setRoleId", roleId);
            playerData.Call("setServerName",serverName);
            playerData.Call("setRoleName", roleName);

            AndroidJavaObject hashmap = new AndroidJavaObject("java.util.HashMap");
            System.IntPtr methodPut = AndroidJNIHelper.GetMethodID(hashmap.GetRawClass(), "put"
            , "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string logStr = "";
            dict.Add("roleCTime", createTime);//角色创建时间	
            object[] args = new object[2];
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                logStr += kvp.Key + ":" + kvp.Value;
                AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key);
                AndroidJavaObject v = new AndroidJavaObject("java.lang.String", kvp.Value);
                args[0] = k;
                args[1] = v;
                AndroidJNI.CallObjectMethod(hashmap.GetRawObject(), methodPut, AndroidJNIHelper.CreateJNIArgArray(args));
            }
            Debug.Log("UserUpLever data to sdk->" + logStr);
            playerData.Call("setEx", hashmap);
            sdkBase.Call("userUpLevel", playerData);
        }
    }
    #endregion


    /// <summary>
    /// 设置分数
    /// </summary>
    public void SetRankScore(string sorce, string rank)
    {
        //目前rank字段是没用的，暂时先传个OK过去，以后游泳的时候再打开
        Debug.LogError("SetRankScore:" + sorce + "  rank:" + rank);
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            sdkBase.Call("uploadScore", sorce, rank);
        }
    }


    /// <summary>
    /// 社交分享 - 韩国SDK加的接口，只有韩国版才会有
    /// </summary>
    public void SocietyShare(string url, string name, string desc, string picName)
    {
        Debug.LogError("go to KakaoShare....");
#if CommonSDK && UNITY_ANDROID
		sdkBase.Call("doShare", desc, url, name);
#elif CommonSDK && UNITY_IPHONE		
		_KakaoShare(url, name, desc, picName);
#endif
    }


    /// <summary>
    /// 显示游戏中心（这个与用户中心不是一个东西） - 韩国SDK加的接口，只有韩国版才会有
    /// </summary>
    public void ShowGameCenterInfo()
    {
        Debug.LogError("ShowGameCenterInfo....");
#if CommonSDK && UNITY_ANDROID
		sdkBase.Call("showGameCenter", "1", "Test", "1", "1");
#elif CommonSDK && UNITY_IPHONE		
		_ShowGameCenterInfo();
#endif
    }


    /// <summary>
    /// 显示广告通知 - 韩国SDK加的接口，只有韩国版才会有
    /// </summary>
    public void ShowAdsNotice()
    {
        Debug.LogError("ShowAdsNotice....");
#if CommonSDK && UNITY_ANDROID
		sdkBase.Call("showAdsNotice");
#elif CommonSDK && UNITY_IPHONE		
		_ShowAdNotices();
#endif
    }




    /// <summary>
    /// 销毁SDK进程
    /// </summary>
    public void DestroyActivity()
    {
        Debug.LogError("DestroyActivity");
        if (null != sdkBase && DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            sdkBase.Call("onActivityDestroy");
        }
    }



    /// <summary>
    /// 监听返回事件
    /// </summary>
    public void KeyBack()
    {
        Debug.LogError("KeyBack");

        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            sdkBase.Call("onKeyBack");
        }

    }

    /// <summary>
    /// share
    /// </summary>
    public void Share()
    {
        Debug.LogError("Share");

        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            //sdkBase.Call("showShare","内容","标题","分享连接","图片连接");
            //sdkBase.Call("showShare");
            //sdkBase.Call("showShare", "天下武功 為快不破 極致3D武俠動作手遊《這才叫江湖》", "", "我在玩一款超讃的手機遊戲《这才叫江湖》趕緊加入吧！輸入邀請碼：xxxxxx，加入即得到珍貴獎勵，根本停不下來！下載地址：", "http://www.facebook.com/i2iunderworld", ""/*"https://ss0.baidu.com/6ONWsjip0QIZ8tyhnq/it/u=3062734635,4183904616&fm=96"*/);

            // shareMode 0:微信 1：朋友圈 2：新浪微博 3：QQ 4：腾讯微博 5:QQ空间
            // 参数 ： shareMode，shareContent，shareTitle，shareUrl,shareImageUrl
            //sdkBase.Call("showShare","0,1,2,3,4,5","欢迎加入黑夜传说，邀请码:u4vdjr", "好友邀请", "http://game.html5.qq.com/dl?ch=999003&packageName=com.tencent.tmgp.hycs&size=264910638", "http://pp.myapp.com/ma_icon/0/icon_12063713_1450248531/96");

            AndroidJavaObject shareSdk = new AndroidJavaClass("com.talkingsdk.plugin.ZQBShare").CallStatic<AndroidJavaObject>("getInstance");
            AndroidJavaObject shareParams = new AndroidJavaObject("com.talkingsdk.ShareParams");
            shareParams.Call("setShareMode", "0,1,2,3,4,5");
            shareParams.Call("setTitle", "好友邀请");
            shareParams.Call("setContent", "欢迎加入黑夜传说，邀请码:u4vdjr");
            shareParams.Call("setSourceUrl", "http://game.html5.qq.com/dl?ch=999003&packageName=com.tencent.tmgp.hycs&size=264910638");
            shareParams.Call("setImgUrl", "http://pp.myapp.com/ma_icon/0/icon_12063713_1450248531/96");
            shareSdk.Call("share", shareParams);

        }

    }

    /// <summary>
    /// share回调
    ///    code: 1成功，0失败
    /// </summary>
    void onShareRequest(string code)
    {
        Debug.LogError("onShareRequest for unity: " + code);

    }

    /// <summary>
    /// 第三方回调
    /// </summary>
    void onResult(string msg)
    {
        Debug.LogError("onResult for unity: " + msg);
    }





    /// <summary>
    /// SDK初始化完成
    /// </summary>
    /// <param name="test"></param>
    void OnInitComplete(string result)
    {
        Debug.LogError("OnInitComplete:" + result);

        JsonData jd = JsonMapper.ToObject(UnderWorld.Utils.JSON.JsonEncode(UnderWorld.Utils.JSON.JsonDecode(result)));
        m_loginResult.pid = uint.Parse((string)jd["PlatformId"]);
        int retCode =(int) jd["code"];
        bool success = false;
        if(retCode == 1)
        {
            success = true;
            Debug.Log("初始化成功");
        }
        else
        {
            Debug.Log("初始化失败");
        }
        //UmengPlatformHelp.UmengInit(m_loginResult.pid.ToString());//初始化友盟统计平台
        DataManager.Manager<LoginDataManager>().OnSDKInitComplete(success);
    }

    /// <summary>
    /// 回调 - 登陆
    /// </summary>
    /// <param name="result"></param>
    void OnLoginResult(string result)
    {
        JsonData jd = JsonMapper.ToObject(UnderWorld.Utils.JSON.JsonEncode(UnderWorld.Utils.JSON.JsonDecode(result)));

        /*
        m_loginResult.pid = uint.Parse((string)jd["pid"]);
        m_loginResult.token = (string)jd["token"];
        m_loginResult.uid = (string)jd["uid"];
         */

        m_loginResult.token = (string)jd["SessionId"];
        string uid = (string)jd["UserId"];
        if (uid == "")
        {
            uid = (string)jd["UserName"];
        }
        m_loginResult.uid = uid;
        //服务器登录使用account  用uid
        m_loginResult.account = uid;
        //	umengAnalytics.Call("login",uid);
        bool success = true;
        //如果有额外的平台ID，则给它重新赋值
        if (((IDictionary)jd).Contains("Ext"))
        {
            JsonData ext = jd["Ext"];
            if (((IDictionary)ext).Contains("PlatformId"))
            {
                m_loginResult.pid = uint.Parse((string)ext["PlatformId"]);
            }
            if (((IDictionary)ext).Contains("status"))
            {
                int status = int.Parse((string)ext["status"]);
                if (status == 5)
                {
                    success = false;
                }
            }

        }

        DataManager.Manager<LoginDataManager>().OnLogin(success);
    }


   

    void OnLogoutResult(string result)
    {
        Debug.LogError("OnLogoutResult: " + result);
        if (result.Equals("8"))//CODE_LOGOUT_SUCCESS
        {
            DataManager.Manager<LoginDataManager>().OnLogout(true);
        }
        else if (result.Equals("9"))//CODE_LOGOUT_FAIL
        {
            DataManager.Manager<LoginDataManager>().OnLogout(false);
        }
    }

    void OnChangeAccountResult(string result)
    {
        Debug.Log("OnChangeAccount:" + result);
        JsonData jd = JsonMapper.ToObject(UnderWorld.Utils.JSON.JsonEncode(UnderWorld.Utils.JSON.JsonDecode(result)));
       
        bool success = true;
        //如果有额外的平台ID，则给它重新赋值
        if (((IDictionary)jd).Contains("Ext"))
        {
            JsonData ext = jd["Ext"];
            if (((IDictionary)ext).Contains("status"))
            {
                int status = int.Parse((string)ext["status"]);
                if (status == 13)
                {
                    success = false;
                }
            }
            if (success)
            {
                if (((IDictionary)ext).Contains("PlatformId"))
                {
                    m_loginResult.pid = uint.Parse((string)ext["PlatformId"]);
                }
            }
        }

        if (success)
        {
            m_loginResult.token = (string)jd["SessionId"];
            string uid = (string)jd["UserId"];
            if (uid == "")
            {
                uid = (string)jd["UserName"];
            }
            m_loginResult.uid = uid;
            //服务器登录使用account  用uid
            m_loginResult.account = uid;
            //	umengAnalytics.Call("login",uid);
        }

        DataManager.Manager<LoginDataManager>().OnChangeAcountResult(success);
    }

    void OnExitAppResult(string result)
    {
        Debug.Log("OnExitAppResult");
        Application.Quit();
    }


    /// <summary>
    /// Facebook好友邀请
    /// </summary>
    public void FacebookFriendsInvite()
    {

        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            sdkBase.Call("facebookFriendsInvite");
        }
    }

    /// <summary>
    /// 获取Facebook好友列表方法
    /// </summary>
    public void GetFacebookFriends()
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("获取Facebook好友列表方法");

            sdkBase.Call("getFacebookFriends");
        }
    }

    /// <summary>
    /// 游戏角色战力财富值上传接口
    /// </summary>
    public void GameDataUpload()
    {
        string roleProfession = "profession";                // 游戏角色职业，没有可传空字符串
        string serverId = "1";                               // 游戏服Id，数值需大于0
        string serverName = "测试服";                         // 游戏服名称	   
        string roleId = "1";                                 // 游戏角色Id，数值需大于0
        string roleName = "roleName";                        // 游戏角色名称
        int roleLevel = 1;                                   // 游戏角色等级
        int roleBattle = 1;                                  // 角色战力
        int roleWealth = 1;                                  // 角色财富
        Debug.LogError("游戏角色战力财富值上传接口");
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            sdkBase.Call("gameDataUpload", roleProfession, serverId, serverName, roleId, roleName, roleLevel, roleBattle, roleWealth);
        }
    }

    /// <summary>
    /// 呼出社区论坛界面
    /// </summary>
    public void PopCommunityView()
    {

        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("呼出社区论坛界面");
            sdkBase.Call("popCommunityView");
        }
    }

    /// <summary>
    /// 获取Facebook好友列表回调，返回值为json数组字符串
    /// </summary>
    void onFacebookFriendsRequest(string result)
    {
        Debug.LogError("onFacebookFriendsSuccess:" + result);
        //
    }

    /// <summary>
    /// 获取tencent个人信息方法
    /// </summary>
    public void QueryMyInfo()
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("获取tencent个人信息方法");

            sdkBase.Call("queryMyInfo");
        }
    }

    /// <summary>
    /// 进入feedback界面
    /// </summary>
    public void OpenFeedback()
    {
        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            Debug.LogError("feedbackSdk 进入feedback界面");
            AndroidJavaObject feedbackSdk = new AndroidJavaClass("com.talkingsdk.plugin.ZQBFeedback").CallStatic<AndroidJavaObject>("getInstance");
            feedbackSdk.Call("openFeedback");
        }
    }

    /// <summary>
    /// 获取tencent个人信息回调
    /// </summary>
    void onQueryMyInfo(string result)
    {
        Debug.LogError("onQueryMyInfo for unity:" + result);
        //
    }

    void OnDestroy()
    {
        Debug.Log("CommonSDKPlatform OnDestroy");

        if (DataManager.Manager<LoginDataManager>().IsSDKLogin)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                DestroyActivity();
            }
        }


    }

}
