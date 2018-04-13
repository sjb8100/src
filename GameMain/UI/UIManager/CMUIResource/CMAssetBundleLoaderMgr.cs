using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;



public class CMAssetBundleLoaderMgr : IManager
{
    #region property
    private StringBuilder strBuilder = new StringBuilder();
    //assetbundle数据
    private Dictionary<string, CMAssetBundle> m_dicAssetBundleDatas = null;
    #endregion

    #region CMAssetBundle Asset
    /// <summary>
    /// 尝试获取CMAssetBundle
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="ab"></param>
    /// <returns></returns>
    public bool TryGetCMAssetBundle(string abPath,out CMAssetBundle ab)
    {
        return m_dicAssetBundleDatas.TryGetValue(abPath, out ab);
    }
    /// <summary>
    /// 资源引用次数减一
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    public void Release(string abPath,string assetName)
    {
        CMAssetBundle cmAB = null;
        if (m_dicAssetBundleDatas.TryGetValue(abPath, out cmAB) && cmAB.IsDone())
        {
            cmAB.ReleaseAsset(assetName);
        }
    }


    /// <summary>
    /// 获取资源（同步）
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public UnityEngine.Object CreateAsset(string abPath,string assetName)
    {
        CMAssetBundle cmAB = null;
        if (!m_dicAssetBundleDatas.TryGetValue(abPath, out cmAB))
        {
            cmAB = CMAssetBundle.Create(abPath);
            m_dicAssetBundleDatas.Add(abPath, cmAB);
        }

        if (cmAB.GetTaskState() == CMABTaskState.CMTaskState_None)
        {
            string []dependences = GetABAllDependencies(abPath);
            if (null != dependences)
            {
                CMAssetBundle dpCmAB = null;
                AssetBundle dpAB = null;
                for(int i = 0,max = dependences.Length;i < max;i ++)
                {
                    if (!m_dicAssetBundleDatas.TryGetValue(dependences[i], out dpCmAB))
                    {
                        dpCmAB = CMAssetBundle.Create(dependences[i]);
                        m_dicAssetBundleDatas.Add(dependences[i], dpCmAB);
                    }
                    if (dpCmAB.IsDone())
                    {
                        continue;
                    }
                    dpAB = AssetBundle.LoadFromFile(GetFileFullPath("ui/" + dependences[i]));
                    dpCmAB.Done(dpAB);
                }
            }
            cmAB.OnWaitingStart();
            AssetBundle ab = AssetBundle.LoadFromFile(GetFileFullPath("ui/" + abPath));
            cmAB.OnProcessing(1);
            cmAB.Done(ab);
        }

        return cmAB.CreateAsset(assetName);
    }

    /// <summary>
    /// 获取资源（异步）
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <param name="loadFinish"></param>
    /// <param name="param"></param>
    public void CreateAssetAsyn(bool preLoad,string abPath, string assetName, AssetLoadFinsh loadFinish, object param1 = null, object param2 = null, object param3 = null)
    {
        CMAssetBundle cmAB = null;
        if (!m_dicAssetBundleDatas.TryGetValue(abPath,out cmAB))
        {
            cmAB = CMAssetBundle.Create(abPath);
            m_dicAssetBundleDatas.Add(abPath, cmAB);
        }
        cmAB.CreateAssetAsyn(preLoad,assetName, loadFinish, param1, param2, param3);
        LoadAssetBundleAsyn(cmAB.ABPath);
    }
    #endregion

