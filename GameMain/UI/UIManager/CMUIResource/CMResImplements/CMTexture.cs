/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：
 * 命名空间：GameMain.UI.UIManager.UIResource.IUIImplements
 * 文件名：  CMTexture
 * 版本号：  V1.0.0.0
 * 唯一标识：8e579a35-5cf7-44e3-a4fe-005435ed1a9e
 * 当前的用户域：USER-20160526UC
 * 电子邮箱：XXXX@sina.cn
 * 创建时间：7/7/2017 11:00:53 AM
 * 描述：
 ************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public sealed class CMTexture : ICMResource<CMTexture>, IUITexture
{
    #region property
    private Texture m_tex = null;
    #endregion

    #region IUITexture
    public CMTexture(string abPath,string assetName,float timeIdleStateKeep = 120, float timeRelaseStateKeep = 10)
    {
        InitResource(abPath, assetName, timeIdleStateKeep, timeRelaseStateKeep);
    }
    public Texture GetTexture()
    {
        if (null == m_tex && null != ResObj && ResObj is Texture)
        {
            m_tex = (Texture)ResObj;
        }
        return m_tex;
    }

    #endregion


    #region CMResource
    public override bool Release(bool force = false)
    {
        if (base.Release(force))
        {
            m_tex = null;
            DataManager.Manager<CMAssetBundleLoaderMgr>().Release(AbPath, AssetName);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 返回本身
    /// </summary>
    /// <returns></returns>
    protected override CMTexture GetThis()
    {
        return this;
    }

    /// <summary>
    /// 加载完成
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="param"></param>
    protected override void OnLoadAssetComplte(UnityEngine.Object obj, object param1 = null, object param2 = null,object param3 = null)
    {
        if (null != obj && obj is Texture)
        {
            m_tex = (Texture)obj;
        }
        base.OnLoadAssetComplte(obj, param1, param2, param3);
    }
    #endregion
}
