/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Consignment
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ConsignmentPanel_Item
 * 版本号：  V1.0.0.0
 * 创建时间：11/14/2016 9:41:04 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using table;
using GameCmd;
using Client;
using System.Collections;

partial class ConsignmentPanel
{
    #region Define
    //物品寄售类型
    public enum ConsignmentItemMode
    {
        Buy = 1,
        Sell,
        ShowItem,
        Account,
        Max,
    }
    //出售状态
    enum SellItemState
    {
        SellItem = 1,
        SellList,
        Max,
    }

    //翻页状态
    enum PageState
    {
        PreviousPage,
        NextPage,
        Max,
    }

    enum SellItemType
    {
        None = 0,
        Item,
        Pet,
        ItemEquipment,
        ItemUseable,
        ItemProps,
        ItemAll,
        Max,
    }

    /// <summary>
    /// 背包物品类型页签
    /// </summary>
    public enum KnapsackItemType
    {
        None = 0,
        ItemEquipment,
        ItemUseable,
        ItemProps,
        ItemAll,
        Max,
    }

    //const int Max_Num = 8;
    #endregion

    #region property
    //物品寄售类型
    List<ItemPriceInfo> sellList = null;
    uint baseID = 0;
    private ConsignmentItemMode m_em_itemMode = ConsignmentItemMode.Max;
    private SellItemState m_em_SellState = SellItemState.Max;
    private SellItemType m_em_sellItemType = SellItemType.Item;
    private int m_int_itemFilterMask = 0;

    private UIItemGrid m_sellItemGrid = null;

    private UISprite levelSortSprite = null;
    private UISprite moneySortSprite = null;

    UIConsignmentItemListGrid TargetBuyGrid = null;
    List<uint> canConsignItemList = new List<uint>();
    List<IPet> canConsignPetList = new List<IPet>();
    private bool needSendMsg = false;
    private uint m_uint_activeFType = 0;
    private uint m_uint_activeStype = 0;
    uint selectSecondsKey = 0;
    private List<uint> mlstFirstTabIds = null;
    private List<uint> mlstSecondTabIds = null;

    List<ConsignmentItem> m_lst_ConsignmentItems = null;


    Dictionary<uint,List<uint>> m_uintDic = new Dictionary<uint,List<uint>>();
    private UISecondTabCreatorBase mSecondTabCreator = null;
    List<ConsignmentCateIDConf> list = null;
    Dictionary<uint, List<ConsignmentCateIDConf>> dic = null;
    ConsignmentManager SaleItemDataManager
    {
        get
        {
            return DataManager.Manager<ConsignmentManager>();
        }
    }
    PetDataManager PetManager
    {
        get
        {
            return DataManager.Manager<PetDataManager>();
        }
    }


    ItemSortType consignItemSortType = ItemSortType.ItemSort_MoneyUp;
    ItemSortType ConsignItemSortType
    {
        get 
        {
            return consignItemSortType;
        }
//         get
//         {
//             return consignItemSortType;
//         }
//         set
//         {
//             consignItemSortType = value;
//             if (levelSortSprite == null)
//             {
//                 levelSortSprite = m_btn_ItemLevel.transform.Find("Arrow").GetComponent<UISprite>();   
//             }
//             if (moneySortSprite == null)
//             {
//                 moneySortSprite = m_btn_ItemUnitPrice.transform.Find("Arrow").GetComponent<UISprite>();
//             }
//             switch (consignItemSortType)
//             {
//                 case ItemSortType.ItemSort_LvDown:
//                     {
//                         levelSortSprite.cachedTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, -180));
//                         levelSortSprite.enabled = true;
//                         moneySortSprite.enabled = false;
//                     }
//                     break;
//                 case ItemSortType.ItemSort_LvUp:
//                     {
//                         levelSortSprite.cachedTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
//                         levelSortSprite.enabled = true;
//                         moneySortSprite.enabled = false;
//                     }
//                     break;
//                 case ItemSortType.ItemSort_MoneyDown:
//                     {
//                         moneySortSprite.cachedTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, -180));
//                         levelSortSprite.enabled = false;
//                         moneySortSprite.enabled = true;
//                     }
//                     break;
//                 case ItemSortType.ItemSort_MoneyUp:
//                     {
//                         moneySortSprite.cachedTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
//                         levelSortSprite.enabled = false;
//                         moneySortSprite.enabled = true;
//                     }
//                     break;
//                 default:
//                     break;
//             }
//         }
    }
    #endregion

    #region Init
    private void InitSaleItem()
    {
        InitConsignItemList();
        if (m_ctor_Toggles != null)
        {
            //GameObject resObj = UIManager.GetResGameObj(GridID.Uitabgrid) as GameObject;
            m_ctor_Toggles.Initialize<UITabGrid>(m_trans_UITabGrid.gameObject, OnUIGridUpdate, OnUIGridEventDlg);
            m_ctor_Toggles.CreateGrids(4);
            SaleItemDataManager.ValueUpdateEvent += SaleItemDataManager_ValueUpdateEvent;
        }
      
        if (null == m_sellItemGrid && m_trans_SellingItem)
        {
            GameObject prefab = m_trans_UIItemGrid.gameObject;
            GameObject cloneObj = NGUITools.AddChild(m_trans_SellingItem.gameObject, prefab);
            m_sellItemGrid = cloneObj.GetComponent<UIItemGrid>();
            if (null == m_sellItemGrid)
            {
                m_sellItemGrid = cloneObj.AddComponent<UIItemGrid>();
                
            }
            if (null != m_sellItemGrid)
            {
                m_sellItemGrid.ResetInfoGrid(false);
                m_sellItemGrid.RegisterUIEventDelegate((eventType, data, param) =>
                    {
                        if (eventType == UIEventType.Click)
                        {
                            OnSellingIconClick();
                        }
                    });
            }
        }
        UIEventListener.Get(m_label_UnitSellPrice.gameObject).onClick = OnUnitPriceClick;
        UIEventListener.Get(m_label_UnitSellNum.gameObject).onClick = OnUnitNumClick;
        SetConsignmentItemMode(ConsignmentItemMode.Buy);
        InitLeftFilter();
    
        //ConsignItemSortType = ItemSortType.ItemSort_MoneyUp;
    }

