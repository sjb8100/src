using Client;
using GameCmd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using table;
using UnityEngine;
//*******************************************************************************************
//	创建日期：	2016-12-19   15:39
//	文件名称：	ShortcutsPanel,cs
//	创 建 人：	Lin Chuang Yuan
//	版权所有：	Lin Chuang Yuan
//	说    明：	设置快捷使用道具
//*******************************************************************************************

partial class SettingPanel : UIPanelBase
{

    #region property

    uint m_selectItemId = 0;

    ShortCuts m_selectSetItem = new ShortCuts { id = 0, itemid = 0 };

    List<uint> m_lstItem = new List<uint>();

    List<ShortCuts> m_lstShortcutSetItem = new List<ShortCuts>();

    UIGridCreatorBase m_itemListGridCreator = null;

    List<UIShortcutSetItemGrid> m_lstShortcutSetItemGrid = new List<UIShortcutSetItemGrid>();

    bool m_clickShortcutBefore = false; //是否点击快捷栏

    int m_fixedShortcutItemCount = 1;

    private UIItemInfoGrid m_baseGrid = null;

    #endregion


    #region method

    /// <summary>
    ///初始化道具快捷使用设置
    /// </summary>
    void InitShortcut()
    {
        if (true == m_trans_UseItemContent.gameObject.activeSelf)
        {
            InitItemGirdList();
            InitShortcutItemGridList();
            SetSelectItemGird(m_lstItem.Count > 0 ? m_lstItem[0] : 0);//默认选中第一个
            SetItemCount();
        }
    }


    /// <summary>
    /// 左侧的grid list
    /// </summary>
    void InitItemGirdList()
    {
        m_lstItem = DataManager.Manager<SettingManager>().GetItemList();

        //UnityEngine.GameObject obj = UIManager.GetResGameObj(GridID.Uishortcutablegrid) as UnityEngine.GameObject;
        m_itemListGridCreator = m_trans_ItemListScrollView.GetComponent<UIGridCreatorBase>();
        if (m_itemListGridCreator == null)
            m_itemListGridCreator = m_trans_ItemListScrollView.gameObject.AddComponent<UIGridCreatorBase>();
        m_itemListGridCreator.gridContentOffset = new UnityEngine.Vector2(-185, 193);
        m_itemListGridCreator.arrageMent = UIGridCreatorBase.Arrangement.Horizontal;
        m_itemListGridCreator.gridWidth = 97;
        m_itemListGridCreator.gridHeight = 97;
        m_itemListGridCreator.rowcolumLimit = 5;

        m_itemListGridCreator.RefreshCheck();
        m_itemListGridCreator.Initialize<UIShortcutAbleGrid>((uint)GridID.Uishortcutablegrid, UIManager.OnObjsCreate, UIManager.OnObjsRelease, OnItemGirdDataUpdate, OnItemGirdEventDlg);
        m_itemListGridCreator.CreateGrids((null != m_lstItem) ? m_lstItem.Count : 0);

    }

