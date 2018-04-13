using System;
using System.Collections.Generic;
using Engine.Utility;
using Engine;

/// <summary>
/// 网络流量监控
/// </summary>
class NetLinkMonitor : INetLinkMonitor
{
    bool m_bOpen;
    long m_ReceiveBytes;
    long m_SendBytes;
    Dictionary<string, uint> m_dicData = new Dictionary<string, uint>();
    public NetLinkMonitor()
    {
        m_bOpen = false;
        m_ReceiveBytes = 0;
        m_SendBytes = 0;
        m_dicData.Add("stMultiAttackDownMagicUserCmd_S", 0);
        m_dicData.Add("stMultiAttackReturnMagicUserCmd_S", 0);
        m_dicData.Add("stPrepareUseSkillSkillUserCmd_S", 0);
        m_dicData.Add("stSendSkillCDMagicUserCmd_S", 0);
        m_dicData.Add("stSendStateIcoListMagicUserCmd_S", 0);
        m_dicData.Add("stStatesChangeDataUserCmd_S", 0);
        m_dicData.Add("stHPChangeDataUserCmd_S", 0);
        m_dicData.Add("stFlagsChangeDataUserCmd_S", 0);
        m_dicData.Add("stSetHPDataUserCmd_S", 0);
        m_dicData.Add("stSetSPDataUserCmd_S", 0);
        m_dicData.Add("stSetHpSpDataUserCmd_CS", 0);
        m_dicData.Add("stObtainExpPropertyUserCmd_S", 0);
    }
    public bool IsOpen
    {
        get
        {
            return m_bOpen;
        }
        set
        {
            m_bOpen = value;
            if (m_bOpen == false)
            {
                m_ReceiveBytes = 0;
                m_SendBytes = 0;
            }
        }
    }

    public void OnReceive(PackageIn msg)
    {
        if (!m_bOpen)
        {
            return;
        }
        m_ReceiveBytes += msg.buffLen;
    }

    public void OnSend(PackageOut msg)
    {
        if (!m_bOpen)
        {
            return;
        }
        m_SendBytes += msg.Length;
    }
    /// <summary>
    /// 设置监控消息
    /// </summary>
    /// <param name="type"></param>
    public void SetMontitorMessage(string type,uint num)
    {
        if(!m_dicData.ContainsKey(type))
        {
            m_dicData.Add(type, 0);
        }
        else
        {
            m_dicData[type] = num;
        }
    }
    /// <summary>
    /// 获取消息数据
    /// </summary>
    /// <returns>要监控的数量和接收次数</returns>
    public Dictionary<string, uint> GetMessageData()
    {
        return m_dicData;
    }
    public long GetTotalReceiveBytes()
    {
        return m_ReceiveBytes;
    }

    public long GetTotalSendBytes()
    {
        return m_SendBytes;
    }



}
