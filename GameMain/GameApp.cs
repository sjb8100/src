using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using Engine;
using Engine.Utility;
using Client;
using Common;
using System;
public class GameApp : BaseApp
{
    #region define
    /// <summary>
    /// 游戏数据初始化类型
    /// </summary>
    public enum GameInitDataType
    {
        None,
        [Description("品质设置")]
        Quality,
        [Description("配置表路径设置")]
        Table,
        [Description("设置特效加载回调")]
        Effect,
        [Description("游戏环境")]
        GameEnvironment,
        [Description("剧情")]
        Story,
        [Description("实体消息缓存")]
        Entity,
        [Description("网络初始化")]
        Net,
        [Description("游戏逻辑初始化")]
        GameLogicData,
        [Description("生成数据管理器")]
        DataManager,
        [Description("SDK初始化")]
        SDK,
        [Description("GVoice")]
        GVoice,
    }

    /// <summary>
    /// 游戏初始化状态数据
    /// </summary>
    public class GameInitData
    {
        //数据初始化类型
        public GameInitDataType InitDataType = GameInitDataType.None;
        //数据初始化描述
        public string Des;
        //初始化调用动作
        public Action<Action<GameInitData, float>, GameInitData> CallMethod;
        //耗时比计数
        public int CostTimeCount = 0;
        private GameInitData()
        {

        }
        public static GameInitData Create(GameInitDataType initType, string des
            , Action<Action<GameInitData, float>, GameInitData> callmethod,int costTimeCount = 7)
        {
            GameInitData data = new GameInitData()
            {
                InitDataType = initType,
                Des = des,
                CallMethod = callmethod,
                CostTimeCount = costTimeCount,
            };
            return data;
        }
    }
    #endregion
    static GameApp s_Inst = null;
    //游戏初始化数据


    private List<GameInitData> m_lstGameInitDatas = null;
    /// <summary>
    /// 构建游戏初始化数据
    /// </summary>
    private void StructGameInitData(GameObject root)
    {
        if (null == m_lstGameInitDatas)
        {
            m_lstGameInitDatas = new List<GameInitData>();
        }
        m_lstGameInitDatas.Clear();

        //1、网络初始化
        GameInitData gameInitData = GameInitData.Create(GameInitDataType.Net, "网络初始化", (progressCallback, data) =>
        {
            //网络初始化
            NetService.Instance.Init();
            progressCallback.Invoke(data, 1);
        });
        m_lstGameInitDatas.Add(gameInitData);

        //2、品质设置

        gameInitData = GameInitData.Create(GameInitDataType.Quality,"品质设置",(progressCallback,data)=>
            {
                //品质设置
                QualitySettings.pixelLightCount = 3;
                QualitySettings.SetQualityLevel((int)QualityLevel.Good);
                progressCallback.Invoke(data, 1f);

                if (SystemInfo.systemMemorySize < 1024)
                {
                    QualitySettings.shadows = ShadowQuality.Disable;
                }
            },30);
        m_lstGameInitDatas.Add(gameInitData);




        //3、配置表设置
        gameInitData = GameInitData.Create(GameInitDataType.Table, "配置表设置", (progressCallback, data) =>
        {
            // 配置表路径设置 
            Table.SetDataPath("common/tab/");
            float totalSubProgress = 0;
            Action<float> subAction = (subProgress) =>
            {
                totalSubProgress = 0.8f * subProgress;
                progressCallback.Invoke(data, totalSubProgress);
            };
            // 加载配置表
            GameTableManager.Instance.LoadTable(subAction);
            GameTableManager.Instance.LoadGloabalConst();
            progressCallback.Invoke(data, 1);
        }, 50);
        m_lstGameInitDatas.Add(gameInitData);



        //5、游戏环境初始化
        gameInitData = GameInitData.Create(GameInitDataType.GameEnvironment, "游戏环境初始化", (progressCallback, data) =>
        {
            // 设置特效加载回调
            EffectLoad effectLoad = new EffectLoad();
            Engine.RareEngine.Instance().SetEffectLoad(effectLoad);
            // 镜头bloom效果
            CameraBloomEffect cameraBloom = new CameraBloomEffect();
            Engine.RareEngine.Instance().SetCameraBloom(cameraBloom);

            progressCallback.Invoke(data, 0.2f);
            //剧情
            SequencerManager.Instance().Init();
            AppleSceneMaterial.Instance();

            progressCallback.Invoke(data, 0.4f);
            // 实体消息缓存
            EntityCreator.Instance().Init();
            progressCallback.Invoke(data, 0.6f);
            // 游戏逻辑初始化
            ClientGlobal.Instance().Init();
            progressCallback.Invoke(data, 0.8f);
            ////生成数据管理器
            //DataManager.Instance.Create();
            //TipsManager.Instance.Initialize();


            progressCallback.Invoke(data, 1);
        },20);
        m_lstGameInitDatas.Add(gameInitData);

        //6、本地数据初始化
        gameInitData = GameInitData.Create(GameInitDataType.DataManager, "本地数据初始化", (progressCallback, data) =>
        {
            //本地数据初始化
            float totalSubProgress = 0;
            Action<float> subAction = (subProgress) =>
            {
                totalSubProgress = 0.99f * subProgress;
                progressCallback.Invoke(data, totalSubProgress);
            };
            DataManager.Instance.Create(subAction);
            TipsManager.Instance.Initialize();
            
            progressCallback.Invoke(data, 1);
        }, 30);
        m_lstGameInitDatas.Add(gameInitData);

        //4、GVoice初始化
        gameInitData = GameInitData.Create(GameInitDataType.GVoice, "GVoice初始化", (progressCallback, data) =>
        {
            //GVoice初始化
            GVoiceManger.Instance.InitGvoice();
            progressCallback.Invoke(data, 1);
        });
        m_lstGameInitDatas.Add(gameInitData);
    }

