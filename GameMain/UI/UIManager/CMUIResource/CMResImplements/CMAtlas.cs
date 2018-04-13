/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager.UIResource.IUIImplements
 * 创建人：  wenjunhua.zqgame
 * 文件名：  CMAtlas
 * 版本号：  V1.0.0.0
 * 创建时间：7/7/2017 11:05:53 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class CMAtlas : ICMResource<CMAtlas>, IUIAtlas
{
    #region property
    private UIAtlas m_atlas = null;
    #endregion

    #region atlas

    public CMAtlas(string abPath, string assetName, float timeIdleStateKeep = 120, float timeRelaseStateKeep = 10)
    {
        InitResource(abPath, assetName, timeIdleStateKeep: timeIdleStateKeep);
    }

    public UIAtlas GetAtlas()
    {
        if (null == m_atlas && null != ResObj)
        {
            GameObject gObj = ResObj as GameObject;
            m_atlas = gObj.GetComponent<UIAtlas>();
        }
        return m_atlas;
    }

    
    #endregion


    #region ICMResource

    protected override CMAtlas GetThis()
    {
        return this;
    }

    public override bool Release(bool force = false)
    {
        if (base.Release(force))
        {
            m_atlas = null;
            DataManager.Manager<CMAssetBundleLoaderMgr>().Release(AbPath, AssetName);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 加载完成
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="param"></param>
    protected override void OnLoadAssetComplte(UnityEngine.Object obj, object param1 = null, object param2 = null, object param3 = null)
    {
        if (null != obj)
        {
            GameObject gObj = obj as GameObject;
            m_atlas = gObj.GetComponent<UIAtlas>();
        }
        base.OnLoadAssetComplte(obj, param1, param2, param3);
    }
    #endregion
}