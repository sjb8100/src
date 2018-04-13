using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public partial class KnapsackPanel : UIPanelBase
{
    #region define

    //public enum KnapsackTabs
    //{
    //    Knapsack = 1,
    //    Storage = 2,
    //    Mall =3,
    //    Compose = 4,
    //}

    /// <summary>
    /// 背包状态
    /// </summary>
    public enum KnapsackStatus
    {
        Normal = 1,             //背包
        CarryShop = 2,          //随身商店
        CarryShopSell = 3,      //随身商店出售 
        WareHouse = 4,          //仓库背包
        WareHouseNPC = 5,       //仓库NPC
        BatchSplit = 6,         //批量分解
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

    /// <summary>
    /// 背包分类
    /// </summary>
    public enum KnapsackType
    {
        None = KnapsackItemType.Max,
        //背包
        Knapsack,
        //随身包裹
        CarryShop,
        //仓库
        Warehouse,
        Max,
    }


    #endregion

    #region  Property
    //物品管理器
    private ItemManager imgr = null;
    //装备管理器
    private EquipManager emgr = null;
    //背包管理器
    private KnapsackManager m_kmgr = null;
    const string CLASS_NAME = "KnapsackPanel";
    private KnapsackItemType m_em_itemType = KnapsackItemType.None;
    private KnapsackType m_em_kanpsackType = KnapsackType.None;
    private Dictionary<uint, UITabGrid> m_dic_tabs = null;
    //背包前一个状态
    private KnapsackStatus m_em_preStatus = KnapsackStatus.Normal;
    //背包当前状态
    private KnapsackStatus m_em_curStatus = KnapsackStatus.Normal;
    //anim
    private TweenPosition tp;
    //资源
    private List<string> assetDependens = new List<string>();
    //背包格子数据
    private List<uint> gridDataList = null;
    //背包中最大可视格子数量
    public const int KNAPSACK_GRID_VIEW_MAX = 25;
    //装备格子
    private Dictionary<GameCmd.EquipPos, UIEquipGrid> equipGridDic = null;
    //装备列表
    private Dictionary<GameCmd.EquipPos, uint> equipDic = null;
    //renderobj
    private IRenerTextureObj m_renderObj = null;
    //初始化mask
    private int m_int_modeInitMask = 0;
    //物品过滤mask
    private int m_int_itemFilterMask = 0;
    //是否过滤
    public bool IsFilter
    {
        get
        {
            return (m_int_itemFilterMask & (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Material)) != 0
               && (m_int_itemFilterMask & (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Equip)) != 0
               && (m_int_itemFilterMask & (1 << (int)GameCmd.ItemBaseType.ItemBaseType_Consumption)) != 0 ? false : true;
        }
    }

    private Dictionary<GameCmd.PACKAGETYPE, int> m_dic_cacheOldUnlock = null;
    #endregion

    #region OverrideMethod
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnLoading()
    {
        base.OnLoading();
        InitWidgets();
    }

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalEvent(true);
        ResetPanel();
        if (null != data && data is KnapsackStatus)
        {
            m_em_curStatus = (KnapsackStatus)data;
        }
        else
        {
            m_em_curStatus = KnapsackStatus.Normal;
        }
        InitData();
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        base.OnJump(jumpData);
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        int firstTabData = -1;
        int secondTabData = -1;
        if (null != jumpData.Tabs && jumpData.Tabs.Length >= 1)
        {
            firstTabData = (int)jumpData.Tabs[0];
        }
        else
        {
            firstTabData = (int)TabMode.BeiBao;
        }
        if (null != jumpData.Tabs && jumpData.Tabs.Length >= 2)
        {
            secondTabData = (int)jumpData.Tabs[1];
        }
        UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, firstTabData);

        if (secondTabData != -1)
        {
            SetActiveTab(secondTabData, true);

            
        }
        if( null !=jumpData )
        {
             if(jumpData.Param is uint)
             {
                uint itemID = (uint)jumpData.Param;
                SetSelectItemId(itemID);
             }
          
        }
    }
    public override UIPanelBase.PanelData GetPanelData()
    {
        return base.GetPanelData();
    }


    public override bool OnMsg(UIMsgID msgid, object param)
    {
        if (msgid == UIMsgID.eShowUI)
        {
            ReturnBackUIMsg showInfo = (ReturnBackUIMsg)param;
            if (showInfo != null)
            {
                if (showInfo.tabs.Length > 0)
                {
                    UIFrameManager.Instance.OnCilckTogglePanel(this.PanelId, 1, showInfo.tabs[0]);
                }
                if (showInfo.tabs.Length > 1)
                {
                    SetActiveTab(showInfo.tabs[1], true);
                }
            }
        }
        return base.OnMsg(msgid, param);

    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalEvent(false);
        //   EnablePlayerView(false);
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        if (m_renderObj != null)
        {
            m_renderObj.Release();
            m_renderObj = null;
        }

        if (null != m_trans_PurchaseCostGrid)
        {
            UICurrencyGrid grid = m_trans_PurchaseCostGrid.GetComponent<UICurrencyGrid>();
            if (null != grid)
            {
                grid.Release(true);
            }
        }

        if (null != equipGridDic)
        {
            var enutor = equipGridDic.GetEnumerator();
            while (enutor.MoveNext())
            {
                enutor.Current.Value.Release(false);
            }
        }
        if (null != equipDic)
            equipDic.Clear();

        //背包格
        if (null != m_ctor_ItemGridScrollView)
        {
            m_ctor_ItemGridScrollView.Release(depthRelease);
        }

        //分解
        if (null != m_ctor_SplitScrollView)
        {
            m_ctor_SplitScrollView.Release(depthRelease);
        }

        //随身商店
        if (null != m_ctor_CarryShopTabScrollView)
        {
            m_ctor_CarryShopTabScrollView.Release(depthRelease);
        }
        if (null != m_ctor_CarryShopGridScrollView)
        {
            m_ctor_CarryShopGridScrollView.Release(depthRelease);
        }

        //出售
        if (null != m_ctor_SellShopGridScrollView)
        {
            m_ctor_SellShopGridScrollView.Release(depthRelease);
            m_bCreateCarryShopSell = false;
        }

        //仓库
        if (null != m_ctor_WareHouseItemGridScrollView)
        {
            m_ctor_WareHouseItemGridScrollView.Release(depthRelease);
        }
    }

    public override void ResetPanel()
    {
        base.ResetPanel();
        if (null != m_dic_cacheOldUnlock)
            m_dic_cacheOldUnlock.Clear();
        if (null != equipDic)
            equipDic.Clear();
        ResetShop();
        ResetBatchSplit();
        ResetWareHouse();
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
        RegisterGlobalEvent(false);
        gridDataList.Clear();
        if (null != m_renderObj)
        {
            m_renderObj.Release();
            m_renderObj = null;
        }

        if (null != m_mallItemBaseGrid)
        {
            m_mallItemBaseGrid.Release(true);
            //UIManager.OnObjsRelease(m_mallItemBaseGrid.transform, (uint)GridID.Uiiteminfogrid);
            m_mallItemBaseGrid = null;
        }


        if (null != equipGridDic)
        {
            var enutor = equipGridDic.GetEnumerator();
            while (enutor.MoveNext())
            {
                enutor.Current.Value.Release(true);
                //UIManager.OnObjsRelease(enutor.Current.Value.CacheTransform, (uint)GridID.Uiequipgrid);
            }
            equipGridDic.Clear();
        }
        if (null != equipDic)
            equipDic.Clear();
    }

    public override bool OnTogglePanel(int tabType, int pageid)
    {
        if (tabType == 1)
        {
            if (Enum.IsDefined(typeof(TabMode), pageid))
            {
                switch (pageid)
                {
                    case (int)TabMode.BeiBao:
                        SetKnapsackStatus(KnapsackStatus.Normal);
                        break;
                    case (int)TabMode.CangKu:
                        SetKnapsackStatus(KnapsackStatus.WareHouse);
                        break;
                    //case (int)TabMode.ShangDian:
                    //    SetKnapsackStatus(KnapsackStatus.CarryShop);
                    //    break;
                    case (int)TabMode.HeCheng:
                        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ComposePanel);
                        break;
                }
            };
        }
        return base.OnTogglePanel(tabType, pageid);
    }
    #endregion

    #region Data UI 交互
    /// <summary>
    /// GridCreator 数据更新回调
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    private void OnUpdateGridData(UIGridBase grid, int index)
    {
        if (grid is UIItemGrid)
        {
            uint location = ItemDefine.TransformLocal2ServerLocation(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN, new Vector2(0, index));
            BaseItem itemData = (gridDataList.Count > index) ? imgr.GetBaseItemByQwThisId(gridDataList[index]) : null;
            UIItemGrid itemGrid = grid as UIItemGrid;
            itemGrid.SetLocation(location);
            UIItemInfoGridBase.InfoGridType gridType = UIItemInfoGridBase.InfoGridType.Knapsack;
            if (IsKnapsackStatus(KnapsackStatus.WareHouse)
                || IsKnapsackStatus(KnapsackStatus.WareHouseNPC))
            {
                gridType = UIItemInfoGridBase.InfoGridType.KnapsackWareHouse;
            }
            else if (IsKnapsackStatus(KnapsackStatus.CarryShopSell))
            {
                gridType = UIItemInfoGridBase.InfoGridType.KnapsackSell;
            }
            else if (IsKnapsackStatus(KnapsackStatus.BatchSplit))
            {
                gridType = UIItemInfoGridBase.InfoGridType.KnapsackSplit;
            }
            itemGrid.SetGridData(gridType, itemData);
            bool checkBoxEnable = false;
            if (null != itemData)
            {
                checkBoxEnable = (IsKnapsackStatus(KnapsackStatus.CarryShopSell) && IsSellShopSelectItem(itemData.QWThisID))
                    || (IsKnapsackStatus(KnapsackStatus.BatchSplit) && IsBatchSplitSelectItem(itemData.QWThisID));
            }
            //批量分解,商店出售模式下，设置选中状态
            itemGrid.EnableCheckBox(checkBoxEnable);
        }
        else if (grid is UISplitGetGrid)
        {
            List<uint> splitGetItems = GetSplitGetSortList();
            if (index < splitGetItems.Count)
            {
                UISplitGetGrid getGrid = grid as UISplitGetGrid;
                uint id = splitGetItems[index];
                //string name = "";
                //string icon = "";
                //if (id == GOLD_SPLIT_GET_ID)
                //{
                //    //金币特殊处理
                //    name = "金币";
                //    icon = "1";
                //}else
                //{
                //    BaseItem item = new BaseItem(id);
                //    name = item.Name;
                //    icon = item.Icon;
                //}
                getGrid.Set(id, string.Format("x{0}", m_dic_splitGetItems[id]));
            }
        }
    }

    /// <summary>
    /// 根据Id更新背包数据
    /// </summary>
    /// <param name="qwThidId"></param>
    public void UpdateKnapsackDataById(uint qwThidId)
    {
        if (gridDataList.Contains(qwThidId))
        {
            UpdateKnapsackGrid(gridDataList.IndexOf(qwThidId));
        }
    }

    private void OnEquipFightPowerChanged()
    {
        UpdateKnapsackGrid();
        if (IsKnapsackStatus(KnapsackStatus.WareHouse) || IsKnapsackStatus(KnapsackStatus.WareHouseNPC))
        {
            UpdateWareHouseCreator();
        }

        if (IsKnapsackStatus(KnapsackStatus.CarryShopSell))
        {
            UpdateSellShopCreator();
        }
    }

    /// <summary>
    /// 更新物品UI
    /// </summary>
    /// <param name="data">物品数据</param>
    private void OnUpdateItemDataUI(object data,bool needForce =false)
    {
        if (null == data || !(data is ItemDefine.UpdateItemPassData))
            return;
        ItemDefine.UpdateItemPassData passData = data as ItemDefine.UpdateItemPassData;
        uint qwThisId = passData.QWThisId;
        if (qwThisId == 0)
        {
            Engine.Utility.Log.Warning("{0}->UpdateItemDataUI qwThisId 0！", CLASS_NAME);
            return;
        }

        BaseItem itemData = imgr.GetBaseItemByQwThisId(qwThisId);
        bool clear = (null == itemData) ? true : false;
        //背包检测刷新
        if (null != m_ctor_ItemGridScrollView)
        {
            bool inOldMain = gridDataList.Contains(qwThisId);
            bool inMain = (!clear) ? itemData.PackType == GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN : false;
            if (inMain)
            {
                int curIndex = (int)itemData.Position.y;
                if (inOldMain)
                {
                    int oldIndex = gridDataList.IndexOf(qwThisId);
                    if (!IsFilter)
                    {
                        if (oldIndex != curIndex)
                        {
                            gridDataList[oldIndex] = 0;

                            m_ctor_ItemGridScrollView.UpdateData(oldIndex);
                        }
                        gridDataList[curIndex] = qwThisId;
                        m_ctor_ItemGridScrollView.UpdateData(curIndex);
                    }
                    else
                    {
                        if (IsItemMaskMatchKnapsackMask(itemData.BaseType))
                            m_ctor_ItemGridScrollView.UpdateData(oldIndex);
                    }

                }
                else
                {
                    if (!IsFilter)
                    {
                        gridDataList[curIndex] = qwThisId;
                        m_ctor_ItemGridScrollView.UpdateData(curIndex);
                    }
                    else
                    {
                        if (IsItemMaskMatchKnapsackMask(itemData.BaseType))
                        {
                            gridDataList.Add(qwThisId);
                            ItemManager.SortItemListBySortId(ref gridDataList);
                            int newAddIndex = gridDataList.IndexOf(qwThisId);
                            m_ctor_ItemGridScrollView.InsertData(newAddIndex);
                        }
                    }
                }
            }
            else if (inOldMain)
            {
                int oldIndex = gridDataList.IndexOf(qwThisId);
                //如果已选中在出售界面,移除刷新
                if (IsSellShopSelectItem(qwThisId))
                {
                    UpdateSellShopSelectData(qwThisId, false);
                }

                if (IsBatchSplitSelectItem(qwThisId))
                {
                    UpdateBatchSplitSelectData(qwThisId, false);
                }
                //刷新数据
                if (!IsFilter)
                {
                    gridDataList[oldIndex] = 0;
                    m_ctor_ItemGridScrollView.UpdateData(oldIndex);
                }
                else
                {
                    gridDataList.Remove(qwThisId);
                    m_ctor_ItemGridScrollView.RemoveData(oldIndex);
                }
            }
        }
        //装备栏检测刷新
        if (null != equipGridDic)
        {
            bool equip = emgr.IsWearEquip(qwThisId);
            UIEquipGrid equpGrid = null;
            GameCmd.EquipPos equipPos = GameCmd.EquipPos.EquipPos_None;
            uint tempEquipId = 0;
            bool fightPowerChanged = false;
            if (!clear && equip)
            {
                equipPos = (GameCmd.EquipPos)((int)itemData.Position.y);
                if (equipGridDic.TryGetValue(equipPos, out equpGrid))
                {
                    if (equipDic.TryGetValue(equipPos, out tempEquipId)
                    && tempEquipId != qwThisId)
                    {
                        equipDic[equipPos] = qwThisId;
                        equpGrid.SetGridData(itemData);
                        fightPowerChanged = true;
                    }
                    else if (!equipDic.ContainsKey(equipPos))
                    {
                        equipDic.Add(equipPos, qwThisId);
                        equpGrid.SetGridData(itemData);
                        fightPowerChanged = true;
                    }
                    if (needForce)
                    {
                        Equip m_Equip = imgr.GetBaseItemByQwThisId<Equip>(qwThisId);
                        if (m_Equip.HaveDurable)
                        {
                            equipDic[equipPos] = qwThisId;
                            equpGrid.SetGridData(itemData);
                            fightPowerChanged = true;
                        }
                    }
                    //加特效  单独调用下面的方法  放在SetGridData会和查看其他玩家有冲突
                    uint StrengthenLv = DataManager.Manager<EquipManager>().GetGridStrengthenLvByPos(equipPos);
                    equpGrid.UpdateStrengthenInfo(StrengthenLv, itemData != null);          
                }
            }
            else
            {
                equipPos = GameCmd.EquipPos.EquipPos_None;
                List<GameCmd.EquipPos> equipPosList = equipDic.Keys.ToList();
                for (int i = 0; i < equipPosList.Count; i++)
                {
                    if (equipDic.TryGetValue(equipPosList[i], out tempEquipId)
                    && tempEquipId == qwThisId && qwThisId != 0)
                    {
                        equipPos = equipPosList[i];
                        equipDic[equipPos] = 0;
                        fightPowerChanged = true;
                        break;
                    }
                }

                if (equipGridDic.TryGetValue(equipPos, out equpGrid))
                {
                    equpGrid.SetGridData(null);
                    fightPowerChanged = true;
                }
            }
            if (fightPowerChanged)
            {
                OnEquipFightPowerChanged();
            }
        }

        //仓库检测刷新
        if (IsInitMode(KnapsackStatus.WareHouse))
        {
            bool inCurrentWareHouse = (!clear) ? itemData.PackType == m_em_activeWareHouse : false;
            if (wareHouseItems.Contains(qwThisId) && !inCurrentWareHouse)
            {
                int index = wareHouseItems.IndexOf(qwThisId);
                wareHouseItems[index] = 0;
                m_ctor_WareHouseItemGridScrollView.UpdateData(index);
            }
            else if (inCurrentWareHouse)
            {
                int curIndex = (int)itemData.Position.y;
                if (wareHouseItems.Contains(qwThisId))
                {
                    int oldIndex = wareHouseItems.IndexOf(qwThisId);
                    if (curIndex != oldIndex)
                    {
                        wareHouseItems[curIndex] = qwThisId;
                        wareHouseItems[oldIndex] = 0;
                        m_ctor_WareHouseItemGridScrollView.UpdateData(oldIndex);
                    }

                }
                else
                    wareHouseItems[curIndex] = qwThisId;
                m_ctor_WareHouseItemGridScrollView.UpdateData(curIndex);
            }

        }
    }

    /// <summary>
    /// 开启角色渲染
    /// </summary>
    /// <param name="enable"></param>
    public void EnableRenderTexture(bool enable)
    {
        if (null != m__CharacterRenderTexture && null != m__CharacterRenderTexture.GetComponent<UIRenderTexture>())
        {
            m__CharacterRenderTexture.GetComponent<UIRenderTexture>().Enable(enable);
        }
        SetPlayerFightPower();
    }

    /// <summary>
    /// 更新解锁背包数量
    /// </summary>
    /// <param name="unlockNum"></param>
    /// <param name="max"></param>
    public void UpdateKnapsackCapacityInfo()
    {
        int unlockNum = (int)m_kmgr.GetUnlockGridByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
        int max = (int)m_kmgr.GetMaxGridHaveByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
        if (null != m_label_CapacityNum)
            m_label_CapacityNum.text = unlockNum + "/" + max;
    }

    /// <summary>
    /// 解锁信息改变
    /// </summary>
    /// <param name="pType"></param>
    private void OnUnlockInfoChanged(KnapsackManager.UnlockPassData passData)
    {

        GameCmd.PACKAGETYPE type = (GameCmd.PACKAGETYPE)passData.Type;
        KnapsackDefine.LocalUnlockInfo unlockInfo
            = m_kmgr.GetUnlockInfoByPackageType(type);
        if (null == unlockInfo || !unlockInfo.IsUnlock)
        {
            //如果当前包裹未激活返回
            return;
        }
        int oldCount = 0;
        if (m_dic_cacheOldUnlock.ContainsKey(type))
        {
            oldCount = m_dic_cacheOldUnlock[type];
            m_dic_cacheOldUnlock[type] = unlockInfo.UnlockNum;
        }
        else
        {
            m_dic_cacheOldUnlock.Add(type, unlockInfo.UnlockNum);
        }
        int needAdd = 0;
        int totalCount = unlockInfo.UnlockNum;
        int maxGridPackageHave = m_kmgr.GetMaxGridHaveByPackageType(type);
        bool needFocusLastUnlock = false;

        int oldLockNum = 0;
        if (type == GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN && !IsFilter)
        {
            //背包
            oldLockNum = (int)Math.Max(0, gridDataList.Count - totalCount);
            needAdd = (oldLockNum >= KnapsackManager.KNAPSACK_GRID_COLUMN_MAX) ? 0 : KnapsackManager.KNAPSACK_GRID_COLUMN_MAX;
            needAdd = Math.Min(needAdd, maxGridPackageHave - gridDataList.Count);

            needFocusLastUnlock = (needAdd > 0 && oldLockNum == 0) || !m_ctor_ItemGridScrollView.IsGridInClipArea(totalCount, true);
            for (int i = 0; i < needAdd; i++)
            {
                gridDataList.Add(0);
                m_ctor_ItemGridScrollView.InsertData(gridDataList.Count - 1);
            }
            for (int i = (int)oldCount; i < totalCount; i++)
            {
                m_ctor_ItemGridScrollView.UpdateData(i);
            }

            if (needFocusLastUnlock && !passData.IsReconnect)
            {
                m_ctor_ItemGridScrollView.FocusGrid(gridDataList.Count - 1, false);
            }
            UpdateKnapsackCapacityInfo();
        }
        else if (m_kmgr.IsWareHosue(type)
           && IsKnapsackStatus(KnapsackStatus.WareHouse) ||
           IsKnapsackStatus(KnapsackStatus.WareHouseNPC))
        {
            //仓库
            oldLockNum = (int)Math.Max(0, wareHouseItems.Count - totalCount);
            needAdd = (oldLockNum >= KnapsackManager.KNAPSACK_GRID_COLUMN_MAX) ? 0 : KnapsackManager.KNAPSACK_GRID_COLUMN_MAX;
            needAdd = Math.Min(needAdd, maxGridPackageHave - wareHouseItems.Count);

            needFocusLastUnlock = (needAdd > 0 && oldLockNum == 0) || !m_ctor_WareHouseItemGridScrollView.IsGridInClipArea(totalCount, true);

            for (int i = 0; i < needAdd; i++)
            {
                wareHouseItems.Add(0);
                m_ctor_WareHouseItemGridScrollView.InsertData(wareHouseItems.Count - 1);
            }
            for (int i = (int)oldCount; i < totalCount; i++)
            {
                m_ctor_WareHouseItemGridScrollView.UpdateData(i);
            }

            if (needFocusLastUnlock && !passData.IsReconnect)
            {
                m_ctor_WareHouseItemGridScrollView.FocusGrid(wareHouseItems.Count - 1, false);
            }
            UpdateWareHouseStatus();
            UpdateWareHouseCapcity();
        }
    }

    /// 解锁背包格子
    /// </summary>
    /// <param name="num">需要解锁的数量</param>
    /// <param name="pType">背包类型</param>
    private void DoUnlocKnapsackGrid(int num, GameCmd.PACKAGETYPE pType)
    {
        string msg = string.Format("是否要花费{0}，解锁{1}个背包格？", m_kmgr.GetUnlockGridCostString(pType, num), num);
        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.YesNO, msg,
            () =>
            {
                m_kmgr.UnlockKnapsackGrid(pType, num);
            }, null, title: "提示");
    }
    #endregion

    /// <summary>
    /// 角色属性改变广播
    /// </summary>
    /// <param name="nEventID"></param>
    /// <param name="param"></param>
    private void OnCharacterPropertyChange(int nEventID, object param)
    {
        switch (nEventID)
        {
            case (int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE:
                if (null != m_label_lvlabel)
                    m_label_lvlabel.text = "Lv." + Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level);
                break;
            case (int)Client.GameEventID.PLAYER_FIGHTPOWER_REFRESH:
                SetPlayerFightPower();
                break;
            case (int)Client.GameEventID.ENTITYSYSTEM_CHANGERENDEROBJ:
                {
                    Client.stRefreshRenderObj data = (Client.stRefreshRenderObj)param;
                    if (m_renderObj != null
                        && data.userID == DataManager.Instance.UserId)
                    {
                        Client.EquipPos pos = DataManager.Manager<SuitDataManager>().GetPosBySuitType((uint)data.suitType);
                        m_renderObj.ChangeSuit(pos, (int)data.suitID);
                    }
                }
                break;
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
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONADD, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONDISCARD, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONWEAR, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTFASHIONTAKEOFF, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.REFRESHITEMDURABLE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnCharacterPropertyChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.PLAYER_FIGHTPOWER_REFRESH, OnCharacterPropertyChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGERENDEROBJ, OnCharacterPropertyChange);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_WAREHOUSESTORECOPPERCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_KNAPSACKUNLOCKINFOCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UNLOCKWAREHOSUE, OnGlobalUIEventHandler);

            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENTGUIDEITEMFOCUS, OnGlobalUIEventHandler);

            //装备格强化
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_GRIDSTRENGTHENLVCHANGED, OnGlobalUIEventHandler);
            //颜色套装变化
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_EQUIPCOLORSUITCHANGE, OnGlobalUIEventHandler);
            //宝石套装变化
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_EQUIPSTONESUITCHANGE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.ENTITYSYSTEM_NEWNAME, OnGlobalUIEventHandler);
        }
        else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONADD, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONDISCARD, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONWEAR, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTFASHIONTAKEOFF, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.REFRESHITEMDURABLE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_PROPUPDATE, OnCharacterPropertyChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.PLAYER_FIGHTPOWER_REFRESH, OnCharacterPropertyChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_CHANGERENDEROBJ, OnCharacterPropertyChange);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_WAREHOUSESTORECOPPERCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_KNAPSACKUNLOCKINFOCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UNLOCKWAREHOSUE, OnGlobalUIEventHandler);

            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENTGUIDEITEMFOCUS, OnGlobalUIEventHandler);

            //装备格强化
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_GRIDSTRENGTHENLVCHANGED, OnGlobalUIEventHandler);

            //颜色套装变化
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_EQUIPCOLORSUITCHANGE, OnGlobalUIEventHandler);
            //宝石套装变化
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_EQUIPSTONESUITCHANGE, OnGlobalUIEventHandler);
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.ENTITYSYSTEM_NEWNAME, OnGlobalUIEventHandler);
        }
    }

    private void OnGlobalUIEventHandler(int eventType, object data)
    {
        switch (eventType)
        {
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                OnUpdateItemDataUI(data);
                UpdateKnapsackGrid();
                UpdateWareHouseCreator();
                break;
            //修理
            case (int)Client.GameEventID.REFRESHITEMDURABLE:
                {
                     OnUpdateItemDataUI(data,true);
                }
                break;
            case (int)Client.GameEventID.UIEVENT_KNAPSACKUNLOCKINFOCHANGED:
                OnUnlockInfoChanged((KnapsackManager.UnlockPassData)data);
                break;
            case (int)Client.GameEventID.UIEVENT_WAREHOUSESTORECOPPERCHANGED:
                UpdateWareHouseStoreCopperNum((uint)data);
                break;
            case (int)Client.GameEventID.UIEVENT_UNLOCKWAREHOSUE:
                OnUnlockWareHosue((GameCmd.PACKAGETYPE)data);
                break;
            case (int)Client.GameEventID.UIEVENTFASHIONCHANGED:
            case (int)Client.GameEventID.UIEVENTFASHIONDISCARD:
            case (int)Client.GameEventID.UIEVENTFASHIONADD:
            case (int)Client.GameEventID.UIEVENTFASHIONWEAR:
            case (int)Client.GameEventID.UIEVENTFASHIONTAKEOFF:
                OnFashionDataChanged(eventType, (uint)data);
                break;

            case (int)Client.GameEventID.UIEVENTGUIDEITEMFOCUS:
                {
                    if (null != data && data is GuideDefine.GuideItemFocusData
                        && null != m_ctor_ItemGridScrollView)
                    {
                        GuideDefine.GuideItemFocusData fd = data as GuideDefine.GuideItemFocusData;
                        if (fd.DependType == GuideDefine.GuideGUIDependType.Knapsack)
                        {
                            uint qwid = (uint)fd.Data;
                            if (gridDataList.Contains(qwid))
                            {
                                m_ctor_ItemGridScrollView.FocusGrid(gridDataList.IndexOf(qwid));
                            }
                        }
                    }
                }
                break;
            //装备格强化属性变更
            case (int)Client.GameEventID.UIEVENT_GRIDSTRENGTHENLVCHANGED:
                {
                    if (null != data && data is List<GameCmd.StrengthList>)
                    {
                        List<GameCmd.StrengthList> slist = data as List<GameCmd.StrengthList>;
                        if (null != slist)
                        {
                            GameCmd.StrengthList tempsData = null;
                            bool equip = false;
                            uint equipId = 0;
                            for (int i = 0; i < slist.Count; i++)
                            {
                                tempsData = slist[i];
                                equip = DataManager.Manager<EquipManager>().IsEquipPos(tempsData.equip_pos, out equipId);
                                equipGridDic[tempsData.equip_pos].UpdateStrengthenInfo(tempsData.level, equip);
                            }
                        }
                    }
                }
                break;
            //强化套装属性变更
            case (int)Client.GameEventID.UIEVENT_STRENGTHENACTIVESUITCHANGED:
                {
                    UpdateStrengthenSuitData();
                }
                break;
            //颜色套装属性变更
            case (int)Client.GameEventID.UIEVENT_EQUIPCOLORSUITCHANGE:
                {
                    UpdateEquipColorSuitData();
                }
                break;
            //宝石套装属性变更
            case (int)Client.GameEventID.UIEVENT_EQUIPSTONESUITCHANGE:
                {
                    UpdateEquipStoneSuitData();
                }
                break;
            //改名
            case (int)Client.GameEventID.ENTITYSYSTEM_NEWNAME:
                {
                    Client.stNewName name = (Client.stNewName)data;

                    m_label_namelabel.text = name.newName;
                }
                break;
          


        }
    }

    private void UpdateStrengthenSuitData()
    {
        uint activeSuitLv = emgr.ActiveStrengthenSuitLv;
        if (null != m_label_ActiveGridSuitLv)
        {
            m_label_ActiveGridSuitLv.text = activeSuitLv.ToString();
        }
        bool visble = activeSuitLv == 0;
        if (null != m_btn_BtnGridSuitNormal && m_btn_BtnGridSuitNormal.gameObject.activeSelf != visble)
        {
            m_btn_BtnGridSuitNormal.gameObject.SetActive(visble);
        }
        visble = !visble;
        if (null != m_btn_BtnGridSuitActive && m_btn_BtnGridSuitActive.gameObject.activeSelf != visble)
        {
            m_btn_BtnGridSuitActive.gameObject.SetActive(visble);
        }
    }
    private void UpdateEquipColorSuitData()
    {
        uint activeColorSuitLv = emgr.ActiveColorSuitLv;
        if (null != m_label_ActiveGridSuitLv)
        {
            m_label_ActiveColorSuitLv.text = activeColorSuitLv.ToString();
        }
        bool visble = activeColorSuitLv == 0;
        if (null != m_btn_BtnColorSuitNormal && m_btn_BtnColorSuitNormal.gameObject.activeSelf != visble)
        {
            m_btn_BtnColorSuitNormal.gameObject.SetActive(visble);
        }
        visble = !visble;
        if (null != m_btn_BtnColorSuitActive && m_btn_BtnColorSuitActive.gameObject.activeSelf != visble)
        {
            m_btn_BtnColorSuitActive.gameObject.SetActive(visble);
        }
    }
    private void UpdateEquipStoneSuitData()
    {
        uint activeStoneSuitLv = emgr.ActiveStoneSuitLv;
        if (null != m_label_ActiveGridSuitLv)
        {
            m_label_ActiveStoneSuitLv.text = activeStoneSuitLv.ToString();
        }
        bool visble = activeStoneSuitLv == 0;
        if (null != m_btn_BtnStoneSuitNormal && m_btn_BtnStoneSuitNormal.gameObject.activeSelf != visble)
        {
            m_btn_BtnStoneSuitNormal.gameObject.SetActive(visble);
        }
        visble = !visble;
        if (null != m_btn_BtnStoneSuitActive && m_btn_BtnStoneSuitActive.gameObject.activeSelf != visble)
        {
            m_btn_BtnStoneSuitActive.gameObject.SetActive(visble);
        }
        if(activeStoneSuitLv != 0)
        {
            if (m_renderObj != null)
            {
                DataManager.Manager<EquipManager>().AddEquipStoneSuitParticle(m_renderObj, activeStoneSuitLv);
            }
        }
     
    }



    /// <summary>
    /// 时装状态改变
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="id"></param>
    private void OnFashionDataChanged(int eventType, uint id)
    {
        if (eventType == (int)Client.GameEventID.UIEVENTFASHIONADD)
        {


        }
        else if (eventType == (int)Client.GameEventID.UIEVENTFASHIONDISCARD)
        {

        }
        else if (eventType == (int)Client.GameEventID.UIEVENTFASHIONWEAR)
        {
            if (null != m_renderObj)
            {
                //FashionDefine.LocalFashionData data
                //    = DataManager.Manager<FashionManager>().GetOwerFashionData(id);
                //if (null == data)
                //{
                //    Engine.Utility.Log.Error("Wear OnFashionDataChanged data null QwThisId = {0}", id);
                //    return;
                //}
                //m_renderObj.ChangeSuit((Client.EquipPos)data.LocalDB.type, (int)data.BaseId);
            }
            else
            {
                EnablePlayerView(true);
            }
        }
        else if (eventType == (int)Client.GameEventID.UIEVENTFASHIONWEARDEFAULT)
        {
            if (null != m_renderObj)
            {
                EnablePlayerView(true);
            }
            //FashionDefine.LocalFashionData data
            //        = DataManager.Manager<FashionManager>().GetFashionLocalDB(id);
            //if (null == data)
            //{
            //    Engine.Utility.Log.Error("Wear UIEventFashionWearDefault data null id = {0}", id);
            //    return;
            //}
            //m_renderObj.ChangeSuit((Client.EquipPos)data.LocalDB.type, 0);
        }
        else if (eventType == (int)Client.GameEventID.UIEVENTFASHIONTAKEOFF)
        {

        }
        else if (eventType == (int)Client.GameEventID.UIEVENTFASHIONCHANGED)
        {

        }
    }
    #region Init

    /// <summary>
    /// 是否初始化格子生成器
    /// </summary>
    /// <returns></returns>
    private void InitData()
    {
        SetKnapsackStatus(m_em_curStatus);
        SetKnapsackItemType(KnapsackItemType.ItemAll, true);
    }

    /// <summary>
    /// 初始化背包组件
    /// </summary>
    private void InitWidgets()
    {
        imgr = DataManager.Manager<ItemManager>();
        emgr = DataManager.Manager<EquipManager>();
        m_kmgr = DataManager.Manager<KnapsackManager>();
        gridDataList = new List<uint>();
        m_dic_tabs = new Dictionary<uint, UITabGrid>();
        m_dic_cacheOldUnlock = new Dictionary<GameCmd.PACKAGETYPE, int>();
        Transform ts = null;
        UITabGrid tab = null;
        if (null != m_trans_StorageToggle)
        {
            for (KnapsackType i = KnapsackType.None + 1; i < KnapsackType.Max; i++)
            {
                ts = m_trans_StorageToggle.Find(i.ToString());
                if (null == ts)
                {
                    continue;
                }

                tab = ts.GetComponent<UITabGrid>();
                if (null == tab)
                {
                    tab = ts.gameObject.AddComponent<UITabGrid>();
                }
                if (null != tab && !m_dic_tabs.ContainsKey((uint)i))
                {
                    tab.RegisterUIEventDelegate(OnUIEventCallback);
                    tab.SetHightLight(false);
                    tab.SetGridData(i);
                    m_dic_tabs.Add((uint)i, tab);
                }
            }
        }
        //SetKnapsackStatus(KnapsackStatus.Normal);

        if (null != m_trans_bookmarktoggle)
        {
            for (KnapsackItemType i = KnapsackItemType.None + 1; i < KnapsackItemType.Max; i++)
            {
                ts = m_trans_bookmarktoggle.Find(i.ToString());
                if (null == ts)
                {
                    continue;
                }

                tab = ts.GetComponent<UITabGrid>();
                if (null == tab)
                {
                    tab = ts.gameObject.AddComponent<UITabGrid>();
                }
                if (null != tab && !m_dic_tabs.ContainsKey((uint)i))
                {
                    tab.RegisterUIEventDelegate(OnUIEventCallback);
                    tab.SetHightLight(false);
                    tab.SetGridData(i);
                    m_dic_tabs.Add((uint)i, tab);
                }
            }
        }
        //SetKnapsackItemType(KnapsackItemType.ItemAll, true);

        //初始化生成器

        if (null != m_ctor_ItemGridScrollView && null != m_trans_UIItemGrid)
        {
            if (!m_ctor_ItemGridScrollView.Visible)
            {
                m_ctor_ItemGridScrollView.SetVisible(true);
            }
            m_ctor_ItemGridScrollView.RefreshCheck();
            m_ctor_ItemGridScrollView.Initialize<UIItemGrid>(m_trans_UIItemGrid.gameObject, OnUpdateGridData, OnUIEventCallback);
            //m_ctor_ItemGridScrollView.Initialize<UIItemGrid>((uint)GridID.Uiitemgrid
            //    , UIManager.OnObjsCreate, UIManager.OnObjsRelease
            //    , OnUpdateGridData, OnUIEventCallback);

        }
    }

    /// <summary>
    /// 初始化Normal
    /// </summary>
    private void InitNormal()
    {
        if (IsInitMode(KnapsackStatus.Normal))
        {
            return;
        }
        SetInitMode(KnapsackStatus.Normal);
        if (null == equipGridDic)
            equipGridDic = new Dictionary<GameCmd.EquipPos, UIEquipGrid>();
        equipGridDic.Clear();

        if (null == equipDic)
            equipDic = new Dictionary<GameCmd.EquipPos, uint>();
        equipDic.Clear();

        //创建装备格子初始化
        if (null != m_trans_EquipmentGridRoot)
        {
            UIEquipGrid equipGridTemp = null;
            Transform equipTs = null;
            string name = "";
            Transform tempCloneTransform = null;
            for (GameCmd.EquipPos i = GameCmd.EquipPos.EquipPos_Hat; i < GameCmd.EquipPos.EquipPos_Max; i++)
            {
                if (i == GameCmd.EquipPos.EquipPos_Capes
                    || i == GameCmd.EquipPos.EquipPos_Office)
                {
                    continue;
                }
                name = i.ToString().Split(new char[] { '_' })[1];
                equipTs = Util.findTransform(m_trans_EquipmentGridRoot, name);
                if (null == equipTs)
                {
                    continue;
                }
                //tempCloneTransform = UIManager.OnObjsCreate((uint)GridID.Uiequipgrid);
                //if (null == tempCloneTransform)
                //{
                //    break;
                //}
                //Util.AddChildToTarget(equipTs, tempCloneTransform);
                tempCloneTransform = equipTs.GetChild(0);
                if (null == tempCloneTransform)
                {
                    continue;
                }
                equipGridTemp = tempCloneTransform.GetComponent<UIEquipGrid>();
                if (null == equipGridTemp)
                {
                    equipGridTemp = tempCloneTransform.gameObject.AddComponent<UIEquipGrid>();
                }
                equipGridTemp.SetVisible(true);
                equipGridTemp.InitEquipGrid(i);
                equipGridTemp.RegisterUIEventDelegate(OnEquipGridEventCallback);
                equipGridDic.Add(i, equipGridTemp);
            }
        }

    }

    /// <summary>
    /// 是否初始化当前模式
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public bool IsInitMode(KnapsackStatus status)
    {
        return (m_int_modeInitMask & (1 << (int)status)) != 0;
    }

    /// <summary>
    /// 设置初始化状态
    /// </summary>
    /// <param name="status"></param>
    /// <param name="init"></param>
    public void SetInitMode(KnapsackStatus status, bool init = true)
    {
        if (init)
        {
            m_int_modeInitMask |= ((1 << (int)status));
        }
        else
        {
            m_int_modeInitMask &= (~((1 << (int)status)));
        }

    }

    /// <summary>
    /// 初始化个模式组件
    /// </summary>
    private void InitPanelModeWidgets()
    {
        switch (m_em_curStatus)
        {
            case KnapsackStatus.Normal:
                {
                    InitNormal();
                }
                break;
            case KnapsackStatus.CarryShop:
                {
                    InitCarryShop();
                }
                break;
            case KnapsackStatus.CarryShopSell:
                {
                    InitSellShop();
                }
                break;
            case KnapsackStatus.WareHouse:
            case KnapsackStatus.WareHouseNPC:
                {
                    InitWareHouse();
                }
                break;
            case KnapsackStatus.BatchSplit:
                {
                    InitBatchSplit();
                }
                break;
        }
    }
    #endregion

    #region Op
    public bool IsItemMaskMatchKnapsackMask(GameCmd.ItemBaseType baseType)
    {
        return ((1 << (int)baseType & m_int_itemFilterMask) != 0);
    }
    /// <summary>
    /// 更新组件可视状态
    /// </summary>
    public void UpdateWidgetsVisibleStatus()
    {
        bool visble = !(IsKnapsackStatus(KnapsackStatus.BatchSplit) || IsKnapsackStatus(KnapsackStatus.CarryShopSell));
        if (null != m_trans_bookmarktoggle && m_trans_bookmarktoggle.gameObject.activeSelf != visble)
        {
            m_trans_bookmarktoggle.gameObject.SetActive(visble);
        }

        //随身商店
        visble = IsKnapsackStatus(KnapsackStatus.CarryShop);
        if (null != m_trans_CarryShopConetent
            && m_trans_CarryShopConetent.gameObject.activeSelf != visble)
        {
            m_trans_CarryShopConetent.gameObject.SetActive(visble);
        }

        if (null != m_trans_RightContent && m_trans_RightContent.gameObject.activeSelf == visble)
        {
            m_trans_RightContent.gameObject.SetActive(!visble);
        }

        //出售
        visble = IsKnapsackStatus(KnapsackStatus.CarryShopSell);
        if (null != m_trans_SellShopConetent
            && m_trans_SellShopConetent.gameObject.activeSelf != visble)
        {
            m_trans_SellShopConetent.gameObject.SetActive(visble);
        }
        visble = IsKnapsackStatus(KnapsackStatus.CarryShopSell)
            || IsKnapsackStatus(KnapsackStatus.BatchSplit);
        if (null != m_btn_BackToPkg && m_btn_BackToPkg.gameObject.activeSelf != visble)
        {
            m_btn_BackToPkg.gameObject.SetActive(visble);
        }

        //仓库
        visble = IsKnapsackStatus(KnapsackStatus.WareHouse)
            || IsKnapsackStatus(KnapsackStatus.WareHouseNPC);
        if (null != m_trans_WareHouseContent
            && m_trans_WareHouseContent.gameObject.activeSelf != visble)
        {
            m_trans_WareHouseContent.gameObject.SetActive(visble);
        }

        visble = true;
        if (null != m_btn_WareHouseArrangeBtn &&
            m_btn_WareHouseArrangeBtn.gameObject.activeSelf != visble)
        {
            m_btn_WareHouseArrangeBtn.gameObject.SetActive(visble);
        }


        visble = true;
        if (null != m_btn_WareHouseStoreCopperBtn &&
            m_btn_WareHouseStoreCopperBtn.gameObject.activeSelf != visble)
            m_btn_WareHouseStoreCopperBtn.gameObject.SetActive(visble);

        //player info
        visble = IsKnapsackStatus(KnapsackStatus.Normal);
        if (null != m_trans_PlayerContent
            && m_trans_PlayerContent.gameObject.activeSelf != visble)
        {
            m_trans_PlayerContent.gameObject.SetActive(visble);
        }
        //toggle

        //Btns
        visble = IsKnapsackStatus(KnapsackStatus.Normal);
        if (null != m_btn_repairs
            && m_btn_repairs.gameObject.activeSelf != visble)
        {
            m_btn_repairs.gameObject.SetActive(visble);
        }

        if (null != m_btn_Split
            && m_btn_Split.gameObject.activeSelf)
        {
            m_btn_Split.gameObject.SetActive(false);
        }

        if (null != m_btn_CarryShopSellBtn
             && m_btn_CarryShopSellBtn.gameObject.activeSelf != visble)
        {
            m_btn_CarryShopSellBtn.gameObject.SetActive(visble);
        }

        if (!m_btn_arrange.gameObject.activeSelf)
            m_btn_arrange.gameObject.SetActive(true);

        visble = IsKnapsackStatus(KnapsackStatus.BatchSplit);
        if (null != m_trans_SplitContent
            && m_trans_SplitContent.gameObject.activeSelf != visble)
        {
            m_trans_SplitContent.gameObject.gameObject.SetActive(visble);
        }
        visble = !IsKnapsackStatus(KnapsackStatus.Normal)
            && !IsKnapsackStatus(KnapsackStatus.CarryShop);
        if (null != m_label_LongClickTips
            && m_label_LongClickTips.gameObject.activeSelf != visble)
        {
            m_label_LongClickTips.gameObject.SetActive(visble);
        }

    }

    /// <summary>
    /// 更新Normal
    /// </summary>
    private void CreateNormal()
    {
        EnablePlayerView(true);
        if (null != m_label_namelabel)
            m_label_namelabel.text = Client.ClientGlobal.Instance().MainPlayer.GetName();
        if (null != m_label_lvlabel)
            m_label_lvlabel.text = "Lv." + Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.CreatureProp.Level);
        UpdateStrengthenSuitData();
        UpdateEquipColorSuitData();
        UpdateEquipStoneSuitData();
        SetPlayerFightPower();
        UpdateEquipGridInfo();
        UpdateKnapsackCapacityInfo();
    }