    public static GameApp Instance()
    {
        if (null == s_Inst)
        {
            s_Inst = new GameApp();
        }

        return s_Inst;
    }

    

    private IPlayer m_mainPlayer = null;
    private CameraFollow m_CameraCtrl = null;

    private bool m_bLoadLog = false;
    private float m_fLastUpdateTime = 0.0f;

    //private void LoadRemoteFileFinish(WWW www, object param = null)
    //{
    //    if (www.error != null || www.text == string.Empty || www.text.Length == 0)
    //    {
    //        //if (m_callBackRemote != null)
    //        //{
    //        //    m_callBackRemote(UpgradeError.UpgradeError_DownloadFileError, 0, 0, www.url);
    //        //}
    //        return;
    //    }

    //    Engine.Utility.Log.Trace("Read file www time:{0}", Time.realtimeSinceStartup - m_fCurTime);
    //}
    /**
    @brief 初始化
     @param strRootName 根节点名称
    */
    override public void Init(ref GameObject root)
    {
        // 游戏引擎底层初始化
        base.Init(ref root);
        
  
        // 设定最大帧率
        if (!Application.isEditor)
        {
            Application.targetFrameRate = 30;
        }

        GameObject mainObj = root;
        //初始化携程管理器
        CoroutineMgr.Instance.Initialize(root);
        //构造游戏初始化数据
        StructGameInitData(mainObj);

        //初始化游戏配置
        DoDataInit((startdata) =>
            {
                if (null != LaunchPanel.Me)
                {
                    LaunchPanel.Me.Tips = Util.GetText(startdata.Des);
                }
            }, (completeData,progress) =>
            {
                LaunchPanel.Me.Progress =  (0.7f) * progress;
                if (1 - progress <= Mathf.Epsilon)
                {
                    Action<float> uiPreLoadCallback = (uiprogress) =>
                        {
                            LaunchPanel.Me.Progress = 0.7f * progress + 0.3f * uiprogress;
                            if (1 - uiprogress <= Mathf.Epsilon)
                            {
                                //执行游戏进程
                                DoGameStep(mainObj);
                            }
                        };
                    //预加载UI
                    DataManager.Manager<UIManager>().ChangeUISceneState(UIManager.UISceneState.USS_Login, uiPreLoadCallback);
                }
            });

        // 设置显示FPS
        if(GlobalConfig.Instance().FPS)
        {
            ShowFPS(true);
        }


        //注册低内存回调
        ResgisterLowMemory(OnLowMemeory);

        KHttpDown.Instance();

        return;
        // 创建场景
//         IMapSystem mapSystem = ClientGlobal.Instance().GetMapSystem();
//         if(mapSystem!=null)
//         {
//             mapSystem.EnterMap(0);
//         }

                
        // 创建主角
        //string strPlayerRes = "Character/female.obj";
        //IEntitySystem es = ClientGlobal.Instance().GetEntitySystem();
        //if (es != null)
        //{
        //    Client.EntityCreateData data = new Client.EntityCreateData();
        //    data.strName = strPlayerRes;
        //    data.PropList = new Client.EntityAttr[(int)Client.PlayerProp.End - (int)Client.EntityProp.Begin];
        //    m_mainPlayer = es.CreateEntity(Client.EntityType.EntityType_Player, data) as IPlayer;
        //    if (m_mainPlayer != null)
        //    {
        //        m_mainPlayer.AddCommpent(EntityCommpent.EntityCommpent_Visual);
        //        m_mainPlayer.AddCommpent(EntityCommpent.EntityCommpent_Move);

        //        Vector3 pos = Vector3.zero;
        //        m_mainPlayer.SendMessage(EntityMessage.EntityCommand_SetPos, pos);
        //        ClientGlobal.Instance().MainPlayer = m_mainPlayer;
        //    }
        //}

        //m_CameraCtrl = new CameraFollow();
        //m_CameraCtrl.camera = cam;
        //m_CameraCtrl.target = m_mainPlayer;
        //m_CameraCtrl.SetCameraOffset(30.0f, 45.0f, 25);
        //cam.SetCameraCtrl(m_CameraCtrl);

        // 创建特效
        //string strEffect = "Effect/jizhong_ruiji.obj";
        //IRenderObj effect = renderSys.CreateRenderObj(ref strEffect);
        //effect.GetNode().SetLocalPosition(new Vector3(0, 0, 0));

        //UISystem.UIManager.Instance().Init();

        //NGUITools.GetUIRoot();

        ShowFPS(true);

        // 执行Lua
        //Client.ILuaSystem luaSystem = ClientGlobal.Instance().GetLuaSystem();
        //if (luaSystem != null)
        //{
        //    string strLuaMain = "Lua/Main.lua";
        //    LuaBinder.Bind(luaSystem.GetLuaState());
        //    luaSystem.DoFile(ref strLuaMain);
        //}

        /////////////////////////////////////////////////////////////////////////////////////////
        // 以下为测试代码，可以注释掉 不要删除，主要是示例矩阵和矢量的一些用法
        // 测试代码
        // Unity使用左手系
        Quaternion rotate = new Quaternion();
        rotate.eulerAngles = new Vector3(0, 90, 0);
        Matrix4x4 mat = new Matrix4x4();
        mat.SetTRS(Vector3.zero, rotate, Vector3.one);

        // 获取矩阵中的轴向量
        Quaternion rot = new Quaternion();
        Vector3 right = mat.GetColumn(0);   // X
        Vector4 up = mat.GetColumn(1);      // Y
        Vector4 look = mat.GetColumn(2);    // Z
        rot.SetLookRotation(look, up);

        // 示例：旋转向量
        right = rotate * right;
        /////////////////////////////////////////////////////////////////////
    }

    
    /// <summary>
    /// 执行数据加载
    /// </summary>
    /// <param name="startCallBack">开始配置回调</param>
    /// <param name="completeCallback">配置完成回调</param>
    private void DoDataInit(Action<GameInitData> startCallBack,Action<GameInitData,float> completeCallback)
    {
        if (null == m_lstGameInitDatas)
        {
            return;
        }

        int totalCount = 0;
        Dictionary<GameInitDataType, int> initTimeCostCount = new Dictionary<GameInitDataType,int>();
        for(int i =0 ,max = m_lstGameInitDatas.Count;i < max;i++)
        {
            totalCount += m_lstGameInitDatas[i].CostTimeCount;
            if (!initTimeCostCount.ContainsKey(m_lstGameInitDatas[i].InitDataType))
            {
                initTimeCostCount.Add(m_lstGameInitDatas[i].InitDataType, 0);
            }
        }
        int complteCount = 0;
        // Log异常函数
        Engine.Utility.Log.logException = OnLogException;

        Action<GameInitData,float> onceComplteCallback = (initData,progress) =>
            {
                int tempCount = (int)(initData.CostTimeCount * progress);

                complteCount -= initTimeCostCount[initData.InitDataType];
                initTimeCostCount[initData.InitDataType] = tempCount;
                complteCount += tempCount;

                float totalProgress = complteCount / (float)totalCount;
                if (null != completeCallback)
                {
                    completeCallback.Invoke(initData, totalProgress);
                }
            };
        //异步加载
        DoDataInitProccess(m_lstGameInitDatas, startCallBack, onceComplteCallback);
    }

