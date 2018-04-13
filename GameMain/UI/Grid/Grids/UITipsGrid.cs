/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UITipsGrid
 * 版本号：  V1.0.0.0
 * 创建时间：7/19/2017 11:10:44 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UITipsGrid : UIGridBase
{
    #region property
    private UILabel m_labDes = null;
    private TweenAlpha m_ta = null;
    private Action<UITipsGrid> m_animDlg = null;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        //m_labDes = CacheTransform.Find("Content/Des").GetComponent<UIXmlRichText>();
        m_labDes = CacheTransform.Find("Content/Des").GetComponent<UILabel>();
        m_ta = CacheTransform.Find("Content/Des").GetComponent<TweenAlpha>();
        if (null != m_ta)
        {
            if (m_ta.enabled)
                m_ta.enabled = false;
        }
        SetTriggerEffect(false);
        m_btnSoundEffect = ButtonPlay.ButtonSountEffectType.None;
    }

    public override void Reset()
    {
        base.Reset();
        if (null != m_ta)
        {
            m_ta.enabled = false;
            m_ta.ResetToBeginning();
        }
        
    }
    #endregion

    #region Set

    public void PlayTips(string txt)
    {
        if (!Visible)
        {
            SetVisible(true);
        }

        if (null != m_labDes)
        {
            //m_labDes.AddXml(RichXmlHelper.RichXmlAdapt(txt));
            m_labDes.text = txt;
        }

        if (m_ta != null)
        {
            m_ta.ResetToBeginning();
            m_ta.Play(true);
        }

    }

    public void SetAnimDlg(Action<UITipsGrid> dlg)
    {
        m_animDlg = dlg;
        if (null != m_ta && null != m_animDlg)
        {
            m_ta.onFinished.Clear();
            EventDelegate.Callback callback = () =>
            {
                m_animDlg.Invoke(this);
            };
            EventDelegate.Set(m_ta.onFinished, callback);
        }
    }

    #endregion
}