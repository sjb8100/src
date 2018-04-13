/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Knapsack
 * 创建人：  wenjunhua.zqgame
 * 文件名：  KnapsackPanel_SellShop
 * 版本号：  V1.0.0.0
 * 创建时间：8/7/2017 8:24:01 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
partial class KnapsackPanel
{
    #region property
    //随身商店出售物品生成器
    //出售选中的道具字典<index,qwThisId>
    private Dictionary<uint, int> sellShopSelectIds = null;

    //装备出售档次过滤
    private int m_iSellEquipGradeFillterMask = 0;
    //
    private int m_iSellQualityFilterMask = 0;
    #endregion

    #region Init
    //单次最大出售数量
    public const int MAX_SELL_COUNT = 20;
    /// <summary>
    /// 初始化商店出售
    /// </summary>
    private void InitSellShop()
    {
        if (IsInitMode(KnapsackStatus.CarryShopSell))
        {
            return;
        }
        SetInitMode(KnapsackStatus.CarryShopSell);
        sellShopSelectIds = new Dictionary<uint, int>();
        if (null != m_ctor_SellShopGridScrollView && null != m_trans_UIItemGrid)
        {
            m_ctor_SellShopGridScrollView.RefreshCheck();
            //m_ctor_SellShopGridScrollView.Initialize<UIItemGrid>((uint)GridID.Uiitemgrid
            //    , UIManager.OnObjsCreate, UIManager.OnObjsRelease
            //    , OnSellShopGridUpdate, OnSellShopGridUIEvent);

            m_ctor_SellShopGridScrollView.Initialize<UIItemGrid>(m_trans_UIItemGrid.gameObject
               , OnSellShopGridUpdate, OnSellShopGridUIEvent);

        }
        if (null != m_btn_QualityNone)
        {
            m_btn_QualityNone.GetComponentInChildren<UILabel>().color
                = ColorManager.GetColorByQuality(ItemDefine.ItemQualityType.White);
        }
        if (null != m_btn_Quality1)
        {
            m_btn_Quality1.GetComponentInChildren<UILabel>().color
                = ColorManager.GetColorByQuality(ItemDefine.ItemQualityType.Yellow);
        }
        if (null != m_btn_Quality2)
        {
            m_btn_Quality2.GetComponentInChildren<UILabel>().color
                = ColorManager.GetColorByQuality(ItemDefine.ItemQualityType.Green);
        }
        if (null != m_btn_Quality3)
        {
            m_btn_Quality3.GetComponentInChildren<UILabel>().color
                = ColorManager.GetColorByQuality(ItemDefine.ItemQualityType.Blue);
        }
        if (null != m_btn_Quality4)
        {
            m_btn_Quality4.GetComponentInChildren<UILabel>().color
                = ColorManager.GetColorByQuality(ItemDefine.ItemQualityType.Purple);
        }
        if (null != m_btn_Quality5)
        {
            m_btn_Quality5.GetComponentInChildren<UILabel>().color
                = ColorManager.GetColorByQuality(ItemDefine.ItemQualityType.Orange);
        }
    }

    #endregion

    #region Op
    private bool m_bCreateCarryShopSell = false;
    /// <summary>
    /// 刷新出售
    /// </summary>
    private void CreateCarryShopSell()
    {
        ResetShop();
        if (null != m_ctor_SellShopGridScrollView && !m_bCreateCarryShopSell)
        {
            m_bCreateCarryShopSell = true;
            m_ctor_SellShopGridScrollView.CreateGrids(MAX_SELL_COUNT);
        }
        SetKnapsackItemType(KnapsackItemType.ItemAll);
        UpdateSellShopGainUI();
        EnableFilterUI(false);
    }

    /// <summary>
    /// 随身Sell商店格子滑动数据更新回调
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    private void OnSellShopGridUpdate(UIGridBase data, int index)
    {
        UIItemGrid itemGrid = data as UIItemGrid;

        if (sellShopSelectIds.ContainsValue(index))
        {
            uint id = 0;
            foreach (KeyValuePair<uint, int> pair in sellShopSelectIds)
            {
                if (pair.Value == index)
                {
                    id = pair.Key;
                    break;
                }
            }
            BaseItem itemData = imgr.GetBaseItemByQwThisId(id);
            itemGrid.SetGridData(UIItemInfoGridBase.InfoGridType.None, itemData);
        }
        else
            itemGrid.SetGridData(UIItemInfoGridBase.InfoGridType.None, null);
    }

