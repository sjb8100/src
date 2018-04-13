/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIAwardItemGrid
 * 版本号：  V1.0.0.0
 * 创建时间：4/3/2018 2:53:49 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIOfflineRewardGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/27/2017 4:09:54 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIAwardItemGrid : UIItemInfoGridBase
{
    #region property
    private Transform m_ts_none;
    private Transform mtsInfoRoot;
    private uint baseID = 0;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
        m_ts_none = CacheTransform.Find("Content/None");
        mtsInfoRoot = CacheTransform.Find("Content/InfoGridRoot");
    }


    public void SetGridData(uint baseID, uint num)
    {
        this.baseID = baseID;
        BaseItem baseItem = null;
        if (baseID != 0)
        {
            baseItem = new BaseItem(baseID);
        }
        bool empty = (null == baseItem);
        if (null != m_ts_none && m_ts_none.gameObject.activeSelf != empty)
        {
            m_ts_none.gameObject.SetActive(empty);
        }

        if (null != mtsInfoRoot && mtsInfoRoot.gameObject.activeSelf == empty)
        {
            mtsInfoRoot.gameObject.SetActive(!empty);
        }
        if (!empty)
        {
            ResetInfoGrid();
            SetBg(true);
            bool visible = num > 1;
            if (visible)
            {
                SetNum(true, num.ToString());
            }
            SetIcon(true, baseItem.Icon);
            SetBorder(true, baseItem.BorderIcon);
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
        baseID = 0;
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
                        if (baseID != 0)
                        {
                            TipsManager.Instance.ShowItemTips(baseID);
                        }

                    }
                }
                break;
        }
    }
    #endregion
}