    /// <summary>
    /// 初始化左侧筛选项
    /// </summary>
    private void InitLeftFilter()
    {
        SaleItemDataManager.SetConsignmentList();  
        SaleItemDataManager.GetConsignmentCateInfo(ref list, ref dic);
        if (list != null && dic != null)
        {
            if (mSecondTabCreator == null)
            {
                if (null != m_scrollview_TypeScrollView && null == mSecondTabCreator)
                {
                    mSecondTabCreator = m_scrollview_TypeScrollView.GetComponent<UISecondTabCreatorBase>();
                    if (null == mSecondTabCreator)
                        mSecondTabCreator = m_scrollview_TypeScrollView.gameObject.AddComponent<UISecondTabCreatorBase>();
                    if (null != mSecondTabCreator)
                    {
                        GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
                        GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uisecondtypegrid) as GameObject;
                        mSecondTabCreator.Initialize<UISecondTypeGrid>(cloneFTemp, cloneSTemp
                            , OnUpdateGridData, OnUpdateSecondTabGrid, OnGridEventDlg);
//                         mSecondTabCreator.Initialize<UISecondTypeGrid>(m_trans_UICtrTypeGrid.gameObject, m_widget_UISecondTypeGrid.gameObject
//                           , OnUpdateGridData, OnUpdateSecondTabGrid, OnGridEventDlg);
                    }
                }
            }

            List<int> secondTabsNums = new List<int>();
            if (null == mlstFirstTabIds)
            {
                mlstFirstTabIds = new List<uint>();
            }
            mlstFirstTabIds.Clear();
            foreach (var i in dic)
            {
                mlstFirstTabIds.Add(i.Key);
                secondTabsNums.Add(i.Value.Count);
                for (int a = 0; a < i.Value.Count;a++ )
                {
                    if (m_uintDic.ContainsKey(i.Key))
                    {
                        m_uintDic[i.Key].Add(i.Value[a].SubID);
                    }
                    else
                    {
                        List<uint> li = new List<uint>();
                        li.Add(i.Value[a].SubID);
                        m_uintDic.Add(i.Key, li);
                    }
                  }
            }

            if (null != mSecondTabCreator)
            {
                mSecondTabCreator.CreateGrids(secondTabsNums);
            }
        }
    }

    private bool m_bInitSaleItemCreator = false;
    /// <summary>
    /// 初始化寄售列表
    /// </summary>
    private void InitConsignItemList()
    {
        if (null != m_ctor_ItemListGrid)
        {
            if (!m_bInitSaleItemCreator)
            {
                m_ctor_ItemListGrid.Initialize<UIConsignmentItemListGrid>(m_trans_UIConsignmentItemListGrid.gameObject, OnUpdateGridData, OnGridEventDlg);   
            }
        }
        m_label_PageNum.text = "1/1";
    }

    #endregion

    #region OnClick
    void OnSellingIconClick()
    {
        if (SaleItemDataManager.WantSellItem != null)
        {
            if (SaleItemDataManager.WantSellItem is BaseItem)
            {
                BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
                TipsManager.Instance.ShowItemTips(itemData.ServerData.qwThisID);
            }
        }
    }

    void OnUnitPriceClick(GameObject caster)
    {
        if (SaleItemDataManager.WantSellItem == null)
        {
            return;
        }
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {
            uint tempMax = 10000000;
            if (SaleItemDataManager.WantSellItem is BaseItem)
            {
                BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
                if (!itemData.IsTreasure)
                {               
                    return;
                }
                ConsignmentCanSellItem canSellItem = SaleItemDataManager.GetConsignmentCanSellItemInfo(itemData.BaseData.itemID);
            
                tempMax = (canSellItem != null) ? canSellItem.MaxYuanBao : tempMax;
                mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
                {
                    maxInputNum = tempMax,
                    onClose = OnClosePriceInput,
                    onInputValue = OnItemPriceConfirm,
                    showLocalOffsetPosition = new Vector3(-102, -52, 0),
                });
            }
           
        }
    }
    void OnClosePriceInput()
    {
        int tempMin = 0;
        if (int.TryParse(m_label_UnitSellPrice.text, out tempMin))
        {
            if (SaleItemDataManager.WantSellItem is BaseItem)
            {
                BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
                if (!itemData.IsTreasure)
                {
                    return;
                }
                ConsignmentCanSellItem canSellItem = SaleItemDataManager.GetConsignmentCanSellItemInfo(itemData.BaseData.itemID);
                if (canSellItem != null)
                {
                    if (tempMin < canSellItem.MinYuanBao)
                    {
                        m_label_UnitSellPrice.text = canSellItem.MinYuanBao.ToString();
                    }
                }
              
            }
        }
       
    }

    void OnUnitNumClick(GameObject caster)
    {
        if (SaleItemDataManager.WantSellItem == null)
        {
            return;
        }
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {
            uint maxNum = 1;
            if (SaleItemDataManager.WantSellItem is BaseItem)
            {
                BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
                maxNum = itemData.Num;
            }
            mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = maxNum,
                onInputValue = OnItemNumConfirm,
                onClose = OnCloseInput,
                showLocalOffsetPosition = new Vector3(-102, -52, 0),
            });
        }
    }
    void OnItemPriceConfirm(int price)
    {
        m_label_UnitSellPrice.text = price.ToString();
        m_label_TotalPriceNum.text = (price * UnitSellNum).ToString();
    }

    void OnItemNumConfirm(int num)
    {
        UnitSellNum = num;
        m_label_TotalPriceNum.text = (int.Parse(m_label_UnitSellPrice.text) * num).ToString();
        SetFactorageNum();
        m_sellItemGrid.ChangeItemNum(UnitSellNum);
    }
    void OnCloseInput()
    {
        if (UnitSellNum == 0)
        {
            OnItemNumConfirm(1);
        }
    }
    /// <summary>
    /// 设置手续费
    /// </summary>
    void SetFactorageNum()
    {
        m_label_FactorageNum.text = "0";
        if (SaleItemDataManager.WantSellItem != null)
        {
            if (SaleItemDataManager.WantSellItem is BaseItem)
            {
                BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
                ConsignmentCanSellItem sellItem = SaleItemDataManager.GetConsignmentCanSellItemInfo(itemData.BaseData.itemID);
                if (sellItem != null)
                {
                    int factorageNum = UnitSellNum * (int)sellItem.Factorage;
                    if (factorageNum > SaleItemDataManager.MaxSellCost)
                    {
                        factorageNum = SaleItemDataManager.MaxSellCost;
                    }
                    int totalCoupon = ClientGlobal.Instance().MainPlayer.GetProp((int)PlayerProp.Coupon);
                    m_label_FactorageNum.text = factorageNum.ToString();
                    m_label_FactorageNum.color = (factorageNum > totalCoupon) ? new Color(198 / 255.0f, 28 / 255.0f, 28 / 255.0f) : new Color(9 / 255.0f, 127 / 255.0f, 29 / 255.0f);
                }
            }
        }
    }

    void onClick_JobPopupList_Btn(GameObject caster)
    {
        EnableFilterUI(FilterType.job, true);
    }

    void onClick_GradePopupList_Btn(GameObject caster)
    {
        EnableFilterUI(FilterType.grade,true);
    }

    void onClick_ColourPopupList_Btn(GameObject caster)
    {
    }

    void onClick_BtnSearch_Btn(GameObject caster)
    {
        ReqSearchConsignment(m_input_SearchInput.value);
    }

    void onClick_ParentContent_Btn(GameObject caster)
    {
    }

    void onClick_ItemLevel_Btn(GameObject caster)
    {
        //ConsignItemSortType = (ConsignItemSortType == ItemSortType.ItemSort_LvDown) ? ItemSortType.ItemSort_LvUp : ItemSortType.ItemSort_LvDown;
        if (SaleItemDataManager.IsSearching)
        {
            //ReqSearchPageConsignment();
            ReqSearchConsignment(m_input_SearchInput.value);
        }
        else
        {
            ReqConsignmentItemList();
        }
    }

    void onClick_ItemUnitPrice_Btn(GameObject caster)
    {
        //ConsignItemSortType = (ConsignItemSortType == ItemSortType.ItemSort_MoneyDown) ? ItemSortType.ItemSort_MoneyUp : ItemSortType.ItemSort_MoneyDown;
        if (SaleItemDataManager.IsSearching)
        {
            //ReqSearchPageConsignment();
            ReqSearchConsignment(m_input_SearchInput.value);
        }
        else
        {
            ReqConsignmentItemList();
        }
    }

    void onClick_Btn_PreviousPage_Btn(GameObject caster)
    {
        if (SaleItemDataManager.IsSearching)
        {
            ReqSearchPageConsignment(PageState.PreviousPage);
        }
        else
        {
            ReqConsignmentItemList(PageState.PreviousPage);
        }
    }

    void onClick_Btn_NextPage_Btn(GameObject caster)
    {
        if (SaleItemDataManager.IsSearching)
        {
            ReqSearchPageConsignment(PageState.NextPage);
        }
        else
        {
            ReqConsignmentItemList(PageState.NextPage);
        }
    }

    void onClick_Btn_TakeOut_Btn(GameObject caster)
    {
        SaleItemDataManager.ReqGetSellMoneyConsignment();
    }

    void onClick_Btn_SalesRecords_Btn(GameObject caster)
    {
        if (DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.SalesRecordsPanel))
        {
            DataManager.Manager<UIPanelManager>().HidePanel(PanelID.SalesRecordsPanel);
        }
        else
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.SalesRecordsPanel);
        }
    }

    int unitSellNum = 0;
    int UnitSellNum
    {
        get
        {
            return unitSellNum;
        }
        set
        {
            unitSellNum = value;
            m_label_UnitSellNum.text = value.ToString();
            int price = int.Parse(m_label_UnitSellPrice.text);
            m_label_TotalPriceNum.text = (price * value).ToString();
            SetFactorageNum();     
        }
    }

    void onClick_Btn_Less_Btn(GameObject caster)
    {
        if (SaleItemDataManager.WantSellItem != null)
        {
            int tempNum = int.Parse(m_label_UnitSellNum.text);
            if (tempNum > 1)
            {
                tempNum -= 1;
            }
            else
            {
                tempNum = 1;
            }
            UnitSellNum = tempNum;
            m_sellItemGrid.ChangeItemNum(UnitSellNum);
        }
    }

    void onClick_Btn_Add_Btn(GameObject caster)
    {
        if (SaleItemDataManager.WantSellItem != null)
        {
            uint maxNum = 1;
            if (SaleItemDataManager.WantSellItem is BaseItem)
            {
                BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
                maxNum = itemData.Num;
            }
            int tempNum = int.Parse(m_label_UnitSellNum.text);
            if (tempNum < maxNum)
            {
                tempNum += 1;
            }
            else
            {
                tempNum = (int)maxNum;
            }
            UnitSellNum = tempNum;
            m_sellItemGrid.ChangeItemNum(UnitSellNum);
        }
    }

    void onClick_Btn_Max_Btn(GameObject caster)
    {
        if (SaleItemDataManager.WantSellItem != null)
        {
            uint maxNum = 1;
            if (SaleItemDataManager.WantSellItem is BaseItem)
            {
                BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
                maxNum = itemData.Num;
            }
            UnitSellNum = (int)maxNum;
            m_sellItemGrid.ChangeItemNum(UnitSellNum);
        }
    }

    void onClick_Btn_Sell_Btn(GameObject caster)
    {
        if (SaleItemDataManager.WantSellItem != null)
        {
            if (SaleItemDataManager.WantSellItem is BaseItem)
            {
                BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
                if (itemData != null)
                {
                    stSellItemConsignmentUserCmd_C cmd = new stSellItemConsignmentUserCmd_C();
                    cmd.item_id = itemData.QWThisID;
                    cmd.money = uint.Parse(m_label_UnitSellPrice.text);
                    if (cmd.money <= 0)
                    {
                        return;
                    }
                    cmd.num = uint.Parse(m_label_UnitSellNum.text);  
                    NetService.Instance.Send(cmd);
                }
            }
            else if (SaleItemDataManager.WantSellItem is IPet)
            {
                IPet petData = SaleItemDataManager.WantSellItem as IPet;
                if (petData != null && petData != null)
                {
                    stSellPetConsignmentUserCmd_C cmd = new stSellPetConsignmentUserCmd_C();
                    cmd.pet_id = petData.GetID();
                    cmd.money = uint.Parse(m_label_UnitSellPrice.text);
                    if (cmd.money <= 0)
                    {
                        return;
                    }
                    NetService.Instance.Send(cmd);
                }
            }
        }
        m_ctor_SellItemPriceList.SetVisible(false);
    }

    void onClick_BuyToggle_Btn(GameObject caster)
    {
        if (IsConsignmentItemMode(ConsignmentItemMode.Buy))
        {
            return;
        }
        SetConsignmentItemMode(ConsignmentItemMode.Buy);
    }

    void onClick_SellToggle_Btn(GameObject caster)
    {
        if (IsConsignmentItemMode(ConsignmentItemMode.Sell))
        {
            return;
        }
        SetConsignmentItemMode(ConsignmentItemMode.Sell);
    }

    void onClick_ItemAccountToggle_Btn(GameObject caster)
    {
        if (IsConsignmentItemMode(ConsignmentItemMode.Account))
        {
            return;
        }
        SetConsignmentItemMode(ConsignmentItemMode.Account);
    }

    void onClick_Btn_Change_Btn(GameObject caster)
    {
        if (m_em_SellState == SellItemState.SellItem)
        {
            SetSellItemState(SellItemState.SellList);
        }
        else
        {
            SetSellItemState(SellItemState.SellItem);
        }
    }
    #endregion

    #region Op
    public void SetDefaultSellItemState()
    {
        SetSellItemState(SellItemState.SellItem);
    }
    /// <summary>
    /// 是否为当前物品寄售类型
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private bool IsConsignmentItemMode(ConsignmentItemMode mode)
    {
        return (m_em_itemMode == mode);
    }

    private void InitConsignItemOnShow()
    {
        SetConsignmentItemMode(m_em_itemMode);
    }

    /// <summary>
    /// 设置买卖界面
    /// </summary>
    /// <param name="mode"></param>
    private void SetConsignmentItemMode(ConsignmentItemMode mode)
    {
        m_em_itemMode = mode;
        curShowWhatItem = ShowWhatItem.ShowNormalItem;
        m_saleUIToggle = mode;
        switch (mode)
        {
            case ConsignmentItemMode.Sell:
                {
                    m_trans_SellContent.gameObject.SetActive(true);
                    m_trans_BuyContent.gameObject.SetActive(false);
                    SetSellItemState(SellItemState.SellItem);
                }
                break;
            case ConsignmentItemMode.Account:
                {
                    m_trans_SellContent.gameObject.SetActive(true);
                    m_trans_BuyContent.gameObject.SetActive(false);
                    SetSellItemState(SellItemState.SellList);
                }
                break;
            case ConsignmentItemMode.ShowItem:
                {
                    curShowWhatItem = ShowWhatItem.ShowGreatItem;
                    m_trans_SellContent.gameObject.SetActive(false);
                    m_trans_BuyContent.gameObject.SetActive(true);
                    
                    SetSelectFirstType(0);
                   // ReqConsignmentItemList();
                }
                break;
            default:
                {
                    m_trans_SellContent.gameObject.SetActive(false);
                    m_trans_BuyContent.gameObject.SetActive(true);
                    SetSelectFirstType(0);
                    //ReqConsignmentItemList();
                }
                break;
        }
    }

    /// <summary>
    /// 设置买卖界面
    /// </summary>
    /// <param name="mode"></param>
    private void SetSellItemState(SellItemState mode)
    {
        m_em_SellState = mode;
        if (mode == SellItemState.SellItem)
        {
            m_label_Label_Change.text = "我的寄售";
            m_trans_SellingContent.gameObject.SetActive(true);
            m_trans_SellListContent.gameObject.SetActive(false);
            SetFilterType(KnapsackItemType.ItemAll);
        }
        else
        {
            m_label_Label_Change.text = "出售物品";
            m_trans_SellingContent.gameObject.SetActive(false);
            m_trans_SellListContent.gameObject.SetActive(true);
            ShowSellingItems();
        }
    }

    /// <summary>
    /// 设置选中第一分页
    /// </summary>
    /// <param name="type"></param>
    private void SetSelectFirstType(uint type, bool force = false)
    {      
        if (null == mSecondTabCreator)
        {
            return;
        }
        if (m_uint_activeFType == type && !force && type != 0)
        {
            mSecondTabCreator.DoToggle(mlstFirstTabIds.IndexOf(m_uint_activeFType), true, true);
            return;
        }
        m_uint_activeFType = type;
        if (type == 0)
        {
            //关注页签 不需要走后面的逻辑
         
            selectSecondsKey = 0;
            m_uint_activeStype = 0;
            mSecondTabCreator.DoToggle(mlstFirstTabIds.IndexOf(m_uint_activeFType), true, true);
            if (m_saleUIToggle == ConsignmentItemMode.Buy)
            {
                NetService.Instance.Send(new stGetStarItemConsignmentUserCmd_C());
                return;
            }
            else if (m_saleUIToggle == ConsignmentItemMode.ShowItem)
            {
                NetService.Instance.Send(new stGetGreatConsignmentUserCmd_C());
                return;
            }
        }
        else 
        {
            selectSecondsKey = m_uintDic[m_uint_activeFType][0]; 
        }
        mSecondTabCreator.Open(mlstFirstTabIds.IndexOf(m_uint_activeFType), true);
        
        filter_job = 0;
        filter_grade = 0;
        m_int_beginIndex = 0;
        SetSelectSecondType(selectSecondsKey, m_uint_activeStype == 0);
    }


    uint filter_job = 0;
    uint filter_grade = 0;
    /// <summary>
    /// 设置选中二级分页
    /// </summary>
    /// <param name="type"></param>
    /// <param name="force"></param>
    private void SetSelectSecondType(uint type, bool force = false)
    {
        if (null == mSecondTabCreator)
        {
            return;
        }
        if (m_uint_activeStype == type && !force)
            return;
        UISecondTypeGrid sGrid = null;
        if (m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), m_uintDic[m_uint_activeFType].IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(false);
            }
        }
        m_btn_JobPopupList.gameObject.SetActive(false);
        m_btn_GradePopupList.gameObject.SetActive(false);
        m_uint_activeStype = type;
        if (m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), m_uintDic[m_uint_activeFType].IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(true);
            }
        }
        ConsignmentCateIDConf cateIDConf = SaleItemDataManager.GetSubConsignmentCateIDConf(m_uint_activeFType, m_uint_activeStype);
            if (cateIDConf != null)
            {
                needSendMsg = false;
                ConsignmentFilterConf filterConf = null;
                if (cateIDConf.FilterID1 > 0)
                {
                    filterConf = SaleItemDataManager.GetConsignmentFilterInfo(cateIDConf.FilterID1);
                    for (int m = 0; m < m_sprite_JobContent.transform.childCount;m++ )
                    {
                        Transform trans = m_sprite_JobContent.transform.GetChild(m);
                        trans.gameObject.SetActive(false);
                    }
                    if (filterConf != null)
                    {
                        string[] temp = filterConf.Filters.Split('|');
                        for (uint i = 0; i < temp.Length; i++)
                        {
                            UILabel label = m_sprite_JobContent.transform.GetChild((int)i).GetComponent<UILabel>();
                            label.text = temp[i];
                            label.gameObject.name = temp[i]+"_"+i;
                            label.gameObject.SetActive(true);
                            UIEventListener.Get(label.gameObject).onClick = OnClickFilter1;
                        }
                        m_btn_JobPopupList.gameObject.SetActive(true);
                        m_label_JobLabel.text = temp[0];
                        filter_job = 0;
                    }
                }
                if (cateIDConf.FilterID2 > 0)
                {
                    filterConf = SaleItemDataManager.GetConsignmentFilterInfo(cateIDConf.FilterID2);
                    for (int m = 0; m < m_sprite_GradeContent.transform.childCount; m++)
                    {
                        Transform trans = m_sprite_GradeContent.transform.GetChild(m);
                        trans.gameObject.SetActive(false);
                    }
                    if (filterConf != null)
                    {
                        string[] temp = filterConf.Filters.Split('|');
                        for (uint i = 0; i < temp.Length; i++)
                        {
                            UILabel label = m_sprite_GradeContent.transform.GetChild((int)i).GetComponent<UILabel>();
                            label.text = temp[i];
                            label.gameObject.name = temp[i] + "_" + i;
                            label.gameObject.SetActive(true);
                            UIEventListener.Get(label.gameObject).onClick = OnClickFilter2;
                        }
                        m_btn_GradePopupList.gameObject.SetActive(true);
                        m_label_GradeLabel.text = temp[0];
                        filter_grade = 0;
                    }
                }
                needSendMsg = true;
            }
            ReqConsignmentItemList();
        
    }
    void OnClickFilter1(GameObject go) 
    {
        string[] param = go.name.Split('_');        
        if (uint.TryParse(param[1], out filter_job))
        {
            m_label_JobLabel.text = param[0];
        }
        ReqConsignmentItemList();
        EnableFilterUI(FilterType.job,false);

    }
    void OnClickFilter2(GameObject go)
    {
        string[] param = go.name.Split('_');
        if (uint.TryParse(param[1], out filter_grade))
        {
            m_label_GradeLabel.text = param[0];
        }
        ReqConsignmentItemList();
        EnableFilterUI(FilterType.grade, false);
    }
    void UpdateListUI(ulong market_id,uint num)
    {
        int count = m_lst_ConsignmentItems.Count <= 8 ? m_lst_ConsignmentItems.Count : 8;
        bool needRemoveGrid = false;
        for (int i = 0; i < m_lst_ConsignmentItems.Count; i++)
        {
            if (m_lst_ConsignmentItems[i].Market_ID == market_id)
             {
                 if (num <= 0)
                 {
                     m_lst_ConsignmentItems.RemoveAt(i);
                     needRemoveGrid = true;
                 }
                 else
                 {
                     m_lst_ConsignmentItems[i].page_info.item_num = num;
                 }
             }
        }

        if (m_lst_ConsignmentItems.Count == 0)
        {
            ReqConsignmentItemList(PageState.PreviousPage);
            return;
        }
        if (needRemoveGrid)
        {
            m_ctor_ItemListGrid.RemoveData(count - 1);
        }     
        m_ctor_ItemListGrid.UpdateActiveGridData();
    }
    int m_int_beginIndex = 0;
    int m_int_endIndex = 0;
    /// <summary>
    /// 请求获取寄售列表
    /// </summary>
    private void ReqConsignmentItemList(PageState pageState = PageState.Max)
    {
            int begin_index = 0;
           
            if (m_uint_activeFType != 0)
            {
                    if (pageState == PageState.PreviousPage || pageState == PageState.NextPage)
                    {
                        if (SaleItemDataManager.ReqItemListState != ReqConsignListState.None)
                        {
                            return;
                        }
                    }
                    SaleItemDataManager.ReqItemListState = ReqConsignListState.ResetPage;
                 
                    uint pageDir = 0;
                    switch (pageState)
                    {
                        case PageState.PreviousPage:
                            begin_index = SaleItemDataManager.BeginIndex;
                            pageDir = 1;
                            SaleItemDataManager.ReqItemListState = ReqConsignListState.PreviousPage;
                            break;
                        case PageState.NextPage:
                            begin_index = (int)SaleItemDataManager.EndIndex;
                            SaleItemDataManager.ReqItemListState = ReqConsignListState.NextPage;
                            break;
                        default:
                            if (ConsignItemSortType == ItemSortType.ItemSort_LvUp || ConsignItemSortType == ItemSortType.ItemSort_MoneyUp)
                            {
                                begin_index = -1;
                            }
                            else
                            {
                                begin_index = 0;
                            }
                            break;
                    }
                    uint filter_0 = filter_job;
                    uint filter_1 = filter_grade;
                    SaleItemDataManager.ReqConsignmentItemList(m_uint_activeStype, filter_0, filter_1, ConsignItemSortType, begin_index, pageDir, curShowWhatItem);
                    }
            else 
            {
                List<ConsignmentItem> temp_itemlists = new List<ConsignmentItem>();
                //针对所有公示和我的关注无法请求分页信息 所以客户端自己做数据分页
                MyConsignStarInfo infos = SaleItemDataManager.GetItemStarInfo();

                int allPage = infos.consignmentItemList.Count % 8 == 0 ? infos.consignmentItemList.Count / 8 : infos.consignmentItemList.Count / 8 + 1;
                switch (pageState)
                {
                    case PageState.PreviousPage:
                        if (m_int_beginIndex  >= 8)
                        {
                            m_int_beginIndex -= 8;
                        }
                        else 
                        {
                            m_int_beginIndex = 0;
                        }
                       
                        break;
                    case PageState.NextPage:
                        if (m_int_beginIndex + 8 < infos.consignmentItemList.Count)
                        {
                            m_int_beginIndex += 8;                          
                        }
                       
                        break;
                }
                begin_index = m_int_beginIndex;
                for (int i = begin_index; i < infos.consignmentItemList.Count && i < begin_index + 8; i++)
                {
                    temp_itemlists.Add(infos.consignmentItemList[i]);              
                }
                m_lst_ConsignmentItems = temp_itemlists;
                int count = m_lst_ConsignmentItems.Count <= 8 ? m_lst_ConsignmentItems.Count : 8;
                if (m_ctor_ItemListGrid != null)
                {
                    m_ctor_ItemListGrid.CreateGrids(count);
                }     
                //更新页数
                int page = begin_index / 8 + 1;
               
                m_label_PageNum.text = string.Format("{0}/{1}", page, allPage);

            }        
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="key_words"></param>
    private void ReqSearchConsignment(string key_words)
    {
        SaleItemDataManager.ReqSearchConsignment(key_words, ConsignItemSortType);
    }

    /// <summary>
    /// 搜索列表翻页
    /// </summary>
    private void ReqSearchPageConsignment(PageState pageState = PageState.Max)
    {
        //上一条消息还没返回,不做处理
        if (pageState == PageState.PreviousPage || pageState == PageState.NextPage)
        {
            if (SaleItemDataManager.ReqItemListState != ReqConsignListState.None)
            {
                return;
            }
        }
        SaleItemDataManager.ReqItemListState = ReqConsignListState.ResetPage;
        int begin_index = 0;
        uint begin_base_id = 0;
        uint pageDir = 0;
        switch (pageState)
        {
            case PageState.PreviousPage:
                begin_index = SaleItemDataManager.BeginIndex;
                pageDir = 1;
                SaleItemDataManager.ReqItemListState = ReqConsignListState.PreviousPage;
                break;
            case PageState.NextPage:
                begin_index = (int)SaleItemDataManager.EndIndex;
                SaleItemDataManager.ReqItemListState = ReqConsignListState.NextPage;
                break;
            default:
                break;
        }
        SaleItemDataManager.ReqSearchPageConsignment(begin_index, ConsignItemSortType, pageDir);
    }

    private bool isShowSellList = false;
    /// <summary>
    /// 选中要卖的物品
    /// </summary>
    private void SetSelectSellItem(object iGrid)
    {
        if (iGrid == null)
        {
            if (null != m_sellItemGrid)
            {
                m_sellItemGrid.ResetInfoGrid(false);
                m_sellItemGrid.SetTreasureVisible(false);
            }
            SaleItemDataManager.WantSellItem = null;
            m_label_UnitSellPrice.text = "0";
            UnitSellNum = 0;
            m_label_GuidancePrices.text = "(0-0)";
            m_label_SellingItemHint.enabled = true;
            m_label_SellingItemName.text = "";
            m_label_SellingItemLevel.text = "";
            return;
        }
        if (iGrid is UIItemGrid)
        {
            m_sellItemGrid.ResetInfoGrid(true);
            UIItemGrid itemGrid = iGrid as UIItemGrid;
            BaseItem itemData = itemGrid.Data;
            if (itemData == null)
            {
                return;
            }        
            SetCurItem(itemData);
        }
        else if (iGrid is UIConsignPetListGrid)
        {
            UIConsignPetListGrid petGrid = iGrid as UIConsignPetListGrid;
            IPet petData = petGrid.petData;
            if (petData == null)
            {
                return;
            }
            table.PetDataBase petdb = PetManager.GetPetDataBase(petData.PetBaseID);
            //if (null != petGrid.Icon)
            //{
            //    m_sprite_SellingIcon.atlas = petGrid.Icon.atlas;
            //}
            //m_sprite_SellingIcon.spriteName = petdb.icon;
            //m_sprite_SellingIcon.MakePixelPerfect();
            m_label_SellingItemName.text = petdb.petName;
            m_label_SellingItemLevel.text = "";
            m_label_SellingItemHint.enabled = false;
            SaleItemDataManager.WantSellItem = petData;
            m_label_GuidancePrices.text = "";
            m_label_UnitSellPrice.text = "0";
            OnItemNumConfirm(1);
        }
    }

    private bool m_bInitSellItemPriceCreator = false;
    private bool m_bInitItemGridCreator = false;
    /// <summary>
    /// 显示背包中能寄售的物品
    /// </summary>
    private void ShowCanSellItems()
    {
        //清空选中的寄售物品
        SetSelectSellItem(null);
        if (m_em_sellItemType == SellItemType.Item)
        {
            if (null != m_ctor_ItemGridScrollView)
            {
                m_ctor_ItemGridScrollView.SetVisible(true);
                if (!m_bInitItemGridCreator)
                {
                    m_bInitItemGridCreator = true;
                    m_ctor_ItemGridScrollView.RefreshCheck();
                    m_ctor_ItemGridScrollView.Initialize<UIItemGrid>(m_trans_UIItemGrid.gameObject,  OnUpdateGridData, OnGridEventDlg);
                }

                canConsignItemList.Clear();

                List<uint> bagItemList = DataManager.Manager<ItemManager>().DoFilterItemData(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN, m_int_itemFilterMask);
                for (int i = 0; i < bagItemList.Count; i++)
                {
                    BaseItem baseItem = DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(bagItemList[i]);
                    if (SaleItemDataManager.IsConsignableItem(baseItem))
                    {
                        canConsignItemList.Add(baseItem.QWThisID);
                    }
                }
                if (null != canConsignItemList && canConsignItemList.Count > 0)
                {
                    ItemManager.SortItemListBySortId(ref canConsignItemList);
                }
                int mainPackMaxGridHave = DataManager.Manager<KnapsackManager>().GetMaxGridHaveByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
                m_ctor_ItemGridScrollView.CreateGrids(mainPackMaxGridHave);
            }
            m_ctor_PetGridScrollView.SetVisible(false);
        }
        else if (m_em_sellItemType == SellItemType.Pet)
        {
            m_ctor_ItemGridScrollView.SetVisible(false);
            m_ctor_PetGridScrollView.SetVisible(true);
            if (null != m_ctor_PetGridScrollView)
            {
                m_ctor_PetGridScrollView.gridContentOffset = new UnityEngine.Vector2(-176f, 0);
                m_ctor_PetGridScrollView.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
                m_ctor_PetGridScrollView.gridWidth = 86;
                m_ctor_PetGridScrollView.gridHeight = 86;
                m_ctor_PetGridScrollView.rowcolumLimit = 5;
                m_ctor_PetGridScrollView.RefreshCheck();
                m_ctor_PetGridScrollView.Initialize<UIConsignPetListGrid>(m_trans_UIConsignPetListGrid.gameObject, OnUpdateGridData, OnGridEventDlg);

                if (canConsignPetList != null)
                {
                    canConsignPetList.Clear();
                    canConsignPetList = null;
                }
                canConsignPetList = new List<IPet>(PetManager.GetPetDic().Values);
                for (int i = 0; i < canConsignPetList.Count; i++)
                {
                    IPet petData = canConsignPetList[i];
                    if (petData != null && petData.GetID() == PetManager.CurFightingPet)
                    {
                        canConsignPetList.Remove(petData);
                        break;
                    }
                }
                m_ctor_PetGridScrollView.CreateGrids(canConsignPetList.Count);
            }
        }
        if (null != m_ctor_SellItemPriceList)
        {
            if (!m_bInitSellItemPriceCreator)
            {
                m_bInitSellItemPriceCreator = true;
                m_ctor_SellItemPriceList.Initialize<UISellItemPriceGrid>(m_sprite_UISellItemPriceGrid.gameObject, OnUpdateGridData, OnGridEventDlg);
            }
        }
    }

    private bool m_bInitSellListGridCreator = false;
    /// <summary>
    /// 显示自己的寄售列表
    /// </summary>
    private void ShowSellingItems()
    {
        if (null != m_ctor_SellListScrollViewContent)
        {
            if (!m_bInitSellListGridCreator)
            {
                m_bInitSellListGridCreator = true;
                m_ctor_SellListScrollViewContent.Initialize<UIConsignmentSellListGrid>(m_trans_UIConsignmentSellListGrid.gameObject,OnUpdateGridData, OnGridEventDlg);
            }
            MyConsignItemInfo itemSellInfo = SaleItemDataManager.GetItemSellInfo();
            if (itemSellInfo != null && itemSellInfo.consignmentItemList != null)
            {
                m_ctor_SellListScrollViewContent.CreateGrids(itemSellInfo.consignmentItemList.Count);
            }
            uint maxNum = GameTableManager.Instance.GetGlobalConfig<uint>("MaxSellItemNum");
            string text = string.Format("{0}/{1}", itemSellInfo.consignmentItemList.Count, maxNum);
            m_label_SaleTitleName.text = string.Format("出售列表({0})", text);
        }


        m_label_MyConsignJinBi.text = SaleItemDataManager.ConsignTotalJinBi.ToString();
        m_label_MyConsignYuanBao.text = SaleItemDataManager.ConsignTotalYuanBao.ToString();
    }

    /// <summary>
    /// 更新寄售列表
    /// </summary>
    void UpdateSaleItemList()
    {
        if (m_uint_activeFType != 0 || SaleItemDataManager.IsSearching || m_saleUIToggle == ConsignmentItemMode.ShowItem)
        {
            m_lst_ConsignmentItems = SaleItemDataManager.GetPageItemLists();
        }
        else
        {
            MyConsignStarInfo datas = SaleItemDataManager.GetItemStarInfo();
            m_lst_ConsignmentItems = datas.consignmentItemList;
        }
        //itemlists.Sort(SortPageItems);
        int count = m_lst_ConsignmentItems.Count <= 8 ? m_lst_ConsignmentItems.Count : 8;
        m_ctor_ItemListGrid.CreateGrids(count);
        //更新页数
        uint page = SaleItemDataManager.CurPage > 0 ? SaleItemDataManager.CurPage : 1;
        uint allpage = SaleItemDataManager.AllPage > 0 ? SaleItemDataManager.AllPage : 1;
        m_label_PageNum.text = string.Format("{0}/{1}", page, allpage);
    }

    void UpdateAllStarItemsList() 
    {
        MyConsignStarInfo datas = SaleItemDataManager.GetItemStarInfo();
        m_lst_ConsignmentItems = datas.consignmentItemList;
        int count = m_lst_ConsignmentItems.Count <= 8 ? m_lst_ConsignmentItems.Count : 8;
        m_ctor_ItemListGrid.CreateGrids(count);

        m_int_beginIndex = 0;
        //更新页数
        uint page = SaleItemDataManager.CurPage > 0 ? SaleItemDataManager.CurPage : 1;
        uint allpage = SaleItemDataManager.AllPage > 0 ? SaleItemDataManager.AllPage : 1;
        m_label_PageNum.text = string.Format("{0}/{1}", page, allpage);
    }
    void UpdateSingleStarItemsList(ulong marked_id,bool star) 
    {
        MyConsignStarInfo datas = SaleItemDataManager.GetItemStarInfo();
        m_lst_ConsignmentItems = datas.consignmentItemList;
        List<UIConsignmentItemListGrid> saleItemLists = m_ctor_ItemListGrid.GetGrids<UIConsignmentItemListGrid>();
        for (int i = 0; i < saleItemLists.Count;i++ )
        {
            if (saleItemLists[i].marked_id == marked_id)
            {
                saleItemLists[i].SetStarValue(star);
            }
        }
    }
    int SortPageItems(ItemPageInfo a, ItemPageInfo b)
    {
        ItemDataBase baseDataA = GameTableManager.Instance.GetTableItem<ItemDataBase>(a.item_base_id);
        ItemDataBase baseDataB = GameTableManager.Instance.GetTableItem<ItemDataBase>(b.item_base_id);
        if (baseDataA == null || baseDataB == null)
        {
            return 0;
        }
        switch (ConsignItemSortType)
        {
            case ItemSortType.ItemSort_LvDown:
                {
                    if (baseDataB.useLevel == baseDataA.useLevel)
                    {
                        if (b.money == a.money)
                        {
                            if (b.item_base_id == a.item_base_id)
                            {
                                return (int)a.item_num - (int)b.item_num;
                            }
                            else
                            {
                                return (int)a.item_base_id - (int)b.item_base_id;
                            }
                        }
                        else
                        {
                            return (int)a.money - (int)b.money;
                        }
                    }
                    else
                    {
                        return (int)baseDataB.useLevel - (int)baseDataA.useLevel;
                    }
                }
                break;
            case ItemSortType.ItemSort_LvUp:
                {
                    if (baseDataB.useLevel == baseDataA.useLevel)
                    {
                        if (b.money == a.money)
                        {
                            if (b.item_base_id == a.item_base_id)
                            {
                                return (int)a.item_num - (int)b.item_num;
                            }
                            else
                            {
                                return (int)a.item_base_id - (int)b.item_base_id;
                            }
                        }
                        else
                        {
                            return (int)a.money - (int)b.money;
                        }
                    }
                    else
                    {
                        return (int)baseDataA.useLevel - (int)baseDataB.useLevel;
                    }
                }
                break;
            case ItemSortType.ItemSort_MoneyDown:
                {
                    if (b.money == a.money)
                    {
                        if (baseDataB.useLevel == baseDataA.useLevel)
                        {
                            if (b.item_base_id == a.item_base_id)
                            {
                                return (int)a.item_num - (int)b.item_num;
                            }
                            else
                            {
                                return (int)a.item_base_id - (int)b.item_base_id;
                            }
                        }
                        else
                        {
                            return (int)baseDataB.useLevel - (int)baseDataA.useLevel;
                        }
                    }
                    else
                    {
                        return (int)b.money - (int)a.money;
                    }
                }
                break;
            case ItemSortType.ItemSort_MoneyUp:
                {
                    if (a.money == b.money)
                    {
                        if (baseDataA.useLevel == baseDataB.useLevel)
                        {
                            if (b.item_base_id == a.item_base_id)
                            {
                                return (int)a.item_num - (int)b.item_num;
                            }
                            else
                            {
                                return (int)a.item_base_id - (int)b.item_base_id;
                            }
                        }
                        else
                        {
                            return (int)baseDataB.useLevel - (int)baseDataA.useLevel;
                        }
                    }
                    else
                    {
                        return (int)a.money - (int)b.money;
                    }
                }
                break;
            default:
                break;
        }
        return 0;
    }

    #endregion

    #region UIEvent
    private void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemGrid)
        {
            UIItemGrid itemGrid = grid as UIItemGrid;
            BaseItem itemData = (canConsignItemList.Count > index) ? DataManager.Manager<ItemManager>().GetBaseItemByQwThisId(canConsignItemList[index]) : null;
            if (itemGrid != null)
            {
                itemGrid.SetGridData(UIItemInfoGridBase.InfoGridType.Consignment,itemData);
            }
        }
        else if (grid is UIConsignPetListGrid)
        {
            UIConsignPetListGrid petGrid = grid as UIConsignPetListGrid;
            IPet petData = (canConsignPetList.Count > index) ? canConsignPetList[index] : null;
            if (petGrid != null)
            {
                petGrid.SetGridData(petData);
            }
        }
        else if (grid is UIConsignmentSellListGrid)
        {
            UIConsignmentSellListGrid sellListGrid = grid as UIConsignmentSellListGrid;
            if (sellListGrid != null)
            {
                MyConsignItemInfo itemSellInfo = SaleItemDataManager.GetItemSellInfo();
                if (itemSellInfo != null)
                {
                    if(index < itemSellInfo.consignmentItemList.Count )
                    {
                        ConsignmentItem item = itemSellInfo.consignmentItemList[index];
                        sellListGrid.UpdateItemInfo(item.page_info, item.sell_timeInfo, item.server_data);
                    }
                }
            }
        }
        else if(grid is UISellItemPriceGrid)
        {
            UISellItemPriceGrid sGrid = grid as UISellItemPriceGrid;
            sGrid.SetGridData(sellList[index]);
            //sGrid.SetGridIcon(baseID,sellList[index].num);
        }
        else if (grid is UICtrTypeGrid)
        {
                UICtrTypeGrid data = grid as UICtrTypeGrid;
                data.SetRedPointStatus(false);
                ConsignmentCateIDConf tab = list[index];
                data.SetData(tab.ID, tab.Name,dic[tab.ID].Count);           
        }
        else if(grid is UIConsignmentItemListGrid)
        {
            UIConsignmentItemListGrid data = grid as UIConsignmentItemListGrid;
            if (index < m_lst_ConsignmentItems.Count)
            {
                data.UpdateItemInfo(m_lst_ConsignmentItems[index].page_info, m_lst_ConsignmentItems[index].server_data);
                if (index == 0)
                {
                    OnSelectItemListGrid(data);
                }
            }                   
        }
    }


    private void OnUpdateSecondTabGrid(UIGridBase grid, object id, int index)
    {
        if (grid is UISecondTypeGrid)
        {
            UISecondTypeGrid sGrid = grid as UISecondTypeGrid;
            sGrid.SetRedPoint(false);
            List<ConsignmentCateIDConf> li = dic[(uint)id];
            sGrid.SetData(li[index].SubID,li[index].SubName,m_uint_activeStype == dic[(uint)id][index].SubID);
        }
    }


    /// <summary>
    /// 格子事件委托
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UICtrTypeGrid)
                    {
                        UICtrTypeGrid tGrid = data as UICtrTypeGrid;
                        SetSelectFirstType((uint)tGrid.ID);
                    }
                    else if (data is UISecondTypeGrid)
                    {
                        UISecondTypeGrid sGrid = data as UISecondTypeGrid;
                        SetSelectSecondType(sGrid.Data);
                    }
                    else if (data is UIItemGrid)
                    {
                        UIItemGrid iGrid = data as UIItemGrid;
                        SetSelectSellItem(iGrid);
                    }
                    else if (data is UIConsignPetListGrid)
                    {
                        UIConsignPetListGrid cGrid = data as UIConsignPetListGrid;
                        SetSelectSellItem(cGrid);
                    }
                    else if (data is UIConsignmentItemListGrid)
                    {
                        UIConsignmentItemListGrid iGrid = data as UIConsignmentItemListGrid;
                        OnSelectItemListGrid(iGrid);
                    }
                }
                break;
            case UIEventType.Press:
                {
                    if (data is UIItemGrid)
                    {
                        bool isdown = tempShowItem == null;
                        if (isdown)
                        {
                            UIItemGrid itemGrid = data as UIItemGrid;
                            tempShowItem = itemGrid.Data;
                            Invoke("ShowItemTips", 0.5f);
                        }
                        else
                        {
                            CancelInvoke("ShowItemTips");
                            tempShowItem = null;
                        }
                    }
                }
               break;
        }
    }

    BaseItem tempShowItem = null;
    void ShowItemTips()
    {
        if (tempShowItem != null)
        {
            TipsManager.Instance.ShowItemTips(tempShowItem);
        }
    }

    void SaleItemDataManager_ValueUpdateEvent(object sender, ValueUpdateEventArgs e)
    {
        if (e != null)
        {
            if (e.key == SaleItemDispatchEvents.RefreshSaleItemInfo.ToString())
            {
                UpdateSaleItemList();
            }
            else if (e.key == SaleItemDispatchEvents.RefreshSellItemInfo.ToString())
            {
                ShowCanSellItems();
            }
            else if (e.key == SaleItemDispatchEvents.BuyConsignItemInfo.ToString())
            {
                DataManager.Manager<UIPanelManager>().HidePanel(PanelID.BuyPanel);
                if (SaleItemDataManager.IsSearching)
                {
                    ReqSearchConsignment(m_input_SearchInput.value);
                }
                else
                {
                    uint num = 0;
                    if(e.oldValue != null)
                    {
                        num = (uint)e.oldValue;
                    }
                    if (e.newValue != null)
                    {
                        ulong market_id = (ulong)e.newValue;
                        UpdateListUI(market_id, num);
                    }
                   
                }
            }
            else if (e.key == SaleItemDispatchEvents.RefreshMyConsignList.ToString())
            {
                ShowSellingItems();
            }
            else if (e.key == SaleItemDispatchEvents.ReFreshMyConsignMoney.ToString())
            {
                m_label_MyConsignJinBi.text = SaleItemDataManager.ConsignTotalJinBi.ToString();
                m_label_MyConsignYuanBao.text = SaleItemDataManager.ConsignTotalYuanBao.ToString();
            }
            else if (e.key == SaleItemDispatchEvents.GetAllStarItems.ToString())
            {
                UpdateAllStarItemsList();
            }
            else if (e.key == SaleItemDispatchEvents.RefreshSingleStarState.ToString())
            {
                ulong marked_id = (ulong)e.oldValue;
                bool star =(bool)e.newValue;
                UpdateSingleStarItemsList(marked_id,star);
            }
            else if(e.key == SaleItemDispatchEvents.RefreshItemPrePrice.ToString())
            {
                uint baseid = (uint)e.oldValue;
                prePrice = (int)e.newValue;
                SetRecommondPrice(baseid);
              
            }
        }
    }

    #endregion

    protected override void OnDisable()
    {
        base.OnDisable();
        SaleItemDataManager.ClearItemInfo();
    }

 
    void OnShowItemPriceListArea(ItemPriceParam info) 
    {
        if (info == null || info.list.Count == 0)
       {
           m_ctor_SellItemPriceList.SetVisible(false);
           return;
       }
        m_ctor_SellItemPriceList.SetVisible(true);
        baseID = info.baseid;
        sellList = info.list;
        if (sellList.Count >0)
        {
            m_ctor_SellItemPriceList.CreateGrids(sellList.Count);
        }
        m_label_UnitSellPrice.text = sellList[0].price.ToString();
    }
    void onClick_ListCloseBtn_Btn(GameObject caster)
    {
        m_ctor_SellItemPriceList.SetVisible(false);
    }

    enum FilterType 
    {
         job = 0,
        grade,
    }

    private void EnableFilterUI(FilterType type,bool enable= true)
    {
        if (enable)
        {
            if (type == FilterType.job)
            {
                m_sprite_JobContent.gameObject.SetActive(true);
                m_sprite_GradeContent.gameObject.SetActive(false);
                m_btn_JobFillterCollider.isEnabled = true;
                m_btn_GradeFillterCollider.isEnabled = false;
            }
            else
            {
                m_sprite_JobContent.gameObject.SetActive(false);
                m_sprite_GradeContent.gameObject.SetActive(true);
                m_btn_JobFillterCollider.isEnabled = false;
                m_btn_GradeFillterCollider.isEnabled = true;
            }
        }
        else 
        {
            m_sprite_JobContent.gameObject.SetActive(false);
            m_sprite_GradeContent.gameObject.SetActive(false);
            m_btn_JobFillterCollider.isEnabled = false;
            m_btn_GradeFillterCollider.isEnabled = false;
        }
           
    }

    void onClick_JobFillterCollider_Btn(GameObject caster)
    {
        EnableFilterUI(FilterType.job,false);
    }



    void onClick_GradeFillterCollider_Btn(GameObject caster)
    {
        EnableFilterUI(FilterType.grade, false);
    }

    void onClick_BuyItemBtn_Btn(GameObject caster)
    {
        //购买
        if (TargetBuyGrid != null)
        {
            UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
            if (mgr.IsShowPanel(PanelID.BuyPanel))
            {
                mgr.HidePanel(PanelID.BuyPanel);
            }
            else
            {
  
                  mgr.ShowPanel(PanelID.BuyPanel, data: new BuyPanel.HandInputInitData()
                {
                    item_id = TargetBuyGrid.itemPageInfo.item_base_id,
                    price = (int)TargetBuyGrid.itemPageInfo.money,
                    min = 1,
                    max = (int)TargetBuyGrid.itemPageInfo.item_num,
                    moneyType = TargetBuyGrid.itemPageInfo.great ? (uint)ClientMoneyType.YuanBao :(uint)ClientMoneyType.YinLiang, 
                    onBuyBtnClick = OnBuyPanelCallBack,
                });
              
            }
        }
       
      
    }

    void OnBuyPanelCallBack(int num)
    {
        if (TargetBuyGrid.itemPageInfo != null)
        {
            DataManager.Manager<ConsignmentManager>().ReqBuyItemConsignment(TargetBuyGrid.itemPageInfo.market_id, (uint)num);
        }
    }
    void OnSelectItemListGrid(UIConsignmentItemListGrid grid) 
    {
        if (TargetBuyGrid != null && TargetBuyGrid != grid)
        {
            TargetBuyGrid.SetSelect(false);
        }
        TargetBuyGrid = grid;
        grid.SetSelect(true);
        if (TargetBuyGrid != null)
        {
            m_btn_BuyItemBtn.gameObject.SetActive(TargetBuyGrid.CanBuy);
            m_label_NoBuyTime.gameObject.SetActive(!TargetBuyGrid.CanBuy);
        }
    }

    void onClick_ItemAll_Btn(GameObject caster)
    {
        SetFilterType(KnapsackItemType.ItemAll);
    }

    void onClick_ItemEquipment_Btn(GameObject caster)
    {
        SetFilterType(KnapsackItemType.ItemEquipment);

    }

    void onClick_ItemUseable_Btn(GameObject caster)
    {
        SetFilterType(KnapsackItemType.ItemUseable);

    }

    void onClick_ItemProps_Btn(GameObject caster)
    {
        SetFilterType(KnapsackItemType.ItemProps);

      
    }

    void SetFilterType(KnapsackItemType itemType) 
    {
        switch (itemType)
        {
            case KnapsackItemType.ItemAll:
                m_int_itemFilterMask = (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Equip
                                        | 1 << (int)GameCmd.ItemBaseType.ItemBaseType_Consumption
                                        | 1 << (int)GameCmd.ItemBaseType.ItemBaseType_Material);
                break;
            case KnapsackItemType.ItemEquipment:
                m_int_itemFilterMask = (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Equip);
                break;
            case KnapsackItemType.ItemProps:
                m_int_itemFilterMask = (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Material);
                break;
            case KnapsackItemType.ItemUseable:
                m_int_itemFilterMask = (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Consumption);
                break;
        }
        Transform trans = m_trans_bookmarktoggle.Find(itemType.ToString());
        if (trans != null)
        {
            UIToggle togg = trans.GetComponentInChildren<UIToggle>();
            if (togg != null)
            {
                togg.value = true;
            }
        }
        ShowCanSellItems();
    }
    /// <summary>
    ///  重置购买信息
    /// </summary>
    void ResetSellData() 
    {
       
        curPricePercent = 0;
        prePrice = 0;
        m_label_RecommondPrice.text = "0%";
        m_label_UnitSellPrice.text = "0";
        fax = GameTableManager.Instance.GetGlobalConfig<uint>("SellItemFax");
        m_label_FaxNum.text = fax + "%";   
    }
