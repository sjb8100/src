/************************************************************************************
 * Copyright (c) 2018  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Mall
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ShopPanel
 * 版本号：  V1.0.0.0
 * 创建时间：3/30/2018 2:34:47 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using GameCmd;

partial class ShopPanel
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
    //1级活动页签
    private TabMode memActiveFirstTab = TabMode.None;
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

    private int ActiveStore
    {
        get
        {
            int activeStore = (int)CommonStore.CommonStore_Three;
            switch (memActiveFirstTab)
            {
                case TabMode.JinBi:
                    activeStore = (int)CommonStore.CommonStore_Three;
                    break;
                case TabMode.ShengWang:
                    activeStore = (int)CommonStore.CommonStore_Ten;
                    break;
                case TabMode.YuBi:
                    activeStore = (int)CommonStore.CommonStore_Eight;
                    break;
                case TabMode.JiFen:
                    activeStore = (int)CommonStore.CommonStore_Eleven;
                    break;
                case TabMode.LieHun:
                    activeStore = (int)CommonStore.CommonStore_Thirteen;
                    break;
                case TabMode.ZhanXun:
                    activeStore = (int)CommonStore.CommonStore_Twelve;
                    break;
            }
            return activeStore;
        }
    }
    //面板显示数据
    private MallPanelShowData m_mallPanelData = null;
    public MallPanelShowData MallPanelData
    {
        get
        {
            return m_mallPanelData;
        }
    }

    //第一次跳转数据
    private PanelJumpData mFirstJumpData = null;
    private bool mBSecondJump = false;


    CMResAsynSeedData<CMAtlas> m_costIconCASD = null;
    CMResAsynSeedData<CMAtlas> m_ownIconCASD = null;
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (null != mFirstJumpData)
        {
            mFirstJumpData = GetPanelData().JumpData;
            mFirstJumpData.IsBackspacing = true;
        }

        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
        int secondTabData = -1;
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 0)
        {
            firstTabData = jumpData.Tabs[0];

        }
        else
        {
            firstTabData = (int)TabMode.JinBi;
        }
        memActiveFirstTab = TabMode.None;
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
        if (null != jumpData.Tabs && jumpData.Tabs.Length > 1)
        {
            SetActiveTab(jumpData.Tabs[1], true, true);
        }

        if (jumpData.IsBackspacing && null != jumpData.ExtParam && jumpData.ExtParam is int)
        {
            purchaseNum = (int)jumpData.ExtParam;
        }
        if (null != jumpData.Param && jumpData.Param is uint)
        {
            SetSelectItemId((uint)jumpData.Param, true, true, jumpData.IsBackspacing);
        }
        mBSecondJump = (null != mFirstJumpData);
        if (null == mFirstJumpData)
        {
            mFirstJumpData = GetPanelData().JumpData;
            mFirstJumpData.IsBackspacing = true;
        }
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Tabs = new int[2];
        pd.JumpData.Tabs[0] = (int)memActiveFirstTab;
        pd.JumpData.Tabs[1] = mActiveSecondTab;
        pd.JumpData.Param = selectMallItemId;
        pd.JumpData.ExtParam = purchaseNum;
        return pd;
    }

    protected override void OnHide()
    {
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        RegisterGlobalEvent(false);
        ResetCacheLogicReturnJumpData();

        if (null != m_mallItemBaseGrid)
        {
            m_mallItemBaseGrid.Release(false);
        }

        if (null != m_ctor_CategoryTagContent)
        {
            m_ctor_CategoryTagContent.Release(depthRelease);
        }

        if (null != m_ctor_MallScrollView)
        {
            m_ctor_MallScrollView.Release(depthRelease);
        }

        if (null != m_costIconCASD)
        {
            m_costIconCASD.Release(depthRelease);
        }
        if (null != m_ownIconCASD)
        {
            m_ownIconCASD.Release(depthRelease);
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        RegisterGlobalUIEvent(false);
    }


    public override bool ExecuteReturnLogic()
    {
        if (mBSecondJump && null != mFirstJumpData && GetPanelData() != null)
        {
            PanelJumpData cPJ = GetPanelData().JumpData;
            if (cPJ.Tabs != null && cPJ.Tabs.Length > 0 && mFirstJumpData.Tabs != null && mFirstJumpData.Tabs.Length > 0)
            {
                if (cPJ.Tabs[0] != mFirstJumpData.Tabs[0])
                {
                    cPJ = mFirstJumpData;
                    ResetCacheLogicReturnJumpData();
                    OnJump(cPJ);
                    return true;
                }
            }
        }
        return false;
    }

    private void ResetCacheLogicReturnJumpData()
    {
        mBSecondJump = false;
        mFirstJumpData = null;
        selectMallItemId = 0;
        purchaseNum = 0;
    }

    void EventCallBack(int nEventID, object param)
    {


    }
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalEvent(true);
    }
    #endregion

    #region IGlobalEvent
    private void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, GlobalEventHandler);
            
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.PLAYER_LOGIN_SUCCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.RECONNECT_SUCESS, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM, GlobalEventHandler);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventid"></param>
    /// <param name="data"></param>
    public void GlobalEventHandler(int eventid, object data)
    {
        switch (eventid)
        {
            case (int)Client.GameEventID.UIEVENT_REFRESHPURCHASENUM:
                if (IsSelectItemEnable() && CurrentMallData.LocalMall.mallItemId == ((uint)data))
                {
                    //刷新次数
                    UpdateLeftTimes();
                }
                break;
            case (int)Client.GameEventID.RECONNECT_SUCESS:
                {
                    //刷新面板
                }
                break;
            case (int)Client.GameEventID.UIEVENT_REFRESHCURRENCYNUM:
                {
                    UpdateOwer();
                }
                break;
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
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_REFRESHPURCHASENUM, GlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHPURCHASENUM, GlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_TAGSTATUSCHANGED, GlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_REFRESHPURCHASENUM, GlobalUIEventHandler);
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
    public void SetSelectItemId(uint mallItemId, bool force = false, bool needFocus = false, bool isBackSpacing = false)
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
            grid = m_ctor_MallScrollView.GetGrid<UIMallGrid>(m_lst_mallDatas.IndexOf(mallItemId));
            if (null != grid)
            {
                grid.SetHightLight(true);
            }

            if (needFocus)
            {
                m_ctor_MallScrollView.FocusGrid(m_lst_mallDatas.IndexOf(mallItemId));
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

    /// <summary>
    /// 设置活动标签
    /// </summary>
    /// <param name="tabId"></param>
    public void SetActiveTab(int tabId, bool force = false, bool needFocus = false)
    {
        if (tabId == mActiveSecondTab && !force)
            return;

        if (null != m_ctor_CategoryTagContent)
        {
            UITabGrid grid = m_ctor_CategoryTagContent.GetGrid<UITabGrid>(m_lst_mallTabDatas.IndexOf(mActiveSecondTab));
            if (null != grid)
            {
                grid.SetHightLight(false);
            }
            grid = m_ctor_CategoryTagContent.GetGrid<UITabGrid>(m_lst_mallTabDatas.IndexOf(tabId));
            if (null != grid)
            {
                grid.SetHightLight(true);
            }

            if (needFocus)
            {
                m_ctor_CategoryTagContent.FocusGrid(m_lst_mallTabDatas.IndexOf(tabId));
            }
        }
        mActiveSecondTab = tabId;
        //手动设置二级页签
        dicActiveTabGrid[2] = tabId;

        CreateMallUIList();
    }

    public void CreateTab()
    {
        StructTabData();
        if (null != m_ctor_CategoryTagContent)
        {
            mActiveSecondTab = (m_lst_mallTabDatas.Count > 0) ? m_lst_mallTabDatas[0] : 0;
            int count = m_lst_mallTabDatas.Count;
            if (m_lst_mallTabDatas.Count == 1 && m_lst_mallTabDatas[0] == 0)
                count = 0;
            m_ctor_CategoryTagContent.CreateGrids(count, true);
        }
        SetActiveTab(mActiveSecondTab, true);
    }

    private void SetActiveFirstTab(TabMode type, bool force = false)
    {
        if (memActiveFirstTab == type && !force)
        {
            return;
        }
        this.memActiveFirstTab = type;
        this.mActiveSecondTab = 0;
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
            tabGrid.SetName(DataManager.Manager<MallManager>().GetMallTagName(ActiveStore, tabKey));
            tabGrid.SetHightLight(mActiveSecondTab == tabKey ? true : false);
            tabGrid.SetGridData(tabKey);
            tabGrid.SetDepth(startDepth);
        }

    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            SetActiveFirstTab((TabMode)pageid, false);
        }
        return base.OnTogglePanel(tabType, pageid);
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
        m_lst_mallTabDatas.Clear();
        m_lst_mallTabDatas.AddRange(
            DataManager.Manager<MallManager>().GetMallTagDatas((int)ActiveStore));
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
            DataManager.Manager<MallManager>().GetMallDatas((int)ActiveStore, (int)mActiveSecondTab));
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
    private void InitWidgets()
    {
        RegisterGlobalUIEvent(true);
        Transform cloneObj = null;
        if (null != m_trans_MallBaseGridRoot)
        {
            cloneObj = m_trans_MallBaseGridRoot.GetChild(0);
        }
        if (null != cloneObj && null == m_mallItemBaseGrid)
        {
            if (null != cloneObj)
            {
                m_mallItemBaseGrid = cloneObj.GetComponent<UIItemInfoGrid>();
                if (null == m_mallItemBaseGrid)
                {
                    m_mallItemBaseGrid = cloneObj.gameObject.AddComponent<UIItemInfoGrid>();
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

        if (null != m_ctor_CategoryTagContent && null != m_trans_UITabGrid)
        {
            m_ctor_CategoryTagContent.RefreshCheck();
            m_ctor_CategoryTagContent.Initialize<UITabGrid>(m_trans_UITabGrid.gameObject, OnUpdateMallGridData, OnGridUIEventDlg);
        }


        if (null != m_ctor_MallScrollView && null != m_trans_UIMallGrid)
        {
            m_ctor_MallScrollView.Initialize<UIMallGrid>(m_trans_UIMallGrid.gameObject
                                   , OnUpdateMallGridData, OnGridUIEventDlg);
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
        //if (CurrentMallData.IsDayNumLimit 
        //    && CurrentMallData.LeftTimes < purchaseNum)
        //{
        //    purchaseNum = CurrentMallData.LeftTimes;
        //}
        purchaseNum = Mathf.Max(0, Mathf.Min(purchaseNum, CurrentMallData.MaxPurchaseNum));
        if (null != m_label_ExchangeNum)
            m_label_ExchangeNum.text = "" + purchaseNum;
        UpdateCost();
    }

    /// <summary>
    ///  更新价格
    /// </summary>
    private void UpdateCost()
    {
        MallDefine.MallLocalData current = CurrentMallData;
        string costIconName = "";
        uint totalCost = 0;
        if (null != current)
        {
            costIconName = MallDefine.GetCurrencyIconNameByType((GameCmd.MoneyType)current.LocalMall.moneyType);
            totalCost = (uint)((current.IsInDiscount) ? current.LocalMall.offPrice * purchaseNum
                : current.LocalMall.buyPrice * purchaseNum);
        }
        if (null != m_sprite_CostIcon)
        {
            UIManager.GetAtlasAsyn(costIconName,
            ref m_costIconCASD, () =>
            {
                if (null != m_sprite_CostIcon)
                {
                    m_sprite_CostIcon.atlas = null;
                }
            }
            , m_sprite_CostIcon, false);
        }

        if (null != m_label_CostNum)
        {
            m_label_CostNum.text = totalCost.ToString();;
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
                    m_mallItemBaseGrid.SetMuhonMask(enable, Muhon.GetMuhonStarLevel(current.LocalItem.BaseId));
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
        UpdateOwer();
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

    #region 点券

    #endregion

    #region 文钱

    #endregion

    #region UIEvent

    private void UpdateOwer()
    {
        MallDefine.MallLocalData current = CurrentMallData;
        string costIconName = "";
        int ownerNum = 0;
        if (null != current)
        {
            costIconName = MallDefine.GetCurrencyIconNameByType((GameCmd.MoneyType)current.LocalMall.moneyType);
            ownerNum = UIPlayerProprty.GetNum((ClientMoneyType)current.LocalMall.moneyType);
        }
        if (null != m_sprite_CostIcon)
        {
            UIManager.GetAtlasAsyn(costIconName,
            ref m_ownIconCASD, () =>
            {
                if (null != m_sprite_OwnIcon)
                {
                    m_sprite_OwnIcon.atlas = null;
                }
            }
            , m_sprite_OwnIcon, false);
        }

        if (null != m_label_OwnNum)
        {
            m_label_OwnNum.text = TextManager.GetFormatNumText((uint)ownerNum);
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
        DataManager.Manager<MallManager>().PurchaseMallItem(selectMallItemId, (uint)ActiveStore, purchaseNum);
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
        //int maxInputNum = ((CurrentMallData.IsDayNumLimit) ? Mathf.Min(CurrentMallData.LeftTimes, MAX_INPUT_NUM) : MAX_INPUT_NUM);
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