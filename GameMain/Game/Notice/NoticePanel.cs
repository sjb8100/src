using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class NoticePanel : UIPanelBase
{
    #region property
    //确认点击响应
    private Action confirmClickAction;
    //取消点击响应
    private Action cancelClickAction;
    //中间响应
    private Action centerClickAction;
    #endregion
    #region OverrideMethod

    protected override void OnLoading()
    {
        base.OnLoading();
    }

    public override void ResetPanel()
    {
        base.ResetPanel();
        if (m_btn_Center.gameObject.activeSelf)
            m_btn_Center.gameObject.SetActive(false);
        if (m_btn_Cancel.gameObject.activeSelf)
            m_btn_Cancel.gameObject.SetActive(false);
        if (m_btn_Confirm.gameObject.activeSelf)
            m_btn_Confirm.gameObject.SetActive(false);
        ResetAnim();
    }

    protected override void OnPrepareShow(object data)
    {
        base.OnPrepareShow(data);
    }

    protected override void OnShow(object pdata)
    {
        base.OnShow(pdata);
    }

    #endregion

    #region Set

    /// <summary>
    /// 设置消息
    /// </summary>
    /// <param name="msg"></param>
    public void SetMsg (string msg)
    {
        m_label_Message.text = msg;
    }

    /// <summary>
    /// 设置中键响应
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="action"></param>
    public void SetCenterBtnCallBack(string txt,Action action = null)
    {
        m_label_CenterText.text = string.IsNullOrEmpty(txt) ? "确定" : txt;
        if (!m_btn_Center.gameObject.activeSelf)
            m_btn_Center.gameObject.SetActive(true);
        centerClickAction = action;
    }

    /// <summary>
    /// 设置确定响应
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="action"></param>
    public void SetConfirmBtnCallBack(string txt, Action action = null)
    {
        m_label_ConfirmText.text = string.IsNullOrEmpty(txt) ? "确定" : txt;
        if (!m_btn_Confirm.gameObject.activeSelf)
            m_btn_Confirm.gameObject.SetActive(true);
        confirmClickAction = action;
    }

    /// <summary>
    /// 设置取消回到
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="action"></param>
    public void SetCancelBtnCallBack(string txt, Action action = null)
    {
        m_label_CancelText.text = string.IsNullOrEmpty(txt) ? "取消" : txt;
        if (!m_btn_Center.gameObject.activeSelf)
            m_btn_Cancel.gameObject.SetActive(true);
        cancelClickAction = action;
    }

    #endregion
    #region UIEvent
    void onClick_Confirm_Btn(GameObject caster)
    {
        if (null != confirmClickAction)
        {
            confirmClickAction.Invoke();
        }
        HideSelf();
    }

    void onClick_Cancel_Btn(GameObject caster)
    {
        if (null != cancelClickAction)
        {
            cancelClickAction.Invoke();
        }
        HideSelf();
    }

    void onClick_Center_Btn(GameObject caster)
    {
        if (null != centerClickAction)
        {
            centerClickAction.Invoke();
        }
        HideSelf();
    }

    #endregion
    private TweenScale ts = null;
    #region IUIAnimation 
    //动画In
    public override void AnimIn(EventDelegate.Callback onComplete)
    {
        if (null == ts)
        {
            ts = m_trans_Content.GetComponent<TweenScale>();
        }
        EventDelegate.Set(ts.onFinished, onComplete);
        ts.PlayForward();
        ts.enabled = true;
    }
    //动画Out
    public override void AnimOut(EventDelegate.Callback onComplete)
    {
        if (null == ts)
        {
            ts = m_trans_Content.GetComponent<TweenScale>();
        }
        EventDelegate.Set(ts.onFinished, onComplete);
        ts.PlayReverse();
    }
    //重置动画
    public void ResetAnim()
    {
        //if (null != ts)
        //    ts.ResetToBeginning();
    }
    
    
    #endregion
}