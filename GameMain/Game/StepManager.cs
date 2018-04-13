using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Engine.Utility;
using System;
using UnityEngine.SceneManagement;
using Engine;
using DG.Tweening;
//游戏逻辑步骤
public enum Step
{
    NULL = 0,
    LOGIN = 1,
    LOAD = 2,
    SCENE_READY = 4,
    UI_READY = 8,
    DATA_READY = 64,
    CREATE_ROLE = 128,//创建角色
    GAME_READY = SCENE_READY | UI_READY | DATA_READY,
}

public class StepManager : MonoBehaviour
{
    static StepManager ms_instance;
    public static StepManager Instance
    {
        get { return ms_instance; }
    }

    Step m_step;
    public Step CurrentStep
    {
        get { return m_step; }
    }

    public enum IS_NEW_ROLE_LOGIN
    {
        NULL = 0,
        CREATE = 1,
        LOGIN = 2,

        CREATE_LOGIN = CREATE | LOGIN,
    };
    public IS_NEW_ROLE_LOGIN m_bIsNewRoleLogin = IS_NEW_ROLE_LOGIN.NULL;
    
    //首次登录吗
    bool m_firstLogin = true;
    // 状态标识码
    private uint m_uStepFlag = 0;

    //基础ab资源，加载之后就不用做unload
    static List<AssetBundle> m_common_asset_list = new List<AssetBundle>();

