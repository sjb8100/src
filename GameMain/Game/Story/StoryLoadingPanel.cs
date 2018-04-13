/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Story
 * 创建人：  wenjunhua.zqgame
 * 文件名：  StoryLoadingPanel
 * 版本号：  V1.0.0.0
 * 创建时间：3/29/2017 7:55:21 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
partial class StoryLoadingPanel
{
    #region property
    private string prefixTxt = "";
    public static string[] suffixTexts = new string[]
    {
        "",
        ".",
        "..",
        "...",
        "....",
        ".....",
        "......",
    };
    private TweenAlpha m_ta = null;
    #endregion

    #region overridemethod

    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
        mbAnimEnable = true;
    }

    protected override void OnLoading()
    {
        base.OnLoading();
        prefixTxt = DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_Txt_Story_juqingjiazaizhong);
        if (null != m_widget_Content)
        {
            m_ta = m_widget_Content.GetComponent<TweenAlpha>();
        }
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null != data && data is LocalTextType)
        {
            prefixTxt = DataManager.Manager<TextManager>().GetLocalText((LocalTextType)data);
        }
        ResetAnimData();
    }

    #endregion

    #region OP
    private string BuildLoadAnimTxt(int index)
    {
        StringBuilder builder = new StringBuilder(prefixTxt);
        if (index < suffixTexts.Length)
        {
            builder.Append(suffixTexts[index]);
        }
        return builder.ToString();
    }
    #endregion

    #region mono
    private int animIndex = 0;
    private const float ANIM_FRAME_GAP = 0.3f;
    private float frameSinceStart = 0;
    private void ResetAnimData()
    {
        animIndex = 0;
        frameSinceStart = 0;
        if (null != m_label_LoadingContent)
        {
            m_label_LoadingContent.text = BuildLoadAnimTxt(animIndex);
        }
    }
    void Update()
    {
        if (null != m_label_LoadingContent && mbAnimEnable)
        {
            if (frameSinceStart < ANIM_FRAME_GAP)
            {
                frameSinceStart += Time.deltaTime;

                if (frameSinceStart >= ANIM_FRAME_GAP)
                {
                    animIndex += 1;
                    if (animIndex > suffixTexts.Length -1)
                    {
                        animIndex = 0;
                    }
                    frameSinceStart = 0;
                    m_label_LoadingContent.text = BuildLoadAnimTxt(animIndex);
                }
            }
        }
    }
    #endregion

    #region IUIAnimation
    //动画In
    public override void AnimIn(EventDelegate.Callback onComplete)
    {
        if (null != m_ta)
        {
            if (m_ta.enabled)
                m_ta.enabled = false;
            m_ta.ResetToBeginning();
        }
    }
    //动画Out
    public override void AnimOut(EventDelegate.Callback onComplete)
    {
        base.AnimOut(onComplete);
        if (null != m_ta)
        {
            m_ta.enabled = true;
            m_ta.PlayForward();
        }
    }
    //重置动画
    public void ResetAnim()
    {
        if (null != m_ta)
            m_ta.ResetToBeginning();
    }
    #endregion
}