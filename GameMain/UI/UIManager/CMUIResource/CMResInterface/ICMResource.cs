/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager.CMUIResource.CMResInterface
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ICMResource
 * 版本号：  V1.0.0.0
 * 创建时间：9/6/2017 5:05:55 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public delegate void CMResEvent<T>(T res, object param1, object param2, object param3);

public class CMResAsynSeedData<T> 
{
    private ICMResource<T> m_res;
    
    private int m_seed = 0;
    public int Seed
    {
        get
        {
            return m_seed;
        }
    }

    private Action m_releaseAction;
    public CMResAsynSeedData(Action releaseAction = null)
    {
        this.m_releaseAction = releaseAction;
    }

    public void Initialize(ICMResource<T> res,int seed,Action onRelease = null)
    {
        Release();
        this.m_res = res;
        this.m_seed = seed;
        if (null != onRelease)
        {
            this.m_releaseAction = onRelease;
        }
    }

    public bool IsRelease()
    {
        return m_seed == 0 && null == m_res;
    }

    public void Release(bool clearReleaseAction = false)
    {
        if (!IsRelease())
        {
            if (null != m_releaseAction)
            {
                m_releaseAction.Invoke();
            }
            m_res.ReleaseReference(m_seed);
            m_res = null;
            m_seed = 0;
        }

        if (null != m_releaseAction && clearReleaseAction)
        {
            m_releaseAction = null;
        }
    }

    public void Reset()
    {
        if (!IsRelease())
        {
            m_res.ReleaseReference(m_seed);
            m_res = null;
            m_seed = 0;
        }
    }
}

public class CMEventBaseDelegate<T> : CMBaseParam 
{
    public CMResEvent<T> Dlg;
}

public enum CMResState
{
    CMRS_None,
    //加载中
    CMRS_Loading,
    //加载完成
    CMRS_OnLoad,
    //活跃
    CMRS_Active,
    //等待
    CMRS_Idle,
    //已释放
    CMRS_Release,
}

public class CMBaseParam
{
    public object Param1;
    public object Param2;
    public object Param3;
}

public abstract class ICMResource<T> : IRefObjs
{
    //对象池时间戳
    public static float TimeStamp
    {
        get
        {
            return UnityEngine.Time.time;
        }
    }

    

    //CMRS_Release最大维持时间(秒)
    private float m_fTimeRelaseStateKeep = 10f;
    //CMRS_Idle最大维持时间(秒)
    private float m_fTimeIdleStateKeep = 120f;

    private float m_fTimeSinceRelease = 0;
    private float m_fTimdeSinceIdle = 0;

    //资源对象
    private UnityEngine.Object m_resObj = null;
    protected UnityEngine.Object ResObj
    {
        get
        {
            return m_resObj;
        }
    }

    private Dictionary<int, CMEventBaseDelegate<T>> m_dlgs = new Dictionary<int, CMEventBaseDelegate<T>>();

    private int m_asynSeed = 0;

    /// <summary>
    /// 资源状态
    /// </summary>
    protected CMResState m_state = CMResState.CMRS_None;
    public CMResState State
    {
        get
        {
            return m_state;
        }
    }

    protected string m_strAbPath = "";
    public string AbPath
    {
        get
        {
            return m_strAbPath;
        }
    }
    protected string m_strAssetName = "";
    public string AssetName
    {
        get
        {
            return m_strAssetName;
        }
    }

    protected void InitResource(string path, string name, float timeIdleStateKeep = 120, float timeRelaseStateKeep = 10)
    {
        if (string.IsNullOrEmpty(AbPath))
        {
            m_strAbPath = path;
        }

        if (string.IsNullOrEmpty(AssetName))
        {
            m_strAssetName = name;
        }

        this.m_fTimeIdleStateKeep = timeIdleStateKeep;
        this.m_fTimeRelaseStateKeep = timeRelaseStateKeep;
    }

    private void StructCMResAsynSeedData(ref CMResAsynSeedData<T> data)
    {
        if (null != data)
        {
            data.Reset();
        }

        if (null == data)
        {
            data = new CMResAsynSeedData<T>();
        }
        data.Initialize(this, ++m_asynSeed);
    }

    /// <summary>
    /// 预加载
    /// </summary>
    public void PreLoad()
    {

    }

    /// <summary>
    /// 是否当前资源加载完毕
    /// </summary>
    /// <returns></returns>
    public bool IsReady()
    {
        return !(State == CMResState.CMRS_None || State == CMResState.CMRS_Loading || State == CMResState.CMRS_Release);
    }

    public override void OnObjsRefChange2Zero()
    {
        base.OnObjsRefChange2Zero();
        //Engine.Utility.Log.Info("CMResource {0} OnObjsRefChange2Zero", AssetName);
        if( IsReady() && ChangeState(CMResState.CMRS_Idle))
        {
            m_dlgs.Clear();
        }
    }