    Transform m_panel_root;
    void Awake()
    {
        ms_instance = this;

        DontDestroyOnLoad(Util.UIRoot);
        m_panel_root = Util.UICameraObj.transform;

        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE, OnSceneEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, OnSceneEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, OnSceneEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.SYSTEM_LOADUICOMPELETE, OnSceneEvent);
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.LOGINSUCESSANDRECEIVEALLINFO, OnSceneEvent);

    }

    IEnumerator BatchingScene(bool bFirst = false)
    {
        yield return new WaitForEndOfFrame();

        DataManager.Manager<UIPanelManager>().HidePanel(PanelID.LoadingPanel);

        if (Client.ClientGlobal.Instance().MainPlayer.IsDead())
        {
            DataManager.Manager<UIPanelManager>().HideMain();
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ReLivePanel);
        }
        else
        {
            DataManager.Manager<UIPanelManager>().ShowMain(bFirst);
        }

        yield return null;
    }

    void Start()
    {
        AddLoginScene(LOGINSCENE, UnityEngine.SceneManagement.LoadSceneMode.Additive, (obj) =>
        {
            StepManager.Instance.OnBeginStep(Step.LOGIN);
        });
    }


    private System.Collections.IEnumerator SetHeartBeat()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Protocol.Instance.SetHeartBeat(m_uSaveHeartTime);
    }


    void PlayAudio(GameObject go, string path)
    {
        Engine.IAudio audio = Engine.RareEngine.Instance().GetAudio();
        if (audio == null)
        {
            return;
        }
        audio.PlayEffect(go, path);
    }

    private void MovieEnd_Impl()
    {
        string strCameraName = "MainCamera";
        Engine.ICamera cam = Engine.RareEngine.Instance().GetRenderSystem().GetCamera(ref strCameraName);
        if (cam == null)
        {
            return;
        }

        ///rotate
        cam.GetNode().GetTransForm().DORotate(new Vector3(38f, 45f, 0f), 3);

        ///fieldofview
        float fieldOfView = 30;
        Tween t = DOTween.To(() => fieldOfView, x => fieldOfView = x, 45, 3);
        // 给执行 t 变化时，每帧回调一次 UpdateTween 方法
        t.OnUpdate(() => UpdateTween(fieldOfView));

        /// fog
        float fFogStartDistance = 60;
        float fFogEndDistance = 250;
        RenderSettings.fogStartDistance = fFogStartDistance;
        RenderSettings.fogEndDistance = fFogEndDistance;

        Tween t1 = DOTween.To(() => fFogStartDistance, x => fFogStartDistance = x, 20, 3);
        t1.OnUpdate(() => UpdateFogStartDistance(fFogStartDistance));

        Tween t2 = DOTween.To(() => fFogEndDistance, x => fFogEndDistance = x, 170, 3);
        t2.OnUpdate(() => UpdateFogEndDistance(fFogEndDistance));

        // move
        Tweener tweener = cam.GetNode().GetTransForm().DOMove(newPos, 3f);

        //Audio
        table.ResourceDataBase rdb = GameTableManager.Instance.GetTableItem<table.ResourceDataBase>(45001);
        if (rdb != null)
        {
            PlayAudio(this.gameObject, rdb.strPath);
        }
        //

        Client.IPlayer pPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (pPlayer == null)
            return;
        tweener.OnComplete(() =>
        {
            CameraFollow.Instance.camera = cam;
            pPlayer = Client.ClientGlobal.Instance().MainPlayer;
            CameraFollow.Instance.target = pPlayer;
            cam.SetCameraCtrl(CameraFollow.Instance);

            cam.SetFarClipPlane(140);

            if (post != null)
            {
                GameObject.DestroyImmediate(post);
                post = null;
            }

            UIManager.Instance.SetCameraState(true);


            Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
            if (cs != null)
            {
                cs.GetActiveCtrl().SetHost(pPlayer);
            }

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.CAMERA_MOVE_END, null);
        });

    }
    Vector3 newPos;
    private void MovieEnd()
    {
        string strCameraName = "MainCamera";
        Engine.ICamera cam = Engine.RareEngine.Instance().GetRenderSystem().GetCamera(ref strCameraName);
        if (cam == null)
        {
            return;
        }

        CameraFollow.Instance.camera = null;

        Vector3 savePos = new Vector3(-28.52f, 97.2f, -121.51f);

        Client.IPlayer pPlayer = Client.ClientGlobal.Instance().MainPlayer;
        if (pPlayer == null)
            return;

        newPos = cam.GetNode().GetWorldPosition();
        newPos.y = 49.36f;

        cam.GetNode().GetTransForm().position = savePos;

        //旋转
        Vector3 rotationVector3 = new Vector3(34.923f, 52.184f, 1.782f);
        Quaternion rotation = Quaternion.Euler(rotationVector3);
        cam.GetNode().GetTransForm().rotation = rotation;

        cam.SetFieldOfView(30);

        RenderSettings.fogStartDistance = 60f;
        RenderSettings.fogEndDistance = 250f;

        // 3 秒内 旋转角度 到(38f, 45f, 0f)
        //cam.GetNode().GetTransForm().DORotate(new Vector3(38f, 45f, 0f), 3);

        //float fieldOfView = 30;
        //Tween t = DOTween.To(() => fieldOfView, x => fieldOfView = x, 45, 3);
        //// 给执行 t 变化时，每帧回调一次 UpdateTween 方法
        //t.OnUpdate(() => UpdateTween(fieldOfView));


        //float fFogStartDistance = 60;
        //float fFogEndDistance = 250;
        //RenderSettings.fogStartDistance = fFogStartDistance;
        //RenderSettings.fogEndDistance = fFogEndDistance;
     
        //Tween t1 = DOTween.To(() => fFogStartDistance, x => fFogStartDistance = x, 20, 3);
        //t1.OnUpdate(() => UpdateFogStartDistance(fFogStartDistance));

        //Tween t2 = DOTween.To(() => fFogEndDistance, x => fFogEndDistance = x, 170, 3);
        //t2.OnUpdate(() => UpdateFogEndDistance(fFogEndDistance));


        //Tweener tweener = cam.GetNode().GetTransForm().DOMove(newPos, 3f);

        UIManager.Instance.SetCameraState(false);


        Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
        if (cs != null)
        {
            cs.GetActiveCtrl().SetHost(null);
        }

        Invoke("MovieEnd_Impl", 2f);


    }

    private void UpdateTween(float num)
    {
        string strCameraName = "MainCamera";
        Engine.ICamera cam = Engine.RareEngine.Instance().GetRenderSystem().GetCamera(ref strCameraName);
        if (cam == null)
        {
            return;
        }

        cam.SetFieldOfView(num);

    }

    private void UpdateFogStartDistance(float f)
    {
        RenderSettings.fogStartDistance = f;
     
    }

    private void UpdateFogEndDistance(float f)
    {
        RenderSettings.fogEndDistance = f;
    }


    private void PlayMovie()
    {
        string strPath;
        if (Application.platform == RuntimePlatform.Android)
        {
            strPath = System.IO.Path.Combine(Application.persistentDataPath, "assets/video/openvideo.mp4");
            if (!System.IO.File.Exists(strPath))
            {
                strPath = System.IO.Path.Combine(Application.streamingAssetsPath, "video/openvideo.mp4");
            }
            Handheld.PlayFullScreenMovie(strPath, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.Fill);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            //strPath = System.IO.Path.Combine(Application.persistentDataPath, "video/openvideo.mp4");
            //if (!System.IO.File.Exists(strPath))
            //{
                //strPath = System.IO.Path.Combine(Application.streamingAssetsPath, "video/openvideo.mp4");
            //}
            Handheld.PlayFullScreenMovie("video/openvideo.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.Fill);
        }
        else
        {

        }
        

        StartCoroutine(SetHeartBeat());

        MovieEnd();
    }

    private uint m_uSaveHeartTime = 1000000;
    private PostRender.K3DPostRenderRadialBlur post = null;

    private void PlayMovie_Start()
    {
        OfflineManager omgr = DataManager.Manager<OfflineManager>();
        omgr.mShowAdvertise = false;

        //设置相机参数
        string strCameraName = "MainCamera";
        Engine.ICamera cam = Engine.RareEngine.Instance().GetRenderSystem().GetCamera(ref strCameraName);
        if (cam == null)
        {
            return;
        }
        cam.SetFarClipPlane(250);


        if (cam.GetNode().GetTransForm().gameObject.GetComponent<PostRender.K3DPostRenderRadialBlur>() == null)
        {
            post = cam.GetNode().GetTransForm().gameObject.AddComponent<PostRender.K3DPostRenderRadialBlur>();
        }

        m_uSaveHeartTime = Protocol.Instance.SetHeartBeat(4000000);
    }

    private void PlayMovie_Mobile(FullScreenMovieControlMode type)
    {
        PlayMovie();
    }


    private void PlayMovie_PC()
    {
        //MovieTexture movTexture = null;
        //movTexture = (MovieTexture)UnityEngine.Resources.Load("openvideo", typeof(MovieTexture));
        //movTexture.Play();
    }

    private void ShowUINotfy()
    {
        Client.IMapSystem ms = Client.ClientGlobal.Instance().GetMapSystem();

        if (ms != null)
        {
            if (ms.IsFirstLoad())
            {
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.advertisePanel);
            }
        }


    }
    void OnGameReady()
    {
        if (m_uStepFlag == (uint)Step.GAME_READY)
        {
            if (StepManager.Instance.m_bIsNewRoleLogin == IS_NEW_ROLE_LOGIN.CREATE_LOGIN)
            {
                if (Application.isEditor)
                {
                    StepManager.Instance.m_bIsNewRoleLogin = IS_NEW_ROLE_LOGIN.NULL;

                    PlayMovie_Start();
                    MovieEnd();

                }
                else
                {
                    StepManager.Instance.m_bIsNewRoleLogin = IS_NEW_ROLE_LOGIN.NULL;

                    PlayMovie_Start();
                    PlayMovie_Mobile(FullScreenMovieControlMode.CancelOnInput);
                }
            }

            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.SYSTEM_GAME_READY, null);
            m_uStepFlag = 0;

        }
    }


    public void Clear()
    {
        m_uStepFlag = 0;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    void OnSceneEvent(int id, object p)
    {
        if (id == (int)Client.GameEventID.SYSTEM_LOADSCENECOMPELETE)
        {
            Client.stLoadSceneComplete loadScene = (Client.stLoadSceneComplete)p;

            DataManager.Manager<UIPanelManager>().SendMsg(PanelID.LoadingPanel, UIMsgID.eLoadingTips, "马上进入场景");

            if (Client.ClientGlobal.Instance().MainPlayer == null)
            {
                return;
            }

            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.LoadingPanel);

            if (Client.ClientGlobal.Instance().MainPlayer.IsDead())
            {
                DataManager.Manager<UIPanelManager>().HideMain();
                DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ReLivePanel);
            }
            else
            {
                DataManager.Manager<UIPanelManager>().ShowMain(loadScene.bFirstLoadScene);
            }


            if (loadScene.bFirstLoadScene)
            {
                m_uStepFlag |= (uint)Step.SCENE_READY;
                Engine.Utility.Log.Info("Step:{0}", Client.GameEventID.SYSTEM_LOADSCENECOMPELETE.ToString());
                OnGameReady();
            }
             
            Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTADDDISPLAYEFFECT, loadScene.nMapID);
        }
        else if (id == (int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP)
        {
            DataManager.Manager<UIPanelManager>().ShowLoading();

            Client.IControllerSystem cs = Client.ClientGlobal.Instance().GetControllerSystem();
            if (cs != null)
            {
                Client.ICombatRobot robot = cs.GetCombatRobot();
                if (robot != null && robot.Status != Client.CombatRobotStatus.STOP)
                {
                    robot.Stop();
                }
            }
        }
        else if (id == (int)Client.GameEventID.SYSTEM_LOADUICOMPELETE)
        {
            m_uStepFlag |= (uint)Step.UI_READY;
            Engine.Utility.Log.Info("Step:{0}", Client.GameEventID.SYSTEM_LOADUICOMPELETE.ToString());
            OnGameReady();
        }
        else if (id == (int)Client.GameEventID.LOGINSUCESSANDRECEIVEALLINFO)
        {
            m_uStepFlag |= (uint)Step.DATA_READY;
            Engine.Utility.Log.Info("Step:{0}", Client.GameEventID.LOGINSUCESSANDRECEIVEALLINFO.ToString());
            OnGameReady();
            //ShowUINotfy();
        }
        else if (id == (int)Client.GameEventID.RECONNECT_SUCESS)
        {
            m_uStepFlag |= (uint)Step.SCENE_READY;
            Engine.Utility.Log.Info("Step:{0}", Client.GameEventID.SYSTEM_LOADSCENECOMPELETE.ToString());
            OnGameReady();
        }
    }


    void OnLoginSceneLoad(GameObject go)
    {
        if (go != null)
        {
            go.SetActive(false);
        }
    }
    public void OnBeginStep(Step step)
    {
        m_step = step;
        switch (CurrentStep)
        {
            case Step.LOGIN:
                OnEnterLogin();
                break;
            case Step.LOAD:
                OnEnterLoad();
                break;
        }
    }

    void OnEnterLoad()
    {
        DoLoad();
    }

    private void DoLoading()
    {
        UnLoadLoginScene();

        ////临时测试用的地形
        //GameObject terrain = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //terrain.transform.localPosition = Vector3.zero;
        //terrain.transform.localScale = new Vector3(100, 1, 100);
        //terrain.name = "terrain";
        //terrain.layer = LayerUtil.walkable;
        //yield return true;

        //删除
        if (LaunchPanel.Me != null)
        {
            LaunchPanel.Me.Destroy();
        }
        Resources.UnloadUnusedAssets();
        //开启计算ping值
        Protocol.Instance.SetTimer();
        //yield return true;
    }

    public void DoLoad()
    {
        DataManager.Manager<UIPanelManager>().ShowLoading(progress: 0);
        DataManager.Manager<UIManager>().ChangeUISceneState(UIManager.UISceneState.USS_Ingame);
        // 创建Camera
        GameApp.Instance().CreateMainCamera(true);
        CoroutineMgr.Instance.DelayInvokeMethod(0.1f, DoLoading);
    }

    void OnEnterLogin()
    {
        StartCoroutine(DoLoadLogin());
    }

    IEnumerator DoLoadLogin()
    {
        yield return true;
        yield return true;


        if (m_firstLogin)
        {
            //更换背景画面     
            //LaunchPanel.Me.SetBottomAndLogState();
            LaunchPanel.Me.Tips = Util.GetText("正在启动游戏......");
        }
        else
        {
            LaunchPanel.Me.Tips = Util.GetText("正在回到登录界面......");
        }

        yield return true;

        LaunchPanel.Me.Visible = false;
        DataManager.Manager<LoginDataManager>().Login();
        yield return true;
    }


    //-----------------------------------------------------------------------------------------------
    public const string LOGINSCENE = "DengLuJieMian01";
    public const string CHOOSEROLESCENE = "XuanRenJieMian01";

    public GameObject GetSceneRoot(string sceneName)
    {
        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        if (scene != null && scene.IsValid() && scene.isLoaded)
        {
            GameObject[] objs = scene.GetRootGameObjects();
            foreach (var item in objs)
            {
                if (item.name.Equals(sceneName))
                {
                    return item;
                }
            }
        }
        return null;
    }

    public void AddLoginScene(string sceneName, LoadSceneMode loadMode, Action<GameObject> callback)
    {

        if (GetSceneSetState(sceneName, true))
        {
            SwitchSceneDisable(sceneName);
            if (callback != null)
            {
                callback(GetSceneRoot(sceneName));
            }
            return;
        }
        else
        {
            StartCoroutine(LoadScene(sceneName, loadMode, callback));
        }
    }

    IEnumerator LoadScene(string sceneName, LoadSceneMode loadMode, Action<GameObject> callback)
    {
        AssetBundle sceneab = null;
        if (Application.isEditor)
        {
            sceneab = Engine.RareEngine.LoadFromFile("loginscene/" + sceneName.ToLower() + ".scene");
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                string strPath = System.IO.Path.Combine(Application.persistentDataPath + "/assets/", "loginscene/" + sceneName.ToLower() + ".scene");
                sceneab = AssetBundle.LoadFromFile(strPath);
                if (sceneab == null)
                {
                    strPath = System.IO.Path.Combine(Application.streamingAssetsPath, "loginscene/" + sceneName.ToLower() + ".scene");
                    sceneab = AssetBundle.LoadFromFile(strPath);
                }
            }
            else
            {
                string strPath = Application.persistentDataPath + "/loginscene/" + sceneName.ToLower() + ".scene";
                sceneab = AssetBundle.LoadFromFile(strPath);
                if (sceneab == null)
                {
                    strPath = System.IO.Path.Combine(Application.streamingAssetsPath, "loginscene/" + sceneName.ToLower() + ".scene");
                    sceneab = AssetBundle.LoadFromFile(strPath);
                }
            }
        }

        AsyncOperation asyn = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadMode);
        yield return asyn;
        if (sceneab != null)
        {
            sceneab.Unload(false);
        }

        SwitchSceneDisable(sceneName);

        GameObject sceneRoot = GetSceneRoot(sceneName);
        if (sceneRoot != null)
        {

            Transform camera = sceneRoot.transform.Find("MainCamera");
            if (camera != null)
            {
                if (camera.GetComponent<AudioListener>())
                {
                    GameObject.Destroy(camera.GetComponent<AudioListener>());
                }

                AudioSource audioSource = camera.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    Client.IGameOption option = Client.ClientGlobal.Instance().gameOption;
                    if (option != null)
                    {
                        //audioSource.volume = option.GetInt
                        float volume = option.GetInt("Sound", "soundslider", 1);
                        audioSource.volume = ((float)volume) / 100.0f;
                    }

                }

                if (sceneName == CHOOSEROLESCENE)
                {
                    Camera cam = camera.GetComponent<Camera>();
                    cam.cullingMask = -1 - (1 << LayerMask.NameToLayer("UI"));
                    UnityStandardAssets.ImageEffects.BloomAndFlares bloom = camera.GetComponent<UnityStandardAssets.ImageEffects.BloomAndFlares>();
                    if (bloom == null)
                    {
                        bloom = camera.gameObject.AddComponent<UnityStandardAssets.ImageEffects.BloomAndFlares>();
                        bloom.lensFlareShader = Shader.Find("Hidden/LensFlareCreate");
                        bloom.vignetteShader = Shader.Find("Hidden/Vignetting");
                        bloom.separableBlurShader = Shader.Find("Hidden/SeparableBlurPlus");
                        bloom.addBrightStuffOneOneShader = Shader.Find("Hidden/BlendOneOne");
                        bloom.screenBlendShader = Shader.Find("Hidden/Blend");
                        bloom.hollywoodFlaresShader = Shader.Find("Hidden/MultipassHollywoodFlares");
                        bloom.brightPassFilterShader = Shader.Find("Hidden/BrightPassFilterForBloom");

                        bloom.tweakMode = UnityStandardAssets.ImageEffects.TweakMode34.Complex;
                        bloom.screenBlendMode = UnityStandardAssets.ImageEffects.BloomScreenBlendMode.Screen;
                        bloom.hdr = UnityStandardAssets.ImageEffects.HDRBloomMode.Auto;
                        bloom.lensflares = false;
                        bloom.bloomIntensity = 0.3f;
                        bloom.bloomThreshold = 0.25f;
                        bloom.bloomBlurIterations = 2;
                        bloom.sepBlurSpread = 1.5f;
                        bloom.useSrcAlphaAsMask = 0f;
                    }
                }
                else if (sceneName == LOGINSCENE)
                {
                    //登录界面 场景 晃动效果
                    sceneRoot.AddComponent<LoginSceneGyr>();
                }
            }
        }
        if (callback != null)
        {
            callback(sceneRoot);
        }
    }

    void SwitchSceneDisable(string sceneName)
    {
        if (sceneName != LOGINSCENE)
        {
            GetSceneSetState(LOGINSCENE, false);
        }
        else if (sceneName != CHOOSEROLESCENE)
        {
            GetSceneSetState(CHOOSEROLESCENE, false);
        }
    }

    bool GetSceneSetState(string sceneName, bool activity)
    {
        UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);
        if (scene != null && scene.IsValid() && scene.isLoaded)
        {
            GameObject[] objs = scene.GetRootGameObjects();
            if (objs != null)
            {
                foreach (var obj in objs)
                {
                    obj.SetActive(activity);
                }
            }
            return true;
        }
        return false;
    }

    void UnLoadLoginScene()
    {
        //UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(LOGINSCENE);
        //if (scene != null && scene.IsValid() && scene.isLoaded)
        //{
        //    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(LOGINSCENE);
        //}
        //scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(CHOOSEROLESCENE);
        //if (scene != null && scene.IsValid() && scene.isLoaded)
        //{
        //    UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(CHOOSEROLESCENE);
        //}
    }
}
