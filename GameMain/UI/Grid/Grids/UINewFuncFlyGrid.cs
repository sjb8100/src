/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UINewFuncFlyGrid
 * 版本号：  V1.0.0.0
 * 创建时间：3/2/2017 2:23:46 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UINewFuncFlyGrid : UIGridBase
{
    #region property
    private UIPlayTween m_playTween = null;
    private TweenScale m_ts = null;
    private TweenPosition m_tp = null;
    private UISprite m_sp_icon = null;
    private UITexture m_IconTex = null;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_playTween = CacheTransform.Find("Content").GetComponent<UIPlayTween>();
        m_playTween.resetOnPlay = true;
        m_sp_icon = CacheTransform.Find("Content/Icon").GetComponent<UISprite>();
        m_IconTex = CacheTransform.Find("Content/IconTex").GetComponent<UITexture>();
        
        m_tp = CacheTransform.Find("Content").GetComponent<TweenPosition>();
        CacheTransform.Find("Content").GetComponent<TweenPosition>().worldSpace = true;
        m_ts = CacheTransform.Find("Content").GetComponent<TweenScale>();
        m_ts.onFinished.Add(new EventDelegate(() =>
            {
                AnimOut();
            }));
    }
    #endregion

    #region Op
    private GuideDefine.FuncOpenShowData m_showData = null;
    private Action<GuideDefine.FuncOpenShowData> complteCallback = null;
    private CMResAsynSeedData<CMAtlas> m_flyIconCSAD = null;
    private CMResAsynSeedData<CMTexture> m_fyIconTexCSAD = null;
    public void StartFlyToTarget(GuideDefine.FuncOpenShowData showData, Vector3 startWorldPos, Action<GuideDefine.FuncOpenShowData> complteCallback)
    {
        this.m_showData = showData;
        this.complteCallback = complteCallback;
        if (null != m_tp && null != m_ts && null != m_playTween)
        {
            if (null != showData.TargetObj)
            {
                SetVisible(true);
                m_tp.from = startWorldPos;
                m_tp.to = showData.TargetObj.transform.position;

                if (m_showData.FOT == GuideDefine.FuncOpenType.Base)
                {
                    if (null != m_IconTex && m_IconTex.cachedGameObject.activeSelf)
                    {
                        m_IconTex.cachedGameObject.SetActive(false);
                    }

                    if (null != m_sp_icon)
                    {
                        if (!m_sp_icon.cachedGameObject.activeSelf)
                        {
                            m_sp_icon.cachedGameObject.SetActive(true);
                        }
                        UIManager.GetAtlasAsyn(m_showData.Icon, ref m_flyIconCSAD, () =>
                        {
                            if (null != m_sp_icon)
                            {
                                m_sp_icon.atlas = null;
                            }
                        }, m_sp_icon);
                    }

                }
                else
                {
                    if (null != m_sp_icon && m_sp_icon.cachedGameObject.activeSelf)
                    {
                        m_sp_icon.cachedGameObject.SetActive(false);
                    }

                    if (null != m_IconTex)
                    {
                        if (!m_IconTex.cachedGameObject.activeSelf)
                        {
                            m_IconTex.cachedGameObject.SetActive(true);
                        }
                        UIManager.GetTextureAsyn(m_showData.Icon, ref m_fyIconTexCSAD, () =>
                        {
                            if (null != m_IconTex)
                            {
                                m_IconTex.mainTexture = null;
                            }
                        }, m_IconTex);
                    }

                }
                m_playTween.Play(true);
            }else
            {
                SetVisible(false);
                Release();
            }
        }
    }
    
    /// <summary>
    /// 动画播放完成
    /// </summary>
    private void AnimOut()
    {
        if (null != complteCallback)
        {
            SetVisible(false);
            Release();
            complteCallback.Invoke(m_showData);
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_flyIconCSAD)
        {
            m_flyIconCSAD.Release(true);
            m_flyIconCSAD = null;
        }

        if (null != m_fyIconTexCSAD)
        {
            m_fyIconTexCSAD.Release(true);
            m_fyIconTexCSAD = null;
        }
    }

    #endregion

}