    /// <summary>
    /// 改变资源状态
    /// </summary>
    /// <param name="state"></param>
    protected bool ChangeState(CMResState state)
    {
        if (this.m_state != state)
        {
            this.m_state = state;
            if (state == CMResState.CMRS_Idle)
            {
                m_fTimdeSinceIdle = ICMResource<T>.TimeStamp;
            }else if (state == CMResState.CMRS_Release)
            {
                m_fTimeSinceRelease = ICMResource<T>.TimeStamp;
            }
            
            return true;
        }
        return false;
    }

    protected virtual void OnLoadAssetComplte(UnityEngine.Object resObj,object param1 = null,object param2 = null,object param3 = null)
    {
        this.m_resObj = resObj;
        if (null == resObj)
        {
            Engine.Utility.Log.Error("ICMResource Load res failed,resName:{0},abName:{1}", m_strAssetName, m_strAbPath);
        }
        if (RefCount > 0)
        {
            ChangeState(CMResState.CMRS_Active);
        }
        else
        {
            ChangeState(CMResState.CMRS_OnLoad);
        }

        if (m_dlgs.Count > 0)
        {
            var enumerator = m_dlgs.GetEnumerator();
            CMEventBaseDelegate<T> dlg = null;
            while(enumerator.MoveNext())
            {
                dlg = enumerator.Current.Value;
                dlg.Dlg.Invoke(GetThis(), dlg.Param1, dlg.Param2, dlg.Param3);
            }
            m_dlgs.Clear();
        }
    }

    #region Create
    /// <summary>
    /// 异步
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    /// <param name="atlas"></param>
    /// <param name="createDlg"></param>
    /// <param name="custParam"></param>
    public void GetCMResourceAsyn(ref CMResAsynSeedData<T> seedData, CMResEvent<T> createDlg, object param1 = null, object param2 = null, object param3 = null)
    {
        StructCMResAsynSeedData(ref seedData);
        AddRef();
        if (IsReady())
        {
            ChangeState(CMResState.CMRS_Active);
            if (null != createDlg)
            {
                createDlg.Invoke(GetThis(), param1, param2, param3);
            }
        }
        else
        {
            if (!m_dlgs.ContainsKey(seedData.Seed))
            {
                m_dlgs.Add(seedData.Seed, new CMEventBaseDelegate<T>()
                {
                    Dlg = createDlg,
                    Param1 = param1,
                    Param2 = param2,
                    Param3 = param3,
                });
            }

            if (State != CMResState.CMRS_Loading)
            {
                ChangeState(CMResState.CMRS_Loading);
                DataManager.Manager<CMAssetBundleLoaderMgr>().CreateAssetAsyn(false,AbPath, AssetName, OnLoadAssetComplte, param1, param2, param3);
            }
        }
    }

    /// <summary>
    /// 同步
    /// </summary>
    /// <param name="abPath"></param>
    /// <param name="assetName"></param>
    public T GetCMResource()
    {
        AddRef();
        UnityEngine.Object obj = ResObj;
        if (!IsReady())
        {
            obj = DataManager.Manager<CMAssetBundleLoaderMgr>().CreateAsset(AbPath, AssetName);
            OnLoadAssetComplte(obj);
        }
        return GetThis();

    }

    /// <summary>
    /// 实现类一定要重载
    /// </summary>
    /// <returns></returns>
    protected virtual T GetThis()
    {
        return default(T);
    }

    #endregion

    #region Release

    /// <summary>
    /// 是否可以从资源管理器内释放该对象
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    public bool CanRemoveFromMgr(bool force = false)
    {
        float targetTime = m_fTimeRelaseStateKeep;
        if (CMResourceMgr.CM_RES_RECYCLE_TEST_MODE)
            targetTime = CMResourceMgr.CM_RES_RECYCLE_TEST_OBJ_TIME;
        if (State == CMResState.CMRS_Release
            && (force ||  (TimeStamp - m_fTimeSinceRelease >= targetTime)))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    public bool CanRelease(bool force = false)
    {
        if (RefCount > 0)
        {
            return false;
        }
        float targetTime = m_fTimdeSinceIdle;
        if (CMResourceMgr.CM_RES_RECYCLE_TEST_MODE)
            targetTime = CMResourceMgr.CM_RES_RECYCLE_TEST_TIME;

        if (force
            || (State == CMResState.CMRS_Idle && (TimeStamp - m_fTimdeSinceIdle >= targetTime)))
        {
            return true;
        }

        return false;
    }

    public virtual void ReleaseReference(int asynSeedId = 0)
    {
        if (RefCount ==0)
        {
            Engine.Utility.Log.Error("ICMResource->ReleaseReference error,Refcount is Zero,AssetName:{0}", AssetName);
            return;
        }
        RemoveRef();
        if (asynSeedId != 0 && m_dlgs.ContainsKey(asynSeedId))
        {
            m_dlgs.Remove(asynSeedId);
        }
    }

    public virtual bool Release(bool force = false)
    {
        if (CanRelease(force))
        {
            m_resObj = null; 
            ChangeState(CMResState.CMRS_Release);
            return true;
        }
        return false;
    }
    #endregion
}