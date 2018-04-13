/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.UIManager.CMObjPool
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ObjPool
 * 版本号：  V1.0.0.0
 * 创建时间：8/31/2017 11:00:41 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class CMObjPool
{
    #region 
    public enum CMObjPoolState
    {
        None = 0,
        Active,     //活跃
        Idle,       //空闲
        Release,    //释放
    }
    #endregion

    #region property
    //对象池时间戳
    public static float PoolTimeStamp
    {
        get
        {
            return Time.time;
        }
    }
    //是否要检测释放
    private bool m_bCheckRelease = false;

    //对象池名称
    private string m_sPoolName = "";
    public string PoolName
    {
        get
        {
            return m_sPoolName;
        }
    }
    //测试模式
    private bool m_bTestMode = false;
    //测试模式下时间
    private float m_fTestModeTime = 10f;
    //对象池根节点
    private Transform m_parent = null;
    //对象挂接点
    private Transform m_objMount = null;
    //克隆模板
    private Transform m_prefabTrans = null;
    //克隆模板对象
    private GameObject m_prefabObj = null;

    //是否正在越界剔除
    public bool m_bcullingActive = false;
    //释放过程中是否进实例对象越界剔除
    public bool m_bcullDespawned = false;
    //实例对象越界上限数量
    public int m_icullAbove = 50;
    //实例对象越界剔除延时
    public int m_cullDelay = 60;
    //单次剔除对象数
    public int m_iCullMaxPerPass = 5;

    //是否打印日志
    private bool m_blogMessage = false;
    //是否强制日志静默
    private bool m_bforceLoggingSilent = false;
    public bool LogMessage
    {
        get
        {
            if (m_bforceLoggingSilent)
                return false;
            return m_blogMessage;
        }
    }

    //预加载数量
    private int m_iPreLoadNum = 1;
    //是否已经执行预加载
    private bool m_bpreloaded = false;
    public bool Preloaded
    {
        get
        {
            return m_bpreloaded;
        }
    }

    private string m_strScript = "";
    //资源保存类型
    private int m_iresKeeepType = 1;

    private bool m_bLimitInstance = false;
    //最大持有数量，包括活跃与不活跃
    private int m_iLimitAmount = 100;

    //对象池空闲，维持数量
    private int m_iMaxKeepWhenIdle = 1;
    
    //对象池最大空闲时间
    private float m_fMaxPoolIdleTimeToRelease = 120f;
    //空闲状态开始时间戳
    private float m_fIdleStatusStartTimeStamp = 0;

    //对象池转从活跃入空闲状态时间
    private float m_fMaxPoolActiveToIdle = 60f;
    //最近一次活跃时间戳
    private float m_fLasetActiveTimeStamp = 0;

    //活跃列表
    private List<Transform> m_lstActive = null;

    //不活跃列表
    private List<Transform> m_lstInactive = null;

    //对象池状态
    private CMObjPoolState m_state = CMObjPoolState.None;
    public CMObjPoolState State
    {
        get
        {
            return m_state;
        }
    }

    /// <summary>
    /// 强制回收等待状态的对象
    /// </summary>
    public void ForceDespawnIdleInstance()
    {
        if (m_state != CMObjPoolState.Release && null != m_lstInactive && m_lstInactive.Count > 0)
        {
            for (int i = 0, max = m_lstInactive.Count; i < max; i++)
            {
                GameObject.Destroy(m_lstInactive[i].gameObject);
            }

            this.m_lstInactive.Clear();
        }
    }

    //是否可以释放
    public bool CanRelease(bool force = false)
    {
        float targetTime = m_fMaxPoolIdleTimeToRelease;
        if (m_bTestMode)
            targetTime = m_fTestModeTime;
        return m_bCheckRelease && (m_lstActive.Count == 0 && State == CMObjPoolState.Idle)
                && (PoolTimeStamp - m_fIdleStatusStartTimeStamp > targetTime); 
    }

    /// <summary>
    /// 总数量
    /// </summary>
    private int totalCount
    {
        get
        {
            int count = 0;
            count += m_lstActive.Count;
            count += m_lstInactive.Count;
            return count;
        }
    }

    /// <summary>
    /// 活跃数量
    /// </summary>
    public int ActiveCount
    {
        get
        {
            return m_lstActive.Count;
        }
    }
    #endregion

    #region Create
    private CMObjPool(string poolName
        , Transform ts
        ,bool needRelease = true
        ,int preLoadNum =0
        ,int maxHoldNum = 100
        ,bool cullNumEnable = true
        ,int cullNum = 20
        ,int maxKeepWhenIdle = 20
        ,float maxIdle2Release = 120f
        ,float maxActive2Idle = 60f
        ,Transform parent = null
        ,bool needCreateMountNode = true
        , string addScriptName = ""
        , bool testMode = false)
    {
        this.m_sPoolName = poolName;
        this.m_bCheckRelease = needRelease;
        this.m_prefabTrans = ts;
        this.m_prefabObj = m_prefabTrans.gameObject;
        this.m_parent = parent;
        this.m_iPreLoadNum = preLoadNum;
        this.m_iLimitAmount = maxHoldNum;
        this.m_bcullDespawned = cullNumEnable;
        this.m_icullAbove = cullNum;
        this.m_iMaxKeepWhenIdle = maxKeepWhenIdle;
        this.m_fMaxPoolIdleTimeToRelease = maxIdle2Release;
        this.m_fMaxPoolActiveToIdle = maxActive2Idle;
        this.m_strScript = addScriptName;
        this.m_bTestMode = testMode;
        if (needCreateMountNode)
        {
            m_objMount = new GameObject(poolName + "Pool").transform;
            if (null != this.m_parent)
            {
                m_objMount.parent = this.m_parent;
                m_objMount.gameObject.layer = parent.gameObject.layer;
                m_objMount.localScale = Vector3.one;
                m_objMount.localPosition = Vector3.zero;
                m_objMount.localRotation = Quaternion.identity;
            }
        }
        m_lstActive = new List<Transform>();
        m_lstInactive = new List<Transform>();
        BuildCloneObj();
        PreloadInstances();
    }

    /// <summary>
    /// 创建对象池
    /// </summary>
    /// <param name="poolName">对象池名称</param>
    /// <param name="cloneObj">克隆模板</param>
    /// <param name="parent">对象池根节点</param>
    /// <param name="preloadNum">预加载数量</param>
    /// <param name="maxHoldNum">最大持有数量</param>
    /// <param name="maxInativeNum">最大不活跃数量</param>
    /// <param name="maxKeepWhenIdle">空闲状态最大保持数</param>
    /// <param name="maxIdle2Release">空闲到释放的时间</param>
    /// <param name="maxActive2Idle">活跃转空闲时间</param>
    /// <returns></returns>
    /// 
    public static CMObjPool Create(string poolName
        , Transform ts
        ,bool needRelease = true
        ,int preLoadNum =0
        ,int maxHoldNum = 100
        ,bool cullNumEnable = true
        ,int cullNum = 20
        ,int maxKeepWhenIdle = 20
        ,float maxIdle2Release = 120f
        ,float maxActive2Idle = 60f
        ,Transform parent = null
        ,bool needCreateMountNode = true
        ,string addScriptName = "",bool testMode = false)
    {
        if (string.IsNullOrEmpty(poolName) || null == ts)
        {
            return null;
        }
        return new CMObjPool(poolName, ts, needRelease, preLoadNum, maxHoldNum, cullNumEnable
            , cullNum, maxKeepWhenIdle, maxIdle2Release, maxActive2Idle, parent, needCreateMountNode, addScriptName, testMode);
    }
    #endregion

    #region Op
    /// <summary>
    /// 执行预加载
    /// </summary>
    /// <returns></returns>
    public List<Transform> PreloadInstances()
    {
        List<Transform> instances = new List<Transform>();
        if (m_bpreloaded)
        {
            return instances;
        }

        if (null == m_prefabTrans)
        {
            return instances;
        }

        Transform trans;
        while(this.totalCount < this.m_iPreLoadNum)
        {
            trans = SpawnNew(Vector3.zero, Quaternion.identity);
            if (null == trans)
            {
                continue;
            }
            DespawnInstance(trans);
            instances.Add(trans);
        }

        if (m_bcullDespawned && totalCount > m_icullAbove)
        {
            //当前实例数量超过裁剪上限
        }
        return instances;
    }

    public void HandleIdle()
    {
        Transform ts = null;
        while (totalCount > m_iMaxKeepWhenIdle && m_lstInactive.Count > 0)
        {
            //if (m_sPoolName.Contains("MainPanel"))
            //{
            //    Debug.LogError("Destory Main");
            //}
            ts = m_lstInactive[0];
            m_lstInactive.RemoveAt(0);
            if (null == ts)
                continue;
            GameObject.Destroy(ts.gameObject);
        }
    }

    /// <summary>
    /// 释放
    /// </summary>
    /// <param name="force"></param>
    /// <returns></returns>
    public bool Release(bool force = false)
    {
        bool sucess = false;
        if (CanRelease(force))
        {
            //if (m_sPoolName.Contains("MainPanel"))
            //{
            //    Debug.LogError("Destory Main Release");
            //}
            this.m_prefabObj = null;
            this.m_parent = null;
            GameObject.Destroy(this.m_objMount.gameObject);
            this.m_objMount = null;
            for (int i = 0, max = m_lstInactive.Count; i < max; i++)
            {
                if (null == m_lstInactive[i])
                    continue;
               GameObject.Destroy(m_lstInactive[i].gameObject);
            }
            this.m_lstInactive.Clear();

            for (int i = 0, max = m_lstActive.Count; i < max; i++)
            {
                if (null == m_lstActive[i])
                    continue;
                GameObject.Destroy(m_lstActive[i].gameObject);
            }
            this.m_lstActive.Clear();

            if (null != cloneTs)
            {
                GameObject.Destroy(cloneTs.gameObject);
            }


            
            
            m_state = CMObjPoolState.Release;
            PrintMessage("ObjPoot {0} Change to sate {1}", m_sPoolName, m_state);
        }
        return sucess;
    }

    /// <summary>
    /// 输出日志
    /// </summary>
    /// <param name="msg"></param>
    private void PrintMessage(string format,params object[] args)
    {
        if (LogMessage)
            Engine.Utility.Log.Info(format, args);
    }
            
    /// <summary>
    /// 重命名
    /// </summary>
    /// <param name="instance"></param>
    private void NameInstance(Transform instance)
    {
        instance.name += (this.totalCount + 1).ToString("#0000");
    }

    /// <summary>
    /// 获取实例
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="setActive"></param>
    /// <returns></returns>
    public Transform SpawnInstance(Vector3 pos, Quaternion rot ,bool setActive = true)
    {
        Transform ts = null;
        if (m_lstInactive.Count == 0)
        {
            ts = SpawnNew(pos,rot);
        }else
        {
            ts = m_lstInactive[0];
            m_lstInactive.RemoveAt(0);
            m_lstActive.Add(ts);
            ts.localPosition = pos;
            ts.localRotation = rot;
            if (setActive)
            {
                ts.gameObject.SetActive(true);
            }
        }
        ChangeState(CMObjPoolState.Active);
        return ts;
    }
    private Transform cloneTs = null;
    private void BuildCloneObj()
    {
        if (null == cloneTs && !string.IsNullOrEmpty(m_strScript))
        {
            cloneTs = (Transform)UnityEngine.Object.Instantiate(m_prefabTrans, Vector3.zero, Quaternion.identity);
            if (null != m_objMount)
            {
                cloneTs.parent = m_objMount;
            }
            else if (null != m_parent)
            {
                cloneTs.parent = m_parent;
            }
            if (!cloneTs.gameObject.activeSelf)
            {
                cloneTs.gameObject.SetActive(true);
            }
            if (null == cloneTs.GetComponent(m_strScript))
            {
                Util.AddComponent(cloneTs.gameObject, m_strScript);
            }
            cloneTs.gameObject.SetActive(false);
        }
        
    }

    /// <summary>
    /// 创建新对象
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    /// <param name="setInactive"></param>
    /// <returns></returns>
    private Transform SpawnNew(Vector3 pos, Quaternion rot,bool setInactive = true)
    {
        //if (totalCount >=1 && m_sPoolName.Contains("MainPanelk"))
        //{
        //    Debug.LogError("Create out");
        //}
        if (m_bLimitInstance && totalCount >= m_iLimitAmount)
        {
            //实例数量限制
            return null;
        }
        Transform t = null;
        if (null != cloneTs)
        {
            t = (Transform)UnityEngine.Object.Instantiate(cloneTs, pos, rot);
        }else
        {
            t = (Transform)UnityEngine.Object.Instantiate(m_prefabTrans, pos, rot);
        }
        
        NameInstance(t);
        if (null != m_objMount)
        {
            t.parent = m_objMount;
        }else if (null != m_parent)
        {
            t.parent = m_parent;
        }
        m_lstActive.Add(t);
        return t;
    }
    /// <summary>
    /// 释放实例
    /// </summary>
    /// <param name="t"></param>
    /// <param name="setInactive"></param>
    public void DespawnInstance(Transform t,bool setInactive =false)
    {
        this.m_lstActive.Remove(t);
        //Debug.LogError("DespawnInstance:" + t.name);
        if (null != m_objMount)
        {
            t.parent = m_objMount;
        }
        else if (null != m_parent)
        {
            t.parent = m_parent;
        }
        if (setInactive)
        {
            t.gameObject.SetActive(false);
        }
        if (!m_lstInactive.Contains(t))
        {
            m_lstInactive.Add(t);
        }
        t.localPosition = Vector3.zero;
        ChangeState(CMObjPoolState.Active);
        return;
        //实例裁减剔除
        if (!m_bcullingActive && m_bcullDespawned
            && totalCount > m_icullAbove)
        {
            m_bcullingActive = true;
            CoroutineMgr.Instance.StartCorountine(CullDespawned());
        }
        
    }

    /// <summary>
    /// 协成执行剔除
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator CullDespawned()
    {
        yield return new WaitForSeconds(m_cullDelay);
        while (totalCount > m_icullAbove && m_icullAbove > 0)
        {
            for(int i = 0 ;i < m_iCullMaxPerPass;i++)
            {
                if (totalCount < m_icullAbove)
                    break;
                if (m_lstInactive.Count > 0)
                {
                    Transform t = m_lstInactive[0];
                    m_lstInactive.RemoveAt(0);
                    GameObject.Destroy(t.gameObject);
                }
                else
                {
                    break;
                }
            }
            // Check again later
            yield return new WaitForSeconds(this.m_cullDelay);
        }
        m_bcullingActive = false;
        yield return null;
    }
    
    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="state"></param>
    private void ChangeState(CMObjPoolState state)
    {
        if (m_state == CMObjPoolState.Release)
            return ;
        if (m_state != state)
        {
            bool setSuccess = true;
            switch(state)
            {
                case CMObjPoolState.Active:
                    {
                        m_fLasetActiveTimeStamp = PoolTimeStamp;
                    }
                    break;
                case CMObjPoolState.Idle:
                    {
                        m_fIdleStatusStartTimeStamp = PoolTimeStamp;
                        HandleIdle();
                        PrintMessage("ObjPoot {0} Change to sate {1}", m_sPoolName, m_state);
                    }
                    break;
                case CMObjPoolState.Release:
                    {
                      setSuccess =  Release();
                    }
                    break;
            }
            if (setSuccess)
                m_state = state;
            
        }
    }
    #endregion

    #region recycle

    /// <summary>
    /// 执行对象状态
    /// </summary>
    private void ProcessPoolState()
    {
        float targetTime = 0;
        
        if (m_state == CMObjPoolState.Active)
        {
            targetTime = m_fMaxPoolActiveToIdle;
            if (m_bTestMode)
                targetTime = m_fTestModeTime;
            if (PoolTimeStamp - m_fLasetActiveTimeStamp > targetTime)
            {
                ChangeState(CMObjPoolState.Idle);
            }
        }else if (m_state == CMObjPoolState.Idle && m_lstActive.Count ==0)
        {
            targetTime = m_fMaxPoolIdleTimeToRelease;
            if (m_bTestMode)
                targetTime = m_fTestModeTime;
            if (PoolTimeStamp - m_fIdleStatusStartTimeStamp > targetTime)
            {
                ChangeState(CMObjPoolState.Release);
            }
            
        }
    }

    /// <summary>
    /// 执行
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Process(float deltaTime)
    {
        ProcessPoolState();
    }
    #endregion
}