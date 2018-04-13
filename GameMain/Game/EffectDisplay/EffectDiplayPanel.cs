/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.EffectDisplay
 * 创建人：  wenjunhua.zqgame
 * 文件名：  EffectDiplayPanel
 * 版本号：  V1.0.0.0
 * 创建时间：6/28/2017 10:46:22 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class EffectDiplayPanel : IGlobalEvent
{
    #region property
    private UIParticleWidget m_paritcle;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        if (null != m_trans_ParticleEffectRoot)
        {
            m_paritcle = m_trans_ParticleEffectRoot.GetComponent<UIParticleWidget>();
            if (null == m_paritcle)
            {
                m_paritcle = m_trans_ParticleEffectRoot.gameObject.AddComponent<UIParticleWidget>();
                m_paritcle.depth = m_label_MapName.depth + 1;
            }
        }

        m_lstShowTips = new List<UITipsGrid>();
        m_lstCacheTips = new List<UITipsGrid>();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalEvent(true);
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalEvent(false);
        ReleaseAllDisplayEffect();
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTCLEARDISPLAYCACHE);
    }
    #endregion

    #region IGlobalEvent
    //注册/反注册全局事件
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTPlayDISPLAYEFFECT, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTPlayDISPLAYEFFECT, GlobalEventHandler);
        }
    }

    //全局事件处理
    public void GlobalEventHandler(int eventid, object data)
    {
        switch (eventid)
        {
            case (int)Client.GameEventID.UIEVENTPlayDISPLAYEFFECT:
                {
                    EffectDisplayManager.EffectDisplayData displayData = data as EffectDisplayManager.EffectDisplayData;
                    PlayDisplayEffect(displayData);
                }
                break;
        }
    }
    #endregion

    #region MapNameDisplay(进入地图名称显示) 粒子特效（升级、任务完成、强化）
    /// <summary>
    /// 播放展示效果
    /// </summary>
    /// <param name="displayData"></param>
    private void PlayDisplayEffect(EffectDisplayManager.EffectDisplayData displayData)
    {
        switch(displayData.DisPlayType)
        {
            case EffectDisplayManager.EffectDisplayData.EffectDisplayType.Disp_MapName:
                {
                    PlayMapDisplayEffect((string)displayData.Data);
                }
                break;
            case EffectDisplayManager.EffectDisplayData.EffectDisplayType.Disp_Partical:
                {
                    PlayParticalDisplayEffect((uint)displayData.Data);
                }
                break;
            case EffectDisplayManager.EffectDisplayData.EffectDisplayType.Disp_Tips:
                {
                    PlayTipsDisplayEffect((string)displayData.Data);
                }
                break;
        }
    }

    /// <summary>
    /// 播放进入地图名称展示
    /// </summary>
    /// <param name="mapName"></param>
    private void PlayMapDisplayEffect(string mapName)
    {
        if (null != m_label_MapName)
        {
            m_label_MapName.text = mapName;
            StopCoroutine(DelayHold());
            if (!m_label_MapName.gameObject.activeSelf)
            {
                m_label_MapName.gameObject.SetActive(true);
            }
            TweenAlpha ta = m_label_MapName.gameObject.GetComponent<TweenAlpha>();
            ta.from = 0f;
            ta.to = 1f;
            ta.onFinished.Clear();
            ta.ResetToBeginning();
            EventDelegate.Callback callback = () =>
            {
                StartCoroutine(DelayHold());
            };
            EventDelegate.Set(ta.onFinished, callback);
            ta.PlayForward();
        }
    }

    /// <summary>
    /// 延迟名称显示
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator DelayHold()
    {
        yield return new WaitForSeconds(3f);
        if (null != m_label_MapName && m_label_MapName.gameObject.activeSelf)
        {
            TweenAlpha ta = m_label_MapName.gameObject.GetComponent<TweenAlpha>();
            ta.from = 1f;
            ta.to = 0f;
            ta.ResetToBeginning();
            ta.onFinished.Clear();
            EventDelegate.Callback callback = () =>
                {
                    ResetMapDisplay();
                };
            EventDelegate.Set(ta.onFinished, callback);
            ta.PlayForward();
        }
    }

    /// <summary>
    /// 重置地图名称展示
    /// </summary>
    private void ResetMapDisplay()
    {
        if (null != m_label_MapName.gameObject)
        {
            if (m_label_MapName.gameObject.activeSelf)
            {
                m_label_MapName.gameObject.SetActive(false);
            }
            TweenAlpha ta = m_label_MapName.gameObject.GetComponent<TweenAlpha>();
            if (null != ta)
            {
                ta.onFinished.Clear();
                if (ta.enabled)
                {
                    ta.enabled = false;
                }
            }
        }
    }

    private void PlayParticalDisplayEffect(uint resID)
    {
        if (null != m_paritcle)
        {
            m_paritcle.AddParticle(resID, endCallback:OnParticleDispalyEffectComplete);
        }
    }

    private void OnParticleDispalyEffectComplete(string strEffectName, string strEvent, object param)
    {
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENTDISPLAYEFFECTCOMPLETE);
    }

    
    /// <summary>
    /// 释放所有正在执行的展示效果
    /// </summary>
    private void ReleaseAllDisplayEffect()
    {
        ResetMapDisplay();
        if (null != m_paritcle)
        {
            m_paritcle.ReleaseParticle();
        }
        ReleaseAllTips();
    }

    #endregion

    #region tips(飘字提示)
    private List<UITipsGrid> m_lstShowTips = null;
    private List<UITipsGrid> m_lstCacheTips = null;
    /// <summary>
    /// 刷新Tips位置
    /// </summary>
    private void RefreshTipsPos()
    {
        Vector3 startPos = Vector3.zero;
        Vector3 targetPos = startPos;
        if (m_lstShowTips.Count > 0)
        {
            UITipsGrid tipsGrid = null;
            while (m_lstShowTips.Count > EffectDisplayManager.MaxTipAliveNum)
            {
                ReleaseTips(m_lstShowTips[0]);
            }
            for (int i = 0, max = m_lstShowTips.Count; i < max; i++)
            {
                targetPos.y = startPos.y + (max - 1-i) * EffectDisplayManager.TipsUIGapY;
                m_lstShowTips[i].transform.localPosition = targetPos;
            }
        }
        
    }

    private UITipsGrid GetTipsGrid()
    {
        UITipsGrid grid = null;
        if (null != m_lstCacheTips && m_lstCacheTips.Count > 0)
        {
            grid = m_lstCacheTips[0];
            m_lstCacheTips.Remove(grid);
        }
        else
        {
            GameObject cloneRes = UIManager.GetResGameObj(GridID.Uitipsgrid) as GameObject;
            if (null != cloneRes)
            {
                GameObject obj = NGUITools.AddChild(m_trans_TipsEffectRoot.gameObject, cloneRes);
                grid = obj.GetComponentInChildren<UITipsGrid>();
                if (null == grid)
                    grid = obj.AddComponent<UITipsGrid>();
                grid.SetAnimDlg(OnTipsFinish);
            }
        }
        if (null != grid)
        {
            grid.SetVisible(true);
            m_lstShowTips.Add(grid);
        }
        return grid;
    }

    /// <summary>
    /// 播放飘字
    /// </summary>
    /// <param name="txt"></param>
    private void PlayTipsDisplayEffect(string txt)
    {
        UITipsGrid tipsGrid = GetTipsGrid();
        if (null != tipsGrid)
        {
            tipsGrid.PlayTips(txt);
            RefreshTipsPos();
        }
    }

    private void OnTipsFinish(UITipsGrid tips)
    {
        ReleaseTips(tips);
    }

    private void ReleaseTips(UITipsGrid tips)
    {
        if (null == tips)
            return;
        tips.Reset();
        tips.SetVisible(false);
        if (m_lstShowTips.Contains(tips))
        {
            m_lstShowTips.Remove(tips);
        }

        if (!m_lstCacheTips.Contains(tips))
        {
            m_lstCacheTips.Add(tips);
        }
    }

    private void ReleaseAllTips()
    {
        if (null != m_lstShowTips)
        {
            while(m_lstShowTips.Count >0)
            {
                ReleaseTips(m_lstShowTips[0]);
            }
        }
    }
    #endregion
}