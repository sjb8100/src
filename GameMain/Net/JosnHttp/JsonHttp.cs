using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Common;
namespace GX.Net
{
    public enum HttpCompress
    {
        None,
        //[NotRenamed]
        gzip,
        //[NotRenamed]
        flate,
        //[NotRenamed]
        zlib,
        //[NotRenamed]
        lzw,
    }

    public partial class JsonHttp
    {
        #region JsonPmd
        public partial class HttpPackage
        {
            /// <summary>
            /// 消息类型
            /// </summary>
            public string @do { get; set; }
            /// <summary>
            /// 应用层消息内容
            /// </summary>
            public object data;
            /// <summary>
            /// 游戏ID
            /// </summary>
            public uint gameid { get; set; }
            /// <summary>
            /// 如果客户端发，服务器照样返回
            /// </summary>
            public uint zoneid { get; set; }
            /// <summary>
            /// 可唯一代表一个用户身份的ID，由平台统一生成
            /// </summary>
            public string uid { get; set; }
            /// <summary>
            /// 组合字段：&lt;平台id&gt;::&lt;平台账户&gt;
            /// </summary>
            public string sid { get; set; }
            /// <summary>
            /// 由PlatTokenLoginReturn返回，
            /// </summary>
            public string unigame_plat_login { get; set; }
            /// <summary>
            /// UNIX时间戳，单位秒
            /// </summary>
            public ulong unigame_plat_timestamp { get; set; }

            public uint logintype = 3;
        }

        public partial class HttpPackageReturn<T>
        {
            /// <summary>
            /// 消息类型
            /// </summary>
            public string @do { get; set; }
            /// <summary>
            /// 应用层消息内容
            /// </summary>
            public T data = default(T);
            /// <summary>
            /// 游戏ID
            /// </summary>
            public uint gameid { get; set; }
            /// <summary>
            /// 如果客户端发，服务器照样返回
            /// </summary>
            public uint zoneid { get; set; }
            public Pmd.HttpReturnCode errno { get; set; }

            public static HttpPackageReturn<T> Create(WWW www, JsonHttp host)
            {
                if (www == null)
                    return null;
                if (!string.IsNullOrEmpty(www.error))
                    return null;

                HttpPackageReturn<T> recv;
#if GX_JSONNET
				try
				{
					// TODO: 服务器下发的json有个bug，空的数组不会用“[]”而是“{}”，而Json.Net是强类型的，这里统一替换为null以应对空数组和空对象的情况
					recv = Json.Deserialize<HttpPackageReturn<T>>(Json.Proxy is JsonNetProxy ? www.text.Replace("{}", "null") : www.text);
				}
#else
                try { recv = Common.Json.Deserialize<HttpPackageReturn<T>>(www.text); }
#endif
                catch { return null; }
                if (recv == null)
                    return null;
                //if (recv.errno != Pmd.HttpReturnCode.HttpReturnCode_Null && recv.errno != Pmd.HttpReturnCode.HttpReturnCode_SignError)
                //    return null;
                return recv;
            }
        }
        #endregion

        public event EventHandler<HttpRequestEventArgs> OnRequest;
        public event EventHandler<HttpResponseEventArgs> OnResponse;

        /// <summary>
        /// 网络收发日志开关
        /// </summary>
        public bool Log { get; set; }

        public static readonly Dictionary<string, string> HeaderJson = new Dictionary<string, string>()
		{
			{"Content-Type", "application/json; charset=utf-8"},
		};

        /// <summary>
        /// HTTP消息的压缩方法
        /// </summary>
        public HttpCompress Compress { get; set; }

        /// <summary>
        /// 唯一标识某个游戏的类型ID
        /// </summary>
        public uint GameID { get; set; }
        /// <summary>
        /// 同一游戏的不同游戏分区
        /// </summary>
        public uint ZoneID { get; set; }
        /// <summary>
        /// unilight服务器时区相对于UTC的时间偏移量，单位秒
        /// </summary>
        public int TimeZone { get; private set; }
        /// <summary>
        /// LoginServer的地址
        /// </summary>
        public string LoginUrl { get; set; }
        /// <summary>
        /// unilight逻辑服务器的网关地址，由LoginServer派发得到
        /// </summary>
        public string GatewayUrl { get; private set; }
        /// <summary>
        /// <see cref="PlatInfo"/>的获取回调
        /// </summary>
        public Func<Pmd.PlatInfo> PlatInfoFactory { get; set; }

