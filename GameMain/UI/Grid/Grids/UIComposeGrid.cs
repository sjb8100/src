/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIComposeGrid
 * 版本号：  V1.0.0.0
 * 创建时间：11/7/2016 2:16:19 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIComposeGrid : UIItemInfoGridBase
{
    #region property
    //名称
    private UILabel m_lab_name;
    //描述
    private UILabel m_lab_des;
    private uint m_uint_id;
    public uint Id
    {
        get
        {
            return m_uint_id;
        }
    }
    private UIToggle m_tg_hightlight = null;
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        m_lab_des = CacheTransform.Find("Content/Des").GetComponent<UILabel>();
        m_lab_name = CacheTransform.Find("Content/Name").GetComponent<UILabel>();
        m_tg_hightlight = CacheTransform.Find("Content/Toggle").GetComponent<UIToggle>();
        if (null != m_tg_hightlight)
        {
            m_tg_hightlight.instantTween = true;
        }
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    public override void SetGridData(object data)
    {
 	    base.SetGridData(data);
        if (null != data && data is uint)
        {
            m_uint_id = (uint) data;
        }else
        {
            m_uint_id = 0;
        }
        bool empty = false;
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_uint_id);
        empty = !baseItem.IsBaseReady;
        ResetInfoGrid(!empty);
        if (!empty)
        {
            m_uint_id = (uint)data;
            if (null != m_lab_name)
            {
                m_lab_name.text = baseItem.Name;
            }

            SetIcon(true, baseItem.Icon);
            SetBorder(true, baseItem.BorderIcon);
            SetBindMask(baseItem.IsBind);
            int num = DataManager.Manager<ItemManager>().GetItemNumByBaseId(baseItem.BaseId);
            //if (num >= 1)
            {
                SetNum(true, num.ToString());
            }
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

            ItemManager img = DataManager.Manager<ItemManager>();
            if (null != m_lab_des)
            {
                int composeNum = 0;
                table.ComposeDataBase 
                    db = GameTableManager.Instance.GetTableItem<table.ComposeDataBase>(m_uint_id);
                if (null == db)
                {
                    Engine.Utility.Log.Error("UIComposeGrid error,get ComposeDataBase exception id = {0}", m_uint_id);
                }else
                {
                    int tempComposeNum = 0;
                    //材料1
                    if (db.costItem1 != 0 && db.costNum1 >0)
                    {
                        tempComposeNum = (int)(img.GetItemNumByBaseId(db.costItem1) / db.costNum1);
                        composeNum = tempComposeNum;
                    }
                    //材料2
                    if (db.costItem2 != 0 && db.costNum2 > 0)
                    {
                        tempComposeNum = (int)(img.GetItemNumByBaseId(db.costItem2) / db.costNum2);
                        if (tempComposeNum < composeNum)
                        {
                            composeNum = tempComposeNum;
                        }
                    }
                    //材料3
                    if (db.costItem3 != 0 && db.costNum3 > 0)
                    {
                        tempComposeNum = (int)(img.GetItemNumByBaseId(db.costItem3) / db.costNum3);
                        if (tempComposeNum < composeNum)
                        {
                            composeNum = tempComposeNum;
                        }
                    }
                    //材料4
                    if (db.costItem4 != 0 && db.costNum4 > 0)
                    {
                        tempComposeNum = (int)(img.GetItemNumByBaseId(db.costItem4) / db.costNum4);
                        if (tempComposeNum < composeNum)
                        {
                            composeNum = tempComposeNum;
                        }
                    }
                    //材料5
                    if (db.costItem5 != 0 && db.costNum5 > 0)
                    {
                        tempComposeNum = (int)(img.GetItemNumByBaseId(db.costItem1) / db.costNum1);
                        if (tempComposeNum < composeNum)
                        {
                            composeNum = tempComposeNum;
                        }
                    }
                }
                m_lab_des.text = string.Format("可合成：{0}", composeNum);
            }
        }
    }

    public override void SetHightLight(bool hightLight)
    {
        base.SetHightLight(hightLight);
        if (null != m_tg_hightlight && m_tg_hightlight.value != hightLight)
        {
            m_tg_hightlight.value = hightLight;
        }
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (null != m_baseGrid)
        {
            m_baseGrid.Release(true);
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
                    if (data is UIItemInfoGrid && m_uint_id != 0)
                    {
                        base.InfoGridUIEventDlg(eventType, this, param);
                        TipsManager.Instance.ShowItemTips(m_uint_id);
                    }
                }
                break;
        }

    }
    #endregion
}