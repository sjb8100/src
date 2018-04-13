using System;
using System.Collections.Generic;
using UnityEngine;
using GameCmd;
using Common;

//stUserPingSceneServerPropertyUserCmd_CS
public partial class Protocol : MonoBehaviour,Engine.Utility.ITimer
{
    const int TIMER_ID = 2100;
    Dictionary<uint,int> m_dicSendTime = new Dictionary<uint,int>();
    bool setTimer = false;
    uint m_seesion = 0;
    int m_pingValue = 0;
    public int Ping
    {
        get { return m_pingValue; }
    }
    public void SetTimer()
    {
        if (setTimer == false)
        {
            Engine.Utility.TimerAxis.Instance().SetTimer(TIMER_ID, 3000, this, Engine.Utility.TimerAxis.INFINITY_CALL, "PING");
            setTimer = true;
        }
    }

    public void Clear()
    {
        if (setTimer)
        {
            Engine.Utility.TimerAxis.Instance().KillTimer(TIMER_ID, this);
            setTimer = true;
        }
    }

    public void OnTimer124()
    {
        int time = Mathf.RoundToInt(Time.realtimeSinceStartup * 1000);
        m_seesion++;
        m_dicSendTime.Add(m_seesion, time);
        NetService.Instance.Send(new stUserPingSceneServerPropertyUserCmd_CS() { session = m_seesion });
    }

    [Execute]
    public void Excute(stUserPingSceneServerPropertyUserCmd_CS cmd)
    {
        int curTime = Mathf.RoundToInt(Time.realtimeSinceStartup * 1000);
        int time = 0;
        if (m_dicSendTime.TryGetValue(cmd.session, out time))
        {
            m_pingValue = curTime - time;
            m_dicSendTime.Remove(cmd.session);
        }
    }
}