        public JsonHttp(string loginUrl, uint gameID, uint zoneID = 0, GX.Net.MessageDispatcher<object> dispatcher = null)
        {
            this.Log = true;

            this.LoginUrl = loginUrl;
            this.GameID = gameID;
            this.ZoneID = zoneID;

            this.OnRequest += (s, e) =>
            {
                if (this.Log)
                    Debug.Log(string.Format("<color=green>[SEND]</color> {0}\n{1}", e.Data, e.Url));
            };
            this.OnResponse += (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.WWW.error))
                    Debug.Log(string.Format("<color=red>[RECV]</color> {0}\n{1}", e.WWW.error, e.WWW.url));
                else if (this.Log)
                    Debug.Log(string.Format("<color=cyan>[RECV]</color> {0}\n{1}", e.WWW.text, e.WWW.url));
            };
            //this.PlatInfoFactory = () => new Pmd.PlatInfo()
            //{
            //    platid = Pmd.PlatType.PlatType_Normal,
            //    account = SystemInfo.deviceUniqueIdentifier,
            //};

            // 消息派发器初始化
            if (dispatcher == null)
            {
                this.dispatcher = new MessageDispatcher<object>();
                this.dispatcher.StaticRegister();
            }
            else
            {
                this.dispatcher = dispatcher;
            }
        }

        #region SendTo
        /// <summary>
        /// 适应于unilight服务器的底层消息发送
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="do">发送的消息类型，默认为<paramref name="data"/>的类型名</param>
        /// <returns></returns>
        protected WWW SendToHandle(string url, object data, string @do = null)
        {
            if (@do == null && data != null)
                @do = data.GetType().FullName;

            // 消息包装
            var cmd = new HttpPackage()
            {
                @do = @do,
                data = data,
                gameid = GameID,
                zoneid = ZoneID,
                uid = UID,
                sid = SID,
                unigame_plat_login = PlatToken,
                unigame_plat_timestamp = DateTime.Now.ToUnixTime(),
            };
            var json = Common.Json.Serialize(cmd);

            // 签名
            var query = new UriQuery();
            if (!string.IsNullOrEmpty(PlatKey))
            {
                string md5str = Common.MD5.ComputeHashString(json + cmd.unigame_plat_timestamp.ToString() + PlatKey);
                //Debug.LogError("json str is " + json);
                //Debug.LogError("unigame_plat_timestamp str is " + cmd.unigame_plat_timestamp.ToString());
                //Debug.LogError("PlatKey str is " + PlatKey);
                //Debug.LogError("md5 str is " + md5str);
                query["unigame_plat_sign"] = md5str;
            }
            if (Compress != HttpCompress.None)
                query["compress"] = Compress.ToString();
            if (query.Any())
                url += "?" + query.ToString();

            if (OnRequest != null)
                OnRequest(this, new HttpRequestEventArgs(url, json));

            // 数据压缩
            byte[] post = null;
            switch (Compress)
            {
                case HttpCompress.None:
                    post = Common.Encoding.GetBytes(json);
                    break;
                case HttpCompress.gzip:
                case HttpCompress.flate:
                case HttpCompress.zlib:
                case HttpCompress.lzw:
                default:
                    throw new NotSupportedException(Compress.ToString());
            }

            // 网络发送
            return new WWW(url, post, HeaderJson);
        }
        protected IEnumerator SendToHandle(Action<WWW> callback, string url, object data, string @do)
        {
            var www = SendToHandle(url, data, @do);
            yield return www;
            if (callback != null)
                callback(www);
        }
        /// <summary>
        /// 适应于unilight服务器的底层消息发送
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="callback"></param>
        protected void SendTo(string url, object data, Action<WWW> callback)
        {
            StartCoroutine(SendToHandle(www =>
            {
                if (OnResponse != null)
                    OnResponse(this, new HttpResponseEventArgs(www));
                if (callback != null)
                    callback(www);
            }, url, data, null));
        }
        #endregion

        #region LoginServer API
        /// <summary>
        /// LoginServer登陆，需要预先设置PlatInfo
        /// </summary>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public void Login(Action<Pmd.PlatTokenLoginReturn> success, Action<string> error = null)
        {
            var platinfo = PlatInfoFactory();
            if (platinfo == null)
            {
                var msg = "[WWW] ERROR PlatInfo is null";
                if (error == null) Debug.LogError(msg); else error(msg);
                return;
            }

            // 清除上次平台登录痕迹
            this.GatewayUrl = null;
            this.TimeZone = 0;
            this.ResetSession();

            SendTo(LoginUrl, new Pmd.PlatTokenLogin() { platinfo = platinfo }, (WWW www) =>
            {
                var recv = HttpPackageReturn<Pmd.PlatTokenLoginReturn>.Create(www, this);
                if (recv == null || recv.errno == Pmd.HttpReturnCode.HttpReturnCode_SignError)
                {
                    var msg = string.Format("[WWW] ERROR {0} {1}\n{2}", LoginUrl, www.error, www.text);
                    if (error == null) Debug.LogError(msg); else error(msg);
                    return;
                }

                this.TimeZone = recv.data.timezone_offset;

                // session更新
                this.UID = recv.data.uid;
                this.SID = recv.data.sid;
                this.PlatKey = recv.data.unigame_plat_key;
                this.PlatToken = recv.data.unigame_plat_login;
                this.PlatTokenTimeout = DateTime.Now + TimeSpan.FromSeconds(recv.data.unigame_plat_login_life);
                this.Serialize();

                if (success != null)
                    success(recv.data);
            });
        }

        /// <summary>
        /// 获取区列表
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public void RequestZoneList(Action<Pmd.ZoneInfoListLoginUserPmd_S> success, Action<string> error = null)
        {
            SendTo(LoginUrl, new Pmd.RequestZoneList() { }, (WWW www) =>
            {
                var recv = HttpPackageReturn<Pmd.ZoneInfoListLoginUserPmd_S>.Create(www, this);
                if (recv == null || recv.errno == Pmd.HttpReturnCode.HttpReturnCode_SignError)
                {
                    var msg = string.Format("[WWW] ERROR {0} {1}\n{2}", LoginUrl, www.error, www.text);
                    if (error == null) Debug.LogError(msg); else error(msg);
                    return;
                }
                Dispatch(recv.data, success);
            });
        }
        public void RequestLoginUserInfo(Action<Pmd.RequestUserZoneInfoLoginUserPmd_S> userInfo)
        {
            SendTo(LoginUrl, new Pmd.RequestUserZoneInfoLoginUserPmd_C() { gameid = GameID }, (WWW www) =>
                {
                    var recv = HttpPackageReturn<Pmd.RequestUserZoneInfoLoginUserPmd_S>.Create(www, this);
                    if (recv != null && userInfo != null)
                    {
                        userInfo(recv.data);
                    }
                });
        }
        public void SelectZone(Action success, Action<string> error = null)
        {
            this.GatewayUrl = null;
            SendTo(LoginUrl, new Pmd.RequestSelectZone() { }, (WWW www) =>
            {
                var recv = HttpPackageReturn<Pmd.RequestSelectZoneReturn>.Create(www, this);
                if (recv == null)
                {
                    var msg = string.Format("[WWW] ERROR {0} {1}\n{2}", LoginUrl, www.error, www.text);
                    if (error == null) Debug.LogError(msg); else error(msg);
                    return;
                }
                if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_SignError)
                {
                    Login(login => SelectZone(success, error), error);
                    return;
                }
                this.GatewayUrl = recv.data.gatewayurl;
                if (string.IsNullOrEmpty(recv.data.gatewayurl))
                {
                    Engine.Utility.Log.Error("select zone gatewayurl is null");
                }
                if (success != null)
                    success();
            });
        }
        #endregion

        /// <summary>
        /// 发送消息，可自动填充平台必要字段
        /// </summary>
        /// <param name="message"></param>
        /// <param name="success"></param>
        /// <returns></returns>
        protected void SendMessage<TResponse>(object message, Action<TResponse> success, Action<string> error)
        {
            // 确保平台登陆有效
            if (HasSession() == false)
            {
                Deserialize();
                if (HasSession() == false)
                {
                    Login(login => SendMessage(message, success, error), error);
                    return;
                }
            }

            // 选区以确保网关地址有效
            if (string.IsNullOrEmpty(GatewayUrl))
            {
                SelectZone(() => SendMessage(message, success, error), error);
                return;
            }

            // 向unilight逻辑服务器发送消息
            SendTo(GatewayUrl, message, (WWW www) =>
            {
                var recv = HttpPackageReturn<TResponse>.Create(www, this);
                if (recv == null)
                {
                    var msg = string.Format("[WWW] ERROR {0} {1}\n{2}", GatewayUrl, www.error, www.text);
                    if (error == null) Debug.LogError(msg); else error(msg);
                    return;
                }
                if (recv.errno == Pmd.HttpReturnCode.HttpReturnCode_SignError)
                {
                    Login(login => SendMessage(message, success, error), error);
                    return;
                }
                if (success != null)
                    success(recv.data);
            });
        }

        #region 消息分发
        private readonly GX.Net.MessageDispatcher<object> dispatcher;
        public GX.Net.MessageDispatcher<object> Dispatcher { get { return dispatcher; } }

        public bool Dispatch<TResponse>(TResponse message, Action<TResponse> callback)
        {
            if (message == null)
                return false;
            IEnumerator coroutine = null;
            if (callback != null)
            {
                callback(message);
            }
            else if (Dispatcher.Dispatch(message, out coroutine) == false)
            {
                Debug.LogWarning(string.Format("未处理的消息: {0}\n{1}", message.GetType(), message.Dump()));
                return false;
            }
            if (coroutine != null)
            {
                StartCoroutine(coroutine);
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 发送消息，并在必要时自动进行消息响应函数回调
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public void SendAsync<TResponse>(object request, Action<TResponse> success = null, Action<string> error = null)
        {
            SendMessage<TResponse>(request,
                response => Dispatch(response, success),
                msg => { if (error == null) Debug.LogError(msg); else error(msg); });
        }
        /// <summary>
        /// 发送消息，并在必要时自动进行消息响应函数回调
        /// </summary>
        /// <param name="request"></param>
        public void SendAsync(object request)
        {
            SendAsync<object>(request, null, null);
        }

        #region Session
        /// <summary>
        /// 可唯一代表一个用户身份的ID，由平台统一生成
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        /// 组合字段：<平台id>::<平台账户>
        /// </summary>
        public string SID { get; set; }
        /// <summary>
        /// 平台登录密钥，用于上行消息URL签名
        /// </summary>
        public string PlatKey { get; set; }
        /// <summary>
        /// 平台登录token，用于上行消息
        /// </summary>
        public string PlatToken { get; set; }
        /// <summary>
        /// 平台登录token的有效时间，过期后或服务器返回HttpReturnCode_SignError时客户端需要重新走登陆流程
        /// </summary>
        public DateTime PlatTokenTimeout { get; set; }

        public bool HasSession()
        {
            if (string.IsNullOrEmpty(this.UID) || string.IsNullOrEmpty(this.SID))
                return false;
            if (string.IsNullOrEmpty(this.PlatKey) || string.IsNullOrEmpty(PlatToken))
                return false;
            //if (DateTime.Now > this.PlatTokenTimeout)
            //	return false;
            return true;
        }

        public void ResetSession()
        {
            this.UID = null;
            this.SID = null;
            this.PlatKey = null;
            this.PlatToken = null;
            this.PlatTokenTimeout = DateTime.MinValue;
            Serialize();
        }

        private void Serialize()
        {
            PlayerPrefs.SetString("HTTP.UID", this.UID);
            PlayerPrefs.SetString("HTTP.SID", this.SID);
            PlayerPrefs.SetString("HTTP.PlatKey", this.PlatKey);
            PlayerPrefs.SetString("HTTP.PlatToken", this.PlatToken);
            PlayerPrefs.SetString("HTTP.PlatTokenTimeout", this.PlatTokenTimeout.ToBinary().ToString());
        }

        private void Deserialize()
        {
            this.UID = PlayerPrefs.GetString("HTTP.UID");
            this.SID = PlayerPrefs.GetString("HTTP.SID");
            this.PlatKey = PlayerPrefs.GetString("HTTP.PlatKey");
            this.PlatToken = PlayerPrefs.GetString("HTTP.PlatToken");
            long b = 0;
            this.PlatTokenTimeout = PlayerPrefs.GetString("HTTP.PlatTokenTimeout").TryParse(out b) ? DateTime.FromBinary(b) : DateTime.MinValue;
        }
        #endregion

        protected static Coroutine StartCoroutine(IEnumerator routine)
        {
            return SingletonHttp.Instance.StartCoroutine(routine);
        }
    }
    public class SingletonHttp : SingletonMono<SingletonHttp>
    {

    }
    #region class HttpClient2 [Obsolete]
    [Obsolete("Please use JsonHttp instead")]
    public partial class HttpClient2 : JsonHttp
    {
        public HttpClient2(string loginUrl, uint gameID, uint zoneID = 0)
            : base(loginUrl, gameID, zoneID)
        {
        }
    }
    #endregion
}


