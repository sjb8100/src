/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Forging
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ComposePanel
 * 版本号：  V1.0.0.0
 * 创建时间：11/7/2016 10:21:45 AM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ComposePanel
{
    #region property
    //当前分类数据
    private List<uint> m_list_composeDatas = null;
    //当前活动的1级标签
    private uint m_uint_activeFType = 0;
    //当前活动的2级标签
    private uint m_uint_activeStype = 0;
    //当前选中的合成id
    private uint m_uint_composeId = 0;
    //1级页签
    private UISecondTabCreatorBase mSecondTabCreator = null;
    private List<uint> mlstFirstTabIds = null;
    //合成项生成器
    //合成管理器
    private ComposeManager m_mgr = null;
    private bool initWidgets = false;
    private UICurrencyGrid m_composeAllC = null;
    private UICurrencyGrid m_composeSingleC = null;
    private UIComposeNeedGrid m_targetNeedGrid = null;
    private UIComposeNeedGrid m_costNeedGrid1 = null;
    private UIComposeNeedGrid m_costNeedGrid2 = null;
    private UIComposeNeedGrid m_costNeedGrid3 = null;
    private UIComposeNeedGrid m_costNeedGrid4 = null;
    private UIComposeNeedGrid m_costNeedGrid5 = null;
    #endregion

    #region OverrideMethod
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        RegisterGlobalUIEvent(true);
        InitData();
    }

    protected override void OnJump(UIPanelBase.PanelJumpData jumpData)
    {
        if (null == jumpData)
        {
            jumpData = new PanelJumpData();
        }
        if (null != jumpData.Param && jumpData.Param is uint)
        {
            uint itemId = (uint)jumpData.Param;
            FocusComposeData(itemId);
        }else
        {
            SetSelectFirstType(0);
        }
    }

    public override PanelData GetPanelData()
    {
        PanelData pd = base.GetPanelData();
        pd.JumpData = new PanelJumpData();
        pd.JumpData.Param = m_uint_composeId;
        return pd;
    }

    protected override void OnHide()
    {
        base.OnHide();
        RegisterGlobalUIEvent(false);
        Release();
    }

    public override void Release(bool depthRelease = true)
    {
        base.Release(depthRelease);
        m_list_composeDatas = null;
        if (null != m_ctor_ComposeDatasScrollview)
        {
            m_ctor_ComposeDatasScrollview.Release();
        }

        if (null != m_targetNeedGrid)
        {
            m_targetNeedGrid.Release(false);
        }
        if (null != m_costNeedGrid1)
        {
            m_costNeedGrid1.Release(false);
        }
        if (null != m_costNeedGrid2)
        {
            m_costNeedGrid2.Release(false);
        }
        if (null != m_costNeedGrid3)
        {
            m_costNeedGrid3.Release(false);
        }
        if (null != m_costNeedGrid4)
        {
            m_costNeedGrid4.Release(false);
        }
        if (null != m_costNeedGrid5)
        {
            m_costNeedGrid5.Release(false);
        }

        if (null != m_composeAllC)
        {
            m_composeAllC.Release(false);
        }

        if (null != m_composeSingleC)
        {
            m_composeSingleC.Release(false);
        }
    }

    protected override void OnPanelBaseDestory()
    {
        base.OnPanelBaseDestory();
    }

   
    #endregion

    #region Init

    public void RegisterGlobalUIEvent(bool register)
    {
        if (register)
        {
            Engine.Utility.EventEngine.Instance().AddEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGolobalEventDlg);
        }else
        {
            Engine.Utility.EventEngine.Instance().RemoveEventListener((int)Client.GameEventID.UIEVENT_UPDATEITEM, OnGolobalEventDlg);
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitData()
    {
        InitWidgets();
        SetSelectFirstType(0, true);
        UpdatePanel();
    }

    private void OnGolobalEventDlg(int eventType, object data)
    {
        switch(eventType)
        {
            case (int)Client.GameEventID.UIEVENT_UPDATEITEM:
                UpdatePanel();
                break;
        }
    }

    /// <summary>
    /// 更新面板
    /// </summary>
    private void UpdatePanel()
    {

        if (null != m_list_composeDatas && null != m_ctor_ComposeDatasScrollview)
        {
            int preCount = (null != m_list_composeDatas) ? m_list_composeDatas.Count : 0;
            m_list_composeDatas.Clear();

            int curCount = 0;
            if (m_uint_activeFType == 0)
            {
                m_list_composeDatas.AddRange(m_mgr.GetCanComposeItemIds());
            }
            else
            {
                m_list_composeDatas.AddRange(
                    m_mgr.GetComposeDatas(m_uint_activeFType, m_uint_activeStype));
            }
            curCount = m_list_composeDatas.Count;
            int needAdd = curCount - preCount;
            if (needAdd < 0)
            {
                needAdd = Mathf.Abs(needAdd);
                //删除
                for (int i = 0; i < needAdd; i++)
                {
                    m_ctor_ComposeDatasScrollview.RemoveData(preCount - i - 1);
                }
            }
            else if (needAdd > 0)
            {
                //添加
                for (int i = 0; i < needAdd; i++)
                {
                    m_ctor_ComposeDatasScrollview.InsertData(preCount + i);
                }
            }
            m_ctor_ComposeDatasScrollview.UpdateActiveGridData();
        }
        
        UpdateCanComposeRedPoint();
        UpdateComposeUI();
    }

    /// <summary>
    /// 可合成装备红点提示
    /// </summary>
    private void UpdateCanComposeRedPoint()
    {
        UITypeGrid tGrid = null;
        if (null != mSecondTabCreator)
        {
            UICtrTypeGrid ctg = mSecondTabCreator.GetGrid<UICtrTypeGrid>(0);
            if (null != ctg)
            {
                List<uint> datas = m_mgr.GetCanComposeItemIds();
                ctg.SetRedPointStatus((null != datas && datas.Count != 0));
            }
        }
    }
    
    /// <summary>
    /// 初始化
    /// </summary>
    private void InitWidgets()
    {
        if (initWidgets)
        {
            return;
        }
        initWidgets = true;
        
        m_mgr = DataManager.Manager<ComposeManager>();
        Transform clone = null;
        if (null != m_trans_UIIFGTarget && null == m_targetNeedGrid)
        {
            clone = m_trans_UIIFGTarget.GetChild(0);
            if (null != clone)
            {
                m_targetNeedGrid = clone.GetComponent<UIComposeNeedGrid>();
                if (null == m_targetNeedGrid)
                {
                    m_targetNeedGrid = clone.gameObject.AddComponent<UIComposeNeedGrid>();
                }
            }
            m_targetNeedGrid.RegisterUIEventDelegate(OnGridEventDlg);
        }

        if (null != m_trans_UIIFGCost1 && null == m_costNeedGrid1)
        {
            clone = m_trans_UIIFGCost1.GetChild(0);
            if (null != clone)
            {
                Util.AddChildToTarget(m_trans_UIIFGCost1, clone);
                m_costNeedGrid1 = clone.GetComponent<UIComposeNeedGrid>();
                if (null == m_costNeedGrid1)
                {
                    m_costNeedGrid1 = clone.gameObject.AddComponent<UIComposeNeedGrid>();
                }
            }
            m_costNeedGrid1.RegisterUIEventDelegate(OnGridEventDlg);
        }

        if (null != m_trans_UIIFGCost2 && null == m_costNeedGrid2)
        {
            clone = m_trans_UIIFGCost2.GetChild(0);
            if (null != clone)
            {
                Util.AddChildToTarget(m_trans_UIIFGCost2, clone);
                m_costNeedGrid2 = clone.GetComponent<UIComposeNeedGrid>();
                if (null == m_costNeedGrid2)
                {
                    m_costNeedGrid2 = clone.gameObject.AddComponent<UIComposeNeedGrid>();
                }
            }
            m_costNeedGrid2.RegisterUIEventDelegate(OnGridEventDlg);
        }

        if (null != m_trans_UIIFGCost3 && null == m_costNeedGrid3)
        {
            clone = m_trans_UIIFGCost3.GetChild(0);
            if (null != clone)
            {
                Util.AddChildToTarget(m_trans_UIIFGCost3, clone);
                m_costNeedGrid3 = clone.GetComponent<UIComposeNeedGrid>();
                if (null == m_costNeedGrid3)
                {
                    m_costNeedGrid3 = clone.gameObject.AddComponent<UIComposeNeedGrid>();
                }
            }
            m_costNeedGrid3.RegisterUIEventDelegate(OnGridEventDlg);
        }

        if (null != m_trans_UIIFGCost4 && null == m_costNeedGrid4)
        {
            clone = m_trans_UIIFGCost4.GetChild(0);
            if (null != clone)
            {
                Util.AddChildToTarget(m_trans_UIIFGCost4, clone);
                m_costNeedGrid4 = clone.GetComponent<UIComposeNeedGrid>();
                if (null == m_costNeedGrid4)
                {
                    m_costNeedGrid4 = clone.gameObject.AddComponent<UIComposeNeedGrid>();
                }
            }
            m_costNeedGrid4.RegisterUIEventDelegate(OnGridEventDlg);
        }

        if (null != m_trans_UIIFGCost5 && null == m_costNeedGrid5)
        {
            clone = m_trans_UIIFGCost5.GetChild(0);
            if (null != clone)
            {
                Util.AddChildToTarget(m_trans_UIIFGCost5, clone);
                m_costNeedGrid5 = clone.GetComponent<UIComposeNeedGrid>();
                if (null == m_costNeedGrid5)
                {
                    m_costNeedGrid5 = clone.gameObject.AddComponent<UIComposeNeedGrid>();
                }
            }
            m_costNeedGrid5.RegisterUIEventDelegate(OnGridEventDlg);
        }
        
        if (null != m_trans_SingleCompoundCurrency && null == m_composeSingleC)
        {
            m_composeSingleC = m_trans_SingleCompoundCurrency.GetComponent<UICurrencyGrid>();
            if (null == m_composeSingleC)
            {
                m_composeSingleC = m_trans_SingleCompoundCurrency.gameObject.AddComponent<UICurrencyGrid>();
            }
        }
        if (null != m_trans_CompoundAllCurrency && null == m_composeAllC)
        {
            m_composeAllC = m_trans_CompoundAllCurrency.GetComponent<UICurrencyGrid>();
            if (null == m_composeAllC)
            {
                m_composeAllC = m_trans_CompoundAllCurrency.gameObject.AddComponent<UICurrencyGrid>();
            }
        }
        

       //构建1级2级分类页签
        if (null != m_scrollview_TypeScrollView && null == mSecondTabCreator)
        {
            mSecondTabCreator = m_scrollview_TypeScrollView.GetComponent<UISecondTabCreatorBase>();
            if (null != mSecondTabCreator)
            {
                GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
                GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uisecondtypegrid) as GameObject;
                mSecondTabCreator.Initialize<UISecondTypeGrid>(cloneFTemp, cloneSTemp
                    , OnGridUpdate, OnUpdateSecondTabGrid, OnGridEventDlg);
            }
        }

        if (null == mlstFirstTabIds)
        {
            mlstFirstTabIds = new List<uint>();
        }
        mlstFirstTabIds.Clear();

        List<CategoryTypeData> firstTabDatas = new List<CategoryTypeData>();
        firstTabDatas = m_mgr.GetCategoryTypeDatasByFirstType();
        if (null == firstTabDatas ||
            firstTabDatas.Count == 0)
        {
            Engine.Utility.Log.Error("ComposePanel InitWidgets Error,get cateoryTypeData null");
            return;
        }

        firstTabDatas.Sort((left, right) =>
            {
                return (int)left.m_uint_categoryId - (int)right.m_uint_categoryId;
            });
        List<int> secondTabsNums = new List<int>();
        CategoryTypeData ctData = null;
        int num = 0;
        for (int i = 0; i < firstTabDatas.Count; i++)
        {
            ctData = firstTabDatas[i];
            num = (null != ctData) ? ctData.Count : 0;
            secondTabsNums.Add(num);
            if (!mlstFirstTabIds.Contains(firstTabDatas[i].m_uint_categoryId))
            {
                mlstFirstTabIds.Add(firstTabDatas[i].m_uint_categoryId);
            }
        }

        if (null != mSecondTabCreator)
        {
            mSecondTabCreator.CreateGrids(secondTabsNums);
        }

        SetSelectFirstType(0, true);
        UpdateCanComposeRedPoint();
    }

    /// <summary>
    /// 设置选中第一分页
    /// </summary>
    /// <param name="type"></param>
    private void SetSelectFirstType(uint type,bool force = false)
    {
        if (null == mSecondTabCreator)
        {
            return;
        }
        if (m_uint_activeFType == type && !force)
        {
            mSecondTabCreator.DoToggle(mlstFirstTabIds.IndexOf(m_uint_activeFType), true, true);
            return;
        }
        m_uint_activeFType = type;
        mSecondTabCreator.Open(mlstFirstTabIds.IndexOf(m_uint_activeFType), true);
        CategoryTypeData ctd = m_mgr.GetCategoryTypeDataByType(CategoryTypeData.CategoryType.First, m_uint_activeFType);
        uint selectSecondsKey = 0;
        if (null != ctd && ctd.GetDatas().Count != 0)
        {
            selectSecondsKey = ctd.GetDatas()[0];
        }
        SetSelectSecondType(selectSecondsKey, m_uint_activeStype == 0);
    }

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

        CategoryTypeData ftd = m_mgr.GetCategoryTypeDataByType(CategoryTypeData.CategoryType.First, m_uint_activeFType);
        UISecondTypeGrid sGrid = null;
        if (null != ftd && m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), ftd.IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(false);
            }
        }
        
        m_uint_activeStype = type;
        if (null != ftd && m_uint_activeFType != 0)
        {
            sGrid = mSecondTabCreator.GetGrid<UISecondTypeGrid>(mlstFirstTabIds.IndexOf(m_uint_activeFType), ftd.IndexOf(m_uint_activeStype));
            if (null != sGrid)
            {
                sGrid.SetHightLight(true);
            }
        }
        BuildComposeList();
        UpdateComposeUI();
    }

    /// <summary>
    /// 设置选中id
    /// </summary>
    /// <param name="composeId"></param>
    private void SetSelectId(uint composeId)
    {
        if (m_uint_composeId == composeId || null == m_ctor_ComposeDatasScrollview)
        {
            return;
        }
        UIComposeGrid cGrid = (m_list_composeDatas.Contains(m_uint_composeId))
            ? m_ctor_ComposeDatasScrollview.GetGrid<UIComposeGrid>(m_list_composeDatas.IndexOf(m_uint_composeId)) : null;
        if (null != cGrid)
        {
            cGrid.SetHightLight(false);
        }
        m_uint_composeId = composeId;
        cGrid = (m_list_composeDatas.Contains(m_uint_composeId))
           ? m_ctor_ComposeDatasScrollview.GetGrid<UIComposeGrid>(m_list_composeDatas.IndexOf(m_uint_composeId)) : null;
        if (null != cGrid)
        {
            cGrid.SetHightLight(true);
        }
        UpdateComposeUI();
    }

    /// <summary>
    /// 获取分类格子
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private T GetTypeGrid<T>(bool firstType) where T: UIGridBase
    {
        T target = default(T);
        GameObject obj = null;
        GameObject resObj = null;
        if (firstType)
        {
            resObj = UIManager.GetResGameObj(GridID.Uitypegrid) as GameObject;
        }
        else
        {
            resObj = UIManager.GetResGameObj(GridID.Uisecondtypegrid) as GameObject;
        }
        if (null != resObj)
        {
            obj = Instantiate(resObj) as GameObject;
            if (null != obj)
            {
                target = obj.GetComponent<T>();
                if (null == target)
                {
                    target = obj.gameObject.AddComponent<T>();
                }
                UIDragScrollView scrollView = obj.GetComponent<UIDragScrollView>();
                if (null == scrollView)
                    scrollView = obj.AddComponent<UIDragScrollView>();
            }
        }
        return target;
    }

    #endregion

    #region Op

    /// <summary>
    /// shi
    /// </summary>
    /// <param name="composeId"></param>
    public void FocusComposeData(uint composeId)
    {
        table.ComposeDataBase curDB
            = GameTableManager.Instance.GetTableItem<table.ComposeDataBase>(composeId);
        if (null != curDB)
        {
            CategoryTypeData ctd = m_mgr.GetCategoryTypeDataByType(CategoryTypeData.CategoryType.First, curDB.fType);
            if (null != mSecondTabCreator && null != ctd)
            {
                uint secondType = m_mgr.BuildComposeSecondsKey(curDB.fType, curDB.sType);
                SetSelectFirstType(curDB.fType, true);
                SetSelectSecondType(secondType, true);
                mSecondTabCreator.FocusData(mlstFirstTabIds.IndexOf(curDB.fType), ctd.IndexOf(secondType));
            }
            SetSelectId(composeId);
        }
    }

    /// <summary>
    /// 格子更新
    /// </summary>
    /// <param name="gridBase"></param>
    /// <param name="index"></param>
    private void OnGridUpdate(UIGridBase gridBase,int index)
    {
        if (gridBase is UIComposeGrid)
        {
            UIComposeGrid cGrid = gridBase as UIComposeGrid;
            if (m_list_composeDatas.Count > index)
            {
                cGrid.SetGridData(m_list_composeDatas[index]);
                cGrid.SetHightLight(m_uint_composeId == m_list_composeDatas[index]);
            }
        }else if (gridBase is UICtrTypeGrid)
        {
            if (index < mlstFirstTabIds.Count)
            {
                UICtrTypeGrid ctg = gridBase as UICtrTypeGrid;
                CategoryTypeData fctd = null;
                if (index == 0)
                {
                    fctd = new CategoryTypeData(0, "可合成"); 
                    ctg.EnableArrow(false);
                }else
                {
                    ctg.SetRedPointStatus(false);
                    fctd = m_mgr.GetCategoryTypeDataByType(CategoryTypeData.CategoryType.First
                     , mlstFirstTabIds[index]);
                }
                if (null != fctd)
                {
                    ctg.SetData(fctd.m_uint_categoryId
                    , fctd.m_str_categoryName
                    , fctd.Count);
                }
            }
        }
    }

    /// <summary>
    /// 二级页签刷新回调
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="id"></param>
    /// <param name="index"></param>
    private void OnUpdateSecondTabGrid(UIGridBase grid ,object id,int index)
    {
        if (grid is UISecondTypeGrid)
        {
            UISecondTypeGrid sGrid = grid as UISecondTypeGrid;
            sGrid.SetRedPoint(false);
            uint cid = (uint)id;
            CategoryTypeData fctd = null;
            if (cid == 0)
            {
                fctd = new CategoryTypeData(0, "可合成");
            } else
            {
                fctd = m_mgr.GetCategoryTypeDataByType(CategoryTypeData.CategoryType.First
                     , cid);
            }
            CategoryTypeData sctd = m_mgr.GetCategoryTypeDataByType(CategoryTypeData.CategoryType.Second
                    , fctd.GetDataByIndex(index));
            if (null != sctd)
            {
                sGrid.SetData(sctd.m_uint_categoryId
                , sctd.m_str_categoryName
                , (m_uint_activeStype == sctd.m_uint_categoryId));
            }
        }
    }

    /// <summary>
    /// 格子事件委托
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnGridEventDlg(UIEventType eventType,object data,object param)
    {
        switch(eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UICtrTypeGrid)
                    {
                        UICtrTypeGrid tGrid = data as UICtrTypeGrid;
                        SetSelectFirstType((uint)tGrid.ID);
                    }else if (data is UISecondTypeGrid)
                    {
                        UISecondTypeGrid sGrid = data as UISecondTypeGrid;
                        SetSelectSecondType(sGrid.Data);
                    }else if (data is UIComposeGrid)
                    {
                        UIComposeGrid cGrid = data as UIComposeGrid;
                        SetSelectId(cGrid.Id);
                    }
                }
                break;

        }
    }

    /// <summary>
    /// 构建合成数据列表
    /// </summary>
    private void BuildComposeList()
    {
        StructData();
        BuildComposeUI();
    }

    /// <summary>
    /// 获取列表数据
    /// </summary>
    public void StructData()
    {
        if (null == m_list_composeDatas)
        {
            m_list_composeDatas = new List<uint>();
        }
        m_list_composeDatas.Clear();
        if (m_uint_activeFType == 0)
        {
            m_list_composeDatas.AddRange(
            DataManager.Manager<ComposeManager>().GetCanComposeItemIds());
        }else
        {
            m_list_composeDatas.AddRange(
            DataManager.Manager<ComposeManager>().GetComposeDatas(m_uint_activeFType, m_uint_activeStype));
        }
        
        m_uint_composeId = (m_list_composeDatas.Count != 0) ? m_list_composeDatas[0] : 0;
    }
    private bool m_bInitCreator = false;
    /// <summary>
    /// 构建UI
    /// </summary>
    private void BuildComposeUI()
    {
        if (!m_bInitCreator)
        {
            m_bInitCreator = true;
            if (null != m_ctor_ComposeDatasScrollview && null != m_trans_UIComposeGrid)
            {
                m_ctor_ComposeDatasScrollview.RefreshCheck();
                m_ctor_ComposeDatasScrollview.Initialize<UIComposeGrid>(m_trans_UIComposeGrid.gameObject
                    , OnGridUpdate, OnGridEventDlg);
            }
        }

        if (null != m_ctor_ComposeDatasScrollview)
        {
            m_ctor_ComposeDatasScrollview.CreateGrids(m_list_composeDatas.Count);
        }
    }
    
    /// <summary>
    /// 刷新
    /// </summary>
    private void UpdateComposeUI()
    {
        BaseItem composeItem = (m_uint_composeId != 0)
            ? DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(m_uint_composeId) : null;
        table.ComposeDataBase composeDb
            = GameTableManager.Instance.GetTableItem<table.ComposeDataBase>(m_uint_composeId);

        //更新材料
        uint composeNum = 0;
        BaseItem costItem = null;
        uint holdNum = 0;
        
        if (null != m_costNeedGrid1)
        {
            costItem = (null != composeDb && composeDb.costItem1 != 0 && composeDb.costNum1 != 0)
                ? DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(composeDb.costItem1) : null;
            if (null != costItem)
            {
                holdNum = (uint)DataManager.Manager<ItemManager>().GetItemNumByBaseId(costItem.BaseId);
                composeNum = holdNum / composeDb.costNum1;
                m_costNeedGrid1.SetGridViewData(false, true, composeDb.costItem1, composeDb.costNum1);
            }
            else
            {
                m_costNeedGrid1.SetGridViewData(true);
            }
        }
        if (null != m_costNeedGrid2)
        {
            costItem = (null != composeDb && composeDb.costItem2 != 0 && composeDb.costNum2 != 0)
                ? DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(composeDb.costItem2) : null;
            if (null != costItem)
            {
                holdNum = (uint)DataManager.Manager<ItemManager>().GetItemNumByBaseId(costItem.BaseId);
                uint tempNum = holdNum / composeDb.costNum2;
                if (composeNum > tempNum)
                {
                    composeNum = tempNum;
                }
                m_costNeedGrid2.SetGridViewData(false, true, composeDb.costItem2, composeDb.costNum2);
            }
            else
            {
                m_costNeedGrid2.SetGridViewData(true);
            }
        }
        if (null != m_costNeedGrid3)
        {
            costItem = (null != composeDb && composeDb.costItem3 != 0 && composeDb.costNum3 != 0)
                ? DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(composeDb.costItem3) : null;
            if (null != costItem)
            {
                holdNum = (uint)DataManager.Manager<ItemManager>().GetItemNumByBaseId(costItem.BaseId);
                uint tempNum = holdNum / composeDb.costNum3;
                if (composeNum > tempNum)
                {
                    composeNum = tempNum;
                }
                m_costNeedGrid3.SetGridViewData(false, true, composeDb.costItem3, composeDb.costNum3);
            }
            else
            {
                m_costNeedGrid3.SetGridViewData(true);
            }
        }
        if (null != m_costNeedGrid4)
        {
            costItem = (null != composeDb && composeDb.costItem4 != 0 && composeDb.costNum4 != 0)
                   ? DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(composeDb.costItem4) : null;
            if (null != costItem)
            {
                holdNum = (uint)DataManager.Manager<ItemManager>().GetItemNumByBaseId(costItem.BaseId);
                uint tempNum = holdNum / composeDb.costNum4;
                if (composeNum > tempNum)
                {
                    composeNum = tempNum;
                }
                m_costNeedGrid4.SetGridViewData(false, true, composeDb.costItem4, composeDb.costNum4);
            }
            else
            {
                m_costNeedGrid4.SetGridViewData(true);
            }

        }
        if (null != m_costNeedGrid5)
        {
            costItem = (null != composeDb && composeDb.costItem5 != 0 && composeDb.costNum5 != 0)
                ? DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(composeDb.costItem5) : null;
            if (null != costItem)
            {
                holdNum = (uint)DataManager.Manager<ItemManager>().GetItemNumByBaseId(costItem.BaseId);
                uint tempNum = holdNum / composeDb.costNum5;
                if (composeNum > tempNum)
                {
                    composeNum = tempNum;
                }
                m_costNeedGrid5.SetGridViewData(false, true, composeDb.costItem5, composeDb.costNum5);
            }
            else
            {
                m_costNeedGrid5.SetGridViewData(true);
            }
        }

        if (null != m_targetNeedGrid)
        {
            if (null != composeItem)
            {
                m_targetNeedGrid.SetGridViewData(false, false, composeItem.BaseId, composeNum);
            }
            else
            {
                m_targetNeedGrid.SetGridViewData(true);
            }
        }

        uint costType = (null != composeDb) ? composeDb.moneyType : (uint)GameCmd.MoneyType.MoneyType_Gold;
        uint composeCost = (null != composeDb) ? composeDb.costMoney : 0;
        //更新按钮
        UpdateCompseoBtnStatus(composeNum, costType, composeCost);

    }

    /// <summary>
    /// 更新按钮状态
    /// </summary>
    /// <param name="composeNum"></param>
    /// <param name="costType"></param>
    /// <param name="composeCost"></param>
    private void UpdateCompseoBtnStatus(uint composeNum,uint costType,uint composeCost)
    {
        bool enableCompose = (composeNum >= 1) ? true : false;
        //拥有金钱数量
        uint currencyHold = (uint)DataManager.Instance.GetCurrencyNumByType((GameCmd.MoneyType)costType);
        //是否足够
        uint composeNeedCost = (composeNum > 0) ? composeCost : 0;
        bool currencyEnough = (composeNeedCost < currencyHold);
        enableCompose = true;
        currencyEnough = true;
        if (null != m_btn_SingleCompound)
        {
            //m_btn_SingleCompound.isEnabled = enableCompose && currencyEnough;
            UILabel btnName = m_btn_SingleCompound.transform.Find("Label").GetComponent<UILabel>();
            //if (null != btnName)
            //{
            //    //ColorType cT = ColorType.JZRY_Txt_Black;
            //    //if (!enableCompose)
            //    //    cT = ColorType.Gray;
            //    //btnName.text = ColorManager.GetColorString(cT, "单次合成");
            //}
        }
        UICurrencyGrid currencyGird = m_trans_SingleCompoundCurrency.GetComponent<UICurrencyGrid>();
        if (null != currencyGird)
        {
            currencyGird.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                    MallDefine.GetCurrencyIconNameByType((GameCmd.MoneyType)costType),
                composeNeedCost
                ,(enableCompose && !currencyEnough) ? ColorType.Red : ColorType.JZRY_Txt_Black));
        }

        composeNeedCost = composeCost * composeNum;
        currencyEnough = (composeNeedCost < currencyHold);
        if (null != m_btn_CompounAll)
        {
            //m_btn_CompounAll.isEnabled = enableCompose && currencyEnough;
            UILabel btnName = m_btn_CompounAll.transform.Find("Label").GetComponent<UILabel>();
            //if (null != btnName)
            //{
            //    ColorType cT = ColorType.JZRY_Txt_Black;
            //    //if (!enableCompose)
            //    //    cT = ColorType.Gray;
            //    btnName.text = ColorManager.GetColorString(cT, "全部合成");
            //}
        }
       
        currencyGird = m_trans_CompoundAllCurrency.GetComponent<UICurrencyGrid>();
        if (null != currencyGird)
        {
            currencyGird.SetGridData(new UICurrencyGrid.UICurrencyGridData(
                    MallDefine.GetCurrencyIconNameByType((GameCmd.MoneyType)costType)
                    , composeNeedCost
                , (enableCompose && !currencyEnough) ? ColorType.Red : ColorType.JZRY_Txt_Black));
        }
    }

    #endregion

    #region UIEvent
    void onClick_SingleCompound_Btn(GameObject caster)
    {
        if (m_uint_composeId == 0)
        {
            TipsManager.Instance.ShowTips("请选择要合成的道具或装备");
            return;
        }
        m_mgr.DoCompose(m_uint_composeId, false);
    }

    void onClick_CompounAll_Btn(GameObject caster)
    {
        if (m_uint_composeId == 0)
        {
            TipsManager.Instance.ShowTips("请选择要合成的道具或装备");
            return;
        }
        m_mgr.DoCompose(m_uint_composeId, true);
    }
    #endregion
}