    /// <summary>
    /// 随身Sell商店格子UI事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnSellShopGridUIEvent(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    UIItemGrid gird = data as UIItemGrid;
                    if (!gird.Empty && sellShopSelectIds.ContainsKey(gird.Data.QWThisID))
                    {
                        UpdateSellShopSelectData(gird.Data.QWThisID, false);
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 出售商店是否选中物品
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <returns></returns>
    public bool IsSellShopSelectItem(uint qwThisId)
    {
        if (!IsInitMode(KnapsackStatus.CarryShopSell))
        {
            return false;
        }
        return sellShopSelectIds.ContainsKey(qwThisId);
    }

    /// <summary>
    /// 刷新背包格UI
    /// </summary>
    public void UpdateSellShopGridUI(int index)
    {
        if (null != m_ctor_SellShopGridScrollView)
            m_ctor_SellShopGridScrollView.UpdateData(index);
    }

    /// <summary>
    /// 更新出售商店选中数据
    /// </summary>
    /// <param name="qwThis">物品id</param>
    /// <param name="select">是否被选中</param>
    public void UpdateSellShopSelectData(uint qwThisId, bool select, Action actionDlg = null)
    {
        if (!IsInitMode(KnapsackStatus.CarryShopSell))
        {
            return;
        }
        BaseItem baseItem = imgr.GetBaseItemByQwThisId(qwThisId);
        if (null != baseItem && !baseItem.CanSell2NPC && select)
        {
            return;
        }
        int updateIndex = 0;
        if (select && !IsSellShopSelectItem(qwThisId))
        {
            if (sellShopSelectIds.Count < MAX_SELL_COUNT)
            {
                //获取插入格子索引
                if (sellShopSelectIds.Count == 0)
                {
                    updateIndex = 0;
                }
                else
                {
                    List<int> selectIndexs = new List<int>();
                    selectIndexs.AddRange(sellShopSelectIds.Values);
                    selectIndexs.Sort((left, right) =>
                    {
                        return left - right;
                    });
                    int oldIndex = 0;
                    for (int i = 0; i <= selectIndexs.Count; i++)
                    {
                        if ((i < selectIndexs.Count && i != selectIndexs[i]) || i >= selectIndexs.Count)
                        {
                            updateIndex = i;
                            break;
                        }
                    }
                }
                sellShopSelectIds.Add(qwThisId, updateIndex);
            }
            else if (null != actionDlg)
            {
                actionDlg.Invoke();
            }
        }
        else if (!select && IsSellShopSelectItem(qwThisId))
        {
            updateIndex = sellShopSelectIds[qwThisId];
            sellShopSelectIds.Remove(qwThisId);
            UpdateKnapsackDataById(qwThisId);
            //UpdateItemDataUI(qwThisId);
        }

        UpdateSellShopGridUI(updateIndex);
        //刷新出售获得的钱币UI
        UpdateSellShopGainUI();
    }

    /// <summary>
    /// 刷新出售获得的钱币UI
    /// </summary>
    public void UpdateSellShopGainUI()
    {
        if (null == m_label_SellShopGainNum || null == m_sprite_SellShopGain)
            return;
        //1、更新获得钱币图标
        //m_sprite_SellShopGain.spriteName = "";
        //2、更新获得钱币数量
        uint totalGain = 0;
        BaseItem data = null;
        foreach (KeyValuePair<uint, int> pair in sellShopSelectIds)
        {
            data = imgr.GetBaseItemByQwThisId(pair.Key);
            if (null == data)
            {
                Engine.Utility.Log.Error(CLASS_NAME + "-> GetItemDataBy Id = {0} null", pair.Key);
                continue;
            }
            totalGain += data.BaseData.sellPrice * data.Num;
        }
        for (int i = 0; i < sellShopSelectIds.Count; i++)
        {

        }
        m_label_SellShopGainNum.text = "" + totalGain;
    }

    /// <summary>
    /// 进行自动勾选装备操作
    /// </summary>
    private void DoAutoSelectSellEquip()
    {
        if (null == gridDataList)
        {
            return;
        }
        BaseEquip baseEquip = null;
        bool select = false;
        for (int i = 0; i < gridDataList.Count; i++)
        {
            baseEquip = imgr.GetBaseItemByQwThisId<BaseEquip>(gridDataList[i]);
            if (null != baseEquip && !baseEquip.IsMuhon)
            {
                if (IsGradeFilterEnable())
                {
                    select = IsMatchGradeFilter(baseEquip.EquipGrade);
                    if (IsQualityFilterEnable())
                    {
                        select = select && IsMatchQualityFilter(baseEquip.QualityType);
                    }
                }
                else if (IsQualityFilterEnable())
                {
                    select = IsMatchQualityFilter(baseEquip.QualityType);
                }
                UpdateSellShopSelectData(gridDataList[i], select);
            }
        }
        //刷新格子状态
        UpdateKnapsackGrid();
    }

    private void UpdateSellShopCreator()
    {
        if (null != m_ctor_SellShopGridScrollView)
        {
            m_ctor_SellShopGridScrollView.UpdateActiveGridData();
        }
    }
    #endregion

    #region SellShopFilter（出售过滤）

    /// <summary>
    /// 重置
    /// </summary>
    private void ResetGradeFilterMask()
    {
        m_iSellEquipGradeFillterMask = 0;
    }

    /// <summary>
    /// 重置
    /// </summary>
    private void ResetQualityFilterMask()
    {
        m_iSellQualityFilterMask = 0;
    }

    /// <summary>
    /// 更新FilterUI状态
    /// </summary>
    private void UpdateSellFilterUIStatus()
    {
        if (m_trans_SellFilterContent.gameObject.activeSelf)
        {
            bool visible = false;
            if (null != m_sprite_GradeFilterContent && m_sprite_GradeFilterContent.gameObject.activeSelf)
            {
                m_btn_Grade1.GetComponent<UIToggle>().value = IsMatchGradeFilter(EquipDefine.EquipGradeType.One);
                m_btn_Grade2.GetComponent<UIToggle>().value = IsMatchGradeFilter(EquipDefine.EquipGradeType.Two);
                m_btn_Grade3.GetComponent<UIToggle>().value = IsMatchGradeFilter(EquipDefine.EquipGradeType.Three);
                m_btn_Grade4.GetComponent<UIToggle>().value = IsMatchGradeFilter(EquipDefine.EquipGradeType.Four);
                m_btn_Grade5.GetComponent<UIToggle>().value = IsMatchGradeFilter(EquipDefine.EquipGradeType.Five);
                m_btn_Grade6.GetComponent<UIToggle>().value = IsMatchGradeFilter(EquipDefine.EquipGradeType.Six);
                m_btn_Grade7.GetComponent<UIToggle>().value = IsMatchGradeFilter(EquipDefine.EquipGradeType.Seven);
                m_btn_GradeAll.GetComponent<UIToggle>().value = IsMatchGradeFilter(EquipDefine.EquipGradeType.One)
                    && IsMatchGradeFilter(EquipDefine.EquipGradeType.Two) && IsMatchGradeFilter(EquipDefine.EquipGradeType.Three)
                    && IsMatchGradeFilter(EquipDefine.EquipGradeType.Four) && IsMatchGradeFilter(EquipDefine.EquipGradeType.Five)
                    && IsMatchGradeFilter(EquipDefine.EquipGradeType.Six) && IsMatchGradeFilter(EquipDefine.EquipGradeType.Seven);
            }

            if (null != m_sprite_QualityFilterContent && m_sprite_QualityFilterContent.gameObject.activeSelf)
            {
                m_btn_QualityNone.GetComponent<UIToggle>().value = IsMatchQualityFilter(ItemDefine.ItemQualityType.White);
                m_btn_Quality1.GetComponent<UIToggle>().value = IsMatchQualityFilter(ItemDefine.ItemQualityType.Yellow);
                m_btn_Quality2.GetComponent<UIToggle>().value = IsMatchQualityFilter(ItemDefine.ItemQualityType.Green);
                m_btn_Quality3.GetComponent<UIToggle>().value = IsMatchQualityFilter(ItemDefine.ItemQualityType.Blue);
                m_btn_Quality4.GetComponent<UIToggle>().value = IsMatchQualityFilter(ItemDefine.ItemQualityType.Purple);
                m_btn_Quality5.GetComponent<UIToggle>().value = IsMatchQualityFilter(ItemDefine.ItemQualityType.Orange);
                m_btn_QualityAll.GetComponent<UIToggle>().value = IsMatchQualityFilter(ItemDefine.ItemQualityType.White)
                    && IsMatchQualityFilter(ItemDefine.ItemQualityType.Yellow) && IsMatchQualityFilter(ItemDefine.ItemQualityType.Green)
                    && IsMatchQualityFilter(ItemDefine.ItemQualityType.Blue) && IsMatchQualityFilter(ItemDefine.ItemQualityType.Purple)
                    && IsMatchQualityFilter(ItemDefine.ItemQualityType.Orange);
            }
        }
    }


    /// <summary>
    /// 设置档次过滤mask
    /// </summary>
    /// <param name="gradeType"></param>
    /// <param name="add">true为添加，false为移除</param>
    private void SetGradeFilterMask(EquipDefine.EquipGradeType gradeType, bool add = true, bool updateCheckStatus = true, bool updateSelect = true)
    {
        if (add)
        {
            m_iSellEquipGradeFillterMask |= (1 << (int)gradeType);
        }
        else
        {
            m_iSellEquipGradeFillterMask &= (~(1 << (int)gradeType));
        }
        if (updateCheckStatus)
        {
            UpdateSellFilterUIStatus();
        }
        if (updateSelect)
        {
            DoAutoSelectSellEquip();
        }
    }

    /// <summary>
    /// 设置品质过滤
    /// </summary>
    /// <param name="qualityType"></param>
    /// <param name="add"></param>
    private void SetQualityFiterMask(ItemDefine.ItemQualityType qualityType, bool add = true, bool updateCheckStats = true, bool updateSelect = true)
    {
        if (add)
        {
            m_iSellQualityFilterMask |= (1 << (int)qualityType);
        }
        else
        {
            m_iSellQualityFilterMask &= (~(1 << (int)qualityType));
        }

        if (updateSelect)
        {
            DoAutoSelectSellEquip();
        }

        if (updateCheckStats)
        {
            UpdateSellFilterUIStatus();
        }
    }

    /// <summary>
    /// 是否匹配档次过滤
    /// </summary>
    /// <param name="gradeType"></param>
    /// <returns></returns>
    private bool IsMatchGradeFilter(EquipDefine.EquipGradeType gradeType)
    {
        return (m_iSellEquipGradeFillterMask & (1 << (int)gradeType)) != 0;
    }

    /// <summary>
    /// 档次过滤是否可用
    /// </summary>
    /// <returns></returns>
    private bool IsGradeFilterEnable()
    {
        return m_iSellEquipGradeFillterMask != 0;
    }

    private bool IsMatchQualityFilter(ItemDefine.ItemQualityType qulityType)
    {
        return (m_iSellQualityFilterMask & (1 << (int)qulityType)) != 0;
    }

    /// <summary>
    /// 质量过滤是否可用
    /// </summary>
    /// <returns></returns>
    private bool IsQualityFilterEnable()
    {
        return m_iSellQualityFilterMask != 0;
    }

    void onClick_GradeAll_Btn(GameObject caster)
    {
        bool select = IsMatchGradeFilter(EquipDefine.EquipGradeType.One)
                    && IsMatchGradeFilter(EquipDefine.EquipGradeType.Two) && IsMatchGradeFilter(EquipDefine.EquipGradeType.Three)
                    && IsMatchGradeFilter(EquipDefine.EquipGradeType.Four) && IsMatchGradeFilter(EquipDefine.EquipGradeType.Five)
                    && IsMatchGradeFilter(EquipDefine.EquipGradeType.Six) && IsMatchGradeFilter(EquipDefine.EquipGradeType.Seven);
        for (EquipDefine.EquipGradeType i = EquipDefine.EquipGradeType.None + 1; i < EquipDefine.EquipGradeType.Max; i++)
        {
            SetGradeFilterMask(i, !select, false, false);
        }
        UpdateSellFilterUIStatus();
        DoAutoSelectSellEquip();
    }

    void onClick_Grade2_Btn(GameObject caster)
    {
        SetGradeFilterMask(EquipDefine.EquipGradeType.Two
            , !IsMatchGradeFilter(EquipDefine.EquipGradeType.Two));
    }

    void onClick_Grade3_Btn(GameObject caster)
    {
        SetGradeFilterMask(EquipDefine.EquipGradeType.Three
            , !IsMatchGradeFilter(EquipDefine.EquipGradeType.Three));
    }

    void onClick_Grade5_Btn(GameObject caster)
    {
        SetGradeFilterMask(EquipDefine.EquipGradeType.Five
            , !IsMatchGradeFilter(EquipDefine.EquipGradeType.Five));
    }

    void onClick_Grade4_Btn(GameObject caster)
    {
        SetGradeFilterMask(EquipDefine.EquipGradeType.Four
            , !IsMatchGradeFilter(EquipDefine.EquipGradeType.Four));
    }

    void onClick_Grade6_Btn(GameObject caster)
    {
        SetGradeFilterMask(EquipDefine.EquipGradeType.Six
           , !IsMatchGradeFilter(EquipDefine.EquipGradeType.Six));
    }

    void onClick_Grade7_Btn(GameObject caster)
    {
        SetGradeFilterMask(EquipDefine.EquipGradeType.Seven
           , !IsMatchGradeFilter(EquipDefine.EquipGradeType.Seven));
    }

    void onClick_Grade1_Btn(GameObject caster)
    {
        SetGradeFilterMask(EquipDefine.EquipGradeType.One
           , !IsMatchGradeFilter(EquipDefine.EquipGradeType.One));
    }

    void onClick_QualityAll_Btn(GameObject caster)
    {
        bool select = IsMatchQualityFilter(ItemDefine.ItemQualityType.White)
                    && IsMatchQualityFilter(ItemDefine.ItemQualityType.Yellow) && IsMatchQualityFilter(ItemDefine.ItemQualityType.Green)
                    && IsMatchQualityFilter(ItemDefine.ItemQualityType.Blue) && IsMatchQualityFilter(ItemDefine.ItemQualityType.Purple)
                    && IsMatchQualityFilter(ItemDefine.ItemQualityType.Orange);
        for (ItemDefine.ItemQualityType i = ItemDefine.ItemQualityType.White; i < ItemDefine.ItemQualityType.Max; i++)
        {
            SetQualityFiterMask(i, !select, false, false);
        }
        UpdateSellFilterUIStatus();
        DoAutoSelectSellEquip();
    }

    void onClick_QualityNone_Btn(GameObject caster)
    {
        SetQualityFiterMask(ItemDefine.ItemQualityType.White
            , !IsMatchQualityFilter(ItemDefine.ItemQualityType.White));
    }

    void onClick_Quality1_Btn(GameObject caster)
    {
        SetQualityFiterMask(ItemDefine.ItemQualityType.Yellow
            , !IsMatchQualityFilter(ItemDefine.ItemQualityType.Yellow));
    }

    void onClick_Quality3_Btn(GameObject caster)
    {
        SetQualityFiterMask(ItemDefine.ItemQualityType.Blue
            , !IsMatchQualityFilter(ItemDefine.ItemQualityType.Blue));
    }

    void onClick_Quality2_Btn(GameObject caster)
    {
        SetQualityFiterMask(ItemDefine.ItemQualityType.Green
            , !IsMatchQualityFilter(ItemDefine.ItemQualityType.Green));
    }

    void onClick_Quality4_Btn(GameObject caster)
    {
        SetQualityFiterMask(ItemDefine.ItemQualityType.Purple
            , !IsMatchQualityFilter(ItemDefine.ItemQualityType.Purple));
    }

    void onClick_Quality5_Btn(GameObject caster)
    {
        SetQualityFiterMask(ItemDefine.ItemQualityType.Orange
            , !IsMatchQualityFilter(ItemDefine.ItemQualityType.Orange));
    }

    /// <summary>
    /// 設置过滤UI
    /// </summary>
    /// <param name="enable"></param>
    /// <param name="isGradeFilter"></param>
    private void EnableFilterUI(bool enable, bool isGradeFilter = true)
    {
        if (null != m_trans_SellFilterContent && m_trans_SellFilterContent.gameObject.activeSelf != enable)
        {
            m_trans_SellFilterContent.gameObject.SetActive(enable);
        }

        if (enable)
        {
            if (null != m_sprite_GradeFilterContent)
            {
                if (m_sprite_GradeFilterContent.gameObject.activeSelf != isGradeFilter)
                    m_sprite_GradeFilterContent.gameObject.SetActive(isGradeFilter);
                if (isGradeFilter)
                {
                    m_btn_GradeFilterBtn.GetComponentInChildren<TweenRotation>().Play(true);
                }
            }


            if (null != m_sprite_QualityFilterContent)
            {
                if (m_sprite_QualityFilterContent.gameObject.activeSelf == isGradeFilter)
                    m_sprite_QualityFilterContent.gameObject.SetActive(!isGradeFilter);
                if (!isGradeFilter)
                {
                    m_btn_QualityFilterBtn.GetComponentInChildren<TweenRotation>().Play(true);
                }
            }
            UpdateSellFilterUIStatus();
        }
        else
        {
            if (null != m_btn_QualityFilterBtn)
            {
                m_btn_QualityFilterBtn.GetComponentInChildren<TweenRotation>().Play(false);
            }

            if (null != m_btn_GradeFilterBtn)
            {
                m_btn_GradeFilterBtn.GetComponentInChildren<TweenRotation>().Play(false);
            }
        }
    }

    void onClick_GradeFilterBtn_Btn(GameObject caster)
    {
        EnableFilterUI(true);
    }

    void onClick_QualityFilterBtn_Btn(GameObject caster)
    {
        EnableFilterUI(true, false);
    }

    void onClick_SellFillterCollider_Btn(GameObject caster)
    {
        EnableFilterUI(false);
    }
    #endregion
}