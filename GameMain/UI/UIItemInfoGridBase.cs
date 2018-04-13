/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Base
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIItemGridBase
 * 版本号：  V1.0.0.0
 * 创建时间：2/9/2017 3:31:54 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIItemInfoGridBase : UIGridBase,IUIItemInfoGrid
{
    #region define
    public enum InfoGridType
    {
        None = 0,
        Knapsack,
        KnapsackSplit,
        KnapsackSell,
        KnapsackWareHouse,
        GrowMuhon,
        GrowForging,
        Consignment,
    }

    #endregion

    #region property
    protected UIItemInfoGrid m_baseGrid = null;
    private Transform m_ts_notEnoughGet = null;
    private bool m_bool_notEnoughGet = false;
    public bool NotEnough
    {
        get
        {
            return m_bool_notEnoughGet;
        }
    }
    #endregion

    #region initGrid
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="baseGridObj"></param>
    protected void InitItemInfoGrid(Transform baseGridTs,bool reset = false)
    {
        if (null == baseGridTs)
        {
            return;
        }
        m_baseGrid = baseGridTs.GetComponent<UIItemInfoGrid>();
        if (null == m_baseGrid)
        {
            m_baseGrid = baseGridTs.gameObject.AddComponent<UIItemInfoGrid>();
        }
        if (null != m_baseGrid)
        {
            if (reset)
            {
                m_baseGrid.Reset();
            }
            //m_baseGrid.SetTriggerEffect(true, (int)UIBase.UITriggerEffectType.Scale);
            m_baseGrid.RegisterUIEventDelegate(InfoGridUIEventDlg);
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="baseGrid"></param>
    protected void InitItemInfoGrid(UIItemInfoGrid baseGrid)
    {
        if (null == baseGrid)
        {
            return;
        }
        this.m_baseGrid = baseGrid;
        if (null != m_baseGrid)
        {
            //m_baseGrid.SetTriggerEffect(true, (int)UIBase.UITriggerEffectType.Scale);
            m_baseGrid.RegisterUIEventDelegate(InfoGridUIEventDlg);
        }
    }

    #endregion

    #region IUIItemInfoGrid

    public void SetChips(bool enable)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetChips(enable);
        }
    }

    public void SetBg(bool enable)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetBg(enable);
        }
    }

    /// <summary>
    /// 重置设为不可见
    /// </summary>
    public void ResetInfoGrid(bool infoGridVisible = true)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.Reset();
            if (m_baseGrid.Visible != infoGridVisible)
                m_baseGrid.SetVisible(infoGridVisible);

            m_baseGrid.RegisterUIEventDelegate(InfoGridUIEventDlg);
        }
        m_bool_notEnoughGet = false;
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="iconName"></param>
    /// <param name="dType"></param>
    public void SetIcon(bool enable, string iconName = "",bool makePerfectPixel = true,bool circleStyle = false)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetIcon(enable, iconName, makePerfectPixel, circleStyle);
        }
    }

    /// <summary>
    /// 设置叠加数量
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="num"></param>
    public void SetNum(bool enable, string num ="")
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetNum(enable, num);
        }
    }

    /// <summary>
    /// 设置边框
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="iconName"></param>
    public void SetBorder(bool enable, string iconName = "", bool circleStyle = false)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetBorder(enable, iconName, circleStyle);
        }
    }

    /// <summary>
    /// 设置限时标示
    /// </summary>
    /// <param name="enable"></param>
    public void SetTimeLimitMask(bool enable)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetTimeLimitMask(enable);
        }
    }

    /// <summary>
    /// 设置绑定标示
    /// </summary>
    /// <param name="enable"></param>
    public void SetBindMask(bool enable)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetLockMask(enable);
        }
    }

    /// <summary>
    /// 设置圣魂标示
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="startLv"></param>
    public void SetMuhonMask(bool enable, uint startLv = 0)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetMuhonMask(enable, startLv);
        }
    }

    /// <summary>
    /// 设置武魂等级
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="txt"></param>
    public void SetMuhonLv(bool enable,string txt ="")
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetMuhonLv(enable, txt);
        }
    }

    /// <summary>
    /// 设置符石标示
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="grade"></param>
    public void SetRuneStoneMask(bool enable, uint grade = 0)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetRunestoneMask(enable, grade);
        }
    }
    /// <summary>
    /// 设置战斗力提升标示
    /// </summary>
    /// <param name="enable"></param>
    public void SetFightPower(bool enable, bool isUp = false)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetFightPower(enable, isUp);
        }
    }

    /// <summary>
    /// 设置警告遮罩
    /// </summary>
    /// <param name="enable"></param>
    public void SetWarningMask(bool enable,bool circleStyle = false)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetWarningMask(enable, circleStyle);
        }
    }
    public void SetDurableMask(bool enable)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetDurableMask(enable);
        }
    }

    /// <summary>
    /// 设置不足
    /// </summary>
    /// <param name="enable"></param>
    public void SetNotEnoughGet(bool enable)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetNotEnoughGet(enable);
        }
    }

    public void SetItemHighLight(bool highLight)
    {
        if (null != m_baseGrid)
        {
            m_baseGrid.SetHightLight(highLight);
        }
    }

    #endregion

    #region UIEventCallBack
    protected virtual void InfoGridUIEventDlg(UIEventType eventType,object data,object param)
    {
        InvokeUIDlg(eventType, this, data);
    }
    #endregion
}