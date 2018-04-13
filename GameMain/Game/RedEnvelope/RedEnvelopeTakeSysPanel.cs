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

partial class RedEnvelopeTakeSysPanel: UIPanelBase,ITimer
{
    uint m_uRedID = 0;
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
        UIEventListener.Get(m__enter_right.gameObject).onClick = OnGrapSys;
        m_pos = m__enter_right.transform.localPosition;
        m_moneyType = GameTableManager.Instance.GetGlobalConfig<uint>("SysRedEnvelopeCurrencyType");
        m_maxNum = GameTableManager.Instance.GetGlobalConfig<uint>("SysRedEnvelopeCurrencyNum");
    }
    void OnGrapSys(GameObject go)
    {
        stRecvRedPacketChatUserCmd_CS cmd = new stRecvRedPacketChatUserCmd_CS();
        cmd.id = m_uRedID;
        NetService.Instance.Send(cmd);
        HideSelf();
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (!TimerAxis.Instance().IsExist(timerID, this))
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
            if (jumpData.Param != null)
            {
                stAddSysRedPacketChatUserCmd_S cmd = jumpData.Param as stAddSysRedPacketChatUserCmd_S;
                if (cmd != null)
                {
                    m_uRedID = cmd.id;
                    m_label_up_label.text = cmd.name;
                    if (cmd.money_type == m_moneyType)
                    {
                        if (cmd.money >= m_maxNum)
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
        m__enter_right.transform.DOShakePosition(dur, new Vector3(0, offset, 0), power);
       // m__enter_right.transform.DOLocalJump(new Vector3(m_pos.x, m_pos.y, 0), power, frequency, dur, true);
    }

    void ShowEffect(uint effectID)
    {

        UIParticleWidget p = m_trans_redeffect.GetComponent<UIParticleWidget>();
        if (p == null)
        {
            p = m_trans_redeffect.gameObject.AddComponent<UIParticleWidget>();
            p.depth = 100;
        }
        if (p != null)
        {
            p.ReleaseParticle();
            p.AddParticle(effectID);
        }
    }
    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == timerID)
        {
            ShowAni();
        }
    }

}