//    CMResAsynSeedData<CMAtlas> m_emjCASD = null;
    void SetCurItem(BaseItem itemData) 
    {
        ResetSellData();
        m_sellItemGrid.SetGridData(UIItemInfoGridBase.InfoGridType.Consignment, itemData);

//         UIManager.GetTextureAsyn(sdb.strprofessionIcon, ref m_playerAvataCASD, () =>
//         {
//             if (null != m_sprite_icon_head)
//             {
//                 m_sprite_icon_head.atlas = null;
//             }
//         }, m_sprite_icon_head);
// 
//         UIAtlas atlas = DataManager.Manager<UIManager>().GetAtlasByIconName(itemData.Icon);
        m_label_SellingItemName.text = itemData.BaseData.itemName;
        m_label_SellingItemLevel.text = "等级 " + itemData.BaseData.useLevel;
        m_label_SellingItemHint.enabled = false;
        SaleItemDataManager.WantSellItem = itemData;   
        //显示其他玩家的这个物品的寄售价格列表
        if (!isShowSellList)
        {
            NetService.Instance.Send(new stReqPriceInfoConsignmentUserCmd_C() { item_base_id = itemData.BaseData.itemID });
        }
        OnItemNumConfirm(1);

        m_sellItemGrid.ChangeItemNum(1);
        ConsignmentCanSellItem canSellItem = SaleItemDataManager.GetConsignmentCanSellItemInfo(itemData.BaseData.itemID);
        if (itemData.IsTreasure)
        {
            m_trans_TreasureContent.gameObject.SetActive(true);
            m_trans_CommonItemContent.gameObject.SetActive(false);       
            if (canSellItem != null)
            {
                m_label_GuidancePrices.text = string.Format("({0}-{1})", canSellItem.MinYuanBao, canSellItem.MaxYuanBao);
                //策划要求  推荐售价为最低价格的10倍
                m_label_UnitSellPrice.text = (canSellItem.MinYuanBao*10).ToString();
            }
        }
        else 
        {
            if (canSellItem != null)
            {
                m_label_GuidancePrices.text = string.Format("({0}-{1})", canSellItem.MinMoney, canSellItem.MaxMoney);
                //策划要求  推荐售价为最低价格的10倍
                m_label_UnitSellPrice.text =(canSellItem.MinMoney*10 ).ToString();
            }
            m_trans_TreasureContent.gameObject.SetActive(false);
            m_trans_CommonItemContent.gameObject.SetActive(true);
        }

        NetService.Instance.Send(new stItemPriceConsignmentUserCmd_CS() { item_baseid = itemData .BaseId});      
    }
    //当前推荐比例
    float curPricePercent = 0;
    //当前价格
    int prePrice = 0;
    //当前税率
    uint fax = 0;
    void onClick_CommonBtn_Less_Btn(GameObject caster)
    {
        uint itemMinPrice = 0;
        uint itemMaxPrice = 0;
        BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
        if (itemData != null)
        {
            ConsignmentCanSellItem canSellItem = SaleItemDataManager.GetConsignmentCanSellItemInfo(itemData.BaseData.itemID);
            if (canSellItem != null)
            {
                if (itemData.IsTreasure)
                {
                    itemMinPrice = canSellItem.MinYuanBao;
                    itemMaxPrice = canSellItem.MaxYuanBao;
                }
                else
                {
                    itemMinPrice = canSellItem.MinMoney;
                    itemMaxPrice = canSellItem.MaxMoney;
                }
            }
        }


       
        int perAddPercent = SaleItemDataManager.SinglePriceChageValue;
        int total = SaleItemDataManager.MaxRecommendPricePercent;


        if (!NoCanLess)
        {
            if (Math.Abs(curPricePercent) < total)
            {
                curPricePercent -= perAddPercent;
            }
            else if (Math.Abs(curPricePercent) == total)
            {
                if (curPricePercent > 0)
                {
                    curPricePercent -= perAddPercent;
                }
            }  
        }      
        float bili = 1 + curPricePercent / 100f;
        int actuallyPrice = (int)(prePrice * bili + 0.5);

        if (actuallyPrice > itemMinPrice)
        {          
            m_label_RecommondPrice.text = curPricePercent + "%";
            m_label_UnitSellPrice.text = actuallyPrice.ToString();
            NoCanLess = false;
        }
        else
        {
            int gap = (int)(prePrice - itemMinPrice);
            int percent = (int)(gap * 1.0f / prePrice * 100);
            m_label_RecommondPrice.text ="-"+ percent + "%";
            m_label_UnitSellPrice.text = itemMinPrice.ToString();
            NoCanLess = true;
        }
        if (actuallyPrice < itemMinPrice || NoCanLess)
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Trading_IsOutOfLimit);
            return;
        }
        NoCanAdd = false;    
    }


    bool NoCanAdd = false;
    bool NoCanLess = false;
    void onClick_CommonBtn_Add_Btn(GameObject caster)
    {
       
        uint itemMinPrice = 0;
        uint itemMaxPrice = 0;
        BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
        if (itemData != null)
        {
            ConsignmentCanSellItem canSellItem = SaleItemDataManager.GetConsignmentCanSellItemInfo(itemData.BaseData.itemID);
            if (canSellItem != null)
            {
                   if (itemData.IsTreasure)
                    {
                        itemMinPrice = canSellItem.MinYuanBao;
                        itemMaxPrice = canSellItem.MaxYuanBao;
                    }
                    else 
                    {
                        itemMinPrice = canSellItem.MinMoney;
                        itemMaxPrice = canSellItem.MaxMoney;
                    }
            }
        }

       
        int total = SaleItemDataManager.MaxRecommendPricePercent;
        int perAddPercent = SaleItemDataManager.SinglePriceChageValue;


        if (!NoCanAdd)
        {
            if (Math.Abs(curPricePercent) < total)
            {
                curPricePercent += perAddPercent;
            }
            else if (Math.Abs(curPricePercent) == total)
            {
                if (curPricePercent < 0)
                {
                    curPricePercent += perAddPercent;
                }
            }
        }
      

        float bili = 1 + curPricePercent / 100.0f;
        int actuallyPrice = (int)(prePrice * bili +0.5);
        if (actuallyPrice < itemMaxPrice)
        {
            m_label_RecommondPrice.text = curPricePercent + "%";
            m_label_UnitSellPrice.text = actuallyPrice.ToString();
            NoCanAdd = false;
        }
        else
        {
            int gap = (int)(itemMaxPrice - prePrice);
            int percent = (int)(gap * 1.0f / prePrice * 100);
            m_label_RecommondPrice.text =  percent + "%";
            m_label_UnitSellPrice.text = itemMaxPrice.ToString();
            NoCanAdd = true;
        }
        if (actuallyPrice > itemMaxPrice || NoCanAdd)
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Trading_IsOutOfLimit);
            return;
        }       
        NoCanLess = false;      
    }

    void SetRecommondPrice(uint baseid) 
    {
        if (prePrice == 0)
        {
            BaseItem itemData = SaleItemDataManager.WantSellItem as BaseItem;
            if (itemData != null)
            {
                ConsignmentCanSellItem canSellItem = SaleItemDataManager.GetConsignmentCanSellItemInfo(itemData.BaseData.itemID);
                if (canSellItem != null)
                {
                    //策划要求最小值  *10 作为推荐售价
                    if (itemData.IsTreasure)
                    {
                        prePrice = (int)canSellItem.MinYuanBao *10;
                    }
                    else 
                    {
                        prePrice =(int)canSellItem.MinMoney * 10;
                    }
                   
                }
            }
        }
        m_label_UnitSellPrice.text = prePrice.ToString();
        m_label_TotalPriceNum.text = (prePrice * UnitSellNum).ToString();
      
    }
}