    /// <summary>
    /// 执行数据加载进程
    /// </summary>
    /// <param name="datas"></param>
    /// <param name="startCallBack"></param>
    /// <param name="completeCallback"></param>
    private void DoDataInitProccess(List<GameInitData> datas
        ,Action<GameInitData> startCallBack
        ,Action<GameInitData,float> completeCallback)
    {
        if (null != datas && datas.Count > 0)
        {
            GameInitData initData = datas[0];
            Action<GameInitData,float> onComplete = (data,progress) =>
            {
                if (null != completeCallback)
                {
                    completeCallback.Invoke(data, progress);
                }
                if (1 - progress <= Mathf.Epsilon)
                {
                    datas.RemoveAt(0);
                    CoroutineMgr.Instance.DelayInvokeMethod(0.1f, () =>
                        {
                            DoDataInitProccess(datas, startCallBack, completeCallback);
                        });
                }
            };
            //开启数据配置协成
            CoroutineMgr.Instance.StartCorountine(InitGameDataCor(initData,startCallBack, onComplete));
        }
    }

    /// <summary>
    /// 执行配置数据协成
    /// </summary>
    /// <param name="initData"></param>
    /// <param name="complte"></param>
    /// <returns></returns>
    private System.Collections.IEnumerator InitGameDataCor(GameInitData initData,Action<GameInitData> startCallBack,Action<GameInitData,float> complte)
    {
        yield return new WaitForEndOfFrame();
        if (null != startCallBack)
        {
            startCallBack.Invoke(initData);
        }
        if (null != initData.CallMethod)
        {
            initData.CallMethod.Invoke(complte,initData);
        }
        else
        {
            complte.Invoke(initData, 1f);
        }
        yield break;
    }


