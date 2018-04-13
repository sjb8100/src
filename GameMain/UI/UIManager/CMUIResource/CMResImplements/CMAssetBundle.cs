/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager.CMUIResource.CMResImplements
 * 创建人：  wenjunhua.zqgame
 * 文件名：  CMAssetBundle
 * 版本号：  V1.0.0.0
 * 创建时间：8/29/2017 11:03:09 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//资源加载完成回调
public delegate void AssetLoadFinsh(UnityEngine.Object obj,object param1,object param2,object param3);
class CMAssetLoadFinsh : CMBaseParam
{
    public string AssetName;
    public AssetLoadFinsh FinshDlg;
}

class CMAsset : IRefObjs
{
    #region property
    private string m_strAssetName = "";
    public string AssetName
    {
        get
        {
            return m_strAssetName;
        }
    }

    private UnityEngine.Object m_asset = null;
    public UnityEngine.Object Asset
    {
        get
        {
            return m_asset;
        }
    }

    private CMAssetBundle m_cmAB = null;

    private bool m_bInit = false;
    #endregion

    #region OP

    public CMAsset(string assetName,CMAssetBundle cmAssetBundle)
    {
        this.m_asset = null;
        this.m_strAssetName = assetName;
        m_cmAB = cmAssetBundle;
    }

    public void InitAsset(UnityEngine.Object asset)
    {
        m_bInit = true;
        this.m_asset = asset;
    }

    public bool Ready
    {
        get
        {
            return m_bInit;
        }
    }
    #endregion 

    public override void OnObjsRefChange2Zero()
    {
        base.OnObjsRefChange2Zero();
        UnLoad();
    }

    internal void UnLoad()
    {
        m_cmAB.UnloadAsset(m_strAssetName);
    }
}

public class CMAssetBundle : ICMABTask 
{
    #region property
    private Dictionary<string, CMAsset> m_dicAssets = null;
    private AssetBundle m_ab = null;

    private string m_abPath = "";
    public string ABPath
    {
        get
        {
            return m_abPath;
        }
    }

    
    #endregion

    #region ICMABTask
    private float m_progress = 0;
    private CMABTaskState m_taskState = CMABTaskState.CMTaskState_None;
    //进入等待列表
    public void OnWaitingStart()
    {
        m_taskState = CMABTaskState.CMTaskState_Waiting;
    }

    //进行中
    public void OnProcessing(float progress)
    {
        if (m_taskState != CMABTaskState.CMTaskState_Processing
            && (1 - progress > Mathf.Epsilon))
        {
            m_taskState = CMABTaskState.CMTaskState_Processing;
        }
        m_progress = progress;
        m_taskState = CMABTaskState.CMTaskState_Processing;
    }

    //完成
    public void Done(AssetBundle ab)
    {
        m_taskState = CMABTaskState.CMTaskState_Done;
        OnLoadComplete(ab);
    }

    //获取进度
    public float GetProgress()
    {
        return m_progress;
    }

    //是否完成
    public bool IsDone()
    {
        return (m_taskState == CMABTaskState.CMTaskState_Done);
    }

    //获取状态
    public CMABTaskState GetTaskState()
    {
        return m_taskState;
    }
    #endregion

    #region CMAsset
    private Dictionary<string, AssetBundleRequest> m_dicLoadAsset = null;
    private Dictionary<string, List<CMAssetLoadFinsh>> m_dicLoadAssetDlg = null;

    public void StartLoad()
    {

    }

    /// <summary>
    /// 添加资源
    /// </summary>
    /// <param name="callback"></param>
    private void AddAssetLoadComplteDlg(string assetName, AssetLoadFinsh callback, object param1 = null, object param2 = null,object param3 = null)
    {
        CMAssetLoadFinsh callbackData = new CMAssetLoadFinsh()
        {
            AssetName = assetName,
            FinshDlg = callback,
            Param1 = param1,
            Param2 = param2,
            Param3 = param3,
        };
        List<CMAssetLoadFinsh> datas = null;
        if (!m_dicLoadAssetDlg.TryGetValue(callbackData.AssetName, out datas))
        {
            datas = new List<CMAssetLoadFinsh>();
            m_dicLoadAssetDlg.Add(callbackData.AssetName, datas);
        }
        datas.Add(callbackData);
    }

    /// <summary>
    /// 是否资源已经准备好（加载完成）
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public bool IsAssetReady(string assetName)
    {
        return m_dicAssets.ContainsKey(assetName) && m_dicAssets[assetName].Ready;
    }

