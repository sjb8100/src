using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class CMResourceMgr : IManager
{
    #region property
    public const bool CM_RES_RECYCLE_TEST_MODE = true;
    public const float CM_RES_RECYCLE_TEST_TIME = 10f;
    public const float CM_RES_RECYCLE_TEST_OBJ_TIME = 10f;

    public const string CLASS_NAME = "CMResourceMgr";
    //释放空资源等待时间
    public const float TIME_TO_RELEASE_EMPTY_RES_OBJ = 10f;

    //资源本地数据
    private Dictionary<uint, CMResourceDefine.LocalResourceData> m_dicLocalResData = null;

    //图集
    private Dictionary<string, CMAtlas> m_atlas = null;
    //字体
    private Dictionary<string, CMFont> m_fonts = null;
    //预制对象
    private Dictionary<string, CMObj> m_objs = null;
    //贴图
    private Dictionary<string, CMTexture> m_texs = null;

    private StringBuilder strBuilder = new StringBuilder();
    #endregion

    #region IManager Method
    public void Initialize()
    {
        m_dicLocalResData = new Dictionary<uint, CMResourceDefine.LocalResourceData>();

        m_atlas = new Dictionary<string, CMAtlas>();

        m_fonts = new Dictionary<string, CMFont>();

        m_objs = new Dictionary<string, CMObj>();

        m_texs = new Dictionary<string, CMTexture>();


        List<table.UIResourceDataBase> resList = GameTableManager.Instance.GetTableList<table.UIResourceDataBase>();
        if (null != resList)
        {
            table.UIResourceDataBase db = null;
            CMResourceDefine.LocalResourceData localData = null;
            for (int i = 0, max = resList.Count; i < max; i++)
            {
                db = resList[i];
                localData = CMResourceDefine.LocalResourceData.Create(db.ID);
                m_dicLocalResData.Add(db.ID, localData);
            }
        }
    }

    public void Reset(bool depthClearData = false)
    {

    }

    public void Process(float deltaTime)
    {
        if (ProcessRecycle(false))
        {
            GameApp.Instance().UnloadUnusedAsset();
        }
    }

    public void ClearData()
    {

    }
    #endregion

    #region Recycle

    private List<string> cmTempList = new List<string>();
    /// <summary>
    /// 释放Release状态的空资源对象
    /// </summary>
    /// <param name="force"></param>
    private void ProcessReleaseEmptyResObj(bool force = false)
    {
        cmTempList.Clear();
        //Atlas
        var atlasEntor = m_atlas.GetEnumerator();
        while (atlasEntor.MoveNext())
        {
            if (atlasEntor.Current.Value.CanRemoveFromMgr(force))
            {
                cmTempList.Add(atlasEntor.Current.Key);
            }
        }
        if (cmTempList.Count > 0)
        {
            for (int i = 0, max = cmTempList.Count; i < max; i++)
            {
                if (string.IsNullOrEmpty(cmTempList[i]))
                    continue;
                m_atlas.Remove(cmTempList[i]);
            }
            cmTempList.Clear();
        }
        
        //Font
        var fontEntor = m_fonts.GetEnumerator();
        while (fontEntor.MoveNext())
        {
            if (fontEntor.Current.Value.CanRemoveFromMgr(force))
            {
                cmTempList.Add(atlasEntor.Current.Key);
            }
        }
        if (cmTempList.Count > 0)
        {
            for (int i = 0, max = cmTempList.Count; i < max; i++)
            {
                if (string.IsNullOrEmpty(cmTempList[i]))
                    continue;
                m_fonts.Remove(cmTempList[i]);
            }
            cmTempList.Clear();
        }

        //GameObj
        var objsEntor = m_objs.GetEnumerator();
        while (objsEntor.MoveNext())
        {
            if (objsEntor.Current.Value.CanRemoveFromMgr(force))
            {
                cmTempList.Add(atlasEntor.Current.Key);
            }
        }
        if (cmTempList.Count > 0)
        {
            for (int i = 0, max = cmTempList.Count; i < max; i++)
            {
                if (string.IsNullOrEmpty(cmTempList[i]))
                    continue;
                m_objs.Remove(cmTempList[i]);
            }
            cmTempList.Clear();
        }

        //Texture
        var texsEntor = m_texs.GetEnumerator();
        while (texsEntor.MoveNext())
        {
            if (texsEntor.Current.Value.CanRemoveFromMgr(force))
            {
                cmTempList.Add(atlasEntor.Current.Key);
            } 
        }
        if (cmTempList.Count > 0)
        {
            for (int i = 0, max = cmTempList.Count; i < max; i++)
            {
                if (string.IsNullOrEmpty(cmTempList[i]))
                    continue;
                m_texs.Remove(cmTempList[i]);
            }
            cmTempList.Clear();
        }
    }

    /// <summary>
    /// 释放处于等待状态的资源
    /// </summary>
    /// <param name="force"></param>
    private bool ProcessReleaseIdleResObj(bool force = false)
    {
        bool releaseSuccess = false;
        var atlasEntor = m_atlas.GetEnumerator();
        while(atlasEntor.MoveNext())
        {
            if (atlasEntor.Current.Value.CanRelease(force))
            {
                atlasEntor.Current.Value.Release(force);
                releaseSuccess = true;
            }
        }

        var fontEntor = m_fonts.GetEnumerator();
        while (fontEntor.MoveNext())
        {
            if (fontEntor.Current.Value.CanRelease(force))
            {
                fontEntor.Current.Value.Release(force);
                releaseSuccess = true;
            }
        }

        var texsEntor = m_texs.GetEnumerator();
        while (texsEntor.MoveNext())
        {
            if (texsEntor.Current.Value.CanRelease(force))
            {
                texsEntor.Current.Value.Release(force);
                releaseSuccess = true;
            }
        }

        var objsEntor = m_objs.GetEnumerator();
        while (objsEntor.MoveNext())
        {
            if (objsEntor.Current.Value.CanRelease(force))
            {
                objsEntor.Current.Value.Release(force);
                releaseSuccess = true;
            }
        }

        return releaseSuccess;
    }

    /// <summary>
    /// 执行回收
    /// </summary>
    /// <param name="force">是否强制回收没有引用的资源</param>
    /// <returns></returns>
    private bool ProcessRecycle(bool force = false)
    {
        bool recycleSuccess = ProcessReleaseIdleResObj(force);
        ProcessReleaseEmptyResObj(force);
        return recycleSuccess;
    }
    #endregion

    #region Get
    /// <summary>
    /// 获取图集（异步）
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <param name="atlas"></param>
    /// <param name="createDlg"></param>
    /// <param name="custParam"></param>
    /// <returns></returns>
    public bool GetAtlasAsyn(string abPath, string assetName, ref CMResAsynSeedData<CMAtlas> seedData, CMResEvent<CMAtlas> createDlg, float timeIdleStateKeep = 120, float timeRelaseStateKeep = 10, object param1 = null, object param2 = null, object param3 = null)
    {
        CMAtlas cmatlas = null;
        if (!m_atlas.TryGetValue(assetName, out cmatlas))
        {
            cmatlas = new CMAtlas(abPath, assetName, timeIdleStateKeep,timeRelaseStateKeep);
            m_atlas.Add(assetName, cmatlas);
        }
        cmatlas.GetCMResourceAsyn(ref seedData, createDlg, param1, param2, param3);
        return true;
    }

    /// <summary>
    /// 获取图集同步
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <param name="atlas"></param>
    /// <param name="createDlg"></param>
    /// <param name="custParam"></param>
    /// <returns></returns>
    public CMAtlas GetAtlas(string abPath, string assetName, float timeIdleStateKeep = 120, float timeRelaseStateKeep = 10)
    {
        CMAtlas cmatlas = null;
        if (!m_atlas.TryGetValue(assetName, out cmatlas))
        {
            cmatlas = new CMAtlas(abPath, assetName, timeRelaseStateKeep, timeRelaseStateKeep);
            m_atlas.Add(assetName, cmatlas);
        }
        return cmatlas.GetCMResource();
    }

    /// <summary>
    /// 获取贴图（异步）
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <param name="tex"></param>
    /// <param name="createDlg"></param>
    /// <param name="custParam"></param>
    /// <returns></returns>
    public bool GetTextureAsyn(string abPath, string assetName, ref CMResAsynSeedData<CMTexture> seedData, CMResEvent<CMTexture> createDlg, float timeIdleStateKeep = 120, float timeRelaseStateKeep = 10, object param1 = null, object param2 = null, object param3 = null)
    {
        CMTexture cmtex = null;
        if (!m_texs.TryGetValue(assetName, out cmtex))
        {
            cmtex = new CMTexture(abPath, assetName, timeIdleStateKeep,timeRelaseStateKeep);
            m_texs.Add(assetName, cmtex);
        }
        cmtex.GetCMResourceAsyn(ref seedData, createDlg, param1, param2, param3);
        return true;
    }

    /// <summary>
    ///  获取贴图（同步）
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public CMTexture GetTexture(string abPath, string assetName,float timeIdleStateKeep = 120, float timeRelaseStateKeep = 10)
    {
        CMTexture cmtex = null;
        if (!m_texs.TryGetValue(assetName, out cmtex))
        {
            cmtex = new CMTexture(abPath, assetName, timeIdleStateKeep, timeRelaseStateKeep);
            m_texs.Add(assetName, cmtex);
        }
        return cmtex.GetCMResource();
    }

    /// <summary>
    /// 获取字体(异步)
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <param name="font"></param>
    /// <param name="createDlg"></param>
    /// <param name="custParam"></param>
    /// <returns></returns>
    public bool GetFontAsyn(string abPath, string assetName, ref CMResAsynSeedData<CMFont> seedData, CMResEvent<CMFont> createDlg, float timeIdleStateKeep = 120, float timeRelaseStateKeep = 10, object param1 = null, object param2 = null, object param3 = null)
    {
        CMFont cmfont = null;
        if (!m_fonts.TryGetValue(assetName, out cmfont))
        {
            cmfont = new CMFont(abPath,assetName,timeIdleStateKeep,timeRelaseStateKeep);
            m_fonts.Add(assetName, cmfont);
        }
        cmfont.GetCMResourceAsyn(ref seedData, createDlg, param1, param2, param3);
        return true;
    }

    /// <summary>
    /// 获取字体(同步)
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public IUIFont GetFont(string abPath, string assetName, float timeIdleStateKeep = 120, float timeRelaseStateKeep = 10)
    {
        CMFont cmfont = null;
        if (!m_fonts.TryGetValue(assetName, out cmfont))
        {
            cmfont = new CMFont(abPath,assetName,timeIdleStateKeep,timeRelaseStateKeep);
            m_fonts.Add(assetName, cmfont);
        }
        return cmfont.GetCMResource();
    }
    private CMResAsynSeedData<CMObj> cmResAsynSeed = null;
    /// <summary>
    /// 获取游戏对象（异步）
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <param name="obj"></param>
    /// <param name="createDlg"></param>
    /// <param name="custParam"></param>
    /// <returns></returns>
    public bool GetGameObjAsyn(string abPath, string assetName, CMResEvent<CMObj> createDlg,object param1 = null, object param2 = null, object param3 = null)
    {
        CMObj cmObj = null;
        if (!m_objs.TryGetValue(assetName, out cmObj))
        {
            cmObj = new CMObj(abPath,assetName);
            m_objs.Add(assetName, cmObj);
        }
        cmResAsynSeed = null;
        cmObj.GetCMResourceAsyn(ref cmResAsynSeed, createDlg, param1, param2, param3);
        return true;
    }

    /// <summary>
    /// 获取游戏对象（同步）
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public CMObj GetGameObj(string abPath, string assetName)
    {
        CMObj cmObj = null;
        if (!m_objs.TryGetValue(assetName, out cmObj))
        {
            cmObj = new CMObj(abPath,assetName);
            m_objs.Add(assetName, cmObj);
        }
        return cmObj.GetCMResource();
    }

    public bool TryGetCMObj(string assetName,out CMObj cmObj)
    {
        return m_objs.TryGetValue(assetName, out cmObj);
    }
    #endregion

    #region LowMemory

    /// <summary>
    /// 低内存调用
    /// </summary>
    public bool OnLowMemory()
    {
        bool success = ProcessRecycle(true);
        return success;
    }
    #endregion

    #region ResDataOp(资源数据操作)

    /// <summary>
    /// 获取本地资源数据
    /// </summary>
    /// <param name="resID"></param>
    /// <returns></returns>
    public CMResourceDefine.LocalResourceData GetLocalResourceData(uint resID)
    {
        return (m_dicLocalResData.ContainsKey(resID) ? m_dicLocalResData[resID] : null);
    }
    #endregion


    #region Static

    /// <summary>
    /// 获取资源路径
    /// </summary>
    /// <param name="resId"></param>
    /// <returns></returns>
    public static string GetResPathByResID(uint resId)
    {
        CMResourceDefine.LocalResourceData ld = DataManager.Manager<CMResourceMgr>().GetLocalResourceData(resId);
        return (null != ld) ? ld.ResRelativePath : "";
    }

    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="resId"></param>
    /// <returns></returns>
    public static byte[] ReadFile(uint resId)
    {
        return ReadFile(GetResPathByResID(resId));
    }

    /// <summary>
    /// 读取文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static byte[] ReadFile(string path)
    {
        byte[] buffers = Engine.Utility.FileUtils.Instance().ReadFile("ui/" + path);
        return buffers;
    }

    #endregion

}