/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids.TabGrid
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIItemGetAnimGrid
 * 版本号：  V1.0.0.0
 * 创建时间：6/20/2017 10:27:44 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIItemGetAnimGrid : UIItemInfoGridBase
{
    private UIPlayTween m_playTween = null;
    private TweenPosition m_tp = null;
    private TweenScale m_ts = null;
    private uint m_uitemId = 0;
    private int m_inum = 0;
    private Transform m_tsContent;
    private UIPanel m_panel = null;
    protected override void OnAwake()
    {
        base.OnAwake();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
        m_tsContent = CacheTransform.Find("Content");
        m_playTween = CacheTransform.Find("Content").GetComponent<UIPlayTween>();
        m_playTween.resetOnPlay = true;
        m_tp = CacheTransform.Find("Content").GetComponent<TweenPosition>();
        m_tp.method = UITweener.Method.EaseIn;
        m_ts = CacheTransform.Find("Content").GetComponent<TweenScale>();
        m_tp.worldSpace = true;
        m_panel = CacheTransform.GetComponent<UIPanel>();
        m_tp.onFinished.Add(new EventDelegate(() =>
        {
            SetVisible(false);
            AnimOut();
        }));
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        AnimOut();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
        }
    }
    public void InitGrid(uint itemId, int num,int depth,Vector3 sourceWPos, Vector3 targetWPos, Action<uint, int,UIItemGetAnimGrid> animOutCallback)
    {
        ResetInfoGrid();
        if (null != m_panel)
        {
            m_panel.depth = depth;
        }
        m_bAnimOut = false;
        m_tsContent.localPosition = Vector3.zero;
        m_tsContent.localScale = Vector3.one;
        if (null != m_ts && null != m_tp && null != m_playTween)
        {
            
            m_uitemId = itemId;
            m_ts.enabled = false;
            //m_ts.ResetToBeginning();
            m_tp.enabled = false;
            //m_tp.ResetToBeginning();
            m_inum = num;
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(itemId);
            SetIcon(true, baseItem.Icon);
            SetBorder(true, baseItem.BorderIcon);
            bool enable = baseItem.Num > 1;
            SetNum(enable, baseItem.Num.ToString());
            //SetBindMask(baseItem.IsBind);
            if (baseItem.IsMuhon)
            {
                SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
                SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
            }
            else if (baseItem.IsRuneStone)
            {
                SetRuneStoneMask(true, (uint)baseItem.Grade);
            }
            this.m_animOut = animOutCallback;
            if (null != m_tp)
            {
                m_tp.from = sourceWPos;
                m_tp.to = targetWPos;
            }
        }
    }

    public void StartFlyToTarget()
    {
        if (null != m_ts && null != m_tp && null != m_playTween)
        {
            m_playTween.Play(true);
        }
        else
        {
            SetVisible(false);
        }
        
    }

    private Action<uint, int, UIItemGetAnimGrid> m_animOut = null;
    private bool m_bAnimOut = false;
    private void AnimOut()
    {
        Release();
        if (m_bAnimOut)
        {
            return;
        }
        m_bAnimOut = true;
        if (null != m_animOut)
        {
            m_animOut.Invoke(m_uitemId, m_inum,this);
        }
    }
}