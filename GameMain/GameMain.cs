using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

#if UNITY_ANDROID && !UNITY_EDITOR
#else
using Engine;
#endif

public class GameMain : MonoBehaviour
{

#if UNITY_ANDROID && !UNITY_EDITOR
    /// 游戏主框架
    public object m_RareApp = null;
    public Type mainType = null;
#else
    private GameApp m_RareApp = null;
#endif

    private string m_strMsg = "";

    //Engine.Utility.HttpDownloadFile http = null;

    private static int m_nUpdateIndex = 0;

    public static GameMain Instance = null;

    #region GuideDebug
    public bool OpenNewGuide = true;
    public bool OpenNewFuncGuide = true;
    #endregion

    // unityPlayer.SendMessage 响应函数
    void unpackage_android2unity(string strMsg)
    {
        int pos = strMsg.IndexOf('|');
        if (pos == -1)
        {
            return;
        }

        string strCmd = strMsg.Substring(0, pos);
        string strParam = strMsg.Substring(pos + 1, strMsg.Length - pos - 1);

        string[] strArgs = strParam.Split(',');
        if (strArgs.Length != 4)
        {
            return;
        }

        int nCur = int.Parse(strArgs[0]);
        int nTotal = int.Parse(strArgs[1]);
        string strFileName = strArgs[2];
        //string strInfo = strArgs[3];

        if (strCmd == "UnPackage")
        {
            //Star.Upgrade.GameUpgrade.Instance().OnUnpackageProgress(strFileName, nCur, nTotal);

            m_strMsg = string.Format("UnPackagr:{0:}/{1} {2}", nCur, nTotal, strFileName);
        }
        else if (strCmd == "ExtractZip")
        {
            if (nCur >= nTotal)
            {
                //EnterGameUpgrade();
            }
            //EventManager.send(EventType_Global.Game_UpdateProgress, new object[] { Star.Upgrade.UpgradeError.UpgradeError_CopyStreamFile, (long)nCur, (long)nTotal, strFileName });

            m_strMsg = string.Format("ExtractZip:{0:}/{1} {2}", nCur, nTotal, strFileName);
        }
    }

    void Awake()
    {
        Instance = this;

        //transform.gameObject.name = "GameRoot";

        // 只有安卓真机或者模拟器运行时会动态加载dll
        //LoadAllDLL();


    }

    private void LoadAllDLL()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Application.platform == RuntimePlatform.Android)
        {
            // 动态加载dll
            Assembly.LoadFrom(Application.persistentDataPath + "/dll/DestMath.dll");
            Assembly.LoadFrom(Application.persistentDataPath + "/dll/Engine.dll");
            Assembly.LoadFrom(Application.persistentDataPath + "/dll/Interface.dll");
            Assembly.LoadFrom(Application.persistentDataPath + "/dll/Common.dll");
            Assembly.LoadFrom(Application.persistentDataPath + "/dll/EntitySystem.dll");
            Assembly.LoadFrom(Application.persistentDataPath + "/dll/Controller.dll");
            Assembly.LoadFrom(Application.persistentDataPath + "/dll/SkillSystem.dll");
            Assembly.LoadFrom(Application.persistentDataPath + "/dll/ClientGlobal.dll");
            Assembly.LoadFrom(Application.persistentDataPath + "/dll/UI.dll");

            Assembly main = Assembly.LoadFrom(Application.persistentDataPath + "/dll/Main.dll");

            mainType = main.GetType("GameApp");
            m_RareApp = Activator.CreateInstance(mainType);
        }