    #region Load AssetBundle
    //等待加载ab列表
    private System.Collections.Generic.Queue<string> waitingABPath = null;
    //加载异步数据
    private AssetBundleCreateRequest abCreateRequiest = null;
    private const int MAX_ASYN_LOADAB_NUM_PER = 5;
    //当前加载文件路径
    private string curLoadPath = "";
    private Dictionary<string, AssetBundleCreateRequest> asynLoadingAbDic = null;
    /// <summary>
    /// 异步加载AB
    /// </summary>
    /// <param name="abPath"></param>
    public void LoadAssetBundleAsyn(string abPath)
    {
        CMAssetBundle cmAB = null;
        if (!m_dicAssetBundleDatas.TryGetValue(abPath, out cmAB))
        {
            cmAB = CMAssetBundle.Create(abPath);
            m_dicAssetBundleDatas.Add(abPath, cmAB);
        }
        if (cmAB.GetTaskState() != CMABTaskState.CMTaskState_None)
        {
            //已经在加载了
            return;
        }

        string[] dependenciesPath = GetABAllDependencies(cmAB.ABPath);

        if (null != dependenciesPath)
        {
            for(int i = 0,max = dependenciesPath.Length;i < max;i++)
            {
                if (!waitingABPath.Contains(dependenciesPath[i]))
                {
                    waitingABPath.Enqueue(dependenciesPath[i]);
                }
                    
            }
        }

        waitingABPath.Enqueue(abPath);
        cmAB.OnWaitingStart();
        //DoNextABLoad();
        ProcessABLoadAsyn();
    }

    //是否正在加载
    private bool IsLoadingAB
    {
        get
        {
            return null != abCreateRequiest && !string.IsNullOrEmpty(curLoadPath);
        }
    }

    /// <summary>
    /// 是否有下一个要加载的资源
    /// </summary>
    private bool HasNextAB
    {
        get
        {
            return (null != waitingABPath && waitingABPath.Count > 0);
        }
    }

    private bool CanLoadNextABAsyn()
    {
        if ((waitingABPath.Count > 0)
            && (CurAsynLoadingAbNum < MAX_ASYN_LOADAB_NUM_PER))
        {
            string nextAb = waitingABPath.Peek();
            if (IsDependenceLoadComplete(nextAb))
            {
                return true;
            }
        }
        return false;
    }

    //当前正在异步加载的AB数量
    private int CurAsynLoadingAbNum
    {
        get
        {
            return asynLoadingAbDic.Count;
        }
    }

   

    /// <summary>
    /// 执行下一个ab加载
    /// </summary>
    /// <returns></returns>
    private bool DoNextABLoad()
    {
        if (null != abCreateRequiest || !HasNextAB)
        {
            return false;
        }
        bool doNextSuccess = false;
        if (waitingABPath.Count >0)
        {
            string path = waitingABPath.Dequeue();
            CMAssetBundle cmAB = null;
            if (m_dicAssetBundleDatas.TryGetValue(path, out cmAB)
                && (cmAB.GetTaskState() == CMABTaskState.CMTaskState_Processing
                || cmAB.GetTaskState() == CMABTaskState.CMTaskState_Done))
            {
                return DoNextABLoad();
            }
            else
            {
                if (null == cmAB)
                {
                    cmAB = CMAssetBundle.Create(path);
                    m_dicAssetBundleDatas.Add(cmAB.ABPath, cmAB);
                }

                try
                {
                    abCreateRequiest = AssetBundle.LoadFromFileAsync(GetFileFullPath("ui/" + path));
                    if (null != abCreateRequiest)
                    {
                        doNextSuccess = true;
                        curLoadPath = path;
                        cmAB.OnProcessing(abCreateRequiest.progress);
                    }
                }
                catch (Exception e)
                {
                    cmAB.Done(null);
                    Debug.LogError(e);
                }
            }
           
        }
        return doNextSuccess;
    }

    /// <summary>
    /// 获取CMAssetBundle没有就创建
    /// </summary>
    /// <param name="abPath"></param>
    /// <returns></returns>
    private CMAssetBundle GetCMAssetBundle(string abPath)
    {
        CMAssetBundle cmAssetBundle = null;
        if (!TryGetCMAssetBundle(abPath,out cmAssetBundle))
        {
            cmAssetBundle = CMAssetBundle.Create(abPath);
            m_dicAssetBundleDatas.Add(abPath, cmAssetBundle);
        }
        return cmAssetBundle;
    }

