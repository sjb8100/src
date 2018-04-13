/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Knapsack
 * 创建人：  wenjunhua.zqgame
 * 文件名：  KnapsackPanel_Warehouse
 * 版本号：  V1.0.0.0
 * 创建时间：10/15/2016 12:47:11 PM
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
    /// <summary>
    /// 仓库状态
    /// </summary>
    //仓库物品列表
    private List<uint> wareHouseItems = null;
    //仓库tab
    private Dictionary<GameCmd.PACKAGETYPE, UITabGrid> m_dic_wareHouseTabs = null;
    private GameCmd.PACKAGETYPE m_em_activeWareHouse = GameCmd.PACKAGETYPE.PACKAGETYPE_NONE;
    #endregion

    #region init
    /// <summary>
    /// 初始化仓库
    /// </summary>
    private void InitWareHouse()
    {
        if (IsInitMode(KnapsackStatus.WareHouse))
        {
            return;
        }
        SetInitMode(KnapsackStatus.WareHouse);
        Transform ts = null;
        UITabGrid tab = null;
        m_dic_wareHouseTabs = new Dictionary<GameCmd.PACKAGETYPE, UITabGrid>();
        if (null != m_trans_WareHouseContent)
        {
            for (GameCmd.PACKAGETYPE i = GameCmd.PACKAGETYPE.PACKAGETYPE_STORE1;
            i <= GameCmd.PACKAGETYPE.PACKAGETYPE_STORE3; i++)
            {
                ts = m_trans_WareHouseContent.Find(GetWareHouseTabNameByStore(i));
                if (null == ts)
                {
                    continue;
                }
                tab = ts.GetComponent<UITabGrid>();
                if (null == tab)
                {
                    tab = ts.gameObject.AddComponent<UITabGrid>();
                }
                if (null != tab)
                {
                    tab.SetGridData(i);
                    tab.RegisterUIEventDelegate(OnWareHouseGridUIEvent);
                    tab.SetHightLight(false);
                    m_dic_wareHouseTabs.Add(i, tab);
                }
            }
        }
        wareHouseItems = new List<uint>();
        if (null != m_ctor_WareHouseItemGridScrollView && null != m_trans_UIItemGrid)
        {
            GameObject wareHosueGridClone = UIManager.GetResGameObj(GridID.Uiitemgrid) as GameObject;
            m_ctor_WareHouseItemGridScrollView.RefreshCheck();
            //m_ctor_WareHouseItemGridScrollView.Initialize<UIItemGrid>((uint)GridID.Uiitemgrid
            //    , UIManager.OnObjsCreate, UIManager.OnObjsRelease
            //    , OnWareHouseGridDataUpdate, OnWareHouseGridUIEvent);

            m_ctor_WareHouseItemGridScrollView.Initialize<UIItemGrid>(m_trans_UIItemGrid.gameObject
                , OnWareHouseGridDataUpdate, OnWareHouseGridUIEvent);
        }
    }

    private void UpdateWareHouseCreator()
    {
        if (null != m_ctor_WareHouseItemGridScrollView)
        {
            m_ctor_WareHouseItemGridScrollView.UpdateActiveGridData();
        }
    }
    public void SetWareHousePackage(GameCmd.PACKAGETYPE ptype,bool force = false)
    {
        if (!m_kmgr.IsWareHosue(ptype) || (m_em_activeWareHouse == ptype && !force))
        {
            return;
        }
       
        if (null != m_dic_wareHouseTabs )
        {
            UITabGrid tab = null;
            if (m_dic_wareHouseTabs.TryGetValue(m_em_activeWareHouse, out tab))
            {
                tab.SetHightLight(false);
            }
            if (m_dic_wareHouseTabs.TryGetValue(ptype, out tab))
            {
                tab.SetHightLight(true);
            }
        }
        m_em_activeWareHouse = ptype;
        CreateWareHouse();
        UpdateWareHouseCapcity();
        UpdateWareHouseStoreCopperNum(m_kmgr.WareHouseStoreCopperNum);
    }

    /// <summary>
    /// 重置仓库状态
    /// </summary>
    public void ResetWareHouse()
    {
        if (IsInitMode(KnapsackStatus.WareHouse))
        {
            wareHouseItems.Clear();
        }
    }
    #endregion

    #region Op

    /// <summary>
    /// 解锁仓库
    /// </summary>
    /// <param name="unlockWareHouse"></param>
    private void OnUnlockWareHosue(GameCmd.PACKAGETYPE unlockWareHouse)
    {
        if (IsKnapsackStatus(KnapsackStatus.WareHouse)
            || IsKnapsackStatus(KnapsackStatus.WareHouseNPC))
        {

        }
    }

    private void UpdateWareHouseStatus()
    {
        if (!IsInitMode(KnapsackStatus.WareHouse))
        {
            return;
        }
        if (IsKnapsackStatus(KnapsackStatus.WareHouseNPC) 
            || IsKnapsackStatus(KnapsackStatus.WareHouse))
        {
            if (null != m_dic_wareHouseTabs)
            {
                UITabGrid tab = null;
                KnapsackDefine.LocalUnlockInfo localInfo = null;
                GameObject obj = null;
                bool visible = false;
                if (m_dic_wareHouseTabs.TryGetValue(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE1,out tab))
                {
                    obj = tab.CacheTransform.Find("Content/Toggle/Lock").gameObject;
                    if (null != obj)
                    {
                        localInfo = m_kmgr.GetUnlockInfoByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE1);
                        visible = (null != localInfo) ? localInfo.IsUnlock : false;
                        if (obj.activeSelf == visible)
                        {
                            obj.SetActive(!visible);
                        }
                    }
                }
                if (m_dic_wareHouseTabs.TryGetValue(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE2,out tab))
                {
                    obj = tab.CacheTransform.Find("Content/Toggle/Lock").gameObject;
                    if (null != obj)
                    {
                        localInfo = m_kmgr.GetUnlockInfoByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE2);
                        visible = (null != localInfo) ? localInfo.IsUnlock : false;
                        if (obj.activeSelf == visible)
                        {
                            obj.SetActive(!visible);
                        }
                    }
                }
                if (m_dic_wareHouseTabs.TryGetValue(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE3,out tab))
                {
                    obj = tab.CacheTransform.Find("Content/Toggle/Lock").gameObject;
                    if (null != obj)
                    {
                        localInfo = m_kmgr.GetUnlockInfoByPackageType(GameCmd.PACKAGETYPE.PACKAGETYPE_STORE3);
                        visible = (null != localInfo) ? localInfo.IsUnlock : false;
                        if (obj.activeSelf == visible)
                        {
                            obj.SetActive(!visible);
                        }
                    }
                }
            }
            
        }
    }

    /// <summary>
    /// 获取仓库tabname
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    private string GetWareHouseTabNameByStore(GameCmd.PACKAGETYPE pType)
    {
        string tabName = "unknow";
        switch (pType)
        {
            case GameCmd.PACKAGETYPE.PACKAGETYPE_STORE1:
                tabName = "WareHousebookmarktoggle/WareHouseToggle1";
                break;
            case GameCmd.PACKAGETYPE.PACKAGETYPE_STORE2:
                tabName = "WareHousebookmarktoggle/WareHouseToggle2";
                break;
            case GameCmd.PACKAGETYPE.PACKAGETYPE_STORE3:
                tabName = "WareHousebookmarktoggle/WareHouseToggle3";
                break;
        }
        return tabName;

    }

    /// <summary>
    /// 刷新仓库
    /// </summary>
    private void CreateWareHouse()
    {
        if (!(IsInitMode(KnapsackStatus.WareHouse) || IsInitMode(KnapsackStatus.WareHouseNPC))
            || null == m_ctor_WareHouseItemGridScrollView)
        {
            
            return;
        }
        wareHouseItems.Clear();
        List<uint> wareHouseItemDatas =  imgr.GetItemDataByPackageType(m_em_activeWareHouse);
        Dictionary<uint, uint> gridDataDic = new Dictionary<uint, uint>();
        BaseItem data = null;
        if (null != wareHouseItemDatas && wareHouseItemDatas.Count > 0)
        {
            for (int i = 0; i < wareHouseItemDatas.Count; i++)
            {
                data = imgr.GetBaseItemByQwThisId(wareHouseItemDatas[i]);
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
        }
        //重新构建格子列表
        KnapsackDefine.LocalUnlockInfo wareHouseUnlockInfo
            = m_kmgr.GetUnlockInfoByPackageType(m_em_activeWareHouse);
        if (null == wareHouseUnlockInfo)
        {
            Engine.Utility.Log.Error("CreateWareHouse failed,wareHouseUnlockInfo null");
            m_ctor_WareHouseItemGridScrollView.CreateGrids(0);
            return;
        }
        int mainPackMaxGridHave = m_kmgr.GetMaxGridHaveByPackageType(m_em_activeWareHouse);
        int max = wareHouseUnlockInfo.UnlockNum;
        int mod = max % KnapsackManager.KNAPSACK_GRID_COLUMN_MAX;
        int lockNum = ((mod != 0) ? (KnapsackManager.KNAPSACK_GRID_COLUMN_MAX * 2 - mod) : KnapsackManager.KNAPSACK_GRID_COLUMN_MAX);
        lockNum = Math.Min(lockNum, mainPackMaxGridHave - max);
        uint combineLocation = 0;
        int needNum = max + lockNum;
        if (needNum < 25)
        {
            needNum = 25;
        }
        for (int i = 0; i < needNum; i++)
        {
            combineLocation = ItemDefine.TransformLocal2ServerLocation(m_em_activeWareHouse, new Vector2(0, i));
            wareHouseItems.Add((gridDataDic.ContainsKey(combineLocation)) ? gridDataDic[combineLocation] : 0);
        }
        //创建格子数据
        m_ctor_WareHouseItemGridScrollView.CreateGrids(wareHouseItems.Count);
        UpdateWareHouseStatus();
        if (m_dic_cacheOldUnlock.ContainsKey(m_em_activeWareHouse))
        {
            m_dic_cacheOldUnlock[m_em_activeWareHouse] = wareHouseUnlockInfo.UnlockNum;
        }
        else
        {
            m_dic_cacheOldUnlock.Add(m_em_activeWareHouse, wareHouseUnlockInfo.UnlockNum);
        }
    }

    /// <summary>
    /// 仓库数据
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    public void OnWareHouseGridDataUpdate(UIGridBase grid, int index)
    {
        KnapsackManager knapMgr = DataManager.Manager<KnapsackManager>();
        uint location = ItemDefine.TransformLocal2ServerLocation(m_em_activeWareHouse, new Vector2(0, index));
        BaseItem itemData = (wareHouseItems.Count > index) ? imgr.GetBaseItemByQwThisId(wareHouseItems[index]) : null;
        UIItemGrid itemGrid = grid as UIItemGrid;
        itemGrid.EnableCheckBox(false);
        if (null == itemData)
        {
            itemGrid.SetLocation(location);
            //清空
        }
        itemGrid.SetLock(!knapMgr.IsGridUnlock(location));
        itemGrid.SetGridData(UIItemInfoGridBase.InfoGridType.None,itemData);
    }



    /// <summary>
    /// 仓库格子点击实现
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    public void OnWareHouseGridUIEvent(UIEventType eventType, object data, object param)
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
                            DataManager.Manager<KnapsackManager>().MoveItems(grid.Data.QWThisID, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN);
                        }
                        else if (grid.IsLock)
                        {
                            KnapsackDefine.LocalUnlockInfo unlcokInfo
                                = m_kmgr.GetUnlockInfoByPackageType(m_em_activeWareHouse);
                            int unlockNum = ((int)ItemDefine.TransformServerLocation2Local(grid.Location).Position.y + 1) - unlcokInfo.UnlockNum;
                            if (unlockNum == 0)
                            {
                                Engine.Utility.Log.Warning(CLASS_NAME + "-> unlock grid failed,num = {0} data error!", unlockNum);
                                return;
                            }
                            //解锁格子
                            DoUnlocKnapsackGrid(unlockNum, m_em_activeWareHouse);
                        }
                    }else if (data is UITabGrid)
                    {
                        UITabGrid tabGrid = data as UITabGrid;
                        KnapsackDefine.LocalUnlockInfo localInfo
                            = m_kmgr.GetUnlockInfoByPackageType((GameCmd.PACKAGETYPE)tabGrid.Data);
                        if (null != localInfo && localInfo.IsUnlock)
                        {
                            SetWareHousePackage((GameCmd.PACKAGETYPE)tabGrid.Data);
                        }
                        else
                        {
                            TipsManager.Instance.ShowTips("购买皇令解锁仓库");
                        }
                    }
                    
                }
                break;
            case UIEventType.LongPress:
                {
                    if (data is UIItemGrid)
                    {
                        UIItemGrid grid = data as UIItemGrid;
                        if (!grid.Empty)
                        {
                            imgr.OnUIItemGridClicked(grid.Data.QWThisID);
                        }
                    }
                }
               break;
        }
    }

    /// <summary>
    /// 更新容量
    /// </summary>
    private void UpdateWareHouseCapcity()
    {
        if (null != m_label_WareHouseCapacityNum)
        {
            KnapsackDefine.LocalUnlockInfo wareHouseUnlockInfo =
                m_kmgr.GetUnlockInfoByPackageType(m_em_activeWareHouse);
            if (null == wareHouseUnlockInfo)
            {
                return;
            }
            int mainPackMaxGridHave = m_kmgr.GetMaxGridHaveByPackageType(m_em_activeWareHouse);
            int unlockNum = wareHouseUnlockInfo.UnlockNum;
            m_label_WareHouseCapacityNum.text = string.Format("{0}/{1}", unlockNum, mainPackMaxGridHave);
        }
    }
    
    /// <summary>
    /// 更新仓库存储金币数量
    /// </summary>
    /// <param name="num"></param>
    public void UpdateWareHouseStoreCopperNum(uint num)
    {
        if (null != m_label_WareHouseStoreCopperNum)
        {
            m_label_WareHouseStoreCopperNum.text = "" + num;
        }
    }

    #endregion

    #region UIEvent

    /*******************仓库UI事件************************/
    //仓库整理
    void onClick_WareHouseArrangeBtn_Btn(GameObject caster)
    {
        DataManager.Manager<KnapsackManager>().Tidy(m_em_activeWareHouse);
    }

    //仓库存入金币
    void onClick_WareHouseStoreCopperBtn_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GoldStoreGetPanel);
    }

    #endregion
}