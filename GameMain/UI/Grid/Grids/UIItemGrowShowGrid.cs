/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIItemGrowShowGrid
 * 版本号：  V1.0.0.0
 * 创建时间：2/9/2017 11:08:20 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIItemGrowShowGrid : UIItemInfoGridBase
{
    #region define
    #endregion

    #region property
    //名称
    private UILabel mlabName;
    private uint m_uint_id = 0;
    private uint ID
    {
        get
        {
            return m_uint_id;
        }
    }

    private Transform mtsInfoRoot = null;
    private Transform m_ts_none;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        mlabName = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
        m_ts_none = CacheTransform.Find("Content/None");
        mtsInfoRoot = CacheTransform.Find("Content/InfoGridRoot");
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data || !(data is uint))
        {
            return;
        }
        ResetInfoGrid();

        this.m_uint_id = (uint)data;
        bool none = (m_uint_id == 0);
        if (null != m_ts_none && m_ts_none.gameObject.activeSelf != none)
        {
            m_ts_none.gameObject.SetActive(none);
        }
        if (null != mtsInfoRoot && mtsInfoRoot.gameObject.activeSelf == none)
        {
            mtsInfoRoot.gameObject.SetActive(!none);
        }

        if (null != mlabName && mlabName.gameObject.activeSelf == none)
        {
            mlabName.gameObject.SetActive(!none);
        }
        if (!none)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(m_uint_id);
            if (baseItem == null)
            {
                baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_uint_id);
            }

            if (null != mlabName)
            {
                mlabName.text = baseItem.Name;
            }

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
                SetWarningMask(true);
            }

            SetIcon(true, baseItem.Icon);
            SetBorder(true, baseItem.BorderIcon);
            if (baseItem.IsMuhon)
            {
                SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
                SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
            }else if (baseItem.IsRuneStone)
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
            m_baseGrid.Release(depthRelease);
        }
    }
    #endregion

    #region Set
    /// <summary>
    /// 设置名称
    /// </summary>
    /// <param name="name"></param>
    private void SetName(UILabel label,string name = "")
    {
        if (null != label)
        {
            label.text = name;
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