    private List<string> tempStrLst = new List<string>();
    /// <summary>
    /// 执行assetbundle异步加载检测
    /// </summary>
    private void ProcessABLoadAsyn()
    {
        CMAssetBundle tempCMAB = null;
        string tempAbPath = null;
        AssetBundleCreateRequest tempABCreateRequiest = null;
        if (CurAsynLoadingAbNum > 0)
        {
            var enumerator = asynLoadingAbDic.GetEnumerator();
            while(enumerator.MoveNext())
            {
                if (enumerator.Current.Value.isDone)
                {
                    tempStrLst.Add(enumerator.Current.Key);
                }
                
            }
        }
        if (tempStrLst.Count > 0)
        {
            for (int i = 0, max = tempStrLst.Count; i < max; i++)
            {
                tempAbPath = tempStrLst[i];
                tempCMAB = GetCMAssetBundle(tempAbPath);
                if (asynLoadingAbDic.TryGetValue(tempAbPath, out tempABCreateRequiest))
                {
                    if (!tempCMAB.IsDone())
                    {
                        //执行完成
                        tempCMAB.Done(tempABCreateRequiest.assetBundle);
                        //给依赖资源添加引用计数
                        AddABDependenceRef(tempStrLst[i]);
                    }

                    if (asynLoadingAbDic.ContainsKey(tempAbPath))
                        asynLoadingAbDic.Remove(tempAbPath);
                }
            }
            tempStrLst.Clear();
        }

        while(CanLoadNextABAsyn())
        {
            tempAbPath = waitingABPath.Dequeue();
            tempCMAB = GetCMAssetBundle(tempAbPath);
            if (tempCMAB.GetTaskState() != CMABTaskState.CMTaskState_None
                && tempCMAB.GetTaskState() != CMABTaskState.CMTaskState_Waiting)
            {
                continue;
            }
            tempABCreateRequiest = AssetBundle.LoadFromFileAsync(GetFileFullPath("ui/" + tempAbPath));
            tempCMAB.OnProcessing(tempABCreateRequiest.progress);

            asynLoadingAbDic.Add(tempAbPath, tempABCreateRequiest);
        }
    }

    private List<string> alreadyLoadAsset = new List<string>();
    private void ProcessAbAssetLoadAsyn(float deltaTime)
    {
        var enumerator = m_dicAssetBundleDatas.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.Process(deltaTime);
            if (enumerator.Current.Value.HasAssetLoadComplete())
            {
                alreadyLoadAsset.Add(enumerator.Current.Key);
            }
        }