    /// <summary>
    ///数据加载初始化完毕开始游戏进程
    /// </summary>
    private void DoGameStep(GameObject root)
    {
        //初始化加载UI界面 
        root.AddComponent<StepManager>();

        m_fLastUpdateTime = Time.realtimeSinceStartup;

        IControllerSystem cs = ClientGlobal.Instance().GetControllerSystem();
        //UnityEngine.Assertions.Assert.IsNull(cs, "ControllerSystem is null");
        if (cs != null)
        {
            cs.SetControllerHelper(new ControllerHelper());
        }
        Engine.Utility.Log.Info("GameApp -> Init");

        CreateMainCamera(false);

        TestGetSysInfo();
    }

    public void CreateMainCamera(bool active)
    {
        // 逻辑层初始化
        IRenderSystem renderSys = RareEngine.Instance().GetRenderSystem();
        if (renderSys == null)
        {
            Log.Error("RenderSystem is null!");
            // return;
        }

        // 设置主Camera
        string strCameraName = "MainCamera";
        ICamera cam = renderSys.GetCamera(ref strCameraName);
        if (cam != null)
        {
            cam.Enable(active);
            return;
        }

        cam = renderSys.CreateCamera(ref strCameraName, 0.3f, 140f, 45f, -1, CameraClearFlags.Skybox);
        if (cam != null)
        {

            Camera camera = cam.GetNode().GetTransForm().gameObject.GetComponent<Camera>();
            AudioListener listener = camera.transform.GetComponent<AudioListener>();
            if (listener != null)
            {
                GameObject.Destroy(listener);
            }
            CameraFollow.Instance.camera = cam;
            Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
            if (audio != null)
            {
                audio.SetListener(CameraFollow.Instance.CreateAudioListener());
            }

            camera.cullingMask = -1 - (1 << LayerMask.NameToLayer("UI")) - (1 << LayerMask.NameToLayer("ShowModel")) - (1 << LayerMask.NameToLayer("UIHide"));
            camera.tag = strCameraName;
            cam.SetDepth((float)Engine.CameraDepth.Scene);
            cam.LookAt(new Vector3(0, 3, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            UnityEngine.MonoBehaviour.DontDestroyOnLoad(camera.gameObject);
            cam.Enable(active);
        }
    }

    void OnLogException()
    {
        // 处理log显示
        if (Application.isEditor)
        {
            UnityEngine.Debug.Break(); // 中断游戏
        }
    }

    // 预update
    override public void PreUpdate(float dt)
    {
        base.PreUpdate(dt);

        // 添加逻辑层预更新
    }

    /**
    @brief 更新
    @param 
    */
    override public void Update(float dt)
    {
        KDownloadInstance.Instance().Update();
        // 限帧
        //float fCurTime = Time.realtimeSinceStartup;
        //if (fCurTime - m_fLastUpdateTime < 0.1f)
        //{
        //    return;
        //}
        //m_fLastUpdateTime = fCurTime;

        //Profiler.BeginSample("ClientGlobal.Update");
        /// 逻辑层更新
        ClientGlobal.Instance().Update(dt);
        //Profiler.EndSample();

        FirstFightMgr.Instance().FrameMove(dt);


        //Profiler.BeginSample("DataManager.Update");
        //管理器Process
        DataManager.Instance.Process(dt);
        //Profiler.EndSample();

        //卸载资源
        ProcessUnloadUnuseAsset();

        base.Update(dt);
 //       Profiler.EndSample();
    }

    /// <summary>
    /// 延迟更新
    /// </summary>
    override public void LateUpdate(float dt)
    {
        ClientGlobal.Instance().LateUpdate(dt);
        base.LateUpdate(dt);
    }
// 
//     // Update is called once per frame
//     /// <summary>
//     ///  固定帧率的Update
//     /// </summary>
//     override public void FixedUpdate(float dt)
//     {
//         ClientGlobal.Instance().Update(dt);
//         base.FixedUpdate(dt);
// 
//     }

    // 热键消息回调
    public override bool OnMessage(MessageCode code, object param1 = null, object param2 = null, object param3 = null)
    {
        if (!UICamera.isOverUI) // UI消息 如果是UI上的消息直接忽略
        {
            IControllerSystem ctrlSys = ClientGlobal.Instance().GetControllerSystem();
            if (ctrlSys != null)
            {
                ctrlSys.OnMessage(code, param1, param2, param3);
            }

            // 镜头按键处理
            CameraFollow.Instance.OnMessage(code, param1, param2, param3);
        }

        if (code == MessageCode.MessageCode_Begin)
        {
            Vector3 pos = new Vector3((float)param1, (float)param2, 0);
            if (DataManager.Instance.Ready)
            {
                DataManager.Manager<UIPanelManager>().SendMsg(PanelID.MainPanel, UIMsgID.eShortcutRect, pos);
            }
          
        }

        return false;
    }

    /**
    @brief 释放
    @param 
    */
    override public void Release()
    {
        //KHttpDown.Instance().Release();
        KDownloadInstance.Instance().Quit();

        EntityCreator.Instance().Destroy();
        /// 释放资源
        ClientGlobal.Instance().Release();

        // 网络关闭
        Protocol.Instance.CloseHeartMsg();

        // Log关闭
        Log.Close();
        /// 逻辑层释放
        base.Release();
    }

    //-----------------------------------------------------------------------------------------------

    void TestGetSysInfo()
    {
        GetCupName();
        GetCpuNumCores();
        GetMaxCpuFreq();
        GetRamMemory();
    }
    public string GetCupName()
    {
        string name = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            //Debug.LogError("GameApp GetCupName:");
            AndroidJavaClass jc = new AndroidJavaClass("com.hxll.sysinfo.SysInfo");
            if (jc != null)
            {
                name = jc.CallStatic<string>("getCpuName");
            }
            //Debug.LogError("GameApp GetCupName ret :" + name);
        }

        return name;
    }



    
    public int GetCpuNumCores()
    {
        int cores = 0;
        if (Application.platform == RuntimePlatform.Android)
        {
            //Debug.LogError("GameApp GetCpuNumCores:");
            AndroidJavaClass jc = new AndroidJavaClass("com.hxll.sysinfo.SysInfo");
            if (jc != null)
            {
                cores = jc.CallStatic<int>("getNumCores");
            }
            //Debug.LogError("GameApp GetCpuNumCores ret :" + cores);
        }

        return 0;
    }

    //获取CPU最大频率
    public string GetMaxCpuFreq() 
    {
        string ret = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            //Debug.LogError("GameApp GetMaxCpuFreq:");
            AndroidJavaClass jc = new AndroidJavaClass("com.hxll.sysinfo.SysInfo");
            if (jc != null)
            {
                ret = jc.CallStatic<string>("getMinCpuFreq");
            }
            //Debug.LogError("GameApp GetMaxCpuFreq ret :" + ret);
        }

        return ret;
    }

