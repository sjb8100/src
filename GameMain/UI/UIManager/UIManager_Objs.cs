/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIManager_Objs
 * 版本号：  V1.0.0.0
 * 创建时间：9/1/2017 11:41:41 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class UIManager
{
    #region Gameobjs
    /// <summary>
    /// 获取对象（异步）
    /// </summary>
    /// <param name="resID">资源ID</param>
    /// <param name="te">回调</param>
    /// <param name="param">参数</param>
    public static void GetObjAsyn(uint resID, CMResEvent<Transform> te, object param1 = null,object param2 = null,object param3 = null)
    {
        DataManager.Manager<CMObjPoolManager>().SpawnObjAsyn(resID, te, param1,param2,param3);
    }

    /// <summary>
    /// 获取对象（同步）
    /// </summary>
    /// <param name="resID">资源ID</param>
    /// <returns></returns>
    public static Transform GetObj(uint resID)
    {
        return DataManager.Manager<CMObjPoolManager>().SpawnObj(resID);
    }

    public static Transform GetObj(GridID gridID)
    {
        return GetObj((uint)gridID);
    }

    public static GameObject GetResGameObj(uint resID)
    {
        table.UIResourceDataBase resDB = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        return GetResGameObj(resDB.assetbundlePath, resDB.resRelativePath);
    }

    public static GameObject GetResGameObj(GridID id)
    {
        return GetResGameObj((uint)id);
    }

    public static GameObject GetResGameObj(string abPath,string assetName)
    {
        return DataManager.Manager<CMResourceMgr>().GetGameObj(abPath, assetName).GetGameObj();
    }
    #endregion

    #region Texture
    /// <summary>
    /// 获取贴图（异步）
    /// </summary>
    /// <param name="resID"></param>
    /// <param name="tex"></param>
    /// <param name="te"></param>
    /// <param name="param"></param>
    private static void GetTextureAsyn(uint resID, ref CMResAsynSeedData<CMTexture> seedData, CMResEvent<CMTexture> te, object param1 = null,object param2 = null,object param3 = null)
    {
        table.UIResourceDataBase rsDb = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        if (null == rsDb)
        {
            if (null != te)
            {
                te.Invoke(null, param1,param2,param3);
            }
            return;
        }

        DataManager.Manager<CMResourceMgr>().GetTextureAsyn(rsDb.assetbundlePath, rsDb.resRelativePath,
            ref seedData, te,
            rsDb.idle2releaseTime, 10f,
            param1, param2, param3);
    }

    /// <summary>
    /// 获取CMTexture异步
    /// </summary>
    /// <param name="resID"></param>
    /// <param name="seedData"></param>
    public static void GetTextureAsyn(uint resID, ref CMResAsynSeedData<CMTexture> seedData,Action releaseAction,UITexture tex,bool makeperfect = true)
    {
        if (null == seedData)
        {
            seedData = new CMResAsynSeedData<CMTexture>(releaseAction);
        }
        GetTextureAsyn(resID, ref seedData, OnGetCMTextureDlg, param1: tex,param2:makeperfect);
    }

    public static void GetTextureAsyn(string iconName, ref CMResAsynSeedData<CMTexture> seedData, Action releaseAction, UITexture tex, bool makeperfect = true)
    {
        uint resID = DataManager.Manager<UIManager>().GetResIDByFileName(false, iconName);
        GetTextureAsyn(resID, ref seedData, releaseAction, tex, makeperfect);
    }

    /// <summary>
    /// 获取贴图（同步）
    /// </summary>
    /// <param name="resID"></param>
    /// <returns></returns>
    public static CMTexture GetTexture(uint resID)
    {
        table.UIResourceDataBase rsDb = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        if (null == rsDb)
        {
            return null;
        }
        return DataManager.Manager<CMResourceMgr>().GetTexture(rsDb.assetbundlePath, rsDb.resRelativePath, rsDb.idle2releaseTime);
    }

    public static IUITexture GetTexture(TextureID texID)
    {
        return GetTexture((uint)texID);
    }
    
    #endregion

    #region Atlas
    /// <summary>
    /// 获取图集（异步）
    /// </summary>
    /// <param name="resID"></param>
    /// <param name="atlas"></param>
    /// <param name="ae"></param>
    /// <param name="param"></param>
    public static void GetAtlasAsyn(uint resID, ref CMResAsynSeedData<CMAtlas> seedData, CMResEvent<CMAtlas> createDlg, 
        object param1 = null, object param2 = null, object param3 = null)
    {
        table.UIResourceDataBase rsDb = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        if (null == rsDb)
        {
            if (null != createDlg)
            {
                createDlg.Invoke(null, param1, param2, param3);
            }
            return;
        }
        DataManager.Manager<CMResourceMgr>().GetAtlasAsyn(rsDb.assetbundlePath, rsDb.resRelativePath, 
            ref seedData, createDlg,
            rsDb.idle2releaseTime,10 
            ,param1, param2, param3);
    }

    public static void GetAtlasAsyn(uint resID, ref CMResAsynSeedData<CMAtlas> seedData, Action releaseAction,
        UISprite sprite, string iconName, bool makePerfect = true)
    {
        if (null != seedData)
        {
            seedData.Release(true);
            seedData = null;
        }
        if (null == seedData)
        {
            seedData = new CMResAsynSeedData<CMAtlas>(releaseAction);
        }
        GetAtlasAsyn(resID, ref seedData, OnGetCMAtlasAsynDlg, sprite, iconName, makePerfect);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iconName"></param>
    /// <param name="seedData"></param>
    /// <param name="releaseAction"></param>
    /// <param name="sprite"></param>
    /// <param name="makePerfect"></param>
    public static void GetAtlasAsyn(string iconName, ref CMResAsynSeedData<CMAtlas> seedData, Action releaseAction,
        UISprite sprite, bool makePerfect = true)
    {
        uint resId = UIManager.Instance.GetResIDByFileName(true, iconName);
        if (resId == 0)
        {
            Engine.Utility.Log.Error("UIManager-> failed,resid is zero,iconName:{0}", iconName);
            return;
        }
        if (null != seedData)
        {
            seedData.Release(true);
            seedData = null;
        }
        if (null == seedData)
        {
            seedData = new CMResAsynSeedData<CMAtlas>(releaseAction);
        }
        GetAtlasAsyn(resId, ref seedData, OnGetCMAtlasAsynDlg, sprite, iconName, makePerfect);
    }
    public static void GetQualityAtlasAsyn(uint qua,ref CMResAsynSeedData<CMAtlas> seedData, Action releaseAction,
        UISprite sprite, bool makePerfect = true)
    {
        string name =  ItemDefine.GetItemBorderIcon(qua);
        GetAtlasAsyn(name, ref seedData, releaseAction, sprite, makePerfect);
    }
    /// <summary>
    /// 获取图集（同步）
    /// </summary>
    /// <param name="resID"></param>
    /// <returns></returns>
    public static CMAtlas GetAtlas(uint resID)
    {
        table.UIResourceDataBase rsDb = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        if (null == rsDb)
        {
            return null;
        }
        return DataManager.Manager<CMResourceMgr>().GetAtlas(rsDb.assetbundlePath, rsDb.resRelativePath, rsDb.idle2releaseTime, 10);
    }

    //public static CMAtlas GetAtlas(AtlasID atlasID)
    //{
    //    return GetAtlas((uint)atlasID);
    //}
    #endregion

    #region Font
    /// <summary>
    ///  获取字图（异步）
    /// </summary>
    /// <param name="resID"></param>
    /// <param name="font"></param>
    /// <param name="fe"></param>
    /// <param name="param"></param>
    public static void GetFontAsyn(uint resID, ref CMResAsynSeedData<CMFont> seedData, CMResEvent<CMFont> fe, object param = null)
    {
        table.UIResourceDataBase rsDb = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        if (null == rsDb)
        {
            if (null != fe)
            {
                fe.Invoke(null, param,null,null);
            }
            return;
        }

        DataManager.Manager<CMResourceMgr>().GetFontAsyn(rsDb.assetbundlePath, rsDb.resRelativePath, ref seedData, fe, rsDb.idle2releaseTime, 10, param);
    }

    /// <summary>
    /// 获取字体（同步）
    /// </summary>
    /// <param name="resID"></param>
    /// <returns></returns>
    public static IUIFont GetFont(uint resID)
    {
        table.UIResourceDataBase rsDb = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        if (null == rsDb)
        {
            return null;
        }
        return DataManager.Manager<CMResourceMgr>().GetFont(rsDb.assetbundlePath, rsDb.resRelativePath, rsDb.idle2releaseTime, 10);
    }

    public static IUIFont GetFont(FontID fontID)
    {
        return GetFont((uint)fontID);
    }
    #endregion

    #region Objs Create Release Dlg

    public static void ReleaseObjs(uint resID,Transform trans)
    {
        table.UIResourceDataBase db = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        if (null == db)
        {
            Engine.Utility.Log.Error("UIManager->ReleaseObjs failed, assetDB not found resId =",resID);
            return ;
        }
        DataManager.Manager<CMObjPoolManager>().DespawnObj(db.resRelativePath, trans);
    }

    public static Transform OnObjsCreate(uint resID)
    {
        return GetObj(resID);
    }


    public static void OnObjsRelease(Transform ts,uint resID)
    {
        ReleaseObjs(resID, ts);
    }
    #endregion

    #region AssetsGet Delegate

    /// <summary>
    /// 异步获取CMAtlas回调
    /// </summary>
    /// <param name="atlas"></param>
    /// <param name="param1"></param>
    /// <param name="parma2"></param>
    /// <param name="param3"></param>
    private static void OnGetCMAtlasAsynDlg(IUIAtlas atlas, object param1, object param2, object param3)
    {
        if (null != param1 && param1 is UISprite)
        {
            UISprite sprite = (UISprite)param1;
            UIAtlas atl = (null != atlas) ? atlas.GetAtlas() : null;
            sprite.atlas = atl;
            if (null != param2 && param2 is string)
            {
                sprite.spriteName = (string)param2;
            }
            if (null != param3 && param3 is bool)
            {
                bool makePerfect = (bool)param3;
                if (null != sprite && makePerfect)
                    sprite.MakePixelPerfect();
            }
        }
    }

    /// <summary>
    /// 异步获取CMTexture回调
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    /// <param name="param3"></param>
    private static void OnGetCMTextureDlg(IUITexture tex, object param1, object param2, object param3)
    {
        if (null != param1 && param1 is UITexture)
        {
            UITexture uitexture = (UITexture)param1;
            Texture texture = (null != tex) ? tex.GetTexture() : null;
            uitexture.mainTexture = texture;
            if (null != param2 && param2 is bool)
            {
                bool makePerfect = (bool)param2;
                if (makePerfect && null != uitexture)
                    uitexture.MakePixelPerfect();
            }

        }
    }
    #endregion

    #region AddGrid
    public static Transform AddGridTransform(GridID gridID,Transform targetTrans)
    {
        Transform ts = null;
        if (null != targetTrans)
        {
            ts = UIManager.GetObj((uint)gridID);
            if (null != ts)
            {
                Util.AddChildToTarget(targetTrans, ts);
            }
        }

        return ts;
    }
    #endregion

}