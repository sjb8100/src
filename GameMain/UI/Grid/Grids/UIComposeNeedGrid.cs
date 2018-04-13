/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIComposeNeedGrid
 * 版本号：  V1.0.0.0
 * 创建时间：2/9/2017 2:56:27 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIComposeNeedGrid : UIItemInfoGridBase
{
    #region property
    private Transform m_ts_infoRoot;
    private uint m_uint_baseGrid = 0;
    private UILabel m_labBottomNum = null;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_ts_infoRoot = CacheTransform.Find("Content/InfoGridRoot/InfoGrid");
        m_labBottomNum = CacheTransform.Find("Content/InfoGridRoot/BottomNum").GetComponent<UILabel>();
        InitItemInfoGrid(m_ts_infoRoot);
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(depthRelease);
        }
    }
    #endregion

    #region Set
    /// <summary>
    /// 设置格子参数
    /// </summary>
    /// <param name="empty">是否为空</param>
    /// <param name="isCost">是否为消耗样式</param>
    /// <param name="baseId">表格id</param>
    /// <param name="num">数量 ： （isCost为true消耗数量，反之表示合成数量）</param>
    public void SetGridViewData(bool empty,bool isCost = true,uint baseId = 0,uint num = 0)
    {
        ResetInfoGrid(!empty);
        this.m_uint_baseGrid = baseId;

        bool bottomVisible = !empty && isCost;
        if (null != m_labBottomNum)
        {
            if (m_labBottomNum.gameObject.activeSelf != bottomVisible)
            {
                m_labBottomNum.gameObject.SetActive(bottomVisible);
            }
        }
        if (null != m_ts_infoRoot)
        {
            if (m_ts_infoRoot.gameObject.activeSelf == empty)
            {
                m_ts_infoRoot.gameObject.SetActive(!empty);
            }
            if (!empty)
            {
                BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseId);
                int holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(baseItem.BaseId);
                bool needShowNotEnough = isCost && (holdNum < num);
                string showText = num.ToString();
                if (!isCost)
                {
                    SetNum(true, showText);
                }
                if (null != m_labBottomNum)
                {
                    if (isCost)
                    {
                        showText = ItemDefine.BuilderStringByHoldAndNeedNum((uint)holdNum, num);
                        m_labBottomNum.text = showText;
                    }
                }
                
                SetNotEnoughGet(needShowNotEnough);
                SetIcon(true, baseItem.Icon);
                SetBorder(true, baseItem.BorderIcon);
                SetBindMask(baseItem.IsBind);

                if (baseItem.IsMuhon)
                {
                    SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
                    SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
                }
                else if (baseItem.IsRuneStone)
                {
                    SetRuneStoneMask(true, (uint)baseItem.Grade);
                }else if (baseItem.IsChips)
                {
                    SetChips(true);
                }

            }
        }
    }
    #endregion

    #region UIEventCallBack
    protected override void InfoGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemInfoGrid && m_uint_baseGrid != 0)
                    {
                        UIItemInfoGrid infoGrid = data as UIItemInfoGrid;
                        if (infoGrid.NotEnough)
                        {
                            TipsManager.Instance.ShowItemTips(m_uint_baseGrid);
                            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: m_uint_baseGrid);
                        }
                        else
                        {
                            TipsManager.Instance.ShowItemTips(m_uint_baseGrid);
                        }
                    }
                }
                break;
        }

    }
    #endregion
}