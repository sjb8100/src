using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;
using Engine.Utility;

partial class RedEnvelopePanel: UIPanelBase
{
    TabMode curMode = TabMode.ShiJie;
    RedEnvelopeDataManager m_dataManager
    {
        get
        {
            return DataManager.Manager<RedEnvelopeDataManager>();
        }
    }
    protected override void OnLoading()
    {
        base.OnLoading();
     
        InitRedScrollView();
    }

    void m_dataManager_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if(e == null)
        {
            return;
        }
        if(e.key == RedEnveLopeEvent.RefreshAllRedEnveLope.ToString())
        {
            RefreshScrollView();
        }
        else if(e.key == RedEnveLopeEvent.ShowRedEffect.ToString())
        {
            ShowEffect(50039);
        }
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_dataManager.ValueUpdateEvent += m_dataManager_ValueUpdateEvent;
        if(DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.RedEnvelopeTakePanel))
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.RedEnvelopeTakePanel);
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        m_dataManager.ValueUpdateEvent -= m_dataManager_ValueUpdateEvent;
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (jumpData != null)
        {
            if (jumpData.Param != null)
            {
                if (jumpData.Param is TabMode)
                {
                    curMode = (TabMode)jumpData.Param;
                }
            }
        }
        else
        {
            CHATTYPE chatType = DataManager.Manager<ChatDataManager>().CurChatType;
            if (chatType == CHATTYPE.CHAT_WORLD)
            {
                curMode = TabMode.ShiJie;
            }
            else
            {
                curMode = TabMode.ShiZu;
            }
        
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, (int)curMode);
        ReqRefreshAllInfo();
    }
    void InitRedScrollView()
    {
        Transform resObj = m_trans_uiRedEnvelopeGrid;
        if(resObj != null)
        {
           
            m_ctor_RedEnvelopeScrollView.Initialize<UIRedEnvelopeGrid>(resObj.gameObject, OnShowGridData, OnGridUIEventDlg);
            m_ctor_RedEnvelopeScrollView.RefreshCheck();
           
        }
    }
    void RefreshScrollView()
    {
        List<RedPacket> packets = null;
        if (curMode == TabMode.ShiJie)
        {
            packets = m_dataManager.WorldRedPackets;
        }
        else
        {
            packets = m_dataManager.ClanRedPackets;
        }
        int count = packets.Count;
        if(count == 0)
        {
            m_trans_nohave.gameObject.SetActive(true);
        }
        else
        {
            m_trans_nohave.gameObject.SetActive(false);
        }
        if(m_ctor_RedEnvelopeScrollView != null)
        {
            m_ctor_RedEnvelopeScrollView.CreateGrids(count);
        }
       
    }
    void ShowEffect(uint effectID)
    {
        Transform bg = transform.Find("Up/RedEnvelopeScrollView/gridRoot");
        if(bg != null)
        {
            UIParticleWidget p = bg.GetComponent<UIParticleWidget>();
            if (p == null)
            {
                p = bg.gameObject.AddComponent<UIParticleWidget>();
                p.depth = 100;
            }
            if (p != null)
            {
                p.ReleaseParticle();
                p.AddParticle(effectID);
            }
        }
   
    }
    public override bool OnTogglePanel(int tabType, int pageid)
    {
        curMode = (TabMode)pageid;
        if(curMode == TabMode.ShiZu)
        {
            if(!DataManager.Manager<ClanManger>().IsJoinFormal)
            {
                TipsManager.Instance.ShowTips(LocalTextType.Red_Tips_nihaimeiyoujiarushizu);
               // return false;
            }
        }
        RefreshScrollView();
        return base.OnTogglePanel(tabType, pageid);
    }
    void OnShowGridData(UIGridBase grid, int index)
    {
        UIRedEnvelopeGrid redGrid = grid as UIRedEnvelopeGrid;
        if(redGrid != null)
        {
            List<RedPacket> packets = null;
            if(curMode == TabMode.ShiJie)
            {
                packets = m_dataManager.WorldRedPackets;
            }
            else
            {
                packets = m_dataManager.ClanRedPackets;
            }
            if(index < packets.Count)
            {
                redGrid.InitRedGrid(packets[index]);
            }
        }
    }
    void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {

    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
    }
    protected override void OnUIBaseDestroy()
    {
        base.OnUIBaseDestroy();
    }

    void onClick_Btn_history_Btn(GameObject caster)
    {
        stRedPacketLogChatUserCmd_C cmd = new stRedPacketLogChatUserCmd_C();
        NetService.Instance.Send(cmd);
    }

    void onClick_Btn_fresh_Btn(GameObject caster)
    {
        ReqRefreshAllInfo();
    }
    void ReqRefreshAllInfo()
    {
        stRefreshRedPacketChatUserCmd_C cmd = new stRefreshRedPacketChatUserCmd_C();
        if (curMode == TabMode.ShiJie)
        {
            cmd.world = true;
        }
        else
        {
            cmd.world = false;
        }
        NetService.Instance.Send(cmd);
    }
    void onClick_Btn_Send_Btn(GameObject caster)
    {
        int lv = GameTableManager.Instance.GetGlobalConfig<int>("UserRedPacketLevel");
        if(MainPlayerHelper.GetPlayerLevel()<lv)
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Red_Tips_njicainengfafanghongbao, lv);
            return;
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RedEnvelopeSendPanel);
    }


}
