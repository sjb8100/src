using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ForgingPanel : UIPanelBase
{
    #region define
    /// <summary>
    /// 锻造面板模式
    /// </summary>
    public enum ForgingPanelMode
    {
        None = 0,
        //精炼
        Refine = 1,
        //合成
        Compound = 2,
        //祝福合成
        CompoundZF = 3,
        //加工
        Proccess = 4,
        //镶嵌
        Inlay = 5,
        //强化
        Strengthen = 6,
        Max,
    }

    /// <summary>
    /// 锻造列表模式
    /// </summary>
    public enum ForgingListMode
    {
        Equip = 1,
        Knapsack = 2,
        Max,
    }

    /// <summary>
    /// 精炼面板数据
    /// </summary>
    public class ForgingData
    {
        //当前选中的id
        public uint selectId;
        //当前页签状态
        public ForgingPanelMode m_em_mode = ForgingPanelMode.None;
    }
    #endregion

    #region Property
    //物品管理器
    private ItemManager imgr = null;
    //装备管理器
    private EquipManager emgr = null;
    //文本管理器
    private TextManager m_tmgr = null;
    private TweenPosition tp;
    //锻造模式
    private ForgingPanelMode m_em_panelMode = ForgingPanelMode.None;
    //锻造列表模式
    private ForgingListMode m_em_listMode = ForgingListMode.Equip;
    private uint selectEquipId = 0;
    //已装备
    private List<uint> m_list_equips = new List<uint>();
    //面板模式TabGrid
    private Dictionary<ForgingPanelMode, UITabGrid> m_dic_panelModes = null;
    //内容
    private Dictionary<ForgingPanelMode, Transform> m_dic_Contents = null;
    //列表TabGrid
    private Dictionary<ForgingListMode, UITabGrid> m_dic_listModes = null;
    //组件初始化mask
    private int m_int_modeInitMask;
    /// <summary>
    /// 当前选中装备数据
    /// </summary>
    public Equip Data
    {
        get
        {
            return (null != imgr) ? imgr.GetBaseItemByQwThisId<Equip>(selectEquipId) : null;
        }

    }

    private Transform m_trans_FunctioToggles;

    #endregion

    #region Override Method
    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        m_trans_FunctioToggles = CacheTransform.Find("RightTabs(Clone)");
        RegisterGlobalEvent(true);
        SetEquipFilterStatus(false);
        UpdateApplyRedPoint();
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        int firstTabData = -1;
        int secondTabData = -1;
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        
        if (firstTabData == -1)
        {
            firstTabData = (null != jumpData.Tabs && jumpData.Tabs.Length >= 1) ? jumpData.Tabs[0] : (int)TabMode.QiangHua;
        }

        if (secondTabData == -1 && null != jumpData.Tabs && jumpData.Tabs.Length >= 2)
        {
            secondTabData = jumpData.Tabs[1];
        }
        m_em_panelMode = ForgingPanelMode.None;
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);
        bool isInlay = IsPanelMode(ForgingPanelMode.Inlay) || IsPanelMode(ForgingPanelMode.Strengthen);
        bool success = false;

        if (isInlay)
        {
            GameCmd.EquipPos selectPos = GameCmd.EquipPos.EquipPos_AdornlOne;
            if (null != jumpData.Param)
            {
                if (jumpData.Param is GameCmd.EquipPos)
                {
                    selectPos = (GameCmd.EquipPos)(jumpData.Param);
                }
                m_emEquipPartMode =DataManager.Manager<ForgingManager>().GetEquipModeByEquipPos(selectPos);
            }else
            {
                if (secondTabData != -1)
                {
                    m_emEquipPartMode = (EquipPartMode)secondTabData;
                }
                else
                {
                    m_emEquipPartMode = EquipPartMode.AttackPart;
                }
                selectPos = GetEquipPosByIndex(0);
            }
            SetSelectEquipPartMode(m_emEquipPartMode, true, false);
            SetSelectPart(selectPos, true, true);
        }
        else
        {
            m_em_listMode = ForgingListMode.Equip;
            uint selectId = 0;
            
            if (null != jumpData.Param)
            {
                selectId = (uint)jumpData.Param;
                Equip equip = imgr.GetBaseItemByQwThisId<Equip>((uint)jumpData.Param);
                if (null != equip)
                {
                    m_em_listMode = (equip.IsWear) ? ForgingListMode.Equip : ForgingListMode.Knapsack;
                    success = true;
                }
            }
            if (!success)
            {
                m_em_listMode = GetCanOpenListMode();
                List<uint> equipList = GetEquipsByListMode(m_em_listMode);
                selectId = (null != equipList && equipList.Count > 0) ? equipList[0] : 0;
            }
            SetForgingListMode(m_em_listMode, true, false);
            SetSelectId(((null != jumpData.Param) ? (uint)jumpData.Param : ((m_list_equips.Count > 0) ? m_list_equips[0]:0)), true,true);
            if (jumpData.Tabs.Length >= 3 && jumpData.Tabs[2] != 0)
            {
                m_em_panelMode = (ForgingPanelMode)jumpData.Tabs[2];
                SetPanelMode(m_em_panelMode, true);
                if (null != m_trans_FunctioToggles
                    && m_trans_FunctioToggles.gameObject.activeSelf)
                {
                    m_trans_FunctioToggles.gameObject.SetActive(false);
                }
            }
        }
        UpdateForgingWidgetsVisible();
        frogingReady = true;
        if (null != m_trans_FunctioToggles)
            NGUITools.MarkParentAsChanged(m_trans_FunctioToggles.gameObject);
    }

    public override UIPanelBase.PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        TabMode tabMode = GetCurTableMode();
        pd.JumpData.Tabs = new int[3];
        pd.JumpData.Tabs[0] = (int)tabMode;

        if (tabMode == TabMode.XiangQian || tabMode == TabMode.QiangHua)
        {
            pd.JumpData.Param = m_emSelectInlayPos;
            pd.JumpData.Tabs[1] = (int)m_emEquipPartMode;
        }
        else
        {
            if (IsPanelMode(ForgingPanelMode.CompoundZF))
            {
                pd.JumpData.Tabs[2] = (int)m_em_panelMode;
            }
            //else
            //{
            //    pd.JumpData.Tabs[2] = (int)m_uEquipFilterTypeMask;
            //}
            pd.JumpData.Param = selectEquipId;
            pd.JumpData.Tabs[1] = (int)m_em_listMode;
        }
        return pd;
    }
    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_ctor_EquipScrollView != null)
        {
            m_ctor_EquipScrollView.Release(depthRelease);
        }
        if (m_ctor_EquipFilterScrollView != null)
        {
            m_ctor_EquipFilterScrollView.Release(depthRelease);
        }

        if (null != m_protectCreator)
        {
            m_protectCreator.Release();
        }

        if (null != m_ctor_InlayScrollView)
        {
            m_ctor_InlayScrollView.Release();
        }
    }
    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalEvent(false);
        frogingReady = false;
        ResetFrogingUI();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }
    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            if (Enum.IsDefined(typeof(TabMode), pageid))
            {
                switch(pageid)
                {
                    //case (int)TabMode.Refine:
                    //    SetPanelMode(ForgingPanelMode.Refine, true);
                    //    break;
                    case (int)TabMode.HeCheng:
                        SetPanelMode(ForgingPanelMode.Compound, true);
                        break;
                    case (int)TabMode.JiaGong:
                        SetPanelMode(ForgingPanelMode.Proccess, true);
                        break;
                    case (int)TabMode.XiangQian:
                        SetPanelMode(ForgingPanelMode.Inlay, true);
                        break;
                    case (int)TabMode.QiangHua:
                        SetPanelMode(ForgingPanelMode.Strengthen, true);
                        break;
                }
                if (!IsPanelMode(ForgingPanelMode.CompoundZF))
                {
                    ResetFrogingUI();
                }
                if (!(IsPanelMode(ForgingPanelMode.Inlay) || IsPanelMode(ForgingPanelMode.Strengthen)) && frogingReady)
                {
                    UpdateDataList();
                }
            };
        }
        return base.OnTogglePanel(tabType, pageid);
    }

    public override bool ExecuteReturnLogic()
    {
        if ((IsPanelMode(ForgingPanelMode.Compound)
            || IsPanelMode(ForgingPanelMode.Proccess))
            && SetEquipFilterStatus(false))
        {
            return true;
        }

        if (IsPanelMode(ForgingPanelMode.CompoundZF))
        {
            SetPanelMode(ForgingPanelMode.Compound);
            return true;
        }
        return base.ExecuteReturnLogic();
    }
    #endregion

    private TabMode GetCurTableMode()
    {
        TabMode mode = TabMode.None;
        switch(m_em_panelMode)
        {
            case ForgingPanelMode.Inlay:
                mode = TabMode.XiangQian;
                break;
            case ForgingPanelMode.Strengthen:
                mode = TabMode.QiangHua;
                break;
            case ForgingPanelMode.Compound:
            case ForgingPanelMode.CompoundZF:
                mode = TabMode.HeCheng;
                break;
            case ForgingPanelMode.Proccess:
                mode = TabMode.JiaGong;
                break;
            //case ForgingPanelMode.Refine:
            //    mode = TabMode.Refine;
            //    break;
            
        }
        return mode;
    }

    /// <summary>
    /// 重置可能由于祝福合成导致的UI错乱
    /// </summary>
    private void ResetFrogingUI()
    {
        if (null != m_trans_FunctionConent)
        {
            Vector3 pos = m_trans_FunctionConent.localPosition;
            pos.x = 0;
            
            TweenPosition tp = m_trans_FunctionConent.GetComponent<TweenPosition>();
            if (null != tp)
                tp.ResetToBeginning();

            m_trans_FunctionConent.localPosition = pos;
        }

        if (null != m_trans_FunctioToggles && !m_trans_FunctioToggles.gameObject.activeSelf)
        {
            m_trans_FunctioToggles.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 初始化锻造
    /// </summary>
    private void InitForgingWidgets()
    {
        UITabGrid tabGrid = null;
        Transform ts = null;
        //页签tab
        if (null == m_dic_panelModes)
        {
            m_dic_panelModes = new Dictionary<ForgingPanelMode, UITabGrid>();
        }
        m_dic_panelModes.Clear();
        if (null == m_dic_Contents)
        {
            m_dic_Contents = new Dictionary<ForgingPanelMode, Transform>();
        }
        m_dic_Contents.Clear();
        for (ForgingPanelMode i = ForgingPanelMode.Refine; i < ForgingPanelMode.Max; i++)
        {
            if (null != m_trans_FunctioToggles)
            {
                ts = m_trans_FunctioToggles.Find(i.ToString() + "Toggle");
                if (null != ts)
                {
                    tabGrid = ts.GetComponent<UITabGrid>();
                    if (null == tabGrid)
                    {
                        tabGrid = ts.gameObject.AddComponent<UITabGrid>();
                    }
                    tabGrid.SetGridData(i);
                    tabGrid.RegisterUIEventDelegate(OnUIEventCallback);
                    tabGrid.SetHightLight(false);
                    m_dic_panelModes.Add(i, tabGrid);
                }
            }
            
            if (null != m_trans_FunctionConent)
            {
                if (i != ForgingPanelMode.CompoundZF)
                {
                    ts = m_trans_FunctionConent.Find(i.ToString() + "Content");
                }
                else if (m_dic_Contents.ContainsKey(ForgingPanelMode.Compound))
                {
                    ts = m_dic_Contents[ForgingPanelMode.Compound].Find(i.ToString() + "Content");
                }
                if (null != ts)
                {
                    m_dic_Contents.Add(i, ts);
                }
            }
            
        }

        //列表
        if (null == m_dic_listModes)
        {
            m_dic_listModes = new Dictionary<ForgingListMode, UITabGrid>();
        }
        m_dic_listModes.Clear();
        if (null == m_trans_ListTab)
        {
            Engine.Utility.Log.Error("ForgingPanel->InitForgingWidgets failed Transform ListTab null");
        }
        else
        {
            for (ForgingListMode i = ForgingListMode.Equip; i < ForgingListMode.Max;i++)
            {
                ts = m_trans_ListTab.Find(i.ToString() + "Tab");
                if (null == ts)
                {
                    continue;
                }
                tabGrid = ts.GetComponent<UITabGrid>();
                if (null == tabGrid)
                {
                    tabGrid = ts.gameObject.AddComponent<UITabGrid>();
                }
                tabGrid.SetGridData(i);
                tabGrid.RegisterUIEventDelegate(OnUIEventCallback);
                tabGrid.SetHightLight(false);
                m_dic_listModes.Add(i, tabGrid);
            }
        }
        UpdateForgingToggleVisible();
    }

    public void InitWidgets()
    {
        imgr = DataManager.Manager<ItemManager>();
        emgr = DataManager.Manager<EquipManager>();
        m_tmgr = DataManager.Manager<TextManager>();

        InitEquipPosWidgets();
        //初始化列表生成器
        if (null != m_ctor_EquipScrollView && null != m_trans_UIEquipInfoGrid)
        {
            m_ctor_EquipScrollView.RefreshCheck();
            m_ctor_EquipScrollView.Initialize<UIEquipInfoGrid>(m_trans_UIEquipInfoGrid.gameObject, OnUpdateGridData, OnUIEventCallback);
        }
        if (null != m_trans_FunctionConent)
        {
            TweenPosition tp = m_trans_FunctionConent.GetComponent<TweenPosition>();
            if (null != tp)
            {
                tp.method = UITweener.Method.EaseIn;
                tp.AddOnFinished(() =>
                    {
                        if (IsPanelMode(ForgingPanelMode.Compound))
                        {
                            if (null != m_trans_CompoundZFContent
                                && m_trans_CompoundZFContent.gameObject.activeSelf)
                            {
                                m_trans_CompoundZFContent.gameObject.SetActive(false);
                            }
                            if (null != m_trans_FunctioToggles
                                && !m_trans_FunctioToggles.gameObject.activeSelf)
                            {
                                m_trans_FunctioToggles.gameObject.SetActive(true);
                            }
                            if (null != m_ctor_EquipScrollView)
                            {
                                m_ctor_EquipScrollView.GetComponent<UIPanel>().SetDirty();
                            }
                        }else if (IsPanelMode(ForgingPanelMode.CompoundZF) 
                            && IsInitStatus(ForgingPanelMode.CompoundZF))
                        {
                            //UpdateCompoundZF(Data);
                            m_protectCreator.SetDirty();
                        }
                    });
            }
        }
        //初始化页签
        InitForgingWidgets();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void InitStatusdWidgets()
    {
        switch (m_em_panelMode)
        {
            case ForgingPanelMode.Refine:
                InitRefineWidgets();
                break;
            case ForgingPanelMode.Compound:
                InitCompoundWidgets();
                break;
            case ForgingPanelMode.CompoundZF:
                InitCompoundWidgets();
                InitCompoundZFWidgets();
                break;
            case ForgingPanelMode.Proccess:
                InitProccessWidgets();
                break;
            case ForgingPanelMode.Inlay:
                InitInlayWidgets();
                break;
            case ForgingPanelMode.Strengthen:
                InitStrengthenWidgets();
                break;
        }
    }

    /// <summary>
    /// 是否初始化当前模式
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public bool IsInitStatus(ForgingPanelMode status)
    {
        return (m_int_modeInitMask & (1 << (int)status)) != 0;
    }

    /// <summary>
    /// 设置初始化状态
    /// </summary>
    /// <param name="status"></param>
    /// <param name="init"></param>
    public void SetInitStatus(ForgingPanelMode status, bool init)
    {
        if (init)
        {
            m_int_modeInitMask |= ((1<< (int)status));
        }
        else
        {
            m_int_modeInitMask &= (~((1<< (int)status)));
        }

    }

    /// <summary>
    /// 检测列表模式是否正确
    /// </summary>
    private void CheckForgingModeList()
    {
        bool isEquipEmp = (GetEquipsByListMode(m_em_listMode).Count == 0);
        ForgingListMode nextmode = m_em_listMode;
        if (nextmode == ForgingListMode.Equip)
        {
            if (isEquipEmp)
            {
                isEquipEmp = (GetEquipsByListMode(ForgingListMode.Knapsack).Count == 0);
                if (!isEquipEmp)
                {
                    nextmode = ForgingListMode.Knapsack;
                }
            }
            
        }else
        {
            if (isEquipEmp)
            {
                nextmode = ForgingListMode.Equip;
            }
        }
        SetForgingListMode(nextmode, true);
        UpdateForgingWidgetsVisible();
    }

    /// <summary>
    /// 根据锻造列表模式获取装备列表
    /// </summary>
    /// <param name="mode"></param>
    private List<uint> GetEquipsByListMode(ForgingListMode mode)
    {
        List<GameCmd.EquipType> filterEquipType = new List<GameCmd.EquipType>();
        filterEquipType.Add(GameCmd.EquipType.EquipType_SoulOne);
        filterEquipType.Add(GameCmd.EquipType.EquipType_Office);
        
        if (mode == ForgingListMode.Knapsack && !EquipFilterEnable() 
            || mode == ForgingListMode.Equip)
        {
            GameCmd.PACKAGETYPE pType = GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP;
            pType = GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN;
            return emgr.GetEquips(((mode == ForgingListMode.Equip) ?
                GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP : GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN)
                , filterEquipType);
        }else
        {
            return emgr.GetMatchEquipInKnapsack(m_uEquipFilterTypeMask);
        }
    }

    /// <summary>
    /// 有物品变化广播
    /// </summary>
    /// <param name="data">物品id</param>
    public void OnGlobalUIEventHandler(int eventType,object data)
    {
        switch(eventType)
        {
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                OnItemDataChangeed((ItemDefine.UpdateItemPassData)data);
                break;
            case (int)Client.GameEventID.UIEVENTEQUIPCOMPOUNDCOMPLETE:
                OnEquipCompoundComplete((uint)data);
                break;
            case (int)Client.GameEventID.UIEVENTEQUIPROPERTYPREMOVE:
                OnRemoveComplete();
                break;
            case (int)Client.GameEventID.UIEVENTEQUIPROPERTYPPROMOTE:
                OnPromoteComplete((bool)data);
                break;
            case (int)Client.GameEventID.UIEVENTRUNESTONEACTIVE:
                OnFetchComplete((uint)data);
                break;
            case (int)Client.GameEventID.UIEVENTGEMINLAYCHANGED:
                {
                    UpdateInlayPartList();
                    UpdateInlayInfo();
                    UpdateApplyRedPoint();
                    UpdateTypeGrid();
                }
                break;
            case (int)Client.GameEventID.UIEVENTEQUIPREFINECOMPLETE:
                if (IsPanelMode(ForgingPanelMode.Refine))
                {
                    OnRefineComplete((uint)data);
                }
                break;

            //装备格强化属性变更
            case (int)Client.GameEventID.UIEVENT_GRIDSTRENGTHENLVCHANGED:
                   UpdateApplyRedPoint();
                break;
            //强化套装属性变更
            case (int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED:
                {
                    HandlerStrengthenChanged();                   
                }
                break;
        }

    }

    /// <summary>
    /// 处理镶嵌数据变化
    /// </summary>
    /// <param name="qwThisId"></param>
    private void HandlerInlayDataChanged(ItemDefine.UpdateItemPassData passData)
    {
        if (!IsPanelMode(ForgingPanelMode.Inlay) || null == passData)
        {
            return;
        }
        OnInlayItemUpdate(passData);
    }

    /// <summary>
    /// 处理锻造数据边
    /// </summary>
    /// <param name="qwThis"></param>
    private void HandlerForgingDataChaned(ItemDefine.UpdateItemPassData passData)
    {
        if (IsPanelMode(ForgingPanelMode.Inlay)
            ||IsPanelMode(ForgingPanelMode.Strengthen)
            || null == passData)
        {
            return;
        }
        bool needRefreshUI = false;
        bool needFit = false;
        BaseItem m_tempBaseItem = imgr.GetTempBaseItemByBaseID<BaseItem>(passData.BaseId);
        BaseItem itemData = null;
        if (m_tempBaseItem.IsForgingEquip)
        {
            switch(passData.UpdateType)
            {
                case ItemDefine.UpdateItemType.Add:
                    {
                        itemData = imgr.GetBaseItemByQwThisId(passData.QWThisId);
                        if ((itemData.PackType == GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN
                            && IsForgingListMode(ForgingListMode.Knapsack))
                            || itemData.PackType == GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP
                            && IsForgingListMode(ForgingListMode.Equip))
                        {
                            //m_list_equips.Add(passData.QWThisId);
                            //ItemManager.SortItemListBySortId(ref m_list_equips);
                            m_list_equips.Clear();
                            m_list_equips.AddRange(GetEquipsByListMode(m_em_listMode));
                            m_ctor_EquipScrollView.InsertData(m_list_equips.IndexOf(passData.QWThisId));
                            if (m_list_equips.Count == 1)
                            {
                                SetSelectId(passData.QWThisId, true);
                                m_ctor_EquipScrollView.ResetPosition();
                            }
                            
                        }
                        else
                        {
                            CheckForgingModeList();
                        }
                        needFit = true;
                        
                    }
                    break;
                case ItemDefine.UpdateItemType.Remove:
                    {
                        //道具删除
                        if (m_list_equips.Contains(passData.QWThisId))
                        {
                            int index = m_list_equips.IndexOf(passData.QWThisId);

                            m_list_equips.Remove(passData.QWThisId);
                            m_ctor_EquipScrollView.RemoveData(index);
                            if (m_list_equips.Count == 0)
                            {
                                selectEquipId = 0;
                                CheckForgingModeList();
                            }
                            else if (selectEquipId == passData.QWThisId)
                            {
                                SetSelectId(m_list_equips[0], true);
                            }
                            needFit = true;
                        }
                        
                    }
                    break;
                case ItemDefine.UpdateItemType.Update:
                    {
                        if (m_list_equips.Contains(passData.QWThisId))
                        {
                            itemData = imgr.GetBaseItemByQwThisId(passData.QWThisId);
                            int index = m_list_equips.IndexOf(passData.QWThisId);
                            m_ctor_EquipScrollView.UpdateData(index);
                            needRefreshUI = true;
                        }
                    }
                    break;
            }
        }
        
        if (needRefreshUI 
            || m_tempBaseItem.BaseType == GameCmd.ItemBaseType.ItemBaseType_Material)
        {
            UpdateDataUIByStatus();
        }

        if (needFit)
        {
            m_ctor_EquipScrollView.FitCreator();
        }

    }

    /// <summary>
    /// 物品数据变化响应
    /// </summary>
    /// <param name="qwThisId"></param>
    private void OnItemDataChangeed(ItemDefine.UpdateItemPassData passData)
    {
        if (null == passData)
        {
            return;
        }

        if (IsPanelMode(ForgingPanelMode.Strengthen))
        {
            HandlerStrengthenChanged();
        }else if (IsPanelMode(ForgingPanelMode.Inlay))
        {
            //镶嵌数据更新
            HandlerInlayDataChanged(passData);
        }else
        {
            //锻造数据更新
            HandlerForgingDataChaned(passData);
        }
    }

    private void HandlerStrengthenChanged()
    {
        if (IsPanelMode(ForgingPanelMode.Strengthen))
        {
            if (null != m_ctor_InlayScrollView)
            {
                m_ctor_InlayScrollView.UpdateActiveGridData();
            }
            UpdateStrengthen();
          
        }
    }

    

    /// <summary>
    /// 注册UI全局事件
    /// </summary>
    /// <param name="register"></param>
    public void RegisterGlobalEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTEQUIPCOMPOUNDCOMPLETE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTEQUIPROPERTYPPROMOTE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTEQUIPROPERTYPREMOVE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTRUNESTONEACTIVE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGEMINLAYCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTEQUIPREFINECOMPLETE, OnGlobalUIEventHandler);

            //装备格强化
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_GRIDSTRENGTHENLVCHANGED, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTEQUIPCOMPOUNDCOMPLETE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTEQUIPROPERTYPPROMOTE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTEQUIPROPERTYPREMOVE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTRUNESTONEACTIVE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGEMINLAYCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTEQUIPREFINECOMPLETE, OnGlobalUIEventHandler);

            //装备格强化
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_GRIDSTRENGTHENLVCHANGED, OnGlobalUIEventHandler);
        }
    }

    public void CreateEquipUIList(bool defaultSelectData = true)    
    {
        if (null == m_list_equips)
        {
            m_list_equips = new List<uint>();
        }
        m_list_equips.Clear();
        m_list_equips.AddRange(GetEquipsByListMode(m_em_listMode));
        m_ctor_EquipScrollView.CreateGrids(m_list_equips.Count);

        if (defaultSelectData)
        {
            uint nextId = selectEquipId;
            if (m_list_equips.Count > 0
                && !m_list_equips.Contains(nextId))
            {
                nextId = m_list_equips[0];
            }
            else if (nextId != 0 && !m_list_equips.Contains(nextId))
            {
                nextId = 0;
            }
            SetSelectId(nextId, true);
        }
    }

    /// <summary>
    /// 设置选中id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="force">是否强制更新</param>
    public void SetSelectId(uint id, bool force = false,bool needFocus = false)
    {
        if (null == m_ctor_EquipScrollView || (id == selectEquipId && !force))
            return;
        //刷新数据
        UIEquipInfoGrid infoGrid = null;
        if (m_list_equips.Contains(selectEquipId))
        {
            infoGrid = m_ctor_EquipScrollView.GetGrid<UIEquipInfoGrid>(m_list_equips.IndexOf(selectEquipId));
            if (null != infoGrid)
            {
                infoGrid.SetHightLight(false);
            }
                
        }
        selectEquipId = id;
        if (m_list_equips.Contains(id))
        {
            infoGrid = m_ctor_EquipScrollView.GetGrid<UIEquipInfoGrid>(m_list_equips.IndexOf(id));
            if (null != infoGrid)
            {
                infoGrid.SetHightLight(true);
            }
        }
        if (needFocus)
        {
            m_ctor_EquipScrollView.FocusGrid(m_list_equips.IndexOf(id));
        }
        
        ResetModulesStatus();
        //刷新UI
        UpdateDataUIByStatus();
    }

    /// <summary>
    /// 重置各个功能模块状态
    /// </summary>
    public void ResetModulesStatus()
    {
        ResetRefine();
        ResetCompound();
        ResetProccess();
    }
    bool frogingReady = false;
    /// <summary>
    /// 设置页签状态
    /// </summary>
    /// <param name="status">页签状态</param>
    /// <param name="force">是否强制更新</param>
    public void SetPanelMode(ForgingPanelMode mode,bool force = false)
    {
        if (this.m_em_panelMode == mode && !force)
            return;
       
        UITabGrid grid = null;
        bool includeZf = true;
        //if (mode != ForgingPanelMode.CompoundZF)
        //{
        //    ResetFrogingUI();
        //}
        if (IsPanelMode(ForgingPanelMode.Compound) && mode == ForgingPanelMode.CompoundZF)
        {
            if (null != m_trans_CompoundZFContent && !m_trans_CompoundZFContent.gameObject.activeSelf)
            {
                m_trans_CompoundZFContent.gameObject.SetActive(true);
            }
            if (null != m_trans_FunctioToggles && m_trans_FunctioToggles.gameObject.activeSelf)
            {
                m_trans_FunctioToggles.gameObject.SetActive(false);
            }
            //InitCompoundZFWidgets();
            //UpdateCompoundZF(Data);
            DoZFAnim(true);
            
        }
        else if (IsPanelMode(ForgingPanelMode.CompoundZF) && mode == ForgingPanelMode.Compound)
        {
            DoZFAnim(false);
            includeZf = false;
        }
        if (null != m_dic_panelModes && m_dic_panelModes.TryGetValue(m_em_panelMode, out grid))
        {
            grid.SetHightLight(false);
        }
        ForgingPanelMode preMode = m_em_panelMode;
        this.m_em_panelMode = mode;
        if (null != m_dic_panelModes && m_dic_panelModes.TryGetValue(m_em_panelMode, out grid))
        {
            grid.SetHightLight(true);
        }

        UpdateForgingWidgetsVisible(includeZf);
        InitStatusdWidgets();
        if (IsPanelMode(ForgingPanelMode.Inlay)
            || IsPanelMode(ForgingPanelMode.Strengthen))
        {
            SetSelectEquipPartMode(EquipPartMode.AttackPart, true);
        }
        else if (!IsPanelMode(ForgingPanelMode.Strengthen)
            && (preMode == ForgingPanelMode.Inlay || preMode == ForgingPanelMode.Strengthen))
        {
            CheckForgingModeList();
        }
        
        UpdateDataUIByStatus();
        RefreshAllTab();
        ResetModulesStatus();
    }

    /// <summary>
    /// 播放祝福动画
    /// </summary>
    /// <param name="forward"></param>
    private void DoZFAnim(bool forward)
    {
        if (null != m_trans_FunctionConent)
        {
            TweenPosition tp = m_trans_FunctionConent.GetComponent<TweenPosition>();
            if (null != tp)
            {
                if (forward)
                {
                    tp.PlayForward();
                }
                else
                {
                    tp.PlayReverse();
                }
            }
        }
        
    }

    /// <summary>
    /// 更新功能toggle可视状态
    /// </summary>
    private void UpdateForgingToggleVisible()
    {
        UITabGrid tabGrid = null;
        bool visible = false;
        for (ForgingPanelMode i = ForgingPanelMode.Refine; i < ForgingPanelMode.Max; i++)
        {
            visible = IsPanelMode(i);
            if (i == ForgingPanelMode.Compound)
            {
                visible = visible || IsPanelMode(ForgingPanelMode.Compound);
            }
            if (null != m_dic_panelModes && m_dic_panelModes.TryGetValue(i, out tabGrid))
            {
                tabGrid.SetHightLight(visible);
            }
        }
    }

    /// <summary>
    /// 当前是否有装备
    /// </summary>
    /// <returns></returns>
    public bool HasEquips()
    {
        return emgr.GetForgingEquips().Count > 0;
    }

    /// <summary>
    /// 更新组件显示
    /// </summary>
    private void UpdateForgingWidgetsVisible(bool includeZf = true)
    {
        bool visible = false;
        bool hasEquip = HasEquips();
        UITabGrid tabGrid = null;
        for(ForgingPanelMode i = ForgingPanelMode.Refine;i < ForgingPanelMode.Max;i++)
        {
            if (i == ForgingPanelMode.CompoundZF && !includeZf)
                continue;
            visible = IsPanelMode(i);

            if (null != m_dic_panelModes && m_dic_panelModes.TryGetValue(i, out tabGrid))
            {
                tabGrid.SetHightLight(visible);
            }

            if (i == ForgingPanelMode.Compound)
            {
                visible = visible || IsPanelMode(ForgingPanelMode.CompoundZF);
            }
            if (i != ForgingPanelMode.Inlay 
                && i != ForgingPanelMode.Strengthen)
            {
                visible &= hasEquip;
            }
            if (i == ForgingPanelMode.Inlay
                && IsSelectInlayGridIndexType(EquipManager.EquipGridIndexType.None)
                && emgr.IsUnlockEquipGridIndex(EquipManager.EquipGridIndexType.First))
            {
                SelectInlayGridIndexType(EquipManager.EquipGridIndexType.First);
            }
            if (m_dic_Contents.ContainsKey(i) && m_dic_Contents[i].gameObject.activeSelf != visible)
            {
                m_dic_Contents[i].gameObject.SetActive(visible);
            }
        }
        //visible = !IsPanelMode(ForgingPanelMode.CompoundZF);
        //if (null != m_trans_FunctioToggles && m_trans_FunctioToggles.gameObject.activeSelf != visible)
        //{
        //    m_trans_FunctioToggles.gameObject.SetActive(visible);
        //}
        visible = IsPanelMode(ForgingPanelMode.Inlay) || IsPanelMode(ForgingPanelMode.Strengthen);
        if (null != m_trans_ScorllAreaInlay && m_trans_ScorllAreaInlay.gameObject.activeSelf != visible)
        {
            m_trans_ScorllAreaInlay.gameObject.SetActive(visible);
        }

        visible = !visible;
        if (null != m_trans_ScorllAreaEquip && m_trans_ScorllAreaEquip.gameObject.activeSelf != visible)
        {
            m_trans_ScorllAreaEquip.gameObject.SetActive(visible);
        }
        visible = !hasEquip && !(IsPanelMode(ForgingPanelMode.Inlay) || IsPanelMode(ForgingPanelMode.Strengthen));
        if (null != m_trans_NullEquipTipsContent
            && m_trans_NullEquipTipsContent.gameObject.activeSelf != visible)
        {
            m_trans_NullEquipTipsContent.gameObject.SetActive(visible);
        }
    }

    /// <summary>
    /// 是否当前模式为mode
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public bool IsPanelMode(ForgingPanelMode mode)
    {
        return (m_em_panelMode == mode);
    }

    /// <summary>
    /// 获取当前可选中的列表模式（默认已装备装备列表）
    /// </summary>
    /// <returns></returns>
    public ForgingListMode GetCanOpenListMode()
    {
        ForgingListMode listmode = ForgingListMode.Equip;
        bool isEquipEmp = (GetEquipsByListMode(listmode).Count == 0);
        if (isEquipEmp)
        {
            isEquipEmp = (GetEquipsByListMode(ForgingListMode.Knapsack).Count == 0);
            if (!isEquipEmp)
            {
                listmode = ForgingListMode.Knapsack;
            }
        }
        return listmode;
    }

    /// <summary>
    /// 设置锻造列表模式
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="force"></param>
    public void SetForgingListMode(ForgingListMode mode,bool force = false,bool defaltSelectData = true)
    {
        if (this.m_em_listMode == mode && !force)
            return;

        UITabGrid grid = null;
        if (null != m_dic_listModes && m_dic_listModes.TryGetValue(m_em_listMode, out grid))
        {
            grid.SetHightLight(false);
        }
        this.m_em_listMode = mode;
        if (null != m_dic_panelModes && m_dic_listModes.TryGetValue(m_em_listMode, out grid))
        {
            grid.SetHightLight(true);
        }
        //切换到装备模式重置过滤
        if (IsForgingListMode(ForgingListMode.Equip))
        {
            ResetEquipFilter();
        }
        CreateEquipUIList(defaltSelectData);
    }

    /// <summary>
    /// 是否当前模式为mode
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    public bool IsForgingListMode(ForgingListMode mode)
    {
        return (m_em_listMode == mode);
    }

    /// <summary>
    /// 根据页签状态刷新UI
    /// </summary>
    public void UpdateDataUIByStatus()
    {
        //if (selectEquipId == 0 || !imgr.ExistItem(selectEquipId))
        //    return;
        Equip data = imgr.GetBaseItemByQwThisId<Equip>(selectEquipId);
        switch (m_em_panelMode)
        {
            case ForgingPanelMode.Refine:
                UpdateRefine(data);
                break;
            case ForgingPanelMode.Compound:
                UpdateCompound(data);
                break;
            case ForgingPanelMode.CompoundZF:
                UpdateCompound(data);
                UpdateCompoundZF(data);
                break;
            case ForgingPanelMode.Proccess:
                UpdateProccess(data);
                break;
            case  ForgingPanelMode.Inlay:
                UpdateInlayInfo();
                break;
            case ForgingPanelMode.Strengthen:
                UpdateStrengthen();
                break;
        }
    }

    /// <summary>
    /// 更新装备列表数据
    /// </summary>
    private void UpdateDataList()
    {
        if (null != m_ctor_EquipScrollView)
        {
            m_ctor_EquipScrollView.UpdateActiveGridData();
        }
    }

    #region 精炼
    //ForgingPanel_Refine.cs
    #endregion

    #region 合成
    //ForgingPanel_Compound.cs
    #endregion

    #region 加工
    //ForgingPanel_Proccess.cs
    #endregion

    #region 消除
    //ForgingPanel_消除.cs
    #endregion

    #region 装备过滤(EquipFilter)
    private uint m_uEquipFilterTypeMask = 0;
    private List<uint> m_lstFilsterEquipType = null;
    private bool m_bInitFilster = false;
    /// <summary>
    /// 设置装备过滤界面状态
    /// </summary>
    /// <param name="enable"></param>
    /// <returns></returns>
    private bool SetEquipFilterStatus(bool enable)
    {
        if (enable && !CanSelectEquipFilterType(m_uEquipFilterTypeMask))
        {
            return false;
        }
        bool succeess = false;
        if (null != m_trans_EquipFilter && m_trans_EquipFilter.gameObject.activeSelf != enable)
        {
            m_trans_EquipFilter.gameObject.SetActive(enable);
            succeess = true;
        }

        if (null != m_sprite_EquipFilterArrow)
        {
            Vector3 localrotate = m_sprite_EquipFilterArrow.transform.localEulerAngles;
            localrotate.z = (enable) ? 0 : 90;
            m_sprite_EquipFilterArrow.transform.localEulerAngles = localrotate;
        }

        if (enable)
        {
            if (null != m_ctor_EquipFilterScrollView && null != m_trans_UIEquipFilterGrid)
            {
                if (!m_bInitFilster)
                {
                    m_ctor_EquipFilterScrollView.RefreshCheck();
                    m_ctor_EquipFilterScrollView.Initialize<UIEquipFilterGrid>(m_trans_UIEquipFilterGrid.gameObject, OnUpdateGridData, OnUIEventCallback);
                    m_lstFilsterEquipType = new List<uint>();
                    m_lstFilsterEquipType.Add(0);
                    for (GameCmd.EquipType i = GameCmd.EquipType.EquipType_None + 1; i < GameCmd.EquipType.EquipType_Max; i++)
                    {
                        if (!Enum.IsDefined(typeof(GameCmd.EquipType), i)
                            || i == GameCmd.EquipType.EquipType_SoulOne
                            || i == GameCmd.EquipType.EquipType_Office
                            || i == GameCmd.EquipType.EquipType_Capes)
                            continue;
                        m_lstFilsterEquipType.Add((uint)i);
                    }

                    if (null != m_sprite_EquipFilterBg)
                    {
                        UIEventListener.Get(m_sprite_EquipFilterBg.gameObject).onClick = (obj) =>
                        {
                            SetEquipFilterStatus(false);
                        };
                    }
                    m_ctor_EquipFilterScrollView.CreateGrids(m_lstFilsterEquipType.Count);
                }
                
            }
        }

        return succeess;
    }

    /// <summary>
    /// 设置装备过滤
    /// </summary>
    /// <param name="mask"></param>
    /// <returns></returns>
    private void SetEquipFilterType(uint mask)
    {
        if (!CanSelectEquipFilterType(mask))
        {
            TipsManager.Instance.ShowTips("背包中无该部位装备");
            return;
        }

        m_dic_listModes[ForgingListMode.Knapsack].SetName(GetEquipFilterNameByTypeMask(mask));
        m_uEquipFilterTypeMask = mask;
        SetEquipFilterStatus(false);
        selectEquipId = 0;
        CreateEquipUIList();
    }

    /// <summary>
    /// 重置过滤状态
    /// </summary>
    private void ResetEquipFilter()
    {
        m_uEquipFilterTypeMask = 0;
        m_dic_listModes[ForgingListMode.Knapsack].SetName(GetEquipFilterNameByTypeMask(m_uEquipFilterTypeMask));
    }
    /// <summary>
    /// 背包中装备过滤是否可用
    /// </summary>
    /// <returns></returns>
    private bool EquipFilterEnable()
    {
        return m_uEquipFilterTypeMask != 0;
    }

    /// <summary>
    /// 是否可以选中过滤部位
    /// </summary>
    /// <param name="mask"></param>
    /// <returns></returns>
    private bool CanSelectEquipFilterType(uint mask)
    {
        List<uint> result = null;
        if (mask == 0)
        {
            List<GameCmd.EquipType> filterEquipType = new List<GameCmd.EquipType>();
            filterEquipType.Add(GameCmd.EquipType.EquipType_SoulOne);
            filterEquipType.Add(GameCmd.EquipType.EquipType_Office);
            result = emgr.GetEquips(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN, filterEquipType);
        }else
        {
           result = emgr.GetMatchEquipInKnapsack(mask);
        }
        return (null != result && result.Count > 0);
    }

    /// <summary>
    /// 获取过滤名称
    /// </summary>
    /// <param name="mask"></param>
    /// <returns></returns>
    private string GetEquipFilterNameByTypeMask(uint mask)
    {
        string name = "背包.全部";
        if (mask != 0)
        {
            name = string.Format("背包.{0}"
                , EquipDefine.GetEquipTypeName((GameCmd.EquipType)mask));
        }
        
        return name;
    }

    #endregion

    #region UIEvent callback
    public void onClick_Toggle_Btn(GameObject obj)
    {

    }
    private void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (null == grid)
            return;
        if (grid is UIEquipInfoGrid)
        {
            UIEquipInfoGrid egrid = grid as UIEquipInfoGrid;
            if (null == m_list_equips || index >= m_list_equips.Count)
            {
                return;
            }
            uint currentId = m_list_equips[index];
            Equip iData = imgr.GetBaseItemByQwThisId<Equip>(currentId);
            if (null == iData)
                return;
            bool growMaskEnable = false;
            bool condiEnable = false;
            switch(m_em_panelMode)
            {
                case ForgingPanelMode.Refine:
                    growMaskEnable = iData.CanRefine;
                    condiEnable = growMaskEnable;
                    break;
                case ForgingPanelMode.Compound:
                case ForgingPanelMode.CompoundZF:
                    growMaskEnable = iData.CompoundMaskEnable;
                    condiEnable = iData.AdditionAttrCount > 0;
                    break;
                case ForgingPanelMode.Proccess:
                    growMaskEnable = iData.CanProccess;
                    condiEnable = growMaskEnable;
                    break;
            }
            egrid.SetGridViewData(iData.QWThisID, (currentId == selectEquipId), growMaskEnable,condiEnable);
        }else if (grid is UIEquipPropertyProtectGrid)
        {
            if (null != m_lst_equipAtts && m_lst_equipAtts.Count > index)
            {
                GameCmd.PairNumber attr = m_lst_equipAtts[index];
                UIEquipPropertyProtectGrid epGrid = grid as UIEquipPropertyProtectGrid;
                
                bool select = m_lst_checkProtectAttr.Contains(attr.id);
                int fillNum = 0;
                int needNum = 0;
                if (select)
                {
                    ProtectAttrData pAD = GetCompoundZfProtectAttrData(attr.id);
                    if (null != pAD)
                    {
                        fillNum = pAD.FillNum;
                        needNum = pAD.NeedNum;
                    }
                }
                
                epGrid.SetVisible(true);
                epGrid.SetGridView(attr, select, fillNum
                    , needNum, callback: OnCompoundProtectStatusChanged);
            }
        }
        else if (grid is UIEquipFilterGrid)
        {
            UIEquipFilterGrid efGrid = grid as UIEquipFilterGrid;
            uint mask = m_lstFilsterEquipType[index];
            efGrid.SetData(GetEquipFilterNameByTypeMask(mask),mask, m_uEquipFilterTypeMask == mask);
        }
    }

    private void OnUIEventCallback(UIEventType eventType, object data, object parms)
    {
        if (null == data)
        {
            return ;
        }
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIEquipInfoGrid)
                    {
                        UIEquipInfoGrid egrid = data as UIEquipInfoGrid;
                        SetSelectId(egrid.QWThisId);
                    }
                    else if (data is UITabGrid)
                    {
                        UITabGrid tgrid = data as UITabGrid;
                        if (null != tgrid.Data)
                        {
                            if (tgrid.Data is ForgingPanelMode)
                            {
                                ForgingPanelMode nextMode = (ForgingPanelMode)tgrid.Data;
                                if (!HasEquips() && nextMode != ForgingPanelMode.Inlay)
                                {
                                    TipsManager.Instance.ShowTips("您还未拥有装备");
                                }
                                //面板页签
                                SetPanelMode(nextMode);
                                if (!IsPanelMode(ForgingPanelMode.Inlay))
                                {
                                    UpdateDataList();
                                }
                            }
                            else if (tgrid.Data is ForgingListMode)
                            {
                                //装备列表模式
                                bool isEmpty = GetEquipsByListMode((ForgingListMode)tgrid.Data).Count == 0;
                                if (isEmpty)
                                {
                                    TipsManager.Instance.ShowTips("该分页无装备");
                                }
                                else
                                {
                                    ForgingListMode fMode = (ForgingListMode)tgrid.Data;
                                    if (fMode == ForgingListMode.Knapsack 
                                        && IsForgingListMode(ForgingListMode.Knapsack))
                                    {
                                        SetEquipFilterStatus(true);
                                    }
                                    SetForgingListMode((ForgingListMode)tgrid.Data);
                                }
                            }
                            else if (tgrid.Data is ForgingProccessMode)
                            {
                                //加工模式
                                SetProccessMode((ForgingProccessMode)tgrid.Data);
                            }
                            else if (tgrid.Data is EquipPartMode)
                            {
                                //装备部位
                                SetSelectEquipPartMode((EquipPartMode)tgrid.Data);
                            }
                        }
                    }
                    else if (data is UIItemInfoGrid)
                    {
                        UIItemInfoGrid itemInfoGrid = data as UIItemInfoGrid;
                        if (itemInfoGrid.NotEnough && null != parms && parms is uint)
                        {
                            ShowItemGet((uint)parms);
                        }
                    }else if (data is UIEquipFilterGrid)
                    {
                        UIEquipFilterGrid equipFilterGrid = data as UIEquipFilterGrid;
                        SetEquipFilterType(equipFilterGrid.EquipType);
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// 显示获取
    /// </summary>
    /// <param name="baseId"></param>
    private void ShowItemGet(uint baseId)
    {
        TipsManager.Instance.ShowItemTips(baseId);
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: baseId);
    }

    /// <summary>
    /// 合成操作
    /// </summary>
    private void DoRefine()
    {
        Equip data = Data;
        if (null == data)
        {
            TipsManager.Instance.ShowTips("数据错误");
            return;
        }
        emgr.EquipRefine(selectEquipId, m_uint_refineAssistBaseId, m_bool_refineAutoUseDQ);
    }

    void onClick_BtnRefine_Btn(GameObject caster)
    {
        DoRefine();
    }

    #endregion


    private void UpdateApplyRedPoint()
    {
        UITabGrid tabGrid = null;
        Dictionary<int, UITabGrid> dicTabs = null;
        if (dicUITabGrid.TryGetValue(1, out dicTabs))
        {
            bool CanStrength = DataManager.Manager<ForgingManager>().HaveEquipCanStrengthen();
            bool CanInlay =DataManager.Manager<ForgingManager>().HaveEquipCanInlayByIndex();
            if (dicTabs != null)
            {
                if (dicTabs.TryGetValue((int)TabMode.QiangHua, out tabGrid))
                {
                    tabGrid.SetRedPointStatus(CanStrength);                
                }
                if (dicTabs.TryGetValue((int)TabMode.XiangQian, out tabGrid))
                {
                    tabGrid.SetRedPointStatus(CanInlay);
                }
                RefreshAllTab();
            }
        }
       
    }

    void UpdateTypeGrid() 
    {
        if (IsPanelMode(ForgingPanelMode.Inlay))
        {
//             //更新镶嵌数据
//             UpdateInlayInfo();
            //构建宝石列表
            BuildGemList();
        }
    }
    #region IUIAnimation
    //动画In
    public override void AnimIn(EventDelegate.Callback onComplete)
    {
        if (null == tp)
        {
            tp = m_trans_Content.GetComponent<TweenPosition>();
        }
        EventDelegate.Set(tp.onFinished, onComplete);
        tp.PlayForward();
        tp.enabled = true;
    }
    //动画Out
    public override void AnimOut(EventDelegate.Callback onComplete)
    {
        if (null == tp)
        {
            tp = m_trans_Content.GetComponent<TweenPosition>();
        }
        EventDelegate.Set(tp.onFinished, onComplete);
        tp.PlayReverse();
    }
    //重置动画
    public void ResetAnim()
    {
        //if (null != tp)
        //    tp.ResetToBeginning();
    }
    #endregion

}
