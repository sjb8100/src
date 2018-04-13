/************************************************************************************
 * Copyright (c) 2017  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Forging
 * 创建人：  wenjunhua.zqgame
 * 文件名：  EquipExchangePanel
 * 版本号：  V1.0.0.0
 * 创建时间：8/14/2017 5:26:10 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class EquipExchangePanel : IGlobalEvent
{
    #region property
    //分类数据
    private Category category = null;
    //兑换装备信息
    private UIItemInfoGrid m_exchangeItemBaseGrid = null;
    //选中一级页签ID
    private uint selectfTabID = 0;
    //选中二级页签ID
    private uint selectsTabID = 0;
    //当前选中id
    private uint selectExchangeId = 0;
    //购买数量
    private uint exchangeNum = 1;
    private const uint MAX_RECHANGE_NUM = 10;

    private EquipDefine.LocalExchangeDB SelectExchangeDB
    {
        get
        {
            return DataManager.Manager<EquipManager>().GetExchangeLocalDB(selectExchangeId);
        }
    }
    #endregion

    #region overridemethod
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        category = DataManager.Manager<EquipManager>().ExchangeCategory;
        
        InitFirstTab();
        RegisterGlobalEvent(true);
        UpdateSelectIdExchangeInfo();
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (null != jumpData && null != jumpData.Tabs && jumpData.Tabs.Length >= 1)
        {

            if (null != jumpData.ExtParam && jumpData.ExtParam is uint)
            {
                exchangeNum = (uint)jumpData.ExtParam;
            }
            else
            {
                exchangeNum = 1;
            }

            SetFirstActiveTab((uint)jumpData.Tabs[0], true);
            if (jumpData.Tabs.Length >= 2)
            {
                SetSecondActiveTab((uint)jumpData.Tabs[1], true, true);
            }

            if (null != jumpData.Param && jumpData.Param is uint)
            {
                SetSelectId((uint)jumpData.Param, true, true);
            }
        }
        else
        {
            exchangeNum = 1;
            SetFirstActiveTab(category.ChildCategoryData[0].Id, true);
        }
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData data = base.GetPanelData();
        data.JumpData = new PanelJumpData();
        data.JumpData.Tabs = new int[2];
        data.JumpData.Tabs[0] = (int)selectfTabID;
        data.JumpData.Tabs[1] = (int)selectsTabID;
        data.JumpData.Param = (uint)selectExchangeId;
        data.JumpData.ExtParam = (uint)exchangeNum;
        return data;
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalEvent(false);
        category = null;
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
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
        if (m_exchangeItemBaseGrid != null)
        {
            m_exchangeItemBaseGrid.Release(true);
        }
    }

    #endregion

    #region Init

    private void InitWidgets()
    {
        Transform cloneObj = null;// UIManager.GetObj((uint)GridID.Uiiteminfogrid);
        if (null != m_trans_ItemBaseGridRoot)
        {
            cloneObj = m_trans_ItemBaseGridRoot.GetChild(0);
        }
        if (null != cloneObj && null == m_exchangeItemBaseGrid)
        {
            m_exchangeItemBaseGrid = cloneObj.GetComponent<UIItemInfoGrid>();
            if (null == m_exchangeItemBaseGrid)
            {
                m_exchangeItemBaseGrid = cloneObj.gameObject.AddComponent<UIItemInfoGrid>();
            }
            if (null != m_exchangeItemBaseGrid)
            {
                m_exchangeItemBaseGrid.RegisterUIEventDelegate((eventType, data, param) =>
                {
                    if (eventType == UIEventType.Click)
                    {
                        //if (null != CurrentMallData && null != CurrentMallData.LocalItem)
                        //{
                        //    TipsManager.Instance.ShowItemTips(CurrentMallData.LocalItem);
                        //}
                    }
                });
            }
            m_trans_ItemBaseGridRoot.localScale = new Vector3(0.9f, 0.9f, 0.9f);

        }

        if (null != m_ctor_RightTabRoot && null != m_trans_TogglePanel)
        {
            m_ctor_RightTabRoot.Initialize<UITabGrid>(m_trans_TogglePanel.gameObject
                , (gridBase, index) =>
                    {
                        UITabGrid gGRid = gridBase as UITabGrid;
                        if (null != gGRid)
                        {
                            Category tc = category.ChildCategoryData[index];
                            gGRid.SetGridData(tc.Id);
                            gGRid.SetName(tc.Name);
                            gGRid.SetHightLight((tc.Id == selectfTabID));
                        }
                    }
                , (eventType, data, param) =>
                {
                    if (eventType == UIEventType.Click)
                    {
                        UITabGrid gGRid = data as UITabGrid;
                        SetFirstActiveTab((uint)gGRid.Data);
                    }
                });
        }


        if (null != m_ctor_CategoryTagContent && null != m_trans_UITabGrid)
        {
            m_ctor_CategoryTagContent.Initialize<UITabGrid>(m_trans_UITabGrid.gameObject, OnGridUpdate, OnGridEventDlg);
        }

        if (null != m_ctor_ExchangeScrollView && null != m_trans_UIExchangeGrid)
        {
            m_ctor_ExchangeScrollView.Initialize<UIExchangeGrid>(m_trans_UIExchangeGrid.gameObject, OnGridUpdate, OnGridEventDlg);
        }
    }
    private bool isInitFirstTab = false;
    private void InitFirstTab()
    {
        if (isInitFirstTab)
            return;
        isInitFirstTab = true;
        if (null != m_ctor_RightTabRoot)
        {
            m_ctor_RightTabRoot.CreateGrids(category.ChildCategoryData.Count);
        }

    }
    #endregion


    #region CreatorEvent
    private void OnGridUpdate(UIGridBase gridBase, int index)
    {
        if (gridBase is UITabGrid)
        {
            UITabGrid tGrid = gridBase as UITabGrid;
            Category c = null;
            if (category.TryGetCategory(selectfTabID, out c))
            {
                Category s = c.ChildCategoryData[index];
                tGrid.SetGridData(s.Id);
                tGrid.SetName(s.Name, 3);
                tGrid.SetHightLight(s.Id == selectsTabID);
            }

        }
        else if (gridBase is UIExchangeGrid)
        {
            UIExchangeGrid eGrid = gridBase as UIExchangeGrid;
            Category f = null;
            Category s = null;
            if (category.TryGetCategory(selectfTabID, out f)
                && f.TryGetCategory(selectsTabID, out s))
            {
                eGrid.SetGridData(s.Datas[index]);
                eGrid.SetHightLight(s.Datas[index] == selectExchangeId);
            }
        }
    }

    private void OnGridEventDlg(UIEventType type, object data, object param)
    {
        switch (type)
        {
            case UIEventType.Click:
                {
                    if (data is UITabGrid)
                    {
                        UITabGrid tGrid = data as UITabGrid;
                        SetSecondActiveTab((uint)tGrid.Data);
                    }
                    else if (data is UIExchangeGrid)
                    {
                        UIExchangeGrid tGrid = data as UIExchangeGrid;
                        SetSelectId(tGrid.ExchangedID, needResetChooseNum: true);
                    }
                }
                break;
        }
    }
    #endregion

    #region OP
    /// <summary>
    /// 设置一级活动页签
    /// </summary>
    /// <param name="tabID"></param>
    /// <param name="force"></param>
    private void SetFirstActiveTab(uint tabID, bool force = false)
    {
        if (selectfTabID == tabID && !force)
        {
            return;
        }
        Category next = null;
        if (category.TryGetCategory(tabID, out next))
        {
            Category cur = null;
            int index = 0;
            UITabGrid tabGrid = null;
            if (category.TryGetCategory(selectfTabID, out cur))
            {
                index = category.ChildCategoryData.IndexOf(cur);
                tabGrid = m_ctor_RightTabRoot.GetGrid<UITabGrid>(index);
                if (null != tabGrid)
                {
                    tabGrid.SetHightLight(false);
                }
            }

            index = category.ChildCategoryData.IndexOf(next);

            tabGrid = m_ctor_RightTabRoot.GetGrid<UITabGrid>(index);
            if (null != tabGrid)
            {
                tabGrid.SetHightLight(true);
            }
            selectfTabID = tabID;


            if (next.ChildCategoryData.Count > 0)
            {
                m_ctor_CategoryTagContent.CreateGrids(next.ChildCategoryData.Count);
                SetSecondActiveTab(next.ChildCategoryData[0].Id, force: true);
            }
        }
    }

    /// <summary>
    /// 设置二级活动页签
    /// </summary>
    /// <param name="tabID"></param>
    /// <param name="force"></param>
    /// <param name="focus"></param>
    private void SetSecondActiveTab(uint tabID, bool force = false, bool focus = false)
    {
        if (selectsTabID == tabID && !force)
        {
            return;
        }
        Category f = null;
        if (category.TryGetCategory(selectfTabID, out f))
        {
            Category cur = null;
            Category next = null;
            if (f.TryGetCategory(tabID, out next))
            {
                int index = 0;
                UITabGrid tabGrid = null;
                if (f.TryGetCategory(selectsTabID, out cur))
                {
                    index = f.ChildCategoryData.IndexOf(cur);
                    tabGrid = m_ctor_CategoryTagContent.GetGrid<UITabGrid>(index);
                    if (null != tabGrid)
                    {
                        tabGrid.SetHightLight(false);
                    }
                }

                index = f.ChildCategoryData.IndexOf(next);
                tabGrid = m_ctor_CategoryTagContent.GetGrid<UITabGrid>(index);
                if (null != tabGrid)
                {
                    tabGrid.SetHightLight(true);
                }
                selectsTabID = tabID;
                if (focus)
                {
                    m_ctor_CategoryTagContent.FocusGrid(index);
                }

                m_ctor_ExchangeScrollView.CreateGrids(next.Datas.Count);
                if (next.Datas.Count > 0)
                {
                    SetSelectId(next.Datas[0], force, focus);
                }
            }
        }
    }

    /// <summary>
    /// 设置选中兑换物品ID
    /// </summary>
    /// <param name="selectID"></param>
    /// <param name="force"></param>
    /// <param name="focus"></param>
    private void SetSelectId(uint selectID, bool force = false, bool focus = false, bool needResetChooseNum = false)
    {
        if (selectExchangeId == selectID && !force)
        {
            return;
        }

        Category f = null;
        Category s = null;
        if (category.TryGetCategory(selectfTabID, out f)
            && f.TryGetCategory(selectsTabID, out s))
        {
            if (s.Datas.Contains(selectID))
            {
                UIExchangeGrid grid = null;

                int index = s.Datas.IndexOf(selectExchangeId);
                grid = m_ctor_ExchangeScrollView.GetGrid<UIExchangeGrid>(index);
                if (null != grid)
                {
                    grid.SetHightLight(false);
                }

                index = s.Datas.IndexOf(selectID);
                grid = m_ctor_ExchangeScrollView.GetGrid<UIExchangeGrid>(index);
                if (null != grid)
                {
                    grid.SetHightLight(true);
                }
                selectExchangeId = selectID;
                if (needResetChooseNum)
                {
                    exchangeNum = 1;
                }
                if (focus)
                {
                    m_ctor_ExchangeScrollView.FocusGrid(index);
                }
                UpdateSelectIdExchangeInfo();
            }
        }

    }

    /// <summary>
    /// 更新选中兑换信息
    /// </summary>
    private void UpdateSelectIdExchangeInfo()
    {
        EquipDefine.LocalExchangeDB db = SelectExchangeDB;
        if (null == db)
        {
            return;
        }
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(db.TargetID);

        if (null != m_label_ItemName)
            m_label_ItemName.text = baseItem.Name;
        if (null != m_label_ItemUseLv)
        {
            ColorType color = (baseItem.UseLv > DataManager.Instance.PlayerLv) ? ColorType.JZRY_Txt_NotMatchRed : ColorType.JZRY_Green;
            m_label_ItemUseLv.text = DataManager.Manager<TextManager>()
                .GetLocalFormatText(LocalTextType.Local_TXT_Mall_UselevelDescribe
                , ColorManager.GetNGUIColorOfType(color), baseItem.UseLv);
        }

        if (null != m_label_ItemDes)
            m_label_ItemDes.text = baseItem.DesNoColor;

        if (null != m_exchangeItemBaseGrid)
        {
            m_exchangeItemBaseGrid.Reset();
            m_exchangeItemBaseGrid.SetIcon(true, baseItem.Icon);
            m_exchangeItemBaseGrid.SetBorder(true, baseItem.BorderIcon);
        }

        UpdateCost();
        UpdateOwer();
        UpdateExchangeNum();
        baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(db.CostID);

        UIManager.GetTextureAsyn(baseItem.Icon,
            ref m_costIconCASD, () =>
            {
                if (null != m__CostIcon)
                {
                    m__CostIcon.mainTexture = null;
                }
            }
            , m__CostIcon, false);
        UIManager.GetTextureAsyn(baseItem.Icon,
           ref m_ownIconCASD, () =>
           {
               if (null != m__OwnIcon)
               {
                   m__OwnIcon.mainTexture = null;
               }
           }
           , m__OwnIcon, false);
    }
    CMResAsynSeedData<CMTexture> m_costIconCASD = null;
    CMResAsynSeedData<CMTexture> m_ownIconCASD = null;

    #endregion

    #region IGlobalEvent
    /// <summary>
    /// 注册UI全局事件
    /// </summary>
    /// <param name="register"></param>
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, GlobalEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_LEVELUP, GlobalEventHandler);
        }
    }

    public void GlobalEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                if (null != data && data is ItemDefine.UpdateItemPassData)
                {
                    ItemDefine.UpdateItemPassData pass = data as ItemDefine.UpdateItemPassData;
                    if (null != SelectExchangeDB && SelectExchangeDB.CostID == pass.BaseId)
                    {
                        UpdateOwer();
                    }
                }
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_LEVELUP:
                UpdateExchangeNum();
                break;
        }
    }
    #endregion

    #region UIEvent
    void onClick_ItemGetBtn_Btn(GameObject caster)
    {
        if (null != SelectExchangeDB)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: SelectExchangeDB.CostID);
        }
    }

    void onClick_ExchangeBtn_Btn(GameObject caster)
    {
        if (null != SelectExchangeDB)
        {
            if (exchangeNum > 0)
            {
                int owerNum = DataManager.Manager<ItemManager>().GetItemNumByBaseId(SelectExchangeDB.CostID);
                if (owerNum < (exchangeNum * SelectExchangeDB.CostNum))
                {
                    TipsManager.Instance.ShowTips("消耗道具数量不足");
                }
                else
                {
                    DataManager.Manager<EquipManager>().DoExchange(SelectExchangeDB.ExchangeID, exchangeNum);
                }
            }

        }
    }

    private void OnAddRemove(bool add)
    {
        exchangeNum = (add) ? (exchangeNum + 1) : (exchangeNum - 1);
        UpdateExchangeNum();
    }
    void onClick_BtnAdd_Btn(GameObject caster)
    {
        OnAddRemove(true);
    }

    void onClick_BtnRemove_Btn(GameObject caster)
    {
        OnAddRemove(false);
    }

    private void OnHandInput(int num)
    {
        exchangeNum = (uint)num;
        UpdateExchangeNum();
    }

    void onClick_BtnMax_Btn(GameObject caster)
    {
        OnHandInput((int)MAX_RECHANGE_NUM);
    }

    private void UpdateOwer()
    {
        if (null != m_label_OwnNum && null != SelectExchangeDB)
        {
            m_label_OwnNum.text =
                DataManager.Manager<ItemManager>().GetItemNumByBaseId(SelectExchangeDB.CostID).ToString();
        }
    }

    private void UpdateCost()
    {
        if (null != m_label_CostNum && null != SelectExchangeDB)
        {
            m_label_CostNum.text = (SelectExchangeDB.CostNum * exchangeNum).ToString();
        }
    }

    private void UpdateExchangeNum()
    {
        exchangeNum = (uint)Mathf.Max(1, Mathf.Min(exchangeNum, MAX_RECHANGE_NUM));
        if (null != m_label_ExchangeNum)
            m_label_ExchangeNum.text = exchangeNum.ToString();
        UpdateCost();
    }

    void OnCloseInput()
    {
        if (exchangeNum == 0)
        {
            OnHandInput(1);
        }
    }

    void onClick_HandInputBtn_Btn(GameObject caster)
    {
        UIPanelManager mgr = DataManager.Manager<UIPanelManager>();
        if (mgr.IsShowPanel(PanelID.HandInputPanel))
        {
            mgr.HidePanel(PanelID.HandInputPanel);
        }
        else
        {
            mgr.ShowPanel(PanelID.HandInputPanel, data: new HandInputPanel.HandInputInitData()
            {
                maxInputNum = MAX_RECHANGE_NUM,
                onInputValue = OnHandInput,
                onClose = OnCloseInput,
                showLocalOffsetPosition = new Vector3(308, 113, 0),
            });
        }
    }
    #endregion
}