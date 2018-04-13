/************************************************************************************
 * Copyright (c) 2016  All Rights Reserved.
 * CLR版本： 4.0.30319.42000
 * 公司名称：ZQB
 * 命名空间：GameMain.Game.Forging
 * 创建人：  wenjunhua.zqgame
 * 文件名：  ForgingPanel_Inlay
 * 版本号：  V1.0.0.0
 * 创建时间：11/10/2016 3:49:02 PM
 * 描述：
 ************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

partial class ForgingPanel
{


    #region property
    //宝石镶嵌格子
    private Dictionary<EquipManager.EquipGridIndexType, UIGemInLayGrid> m_dic_inLayGrids = null;
    private Dictionary<EquipPartMode, UITabGrid> m_dic_EquipPartDic = null;
    //选中装备镶嵌格子
    private EquipManager.EquipGridIndexType selectEquipGridType = EquipManager.EquipGridIndexType.None;
    //部位列表生成器
    private Dictionary<EquipPartMode, List<GameCmd.EquipPos>> m_dicInlayParts = null;
    #endregion

    #region InitInlay
    private void InitInlayWidgets()
    {
        if (IsInitStatus(ForgingPanelMode.Inlay))
        {
            return;
        }
        SetInitStatus(ForgingPanelMode.Inlay, true);

        //初始化镶嵌格子
        if (null != m_trans_EquipGridInLayContent)
        {
            if (null == m_dic_inLayGrids)
            {
                m_dic_inLayGrids = new Dictionary<EquipManager.EquipGridIndexType, UIGemInLayGrid>();
            }
            m_dic_inLayGrids.Clear();
            Transform inLayTs = null;
            UIGemInLayGrid gemGrid = null;
            for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.First
            ; i < EquipManager.EquipGridIndexType.Max; i++)
            {
                inLayTs = Util.findTransform(m_trans_EquipGridInLayContent, i.ToString() + "InlayBox");
                if (null == inLayTs)
                {
                    continue;
                }
                gemGrid = inLayTs.GetComponent<UIGemInLayGrid>();
                if (null == gemGrid)
                {
                    gemGrid = inLayTs.gameObject.AddComponent<UIGemInLayGrid>();
                }
                gemGrid.SetGridData(i);
                gemGrid.RegisterUIEventDelegate(OnInlayGridUIEventDlg);
                if (!m_dic_inLayGrids.ContainsKey(i))
                {
                    m_dic_inLayGrids.Add(i, gemGrid);
                }
            }
        }

        InitCanInlayGemWidgets();
    }


    private void InitEquipPosWidgets()
    {
        if (null == m_dic_EquipPartDic)
        {
            m_dic_EquipPartDic = new Dictionary<EquipPartMode, UITabGrid>();
        }
        m_dic_EquipPartDic.Clear();

        StructEquipPartInlayData();

        //生成装备部位
        if (null != m_ctor_InlayScrollView && null != m_trans_UIEquipPosStatusGrid)
        {
            m_ctor_InlayScrollView.RefreshCheck();
            m_ctor_InlayScrollView.Initialize<UIEquipPosStatusGrid>(m_trans_UIEquipPosStatusGrid.gameObject
                , OnUpdateInlayGrid, OnInlayGridUIEventDlg);
        }

        //列表
        if (null == m_dic_EquipPartDic)
        {
            m_dic_EquipPartDic = new Dictionary<EquipPartMode, UITabGrid>();
        }
        m_dic_EquipPartDic.Clear();
        if (null == m_trans_PartTab)
        {
            Engine.Utility.Log.Error("ForgingPanel->InitForgingWidgets failed Transform ListTab null");
        }
        else
        {
            UITabGrid tabGrid = null;
            Transform ts = null;
            for (EquipPartMode i = EquipPartMode.AttackPart; i < EquipPartMode.Max; i++)
            {
                ts = m_trans_PartTab.Find(i.ToString() + "Tab");
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
                tabGrid.SetName(GetEquipPartModeName(i));
                tabGrid.SetHightLight(false);
                m_dic_EquipPartDic.Add(i, tabGrid);            
            }
        }
        RefreshAllTab();
        
    }
    void RefreshAllTab() 
    {
        if (m_dic_EquipPartDic == null)
        {
            return;
        }
        foreach (var i in m_dic_EquipPartDic)
        {
            RefreshTabRedPoint(i.Key, i.Value);
        }
    }
    void RefreshTabRedPoint(EquipPartMode mode,UITabGrid tab) 
    {
        bool value = false;
        if (IsPanelMode(ForgingPanelMode.Strengthen))
        {
            value =  DataManager.Manager<ForgingManager>().HaveEquipCanStrengthenByEquipPartMode(mode);
        }
        else if (IsPanelMode(ForgingPanelMode.Inlay))
        {
            value = DataManager.Manager<ForgingManager>().HaveEquipCanInlayByEquipPartMode(mode);
        }       
        tab.SetRedPointStatus(value);
    }

   
    private UISecondTabCreatorBase mSecondsTabCreator = null;
    private void InitCanInlayGemWidgets()
    {
        if (null == mSecondsTabCreator && null != m_scrollview_GemScrollView)
        {
            mSecondsTabCreator = m_scrollview_GemScrollView.GetComponent<UISecondTabCreatorBase>();
            if (null != mSecondsTabCreator)
            {
                GameObject cloneFTemp = UIManager.GetResGameObj(GridID.Uictrtypegrid) as GameObject;
                GameObject cloneSTemp = UIManager.GetResGameObj(GridID.Uigemgrid) as GameObject;
                mSecondsTabCreator.Initialize<UIGemGrid>(cloneFTemp, cloneSTemp
                    , OnUpdateInlayGrid, OnUpdateSecondGemGrid, OnInlayGridUIEventDlg);
            }
        }
    }
    #endregion

    #region EquipPart 装备部位
    //当前选中装备位置
    private GameCmd.EquipPos m_emSelectInlayPos = GameCmd.EquipPos.EquipPos_None;

    /// <summary>
    /// 构建装备部位镶嵌数据
    /// </summary>
    private void StructEquipPartInlayData()
    {
        if (null == m_dicInlayParts)
        {
            m_dicInlayParts = new Dictionary<EquipPartMode, List<GameCmd.EquipPos>>();
            EquipPartMode em = EquipPartMode.AttackPart;
            List<GameCmd.EquipPos> epsList = null;
            for (GameCmd.EquipPos i = GameCmd.EquipPos.EquipPos_Hat; i < GameCmd.EquipPos.EquipPos_Max; i++)
            {
                em = DataManager.Manager<ForgingManager>().GetEquipModeByEquipPos(i);
                if (em == EquipPartMode.None)
                {
                    continue;
                }

                if (!m_dicInlayParts.TryGetValue(em, out epsList))
                {
                    epsList = new List<GameCmd.EquipPos>();
                    m_dicInlayParts.Add(em, epsList);
                }

                if (!epsList.Contains(i))
                {
                    epsList.Add(i);
                }
            }
        }
    }


    /// <summary>
    /// 设置选中装备部位
    /// </summary>
    /// <param name="ep"></param>
    /// <param name="force"></param>
    private void SetSelectPart(GameCmd.EquipPos ep,bool force = false,bool needFocus = false)
    {
        if (null == m_ctor_InlayScrollView || null == m_dicInlayParts || (ep == m_emSelectInlayPos && !force))
        {
            return ;
        }
        List<GameCmd.EquipPos> eps = null;
        //刷新数据
        UIEquipPosStatusGrid grid = null;
        EquipPartMode epm = DataManager.Manager<ForgingManager>().GetEquipModeByEquipPos(m_emSelectInlayPos);
        if (m_dicInlayParts.TryGetValue(epm, out eps) && eps.Contains(m_emSelectInlayPos))
        {
            grid = m_ctor_InlayScrollView.GetGrid<UIEquipPosStatusGrid>(eps.IndexOf(m_emSelectInlayPos));
            if (null != grid)
            {
                grid.SetHightLight(false);
            }
        }
        epm = DataManager.Manager<ForgingManager>().GetEquipModeByEquipPos(ep);
        if (m_dicInlayParts.TryGetValue(epm, out eps) && eps.Contains(ep))
        {
            grid = m_ctor_InlayScrollView.GetGrid<UIEquipPosStatusGrid>(eps.IndexOf(ep));
            if (null != grid)
            {
                grid.SetHightLight(true);
            }
        }
        m_emSelectInlayPos = ep;
        if (needFocus && m_dicInlayParts.TryGetValue(epm, out eps))
        {
            m_ctor_InlayScrollView.FocusGrid(eps.IndexOf(ep));
        }
        if (IsPanelMode(ForgingPanelMode.Inlay))
        {
            //更新镶嵌数据
            UpdateInlayInfo();
            //构建宝石列表
            BuildGemList();
        }
        else if (IsPanelMode(ForgingPanelMode.Strengthen))
        {
            UpdateStrengthen();
        }

        RefreshBtnState();
    }
    void RefreshBtnState() 
    {
        bool enable = DataManager.Manager<ForgingManager>().JudgeEquipPosCanStrengthen(m_emSelectInlayPos);
        m_sprite_SingleStrengthenRedPoint.gameObject.SetActive(enable);
    }
    
    private EquipPartMode m_emEquipPartMode = EquipPartMode.None;
    /// <summary>
    /// 设置当前部位模式
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="force"></param>
    private void SetSelectEquipPartMode(EquipPartMode mode, bool force = false, bool defaultSelectData = true)
    {
        if (m_emEquipPartMode == mode && !force)
        {
            return;
        }
        UITabGrid tabGrid = null;
        if (null != m_dic_EquipPartDic && m_dic_EquipPartDic.TryGetValue(m_emEquipPartMode, out tabGrid))
        {
            tabGrid.SetHightLight(false);
        }

        if (null != m_dic_EquipPartDic && m_dic_EquipPartDic.TryGetValue(mode, out tabGrid))
        {
            tabGrid.SetHightLight(true);
        }
        m_emEquipPartMode = mode;
        BuildEquipPartList(defaultSelectData);
    }

    /// <summary>
    /// 生成不为列表
    /// </summary>
    private void BuildEquipPartList(bool defaultSelectData = true)
    {
        int count = 0;
        if (null != m_dicInlayParts && m_dicInlayParts.ContainsKey(m_emEquipPartMode))
        {
            count = m_dicInlayParts[m_emEquipPartMode].Count;
        }
        if (null != m_ctor_InlayScrollView)
        {
            m_ctor_InlayScrollView.CreateGrids(count);
        }
        if (defaultSelectData)
        {
            SetSelectPart(GetEquipPosByIndex(0), true);
        }
    }

    private GameCmd.EquipPos GetEquipPosByIndex(int index)
    {
        List<GameCmd.EquipPos> posList = null;
        if (m_dicInlayParts.TryGetValue(m_emEquipPartMode, out posList) && posList.Count > index)
        {
            return posList[index];
        }
        return GameCmd.EquipPos.EquipPos_None;
    }
    #endregion

    #region GemList 宝石列表
    //当前可镶嵌数据
    private Dictionary<GameCmd.GemType, GemInlayUpdateData> mdicCanInlayDatas = null;
    private List<GameCmd.GemType> mlstCanInlayGemType = null;
    //当前展开的宝石类型
    private GameCmd.GemType m_emActiveGemType = GameCmd.GemType.GemType_None;
    /// <summary>
    /// 构建宝石列表
    /// </summary>
    private void BuildGemList()
    {
        StructGemInlayDatas();
        BuildGemDataUI();
    }

    /// <summary>
    /// 构建宝石镶嵌数据
    /// </summary>
    private void StructGemInlayDatas()
    {
        //可镶嵌数据
        if (null == mdicCanInlayDatas)
        {
            mdicCanInlayDatas = new Dictionary<GameCmd.GemType, GemInlayUpdateData>();
        }
        mdicCanInlayDatas.Clear();

        //获取可镶嵌宝石列表
        if (null == mlstCanInlayGemType)
        {
            mlstCanInlayGemType = new List<GameCmd.GemType>();
        }
        mlstCanInlayGemType.Clear();



        mlstCanInlayGemType.AddRange(emgr.GeCanInlaytGemTypeByPos(m_emSelectInlayPos));

        //获取宝石列表
        List<uint> gemItemList = new List<uint>();
        List<uint> subTypes = new List<uint>();
        subTypes.Add((uint)ItemDefine.ItemMaterialSubType.Gem);
        gemItemList.AddRange(imgr.GetItemByType(GameCmd.ItemBaseType.ItemBaseType_Material, subTypes, GameCmd.PACKAGETYPE.PACKAGETYPE_MAIN));

        GemInlayUpdateData gemInlayUpdateData = null;
        for (int i = 0; i < mlstCanInlayGemType.Count; i++)
        {
            if (!mdicCanInlayDatas.TryGetValue(mlstCanInlayGemType[i], out gemInlayUpdateData))
            {
                gemInlayUpdateData = new GemInlayUpdateData(mlstCanInlayGemType[i]);
                mdicCanInlayDatas.Add(mlstCanInlayGemType[i], gemInlayUpdateData);
            }
            if (null != gemInlayUpdateData)
            {
                gemInlayUpdateData.StructData(gemItemList);
            }
        }
    }

    /// <summary>
    /// 生成宝石数据UI
    /// </summary>
    private void BuildGemDataUI()
    {
        m_emActiveGemType = GameCmd.GemType.GemType_None;
        if (null != mSecondsTabCreator)
        {
             //1、获取补一个不为空宝石类型
            GameCmd.GemType nextActiveGemType = GameCmd.GemType.GemType_None;
            List<int> secondTabDatas = new List<int>();
            UISecondTabCreatorBase.SecondsTabData data = null;
            GemInlayUpdateData updateData = null;
            for (int i = 0; i < mlstCanInlayGemType.Count;i++)
            {
                updateData = GetGemInlayUpdateData(mlstCanInlayGemType[i]);
                if (null == updateData)
                {
                    continue;
                }

                secondTabDatas.Add(updateData.Count);
                if (updateData.Count != 0
                    && nextActiveGemType == GameCmd.GemType.GemType_None)
                {
                    nextActiveGemType = mlstCanInlayGemType[i];
                }

            }
            mSecondsTabCreator.CreateGrids(secondTabDatas); 

            //2、展开当前活跃宝石列表，并生成UI
            SetActiveGemType(nextActiveGemType);
        }
    }

    /// <summary>
    /// 设置展开的宝石类型
    /// </summary>
    /// <param name="gType">目标宝石类型</param>
    /// <param name="instant">是否直接展开不需要过度</param>
    private void SetActiveGemType(GameCmd.GemType gType,bool instant = false)
    {
        if (gType == GameCmd.GemType.GemType_None)
        {
            return;
        }
        if (null != mSecondsTabCreator)
        {
            UICtrTypeGrid ctgGrid = mSecondsTabCreator.GetGrid<UICtrTypeGrid>(mlstCanInlayGemType.IndexOf(m_emActiveGemType));
            GemInlayUpdateData gemCurUpdateData = GetGemInlayUpdateData();
            GemInlayUpdateData gemNextUpdateData = GetGemInlayUpdateData(gType);
            if (m_emActiveGemType == gType)
            {
                if (!mSecondsTabCreator.IsOpen(mlstCanInlayGemType.IndexOf(m_emActiveGemType)) 
                    || gemCurUpdateData.Count == 0)
                {
                    mSecondsTabCreator.Close(mlstCanInlayGemType.IndexOf(m_emActiveGemType),true);
                    m_emActiveGemType = GameCmd.GemType.GemType_None;
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Gemstone_Commond_2);
                }
                else
                {
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Gemstone_Commond_1);
                }
                return;
            }
            if (null != gemCurUpdateData && (gemCurUpdateData.Count == 0 || gemNextUpdateData.Count != 0))
            {
                mSecondsTabCreator.Close(mlstCanInlayGemType.IndexOf(m_emActiveGemType),true);
            }
            if (null != gemNextUpdateData)
            {
                if (gemNextUpdateData.Count != 0)
                {
                    m_emActiveGemType = gType;
                    mSecondsTabCreator.Open(mlstCanInlayGemType.IndexOf(m_emActiveGemType), instant);
                }
                else
                {
                    TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Gemstone_Commond_2);
                }
            }
        }

        
    }


    /// <summary>
    /// 获取当前活跃的宝石控制格子
    /// </summary>
    /// <returns></returns>
    private GemInlayUpdateData GetGemInlayUpdateData()
    {
        return GetGemInlayUpdateData(m_emActiveGemType);
    }

    /// <summary>
    /// 获取的宝石控制格子
    /// </summary>
    /// <param name="gemType"></param>
    /// <returns></returns>
    private GemInlayUpdateData GetGemInlayUpdateData(GameCmd.GemType gemType)
    {
        return mdicCanInlayDatas.ContainsKey(gemType) ? mdicCanInlayDatas[gemType] : null;
    }

    #endregion

    #region 更新信息

    /// <summary>
    /// 物品信息变更
    /// </summary>
    /// <param name="passData"></param>
    private void OnInlayItemUpdate(ItemDefine.UpdateItemPassData passData)
    {
        BaseItem baseItem = DataManager.Manager<ItemManager>().GetTempBaseItemByBaseID<BaseItem>(passData.BaseId);
        if (baseItem.IsGem || baseItem.IsForgingEquip)
        {
            UpdateInlayPartList();
            UpdateInlayInfo();
            if (baseItem.IsGem)
            {
                Gem gem = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<Gem>(passData.BaseId, ItemDefine.ItemDataType.Gem);
                GemInlayUpdateData gemInlayUpdateData = null;
                if (mlstCanInlayGemType.Contains(gem.Type)
                    && mdicCanInlayDatas.TryGetValue(gem.Type, out gemInlayUpdateData))
                {
                    int firstIndex = mlstCanInlayGemType.IndexOf(gem.Type);
                    int secondIndex = -1;
                    GemInlayUpdateData.GemInlayUpdateType updateType = gemInlayUpdateData.UpdateItem(passData.BaseId,out secondIndex);
                    if (null != mSecondsTabCreator)
                    {
                        if (updateType == GemInlayUpdateData.GemInlayUpdateType.GIUT_Insert)
                        {
                            //数据新增
                            mSecondsTabCreator.InsertIndex(firstIndex, secondIndex);
                        }else if (updateType == GemInlayUpdateData.GemInlayUpdateType.GIUT_Remove)
                        {
                            //数据删除
                            mSecondsTabCreator.RemoveIndex(firstIndex, secondIndex);
                        }else if (updateType == GemInlayUpdateData.GemInlayUpdateType.GIUT_Update)
                        {
                            //数据更新
                            mSecondsTabCreator.UpdateIndex(firstIndex, secondIndex);
                        }
                    }
                    
                }
            }
        }
    }

    /// <summary>
    /// 更新镶嵌部位列表
    /// </summary>
    private void UpdateInlayPartList()
    {
        if (null != m_ctor_InlayScrollView)
        {
            m_ctor_InlayScrollView.UpdateActiveGridData();
        }
    }

    /// <summary>
    /// 更新镶嵌信息
    /// </summary>
    private void UpdateInlayInfo()
    {
        UIGemInLayGrid inLayGrid = null;
        int index = 0;
        StringBuilder builder = new StringBuilder();
        uint inlayBaseId = 0;
        Gem gem = null;
        for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.First; i < EquipManager.EquipGridIndexType.Max; i++)
        {
            index = (int)i - 1;
            if (i < 0)
                continue;
            if (null != m_dic_inLayGrids && m_dic_inLayGrids.TryGetValue(i, out inLayGrid))
            {
                inLayGrid.UpdateGridData(m_emSelectInlayPos);
            }

            if (emgr.TryGetEquipGridInlayGem(m_emSelectInlayPos, i, out inlayBaseId))
            {
                gem = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<Gem>(inlayBaseId, ItemDefine.ItemDataType.Gem);
                builder.Append(DataManager.Manager<TextManager>().GetLocalFormatText(GetEquipGridIndexTextType(i), gem.AttrDes) + "\n");
            }
        }

        if (null != m_label_InlayGemAttrDes)
        {
            m_label_InlayGemAttrDes.text = builder.ToString();
        }
    }

    private LocalTextType GetEquipGridIndexTextType(EquipManager.EquipGridIndexType index)
    {
        LocalTextType tt = LocalTextType.LocalText_None;
        switch(index)
        {
            case EquipManager.EquipGridIndexType.First:
                tt = LocalTextType.Local_Txt_Set_1;
                break;
            case EquipManager.EquipGridIndexType.Second:
                tt = LocalTextType.Local_Txt_Set_2;
                break;
            case EquipManager.EquipGridIndexType.Third:
                tt = LocalTextType.Local_Txt_Set_3;
                break;
        }
        return tt;
    }

    #endregion

    #region GridCallback
    /// <summary>
    /// 镶嵌格子数据刷新回调
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="index"></param>
    private void OnUpdateInlayGrid(UIGridBase grid,int index)
    {
        if (grid is UICtrTypeGrid)
        {
            UICtrTypeGrid ctg = grid as UICtrTypeGrid;
            if (mlstCanInlayGemType.Count > index)
            {
                GameCmd.GemType gemType = mlstCanInlayGemType[index];
                
                GemInlayUpdateData updateData =GetGemInlayUpdateData(gemType);
                int num = (null != updateData) ? updateData.Count : 0; 
                ctg.SetData(gemType, DataManager.Manager<TextManager>().GetGemNameByType(gemType), num);
                ctg.SetRedPointStatus(DataManager.Manager<ForgingManager>().HaveGemCanImprove(m_emSelectInlayPos, gemType));
            }
        }
        else if (grid is UIEquipPosStatusGrid)
        {
            UIEquipPosStatusGrid posGrid = grid as UIEquipPosStatusGrid;
            GameCmd.EquipPos pos = GetEquipPosByIndex(index);
            bool isInlay = IsPanelMode(ForgingPanelMode.Inlay);
            posGrid.SetGridViewData(pos,isInlay);
            posGrid.SetHightLight(pos == m_emSelectInlayPos);
            if (isInlay)
            {
                Gem gem = null;
                uint inLayGemBaseId = 0;
                string inlayGemIconName = "";
                bool inlay = false;
                bool unlock = false;
                for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.First; i < EquipManager.EquipGridIndexType.Max; i++)
                {
                    inlayGemIconName = "";
                    inlay = false;
                    unlock = false;
                    if (i < 0)
                        continue;

                    if (emgr.TryGetEquipGridInlayGem(pos, i, out inLayGemBaseId))
                    {
                        gem = DataManager.Manager<ItemManager>()
                    .GetTempBaseItemByBaseID<Gem>(inLayGemBaseId, ItemDefine.ItemDataType.Gem);
                        inlayGemIconName = gem.Icon;
                        unlock = true;
                        inlay = true;
                    }
                    else if (emgr.IsUnlockEquipGridIndex(i))
                    {
                        unlock = true;
                    }
                    posGrid.SetInlayIcon(i, unlock, inlay, inlayGemIconName);
                }
            }
        }
    }

    /// <summary>
    /// 宝石二级页签数据刷新
    /// </summary>
    /// <param name="gridBase"></param>
    /// <param name="id"></param>
    /// <param name="index"></param>
    private void OnUpdateSecondGemGrid(UIGridBase gridBase, object id,int index)
    {
        if (gridBase is UIGemGrid  && id is GameCmd.GemType)
        {
            GameCmd.GemType gemType = (GameCmd.GemType)id;
            GemInlayUpdateData data = GetGemInlayUpdateData(gemType);
            uint gembaseId = 0;
            if (null != data && data.TryGetBaseIdByIndex(index,out gembaseId))
            {
                UIGemGrid gemGrid = gridBase as UIGemGrid;
                gemGrid.SetGridData(gembaseId);
                bool show = DataManager.Manager<ForgingManager>().HaveGemCanImprove(m_emSelectInlayPos, gemType)
                    && (gembaseId == data.PrefectGemID && data.PrefectGemID != 0);
                gemGrid.SetRedPointStatus(show);
            }
            

        }
    }

    /// <summary>
    /// 镶嵌格子数据UI事件回调
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="data"></param>
    /// <param name="param"></param>
    private void OnInlayGridUIEventDlg(UIEventType eventType,object data,object param)
    {
        switch(eventType)
        {
            case UIEventType.Click:
                {
                    if (data is UICtrTypeGrid)
                    {
                        UICtrTypeGrid ctg = data as UICtrTypeGrid;
                        SetActiveGemType((GameCmd.GemType)ctg.ID);
                    }
                    else if (data is UIGemGrid)
                    {
                        UIGemGrid gGRid = data as UIGemGrid;
                        //镶嵌
                        DoInlay(gGRid.Data);
                    }
                    else if (data is UIEquipPosStatusGrid)
                    {
                        UIEquipPosStatusGrid epsGrid = data as UIEquipPosStatusGrid;
                        SetSelectPart(epsGrid.Data);
                    }
                    else if (data is UIGemInLayGrid)
                    {
                        UIGemInLayGrid gInlayGrid = data as UIGemInLayGrid;
                        DoUnload(gInlayGrid.Data);
                    }
                }
                break;
        }
    }
    #endregion

    #region Op

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private bool IsSelectInlayGridIndexType(EquipManager.EquipGridIndexType type)
    {
        return type == selectEquipGridType; 
    }
    /// <summary>
    /// 设置选中镶嵌格子
    /// </summary>
    /// <param name="type"></param>
    private void SelectInlayGridIndexType(EquipManager.EquipGridIndexType type,bool force = false)
    {
        if (type == selectEquipGridType && !force)
        {
            return;
        }
        UIGemInLayGrid grid = null;
        if (null != m_dic_inLayGrids && m_dic_inLayGrids.TryGetValue(selectEquipGridType, out grid))
        {
            grid.SetHightLight(false);
        }
        selectEquipGridType = type;
        if (null != m_dic_inLayGrids && m_dic_inLayGrids.TryGetValue(selectEquipGridType, out grid))
        {
            grid.SetHightLight(true);
        }
    }
    /// <summary>
    /// 获取装备部位分类的名称
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private string GetEquipPartModeName(EquipPartMode mode)
    {
        LocalTextType key = LocalTextType.LocalText_None;
        switch(mode)
        {
            case EquipPartMode.AttackPart:
                key = LocalTextType.Local_TXT_Attack;
                break;
            case EquipPartMode.DefendPart:
                key = LocalTextType.Local_TXT_Defend;
                break;
        }
        return DataManager.Manager<TextManager>().GetLocalText(key);
    }

    /// <summary>
    /// 卸下
    /// </summary>
    /// <param name="index"></param>
    private void DoUnload(EquipManager.EquipGridIndexType index)
    {
        uint inlayBaseId = 0;
        if (!emgr.IsUnlockEquipGridIndex(index))
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Gemstone_Commond_3, emgr.GetEquipGridUnlockLevel(index));
        }else if (emgr.TryGetEquipGridInlayGem(m_emSelectInlayPos, index, out inlayBaseId))
        {
            //卸下
            emgr.GemUnload(m_emSelectInlayPos, index);
        }else
        {
            GemInlayUpdateData data =  GetGemInlayUpdateData();
            uint canInlayBaseId = 0;
            if (null != data && data.TryGetCanInlayGem(out canInlayBaseId))
            {
                DoInlay(canInlayBaseId, index);
            }else
            {
                TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Gemstone_Commond_4);
            }
        }
        
    }

    /// <summary>
    /// 镶嵌
    /// </summary>
    /// <param name="gemBaseId"></param>
    private void DoInlay(uint gemBaseId, EquipManager.EquipGridIndexType index = EquipManager.EquipGridIndexType.None)
    {
        EquipManager.EquipGridIndexType inlayGridIndex = EquipManager.EquipGridIndexType.None;
        if (index == EquipManager.EquipGridIndexType.None)
        {
            for (EquipManager.EquipGridIndexType i = EquipManager.EquipGridIndexType.First; i < EquipManager.EquipGridIndexType.Max; i++)
            {
                if (emgr.IsEquipGridIndexEmpty(m_emSelectInlayPos, i))
                {
                    inlayGridIndex = i;
                    break;
                }
            }
            
        }else
        {
            inlayGridIndex = index;
        }

        if (inlayGridIndex == EquipManager.EquipGridIndexType.None || !emgr.IsUnlockEquipGridIndex(inlayGridIndex))
        {
            TipsManager.Instance.ShowLocalFormatTips(LocalTextType.Gemstone_Commond_5);
            return;
        }

        if (inlayGridIndex != EquipManager.EquipGridIndexType.None)
        {
            emgr.GemInlay(m_emSelectInlayPos, inlayGridIndex, gemBaseId);
        }
    }
    #endregion

    #region UIEvent
    void onClick_BtnItemCompound_Btn(GameObject caster)
    {
        DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.ComposePanel);
    }

    void onClick_BtnItemBuy_Btn(GameObject caster)
    {
        {
            //点击获取格子
            UIPanelBase.PanelJumpData jumpdata = new PanelJumpData();
            ReturnBackUIMsg msg = new ReturnBackUIMsg();
            jumpdata.Tabs = new int[2];
            jumpdata.Tabs[0] = (int)GameCmd.CommonStore.CommonStore_One;
            jumpdata.Tabs[1] = 3;
            DataManager.Manager<UIPanelManager>().ShowPanel(PanelID.MallPanel,jumpData:jumpdata);
        }
    }
    #endregion
} 