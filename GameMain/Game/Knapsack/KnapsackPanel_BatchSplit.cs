/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Knapsack
 * 创建人：  wenjunhua.zqgame
 * 文件名：  KnapsackPanel_BatchSplit
 * 版本号：  V1.0.0.0
 * 创建时间：10/15/2016 12:46:25 PM
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
    //装备分解获得金币id
    public const uint GOLD_SPLIT_GET_ID = 0;
    //已选中分解物品
    private Dictionary<uint, uint> m_dic_selectSplitIds = null;
    //分解获得物品字典
    private Dictionary<uint, uint> m_dic_splitGetItems = null;
    //自动选择5档一下装备
    private bool m_bool_autoSelectSplitEquip = false;
    //批量分解格子生成器
    //已选装备列表
    private const string SPLIT_SELECT_TEXT_FORMAT = "已选装备数量：{0}/{1}";
    #endregion

    #region init
    /// <summary>
    /// 初始化批量分解
    /// </summary>
    private void InitBatchSplit()
    {
        if (IsInitMode(KnapsackStatus.BatchSplit))
        {
            return;
        }
        SetInitMode(KnapsackStatus.BatchSplit);
        m_dic_selectSplitIds = new Dictionary<uint, uint>();
        m_dic_splitGetItems = new Dictionary<uint, uint>();

        if (null != m_ctor_SplitScrollView)
        {
            m_ctor_SplitScrollView.RefreshCheck();
            m_ctor_SplitScrollView.Initialize<UISplitGetGrid>((uint)GridID.Uisplitgetgrid
                , UIManager.OnObjsCreate, UIManager.OnObjsRelease
                , OnUpdateGridData, OnGridUIEventDlg);
        }
    }
    #endregion

    #region Op
    
    /// <summary>
    /// 重置分解
    /// </summary>
    private void ResetBatchSplit()
    {
        if (!IsInitMode(KnapsackStatus.BatchSplit))
        {
            return;
        }
        m_dic_selectSplitIds.Clear();
        m_dic_splitGetItems.Clear();
        m_bool_autoSelectSplitEquip = false;
        m_ctor_SplitScrollView.CreateGrids(0);
    }

    /// <summary>
    /// 进行自动勾选装备操作
    /// </summary>
    private void DoAutoSelectEquip()
    {
        if (null == gridDataList)
        {
            return;
        }
        BaseItem baseItem = null;
        for (int i = 0; i < gridDataList.Count;i++ )
        {
            baseItem = imgr.GetBaseItemByQwThisId<BaseItem>(gridDataList[i]);
            if (null != baseItem 
                && baseItem.IsEquip
                && baseItem.Grade <= ItemDefine.CONST_AUTO_SELECT_BATCH_ITEM_GRADE)
            {
                UpdateBatchSplitSelectData(gridDataList[i], m_bool_autoSelectSplitEquip);
            }
        }
    }
    /// <summary>
    /// 获取分解获得列表
    /// </summary>
    /// <returns></returns>
    private List<uint> GetSplitGetSortList()
    {
        List<uint> sortList = new List<uint>();
        sortList.AddRange(m_dic_splitGetItems.Keys);
        sortList.Sort();
        return sortList;
    }

    /// <summary>
    /// 更新批量分解数据
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <param name="select"></param>
    private void UpdateBatchSplitSelectData(uint qwThisId, bool select,Action fullDlg = null)
    {
        BaseEquip baseItem = imgr.GetBaseItemByQwThisId<BaseEquip>(qwThisId);
        if (!IsInitMode(KnapsackStatus.BatchSplit))
        {
            return ;
        }
       
        if (null != baseItem && !baseItem.CanSplit && select)
        {
            return;
        }

        bool needUpdateData = false;
        uint baseId = 0;
        if (IsBatchSplitSelectItem(qwThisId) && !select)
        {
            needUpdateData = true;
            baseId = m_dic_selectSplitIds[qwThisId];
            m_dic_selectSplitIds.Remove(qwThisId);
        }
        else if (!IsBatchSplitSelectItem(qwThisId) && select)
        {
            if (null != baseItem 
                && baseItem.IsBaseReady 
                && baseItem.IsEquip)
            {
                if (m_dic_selectSplitIds.Count < EquipManager.CONST_BATCH_SPLIT_NUM_MAX)
                {
                    needUpdateData = true;
                    m_dic_selectSplitIds.Add(qwThisId, baseItem.BaseId);
                    baseId = baseItem.BaseId;
                }else if (null != fullDlg)
                {
                    fullDlg.Invoke();
                }
                
            }
        }

        if (needUpdateData)
        {
            if (null != gridDataList && gridDataList.Contains(qwThisId))
            {
                UpdateKnapsackGrid(gridDataList.IndexOf(qwThisId));
            }
            IncrementalUpdateSplitGetData(baseId, select);
            UpdateBatchSplitInfo();
        }
    }

    /// <summary>
    /// 刷新批量分解数据
    /// </summary>
    private void UpdateBatchSplitInfo()
    {
        //刷新选中数量
        if (null != m_label_SpliteSelectEquipLab)
        {
            m_label_SpliteSelectEquipLab.text = string.Format(SPLIT_SELECT_TEXT_FORMAT, m_dic_selectSplitIds.Count, EquipManager.CONST_BATCH_SPLIT_NUM_MAX);
        }
        bool enableNoneNotice = (m_dic_selectSplitIds.Count == 0) ? true : false;
        //是否显示选择分解装备提示
        if (null != m_label_NoneSplitEquipNotice && m_label_NoneSplitEquipNotice.enabled != enableNoneNotice)
        {
            m_label_NoneSplitEquipNotice.enabled = enableNoneNotice;
        }
        if ( null !=  m_btn_AutoSelectEquip.GetComponent<UIToggle>())
        {
            m_btn_AutoSelectEquip.GetComponent<UIToggle>().value = m_bool_autoSelectSplitEquip;
        }
    }

    /// <summary>
    /// 增量更新分解获得道具
    /// </summary>
    /// <param name="qwThisId">装备唯一id</param>
    /// <param name="select">是否选择</param>
    private void IncrementalUpdateSplitGetData(uint baseId, bool select)
    {
        table.EquipSplitDataBase splitDB = imgr.GetLocalDataBase<table.EquipSplitDataBase>(baseId);
        if (null == splitDB)
        {
            Engine.Utility.Log.Error("IncrementalUpdateSplitGetData faield,unknow equip baseid = {0}", baseId);
            return;
        }
        int updateIndex = 0;
        List<uint> splitGetItems = GetSplitGetSortList();
        //金币刷新
        if (splitDB.splitGetMoney != 0)
        {
            if (splitGetItems.Contains(GOLD_SPLIT_GET_ID))
            {
                if (select)
                {
                    //更新
                    m_dic_splitGetItems[GOLD_SPLIT_GET_ID] += splitDB.splitGetMoney;
                    if (null != m_ctor_SplitScrollView)
                    {
                        m_ctor_SplitScrollView.UpdateData(splitGetItems.IndexOf(GOLD_SPLIT_GET_ID));
                    }
                }
                else
                {
                    if (m_dic_splitGetItems[GOLD_SPLIT_GET_ID] <= splitDB.splitGetMoney)
                    {
                        //移除
                        m_dic_splitGetItems.Remove(GOLD_SPLIT_GET_ID);
                        if (null != m_ctor_SplitScrollView)
                        {
                            m_ctor_SplitScrollView.RemoveData(splitGetItems.IndexOf(GOLD_SPLIT_GET_ID));
                        }
                    }
                    else
                    {
                        m_dic_splitGetItems[GOLD_SPLIT_GET_ID] -= splitDB.splitGetMoney;
                        //更新
                        if (null != m_ctor_SplitScrollView)
                        {
                            m_ctor_SplitScrollView.UpdateData(splitGetItems.IndexOf(GOLD_SPLIT_GET_ID));
                        }
                    }
                }
            }
            else if (select)
            {
                //插入
                m_dic_splitGetItems.Add(GOLD_SPLIT_GET_ID, splitDB.splitGetMoney);
                if (null != m_ctor_SplitScrollView)
                {
                    m_ctor_SplitScrollView.InsertData(0);
                }
            }
        }
        splitGetItems = GetSplitGetSortList();
        //道具刷新
        if (splitDB.splitGetItem != 0 && splitDB.splitGetItemNum > 0)
        {
            if (splitGetItems.Contains(splitDB.splitGetItem))
            {
                if (select)
                {
                    //更新
                    m_dic_splitGetItems[splitDB.splitGetItem] += splitDB.splitGetItemNum;
                    if (null != m_ctor_SplitScrollView)
                    {
                        m_ctor_SplitScrollView.UpdateData(splitGetItems.IndexOf(splitDB.splitGetItem));
                    }
                }
                else
                {
                    if (m_dic_splitGetItems[splitDB.splitGetItem] <= splitDB.splitGetItemNum)
                    {
                        //移除
                        m_dic_splitGetItems.Remove(splitDB.splitGetItem);
                        if (null != m_ctor_SplitScrollView)
                        {
                            m_ctor_SplitScrollView.RemoveData(splitGetItems.IndexOf(splitDB.splitGetItem));
                        }
                    }
                    else
                    {
                        m_dic_splitGetItems[splitDB.splitGetItem] -= splitDB.splitGetItemNum;
                        //更新
                        if (null != m_ctor_SplitScrollView)
                        {
                            m_ctor_SplitScrollView.UpdateData(splitGetItems.IndexOf(splitDB.splitGetItem));
                        }
                    }
                }
            }
            else if (select)
            {
                //插入
                m_dic_splitGetItems.Add(splitDB.splitGetItem, splitDB.splitGetItemNum);
                splitGetItems = GetSplitGetSortList();
                if (null != m_ctor_SplitScrollView)
                {
                    m_ctor_SplitScrollView.InsertData(splitGetItems.IndexOf(splitDB.splitGetItem));
                }
            }
        }
    }

    /// <summary>
    /// 是否批量分解选中该装备
    /// </summary>
    /// <param name="qwThisId"></param>
    /// <returns></returns>
    private bool IsBatchSplitSelectItem(uint qwThisId)
    {
        if (!IsInitMode(KnapsackStatus.BatchSplit))
        {
            return false;
        }
        return m_dic_selectSplitIds.ContainsKey(qwThisId);
    }

    /// <summary>
    /// 刷新批量分解
    /// </summary>
    private void CreateBatchSplit()
    {
        SetKnapsackItemType(KnapsackItemType.ItemEquipment);
        ResetBatchSplit();
        UpdateBatchSplitInfo();
    }

    /// <summary>
    /// 执行批量分解
    /// </summary>
    private void DoBatchSplit()
    {
        if (m_dic_selectSplitIds.Count == 0)
        {
            TipsManager.Instance.ShowTips("分解列表不能为空");
            return;
        }
        Dictionary<uint, uint>.Enumerator bse = m_dic_selectSplitIds.GetEnumerator();
        bool hasRareEquip = false;
        BaseItem baseItem = null;
        while(bse.MoveNext())
        {
            baseItem = imgr.GetBaseItemByQwThisId(bse.Current.Key);
            if (null != baseItem && baseItem.Grade >= ItemDefine.CONST_RARE_EQUIP_GRADE)
            {
                hasRareEquip = true;
                break;
            }
            
        }
        Action batchSplitAction = ()=>
            {
                emgr.SplitEquip(m_dic_selectSplitIds.Keys.ToList());
            };
        if (hasRareEquip)
        {
            string tips = "所选中包含须有装备，分解后将无法找回，是否继续分解？";
            TipsManager.Instance.ShowTipWindow(Client.TipWindowType.CancelOk, tips, batchSplitAction);
        }
        else
        {
            batchSplitAction.Invoke();
        }
        
    }

    private void SetAutoSelectEquip()
    {
        if (null != m_btn_AutoSelectEquip)
        {
            m_bool_autoSelectSplitEquip = !m_bool_autoSelectSplitEquip;
            m_btn_AutoSelectEquip.GetComponent<UIToggle>().value = m_bool_autoSelectSplitEquip;
            DoAutoSelectEquip();
        }
    }

    #endregion

    #region UIEvent
    void onClick_BackToPkg_Btn(GameObject caster)
    {
        bool resetTab = IsKnapsackStatus(KnapsackStatus.BatchSplit);
        SetKnapsackStatus(m_em_preStatus);
        if (resetTab)
        {
            SetKnapsackItemType(KnapsackItemType.ItemAll);
        }
    }

    void onClick_BtnDoSplit_Btn(GameObject caster)
    {
        DoBatchSplit();
    }

    void onClick_AutoSelectEquip_Btn(GameObject caster)
    {
        SetAutoSelectEquip();
    }
    #endregion
}