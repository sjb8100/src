
//*************************************************************************
//	创建日期:	2017/1/5 星期四 17:39:02
//	文件名称:	CommonWaitDataManager
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class CommonWaitDataManager : IManager
{

    public void Initialize()
    {
        waitCd = GameTableManager.Instance.GetGlobalConfig<uint>("WaitPanelCDTime");
        if (waitCd == 0)
        {
            waitCd = 10;
        }
        waitReconnectTime = GameTableManager.Instance.GetGlobalConfig<uint>("WaitReconnectCDTime");
        if (waitReconnectTime == 0)
        {
            waitReconnectTime = 15;
        }
        showWaitPanelTime = GameTableManager.Instance.GetGlobalConfig<uint>("ShowWaitPanelTimes");
        if (showWaitPanelTime == 0)
        {
            showWaitPanelTime = 5;
        }
        heartTime = GameTableManager.Instance.GetGlobalConfig<uint>("NetHeartTime");
        if (heartTime == 0)
        {
            heartTime = 2000;
        }
        reconnectTime = GameTableManager.Instance.GetGlobalConfig<uint>("NetReconnectSeverTime");
        if (reconnectTime == 0)
        {
            reconnectTime = 2000;
        }
        tickOutTime = GameTableManager.Instance.GetGlobalConfig<uint>("NetTimeTickOutTime");
        if (tickOutTime == 0)
        {
            tickOutTime = 3;
        }
    }

    public void Reset(bool depthClearData = false)
    {
    }

    public void Process(float deltaTime)
    {
    }
    public void ClearData()
    {

    }
    int reconnecttime = 0;
    public int ReconnectTime
    {
        set
        {
            reconnecttime = value;
        }
        get
        {
            return reconnecttime;
        }
    }
    //等待面板cd时间(s)
    uint waitCd = 10;
    public uint WaitPanelCDTime
    {
        get
        {
            return waitCd;
        }
    }
    //确认重连提示面板取消按钮倒计时（s)
    uint waitReconnectTime = 15;
    public uint WaitReconnectCDTime
    {
        get
        {
            return waitReconnectTime;
        }
    }
    //显示确认重连面板次数
    uint showWaitPanelTime = 5;
    public uint ShowWaitPanelTimes
    {
        get
        {
            return showWaitPanelTime;
        }
    }
    //心跳间隔时间（ms)
    uint heartTime = 3000;
    public uint NetHeartTime
    {
        get
        {
            if(UnityEngine.Application.isEditor)
            {
                if (GameEntry.Instance().NetDebug)
                {
                    return 1000000;
                }
                else
                {
                    return heartTime;
                }
            }

            return heartTime;

        }
    }
    //重连服务器间隔时间
    uint reconnectTime = 3000;
    public uint NetReconnectSeverTime
    {
        get
        {
            return reconnectTime;
        }
    }
    //网关踢出次数 （NetHeartTime*NetTimeTickOutTime = 被网关踢出的时间）
    public uint tickOutTime = 0;
    public uint NetTimeTickOutTime
    {
        get
        {
            return tickOutTime;
        }
    }
}
