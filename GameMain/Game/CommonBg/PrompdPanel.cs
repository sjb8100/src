//-----------------------------------------
//此文件自动生成，请勿手动修改
//生成日期: 8/22/2016 3:35:22 PM
//-----------------------------------------
using UnityEngine;
using Client;
using System;
using Engine;
using Engine.Utility;
public class TipWidnowParam
{
    public string winTitle;
    public string winContent;
    public Action ok;
    public Action cancel;
    public Action close;
    public TipWindowType windowtype;
    public string oktxt;
    public string canletxt;
    public uint okCDTime;
    public uint cancelCDTime;
    public uint color;
}
partial class PrompdPanel : ITimer
{
    string titleStr;
    string conStr;
    Action _okAction;
    Action _cancelAction;
    Action _closeAction;
    int okCDTime;
    int cancelCDTime;

    TipWidnowParam paramTip;

    uint m_okTimerID = 199;
    uint m_cancleTimerID = 200;
    float m_startTime = 0;
    protected override void OnLoading()
    {
        base.OnLoading();
        Transform bg = transform.Find("bg");
        if (bg != null)
        {
            UIEventListener.Get(bg.gameObject).onClick = BgClick;
        }
    }
    void BgClick(GameObject go)
    {
        return;
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
    }

    private Color ConvertColor(uint color)//16进制颜色值  
    {
        return new Color()
        {
            a = 255,
            r = Convert.ToByte((color >> 16) & 255),
            g = Convert.ToByte((color >> 8) & 255),
            b = Convert.ToByte((color >> 0) & 255)
        };
    }  
    public void Init(TipWidnowParam paramTip)
    {
        titleStr = paramTip.winTitle;
        conStr = paramTip.winContent;
        _okAction = paramTip.ok;
        _cancelAction = paramTip.cancel;
        _closeAction = paramTip.close;
        m_label_name.text = titleStr;
        m_label_content_Label.text = conStr;
        okCDTime = (int)paramTip.okCDTime;
        cancelCDTime = (int)paramTip.cancelCDTime;

        if (paramTip.color == 0x000000)
        {
            m_label_content_Label.color = new Color(28f / 255f, 40f / 255f, 50f / 255f);
        }
        else
        {
            m_label_content_Label.color = Color.white;
        }
        //m_label_content_Label.color = ConvertColor(paramTip.color);


        
        this.paramTip = paramTip;

        bool cancelVisible = false;
        bool okVisible = false;
        string cancelStr = paramTip.canletxt;
        string okStr = paramTip.oktxt;
        if (paramTip.windowtype == TipWindowType.YesNO)
        {
            cancelVisible = true;
            okVisible = true;
            if (string.IsNullOrEmpty(cancelStr))
            {
                cancelStr = "否";
            }

            if (string.IsNullOrEmpty(okStr))
            {
                cancelStr = "是";
            }
        }
        else if (paramTip.windowtype == TipWindowType.CancelOk)
        {
            cancelVisible = true;
            okVisible = true;
            if (string.IsNullOrEmpty(cancelStr))
            {
                cancelStr = "取消";
            }
            if (string.IsNullOrEmpty(okStr))
            {
                cancelStr = "确定";
            }
        }
        else if (paramTip.windowtype == TipWindowType.Ok)
        {
            okVisible = true;
            m_btn_quxiao.gameObject.SetActive(false);
            Vector3 pos = m_btn_queding.transform.localPosition;
            pos.x = 0;
            m_btn_queding.transform.localPosition = pos;

            if (string.IsNullOrEmpty(okStr))
            {
                cancelStr = "确定";
            }
        }

        if (okVisible && null != m_label_queding_Name)
        {
            m_label_queding_Name.text = okStr;
        }

        if (cancelVisible && null != m_label_quxiao_Name)
        {
            m_label_quxiao_Name.text = cancelStr;
        }

        if (paramTip.ok != null)
        {
            if (okCDTime == 0)
            {
                m_label_queding_Name.text = paramTip.oktxt;
            }
            else
            {
                m_label_queding_Name.text = string.Format("{0}({1})", this.paramTip.oktxt, okCDTime);
                KillOkTimer();
                TimerAxis.Instance().SetTimer(m_okTimerID, 500, this);
                m_startTime = Time.realtimeSinceStartup;
            }
        }
        if (paramTip.cancel != null)
        {
            if (cancelCDTime == 0)
            {
                m_label_quxiao_Name.text = paramTip.canletxt;
            }
            else
            {
                m_label_quxiao_Name.text = string.Format("{0}({1})", this.paramTip.canletxt, cancelCDTime);
                KillCancleTimer();
                TimerAxis.Instance().SetTimer(m_cancleTimerID, 500, this);
                m_startTime = Time.realtimeSinceStartup;

            }
        }
    }
    void KillOkTimer()
    {
        if (TimerAxis.Instance().IsExist(m_okTimerID, this))
        {
            TimerAxis.Instance().KillTimer(m_okTimerID, this);
        }
    }
    void KillCancleTimer()
    {
        if (TimerAxis.Instance().IsExist(m_cancleTimerID, this))
        {
            TimerAxis.Instance().KillTimer(m_cancleTimerID, this);
        }
    }
    protected override void OnHide()
    {
        KillCancleTimer();
        KillOkTimer();
    }

    void onClick_Close_Btn(GameObject caster)
    {
        OnCloseCallBack();
    }

    void onClick_Quxiao_Btn(GameObject caster)
    {
        OnCancleCallBack();
    }

    void onClick_Queding_Btn(GameObject caster)
    {
        OnOkCallBack();
    }

    void OnOkCallBack()
    {
        if (_okAction != null)
        {
            _okAction();
        }
        KillOkTimer();
        KillCancleTimer();
        HideSelf();
    }
    void OnCloseCallBack() 
    {
        if (_closeAction != null)
        {
            _closeAction();
        }
        KillOkTimer();
        KillCancleTimer();
        HideSelf();
    }
    void OnCancleCallBack()
    {
        if (_cancelAction != null)
        {
            _cancelAction();
        }
        KillOkTimer();
        KillCancleTimer();
        HideSelf();
    }
    public void OnTimer(uint uTimerID)
    {
        float delta = Time.realtimeSinceStartup - m_startTime;
        if (uTimerID == m_cancleTimerID)
        {
            float cancleTime = cancelCDTime - delta;
            m_label_quxiao_Name.text = string.Format("{0}({1})", this.paramTip.canletxt, (int)cancleTime);
            if (cancleTime <= 0)
            {
                OnCancleCallBack();
            }
        }
        else if (uTimerID == m_okTimerID)
        {
            float okTime = okCDTime - delta;
            m_label_queding_Name.text = string.Format("{0}({1})", this.paramTip.oktxt, (int)okTime);
            if (okTime <= 0)
            {
                OnOkCallBack();
            }
        }
    }
}
