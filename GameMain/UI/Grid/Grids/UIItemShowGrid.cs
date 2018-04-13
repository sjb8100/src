/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIItemShowGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/17/2017 3:49:27 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIItemShowGrid : UIItemInfoGridBase
{
    #region define
    #endregion

    #region property
    private uint m_uint_id = 0;
    private uint ID
    {
        get
        {
            return m_uint_id;
        }
    }
    private Transform m_ts_none;
    private Transform mtsInfoRoot;
    private Transform m_tsUnload;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
        m_ts_none = CacheTransform.Find("Content/None");
        mtsInfoRoot = CacheTransform.Find("Content/InfoGridRoot");
        m_tsUnload = CacheTransform.Find("Content/InfoGridRoot/Unload");
        UIEventListener.Get(m_tsUnload.gameObject).onClick = (obj) =>
            {
                InvokeUIDlg(UIEventType.Click, this, true);
            };
    }

    public void SetGridData(uint id,bool needUnload = false,bool circleStyle = false)
    {
        this.m_uint_id = id;
        bool empty = (id == 0);
        if (null != m_ts_none && m_ts_none.gameObject.activeSelf != empty)
        {
            m_ts_none.gameObject.SetActive(empty);
        }

        if (null != m_tsUnload && m_tsUnload.gameObject.activeSelf != needUnload)
        {
            if (needUnload && !empty)
                m_tsUnload.gameObject.SetActive(needUnload);
        }

        if (null != mtsInfoRoot && mtsInfoRoot.gameObject.activeSelf == empty)
        {
            mtsInfoRoot.gameObject.SetActive(!empty);
        }
        if (!empty)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(m_uint_id);
            if (baseItem == null)
            {
                baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_uint_id);
            }

            ResetInfoGrid();
            SetBg(!circleStyle);
            bool isMatchJob = DataManager.IsMatchPalyerJob((int)baseItem.BaseData.useRole);
            bool fightPowerUp = false;
            if (isMatchJob
                && DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(baseItem.QWThisID, out fightPowerUp))
            {
                SetFightPower(true, fightPowerUp);
            }

            if (!DataManager.IsMatchPalyerLv((int)baseItem.BaseData.useLevel)
                || !isMatchJob)
            {
                SetWarningMask(true, circleStyle);
            }

            SetIcon(true, baseItem.Icon,circleStyle:circleStyle);
            SetBorder(true, baseItem.BorderIcon, circleStyle: circleStyle);
            if (baseItem.IsMuhon)
            {
                SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
                SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
            }
            else if (baseItem.IsRuneStone)
            {
                SetRuneStoneMask(true, (uint)baseItem.Grade);
            }
                
            SetBindMask(baseItem.IsBind);
        }
        
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(false);
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
                    if (data is UIItemInfoGrid)
                    {
                        UIItemInfoGrid infoGrid = data as UIItemInfoGrid;
                        if (ID != 0)
                        {
                            if (infoGrid.NotEnough)
                            {
                                InvokeUIDlg(eventType, data, ID);
                            }
                            else
                            {
                                TipsManager.Instance.ShowItemTips(ID);
                            }
                        }

                    }
                }
                break;
        }
    }
    #endregion
}