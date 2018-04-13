using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using GameCmd;
using Client;
using Engine.Utility;
using UnityEngine;
using DG.Tweening;


partial class RedEnvelopeTakePanel : UIPanelBase,ITimer
{
    bool bWorld = false;
    public Vector3 m_pos = Vector3.zero;
    Vector3 m_rot = Vector3.zero;
    public float offset = 5;
    public int power = 5;
    public int frequency = 4;
    public float dur = 2;
    uint timerID = 1000;

    uint m_moneyType = 1;
    uint m_maxNum = 1000000;
    protected override void OnLoading()
    {
        base.OnLoading();
        UIEventListener.Get(m__enter_left.gameObject).onClick = OnWordGrapPacket;
        m_pos = m__enter_left.transform.localPosition;
        m_moneyType = GameTableManager.Instance.GetGlobalConfig<uint>("UserRedEnvelopeCurrencyType");
        m_maxNum = GameTableManager.Instance.GetGlobalConfig<uint>("UserRedEnvelopeCurrencyNum");
       // m_rot = m__enter_left.transform.localRotation;
    }
    void OnWordGrapPacket(GameObject go)
    {
        RedEnvelopePanel.TabMode tabMode = RedEnvelopePanel.TabMode.ShiJie;
        if(!bWorld)
        {
            tabMode = RedEnvelopePanel.TabMode.ShiZu;
        }
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.RedEnvelopePanel, jumpData: new PanelJumpData() { Param = tabMode });
        HideSelf();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if(!TimerAxis.Instance().IsExist(timerID,this))
        {
            TimerAxis.Instance().SetTimer(timerID, 3000, this);
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        TimerAxis.Instance().KillTimer(timerID, this);
    }
    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (jumpData != null)
        {
            if(jumpData.Param != null)
            {
                stNoticeRedPacketChatUserCmd_S cmd = jumpData.Param as stNoticeRedPacketChatUserCmd_S;
                if(cmd != null)
                {
                    bWorld = cmd.world;
                   if(cmd.money_type == m_moneyType)
                   {
                       if(cmd.gold >= m_maxNum)
                       {
                           ShowEffect(50039);
                       }
                   }
                }
            }
        }
        ShowAni();
    }
    void ShowAni()
    {
      //  m__enter_left.transform.DOLocalJump(new Vector3(m_pos.x,m_pos.y, 0), power, frequency, dur, true);
        m__enter_left.transform.DOShakePosition(dur, new Vector3(0, offset, 0), power);
    }

    void ShowEffect(uint effectID)
    {

        UIParticleWidget p = m_trans_redeffect.GetComponent<UIParticleWidget>();
        if (p == null)
        {
            p = m_trans_redeffect.gameObject.AddComponent<UIParticleWidget>();
            p.depth = 100;
        }
        if(p != null)
        {
            p.ReleaseParticle();
            p.AddParticle(effectID);
        }
    }
    public void OnTimer(uint uTimerID)
    {
        if(uTimerID == timerID)
        {
            ShowAni();
        }
    }
}
