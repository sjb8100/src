/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.UI.Grid.Grids
 * 创建人：  wenjunhua.zqgame
 * 文件名：  UIItemGrowCostGrid
 * 版本号：  V1.0.0.0
 * 创建时间：2/10/2017 2:07:17 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class UIItemGrowCostGrid : UIItemInfoGridBase
{
    #region property
    //名称
    private UILabel mlabName;
    //数量
    private UILabel mlabNum;
    //消耗代币
    private Transform m_ts_costDQ;
    private uint m_uint_baseId;
    public uint BaseId
    {
        get
        {
            return m_uint_baseId;
        }
    }
    #endregion

    #region overridemethod
    protected override void OnAwake()
    {
        base.OnAwake();
        mlabName = CacheTransform.Find("Content/Name").GetComponent<UILabel>();;
        mlabNum = CacheTransform.Find("Content/Num").GetComponent<UILabel>();
        m_ts_costDQ = CacheTransform.Find("Content/CostDQ");
        InitItemInfoGrid(CacheTransform.Find("Content/InfoGridRoot/InfoGrid"));
    }

    /// <summary>
    /// 设置格子数据
    /// </summary>
    /// <param name="baseId">消耗材料id</param>
    /// <param name="num">（useDq为true代表替代点券数量，反之消耗数量）</param>
    /// <param name="useDq">是否使用货币代替</param>
    ///  <param name="mType">useDq 为true 有效</param>
    public void SetGridData(uint baseId, uint num
        , bool useDq = false, uint costNum = 0
        ,GameCmd.MoneyType mType = GameCmd.MoneyType.MoneyType_Gold)
    {
        bool cansee = false;
        this.m_uint_baseId = baseId;
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(baseId);
        if (null != mlabName)
        {
            mlabName.text = baseItem.Name;
        }
        
        int holdNum = 0;
        if (null != mlabNum)
        {
            holdNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(baseId);
            cansee = !useDq;
            if (mlabNum.gameObject.activeSelf != cansee)
                mlabNum.gameObject.SetActive(cansee);
            if (cansee)
            {
                if (num != 0)
                {
                    mlabNum.text = ItemDefine.BuilderStringByHoldAndNeedNum(
                    (uint)holdNum, num);
                }
                else
                {
                    mlabNum.text = "0/0";
                }
                
            }
        }
        ResetInfoGrid();
        SetIcon(true, baseItem.Icon);
        SetBorder(true, baseItem.BorderIcon);
        SetBindMask(baseItem.IsBind);
        cansee = (holdNum < num || (num == 0 && holdNum == num ));
        SetNotEnoughGet(cansee);

        bool fightPowerUp = false;
        if (baseItem.IsEquip
            && DataManager.Manager<EquipManager>().IsEquipNeedFightPowerMask(baseItem.BaseId, out fightPowerUp))
        {
            SetFightPower(true, fightPowerUp);
        }

        if (baseItem.IsMuhon)
        {
            SetMuhonMask(true, Muhon.GetMuhonStarLevel(baseItem.BaseId));
            SetMuhonLv(true, Muhon.GetMuhonLv(baseItem));
        }
        else if (baseItem.IsRuneStone)
        {
            SetRuneStoneMask(true, (uint)baseItem.Grade);
        }
        
        if (null != m_ts_costDQ)
        {
            cansee = useDq;
            if (m_ts_costDQ.gameObject.activeSelf != cansee)
                m_ts_costDQ.gameObject.SetActive(cansee);
            if (cansee)
            {
                UICurrencyGrid costGrid = m_ts_costDQ.GetComponent<UICurrencyGrid>();
                if (null == costGrid)
                {
                    costGrid = m_ts_costDQ.gameObject.AddComponent<UICurrencyGrid>();
                }
                costGrid.SetGridData(
                    new UICurrencyGrid.UICurrencyGridData(MallDefine.GetCurrencyIconNameByType(mType), costNum));
            }
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