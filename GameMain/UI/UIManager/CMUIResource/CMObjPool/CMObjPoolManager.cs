/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager.CMObjPool
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ObjPoolManager
 * 版本号：  V1.0.0.0
 * 创建时间：8/31/2017 11:01:05 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CMObjPoolManager : IManager
{
    #region property
    private Transform m_poolRoot = null;
    private Dictionary<string, CMObjPool> m_pools = null;
    #endregion

    #region IManager Method
    public void Initialize()
    {
        Transform parent = UIRootHelper.Instance.StretchTransRoot;
        m_poolRoot = new GameObject("ObjPoolRoot").transform;
        m_poolRoot.parent = parent;
        m_poolRoot.gameObject.layer = parent.gameObject.layer;
        m_poolRoot.transform.localScale = Vector3.one;
        m_poolRoot.transform.localPosition = new Vector3(0, 0, 10000);
        m_poolRoot.transform.localRotation = Quaternion.identity;
        m_pools = new Dictionary<string, CMObjPool>();
        GameObject.DontDestroyOnLoad(m_poolRoot);
    }

    public void Reset(bool depthClearData = false)
    {
        
    }
    private List<string> clearPool = new List<string>();
    public void Process(float deltaTime)
    {
        var enumerator = m_pools.GetEnumerator();
        while (enumerator.MoveNext())
        {
            enumerator.Current.Value.Process(deltaTime);
            if (enumerator.Current.Value.State == CMObjPool.CMObjPoolState.Release)
            {
                clearPool.Add(enumerator.Current.Key);
            }
        }

        for (int i = 0, max = clearPool.Count; i < max; i++)
        {
            RemovePool(clearPool[i]);
        }
        clearPool.Clear();
    }

    public void ClearData()
    {

    }
    #endregion

    #region SpawnObj

    /// <summary>
    /// 
    /// </summary>
    /// <param name="resID"></param>
    /// <param name="tsEvent"></param>
    /// <param name="param"></param>
    public void SpawnObjAsyn(uint resID, CMResEvent<Transform> tsEvent, object param1 = null, object param2 = null, object param3 = null)
    {
        table.UIResourceDataBase rsDb = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        if (null == rsDb)
        {
            if (null != tsEvent)
            {
                tsEvent.Invoke(null,param1,param2,param3);
            }
            return ;
        }

        CMObjPool pool = null;
        if (m_pools.TryGetValue(rsDb.resRelativePath, out pool) && pool.State != CMObjPool.CMObjPoolState.Release)
        {
            if (null != tsEvent)
            {
                tsEvent.Invoke(pool.SpawnInstance(Vector3.zero, Quaternion.identity, true),param1,param2,param3);
            }

        }
        else
        {
            if (null != pool)
            {
                pool = null;
                RemovePool(rsDb.resRelativePath);
            }
            DataManager.Manager<CMResourceMgr>().GetGameObjAsyn(rsDb.assetbundlePath, rsDb.resRelativePath,(obj, passParam1,passParam2,passParam3) =>
            {
                CMObjPool newpool = CreatePool(
                    rsDb.resRelativePath
                    , obj.GetGameObj().transform
                    ,rsDb.resKeepType != 2
                    ,(int)rsDb.resPreloadNum
                    , (int)rsDb.resCloneNumLimit
                    , (rsDb.releaseCullAboveMask != 0)
                    ,(int)rsDb.releaseCullAboveNum
                    ,(int)rsDb.resIdleKeepNum
                    ,rsDb.idle2releaseTime
                    ,rsDb.active2idleTime
                    , (rsDb.cloneAddScriptState == 1) ? rsDb.resName : "");
                if (null != tsEvent)
                {
                    tsEvent.Invoke(newpool.SpawnInstance(Vector3.zero, Quaternion.identity), passParam1, passParam2, passParam3);
                }
            }, param1,param2,param3);
        }
    }

    /// <summary>
    /// 获取实例（同步）
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    public Transform SpawnObj(uint resID,bool setActive =true)
    {
        table.UIResourceDataBase rsDb = GameTableManager.Instance.GetTableItem<table.UIResourceDataBase>(resID);
        if (null == rsDb)
        {
            
            return null;
        }
        Transform ts = null;
        CMObjPool pool = null;
        if (m_pools.TryGetValue(rsDb.resRelativePath, out pool) && pool.State != CMObjPool.CMObjPoolState.Release)
        {
            ts = pool.SpawnInstance(Vector3.zero, Quaternion.identity,setActive);
        }
        else
        {
            if (null != pool)
            {
                RemovePool(rsDb.resRelativePath);
            }
            IUIGameObj ig = DataManager.Manager<CMResourceMgr>().GetGameObj(rsDb.assetbundlePath, rsDb.resRelativePath);
            if (null == ig.GetGameObj())
            {
                Engine.Utility.Log.Error("ObjPoolManager->ig.GetGameObj() null, assetName:{0}", rsDb.resRelativePath);
                return null;
            }
            CMObjPool newpool = CreatePool(
                    rsDb.resRelativePath
                    , ig.GetGameObj().transform
                    ,rsDb.resKeepType != 2
                    ,(int)rsDb.resPreloadNum
                    , (int)rsDb.resCloneNumLimit
                    , (rsDb.releaseCullAboveMask != 0)
                    ,(int)rsDb.releaseCullAboveNum
                    ,(int)rsDb.resIdleKeepNum
                    ,rsDb.idle2releaseTime
                    ,rsDb.active2idleTime
                    , ((rsDb.cloneAddScriptState == 1) ? rsDb.resName : ""));
            ts = newpool.SpawnInstance(Vector3.zero, Quaternion.identity,setActive);
        }
        return ts;
    }


    /// <summary>
    /// 释放对象
    /// </summary>
    /// <param name="assetName">对象名称</param>
    /// <param name="t"></param>
    public void DespawnObj(string assetName,Transform t,bool setInactive = false)
    {
        CMObjPool pool = null;
        if (m_pools.TryGetValue(assetName, out pool))
        {

            pool.DespawnInstance(t, setInactive);
        }else
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="poolName"></param>
    /// <param name="ts"></param>
    /// <returns></returns>
    private CMObjPool CreatePool(
        string poolName
        , Transform ts
        ,bool needRelease = true
        ,int preLoadNum =0
        ,int maxHoldNum = 100
        ,bool cullNumEnable = true
        ,int cullNum = 20
        ,int maxKeepWhenIdle = 20
        ,float maxIdle2Release = 120f
        ,float maxActive2Idle = 60f
        , string addScriptName = "")
    {
        CMObjPool pool = null;
        if (!m_pools.TryGetValue(poolName, out pool))
        {
            pool = CMObjPool.Create(poolName, ts, needRelease, preLoadNum, maxHoldNum, cullNumEnable
        , cullNum, maxKeepWhenIdle, maxIdle2Release, maxActive2Idle, m_poolRoot, true,addScriptName,CMResourceMgr.CM_RES_RECYCLE_TEST_MODE);
            m_pools.Add(poolName, pool);
        }else
        {
            CMObj cmObj = null;
            if (DataManager.Manager<CMResourceMgr>().TryGetCMObj(poolName, out cmObj))
            {
                cmObj.RemoveRef();
            }
        }
        return pool;
    }

    /// <summary>
    /// 移除对象
    /// </summary>
    /// <param name="poolName"></param>
    /// <returns></returns>
    public bool RemovePool(string poolName)
    {
        bool success = false;
        if (m_pools.Remove(poolName))
        {
            CMObj cmObj = null;
            if (DataManager.Manager<CMResourceMgr>().TryGetCMObj(poolName,out cmObj))
            {
                cmObj.RemoveRef();
            }
            success = true;
        }
        return success;
    }

    #endregion

    #region LowMemory

    /// <summary>
    /// 低内存调用
    /// </summary>
    public bool OnLowMemory()
    {
        return false;
    }

    #endregion
}