    public long GetRamMemory()
    {
        long mem = 0;
        if (Application.platform == RuntimePlatform.Android)
        {
            //Debug.LogError("GameApp GetRamMemory:");
            AndroidJavaClass jc = new AndroidJavaClass("com.hxll.sysinfo.SysInfo");
            if (jc != null)
            {
                mem = jc.CallStatic<long>("getRamMemory");
            }
            //Debug.LogError("GameApp GetRamMemory ret :" + mem);
        }

        return mem;
    }

    #region LowMemery(低内存警告)
    //游戏自动回收资源间隔
    private const float UNLOAD_UNUSE_ASSET_GAP_TIME = 5*60;//5分钟
    //最近一次执行Unload时间
    private float m_unloadSinceLast = 0;
    //回收无用资源标记是否可用
    private bool m_bunloadUnuseAssetMaskEnable = false;
    //卸载资源异步状态
    private UnityEngine.AsyncOperation uloadResAsyn = null;

    public static float GameTimeStamp
    {
        get
        {
            return Time.time;
        }
    }
    /// <summary>
    /// 是否正在释放资源
    /// </summary>
    /// <returns></returns>
    public bool IsUnloadUnusedAsset()
    {
        return null != uloadResAsyn && !uloadResAsyn.isDone;
    }

