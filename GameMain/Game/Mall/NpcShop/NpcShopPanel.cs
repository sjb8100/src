using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Common;
using Client;
using GameCmd;
using DG.Tweening;
using table;

public partial class NpcShopPanel:UIPanelBase
{
    #region define
    //商城显示面板
    public class MallPanelShowData
    {
        //一级分类
        public int FirstMask = 1;
        //二级分类
        public int SecondMasK = 1;
    }
    #endregion

    #region property
    //商城物品字典
    private List<uint> m_lst_mallDatas = null;
    //商品页签信息
    private List<int> m_lst_mallTabDatas = null;
    //当前活动的商城类型
    private GameCmd.CommonStore activeStore = GameCmd.CommonStore.CommonStore_Four;
    private Dictionary<GameCmd.CommonStore, UIToggleGrid> m_dic_commontabs = new Dictionary<CommonStore,UIToggleGrid>();
    private UIItemInfoGrid m_mallItemBaseGrid = null;
    //当前选中id
    private uint selectMallItemId = 0;
    //购买数量
    private int purchaseNum = 1;
    //当前活动的页签
    private int activeTabId = 0;
    //当前选中商城数据
    public MallDefine.MallLocalData CurrentMallData
    {
        get
        {
            return (selectMallItemId != 0) ? DataManager.Manager<MallManager>().GetMallLocalDataByMallItemId(selectMallItemId) : null;
        }
    }

    Dictionary<uint, List<int>> dic = null;

    //面板显示数据
    private MallPanelShowData m_mallPanelData = null;
    public MallPanelShowData MallPanelData
    {
        get
        {
            return m_mallPanelData;
        }
    }
    NpcShopTabs npcData = null;
    #endregion

