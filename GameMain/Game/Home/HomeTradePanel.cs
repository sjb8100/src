
//*******************************************************************************************
//	创建日期：	2016-9-27   15:56
//	文件名称：	HomeTradePanel,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	资源购买回收
//*******************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;

partial class HomeTradePanel : UIPanelBase
{
    #region property

    public enum ETradeType
    {
        ETradeType_Buy,
        ETradeType_Sell
    }

    private ETradeType m_tradeType = ETradeType.ETradeType_Buy;

    //商品页签
    private List<uint> m_lstHomeTradeTab = null;

    //商品页签信息
    private Dictionary<uint, string> m_dicHomeTradeTab = null;

    //当前活动的页签
    private uint m_nSelectTabId = 0;

    //当前选中id
    private uint m_nSelectMallItemId = 0;

    private uint m_nPurchaseNum = 1;

    private UIGridCreatorBase m_tabGridCreator = null;  //页签

    private UIGridCreatorBase m_GridCreator = null;     //商品

    private Dictionary<uint, List<uint>> m_dicMallItem = null;

    private Dictionary<uint, CropLivestock> m_dicCropLivestock = null;

    #endregion


    #region override

    protected override void OnLoading()
    {
        base.OnLoading();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_dicCropLivestock = DataManager.Manager<HomeDataManager>().GetCropLivestockDic();

        onClick_BuyToggle_Btn(m_btn_BuyToggle.gameObject);
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {



        return true;
    }
    #endregion


    #region method

    void InitWidgets()
    {

    }

    void SetSelectStore(ETradeType tradeType)
    {
        //买
        if (tradeType == ETradeType.ETradeType_Buy)
        {
            m_dicMallItem = DataManager.Manager<HomeDataManager>().BuyItemDic;
            m_lstHomeTradeTab = DataManager.Manager<HomeDataManager>().GetTabList(m_tradeType);
            m_dicHomeTradeTab = DataManager.Manager<HomeDataManager>().TabNameDic;

            m_label_AddRemove_Text.text = "购买数量：";
            m_label_Obtain_Text.text = "消    耗：";
            m_label_Btn_name.text = "购买";
        }

        //卖
        if (tradeType == ETradeType.ETradeType_Sell)
        {
            m_dicMallItem = DataManager.Manager<HomeDataManager>().SellItemDic;
            m_lstHomeTradeTab = DataManager.Manager<HomeDataManager>().GetTabList(m_tradeType);
            m_dicHomeTradeTab = DataManager.Manager<HomeDataManager>().TabNameDic;

            m_label_AddRemove_Text.text = "出售数量：";
            m_label_Obtain_Text.text = "获    得：";
            m_label_Btn_name.text = "出售";
        }

        if (m_GridCreator != null) m_GridCreator.ClearAll();//清除所有格子
        CreateHomeTradeUI();
    }

    /// <summary>
    /// 创建商城UI
    /// </summary>
    public void CreateHomeTradeUI()
    {
        m_nSelectTabId = m_lstHomeTradeTab[0];
        InitTabWidgets();
        CreateTab(); // 创建商品页签
        SetSelectTab(m_nSelectTabId);//当前选中的页面签
    }

    /// <summary>
    /// 创建商品页签
    /// </summary>
    void InitTabWidgets()
    {
        if (m_tabGridCreator == null)
        {
            m_tabGridCreator = m_trans_CategoryTagContent.GetComponent<UIGridCreatorBase>();
            if (m_tabGridCreator == null)
            {
                m_tabGridCreator = m_trans_CategoryTagContent.gameObject.AddComponent<UIGridCreatorBase>();
            }

            m_tabGridCreator.gridContentOffset = new Vector2(0, 0);
            m_tabGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
            m_tabGridCreator.gridWidth = 150;
            m_tabGridCreator.gridHeight = 55;

            m_tabGridCreator.RefreshCheck();
            UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uitabgrid) as UnityEngine.GameObject;
            m_tabGridCreator.Initialize<UITabGrid>(obj, OnUpdateGridData, OnGridUIEventDlg);
        }
    }

    void CreateTab()
    {
        if (m_tabGridCreator != null)
        {
            m_tabGridCreator.CreateGrids(m_lstHomeTradeTab != null ? m_lstHomeTradeTab.Count : 0);
        }
    }