    /// <summary>
    /// 数据跟新
    /// </summary>
    /// <param name="data"></param>
    /// <param name="index"></param>
    void OnItemGirdDataUpdate(UIGridBase data, int index)
    {
        if (m_lstItem != null && index < m_lstItem.Count)
        {
            UIShortcutAbleGrid grid = data as UIShortcutAbleGrid;
            if (grid == null)
            {
                return;
            }
            BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_lstItem[index]);

            grid.SetGridData(baseItem);
        }
    }

    /// <summary>
    /// 左侧物品栏点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnItemGirdEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIShortcutAbleGrid grid = data as UIShortcutAbleGrid;
            if (grid == null)
            {
                return;
            }

            //物品栏高亮
            SetSelectItemGird(grid.Data.BaseId);

            //快捷栏设置高亮
            for (int i = 0; i < m_lstShortcutSetItemGrid.Count; i++)
            {
                if (m_lstShortcutSetItemGrid[i].itemData.itemid != 0)
                {
                    m_lstShortcutSetItemGrid[i].SetSelect(true);
                }
                else
                {
                    m_lstShortcutSetItemGrid[i].SetSelect(false);
                }
            }

            //UIShortcutSetItemGrid setItemGrid = m_lstShortcutSetItemGrid.Find((d) => { return d.itemData.itemid == grid.Data.BaseId; });
            //if (setItemGrid != null)
            //{
            //    ShortCuts itemData = new ShortCuts { id = setItemGrid.itemData.id, itemid = setItemGrid.itemData.itemid };
            //    SetSelectSetItemGrid1(itemData);
            //}
            //else
            //{
            //    CleanSetItemGridSelectState();
            //}

            m_clickShortcutBefore = false;
        }
    }

    /// <summary>
    /// 道具详情
    /// </summary>
    /// <param name="itemId"></param>
    void ItemInfoDisplay(uint itemId)
    {
        ItemDataBase itemDataBase = GameTableManager.Instance.GetTableItem<ItemDataBase>(itemId);

        if (itemDataBase == null)
        {
            return;
        }

        if (m_trans_ItemInfoRoot.childCount == 0)
        {
            GameObject preObj = UIManager.GetResGameObj(GridID.Uiiteminfogrid) as GameObject;
            GameObject cloneObj = NGUITools.AddChild(m_trans_ItemInfoRoot.gameObject, preObj);
            if (null != cloneObj)
            {
                m_baseGrid = cloneObj.GetComponent<UIItemInfoGrid>();
                if (null == m_baseGrid)
                {
                    m_baseGrid = cloneObj.AddComponent<UIItemInfoGrid>();
                }
            }
        }


        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(itemDataBase.itemID);
        int itemCount = DataManager.Manager<ItemManager>().GetItemNumByBaseId(itemDataBase.itemID);//道具存量

        m_baseGrid.Reset();
        m_baseGrid.SetBorder(true, baseItem.BorderIcon);
        m_baseGrid.SetIcon(true, baseItem.Icon);
        m_baseGrid.SetNum(true, itemCount.ToString());
        if (itemCount < 1)
        {
            m_baseGrid.SetNotEnoughGet(true);
            m_baseGrid.RegisterUIEventDelegate(UIItemInfoEventDelegate);
        }
        else
        {
            m_baseGrid.SetNotEnoughGet(false);
            m_baseGrid.UnRegisterUIEventDelegate();
        }

        m_label_ItemName.text = itemDataBase.itemName;
        m_label_ItemDes.text = itemDataBase.description;
        m_label_ItemLevel.text = string.Format("物品使用等级：{0}", itemDataBase.useLevel);
    }

    void UIItemInfoEventDelegate(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.GetWayPanel, data: m_selectItemId);
        }
    }


    void SetSelectSetItemGrid1(ShortCuts item)
    {
        //CleanSetItemGridSelectState();

        UIShortcutSetItemGrid newSetItemGrid = m_lstShortcutSetItemGrid.Find((data) => { return data.itemData.itemid == item.itemid && data.itemData.id == item.id; });
        if (newSetItemGrid != null)
        {
            newSetItemGrid.SetSelect(true);
        }
    }

    /// <summary>
    /// 设置选中的
    /// </summary>
    /// <param name="itemId"></param>
    void SetSelectItemGird(uint itemId)
    {
        UIShortcutAbleGrid grid = m_itemListGridCreator.GetGrid<UIShortcutAbleGrid>(m_lstItem.IndexOf(m_selectItemId));
        if (grid != null)
        {
            grid.SetItemHighLight(false);
        }

        grid = m_itemListGridCreator.GetGrid<UIShortcutAbleGrid>(m_lstItem.IndexOf(itemId));
        if (grid != null)
        {
            grid.SetItemHighLight(true);
        }

        ItemInfoDisplay(itemId);

        m_selectItemId = itemId;
    }


    /// <summary>
    /// 快捷栏的item List
    /// </summary>
    void InitShortcutItemGridList()
    {
        m_lstShortcutSetItemGrid.Clear();  //用于UI缓存的grid

        m_lstShortcutSetItem = DataManager.Manager<SettingManager>().GetShortcutSetItemList();   //我们需要的数据list

        m_fixedShortcutItemCount = DataManager.Manager<SettingManager>().FixedShortcutItemCount;

        for (int i = 0; i < m_trans_CoteContent.childCount; i++)
        {
            Transform gridTransf = m_trans_CoteContent.GetChild(i);

            UIShortcutSetItemGrid grid = gridTransf.gameObject.GetComponent<UIShortcutSetItemGrid>();
            if (grid == null)
            {
                grid = gridTransf.gameObject.AddComponent<UIShortcutSetItemGrid>();
            }

            if (i < m_lstShortcutSetItem.Count)
            {
                grid.SetGridData(m_lstShortcutSetItem[i]);
                grid.RegisterUIEventDelegate(OnShortcutItemGirdEventDlg);

                m_lstShortcutSetItemGrid.Add(grid); //用于UI缓存的grid
            }
        }
    }

    /// <summary>
    /// 快捷栏点击事件
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnShortcutItemGirdEventDlg(UIEventType eventType, object data, object param)
    {
        if (eventType == UIEventType.Click)
        {
            UIShortcutSetItemGrid grid = data as UIShortcutSetItemGrid;
            if (grid == null)
            {
                return;
            }

            //如果点击的是grid的移除按钮
            if (param != null)
            {
                int removeBtn = (int)param;
                if (removeBtn == 1)
                {
                    RemoveSetItem(grid);
                    return;
                }
            }

            //点的是grid
            //第二次点击取消选中
            //if (grid.clicks >= 1)
            //{
            //    CleanSetItemGridSelectState();
            //    this.m_selectSetItem = new ShortCuts { id = 0, itemid = 0 };
            //    return;
            //}

            //快捷栏选中
            //SetSelectSetItemGrid(grid.itemData);

            //设置
            if (m_clickShortcutBefore == false) 
            {
                ShortCuts itemData = new ShortCuts { id = grid.itemData.id, itemid = m_selectItemId };
                UpdateShortcutSetItemGridList(grid.itemData, itemData);
            }

            /*
            //1、上次点的是物品栏，2、快捷栏为空
            if (m_clickShortcutBefore == false && grid.itemData.itemid == 0)
            {
                ShortCuts itemData = new ShortCuts { id = grid.itemData.id, itemid = m_selectItemId };
                grid.SetGridData(itemData);
            }

            //1、上次点的是物品栏，2、对应快捷栏不为空或为空
            UIShortcutSetItemGrid tempGrid = m_lstShortcutSetItemGrid.Find((gridData) => { return gridData.itemData.itemid == m_selectItemId; });
            if (m_clickShortcutBefore == false)
            {
                if (tempGrid != null) //对应快捷栏不为空
                {
                    ExchangeItem(grid.itemData.id, grid.itemData.itemid, tempGrid.itemData.id, tempGrid.itemData.itemid);
                }
                else //对应快捷栏为空
                {
                    ShortCuts itemData = new ShortCuts { id = grid.itemData.id, itemid = m_selectItemId };
                    grid.SetGridData(itemData);
                }
            }
            */

            //上次点击的是快捷栏，交换
            //if (m_clickShortcutBefore == true)
            //{
            //    UIShortcutSetItemGrid oldSetItemGrid = m_lstShortcutSetItemGrid.Find((d) => { return d.itemData.itemid == m_selectSetItem.itemid && d.itemData.id == m_selectSetItem.id; });
            //    if (oldSetItemGrid != null)
            //    {
            //        ExchangeItem(grid.itemData.id, grid.itemData.itemid, oldSetItemGrid.itemData.id, oldSetItemGrid.itemData.itemid);
            //    }
            //}

            //设置物品栏选中
            if (grid.itemData.itemid != 0)
            {
                SetSelectItemGird(grid.itemData.itemid);
            }

            this.m_selectSetItem = grid.itemData;  //把最新选中的Item 赋值给m_selectSetItemId

            m_clickShortcutBefore = true;
        }
    }

    /// <summary>
    /// 快捷栏中设置选中 grid
    /// </summary>
    void SetSelectSetItemGrid(ShortCuts item)
    {
        //CleanSetItemGridSelectState();

        UIShortcutSetItemGrid newSetItemGrid = m_lstShortcutSetItemGrid.Find((data) => { return data.itemData.itemid == item.itemid && data.itemData.id == item.id; });
        if (newSetItemGrid != null)
        {
            newSetItemGrid.SetSelect(true);
            newSetItemGrid.clicks++;
        }

    }

    /// <summary>
    /// 交换
    /// </summary>
    void ExchangeItem(uint newIndex, uint newItemId, uint oldIndex, uint oldItemId)
    {
        //oldItem  赋最新点击的值
        UIShortcutSetItemGrid oldSetItemGrid = m_lstShortcutSetItemGrid.Find((data) => { return data.itemData.id == oldIndex; });
        if (oldSetItemGrid != null)
        {
            ShortCuts itemData = new ShortCuts { id = oldIndex, itemid = newItemId };
            oldSetItemGrid.SetGridData(itemData);
        }

        //newItem   赋上次点击的值
        UIShortcutSetItemGrid newSetItemGrid = m_lstShortcutSetItemGrid.Find((data) => { return data.itemData.id == newIndex; });
        if (newSetItemGrid != null)
        {
            ShortCuts itemData = new ShortCuts { id = newIndex, itemid = oldItemId };
            newSetItemGrid.SetGridData(itemData);
        }
    }

    /// <summary>
    /// 清除快捷栏选中状态
    /// </summary>
    //void CleanSetItemGridSelectState()
    //{
    //    for (int i = 0; i < m_lstShortcutSetItemGrid.Count; i++)
    //    {
    //        m_lstShortcutSetItemGrid[i].SetSelect(false);
    //        m_lstShortcutSetItemGrid[i].clicks = 0;
    //    }
    //}

    //void CleanOtherSetItemGridSelectClicks(ShortCuts item)
    //{
    //    for (int i = 0; i < m_lstShortcutSetItemGrid.Count; i++)
    //    {
    //        if (item.id == m_lstShortcutSetItemGrid[i].itemData.id)
    //        {
    //            continue;
    //        }
    //        m_lstShortcutSetItemGrid[i].clicks = 0;
    //    }
    //}

    void RemoveSetItem(UIShortcutSetItemGrid grid)
    {
        ShortCuts itemData = new ShortCuts { id = grid.itemData.id, itemid = 0 };
        grid.SetGridData(itemData);

        for (int i = 0; i < m_lstShortcutSetItemGrid.Count;i++)
        {
            if (m_lstShortcutSetItemGrid[i].itemData.id == grid.itemData.id)
            {
                m_lstShortcutSetItemGrid[i].itemData.itemid = 0;
            }
        }    
    }


    //设置常驻栏 活动栏
    void SetItemCount()
    {
        //m_slider_Shortcut_SetItemCountSlider.numberOfSteps = 5;  //设置间隔 0、1、2、3、4
        m_slider_Shortcut_SetItemCountSlider.onDragFinished = OnSliderDragFinished;
    }

    void UpdateShortcutSetItemGridList(ShortCuts oldItemData ,ShortCuts itemData)
    {
        UIShortcutSetItemGrid grid = m_lstShortcutSetItemGrid.Find((d) => { return d.itemData.itemid == itemData.itemid; });

        if (grid != null)
        {
            if (grid.itemData.id != itemData.id)
            {
                ShortCuts tempItem = new ShortCuts();
                tempItem.id = grid.itemData.id;
                tempItem.itemid = oldItemData.itemid;

                //点到的grid设置为最新
                UIShortcutSetItemGrid tempGrid = m_lstShortcutSetItemGrid.Find((d) => { return d.itemData.id == itemData.id; });
                tempGrid.SetGridData(itemData);

                //已经存在的，设置为点到的grid之前的数据
                grid.SetGridData(tempItem);

                for (int i = 0; i < m_lstShortcutSetItemGrid.Count; i++)
                {
                    if (m_lstShortcutSetItemGrid[i].itemData.id == itemData.id)
                    {
                        m_lstShortcutSetItemGrid[i].itemData.itemid = itemData.itemid;
                    }

                    if (m_lstShortcutSetItemGrid[i].itemData.id == tempItem.id)
                    {
                        m_lstShortcutSetItemGrid[i].itemData.itemid = tempItem.itemid;
                    }
                }
            }
        }
        else 
        {
            //找到当前位置
            UIShortcutSetItemGrid newGrid = m_lstShortcutSetItemGrid.Find((d) => { return d.itemData.id == itemData.id;});
            newGrid.SetGridData(itemData);

            for (int i = 0; i < m_lstShortcutSetItemGrid.Count; i++)
            {
                if (m_lstShortcutSetItemGrid[i].itemData.id == itemData.id)
                {
                    m_lstShortcutSetItemGrid[i].itemData.itemid = itemData.itemid;
                }
            }
        }


    }

    void OnSliderDragFinished()
    {
        float value = m_slider_Shortcut_SetItemCountSlider.value;
        if (value >= 0 && value < 0.375)
        {
            value = 0.25f;
            m_slider_Shortcut_SetItemCountSlider.value = value;

        }

        m_fixedShortcutItemCount = (int)(value * 100 / 25);   //固定快捷道具数量
        //常驻栏数量label设置
        m_label_SetItemCountLabel.text = m_fixedShortcutItemCount.ToString();

        DataManager.Manager<SettingManager>().FixedShortcutItemCount = m_fixedShortcutItemCount;
        Engine.Utility.EventEngine.Instance().DispatchEvent((int)Client.GameEventID.UIEVENT_SHORTCUTITEMUI, m_fixedShortcutItemCount);
    }


    #endregion


    #region click

    /// <summary>
    /// 
    /// </summary>
    /// <param name="caster"></param>

    void SaveShortcutItemSetting()
    {
        //CleanSetItemGridSelectState();

       // m_lstShortcutSetItem = DataManager.Manager<SettingManager>().GetShortcutSetItemList();

        //m_lstShortcutSetItem.Clear();

        List<ShortCuts> tempList = new List<ShortCuts>();

        if (m_lstShortcutSetItemGrid.Count == 0)
        {
            return;
        }

        for (int i = 0; i < m_lstShortcutSetItemGrid.Count; i++)
        {
            ShortCuts itemData = m_lstShortcutSetItemGrid[i].itemData;
            tempList.Add(itemData);
        }

        DataManager.Manager<SettingManager>().ReqAllShortCutItemList(tempList);
    }

    #endregion


    void ReleaseShortCut(bool depthRelease = true)
    {
        if (null != m_itemListGridCreator)
        {
            m_itemListGridCreator.Release(depthRelease);
        }
    }

    void OnPanelBaseDestoryShortCut()
    {
        if (m_baseGrid != null)
        {
            m_baseGrid.Release(true);
            UIManager.OnObjsRelease(m_baseGrid.CacheTransform, (uint)GridID.Uiiteminfogrid);
            m_baseGrid = null;
        }
    }
}

