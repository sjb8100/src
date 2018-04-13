/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIZFGrid
 * 版本号：  V1.0.0.0
 * 创建时间：5/17/2017 11:34:47 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UIZFGrid : UIItemInfoGridBase
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
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    public override void SetGridData(object data)
    {
        base.SetGridData(data);
        if (null == data || !(data is uint))
        {
            return;
        }
        this.m_uint_id = (uint)data;
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_uint_id);
        bool cansee = false;
        int holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(m_uint_id);
        SetIcon(true, baseItem.Icon);
        SetBorder(true, baseItem.BorderIcon);
        SetBindMask(baseItem.IsBind);
        cansee = holdNum < 1;
        SetNotEnoughGet(cansee);
        SetNum(true, ItemDefine.BuilderStringByHoldAndNeedNum((uint)holdNum,1));
        SetIcon(true, baseItem.Icon);
        SetBorder(true, baseItem.BorderIcon);
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

    #region Set
    /// <summary>
    /// 设置名称
    /// </summary>
    /// <param name="name"></param>
    private void SetName(UILabel label, string name = "")
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