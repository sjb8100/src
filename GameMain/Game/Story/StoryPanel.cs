/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Story
 * 创建人：  wenjunhua.zqgame
 * 文件名：  StoryPanel
 * 版本号：  V1.0.0.0
 * 创建时间：3/14/2017 10:59:18 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public partial class StoryPanel
{
    #region define
    /// <summary>
    /// 剧情数据
    /// </summary>
    public class StoryData
    {
        public string Des = "";
        public bool ShowSkip = false;
        public Action SkipDlg = null;
        public Action ColliderClickDlg = null;
        public string BgTexPath = "";
    }
    #endregion

    #region property
    private StoryData m_data = null;
    private Engine.ITexture m_texture = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null != data && data is StoryData)
        {
            m_data = data as StoryData;
            SetStoryContent(m_data.Des);
            SetSkipStatus(m_data.ShowSkip);
            LoadBg(m_data.BgTexPath);
        }
    }

    /// <summary>
    /// 显示状态改变
    /// </summary>
    /// <param name="visible"></param>
    protected override void OnVisibleChanged(bool visible)
    {
        base.OnVisibleChanged(visible);
        if (visible)
        {
            PlayFadeInAnim();
        }
    }

    protected override void OnHide()
    {
        base.OnHide();
        ReleaseTex();
    }

    private void ReleaseTex()
    {
        if (null != m_texture)
        {
            m_texture.Release();
            m_texture = null;
        }
        if (m__TextureBg != null)
        {
            m__TextureBg.mainTexture = null;
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        ReleaseTex();
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        if (null != m_data && null != m_data.ColliderClickDlg)
        {
            m_data.ColliderClickDlg.Invoke();
        }
    }

    #endregion

    #region Op

    /// <summary>
    /// 設置跳過按鈕狀態
    /// </summary>
    /// <param name="visible"></param>
    private void SetSkipStatus(bool visible)
    {
        if (null != m_btn_Skip && m_btn_Skip.gameObject.activeSelf != visible)
        {
            m_btn_Skip.gameObject.SetActive(visible);
        }
    }

    private void Skip()
    {
        if (null != m_data && null != m_data.SkipDlg)
            m_data.SkipDlg.Invoke();
    }

    private void LoadBg(string bgPath)
    {
        ReleaseTex();
        if (null != m__TextureBg)
        {
            if (!string.IsNullOrEmpty(bgPath))
            {
                Engine.RareEngine.Instance().GetRenderSystem().CreateTexture(ref bgPath, ref m_texture, null, null, Engine.TaskPriority.TaskPriority_Immediate);
                if (m_texture != null)
                {
                    m__TextureBg.mainTexture = m_texture.GetTexture();
                    //m__TextureBg.MakePixelPerfect();
                }
            }
        }
    }

    /// <summary>
    /// 播放淡入动画
    /// </summary>
    private void PlayFadeInAnim()
    {
        UIPlayTween[] tween = CacheTransform.Find("Content").GetComponents<UIPlayTween>();
        if (null != tween)
        {
            for(int i=0;i < tween.Length;i++)
            {
                tween[i].resetOnPlay = true;
                tween[i].Play(true);
            }
        }
    }

    /// <summary>
    /// 设置剧情内容
    /// </summary>
    /// <param name="content"></param>
    private void SetStoryContent(string content)
    {
        if (null != m_label_StoryContent)
        {
            m_label_StoryContent.text = content;
        }
    }
    #endregion

    #region UIEvent

    void onClick_Skip_Btn(GameObject caster)
    {
        Skip();
    }

    #endregion
}