
//*************************************************************************
//	创建日期:	2017/12/7 星期四 10:08:21
//	文件名称:	RedEnvelopeDataManager
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;
using Engine.Utility;
using UnityEngine;
public enum RedEnveLopeEvent
{
    RefreshAllRedEnveLope,//刷新所有红包
    RefreshRedEnvelopeGrid,//刷新单个格子
    ShowRedEffect,//显示红包特效
}
class RedEnvelopeDataManager : BaseModuleData, IManager
{

    List<RedPacket> m_worldRedPacketList = new List<RedPacket>();
    public List<RedPacket> WorldRedPackets
    {
        get
        {
            return m_worldRedPacketList;
        }
    }
    List<RedPacket> m_clanRedPacketList = new List<RedPacket>();

    public List<RedPacket> ClanRedPackets
    {
        get
        {
            return m_clanRedPacketList;
        }
    }
    uint m_uSysRedPacketID = 0;//系统红包id
    public uint SysRedPacketID
    {
        get
        {
            return m_uSysRedPacketID;
        }
        set
        {
            m_uSysRedPacketID = value;
        }
    }
    #region IManager
    public void Initialize()
    {

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
    #endregion
    #region 协议处理
    /// <summary>
    /// 接收所有红包信息
    /// </summary>
    /// <param name="cmd"></param>
    public void OnReciveAllRedData(stAllRedPacketChatUserCmd_S cmd)
    {
        if (cmd.world)
        {
            m_worldRedPacketList.Clear();
            m_worldRedPacketList.AddRange(cmd.red_packets);
        }
        else
        {
            m_clanRedPacketList.Clear();
            m_clanRedPacketList.AddRange(cmd.red_packets);
        }
        SortAllRedPacket();
    }
    /// <summary>
    /// 通知红包领取
    /// </summary>
    /// <param name="cmd"></param>
    public void OnNoticeRedEnveLopeInfo(stNoticeRedPacketChatUserCmd_S cmd)
    {
        RedPacket rp = new RedPacket();
        rp.status = (uint)RedPacketStatus.eRedPacketNormal;
        rp.title = cmd.title;
        rp.id = cmd.id;
        rp.name = cmd.name;
        AddPacket(cmd.world, rp);
        ChangeRedStatus(cmd.id, RedPacketStatus.eRedPacketNormal);
        SortAllRedPacket();
        if (!DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RedEnvelopePanel))
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RedEnvelopeTakePanel, jumpData: new UIPanelBase.PanelJumpData() { Param = cmd });
        }
        uint langTextID = 140001;
        ChatChannel channel = DataManager.Manager<ChatDataManager>().GetChannelByType(CHATTYPE.CHAT_WORLD);
        if (!cmd.world)
        {
            langTextID = 140002;
            channel = DataManager.Manager<ChatDataManager>().GetChannelByType(CHATTYPE.CHAT_CLAN);
        }
        stSendInfoReminderChatUserCmd_S chatCmd = new stSendInfoReminderChatUserCmd_S();
        chatCmd.id = langTextID;
        chatCmd.szInfo = new List<string> { cmd.name };
        Protocol.Instance.OnReceiveRemindText(chatCmd);
        uint moneyType = GameTableManager.Instance.GetGlobalConfig<uint>("UserRedEnvelopeCurrencyType");
        uint maxNum = GameTableManager.Instance.GetGlobalConfig<uint>("UserRedEnvelopeCurrencyNum");
        if (cmd.money_type == moneyType)
        {
            if (cmd.gold >= maxNum)
            {
                DispatchValueUpdateEvent(RedEnveLopeEvent.ShowRedEffect.ToString(), null, null);
            }
        }


        if (channel != null)
        {
            channel.Add(channel.ToChatInfoWithRedPackgetMsg(cmd));
        }
    }

    /// <summary>
    ///抢到的红包
    /// </summary>
    /// <param name="cmd"></param>
    public void OnReciveGrapRedEnvelopeInfo(stRecvRedPacketChatUserCmd_CS cmd)
    {
        ChangeRedStatus(cmd.id, RedPacketStatus.eRedPacketRecv);

        DispatchValueUpdateEvent(RedEnveLopeEvent.RefreshRedEnvelopeGrid.ToString(), null, cmd.id);
    }
    /// <summary>
    /// 红包被抢光
    /// </summary>
    /// <param name="cmd"></param>
    public void OnNoticeRedEnvelopeEmpty(stRedPacketEmptyChatUserCmd_S cmd)
    {
        ChangeRedStatus(cmd.id, RedPacketStatus.eRedpacketEmpty);
        DispatchValueUpdateEvent(RedEnveLopeEvent.RefreshRedEnvelopeGrid.ToString(), null, cmd.id);
    }
    /// <summary>
    /// 红包详情
    /// </summary>
    /// <param name="cmd"></param>
    public void OnReciveRedPacketDetails(stRedPacketInfoChatUserCmd_S cmd)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RedEnvelopeDetailPanel, jumpData: new UIPanelBase.PanelJumpData() { Param = cmd });
    }
    /// <summary>
    /// 红包记录
    /// </summary>
    /// <param name="cmd"></param>
    public void OnRedEnvelopeLog(stRedPacketLogChatUserCmd_S cmd)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RedEnvelopeRecordPanel, jumpData: new UIPanelBase.PanelJumpData() { Param = cmd });
    }

    /// <summary>
    /// 添加系统红包 //dolua tc()  //dolua te()
    /// </summary>
    /// <param name="cmd"></param>
    public void OnAddSysRedPacket(stAddSysRedPacketChatUserCmd_S cmd)
    {
        SysRedPacketID = cmd.id;
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RedEnvelopeTakeSysPanel, jumpData: new UIPanelBase.PanelJumpData() { Param = cmd });
    }
    /// <summary>
    /// 移除系统和用户红包
    /// </summary>
    /// <param name="cmd"></param>
    public void OnRemoveSysRedPacket(stRemoveSysRedPacketChatUserCmd_S cmd)
    {
        if (cmd.id == SysRedPacketID)
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.RedEnvelopeTakeSysPanel);
        }
        else
        {
            RemovePacketByID(cmd.id);
        }
    }
    #endregion

    void RemovePacketByID(uint redID)
    {
        for (int i = 0; i < m_worldRedPacketList.Count; i++)
        {
            RedPacket rp = m_worldRedPacketList[i];
            if (rp.id == redID)
            {
                m_worldRedPacketList.RemoveAt(i);
                break;
            }
        }

        for (int i = 0; i < m_clanRedPacketList.Count; i++)
        {
            RedPacket rp = m_clanRedPacketList[i];
            if (rp.id == redID)
            {
                m_clanRedPacketList.RemoveAt(i);
                break;
            }
        }
        RefreshMainRedPacketUI();
    }
    void AddPacket(bool bWrold, RedPacket rp)
    {
        if (bWrold)
        {
            AddPacketToList(m_worldRedPacketList, rp);
        }
        else
        {
            AddPacketToList(m_clanRedPacketList, rp);
        }
    }
    void AddPacketToList(List<RedPacket> redList, RedPacket rp)
    {
        bool bContain = false;
        foreach (var item in redList)
        {
            if (item.id == rp.id)
            {
                bContain = true;
                break;
            }
        }
        if (!bContain)
        {
            redList.Add(rp);
        }
    }
    void RefreshMainRedPacketUI()
    {
        DispatchValueUpdateEvent(RedEnveLopeEvent.RefreshAllRedEnveLope.ToString(), null, null);
    }
    void SortAllRedPacket()
    {
        SortRedList(m_worldRedPacketList);
        SortRedList(m_clanRedPacketList);
        RefreshMainRedPacketUI();
    }
    void SortRedList(List<RedPacket> redList)
    {
        redList.Sort((x1, x2) =>
        {
            if (x1.status > x2.status)
            {
                return 1;
            }
            else if (x1.status < x2.status)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        });
    }
    /// <summary>
    /// 改变红包状态并且刷新界面
    /// </summary>
    /// <param name="redid"></param>
    /// <param name="status"></param>
    void ChangeRedStatus(uint redid, RedPacketStatus status)
    {
        for (int i = 0; i < m_clanRedPacketList.Count; i++)
        {
            RedPacket rp = m_clanRedPacketList[i];
            if (rp.id == redid)
            {
                if (rp.status != (uint)RedPacketStatus.eRedpacketEmpty)
                {
                    rp.status = (uint)status;
                    m_clanRedPacketList[i] = rp;
                    break;
                }
            }

        }
        for (int i = 0; i < m_worldRedPacketList.Count; i++)
        {
            RedPacket rp = m_worldRedPacketList[i];
            if (rp.id == redid)
            {
                if (rp.status != (uint)RedPacketStatus.eRedpacketEmpty)
                {
                    rp.status = (uint)status;
                    m_worldRedPacketList[i] = rp;
                    break;
                }
            }

        }

    }

    public RedPacket GetRedPacketByID(uint id)
    {
        RedPacket rp = null;
        foreach (var item in m_worldRedPacketList)
        {
            if (item.id == id)
            {
                rp = item;
                return rp;
            }
        }
        foreach (var item in m_clanRedPacketList)
        {
            if (item.id == id)
            {
                rp = item;
                return rp;
            }
        }
        return rp;
    }
    public CHATTYPE GetRedEnvelopeChannelByID(uint redID)
    {
        CHATTYPE ct = CHATTYPE.CHAT_NONE;
        foreach (var item in m_worldRedPacketList)
        {
            if (item.id == redID)
            {
                ct = CHATTYPE.CHAT_WORLD;
                return ct;
            }
        }
        foreach (var item in m_clanRedPacketList)
        {
            if (item.id == redID)
            {
                ct = CHATTYPE.CHAT_CLAN;
                return ct;
            }
        }
        return ct;
    }

    public string GetShortMessage(string message)
    {
        string str = message;
        if (message.Length > 8)
        {
            str = message.Substring(0, 8) + "...";
        }
        return str;
    }
}