    /// <summary>
    /// 是否资源正在加载
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    private bool IsLoadingAsset(string assetName)
    {
        return m_dicLoadAsset.ContainsKey(assetName);
    }

    /// <summary>
    /// 是否正在加载资源
    /// </summary>
    /// <returns></returns>
    private bool IsLoadingAsset()
    {
        return m_dicLoadAsset.Count > 0;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="assetName"></param>
    private void LoadAssetAsyn(string assetName)
    {
        if (!string.IsNullOrEmpty(assetName) && IsDone() && !IsAssetReady(assetName) && !IsLoadingAsset(assetName))
        {
            if (null == m_ab)
            {
                return;
            }
            AssetBundleRequest abRequest = m_ab.LoadAssetAsync(assetName);
            m_dicLoadAsset.Add(assetName, abRequest);
        }
    }

    /// <summary>
    /// 检测缓存加载的资源
    /// </summary>
    private void CheckCacheToLoadAsset()
    {
        var enumerator = m_dicLoadAssetDlg.GetEnumerator();
        while(enumerator.MoveNext())
        {
            LoadAssetAsyn(enumerator.Current.Key);
        }
    }

    /// <summary>
    /// 资源加载完成
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <param name="assetObj">资源对象</param>
    private void OnAssetLoaded(string assetName,UnityEngine.Object assetObj)
    {
        CMAsset cmA = null;
        bool needInit = false;
        if (!m_dicAssets.TryGetValue(assetName, out cmA))
        {
            needInit = true;
            cmA = new CMAsset(assetName, this);
            m_dicAssets.Add(assetName, cmA);
        }else if (!cmA.Ready)
        {
            needInit = true;
        }

        if (needInit)
        {
            cmA.InitAsset(assetObj);
            //对依赖资源添加引用计数
            ReferenceDependenceAssset(assetName);
        }
    }

    /// <summary>
    /// 尝试获取资源
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="assetObj"></param>
    /// <returns></returns>
    private bool TryGetAsset(string assetName,out CMAsset cmasset)
    {
        return m_dicAssets.TryGetValue(assetName, out cmasset);
    }

    /// <summary>
    /// 回调资源加载完成
    /// </summary>
    /// <param name="assetName"></param>
    private void InvokeAssetLoadDlg(string assetName)
    {
        List<CMAssetLoadFinsh> callbacks = null;
        if (m_dicLoadAssetDlg.TryGetValue(assetName,out callbacks))
        {
            CMAsset cmasset = null;

            if (TryGetAsset(assetName, out cmasset))
            {
                for (int i = 0, max = callbacks.Count; i < max;i++ )
                {
                    if (null == callbacks[i].FinshDlg)
                        continue;
                    callbacks[i].FinshDlg.Invoke(cmasset.Asset, callbacks[i].Param1, callbacks[i].Param2, callbacks[i].Param3);
                }
            }
            m_dicLoadAssetDlg.Remove(assetName);
        }
    }

    private void InvokeAssetLoadAllDlg()
    {

    }

    //加载完成列表
    private List<string> completeAsset = new List<string>(); 
    private void ProcessAssetLoadAsyn()
    {
        if (IsLoadingAsset())
        {
            var enumerator = m_dicLoadAsset.GetEnumerator();
            while(enumerator.MoveNext())
            {
                if (!enumerator.Current.Value.isDone)
                {
                    continue;
                }
                OnAssetLoaded(enumerator.Current.Key, enumerator.Current.Value.asset);
                completeAsset.Add(enumerator.Current.Key);
            }
            if (completeAsset.Count > 0)
            {
                for (int i = 0, max = completeAsset.Count; i < max;i++ )
                {
                    if (m_dicLoadAsset.ContainsKey(completeAsset[i]))
                        m_dicLoadAsset.Remove(completeAsset[i]);
                }
                //completeAsset.Clear();
            }
        }
        
    }
    #endregion


    #region Create
    private CMAssetBundle (string abPath)
    {
        this.m_abPath = abPath;
        m_dicAssets = new Dictionary<string, CMAsset>();
        m_taskState = CMABTaskState.CMTaskState_None;
        m_dicLoadAssetDlg = new Dictionary<string, List<CMAssetLoadFinsh>>();
        m_dicLoadAsset = new Dictionary<string, AssetBundleRequest>();
    }

    public static CMAssetBundle Create(string path)
    {
        return new CMAssetBundle(path);
    }

    public void CreateAllAsset()
    {

    }

    /// <summary>
    /// 穿件资源异步
    /// </summary>
    ///<param name="preLoad">是否为预加载</param>
    /// <param name="assetName"></param>
    /// <param name="callback"></param>
    /// <param name="param"></param>
    public void CreateAssetAsyn(bool preLoad,string assetName, AssetLoadFinsh callback, object param1 = null, object param2 = null, object param3 = null)
    {
        AddAssetLoadComplteDlg(assetName,callback,param1,param2,param3);
        
        if (!preLoad)
            ReferenceAssset(assetName);

        if (IsDone())
        {
            if (IsAssetReady(assetName))
            {
                InvokeAssetLoadDlg(assetName);
            }else
            {
                CheckCacheToLoadAsset();
            }
        }
    }

    /// <summary>
    /// 创建资源（同步）
    /// </summary>
    public UnityEngine.Object CreateAsset(string assetName)
    {
        CMAsset cmasset = null;
        UnityEngine.Object obj = null;
        if (IsDone() && null != m_ab 
            && (!TryGetAsset(assetName, out cmasset) || !cmasset.Ready))
        {
            obj = m_ab.LoadAsset(assetName);
            
        }else if (null != cmasset && cmasset.Ready)
        {
            obj = cmasset.Asset;
        }
        //对本身添加引用计数
        ReferenceAssset(assetName);
        if (null != obj)
        {
            OnAssetLoaded(assetName, obj);
            InvokeAssetLoadDlg(assetName);
        }
        return obj;
    }

    /// <summary>
    /// 对assetName的依赖资源进行引用计数
    /// </summary>
    /// <param name="assetName"></param>
    public void ReferenceDependenceAssset(string assetName)
    {
        DataManager.Manager<CMAssetBundleLoaderMgr>().AddDependenceAssetRef(assetName);
    }

    /// <summary>
    /// 引用资源
    /// </summary>
    /// <param name="assetName">资源名称</param>
    public void ReferenceAssset(string assetName)
    {
        //if (!IsDone())
        //{
        //    Debug.LogError(string.Format("CMAssetBundle {0} Not Ready ",assetName));
        //    return ;
        //}
        CMAsset cmasset = null;
        if (!TryGetAsset(assetName, out cmasset))
        {
            cmasset = new CMAsset(assetName, this);
            m_dicAssets.Add(assetName, cmasset);
        }
        cmasset.AddRef();
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="assetName"></param>
    public void ReleaseAsset(string assetName)
    {
        CMAsset cmasset = null;
        if (TryGetAsset(assetName,out cmasset))
        {
            cmasset.RemoveRef();
        }
    }

    /// <summary>
    ///  卸载资源
    /// </summary>
    /// <param name="assetName"></param>
    public void UnloadAsset(string assetName)
    {
        CMAsset cmasset = null;
        if (TryGetAsset(assetName,out cmasset))
        {
            m_dicAssets.Remove(assetName);
            DataManager.Manager<CMAssetBundleLoaderMgr>().RemoveDependenceAssetRef(assetName);
            //Engine.Utility.Log.Info("CmAssetBundle->UnloadAsset {0}", assetName);
        }
    }
    #endregion

    #region OP
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {
        ProcessAssetLoadAsyn();
    }

    /// <summary>
    /// 是否有资源加载完成
    /// </summary>
    /// <returns></returns>
    public bool HasAssetLoadComplete()
    {
        return completeAsset.Count > 0;
    }

    /// <summary>
    /// 处理资源创建回掉
    /// </summary>
    public void HandleAssetCreateDlg()
    {
        if ( completeAsset.Count <= 0 || !DataManager.Manager<CMAssetBundleLoaderMgr>().IsDependenceLoadComplete(ABPath))
        {
            return;
        }
        for (int i = 0, max = completeAsset.Count; i < max; i++)
        {
            InvokeAssetLoadDlg(completeAsset[i]);
        }
        completeAsset.Clear();
    }

    #endregion

    
    #region Callback

    /// <summary>
    /// AssetBundle加载完成
    /// </summary>
    /// <param name="ab">AssetBundle</param>
    public void OnLoadComplete(AssetBundle ab)
    {
        m_ab = ab;
        OnFinish();
    }

    /// <summary>
    /// 执行完成后回调事件
    /// </summary>
    public void OnFinish()
    {
        CheckCacheToLoadAsset();
    }

    #endregion
}