    /// <summary>
    /// 选中的页签
    /// </summary>
    /// <param name="id"></param>
    void SetSelectTab(uint id)
    {
        UITabGrid grid = m_tabGridCreator.GetGrid<UITabGrid>(m_lstHomeTradeTab.IndexOf(m_nSelectTabId));
        if (grid != null)
        {
            grid.SetHightLight(false);
        }

        grid = m_tabGridCreator.GetGrid<UITabGrid>(m_lstHomeTradeTab.IndexOf(id));
        if (grid != null)
        {
            grid.SetHightLight(true);
        }

        m_nSelectTabId = id;

        CreateMallUIList();
    }

    /// <summary>
    /// 构建商城列表UI
    /// </summary>
    private void CreateMallUIList()
    {
        m_GridCreator = m_trans_ScrollViewContent.GetComponent<UIGridCreatorBase>();
        if (m_GridCreator == null)
        {
            m_GridCreator = m_trans_ScrollViewContent.gameObject.AddComponent<UIGridCreatorBase>();
        }

        m_GridCreator.gridContentOffset = new Vector2(-200f, 0);
        m_GridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
        m_GridCreator.gridWidth = 356;
        m_GridCreator.gridHeight = 145;
        m_GridCreator.rowcolumLimit = 2;
        m_GridCreator.RefreshCheck();
        if (m_tradeType == ETradeType.ETradeType_Buy)  //买
        {
            GameObject resObj = UIManager.GetResGameObj(GridID.Uimallgrid) as GameObject;
            m_GridCreator.Initialize<UIHomeTradeBuyGrid>(resObj, OnUpdateGridData, OnGridUIEventDlg);
        }

        if (m_tradeType == ETradeType.ETradeType_Sell) //卖
        {
            GameObject resObj = UIManager.GetResGameObj(GridID.Uihometradegrid) as GameObject;
            m_GridCreator.Initialize<UIHomeTradeSellGrid>(resObj, OnUpdateGridData, OnGridUIEventDlg);
        }
        m_GridCreator.CreateGrids(m_dicMallItem[m_nSelectTabId].Count);

        //默认选中第一个
        uint nextActiveItemId = 0;
        if (m_dicMallItem[m_nSelectTabId].Count > 0)
            nextActiveItemId = m_dicMallItem[m_nSelectTabId][0];
        SetSelectItem(nextActiveItemId);//选中第一个

    }