#endif
    }

    // Use this for initialization
    void Start()
    {
        string strRootName = transform.gameObject.name;
        GameObject root = transform.gameObject;
        m_RareApp = GameApp.Instance();
#if UNITY_ANDROID && !UNITY_EDITOR
        MethodInfo mi =mainType.GetMethod("Init");
        mi.Invoke(m_RareApp, new object[] { root });
#else
        m_RareApp.Init(ref root);
#endif
        if (Application.isEditor)
        {
            //  root.AddComponent<NetDataMonitor>();
        }
        Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP, OnEventCallBack);
    }

    void OnEventCallBack(int nEventID, object param)
    {
        if (nEventID == (int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP)
        {
            GameApp.Instance().OnLowMemoryWarning();
        }
    }
    /// <summary>
    /// 渲染帧率更新
    /// </summary>
    void Update()
    {
        m_nUpdateIndex++;
        //Profiler.BeginSample("InputUtil");
        InputUtil.Update();
        //Profiler.EndSample();

#if UNITY_ANDROID && !UNITY_EDITOR
        MethodInfo mi = mainType.GetMethod("PreUpdate");
        mi.Invoke(m_RareApp, new object[] { Time.deltaTime });

        mi = mainType.GetMethod("Update");
        mi.Invoke(m_RareApp, new object[] { Time.deltaTime });
#else
        m_RareApp.PreUpdate(Time.deltaTime);
        m_RareApp.Update(Time.deltaTime);
#endif
        ///Engine.Utility.Log.Trace("Update>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>{0}", m_nUpdateIndex);
    }

    public void OnDestroy()
    {
        Engine.Utility.EventEngine.Instance().RemoveAllEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEAVEMAP);
    }

    /// <summary>
    /// 延迟更新
    /// </summary>
    void LateUpdate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        MethodInfo mi = mainType.GetMethod("LateUpdate");
        mi.Invoke(m_RareApp, new object[] { Time.deltaTime });
#else
        m_RareApp.LateUpdate(Time.deltaTime);
#endif
    }

    // Update is called once per frame
    /// <summary>
    ///  固定帧率的Update
    /// </summary>
    void FixedUpdate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        MethodInfo mi = mainType.GetMethod("FixedUpdate");
        mi.Invoke(m_RareApp, new object[] { Time.deltaTime });
#else
        m_RareApp.FixedUpdate(Time.fixedDeltaTime);
