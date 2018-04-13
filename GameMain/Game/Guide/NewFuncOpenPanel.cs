/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Guide
 * 创建人：  wenjunhua.zqgame
 * 文件名：  NewFuncOpenPanel
 * 版本号：  V1.0.0.0
 * 创建时间：2/28/2017 5:06:47 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class NewFuncOpenPanel
{
    #region property
    private GuideDefine.FuncOpenShowData m_data;
    #endregion

    #region overridemethod
    protected override void OnInitPanelData()
    {
        mbAnimEnable = true;
    }
    protected override void OnLoading()
    {
        base.OnLoading();
        
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null != m_sprite_Icon)
        {
            DataManager.Manager<GuideManager>().NewFuncFlyStartPos = m_sprite_Icon.transform.position;
        }
        if (null != data && data is GuideDefine.FuncOpenShowData)
        {
            this.m_data = data as GuideDefine.FuncOpenShowData;

            if (null != m_label_NewFuncName)
            {
                m_label_NewFuncName.text = m_data.Name;
            }

            if (m_data.FOT == GuideDefine.FuncOpenType.Base)
            {
                if (null != m__TexIcon && m__TexIcon.cachedGameObject.activeSelf)
                {
                    m__TexIcon.cachedGameObject.SetActive(false);
                }

                if (null != m_sprite_Icon)
                {
                    if (!m_sprite_Icon.cachedGameObject.activeSelf)
                    {
                        m_sprite_Icon.cachedGameObject.SetActive(true);
                    }
                    UIManager.GetAtlasAsyn(m_data.Icon, ref m_flyCASD, () =>
                    {
                        if (null != m_sprite_Icon)
                        {
                            m_sprite_Icon.atlas = null;
                        }
                    }, m_sprite_Icon);
                }
               
            }else
            {
                if (null != m_sprite_Icon && m_sprite_Icon.cachedGameObject.activeSelf)
                {
                    m_sprite_Icon.cachedGameObject.SetActive(false);
                }

                if (null != m__TexIcon)
                {
                    if (!m__TexIcon.cachedGameObject.activeSelf)
                    {
                        m__TexIcon.cachedGameObject.SetActive(true);
                    }
                    UIManager.GetTextureAsyn(m_data.Icon, ref m_flyTexCASD, () =>
                    {
                        if (null != m__TexIcon)
                        {
                            m__TexIcon.mainTexture = null;
                        }
                    }, m__TexIcon);
                }
                
            }
            
            if (null != m_label_NewFuncOpenTxt)
            {
                m_label_NewFuncOpenTxt.text = m_data.Title;
            }
        }
        m_float_sinceShow = 0;
    }


    protected override void OnHide()
    {
        base.OnHide();
        Release();
        //tp.onFinished.Clear();
        if (null == m_data)
        {
            Engine.Utility.Log.Error("UIEventHandler dispatch UIEventNewFuncOpenRead faield,data null!");
            return ;
        }
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTNEWFUNCOPENREAD, m_data);
    }

    private CMResAsynSeedData<CMAtlas> m_flyCASD = null;
    private CMResAsynSeedData<CMTexture> m_flyTexCASD = null;
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_flyCASD)
        {
            m_flyCASD.Release(depthRelease);
            m_flyCASD = null; 
        }

        if (null != m_flyTexCASD)
        {
            m_flyTexCASD.Release(depthRelease);
            m_flyTexCASD = null;
        }
    }

    public override void OnColliderMaskClicked()
    {
        base.OnColliderMaskClicked();
        if (!m_bool_playAnim)
        {
            HideSelf();
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

    void onClick_ClickEventObj_Btn(GameObject caster)
    {
        HideSelf();
    }
    #endregion

    float m_float_sinceShow = 0;
    void Update()
    {
        if (m_float_sinceShow < GuideManager.NewFuncAutoCloseDelay)
        {
            m_float_sinceShow += Time.deltaTime;
            if (m_float_sinceShow >= GuideManager.NewFuncAutoCloseDelay)
            {
                HideSelf();
            }
        }
    }
    
    #region IUIAnimation
    private TweenPosition tp;
    private bool m_bool_playAnim = false;
    //动画In
    public override void AnimIn(EventDelegate.Callback onComplete)
    {
        if (null == tp)
        {
            tp = m_widget_Content.GetComponent<TweenPosition>();
        }
        if (null != tp)
            tp.ResetToBeginning();
        
        EventDelegate.Callback animInAction = () =>
            {
                m_bool_playAnim = false;
            };
        if (null == onComplete)
        {
            onComplete = animInAction;
        }else
        {
            onComplete += animInAction;
        }
        
        tp.onFinished.Clear();
        EventDelegate.Set(tp.onFinished, onComplete);
        tp.PlayForward();
        tp.enabled = true;
        m_bool_playAnim = true;
    }
    //动画Out
    public override void AnimOut(EventDelegate.Callback onComplete)
    {
        base.AnimOut(onComplete);
        //if (null == ta)
        //{
        //    ta = m_widget_Content.GetComponent<TweenAlpha>();
        //}
        //tp.onFinished.Clear();
        //EventDelegate d = new EventDelegate(onComplete);
        //d.oneShot = true;
        //EventDelegate.Set(tp.onFinished, d);
        //tp.PlayForward();
    }
    //重置动画
    public void ResetAnim()
    {
        if (null != tp)
            tp.ResetToBeginning();
    }
    #endregion
}