//     void UpdateSingleEquipGridInfo(uint equipId) 
//     {
//         UIEquipGrid equipGrid = null;
//         BaseItem baseItem = null;
//         uint curEquipId = 0;
//         for (GameCmd.EquipPos i = GameCmd.EquipPos.EquipPos_Hat; i < GameCmd.EquipPos.EquipPos_Max; i++)
//         {
//             if (i == GameCmd.EquipPos.EquipPos_Capes
//                 || i == GameCmd.EquipPos.EquipPos_Office)
//             {
//                 continue;
//             }
//             if (equipGridDic.TryGetValue(i, out equipGrid))
//             {
//                   equipGrid.SetPreView(EquipDefine.GetEquipPartIcon(i));
//                   if (emgr.IsEquipPos(i, out curEquipId))
//                   {
//                       if (curEquipId == equipId)
//                       {
//                           baseItem = imgr.GetBaseItemByQwThisId<BaseItem>(equipId);
//                           if (baseItem != null)
//                           {
//                               equipGrid.SetGridData(baseItem);
//                           }
//                       }                  
//                   }
//             }
//         }
//     }
    /// <summary>
    /// 刷新装备栏
    /// </summary>
    private void UpdateEquipGridInfo()
    {
        //装备栏
        uint equipId = 0;
        uint cacheEquipId = 0;
        UIEquipGrid equipGrid = null;
        BaseItem baseItem = null;
        bool needSetData = false;
        for (GameCmd.EquipPos i = GameCmd.EquipPos.EquipPos_Hat; i < GameCmd.EquipPos.EquipPos_Max; i++)
        {
            if (i == GameCmd.EquipPos.EquipPos_Capes
                || i == GameCmd.EquipPos.EquipPos_Office)
            {
                continue;
            }
            needSetData = false;
            if (equipGridDic.TryGetValue(i, out equipGrid))
            {
                equipGrid.SetPreView(EquipDefine.GetEquipPartIcon(i));
                if (emgr.IsEquipPos(i, out equipId))
                {
                    baseItem = imgr.GetBaseItemByQwThisId<BaseItem>(equipId);
                    if (equipDic.TryGetValue(i, out cacheEquipId)
                        && cacheEquipId != equipId)
                    {
                        equipDic[i] = equipId;
                        needSetData = true;
                    }
                    else if (!equipDic.ContainsKey(i))
                    {
                        equipDic.Add(i, equipId);
                        needSetData = true;
                    }

                    if (needSetData)
                    {
                        equipGrid.SetGridData(baseItem);
                    }
                }
                else
                {
                    baseItem = null;
                    if (equipDic.ContainsKey(i))
                    {
                        equipDic[i] = 0;
                    }
                    needSetData = true;
                }

                if (needSetData)
                {
                    equipGrid.SetGridData(baseItem);
                }
                //加特效  单独调用下面的方法  放在SetGridData会和查看其他玩家有冲突
                uint StrengthenLv = DataManager.Manager<EquipManager>().GetGridStrengthenLvByPos(i);
                equipGrid.UpdateStrengthenInfo(StrengthenLv, baseItem != null);
            }
          
        }
    }

    /// <summary>
    /// 生成背包格子
    /// </summary>
    private void CreateKnapsackGrid()
    {
        if (null == m_ctor_ItemGridScrollView)
        {
            return;
        }
        List<uint> itemIds = DataManager.Manager<ItemManager>().DoFilterItemData(
          GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN, m_int_itemFilterMask);
        if (null == itemIds)
        {
            m_ctor_ItemGridScrollView.CreateGrids(0);
            return;
        }

        gridDataList.Clear();
        Dictionary<uint, uint> gridDataDic = new Dictionary<uint, uint>();
        BaseItem data = null;
        foreach (uint qwid in itemIds)
        {
            data = imgr.GetBaseItemByQwThisId(qwid);
            if (null == data || data.Num == 0)
                continue;
            if (gridDataDic.ContainsKey(data.ServerLocaltion))
            {
                Engine.Utility.Log.Warning(CLASS_NAME + "->CreateItemDataUI add to temp Dic Faield,Have same key = {0}", data.QWThisID);
                continue;
            }
            else
            {
                gridDataDic.Add(data.ServerLocaltion, data.QWThisID);
            }
        }

        //重新构建格子列表
        KnapsackDefine.LocalUnlockInfo mainPackUnlockInfo
            = m_kmgr.GetUnlockInfoByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
        int mainPackMaxGridHave = m_kmgr.GetMaxGridHaveByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
        if (null == mainPackUnlockInfo)
        {
            m_ctor_ItemGridScrollView.CreateGrids(0);
            return;
        }
        int max = (IsFilter) ? itemIds.Count : mainPackUnlockInfo.UnlockNum;
        int mod = mainPackUnlockInfo.UnlockNum % KnapsackManager.KNAPSACK_GRID_COLUMN_MAX;
        int lockNum = (IsFilter) ? 0 : ((mod != 0) ? (KnapsackManager.KNAPSACK_GRID_COLUMN_MAX * 2 - mod) : KnapsackManager.KNAPSACK_GRID_COLUMN_MAX);
        lockNum = Math.Min(lockNum, mainPackMaxGridHave - max);
        uint combineLocation = 0;
        for (int i = 0; i < max + lockNum; i++)
        {
            if (IsFilter)
            {
                data = imgr.GetBaseItemByQwThisId(itemIds[i]);
                if (null != data)
                {
                    combineLocation = data.ServerLocaltion;
                }else
                {
                    Engine.Utility.Log.Error("CreateKnapsackGrid->Find item failed,QwThisID={0}",itemIds[i]);
                    continue;
                }
            }else
            {
                combineLocation = ItemDefine.TransformLocal2ServerLocation(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN, new Vector2(0, i));
            }
            gridDataList.Add((gridDataDic.ContainsKey(combineLocation)) ? gridDataDic[combineLocation] : 0);
        }

        //创建格子数据
        m_ctor_ItemGridScrollView.CreateGrids(gridDataList.Count);
        if (m_dic_cacheOldUnlock.ContainsKey(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN))
        {
            m_dic_cacheOldUnlock[GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN] = mainPackUnlockInfo.UnlockNum;
        }
        else
        {
            m_dic_cacheOldUnlock.Add(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN, mainPackUnlockInfo.UnlockNum);
        }
    }

    /// <summary>
    ///刷新背包单个格子 
    /// </summary>
    /// <param name="index"></param>
    private void UpdateKnapsackGrid(int index = -1)
    {
        if (null != m_ctor_ItemGridScrollView)
        {
            if (index == -1)
            {
                m_ctor_ItemGridScrollView.UpdateActiveGridData();
            }
            else if (index >= 0)
            {
                m_ctor_ItemGridScrollView.UpdateData(index);
            }
        }
    }

    /// <summary>
    /// 根据面板模式刷新数据
    /// </summary>
    private void CreateDataByPanelMode()
    {
        switch (m_em_curStatus)
        {
            case KnapsackStatus.Normal:
                {
                    CreateNormal();
                }
                break;
            case KnapsackStatus.CarryShop:
                {
                    CreateCarryShop();
                }
                break;
            case KnapsackStatus.CarryShopSell:
                {
                    CreateCarryShopSell();
                }
                break;
            case KnapsackStatus.WareHouse:
            case KnapsackStatus.WareHouseNPC:
                {
                    SetWareHousePackage(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE1, true);
                    //CreateWareHouse();
                }
                break;
            case KnapsackStatus.BatchSplit:
                {
                    CreateBatchSplit();
                }
                break;
        }
    }
    /// <summary>
    /// 当前背包状态是否为status
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public bool IsKnapsackStatus(KnapsackStatus status)
    {
        return (m_em_curStatus == status) ? true : false;
    }

    /// <summary>
    /// 设置背包状态
    /// </summary>
    /// <param name="status"></param>
    public void SetKnapsackStatus(KnapsackStatus status)
    {
        m_em_preStatus = m_em_curStatus;
        m_em_curStatus = status;
        UpdateWidgetsVisibleStatus();
        InitPanelModeWidgets();
        CreateDataByPanelMode();
        switch (status)
        {
            case KnapsackStatus.Normal:
                {
                    SetKnapsackType(KnapsackType.Knapsack);
                    UpdateKnapsackGrid();
                }
                break;
            case KnapsackStatus.CarryShop:
                {
                    SetKnapsackType(KnapsackType.CarryShop);
                    UpdateKnapsackGrid();
                }
                break;
            case KnapsackStatus.CarryShopSell:
                {
                    SetKnapsackType(KnapsackType.CarryShop);
                    UpdateKnapsackGrid();
                }
                break;
            case KnapsackStatus.WareHouse:
            case KnapsackStatus.WareHouseNPC:
                {
                    SetKnapsackType(KnapsackType.Warehouse);
                    UpdateKnapsackGrid();
                }
                break;
            case KnapsackStatus.BatchSplit:
                {
                    SetKnapsackType(KnapsackType.Knapsack);
                }
                break;
        }
        UpdateKnapsackGrid();
    }

    /// <summary>
    /// 启用角色
    /// </summary>
    /// <param name="enable"></param>
    private void EnablePlayerView(bool enable)
    {
        if (null == m_renderObj)
        {
            m_renderObj = DataManager.Manager<RenderTextureManager>().CreateRenderTextureObj(DataManager.Instance.MainPlayer, 750);
            if (null != m_renderObj)
            {
                SetRotateYNormal();
                m_renderObj.SetDisplayCamera(new Vector3(-3, 1f, -3f), new Vector3(0, 45, 0));
                m_renderObj.PlayModelAni(Client.EntityAction.Stand);
                if (null != m__CharacterRenderTexture)
                {
                    UIRenderTexture rt = m__CharacterRenderTexture.GetComponent<UIRenderTexture>();
                    if (null == rt)
                    {
                        rt = m__CharacterRenderTexture.gameObject.AddComponent<UIRenderTexture>();
                    }
                    if (null != rt)
                    {
                        rt.SetDepth(0);
                        rt.Initialize(m_renderObj, m_renderObj.YAngle, new Vector2(700f, 700f));

                    }
                }
                else
                {
                    Engine.Utility.Log.Error("CreatePlayerView failed,UITexture null");
                    m_renderObj.Release();
                    m_renderObj = null;
                }

              
            }
        }
        else
        {
            m_renderObj.Enable(enable);
            m_renderObj.SetModelRotateY(-135);
            m_renderObj.PlayModelAni(Client.EntityAction.Stand);
        }
    }

    /// <summary>
    /// 设置初始角色旋转
    /// </summary>
    private void SetRotateYNormal()
    {
        if (null != m_renderObj)
        {
            m_renderObj.SetModelRotateY(-135);
        }
    }

    /// <summary>
    /// 设置物品活动页签
    /// </summary>
    /// <param name="itemType"></param>
    /// <param name="force"></param>
    private void SetKnapsackItemType(KnapsackItemType itemType, bool force = false)
    {
        if (m_em_itemType == itemType && !force)
        {
            return;
        }

        UITabGrid tab = null;
        if (null != m_dic_tabs && m_dic_tabs.TryGetValue((uint)m_em_itemType, out tab))
        {
            tab.SetHightLight(false);
        }
        m_em_itemType = itemType;
        if (null != m_dic_tabs && m_dic_tabs.TryGetValue((uint)m_em_itemType, out tab))
        {
            tab.SetHightLight(true);
        }
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
        CreateKnapsackGrid();
    }



    /// <summary>
    /// 设置背包活动页签
    /// </summary>
    /// <param name="type"></param>
    /// <param name="force"></param>
    private void SetKnapsackType(KnapsackType type, bool force = false)
    {
        if (m_em_kanpsackType == type && !force)
        {
            return;
        }

        UITabGrid tab = null;
        if (null != m_dic_tabs && m_dic_tabs.TryGetValue((uint)m_em_kanpsackType, out tab))
        {
            tab.SetHightLight(false);
        }
        m_em_kanpsackType = type;
        if (null != m_dic_tabs && m_dic_tabs.TryGetValue((uint)m_em_kanpsackType, out tab))
        {
            tab.SetHightLight(true);
        }
    }

    /// <summary>
    /// 设置玩家的战斗力
    /// </summary>
    private void SetPlayerFightPower()
    {
        if (null != m_label_fighelabel)
        {
            m_label_fighelabel.text =
                Client.ClientGlobal.Instance().MainPlayer.GetProp((int)Client.FightCreatureProp.Power).ToString();
        }

    }
    #endregion

    #region CarryShop 随身商店
    //KnapsackPanel_CarryShop.cs
    #endregion

    #region  Warehouse 仓库
    //KnapsackPanel_Warehouse.cs
    #endregion

    #region 批量分解
    //KnapsackPanel_BatchSplit.cs
    #endregion

    #region UIEvent

    /// <summary>
    /// 改名
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Btn_gaiming_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ReNamePanel);
    }

    //显示强化套装
    void onClick_BtnGridSuitNormal_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Strengthen,
            vec = new Vector3(133.8f, 201.31f, 0),
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }

    void onClick_BtnGridSuitActive_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Strengthen,
            vec = new Vector3(133.8f, 201.31f, 0),
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }
    void onClick_BtnColorSuitNormal_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Color,
            vec = new Vector3(133.8f, 201.31f, 0),
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }

    void onClick_BtnColorSuitActive_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Color,
            vec = new Vector3(133.8f, 201.31f, 0),
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }

    void onClick_BtnStoneSuitNormal_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Stone,
            vec = new Vector3(133.8f, 201.31f, 0),
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }

    void onClick_BtnStoneSuitActive_Btn(GameObject caster)
    {
        SuitPanelParam param = new SuitPanelParam()
        {
            m_type = SuitPanelParam.SuitType.Stone,
            vec = new Vector3(133.8f, 201.31f, 0),
        };
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GridStrengthenSuitPanel, data: param);
    }

    //整理
    void onClick_Arrange_Btn(GameObject caster)
    {
        DataManager.Manager<KnapsackManager>().Tidy();
    }

    //批量分解
    void onClick_Split_Btn(GameObject caster)
    {
        SetKnapsackStatus(KnapsackStatus.BatchSplit);
    }

    //修理所有
    void onClick_Repairs_Btn(GameObject caster)
    {
        List<uint> equipList = emgr.GetEquips(GameCmd.PACKAGETYPE.PACKAGETYPE_EQUIP);
        string msg = "";
        uint cost = emgr.GetEquipRepairCost(equipList).Num;
        if (null == equipList || equipList.Count == 0 || cost == 0)
        {
            msg = "没有装备要修理!";
            TipsManager.Instance.ShowTips(msg);
            return;
        }

        //msg = string.Format("是否花费{0}金币修理所有装备", cost);
        msg = string.Format("是否花费大量金币修理所有装备");
        TipsManager.Instance.ShowTipWindow(Client.TipWindowType.YesNO, msg,
            () =>
            {
                emgr.RepairEquip(equipList);
            }, null, title: "提示");
    }

    /// <summary>
    /// 商店返回
    /// </summary>
    /// <param name="caster"></param>
    void onClick_Back_Btn(GameObject caster)
    {
        SetKnapsackStatus(KnapsackStatus.CarryShop);
    }

    /// <summary>
    /// 随身商店出售
    /// </summary>
    /// <param name="caster"></param>
    void onClick_CarryShopSellBtn_Btn(GameObject caster)
    {
        SetKnapsackStatus(KnapsackStatus.CarryShopSell);
    }

    /// <summary>
    /// 随身商店确认出售
    /// </summary>
    /// <param name="caster"></param>
    void onClick_SellShopConfirmSellBtn_Btn(GameObject caster)
    {
        string msg = "请选择要出售的商品！";
        if (sellShopSelectIds.Count == 0)
        {
            msg = "请选择要出售的商品！";
            TipsManager.Instance.ShowTipWindow(Client.TipWindowType.Ok, msg, null, title: "提示");
        }
        else
        {
            List<uint> sellList = sellShopSelectIds.Keys.ToList();
            msg = string.Format("确定要出售{0}组物品吗！", sellList.Count);
            TipsManager.Instance.ShowTipWindow(Client.TipWindowType.YesNO, msg,
            () =>
            {
                ResetGradeFilterMask();
                ResetQualityFilterMask();
                DataManager.Manager<MallManager>().Sell(sellList);
            }, null, title: "提示");
        }
    }

    /// <summary>
    /// 格子UI事件响应回调
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnUIEventCallback(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UIItemGrid)
                    {
                        UIItemGrid grid = data as UIItemGrid;
                        if (!grid.Empty)
                        {
                            BaseItem baseItem = imgr.GetBaseItemByQwThisId(grid.Data.QWThisID);
                            if (null == baseItem)
                            {
                                return;
                            }
                            switch (m_em_curStatus)
                            {
                                case KnapsackStatus.WareHouse:
                                case KnapsackStatus.WareHouseNPC:
                                    if (baseItem.CanStore2WareHouse)
                                    {
                                        m_kmgr.MoveItems(grid.Data.QWThisID, m_em_activeWareHouse);
                                    }
                                    else
                                    {
                                        TipsManager.Instance.ShowTips("该物品无法存入仓库");
                                    }
                                    break;
                                case KnapsackStatus.BatchSplit:
                                    BaseEquip baseEquip = imgr.GetBaseItemByQwThisId<BaseEquip>((grid.Data.QWThisID));
                                    if (null != baseEquip && !IsBatchSplitSelectItem(grid.Data.QWThisID)
                                        && !baseEquip.CanSplit)
                                    {
                                        TipsManager.Instance.ShowTips("该道具无法进行分解！");
                                        return;
                                    }
                                    OnItemGridCheckStateChange(grid, !IsBatchSplitSelectItem(grid.Data.QWThisID));
                                    if (null != m_ctor_ItemGridScrollView
                                        && null != gridDataList && gridDataList.Contains(grid.Data.QWThisID))
                                    {
                                        m_ctor_ItemGridScrollView.UpdateData(gridDataList.IndexOf(grid.Data.QWThisID));
                                    }
                                    break;
                                case KnapsackStatus.CarryShopSell:
                                    {
                                        if (!baseItem.CanSell2NPC)
                                        {
                                            if (IsSellShopSelectItem(grid.Data.QWThisID))
                                                OnItemGridCheckStateChange(grid, false);
                                            TipsManager.Instance.ShowTips("该物品无法出售");
                                            return;
                                        }
                                        else
                                        {
                                            OnItemGridCheckStateChange(grid, !IsSellShopSelectItem(grid.Data.QWThisID));
                                            if (null != m_ctor_ItemGridScrollView
                                                && null != gridDataList && gridDataList.Contains(grid.Data.QWThisID))
                                            {
                                                m_ctor_ItemGridScrollView.UpdateData(gridDataList.IndexOf(grid.Data.QWThisID));
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    imgr.OnUIItemGridClicked(grid.Data.QWThisID);
                                    break;
                            }
                        }
                        else if (grid.IsLock)
                        {
                            KnapsackDefine.LocalUnlockInfo unlcokInfo
                                = DataManager.Manager<KnapsackManager>().GetUnlockInfoByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
                            int unlockNum = ((int)ItemDefine.TransformServerLocation2Local(grid.Location).Position.y + 1) - unlcokInfo.UnlockNum;
                            if (unlockNum == 0)
                            {
                                Engine.Utility.Log.Warning(CLASS_NAME + "-> unlock grid failed,num = {0} data error!", unlockNum);
                                return;
                            }
                            //解锁格子
                            DoUnlocKnapsackGrid(unlockNum, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
                        }
                    }
                    else if (data is UITabGrid)
                    {
                        UITabGrid tabGRid = data as UITabGrid;

                        if (tabGRid.Data is KnapsackItemType)
                        {
                            if (IsKnapsackStatus(KnapsackStatus.BatchSplit))
                            {
                                return;
                            }
                            KnapsackItemType iType = (KnapsackItemType)tabGRid.Data;
                            if (iType != m_em_itemType)
                            {
                                SetKnapsackItemType(iType);
                            }

                        }
                        else if (tabGRid.Data is KnapsackType)
                        {
                            KnapsackType iType = (KnapsackType)tabGRid.Data;
                            if (iType != m_em_kanpsackType)
                            {
                                SetKnapsackType(iType);
                                if (iType == KnapsackType.Knapsack)
                                {
                                    SetKnapsackStatus(KnapsackStatus.Normal);
                                }
                                else if (iType == KnapsackType.Warehouse)
                                {
                                    SetKnapsackStatus(KnapsackStatus.WareHouse);
                                }
                                else if (iType == KnapsackType.CarryShop)
                                {
                                    SetKnapsackStatus(KnapsackStatus.CarryShop);
                                }
                            }

                        }
                    }
                }
                break;
            case UIEventType.LongPress:
                {
                    if (data is UIItemGrid)
                    {
                        UIItemGrid iGrid = data as UIItemGrid;
                        if (!IsKnapsackStatus(KnapsackStatus.Normal)
                            && !IsKnapsackStatus(KnapsackStatus.CarryShop)
                            && !iGrid.Empty && !iGrid.IsLock)
                        {
                            imgr.OnUIItemGridClicked(iGrid.Data.QWThisID);
                        }
                    }
                }
                break;
        }
    }

    /// <summary>
    /// 背包格子CheckBoxState改变回调
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="value"></param>
    private void OnItemGridCheckStateChange(UIItemGrid grid, bool value)
    {
        if (IsKnapsackStatus(KnapsackStatus.CarryShopSell))
        {
            UpdateSellShopSelectData(grid.Data.QWThisID, value, () =>
            {
                TipsManager.Instance.ShowTips("无法选择该道具，已达可选单次最大出售数量！");
            });
        }
        else if (IsKnapsackStatus(KnapsackStatus.BatchSplit))
        {
            UpdateBatchSplitSelectData(grid.Data.QWThisID, value, () =>
                {
                    TipsManager.Instance.ShowTips("无法选择该道具，已达可选最大分解数量！");
                });
        }

    }

    /// <summary>
    /// 装备格子事件回调
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnEquipGridEventCallback(UIEventType eventType, object data, object param)
    {
        switch (eventType)
        {
            case UIEventType.Click:
                {
                    UIEquipGrid equipGrid = data as UIEquipGrid;
                    if (null != equipGrid.Data)
                        imgr.OnUIItemGridClicked(equipGrid.Data.QWThisID);
                    else
                    {
                        Engine.Utility.Log.Info("Equip pos is null");
                    }
                }
                break;
        }
    }
    #endregion

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

    #region GuideTargetObj

    /// <summary>
    /// 根据物品唯一id获取目标格子对象
    /// </summary>
    /// <param name="qwThisIds"></param>
    /// <returns></returns>
    public GameObject GetGuideTargetObjByQwThisId(uint qwThisIds)
    {
        if (null != gridDataList && null != m_ctor_ItemGridScrollView && gridDataList.Contains(qwThisIds))
        {
            UIGridBase grid = m_ctor_ItemGridScrollView.GetGrid(gridDataList.IndexOf(qwThisIds));
            if (null != grid)
            {
                return grid.gameObject;
            }
        }
        return null;
    }
    #endregion

}