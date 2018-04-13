/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIRSInfoSelectGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/17/2017 7:42:22 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIRSInfoSelectGrid : UIItemInfoGridBase
{
    #region Property
    //装备名称
    private UILabel m_name;
    private UILabel des;
    private UIToggle m_toggle;

    private Transform mtsHightlight = null;

    private uint baseId = 0;
    public uint BaseId
    {
        get
        {
            return baseId;
        }
    }
    //不可用背景
    private Transform mtsDisableBg = null;
    private bool mbEnable = false;
    #endregion
    #region Override Method
    protected override void OnAwake()
    {
        base.OnAwake();
        m_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
        m_toggle = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        mtsDisableBg = CacheTransform.Find("Content/Disable");
        mtsHightlight = CacheTransform.Find("Content/Hightlight");
    }

    /// <summary>
    /// 设置格子数据
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="check"></param>
    /// <param name="enable"></param>
    public void SetGridViewData(uint baseId, bool check, bool enable)
    {
        this.baseId = baseId;
        RuneStone baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<RuneStone>(baseId,ItemDefine.ItemDataType.RuneStone);
        mbEnable = enable;
        SetHightLight(check);
        if (null != baseItem)
        {
            if (null != m_name)
            {
                m_name.text = baseItem.Name;
            }
            SetIcon(true, baseItem.Icon);
            SetBorder(true, baseItem.BorderIcon);
            SetBindMask(baseItem.IsBind);
            SetRuneStoneMask(true, (uint)baseItem.Grade);
            int holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(baseId);
            if (holdNum > 1)
            {
                SetNum(true, holdNum.ToString());
            }
        }
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        bool enableHightLight = mbEnable && hightLight;
        if (null != m_toggle)
        {
            if (m_toggle.value != enableHightLight)
            {
                m_toggle.value = enableHightLight;
            }
        }

        if (null != mtsDisableBg && mtsDisableBg.gameObject.activeSelf == mbEnable)
        {
            mtsDisableBg.gameObject.SetActive(!mbEnable);
        }

        if (null != mtsHightlight && mtsHightlight.gameObject.activeSelf != hightLight)
        {
            mtsHightlight.gameObject.SetActive(hightLight);
        }
    }
    public override void Reset()
    {
        base.Reset();
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
                    base.InfoGridUIEventDlg(eventType, this, param);
                    if (data is UIItemInfoGrid && baseId != 0)
                    {
                        TipsManager.Instance.ShowItemTips(baseId);
                    }
                }
                break;
        }

    }
    #endregion
}