    /// <summary>
    /// 卸载没有使用资源是否准备好了
    /// </summary>
    /// <returns></returns>
    public bool IsUnloadUnusedAssetReady()
    {
        return GameTimeStamp - m_unloadSinceLast >= UNLOAD_UNUSE_ASSET_GAP_TIME;
    }

    /// <summary>
    /// 执行卸载
    /// </summary>
    private void DoUnloadUnusedAsset()
    {
        if (!IsUnloadUnusedAsset())
        {
            uloadResAsyn = Resources.UnloadUnusedAssets();
            m_unloadSinceLast = GameTimeStamp;
            //Engine.Utility.Log.Info("GameApp->DoUnloadUnusedAsset Time:{0}" ,GameTimeStamp);
        }
        m_bunloadUnuseAssetMaskEnable = false;
    }

    /// <summary>
    /// update检测卸载资源
    /// </summary>
    private void ProcessUnloadUnuseAsset()
    {
        if (IsUnloadUnusedAssetReady() && m_bunloadUnuseAssetMaskEnable)
        {
            DoUnloadUnusedAsset();
        }
    }

    /// <summary>
    /// 释放未使用资源
    /// </summary>
    /// <param name="forceUnload">强制立即执行Unload</param>
    /// <param name="forceGc">执行GC</param>
    public void UnloadUnusedAsset(bool forceUnload = false,bool forceGc = false)
    {
        if (!IsUnloadUnusedAsset())
        {
            if (forceUnload)
            {
                DoUnloadUnusedAsset();
            }
            else
            {
                m_bunloadUnuseAssetMaskEnable = true;
            }
        }
       
        if (forceGc)
        {
            GC.Collect();
        }
    }


    public void OnLowMemoryWarning()
    {
        OnLowMemeory();
    }
    
    /// <summary>
    /// 低内存
    /// </summary>
    private void OnLowMemeory()
    {
        //RareEngine.Instance().OnLowMemoryWarning();
        //EventEngine.Instance().DispatchEvent((int)GameEventID.APPLICATION_LOWMEMORY);
        //UnloadUnusedAsset();
    }

    /// <summary>
    /// 注册低内存警告
    /// </summary>
    /// <param name="callback"></param>
    public static void ResgisterLowMemory(Application.LowMemoryCallback callback)
    {
        //先移除
        Application.lowMemory -= callback;
        //添加
        Application.lowMemory += callback;
    }

    /// <summary>
    /// 解除注册低内存警告
    /// </summary>
    /// <param name="callback"></param>
    public static void UnRegisterLowMemoy(Application.LowMemoryCallback callback)
    {
        Application.lowMemory -= callback;
    }
    #endregion
}