#region class HttpClient [Obsolete]
#if UNITY
namespace GX.Net
{
	[Obsolete("Please use JsonHttp instead")]
	public partial class HttpClient
	{
#region Cmd
#pragma warning disable 0649
		class HttpSend
		{
			public int gameid;
			public int zoneid;
			public string uid;

			public string @do;
			public object data;
		}

		class HttpRecv<T>
		{
			public int gameid;
			public int zoneid;
			public string uid;

			public string @do;
			public T data;

			public string errno;
			public uint st;

			public static HttpRecv<T> Create(WWW www)
			{
				if (www == null)
					return null;
				if (!string.IsNullOrEmpty(www.error))
					return null;

				HttpRecv<T> recv;
				try { recv = Json.Deserialize<HttpRecv<T>>(www.text); }
				catch { return null; }

				if (!string.IsNullOrEmpty(recv.errno) && recv.errno != "0")
					return null;

				return recv;
			}
		}

		class RegisterNewAccount
		{
			public const string Name = "register-newaccount";
			public string mid;
		}

		class RegisterNewAccountOK
		{
			public string mid;
			public string sid;
			public string uid;
		}

		class GetZoneGatewayUrl
		{
			public const string Name = "get-zone-gatewayurl";
			public int gameid;
			public int zoneid;
		}

