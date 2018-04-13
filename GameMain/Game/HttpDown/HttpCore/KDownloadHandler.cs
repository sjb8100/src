using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

class KDownloadHandler : DownloadHandlerScript
{
    private FileStream m_FileStream = null;

    //要下载的文件总长度
    private int m_nContentLength = 0;

    //已经下载的数据长度
    private int m_nDownedLength = 0;
    public int GetDownedLength()
    {
        return m_nDownedLength;
    }

    //要保存的文件名称
    private string m_strFileName;

    public string GetFileNameTemp()
    {
        return m_strFileName + ".fd";
    }

    //要保存的文件路径
    private string m_strSavePath = null;
    public string DirectoryPath
    {
        get { return m_strSavePath.Substring(0, m_strSavePath.LastIndexOf('/')); }
    }

    //接收到数据总长度的事件回调.
    private event Action<int> pFunTotalLength = null;
    public void RegisteReceiveTotalLengthBack(Action<int> back)
    {
        if (back != null)
            pFunTotalLength += back;
    }


    //进度处理
    private event Action<float> pFunProgress = null;
    public void RegisteProgressBack(Action<float> back)
    {
        if (back != null)
            pFunProgress += back;
    }


    //下载完成
    private event Action<string> pFunComplete = null;
    public void RegisteCompleteBack(Action<string> back)
    {
        if (back != null)
            pFunComplete += back;
    }


    //-----------------------------------------------------------------------
    // 取消事件.
    public void UnRegisteCompleteBack(Action<string> back)
    {
        if (back != null)
            pFunComplete -= back;
    }
    public void UnRegisteReceiveTotalLengthBack(Action<int> back)
    {
        if (back != null)
            pFunTotalLength -= back;
    }
    public void UnRegisteProgressBack(Action<float> back)
    {
        if (back != null)
            pFunProgress -= back;
    }
    //-----------------------------------------------------------------------


    public KDownloadHandler(string filePath)
        : base(new byte[1024 * 200])
    {
        m_strSavePath = filePath;
        m_strFileName = m_strSavePath.Substring(m_strSavePath.LastIndexOf('/') + 1);
        //文件流操作的是临时文件
        m_FileStream = new FileStream(m_strSavePath + ".fd", FileMode.Append, FileAccess.Write);
        //设置已经下载的数据长度
        m_nDownedLength = (int)m_FileStream.Length;
    }

    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        if (data == null || data.Length == 0)
        {
            return false;
        }
        m_FileStream.Write(data, 0, dataLength);
        m_nDownedLength += dataLength;

        if (pFunProgress != null)
            pFunProgress.Invoke((float)m_nDownedLength / m_nContentLength);

        return true;
    }

    protected override void ReceiveContentLength(int contentLength)
    {
        m_nContentLength = contentLength + m_nDownedLength;
        if (pFunTotalLength != null)
            pFunTotalLength.Invoke(m_nContentLength);
    }

    // 下载完成
    protected override void CompleteContent()
    {
        string CompleteFilePath = DirectoryPath + "/" + m_strFileName;
        string TempFilePath = m_FileStream.Name;
        OnDispose();

        if (File.Exists(TempFilePath))
        {
            if (File.Exists(CompleteFilePath))
            {
                File.Delete(CompleteFilePath);
            }
            File.Move(TempFilePath, CompleteFilePath);

        }
        else
        {
            //下载的文件木有了.
        }
        if (pFunComplete != null)
            pFunComplete.Invoke(CompleteFilePath);

    }
    public void OnDispose()
    {
        m_FileStream.Close();
        m_FileStream.Dispose();
    }

}