#endif

    }

    void DestroyLightInfo()
    {
        GameObject obj = GameObject.Find("DirectionalLight1");
        if (obj != null)
        {
            GameObject.DestroyImmediate(obj);
            obj = null;
        }
        obj = GameObject.Find("DirectionalLight2");
        if (obj != null)
        {
            GameObject.DestroyImmediate(obj);
            obj = null;
        }
        obj = GameObject.Find("DirectionalLight3");
        if (obj != null)
        {
            GameObject.DestroyImmediate(obj);
            obj = null;
        }
    }
    public void GoToLogin(LoginPanel.ShowUIEnum uienum = LoginPanel.ShowUIEnum.Authorize, Action act = null)
    {
        Action<GameObject> action = (obj) =>
            {
                CameraFollow.Instance.target = null;

                Client.ClientGlobal.Instance().Clear(true);
                //
                DestroyLightInfo();
                DataManager.Instance.Reset(true);
                StepManager.Instance.Clear();
                DataManager.Manager<LoginDataManager>().GotoLogin(uienum, act);
            };
        UnityEngine.SceneManagement.LoadSceneMode mode = UnityEngine.SceneManagement.LoadSceneMode.Additive;
        if (StepManager.Instance.CurrentStep != Step.LOGIN)
        {
            mode = UnityEngine.SceneManagement.LoadSceneMode.Single;
        }
        StepManager.Instance.AddLoginScene(StepManager.LOGINSCENE, mode, action);
    }

    private void LoadRenderObj(string strObj)
    {
        //人物资源预先加载
        IRenderObj obj = null;
        IRenderSystem rs = RareEngine.Instance().GetRenderSystem();

        rs.CreateRenderObj(ref strObj, ref obj, null, Vector3.zero, TaskPriority.TaskPriority_Immediate, true);
        if (obj != null)
            rs.RemoveRenderObj(obj);
    }

    private void LoadEffect(string strEffect, int nCount = 1)
    {
        Engine.IRenderSystem renderSys = Engine.RareEngine.Instance().GetRenderSystem();
        Engine.IEffect effect = null;
        List<Engine.IEffect> temp = new List<IEffect>();

        for (int i = 0; i < nCount; i++)
        {
            renderSys.CreateEffect(ref strEffect, ref effect, null, Engine.TaskPriority.TaskPriority_Immediate, true);
            temp.Add(effect);
        }


        for (int i = 0; i < nCount; i++)
        {
            renderSys.RemoveEffect(temp[i]);
        }
        temp.Clear();

    }

    private void LoadPrepareResource()
    {
        //-------------------------------------------------------
        LoadRenderObj("character/fashi_nan04_high/fashi_nan04_high.obj");
        LoadRenderObj("character/zhanshi_nan04_high/zhanshi_nan04_high.obj");
        LoadRenderObj("character/anwu_nv04_high/anwu_nv04_high.obj");
        LoadRenderObj("character/huanshi_nv04_high/huanshi_nv04_high.obj");

        //特效加载
        LoadEffect("effect/character/zhanshi/zs_zhanshi_tx.fx", 1);
        LoadEffect("effect/character/fashi/s_05.fx", 1);
        LoadEffect("effect/character/anwu/an_zhanshi_tx.fx", 1);
        LoadEffect("effect/character_show/anwuzhaohuanshou.fx", 1);
        LoadEffect("effect/character/huanshi/h_03.fx", 1);
    }
    //-------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 重新选择角色界面
    /// </summary>
    public void GotoReSelecetRole()
    {
        CameraFollow.Instance.target = null;
        DataManager.Instance.Reset(true);
        Client.ClientGlobal.Instance().Clear(true);
        //
        DestroyLightInfo();
        // 加载资源
        LoadPrepareResource();

        StepManager.Instance.Clear();
        GameCmd.stBackSelectUserCmd cmd = new GameCmd.stBackSelectUserCmd();
        NetService.Instance.Send(cmd);
    }

    /// <summary>
    /// 应用退出
    /// </summary>
    void OnApplicationQuit()
    {
        GVoiceManger.Instance.OnQuit();
        Pmd.UserLogoutTokenLoginUserPmd_C cmd = new Pmd.UserLogoutTokenLoginUserPmd_C();
        NetService.Instance.Send(cmd);
#if UNITY_ANDROID && !UNITY_EDITOR
        MethodInfo mi = mainType.GetMethod("Release");
        mi.Invoke(m_RareApp, null);
#else
        m_RareApp.Release();
#endif
    }

    //List<uint> mapID = new List<uint>();
    //bool bInit = false;
    //int nIndex = 0;
    //static int nChangeCount = 0;
    //KNpc npc1 = null;
    //KNpc npc2 = null;
    void OnGUI()
    {

        ////测试http下载
        //if (GUI.Button(new Rect(300, 80, 200, 100), "Test"))
        //{
            //string url = "http://update.jzry.zqgame.com/ioszhengshi/resources/开篇.rar";
            //KHttpDown.Instance().DownLoadFile(url, "c://Game.ver");

            //ZipHelper.UnZip("c://map.zip", "c://");
            //KDownlaodFile.Instance().StartDownload("http://update.jzry.zqgame.com/androidmicro/resources/map.zip", "c://map.zip");

            //string strScr = GlobalConst.localFilePath + "assets/map.zip";
            //string strDest = GlobalConst.localFilePath + "assets/";
            //UnityEngine.Debug.Log("AAAAAAAAAAAAAAAAAAAAAAA");
            //UnityEngine.Debug.Log(strScr);
            //UnityEngine.Debug.Log(strDest);
            //ZipHelper.UnZip(strScr, strDest);
        //}

        //if (GUI.Button(new Rect(300, 180, 200, 100), "TestAAA"))
        //{
        //    //KHttpDown.Instance().TestAAA();
        //}
        //if (GUI.Button(new Rect(300, 280, 200, 100), "TestBBB"))
        //{
        //    //KHttpDown.Instance().TestBBB();
        //}



        //if (GUI.Button(new Rect(300, 80, 200, 100), "streamingAssetsPath"))
        //{
        //    string path = Application.streamingAssetsPath;
        //    if (Directory.Exists(path))
        //    {
        //        //获取文件信息
        //        DirectoryInfo direction = new DirectoryInfo(path);

        //         FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

        //         for (int i = 0; i < files.Length; i++)
        //         {
        //             string name  = files[i].FullName;
        //             UnityEngine.Debug.Log(name);

        //         }
        //    }

        //}

        //if (GUI.Button(new Rect(300, 280, 200, 100), "persistentDataPath"))
        //{
        //    string path = Application.persistentDataPath;
        //    if (Directory.Exists(path))
        //    {
        //        //获取文件信息
        //        DirectoryInfo direction = new DirectoryInfo(path);

        //        FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

        //        for (int i = 0; i < files.Length; i++)
        //        {
        //            string name = string.Format("文件名称{0}--大小{1}", files[i].FullName, files[i].Length);
        //            UnityEngine.Debug.Log(name);

        //        }
        //    }


        //}
        //if (GUI.Button(new Rect(300, 80, 200, 100), "AddNpc"))
        //{
        //    // 查看资源
        //    //Engine.RareEngine.Instance().DumpResouce(typeof(UnityEngine.Texture2D));

        //    //npc1 = FirstFightMgr.Instance.AddNpc(13811);
        //    //npc2 = FirstFightMgr.Instance.AddNpc(13811);

        //    FirstFightMgr.Instance.EnterScene();

        //}

        //if (GUI.Button(new Rect(300, 180, 200, 100), "CastSkill"))
        //{
        //    //npc1.CastSkill(npc2, 10005);

        //    FirstFightMgr.Instance.Test();

        //}
        //if (GUI.Button(new Rect(300, 280, 200, 100), "Move"))
        //{
        //    Client.Move move = new Client.Move();
        //    //move.m_dir = Global.S2CDirection(cmd.dir);
        //    move.strRunAct = Client.EntityAction.Run; // 动作名需要统一处理
        //    move.m_target = new Vector3(100,0,100);
        //    move.m_speed = npc2.m_pEntity.GetProp((int)Client.WorldObjProp.MoveSpeed) * Client.EntityConst.MOVE_SPEED_RATE;
        //    // Log.Error( "npc pos is" + pos.ToString() );
        //    npc2.m_pEntity.SendMessage(Client.EntityMessage.EntityCommand_MoveTo, (object)move);

        //}



        //if (bInit == false)
        //{
        //    mapID.Add(1);
        //    mapID.Add(2);
        //    mapID.Add(3);
        //    mapID.Add(4);
        //    mapID.Add(5);
        //    mapID.Add(6);
        //    mapID.Add(7);
        //    mapID.Add(8);
        //    mapID.Add(9);
        //    mapID.Add(10);
        //    mapID.Add(11);
        //    mapID.Add(12);
        //    mapID.Add(13);
        //    mapID.Add(14);
        //    mapID.Add(15);
        //    mapID.Add(16);
        //    mapID.Add(17);
        //    mapID.Add(18);
        //    mapID.Add(19);
        //    mapID.Add(20);
        //    mapID.Add(21);


        //    bInit = true;
        //}


        //if (GUI.Button(new Rect(200, 200, 200, 200), nChangeCount.ToString()))
        //{
        //    nChangeCount++;
        //    Client.IMapSystem mapsys = Client.ClientGlobal.Instance().GetMapSystem();
        //    mapsys.EnterMap(mapID[nIndex++], new Vector3(0, 0, 0));

        //    if (nIndex >= mapID.Count - 1)
        //    {
        //        nIndex = 0;
        //    }

        //}

    }
}