		class GetZoneGatewayUrlOK
		{
			public int gameid;
			public int zoneid;
			public string gatewayurl;
		}
#pragma warning restore 0649
        #endregion

		public event EventHandler<HttpRequestEventArgs> OnRequest;
		public event EventHandler<HttpResponseEventArgs> OnResponse;

		public static readonly Dictionary<string, string> HeaderJson = new Dictionary<string, string>()
		{
			{"Content-Type", "application/json; charset=utf-8"},
		};

		public string UID { get; set; }
		public string SID { get; set; }

		public int GameID { get; set; }
		public int ZoneID { get; set; }

		public string LoginUrl { get; set; }
		public string GatewayUrl { get; private set; }

		public bool HasSession()
		{
			return !string.IsNullOrEmpty(this.UID) && !string.IsNullOrEmpty(this.SID);
		}

		public void ResetSession()
		{
			this.UID = null;
			this.SID = null;
			Serialize();
		}

		/// <summary>
		/// 适应于unilight服务器的底层消息发送
		/// </summary>
		/// <param name="url"></param>
		/// <param name="do"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		protected WWW SendTo(string url, string @do, object data)
		{
			var json = GX.Json.Serialize(new HttpSend()
			{
				@do = @do,
				data = data,
				uid = UID,
				gameid = GameID,
				zoneid = ZoneID,
			});
			if (!string.IsNullOrEmpty(SID))
				url += "?smd=md5&sign=" + GX.MD5.ComputeHashString(json + SID);
			if (OnRequest != null)
				OnRequest(this, new HttpRequestEventArgs(url, json));
			var www = new WWW(url, GX.Encoding.GetBytes(json), HeaderJson);
			return www;
		}

