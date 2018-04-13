
//*************************************************************************
//	创建日期:	2017/9/5 星期二 16:23:47
//	文件名称:	NetDataMonitor
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Engine;
using Engine.Utility;
public class NetDataMonitor : MonoBehaviour
{
    public INetLinkMonitor m_monitor = null;
    Rect windowRect = new Rect(0, 0, 400, 600);
    bool bShow = false;
    void Start()
    {
        m_monitor = NetService.Instance.CurrentMoniter;
    }
    void OnGUI()
    {
        if(Application.isEditor)
        {
            if (GUILayout.Button("HideNetMonitor"))
            {
                bShow = false;
            }
            if (GUILayout.Button("ShowNetMonitor"))
            {
                bShow = true;
            }
            if(bShow)
            {
                windowRect = GUI.Window(0, windowRect, DoDragWindow, "可拖动窗口");
            }
      
        }
        
    }
    public string messageStr = "";
    void DoDragWindow(int winID)
    {
    
        GUI.BringWindowToFront(winID);
        GUI.backgroundColor = Color.black;
        GUI.DragWindow(new Rect(0, 0, 400, 600));
    //    GUILayout.BeginHorizontal();
        long recvdataNum = m_monitor.GetTotalReceiveBytes();
        float recvkNum = recvdataNum*1.0f / 1024;
        float recvmNum = recvkNum*1.0f / 1024;
        string recvStr = string.Format("接收数据：{0}k = {1}M", recvkNum.ToString("f2"), recvmNum.ToString("f2"));
        GUI.color = Color.red;
        GUILayout.Label(recvStr);

        GUI.color = Color.yellow;
        long senddataNum = m_monitor.GetTotalSendBytes();
        float sendkNum = senddataNum*1.0f / 1024;
        float sendmNum = sendkNum*1.0f / 1024;
        string sendStr = string.Format("发送数据：{0}k = {1}M", sendkNum.ToString("f2"), sendmNum.ToString("f2"));
        GUILayout.Label(sendStr);
      //  GUILayout.EndHorizontal();


        
        GUILayout.BeginVertical();
        GUI.color = Color.blue;
        Dictionary<string, uint> dic = m_monitor.GetMessageData();
        var dicSort = from objDic in dic orderby objDic.Value descending select objDic;
        foreach (KeyValuePair<string, uint> kvp in dicSort)
        {
            string msg = string.Format("{0}  次数：{1}", kvp.Key, kvp.Value);
            GUILayout.Label(msg);
        }
        GUILayout.EndVertical();
    }

}
