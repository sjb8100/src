//-----------------------------------------
//此文件自动生成，请勿手动修改
//-----------------------------------------
using UnityEngine;
using Engine;
using Engine.Utility;
using System;

public class WaitPanelShowData
{
    public WaitPanelType type;  // 多余的设计
    public int cdTime;
    public string des;
    public Action timeOutDel;
    public bool useBoxMask;
    public bool showTimer = true;   // 是否显示倒计时
}
public enum WaitPanelType
{
    Reconnect = 0,//断线重连
    ArenaChallenge = 1, //武斗场挑战
    Login = 2,//登录请求
    CDKey =3, //CDKEY兑换等待
    Waitting = 4,//等待
}
partial class CommonWaitingPanel : ITimer
{
    CommonWaitDataManager WaitData
    {
        get
        {
            return DataManager.Manager<CommonWaitDataManager>();
        }
    }

    int m_nCountDown = 10;
    WaitPanelShowData m_WaitShowData = null;

    private readonly uint m_uWaitTimerID = 100;
    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();

    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        WaitPanelShowData showData = data as WaitPanelShowData;
        if(data==null)
        {
            Engine.Utility.Log.Error("CommonWaitingPanel OnShow Failed：WaitPanelShowData is null!");
            return;
        }
        if(m_WaitShowData != null)
        {
            if(m_WaitShowData.timeOutDel != null)
            {
                m_WaitShowData.timeOutDel = null;
            }
            m_WaitShowData = null;
        }
        m_WaitShowData = showData;
        m_nCountDown = m_WaitShowData.cdTime;

        m_label_countdown.text = m_WaitShowData.cdTime.ToString();
        m_label_Des.text = m_WaitShowData.des;

        if (m_label_countdown != null && m_label_countdown.gameObject!=null)
        {
            m_label_countdown.gameObject.SetActive(m_WaitShowData.showTimer);
        }

        TimerAxis.Instance().KillTimer(m_uWaitTimerID, this);
        TimerAxis.Instance().SetTimer(m_uWaitTimerID, 1000, this);

        if (m_WaitShowData.useBoxMask == true)
        {

        }
        else 
        {

        }

        if (m_WaitShowData.type == WaitPanelType.Reconnect)
        {
            WaitData.ReconnectTime++;
        }
        
    }
    //-------------------------------------------------------------------------------------------------------
    protected override void OnHide()
    {
        m_WaitShowData = null;
        TimerAxis.Instance().KillTimer(m_uWaitTimerID, this);
        base.OnHide();
    }

    protected override void OnDisable()
    {
        TimerAxis.Instance().KillTimer(m_uWaitTimerID, this);
        base.OnDisable();
    }
    protected override void OnPanelBaseDestory()
    {
        TimerAxis.Instance().KillTimer(m_uWaitTimerID, this);
        base.OnPanelBaseDestory();
    }

    public void OnTimer(uint uTimerID)
    {
        if (uTimerID == m_uWaitTimerID)
        {
            m_nCountDown -= 1;
            if(m_WaitShowData==null)
            {
                TimerAxis.Instance().KillTimer(m_uWaitTimerID, this);
                HideSelf();
                return;
            }

            if (m_label_countdown != null && m_WaitShowData.showTimer)
            {
                m_label_countdown.text = m_nCountDown.ToString();
            }

            if (m_nCountDown <= 0)
            {
                if (m_WaitShowData.timeOutDel != null)
                {
                    m_WaitShowData.timeOutDel.Invoke();
                }
                TimerAxis.Instance().KillTimer(m_uWaitTimerID, this);
                HideSelf();
            }
        }
    }
}