		/// <summary>
		/// 发送消息，可自动填充平台必要字段
		/// </summary>
		/// <param name="do"></param>
		/// <param name="message"></param>
		/// <param name="callback"></param>
		/// <returns></returns>
		protected IEnumerator SendMessage(string @do, object message, Action<WWW> callback)
		{
#region register-newaccount
			if (HasSession() == false)
			{
				Deserialize();
				if (HasSession() == false)
				{
					var www = SendTo(LoginUrl, RegisterNewAccount.Name, new RegisterNewAccount() { mid = SystemInfo.deviceUniqueIdentifier });
					yield return www;
					if (OnResponse != null)
						OnResponse(this, new HttpResponseEventArgs(www));
					var recv = HttpRecv<RegisterNewAccountOK>.Create(www);
					if (recv == null)
					{
						if (OnResponse == null)
							Debug.LogError(string.Format("[WWW] ERROR {0} {1}\n{2}", LoginUrl, www.error, www.text));
						yield break;
					}

					this.GameID = recv.gameid;
					this.ZoneID = recv.zoneid;

					this.UID = recv.data.uid;
					this.SID = recv.data.sid;
					this.Serialize();
				}
			}
            #endregion

#region get-zone-gatewayurl
			if (string.IsNullOrEmpty(GatewayUrl))
			{
				var www = SendTo(LoginUrl, GetZoneGatewayUrl.Name, new GetZoneGatewayUrl() { gameid = this.GameID, zoneid = this.ZoneID });
				yield return www;
				if (OnResponse != null)
					OnResponse(this, new HttpResponseEventArgs(www));
				var recv = HttpRecv<GetZoneGatewayUrlOK>.Create(www);
				if (recv == null)
				{
					if (OnResponse == null)
						Debug.LogError(string.Format("[WWW] ERROR {0} {1}\n{2}", LoginUrl, www.error, www.text));
					yield break;
				}

				this.GameID = recv.gameid;
				this.ZoneID = recv.zoneid;
				this.GatewayUrl = recv.data.gatewayurl;
			}
            #endregion

			{
				var www = SendTo(GatewayUrl, @do, message);
				yield return www;
				if (OnResponse != null)
					OnResponse(this, new HttpResponseEventArgs(www));
				if (!string.IsNullOrEmpty(www.error))
				{
					if (OnResponse == null)
						Debug.LogError(string.Format("[WWW] ERROR {0} {1}\n{2}", GatewayUrl, www.error, www.text));
					yield break;
				}
				if (callback != null)
					callback(www);
			}
		}