    #region overridemethod
    protected override void OnInitPanelData()
    {
        base.OnInitPanelData();
    }

    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }
    protected override void OnHide()
    {
        base.OnHide();
        DataManager.Manager<TaskDataManager>().DoingTaskID = 0;
    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        if (null != data && data is MallPanelShowData)
        {
            m_mallPanelData = data as MallPanelShowData;
        }

        if (null == m_mallPanelData)
        {
            m_mallPanelData = new MallPanelShowData();
        }
        selectMallItemId = 0;
        List<uint> li = null;
        if (null != data && data is NpcShopTabs)
        {
            npcData = data as NpcShopTabs;
             li= npcData.param.Keys.ToList();
        }
        if (li != null)
        {
           SetActiveStore((GameCmd.CommonStore)li[0], true);
        }
     
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        RegisterGlobalUIEvent(false);
        Release();

        if (m_mallItemBaseGrid != null)
        {
            m_mallItemBaseGrid.Release(true);
           // UIManager.OnObjsRelease(m_mallItemBaseGrid.CacheTransform, (uint)GridID.Uiiteminfogrid);
            m_mallItemBaseGrid = null;
        }
    }
    #endregion

    #region Common

    /// <summary>
    /// 注册ui全局事件
    /// </summary>
    /// <param name="register"></param>
    private void RegisterGlobalUIEvent(bool register)
    {
        if (register)
        {

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHPURCHASENUM, GlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_TAGSTATUSCHANGED, GlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHPURCHASENUM, GlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_TAGSTATUSCHANGED, GlobalUIEventHandler);
        }
    }

    private void GlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENT_REFRESHPURCHASENUM:
                if (IsSelectItemEnable() && CurrentMallData.LocalMall.mallItemId == ((uint)data))
                {
                    //刷新次数
                    UpdateLeftTimes();
                }
                break;
            case (int)Client.GameEventID.UIEVENT_TAGSTATUSCHANGED:
                uint mallItemId = (uint)data;
                if (null != m_ctor_MallScrollView
                    && null != m_lst_mallDatas
                    && m_lst_mallDatas.Contains(mallItemId))
                {
                    m_ctor_MallScrollView.UpdateData(m_lst_mallDatas.IndexOf(mallItemId));
                }

                if (mallItemId == selectMallItemId)
                {
                    UpdatePurchaseInfo();
                }
                break;
        }
    }
    /// <summary>
    /// 设置选中数据
    /// </summary>
    /// <param name="mallItemId"></param>
    public void SetSelectItemId(uint mallItemId, bool force = false, bool needFocus = false,bool isBackSpace = false)
    {
        if (this.selectMallItemId == mallItemId && !force)
            return;
        if (null != m_ctor_MallScrollView)
        {
            UIMallGrid grid = m_ctor_MallScrollView.GetGrid<UIMallGrid>(m_lst_mallDatas.IndexOf(selectMallItemId));
            if (null != grid)
            {
                grid.SetHightLight(false);
            }
            if (needFocus)
            {
                m_ctor_MallScrollView.FocusGrid(m_lst_mallDatas.IndexOf(mallItemId));
            }
            grid = m_ctor_MallScrollView.GetGrid<UIMallGrid>(m_lst_mallDatas.IndexOf(mallItemId));
            if (null != grid)
            {
                grid.SetHightLight(true);
            }
        }     
        //重置购买数量
        if (isBackSpace)
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

    /// <summary>
    /// 设置活动标签
    /// </summary>
    /// <param name="tabId"></param>
    public void SetActiveTab(int tabId, bool force = false)
    {
        if (tabId == activeTabId && !force)
            return;

        if (null != m_ctor_CategoryTagContent)
        {
            if (m_lst_mallTabDatas == null)
            {
                StructTabData();
            }
            UITabGrid grid = m_ctor_CategoryTagContent.GetGrid<UITabGrid>(m_lst_mallTabDatas.IndexOf(activeTabId));
            if (null != grid)
            {
                grid.SetHightLight(false);
            }
            grid = m_ctor_CategoryTagContent.GetGrid<UITabGrid>(m_lst_mallTabDatas.IndexOf(tabId));
            if (null != grid)
            {
                grid.SetHightLight(true);
            }
        }
        activeTabId = tabId;
        //手动设置二级页签
        dicActiveTabGrid[2] = tabId;
        CreateMallUIList();
    }

    public void CreateTab()
    {
        StructTabData();
        if (null != m_ctor_CategoryTagContent)
        {
            activeTabId = (m_lst_mallTabDatas.Count > 0) ? m_lst_mallTabDatas[0] : 0;
            int count = m_lst_mallTabDatas.Count;
            if (m_lst_mallTabDatas.Count == 1 && m_lst_mallTabDatas[0] == 0)
                count = 0;
            m_ctor_CategoryTagContent.CreateGrids(count, true);
        }
        if (null != m_ctor_RightTabRoot)
        {
            Dictionary<uint, List<int>> dic = DataManager.Manager<Mall_NpcShopManager>().Tabs;
            m_ctor_RightTabRoot.CreateGrids(dic.Count, true);
            List<UIToggleGrid> l = m_ctor_RightTabRoot.GetGrids<UIToggleGrid>();
            for (int i = 0; i < l.Count; i++)
            {
                uint key = DataManager.Manager<Mall_NpcShopManager>().GetNpcShopKey(i);
                if (!m_dic_commontabs.ContainsKey((GameCmd.CommonStore)key))
                {
                 m_dic_commontabs.Add((GameCmd.CommonStore)key, l[i]);
                }
               
            }
            
        }
        SetActiveTab(activeTabId, true);
    }

    /// <summary>
    /// 设置活动数据
    /// </summary>
    /// <param name="activeStore"></param>
    /// <param name="force"></param>
    private void SetActiveStore(GameCmd.CommonStore activeStore, bool force = false)
    {
        UIToggleGrid tog = null;
        if (null != m_dic_commontabs && m_dic_commontabs.TryGetValue(this.activeStore, out tog))
        {
            tog.SetHightLight(false);
        }
        this.activeStore = activeStore;
        if (null != m_dic_commontabs && m_dic_commontabs.TryGetValue(this.activeStore, out tog))
        {
            tog.SetHightLight(true);
        }
        this.activeStore = activeStore;
        this.activeTabId = 0;
        CreateTab();
    }
    /// <summary>
    /// 商城格子数据刷新
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    private void OnUpdateMallGridData(UIGridBase grid, int index)
    {
        if (grid is UIMallGrid)
        {
            if (index >= m_lst_mallDatas.Count)
            {
                Engine.Utility.Log.Error("MallPanel OnUpdateMallGridData faield,m_lst_mallDatas data errir");
                return;
            }

            UIMallGrid mallGrid = grid as UIMallGrid;
            MallDefine.MallLocalData data = DataManager.Manager<MallManager>().GetMallLocalDataByMallItemId(m_lst_mallDatas[index]);
            if (null == data)
            {
                Engine.Utility.Log.Error("MallPanel OnUpdateMallGridData faield,mall data errir index:{0}", index);
                return;
            }
            mallGrid.SetGridData(data.MallId);
            bool select = (data.MallId == selectMallItemId) ? true : false;
            mallGrid.SetHightLight(select);
        }
        else if (grid is UITabGrid)
        {
            if (index > m_lst_mallTabDatas.Count)
            {
                Engine.Utility.Log.Error("MallPanel OnUpdateMallGridData faield,m_lst_mallTabDatas data errir");
                return;
            }
            int tabKey = m_lst_mallTabDatas[index];
            int startDepth = 2 + m_lst_mallTabDatas.Count * 2 - index * 2;
            UITabGrid tabGrid = grid as UITabGrid;
            tabGrid.SetName(DataManager.Manager<MallManager>().GetMallTagName((int)activeStore, tabKey));
            tabGrid.SetHightLight(activeTabId == tabKey ? true : false);
            tabGrid.SetGridData(tabKey);
            tabGrid.SetDepth(startDepth);
        }
        else if (grid is UIToggleGrid)
        {
            UIToggleGrid tog = grid as UIToggleGrid;
            uint shopID = DataManager.Manager<Mall_NpcShopManager>().GetNpcShopKey(index);
            string shopName = DataManager.Manager<Mall_NpcShopManager>().GetNpcShopName(index);
            tog.SetName(shopName);
            tog.SetHightLight((uint)activeStore == shopID?true:false);
            tog.SetGridTab((int)shopID);
        
        }

    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            SetActiveStore((GameCmd.CommonStore)pageid, false);
        }
        return base.OnTogglePanel(tabType, pageid);
    }

    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (UIMsgID.eShowUI == msgid)
        {
            if (param is ReturnBackUIMsg)
            {
                ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
                if (showInfo.tabs.Length > 0)
                {
                    UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, showInfo.tabs[0]);
                }

                if (showInfo.tabs.Length > 1)
                {
                    SetActiveTab(showInfo.tabs[1], true);
                }

                if (showInfo.param is uint)
                {
                    SetSelectItemId((uint)showInfo.param, true);
                }
            }
        }
        return base.OnMsg(msgid, param);
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (jumpData == null)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
        int secondTabData = -1;
        UIPanelBase.PanelJumpData data = (UIPanelBase.PanelJumpData)jumpData;
        if (data.Tabs != null)
        {
            if (data.Tabs.Length > 0)
            {
                UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, data.Tabs[0]);
            }
            if (data.Tabs.Length > 1)
            {
                SetActiveTab(data.Tabs[1], true);
            }
        }    
        if (data.Param is uint)
        {
                    uint storeItemID = (uint)data.Param;
                    StoreDataBase table = GameTableManager.Instance.GetTableItem<StoreDataBase>(storeItemID);
                    if (table != null)
                    {                    
                        SetActiveStore((GameCmd.CommonStore)(int)table.storeId);
                        SetActiveTab((int)table.tag);
                        SetSelectItemId(storeItemID, true,true);
                    }
                    else
                    {
                        Engine.Utility.Log.Error("ID为{0}的商品为null", storeItemID);
                    }
        }       
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[2];
        pd.JumpData.Tabs[0] = (int)activeStore;
        pd.JumpData.Tabs[1] = activeTabId;
        pd.JumpData.Param = selectMallItemId;
        pd.JumpData.ExtParam = purchaseNum;       
        return pd;
    }
    /// <summary>
    /// 商城格子UI事件委托
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridUIEventDlg(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
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
                        if (tabGrid.Data is int)
                        {
                            SetActiveTab((int)tabGrid.Data);
                        }
                       
                    }
                }
                else if (data is UIToggleGrid)
                {
                    UIToggleGrid tab = data as UIToggleGrid;
                    SetActiveStore((GameCmd.CommonStore)(int)tab.Data);
                }
                break;
        }
    }

    /// <summary>
    /// 构建页签数据
    /// </summary>
    private void StructTabData()
    {
        if (null == m_lst_mallTabDatas)
            m_lst_mallTabDatas = new List<int>();

        if (dic.ContainsKey((uint)activeStore))
        {
            m_lst_mallTabDatas = dic[(uint)activeStore];

        }
        else
        {
            Engine.Utility.Log.Error("商城页签枚举值与NPC商店商城ID不匹配");
        }
        
    }

    /// <summary>
    /// 构建数据
    /// </summary>
    private void StructData()
    {
        if (null == m_lst_mallDatas)
            m_lst_mallDatas = new List<uint>();
        m_lst_mallDatas.Clear();
        m_lst_mallDatas.AddRange(
            DataManager.Manager<MallManager>().GetMallDatas((int)activeStore, (int)activeTabId));
    }

    /// <summary>
    /// 构建商城列表UI
    /// </summary>
    private void CreateMallUIList()
    {
        StructData();
        if (null != m_ctor_MallScrollView)
        {
            m_ctor_MallScrollView.CreateGrids(m_lst_mallDatas.Count);
            uint nextActiveItemId = 0;
            if (m_lst_mallDatas.Count > 0)
                nextActiveItemId = m_lst_mallDatas[0];
            SetSelectItemId(nextActiveItemId);
        }
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_MallScrollView != null)
        {
            m_ctor_MallScrollView.Release(depthRelease);
        }
        if (m_ctor_CategoryTagContent != null)
        {
            m_ctor_CategoryTagContent.Release(depthRelease);
        }
        if (m_ctor_RightTabRoot != null)
        {
            m_ctor_RightTabRoot.Release(depthRelease);
        }
    }
 
    private void InitWidgets()
    {
        dic = DataManager.Manager<Mall_NpcShopManager>().Tabs;
        RegisterGlobalUIEvent(true);
        GameObject preObj = m_trans_UIItemInfoGrid.gameObject;
        if (null != preObj && null != m_trans_MallBaseGridRoot && null == m_mallItemBaseGrid)
        {
            GameObject cloneObj = NGUITools.AddChild(m_trans_MallBaseGridRoot.gameObject, preObj);
            if (null != cloneObj)
            {
                m_mallItemBaseGrid = cloneObj.GetComponent<UIItemInfoGrid>();
                if (null == m_mallItemBaseGrid)
                {
                    m_mallItemBaseGrid = cloneObj.AddComponent<UIItemInfoGrid>();
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
            }
            m_trans_MallBaseGridRoot.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        }
        if (null != m_trans_PurchaseCostGrid)
        {
            if (null == m_trans_PurchaseCostGrid.GetComponent<UICurrencyGrid>())
                m_trans_PurchaseCostGrid.gameObject.AddComponent<UICurrencyGrid>();
        }
        if (null != m_trans_OwnMoneyGrid)
        {
            if (null == m_trans_OwnMoneyGrid.GetComponent<UICurrencyGrid>())
                m_trans_OwnMoneyGrid.gameObject.AddComponent<UICurrencyGrid>();
        }

        if (null != m_ctor_MallScrollView)
        {
            if (null == m_trans_PurchaseCostGrid.GetComponent<UICurrencyGrid>())
                m_trans_PurchaseCostGrid.gameObject.AddComponent<UICurrencyGrid>();
            if (null == m_trans_OwnMoneyGrid.GetComponent<UICurrencyGrid>())
                m_trans_OwnMoneyGrid.gameObject.AddComponent<UICurrencyGrid>();
        }

        if (null != m_ctor_CategoryTagContent)
        {
            m_ctor_CategoryTagContent.RefreshCheck();
            m_ctor_CategoryTagContent.Initialize<UITabGrid>(m_trans_UITabGrid.gameObject, OnUpdateMallGridData, OnGridUIEventDlg);
        }

        if (null != m_ctor_RightTabRoot)
        {
            List<string> m_lstDepende = new List<string>();               
            m_ctor_RightTabRoot.RefreshCheck();
            m_ctor_RightTabRoot.Initialize<UIToggleGrid>(m_trans_TogglePanel.gameObject, OnUpdateMallGridData, OnGridUIEventDlg);
                
        }

        if (null != m_ctor_MallScrollView)
        {
                m_ctor_MallScrollView.RefreshCheck();
                m_ctor_MallScrollView.Initialize<UIMallGrid>(m_trans_UIMallGrid.gameObject, OnUpdateMallGridData, OnGridUIEventDlg);

        }

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

    /// <summary>
    /// 刷新购买数量
    /// </summary>
    private void UpdatePurchaseNum()
    {
        if (!IsSelectItemEnable())
        {
            return;
        }
        purchaseNum = Mathf.Max(0, Mathf.Min(purchaseNum,CurrentMallData.MaxPurchaseNum));
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
        if(null != m_trans_OwnMoneyGrid)
        {
            UICurrencyGrid currencyGrid = m_trans_OwnMoneyGrid.GetComponent<UICurrencyGrid>();
            if (null != currencyGrid)
            {
                GameCmd.MoneyType moneyType = GameCmd.MoneyType.MoneyType_Coin;
                uint totalHave = 0;
                if (null != current)
                {
                    moneyType = (GameCmd.MoneyType)current.LocalMall.moneyType;

                    totalHave = (uint)MainPlayerHelper.GetMoneyNumByType((ClientMoneyType)current.LocalMall.moneyType);
                }
                currencyGrid.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                    MallDefine.GetCurrencyIconNameByType((GameCmd.MoneyType)moneyType), totalHave));
            }
        
        }
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
                ColorType color = (current.LocalItem.UseLv > DataManager.Instance.PlayerLv) ? ColorType.Red : ColorType.Green;
                m_label_MallItemUseLv.text = DataManager.Manager<TextManager>()
                    .GetLocalFormatText(LocalTextType.Local_TXT_Mall_UselevelDescribe
                    , ColorManager.GetNGUIColorOfType(color), current.LocalItem.UseLv);
            }

            if (null != m_label_MallItemDes)
                m_label_MallItemDes.text = current.LocalItem.Des;

            if (null != m_mallItemBaseGrid)
            {
                m_mallItemBaseGrid.Reset();
                m_mallItemBaseGrid.SetIcon(true, current.LocalItem.Icon);
                m_mallItemBaseGrid.SetBorder(true, current.LocalItem.BorderIcon);
                //m_mallItemBaseGrid.SetBindMask(current.LocalItem.IsBind);
                //SetTimeLimitMask(false);
                //SetFightUp(false);
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

    public bool IsSelectItemEnable()
    {
        if (null == CurrentMallData)
        {
            return false;
        }
        else 
        {
           if( null == CurrentMallData.LocalItem || null == CurrentMallData.LocalMall)
           {
               return false;
           }
        }
        return true;
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
        //变更输入数量
        //if (CurrentMallData.IsDayNumLimit
        //    && CurrentMallData.LeftTimes < MAX_INPUT_NUM
        //    && DataManager.Manager<UIPanelManager>().IsShowPanel(PanelID.HandInputPanel))
        //{
        //    HandInputPanel panel
        //        = DataManager.Manager<UIPanelManager>().GetPanel<HandInputPanel>(PanelID.HandInputPanel);
        //}
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

    #endregion

    #region Update
    //更新商城刷新间隔
    public const float MALL_REFRESH_LEFTTIME_GAP = 1F;
    //下一次刷新商城打折剩余的时间
    private float next_refresh_left_time = 0;
    private void ImmediatelyRefreshLeftTime()
    {
        next_refresh_left_time = 0;
    }

    /// <summary>
    /// 刷新打折剩余时间
    /// </summary>
    private void UpdateDisCountLeftTime()
    {
        bool visible = (null != CurrentMallData
            && CurrentMallData.IsOpenDiscount
            && CurrentMallData.HasSchedule);
        long leftSeconds = 0;
        if (visible)
        {
            CurrentMallData.CheckTagStatus();
            visible = CurrentMallData.IsTimeInSchedule(DateTimeHelper.Instance.Now, out leftSeconds);
        }
        if (null != m_label_DiscountLeftTime)
        {
            if (m_label_DiscountLeftTime.gameObject.activeSelf != visible)
            {
                m_label_DiscountLeftTime.gameObject.SetActive(visible);
            }

            if (visible)
            {
                m_label_DiscountLeftTime.text = string.Format("剩余时间：{0}"
                    , DateTimeHelper.ParseTimeSeconds((int)leftSeconds));
            }
        }
    }
    void Update()
    {
        if (null != CurrentMallData
            && CurrentMallData.IsOpenDiscount
            && CurrentMallData.HasSchedule)
        {
            next_refresh_left_time -= Time.deltaTime;
            if (next_refresh_left_time <= UnityEngine.Mathf.Epsilon)
            {
                next_refresh_left_time = MALL_REFRESH_LEFTTIME_GAP;
                UpdateDisCountLeftTime();
            }
        }

    }
    #endregion

    #region UIEvent
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
        DataManager.Manager<MallManager>().PurchaseMallItem(selectMallItemId, (uint)activeStore, purchaseNum);
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
        if (CurrentMallData != null)
        {
            OnHandInput(CurrentMallData.MaxPurchaseNum);
        }
      
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
    #endregion


    
}