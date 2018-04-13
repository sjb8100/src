
//*************************************************************************
//	创建日期:	2017/12/8 星期五 15:05:44
//	文件名称:	UIRedEnvelopeGrid
//   创 建 人:   zhudianyu	
//	版权所有:	zhudianyu
//	说    明:	
//*************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;
class UIRedEnvelopeGrid : UIGridBase
{
    Transform m_transOpen;
    Transform m_transOpenAble;
    UILabel m_openTitleName;
    UILabel m_openBlessmsg;
    UILabel m_openStausDes;
    Transform m_openDetail;
    Transform m_openHasOpen;
    Transform m_openNoOpen;

    UILabel m_openableName;
    UILabel m_openableDes;
    bool bInit = false;

    RedPacket m_curRedPacket;

    RedEnvelopeDataManager m_dataManager
    {
        get
        {
            return DataManager.Manager<RedEnvelopeDataManager>();
        }
    }
    void InitControl()
    {
      
        m_transOpen = transform.Find("bg/open");
        m_transOpenAble = transform.Find("bg/openable");
        if (m_transOpen != null)
        {
            m_openTitleName = m_transOpen.Find("title_label").GetComponent<UILabel>();
            m_openBlessmsg = m_transOpen.Find("text_label").GetComponent<UILabel>();
            m_openHasOpen = m_transOpen.Find("lightMark");
            m_openNoOpen = m_transOpen.Find("darkMark");
            m_openDetail = m_transOpen.Find("hypelink");
            if (m_openDetail != null)
            {
                UIEventListener.Get(m_openDetail.gameObject).onClick = LookPacketDetails;
            }
        }
        if (m_transOpenAble != null)
        {
            m_openableDes = m_transOpenAble.Find("bg/des_label").GetComponent<UILabel>();
            m_openableName = m_transOpenAble.Find("name_label").GetComponent<UILabel>();

        }
        UIEventListener.Get(gameObject).onClick = OpenPacket;

    }

    protected override void OnEnable()
    {
        m_dataManager.ValueUpdateEvent += m_dataManager_ValueUpdateEvent;
    }

    void m_dataManager_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if(e != null)
        {
            if(e.key == RedEnveLopeEvent.RefreshRedEnvelopeGrid.ToString())
            {
                uint redID = (uint)e.newValue;
                if(m_curRedPacket == null)
                {
                    return;
                }
                if(redID == m_curRedPacket.id)
                {
                    RedPacket rp = m_dataManager.GetRedPacketByID(redID);
                    if (rp != null)
                    {
                        m_curRedPacket = rp;
                        InitRedGrid(m_curRedPacket);
                    }
                }
              
            }
        }
    }
    protected override void OnDisable()
    {
        m_dataManager.ValueUpdateEvent -= m_dataManager_ValueUpdateEvent;
    }
    void OpenPacket(GameObject go)
    {
        if (m_curRedPacket != null)
        {
            if (m_curRedPacket.status == (uint)RedPacketStatus.eRedPacketNormal)
            {
                stRecvRedPacketChatUserCmd_CS cmd = new stRecvRedPacketChatUserCmd_CS();
                cmd.id = m_curRedPacket.id;
                NetService.Instance.Send(cmd);
            }
            else if (m_curRedPacket.status == (uint)RedPacketStatus.eRedpacketEmpty)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Red_Tips_henyihanhongbaoyijingbeiqiangwanle);
                stRedPacketInfoChatUserCmd_C cmd = new stRedPacketInfoChatUserCmd_C();
                cmd.id = m_curRedPacket.id;
                NetService.Instance.Send(cmd);
                return;
            }
            else
            {
                stRedPacketInfoChatUserCmd_C cmd = new stRedPacketInfoChatUserCmd_C();
                cmd.id = m_curRedPacket.id;
                NetService.Instance.Send(cmd);
            }
        }

    }
    void LookPacketDetails(GameObject go)
    {


    }
    protected override void OnAwake()
    {
        base.OnAwake();
        InitControl();
    }
    public void InitRedGrid(RedPacket packet)
    {
        InitControl();
        m_curRedPacket = packet;
        string str = m_dataManager.GetShortMessage(packet.title);

        if (packet.status == (uint)RedPacketStatus.eRedPacketNormal)
        {//可以领取m_transOpenAble
            ShowOpenAble(true);

            m_openableDes.text = str;
            m_openableName.text = packet.name;
        }
        else if (packet.status == (uint)RedPacketStatus.eRedPacketRecv)
        {//已经领取	
            ShowOpenAble(false);
            m_openTitleName.text = packet.name;
            m_openBlessmsg.text = packet.title;
            m_openHasOpen.gameObject.SetActive(true);
            m_openNoOpen.gameObject.SetActive(false);
        }
        else
        {//领空了
            ShowOpenAble(false);
            m_openTitleName.text = packet.name;
            m_openBlessmsg.text = packet.title;
            m_openHasOpen.gameObject.SetActive(false);
            m_openNoOpen.gameObject.SetActive(true);
        }
    }
    void ShowOpenAble(bool bShow)
    {
        m_transOpenAble.gameObject.SetActive(bShow);
        m_transOpen.gameObject.SetActive(!bShow);
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }
    protected override void OnUIBaseDestroy()
    {

    }
}
