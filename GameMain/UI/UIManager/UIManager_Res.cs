/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIManager_Define
 * 版本号：  V1.0.0.0
 * 创建时间：10/30/2017 10:40:11 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

partial class UIManager
{
    public enum UISceneState
    {
        USS_None,
        USS_Login = 1,
        USS_Ingame = 2,
    }

    private UISceneState m_curSate = UISceneState.USS_None;

    public void ChangeUISceneState(UISceneState state,Action<float> progressCallback = null,bool force = true)
    {
        if (m_curSate == state && !force)
        {
            Engine.Utility.Log.Error("UIManager->ChangeUISceneState failed,already state:{0}",state);
            return;
        }

        if (m_curSate != UISceneState.USS_None)
            OnSceneStateOut(m_curSate);
        OnSceneStateEnter(state, progressCallback);

    }

    /// <summary>
    ///进度
    /// </summary>
    private float m_progress = 0;
    public float Progress
    {
        get
        {
            return m_progress;
        }
    }

    private Action<float> m_progressCallback = null;
    /// <summary>
    /// 资源预加载回调
    /// </summary>
    /// <param name="progress"></param>
    private void OnResPreLoad(float progress)
    {
        Engine.Utility.Log.Info("PreLoadRes Progress:" + progress);
        m_progress = progress;
        if (null != m_progressCallback)
        {
            m_progressCallback.Invoke(progress);
            if (1 - progress <= UnityEngine.Mathf.Epsilon)
            {
                m_progressCallback = null;
            }
        }



    }

    /// <summary>
    /// 是否预加载标示匹配当前
    /// </summary>
    /// <param name="preLoadMask"></param>
    /// <returns></returns>
    private bool IsPreLoadMaskMatch(uint preLoadMask)
    {
        return (m_curSate != UISceneState.USS_None && ((uint)m_curSate & preLoadMask) != 0);
    }

    /// <summary>
    /// 预加载
    /// </summary>
    private void DoPreLoadSceneUI()
    {
        int max = 0;
        table.UIResourceDataBase tempRes = null;
        //1、获取预加载资源列表
        List<uint> needPreLoadRes = new List<uint>();
        List<table.UIResourceDataBase> resList = GameTableManager.Instance.GetTableList<table.UIResourceDataBase>();
        if (null != resList)
        {
            for (int i = 0, maxR = resList.Count; i < maxR; i++)
            {
                tempRes = resList[i];
                if (IsPreLoadMaskMatch(tempRes.resPreloadMask))
                {
                    needPreLoadRes.Add(tempRes.ID);
                }
            }
            max = needPreLoadRes.Count;
        }
        //取消预加载
        //max = 0;
        //2、执行预加载
        if (max == 0)
        {
            OnResPreLoad(1f);
            return;
        }
        int loadCount = 0;
        AssetLoadFinsh callBack = (resObj, param1, param2, parm3) =>
            {
                uint resID =0;
                if (null != param1 && param1 is uint)
                {
                    resID = (uint)param1;
                    if (needPreLoadRes.Contains(resID))
                    {
                        loadCount++;
                        OnResPreLoad((float)loadCount / max);
                    }
                }
                if (null == resObj)
                {
                    Engine.Utility.Log.Error("UIManager->Preload AssetError ResID:{0}", resID);
                }
                
            };
        for (int i = 0; i < max;i++ )
        {
            tempRes = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(needPreLoadRes[i]);
            if (null == tempRes)
            {
                callBack.Invoke(null, needPreLoadRes[i],null,null);
                continue;
            }
            DataManager.Manager<CMAssetBundleLoaderMgr>().CreateAssetAsyn(true,tempRes.assetbundlePath, tempRes.resRelativePath, callBack,tempRes.ID);
        }
    }

    /// <summary>
    /// 释放保存状态不匹配当前UISceneState的资源
    /// </summary>
    private void ReleaseNotMatchSceneStateRes()
    {

    }

    /// <summary>
    /// UISceneSate退出回调
    /// </summary>
    /// <param name="state"></param>
    private void OnSceneStateOut(UISceneState state)
    {
        Engine.Utility.Log.Info("UIManager->OnSceneStateOut state:{0}", state);
    }

    /// <summary>
    /// UISceneState进入回调
    /// </summary>
    /// <param name="state"></param>
    private void OnSceneStateEnter(UISceneState state,Action<float> progressCallback = null)
    {
        Engine.Utility.Log.Info("UIManager->OnSceneStateEnter state:{0}", state);
        m_curSate = state;
        m_progress = 0;
        ReleaseNotMatchSceneStateRes();
        m_progressCallback = progressCallback;
        DoPreLoadSceneUI();
    }




}