		public IEnumerator SendAsync(string action, Dictionary<string, object> message, Action<WWW> callback = null)
		{
			return SendMessage(action, message, callback);
		}

		private readonly GX.Net.MessageDispatcher<object> dispatcher = new Net.MessageDispatcher<object>();
		public GX.Net.MessageDispatcher<object> Dispatcher { get { return dispatcher; } }

		public IEnumerator SendAsync<TResponse>(object request, Func<TResponse, IEnumerator> callback = null)
		{
			return SendMessage(request.GetType().Name, request, www =>
			{
				var recv = HttpRecv<TResponse>.Create(www);
				if (recv == null)
				{
					Debug.LogError(string.Format("[HTTP RECV ERROR] {0}", www.text));
					return;
				}
				var message = recv.data;
				IEnumerator coroutine;
				if (callback != null)
				{
					coroutine = callback(recv.data);
				}
				else if (Dispatcher.Dispatch(message, out coroutine) == false)
				{
					Debug.LogWarning(string.Format("未处理的消息: {0}\n{1}", message.GetType(), message.Dump()));
					return;
				}
				if (coroutine != null)
				{
					Singleton.Instance.StartCoroutine(coroutine);
				}
			});
		}

		public void Send<TResponse>(object request, Func<TResponse, IEnumerator> callback)
		{
			Singleton.Instance.StartCoroutine(SendAsync<TResponse>(request, callback));
		}

		public void Send<TResponse>(object request, Action<TResponse> callback = null)
		{
			if (callback == null)
				Singleton.Instance.StartCoroutine(SendAsync<TResponse>(request, null));
			else
				Singleton.Instance.StartCoroutine(SendAsync<TResponse>(request, new Func<TResponse, IEnumerator>(response =>
				{
					callback(response);
					return null;
				})));
		}

#region Serialize
		private void Serialize()
		{
			PlayerPrefs.SetString("HTTP.UID", this.UID);
			PlayerPrefs.SetString("HTTP.SID", this.SID);
		}

		private void Deserialize()
		{
			this.UID = PlayerPrefs.GetString("HTTP.UID");
			this.SID = PlayerPrefs.GetString("HTTP.SID");
		}
        #endregion
	}
}
#endif
#endregion