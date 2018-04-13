/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIProccessGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/16/2017 5:16:11 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIProccessGrid : UIItemInfoGridBase
{
    #region property
    //名称
    private UILabel mlabName;
    //数量
    private UILabel mlabNum;
    private uint m_uint_baseId;
    public uint BaseId
    {
        get
        {
            return m_uint_baseId;
        }
    }

    private Transform mtsInfos = null;
    private Transform mtsEmpty = null;
    private Transform m_ts_unload;

    public bool Empty
    {
        get
        {
            return m_uint_baseId == 0;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        mlabName = CacheTransform.Find("Content/Infos/Name").GetComponent<UILabel>(); ;
        mlabNum = CacheTransform.Find("Content/Infos/Num").GetComponent<UILabel>();
        mtsInfos = CacheTransform.Find("Content/Infos");
        mtsEmpty = CacheTransform.Find("Content/Empty");
        InitItemInfoGrid(CacheTransform.Find("Content/Infos/InfoGridRoot/InfoGrid"));
        m_ts_unload = CacheTransform.Find("Content/Unload");
        if (null != m_ts_unload)
        {
            UIEventListener.Get(m_ts_unload.gameObject).onClick = (obj) =>
            {
                InvokeUIDlg(UIEventType.Click, this, true);
            };
        }
    }

    /// <summary>
    /// 设置格子数据
    /// </summary>
    /// <param name="baseId">消耗材料id</param>
    ///  <param name="costNum">消耗数量</param>
    public void SetGridData(uint baseId, uint costNum = 1, bool needUnload = false)
    {
        bool cansee = false;
        this.m_uint_baseId = baseId;
        bool empty = baseId == 0;
        if (null != mtsEmpty && mtsEmpty.gameObject.activeSelf != empty)
        {
            mtsEmpty.gameObject.SetActive(empty);
        }

        if (null != mtsInfos && mtsInfos.gameObject.activeSelf == empty)
        {
            mtsInfos.gameObject.SetActive(!empty);
        }

        bool visible = !empty && needUnload;
        if (null != m_ts_unload && m_ts_unload.gameObject.activeSelf != visible)
        {
            m_ts_unload.gameObject.SetActive(visible);
        }

        if (!empty)
        {
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseId);
            if (null != mlabName)
            {
                mlabName.text = baseItem.Name;
            }

            int holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(baseId);
            if (null != mlabNum)
            {
                mlabNum.text = ItemDefine.BuilderStringByHoldAndNeedNum(
                        (uint)holdNum, costNum);
            }
            ResetInfoGrid();
            SetIcon(true, baseItem.Icon);
            SetBorder(true, baseItem.BorderIcon);
            SetBindMask(baseItem.IsBind);
            cansee = holdNum < costNum;
            SetNotEnoughGet(cansee);
            SetRuneStoneMask(true, (uint)baseItem.Grade);
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
                        if (BaseId != 0)
                        {
                            if (infoGrid.NotEnough)
                            {
                                InvokeUIDlg(eventType, data, m_uint_baseId);
                            }
                            else
                            {
                                TipsManager.Instance.ShowItemTips(BaseId);
                            }
                        }

                    }
                }
                break;
        }

    }
    #endregion
}