        if (alreadyLoadAsset.Count > 0)
        {
            CMAssetBundle cmab = null;
            for (int i = 0, max = alreadyLoadAsset.Count; i < max; i++)
            {
                if (TryGetCMAssetBundle(alreadyLoadAsset[i], out cmab))
                {
                    cmab.HandleAssetCreateDlg();
                }
            }
            alreadyLoadAsset.Clear();
        }
    }

    public bool IsAssetReady(string ab,string assetName)
    {
        CMAssetBundle cb = null;
        if (TryGetCMAssetBundle(ab,out cb) && cb.IsAssetReady(assetName))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 是否依赖资源加载完成
    /// </summary>
    /// <param name="abPath"></param>
    /// <returns></returns>
    public bool IsDependenceLoadComplete(string abPath)
    {
        string[] dependences = GetABAllDependencies(abPath);
        bool complete = true;
        if (null != dependences)
        {
            CMAssetBundle dpCmAB = null;
            for (int i = 0, max = dependences.Length; i < max; i++)
            {
                if (!TryGetCMAssetBundle(dependences[i],out dpCmAB) || !dpCmAB.IsDone())
                {
                    complete = false;
                    break;
                }
                
            }
        }
        return complete;
    }

    /// <summary>
    /// abpath AssetBundle加载完成，给依赖AB添加引用计数
    /// </summary>
    /// <param name="abpath"></param>
    private void AddABDependenceRef(string abpath)
    {
        if (m_dicAssetBundleDatas.ContainsKey(abpath))
        {
            string []dependencies = GetABAllDependencies(abpath);
            if (null != dependencies)
            {
                CMAssetBundle cmAB = null;
                for(int i =0 ,max = dependencies.Length;i < max;i ++)
                {
                    if (m_dicAssetBundleDatas.TryGetValue(dependencies[i],out cmAB))
                    {
                        //cmAB.AddRef();
                    }
                }
            }
        }
    }

    #endregion



    #region Load AssetBundle Obj
    private Dictionary<string, AssetBundle> cacheAssetBundleDic = new Dictionary<string, AssetBundle>();
    public void ClearCacheAssetBundle()
    {
        foreach (AssetBundle bundle in cacheAssetBundleDic.Values)
        {
            if (bundle == null)
                continue;
            bundle.Unload(false);
        }
        if (am != null)
            am = null;
        cacheAssetBundleDic.Clear();
    }
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="name">资源名称</param>
    /// <param name="callback">回调函数</param>
    public void LoadAssetBundle(string name, Action<UnityEngine.Object> callback)
    {
        string realName = name;
        string assetName0 = realName.Replace(System.IO.Path.GetExtension(realName), "");
        string assetName = assetName0.Substring(assetName0.LastIndexOf("/") + 1);
        if (cacheAssetBundleDic.ContainsKey(name))
        {
            //UnityEngine.Object obj = cacheAssetBundleDic[name].LoadAsset(assetName);
            //callback(obj);
            //return;
        }
        Action<List<AssetBundle>> action = (dependenceAssetBundle) =>
        {
            LoadResReturnWWW(realName, (www) =>
            {
                AssetBundle ab = www.assetBundle;
                if (!cacheAssetBundleDic.ContainsKey(realName))
                    cacheAssetBundleDic.Add(realName, ab);
                UnityEngine.Object obj = ab.LoadAsset(assetName);
                callback(obj);
            });
        };
        LoadDependenceAsset(realName.Substring(PathDefine.ResPath.Length), action);
    }

    /// <summary>
    /// 加载依赖资源
    /// </summary>
    /// <param name="targetAssetName"></param>
    /// <param name="action"></param>
    public void LoadDependenceAsset(string targetAssetName, Action<List<AssetBundle>> action)
    {
        Action<AssetBundleManifest> dependenceAction = (mainifest) =>
        {
            List<AssetBundle> dependenceAssetBundleList = new List<AssetBundle>();
            string[] dependences = mainifest.GetAllDependencies(targetAssetName);
            int dependenceLength = dependences.Length;
            int count = 0;
            Action<AssetBundle> onAssetBundleLoad = (ab) =>
            {
                count++;
                if (count == dependenceLength)
                {
                    //依赖加载完成
                    action(dependenceAssetBundleList);
                }
            };

            if (dependenceLength == 0)
            {
                //no dependence
                action(dependenceAssetBundleList);
            }
            else
            {
                LoadDependenceAsset(new List<string>(dependences), onAssetBundleLoad);
            }
        };

        //加载AssetBundleManifest
        LoadAssetBundleManifest(dependenceAction);
    }

    /// <summary>
    /// 逐个加载依赖资源
    /// </summary>
    /// <param name="dependenceName">依赖资源列表</param>
    /// <param name="callback">没加载完成回调一次</param>
    public void LoadDependenceAsset(List<string> dependenceName,Action<AssetBundle> callback)
    {
        if (dependenceName.Count != 0)
        {
            string resName =PathDefine.ResPath + dependenceName[0];

            Action<AssetBundle> loadAction = (ab) =>
            {
                dependenceName.Remove(dependenceName[0]);
                if (callback != null)
                    callback.Invoke(ab);
                LoadDependenceAsset(dependenceName, callback);

            };
            if (cacheAssetBundleDic.ContainsKey(resName))
            {
                loadAction.Invoke(cacheAssetBundleDic[resName]);
            }else
            {
                LoadResReturnWWW(resName, (www) =>
                    {
                        AssetBundle ab = www.assetBundle;
                        cacheAssetBundleDic.Add(resName, ab);
                        loadAction.Invoke(ab);
                    });
            }
        }
    }

    AssetBundleManifest am = null;
    /// <summary>
    /// 加载AssetBundleManifest
    /// </summary>
    /// <param name="action"></param>
    public void LoadAssetBundleManifest(Action<AssetBundleManifest> action)
    {
        string manifestName = PathDefine.ResPath + GetRuntimePlatformName(); ;
        if (cacheAssetBundleDic.ContainsKey(manifestName))
        {
            if (am == null)
            {
                am = (cacheAssetBundleDic[manifestName].LoadAsset("AssetBundleManifest")) as AssetBundleManifest;
            }
            action(am);
        }
        else
        {
            LoadResReturnWWW(manifestName, (www) =>
            {
                AssetBundle ab = www.assetBundle;
                if (null == ab)
                {
                    Debug.LogError("Load manifest assetbunle null");
                }
                //Debug.LogError("Manifest www size:" + www.size + ",url:" + www.url);
                AssetBundleManifest manifest = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                action(manifest);
                cacheAssetBundleDic.Add(manifestName, ab);
            });
        }

    }

    /// <summary>
    /// 通过www加载资源并返回www
    /// </summary>
    /// <param name="resName"></param>
    /// <param name="callback"></param>
    public void LoadResReturnWWW(string path, Action<WWW> callback)
    {
        string url = "";
#if UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE
        url = PathDefine.WWW_Local_File_Path(path);
#else 
        url = path;
#endif
        //CoroutineMgr.Instance.StartLoadResCoroutine(url, callback);
    }

    #endregion

    #region AssetBundleManifest
    public static string GetFileFullPath(string path)
    {
        string strResPath = Engine.Utility.FileUtils.Instance().FullPathFileName(ref path, Engine.Utility.FileUtils.UnityPathType.UnityPath_CustomPath); // 资源路径
        strResPath = System.IO.Path.GetFullPath(strResPath);
        if (!System.IO.File.Exists(strResPath))
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                strResPath = Application.dataPath + "!assets/" + path;
            }
            else
            {
                strResPath = Engine.Utility.FileUtils.Instance().FullPathFileName(ref path, Engine.Utility.FileUtils.UnityPathType.UnityPath_StreamAsset);
            }
        }

        return strResPath;
    }
    private AssetBundleManifest m_abMainfest = null;
    private void InitManifest()
    {
        AssetBundle ab = AssetBundle.LoadFromFile(GetFileFullPath("ui/ui"));
        m_abMainfest = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
        if (m_abMainfest == null)
        {
            Engine.Utility.Log.Error("Error Load AssetBundleManifest failed");
        }
        if (null != ab)
        {
            ab.Unload(false);
        }
    }
    private Dictionary<string, List<CMAssetDependenceData>> m_dicAssetDependence = new Dictionary<string,List<CMAssetDependenceData>>();
    /// <summary>
    /// 初始化
    /// </summary>
    private void InitAssetsDependenceData()
    {
        AssetBundle ab = Engine.RareEngine.LoadFromFile("ui/uidependces.u3d");
        if(ab == null)
        {
            Engine.Utility.Log.Error("uidependces.u3d ab is null ");
            return;
        }
        UIAssetDependence ud = ab.LoadAllAssets<UIAssetDependence>()[0];
        if(ud == null)
        {
            Engine.Utility.Log.Error("UIAssetDependence is null ");
            return;
        }
        foreach(var dic in ud.m_dicAssetDependence)
        {
            m_dicAssetDependence.Add(dic.Key, dic.Value.cmList);
        }
       // m_dicAssetDependence = ud.m_dicAssetDependence;
        /*
        m_dicAssetDependence = new Dictionary<string, List<CMAssetDependenceData>>();
        Engine.JsonNode root = Engine.RareJson.ParseJsonFile("ui/assetsdependence.json");
        if (root == null)
        {
            Engine.Utility.Log.Error("CMAssetBundleLoaderMgr 解析{0}文件失败!", "ui/assetsdependence.json");
            return;
        }

        Engine.JsonArray assets = (Engine.JsonArray)root["Assets"];
        CMAssetDependenceData dependencenData = null;
        for (int i = 0; i < assets.Count; i++)
        {
            Engine.JsonObject obj = (Engine.JsonObject)assets[i];
            if (obj != null)
            {
                Engine.JsonArray dependence = (Engine.JsonArray)obj["dependence"];
                if (null == dependence)
                {
                    continue;
                }
                string path = obj["path"];
                for (int k = 0; k < dependence.Count; k++)
                {
                    Engine.JsonObject dependenceobj = (Engine.JsonObject)dependence[k];
                    dependencenData = new CMAssetDependenceData();
                    dependencenData.AssetPath = path;
                    dependencenData.DependentAssetPath = dependenceobj["assetPath"];
                    dependencenData.DependentAssetBundlePath = dependenceobj["assetBundlePath"];
                    if (!m_dicAssetDependence.ContainsKey(path))
                        m_dicAssetDependence.Add(path, new List<CMAssetDependenceData>());
                    m_dicAssetDependence[path].Add(dependencenData);
                }
            }
        }
         * */
    }

    /// <summary>
    /// 添加依赖资源引用计数
    /// </summary>
    /// <param name="assetName"></param>
    public void AddDependenceAssetRef(string assetName)
    {
        List<CMAssetDependenceData> dData = null;
        if (TryGetAssetDependenceData(assetName,out dData))
        {
            for(int i = 0,max = dData.Count;i < max;i ++)
            {
                AddAssetRef(dData[i].DependentAssetPath, dData[i].DependentAssetBundlePath);
            }
        }
    }

    /// <summary>
    /// 移除依赖资源引用计数
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="abPath"></param>
    public void RemoveDependenceAssetRef(string assetName)
    {
        List<CMAssetDependenceData> dData = null;
        if (TryGetAssetDependenceData(assetName, out dData))
        {
            for (int i = 0, max = dData.Count; i < max; i++)
            {
                RemoveAssetRef(dData[i].DependentAssetPath, dData[i].DependentAssetBundlePath);
            }
        }
    }


    /// <summary>
    /// 添加资源引用计数
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="abPath"></param>
    public void AddAssetRef(string assetName,string abPath)
    {
        CMAssetBundle ab = null;
        if (!TryGetCMAssetBundle(abPath,out ab))
        {
            Debug.LogError(string.Format("CMAssetBundleLoaderMgr->AddAssetRef failed,CmassetBundle {0} not exit",abPath));
            return;
        }
        ab.ReferenceAssset(assetName);
    }

    /// <summary>
    /// 移除资源引用计数
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="abPath"></param>
    public void RemoveAssetRef(string assetName, string abPath)
    {
        CMAssetBundle ab = null;
        if (!TryGetCMAssetBundle(abPath, out ab))
        {
            Debug.LogError(string.Format("CMAssetBundleLoaderMgr->RemoveAssetRef failed,CmassetBundle {0} not exit", abPath));
            return;
        }
        ab.ReleaseAsset(assetName);
    }

    /// <summary>
    /// 尝试获得单个资源的依赖关系
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="assetDependence"></param>
    /// <returns></returns>
    public bool TryGetAssetDependenceData(string assetName, out List<CMAssetDependenceData> assetDependence)
    {
        return m_dicAssetDependence.TryGetValue(assetName, out assetDependence);
    }

    /// <summary>
    /// 获取assetbundle所有依赖
    /// </summary>
    /// <param name="abPathName"></param>
    /// <returns></returns>
    public string[] GetABAllDependencies(string abPathName)
    {
        if (!string.IsNullOrEmpty(abPathName) && null != m_abMainfest)
        {
            return m_abMainfest.GetAllDependencies(abPathName);
        }
        return null;
    }
    #endregion

    #region IManager Method
    
    public void Initialize()
    {
        m_dicAssetBundleDatas = new Dictionary<string, CMAssetBundle>();
        waitingABPath = new Queue<string>();
        asynLoadingAbDic = new Dictionary<string, AssetBundleCreateRequest>();
        InitAssetsDependenceData();
        InitManifest();
    }

    public void Reset(bool depthClearData = false)
    {
        
    }

    public void Process(float deltaTime)
    {
        ProcessABLoadAsyn();
        ProcessAbAssetLoadAsyn(deltaTime);
    }
    public void ClearData()
    {

    }
    #endregion
    /// <summary>
    /// 获取对应平台的名称
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static string GetStringNameOfPlatform(RuntimePlatform platform)
    {
        string stringName = "";
        switch (platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                //stringName = "Windows";
                //break;
            case RuntimePlatform.Android:
                stringName = "Android";
                break;
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.OSXPlayer:
                stringName = "IOS";
                break;
        }
        return stringName;
    }

    /// <summary>
    /// 获取当前平台名称
    /// </summary>
    /// <returns></returns>
    public static string GetRuntimePlatformName()
    {
        return GetStringNameOfPlatform(Application.platform);
    }

    public string BuildAssetBundlePath()
    {
        string path = "";
        return path;
    }

}