    /// <summary>
    /// grid点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                if (data is UIHomeTradeBuyGrid)
                {
                    UIHomeTradeBuyGrid buyGrid = data as UIHomeTradeBuyGrid;
                    if (null != buyGrid)
                        SetSelectItem(buyGrid.MallItemId);
                }
                else if (data is UIHomeTradeSellGrid)
                {
                    UIHomeTradeSellGrid sellGrid = data as UIHomeTradeSellGrid;
                    if (null != sellGrid)
                        SetSelectItem(sellGrid.MallItemId);
                }
                else if (data is UITabGrid)
                {
                    UITabGrid tabGrid = data as UITabGrid;
                    if (null != tabGrid)
                        SetSelectTab((uint)tabGrid.Data);
                }
                break;
        }
    }

    /// <summary>
    /// 选中的商品
    /// </summary>
    /// <param name="id"></param>
    void SetSelectItem(uint id)
    {
        if (m_tradeType == ETradeType.ETradeType_Buy)
        {
            if (null != m_GridCreator)
            {
                UIHomeTradeBuyGrid grid = m_GridCreator.GetGrid<UIHomeTradeBuyGrid>(m_dicMallItem[m_nSelectTabId].IndexOf(m_nSelectMallItemId));
                if (null != grid)
                {
                    grid.SetHightLight(false);
                }
                grid = m_GridCreator.GetGrid<UIHomeTradeBuyGrid>(m_dicMallItem[m_nSelectTabId].IndexOf(id));
                if (null != grid)
                {
                    grid.SetHightLight(true);
                }
            }
        }

        if (m_tradeType == ETradeType.ETradeType_Sell)
        {
            if (null != m_GridCreator)
            {
                UIHomeTradeSellGrid grid = m_GridCreator.GetGrid<UIHomeTradeSellGrid>(m_dicMallItem[m_nSelectTabId].IndexOf(m_nSelectMallItemId));
                if (null != grid)
                {
                    grid.SetHightLight(false);
                }
                grid = m_GridCreator.GetGrid<UIHomeTradeSellGrid>(m_dicMallItem[m_nSelectTabId].IndexOf(id));
                if (null != grid)
                {
                    grid.SetHightLight(true);
                }
            }
        }

        //重置购买数量
        m_nPurchaseNum = 1;
        this.m_nSelectMallItemId = id;
        UpdatePurchaseInfo();
    }

    /// <summary>
    /// 跟新商品展示信息
    /// </summary>
    void UpdatePurchaseInfo()
    {
        HomeTradeDataBase htDb = GameTableManager.Instance.GetTableItem<HomeTradeDataBase>(m_nSelectMallItemId);
        if (htDb == null)
        {
            Engine.Utility.Log.Error("Can Not Find Data !!!");
            return;
        }

        if (m_tradeType == ETradeType.ETradeType_Buy)
        {
            SeedAndCubDataBase data = GameTableManager.Instance.GetTableItem<SeedAndCubDataBase>(htDb.itemID);//种子幼崽表中取
            if (data == null)
            {
                Engine.Utility.Log.Error("Can Not Find Data !!!");
                return;
            }

            m_label_MallItemName.text = data.name;
            m_label_MallItemDes.text = data.des;
        }
        else
        {
            ItemDataBase data = GameTableManager.Instance.GetTableItem<ItemDataBase>(htDb.itemID);//item表中取
            if (data == null)
            {
                Engine.Utility.Log.Error("Can Not Find Data !!!");
                return;
            }

            m_label_MallItemName.text = data.itemName;
            m_label_MallItemDes.text = data.description;
        }

        UpdatePurchaseNum();
    }

    /// <summary>
    /// 买格子数据刷新
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    private void OnUpdateGridData(UIGridBase grid, int index)
    {
        //买
        if (grid is UIHomeTradeBuyGrid)
        {
            if (index >= m_dicMallItem[m_nSelectTabId].Count)
            {
                Engine.Utility.Log.Error("Out Of Index !!!");
                return;
            }

            UIHomeTradeBuyGrid buyGrid = grid as UIHomeTradeBuyGrid;
            if (buyGrid == null)
            {
                return;
            }

            HomeTradeDataBase htDb = GameTableManager.Instance.GetTableItem<HomeTradeDataBase>(m_dicMallItem[m_nSelectTabId][index]);//大管家
            if (htDb == null)
            {
                Engine.Utility.Log.Error("Can Not Find Data !!!");
                return;
            }

            SeedAndCubDataBase data = GameTableManager.Instance.GetTableItem<SeedAndCubDataBase>(htDb.itemID);//种子幼崽表中取
            if (data == null)
            {
                Engine.Utility.Log.Error("Can Not Find Data !!!");
                return;
            }

            buyGrid.SetGridData(htDb.indexId);
            buyGrid.SetIcon(data.icon);
            buyGrid.SetName(data.name);
            buyGrid.SetNum(htDb.pileNum); //叠加数量
            buyGrid.SetPrice((GameCmd.MoneyType)htDb.moneyType, htDb.price); //现在的价格
            bool select = (htDb.indexId == m_nSelectMallItemId) ? true : false;
            buyGrid.SetHightLight(select);
            //buyGrid.SetTag("");

        }
        //卖
        else if (grid is UIHomeTradeSellGrid)
        {
            if (index >= m_dicMallItem[m_nSelectTabId].Count)
            {
                Engine.Utility.Log.Error("Out Of Index !!!");
                return;
            }

            UIHomeTradeSellGrid sellGrid = grid as UIHomeTradeSellGrid;
            if (sellGrid == null)
            {
                return;
            }

            HomeTradeDataBase htDb = GameTableManager.Instance.GetTableItem<HomeTradeDataBase>(m_dicMallItem[m_nSelectTabId][index]);
            if (htDb == null)
            {
                Engine.Utility.Log.Error("Can Not Find Data !!!");
                return;
            }

            ItemDataBase data = GameTableManager.Instance.GetTableItem<ItemDataBase>(htDb.itemID);//item表中取
            if (data == null)
            {
                Engine.Utility.Log.Error("Can Not Find Data !!!");
                return;
            }

            CropLivestock cropLivestock = m_dicCropLivestock[data.itemID];

            sellGrid.SetGridData(htDb.indexId);
            sellGrid.SetIcon(data.itemIcon);
            sellGrid.SetName(data.itemName);
            sellGrid.SetNum(htDb.pileNum); //叠加数量
            sellGrid.SetPrice((GameCmd.MoneyType)htDb.moneyType, (uint)cropLivestock.m_nPrice, cropLivestock.m_fPriceChangeRate, cropLivestock.m_bUp); //现在的价格
            bool select = (htDb.indexId == m_nSelectMallItemId) ? true : false;
            sellGrid.SetHightLight(select);
            //buyGrid.SetTag("");
        }
            //页签
        else if (grid is UITabGrid)
        {
            if (index > m_lstHomeTradeTab.Count)
            {
                Engine.Utility.Log.Error("Can Not Find Data !!!");
                return;
            }

            UITabGrid tabGrid = grid as UITabGrid;
            if (grid == null)
            {
                return;
            }

            tabGrid.SetGridData(m_lstHomeTradeTab[index]);
            tabGrid.SetName(m_dicHomeTradeTab[m_lstHomeTradeTab[index]]);
            bool select = m_lstHomeTradeTab[index] == m_nSelectTabId ? true : false;
            tabGrid.SetHightLight(select);
        }
    }

    private void OnAddRemove(bool add)
    {
        if (!add && m_nPurchaseNum == 1)
            return;
        m_nPurchaseNum = (add) ? (m_nPurchaseNum + 1) : (m_nPurchaseNum - 1);
        UpdatePurchaseNum();
    }

    void UpdatePurchaseNum()
    {
        HomeTradeDataBase htDb = GameTableManager.Instance.GetTableItem<HomeTradeDataBase>(m_nSelectMallItemId);
        if (htDb == null)
        {
            Engine.Utility.Log.Error("Can Not Find Data !!!");
            return;
        }

        if (null != m_label_PurchaseNum)
            m_label_PurchaseNum.text = "" + m_nPurchaseNum * htDb.pileNum;
        UpdateCost();
    }

    void UpdateCost()
    {
        if (null != m_trans_PurchaseCostGrid)
        {
            UICurrencyGrid currencyGrid = m_trans_PurchaseCostGrid.GetComponent<UICurrencyGrid>();
            /*
                        if (null != currencyGrid)
                        {
                            GameCmd.MoneyType moneyType = GameCmd.MoneyType.MoneyType_GoldTicket;
                            uint totalCost = 0;
                            if (null != current)
                            {
                                moneyType = (GameCmd.MoneyType)current.LocalMall.moneyType;
                                totalCost = current.LocalMall.buyPrice * purchaseNum;
                            }
                            currencyGrid.SetGridData(new MallDefine.CurrencyData(moneyType, totalCost));
                        }
                        */
            if (m_tradeType == ETradeType.ETradeType_Buy)
            {

            }
            else
            {

            }
        }
    }
    #endregion


    #region click
    void onClick_BuyToggle_Btn(GameObject caster)
    {
        m_tradeType = ETradeType.ETradeType_Buy;
        SetSelectStore(m_tradeType);
    }

    void onClick_SellToggle_Btn(GameObject caster)
    {
        m_tradeType = ETradeType.ETradeType_Sell;
        SetSelectStore(m_tradeType);
    }



    void onClick_PurchaseBtn_Btn(GameObject caster)
    {
        if (m_nSelectMallItemId == 0)
        {
            TipsManager.Instance.ShowTips("请选择要购买的商品");
            return;
        }
        if (m_nPurchaseNum == 0)
        {
            TipsManager.Instance.ShowTips("购买数量不能为0");
            return;
        }

        HomeTradeDataBase htDb = GameTableManager.Instance.GetTableItem<HomeTradeDataBase>(m_nSelectMallItemId);
        if (htDb == null)
        {
            Engine.Utility.Log.Error("Can Not Find Data !!!");
            return;
        }

        if (m_tradeType == ETradeType.ETradeType_Buy)
        {
            DataManager.Manager<HomeDataManager>().ReqBuySeedAndCub(htDb.itemID, m_nPurchaseNum * htDb.pileNum);
        }
        else
        {
            DataManager.Manager<HomeDataManager>().ReqSellSeedAndCub(htDb.itemID, m_nPurchaseNum * htDb.pileNum);
        }
    }



    void onClick_BtnAdd_Btn(GameObject caster)
    {
        OnAddRemove(true);
    }



    void onClick_BtnRemove_Btn(GameObject caster)
    {
        OnAddRemove(false);
    }


    #endregion


}

