using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 协成管理器
/// </summary>
public class CoroutineMgr : Singleton<CoroutineMgr>
{
    private CoroutineCore core = null;


    #region IManager Method
    public void Initialize(GameObject mainObj)
    {
        if (null == mainObj)
        {
            Engine.Utility.Log.Error("CoroutineMgr Initialize Failed, MainObject not found!");
            return;
        }
        core = mainObj.AddComponent<CoroutineCore>();
    }

    public void Reset()
    {
    }

    public void Process(float deltaTime)
    {
    }
    public void ClearData()
    {

    }
    #endregion

    /// <summary>
    /// 开启一个协成
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public Coroutine StartCorountine(IEnumerator e)
    {
        if (null == core)
            return null;
        return core.StartCoroutine(e);
    }

    /// <summary>
    /// 暂停一个协成
    /// </summary>
    /// <param name="c"></param>
    public void StopCoroutines(Coroutine c)
    {
        if (null == core)
            return;
        if (null == c)
            return;
        core.StopCoroutine(c);
    }

    /// <summary>
    /// 停止所有协成
    /// </summary>
    public void StopAllCoroutines()
    {
        if (null == core)
            return;
        core.StopAllCoroutines();
    }

    /// <summary>
    /// 加载资源协成（www加载）
    /// </summary>
    /// <param name="url">url</param>
    /// <param name="callback">加载完成回调</param>
    /// <returns></returns>
    public IEnumerator LoadResCoroutine(string url, Action<WWW> callback)
    {
        if (string.IsNullOrEmpty(url))
        {
            if (null != callback)
                callback(null);
            yield break;
        }

        WWW www = new WWW(url);
        yield return www;
        if (www.isDone && null != callback)
        {
            callback(www);
        }
    }

    /// <summary>
    /// 开始加载资源协成
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public Coroutine StartLoadResCoroutine(string url,Action<WWW> callback)
    {
        return (StartCorountine(LoadResCoroutine(url, callback)));
    }

    /// <summary>
    /// 延迟调用方法
    /// </summary>
    /// <param name="delayTime">延迟时间</param>
    /// <param name="action">触发方法</param>
    /// <param name="waitEndOfFrame">是否延迟为等待当前帧完成</param>
    public void DelayInvokeMethod(float delayTime,Action action,bool waitEndOfFrame = false)
    {
        if (null == action)
            return;
        StartCorountine(DelayInvokeAction(delayTime, action, waitEndOfFrame));
    }


    /// <summary>
    /// 延迟触发action
    /// </summary>
    /// <param name="delayTime">延迟时间</param>
    /// <param name="action">动作</param>
    /// <param name="waitEndOfFrame">是否为等待帧完成</param>
    /// <returns></returns>
    private IEnumerator DelayInvokeAction(float delayTime,Action action,bool waitEndOfFrame)
    {
        if (waitEndOfFrame)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

        }else
        {
            yield return new WaitForSeconds(delayTime);
        }
        
        if (null != action)
            action.Invoke();
    }

}

public sealed class CoroutineCore : MonoBehaviour
{
    void OnDestroy()
    {
        StopAllCoroutines();
    }
}