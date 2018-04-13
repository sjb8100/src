/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Knapsack
 * 创建人：  wenjunhua.zqgame
 * 文件名：  KnapsackPanel_CarryShop
 * 版本号：  V1.0.0.0
 * 创建时间：10/15/2016 12:45:27 PM
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
    
    //当前选中购买的道具id
    private uint carryShopSelectId = 0;
    //页签
    //商城物品
    private List<uint> m_lst_shopids = null;
    //商品页签信息
    private List<int> m_lst_shoptabs = null;
    //当前活动页签
    private int activeTabId = 0;
    private bool initCarryShop = false;
    #endregion

    /// <summary>
    /// 构建随身包裹页签数据
    /// </summary>
    private void StructCarryShopTabData()
    {
        if (null == m_lst_shoptabs)
            m_lst_shoptabs = new List<int>();
        m_lst_shoptabs.Clear();
        m_lst_shoptabs.AddRange(
            DataManager.Manager<MallManager>().GetMallTagDatas((int)GameCmd.CommonStore.CommonStore_Three));
    }

    /// <summary>
    /// 构建随身包裹当前页签数据
    /// </summary>
    private void StructCarryShopData()
    {
        if (null == m_lst_shopids)
            m_lst_shopids = new List<uint>();
        m_lst_shopids.Clear();
        m_lst_shopids.AddRange(
            DataManager.Manager<MallManager>().GetMallDatas((int)GameCmd.CommonStore.CommonStore_Three, activeTabId));
    }

    /// <summary>
    /// 创建格子UI列表
    /// </summary>
    private void CreateCarryShopUIList()
    {
        StructCarryShopData();
        if (null != m_ctor_CarryShopGridScrollView)
        {
            m_ctor_CarryShopGridScrollView.CreateGrids(m_lst_shopids.Count);
            uint nextActiveItemId = 0;
            if (m_lst_shopids.Count > 0)
                nextActiveItemId = m_lst_shopids[0];
            SetSelectItemId(nextActiveItemId);
        }
    }

    /// <summary>
    /// 设置活动标签
    /// </summary>
    /// <param name="tabId"></param>
    public void SetActiveTab(int tabId,bool force = false)
    {
        if (tabId == activeTabId && !force)
            return;

        if (null != m_ctor_CarryShopTabScrollView)
        {
            UITabGrid grid = m_ctor_CarryShopTabScrollView.GetGrid<UITabGrid>(m_lst_shoptabs.IndexOf(activeTabId));
            if (null != grid)
            {
                grid.SetHightLight(false);
            }
            grid = m_ctor_CarryShopTabScrollView.GetGrid<UITabGrid>(m_lst_shoptabs.IndexOf(tabId));
            if (null != grid)
            {
                grid.SetHightLight(true);
            }
        }
        activeTabId = tabId;
        CreateCarryShopUIList();
    }

    /// <summary>
    /// 设置选中数据
    /// </summary>
    /// <param name="mallItemId"></param>
    public void SetSelectItemId(uint mallItemId, bool force = false, bool needFocus = false, bool isBackSpacing = false)
    {
        if (this.selectMallItemId == mallItemId && !force)
            return;
        if (null != m_ctor_CarryShopGridScrollView)
        {

            UIMallGrid grid = m_ctor_CarryShopGridScrollView.GetGrid<UIMallGrid>(m_lst_shopids.IndexOf(selectMallItemId));
            if (null != grid)
            {
                grid.SetHightLight(false);
            }
            grid = m_ctor_CarryShopGridScrollView.GetGrid<UIMallGrid>(m_lst_shopids.IndexOf(mallItemId));
            if (null != grid)
            {
                grid.SetHightLight(true);
            }

            if (needFocus)
            {
                m_ctor_CarryShopGridScrollView.FocusGrid(m_lst_shopids.IndexOf(mallItemId));
            }
        }
        //重置购买数量
        if (!isBackSpacing)
        {
            purchaseNum = 1;
        }
        this.selectMallItemId = mallItemId;
        //检测tag是否变化
        if (null != CurrentMallData)
        {
            CurrentMallData.CheckTagStatus();
        }
        ImmediatelyRefreshLeftTime();
        UpdatePurchaseInfo();
    }

    //更新商城刷新间隔
    public const float MALL_REFRESH_LEFTTIME_GAP = 1F;
    //下一次刷新商城打折剩余的时间
    private float next_refresh_left_time = 0;
    private void ImmediatelyRefreshLeftTime()
    {
        next_refresh_left_time = 0;
    }

    /// <summary>
    /// 刷新购买信息
    /// </summary>
    private void UpdatePurchaseInfo()
    {
        bool enable = IsSelectItemEnable();
        if (null != m_trans_PurchaseContent
            && m_trans_PurchaseContent.gameObject.activeSelf != enable)
        {
            m_trans_PurchaseContent.gameObject.SetActive(enable);
        }
        if (!enable)
        {
            return;
        }
        MallDefine.MallLocalData current = CurrentMallData;
        bool mallInfoVisible = (null != current) ? true : false;
        if (null != m_trans_MallItemInfo && m_trans_MallItemInfo.gameObject.activeSelf != mallInfoVisible)
            m_trans_MallItemInfo.gameObject.SetActive(mallInfoVisible);
        if (mallInfoVisible)
        {
            if (null != m_label_MallItemName)
                m_label_MallItemName.text = current.LocalItem.Name;
            if (null != m_label_MallItemUseLv)
            {
                ColorType color = (current.LocalItem.UseLv > DataManager.Instance.PlayerLv) ? ColorType.Red : ColorType.JZRY_Green;
                m_label_MallItemUseLv.text = DataManager.Manager<TextManager>()
                    .GetLocalFormatText(LocalTextType.Local_TXT_Mall_UselevelDescribe
                    , ColorManager.GetNGUIColorOfType(color), current.LocalItem.UseLv);
            }

            if (null != m_label_MallItemDes)
                m_label_MallItemDes.text = current.LocalItem.DesNoColor;

            if (null != m_mallItemBaseGrid)
            {
                m_mallItemBaseGrid.Reset();
                m_mallItemBaseGrid.SetIcon(true, current.LocalItem.Icon);
                m_mallItemBaseGrid.SetBorder(true, current.LocalItem.BorderIcon);
                if (current.LocalItem.IsMuhon)
                {
                    m_mallItemBaseGrid.SetMuhonMask(enable
                        , Muhon.GetMuhonStarLevel(current.LocalItem.BaseId));
                    m_mallItemBaseGrid.SetMuhonLv(true, Muhon.GetMuhonLv(current.LocalItem));
                }
                else if (current.LocalItem.IsRuneStone)
                {
                    m_mallItemBaseGrid.SetRunestoneMask(enable, (uint)current.LocalItem.Grade);
                }

            }
        }
        if (null != m_label_ChooseMallItemNotice && m_label_ChooseMallItemNotice.gameObject.activeSelf == mallInfoVisible)
            m_label_ChooseMallItemNotice.gameObject.SetActive(!mallInfoVisible);

        UpdateLeftTimes();

        if (null != m_label_DiscountLeftTime)
        {
            bool visible = current.IsInDiscount && current.HasSchedule;
            if (m_label_DiscountLeftTime.gameObject.activeSelf != visible)
            {
                m_label_DiscountLeftTime.gameObject.SetActive(visible);
            }
        }

        UpdatePurchaseNum();
    }

    private void UpdateLeftTimes()
    {
        if (!IsSelectItemEnable())
        {
            return;
        }
        if (null != m_label_PurchaseLeft)
        {
            if (m_label_PurchaseLeft.gameObject.activeSelf != CurrentMallData.IsDayNumLimit)
            {
                m_label_PurchaseLeft.gameObject.SetActive(CurrentMallData.IsDayNumLimit);
            }
            if (CurrentMallData.IsDayNumLimit)
            {
                m_label_PurchaseLeft.text = string.Format(
                    DataManager.Manager<TextManager>().GetLocalText(LocalTextType.Local_TXT_Store_Limit),
                    (CurrentMallData.LeftTimes > 0) ?
                    ColorManager.GetColorString(ColorType.Green, CurrentMallData.LeftTimes.ToString())
                    : ColorManager.GetColorString(ColorType.Red, CurrentMallData.LeftTimes.ToString()));
            }
        }
        UpdatePurchaseNum();
    }


    //创建页签
    public void CreateTab()
    {
        if (null != m_ctor_CarryShopTabScrollView)
        {
            if (m_lst_shoptabs.Count > 0)
                activeTabId = m_lst_shoptabs[0];
            int count = m_lst_shoptabs.Count;
            if (m_lst_shoptabs.Count == 1 && m_lst_shoptabs[0] == 0)
                count = 0;
            m_ctor_CarryShopTabScrollView.CreateGrids(count);
        }
    }

    /// <summary>
    /// 刷新随身包裹
    /// </summary>
    private void CreateCarryShop()
    {
        CreateTab();
        activeTabId = (m_lst_shoptabs.Count != 0) ? m_lst_shoptabs[0] : 0;
        SetActiveTab(activeTabId, true);
    }

    /// <summary>
    /// 随身商店格子滑动数据更新回调
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    private void OnCarryShopGridDataUpdate(UIGridBase grid, int index)
    {
        if (grid is UIMallGrid)
        {
            if (index >= m_lst_shopids.Count)
            {
                Engine.Utility.Log.Error("KnapsackPanel OnCarryShopGridDataUpdate faield,mallItemIdsDic data errir");
                return;
            }

            UIMallGrid mallGrid = grid as UIMallGrid;
            MallDefine.MallLocalData data = DataManager.Manager<MallManager>().GetMallLocalDataByMallItemId(m_lst_shopids[index]);
            if (null == data)
            {
                Engine.Utility.Log.Error("KnapsackPanel OnCarryShopGridDataUpdate faield,mall data errir index:{0}", index);
                return;
            }
            mallGrid.SetGridData(data.MallId);
            bool select = (data.MallId == selectMallItemId) ? true : false;
            mallGrid.SetHightLight(select);
        }
        else if (grid is UITabGrid)
        {
            if (index > m_lst_shoptabs.Count)
            {
                Engine.Utility.Log.Error("KnapsackPanel OnCarryShopGridDataUpdate faield,mallTabDic data errir");
                return;
            }
            int tabKey = m_lst_shoptabs[index];
            int startDepth = 2 + m_lst_shoptabs.Count * 2 - index * 2;
            UITabGrid tabGrid = grid as UITabGrid;
            tabGrid.SetHightLight(activeTabId == tabKey ? true : false);
            tabGrid.SetGridData(tabKey);
            tabGrid.SetName(DataManager.Manager<MallManager>().GetMallTagName((int)GameCmd.CommonStore.CommonStore_Three, tabKey));
            tabGrid.SetDepth(startDepth);
        }

    }

    /// <summary>
    /// 格子UI事件委托
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIMallGrid)
                    {
                        UIMallGrid mallGrid = data as UIMallGrid;
                        if (null != mallGrid)
                            SetSelectItemId(mallGrid.MallItemId);
                    }
                    else if (data is UITabGrid)
                    {
                        UITabGrid tabGrid = data as UITabGrid;
                        if (null != tabGrid)
                        {
                            SetActiveTab((int)tabGrid.Data);
                        }
                    }
                    else if (data is UISplitGetGrid && null != param)
                    {
                        UISplitGetGrid getGrid = data as UISplitGetGrid;
                        if (null != getGrid)
                        {
                            //点击icon弹出tips
                            TipsManager.Instance.ShowItemTips(getGrid.BaseId, needCompare: false);
                        }

                    }
                }
                break;
        }
    }

    #region Init
    /// <summary>
    /// 初始化随身商店
    /// </summary>
    private void InitCarryShop()
    {
        if (IsInitMode(KnapsackStatus.CarryShop))
        {
            return;
        }
        SetInitMode(KnapsackStatus.CarryShop);
        StructCarryShopTabData();

        Transform preObj = null;// UIManager.GetObj((uint)GridID.Uiiteminfogrid);
        if (null != m_trans_MallBaseGridRoot)
        {
            preObj = m_trans_MallBaseGridRoot.GetChild(0);
        }
        if (null != preObj && null != m_trans_MallBaseGridRoot && null == m_mallItemBaseGrid)
        {
            //Util.AddChildToTarget(m_trans_MallBaseGridRoot, preObj);
            m_mallItemBaseGrid = preObj.GetComponent<UIItemInfoGrid>();
            if (null == m_mallItemBaseGrid)
            {
                m_mallItemBaseGrid = preObj.gameObject.AddComponent<UIItemInfoGrid>();
            }
            if (null != m_mallItemBaseGrid)
            {
                m_mallItemBaseGrid.RegisterUIEventDelegate((eventType, data, param) =>
                {
                    if (eventType == UIEventType.Click)
                    {
                        if (null != CurrentMallData && null != CurrentMallData.LocalItem)
                        {
                            TipsManager.Instance.ShowItemTips(CurrentMallData.LocalItem);
                        }
                    }
                });
            }
            m_trans_MallBaseGridRoot.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        }
        if (null != m_trans_PurchaseCostGrid)
        {
            if (null == m_trans_PurchaseCostGrid.GetComponent<UICurrencyGrid>())
                m_trans_PurchaseCostGrid.gameObject.AddComponent<UICurrencyGrid>();
        }

        if (null != m_ctor_CarryShopTabScrollView && null != m_trans_UITabGrid)
        {
            m_ctor_CarryShopTabScrollView.RefreshCheck();
            m_ctor_CarryShopTabScrollView.Initialize<UITabGrid>(m_trans_UITabGrid.gameObject, OnCarryShopGridDataUpdate, OnGridUIEventDlg);
            //m_ctor_CarryShopTabScrollView.Initialize<UITabGrid>((uint)GridID.Uitabgrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease
            //, OnCarryShopGridDataUpdate, OnGridUIEventDlg);
        }
        if (null != m_ctor_CarryShopGridScrollView && null != m_trans_UIMallGrid)
        {
            m_ctor_CarryShopGridScrollView.RefreshCheck();
            //m_ctor_CarryShopGridScrollView.Initialize<UIMallGrid>((uint)GridID.Uimallgrid
            //    , UIManager.OnObjsCreate
            //    , UIManager.OnObjsRelease, OnCarryShopGridDataUpdate, OnGridUIEventDlg);
            m_ctor_CarryShopGridScrollView.Initialize<UIMallGrid>(m_trans_UIMallGrid.gameObject, OnCarryShopGridDataUpdate, OnGridUIEventDlg);
        }

    }
    #endregion

    #region Op
    /// <summary>
    /// 重置商店
    /// </summary>
    private void ResetShop()
    {
        if (IsInitMode(KnapsackStatus.CarryShopSell))
        {
            ResetGradeFilterMask();
            ResetQualityFilterMask();
            sellShopSelectIds.Clear();

            m_ctor_SellShopGridScrollView.UpdateActiveGridData();
        }
        if (IsInitMode(KnapsackStatus.CarryShop))
        {
            carryShopSelectId = 0;
            m_lst_shopids.Clear();
            activeTabId = 0;
        }
    }

    
    #endregion

    #region CarryShopEvent
    //二级活动页签
    private int mActiveSecondTab = 0;
    private UIItemInfoGrid m_mallItemBaseGrid = null;
    //当前选中id
    private uint selectMallItemId = 0;
    //购买数量
    private int purchaseNum = 1;
    //当前选中商城数据
    public MallDefine.MallLocalData CurrentMallData
    {
        get
        {
            return (selectMallItemId != 0) ? DataManager.Manager<MallManager>().GetMallLocalDataByMallItemId(selectMallItemId) : null;
        }
    }
    void onClick_PurchaseBtn_Btn(GameObject caster)
    {
        if (selectMallItemId == 0)
        {
            TipsManager.Instance.ShowTips("请选择要购买的商品");
            return;
        }
        if (purchaseNum == 0)
        {
            TipsManager.Instance.ShowTips("购买数量不能为0");
            return;
        }
        DataManager.Manager<MallManager>().PurchaseMallItem(selectMallItemId, (uint)GameCmd.CommonStore.CommonStore_Three, purchaseNum);
    }

    void onClick_BtnAdd_Btn(GameObject caster)
    {
        OnAddRemove(true);
    }

    void onClick_BtnRemove_Btn(GameObject caster)
    {
        OnAddRemove(false);
    }

    void onClick_BtnMax_Btn(GameObject caster)
    {
        OnHandInput(CurrentMallData.MaxPurchaseNum);
    }

    void onClick_HandInputBtn_Btn(GameObject caster)
    {
        if (!IsHavePurchaseTimes())
        {
            TipsManager.Instance.ShowTips("今日购买已达上限，请明日再来");
            return;
        }
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {
            mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = (uint)CurrentMallData.MaxPurchaseNum,
                onInputValue = OnHandInput,
                onClose = OnCloseInput,
                showLocalOffsetPosition = new Vector3(308, 113, 0),
            });
        }
    }

    void OnCloseInput()
    {
        if (purchaseNum == 0)
        {
            OnHandInput(1);
        }
    }

    private void OnHandInput(int num)
    {
        if (null == CurrentMallData)
        {
            return;
        }
        purchaseNum = num;
        UpdatePurchaseNum();
        if (!IsHavePurchaseTimes())
        {
            TipsManager.Instance.ShowTips("今日购买已达上限，请明日再来");
        }
    }
    /// <summary>
    /// 刷新购买数量
    /// </summary>
    private void UpdatePurchaseNum()
    {
        if (!IsSelectItemEnable())
        {
            return;
        }
        purchaseNum = Mathf.Max(0, Mathf.Min(purchaseNum, CurrentMallData.MaxPurchaseNum));
        if (null != m_label_PurchaseNum)
            m_label_PurchaseNum.text = "" + purchaseNum;
        UpdateCost();
    }

    /// <summary>
    ///  更新价格
    /// </summary>
    private void UpdateCost()
    {
        MallDefine.MallLocalData current = CurrentMallData;
        if (null != m_trans_PurchaseCostGrid)
        {
            UICurrencyGrid currencyGrid = m_trans_PurchaseCostGrid.GetComponent<UICurrencyGrid>();
            if (null != currencyGrid)
            {
                GameCmd.MoneyType moneyType = GameCmd.MoneyType.MoneyType_Coin;
                uint totalCost = 0;
                if (null != current)
                {
                    moneyType = (GameCmd.MoneyType)current.LocalMall.moneyType;
                    totalCost = (uint)((current.IsInDiscount) ? current.LocalMall.offPrice * purchaseNum
                        : current.LocalMall.buyPrice * purchaseNum);
                }
                currencyGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                    MallDefine.GetCurrencyIconNameByType((GameCmd.MoneyType)moneyType), totalCost));
            }
        }
    }

    /// <summary>
    /// 是否还有购买次数
    /// </summary>
    /// <returns></returns>
    public bool IsHavePurchaseTimes()
    {

        if (IsSelectItemEnable()
            && (!CurrentMallData.IsDayNumLimit
            || (CurrentMallData.IsDayNumLimit && CurrentMallData.LeftTimes > 0)))
        {
            return true;
        }
        return false;
    }

    public bool IsSelectItemEnable()
    {
        if (null == CurrentMallData
            || null == CurrentMallData.LocalItem
            || null == CurrentMallData.LocalMall)
        {
            return false;
        }
        return true;
    }

    private void OnAddRemove(bool add)
    {
        purchaseNum = (add) ? (purchaseNum + 1) : (purchaseNum - 1);
        UpdatePurchaseNum();
        if (!IsHavePurchaseTimes())
        {
            TipsManager.Instance.ShowTips("今日购买已达上限，请明日再来");
        }
    }

    #endregion
}