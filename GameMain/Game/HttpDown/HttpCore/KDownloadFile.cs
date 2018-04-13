using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

class KDownlaodFile
{
    private static KDownlaodFile s_Instance = null;
    public static KDownlaodFile Instance()
    {
        if (s_Instance == null)
        {
            s_Instance = new KDownlaodFile();
        }
        return s_Instance;
    }

    public Dictionary<string, UnityWebRequest> m_listRequest = new Dictionary<string, UnityWebRequest>();

    private List<string> m_RemoveList = new List<string>();
    private List<string> m_ErrorList = new List<string>();
    private float m_fLastCheckTime = 0;

    public KDownloadHandler StartDownload(string url, string savePath)
    {
        if (m_listRequest.ContainsKey(url))
        {
            // 在下载列表里
            return null;
        }

        KDownloadHandler loadHandler = new KDownloadHandler(savePath);
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.chunkedTransfer = true;
        request.disposeDownloadHandlerOnDispose = true;
        request.SetRequestHeader("Range", "bytes=" + loadHandler.GetDownedLength() + "-");
        request.downloadHandler = loadHandler;
        request.Send();

        m_listRequest.Add(url, request);
        return loadHandler;

    }

    public void StopDownload(string url)
    {
        UnityWebRequest request = null;
        if (!m_listRequest.TryGetValue(url, out request))
        {
            return;
        }
        m_listRequest.Remove(url);

        //释放文件操作的资源
        (request.downloadHandler as KDownloadHandler).OnDispose();

        request.Abort();    //中止下载
        request.Dispose();  //释放

    }

    bool CkeckNetworking()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CheckErrorList()
    {
        if (m_fLastCheckTime < Time.time)
        {
            m_fLastCheckTime = Time.time + 5;
        }
        else
        {
            return;
        }

        if (m_ErrorList.Count < 1)
            return;

        //网络检查.
        if (CkeckNetworking() == false)
        {
            UnityEngine.Debug.Log("网络不通");
            return;

        }
        for (int i = 0; i < m_ErrorList.Count; i++)
        {
            string strLog = string.Format("继续下载失败文件:{0}", m_ErrorList[i]);
            UnityEngine.Debug.Log(strLog);

            KDownloadInstance.Instance().OnError(m_ErrorList[i]);
        }

        m_ErrorList.Clear();

    }

    public void Update()
    {
        CheckErrorList();

        Dictionary<string, UnityWebRequest>.Enumerator it = m_listRequest.GetEnumerator();
        while (it.MoveNext())
        {
            string url = it.Current.Key;
            UnityWebRequest request = it.Current.Value;
            if (request.isError)
            {
                // 下载失败...
                //request.Dispose();
                (request.downloadHandler as KDownloadHandler).OnDispose();
                request.Abort();    //中止下载
                request.Dispose();  //释放

                m_RemoveList.Add(url);
                m_ErrorList.Add(url);
  
            }
            else if (request.isDone)
            {
                request.Dispose();
                m_RemoveList.Add(url);
            }
        }

        for (int i = 0; i < m_RemoveList.Count; i++)
        {
            m_listRequest.Remove(m_RemoveList[i]);
           
        }
        m_RemoveList.Clear();


    }

    public void Quit()
    {
        foreach (string url in m_listRequest.Keys)
        {
            (m_listRequest[url].downloadHandler as KDownloadHandler).OnDispose();  //释放资源
            m_listRequest[url].Dispose();
        }
        m_listRequest.Clear();
    }

}

