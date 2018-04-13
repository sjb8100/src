/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager.UIResource.IUIImplements
 * 创建人：  wenjunhua.zqgame
 * 文件名：  CMObj
 * 版本号：  V1.0.0.0
 * 创建时间：7/7/2017 11:09:28 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class CMObj : ICMResource<CMObj>, IUIGameObj
{
    #region property
    private GameObject m_gameObj = null;
    private List<CMEventBaseDelegate<IUIGameObj>> m_dlgs = new List<CMEventBaseDelegate<IUIGameObj>>();
    #endregion

    #region IUIGameObj

    public CMObj(string abPath,string assetName)
    {
        InitResource(abPath, assetName, timeIdleStateKeep: 0);
    }

    public GameObject GetGameObj()
    {
        if (null == m_gameObj && null != ResObj && ResObj is GameObject)
        {
            m_gameObj = (GameObject)ResObj;
        }
        return m_gameObj;
    }
    #endregion


    

    #region ICMResource

    public override bool Release(bool force = false)
    {
        if (base.Release(force))
        {
            m_gameObj = null;
            DataManager.Manager<CMAssetBundleLoaderMgr>().Release(AbPath, AssetName);
            return true;
        }
        return false;
    }

    protected override CMObj GetThis()
    {
        return this;
    }
    /// <summary>
    /// 加载完成
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="param"></param>
    protected override void OnLoadAssetComplte(UnityEngine.Object resObj,object param1 = null,object param2 = null,object param3 = null)
    {
        if (null != resObj && resObj is GameObject)
        {
            m_gameObj = (GameObject)resObj;
        }
        base.OnLoadAssetComplte(resObj, param1, param2, param3);
    }
    #endregion
}